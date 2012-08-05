using Gdi=System.Drawing;

namespace afh.Drawing{
	/// <summary>
	/// 描画関連の便利な関数を提供します。
	/// </summary>
	public static class GraphicsUtils{
		/// <summary>
		/// 枠付で矩形を塗り潰します。
		/// </summary>
		/// <param name="g">描画対象の矩形を指定します。</param>
		/// <param name="fill">矩形を塗り潰すのに使用するブラシを指定します。</param>
		/// <param name="frame">矩形の枠を描画する為のペンを指定します。</param>
		/// <param name="rect">矩形を指定します。</param>
		public static void FillRectangleFramed(Gdi::Graphics g,Gdi::Brush fill,Gdi::Pen frame,Gdi::Rectangle rect){
			g.FillRectangle(fill,rect);
			rect.Width--;
			rect.Height--;
			g.DrawRectangle(frame,rect);
		}
		/// <summary>
		/// 矩形を塗り潰し、反転色で枠を描画します。
		/// </summary>
		/// <param name="g">描画対象の矩形を指定します。</param>
		/// <param name="fill">塗り潰すのに使用する色を指定します。</param>
		/// <param name="rect">対象の矩形を指定します。</param>
		public static void FillRectangleReverseDotFramed(Gdi::Graphics g,Gdi::Color fill,Gdi::Rectangle rect){
			using(Gdi::SolidBrush brush=new Gdi::SolidBrush(fill))
			using(Gdi::Pen pen=new Gdi::Pen(~(afh.Drawing.Color32Argb)fill)){
				pen.DashStyle=Gdi::Drawing2D.DashStyle.Dot;
				FillRectangleFramed(g,brush,pen,rect);
			}
		}
		/// <summary>
		/// 指定した色の反転色で矩形枠を描画します。
		/// </summary>
		/// <param name="g">描画対象の矩形を指定します。</param>
		/// <param name="fill">矩形枠の反転色を指定します。</param>
		/// <param name="rect">対象の矩形を指定します。</param>
		public static void DrawRectangleReverseDotFramed(Gdi::Graphics g,Gdi::Color fill,Gdi::Rectangle rect){
			using(Gdi::Pen pen=new Gdi::Pen(~(afh.Drawing.Color32Argb)fill)){
				pen.DashStyle=Gdi::Drawing2D.DashStyle.Dot;
				rect.Width--;
				rect.Height--;
				g.DrawRectangle(pen,rect);
			}
		}

		private static readonly Gdi::Bitmap dummy=new Gdi::Bitmap(1,1);
		private static readonly Gdi::Graphics g;
		static GraphicsUtils(){
			g=Gdi::Graphics.FromImage(dummy);
		}

