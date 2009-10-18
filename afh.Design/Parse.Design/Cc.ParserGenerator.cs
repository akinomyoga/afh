using afh.Parse.Design;
using Gen=System.Collections.Generic;
using TextRange=afh.Parse.LinearLetterReader.TextRange;
using AnalyzeError=afh.Parse.AnalyzeError;

using Interop=System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;

namespace afh.Parse.Cc{
	[Interop::ComVisible(true)]
	[Interop::Guid("E3D743BC-36D2-48aa-BC2E-02005DF9C3EB")]
	[CodeGeneratorRegistration(typeof(ContextParserGenerator),"ContextParserGenerator",vsContextGuids.vsContextGuidVCSProject,GeneratesDesignTimeSource=true)]
	[ProvideObject(typeof(ContextParserGenerator))]
	public sealed class ContextParserGenerator:afh.Design.TextFileGenerator,IVsSingleFileGenerator{
		public ContextParserGenerator():base(){}

		public override string OutputExtension {
			get {return ".cs";}
		}
		public override string Translate(string inputText,string defaultNamespace,Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress generateProgress) {
			ContextCompiler cc=new ContextCompiler();
			cc.DefaultNamespace=defaultNamespace;
			cc.Parse(inputText);
			string r=cc.ToCSharpSource();

			foreach(ContextCompiler.ErrorInfo err in cc.EnumErrors()){
				generateProgress.GeneratorError(0,0,err.message,(uint)err.line,(uint)err.column+1);
			}

			return r;
		}
	}
}