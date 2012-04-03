using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace afh.Configuration{
	/// <summary>
	/// 既存の設定ファイルを読み込み事ができなかった場合に、
	/// 設定ファイルを上書きするかどうかを確認するダイアログです
	/// </summary>
	internal class ConfirmOverwriteSetting:System.Windows.Forms.Form{
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Button btnNo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// デザイナ変数。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ConfirmOverwriteSetting(){
			InitializeComponent();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region デザイナコード
		/// <summary>
		/// フォームにコントロールを配置します。
		/// デザイナ サポートに使用します。
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
			this.btnYes.Text = "はい(&Y)";
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnNo
			// 
			this.btnNo.Location = new System.Drawing.Point(104, 120);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(88, 24);
			this.btnNo.TabIndex = 1;
			this.btnNo.Text = "いいえ(&N)";
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "設定ファイル Setting.xml を読み込むのに失敗しました。アプリケーションは既定の設定で起動します。";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "既存の設定ファイルを、既定の設定で上書きしますか?";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(40, 80);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(248, 16);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "既存のファイルを *.bk にバックアップします。(&B)";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "(上書きする場合)";
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
			this.Text = "設定ファイル上書き確認";
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
		/// 設定ファイル上書きの確認を取り、結果を取得します。
		/// </summary>
		/// <returns>上書き確認の結果</returns>
		public static ConfirmOverwriteSetting.Result Confirm(){
			ConfirmOverwriteSetting f=new ConfirmOverwriteSetting();
			while(!f.result)f.ShowDialog();
			ConfirmOverwriteSetting.Result r=new ConfirmOverwriteSetting.Result(f.yes,f.checkBox1.Checked);
			f.Dispose();
			return r;
		}
		/// <summary>
		/// 設定ファイル上書きの確認を取り、結果を取得します。
		/// (既に起動した後で確認を取る時の為のメッセージが表示されます。)
		/// </summary>
		/// <returns>上書き確認の結果</returns>
		public static ConfirmOverwriteSetting.Result Confirm2(){
			ConfirmOverwriteSetting f=new ConfirmOverwriteSetting();
			f.label1.Text=AFTER_LOAD1;
			f.label2.Text=AFTER_LOAD2;
			while(!f.result)f.ShowDialog();
			ConfirmOverwriteSetting.Result r=new ConfirmOverwriteSetting.Result(f.yes,f.checkBox1.Checked);
			f.Dispose();
			return r;
		}
		private const string AFTER_LOAD1="設定ファイル Setting.xml を読み込むのに失敗しています。現在は設定ファイルを使用せずに起動している為、変更された設定は保存されません。";
		private const string AFTER_LOAD2="既定の設定を上書きして現在の設定を保存することが出来ます。上書きしますか?";
		public struct Result{
			/// <summary>
			/// 設定ファイルを上書きするか否かの値を保持します。
			/// </summary>
			public bool overwrite;
			/// <summary>
			/// 上書きする場合、バックアップを取るか否かの値を持ちます。
			/// </summary>
			public bool backup;
			public Result(bool overwrite,bool backup){
				this.overwrite=overwrite;
				this.backup=backup;
			}
		}
	}
}
