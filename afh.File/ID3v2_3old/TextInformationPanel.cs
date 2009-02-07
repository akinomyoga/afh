using afh.File.Mp3_;

namespace afh.File.ID3v2_3_{
	public class TextInformationPanel:Mp3DataControl {
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private TextSlashListBox txtTCOM;
		private TextSlashListBox txtTEXT;
		private TextInformationEditor txtTIT1;
		private TextInformationEditor txtTIT2;
		private TextInformationEditor txtTIT3;
		private TextInformationEditor txtTOFN;
		private TextInformationEditor txtTOWN;
		private TextSlashListBox txtTPE1;
		private TextInformationEditor txtTPE2;
		private TextInformationEditor txtTPE3;
		private TextInformationEditor txtTPE4;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private TextInformationEditor txtTALB;
	
		private bool initialized=false;
		public TextInformationPanel():base(){
			this.InitializeComponent();
			this.UpdateFile();
			this.initialized=true;
		}
		private void UpdateFile(){
			this.txtTALB.File=this.File;
			this.txtTCOM.File=this.File;
			this.txtTEXT.File=this.File;
			this.txtTIT1.File=this.File;
			this.txtTIT2.File=this.File;
			this.txtTIT3.File=this.File;
			this.txtTOFN.File=this.File;
			this.txtTOWN.File=this.File;
			this.txtTPE1.File=this.File;
			this.txtTPE2.File=this.File;
			this.txtTPE3.File=this.File;
			this.txtTPE4.File=this.File;
		}
		public override MP3File File {
			get {return base.File;}
			set {
				base.File=value;
				if(initialized)this.UpdateFile();
			}
		}
		protected override void OnUpdateTag(){
			base.OnUpdateTag();
		}

