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

			// ShowDialog ���O�� InternalClose ���ꂽ���̈�
			this.Load+=(System.EventHandler)delegate(object sender,System.EventArgs e){
				if(this.closing)this.Close();
			};
		}

		#region http://www.atmarkit.co.jp/fdotnet/dotnettips/142closebtn/closebtn.html
		// �R���g���[���{�b�N�X�́m����n�{�^���̖�����
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
		// �m����n�{�^���𖳌������邽�߂̒l
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
			this.btnCancel.Text = "�L�����Z��(&C)";
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
			this.label.Text = "�������ł�...";
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
			this.Text = "������";
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
		//		�\�����
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
		/// ���݂̒l��ݒ肵�܂��B
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
		/// �^�C�g���o�[�ɕ\������镶�����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�^�C�g���o�[�ɕ\�����镶������w�肵�܂��B</param>
		public void SetTitle(string value){
			if(this.InvokeRequired)
				this.Invoke(new System.Action<string>(setTitle),value);
			else setTitle(value);
		}
		private void setTitle(string value){
			this.Text=value;
		}

		/// <summary>
		/// ���݂̏������e��������镶�����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">���݂̏������e��������镶������w�肵�܂��B</param>
		public void SetDescription(string value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<string>(setDescription),value);
			else setDescription(value);
		}
		private void setDescription(string value) {
			this.label.Text=value;
		}

		/// <summary>
		/// �^�C�g���o�[�ɕ\�������A�C�R����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�^�C�g���o�[�ɕ\������A�C�R�����w�肵�܂��B</param>
		public void SetIcon(System.Drawing.Icon value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<System.Drawing.Icon>(setIcon),value);
			else setIcon(value);
		}
		private void setIcon(System.Drawing.Icon value) {
			this.Icon=value;
		}

		/// <summary>
		/// �����̍����ɕ\������摜��ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�����̍����ɕ\������摜���w�肵�܂��B</param>
		public void SetImage(System.Drawing.Image value) {
			if(this.InvokeRequired)
				this.Invoke(new System.Action<System.Drawing.Image>(setImage),value);
			else setImage(value);
		}
		private void setImage(System.Drawing.Image value) {
			this.picture.Image=value;
		}

		/// <summary>
		/// �L�����Z���{�^����L���ɂ��邩�ǂ�����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�L�����Z���{�^����L���ɂ���ꍇ�ɂ� true ���w�肵�܂��B
		/// �L�����Z���{�^���𖳌��ɂ���ꍇ�ɂ� false ���w�肵�܂��B</param>
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
	/// ���������̐i�s�󋵂�\������ׂ̃_�C�A���O�ł��B
	/// </summary>
	public sealed class ProgressDialog:System.ComponentModel.Component{
		private readonly object sync=new object();
		private ProgressDialogForm form;
		/// <summary>
		/// ProgressDialog �����������܂��B
		/// </summary>
		public ProgressDialog(){}
		//=================================================
		//		�i�s��
		//=================================================
		private int maximum=100;
		private int minimum=0;
		private int value=0;
		/// <summary>
		/// �i�s�󋵂̍ŏ��l���擾���͐ݒ肵�܂��B
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
		/// �i�s�󋵂̍ő�l���擾���͐ݒ肵�܂��B
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
		/// ���݂̐i�s�󋵂��擾���͐ݒ肵�܂��B
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
		/// ���݂̐i�s�󋵂�S�����Ŏ擾���܂��B
		/// </summary>
		public float Percentage{
			get{return this.value/(float)(this.maximum-this.minimum);}
		}
		//=================================================
		//		�\����\��
		//=================================================
		private bool showing=false;
		private System.Windows.Forms.IWin32Window owner;
		private const int DELAY=1000;
		/// <summary>
		/// �_�C�A���O��\�����܂��B
		/// </summary>
		public void Show(){
			this.Show(null);
		}
		/// <summary>
		/// �_�C�A���O��\�����܂��B
		/// </summary>
		/// <param name="owner">�e Window ���w�肵�܂��B</param>
		public void Show(System.Windows.Forms.IWin32Window owner){
			if(this.showing)return;
			this.showing=true;
			this.iscanceled=false;
			this.owner=owner;

			System.Threading.Thread thread=new System.Threading.Thread(new System.Threading.ThreadStart(StartShow));
			thread.Name="<ImageArrange>afh.ImageArrange.ProgressDialog �i���󋵂̕\��";
			thread.IsBackground=true;
			//thread.Priority=System.Threading.ThreadPriority.BelowNormal;
			thread.Start();
		}
		/// <summary>
		/// �_�C�A���O����܂��B
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

			// ���ɔ����ȏ�I����Ă���ꍇ�ɂ͂��������҂��Ă݂�
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
		//		�\�����e
		//=================================================
		private string title="������";
		private string description="�������ł�...";
		private System.Drawing.Icon icon;
		private System.Drawing.Image image;
		private bool cancancel=true;
		/// <summary>
		/// �^�C�g���o�[�ɕ\�����镶������擾���͐ݒ肵�܂��B
		/// </summary>
		public string Title{
			get{return this.title;}
			set{
				this.title=value;
				if(this.form!=null)this.form.SetTitle(value);
			}
		}
		/// <summary>
		/// �^�C�g���o�[�ɕ\������A�C�R�����w�肵�܂��B
		/// </summary>
		public System.Drawing.Icon Icon{
			get{return this.icon;}
			set{
				this.icon=value;
				if(this.form!=null)this.form.SetIcon(value);
			}
		}
		/// <summary>
		/// ���݂̏����̓��e��������镶������擾���͐ݒ肵�܂��B
		/// </summary>
		public string Description{
			get{return this.description;}
			set{
				this.description=value;
				if(this.form!=null)this.form.SetDescription(value);
			}
		}
		/// <summary>
		/// ���݂̏����̓��e�Ɋւ���摜���w�肵�܂��B
		/// �A�j���[�V���� gif �����\�ł��B
		/// </summary>
		public System.Drawing.Image Image{
			get{return this.image;}
			set{
				this.image=value;
				if(this.form!=null)this.form.SetImage(value);
			}
		}
		/// <summary>
		/// ���[�U�[�ɂ��L�����Z�����\���ǂ������w�肵�܂��B
		/// </summary>
		public bool CanCancel{
			get{return this.cancancel;}
			set{
				this.cancancel=value;
				if(this.form!=null)this.form.SetCanCancel(value);
			}
		}
		//=================================================
		//		�L�����Z��
		//=================================================
		private void btnCancel_Click(object sender,System.EventArgs args){
			this.iscanceled=true;
			if(this.Canceled==null)return;
			this.Canceled(sender,args);
		}
		/// <summary>
		/// ���[�U�ɂ���đ�����L�����Z�����ꂽ�ۂɔ������܂��B
		/// </summary>
		public event System.EventHandler Canceled;
		/// <summary>
		/// ���[�U�[�ɂ���đ��삪�L�����Z�����ꂽ���ǂ������擾���܂��B
		/// </summary>
		public bool IsCanceled{
			get{return this.iscanceled;}
		}private bool iscanceled;
	}
}
