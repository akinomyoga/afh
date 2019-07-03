namespace afh.File{
	/// <summary>
	/// Stream �ɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public static class StreamUtil{
		/// <summary>
		/// ���̓X�g���[������S�Ă̓��e��ǂݎ���āA�o�̓X�g���[���ւƏ������݂܂��B
		/// </summary>
		/// <param name="dstOStr">�o�͐�̃X�g���[�����w�肵�܂��BCanWrite ���^�ł���K�v������܂��B</param>
		/// <param name="srcIStr">�ǂݎ�茳�̃X�g���[�����w�肵�܂��BCanRead ���^�ł���K�v������܂��B</param>
		public static void PassAll(System.IO.Stream dstOStr,System.IO.Stream srcIStr){
			const int SZ_BUFF=0x1000;
			byte[] buff=new byte[SZ_BUFF];
			int nByte;
			do{
				nByte=srcIStr.Read(buff,0,SZ_BUFF);
				dstOStr.Write(buff,0,nByte);
			}while(nByte>0);
		}
	}
}