namespace afh.File.ID3v2_{
	using Marshal=System.Runtime.InteropServices.Marshal;
	public interface ITag{
		string TagVersion{get;}
		bool HasExtendedHeader{get;set;}
		bool IsExperimental{get;set;}
		bool HasCrc32{get;set;}

		/// <summary>
		/// �Ǎ����̃t�@�C���ɉ����� Tag �S�̂̑傫���ł��B
		/// �t�@�C������ǂݎ��ꂽ���ł͂Ȃ����ɂ͕��̒l��Ԃ��܂��B
		/// </summary>
		int OriginalTagSize{get;}
	}

	public class TagWriter{
		System.IO.Stream str;
		public TagWriter(System.IO.Stream str){
			if(!str.CanWrite||!str.CanSeek)
				throw new System.ArgumentException("str","�w�肵�� System.IO.Stream �̓����_�������ɑΉ����Ă��܂���B");
			this.str=str;
		}

		/// <summary>
		/// �\���̂� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="structure">�������ލ\���̂��w�肵�܂��B</param>
		public void WriteStructure(System.ValueType structure){
			int len=Marshal.SizeOf(structure);
			this.WriteStructure(structure,len,len);
		}
		/// <summary>
		/// �\���̂� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="structure">�������ލ\���̂��w�肵�܂��B</param>
		/// <param name="length">
		/// �������ޒ������w�肵�܂��B�\���̂̏��߂���r���܂ł̏����������ގ����o���܂��B
		/// �\���̎��̂̒��������傫�Ȓl���w�肵���ꍇ�ɂ̓G���[�ɂȂ�܂��B
		/// </param>
		public void WriteStructure(System.ValueType structure,int length){
			int len=Marshal.SizeOf(structure);
			this.WriteStructure(structure,len,length);
		}
		/// <summary>
		/// �\���̂� Stream �ɏ������݂܂��B����m�F�ς݁B
		/// </summary>
		/// <param name="structure">�������ލ\���̂��w�肵�܂��B</param>
		/// <param name="strlen">�\���̂̒������w�肵�܂��B</param>
		/// <param name="copylen">�������ޒ������w�肵�܂��B</param>
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