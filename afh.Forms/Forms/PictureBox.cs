using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using Gen=System.Collections.Generic;
using Thr=System.Threading;
using CM=System.ComponentModel;

namespace afh.Forms{
	// TODO: InitialImage
	// TODO: ErrorImage
	/// <summary>
	/// �摜��\������ׂ̃R���g���[���ł��B
	/// </summary>
	public class PictureBox:Forms::Control{
		Gdi::Image bmp=null;
		bool bmp_autodel=false;

		/// <summary>
		/// PictureBox �̃R���X�g���N�^�ł��B
		/// </summary>
		public PictureBox(){}
		/// <summary>
		/// Paint �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� PaintEventArgs�B</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			this.Draw(e.Graphics);
			base.OnPaint(e);
		}
		/// <summary>
		/// PictureBox �ɂ���Ďg�p����Ă��邷�ׂẴ��\�[�X��������܂��B
		/// �I�v�V�����ŁA�}�l�[�W ���\�[�X��������܂��B
		/// </summary>
		/// <param name="disposing">�}�l�[�W ���\�[�X�ƃA���}�l�[�W ���\�[�X�̗������������ꍇ�� true�B
		/// �A���}�l�[�W ���\�[�X�������������ꍇ�� false�B</param>
		protected override void Dispose(bool disposing){
			this.Animate(false);

			if(this.bmp!=null&&this.bmp_autodel)
				this.bmp.Dispose();

			if(this.buff_g!=null)this.buff_g.Dispose();
			if(this.anim_g!=null)this.anim_g.Dispose();
			if(this.buff_ctx!=null)this.buff_ctx.Dispose();

			base.Dispose(disposing);
		}
		/// <summary>
		/// VisibleChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnVisibleChanged(System.EventArgs e){
			base.OnVisibleChanged(e);
			this.Animate();
		}
		/// <summary>
		/// EnabledChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnEnabledChanged(System.EventArgs e){
			base.OnEnabledChanged(e);
			this.Animate();
		}
		/// <summary>
		/// ParentChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnParentChanged(System.EventArgs e){
			base.OnParentChanged(e);
			this.Animate();
		}
		/// <summary>
		/// SizeChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnSizeChanged(System.EventArgs e){
			base.OnSizeChanged(e);

			if(this.anim_g!=null)anim_g.Dispose();
			this.anim_g=null;

			this.UpdateGeometry();
		}
		/// <summary>
		/// DockChanged �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnDockChanged(System.EventArgs e){
			base.OnDockChanged(e);
			this.UpdateGeometry();
		}

		/// <summary>
		/// �\������摜���擾���͐ݒ肵�܂��B
		/// </summary>
		public Gdi::Image Image{
			get{return this.bmp;}
			set{
				if(this.bmp==value)return;

				this.Animate(false);

				// �����Ń��[�h�����r�b�g�}�b�v�̔j��
				if(this.bmp_autodel){
					this.bmp.Dispose();
					this.bmp_autodel=false;
				}

				this.bmp=value;
				this.center_x=0.5f;
				this.center_y=0.5f;
				this.mag=1f;
				this.UpdateGeometry();
				this.Animate(true);
				//using(Gdi::Graphics g=this.CreateGraphics())this.Draw(g);
			}
		}


		private Gdi::RectangleF rcDest=new System.Drawing.Rectangle(0,0,1,1);
		private Gdi::RectangleF rcSrc=new System.Drawing.Rectangle(0,0,1,1);
		private void Draw(Gdi::Graphics g){
			if(this.bmp!=null){
				//g.DrawLine(Gdi::Pens.Blue,0,0,100,100);
				/*
				g.DrawImageUnscaledAndClipped(
					bmp,
					new Gdi::Rectangle(0,0,this.Width,this.Height)
					);
				//*/
				g.Clear(this.BackColor);
				g.DrawImage(this.bmp,rcDest,rcSrc,Gdi::GraphicsUnit.Pixel);
			}
		}

		//============================================================
		//		���[�h
		//============================================================
		/// <summary>
		/// �摜���t�@�C���p�X���w�肵�ēǂݍ��݂܂��B
		/// </summary>
		/// <param name="path">�ǂݍ��މ摜�t�@�C���ւ̃p�X���w�肵�܂��B</param>
		public void LoadImageFromFile(string path){
			try{
				this.Image=Gdi::Bitmap.FromFile(path);
				this.bmp_autodel=true;
			}catch{
				// TODO: ErrorImage
				this.Image=null;
			}
		}
		//============================================================
		//		�\���`��
		//============================================================
		/// <summary>
		/// �\������傫���̌���̎d����\�����܂��B
		/// </summary>
		public enum SizeModeType{
			/// <summary>
			/// PictureBox �̑傫���ɍ��킹�ĉ摜���g�k���ĕ\�����܂��B
			/// </summary>
			Stretch,
			/// <summary>
			/// �w�肵���g�嗦�ŁA�w�肵���ʒu�ɕ\�����郂�[�h�ł��B
			/// </summary>
			Magnify,
			/// <summary>
			/// �w�肵���g�嗦�ŁA�w�肵���ʒu�ɕ\�����郂�[�h�ł��B
			/// �\���͈͂ɗ]�T������ꍇ�ɂ͏o���邾���S�̂�\�����܂��B
			/// ��̓I�ɂ́A�摜���N���C�A���g�̈�Ɏ��܂�ꍇ�ɂ͒����ɕ\�����A
			/// �摜���N���C�A���g�̈�Ɏ��܂�Ȃ��ꍇ�ŁA���̗]����������l�Ȑݒ�̎��͂��̕������l�߂ĕ\�����܂��B
			/// </summary>
			MagnifyAutoPosition,
			/// <summary>
			/// PictureBox ���̂̑傫�����摜�ɍ��킹�ĕω������܂��B
			/// </summary>
			AutoSize,
		}
		private SizeModeType size_mode=SizeModeType.Stretch;
		/// <summary>
		/// �\������傫�������肷����@���擾���͐ݒ肵�܂��B
		/// </summary>
		public SizeModeType SizeMode{
			get{return size_mode;}
			set{
				if(this.size_mode==value)return;
				this.size_mode=value;
				this.UpdateGeometry();
			}
		}
		private bool IsSizeModeMagnify{
			get{return this.size_mode==SizeModeType.Magnify||this.size_mode==SizeModeType.MagnifyAutoPosition;}
		}
		//------------------------------------------------------------
		bool limit_mag_full=false;
		/// <summary>
		/// SizeMode �� Stretch �̏ꍇ�Ɋg�嗦��������ɐ������܂��B
		/// SizeModeType.Magnify �ɉ����Ď蓮�Ŋg�嗦��ݒ肷��ꍇ�ɂ͉��̉e�����^���܂���B
		/// </summary>
		[CM::Description("SizeMode �� Stretch �̏ꍇ�ɁA�g�嗦��������ɐ������܂��B")]
		[CM::DefaultValue(false)]
		public bool LimitMagnificationFullsize{
			get{return this.limit_mag_full;}
			set{
				if(this.limit_mag_full==value)return;
				this.limit_mag_full=value;
				this.UpdateGeometry();
			}
		}
		/// <summary>
		/// SizeMode �� Stretch �̏ꍇ�ɗL���ł��B
		/// �摜�̏c�����ۑ����܂��B
		/// </summary>
		bool fix_aspect=true;
		/// <summary>
		/// �摜�̏c������Œ肷�邩�ۂ����w�肵�܂��B
		/// SizeMode �� Stretch �̏ꍇ�ɗL���ł��B
		/// </summary>
		[CM::Description("SizeMode �� Stretch �̏ꍇ�ɁA�摜�̏c�����ۑ����܂�")]
		[CM::DefaultValue(true)]
		public bool FixAspectRatio{
			get{return this.fix_aspect;}
			set{
				if(this.fix_aspect==value)return;
				this.fix_aspect=value;
				this.UpdateGeometry();
			}
		}
		//------------------------------------------------------------
		float mag=1;
		/// <summary>
		/// SizeModeType.Magnify, SizeModeType.MagnifyAutoPosition, SizeModeType.AutoSize �ɉ�����A
		/// �摜�̊g�嗦���w�肵�܂��B
		/// </summary>
		[CM::Description("�摜�̊g�嗦���w�肵�܂��B")]
		[CM::DefaultValue(1.0f)]
		public float Magnification{
			get{return this.mag;}
			set{
				if(this.mag==value)return;
				this.mag=value;
				if(this.IsSizeModeMagnify||this.size_mode==SizeModeType.AutoSize)
					this.UpdateGeometry();
			}
		}
		private float center_x=.5f;
		/// <summary>
		/// �g��̒��S�� X ���W [0.0-1.0] ���擾���͐ݒ肵�܂��B
		/// </summary>
		[CM::Description("�g��̒��S�� X ���W [0.0-1.0] ���擾���͐ݒ肵�܂��B")]
		[CM::DefaultValue(.5f)]
		public float MagCenterX{
			get{return this.center_x;}
			set{
				if(value<0)value=0;else if(value>1)value=1;
				if(this.center_x==value)return;
				this.center_x=value;
				if(this.IsSizeModeMagnify)
					this.UpdateGeometry();
			}
		}
		private float center_y=.5f;
		/// <summary>
		/// �g��̒��S�� Y ���W [0.0-1.0] ���擾���͐ݒ肵�܂��B
		/// </summary>
		[CM::Description("�g��̒��S�� Y ���W [0.0-1.0] ���擾���͐ݒ肵�܂��B")]
		[CM::DefaultValue(.5f)]
		public float MagCenterY{
			get{return this.center_y;}
			set{
				if(value<0)value=0;else if(value>1)value=1;
				if(this.center_y==value)return;
				this.center_y=value;
				if(this.IsSizeModeMagnify)
					this.UpdateGeometry();
			}
		}
		//------------------------------------------------------------
