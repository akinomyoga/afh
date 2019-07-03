#define normal
using Gen=System.Collections.Generic;
using CmM=System.ComponentModel;
namespace afh.Collections {
	/// <summary>
	/// System.Collections.Gen.ICollection&lt;T&gt; ��ҏW���� Form �ł��B
	/// </summary>
	/// <typeparam name="T">�W���̗v�f�̌^���w�肵�܂��B</typeparam>
	public abstract class CollectionEditorBase<T>:System.Windows.Forms.UserControl {
		/// <summary>
		/// �W���ɑ΂��鑀����s�� button ���ڂ��� panel ��ێ����܂��B
		/// </summary>
		protected System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSortUp;
		private System.Windows.Forms.Button btnSortDown;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAddNew;
		/// <summary>
		/// �W���̓��e��\������ׂ� ListBox ��ێ����܂��B
		/// </summary>
		protected System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// CollectionEditor �̃R���X�g���N�^�ł��B
		/// �w�肵�� <see cref="Gen::IList&lt;T&gt;"/> ���g�p���ď����������s���܂��B
		/// </summary>
		public CollectionEditorBase(Gen::IList<T> items):base(){
			this.InitializeComponent();
			this.InitializeComponent2();
			this.list=items;
		}
		/// <summary>
		/// CollectionEditor �̃R���X�g���N�^�ł��B
		/// </summary>
		public CollectionEditorBase():this(new Gen::List<T>()){}
		private void InitializeComponent2(){
			this.btnSortUp.Image=Drawing.Icons.SortUp;
			this.btnSortDown.Image=Drawing.Icons.SortDown;
			this.btnDelete.Image=Drawing.Icons.Delete;
			this.btnAddNew.Image=Drawing.Icons.AddNew;
		}

		/// <summary>
		/// �w�肵�����ڂ� PropertyGrid �ɐݒ肵�܂��B
		/// </summary>
		/// <param name="index">���ڂ̔ԍ����w�肵�܂��B</param>
		protected abstract void SetToEditor(int index);
		/// <summary>
		/// �ꗗ�ɒǉ�����ׁA<typeparamref name="T"/> �̐V�����C���X�^���X���쐬���܂��B
		/// </summary>
		/// <returns>�쐬���� <typeparamref name="T"/> �̃C���X�^���X��Ԃ��܂��B</returns>
		protected abstract T CreateNewInstance();
		//=================================================
		//		�\���̍X�V
		//=================================================
		private bool suppressUpdateView=false;
		/// <summary>
		/// ListBoxItem �̎w�肵�����ڂ̕\����ύX���܂��B
		/// �ύX�����s���Ă���ԁA�I�����ڂ̕ω��ɂ��\����}�����܂��B
		/// </summary>
		/// <param name="index">�ύX�Ώۂ̍��ڂ̔ԍ����w�肵�܂��B</param>
		/// <param name="x">�V�����\�����镶������w�肵�܂��B</param>
		protected void listBox1_setItem(int index,string x){
			this.suppressUpdateView=true;
			this.listBox1.Items[index]=x;
			this.suppressUpdateView=false;
		}
		/// <summary>
		///	�ҏW���� <typeparamref name="T"/> �C���X�^���X�ɕύX���������ꍇ�ȂǁA
		/// �\��������̍X�V���K�v�ȏꍇ�ɌĂяo���܂��B
		/// </summary>
		protected void UpdateSelectedString(){
			int index=this.listBox1.SelectedIndex;
			if(index<0)return;
			string x=this.GetShowString(this.list[index]);
			if((string)this.listBox1.Items[index]!=x)this.listBox1_setItem(index,x);
		}
		private void UpdateEnabled(){
			int index=this.listBox1.SelectedIndex;
			this.btnDelete.Enabled=0<=index;
			this.btnSortUp.Enabled=1<=index;
			this.btnSortDown.Enabled=0<=index&&index+1<this.listBox1.Items.Count;
		}
		//=================================================
		//		�\��������
		//=================================================
		/// <summary>
		/// ShowString ���g�p���Ďw�肵�����ڂɑΉ�����\����������擾���܂��B
		/// </summary>
		/// <param name="item">���ڂ��w�肵�܂��B</param>
		/// <returns>���ڂɑΉ�����\���������Ԃ��܂��B</returns>
		protected string GetShowString(T item){
			string r=this.desc(item);
			return r==""?"<anonymous>":r;
		}
		private Converter<T,string> desc=_ToString;
		/// <summary>
		/// �e���ڂɑ΂��ă��X�g�ɕ\�����镶������w�肷��֐����擾���͐ݒ肵�܂��B
		/// ����ł͒P�� ToString() �����s���镨���ݒ肳��Ă��܂��B
		/// <para>
		/// <see cref="M:CollectionEditor&lt;T&gt;.GetShowString"/> �͂����Ɏw��ł��܂���B
		/// �������[�v��h���ׂɐݒ肵�悤�Ƃ��Ă��������܂��B
		/// </para>
		/// </summary>
		[CmM.Browsable(false)]
		[CmM::DesignerSerializationVisibility(CmM::DesignerSerializationVisibility.Hidden)]
		public Converter<T,string> ShowString{
			get{
				return this.desc;
			}
			set{
				if(this.desc==(Converter<T,string>)GetShowString)return;
				this.desc=value??_ToString;
			}
		}
		private static string _ToString(T item){
			return item.ToString();
		}

