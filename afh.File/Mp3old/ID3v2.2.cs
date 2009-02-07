using Interop=System.Runtime.InteropServices;

namespace afh.File.ID3v2_2_{
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct TagHeader{
		//=================================================
		//		Fields
		//=================================================
		/// <summary>
		/// ID3 でなければなりません。
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
		/// 初めの三バイトによってこれが正しい ID3v2 ヘッダであることを確認します。
		/// 正しいと判断できる場合に true を返します。それ以外の場合には false を返します。
		/// </summary>
		public bool IsID3v2{
			get{return (string)this.cc=="ID3";}
		}
		/// <summary>
		/// この ID3v2 のバージョンを文字列として取得します。
		/// </summary>
		public string VersionString{get{return "2."+this.verMinor.ToString()+"."+this.verBuild.ToString();}}
		/// <summary>
		/// この ID3v2 のバージョンを System.Version として取得します。
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