namespace afh.Tester{
	/// <summary>
	/// そのクラス又は構造体に、テストの対象が含まれている事を表示する為の属性です。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class TestTargetAttribute:System.Attribute{
		/// <summary>
		/// TestTargetAttribute のコンストラクタです。
		/// </summary>
		public TestTargetAttribute(){}
	}
	/// <summary>
	/// テストを実行するメソッドである事を知らせる属性です。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class TestMethodAttribute:System.Attribute{
		private string desc;
		/// <summary>
		/// TestMethodAttribute のコンストラクタです。
		/// </summary>
		public TestMethodAttribute():this(""){}
		/// <summary>
		/// TestMethodAttribute のコンストラクタです。
		/// </summary>
		/// <param name="description">テストの内容に関して簡潔な説明を指定します。</param>
		public TestMethodAttribute(string description){
			this.desc=description;
		}
		/// <summary>
		/// テストの内容に関する簡潔な説明を取得します。
		/// </summary>
		public string Description{
			get{return this.desc;}
		}
	}
	/// <summary>
	/// 速度計測を実行するメソッドである事を知らせる属性です。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class BenchMethodAttribute:System.Attribute{
		private string desc;
		/// <summary>
		/// BenchMethodAttribute のコンストラクタです。
		/// </summary>
		public BenchMethodAttribute():this(""){}
		/// <summary>
		/// BenchMethodAttribute のコンストラクタです。
		/// </summary>
		/// <param name="description">速度測定の内容に関して簡潔な説明を指定します。</param>
		public BenchMethodAttribute(string description){
			this.desc=description;
		}
		/// <summary>
		/// テストの内容に関する簡潔な説明を取得します。
		/// </summary>
		public string Description {
			get { return this.desc; }
		}
	}
}