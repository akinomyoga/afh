using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;

namespace afh.Forms{
	internal sealed class ProgressDialogForm:System.Windows.Forms.Form{
		public System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.Label label;
		public System.Windows.Forms.PictureBox picture;
		public System.Windows.Forms.ProgressBar progress;
	
		public ProgressDialogForm(){
			this.InitializeComponent();
			this.DisableCloseButton();
			this.Closing+=new System.ComponentModel.CancelEventHandler(ProgressDialogForm_Closing);

			// ShowDialog 直前に InternalClose された時の為
			this.Load+=(System.EventHandler)delegate(object sender,System.EventArgs e){
				if(this.closing)this.Close();
			};
		}

		#region http://www.atmarkit.co.jp/fdotnet/dotnettips/142closebtn/closebtn.html
		// コントロールボックスの［閉じる］ボタンの無効化
		private void DisableCloseButton(){
#if WIN32
			System.IntPtr hMenu=GetSystemMenu(this.Handle,0);
			RemoveMenu(hMenu,SC_CLOSE,MF_BYCOMMAND);
#endif
		}
#if WIN32
		[Interop::DllImport("USER32.DLL")]
		private static extern System.IntPtr GetSystemMenu(System.IntPtr hWnd,System.UInt32 bRevert);
		[Interop::DllImport("USER32.DLL")]
		private static extern System.UInt32 RemoveMenu(System.IntPtr hMenu,System.UInt32 nPosition,System.UInt32 wFlags);
		// ［閉じる］ボタンを無効化するための値
		private const System.UInt32 SC_CLOSE = 0x0000F060;
		private const System.UInt32 MF_BYCOMMAND = 0x00000000;
#endif
		void ProgressDialogForm_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			if(!this.closing)e.Cancel=true;
		}
		#endregion

