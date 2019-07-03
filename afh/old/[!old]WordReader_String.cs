namespace afh.Parse{
	//-------------------------------------------------
	//		�����񃊃e����
	//		���K�\�����e����
	//-------------------------------------------------
	//		"([^"\\]|\[^\r\n])*"
	//		'([^'\\]|\[^\r\n])*'
	//		@"([^"]|"")*"
	//		
	//		/([^/\\]|\[^\r\n])*/[igm]*
	//	����:
	//		������:***;�ʒu:***:
	//		�Ŏw�肳�������𖞂�����ԂŌĂяo����
	//-------------------------------------------------
	public abstract partial class AbstractWordReader{
		/// <summary>
		/// double quotation �ň͂܂ꂽ��������擾���܂��B
		/// ���s�͋�����Ă��܂���B
		/// ���݈ʒu�� " ���݂��ԂŌĂяo���ĉ������B
		/// </summary>
		protected void ReadStringDQ(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '"':
						[add][nexit];
					case '\\':skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		err:[error:"������ɏI�[�� \" �����݂��܂���B"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '"':
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;;
					case '\\':skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError("������ɏI�[�� \" �����݂��܂���B",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// single quotation �ň͂܂ꂽ��������擾���܂��B
		/// ���s�͋�����Ă��܂���B
		/// ���݈ʒu�� ' ���݂��ԂŌĂяo���ĉ������B
		/// </summary>
		protected void ReadStringSQ(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '\'':
						[add][nexit];
					case '\\':
						skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		err:[error:"������ɏI�[�� ' �����݂��܂���B"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '\'':
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;;
					case '\\':
						skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError("������ɏI�[�� ' �����݂��܂���B",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// �����s��������擾���܂��B
		/// �ʒu: '"' �̈ʒu;
		/// ������: "@";
		/// </summary>
		protected void ReadStringMulti(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			while(true)switch([letter]){
				case '"':
					[add][next]
					if([not:"])return;
					goto default;
				default:
					[add][if!next]goto err;
					break;
			}
		err:[error:"������ɏI�[�� \" �����݂��܂���B"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			while(true)switch(this.lreader.CurrentLetter){
				case '"':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!='"')return;
					goto default;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		err:this.lreader.SetError("������ɏI�[�� \" �����݂��܂���B",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// ���K�\�����e�������擾���܂��B
		/// </summary>
		protected void ReadRegExp(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '/':
						[add][next];
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		suffix:
			while(true)switch([letter]){
				case 'g':case 'i':case 'm':
					[add][next]
					break;
				default:
					return;
			}

		err:[error:"���K�\�����e�����ɏI�[�� / �����݂��܂���B"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '/':
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;;
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		suffix:
			while(true)switch(this.lreader.CurrentLetter){
				case 'g':case 'i':case 'm':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				default:
					return;
			}

		err:this.lreader.SetError("���K�\�����e�����ɏI�[�� / �����݂��܂���B",0,null);
			#endregion #OUT#
		}
	}
}