		#region MeasureString
		/// <summary>
		/// 指定した文字列内の文字位置の範囲にそれぞれが外接する System.Drawing.Region オブジェクトの配列を取得します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="layoutRect">文字列のレイアウト矩形を指定する System.Drawing.RectangleF 構造体。</param>
		/// <param name="stringFormat">行間など、文字列の書式情報を表す System.Drawing.StringFormat。</param>
		/// <returns>このメソッドは、指定した文字列内の文字位置の範囲にそれぞれが外接する System.Drawing.Region オブジェクトの配列を返します。</returns>
		public static Gdi::Region[] MeasureCharacterRanges(string text,Gdi::Font font,Gdi::RectangleF layoutRect,Gdi::StringFormat stringFormat){
			return g.MeasureCharacterRanges(text,font,layoutRect,stringFormat);
		}
		/// <summary>
		/// 指定した System.Drawing.Font で描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font){
			return g.MeasureString(text,font);
		}
		/// <summary>
		/// 指定した System.Drawing.Font で描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列の書式を定義する System.Drawing.Font。</param>
		/// <param name="width">文字列の最大幅 (ピクセル単位)。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,int width){
			return g.MeasureString(text,font,width);
		}
		/// <summary>
		/// 指定したレイアウト領域内に指定した System.Drawing.Font で描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="layoutArea">テキストの最大レイアウト領域を指定する System.Drawing.SizeF 構造体。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::SizeF layoutArea){
			return g.MeasureString(text,font,layoutArea);
		}
		/// <summary>
		/// 指定した System.Drawing.Font を使用し、指定した System.Drawing.StringFormat で書式指定して描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="width">文字列の最大幅。</param>
		/// <param name="format">行間など、文字列の書式情報を表す System.Drawing.StringFormat。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータおよび stringFormat パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,int width,Gdi::StringFormat format){
			return g.MeasureString(text,font,width,format);
		}
		/// <summary>
		/// 指定した System.Drawing.Font を使用し、指定した System.Drawing.StringFormat で書式指定して描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="origin">文字列の左上隅を表す System.Drawing.PointF 構造体。</param>
		/// <param name="stringFormat">行間など、文字列の書式情報を表す System.Drawing.StringFormat。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータおよび stringFormat パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::PointF origin,Gdi::StringFormat stringFormat){
			return g.MeasureString(text,font,origin,stringFormat);
		}
		/// <summary>
		/// 指定した System.Drawing.Font を使用し、指定した System.Drawing.StringFormat で書式指定して描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="layoutArea">テキストの最大レイアウト領域を指定する System.Drawing.SizeF 構造体。</param>
		/// <param name="stringFormat"></param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータおよび stringFormat パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::SizeF layoutArea,Gdi::StringFormat stringFormat){
			return g.MeasureString(text,font,layoutArea,stringFormat);
		}
		/// <summary>
		/// 指定した System.Drawing.Font を使用し、指定した System.Drawing.StringFormat で書式指定して描画した場合の、指定した文字列を計測します。
		/// </summary>
		/// <param name="text">計測する文字列。</param>
		/// <param name="font">文字列のテキスト形式を定義する System.Drawing.Font。</param>
		/// <param name="layoutArea">テキストの最大レイアウト領域を指定する System.Drawing.SizeF 構造体。</param>
		/// <param name="stringFormat">行間など、文字列の書式情報を表す System.Drawing.StringFormat。</param>
		/// <param name="charactersFitted">文字列の文字数。</param>
		/// <param name="linesFilled">文字列のテキスト行数。</param>
		/// <returns>このメソッドは、text パラメータに指定された文字列のサイズを System.Drawing.Graphics.PageUnit プロパティで指定された単位で表す
		/// System.Drawing.SizeF 構造体を、font パラメータおよび stringFormat パラメータで描画されたとおりに返します。</returns>
		public static Gdi::SizeF MeasureString(
			string text,Gdi::Font font,Gdi::SizeF layoutArea,
			Gdi::StringFormat stringFormat,out int charactersFitted,out int linesFilled
		){
			return g.MeasureString(text,font,layoutArea,stringFormat,out charactersFitted,out linesFilled);
		}
		#endregion
	}
}
#if OBSOLETE
using Interop=System.Runtime.InteropServices;
using afh.Drawing;

namespace mwg.Forms{
	public static class ControlPaint{
		/*
		public enum FrameType{
			Dotted,
		}
		public static void DrawReversibleFrame(Gdi::Rectangle rect,Gdi::Rectangle clip,FrameType ftype){
			Gdi::Bitmap bmp=new Gdi::Bitmap(rect.Width,rect.Height);
			Gdi::Graphics gtemp=Gdi::Graphics.FromImage(bmp);
			gtemp.CopyFromScreen(rect.Location,Gdi::Point.Empty,rect.Size);
			gtemp.Dispose();
			for(int x=0;x<rect.Width;x++)
				bmp.SetPixel(x,0,Gdi::Color.FromArgb(bmp.GetPixel(x,0).ToArgb()^0x00FFFFFF));
			g.DrawImageUnscaled(bmp,Gdi::Point.Empty);
			Gdi::Graphics g;
			bmp.Dispose();
		}
		//*/
		/*
		public static void DrawReversible(Gdi::Graphics g,Gdi::Pen pen,Gdi::Rectangle rect){
			System.IntPtr hDC=g.GetHdc();
			int nDrawMode=SetROP2(hDC,7);
			System.IntPtr oPen=SelectObject(hDC,pen);
			SetBkColor(hDC,(ColorRef)(Color32Argb)Gdi::Color.Transparent);
			Rectangle(hDC,rect.X+(int)g.Transform.OffsetX,rect.Y+(int)g.Transform.OffsetY,rect.Right,rect.Bottom);
			SetROP2(hDC,nDrawMode);
			SelectObject(hDC,oPen);
		}
		[Interop::DllImport("gdi32.dll")]
		static extern int SetROP2(System.IntPtr hdc,int fnDrawMode);
		[Interop::DllImport("gdi32.dll")]
		static extern System.IntPtr SelectObject(System.IntPtr hdc,System.IntPtr hgdiobj);
		//*/
	}
	internal class ScreenPixels:System.IDisposable{
		const int X_UNIT=32;
		const int Y_UNIT=32;
		const int X_MASK=X_UNIT-1;
		const int Y_MASK=Y_UNIT-1;
		Gdi::Graphics g;
		Gdi::Point g_ofs;

