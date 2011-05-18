namespace afh.Parse.Tester {
	partial class Form1 {
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.domViewer1 = new afh.HTML.DOMTree();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Left;
			this.listView1.Location = new System.Drawing.Point(225,0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(321,484);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "単語";
			this.columnHeader1.Width = 213;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "種類";
			this.columnHeader2.Width = 98;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.textBox1.Location = new System.Drawing.Point(0,0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(222,484);
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(0,0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(115,25);
			this.button1.TabIndex = 2;
			this.button1.Text = "Read[Javascript]";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(121,0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(160,25);
			this.button2.TabIndex = 3;
			this.button2.Text = "WriteHTML of LetterType";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(304,0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(113,25);
			this.button3.TabIndex = 4;
			this.button3.Text = "TestHTMLParser";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(423,0);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(89,25);
			this.button4.TabIndex = 6;
			this.button4.Text = "Open HTML...";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "htm";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "HTML ドキュメント (*.htm;*.html)|*.htm;*.html|すべてのファイル|*.*";
			// 
			// domViewer1
			// 
			this.domViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.domViewer1.Location = new System.Drawing.Point(549,0);
			this.domViewer1.Name = "domViewer1";
			this.domViewer1.Size = new System.Drawing.Size(509,484);
			this.domViewer1.TabIndex = 5;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.domViewer1);
			this.panel1.Controls.Add(this.splitter2);
			this.panel1.Controls.Add(this.listView1);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0,32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1058,484);
			this.panel1.TabIndex = 7;
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(546,0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3,484);
			this.splitter2.TabIndex = 7;
			this.splitter2.TabStop = false;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(222,0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3,484);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.button2);
			this.panel2.Controls.Add(this.button4);
			this.panel2.Controls.Add(this.button3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0,0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1058,32);
			this.panel2.TabIndex = 8;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F,12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1058,516);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private afh.HTML.DOMTree domViewer1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Splitter splitter1;
	}
}

