using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2D=System.Drawing.Drawing2D;
using Adrw=afh.Drawing;
using Color=afh.Drawing.Color32Argb;

namespace afh.Rendering{
	public abstract class Box:IUnit{
		/// <summary>
		/// 内容を指定した位置に描画します。
		/// </summary>
		/// <param name="g">描画先のグラフィックスを指定します。</param>
		public void Draw(Drawer g){
			float x=g.DeltaX,y=g.DeltaY;

			// 背景
			// TODO: Background class を作る? color, image, repeat-x,y, position, stretch

			// 内容
			g.DeltaX=x+this.marginL+this.borderL+this.paddingL;
			g.DeltaY=y+this.marginT+this.borderT+this.paddingT;
			this.DrawContent(g);

			// 境界
			g.DeltaX=x;
			g.DeltaY=y;
			float t=this.marginT+this.borderT/2;
			float b=this.areaH-this.marginB-this.borderB/2;
			float l=this.marginL+this.borderL/2;
			float r=this.areaW-this.marginR-this.borderR/2;
			Gdi::PointF lt=new Gdi::PointF(l,t);
			Gdi::PointF rt=new Gdi::PointF(r,t);
			Gdi::PointF lb=new Gdi::PointF(l,b);
			Gdi::PointF rb=new Gdi::PointF(r,b);
			if(this.borderT>0)g.DrawLine(lt,rt,this.borderCT,this.borderT);
			if(this.borderR>0)g.DrawLine(rt,rb,this.borderCR,this.borderR);
			if(this.borderB>0)g.DrawLine(lb,rb,this.borderCB,this.borderB);
			if(this.borderL>0)g.DrawLine(lt,lb,this.borderCL,this.borderL);
		}
		protected abstract void DrawContent(Drawer g);

		/// <summary>
		/// ボックスの中心の X 座標を取得します。
		/// </summary>
		public float PivotX { get { return this.pivotX; } }
		/// <summary>
		/// ボックスの中心の Y 座標を取得します。
		/// </summary>
		public float PivotY { get { return this.pivotY; } }
		/// <summary>
		/// 内容を描画した時の画面上での高さを取得します。
		/// </summary>
		public float AreaHeight { get { return this.areaH; } }
		/// <summary>
		/// 内容を描画した時の画面上での高さを取得します。
		/// </summary>
		public float AreaWidth { get { return this.areaW; } }

		protected float pivotX=0;
		protected float pivotY=0;
		protected float areaH=0;
		protected float areaW=0;

		// 4B * 18 = 72B
		protected float marginT=0;
		protected float marginB=0;
		protected float marginL=0;
		protected float marginR=0;
		protected Color borderCT=0xff000000;
		protected Color borderCB=0xff000000;
		protected Color borderCL=0xff000000;
		protected Color borderCR=0xff000000;
		protected float borderT=0;
		protected float borderB=0;
		protected float borderL=0;
		protected float borderR=0;
		protected float paddingT=0;
		protected float paddingB=0;
		protected float paddingL=0;
		protected float paddingR=0;
		protected float height=0;
		protected float width=0;

		public virtual void RecalcSize() {
			this.areaH=this.marginT+this.borderT+this.paddingT+this.height+this.paddingB+this.borderB+this.marginB;
			this.areaW=this.marginL+this.borderL+this.paddingL+this.width+this.paddingR+this.borderR+this.marginR;
		}
	}
	public class Line:Box{
		Gen::List<IUnit> elems=new Gen::List<IUnit>();
		public Line() {
			this.InitializeTest();
		}

		private void InitializeTest() {
			this.borderT=1;
			this.borderB=1;
			this.borderL=1;
			this.borderR=1;
			this.paddingT=1;
			this.paddingB=1;
			this.paddingR=1;
			this.paddingL=1;
			this.marginR=1;
			this.borderCT=0xffff0000;
			this.borderCB=0xffff0000;
			this.borderCR=0xffff0000;
			this.borderCL=0xffff0000;
		}

		public void AppendChild(IUnit elem) {
			this.elems.Add(elem);
			this.RecalcSize();
		}


		protected override void DrawContent(Drawer g) {
			//float x=g.DeltaX;
			float y=g.DeltaY;
			foreach(IUnit elem in this.elems) {
				g.DeltaY=y+this.content_pivy-elem.PivotY;
				elem.Draw(g);
				g.DeltaX+=elem.AreaWidth;
			}
		}
		private float content_pivy=0;
		public override void RecalcSize() {
			float below=0;
			float pivy=0;
			float width=0;
			foreach(IUnit elem in this.elems) {
				width+=elem.AreaWidth;
				// max of above pivot
				float elemv=elem.PivotY;
				if(elemv>pivy) pivy=elemv;

				// max of below pivot
				elemv=elem.AreaHeight-elemv;
				if(elemv>below) below=elemv;
			}
			this.height=pivy+below;
			this.width=width;
			this.content_pivy=pivy;
			this.pivotY=this.marginT+this.borderT+this.paddingT+pivy;

			base.RecalcSize();
		}
	}

	public class TextNode:Box{
		internal TextNode(string text,Document document){
			this.c=Gdi::Color.Black;
			this.doc=document;
			this.text=text;
			this.f=new FontManager("MS PGothic",10);
			this.RecalcSize();
		}

		private string text;
		private FontManager f;
		private Color c;
		private Document doc;
			
		/// <summary>
		/// 描画に使用するフォントを取得します。
		/// </summary>
		public FontManager Font{
			get{return this.f;}
		}
		/// <summary>
		/// 表示する文字列を取得亦は設定します。
		/// </summary>
		public string Text{
			get{return this.text;}
			set{
				if(this.text==value)return;
				this.text=value;
				this.RecalcSize();
			}
		}

		protected override void DrawContent(Drawer d){
			d.DrawString(text,new Gdi::Point(0,0),f,c);
		}

		public override void RecalcSize() {
			Gdi::SizeF s=this.doc.Drawer.MeasureString(this.text,this.f);
			this.height=s.Height;
			this.width=s.Width;
			this.pivotY=this.marginT+this.borderT+this.paddingT+this.height/2;
			base.RecalcSize();
		}
	}

	public class Test:IUnit {
		private int height;
		private int width;
		public Test(int width,int height) {
			this.width=width;
			this.height=height;
		}

		public float PivotX {
			get { return 0; }
		}

		public float PivotY {
			get { return this.height/2; }
		}

		public float AreaHeight{
			get { return this.height; }
		}
		public float AreaWidth {
			get { return this.width; }
		}

		public void Draw(Drawer g) {
			int half=this.height/2;
			g.DrawLine(new Gdi::PointF(0,half),new Gdi::PointF(this.width,half),0xff000000,this.height);
		}
	}
}