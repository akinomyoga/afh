using afh.File.Mp3_;

namespace afh.File.ID3v2_3_{
	/// <summary>
	/// 単一 frame 編集用コントロールの基本クラス。
	/// これは、tag 内に最高でも一つしか含まれない種類の Frame を編集する為の UserControl。
	/// </summary>
	/// <remarks>
	/// 実際に使う際には <see cref="M:ExistsFrame"/> 及び <see cref="FilterFrameId"/>, <see cref="M:ApplyToControl"/> を override して下さい。
	/// ExistsFrame 及び FilterFrameId では必ず基底クラスの物を呼び出して下さい。
	/// <code>
	/// public override bool ExistsFrame{
	///		set{
	///			base.ExistsFrame=value;
	///			// .. 各種処理が続く
	///		}
	/// }
	/// 
	/// public override void FilterFrameId(string frameId){
	///		base.FilterFrameId();
	///		// .. 適宜 System.ApplicationException を投げる
	/// }
	/// </code>
	/// </remarks>
	public class SingleFrameEditor:Mp3DataControl{
		protected System.Windows.Forms.Label label1;
		protected System.Windows.Forms.Button button1;
	
		//=================================================
		//		Fields
		//=================================================
		private string frameId="<frame>";
	
		public string FrameId{
			get{return this.frameId;}
			set{
				try{
					this.FilterFrameId(value);
				}catch(System.ApplicationException e){
					__dll__.log.WriteError(e,"指定した FrameId はこの UserControl で編集するのに不適切な物であると判定されました。");
					value="<frame>";
				}

				this.frameId=value;
				if(Frame.FramesData.ContainsKey(this.frameId)){
					this.label1.Text=this.frameId+": "+Frame.FramesData[this.frameId].JpnName;
					ToolTipDescription=Frame.FramesData[this.frameId].JpnDescription;
				}else{
					this.label1.Text=this.frameId+": 不明な種類の Text Information Frame です。";
				}
				this.UpdateView();
			}
		}
		/// <summary>
		/// 受容できる FrameId を限定します。
		/// 受容できないと判定した場合には System.ApplicationException を投げます。
		/// 簡単に言えば、指定した frameId を持つ frame をこの Control で編集するのは適切でないと判断される時に
		/// System.ApplicationException を投げます。
		/// <para>こうして、受容できないと判定された frameId の場合には frameId が設定されていない状態になります。</para>
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// 指定した frameId を持つ frame をこの Control で編集するのは適切でないと判断される時に発生します。
		/// </exception>
		protected virtual void FilterFrameId(string value){
			if(value==null||value.Length!=4)
				throw new System.ApplicationException("FrameId には四文字の文字列を指定して下さい。");
		}

		private bool existsframe=false;
		/// <summary>
		/// 対応する Frame が存在する時と存在しない時で編集用コントロールの状態を変化させます。
		/// </summary>
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		protected virtual bool ExistsFrame{
			set{
				this.existsframe=value;
				if(value){
					ToolTipButton=this.frameId+" Frame は tag 内に存在していますので編集できます。削除するには [削除] を押して下さい";
					this.button1.Text="Frame 削除";
				}else{
					ToolTipButton=this.frameId+" Frame は tag 内に存在しません。編集するには [作成] を押して下さい";
					this.button1.Text="Frame 作成";
				}
			}
		}

		//=================================================
		//		Constructor
		//=================================================
		protected Frame Frame{
			get{return this.existsframe?this.tag230.Frames[frameId][0]:null;}
		}
		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			if(disposing){
				if(this.font_original!=null)this.font_original.Dispose();
				if(this.font_underline!=null)this.font_underline.Dispose();
			}
		}

