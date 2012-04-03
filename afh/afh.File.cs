namespace afh.File{
	/// <summary>
	/// Stream に対する操作を提供します。
	/// </summary>
	public static class StreamUtil{
		/// <summary>
		/// 入力ストリームから全ての内容を読み取って、出力ストリームへと書き込みます。
		/// </summary>
		/// <param name="dstOStr">出力先のストリームを指定します。CanWrite が真である必要があります。</param>
		/// <param name="srcIStr">読み取り元のストリームを指定します。CanRead が真である必要があります。</param>
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