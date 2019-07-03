#define ONE_NODE_ONE_TESTER

using Gen=System.Collections.Generic;
using BaseRegexFactory=afh.RegularExpressions.RegexFactory3<char>;
using RangeErrorPair=System.Collections.Generic.KeyValuePair<
	afh.Parse.LinearLetterReader.TextRange,
	afh.Parse.AnalyzeError
>;

namespace afh.RegularExpressions{
	/// <summary>
	/// �ʏ�̕�����ɑ΂��鐳�K�\����񋟂��܂��B
	/// </summary>
	public partial class StringRegex:BaseRegexFactory{

		static StringRegex instance=new StringRegex();
		/// <summary>
		/// StringRegex �̊�{�C���X�^���X���擾���܂��B
		/// </summary>
		public static StringRegex Instance{
			get{return instance;}
		}

		private Parser parser;
		private Grammar grammar;
		/// <summary>
		/// �V����������p���K�\�����쐬���܂��B
		/// ���K�\���̎w��̎d���������̍D�݂ɍ��킹�ĕύX�������ꍇ�ȂǂɎg�p���܂��B
		/// �ʏ�́AStringRegex.Instance ���g�p���ĉ������B
		/// </summary>
		public StringRegex(){
			this.grammar=new Grammar();
			this.parser=new Parser(this);
		}
#if OBSOLETE
		/// <summary>
		/// �w�肵�������� INode �Ɉ�v���邩�ۂ��𔻒肵�܂��B
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
		/// ���K�\��������ɑΉ����鐳�K������쐬���܂��B
		/// </summary>
		/// <param name="regularExpression">���K�\����������w�肵�܂��B</param>
		/// <returns>�w�肵�����K�\�������񂩂�쐬�������K�����Ԃ��܂��B</returns>
		public RegLan CreateLanguage(string regularExpression){
			return new RegLan(regularExpression);
		}
		/// <summary>
		/// ���K�\�����������͂��܂��B
		/// </summary>
		/// <param name="regularExpression">���K�\����������w�肵�܂��B</param>
		/// <returns>��͌��ʂɊւ������Ԃ��܂��B</returns>
		protected ParseResult Parse(string regularExpression){
			ParseResult ret;
			ret.node=this.parser.Parse(regularExpression);
			ret.errors=afh.Collections.Enumerable.From(this.parser.Errors).ToArray();
			return ret;
		}
		/// <summary>
		/// ���K�\��������̉�͌��ʂ��i�[����\���̂ł��B
		/// </summary>
		protected struct ParseResult{
			/// <summary>
			/// ���K�\���̃��[�g�v�f��ێ����܂��B
			/// </summary>
			public INode node;
			/// <summary>
			/// ���K�\���̓ǂݎ�莞�ɋN��������O�ɂ��Ă̏���ێ����܂��B
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
			//		�p�^�[��
			//--------------------------------------------------------
			/// <summary>
			/// " �Ɉ˂��Ĉ͂܂ꂽ������ /"(?:[^\\"]|\\.)*"/ �Ɉ�v���܂��B
			/// \ �̓G�X�P�[�v�����ł��B\ �̌�̕����͉��ł� (���s�ł�) �ǂݎ��܂��B
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

				// �ꕶ����
				if(t.EndOfStream||!afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csymf))return false;
				t.MoveNext();

