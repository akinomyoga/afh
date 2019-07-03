using Interop=System.Runtime.InteropServices;
using Gen=System.Collections.Generic;
namespace afh.File.ID3v2_3_{

	#region cls:Tag
	public class Tag:ID3v2_.ITag{
		public TagHeader header;
		public ExtendedHeader extheader=null;
		private Collections.DictionaryP<string,Frame> frames;

		//=================================================
		//		Properties
		//=================================================
		/// <summary>
		/// Tag �� ID3 �ɉ����� version ���擾���܂��B
		/// </summary>
		public string TagVersion{
			get{return "2.3.0";}
		}
		/// <summary>
		/// ExtendedHeader ���g�p���邩�ǂ������擾���͐ݒ肵�܂��B
		/// </summary>
		public bool HasExtendedHeader{
			get{return this.header.hasExtHeader;}
			set{
				this.header.hasExtHeader=value;
				if(value&&this.extheader==null)
					this.extheader=new ExtendedHeader();
			}
		}
		/// <summary>
		/// ID3v2.3 �ł͌��݂� false ��ݒ肷��l�ɂ��ĉ������B
		/// </summary>
		public bool IsExperimental{
			get{return this.header.experimental;}
			set{this.header.experimental=value;}
		}
		/// <summary>
		/// �t�@�C���ւ̕ۑ����� CRC32 �l���L�^���邩�ǂ������擾���͐ݒ肵�܂��B
		/// </summary>
		public bool HasCrc32{
			get{return this.HasExtendedHeader&&this.extheader.hasCrc32;}
			set{
				if(!this.HasExtendedHeader)this.HasExtendedHeader=true;
				this.extheader.hasCrc32=false;
			}
		}

		/// <summary>
		/// ���X�t�@�C������ǂݎ��ꂽ���ł��邩�ǂ������擾���܂��B
		/// </summary>
		public bool FromFile{
			get{return this.header.fromfile;}
		}
		/// <summary>
		/// �Ǎ����̃t�@�C���ɉ����� Tag �S�̂̑傫�����擾���܂��B
		/// �t�@�C������ǂݎ��ꂽ���ł͂Ȃ����ɂ͕��̒l��Ԃ��܂��B
		/// </summary>
		public int OriginalTagSize{
			get{return this.header.fromfile?this.header.size+10:-1;}
			// 10 �� sizeof(TagHeaderInfo)
		}
		/// <summary>
		/// �Ǎ����̃t�@�C�������擾���܂��B
		/// </summary>
		//public string OriginalFileName{
		//	get{return "";}
		//}

