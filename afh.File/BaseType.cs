using Interop=System.Runtime.InteropServices;
namespace afh.File{
	public static class __dll__{
		public static afh.Application.Log log=afh.Application.LogView.Instance.CreateLog("<afh.File>");

		//public static void Initialize() {
			//new FileDataCached.CustomAccessor<UnknownFrame>();
			//new FileDataCached.CustomAccessor<TextInformationFrame>();
		//}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void ThrowFatalException(string msg) {
			throw new System.Exception("FATAL 異常な状態 (Algorithm 上不可):\r\n"+msg);
		}
	}

	#region struct:UInt16BE
	/// <summary>
	/// uint16-BE を表現する構造体です。sizeof(UInt16BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct UInt16BE{
		private byte data0;
		private byte data1;
		/// <summary>
		/// uhsort を用いて UInt16BE を初期化します。
		/// </summary>
		/// <param name="val">新しい UInt16BE の表す値を指定します。</param>
		public UInt16BE(ushort val) {
			this.data0=(byte)(val>>8);
			this.data1=(byte)val;
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt16BE を ushort に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator ushort(UInt16BE value) {
			return (ushort)((value.data0<<8)|value.data1);
		}
		/// <summary>
		/// ushort を UInt16BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator UInt16BE(ushort value) {
			return new UInt16BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static UInt16BE operator +(UInt16BE l,UInt16BE r) {
			return (UInt16BE)((ushort)l+(ushort)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator ==(UInt16BE l,UInt16BE r) {
			return l.data0==r.data0&&l.data1==r.data1;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator !=(UInt16BE l,UInt16BE r) {return !(l==r); }
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が UInt16BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj){
			if(obj is UInt16BE) {
				return this==(UInt16BE)obj;
			}else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode() { return ((ushort)this).GetHashCode(); }
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString() { return ((ushort)this).ToString(); }
	}
	#endregion

	#region struct:Int16BE
	/// <summary>
	/// int16-BE を表現する構造体です。sizeof(Int16BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Int16BE{
		private byte data0;
		private byte data1;
		/// <summary>
		/// short を用いて Int16BE を初期化します。
		/// </summary>
		/// <param name="val">新しい Int16BE の表す値を指定します。</param>
		public Int16BE(short val) {
			this.data0=(byte)(val>>8);
			this.data1=(byte)val;
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// Int16BE を short に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator short(Int16BE value) {
			return (short)((value.data0<<8)|value.data1);
		}
		/// <summary>
		/// short を Int16BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator Int16BE(short value) {
			return new Int16BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static Int16BE operator +(Int16BE l,Int16BE r) {
			return (Int16BE)((short)l+(short)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator ==(Int16BE l,Int16BE r) {
			return l.data0==r.data0&&l.data1==r.data1;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator !=(Int16BE l,Int16BE r) {return !(l==r); }
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が Int16BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj){
			if(obj is Int16BE) {
				return this==(Int16BE)obj;
			}else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode() { return ((short)this).GetHashCode(); }
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString() { return ((short)this).ToString(); }
	}
	#endregion

	#region struct:UInt32BE
	/// <summary>
	/// uint32-BE を表現する構造体です。sizeof(UInt32BE)==4
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct UInt32BE{
		//[Interop::MarshalAs(Interop::UnmanagedType.ByValArray,SizeConst=4)]
		//private byte[] data;
		private byte data0;
		private byte data1;
		private byte data2;
		private byte data3;
		/// <summary>
		/// uint を用いて UInt32BE を初期化します。
		/// </summary>
		/// <param name="val">新しい UInt32BE の表す値を指定します。</param>
		public UInt32BE(uint val) {
			//this.data=new byte[4];
			this.data0=(byte)(val>>24);
			this.data1=(byte)(val>>16);
			this.data2=(byte)(val>>8);
			this.data3=(byte)val;
		}
		/// <summary>
		/// ASCII 文字四つのコードに変換した値を取得します。
		/// </summary>
		/// <returns>四文字の文字列を返します。</returns>
		public string GetFourCC(){
			return System.Text.Encoding.ASCII.GetString(new byte[]{this.data0,this.data1,this.data2,this.data3});
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt32BE を uint に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator uint(UInt32BE value){
			return (uint)(value.data3|(value.data2<<8)|(value.data1<<16)|(value.data0<<24));
		}
		/// <summary>
		/// uint を UInt32BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator UInt32BE(uint value){
			return new UInt32BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static UInt32BE operator +(UInt32BE l,UInt32BE r){
			return (UInt32BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator ==(UInt32BE l,UInt32BE r){
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator !=(UInt32BE l,UInt32BE r){return !(l==r);}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が UInt32BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj){
			if(obj is UInt32BE){
				return this==(UInt32BE)obj;
			}else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode(){return ((uint)this).GetHashCode();}
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString(){return ((uint)this).ToString();}
	}
	#endregion

	#region struct:Int32BE
	/// <summary>
	/// int32-BE を表現する構造体です。sizeof(Int32BE)==4。
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Int32BE{
		//[Interop::MarshalAs(Interop::UnmanagedType.ByValArray,SizeConst=4)]
		//private byte[] data;
		private byte data0;
		private byte data1;
		private byte data2;
		private byte data3;
		/// <summary>
		/// int を用いて Int32BE を初期化します。
		/// </summary>
		/// <param name="val">新しい Int32BE の表す値を指定します。</param>
		public Int32BE(int val) {
			//this.data=new byte[4];
			this.data0=(byte)(val>>24);
			this.data1=(byte)(val>>16);
			this.data2=(byte)(val>>8);
			this.data3=(byte)val;
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// Int32BE を int に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator int(Int32BE value){
			return value.data3|(value.data2<<8)|(value.data1<<16)|(value.data0<<24);
		}
		/// <summary>
		/// int を Int32BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator Int32BE(int value){
			return new Int32BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static Int32BE operator +(Int32BE l,Int32BE r){
			return (Int32BE)((int)l+(int)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator ==(Int32BE l,Int32BE r){
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator !=(Int32BE l,Int32BE r){return !(l==r);}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が Int32BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj){
			if(obj is Int32BE){
				return this==(Int32BE)obj;
			}else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode(){return ((int)this).GetHashCode();}
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString(){return ((int)this).ToString();}
	}
	#endregion

	#region struct:UInt24BE
	/// <summary>
	/// int28-BE を表現する構造体です。sizeof(UInt24BE)==3。
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct UInt24BE {
		//[Interop::MarshalAs(Interop::UnmanagedType.ByValArray,SizeConst=4)]
		//private byte[] data;
		private byte data0;
		private byte data1;
		private byte data2;
		/// <summary>
		/// int を用いて UInt24BE を初期化します。
		/// </summary>
		/// <param name="val">新しい UInt24BE の表す値を指定します。</param>
		public UInt24BE(uint val) {
			//this.data=new byte[4];
			if(val>>24!=0) throw new System.OverflowException("指定した整数値は UInt24BE で表現するには大きすぎます。UInt24BE を初期化できません。");
			this.data0=(byte)(val>>16);
			this.data1=(byte)(val>>8);
			this.data2=(byte)val;
		}
		/// <summary>
		/// UInt24BE で表現し得る最大の値を返します。
		/// </summary>
		public UInt24BE MaxValue {
			get {
				UInt24BE r=new UInt24BE();
				r.data0=r.data1=r.data2=0xff;
				return r;
			}
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt24BE を int に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator uint(UInt24BE value) {
			return (uint)(value.data2|value.data1<<8|value.data0<<16);
		}
		/// <summary>
		/// uint を UInt24BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator UInt24BE(uint value) {
			return new UInt24BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static UInt24BE operator+(UInt24BE l,UInt24BE r) {
			return (UInt24BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator==(UInt24BE l,UInt24BE r) {
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator!=(UInt24BE l,UInt24BE r) { return !(l==r); }
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が UInt24BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj) {
			if(obj is UInt24BE) {
				return this==(UInt24BE)obj;
			} else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode() { return ((uint)this).GetHashCode(); }
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString() { return ((uint)this).ToString(); }
	}
	#endregion

	#region struct:UInt28BE
	/// <summary>
	/// int28-BE を表現する構造体です。sizeof(UInt28BE)==4。
	/// 各バイトの下位 7bit を使用して整数を表現します。上位 1bit は常に 0 を指定します。
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct UInt28BE{
		//[Interop::MarshalAs(Interop::UnmanagedType.ByValArray,SizeConst=4)]
		//private byte[] data;
		private byte data0;
		private byte data1;
		private byte data2;
		private byte data3;
		/// <summary>
		/// int を用いて UInt28BE を初期化します。
		/// </summary>
		/// <param name="val">新しい UInt28BE の表す値を指定します。</param>
		public UInt28BE(uint val) {
			//this.data=new byte[4];
			if(val>>28!=0)throw new System.OverflowException("指定した整数値は UInt28BE で表現するには大きすぎます。UInt28BE を初期化できません。");
			this.data0=(byte)(val>>21);
			this.data1=(byte)(val>>14);
			this.data2=(byte)(val>>7);
			this.data3=(byte)val;
			this.Normalize();
		}
		/// <summary>
		/// 各バイトの上位 1bit を 0 に設定します。
		/// </summary>
		private void Normalize(){
			this.data0&=0x7f;
			this.data1&=0x7f;
			this.data2&=0x7f;
			this.data3&=0x7f;
		}
		/// <summary>
		/// UInt28BE で表現し得る最大の値を返します。
		/// </summary>
		public UInt28BE MaxValue{
			get{
				UInt28BE r=new UInt28BE();
				r.data0=r.data1=r.data2=r.data3=0x7f;
				return r;
			}
		}
		/// <summary>
		/// この UInt28BE が標準形式であるかどうかを取得します。
		/// </summary>
		public bool Normalized{
			get{return ((this.data0|this.data1|this.data2|this.data3)&0x80)!=0;}
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt28BE を int に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator int(UInt28BE value) {
			value.Normalize();
			return (int)(value.data3|value.data2<<7|value.data1<<14|value.data0<<21);
		}
		/// <summary>
		/// UInt28BE を uint に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator uint(UInt28BE value) {
			value.Normalize();
			return (uint)(value.data3|value.data2<<7|value.data1<<14|value.data0<<21);
		}
		/// <summary>
		/// uint を UInt28BE に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator UInt28BE(uint value) {
			return new UInt28BE(value);
		}
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="l">被加数を指定します。</param>
		/// <param name="r">加数を指定します。</param>
		/// <returns>和を返します。</returns>
		public static UInt28BE operator+(UInt28BE l,UInt28BE r) {
			return (UInt28BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が同じ値である場合に true を返します。</returns>
		public static bool operator==(UInt28BE l,UInt28BE r) {
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// 不等を判定します。
		/// </summary>
		/// <param name="l">一つ目の値を指定します。</param>
		/// <param name="r">二つ目の値を指定します。</param>
		/// <returns>両者が異なる値である場合に true を返します。</returns>
		public static bool operator!=(UInt28BE l,UInt28BE r) { return !(l==r); }
		/// <summary>
		/// 等値を判定します。
		/// </summary>
		/// <param name="obj">比較対象の値を指定します。</param>
		/// <returns>指定された値が UInt28BE で且つ当構造体と同じ値を表している場合に true を返します。</returns>
		public override bool Equals(object obj) {
			if(obj is UInt28BE) {
				return this==(UInt28BE)obj;
			} else return false;
		}
		/// <summary>
		/// この構造体のハッシュ値を求めます。
		/// </summary>
		/// <returns>求めたハッシュ値を返します。</returns>
		public override int GetHashCode() { return ((uint)this).GetHashCode(); }
		/// <summary>
		/// この構造体の表す値を文字列に変換します。
		/// </summary>
		/// <returns>この構造体の表す値を文字列として返します。</returns>
		public override string ToString() { return ((uint)this).ToString(); }
	}
	#endregion

	#region struct:Fixed16d16BE
	/// <summary>
	/// fixed16.16-BE を表現する構造体です。sizeof(Fixed16d16BE)==4
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Fixed16d16BE{
		private Int32BE value;
		/// <summary>
		/// 整数部を表現する整数値を取得します。
		/// </summary>
		public short Mantissa{
			get{return (short)((int)this.value>>16);}
		}
		/// <summary>
		/// 小数部を表現する整数値を取得します。
		/// </summary>
		public ushort Fraction{
			get{return (ushort)this.value;}
		}
		/// <summary>
		/// この構造体の表す値を double で取得亦は設定します。
		/// </summary>
		public double Value{
			get{return (int)this.value/(double)0x10000;}
			set{
				if(value>=short.MaxValue+1||value<short.MinValue)
					throw new System.ArgumentOutOfRangeException("value");
				this.value=(Int32BE)(int)(value*0x10000);
			}
		}
		/// <summary>
		/// double を用いて Fixed16d16BE を初期化します。
		/// </summary>
		/// <param name="val">新しい Fixed16d16BE の値を指定します。</param>
		public Fixed16d16BE(double val){
			if(val>=short.MaxValue+1||val<short.MinValue)
				throw new System.ArgumentOutOfRangeException("val");
			this.value=(Int32BE)(int)(val*0x10000);
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// 整数型に格納されたデータを固定小数点 Fixed16d16BE に変換します。
		/// </summary>
		/// <param name="val">固定小数点情報を格納してある整数値を指定します。</param>
		/// <returns>固定小数点型を返します。</returns>
		public static explicit operator Fixed16d16BE(int val){
			Fixed16d16BE r=new Fixed16d16BE();
			r.value=(Int32BE)val;
			return r;
		}
		/// <summary>
		/// 固定小数点値を表すデータを整数値として表現した物に変換します。
		/// </summary>
		/// <param name="val">固定小数点値を指定します。</param>
		/// <returns>固定小数点値を表現している整数値を返します。</returns>
		public static explicit operator int(Fixed16d16BE val){return (int)val.value;}
		/// <summary>
		/// double を Fixed16d16BE に変換します。
		/// </summary>
		/// <param name="val">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator Fixed16d16BE(double val){return new Fixed16d16BE(val);}
		/// <summary>
		/// Fixed16d16BE を double に変換します。
		/// </summary>
		/// <param name="val">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator double(Fixed16d16BE val){return val.Value;}
		/// <summary>
		/// この構造体の表す値を文字列で表現します。
		/// </summary>
		/// <returns>文字列で表現されたこの構造体の値を返します。</returns>
		public override string ToString(){
			return this.Value.ToString();
		}
	}
	#endregion

	#region struct:Fixed2d14BE
	/// <summary>
	/// fixed2.14-BE を表現する構造体です。sizeof(Fixed2d14BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Fixed2d14BE{
		private Int16BE value;
		/// <summary>
		/// 実数部を表す整数値を取得します。
		/// </summary>
		public short Mantissa{
			get{return (short)((short)this.value>>14);}
		}
		/// <summary>
		/// 小数部を表現する整数値を取得します。
		/// </summary>
		public short Fraction{
			get{return (short)((short)this.value&0x3fff);}
		}
		/// <summary>
		/// 小数値を double として取得します。
		/// </summary>
		public double Value{
			get{return (short)this.value/(double)0x4000;}
			set{
				if(value>=short.MaxValue+1||value<short.MinValue)
					throw new System.ArgumentOutOfRangeException("value");
				this.value=(Int16BE)(short)(value*0x4000);
			}
		}
		/// <summary>
		/// double を用いて Fixed2d14BE を初期化します。
		/// </summary>
		/// <param name="val">新しい Fixed2d14BE の表す値を指定します。</param>
		public Fixed2d14BE(double val){
			if(val>=short.MaxValue+1||val<short.MinValue)
				throw new System.ArgumentOutOfRangeException("val");
			this.value=(Int16BE)(short)(val*0x4000);
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// double を Fixed2d14BE に変換します。
		/// </summary>
		/// <param name="val">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator Fixed2d14BE(double val){return new Fixed2d14BE(val);}
		/// <summary>
		/// Fixed2d14BE を double に変換します。
		/// </summary>
		/// <param name="val">変換元の値を指定します。</param>
		/// <returns>変換後の値を返します。</returns>
		public static explicit operator double(Fixed2d14BE val){return val.Value;}
		/// <summary>
		/// この構造体の表す値を文字列で表現します。
		/// </summary>
		/// <returns>文字列で表現されたこの構造体の値を返します。</returns>
		public override string ToString(){
			return this.Value.ToString();
		}
	}
	#endregion

	#region struct:CharCode4
	/// <summary>
	/// ASCII 文字四文字コードを表現する構造体です。FourCC 等に対応します。sizeof(CharCode4)==4
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct CharCode4{
		private byte data0;
		private byte data1;
		private byte data2;
		private byte data3;
		/// <summary>
		/// 長さ 4 の byte[] を取得亦は設定します。
		/// </summary>
		private byte[] Data{
			get{return new byte[]{data0,data1,data2,data3};}
			set{
				this.data0=value[0];
				this.data1=value[1];
				this.data2=value[2];
				this.data3=value[3];
			}
		}
		/// <summary>
		/// CharCode4 を string に変換します。
		/// </summary>
		/// <param name="fourcc">変換元の値を指定します。</param>
		/// <returns>変換後の値を指定します。</returns>
		public static explicit operator string(CharCode4 fourcc){
			return System.Text.Encoding.ASCII.GetString(fourcc.Data);
		}
		/// <summary>
		/// string を CharCode4 に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を指定します。</returns>
		public static explicit operator CharCode4(string value){
			//-- 長さを 4 に揃える
			if(value.Length>4)value=value.Substring(0,4);else while(value.Length<4)value+=" ";

			CharCode4 r=new CharCode4();
			r.Data=System.Text.Encoding.ASCII.GetBytes(value);
			return r;
		}
		/// <summary>
		/// この構造体の値を文字列値として取得します。
		/// </summary>
		/// <returns>この構造体の値を返します。</returns>
		public override string ToString(){return (string)this;}
	}
	#endregion

	#region struct:CharCode3
	/// <summary>
	/// ASCII 文字三文字コードを表現する構造体です。sizeof(CharCode3)==3
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct CharCode3{
		private byte data0;
		private byte data1;
		private byte data2;
		/// <summary>
		/// 長さ 3 の byte[] を取得亦は設定します。
		/// </summary>
		private byte[] Data {
			get { return new byte[] {data0,data1,data2}; }
			set {
				this.data0=value[0];
				this.data1=value[1];
				this.data2=value[2];
			}
		}
		/// <summary>
		/// CharCode3 を string に変換します。
		/// </summary>
		/// <param name="tcc">変換元の値を指定します。</param>
		/// <returns>変換後の値を指定します。</returns>
		public static explicit operator string(CharCode3 tcc) {
			return System.Text.Encoding.ASCII.GetString(tcc.Data);
		}
		/// <summary>
		/// string を CharCode3 に変換します。
		/// </summary>
		/// <param name="value">変換元の値を指定します。</param>
		/// <returns>変換後の値を指定します。</returns>
		public static explicit operator CharCode3(string value){
			//-- 長さを 3 に揃える
			if(value.Length>3) value=value.Substring(0,3); else while(value.Length<3) value+=" ";

			CharCode3 r=new CharCode3();
			r.Data=System.Text.Encoding.ASCII.GetBytes(value);
			return r;
		}
		/// <summary>
		/// この構造体の値を文字列値として取得します。
		/// </summary>
		/// <returns>この構造体の値を返します。</returns>
		public override string ToString() { return (string)this; }
	}
	#endregion
}