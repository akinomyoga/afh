using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;

using Ref=System.Reflection;
using Rgx=System.Text.RegularExpressions;
using Interop=System.Runtime.InteropServices;

namespace afh.Design{
	[Interop::ComVisible(true)]
	public abstract class TextFileGenerator:IVsSingleFileGenerator{
		/// <summary>
		/// 既定の出力ファイルの拡張子を返します。
		/// </summary>
		/// <param name="pbstrDefaultExtension">ここに拡張子を返します。</param>
		/// <returns>成功した場合に、<see cref="VSConstants.S_OK"/> を返します。</returns>
		int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension) {
			pbstrDefaultExtension=this.OutputExtension;
			return VSConstants.S_OK;
		}

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
		int IVsSingleFileGenerator.Generate(
			string wszInputFilePath,
			string bstrInputFileContents,
			string wszDefaultNamespace,
			IntPtr[] rgbOutputFileContents,
			out uint pcbOutput,
			IVsGeneratorProgress pGenerateProgress
		){
			string outstr=this.Translate(bstrInputFileContents,wszDefaultNamespace,pGenerateProgress);

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

		public abstract string OutputExtension{get;}
		public abstract string Translate(string inputText,string defaultNamespace,IVsGeneratorProgress generateProgress);

		private static Rgx::Regex rx_crlf=new Rgx::Regex("\r\n?|\n",Rgx::RegexOptions.Compiled);
		/// <summary>
		/// \r\n や \r や \n の混在した改行を、\r\n に統一します。
		/// </summary>
		/// <param name="input">様々な改行が混在している可能性のある文字列を指定します。</param>
		/// <returns>改行の種類を \r\n に統一した文字列を返します。</returns>
		protected string NormalizeCrlf(string input){
			return afh.Text.Regexs.Rx_NewLine.Replace(input,"\r\n");
		}
		/// <summary>
		/// 之によって生成されたファイルに付ける為の、之が自動生成ファイルである旨を説明するメッセージです。
		/// </summary>
		protected string Header{
			get{
				System.Type t=this.GetType();
				string module=Ref::Assembly.GetAssembly(t).ManifestModule.Name;
				return string.Format(HEADER,module,t.FullName);
			}
		}
		/// <summary>
		/// 之によって生成されたファイルに付ける為の、之が自動生成ファイルである旨を説明するメッセージです。
		/// アセンブリ名に対応する部分が "{0}" で記述され、クラス名に対応する部分が "{1}" で記述されています。
		/// string.Format を使用して適当な文字列に置換される様にして下さい。
		/// </summary>
		protected const string HEADER=@"/*
	このソースコードは [{0}] {1} によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[{0}] {1}'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
";

	}
}