		//=================================================
		//		���X�g�̃C���X�^���X
		//=================================================
		private Gen::IList<T> list;
		/// <summary>
		/// ���� Control ���g�p���ĕҏW����
		/// <see cref="System.Collections.Generic.ICollection&lt;T&gt;"/> ���w�肵�܂��B
		/// </summary>
		[CmM.Browsable(false)]
		[CmM::DesignerSerializationVisibility(CmM::DesignerSerializationVisibility.Hidden)]
		public Gen::IList<T> List{
			get{return this.list;}
			set{
				if(this.list==value)return;
				this.list=value??new Gen::List<T>();
				this.listBox1.Items.Clear();
				foreach(T item in this.list){
					this.listBox1.Items.Add(this.GetShowString(item));
				}
				if(this.listBox1.Items.Count>0){
					this.listBox1.SelectedIndex=0;
					this.SetToEditor(0);
				}
			}
		}

		//=================================================
		//		�e����
		//=================================================
		/// <summary>
		/// �V���� <typeparamref name="T"/> �̃C���X�^���X��ǉ����܂��B
		/// </summary>
		public void AddNew(){
			if(this.list==null)return;
			T item=this.CreateNewInstance();
			this.list.Add(item);
			this.listBox1.Items.Add(this.GetShowString(item));
			this.listBox1.SelectedIndex=this.listBox1.Items.Count-1;
		}
		/// <summary>
		/// �I������Ă��鍀�ڂ��폜���܂��B
		/// </summary>
		public void Delete(){
			this.Delete(this.listBox1.SelectedIndex);
			this.UpdateEnabled();
		}
		/// <summary>
		/// �w�肵���ꏊ�ɂ��鍀�ڂ��폜���܂��B
		/// </summary>
		/// <param name="index">�폜���鍀�ڂ� 0 ����n�܂�ԍ����w�肵�܂��B</param>
		public void Delete(int index){
			if(this.list==null||index<0||index>=this.list.Count)return;
			if(this.listBox1.SelectedIndex==index&&this.listBox1.Items.Count>1){
				if(index==this.listBox1.Items.Count-1){
					this.listBox1.SelectedIndex=index-1;
				}else{
					this.listBox1.SelectedIndex=index+1;
				}
			}
			this.list.RemoveAt(index);
			this.listBox1.Items.RemoveAt(index);
		}
		/// <summary>
		/// �I���������ڂ̏ꏊ�ƁA���̑O�Ɉʒu���鍀�ڂ̏ꏊ���������܂��B
		/// </summary>
		/// <returns>���������{�����ꍇ�� true ���A���{���Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool SortUp(){
			return this.SortUp(this.listBox1.SelectedIndex);
		}
		/// <summary>
		/// �w�肵�����ڂ̏ꏊ�ƁA���̑O�Ɉʒu���鍀�ڂ̏ꏊ���������܂��B
		/// </summary>
		/// <param name="index">�������鍀�ڂ̈ʒu���w�肵�܂��B</param>
		/// <returns>���������{�����ꍇ�� true ���A���{���Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool SortUp(int index){
			if(this.list==null||index<1||index>=this.list.Count)return false;

			// ����
			T item=this.list[index-1];
			this.list[index-1]=this.list[index];
			this.list[index]=item;
			string text=(string)this.listBox1.Items[index-1];
			this.listBox1.Items[index-1]=this.listBox1.Items[index];
			this.listBox1.Items[index]=text;

			// �I���ʒu�̈ړ�
			if(this.listBox1.SelectedIndex==index){
				this.listBox1.SelectedIndex--;
			}else if(this.listBox1.SelectedIndex==index-1){
				this.listBox1.SelectedIndex++;
			}