		#region Designer's Code
		private void InitializeComponent(){
			this.progress = new System.Windows.Forms.ProgressBar();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.picture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
			this.SuspendLayout();
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(12,63);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(347,24);
			this.progress.Step = 1;
			this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progress.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(267,93);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(92,23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "キャンセル(&C)";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label
			// 
			this.label.AutoEllipsis = true;
			this.label.Location = new System.Drawing.Point(66,9);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(293,48);
			this.label.TabIndex = 2;
			this.label.Text = "処理中です...";
			// 
			// picture
			// 
			this.picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picture.Location = new System.Drawing.Point(12,9);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(48,48);
			this.picture.TabIndex = 3;
			this.picture.TabStop = false;
			// 
			// ProgressDialogForm
			// 
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(371,120);
			this.Controls.Add(this.picture);
			this.Controls.Add(this.label);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.progress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressDialogForm";
			this.Text = "処理中";
			((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		void btnCancel_Click(object sender,System.EventArgs e) {
			this.InternalClose();
		}

		private bool closing=false;
		internal void InternalClose(){
			this.closing=true;
			if(this.InvokeRequired)
				this.Invoke(new afh.VoidCB(this.Close));
			else this.Close();
		}

		//=================================================
		//		表示情報
		//=================================================
		/// <summary>
		/// Maximum
		/// </summary>
		public int Maximum{
			set{
				if(this.InvokeRequired)
					this.Invoke(new System.Action<int>(setMaximum),value);
				else setMaximum(value);
			}
		}
		private void setMaximum(int value){
			this.progress.Maximum=value;
		}
		/// <summary>
		/// Minimum
		/// </summary>
		public int Minimum{
			set{
				if(this.InvokeRequired)
					this.Invoke(new System.Action<int>(setMinimum),value);
				else setMinimum(value);
			}
		}
		private void setMinimum(int value){
			this.progress.Minimum=value;
		}
		/// <summary>
		/// 現在の値を設定します。
		/// </summary>
		public int Value{
			set{
				if(this.InvokeRequired)
					this.Invoke(new System.Action<int>(setValue),value);
				else setValue(value);
			}
		}
		private void setValue(int value){
			this.progress.Value=value;
		}

		/// <summary>
		/// タイトルバーに表示される文字列を設定します。
		/// </summary>
		/// <param name="value">タイトルバーに表示する文字列を指定します。</param>
		public void SetTitle(string value){
			if(this.InvokeRequired)
				this.Invoke(new System.Action<string>(setTitle),value);
			else setTitle(value);
		}
		private void setTitle(string value){
			this.Text=value;
		}

		/// <summary>
		/// 現在の処理内容を説明する文字列を設定します。
		/// </summary>
		/// <param name="value">現在の処理内容を説明する文字列を指定します。</param>
		public void SetDescription(string value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<string>(setDescription),value);
			else setDescription(value);
		}
		private void setDescription(string value) {
			this.label.Text=value;
		}

		/// <summary>
		/// タイトルバーに表示されるアイコンを設定します。
		/// </summary>
		/// <param name="value">タイトルバーに表示するアイコンを指定します。</param>
		public void SetIcon(System.Drawing.Icon value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<System.Drawing.Icon>(setIcon),value);
			else setIcon(value);
		}
		private void setIcon(System.Drawing.Icon value) {
			this.Icon=value;
		}

		/// <summary>
		/// 説明の左側に表示する画像を設定します。
		/// </summary>
		/// <param name="value">説明の左側に表示する画像を指定します。</param>
		public void SetImage(System.Drawing.Image value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<System.Drawing.Image>(setImage),value);
			else setImage(value);
		}
		private void setImage(System.Drawing.Image value) {
			this.picture.Image=value;
		}

		/// <summary>
		/// キャンセルボタンを有効にするかどうかを設定します。
		/// </summary>
		/// <param name="value">キャンセルボタンを有効にする場合には true を指定します。
		/// キャンセルボタンを無効にする場合には false を指定します。</param>
		public void SetCanCancel(bool value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<bool>(setCanCancel),value);
			else setCanCancel(value);
		}
		private void setCanCancel(bool value) {
			this.btnCancel.Enabled=value;
		}
	}

	/// <summary>
	/// 長い処理の進行状況を表示する為のダイアログです。
	/// </summary>
	public sealed class ProgressDialog:System.ComponentModel.Component{
		private readonly object sync=new object();
		private ProgressDialogForm form;
		/// <summary>
		/// ProgressDialog を初期化します。
		/// </summary>
		public ProgressDialog(){}
		//=================================================
		//		進行状況
		//=================================================
		private int maximum=100;
		private int minimum=0;
		private int value=0;
		/// <summary>
		/// 進行状況の最小値を取得亦は設定します。
		/// </summary>
		public int ProgressMax{
			get{return this.maximum;}
			set{
				this.maximum=value;
				lock(this.sync)if(this.form!=null)
					this.form.Maximum=value;
			}
		}
		/// <summary>
		/// 進行状況の最大値を取得亦は設定します。
		/// </summary>
		public int ProgressMin {
			get{return this.minimum;}
			set{
				this.minimum=value;
				lock(this.sync)if(this.form!=null)
					this.form.Minimum=value;
			}
		}
		/// <summary>
		/// 現在の進行状況を取得亦は設定します。
		/// </summary>
		public int Progress{
			get{return this.value;}
			set{
				this.value=value;
				lock(this.sync)if(this.form!=null)
					this.form.Value=value;
			}
		}
		/// <summary>
		/// 現在の進行状況を百分率で取得します。
		/// </summary>
		public float Percentage{
			get{return this.value/(float)(this.maximum-this.minimum);}
		}
		//=================================================
		//		表示非表示
		//=================================================
		private bool showing=false;
		private System.Windows.Forms.IWin32Window owner;
		private const int DELAY=1000;
		/// <summary>
		/// ダイアログを表示します。
		/// </summary>
		public void Show(){
			this.Show(null);
		}
		/// <summary>
		/// ダイアログを表示します。
		/// </summary>
		/// <param name="owner">親 Window を指定します。</param>
		public void Show(System.Windows.Forms.IWin32Window owner){
			if(this.showing)return;
			this.showing=true;
			this.iscanceled=false;
			this.owner=owner;

			System.Threading.Thread thread=new System.Threading.Thread(new System.Threading.ThreadStart(StartShow));
			thread.Name="<ImageArrange>afh.ImageArrange.ProgressDialog 進捗状況の表示";
			thread.IsBackground=true;
			//thread.Priority=System.Threading.ThreadPriority.BelowNormal;
			thread.Start();
		}
		/// <summary>
		/// ダイアログを閉じます。
		/// </summary>
		public void Close(){
			this.showing=false;
			lock(this.sync)
				if(this.form!=null)
					this.form.InternalClose();
		}

		private void StartShow(){
			System.Threading.Thread.Sleep(DELAY/2);
			if(!this.showing)return;

			// 既に半分以上終わっている場合にはもう少し待ってみる
			if(this.Percentage>0.6){
				System.Threading.Thread.Sleep(DELAY/2);
				if(!this.showing)return;
			}

			this.form=new ProgressDialogForm();
			this.form.btnCancel.Click+=new System.EventHandler(btnCancel_Click);
			this.form.SetTitle(this.title);
			this.form.SetDescription(this.description);
			this.form.SetCanCancel(this.cancancel);
			this.form.SetImage(this.image);
			this.form.SetIcon(this.icon);
			this.form.Maximum=this.maximum;
			this.form.Minimum=this.minimum;
			this.form.Value=this.value;

			if(this.showing){
				if(this.owner!=null)
					this.form.ShowDialog(this.owner);
				else
					this.form.ShowDialog();
			}

			lock(this.sync){
				this.form.Dispose();
				this.form=null;
			}
		}
		//=================================================
		//		表示内容
		//=================================================
		private string title="処理中";
		private string description="処理中です...";
		private System.Drawing.Icon icon;
		private System.Drawing.Image image;
		private bool cancancel=true;
		/// <summary>
		/// タイトルバーに表示する文字列を取得亦は設定します。
		/// </summary>
		public string Title{
			get{return this.title;}
			set{
				this.title=value;
				if(this.form!=null)this.form.SetTitle(value);
			}
		}
		/// <summary>
		/// タイトルバーに表示するアイコンを指定します。
		/// </summary>
		public System.Drawing.Icon Icon{
			get{return this.icon;}
			set{
				this.icon=value;
				if(this.form!=null)this.form.SetIcon(value);
			}
		}
		/// <summary>
		/// 現在の処理の内容を説明する文字列を取得亦は設定します。
		/// </summary>
		public string Description{
			get{return this.description;}
			set{
				this.description=value;
				if(this.form!=null)this.form.SetDescription(value);
			}
		}
		/// <summary>
		/// 現在の処理の内容に関する画像を指定します。
		/// アニメーション gif 等も可能です。
		/// </summary>
		public System.Drawing.Image Image{
			get{return this.image;}
			set{
				this.image=value;
				if(this.form!=null)this.form.SetImage(value);
			}
		}
		/// <summary>
		/// ユーザーによるキャンセルが可能かどうかを指定します。
		/// </summary>
		public bool CanCancel{
			get{return this.cancancel;}
			set{
				this.cancancel=value;
				if(this.form!=null)this.form.SetCanCancel(value);
			}
		}
		//=================================================
		//		キャンセル
		//=================================================
		private void btnCancel_Click(object sender,System.EventArgs args){
			this.iscanceled=true;
			if(this.Canceled==null)return;
			this.Canceled(sender,args);
		}
		/// <summary>
		/// ユーザによって操作をキャンセルされた際に発生します。
		/// </summary>
		public event System.EventHandler Canceled;
		/// <summary>
		/// ユーザーによって操作がキャンセルされたかどうかを取得します。
		/// </summary>
		public bool IsCanceled{
			get{return this.iscanceled;}
		}private bool iscanceled;
	}
}
