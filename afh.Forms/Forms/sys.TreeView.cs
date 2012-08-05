using Frms=System.Windows.Forms;
using Gdi=System.Drawing;
using Gen=System.Collections.Generic;
using CM=System.ComponentModel;
using Interop=System.Runtime.InteropServices;
using Ref=System.Reflection;
using TV=mwg.Win32.TreeView;

namespace mwg.SysForms{
	using mwg.Win32;

	[Interop::ComVisible(true),Interop::ClassInterface(Interop::ClassInterfaceType.AutoDispatch)]
	[CM::DefaultEvent("AfterSelect"),CM::Designer("System.Windows.Forms.Design.TreeViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Frms::Docking(Frms::DockingBehavior.Ask),CM::DefaultProperty("Nodes")]
	//[SRDescription("DescriptionTreeView")]
	public class TreeView:Frms::Control{
		private static readonly string backSlash = @"\";
		private static readonly int MaxIndent = 0x7d00;
		private const int TREEVIEWSTATE_checkBoxes = 8;
		private const int TREEVIEWSTATE_doubleclickFired = 0x800;
		private const int TREEVIEWSTATE_fullRowSelect = 0x200;
		private const int TREEVIEWSTATE_hideSelection = 1;
		private const int TREEVIEWSTATE_hotTracking = 0x100;
		private const int TREEVIEWSTATE_ignoreSelects = 0x10000;
		private const int TREEVIEWSTATE_labelEdit = 2;
		private const int TREEVIEWSTATE_lastControlValidated = 0x4000;
		private const int TREEVIEWSTATE_mouseUpFired = 0x1000;
		private const int TREEVIEWSTATE_scrollable = 4;
		private const int TREEVIEWSTATE_showLines = 0x10;
		private const int TREEVIEWSTATE_showNodeToolTips = 0x400;
		private const int TREEVIEWSTATE_showPlusMinus = 0x20;
		private const int TREEVIEWSTATE_showRootLines = 0x40;
		private const int TREEVIEWSTATE_showTreeViewContextMenu = 0x2000;
		private const int TREEVIEWSTATE_sorted = 0x80;
		private const int TREEVIEWSTATE_stopResizeWindowMsgs = 0x8000;

		private Frms::BorderStyle borderStyle = Frms::BorderStyle.Fixed3D;
		private string controlToolTipText;
		private const int DefaultTreeViewIndent = 0x13;
		private Frms::MouseButtons downButton;
		private Frms::TreeViewDrawMode drawMode;
		private System.IntPtr hNodeMouseDown = System.IntPtr.Zero;
		private bool hoveredAlready;
		private int indent = -1;
		private Frms::ImageList imageList;
		private Frms::ImageList internalStateImageList;
		private Frms::ImageList stateImageList;
		private Frms::ImageList.Indexer imageIndexer;
		private Frms::ImageList.Indexer selectedImageIndexer;
		private Frms::TreeNode prevHoveredNode;
		private Frms::TreeNode topNode;
		internal Frms::TreeNode editNode;
		internal Frms::TreeNode root;
		internal Frms::TreeNode selectedNode;
		internal Frms::TreeNodeCollection nodes;
		private int itemHeight = -1;
		private Gdi::Color lineColor;
		private string pathSeparator = backSlash;
		private bool rightToLeftLayout;
		private bool setOddHeight;
		private System.Collections.IComparer treeViewNodeSorter;
		private System.Collections.Specialized.BitVector32 treeViewState = new BitVector32(0x75);
		internal bool nodesCollectionClear;
		internal Gen::Dictionary<System.IntPtr,Frms::TreeNode> nodeTable = new Gen::Dictionary<System.IntPtr,Frms::TreeNode>();

		//[SRCategory("CatBehavior"),SRDescription("TreeViewAfterCheckDescr")]
		public event Frms::TreeViewEventHandler AfterCheck;

		//[SRCategory("CatBehavior"),SRDescription("TreeViewAfterCollapseDescr")]
		public event Frms::TreeViewEventHandler AfterCollapse;

		//[SRDescription("TreeViewAfterExpandDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeViewEventHandler AfterExpand;

		//[SRCategory("CatBehavior"),SRDescription("TreeViewAfterEditDescr")]
		public event Frms::NodeLabelEditEventHandler AfterLabelEdit;

		//[SRDescription("TreeViewAfterSelectDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeViewEventHandler AfterSelect;

		//[SRDescription("TreeViewBeforeCheckDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeViewCancelEventHandler BeforeCheck;

		//[SRCategory("CatBehavior"),SRDescription("TreeViewBeforeCollapseDescr")]
		public event Frms::TreeViewCancelEventHandler BeforeCollapse;

		//[SRDescription("TreeViewBeforeExpandDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeViewCancelEventHandler BeforeExpand;

		//[SRDescription("TreeViewBeforeEditDescr"),SRCategory("CatBehavior")]
		public event Frms::NodeLabelEditEventHandler BeforeLabelEdit;

		//[SRDescription("TreeViewBeforeSelectDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeViewCancelEventHandler BeforeSelect;

		//[SRCategory("CatBehavior"),SRDescription("TreeViewDrawNodeEventDescr")]
		public event Frms::DrawTreeNodeEventHandler DrawNode;

		//[SRCategory("CatAction"),SRDescription("ListViewItemDragDescr")]
		public event Frms::ItemDragEventHandler ItemDrag;

		//[SRCategory("CatBehavior"),SRDescription("TreeViewNodeMouseClickDescr")]
		public event Frms::TreeNodeMouseClickEventHandler NodeMouseClick;

		//[SRDescription("TreeViewNodeMouseDoubleClickDescr"),SRCategory("CatBehavior")]
		public event Frms::TreeNodeMouseClickEventHandler NodeMouseDoubleClick;

		//[SRCategory("CatAction"),SRDescription("TreeViewNodeMouseHoverDescr")]
		public event Frms::TreeNodeMouseHoverEventHandler NodeMouseHover;

		//[SRDescription("ControlOnRightToLeftLayoutChangedDescr"),SRCategory("CatPropertyChanged")]
		public event System.EventHandler RightToLeftLayoutChanged;

		[CM::EditorBrowsable(CM::EditorBrowsableState.Never),CM::Browsable(false)]
		public event System.EventHandler BackgroundImageChanged {
			add {
				base.BackgroundImageChanged += value;
			}
			remove {
				base.BackgroundImageChanged -= value;
			}
		}

		[CM::Browsable(false),CM::EditorBrowsable(CM::EditorBrowsableState.Never)]
		public event System.EventHandler BackgroundImageLayoutChanged {
			add {
				base.BackgroundImageLayoutChanged += value;
			}
			remove {
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		[CM::EditorBrowsable(CM::EditorBrowsableState.Never),CM::Browsable(false)]
		public event System.EventHandler PaddingChanged {
			add {
				base.PaddingChanged += value;
			}
			remove {
				base.PaddingChanged -= value;
			}
		}

		[CM::EditorBrowsable(CM::EditorBrowsableState.Never),CM::Browsable(false)]
		public event Frms::PaintEventHandler Paint {
			add {
				base.Paint += value;
			}
			remove {
				base.Paint -= value;
			}
		}

		[CM::Browsable(false),CM::EditorBrowsable(CM::EditorBrowsableState.Never)]
		public event System.EventHandler TextChanged{
			add {
				base.TextChanged += value;
			}
			remove {
				base.TextChanged -= value;
			}
		}

		public TreeView() {
			this.root = new Frms::TreeNode(this);
			this.SelectedImageIndexer.Index = 0;
			this.ImageIndexer.Index = 0;
			base.SetStyle(Frms::ControlStyles.UserPaint,false);
			base.SetStyle(Frms::ControlStyles.StandardClick,false);
			base.SetStyle(Frms::ControlStyles.UseTextForAccessibility,false);
		}

		public void BeginUpdate() {
			base.BeginUpdateInternal();
		}

		public void CollapseAll() {
			this.root.Collapse();
		}

		private void ContextMenuStripClosing(object sender,Frms::ToolStripDropDownClosingEventArgs e) {
			Frms::ContextMenuStrip strip = sender as Frms::ContextMenuStrip;
			strip.Closing -= new Frms::ToolStripDropDownClosingEventHandler(this.ContextMenuStripClosing);
			base.SendMessage(0x110b,8,(string)null);
		}

		/*
		protected override void CreateHandle() {
			if(!base.RecreatingHandle) {
				System.IntPtr userCookie=UnsafeNativeMethods.ThemingScope.Activate();
				try{
					NativeMethods.INITCOMMONCONTROLSEX icc=new NativeMethods.INITCOMMONCONTROLSEX();
					icc.dwICC = 2;
					SafeNativeMethods.InitCommonControlsEx(icc);
				}finally{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}
		//*/


		private unsafe void CustomDraw(ref Frms::Message m) {
			Frms::TreeNode node;

			NMTVCUSTOMDRAW* lp=(NMTVCUSTOMDRAW*)m.LParam;

			NMTVCUSTOMDRAW lParam=(NMTVCUSTOMDRAW)m.GetLParam(typeof(NMTVCUSTOMDRAW));
			switch(lParam.nmcd.dwDrawStage){
				case 0x10001:
					node=this.NodeFromHandle(lParam.nmcd.dwItemSpec);
					if(node==null){
						m.Result=(System.IntPtr)4;
						return;
					}

					int uItemState = lParam.nmcd.uItemState;
					if(this.drawMode==Frms::TreeViewDrawMode.OwnerDrawText) {
						lParam.clrText = lParam.clrTextBk;
						Interop::Marshal.StructureToPtr(lParam,m.LParam,false);
						m.Result = (IntPtr)0x12;
						return;
					}
					if(this.drawMode==Frms::TreeViewDrawMode.OwnerDrawAll) {
						Frms::DrawTreeNodeEventArgs args;
						Gdi::Graphics graphics = Gdi::Graphics.FromHdcInternal(lParam.nmcd.hdc);
						try{
							Gdi::Rectangle rowBounds = node.RowBounds;
							SCROLLINFO si = new SCROLLINFO();
							si.cbSize=Interop::Marshal.SizeOf(typeof(SCROLLINFO));
							si.fMask=4;
							if(User32.GetScrollInfo(new Interop::HandleRef(this,base.Handle),0,ref si)) {
								int nPos=si.nPos;
								if(nPos>0) {
									rowBounds.X-=nPos;
									rowBounds.Width+=nPos;
								}
							}
							args = new Frms::DrawTreeNodeEventArgs(graphics,node,rowBounds,(Frms::TreeNodeStates)uItemState);
							this.OnDrawNode(args);
						}finally{
							graphics.Dispose();
						}
						if(!args.DrawDefault){
							m.Result=(System.IntPtr)4;
							return;
						}
					}

					Frms::OwnerDrawPropertyBag itemRenderStyles = this.GetItemRenderStyles(node,uItemState);
					if(itemRenderStyles==null)break;

					Gdi::Color color;
					if(!(color=itemRenderStyles.ForeColor).IsEmpty){
						lp->clrText=Gdi::ColorTranslator.ToWin32(color);
					}
					if(!(color=itemRenderStyles.BackColor).IsEmpty){
						lp->clrTextBk=Gdi::ColorTranslator.ToWin32(color);
					}
					
					if(itemRenderStyles.Font==null)break;
					Gdi32.SelectObject(lp->nmcd,itemRenderStyles);

					m.Result=(System.IntPtr)2;
					return;

				case 0x10002:
					if(this.drawMode!=Frms::TreeViewDrawMode.OwnerDrawText) {
						break;
					}

					node = this.NodeFromHandle(lParam.nmcd.dwItemSpec);
					if(node != null) {
						Gdi::Graphics graphics = Graphics.FromHdcInternal(lParam.nmcd.hdc);
						try {
							Gdi::Rectangle bounds = node.Bounds;
							Gdi::Size size = Frms::TextRenderer.MeasureText(node.Text,node.TreeView.Font);
							Gdi::Point location = new Gdi::Point(bounds.X-1,bounds.Y);
							bounds = new Gdi::Rectangle(location,new Gdi::Size(size.Width,bounds.Height));
							Frms::DrawTreeNodeEventArgs e = new Frms::DrawTreeNodeEventArgs(graphics,node,bounds,(Frms::TreeNodeStates)lParam.nmcd.uItemState);
							this.OnDrawNode(e);

							if(e.DrawDefault){
								Frms::TreeNodeStates state = e.State;
								Gdi::Font font = (node.NodeFont != null) ? node.NodeFont : node.TreeView.Font;
								Gdi::Color color
									=(state&Frms::TreeNodeStates.Selected)==Frms::TreeNodeStates.Selected&&node.TreeView.Focused ? Gdi::SystemColors.HighlightText
									:node.ForeColor!=Gdi::Color.Empty ? node.ForeColor
									:node.TreeView.ForeColor;
								if((state&Frms::TreeNodeStates.Selected)==Frms::TreeNodeStates.Selected){
									graphics.FillRectangle(Gdi::SystemBrushes.Highlight,bounds);
									Frms::ControlPaint.DrawFocusRectangle(graphics,bounds,color,Gdi::SystemColors.Highlight);
								}else{
									graphics.FillRectangle(Gdi::SystemBrushes.Window,bounds);
								}

								Frms::TextRenderer.DrawText(graphics,e.Node.Text,font,bounds,color,Frms::TextFormatFlags.GlyphOverhangPadding);
							}
						}finally{
							graphics.Dispose();
						}
						m.Result=(System.IntPtr)0x20;
						return;
					}
					return;

				case 1:
					m.Result = (System.IntPtr)0x20;
					return;
			}
			m.Result=System.IntPtr.Zero;
		}

		private void DetachImageList(object sender,System.EventArgs e) {
			this.ImageList = null;
		}

		private void DetachStateImageList(object sender,System.EventArgs e) {
			this.internalStateImageList = null;
			this.StateImageList = null;
		}

		protected override void Dispose(bool disposing) {
			if(disposing){
				foreach(Frms::TreeNode node in this.Nodes){
					node.ContextMenu = null;
				}
				lock(this){
					if(this.imageList!=null){
						this.imageList.Disposed-=new System.EventHandler(this.DetachImageList);
						this.imageList=null;
					}
					if(this.stateImageList != null){
						this.stateImageList.Disposed -= new System.EventHandler(this.DetachStateImageList);
						this.stateImageList = null;
					}
				}
			}
			base.Dispose(disposing);
		}

		public void EndUpdate() {
			base.EndUpdateInternal();
		}

		public void ExpandAll() {
			this.root.ExpandAll();
		}

		internal void ForceScrollbarUpdate(bool delayed) {
			if(!base.IsUpdating() && base.IsHandleCreated) {
				base.SendMessage((int)WM.SETREDRAW,0,0);
				if(delayed){
					User32.PostMessage(this,WM.SETREDRAW,(System.IntPtr)1,System.IntPtr.Zero);
				}else{
					base.SendMessage((int)WM.SETREDRAW,1,0);
				}
			}
		}

		protected Frms::OwnerDrawPropertyBag GetItemRenderStyles(Frms::TreeNode node,int state) {
			Frms::OwnerDrawPropertyBag bag = new Frms::OwnerDrawPropertyBag();
			if((node != null) && (node.propBag != null)) {
				if((state & 0x47) == 0) {
					bag.ForeColor = node.propBag.ForeColor;
					bag.BackColor = node.propBag.BackColor;
				}
				bag.Font = node.propBag.Font;
			}
			return bag;
		}

		public Frms::TreeNode GetNodeAt(Gdi::Point pt) {
			return this.GetNodeAt(pt.X,pt.Y);
		}

		public Frms::TreeNode GetNodeAt(int x,int y) {
			TV.HITTESTINFO lParam=new TV.HITTESTINFO();
			lParam.pt_x=x;
			lParam.pt_y=y;
			System.IntPtr handle=TV.HitTest(this,&lParam);
			return handle==System.IntPtr.Zero?null:this.NodeFromHandle(handle);
		}

		public int GetNodeCount(bool includeSubTrees) {
			return this.root.GetNodeCount(includeSubTrees);
		}

		public Frms::TreeViewHitTestInfo HitTest(Gdi::Point pt) {
			return this.HitTest(pt.X,pt.Y);
		}

		public Frms::TreeViewHitTestInfo HitTest(int x,int y) {
			TV.HITTESTINFO lParam=new TV.HITTESTINFO();
			lParam.pt_x=x;
			lParam.pt_y=y;
			System.IntPtr handle=TV.HitTest(this,&lParam);
			Frms::TreeNode hitNode=System.IntPtr.Zero?null:this.NodeFromHandle(handle);
			return new Frms::TreeViewHitTestInfo(hitNode,(Frms::TreeViewHitTestLocations)lParam.flags);
		}

		private void ImageListChangedHandle(object sender,System.EventArgs e) {
			if(sender!=null&&sender==this.imageList&&base.IsHandleCreated){
				this.BeginUpdate();
				foreach(Frms::TreeNode node in this.Nodes) {
					this.UpdateImagesRecursive(node);
				}
				this.EndUpdate();
			}
		}

		private void ImageListRecreateHandle(object sender,System.EventArgs e) {
			if(base.IsHandleCreated) {
				IntPtr lparam = (this.ImageList == null) ? IntPtr.Zero : this.ImageList.Handle;
				base.SendMessage(0x1109,0,lparam);
			}
		}

		protected override bool IsInputKey(Frms::Keys keyData) {
			if((this.editNode != null) && ((keyData & Keys.Alt) == Keys.None)) {
				switch((keyData & Keys.KeyCode)) {
					case Keys.Prior:
					case Keys.Next:
					case Keys.End:
					case Keys.Home:
					case Keys.Return:
					case Keys.Escape:
						return true;
				}
			}
			return base.IsInputKey(keyData);
		}

		internal Frms::TreeNode NodeFromHandle(System.IntPtr handle) {
			return (Frms::TreeNode)this.nodeTable[handle];
		}

		protected virtual void OnAfterCheck(Frms::TreeViewEventArgs e) {
			if(this.AfterCheck != null) {
				this.AfterCheck(this,e);
			}
		}

		protected internal virtual void OnAfterCollapse(Frms::TreeViewEventArgs e) {
			if(this.AfterCollapse != null) {
				this.AfterCollapse(this,e);
			}
		}

		protected virtual void OnAfterExpand(Frms::TreeViewEventArgs e) {
			if(this.AfterExpand != null) {
				this.AfterExpand(this,e);
			}
		}

		protected virtual void OnAfterLabelEdit(Frms::NodeLabelEditEventArgs e) {
			if(this.AfterLabelEdit != null) {
				this.AfterLabelEdit(this,e);
			}
		}

		protected virtual void OnAfterSelect(Frms::TreeViewEventArgs e) {
			if(this.AfterSelect != null) {
				this.AfterSelect(this,e);
			}
		}

		protected virtual void OnBeforeCheck(Frms::TreeViewCancelEventArgs e) {
			if(this.BeforeCheck != null) {
				this.BeforeCheck(this,e);
			}
		}

		protected internal virtual void OnBeforeCollapse(Frms::TreeViewCancelEventArgs e) {
			if(this.BeforeCollapse != null) {
				this.BeforeCollapse(this,e);
			}
		}

		protected virtual void OnBeforeExpand(Frms::TreeViewCancelEventArgs e) {
			if(this.BeforeExpand != null) {
				this.BeforeExpand(this,e);
			}
		}

		protected virtual void OnBeforeLabelEdit(Frms::NodeLabelEditEventArgs e) {
			if(this.BeforeLabelEdit != null) {
				this.BeforeLabelEdit(this,e);
			}
		}

		protected virtual void OnBeforeSelect(Frms::TreeViewCancelEventArgs e) {
			if(this.BeforeSelect != null) {
				this.BeforeSelect(this,e);
			}
		}

		protected virtual void OnDrawNode(Frms::DrawTreeNodeEventArgs e) {
			if(this.DrawNode != null) {
				this.DrawNode(this,e);
			}
		}

		protected override void OnHandleCreated(System.EventArgs e) {
			Frms::TreeNode selectedNode=this.selectedNode;
			this.selectedNode=null;

			base.OnHandleCreated(e);

			int num=(int)base.SendMessage(0x2008,0,0);
			if(num<5) {
				base.SendMessage(0x2007,5,0);
			}
			if(this.CheckBoxes) {
				System.IntPtr windowLong=User32.GetWindowLongPtr(this,-16);
				windowLong|=(System.IntPtr)TVS.CHECKBOXES;
				User32.SetWindowLongPtr(this,-16,windowLong);
			}
			if(this.ShowNodeToolTips&&!base.DesignMode) {
				System.IntPtr windowLong=User32.GetWindowLongPtr(new HandleRef(this,base.Handle),-16);
				windowLong|=(System.IntPtr)TVS.INFOTIP;
				User32.SetWindowLongPtr(this,-16,windowLong);
			}
			Gdi::Color color;
			if((color = this.BackColor) != Gdi::SystemColors.Window) {
				base.SendMessage(0x111d,0,Gdi::ColorTranslator.ToWin32(color));
			}
			if((color = this.ForeColor) != Gdi::SystemColors.WindowText) {
				base.SendMessage(0x111e,0,Gdi::ColorTranslator.ToWin32(color));
			}
			if(this.lineColor != Gdi::Color.Empty) {
				base.SendMessage(0x1128,0,Gdi::ColorTranslator.ToWin32(this.lineColor));
			}
			if(this.imageList != null) {
				base.SendMessage(0x1109,0,this.imageList.Handle);
			}
			if(this.stateImageList != null&&this.stateImageList.Images.Count > 0) {
				Gdi::Image[] images = new Gdi::Image[this.stateImageList.Images.Count + 1];
				images[0] = this.stateImageList.Images[0];
				for(int i = 1;i <= this.stateImageList.Images.Count;i++) {
					images[i] = this.stateImageList.Images[i - 1];
				}
				this.internalStateImageList = new Frms::ImageList();
				this.internalStateImageList.Images.AddRange(images);
				base.SendMessage(0x1109,2,this.internalStateImageList.Handle);
			}
			if(this.indent!=-1) {
				base.SendMessage(0x1107,this.indent,0);
			}
			if(this.itemHeight!=-1) {
				base.SendMessage(0x111b,this.ItemHeight,0);
			}
			int cx = 0;
			try {
				this.treeViewState[0x8000] = true;
				cx = base.Width;
				int flags = 0x16;
				SafeNativeMethods.SetWindowPos(new HandleRef(this,base.Handle),NativeMethods.NullHandleRef,base.Left,base.Top,0x7fffffff,base.Height,flags);
				this.root.Realize(false);
				if(cx != 0) {
					SafeNativeMethods.SetWindowPos(new HandleRef(this,base.Handle),NativeMethods.NullHandleRef,base.Left,base.Top,cx,base.Height,flags);
				}
			} finally {
				this.treeViewState[0x8000] = false;
			}
			this.SelectedNode = selectedNode;
		}

		protected override void OnHandleDestroyed(System.EventArgs e) {
			this.selectedNode = this.SelectedNode;
			base.OnHandleDestroyed(e);
		}

		protected virtual void OnItemDrag(Frms::ItemDragEventArgs e) {
			if(this.ItemDrag != null) {
				this.ItemDrag(this,e);
			}
		}

		protected override void OnKeyDown(Frms::KeyEventArgs e) {
			base.OnKeyDown(e);
			if(!e.Handled && (this.CheckBoxes && ((e.KeyData & Keys.KeyCode) == Keys.Space))) {
				TreeNode selectedNode = this.SelectedNode;
				if(selectedNode != null) {
					if(!this.TreeViewBeforeCheck(selectedNode,TreeViewAction.ByKeyboard)) {
						selectedNode.CheckedInternal = !selectedNode.CheckedInternal;
						this.TreeViewAfterCheck(selectedNode,TreeViewAction.ByKeyboard);
					}
					e.Handled = true;
				}
			}
		}

		protected override void OnKeyPress(Frms::KeyPressEventArgs e) {
			base.OnKeyPress(e);
			if(!e.Handled && (e.KeyChar == ' ')) {
				e.Handled = true;
			}
		}

		protected override void OnKeyUp(Frms::KeyEventArgs e) {
			base.OnKeyUp(e);
			if(!e.Handled && ((e.KeyData & Keys.KeyCode) == Keys.Space)) {
				e.Handled = true;
			}
		}

		protected override void OnMouseHover(System.EventArgs e) {
			TV.HITTESTINFO lParam = new TV.HITTESTINFO();
			Gdi::Point position=Cursor.Position;
			position = base.PointToClientInternal(position);
			lParam.pt_x = position.X;
			lParam.pt_y = position.Y;
			System.IntPtr handle=TV.HitTest(this,&lParam);
			if(handle!=System.IntPtr.Zero&&(lParam.flags&70)!=0) {
				Frms::TreeNode node = this.NodeFromHandle(handle);
				if((node != this.prevHoveredNode) && (node != null)) {
					this.OnNodeMouseHover(new Frms::TreeNodeMouseHoverEventArgs(node));
					this.prevHoveredNode = node;
				}
			}
			if(!this.hoveredAlready) {
				base.OnMouseHover(e);
				this.hoveredAlready = true;
			}
			base.ResetMouseEventArgs();
		}

		protected override void OnMouseLeave(System.EventArgs e) {
			this.hoveredAlready = false;
			base.OnMouseLeave(e);
		}

		protected virtual void OnNodeMouseClick(Frms::TreeNodeMouseClickEventArgs e) {
			if(this.onNodeMouseClick != null) {
				this.onNodeMouseClick(this,e);
			}
		}

		protected virtual void OnNodeMouseDoubleClick(Frms::TreeNodeMouseClickEventArgs e) {
			if(this.onNodeMouseDoubleClick != null) {
				this.onNodeMouseDoubleClick(this,e);
			}
		}

		protected virtual void OnNodeMouseHover(Frms::TreeNodeMouseHoverEventArgs e) {
			if(this.onNodeMouseHover != null) {
				this.onNodeMouseHover(this,e);
			}
		}

		[CM::EditorBrowsable(CM::EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(System.EventArgs e) {
			if(!base.GetAnyDisposingInHierarchy()) {
				if(this.RightToLeft == Frms::RightToLeft.Yes) {
					base.RecreateHandle();
				}
				if(this.RightToLeftLayoutChanged != null) {
					this.RightToLeftLayoutChanged(this,e);
				}
			}
		}

		private void RefreshNodes() {
			Frms::TreeNode[] dest = new TreeNode[this.Nodes.Count];
			this.Nodes.CopyTo(dest,0);
			this.Nodes.Clear();
			this.Nodes.AddRange(dest);
		}

		private void ResetIndent() {
			this.indent = -1;
			base.RecreateHandle();
		}

		private void ResetItemHeight() {
			this.itemHeight = -1;
			base.RecreateHandle();
		}

		internal void SetToolTip(Frms::ToolTip toolTip,string toolTipText) {
			if(toolTip != null) {
				UnsafeNativeMethods.SendMessage(new HandleRef(toolTip,toolTip.Handle),0x418,0,SystemInformation.MaxWindowTrackSize.Width);
				UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1118,new HandleRef(toolTip,toolTip.Handle),0);
				this.controlToolTipText = toolTipText;
			}
		}

		private bool ShouldSerializeImageIndex() {
			if(this.imageList != null) {
				return (this.ImageIndex != 0);
			}
			return (this.ImageIndex != -1);
		}

		private bool ShouldSerializeIndent() {
			return (this.indent != -1);
		}

		private bool ShouldSerializeItemHeight() {
			return (this.itemHeight != -1);
		}

		private bool ShouldSerializeSelectedImageIndex() {
			if(this.imageList != null) {
				return (this.SelectedImageIndex != 0);
			}
			return (this.SelectedImageIndex != -1);
		}

		private void ShowContextMenu(Frms::TreeNode treeNode) {
			if((treeNode.ContextMenu != null) || (treeNode.ContextMenuStrip != null)) {
				ContextMenu contextMenu = treeNode.ContextMenu;
				ContextMenuStrip contextMenuStrip = treeNode.ContextMenuStrip;
				if(contextMenu != null) {
					POINT pt = new POINT();
					UnsafeNativeMethods.GetCursorPos(pt);
					UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this,base.Handle));
					contextMenu.OnPopup(System.EventArgs.Empty);
					SafeNativeMethods.TrackPopupMenuEx(new Interop::HandleRef(contextMenu,contextMenu.Handle),0x40,pt.x,pt.y,new Interop::HandleRef(this,base.Handle),null);
					User32.PostMessage(wnd,WM.NULL,System.IntPtr.Zero,System.IntPtr.Zero);
				} else if(contextMenuStrip != null) {
					User32.PostMessage(wnd,TVM.SELECTITEM,(System.IntPtr)8,treeNode.Handle);
					contextMenuStrip.ShowInternal(this,base.PointToClient(Frms::Control.MousePosition),false);
					contextMenuStrip.Closing += new Frms::ToolStripDropDownClosingEventHandler(this.ContextMenuStripClosing);
				}
			}
		}

		public void Sort() {
			this.Sorted = true;
			this.RefreshNodes();
		}

		private void StateImageListChangedHandle(object sender,System.EventArgs e) {
			if(((sender != null) && (sender == this.stateImageList)) && base.IsHandleCreated) {
				if((this.stateImageList != null) && (this.stateImageList.Images.Count > 0)) {
					Image[] images = new Image[this.stateImageList.Images.Count + 1];
					images[0] = this.stateImageList.Images[0];
					for(int i = 1;i <= this.stateImageList.Images.Count;i++) {
						images[i] = this.stateImageList.Images[i - 1];
					}
					if(this.internalStateImageList != null) {
						this.internalStateImageList.Images.Clear();
						this.internalStateImageList.Images.AddRange(images);
					} else {
						this.internalStateImageList = new ImageList();
						this.internalStateImageList.Images.AddRange(images);
					}
					if(this.internalStateImageList != null) {
						base.SendMessage(0x1109,2,this.internalStateImageList.Handle);
					}
				} else {
					this.UpdateCheckedState(this.root,true);
				}
			}
		}

		private void StateImageListRecreateHandle(object sender,System.EventArgs e) {
			if(base.IsHandleCreated) {
				System.IntPtr zero = IntPtr.Zero;
				if(this.internalStateImageList != null) {
					zero = this.internalStateImageList.Handle;
				}
				base.SendMessage(0x1109,2,zero);
			}
		}

		public override string ToString() {
			string str = base.ToString();
			if(this.Nodes != null) {
				str = str + ", Nodes.Count: " + this.Nodes.Count.ToString(CultureInfo.CurrentCulture);
				if(this.Nodes.Count > 0) {
					str = str + ", Nodes[0]: " + this.Nodes[0].ToString();
				}
			}
			return str;
		}

		internal void TreeViewAfterCheck(Frms::TreeNode node,Frms::TreeViewAction actionTaken) {
			this.OnAfterCheck(new TreeViewEventArgs(node,actionTaken));
		}

		internal bool TreeViewBeforeCheck(Frms::TreeNode node,Frms::TreeViewAction actionTaken) {
			TreeViewCancelEventArgs e = new TreeViewCancelEventArgs(node,false,actionTaken);
			this.OnBeforeCheck(e);
			return e.Cancel;
		}

		private unsafe void TvnBeginDrag(Frms::MouseButtons buttons,NativeMethods.NMTREEVIEW* nmtv) {
			NativeMethods.TV_ITEM itemNew = nmtv.itemNew;
			if(itemNew.hItem != IntPtr.Zero) {
				TreeNode item = this.NodeFromHandle(itemNew.hItem);
				this.OnItemDrag(new ItemDragEventArgs(buttons,item));
			}
		}

		private System.IntPtr TvnBeginLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi) {
			if(nmtvdi.item.hItem == IntPtr.Zero) {
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtvdi.item.hItem);
			NodeLabelEditEventArgs e = new NodeLabelEditEventArgs(node);
			this.OnBeforeLabelEdit(e);
			if(!e.CancelEdit) {
				this.editNode = node;
			}
			return (e.CancelEdit ? ((IntPtr)1) : IntPtr.Zero);
		}

		private System.IntPtr TvnEndLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi) {
			this.editNode = null;
			if(nmtvdi.item.hItem == IntPtr.Zero) {
				return (IntPtr)1;
			}
			TreeNode node = this.NodeFromHandle(nmtvdi.item.hItem);
			string label = (nmtvdi.item.pszText == IntPtr.Zero) ? null : Marshal.PtrToStringAuto(nmtvdi.item.pszText);
			NodeLabelEditEventArgs e = new NodeLabelEditEventArgs(node,label);
			this.OnAfterLabelEdit(e);
			if(((label != null) && !e.CancelEdit) && (node != null)) {
				node.text = label;
				if(this.Scrollable) {
					this.ForceScrollbarUpdate(true);
				}
			}
			return (e.CancelEdit ? IntPtr.Zero : ((IntPtr)1));
		}

		private unsafe void TvnExpanded(NativeMethods.NMTREEVIEW* nmtv) {
			NativeMethods.TV_ITEM itemNew = nmtv.itemNew;
			if(itemNew.hItem != IntPtr.Zero) {
				TreeViewEventArgs args;
				TreeNode node = this.NodeFromHandle(itemNew.hItem);
				if((itemNew.state & 0x20) == 0) {
					args = new TreeViewEventArgs(node,TreeViewAction.Collapse);
					this.OnAfterCollapse(args);
				} else {
					args = new TreeViewEventArgs(node,TreeViewAction.Expand);
					this.OnAfterExpand(args);
				}
			}
		}

		private unsafe System.IntPtr TvnExpanding(NativeMethods.NMTREEVIEW* nmtv) {
			NativeMethods.TV_ITEM itemNew = nmtv.itemNew;
			if(itemNew.hItem == IntPtr.Zero) {
				return IntPtr.Zero;
			}
			TreeViewCancelEventArgs e = null;
			if((itemNew.state & 0x20) == 0) {
				e = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem),false,TreeViewAction.Expand);
				this.OnBeforeExpand(e);
			} else {
				e = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem),false,TreeViewAction.Collapse);
				this.OnBeforeCollapse(e);
			}
			return (e.Cancel ? ((IntPtr)1) : IntPtr.Zero);
		}

		private unsafe void TvnSelected(NativeMethods.NMTREEVIEW* nmtv) {
			if(!this.nodesCollectionClear) {
				if(nmtv.itemNew.hItem != IntPtr.Zero) {
					TreeViewAction unknown = TreeViewAction.Unknown;
					switch(nmtv.action) {
						case 1:
							unknown = TreeViewAction.ByMouse;
							break;

						case 2:
							unknown = TreeViewAction.ByKeyboard;
							break;
					}
					this.OnAfterSelect(new TreeViewEventArgs(this.NodeFromHandle(nmtv.itemNew.hItem),unknown));
				}
				NativeMethods.RECT lParam = new NativeMethods.RECT();
				(IntPtr) &lParam.left = nmtv.itemOld.hItem;
				if((nmtv.itemOld.hItem != IntPtr.Zero) && (((int)UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1104,1,ref lParam)) != 0)) {
					SafeNativeMethods.InvalidateRect(new HandleRef(this,base.Handle),ref lParam,true);
				}
			}
		}

		private unsafe System.IntPtr TvnSelecting(NativeMethods.NMTREEVIEW* nmtv) {
			if(this.treeViewState[0x10000]) {
				return (IntPtr)1;
			}
			if(nmtv.itemNew.hItem == IntPtr.Zero) {
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtv.itemNew.hItem);
			TreeViewAction unknown = TreeViewAction.Unknown;
			switch(nmtv.action) {
				case 1:
					unknown = TreeViewAction.ByMouse;
					break;

				case 2:
					unknown = TreeViewAction.ByKeyboard;
					break;
			}
			TreeViewCancelEventArgs e = new TreeViewCancelEventArgs(node,false,unknown);
			this.OnBeforeSelect(e);
			return (e.Cancel ? ((IntPtr)1) : IntPtr.Zero);
		}

		private void UpdateCheckedState(Frms::TreeNode node,bool update) {
			if(update) {
				node.CheckedInternal = node.CheckedInternal;
				for(int i = node.Nodes.Count - 1;i >= 0;i--) {
					this.UpdateCheckedState(node.Nodes[i],update);
				}
			} else {
				node.CheckedInternal = false;
				for(int j = node.Nodes.Count - 1;j >= 0;j--) {
					this.UpdateCheckedState(node.Nodes[j],update);
				}
			}
		}

		private void UpdateImagesRecursive(Frms::TreeNode node) {
			node.UpdateImage();
			foreach(TreeNode node2 in node.Nodes) {
				this.UpdateImagesRecursive(node2);
			}
		}

		internal override void UpdateStylesCore() {
			base.UpdateStylesCore();
			if((base.IsHandleCreated && this.CheckBoxes) && ((this.StateImageList != null) && (this.internalStateImageList != null))) {
				base.SendMessage(0x1109,2,this.internalStateImageList.Handle);
			}
		}

		private void WmMouseDown(ref Frms::Message m,Frms::MouseButtons button,int clicks) {
			base.SendMessage(0x110b,8,(string)null);
			this.OnMouseDown(new MouseEventArgs(button,clicks,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
			if(!base.ValidationCancelled) {
				this.DefWndProc(ref m);
			}
		}

		private void WmNeedText(ref Frms::Message m) {
			NativeMethods.TOOLTIPTEXT lParam = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			string controlToolTipText = this.controlToolTipText;
			NativeMethods.TV_HITTESTINFO tv_hittestinfo = new NativeMethods.TV_HITTESTINFO();
			Point position = Cursor.Position;
			position = base.PointToClientInternal(position);
			tv_hittestinfo.pt_x = position.X;
			tv_hittestinfo.pt_y = position.Y;
			IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo);
			if((handle != IntPtr.Zero) && ((tv_hittestinfo.flags & 70) != 0)) {
				TreeNode node = this.NodeFromHandle(handle);
				if((this.ShowNodeToolTips && (node != null)) && !string.IsNullOrEmpty(node.ToolTipText)) {
					controlToolTipText = node.ToolTipText;
				} else if((node != null) && (node.Bounds.Right > base.Bounds.Right)) {
					controlToolTipText = node.Text;
				} else {
					controlToolTipText = null;
				}
			}
			lParam.lpszText = controlToolTipText;
			lParam.hinst = IntPtr.Zero;
			if(this.RightToLeft == RightToLeft.Yes) {
				lParam.uFlags |= 4;
			}
			Marshal.StructureToPtr(lParam,m.LParam,false);
		}

		private unsafe void WmNotify(ref Frms::Message m) {
			NativeMethods.NMHDR* lParam = (NativeMethods.NMHDR*)m.LParam;
			if(lParam->code == -12) {
				this.CustomDraw(ref m);
			} else {
				NativeMethods.NMTREEVIEW* nmtv = (NativeMethods.NMTREEVIEW*)m.LParam;
				switch(nmtv->nmhdr.code) {
					case -5:
					case -2: {
							MouseButtons left = MouseButtons.Left;
							NativeMethods.TV_HITTESTINFO tv_hittestinfo = new NativeMethods.TV_HITTESTINFO();
							Point position = Cursor.Position;
							position = base.PointToClientInternal(position);
							tv_hittestinfo.pt_x = position.X;
							tv_hittestinfo.pt_y = position.Y;
							IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo);
							if((nmtv->nmhdr.code != -2) || ((tv_hittestinfo.flags & 70) != 0)) {
								left = (nmtv->nmhdr.code == -2) ? MouseButtons.Left : MouseButtons.Right;
							}
							if((((nmtv->nmhdr.code != -2) || ((tv_hittestinfo.flags & 70) != 0)) || this.FullRowSelect) && ((handle != IntPtr.Zero) && !base.ValidationCancelled)) {
								this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(handle),left,1,position.X,position.Y));
								this.OnClick(new MouseEventArgs(left,1,position.X,position.Y,0));
								this.OnMouseClick(new MouseEventArgs(left,1,position.X,position.Y,0));
							}
							if(nmtv->nmhdr.code == -5) {
								TreeNode treeNode = this.NodeFromHandle(handle);
								if((treeNode != null) && ((treeNode.ContextMenu != null) || (treeNode.ContextMenuStrip != null))) {
									this.ShowContextMenu(treeNode);
								} else {
									this.treeViewState[0x2000] = true;
									base.SendMessage(0x7b,base.Handle,SafeNativeMethods.GetMessagePos());
								}
								m.Result = (IntPtr)1;
							}
							if(!this.treeViewState[0x1000] && ((nmtv->nmhdr.code != -2) || ((tv_hittestinfo.flags & 70) != 0))) {
								this.OnMouseUp(new MouseEventArgs(left,1,position.X,position.Y,0));
								this.treeViewState[0x1000] = true;
							}
							break;
						}
					case -460:
					case -411:
						m.Result = this.TvnEndLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
						return;

					case -459:
					case -410:
						m.Result = this.TvnBeginLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
						return;

					case -458:
					case -453:
					case -452:
					case -409:
					case -404:
					case -403:
						break;

					case -457:
					case -408:
						this.TvnBeginDrag(MouseButtons.Right,nmtv);
						return;

					case -456:
					case -407:
						this.TvnBeginDrag(MouseButtons.Left,nmtv);
						return;

					case -455:
					case -406:
						this.TvnExpanded(nmtv);
						return;

					case -454:
					case -405:
						m.Result = this.TvnExpanding(nmtv);
						return;

					case -451:
					case -402:
						this.TvnSelected(nmtv);
						return;

					case -450:
					case -401:
						m.Result = this.TvnSelecting(nmtv);
						return;

					default:
						return;
				}
			}
		}

		private void WmPrint(ref Frms::Message m) {
			base.WndProc(ref m);
			if((((2 & ((int)m.LParam)) != 0) && Application.RenderWithVisualStyles) && (this.BorderStyle == BorderStyle.Fixed3D)) {
				IntSecurity.UnmanagedCode.Assert();
				try {
					using(Graphics graphics = Graphics.FromHdc(m.WParam)) {
						Rectangle rect = new Rectangle(0,0,base.Size.Width - 1,base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder),rect);
						rect.Inflate(-1,-1);
						graphics.DrawRectangle(SystemPens.Window,rect);
					}
				} finally {
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		private unsafe bool WmShowToolTip(ref Frms::Message m) {
			NativeMethods.NMHDR* lParam = (NativeMethods.NMHDR*)m.LParam;
			IntPtr hwndFrom = lParam->hwndFrom;
			NativeMethods.TV_HITTESTINFO tv_hittestinfo = new NativeMethods.TV_HITTESTINFO();
			Point position = Cursor.Position;
			position = base.PointToClientInternal(position);
			tv_hittestinfo.pt_x = position.X;
			tv_hittestinfo.pt_y = position.Y;
			IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo);
			if((handle != IntPtr.Zero) && ((tv_hittestinfo.flags & 70) != 0)) {
				TreeNode node = this.NodeFromHandle(handle);
				if((node != null) && !this.ShowNodeToolTips) {
					Rectangle bounds = node.Bounds;
					bounds.Location = base.PointToScreen(bounds.Location);
					UnsafeNativeMethods.SendMessage(new HandleRef(this,hwndFrom),0x41f,1,ref bounds);
					SafeNativeMethods.SetWindowPos(new HandleRef(this,hwndFrom),NativeMethods.HWND_TOPMOST,bounds.Left,bounds.Top,0,0,0x15);
					return true;
				}
			}
			return false;
		}

		[SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Frms::Message m) {
			switch(m.Msg) {
				case 0x4e: {
						NativeMethods.NMHDR lParam = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
						switch(lParam.code) {
							case -521:
								if(!this.WmShowToolTip(ref m)) {
									base.WndProc(ref m);
									return;
								}
								m.Result = (IntPtr)1;
								return;

							case -520:
							case -530:
								UnsafeNativeMethods.SendMessage(new HandleRef(lParam,lParam.hwndFrom),0x418,0,SystemInformation.MaxWindowTrackSize.Width);
								this.WmNeedText(ref m);
								m.Result = (IntPtr)1;
								return;
						}
						base.WndProc(ref m);
						return;
					}
				case 0x7b: {
						if(this.treeViewState[0x2000]) {
							this.treeViewState[0x2000] = false;
							base.WndProc(ref m);
							return;
						}
						TreeNode selectedNode = this.SelectedNode;
						if((selectedNode == null) || ((selectedNode.ContextMenu == null) && (selectedNode.ContextMenuStrip == null))) {
							base.WndProc(ref m);
							return;
						}
						Point pt = new Point(selectedNode.Bounds.X,selectedNode.Bounds.Y + (selectedNode.Bounds.Height / 2));
						if(base.ClientRectangle.Contains(pt)) {
							if(selectedNode.ContextMenu != null) {
								selectedNode.ContextMenu.Show(this,pt);
								return;
							}
							if(selectedNode.ContextMenuStrip == null) {
								return;
							}
							bool isKeyboardActivated = ((int)((long)m.LParam)) == -1;
							selectedNode.ContextMenuStrip.ShowInternal(this,pt,isKeyboardActivated);
						}
						return;
					}
				case 0x83:
				case 5:
				case 70:
				case 0x47:
					if(this.treeViewState[0x8000]) {
						this.DefWndProc(ref m);
						return;
					}
					base.WndProc(ref m);
					return;

				case 7:
					if(!this.treeViewState[0x4000]) {
						base.WndProc(ref m);
						return;
					}
					this.treeViewState[0x4000] = false;
					base.WmImeSetFocus();
					this.DefWndProc(ref m);
					this.OnGotFocus(EventArgs.Empty);
					return;

				case 0x15:
					base.SendMessage(0x1107,this.Indent,0);
					base.WndProc(ref m);
					return;

				case 0x201: {
						try {
							this.treeViewState[0x10000] = true;
							this.FocusInternal();
						} finally {
							this.treeViewState[0x10000] = false;
						}
						this.treeViewState[0x1000] = false;
						NativeMethods.TV_HITTESTINFO tv_hittestinfo = new NativeMethods.TV_HITTESTINFO();
						tv_hittestinfo.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_hittestinfo.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo);
						if((tv_hittestinfo.flags & 0x40) != 0) {
							this.OnMouseDown(new MouseEventArgs(MouseButtons.Left,1,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
							if(!base.ValidationCancelled && this.CheckBoxes) {
								TreeNode node = this.NodeFromHandle(this.hNodeMouseDown);
								if(!this.TreeViewBeforeCheck(node,TreeViewAction.ByMouse) && (node != null)) {
									node.CheckedInternal = !node.CheckedInternal;
									this.TreeViewAfterCheck(node,TreeViewAction.ByMouse);
								}
							}
							m.Result = IntPtr.Zero;
						} else {
							this.WmMouseDown(ref m,MouseButtons.Left,1);
						}
						this.downButton = MouseButtons.Left;
						return;
					}
				case 0x202:
				case 0x205: {
						NativeMethods.TV_HITTESTINFO tv_hittestinfo2 = new NativeMethods.TV_HITTESTINFO();
						tv_hittestinfo2.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_hittestinfo2.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo2);
						if(handle != IntPtr.Zero) {
							if(!base.ValidationCancelled && (!this.treeViewState[0x800] & !this.treeViewState[0x1000])) {
								if(handle == this.hNodeMouseDown) {
									this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(handle),this.downButton,1,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam)));
								}
								this.OnClick(new MouseEventArgs(this.downButton,1,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
								this.OnMouseClick(new MouseEventArgs(this.downButton,1,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
							}
							if(this.treeViewState[0x800]) {
								this.treeViewState[0x800] = false;
								if(!base.ValidationCancelled) {
									this.OnNodeMouseDoubleClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(handle),this.downButton,2,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam)));
									this.OnDoubleClick(new MouseEventArgs(this.downButton,2,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
									this.OnMouseDoubleClick(new MouseEventArgs(this.downButton,2,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
								}
							}
						}
						if(!this.treeViewState[0x1000]) {
							this.OnMouseUp(new MouseEventArgs(this.downButton,1,NativeMethods.Util.SignedLOWORD(m.LParam),NativeMethods.Util.SignedHIWORD(m.LParam),0));
						}
						this.treeViewState[0x800] = false;
						this.treeViewState[0x1000] = false;
						base.CaptureInternal = false;
						this.hNodeMouseDown = IntPtr.Zero;
						return;
					}
				case 0x203:
					this.WmMouseDown(ref m,MouseButtons.Left,2);
					this.treeViewState[0x800] = true;
					this.treeViewState[0x1000] = false;
					base.CaptureInternal = true;
					return;

				case 0x204: {
						this.treeViewState[0x1000] = false;
						NativeMethods.TV_HITTESTINFO tv_hittestinfo3 = new NativeMethods.TV_HITTESTINFO();
						tv_hittestinfo3.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_hittestinfo3.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this,base.Handle),0x1111,0,tv_hittestinfo3);
						this.WmMouseDown(ref m,MouseButtons.Right,1);
						this.downButton = MouseButtons.Right;
						return;
					}
				case 0x206:
					this.WmMouseDown(ref m,MouseButtons.Right,2);
					this.treeViewState[0x800] = true;
					this.treeViewState[0x1000] = false;
					base.CaptureInternal = true;
					return;

				case 0x207:
					this.treeViewState[0x1000] = false;
					this.WmMouseDown(ref m,MouseButtons.Middle,1);
					this.downButton = MouseButtons.Middle;
					return;

				case 0x209:
					this.treeViewState[0x1000] = false;
					this.WmMouseDown(ref m,MouseButtons.Middle,2);
					return;

				case 0x2a3:
					this.prevHoveredNode = null;
					base.WndProc(ref m);
					return;

				case 0x114:
					base.WndProc(ref m);
					if(this.DrawMode == TreeViewDrawMode.OwnerDrawAll) {
						base.Invalidate();
					}
					return;

				case 0x113f:
				case 0x110d:
					base.WndProc(ref m);
					if(this.CheckBoxes) {
						NativeMethods.TV_ITEM tv_item = (NativeMethods.TV_ITEM)m.GetLParam(typeof(NativeMethods.TV_ITEM));
						if(!(tv_item.hItem != IntPtr.Zero)) {
							return;
						}
						NativeMethods.TV_ITEM tv_item2 = new NativeMethods.TV_ITEM();
						tv_item2.mask = 0x18;
						tv_item2.hItem = tv_item.hItem;
						tv_item2.stateMask = 0xf000;
						UnsafeNativeMethods.SendMessage(new HandleRef(null,base.Handle),NativeMethods.TVM_GETITEM,0,ref tv_item2);
						this.NodeFromHandle(tv_item.hItem).CheckedStateInternal = (tv_item2.state >> 12) > 1;
					}
					return;

				case 0x204e:
					this.WmNotify(ref m);
					return;

				case 0x317:
					this.WmPrint(ref m);
					return;
			}
			base.WndProc(ref m);
		}

		public override Gdi::Color BackColor {
			get {
				if(this.ShouldSerializeBackColor()) {
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set {
				base.BackColor = value;
				if(base.IsHandleCreated) {
					base.SendMessage(0x111d,0,ColorTranslator.ToWin32(this.BackColor));
					base.SendMessage(0x1107,this.Indent,0);
				}
			}
		}

		[CM::Browsable(false),CM::EditorBrowsable(EditorBrowsableState.Never)]
		public override Gdi::Image BackgroundImage {
			get {
				return base.BackgroundImage;
			}
			set {
				base.BackgroundImage = value;
			}
		}

		[CM::Browsable(false),CM::EditorBrowsable(EditorBrowsableState.Never)]
		public override Frms::ImageLayout BackgroundImageLayout {
			get {
				return base.BackgroundImageLayout;
			}
			set {
				base.BackgroundImageLayout = value;
			}
		}

		//[SRDescription("borderStyleDescr"),SRCategory("CatAppearance")]
		[Interop::DispId(-504),CM::DefaultValue(2)]
		public Frms::BorderStyle BorderStyle {
			get {
				return this.borderStyle;
			}
			set {
				if(this.borderStyle != value) {
					if(!ClientUtils.IsEnumValid(value,(int)value,0,2)) {
						throw new InvalidEnumArgumentException("value",(int)value,typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		[CM::DefaultValue(false)]
		//[SRCategory("CatAppearance"),SRDescription("TreeViewCheckBoxesDescr")]
		public bool CheckBoxes {
			get {
				return this.treeViewState[8];
			}
			set {
				if(this.CheckBoxes != value) {
					this.treeViewState[8] = value;
					if(base.IsHandleCreated) {
						if(this.CheckBoxes) {
							base.UpdateStyles();
						} else {
							this.UpdateCheckedState(this.root,false);
							base.RecreateHandle();
						}
					}
				}
			}
		}

		protected override Frms::CreateParams CreateParams {
			[SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.UnmanagedCode)]
			get {
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysTreeView32";
				if(base.IsHandleCreated) {
					int windowLong = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this,base.Handle),-16));
					createParams.Style |= windowLong & 0x300000;
				}
				switch(this.borderStyle) {
					case BorderStyle.FixedSingle:
						createParams.Style |= 0x800000;
						break;

					case BorderStyle.Fixed3D:
						createParams.ExStyle |= 0x200;
						break;
				}
				if(!this.Scrollable) {
					createParams.Style |= 0x2000;
				}
				if(!this.HideSelection) {
					createParams.Style |= 0x20;
				}
				if(this.LabelEdit) {
					createParams.Style |= 8;
				}
				if(this.ShowLines) {
					createParams.Style |= 2;
				}
				if(this.ShowPlusMinus) {
					createParams.Style |= 1;
				}
				if(this.ShowRootLines) {
					createParams.Style |= 4;
				}
				if(this.HotTracking) {
					createParams.Style |= 0x200;
				}
				if(this.FullRowSelect) {
					createParams.Style |= 0x1000;
				}
				if(this.setOddHeight) {
					createParams.Style |= 0x4000;
				}
				if((this.ShowNodeToolTips && base.IsHandleCreated) && !base.DesignMode) {
					createParams.Style |= 0x800;
				}
				if(this.CheckBoxes && base.IsHandleCreated) {
					createParams.Style |= 0x100;
				}
				if(this.RightToLeft == RightToLeft.Yes) {
					if(this.RightToLeftLayout) {
						createParams.ExStyle |= 0x400000;
						createParams.ExStyle &= -28673;
						return createParams;
					}
					createParams.Style |= 0x40;
				}
				return createParams;
			}
		}

		protected override Gdi::Size DefaultSize {
			get {
				return new Gdi::Size(0x79,0x61);
			}
		}

		[CM::EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered {
			get {
				return base.DoubleBuffered;
			}
			set {
				base.DoubleBuffered = value;
			}
		}

		[CM::DefaultValue(0)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewDrawModeDescr")]
		public Frms::TreeViewDrawMode DrawMode {
			get {
				return this.drawMode;
			}
			set {
				if(!ClientUtils.IsEnumValid(value,(int)value,0,2)) {
					throw new CM::InvalidEnumArgumentException("value",(int)value,typeof(TreeViewDrawMode));
				}
				if(this.drawMode != value) {
					this.drawMode = value;
					base.Invalidate();
					if(this.DrawMode == Frms::TreeViewDrawMode.OwnerDrawAll) {
						base.SetStyle(Frms::ControlStyles.ResizeRedraw,true);
					}
				}
			}
		}

		public override Gdi::Color ForeColor {
			get {
				if(this.ShouldSerializeForeColor()) {
					return base.ForeColor;
				}
				return Gdi::SystemColors.WindowText;
			}
			set {
				base.ForeColor = value;
				if(base.IsHandleCreated) {
					base.SendMessage(0x111e,0,Gdi::ColorTranslator.ToWin32(this.ForeColor));
				}
			}
		}

		[CM::DefaultValue(false)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewFullRowSelectDescr")]
		public bool FullRowSelect {
			get {
				return this.treeViewState[0x200];
			}
			set {
				if(this.FullRowSelect != value) {
					this.treeViewState[0x200] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		[CM::DefaultValue(true)]
		//[SRDescription("TreeViewHideSelectionDescr"),SRCategory("CatBehavior")]
		public bool HideSelection {
			get {
				return this.treeViewState[1];
			}
			set {
				if(this.HideSelection != value) {
					this.treeViewState[1] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		[CM::DefaultValue(false)]
		//[SRDescription("TreeViewHotTrackingDescr"),SRCategory("CatBehavior")]
		public bool HotTracking {
			get {
				return this.treeViewState[0x100];
			}
			set {
				if(this.HotTracking != value) {
					this.treeViewState[0x100] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		[CM::Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",typeof(UITypeEditor))]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewImageIndexDescr")]
		[CM::RefreshProperties(CM::RefreshProperties.Repaint)]
		[CM::TypeConverter(typeof(NoneExcludedImageIndexConverter)),CM::DefaultValue(-1)]
		[Frms::RelatedImageList("ImageList"),CM::Localizable(true)]
		public int ImageIndex {
			get {
				if(this.imageList == null) {
					return -1;
				}
				if(this.ImageIndexer.Index >= this.imageList.Images.Count) {
					return Math.Max(0,this.imageList.Images.Count - 1);
				}
				return this.ImageIndexer.Index;
			}
			set {
				if(value == -1) {
					value = 0;
				}
				if(value < 0) {
					object[] args = new object[] { "ImageIndex",value.ToString(CultureInfo.CurrentCulture),0.ToString(CultureInfo.CurrentCulture) };
					throw new ArgumentOutOfRangeException("ImageIndex",SR.GetString("InvalidLowBoundArgumentEx",args));
				}
				if(this.ImageIndexer.Index != value) {
					this.ImageIndexer.Index = value;
					if(base.IsHandleCreated) {
						base.RecreateHandle();
					}
				}
			}
		}

		internal Frms::ImageList.Indexer ImageIndexer {
			get {
				if(this.imageIndexer == null) {
					this.imageIndexer = new Frms::ImageList.Indexer();
				}
				this.imageIndexer.ImageList = this.ImageList;
				return this.imageIndexer;
			}
		}

		//[SRDescription("TreeViewImageKeyDescr"),SRCategory("CatBehavior")]
		[CM::Localizable(true),CM::TypeConverter(typeof(ImageKeyConverter))]
		[CM::Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",typeof(UITypeEditor))]
		[CM::DefaultValue(""),CM::RefreshProperties(RefreshProperties.Repaint),Frms::RelatedImageList("ImageList")]
		public string ImageKey {
			get {
				return this.ImageIndexer.Key;
			}
			set {
				if(this.ImageIndexer.Key != value) {
					this.ImageIndexer.Key = value;
					if(string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone"))) {
						this.ImageIndex = (this.ImageList != null) ? 0 : -1;
					}
					if(base.IsHandleCreated) {
						base.RecreateHandle();
					}
				}
			}
		}

		//[SRCategory("CatBehavior"),SRDescription("TreeViewImageListDescr")]
		[CM::RefreshProperties(RefreshProperties.Repaint),CM::DefaultValue((string)null)]
		public Frms::ImageList ImageList {
			get {
				return this.imageList;
			}
			set {
				if(value != this.imageList) {
					EventHandler handler = new EventHandler(this.ImageListRecreateHandle);
					EventHandler handler2 = new EventHandler(this.DetachImageList);
					EventHandler handler3 = new EventHandler(this.ImageListChangedHandle);
					if(this.imageList != null) {
						this.imageList.RecreateHandle -= handler;
						this.imageList.Disposed -= handler2;
						this.imageList.ChangeHandle -= handler3;
					}
					this.imageList = value;
					if(value != null) {
						value.RecreateHandle += handler;
						value.Disposed += handler2;
						value.ChangeHandle += handler3;
					}
					if(base.IsHandleCreated) {
						base.SendMessage(0x1109,0,(value == null) ? IntPtr.Zero : value.Handle);
						if((this.StateImageList != null) && (this.StateImageList.Images.Count > 0)) {
							base.SendMessage(0x1109,2,this.internalStateImageList.Handle);
						}
					}
					this.UpdateCheckedState(this.root,true);
				}
			}
		}

		//[SRCategory("CatBehavior"),SRDescription("TreeViewIndentDescr")]
		[CM::Localizable(true)]
		public int Indent {
			get {
				if(this.indent != -1) {
					return this.indent;
				}
				if(base.IsHandleCreated) {
					return (int)base.SendMessage(0x1106,0,0);
				}
				return 0x13;
			}
			set {
				if(this.indent != value) {
					if(value < 0) {
						object[] args = new object[] { "Indent",value.ToString(CultureInfo.CurrentCulture),0.ToString(CultureInfo.CurrentCulture) };
						throw new ArgumentOutOfRangeException("Indent",SR.GetString("InvalidLowBoundArgumentEx",args));
					}
					if(value > MaxIndent) {
						throw new ArgumentOutOfRangeException("Indent",SR.GetString("InvalidHighBoundArgumentEx",new object[] { "Indent",value.ToString(CultureInfo.CurrentCulture),MaxIndent.ToString(CultureInfo.CurrentCulture) }));
					}
					this.indent = value;
					if(base.IsHandleCreated) {
						base.SendMessage(0x1107,value,0);
						this.indent = (int)base.SendMessage(0x1106,0,0);
					}
				}
			}
		}

		//[SRCategory("CatAppearance"),SRDescription("TreeViewItemHeightDescr")]
		public int ItemHeight {
			get {
				if(this.itemHeight != -1) {
					return this.itemHeight;
				}
				if(base.IsHandleCreated) {
					return (int)base.SendMessage(0x111c,0,0);
				}
				if(this.CheckBoxes && (this.DrawMode == TreeViewDrawMode.OwnerDrawAll)) {
					return Math.Max(0x10,base.FontHeight + 3);
				}
				return (base.FontHeight + 3);
			}
			set {
				if(this.itemHeight != value) {
					if(value < 1) {
						object[] args = new object[] { "ItemHeight",value.ToString(CultureInfo.CurrentCulture),1.ToString(CultureInfo.CurrentCulture) };
						throw new ArgumentOutOfRangeException("ItemHeight",SR.GetString("InvalidLowBoundArgumentEx",args));
					}
					if(value >= 0x7fff) {
						object[] objArray2 = new object[] { "ItemHeight",value.ToString(CultureInfo.CurrentCulture),((short)0x7fff).ToString(CultureInfo.CurrentCulture) };
						throw new ArgumentOutOfRangeException("ItemHeight",SR.GetString("InvalidHighBoundArgument",objArray2));
					}
					this.itemHeight = value;
					if(base.IsHandleCreated) {
						if((this.itemHeight % 2) != 0) {
							this.setOddHeight = true;
							try {
								base.RecreateHandle();
							} finally {
								this.setOddHeight = false;
							}
						}
						base.SendMessage(0x111b,value,0);
						this.itemHeight = (int)base.SendMessage(0x111c,0,0);
					}
				}
			}
		}

		//[SRCategory("CatBehavior"),SRDescription("TreeViewLabelEditDescr")]
		[CM::DefaultValue(false)]
		public bool LabelEdit {
			get {
				return this.treeViewState[2];
			}
			set {
				if(this.LabelEdit != value) {
					this.treeViewState[2] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		//[SRDescription("TreeViewLineColorDescr"),SRCategory("CatBehavior")]
		[CM::DefaultValue(typeof(Gdi::Color),"Black")]
		public Gdi::Color LineColor {
			get {
				if(base.IsHandleCreated) {
					int num = (int)((long)base.SendMessage(0x1129,0,0));
					return ColorTranslator.FromWin32(num);
				}
				return this.lineColor;
			}
			set {
				if(this.lineColor != value) {
					this.lineColor = value;
					if(base.IsHandleCreated) {
						base.SendMessage(0x1128,0,ColorTranslator.ToWin32(this.lineColor));
					}
				}
			}
		}

		//[SRCategory("CatBehavior"),SRDescription("TreeViewNodesDescr")]
		[CM::Localizable(true),CM::MergableProperty(false),CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Content)]
		public Frms::TreeNodeCollection Nodes {
			get {
				if(this.nodes == null) {
					this.nodes = new TreeNodeCollection(this.root);
				}
				return this.nodes;
			}
		}

		[CM::DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),CM::Browsable(false),CM::EditorBrowsable(CM::EditorBrowsableState.Never)]
		public Frms::Padding Padding {
			get {
				return base.Padding;
			}
			set {
				base.Padding = value;
			}
		}

		[CM::DefaultValue(@"\")]
		//[SRDescription("TreeViewPathSeparatorDescr"),SRCategory("CatBehavior")]
		public string PathSeparator {
			get {
				return this.pathSeparator;
			}
			set {
				this.pathSeparator = value;
			}
		}

		[CM::Localizable(true),CM::DefaultValue(false)]
		//[SRDescription("ControlRightToLeftLayoutDescr"),SRCategory("CatAppearance")]
		public virtual bool RightToLeftLayout {
			get {
				return this.rightToLeftLayout;
			}
			set {
				if(value != this.rightToLeftLayout) {
					this.rightToLeftLayout = value;
					using(LayoutTransaction transaction = new LayoutTransaction(this,this,PropertyNames.RightToLeftLayout)) {
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		[CM::DefaultValue(true)]
		//[SRDescription("TreeViewScrollableDescr"),SRCategory("CatBehavior")]
		public bool Scrollable {
			get {
				return this.treeViewState[4];
			}
			set {
				if(this.Scrollable != value) {
					this.treeViewState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		[CM::Localizable(true),CM::TypeConverter(typeof(NoneExcludedImageIndexConverter)),CM::DefaultValue(-1),Frms::RelatedImageList("ImageList")]
		[CM::Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",typeof(UITypeEditor))]
		//[SRDescription("TreeViewSelectedImageIndexDescr"),SRCategory("CatBehavior")]
		public int SelectedImageIndex {
			get {
				if(this.imageList == null) {
					return -1;
				}
				if(this.SelectedImageIndexer.Index >= this.imageList.Images.Count) {
					return Math.Max(0,this.imageList.Images.Count - 1);
				}
				return this.SelectedImageIndexer.Index;
			}
			set {
				if(value == -1) {
					value = 0;
				}
				if(value < 0) {
					object[] args = new object[] { "SelectedImageIndex",value.ToString(CultureInfo.CurrentCulture),0.ToString(CultureInfo.CurrentCulture) };
					throw new ArgumentOutOfRangeException("SelectedImageIndex",SR.GetString("InvalidLowBoundArgumentEx",args));
				}
				if(this.SelectedImageIndexer.Index != value) {
					this.SelectedImageIndexer.Index = value;
					if(base.IsHandleCreated) {
						base.RecreateHandle();
					}
				}
			}
		}

		internal Frms::ImageList.Indexer SelectedImageIndexer {
			get {
				if(this.selectedImageIndexer == null) {
					this.selectedImageIndexer = new ImageList.Indexer();
				}
				this.selectedImageIndexer.ImageList = this.ImageList;
				return this.selectedImageIndexer;
			}
		}

		[CM::Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",typeof(UITypeEditor))]
		[CM::RefreshProperties(RefreshProperties.Repaint),Frms::RelatedImageList("ImageList"),CM::Localizable(true),CM::TypeConverter(typeof(ImageKeyConverter)),CM::DefaultValue("")]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewSelectedImageKeyDescr")]
		public string SelectedImageKey {
			get {
				return this.SelectedImageIndexer.Key;
			}
			set {
				if(this.SelectedImageIndexer.Key != value) {
					this.SelectedImageIndexer.Key = value;
					if(string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone"))) {
						this.SelectedImageIndex = (this.ImageList != null) ? 0 : -1;
					}
					if(base.IsHandleCreated) {
						base.RecreateHandle();
					}
				}
			}
		}

		//[SRDescription("TreeViewSelectedNodeDescr"),SRCategory("CatAppearance")]
		[CM::Browsable(false),CM::DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Frms::TreeNode SelectedNode {
			get {
				if(base.IsHandleCreated) {
					System.IntPtr handle = base.SendMessage(0x110a,9,0);
					if(handle == IntPtr.Zero) {
						return null;
					}
					return this.NodeFromHandle(handle);
				}
				if((this.selectedNode != null) && (this.selectedNode.TreeView == this)) {
					return this.selectedNode;
				}
				return null;
			}
			set {
				if(base.IsHandleCreated && ((value == null) || (value.TreeView == this))) {
					System.IntPtr lparam = (value == null) ? System.IntPtr.Zero : value.Handle;
					base.SendMessage(0x110b,9,lparam);
					this.selectedNode = null;
				} else {
					this.selectedNode = value;
				}
			}
		}

		[CM::DefaultValue(true)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewShowLinesDescr")]
		public bool ShowLines {
			get {
				return this.treeViewState[0x10];
			}
			set {
				if(this.ShowLines != value) {
					this.treeViewState[0x10] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		[CM::DefaultValue(false)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewShowShowNodeToolTipsDescr")]
		public bool ShowNodeToolTips {
			get {
				return this.treeViewState[0x400];
			}
			set {
				if(this.ShowNodeToolTips != value) {
					this.treeViewState[0x400] = value;
					if(this.ShowNodeToolTips) {
						base.RecreateHandle();
					}
				}
			}
		}

		[CM::DefaultValue(true)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewShowPlusMinusDescr")]
		public bool ShowPlusMinus {
			get {
				return this.treeViewState[0x20];
			}
			set {
				if(this.ShowPlusMinus != value) {
					this.treeViewState[0x20] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		[CM::DefaultValue(true)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewShowRootLinesDescr")]
		public bool ShowRootLines {
			get {
				return this.treeViewState[0x40];
			}
			set {
				if(this.ShowRootLines != value) {
					this.treeViewState[0x40] = value;
					if(base.IsHandleCreated) {
						base.UpdateStyles();
					}
				}
			}
		}

		//[SRCategory("CatBehavior"),SRDescription("TreeViewSortedDescr")]
		[CM::EditorBrowsable(EditorBrowsableState.Never),CM::DefaultValue(false),CM::Browsable(false)]
		public bool Sorted {
			get {
				return this.treeViewState[0x80];
			}
			set {
				if(this.Sorted != value) {
					this.treeViewState[0x80] = value;
					if((this.Sorted && (this.TreeViewNodeSorter == null)) && (this.Nodes.Count >= 1)) {
						this.RefreshNodes();
					}
				}
			}
		}

		[CM::DefaultValue((string)null)]
		//[SRCategory("CatBehavior"),SRDescription("TreeViewStateImageListDescr")]
		public Frms::ImageList StateImageList {
			get {
				return this.stateImageList;
			}
			set {
				if(value != this.stateImageList) {
					System.EventHandler handler = new System.EventHandler(this.StateImageListRecreateHandle);
					System.EventHandler handler2 = new System.EventHandler(this.DetachStateImageList);
					System.EventHandler handler3 = new System.EventHandler(this.StateImageListChangedHandle);
					if(this.stateImageList != null) {
						this.stateImageList.RecreateHandle -= handler;
						this.stateImageList.Disposed -= handler2;
						this.stateImageList.ChangeHandle -= handler3;
					}
					this.stateImageList = value;
					if((this.stateImageList != null) && (this.stateImageList.Images.Count > 0)) {
						Image[] images = new Image[this.stateImageList.Images.Count + 1];
						images[0] = this.stateImageList.Images[0];
						for(int i = 1;i <= this.stateImageList.Images.Count;i++) {
							images[i] = this.stateImageList.Images[i - 1];
						}
						this.internalStateImageList = new ImageList();
						this.internalStateImageList.Images.AddRange(images);
					}
					if(value != null) {
						value.RecreateHandle += handler;
						value.Disposed += handler2;
						value.ChangeHandle += handler3;
					}
					if(base.IsHandleCreated) {
						if(((this.stateImageList != null) && (this.stateImageList.Images.Count > 0)) && (this.internalStateImageList != null)) {
							base.SendMessage(0x1109,2,this.internalStateImageList.Handle);
						}
						this.UpdateCheckedState(this.root,true);
						if(((value == null) || (this.stateImageList.Images.Count == 0)) && this.CheckBoxes) {
							base.RecreateHandle();
						} else {
							this.RefreshNodes();
						}
					}
				}
			}
		}

		[CM::Browsable(false),CM::EditorBrowsable(EditorBrowsableState.Never),CM::Bindable(false)]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				base.Text = value;
			}
		}

		//[SRCategory("CatAppearance"),SRDescription("TreeViewTopNodeDescr")]
		[CM::Browsable(false),CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Hidden)]
		public Frms::TreeNode TopNode {
			get {
				if(!base.IsHandleCreated) {
					return this.topNode;
				}
				System.IntPtr handle = base.SendMessage(0x110a,5,0);
				if(!(handle == System.IntPtr.Zero)) {
					return this.NodeFromHandle(handle);
				}
				return null;
			}
			set {
				if(base.IsHandleCreated && ((value == null) || (value.TreeView == this))) {
					System.IntPtr lparam = (value == null) ? System.IntPtr.Zero : value.Handle;
					base.SendMessage(0x110b,5,lparam);
					this.topNode = null;
				} else {
					this.topNode = value;
				}
			}
		}

		//[SRDescription("TreeViewNodeSorterDescr"),SRCategory("CatBehavior")]
		[CM::Browsable(false),CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Hidden)]
		public System.Collections.IComparer TreeViewNodeSorter {
			get {
				return this.treeViewNodeSorter;
			}
			set {
				if(this.treeViewNodeSorter != value) {
					this.treeViewNodeSorter = value;
					if(value != null) {
						this.Sort();
					}
				}
			}
		}

		//[SRCategory("CatAppearance"),SRDescription("TreeViewVisibleCountDescr")]
		[CM::Browsable(false),CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Hidden)]
		public int VisibleCount {
			get {
				if(base.IsHandleCreated) {
					return (int)base.SendMessage(0x1110,0,0);
				}
				return 0;
			}
		}
	}
}