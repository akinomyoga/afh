using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using CM=System.ComponentModel;

namespace afh.Forms{
	//===================================================================
	//		Inheritable Properties Definitions
	//===================================================================
	//#��template DeclareInheritProperty<T,PROPNAME,DEFAULT,ONCHANGED>
	public partial class TreeNode{
		//#define PropInherit	PROPNAME##Inherit
		//#define InheritBits	s##PROPNAME##Inherit
		//#define IS_BOOL		__afh::equal(T,bool)
		//#define PropBits		b##PROPNAME

		/// <summary>
		/// DESCRIPTION(x)�̌�����@���擾���͐ݒ肵�܂��B
		/// </summary>
		[ATTR_FOR_INHERITANCE]
		[CM::Description("DESCRIPTION(x)�̌�����@���擾���͐ݒ肵�܂��B")]
		public TreeNodeInheritType PropInherit{
			get{return (TreeNodeInheritType)this.bits[InheritBits];}
			set{this.bits[InheritBits]=(uint)value;}
		}
		/// <summary>
		/// DESCRIPTION(x)���擾���͐ݒ肵�܂��B
		/// </summary>
		[ATTR_DEFAULTVALUE]
		[CM::Description("DESCRIPTION(x)���擾���͐ݒ肵�܂��B")]
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
		/// DESCRIPTION(x)���擾���͐ݒ肵�܂��B
		/// </summary>
		[ATTR_DEFAULTVALUE]
		[CM::Description("DESCRIPTION(x)���擾���͐ݒ肵�܂��B")]
		public T PROPNAME{
			get{return this.MemberName;}
			set{
				if(this.MemberName==value)return;
				this.MemberName=value;
				this.OnPROPNAMEChanged();
			}
		}
		/// <summary>
		/// PROPNAME �̒l���ύX���ꂽ���ɔ�������C�x���g�ł��B
		/// </summary>
		[CM::Category("�v���p�e�B�ύX")]
		[CM::Description("PROPNAME �v���p�e�B�̒l���ς�������ɔ������܂��B")]
		public event afh.VoidCB PROPNAMEChanged;
		private void OnPROPNAMEChanged(){
			if(this.PROPNAMEChanged==null)return;
			this.PROPNAMEChanged();
		}
	}
	//#��template
	//===================================================================
	//		Inheritable Properties Declarations
	//===================================================================
	// �� sPROPNAMEInherit �͎����Œ�`���ĉ������B

	// property: NodeHeight
	//#define DESCRIPTION(VOID) ���̃m�[�h���̂̍���
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(14)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<int,NodeHeight,14,"#this.RefreshDisplaySize()#">

	// property: ChildIndentWidth [�p�~]
	//-#define DESCRIPTION(VOID) �q�m�[�h�̃C���f���g��
	//-#DeclareInheritProperty<int,ChildIndentWidth,18, >

