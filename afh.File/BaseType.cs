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
			throw new System.Exception("FATAL �ُ�ȏ�� (Algorithm ��s��):\r\n"+msg);
		}
	}

	#region struct:UInt16BE
	/// <summary>
	/// uint16-BE ��\������\���̂ł��Bsizeof(UInt16BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct UInt16BE{
		private byte data0;
		private byte data1;
		/// <summary>
		/// uhsort ��p���� UInt16BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� UInt16BE �̕\���l���w�肵�܂��B</param>
		public UInt16BE(ushort val) {
			this.data0=(byte)(val>>8);
			this.data1=(byte)val;
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt16BE �� ushort �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator ushort(UInt16BE value) {
			return (ushort)((value.data0<<8)|value.data1);
		}
		/// <summary>
		/// ushort �� UInt16BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator UInt16BE(ushort value) {
			return new UInt16BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static UInt16BE operator +(UInt16BE l,UInt16BE r) {
			return (UInt16BE)((ushort)l+(ushort)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator ==(UInt16BE l,UInt16BE r) {
			return l.data0==r.data0&&l.data1==r.data1;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator !=(UInt16BE l,UInt16BE r) {return !(l==r); }
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� UInt16BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj){
			if(obj is UInt16BE) {
				return this==(UInt16BE)obj;
			}else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode() { return ((ushort)this).GetHashCode(); }
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString() { return ((ushort)this).ToString(); }
	}
	#endregion

	#region struct:Int16BE
	/// <summary>
	/// int16-BE ��\������\���̂ł��Bsizeof(Int16BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Int16BE{
		private byte data0;
		private byte data1;
		/// <summary>
		/// short ��p���� Int16BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� Int16BE �̕\���l���w�肵�܂��B</param>
		public Int16BE(short val) {
			this.data0=(byte)(val>>8);
			this.data1=(byte)val;
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// Int16BE �� short �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator short(Int16BE value) {
			return (short)((value.data0<<8)|value.data1);
		}
		/// <summary>
		/// short �� Int16BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator Int16BE(short value) {
			return new Int16BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static Int16BE operator +(Int16BE l,Int16BE r) {
			return (Int16BE)((short)l+(short)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator ==(Int16BE l,Int16BE r) {
			return l.data0==r.data0&&l.data1==r.data1;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator !=(Int16BE l,Int16BE r) {return !(l==r); }
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� Int16BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj){
			if(obj is Int16BE) {
				return this==(Int16BE)obj;
			}else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode() { return ((short)this).GetHashCode(); }
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString() { return ((short)this).ToString(); }
	}
	#endregion

	#region struct:UInt32BE
	/// <summary>
	/// uint32-BE ��\������\���̂ł��Bsizeof(UInt32BE)==4
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
		/// uint ��p���� UInt32BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� UInt32BE �̕\���l���w�肵�܂��B</param>
		public UInt32BE(uint val) {
			//this.data=new byte[4];
			this.data0=(byte)(val>>24);
			this.data1=(byte)(val>>16);
			this.data2=(byte)(val>>8);
			this.data3=(byte)val;
		}
		/// <summary>
		/// ASCII �����l�̃R�[�h�ɕϊ������l���擾���܂��B
		/// </summary>
		/// <returns>�l�����̕������Ԃ��܂��B</returns>
		public string GetFourCC(){
			return System.Text.Encoding.ASCII.GetString(new byte[]{this.data0,this.data1,this.data2,this.data3});
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt32BE �� uint �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator uint(UInt32BE value){
			return (uint)(value.data3|(value.data2<<8)|(value.data1<<16)|(value.data0<<24));
		}
		/// <summary>
		/// uint �� UInt32BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator UInt32BE(uint value){
			return new UInt32BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static UInt32BE operator +(UInt32BE l,UInt32BE r){
			return (UInt32BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator ==(UInt32BE l,UInt32BE r){
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator !=(UInt32BE l,UInt32BE r){return !(l==r);}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� UInt32BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj){
			if(obj is UInt32BE){
				return this==(UInt32BE)obj;
			}else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode(){return ((uint)this).GetHashCode();}
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString(){return ((uint)this).ToString();}
	}
	#endregion

	#region struct:Int32BE
	/// <summary>
	/// int32-BE ��\������\���̂ł��Bsizeof(Int32BE)==4�B
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
		/// int ��p���� Int32BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� Int32BE �̕\���l���w�肵�܂��B</param>
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
		/// Int32BE �� int �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator int(Int32BE value){
			return value.data3|(value.data2<<8)|(value.data1<<16)|(value.data0<<24);
		}
		/// <summary>
		/// int �� Int32BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator Int32BE(int value){
			return new Int32BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static Int32BE operator +(Int32BE l,Int32BE r){
			return (Int32BE)((int)l+(int)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator ==(Int32BE l,Int32BE r){
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator !=(Int32BE l,Int32BE r){return !(l==r);}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� Int32BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj){
			if(obj is Int32BE){
				return this==(Int32BE)obj;
			}else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode(){return ((int)this).GetHashCode();}
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString(){return ((int)this).ToString();}
	}
	#endregion

	#region struct:UInt24BE
	/// <summary>
	/// int28-BE ��\������\���̂ł��Bsizeof(UInt24BE)==3�B
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
		/// int ��p���� UInt24BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� UInt24BE �̕\���l���w�肵�܂��B</param>
		public UInt24BE(uint val) {
			//this.data=new byte[4];
			if(val>>24!=0) throw new System.OverflowException("�w�肵�������l�� UInt24BE �ŕ\������ɂ͑傫�����܂��BUInt24BE ���������ł��܂���B");
			this.data0=(byte)(val>>16);
			this.data1=(byte)(val>>8);
			this.data2=(byte)val;
		}
		/// <summary>
		/// UInt24BE �ŕ\��������ő�̒l��Ԃ��܂��B
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
		/// UInt24BE �� int �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator uint(UInt24BE value) {
			return (uint)(value.data2|value.data1<<8|value.data0<<16);
		}
		/// <summary>
		/// uint �� UInt24BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator UInt24BE(uint value) {
			return new UInt24BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static UInt24BE operator+(UInt24BE l,UInt24BE r) {
			return (UInt24BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator==(UInt24BE l,UInt24BE r) {
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator!=(UInt24BE l,UInt24BE r) { return !(l==r); }
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� UInt24BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj) {
			if(obj is UInt24BE) {
				return this==(UInt24BE)obj;
			} else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode() { return ((uint)this).GetHashCode(); }
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString() { return ((uint)this).ToString(); }
	}
	#endregion

	#region struct:UInt28BE
	/// <summary>
	/// int28-BE ��\������\���̂ł��Bsizeof(UInt28BE)==4�B
	/// �e�o�C�g�̉��� 7bit ���g�p���Đ�����\�����܂��B��� 1bit �͏�� 0 ���w�肵�܂��B
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
		/// int ��p���� UInt28BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� UInt28BE �̕\���l���w�肵�܂��B</param>
		public UInt28BE(uint val) {
			//this.data=new byte[4];
			if(val>>28!=0)throw new System.OverflowException("�w�肵�������l�� UInt28BE �ŕ\������ɂ͑傫�����܂��BUInt28BE ���������ł��܂���B");
			this.data0=(byte)(val>>21);
			this.data1=(byte)(val>>14);
			this.data2=(byte)(val>>7);
			this.data3=(byte)val;
			this.Normalize();
		}
		/// <summary>
		/// �e�o�C�g�̏�� 1bit �� 0 �ɐݒ肵�܂��B
		/// </summary>
		private void Normalize(){
			this.data0&=0x7f;
			this.data1&=0x7f;
			this.data2&=0x7f;
			this.data3&=0x7f;
		}
		/// <summary>
		/// UInt28BE �ŕ\��������ő�̒l��Ԃ��܂��B
		/// </summary>
		public UInt28BE MaxValue{
			get{
				UInt28BE r=new UInt28BE();
				r.data0=r.data1=r.data2=r.data3=0x7f;
				return r;
			}
		}
		/// <summary>
		/// ���� UInt28BE ���W���`���ł��邩�ǂ������擾���܂��B
		/// </summary>
		public bool Normalized{
			get{return ((this.data0|this.data1|this.data2|this.data3)&0x80)!=0;}
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// UInt28BE �� int �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator int(UInt28BE value) {
			value.Normalize();
			return (int)(value.data3|value.data2<<7|value.data1<<14|value.data0<<21);
		}
		/// <summary>
		/// UInt28BE �� uint �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator uint(UInt28BE value) {
			value.Normalize();
			return (uint)(value.data3|value.data2<<7|value.data1<<14|value.data0<<21);
		}
		/// <summary>
		/// uint �� UInt28BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator UInt28BE(uint value) {
			return new UInt28BE(value);
		}
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="l">��������w�肵�܂��B</param>
		/// <param name="r">�������w�肵�܂��B</param>
		/// <returns>�a��Ԃ��܂��B</returns>
		public static UInt28BE operator+(UInt28BE l,UInt28BE r) {
			return (UInt28BE)((uint)l+(uint)r);
		}
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂������l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator==(UInt28BE l,UInt28BE r) {
			return l.data0==r.data0&&l.data1==r.data1&&l.data2==r.data2&&l.data3==r.data3;
		}
		/// <summary>
		/// �s���𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��ڂ̒l���w�肵�܂��B</param>
		/// <param name="r">��ڂ̒l���w�肵�܂��B</param>
		/// <returns>���҂��قȂ�l�ł���ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool operator!=(UInt28BE l,UInt28BE r) { return !(l==r); }
		/// <summary>
		/// ���l�𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̒l���w�肵�܂��B</param>
		/// <returns>�w�肳�ꂽ�l�� UInt28BE �Ŋ����\���̂Ɠ����l��\���Ă���ꍇ�� true ��Ԃ��܂��B</returns>
		public override bool Equals(object obj) {
			if(obj is UInt28BE) {
				return this==(UInt28BE)obj;
			} else return false;
		}
		/// <summary>
		/// ���̍\���̂̃n�b�V���l�����߂܂��B
		/// </summary>
		/// <returns>���߂��n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode() { return ((uint)this).GetHashCode(); }
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <returns>���̍\���̂̕\���l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public override string ToString() { return ((uint)this).ToString(); }
	}
	#endregion

	#region struct:Fixed16d16BE
	/// <summary>
	/// fixed16.16-BE ��\������\���̂ł��Bsizeof(Fixed16d16BE)==4
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Fixed16d16BE{
		private Int32BE value;
		/// <summary>
		/// ��������\�����鐮���l���擾���܂��B
		/// </summary>
		public short Mantissa{
			get{return (short)((int)this.value>>16);}
		}
		/// <summary>
		/// ��������\�����鐮���l���擾���܂��B
		/// </summary>
		public ushort Fraction{
			get{return (ushort)this.value;}
		}
		/// <summary>
		/// ���̍\���̂̕\���l�� double �Ŏ擾���͐ݒ肵�܂��B
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
		/// double ��p���� Fixed16d16BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� Fixed16d16BE �̒l���w�肵�܂��B</param>
		public Fixed16d16BE(double val){
			if(val>=short.MaxValue+1||val<short.MinValue)
				throw new System.ArgumentOutOfRangeException("val");
			this.value=(Int32BE)(int)(val*0x10000);
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// �����^�Ɋi�[���ꂽ�f�[�^���Œ菬���_ Fixed16d16BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�Œ菬���_�����i�[���Ă��鐮���l���w�肵�܂��B</param>
		/// <returns>�Œ菬���_�^��Ԃ��܂��B</returns>
		public static explicit operator Fixed16d16BE(int val){
			Fixed16d16BE r=new Fixed16d16BE();
			r.value=(Int32BE)val;
			return r;
		}
		/// <summary>
		/// �Œ菬���_�l��\���f�[�^�𐮐��l�Ƃ��ĕ\���������ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�Œ菬���_�l���w�肵�܂��B</param>
		/// <returns>�Œ菬���_�l��\�����Ă��鐮���l��Ԃ��܂��B</returns>
		public static explicit operator int(Fixed16d16BE val){return (int)val.value;}
		/// <summary>
		/// double �� Fixed16d16BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator Fixed16d16BE(double val){return new Fixed16d16BE(val);}
		/// <summary>
		/// Fixed16d16BE �� double �ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator double(Fixed16d16BE val){return val.Value;}
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ŕ\�����܂��B
		/// </summary>
		/// <returns>������ŕ\�����ꂽ���̍\���̂̒l��Ԃ��܂��B</returns>
		public override string ToString(){
			return this.Value.ToString();
		}
	}
	#endregion

	#region struct:Fixed2d14BE
	/// <summary>
	/// fixed2.14-BE ��\������\���̂ł��Bsizeof(Fixed2d14BE)==2
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct Fixed2d14BE{
		private Int16BE value;
		/// <summary>
		/// ��������\�������l���擾���܂��B
		/// </summary>
		public short Mantissa{
			get{return (short)((short)this.value>>14);}
		}
		/// <summary>
		/// ��������\�����鐮���l���擾���܂��B
		/// </summary>
		public short Fraction{
			get{return (short)((short)this.value&0x3fff);}
		}
		/// <summary>
		/// �����l�� double �Ƃ��Ď擾���܂��B
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
		/// double ��p���� Fixed2d14BE �����������܂��B
		/// </summary>
		/// <param name="val">�V���� Fixed2d14BE �̕\���l���w�肵�܂��B</param>
		public Fixed2d14BE(double val){
			if(val>=short.MaxValue+1||val<short.MinValue)
				throw new System.ArgumentOutOfRangeException("val");
			this.value=(Int16BE)(short)(val*0x4000);
		}
		//=================================================
		//		Operators
		//=================================================
		/// <summary>
		/// double �� Fixed2d14BE �ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator Fixed2d14BE(double val){return new Fixed2d14BE(val);}
		/// <summary>
		/// Fixed2d14BE �� double �ɕϊ����܂��B
		/// </summary>
		/// <param name="val">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static explicit operator double(Fixed2d14BE val){return val.Value;}
		/// <summary>
		/// ���̍\���̂̕\���l�𕶎���ŕ\�����܂��B
		/// </summary>
		/// <returns>������ŕ\�����ꂽ���̍\���̂̒l��Ԃ��܂��B</returns>
		public override string ToString(){
			return this.Value.ToString();
		}
	}
	#endregion

	#region struct:CharCode4
	/// <summary>
	/// ASCII �����l�����R�[�h��\������\���̂ł��BFourCC ���ɑΉ����܂��Bsizeof(CharCode4)==4
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct CharCode4{
		private byte data0;
		private byte data1;
		private byte data2;
		private byte data3;
		/// <summary>
		/// ���� 4 �� byte[] ���擾���͐ݒ肵�܂��B
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
		/// CharCode4 �� string �ɕϊ����܂��B
		/// </summary>
		/// <param name="fourcc">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l���w�肵�܂��B</returns>
		public static explicit operator string(CharCode4 fourcc){
			return System.Text.Encoding.ASCII.GetString(fourcc.Data);
		}
		/// <summary>
		/// string �� CharCode4 �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l���w�肵�܂��B</returns>
		public static explicit operator CharCode4(string value){
			//-- ������ 4 �ɑ�����
			if(value.Length>4)value=value.Substring(0,4);else while(value.Length<4)value+=" ";

			CharCode4 r=new CharCode4();
			r.Data=System.Text.Encoding.ASCII.GetBytes(value);
			return r;
		}
		/// <summary>
		/// ���̍\���̂̒l�𕶎���l�Ƃ��Ď擾���܂��B
		/// </summary>
		/// <returns>���̍\���̂̒l��Ԃ��܂��B</returns>
		public override string ToString(){return (string)this;}
	}
	#endregion

	#region struct:CharCode3
	/// <summary>
	/// ASCII �����O�����R�[�h��\������\���̂ł��Bsizeof(CharCode3)==3
	/// </summary>
	[System.Serializable]
	[Interop::StructLayout(Interop.LayoutKind.Sequential)]
	public struct CharCode3{
		private byte data0;
		private byte data1;
		private byte data2;
		/// <summary>
		/// ���� 3 �� byte[] ���擾���͐ݒ肵�܂��B
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
		/// CharCode3 �� string �ɕϊ����܂��B
		/// </summary>
		/// <param name="tcc">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l���w�肵�܂��B</returns>
		public static explicit operator string(CharCode3 tcc) {
			return System.Text.Encoding.ASCII.GetString(tcc.Data);
		}
		/// <summary>
		/// string �� CharCode3 �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̒l���w�肵�܂��B</param>
		/// <returns>�ϊ���̒l���w�肵�܂��B</returns>
		public static explicit operator CharCode3(string value){
			//-- ������ 3 �ɑ�����
			if(value.Length>3) value=value.Substring(0,3); else while(value.Length<3) value+=" ";

			CharCode3 r=new CharCode3();
			r.Data=System.Text.Encoding.ASCII.GetBytes(value);
			return r;
		}
		/// <summary>
		/// ���̍\���̂̒l�𕶎���l�Ƃ��Ď擾���܂��B
		/// </summary>
		/// <returns>���̍\���̂̒l��Ԃ��܂��B</returns>
		public override string ToString() { return (string)this; }
	}
	#endregion
}