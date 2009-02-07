using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Interop=System.Runtime.InteropServices;

namespace afh.Text{
	/// <summary>
	/// 漢字の変換を行う為の静的クラスです。
	/// </summary>
	public static class 漢字変換{
		private static Gen::Dictionary<char,char> kan_shin=new Gen::Dictionary<char,char>(0x9c4);

		static unsafe 漢字変換(){
			const int BUFFSIZE=0x100;
			System.IO.Stream data=Ref::Assembly.GetAssembly(typeof(漢字変換)).GetManifestResourceStream("afh.Text.kantai_shinji.bin");
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
		/// 簡体字を文部省の定める新字体に変換します。
		/// </summary>
		/// <param name="ch">簡体字である可能性のある文字を指定します。</param>
		/// <returns>指定した文字が簡体字の場合には、それに対応する新字体を返します。
		/// それ以外の場合には指定された文字を其の儘返します。</returns>
		public static char 簡体字to新字体(char ch) {
			return kan_shin.ContainsKey(ch)?kan_shin[ch]:ch;
		}
		/// <summary>
		/// 文字列中の簡体字を新字体に変換して返します。
		/// </summary>
		/// <param name="text">簡体字を含んでいる可能性のある文字列を指定します。</param>
		/// <returns>指定した文字列に簡体字が含まれていた場合には、その簡体字を新字体に変換した文字に置き換えた文字列を返します。
		/// それ以外の場合には、指定した文字列と同内容の文字列を返します。</returns>
		public static unsafe string 簡体字to新字体(string text) {
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