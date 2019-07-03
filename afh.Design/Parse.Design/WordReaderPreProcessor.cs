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
	/// WordReader ����������ۂɎg�p����Ǝ��̋L�q���@���������܂��B
	/// </summary>
	[Interop::ComVisible(true)]
	[Interop::Guid("D237DE4C-2F29-4b8c-8AFF-165E6D3A7F3C")]
	[CodeGeneratorRegistration(typeof(WordReaderPreProcessor),"WordReaderPreProcessor",vsContextGuids.vsContextGuidVCSProject,GeneratesDesignTimeSource=true)]
	[ProvideObject(typeof(WordReaderPreProcessor))]
	public class WordReaderPreProcessor:IVsSingleFileGenerator{
		#region IVsSingleFileGenerator �����o

		/// <summary>
		/// ����̏o�̓t�@�C���̊g���q��Ԃ��܂��B
		/// </summary>
		/// <param name="pbstrDefaultExtension">�����Ɋg���q��Ԃ��܂��B</param>
		/// <returns>���������ꍇ�ɁA<see cref="VSConstants.S_OK"/> ��Ԃ��܂��B</returns>
		public int DefaultExtension(out string pbstrDefaultExtension) {
			pbstrDefaultExtension=".generated.cs";
			return VSConstants.S_OK;
		}

		const string HEADER=@"/*
	���̃\�[�X�R�[�h�� [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor �ɂ���Ď����I�ɐ������ꂽ���ł��B
	���̃\�[�X�R�[�h��ύX���Ă��A���̃\�[�X�R�[�h�̌��ɂȂ����t�@�C����ύX���Ȃ��ƕύX�͓K�p����܂���B

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
";
		/// <summary>
		/// �t�@�C���̓��e���󂯎��A�o�̓t�@�C���̓��e�𐶐����܂��B
		/// </summary>
		/// <param name="wszInputFilePath">���̓t�@�C���̃p�X��Ԃ��܂��B</param>
		/// <param name="bstrInputFileContents">���̓t�@�C���̓��e��Ԃ��܂��B</param>
		/// <param name="wszDefaultNamespace">����̖��O��Ԃ��w�肵�܂��B</param>
		/// <param name="rgbOutputFileContents">�o�̓t�@�C���̓��e��Ԃ��܂��B</param>
		/// <param name="pcbOutput">�o�̓t�@�C���̒�����Ԃ��܂��B</param>
		/// <param name="pGenerateProgress">�R���p�C���󋵂��󂯎��̂Ɏg�p���܂��B</param>
		/// <returns>���������ꍇ�� <see cref="VSConstants.S_OK"/> ��Ԃ��܂��B</returns>
		public int Generate(
			string wszInputFilePath,
			string bstrInputFileContents,
			string wszDefaultNamespace,
			IntPtr[] rgbOutputFileContents,
			out uint pcbOutput,
			IVsGeneratorProgress pGenerateProgress
		){
			string outstr=HEADER+regex.Replace(bstrInputFileContents,this.MatchEvaluator);

			// byte[] �Ƃ��Ď擾
			System.IO.MemoryStream memstr=new System.IO.MemoryStream();
			System.IO.TextWriter writer=new System.IO.StreamWriter(memstr,System.Text.Encoding.UTF8);
			writer.Write(outstr);
			writer.Flush();
			int length=checked((int)memstr.Length);
			byte[] buff=memstr.GetBuffer();
			System.Console.Write("\r\n");
			writer.Close();
			memstr.Close();

			// unmanaged �ɓ]��
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
