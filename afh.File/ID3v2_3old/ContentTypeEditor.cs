namespace afh.File.ID3v2_3_{
	public sealed class ContentTypeEditor:SingleFrameEditor {
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private TCONFrameEditBox editBox1;

		public ContentTypeEditor():base(){
			this.ToolTipTarget=this.label1;

			this.InitializeComponent();

			this.FrameId="TCON";

			// 配置換え
			this.SuspendLayout();
			this.panel1.SuspendLayout();

			this.Controls.Remove(this.label1);
			this.Controls.Remove(this.button1);
			this.label1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.button1.Dock=System.Windows.Forms.DockStyle.Right;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button1);

			this.panel1.ResumeLayout();
			this.ResumeLayout();
		}

		#region Designer Code
		private void InitializeComponent() {
			this.components=new System.ComponentModel.Container();
			this.editBox1=new afh.File.ID3v2_3_.TCONFrameEditBox();
			this.panel1=new System.Windows.Forms.Panel();
			this.toolTip1=new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location=new System.Drawing.Point(3,3);
			this.label1.Text="TCON: Content type";
			// 
			// button1
			// 
			this.button1.Location=new System.Drawing.Point(400,3);
			this.button1.MaximumSize=new System.Drawing.Size(1000,20);
			this.button1.MinimumSize=new System.Drawing.Size(40,20);
			// 
			// editBox1
			// 
			this.editBox1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.editBox1.Enabled=false;
			this.editBox1.Location=new System.Drawing.Point(0,27);
			this.editBox1.Name="editBox1";
			this.editBox1.Padding=new System.Windows.Forms.Padding(3);
			this.editBox1.Size=new System.Drawing.Size(509,289);
			this.editBox1.TabIndex=0;
			// 
			// panel1
			// 
			this.panel1.Dock=System.Windows.Forms.DockStyle.Top;
			this.panel1.Location=new System.Drawing.Point(0,0);
			this.panel1.Name="panel1";
			this.panel1.Size=new System.Drawing.Size(509,27);
			this.panel1.TabIndex=3;
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay=2147483647;
			this.toolTip1.InitialDelay=500;
			this.toolTip1.ReshowDelay=100;
			this.toolTip1.ShowAlways=true;
			// 
			// ContentTypeEditor
			// 
			this.Controls.Add(this.editBox1);
			this.Controls.Add(this.panel1);
			this.FrameId="TCON";
			this.Name="ContentTypeEditor";
			this.Size=new System.Drawing.Size(509,316);
			this.toolTip1.SetToolTip(this,"Content type.\r\nTCON Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			this.ToolTip=this.toolTip1;
			this.Controls.SetChildIndex(this.panel1,0);
			this.Controls.SetChildIndex(this.button1,0);
			this.Controls.SetChildIndex(this.editBox1,0);
			this.Controls.SetChildIndex(this.label1,0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		//=================================================
		//		override SingleFrameEditor
		//=================================================
		protected override void FilterFrameId(string value) {
			base.FilterFrameId(value);
			if(value!="TCON")
				throw new System.ApplicationException("このコントロールでは TCON Frame しか編集できません。");
		}
		protected override bool ExistsFrame {
			set {
				base.ExistsFrame=value;
				if(this.editBox1!=null)
					this.editBox1.Enabled=value;
			}
		}
		protected override void ApplyToControl(Frame frame) {
			if(frame==null){
				this.editBox1.Frame=null;
			}else{
				this.editBox1.Frame=(TCONFrame)frame;
			}
		}
	}
}