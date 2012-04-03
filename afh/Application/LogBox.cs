namespace afh.Application{
	/// <summary>
	/// afh.Application.LogBox に対応する内容を表示する TextBox です。
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(LogBox))]
	public class LogBox:System.Windows.Forms.TextBox{
		/// <summary>
		/// 既定の LogBox コンストラクタです。
		/// </summary>
		public LogBox():this(""){}
		/// <summary>
		/// LogBox コンストラクタ
		/// </summary>
		/// <param name="name">このログに関連付ける名前を指定します。</param>
		public LogBox(string name){
			this.InitializeComponent();
			this.Name=name;
		}

		#region デザイナコード
		private void InitializeComponent(){
			// 
			// LogBox
			// 
			this.AcceptsReturn = true;
			this.AcceptsTab = true;
			this.BackColor = System.Drawing.Color.Black;
			this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.ForeColor = System.Drawing.Color.Silver;
			this.Multiline = true;
			this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.WordWrap = false;
		}
		#endregion

		/// <summary>
		/// この LogBox インスタンスの名前を表す文字列を指定します。
		/// </summary>
		/// <returns>この LogBox インスタンスの名前を返します。</returns>
		public override string ToString(){return this.Name;}
	}
}