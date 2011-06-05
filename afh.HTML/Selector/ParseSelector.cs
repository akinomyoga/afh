using afh.Parse;
using Gen=System.Collections.Generic;
namespace afh.HTML.Selector{

	#region class Parser
	/// <summary>
	/// 文字列で表された Selector を読み取ります。
	/// </summary>
	internal sealed class Parser{
		private WordReader wreader;
		private AWordReader areader;
		private Rule rule;
		private SimpleSelector simple;
		public Parser(string text){
			this.wreader=new WordReader(text);
			this.areader=new AWordReader(this.wreader.LetterReader);
		}
		public Selector Parse(){
			System.Collections.Generic.List<SimpleSelector> simples=new System.Collections.Generic.List<SimpleSelector>();
			while(!this.wreader.LetterReader.IsEndOfText&&this.readSimple()){
				simples.Add(this.simple);
			}
			return new Selector(simples.ToArray());
		}
		//===========================================================
		//		readSimple : SimpleSelector を読み取ります
		//===========================================================
		/// <summary>
		/// SimpleSelector を読み取ります。
		/// 呼び出し時{wreader位置: SimpleSelector を構成する物の内で一番初めの word;}
		/// 呼び出し後{wreader位置: 次に処理されるべき単語;}
		/// </summary>
		/// <returns>読込に成功した場合に true を読込に失敗した場合に false を返します。
		/// 異常があっても出来るだけ読み込むようにする為、false が返される時は文字列の末端になります。</returns>
		private bool readSimple(){
			this.simple.name=null;
			bool relat=false;
			do switch(this.wreader.CurrentType.value){
				case WordReader.vRelation:
					if(relat)goto next;
					relat=true;
					this.simple.relation=this.wreader.CurrentWord[0];
					break;
				case WordType.vIdentifier:
					if(!relat){
						relat=true;
						this.simple.relation=' ';
					}
					this.simple.name=this.wreader.CurrentWord;
					break;
				case WordReader.vOpenBra:
					if(!relat)this.simple.relation=' ';
					if(this.simple.name==null)this.simple.name="*";
					this.simple.rules=this.readSimple_attrs();
					return true;
			}while(this.wreader.ReadNext());
		next:
			if(this.simple.name==null)this.simple.name="*";
			this.simple.rules=new Rule[]{};
			return relat;
		}
		/// <summary>
		/// [] で囲まれた属性に依る Selector 規則を読み取ります。
		/// 呼び出し時{areader位置: "[";}
		/// 呼び出し後{wreader位置: "]";}
		/// </summary>
		/// <returns>読み取った規則を返します。</returns>
		private Rule[] readSimple_attrs(){
			System.Collections.Generic.List<Rule> rules=new System.Collections.Generic.List<Rule>();
			this.rule.varName=null;
			this.rule.value=null;
			while(this.areader.ReadNext_modeId()){
				if(this.readRule())rules.Add(this.rule);
				if(this.areader.CurrentType==WordType.Invalid){
					this.areader.LetterReader.SetError("属性値規則一覧に終端 ']' がありません。",0,null);
					break;
				}
				switch(this.areader.CurrentWord[0]){
					case ']':
						this.wreader.ReadNext();
						goto ret;
					case ',':case '&':case '|':
					default:
						break;
				}
			}
		ret:
			return rules.ToArray();
		}
		