		public Collections.DictionaryP<string,Frame> Frames{
			get{return this.frames;}
		}
		//=================================================
		//		Constructor
		//=================================================
		/// <summary>
		/// �V���� Tag �𐶐����܂��B
		/// </summary>
		public Tag(){
			this.header=new TagHeader();
		}
		public Tag(byte[] image):this(image,0){}
		public Tag(byte[] image,int offset){
			this.header=new TagHeader(image,offset);
			BinarySubstr data=this.header.content;
#if DEBUG
			__dll__.log.Lock();
			__dll__.log.WriteLine("[ID3v2.3.0 Tag]");
			__dll__.log.AddIndent();
			__dll__.log.WriteVar("IsExperimentalTag",this.header.experimental.ToString());
			__dll__.log.WriteVar("Unsynchronization",this.header.unsynchronization.ToString());
			__dll__.log.WriteVar("HasExtendedHeader",this.header.hasExtHeader.ToString());
#endif
			if(this.header.hasExtHeader){
				this.extheader=new ExtendedHeader(data);
#if DEBUG
				__dll__.log.WriteVar("PaddingSize",this.extheader.paddingSize.ToString());
				__dll__.log.WriteVar("HasCRC32",this.extheader.hasCrc32.ToString());
#endif
				if(this.extheader.hasCrc32){
					uint crc32=data.CalculateCRC32();
					__dll__.log.WriteLine("    CRC32 Check: �� {0:X8} --><-- {1:X8} ��",crc32,this.extheader.crc32);
					if(crc32!=this.extheader.crc32)
						throw new System.ApplicationException("Crc �l����v���܂���B");
				}
			}
			afh.Collections.DictionaryP<string,FrameHeader> headers=FrameHeader.GetFrameHeaders(data);
			this.frames=headers.Map<Frame>(Frame.CreateFrame);
#if DEBUG
			__dll__.log.RemoveIndent();
			foreach(Gen::KeyValuePair<string,Frame> pair in this.frames){
				__dll__.log.WriteLine("Frame---");
				__dll__.log.AddIndent();
				__dll__.log.WriteVar("FrameID",pair.Key);
				__dll__.log.WriteVar("FrameType",pair.Value.GetType().FullName);
				//__dll__.log.WriteVar("Discards on TagChange",pair.Value.discardOnAlteringTag.ToString());
				//__dll__.log.WriteVar("Discards on FileChange",pair.Value.discardOnAlteringFile.ToString());
				//__dll__.log.WriteVar("ReadOnly",pair.Value.readOnly.ToString());
				//__dll__.log.WriteVar("Compressed",pair.Value.compression.ToString());
				//__dll__.log.WriteVar("Encrypted",pair.Value.encryption.ToString());
				//__dll__.log.WriteVar("Grouping",pair.Value.grouping.ToString());
				if(pair.Value is ID3v2_3_.TextInformationFrame)
					__dll__.log.WriteVar("TextContent",((ID3v2_3_.TextInformationFrame)pair.Value).Text);
				__dll__.log.RemoveIndent();
			}
			__dll__.log.Unlock();
#endif
		}
		//=================================================
		//		WriteFile
		//=================================================
		public void WriteToStream(System.IO.Stream str){
		}
	}
	#endregion

	#region cls:TagHeader
	public sealed class TagHeader{
		internal bool unsynchronization;
		internal bool hasExtHeader;
		internal bool experimental;
		
		internal bool fromfile;
		/// <summary>
		/// ���̃t�@�C���ɉ����� TagHeader �� 10B �������� Tag �̑傫���ł��B
		/// </summary>
		internal int size;

		public BinarySubstr content;

		/// <summary>
		/// �V���� TagHeader �𐶐����܂��B
		/// </summary>
		public TagHeader(){
			this.unsynchronization=false;
			this.hasExtHeader=false;
			this.experimental=false;
			this.content=null;

			this.fromfile=false;
		}
		public TagHeader(byte[] image):this(image,0){}
		public unsafe TagHeader(byte[] image,int offset){
			this.fromfile=true;

			fixed(byte* pImgB=&image[0]){
				TagHeaderInfo* p=(TagHeaderInfo*)(pImgB+offset);
				if(!p->IsID3v2)throw new System.ApplicationException("�L���� ID3v2.3.0 tag header �ł͂���܂���B");
				this.unsynchronization=p->Unsynchronization;
				this.hasExtHeader=p->HasExtendedHeader;
				this.experimental=p->IsExperimental;
				this.size=(int)p->size;

				this.content=new BinarySubstr(image,offset+(int)sizeof(TagHeaderInfo),(int)p->size);
			}
			if(this.unsynchronization){
				this.content=this.content.ResolveUnsync();
			}
		}
		public void Write(ID3v2_.TagWriter w){
			TagHeaderInfo h=new TagHeaderInfo();
			h.cc=(CharCode3)"ID3";
			h.verMinor=3;
			h.verBuild=0;
			h.flags=(HeaderFlags)0;
			if(this.experimental)h.flags|=HeaderFlags.Experimatal;
			if(this.hasExtHeader)h.flags|=HeaderFlags.ExtendedHeader;
			h.size=(UInt28BE)0;
			w.WriteStructure(h);
		}

