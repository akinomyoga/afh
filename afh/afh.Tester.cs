namespace afh.Tester{
	/// <summary>
	/// ���̃N���X���͍\���̂ɁA�e�X�g�̑Ώۂ��܂܂�Ă��鎖��\������ׂ̑����ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class TestTargetAttribute:System.Attribute{
		/// <summary>
		/// TestTargetAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		public TestTargetAttribute(){}
	}
	/// <summary>
	/// �e�X�g�����s���郁�\�b�h�ł��鎖��m�点�鑮���ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class TestMethodAttribute:System.Attribute{
		private string desc;
		/// <summary>
		/// TestMethodAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		public TestMethodAttribute():this(""){}
		/// <summary>
		/// TestMethodAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="description">�e�X�g�̓��e�Ɋւ��ĊȌ��Ȑ������w�肵�܂��B</param>
		public TestMethodAttribute(string description){
			this.desc=description;
		}
		/// <summary>
		/// �e�X�g�̓��e�Ɋւ���Ȍ��Ȑ������擾���܂��B
		/// </summary>
		public string Description{
			get{return this.desc;}
		}
	}
	/// <summary>
	/// ���x�v�������s���郁�\�b�h�ł��鎖��m�点�鑮���ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class BenchMethodAttribute:System.Attribute{
		private string desc;
		/// <summary>
		/// BenchMethodAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		public BenchMethodAttribute():this(""){}
		/// <summary>
		/// BenchMethodAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="description">���x����̓��e�Ɋւ��ĊȌ��Ȑ������w�肵�܂��B</param>
		public BenchMethodAttribute(string description){
			this.desc=description;
		}
		/// <summary>
		/// �e�X�g�̓��e�Ɋւ���Ȍ��Ȑ������擾���܂��B
		/// </summary>
		public string Description {
			get { return this.desc; }
		}
	}
}