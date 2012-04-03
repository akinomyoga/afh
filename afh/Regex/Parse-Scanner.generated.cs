/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
using afh.Parse;
using Gen=System.Collections.Generic;

namespace afh.RegularExpressions{
	internal partial class RegexScannerA:AbstractWordReader{
		public override bool ReadNext(){
			this.cword="";
			this.wtype=WordType.Invalid;
			if(lreader.IsEndOfText)return false;

			this.lreader.StoreCurrentPos(0);
			switch(this.lreader.CurrentLetter){
				case '\\':
					wtype=WtCommand;
					readCommand(grammar.CommandSet);
					break;
				case ':':
					if(!grammar.EnablesColonCommand){
						this.lreader.SetError(("コロン : の文字の前には \\ を置く様にして下さい。"),0,null);
						goto default;
					}
					wtype=WtCommandC;
					readCommand(grammar.ColonCommandSet);
					break;
				// + * ? +? *? ??
				case '+':case '*':case '?':
					readSuffix();
					break;
				case '(':
					readOpenParen();
					break;
				case ')':case '|':
					wtype=WordType.Operator;
					this.cword+=this.lreader.CurrentLetter;
					lreader.MoveNext();
					break;
				case '{':
					readQuantifier();
					break;
				case '[':
					readCharClass();
					break;
				default:
					wtype=WordType.Text;
					this.cword+=this.lreader.CurrentLetter;
					lreader.MoveNext();
					break;
			}
			return true;
		}
		// + * ? +? *? ??
		private void readSuffix(){
			wtype=WordType.Suffix;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			if((this.lreader.CurrentLetter=='?')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
		}
		//============================================================
		//		文字クラス [...] の読み取り
		//============================================================
		private void readCharClass(){
			wtype=WtCharClass;
			if(!this.lreader.MoveNext())goto MISSING_CLOSE;

			int level=0;
			while(true)switch(this.lreader.CurrentLetter){
				case '[':
					level++;
					goto default;
				case ']':
					if(level--==0){this.lreader.MoveNext();return;}
					goto default;
				case '\\':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
						this.lreader.SetError(("エスケープシーケンス/コマンドが完結していません。"),0,null);
						goto MISSING_CLOSE;
					}
					goto default;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto MISSING_CLOSE;
					break;
			}
		MISSING_CLOSE:
			this.lreader.SetError(("クラス定義に末端の ] が存在しません。"),0,null);
			return;
		}
		//============================================================
		//		量指定詞 {...} の読み取り
		//============================================================
		private void readQuantifier() {
			wtype=WordType.Suffix;
			if(!this.lreader.MoveNext())goto ERR_Q;

			//-- 引数数値の読み取り
			string arg="";
			string arg1=null;
			bool comma=false;
			while(!(this.lreader.CurrentLetter=='}')&&!(this.lreader.CurrentLetter=='+')&&!(this.lreader.CurrentLetter=='?')){
				if(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')){
					arg+=this.lreader.CurrentLetter;
				}else if((this.lreader.CurrentLetter==',')){
					if(comma)
						this.lreader.SetError(("量指定詞の中の , は最大一つまでです。"),0,null);
					else{
						comma=true;
						arg1=arg;
						arg="";
					}
				}else{
					this.lreader.SetError(("量指定詞内の文字として無効です。数字か , を指定して下さい。"),0,null);
				}
				if(!this.lreader.MoveNext())goto ERR_Q;
			}

			//-- 貪欲性 {m,n} {m,n?} {m,n+} 判別
			char chGreedy=this.lreader.CurrentLetter;
			if(!(this.lreader.CurrentLetter=='}')){
				if(!this.lreader.MoveNext())
					this.lreader.SetError(("量指定詞が終了していない内に表現の末端に達しました。"),0,null);
				else if(!(this.lreader.CurrentLetter=='}'))
					this.lreader.SetError(("量指定詞内では、+/? の直後は } が来なければ為りません。'}' が在った物と見做します。"),0,null);
			}
			lreader.MoveNext(); // } 読み飛ばし

			//-- 種類・引数の決定
			if(comma){
				if(arg==""){
					cword="q>"; // {m,} 形式
					value=arg1;
				}else{
					cword="q"; // {m,n} 形式
					value=arg1+","+arg;
				}
			}else{
				if(arg==""){
					this.lreader.SetError(("量指定詞の中に指定が存在していません。"),0,null);
					return;
				}else{
					cword="q="; // {m,n} 形式
					value=arg;
				}
			}

			// if((this.lreader.CurrentLetter=='?')){this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;} // 廃止
			if(chGreedy!='}')cword+=chGreedy;
			return;
		ERR_Q:
			this.lreader.SetError(("量指定詞に終端の } が在りません。"),0,null);
		}
		//============================================================
		//		コマンド \* の読み取り
		//============================================================
		private void readCommand(Gen::Dictionary<string,CommandData> commands){
			__debug__.RegexParserToDo("複数文字名のコマンド");

			if(!this.lreader.MoveNext()) {
				this.lreader.SetError((@"\ の後にコマンド名が続いていません。"),0,null);
				wtype=WordType.Text;
				return;
			}
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			
			CommandData dat;
			if(!commands.TryGetValue(cword,out dat)){
				this.lreader.SetError((@"指定した名前のコマンドは登録されていません。"),0,null);
				wtype=WordType.Text;
				return;
			}

			if(dat.ArgumentType==CommandArgumentType.Brace){
				if(!(this.lreader.CurrentLetter=='{')){
					value=null;
				}else{
					string cmd=cword;
					cword="";
					if(!this.lreader.MoveNext())goto ERR_ARG;

					int iP=1;
					bool escape=false;
					while(true){
						if(escape){
							escape=false;
						}else{
							if((this.lreader.CurrentLetter=='{')){
								iP++;
							}else if((this.lreader.CurrentLetter=='}')){
								if(--iP==0){lreader.MoveNext();break;}
							}else if((this.lreader.CurrentLetter=='\\')){
								escape=true;
							}
						}
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto ERR_ARG;
					}
					value=cword;
					cword=cmd;
					return;
				ERR_ARG:
					value=cword;
					cword=cmd;
					this.lreader.SetError(("引数の終端が来ない内に、正規表現の末端に達しました。"),0,null);
				}
			}
		}
		//============================================================
		//		グループ (...) の読み取り
		//============================================================
		private void readOpenParen() {
			// ( (?: (?<= (?<! (?> (?! (?=
			// (?imxn-imxn: (?<...>
			wtype=WordType.Operator;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			if(!(this.lreader.CurrentLetter=='?'))return; // "(": Capture
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			switch(this.lreader.CurrentLetter){
				case ':':
					cword="(?:";
					{this.lreader.MoveNext();return;}
				case '\'':
					if(!this.lreader.MoveNext())return;
					cword="";
					while(!(this.lreader.CurrentLetter=='\'')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
							this.lreader.SetError(("キャプチャ名に終端がありません。キャプチャ名は '\\'' で終わる必要があります。"),0,null);
							break;
						}
					}
					value=cword;
					cword="(?<>";
					{this.lreader.MoveNext();return;} // ' を読み飛ばす
				case '<':
					// "(?<=" : 後読み一致
					// "(?<!" : 後読み不一致
					// "(?<>" : 名前付き捕獲 
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='!')||(this.lreader.CurrentLetter=='=')){
						this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
					}else{
						cword="";
						while(!(this.lreader.CurrentLetter=='>')){
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
								this.lreader.SetError(("キャプチャ名に終端がありません。キャプチャ名は '>' で終わる必要があります。"),0,null);
								break;
							}
						}
						value=cword;
						cword="(?<>";
						{this.lreader.MoveNext();return;} // > を読み飛ばす
					}
				case '!': // 先読み不一致
				case '=': // 先読み一致
				case '>': // 非バックトラック
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				case '#':
					wtype=WordType.Comment;
					while(!(this.lreader.CurrentLetter==')')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
							this.lreader.SetError(("コメントの終わり ')' が在りません。"),0,null);
							return;
						}
					}
					{this.lreader.MoveNext();return;} // skip ')'
				default:
					// "(?flags:": (?imnx-imnx:
					// "(?flags)": (?imnx-imnx)
					if((this.lreader.CurrentLetter=='-')||flags.IndexOf(this.lreader.CurrentLetter)>=0){
						cword="";
						bool minused=(this.lreader.CurrentLetter=='-');
						do{
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
								value=cword;
								cword="(?flags:";
							}
							if((this.lreader.CurrentLetter=='-')){
								if(minused)break;
								minused=true;
							}
						}while((this.lreader.CurrentLetter=='-')||flags.IndexOf(this.lreader.CurrentLetter)>=0);

						value=cword;
						cword="(?flags";
						if((this.lreader.CurrentLetter==':')||(this.lreader.CurrentLetter==')')){
							this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
						}else{
							string errmsg="'"+this.lreader.CurrentLetter+"' は '(?flags' の後として予期しない文字です。':' が間にあると解釈します。";
							this.lreader.SetError((errmsg),0,null);
							cword="(?flags:";
							return;
						}
					}else{
						string errmsg="'"+this.lreader.CurrentLetter+"' は '(?' の後として予期しない文字です。'(?:' と解釈します。";
						this.lreader.SetError((errmsg),0,null);
						cword="(?:";
						return;
					}
			}
		}

	}
}