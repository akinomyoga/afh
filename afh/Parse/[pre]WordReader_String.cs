namespace afh.Parse {
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
	public abstract partial class AbstractWordReader {
		/// <summary>
		/// double quotation �ň͂܂ꂽ��������擾���܂��B
		/// ���s�͋�����Ă��܂���B
		/// ���݈ʒu�� " ���݂��ԂŌĂяo���ĉ������B
		/// </summary>
		protected void ReadStringDQ() {
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '"':
						add;nexit;
					case '\\':skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		err:error("������ɏI�[�� \" �����݂��܂���B");
		}
		/// <summary>
		/// single quotation �ň͂܂ꂽ��������擾���܂��B
		/// ���s�͋�����Ă��܂���B
		/// ���݈ʒu�� ' ���݂��ԂŌĂяo���ĉ������B
		/// </summary>
		protected void ReadStringSQ() {
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '\'':
						add;nexit;
					case '\\':
						skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		err:error("������ɏI�[�� ' �����݂��܂���B");
		}
		/// <summary>
		/// �����s��������擾���܂��B
		/// �ʒu: '"' �̈ʒu;
		/// ������: "@";
		/// </summary>
		protected void ReadStringMulti() {
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			while(true)switch(letter){
				case '"':
					add;next;
					if("not:\"")return;
					goto default;
				default:
					add;if(!next)goto err;
					break;
			}
		err:error("������ɏI�[�� \" �����݂��܂���B");
		}
		/// <summary>
		/// ���K�\�����e�������擾���܂��B
		/// </summary>
		protected void ReadRegExp(){
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '/':
						add;next;
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		suffix:
			while(true)switch(letter){
				case 'g':case 'i':case 'm':
					add;next;
					break;
				default:
					return;
			}

		err: error("���K�\�����e�����ɏI�[�� / �����݂��܂���B");
		}
	}
}