using Marshal=System.Runtime.InteropServices.Marshal;
using Gen=System.Collections.Generic;
namespace afh.File{
	using afh.File.Design;

	/// <summary>
	/// System.IO.Stream を通じて様々な情報を書き込んだり読み込んだりする方法を提供するクラスです。
	/// </summary>
	public partial class StreamAccessor{
		/// <summary>
		/// 読み込んだり書き込んだりする対象の <see cref="System.IO.Stream"/> を保持します。
		/// </summary>
		protected System.IO.Stream stream;
		/// <summary>
		/// 現在の読み書き対象の Stream を取得します。
		/// </summary>
		public System.IO.Stream Stream{
			get{return this.stream;}
		}
		/// <summary>
		/// 小さなデータを読み込んだり書き込んだりする際に使用するバッファです。
		/// </summary>
		protected byte[] buff=new byte[16];

		/// <summary>
		/// StreamAccessor を初期化します。
		/// </summary>
		/// <param name="stream">情報を書き込む先の <see cref="System.IO.Stream"/> を指定します。
		/// この Stream は、読み書きの途中にラップされる可能性がありますので、
		/// 後から Stream に操作を加える場合には <see cref="P:StreamAccessor.Stream"/> を通して行って下さい。</param>
		public StreamAccessor(System.IO.Stream stream):base(){
			//if(!stream.CanRead||!stream.CanSeek||!stream.CanWrite)
			//	throw new System.ArgumentException("読み書き移動が可能なストリームにしか対応していません","stream");
			this.stream=stream;
		}
		static StreamAccessor(){
			System.Array.Clear(zeroes,0,ZERO_BUFF_SIZE);
		}
		//===========================================================
		//		Stream 位置管理
		//===========================================================
		/// <summary>
		/// Stream 内の現在位置を取得亦は設定します。
		/// </summary>
		public long Position{
			get{return this.stream.Position;}
			set{this.stream.Position=value;}
		}
		/// <summary>
		/// 現在の位置を記録して、新しい位置に移動します。
		/// </summary>
		/// <param name="position">新しい位置を指定します。</param>
		public void PushPosition(long position) {
			if(this.pos_stack==null){
				this.pos_stack=new Gen::Stack<long>();
			}
			this.pos_stack.Push(this.stream.Position);
			this.stream.Position=position;
		}
		/// <summary>
		/// PushPosition メソッドで移動する前の位置に戻ります。
		/// </summary>
		/// <returns>このメソッドを実行する前に居た位置を返します。</returns>
		public long PopPosition(){
			long position=this.stream.Position;
			this.stream.Position=this.pos_stack.Pop();
			return position;
		}
		/// <summary>
		/// 指定した長さだけ Stream を読み飛ばします。
		/// </summary>
		/// <param name="len">読み飛ばす長さをバイト単位で指定します。</param>
		public void Skip(long len) {
			this.stream.Seek(len,System.IO.SeekOrigin.Current);
		}
		private Gen::Stack<long> pos_stack;

		/// <summary>
		/// Stream の残りの長さを取得します。
		/// Position 及び Length が使用不可能の場合には例外が発生します。
		/// </summary>
		public long RestLength{
			get{return this.stream.Length-this.stream.Position;}
		}
		//===========================================================
		//		原始型 Primitive Types
		//===========================================================
		/// <summary>
		/// Int28 形式で表現出来る最小の数値を保持します。
		/// </summary>
		public const int MinInt28=unchecked((int)0xf8000000);
		/// <summary>
		/// Int28 形式で表現出来る最大の数値を保持します。
		/// </summary>
		public const int MaxInt28=0x07ffffff;
		/// <summary>
		/// UInt28 形式で表現出来る最大の数値を保持します。
		/// </summary>
		public const int MaxUInt28=0x0fffffff;
		/// <summary>
		/// Int24 形式で表現出来る最小の数値を保持します。
		/// </summary>
		public const int MinInt24=unchecked((int)0xff800000);
		/// <summary>
		/// Int24 形式で表現出来る最大の数値を保持します。
		/// </summary>
		public const int MaxInt24=0x007fffff;
		/// <summary>
		/// UInt24 形式で表現出来る最大の数値を保持します。
		/// </summary>
		public const int MaxUInt24=0x00ffffff;
		
