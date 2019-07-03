namespace ksh{
	/// <summary>
	/// ���f����\������\���̂ł��B
	/// </summary>
	/// <typeparam name="T">�����E�����̌^���w�肵�܂��B</typeparam>
	public struct Complex<T> where T:IComplexible<T>{
		T real;
		T imag;

		/// <summary>
		/// Complex �^���w�肵�������̑g���Ȃď��������܂��B
		/// </summary>
		/// <param name="real">�������w�肵�܂��B</param>
		/// <param name="imaginary">�������w�肵�܂��B</param>
		public Complex(T real,T imaginary){
			this.real=real;
			this.imag=imaginary;
		}

		/// <summary>
		/// ���f���𕶎���ŕ\�����܂��B
		/// </summary>
		/// <returns>������ŕ\���������f����Ԃ��܂��B</returns>
		public override string ToString(){
			if(real.IsZero){
				return imag.IsZero?"0":imag.ToString()+" i";
			}else{
				return imag.IsZero?real.ToString():real.ToString()+" + i "+imag.ToString();
			}
			return base.ToString();
		}
	}
	/// <summary>
	/// ���f���̗v�f�Ƃ��Ďg�p�o����^��\�����܂��B
	/// </summary>
	public interface IComplexible<T>{
		/// <summary>
		/// �l�� 0 �ł��邩�ǂ�����Ԃ��܂��B
		/// </summary>
		bool IsZero{get;}
		/// <summary>
		/// �l�𕶎���ŕ\�����������擾���܂��B
		/// </summary>
		/// <returns>�l�̕�����\����Ԃ��܂��B</returns>
		string ToString();
		/// <summary>
		/// ���Z�����s���܂��B
		/// </summary>
		/// <param name="right">���l�ɉE���瑫���l���w�肵�܂��B</param>
		/// <returns>���̃C���X�^���X�Ɉ����Ŏw�肵���l���E���瑫�������ʂ�Ԃ��܂��B</returns>
		T Add(T right);
	}
}