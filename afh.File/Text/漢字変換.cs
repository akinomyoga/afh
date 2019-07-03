using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Interop=System.Runtime.InteropServices;

namespace afh.Text{
	/// <summary>
	/// �����̕ϊ����s���ׂ̐ÓI�N���X�ł��B
	/// </summary>
	public static class �����ϊ�{
		private static Gen::Dictionary<char,char> kan_shin=new Gen::Dictionary<char,char>(0x9c4);

		static unsafe �����ϊ�(){
			const int BUFFSIZE=0x100;
			System.IO.Stream data=Ref::Assembly.GetAssembly(typeof(�����ϊ�)).GetManifestResourceStream("afh.Text.kantai_shinji.bin");
			byte[] buffer=new byte[BUFFSIZE];
			while(true){
				int read=data.Read(buffer,0,BUFFSIZE);
				if(read==0)break;
				fixed(byte* pBase=buffer) {
					char* pwch=(char*)pBase;
					char* pwchM=(char*)(pBase+read);
					while(pwch<pwchM)kan_shin.Add(*pwch++,*pwch++);
				}
			}
		}
		/// <summary>
		/// �ȑ̎��𕶕��Ȃ̒�߂�V���̂ɕϊ����܂��B
		/// </summary>
		/// <param name="ch">�ȑ̎��ł���\���̂��镶�����w�肵�܂��B</param>
		/// <returns>�w�肵���������ȑ̎��̏ꍇ�ɂ́A����ɑΉ�����V���̂�Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ͎w�肳�ꂽ�����𑴂̘ԕԂ��܂��B</returns>
		public static char �ȑ̎�to�V����(char ch) {
			return kan_shin.ContainsKey(ch)?kan_shin[ch]:ch;
		}
		/// <summary>
		/// �����񒆂̊ȑ̎���V���̂ɕϊ����ĕԂ��܂��B
		/// </summary>
		/// <param name="text">�ȑ̎����܂�ł���\���̂��镶������w�肵�܂��B</param>
		/// <returns>�w�肵��������Ɋȑ̎����܂܂�Ă����ꍇ�ɂ́A���̊ȑ̎���V���̂ɕϊ����������ɒu���������������Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ́A�w�肵��������Ɠ����e�̕������Ԃ��܂��B</returns>
		public static unsafe string �ȑ̎�to�V����(string text) {
			char* pOutB=(char*)Interop::Marshal.AllocHGlobal((text.Length+1)*sizeof(char));
			fixed(char* pInB=text){
				char* pIn=pInB;
				char* pOut=pOutB;
				char* pInM=pIn+text.Length;
				while(pIn<pInM) {
					char key=*pIn++;
					*pOut++=kan_shin.ContainsKey(key)?kan_shin[key]:key;
				}
				*pOut='\0';
			}
			string ret=new string(pOutB);
			Interop::Marshal.FreeHGlobal((System.IntPtr)pOutB);
			return ret;
		}
	}
}