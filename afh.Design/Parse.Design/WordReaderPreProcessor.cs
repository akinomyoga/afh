using System;
using System.Collections.Generic;
using System.Text;
using Interop=System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;

namespace afh.Parse.Design {
	/// <summary>
	/// WordReader を実装する際に使用する独自の記述方法を解決します。
	/// </summary>
	[Interop::ComVisible(true)]
	[Interop::Guid("D237DE4C-2F29-4b8c-8AFF-165E6D3A7F3C")]
	[CodeGeneratorRegistration(typeof(WordReaderPreProcessor),"WordReaderPreProcessor",vsContextGuids.vsContextGuidVCSProject,GeneratesDesignTimeSource=true)]
	[ProvideObject(typeof(WordReaderPreProcessor))]
	public class WordReaderPreProcessor:IVsSingleFileGenerator{
		#region IVsSingleFileGenerator メンバ

		/// <summary>
		/// 既定の出力ファイルの拡張子を返します。
		/// </summary>
		/// <param name="pbstrDefaultExtension">ここに拡張子を返します。</param>
		/// <returns>成功した場合に、<see cref="VSConstants.S_OK"/> を返します。</returns>
		public int DefaultExtension(out string pbstrDefaultExtension) {
			pbstrDefaultExtension=".generated.cs";
			return VSConstants.S_OK;
		}

		const string HEADER=@"/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
";
		/// <summary>
		/// ファイルの内容を受け取り、出力ファイルの内容を生成します。
		/// </summary>
		/// <param name="wszInputFilePath">入力ファイルのパスを返します。</param>
		/// <param name="bstrInputFileContents">入力ファイルの内容を返します。</param>
		/// <param name="wszDefaultNamespace">既定の名前空間を指定します。</param>
		/// <param name="rgbOutputFileContents">出力ファイルの内容を返します。</param>
		/// <param name="pcbOutput">出力ファイルの長さを返します。</param>
		/// <param name="pGenerateProgress">コンパイル状況を受け取るのに使用します。</param>
		/// <returns>成功した場合に <see cref="VSConstants.S_OK"/> を返します。</returns>
		public int Generate(
			string wszInputFilePath,
			string bstrInputFileContents,
			string wszDefaultNamespace,
			IntPtr[] rgbOutputFileContents,
			out uint pcbOutput,
			IVsGeneratorProgress pGenerateProgress
		){
			string outstr=HEADER+regex.Replace(bstrInputFileContents,this.MatchEvaluator);

			// byte[] として取得
			System.IO.MemoryStream memstr=new System.IO.MemoryStream();
			System.IO.TextWriter writer=new System.IO.StreamWriter(memstr,System.Text.Encoding.UTF8);
			writer.Write(outstr);
			writer.Flush();
			int length=checked((int)memstr.Length);
			byte[] buff=memstr.GetBuffer();
			System.Console.Write("\r\n");
			writer.Close();
			memstr.Close();

			// unmanaged に転送
			IntPtr buff2=Interop::Marshal.AllocCoTaskMem(length);
			Interop::Marshal.Copy(buff,0,buff2,length);

			rgbOutputFileContents[0]=buff2;
			pcbOutput=(uint)length;
			return VSConstants.S_OK;
		}

		private string MatchEvaluator(System.Text.RegularExpressions.Match m){
			const string LETTER="this.lreader.CurrentLetter";
			const string TYPE="this.lreader.CurrentType";

			string arg;
			switch(this.GetMatchType(m)){
				case MatchType.Add:
					return "this.cword+="+LETTER+";";
				case MatchType.IfNotNext:
					return "if(!this.lreader.MoveNext())";
				case MatchType.Next:
					return "if(!this.lreader.MoveNext())return;";
				case MatchType.Nexit:
					return "{this.lreader.MoveNext();return;}";
				case MatchType.Is:
					arg=m.Groups[1+(int)MatchType.Is].Value;
					if(arg=="term")
						arg="("+LETTER+@"=='\r'||"+LETTER+@"=='\n'||"+LETTER+@"=='\u2028'||"+LETTER+@"=='\u2029')";
					else
						arg="("+LETTER+"=='"+arg+"')";
					return "not"==m.Groups[(int)MatchType.Is].Value ? "!"+arg : arg;
				case MatchType.IsRange:
					arg="('"+
						m.Groups[1+(int)MatchType.IsRange].Value+
						"'<="+LETTER+"&&"+LETTER+"<='"+
						m.Groups[2+(int)MatchType.IsRange].Value+
						"')";
					return "not"==m.Groups[(int)MatchType.IsRange].Value ? "!"+arg : arg;
				case MatchType.Error:
					arg=m.Groups[1+(int)MatchType.Error].Value;
					arg="this.lreader.SetError("+arg+",0,null);";
					if("errorexit"==m.Groups[(int)MatchType.Error].Value)
						return "{"+arg+"return;}";
					return arg;
				case MatchType.Type:
					return TYPE;
				case MatchType.Letter:
					return LETTER;
				case MatchType.CaseTerm:
					return @"case '\r':case '\n':case '\u2028':case '\u2029':";
				default:
					return m.Value;
			}
		}
		private static System.Text.RegularExpressions.Regex regex=new System.Text.RegularExpressions.Regex(
			@"""(is|not)\:(\\.|[^\\]|term)""|"+											// 1 2
			@"""(is|not)\:(\\.|[^\\])\-(\\.|[^\\])""|"+									// 3 4 5
			@"\b(error(?:exit)?)(\((?:(?:""(?:[^\\""\r\n]|\\.)*"")|[^\)""])+\))\;|"+	// 6 7
			@"\b(add;)|\b(if\(\!next\))|\b(next;)|\b(nexit;)|"+							// 8 9 10 11
			@"\b(type)\b|\b(letter)\b|\b(case term\:)",									// 12 13 14
			System.Text.RegularExpressions.RegexOptions.Compiled|System.Text.RegularExpressions.RegexOptions.Multiline);
		//=================================================
		//		MatchType
		//=================================================
		private enum MatchType{
			None=0,
			Is=1,
			IsRange=3,
			Add=8,
			IfNotNext=9,
			Next=10,
			Nexit=11,
			Type=12,
			Letter=13,
			CaseTerm=14,
			Error=6
		}
		private static MatchType[] types=new MatchType[]{
			MatchType.Add,MatchType.Next,MatchType.Nexit,
			MatchType.Is,MatchType.IsRange,
			MatchType.IfNotNext,MatchType.Error,
			MatchType.Type,MatchType.Letter,
			MatchType.CaseTerm
		};
		private MatchType GetMatchType(System.Text.RegularExpressions.Match m){
			for(int i=0;i<types.Length;i++)
				if(m.Groups[(int)types[i]].Value!="")return types[i];
			return MatchType.None;
		}

		#endregion
	}
}