		#region Tooltip
		//=================================================
		//		Tooltop の表示
		//=================================================
		private System.Windows.Forms.ToolTip tooltip=null;
		private System.Windows.Forms.Control tooltip_target;
		/// <summary>
		/// ToolTip を表示する場所を指定します。既定値はこのコントロール全体です。
		/// </summary>
		protected System.Windows.Forms.Control ToolTipTarget{
			get{return this.tooltip_target??this;}
			set{this.tooltip_target=value;}
		}
		private void set_tooltip(){
			if(this.tooltip!=null)this.tooltip.SetToolTip(this.ToolTipTarget,label_desc+"\r\n"+label_button);
		}
		private void show_tooltip(){
			if(this.tooltip!=null)this.tooltip.Show(label_desc+"\r\n"+label_button,this.label1,0,20,5000);
		}
		public System.Windows.Forms.ToolTip ToolTip{
			get{return this.tooltip;}
			set{
				this.tooltip=value;
				set_tooltip();
			}
		}
		private string label_button="";
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		protected string ToolTipButton{
			get{return this.label_button;}
			set{
				this.label_button=value;
				set_tooltip();
			}
		}
		private string label_desc="";
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		protected string ToolTipDescription{
			get{return this.label_desc;}
			set{
				this.label_desc=value;
				set_tooltip();
			}
		}
		#endregion

		//=================================================
		//		Update
		//=================================================
		protected override void OnUpdateTag(){
			base.OnUpdateTag();
			this.UpdateView();
		}

		private void UpdateView(){
			if(this.file==null||this.tag230==null){
				this.ExistsFrame=false;
			}else{
				this.ExistsFrame=this.tag230.Frames.ContainsKey(this.frameId);
				this.ApplyToControl(this.Frame);
			}
		}
		/// <summary>
		/// Frame の内容を編集用の Control に適用します。
		/// </summary>
		/// <param name="frame">
		/// Control に適用する frame を指定します。
		/// 対応する frame が存在しない時には null を指定します。
		/// </param>
		protected virtual void ApplyToControl(Frame frame){
		}
		
		//=================================================
		//		Constructor
		//=================================================
		public SingleFrameEditor():base(){
			this.InitializeComponent();
			this.UpdateView();
		}

		#region Desinger Code
		private void InitializeComponent() {
			this.button1=new System.Windows.Forms.Button();
			this.label1=new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location=new System.Drawing.Point(0,18);
			this.button1.Name="button1";
			this.button1.Size=new System.Drawing.Size(80,20);
			this.button1.TabIndex=0;
			this.button1.Text="Frame 作成";
			this.button1.UseVisualStyleBackColor=true;
			this.button1.Click+=new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(0,0);
			this.label1.Name="label1";
			this.label1.Padding=new System.Windows.Forms.Padding(3);
			this.label1.Size=new System.Drawing.Size(114,18);
			this.label1.TabIndex=2;
			this.label1.Text="<frame>: description";
			this.label1.MouseLeave+=new System.EventHandler(this.label1_MouseLeave);
			this.label1.Click+=new System.EventHandler(this.label1_Click);
			this.label1.MouseEnter+=new System.EventHandler(this.label1_MouseEnter);
			// 
			// SingleFrameEditor
			// 
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name="SingleFrameEditor";
			this.Size=new System.Drawing.Size(325,40);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void button1_Click(object sender,System.EventArgs e){
			if(this.tag230==null){
				throw new System.ApplicationException("ID3v2.3.0 tag が指定されていません。");
			}
			if(this.FrameId=="<frame>"){
				throw new System.ApplicationException("<frame> は「何の TextInformationFrame も設定されていない」という意味なので、\r\n"
					+"新しく Frame を作成したり既存の Frame を削除したりする事は出来ません。");
			}
			if(this.existsframe){
				// 削除
				this.tag230.Frames.RemoveAll(this.FrameId);
			}else{
				//作成
				this.tag230.Frames.Add(this.FrameId,Frame.CreateFrame(this.FrameId));
			}
			this.UpdateView();
		}

		private void label1_Click(object sender,System.EventArgs e) {
			show_tooltip();
		}

		private bool font_initialized=false;
		private System.Drawing.Font font_original=null;
		private System.Drawing.Font font_underline=null;
		private void font_initialize(){
			if(this.font_initialized)return;
			this.font_original=this.label1.Font;
			this.font_underline=new System.Drawing.Font(this.Font,System.Drawing.FontStyle.Underline);
			this.font_initialized=true;
		}
		private void label1_MouseEnter(object sender,System.EventArgs e) {
			font_initialize();
			this.label1.Font=this.font_underline;
		}
		private void label1_MouseLeave(object sender,System.EventArgs e) {
			font_initialize();
			this.label1.Font=this.font_original;
			if(this.tooltip!=null)this.tooltip.Hide(this.label1);
		}
	}
}