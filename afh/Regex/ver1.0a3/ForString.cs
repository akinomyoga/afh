#define ONE_NODE_ONE_TESTER

using Gen=System.Collections.Generic;
using BaseRegexFactory=afh.RegularExpressions.RegexFactory3<char>;
using RangeErrorPair=System.Collections.Generic.KeyValuePair<
	afh.Parse.LinearLetterReader.TextRange,
	afh.Parse.AnalyzeError
>;

namespace afh.RegularExpressions{
	/// <summary>
	/// 通常の文字列に対する正規表現を提供します。
	/// </summary>
	public partial class StringRegex:BaseRegexFactory{

		static StringRegex instance=new StringRegex();
		/// <summary>
		/// StringRegex の基本インスタンスを取得します。
		/// </summary>
		public static StringRegex Instance{
			get{return instance;}
		}

		private Parser parser;
		private Grammar grammar;
		/// <summary>
		/// 新しい文字列用正規表現を作成します。
		/// 正規表現の指定の仕方を自分の好みに合わせて変更したい場合などに使用します。
		/// 通常は、StringRegex.Instance を使用して下さい。
		/// </summary>
		public StringRegex(){
			this.grammar=new Grammar();
			this.parser=new Parser(this);
		}
#if OBSOLETE
		/// <summary>
		/// 指定した文字列が INode に一致するか否かを判定します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		[System.Obsolete]
		public bool IsMatching(string text,INode node){
			return base.IsMatching(new StringStreamAdapter(text,0),node);
		}
#endif
		/// <summary>
		/// 正規表現文字列に対応する正規言語を作成します。
		/// </summary>
		/// <param name="regularExpression">正規表現文字列を指定します。</param>
		/// <returns>指定した正規表現文字列から作成した正規言語を返します。</returns>
		public RegLan CreateLanguage(string regularExpression){
			return new RegLan(regularExpression);
		}
		/// <summary>
		/// 正規表現文字列を解析します。
		/// </summary>
		/// <param name="regularExpression">正規表現文字列を指定します。</param>
		/// <returns>解析結果に関する情報を返します。</returns>
		protected ParseResult Parse(string regularExpression){
			ParseResult ret;
			ret.node=this.parser.Parse(regularExpression);
			ret.errors=afh.Collections.Enumerable.From(this.parser.Errors).ToArray();
			return ret;
		}
		/// <summary>
		/// 正規表現文字列の解析結果を格納する構造体です。
		/// </summary>
		protected struct ParseResult{
			/// <summary>
			/// 正規表現のルート要素を保持します。
			/// </summary>
			public INode node;
			/// <summary>
			/// 正規表現の読み取り時に起こった例外についての情報を保持します。
			/// </summary>
			public RangeErrorPair[] errors;
		}

		#region CustomNodes
#if Ver1_0a2
		[System.Obsolete]
		private sealed class CharEqualsNode:ElemEqualsNode{
			public CharEqualsNode(char letter)
				:base(Grammar.CharToSource(letter),letter){}
		}
#endif
		private static class Nodes{
			//--------------------------------------------------------
			//		パターン
			//--------------------------------------------------------
			/// <summary>
			/// " に依って囲まれた文字列 /"(?:[^\\"]|\\.)*"/ に一致します。
			/// \ はエスケープ文字です。\ の後の文字は何でも (改行でも) 読み取ります。
			/// </summary>
			public static readonly FunctionNode QuotedString=new FunctionNode(":\"",delegate(ITypedStream<char> t){
				if(t.EndOfStream||t.Current!='"')return false;

				t.MoveNext();
				while(!t.EndOfStream){
					switch(t.Current){
						case '\r':
						case '\n':
						case '\f':
							return false;
						case '\\':
							t.MoveNext();
							if(t.EndOfStream)return false;
							break;
						case '"':
							t.MoveNext();
							return true;
					}
					t.MoveNext();
				}
				return false;
			});
			public static readonly FunctionNode LineBreak=new FunctionNode(":n",procLineBreak);
			public static readonly FunctionNode Word=new FunctionNode(":w",delegate(ITypedStream<char> t){
				if(!t.IsStart&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym))return false;

				// 一文字目
				if(t.EndOfStream||!afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csymf))return false;
				t.MoveNext();

