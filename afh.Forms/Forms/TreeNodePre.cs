using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using CM=System.ComponentModel;

namespace afh.Forms{
	//===================================================================
	//		Inheritable Properties Definitions
	//===================================================================
	//#→template DeclareInheritProperty<T,PROPNAME,DEFAULT,ONCHANGED>
	public partial class TreeNode{
		//#define PropInherit	PROPNAME##Inherit
		//#define InheritBits	s##PROPNAME##Inherit
		//#define IS_BOOL		__afh::equal(T,bool)
		//#define PropBits		b##PROPNAME

		/// <summary>
		/// DESCRIPTION(x)の決定方法を取得又は設定します。
		/// </summary>
		[ATTR_FOR_INHERITANCE]
		[CM::Description("DESCRIPTION(x)の決定方法を取得又は設定します。")]
		public TreeNodeInheritType PropInherit{
			get{return (TreeNodeInheritType)this.bits[InheritBits];}
			set{this.bits[InheritBits]=(uint)value;}
		}
		/// <summary>
		/// DESCRIPTION(x)を取得又は設定します。
		/// </summary>
		[ATTR_DEFAULTVALUE]
		[CM::Description("DESCRIPTION(x)を取得又は設定します。")]
		public T PROPNAME{
			get{
				switch(this.PropInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.PROPNAME;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.PROPNAME;
						break;
					case TreeNodeInheritType.Custom:
#if IS_BOOL
						return this.bits[PropBits];
#else
						T ret;
						if(this.xmem.GetMember("PROPNAME",out ret))
							return ret;
						break;
#endif
				}
				return DEFAULT;
			}
			set {
//#define NO_ONCHANGED	__afh::equal(ONCHANGED)
#if !NO_ONCHANGED
				if(this.PropInherit==TreeNodeInheritType.Custom){
#	if IS_BOOL
					if(this.bits[PropBits]==value)return;
#	else
					T val;
					if(this.xmem.GetMember("PROPNAME",out val)&&val==value)return;
#	endif
				}else{
					this.PropInherit=TreeNodeInheritType.Custom;
				}
#else
				this.PropInherit=TreeNodeInheritType.Custom;
#endif
#if IS_BOOL
				this.bits[PropBits]=value;
#else
				this.xmem["PROPNAME"]=value;
#endif
				ONCHANGED;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define MemberName m_##PROPNAME
		//#define PROPNAMEChanged PROPNAME##Changed
		//#define OnPROPNAMEChanged On##PROPNAME##Changed
		private T MemberName=DEFAULT;
		/// <summary>
		/// DESCRIPTION(x)を取得又は設定します。
		/// </summary>
		[ATTR_DEFAULTVALUE]
		[CM::Description("DESCRIPTION(x)を取得又は設定します。")]
		public T PROPNAME{
			get{return this.MemberName;}
			set{
				if(this.MemberName==value)return;
				this.MemberName=value;
				this.OnPROPNAMEChanged();
			}
		}
		/// <summary>
		/// PROPNAME の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("PROPNAME プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB PROPNAMEChanged;
		private void OnPROPNAMEChanged(){
			if(this.PROPNAMEChanged==null)return;
			this.PROPNAMEChanged();
		}
	}
	//#←template
	//===================================================================
	//		Inheritable Properties Declarations
	//===================================================================
	// ※ sPROPNAMEInherit は自分で定義して下さい。

	// property: NodeHeight
	//#define DESCRIPTION(VOID) このノード自体の高さ
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(14)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<int,NodeHeight,14,"#this.RefreshDisplaySize()#">

	// property: ChildIndentWidth [廃止]
	//-#define DESCRIPTION(VOID) 子ノードのインデント幅
	//-#DeclareInheritProperty<int,ChildIndentWidth,18, >

	// property: IsEnabled
	//#define DESCRIPTION(VOID) ノードが有効であるか否か
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(true)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Inherit)
	//#DeclareInheritProperty<bool,IsEnabled,true,"#this.OnIsEnabledChanged(true,new TreeNodePropertyChangingEventArgs<bool>(!value,value))#">

