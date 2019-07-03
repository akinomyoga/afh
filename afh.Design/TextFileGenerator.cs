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
		/// ����̏o�̓t�@�C���̊g���q��Ԃ��܂��B
		/// </summary>
		/// <param name="pbstrDefaultExtension">�����Ɋg���q��Ԃ��܂��B</param>
		/// <returns>���������ꍇ�ɁA<see cref="VSConstants.S_OK"/> ��Ԃ��܂��B</returns>
		int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension) {
			pbstrDefaultExtension=this.OutputExtension;
			return VSConstants.S_OK;
		}

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
		int IVsSingleFileGenerator.Generate(
			string wszInputFilePath,
			string bstrInputFileContents,
			string wszDefaultNamespace,
			IntPtr[] rgbOutputFileContents,
			out uint pcbOutput,
			IVsGeneratorProgress pGenerateProgress
		){
			string outstr=this.Translate(bstrInputFileContents,wszDefaultNamespace,pGenerateProgress);

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

		public abstract string OutputExtension{get;}
		public abstract string Translate(string inputText,string defaultNamespace,IVsGeneratorProgress generateProgress);

		private static Rgx::Regex rx_crlf=new Rgx::Regex("\r\n?|\n",Rgx::RegexOptions.Compiled);
		/// <summary>
		/// \r\n �� \r �� \n �̍��݂������s���A\r\n �ɓ��ꂵ�܂��B
		/// </summary>
		/// <param name="input">�l�X�ȉ��s�����݂��Ă���\���̂��镶������w�肵�܂��B</param>
		/// <returns>���s�̎�ނ� \r\n �ɓ��ꂵ���������Ԃ��܂��B</returns>
		protected string NormalizeCrlf(string input){
			return afh.Text.Regexs.Rx_NewLine.Replace(input,"\r\n");
		}
		/// <summary>
		/// �V�ɂ���Đ������ꂽ�t�@�C���ɕt����ׂ́A�V�����������t�@�C���ł���|��������郁�b�Z�[�W�ł��B
		/// </summary>
		protected string Header{
			get{
				System.Type t=this.GetType();
				string module=Ref::Assembly.GetAssembly(t).ManifestModule.Name;
				return string.Format(HEADER,module,t.FullName);
			}
		}
		/// <summary>
		/// �V�ɂ���Đ������ꂽ�t�@�C���ɕt����ׂ́A�V�����������t�@�C���ł���|��������郁�b�Z�[�W�ł��B
		/// �A�Z���u�����ɑΉ����镔���� "{0}" �ŋL�q����A�N���X���ɑΉ����镔���� "{1}" �ŋL�q����Ă��܂��B
		/// string.Format ���g�p���ēK���ȕ�����ɒu�������l�ɂ��ĉ������B
		/// </summary>
		protected const string HEADER=@"/*
	���̃\�[�X�R�[�h�� [{0}] {1} �ɂ���Ď����I�ɐ������ꂽ���ł��B
	���̃\�[�X�R�[�h��ύX���Ă��A���̃\�[�X�R�[�h�̌��ɂȂ����t�@�C����ύX���Ȃ��ƕύX�͓K�p����܂���B

	This source code was generated automatically by a file-generator, '[{0}] {1}'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
";

	}
}