using afh.Parse;
using Rgx=afh.RegularExpressions.StringRegex;
using Gen=System.Collections.Generic;
using System.Text.RegularExpressions;
using Ref=System.Reflection;

namespace afh.Preprocessor{
	internal sealed partial class CppWordReader:AbstractWordReader{
		enum Mode{
			General,
			MacroContent,
		}
		private Mode mode=Mode.General;
		private int line_count=1;
		public int LineNumber{
			get{return line_count;}
			internal set{line_count=value;}
		}

		public CppWordReader(string text):base(text){}
		private CppWordReader(string text,Mode mode):base(text){
			this.mode=mode;
		}
		public static CppWordReader MacroContentMode(string text){
			return new CppWordReader(text,Mode.MacroContent);
		}

		public override bool ReadNext(){
			this.cword="";
			if(lreader.IsEndOfText){
				if(st_state.Count==0)return false;
				return this.PopStateAndNext();
			}

			this.lreader.StoreCurrentPos(0);
			switch(lreader.CurrentType.purpose){
				case LetterType.P_Number:
					readNumber();
					break;
				case LetterType.P_Token:
					readIdentifier();
					break;
				case LetterType.P_Operator:
					readOperator();
					break;
				default:
					readSpace();
					break;
			}
			return true;
		}

		private struct State{
			public LinearLetterReader lreader;
			public string cword;
			public int line_count;
			public WordType wtype;

			public State(CppWordReader wr){
				this.lreader=wr.lreader;
				this.cword=wr.cword;
				this.line_count=wr.line_count;
				this.wtype=wr.wtype;
			}

			public void Restore(CppWordReader wr){
				wr.lreader=this.lreader;
				wr.cword=this.cword;
				wr.line_count=this.line_count;
				wr.wtype=this.wtype;
			}
		}

		Gen::Stack<State> st_state=new System.Collections.Generic.Stack<State>();
		/// <summary>
		/// ソースコードを挿入します。
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public void InsertText(string text){
			st_state.Push(new State(this));
			this.lreader=new LinearLetterReader(text);
		}
		/// <summary>
		/// 挿入したソースコードから抜け出すと共に、
		/// 次の単語の読み取りを行います。
		/// </summary>
		/// <returns></returns>
		private bool PopStateAndNext(){
			st_state.Pop().Restore(this);
			return this.ReadNext();
		}
	}

	public class Preprocessor{
		Gen::Stack<System.Text.StringBuilder> buf_stack=new System.Collections.Generic.Stack<System.Text.StringBuilder>();
		System.Text.StringBuilder buf{
			get{return buf_stack.Peek();}
		}
		private void PushBuffer(){
			buf_stack.Push(new System.Text.StringBuilder());
		}
		private string PopBuffer(){
			return buf_stack.Pop().ToString();
		}

		CppWordReader wr=null;
		string filename=null;

		Gen::Dictionary<string,DefineMacro> macros=new System.Collections.Generic.Dictionary<string,DefineMacro>();
		public Preprocessor(){}


		public string Preprocess(string text){
			this.PushBuffer();
			this.process(text);
			return this.PopBuffer();
		}

