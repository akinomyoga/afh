namespace afh.Collections{
	/// <summary>
	/// ���� 2B �� BitArray �̈ʒu�̎w��Ɏg�p���܂��B
	/// ��� 2B �� BitArray �̒����̎w��Ɏg�p���܂��B
	/// </summary>
	public enum BitSection:int{}

	/// <summary>
	/// 32bit �� bit �z���\������\���̂ł��B
	/// </summary>
	[System.Serializable]
	public struct BitArray32{
		private int val;
		/// <summary>
		/// BitArray32 �̒l�����������܂��B
		/// </summary>
		/// <param name="value">��������̓��e���܂� int �l���w�肵�܂��B</param>
		public BitArray32(int value){
			this.val=value;
		}
		/// <summary>
		/// �w�肵���ԍ��� bit �̒l���擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="index">�擾���͐ݒ肷��Ώۂ� bit �̔ԍ����w�肵�܂��B</param>
		/// <returns>
		/// �l���擾����ꍇ�́Abit �� 1 �̎� true ��Ԃ��܂��B
		/// bit �� 0 �̏ꍇ�� false ��Ԃ��܂��B</returns>
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
		/// Bit ���� or ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">| ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">| ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���_���a (Or) ���Z�̌��ʂ� BitArray32 ��Ԃ��܂��B</returns>
		public static BitArray32 operator|(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val|right.val);
		}
		/// <summary>
		/// Bit ���� and ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">&amp; ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">&amp; ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���_���� (And) ���Z�̌��ʂ� BitArray32 ��Ԃ��܂��B</returns>
		public static BitArray32 operator&(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val&right.val);
		}
		/// <summary>
		/// Bit ���� xor ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">^ ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">^ ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���r���I�_���a (Xor) ���Z�̌��ʂ� BitArray32 ��Ԃ��܂��B</returns>
		public static BitArray32 operator^(BitArray32 left,BitArray32 right){
			return new BitArray32(left.val^right.val);
		}
		/// <summary>
		/// ������x�� bit �������������𕄍����������Ƃ��Ď擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="index">�Ώۂ̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="len">�Ώۂ� bit �����w�肵�܂��B</param>
		/// <returns>�l���擾����ꍇ�ɂ́A�w�肵���ʒu�ɑ��݂������ǂݏo���āA
		/// �������������Ƃ��ĕԂ��܂��B</returns>
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
		/// �������������𕄍����������Ƃ��Ď擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="sec">�g�p���� bit �̈ʒu�ƕ����w�肵�܂��B
		/// ���� 2B �Ɉʒu���w�肵�܂��B
		/// ��� 2B �ɕ����w�肵�܂��B
		/// </param>
		/// <returns>�l���擾����ꍇ�ɂ́A
		/// �w�肵���ꏊ�ɑΉ�������𕄍����������Ƃ��ĕԂ��܂��B
		/// </returns>
		public uint this[BitSection sec]{
			get{return this[(int)sec&0xffff,(int)((uint)sec>>16)];}
			set{this[(int)sec&0xffff,(int)((uint)sec>>16)]=value;}
		}
	}

	/// <summary>
	/// 64bit �� bit �z���\������\���̂ł��B
	/// </summary>
	[System.Serializable]
	public struct BitArray64{
		private long val;
		/// <summary>
		/// BitArray64 �̒l�����������܂��B
		/// </summary>
		/// <param name="value">��������̓��e���܂� long �l���w�肵�܂��B</param>
		public BitArray64(long value){
			this.val=value;
		}
		/// <summary>
		/// �w�肵���ԍ��� bit �̒l���擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="index">�擾���͐ݒ肷��Ώۂ� bit �̔ԍ����w�肵�܂��B</param>
		/// <returns>
		/// �l���擾����ꍇ�́Abit �� 1 �̎� true ��Ԃ��܂��B
		/// bit �� 0 �̏ꍇ�� false ��Ԃ��܂��B</returns>
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
		/// Bit ���� or ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">| ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">| ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���_���a (Or) ���Z�̌��ʂ� BitArray64 ��Ԃ��܂��B</returns>
		public static BitArray64 operator|(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val|right.val);
		}
		/// <summary>
		/// Bit ���� and ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">&amp; ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">&amp; ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���_���� (And) ���Z�̌��ʂ� BitArray64 ��Ԃ��܂��B</returns>
		public static BitArray64 operator&(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val&right.val);
		}
		/// <summary>
		/// Bit ���� xor ���Z�����s���܂��B
		/// </summary>
		/// <param name="left">^ ���Z�q�̍��ӂ��w�肵�܂��B</param>
		/// <param name="right">^ ���Z�q�̉E�ӂ��w�肵�܂��B</param>
		/// <returns>Bit ���r���I�_���a (Xor) ���Z�̌��ʂ� BitArray64 ��Ԃ��܂��B</returns>
		public static BitArray64 operator^(BitArray64 left,BitArray64 right){
			return new BitArray64(left.val^right.val);
		}
		/// <summary>
		/// ������x�� bit �������������𕄍����������Ƃ��Ď擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="index">�Ώۂ̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="len">�Ώۂ� bit �����w�肵�܂��B</param>
		/// <returns>�l���擾����ꍇ�ɂ́A�w�肵���ʒu�ɑ��݂������ǂݏo���āA
		/// �������������Ƃ��ĕԂ��܂��B</returns>
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
		/// �������������𕄍����������Ƃ��Ď擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="sec">�g�p���� bit �̈ʒu�ƕ����w�肵�܂��B
		/// ���� 2B �Ɉʒu���w�肵�܂��B
		/// ��� 2B �ɕ����w�肵�܂��B
		/// </param>
		/// <returns>�l���擾����ꍇ�ɂ́A
		/// �w�肵���ꏊ�ɑΉ�������𕄍����������Ƃ��ĕԂ��܂��B
		/// </returns>
		public uint this[BitSection sec]{
			get{return this[(int)sec&0xffff,(int)((uint)sec>>16)];}
			set{this[(int)sec&0xffff,(int)((uint)sec>>16)]=value;}
		}
	}
}