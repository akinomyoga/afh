using Interop=System.Runtime.InteropServices;

namespace afh.File.ID3v2_2_{
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct TagHeader{
		//=================================================
		//		Fields
		//=================================================
		/// <summary>
		/// ID3 �łȂ���΂Ȃ�܂���B
		/// </summary>
		public CharCode3 cc;
		public byte verMinor;
		public byte verBuild;
		public HeaderFlags flags;
		public UInt28BE size;
		//=================================================
		//		Properties
		//=================================================
		/// <summary>
		/// ���߂̎O�o�C�g�ɂ���Ă��ꂪ������ ID3v2 �w�b�_�ł��邱�Ƃ��m�F���܂��B
		/// �������Ɣ��f�ł���ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B
		/// </summary>
		public bool IsID3v2{
			get{return (string)this.cc=="ID3";}
		}
		/// <summary>
		/// ���� ID3v2 �̃o�[�W�����𕶎���Ƃ��Ď擾���܂��B
		/// </summary>
		public string VersionString{get{return "2."+this.verMinor.ToString()+"."+this.verBuild.ToString();}}
		/// <summary>
		/// ���� ID3v2 �̃o�[�W������ System.Version �Ƃ��Ď擾���܂��B
		/// </summary>
		public System.Version Version{get{return new System.Version(2,(int)this.verMinor,(int)this.verBuild);}}
	}
	[System.Flags]
	public enum HeaderFlags:byte{
		Unsynchronisation=0x80,
		Compression=0x40
	}
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct FrameHeader{
		public CharCode3 frameID;
		public UInt24BE size;
	}
}