		private void process(string text){
			CppWordReader wr_orig=this.wr;
			this.wr=new CppWordReader(text);
			while(wr.ReadNext()){
				switch(wr.CurrentType.value){
					case WordType.vKeyWord:
						// @ マクロ
						break;
					case WordType.vTag:
						// # マクロ
						// #line
						{
							Rgx.Capture c=Rgx.Match(@"#:bline:b((?<line>\d+):b)?(?<filename>:"")",wr.CurrentWord);
							if(c!=null){
								filename=c.Groups["filename"].Last.Value;
								if(c.Groups["line"].Count>0){
									int line;
									if(int.TryParse(c.Groups["line"].Last.Value,out line))
										wr.LineNumber=line-1;
								}
								buf.AppendFormat("#line {0} {1}",wr.LineNumber+1,filename);
								break;
							}
						}
						// #pragma begin(define)
						{
							Rgx.Capture c=DefineMacro.REG_DECL.Match(wr.CurrentWord);
							if(c!=null){
								DefineMacro macro=this.ReadDefineMacro(c);
								if(!macros.ContainsKey(macro.Name)){
									macros.Add(macro.Name,macro);
									break;
								}else{
									buf.AppendLine();
									buf.AppendLine("#error マクロ "+macro.Name+" は既に定義されています");
									buf.AppendFormat("#line {0} {1}",wr.LineNumber+1,filename);
								}
							}
						}
						// #pragma undef
						{
							Rgx.Capture c=DefineMacro.REG_UNDEF.Match(wr.CurrentWord);
							if(c!=null){
								string name=c.Groups["name"].Last.Value;
								if(macros.Remove(name))break;
								buf.AppendLine("#error "+name+" は定義されていないマクロです");
								buf.AppendFormat("#line {0} {1}\r\n",wr.LineNumber+1,filename);
							}
						}
						goto default;
					case WordType.vIdentifier:
						if(!macros.ContainsKey(wr.CurrentWord))goto default;
						this.ReadMacroRef(wr.CurrentWord);
						break;
					case WordType.vComment:
					default:
						buf.Append(wr.CurrentWord);
						break;
				}
			}
			this.wr=wr_orig;
		}
		//============================================================
		//		マクロ処理
		//============================================================
		private DefineMacro ReadDefineMacro(Rgx.Capture c){
			System.Text.StringBuilder buf=new System.Text.StringBuilder();
			if(filename!=null)
				buf.AppendFormat("\r\n#line {0} {1}\r\n",wr.LineNumber+1,filename);

			int nest=0;
			while(wr.ReadNext()){
				switch(wr.CurrentType.value){
					case WordType.vTag:
						if(DefineMacro.REG_BEGIN.IsMatching(wr.CurrentWord)){
							nest++;
						}else if(DefineMacro.REG_END.IsMatching(wr.CurrentWord)){
							if(nest--==0)goto break_loop;
						}
						goto default;
					default:
						buf.Append(wr.CurrentWord);
						break;
				}
			}
		break_loop:
			string content=buf.ToString();
			if(filename!=null)
				this.buf.AppendFormat("#line {0} {1}",wr.LineNumber+1,filename);

			return new DefineMacro(c,content);
		}

		private void ReadMacroRef(string name){
			// マクロ解決に失敗した時用
			this.PushBuffer();
			buf.Append(wr.CurrentWord);

			//-- 引数読み取り
			string[] args=this.ReadArguments();
			if(args==null)goto cancel; // 引数がない場合→マクロでないと判定

			//-- 実体化
			for(int i=0;i<args.Length;i++)
				args[i]=this.Preprocess(args[i]);
			wr.InsertText(macros[name].Instantiate(args,wr.LineNumber,filename));

			this.PopBuffer();
			return;
		cancel:
			string x=this.PopBuffer();
			buf.Append(x);
			return;
		}

		/// <summary>
		/// 引数 (alpha, beta, ...) を読み取ります。
		/// 読み取った部分の内容は buf にも出力されます。
		/// </summary>
		/// <returns>引数として読み取った物を返します。</returns>
		private string[] ReadArguments(){
			//-- (
			while(wr.ReadNext()&&wr.CurrentType==WordType.Space)
				buf.Append(wr.CurrentWord);

			if(wr.CurrentType!=WordType.Operator||wr.CurrentWord!="(")
				return null;

//			if(!wr.ReadNext())goto no_endof_args;


			//-- 実引数
			int nest_level=0;
			bool appeared_comma=false;
			Gen::List<string> args=new System.Collections.Generic.List<string>();
			System.Text.StringBuilder buf_arg=new System.Text.StringBuilder();
			while(wr.ReadNext()){
				buf.Append(wr.CurrentWord);
				switch(wr.CurrentType.value){
					case WordType.vOperator:
						if(wr.CurrentWord=="("){
							nest_level++;
							goto default;
						}else if(wr.CurrentWord==")"){
							if(nest_level-->0)goto default;

							if(appeared_comma)
								args.Add(buf_arg.ToString());
							return args.ToArray();
						}else if(wr.CurrentWord==","){
							appeared_comma=true;
							args.Add(buf_arg.ToString());
							buf_arg.Remove(0,buf_arg.Length);
						}
						goto default;
					default:
						buf_arg.Append(wr.CurrentWord);
						break;
				}
			}

			//-- 失敗
			wr.LetterReader.SetError("",0,null);
			return null;
		}
	}

