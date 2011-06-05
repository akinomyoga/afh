using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2D=System.Drawing.Drawing2D;
using Adrw=afh.Drawing;
using Color=afh.Drawing.Color32Argb;

namespace afh.Rendering{

	public class Document:System.IDisposable{
		public Document(System.Windows.Forms.Control ctrl){
			this.Drawer=ctrl.CreateGraphics();
		}

		private Drawer d;
		public Drawer Drawer{
			get{return this.d;}
			internal set{this.d=value;}
		}

		public TextNode CreateText(string text){
			return new TextNode(text,this);
		}

		public void Dispose(){
			this.d.Dispose();
		}
	}

}