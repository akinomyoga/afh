/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
using afh.Parse;

namespace afh.Preprocessor{
	internal sealed partial class CppWordReader:AbstractWordReader{
		private void readOperator(){
			this.wtype=WordType.Operator;
			char c;
			switch(this.lreader.CurrentLetter){
				case '=':case '!':
					// o o=
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					goto aft_equal;
				case ':':
					// : ::
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter==':')){this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;}
					return;
				case '-':
					// - -- -= -> ->*
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='-')||(this.lreader.CurrentLetter=='=')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					}else if((this.lreader.CurrentLetter=='>')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						if((this.lreader.CurrentLetter=='*')){this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;}
					}
					return;
				case '+':
					// + ++ +=
					c=this.lreader.CurrentLetter;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!=c)goto aft_equal;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				case '&':case '|':case '<':case '>':
					// o oo o= oo=
					c=this.lreader.CurrentLetter;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!=c)goto aft_equal;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					goto aft_equal;
				case '*':case '%':case '^':
					// o o=
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					goto aft_equal;
				case '/':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='*')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
							this.lreader.SetError(("コメントに終端 */ がありません。"),0,null);
							return;
						}
						readMultiComment();return;
					}else if((this.lreader.CurrentLetter=='/')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						readLineComment();return;
					}else goto aft_equal;
				case '"':ReadStringDQ();return;
				case '\'':ReadStringSQ();return;
				case '.':
					// . .. ... .*
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')){
						lreader.MoveToPos(0);
						this.cword="";
						readNumber();
						return;
					}
					if((this.lreader.CurrentLetter=='*')){this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;}
					if(!(this.lreader.CurrentLetter=='.'))return;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(!(this.lreader.CurrentLetter=='.'))return;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}		// ... 演算子
				case '\\':
					if(tryReadUCN()){
						cword="";
						lreader.MoveToPos(0);
						readIdentifier();
					}
					goto default;
				case '#':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;

					if(mode==Mode.MacroContent){
						if((this.lreader.CurrentLetter=='#')||(this.lreader.CurrentLetter=='@')||(this.lreader.CurrentLetter=='&')){
							wtype=WordType.Operator;
							this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
						}
					}

					wtype=WordType.Tag;
					readLineComment();
					wtype=WordType.Tag;
					return;
				case '@':
					wtype=WordType.KeyWord;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					readLineComment();
					wtype=WordType.KeyWord;
					return;
				case ',':case '?':
				case '[':case ']':
				case '(':case ')':
				case '{':case '}':
				default:
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				aft_equal:
					if(!(this.lreader.CurrentLetter=='='))return;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
			}
		}
		//============================================================
		//		数値リテラル
		//============================================================
		private void readNumber(){
			wtype=WordType.Literal;
			if((this.lreader.CurrentLetter=='0')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='x')||(this.lreader.CurrentLetter=='X')){
					readNumber_x();
					return;
				}
			}
			readNumber_f();
		}

		private void readNumber_x(){
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [0-9a-fA-F]+
			if(!(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')||('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')||('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F')))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')||('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')||('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F'));
			return;

		err:this.lreader.SetError(("0x の次には [0-9a-fA-F]+ が来るべきです"),0,null);
		}

		private void readNumber_f(){
			bool dot=false;
			while(true)switch(this.lreader.CurrentLetter){
				case '0':case '1':case '2':case '3':case '4':
				case '5':case '6':case '7':case '8':case '9':
					goto _add;
				case '.':
					// 既出だったら×
					if(dot)goto default;
					// 先読み 0-9 だったら○
					lreader.StoreCurrentPos(1);
					if(!this.lreader.MoveNext())return;
					if(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')){
						this.cword+='.';
						dot=true;
						goto _add;
					}
					lreader.MoveToPos(1);
					goto default;
				case 'e':case 'E':
					readNumber_e();
					if((this.lreader.CurrentLetter=='f')||(this.lreader.CurrentLetter=='F'))goto case 'f';
					return;
				default:
					return;
				_add:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				//----------------------------------------------------
				//		Prefixes
				//----------------------------------------------------
				// prefix: f
				case 'f':case 'F':
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				// prefix: i64
				case 'i':
					if(dot)goto default;
					lreader.StoreCurrentPos(1);
					do{
						if(!this.lreader.MoveNext())break;
						if(!(this.lreader.CurrentLetter=='6'))break;
						if(!this.lreader.MoveNext())break;
						if(!(this.lreader.CurrentLetter=='4'))break;
						cword+="i64";
						{this.lreader.MoveNext();return;}
#pragma warning disable 162
					}while(false);
#pragma warning restore 162
					lreader.MoveToPos(1);
					goto default;
				// prefix: u ul ull
				case 'u':case 'U':
					if(dot)goto default;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='l')||(this.lreader.CurrentLetter=='L')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						if((this.lreader.CurrentLetter=='l')||(this.lreader.CurrentLetter=='L'))this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					}
					return;
				// prefix: l ll llu lu
				case 'l':case 'L':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='l')||(this.lreader.CurrentLetter=='L'))this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(!dot&&((this.lreader.CurrentLetter=='u')||(this.lreader.CurrentLetter=='U')))this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					return;
			}
		}
		private void readNumber_e(){
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [+-]?
			if((this.lreader.CurrentLetter=='-')||(this.lreader.CurrentLetter=='+')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			}

			// \d+
			if(!(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'));

			return;

		err:this.lreader.SetError(("指数表記の後には [+-]?\\d+ が必要です。"),0,null);
		}
		//============================================================
		//		コメント
		//============================================================
		private void readMultiComment(){
			this.wtype=WordType.Comment;
			bool reach;
			do{
				if(tryReadLineBreak()){
					// 改行の読み取り
					if(lreader.IsEndOfText)goto no_end;
					reach=false;
				}else{
					reach=(this.lreader.CurrentLetter=='*');
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto no_end;
				}
			}while(!reach||!(this.lreader.CurrentLetter=='/'));
			this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
		no_end:
			this.lreader.SetError(("コメントに終端 */ がありません"),0,null);
		}
		/// <summary>
		/// 単一行コメントを読み取ります。
		/// // の次の位置で呼び出して下さい。
		/// </summary>
		private void readLineComment(){
			this.wtype=WordType.Comment;
			while(!(this.lreader.CurrentLetter=='\r')&&!(this.lreader.CurrentLetter=='\n')){
				if((this.lreader.CurrentLetter=='\\')){
					// "\\\\" "\\\r" "\\\n" "\\\r\n"
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(tryReadLineBreak()){
						if(lreader.IsEndOfText)return;
					}else if((this.lreader.CurrentLetter=='\\')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					}
				}else{
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				}
			}
			{this.lreader.MoveNext();return;}
		}
		//============================================================
		//		識別子
		//============================================================
		private void readIdentifier(){
			lreader.StoreCurrentPos(1);
			if((this.lreader.CurrentLetter=='L')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='\"')){
					ReadStringDQ();
					return;
				}else if((this.lreader.CurrentLetter=='\'')){
					ReadStringSQ();
					return;
				}
			}
			lreader.MoveToPos(1);

			cword="";
			wtype=WordType.Identifier;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true){
				if(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||(this.lreader.CurrentLetter=='_')||(this.lreader.CurrentLetter=='$'))goto _add;
				if((this.lreader.CurrentLetter=='\\')){
					if(!tryReadUCN())return;
				}
				break;
			_add:
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
		}
		//============================================================
		//		Universal-Character-Name
		//============================================================
		private bool tryReadUCN(){
			string old_cword=cword;
			lreader.StoreCurrentPos(1);
			if((this.lreader.CurrentLetter=='u')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto fail;
				for(int i=0;i<4;i++){
					if(!('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')&&!('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')&&!('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F'))goto fail;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto fail;
				}
				return true;
			}else if((this.lreader.CurrentLetter=='U')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto fail;
				for(int i=0;i<8;i++){
					if(!('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')&&!('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')&&!('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F'))goto fail;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto fail;
				}
				return true;
			}
		fail:
			cword=old_cword;
			lreader.MoveToPos(1);
			return false;
		}
		//============================================================
		//		空白
		//============================================================
		private void readSpace(){
			wtype=WordType.Space;

			//-- 改行の場合
			if(tryReadLineBreak())return;

			//-- 改行以外の連続空白
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace&&!(this.lreader.CurrentLetter=='\r')&&!(this.lreader.CurrentLetter=='\n')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
		}
		private bool tryReadLineBreak(){
			switch(this.lreader.CurrentLetter){
				case '\r':
					line_count++;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())break;
					if(!(this.lreader.CurrentLetter=='\n'))break;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())break;
					break;
				case '\n':
					line_count++;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())break;
					break;
				default:
					return false;
			}
			return true;
		}
	}
}