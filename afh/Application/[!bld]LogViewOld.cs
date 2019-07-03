using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace afh.Application{
	/// <summary>
	/// LogViewOld �͕������� LogBox ��\������ׂ̃R���g���[���ł��B
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogView))]
	[System.Obsolete]
	public class LogViewOld : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// LogViewOld �̃R���X�g���N�^�ł��BLogViewOld �����������܂��B
		/// </summary>
		public LogViewOld(){
			InitializeComponent();
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(128, 208);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(128, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 208);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(131, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(205, 208);
			this.panel1.TabIndex = 2;
			// 
			// LogViewOld
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.listBox1);
			this.Name = "LogViewOld";
			this.Size = new System.Drawing.Size(336, 208);
			this.ResumeLayout(false);

		}
		#endregion

		private afh.Application.LogBox current;
		/// <summary>
		/// ���ݕ\�����Ă��� LogBox ���擾���͐ݒ肵�܂��B
		/// </summary>
		protected afh.Application.LogBox CurrentLog{
			get{return this.current;}
			set{
				this.panel1.Controls.Clear();
				value.Dock=System.Windows.Forms.DockStyle.Fill;
				this.current=value;
				try{this.panel1.Controls.Add(value);}catch(System.Exception e){
					afh.Application.Log.AfhOut.WriteError(e);
				}
			}
		}
		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e){
			if(this.listBox1.SelectedIndex<0)return;
			if(this.listBox1.SelectedItem is afh.Application.LogBox){
				this.CurrentLog=(afh.Application.LogBox)this.listBox1.SelectedItem;
			}
		}
		/// <summary>
		/// �V���� LogBox ���쐬���āA���� LogViewOld �ɓo�^���܂��B
		/// <!--�V���� LogBox �̍쐬�͂��� LogViewOld ���Ǘ����Ă���X���b�h���ō쐬�����̂ŁA�X���b�h�Z�[�t�ł��B-->
		/// </summary>
		/// <param name="name">�V�����쐬���� LogBox �̕\�������w�肵�܂��B</param>
		/// <returns>�V�����쐬���ꂽ LogBox ��Ԃ��܂��B</returns>
		public LogBox CreateLog(string name){
			if(this.InvokeRequired){
				if(!this.IsHandleCreated)
					System.Console.WriteLine("LogViewHandle: "+this.Handle.ToString());
				return (LogBox)this.Invoke(new DlgCreateLog(this.CreateLog_),name);
			}else return this.CreateLog_(name);
		}

		private delegate LogBox DlgCreateLog(string name);
		private LogBox CreateLog_(string name){
			LogBox r=new LogBox(name);
			LogViewOld.Instance.AddLog(r);
			System.Console.WriteLine(r.Handle);
			//WriteWindowThreadId(r);
			return r;
		}//*/
		[System.Runtime.InteropServices.DllImport("user32.dll",SetLastError=true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd,out uint lpdwProcessId);
		private static void WriteWindowThreadId(System.Windows.Forms.Control ctrl){
			uint x;
			System.Console.WriteLine("ThreadId: "+GetWindowThreadProcessId(ctrl.Handle,out x));
		}
		/// <summary>
		/// �\�����鎖���o���� LogBox ��ǉ����܂��B
		/// </summary>
		/// <param name="value">�o�^���� LogBox ���������܂��B</param>
		public void AddLog(Application.LogBox value){
			this.listBox1.Items.Add(value);
			if(this.listBox1.Items.Count==1)this.listBox1.SelectedIndex=0;
		}
		/// <summary>
		/// �o�^����Ă��� LogBox �̓o�^���������܂��B
		/// </summary>
		/// <param name="value">�o�^���������� LogBox ���w�肵�܂��B</param>
		public void RemoveLog(Application.LogBox value){
			this.listBox1.Items.Remove(value);
		}
		/// <summary>
		/// LogBox �̃��X�g�����̕����擾���͐ݒ肵�܂��B
		/// </summary>
		public int ListWidth{
			get{return this.listBox1.Width;}
			set{this.listBox1.Width=value;}
		}
		//===========================================================
		//		�Ǘ��C���X�^���X (�� Singleton)
		//===========================================================
		private static readonly LogViewOld inst=new LogViewOld();
		/// <summary>
		/// LogViewOld �̊Ǘ��C���X�^���X���擾���܂��B
		/// </summary>
		public static LogViewOld Instance{get{return LogViewOld.inst;}}
	}
}