				while(!t.EndOfStream&&afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csym))t.MoveNext();
				return true;
			});
			public static readonly FunctionNode WordUnicode=new FunctionNode(":ｗ",delegate(ITypedStream<char> t){
				if(!t.IsStart&&IsUnicodeWord(t.Previous))return false;

				// 一文字目
				if(t.EndOfStream)return false;
				char c=t.Current;
				if(!IsUnicodeWord(c)||'0'<=c&&c<='9')return false;
				t.MoveNext();

				while(!t.EndOfStream&&IsUnicodeWord(t.Current))t.MoveNext();
				return true;
			});

			private static bool procLineBreak(ITypedStream<char> t){
				if(t.Current=='\r'){
					t.MoveNext();
					if(t.Current=='\n')
						t.MoveNext();
					return true;
				}
				
				if(t.Current=='\n'){
					t.MoveNext();
					return true;
				}

				return false;
			}
			private static bool isUnicodeSpace_NotLineBreak(char c){
				return "\t\v\x85".IndexOf(c)>=0
						||afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Z);
			}

			// \b|[ \t\v]+
			public static readonly FunctionNode WordBlanks=new FunctionNode(":b",delegate(ITypedStream<char> t){
				bool isborder=procWordBorder(t);

				// 一文字目
				if(t.EndOfStream)return isborder;
				char c=t.Current;
				if(!(c==' '||c=='\t'||c=='\v'))return isborder;
				t.MoveNext();c=t.Current;

				while(!t.EndOfStream&&(c==' '||c=='\t'||c=='\v')){
					t.MoveNext();c=t.Current;
				}
				return true;
			});
			// \ｂ|[\t\v\x{85}\p{Z}]+
			public static readonly FunctionNode WordBlanksUnicode=new FunctionNode(":ｂ",delegate(ITypedStream<char> t){
				bool isborder=procWordBorderUnicode(t);

				// 一文字目
				if(t.EndOfStream) return isborder;
				char c=t.Current;
				if(!isUnicodeSpace_NotLineBreak(t.Current))return isborder;
				t.MoveNext();

				while(!t.EndOfStream&&isUnicodeSpace_NotLineBreak(t.Current)){
					t.MoveNext();
				}
				return true;
			});
			//--------------------------------------------------------
			//		アンカー
			//--------------------------------------------------------
			public static readonly FunctionNode Start=new FunctionNode(@"\A",delegate(ITypedStream<char> t){
				return t.IsStart;
			});
			public static readonly FunctionNode End=new FunctionNode(@"\Z",delegate(ITypedStream<char> t){
				procLineBreak(t);
				return t.EndOfStream;
			});
			public static readonly FunctionNode EndExact=new FunctionNode(@"\z",delegate(ITypedStream<char> t){
				return t.EndOfStream;
			});
			public static readonly FunctionNode WordBorder=new FunctionNode(@"\b",procWordBorder);
			public static readonly FunctionNode NotWordBorder=new FunctionNode(@"\B",procNotWordBorder);
			public static readonly FunctionNode WordBorderUnicode=new FunctionNode(@"\ｂ",procWordBorderUnicode);
			public static readonly FunctionNode NotWordBorderUnicode=new FunctionNode(@"\Ｂ",procNotWordBorderUnicode);

			private static bool procWordBorder(ITypedStream<char> t){
				bool l=!t.IsStart&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
				bool r=!t.EndOfStream&&afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csym);
				return l!=r;
			}
			private static bool procWordBorderUnicode(ITypedStream<char> t){
				bool l=!t.IsStart&&IsUnicodeWord(t.Previous);
				bool r=!t.EndOfStream&&IsUnicodeWord(t.Current);
				return l!=r;
			}
			private static bool procNotWordBorder(ITypedStream<char> t){
				bool l=!t.IsStart&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
				bool r=!t.EndOfStream&&afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csym);
				return l==r;
			}
			private static bool procNotWordBorderUnicode(ITypedStream<char> t){
				bool l=!t.IsStart&&IsUnicodeWord(t.Previous);
				bool r=!t.EndOfStream&&IsUnicodeWord(t.Current);
				return l==r;
			}
		}
#if OBSOLETE
		/// <summary>
		/// " に依って囲まれた文字列 /"(?:[^\\"]|\\.)*"/ に一致します。
		/// \ はエスケープ文字です。\ の後の文字は何でも (改行でも) 読み取ります。
		/// </summary>
		private sealed class QuotedStringNode:INode,ITester{
			private QuotedStringNode(){}
			private static readonly QuotedStringNode instance=new QuotedStringNode();
			/// <summary>
			/// QuotedStringNode の唯一のインスタンスを取得します。
			/// </summary>
			public static QuotedStringNode Instance{
				get{return instance;}
			}

			public override string ToString(){
				return ":\"";
			}
			//--------------------------------------------------------
			//		INode
			//--------------------------------------------------------
			public ITester GetTester(){return this;}

			public RegexFactory2<char>.NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			//--------------------------------------------------------
			//		ITester
			//--------------------------------------------------------
			public ITester Read(Status s){
				ITypedStream<char> target=s.Target;
				if(target.Current!='"')goto FAIL;

				target.MoveNext();
				while(!target.EndOfStream){
					switch(target.Current){
						case '\r':case '\n':case '\f':
							goto FAIL;
						case '\\':
							target.MoveNext();
							if(target.EndOfStream)goto FAIL;
							break;
						case '"':
							target.MoveNext();
							s.Success=true;
							return null;
					}
					target.MoveNext();
				}

			FAIL:
				s.Success=false;
				return null;
			}
			public bool Indefinite{
				get{return false;}
			}
			public RegexFactory2<char>.ITester Clone(){return this;}
		}
		private sealed class AnchorNode:INode,ITester{
			private enum TYPE{
				WordBorder,
				NotWordBorder,
				WordBorderUnicode,
				NotWordBorderUnicode,
			}
			TYPE type;
			private AnchorNode(TYPE type){
				this.type=type;
			}
			public static readonly AnchorNode WordBorder=new AnchorNode(TYPE.WordBorder);
			public static readonly AnchorNode NotWordBorder=new AnchorNode(TYPE.NotWordBorder);
			public static readonly AnchorNode WordBorderUnicode=new AnchorNode(TYPE.WordBorderUnicode);
			public static readonly AnchorNode NotWordBorderUnicode=new AnchorNode(TYPE.NotWordBorderUnicode);
			//--------------------------------------------------------
			//		INode
			//--------------------------------------------------------
			public RegexFactory2<char>.ITester GetTester(){return this;}
			public RegexFactory2<char>.NodeAssociativity Associativity{
				get{return RegexFactory2<char>.NodeAssociativity.Strong;}
			}
			public System.Collections.Generic.IEnumerable<RegexFactory2<char>.INode> EnumChildren {
				get{return EmptyNodes;}
			}
			//--------------------------------------------------------
			//		ITester
			//--------------------------------------------------------
			public RegexFactory2<char>.ITester Read(Status s){
				ITypedStream<char> t=s.Target;
				switch(type){
					case TYPE.WordBorder:{
						bool l=!t.IsStart&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
						bool r=!t.EndOfStream&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
						s.Success=l!=r;
						break;
					}
					case TYPE.NotWordBorder:{
						bool l=!t.IsStart&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
						bool r=!t.EndOfStream&&afh.Text.CharUtils.Is(t.Previous,afh.Text.CLangCType.csym);
						s.Success=l==r;
						break;
					}
					case TYPE.WordBorderUnicode:{
						bool l=!t.IsStart&&IsUnicodeWord(t.Previous);
						bool r=!t.EndOfStream&&IsUnicodeWord(t.Previous);
						s.Success=l!=r;
						break;
					}
					case TYPE.NotWordBorderUnicode:{
						bool l=!t.IsStart&&IsUnicodeWord(t.Previous);
						bool r=!t.EndOfStream&&IsUnicodeWord(t.Previous);
						s.Success=l==r;
						break;
					}
				}
				return null;
			}
			public bool Indefinite{get{return false;}}
			public RegexFactory2<char>.ITester Clone(){
				throw new System.Exception("The method or operation is not implemented.");
			}
		}