		[Interop::StructLayout(Interop::LayoutKind.Sequential)]
		private struct TagHeaderInfo {
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
			public bool IsID3v2 {
				get { return (string)this.cc=="ID3"&&this.verMinor==3&&this.verBuild==0; }
			}
			public bool Unsynchronization{get{return (this.flags&HeaderFlags.Unsynchronization)!=0;}}
			public bool HasExtendedHeader{get{return (this.flags&HeaderFlags.ExtendedHeader)!=0;}}
			public bool IsExperimental{get{return (this.flags&HeaderFlags.Experimatal)!=0;}}
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
		private enum HeaderFlags:byte{
			/// <summary>
			/// ���e�ɔ񓯊����������s���Ă��鎖�������܂��B
			/// </summary>
			Unsynchronization=0x80,
			/// <summary>
			/// �g�� header �����݂��Ă��鎖�������܂��B
			/// </summary>
			ExtendedHeader=0x40,
			/// <summary>
			/// �����p�� tag �ł��鎖�������܂��B
			/// </summary>
			Experimatal=0x20
		}
	}
	#endregion

	#region cls:ExtendedHeader
	public sealed class ExtendedHeader{
		public uint length{get{return this.hasCrc32?10u:6u;}}
		public bool hasCrc32;
		public uint crc32;
		public uint paddingSize;

		public uint Size{get{return this.length+4;}}
	
		public ExtendedHeader(){
			this.hasCrc32=false;
			this.crc32=0;
			this.paddingSize=0;
		}
		/// <summary>
		/// ExtendedHeader �����������܂��B
		/// ExtendedHeader ��ǂݎ������A�ǂݎ��ʒu�͓ǂݎ������������ɐi�߂��܂��B
		/// </summary>
		/// <param name="data">ExtendedHeader �̏����w�肵�܂��B</param>
		public unsafe ExtendedHeader(BinarySubstr data){
			fixed(byte* fix=data.array){
				ExtendedHeaderInfo* xh=(ExtendedHeaderInfo*)data.CurrentPtr;
				this.hasCrc32=xh->HasCrc32;
				if(this.hasCrc32)this.crc32=(uint)xh->Crc32;
				this.paddingSize=(uint)xh->paddingSize;

				data.index+=(int)this.Size;
				data.end-=(int)this.paddingSize;
			}
		}
		[Interop::StructLayout(Interop::LayoutKind.Sequential)]
		private struct ExtendedHeaderInfo{
			public UInt32BE length;
			public ExtendedFlags flags;
			public UInt32BE paddingSize;
			public bool HasCrc32{get{return (this.flags&ExtendedFlags.CrcDataPresent)!=0;}}
			public unsafe UInt32BE Crc32{
				get{fixed(UInt32BE* p=&this.paddingSize)return *(p+1);}
			}
			/// <summary>
			/// ExtendedHeader �̎��̈ʒu�̃|�C���^���擾���܂��B
			/// ExtendedHeader �̑傫���� flags �ɐݒ肳�ꂽ�l�ɂ���ĕς��̂ł���ɉ����Č��ʂ��v�Z���܂��B
			/// </summary>
			public unsafe byte* Next{
				get{fixed(UInt32BE* p=&this.paddingSize)return (byte*)(p+(this.HasCrc32?2:1));}
			}
		}
		[System.Flags]
		private enum ExtendedFlags:ushort{
			CrcDataPresent=0x0080	// 0x80 0x00
		}
	}
	#endregion

	#region cls:FrameHeader
	public sealed class FrameHeader{
		public string frameId;

		public bool discardOnAlteringTag;
		public bool discardOnAlteringFile;
		public bool readOnly;
		public bool compression;
		public bool encryption;
		public bool grouping;

		public uint trueSize;
		public byte enryptionMethod;
		public byte groupId;

		public BinarySubstr content;

