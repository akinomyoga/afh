//#define design
using Gen=System.Collections.Generic;
using CmM=System.ComponentModel;
namespace afh.File.ID3v2_3_{
	/// <summary>
	/// System.Collections.Gen.ICollection&lt;T&gt; を編集する Form です。
	/// </summary>
	public class TCONFrameEditBox
#if design
		:dummyTCONFrameEditor
#else
		:afh.Collections.CollectionEditorBase<Genre>
#endif
	{
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.ListBox listCand;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Splitter splitter1;

		private TCONFrame frame;
		/// <summary>
		/// 現在の編集の対象である TCONFrame を取得亦は設定します。
		/// null 値は現在編集できる TCONFrame が無い事を示します。
		/// </summary>
		[CmM.Browsable(false)]
		[CmM::DesignerSerializationVisibility(CmM::DesignerSerializationVisibility.Hidden)]
		public TCONFrame Frame{
			get{return this.frame;}
			set{
				this.frame=value;
				if(this.Enabled=value!=null){
					this.List=value.Genres;

					this.suppressTextChange=true;
					this.textDescription.Text=value.Description;
					this.suppressTextChange=false;
				}else{
					this.List=null;
					this.suppressTextChange=true;
					this.textDescription.Text="";
					this.suppressTextChange=false;
				}
			}
		}
		//=================================================
		//		Constructor
		//=================================================
		/// <summary>
		/// CollectionEditor のコンストラクタです。
		/// 指定した <see cref="Gen::IList&lt;Genre&gt;"/> を使用して初期化を実行します。
		/// </summary>
		public TCONFrameEditBox(TCONFrame frame):base(frame.Genres){
			this.InitializeComponent();
			this.InitializeComponent2();
			this.Frame=frame;
		}
		/// <summary>
		/// CollectionEditor のコンストラクタです。
		/// </summary>
		public TCONFrameEditBox():base(){
			this.InitializeComponent();
			this.InitializeComponent2();
			this.Frame=null;
		}
		private void InitializeComponent2(){
			const System.Reflection.BindingFlags PulicStatic=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public;
			System.Reflection.FieldInfo[] finfos=typeof(Genre).GetFields(PulicStatic);
			for(int i=0;i<finfos.Length;i++){
				this.listCand.Items.Add(new GenreItem(finfos[i]));
			}
			this.listCand.SelectedIndex=0;

			this.ShowString=this.contentEditorBase_ShowString;
		}
		private struct GenreItem{
			private System.Reflection.FieldInfo finfo;
			private string desc;
			public GenreItem(System.Reflection.FieldInfo finfo){
				this.finfo=finfo;
				this.desc="";
				this.desc=TCONFrame.GenreToId((Genre)finfo.GetValue(null))+" "+afh.Enum.GetDescription(finfo);
			}
			public Genre Value{
				get{return (Genre)this.finfo.GetValue(null);}
			}
			public string Description{
				get{return this.desc;}
			}
			public override string ToString() {
				return this.desc;
			}
		}
		//=================================================
		//		override CollectionEditor
		//=================================================
		/// <summary>
		/// 一覧に追加する為、<see cref="Genre"/> の新しいインスタンスを作成します。
		/// </summary>
		/// <returns>作成した <see cref="Genre"/> のインスタンスを返します。</returns>
		protected override Genre CreateNewInstance(){
			if(this.listCand.SelectedItem!=null)
				return ((GenreItem)this.listCand.SelectedItem).Value;
			return new Genre();
		}
		/// <summary>
		/// 指定した項目を PropertyGrid に設定します。
		/// </summary>
		/// <param name="index">項目の番号を指定します。</param>
		protected override void SetToEditor(int index){
			return;
		}

		#region Designer Code
		private void InitializeComponent() {
			this.splitter1=new System.Windows.Forms.Splitter();
			this.panel2=new System.Windows.Forms.Panel();
			this.textDescription=new System.Windows.Forms.TextBox();
			this.label1=new System.Windows.Forms.Label();
			this.listCand=new System.Windows.Forms.ListBox();
			this.label2=new System.Windows.Forms.Label();
			this.panel3=new System.Windows.Forms.Panel();
			this.label3=new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Dock=System.Windows.Forms.DockStyle.Left;
			this.panel1.Location=new System.Drawing.Point(162,49);
			this.panel1.Size=new System.Drawing.Size(33,220);
			// 
			// listBox1
			// 
			this.listBox1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.listBox1.Location=new System.Drawing.Point(195,67);
			this.listBox1.Size=new System.Drawing.Size(155,196);
			// 
			// splitter1
			// 
			this.splitter1.Location=new System.Drawing.Point(159,49);
			this.splitter1.Name="splitter1";
			this.splitter1.Size=new System.Drawing.Size(3,220);
			this.splitter1.TabIndex=6;
			this.splitter1.TabStop=false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.textDescription);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock=System.Windows.Forms.DockStyle.Top;
			this.panel2.Location=new System.Drawing.Point(3,3);
			this.panel2.Name="panel2";
			this.panel2.Padding=new System.Windows.Forms.Padding(5);
			this.panel2.Size=new System.Drawing.Size(347,46);
			this.panel2.TabIndex=7;
			// 
			// textDescription
			// 
			this.textDescription.Dock=System.Windows.Forms.DockStyle.Bottom;
			this.textDescription.Location=new System.Drawing.Point(5,22);
			this.textDescription.Name="textDescription";
			this.textDescription.Size=new System.Drawing.Size(337,19);
			this.textDescription.TabIndex=1;
			this.textDescription.TextChanged+=new System.EventHandler(this.textDescription_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(3,7);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(178,12);
			this.label1.TabIndex=0;
			this.label1.Text="Content-Type に関する詳細な説明";
			// 
			// listCand
			// 
			this.listCand.Dock=System.Windows.Forms.DockStyle.Fill;
			this.listCand.FormattingEnabled=true;
			this.listCand.ItemHeight=12;
			this.listCand.Location=new System.Drawing.Point(0,18);
			this.listCand.Name="listCand";
			this.listCand.Size=new System.Drawing.Size(156,196);
			this.listCand.TabIndex=8;
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Dock=System.Windows.Forms.DockStyle.Top;
			this.label2.Location=new System.Drawing.Point(0,0);
			this.label2.Margin=new System.Windows.Forms.Padding(3);
			this.label2.Name="label2";
			this.label2.Padding=new System.Windows.Forms.Padding(3);
			this.label2.Size=new System.Drawing.Size(102,18);
			this.label2.TabIndex=9;
			this.label2.Text="追加する候補一覧";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.listCand);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock=System.Windows.Forms.DockStyle.Left;
			this.panel3.Location=new System.Drawing.Point(3,49);
			this.panel3.Name="panel3";
			this.panel3.Size=new System.Drawing.Size(156,220);
			this.panel3.TabIndex=10;
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Dock=System.Windows.Forms.DockStyle.Top;
			this.label3.Location=new System.Drawing.Point(195,49);
			this.label3.Name="label3";
			this.label3.Padding=new System.Windows.Forms.Padding(3);
			this.label3.Size=new System.Drawing.Size(148,18);
			this.label3.TabIndex=11;
			this.label3.Text="現在の Content-Type 一覧";
			// 
			// TCONFrameEditBox
			// 
			this.Controls.Add(this.label3);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Name="TCONFrameEditBox";
			this.Padding=new System.Windows.Forms.Padding(3);
			this.Size=new System.Drawing.Size(353,272);
			this.Controls.SetChildIndex(this.panel2,0);
			this.Controls.SetChildIndex(this.panel3,0);
			this.Controls.SetChildIndex(this.splitter1,0);
			this.Controls.SetChildIndex(this.panel1,0);
			this.Controls.SetChildIndex(this.label3,0);
			this.Controls.SetChildIndex(this.listBox1,0);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private string contentEditorBase_ShowString(Genre g){
			return TCONFrame.GenreToId(g)+" "+afh.Enum.GetDescription(g);
		}
		private bool suppressTextChange=false;
		private void textDescription_TextChanged(object sender,System.EventArgs e){
			if(suppressTextChange)return;
			this.frame.Description=this.textDescription.Text;
		}
	}

#if design
	public class dummyTCONFrameEditor:afh.Collections.CollectionEditorBase<Genre>{
		public dummyTCONFrameEditor(Gen::IList<Genre> items):base(items){}
		public dummyTCONFrameEditor():base(){}
		protected override void SetToEditor(int index){}
		protected override Genre CreateNewInstance(){return 0;}
	}
#endif

}