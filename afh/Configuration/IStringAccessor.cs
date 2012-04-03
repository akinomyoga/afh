#if !NET1_1
using Emit=System.Reflection.Emit;
#endif

namespace afh.Configuration{
	/// <summary>
	/// 文字列を使用して値を読み書きします。
	/// また、値の変更もイベントによって通知します。
	/// 値の本来の型は文字列に限りません。
	/// </summary>
	public interface IStringAccessor{
		/// <summary>
		/// 文字列によって値を取得亦は設定します。
		/// </summary>
		string ValueString{get;set;}
		/// <summary>
		/// 本来の型によって値を取得亦は設定します。
		/// </summary>
		object Value{get;set;}
		/// <summary>
		/// 本来の型を取得します。
		/// </summary>
		System.Type ValueType{get;}
		/// <summary>
		/// 値に変更があった時に発生します。
		/// </summary>
		event afh.EventHandler<object> Changed;
	}
	/// <summary>
	/// IStringAccessor を公開する実装を持つ事を示します。
	/// </summary>
	public interface IStringAccessible{
		/// <summary>
		/// IStringAccessor を取得します。
		/// </summary>
		/// <returns>IStringAccessor を実装するオブジェクトを返します。</returns>
		IStringAccessor GetStringAccessor();
	}
	/// <summary>
	/// 特定のオブジェクトの特定のプロパティ対し、
	/// 値の読み書き及び変更の検出を行います。
	/// </summary>
	public class PropertyAccessor:IStringAccessor{
		private object obj;
		private System.Reflection.PropertyInfo p;
		private System.Reflection.EventInfo e;
		//=======================================================
		//		.ctor
		//=======================================================
		/// <summary>
		/// PropertyAccessor をプロパティの名前とイベントの名前を指定して初期化します。
		/// </summary>
		/// <param name="obj">対象のオブジェクトを指定します。</param>
		/// <param name="propertyName">オブジェクトの値として使用するプロパティを名前で指定します。
		/// null を指定した場合には eventName と共に無視します。</param>
		/// <param name="eventName">オブジェクトが変更された事を検出する為の event を名前で指定します。
		/// null を指定した場合には propertyName と共に無視します。</param>
		public PropertyAccessor(object obj,string propertyName,string eventName){
			System.Type t=obj.GetType();
			//--Property
			this.p=t.GetProperty(propertyName,new System.Type[]{});
			if(this.p==null)throw new System.NotImplementedException(t.FullName+" はプロパティ "+propertyName+" を実装しません。");
			//--Event
			this.e=t.GetEvent(eventName);
			if(this.e==null)throw new System.NotImplementedException(t.FullName+" はイベント "+eventName+" を実装しません。");
			this.HookEvent(this.obj=obj,this.e);
		}
		/// <summary>
		/// PropertyAccessor を PropertyInfo と EventInfo を使用して初期化します。
		/// </summary>
		/// <param name="obj">対象のオブジェクトを指定します。</param>
		/// <param name="p">オブジェクトの値として使用するプロパティを指定します。</param>
		/// <param name="e">オブジェクトが変更された事を検出する為の event を指定します。</param>
		public PropertyAccessor(object obj,System.Reflection.PropertyInfo p,System.Reflection.EventInfo e){
			this.p=p;
			this.HookEvent(this.obj=obj,this.e=e);
		}
		//=======================================================
		//		Event
		//=======================================================
		/// <summary>
		/// 値を文字列として取得亦は設定します。
		/// </summary>
		public string ValueString{
			get{return StringConvert.From(this.p.PropertyType,this.Value);}
			set{this.Value=StringConvert.To(this.p.PropertyType,value);}
		}
		/// <summary>
		/// 値を取得亦は設定します。
		/// </summary>
		public object Value{
			get{return this.p.GetValue(this.obj,null);}
			set{this.p.SetValue(this.obj,value,null);}
		}
		/// <summary>
		/// このクラスによって読み書きする値の型を取得します。
		/// </summary>
		public System.Type ValueType{get{return this.p.PropertyType;}}
		//=======================================================
		//		Event
		//=======================================================
		/// <summary>
		/// コントロールを介して設定の候補が変更された時に発生するイベントです。
		/// </summary>
		public event afh.EventHandler<object> Changed;
		/// <summary>
		/// Change イベントを発生させます。
		/// </summary>
		protected virtual void OnChanged(object sender){
			if(this.Changed==null)return;
			this.Changed(sender,this.Value);
		}


