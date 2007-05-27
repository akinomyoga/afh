using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Tester{
	/// <summary>
	/// ErrorView の概要の説明です。
	/// </summary>
	public class ErrorView : System.Windows.Forms.Form{
		private System.Windows.Forms.TreeView treeView1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorView(){
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

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(292, 273);
			this.treeView1.TabIndex = 0;
			// 
			// ErrorView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.treeView1);
			this.MinimizeBox = false;
			this.Name = "ErrorView";
			this.Text = "ErrorView";
			this.ResumeLayout(false);

		}
		#endregion

		public void AddException(System.Exception e){
			this.treeView1.Nodes.Add(this.Exception2TreeNode("",e));
		}
		private System.Windows.Forms.TreeNode Exception2TreeNode(string name,System.Exception e){
			if(name==null||name==""){
				name=e.GetType().ToString();
			}else name+=":\t"+e.GetType().ToString();
			System.Windows.Forms.TreeNode tn=new TreeNode(name);
			tn.Nodes.Add("Message:"+e.Message);
			tn.Nodes.Add("HelpLink:\t"+e.HelpLink);
			tn.Nodes.Add("Source:\t"+e.Source);
			tn.Nodes.Add(this.StackTrace2TreeNode(e.StackTrace));
			tn.Nodes.Add("TargetSite:\t"+e.TargetSite);
			System.Exception e2=e.InnerException;
			if(e2!=null)tn.Nodes.Add(this.Exception2TreeNode("InnerException",e2));
			e2=e.GetBaseException();
			if(e2!=null&&e!=e2)tn.Nodes.Add(this.Exception2TreeNode("BaseException",e2));
			return tn;
		}
		private System.Windows.Forms.TreeNode StackTrace2TreeNode(string stack){
			string name="StackTrace";
			string[] sub=stack.Replace(" at ","|at ").Replace(" in ","|    in ").Split(new char[]{'|'});
			System.Windows.Forms.TreeNode tn=new System.Windows.Forms.TreeNode(name);
			foreach(string str in sub){
				tn.Nodes.Add(new System.Windows.Forms.TreeNode(str));
			}
			return tn;
		}
	}
}
