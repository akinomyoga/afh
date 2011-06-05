using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2D=System.Drawing.Drawing2D;
using Adrw=afh.Drawing;
using Color=afh.Drawing.Color32Argb;

namespace afh.Rendering{
	/// <summary>
	/// 描画の最小単位を表現します。
	/// </summary>
	public interface IUnit{
		/// <summary>
		/// ボックスの中心の X 座標を取得します。
		/// </summary>
		float PivotX{get;}
		/// <summary>
		/// ボックスの中心の X 座標を取得します。
		/// </summary>
		float PivotY{get;}
		/// <summary>
		/// 内容を描画した時の画面上での高さを取得します。
		/// </summary>
		float AreaHeight{get;}
		/// <summary>
		/// 内容を描画した時の画面上での高さを取得します。
		/// </summary>
		float AreaWidth{get;}
		/// <summary>
		/// 内容を指定した位置に描画します。
		/// </summary>
		/// <param name="g">描画先のグラフィックスを指定します。</param>
		void Draw(Drawer g);
	}

	/// <summary>
	/// 描画手段を提供します。
	/// </summary>
	public abstract class Drawer{
		/// <summary>
		/// 描画の基準点の x 座標を指定します。
		/// </summary>
		public abstract float DeltaX {get;set;}
		/// <summary>
		/// 描画の基準点の y 座標を指定します。
		/// </summary>
		public abstract float DeltaY {get;set;}

		/// <summary>
		/// 線の描画を行います。
		/// </summary>
		/// <param name="p1">線の起点を指定します。</param>
		/// <param name="p2">線の終点を指定します。</param>
		/// <param name="color">線の色を指定します。</param>
		/// <param name="width">線の太さを指定します。</param>
		public abstract void DrawLine(Gdi::PointF p1,Gdi::PointF p2,Color color,float width);
		public abstract void DrawString(string text,Gdi::PointF p2,FontManager f,Color color);
		public abstract Gdi::SizeF MeasureString(string text,FontManager f);
		public static implicit operator Drawer(Gdi::Graphics g){
			return new GdiDrawer(g);
		}

		public abstract void Dispose();
	}

	/// <summary>
	/// System.Drawing.Graphics のアダプタ。
	/// </summary>
	internal sealed class GdiDrawer:Drawer,System.IDisposable {
		private Gdi::Graphics g;
		private Gdi::Pen pen=new Gdi::Pen((Color)0xff000000);

		public GdiDrawer(Gdi::Graphics g) {
			this.g=g;
		}

		private float dx=0;
		private float dy=0;
		public override float DeltaX {
			get { return this.dx; }
			set {
				if(this.dx==value) return;
				this.dx=value;
				g.Transform=new Gdi2D::Matrix(1,0,0,1,this.dx,this.dy);
			}
		}
		public override float DeltaY {
			get { return this.dy; }
			set {
				if(this.dy==value) return;
				this.dy=value;
				g.Transform=new Gdi2D::Matrix(1,0,0,1,this.dx,this.dy);
			}
		}
		public override void DrawLine(System.Drawing.PointF p1,System.Drawing.PointF p2,Color color,float width) {
			this.pen.Color=(Gdi::Color)color;
			this.pen.Width=width;
			//this.g.DrawLine(this.pen,p1,p2);
			this.g.DrawLine(this.pen,Gdi::Point.Truncate(p1),Gdi::Point.Truncate(p2));
		}
		public override void DrawString(string text,Gdi::PointF p,FontManager f,Color color) {
			this.g.DrawString(text,f.Font,new Gdi::SolidBrush((Gdi::Color)color),p);
		}
		public override Gdi::SizeF MeasureString(string text,FontManager f) {
			return this.g.MeasureString(text,f.Font);
		}
		public override void Dispose() {
			this.g.Dispose();
			this.pen.Dispose();
		}
	}

	/// <summary>
	/// Style などに準拠した描画を行う要素です。
	/// </summary>
	public class Element{
		internal Element parent;
		internal Style style;
		public Element(Element parent){
			this.parent=parent;
			this.style=new Style(parent.style);
		}
		public Element(){
			this.parent=null;
			this.style=new Style();
		}
	}
	/// <summary>
	/// 書式などの見た目に関する設定を保持するクラスです。
	/// </summary>
	public sealed class Style{
		private Style parent;

