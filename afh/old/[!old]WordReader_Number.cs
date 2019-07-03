namespace afh.Parse{
	//-------------------------------------------------
	//		���l���e����
	//-------------------------------------------------
	//		0[xX][0-9a-fA-F]+
	//		(\d*\.)?\d+([eE][+-]?\d+)?[fF]?
	//		\d+[uU]?[lL]?
	//	����:
	//		������:***;�ʒu:***:
	//		�Ŏw�肳�������𖞂�����ԂŌĂяo����
	//-------------------------------------------------
	public abstract partial class AbstractWordReader{
		/// <summary>
		/// ���l���e������ǂݎ��܂��B
		/// ������: "";
		/// �ʒu: \d;
		/// �� '.' �̎��ɐ��������鎞�݈̂ʒu�� '.' �Ƃ��ČĂяo���\�ł��B
		/// </summary>
		/// <remarks>
		/// �z�肳���Ăяo���󋵂́A
		/// �@�P��̈ꕶ���ڂɐ������������ꍇ
		/// �A�P��̈ꕶ���ڂ� '.' ������w���̎��ɐ������m�F�����x�ꍇ
		/// �@(���̏ꍇ�� MoveToPos(0) �����s���Ĉʒu�� '.' �ɍ��킹�ĉ�����)
		/// </remarks>
		protected void ReadNumber(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			if([is:0]){
				[add][next]
				if([is:x]||[is:X]){
					ReadNumber_x();
					return;
				}
			}
			ReadNumber_f();
#endif
			#region #OUT#
			if(this.lreader.CurrentLetter=='0'){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if(this.lreader.CurrentLetter=='x'||this.lreader.CurrentLetter=='X'){
					ReadNumber_x();
					return;
				}
			}
			ReadNumber_f();
			#endregion #OUT#
		}
		/// <summary>
		/// 16 �i����ǂݎ��܂��B
		/// ������: "0";
		/// �ʒu: 'x' | 'X';
		/// </summary>
		private void ReadNumber_x(){
#if MACRO_WORDREADER
			[add][if!next]goto err;

			// [0-9a-fA-F]+
			if(!([is:0-9]||[is:a-f]||[is:A-F]))goto err;
			do{
				[add][next]
			}while([is:0-9]||[is:a-f]||[is:A-F]);
			return;

		err:[error:"0x �̎��ɂ� [0-9a-fA-F]+ ������ׂ��ł�"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [0-9a-fA-F]+
			if(!('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'||'a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f'||'A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F'))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'||'a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f'||'A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F');
			return;

		err:this.lreader.SetError("0x �̎��ɂ� [0-9a-fA-F]+ ������ׂ��ł�",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// �ʏ�̐���ǂݎ��܂��B
		/// ������: "0" | "";
		/// �ʒu: ������Ɏ��ɒǉ������ł��낤����;
		/// ����: ".a" ���̕������^�����Ă��ꕶ�����ǂݎ��܂���B�������[�v�ɂȂ�܂��B
		/// </summary>
		private void ReadNumber_f(){
#if MACRO_WORDREADER
			bool dot=false;
			while(true)switch([letter]){
				case '0':case '1':case '2':case '3':case '4':
				case '5':case '6':case '7':case '8':case '9':
					goto add;
				case '.':
					// ���o��������~
					if(dot)goto default;
					// ��ǂ� 0-9 �������灛
					lreader.StoreCurrentPos(1);
					[next]
					if([is:0-9]){
						this.cword+='.';
						dot=true;
						goto add;
					}
					lreader.MoveToPos(1);
					goto default;
				case 'e':case 'E':
					ReadNumber_e();
					if([is:f]||[is:F])goto case 'f';
					return;
				case 'f':case 'F':
					[add][nexit]
				case 'u':case 'U':
					if(dot)goto default;
					[add][next]
					if([is:l]||[is:L])goto case 'l';
					return;
				case 'l':case 'L':
					if(dot)goto default;
					[add][nexit]
				default:
					return;
				add:
					[add][next]
					break;
			}
#endif
			#region #OUT#
			bool dot=false;
			while(true)switch(this.lreader.CurrentLetter){
				case '0':case '1':case '2':case '3':case '4':
				case '5':case '6':case '7':case '8':case '9':
					goto add;
				case '.':
					// ���o��������~
					if(dot)goto default;
					// ��ǂ� 0-9 �������灛
					lreader.StoreCurrentPos(1);
					if(!this.lreader.MoveNext())return;
					if('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'){
						this.cword+='.';
						dot=true;
						goto add;
					}
					lreader.MoveToPos(1);
					goto default;
				case 'e':case 'E':
					ReadNumber_e();
					if(this.lreader.CurrentLetter=='f'||this.lreader.CurrentLetter=='F')goto case 'f';
					return;
				case 'f':case 'F':
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case 'u':case 'U':
					if(dot)goto default;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter=='l'||this.lreader.CurrentLetter=='L')goto case 'l';
					return;
				case 'l':case 'L':
					if(dot)goto default;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				default:
					return;
				add:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// ���������_�̎w������ǂݎ��܂��B
		/// ������: ���ꖘ�ɓǂ񂾐�;
		/// �ʒu: 'e' | 'E'
		/// </summary>
		private void ReadNumber_e(){
#if MACRO_WORDREADER
			[add][if!next]goto err;

			// [+-]?
			if([is:-]||[is:+]){
				[add][if!next]goto err;
			}

			// \d+
			if(!([is:0-9]))goto err;
			do{
				[add][next]
			}while([is:0-9]);

			return;

		err:[error:"�w���\�L�̌�ɂ� [+-]?\\d+ ���K�v�ł��B"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [+-]?
			if(this.lreader.CurrentLetter=='-'||this.lreader.CurrentLetter=='+'){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			}

			// \d+
			if(!('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9');

			return;

		err:this.lreader.SetError("�w���\�L�̌�ɂ� [+-]?\\d+ ���K�v�ł��B",0,null);
			#endregion #OUT#

		}
	}
}