namespace ksh{
	/// <summary>
	/// 複素数を表現する構造体です。
	/// </summary>
	/// <typeparam name="T">実部・虚部の型を指定します。</typeparam>
	public struct Complex<T> where T:IComplexible<T>{
		T real;
		T imag;

		/// <summary>
		/// Complex 型を指定した実数の組を以て初期化します。
		/// </summary>
		/// <param name="real">実部を指定します。</param>
		/// <param name="imaginary">虚部を指定します。</param>
		public Complex(T real,T imaginary){
			this.real=real;
			this.imag=imaginary;
		}

		/// <summary>
		/// 複素数を文字列で表現します。
		/// </summary>
		/// <returns>文字列で表現した複素数を返します。</returns>
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
	/// 複素数の要素として使用出来る型を表現します。
	/// </summary>
	public interface IComplexible<T>{
		/// <summary>
		/// 値が 0 であるかどうかを返します。
		/// </summary>
		bool IsZero{get;}
		/// <summary>
		/// 値を文字列で表現した物を取得します。
		/// </summary>
		/// <returns>値の文字列表現を返します。</returns>
		string ToString();
		/// <summary>
		/// 加算を実行します。
		/// </summary>
		/// <param name="right">当値に右から足す値を指定します。</param>
		/// <returns>このインスタンスに引数で指定した値を右から足した結果を返します。</returns>
		T Add(T right);
	}
}