		private Gen::Dictionary<string,object> props=new Gen::Dictionary<string,object>();

		internal Style(){
			this.parent=null;
		}
		internal Style(Style parent){
			this.parent=parent;
		}
		public object this[string key]{
			get{
				object ret;
				return this.props.TryGetValue(key,out ret) ? ret
					:this.parent!=null ? this.parent[key]
					:null;
			}
			set{
				switch(key){
					case "margin-top":
						break;
					case "margin-bottom":
					case "margin-left":
					case "margin-right":
					default:
						this.props[key]=value;
						break;
				}
			}
		}
		//===========================================================
		//		他
		//===========================================================
		private static readonly object AUTO=new object();
		private static readonly object INHERIT=new object();
		private object GetRaw(string key,bool inherit){
			object r;
			if(this.props.TryGetValue(key,out r)){
				if(r!=INHERIT)return r;
			}else{
				if(!inherit)return null;
			}
			return this.parent==null?null:this.parent.GetRaw(key,inherit);
		}
		//===========================================================
		//		float
		//===========================================================
		public float MarginTop{
			get{return (float)(this.GetRaw("afh:margin-top",false)??0);}
		}
		public float MarginBottom{
			get{return (float)(this.GetRaw("afh:margin-bottom",false)??0);}
		}
		public float MarginLeft{
			get{return (float)(this.GetRaw("afh:margin-left",false)??0);}
		}
		public float MarginRight{
			get{return (float)(this.GetRaw("afh:margin-right",false)??0);}
		}
		//===========================================================
		//		色々
		//===========================================================
		private static readonly Color def_scrollbar_base_color		=(Color)System.Drawing.SystemColors.ScrollBar;
		private static readonly Color def_scrollbar_highlight_color	=(Color)System.Drawing.SystemColors.ControlLightLight;
		private static readonly Color def_scrollbar_3dlight_color	=(Color)System.Drawing.SystemColors.ControlLight;
		private static readonly Color def_scrollbar_shadow_color	=(Color)System.Drawing.SystemColors.ControlDark;
		private static readonly Color def_scrollbar_darkshadow_color=(Color)System.Drawing.SystemColors.ControlDarkDark;
		private static readonly Color def_scrollbar_arrow_color		=(Color)System.Drawing.SystemColors.ControlText;
		private static readonly Color def_scrollbar_face_color		=(Color)System.Drawing.SystemColors.Control;
		public Color Color{
			get{return (Color)(this.GetRaw("afh:color",true)??Color.Black);}
		}
		public Color BackgroundColor{
			get{return (Color)(this.GetRaw("afh:background-color",true)??Color.White);}
		}
		public Color BorderTopColor{
			get{return (Color)(this.GetRaw("afh:border-top-color",false)??Color.Black);}
		}
		public Color BorderBottomColor{
			get{return (Color)(this.GetRaw("afh:border-bottom-color",false)??Color.Black);}
		}
		public Color BorderLeftColor{
			get{return (Color)(this.GetRaw("afh:border-left-color",false)??Color.Black);}
		}
		public Color BorderRightColor{
			get{return (Color)(this.GetRaw("afh:border-right-color",false)??Color.Black);}
		}
		public Color ScrollbarBaseColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-base-color",false)??def_scrollbar_base_color);}
		}
		public Color ScrollbarHighlightColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-highlight-color",false)??def_scrollbar_highlight_color); }
		}
		public Color Scrollbar3dLightColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-3dlight-color",false)??def_scrollbar_3dlight_color);}
		}
		public Color ScrollbarShadowColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-shadow-color",false)??def_scrollbar_shadow_color);}
		}
		public Color ScrollbarDarkshadowColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-darkshadow-color",false)??def_scrollbar_darkshadow_color);}
		}
		public Color ScrollbarArrowColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-arrow-color",false)??def_scrollbar_arrow_color);}
		}
		public Color ScrollbarFaceColor{
			get{return (Color)(this.GetRaw("afh:scrollbar-face-color",false)??def_scrollbar_face_color);}
		}
	}
}