		#region Read:原始
		/// <summary>
		/// Stream から情報を読み込みます。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み込んだ情報を <see cref="bool"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Boolean への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="float"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Single への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="double"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Single への読込には対応していません。");
			}
		}
		//=================================================
		//		整数の読込
		//=================================================
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="sbyte"/> で返します。</returns>
		public sbyte ReadSByte(EncodingType enc){
			if(enc!=EncodingType.I1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("指定した種類から System.SByte への読込には対応していません。");
			read(1);
			return (sbyte)this.buff[0];
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="byte"/> で返します。</returns>
		public byte ReadByte(EncodingType enc){
			if(enc!=EncodingType.U1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("指定した種類から System.Byte への読込には対応していません。");
			read(1);
			return this.buff[0];
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="short"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Int16 への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="ushort"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Int16 への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="int"/> で返します。</returns>
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
					return value<<8>>8; // 符号拡張
				case EncodingType.I3BE:
					read(3);
					return (sbyte)this.buff[0]<<16|this.buff[1]<<8|this.buff[2];
				case EncodingType.Int28:
					read(4);
					value=0x7f&this.buff[0]|(0x7f&this.buff[1])<<7|(0x7f&this.buff[2])<<14|(0x7f&this.buff[3])<<21;
					return value<<4>>4; // 符号拡張
				case EncodingType.Int28BE:
					read(4);
					value=(0x7f&this.buff[0])<<21|(0x7f&this.buff[1])<<14|(0x7f&this.buff[2])<<7|0x7f&this.buff[3];
					return value<<4>>4; // 符号拡張
				default:
					throw new System.ApplicationException("指定した種類から System.Int32 への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="uint"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.UInt32 への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="long"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.Int64 への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から情報を読み取ります。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み取った情報を <see cref="ulong"/> で返します。</returns>
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
					throw new System.ApplicationException("指定した種類から System.UInt64 への読込には対応していません。");
			}
		}
		#endregion

		#region Write:原始
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="bool"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Boolean から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="float"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Single から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="double"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Double から指定した種類への書込には対応していません。");
			}
		}
		//=================================================
		//		整数型の書込
		//=================================================
		/// <summary>
		/// Stream に <see cref="byte"/> を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		public void Write(byte value,EncodingType enc){
			if(enc!=EncodingType.U1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("System.Byte から指定した種類への書込には対応していません。");
			this.stream.WriteByte(value);
		}
		/// <summary>
		/// Stream に <see cref="sbyte"/> を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		public void Write(sbyte value,EncodingType enc){
			if(enc!=EncodingType.I1&&enc!=EncodingType.NoSpecified)
				throw new System.ApplicationException("System.SByte から指定した種類への書込には対応していません。");
			this.stream.WriteByte((byte)value);
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="short"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Int16 から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="ushort"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.UInt16 から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="int"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Int32 から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="uint"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.UInt32 から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="long"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.Int64 から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// Stream に情報を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を <see cref="ulong"/> で指定します。</param>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
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
					throw new System.ApplicationException("System.UInt64 から指定した種類への書込には対応していません。");
			}
		}
		#endregion

		//===========================================================
		//		他
		//===========================================================
		private const int ZERO_BUFF_SIZE=0x100;
		private static byte[] zeroes=new byte[ZERO_BUFF_SIZE];
		/// <summary>
		/// 指定した長さだけゼロクリアを行います。
		/// つまり、指定した長さだけ 0 を書き込みます。
		/// </summary>
		/// <param name="length">0 で埋め尽くす長さを指定します。</param>
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
		/// 部分 Stream を初期化して返します。
		/// 作成した SubStream の長さの分だけ現在位置を進めます。
		/// </summary>
		/// <param name="length">作成する SubStream の長さを指定します。
		/// 現在の Stream の残り長さより大きな値を指定した場合には SubStream の長さは残り長さになります。</param>
		/// <returns>作成した SubStream を返します。</returns>
		public SubStream ReadSubStream(long length){
			if(length>this.RestLength)length=this.RestLength;
			SubStream r=new SubStream(ref this.stream,length);
			this.stream.Seek(length,System.IO.SeekOrigin.Current);
			return r;
		}
		/// <summary>
		/// 書き込み用に部分 Stream を初期化して返します。
		/// 作成した SubStream の長さの分だけ現在位置を進めます。
		/// </summary>
		/// <param name="length">作成する SubStream の長さを指定します。</param>
		/// <returns>作成した SubStream を返します。</returns>
		public SubStream WriteSubStream(long length){
			SubStream r=new SubStream(ref this.stream,length);
			this.stream.Seek(length,System.IO.SeekOrigin.Current);
			return r;
		}
		/// <summary>
		/// 指定した Stream を初めから書き込みます。
		/// </summary>
		/// <param name="stream">書き込む Stream を指定します。</param>
		public void WriteStream(System.IO.Stream stream){
			const int BUFFSIZE=0x1000; // 4K

			stream.Position=0;

			// 無限 loop (Substream への本 Stream の書込時) を防ぐ為、
			// 初めに書き込む長さを固定
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
			// MaxValue を超過する場合
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

			// 書込
			stream.Read(buff,0,(int)len);
			this.stream.Write(buff,0,(int)len);
#endif
		}
		/// <summary>
		/// バイナリ配列 byte[] を Strema から読み取ります。
		/// </summary>
		/// <param name="length">読み取る配列の大きさを指定します。</param>
		/// <returns>読み取った内容を格納している配列を返します。</returns>
		public byte[] ReadBytes(int length){
			byte[] ret=new byte[length];
			stream.Read(ret,0,length);
			return ret;
		}
		/// <summary>
		/// 指定した byte[] を Stream に書き込みます。
		/// </summary>
		/// <param name="bytes">書き込む内容を保持している byte 配列を指定します。</param>
		public void WriteBytes(byte[] bytes){
			stream.Write(bytes,0,bytes.Length);
		}

		#region Read
		/// <summary>
		/// 指定した長さだけ読み取って <see cref="buff"/> に書き込みます。
		/// </summary>
		/// <param name="len">読み取る長さを指定します。
		/// buff の大きさは 16 なので、それより大きな値を指定する事は出来ません。</param>
		/// <exception cref="System.ApplicationException">
		/// 指定した長さ分だけ読み取る事が出来なかった場合に発生します。
		/// 現在位置は読み取りを試行する前の位置に戻ります。
		/// </exception>
		protected void read(int len){
			int read=this.stream.Read(this.buff,0,len);
			if(len!=read){
				this.stream.Seek(-read,System.IO.SeekOrigin.Current);
				throw new StreamOverRunException("指定した数の byte を読み取る事が出来ませんでした。");
			}
		}
		/// <summary>
		/// 指定した長さだけ先読みして <see cref="M:buff"/> に書き込みます。
		/// 現在位置は読み取りを試行する前の位置に戻ります。
		/// </summary>
		/// <param name="offset">現在位置を基準にした読み取りの開始位置を指定します。</param>
		/// <param name="len">読み取る長さを指定します。
		/// buff の大きさは 16 なので、それより大きな値を指定する事は出来ません。</param>
		/// <exception cref="System.ApplicationException">
		/// 指定した長さ分だけ読み取る事が出来なかった場合に発生します。
		/// </exception>
		private void peek(int offset,int len){
			long pos=this.stream.Position;
			if(offset!=0)this.stream.Seek(offset,System.IO.SeekOrigin.Current);
			int read=this.stream.Read(this.buff,0,len);
			this.stream.Seek(pos,System.IO.SeekOrigin.Begin);
			if(len!=read){
				throw new StreamOverRunException("指定した数の byte を読み取る事が出来ませんでした。");
			}
		}
		/// <summary>
		/// Stream から 1B だけ読み取って、<see cref="M:buff"/>[0] に設定します。
		/// </summary>
		/// <returns>読み取る事が出来た場合に true を返します。それ以外の場合に false を返します。</returns>
		private bool readbyte(){
			return this.stream.Read(this.buff,0,1)==1;
		}
		//=================================================
		//		ReadStruct: 任意の構造体の読込
		//=================================================
		/// <summary>
		/// 指定した型の構造体の途中までを読み込みます。
		/// </summary>
		/// <typeparam name="T">構造体の型を指定します。</typeparam>
		/// <param name="readlength">読み取る長さを指定します。構造体の途中までしか読み取らない場合にその長さを使用します。</param>
		/// <returns>読み取った構造体を返します。</returns>
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::ReadStruct+Example"]/*'/>
		public T ReadStruct<T>(int readlength) where T:struct{
			int strlen=Marshal.SizeOf(typeof(T));
			return (T)this.ReadStruct(typeof(T),strlen,readlength);
		}
		/// <summary>
		/// 指定した型の構造体を読み取ります。
		/// </summary>
		/// <typeparam name="T">読み取る構造体の型を指定します。</typeparam>
		/// <returns>読み取った構造体を返します。</returns>
		public T ReadStruct<T>() where T:struct{
			int strlen=Marshal.SizeOf(typeof(T));
			return (T)this.ReadStruct(typeof(T),strlen,strlen);
		}
		/// <summary>
		/// 指定した型の構造体の途中までを読み込みます。
		/// </summary>
		/// <param name="type">構造体の型を指定します。</param>
		/// <param name="readlength">読み取る長さを指定します。構造体の途中までしか読み取らない場合にその長さを使用します。</param>
		/// <returns>読み取った構造体を返します。</returns>
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::ReadStruct+Example"]/*'/>
		public object ReadStruct(System.Type type,int readlength){
			if(!type.IsValueType)throw new System.ArgumentException("指定した型は構造体ではありません。","type");
			int strlen=Marshal.SizeOf(type);
			return this.ReadStruct(type,strlen,readlength);
		}
		/// <summary>
		/// 指定した型の構造体を読み取ります。
		/// </summary>
		/// <param name="type">読み取る構造体の型を指定します。</param>
		/// <returns>読み取った構造体を返します。</returns>
		public object ReadStruct(System.Type type){
			if(!type.IsValueType)throw new System.ArgumentException("指定した型は構造体ではありません。","type");
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
		//		Read: 任意の型の読込
		//=================================================
		/// <summary>
		/// 指定した型の情報を読み取ります。
		/// </summary>
		/// <typeparam name="T">読み込む情報の型を指定します。</typeparam>
		/// <returns>読み取った情報を指定した型にして返します。</returns>
		public T Read<T>(){
			return (T)this.Read(typeof(T));
		}
		/// <summary>
		/// 指定した型の情報を読み取ります。
		/// </summary>
		/// <param name="type">読み込む情報の型を指定します。</param>
		/// <returns>読み取った情報を指定した型にして返します。</returns>
		public object Read(System.Type type){
			return this.Read(type,EncodingType.NoSpecified);
		}
		/// <summary>
		/// 指定した型の情報を読み取ります。
		/// </summary>
		/// <typeparam name="T">読み込む情報の型を指定します。</typeparam>
		/// <param name="enc">読み込む情報を Stream に格納するのに使われている形式を指定します。</param>
		/// <returns>読み取った情報を指定した型にして返します。</returns>
		public T Read<T>(EncodingType enc){
			return (T)this.Read(typeof(T),enc);
		}
		/// <summary>
		/// 指定した型の情報を読み取ります。
		/// </summary>
		/// <param name="type">読み込む情報の型を指定します。</param>
		/// <param name="enc">読み込む情報を Stream に格納するのに使われている形式を指定します。</param>
		/// <returns>読み取った情報を指定した型にして返します。</returns>
		public object Read(System.Type type,EncodingType enc){
			if(type.IsEnum)type=System.Enum.GetUnderlyingType(type);

			/* 基本型 */
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

			/* 構造体 */
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
			throw new System.ApplicationException("指定した型の書込には対応していません。");
		}
		#endregion

		#region Write
		/// <summary>
		/// 指定した <see cref="byte"/> 配列を Stream に書き込みます。
		/// </summary>
		/// <param name="value">書き込む byte 配列を指定します。</param>
		public void Write(byte[] value){
			if(value==null||value.Length==0)return;
			this.stream.Write(value,0,value.Length);
		}
		//=================================================
		//		WriteStruct: 任意の構造体の書込
		//=================================================
		/// <summary>
		/// 構造体を Stream に書き込みます。
		/// </summary>
		/// <param name="structure">書き込む構造体を指定します。</param>
		public void WriteStruct(System.ValueType structure){
			byte[] buff=struct_to_bytes(structure);
			this.stream.Write(buff,0,buff.Length);
		}
		/// <summary>
		/// 構造体を Stream に書き込みます。
		/// </summary>
		/// <param name="structure">書き込む構造体を指定します。</param>
		/// <param name="length">
		/// 書き込む長さを指定します。構造体の初めから途中までの情報を書き込む事が出来ます。
		/// 構造体自体の長さよりも大きな値を指定した場合にはエラーになります。
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 指定した書き込む長さは構造体の大きさを超えています。
		/// </exception>
		public void WriteStruct(System.ValueType structure,int length){
			byte[] buff=struct_to_bytes(structure);
			this.stream.Write(buff,0,length);
		}
		/// <summary>
		/// 構造体を byte 配列に変換します。動作確認済み。
		/// </summary>
		/// <param name="structure">変換元の構造体を指定します。</param>
		private byte[] struct_to_bytes(System.ValueType structure){
			return this.struct_to_bytes(structure,Marshal.SizeOf(structure));
		}
		/// <summary>
		/// 構造体を byte 配列に変換します。動作確認済み。
		/// </summary>
		/// <param name="structure">変換元の構造体を指定します。</param>
		/// <param name="strlen">構造体の大きさを指定します。正しい大きさを指定して下さい。</param>
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
		//		Write: 任意の型の書込
		//=================================================
		/// <summary>
		/// 指定した情報を指定した型として書き込みます。
		/// </summary>
		/// <typeparam name="T">書き込むデータの型を指定します。</typeparam>
		/// <param name="value">書き込むデータを指定します。</param>
		public void WriteAs<T>(T value)					{this.Write(value,typeof(T),EncodingType.NoSpecified);}
		/// <summary>
		/// 指定した情報を指定した型として書き込みます。
		/// </summary>
		/// <typeparam name="T">書き込むデータの型を指定します。</typeparam>
		/// <param name="value">書き込むデータを指定します。</param>
		/// <param name="enc">データを書き込むのに使用する形式を指定します。</param>
		public void WriteAs<T>(T value,EncodingType enc){this.Write(value,typeof(T),enc);}
		/// <summary>
		/// 指定したデータを、その型に合った方法で書き込みます。
		/// </summary>
		/// <param name="value">書き込むデータを指定します。</param>
		/// <param name="enc">データを書き込むのに使用する形式を指定します。</param>
		public void Write(object value,EncodingType enc){this.Write(value,value.GetType(),enc);}
		/// <summary>
		/// 指定したデータを、その型に合った方法で書き込みます。
		/// </summary>
		/// <param name="value">書き込むデータを指定します。</param>
		public void Write(object value)					{this.Write(value,value.GetType(),EncodingType.NoSpecified);}
		private void Write(object value,System.Type type,EncodingType enc){
			if(type.IsEnum)type=System.Enum.GetUnderlyingType(type);

			/* 基本型 */
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

			/* 構造体 */
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
						afh.File.__dll__.log.WriteError(e,type.FullName+" 構造体の書込に失敗しました。");
						break;
					}
			}
			throw new System.ApplicationException("指定したオブジェクトは書き込む事が出来ません。対応していない種類の型です。");			
		}
		#endregion

		#region Read:文字列
		//=================================================
		//		文字列の読込
		//=================================================
		/// <summary>
		/// 指定した文字コードに対応する Preamble (BOM 等) があった場合に、
		/// その分だけ現在位置を進めます。
		/// </summary>
		/// <param name="encoding">読み取りに使用する文字コードを指定します。</param>
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

					// ASCII を基本にした物でなければ ASCII に置き換え
					byte[] b=encoding_ansi.GetBytes("aA1");
					if(b.Length!=3||b[0]!=0x61||b[1]!=0x41||b[2]!=0x31)
						encoding_ansi=System.Text.Encoding.ASCII;
				}
				return encoding_ansi;
			}
		}
		/// <summary>
		/// 指定した EncodingType から読み取り用 System.Text.Encoding インスタンスを取得します。
		/// <para>このメソッドの実行の際に BOM を読み取る事がありますが、Stream の現在位置は変更しません。</para>
		/// </summary>
		/// <param name="enc">文字コード亦は文字コード選択方法を指定します。</param>
		/// <param name="terminated">
		/// 現在 null 終端文字列を読み取ろうとしている場合に true を指定します。
		/// true を指定すると EncodingType.EncWide に対して utf-32 及び utf-32BE の BOM の確認は行わず utf-16 として扱います。
		/// </param>
		/// <param name="bomoffset">
		/// BOM が記述されていると考えられる場合に、BOM が現在位置よりどれだけ先に記述されているかを指定します。
		/// 現在位置から BOM が始まる場合には 0 を指定します。
		/// </param>
		/// <returns>取得した Encoding を返します。</returns>
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
				//case EncodingType.EncRaw:// 無い方が速い筈
				default:
					try{
						return System.Text.Encoding.GetEncoding((int)((uint)enc>>16));
					}catch{
						return System.Text.Encoding.UTF8;
					}
			}
		}
		/// <summary>
		/// Stream から指定した長さのデータを読み取って文字列にします。
		/// </summary>
		/// <param name="enc">文字列を格納するのに使用している文字コードを指定します。</param>
		/// <param name="bytecount">文字列情報の長さを byte 単位で指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadStringByByteCount(EncodingType enc,int bytecount){
			return this.ReadString_bytecount(detect_encoding(enc,false,0),(uint)bytecount);
		}
		/// <summary>
		/// Stream から指定した長さのデータを読み取って文字列にします。
		/// </summary>
		/// <param name="enc">文字列を格納するのに使用している文字コードを指定します。</param>
		/// <param name="bytecount">文字列情報の長さを byte 単位で指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadStringByByteCount(EncodingType enc,uint bytecount){
			return this.ReadString_bytecount(detect_encoding(enc,false,0),bytecount);
		}
		/// <summary>
		/// Stream から指定した文字数だけ文字を読み取ります。
		/// </summary>
		/// <param name="enc">文字列を格納するのに使用している文字コードを指定します。</param>
		/// <param name="charcount">読み取る文字列の文字数を指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadStringByCharCount(EncodingType enc,int charcount){
			return this.ReadString_charcount(detect_encoding(enc,false,0),(uint)charcount);
		}
		/// <summary>
		/// Stream から指定した文字数だけ文字を読み取ります。
		/// </summary>
		/// <param name="enc">文字列を格納するのに使用している文字コードを指定します。</param>
		/// <param name="charcount">読み取る文字列の文字数を指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadStringByCharCount(EncodingType enc,uint charcount){
			return this.ReadString_charcount(detect_encoding(enc,false,0),charcount);
		}
		/// <summary>
		/// Stream から情報を読み込みます。
		/// </summary>
		/// <param name="enc">情報の Stream への格納形式を指定します。</param>
		/// <returns>読み込んだ文字列を <see cref="string"/> で返します。</returns>
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
				case EncodingType.String: // 最後迄読み取り
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
					throw new System.ApplicationException("指定した種類から System.String への読込には対応していません。");
			}
		}
		/// <summary>
		/// Stream から文字列を読み取ります。Stream の末端まで全て読み取ります。
		/// </summary>
		/// <param name="encoding">文字列の復号に使用する <see cref="System.Text.Encoding"/> を指定します。</param>
		/// <returns>読み込んだ文字列を <see cref="string"/> で返します。</returns>
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
		/// Stream から文字列を読み取ります。Null 文字までの情報を読み取ります。
		/// </summary>
		/// <param name="encoding">文字列の復号に使用する <see cref="System.Text.Encoding"/> を指定します。</param>
		/// <returns>読み込んだ文字列を <see cref="string"/> で返します。</returns>
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
		/// Stream から情報を読み込みます。
		/// </summary>
		/// <param name="charcount">読み取る文字数を指定します。BOM は文字数に含めません。</param>
		/// <param name="encoding">文字列の復号に使用する <see cref="System.Text.Encoding"/> を指定します。</param>
		/// <returns>読み込んだ文字列を <see cref="string"/> で返します。</returns>
		private string ReadString_charcount(System.Text.Encoding encoding,uint charcount){
			const string ERROR_READOVER=@"申し訳ございません。文字を読み取り過ぎました。
関数 afh.File.StreamAccessor.ReadString_charcount は 「1B で一文字以上の文字を表現する文字コードは存在しない」という仮定の下に実装されています。
この例外が発生するのという事は、1B で二文字以上を表す System.Text.Encoding が存在するという事を示しています。
現在の実装ではこの種類の文字コードには対応していません。ReadString_charcount の実装を見直す必要があります。";
			const int INTMAX=0x7fffffff;
			//---------------------------------------------
			skip_preamble(encoding);
			if(charcount==0)return "";

			System.Text.Decoder dec=encoding.GetDecoder();
			System.Text.StringBuilder build=new System.Text.StringBuilder();

			int capacity=charcount>INTMAX?INTMAX:(int)charcount;
			byte[] bytes=new byte[capacity];
			char[] chars=new char[capacity];
			int ibyte0;	// 読み取る予定の byte 数
			int ibyte;  // 実際に読み取った byte 数
			int ichar;  // 実際に変換して出てきた char 数
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
		/// Stream から情報を読み込みます。
		/// </summary>
		/// <param name="bytecount">復号するバイト数を指定します。</param>
		/// <param name="encoding">文字列の復号に使用する <see cref="System.Text.Encoding"/> を指定します。</param>
		/// <returns>読み込んだ文字列を <see cref="string"/> で返します。</returns>
		private string ReadString_bytecount(System.Text.Encoding encoding,uint bytecount){
			bytecount-=(uint)skip_preamble(encoding);

			const uint INTSUP=0x80000000;
			if((bytecount&INTSUP)!=0){
				System.Text.Decoder dec=encoding.GetDecoder();
				System.Text.StringBuilder build=new System.Text.StringBuilder();
				byte[] buffer=new byte[INTSUP];
				char[] chars=new char[INTSUP];

				// INTSUP - 1 文字
				int ibyte=this.stream.Read(buffer,0,unchecked((int)(INTSUP-1)));
				int ichar=dec.GetChars(buffer,0,ibyte,chars,0);
				build.Append(chars,0,ichar);
				if((uint)ibyte!=INTSUP)return build.ToString();

				// 1 文字
				ibyte=this.stream.Read(buffer,0,1);
				ichar=dec.GetChars(buffer,0,ibyte,chars,0);
				build.Append(chars,0,ichar);
				if(ibyte!=1)return build.ToString();

				// 残り
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

		#region Write:文字列
		/// <summary>
		/// 書き込み用の <see cref="System.Text.Encoding"/> を取得します。
		/// </summary>
		/// <param name="enc">書き込み用の文字コードを指定します。
		/// <include file="document.xml" path='document/desc[@name="afh.File.StreamAccessor::enc2"]/*'/>
		/// </param>
		/// <param name="terminated">
		/// これが null 終端文字列である時に true を指定します。それ以外の場合には false を指定します。
		/// </param>
		/// <param name="bom">BOM の指定がある場合に BOM を返します。</param>
		/// <returns>取得した System.Text.Encoding を返します。</returns>
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
		/// 文字列を Stream に書き込みます。
		/// </summary>
		/// <param name="value">書き込む値を指定します。</param>
		/// <param name="enc">値を書き込む方法と文字コードを指定します。
		/// <include file='document.xml' path='document/desc[@name="afh.File.StreamAccessor::enc2"]/*'/>
		/// </param>
		/// <remarks>
		/// 読み書きする文字列の形式は以下の様になります:
		/// <c>ENC? LEN? C* 0?</c>
		/// <dl>
		/// <dt>ENC</dt><dd>Charater Encoding (EncEmbedded を指定した場合 2B)</dd>
		/// <dt>LEN</dt><dd>Length (StrPascal を指定した場合 1B; StrSize か StrBasic を指定した場合 4B)</dd>
		/// <dt>C</dt><dd>文字の列</dd>
		/// <dt>0</dt><dd>終端符号 (StrTerminated を指定した場合)</dd>
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
					//if(value.Length>uint.MaxValue) // ← あり得ない
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
					if(len>uint.MaxValue) // 4GB 以上の時
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
					throw new System.ApplicationException("System.String から指定した種類への書込には対応していません。");
			}
		}
		/// <summary>
		/// 文字列を書き込みます。
		/// </summary>
		/// <param name="value">書き込む値を指定します。</param>
		/// <param name="length">書き込む文字列の長さを文字数で指定します。</param>
		public void WriteStringByCharCount(string value,int length) {
			int delta=value.Length-length;
			this.Write(
				delta>0?value.Substring(0,length) : delta<0?value+new string(' ',delta) : value,
				EncodingType.String);
		}
		private static string GetMessage_StringTooLong(string value,uint maxlen){
			const string F="引数に指定した文字列が長すぎます。\r\n    指定した文字列 \"{0}\" の長さ: {1}\r\n    最大文字数: {2}";
			return string.Format(F,value,value.Length,maxlen);
		}
		private static string GetMessage_StringTooLarge(string value,uint maxlen){
			const string F="引数に指定した文字列が長すぎます。\r\n    指定した文字列 \"{0}\" の byte 長: {1}\r\n    最大 byte 数: {2}";
			return string.Format(F,value,value.Length,maxlen);
		}
		#endregion

		//===========================================================
		//		他
		//===========================================================
		/// <summary>
		/// 指定した領域を byte[] として固定し、指定した操作を行います。
		/// </summary>
		/// <param name="index">領域の初めの位置を指定します。</param>
		/// <param name="length">領域の長さを指定します。</param>
		/// <param name="deleg">
		/// 変更を適用する場合には true を返します。
		/// 変更を行っていない場合、亦は変更を破棄する場合には false を返します。
		/// true が返された場合には、変更が行われた byte[] を対応する <see cref="System.IO.Stream"/> の領域に上書きします。
		/// 上書きする場合には <paramref name="deleg"/> に渡した readlength の長さではなくて、
		/// 初めに要求した領域の長さ <paramref name="length"/> だけ上書きされる事になります。
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
		/// スレッドセーフな StreamAccessor を提供します。
		/// 詳細に関しては <see cref="SyncStreamAccessor"/> を参照して下さい。
		/// </summary>
		/// <param name="accessor">元になる StreamAccessor を指定します。</param>
		/// <returns>
		/// SyncStreamAccessor のインスタンスを返します。
		/// </returns>
		public static SyncStreamAccessor Synchronized(StreamAccessor accessor){
			return (accessor  as SyncStreamAccessor)??new SyncStreamAccessor(accessor.stream);
		}
		/// <summary>
		/// EncodingType を文字列にします。
		/// </summary>
		/// <param name="enc">文字列にする EncodingType の値を指定します。</param>
		/// <returns>文字列にして表現された EncodingType を取得します。</returns>
		public static string EncodingTypeToString(EncodingType enc){
			string r=afh.Enum.GetDescription(enc&EncodingType.MASK_TYPE,"TYPE");
			if(!r.StartsWith("Str"))return "EncodingType."+r;
			return "EncodingType."+r
				+"|EncodingType."+afh.Enum.GetDescription(enc&EncodingType.MASK_STRING_ENCSPEC,"STRING_ENCSPEC")
				+"|EncodingType."+afh.Enum.GetDescription(enc&EncodingType.MASK_OPTION,"CHARCTER_CODE");
		}
		/// <summary>
		/// 文字列エンコーディングを EncodingType で表現します。
		/// </summary>
		/// <param name="enc">EncodingType で表現したい Encoding を指定します。</param>
		/// <returns>enc を EncodingType に変換した値を返します。</returns>
		public static EncodingType EncodingToEncodingType(System.Text.Encoding enc){
			return (EncodingType)(uint)(enc.CodePage<<16);
		}
	}

	/// <summary>
	/// (概ね) スレッドセーフな <see cref="StreamAccessor"/> です。
	/// <para>
	/// 但し、Stream の末端を超えて読み取ろうとした場合 (例外を発生させます) には、スレッドセーフではありません。
	/// 特にこの場合に他のスレッドから Seek(SeekOrigin.Current) を要求されると Seek 後の位置がずれる可能性があります。
	/// 然し、実際には「複数のスレッドから自由に Seek をかけられる様な状態での使い方がある」
	/// とは考えがたいので敢えて同期を行わない仕様になっています。
	/// </para>
	/// </summary>
	public sealed class SyncStreamAccessor:StreamAccessor{
		private System.IO.Stream unsync;
		/// <summary>
		/// StreamAccessor を初期化します。
		/// </summary>
		/// <param name="unsyncstream">情報を書き込む先の stream を指定します。</param>
		public SyncStreamAccessor(System.IO.Stream unsyncstream):base(System.IO.Stream.Synchronized(unsyncstream)){
			this.unsync=unsyncstream;
		}

		/// <summary>
		/// 指定した領域を byte[] として固定し、指定した操作を行います。
		/// </summary>
		/// <param name="index">領域の初めの位置を指定します。</param>
		/// <param name="length">領域の長さを指定します。</param>
		/// <param name="deleg">
		/// 変更を適用する場合には true を返します。
		/// 変更を行っていない場合、亦は変更を破棄する場合には false を返します。
		/// true が返された場合には、変更が行われた byte[] を対応する <see cref="System.IO.Stream"/> の領域に上書きします。
		/// 上書きする場合には <paramref name="deleg"/> に渡した readlength の長さではなくて、
		/// 初めに要求した領域の長さ <paramref name="length"/> だけ上書きされる事になります。
		/// </param>
		public unsafe override void Fix(long index,int length,PointerOperation deleg){
			if(!freeFix){
				lock(this.unsync)base.Fix(index,length,deleg);
				return;
			}

			// deleg の処理に時間がかかる & 他のスレッドから操作する頻度が高い場合にはこっち。
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
		/// Stream の内容を byte[] に移して操作をしている間に、Stream の lock を解除するかどうかを取得亦は設定します。
		/// (少なくとも Stream から byte[] へ、また、byte[] から Stream へのデータ転送時には lock されている必要があります。)
		/// <para>既定値は false です。</para>
		/// <para>
		/// <see cref="M:Fix"/> の際の処理に時間がかかり且つその間に他のスレッドから Stream を操作しようとする頻度が高い場合には true を指定します。
		/// また、<see cref="M:Fix"/> の際の処理に時間がかかり且つその間に変更を行わない場合 (つまり Fix の引数 deleg が false を返す場合) にも true を指定します。
		/// それ以外の場合には false を指定して下さい。true を指定すると lock を二回行う事になりオーバーヘッドが大きくなりますので、極力 false を指定する様にして下さい。
		/// </para>
		/// </summary>
		public bool FreeWhileFixing{
			get{return this.freeFix;}
			set{this.freeFix=value;}
		}
	}

	/// <summary>
	/// ポインタ操作を表すメソッドを表します。
	/// </summary>
	/// <param name="ptr">操作対象の情報へのポインタを指定します。</param>
	/// <param name="readlength">
	/// 実際に読み取る事が出来た長さを指定します。
	/// 要求された長さを読み取る前に Stream の終端を超えていた場合などに、
	/// 要求された長さと readlength の長さが異なる値になります。
	/// 領域の readlength 以上の index の場所も変更可能です。
	/// </param>
	/// <returns>
	/// 書込などの変更を行った場合に true を返します。
	/// 変更を行わなかった場合、亦は変更を破棄し元の状態の儘にして置くには false を返します。
	/// </returns>
	public unsafe delegate bool PointerOperation(byte* ptr,int readlength);

	/// <summary>
	/// ファイルの内容が予期される形式と異なる場合に発生させる例外です。
	/// </summary>
	[System.Serializable]
	public sealed class FileFormatException:System.Exception{
		private const string DEFAULT_MSG="ファイル／ストリームの形式が予期された物と異なります。正しい形式のデータを指定して下さい。";
		public FileFormatException():base(DEFAULT_MSG){}
		public FileFormatException(System.Exception innerException):base(DEFAULT_MSG,innerException){}
		public FileFormatException(string additional_msg):base(DEFAULT_MSG+additional_msg){}
		public FileFormatException(string additional_msg,System.Exception innerException):base(DEFAULT_MSG+additional_msg,innerException){}
	}

	/// <summary>
	/// Stream の終端を越えて読み書きの動作を使用とした場合に発生させる例外です。
	/// </summary>
	[System.Serializable]
	public sealed class StreamOverRunException:System.Exception{
		const string OVERREAD="Stream の終端を越えて読み込もうとしています";
		const string OVERWRITE="Stream の終端を越えて書き込もうとしています";
		/// <summary>
		/// StreamOverRunException のコンストラクタです。
		/// </summary>
		public StreamOverRunException():base(OVERREAD+"。"){}
		/// <summary>
		/// StreamOverRunException のコンストラクタです。
		/// </summary>
		/// <param name="message">例外に関する追加の説明を指定します。</param>
		public StreamOverRunException(string message):base(OVERREAD+": "+message){}
		/// <summary>
		/// StreamOverRunException のコンストラクタです。
		/// </summary>
		/// <param name="innerException">この例外を発生させる原因となる内部例外があった場合に、それを指定します。</param>
		public StreamOverRunException(System.Exception innerException):base(OVERREAD+"。",innerException){}
		/// <summary>
		/// StreamOverRunException のコンストラクタです。
		/// </summary>
		/// <param name="message">例外に関する追加の説明を指定します。</param>
		/// <param name="innerException">この例外を発生させる原因となる内部例外があった場合に、それを指定します。</param>
		public StreamOverRunException(string message,System.Exception innerException):base(OVERREAD+": "+message,innerException){}
	}

	/// <summary>
	/// System.IO.Stream に対する操作を提供します。
	/// </summary>
	public static class StreamUtils{
		/// <summary>
		/// System.IO.Compression.GZipStream を使用して指定した Stream を圧縮しシーク可能な MemoryStream に置き換えます。
		/// </summary>
		/// <param name="stream">圧縮の対象となる Stream を指定します。
		/// Stream を全て読み取って圧縮した結果を格納している MemoryStream を返します。
		/// 元のストリームは閉じられます。</param>
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
		/// System.IO.Compression.GZipStream を使用して指定した Stream を展開しシーク可能な MemoryStream に置き換えます。
		/// </summary>
		/// <param name="stream">展開の対象となる圧縮された Stream を指定します。
		/// Stream を全て読み取って展開した結果を格納している MemoryStream を返します。
		/// 元のストリームは閉じられます。</param>
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
		/// System.IO.Compression.DeflateStream を使用して指定した Stream を圧縮しシーク可能な MemoryStream に置き換えます。
		/// </summary>
		/// <param name="stream">圧縮の対象となる Stream を指定します。
		/// Stream を全て読み取って圧縮した結果を格納している MemoryStream を返します。
		/// 元のストリームは閉じられます。</param>
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
		/// System.IO.Compression.DeflateStream を使用して指定した Stream を展開しシーク可能な MemoryStream に置き換えます。
		/// </summary>
		/// <param name="stream">展開の対象となる圧縮された Stream を指定します。
		/// Stream を全て読み取って展開した結果を格納している MemoryStream を返します。
		/// 元のストリームは閉じられます。</param>
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