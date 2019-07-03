using afh.File.Mp3_;

namespace afh.File.ID3v2_3_{
	/// <summary>
	/// �P�� frame �ҏW�p�R���g���[���̊�{�N���X�B
	/// ����́Atag ���ɍō��ł�������܂܂�Ȃ���ނ� Frame ��ҏW����ׂ� UserControl�B
	/// </summary>
	/// <remarks>
	/// ���ۂɎg���ۂɂ� <see cref="M:ExistsFrame"/> �y�� <see cref="FilterFrameId"/>, <see cref="M:ApplyToControl"/> �� override ���ĉ������B
	/// ExistsFrame �y�� FilterFrameId �ł͕K�����N���X�̕����Ăяo���ĉ������B
	/// <code>
	/// public override bool ExistsFrame{
	///		set{
	///			base.ExistsFrame=value;
	///			// .. �e�폈��������
	///		}
	/// }
	/// 
	/// public override void FilterFrameId(string frameId){
	///		base.FilterFrameId();
	///		// .. �K�X System.ApplicationException �𓊂���
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
					__dll__.log.WriteError(e,"�w�肵�� FrameId �͂��� UserControl �ŕҏW����̂ɕs�K�؂ȕ��ł���Ɣ��肳��܂����B");
					value="<frame>";
				}

				this.frameId=value;
				if(Frame.FramesData.ContainsKey(this.frameId)){
					this.label1.Text=this.frameId+": "+Frame.FramesData[this.frameId].JpnName;
					ToolTipDescription=Frame.FramesData[this.frameId].JpnDescription;
				}else{
					this.label1.Text=this.frameId+": �s���Ȏ�ނ� Text Information Frame �ł��B";
				}
				this.UpdateView();
			}
		}
		/// <summary>
		/// ��e�ł��� FrameId �����肵�܂��B
		/// ��e�ł��Ȃ��Ɣ��肵���ꍇ�ɂ� System.ApplicationException �𓊂��܂��B
		/// �ȒP�Ɍ����΁A�w�肵�� frameId ������ frame ������ Control �ŕҏW����͓̂K�؂łȂ��Ɣ��f����鎞��
		/// System.ApplicationException �𓊂��܂��B
		/// <para>�������āA��e�ł��Ȃ��Ɣ��肳�ꂽ frameId �̏ꍇ�ɂ� frameId ���ݒ肳��Ă��Ȃ���ԂɂȂ�܂��B</para>
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// �w�肵�� frameId ������ frame ������ Control �ŕҏW����͓̂K�؂łȂ��Ɣ��f����鎞�ɔ������܂��B
		/// </exception>
		protected virtual void FilterFrameId(string value){
			if(value==null||value.Length!=4)
				throw new System.ApplicationException("FrameId �ɂ͎l�����̕�������w�肵�ĉ������B");
		}

		private bool existsframe=false;
		/// <summary>
		/// �Ή����� Frame �����݂��鎞�Ƒ��݂��Ȃ����ŕҏW�p�R���g���[���̏�Ԃ�ω������܂��B
		/// </summary>
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		protected virtual bool ExistsFrame{
			set{
				this.existsframe=value;
				if(value){
					ToolTipButton=this.frameId+" Frame �� tag ���ɑ��݂��Ă��܂��̂ŕҏW�ł��܂��B�폜����ɂ� [�폜] �������ĉ�����";
					this.button1.Text="Frame �폜";
				}else{
					ToolTipButton=this.frameId+" Frame �� tag ���ɑ��݂��܂���B�ҏW����ɂ� [�쐬] �������ĉ�����";
					this.button1.Text="Frame �쐬";
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
		//		Tooltop �̕\��
		//=================================================
		private System.Windows.Forms.ToolTip tooltip=null;
		private System.Windows.Forms.Control tooltip_target;
		/// <summary>
		/// ToolTip ��\������ꏊ���w�肵�܂��B����l�͂��̃R���g���[���S�̂ł��B
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
		/// Frame �̓��e��ҏW�p�� Control �ɓK�p���܂��B
		/// </summary>
		/// <param name="frame">
		/// Control �ɓK�p���� frame ���w�肵�܂��B
		/// �Ή����� frame �����݂��Ȃ����ɂ� null ���w�肵�܂��B
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
			this.button1.Text="Frame �쐬";
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
				throw new System.ApplicationException("ID3v2.3.0 tag ���w�肳��Ă��܂���B");
			}
			if(this.FrameId=="<frame>"){
				throw new System.ApplicationException("<frame> �́u���� TextInformationFrame ���ݒ肳��Ă��Ȃ��v�Ƃ����Ӗ��Ȃ̂ŁA\r\n"
					+"�V���� Frame ���쐬����������� Frame ���폜�����肷�鎖�͏o���܂���B");
			}
			if(this.existsframe){
				// �폜
				this.tag230.Frames.RemoveAll(this.FrameId);
			}else{
				//�쐬
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