		/// <summary>
		/// <see cref="BinarySubstr"/> ���� FrameHeader ��ǂݎ��܂��B
		/// </summary>
		/// <param name="image">FrameHeader �̏������� BinarySubstr ���w�肵�܂��B</param>
		public unsafe FrameHeader(BinarySubstr image){
			fixed(byte* fix=image.array){
				byte* pB=image.CurrentPtr;
				FrameHeaderInfo* fh=(FrameHeaderInfo*)pB;
				this.frameId=(string)fh->frameID;

				this.discardOnAlteringTag=fh->DiscardsIfTagAlter;
				this.discardOnAlteringFile=fh->DiscardsIfFileAlter;
				this.readOnly=fh->IsReadOnly;
				this.compression=fh->IsCompressed;
				this.encryption=fh->IsEncrypted;
				this.grouping=fh->IsGrouped;

				if(this.compression)this.trueSize=fh->TrueSize;
				if(this.encryption)this.trueSize=fh->EncryptionMethod;
				if(this.grouping)this.groupId=fh->GroupId;

				image.index+=fh->HeaderSize;
				this.content=image.ReadRange((int)(uint)fh->size-fh->ExtraHeaderSize);
			}
			// this.content �̓��e���𓀁E����
		}

		/// <summary>
		/// �V���� FrameHeader ���쐬���܂��B
		/// </summary>
		/// <param name="frameId">�V���� FrameHeader �� FrameId ���w�肵�܂��B</param>
		internal FrameHeader(string frameId){
			this.frameId=frameId;

			this.discardOnAlteringTag=false;
			this.discardOnAlteringFile=Frame.FramesData[frameId].DiscardOnAlteringFile;
			this.readOnly=false;
			this.compression=false;
			this.encryption=false;
			this.grouping=false;

			//this.trueSize=0;
			//this.encryptionMethod=0;
			//this.groupId=0;

			this.content=null;
		}

		/// <summary>
		/// Frame ��񂪕��񂾃f�[�^������ BinarySubstr ���� FrameHeader �̏W����ǂݎ��܂��B
		/// </summary>
		/// <param name="data">Frame ��񂪕��񂾃f�[�^���Q�Ƃ��� BinarySubstr ���w�肵�܂��B</param>
		/// <returns>�ǂݎ���� FrameHeader �̏W����Ԃ��܂��B</returns>
		public static afh.Collections.DictionaryP<string,FrameHeader> GetFrameHeaders(BinarySubstr data){
			FrameHeader fheader;
			afh.Collections.DictionaryP<string,FrameHeader> dic=new afh.Collections.DictionaryP<string,FrameHeader>();
			while(null!=(fheader=ReadFrameHeader(data)))dic.Add(fheader.frameId,fheader);
			return dic;
		}
		private unsafe static FrameHeader ReadFrameHeader(BinarySubstr data){
			string frameID;
			fixed(byte* fix=data.array){
				byte* p=data.CurrentPtr;
				byte* pM=data.ptrEnd-11;
				while(*p==0)if(pM<=p++)return null;
				frameID=(string)*(CharCode4*)p;
			}
			return new FrameHeader(data);
		}
		[Interop::StructLayout(Interop::LayoutKind.Sequential)]
		private struct FrameHeaderInfo{
			public CharCode4 frameID;
			public UInt32BE size;
			public FrameHeaderFlags flags;
			private byte additionalData;
			//=============================================
			//		Properties
			//=============================================
			public bool DiscardsIfTagAlter{
				get{return (this.flags&FrameHeaderFlags.DiscardOnAlteringTag)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.DiscardOnAlteringTag;
					else this.flags&=~FrameHeaderFlags.DiscardOnAlteringTag;
				}
			}
			public bool DiscardsIfFileAlter{
				get{return (this.flags&FrameHeaderFlags.DiscardOnAlteringFile)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.DiscardOnAlteringFile;
					else this.flags&=~FrameHeaderFlags.DiscardOnAlteringFile;
				}
			}
			public bool IsReadOnly{
				get{return (this.flags&FrameHeaderFlags.ReadOnly)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.ReadOnly;
					else this.flags&=~FrameHeaderFlags.ReadOnly;
				}
			}
			public bool IsCompressed{
				get{return (this.flags&FrameHeaderFlags.Compression)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.Compression;
					else this.flags&=~FrameHeaderFlags.Compression;
				}
			}
			public bool IsEncrypted{
				get{return (this.flags&FrameHeaderFlags.Encryption)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.Encryption;
					else this.flags&=~FrameHeaderFlags.Encryption;
				}
			}
			public bool IsGrouped{
				get{return (this.flags&FrameHeaderFlags.Grouping)!=0;}
				set{
					if(value)this.flags|=FrameHeaderFlags.Grouping;
					else this.flags&=~FrameHeaderFlags.Grouping;
				}
			}