#endif
		#endregion

		#region API - Capture, RegLan, etc.
		/// <summary>
		/// 一致した部分列に対する情報を提供します。
		/// </summary>
		public class Capture:CaptureBase<Capture>{
			internal Capture(Capture capture,int index)
				:base(capture,index){}
			internal Capture(Evaluator eval):base(eval){}
			/// <summary>
			/// 基底クラスから利用する為の関数です。
			/// 指定した番号に対応する子キャプチャ部分を作成します。
			/// </summary>
			/// <param name="index">子キャプチャの番号を指定します。
			/// この番号は MatchData に保持している List&lt;ICaptureRange&gt; 内の番号です。</param>
			/// <returns>作成した Capture インスタンスを返します。</returns>
			protected internal sealed override Capture CreateCapture(int index){
				return new Capture(this,index);
			}
			/// <summary>
			/// 一致した部分文字列を取得します。
			/// </summary>
			public string Value{
				get{
					StringStreamAdapter str=this.TargetStream as StringStreamAdapter;
					if(str!=null)
						return str.Substr(this.Start,this.End);

					ArrayStreamAdapter<char> arr=this.TargetStream as ArrayStreamAdapter<char>;
					if(arr!=null)
						return new string(arr.Array,this.Start,this.Length);

					return new string(this.ToArray());
				}
			}
			/// <summary>
			/// このインスタンスを文字列に変換します。
			/// 一致した部分文字列を返します。
			/// </summary>
			/// <returns>一致した部分文字列を返します。
			/// もし一致した内容がない場合には "&lt;null&gt;" という内容を返します。</returns>
			public sealed override string ToString(){
				return this.Value??"<null>";
			}
			/// <summary>
			/// 文字列に変換します。一致した部分文字列に変換されます。
			/// </summary>
			/// <param name="c">変換元の Capture インスタンスを返します。</param>
			/// <returns>変換後の一致文字列を返します。</returns>
			public static implicit operator string(Capture c){
				return c.Value;
			}
		}

		/*
		/// <summary>
		/// 一致した部分列に対する情報を提供し、置換後の情報を受け取る為に使用します。
		/// </summary>
		public sealed class ReplaceCapture:Capture{
			internal ReplaceCapture(Capture capture,int index)
				:base(capture,index){}
			internal ReplaceCapture(Evaluator eval):base(eval){}

			private string replaced=null;
			/// <summary>
			/// 置換後の値を指定します。
			/// </summary>
			public string NewValue{
				get{return this.replaced;}
				set{this.replaced=value;}
			}
		}
		//*/

		/// <summary>
		/// 正規表現によって表される言語 (文字列集合) を表現します。
		/// </summary>
		public class RegLan{
			INode node;
			private bool overlap_search=false;
			/// <summary>
			/// 一度マッチした部分を続けてマッチさせるか否かを取得又は設定します。
			/// </summary>
			public bool OverlapSearch{
				get{return this.overlap_search;}
				set{this.overlap_search=value;}
			}

			/// <summary>
			/// 既定の StringRegex を使用して RegLan を初期化します。
			/// </summary>
			/// <param name="regex">この正規言語を規定する正規表現を指定します。</param>
			public RegLan(string regex){
				ParseResult res=StringRegex.Instance.Parse(regex);
				this.node=res.node;
				if(res.errors.Length>0){
					System.Text.StringBuilder build=new System.Text.StringBuilder();
					build.Append("正規表現の構文解析中にエラーが発生しました。\r\n");
					foreach(RangeErrorPair p in res.errors){
						build.AppendFormat("ParseError: {0}-{1}\r\n\t{2}\r\n",p.Key.start,p.Key.end,p.Value.message);
					}
					throw new System.FormatException(build.ToString());
				}
			}
			internal RegLan(INode node){
				this.node=node;
			}

			/// <summary>
			/// このインスタンスの内容を文字列として取得します。
			/// </summary>
			/// <returns>この正規言語を表現する正規表現を返します。</returns>
			public override string ToString(){
				return this.node.ToString();
			}

			//========================================================
			//		Replace
			//========================================================
			/// <summary>
			/// 置換をする為の評価を行う関数の型です。
			/// </summary>
			/// <param name="c">置換の対象となる部分列を指定します。</param>
			/// <returns>置換後の内容を返します。</returns>
			public delegate string ReplaceProc(Capture c);
			/// <summary>
			/// 文字列の置換を行います。
			/// </summary>
			/// <param name="text">置換の対象の文字列を格納している文字列を指定しています。</param>
			/// <param name="p">置換処理を行う関数を指定します。</param>
			/// <returns>置換をした結果の文字列を返します。</returns>
			public string Replace(string text,ReplaceProc p) {
				return this.Replace(new StringStreamAdapter(text),p);
			}
			/// <summary>
			/// 文字列の置換を行います。
			/// </summary>
			/// <param name="data">置換の対象の文字列を格納している配列を指定しています。</param>
			/// <param name="p">置換処理を行う関数を指定します。</param>
			/// <returns>置換をした結果の文字列を返します。</returns>
			public string Replace(char[] data,ReplaceProc p) {
				return this.Replace(new ArrayStreamAdapter<char>(data),p);
			}
			/// <summary>
			/// 文字列の置換を行います。
			/// </summary>
			/// <param name="str">置換の対象の文字列を格納している Stream を指定しています。</param>
			/// <param name="p">置換処理を行う関数を指定します。</param>
			/// <returns>置換をした結果の文字列を返します。</returns>
			public string Replace(ITypedStream<char> str,ReplaceProc p){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,node);
				eval.OverlapSearch=false;

				int i=0;
				System.Text.StringBuilder b=new System.Text.StringBuilder();
				while(eval.Match()){
					Capture rep=new Capture(eval);
					if(i<rep.Start)
						AppendSubstring(b,str,i,rep.Start);

					b.Append(p(rep));

					i=rep.End;
				}

				// 残りを全て追加
				AppendSubstring(b,str,i);

				return b.ToString();
			}
			internal static string Substring(ITypedStream<char> str,int start,int end){
				StringStreamAdapter strstr=str as StringStreamAdapter;
				if(strstr!=null)
					return strstr.Substr(start,end);

				ArrayStreamAdapter<char> arr=str as ArrayStreamAdapter<char>;
				if(arr!=null)
					return new string(arr.Array,start,end-start);

				// 一文字ずつ読み出し
				int index=str.Index;
				str.Index=start;
				System.Text.StringBuilder b=new System.Text.StringBuilder(end-start);
				do{
					b.Append(str.Current);
					str.MoveNext();
				}while(!str.EndOfStream&&str.Index<end);
				str.Index=index;

				return b.ToString();
			}
			/// <summary>
			/// 指定した範囲の内容を、StringBuilder に追加します。
			/// </summary>
			/// <param name="build">書き込み先のバッファを指定します。</param>
			/// <param name="str">書き込む内容を保持している Stream を指定します。</param>
			/// <param name="start">書き込む内容の開始位置を指定します。</param>
			/// <param name="end">書き込む内容の終了位置を指定します。此処に指定した位置の文字は追加されません。</param>
			internal static void AppendSubstring(System.Text.StringBuilder build,ITypedStream<char> str,int start,int end){
				StringStreamAdapter strstr=str as StringStreamAdapter;
				if(strstr!=null)
					build.Append(strstr.ContentText,start,end-start);

				ArrayStreamAdapter<char> arr=str as ArrayStreamAdapter<char>;
				if(arr!=null)
					build.Append(arr.Array,start,end-start);

				// 一文字ずつ読み出し
				int index=str.Index;
				str.Index=start;
				do{
					build.Append(str.Current);
				}while(!str.EndOfStream&&str.Index<end);
				str.Index=index;
			}
			/// <summary>
			/// 指定した位置から末端までの内容を、StringBuilder に追加します。
			/// </summary>
			/// <param name="build">書き込み先のバッファを指定します。</param>
			/// <param name="str">書き込む内容を保持している Stream を指定します。</param>
			/// <param name="start">書き込む内容の開始位置を指定します。</param>
			internal static void AppendSubstring(System.Text.StringBuilder build,ITypedStream<char> str,int start){
				StringStreamAdapter strstr=str as StringStreamAdapter;
				if(strstr!=null){
					string content=strstr.ContentText;
					build.Append(content,start,content.Length-start);
				}

				ArrayStreamAdapter<char> arr=str as ArrayStreamAdapter<char>;
				if(arr!=null){
					char[] array=arr.Array;
					build.Append(array,start,array.Length-start);
				}

				// 一文字ずつ読み出し
				int index=str.Index;
				str.Index=start;
				do{
					build.Append(str.Current);
					str.MoveNext();
				}while(!str.EndOfStream);
				str.Index=index;
			}
			//========================================================
			//		IsMatching
			//========================================================
			/// <summary>
			/// 文字列の中にパターンに一致する部分が存在するか否かを判定します。
			/// </summary>
			/// <param name="text">検索対象の文字列を指定します。</param>
			/// <returns>パターンに一致する部分が見つかった場合に true を返します。
			/// 見つからなかった場合に false を返します。</returns>
			public bool IsMatching(string text){
				return this.IsMatching(new StringStreamAdapter(text));
			}
			/// <summary>
			/// 文字列の中にパターンに一致する部分が存在するか否かを判定します。
			/// </summary>
			/// <param name="data">検索対象の文字配列を指定します。</param>
			/// <returns>パターンに一致する部分が見つかった場合に true を返します。
			/// 見つからなかった場合に false を返します。</returns>
			public bool IsMatching(char[] data){
				return this.IsMatching(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// 文字列の中にパターンに一致する部分が存在するか否かを判定します。
			/// </summary>
			/// <param name="str">検索対象の文字ストリームを指定します。</param>
			/// <returns>パターンに一致する部分が見つかった場合に true を返します。
			/// 見つからなかった場合に false を返します。</returns>
			public bool IsMatching(ITypedStream<char> str){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,this.node);
				return eval.Match();
			}
			//========================================================
			//		IsMatching
			//========================================================
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を検索します。
			/// </summary>
			/// <param name="text">検索対象の文字列を指定します。</param>
			/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
			/// 一致する部分列が見つからなかった場合には null を返します。</returns>
			public Capture Match(string text){
				return this.Match(new StringStreamAdapter(text));
			}
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を検索します。
			/// </summary>
			/// <param name="data">検索対象の文字配列を指定します。</param>
			/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
			/// 一致する部分列が見つからなかった場合には null を返します。</returns>
			public Capture Match(char[] data){
				return this.Match(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を検索します。
			/// </summary>
			/// <param name="str">検索対象の文字ストリームを指定します。</param>
			/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
			/// 一致する部分列が見つからなかった場合には null を返します。</returns>
			public Capture Match(ITypedStream<char> str){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,this.node);
				if(eval.Match())
					return new Capture(eval);
				else
					return null;
			}
			//========================================================
			//		Matches
			//========================================================
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を複数検索します。
			/// </summary>
			/// <param name="text">検索対象の文字列を指定します。</param>
			/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
			public Gen::IEnumerable<Capture> Matches(string text){
				return this.Matches(new StringStreamAdapter(text));
			}
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を複数検索します。
			/// </summary>
			/// <param name="data">検索対象の文字配列を指定します。</param>
			/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
			public Gen::IEnumerable<Capture> Matches(char[] data){
				return this.Matches(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// 文字列の中からパターンに一致する部分列を複数検索します。
			/// </summary>
			/// <param name="str">検索対象の文字ストリームを指定します。</param>
			/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
			public Gen::IEnumerable<Capture> Matches(ITypedStream<char> str){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,node);
				eval.OverlapSearch=this.overlap_search;
				while(eval.Match())
					yield return new Capture(eval);
			}
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を複数検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="text">検索対象の文字列を指定します。</param>
		/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,string text) {
			return new RegLan(regex).Matches(text);
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を複数検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="data">検索対象の文字配列を指定します。</param>
		/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,char[] data){
			return new RegLan(regex).Matches(data);
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を複数検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="str">検索対象の文字ストリームを指定します。</param>
		/// <returns>一致部分列を列挙する列挙可能体を返します。</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,ITypedStream<char> str){
			return new RegLan(regex).Matches(str);
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="text">検索対象の文字列を指定します。</param>
		/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
		/// 一致する部分列が見つからなかった場合には null を返します。</returns>
		public static Capture Match(string regex,string text){
			return new RegLan(regex).Match(text);
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="data">検索対象の文字配列を指定します。</param>
		/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
		/// 一致する部分列が見つからなかった場合には null を返します。</returns>
		public static Capture Match(string regex,char[] data) {
			return new RegLan(regex).Match(data);
		}
		/// <summary>
		/// 文字列の中からパターンに一致する部分列を検索します。
		/// </summary>
		/// <param name="regex">パターンを規定する正規表現を指定します。</param>
		/// <param name="str">検索対象の文字ストリームを指定します。</param>
		/// <returns>一致する部分列が見つかった場合には、その情報を保持する Capture を返します。
		/// 一致する部分列が見つからなかった場合には null を返します。</returns>
		public static Capture Match(string regex,ITypedStream<char> str) {
			return new RegLan(regex).Match(str);
		}
		#endregion

		private class Grammar:IRegexGrammar{
			internal const string CONTROL_LETTERS=CharClasses.CONTROL_LETTERS;
			const string COMMAND_LETTERS_SPACE=CharClasses.COMMAND_LETTERS_SPACE;
			const string SPACE_LETTERS=CharClasses.SPACE_LETTERS;

			private Gen::Dictionary<string,CommandData> cmd
				=new Gen::Dictionary<string,CommandData>();
			public Gen::Dictionary<string,CommandData> CommandSet{
				get{return cmd;}
			}

			private Gen::Dictionary<string,CommandData> cmdc
				=new Gen::Dictionary<string,CommandData>();
			public Gen::Dictionary<string,CommandData> ColonCommandSet{
				get{return cmdc;}
			}

			public Grammar(){
				CommandData NOARG=CommandData.NoArg;
				CommandData BRACE=CommandData.Brace;

				// cmddic の初期化
				string letters=CONTROL_LETTERS+COMMAND_LETTERS_SPACE;
				for(int i=0;i<letters.Length;i++){
					cmd[letters[i].ToString()]=NOARG;
				}
			}

			public void RegisterCommand(string name,CommandData dat){
				this.cmd[name]=dat;
			}
			public void RegisterCommandC(string name,CommandData dat){
				this.cmdc[name]=dat;
			}

			public bool EnablesColonCommand{
				get{return true;}
			}

			/// <summary>
			/// 指定した文字を正規表現で表現します。
			/// </summary>
			/// <param name="letter">正規表現に変換したい文字を指定します。</param>
			/// <returns>文字を正規表現にして返します。
			/// 文字が特別なエスケープ表現を持っている場合には、そのエスケープ表現で返します。
			/// それ以外の場合にはその儘返します。
			/// </returns>
			public static string CharToSource(char letter){
				if(CONTROL_LETTERS.IndexOf(letter)>=0)
					return "\\"+letter;

				// 空白系の文字
				int i=SPACE_LETTERS.IndexOf(letter);
				if(i>=0&&letter!='\b')
					return "\\"+COMMAND_LETTERS_SPACE[i];

				return letter.ToString();
			}
		}
		private static bool IsUnicodeWord(char c){
			afh.Text.GeneralCategory cat=afh.Text.CharUtils.GetGeneralCategory(c);
			return afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.L)
				||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Nd)
				||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Pc);
		}
		/// <summary>
		/// 正規表現文字列を解析するクラスです。
		/// </summary>
		protected sealed class Parser:ParserBase{
			private readonly Grammar grammar;
			/// <summary>
			/// Parser のインスタンスを初期化します。
			/// </summary>
			/// <param name="factory">正規表現を定義する StringRegex を指定します。</param>
			public Parser(StringRegex factory):base(factory){
				this.grammar=((StringRegex)this.parent).grammar;
				this.InitializeCommand();
			}

			/// <summary>
			/// この正規表現の文法を定義する IRegexGrammar インスタンスを取得します。
			/// </summary>
			/// <returns>この正規表現の文法を定義している RegexGrammar インスタンスを返します。</returns>
			protected override IRegexGrammar GetGrammar(){
				return this.grammar;
			}
			private readonly Gen::Dictionary<string,INode> command_nodes=new Gen::Dictionary<string,INode>();

			private void RegisterCommand(string name,INode node){
				this.grammar.RegisterCommand(name,CommandData.NoArg);
				this.command_nodes[name]=node;
			}
			private void InitializeCommand(){
				//------------------------------------------------
				//		エスケープ
				//------------------------------------------------
				for(int i=0;i<Grammar.CONTROL_LETTERS.Length;i++){
					command_nodes[Grammar.CONTROL_LETTERS[i].ToString()]
						=CreateCharacterNode(Grammar.CONTROL_LETTERS[i]);
				}
				//------------------------------------------------
				//		特別の文字
				//------------------------------------------------
				command_nodes["t"]=CreateCharacterNode('\t');
				command_nodes["v"]=CreateCharacterNode('\v');
				command_nodes["n"]=CreateCharacterNode('\n');
				command_nodes["r"]=CreateCharacterNode('\r');
				//command_nodes["b"]=CreateCharacterNode('\b');
				command_nodes["f"]=CreateCharacterNode('\f');
				command_nodes["a"]=CreateCharacterNode('\a');
				//------------------------------------------------
				//		ANSI 文字クラス (ECMAScript 互換)
				//------------------------------------------------
				RegisterCommand("w",new FunctionElemNode(@"\w",delegate(char c){
					return 'a'<=c&&c<='z'||'A'<=c&&c<='Z'||'0'<=c&&c<='9'||c=='_';
				}));
				RegisterCommand("W",new FunctionElemNode(@"\W",delegate(char c){
					return !('a'<=c&&c<='z'||'A'<=c&&c<='Z'||'0'<=c&&c<='9'||c=='_');
				}));
				RegisterCommand("d",new FunctionElemNode(@"\d",delegate(char c){
					return '0'<=c&&c<='9';
				}));
				RegisterCommand("D",new FunctionElemNode(@"\D",delegate(char c){
					return !('0'<=c&&c<='9');
				}));
				RegisterCommand("s",new FunctionElemNode(@"\s",delegate(char c){
					return c==' '||c=='\f'||c=='\n'||c=='\r'||c=='\t'||c=='\v';
				}));
				RegisterCommand("S",new FunctionElemNode(@"\S",delegate(char c){
					return !(c==' '||c=='\f'||c=='\n'||c=='\r'||c=='\t'||c=='\v');
				}));
				//------------------------------------------------
				//		Unicode 文字クラス
				//------------------------------------------------
				RegisterCommand("ｗ",new FunctionElemNode(@"\ｗ",IsUnicodeWord));
				RegisterCommand("Ｗ",new FunctionElemNode(@"\Ｗ",delegate(char c){
					afh.Text.GeneralCategory cat=afh.Text.CharUtils.GetGeneralCategory(c);
					return !(afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.L)
						||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Nd)
						||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Pc));
				}));
				RegisterCommand("ｓ",new FunctionElemNode(@"\ｓ",delegate(char c){
					return "\f\n\r\t\v\x85".IndexOf(c)>=0
						||afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Z);
				}));
				RegisterCommand("Ｓ",new FunctionElemNode(@"\Ｓ",delegate(char c){
					return !("\f\n\r\t\v\x85".IndexOf(c)>=0
						||afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Z));
				}));
				RegisterCommand("ｄ",new FunctionElemNode(@"\ｄ",delegate(char c){
					return afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Nd);
				}));
				RegisterCommand("Ｄ",new FunctionElemNode(@"\Ｄ",delegate(char c) {
					return !afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Nd);
				}));
				//------------------------------------------------
				//		アンカー
				//------------------------------------------------
				RegisterCommand("b",Nodes.WordBorder);
				RegisterCommand("B",Nodes.NotWordBorder);
				RegisterCommand("ｂ",Nodes.WordBorderUnicode);
				RegisterCommand("Ｂ",Nodes.NotWordBorderUnicode);
				RegisterCommand("A",Nodes.Start);
				RegisterCommand("Z",Nodes.End);
				RegisterCommand("z",Nodes.EndExact);
				//------------------------------------------------
				//		他 (ProcessCommand 内に Node 定義)
				//------------------------------------------------
				this.grammar.RegisterCommand("p",CommandData.Brace);
				this.grammar.RegisterCommand("P",CommandData.Brace);
				this.grammar.RegisterCommandC("\"",CommandData.NoArg);
				this.grammar.RegisterCommandC("n",CommandData.NoArg);
				this.grammar.RegisterCommandC("w",CommandData.NoArg);
				this.grammar.RegisterCommandC("ｗ",CommandData.NoArg);
				this.grammar.RegisterCommandC("b",CommandData.NoArg);
				this.grammar.RegisterCommandC("ｂ",CommandData.NoArg);
			}

			/// <summary>
			/// コマンドに対応する正規表現要素を作ります。
			/// </summary>
			/// <param name="name">コマンドの名前を指定します。</param>
			/// <param name="arg">コマンドの引数を指定します。</param>
			/// <returns>作成した正規表現要素を返します。</returns>
			protected override INode ProcessCommand(string name,string arg){
				// 辞書に登録済の物
				{
					INode node;
					if(command_nodes.TryGetValue(name,out node))return node;
				}
				switch(name){
					//------------------------------------------------
					//		名前付き文字クラス
					//------------------------------------------------
#if OBSOLETE
					case "p":{
						if(arg.StartsWith("Is"))arg=arg.Substring(2);
						INode node;
						if(!CharClasses.nodes_p.TryGetValue(arg.Replace("-","").ToLower(),out node)){
							throw new RegexParseException("指定した文字クラス '"+arg+"' は登録されていません。");
						}
						return node;
					}
					case "P":{
						if(arg.StartsWith("Is"))arg=arg.Substring(2);
						INode node;
						if(!CharClasses.nodes_P.TryGetValue(arg.Replace("-","").ToLower(),out node)){
							throw new RegexParseException("指定した文字クラス '"+arg+"' は登録されていません。");
						}
						return node;
					}
#endif
					case "p":{
						CharClasses.CharClassInfo info=CharClasses.GetInfo(arg);
						if(info==null){
							throw new RegexParseException("指定した文字クラス '"+arg+"' は登録されていません。");
						}
						return info.node_pos;
					}
					case "P":{
						CharClasses.CharClassInfo info=CharClasses.GetInfo(arg);
						if(info==null){
							throw new RegexParseException("指定した文字クラス '"+arg+"' は登録されていません。");
						}
						return info.node_neg;
					}
					default:
						__debug__.RegexParserAssert(false);
						return null;
				}
			}
			/// <summary>
			/// コロンコマンドに対応する正規表現要素を作ります。
			/// </summary>
			/// <param name="name">コロンコマンドの名前を指定します。</param>
			/// <param name="arg">コロンコマンドの引数を指定します。</param>
			/// <returns>作成した正規表現要素を返します。</returns>
			protected override INode ProcessCommandC(string name,string arg) {
				switch(name){
					case "\"":return Nodes.QuotedString;
					case "n":return Nodes.LineBreak;
					case "w":return Nodes.Word;
					case "ｗ":return Nodes.WordUnicode;
					case "b":return Nodes.WordBlanks;
					case "ｂ":return Nodes.WordBlanksUnicode;
					default:
						__debug__.RegexParserAssert(false);
						return null;
				}
			}
			/// <summary>
			/// 文字から正規表現要素を作ります。
			/// </summary>
			/// <param name="letter">正規表現要素の元となる文字を指定します。</param>
			/// <returns>指定した文字を元に作成した正規表現要素を返します。</returns>
			protected override INode ProcessLetter(char letter){
				__debug__.RegexParserAssert(
					letter!='.'&&letter!='^'&&letter!='$',
					"呼び出し元で filter されている筈");

				return CreateCharacterNode(letter);
			}
			/// <summary>
			/// クラス表現から正規表現要素を作ります。
			/// </summary>
			/// <param name="content">クラス指定 [...] の中身を指定します。</param>
			/// <returns>指定した文字列を元に作成した正規表現要素を返します。</returns>
			protected override INode ProcessClass(string content){
				//return new CharClassReader(this,content).CreateNode();
				return new CharClasses.CharClassNodeGenerator(this.processClassError,content).CreateNode();
			}

			internal void processClassError(string msg,string expression,int index){
				this.Error(string.Format("文字クラス \"{0}\":{1} 文字目\r\n\t{2}",expression,index,msg));
			}

			/// <summary>
			/// 指定した文字に対応した INode インスタンスを作成します。
			/// </summary>
			/// <param name="ch">一致対象の文字を指定します。</param>
			/// <returns>指定した文字に一致する INode を作成して返します。</returns>
			public static INode CreateCharacterNode(char ch){
				return new EquivElemNode(Grammar.CharToSource(ch),ch);
			}
		}
