using afh.Parse;

namespace afh.Preprocessor{
	internal sealed partial class CppWordReader:AbstractWordReader{
		private void readOperator(){
			this.wtype=WordType.Operator;
			char c;
			switch(this.lreader.CurrentLetter){
				case '=':case '!':
					// o o=
					add;next;
					goto aft_equal;
				case ':':
					// : ::
					add;next;
					if("is::"){add;next;}
					return;
				case '-':
					// - -- -= -> ->*
					add;next;
					if("is:-"||"is:="){
						add;next;
					}else if("is:>"){
						add;next;
						if("is:*"){add;next;}
					}
					return;
				case '+':
					// + ++ +=
					c=letter;
					add;next;
					if(letter!=c)goto aft_equal;
					add;nexit;
				case '&':case '|':case '<':case '>':
					// o oo o= oo=
					c=letter;
					add;next;
					if(letter!=c)goto aft_equal;
					add;next;
					goto aft_equal;
				case '*':case '%':case '^':
					// o o=
					add;next;
					goto aft_equal;
				case '/':
					add;next;
					if("is:*"){
						add;if(!next){
							error("�R�����g�ɏI�[ */ ������܂���B");
							return;
						}
						readMultiComment();return;
					}else if("is:/"){
						add;next;
						readLineComment();return;
					}else goto aft_equal;
				case '"':ReadStringDQ();return;
				case '\'':ReadStringSQ();return;
				case '.':
					// . .. ... .*
					add;next;
					if("is:0-9"){
						lreader.MoveToPos(0);
						this.cword="";
						readNumber();
						return;
					}
					if("is:*"){add;next;}
					if("not:.")return;
					add;next;
					if("not:.")return;
					add;nexit;		// ... ���Z�q
				case '\\':
					if(tryReadUCN()){
						cword="";
						lreader.MoveToPos(0);
						readIdentifier();
					}
					goto default;
				case '#':
					add;next;

					if(mode==Mode.MacroContent){
						if("is:#"||"is:@"||"is:&"){
							wtype=WordType.Operator;
							add;nexit;
						}
					}

					wtype=WordType.Tag;
					readLineComment();
					wtype=WordType.Tag;
					return;
				case '@':
					wtype=WordType.KeyWord;
					add;next;
					readLineComment();
					wtype=WordType.KeyWord;
					return;
				case ',':case '?':
				case '[':case ']':
				case '(':case ')':
				case '{':case '}':
				default:
					add;nexit;
				aft_equal:
					if("not:=")return;
					add;nexit;
			}
		}
		//============================================================
		//		���l���e����
		//============================================================
		private void readNumber(){
			wtype=WordType.Literal;
			if("is:0"){
				add;next;
				if("is:x"||"is:X"){
					readNumber_x();
					return;
				}
			}
			readNumber_f();
		}

		private void readNumber_x(){
			add;if(!next)goto err;

			// [0-9a-fA-F]+
			if(!("is:0-9"||"is:a-f"||"is:A-F"))goto err;
			do{
				add;next;
			}while("is:0-9"||"is:a-f"||"is:A-F");
			return;

		err:error("0x �̎��ɂ� [0-9a-fA-F]+ ������ׂ��ł�");
		}