		/// <summary>
		/// スクリーンへのアクセスのキャッシュです。
		/// </summary>
		Gdi::Bitmap bmp;
		Gdi::Graphics bmp_g;
		Gdi::Imaging.BitmapData bmp_d;
		bool bmp_dirty=false;

		bool first=true;
		Gdi::Point current;
		/// <summary>
		/// 現在のスクリーン座標を取得又は設定します。
		/// </summary>
		Gdi::Point Current{
			get{return this.current;}
			set{
				if(!first&&this.current==value)return;
				Flush();
				bmp_g.CopyFromScreen(value,Gdi::Point.Empty,new Gdi::Size(X_UNIT,Y_UNIT));
				this.bmp_d=bmp.LockBits(
					new Gdi::Rectangle(0,0,X_UNIT,Y_UNIT),
					Gdi::Imaging.ImageLockMode.ReadWrite,
					Gdi::Imaging.PixelFormat.Format24bppRgb);
				this.current=value;
				this.first=false;
			}
		}
		//============================================================
		//		初期化
		//============================================================
		public ScreenPixels(Gdi::Graphics target,Gdi::Point targetOffset){
			this.g=target;
			this.g_ofs=targetOffset;

			this.bmp=new Gdi::Bitmap(X_UNIT,Y_UNIT,Gdi::Imaging.PixelFormat.Format24bppRgb);
			this.bmp_g=System.Drawing.Graphics.FromImage(bmp);
			this.current=Gdi::Point.Empty;
		}
		public ScreenPixels(Gdi::Graphics target,int x,int y)
			:this(target,new Gdi::Point(x,y))
		{}
		//============================================================
		//		アクセス
		//============================================================
		public unsafe afh.Drawing.Color32Argb this[int x,int y]{
			get{
				TranslatePoints(ref x,ref y);
				return *(int*)((int)bmp_d.Scan0+bmp_d.Stride*y+x*3);//(afh.Drawing.Color32Argb)bmp.GetPixel(x,y);
			}
			set{
				TranslatePoints(ref x,ref y);
				*(int*)((int)bmp_d.Scan0+bmp_d.Stride*y+x*3)=(int)value;
				//bmp.SetPixel(x,y,(Gdi::Color)value);
				bmp_dirty=true;
			}
		}

		private void TranslatePoints(ref int x,ref int y){
			// screen 座標へ変換
			x+=g_ofs.X+(int)g.Transform.OffsetX;
			y+=g_ofs.Y+(int)g.Transform.OffsetY;

			// キャッシュの確認
			int dx=x-current.X;
			int dy=y-current.Y;
			if(first||dx<0||X_UNIT<=dx||dy<0||Y_UNIT<=dy){
				Current=new Gdi::Point(x&~X_MASK,y&~Y_MASK);
			}

			// キャッシュ座標へ変換
			x&=X_MASK;
			y&=Y_MASK;
		}

		public void Flush(){
			if(bmp_dirty){
				this.bmp.UnlockBits(bmp_d);
				g.DrawImageUnscaled(
					bmp,
					current.X-g_ofs.X-(int)g.Transform.OffsetX,
					current.Y-g_ofs.Y-(int)g.Transform.OffsetY
					);
				bmp_dirty=false;
			}
		}

		public void Dispose(){
			Flush();
			this.bmp_g.Dispose();
			this.bmp.Dispose();
		}
	}
}
#endif
