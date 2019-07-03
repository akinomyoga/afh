using Marshal=System.Runtime.InteropServices.Marshal;
using Gen=System.Collections.Generic;
namespace afh.File{
	using afh.File.Design;

	/// <summary>
	/// System.IO.Stream ��ʂ��ėl�X�ȏ����������񂾂�ǂݍ��񂾂肷����@��񋟂���N���X�ł��B
	/// </summary>
	public partial class StreamAccessor{
		/// <summary>
		/// �ǂݍ��񂾂菑�����񂾂肷��Ώۂ� <see cref="System.IO.Stream"/> ��ێ����܂��B
		/// </summary>
		protected System.IO.Stream stream;
		/// <summary>
		/// ���݂̓ǂݏ����Ώۂ� Stream ���擾���܂��B
		/// </summary>
		public System.IO.Stream Stream{
			get{return this.stream;}
		}
		/// <summary>
		/// �����ȃf�[�^��ǂݍ��񂾂菑�����񂾂肷��ۂɎg�p����o�b�t�@�ł��B
		/// </summary>
		protected byte[] buff=new byte[16];

		/// <summary>
		/// StreamAccessor �����������܂��B
		/// </summary>
		/// <param name="stream">�����������ސ�� <see cref="System.IO.Stream"/> ���w�肵�܂��B
		/// ���� Stream �́A�ǂݏ����̓r���Ƀ��b�v�����\��������܂��̂ŁA
		/// �ォ�� Stream �ɑ����������ꍇ�ɂ� <see cref="P:StreamAccessor.Stream"/> ��ʂ��čs���ĉ������B</param>
		public StreamAccessor(System.IO.Stream stream):base(){
			//if(!stream.CanRead||!stream.CanSeek||!stream.CanWrite)
			//	throw new System.ArgumentException("�ǂݏ����ړ����\�ȃX�g���[���ɂ����Ή����Ă��܂���","stream");
			this.stream=stream;
		}
		static StreamAccessor(){
			System.Array.Clear(zeroes,0,ZERO_BUFF_SIZE);
		}
		//===========================================================
		//		Stream �ʒu�Ǘ�
		//===========================================================
		/// <summary>
		/// Stream ���̌��݈ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public long Position{
			get{return this.stream.Position;}
			set{this.stream.Position=value;}
		}
		/// <summary>
		/// ���݂̈ʒu���L�^���āA�V�����ʒu�Ɉړ����܂��B
		/// </summary>
		/// <param name="position">�V�����ʒu���w�肵�܂��B</param>
		public void PushPosition(long position) {
			if(this.pos_stack==null){
				this.pos_stack=new Gen::Stack<long>();
			}
			this.pos_stack.Push(this.stream.Position);
			this.stream.Position=position;
		}
		/// <summary>
		/// PushPosition ���\�b�h�ňړ�����O�̈ʒu�ɖ߂�܂��B
		/// </summary>
		/// <returns>���̃��\�b�h�����s����O�ɋ����ʒu��Ԃ��܂��B</returns>
		public long PopPosition(){
			long position=this.stream.Position;
			this.stream.Position=this.pos_stack.Pop();
			return position;
		}
		/// <summary>
		/// �w�肵���������� Stream ��ǂݔ�΂��܂��B
		/// </summary>
		/// <param name="len">�ǂݔ�΂��������o�C�g�P�ʂŎw�肵�܂��B</param>
		public void Skip(long len) {
			this.stream.Seek(len,System.IO.SeekOrigin.Current);
		}
		private Gen::Stack<long> pos_stack;

		/// <summary>
		/// Stream �̎c��̒������擾���܂��B
		/// Position �y�� Length ���g�p�s�\�̏ꍇ�ɂ͗�O���������܂��B
		/// </summary>
		public long RestLength{
			get{return this.stream.Length-this.stream.Position;}
		}
		//===========================================================
		//		���n�^ Primitive Types
		//===========================================================
		/// <summary>
		/// Int28 �`���ŕ\���o����ŏ��̐��l��ێ����܂��B
		/// </summary>
		public const int MinInt28=unchecked((int)0xf8000000);
		/// <summary>
		/// Int28 �`���ŕ\���o����ő�̐��l��ێ����܂��B
		/// </summary>
		public const int MaxInt28=0x07ffffff;
		/// <summary>
		/// UInt28 �`���ŕ\���o����ő�̐��l��ێ����܂��B
		/// </summary>
		public const int MaxUInt28=0x0fffffff;
		/// <summary>
		/// Int24 �`���ŕ\���o����ŏ��̐��l��ێ����܂��B
		/// </summary>
		public const int MinInt24=unchecked((int)0xff800000);
		/// <summary>
		/// Int24 �`���ŕ\���o����ő�̐��l��ێ����܂��B
		/// </summary>
		public const int MaxInt24=0x007fffff;
		/// <summary>
		/// UInt24 �`���ŕ\���o����ő�̐��l��ێ����܂��B
		/// </summary>
		public const int MaxUInt24=0x00ffffff;
		