		#region [プログラム生成コード] イベントフック
		/// <summary>
		/// obj の e で指定したイベントに OnChanged を呼び出す関数をフックします。
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
		protected virtual void HookEvent(object obj,System.Reflection.EventInfo e){
			System.Type t=e.EventHandlerType;
			switch(t.GUID.GetHashCode()&0x7fffffff){
				case 0x1D95059F:
					if(t!=typeof(System.EventHandler))goto default;
					e.AddEventHandler(obj,new System.EventHandler(this.obj_Event));
					break;
				case 0x55BA541E:
					if(t!=typeof(System.Windows.Forms.ColumnClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ColumnClickEventHandler(this.obj_ColumnClick));
					break;
				case 0x4E74E8C7:
					if(t!=typeof(System.Windows.Forms.ContentsResizedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ContentsResizedEventHandler(this.obj_ContentsResized));
					break;
				case 0x2A44964C:
					if(t!=typeof(System.Windows.Forms.ControlEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ControlEventHandler(this.obj_Control));
					break;
				case 0x66D696A2:
					if(t!=typeof(System.Windows.Forms.ConvertEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ConvertEventHandler(this.obj_Convert));
					break;
				case 0x446EC337:
					if(t!=typeof(System.Windows.Forms.DateBoldEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateBoldEventHandler(this.obj_DateBold));
					break;
				case 0x38613C19:
					if(t!=typeof(System.Windows.Forms.DateRangeEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateRangeEventHandler(this.obj_DateRange));
					break;
				case 0x20A212A4:
					if(t!=typeof(System.Windows.Forms.DragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DragEventHandler(this.obj_Drag));
					break;
				case 0x4E11F670:
					if(t!=typeof(System.Windows.Forms.DrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DrawItemEventHandler(this.obj_DrawItem));
					break;
				case 0x10250BF6:
					if(t!=typeof(System.Windows.Forms.GiveFeedbackEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.GiveFeedbackEventHandler(this.obj_GiveFeedback));
					break;
				case 0x18225CB1:
					if(t!=typeof(System.Windows.Forms.HelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.HelpEventHandler(this.obj_Help));
					break;
				case 0x29F46229:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangedEventHandler(this.obj_InputLanguageChanged));
					break;
				case 0x42BF2A43:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangingEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangingEventHandler(this.obj_InputLanguageChanging));
					break;
				case 0x5DB1430A:
					if(t!=typeof(System.Windows.Forms.InvalidateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InvalidateEventHandler(this.obj_Invalidate));
					break;
				case 0x02E89CB8:
					if(t!=typeof(System.Windows.Forms.ItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemChangedEventHandler(this.obj_ItemChanged));
					break;
				case 0x0517F0C8:
					if(t!=typeof(System.Windows.Forms.ItemCheckEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemCheckEventHandler(this.obj_ItemCheck));
					break;
				case 0x1D3727D9:
					if(t!=typeof(System.Windows.Forms.ItemDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemDragEventHandler(this.obj_ItemDrag));
					break;
				case 0x53B8A498:
					if(t!=typeof(System.Windows.Forms.KeyEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyEventHandler(this.obj_Key));
					break;
				case 0x5C06F652:
					if(t!=typeof(System.Windows.Forms.KeyPressEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyPressEventHandler(this.obj_KeyPress));
					break;
				case 0x204BCE44:
					if(t!=typeof(System.Windows.Forms.LabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LabelEditEventHandler(this.obj_LabelEdit));
					break;
				case 0x71ACB009:
					if(t!=typeof(System.Windows.Forms.LayoutEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LayoutEventHandler(this.obj_Layout));
					break;
				case 0x6FAEA402:
					if(t!=typeof(System.Windows.Forms.LinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkClickedEventHandler(this.obj_LinkClicked));
					break;
				case 0x0396D3CE:
					if(t!=typeof(System.Windows.Forms.LinkLabelLinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.obj_LinkLabelLinkClicked));
					break;
				case 0x2797C235:
					if(t!=typeof(System.Windows.Forms.MeasureItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MeasureItemEventHandler(this.obj_MeasureItem));
					break;
				case 0x580DBFE2:
					if(t!=typeof(System.Windows.Forms.MouseEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MouseEventHandler(this.obj_Mouse));
					break;
				case 0x0F44AE91:
					if(t!=typeof(System.Windows.Forms.NavigateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NavigateEventHandler(this.obj_Navigate));
					break;
				case 0x6EDAE89F:
					if(t!=typeof(System.Windows.Forms.NodeLabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NodeLabelEditEventHandler(this.obj_NodeLabelEdit));
					break;
				case 0x6FEF62D4:
					if(t!=typeof(System.Windows.Forms.PaintEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PaintEventHandler(this.obj_Paint));
					break;
				case 0x7F431C0A:
					if(t!=typeof(System.Windows.Forms.PropertyTabChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyTabChangedEventHandler(this.obj_PropertyTabChanged));
					break;
				case 0x0FC32FB2:
					if(t!=typeof(System.Windows.Forms.PropertyValueChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyValueChangedEventHandler(this.obj_PropertyValueChanged));
					break;
				case 0x0CDDEA26:
					if(t!=typeof(System.Windows.Forms.QueryAccessibilityHelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryAccessibilityHelpEventHandler(this.obj_QueryAccessibilityHelp));
					break;
				case 0x44489E3B:
					if(t!=typeof(System.Windows.Forms.QueryContinueDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryContinueDragEventHandler(this.obj_QueryContinueDrag));
					break;
				case 0x4DF02610:
					if(t!=typeof(System.Windows.Forms.ScrollEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ScrollEventHandler(this.obj_Scroll));
					break;
				case 0x497C3C54:
					if(t!=typeof(System.Windows.Forms.SelectedGridItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.obj_SelectedGridItemChanged));
					break;
				case 0x64F16DF0:
					if(t!=typeof(System.Windows.Forms.SplitterEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SplitterEventHandler(this.obj_Splitter));
					break;
				case 0x7936C631:
					if(t!=typeof(System.Windows.Forms.StatusBarDrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarDrawItemEventHandler(this.obj_StatusBarDrawItem));
					break;
				case 0x3DE904A9:
					if(t!=typeof(System.Windows.Forms.StatusBarPanelClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarPanelClickEventHandler(this.obj_StatusBarPanelClick));
					break;
				case 0x0DF6A2B1:
					if(t!=typeof(System.Windows.Forms.ToolBarButtonClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ToolBarButtonClickEventHandler(this.obj_ToolBarButtonClick));
					break;
				case 0x4CC730F7:
					if(t!=typeof(System.Windows.Forms.TreeViewCancelEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewCancelEventHandler(this.obj_TreeViewCancel));
					break;
				case 0x6D127419:
					if(t!=typeof(System.Windows.Forms.TreeViewEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewEventHandler(this.obj_TreeView));
					break;
				case 0x1C7F90A6:
					if(t!=typeof(System.Windows.Forms.UICuesEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UICuesEventHandler(this.obj_UICues));
					break;
				case 0x6903E1C8:
					if(t!=typeof(System.Windows.Forms.UpDownEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UpDownEventHandler(this.obj_UpDown));
					break;
				default:
#if NET1_1
					throw System.ApplicationException("指定した種類のイベントには対応していません。");
#else
					e.AddEventHandler(obj,this.CreateEventHandler(e));
#endif
					break;
			}
		}
		private void obj_Event(System.Object sender,System.EventArgs e){this.OnChanged(sender);}
		private void obj_ColumnClick(System.Object sender,System.Windows.Forms.ColumnClickEventArgs e){this.OnChanged(sender);}
		private void obj_ContentsResized(System.Object sender,System.Windows.Forms.ContentsResizedEventArgs e){this.OnChanged(sender);}
		private void obj_Control(System.Object sender,System.Windows.Forms.ControlEventArgs e){this.OnChanged(sender);}
		private void obj_Convert(System.Object sender,System.Windows.Forms.ConvertEventArgs e){this.OnChanged(sender);}
		private void obj_DateBold(System.Object sender,System.Windows.Forms.DateBoldEventArgs e){this.OnChanged(sender);}
		private void obj_DateRange(System.Object sender,System.Windows.Forms.DateRangeEventArgs e){this.OnChanged(sender);}
		private void obj_Drag(System.Object sender,System.Windows.Forms.DragEventArgs e){this.OnChanged(sender);}
		private void obj_DrawItem(System.Object sender,System.Windows.Forms.DrawItemEventArgs e){this.OnChanged(sender);}
		private void obj_GiveFeedback(System.Object sender,System.Windows.Forms.GiveFeedbackEventArgs e){this.OnChanged(sender);}
		private void obj_Help(System.Object sender,System.Windows.Forms.HelpEventArgs hlpevent){this.OnChanged(sender);}
		private void obj_InputLanguageChanged(System.Object sender,System.Windows.Forms.InputLanguageChangedEventArgs e){this.OnChanged(sender);}
		private void obj_InputLanguageChanging(System.Object sender,System.Windows.Forms.InputLanguageChangingEventArgs e){this.OnChanged(sender);}
		private void obj_Invalidate(System.Object sender,System.Windows.Forms.InvalidateEventArgs e){this.OnChanged(sender);}
		private void obj_ItemChanged(System.Object sender,System.Windows.Forms.ItemChangedEventArgs e){this.OnChanged(sender);}
		private void obj_ItemCheck(System.Object sender,System.Windows.Forms.ItemCheckEventArgs e){this.OnChanged(sender);}
		private void obj_ItemDrag(System.Object sender,System.Windows.Forms.ItemDragEventArgs e){this.OnChanged(sender);}
		private void obj_Key(System.Object sender,System.Windows.Forms.KeyEventArgs e){this.OnChanged(sender);}
		private void obj_KeyPress(System.Object sender,System.Windows.Forms.KeyPressEventArgs e){this.OnChanged(sender);}
		private void obj_LabelEdit(System.Object sender,System.Windows.Forms.LabelEditEventArgs e){this.OnChanged(sender);}
		private void obj_Layout(System.Object sender,System.Windows.Forms.LayoutEventArgs e){this.OnChanged(sender);}
		private void obj_LinkClicked(System.Object sender,System.Windows.Forms.LinkClickedEventArgs e){this.OnChanged(sender);}
		private void obj_LinkLabelLinkClicked(System.Object sender,System.Windows.Forms.LinkLabelLinkClickedEventArgs e){this.OnChanged(sender);}
		private void obj_MeasureItem(System.Object sender,System.Windows.Forms.MeasureItemEventArgs e){this.OnChanged(sender);}
		private void obj_Mouse(System.Object sender,System.Windows.Forms.MouseEventArgs e){this.OnChanged(sender);}
		private void obj_Navigate(System.Object sender,System.Windows.Forms.NavigateEventArgs ne){this.OnChanged(sender);}
		private void obj_NodeLabelEdit(System.Object sender,System.Windows.Forms.NodeLabelEditEventArgs e){this.OnChanged(sender);}
		private void obj_Paint(System.Object sender,System.Windows.Forms.PaintEventArgs e){this.OnChanged(sender);}
		private void obj_PropertyTabChanged(System.Object s,System.Windows.Forms.PropertyTabChangedEventArgs e){this.OnChanged(s);}
		private void obj_PropertyValueChanged(System.Object s,System.Windows.Forms.PropertyValueChangedEventArgs e){this.OnChanged(s);}
		private void obj_QueryAccessibilityHelp(System.Object sender,System.Windows.Forms.QueryAccessibilityHelpEventArgs e){this.OnChanged(sender);}
		private void obj_QueryContinueDrag(System.Object sender,System.Windows.Forms.QueryContinueDragEventArgs e){this.OnChanged(sender);}
		private void obj_Scroll(System.Object sender,System.Windows.Forms.ScrollEventArgs e){this.OnChanged(sender);}
		private void obj_SelectedGridItemChanged(System.Object sender,System.Windows.Forms.SelectedGridItemChangedEventArgs e){this.OnChanged(sender);}
		private void obj_Splitter(System.Object sender,System.Windows.Forms.SplitterEventArgs e){this.OnChanged(sender);}
		private void obj_StatusBarDrawItem(System.Object sender,System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent){this.OnChanged(sender);}
		private void obj_StatusBarPanelClick(System.Object sender,System.Windows.Forms.StatusBarPanelClickEventArgs e){this.OnChanged(sender);}
		private void obj_ToolBarButtonClick(System.Object sender,System.Windows.Forms.ToolBarButtonClickEventArgs e){this.OnChanged(sender);}
		private void obj_TreeViewCancel(System.Object sender,System.Windows.Forms.TreeViewCancelEventArgs e){this.OnChanged(sender);}
		private void obj_TreeView(System.Object sender,System.Windows.Forms.TreeViewEventArgs e){this.OnChanged(sender);}
		private void obj_UICues(System.Object sender,System.Windows.Forms.UICuesEventArgs e){this.OnChanged(sender);}
		private void obj_UpDown(System.Object source,System.Windows.Forms.UpDownEventArgs e){this.OnChanged(source);}
		#endregion

#if !NET1_1
		/// <summary>
		/// 指定したイベントに追加出来るようなイベントハンドラを作成し、<see cref="System.Delegate"/> として返します。
		/// イベントハンドラ内では、 this.OnChanged(sender) を呼び出します。
		/// </summary>
		/// <param name="e">追加先のイベントの情報を指定します。</param>
		/// <returns>作成したメソッドを参照するデリゲートを返します。</returns>
		protected System.Delegate CreateEventHandler(System.Reflection.EventInfo e){
			System.Reflection.MethodInfo minfo=e.EventHandlerType.GetMethod("Invoke");
			if(minfo.ReturnType!=typeof(void))
				throw new System.ApplicationException(@"このイベントには返値を指定しなければ為りません。
現在、返値を必要とするイベントへのフックには対応していません。");

			//-- 引数の型
			System.Reflection.ParameterInfo[] infoParams=minfo.GetParameters();
			System.Type[] tParams=new System.Type[infoParams.Length];
			tParams[0]=typeof(PropertyAccessor); // this-parameter
			for(int i=0;i<infoParams.Length;i++)tParams[i+1]=infoParams[i].ParameterType;

			Emit.DynamicMethod eh=new Emit.DynamicMethod("eh",null,tParams,typeof(PropertyAccessor));
			
			System.Reflection.Emit.ILGenerator ilgen=eh.GetILGenerator();
			ilgen.Emit(Emit.OpCodes.Ldarg_0); // load this
			ilgen.Emit(infoParams.Length==0?Emit.OpCodes.Ldnull:Emit.OpCodes.Ldarg_1); // load sender (引数がない場合には null)
			ilgen.Emit(Emit.OpCodes.Callvirt,typeof(PropertyAccessor).GetMethod("OnChanged")); // this.OnChnaged(sender);
			ilgen.Emit(Emit.OpCodes.Ret); // return;
			
			return eh.CreateDelegate(e.EventHandlerType,this);
		}
#endif

	}
}
