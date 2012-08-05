/*
	このソースコードは [afh.Design.dll] afh.Design.TemplateProcessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Design.dll] afh.Design.TemplateProcessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using CM=System.ComponentModel;

namespace afh.Forms{
	//===================================================================
	//		Inheritable Properties Definitions
	//===================================================================

	//===================================================================
	//		Inheritable Properties Declarations
	//===================================================================
	// ※ sPROPNAMEInherit は自分で定義して下さい。

	// property: NodeHeight
	//#define DESCRIPTION(VOID) このノード自体の高さ
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(14)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define PropInherit	NodeHeightInherit
		//#define InheritBits	sNodeHeightInherit
		//#define IS_BOOL		__afh::equal(int,bool)
		//#define PropBits		bNodeHeight

		/// <summary>
		/// このノード自体の高さの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("このノード自体の高さの決定方法を取得又は設定します。")]
		public TreeNodeInheritType NodeHeightInherit{
			get{return (TreeNodeInheritType)this.bits[sNodeHeightInherit];}
			set{this.bits[sNodeHeightInherit]=(uint)value;}
		}
		/// <summary>
		/// このノード自体の高さを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(14)]
		[CM::Description("このノード自体の高さを取得又は設定します。")]
		public int NodeHeight{
			get{
				switch(this.NodeHeightInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.NodeHeight;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.NodeHeight;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bNodeHeight];
#else
						int ret;
						if(this.xmem.GetMember("NodeHeight",out ret))
							return ret;
						break;
#endif
				}
				return 14;
			}
			set {
//#define NO_ONCHANGED	__afh::equal(this.RefreshDisplaySize())
#if !false
				if(this.NodeHeightInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bNodeHeight]==value)return;
#	else
					int val;
					if(this.xmem.GetMember("NodeHeight",out val)&&val==value)return;
#	endif
				}else{
					this.NodeHeightInherit=TreeNodeInheritType.Custom;
				}
#else
				this.NodeHeightInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bNodeHeight]=value;
#else
				this.xmem["NodeHeight"]=value;
#endif
				this.RefreshDisplaySize();
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define MemberName m_NodeHeight
		//#define PROPNAMEChanged NodeHeightChanged
		//#define OnPROPNAMEChanged OnNodeHeightChanged
		private int m_NodeHeight=14;
		/// <summary>
		/// このノード自体の高さを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(14)]
		[CM::Description("このノード自体の高さを取得又は設定します。")]
		public int NodeHeight{
			get{return this.m_NodeHeight;}
			set{
				if(this.m_NodeHeight==value)return;
				this.m_NodeHeight=value;
				this.OnNodeHeightChanged();
			}
		}
		/// <summary>
		/// NodeHeight の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("NodeHeight プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB NodeHeightChanged;
		private void OnNodeHeightChanged(){
			if(this.NodeHeightChanged==null)return;
			this.NodeHeightChanged();
		}
	}

	// property: ChildIndentWidth [廃止]
	//-#define このノード自体の高さ 子ノードのインデント幅
	//-#DeclareInheritProperty<int,ChildIndentWidth,18, >

	// property: IsEnabled
	//#define このノード自体の高さ ノードが有効であるか否か
	//#define CM::DefaultValue(14)		CM::DefaultValue(true)
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Inherit)
	public partial class TreeNode{
		//#define NodeHeightInherit	IsEnabledInherit
		//#define sNodeHeightInherit	sIsEnabledInherit
		//#define false		__afh::equal(bool,bool)
		//#define bNodeHeight		bIsEnabled

		/// <summary>
		/// ノードが有効であるか否かの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Inherit)]
		[CM::Description("ノードが有効であるか否かの決定方法を取得又は設定します。")]
		public TreeNodeInheritType IsEnabledInherit{
			get{return (TreeNodeInheritType)this.bits[sIsEnabledInherit];}
			set{this.bits[sIsEnabledInherit]=(uint)value;}
		}
		/// <summary>
		/// ノードが有効であるか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(true)]
		[CM::Description("ノードが有効であるか否かを取得又は設定します。")]
		public bool IsEnabled{
			get{
				switch(this.IsEnabledInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.IsEnabled;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.IsEnabled;
						break;
					case TreeNodeInheritType.Custom:
#if true
						return this.bits[bIsEnabled];
#else
						bool ret;
						if(this.xmem.GetMember("IsEnabled",out ret))
							return ret;
						break;
#endif
				}
				return true;
			}
			set {
//#define false	__afh::equal(this.OnIsEnabledChanged(true,new TreeNodePropertyChangingEventArgs<bool>(!value,value)))
#if !false
				if(this.IsEnabledInherit==TreeNodeInheritType.Custom){
#	if true
					if(this.bits[bIsEnabled]==value)return;
#	else
					bool val;
					if(this.xmem.GetMember("IsEnabled",out val)&&val==value)return;
#	endif
				}else{
					this.IsEnabledInherit=TreeNodeInheritType.Custom;
				}
#else
				this.IsEnabledInherit=TreeNodeInheritType.Custom;
#endif
#if true
				this.bits[bIsEnabled]=value;
#else
				this.xmem["IsEnabled"]=value;
#endif
				this.OnIsEnabledChanged(true,new TreeNodePropertyChangingEventArgs<bool>(!value,value));
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_NodeHeight m_IsEnabled
		//#define NodeHeightChanged IsEnabledChanged
		//#define OnNodeHeightChanged OnIsEnabledChanged
		private bool m_IsEnabled=true;
		/// <summary>
		/// ノードが有効であるか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(true)]
		[CM::Description("ノードが有効であるか否かを取得又は設定します。")]
		public bool IsEnabled{
			get{return this.m_IsEnabled;}
			set{
				if(this.m_IsEnabled==value)return;
				this.m_IsEnabled=value;
				this.OnIsEnabledChanged();
			}
		}
		/// <summary>
		/// IsEnabled の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("IsEnabled プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB IsEnabledChanged;
		private void OnIsEnabledChanged(){
			if(this.IsEnabledChanged==null)return;
			this.IsEnabledChanged();
		}
	}

	// property: Icon
	//#define ノードが有効であるか否か TreeNode に表示するアイコン
	//#define CM::DefaultValue(true)		TreeNodeIcon.DefaultValue
	//#define CM::DefaultValue(TreeNodeInheritType.Inherit)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define IsEnabledInherit	IconInherit
		//#define sIsEnabledInherit	sIconInherit
		//#define true		__afh::equal(ITreeNodeIcon,bool)
		//#define bIsEnabled		bIcon

		/// <summary>
		/// TreeNode に表示するアイコンの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("TreeNode に表示するアイコンの決定方法を取得又は設定します。")]
		public TreeNodeInheritType IconInherit{
			get{return (TreeNodeInheritType)this.bits[sIconInherit];}
			set{this.bits[sIconInherit]=(uint)value;}
		}
		/// <summary>
		/// TreeNode に表示するアイコンを取得又は設定します。
		/// </summary>
		[TreeNodeIcon.DefaultValue]
		[CM::Description("TreeNode に表示するアイコンを取得又は設定します。")]
		public ITreeNodeIcon Icon{
			get{
				switch(this.IconInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.Icon;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.Icon;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bIcon];
#else
						ITreeNodeIcon ret;
						if(this.xmem.GetMember("Icon",out ret))
							return ret;
						break;
#endif
				}
				return TreeNodeIcon.File;
			}
			set {
//#define false	__afh::equal( )
#if !true
				if(this.IconInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bIcon]==value)return;
#	else
					ITreeNodeIcon val;
					if(this.xmem.GetMember("Icon",out val)&&val==value)return;
#	endif
				}else{
					this.IconInherit=TreeNodeInheritType.Custom;
				}
#else
				this.IconInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bIcon]=value;
#else
				this.xmem["Icon"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_IsEnabled m_Icon
		//#define IsEnabledChanged IconChanged
		//#define OnIsEnabledChanged OnIconChanged
		private ITreeNodeIcon m_Icon=TreeNodeIcon.File;
		/// <summary>
		/// TreeNode に表示するアイコンを取得又は設定します。
		/// </summary>
		[TreeNodeIcon.DefaultValue]
		[CM::Description("TreeNode に表示するアイコンを取得又は設定します。")]
		public ITreeNodeIcon Icon{
			get{return this.m_Icon;}
			set{
				if(this.m_Icon==value)return;
				this.m_Icon=value;
				this.OnIconChanged();
			}
		}
		/// <summary>
		/// Icon の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("Icon プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB IconChanged;
		private void OnIconChanged(){
			if(this.IconChanged==null)return;
			this.IconChanged();
		}
	}

	// property: IconSize
	//#define TreeNode に表示するアイコン アイコンを表示する時の大きさ
	//#define DefaultIconSize		new Gdi::Size(12,12)
	//#define TreeNodeIcon.DefaultValue		CM::DefaultValue(typeof(Gdi::Size),"12,12")
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define IconInherit	IconSizeInherit
		//#define sIconInherit	sIconSizeInherit
		//#define false		__afh::equal(Gdi::Size,bool)
		//#define bIcon		bIconSize

		/// <summary>
		/// アイコンを表示する時の大きさの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("アイコンを表示する時の大きさの決定方法を取得又は設定します。")]
		public TreeNodeInheritType IconSizeInherit{
			get{return (TreeNodeInheritType)this.bits[sIconSizeInherit];}
			set{this.bits[sIconSizeInherit]=(uint)value;}
		}
		/// <summary>
		/// アイコンを表示する時の大きさを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Size),"12,12")]
		[CM::Description("アイコンを表示する時の大きさを取得又は設定します。")]
		public Gdi::Size IconSize{
			get{
				switch(this.IconSizeInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.IconSize;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.IconSize;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bIconSize];
#else
						Gdi::Size ret;
						if(this.xmem.GetMember("IconSize",out ret))
							return ret;
						break;
#endif
				}
				return new Gdi::Size(12,12);
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.IconSizeInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bIconSize]==value)return;
#	else
					Gdi::Size val;
					if(this.xmem.GetMember("IconSize",out val)&&val==value)return;
#	endif
				}else{
					this.IconSizeInherit=TreeNodeInheritType.Custom;
				}
#else
				this.IconSizeInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bIconSize]=value;
#else
				this.xmem["IconSize"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_Icon m_IconSize
		//#define IconChanged IconSizeChanged
		//#define OnIconChanged OnIconSizeChanged
		private Gdi::Size m_IconSize=new Gdi::Size(12,12);
		/// <summary>
		/// アイコンを表示する時の大きさを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Size),"12,12")]
		[CM::Description("アイコンを表示する時の大きさを取得又は設定します。")]
		public Gdi::Size IconSize{
			get{return this.m_IconSize;}
			set{
				if(this.m_IconSize==value)return;
				this.m_IconSize=value;
				this.OnIconSizeChanged();
			}
		}
		/// <summary>
		/// IconSize の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("IconSize プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB IconSizeChanged;
		private void OnIconSizeChanged(){
			if(this.IconSizeChanged==null)return;
			this.IconSizeChanged();
		}
	}

	// property: IconVisible
	//#define アイコンを表示する時の大きさ アイコンを表示するか否か
	//#define CM::DefaultValue(typeof(Gdi::Size),"12,12")		CM::DefaultValue(false)
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define IconSizeInherit	IconVisibleInherit
		//#define sIconSizeInherit	sIconVisibleInherit
		//#define false		__afh::equal(bool,bool)
		//#define bIconSize		bIconVisible

		/// <summary>
		/// アイコンを表示するか否かの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("アイコンを表示するか否かの決定方法を取得又は設定します。")]
		public TreeNodeInheritType IconVisibleInherit{
			get{return (TreeNodeInheritType)this.bits[sIconVisibleInherit];}
			set{this.bits[sIconVisibleInherit]=(uint)value;}
		}
		/// <summary>
		/// アイコンを表示するか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(false)]
		[CM::Description("アイコンを表示するか否かを取得又は設定します。")]
		public bool IconVisible{
			get{
				switch(this.IconVisibleInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.IconVisible;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.IconVisible;
						break;
					case TreeNodeInheritType.Custom:
#if true
						return this.bits[bIconVisible];
#else
						bool ret;
						if(this.xmem.GetMember("IconVisible",out ret))
							return ret;
						break;
#endif
				}
				return false;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.IconVisibleInherit==TreeNodeInheritType.Custom){
#	if true
					if(this.bits[bIconVisible]==value)return;
#	else
					bool val;
					if(this.xmem.GetMember("IconVisible",out val)&&val==value)return;
#	endif
				}else{
					this.IconVisibleInherit=TreeNodeInheritType.Custom;
				}
#else
				this.IconVisibleInherit=TreeNodeInheritType.Custom;
#endif
#if true
				this.bits[bIconVisible]=value;
#else
				this.xmem["IconVisible"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_IconSize m_IconVisible
		//#define IconSizeChanged IconVisibleChanged
		//#define OnIconSizeChanged OnIconVisibleChanged
		private bool m_IconVisible=false;
		/// <summary>
		/// アイコンを表示するか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(false)]
		[CM::Description("アイコンを表示するか否かを取得又は設定します。")]
		public bool IconVisible{
			get{return this.m_IconVisible;}
			set{
				if(this.m_IconVisible==value)return;
				this.m_IconVisible=value;
				this.OnIconVisibleChanged();
			}
		}
		/// <summary>
		/// IconVisible の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("IconVisible プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB IconVisibleChanged;
		private void OnIconVisibleChanged(){
			if(this.IconVisibleChanged==null)return;
			this.IconVisibleChanged();
		}
	}

	// property: CheckBox
	//#define アイコンを表示するか否か 表示する CheckBox の種類
	//#define CM::DefaultValue(false)		CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define IconVisibleInherit	CheckBoxInherit
		//#define sIconVisibleInherit	sCheckBoxInherit
		//#define true		__afh::equal(ITreeNodeCheckBox,bool)
		//#define bIconVisible		bCheckBox

		/// <summary>
		/// 表示する CheckBox の種類の決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("表示する CheckBox の種類の決定方法を取得又は設定します。")]
		public TreeNodeInheritType CheckBoxInherit{
			get{return (TreeNodeInheritType)this.bits[sCheckBoxInherit];}
			set{this.bits[sCheckBoxInherit]=(uint)value;}
		}
		/// <summary>
		/// 表示する CheckBox の種類を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")]
		[CM::Description("表示する CheckBox の種類を取得又は設定します。")]
		public ITreeNodeCheckBox CheckBox{
			get{
				switch(this.CheckBoxInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.CheckBox;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.CheckBox;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bCheckBox];
#else
						ITreeNodeCheckBox ret;
						if(this.xmem.GetMember("CheckBox",out ret))
							return ret;
						break;
#endif
				}
				return TreeNodeCheckBox.DoubleBorder;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.CheckBoxInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bCheckBox]==value)return;
#	else
					ITreeNodeCheckBox val;
					if(this.xmem.GetMember("CheckBox",out val)&&val==value)return;
#	endif
				}else{
					this.CheckBoxInherit=TreeNodeInheritType.Custom;
				}
#else
				this.CheckBoxInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bCheckBox]=value;
#else
				this.xmem["CheckBox"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_IconVisible m_CheckBox
		//#define IconVisibleChanged CheckBoxChanged
		//#define OnIconVisibleChanged OnCheckBoxChanged
		private ITreeNodeCheckBox m_CheckBox=TreeNodeCheckBox.DoubleBorder;
		/// <summary>
		/// 表示する CheckBox の種類を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")]
		[CM::Description("表示する CheckBox の種類を取得又は設定します。")]
		public ITreeNodeCheckBox CheckBox{
			get{return this.m_CheckBox;}
			set{
				if(this.m_CheckBox==value)return;
				this.m_CheckBox=value;
				this.OnCheckBoxChanged();
			}
		}
		/// <summary>
		/// CheckBox の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("CheckBox プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB CheckBoxChanged;
		private void OnCheckBoxChanged(){
			if(this.CheckBoxChanged==null)return;
			this.CheckBoxChanged();
		}
	}

	// property: IndentArea
	//#define 表示する CheckBox の種類 子要素を表示する際の IndentArea の種類
	//#define CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")		TreeNodeIndentArea.DefaultValue
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define CheckBoxInherit	ChildIndentInherit
		//#define sCheckBoxInherit	sChildIndentInherit
		//#define false		__afh::equal(ITreeNodeIndentArea,bool)
		//#define bCheckBox		bChildIndent

		/// <summary>
		/// 子要素を表示する際の IndentArea の種類の決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("子要素を表示する際の IndentArea の種類の決定方法を取得又は設定します。")]
		public TreeNodeInheritType ChildIndentInherit{
			get{return (TreeNodeInheritType)this.bits[sChildIndentInherit];}
			set{this.bits[sChildIndentInherit]=(uint)value;}
		}
		/// <summary>
		/// 子要素を表示する際の IndentArea の種類を取得又は設定します。
		/// </summary>
		[TreeNodeIndentArea.DefaultValue]
		[CM::Description("子要素を表示する際の IndentArea の種類を取得又は設定します。")]
		public ITreeNodeIndentArea ChildIndent{
			get{
				switch(this.ChildIndentInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.ChildIndent;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.ChildIndent;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bChildIndent];
#else
						ITreeNodeIndentArea ret;
						if(this.xmem.GetMember("ChildIndent",out ret))
							return ret;
						break;
#endif
				}
				return TreeNodeIndentArea.Default;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.ChildIndentInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bChildIndent]==value)return;
#	else
					ITreeNodeIndentArea val;
					if(this.xmem.GetMember("ChildIndent",out val)&&val==value)return;
#	endif
				}else{
					this.ChildIndentInherit=TreeNodeInheritType.Custom;
				}
#else
				this.ChildIndentInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bChildIndent]=value;
#else
				this.xmem["ChildIndent"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_CheckBox m_ChildIndent
		//#define CheckBoxChanged ChildIndentChanged
		//#define OnCheckBoxChanged OnChildIndentChanged
		private ITreeNodeIndentArea m_ChildIndent=TreeNodeIndentArea.Default;
		/// <summary>
		/// 子要素を表示する際の IndentArea の種類を取得又は設定します。
		/// </summary>
		[TreeNodeIndentArea.DefaultValue]
		[CM::Description("子要素を表示する際の IndentArea の種類を取得又は設定します。")]
		public ITreeNodeIndentArea ChildIndent{
			get{return this.m_ChildIndent;}
			set{
				if(this.m_ChildIndent==value)return;
				this.m_ChildIndent=value;
				this.OnChildIndentChanged();
			}
		}
		/// <summary>
		/// ChildIndent の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("ChildIndent プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB ChildIndentChanged;
		private void OnChildIndentChanged(){
			if(this.ChildIndentChanged==null)return;
			this.ChildIndentChanged();
		}
	}

	// property: CheckBoxVisible
	//#define 子要素を表示する際の IndentArea の種類 CheckBox を表示するか否か
	//#define TreeNodeIndentArea.DefaultValue		CM::DefaultValue(false)
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Inherit)
	public partial class TreeNode{
		//#define ChildIndentInherit	CheckBoxVisibleInherit
		//#define sChildIndentInherit	sCheckBoxVisibleInherit
		//#define false		__afh::equal(bool,bool)
		//#define bChildIndent		bCheckBoxVisible

		/// <summary>
		/// CheckBox を表示するか否かの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Inherit)]
		[CM::Description("CheckBox を表示するか否かの決定方法を取得又は設定します。")]
		public TreeNodeInheritType CheckBoxVisibleInherit{
			get{return (TreeNodeInheritType)this.bits[sCheckBoxVisibleInherit];}
			set{this.bits[sCheckBoxVisibleInherit]=(uint)value;}
		}
		/// <summary>
		/// CheckBox を表示するか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(false)]
		[CM::Description("CheckBox を表示するか否かを取得又は設定します。")]
		public bool CheckBoxVisible{
			get{
				switch(this.CheckBoxVisibleInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.CheckBoxVisible;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.CheckBoxVisible;
						break;
					case TreeNodeInheritType.Custom:
#if true
						return this.bits[bCheckBoxVisible];
#else
						bool ret;
						if(this.xmem.GetMember("CheckBoxVisible",out ret))
							return ret;
						break;
#endif
				}
				return false;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.CheckBoxVisibleInherit==TreeNodeInheritType.Custom){
#	if true
					if(this.bits[bCheckBoxVisible]==value)return;
#	else
					bool val;
					if(this.xmem.GetMember("CheckBoxVisible",out val)&&val==value)return;
#	endif
				}else{
					this.CheckBoxVisibleInherit=TreeNodeInheritType.Custom;
				}
#else
				this.CheckBoxVisibleInherit=TreeNodeInheritType.Custom;
#endif
#if true
				this.bits[bCheckBoxVisible]=value;
#else
				this.xmem["CheckBoxVisible"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_ChildIndent m_CheckBoxVisible
		//#define ChildIndentChanged CheckBoxVisibleChanged
		//#define OnChildIndentChanged OnCheckBoxVisibleChanged
		private bool m_CheckBoxVisible=false;
		/// <summary>
		/// CheckBox を表示するか否かを取得又は設定します。
		/// </summary>
		[CM::DefaultValue(false)]
		[CM::Description("CheckBox を表示するか否かを取得又は設定します。")]
		public bool CheckBoxVisible{
			get{return this.m_CheckBoxVisible;}
			set{
				if(this.m_CheckBoxVisible==value)return;
				this.m_CheckBoxVisible=value;
				this.OnCheckBoxVisibleChanged();
			}
		}
		/// <summary>
		/// CheckBoxVisible の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("CheckBoxVisible プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB CheckBoxVisibleChanged;
		private void OnCheckBoxVisibleChanged(){
			if(this.CheckBoxVisibleChanged==null)return;
			this.CheckBoxVisibleChanged();
		}
	}

	// property: BackColor
	//#define CheckBox を表示するか否か ノードを描画する際の背景色
	//#define CM::DefaultValue(false)		CM::DefaultValue(typeof(Gdi::Color),"Transparent")
	//#define CM::DefaultValue(TreeNodeInheritType.Inherit)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define CheckBoxVisibleInherit	BackColorInherit
		//#define sCheckBoxVisibleInherit	sBackColorInherit
		//#define true		__afh::equal(Gdi::Color,bool)
		//#define bCheckBoxVisible		bBackColor

		/// <summary>
		/// ノードを描画する際の背景色の決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("ノードを描画する際の背景色の決定方法を取得又は設定します。")]
		public TreeNodeInheritType BackColorInherit{
			get{return (TreeNodeInheritType)this.bits[sBackColorInherit];}
			set{this.bits[sBackColorInherit]=(uint)value;}
		}
		/// <summary>
		/// ノードを描画する際の背景色を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Color),"Transparent")]
		[CM::Description("ノードを描画する際の背景色を取得又は設定します。")]
		public Gdi::Color BackColor{
			get{
				switch(this.BackColorInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.BackColor;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.BackColor;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bBackColor];
#else
						Gdi::Color ret;
						if(this.xmem.GetMember("BackColor",out ret))
							return ret;
						break;
#endif
				}
				return Gdi::Color.White;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.BackColorInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bBackColor]==value)return;
#	else
					Gdi::Color val;
					if(this.xmem.GetMember("BackColor",out val)&&val==value)return;
#	endif
				}else{
					this.BackColorInherit=TreeNodeInheritType.Custom;
				}
#else
				this.BackColorInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bBackColor]=value;
#else
				this.xmem["BackColor"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_CheckBoxVisible m_BackColor
		//#define CheckBoxVisibleChanged BackColorChanged
		//#define OnCheckBoxVisibleChanged OnBackColorChanged
		private Gdi::Color m_BackColor=Gdi::Color.White;
		/// <summary>
		/// ノードを描画する際の背景色を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Color),"Transparent")]
		[CM::Description("ノードを描画する際の背景色を取得又は設定します。")]
		public Gdi::Color BackColor{
			get{return this.m_BackColor;}
			set{
				if(this.m_BackColor==value)return;
				this.m_BackColor=value;
				this.OnBackColorChanged();
			}
		}
		/// <summary>
		/// BackColor の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("BackColor プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB BackColorChanged;
		private void OnBackColorChanged(){
			if(this.BackColorChanged==null)return;
			this.BackColorChanged();
		}
	}

	// property: ForeColor
	//#define ノードを描画する際の背景色 ノードを描画する際の前景色
	//#define CM::DefaultValue(typeof(Gdi::Color),"Transparent")		CM::DefaultValue(typeof(Gdi::Color),"Black")
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define BackColorInherit	ForeColorInherit
		//#define sBackColorInherit	sForeColorInherit
		//#define false		__afh::equal(Gdi::Color,bool)
		//#define bBackColor		bForeColor

		/// <summary>
		/// ノードを描画する際の前景色の決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("ノードを描画する際の前景色の決定方法を取得又は設定します。")]
		public TreeNodeInheritType ForeColorInherit{
			get{return (TreeNodeInheritType)this.bits[sForeColorInherit];}
			set{this.bits[sForeColorInherit]=(uint)value;}
		}
		/// <summary>
		/// ノードを描画する際の前景色を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Color),"Black")]
		[CM::Description("ノードを描画する際の前景色を取得又は設定します。")]
		public Gdi::Color ForeColor{
			get{
				switch(this.ForeColorInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.ForeColor;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.ForeColor;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bForeColor];
#else
						Gdi::Color ret;
						if(this.xmem.GetMember("ForeColor",out ret))
							return ret;
						break;
#endif
				}
				return Gdi::Color.Black;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.ForeColorInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bForeColor]==value)return;
#	else
					Gdi::Color val;
					if(this.xmem.GetMember("ForeColor",out val)&&val==value)return;
#	endif
				}else{
					this.ForeColorInherit=TreeNodeInheritType.Custom;
				}
#else
				this.ForeColorInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bForeColor]=value;
#else
				this.xmem["ForeColor"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_BackColor m_ForeColor
		//#define BackColorChanged ForeColorChanged
		//#define OnBackColorChanged OnForeColorChanged
		private Gdi::Color m_ForeColor=Gdi::Color.Black;
		/// <summary>
		/// ノードを描画する際の前景色を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(typeof(Gdi::Color),"Black")]
		[CM::Description("ノードを描画する際の前景色を取得又は設定します。")]
		public Gdi::Color ForeColor{
			get{return this.m_ForeColor;}
			set{
				if(this.m_ForeColor==value)return;
				this.m_ForeColor=value;
				this.OnForeColorChanged();
			}
		}
		/// <summary>
		/// ForeColor の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("ForeColor プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB ForeColorChanged;
		private void OnForeColorChanged(){
			if(this.ForeColorChanged==null)return;
			this.ForeColorChanged();
		}
	}
	
	// property: Font
	//#define ノードを描画する際の前景色 ノードを描画する際のフォント
	//#define CM::DefaultValue(typeof(Gdi::Color),"Black")		FontDefaultValue
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define ForeColorInherit	FontInherit
		//#define sForeColorInherit	sFontInherit
		//#define false		__afh::equal(Gdi::Font,bool)
		//#define bForeColor		bFont

		/// <summary>
		/// ノードを描画する際のフォントの決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("ノードを描画する際のフォントの決定方法を取得又は設定します。")]
		public TreeNodeInheritType FontInherit{
			get{return (TreeNodeInheritType)this.bits[sFontInherit];}
			set{this.bits[sFontInherit]=(uint)value;}
		}
		/// <summary>
		/// ノードを描画する際のフォントを取得又は設定します。
		/// </summary>
		[FontDefaultValue]
		[CM::Description("ノードを描画する際のフォントを取得又は設定します。")]
		public Gdi::Font Font{
			get{
				switch(this.FontInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.Font;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.Font;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bFont];
#else
						Gdi::Font ret;
						if(this.xmem.GetMember("Font",out ret))
							return ret;
						break;
#endif
				}
				return Forms::Control.DefaultFont;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.FontInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bFont]==value)return;
#	else
					Gdi::Font val;
					if(this.xmem.GetMember("Font",out val)&&val==value)return;
#	endif
				}else{
					this.FontInherit=TreeNodeInheritType.Custom;
				}
#else
				this.FontInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bFont]=value;
#else
				this.xmem["Font"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_ForeColor m_Font
		//#define ForeColorChanged FontChanged
		//#define OnForeColorChanged OnFontChanged
		private Gdi::Font m_Font=Forms::Control.DefaultFont;
		/// <summary>
		/// ノードを描画する際のフォントを取得又は設定します。
		/// </summary>
		[FontDefaultValue]
		[CM::Description("ノードを描画する際のフォントを取得又は設定します。")]
		public Gdi::Font Font{
			get{return this.m_Font;}
			set{
				if(this.m_Font==value)return;
				this.m_Font=value;
				this.OnFontChanged();
			}
		}
		/// <summary>
		/// Font の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("Font プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB FontChanged;
		private void OnFontChanged(){
			if(this.FontChanged==null)return;
			this.FontChanged();
		}
	}

	// property: DDBehavior
	//#define ノードを描画する際のフォント ドラッグドロップに関する動作
	//#define FontDefaultValue		TreeNodeDDBehavior.DefaultValue
	//#define CM::DefaultValue(TreeNodeInheritType.Default)	CM::DefaultValue(TreeNodeInheritType.Default)
	public partial class TreeNode{
		//#define FontInherit	DDBehaviorInherit
		//#define sFontInherit	sDDBehaviorInherit
		//#define false		__afh::equal(ITreeNodeDDBehavior,bool)
		//#define bFont		bDDBehavior

		/// <summary>
		/// ドラッグドロップに関する動作の決定方法を取得又は設定します。
		/// </summary>
		[CM::DefaultValue(TreeNodeInheritType.Default)]
		[CM::Description("ドラッグドロップに関する動作の決定方法を取得又は設定します。")]
		public TreeNodeInheritType DDBehaviorInherit{
			get{return (TreeNodeInheritType)this.bits[sDDBehaviorInherit];}
			set{this.bits[sDDBehaviorInherit]=(uint)value;}
		}
		/// <summary>
		/// ドラッグドロップに関する動作を取得又は設定します。
		/// </summary>
		[TreeNodeDDBehaviorStatic.DefaultValue]
		[CM::Description("ドラッグドロップに関する動作を取得又は設定します。")]
		public ITreeNodeDDBehavior DDBehavior{
			get{
				switch(this.DDBehaviorInherit){
					case TreeNodeInheritType.Default:
						if(this.view!=null)
							return this.view.DefaultNodeParams.DDBehavior;
						break;
					case TreeNodeInheritType.Inherit:
						if(this.parent!=null)
							return this.parent.DDBehavior;
						break;
					case TreeNodeInheritType.Custom:
#if false
						return this.bits[bDDBehavior];
#else
						ITreeNodeDDBehavior ret;
						if(this.xmem.GetMember("DDBehavior",out ret))
							return ret;
						break;
#endif
				}
				return TreeNodeDDBehaviorStatic.Empty;
			}
			set {
//#define true	__afh::equal( )
#if !true
				if(this.DDBehaviorInherit==TreeNodeInheritType.Custom){
#	if false
					if(this.bits[bDDBehavior]==value)return;
#	else
					ITreeNodeDDBehavior val;
					if(this.xmem.GetMember("DDBehavior",out val)&&val==value)return;
#	endif
				}else{
					this.DDBehaviorInherit=TreeNodeInheritType.Custom;
				}
#else
				this.DDBehaviorInherit=TreeNodeInheritType.Custom;
#endif
#if false
				this.bits[bDDBehavior]=value;
#else
				this.xmem["DDBehavior"]=value;
#endif
				 ;
			}
		}
	}
	public partial class TreeNodeSettings{
		//#define m_Font m_DDBehavior
		//#define FontChanged DDBehaviorChanged
		//#define OnFontChanged OnDDBehaviorChanged
		private ITreeNodeDDBehavior m_DDBehavior=TreeNodeDDBehaviorStatic.Empty;
		/// <summary>
		/// ドラッグドロップに関する動作を取得又は設定します。
		/// </summary>
		[TreeNodeDDBehaviorStatic.DefaultValue]
		[CM::Description("ドラッグドロップに関する動作を取得又は設定します。")]
		public ITreeNodeDDBehavior DDBehavior{
			get{return this.m_DDBehavior;}
			set{
				if(this.m_DDBehavior==value)return;
				this.m_DDBehavior=value;
				this.OnDDBehaviorChanged();
			}
		}
		/// <summary>
		/// DDBehavior の値が変更された時に発生するイベントです。
		/// </summary>
		[CM::Category("プロパティ変更")]
		[CM::Description("DDBehavior プロパティの値が変わった時に発生します。")]
		public event afh.VoidCB DDBehaviorChanged;
		private void OnDDBehaviorChanged(){
			if(this.DDBehaviorChanged==null)return;
			this.DDBehaviorChanged();
		}
	}
	//===================================================================
	//		Events
	//===================================================================
	public partial class TreeNode{


		//#define DESCRIPTION	ノードの表示サイズが変化した際に発生します。
		//#define EventArgs		TreeNodePropertyChangingEventArgs<Gdi::Size>
		//#define OnEventName	OnDisplaySizeChanged
		/// <summary>
		/// ノードの表示サイズが変化した際に発生します。
		/// </summary>
		[CM::Description("ノードの表示サイズが変化した際に発生します。")]
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<Gdi::Size>> DisplaySizeChanged{
			add{this.xmem.AddHandler("DisplaySizeChanged",value);}
			remove{this.xmem.RemoveHandler("DisplaySizeChanged",value);}
		}
		private void OnDisplaySizeChanged(TreeNodePropertyChangingEventArgs<Gdi::Size> e){
			afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<Gdi::Size>> m;
			if(this.xmem.GetMember("DisplaySizeChanged",out m))m(this,e);
		}

		//#define ノードの表示サイズが変化した際に発生します。 ノードの有効/無効の設定が変化した時に発生します。
		//#define TreeNodePropertyChangingEventArgs<Gdi::Size>		TreeNodePropertyChangingEventArgs<bool>
		//#define OnDisplaySizeChanged	OnIsEnabledChanged
		/// <summary>
		/// ノードの有効/無効の設定が変化した時に発生します。
		/// </summary>
		[CM::Description("ノードの有効/無効の設定が変化した時に発生します。")]
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> IsEnabledChanged{
			add{this.xmem.AddHandler("IsEnabledChanged",value);}
			remove{this.xmem.RemoveHandler("IsEnabledChanged",value);}
		}
		private void OnIsEnabledChanged(TreeNodePropertyChangingEventArgs<bool> e){
			afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> m;
			if(this.xmem.GetMember("IsEnabledChanged",out m))m(this,e);
		}

		//#define ノードの有効/無効の設定が変化した時に発生します。 ノードの選択状態が変化した時に発生します。
		//#define TreeNodePropertyChangingEventArgs<bool>		TreeNodePropertyChangingEventArgs<bool>
		//#define private		internal
		//#define OnIsEnabledChanged	OnIsSelectedChanged
		/// <summary>
		/// ノードの選択状態が変化した時に発生します。
		/// </summary>
		[CM::Description("ノードの選択状態が変化した時に発生します。")]
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> IsSelectedChanged{
			add{this.xmem.AddHandler("IsSelectedChanged",value);}
			remove{this.xmem.RemoveHandler("IsSelectedChanged",value);}
		}
		internal void OnIsSelectedChanged(TreeNodePropertyChangingEventArgs<bool> e){
			afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> m;
			if(this.xmem.GetMember("IsSelectedChanged",out m))m(this,e);
		}

		//#define internal internal
		//#define ノードの選択状態が変化した時に発生します。 ノードが TreeView 内でのフォーカスを取得した場合に発生します。
		//#define OnIsSelectedChanged	OnAcquiredFocus
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを取得した場合に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを取得した場合に発生します。")]
		public event afh.CallBack<TreeNode> AcquiredFocus{
			add{this.xmem.AddHandler("AcquiredFocus",value);}
			remove{this.xmem.RemoveHandler("AcquiredFocus",value);}
		}
		internal void OnAcquiredFocus(){
			afh.CallBack<TreeNode> m;
			if(this.xmem.GetMember("AcquiredFocus",out m))m(this);
		}

		//#define ノードが TreeView 内でのフォーカスを取得した場合に発生します。 ノードが TreeView 内でのフォーカスを失う前に発生します。
		//#define OnAcquiredFocus	OnLosingFocus
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event afh.CallBack<TreeNode> LosingFocus{
			add{this.xmem.AddHandler("LosingFocus",value);}
			remove{this.xmem.RemoveHandler("LosingFocus",value);}
		}
		internal void OnLosingFocus(){
			afh.CallBack<TreeNode> m;
			if(this.xmem.GetMember("LosingFocus",out m))m(this);
		}

		//#define internal internal



		//#define DESCRIPTTION マウスがその TreeNode に進入した時に発生します。
		//#define EventHandler	TreeNodeEventHandler
		//#define TreeNodePropertyChangingEventArgs<bool>		TreeNodeEventArgs
		//#define OnLosingFocus	OnMouseEnter
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeEventHandler MouseEnter{
			add{this.xmem.AddHandler("MouseEnter",value);}
			remove{this.xmem.RemoveHandler("MouseEnter",value);}
		}
		internal void OnMouseEnter(object sender,TreeNodeEventArgs e){
			TreeNodeEventHandler m;
			if(this.xmem.GetMember("MouseEnter",out m))m(sender,e);
		}
		//#define マウスがその TreeNode に進入した時に発生します。 マウスのその TreeNode から退去した時に発生します。
		//#define TreeNodeEventHandler	TreeNodeEventHandler
		//#define TreeNodeEventArgs		TreeNodeEventArgs
		//#define OnMouseEnter	OnMouseLeave
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeEventHandler MouseLeave{
			add{this.xmem.AddHandler("MouseLeave",value);}
			remove{this.xmem.RemoveHandler("MouseLeave",value);}
		}
		internal void OnMouseLeave(object sender,TreeNodeEventArgs e){
			TreeNodeEventHandler m;
			if(this.xmem.GetMember("MouseLeave",out m))m(sender,e);
		}
		//#define マウスのその TreeNode から退去した時に発生します。 マウスのボタンが下がった時に発生します。
		//#define TreeNodeEventHandler	TreeNodeMouseEventHandler
		//#define TreeNodeEventArgs		TreeNodeMouseEventArgs
		//#define OnMouseLeave	OnMouseDown
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeMouseEventHandler MouseDown{
			add{this.xmem.AddHandler("MouseDown",value);}
			remove{this.xmem.RemoveHandler("MouseDown",value);}
		}
		internal void OnMouseDown(object sender,TreeNodeMouseEventArgs e){
			TreeNodeMouseEventHandler m;
			if(this.xmem.GetMember("MouseDown",out m))m(sender,e);
		}
		//#define マウスのボタンが下がった時に発生します。 マウスによってクリックされた時に発生します。
		//#define TreeNodeMouseEventHandler	TreeNodeMouseEventHandler
		//#define TreeNodeMouseEventArgs		TreeNodeMouseEventArgs
		//#define OnMouseDown	OnMouseClick
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeMouseEventHandler MouseClick{
			add{this.xmem.AddHandler("MouseClick",value);}
			remove{this.xmem.RemoveHandler("MouseClick",value);}
		}
		internal void OnMouseClick(object sender,TreeNodeMouseEventArgs e){
			TreeNodeMouseEventHandler m;
			if(this.xmem.GetMember("MouseClick",out m))m(sender,e);
		}
		//#define マウスによってクリックされた時に発生します。 マウスによってクリックされた時に発生します。
		//#define TreeNodeMouseEventHandler	TreeNodeMouseEventHandler
		//#define TreeNodeMouseEventArgs		TreeNodeMouseEventArgs
		//#define OnMouseClick	OnMouseDoubleClick
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeMouseEventHandler MouseDoubleClick{
			add{this.xmem.AddHandler("MouseDoubleClick",value);}
			remove{this.xmem.RemoveHandler("MouseDoubleClick",value);}
		}
		internal void OnMouseDoubleClick(object sender,TreeNodeMouseEventArgs e){
			TreeNodeMouseEventHandler m;
			if(this.xmem.GetMember("MouseDoubleClick",out m))m(sender,e);
		}
		//#define マウスによってクリックされた時に発生します。 マウスのボタンが上がった時に発生します。
		//#define TreeNodeMouseEventHandler	TreeNodeMouseEventHandler
		//#define TreeNodeMouseEventArgs		TreeNodeMouseEventArgs
		//#define OnMouseDoubleClick	OnMouseUp
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeMouseEventHandler MouseUp{
			add{this.xmem.AddHandler("MouseUp",value);}
			remove{this.xmem.RemoveHandler("MouseUp",value);}
		}
		internal void OnMouseUp(object sender,TreeNodeMouseEventArgs e){
			TreeNodeMouseEventHandler m;
			if(this.xmem.GetMember("MouseUp",out m))m(sender,e);
		}
		//#define マウスのボタンが上がった時に発生します。 マウスポインタが上で移動を行った時に発生します。
		//#define TreeNodeMouseEventHandler	TreeNodeMouseEventHandler
		//#define TreeNodeMouseEventArgs		TreeNodeMouseEventArgs
		//#define OnMouseUp	OnMouseMove
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeMouseEventHandler MouseMove{
			add{this.xmem.AddHandler("MouseMove",value);}
			remove{this.xmem.RemoveHandler("MouseMove",value);}
		}
		internal void OnMouseMove(object sender,TreeNodeMouseEventArgs e){
			TreeNodeMouseEventHandler m;
			if(this.xmem.GetMember("MouseMove",out m))m(sender,e);
		}

#if DRAGDROP_EVENT

		//#define マウスポインタが上で移動を行った時に発生します。 ドラッグされたオブジェクトが上を移動した時に発生します。
		//#define TreeNodeMouseEventHandler	TreeNodeDragEventHandler
		//#define TreeNodeMouseEventArgs		TreeNodeDragEventArgs
		//#define OnMouseMove	OnDragMove
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeDragEventHandler DragMove{
		  add{this.xmem.AddHandler("DragMove",value);}
		  remove{this.xmem.RemoveHandler("DragMove",value);}
		}
		internal void OnDragMove(object sender,TreeNodeDragEventArgs e){
		  TreeNodeDragEventHandler m;
		  if(this.xmem.GetMember("DragMove",out m))m(sender,e);
		}
		//#define ドラッグされたオブジェクトが上を移動した時に発生します。 オブジェクトがドロップされた直後に発生します。
		//#define TreeNodeDragEventHandler	TreeNodeDragEventHandler
		//#define TreeNodeDragEventArgs		TreeNodeDragEventArgs
		//#define OnDragMove	OnDragDrop
		/// <summary>
		/// ノードが TreeView 内でのフォーカスを失う前に発生します。
		/// </summary>
		[CM::Description("ノードが TreeView 内でのフォーカスを失う前に発生します。")]
		public event TreeNodeDragEventHandler DragDrop{
		  add{this.xmem.AddHandler("DragDrop",value);}
		  remove{this.xmem.RemoveHandler("DragDrop",value);}
		}
		internal void OnDragDrop(object sender,TreeNodeDragEventArgs e){
		  TreeNodeDragEventHandler m;
		  if(this.xmem.GetMember("DragDrop",out m))m(sender,e);
		}
#endif


	}
}