		//===========================================================
		//		readRule : 属性選択規則 Rule を読み取ります
		//===========================================================
		private const string readRule_err11="予期せぬ演算子です。この演算子は無視します。";
		private const string readRule_err12="先に識別子を記述して下さい。これらの記述は無視します。";
		private const string readRule_err13="初めの規則で識別子は省略出来ません。この比較演算子は無かった事にします。";
		private const string readRule_err14="初めの規則で識別子は省略出来ません。この修飾子は無かった事にします。";
		private const string readRule_err21="識別子の直後に文字列が来るのは変です。これは無視します。\r\n識別子の後には規則の末端か、比較演算子が来なければ為りません。";
		private const string readRule_err22="識別子を複数指定する事は出来ません。これは無視します。\r\n識別子の後には規則の末端か、比較演算子が来なければ為りません。";
		private const string readRule_err31="【algorithmicly unexpected】\r\n識別子[+変数修飾子] の直後に{0}が来るのは変です。これは無視します。\r\n識別子[+変数修飾子] の後には規則の末端か、比較演算子が来なければ為りません。";
		private const string readRule_err41="比較演算子の後に亦比較演算子が来るのは変です。\r\n無視します。";
		private const string readRule_err42="予期せぬ演算子です。\r\n無視します。";
		private const string readRule_err43="比較対象値が省略されています。比較対象値は空白と見做します。";
		private const string readRule_err61="比較対象値+修飾子 の後には規則の末端しか来る事が出来ません。これらの記述は無視されます。";
		private enum readRule_ret{
			JumpTo1,DropFrom1,
			EnterIn2,JumpTo2,DropFrom2,
			EnterIn3,JumpTo3,DropFrom3,
			EnterIn4,JumpTo4,DropFrom4,
			EnterIn5,JumpTo5,DropFrom5,
			EnterIn6,JumpTo6,DropFrom6,
		}
		/// <summary>
		/// Rule を読み取ります。
		/// <para>
		/// this.rule の varName 及び value には前回読み取った規則の値が入っています。
		/// 新しく規則群を読み取る際には両 field を null に設定して下さい。
		/// [width&gt;=10&amp;&lt;=50] は [width&gt;=10&amp;width&lt;=50] と解釈します。
		/// [href.l.s="hello"&amp;.l.m="how are you"] は [href.l.s="hello"&amp;href.l.m="how are you"] と解釈します。
		/// </para>
		/// </summary>
		/// <returns>読み取る事が出来た場合に true を読み取りに失敗した場合に false を返します。</returns>
		private bool readRule(){
			readRule_ret ret=readRule_ret.JumpTo1;
			while(true)switch(ret){
				// 変数名読込
				case readRule_ret.JumpTo1:
					this.areader.mode=AWordReaderMode.ReadIdentifier;
					ret=this.readRule1();
					this.areader.mode=AWordReaderMode.Normal;
					break;
				case readRule_ret.DropFrom1:
					return false;
				// 変数修飾子読込
				case readRule_ret.EnterIn2:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo2;
					else
						goto case readRule_ret.DropFrom2;
				case readRule_ret.JumpTo2:
					ret=this.readRule2();
					break;
				case readRule_ret.DropFrom2:
					this.rule.ope=' ';
					return true;
				// 比較演算子読込
				case readRule_ret.EnterIn3:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo3;
					else
						goto case readRule_ret.DropFrom3;
				case readRule_ret.JumpTo3:
					ret=this.readRule3();
					break;
				case readRule_ret.DropFrom3:
					this.rule.ope=' ';
					return true;
				// 比較対象値読込
				case readRule_ret.EnterIn4:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo4;
					else
						goto case readRule_ret.DropFrom4;
				case readRule_ret.JumpTo4:
					ret=this.readRule4();
					break;
				case readRule_ret.DropFrom4:
					// 若し null でなかったら前回の値を参照
					if(this.rule.value==null){
						this.rule.value="";
						this.rule.valModifier="";
					}
					return true;
				// 対象値修飾子読込
				case readRule_ret.EnterIn5:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo5;
					else
						goto case readRule_ret.DropFrom5;
				case readRule_ret.JumpTo5:
					ret=this.readRule5();
					break;
				case readRule_ret.DropFrom5:
					this.rule.valModifier="";
					return true;
				// ごみ読み切り
				case readRule_ret.EnterIn6:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo6;
					else
						goto case readRule_ret.DropFrom6;
				case readRule_ret.JumpTo6:
					ret=this.readRule6();
					break;
				case readRule_ret.DropFrom6:
					return true;
			}
		}
		/// <summary>
		/// 変数名の取得をします。
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule1() {
			do switch(this.areader.CurrentType.value){
				case WordType.vIdentifier:
					this.rule.varName=this.areader.CurrentWord;
					return readRule_ret.EnterIn2;
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom1;
						case '~':case '<':case '>':case '=':case '!':case '↓':case '→':
							if(this.rule.varName!=null){
								return readRule_ret.JumpTo3;
							}
							this.areader.LetterReader.SetError(readRule_err13,0,null);
							break;
						default:
							this.areader.LetterReader.SetError(readRule_err11,0,null);
							break;
					}
					break;
				case WordType.vModifier:
					if(this.rule.varName!=null){
						return readRule_ret.JumpTo2;
					}
					this.areader.LetterReader.SetError(readRule_err14,0,null);
					break;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(readRule_err12,0,null);
					break;
				case WordType.vElement:
					this.rule.varName=this.areader.CurrentWord;
					this.rule.varModifier="";
					this.rule.ope='籠';
					this.rule.value=null;
					this.rule.valModifier="";
					return readRule_ret.EnterIn6;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom1;
		}
		/// <summary>
		/// 変数に対する修飾子の取得をします。
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule2(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					this.rule.varModifier="";
					return readRule_ret.JumpTo3;
				case WordType.vModifier:
					rule.varModifier=this.areader.CurrentWord;
					return readRule_ret.EnterIn3;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(readRule_err21,0,null);
					break;
				case WordType.vIdentifier:
					this.areader.LetterReader.SetError(readRule_err22,0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom2;
		}
		/// <summary>
		/// 比較演算子の取得をします。
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule3(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					char char1=this.areader.CurrentWord[0];
					switch(char1){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom3;
						case '~':case '=':case '!':case '→':
							rule.ope=char1;
							return readRule_ret.EnterIn4;
						case '<':
							rule.ope=this.areader.CurrentWord.Length==2?'≦':'<';
							return readRule_ret.EnterIn4;
						case '>':
							rule.ope=this.areader.CurrentWord.Length==2?'≧':'>';
							return readRule_ret.EnterIn4;
						case '↓':
							rule.ope=char1;
							rule.value="";
							rule.valModifier="";
							return readRule_ret.EnterIn6;
						default:
							this.areader.LetterReader.SetError(readRule_err42,0,null);
							break;
					}
					break;
				// 以下アルゴリズム上到達不能の筈だが念の為
				case WordType.vModifier:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"再度修飾子"),0,null);
					break;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"文字列"),0,null);
					break;
				case WordType.vIdentifier:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"識別子"),0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom3;
		}
		/// <summary>
		/// 比較対象値の取得
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule4(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom4;
						case '~':case '<':case '>':case '=':case '!':case '↓':case '→':
							this.areader.LetterReader.SetError(readRule_err41,0,null);
							break;
						default:
							this.areader.LetterReader.SetError(readRule_err42,0,null);
							break;
					}
					break;
				case WordType.vLiteral:
				case WordType.vIdentifier:
					this.rule.value=this.areader.CurrentWord;
					return readRule_ret.EnterIn5;
				case WordType.vModifier:
					if(this.rule.value==null){
						this.areader.LetterReader.SetError(readRule_err43,0,0);
						this.rule.value="";
					}
					return readRule_ret.JumpTo5;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom4;
		}
		/// <summary>
		/// 比較対象値修飾子の取得
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule5(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom5;
						default:
							this.rule.value+=this.areader.CurrentWord;
							break;
					}
					break;
				case WordType.vLiteral:
				case WordType.vIdentifier:
					this.rule.value+=this.areader.CurrentWord;
					break;
				case WordType.vModifier:
					this.rule.valModifier=this.areader.CurrentWord;
					return readRule_ret.EnterIn6;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom5;
		}
		/// <summary>
		/// ごみの読み切り
		/// </summary>
		/// <returns>次の動作を指定します。</returns>
		private readRule_ret readRule6(){
			do switch(this.areader.CurrentWord[0]){
				case ']':case ',':case '|':case '&':
					return readRule_ret.DropFrom6;
				default:
					this.areader.LetterReader.SetError(readRule_err61,0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom6;
		}
	}
	#endregion

	#region class WordReader
	/// <summary>
	/// セレクタの読み取りの為の wordreader です。
	/// </summary>
	internal sealed class WordReader:afh.Parse.AbstractWordReader{
		public WordReader(string text):base(text){}
		//=======================================
		//		定数
		//=======================================
		internal static readonly WordType wtOpenBra=new WordType(vOpenBra);
		internal static readonly WordType wtRelation=new WordType(vRelation);
		internal const int vOpenBra=0x100;
		internal const int vRelation=0x101;
		//=======================================
		//		読み取りメソッド
		//=======================================
		public override bool ReadNext(){
		#if MACRO_WORDREADER
			this.cword="";
			while([type].IsInvalid||[type].IsSpace)[if!next]return false;
			this.lreader.StoreCurrentPos(0);
			switch([type].purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
		#endif
			#region #OUT#
			this.cword="";
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace)if(!this.lreader.MoveNext())return false;
			this.lreader.StoreCurrentPos(0);
			switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
			#endregion #OUT#
		}
		private void readOperator(){
			this.wtype=WordType.Operator;
		#if MACRO_WORDREADER
			switch([letter]){
				case '@':this.readIdentifier();break;
				case '/':case '+':case '-':case '<':case '>':
					this.wtype=wtRelation;
					[add][nexit]
				case '[':
					this.wtype=wtOpenBra;
					[add][nexit]
				case '*':
					this.wtype=WordType.Identifier;
					[add][nexit]
				default:
					this.wtype=WordType.Invalid;
					[error:[letter]+"は対応していない記号です。無視します。"]
					[nexit]
			}
		#endif
			#region #OUT#
			switch(this.lreader.CurrentLetter){
				case '@':this.readIdentifier();break;
				case '/':case '+':case '-':case '<':case '>':
					this.wtype=wtRelation;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '[':
					this.wtype=wtOpenBra;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '*':
					this.wtype=WordType.Identifier;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				default:
					this.wtype=WordType.Invalid;
					this.lreader.SetError(this.lreader.CurrentLetter+"は対応していない記号です。無視します。",0,null);
					this.lreader.MoveNext();return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// Selector 用の識別子を読み取ります。
		/// \ をエスケープ文字として使用出来ます。
		/// 呼び出し条件{
		///		文字: 識別子の文字|@
		///		位置: 一文字目の文字
		/// }
		/// </summary>
		private void readIdentifier(){
			this.wtype=WordType.Identifier;
		#if MACRO_WORDREADER
			[add][next]
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					if([is:_]||[is:-])goto case LetterType.P_Token;
					if([not:\\])goto default;
					[next]
					goto case LetterType.P_Token;
				default:
					return;
			}
		#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					if(this.lreader.CurrentLetter=='_'||this.lreader.CurrentLetter=='-')goto case LetterType.P_Token;
					if(this.lreader.CurrentLetter!='\\')goto default;
					if(!this.lreader.MoveNext())return;
					goto case LetterType.P_Token;
				default:
					return;
			}
			#endregion #OUT#
		}
	}
	#endregion

	#region class AWordReader
	internal enum AWordReaderMode{Normal,ReadIdentifier}
	/// <summary>
	/// 属性セレクタ部の読み取りの為の wordreader です。
	/// </summary>
	internal sealed class AWordReader:AbstractWordReader{
		public AWordReaderMode mode=AWordReaderMode.Normal;
		public AWordReader(LinearLetterReader lreader):base(lreader){}
		public bool ReadNext_modeId(){
			AWordReaderMode mode0=this.mode;
			this.mode=AWordReaderMode.ReadIdentifier;
			bool r=this.ReadNext();
			this.mode=mode0;
			return r;
		}
		public override bool ReadNext(){
		#if MACRO_WORDREADER
			this.cword="";
			this.wtype=WordType.Invalid;
			while([type].IsInvalid||[type].IsSpace)[if!next]return false;
			this.lreader.StoreCurrentPos(0);
			switch([type].purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
		#endif
			#region #OUT#
			this.cword="";
			this.wtype=WordType.Invalid;
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace)if(!this.lreader.MoveNext())return false;
			this.lreader.StoreCurrentPos(0);
			switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
			#endregion #OUT#
		}
		private void readOperator(){
			this.wtype=WordType.Operator;
		#if MACRO_WORDREADER
			switch([letter]){
				//-------------------------------
				//		演算子
				//-------------------------------
				case '→':case '↓': // 単独演算子
					[add][nexit]
				case '!':case '~':case '=': // 後に = が来る演算子
					[add][next]
					if([is:=]){[add][nexit]}
					[error:"今の所 "+this.cword+" は、後に = が来る場合にしか対応していません。"]
					this.cword+="=";
					break;
				case '<':case '>': // 後に = が来るかも知れない演算子
					[add][next]
					if([not:=])return;
					goto case ',';
				//-------------------------------
				//		他
				//-------------------------------
				case '@':this.readIdentifier();break;
				case '\'':
					this.ReadStringSQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '"':
					this.ReadStringDQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '{':
					if(this.mode!=AWordReaderMode.ReadIdentifier)goto default;
					this.ReadBracketRange();
					break;
				case ',':case '&':case '|':case ']':
					[add][nexit]
				case '.':
					[add][next]
					if([type].IsNumber){
						this.cword="";
						this.lreader.MoveToPos(0);
						this.ReadNumber();
					}
					this.readModifiers();
					break;
				default:
					this.wtype=WordType.Invalid;
					[error:[letter]+"は対応していない記号です。無視します。"]
					[add][nexit]
			}
		#endif
			#region #OUT#
			switch(this.lreader.CurrentLetter){
				//-------------------------------
				//		演算子
				//-------------------------------
				case '→':case '↓': // 単独演算子
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '!':case '~':case '=': // 後に = が来る演算子
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter=='='){this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;}
					this.lreader.SetError("今の所 "+this.cword+" は、後に = が来る場合にしか対応していません。",0,null);
					this.cword+="=";
					break;
				case '<':case '>': // 後に = が来るかも知れない演算子
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!='=')return;
					goto case ',';
				//-------------------------------
				//		他
				//-------------------------------
				case '@':this.readIdentifier();break;
				case '\'':
					this.ReadStringSQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '"':
					this.ReadStringDQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '{':
					if(this.mode!=AWordReaderMode.ReadIdentifier)goto default;
					this.ReadBracketRange();
					break;
				case ',':case '&':case '|':case ']':
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '.':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentType.IsNumber){
						this.cword="";
						this.lreader.MoveToPos(0);
						this.ReadNumber();
					}
					this.readModifiers();
					break;
				default:
					this.wtype=WordType.Invalid;
					this.lreader.SetError(this.lreader.CurrentLetter+"は対応していない記号です。無視します。",0,null);
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
			}
			#endregion #OUT#
		}
		private string convertStringLiteral(string literal){
			return literal.Length<=2?""
				:literal
				.Substring(1,literal.Length-2)
				.Replace(@"\r","\r")
				.Replace(@"\n","\n")
				.Replace(@"\t","\t")
				.Replace(@"\v","\v")
				.Replace(@"\\","\\");
		}
		/// <summary>
		/// Selector 用の識別子を読み取ります。
		/// \ をエスケープ文字として使用出来ます。
		/// 呼び出し条件{
		///		文字: 識別子の文字|@
		///		位置: 一文字目の文字
		/// }
		/// </summary>
		private void readIdentifier(){
			this.wtype=WordType.Identifier;
		#if MACRO_WORDREADER
			[add][next]
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					if([is:_]||[is:-])goto case LetterType.P_Token;
					if([not:\\])goto default;
					[next]
					goto case LetterType.P_Token;
				default:
					return;
			}
		#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					if(this.lreader.CurrentLetter=='_'||this.lreader.CurrentLetter=='-')goto case LetterType.P_Token;
					if(this.lreader.CurrentLetter!='\\')goto default;
					if(!this.lreader.MoveNext())return;
					goto case LetterType.P_Token;
				default:
					return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// 属性セレクタなどの修飾子を読み取ります。
		/// 呼び出し条件{
		///		文字: . の次の文字
		///		文字列: "."
		/// }
		/// </summary>
		private void readModifiers(){
			this.wtype=WordType.Modifier;
			bool bra=false;
		#if MACRO_WORDREADER
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					switch([letter]){
						case '[':
							bra=true;
							break;
						case ']':
							if(!bra)return;
							bra=false;
							break;
						case '=':case '~':case '!':case '&':case '↓':case '→':
						case '|':case '<':case '>':case ',':
							return;
					}
					[add][next]
					break;
				default:
					[next]
					break;
			}
		#endif
			#region #OUT#
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					switch(this.lreader.CurrentLetter){
						case '[':
							bra=true;
							break;
						case ']':
							if(!bra)return;
							bra=false;
							break;
						case '=':case '~':case '!':case '&':case '↓':case '→':
						case '|':case '<':case '>':case ',':
							return;
					}
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				default:
					if(!this.lreader.MoveNext())return;
					break;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// {} で囲まれた領域を読み取ります。
		/// 呼び出し前{文字位置:'{';}
		/// </summary>
		private void ReadBracketRange(){
			this.wtype=WordType.Element;
#if MACRO_WORDREADER
			[if!next]goto err;
			int count=0;
			while(true)switch([letter]){
				case '{':
					count++;
					goto default;
				case '}':
					count--;
					if(count<0){
						this.wtype=WordType.Element;
						[nexit]
					}
					goto default;
				case '\'':this.ReadStringSQ();break;
				case '"':this.ReadStringDQ();break;
				default:
					[add][if!next]goto err;
					break;
			}
		err:[error:"{} 領域に終端の } が存在しません。"]
#endif
			#region #OUT#
			if(!this.lreader.MoveNext())goto err;
			int count=0;
			while(true)switch(this.lreader.CurrentLetter){
				case '{':
					count++;
					goto default;
				case '}':
					count--;
					if(count<0){
						this.wtype=WordType.Element;
						this.lreader.MoveNext();return;
					}
					goto default;
				case '\'':this.ReadStringSQ();break;
				case '"':this.ReadStringDQ();break;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		err:this.lreader.SetError("{} 領域に終端の } が存在しません。",0,null);
			#endregion #OUT#
		}

	}
	#endregion

}