#if OBSOLETE
		#region CharClassReader
		private class CharClassReader{
			int i;
			readonly Parser parent;
			readonly string expression;
			public CharClassReader(Parser parent,string expression){
				this.parent=parent;
				this.i=0;
				this.expression=expression;
			}
			public INode CreateNode(){
				this.i=0;
				Data d=this.ReadClass();
				return new FunctionElemNode("["+d.name+"]",d.proc);
			}

			private struct Data{
				/// <summary>
				/// 正規表現で表現した時の名前を保持します。
				/// </summary>
				public string name;
				public System.Predicate<char> proc;
				public Data(string name,System.Predicate<char> proc){
					this.name=name;
					this.proc=proc;
				}
			}

			private void ReportError(string msg){
				this.parent.processClassError(msg,expression,i);
			}
			private static string CharToName(char c){
				if(CONTROL_LETTERS.IndexOf(c)>=0)return @"\"+c;

				int index;
				if((index=SPACE_LETTERS.IndexOf(c))>=0)
					return @"\"+COMMAND_LETTERS_SPACE[index];

				return c.ToString();
			}
			//========================================================
			//		読み取り
			//========================================================
			private Data ReadClass(){
				bool mode;
				if(expression[i]=='^'){ // [^...]
					mode=false;
					i++;
				}else{ // [...]
					mode=true;
				}

				string class_name="";
				Gen::List<System.Predicate<char>> proc_list=new Gen::List<System.Predicate<char>>();
				Gen::List<bool> sub_list=new Gen::List<bool>();

				while(i<expression.Length){
					char c1,c2;
					if(ReadChar(out c1)){
						c2='\0';
					}else{
						c2=expression[i];
					}

					Data data;
					switch(c2){
						case '\0':
							// 通常の文字
							if(i>=expression.Length||expression[i]!='-'){
								goto one_char;
							}

							// 範囲指定 ?
							i++;if(i<expression.Length&&ReadChar(out c2)){
								goto range_char;
							}

							if(i<expression.Length&&expression[i]!='['){
								this.ReportError("文字範囲指定 「a-b」 の終端に当たる文字がありません。或いは - にエスケープが必要です。");
							}

							// やはり通常の文字
							i--;{							
								goto one_char;
							}
						case '\\':{
							i++;if(i>=expression.Length) {
								this.ReportError(@"\ に続く文字が必要です。");
								goto ED;
							}

							//-- 名前付き文字クラス
							if(c1=='p'||c1=='P'){
								data=this.CreateNamedClass();
								goto add_data;
							}else{
								this.ReportError("認識出来ないコマンドです。");
								i++;
							}
							break;
						}
						case '-':
							i++;
							if(i<expression.Length&&expression[i]=='['){
								i++;
								data=ReadClass();
								goto add_negative;
							}
							goto one_char;
						case ']':
							i++;
							goto ED;
						default:
							__debug__.RegexParserAssert(false);
							break;
						one_char: // c1 から生成
							data=CreateUnitChar(c1);
							goto add_data;
						range_char: // c1-c2 から生成
							data=CreateCharRange(c1,c2);
							goto add_data;
						add_data: // 文字クラス加算
							if(data.proc!=null){
								proc_list.Add(data.proc);
								class_name+=data.name;
								sub_list.Add(false);
							}
							break;
						add_negative: // 文字クラス減算
							if(data.proc!=null){
								proc_list.Add(data.proc);
								class_name+="-["+data.name+"]";
								sub_list.Add(true);
							}
							break;
					}
				}
			ED:
				return CreateClass(mode,class_name,proc_list.ToArray(),sub_list.ToArray());
			}
			/// <summary>
			/// 通常の文字を読み取れる場合には、その文字を読み取って true を返します。
			/// 通常の文字でない場合には、読み取らずに false を返します。
			/// </summary>
			/// <param name="expression"></param>
			/// <param name="i">文字を読み取った場合には、その次の文字の位置を指した状態で返します。</param>
			/// <param name="c"></param>
			/// <returns>通常の文字を読み取れた場合に true を返します。それ以外の場合に false を返します。</returns>
			private bool ReadChar(out char c){
				c=expression[i];

				switch(c){
					case '\\':{
						i++;if(i>=expression.Length){
							this.ReportError(@"\ に続く文字が必要です。");
							i--;
							return false;
						}

						//-- 特別の文字
						int index=COMMAND_LETTERS_SPACE.IndexOf(expression[i]);
						if(index>=0){
							c=SPACE_LETTERS[index];
							return true;
						}

						//-- 文字エスケープ
						index=CONTROL_LETTERS.IndexOf(expression[i]);
						if(index>=0){
							c=expression[i];
							return true;
						}

						i--;
						return false;
					}
					case '-':
					case ']':
						return false;
					default:
						i++;
						return true;
				}
			}
			//========================================================
			//		判定関数作成
			//========================================================
			/// <summary>
			/// 
			/// </summary>
			/// <param name="mode">文字クラスの正負を指定します。false を指定した時は [^なんとかかんとか] と解釈されます。</param>
			/// <param name="class_name">文字クラスの正規表現を指定します。</param>
			/// <param name="proc_arr">文字クラスを構成する判定関数達を指定します。</param>
			/// <param name="sub_arr">
			/// proc_arr に登録されている条件が、それぞれ加算なのか減算なのかを保持している配列です。
			/// true の時に減算になります。
			/// </param>
			/// <returns></returns>
			private Data CreateClass(
				bool mode,
				string class_name,
				System.Predicate<char>[] proc_arr,
				bool[] sub_arr
			){
				if(!mode)class_name="^"+class_name;

				return new Data(class_name,delegate(char c){
					bool ret=false;
					for(int i=0;i<proc_arr.Length;i++){
						// ret==true の時: 減算有効
						// ret==false の時: 加算有効
						if(ret!=sub_arr[i])continue;
						if(proc_arr[i](c))ret=!ret;
					}
					return mode==ret;
				});
			}
			private Data CreateUnitChar(char c1){
				return new Data(
					CharToName(c1),
					delegate(char c){return c==c1;}
					);
			}
			private Data CreateCharRange(char c1,char c2){
				return new Data(
					CharToName(c1)+"-"+CharToName(c2),
					delegate(char c){return c1<=c&&c<=c2;}
					);
			}
			/// <summary>
			/// 名前付き文字クラス指定を読み取ります。
			/// </summary>
			/// <param name="expression">読み取り対象の文字列を指定します。</param>
			/// <param name="i">p または P を指している状態で渡して下さい。
			/// 終わりの } の次の位置で返します。</param>
			/// <returns>読み取りが成功した場合に判定関数を返します。それ以外の場合に null を返します。</returns>
			private Data CreateNamedClass(){
				char c=expression[i];

				//-- 読み取り
				i++;if(i>=expression.Length||expression[i]!='{'){
					this.ReportError("名前付きクラスに引数が割り当てられていません。");
					return new Data();
				}

				string arg="";
				while(i<expression.Length&&expression[i]!='}')
					arg+=expression[i++];

				if(i>=expression.Length){
					this.ReportError("名前付きクラスの引数に終端 '}' がありません。");
					return new Data();
				}else i++;

				//-- インスタンス検索
				CharClassInfo info=CharClasses.GetInfo(arg);
				if(info==null){
					this.ReportError("指定した文字クラス '"+arg+"' は登録されていません。");
					return new Data();
				}

				if(c=='p')
					return new Data(@"\p{"+info.name+"}",info.proc_pos);
				else
					return new Data(@"\P{"+info.name+"}",info.proc_neg);
			}
		}
		#endregion
#endif
	}
}