		private void readNumber_f(){
			bool dot=false;
			while(true)switch(letter){
				case '0':case '1':case '2':case '3':case '4':
				case '5':case '6':case '7':case '8':case '9':
					goto _add;
				case '.':
					// ���o��������~
					if(dot)goto default;
					// ��ǂ� 0-9 �������灛
					lreader.StoreCurrentPos(1);
					next;
					if("is:0-9"){
						this.cword+='.';
						dot=true;
						goto _add;
					}
					lreader.MoveToPos(1);
					goto default;
				case 'e':case 'E':
					readNumber_e();
					if("is:f"||"is:F")goto case 'f';
					return;
				default:
					return;
				_add:
					add;next;
					break;
				//----------------------------------------------------
				//		Prefixes
				//----------------------------------------------------
				// prefix: f
				case 'f':case 'F':
					add;nexit;
				// prefix: i64
				case 'i':
					if(dot)goto default;
					lreader.StoreCurrentPos(1);
					do{
						if(!next)break;
						if("not:6")break;
						if(!next)break;
						if("not:4")break;
						cword+="i64";
						nexit;
#pragma warning disable 162
					}while(false);
#pragma warning restore 162
					lreader.MoveToPos(1);
					goto default;
				// prefix: u ul ull
				case 'u':case 'U':
					if(dot)goto default;
					add;next;
					if("is:l"||"is:L"){
						add;next;
						if("is:l"||"is:L")add;next;
					}
					return;
				// prefix: l ll llu lu
				case 'l':case 'L':
					add;next;
					if("is:l"||"is:L")add;next;
					if(!dot&&("is:u"||"is:U"))add;next;
					return;
			}
		}
		private void readNumber_e(){
			add;if(!next)goto err;

			// [+-]?
			if("is:-"||"is:+"){
				add;if(!next)goto err;
			}

			// \d+
			if(!("is:0-9"))goto err;
			do{
				add;next;
			}while("is:0-9");

			return;

		err:error("�w���\�L�̌�ɂ� [+-]?\\d+ ���K�v�ł��B");
		}
		//============================================================
		//		�R�����g
		//============================================================
		private void readMultiComment(){
			this.wtype=WordType.Comment;
			bool reach;
			do{
				if(tryReadLineBreak()){
					// ���s�̓ǂݎ��
					if(lreader.IsEndOfText)goto no_end;
					reach=false;
				}else{
					reach="is:*";
					add;if(!next)goto no_end;
				}
			}while(!reach||"not:/");
			add;nexit;
		no_end:
			error("�R�����g�ɏI�[ */ ������܂���");
		}
		/// <summary>
		/// �P��s�R�����g��ǂݎ��܂��B
		/// // �̎��̈ʒu�ŌĂяo���ĉ������B
		/// </summary>
		private void readLineComment(){
			this.wtype=WordType.Comment;
			while("not:\r"&&"not:\n"){
				if("is:\\"){
					// "\\\\" "\\\r" "\\\n" "\\\r\n"
					add;next;
					if(tryReadLineBreak()){
						if(lreader.IsEndOfText)return;
					}else if("is:\\"){
						add;next;
					}
				}else{
					add;next;
				}
			}
			nexit;
		}
		//============================================================
		//		���ʎq
		//============================================================
		private void readIdentifier(){
			lreader.StoreCurrentPos(1);
			if("is:L"){
				add;next;
				if("is:\""){
					ReadStringDQ();
					return;
				}else if("is:\'"){
					ReadStringSQ();
					return;
				}
			}
			lreader.MoveToPos(1);

			cword="";
			wtype=WordType.Identifier;
			add;next;
			while(true){
				if(type.IsToken||type.IsNumber||"is:_"||"is:$")goto _add;
				if("is:\\"){
					if(!tryReadUCN())return;
				}
				break;
			_add:
				add;next;
			}
		}
		//============================================================
		//		Universal-Character-Name
		//============================================================
		private bool tryReadUCN(){
			string old_cword=cword;
			lreader.StoreCurrentPos(1);
			if("is:u"){
				add;if(!next)goto fail;
				for(int i=0;i<4;i++){
					if("not:0-9"&&"not:a-f"&&"not:A-F")goto fail;
					add;if(!next)goto fail;
				}
				return true;
			}else if("is:U"){
				add;if(!next)goto fail;
				for(int i=0;i<8;i++){
					if("not:0-9"&&"not:a-f"&&"not:A-F")goto fail;
					add;if(!next)goto fail;
				}
				return true;
			}
		fail:
			cword=old_cword;
			lreader.MoveToPos(1);
			return false;
		}
		//============================================================
		//		��
		//============================================================
		private void readSpace(){
			wtype=WordType.Space;

			//-- ���s�̏ꍇ
			if(tryReadLineBreak())return;

			//-- ���s�ȊO�̘A����
			while(type.IsInvalid||type.IsSpace&&"not:\r"&&"not:\n"){
				add;next;
			}
		}
		private bool tryReadLineBreak(){
			switch(letter){
				case '\r':
					line_count++;
					add;if(!next)break;
					if("not:\n")break;
					add;if(!next)break;
					break;
				case '\n':
					line_count++;
					add;if(!next)break;
					break;
				default:
					return false;
			}
			return true;
		}
	}
}