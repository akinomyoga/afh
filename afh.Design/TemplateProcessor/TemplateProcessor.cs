using System;
using Gen=System.Collections.Generic;
using System.Text;
using VsInterop=Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

using Interop=System.Runtime.InteropServices;
using Rgx=System.Text.RegularExpressions;
using REP=afh.Text.RegexPatterns;

namespace afh.Design {
	[Interop::ComVisible(true)]
	[Interop::Guid("CCF9346C-04B7-4dba-8CF4-EEE1251EF858")]
	[CodeGeneratorRegistration(typeof(TemplateProcessor),"TemplateProcessor",vsContextGuids.vsContextGuidVCSProject,GeneratesDesignTimeSource=true)]
	[ProvideObject(typeof(TemplateProcessor))]
	public class TemplateProcessor:TextFileGenerator{
		public TemplateProcessor():base(){}

		public override string OutputExtension{
			get{return ".gen.cs";}
		}

		public override string Translate(string inputText,string defaultNamespace,VsInterop::IVsGeneratorProgress generateProgress){
			afh.Text.MultilineString line_column=new afh.Text.MultilineString(inputText);
			ReportError report=delegate(string msg,int index){
				int line,column;
				line_column.GetLineAndColumn(index,out line,out column);
				generateProgress.GeneratorError(1,0,msg,(uint)line,(uint)column);
			};

			ResolveInclude(ref inputText,report);

			DeleteDirective.Resolve(ref inputText,report);

			ResolveTemplate(ref inputText,report);

			ResolveDefine(ref inputText,report);

			return NormalizeCrlf(Header+inputText);
		}

		const string _S_			=REP.SPACE_ExceptNewLine+"+";
		const string _s_			=REP.SPACE_ExceptNewLine+"*";
		const string LINE_S			="^"+_s_;
		const string LINE_E			=_s_+@"(?:\r\n?|\n)?$";

		#region process:include
		const string INCLUDE_DECL	=LINE_S+@"\/\/\#include"+_S_+@"""(?<file>(?:[^\\""]|\\.)*)"""+LINE_E;
		static Rgx::Regex rx_include_decl=new Rgx::Regex(INCLUDE_DECL,Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		static string include_dir	=System.IO.Path.Combine(
			System.IO.Path.GetDirectoryName(typeof(TemplateProcessor).Assembly.Location),
			"include"
			);
		/// <summary>
		/// #include "..." ディレクティブを処理します。
		/// </summary>
		private static void ResolveInclude(ref string text,ReportError report){
			text=rx_include_decl.Replace(text,delegate(Rgx::Match m){
				string filename=m.Groups["file"].Value;
				string ret;
				if(TryReadFile(System.IO.Path.Combine(include_dir,filename),out ret))return ret;
				//if(TryReadFile(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),filename),out ret))return ret;
				report("指定したファイル '"+filename+"' は見つかりません。",m.Index);
				return m.Value;
			});
		}
		private static bool TryReadFile(string path,out string content){
			if(System.IO.File.Exists(path)){
				content=System.IO.File.ReadAllText(path,System.Text.Encoding.Default);
				return true;
			}else{
				content=null;
				return false;
			}
		}
		#endregion

		#region process:define
		const string DEFINE_PARAM	=_s_+"(?<param>"+REP.WORD+")"+_s_;
		const string DEFINE_PARAMS	=_s_+@"(?:\("+DEFINE_PARAM+@"(?:\,"+DEFINE_PARAM+@")*\))?"+_s_;
		const string DEFINE_DECL	=LINE_S+@"\/\/\#define"+_S_+"(?<name>"+REP.WORD+")(?:"+DEFINE_PARAMS+")?"+_S_+@"(?<content>\S.*?)"+LINE_E;
		static Rgx::Regex rx_define_decl=new Rgx::Regex(DEFINE_DECL,Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		const string DEFINE_ARG		=@"\s*(?<arg>(?:[^\(\)\,\'\""]|"+REP.DOUBLEQUOTED+"|"+REP.SINGLEQUOTED+@")+)\s*";
		const string DEFINE_ARGS1	=@"\s*\("+DEFINE_ARG+@"\)";
		const string DEFINE_ARGS2	=@"\s*\("+DEFINE_ARG+@"(?:\,"+DEFINE_ARG+@"){{{0}}}\)";
		private static void ResolveDefine(ref string text,ReportError report){
			// 一つずつ #define を読込・適用
			Rgx::MatchCollection mc=rx_define_decl.Matches(text);
			for(int i=mc.Count-1;i>=0;i--){
				new Define(mc[i]).Apply(ref text,report);
			}
		}
		//static afh.Application.Log log=afh.Application.LogView.Instance.CreateLog("tempProc");
		private struct Define{
			string name;
			//string content;
			string rp_entity;
			int start;

