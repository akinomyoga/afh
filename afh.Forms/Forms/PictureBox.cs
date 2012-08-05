using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using Gen=System.Collections.Generic;
using Thr=System.Threading;
using CM=System.ComponentModel;

namespace afh.Forms{
	// TODO: InitialImage
	// TODO: ErrorImage
	/// <summary>
	/// 画像を表示する為のコントロールです。
	/// </summary>
	public class PictureBox:Forms::Control{
		Gdi::Image bmp=null;
		bool bmp_autodel=false;

		/// <summary>
		/// PictureBox のコンストラクタです。
		/// </summary>
		public PictureBox(){}
		/// <summary>
		/// Paint イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している PaintEventArgs。</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			this.Draw(e.Graphics);
			base.OnPaint(e);
		}
		/// <summary>
		/// PictureBox によって使用されているすべてのリソースを解放します。
		/// オプションで、マネージ リソースも解放します。
		/// </summary>
		/// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。
		/// アンマネージ リソースだけを解放する場合は false。</param>
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
		/// VisibleChanged イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnVisibleChanged(System.EventArgs e){
			base.OnVisibleChanged(e);
			this.Animate();
		}
		/// <summary>
		/// EnabledChanged イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnEnabledChanged(System.EventArgs e){
			base.OnEnabledChanged(e);
			this.Animate();
		}
		/// <summary>
		/// ParentChanged イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnParentChanged(System.EventArgs e){
			base.OnParentChanged(e);
			this.Animate();
		}
		/// <summary>
		/// SizeChanged イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnSizeChanged(System.EventArgs e){
			base.OnSizeChanged(e);

			if(this.anim_g!=null)anim_g.Dispose();
			this.anim_g=null;

			this.UpdateGeometry();
		}
		/// <summary>
		/// DockChanged イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnDockChanged(System.EventArgs e){
			base.OnDockChanged(e);
			this.UpdateGeometry();
		}

		/// <summary>
		/// 表示する画像を取得又は設定します。
		/// </summary>
		public Gdi::Image Image{
			get{return this.bmp;}
			set{
				if(this.bmp==value)return;

				this.Animate(false);

				// 自分でロードしたビットマップの破棄
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
		//		ロード
		//============================================================
		/// <summary>
		/// 画像をファイルパスを指定して読み込みます。
		/// </summary>
		/// <param name="path">読み込む画像ファイルへのパスを指定します。</param>
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
		//		表示形式
		//============================================================
		/// <summary>
		/// 表示する大きさの決定の仕方を表現します。
		/// </summary>
		public enum SizeModeType{
			/// <summary>
			/// PictureBox の大きさに合わせて画像を拡縮して表示します。
			/// </summary>
			Stretch,
			/// <summary>
			/// 指定した拡大率で、指定した位置に表示するモードです。
			/// </summary>
			Magnify,
			/// <summary>
			/// 指定した拡大率で、指定した位置に表示するモードです。
			/// 表示範囲に余裕がある場合には出来るだけ全体を表示します。
			/// 具体的には、画像がクライアント領域に収まる場合には中央に表示し、
			/// 画像がクライアント領域に収まらない場合で、縁の余白が見える様な設定の時はその部分を詰めて表示します。
			/// </summary>
			MagnifyAutoPosition,
			/// <summary>
			/// PictureBox 自体の大きさを画像に合わせて変化させます。
			/// </summary>
			AutoSize,
		}
		private SizeModeType size_mode=SizeModeType.Stretch;
		/// <summary>
		/// 表示する大きさを決定する方法を取得又は設定します。
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
		/// SizeMode が Stretch の場合に拡大率を原寸大に制限します。
		/// SizeModeType.Magnify に於いて手動で拡大率を設定する場合には何の影響も与えません。
		/// </summary>
		[CM::Description("SizeMode が Stretch の場合に、拡大率を原寸大に制限します。")]
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
		/// SizeMode が Stretch の場合に有効です。
		/// 画像の縦横比を保存します。
		/// </summary>
		bool fix_aspect=true;
		/// <summary>
		/// 画像の縦横比を固定するか否かを指定します。
		/// SizeMode が Stretch の場合に有効です。
		/// </summary>
		[CM::Description("SizeMode が Stretch の場合に、画像の縦横比を保存します")]
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
		/// SizeModeType.Magnify, SizeModeType.MagnifyAutoPosition, SizeModeType.AutoSize に於ける、
		/// 画像の拡大率を指定します。
		/// </summary>
		[CM::Description("画像の拡大率を指定します。")]
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
		/// 拡大の中心の X 座標 [0.0-1.0] を取得又は設定します。
		/// </summary>
		[CM::Description("拡大の中心の X 座標 [0.0-1.0] を取得又は設定します。")]
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
		/// 拡大の中心の Y 座標 [0.0-1.0] を取得又は設定します。
		/// </summary>
		[CM::Description("拡大の中心の Y 座標 [0.0-1.0] を取得又は設定します。")]
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
		/// PictureBox 自体の大きさを画像に合わせて変化させるか否かを設定します。
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
					// 全体表示の場合
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
					// 拡大表示の場合

					// 位置合わせ X
					float w,l,cw,cl;
					w=this.Width/mag;
					if(w<bmp.Width){ // はみ出る場合
						cw=this.Width;
						l=bmp.Width*center_x-w/2;
						cl=(this.Width-cw)/2f;
					}else{ // 収まる場合
						w=bmp.Width;
						cw=w*mag;
						l=0;
						cl=this.Width/2f-cw*center_x;
					}

					// 位置合わせ Y
					float h,t,ch,ct;
					h=this.Height/mag;
					if(h<bmp.Height){ // はみ出る
						ch=this.Height;
						t=bmp.Height*center_y-h/2;
						ct=(this.Height-ch)/2f;
					}else{ // 収まる
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
					// 切り出し元 X
					float w=this.Width/mag;
					float l;
					if(w<this.bmp.Width){
						// はみ出る場合
						l=ksh.Compare.Clamp(bmp.Width*center_x-w/2,0,bmp.Width-w);
					}else{
						// 収まる場合
						w=this.bmp.Width;
						l=0;
					}

					// 切り出し元 Y
					float h=this.Height/mag;
					float t;
					if(w<this.bmp.Height){
						// はみ出る場合
						t=ksh.Compare.Clamp(bmp.Height*center_y-h/2,0,bmp.Height-h);
					}else{
						// 収まる場合
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

					// 自身の大きさの変更
					Gdi::Size sz=new Gdi::Size((int)rcDest.Width,(int)rcDest.Height);
					if(this.Dock==Forms::DockStyle.None&&this.Size!=sz){
						this.Size=sz;
						return; // SizeChanged から再度呼び出されるのでその折に Invalidate
					}
					break;
			}

			this.Invalidate();
		}
		//============================================================
		//		アニメーション
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

	// TODO: 再生速度の変更 -> ImageInfo の AddFrameTimer / frameTimer を float に
	// TODO: 更新頻度の変更 -> AnimateImages50ms の中の値を変更
	public static class ImageAnimator{
		private static System.Threading.Thread animationThread;
#if USE_ANYFRAMEDIRTY
		private static bool anyFrameDirty;
#endif
		private static Gen::List<ImageInfo> imageInfoList;
		private static System.Threading.ReaderWriterLock rwImgListLock=new Thr::ReaderWriterLock();

		/// <summary>
		/// 現在のスレッドに於ける取得手続き中の WriterLock の数を数えます。
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
		/// 自動で WriterLock を解放する為のクラスです。
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
		/// 自動で ReaderLock を解放する為のクラスです。
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
		/// 動かしている画像の情報を管理します。
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
							this.frameDelay[i]=val>0?val:1; // 最小でも 1
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
			//		現在のフレーム
			//========================================================
			private int frame;
			private int Frame{
				get{return this.frame;}
				set{
					if(this.frame==value)return;

					if(value<0||value>=this.frameCount){
						throw new System.ArgumentException("無効なフレームの番号です。 InvalidFrame","value");
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
			//		フレームの差し替え
			//========================================================
			private int frameTimer;
			/// <summary>
			/// FrameTimer に値を追加します。
			/// </summary>
			/// <param name="value">経過時間を 10ms を単位 ? として指定します。</param>
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
			/// 次のフレームに移動します。
			/// </summary>
			private void NextFrame(){
				this.Frame=(this.frame+1)%this.frameCount;
			}
		}
	}
}