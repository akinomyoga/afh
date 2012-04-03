namespace afh.Configuration{
	/// <summary>
	/// 一時的な設定の変更 (つまりその場で有効になるわけではない設定の変更)
	/// を管理する為の操作を提供します。
	/// </summary>
	/// <remarks>
	/// 茲の説明では、現在有効になっている設定の事を「有効設定」と呼び、
	/// 編集途中(変更中)であって未だ適用されていない設定の事を「仮設定」と呼ぶ事にします。
	/// </remarks>
	public interface ISettingTemporary{
		/// <summary>
		/// 仮設定が有効設定と異なるかどうか(編集されたかどうか)を取得します。
		/// </summary>
		bool IsChanged{get;}
		/// <summary>
		/// 仮設定を有効設定に反映します。
		/// </summary>
		void Apply();
		/// <summary>
		/// 仮設定を現在の有効設定に戻します。
		/// </summary>
		void Restore();
		/// <summary>
		/// 設定の既定値を仮設定に反映します。
		/// (有効にするにはこの後に Apply を呼び出します。)
		/// </summary>
		void RestoreDefaults();
		/// <summary>
		/// 仮設定に変更があった時に発生するイベントです。
		/// </summary>
		event afh.EventHandler<object> Changed;
		/// <summary>
		/// 仮設定が適用された時に発生します。
		/// </summary>
		event afh.VoidEH Applied;
	}
	//CHECK: 子コンテナそれぞれにも SettingKey を割り当てる様な仕組みにする

	#region Attribute:SettingContainer
	/// <summary>
	/// SettingControl を保持するコンテナの属性です。
	/// System.Windows.Forms.ContainerControl を継承するクラスに設定し、
	/// 表示前に Initialize を実行すれば実際に設定を行う事が出来る様になります。
	/// <para>
	/// SettingContainerAttribute を持つクラスのフィールドにも設定する事が出来ます。
	/// その場合は、そのフィールドの型であるクラスにも SettingContainerControl が設定されている必要があります。
	/// <code>
	/// [SettingContainer]
	/// class Container1:System.Windows.Forms.ContainerControl{
	///		[SettingContainer]
	///		Container2 field1;
	/// }
	/// 
	/// class Container2:System.Windows.Forms.ContainerControl{}
	/// </code>
	/// </para>
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Field)]
	public class SettingContainerAttribute:System.Attribute,ISettingTemporary{
		/// <summary>
		/// 管理する ISettingTemporary の集合を保持します。
		/// </summary>
		protected System.Collections.ArrayList list;
		/// <summary>
		/// 設定を保存する先である SettingKey を保持します。
		/// </summary>
		protected internal SettingKey k;
		/// <summary>
		/// 設定を保存する先である SettingKey のパスを保持します。
		/// 有効なのは初期化する迄です。
		/// </summary>
		protected internal string keypath;
		//===========================================================
		//		private methods
		//===========================================================
		private void Add(ISettingTemporary temp){
			temp.Changed+=new afh.EventHandler<object>(attr_Changed);
			this.list.Add(temp);
		}
		private void attr_Changed(object sender,object value){this.OnChanged(sender,value);}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// SettingContainerAttribute のインスタンスを作成します。
		/// </summary>
		/// <param name="keyPath">
		/// 設定の保存先のパスを指定します。
		/// クラスに設定した場合には、Root からの相対パスになります。
		/// フィールドに設定した場合には、そのフィールドを持つインスタンスに対応する保存先である SettingKey からの相対パスになります。
		/// </param>
		public SettingContainerAttribute(string keyPath){
			this.k=null;
			this.keypath=keyPath;
		}
		/// <summary>
		/// SettingContainerAttribute のインスタンスを作成します。
		/// クラスに設定した場合には Root\afh.Configuration.SettingContainer 以下にクラス名のキーを作成してそこに設定を保存します。
		/// フィールドに設定した場合には、そのフィールドの型に宣言されている SettingContainerAttribute の指示する場所に保存します。
		/// </summary>
		public SettingContainerAttribute(){
			this.k=null;
			this.keypath=null;
		}
		/// <summary>
		/// SettingContainerAttribute のインスタンスを作成します。
		/// </summary>
		/// <param name="key">設定の保存先である SettingKey を指定します。</param>
		public SettingContainerAttribute(SettingKey key){
			this.k=key;
			this.keypath=null;
		}
		//===========================================================
		//		ISettingTemporary
		//===========================================================
		/// <summary>
		/// 適用されていない設定の変更があるかどうかを取得します。
		/// </summary>
		public bool IsChanged{
			get{
				foreach(ISettingTemporary ctrl in this.list)if(ctrl.IsChanged)return true;
				return false;
			}
		}
		/// <summary>
		/// 適用されていない設定の変更を適用します。
		/// </summary>
		public void Apply(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.Apply();
			this.OnApplied();
		}
		/// <summary>
		/// 仮設定を現在有効になっている設定に戻します。
		/// </summary>
		public void Restore(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.Restore();
		}
		/// <summary>
		/// 仮設定を既定値に戻します。
		/// </summary>
		public void RestoreDefaults(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.RestoreDefaults();
		}
		/// <summary>
		/// 仮設定が変更された時に発生します。
		/// </summary>
		public event afh.EventHandler<object> Changed;
		/// <summary>
		/// Changed イベントを発生させます。
		/// </summary>
		/// <param name="sender">イベントの発生元のオブジェクトを指定します</param>
		/// <param name="value">変更後の値を指定します。</param>
		protected virtual void OnChanged(object sender,object value){
			if(this.Changed==null)return;
			this.Changed(sender,value);
		}
		/// <summary>
		/// 仮設定が適用された時に発生します。
		/// </summary>
		public event afh.VoidEH Applied;
		/// <summary>
		/// Applied イベントを発生させます。
		/// </summary>
		protected virtual void OnApplied(){
			if(this.Applied==null)return;
			this.Applied(this);
		}
		//===========================================================
		//		コンテナの初期化
		//===========================================================
		/// <summary>
		/// 二回以上 Initialize を行おうとした時に発生するエラーの説明を表します。
		/// </summary>
		protected const string ERROR_DOUBLE_INIT="二回目以降の Initialize は受け付けません";
		/// <summary>
		/// Initialize が実行済であるか否かを表します。
		/// </summary>
		protected bool initialized=false;
		/// <summary>
		/// コンテナオブジェクトをそのフィールドの属性
		/// (SettingButtonAttribute, SettingControlAttribute, SettingContainerAttribute など)
		/// に従って初期化します。
		/// (InitializeComponent の後で実行する様にして下さい。)
		/// </summary>
		/// <param name="obj">初期化する対象のコンテナ</param>
		protected internal virtual void Initialize(System.Windows.Forms.ContainerControl obj){
			if(this.initialized){Application.Log.AfhOut.WriteError(ERROR_DOUBLE_INIT);return;}
			this.initialized=true;
			this.list=new System.Collections.ArrayList();

			System.Type t=obj.GetType();
			this.Initialize_k(t);
			this.Initialize_fields(obj,t);
		}
		/// <summary>
		/// 未だ SettingKey k が設定されていない時に、k を初期化します。
		/// </summary>
		/// <param name="t">この属性の適用先であるコンテナの厳密な型を指定します。</param>
		protected void Initialize_k(System.Type t){
			if(this.k!=null)return;
			if(this.keypath==null){
				this.k=Setting.Root["afh.Configuration.SettingContainer"]["T",t.FullName];
			}else{
				this.k=Setting.Root.GetKey(this.keypath);
			}
		}
		/// <summary>
		/// フィールドを初期化します。
		/// </summary>
		/// <param name="obj">初期化の対象となるコンテナコントロールを指定します。</param>
		/// <param name="t">コンテナコントロールの型を指定します。</param>
		protected void Initialize_fields(System.Windows.Forms.ContainerControl obj,System.Type t){
			System.Reflection.FieldInfo[] fs=t.GetFields(BF_FIELD);
			for(int i=0,iM=fs.Length;i<iM;i++){
				object fval=null;
				//--SettingControlAttribute
				SettingControlAttribute[] ctrls=(SettingControlAttribute[])
					fs[i].GetCustomAttributes(typeof(SettingControlAttribute),false);
				for(int j=0,jM=ctrls.Length;j<jM;j++){
					ctrls[j].SetKeyAndControl(this.k,fs[i].GetValue(obj));
					this.Add(ctrls[j]);
				}
				//--SettingButtonAttribute
				object[] attrs=fs[i].GetCustomAttributes(typeof(SettingButtonAttribute),false);
				if(attrs.Length>0){
					SettingButtonAttribute button=(SettingButtonAttribute)attrs[0];
					fval=fs[i].GetValue(obj);
					if(fval is System.Windows.Forms.Button)
						button.Initialize(this,(System.Windows.Forms.Button)fval);
				}
				//--SettingContainerAttribute
				attrs=fs[i].GetCustomAttributes(typeof(SettingContainerAttribute),false);
				if(attrs.Length>0){
					SettingContainerAttribute container=(SettingContainerAttribute)attrs[0];
					//--value of field
					if(fval==null)fval=fs[i].GetValue(obj);
					if(!(fval is System.Windows.Forms.ContainerControl)){
						Application.Log.AfhOut.WriteError("指定したフィールドの型は System.Windows.Forms.ContainerControl を継承しません。");
						Application.Log.AfhOut.AddIndent();
						Application.Log.AfhOut.WriteVar("Type",t.FullName);
						Application.Log.AfhOut.WriteVar("Field",fs[i].Name);
						Application.Log.AfhOut.WriteVar("FieldType",fs[i].FieldType.FullName);
						Application.Log.AfhOut.RemoveIndent();
						continue;
					}
					//--attrs
					attrs=fval.GetType().GetCustomAttributes(typeof(SettingContainerAttribute),false);
					if(attrs.Length==0){
						Application.Log.AfhOut.WriteError("指定したフィールドの型には SettingContainerAttribute が設定されていません。");
						Application.Log.AfhOut.AddIndent();
						Application.Log.AfhOut.WriteVar("Type",t.FullName);
						Application.Log.AfhOut.WriteVar("Field",fs[i].Name);
						Application.Log.AfhOut.WriteVar("FieldType",fs[i].FieldType.FullName);
						Application.Log.AfhOut.RemoveIndent();
						continue;
					}
					//--ContainerAttribute
					if(container.k==null){
						if(container.keypath==null){
							container=(SettingContainerAttribute)attrs[0];
						}else{
							container.k=this.k.GetKey(container.keypath);
						}
					}
					container.Initialize((System.Windows.Forms.ContainerControl)fval);
					this.Add(container);
				}
			}
		}
		//===========================================================
		//		static
		//===========================================================
		private const string NOT_HAVE_ATTRIBUTE="指定したオブジェクトには SettingContainerAttribute 属性が設定されていません。";
		private const System.Reflection.BindingFlags BF_FIELD
			=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
		/// <summary>
		/// 指定したオブジェクトを設定を行う為のフォームとして初期化します。
		/// </summary>
		/// <param name="obj">初期化するフォーム亦はコンテナ</param>
		public static void InitializeContainer(System.Windows.Forms.ContainerControl obj){
			System.Type t=obj.GetType();
			//--Attribute の取得
			SettingContainerAttribute[] attrs=(SettingContainerAttribute[])
				t.GetCustomAttributes(typeof(SettingContainerAttribute),false);
			if(attrs.Length==0)
				throw new System.ArgumentException(NOT_HAVE_ATTRIBUTE,"obj");
			attrs[0].Initialize(obj);
		}
	}
	#endregion

	#region Attribute:SettingButton
	/// <summary>
	/// SettingContainerAttribute によって初期化されたコンテナに於いて、
	/// そのメンバのボタンがクリックされた時の動作を指定します。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class SettingButtonAttribute:System.Attribute{
		private SettingButtonAttribute.Operation ope;
		private ISettingTemporary parent;
		/// <summary>
		/// SettingButtonAttribute のインスタンスを作成します。
		/// </summary>
		/// <param name="operation">ボタンがクリックされた時の動作を指定します。</param>
		public SettingButtonAttribute(SettingButtonAttribute.Operation operation){
			this.ope=operation;
		}
		private void Exec(){
			if(this.parent==null)return;
			switch(this.ope){
				case Operation.OK:
				case Operation.Apply:
					this.parent.Apply();
					break;
				case Operation.Cancel:
				case Operation.Restore:
					this.parent.Restore();
					break;
				case Operation.RestoreDefaults:
					this.parent.RestoreDefaults();
					break;
			}
		}
		//===========================================================
		//		フック
		//===========================================================
		private System.Windows.Forms.Button b;
		/// <summary>
		/// 指定したオブジェクトのイベントにフックします。
		/// TODO: Button 以外 (event Click, bool Enabled{set;} を別途指定可能に)
		/// </summary>
		/// <param name="parent">このボタンによって操作を受ける対象である ISettingTemporary を指定します。</param>
		/// <param name="obj">このインスタンスを取得したフィールドの値を指定します。</param>
		public void Initialize(ISettingTemporary parent,System.Windows.Forms.Button obj){
			if(this.parent!=null)return;
			if(parent==null||obj==null)return;
			this.parent=parent;
			this.b=obj;
			if((0x10&(int)this.ope)!=0){
				this.b.Enabled=false;
				this.parent.Changed+=new afh.EventHandler<object>(parent_Changed);
				this.parent.Applied+=new VoidEH(parent_Applied);
			}
			obj.Click+=new System.EventHandler(obj_Click);
		}
		private void obj_Click(object sender,System.EventArgs e){
			this.Exec();
		}
		private void parent_Changed(object sender, object value){this.b.Enabled=this.parent.IsChanged;}
		private void parent_Applied(object sender){this.b.Enabled=this.parent.IsChanged;}


		#region /*[プログラム生成コード] イベントフック*/
		/*
		// .NET Framework 2.0 では DynamicMethod で実行時に関数を作成できる
		/// <summary>
		/// /// obj の e で指定したイベントに OnChanged を呼び出す関数をフックします。
		/// <list type="table">
		/// <listheader>対応している EventHandler デリゲートの一覧</listheader>
		/// <item><see cref="System.EventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ColumnClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ContentsResizedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ControlEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ConvertEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DateBoldEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DateRangeEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DrawItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.GiveFeedbackEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.HelpEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InputLanguageChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InputLanguageChangingEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InvalidateEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemCheckEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemDragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.KeyEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.KeyPressEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LabelEditEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LayoutEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LinkClickedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LinkLabelLinkClickedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.MeasureItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.MouseEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.NavigateEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.NodeLabelEditEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PaintEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PropertyTabChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PropertyValueChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.QueryAccessibilityHelpEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.QueryContinueDragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ScrollEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.SelectedGridItemChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.SplitterEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.StatusBarDrawItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.StatusBarPanelClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ToolBarButtonClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.TreeViewCancelEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.TreeViewEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.UICuesEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.UpDownEventHandler"/></item>
		/// </list>
		/// </summary>
		/// protected virtual void HookEvent(object obj,System.Reflection.EventInfo e){
			System.Type t=e.EventHandlerType;
			switch(t.GetHashCode()){
				case 0xE3753C:
					if(t!=typeof(System.EventHandler))goto default;
					e.AddEventHandler(obj,new System.EventHandler(this.obj_Event));
					break;
				case 0x45EBBFC:
					if(t!=typeof(System.Windows.Forms.ColumnClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ColumnClickEventHandler(this.obj_ColumnClick));
					break;
				case 0x45EBC30:
					if(t!=typeof(System.Windows.Forms.ContentsResizedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ContentsResizedEventHandler(this.obj_ContentsResized));
					break;
				case 0x45EBC64:
					if(t!=typeof(System.Windows.Forms.ControlEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ControlEventHandler(this.obj_Control));
					break;
				case 0x45EBC98:
					if(t!=typeof(System.Windows.Forms.ConvertEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ConvertEventHandler(this.obj_Convert));
					break;
				case 0x45EBCCC:
					if(t!=typeof(System.Windows.Forms.DateBoldEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateBoldEventHandler(this.obj_DateBold));
					break;
				case 0x45EBD00:
					if(t!=typeof(System.Windows.Forms.DateRangeEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateRangeEventHandler(this.obj_DateRange));
					break;
				case 0x45EBD34:
					if(t!=typeof(System.Windows.Forms.DragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DragEventHandler(this.obj_Drag));
					break;
				case 0x45EBD68:
					if(t!=typeof(System.Windows.Forms.DrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DrawItemEventHandler(this.obj_DrawItem));
					break;
				case 0x45EBD9C:
					if(t!=typeof(System.Windows.Forms.GiveFeedbackEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.GiveFeedbackEventHandler(this.obj_GiveFeedback));
					break;
				case 0x45EBDD0:
					if(t!=typeof(System.Windows.Forms.HelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.HelpEventHandler(this.obj_Help));
					break;
				case 0x45EBE04:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangedEventHandler(this.obj_InputLanguageChanged));
					break;
				case 0x45EBE38:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangingEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangingEventHandler(this.obj_InputLanguageChanging));
					break;
				case 0x45EBE6C:
					if(t!=typeof(System.Windows.Forms.InvalidateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InvalidateEventHandler(this.obj_Invalidate));
					break;
				case 0x45EBEA0:
					if(t!=typeof(System.Windows.Forms.ItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemChangedEventHandler(this.obj_ItemChanged));
					break;
				case 0x45EBED4:
					if(t!=typeof(System.Windows.Forms.ItemCheckEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemCheckEventHandler(this.obj_ItemCheck));
					break;
				case 0x45EBF08:
					if(t!=typeof(System.Windows.Forms.ItemDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemDragEventHandler(this.obj_ItemDrag));
					break;
				case 0x45EBF3C:
					if(t!=typeof(System.Windows.Forms.KeyEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyEventHandler(this.obj_Key));
					break;
				case 0x45EBF70:
					if(t!=typeof(System.Windows.Forms.KeyPressEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyPressEventHandler(this.obj_KeyPress));
					break;
				case 0x45EBFA4:
					if(t!=typeof(System.Windows.Forms.LabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LabelEditEventHandler(this.obj_LabelEdit));
					break;
				case 0x45EBFD8:
					if(t!=typeof(System.Windows.Forms.LayoutEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LayoutEventHandler(this.obj_Layout));
					break;
				case 0x45EC00C:
					if(t!=typeof(System.Windows.Forms.LinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkClickedEventHandler(this.obj_LinkClicked));
					break;
				case 0x45EC040:
					if(t!=typeof(System.Windows.Forms.LinkLabelLinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.obj_LinkLabelLinkClicked));
					break;
				case 0x45EC074:
					if(t!=typeof(System.Windows.Forms.MeasureItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MeasureItemEventHandler(this.obj_MeasureItem));
					break;
				case 0x45EC0A8:
					if(t!=typeof(System.Windows.Forms.MouseEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MouseEventHandler(this.obj_Mouse));
					break;
				case 0x45EC0DC:
					if(t!=typeof(System.Windows.Forms.NavigateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NavigateEventHandler(this.obj_Navigate));
					break;
				case 0x45EC110:
					if(t!=typeof(System.Windows.Forms.NodeLabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NodeLabelEditEventHandler(this.obj_NodeLabelEdit));
					break;
				case 0x45EC144:
					if(t!=typeof(System.Windows.Forms.PaintEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PaintEventHandler(this.obj_Paint));
					break;
				case 0x45EC178:
					if(t!=typeof(System.Windows.Forms.PropertyTabChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyTabChangedEventHandler(this.obj_PropertyTabChanged));
					break;
				case 0x45EC1AC:
					if(t!=typeof(System.Windows.Forms.PropertyValueChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyValueChangedEventHandler(this.obj_PropertyValueChanged));
					break;
				case 0x45EC1E0:
					if(t!=typeof(System.Windows.Forms.QueryAccessibilityHelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryAccessibilityHelpEventHandler(this.obj_QueryAccessibilityHelp));
					break;
				case 0x45EC214:
					if(t!=typeof(System.Windows.Forms.QueryContinueDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryContinueDragEventHandler(this.obj_QueryContinueDrag));
					break;
				case 0x45EC248:
					if(t!=typeof(System.Windows.Forms.ScrollEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ScrollEventHandler(this.obj_Scroll));
					break;
				case 0x45EC27C:
					if(t!=typeof(System.Windows.Forms.SelectedGridItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.obj_SelectedGridItemChanged));
					break;
				case 0x45EC2B0:
					if(t!=typeof(System.Windows.Forms.SplitterEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SplitterEventHandler(this.obj_Splitter));
					break;
				case 0x45EC2E4:
					if(t!=typeof(System.Windows.Forms.StatusBarDrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarDrawItemEventHandler(this.obj_StatusBarDrawItem));
					break;
				case 0x45EC318:
					if(t!=typeof(System.Windows.Forms.StatusBarPanelClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarPanelClickEventHandler(this.obj_StatusBarPanelClick));
					break;
				case 0x45EC34C:
					if(t!=typeof(System.Windows.Forms.ToolBarButtonClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ToolBarButtonClickEventHandler(this.obj_ToolBarButtonClick));
					break;
				case 0x45EC380:
					if(t!=typeof(System.Windows.Forms.TreeViewCancelEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewCancelEventHandler(this.obj_TreeViewCancel));
					break;
				case 0x45EC3B4:
					if(t!=typeof(System.Windows.Forms.TreeViewEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewEventHandler(this.obj_TreeView));
					break;
				case 0x45EC3E8:
					if(t!=typeof(System.Windows.Forms.UICuesEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UICuesEventHandler(this.obj_UICues));
					break;
				case 0x45EC41C:
					if(t!=typeof(System.Windows.Forms.UpDownEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UpDownEventHandler(this.obj_UpDown));
					break;
		default:
					throw new System.Exception("指定したイベントの種類には対応していません");
			}
		}
		private void obj_Event(System.Object sender,System.EventArgs e){this.Exec();}
		private void obj_ColumnClick(System.Object sender,System.Windows.Forms.ColumnClickEventArgs e){this.Exec();}
		private void obj_ContentsResized(System.Object sender,System.Windows.Forms.ContentsResizedEventArgs e){this.Exec();}
		private void obj_Control(System.Object sender,System.Windows.Forms.ControlEventArgs e){this.Exec();}
		private void obj_Convert(System.Object sender,System.Windows.Forms.ConvertEventArgs e){this.Exec();}
		private void obj_DateBold(System.Object sender,System.Windows.Forms.DateBoldEventArgs e){this.Exec();}
		private void obj_DateRange(System.Object sender,System.Windows.Forms.DateRangeEventArgs e){this.Exec();}
		private void obj_Drag(System.Object sender,System.Windows.Forms.DragEventArgs e){this.Exec();}
		private void obj_DrawItem(System.Object sender,System.Windows.Forms.DrawItemEventArgs e){this.Exec();}
		private void obj_GiveFeedback(System.Object sender,System.Windows.Forms.GiveFeedbackEventArgs e){this.Exec();}
		private void obj_Help(System.Object sender,System.Windows.Forms.HelpEventArgs hlpevent){this.Exec();}
		private void obj_InputLanguageChanged(System.Object sender,System.Windows.Forms.InputLanguageChangedEventArgs e){this.Exec();}
		private void obj_InputLanguageChanging(System.Object sender,System.Windows.Forms.InputLanguageChangingEventArgs e){this.Exec();}
		private void obj_Invalidate(System.Object sender,System.Windows.Forms.InvalidateEventArgs e){this.Exec();}
		private void obj_ItemChanged(System.Object sender,System.Windows.Forms.ItemChangedEventArgs e){this.Exec();}
		private void obj_ItemCheck(System.Object sender,System.Windows.Forms.ItemCheckEventArgs e){this.Exec();}
		private void obj_ItemDrag(System.Object sender,System.Windows.Forms.ItemDragEventArgs e){this.Exec();}
		private void obj_Key(System.Object sender,System.Windows.Forms.KeyEventArgs e){this.Exec();}
		private void obj_KeyPress(System.Object sender,System.Windows.Forms.KeyPressEventArgs e){this.Exec();}
		private void obj_LabelEdit(System.Object sender,System.Windows.Forms.LabelEditEventArgs e){this.Exec();}
		private void obj_Layout(System.Object sender,System.Windows.Forms.LayoutEventArgs e){this.Exec();}
		private void obj_LinkClicked(System.Object sender,System.Windows.Forms.LinkClickedEventArgs e){this.Exec();}
		private void obj_LinkLabelLinkClicked(System.Object sender,System.Windows.Forms.LinkLabelLinkClickedEventArgs e){this.Exec();}
		private void obj_MeasureItem(System.Object sender,System.Windows.Forms.MeasureItemEventArgs e){this.Exec();}
		private void obj_Mouse(System.Object sender,System.Windows.Forms.MouseEventArgs e){this.Exec();}
		private void obj_Navigate(System.Object sender,System.Windows.Forms.NavigateEventArgs ne){this.Exec();}
		private void obj_NodeLabelEdit(System.Object sender,System.Windows.Forms.NodeLabelEditEventArgs e){this.Exec();}
		private void obj_Paint(System.Object sender,System.Windows.Forms.PaintEventArgs e){this.Exec();}
		private void obj_PropertyTabChanged(System.Object s,System.Windows.Forms.PropertyTabChangedEventArgs e){this.Exec();}
		private void obj_PropertyValueChanged(System.Object s,System.Windows.Forms.PropertyValueChangedEventArgs e){this.Exec();}
		private void obj_QueryAccessibilityHelp(System.Object sender,System.Windows.Forms.QueryAccessibilityHelpEventArgs e){this.Exec();}
		private void obj_QueryContinueDrag(System.Object sender,System.Windows.Forms.QueryContinueDragEventArgs e){this.Exec();}
		private void obj_Scroll(System.Object sender,System.Windows.Forms.ScrollEventArgs e){this.Exec();}
		private void obj_SelectedGridItemChanged(System.Object sender,System.Windows.Forms.SelectedGridItemChangedEventArgs e){this.Exec();}
		private void obj_Splitter(System.Object sender,System.Windows.Forms.SplitterEventArgs e){this.Exec();}
		private void obj_StatusBarDrawItem(System.Object sender,System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent){this.Exec();}
		private void obj_StatusBarPanelClick(System.Object sender,System.Windows.Forms.StatusBarPanelClickEventArgs e){this.Exec();}
		private void obj_ToolBarButtonClick(System.Object sender,System.Windows.Forms.ToolBarButtonClickEventArgs e){this.Exec();}
		private void obj_TreeViewCancel(System.Object sender,System.Windows.Forms.TreeViewCancelEventArgs e){this.Exec();}
		private void obj_TreeView(System.Object sender,System.Windows.Forms.TreeViewEventArgs e){this.Exec();}
		private void obj_UICues(System.Object sender,System.Windows.Forms.UICuesEventArgs e){this.Exec();}
		private void obj_UpDown(System.Object source,System.Windows.Forms.UpDownEventArgs e){this.Exec();}
		//*/
		#endregion

		//*/
		/// <summary>
		/// ボタンが押された時の動作を指定します。
		/// </summary>
		public enum Operation{
			/// <summary>
			/// 仮設定を適用し有効にします。
			/// [OK] ボタンに指定される事が想定されています。
			/// </summary>
			OK=0x00,
			/// <summary>
			/// 仮設定を適用し有効にします。変更がない場合には無効表示にします。
			/// [適用] ボタンに指定される事が想定されています。
			/// </summary>
			Apply=0x10,
			/// <summary>
			/// 仮設定を無効にし、現在の設定に戻します。
			/// [キャンセル] ボタンに指定される事が想定されています。
			/// </summary>
			Cancel=0x01,
			/// <summary>
			/// 仮設定を無効にし、現在の設定に戻します。変更がない場合には無効表示にします。
			/// [現在の設定] ボタンに指定される事が想定されています。
			/// </summary>
			Restore=0x11,
			/// <summary>
			/// 仮設定として既定値を設定します。
			/// [既定値] ボタンに指定される事が想定されています。
			/// </summary>
			RestoreDefaults=0x03
		}
	}
	#endregion

	#region Attribute:SettingControl
	/// <summary>
	/// 個々の設定を行うコントロールにつける属性のクラスです。
	/// SettingFormAttribute と併せて使用してください。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field,AllowMultiple=true)]
	public class SettingControlAttribute:System.Attribute,ISettingTemporary{
		//===========================================================
		//		初期フィールド
		//===========================================================
		/// <summary>
		/// 設定の値の SettingKey 上の名前
		/// </summary>
		private string varName;
		/// <summary>
		/// 既定値。設定を行わない状態での値を表します。
		/// </summary>
		private object vDefault;
		/// <summary>
		/// コントロールの値として使用するプロパティの名前を保持します。
		/// </summary>
		private string propertyName;
		/// <summary>
		/// propertyName で指定した値の変更を検出する為に使用するイベントの名前を保持します。
		/// </summary>
		private string eventName;
		/// <summary>
		/// 既定値 (設定を行わない状態での値) を取得亦は設定します。
		/// </summary>
		public object DefaultValue{
			get{return this.vDefault;}
			set{this.vDefault=value;}
		}
		//===========================================================
		//		コントロール初期化後のフィールド
		//===========================================================
		/// <summary>
		/// 設定の保存先の SettingKey
		/// </summary>
		private SettingKey key;
		/// <summary>
		/// 対象のオブジェクトへの参照を表します。
		/// </summary>
		private IStringAccessor accessor;
		//===========================================================
		//		Properties
		//===========================================================
		/// <summary>
		/// 変更を加える前の値、つまり、現在有効になっている値を取得します。
		/// </summary>
		protected object ValBefore{
			get{
				if(this.key.Var.HasVariable(this.varName)){
					string value=this.key.Var[this.varName];
					return StringConvert.To(this.accessor.ValueType,value);
				}else return this.vDefault;
			}
		}
		/// <summary>
		/// コントロールによって変更を加えた後の値を取得します。
		/// </summary>
		protected object ValAfter{
			get{return this.accessor.Value;}
		}
		/// <summary>
		/// 変更を加える前の値、つまり、現在有効になっている値を文字列として取得します。
		/// </summary>
		protected string StrBefore{
			get{
				if(this.key.Var.HasVariable(this.varName)){
					return this.key.Var[this.varName];
				}else return StringConvert.From(this.vDefault);
			}
		}
		/// <summary>
		/// コントロールによって変更を加えた後の値を文字列として取得します。
		/// </summary>
		protected string StrAfter{get{return this.strafter;}}
		private string strafter;
		//===========================================================
		//		ISettingTemporary
		//===========================================================
		/// <summary>
		/// コントロールによって変更がなされたかどうかを取得します。
		/// つまり、現在有効になっている設定とコントロールによって設定された値が異なるかどうかを表します。
		/// </summary>
		public bool IsChanged{get{return this.StrAfter!=this.StrBefore;}}
		/// <summary>
		/// コントロールの状態を現在有効な設定に対応した物にします。
		/// </summary>
		public void Restore(){this.accessor.ValueString=this.StrBefore;}
		/// <summary>
		/// コントロールの表示する設定を既定値にします。
		/// </summary>
		public void RestoreDefaults(){this.accessor.Value=this.vDefault;}
		/// <summary>
		/// コントロールで設定した物を有効にします。
		/// </summary>
		public void Apply(){
			if(!this.IsChanged)return;
			this.key.Var[this.varName]=this.strafter;
			this.OnApplied();
		}
		/// <summary>
		/// コントロールを介して設定の候補が変更された時に発生するイベントです。
		/// </summary>
		public event afh.EventHandler<object> Changed;
		/// <summary>
		/// Change イベントを発生させます。
		/// </summary>
		protected virtual void OnChanged(object sender,object value){
			this.strafter=StringConvert.From(this.accessor.ValueType,value);
			if(this.Changed==null)return;
			this.Changed(sender,value);
		}
		/// <summary>
		/// 仮設定が適用された時に発生します。
		/// </summary>
		public event afh.VoidEH Applied;
		/// <summary>
		/// Applied イベントを発生させます。
		/// </summary>
		protected virtual void OnApplied(){
			if(this.Applied==null)return;
			this.Applied(this);
		}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// 一つ一つの設定を行うコントロールに関連づけられる
		/// SettingControlAttribute のコンストラクタ。
		/// TODO: カスタムコントロールのクラスに属性を付ける。
		///     状態←→文字列 の変換を提供しそれを利用する。
		/// </summary>
		/// <param name="varName">設定を保存する先の変数名を指定します。</param>
		/// <param name="propertyName">コントロールの値として使用するプロパティの名前を指定します。</param>
		/// <param name="eventName">propertyName で指定した値の変更を検出する為に使用するイベントの名前を保持します。</param>
		public SettingControlAttribute(string varName,string propertyName,string eventName){
			this.varName=varName;
			this.propertyName=propertyName;
			this.eventName=eventName;
		}
		/// <summary>
		/// 一つ一つの設定を行うコントロールに関連づけられる
		/// SettingControlAttribute のコンストラクタ。
		/// </summary>
		/// <param name="varName">設定を保存する先の変数名を指定します。</param>
		public SettingControlAttribute(string varName):this(varName,null,null){}
		//===========================================================
		//		InitializeControl
		//===========================================================
		private const string ERROR_TYPE_DEFAULT="指定されている既定値の型が誤っています。対応する型か、それに変換可能な string で指定して下さい。";
		private const string ERROR_CONVERT_DEFAULT="文字列で指定された既定値を適当な型に変換できません。変換可能な文字列で指定するか、その型で直接指定して下さい。";
		/// <summary>
		/// 設定を保存する SettingKey と設定に使用するコントロールを設定します。
		/// 一度目の指定のみ有効です。二回目以降の指定は無視されます。
		/// </summary>
		/// <param name="k">設定の保存先を指定します。</param>
		/// <param name="ctrl">設定に使用するコントロール亦はその他のオブジェクトを指定します。</param>
		internal void SetKeyAndControl(SettingKey k,object ctrl){
			//--引数検査
			if(this.key!=null)return;
			if(k==null)throw new System.ArgumentNullException("k","null は指定できません");
			if(ctrl==null)throw new System.ArgumentNullException("ctrl","null は指定できません");
			//--代入
			this.key=k;
			this.accessor=SettingControlAttribute.CreateAccessor(ctrl,this.propertyName,this.eventName);
			if(this.accessor==null){
				this.key=null;
				throw new System.ArgumentException("指定したコントロール亦はオブジェクトには対応していません","ctrl");
			}
			//--vDefault の型合わせ
			if(this.vDefault==null){
				this.vDefault=this.accessor.Value;
			}else if(this.vDefault.GetType()!=this.accessor.ValueType){
				if(this.vDefault.GetType()!=typeof(string))throw new System.Exception(ERROR_TYPE_DEFAULT);
				try{this.vDefault=StringConvert.To(this.accessor.ValueType,(string)this.vDefault);}
				catch(System.Exception e){throw new System.InvalidCastException(ERROR_CONVERT_DEFAULT,e);}
			}
			//--設定
			if(this.key.Var.HasVariable(this.varName)){
				this.accessor.ValueString=this.key.Var[this.varName];
			}else{
				this.accessor.Value=this.vDefault;
				this.key.Var[this.varName]=StringConvert.From(this.accessor.ValueType,this.vDefault);
			}
			this.strafter=this.accessor.ValueString;
			this.accessor.Changed+=new afh.EventHandler<object>(this.OnChanged);
		}
		//===========================================================
		//		static
		//===========================================================
		static SettingControlAttribute(){
			PropEventHolder.Register(typeof(System.Windows.Forms.CheckBox),"Checked","CheckedChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.ComboBox),"SelectedIndex","SelectedIndexChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.DateTimePicker),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.DomainUpDown),"SelectedIndex","SelectedItemChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.HScrollBar),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.MonthCalendar),"SelectionRange","DateChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.NumericUpDown),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.RadioButton),"Checked","CheckedChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.TextBox),"Text","TextChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.TrackBar),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.VScrollBar),"Value","ValueChanged");
			//TODO>DONE: 以下の物は PropertyAccessor を使用して書込を行っても Changed イベントが発生しない
			// MenuItemAccessor で IStringAccessor を実装した
			// PropEventHolder.Register(typeof(System.Windows.Forms.MenuItem),"Checked","Click");
		}

		#region Convert.IStringAccessor GetAccessor
		/// <summary>
		/// 対象のオブジェクトを特徴付ける状態に対して文字列で値を取得・設定する手段を提供します。
		/// </summary>
		/// <param name="obj">対象のオブジェクトを指定します。</param>
		/// <param name="propertyName">指定したオブジェクトの値として使用するプロパティの名前を指定します。
		/// null を設定した場合には既定のプロパティとイベントの組み合わせを検索します。</param>
		/// <param name="eventName">propertyName で指定した値の変更を検出する為に使用できるイベントの名前を指定します。
		/// null を設定した場合には既定のプロパティとイベントの組み合わせを検索します。</param>
		/// <returns>Convert.IStringAccessor を実装するインスタンスを返します。
		/// 対応する IStringAccessor を作成できない場合には null を返します。</returns>
		public static IStringAccessor CreateAccessor(object obj,string propertyName,string eventName){
			//--プロパティ・イベントの指定がある時の場合
			if(propertyName!=null&&eventName!=null)return new PropertyAccessor(obj,propertyName,eventName);
			//--ProertyAccessor の作成情報が登録されている場合
			System.Type t=obj.GetType();
			IStringAccessor ret=PropEventHolder.CreateAccessor(obj,t);
			if(ret!=null)return ret;
			//--その他
			switch(t.GUID.GetHashCode()&0x7fffffff){
				case 0x55B66084:
					if(t!=typeof(System.Windows.Forms.MenuItem))goto default;
					return new SettingControlAttribute.MenuItemAccessor((System.Windows.Forms.MenuItem)obj);
				case 0x5DCD9E6E:
					if(t!=typeof(System.Windows.Forms.ToolBarButton))goto default;
					return new SettingControlAttribute.ToolBarButtonAccessor((System.Windows.Forms.ToolBarButton)obj);
				case 0x497A7A3F:
					if(t!=typeof(System.Windows.Forms.CheckedListBox))goto default;
					return new SettingControlAttribute.CheckedListBoxAccessor((System.Windows.Forms.CheckedListBox)obj);
			//	case 0x10371C67:
			//		if(t!=typeof(System.Windows.Forms.MonthCalendar))goto default;
			//		return new SettingControlAttribute.MonthCalendarAccessor((System.Windows.Forms.MonthCalendar)obj);
				default:
					break;
			}
			return null;
		}
		/// <summary>
		/// PropertyInfo と EventInfo を保持し、
		/// 与えられたオブジェクトの PropertyAccessor インスタンスを作成します。
		/// </summary>
		private class PropEventHolder{
			private System.Reflection.PropertyInfo p;
			private System.Reflection.EventInfo e;
			public System.Reflection.PropertyInfo Property{get{return this.p;}}
			public System.Reflection.EventInfo Event{get{return this.e;}}
			internal PropEventHolder(System.Type type,string propertyName,string eventName){
				this.p=type.GetProperty(propertyName);
				this.e=type.GetEvent(eventName);
			}
			internal PropEventHolder(System.Type type,System.Reflection.PropertyInfo prop,System.Reflection.EventInfo ev){
				this.p=prop;
				this.e=ev;
			}
			private static readonly System.Collections.Hashtable hash=new System.Collections.Hashtable(10);
			/// <summary>
			/// 指定した type の既定の property 及び event を
			/// PropEventHolder として登録します。
			/// </summary>
			/// <param name="type">property 及び event を持つ型を指定します。</param>
			/// <param name="propertyName">property の名前を指定します。</param>
			/// <param name="eventName">event の名前を指定します。</param>
			public static void Register(System.Type type,string propertyName,string eventName){
				PropEventHolder.hash.Add(type,new PropEventHolder(type,propertyName,eventName));
			}
			/// <summary>
			/// Register された情報を元にして PropertyAccessor を作成して返します。
			/// </summary>
			/// <param name="obj">Access したい値を持つ object を指定します。</param>
			/// <param name="type">obj の型を指定します。GetType 実行コストを削減する為の引数です。
			/// 他の型を指定しないように注意してください。</param>
			/// <returns>作成した PropertyAccessor を返します。</returns>
			internal static IStringAccessor CreateAccessor(object obj,System.Type type){
				if(!PropEventHolder.hash.ContainsKey(type))return null;
				PropEventHolder peh=(PropEventHolder)PropEventHolder.hash[type];
				return new PropertyAccessor(obj,peh.p,peh.e);
			}
			/// <summary>
			/// Register された情報を元にして PropertyAccessor を作成して返します。
			/// </summary>
			/// <param name="obj">Access したい値を持つ object を指定します。</param>
			/// <returns>作成した PropertyAccessor を返します。</returns>
			public static IStringAccessor CreateAccessor(object obj){
				return PropEventHolder.CreateAccessor(obj,obj.GetType());
			}
		}
		/// <summary>
		/// CheckedListBox の状態を bool[] で把握し、string によるアクセスを提供します。 
		/// </summary>
		private class CheckedListBoxAccessor:IStringAccessor{
			System.Windows.Forms.CheckedListBox box;
			string str;
			bool[] arr;
			public CheckedListBoxAccessor(System.Windows.Forms.CheckedListBox box){
				this.box=box;
				//--this.arr
				int i=box.Items.Count;
				this.arr=new bool[i];
				while(0<i--)this.arr[i]=false;
				for(i=0;i<box.SelectedIndices.Count;i++)this.arr[i]=true;
				//--this.str
				this.str=StringConvert.From(this.arr);

				this.box.ItemCheck+=new System.Windows.Forms.ItemCheckEventHandler(box_ItemCheck);
			}
			private void box_ItemCheck(object sender,System.Windows.Forms.ItemCheckEventArgs e){
				//--変化がない時を除く
				if(e.NewValue==System.Windows.Forms.CheckState.Indeterminate)return;
				int i=e.Index;
				if(this.arr[i]^(e.NewValue==System.Windows.Forms.CheckState.Unchecked))return;
				//--値の変更
				bool val=this.arr[i]=e.NewValue==System.Windows.Forms.CheckState.Checked;
				this.str=this.str.Remove(i,1).Insert(i,val?"1":"0");
				this.OnChanged();
			}
			public string ValueString{
				get{return this.str;}
				set{
					//--value 調整
					int iM=this.box.Items.Count;
					if(value.Length>iM)value=value.Substring(0,iM);
					else while(value.Length<iM)value+="0";
					//--設定
					this.str=value;
					this.arr=StringConvert.ToBooleanArray(value);
					for(int i=0;i<iM;i++)this.box.SetItemChecked(i,this.arr[i]);
				}
			}
			public object Value{
				get{return this.arr;}
				set{
					if(value is bool[]){
						int iM=this.box.Items.Count;
						bool[] val0=(bool[])value;
						int im=val0.Length;
						if(im==iM){
							this.arr=val0;
						}else if(im<iM){
							this.arr=new bool[iM];
							System.Array.Copy(val0,this.arr,im);
							do{this.arr[im]=false;}while(++im<iM);
						}else{
							this.arr=new bool[iM];
							System.Array.Copy(val0,this.arr,iM);
						}
						this.str=StringConvert.FromBooleanArray(this.arr);
						for(int i=0;i<iM;i++)this.box.SetItemChecked(i,this.arr[i]);
					}else throw new System.ArgumentException("bool[] を代入して下さい","value");
				}
			}
			public System.Type ValueType{get{return typeof(bool[]);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.box,this.Value);
			}
		}
		/// <summary>
		/// MenuItem のチェック状態に対して string によるアクセスを提供します。 
		/// ※プログラムから MenuItem.Checked を触ると不整合が起きます
		/// </summary>
		private class MenuItemAccessor:IStringAccessor{
			private System.Windows.Forms.MenuItem item;
			public MenuItemAccessor(System.Windows.Forms.MenuItem item){
				this.item=item;
				this.item.Click+=new System.EventHandler(item_Click);
			}
			private void item_Click(object sender, System.EventArgs e){
				this.item.Checked=!this.item.Checked;
				this.OnChanged();
			}
			public string ValueString{get{return this.item.Checked.ToString();}
				set{
					bool val=StringConvert.ToBoolean(value);
					if(val==this.item.Checked)return;
					this.item.Checked=val;
					this.OnChanged();
				}
			}
			public object Value{get{return this.item.Checked;}
				set{
					if(!(value is bool))
						throw new System.ArgumentException("設定する値の型は bool でなければなりません。","value");
					bool val=(bool)value;
					if(val==this.item.Checked)return;
					this.item.Checked=val;
					this.OnChanged();
				}
			}
			public System.Type ValueType{get{return typeof(bool);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.item,this.Value);
			}
		}
		/// <summary>
		/// ToolBarButton の押下状態に対して string によるアクセスを提供します。 
		/// ※プログラムから ToolBarButton.Pushed を触ると不整合が起きます
		/// </summary>
		private class ToolBarButtonAccessor:IStringAccessor{
			private System.Windows.Forms.ToolBarButton button;
			private bool current;
			public ToolBarButtonAccessor(System.Windows.Forms.ToolBarButton button){
				this.button=button;
				this.current=this.button.Pushed;
				this.button.Parent.ButtonClick+=new System.Windows.Forms.ToolBarButtonClickEventHandler(Parent_ButtonClick);
			}
			private void Parent_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e){
				if(e.Button!=this.button||this.button.Pushed==this.current)return;
				this.current=this.button.Pushed=true;
			}
			public string ValueString{get{return this.current.ToString();}
				set{
					bool val=StringConvert.ToBoolean(value);
					if(val==this.current)return;
					this.button.Pushed=this.current=val;
					this.OnChanged();
				}
			}
			public object Value{get{return this.current;}
				set{
					if(!(value is bool))
						throw new System.ArgumentException("設定する値の型は bool でなければなりません。","value");
					bool val=(bool)value;
					if(this.current==val)return;
					this.button.Pushed=this.current=val;
					this.OnChanged();
				}
			}
			public System.Type ValueType{get{return typeof(bool);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.button,this.Value);
			}
		}
