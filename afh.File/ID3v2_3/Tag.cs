using afh.File.ID3;

namespace afh.File.ID3v2_3{

	public class Tag{
		private bool experimental;
		private bool has_ext;
		private bool hascrc;

		public static Tag ReadFromStream(StreamAccessor accessor,out int tagsize){
			Tag tag=new Tag();

			bool unsync;
			int size;
			try{
				size=tag.ReadTagHeader(accessor,out unsync);
				tagsize=size+10;
			}catch(System.Exception e){
				__dll__.log.WriteError(e,"TagHeader �̓Ǎ��Ɏ��s���܂����B");
				tagsize=0;
				return null;
			}

			System.IO.Stream str=accessor.ReadSubStream((long)size);
			StreamAccessor acc_str;
			if(unsync){
				System.IO.Stream stream=ID3Utils.ID3ResolveUnsync(str);
				str.Close();
				acc_str=new StreamAccessor(stream);
			}else{
				acc_str=new StreamAccessor(str);
			}

			if(tag.has_ext){
				uint crc32;
				int padding;
				try{
					tag.ReadExtHeader(acc_str,out crc32,out padding);
				}catch(Exception exception2){
					exception=exception2;
					__dll__.log.WriteError(exception,"ExtendedHeader �̓Ǎ��Ɏ��s���܂����B");
					return null;
				}
				if(tag.hascrc){
					long crc_start=acc_str.Stream.Position;
					long crc_len=(acc_str.Stream.Length-crc_start)-padding;
					if(crc_len<0L){
						__dll__.log.WriteError(string.Format("Extended Header-Padding Size �̒l���s���ł��B�傫�����܂�:{0}",padding));
						return null;
					}
					if(crc32!=ID3Utils.CalculateCRC32(acc_str.Stream,crc_start,crc_len)){
						__dll__.log.WriteError("�t�@�C���� CRC32 �l����v���܂���ł����B");
						return null;
					}
				}
			}
			tag.frames=new afh.Collections.DictionaryP<string,Frame>();
			while(true){
				Frame val=acc_str.Read<Frame>(EncodingType.NoSpecified);
				if(val==Frame.EndOfFrames){
					acc_str.Stream.Close();
					return tag;
				}
				tag.frames.Add(val.FrameId,val);
			}
		}

		private int ReadTagHeader(StreamAccessor accessor,out bool unsync){
			const string DIFF_VERSION=@"�ǂݍ������Ƃ��Ă��� ID3 �^�O�̔ł� 2.{0}.{1}�ł��B 
���̊֐��ł� 2.3.0 �̓ǂݎ��ɂ����Ή����Ă��܂���B";
			//-------------------------------------------------------

			if(accessor.ReadString(EncodingType.CC3)!="ID3")
				throw new FileFormatException("�ǂݍ������Ƃ��Ă���o�C�i���X�g���[���� ID3 �^�O�ł͂���܂���B");
			
			byte minor		=accessor.ReadByte(EncodingType.U1);
			byte revision	=accessor.ReadByte(EncodingType.U1);
			if(minor!=3||revision!=0)
				throw new FileFormatException(string.Format(DIFF_VERSION,minor,revision));
			
			byte flags=accessor.ReadByte(EncodingType.U1);
			unsync				=(flags&0x80)!=0;
			this.has_ext		=(flags&0x40)!=0;
			this.experimental	=(flags&0x20)!=0;
			
			return accessor.ReadInt32(EncodingType.Int28BE);
		}

