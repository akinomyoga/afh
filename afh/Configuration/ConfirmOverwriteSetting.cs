using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace afh.Configuration{
	/// <summary>
	/// �����̐ݒ�t�@�C����ǂݍ��ݎ����ł��Ȃ������ꍇ�ɁA
	/// �ݒ�t�@�C�����㏑�����邩�ǂ������m�F����_�C�A���O�ł�
	/// </summary>
	internal class ConfirmOverwriteSetting:System.Windows.Forms.Form{
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Button btnNo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// �f�U�C�i�ϐ��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ConfirmOverwriteSetting(){
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

		#region �f�U�C�i�R�[�h
		/// <summary>
		/// �t�H�[���ɃR���g���[����z�u���܂��B
		/// �f�U�C�i �T�|�[�g�Ɏg�p���܂��B
		/// </summary>
		private void InitializeComponent(){
			this.btnYes = new System.Windows.Forms.Button();
			this.btnNo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnYes
			// 
			this.btnYes.Location = new System.Drawing.Point(8, 120);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(88, 24);
			this.btnYes.TabIndex = 0;
			this.btnYes.Text = "�͂�(&Y)";
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnNo
			// 
			this.btnNo.Location = new System.Drawing.Point(104, 120);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(88, 24);
			this.btnNo.TabIndex = 1;
			this.btnNo.Text = "������(&N)";
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "�ݒ�t�@�C�� Setting.xml ��ǂݍ��ނ̂Ɏ��s���܂����B�A�v���P�[�V�����͊���̐ݒ�ŋN�����܂��B";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "�����̐ݒ�t�@�C�����A����̐ݒ�ŏ㏑�����܂���?";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(40, 80);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(248, 16);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "�����̃t�@�C���� *.bk �Ƀo�b�N�A�b�v���܂��B(&B)";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "(�㏑������ꍇ)";
			// 
			// ConfirmOverwriteSetting
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 151);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfirmOverwriteSetting";
			this.ShowInTaskbar = false;
			this.Text = "�ݒ�t�@�C���㏑���m�F";
			this.ResumeLayout(false);

		}
		#endregion

		private bool yes;
		private bool result;
		private void btnYes_Click(object sender, System.EventArgs e){
			this.result=true;
			this.yes=true;
			this.Close();
		}
		private void btnNo_Click(object sender, System.EventArgs e){
			this.result=true;
			this.yes=false;
			this.Close();
		}
		/// <summary>
		/// �ݒ�t�@�C���㏑���̊m�F�����A���ʂ��擾���܂��B
		/// </summary>
		/// <returns>�㏑���m�F�̌���</returns>
		public static ConfirmOverwriteSetting.Result Confirm(){
			ConfirmOverwriteSetting f=new ConfirmOverwriteSetting();
			while(!f.result)f.ShowDialog();
			ConfirmOverwriteSetting.Result r=new ConfirmOverwriteSetting.Result(f.yes,f.checkBox1.Checked);
			f.Dispose();
			return r;
		}
		/// <summary>
		/// �ݒ�t�@�C���㏑���̊m�F�����A���ʂ��擾���܂��B
		/// (���ɋN��������Ŋm�F����鎞�ׂ̈̃��b�Z�[�W���\������܂��B)
		/// </summary>
		/// <returns>�㏑���m�F�̌���</returns>
		public static ConfirmOverwriteSetting.Result Confirm2(){
			ConfirmOverwriteSetting f=new ConfirmOverwriteSetting();
			f.label1.Text=AFTER_LOAD1;
			f.label2.Text=AFTER_LOAD2;
			while(!f.result)f.ShowDialog();
			ConfirmOverwriteSetting.Result r=new ConfirmOverwriteSetting.Result(f.yes,f.checkBox1.Checked);
			f.Dispose();
			return r;
		}
		private const string AFTER_LOAD1="�ݒ�t�@�C�� Setting.xml ��ǂݍ��ނ̂Ɏ��s���Ă��܂��B���݂͐ݒ�t�@�C�����g�p�����ɋN�����Ă���ׁA�ύX���ꂽ�ݒ�͕ۑ�����܂���B";
		private const string AFTER_LOAD2="����̐ݒ���㏑�����Č��݂̐ݒ��ۑ����邱�Ƃ��o���܂��B�㏑�����܂���?";
		public struct Result{
			/// <summary>
			/// �ݒ�t�@�C�����㏑�����邩�ۂ��̒l��ێ����܂��B
			/// </summary>
			public bool overwrite;
			/// <summary>
			/// �㏑������ꍇ�A�o�b�N�A�b�v����邩�ۂ��̒l�������܂��B
			/// </summary>
			public bool backup;
			public Result(bool overwrite,bool backup){
				this.overwrite=overwrite;
				this.backup=backup;
			}
		}
	}
}
