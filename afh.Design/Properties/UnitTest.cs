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
#if DEBUG
	public class UnitTest{
		public static void test_stringLineColumn(afh.Application.Log log){
			const string sample=@"
今日は

作用なら

おほほほほほほほほ

";
			afh.Text.MultilineString mstr=new afh.Text.MultilineString(sample);
			log.DumpString(sample);
			log.WriteLine("3 行 1 列: {0}",mstr[3,1]);
		}


		public static void test_templateProc(afh.Application.Log log){
			const string input=@"
namespace afh{
	//#include ""mwg.Controls.WB.txt""
	public static partial class Math{
		//#define add hello(x)
		//#define square(x,y) ((x+y)*(y+x))
		//#define RAW(x)	x

		//#→template ADD_ARGS<abc,efg>
		{args[iArgs++]=double.Parse(abc);if(iArgs==NumberOfArgs)mode=efg;}
		//#←template
		//#→template ADD_ARGS<_word>
		{args[iArgs++]=double.Parse(_word);if(iArgs==NumberOfArgs)mode=SKIP_MODE;}
		//#←template
		//---------------------------------------------
		//#>>delete
		これはごみごみ!!!
		//#<<delete
		RAW(""#Hello<xxx>#"") end;
		RAW(abc) end;
		public static void Main(){
			//#ADD_ARGS<""0"">
			//#ADD_ARGS<word>
			//#ADD_ARGS<word>
			//#ADD_ARGS<word>
			//#ADD_ARGS<""0"">
			//#ADD_ARGS<SEKAI,OHAYOU!!>
			add;add;
			square(add,add);
			square(""#今日は ()()()((()))#"",789);
		}
	}

}			";
			log.WriteLine("入力:");
			log.DumpString(input);
			log.WriteLine();

			TemplateProcessor proc=new TemplateProcessor();
			GenError e=new GenError();
			string output=proc.Translate(input,"test",e);

			log.WriteLine("出力:");
			log.DumpString(output);
			foreach(GenError.Error err in e.errors)log.WriteLine(err);
		}

		public static void test_shoshiki(afh.Application.Log log){
			log.WriteLine(123.ToString("D4"));
			log.WriteLine(123.ToString("F4"));
		}

		public static void test_括弧の一致(afh.Application.Log log){
			Rgx::Regex rx=new System.Text.RegularExpressions.Regex(
				"^"+REP.CreateMatchParen('(',')',true)+"$",
				System.Text.RegularExpressions.RegexOptions.Compiled
				);
			log.WriteLine(rx.Match("fjsadfj(ssddd+dsada)dasdasd").Success);
			log.WriteLine(rx.Match("fjsadfj(ssddd+d(sada)dasdasd").Success);
			log.WriteLine(rx.Match("fjsadfj(ssddd+d(sada)das')')d\"((((((\"asd").Success);
		}

		public static void test_Preprocessor(afh.Application.Log log){
			string input=System.IO.File.ReadAllText(
				System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory,"test_preprocessor.cpp"),
				System.Text.Encoding.Default
				);
			log.WriteLine("入力:");
			//log.DumpString(input);
			log.WriteLine();

			string output=new afh.Preprocessor.Preprocessor().Preprocess(input);
			log.WriteLine("出力:");
			//log.DumpString(output);
		}
	}

	internal class GenError:VsInterop::IVsGeneratorProgress{
		public GenError(){}

		public Gen::List<Error> errors=new Gen::List<Error>();

		#region IVsGeneratorProgress メンバ
		public int GeneratorError(int fWarning,uint dwLevel,string bstrError,uint dwLine,uint dwColumn) {
			this.errors.Add(new Error(bstrError,dwLine,dwColumn));
			return 0;
		}

		public int Progress(uint nComplete,uint nTotal) {
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		public struct Error{
			public string msg;
			public uint line;
			public uint column;

			public Error(string msg,uint line,uint column){
				this.msg=msg;
				this.line=line;
				this.column=column;
			}

			public override string ToString() {
				return line.ToString()+"行 "+column.ToString()+"列: "+msg;
			}
		}
	}
#endif

}
internal static class _debug_{
	[System.Diagnostics.Conditional("DEBUG")]
	[System.Diagnostics.DebuggerHidden]
	public static void PreprocessorAssert(bool cond,params string[] msg){
		afh.DebugUtils.Assert(cond,msg);
	}
}
