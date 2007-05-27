using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using afh.Configuration;

namespace Tester{
	/// <summary>
	/// TestSettingForm の概要の説明です。
	/// </summary>
	[SettingContainer("afh.Application.SettingContainer\\TestSettingForm")]
	public class TestSettingForm : System.Windows.Forms.Form{
		[SettingButton(SettingButtonAttribute.Operation.Cancel)]
		private System.Windows.Forms.Button btnCancel;
		[SettingButton(SettingButtonAttribute.Operation.OK)]
		private System.Windows.Forms.Button btnOK;
		[SettingButton(SettingButtonAttribute.Operation.Restore)]
		private System.Windows.Forms.Button btnRest;
		[SettingButton(SettingButtonAttribute.Operation.Apply)]
		private System.Windows.Forms.Button btnApply;
		[SettingButton(SettingButtonAttribute.Operation.RestoreDefaults)]
		private System.Windows.Forms.Button btnDefault;
		[SettingControl("CheckBox")]
		private System.Windows.Forms.CheckBox checkBox1;
		[SettingControl("TextBox")]
		private System.Windows.Forms.TextBox textBox1;
		[SettingControl("RadioButton")]
		private System.Windows.Forms.RadioButton radioButton1;
		[SettingControl("ComboBox")]
		private System.Windows.Forms.ComboBox comboBox1;
		[SettingControl("NumericUpDown")]
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		[SettingControl("DomainUpDown")]
		private System.Windows.Forms.DomainUpDown domainUpDown1;
		[SettingControl("MonthCalendar")]
		private System.Windows.Forms.MonthCalendar monthCalendar1;
		[SettingControl("CheckedListBox")]
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		[SettingControl("TrackBar")]
		private System.Windows.Forms.TrackBar trackBar1;
		[SettingControl("HScrollBar")]
		private System.Windows.Forms.HScrollBar hScrollBar1;
		[SettingControl("VScrollBar")]
		private System.Windows.Forms.VScrollBar vScrollBar1;
		[SettingControl("DateTimePicker")]
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		[SettingControl("MenuItem")]
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.ComponentModel.Container components=null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TestSettingForm(){
			InitializeComponent();
			afh.Configuration.SettingContainerAttribute.InitializeContainer(this);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components!=null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnRest = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnDefault = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(8, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 24);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "キャンセル(&C)";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(8, 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(96, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnRest
			// 
			this.btnRest.Location = new System.Drawing.Point(8, 72);
			this.btnRest.Name = "btnRest";
			this.btnRest.Size = new System.Drawing.Size(96, 24);
			this.btnRest.TabIndex = 2;
			this.btnRest.Text = "現在の設定(&R)";
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(8, 40);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(96, 24);
			this.btnApply.TabIndex = 3;
			this.btnApply.Text = "適用(&A)";
			// 
			// btnDefault
			// 
			this.btnDefault.Location = new System.Drawing.Point(8, 104);
			this.btnDefault.Name = "btnDefault";
			this.btnDefault.Size = new System.Drawing.Size(96, 24);
			this.btnDefault.TabIndex = 4;
			this.btnDefault.Text = "既定値(&D)";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(120, 8);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(112, 16);
			this.checkBox1.TabIndex = 5;
			this.checkBox1.Text = "実験結果を観察";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(120, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(136, 19);
			this.textBox1.TabIndex = 7;
			this.textBox1.Text = "設定項目です";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(120, 56);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(72, 16);
			this.radioButton1.TabIndex = 8;
			this.radioButton1.Text = "設定項目";
			// 
			// comboBox1
			// 
			this.comboBox1.Items.AddRange(new object[] {
														   "Setting",
														   "設定",
														   "Configuration",
														   "オプション"});
			this.comboBox1.Location = new System.Drawing.Point(120, 80);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(136, 20);
			this.comboBox1.TabIndex = 9;
			this.comboBox1.Text = "Setting";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(120, 104);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(136, 19);
			this.numericUpDown1.TabIndex = 10;
			// 
			// domainUpDown1
			// 
			this.domainUpDown1.Items.Add("Setting");
			this.domainUpDown1.Items.Add("設定");
			this.domainUpDown1.Items.Add("Configuration");
			this.domainUpDown1.Items.Add("オプション");
			this.domainUpDown1.Location = new System.Drawing.Point(120, 128);
			this.domainUpDown1.Name = "domainUpDown1";
			this.domainUpDown1.Size = new System.Drawing.Size(136, 19);
			this.domainUpDown1.TabIndex = 11;
			this.domainUpDown1.Text = "Setting";
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(120, 152);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.TabIndex = 12;
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.Items.AddRange(new object[] {
																 "Setting",
																 "設定",
																 "Configuration",
																 "オプション"});
			this.checkedListBox1.Location = new System.Drawing.Point(288, 152);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(128, 144);
			this.checkedListBox1.TabIndex = 13;
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(288, 8);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(128, 42);
			this.trackBar1.TabIndex = 14;
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(288, 56);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(128, 16);
			this.hScrollBar1.TabIndex = 15;
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(264, 8);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(16, 136);
			this.vScrollBar1.TabIndex = 16;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(288, 80);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(128, 19);
			this.dateTimePicker1.TabIndex = 17;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3});
			this.menuItem1.Text = "項目";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Menu1";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Menu2";
			// 
			// TestSettingForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(424, 301);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.hScrollBar1);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.checkedListBox1);
			this.Controls.Add(this.monthCalendar1);
			this.Controls.Add(this.domainUpDown1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.btnDefault);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnRest);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "TestSettingForm";
			this.Text = "自動設定項目初期化実験";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e){
			this.DialogResult=System.Windows.Forms.DialogResult.OK;
			this.Close();
		}
		private void btnCancel_Click(object sender, System.EventArgs e){
			this.DialogResult=System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
	}
	/// <summary>
	/// 設定に対するアクセスを提供します。
	/// </summary>
	public class TestSetting{
		//※ 既定値は Form/ContainerControl を初期化するまでは、SettingKey に反映されません。
		// よってクラスの初期化の際に Form/ContainerControl のシングルインスタンスを初期化します。
		// 設定のウィンドウを表示する際にはこのシングルインスタンスを用いると良いでしょう。
		private static TestSettingForm form=new TestSettingForm();

		private static SettingKey Key=Setting.Root["afh.Application.SettingContainer\\TestSettingForm"];
		public static bool CheckBox{
			get{return (bool)afh.Convert.Convert.FromString(typeof(bool),Key.Var["CheckBox"]);}
		}
		public static string TextBox{
			get{return Key.Var["TextBox"];}
		}
	}
}