	internal class DefineMacro{
		private string name;
		private string[] parameters;
		private string content;

		public string Name{
			get{return name;}
		}
		public int ParamCount{
			get{return parameters.Length;}
		}

		const string DEFINE_HEAD="#:bpragma:bdefine:b";
		const string REX_ARGS=@"(?<param>:ｗ):ｂ(?:,:ｂ(?<param>:ｗ):ｂ)*";
		const string REX=DEFINE_HEAD+@"\(:bbegin:b\):ｂ(?<name>:ｗ):ｂ\(:ｂ(?:"+REX_ARGS+@")?\)";
		public static readonly Rgx.RegLan REG_DECL=new Rgx.RegLan(REX);
		public static readonly Rgx.RegLan REG_BEGIN=new Rgx.RegLan(DEFINE_HEAD+@"#:bpragma:bdefine:b\(:bbegin:b\)");
		public static readonly Rgx.RegLan REG_END=new Rgx.RegLan(DEFINE_HEAD+@"#:bpragma:bdefine:b\(:bend:b\)");
		public static readonly Rgx.RegLan REG_UNDEF=new Rgx.RegLan(DEFINE_HEAD+@"#:bpragma:bdefine\(:bundef:b\):ｂ(?<name>:ｗ)");
		public static readonly Rgx.RegLan REG_IMPORT=new Rgx.RegLan(DEFINE_HEAD+@"\(:bimport:b\):ｂ(?<name>:ｗ):ｂ\[(?<dll>[^\>]+)\]");
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c">REG_DECL に依る一致結果を指定します。</param>
		/// <param name="content">マクロの内容を指定します。</param>
		public DefineMacro(Rgx.Capture c,string content){
			this.name=c.Groups["name"].Last.Value;

			int nParam=c.Groups["param"].Count;
			this.parameters=new string[nParam];
			for(int i=0;i<nParam;i++){
				parameters[i]=c.Groups["param"][i].Value;
			}

			this.content=content;
		}

		public string Instantiate(string[] args,int line,string file){
			_debug_.PreprocessorAssert(args.Length==parameters.Length,"実引数の長さと仮引数の長さの不整合");

			// a. 識別子を置換
			// b. #@ → 文字化
			// c. #& → 文字列化
			// d. ## トークン接続
			// ■※■ aa##aa 等には対応する。aa ## aa には対応しない。
			System.Text.StringBuilder buf=new System.Text.StringBuilder();
			CppWordReader wr=CppWordReader.MacroContentMode(content);

			const int MODE_NON=0;
			const int MODE_STR=1;
			const int MODE_CHR=2;
			int mode=MODE_NON;
			while(wr.ReadNext()){
				string word=wr.CurrentWord;
				switch(wr.CurrentType.value){
					case WordType.vIdentifier:
						int i=IndexOfPrameter(word);
						if(i>=0)word=args[i];
						goto default;
					case WordType.vOperator:
						if(mode!=MODE_NON)goto default;
						if(word=="#&"){
							mode=MODE_STR;
							break;
						}else if(word=="#@"){
							mode=MODE_CHR;
							break;
						}else if(word=="##"){
							break;
						}
						goto default;
					case WordType.vSpace:
					case WordType.vComment:
						buf.Append(wr.CurrentWord);
						break;
					default:
						if(mode==MODE_CHR)
							buf.Append(afh.Text.TextUtils.Characterize(word));
						else if(mode==MODE_STR)
							buf.Append(afh.Text.TextUtils.Stringize(word));
						else
							buf.Append(word);
						mode=MODE_NON;
						break;
				}
			}
			//--------------------------------------------------------
			buf.AppendLine();
			if(file!=null)
				buf.AppendFormat("#line {0} {1}\r\n",line,file);
			return buf.ToString();
		}

		private int IndexOfPrameter(string paramName){
			for(int index=0;index<parameters.Length;index++)
				if(parameters[index]==paramName)return index;
			return -1;
		}
	}

	internal class DefineMacroLoad{