			/// <summary>
			/// ���k�O�̑傫�����擾���܂��B
			/// ���̃v���p�e�B�� flags �� Compression ���܂܂��ꍇ�ɂ����Ӗ��̂���l��Ԃ��܂��B
			/// ���̑��̏ꍇ�ɕԂ����l�͖���`�ł�
			/// </summary>
			public unsafe uint TrueSize{
				get{
					fixed(byte* pB=&this.additionalData)return (uint)*(UInt32BE*)pB;
				}
			}
			/// <summary>
			/// �Í��������w�肷��ԍ����擾���܂��B
			/// flags �� Encryption ���܂܂��ꍇ�ɂ����L���Ȓl��Ԃ��܂��B
			/// ���̏ꍇ�ɂ͂��̒l�͖���`�ł��B
			/// �Í������̔ԍ��� ENCR �t���[���ɓo�^����܂��B
			/// </summary>
			public unsafe byte EncryptionMethod{
				get{
					fixed(byte* pB=&this.additionalData){
						return IsCompressed?*(pB+sizeof(UInt32BE)):*pB;
					}
				}
			}
			/// <summary>
			/// Group ���ʎq���擾���܂��B
			/// </summary>
			public unsafe byte GroupId{
				get{
					fixed(byte* pB=&this.additionalData){
						byte* p=pB;
						if(IsCompressed)p+=sizeof(UInt32BE);
						if(IsEncrypted)p+=sizeof(byte);
						return *p;
					}
				}
			}
			/// <summary>
			/// ���� header �̑傫�����擾���܂��B
			/// </summary>
			public int HeaderSize{
				get{
					int r=10;
					if(this.IsCompressed)r+=4;
					if(this.IsEncrypted)r++;
					if(this.IsGrouped)r++;
					return r;
				}
			}
			/// <summary>
			/// ���� header �̖����ɒǉ��������̒������擾���܂��B
			/// </summary>
			public int ExtraHeaderSize{
				get{
					int r=0;
					if(this.IsCompressed)r+=4;
					if(this.IsEncrypted)r++;
					if(this.IsGrouped)r++;
					return r;
				}
			}
		}
		[System.Flags]
		public enum FrameHeaderFlags:ushort{
			DiscardOnAlteringTag	=0x0080, // 0x80 0x00
			DiscardOnAlteringFile	=0x0040, // 0x40 0x00
			ReadOnly				=0x0020, // 0x20 0x00
			Compression				=0x8000, // 0x00 0x80
			Encryption				=0x4000, // 0x00 0x40
			Grouping				=0x2000, // 0x00 0x20
		}
	}
	#endregion

	#region cls:Frame
	public class Frame{
		protected FrameHeader header;
		private BinarySubstr data;
		/// <summary>
		/// �t�@�C���ɕۑ�����Ă������X�̃f�[�^���擾���܂��B
		/// ����́AFrame �̓��e�ɕύX��������ꂽ��ł��t�@�C����ǂݍ��񂾎��̘Ԃŕω����܂���B
		/// </summary>
		protected BinarySubstr OriginalData{
			get{return this.data;}
		}
		public Frame(FrameHeader header){
			this.header=header;
			this.data=header.content;
		}

