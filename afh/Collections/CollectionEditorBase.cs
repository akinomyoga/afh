#define normal
using Gen=System.Collections.Generic;
using CmM=System.ComponentModel;
namespace afh.Collections {
	/// <summary>
	/// System.Collections.Gen.ICollection&lt;T&gt; を編集する Form です。
	/// </summary>
	/// <typeparam name="T">集合の要素の型を指定します。</typeparam>
	public abstract class CollectionEditorBase<T>:System.Windows.Forms.UserControl {
		/// <summary>
		/// 集合に対する操作を行う button を載せた panel を保持します。
		/// </summary>
		protected System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSortUp;
		private System.Windows.Forms.Button btnSortDown;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAddNew;
		/// <summary>
		/// 集合の内容を表示する為の ListBox を保持します。
		/// </summary>
		protected System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// CollectionEditor のコンストラクタです。
		/// 指定した <see cref="Gen::IList&lt;T&gt;"/> を使用して初期化を実行します。
		/// </summary>
		public CollectionEditorBase(Gen::IList<T> items):base(){
			this.InitializeComponent();
			this.InitializeComponent2();
			this.list=items;
		}
		/// <summary>
		/// CollectionEditor のコンストラクタです。
		/// </summary>
		public CollectionEditorBase():this(new Gen::List<T>()){}
		private void InitializeComponent2(){
			this.btnSortUp.Image=Drawing.Icons.SortUp;
			this.btnSortDown.Image=Drawing.Icons.SortDown;
			this.btnDelete.Image=Drawing.Icons.Delete;
			this.btnAddNew.Image=Drawing.Icons.AddNew;
		}

		/// <summary>
		/// 指定した項目を PropertyGrid に設定します。
		/// </summary>
		/// <param name="index">項目の番号を指定します。</param>
		protected abstract void SetToEditor(int index);
		/// <summary>
		/// 一覧に追加する為、<typeparamref name="T"/> の新しいインスタンスを作成します。
		/// </summary>
		/// <returns>作成した <typeparamref name="T"/> のインスタンスを返します。</returns>
		protected abstract T CreateNewInstance();
		//=================================================
		//		表示の更新
		//=================================================
		private bool suppressUpdateView=false;
		/// <summary>
		/// ListBoxItem の指定した項目の表示を変更します。
		/// 変更を実行している間、選択項目の変化による表示を抑制します。
		/// </summary>
		/// <param name="index">変更対象の項目の番号を指定します。</param>
		/// <param name="x">新しく表示する文字列を指定します。</param>
		protected void listBox1_setItem(int index,string x){
			this.suppressUpdateView=true;
			this.listBox1.Items[index]=x;
			this.suppressUpdateView=false;
		}
		/// <summary>
		///	編集中の <typeparamref name="T"/> インスタンスに変更があった場合など、
		/// 表示文字列の更新が必要な場合に呼び出します。
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
		//		表示文字列
		//=================================================
		/// <summary>
		/// ShowString を使用して指定した項目に対応する表示文字列を取得します。
		/// </summary>
		/// <param name="item">項目を指定します。</param>
		/// <returns>項目に対応する表示文字列を返します。</returns>
		protected string GetShowString(T item){
			string r=this.desc(item);
			return r==""?"<anonymous>":r;
		}
		private Converter<T,string> desc=_ToString;
		/// <summary>
		/// 各項目に対してリストに表示する文字列を指定する関数を取得亦は設定します。
		/// 既定では単に ToString() を実行する物が設定されています。
		/// <para>
		/// <see cref="M:CollectionEditor&lt;T&gt;.GetShowString"/> はここに指定できません。
		/// 無限ループを防ぐ為に設定しようとしても無視します。
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
		//		リストのインスタンス
		//=================================================
		private Gen::IList<T> list;
		/// <summary>
		/// この Control を使用して編集する
		/// <see cref="System.Collections.Generic.ICollection&lt;T&gt;"/> を指定します。
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
		//		各操作
		//=================================================
		/// <summary>
		/// 新しい <typeparamref name="T"/> のインスタンスを追加します。
		/// </summary>
		public void AddNew(){
			if(this.list==null)return;
			T item=this.CreateNewInstance();
			this.list.Add(item);
			this.listBox1.Items.Add(this.GetShowString(item));
			this.listBox1.SelectedIndex=this.listBox1.Items.Count-1;
		}
		/// <summary>
		/// 選択されている項目を削除します。
		/// </summary>
		public void Delete(){
			this.Delete(this.listBox1.SelectedIndex);
			this.UpdateEnabled();
		}
		/// <summary>
		/// 指定した場所にある項目を削除します。
		/// </summary>
		/// <param name="index">削除する項目の 0 から始まる番号を指定します。</param>
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
		/// 選択した項目の場所と、その前に位置する項目の場所を交換します。
		/// </summary>
		/// <returns>交換を実施した場合に true を、実施しなかった場合に false を返します。</returns>
		public bool SortUp(){
			return this.SortUp(this.listBox1.SelectedIndex);
		}
		/// <summary>
		/// 指定した項目の場所と、その前に位置する項目の場所を交換します。
		/// </summary>
		/// <param name="index">交換する項目の位置を指定します。</param>
		/// <returns>交換を実施した場合に true を、実施しなかった場合に false を返します。</returns>
		public bool SortUp(int index){
			if(this.list==null||index<1||index>=this.list.Count)return false;

			// 交換
			T item=this.list[index-1];
			this.list[index-1]=this.list[index];
			this.list[index]=item;
			string text=(string)this.listBox1.Items[index-1];
			this.listBox1.Items[index-1]=this.listBox1.Items[index];
			this.listBox1.Items[index]=text;

			// 選択位置の移動
			if(this.listBox1.SelectedIndex==index){
				this.listBox1.SelectedIndex--;
			}else if(this.listBox1.SelectedIndex==index-1){
				this.listBox1.SelectedIndex++;
			}

			return true;
		}
		/// <summary>
		/// 選択した項目の場所と、その次に位置する項目の場所を交換します。
		/// </summary>
		/// <returns>交換を実施した場合に true を、実施しなかった場合に false を返します。</returns>
		public bool SortDown(){
			return this.SortUp(this.listBox1.SelectedIndex+1);
		}
		/// <summary>
		/// 指定した項目の場所と、その次に位置する項目の場所を交換します。
		/// </summary>
		/// <param name="index">交換する項目の位置を指定します。</param>
		/// <returns>交換を実施した場合に true を、実施しなかった場合に false を返します。</returns>
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
			this.toolTip1.SetToolTip(this.btnSortUp,"選択項目を上に移動");
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
			this.toolTip1.SetToolTip(this.btnSortDown,"選択項目を下に移動");
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
			this.toolTip1.SetToolTip(this.btnAddNew,"新しい項目を追加");
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
			this.toolTip1.SetToolTip(this.btnDelete,"選択項目を削除");
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