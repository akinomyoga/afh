using Gen=System.Collections.Generic;
using Reg=System.Text.RegularExpressions;

namespace afh.Text{
	/// <summary>
	/// �悭�g�����K�\����Z�߂��N���X�ł��B
	/// </summary>
	public static class Regexs{
		/// <summary>
		/// \r\n \n \r �̉��s�̂ǂꂩ�Ɉ�v���܂��B
		/// </summary>
		public static Reg::Regex Rx_NewLine=new Reg::Regex("\r\n?|\n",Reg::RegexOptions.Compiled);
	}
	/// <summary>
	/// �悭�g�������񑀍��Z�߂��N���X�ł��B
	/// </summary>
	public static class TextUtils{
		/// <summary>
		/// �����z���A�����Ĉ�̕�����ɂ��܂��B
		/// </summary>
		/// <param name="textArray">�A�����镶������i�[���Ă��镶���z����w�肵�܂��B</param>
		/// <param name="splitter">�A�����邻�ꂼ��̕�����ɋ��ށA��؂蕶������w�肵�܂��B</param>
		/// <returns>�A���������ʂƂ��Ăł��镶�����Ԃ��܂��B</returns>
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
		/// �����z���A�����Ĉ�̕�����ɂ��܂��B
		/// </summary>
		/// <param name="textArray">�A�����镶������i�[���Ă��镶���z����w�肵�܂��B</param>
		/// <returns>�A���������ʂƂ��Ăł��镶�����Ԃ��܂��B</returns>
		public static string Join(string[] textArray){
			return Join(textArray,null);
		}
	
		/// <summary>
		/// �w�肵�����������e�Ɏ��A�����񃊃e�����\�L���擾���܂��B
		/// </summary>
		/// <param name="content">�����񃊃e�����̓��e���w�肵�܂��B</param>
		/// <returns>�����񃊃e�����\�L�ɂȂ����������Ԃ��܂��B</returns>
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
		/// �w�肵�����������e�Ɏ��A�������e�����\�L���擾���܂��B
		/// </summary>
		/// <param name="content">�������e�����̓��e���w�肵�܂��B</param>
		/// <returns>�������e�����\�L�ɂȂ����������Ԃ��܂��B</returns>
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
		/// �w�肵��������� HTML �ŕ\������������ɕϊ����܂��B
		/// &amp; �� &lt; &gt; ���� HTML ���œ��ʂȈӖ����������� HTML �̎Q�Ǝ��̂ɒu�������܂��B
		/// </summary>
		/// <param name="text">��������w�肵�܂��B</param>
		/// <returns>HTML �ŕ\�������������Ԃ��܂��B</returns>
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
		/// �w�肵��������� HTML �ŕ\������������ɕϊ����܂��B
		/// &amp; �� &lt; &gt; ���� HTML ���œ��ʂȈӖ����������� HTML �̎Q�Ǝ��̂ɒu�������܂��B
		/// </summary>
		/// <param name="text">��������w�肵�܂��B</param>
		/// <returns>HTML �ŕ\�������������Ԃ��܂��B</returns>
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
	/// ������� n �����ڂƉ��s����ڂ�Ή��t����ׂ̃N���X�ł��B
	/// </summary>
	public class MultilineString{
		private string str;
		private int[] i_lines;
		private int c_lines;	// �s�̐�

		/// <summary>
		/// MultilineString �̐V�����C���X�^���X�𐶐����܂��B
		/// </summary>
		/// <param name="text">���s����ڂƉ������ڂ̑Ή���t���镶������w�肵�܂��B</param>
		public MultilineString(string text) {
			Gen::List<int> lines=new Gen::List<int>();
			lines.Add(0);
			bool lineend=false,cr=false;
			for(int i=0,iM=text.Length;i<iM;i++) {
				if(text[i]=='\n') {
					if(lineend&&!cr) lines.Add(i); // �O�̕���=='\n'
					cr=false;
					lineend=true;
				} else if(lineend) {
					// �O�̕���== '\n' | '\r'
					lines.Add(i);
					cr=lineend=text[i]=='\r';
				}
			}
			this.c_lines=lines.Count;
			
			// ������̖���
			lines.Add(text.Length);
			this.i_lines=lines.ToArray();
			
			this.str=text;
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ��镶�����擾���܂��B
		/// </summary>
		/// <param name="line">�s�ԍ����w�肵�܂��B��ԏ��߂̍s�̔ԍ��� 0 �ł��B</param>
		/// <param name="column">��ԍ����w�肵�܂��B�e�s�̈�ԏ��߂̕����̔ԍ��� 0 �ł��B</param>
		/// <returns>�w�肵���ʒu�ɂ��镶����Ԃ��܂��B</returns>
		public char this[int line,int column]{
			get{return this.str[this.GetIndexOfChar(line,column)];}
		}
		/// <summary>
		/// ���̃C���X�^���X�̕�������擾���܂��B
		/// </summary>
		public string Text{
			get{return this.str;}
		}
		/// <summary>
		/// �w�肵���������A������̒��ŉ��s�������ڂɈʒu���Ă��邩���擾���܂��B
		/// </summary>
		/// <param name="letterIndex">���s����ڂɂ��邩��m�肽�������́A�����񒆂ɉ�����ʒu���w�肵�܂��B</param>
		/// <param name="lineNumber">���s�ڂɈʒu���Ă��邩��Ԃ��܂��B���߂̍s�� 0 �s�ڂł��B</param>
		/// <param name="columnNumber">����ڂɈʒu���Ă��邩��Ԃ��܂��B���߂̗�� 0 ��ڂł��B</param>
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
		/// �w�肵���s�E��ɂ��镶�����A������S�̂��猩�ĉ������ڂɈʒu���Ă��邩���擾���܂��B
		/// </summary>
		/// <param name="line">���s�ڂ̕��������w�肵�܂��B���߂̍s�� 0 �s�ڂł��B</param>
		/// <param name="column">����ڂ̕��������w�肵�܂��B���߂̗�� 0 ��ڂł��B</param>
		/// <returns>������S�̂ŁA�������������ڂɂ��邩��Ԃ��܂��B��ԏ��߂̕����� 0 �����ڂł��B</returns>
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