			IDefineProcessor proc;

			const string PARAM_HEAD="__AFH_DEFINE_PARAM__";

			public Define(Rgx::Match m){
				this.name=m.Groups["name"].Value;
				string content=m.Groups["content"].Value.Trim();
				
				int c_param=0;
				foreach(Rgx::Capture param in m.Groups["param"].Captures){
					content=Rgx::Regex.Replace(content,@"\b"+param.Value+@"\b",PARAM_HEAD+c_param++.ToString());
				}
				//this.content=content;

				string rp=@"\b"+this.name+@"\b";
				if(c_param==1)
					rp+=DEFINE_ARGS1;
				else if(c_param>0)
					rp+=string.Format(DEFINE_ARGS2,c_param-1);
				this.rp_entity=rp;

				this.start=m.Index+m.Length;

				//-- 適当なハンドラを初期化
				if(AfhDefineProcessor.TryCreateInstance(content,out this.proc))
					return;
				this.proc=new DefineProcessor(content);
			}
			public void Apply(ref string text,ReportError report){
				Define self=this;
				text=Rgx::Regex.Replace(text,this.rp_entity,delegate(Rgx::Match m){
					if(m.Index<self.start)return m.Value;

					return self.Instantiate(m.Groups["arg"].Captures);
				});
			}

