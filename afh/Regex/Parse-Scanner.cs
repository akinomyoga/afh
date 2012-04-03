using afh.Parse;
using Gen=System.Collections.Generic;

namespace afh.RegularExpressions{
	internal partial class RegexScannerA:AbstractWordReader{
		public override bool ReadNext(){
			this.cword="";
			this.wtype=WordType.Invalid;
			if(lreader.IsEndOfText)return false;

			this.lreader.StoreCurrentPos(0);
			switch(letter){
				case '\\':
					wtype=WtCommand;
					readCommand(grammar.CommandSet);
					break;
				case ':':
					if(!grammar.EnablesColonCommand){
						error("コロン : の文字の前には \\ を置く様にして下さい。");
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
					add;
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
					add;
					lreader.MoveNext();
					break;
			}
			return true;
		}
		// + * ? +? *? ??
		private void readSuffix(){
			wtype=WordType.Suffix;
			add;next;
			if("is:?"){
				add;next;
			}
		}
		//============================================================
		//		文字クラス [...] の読み取り
		//============================================================
		private void readCharClass(){
			wtype=WtCharClass;
			if(!next)goto MISSING_CLOSE;

			int level=0;
			while(true)switch(letter){
				case '[':
					level++;
					goto default;
				case ']':
					if(level--==0)nexit;
					goto default;
				case '\\':
					add;if(!next){
						error("エスケープシーケンス/コマンドが完結していません。");
						goto MISSING_CLOSE;
					}
					goto default;
				default:
					add;if(!next)goto MISSING_CLOSE;
					break;
			}
		MISSING_CLOSE:
			error("クラス定義に末端の ] が存在しません。");
			return;
		}
		//============================================================
		//		量指定詞 {...} の読み取り
		//============================================================
		private void readQuantifier() {
			wtype=WordType.Suffix;
			if(!next)goto ERR_Q;

			//-- 引数数値の読み取り
			string arg="";
			string arg1=null;
			bool comma=false;
			while("not:}"&&"not:+"&&"not:?"){
				if("is:0-9"){
					arg+=letter;
				}else if("is:,"){
					if(comma)
						error("量指定詞の中の , は最大一つまでです。");
					else{
						comma=true;
						arg1=arg;
						arg="";
					}
				}else{
					error("量指定詞内の文字として無効です。数字か , を指定して下さい。");
				}
				if(!next)goto ERR_Q;
			}

			//-- 貪欲性 {m,n} {m,n?} {m,n+} 判別
			char chGreedy=letter;
			if("not:}"){
				if(!next)
					error("量指定詞が終了していない内に表現の末端に達しました。");
				else if("not:}")
					error("量指定詞内では、+/? の直後は } が来なければ為りません。'}' が在った物と見做します。");
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
					error("量指定詞の中に指定が存在していません。");
					return;
				}else{
					cword="q="; // {m,n} 形式
					value=arg;
				}
			}

			// if("is:?"){add;next;} // 廃止
			if(chGreedy!='}')cword+=chGreedy;
			return;
		ERR_Q:
			error("量指定詞に終端の } が在りません。");
		}
		//============================================================
		//		コマンド \* の読み取り
		//============================================================
		private void readCommand(Gen::Dictionary<string,CommandData> commands){
			__debug__.RegexParserToDo("複数文字名のコマンド");

			if(!next) {
				error(@"\ の後にコマンド名が続いていません。");
				wtype=WordType.Text;
				return;
			}
			add;next;
			
			CommandData dat;
			if(!commands.TryGetValue(cword,out dat)){
				error(@"指定した名前のコマンドは登録されていません。");
				wtype=WordType.Text;
				return;
			}

			if(dat.ArgumentType==CommandArgumentType.Brace){
				if("not:{"){
					value=null;
				}else{
					string cmd=cword;
					cword="";
					if(!next)goto ERR_ARG;

					int iP=1;
					bool escape=false;
					while(true){
						if(escape){
							escape=false;
						}else{
							if("is:{"){
								iP++;
							}else if("is:}"){
								if(--iP==0){lreader.MoveNext();break;}
							}else if("is:\\"){
								escape=true;
							}
						}
						add;if(!next)goto ERR_ARG;
					}
					value=cword;
					cword=cmd;
					return;
				ERR_ARG:
					value=cword;
					cword=cmd;
					error("引数の終端が来ない内に、正規表現の末端に達しました。");
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
			add;next;
			if("not:?")return; // "(": Capture
			add;next;
			switch(letter){
				case ':':
					cword="(?:";
					nexit;
				case '\'':
					next;
					cword="";
					while("not:\'"){
						add;if(!next){
							error("キャプチャ名に終端がありません。キャプチャ名は '\\'' で終わる必要があります。");
							break;
						}
					}
					value=cword;
					cword="(?<>";
					nexit; // ' を読み飛ばす
				case '<':
					// "(?<=" : 後読み一致
					// "(?<!" : 後読み不一致
					// "(?<>" : 名前付き捕獲 
					add;next;
					if("is:!"||"is:="){
						add;nexit;
					}else{
						cword="";
						while("not:>"){
							add;if(!next){
								error("キャプチャ名に終端がありません。キャプチャ名は '>' で終わる必要があります。");
								break;
							}
						}
						value=cword;
						cword="(?<>";
						nexit; // > を読み飛ばす
					}
				case '!': // 先読み不一致
				case '=': // 先読み一致
				case '>': // 非バックトラック
					add;nexit;
				case '#':
					wtype=WordType.Comment;
					while("not:)"){
						add;if(!next){
							error("コメントの終わり ')' が在りません。");
							return;
						}
					}
					nexit; // skip ')'
				default:
					// "(?flags:": (?imnx-imnx:
					// "(?flags)": (?imnx-imnx)
					if("is:-"||flags.IndexOf(letter)>=0){
						cword="";
						bool minused="is:-";
						do{
							add;if(!next){
								value=cword;
								cword="(?flags:";
							}
							if("is:-"){
								if(minused)break;
								minused=true;
							}
						}while("is:-"||flags.IndexOf(letter)>=0);

						value=cword;
						cword="(?flags";
						if("is::"||"is:)"){
							add;nexit;
						}else{
							string errmsg="'"+letter+"' は '(?flags' の後として予期しない文字です。':' が間にあると解釈します。";
							error(errmsg);
							cword="(?flags:";
							return;
						}
					}else{
						string errmsg="'"+letter+"' は '(?' の後として予期しない文字です。'(?:' と解釈します。";
						error(errmsg);
						cword="(?:";
						return;
					}
			}
		}

	}
}