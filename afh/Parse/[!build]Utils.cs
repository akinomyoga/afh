namespace afh.Parse{
	/// <summary>
	/// �ȒP�ȓǂݎ����s���̂ɕ֗��Ȋ֐���񋟂��܂��B
	/// </summary>
	public static class Utils{
		/// <summary>
		/// ���l��ǂݎ��܂��B
		/// ���l�̓ǂݎ��́AAbstractWordReader.ReadNumber �Ɠ��l�̓ǂݎ����ɂ���čs���܂��B
		/// AbstractWordReader �ɂ���ēǂݎ��ꂽ��𐔒l�ɕϊ�����̂Ɏg�p���܂��B
		/// </summary>
		/// <param name="text">�ǂݎ�鐔�l���i�[���Ă��镶������w�肵�܂��B</param>
		/// <param name="index">������̒��ɉ�����A���l�̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="value">�ǂݎ�������l��Ԃ��܂��B</param>
		/// <returns>�ǂݎ�肪���������ꍇ�� true ��Ԃ��܂��B
		/// ���s�����ꍇ�� false ��Ԃ��܂��B</returns>
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