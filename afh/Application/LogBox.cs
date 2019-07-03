namespace afh.Application{
	/// <summary>
	/// afh.Application.LogBox �ɑΉ�������e��\������ TextBox �ł��B
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogBox))]
	public class LogBox:System.Windows.Forms.TextBox{
		/// <summary>
		/// ����� LogBox �R���X�g���N�^�ł��B
		/// </summary>
		public LogBox():this(""){}
		/// <summary>
		/// LogBox �R���X�g���N�^
		/// </summary>
		/// <param name="name">���̃��O�Ɋ֘A�t���閼�O���w�肵�܂��B</param>
		public LogBox(string name){
			this.InitializeComponent();
			this.Name=name;
		}

		#region �f�U�C�i�R�[�h
		private void InitializeComponent(){
			// 
			// LogBox
			// 
			this.AcceptsReturn = true;
			this.AcceptsTab = true;
			this.BackColor = System.Drawing.Color.Black;
			this.Font = new System.Drawing.Font("�l�r �S�V�b�N", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.ForeColor = System.Drawing.Color.Silver;
			this.Multiline = true;
			this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.WordWrap = false;
		}
		#endregion

		/// <summary>
		/// ���� LogBox �C���X�^���X�̖��O��\����������w�肵�܂��B
		/// </summary>
		/// <returns>���� LogBox �C���X�^���X�̖��O��Ԃ��܂��B</returns>
		public override string ToString(){return this.Name;}
	}
}