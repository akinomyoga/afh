namespace afh.File.ID3v2_3_{
	/// <summary>
	/// �P�� TextInformationFrame �p�ҏW TextBox �̃N���X�ł��B
	/// �\�ʂ� Button ���g�p���� Frame �� Tag �ɒǉ�������ATag ����폜�����肷�鎖���\�ł��B
	/// </summary>
	public sealed class TextInformationEditor:SingleFrameEditor{
		private System.Windows.Forms.TextBox textBox1;

		public TextInformationEditor():base(){
			this.InitializeComponent();
		}

		#region Designer Code
		private void InitializeComponent(){
			this.textBox1=new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock=System.Windows.Forms.DockStyle.Top;
			// 
			// button1
			// 
			this.button1.Dock=System.Windows.Forms.DockStyle.Right;
			this.button1.Location=new System.Drawing.Point(244,18);
			this.button1.MaximumSize=new System.Drawing.Size(100,20);
			this.button1.MinimumSize=new System.Drawing.Size(80,20);
			// 
			// textBox1
			// 
			this.textBox1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location=new System.Drawing.Point(0,18);
			this.textBox1.Name="textBox1";
			this.textBox1.Size=new System.Drawing.Size(244,19);
			this.textBox1.TabIndex=1;
			this.textBox1.TextChanged+=new System.EventHandler(this.textBox1_TextChanged);
			// 
			// TextInformationEditor
			// 
			this.Controls.Add(this.textBox1);
			this.MaximumSize=new System.Drawing.Size(1024,40);
			this.MinimumSize=new System.Drawing.Size(200,40);
			this.Name="TextInformationEditor";
			this.Size=new System.Drawing.Size(324,40);
			this.Controls.SetChildIndex(this.label1,0);
			this.Controls.SetChildIndex(this.button1,0);
			this.Controls.SetChildIndex(this.textBox1,0);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		//=================================================
		//		Override SingleFrameEditor
		//=================================================
		protected override bool ExistsFrame{
			set{
				base.ExistsFrame=value;
				if(this.textBox1==null)return;
				this.textBox1.Enabled=value;
				this.ApplyToControl(this.Frame);
			}
		}
		protected override void ApplyToControl(Frame frame){
			this.suppress_textchanged=true;
			this.textBox1.Text=frame==null?"":((TextInformationFrame)frame).Text;
			this.suppress_textchanged=false;
		}
		protected override void FilterFrameId(string value) {
			base.FilterFrameId(value);
			if(value[0]!='T'||Frame.FramesData.ContainsKey(value)&&Frame.FramesData[value].FrameType!="text")
				throw new System.ApplicationException("�w�肵�� FrameId �͒P���� Text Information Frame �̕��ł͂���܂���B");
		}

		private bool suppress_textchanged=false;
		private void textBox1_TextChanged(object sender,System.EventArgs e){
			if(suppress_textchanged)return;
			((TextInformationFrame)this.Frame).Text=this.textBox1.Text;
		}
	}
}