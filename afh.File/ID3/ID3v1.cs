using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;
using Ref=System.Reflection;

namespace afh.File.ID3v1_0{
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public unsafe struct tagID3v1_0{
		public fixed byte pstrId[3];
		public fixed byte pstrSong[30];
		public fixed byte pstrArtist[30];
		public fixed byte pstrAlbum[30];
		public fixed byte pstrYear[4];
		public fixed byte pstrComment[30];
		public byte nGenre;
	}
}
namespace afh.File.ID3v1_1{
	using Utils=afh.File.ID3.ID3Utils;

	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public unsafe struct tagID3v1_1{
		public const int ID_SIZE		=3;
		public const int SONG_SIZE		=30;
		public const int ARTIST_SIZE	=30;
		public const int ALBUM_SIZE		=30;
		public const int YEAR_SIZE		=4;
		public const int COMMENT_SIZE	=28;
		public const int SIZE			=0x80;
		public fixed byte pstrId		[ID_SIZE];
		public fixed byte pstrSong		[SONG_SIZE];
		public fixed byte pstrArtist	[ARTIST_SIZE];
		public fixed byte pstrAlbum		[ALBUM_SIZE];
		public fixed byte pstrYear		[YEAR_SIZE];
		public fixed byte pstrComment	[COMMENT_SIZE];
		public ushort nSongNumber;
		public byte nGenre;
	}

	public class Tag{
		public bool IsChanged{
			get{return this.changed;}
		}
		private bool changed;
		private tagID3v1_1 data;
		private System.Text.Encoding enc;

		private string song;
		private string artist;
		private string album;
		private string year;
		private string comment;
		private ushort songNum;
		private byte genre;

		public Tag(System.Text.Encoding enc){
			this.changed=false;
			this.changed=true;
			this.data=new tagID3v1_1();
			this.enc=enc;
			this.song="<����>";
			this.artist="";
			this.album="";
			this.year="2000";
			this.comment="";
			this.songNum=0;
			this.genre=0xff;
		}

		internal unsafe Tag(StreamAccessor accessor,System.Text.Encoding enc){
			this.changed=false;
			byte[] buffer=new byte[tagID3v1_1.SIZE];
			if(tagID3v1_1.SIZE!=accessor.Stream.Read(buffer,0,tagID3v1_1.SIZE)){
				throw new FileFormatException("Stream �̒����� ID3v1 �̃^�O���i�[�ł���傫���ɂȂ��Ă��܂���B");
			}
			fixed(byte* pData=buffer) {
				this.data=*(tagID3v1_1*)pData;
			}
			this.Decode(enc);
		}

		private unsafe void Decode(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data) {
				if("TAG"!=Utils.PtrToTerminatedString(pTag->pstrId,tagID3v1_1.ID_SIZE,System.Text.Encoding.ASCII)){
					throw new FileFormatException("ID3v1 �̃^�O�ł͂���܂���B");
				}
				this.song	=Utils.PtrToTerminatedString(pTag->pstrSong,	tagID3v1_1.SONG_SIZE,	enc);
				this.artist =Utils.PtrToTerminatedString(pTag->pstrArtist,	tagID3v1_1.ARTIST_SIZE,	enc);
				this.album	=Utils.PtrToTerminatedString(pTag->pstrAlbum,	tagID3v1_1.ALBUM_SIZE,	enc);
				this.year	=Utils.PtrToTerminatedString(pTag->pstrYear,	tagID3v1_1.YEAR_SIZE,	enc);
				this.comment=Utils.PtrToTerminatedString(pTag->pstrComment,	tagID3v1_1.COMMENT_SIZE,enc);
			}
			this.songNum=this.data.nSongNumber;
			this.genre=this.data.nGenre;
			this.enc=enc;
		}

