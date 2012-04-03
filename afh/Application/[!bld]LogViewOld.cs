using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace afh.Application{
	/// <summary>
	/// LogViewOld は複数ある LogBox を表示する為のコントロールです。
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogView))]
	[System.Obsolete]
	public class LogViewOld : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// LogViewOld のコンストラクタです。LogViewOld を初期化します。
		/// </summary>
		public LogViewOld(){
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
		/// 現在表示している LogBox を取得亦は設定します。
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
		/// 新しい LogBox を作成して、この LogViewOld に登録します。
		/// <!--新しい LogBox の作成はこの LogViewOld を管理しているスレッド内で作成されるので、スレッドセーフです。-->
		/// </summary>
		/// <param name="name">新しく作成する LogBox の表示名を指定します。</param>
		/// <returns>新しく作成された LogBox を返します。</returns>
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
		/// 表示する事が出来る LogBox を追加します。
		/// </summary>
		/// <param name="value">登録する LogBox を解除します。</param>
		public void AddLog(Application.LogBox value){
			this.listBox1.Items.Add(value);
			if(this.listBox1.Items.Count==1)this.listBox1.SelectedIndex=0;
		}
		/// <summary>
		/// 登録されている LogBox の登録を解除します。
		/// </summary>
		/// <param name="value">登録を解除する LogBox を指定します。</param>
		public void RemoveLog(Application.LogBox value){
			this.listBox1.Items.Remove(value);
		}
		/// <summary>
		/// LogBox のリスト部分の幅を取得亦は設定します。
		/// </summary>
		public int ListWidth{
			get{return this.listBox1.Width;}
			set{this.listBox1.Width=value;}
		}
		//===========================================================
		//		管理インスタンス (準 Singleton)
		//===========================================================
		private static readonly LogViewOld inst=new LogViewOld();
		/// <summary>
		/// LogViewOld の管理インスタンスを取得します。
		/// </summary>
		public static LogViewOld Instance{get{return LogViewOld.inst;}}
	}
}
