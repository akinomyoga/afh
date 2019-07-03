#define design
using afh.File.MP3;

namespace afh.File.ID3v2_3_{
	/// <summary>
	/// �P�� TextInformationFrame �p�ҏW TextBox �̃N���X�ł��B
	/// ����́Atag ���ɍō��ł�������܂܂�Ȃ���ނ� TextInformationFrame ��ҏW����ׂ� TextBox �ł��B
	/// �\�ʂ� Button ���g�p���� Frame �� Tag �ɒǉ�������ATag ����폜�����肷�鎖���\�ł��B
	/// </summary>
	public class TextInformationBox0:Mp3DataControl{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
	
		//=================================================
		//		Tag Editor
		//=================================================
		private string frameId="<frame>";
	
		public string FrameId{
			get{return this.frameId;}
			set{
				if(value==null||value[0]!='T'||value=="TXXX"){
					if(value!="<frame>"&&__dll__.log!=null){
						__dll__.log.WriteError(new System.ArgumentException("value","�w�肵�� frameId �� Text Information Frame �̕��ł͂���܂���B"));
						value="<frame>";
					}
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

		private bool existsframe=false;
		private bool ExistsFrame{
			set{
				this.existsframe=value;
				this.textBox1.Enabled=value;
				if(value){
					ToolTipButton=this.frameId+" Frame �� tag ���ɑ��݂��Ă��܂��̂ŕҏW�ł��܂��B�폜����ɂ� [�폜] �������ĉ�����";
					this.button1.Text="Frame �폜";
				}else{
					ToolTipButton=this.frameId+" Frame �� tag ���ɑ��݂��܂���B�ҏW����ɂ� [�쐬] �������ĉ�����";
					this.button1.Text="Frame �쐬";
					this.textBox1.Text="";
				}
			}
		}

		//=================================================
		//		Tooltop �̕\��
		//=================================================
		private System.Windows.Forms.ToolTip tooltip=null;
		private void set_tooltip(){
			if(this.tooltip!=null)this.tooltip.SetToolTip(this,label_desc+"\r\n"+label_button);
		}
		public System.Windows.Forms.ToolTip ToolTip{
			get{return this.tooltip;}
			set{
				this.tooltip=value;
				set_tooltip();
			}
		}
		private string label_button="";
		private string ToolTipButton{
			get{return this.label_button;}
			set{
				this.label_button=value;
				set_tooltip();
			}
		}
		private string label_desc="";
		private string ToolTipDescription{
			get{return this.label_desc;}
			set{
				this.label_desc=value;
				set_tooltip();
			}
		}
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
				if(this.existsframe){
					this.textBox1.Text=((TextInformationFrame)this.tag230.Frames[frameId][0]).Text;
				}
			}
		}
		
		public TextInformationBox0():base(){
			this.InitializeComponent();
			this.UpdateView();
		}

		#region Desinger Code
		private void InitializeComponent() {
			this.button1=new System.Windows.Forms.Button();
			this.textBox1=new System.Windows.Forms.TextBox();
			this.label1=new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Dock=System.Windows.Forms.DockStyle.Right;
			this.button1.Location=new System.Drawing.Point(245,18);
			this.button1.MaximumSize=new System.Drawing.Size(100,20);
			this.button1.MinimumSize=new System.Drawing.Size(80,20);
			this.button1.Name="button1";
			this.button1.Size=new System.Drawing.Size(80,20);
			this.button1.TabIndex=0;
			this.button1.Text="Frame �쐬";
			this.button1.UseVisualStyleBackColor=true;
			this.button1.Click+=new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location=new System.Drawing.Point(0,18);
			this.textBox1.Name="textBox1";
			this.textBox1.Size=new System.Drawing.Size(245,19);
			this.textBox1.TabIndex=1;
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Dock=System.Windows.Forms.DockStyle.Top;
			this.label1.Location=new System.Drawing.Point(0,0);
			this.label1.Name="label1";
			this.label1.Padding=new System.Windows.Forms.Padding(3);
			this.label1.Size=new System.Drawing.Size(114,18);
			this.label1.TabIndex=2;
			this.label1.Text="<frame>: description";
			// 
			// TextInformationBox
			// 
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.MaximumSize=new System.Drawing.Size(1024,40);
			this.MinimumSize=new System.Drawing.Size(200,40);
			this.Name="TextInformationBox";
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
	}
}