		#region Read:���n
		/// <summary>
		/// Stream �������ǂݍ��݂܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾏��� <see cref="bool"/> �ŕԂ��܂��B</returns>
		public unsafe bool ReadBoolean(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I1:
				case EncodingType.U1:
					return this.stream.ReadByte()!=0;
				case EncodingType.I4:
				case EncodingType.U4:
				case EncodingType.I4BE:
				case EncodingType.U4BE:
					read(4);
					fixed(byte* b=&this.buff[0])return *(int*)b!=0;
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Boolean �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="float"/> �ŕԂ��܂��B</returns>
		public unsafe float ReadFloat32(EncodingType enc) {
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.F4:
					read(4);
					fixed(byte* b=&this.buff[0])return *(float*)b;
				case EncodingType.F4BE:
					read(4);
					byte* rev=stackalloc byte[4];
					rev[0]=this.buff[3];
					rev[1]=this.buff[2];
					rev[2]=this.buff[1];
					rev[3]=this.buff[0];
					return *(float*)rev;
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Single �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="double"/> �ŕԂ��܂��B</returns>
		public unsafe double ReadFloat64(EncodingType enc) {
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.F8:
					read(8);
					fixed(byte* b=&this.buff[0])return *(double*)b;
				case EncodingType.F8BE:
					read(8);
					byte* rev=stackalloc byte[8];
					rev[0]=this.buff[7];
					rev[1]=this.buff[6];
					rev[2]=this.buff[5];
					rev[3]=this.buff[4];
					rev[4]=this.buff[3];
					rev[5]=this.buff[2];
					rev[6]=this.buff[1];
					rev[7]=this.buff[0];
					return *(double*)rev;
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Single �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		//=================================================
		//		�����̓Ǎ�
		//=================================================
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="sbyte"/> �ŕԂ��܂��B</returns>
		public sbyte ReadSByte(EncodingType enc){
			if(enc!=EncodingType.I1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("�w�肵����ނ��� System.SByte �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			read(1);
			return (sbyte)this.buff[0];
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="byte"/> �ŕԂ��܂��B</returns>
		public byte ReadByte(EncodingType enc){
			if(enc!=EncodingType.U1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("�w�肵����ނ��� System.Byte �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			read(1);
			return this.buff[0];
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="short"/> �ŕԂ��܂��B</returns>
		public unsafe short ReadInt16(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I2:
					read(2);
					return (short)(this.buff[0]|this.buff[1]<<8);
				case EncodingType.I2BE:
					read(2);
					return (short)(this.buff[0]<<8|this.buff[1]);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Int16 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="ushort"/> �ŕԂ��܂��B</returns>
		public unsafe ushort ReadUInt16(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U2:
					read(2);
					return (ushort)(this.buff[0]|this.buff[1]<<8);
				case EncodingType.U2BE:
					read(2);
					return (ushort)(this.buff[0]<<8|this.buff[1]);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Int16 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="int"/> �ŕԂ��܂��B</returns>
		public unsafe int ReadInt32(EncodingType enc){
			int value;
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I4:
					read(4);
					fixed(byte* b=&this.buff[0])return *(int*)b;
				case EncodingType.I4BE:
					read(4);
					return this.buff[0]<<24|this.buff[1]<<16|this.buff[2]<<8|this.buff[3];
				case EncodingType.I3:
					read(3);
					this.buff[3]=0;
					fixed(byte* b=&this.buff[0])value=*(int*)b;
					return value<<8>>8; // �����g��
				case EncodingType.I3BE:
					read(3);
					return (sbyte)this.buff[0]<<16|this.buff[1]<<8|this.buff[2];
				case EncodingType.Int28:
					read(4);
					value=0x7f&this.buff[0]|(0x7f&this.buff[1])<<7|(0x7f&this.buff[2])<<14|(0x7f&this.buff[3])<<21;
					return value<<4>>4; // �����g��
				case EncodingType.Int28BE:
					read(4);
					value=(0x7f&this.buff[0])<<21|(0x7f&this.buff[1])<<14|(0x7f&this.buff[2])<<7|0x7f&this.buff[3];
					return value<<4>>4; // �����g��
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Int32 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="uint"/> �ŕԂ��܂��B</returns>
		public unsafe uint ReadUInt32(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U4:
					read(4);
					fixed(byte* b=&this.buff[0])return *(uint*)b;
				case EncodingType.U4BE:
					read(4);
					return (uint)(this.buff[0]<<24|this.buff[1]<<16|this.buff[2]<<8|this.buff[3]);
				case EncodingType.U3:
					read(3);
					this.buff[3]=0;
					fixed(byte* b=&this.buff[0])return *(uint*)b;
				case EncodingType.U3BE:
					read(3);
					return (uint)(this.buff[0]<<16|this.buff[1]<<8|this.buff[2]);
				case EncodingType.UInt28:
					read(4);
					return (uint)(0x7f&this.buff[0]|(0x7f&this.buff[1])<<7|(0x7f&this.buff[2])<<14|(0x7f&this.buff[3])<<21);
				case EncodingType.UInt28BE:
					read(4);
					return (uint)((0x7f&this.buff[0])<<21|(0x7f&this.buff[1])<<14|(0x7f&this.buff[2])<<7|0x7f&this.buff[3]);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.UInt32 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="long"/> �ŕԂ��܂��B</returns>
		public unsafe long ReadInt64(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I8:
					read(8);
					fixed(byte* b=&this.buff[0])return *(long*)b;
				case EncodingType.I8BE:
					read(8);
					return (long)(this.buff[0]<<24|this.buff[1]<<16|this.buff[2]<<8|this.buff[3])<<32
						|(long)(uint)(this.buff[4]<<24|this.buff[5]<<16|this.buff[6]<<8|this.buff[7]);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.Int64 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������� <see cref="ulong"/> �ŕԂ��܂��B</returns>
		public unsafe ulong ReadUInt64(EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U8:
					read(8);
					fixed(byte* b=&this.buff[0])return *(ulong*)b;
				case EncodingType.U8BE:
					read(8);
					return (ulong)(this.buff[0]<<24|this.buff[1]<<16|this.buff[2]<<8|this.buff[3])<<32
						|(ulong)(uint)(this.buff[4]<<24|this.buff[5]<<16|this.buff[6]<<8|this.buff[7]);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.UInt64 �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		#endregion

		#region Write:���n
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="bool"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(bool value,EncodingType enc){
			switch(enc){
				case EncodingType.I1:
				case EncodingType.U1:
					this.stream.WriteByte((byte)(value?1:0));
					break;
				case EncodingType.I4:
				case EncodingType.U4:
					fixed(byte* b=&this.buff[0])*(int*)b=value?1:0;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.I4BE:
				case EncodingType.U4BE:
					fixed(byte* b=&this.buff[0])*(int*)b=value?0x01000000:0;
					this.stream.Write(this.buff,0,4);
					break;
				default:
					throw new System.ApplicationException("System.Boolean ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="float"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(float value,EncodingType enc) {
			switch(enc) {
				case EncodingType.NoSpecified:
				case EncodingType.F4:
					fixed(byte* b=&this.buff[0])*(float*)b=value;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.F4BE:
					byte* buf2=(byte*)&value;
					this.buff[0]=buf2[3];
					this.buff[1]=buf2[2];
					this.buff[2]=buf2[1];
					this.buff[3]=buf2[0];
					this.stream.Write(this.buff,0,4);
					break;
				default:
					throw new System.ApplicationException("System.Single ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="double"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(double value,EncodingType enc) {
			switch(enc) {
				case EncodingType.NoSpecified:
				case EncodingType.F8:
					fixed(byte* b=&this.buff[0])*(double*)b=value;
					this.stream.Write(this.buff,0,8);
					break;
				case EncodingType.F8BE:
					byte* buf2=(byte*)&value;
					this.buff[0]=buf2[7];
					this.buff[1]=buf2[6];
					this.buff[2]=buf2[5];
					this.buff[3]=buf2[4];
					this.buff[4]=buf2[3];
					this.buff[5]=buf2[2];
					this.buff[6]=buf2[1];
					this.buff[7]=buf2[0];
					this.stream.Write(this.buff,0,8);
					break;
				default:
					throw new System.ApplicationException("System.Double ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		//=================================================
		//		�����^�̏���
		//=================================================
		/// <summary>
		/// Stream �� <see cref="byte"/> ���������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ����w�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public void Write(byte value,EncodingType enc){
			if(enc!=EncodingType.U1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("System.Byte ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			this.stream.WriteByte(value);
		}
		/// <summary>
		/// Stream �� <see cref="sbyte"/> ���������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ����w�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public void Write(sbyte value,EncodingType enc){
			if(enc!=EncodingType.I1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("System.SByte ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			this.stream.WriteByte((byte)value);
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="short"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(short value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I2:
					fixed(byte* b=&this.buff[0])*(short*)b=value;
					this.stream.Write(this.buff,0,2);
					break;
				case EncodingType.I2BE:
					this.buff[0]=(byte)(value>>8);
					this.buff[1]=(byte)value;
					this.stream.Write(this.buff,0,2);
					break;
				default:
					throw new System.ApplicationException("System.Int16 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="ushort"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(ushort value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U2:
					fixed(byte* b=&this.buff[0])*(ushort*)b=value;
					this.stream.Write(this.buff,0,2);
					break;
				case EncodingType.U2BE:
					this.buff[0]=(byte)(value>>8);
					this.buff[1]=(byte)value;
					this.stream.Write(this.buff,0,2);
					break;
				default:
					throw new System.ApplicationException("System.UInt16 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="int"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(int value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I4:
					fixed(byte* b=&this.buff[0])*(int*)b=value;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.I4BE:
					this.buff[0]=(byte)(value>>24);
					this.buff[1]=(byte)(value>>16);
					this.buff[2]=(byte)(value>>8);
					this.buff[3]=(byte)value;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.I3:
					fixed(byte* b=&this.buff[0])*(int*)b=value;
					this.stream.Write(this.buff,0,3);
					break;
				case EncodingType.I3BE:
					this.buff[0]=(byte)(value>>16);
					this.buff[1]=(byte)(value>>8);
					this.buff[2]=(byte)value;
					this.stream.Write(this.buff,0,3);
					break;
				case EncodingType.Int28:
					this.buff[0]=(byte)(0x7f&value);
					this.buff[1]=(byte)(0x7f&value>>7);
					this.buff[2]=(byte)(0x7f&value>>14);
					this.buff[3]=(byte)(0x7f&value>>21);
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.Int28BE:
					this.buff[0]=(byte)(0x7f&value>>21);
					this.buff[1]=(byte)(0x7f&value>>14);
					this.buff[2]=(byte)(0x7f&value>>7);
					this.buff[3]=(byte)(0x7f&value);
					this.stream.Write(this.buff,0,4);
					break;
				default:
					throw new System.ApplicationException("System.Int32 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="uint"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(uint value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U4:
					fixed(byte* b=&this.buff[0])*(uint*)b=value;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.U4BE:
					this.buff[0]=(byte)(value>>24);
					this.buff[1]=(byte)(value>>16);
					this.buff[2]=(byte)(value>>8);
					this.buff[3]=(byte)value;
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.U3:
					fixed(byte* b=&this.buff[0])*(uint*)b=value;
					this.stream.Write(this.buff,0,3);
					break;
				case EncodingType.U3BE:
					this.buff[0]=(byte)(value>>16);
					this.buff[1]=(byte)(value>>8);
					this.buff[2]=(byte)value;
					this.stream.Write(this.buff,0,3);
					break;
				case EncodingType.UInt28:
					this.buff[0]=(byte)(0x7f&value);
					this.buff[1]=(byte)(0x7f&value>>7);
					this.buff[2]=(byte)(0x7f&value>>14);
					this.buff[3]=(byte)(0x7f&value>>21);
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.UInt28BE:
					this.buff[0]=(byte)(0x7f&value>>21);
					this.buff[1]=(byte)(0x7f&value>>14);
					this.buff[2]=(byte)(0x7f&value>>7);
					this.buff[3]=(byte)(0x7f&value);
					this.stream.Write(this.buff,0,4);
					break;
				default:
					throw new System.ApplicationException("System.UInt32 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="long"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(long value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.I8:
					fixed(byte* b=&this.buff[0])*(long*)b=value;
					this.stream.Write(this.buff,0,8);
					break;
				case EncodingType.I8BE:
					this.buff[0]=(byte)(value>>56);
					this.buff[1]=(byte)(value>>48);
					this.buff[2]=(byte)(value>>40);
					this.buff[3]=(byte)(value>>32);
					this.buff[4]=(byte)(value>>24);
					this.buff[5]=(byte)(value>>16);
					this.buff[6]=(byte)(value>>8);
					this.buff[7]=(byte)value;
					this.stream.Write(this.buff,0,8);
					break;
				default:
					throw new System.ApplicationException("System.Int64 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream �ɏ����������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ��� <see cref="ulong"/> �Ŏw�肵�܂��B</param>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		public unsafe void Write(ulong value,EncodingType enc){
			switch(enc){
				case EncodingType.NoSpecified:
				case EncodingType.U8:
					fixed(byte* b=&this.buff[0])*(ulong*)b=value;
					this.stream.Write(this.buff,0,8);
					break;
				case EncodingType.U8BE:
					this.buff[0]=(byte)(value>>56);
					this.buff[1]=(byte)(value>>48);
					this.buff[2]=(byte)(value>>40);
					this.buff[3]=(byte)(value>>32);
					this.buff[4]=(byte)(value>>24);
					this.buff[5]=(byte)(value>>16);
					this.buff[6]=(byte)(value>>8);
					this.buff[7]=(byte)value;
					this.stream.Write(this.buff,0,8);
					break;
				default:
					throw new System.ApplicationException("System.UInt64 ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		#endregion

		//===========================================================
		//		��
		//===========================================================
		private const int ZERO_BUFF_SIZE=0x100;
		private static byte[] zeroes=new byte[ZERO_BUFF_SIZE];
		/// <summary>
		/// �w�肵�����������[���N���A���s���܂��B
		/// �܂�A�w�肵���������� 0 ���������݂܂��B
		/// </summary>
		/// <param name="length">0 �Ŗ��ߐs�����������w�肵�܂��B</param>
		public void ZeroPadding(long length) {
			if(length<0L){
				throw new System.ArgumentOutOfRangeException();
			}
			while(length>ZERO_BUFF_SIZE) {
				this.stream.Write(zeroes,0,ZERO_BUFF_SIZE);
				length-=ZERO_BUFF_SIZE;
			}
			if(length>0L) {
				this.stream.Write(zeroes,0,(int)length);
			}
		}

		/// <summary>
		/// ���� Stream �����������ĕԂ��܂��B
		/// �쐬���� SubStream �̒����̕��������݈ʒu��i�߂܂��B
		/// </summary>
		/// <param name="length">�쐬���� SubStream �̒������w�肵�܂��B
		/// ���݂� Stream �̎c�蒷�����傫�Ȓl���w�肵���ꍇ�ɂ� SubStream �̒����͎c�蒷���ɂȂ�܂��B</param>
		/// <returns>�쐬���� SubStream ��Ԃ��܂��B</returns>
		public SubStream ReadSubStream(long length){
			if(length>this.RestLength)length=this.RestLength;
			SubStream r=new SubStream(ref this.stream,length);
			this.stream.Seek(length,System.IO.SeekOrigin.Current);
			return r;
		}
		/// <summary>
		/// �������ݗp�ɕ��� Stream �����������ĕԂ��܂��B
		/// �쐬���� SubStream �̒����̕��������݈ʒu��i�߂܂��B
		/// </summary>
		/// <param name="length">�쐬���� SubStream �̒������w�肵�܂��B</param>
		/// <returns>�쐬���� SubStream ��Ԃ��܂��B</returns>
		public SubStream WriteSubStream(long length){
			SubStream r=new SubStream(ref this.stream,length);
			this.stream.Seek(length,System.IO.SeekOrigin.Current);
			return r;
		}
		/// <summary>
		/// �w�肵�� Stream �����߂��珑�����݂܂��B
		/// </summary>
		/// <param name="stream">�������� Stream ���w�肵�܂��B</param>
		public void WriteStream(System.IO.Stream stream){
			const int BUFFSIZE=0x1000; // 4K

			stream.Position=0;

			// ���� loop (Substream �ւ̖{ Stream �̏�����) ��h���ׁA
			// ���߂ɏ������ޒ������Œ�
			long len=stream.Length;
			if(len==0)return;

			int read;
			byte[] buff=new byte[BUFFSIZE];
			while(len>BUFFSIZE){
				read=stream.Read(buff,0,BUFFSIZE);
				if(read==0)break;
				this.stream.Write(buff,0,read);
				len-=read;
			}
			if(len>0){
				read=stream.Read(buff,0,(int)len);
				if(read!=0)this.stream.Write(buff,0,read);
			}
#if OLD
			byte[] buff;
			// MaxValue �𒴉߂���ꍇ
			if(len>int.MaxValue){
				buff=new byte[int.MaxValue];
				do{
					stream.Read(buff,0,int.MaxValue);
					this.stream.Write(buff,0,int.MaxValue);
					len-=int.MaxValue;
				}while(len>int.MaxValue);

				if(len==0)return;
			}else{
				buff=new byte[len];
			}

			// ����
			stream.Read(buff,0,(int)len);
			this.stream.Write(buff,0,(int)len);
#endif
		}
		/// <summary>
		/// �o�C�i���z�� byte[] �� Strema ����ǂݎ��܂��B
		/// </summary>
		/// <param name="length">�ǂݎ��z��̑傫�����w�肵�܂��B</param>
		/// <returns>�ǂݎ�������e���i�[���Ă���z���Ԃ��܂��B</returns>
		public byte[] ReadBytes(int length){
			byte[] ret=new byte[length];
			stream.Read(ret,0,length);
			return ret;
		}
		/// <summary>
		/// �w�肵�� byte[] �� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="bytes">�������ޓ��e��ێ����Ă��� byte �z����w�肵�܂��B</param>
		public void WriteBytes(byte[] bytes){
			stream.Write(bytes,0,bytes.Length);
		}

		#region Read
		/// <summary>
		/// �w�肵�����������ǂݎ���� <see cref="buff"/> �ɏ������݂܂��B
		/// </summary>
		/// <param name="len">�ǂݎ�钷�����w�肵�܂��B
		/// buff �̑傫���� 16 �Ȃ̂ŁA������傫�Ȓl���w�肷�鎖�͏o���܂���B</param>
		/// <exception cref="System.ApplicationException">
		/// �w�肵�������������ǂݎ�鎖���o���Ȃ������ꍇ�ɔ������܂��B
		/// ���݈ʒu�͓ǂݎ������s����O�̈ʒu�ɖ߂�܂��B
		/// </exception>
		protected void read(int len){
			int read=this.stream.Read(this.buff,0,len);
			if(len!=read){
				this.stream.Seek(-read,System.IO.SeekOrigin.Current);
				throw new StreamOverRunException("�w�肵������ byte ��ǂݎ�鎖���o���܂���ł����B");
			}
		}
		/// <summary>
		/// �w�肵������������ǂ݂��� <see cref="M:buff"/> �ɏ������݂܂��B
		/// ���݈ʒu�͓ǂݎ������s����O�̈ʒu�ɖ߂�܂��B
		/// </summary>
		/// <param name="offset">���݈ʒu����ɂ����ǂݎ��̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="len">�ǂݎ�钷�����w�肵�܂��B
		/// buff �̑傫���� 16 �Ȃ̂ŁA������傫�Ȓl���w�肷�鎖�͏o���܂���B</param>
		/// <exception cref="System.ApplicationException">
		/// �w�肵�������������ǂݎ�鎖���o���Ȃ������ꍇ�ɔ������܂��B
		/// </exception>
		private void peek(int offset,int len){
			long pos=this.stream.Position;
			if(offset!=0)this.stream.Seek(offset,System.IO.SeekOrigin.Current);
			int read=this.stream.Read(this.buff,0,len);
			this.stream.Seek(pos,System.IO.SeekOrigin.Begin);
			if(len!=read){
				throw new StreamOverRunException("�w�肵������ byte ��ǂݎ�鎖���o���܂���ł����B");
			}
		}
		/// <summary>
		/// Stream ���� 1B �����ǂݎ���āA<see cref="M:buff"/>[0] �ɐݒ肵�܂��B
		/// </summary>
		/// <returns>�ǂݎ�鎖���o�����ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		private bool readbyte(){
			return this.stream.Read(this.buff,0,1)==1;
		}
		//=================================================
		//		ReadStruct: �C�ӂ̍\���̂̓Ǎ�
		//=================================================
		/// <summary>
		/// �w�肵���^�̍\���̂̓r���܂ł�ǂݍ��݂܂��B
		/// </summary>
		/// <typeparam name="T">�\���̂̌^���w�肵�܂��B</typeparam>
		/// <param name="readlength">�ǂݎ�钷�����w�肵�܂��B�\���̂̓r���܂ł����ǂݎ��Ȃ��ꍇ�ɂ��̒������g�p���܂��B</param>
		/// <returns>�ǂݎ�����\���̂�Ԃ��܂��B</returns>
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::ReadStruct+Example"]/*'/>
		public T ReadStruct<T>(int readlength) where T:struct{
			int strlen=Marshal.SizeOf(typeof(T));
			return (T)this.ReadStruct(typeof(T),strlen,readlength);
		}
		/// <summary>
		/// �w�肵���^�̍\���̂�ǂݎ��܂��B
		/// </summary>
		/// <typeparam name="T">�ǂݎ��\���̂̌^���w�肵�܂��B</typeparam>
		/// <returns>�ǂݎ�����\���̂�Ԃ��܂��B</returns>
		public T ReadStruct<T>() where T:struct{
			int strlen=Marshal.SizeOf(typeof(T));
			return (T)this.ReadStruct(typeof(T),strlen,strlen);
		}
		/// <summary>
		/// �w�肵���^�̍\���̂̓r���܂ł�ǂݍ��݂܂��B
		/// </summary>
		/// <param name="type">�\���̂̌^���w�肵�܂��B</param>
		/// <param name="readlength">�ǂݎ�钷�����w�肵�܂��B�\���̂̓r���܂ł����ǂݎ��Ȃ��ꍇ�ɂ��̒������g�p���܂��B</param>
		/// <returns>�ǂݎ�����\���̂�Ԃ��܂��B</returns>
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::ReadStruct+Example"]/*'/>
		public object ReadStruct(System.Type type,int readlength){
			if(!type.IsValueType)throw new System.ArgumentException("�w�肵���^�͍\���̂ł͂���܂���B","type");
			int strlen=Marshal.SizeOf(type);
			return this.ReadStruct(type,strlen,readlength);
		}
		/// <summary>
		/// �w�肵���^�̍\���̂�ǂݎ��܂��B
		/// </summary>
		/// <param name="type">�ǂݎ��\���̂̌^���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����\���̂�Ԃ��܂��B</returns>
		public object ReadStruct(System.Type type){
			if(!type.IsValueType)throw new System.ArgumentException("�w�肵���^�͍\���̂ł͂���܂���B","type");
			int strlen=Marshal.SizeOf(type);
			return this.ReadStruct(type,strlen,strlen);
		}
		private object ReadStruct(System.Type type,int strlen,int readlen){
			byte[] data=new byte[strlen];
			this.stream.Read(data,0,readlen);
			System.IntPtr buff=Marshal.AllocHGlobal(strlen);
			try{
				Marshal.Copy(data,0,buff,strlen);
				return Marshal.PtrToStructure(buff,type);
			}finally{
				Marshal.FreeHGlobal(buff);
			}
		}
		//=================================================
		//		Read: �C�ӂ̌^�̓Ǎ�
		//=================================================
		/// <summary>
		/// �w�肵���^�̏���ǂݎ��܂��B
		/// </summary>
		/// <typeparam name="T">�ǂݍ��ޏ��̌^���w�肵�܂��B</typeparam>
		/// <returns>�ǂݎ���������w�肵���^�ɂ��ĕԂ��܂��B</returns>
		public T Read<T>(){
			return (T)this.Read(typeof(T));
		}
		/// <summary>
		/// �w�肵���^�̏���ǂݎ��܂��B
		/// </summary>
		/// <param name="type">�ǂݍ��ޏ��̌^���w�肵�܂��B</param>
		/// <returns>�ǂݎ���������w�肵���^�ɂ��ĕԂ��܂��B</returns>
		public object Read(System.Type type){
			return this.Read(type,EncodingType.NoSpecified);
		}
		/// <summary>
		/// �w�肵���^�̏���ǂݎ��܂��B
		/// </summary>
		/// <typeparam name="T">�ǂݍ��ޏ��̌^���w�肵�܂��B</typeparam>
		/// <param name="enc">�ǂݍ��ޏ��� Stream �Ɋi�[����̂Ɏg���Ă���`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ���������w�肵���^�ɂ��ĕԂ��܂��B</returns>
		public T Read<T>(EncodingType enc){
			return (T)this.Read(typeof(T),enc);
		}
		/// <summary>
		/// �w�肵���^�̏���ǂݎ��܂��B
		/// </summary>
		/// <param name="type">�ǂݍ��ޏ��̌^���w�肵�܂��B</param>
		/// <param name="enc">�ǂݍ��ޏ��� Stream �Ɋi�[����̂Ɏg���Ă���`�����w�肵�܂��B</param>
		/// <returns>�ǂݎ���������w�肵���^�ɂ��ĕԂ��܂��B</returns>
		public object Read(System.Type type,EncodingType enc){
			if(type.IsEnum)type=System.Enum.GetUnderlyingType(type);

			/* ��{�^ */
			if(type.IsPrimitive)switch(type.Name[0]){
				case 'B':
					if(type==typeof(byte))
						return this.ReadByte(enc);
					if(type==typeof(bool))
						return this.ReadBoolean(enc);
					break;
				case 'S':
					if(type==typeof(sbyte))
						return this.ReadSByte(enc);
					break;
				case 'I':
					if(type==typeof(int))
						return this.ReadInt32(enc);
					if(type==typeof(short))
						return this.ReadInt16(enc);
					if(type==typeof(long))
						return this.ReadInt64(enc);
					break;
				case 'U':
					if(type==typeof(uint))
						return this.ReadUInt32(enc);
					if(type==typeof(ushort))
						return this.ReadUInt16(enc);
					if(type==typeof(ulong))
						return this.ReadUInt64(enc);
					break;
			}else if(type==typeof(string)){
				return this.ReadString(enc);
			}

			/* Registered ICustomReader */
			ICustomReader reader=GetCustomReader(type);
			if(reader!=null)return reader.Read(this);

			/* �\���� */
			if(type.IsValueType)switch(type.Name[0]){
				case 'D':
					if(type==typeof(System.DateTime)){
						ulong value=this.ReadUInt64(EncodingType.U8);
						unsafe{return *(System.DateTime*)&value;}
					}
					goto default;
				default:
					return this.ReadStruct(type);
			}
			throw new System.ApplicationException("�w�肵���^�̏����ɂ͑Ή����Ă��܂���B");
		}
		#endregion

		#region Write
		/// <summary>
		/// �w�肵�� <see cref="byte"/> �z��� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="value">�������� byte �z����w�肵�܂��B</param>
		public void Write(byte[] value){
			if(value==null||value.Length==0)return;
			this.stream.Write(value,0,value.Length);
		}
		//=================================================
		//		WriteStruct: �C�ӂ̍\���̂̏���
		//=================================================
		/// <summary>
		/// �\���̂� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="structure">�������ލ\���̂��w�肵�܂��B</param>
		public void WriteStruct(System.ValueType structure){
			byte[] buff=struct_to_bytes(structure);
			this.stream.Write(buff,0,buff.Length);
		}
		/// <summary>
		/// �\���̂� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="structure">�������ލ\���̂��w�肵�܂��B</param>
		/// <param name="length">
		/// �������ޒ������w�肵�܂��B�\���̂̏��߂���r���܂ł̏����������ގ����o���܂��B
		/// �\���̎��̂̒��������傫�Ȓl���w�肵���ꍇ�ɂ̓G���[�ɂȂ�܂��B
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// �w�肵���������ޒ����͍\���̂̑傫���𒴂��Ă��܂��B
		/// </exception>
		public void WriteStruct(System.ValueType structure,int length){
			byte[] buff=struct_to_bytes(structure);
			this.stream.Write(buff,0,length);
		}
		/// <summary>
		/// �\���̂� byte �z��ɕϊ����܂��B����m�F�ς݁B
		/// </summary>
		/// <param name="structure">�ϊ����̍\���̂��w�肵�܂��B</param>
		private byte[] struct_to_bytes(System.ValueType structure){
			return this.struct_to_bytes(structure,Marshal.SizeOf(structure));
		}
		/// <summary>
		/// �\���̂� byte �z��ɕϊ����܂��B����m�F�ς݁B
		/// </summary>
		/// <param name="structure">�ϊ����̍\���̂��w�肵�܂��B</param>
		/// <param name="strlen">�\���̂̑傫�����w�肵�܂��B�������傫�����w�肵�ĉ������B</param>
		private byte[] struct_to_bytes(System.ValueType structure,int strlen){
			System.IntPtr buff=Marshal.AllocHGlobal(strlen);
			try{
				byte[] data=new byte[strlen];
				Marshal.StructureToPtr(structure,buff,false);
				Marshal.Copy(buff,data,0,strlen);
				return data;
			}finally{
				Marshal.FreeHGlobal(buff);
			}
		}
		//=================================================
		//		Write: �C�ӂ̌^�̏���
		//=================================================
		/// <summary>
		/// �w�肵�������w�肵���^�Ƃ��ď������݂܂��B
		/// </summary>
		/// <typeparam name="T">�������ރf�[�^�̌^���w�肵�܂��B</typeparam>
		/// <param name="value">�������ރf�[�^���w�肵�܂��B</param>
		public void WriteAs<T>(T value)					{this.Write(value,typeof(T),EncodingType.NoSpecified);}
		/// <summary>
		/// �w�肵�������w�肵���^�Ƃ��ď������݂܂��B
		/// </summary>
		/// <typeparam name="T">�������ރf�[�^�̌^���w�肵�܂��B</typeparam>
		/// <param name="value">�������ރf�[�^���w�肵�܂��B</param>
		/// <param name="enc">�f�[�^���������ނ̂Ɏg�p����`�����w�肵�܂��B</param>
		public void WriteAs<T>(T value,EncodingType enc){this.Write(value,typeof(T),enc);}
		/// <summary>
		/// �w�肵���f�[�^���A���̌^�ɍ��������@�ŏ������݂܂��B
		/// </summary>
		/// <param name="value">�������ރf�[�^���w�肵�܂��B</param>
		/// <param name="enc">�f�[�^���������ނ̂Ɏg�p����`�����w�肵�܂��B</param>
		public void Write(object value,EncodingType enc){this.Write(value,value.GetType(),enc);}
		/// <summary>
		/// �w�肵���f�[�^���A���̌^�ɍ��������@�ŏ������݂܂��B
		/// </summary>
		/// <param name="value">�������ރf�[�^���w�肵�܂��B</param>
		public void Write(object value)					{this.Write(value,value.GetType(),EncodingType.NoSpecified);}
		private void Write(object value,System.Type type,EncodingType enc){
			if(type.IsEnum)type=System.Enum.GetUnderlyingType(type);

			/* ��{�^ */
			if(type.IsPrimitive)switch(type.Name[0]){
				case 'B':
					if(type==typeof(byte)){
						this.Write((byte)value,enc);
						return;
					}else if(type==typeof(bool)){
						this.Write((bool)value,enc);
						return;
					}else break;
				case 'S':
					if(type==typeof(sbyte)){
						this.Write((sbyte)value,enc);
						return;
					}else break;
				case 'I':
					if(type==typeof(int)){
						this.Write((int)value,enc);
						return;
					}else if(type==typeof(short)){
						this.Write((short)value,enc);
						return;
					}else if(type==typeof(long)){
						this.Write((long)value,enc);
						return;
					}else break;
				case 'U':
					if(type==typeof(uint)){
						this.Write((uint)value,enc);
						return;
					}else if(type==typeof(ushort)){
						this.Write((ushort)value,enc);
						return;
					}else if(type==typeof(ulong)){
						this.Write((ulong)value,enc);
						return;
					}else break;
			}else if(type==typeof(string)){
				this.Write((string)value,enc);
				return;
			}else if(typeof(System.IO.Stream).IsAssignableFrom(type)){
				this.WriteStream((System.IO.Stream)value);
				return;
			}

			/* Registered CustomWriter */
			ICustomWriter writer=GetCustomWriter(type);
			if(writer!=null){
				writer.Write(value,this);
				return;
			}

			/* �\���� */
			if(type.IsValueType)switch(type.Name[0]){
				case 'D':
					if(type==typeof(System.DateTime)){
						System.DateTime dt=(System.DateTime)value;
						unsafe{this.Write(*(long*)&dt);}
						return;
					}
					goto default;
				default:
					try{
						this.WriteStruct((System.ValueType)value);
						return;
					}catch(System.Exception e){
						afh.File.__dll__.log.WriteError(e,type.FullName+" �\���̂̏����Ɏ��s���܂����B");
						break;
					}
			}
			throw new System.ApplicationException("�w�肵���I�u�W�F�N�g�͏������ގ����o���܂���B�Ή����Ă��Ȃ���ނ̌^�ł��B");			
		}
		#endregion

		#region Read:������
		//=================================================
		//		������̓Ǎ�
		//=================================================
		/// <summary>
		/// �w�肵�������R�[�h�ɑΉ����� Preamble (BOM ��) ���������ꍇ�ɁA
		/// ���̕��������݈ʒu��i�߂܂��B
		/// </summary>
		/// <param name="encoding">�ǂݎ��Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		private int skip_preamble(System.Text.Encoding encoding){
			byte[] preamble=encoding.GetPreamble();
			if(preamble.Length==0)return 0;

			byte[] buffer=new byte[preamble.Length];
			int read=this.stream.Read(buffer,0,preamble.Length);
			if(read!=preamble.Length)goto clear;

			for(int i=0;i<preamble.Length;i++){
				if(preamble[i]!=buffer[i])goto clear;
			}
			return preamble.Length;
		clear:
			this.stream.Seek(-read,System.IO.SeekOrigin.Current);
			return 0;
		}
		private static System.Text.Encoding encoding_ansi;
		private static System.Text.Encoding AnsiEncoding{
			get{
				if(encoding_ansi==null){
					encoding_ansi=System.Text.Encoding.Default;

					// ASCII ����{�ɂ������łȂ���� ASCII �ɒu������
					byte[] b=encoding_ansi.GetBytes("aA1");
					if(b.Length!=3||b[0]!=0x61||b[1]!=0x41||b[2]!=0x31)
						encoding_ansi=System.Text.Encoding.ASCII;
				}
				return encoding_ansi;
			}
		}
		/// <summary>
		/// �w�肵�� EncodingType ����ǂݎ��p System.Text.Encoding �C���X�^���X���擾���܂��B
		/// <para>���̃��\�b�h�̎��s�̍ۂ� BOM ��ǂݎ�鎖������܂����AStream �̌��݈ʒu�͕ύX���܂���B</para>
		/// </summary>
		/// <param name="enc">�����R�[�h���͕����R�[�h�I����@���w�肵�܂��B</param>
		/// <param name="terminated">
		/// ���� null �I�[�������ǂݎ�낤�Ƃ��Ă���ꍇ�� true ���w�肵�܂��B
		/// true ���w�肷��� EncodingType.EncWide �ɑ΂��� utf-32 �y�� utf-32BE �� BOM �̊m�F�͍s�킸 utf-16 �Ƃ��Ĉ����܂��B
		/// </param>
		/// <param name="bomoffset">
		/// BOM ���L�q����Ă���ƍl������ꍇ�ɁABOM �����݈ʒu���ǂꂾ����ɋL�q����Ă��邩���w�肵�܂��B
		/// ���݈ʒu���� BOM ���n�܂�ꍇ�ɂ� 0 ���w�肵�܂��B
		/// </param>
		/// <returns>�擾���� Encoding ��Ԃ��܂��B</returns>
		private System.Text.Encoding detect_encoding(EncodingType enc,bool terminated,int bomoffset){
			switch(enc&EncodingType.MASK_STRING_ENCSPEC){
				case EncodingType.EncAnsi:
					return AnsiEncoding;
				case EncodingType.EncWide:
					try{peek(bomoffset,4);}catch{
						return System.Text.Encoding.Unicode;
					}
					if(this.buff[0]==0xff&&this.buff[1]==0xfe){
						//BOM: FF FE
						if(!terminated&&this.buff[2]==0&&this.buff[3]==0)return System.Text.Encoding.UTF32;
						return System.Text.Encoding.Unicode;
					}else if(this.buff[0]==0xfe&&this.buff[1]==0xff){
						//BOM: FE FF
						return System.Text.Encoding.BigEndianUnicode;
					}else if(this.buff[0]==0xef&&this.buff[1]==0xbb&&this.buff[2]==0xbf){
						//BOM: EF BB BF
						return System.Text.Encoding.UTF8;
					}else if(!terminated&&this.buff[0]==0&&this.buff[1]==0&&this.buff[2]==0xfe&&this.buff[3]==0xff){
						//BOM: 00 00 FE FF
						enc=EncodingType.Enc_utf_32BE;
						goto default;
					}
					return System.Text.Encoding.Unicode;
				case EncodingType.EncEmbedded:
					return System.Text.Encoding.GetEncoding(this.ReadUInt16(EncodingType.U2));
				//case EncodingType.EncRaw:// ��������������
				default:
					try{
						return System.Text.Encoding.GetEncoding((int)((uint)enc>>16));
					}catch{
						return System.Text.Encoding.UTF8;
					}
			}
		}
		/// <summary>
		/// Stream ����w�肵�������̃f�[�^��ǂݎ���ĕ�����ɂ��܂��B
		/// </summary>
		/// <param name="enc">��������i�[����̂Ɏg�p���Ă��镶���R�[�h���w�肵�܂��B</param>
		/// <param name="bytecount">��������̒����� byte �P�ʂŎw�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadStringByByteCount(EncodingType enc,int bytecount){
			return this.ReadString_bytecount(detect_encoding(enc,false,0),(uint)bytecount);
		}
		/// <summary>
		/// Stream ����w�肵�������̃f�[�^��ǂݎ���ĕ�����ɂ��܂��B
		/// </summary>
		/// <param name="enc">��������i�[����̂Ɏg�p���Ă��镶���R�[�h���w�肵�܂��B</param>
		/// <param name="bytecount">��������̒����� byte �P�ʂŎw�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadStringByByteCount(EncodingType enc,uint bytecount){
			return this.ReadString_bytecount(detect_encoding(enc,false,0),bytecount);
		}
		/// <summary>
		/// Stream ����w�肵������������������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">��������i�[����̂Ɏg�p���Ă��镶���R�[�h���w�肵�܂��B</param>
		/// <param name="charcount">�ǂݎ�镶����̕��������w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadStringByCharCount(EncodingType enc,int charcount){
			return this.ReadString_charcount(detect_encoding(enc,false,0),(uint)charcount);
		}
		/// <summary>
		/// Stream ����w�肵������������������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">��������i�[����̂Ɏg�p���Ă��镶���R�[�h���w�肵�܂��B</param>
		/// <param name="charcount">�ǂݎ�镶����̕��������w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadStringByCharCount(EncodingType enc,uint charcount){
			return this.ReadString_charcount(detect_encoding(enc,false,0),charcount);
		}
		/// <summary>
		/// Stream �������ǂݍ��݂܂��B
		/// </summary>
		/// <param name="enc">���� Stream �ւ̊i�[�`�����w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾕������ <see cref="string"/> �ŕԂ��܂��B</returns>
		public string ReadString(EncodingType enc){
			System.Text.Encoding encoding;
			switch(enc&EncodingType.MASK_TYPE){
				case EncodingType.NoSpecified:
					enc=EncodingType.EncEmbedded|EncodingType.Enc_utf_16;
					goto case EncodingType.StrSize;
				case EncodingType.CC2:
					read(2);
					return System.Text.Encoding.ASCII.GetString(this.buff,0,2);
				case EncodingType.CC3:
					read(3);
					return System.Text.Encoding.ASCII.GetString(this.buff,0,3);
				case EncodingType.CC4:
					read(4);
					return System.Text.Encoding.ASCII.GetString(this.buff,0,4);
				case EncodingType.String: // �Ō㖘�ǂݎ��
					encoding=detect_encoding(enc,false,0);
					return this.ReadString_toend(encoding);
				case EncodingType.StrBasic:
					encoding=detect_encoding(enc,false,4);
					return this.ReadString_charcount(encoding,this.ReadUInt32(EncodingType.U4));
				case EncodingType.StrPascal:
					encoding=detect_encoding(enc,false,1);
					return this.ReadString_charcount(encoding,(uint)this.stream.ReadByte());
				case EncodingType.StrSize:
					encoding=detect_encoding(enc,false,4);
					return this.ReadString_bytecount(encoding,this.ReadUInt32(EncodingType.U4));
				case EncodingType.StrTerminated:
					encoding=detect_encoding(enc,true,0);
					return this.ReadString_terminated(encoding);
				default:
					throw new System.ApplicationException("�w�肵����ނ��� System.String �ւ̓Ǎ��ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// Stream ���當�����ǂݎ��܂��BStream �̖��[�܂őS�ēǂݎ��܂��B
		/// </summary>
		/// <param name="encoding">������̕����Ɏg�p���� <see cref="System.Text.Encoding"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾕������ <see cref="string"/> �ŕԂ��܂��B</returns>
		private string ReadString_toend(System.Text.Encoding encoding){
			skip_preamble(encoding);

			System.Text.Decoder dec=encoding.GetDecoder();
			System.Text.StringBuilder build=new System.Text.StringBuilder();
			byte[] b=new byte[256];int ibyte;
			char[] c=new char[256];int ichar;
			do{
				ibyte=this.stream.Read(b,0,256);
				if(ibyte==0)break;
				ichar=dec.GetChars(b,0,ibyte,c,0);
				if(ichar==0)continue;
				build.Append(c,0,ichar);
			}while(ibyte==256);

			return build.ToString();
		}
		/// <summary>
		/// Stream ���當�����ǂݎ��܂��BNull �����܂ł̏���ǂݎ��܂��B
		/// </summary>
		/// <param name="encoding">������̕����Ɏg�p���� <see cref="System.Text.Encoding"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾕������ <see cref="string"/> �ŕԂ��܂��B</returns>
		private string ReadString_terminated(System.Text.Encoding encoding){
			skip_preamble(encoding);

			System.Text.Decoder dec=encoding.GetDecoder();
			System.Text.StringBuilder build=new System.Text.StringBuilder();
			char[] c=new char[1];
			while(readbyte()){
				if(dec.GetChars(this.buff,0,1,c,0)==0)continue;
				else if(c[0]=='\0')break;
				else build.Append(c[0]);
			}
			return build.ToString();
		}
		/// <summary>
		/// Stream �������ǂݍ��݂܂��B
		/// </summary>
		/// <param name="charcount">�ǂݎ�镶�������w�肵�܂��BBOM �͕������Ɋ܂߂܂���B</param>
		/// <param name="encoding">������̕����Ɏg�p���� <see cref="System.Text.Encoding"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾕������ <see cref="string"/> �ŕԂ��܂��B</returns>
		private string ReadString_charcount(System.Text.Encoding encoding,uint charcount){
			const string ERROR_READOVER=@"�\���󂲂����܂���B������ǂݎ��߂��܂����B
�֐� afh.File.StreamAccessor.ReadString_charcount �� �u1B �ňꕶ���ȏ�̕�����\�����镶���R�[�h�͑��݂��Ȃ��v�Ƃ�������̉��Ɏ�������Ă��܂��B
���̗�O����������̂Ƃ������́A1B �œ񕶎��ȏ��\�� System.Text.Encoding �����݂���Ƃ������������Ă��܂��B
���݂̎����ł͂��̎�ނ̕����R�[�h�ɂ͑Ή����Ă��܂���BReadString_charcount �̎������������K�v������܂��B";
			const int INTMAX=0x7fffffff;
			//---------------------------------------------
			skip_preamble(encoding);
			if(charcount==0)return "";

			System.Text.Decoder dec=encoding.GetDecoder();
			System.Text.StringBuilder build=new System.Text.StringBuilder();

			int capacity=charcount>INTMAX?INTMAX:(int)charcount;
			byte[] bytes=new byte[capacity];
			char[] chars=new char[capacity];
			int ibyte0;	// �ǂݎ��\��� byte ��
			int ibyte;  // ���ۂɓǂݎ���� byte ��
			int ichar;  // ���ۂɕϊ����ďo�Ă��� char ��
			do{
				ibyte0=charcount>INTMAX?INTMAX:(int)charcount;
				ibyte=this.stream.Read(bytes,0,ibyte0);
				if(ibyte==0)break;

				ichar=dec.GetChars(bytes,0,ibyte,chars,0);
				if(ichar==0)continue;

				if(charcount<ichar)
					throw new System.ApplicationException(ERROR_READOVER);
				charcount-=(uint)ichar;

				build.Append(chars,0,ichar);
			}while(charcount>0&&ibyte0==ibyte);

			return build.ToString();
		}
		/// <summary>
		/// Stream �������ǂݍ��݂܂��B
		/// </summary>
		/// <param name="bytecount">��������o�C�g�����w�肵�܂��B</param>
		/// <param name="encoding">������̕����Ɏg�p���� <see cref="System.Text.Encoding"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݍ��񂾕������ <see cref="string"/> �ŕԂ��܂��B</returns>
		private string ReadString_bytecount(System.Text.Encoding encoding,uint bytecount){
			bytecount-=(uint)skip_preamble(encoding);

			const uint INTSUP=0x80000000;
			if((bytecount&INTSUP)!=0){
				System.Text.Decoder dec=encoding.GetDecoder();
				System.Text.StringBuilder build=new System.Text.StringBuilder();
				byte[] buffer=new byte[INTSUP];
				char[] chars=new char[INTSUP];

				// INTSUP - 1 ����
				int ibyte=this.stream.Read(buffer,0,unchecked((int)(INTSUP-1)));
				int ichar=dec.GetChars(buffer,0,ibyte,chars,0);
				build.Append(chars,0,ichar);
				if((uint)ibyte!=INTSUP)return build.ToString();

				// 1 ����
				ibyte=this.stream.Read(buffer,0,1);
				ichar=dec.GetChars(buffer,0,ibyte,chars,0);
				build.Append(chars,0,ichar);
				if(ibyte!=1)return build.ToString();

				// �c��
				ibyte=this.stream.Read(buffer,0,(int)(bytecount&~INTSUP));
				ichar=dec.GetChars(buffer,0,ibyte,chars,0);
				build.Append(chars,0,ichar);
				return build.ToString();
			}else{
				byte[] buffer=new byte[bytecount];
				int ibyte=this.stream.Read(buffer,0,(int)bytecount);
				return encoding.GetString(buffer,0,ibyte);
			}
		}
		#endregion

		#region Write:������
		/// <summary>
		/// �������ݗp�� <see cref="System.Text.Encoding"/> ���擾���܂��B
		/// </summary>
		/// <param name="enc">�������ݗp�̕����R�[�h���w�肵�܂��B
		/// <include file="document.xml" path='document/desc[@name="afh.File.StreamAccessor::enc2"]/*'/>
		/// </param>
		/// <param name="terminated">
		/// ���ꂪ null �I�[������ł��鎞�� true ���w�肵�܂��B����ȊO�̏ꍇ�ɂ� false ���w�肵�܂��B
		/// </param>
		/// <param name="bom">BOM �̎w�肪����ꍇ�� BOM ��Ԃ��܂��B</param>
		/// <returns>�擾���� System.Text.Encoding ��Ԃ��܂��B</returns>
		private System.Text.Encoding write_encoding(EncodingType enc,bool terminated,out byte[] bom){
			uint codepage=(uint)enc>>16;
			System.Text.Encoding encoding;
			switch(enc&EncodingType.MASK_STRING_ENCSPEC){
				case EncodingType.EncAnsi:
					bom=null;
					return AnsiEncoding;
				case EncodingType.EncWide:
					switch(enc&EncodingType.MASK_OPTION){
						case EncodingType.Enc_utf_32:
						case EncodingType.Enc_utf_32BE:
							if(terminated)goto default;
							goto case EncodingType.Enc_utf_8;
						case EncodingType.Enc_utf_8:
						case EncodingType.Enc_utf_16:
						case EncodingType.Enc_utf_16BE:
							encoding=System.Text.Encoding.GetEncoding((int)codepage);
							break;
						default:
							encoding=System.Text.Encoding.Unicode;
							break;
					}
					bom=encoding.GetPreamble();
					return encoding;
				case EncodingType.EncEmbedded:
					this.Write((ushort)codepage,EncodingType.U2);
					bom=null;
					return System.Text.Encoding.GetEncoding((int)codepage);
			//	case EncodingType.EncRaw:
				default:
					bom=null;
					return System.Text.Encoding.GetEncoding((int)codepage);
			}
		}
		/// <summary>
		/// ������� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="value">�������ޒl���w�肵�܂��B</param>
		/// <param name="enc">�l���������ޕ��@�ƕ����R�[�h���w�肵�܂��B
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::enc2"]/*'/>
		/// </param>
		/// <remarks>
		/// �ǂݏ������镶����̌`���͈ȉ��̗l�ɂȂ�܂�:
		/// <c>ENC? LEN? C* 0?</c>
		/// <dl>
		/// <dt>ENC</dt><dd>Charater Encoding (EncEmbedded ���w�肵���ꍇ 2B)</dd>
		/// <dt>LEN</dt><dd>Length (StrPascal ���w�肵���ꍇ 1B; StrSize �� StrBasic ���w�肵���ꍇ 4B)</dd>
		/// <dt>C</dt><dd>�����̗�</dd>
		/// <dt>0</dt><dd>�I�[���� (StrTerminated ���w�肵���ꍇ)</dd>
		/// </dl>
		/// </remarks>
		public void Write(string value,EncodingType enc){
			if(value==null)throw new System.ArgumentNullException("value");
			System.Text.Encoding encoding;
			byte[] bom;
			switch(enc&EncodingType.MASK_TYPE){
				case EncodingType.NoSpecified:
					enc=EncodingType.EncEmbedded|EncodingType.Enc_utf_16;
					goto case EncodingType.StrSize;
				case EncodingType.CC2:
					if(value.Length>2)value=value.Substring(0,2);
					else while(value.Length<2)value+=" ";
					System.Text.Encoding.ASCII.GetBytes(value,0,2,this.buff,0);
					this.stream.Write(this.buff,0,2);
					break;
				case EncodingType.CC3:
					if(value.Length>3)value=value.Substring(0,3);
					else while(value.Length<3)value+=" ";
					System.Text.Encoding.ASCII.GetBytes(value,0,3,this.buff,0);
					this.stream.Write(this.buff,0,3);
					break;
				case EncodingType.CC4:
					if(value.Length>4)value=value.Substring(0,4);
					else while(value.Length<4)value+=" ";
					System.Text.Encoding.ASCII.GetBytes(value,0,4,this.buff,0);
					this.stream.Write(this.buff,0,4);
					break;
				case EncodingType.String:
					encoding=write_encoding(enc,false,out bom);
					this.Write(bom);
					this.Write(encoding.GetBytes(value));
					break;
				case EncodingType.StrBasic:
					encoding=write_encoding(enc,false,out bom);
					//if(value.Length>uint.MaxValue) // �� ���蓾�Ȃ�
					//	throw new System.ArgumentException(GetMessage_StringTooLong(value,uint.MaxValue),"value");
					this.Write((uint)value.Length,EncodingType.U4);
					this.Write(bom);
					this.Write(encoding.GetBytes(value));
					break;
				case EncodingType.StrPascal:
					encoding=write_encoding(enc,false,out bom);
					if(value.Length>byte.MaxValue)
						throw new System.ArgumentException(GetMessage_StringTooLong(value,byte.MaxValue),"value");
					this.Write((byte)value.Length,EncodingType.U1);
					this.Write(bom);
					this.Write(encoding.GetBytes(value));
					break;
				case EncodingType.StrSize:
					encoding=write_encoding(enc,false,out bom);
					byte[] buffer=encoding.GetBytes(value);
					long len=buffer.Length;if(bom!=null)len+=bom.Length;
					if(len>uint.MaxValue) // 4GB �ȏ�̎�
						throw new System.ArgumentException(GetMessage_StringTooLarge(value,uint.MaxValue),"value");
					this.Write((uint)len,EncodingType.U4);
					this.Write(bom);
					this.Write(buffer);
					break;
				case EncodingType.StrTerminated:
					encoding=write_encoding(enc,true,out bom);
					this.Write(bom);
					this.Write(encoding.GetBytes(value+'\0'));
					break;
				default:
					throw new System.ApplicationException("System.String ����w�肵����ނւ̏����ɂ͑Ή����Ă��܂���B");
			}
		}
		/// <summary>
		/// ��������������݂܂��B
		/// </summary>
		/// <param name="value">�������ޒl���w�肵�܂��B</param>
		/// <param name="length">�������ޕ�����̒����𕶎����Ŏw�肵�܂��B</param>
		public void WriteStringByCharCount(string value,int length) {
			int delta=value.Length-length;
			this.Write(
				delta>0?value.Substring(0,length) : delta<0?value+new string(' ',delta) : value,
				EncodingType.String);
		}
		private static string GetMessage_StringTooLong(string value,uint maxlen){
			const string F="�����Ɏw�肵�������񂪒������܂��B\r\n    �w�肵�������� \"{0}\" �̒���: {1}\r\n    �ő啶����: {2}";
			return string.Format(F,value,value.Length,maxlen);
		}
		private static string GetMessage_StringTooLarge(string value,uint maxlen){
			const string F="�����Ɏw�肵�������񂪒������܂��B\r\n    �w�肵�������� \"{0}\" �� byte ��: {1}\r\n    �ő� byte ��: {2}";
			return string.Format(F,value,value.Length,maxlen);
		}
		#endregion

		//===========================================================
		//		��
		//===========================================================
		/// <summary>
		/// �w�肵���̈�� byte[] �Ƃ��ČŒ肵�A�w�肵��������s���܂��B
		/// </summary>
		/// <param name="index">�̈�̏��߂̈ʒu���w�肵�܂��B</param>
		/// <param name="length">�̈�̒������w�肵�܂��B</param>
		/// <param name="deleg">
		/// �ύX��K�p����ꍇ�ɂ� true ��Ԃ��܂��B
		/// �ύX���s���Ă��Ȃ��ꍇ�A���͕ύX��j������ꍇ�ɂ� false ��Ԃ��܂��B
		/// true ���Ԃ��ꂽ�ꍇ�ɂ́A�ύX���s��ꂽ byte[] ��Ή����� <see cref="System.IO.Stream"/> �̗̈�ɏ㏑�����܂��B
		/// �㏑������ꍇ�ɂ� <paramref name="deleg"/> �ɓn���� readlength �̒����ł͂Ȃ��āA
		/// ���߂ɗv�������̈�̒��� <paramref name="length"/> �����㏑������鎖�ɂȂ�܂��B
		/// </param>
		public virtual unsafe void Fix(long index,int length,PointerOperation deleg){
			byte[] buff=new byte[length];
			this.stream.Seek(0,System.IO.SeekOrigin.Begin);
			int readlength=this.stream.Read(buff,0,length);
			fixed(byte* b=&buff[0])if(deleg(b,readlength)){
				this.stream.Seek(0,System.IO.SeekOrigin.Begin);
				this.stream.Write(buff,0,length);
			}
		}

		/// <summary>
		/// �X���b�h�Z�[�t�� StreamAccessor ��񋟂��܂��B
		/// �ڍׂɊւ��Ă� <see cref="SyncStreamAccessor"/> ���Q�Ƃ��ĉ������B
		/// </summary>
		/// <param name="accessor">���ɂȂ� StreamAccessor ���w�肵�܂��B</param>
		/// <returns>
		/// SyncStreamAccessor �̃C���X�^���X��Ԃ��܂��B
		/// </returns>
		public static SyncStreamAccessor Synchronized(StreamAccessor accessor){
			return (accessor  as SyncStreamAccessor)??new SyncStreamAccessor(accessor.stream);
		}
		/// <summary>
		/// EncodingType �𕶎���ɂ��܂��B
		/// </summary>
		/// <param name="enc">������ɂ��� EncodingType �̒l���w�肵�܂��B</param>
		/// <returns>������ɂ��ĕ\�����ꂽ EncodingType ���擾���܂��B</returns>
		public static string EncodingTypeToString(EncodingType enc){
			string r=afh.Enum.GetDescription(enc&EncodingType.MASK_TYPE,"TYPE");
			if(!r.StartsWith("Str"))return "EncodingType."+r;
			return "EncodingType."+r
				+"|EncodingType."+afh.Enum.GetDescription(enc&EncodingType.MASK_STRING_ENCSPEC,"STRING_ENCSPEC")
				+"|EncodingType."+afh.Enum.GetDescription(enc&EncodingType.MASK_OPTION,"CHARCTER_CODE");
		}
		/// <summary>
		/// ������G���R�[�f�B���O�� EncodingType �ŕ\�����܂��B
		/// </summary>
		/// <param name="enc">EncodingType �ŕ\�������� Encoding ���w�肵�܂��B</param>
		/// <returns>enc �� EncodingType �ɕϊ������l��Ԃ��܂��B</returns>
		public static EncodingType EncodingToEncodingType(System.Text.Encoding enc){
			return (EncodingType)(uint)(enc.CodePage<<16);
		}
	}

	/// <summary>
	/// (�T��) �X���b�h�Z�[�t�� <see cref="StreamAccessor"/> �ł��B
	/// <para>
	/// �A���AStream �̖��[�𒴂��ēǂݎ�낤�Ƃ����ꍇ (��O�𔭐������܂�) �ɂ́A�X���b�h�Z�[�t�ł͂���܂���B
	/// ���ɂ��̏ꍇ�ɑ��̃X���b�h���� Seek(SeekOrigin.Current) ��v�������� Seek ��̈ʒu�������\��������܂��B
	/// �R���A���ۂɂ́u�����̃X���b�h���玩�R�� Seek ����������l�ȏ�Ԃł̎g����������v
	/// �Ƃ͍l���������̂Ŋ����ē������s��Ȃ��d�l�ɂȂ��Ă��܂��B
	/// </para>
	/// </summary>
	public sealed class SyncStreamAccessor:StreamAccessor{
		private System.IO.Stream unsync;
		/// <summary>
		/// StreamAccessor �����������܂��B
		/// </summary>
		/// <param name="unsyncstream">�����������ސ�� stream ���w�肵�܂��B</param>
		public SyncStreamAccessor(System.IO.Stream unsyncstream):base(System.IO.Stream.Synchronized(unsyncstream)){
			this.unsync=unsyncstream;
		}

		/// <summary>
		/// �w�肵���̈�� byte[] �Ƃ��ČŒ肵�A�w�肵��������s���܂��B
		/// </summary>
		/// <param name="index">�̈�̏��߂̈ʒu���w�肵�܂��B</param>
		/// <param name="length">�̈�̒������w�肵�܂��B</param>
		/// <param name="deleg">
		/// �ύX��K�p����ꍇ�ɂ� true ��Ԃ��܂��B
		/// �ύX���s���Ă��Ȃ��ꍇ�A���͕ύX��j������ꍇ�ɂ� false ��Ԃ��܂��B
		/// true ���Ԃ��ꂽ�ꍇ�ɂ́A�ύX���s��ꂽ byte[] ��Ή����� <see cref="System.IO.Stream"/> �̗̈�ɏ㏑�����܂��B
		/// �㏑������ꍇ�ɂ� <paramref name="deleg"/> �ɓn���� readlength �̒����ł͂Ȃ��āA
		/// ���߂ɗv�������̈�̒��� <paramref name="length"/> �����㏑������鎖�ɂȂ�܂��B
		/// </param>
		public unsafe override void Fix(long index,int length,PointerOperation deleg){
			if(!freeFix){
				lock(this.unsync)base.Fix(index,length,deleg);
				return;
			}

			// deleg �̏����Ɏ��Ԃ������� & ���̃X���b�h���瑀�삷��p�x�������ꍇ�ɂ͂������B
			byte[] buff=new byte[length];
			int readlength;
			lock(this.unsync){
				this.unsync.Seek(0,System.IO.SeekOrigin.Begin);
				readlength=this.unsync.Read(buff,0,length);
			}
			fixed(byte* b=&buff[0])if(deleg(b,readlength))lock(this.unsync){
				this.unsync.Seek(0,System.IO.SeekOrigin.Begin);
				this.unsync.Write(buff,0,length);
			}
		}
		private bool freeFix=false;
		/// <summary>
		/// Stream �̓��e�� byte[] �Ɉڂ��đ�������Ă���ԂɁAStream �� lock ���������邩�ǂ������擾���͐ݒ肵�܂��B
		/// (���Ȃ��Ƃ� Stream ���� byte[] �ցA�܂��Abyte[] ���� Stream �ւ̃f�[�^�]�����ɂ� lock ����Ă���K�v������܂��B)
		/// <para>����l�� false �ł��B</para>
		/// <para>
		/// <see cref="M:Fix"/> �̍ۂ̏����Ɏ��Ԃ������芎���̊Ԃɑ��̃X���b�h���� Stream �𑀍삵�悤�Ƃ���p�x�������ꍇ�ɂ� true ���w�肵�܂��B
		/// �܂��A<see cref="M:Fix"/> �̍ۂ̏����Ɏ��Ԃ������芎���̊ԂɕύX���s��Ȃ��ꍇ (�܂� Fix �̈��� deleg �� false ��Ԃ��ꍇ) �ɂ� true ���w�肵�܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ���w�肵�ĉ������Btrue ���w�肷��� lock ����s�����ɂȂ�I�[�o�[�w�b�h���傫���Ȃ�܂��̂ŁA�ɗ� false ���w�肷��l�ɂ��ĉ������B
		/// </para>
		/// </summary>
		public bool FreeWhileFixing{
			get{return this.freeFix;}
			set{this.freeFix=value;}
		}
	}

	/// <summary>
	/// �|�C���^�����\�����\�b�h��\���܂��B
	/// </summary>
	/// <param name="ptr">����Ώۂ̏��ւ̃|�C���^���w�肵�܂��B</param>
	/// <param name="readlength">
	/// ���ۂɓǂݎ�鎖���o�����������w�肵�܂��B
	/// �v�����ꂽ������ǂݎ��O�� Stream �̏I�[�𒴂��Ă����ꍇ�ȂǂɁA
	/// �v�����ꂽ������ readlength �̒������قȂ�l�ɂȂ�܂��B
	/// �̈�� readlength �ȏ�� index �̏ꏊ���ύX�\�ł��B
	/// </param>
	/// <returns>
	/// �����Ȃǂ̕ύX���s�����ꍇ�� true ��Ԃ��܂��B
	/// �ύX���s��Ȃ������ꍇ�A���͕ύX��j�������̏�Ԃ̘Ԃɂ��Ēu���ɂ� false ��Ԃ��܂��B
	/// </returns>
	public unsafe delegate bool PointerOperation(byte* ptr,int readlength);

	/// <summary>
	/// �t�@�C���̓��e���\�������`���ƈقȂ�ꍇ�ɔ����������O�ł��B
	/// </summary>
	[System.Serializable]
	public sealed class FileFormatException:System.Exception{
		private const string DEFAULT_MSG="�t�@�C���^�X�g���[���̌`�����\�����ꂽ���ƈقȂ�܂��B�������`���̃f�[�^���w�肵�ĉ������B";
		public FileFormatException():base(DEFAULT_MSG){}
		public FileFormatException(System.Exception innerException):base(DEFAULT_MSG,innerException){}
		public FileFormatException(string additional_msg):base(DEFAULT_MSG+additional_msg){}
		public FileFormatException(string additional_msg,System.Exception innerException):base(DEFAULT_MSG+additional_msg,innerException){}
	}

	/// <summary>
	/// Stream �̏I�[���z���ēǂݏ����̓�����g�p�Ƃ����ꍇ�ɔ����������O�ł��B
	/// </summary>
	[System.Serializable]
	public sealed class StreamOverRunException:System.Exception{
		const string OVERREAD="Stream �̏I�[���z���ēǂݍ������Ƃ��Ă��܂�";
		const string OVERWRITE="Stream �̏I�[���z���ď����������Ƃ��Ă��܂�";
		/// <summary>
		/// StreamOverRunException �̃R���X�g���N�^�ł��B
		/// </summary>
		public StreamOverRunException():base(OVERREAD+"�B"){}
		/// <summary>
		/// StreamOverRunException �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="message">��O�Ɋւ���ǉ��̐������w�肵�܂��B</param>
		public StreamOverRunException(string message):base(OVERREAD+": "+message){}
		/// <summary>
		/// StreamOverRunException �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="innerException">���̗�O�𔭐������錴���ƂȂ������O���������ꍇ�ɁA������w�肵�܂��B</param>
		public StreamOverRunException(System.Exception innerException):base(OVERREAD+"�B",innerException){}
		/// <summary>
		/// StreamOverRunException �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="message">��O�Ɋւ���ǉ��̐������w�肵�܂��B</param>
		/// <param name="innerException">���̗�O�𔭐������錴���ƂȂ������O���������ꍇ�ɁA������w�肵�܂��B</param>
		public StreamOverRunException(string message,System.Exception innerException):base(OVERREAD+": "+message,innerException){}
	}

	/// <summary>
	/// System.IO.Stream �ɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public static class StreamUtils{
		/// <summary>
		/// System.IO.Compression.GZipStream ���g�p���Ďw�肵�� Stream �����k���V�[�N�\�� MemoryStream �ɒu�������܂��B
		/// </summary>
		/// <param name="stream">���k�̑ΏۂƂȂ� Stream ���w�肵�܂��B
		/// Stream ��S�ēǂݎ���Ĉ��k�������ʂ��i�[���Ă��� MemoryStream ��Ԃ��܂��B
		/// ���̃X�g���[���͕����܂��B</param>
		public static void GZipCompress(ref System.IO.Stream stream){
			System.IO.MemoryStream r=new System.IO.MemoryStream();

			StreamAccessor accessor=new StreamAccessor(new System.IO.Compression.GZipStream(r,System.IO.Compression.CompressionMode.Compress,true));
			accessor.WriteStream(stream);
			accessor.Stream.Close();
			System.Console.WriteLine("Before {0}, After {1}",stream.Length,r.Length);

			stream.Close();
			stream=r;
			r.Position=0;
		}
		/// <summary>
		/// System.IO.Compression.GZipStream ���g�p���Ďw�肵�� Stream ��W�J���V�[�N�\�� MemoryStream �ɒu�������܂��B
		/// </summary>
		/// <param name="stream">�W�J�̑ΏۂƂȂ鈳�k���ꂽ Stream ���w�肵�܂��B
		/// Stream ��S�ēǂݎ���ēW�J�������ʂ��i�[���Ă��� MemoryStream ��Ԃ��܂��B
		/// ���̃X�g���[���͕����܂��B</param>
		public static void GZipDecompress(ref System.IO.Stream stream){
			stream.Position=0;
			System.IO.Stream instr=new System.IO.Compression.GZipStream(stream,System.IO.Compression.CompressionMode.Decompress,true);

			System.IO.MemoryStream outstr=new System.IO.MemoryStream();
			int c;
			const int BLOCK=0x10000;
			byte[] buff=new byte[BLOCK];
			do{
				c=instr.Read(buff,0,BLOCK);
				if(c==0)break;
				outstr.Write(buff,0,c);
			}while(c==BLOCK);

			System.Console.WriteLine("Before {0}, After {1}",stream.Length,outstr.Length);
			outstr.Position=0;

			stream.Close();
			stream=outstr;
		}
		/// <summary>
		/// System.IO.Compression.DeflateStream ���g�p���Ďw�肵�� Stream �����k���V�[�N�\�� MemoryStream �ɒu�������܂��B
		/// </summary>
		/// <param name="stream">���k�̑ΏۂƂȂ� Stream ���w�肵�܂��B
		/// Stream ��S�ēǂݎ���Ĉ��k�������ʂ��i�[���Ă��� MemoryStream ��Ԃ��܂��B
		/// ���̃X�g���[���͕����܂��B</param>
		public static void ZipCompress(ref System.IO.Stream stream) {
			System.IO.MemoryStream r=new System.IO.MemoryStream();

			StreamAccessor accessor=new StreamAccessor(new System.IO.Compression.DeflateStream(r,System.IO.Compression.CompressionMode.Compress,true));
			accessor.WriteStream(stream);
			accessor.Stream.Close();
			System.Console.WriteLine("Before {0}, After {1}",stream.Length,r.Length);

			stream.Close();
			stream=r;
			r.Position=0;
		}
		/// <summary>
		/// System.IO.Compression.DeflateStream ���g�p���Ďw�肵�� Stream ��W�J���V�[�N�\�� MemoryStream �ɒu�������܂��B
		/// </summary>
		/// <param name="stream">�W�J�̑ΏۂƂȂ鈳�k���ꂽ Stream ���w�肵�܂��B
		/// Stream ��S�ēǂݎ���ēW�J�������ʂ��i�[���Ă��� MemoryStream ��Ԃ��܂��B
		/// ���̃X�g���[���͕����܂��B</param>
		public static void ZipDecompress(ref System.IO.Stream stream) {
			stream.Position=0;
			System.IO.Stream instr=new System.IO.Compression.DeflateStream(stream,System.IO.Compression.CompressionMode.Decompress,true);

			System.IO.MemoryStream outstr=new System.IO.MemoryStream();
			int c;
			const int BLOCK=0x10000;
			byte[] buff=new byte[BLOCK];
			do {
				c=instr.Read(buff,0,BLOCK);
				if(c==0) break;
				outstr.Write(buff,0,c);
			} while(c==BLOCK);

			System.Console.WriteLine("Before {0}, After {1}",stream.Length,outstr.Length);
			outstr.Position=0;

			stream.Close();
			stream=outstr;
		}
	}
}