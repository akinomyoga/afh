using Interop=System.Runtime.InteropServices;

namespace afh.File.Mp3_{
	/// <summary>
	/// mp3 �t�@�C����ǂݍ��ލۂɎg�p���܂��B
	/// </summary>
	public class MP3File{
		private string path;
		/// <summary>
		/// ���� image �̎��� mp3 �t�@�C���̏ꏊ���擾���܂��B
		/// </summary>
		public string FilePath{
			get{return this.path;}
		}
		internal byte[] image;
		private ID3v2_3_.Tag tag230;
		/// <summary>
		/// ID3v2.3.0 �� Tag ���擾���܂��B
		/// </summary>
		public ID3v2_3_.Tag Tag230{
			get{return this.tag230;}
		}
		/// <summary>
		/// �����̃t�@�C�������ɂ��� Mp3 �t�@�C�������������܂��B
		/// </summary>
		/// <param name="path">mp3 �t�@�C�����w�肵�ĉ������B</param>
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
					__dll__.log.WriteLine("ID3v2 tag �͂��̃t�@�C���ɂ͊܂܂�Ă��܂���B");
					break;
				case "2.3.0":
					tag230=new afh.File.ID3v2_3_.Tag(image);
					break;
				default:
					__dll__.log.WriteLine("���̃t�@�C���Ɋ܂܂�� tag �� version �� ID3v{0} �ł����A���� version �� tag �ɂ͑Ή����Ă��܂���B");
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
		/// ID3 �łȂ���΂Ȃ�܂���B
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
	public enum ID3v2Flags:byte{
		Unsynchronisation=0x80,
		ExtendedHeader=0x40,	// v2_2 �ł� ���k Compression
		Experimental=0x20,		// v2_3 ����
		Footer=0x10				// v2_4 ����
	}
}

