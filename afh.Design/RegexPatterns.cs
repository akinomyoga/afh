using Gen=System.Collections.Generic;
using Rgx=System.Text.RegularExpressions;

namespace afh.Text{
	internal static class RegexPatterns{
		/// <summary>
		/// "" �ň͂܂ꂽ�����񃊃e�����Ɉ�v���܂��B
		/// </summary>
		public const string DOUBLEQUOTED=@"(?:""(?:[^\\""]|\\.)*"")";
		/// <summary>
		/// '' �ň͂܂ꂽ�������e�����Ɉ�v���܂��B�P�ꕶ���ɂ�����v���܂���B
		/// </summary>
		public const string SINGLEQUOTED=@"(?:'.'|'\\[\w0-9]+')";
		public const string WORD=@"(?:\b[_a-zA-Z][_0-9a-zA-Z]*\b)";

		public const string SPACE_ExceptNewLine=@"[^\r\n\S]";

		/// <summary>
		/// �n�܂�̊��ʂƏI���̊��ʂ���v������l�ȃL���v�`�����s�����K�\���𐶐����܂��B
		/// </summary>
		/// <returns></returns>
		public static string CreateMatchParen(char chOpen,char chClose,bool handlequoted){
			string open		=".()<>".IndexOf(chOpen)>=0?"\\"+chOpen:chOpen.ToString();
			string close	=".()<>".IndexOf(chClose)>=0?"\\"+chClose:chClose.ToString();

			// �O���[�v�����d�����Ȃ��悤�ɂ���ׂ� id
			string id=((uint)chOpen|(uint)chClose<<16).ToString("X8");

			if(handlequoted)
				return "(?:"
					+"[^"+open+close+@"\'\""]"
					+"|"+DOUBLEQUOTED
					+"|"+SINGLEQUOTED
					+"|(?<open"+id+">"+open+")"
					+"|(?<close"+id+"-open"+id+">"+close+")"
				+")*(?(open"+id+")(?!))";
			else
				return "(?:"
					+"[^"+open+close+"]"
					+"|(?<open"+id+">"+open+")"
					+"|(?<close"+id+"-open"+id+">"+close+")"
				+")*(?(open"+id+")(?!))";
		}
	}
}