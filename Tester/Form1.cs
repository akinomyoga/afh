using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Tester{
	/// <summary>
	/// Form1 の概要の説明です。
	/// </summary>
	[afh.Configuration.RestoreProperties("Location")]
	public class Form1 : System.Windows.Forms.Form{
		private System.Windows.Forms.Button button1;
		[afh.Configuration.RestoreProperties("SelectedIndex")]
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1(){
			InitializeComponent();
			afh.Application.LogView.Instance.Dock=System.Windows.Forms.DockStyle.Fill;
			panel1.Controls.Add(afh.Application.LogView.Instance);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(344, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "実行";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(8, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(328, 20);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(328, 256);
			this.label1.TabIndex = 2;
			// 
			// textBox1
			// 
			this.textBox1.AcceptsReturn = true;
			this.textBox1.AcceptsTab = true;
			this.textBox1.Location = new System.Drawing.Point(344, 32);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(544, 256);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "";
			this.textBox1.WordWrap = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(856, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "結果";
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(8, 296);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(880, 272);
			this.panel1.TabIndex = 5;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(898, 575);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "Tester";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(){
			afh.Plugin.PluginReaderAttribute.ReadAssemblies();
			Application.Run(new Form1());
		}
		protected override void OnLoad(EventArgs e){
			base.OnLoad (e);
			afh.Configuration.RestorePropertiesAttribute.Restore(this,"test");
		}
		protected override void OnClosing(CancelEventArgs e){
			afh.Configuration.RestorePropertiesAttribute.Save(this,"test");
			base.OnClosing (e);
		}

		private void Form1_Load(object sender, System.EventArgs e){
			/*
			this.comboBox1.Items.Add(new Javascript.CastTest());
			this.comboBox1.Items.Add(new Javascript.ThrowTest());
			this.comboBox1.Items.Add(new Javascript.GetMethods());
			this.comboBox1.Items.Add(new Javascript.IsSubclassOf());
			this.comboBox1.Items.Add(new Javascript.ParamsAttribute());
			//*/
			System.Reflection.Assembly asm=System.Reflection.Assembly.GetExecutingAssembly();
			foreach(System.Type t in asm.GetTypes()){
				if(t.GetCustomAttributes(typeof(TestFunctionAttribute),false).Length==0)continue;
				this.comboBox1.Items.Add(asm.CreateInstance(t.FullName));
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e){
			if(this.comboBox1.SelectedIndex<0)return;
			TestFunction f=this.comboBox1.SelectedItem as TestFunction;
			if(f==null)return;
			this.label1.Text=f.Description;
		}

		private void button1_Click(object sender, System.EventArgs e){
			if(this.comboBox1.SelectedIndex<0)return;
			TestFunction f=this.comboBox1.SelectedItem as TestFunction;
			if(f==null)return;
			this.textBox1.Text=f.Exec0();
			//this.textBox1.Lines=f.Exec0().Split(new char[]{'\n'});
		}
	}

	public abstract class TestFunction{
		public TestFunction(){}
		//
		//		実行
		//
		public string Exec0(){
			this.outstr="";
			return this.Exec();
		}
		public abstract string Exec();
		protected string outstr;
		protected virtual void WriteLine(string msg){
			this.outstr+=msg+"\r\n";
		}
		//
		//		解説
		//
		public override string ToString(){
			object[] attrs=this.GetType().GetCustomAttributes(typeof(Tester.TestFunctionAttribute),true);
			if(attrs.Length>0)return ((TestFunctionAttribute)attrs[0]).Title;
			return base.ToString();
		}
		public string Description{
			get{
				object[] attrs=this.GetType().GetCustomAttributes(typeof(Tester.TestFunctionAttribute),true);
				if(attrs.Length>0)return ((TestFunctionAttribute)attrs[0]).Description;
				return base.ToString();
			}
		}
	}
	public class TestFunctionAttribute:System.Attribute{
		public TestFunctionAttribute(string title,string desc){this.title=title;this.desc=desc;}
		private string title;
		private string desc;
		public string Title{get{return this.title;}}
		public string Description{get{return this.desc;}}
	}



}
