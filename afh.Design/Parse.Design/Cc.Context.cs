using Gen=System.Collections.Generic;
using Rgx=System.Text.RegularExpressions;
using TextRange=afh.Parse.LinearLetterReader.TextRange;
using AnalyzeError=afh.Parse.AnalyzeError;

namespace afh.Parse.Cc{
	public class ContextCompiler{
		public ContextCompiler(){}
		internal ContextWordReader wreader;
		public LinearLetterReader LetterReader{
			get{return this.wreader.LetterReader;}
		}
		/// <summary>
		/// クラスが定義される名前空間を保持します。
		/// </summary>
		private string @namespace;
		public string DefaultNamespace{
			get{return this.@namespace;}
			set{this.@namespace=value==""?null:value;}
		}
		/// <summary>
		/// 解析クラスの名前を保持します。
		/// </summary>
		private string classname;
		/// <summary>
		/// 複数の context の情報を保持します。
		/// </summary>
		private Gen::Dictionary<string,Context> contextlist;
		internal bool ContainsContext(string contextName){
			return this.contextlist.ContainsKey(contextName);
		}
		private void AddContext(Context ctxt){
			this.contextlist.Add(ctxt.name,ctxt);
		}
		//=================================================
		//		Parse
		//=================================================
		private string original;
		private string processed;
		public void Parse(string text){
			this.contextlist=new Gen::Dictionary<string,Context>();
			this.original=text;
			this.processed=this.PreprocessDirectives(this.original);
			this.Read(this.processed);
		}
		private void Read(string text){
			this.wreader=new ContextWordReader(text);
			this.wreader.ReadNext();

			Context ctxt;
			UserCommand ucmd;
			while(this.wreader.CurrentType!=WordType.Invalid){
				if(this.wreader.CurrentType==WordType.Identifier){
					switch(this.wreader.CurrentWord){
						case "context":
							if(Context.Read(this,out ctxt))this.AddContext(ctxt);
							break;
						case "command":
							if(UserCommand.Read(this,out ucmd))this.AddCommand(ucmd.name,ucmd);
							break;
						case "condition":
							throw new System.ApplicationException("御免。condition は未だ実装していないよ");
						default:
							this.wreader.LetterReader.SetError("定義されていない識別子です。宣言は context command condition の何れかで始めて下さい。",0,null);
							this.wreader.ReadNext();
							break;
					}
				}else{
					if(this.wreader.CurrentType!=WordType.Comment)
						this.wreader.LetterReader.SetError("不明な記号です。宣言は context command condition の何れかで始めて下さい。",0,null);
					this.wreader.ReadNext();
				}
			}
			while(Context.Read(this,out ctxt))this.AddContext(ctxt);
		}
		//=================================================
		//		Preprocess Directives
		//=================================================
		private string PreprocessDirectives(string text){
			Gen::Dictionary<int,string> errors=new Gen::Dictionary<int,string>();
			Gen::List<DefineMacro> defs=new Gen::List<DefineMacro>();

			DefineMacro def;
			string text2=reg_directive.Replace(text,delegate(System.Text.RegularExpressions.Match m){
				if(m.Groups["nspace"].Success){
					if(this.@namespace!=null){
						errors.Add(m.Index,"名前空間を複数指定する事は出来ません。");
					}
					this.@namespace=m.Groups["nspace"].Value;
				}else if(m.Groups["class"].Success){
					if(this.classname!=null){
						errors.Add(m.Index,"クラス名を複数指定する事は出来ません。");
					}
					this.classname=m.Groups["class"].Value;
				}else if(m.Groups["defname"].Success){
					def=new DefineMacro();
					def.name=m.Groups["defname"].Value;
					def.Content=m.Groups["defcontent"].Value;

					// 引数リスト
					System.Text.RegularExpressions.CaptureCollection capts=m.Groups["defarg"].Captures;
					int iM=capts.Count;
					def.args=new string[capts.Count];
					for(int i=0;i<iM;i++)def.args[i]=capts[i].Value;

					defs.Add(def);
				}
				return reg_remove.Replace(m.Value,"");
			});

			foreach(DefineMacro define in defs)define.Apply(ref text2);

			return text2;
		}
		private const string DIREC_LINEEND=@"[^\r\n]*(?:\r\n|\r|\n|$)";
		private const string DEF_ARG=@"(?:[^\r\n\,\)]|\\.)+";
		private static System.Text.RegularExpressions.Regex reg_directive=new System.Text.RegularExpressions.Regex(
			(@"^ *\#namespace +(?<nspace>[_\w\.]+)"+DIREC_LINEEND+"|"+
			@"^ *\#classname +(?<class>[_\w]+)"+DIREC_LINEEND+"|"+
			@"^ *\#def +(?<defname>[_\w]+) *\( *(?<defarg>[_\w]+)(?: *\, *(?<defarg>[_\w]+))*\)"+DIREC_LINEEND+@"(?<defcontent>[\s\S]*?)\#endef"+DIREC_LINEEND+"|"+	// 3 4 5 6 
			@"").Replace(@" ",@"[ \f\t\v]"),// ^ *\#(?:def|endef)\b"+DIREC_LINEEND+"
			System.Text.RegularExpressions.RegexOptions.Compiled|System.Text.RegularExpressions.RegexOptions.Multiline);
		private static System.Text.RegularExpressions.Regex reg_remove=new System.Text.RegularExpressions.Regex(
			@"[^\r\n]+",
			System.Text.RegularExpressions.RegexOptions.Compiled|System.Text.RegularExpressions.RegexOptions.Multiline);
		//=================================================
		//		ToCSharpSource
		//=================================================
		const string CLASS_TEMPLATE=@"
namespace {2}{{
	public partial class {1}{{
		afh.Parse.AbstractWordReader wreader;
		System.Collections.Stack stack;
		readonly object OpenMarker=new object();

