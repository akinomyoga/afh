using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms=System.Windows.Forms;
using Gen=System.Collections.Generic;
#if javascript
using JS=afh.Javascript.Parse;
#endif
using TextRange=afh.Parse.LinearLetterReader.TextRange;

namespace afh.Parse.Tester {
	public partial class Form1:Form{
		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender,EventArgs e) {
			this.listView1.Items.Clear();
#if scriptscanner
			afh.Javascript.Parse.WordReader reader=new afh.Parse.Javascript.WordReader(this.textBox1.Text);
			while(reader.ReadNext()){
				this.listView1.Items.Add(new ListViewItem(new string[]{reader.CurrentWord,reader.CurrentType.ToString()}));
			}
			foreach(KeyValuePair<TextRange,AnalyzeError> pair in reader.LetterReader.EnumErrors()){
				string loc=string.Format("エラー {0}-{1}",pair.Key.start,pair.Key.end);
				this.listView1.Items.Add(new ListViewItem(new string[]{pair.Value.message,loc}));
			}
#endif
#if contextscanner
			afh.Parse.AbstractWordReader reader=afh.Parse.Cc.ContextCompiler.GetWordReader(this.textBox1.Text);
			while(reader.ReadNext()){
				this.listView1.Items.Add(new ListViewItem(new string[]{reader.CurrentWord,reader.CurrentType.ToString()}));
			}
			foreach(KeyValuePair<TextRange,AnalyzeError> pair in reader.LetterReader.EnumErrors()){
				string loc=string.Format("エラー {0}-{1}",pair.Key.start,pair.Key.end);
				this.listView1.Items.Add(new ListViewItem(new string[]{pair.Value.message,loc}));
			}
#endif
#if contextcompiler
			afh.Parse.Cc.ContextCompiler cc=new afh.Parse.Cc.ContextCompiler(this.textBox1.Text);
			cc.ToCSharpSource();
			foreach(KeyValuePair<TextRange,AnalyzeError> pair in cc.LetterReader.EnumErrors()){
				string loc=string.Format("エラー {0}-{1}",pair.Key.start,pair.Key.end);
				this.listView1.Items.Add(new ListViewItem(new string[]{pair.Value.message,loc}));
			}
#endif

#if javascript
			JS::JavascriptParser jp=new afh.Javascript.Parse.JavascriptParser();
			jp.Read(this.textBox1.Text);
#endif
		}

		private void button2_Click(object sender,EventArgs e) {
			for(int i=1;i<0x10;i++)WriteLetterTypeTable(i);
		}
		private void WriteLetterTypeTable(int page){
			System.IO.StreamWriter sw=new System.IO.StreamWriter("LetterType"+page.ToString("X")+".htm");
			sw.Write(@"<html><head><style type='text/css'>
td.invalid{color:gray;}
td.token{background-color:#fdf;color:purple;}
td.ope{background-color:#efe;color:green;}
td.num{background-color:#ffd;color:#880;}
td.space{background-color:#dff;color:#088;}
</style></head><body>
<table>
");
			for(int i=0;i<0x100;i++){
				sw.Write("<tr>");
				for(int j=0;j<0x10;j++){
					sw.Write("<td class='");
					int num=(page<<12)|(i<<4)|j;
					switch(LetterType.GetLetterType((char)num).purpose){
						case LetterType.P_Token:	sw.Write("token"); break;
						case LetterType.P_Operator: sw.Write("ope"); break;
						case LetterType.P_Number:	sw.Write("num"); break;
						case LetterType.P_Space:	sw.Write("space"); break;
						default:					sw.Write("invalid"); break;
					}
					sw.Write(string.Format("'>&#x{0:X};</td>",num));
				}
				sw.Write("</tr>\r\n");
			}
			sw.Write(@"</table>
</body></html>");
			sw.Close();
		}

		private void button3_Click(object sender,EventArgs e){
			this.test_ParseHtml(this.textBox1.Text);
		}

		private void button4_Click(object sender,EventArgs e){
			if(this.openFileDialog1.ShowDialog()!=Forms::DialogResult.OK)return;
			string filename=this.openFileDialog1.FileName;

			//afh.Application.Bookmarks.Test.extract_bookmarks2(filename);return;

			string html=System.IO.File.ReadAllText(filename);
			this.test_ParseHtml(html);
		}
		private void test_ParseHtml(string htmlSource){
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(htmlSource);
			//foreach(KeyValuePair<TextRange,AnalyzeError> pair in hp.wreader.LinearReader.EnumErrors()){
			//  string loc=string.Format("エラー {0}-{1}",pair.Key.start,pair.Key.end);
			//  this.listView1.Items.Add(new ListViewItem(new string[]{pair.Value.message,loc}));
			//}
			foreach(afh.HTML.HTMLError err in doc.ErrorList){
			  string loc=string.Format("Error {0}-{1}",err.start,err.end);
			  this.listView1.Items.Add(new ListViewItem(new string[]{err.message,loc}));
			}
			this.domViewer1.Add(doc);
		}
	}
}