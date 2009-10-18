using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;
using Rgx=System.Text.RegularExpressions;
using REP=afh.Text.RegexPatterns;

namespace afh.Design{
	internal interface IDefineProcessor{
		string Instantiate(string[] args);
	}
	internal abstract class DefineProcessorBase:IDefineProcessor{
		public abstract string Instantiate(string[] args);

		const string PARAM_HEAD="__AFH_DEFINE_PARAM__";
		private static Rgx::Regex rx_param=new Rgx::Regex(@"\b"+PARAM_HEAD+@"(?<index>\d+)\b",Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		protected static string ReplaceParameters(string content,string[] args){
			return rx_param.Replace(content,delegate(Rgx::Match m) {
				string arg=args[int.Parse(m.Groups["index"].Value)];
				return ResolveQuotedArgument(arg);
			}).Replace("##","");
		}
		/// <summary>
		/// "#なんとかかんとか#" の形の引数を解決します。
		/// </summary>
		/// <param name="arg">"#なんとかかんとか#" の形になっている可能性のある引数文字列を指定します。</param>
		/// <returns>
		/// 指定した文字列の内容が \"#なんとかかんとか#\" の形になっていた場合には、
		/// 「なんとかかんとか」の部分のエスケープシーケンスを解決した文字列を返します。
		/// それ以外の場合には arg を素も儘返します。
		/// </returns>
		protected static string ResolveQuotedArgument(string arg){
			if(arg.Length>=4&&arg.StartsWith("\"#")&&arg.EndsWith("#\"")){
				return Rgx::Regex.Unescape(arg.Substring(2,arg.Length-4));
			}
			return arg;
		}
	}
	internal class DefineProcessor:DefineProcessorBase{
		/// <summary>
		/// 置換先の文字列を保持します。
		/// </summary>
		string content;
		/// <summary>
		/// DefineProcessor を初期化します。
		/// </summary>
		/// <param name="content">
		/// 置換先の文字列を指定します。
		/// 引数は _AFH_DEFINE_PARAM__数値 の形で埋め込んで下さい。
		/// </param>
		public DefineProcessor(string content){
			this.content=content;
		}

		public override string Instantiate(string[] args){
			return ReplaceParameters(this.content,args);
		}
	}
	//*
	/// <summary>
	/// __afh::なんとか で使用出来るマクロ定義を提供します。
	/// </summary>
	/// <remarks>
	/// //#define FOO __afh::bar(...)
	/// の形で使用します。
	/// </remarks>
	internal class AfhDefineProcessor:DefineProcessorBase{
		string name;
		string[] arg_templates;
		internal AfhDefineProcessor(string name,string[] args){
			this.name=name;
			this.arg_templates=args;
		}
		//============================================================
		//		処理
		//============================================================
		public override string Instantiate(string[] args){
			// 引数の変換
			string[] afh_args=new string[arg_templates.Length];
			for(int i=0;i<arg_templates.Length;i++)
				afh_args[i]=ReplaceParameters(arg_templates[i],args);

			// 実際の処理
			return methods[this.name](afh_args);
		}
		/// <summary>
		/// 範囲外読み取りに対し空文字列を返す配列です。
		/// </summary>
		private class StringArray{
			string[] array;
			public StringArray(string[] array){
				this.array=array;
			}
			public string this[int index]{
				get{
					if(index<0||this.array.Length<=index)
						return "";
					return this.array[index];
				}
			}
			public static implicit operator StringArray(string[] array){
				return new StringArray(array);
			}
		}

		//============================================================
		//		初期化
		//============================================================
		static string re_arg=@"\s*(?<arg>(?:"
			+@"[^\(\)\,\'\""]"
			+@"|"+REP.DOUBLEQUOTED
			+@"|"+REP.SINGLEQUOTED
			+@"|\("+REP.CreateMatchParen('(',')',true)+@"\)"
		+@")+)\s*";
		static string re_args="(?:"+re_arg+@"(?:\,"+re_arg+")*)?";
		static Rgx::Regex rx_afhdefine=new Rgx::Regex(
			@"^__afh\:\:(?<name>\w+)\s*\("+re_args+@"\)$",
			Rgx::RegexOptions.Singleline|Rgx::RegexOptions.Compiled
			);
		/// <summary>
		/// 指定した define 定義が __afh:: 型であるかどうかを判定し、
		/// __afh:: 型であると判断された場合には、そのハンドラを作成して返します。
		/// </summary>
		/// <param name="content"></param>
		/// <param name="proc"></param>
		/// <returns>__afh:: 型であると判定された場合に true を返します。</returns>
		public static bool TryCreateInstance(string content,out IDefineProcessor proc){
			do{
				Rgx::Match m=rx_afhdefine.Match(content);
				if(!m.Success)break;

				string name=m.Groups["name"].Value;
				if(!methods.ContainsKey(name))break;

				Rgx::CaptureCollection cc=m.Groups["arg"].Captures;
				string[] args=new string[cc.Count];
				for(int i=0;i<cc.Count;i++)
					args[i]=cc[i].Value.Trim();

				proc=new AfhDefineProcessor(name,args);
				return true;
#pragma warning disable 162
			}while(false);
#pragma warning restore 162

			// 失敗した場合
			proc=null;
			return false;
		}
		//============================================================
		//		関数定義
		//============================================================
		private static Gen::Dictionary<string,System.Converter<StringArray,string>> methods
			=new Gen::Dictionary<string,System.Converter<StringArray,string>>();
		static AfhDefineProcessor(){
			methods.Add("equal",delegate(StringArray args){
				return args[0]==args[1]?"true":"false";
			});
			methods.Add("iif",delegate(StringArray args){
				int c;
				bool eval=args[0]=="true"||int.TryParse(args[0],out c)&&c!=0;
				return eval?args[1]:args[2];
			});
		}
	}
	//*/
	// TODO: DefineImportProcessor
	internal class DefineImportProcessor{
		//========================================================
		//	dllimport
		//========================================================
		//	マクロ置換だけでなく、自分で定義した関数によってマクロを処理するという事も可能にする
		//--------------------------------------------------------
		public static void ProcessDllImport(){
		//	if(this.content.StartsWith("__afh::import(")&&this.content.EndsWith(")")){
		//		this.isimport=true;
		//	}
		
		}
	}

}