		/// <summary>
		/// �w�肵�� id �����V���� Frame ���쐬���܂��B
		/// </summary>
		/// <param name="frameId">�V���� Frame �� frameId ���w�肵�܂��B</param>
		/// <returns>�쐬���� Frame �̃C���X�^���X��Ԃ��܂��B</returns>
		public static Frame CreateFrame(string frameId){
			return CreateFrame(new FrameHeader(frameId));
		}
		/// <summary>
		/// �w�肵�� <see cref="FrameHeader"/> ���g�p���� Frame �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="fheader">Frame �Ɋւ������ێ����� FrameHeader ���w�肵�܂��B</param>
		/// <returns>Frame �̃C���X�^���X��Ԃ��܂��B</returns>
		public static Frame CreateFrame(FrameHeader fheader){
			string frameID=fheader.frameId;
			if(frameData.ContainsKey(frameID)){
				switch(frameData[frameID].FrameType){
					case "text":			return new TextInformationFrame(fheader);
					case "slash-list":		return new SlashNameListFrame(fheader);
					case "natural-integer": return new NumericalStringFrame(fheader,NumericalStringFrame.SignFlags.Natural);
					case "positive-integer":return new NumericalStringFrame(fheader,NumericalStringFrame.SignFlags.Positive);
					case "year":			return new YearFrame(fheader);
					case "position":		return new PositionFrame(fheader);
					case "UFID":return new UFIDFrame(fheader);
					case "TDAT":return new TDATFrame(fheader);
					case "TIME":return new TIMEFrame(fheader);
					case "TCON":return new TCONFrame(fheader);
				//	case "TCOP":
				//	case "TFLT":
				//	case "TKEY":
				//	case "TLAN":
				//	case "TMED":
				//	case "TSRC":
				}
			}
			if(frameID[0]=='T'){
				return new TextInformationFrame(fheader);
			}else return new Frame(fheader);
		}

		internal static Gen::Dictionary<string,FrameData> FramesData{
			get{return frameData;}
		}
		private static FrameDataDictionary frameData=new FrameDataDictionary("afh.File.ID3v2_3.FrameData.xml");
		/// <summary>
		/// Frame �Ɋւ������ێ����� XmlDocument ���Ǘ�����N���X�ł��B
		/// </summary>
		private class FrameDataDictionary:Gen::Dictionary<string,FrameData>{
			private System.Xml.XmlDocument doc=new System.Xml.XmlDocument();
			public FrameDataDictionary(string resName):base(){
				System.IO.Stream str
					=System.Reflection.Assembly.GetExecutingAssembly()
					.GetManifestResourceStream(resName);
				this.doc.Load(str);
				str.Close();
				foreach(System.Xml.XmlElement e in this.doc.DocumentElement.GetElementsByTagName("frame")){
					this.Add(e.GetAttribute("id"),new FrameData(e));
				}
			}
		}
		/// <summary>
		/// Frame �Ɋւ������ Xml ����擾���܂��B
		/// </summary>
		internal class FrameData{
			private System.Xml.XmlElement elem;
			public FrameData(System.Xml.XmlElement elem){
				this.elem=elem;
			}
			public string FrameType{
				get{
					string r=this.elem.GetAttribute("type");
					return r==""?"unknown":r;
				}
			}
			public bool DiscardOnAlteringFile{
				get{return this.elem.GetAttribute("discard-on-altering-file").ToLower()=="true";}
			}
			//=============================================
			//		����ʃf�[�^
			//=============================================
			private System.Xml.XmlElement En{
				get{return (System.Xml.XmlElement)elem.GetElementsByTagName("en")[0];}
			}
			private System.Xml.XmlElement Ja{
				get{return (System.Xml.XmlElement)elem.GetElementsByTagName("ja")[0];}
			}
			public string EngName{
				get{
					try{return En.GetAttribute("name");}
					catch{return "";}
				}
			}
			public string JpnName{
				get{
					try{return Ja.GetAttribute("name");}
					catch{return "";}
				}
			}
			public string EngDescription{
				get{
					try{return En.GetAttribute("desc");}
					catch{return "";}
				}
			}
			public string JpnDescription{
				get{
					try{return Ja.GetAttribute("desc");}
					catch{return "";}
				}
			}
		}
	}
	#endregion

	#region frame:UFID
	public class UFIDFrame:Frame{
		public BinarySubstr identifier;
		public UFIDFrame(FrameHeader fheader):base(fheader){
		}
		public void Read(System.Text.Encoding enc){
			this.OriginalData.Clear();
			this.OriginalData.ReadTextNullTerminated(enc);
			this.OriginalData.ReadRangeToEnd();
		}
	}
	#endregion
}