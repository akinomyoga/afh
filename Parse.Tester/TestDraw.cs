using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using afh.Rendering;
using Interop=System.Runtime.InteropServices;
using HDC=System.IntPtr;
using Color=afh.Drawing.Color32Argb;
using ColorRef=afh.Drawing.ColorRef;

namespace afh.Parse.Tester {

	public partial class TestDraw:System.Windows.Forms.Form {

		public TestDraw() {
			InitializeComponent();

			this.doc=new Document(this);

			this.line=new Line();
			Line l2=new Line();
			l2.AppendChild(new Test(20,15));
			this.line.AppendChild(l2);
			this.line.AppendChild(new Test(30,10));
			this.line.AppendChild(doc.CreateText("さようなら"));
			this.line.AppendChild(new Test(50,20));
		}

		private Document doc;
		Gdi::Pen pen=Gdi::Pens.Red;
		Line line;
		FontManager font=new FontManager("MS PGothic",10);
		private void button1_Click(object sender,System.EventArgs e){
			//Gdi::Graphics g=this.CreateGraphics();
			//g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			Drawer d=this.doc.Drawer;
			d.DeltaX=40;
			d.DeltaY=40;
			line.Draw(d);
			//g.DrawLine(this.pen,new Gdi::PointF(10,10),new Gdi::PointF(100,10));
			//g.DrawLine(this.pen,new Gdi::PointF(10,10),new Gdi::PointF(10,100));
			//g.DrawString("あいうえお",font.Font,Gdi::Brushes.Green,new Gdi::PointF(0,100));

			//g.Dispose();
		}

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
			if(doc!=null)doc.Dispose();

			if(disposing){
				if(components!=null)components.Dispose();
			}
			base.Dispose(disposing);
		}
	}

}

