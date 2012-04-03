using Gen=System.Collections.Generic;
using Reg=System.Text.RegularExpressions;

namespace afh.Text{
	/// <summary>
	/// よく使う正規表現を纏めたクラスです。
	/// </summary>
	public static class Regexs{
		/// <summary>
		/// \r\n \n \r の改行のどれかに一致します。
		/// </summary>
		public static Reg::Regex Rx_NewLine=new Reg::Regex("\r\n?|\n",Reg::RegexOptions.Compiled);
	}
	/// <summary>
	/// よく使う文字列操作を纏めたクラスです。
	/// </summary>
	public static class TextUtils{
		/// <summary>
		/// 文字配列を連結して一つの文字列にします。
		/// </summary>
		/// <param name="textArray">連結する文字列を格納している文字配列を指定します。</param>
		/// <param name="splitter">連結するそれぞれの文字列に挟む、区切り文字列を指定します。</param>
		/// <returns>連結した結果としてできる文字列を返します。</returns>
		public static string Join(string[] textArray,string splitter){
			if(textArray.Length==0)return "";
			if(textArray.Length==1)return textArray[0];
			System.Text.StringBuilder buff=new System.Text.StringBuilder();
			buff.Append(textArray[0]);
			for(int i=1;i<textArray.Length;i++){
				if(splitter!=null)buff.Append(splitter);
				buff.Append(textArray[i]);
			}
			return buff.ToString();
		}
		/// <summary>
		/// 文字配列を連結して一つの文字列にします。
		/// </summary>
		/// <param name="textArray">連結する文字列を格納している文字配列を指定します。</param>
		/// <returns>連結した結果としてできる文字列を返します。</returns>
		public static string Join(string[] textArray){
			return Join(textArray,null);
		}
	
		/// <summary>
		/// 指定した文字列を内容に持つ、文字列リテラル表記を取得します。
		/// </summary>
		/// <param name="content">文字列リテラルの内容を指定します。</param>
		/// <returns>文字列リテラル表記になった文字列を返します。</returns>
		public static string Stringize(string content){
			System.Text.StringBuilder buf=new System.Text.StringBuilder();
			buf.Append('"');

			for(int i=0;i<content.Length;i++) {
				const string CTRL="\a\b\r\n\f\v\t\0\\\"";
				const string ESC="abrnfvt0\\\"";
				int ind=CTRL.IndexOf(content[i]);
				if(ind<0){
					buf.Append(content[i]);
					continue;
				}

				buf.Append('\\');
				buf.Append(ESC[ind]);
			}
			
			buf.Append('"');
			return buf.ToString();
		}
		/// <summary>
		/// 指定した文字列を内容に持つ、文字リテラル表記を取得します。
		/// </summary>
		/// <param name="content">文字リテラルの内容を指定します。</param>
		/// <returns>文字リテラル表記になった文字列を返します。</returns>
		public static string Characterize(string content){
			System.Text.StringBuilder buf=new System.Text.StringBuilder();
			buf.Append('\'');

			for(int i=0;i<content.Length;i++) {
				const string CTRL="\a\b\r\n\f\v\t\0\\\'";
				const string ESC="abrnfvt0\\\'";
				int ind=CTRL.IndexOf(content[i]);
				if(ind<0){
					buf.Append(content[i]);
					continue;
				}

				buf.Append('\\');
				buf.Append(ESC[ind]);
			}
			
			buf.Append('\'');
			return buf.ToString();
		}

