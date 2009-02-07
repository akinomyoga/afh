using Interop=System.Runtime.InteropServices;

namespace afh.File.Mp3_{
	/// <summary>
	/// mp3 ファイルを読み込む際に使用します。
	/// </summary>
	public class MP3File{
		private string path;
		/// <summary>
		/// この image の持つ mp3 ファイルの場所を取得します。
		/// </summary>
		public string FilePath{
			get{return this.path;}
		}
		internal byte[] image;
		private ID3v2_3_.Tag tag230;
		/// <summary>
		/// ID3v2.3.0 の Tag を取得します。
		/// </summary>
		public ID3v2_3_.Tag Tag230{
			get{return this.tag230;}
		}
		/// <summary>
		/// 既存のファイルを元にして Mp3 ファイルを初期化します。
		/// </summary>
		/// <param name="path">mp3 ファイルを指定して下さい。</param>
		public unsafe MP3File(string path){
			this.path=path;
			this.image=System.IO.File.ReadAllBytes(path);
			string ver="";
			fixed(byte* pB=&this.image[0]){
				ID3v2Header* head=(ID3v2Header*)pB;
				if(head->IsID3v2)ver=head->VersionString;
			}
			switch(ver){
				case "":
					__dll__.log.WriteLine("ID3v2 tag はこのファイルには含まれていません。");
					break;
				case "2.3.0":
					tag230=new afh.File.ID3v2_3_.Tag(image);
					break;
				default:
					__dll__.log.WriteLine("このファイルに含まれる tag の version は ID3v{0} ですが、この version の tag には対応していません。");
					break;
			}
		}

	}
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct ID3v2Header{
		//=================================================
		//		Fields
		//=================================================
		/// <summary>
		/// ID3 でなければなりません。
		/// </summary>
		public CharCode3 cc;
		public byte verMinor;
		public byte verBuild;
		public ID3v2Flags flags;
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
	public enum ID3v2Flags:byte{
		Unsynchronisation=0x80,
		ExtendedHeader=0x40,	// v2_2 では 圧縮 Compression
		Experimental=0x20,		// v2_3 から
		Footer=0x10				// v2_4 から
	}
}

