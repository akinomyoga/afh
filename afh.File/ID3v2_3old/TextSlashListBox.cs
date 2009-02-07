namespace afh.File.ID3v2_3_{
	/// <summary>
	/// 単一 TextInformationFrame 用編集 TextBox のクラスです。
	/// 表面の Button を使用して Frame を Tag に追加したり、Tag から削除したりする事が可能です。
	/// </summary>
	public sealed class TextSlashListBox:SingleFrameEditor{
		private System.Windows.Forms.TextBox textBox1;

		public TextSlashListBox():base(){
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
			this.button1.Location=new System.Drawing.Point(195,18);
			this.button1.MaximumSize=new System.Drawing.Size(100,20);
			this.button1.MinimumSize=new System.Drawing.Size(80,20);
			// 
			// textBox1
			// 
			this.textBox1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location=new System.Drawing.Point(0,18);
			this.textBox1.Multiline=true;
			this.textBox1.Name="textBox1";
			this.textBox1.ScrollBars=System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size=new System.Drawing.Size(195,52);
			this.textBox1.TabIndex=1;
			this.textBox1.TextChanged+=new System.EventHandler(this.textBox1_TextChanged);
			// 
			// TextSlashListBox
			// 
			this.Controls.Add(this.textBox1);
			this.MinimumSize=new System.Drawing.Size(200,70);
			this.Name="TextSlashListBox";
			this.Size=new System.Drawing.Size(275,70);
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
		protected override void ApplyToControl(Frame frame) {
			if(frame==null){
				this.textBox1.Text="";
			}else{
				this.textBox1.Lines=((SlashNameListFrame)frame).Names.ToArray();
			}
		}
		protected override void FilterFrameId(string value) {
			base.FilterFrameId(value);
			if(!Frame.FramesData.ContainsKey(value)||Frame.FramesData[value].FrameType!="slash-list")
				throw new System.ApplicationException("指定した FrameId は \"/ で区切った Text Information Frame\" の物ではありません。");
		}

		private void textBox1_TextChanged(object sender,System.EventArgs e){
			SlashNameListFrame frame=(SlashNameListFrame)this.Frame;
			frame.Names.Clear();
			string name2;
			foreach(string name in this.textBox1.Lines){
				name2=name.Trim();
				if(name.Length>0)frame.Names.Add(name);
			}
		}
	}
}