	// property: Icon
	//#define DESCRIPTION(VOID) TreeNode に表示するアイコン
	//#define ATTR_DEFAULTVALUE		TreeNodeIcon.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeIcon,Icon,TreeNodeIcon.File, >

	// property: IconSize
	//#define DESCRIPTION(VOID) アイコンを表示する時の大きさ
	//#define DefaultIconSize		new Gdi::Size(12,12)
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Size),"12,12")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Size,IconSize,DefaultIconSize, >

	// property: IconVisible
	//#define DESCRIPTION(VOID) アイコンを表示するか否か
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(false)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<bool,IconVisible,false, >

	// property: CheckBox
	//#define DESCRIPTION(VOID) 表示する CheckBox の種類
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeCheckBox,CheckBox,TreeNodeCheckBox.DoubleBorder, >

	// property: IndentArea
	//#define DESCRIPTION(VOID) 子要素を表示する際の IndentArea の種類
	//#define ATTR_DEFAULTVALUE		TreeNodeIndentArea.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeIndentArea,ChildIndent,TreeNodeIndentArea.Default, >

	// property: CheckBoxVisible
	//#define DESCRIPTION(VOID) CheckBox を表示するか否か
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(false)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Inherit)
	//#DeclareInheritProperty<bool,CheckBoxVisible,false, >

	// property: BackColor
	//#define DESCRIPTION(VOID) ノードを描画する際の背景色
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Color),"Transparent")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Color,BackColor,Gdi::Color.White, >

	// property: ForeColor
	//#define DESCRIPTION(VOID) ノードを描画する際の前景色
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Color),"Black")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Color,ForeColor,Gdi::Color.Black, >
	
	// property: Font
	//#define DESCRIPTION(VOID) ノードを描画する際のフォント
	//#define ATTR_DEFAULTVALUE		FontDefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Font,Font,Forms::Control.DefaultFont, >

	// property: DDBehavior
	//#define DESCRIPTION(VOID) ドラッグドロップに関する動作
	//#define ATTR_DEFAULTVALUE		TreeNodeDDBehavior.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeDDBehavior,DDBehavior,TreeNodeDDBehavior.Empty, >
	//===================================================================
	//		Events
	//===================================================================
	public partial class TreeNode{
		//#→template DeclareEvent0<EventHandler,EventName>
		//#define OnEventName	On##EventName
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		[CM::Description("DESCRIPTION")]
		public event EventHandler EventName{
			add{this.xmem.AddHandler("EventName",value);}
			remove{this.xmem.RemoveHandler("EventName",value);}
		}
		private void OnEventName(){
			EventHandler m;
			if(this.xmem.GetMember("EventName",out m))m(this);
		}
		//#←template
		//#→template DeclareEvent1<EventHandler,EventName,EventArgs>
		//#define OnEventName	On##EventName
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		[CM::Description("DESCRIPTION")]
		public event EventHandler EventName{
			add{this.xmem.AddHandler("EventName",value);}
			remove{this.xmem.RemoveHandler("EventName",value);}
		}
		private void OnEventName(EventArgs e){
			EventHandler m;
			if(this.xmem.GetMember("EventName",out m))m(this,e);
		}
		//#←template

		//#define DESCRIPTION	ノードの表示サイズが変化した際に発生します。
		//#define EventArgs		TreeNodePropertyChangingEventArgs<Gdi::Size>
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",DisplaySizeChanged,EventArgs>

		//#define DESCRIPTION ノードの有効/無効の設定が変化した時に発生します。
		//#define EventArgs		TreeNodePropertyChangingEventArgs<bool>
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",IsEnabledChanged,EventArgs>

		//#define DESCRIPTION ノードの選択状態が変化した時に発生します。
		//#define EventArgs		TreeNodePropertyChangingEventArgs<bool>
		//#define private		internal
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",IsSelectedChanged,EventArgs>

		//#define private internal
		//#define DESCRIPTION ノードが TreeView 内でのフォーカスを取得した場合に発生します。
		//#DeclareEvent0<"#afh.CallBack<TreeNode>#",AcquiredFocus>

		//#define DESCRIPTION ノードが TreeView 内でのフォーカスを失う前に発生します。
		//#DeclareEvent0<"#afh.CallBack<TreeNode>#",LosingFocus>

		//#define private private

		//#→template DeclareTreeNodeMouseEvent<EventName>
		//#define EventHandler	TreeNodeMouseEventHandler
		//#define EventArgs		TreeNodeMouseEventArgs
		//#define OnEventName	On##EventName
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		[CM::Description("DESCRIPTION")]
		public event EventHandler EventName{
			add{this.xmem.AddHandler("EventName",value);}
			remove{this.xmem.RemoveHandler("EventName",value);}
		}
		internal void OnEventName(object sender,EventArgs e){
			EventHandler m;
			if(this.xmem.GetMember("EventName",out m))m(sender,e);
		}
		//#←template
		//#→template DeclareTreeNodeEvent<EventName>
		//#define EventHandler	TreeNodeEventHandler
		//#define EventArgs		TreeNodeEventArgs
		//#define OnEventName	On##EventName
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		[CM::Description("DESCRIPTION")]
		public event EventHandler EventName{
			add{this.xmem.AddHandler("EventName",value);}
			remove{this.xmem.RemoveHandler("EventName",value);}
		}
		internal void OnEventName(object sender,EventArgs e){
			EventHandler m;
			if(this.xmem.GetMember("EventName",out m))m(sender,e);
		}
		//#←template

		//#define DESCRIPTTION マウスがその TreeNode に進入した時に発生します。
		//#DeclareTreeNodeEvent<MouseEnter>
		//#define DESCRIPTTION マウスのその TreeNode から退去した時に発生します。
		//#DeclareTreeNodeEvent<MouseLeave>
		//#define DESCRIPTTION マウスのボタンが下がった時に発生します。
		//#DeclareTreeNodeMouseEvent<MouseDown>
		//#define DESCRIPTTION マウスによってクリックされた時に発生します。
		//#DeclareTreeNodeMouseEvent<MouseClick>
		//#define DESCRIPTTION マウスによってクリックされた時に発生します。
		//#DeclareTreeNodeMouseEvent<MouseDoubleClick>
		//#define DESCRIPTTION マウスのボタンが上がった時に発生します。
		//#DeclareTreeNodeMouseEvent<MouseUp>
		//#define DESCRIPTTION マウスポインタが上で移動を行った時に発生します。
		//#DeclareTreeNodeMouseEvent<MouseMove>

#if DRAGDROP_EVENT
		//#→template DeclareTreeNodeDragEvent<EventName>
		//#define EventHandler	TreeNodeDragEventHandler
		//#define EventArgs		TreeNodeDragEventArgs
		//#define OnEventName	On##EventName
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		[CM::Description("DESCRIPTION")]
		public event EventHandler EventName{
		  add{this.xmem.AddHandler("EventName",value);}
		  remove{this.xmem.RemoveHandler("EventName",value);}
		}
		internal void OnEventName(object sender,EventArgs e){
		  EventHandler m;
		  if(this.xmem.GetMember("EventName",out m))m(sender,e);
		}
		//#←template
		//#define DESCRIPTTION ドラッグされたオブジェクトが上を移動した時に発生します。
		//#DeclareTreeNodeDragEvent<DragMove>
		//#define DESCRIPTTION オブジェクトがドロップされた直後に発生します。
		//#DeclareTreeNodeDragEvent<DragDrop>
#endif


	}
}