#if old
		/// <summary>
		/// MonthCalendar の選択期間に対して string によるアクセスを提供します。 
		/// </summary>
		private class MonthCalendarAccessor:Convert.IStringAccessor{
			private System.Windows.Forms.MonthCalendar cal;
			public MonthCalendarAccessor(System.Windows.Forms.MonthCalendar calendar){
				this.cal=calendar;
				this.cal.DateChanged+=new System.Windows.Forms.DateRangeEventHandler(cal_DateChanged);
			}
			private void cal_DateChanged(object sender, System.Windows.Forms.DateRangeEventArgs e){
				this.OnChanged();
			}
			public string ValueString{
				get{return Convert.Convert.ToString(typeof(System.Windows.Forms.SelectionRange),this.cal.SelectionRange);}
				set{
					this.cal.SelectionRange=(System.Windows.Forms.SelectionRange)
						Convert.Convert.FromString(typeof(System.Windows.Forms.SelectionRange),value);
				}
			}
			public object Value{
				get{return this.cal.SelectionRange;}
				set{
					if(!(value is System.Windows.Forms.SelectionRange))
						throw new System.ArgumentException("設定する値の型は System.Windows.Forms.SelectionRange でなければなりません。","value");
					this.cal.SelectionRange=(System.Windows.Forms.SelectionRange)value;
				}
			}
			public System.Type ValueType{get{return typeof(System.Windows.Forms.SelectionRange);}}
			public event afh.ObjectEH Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.cal,this.Value);
			}

		}
#endif
		#endregion
	}
	#endregion

}