	// property: IsEnabled
	//#define DESCRIPTION(VOID) �m�[�h���L���ł��邩�ۂ�
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(true)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Inherit)
	//#DeclareInheritProperty<bool,IsEnabled,true,"#this.OnIsEnabledChanged(true,new TreeNodePropertyChangingEventArgs<bool>(!value,value))#">

	// property: Icon
	//#define DESCRIPTION(VOID) TreeNode �ɕ\������A�C�R��
	//#define ATTR_DEFAULTVALUE		TreeNodeIcon.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeIcon,Icon,TreeNodeIcon.File, >

	// property: IconSize
	//#define DESCRIPTION(VOID) �A�C�R����\�����鎞�̑傫��
	//#define DefaultIconSize		new Gdi::Size(12,12)
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Size),"12,12")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Size,IconSize,DefaultIconSize, >

	// property: IconVisible
	//#define DESCRIPTION(VOID) �A�C�R����\�����邩�ۂ�
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(false)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<bool,IconVisible,false, >

	// property: CheckBox
	//#define DESCRIPTION(VOID) �\������ CheckBox �̎��
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(ITreeNodeCheckBox),"DoubleBorder")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeCheckBox,CheckBox,TreeNodeCheckBox.DoubleBorder, >

	// property: IndentArea
	//#define DESCRIPTION(VOID) �q�v�f��\������ۂ� IndentArea �̎��
	//#define ATTR_DEFAULTVALUE		TreeNodeIndentArea.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeIndentArea,ChildIndent,TreeNodeIndentArea.Default, >

	// property: CheckBoxVisible
	//#define DESCRIPTION(VOID) CheckBox ��\�����邩�ۂ�
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(false)
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Inherit)
	//#DeclareInheritProperty<bool,CheckBoxVisible,false, >

	// property: BackColor
	//#define DESCRIPTION(VOID) �m�[�h��`�悷��ۂ̔w�i�F
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Color),"Transparent")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Color,BackColor,Gdi::Color.White, >

	// property: ForeColor
	//#define DESCRIPTION(VOID) �m�[�h��`�悷��ۂ̑O�i�F
	//#define ATTR_DEFAULTVALUE		CM::DefaultValue(typeof(Gdi::Color),"Black")
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Color,ForeColor,Gdi::Color.Black, >
	
	// property: Font
	//#define DESCRIPTION(VOID) �m�[�h��`�悷��ۂ̃t�H���g
	//#define ATTR_DEFAULTVALUE		FontDefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<Gdi::Font,Font,Forms::Control.DefaultFont, >

	// property: DDBehavior
	//#define DESCRIPTION(VOID) �h���b�O�h���b�v�Ɋւ��铮��
	//#define ATTR_DEFAULTVALUE		TreeNodeDDBehavior.DefaultValue
	//#define ATTR_FOR_INHERITANCE	CM::DefaultValue(TreeNodeInheritType.Default)
	//#DeclareInheritProperty<ITreeNodeDDBehavior,DDBehavior,TreeNodeDDBehavior.Empty, >
	//===================================================================
	//		Events
	//===================================================================
	public partial class TreeNode{
		//#��template DeclareEvent0<EventHandler,EventName>
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
		//#��template
		//#��template DeclareEvent1<EventHandler,EventName,EventArgs>
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
		//#��template

		//#define DESCRIPTION	�m�[�h�̕\���T�C�Y���ω������ۂɔ������܂��B
		//#define EventArgs		TreeNodePropertyChangingEventArgs<Gdi::Size>
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",DisplaySizeChanged,EventArgs>

		//#define DESCRIPTION �m�[�h�̗L��/�����̐ݒ肪�ω��������ɔ������܂��B
		//#define EventArgs		TreeNodePropertyChangingEventArgs<bool>
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",IsEnabledChanged,EventArgs>

		//#define DESCRIPTION �m�[�h�̑I����Ԃ��ω��������ɔ������܂��B
		//#define EventArgs		TreeNodePropertyChangingEventArgs<bool>
		//#define private		internal
		//#DeclareEvent1<"#afh.EventHandler<TreeNode,EventArgs>#",IsSelectedChanged,EventArgs>

		//#define private internal
		//#define DESCRIPTION �m�[�h�� TreeView ���ł̃t�H�[�J�X���擾�����ꍇ�ɔ������܂��B
		//#DeclareEvent0<"#afh.CallBack<TreeNode>#",AcquiredFocus>

		//#define DESCRIPTION �m�[�h�� TreeView ���ł̃t�H�[�J�X�������O�ɔ������܂��B
		//#DeclareEvent0<"#afh.CallBack<TreeNode>#",LosingFocus>

		//#define private private

		//#��template DeclareTreeNodeMouseEvent<EventName>
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
		//#��template
		//#��template DeclareTreeNodeEvent<EventName>
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
		//#��template

		//#define DESCRIPTTION �}�E�X������ TreeNode �ɐi���������ɔ������܂��B
		//#DeclareTreeNodeEvent<MouseEnter>
		//#define DESCRIPTTION �}�E�X�̂��� TreeNode ����ދ��������ɔ������܂��B
		//#DeclareTreeNodeEvent<MouseLeave>
		//#define DESCRIPTTION �}�E�X�̃{�^���������������ɔ������܂��B
		//#DeclareTreeNodeMouseEvent<MouseDown>
		//#define DESCRIPTTION �}�E�X�ɂ���ăN���b�N���ꂽ���ɔ������܂��B
		//#DeclareTreeNodeMouseEvent<MouseClick>
		//#define DESCRIPTTION �}�E�X�ɂ���ăN���b�N���ꂽ���ɔ������܂��B
		//#DeclareTreeNodeMouseEvent<MouseDoubleClick>
		//#define DESCRIPTTION �}�E�X�̃{�^�����オ�������ɔ������܂��B
		//#DeclareTreeNodeMouseEvent<MouseUp>
		//#define DESCRIPTTION �}�E�X�|�C���^����ňړ����s�������ɔ������܂��B
		//#DeclareTreeNodeMouseEvent<MouseMove>

#if DRAGDROP_EVENT
		//#��template DeclareTreeNodeDragEvent<EventName>
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
		//#��template
		//#define DESCRIPTTION �h���b�O���ꂽ�I�u�W�F�N�g������ړ��������ɔ������܂��B
		//#DeclareTreeNodeDragEvent<DragMove>
		//#define DESCRIPTTION �I�u�W�F�N�g���h���b�v���ꂽ����ɔ������܂��B
		//#DeclareTreeNodeDragEvent<DragDrop>
#endif


	}
}
