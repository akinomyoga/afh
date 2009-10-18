using Gen=System.Collections.Generic;
using Rgx=System.Text.RegularExpressions;

namespace afh.Text{
	internal static class RegexPatterns{
		/// <summary>
		/// "" で囲まれた文字列リテラルに一致します。
		/// </summary>
		public const string DOUBLEQUOTED=@"(?:""(?:[^\\""]|\\.)*"")";
		/// <summary>
		/// '' で囲まれた文字リテラルに一致します。単一文字にしか一致しません。
		/// </summary>
		public const string SINGLEQUOTED=@"(?:'.'|'\\[\w0-9]+')";
		public const string WORD=@"(?:\b[_a-zA-Z][_0-9a-zA-Z]*\b)";

		public const string SPACE_ExceptNewLine=@"[^\r\n\S]";

		/// <summary>
		/// 始まりの括弧と終わりの括弧を一致させる様なキャプチャを行う正規表現を生成します。
		/// </summary>
		/// <returns></returns>
		public static string CreateMatchParen(char chOpen,char chClose,bool handlequoted){
			string open		=".()<>".IndexOf(chOpen)>=0?"\\"+chOpen:chOpen.ToString();
			string close	=".()<>".IndexOf(chClose)>=0?"\\"+chClose:chClose.ToString();

			// グループ名が重複しないようにする為の id
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