#if OLD
		[System.Obsolete]
		bool auto_size=false;
		/// <summary>
		/// PictureBox ���̂̑傫�����摜�ɍ��킹�ĕω������邩�ۂ���ݒ肵�܂��B
		/// </summary>
		[CM::DefaultValue(false)]
		[System.Obsolete]
		public new bool AutoSize{
			get{return this.auto_size;}
			set{
				if(this.auto_size==value)return;
				this.auto_size=value;
				this.UpdateGeometry();
			}
		}
#endif
		//------------------------------------------------------------
		private void UpdateGeometry(){
			if(this.bmp==null)return;

			switch(this.size_mode){
				case SizeModeType.Stretch:{
					// �S�̕\���̏ꍇ
					float mag_w=this.Width/(float)bmp.Width;
					float mag_h=this.Height/(float)bmp.Height;

					if(limit_mag_full){
						if(mag_w>1)mag_w=1;
						if(mag_h>1)mag_h=1;
					}

					if(fix_aspect){
						if(mag_h<mag_w)mag_w=mag_h;
						if(mag_w<mag_h)mag_h=mag_w;
					}

					float w=bmp.Width*mag_w;
					float h=bmp.Height*mag_h;
					rcDest=new Gdi::RectangleF((this.Width-w)/2,(this.Height-h)/2,w,h);
					rcSrc=new Gdi::RectangleF(0,0,bmp.Width,bmp.Height);
					break;
				}
				case SizeModeType.Magnify:{
					// �g��\���̏ꍇ

					// �ʒu���킹 X
					float w,l,cw,cl;
					w=this.Width/mag;
					if(w<bmp.Width){ // �͂ݏo��ꍇ
						cw=this.Width;
						l=bmp.Width*center_x-w/2;
						cl=(this.Width-cw)/2f;
					}else{ // ���܂�ꍇ
						w=bmp.Width;
						cw=w*mag;
						l=0;
						cl=this.Width/2f-cw*center_x;
					}

					// �ʒu���킹 Y
					float h,t,ch,ct;
					h=this.Height/mag;
					if(h<bmp.Height){ // �͂ݏo��
						ch=this.Height;
						t=bmp.Height*center_y-h/2;
						ct=(this.Height-ch)/2f;
					}else{ // ���܂�
						h=bmp.Height;
						ch=h*mag;
						t=0;
						ct=this.Height/2f-ch*center_y;
					}

					rcSrc=new Gdi::RectangleF(l,t,w,h);
					rcDest=new Gdi::RectangleF(cl,ct,cw,ch);
					break;
				}
				case SizeModeType.MagnifyAutoPosition:{
					// �؂�o���� X
					float w=this.Width/mag;
					float l;
					if(w<this.bmp.Width){
						// �͂ݏo��ꍇ
						l=ksh.Compare.Clamp(bmp.Width*center_x-w/2,0,bmp.Width-w);
					}else{
						// ���܂�ꍇ
						w=this.bmp.Width;
						l=0;
					}

					// �؂�o���� Y
					float h=this.Height/mag;
					float t;
					if(w<this.bmp.Height){
						// �͂ݏo��ꍇ
						t=ksh.Compare.Clamp(bmp.Height*center_y-h/2,0,bmp.Height-h);
					}else{
						// ���܂�ꍇ
						h=this.bmp.Height;
						t=0;
					}
					rcSrc=new Gdi::RectangleF(l,t,w,h);

					w*=mag;
					h*=mag;
					l=(this.Width-w)/2;
					t=(this.Height-h)/2;
					rcDest=new Gdi::RectangleF(l,t,w,h);
					break;
				}
				case SizeModeType.AutoSize:
					rcSrc=new Gdi::RectangleF(0,0,this.bmp.Width,this.bmp.Height);
					rcDest=new Gdi::RectangleF(0,0,bmp.Width*mag,bmp.Height*mag);

					// ���g�̑傫���̕ύX
					Gdi::Size sz=new Gdi::Size((int)rcDest.Width,(int)rcDest.Height);
					if(this.Dock==Forms::DockStyle.None&&this.Size!=sz){
						this.Size=sz;
						return; // SizeChanged ����ēx�Ăяo�����̂ł��̐܂� Invalidate
					}
					break;
			}

			this.Invalidate();
		}
		//============================================================
		//		�A�j���[�V����
		//============================================================
		bool animating=false;
		private void Animate(){
			this.Animate(!this.DesignMode&&this.Visible&&this.Enabled&&this.Parent!=null);
		}
		private void Animate(bool animate){
			if(animating==animate)return;

			if(animate){
				if(this.bmp==null||!ImageAnimator.CanAnimate(this.bmp))return;
				if(this.DesignMode||!this.Visible||!this.Enabled||this.Parent==null)return;
				ImageAnimator.Animate(this.bmp,imageAnimator_Animate);
			}else{
				ImageAnimator.StopAnimate(this.bmp,imageAnimator_Animate);
			}
			animating=animate;
		}
		private void imageAnimator_Animate(object sender,System.EventArgs e){
			if(!this.animating)return;

			if(this.InvokeRequired){
				this.BeginInvoke((System.EventHandler)this.imageAnimator_Animate,sender,e);
				return;
			}

			ImageAnimator.UpdateFrames(this.bmp);

			this.Draw(GBuff.Graphics);
			GBuff.Render(anim_g);
		}
		Gdi::BufferedGraphicsContext buff_ctx=new Gdi::BufferedGraphicsContext();
		Gdi::Graphics anim_g=null;
		Gdi::BufferedGraphics buff_g=null;
		private Gdi::BufferedGraphics GBuff{
			get{
				if(anim_g==null){
					anim_g=this.CreateGraphics();
					if(buff_g!=null)buff_g.Dispose();
					buff_g=buff_ctx.Allocate(
						anim_g,
						new Gdi::Rectangle(Gdi::Point.Empty,this.Size)
						);
				}
				return buff_g;
			}
		}

