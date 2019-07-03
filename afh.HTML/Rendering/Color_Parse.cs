namespace afh.Rendering{
	public partial struct ColorName{
		/// <summary>
		/// rgb �֐��Ȃǂ̐��l�������Ɏ�镨�̈������X�g (1,2,3) ����ǂݎ��܂��B
		/// </summary>
		/// <param name="text">�������X�g���i�[���Ă��镶������w�肵�܂��B</param>
		/// <param name="i">rgb ���̓ǂݎ��J�n�ʒu���w�肵�܂��B��̓I�ɂ� '(' �̈ʒu���w��������Ԃœn���ĉ������B
		/// �֐����߂鎞�ɂ� ')' �̎��̕����̈ʒu��Ԃ��܂��B</param>
		/// <returns>�ǂݎ���������̒l��Ԃ��܂��B</returns>
		private static double[] read_func_args(string text,ref int i){
			const int NUL_MODE=0x00;	// �������
			const int NUM_MODE=0x08;	// �������ǂݎ�蒆
			const int FRA_MODE=0x10;	// �������ǂݎ�蒆
			const int AFT_MODE=0x18;	// �ǂݎ��I�� (','|')' �҂�)
			const int SKIP_MODE=0x20;	// ')' �҂� (args ����ꂽ�ꍇ�͈ȍ~�̈����͖���)

			const int CHAR_ELS=0;
			const int CHAR_NUM=1;
			const int CHAR_COM=2;
			const int CHAR_DOT=3;
			const int CHAR_END=4;

			const int NumberOfArgs=4;
			double[] args=new double[NumberOfArgs];int iArgs=0;

			//#��template ADD_ARGS<_word>
			{args[iArgs++]=double.Parse(_word);if(iArgs==NumberOfArgs)mode=SKIP_MODE;}
			//#��template
			//-------------------------------------------------------

			int mode=NUL_MODE;
			string word="";
			while(++i<text.Length){
				char c=text[i];
				if(c>='\xff00')c=(char)(c-'\xff00'+'\x20');
				
				// ��������
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