			return true;
		}
		/// <summary>
		/// �I���������ڂ̏ꏊ�ƁA���̎��Ɉʒu���鍀�ڂ̏ꏊ���������܂��B
		/// </summary>
		/// <returns>���������{�����ꍇ�� true ���A���{���Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool SortDown(){
			return this.SortUp(this.listBox1.SelectedIndex+1);
		}
		/// <summary>
		/// �w�肵�����ڂ̏ꏊ�ƁA���̎��Ɉʒu���鍀�ڂ̏ꏊ���������܂��B
		/// </summary>
		/// <param name="index">�������鍀�ڂ̈ʒu���w�肵�܂��B</param>
		/// <returns>���������{�����ꍇ�� true ���A���{���Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool SortDown(int index){
			return this.SortUp(index+1);
		}

		#region Designer
		private void InitializeComponent() {
			this.components=new System.ComponentModel.Container();
			this.listBox1=new System.Windows.Forms.ListBox();
			this.btnSortUp=new System.Windows.Forms.Button();
			this.btnSortDown=new System.Windows.Forms.Button();
			this.panel1=new System.Windows.Forms.Panel();
			this.btnAddNew=new System.Windows.Forms.Button();
			this.btnDelete=new System.Windows.Forms.Button();
			this.toolTip1=new System.Windows.Forms.ToolTip(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled=true;
			this.listBox1.ItemHeight=12;
			this.listBox1.Location=new System.Drawing.Point(128,19);
			this.listBox1.Name="listBox1";
			this.listBox1.Size=new System.Drawing.Size(115,184);
			this.listBox1.TabIndex=0;
			this.listBox1.SelectedIndexChanged+=new System.EventHandler(this.listBox1_SelectedIndexChanged);
			this.listBox1.KeyDown+=new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
			// 
			// btnSortUp
			// 
			this.btnSortUp.Enabled=false;
			this.btnSortUp.Location=new System.Drawing.Point(0,3);
			this.btnSortUp.Name="btnSortUp";
			this.btnSortUp.Size=new System.Drawing.Size(28,28);
			this.btnSortUp.TabIndex=3;
			this.toolTip1.SetToolTip(this.btnSortUp,"�I�����ڂ���Ɉړ�");
			this.btnSortUp.UseVisualStyleBackColor=true;
			this.btnSortUp.Click+=new System.EventHandler(this.btnSortUp_Click);
			// 
			// btnSortDown
			// 
			this.btnSortDown.Enabled=false;
			this.btnSortDown.Location=new System.Drawing.Point(0,37);
			this.btnSortDown.Name="btnSortDown";
			this.btnSortDown.Size=new System.Drawing.Size(28,28);
			this.btnSortDown.TabIndex=4;
			this.toolTip1.SetToolTip(this.btnSortDown,"�I�����ڂ����Ɉړ�");
			this.btnSortDown.UseVisualStyleBackColor=true;
			this.btnSortDown.Click+=new System.EventHandler(this.btnSortDown_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnAddNew);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnSortDown);
			this.panel1.Controls.Add(this.btnSortUp);
			this.panel1.Location=new System.Drawing.Point(22,19);
			this.panel1.Name="panel1";
			this.panel1.Size=new System.Drawing.Size(28,179);
			this.panel1.TabIndex=5;
			// 
			// btnAddNew
			// 
			this.btnAddNew.Location=new System.Drawing.Point(0,105);
			this.btnAddNew.Name="btnAddNew";
			this.btnAddNew.Size=new System.Drawing.Size(28,28);
			this.btnAddNew.TabIndex=6;
			this.toolTip1.SetToolTip(this.btnAddNew,"�V�������ڂ�ǉ�");
			this.btnAddNew.UseVisualStyleBackColor=true;
			this.btnAddNew.Click+=new System.EventHandler(this.btnAddNew_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled=false;
			this.btnDelete.Location=new System.Drawing.Point(0,71);
			this.btnDelete.Name="btnDelete";
			this.btnDelete.Size=new System.Drawing.Size(28,28);
			this.btnDelete.TabIndex=5;
			this.toolTip1.SetToolTip(this.btnDelete,"�I�����ڂ��폜");
			this.btnDelete.UseVisualStyleBackColor=true;
			this.btnDelete.Click+=new System.EventHandler(this.btnDelete_Click);
			// 
			// CollectionEditorBase
			// 
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.panel1);
			this.Name="CollectionEditorBase";
			this.Padding=new System.Windows.Forms.Padding(1);
			this.Size=new System.Drawing.Size(511,318);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region EventHandlers
		private void listBox1_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e){
			switch(e.KeyData){
				case System.Windows.Forms.Keys.Add:
				case System.Windows.Forms.Keys.Insert:
					this.AddNew();
					break;
				case System.Windows.Forms.Keys.Delete:
					this.Delete();
					break;
				case System.Windows.Forms.Keys.U:
					this.SortUp();
					break;
				case System.Windows.Forms.Keys.D:
					this.SortDown();
					break;
			}
		}

		private void listBox1_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(this.suppressUpdateView)return;
			this.UpdateEnabled();
			this.SetToEditor(this.listBox1.SelectedIndex);
		}

		private void btnSortUp_Click(object sender,System.EventArgs e) {
			this.SortUp();
		}

		private void btnSortDown_Click(object sender,System.EventArgs e) {
			this.SortDown();
		}

		private void btnDelete_Click(object sender,System.EventArgs e) {
			this.Delete();
		}

		private void btnAddNew_Click(object sender,System.EventArgs e) {
			this.AddNew();
		}
		#endregion
	}
}