		{0}
	}}
}}
";
		string cs_code_cache;
		/// <summary>
		/// 
		/// </summary>
		/// <returns>C# のコードを出力します。</returns>
		/// <remarks>
		/// 生成したコードだけでは不完全です。
		/// 先ず、Parse の前にフィールド wreader と stack の初期化を行って下さい。
		/// wreader には対応する WordReader のインスタンスを設定し、
		/// ReadNext を一回呼び出して一つ目の単語の位置にして下さい。
		/// </remarks>
		public string ToCSharpSource(){
			if(cs_code_cache==null){
				System.Text.StringBuilder builder=new System.Text.StringBuilder();
				foreach(Context ctxt in contextlist.Values){
					builder.Append(ctxt.ToCSharpSource(this));
				}
				cs_code_cache=string.Format(
					CLASS_TEMPLATE,
					builder.Replace("\n","\n\t\t").ToString(),
					this.classname??"NantokaParser",
					this.@namespace??"afh.Parse"
					);
			}
			return cs_code_cache;
		}
		//=================================================
		//	エラー列挙
		//=================================================
		public Gen::IEnumerable<ErrorInfo> EnumErrors(){
			afh.Text.MultilineString lc=new afh.Text.MultilineString(this.processed);
			foreach(Gen::KeyValuePair<TextRange,AnalyzeError> pair in this.LetterReader.EnumErrors()){
				ErrorInfo ei=new ErrorInfo();
				ei.message=pair.Value.message;
				lc.GetLineAndColumn(pair.Key.start,out ei.line,out ei.column);
				yield return ei;
			}
		}
		public struct ErrorInfo{
			public int line;
			public int column;
			public string message;
		}