		private unsafe void Encode(){
			if(this.changed){
				this.data=new tagID3v1_1();
				fixed(tagID3v1_1* pTag=&this.data){
					pTag->pstrId[0]=0x54;
					pTag->pstrId[1]=0x41;
					pTag->pstrId[2]=0x47;
					fixed(char* pwstr=this.song)
						this.enc.GetBytes(pwstr,this.song.Length,	pTag->pstrSong,		tagID3v1_1.SONG_SIZE);
					fixed(char* pwstr=this.artist)
						this.enc.GetBytes(pwstr,this.artist.Length,	pTag->pstrArtist,	tagID3v1_1.ARTIST_SIZE);
					fixed(char* pwstr=this.album)
						this.enc.GetBytes(pwstr,this.album.Length,	pTag->pstrAlbum,	tagID3v1_1.ALBUM_SIZE);
					fixed(char* pwstr=this.year)
						this.enc.GetBytes(pwstr,this.year.Length,	pTag->pstrYear,		tagID3v1_1.YEAR_SIZE);
					fixed(char* pwstr=this.comment)
						this.enc.GetBytes(pwstr,this.comment.Length,pTag->pstrComment,	tagID3v1_1.COMMENT_SIZE);
				}
				this.data.nSongNumber=this.songNum;
				this.data.nGenre=this.genre;
			}
		}
		/// <summary>
		/// �^�O�̓��e�𕶎���ɂ��ĕ\�����܂��B
		/// </summary>
		/// <returns>�^�O�̓��e�������������Ԃ��܂��B</returns>
		public override string ToString(){
			return string.Format("ID3v1.1 Tag\r\n�̋Ȗ�:\t{0}\r\n��Ȏ�:\t{1}\r\n�̏W��:\t{2}\r\n���s�N:\t{3}\r\n���ߓ�:\t{4}\r\n",new object[] { this.song,this.artist,this.album,this.year,this.comment });
		}

		public static Tag ReadFromStream(StreamAccessor accessor,System.Text.Encoding enc){
			return new Tag(accessor,enc);
		}

		public static unsafe void WriteToStream(Tag tag,StreamAccessor accessor){
			tag.Encode();
			byte[] buffer=new byte[0x80];
			fixed(byte* data=buffer){
				tagID3v1_1* pDst=(tagID3v1_1*)data;
				*pDst=tag.data;
			}
			accessor.Write(buffer);
		}
		//===========================================================
		//		�ȏ��ǂݎ��
		//===========================================================
		/// <summary>
		/// �w�肵�������R�[�h���g�p���ċȖ������̃f�[�^����ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public unsafe string ReadSong(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data)
				return Utils.PtrToTerminatedString(pTag->pstrSong,	tagID3v1_1.SONG_SIZE,enc);
		}
		/// <summary>
		/// �w�肵�������R�[�h���g�p���č�Ȏ҂̖��O�����̃f�[�^����ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public unsafe string ReadArtist(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data)
				return Utils.PtrToTerminatedString(pTag->pstrArtist,tagID3v1_1.ARTIST_SIZE,enc);
		}
		/// <summary>
		/// �w�肵�������R�[�h���g�p���ĉ̋ȏW�̖��O�����̃f�[�^����ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public unsafe string ReadAlbum(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data)
				return Utils.PtrToTerminatedString(pTag->pstrAlbum,	tagID3v1_1.ALBUM_SIZE,enc);
		}
		/// <summary>
		/// �w�肵�������R�[�h���g�p���Ĕ��s�N�����̃f�[�^����ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public unsafe string ReadYear(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data)
				return Utils.PtrToTerminatedString(pTag->pstrYear,	tagID3v1_1.YEAR_SIZE,enc);
		}
		/// <summary>
		/// �w�肵�������R�[�h���g�p���ċȂ̒��߂����̃f�[�^����ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public unsafe string ReadComment(System.Text.Encoding enc){
			fixed(tagID3v1_1* pTag=&this.data)
				return Utils.PtrToTerminatedString(pTag->pstrComment,tagID3v1_1.COMMENT_SIZE,enc);
		}
		//===========================================================
		//		�ȏ��
		//===========================================================
		/// <summary>
		/// �Ȗ����擾���͐ݒ肵�܂��B
		/// </summary>
		public string Song{
			get{return this.song;}
			set {
				if(this.song==value)return;
				this.song=value;
				this.changed=true;
			}
		}
		/// <summary>
		/// ��Ȏ҂̖��O���擾���͐ݒ肵�܂��B
		/// </summary>
		public string Artist{
			get{return this.artist;}
			set{
				if(this.artist==value)return;
				this.artist=value;
				this.changed=true;
			}
		}
		/// <summary>
		/// �Ȃ̔[�߂��Ă���̋ȏW�̖��O���擾���͐ݒ肵�܂��B
		/// </summary>
		public string Album{
			get{return this.album;}
			set {
				if(this.album==value)return;
				this.album=value;
				this.changed=true;
			}
		}
		/// <summary>
		/// �Ȃ̔��\�N (����) ���擾���͐ݒ肵�܂��B
		/// </summary>
		public string Year{
			get{return this.year;}
			set {
				if(this.year==value)return;
				this.year=value;
				this.changed=true;
			}
		}
		/// <summary>
		/// �Ȃ̒��߂��擾���͐ݒ肵�܂��B
		/// </summary>
		public string Comment{
			get{return this.comment;}
			set {
				if(this.comment==value)return;
				this.comment=value;
				this.changed=true;
			}
		}
	}
}