		private void ReadExtHeader(StreamAccessor accessor,out uint crc32,out int padding){
			uint num=accessor.ReadUInt32(EncodingType.U4BE);
			if((num != 6) && (num != 10)){
				throw new FileFormatException("Extended Header Size �� 6B ���� 10B �ō݂�ׂ��ł��B");
			}
			ushort num2=accessor.ReadUInt16(EncodingType.I2BE);
			this.hascrc=(num2 & 0x8000) != 0;
			if(num != (this.hascrc ? ((long)10) : ((long)6))){
				throw new FileFormatException(string.Format("Extended Header Size �̑傫�����s���ł��B\r\nCRC Check��{0}�ꍇ�ɂ�{1}B �ł���ׂ��ł��B",this.hascrc ? "�s��" : "�s��Ȃ�",this.hascrc ? "10" : "6"));
			}
			padding=accessor.ReadInt32(EncodingType.U4BE);
			crc32=this.hascrc ? accessor.ReadUInt32(EncodingType.U4BE) : 0;
		}

		private void WriteExtHeader(StreamAccessor accessor,out StreamAccessor str_crc32){
			accessor.Write(this.hascrc ? 10 : 6,EncodingType.U4BE);
			ushort num=this.hascrc ? ((ushort)0x8000) : ((ushort)0);
			accessor.Write(num,EncodingType.U2BE);
			accessor.Write((uint)0,EncodingType.U4BE);
			if(this.hascrc){
				str_crc32=new StreamAccessor(accessor.WriteSubStream(4L));
			}else{
				str_crc32=null;
			}
		}

		private void WriteTagHeader(StreamAccessor accessor,bool unsync,out long pos_size){
			accessor.Write("ID3",EncodingType.F4BE);
			accessor.Write((byte)3,EncodingType.U1);
			accessor.Write((byte)0,EncodingType.U1);

			byte num=0;
			if(unsync)num=(byte)(num|0x80);
			if(this.has_ext)num=(byte)(num|0x40);
			if(this.experimental)num=(byte)(num|0x20);

			accessor.Write(num,EncodingType.U1);
			pos_size=accessor.Position;
			accessor.Skip(4L);
		}

		public static void WriteToStream(Tag value,StreamAccessor accessor,out System.Action<long> SetSize){
			long pos_size;
			Stream stream=new MemoryStream();
			StreamAccessor accessor2=new StreamAccessor(stream);
			StreamAccessor accessor3=null;
			long start=0L;
			if(value.has_ext){
				value.WriteExtHeader(accessor2,out accessor3);
				start=stream.Position;
			}
			foreach(Frame frame in value.frames.Values)
				accessor2.WriteAs<Frame>(frame);
			if(value.has_ext){
				long length=stream.Position - start;
				accessor3.Write(ID3Utils.CalculateCRC32(stream,start,length),EncodingType.U4BE);
				accessor3.Stream.Close();
			}
			bool unsync=ID3Utils.ID3RequiredUnsync(stream);
			if(unsync){
				Stream stream2=ID3Utils.ID3Unsynchronize(stream);
				stream.Close();
				stream=stream2;
			}
			value.WriteTagHeader(accessor,unsync,out pos_size);
			SetSize=delegate(long size){
				if(size>StreamAccessor.MaxUInt28){
					throw new System.ArgumentOutOfRangeException("Tag �̏��ʂ��������ĕۑ��ł��܂���BTag �̓��e�� 256MB ��菬�����Ȃ�l�ɂ��ĉ�����");
				}
				accessor.PushPosition(pos_size);
				accessor.Write((uint)size,EncodingType.UInt28BE);
				accessor.PopPosition();
			};
			accessor.WriteStream(stream);
		}

		public bool HasCrc{
			get{return this.hascrc;}
			set{
				if(value)this.has_ext=true;
				this.hascrc=value;
			}
		}

		/// <summary>
		/// ExtendedHeader ���g�p���邩�ǂ������擾���͐ݒ肵�܂��B
		/// </summary>
		public bool HasExtendedHeader{
			get{return this.has_ext;}
			set{this.has_ext=value;}
		}

		public bool IsExperimetal{
			get{return this.experimental;}
		}

		//===========================================================
		//		Frames
		//===========================================================
		private afh.Collections.DictionaryP<string,Frame> frames;
		public void AddFrame(Frame frame){
			this.frames.Add(frame.FrameId,frame);
		}
		public Frame[] this[string frameId]{
			get{return this.frames[frameId];}
		}
	}

}