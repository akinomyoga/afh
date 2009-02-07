namespace afh.File.ID3v2_{
	using Marshal=System.Runtime.InteropServices.Marshal;
	public interface ITag{
		string TagVersion{get;}
		bool HasExtendedHeader{get;set;}
		bool IsExperimental{get;set;}
		bool HasCrc32{get;set;}

		/// <summary>
		/// 読込元のファイルに於ける Tag 全体の大きさです。
		/// ファイルから読み取られた物ではない時には負の値を返します。
		/// </summary>
		int OriginalTagSize{get;}
	}

	public class TagWriter{
		System.IO.Stream str;
		public TagWriter(System.IO.Stream str){
			if(!str.CanWrite||!str.CanSeek)
				throw new System.ArgumentException("str","指定した System.IO.Stream はランダム書込に対応していません。");
			this.str=str;
		}

		/// <summary>
		/// 構造体を Stream に書き込みます。
		/// </summary>
		/// <param name="structure">書き込む構造体を指定します。</param>
		public void WriteStructure(System.ValueType structure){
			int len=Marshal.SizeOf(structure);
			this.WriteStructure(structure,len,len);
		}
		/// <summary>
		/// 構造体を Stream に書き込みます。
		/// </summary>
		/// <param name="structure">書き込む構造体を指定します。</param>
		/// <param name="length">
		/// 書き込む長さを指定します。構造体の初めから途中までの情報を書き込む事が出来ます。
		/// 構造体自体の長さよりも大きな値を指定した場合にはエラーになります。
		/// </param>
		public void WriteStructure(System.ValueType structure,int length){
			int len=Marshal.SizeOf(structure);
			this.WriteStructure(structure,len,length);
		}
		/// <summary>
		/// 構造体を Stream に書き込みます。動作確認済み。
		/// </summary>
		/// <param name="structure">書き込む構造体を指定します。</param>
		/// <param name="strlen">構造体の長さを指定します。</param>
		/// <param name="copylen">書き込む長さを指定します。</param>
		private void WriteStructure(System.ValueType structure,int strlen,int copylen){
			System.IntPtr buff=Marshal.AllocHGlobal(strlen);
			try{
				byte[] data=new byte[copylen];
				Marshal.StructureToPtr(structure,buff,false);
				Marshal.Copy(buff,data,0,strlen);
				str.Write(data,0,copylen);
			}finally{
				Marshal.FreeHGlobal(buff);
			}
		}
	}
}