			static Rgx::Regex rx_param=new Rgx::Regex(@"\b"+PARAM_HEAD+@"(?<index>\d+)\b",Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
			private string Instantiate(Rgx::CaptureCollection cc){
				string[] args=new string[cc.Count];
				for(int i=0;i<cc.Count;i++)
					args[i]=cc[i].Value.TrimEnd();

				return this.proc.Instantiate(args);

				/*
				return rx_param.Replace(this.content,delegate(Rgx::Match m){
					string arg=cc[int.Parse(m.Groups["index"].Value)].Value.TrimEnd();
					if(arg.Length>=4&&arg.StartsWith("\"#")&&arg.EndsWith("#\"")){
						arg=Rgx::Regex.Unescape(arg.Substring(2,arg.Length-4));
					}
					return arg;
				}).Replace("##","");
				//*/
			}
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
		#endregion

		#region process:template
		const string GROUP_NAME		=@"(?<name>"+REP.WORD+@")";
		const string GROUP_PARAM	=@"(?<param>(?:[^\<\>\'\""\r\n]|"+REP.DOUBLEQUOTED+"|"+REP.SINGLEQUOTED+")+)";

		const string TMPL_PARAM		=_s_+"(?<param>"+REP.WORD+")"+_s_;
		const string TMPL_PARAMS	=_s_+@"\<"+DEFINE_PARAM+@"(?:\,"+DEFINE_PARAM+@")*\>"+_s_;
		const string TMPL_START		=LINE_S+@"\/\/\#→template"+_S_+@"(?<name>"+REP.WORD+@")"+TMPL_PARAMS+LINE_E;
		const string TMPL_END		=LINE_S+@"\/\/\#←template"+LINE_E;

		const string TMPL_ARG		=@"\s*(?<arg>(?:[^\<\>\,\'\""]|"+REP.DOUBLEQUOTED+"|"+REP.SINGLEQUOTED+@")+)\s*";
		private static string TMPL_ARGS(int count){
			if(count==1)
				return @"\s*\<"+TMPL_ARG+@"\>";
			else
				return @"\s*\<"+TMPL_ARG+@"(?:\,"+TMPL_ARG+@"){"+(count-1).ToString()+@"}\>";
		}

		// 番号付け前の Entities
		const string DECL_ENTITY0	=LINE_S+@"\/\/(?<sharp>\#)"+REP.WORD+@"\s*\<"+GROUP_PARAM+@"\>"+LINE_E;
		const string INLN_ENTITY0	=@"\/\*(?<sharp>\#)"+REP.WORD+@"\s*\<"+GROUP_PARAM+@"\>\s*\*\/";

		// 番号付けした後の Entities
		const string DIREC_HEAD		=@"\#(?<num>\d+)\#";
		const string DECL_ENTITY	=LINE_S+@"\/\/\#(?<num>\d+)\#{0}\s*\<"+GROUP_PARAM+@"\>"+LINE_E;
		const string INLN_ENTITY	=@"\/\*\#(?<num>\d+)\#{0}\s*\<"+GROUP_PARAM+@"\>\s*\*\/";
		const string TMPL_DECL_ENTT	=LINE_S+@"\/\/"+DIREC_HEAD+"{0}"+LINE_E;
		const string TMPL_INLN_ENTT	=@"\/\*"+DIREC_HEAD+@"{0}\*\/";

		private static Rgx::Regex rx_template_s=new Rgx::Regex(TMPL_START	,Rgx::RegexOptions.Compiled|Rgx::RegexOptions.Multiline);
		private static Rgx::Regex rx_template_e=new Rgx::Regex(TMPL_END		,Rgx::RegexOptions.Compiled|Rgx::RegexOptions.Multiline);
		private static Rgx::Regex rx_entities_raw=new Rgx::Regex(
			DECL_ENTITY0+"|"+INLN_ENTITY0,
			Rgx::RegexOptions.Compiled|Rgx::RegexOptions.Multiline);
		private static Rgx::Regex rx_entities=new Rgx::Regex(
			string.Format(DECL_ENTITY,GROUP_NAME)+"|"+string.Format(INLN_ENTITY,GROUP_NAME),
			Rgx::RegexOptions.Compiled|Rgx::RegexOptions.Multiline);

		private static void ResolveTemplate(ref string text,ReportError report){
			int i;
			// Entity に番号付け
			i=0;
			Gen::Dictionary<string,int> indices=new Gen::Dictionary<string,int>();
			text=rx_entities_raw.Replace(text,delegate(Rgx::Match m){
				string key=i++.ToString();
				int sharp=m.Groups["sharp"].Index;
				indices[key]=sharp-2;
				sharp-=m.Index;
				return m.Value.Substring(0,sharp)+"#"+key+m.Value.Substring(sharp);
			});

			// 一つずつ template を読込・適用
			Rgx::MatchCollection mc=rx_template_s.Matches(text);
			for(i=mc.Count-1;i>=0;i--){
				ResolveTemplate(ref text,mc[i],report);
			}

			// #←template が残っていた場合
			foreach(Rgx::Match m in rx_template_e.Matches(text)){
				report("//#←template に対応する //#→template が存在しません。",m.Index);
			}

			// Entity が残っていた場合
			foreach(Rgx::Match m in rx_entities.Matches(text)) {
				report("解決出来ない template 利用が存在します。",indices[m.Groups["num"].Value]);
			}
			// 方法
			// 1. ENTITY に数字を付け、元の位置との対応を記録
			// 2. ENTITY+数字 を検索し、数字部分から元の位置を取得
		}
		/// <summary>
		/// 指定した "//#→template" の解決を行います。
		/// </summary>
		/// <param name="text">処理する対象の文字列を指定します。</param>
		/// <param name="m_s">rx_template_s "//#→template" の一致を指定します。</param>
		/// <param name="report">エラー報告先を指定します。</param>
		/// <returns>成功した場合に true を返します。失敗した場合に false を返します。</returns>
		private static bool ResolveTemplate(ref string text,Rgx::Match m_s,ReportError report){
			const string TEMP_LOC="__AFH_TEMPLATE_LOC__";

			Rgx::Match m_e=rx_template_e.Match(text,m_s.Index);
			if(!m_e.Success){
				report("//#→template に対応する //#←template が存在しません。",m_s.Index);
				return false;
			}
			int ss=m_s.Index;
			int se=ss+m_s.Length;
			int es=m_e.Index;
			int ee=es+m_e.Length;

			string t_name=m_s.Groups["name"].Value;
			Template temp=new Template(t_name,m_s.Groups["param"].Captures,text.Substring(se,es-se));
			text=text.Substring(0,ss)+TEMP_LOC+text.Substring(ee);

			//
			// #t_name<param> の変換
			//
			temp.ApplyToDeclEntity(ref text,ss);

			//
			// /*#t_name<param>*/ の変換 (template 関数に利用)
			//
			string insts=temp.ApplyToInlineEntity(ref text,ss);

			//
			// /*#t_name<param>*/ の実体化関数を配置
			//
			text=text.Replace(TEMP_LOC,insts);

			return true;
		}

		private struct Template{
			const string PARAM_REP	="__AFH_TEMPLATE_PARAM__";
			const string NAME_REP	="__AFH_TEMPLATE_NAME__";
			private string name;
			private string content;
			private int count;

			public Template(string name,Rgx::CaptureCollection param,string content){
				int i=0;
				
				foreach(Rgx::Capture para in param)
					content=Rgx::Regex.Replace(content,@"\b"+para.Value+@"\b",PARAM_REP+i++.ToString());
				content=Rgx::Regex.Replace(content,@"\b"+name+@"\b(?:\#\#)?",NAME_REP);

				this.name=name;
				this.content=content;
				this.count=i;
			}
			/// <summary>
			/// template 宣言参照を解決します。
			/// </summary>
			/// <param name="text">変換対象の文字列を指定します。</param>
			/// <param name="start">変換の開始点を指定します。</param>
			public void ApplyToDeclEntity(ref string text,int start){
				Template temp=this;
				string rp=string.Format(TMPL_DECL_ENTT,this.name+TMPL_ARGS(count));
				text=Rgx::Regex.Replace(text,rp,delegate(Rgx::Match m){
					// より後の物しか変換しない
					if(m.Index<start)return m.Value;

					return temp.Instantiate(temp.name,m.Groups["arg"].Captures);
				},Rgx::RegexOptions.Multiline);
			}
			/// <summary>
			/// inline の template 参照を解決します。
			/// </summary>
			/// <param name="text">変換対象の文字列を指定します。</param>
			/// <param name="start">変換の開始点を指定します。</param>
			/// <returns>インスタンス化した関数のコードを返します。</returns>
			public string ApplyToInlineEntity(ref string text,int start){
				int i=0;
				Gen::Dictionary<string,string> instances=new Gen::Dictionary<string,string>();
				System.Text.StringBuilder build=new System.Text.StringBuilder();
				Template temp=this;

				string rp=string.Format(TMPL_INLN_ENTT,this.name+TMPL_ARGS(count));
				text=Rgx::Regex.Replace(text,rp,delegate(Rgx::Match m) {
					// より後の物しか変換しない
					if(m.Index<start)return m.Value;

					string args=GetArgStamp(m.Groups["arg"].Captures);
					if(!instances.ContainsKey(args)) {
						instances[args]=temp.name+"_instance_"+i++.ToString();
						build.AppendLine(temp.Instantiate(instances[args],m.Groups["arg"].Captures));
					}
					return instances[args];
				},Rgx::RegexOptions.Multiline);

				return build.ToString();
			}
			private static string GetArgStamp(Rgx::CaptureCollection args){
				string ret="";
				foreach(Rgx::Capture c in args)ret+=(ret==""?"":",")+c.Value;
				return ret;
			}

			static Rgx::Regex rx_param=new Rgx::Regex(@"(?:\#\#)?\b"+PARAM_REP+@"(?<index>\d+)\b(?:\#\#)?",Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
			private string Instantiate(string name,Rgx::CaptureCollection cc){
				return rx_param.Replace(this.content,delegate(Rgx::Match m){
					string arg=cc[int.Parse(m.Groups["index"].Value)].Value;
					if(arg.StartsWith("##"))arg=arg.Substring(2);
					if(arg.EndsWith("##"))arg=arg.Substring(0,arg.Length-2);
					// "#hogehoge#" という指定の時
					if(arg.Length>=4&&arg.StartsWith("\"#")&&arg.EndsWith("#\""))
						arg=Rgx::Regex.Unescape(arg.Substring(2,arg.Length-4)); 
					return arg;
				}).Replace(NAME_REP,name).Trim('\r','\n');
			}
		}
		#endregion
	}
	/// <summary>
	/// エラーの報告に使用します。
	/// </summary>
	/// <param name="msg">エラーの内容を説明する文字列を指定します。</param>
	/// <param name="index">何文字目でエラーが発生したかを指定します。</param>
	internal delegate void ReportError(string msg,int index);

	internal static class DeleteDirective{
		const string _S_			=REP.SPACE_ExceptNewLine+"+";
		const string _s_			=REP.SPACE_ExceptNewLine+"*";
		const string LINE_S			="^"+_s_;
		const string LINE_E			=_s_+@"(?:\r\n?|\n)?$";

		const string DEL_START=LINE_S+@"\/\/\#>>delete"+LINE_E;
		const string DEL_END=LINE_S+@"\/\/\#<<delete"+LINE_E;

		static Rgx::Regex rx_delete_s=new Rgx::Regex(DEL_START,Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		static Rgx::Regex rx_delete_e=new Rgx::Regex(DEL_END,Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		public static void Resolve(ref string text,ReportError report){
			Rgx::MatchCollection mc=rx_delete_s.Matches(text);
			for(int i=mc.Count-1;i>=0;i--){
				Rgx::Match m_s=mc[i];
				Rgx::Match m_e=rx_delete_e.Match(text,m_s.Index);
				if(!m_e.Success){
					report("//#>>delete に対応する //#<<delete が存在しません。",m_s.Index);
					continue;
				}

				// 削除
				int ss=m_s.Index;
				//int se=ss+m_s.Length;
				int es=m_e.Index;
				int ee=es+m_e.Length;
				text=text.Substring(0,ss)+text.Substring(ee);
			}

		}
	}
}
