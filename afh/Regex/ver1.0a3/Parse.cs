using Gen=System.Collections.Generic;
using afh.Parse;
using RangeErrorPair=System.Collections.Generic.KeyValuePair<
	afh.Parse.LinearLetterReader.TextRange,
	afh.Parse.AnalyzeError
>;

namespace afh.RegularExpressions{
	public partial class RegexFactory3<T>{
		/// <summary>
		/// 正規表現読み取りを行うクラスの基本クラスです。
		/// </summary>
		protected abstract class ParserBase{
			RegexScannerA scanner;

			/// <summary>
			/// 正規表現を規定する生成器を保持します。
			/// </summary>
			protected readonly RegexFactory3<T> parent;
			/// <summary>
			/// 生成器を指定して ParserBase を初期化します。
			/// </summary>
			/// <param name="parent">正規表現を規定する生成器を指定します。</param>
			protected ParserBase(RegexFactory3<T> parent){
				this.parent=parent;
			}

			/// <summary>
			/// 指定した正規表現を解釈します。
			/// </summary>
			/// <param name="regex">正規表現を記述する文字列を指定します。</param>
			/// <returns>解釈の結果として出来た構造を返します。</returns>
			public INode Parse(string regex){
				this.scanner=new RegexScannerA(regex,this.GetGrammar());
				if(!this.scanner.ReadNext())return null;

				INode ret=Read();

				// 未だ読み取っていない部分が残っている場合
				if(this.scanner.CurrentType!=WordType.Invalid){
					if(this.scanner.CurrentType==WordType.Operator&&this.scanner.CurrentWord==")"){
						scanner.LetterReader.SetError("余分な ')' です。",0,null);
					}
					__debug__.RegexParserAssert(false);
				}
				return ret;
			}
			/// <summary>
			/// 正規表現の解釈の際に生じたエラーの列挙を行います。
			/// </summary>
			public Gen::IEnumerable<RangeErrorPair> Errors{
				get{
					if(this.scanner==null)return null;
					return this.scanner.LetterReader.EnumErrors();
				}
			}
			//========================================================
			//		継承者向け
			//========================================================
			/// <summary>
			/// この正規表現の文法を定義する IRegexGrammar インスタンスを取得します。
			/// </summary>
			/// <returns>この正規表現の文法を定義している RegexGrammar インスタンスを返します。</returns>
			protected abstract IRegexGrammar GetGrammar();
			/// <summary>
			/// コマンドに対応する正規表現要素を作ります。
			/// </summary>
			/// <param name="name">コマンドの名前を指定します。</param>
			/// <param name="arg">コマンドの引数を指定します。</param>
			/// <returns>作成した正規表現要素を返します。</returns>
			protected abstract INode ProcessCommand(string name,string arg);
			/// <summary>
			/// コロンコマンドに対応する正規表現要素を作ります。
			/// </summary>
			/// <param name="name">コロンコマンドの名前を指定します。</param>
			/// <param name="arg">コロンコマンドの引数を指定します。</param>
			/// <returns>作成した正規表現要素を返します。</returns>
			protected abstract INode ProcessCommandC(string name,string arg);
			/// <summary>
			/// 文字から正規表現要素を作ります。
			/// </summary>
			/// <param name="letter">正規表現要素の元となる文字を指定します。</param>
			/// <returns>指定した文字を元に作成した正規表現要素を返します。</returns>
			protected abstract INode ProcessLetter(char letter);
			/// <summary>
			/// クラス表現から正規表現要素を作ります。
			/// </summary>
			/// <param name="content">クラス指定 [...] の中身を指定します。</param>
			/// <returns>指定した文字列を元に作成した正規表現要素を返します。</returns>
			protected abstract INode ProcessClass(string content);
			/// <summary>
			/// 読み取りの際に生じたエラーを設定します。
			/// </summary>
			/// <param name="message">エラーの内容について説明する文字列を指定します。</param>
			protected void Error(string message){
				this.scanner.LetterReader.SetError(message,0,null);
			}
			//========================================================
			//		読み取りの関数
			//========================================================
			private INode ReadUntilCloseParen(){
				INode ret=Read();

				// 読み取り終わっていないのに終了した場合。
				if(this.scanner.CurrentType==WordType.Invalid){
					this.Error("'(' に対応する終わりの ')' が足りません。");
				}else{
					__debug__.RegexParserAssert(this.scanner.CurrentType==WordType.Operator&&this.scanner.CurrentWord==")");
					this.scanner.ReadNext();
				}

				if(ret==null){
					this.Error("括弧の中身が存在しません。");
				}

				return ret;
			}
			//========================================================
			/// <summary>
			/// '|' で区切られた候補達を読み取ります。
			/// </summary>
			/// <returns>読み取って出来たノードを返します。有効な正規表現指定が見つからなかった場合には null を返します。</returns>
			private INode Read(){
				Gen::List<INode> nodes=new Gen::List<INode>();
				while(true)switch(this.scanner.CurrentType.value) {
					case WordType.vInvalid:
						goto ED_LOOP;
					case WordType.vOperator:
						if(scanner.CurrentWord==")")
							goto ED_LOOP;
						else if(scanner.CurrentWord=="|"){
							this.scanner.ReadNext();
							goto default;
						}else{
							goto default;
						}
					default:
						INode node=this.ReadSequence();
						if(node==null){
							scanner.LetterReader.SetError("正規表現要素が一つもありません。",0,null);
							break;
						}

						nodes.Add(node);
						break;
				}
			ED_LOOP:
				if(nodes.Count==0)return null;
				if(nodes.Count==1)return nodes[0];
				return new OrNode(nodes.ToArray());
			}
			/// <summary>
			/// 要素連続部分 ('|' '(' ')' などで区切られた部分) を読み取ります。
			/// </summary>
			/// <returns>読み取って出来たノードを返します。有効な正規表現指定が見つからなかった場合には null を返します。</returns>
			private INode ReadSequence(){
				Gen::List<INode> nodes=new Gen::List<INode>();
				while(true)switch(this.scanner.CurrentType.value){
					case WordType.vInvalid:
						goto ED_LOOP;
					case WordType.vComment: // (?#)
						this.scanner.ReadNext();
						continue;
					case RegexScannerA.WT_CHARCLASS: // [文字クラス]
						nodes.Add(this.ProcessClass(this.scanner.CurrentWord));
						this.scanner.ReadNext();
						break;
					case RegexScannerA.WT_COMMAND: // コマンド
						parseCommand(ref nodes);
						break;
					case RegexScannerA.WT_COMMAND_C: // : コマンド
						nodes.Add(this.ProcessCommandC(scanner.CurrentWord,scanner.Value));
						this.scanner.ReadNext();
						break;
					case WordType.vSuffix: // ? + *
						parseSuffix(ref nodes);
						break;
					case WordType.vOperator: // ( | )
						if(scanner.CurrentWord==")"||scanner.CurrentWord=="|")
							goto ED_LOOP;
						if(scanner.CurrentWord=="(?flags)"){
							// 括弧の始まる前に flags を覚えておく仕組み
							__debug__.RegexParserToDo("未実装");
						}
						INode node=parseOperator();
						if(node!=null)nodes.Add(node);
						break;
					case WordType.vText: // 通常の文字
						switch(scanner.CurrentWord[0]){
							case '.':nodes.Add(AnyElemNode.Instance);break;
							case '^':nodes.Add(StartOfStreamNode.instance);break;
							case '$':nodes.Add(EndOfStreamNode.instance);break;
							default:
								nodes.Add(this.ProcessLetter(scanner.CurrentWord[0]));
								break;
						}
						this.scanner.ReadNext();
						break;
				}
			ED_LOOP:
				if(nodes.Count==0)return null;
				if(nodes.Count==1)return nodes[0];
				return new SequenceNode(nodes.ToArray());
			}
			//========================================================
			private void parseCommand(ref Gen::List<INode> list){
				try{
					list.Add(this.ProcessCommand(scanner.CurrentWord,scanner.Value));
				}catch(RegexParseException e){
					this.scanner.LetterReader.SetError(e.Message,0,null);
				}
				this.scanner.ReadNext();
			}
			/// <summary>
			/// 括弧の始まり、その他の演算子を読み取ります。
			/// </summary>
			/// <returns></returns>
			private INode parseOperator(){
				string word=this.scanner.CurrentWord;
				string val=this.scanner.Value;
				this.scanner.ReadNext();

				INode content=null;
				if(word[0]=='(')
					content=this.ReadUntilCloseParen();
				if(content==null)return null;

				switch(word){
					case "(":
						return new CaptureNode(content);
					case "(?:":
						return content;
					case "(?<>":
						return new CaptureNode(content,val);
					case "(!":return new AheadAssertNode(content,false,parent);
					case "(=":return new AheadAssertNode(content,true,parent);
					case "(>":return new NonBacktrackNode(content,parent);
					case "(?<=":
					case "(?<!":
					case "(?flags:":
					default:
						__debug__.RegexParserToDo("未実装");
						throw new System.NotImplementedException();
					case "(?flags)":
					case ")":
					case "|":
						__debug__.RegexParserAssert(false,"呼び出し元で処理するので、ここには到達しない筈");
						return null;
				}
			}
			/// <summary>
			/// 量指定詞を読み取ります。
			/// </summary>
			/// <param name="list">今迄に読み取った INode 列を指定します。量指定詞は最後の INode に適用されます。</param>
			private void parseSuffix(ref Gen::List<INode> list){
				if(list.Count==0){
					this.scanner.LetterReader.SetError("Suffix "+scanner.CurrentWord+" に先行する指定詞が存在しません。",0,null);
					this.scanner.ReadNext();
					return;
				}

				INode node=list[list.Count-1];
				switch(scanner.CurrentWord){
					case "+":
						node=new RepeatNode(node,1,-1);
						break;
					case "*":
						node=new RepeatNode(node,0,-1);
						break;
					case "?":
						node=new RepeatNode(node,0,1);
						break;
					case "q":{ // {m,n}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new RepeatNode(node,a,b);
						break;
					}
					case "q=":{ // {m}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);
						
						node=new RepeatNode(node,a,a);
						break;
					}
					case "q>":{ // {m,}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new RepeatNode(node,a,-1);
						break;
					}
					//================================================
					//		Lazy-Repeat
					//================================================
					case "+?":
						node=new LazyRepeatNode(node,1,-1);
						break;
					case "*?":
						node=new LazyRepeatNode(node,0,-1);
						break;
					case "??":
						node=new LazyRepeatNode(node,0,1);
						break;
					case "q?":{ // {m,n?}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,b);
						break;
					}
					case "q=?":{ // {m?} ※ 単なる {m} と等価
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,a);
						break;
					}
					case "q>?":{ // {m,?}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,-1);
						break;
					}
					//================================================
					//		Sticky-Repeat
					//================================================
					case "++":
						node=new StickyRepeatNode(node,1,-1);
						break;
					case "*+":
						node=new StickyRepeatNode(node,0,-1);
						break;
					case "?+":
						node=new StickyRepeatNode(node,0,1);
						break;
					case "q+":{ // {m,n+}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,b);
						break;
					}
					case "q=+":{ // {m+} ※ 単なる {m} と等価
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,a);
						break;
					}
					case "q>+":{ // {m,+}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,-1);
						break;
					}
					default:
						__debug__.RegexParserAssert(false);
						break;
				}
				list[list.Count-1]=node;

				this.scanner.ReadNext();
			}
		}
	}
}