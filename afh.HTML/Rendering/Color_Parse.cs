namespace afh.Rendering{
	public partial struct ColorName{
		/// <summary>
		/// rgb 関数などの数値を引数に取る物の引数リスト (1,2,3) 等を読み取ります。
		/// </summary>
		/// <param name="text">引数リストを格納している文字列を指定します。</param>
		/// <param name="i">rgb 情報の読み取り開始位置を指定します。具体的には '(' の位置を指し示す状態で渡して下さい。
		/// 関数が戻る時には ')' の次の文字の位置を返します。</param>
		/// <returns>読み取った引数の値を返します。</returns>
		private static double[] read_func_args(string text,ref int i){
			const int NUL_MODE=0x00;	// 初期状態
			const int NUM_MODE=0x08;	// 整数部読み取り中
			const int FRA_MODE=0x10;	// 小数部読み取り中
			const int AFT_MODE=0x18;	// 読み取り終了 (','|')' 待ち)
			const int SKIP_MODE=0x20;	// ')' 待ち (args が溢れた場合は以降の引数は無視)

			const int CHAR_ELS=0;
			const int CHAR_NUM=1;
			const int CHAR_COM=2;
			const int CHAR_DOT=3;
			const int CHAR_END=4;

			const int NumberOfArgs=4;
			double[] args=new double[NumberOfArgs];int iArgs=0;

			//#→template ADD_ARGS<_word>
			{args[iArgs++]=double.Parse(_word);if(iArgs==NumberOfArgs)mode=SKIP_MODE;}
			//#←template
			//-------------------------------------------------------

			int mode=NUL_MODE;
			string word="";
			while(++i<text.Length){
				char c=text[i];
				if(c>='\xff00')c=(char)(c-'\xff00'+'\x20');
				
				// 文字分類
				int ct;
				if('0'<=c&&c<='9'){
					ct=CHAR_NUM;
				}else switch(c){
					case ')':ct=CHAR_END;break;
					case ',':ct=CHAR_COM;break;
					case '.':ct=CHAR_DOT;break;
					default: ct=CHAR_ELS;break;
				}

				switch(mode|ct){
				//-- NUL/AFT
					case NUL_MODE|CHAR_NUM:
					case AFT_MODE|CHAR_NUM:
						word=c.ToString();
						mode=NUM_MODE;
						break;
					case NUL_MODE|CHAR_END:
					case AFT_MODE|CHAR_END:
						goto end_paren;
					case NUL_MODE|CHAR_COM:
						//#ADD_ARGS<"0">
						break;
					case AFT_MODE|CHAR_COM:
						mode=NUL_MODE;
						break;
					case NUL_MODE|CHAR_DOT:
					case AFT_MODE|CHAR_DOT:
						word="0.";
						mode=FRA_MODE;
						break;
				//-- NUM/FRA
					case NUM_MODE|CHAR_ELS:
					case FRA_MODE|CHAR_ELS:
						//#ADD_ARGS<word>
						mode=AFT_MODE;
						break;
					case NUM_MODE|CHAR_NUM:
					case FRA_MODE|CHAR_NUM:
						word+=c;
						break;
					case NUM_MODE|CHAR_END:
					case FRA_MODE|CHAR_END:
						//#ADD_ARGS<word>
						goto end_paren;
					case NUM_MODE|CHAR_COM:
					case FRA_MODE|CHAR_COM:
						//#ADD_ARGS<word>
						mode=NUL_MODE;
						break;
					case NUM_MODE|CHAR_DOT:
						word+=c;
						mode=FRA_MODE;
						break;
					case FRA_MODE|CHAR_DOT:
						//#ADD_ARGS<word>
						goto case NUL_MODE|CHAR_DOT;
				//-- SKIP
					case SKIP_MODE|CHAR_END:
					end_paren:
						i++;return args;
				}
			}
			return args;
		}
	
	}
}