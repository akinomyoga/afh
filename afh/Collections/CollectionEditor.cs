//#define design
using Gen=System.Collections.Generic;
#if design
using T=System.Int32;
#endif
namespace afh.Collections{
	/// <summary>
	/// System.Collections.Gen.ICollection&lt;T&gt; ��ҏW���� Form �ł��B
	/// </summary>
	/// <typeparam name="T">�W���̗v�f�̌^���w�肵�܂��B</typeparam>
	public class CollectionEditor
#if design
		:DummyBase
#else
		<T>:CollectionEditorBase<T> where T:new()
#endif
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		/// <summary>
		/// CollectionEditor �̃R���X�g���N�^�ł��B
		/// �w�肵�� <see cref="Gen::IList&lt;T&gt;"/> ���g�p���ď����������s���܂��B
		/// </summary>
		public CollectionEditor(Gen::IList<T> items):base(items){
			this.InitializeComponent();
		}
		/// <summary>
		/// CollectionEditor �̃R���X�g���N�^�ł��B
		/// </summary>
		public CollectionEditor():this(new Gen::List<T>()){}

		/// <summary>
		/// �ꗗ�ɒǉ�����ׁA<typeparamref name="T"/> �̐V�����C���X�^���X���쐬���܂��B
		/// </summary>
		/// <returns>�쐬���� <typeparamref name="T"/> �̃C���X�^���X��Ԃ��܂��B</returns>
		protected override T CreateNewInstance() {
			return new T();
		}
		/// <summary>
		/// �w�肵�����ڂ� PropertyGrid �ɐݒ肵�܂��B
		/// </summary>
		/// <param name="index">���ڂ̔ԍ����w�肵�܂��B</param>
		protected override void SetToEditor(int index){
			if(index<0||index>=this.List.Count){
				this.propertyGrid1.SelectedObject=null;
			}else if(typeof(T).IsPrimitive||typeof(T).IsEnum){
				this.propertyGrid1.SelectedObject=new ObjectForPropertyGrid(this.List,index);
			}else{
				this.propertyGrid1.SelectedObject=this.List[index];
			}
		}
		/// <summary>
		/// ��{�^���� PropertyGrid �ŕҏW�ł���悤�ɂ��邽�߂̕��ł��B
		/// </summary>
		protected struct ObjectForPropertyGrid{
			private int index;
			private Gen::IList<T> list;
			/// <summary>
			/// �w�肵�����X�g�Ɣԍ����g�p���� ObjectForPropertyGrid �����������܂��B
			/// </summary>
			/// <param name="list"><typeparamref name="T"/> �̒l���܂� <see cref="Gen::IList&lt;T&gt;"/> ���w�肵�܂��B</param>
			/// <param name="index"><paramref name="list"/> ���ɉ�����Ώۂ� <typeparamref name="T"/> �� 0 ����n�܂�ԍ����w�肵�܂��B</param>
			public ObjectForPropertyGrid(Gen::IList<T> list,int index){
				this.list=list;
				this.index=index;
			}
			/// <summary>
			/// �l���擾���͐ݒ肵�܂��B
			/// </summary>
			[System.ComponentModel.Description("�l���擾���͐ݒ肵�܂��B")]
			public T Value{
				get{return this.list[this.index];}
				set{this.list[this.index]=value;}
			}
		}
		private void propertyGrid1_PropertyValueChanged(object s,System.Windows.Forms.PropertyValueChangedEventArgs e) {
			this.UpdateSelectedString();
		}

		#region Designer Code
		private void InitializeComponent() {
			this.propertyGrid1=new System.Windows.Forms.PropertyGrid();
			this.splitter1=new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Dock=System.Windows.Forms.DockStyle.Left;
			this.panel1.Location=new System.Drawing.Point(3,3);
			this.panel1.Size=new System.Drawing.Size(33,312);
			// 
			// listBox1
			// 
			this.listBox1.Dock=System.Windows.Forms.DockStyle.Left;
			this.listBox1.Location=new System.Drawing.Point(36,3);
			this.listBox1.Size=new System.Drawing.Size(195,304);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location=new System.Drawing.Point(234,3);
			this.propertyGrid1.Name="propertyGrid1";
			this.propertyGrid1.Size=new System.Drawing.Size(274,312);
			this.propertyGrid1.TabIndex=0;
			this.propertyGrid1.PropertyValueChanged+=new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location=new System.Drawing.Point(231,3);
			this.splitter1.Name="splitter1";
			this.splitter1.Size=new System.Drawing.Size(3,312);
			this.splitter1.TabIndex=6;
			this.splitter1.TabStop=false;
			// 
			// CollectionEditor
			// 
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.splitter1);
			this.Name="CollectionEditor";
			this.Padding=new System.Windows.Forms.Padding(3);
			this.Controls.SetChildIndex(this.panel1,0);
			this.Controls.SetChildIndex(this.listBox1,0);
			this.Controls.SetChildIndex(this.splitter1,0);
			this.Controls.SetChildIndex(this.propertyGrid1,0);
			this.ResumeLayout(false);

		}
		#endregion
	}

#if design
	public class DummyBase:CollectionEditorBase<T>{
		public DummyBase(Gen::IList<T> items):base(items){}
		public DummyBase():base(){}
		protected override void SetToEditor(int index){}
		protected override T CreateNewInstance(){return 0;}
	}
#endif

}