		static System.Text.RegularExpressions.Regex reg_html
			=new System.Text.RegularExpressions.Regex(@"\r\n?|[\&\<\> ""'\t\n]",Reg::RegexOptions.Compiled);
		/// <summary>
		/// 指定した文字列を HTML で表現した文字列に変換します。
		/// &amp; や &lt; &gt; 等の HTML 中で特別な意味を持つ文字を HTML の参照実体に置き換えます。
		/// </summary>
		/// <param name="text">文字列を指定します。</param>
		/// <returns>HTML で表現した文字列を返します。</returns>
		public static string EscapeHtml(string text){
			return reg_html.Replace(text,delegate(Reg::Match m){
				switch(m.Value[0]){
					case '&':return "&amp;";
					case '<':return "&lt;";
					case '>':return "&gt;";
					case ' ':return "&nbsp;";
					case '"':return "&quot;";
					case '\r':
					case '\n':return "&#10;";
					case '\t':return "&#9;";
					case '\'':return "&#39;";
					default:return m.Value;
				}
			});
		}
		/// <summary>
		/// 指定した文字列を HTML で表現した文字列に変換します。
		/// &amp; や &lt; &gt; 等の HTML 中で特別な意味を持つ文字を HTML の参照実体に置き換えます。
		/// </summary>
		/// <param name="text">文字列を指定します。</param>
		/// <returns>HTML で表現した文字列を返します。</returns>
		public static string EscapeXml(string text){
			return reg_html.Replace(text,delegate(Reg::Match m){
				switch(m.Value[0]){
					case '&':return "&amp;";
					case '<':return "&lt;";
					case '>':return "&gt;";
					//case ' ':return "&#32;";
					case '"':return "&quot;";
					case '\r':
					case '\n':return "&#10;";
					case '\t':return "&#9;";
					case '\'':return "&#39;";
					default:return m.Value;
				}
			});
		}
	}
	/// <summary>
	/// 文字列の n 文字目と何行何列目を対応付ける為のクラスです。
	/// </summary>
	public class MultilineString{
		private string str;
		private int[] i_lines;
		private int c_lines;	// 行の数

		/// <summary>
		/// MultilineString の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="text">何行何列目と何文字目の対応を付ける文字列を指定します。</param>
		public MultilineString(string text) {
			Gen::List<int> lines=new Gen::List<int>();
			lines.Add(0);
			bool lineend=false,cr=false;
			for(int i=0,iM=text.Length;i<iM;i++) {
				if(text[i]=='\n') {
					if(lineend&&!cr) lines.Add(i); // 前の文字=='\n'
					cr=false;
					lineend=true;
				} else if(lineend) {
					// 前の文字== '\n' | '\r'
					lines.Add(i);
					cr=lineend=text[i]=='\r';
				}
			}
			this.c_lines=lines.Count;
			
			// 文字列の末尾
			lines.Add(text.Length);
			this.i_lines=lines.ToArray();
			
			this.str=text;
		}
		/// <summary>
		/// 指定した位置にある文字を取得します。
		/// </summary>
		/// <param name="line">行番号を指定します。一番初めの行の番号が 0 です。</param>
		/// <param name="column">列番号を指定します。各行の一番初めの文字の番号が 0 です。</param>
		/// <returns>指定した位置にある文字を返します。</returns>
		public char this[int line,int column]{
			get{return this.str[this.GetIndexOfChar(line,column)];}
		}
		/// <summary>
		/// このインスタンスの文字列を取得します。
		/// </summary>
		public string Text{
			get{return this.str;}
		}
		/// <summary>
		/// 指定した文字が、文字列の中で何行何文字目に位置しているかを取得します。
		/// </summary>
		/// <param name="letterIndex">何行何列目にいるかを知りたい文字の、文字列中に於ける位置を指定します。</param>
		/// <param name="lineNumber">何行目に位置しているかを返します。初めの行は 0 行目です。</param>
		/// <param name="columnNumber">何列目に位置しているかを返します。初めの列は 0 列目です。</param>
		public void GetLineAndColumn(int letterIndex,out int lineNumber,out int columnNumber) {
			int d=0;
			int u=c_lines;
			while(u-d>1) {
				int c=(u+d)/2;
				if(letterIndex<this.i_lines[c]) {
					u=c;
				} else d=c;
			}
			lineNumber=d;
			columnNumber=letterIndex-this.i_lines[d];
		}
		/// <summary>
		/// 指定した行・列にいる文字が、文字列全体から見て何文字目に位置しているかを取得します。
		/// </summary>
		/// <param name="line">何行目の文字かを指定します。初めの行は 0 行目です。</param>
		/// <param name="column">何列目の文字かを指定します。初めの列は 0 列目です。</param>
		/// <returns>文字列全体で、文字が何文字目にいるかを返します。一番初めの文字が 0 文字目です。</returns>
		public int GetIndexOfChar(int line,int column) {
			if(line<0||c_lines<=line)
				throw new System.ArgumentOutOfRangeException("line");
			int ret=this.i_lines[line]+column;
			if(ret>=this.i_lines[line+1])
				throw new System.ArgumentOutOfRangeException("column");
			return ret;
		}
	}
}