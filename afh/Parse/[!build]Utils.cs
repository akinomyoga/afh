namespace afh.Parse{
	/// <summary>
	/// 簡単な読み取りを行うのに便利な関数を提供します。
	/// </summary>
	public static class Utils{
		/// <summary>
		/// 数値を読み取ります。
		/// 数値の読み取りは、AbstractWordReader.ReadNumber と同様の読み取り方によって行います。
		/// AbstractWordReader によって読み取られた語を数値に変換するのに使用します。
		/// </summary>
		/// <param name="text">読み取る数値を格納している文字列を指定します。</param>
		/// <param name="index">文字列の中に於ける、数値の開始位置を指定します。</param>
		/// <param name="value">読み取った数値を返します。</param>
		/// <returns>読み取りが成功した場合に true を返します。
		/// 失敗した場合に false を返します。</returns>
		public static bool ParseDouble(string text,ref int index,out double value){
			int i=index;
			char c=text[i++];
			if(c=='.'){
				
			}if(c<'0'||'9'<c){
				value=double.NaN;
				return false;
			}
		}
	}
}