		#region 命令定義
		//=================================================
		//	コマンド動的登録
		//=================================================
		internal Gen::Dictionary<string,ICommand> cmdreg=new Gen::Dictionary<string,ICommand>(cmdreg_);
		public void AddCommand(string commandName,ICommand command){
			cmdreg[commandName]=command;
		}
		//=================================================
		//	コマンド静的登録
		//=================================================
		/// <summary>
		/// 静的に登録されているコマンドを保持します。
		/// </summary>
		private static Gen::Dictionary<string,ICommand> cmdreg_=new Gen::Dictionary<string,ICommand>();
		public static void AddGlobalCommand(string commandName,ICommand command){
			cmdreg_.Add(commandName,command);
		}
		static ContextCompiler(){
			cmdreg_.Add("read",new ReadCommand());
			cmdreg_.Add("ret",new StringCommand("return;\r\n",true));
			cmdreg_.Add("error",new ErrorCommand());
			cmdreg_.Add("open",new StringCommand("push(this.OpenMarker);\r\n"));
			cmdreg_.Add("clos",new StringCommand(@"System.Collections.ArrayList list=new System.Collections.ArrayList();
while(this.stack.Count>0){
	object v=pop();
	if(v==this.OpenMarker)break;
	list.Add(pop());
}
list.Reverse();
push(list.ToArray());
"));
			cmdreg_.Add("jump",new JumpCommand());
			cmdreg_.Add("next",new StringCommand("this.wreader.ReadNext();\r\n"));
			cmdreg_.Add("ldwd",new StringCommand("push(word);\r\n"));
		}
		#endregion
	}

	/*
	internal class SubstringConnecter{
		Gen::SortedList<int,int> indices=new Gen::SortedDictionary<int,int>();
		private SubstringConnecter(){
			indices.Add(0,0);
		}


		private int GetOriginalIndex(int index){
			int u=indices.Count;
			int d=0;
			while(u-d>1){
				int c=(u+d)/2;
				if(index<indices.Keys[c])
					u=c;
				else d=c;
			}
			int linekey=indices.Keys[d];
			return index-linekey+indices[linekey];
		}
		public void Replace(int destIndex,int destLen,SubstringConnecter src){
			
		}
		public void RegistReplace(int destIndex,int destLen,SubstringConnecter src){

		}
		public void ResolveReplace(){

			this.replaces.Clear();
		}
		private Gen::SortedList<int,ReplaceInfo> replaces=new Gen::SortedList<int,ReplaceInfo>();
		private struct ReplaceInfo:System.IComparable<ReplaceInfo>{
			public int destIndex;
			public int destLen;
			public SubstringConnecter src;

			public int CompareTo(ReplaceInfo ri){
				return this.destIndex-ri.destIndex;
			}
		}


		public SubstringConnecter GetConnecter(string str){
			return insts.ContainsKey(str)?insts[str]:insts[str]=new SubstringConnecter();
		}
		private static Gen::Dictionary<string,SubstringConnecter> insts=new Gen::Dictionary<string,SubstringConnecter>();
	}//*/

	internal struct DefineMacro{
		public string name;
		public string[] args;

		private string content;
		public string Content{
			get{return this.content;}
			set{this.content=value.Replace('\r',' ').Replace('\n',' ');}
		}

		public void Apply(ref string str){
			System.Text.StringBuilder build=new System.Text.StringBuilder();
			build.Append(this.name);
			build.Append(@"[ \f\t\v]*\(");
			if(this.args.Length>0){
				build.AppendFormat(@"(?<{0}>(?:[^\r\n\,\)]|\\.)+)",this.args[0]);
				for(int i=1;i<this.args.Length;i++){
					build.AppendFormat(@"\,(?<{0}>(?:[^\r\n\,\)]|\\.)+)",this.args[i]);
				}
			}
			build.Append(@"\)");

			str=Rgx::Regex.Replace(str,build.ToString(),this.Evaluator);
		}
		private string Evaluator(Rgx::Match m){
			string r=this.content;
			for(int i=0;i<this.args.Length;i++){
				r=Rgx::Regex.Replace(r,@"\b"+this.args[i]+@"\b",m.Groups[this.args[i]].Value);
			}
			return r; 
		}
	}

	#region 各種構造
	internal struct Context{
		internal string name;
		Gen::List<ContextStage> data;

		public string ToCSharpSource(ContextCompiler cc){
			const string METHOD_SIGNATURE=@"
[System.Runtime.CompilerServices.CompilerGenerated]
private void ReadContext_{0}(){{
	string word;";
			const string LABEL_TEMPLATE=@"
#pragma warning disable 164
label_{0}:
#pragma warning restore 164
	word=this.wreader.CurrentWord;
	";
			//-----------------------------------
			System.Text.StringBuilder builder=new System.Text.StringBuilder();
			int stagenum=0;
			bool label=true;
			builder.AppendFormat(METHOD_SIGNATURE,this.name);
			foreach(ContextStage stage in this.data){
				// label
				if(label){
					builder.AppendFormat(LABEL_TEMPLATE,stagenum++);
				}else{
					builder.Append("else ");
				}

				// content
				string src=stage.ToCSharpSource(cc);
				builder.Append(src.Replace("\n","\n\t"));
				label=src[0]=='{';
			}
			builder.Append("\r\n}");
			return builder.ToString();
		}
		/// <summary>
		/// 指定した名前が Context の名前として有効である事を確認します。
		/// </summary>
		/// <returns>名前が Context の名前として有効である場合に true を返します。それ以外の場合には false を返します。</returns>
		private bool CheckName(){
			if(this.name.Length==0)return false;
			if("!\"#$%&'()=~|@[;:],./\\`{+*}<>?1234567890".IndexOf(this.name[0])>=0)return false;
			for(int i=0,iM=this.name.Length;i<iM;i++){
				if("!\"#$%&'()=~|@[;:],./\\`{+*}<>?".IndexOf(this.name[0])>=0)return false;
			}
			return true;
		}
		public static bool Read(ContextCompiler p,out Context context){
			if(p.wreader.CurrentType==WordType.Invalid){
				context=new Context();
				return false;
			}
			context=new Context();
			context.data=new Gen::List<ContextStage>();
			// context
			while(p.wreader.CurrentType!=WordType.Identifier||p.wreader.CurrentWord!="context"){
				p.wreader.LetterReader.SetError("context の開始には keyword 'context' が必要です。",0,null);
				if(!p.wreader.ReadNext())return false;
			}
			p.wreader.ReadNext();

			// ContextName
			while(p.wreader.CurrentType!=WordType.Identifier){
				p.wreader.LetterReader.SetError("keyword 'context' の後には識別子が必要です。",0,null);
				if(!p.wreader.ReadNext())return false;
			}
			context.name=p.wreader.CurrentWord;
			if(!context.CheckName()){
				p.wreader.LetterReader.SetError("指定した識別子は識別子として無効です。適切な物を指定して下さい",0,null);
			}
			p.wreader.ReadNext();

			// {
			while(p.wreader.CurrentType!=WordType.Operator||p.wreader.CurrentWord!="{"){
				p.wreader.LetterReader.SetError("context 宣言の後には context の中身が必要です。中身は { で始めて下さい。",0,null);
				if(!p.wreader.ReadNext())return false;
			}
			p.wreader.ReadNext();

			// List<ContextStage>
			ContextStage stage;
			while(ContextStage.Read(p,out stage)){
				context.data.Add(stage);
			}

			// }
			if(p.wreader.CurrentType==WordType.Operator&&p.wreader.CurrentWord=="}")p.wreader.ReadNext();
			return true;
		}
	}
	/// <summary>
	/// context の中での一つの分岐先を表現します。
	/// </summary>
	internal struct ContextStage{
		public Gen::List<ContextCondition> words;
		public Gen::List<ContextCommand> cmds;

		/// <summary>
		/// ContextStage の内容を C# コードにします。
		/// </summary>
		/// <param name="cc">今日は</param>
		/// <returns>分岐条件がある場合には if(&lt;分岐条件&gt;) で始まるコードを返します。
		/// 分岐条件が無く default の場合には { で始まるコードを返します。</returns>
		public string ToCSharpSource(ContextCompiler cc){
			System.Text.StringBuilder builder=new System.Text.StringBuilder();

			if(words.Count==0){
				if(this.cmds.Count==0)
					throw new System.ApplicationException("Fatal: ここに制御は来ない筈…ContextStage.Read を見よ");
				object key=this.cmds[0];
				cc.wreader.LetterReader.SetError("命令に対応する分岐標がないです。",key,key);
				return "";
			}

			//条件式の入り口
			//	条件のある時:	if(何とかかんとか){
			//	<def>条件の時:	{
			builder.Append("if(");
			bool def=false;
			bool firstword=true;
			foreach(ContextCondition w in words){
				if(w.word=="<def>"){
					if(words.Count>1){
						this.SetErrorButDefault(cc.wreader.LetterReader);
					}
					builder=new System.Text.StringBuilder();
					builder.Append("{\r\n\t");
					def=true;
					break;
				}else{
					if(firstword)firstword=false;else builder.Append("||");
					builder.Append(w.ToCSharpSource(cc));
				}
			}
			if(!def)builder.Append("){\r\n\t");

			//中身
			bool last=false;
			foreach(ContextCommand c in cmds){
				builder.Append(c.ToCSharpSource(cc,ref last).Replace("\n","\n\t"));
			}

			//末端
			if(builder[builder.Length-1]=='\t'){
				builder[builder.Length-1]='}';
			}else builder.Append('}');

			return builder.ToString();
		}
		private void SetErrorButDefault(LinearLetterReader lreader){
			foreach(ContextCondition w in words)
				if(w.word!="<def>")lreader.SetError("分岐標に <def> が含まれているのでこの指定は無意味です。",w,w);
		}
		/// <summary>
		/// ContextStage を読み取ります。
		/// </summary>
		/// <param name="p">読み取りを実行している ContextCompiler を指定します。</param>
		/// <param name="stage">読み取った stage を返します。</param>
		/// <returns>現在の Context 内の stage を全て読み終わってもう残りが無い時に false を返します。
		/// それ以外の場合には stage を読み取って true を返します。</returns>
		public static bool Read(ContextCompiler p,out ContextStage stage){
			stage=new ContextStage();
			if(p.wreader.CurrentType==WordType.Invalid)return false;
			stage.words=new System.Collections.Generic.List<ContextCondition>();
			stage.cmds=new System.Collections.Generic.List<ContextCommand>();

			// 分岐標 読み取り
			ContextCondition ctxword;
			while(p.wreader.CurrentType==WordType.Identifier){
				if(!ContextCondition.Read(p,out ctxword))break;
				stage.words.Add(ctxword);
			}
			if(p.wreader.CurrentType==WordType.Operator&&p.wreader.CurrentWord==":")p.wreader.ReadNext();

			// 命令 読み取り
			ContextCommand ctxcmd;
			while(p.wreader.CurrentType==WordType.Identifier){
				if(!ContextCommand.Read(p,out ctxcmd))break;
				stage.cmds.Add(ctxcmd);
			}
			if(p.wreader.CurrentType==WordType.Operator&&p.wreader.CurrentWord==";")p.wreader.ReadNext();

			// 結果判定
			if(stage.cmds.Count==0){
				// 何も (命令も分岐標も) 読み取っていない時
				if(stage.words.Count==0)return false;

				// 命令が一つもないのに分岐標がある時
				p.wreader.LetterReader.SetError("対応する命令列が存在しません。",0,null);
				stage.cmds.Add(new ContextCommand("ret",""));
			}
			return true;
		}
	}
	//===============================================================
	//		class:ContextCondition
	//===============================================================
	/// <summary>
	/// 分岐標 (context の中で分岐を司る単語) を表現します。
	/// </summary>
	internal class ContextCondition{
		public string wordtype;
		public string word;
		private ContextCondition(string word,string wordtype){
			this.word=word;
			this._wordtype=wordtype;
		}
		/// <summary>
		/// ContextCondition を C# のコードに変換します。
		/// </summary>
		/// <param name="cc">現在使用している ContextCompiler を指定します。</param>
		/// <returns>Boolean 値を返す条件式を C# で表現して返します。
		/// default の場合 (どんな場合でも受容する場合) には "" を返します。</returns>
		public string ToCSharpSource(ContextCompiler cc){
			if(this.word=="<def>")return "";
			if(this.word=="<nul>")return "(this.wreader.CurrentType.value=="+WordType.Invalid.value.ToString()+")";
			if(this.word==""){
				if(this.wordtype=="")
					throw new System.ApplicationException("Fatal: ここに制御は来ない筈…ContextWordReader が空文字列の Identifier を返さぬ限り");
				return "("+this._wordtype+")";
			}else{
				string r=this.wordtype==""?"(":"("+this._wordtype+"&&";
				return r+"word=="+this.word_quoted+")";
			}
		}
		private string word_quoted{
			get{return "\""+this.word.Replace(@"\",@"\\").Replace("\"","\\\"")+"\"";}
		}
		/// <summary>
		/// 単語の種類を簡単な文字列で設定します。
		/// 亦、単語の種類を判定する条件式を取得します。
		/// </summary>
		private string _wordtype{
			get{
				if(this.wordtype=="")return "";
				return "this.wreader.CurrentType.value=="+this.wordtype;
			}
			set{
				if(value==""){
					this.wordtype="";
					return;
				}
				switch(value.ToLower()){
					case "op":case "operator":
						this.wordtype=WordType.Operator.value.ToString();
						break;
					case "id":case "identifier":
						this.wordtype=WordType.Identifier.value.ToString();
						break;
					case "lit":case "literal":
						this.wordtype=WordType.Literal.value.ToString();
						break;
					case "@":case "attr":case "attribute":
						this.wordtype=WordType.Attribute.value.ToString();
						break;
					case "comment":
						this.wordtype=WordType.Comment.value.ToString();
						break;
					case "elem":case "element":
						this.wordtype=WordType.Element.value.ToString();
						break;
					case "keyword":case "key":
						this.wordtype=WordType.KeyWord.value.ToString();
						break;
					case "pref":case "prefix":
						this.wordtype=WordType.Prefix.value.ToString();
						break;
					case "suf":case "suffix":
						this.wordtype=WordType.Suffix.value.ToString();
						break;
					case "mod":case "modifier":
						this.wordtype=WordType.Modifier.value.ToString();
						break;
					case "tag":
						this.wordtype=WordType.Tag.value.ToString();
						break;
					case "txt":case "text":
						this.wordtype=WordType.Text.value.ToString();
						break;
					case "space":
						this.wordtype=WordType.Space.value.ToString();
						break;
					default:
						try{
							this.wordtype=int.Parse(value).ToString();
						}catch{
							this.wordtype="";
						}
						break;
				}
			}
		}
		public static bool Read(ContextCompiler p,out ContextCondition contextword){
			contextword=null;
			string wordtype="";
			string word;
			if(p.wreader.CurrentType==WordType.Invalid)return false;
		start:
			if(p.wreader.CurrentType!=WordType.Identifier){
				if(p.wreader.CurrentWord==":"||p.wreader.CurrentWord=="}")return false;
				throw new System.ApplicationException("Fatal: ここに制御は来る筈ない。ContextWordReader の実装が怪しい");
			}
			// word 取得
			word=p.wreader.CurrentWord;
			if(word[0]=='<'&&word[word.Length-1]=='>'){
				if(word!="<def>"&&word!="<nul>"){
					// TODO: <> でユーザ定義の分岐標を指定できる様にする予定
					p.wreader.LetterReader.SetError(word+" という種類の分岐先は定義されていません",0,null);
					if(!p.wreader.ReadNext())return false;
					goto start;
				}
			}else if(word[0]=='['){
				int i=word.IndexOf(']',1);
				if(i>0){
					// type 指定 [] がある場合
					wordtype=word.Substring(1,i-1);
					word=word.Substring(i+1);
				}
			}
			contextword=new ContextCondition(word,wordtype);
			p.wreader.LetterReader.CopyPosition(0,contextword);	// 後で起こったエラーの際の参照
			p.wreader.ReadNext();
			return true;
		}
	}
	#endregion

	#region Command
	//===============================================================
	//		class:ContextCommand
	//===============================================================
	/// <summary>
	/// context の中での命令を表現します。
	/// </summary>
	internal class ContextCommand{
		public string command;
		public string argument;
		private ContextCommand(){
			this.command="";
			this.argument="";
		}
		public ContextCommand(string command,string argument){
			this.command=command;
			this.argument=argument;
		}
		public string ToCSharpSource(ContextCompiler cc,ref bool last){
			ICommand icmd=cc.cmdreg[this.command];

			// last
			if(last){
				cc.wreader.LetterReader.SetError("到達できない命令です。",this,this);
				return "/* Command '"+this.command+"' */\r\n";
			}
			last=icmd.RequireLast;

			// require argument
			if(this.argument==""&&icmd.RequireArgument){
				cc.wreader.LetterReader.SetError(
					"命令 '"+this.command+"' は引数を必要とします。() で括って引数を指定して下さい。",
					this,this);
				return "/* Command '"+this.command+"' */\r\n";
			}

			try{
				return icmd.ToCSharpSource(cc,this.argument);
			}catch(System.Exception e){
				cc.wreader.LetterReader.SetError("命令コード生成中の例外: "+e.Message,this,this);
				return "/* Command '"+this.command+"' */\r\n";
			}
		}

		public static bool Read(ContextCompiler cc,out ContextCommand contextcmd){
			contextcmd=new ContextCommand();
			AbstractWordReader wreader=cc.wreader;
		start:
			if(wreader.CurrentType==WordType.Invalid)return false;
			string word=wreader.CurrentWord;
			if(wreader.CurrentType!=WordType.Identifier){
				if(word==";"||word=="}")return false;
				throw new System.ApplicationException("Fatal: ここに制御は来る筈ない。ContextWordReader の実装が怪しい");
			}

			// argument があればそれを読み取り。
			wreader.LetterReader.CopyPosition(0,contextcmd);	// 後でエラーが発生した時等の為。
			wreader.LetterReader.CopyPosition(0,1);	// *
			wreader.LetterReader.StoreCurrentPos(2);	// *
			if(wreader.ReadNext()){
				if(wreader.CurrentType==WordType.Suffix){
					contextcmd.argument=wreader.CurrentWord;
					wreader.LetterReader.StoreCurrentPos(2); // *
					wreader.ReadNext();

					// 二重引数を跳ばし読み
					if(wreader.CurrentType==WordType.Suffix){
						wreader.LetterReader.SetError("二重に引数を指定する事は出来ません。",0,null);
					}
					while(wreader.CurrentType==WordType.Suffix)wreader.ReadNext();
				}
			}

			// command が処理可能な物かどうかを確認。
			if(!cc.cmdreg.ContainsKey(word)){
				cc.wreader.LetterReader.SetError(word+" という種類の命令は定義されていません",1,2); // *
				goto start; // 読み直し
			}

			contextcmd.command=word;
			return true;
		}

		//-------------------------------------------------
		public static string ResolveReferenceWord(string source){
			source=Rgx::Regex.Replace(source,@"\bpop\b","this.stack.Pop");
			source=Rgx::Regex.Replace(source,@"\bpush\b","this.stack.Push");
			return source;
		}
	}

	//===============================================================
	//		class:ContextCommand
	//===============================================================
	public interface ICommand{
		bool RequireLast{get;}
		/// <summary>
		/// 引数が必要な場合に true を返します。
		/// 引数を省略可能な場合、亦は引数を使用しない場合その他には false を返します。
		/// </summary>
		bool RequireArgument{get;}
		/// <summary>
		/// Context 命令を C# のコードに変換します。
		/// </summary>
		/// <param name="cc">生成を行っている ContextCompiler を指定します。</param>
		/// <param name="argument">命令の引数がある場合に引数を指定します。引数がない場合には空文字列 "" を指定します。</param>
		/// <returns>生成した C# のコードを返します。</returns>
		/// <exception cref="System.Exception">
		/// 変換が妥当でない、亦は変換が実行できない場合に発生します。
		/// 失敗の理由があればそれも System.Exception を通じて伝達されます。
		/// </exception>
		string ToCSharpSource(ContextCompiler cc,string argument);
	}

	internal struct StringCommand:ICommand{
		bool islast;
		bool hasargument;
		string content;
		public StringCommand(string content):this(content,false){}
		public StringCommand(string content,bool requirelast){
			this.islast=requirelast;
			this.content=ContextCommand.ResolveReferenceWord(content);
			this.hasargument=content.IndexOf("$$")>=0;
		}
		public bool RequireLast{
			get{return this.islast;}
			set{this.islast=value;}
		}
		public bool RequireArgument{
			get{return this.hasargument;}
		}
		public string ToCSharpSource(ContextCompiler cc,string argument){
			return this.hasargument?this.content.Replace("$$",argument):this.content;
		}
	}
	internal struct ReadCommand:ICommand{
		public bool RequireLast{get{return false;}}
		public bool RequireArgument{
			get{return true;}
		}
		public string ToCSharpSource(ContextCompiler cc,string argument){
			if(!cc.ContainsContext(argument)){
				throw new System.ArgumentException("指定した名前  '"+argument+"' という Context は定義されていません。","argument");
			}
			return "this.ReadContext_"+argument+"();\r\n";
		}
	}
	internal struct ErrorCommand:ICommand{
		public bool RequireLast{get{return false;}}
		public bool RequireArgument{
			get{return false;}
		}
		public string ToCSharpSource(ContextCompiler cc,string argument){
			if(argument==""){
				return "this.wreader.LetterReader.SetError(\"解析中の不明なエラー\",0,null);\r\n";
			}else{
				return "this.wreader.LetterReader.SetError(\"エラー: "+argument.Replace(@"\",@"\\").Replace("\"","\\\"")+"\",0,null);\r\n";
			}
		}
	}
	internal struct JumpCommand:ICommand{
		public bool RequireLast{get{return true;}}
		public bool RequireArgument{
			get{return true;}
		}
		public string ToCSharpSource(ContextCompiler cc,string argument){
			const string ARG_NOTINTEGER="jump 命令の引数には 0 以上の整数を指定して下さい。";
			int x;
			try{
				x=int.Parse(argument);
				if(x<0)throw new System.ArgumentException(ARG_NOTINTEGER,"argument");
			}catch(System.Exception e){
				throw new System.ArgumentException(ARG_NOTINTEGER,"argument",e);
			}
			return "goto label_"+x.ToString()+";\r\n";
		}
	}

	//===============================================================
	//		class:UserCommand
	//===============================================================
	internal struct UserCommand:ICommand{
		public string name;
		public string content;
		// TODO: 後で指定できる様にする
		public bool RequireLast{get{return false;}}
		// TODO: 後で指定できる様にする
		public bool RequireArgument{get{return false;}}

		public string ToCSharpSource(ContextCompiler cc,string argument){
			return this.content;
		}
		public static bool Read(ContextCompiler cc,out UserCommand usercmd){
			usercmd=new UserCommand();
			AbstractWordReader wreader=cc.wreader;

			// command
			while(wreader.CurrentType!=WordType.Identifier||wreader.CurrentWord!="command"){
				wreader.LetterReader.SetError("command の開始には keyword 'command' が必要です。",0,null);
				if(!wreader.ReadNext())return false;
			}
			wreader.ReadNext();

			// CommandName
			while(wreader.CurrentType!=WordType.Identifier){
				wreader.LetterReader.SetError("keyword 'context' の後には識別子が必要です。",0,null);
				if(!wreader.ReadNext())return false;
			}
			usercmd.name=wreader.CurrentWord;
			wreader.ReadNext();

			// {}
			while(wreader.CurrentType!=WordType.Literal){
				wreader.LetterReader.SetError("command 宣言の後には command の中身が必要です。中身は { で始めて下さい。",0,null);
				if(!wreader.ReadNext())return false;
			}
			usercmd.content=FormatSource(wreader.CurrentWord);
			wreader.ReadNext();

			return true;
		}
		/// <summary>
		/// 空行を除去し、無駄なインデントを除きます。
		/// </summary>
		/// <param name="source">空行などを含んでいる可能性のあるソースを指定します。</param>
		/// <returns>整形した後の文字列を返します。</returns>
		public static string FormatSource(string source){
			string[] strs=source.Split(new char[]{'\r','\n'});
			int[] lens=new int[strs.Length];

			int i,j;int minlen=0x100;
			for(i=0,j=0;i<strs.Length;i++){
				if(strs[i].Trim().Length==0)continue;
				string str=strs[i];

				// インデントの長さ
				int len=0;
				while(str.Length>0){
					if(str[0]=='\t'){
						len+=4;
					}else if(str[0]==' '){
						len+=1;
					}else break;
					str=str.Substring(1);
				}

				// 最小インデント
				if(len<minlen)minlen=len;

				// 空行を跳ばして格納
				lens[j]=len;
				strs[j++]=str;
			}

			System.Text.StringBuilder build=new System.Text.StringBuilder();
			for(i=0;i<j;i++){
				int indent=lens[i]-minlen;
				build.Append(indent%4==0?new string('\t',indent/4):new string(' ',indent));
				build.AppendLine(strs[i]);
			}
			return ContextCommand.ResolveReferenceWord(build.ToString());
		}
	}
	#endregion
}