#if FALSE
		protected override void WndProc(ref System.Windows.Forms.Message m){
			base.WndProc(ref m);
			if(m.Result==(System.IntPtr)0)return;

			if(m.Msg==(int)mwg.Win32.WM.PAINT){

				Gdi::Graphics g=this.CreateGraphics();
				g.Clear(Gdi::Color.Red);
				g.Dispose();

				m.Result=(System.IntPtr)0;
			}
		}
#endif
	}

	// TODO: �Đ����x�̕ύX -> ImageInfo �� AddFrameTimer / frameTimer �� float ��
	// TODO: �X�V�p�x�̕ύX -> AnimateImages50ms �̒��̒l��ύX
	public static class ImageAnimator{
		private static System.Threading.Thread animationThread;
#if USE_ANYFRAMEDIRTY
		private static bool anyFrameDirty;
#endif
		private static Gen::List<ImageInfo> imageInfoList;
		private static System.Threading.ReaderWriterLock rwImgListLock=new Thr::ReaderWriterLock();

		/// <summary>
		/// ���݂̃X���b�h�ɉ�����擾�葱������ WriterLock �̐��𐔂��܂��B
		/// </summary>
		private class WriterLockWait:System.IDisposable{
			[System.ThreadStatic]
			public static int Count;

			public WriterLockWait(){Count++;}
			public void Dispose(){Count--;}
		}

		public static void Animate(Gdi::Image image,System.EventHandler onFrameChangedHandler) {
			if(image==null)return;

			ImageInfo item=null;
			lock(image)item=new ImageInfo(image);

			StopAnimate(image,onFrameChangedHandler);

			WriterLock w_lock;
			using(new WriterLockWait())
				w_lock=new WriterLock(rwImgListLock);
				
			using(w_lock){
				if(!item.animated)return;

				if(imageInfoList==null){
					imageInfoList=new Gen::List<ImageInfo>();
				}
				item.onFrameChangedHandler=onFrameChangedHandler;
				imageInfoList.Add(item);
				if(animationThread==null) {
					animationThread=new Thr::Thread(new Thr::ThreadStart(ImageAnimator.AnimateImages50ms));
					animationThread.Name=typeof(ImageAnimator).Name;
					animationThread.IsBackground=true;
					animationThread.Start();
				}
			}
		}

		private static void AnimateImages50ms(){
			while(true){
				using(new ReaderLock(rwImgListLock)){
					foreach(ImageInfo info in imageInfoList){
						info.AddFrameTimer(5);
#if USE_ANYFRAMEDIRTY
						if(info.FrameDirty)
							anyFrameDirty=true;
#endif
					}
				}
				Thr::Thread.Sleep(50);
			}
		}

		public static bool CanAnimate(Gdi::Image image){
			if(image==null)return false;

			lock(image){
				foreach(System.Guid guid in image.FrameDimensionsList){
					Gdi::Imaging.FrameDimension dimension=new Gdi::Imaging.FrameDimension(guid);
					if(dimension.Equals(Gdi::Imaging.FrameDimension.Time)){
						return image.GetFrameCount(Gdi::Imaging.FrameDimension.Time)>1;
					}
				}
			}
			return false;
		}

		public static void StopAnimate(Gdi::Image image,System.EventHandler onFrameChangedHandler) {
			if(image==null||imageInfoList==null)return;

			WriterLock w_lock;
			using(new WriterLockWait())
				w_lock=new WriterLock(rwImgListLock);

			using(w_lock){
				ImageInfo item=imageInfoList.Find(delegate(ImageInfo info){return image==info.Image;});
				if(item==null)return;
				if(onFrameChangedHandler==item.onFrameChangedHandler||onFrameChangedHandler!=null&&onFrameChangedHandler.Equals(item.onFrameChangedHandler)) {
					imageInfoList.Remove(item);
				}
			}
		}

#if USE_ANYFRAMEDIRTY
		public static void UpdateFrames(){
			if(!anyFrameDirty||imageInfoList==null||WriterLockWait.Count>0)return;

			using(new ReaderLock(rwImgListLock)){
				foreach(ImageInfo info in imageInfoList){
					info.UpdateFrame();
				}
				anyFrameDirty=false;
			}
		}
		public static void UpdateFrames(Gdi::Image image){
			if(!anyFrameDirty||image==null||imageInfoList==null||WriterLockWait.Count>0)return;

			using(new ReaderLock(rwImgListLock)){
				bool f_isdirty=false;
				bool f_updated=false;
				foreach(ImageInfo info in imageInfoList){
					if(info.Image==image){
						info.UpdateFrame();
						f_updated=true;
					}
					if(info.FrameDirty)f_isdirty=true;
					if(f_isdirty&&f_updated)break;
				}
				anyFrameDirty=f_isdirty;
			}
		}
#else
		public static void UpdateFrames(){
			if(imageInfoList==null||WriterLockWait.Count>0)return;

			using(new ReaderLock(rwImgListLock)){
				foreach(ImageInfo info in imageInfoList){
					info.UpdateFrame();
				}
			}
		}
		public static void UpdateFrames(Gdi::Image image){
			if(image==null||imageInfoList==null||WriterLockWait.Count>0)return;

			using(new ReaderLock(rwImgListLock)){
				foreach(ImageInfo info in imageInfoList){
					if(info.Image==image){
						info.UpdateFrame();
						break;
					}
				}
			}
		}
#endif

		/// <summary>
		/// ������ WriterLock ���������ׂ̃N���X�ł��B
		/// </summary>
		private class WriterLock:System.IDisposable{
			readonly Thr::ReaderWriterLock rw_lock;
			Thr::LockCookie cookie=new Thr::LockCookie();
			readonly bool reader_presence;

			public WriterLock(Thr::ReaderWriterLock rw_lock){
				this.rw_lock=rw_lock;
				this.reader_presence=rw_lock.IsReaderLockHeld;

				if(reader_presence){
					cookie=rw_lock.UpgradeToWriterLock(-1);
				}else{
					rw_lock.AcquireWriterLock(-1);
				}
			}
			public void Dispose(){
				if(reader_presence){
					rw_lock.DowngradeFromWriterLock(ref cookie);
				} else {
					rw_lock.ReleaseWriterLock();
				}
			}
		}
		/// <summary>
		/// ������ ReaderLock ���������ׂ̃N���X�ł��B
		/// </summary>
		private struct ReaderLock:System.IDisposable{
			readonly Thr::ReaderWriterLock rw_lock;
			public ReaderLock(Thr::ReaderWriterLock rw_lock){
				this.rw_lock=rw_lock;
				rw_lock.AcquireReaderLock(-1);
			}
			public void Dispose(){
				rw_lock.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// �������Ă���摜�̏����Ǘ����܂��B
		/// </summary>
		private class ImageInfo{
			public readonly bool animated;
			private Gdi::Image image;
			internal Gdi::Image Image {
				get {return this.image;}
			}

			private readonly int frameCount;
			private int[] frameDelay;
			private const int PROPERTY_TagFrameDelay=0x5100;

			public ImageInfo(Gdi::Image image){
				this.image=image;
				this.animated=ImageAnimator.CanAnimate(image);
				if(this.animated) {
					this.frameCount=image.GetFrameCount(Gdi::Imaging.FrameDimension.Time);
					Gdi::Imaging.PropertyItem propertyItem=image.GetPropertyItem(PROPERTY_TagFrameDelay);
					if(propertyItem!=null) {
						byte[] buffer=propertyItem.Value;
						this.frameDelay=new int[this.frameCount];
						for(int i=0;i<this.frameCount;i++) {
							int val=buffer[i*4]|buffer[i*4+1]<<8|buffer[i*4+2]<<16|buffer[i*4+3]<<24;
							this.frameDelay[i]=val>0?val:1; // �ŏ��ł� 1
						}
					}
				} else {
					this.frameCount=1;
				}
				if(this.frameDelay==null){
					this.frameDelay=new int[this.frameCount];
				}
			}

			internal void UpdateFrame(){
				if(!this.frameDirty)return;

				lock(this.image){
					this.image.SelectActiveFrame(Gdi::Imaging.FrameDimension.Time,this.Frame);
					this.frameDirty=false;
				}
			}

			//========================================================
			//		���݂̃t���[��
			//========================================================
			private int frame;
			private int Frame{
				get{return this.frame;}
				set{
					if(this.frame==value)return;

					if(value<0||value>=this.frameCount){
						throw new System.ArgumentException("�����ȃt���[���̔ԍ��ł��B InvalidFrame","value");
					}
					if(this.animated){
						this.frame=value;
						this.frameDirty=true;
						this.OnFrameChanged(System.EventArgs.Empty);
					}
				}
			}
			private bool frameDirty;
			public bool FrameDirty {
				get {return this.frameDirty;}
			}
			public System.EventHandler onFrameChangedHandler;
			protected void OnFrameChanged(System.EventArgs e){
				if(this.onFrameChangedHandler==null)return;
				this.onFrameChangedHandler(this.image,e);
			}
			//========================================================
			//		�t���[���̍����ւ�
			//========================================================
			private int frameTimer;
			/// <summary>
			/// FrameTimer �ɒl��ǉ����܂��B
			/// </summary>
			/// <param name="value">�o�ߎ��Ԃ� 10ms ��P�� ? �Ƃ��Ďw�肵�܂��B</param>
			public void AddFrameTimer(int value){
				if(value<=0)return;
				this.frameTimer+=value;

				while(this.frameTimer>=CurrentFrameSpan){
					this.frameTimer-=CurrentFrameSpan; //=0;
					this.NextFrame();
				}
			}

			private int CurrentFrameSpan{
				get{return this.frameDelay[frame];}
			}
			/// <summary>
			/// ���̃t���[���Ɉړ����܂��B
			/// </summary>
			private void NextFrame(){
				this.Frame=(this.frame+1)%this.frameCount;
			}
		}
	}
}