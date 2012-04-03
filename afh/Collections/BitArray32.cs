namespace afh.Collections{
	/// <summary>
	/// 下位 2B を BitArray の位置の指定に使用します。
	/// 上位 2B を BitArray の長さの指定に使用します。
	/// </summary>
	public enum BitSection:int{}

	/// <summary>
	/// 32bit の bit 配列を表現する構造体です。
	/// </summary>
	[System.Serializable]
	public struct BitArray32{
		private int val;
		/// <summary>
		/// BitArray32 の値を初期化します。
		/// </summary>
		/// <param name="value">初期化後の内容を含む int 値を指定します。</param>
		public BitArray32(int value){
			this.val=value;
		}
		/// <summary>
		/// 指定した番号の bit の値を取得又は設定します。
		/// </summary>
		/// <param name="index">取得又は設定する対象の bit の番号を指定します。</param>
		/// <returns>
		/// 値を取得する場合は、bit が 1 の時 true を返します。
		/// bit が 0 の場合に false を返します。</returns>
		public bool this[int index]{
			get{
				if(index<0||32<=index)throw new System.ArgumentOutOfRangeException("index");
				return 0!=(this.val&1<<index);
			}
			set{
				if(index<0||32<=index)throw new System.ArgumentOutOfRangeException("index");
				if(value){
					this.val|=1<<index;
				}else{
					this.val&=~(1<<index);
				}
			}
		}
		/// <summary>
		/// Bit 毎の or 演算を実行します。
		/// </summary>
		/// <param name="left">| 演算子の左辺を指定します。</param>
		/// <param name="right">| 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎論理和 (Or) 演算の結果の BitArray32 を返します。</returns>
		public static BitArray32 operator|(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val|right.val);
		}
		/// <summary>
		/// Bit 毎の and 演算を実行します。
		/// </summary>
		/// <param name="left">&amp; 演算子の左辺を指定します。</param>
		/// <param name="right">&amp; 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎論理積 (And) 演算の結果の BitArray32 を返します。</returns>
		public static BitArray32 operator&(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val&right.val);
		}
		/// <summary>
		/// Bit 毎の xor 演算を実行します。
		/// </summary>
		/// <param name="left">^ 演算子の左辺を指定します。</param>
		/// <param name="right">^ 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎排他的論理和 (Xor) 演算の結果の BitArray32 を返します。</returns>
		public static BitArray32 operator^(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val^right.val);
		}
		/// <summary>
		/// 或る程度の bit 幅を持った情報を符号無し整数として取得又は設定します。
		/// </summary>
		/// <param name="index">対象の開始位置を指定します。</param>
		/// <param name="len">対象の bit 幅を指定します。</param>
		/// <returns>値を取得する場合には、指定した位置に存在する情報を読み出して、
		/// 符号無し整数として返します。</returns>
		public uint this[int index,int len]{
			get{return (GetMask(index,len)&(uint)this.val)>>index;}
			set{
				int mask=(int)GetMask(index,len);
				this.val&=~mask;
				this.val|=mask&(int)(value<<index);
			}
		}
		private uint GetMask(int index,int len){
			if(index<0||32<=index)throw new System.ArgumentOutOfRangeException("index");
			if(len<1||32<index+len)throw new System.ArgumentOutOfRangeException("len");
			int tmp=unchecked((int)0x80000000)>>(len-1);
			return (uint)tmp>>(32-len-index);
		}

		/// <summary>
		/// 幅を持った情報を符号無し整数として取得又は設定します。
		/// </summary>
		/// <param name="sec">使用する bit の位置と幅を指定します。
		/// 下位 2B に位置を指定します。
		/// 上位 2B に幅を指定します。
		/// </param>
		/// <returns>値を取得する場合には、
		/// 指定した場所に対応する情報を符号無し整数として返します。
		/// </returns>
		public uint this[BitSection sec]{
			get{return this[(int)sec&0xffff,(int)((uint)sec>>16)];}
			set{this[(int)sec&0xffff,(int)((uint)sec>>16)]=value;}
		}
	}

	/// <summary>
	/// 64bit の bit 配列を表現する構造体です。
	/// </summary>
	[System.Serializable]
	public struct BitArray64{
		private long val;
		/// <summary>
		/// BitArray64 の値を初期化します。
		/// </summary>
		/// <param name="value">初期化後の内容を含む long 値を指定します。</param>
		public BitArray64(long value){
			this.val=value;
		}
		/// <summary>
		/// 指定した番号の bit の値を取得又は設定します。
		/// </summary>
		/// <param name="index">取得又は設定する対象の bit の番号を指定します。</param>
		/// <returns>
		/// 値を取得する場合は、bit が 1 の時 true を返します。
		/// bit が 0 の場合に false を返します。</returns>
		public bool this[int index]{
			get{
				if(index<0||64<=index)throw new System.ArgumentOutOfRangeException("index");
				return 0!=(this.val&1L<<index);
			}
			set{
				if(index<0||64<=index)throw new System.ArgumentOutOfRangeException("index");
				if(value){
					this.val|=1L<<index;
				}else{
					this.val&=~(1L<<index);
				}
			}
		}
		/// <summary>
		/// Bit 毎の or 演算を実行します。
		/// </summary>
		/// <param name="left">| 演算子の左辺を指定します。</param>
		/// <param name="right">| 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎論理和 (Or) 演算の結果の BitArray64 を返します。</returns>
		public static BitArray64 operator|(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val|right.val);
		}
		/// <summary>
		/// Bit 毎の and 演算を実行します。
		/// </summary>
		/// <param name="left">&amp; 演算子の左辺を指定します。</param>
		/// <param name="right">&amp; 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎論理積 (And) 演算の結果の BitArray64 を返します。</returns>
		public static BitArray64 operator&(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val&right.val);
		}
		/// <summary>
		/// Bit 毎の xor 演算を実行します。
		/// </summary>
		/// <param name="left">^ 演算子の左辺を指定します。</param>
		/// <param name="right">^ 演算子の右辺を指定します。</param>
		/// <returns>Bit 毎排他的論理和 (Xor) 演算の結果の BitArray64 を返します。</returns>
		public static BitArray64 operator^(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val^right.val);
		}
		/// <summary>
		/// 或る程度の bit 幅を持った情報を符号無し整数として取得又は設定します。
		/// </summary>
		/// <param name="index">対象の開始位置を指定します。</param>
		/// <param name="len">対象の bit 幅を指定します。</param>
		/// <returns>値を取得する場合には、指定した位置に存在する情報を読み出して、
		/// 符号無し整数として返します。</returns>
		public uint this[int index,int len]{
			get{
				ulong ret=(GetMask(index,len)&(ulong)this.val)>>index;
				return (uint)ret;
			}
			set{
				long mask=(long)GetMask(index,len);
				this.val&=~mask;
				this.val|=mask&((long)value<<index);
			}
		}
		private ulong GetMask(int index,int len){
			if(index<0||64<=index)throw new System.ArgumentOutOfRangeException("index");
			if(len<1||64<index+len)throw new System.ArgumentOutOfRangeException("len");
			long tmp=unchecked((long)0x8000000000000000)>>(len-1);
			return (ulong)tmp>>(64-len-index);
		}
		/// <summary>
		/// 幅を持った情報を符号無し整数として取得又は設定します。
		/// </summary>
		/// <param name="sec">使用する bit の位置と幅を指定します。
		/// 下位 2B に位置を指定します。
		/// 上位 2B に幅を指定します。
		/// </param>
		/// <returns>値を取得する場合には、
		/// 指定した場所に対応する情報を符号無し整数として返します。
		/// </returns>
		public uint this[BitSection sec]{
			get{return this[(int)sec&0xffff,(int)((uint)sec>>16)];}
			set{this[(int)sec&0xffff,(int)((uint)sec>>16)]=value;}
		}
	}
}