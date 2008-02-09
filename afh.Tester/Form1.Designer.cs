namespace afh.Tester {
	partial class Form1{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panelLog = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.splitter2);
			this.panel1.Controls.Add(this.listBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0,0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(3);
			this.panel1.Size = new System.Drawing.Size(722,209);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoEllipsis = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(148,3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(571,203);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(144,3);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(4,203);
			this.splitter2.TabIndex = 2;
			this.splitter2.TabStop = false;
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(3,3);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(141,196);
			this.listBox1.TabIndex = 0;
			this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0,209);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(722,3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panelLog
			// 
			this.panelLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLog.Location = new System.Drawing.Point(0,212);
			this.panelLog.Name = "panelLog";
			this.panelLog.Size = new System.Drawing.Size(722,281);
			this.panelLog.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F,12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722,493);
			this.Controls.Add(this.panelLog);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		[afh.Configuration.RestoreProperties("Height")]
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panelLog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Splitter splitter2;
		[afh.Configuration.RestoreProperties("Width")]
		private System.Windows.Forms.ListBox listBox1;


	}
}