				while(!t.EndOfStream&&afh.Text.CharUtils.Is(t.Current,afh.Text.CLangCType.csym))t.MoveNext();
				return true;
			});
			public static readonly FunctionNode WordUnicode=new FunctionNode(":��",delegate(ITypedStream<char> t){
				if(!t.IsStart&&IsUnicodeWord(t.Previous))return false;

				// �ꕶ����
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

				// �ꕶ����
				if(t.EndOfStream)return isborder;
				char c=t.Current;
				if(!(c==' '||c=='\t'||c=='\v'))return isborder;
				t.MoveNext();c=t.Current;

				while(!t.EndOfStream&&(c==' '||c=='\t'||c=='\v')){
					t.MoveNext();c=t.Current;
				}
				return true;
			});
			// \��|[\t\v\x{85}\p{Z}]+
			public static readonly FunctionNode WordBlanksUnicode=new FunctionNode(":��",delegate(ITypedStream<char> t){
				bool isborder=procWordBorderUnicode(t);

				// �ꕶ����
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
			//		�A���J�[
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
			public static readonly FunctionNode WordBorderUnicode=new FunctionNode(@"\��",procWordBorderUnicode);
			public static readonly FunctionNode NotWordBorderUnicode=new FunctionNode(@"\�a",procNotWordBorderUnicode);

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
		/// " �Ɉ˂��Ĉ͂܂ꂽ������ /"(?:[^\\"]|\\.)*"/ �Ɉ�v���܂��B
		/// \ �̓G�X�P�[�v�����ł��B\ �̌�̕����͉��ł� (���s�ł�) �ǂݎ��܂��B
		/// </summary>
		private sealed class QuotedStringNode:INode,ITester{
			private QuotedStringNode(){}
			private static readonly QuotedStringNode instance=new QuotedStringNode();
			/// <summary>
			/// QuotedStringNode �̗B��̃C���X�^���X���擾���܂��B
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
		/// ��v����������ɑ΂������񋟂��܂��B
		/// </summary>
		public class Capture:CaptureBase<Capture>{
			internal Capture(Capture capture,int index)
				:base(capture,index){}
			internal Capture(Evaluator eval):base(eval){}
			/// <summary>
			/// ���N���X���痘�p����ׂ̊֐��ł��B
			/// �w�肵���ԍ��ɑΉ�����q�L���v�`���������쐬���܂��B
			/// </summary>
			/// <param name="index">�q�L���v�`���̔ԍ����w�肵�܂��B
			/// ���̔ԍ��� MatchData �ɕێ����Ă��� List&lt;ICaptureRange&gt; ���̔ԍ��ł��B</param>
			/// <returns>�쐬���� Capture �C���X�^���X��Ԃ��܂��B</returns>
			protected internal sealed override Capture CreateCapture(int index){
				return new Capture(this,index);
			}
			/// <summary>
			/// ��v����������������擾���܂��B
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
			/// ���̃C���X�^���X�𕶎���ɕϊ����܂��B
			/// ��v���������������Ԃ��܂��B
			/// </summary>
			/// <returns>��v���������������Ԃ��܂��B
			/// ������v�������e���Ȃ��ꍇ�ɂ� "&lt;null&gt;" �Ƃ������e��Ԃ��܂��B</returns>
			public sealed override string ToString(){
				return this.Value??"<null>";
			}
			/// <summary>
			/// ������ɕϊ����܂��B��v��������������ɕϊ�����܂��B
			/// </summary>
			/// <param name="c">�ϊ����� Capture �C���X�^���X��Ԃ��܂��B</param>
			/// <returns>�ϊ���̈�v�������Ԃ��܂��B</returns>
			public static implicit operator string(Capture c){
				return c.Value;
			}
		}

		/*
		/// <summary>
		/// ��v����������ɑ΂������񋟂��A�u����̏����󂯎��ׂɎg�p���܂��B
		/// </summary>
		public sealed class ReplaceCapture:Capture{
			internal ReplaceCapture(Capture capture,int index)
				:base(capture,index){}
			internal ReplaceCapture(Evaluator eval):base(eval){}

			private string replaced=null;
			/// <summary>
			/// �u����̒l���w�肵�܂��B
			/// </summary>
			public string NewValue{
				get{return this.replaced;}
				set{this.replaced=value;}
			}
		}
		//*/

		/// <summary>
		/// ���K�\���ɂ���ĕ\����錾�� (������W��) ��\�����܂��B
		/// </summary>
		public class RegLan{
			INode node;
			private bool overlap_search=false;
			/// <summary>
			/// ��x�}�b�`���������𑱂��ă}�b�`�����邩�ۂ����擾���͐ݒ肵�܂��B
			/// </summary>
			public bool OverlapSearch{
				get{return this.overlap_search;}
				set{this.overlap_search=value;}
			}

			/// <summary>
			/// ����� StringRegex ���g�p���� RegLan �����������܂��B
			/// </summary>
			/// <param name="regex">���̐��K������K�肷�鐳�K�\�����w�肵�܂��B</param>
			public RegLan(string regex){
				ParseResult res=StringRegex.Instance.Parse(regex);
				this.node=res.node;
				if(res.errors.Length>0){
					System.Text.StringBuilder build=new System.Text.StringBuilder();
					build.Append("���K�\���̍\����͒��ɃG���[���������܂����B\r\n");
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
			/// ���̃C���X�^���X�̓��e�𕶎���Ƃ��Ď擾���܂��B
			/// </summary>
			/// <returns>���̐��K�����\�����鐳�K�\����Ԃ��܂��B</returns>
			public override string ToString(){
				return this.node.ToString();
			}

			//========================================================
			//		Replace
			//========================================================
			/// <summary>
			/// �u��������ׂ̕]�����s���֐��̌^�ł��B
			/// </summary>
			/// <param name="c">�u���̑ΏۂƂȂ镔������w�肵�܂��B</param>
			/// <returns>�u����̓��e��Ԃ��܂��B</returns>
			public delegate string ReplaceProc(Capture c);
			/// <summary>
			/// ������̒u�����s���܂��B
			/// </summary>
			/// <param name="text">�u���̑Ώۂ̕�������i�[���Ă��镶������w�肵�Ă��܂��B</param>
			/// <param name="p">�u���������s���֐����w�肵�܂��B</param>
			/// <returns>�u�����������ʂ̕������Ԃ��܂��B</returns>
			public string Replace(string text,ReplaceProc p) {
				return this.Replace(new StringStreamAdapter(text),p);
			}
			/// <summary>
			/// ������̒u�����s���܂��B
			/// </summary>
			/// <param name="data">�u���̑Ώۂ̕�������i�[���Ă���z����w�肵�Ă��܂��B</param>
			/// <param name="p">�u���������s���֐����w�肵�܂��B</param>
			/// <returns>�u�����������ʂ̕������Ԃ��܂��B</returns>
			public string Replace(char[] data,ReplaceProc p) {
				return this.Replace(new ArrayStreamAdapter<char>(data),p);
			}
			/// <summary>
			/// ������̒u�����s���܂��B
			/// </summary>
			/// <param name="str">�u���̑Ώۂ̕�������i�[���Ă��� Stream ���w�肵�Ă��܂��B</param>
			/// <param name="p">�u���������s���֐����w�肵�܂��B</param>
			/// <returns>�u�����������ʂ̕������Ԃ��܂��B</returns>
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

				// �c���S�Ēǉ�
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

				// �ꕶ�����ǂݏo��
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
			/// �w�肵���͈͂̓��e���AStringBuilder �ɒǉ����܂��B
			/// </summary>
			/// <param name="build">�������ݐ�̃o�b�t�@���w�肵�܂��B</param>
			/// <param name="str">�������ޓ��e��ێ����Ă��� Stream ���w�肵�܂��B</param>
			/// <param name="start">�������ޓ��e�̊J�n�ʒu���w�肵�܂��B</param>
			/// <param name="end">�������ޓ��e�̏I���ʒu���w�肵�܂��B�����Ɏw�肵���ʒu�̕����͒ǉ�����܂���B</param>
			internal static void AppendSubstring(System.Text.StringBuilder build,ITypedStream<char> str,int start,int end){
				StringStreamAdapter strstr=str as StringStreamAdapter;
				if(strstr!=null)
					build.Append(strstr.ContentText,start,end-start);

				ArrayStreamAdapter<char> arr=str as ArrayStreamAdapter<char>;
				if(arr!=null)
					build.Append(arr.Array,start,end-start);

				// �ꕶ�����ǂݏo��
				int index=str.Index;
				str.Index=start;
				do{
					build.Append(str.Current);
				}while(!str.EndOfStream&&str.Index<end);
				str.Index=index;
			}
			/// <summary>
			/// �w�肵���ʒu���疖�[�܂ł̓��e���AStringBuilder �ɒǉ����܂��B
			/// </summary>
			/// <param name="build">�������ݐ�̃o�b�t�@���w�肵�܂��B</param>
			/// <param name="str">�������ޓ��e��ێ����Ă��� Stream ���w�肵�܂��B</param>
			/// <param name="start">�������ޓ��e�̊J�n�ʒu���w�肵�܂��B</param>
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

				// �ꕶ�����ǂݏo��
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
			/// ������̒��Ƀp�^�[���Ɉ�v���镔�������݂��邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="text">�����Ώۂ̕�������w�肵�܂��B</param>
			/// <returns>�p�^�[���Ɉ�v���镔�������������ꍇ�� true ��Ԃ��܂��B
			/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
			public bool IsMatching(string text){
				return this.IsMatching(new StringStreamAdapter(text));
			}
			/// <summary>
			/// ������̒��Ƀp�^�[���Ɉ�v���镔�������݂��邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="data">�����Ώۂ̕����z����w�肵�܂��B</param>
			/// <returns>�p�^�[���Ɉ�v���镔�������������ꍇ�� true ��Ԃ��܂��B
			/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
			public bool IsMatching(char[] data){
				return this.IsMatching(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// ������̒��Ƀp�^�[���Ɉ�v���镔�������݂��邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="str">�����Ώۂ̕����X�g���[�����w�肵�܂��B</param>
			/// <returns>�p�^�[���Ɉ�v���镔�������������ꍇ�� true ��Ԃ��܂��B
			/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
			public bool IsMatching(ITypedStream<char> str){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,this.node);
				return eval.Match();
			}
			//========================================================
			//		IsMatching
			//========================================================
			/// <summary>
			/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
			/// </summary>
			/// <param name="text">�����Ώۂ̕�������w�肵�܂��B</param>
			/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
			/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			public Capture Match(string text){
				return this.Match(new StringStreamAdapter(text));
			}
			/// <summary>
			/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
			/// </summary>
			/// <param name="data">�����Ώۂ̕����z����w�肵�܂��B</param>
			/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
			/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			public Capture Match(char[] data){
				return this.Match(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
			/// </summary>
			/// <param name="str">�����Ώۂ̕����X�g���[�����w�肵�܂��B</param>
			/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
			/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
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
			/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
			/// </summary>
			/// <param name="text">�����Ώۂ̕�������w�肵�܂��B</param>
			/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
			public Gen::IEnumerable<Capture> Matches(string text){
				return this.Matches(new StringStreamAdapter(text));
			}
			/// <summary>
			/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
			/// </summary>
			/// <param name="data">�����Ώۂ̕����z����w�肵�܂��B</param>
			/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
			public Gen::IEnumerable<Capture> Matches(char[] data){
				return this.Matches(new ArrayStreamAdapter<char>(data));
			}
			/// <summary>
			/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
			/// </summary>
			/// <param name="str">�����Ώۂ̕����X�g���[�����w�肵�܂��B</param>
			/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
			public Gen::IEnumerable<Capture> Matches(ITypedStream<char> str){
				Status s=new Status(str);
				Evaluator eval=new Evaluator(s,node);
				eval.OverlapSearch=this.overlap_search;
				while(eval.Match())
					yield return new Capture(eval);
			}
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="text">�����Ώۂ̕�������w�肵�܂��B</param>
		/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,string text) {
			return new RegLan(regex).Matches(text);
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="data">�����Ώۂ̕����z����w�肵�܂��B</param>
		/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,char[] data){
			return new RegLan(regex).Matches(data);
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔����𕡐��������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="str">�����Ώۂ̕����X�g���[�����w�肵�܂��B</param>
		/// <returns>��v�������񋓂���񋓉\�̂�Ԃ��܂��B</returns>
		public static Gen::IEnumerable<Capture> Matches(string regex,ITypedStream<char> str){
			return new RegLan(regex).Matches(str);
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="text">�����Ώۂ̕�������w�肵�܂��B</param>
		/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
		/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		public static Capture Match(string regex,string text){
			return new RegLan(regex).Match(text);
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="data">�����Ώۂ̕����z����w�肵�܂��B</param>
		/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
		/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		public static Capture Match(string regex,char[] data) {
			return new RegLan(regex).Match(data);
		}
		/// <summary>
		/// ������̒�����p�^�[���Ɉ�v���镔������������܂��B
		/// </summary>
		/// <param name="regex">�p�^�[�����K�肷�鐳�K�\�����w�肵�܂��B</param>
		/// <param name="str">�����Ώۂ̕����X�g���[�����w�肵�܂��B</param>
		/// <returns>��v���镔���񂪌��������ꍇ�ɂ́A���̏���ێ����� Capture ��Ԃ��܂��B
		/// ��v���镔���񂪌�����Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
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

				// cmddic �̏�����
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
			/// �w�肵�������𐳋K�\���ŕ\�����܂��B
			/// </summary>
			/// <param name="letter">���K�\���ɕϊ��������������w�肵�܂��B</param>
			/// <returns>�����𐳋K�\���ɂ��ĕԂ��܂��B
			/// ���������ʂȃG�X�P�[�v�\���������Ă���ꍇ�ɂ́A���̃G�X�P�[�v�\���ŕԂ��܂��B
			/// ����ȊO�̏ꍇ�ɂ͂��̘ԕԂ��܂��B
			/// </returns>
			public static string CharToSource(char letter){
				if(CONTROL_LETTERS.IndexOf(letter)>=0)
					return "\\"+letter;

				// �󔒌n�̕���
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
		/// ���K�\�����������͂���N���X�ł��B
		/// </summary>
		protected sealed class Parser:ParserBase{
			private readonly Grammar grammar;
			/// <summary>
			/// Parser �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="factory">���K�\�����`���� StringRegex ���w�肵�܂��B</param>
			public Parser(StringRegex factory):base(factory){
				this.grammar=((StringRegex)this.parent).grammar;
				this.InitializeCommand();
			}

			/// <summary>
			/// ���̐��K�\���̕��@���`���� IRegexGrammar �C���X�^���X���擾���܂��B
			/// </summary>
			/// <returns>���̐��K�\���̕��@���`���Ă��� RegexGrammar �C���X�^���X��Ԃ��܂��B</returns>
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
				//		�G�X�P�[�v
				//------------------------------------------------
				for(int i=0;i<Grammar.CONTROL_LETTERS.Length;i++){
					command_nodes[Grammar.CONTROL_LETTERS[i].ToString()]
						=CreateCharacterNode(Grammar.CONTROL_LETTERS[i]);
				}
				//------------------------------------------------
				//		���ʂ̕���
				//------------------------------------------------
				command_nodes["t"]=CreateCharacterNode('\t');
				command_nodes["v"]=CreateCharacterNode('\v');
				command_nodes["n"]=CreateCharacterNode('\n');
				command_nodes["r"]=CreateCharacterNode('\r');
				//command_nodes["b"]=CreateCharacterNode('\b');
				command_nodes["f"]=CreateCharacterNode('\f');
				command_nodes["a"]=CreateCharacterNode('\a');
				//------------------------------------------------
				//		ANSI �����N���X (ECMAScript �݊�)
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
				//		Unicode �����N���X
				//------------------------------------------------
				RegisterCommand("��",new FunctionElemNode(@"\��",IsUnicodeWord));
				RegisterCommand("�v",new FunctionElemNode(@"\�v",delegate(char c){
					afh.Text.GeneralCategory cat=afh.Text.CharUtils.GetGeneralCategory(c);
					return !(afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.L)
						||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Nd)
						||afh.Text.CharUtils.Is(cat,afh.Text.GeneralCategory.Pc));
				}));
				RegisterCommand("��",new FunctionElemNode(@"\��",delegate(char c){
					return "\f\n\r\t\v\x85".IndexOf(c)>=0
						||afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Z);
				}));
				RegisterCommand("�r",new FunctionElemNode(@"\�r",delegate(char c){
					return !("\f\n\r\t\v\x85".IndexOf(c)>=0
						||afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Z));
				}));
				RegisterCommand("��",new FunctionElemNode(@"\��",delegate(char c){
					return afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Nd);
				}));
				RegisterCommand("�c",new FunctionElemNode(@"\�c",delegate(char c) {
					return !afh.Text.CharUtils.Is(c,afh.Text.GeneralCategory.Nd);
				}));
				//------------------------------------------------
				//		�A���J�[
				//------------------------------------------------
				RegisterCommand("b",Nodes.WordBorder);
				RegisterCommand("B",Nodes.NotWordBorder);
				RegisterCommand("��",Nodes.WordBorderUnicode);
				RegisterCommand("�a",Nodes.NotWordBorderUnicode);
				RegisterCommand("A",Nodes.Start);
				RegisterCommand("Z",Nodes.End);
				RegisterCommand("z",Nodes.EndExact);
				//------------------------------------------------
				//		�� (ProcessCommand ���� Node ��`)
				//------------------------------------------------
				this.grammar.RegisterCommand("p",CommandData.Brace);
				this.grammar.RegisterCommand("P",CommandData.Brace);
				this.grammar.RegisterCommandC("\"",CommandData.NoArg);
				this.grammar.RegisterCommandC("n",CommandData.NoArg);
				this.grammar.RegisterCommandC("w",CommandData.NoArg);
				this.grammar.RegisterCommandC("��",CommandData.NoArg);
				this.grammar.RegisterCommandC("b",CommandData.NoArg);
				this.grammar.RegisterCommandC("��",CommandData.NoArg);
			}

			/// <summary>
			/// �R�}���h�ɑΉ����鐳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="name">�R�}���h�̖��O���w�肵�܂��B</param>
			/// <param name="arg">�R�}���h�̈������w�肵�܂��B</param>
			/// <returns>�쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected override INode ProcessCommand(string name,string arg){
				// �����ɓo�^�ς̕�
				{
					INode node;
					if(command_nodes.TryGetValue(name,out node))return node;
				}
				switch(name){
					//------------------------------------------------
					//		���O�t�������N���X
					//------------------------------------------------
#if OBSOLETE
					case "p":{
						if(arg.StartsWith("Is"))arg=arg.Substring(2);
						INode node;
						if(!CharClasses.nodes_p.TryGetValue(arg.Replace("-","").ToLower(),out node)){
							throw new RegexParseException("�w�肵�������N���X '"+arg+"' �͓o�^����Ă��܂���B");
						}
						return node;
					}
					case "P":{
						if(arg.StartsWith("Is"))arg=arg.Substring(2);
						INode node;
						if(!CharClasses.nodes_P.TryGetValue(arg.Replace("-","").ToLower(),out node)){
							throw new RegexParseException("�w�肵�������N���X '"+arg+"' �͓o�^����Ă��܂���B");
						}
						return node;
					}
#endif
					case "p":{
						CharClasses.CharClassInfo info=CharClasses.GetInfo(arg);
						if(info==null){
							throw new RegexParseException("�w�肵�������N���X '"+arg+"' �͓o�^����Ă��܂���B");
						}
						return info.node_pos;
					}
					case "P":{
						CharClasses.CharClassInfo info=CharClasses.GetInfo(arg);
						if(info==null){
							throw new RegexParseException("�w�肵�������N���X '"+arg+"' �͓o�^����Ă��܂���B");
						}
						return info.node_neg;
					}
					default:
						__debug__.RegexParserAssert(false);
						return null;
				}
			}
			/// <summary>
			/// �R�����R�}���h�ɑΉ����鐳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="name">�R�����R�}���h�̖��O���w�肵�܂��B</param>
			/// <param name="arg">�R�����R�}���h�̈������w�肵�܂��B</param>
			/// <returns>�쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected override INode ProcessCommandC(string name,string arg) {
				switch(name){
					case "\"":return Nodes.QuotedString;
					case "n":return Nodes.LineBreak;
					case "w":return Nodes.Word;
					case "��":return Nodes.WordUnicode;
					case "b":return Nodes.WordBlanks;
					case "��":return Nodes.WordBlanksUnicode;
					default:
						__debug__.RegexParserAssert(false);
						return null;
				}
			}
			/// <summary>
			/// �������琳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="letter">���K�\���v�f�̌��ƂȂ镶�����w�肵�܂��B</param>
			/// <returns>�w�肵�����������ɍ쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected override INode ProcessLetter(char letter){
				__debug__.RegexParserAssert(
					letter!='.'&&letter!='^'&&letter!='$',
					"�Ăяo������ filter ����Ă��锤");

				return CreateCharacterNode(letter);
			}
			/// <summary>
			/// �N���X�\�����琳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="content">�N���X�w�� [...] �̒��g���w�肵�܂��B</param>
			/// <returns>�w�肵������������ɍ쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected override INode ProcessClass(string content){
				//return new CharClassReader(this,content).CreateNode();
				return new CharClasses.CharClassNodeGenerator(this.processClassError,content).CreateNode();
			}

			internal void processClassError(string msg,string expression,int index){
				this.Error(string.Format("�����N���X \"{0}\":{1} ������\r\n\t{2}",expression,index,msg));
			}

			/// <summary>
			/// �w�肵�������ɑΉ����� INode �C���X�^���X���쐬���܂��B
			/// </summary>
			/// <param name="ch">��v�Ώۂ̕������w�肵�܂��B</param>
			/// <returns>�w�肵�������Ɉ�v���� INode ���쐬���ĕԂ��܂��B</returns>
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
				/// ���K�\���ŕ\���������̖��O��ێ����܂��B
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
			//		�ǂݎ��
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
							// �ʏ�̕���
							if(i>=expression.Length||expression[i]!='-'){
								goto one_char;
							}

							// �͈͎w�� ?
							i++;if(i<expression.Length&&ReadChar(out c2)){
								goto range_char;
							}

							if(i<expression.Length&&expression[i]!='['){
								this.ReportError("�����͈͎w�� �ua-b�v �̏I�[�ɓ����镶��������܂���B������ - �ɃG�X�P�[�v���K�v�ł��B");
							}

							// ��͂�ʏ�̕���
							i--;{							
								goto one_char;
							}
						case '\\':{
							i++;if(i>=expression.Length) {
								this.ReportError(@"\ �ɑ����������K�v�ł��B");
								goto ED;
							}

							//-- ���O�t�������N���X
							if(c1=='p'||c1=='P'){
								data=this.CreateNamedClass();
								goto add_data;
							}else{
								this.ReportError("�F���o���Ȃ��R�}���h�ł��B");
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
						one_char: // c1 ���琶��
							data=CreateUnitChar(c1);
							goto add_data;
						range_char: // c1-c2 ���琶��
							data=CreateCharRange(c1,c2);
							goto add_data;
						add_data: // �����N���X���Z
							if(data.proc!=null){
								proc_list.Add(data.proc);
								class_name+=data.name;
								sub_list.Add(false);
							}
							break;
						add_negative: // �����N���X���Z
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
			/// �ʏ�̕�����ǂݎ���ꍇ�ɂ́A���̕�����ǂݎ���� true ��Ԃ��܂��B
			/// �ʏ�̕����łȂ��ꍇ�ɂ́A�ǂݎ�炸�� false ��Ԃ��܂��B
			/// </summary>
			/// <param name="expression"></param>
			/// <param name="i">������ǂݎ�����ꍇ�ɂ́A���̎��̕����̈ʒu���w������ԂŕԂ��܂��B</param>
			/// <param name="c"></param>
			/// <returns>�ʏ�̕�����ǂݎ�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			private bool ReadChar(out char c){
				c=expression[i];

				switch(c){
					case '\\':{
						i++;if(i>=expression.Length){
							this.ReportError(@"\ �ɑ����������K�v�ł��B");
							i--;
							return false;
						}

						//-- ���ʂ̕���
						int index=COMMAND_LETTERS_SPACE.IndexOf(expression[i]);
						if(index>=0){
							c=SPACE_LETTERS[index];
							return true;
						}

						//-- �����G�X�P�[�v
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
			//		����֐��쐬
			//========================================================
			/// <summary>
			/// 
			/// </summary>
			/// <param name="mode">�����N���X�̐������w�肵�܂��Bfalse ���w�肵������ [^�Ȃ�Ƃ�����Ƃ�] �Ɖ��߂���܂��B</param>
			/// <param name="class_name">�����N���X�̐��K�\�����w�肵�܂��B</param>
			/// <param name="proc_arr">�����N���X���\�����锻��֐��B���w�肵�܂��B</param>
			/// <param name="sub_arr">
			/// proc_arr �ɓo�^����Ă���������A���ꂼ����Z�Ȃ̂����Z�Ȃ̂���ێ����Ă���z��ł��B
			/// true �̎��Ɍ��Z�ɂȂ�܂��B
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
						// ret==true �̎�: ���Z�L��
						// ret==false �̎�: ���Z�L��
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
			/// ���O�t�������N���X�w���ǂݎ��܂��B
			/// </summary>
			/// <param name="expression">�ǂݎ��Ώۂ̕�������w�肵�܂��B</param>
			/// <param name="i">p �܂��� P ���w���Ă����Ԃœn���ĉ������B
			/// �I���� } �̎��̈ʒu�ŕԂ��܂��B</param>
			/// <returns>�ǂݎ�肪���������ꍇ�ɔ���֐���Ԃ��܂��B����ȊO�̏ꍇ�� null ��Ԃ��܂��B</returns>
			private Data CreateNamedClass(){
				char c=expression[i];

				//-- �ǂݎ��
				i++;if(i>=expression.Length||expression[i]!='{'){
					this.ReportError("���O�t���N���X�Ɉ��������蓖�Ă��Ă��܂���B");
					return new Data();
				}

				string arg="";
				while(i<expression.Length&&expression[i]!='}')
					arg+=expression[i++];

				if(i>=expression.Length){
					this.ReportError("���O�t���N���X�̈����ɏI�[ '}' ������܂���B");
					return new Data();
				}else i++;

				//-- �C���X�^���X����
				CharClassInfo info=CharClasses.GetInfo(arg);
				if(info==null){
					this.ReportError("�w�肵�������N���X '"+arg+"' �͓o�^����Ă��܂���B");
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