		#region Designer Code
		private void InitializeComponent() {
			this.components=new System.ComponentModel.Container();
			this.txtTALB=new afh.File.ID3v2_3_.TextInformationEditor();
			this.toolTip1=new System.Windows.Forms.ToolTip(this.components);
			this.txtTCOM=new afh.File.ID3v2_3_.TextSlashListBox();
			this.txtTEXT=new afh.File.ID3v2_3_.TextSlashListBox();
			this.txtTIT1=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTIT2=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTIT3=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTOFN=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTOWN=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTPE1=new afh.File.ID3v2_3_.TextSlashListBox();
			this.txtTPE2=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTPE3=new afh.File.ID3v2_3_.TextInformationEditor();
			this.txtTPE4=new afh.File.ID3v2_3_.TextInformationEditor();
			this.splitContainer1=new System.Windows.Forms.SplitContainer();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtTALB
			// 
			this.txtTALB.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTALB.FrameId="TALB";
			this.txtTALB.Location=new System.Drawing.Point(5,5);
			this.txtTALB.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTALB.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTALB.Name="txtTALB";
			this.txtTALB.Size=new System.Drawing.Size(251,40);
			this.txtTALB.TabIndex=0;
			this.txtTALB.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTALB,"File 内の音声の音源の題名。\r\nTALB Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTCOM
			// 
			this.txtTCOM.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTCOM.FrameId="TCOM";
			this.txtTCOM.Location=new System.Drawing.Point(5,75);
			this.txtTCOM.MinimumSize=new System.Drawing.Size(200,70);
			this.txtTCOM.Name="txtTCOM";
			this.txtTCOM.Size=new System.Drawing.Size(253,70);
			this.txtTCOM.TabIndex=1;
			this.txtTCOM.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTCOM,"作曲者の名前。\r\nTCOM Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTEXT
			// 
			this.txtTEXT.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTEXT.FrameId="TEXT";
			this.txtTEXT.Location=new System.Drawing.Point(5,5);
			this.txtTEXT.MinimumSize=new System.Drawing.Size(200,70);
			this.txtTEXT.Name="txtTEXT";
			this.txtTEXT.Size=new System.Drawing.Size(253,70);
			this.txtTEXT.TabIndex=2;
			this.txtTEXT.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTEXT,"録音時の文章及び歌詞の著作者。\r\nTEXT Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTIT1
			// 
			this.txtTIT1.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTIT1.FrameId="TIT1";
			this.txtTIT1.Location=new System.Drawing.Point(5,45);
			this.txtTIT1.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTIT1.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTIT1.Name="txtTIT1";
			this.txtTIT1.Size=new System.Drawing.Size(251,40);
			this.txtTIT1.TabIndex=3;
			this.txtTIT1.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTIT1,"音声がより大きな音声分類に属する時に使用されます。\r\nTIT1 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTIT2
			// 
			this.txtTIT2.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTIT2.FrameId="TIT2";
			this.txtTIT2.Location=new System.Drawing.Point(5,85);
			this.txtTIT2.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTIT2.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTIT2.Name="txtTIT2";
			this.txtTIT2.Size=new System.Drawing.Size(251,40);
			this.txtTIT2.TabIndex=4;
			this.txtTIT2.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTIT2,"その作品の実際の名前。\r\nTIT2 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTIT3
			// 
			this.txtTIT3.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTIT3.FrameId="TIT3";
			this.txtTIT3.Location=new System.Drawing.Point(5,125);
			this.txtTIT3.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTIT3.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTIT3.Name="txtTIT3";
			this.txtTIT3.Size=new System.Drawing.Size(251,40);
			this.txtTIT3.TabIndex=5;
			this.txtTIT3.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTIT3,"内容の題に直接的に関連する情報。\r\nTIT3 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTOFN
			// 
			this.txtTOFN.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTOFN.FrameId="TOFN";
			this.txtTOFN.Location=new System.Drawing.Point(5,165);
			this.txtTOFN.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTOFN.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTOFN.Name="txtTOFN";
			this.txtTOFN.Size=new System.Drawing.Size(251,40);
			this.txtTOFN.TabIndex=6;
			this.txtTOFN.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTOFN,"File のあるべき名前。\r\nTOFN Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTOWN
			// 
			this.txtTOWN.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTOWN.FrameId="TOWN";
			this.txtTOWN.Location=new System.Drawing.Point(5,205);
			this.txtTOWN.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTOWN.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTOWN.Name="txtTOWN";
			this.txtTOWN.Size=new System.Drawing.Size(251,40);
			this.txtTOWN.TabIndex=7;
			this.txtTOWN.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTOWN,"File の所有者及び使用許可とその内容を記述するのに使用。\r\nTOWN Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTPE1
			// 
			this.txtTPE1.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTPE1.FrameId="TPE1";
			this.txtTPE1.Location=new System.Drawing.Point(5,145);
			this.txtTPE1.MinimumSize=new System.Drawing.Size(200,70);
			this.txtTPE1.Name="txtTPE1";
			this.txtTPE1.Size=new System.Drawing.Size(253,70);
			this.txtTPE1.TabIndex=8;
			this.txtTPE1.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTPE1,"主な演者。\r\nTPE1 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTPE2
			// 
			this.txtTPE2.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTPE2.FrameId="TPE2";
			this.txtTPE2.Location=new System.Drawing.Point(5,215);
			this.txtTPE2.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTPE2.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTPE2.Name="txtTPE2";
			this.txtTPE2.Size=new System.Drawing.Size(253,40);
			this.txtTPE2.TabIndex=9;
			this.txtTPE2.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTPE2,"音声の演者に関する追加的な情報。\r\nTPE2 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTPE3
			// 
			this.txtTPE3.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTPE3.FrameId="TPE3";
			this.txtTPE3.Location=new System.Drawing.Point(5,255);
			this.txtTPE3.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTPE3.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTPE3.Name="txtTPE3";
			this.txtTPE3.Size=new System.Drawing.Size(253,40);
			this.txtTPE3.TabIndex=10;
			this.txtTPE3.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTPE3,"指揮者の名前。\r\nTPE3 Frame は tag 内に存在しません。編集するには [作成] を押して下さい");
			// 
			// txtTPE4
			// 
			this.txtTPE4.Dock=System.Windows.Forms.DockStyle.Top;
			this.txtTPE4.FrameId="TPE4";
			this.txtTPE4.Location=new System.Drawing.Point(5,295);
			this.txtTPE4.MaximumSize=new System.Drawing.Size(1024,40);
			this.txtTPE4.MinimumSize=new System.Drawing.Size(200,40);
			this.txtTPE4.Name="txtTPE4";
			this.txtTPE4.Size=new System.Drawing.Size(253,40);
			this.txtTPE4.TabIndex=11;
			this.txtTPE4.ToolTip=this.toolTip1;
			this.toolTip1.SetToolTip(this.txtTPE4,"他の既存の作品を remix 亦は似た様な説明を行った際に貢献した人。\r\nTPE4 Frame は tag 内に存在しません。編集するには [作成] を押して下さ"+
					"い");
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location=new System.Drawing.Point(0,0);
			this.splitContainer1.Name="splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.AutoScroll=true;
			this.splitContainer1.Panel1.Controls.Add(this.txtTOWN);
			this.splitContainer1.Panel1.Controls.Add(this.txtTOFN);
			this.splitContainer1.Panel1.Controls.Add(this.txtTIT3);
			this.splitContainer1.Panel1.Controls.Add(this.txtTIT2);
			this.splitContainer1.Panel1.Controls.Add(this.txtTIT1);
			this.splitContainer1.Panel1.Controls.Add(this.txtTALB);
			this.splitContainer1.Panel1.Padding=new System.Windows.Forms.Padding(5);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AutoScroll=true;
			this.splitContainer1.Panel2.Controls.Add(this.txtTPE4);
			this.splitContainer1.Panel2.Controls.Add(this.txtTPE3);
			this.splitContainer1.Panel2.Controls.Add(this.txtTPE2);
			this.splitContainer1.Panel2.Controls.Add(this.txtTPE1);
			this.splitContainer1.Panel2.Controls.Add(this.txtTCOM);
			this.splitContainer1.Panel2.Controls.Add(this.txtTEXT);
			this.splitContainer1.Panel2.Padding=new System.Windows.Forms.Padding(5);
			this.splitContainer1.Size=new System.Drawing.Size(528,359);
			this.splitContainer1.SplitterDistance=261;
			this.splitContainer1.TabIndex=12;
			// 
			// TextInformationPanel
			// 
			this.AutoScroll=true;
			this.Controls.Add(this.splitContainer1);
			this.Name="TextInformationPanel";
			this.Size=new System.Drawing.Size(528,359);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}