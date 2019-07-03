using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace afh.Application{
	/// <summary>
	/// LogView �͕������� Log ��\������ׂ̃R���g���[���ł��B
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogView))]
	public class LogView : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private LogBox log1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// LogView �̃R���X�g���N�^�ł��BLogView �����������܂��B
		/// </summary>
		public LogView(){
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
			this.log1 = new afh.Application.LogBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(0,0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(128,208);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(128,0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3,208);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.log1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(131,0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(205,208);
			this.panel1.TabIndex = 2;
			// 
			// log1
			// 
			this.log1.AcceptsReturn = true;
			this.log1.AcceptsTab = true;
			this.log1.BackColor = System.Drawing.Color.Black;
			this.log1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.log1.Font = new System.Drawing.Font("�l�r �S�V�b�N",9F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(128)));
			this.log1.ForeColor = System.Drawing.Color.Silver;
			this.log1.Location = new System.Drawing.Point(0,0);
			this.log1.Multiline = true;
			this.log1.Name = "log1";
			this.log1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.log1.Size = new System.Drawing.Size(205,208);
			this.log1.TabIndex = 0;
			this.log1.WordWrap = false;
			// 
			// LogView
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.listBox1);
			this.Name = "LogView";
			this.Size = new System.Drawing.Size(336,208);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private afh.Application.Log current;
		/// <summary>
		/// ���ݕ\�����Ă��� Log ���擾���͐ݒ肵�܂��B
		/// </summary>
		protected afh.Application.Log CurrentLog{
			get{return this.current;}
			set{
				if(this.current==value)return;

				if(this.current!=null)this.current.LogBox=null;
				this.current=value;
				if(this.current!=null)this.current.LogBox=this.log1;
			}
		}
		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e){
			if(this.listBox1.SelectedIndex<0)return;
			if(this.listBox1.SelectedItem is afh.Application.Log){
				this.CurrentLog=(afh.Application.Log)this.listBox1.SelectedItem;
			}
		}
		/// <summary>
		/// �w�肵�� log ��I��������Ԃɂ��A���e���\�������l�ɂ��܂��B
		/// </summary>
		/// <param name="log">�I��������Ԃɂ��� Log ���w�肵�܂��B
		/// �����炱�̃C���X�^���X�ɓo�^����Ă��� Log ���w�肷��K�v������܂��B</param>
		public void EnsureVisibleLog(afh.Application.Log log){
			if(!this.listBox1.Items.Contains(log))return;
			this.listBox1.SelectedItem=log;
		}
		/// <summary>
		/// Log �̃��X�g�����̕����擾���͐ݒ肵�܂��B
		/// </summary>
		public int ListWidth{
			get{return this.listBox1.Width;}
			set{this.listBox1.Width=value;}
		}
		//===========================================================
		//		Log �̓o�^�E����
		//===========================================================
		/// <summary>
		/// �V���� Log ���쐬���āA���� LogView �ɓo�^���܂��B
		/// <!--�V���� Log �̍쐬�͂��� LogView ���Ǘ����Ă���X���b�h���ō쐬�����̂ŁA�X���b�h�Z�[�t�ł��B-->
		/// </summary>
		/// <param name="name">�V�����쐬���� Log �̕\�������w�肵�܂��B</param>
		/// <returns>�V�����쐬���ꂽ Log ��Ԃ��܂��B</returns>
		public Log CreateLog(string name){
			Log r=new Log(name);
			this.AddLog(r);
			return r;
		}

		/// <summary>
		/// �\�����鎖���o���� Log ��ǉ����܂��B
		/// </summary>
		/// <param name="value">�o�^���� Log ���w�肵�܂��B</param>
		public void AddLog(Application.Log value){
			this.listBox1.Items.Add(value);
			if(this.listBox1.Items.Count==1)this.listBox1.SelectedIndex=0;
		}
		/// <summary>
		/// �o�^����Ă��� Log �̓o�^���������܂��B
		/// </summary>
		/// <param name="value">�o�^���������� Log ���w�肵�܂��B</param>
		public void RemoveLog(Application.Log value){
			this.listBox1.Items.Remove(value);
		}
		//===========================================================
		//		�Ǘ��C���X�^���X (�� Singleton)
		//===========================================================
		private static readonly LogView inst=new LogView();
		/// <summary>
		/// LogView �̊Ǘ��C���X�^���X���擾���܂��B
		/// </summary>
		public static LogView Instance{get{return LogView.inst;}}
	}
}
