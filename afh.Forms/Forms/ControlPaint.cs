using Gdi=System.Drawing;

namespace afh.Drawing{
	/// <summary>
	/// �`��֘A�֗̕��Ȋ֐���񋟂��܂��B
	/// </summary>
	public static class GraphicsUtils{
		/// <summary>
		/// �g�t�ŋ�`��h��ׂ��܂��B
		/// </summary>
		/// <param name="g">�`��Ώۂ̋�`���w�肵�܂��B</param>
		/// <param name="fill">��`��h��ׂ��̂Ɏg�p����u���V���w�肵�܂��B</param>
		/// <param name="frame">��`�̘g��`�悷��ׂ̃y�����w�肵�܂��B</param>
		/// <param name="rect">��`���w�肵�܂��B</param>
		public static void FillRectangleFramed(Gdi::Graphics g,Gdi::Brush fill,Gdi::Pen frame,Gdi::Rectangle rect){
			g.FillRectangle(fill,rect);
			rect.Width--;
			rect.Height--;
			g.DrawRectangle(frame,rect);
		}
		/// <summary>
		/// ��`��h��ׂ��A���]�F�Řg��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`��Ώۂ̋�`���w�肵�܂��B</param>
		/// <param name="fill">�h��ׂ��̂Ɏg�p����F���w�肵�܂��B</param>
		/// <param name="rect">�Ώۂ̋�`���w�肵�܂��B</param>
		public static void FillRectangleReverseDotFramed(Gdi::Graphics g,Gdi::Color fill,Gdi::Rectangle rect){
			using(Gdi::SolidBrush brush=new Gdi::SolidBrush(fill))
			using(Gdi::Pen pen=new Gdi::Pen(~(afh.Drawing.Color32Argb)fill)){
				pen.DashStyle=Gdi::Drawing2D.DashStyle.Dot;
				FillRectangleFramed(g,brush,pen,rect);
			}
		}
		/// <summary>
		/// �w�肵���F�̔��]�F�ŋ�`�g��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`��Ώۂ̋�`���w�肵�܂��B</param>
		/// <param name="fill">��`�g�̔��]�F���w�肵�܂��B</param>
		/// <param name="rect">�Ώۂ̋�`���w�肵�܂��B</param>
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
		/// �w�肵����������̕����ʒu�͈̔͂ɂ��ꂼ�ꂪ�O�ڂ��� System.Drawing.Region �I�u�W�F�N�g�̔z����擾���܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="layoutRect">������̃��C�A�E�g��`���w�肷�� System.Drawing.RectangleF �\���́B</param>
		/// <param name="stringFormat">�s�ԂȂǁA������̏�������\�� System.Drawing.StringFormat�B</param>
		/// <returns>���̃��\�b�h�́A�w�肵����������̕����ʒu�͈̔͂ɂ��ꂼ�ꂪ�O�ڂ��� System.Drawing.Region �I�u�W�F�N�g�̔z���Ԃ��܂��B</returns>
		public static Gdi::Region[] MeasureCharacterRanges(string text,Gdi::Font font,Gdi::RectangleF layoutRect,Gdi::StringFormat stringFormat){
			return g.MeasureCharacterRanges(text,font,layoutRect,stringFormat);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font �ŕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font){
			return g.MeasureString(text,font);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font �ŕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̏������`���� System.Drawing.Font�B</param>
		/// <param name="width">������̍ő啝 (�s�N�Z���P��)�B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,int width){
			return g.MeasureString(text,font,width);
		}
		/// <summary>
		/// �w�肵�����C�A�E�g�̈���Ɏw�肵�� System.Drawing.Font �ŕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="layoutArea">�e�L�X�g�̍ő僌�C�A�E�g�̈���w�肷�� System.Drawing.SizeF �\���́B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::SizeF layoutArea){
			return g.MeasureString(text,font,layoutArea);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font ���g�p���A�w�肵�� System.Drawing.StringFormat �ŏ����w�肵�ĕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="width">������̍ő啝�B</param>
		/// <param name="format">�s�ԂȂǁA������̏�������\�� System.Drawing.StringFormat�B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^����� stringFormat �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,int width,Gdi::StringFormat format){
			return g.MeasureString(text,font,width,format);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font ���g�p���A�w�肵�� System.Drawing.StringFormat �ŏ����w�肵�ĕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="origin">������̍������\�� System.Drawing.PointF �\���́B</param>
		/// <param name="stringFormat">�s�ԂȂǁA������̏�������\�� System.Drawing.StringFormat�B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^����� stringFormat �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::PointF origin,Gdi::StringFormat stringFormat){
			return g.MeasureString(text,font,origin,stringFormat);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font ���g�p���A�w�肵�� System.Drawing.StringFormat �ŏ����w�肵�ĕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="layoutArea">�e�L�X�g�̍ő僌�C�A�E�g�̈���w�肷�� System.Drawing.SizeF �\���́B</param>
		/// <param name="stringFormat"></param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^����� stringFormat �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
		public static Gdi::SizeF MeasureString(string text,Gdi::Font font,Gdi::SizeF layoutArea,Gdi::StringFormat stringFormat){
			return g.MeasureString(text,font,layoutArea,stringFormat);
		}
		/// <summary>
		/// �w�肵�� System.Drawing.Font ���g�p���A�w�肵�� System.Drawing.StringFormat �ŏ����w�肵�ĕ`�悵���ꍇ�́A�w�肵����������v�����܂��B
		/// </summary>
		/// <param name="text">�v�����镶����B</param>
		/// <param name="font">������̃e�L�X�g�`�����`���� System.Drawing.Font�B</param>
		/// <param name="layoutArea">�e�L�X�g�̍ő僌�C�A�E�g�̈���w�肷�� System.Drawing.SizeF �\���́B</param>
		/// <param name="stringFormat">�s�ԂȂǁA������̏�������\�� System.Drawing.StringFormat�B</param>
		/// <param name="charactersFitted">������̕������B</param>
		/// <param name="linesFilled">������̃e�L�X�g�s���B</param>
		/// <returns>���̃��\�b�h�́Atext �p�����[�^�Ɏw�肳�ꂽ������̃T�C�Y�� System.Drawing.Graphics.PageUnit �v���p�e�B�Ŏw�肳�ꂽ�P�ʂŕ\��
		/// System.Drawing.SizeF �\���̂��Afont �p�����[�^����� stringFormat �p�����[�^�ŕ`�悳�ꂽ�Ƃ���ɕԂ��܂��B</returns>
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
		/// �X�N���[���ւ̃A�N�Z�X�̃L���b�V���ł��B
		/// </summary>
		Gdi::Bitmap bmp;
		Gdi::Graphics bmp_g;
		Gdi::Imaging.BitmapData bmp_d;
		bool bmp_dirty=false;

		bool first=true;
		Gdi::Point current;
		/// <summary>
		/// ���݂̃X�N���[�����W���擾���͐ݒ肵�܂��B
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
		//		������
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
		//		�A�N�Z�X
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
			// screen ���W�֕ϊ�
			x+=g_ofs.X+(int)g.Transform.OffsetX;
			y+=g_ofs.Y+(int)g.Transform.OffsetY;

			// �L���b�V���̊m�F
			int dx=x-current.X;
			int dy=y-current.Y;
			if(first||dx<0||X_UNIT<=dx||dy<0||Y_UNIT<=dy){
				Current=new Gdi::Point(x&~X_MASK,y&~Y_MASK);
			}

			// �L���b�V�����W�֕ϊ�
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
