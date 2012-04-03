using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace afh.Application{
	/// <summary>
	/// LogView は複数ある Log を表示する為のコントロールです。
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogView))]
	public class LogView : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private LogBox log1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// LogView のコンストラクタです。LogView を初期化します。
		/// </summary>
		public LogView(){
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

		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
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
			this.log1.Font = new System.Drawing.Font("ＭＳ ゴシック",9F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(128)));
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
		/// 現在表示している Log を取得亦は設定します。
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
		/// 指定した log を選択した状態にし、内容が表示される様にします。
		/// </summary>
		/// <param name="log">選択した状態にする Log を指定します。
		/// 元からこのインスタンスに登録されている Log を指定する必要があります。</param>
		public void EnsureVisibleLog(afh.Application.Log log){
			if(!this.listBox1.Items.Contains(log))return;
			this.listBox1.SelectedItem=log;
		}
		/// <summary>
		/// Log のリスト部分の幅を取得亦は設定します。
		/// </summary>
		public int ListWidth{
			get{return this.listBox1.Width;}
			set{this.listBox1.Width=value;}
		}
		//===========================================================
		//		Log の登録・解除
		//===========================================================
		/// <summary>
		/// 新しい Log を作成して、この LogView に登録します。
		/// <!--新しい Log の作成はこの LogView を管理しているスレッド内で作成されるので、スレッドセーフです。-->
		/// </summary>
		/// <param name="name">新しく作成する Log の表示名を指定します。</param>
		/// <returns>新しく作成された Log を返します。</returns>
		public Log CreateLog(string name){
			Log r=new Log(name);
			this.AddLog(r);
			return r;
		}

		/// <summary>
		/// 表示する事が出来る Log を追加します。
		/// </summary>
		/// <param name="value">登録する Log を指定します。</param>
		public void AddLog(Application.Log value){
			this.listBox1.Items.Add(value);
			if(this.listBox1.Items.Count==1)this.listBox1.SelectedIndex=0;
		}
		/// <summary>
		/// 登録されている Log の登録を解除します。
		/// </summary>
		/// <param name="value">登録を解除する Log を指定します。</param>
		public void RemoveLog(Application.Log value){
			this.listBox1.Items.Remove(value);
		}
		//===========================================================
		//		管理インスタンス (準 Singleton)
		//===========================================================
		private static readonly LogView inst=new LogView();
		/// <summary>
		/// LogView の管理インスタンスを取得します。
		/// </summary>
		public static LogView Instance{get{return LogView.inst;}}
	}
}