		public static Ref::MethodInfo LoadMethod(string file,string className,string methodName,string[] paramTypes){
			// TODO: 色々な場所の候補
			System.Type type=LoadType(file,className);
			if(type==null)throw new System.Exception("指定した型は見つかりません。");
			
			// メソッドの検索
			Ref::BindingFlags BF=Ref::BindingFlags.NonPublic|Ref::BindingFlags.Public|Ref::BindingFlags.Static;
			System.Type[] tParamTypes=new System.Type[paramTypes.Length];
			for(int i=0;i<paramTypes.Length;i++){
				System.Type tParam=NameToPrimitiveType(paramTypes[i]);
				if(tParam==null)
					throw new System.FormatException("関数の引数型を認識できません。");
			}

			Ref::MethodInfo m=type.GetMethod(methodName,BF,null,tParamTypes,null);
			if(m==null)throw new System.MissingMethodException("指定されたメソッドは見つかりません。");

			return m;
		}

		public static Ref::MethodInfo LoadMethod(string file,string className,string methodName,int numOfParams){
			// TODO: 色々な場所の候補
			System.Type type=LoadType(file,className);
			if(type==null)throw new System.Exception("指定した型は見つかりません。");

			// メソッドの検索
			Ref::BindingFlags BF=Ref::BindingFlags.NonPublic|Ref::BindingFlags.Public|Ref::BindingFlags.Static;
			Ref::MethodInfo m=null;

			foreach(Ref::MethodInfo minfo in type.GetMethods(BF)){
				if(minfo.Name!=methodName)continue;

				// 引数型を check
				Ref::ParameterInfo[] pinfos=minfo.GetParameters();
				int i;
				for(i=0;i<pinfos.Length;i++){
					if(!IsPrimitive(pinfos[i].ParameterType))break;
				}
				if(i!=pinfos.Length)continue;

				if(m!=null)
					throw new System.Reflection.AmbiguousMatchException("オーバーロードを特定できません。");

				m=minfo;
			}

			if(m==null)throw new System.MissingMethodException("指定されたメソッドは見つかりません。");

			return m;
		}
		private static System.Type LoadType(string file,string className){
			if(file==null)throw new System.ArgumentNullException("file");
			if(className==null)throw new System.ArgumentNullException("className");

			string filepath=System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory,file);
			if(!System.IO.File.Exists(filepath))return null;

			System.Reflection.Assembly asm=System.Reflection.Assembly.LoadFrom("filepath");
			try{
				return asm.GetType(className);
			}catch{
				return null;
			}
		}
		private static bool IsPrimitive(System.Type type){
			return type.IsPrimitive||type==typeof(decimal)||type==typeof(string);
		}
		/// <summary>
		/// 基本型を表現する文字列から、それに対応する System.Type を取得します。
		/// </summary>
		/// <param name="typeName">基本型を表現する文字列を指定します。</param>
		/// <returns>指定した typeName に相当する System.Type を指定します。
		/// 対応する物が認められない場合、null を返します。</returns>
		private static System.Type NameToPrimitiveType(string typeName){
			switch(typeName){
				//----------------------------------------------------
				//		整数型
				//----------------------------------------------------
				case "System.Int32":
				case "Int32":
				case "int":
					return typeof(int);
				case "System.UInt32":
				case "UInt32":
				case "uint":
					return typeof(uint);
				case "System.Byte":
				case "Byte":
				case "byte":
					return typeof(byte);
				case "System.SByte":
				case "SByte":
				case "sbyte":
					return typeof(sbyte);
				case "System.Int16":
				case "Int16":
				case "short":
					return typeof(short);
				case "System.UInt16":
				case "UInt16":
				case "ushort":
					return typeof(ushort);
				case "System.Int64":
				case "Int64":
				case "long":
					return typeof(long);
				case "System.UInt64":
				case "UInt64":
				case "ulong":
					return typeof(ulong);
				//----------------------------------------------------
				//		他
				//----------------------------------------------------
				case "System.Char":
				case "Char":
				case "char":
					return typeof(char);
				case "System.Bool":
				case "Bool":
				case "bool":
					return typeof(bool);
				case "System.Double":
				case "Double":
				case "double":
					return typeof(double);
				case "System.Single":
				case "Single":
				case "float":
					return typeof(float);
				case "System.Decimal":
				case "Decimal":
				case "decimal":
					return typeof(decimal);
				case "System.String":
				case "String":
				case "string":
					return typeof(string);
				default:
					return null;
			}
		}
	
	}
}