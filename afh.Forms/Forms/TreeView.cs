using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Gen=System.Collections.Generic;

namespace afh.Forms{
	/// <summary>
	/// �g���\�� TreeView �R���g���[����񋟂��܂��B
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.TreeView))]
	[CM::DefaultProperty("Nodes")]
	[CM::DefaultEvent("FocusedNodeChanged")]
	public class TreeView:System.Windows.Forms.ScrollableControl{
		/// <summary>
		/// �m�[�h���W����ێ����܂��B
		/// </summary>
		internal readonly TreeNode root;
		/// <summary>
		/// ���� View �ɓo�^����Ă���m�[�h�̏W�����w�肵�܂��B
		/// </summary>
		[CM::Category("Tree")]
		[CM::Description("���� View �ɓo�^����Ă���m�[�h�B���w�肵�܂��B")]
		public TreeNodeCollection Nodes{
			get{return this.root.Nodes;}
		}
		/// <summary>
		/// �m�[�h�̊�{�ݒ��ێ����܂��B
		/// </summary>
		private readonly TreeNodeSettings default_param=new TreeNodeSettings();
		/// <summary>
		/// ���� View �ɓo�^����Ă���m�[�h�̊�{�ݒ���Ǘ�����C���X�^���X���擾���܂��B
		/// </summary>
		[CM::TypeConverter(typeof(Design.TreeNodeSettingsConverter))]
		[CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Content)]
		[CM::Category("Tree")]
		[CM::Description("���� View �ɓo�^����Ă���m�[�h�̊�{�ݒ���w�肵�܂��B")]
		public TreeNodeSettings DefaultNodeParams{
			get{return this.default_param;}
		}

		private bool multiSelect=false;
		/// <summary>
		/// TreeNode �𕡐��I���\���ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		[CM::Category("Tree")]
		[CM::Description("TreeNode �𕡐��I���\���ۂ����w�肵�܂��B")]
		[CM::DefaultValue(false)]
		public bool MultiSelect{
			get{return this.multiSelect;}
			set{this.multiSelect=value;}
		}
		//============================================================
		//		�m�[�h���
		//============================================================
		private TreeNode focused_node=null;
		/// <summary>
		/// ���ݒ��ڂ��Ă���m�[�h���擾���܂��B
		/// </summary>
		[CM::Browsable(false)]
		public TreeNode FocusedNode{
			get{return this.focused_node;}
			internal set{
				if(this.focused_node==value)return;

				if(this.focused_node!=null){
					TreeNode old=this.focused_node;
					this.focused_node=null;

					old.OnLosingFocus();
					old.ReDraw(false);
				}

				this.focused_node=value;
				if(value!=null){
					value.OnAcquiredFocus();
					value.ReDraw(false);
					value.EnsureVisible();
				}

				this.OnFocusedNodeChanged();
			}
		}
		private readonly SelectedNodeCollection selected;
		/// <summary>
		/// ���ݑI������Ă���m�[�h�̏W�����擾���܂��B
		/// </summary>
		[CM::Browsable(false)]
		public SelectedNodeCollection SelectedNodes{
			get{
				//OK:_todo.EssentialTreeView("�I��v�f�R���N�V�������ύX���ꂽ�������m���ăC�x���g�𔭂���");
				return this.selected;
			}
		}
		/// <summary>
		/// �w�肵�� TreeNode �����݊��������Ă��邩�ۂ��𔻒肵�܂��B
		/// </summary>
		/// <param name="node">���������Ă��邩�m�F���� TreeNode ���w�肵�܂��B</param>
		/// <returns>node ��������������Ԃɂ��鎞�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		internal bool IsActivated(TreeNode node){
			if(this.dragging)
				return IsDraggedNode(node);
			if(this.mouseActivatedNode!=null)
				return node==this.mouseActivatedNode;
			return node.IsSelected;
		}
		/// <summary>
		/// ���ݑI������Ă���m�[�h�̏W���ɕύX�����������ɔ������܂��B
		/// </summary>
		[CM::Category("Tree")]
		public event afh.CallBack<TreeView> SelectionChanged;
		internal void OnSelectionChanged(){
			if(this.SelectionChanged==null)return;
			this.SelectionChanged(this);
		}
		/// <summary>
		/// ���݃t�H�[�J�X�������Ă���m�[�h���ύX���ꂽ���ɔ������܂��B
		/// </summary>
		[CM::Category("Tree")]
		public event afh.CallBack<TreeView> FocusedNodeChanged;
		internal void OnFocusedNodeChanged(){
			if(this.FocusedNodeChanged==null)return;
			this.FocusedNodeChanged(this);
		}
		//============================================================
		//		������
		//============================================================
		/// <summary>
		/// TreeView �̃C���X�^���X���쐬���A���������܂��B
		/// </summary>
		public TreeView(){
			this.selected=new SelectedNodeCollection(this);
			this.root=new TreeNode();
			this.dropArea=new afh.Forms.Design.ReversibleArea(this);
			this.InitializeComponents();

			if(this.DesignMode){
				this.BackColor=Gdi::Color.AliceBlue;

				this.DefaultNodeParams.IconVisible=true;
				this.root.CheckBoxVisible=true;
				this.DefaultNodeParams.CheckBox=TreeNodeCheckBox.YUI;
				this.DefaultNodeParams.Icon=TreeNodeIcon.Folder;

				// For ScrollBars
				TreeNode node=new TreeNode();
				this.root.Nodes.Add(new TreeNode());
				this.root.Nodes.Add(new TreeNode());
				this.root.Nodes.Add(new TreeNode());
				this.root.Nodes[0].Nodes.Add(node);
				this.root.Nodes[0].Nodes.Add(new TreeNode());

				// For ScrollBar Tests
				//this.AutoScrollMinSize=new Gdi::Size(300,300);
			}
		}
		private void InitializeComponents(){
			//
			// root
			//
			this.root.View=this;
			this.root.NodeHeight=0;
			this.root.IsExpanded=true;
			this.root.DisplaySizeChanged+=new afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<System.Drawing.Size>>(root_DisplaySizeChanged);
			this.root.CheckBoxVisibleInherit=TreeNodeInheritType.Default;
			this.root.IsEnabledInherit=TreeNodeInheritType.Default;
			//
			// default_param
			//
			afh.VoidCB refresh=delegate{this.Refresh();};
			afh.VoidCB recalc_size=delegate{this.root.RefreshDisplaySizeAll();};
//			this.default_param.BackColorChanged			+=default_param_BackColorChanged;
			this.default_param.ForeColorChanged			+=refresh;
			this.default_param.CheckBoxChanged			+=refresh;
			this.default_param.CheckBoxVisibleChanged	+=recalc_size;
			this.default_param.IconChanged				+=refresh;
			this.default_param.IconSizeChanged			+=recalc_size;
			this.default_param.IconVisibleChanged		+=recalc_size;
//			this.default_param.ChildIndentWidthChanged	+=recalc_size;
			this.default_param.ChildIndentChanged		+=recalc_size;
			this.default_param.NodeHeightChanged		+=recalc_size;
			//
			// this
			//
			this.SetStyle(Forms::ControlStyles.UserMouse,true);
			this.AutoScroll=true;
			this.VerticalScroll.SmallChange=14;
			this.HorizontalScroll.SmallChange=14;
			this.BackColor=Gdi::SystemColors.Window;
//			this.BackColorChanged+=new System.EventHandler(TreeView_BackColorChanged);
//			this.VScroll=true;
//			this.HScroll=true;
#if OLD_TEST
			node=this.root.Nodes[1];
			for(int i=0;i<100;i++){
				TreeNode node2=new TreeNode();
				node.Nodes.Add(node2);
				for(int j=0;j<100;j++){
					node2.Nodes.Add(new TreeNode());
					//node2.Nodes.LastNode.BackColor=Gdi::Color.Azure;
				}
			}
			//*/

			node=new TreeNode();
			//node=this.root.Nodes[2];
			for(int i=0;i<=1000;i++){
				node.Nodes.Add(new TreeNode());
			}
			this.root.Nodes.Add(node);

			this.root.Nodes[0].IsChecked=true;
			this.root.Nodes[0].IsExpanded=true;
			this.root.Nodes[1].IsChecked=null;
			this.root.Nodes[2].IsChecked=false;
			//*/
#endif
		}

		void root_DisplaySizeChanged(TreeNode sender,TreeNodePropertyChangingEventArgs<System.Drawing.Size> args){
			Gdi::Size sz=args.NewValue;
			sz.Height+=5; // �����]�T (�������Ȃ��������o�I�ɕ�����)
			this.AutoScrollMinSize=sz;
		}

		void default_param_BackColorChanged(){
			this.BackColor=this.default_param.BackColor;
			this.Refresh();
		}
		void TreeView_BackColorChanged(object sender,System.EventArgs e){
			this.default_param.BackColor=this.BackColor;
		}
		/// <summary>
		/// �R���g���[�� �n���h�����쐬�����Ƃ��ɕK�v�ȍ쐬�p�����[�^���擾���܂��B
		/// </summary>
		protected override System.Windows.Forms.CreateParams CreateParams{
			get{
				const int WS_EX_CLIENTEDGE=0x00000200;

				Forms::CreateParams cp=base.CreateParams;
				cp.ExStyle|=WS_EX_CLIENTEDGE;
				return cp;
			}
		}
		//============================================================
		//		�`��
		//============================================================
		/// <summary>
		/// Paint �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� PaintEventArgs�B</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e){
			base.OnPaint(e);
			this.DrawNodes(e);
		}
		/// <summary>
		/// �m�[�h��`�悵�܂��B
		/// </summary>
		protected internal void DrawNodes(){
			using(Gdi::Graphics g=this.CreateGraphics()){
				g.SetClip(this.ClientRectangle);
				this.DrawNodes(g);
			}
		}
		/// <summary>
		/// �m�[�h���w�肵�� PaintEventArgs ���g�p���ĕ`�悵�܂��B
		/// </summary>
		/// <param name="e">�`��Ɋւ������񋟂���I�u�W�F�N�g���w�肵�܂��B</param>
		protected void DrawNodes(Forms::PaintEventArgs e){
			e.Graphics.SetClip((Gdi::RectangleF)e.ClipRectangle);
			this.DrawNodes(e.Graphics);
		}

		private void DrawNodes(Gdi::Graphics g){
			g.TranslateTransform(this.AutoScrollPosition.X,this.AutoScrollPosition.Y);
			g.TranslateTransform(this.root.ChildIndent.Width,0);
			this.root.DrawChildren(g);
		}

		//============================================================
		//		�L�[�{�[�h�C�x���g
		//============================================================
		/// <summary>
		/// �R�}���h �L�[���������܂��B
		/// </summary>
		/// <param name="msg">��������E�B���h�E ���b�Z�[�W��\���A�Q�Ɠn�����ꂽ Message�B</param>
		/// <param name="keyData">��������L�[��\�� Keys �l�� 1 �B</param>
		/// <returns>�������R���g���[���ɂ���ď������ꂽ�ꍇ�� true�B����ȊO�̏ꍇ�� false�B</returns>
		protected override bool ProcessCmdKey(ref Forms::Message msg,Forms::Keys keyData){
			TreeNode node;
			switch(keyData&~Forms::Keys.Modifiers){
				case System.Windows.Forms.Keys.Down:
					if(this.FocusedNode==null)break;

					node=this.FocusedNode.NextVisibleInTree;
					goto move_to;
				case System.Windows.Forms.Keys.Up:
					if(this.FocusedNode==null)break;

					node=this.FocusedNode.PreviousVisibleInTree;
					goto move_to;
				case System.Windows.Forms.Keys.Left:
					node=FocusedNode;
					if(node==null)break;

					if(node.HasChildren&&node.IsExpanded){
						node.IsExpanded=false;
						break;
					}

					node=node.ParentNode;
					goto move_to;
				case System.Windows.Forms.Keys.Right:
					node=FocusedNode;
					if(node==null||!node.HasChildren)break;

					if(!node.IsExpanded){
						node.IsExpanded=true;
						break;
					}

					node=node.Nodes.FirstNode;
					goto move_to;
				case System.Windows.Forms.Keys.Home:
					node=this.Nodes.FirstNode;
					goto move_to;
				case System.Windows.Forms.Keys.End:
					node=this.Nodes.LastNode;
					while(true){
						if(node==null){
							afh.DebugUtils.Assert(this.Nodes.Count==0);
							break;
						}else if(!node.IsVisible){
							node=node.PreviousSibling;
						}else if(node.IsExpanded&&node.HasChildren){
							node=node.Nodes.LastNode;
						}else{
							break;
						}
					}

					goto move_to;
				move_to:
					if(node==null||node==root)break;
					this.SelectNode(node,keyData&Forms::Keys.Modifiers);
					this.FocusedNode=node;
					return true;
				default:
					return base.ProcessCmdKey(ref msg,keyData);
			}
			return true;
		}

		#region events: Drag and Drop
		//============================================================
		//		�h���b�O�h���b�v�C�x���g
		//============================================================
		//	Dragged Items
		//------------------------------------------------------------
		private TreeNode dragTargetNode=null;
		private bool dragging=false;
		/// <summary>
		/// �w�肵�� TreeNode �����݃h���b�O����Ă��� TreeNode ���ۂ����擾���܂��B
		/// </summary>
		/// <param name="node">�h���b�O����Ă��邩�ۂ��𔻒肷�� TreeNode ���w�肵�܂��B</param>
		/// <returns>
		/// �w�肵�� TreeNode �����݃h���b�O����Ă��� TreeNode �̏ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B
		/// </returns>
		private bool IsDraggedNode(TreeNode node){
			if(!dragging)return false;
			
			if(dragTargetNode!=null)
				return dragTargetNode==node;

			return node.IsSelected;
		}
		private void DragStart(TreeNode key){
			if(this.dragging)return;

			if(this.InvokeRequired){
				this.Invoke((System.Action<TreeNode>)this.DragStart,key);
				return;
			}

			/*
			if(key.IsSelected)
				dragTargetNode=null;
			else{
				dragTargetNode=key;
				dragTargetNode.ReDraw(false);
				this.SelectedNodes.ReDraw();
			}
			dragging=true;

			this.DoDragDrop(key,Forms::DragDropEffects.All);

			dragging=false;
			if(dragTargetNode!=null){
				this.SelectedNodes.ReDraw();
				dragTargetNode.ReDraw(false);
				dragTargetNode=null;
			}
			//*/

			if(key.IsSelected&&this.selected.Count>1){
				Forms::DragDropEffects dde=Forms::DragDropEffects.All;
				Gen::List<TreeNode> draggedNodes=new Gen::List<TreeNode>();
				foreach(TreeNode node in this.selected){
					dde&=node.DDBehavior.GetAllowedDDE(node);
					draggedNodes.Add(node);
				}
				if(dde==Forms::DragDropEffects.None)return;

				dragTargetNode=null;
				dragging=true;

				this.DoDragDrop(
					new Forms::DataObject("afh.Forms.TreeNode:List",draggedNodes),
					dde);

				dragging=false;
			}else{
				Forms::DragDropEffects dde=key.DDBehavior.GetAllowedDDE(key);
				if(dde==Forms::DragDropEffects.None)return;

				dragTargetNode=key;
				dragging=true;
				dragTargetNode.ReDraw(false);
				this.SelectedNodes.ReDraw();

				//this.DoDragDrop(key,Forms::DragDropEffects.All);
				this.DoDragDrop(
					new Forms::DataObject("afh.Forms.TreeNode",key),
					key.DDBehavior.GetAllowedDDE(key));

				dragging=false;
				this.SelectedNodes.ReDraw();
				dragTargetNode.ReDraw(false);
				dragTargetNode=null;
			}
		}
		//------------------------------------------------------------
		//	Drop Target
		//------------------------------------------------------------
		//protected override void OnDragEnter(Forms::DragEventArgs e){
		//  base.OnDragEnter(e);
		//}
		/// <summary>
		/// DragOver �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� DragEventArgs�B</param>
		protected override void OnDragOver(Forms::DragEventArgs e){
			TreeNodeDragEventArgs tde=TreeNodeDragEventArgs.CreateFromDragEventArgs(this,e);
			this.OnDragOver_internal(tde);
			if(tde==null)
				e.Effect=System.Windows.Forms.DragDropEffects.None;
			else if(tde.Handled)return;

			base.OnDragOver(e);
		}
		private void OnDragOver_internal(TreeNodeDragEventArgs tde){
			if(tde==null){
				this.set_LastDragNode(null,null);
				return;
			}else{
				this.set_LastDragNode(tde.Node,tde);
			}

			tde.Node.DDBehavior.OnDrag(tde.Node,tde);
			//tde.Node.OnDragMove(this,tde);
		}
		/// <summary>
		/// DragLeave �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� EventArgs�B</param>
		protected override void OnDragLeave(System.EventArgs e){
			this.set_LastDragNode(null,null);
			base.OnDragLeave(e);
		}
		/// <summary>
		/// DragDrop �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� DragEventArgs�B</param>
		protected override void OnDragDrop(Forms::DragEventArgs e){
			TreeNodeDragEventArgs tde=TreeNodeDragEventArgs.CreateFromDragEventArgs(this,e);
			if(tde!=null){
				// check dragmove
				if(this.LastDragNode!=tde.Node){
					this.OnDragOver_internal(tde);
					tde.Handled=false;
				}

				// drop operation
				tde.Node.DDBehavior.OnDrop(tde.Node,tde);
				this.set_LastDragNode(null,null);
				if(tde.Handled)return;
			}

			this.set_LastDragNode(null,null);
			base.OnDragDrop(e);
		}
		/// <summary>
		/// GiveFeedback �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� GiveFeedbackEventArgs�B</param>
		protected override void OnGiveFeedback(Forms::GiveFeedbackEventArgs e){
			base.OnGiveFeedback(e);

			TreeNode dst=this.lastDragNode;
			if(dst!=null){
				Forms::Cursor c=dst.DDBehavior.GetCursor(dst,e.Effect);
				if(c!=null){
					e.UseDefaultCursors=false;
					Forms::Cursor.Current=c;
					return;
				}
			}
			e.UseDefaultCursors=true;
		}
		//-------------------------------------------------------------------------
		internal readonly Design.ReversibleArea dropArea;
		private TreeNode lastDragNode=null;
		private TreeNode LastDragNode{
			get{return this.lastDragNode;}
		}
		private void set_LastDragNode(TreeNode value,TreeNodeDragEventArgs e){
			if(this.lastDragNode==value)return;
			if(this.lastDragNode!=null){
				this.lastDragNode.DDBehavior.OnLeave(this.lastDragNode,e);
			}

			if(value!=null){
				value.DDBehavior.OnEnter(value,e);
			}
			this.lastDragNode=value;
		}
		#endregion

		#region events: Mouse
		//============================================================
		//		�}�E�X�C�x���g
		//============================================================
		/// <summary>
		/// ���݊��������Ă���m�[�h�������܂��B
		/// activatedNode �̓}�E�X�̃{�^������������Ă���ԁA���̉��ɂ����� TreeNode ���ݒ肳��܂��B
		/// </summary>
		private TreeNode mouseActivatedNode=null;
		/// <summary>
		/// ActivatedNode �� null �̏ꍇ�ɂ́A���ݑI������Ă���m�[�h�� IsActive �ɂȂ�܂��B
		/// ActivatedNode �� null �łȂ��ꍇ�ɂ́A���̃m�[�h�� IsActive �̏�ԂɂȂ�܂��B
		/// </summary>
		private TreeNode MouseActivatedNode{
			get{return this.mouseActivatedNode;}
			set{
				if(mouseActivatedNode==value)return;
				TreeNode oldval=mouseActivatedNode;
				mouseActivatedNode=value;

				if(oldval!=null)oldval.ReDraw(false);
				if(value!=null)value.ReDraw(false);

				// SelectedNodes �� IsActivated ���ω�
				if(oldval==null||value==null){
					this.SelectedNodes.ReDraw();
				}
			}
		}
		/// <summary>
		/// �}�E�X�̃{�^�����������ꂽ���_�ł́A���̉��ɂ����� TreeNode ��ێ����܂��B
		/// �}�E�X�|�C���^������ TreeNode �̊O�ɏo�����_�� null �ƂȂ�܂��B
		/// <remarks>
		/// null �ɂȂ�@��قȂ�̂ŁAmouseDownArgs �Ƃ͕ʁX�ɊǗ����܂��B
		/// </remarks>
		/// </summary>
		private TreeNode mouseDownNode=null;
		private TreeNodeMouseEventArgs mouseDownArgs=null;
		private TreeNode lastMouseNode=null;
		private TreeNode LastMouseNode{
			get{return this.lastMouseNode;}
			set{
				if(this.lastMouseNode==value)return;
				if(this.lastMouseNode!=null){
					this.MouseActivatedNode=null;
					if(mouseDownNode!=null)mouseDownNode=null;
					this.lastMouseNode.OnMouseLeave(this,new TreeNodeEventArgs(this.lastMouseNode));
				}

				if(value!=null){
					value.OnMouseEnter(this,new TreeNodeEventArgs(value));
				}
				this.lastMouseNode=value;
			}
		}
		/// <summary>
		/// �}�E�X���̈�����������̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseLeave(System.EventArgs e) {
			this.LastMouseNode=null;
			base.OnMouseLeave(e);
		}
		/// <summary>
		/// �}�E�X���̈�𓮂������̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e){
			try{
				TreeNodeMouseEventArgs tme=TreeNodeMouseEventArgs.CreateFromMouseEventArgs(this,e);
				if(tme==null){
					this.LastMouseNode=null;
					return;
				}else{
					this.LastMouseNode=tme.Node;
				}

				tme.Node.OnMouseMove(this,tme);
				if(tme.Handled)return;

				//_todo.EssentialTreeView();
			}finally{
				base.OnMouseMove(e);

				// Drag & Drop �J�n
				do{
					if(e.Button!=Forms::MouseButtons.Left)break;
					if(mouseDownArgs==null||mouseDownArgs.HitTypeOn!=TreeNodeHitType.OnContent)break;

					// �h���b�O�͈͂̊m�F
					Gdi::Size sz=Forms::SystemInformation.DragSize;
					int dx=mouseDownArgs.ClientX-e.X;
					if((dx<0?-dx:dx)<sz.Width/2)break;
					int dy=mouseDownArgs.ClientY-e.Y;
					if((dy<0?-dy:dy)<sz.Height/2)break;

					this.DragStart(mouseDownArgs.Node);
				}while(false);
			}
		}
		/// <summary>
		/// �}�E�X�̃{�^�����������ꂽ���̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e){
			try{
				TreeNodeMouseEventArgs tme=TreeNodeMouseEventArgs.CreateFromMouseEventArgs(this,e);
				if(tme==null)return;
				TreeNode node=tme.Node;
				if(e.Button==Forms::MouseButtons.Left){
					this.mouseDownNode=node;
					this.mouseDownArgs=tme;
				}

				node.OnMouseDown(this,tme);
				if(tme.Handled)return;

				switch(tme.HitTypeOn){
					case TreeNodeHitType.OnContent:
						if(e.Button==Forms::MouseButtons.Left||e.Button==Forms::MouseButtons.Right)
							if(node.IsEnabled)
								this.MouseActivatedNode=node;
						break;
					case TreeNodeHitType.OnPlusMinus:
						if(e.Button==Forms::MouseButtons.Left){
							node.IsExpanded=!node.IsExpanded;
						}
						break;
				}
			}finally{
				base.OnMouseDown(e);
			}
		}
		/// <summary>
		/// �}�E�X�̃{�^�����オ�������̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e){
			try{
				this.mouseDownArgs=null;

				TreeNodeMouseEventArgs tme=TreeNodeMouseEventArgs.CreateFromMouseEventArgs(this,e);
				if(tme==null)return;
				TreeNode node=tme.Node;
				this.MouseActivatedNode=null;

				node.OnMouseUp(this,tme);
				if(tme.Handled)return;
			}finally{
				base.OnMouseUp(e);
			}
		}
		/// <summary>
		/// �}�E�X���ȂăN���b�N���ׂ��ꂽ���̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e){
			try{
				TreeNodeMouseEventArgs tme=TreeNodeMouseEventArgs.CreateFromMouseEventArgs(this,e);
				if(tme==null)return;
				TreeNode node=tme.Node;
				if(node!=mouseDownNode)return;

				node.OnMouseClick(this,tme);
				if(tme.Handled)return;

				if(e.Button==System.Windows.Forms.MouseButtons.Left){
					switch(tme.HitTypeOn){
						case TreeNodeHitType.OnContent:
							if(!node.IsEnabled)break;
							//>_todo.EssentialTreeView("�I����Ԃ̕ύX�ɑ΂���C�x���g�����B����� class SelectedNodes ��������������������m��Ȃ��B");

							this.SelectNode(node,Forms::Control.ModifierKeys);
							this.FocusedNode=node;
							break;
						case TreeNodeHitType.OnCheckBox:
							if(node.CheckBoxEnabled)
								node.IsChecked=!(node.IsChecked??false);
							break;
					}
				}
			}finally{
				base.OnMouseClick(e);
			}
		}
		/// <summary>
		/// �}�E�X���Ȃă_�u���N���b�N���ׂ��ꂽ���̃C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g�Ɋ֘A��������w�肵�܂��B</param>
		protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e){
			try{
				TreeNodeMouseEventArgs tme=TreeNodeMouseEventArgs.CreateFromMouseEventArgs(this,e);
				if(tme==null)return;
				TreeNode node=tme.Node;
				if(node!=mouseDownNode)return;

				if(node.IsEnabled){
					node.OnMouseDoubleClick(this,tme);
					if(tme.Handled)return;
				}

				if(e.Button==System.Windows.Forms.MouseButtons.Left){
					switch(tme.HitTypeOn){
						case TreeNodeHitType.OnContent:
							if(!node.IsEnabled)break;
							if(node.HasChildren)
								node.IsExpanded=!node.IsExpanded;
							if(node.CheckBoxVisible&&node.CheckBoxEnabled)
								node.IsChecked=!(node.IsChecked??false);
							break;
						case TreeNodeHitType.OnCheckBox:
							if(node.CheckBoxVisible&&node.CheckBoxEnabled)
								node.IsChecked=!(node.IsChecked??false);
							break;
					}
				}
			}finally{
				base.OnMouseDoubleClick(e);
			}
		}
		/// <summary>
		/// MouseWheel �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� MouseEventArgs�B</param>
		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e){
			_todo.TreeNode("TreeNode ���ꂼ��ɑ΂��Ă� MouseWheel �𔭐�������?");

			int scroll=e.Delta
				*(this.HScroll?this.HorizontalScroll.SmallChange:this.VerticalScroll.SmallChange)
				*Forms::SystemInformation.MouseWheelScrollLines
				/Forms::SystemInformation.MouseWheelScrollDelta;
				
			base.OnMouseWheel(new Forms::MouseEventArgs(e.Button,e.Clicks,e.X,e.Y,scroll));
		}
		#endregion

		//============================================================
		//		�m�[�h�I��
		//============================================================
		/// <summary>
		/// ���� TreeView �ɑ������� TreeNode ���r���āA�ǂ���̕������ɕ\������邩���擾���܂��B
		/// </summary>
		/// <param name="node1">��r���� TreeNode ���w�肵�܂��B</param>
		/// <param name="node2">��r���� TreeNode ���w�肵�܂��B</param>
		/// <returns>
		/// node1 �� node2 ������ TreeNode �̏ꍇ�ɂ� 0 ��Ԃ��܂��B
		/// node1 �̕��� node2 ��艺�ɕ\������鎞�ɂ� 1 ��Ԃ��܂��B
		/// node2 �̕��� node1 ��艺�ɕ\������鎞�ɂ� -1 ��Ԃ��܂��B
		/// </returns>
		public static int ComparePosition(TreeNode node1,TreeNode node2){
			// exceptions
			if(node1.View==null)
				throw new TreeViewMissingException("���� node1 �ɐe View ���֘A�t�����Ă��܂���B");
			if(node2.View==null)
				throw new TreeViewMissingException("���� node2 �ɐe View ���֘A�t�����Ă��܂���B");
			if(node1.View!=node2.View)
				throw new TreeNodeException("��r�Ώۂ� TreeNode ���قȂ� TreeView �ɏ������Ă��܂��B");

			if(node1==node2)return 0;

			int l1=node1.Level;
			int l2=node2.Level;
			if(l1>l2){
				do{
					node1=node1.ParentNode;
					l1--;
				}while(l1>l2);

				if(node1==node2)return 1;
			}else if(l2>l1){
				do{
					node2=node2.ParentNode;
					l2--;
				}while(l2>l1);

				if(node1==node2)return -1;
			}

			return ComparePosition_sameLevel(node1,node2);
		}
		private static int ComparePosition_sameLevel(TreeNode node1,TreeNode node2){
			afh.DebugUtils.Assert(node1.Level==node2.Level);
			if(node1.ParentNode==node2.ParentNode)
				return node1.IndexInSiblings-node2.IndexInSiblings;
			else
				return ComparePosition_sameLevel(node1.ParentNode,node2.ParentNode);
		}
		private void SelectNode(TreeNode node,Forms::Keys modifier){
			const Forms::Keys MODIFIERS=Forms::Keys.Control|Forms::Keys.Shift;
			if(multiSelect)switch(modifier&MODIFIERS){
				case MODIFIERS:
					selected.SwitchRange(this.FocusedNode,node);
					return;
				case Forms::Keys.Shift:
					selected.SelectRange(this.FocusedNode,node);
					return;
				case Forms::Keys.Control:
					this.selected.Switch(node);
					return;
			}

			this.selected.Set(node);
		}
		//============================================================
		//		�m�[�h�ʒu�T��
		//============================================================
		/// <summary>
		/// �w�肵���ʒu�ɂ���m�[�h���擾���܂��B
		/// </summary>
		/// <param name="p">�擾�������m�[�h�̈ʒu���A�_���I�ȍ��W�Ŏw�肵�܂��B</param>
		/// <param name="node">�w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A���̃m�[�h��Ԃ��܂��B
		/// �m�[�h��������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</param>
		/// <param name="type">
		/// �w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A
		/// �w�肵�����W�����̃m�[�h�̒��ł��A�ǂ̈ʒu�ɑ������Ă��邩��Ԃ��܂��B
		/// �m�[�h��������Ȃ������ꍇ�ɂ́A(TreeNodeHitType)0 ��Ԃ��܂��B
		/// </param>
		/// <returns>��v����m�[�h�����������ۂ� true ��Ԃ��܂��A
		/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool LogicalPosition2Node(Gdi::Point p,out TreeNode node,out TreeNodeHitType type) {
			_todo.ExamineTreeView();
			node=this.root.LocalPosition2Node(p,out type);
			return node!=null;
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ���m�[�h���擾���܂��B
		/// </summary>
		/// <param name="p">�擾�������m�[�h�̈ʒu���A�_���I�ȍ��W�Ŏw�肵�܂��B</param>
		/// <param name="node">�w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A���̃m�[�h��Ԃ��܂��B
		/// �m�[�h��������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</param>
		/// <returns>��v����m�[�h�����������ۂ� true ��Ԃ��܂��A
		/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool LogicalPosition2Node(Gdi::Point p,out TreeNode node){
			TreeNodeHitType temp;
			return this.LogicalPosition2Node(p,out node,out temp);
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ���m�[�h���擾���܂��B
		/// </summary>
		/// <param name="pClient">�擾�������m�[�h�̈ʒu���A�N���C�A���g���W�Ŏw�肵�܂��B</param>
		/// <param name="node">�w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A���̃m�[�h��Ԃ��܂��B
		/// �m�[�h��������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</param>
		/// <param name="type">
		/// �w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A
		/// �w�肵�����W�����̃m�[�h�̒��ł��A�ǂ̈ʒu�ɑ������Ă��邩��Ԃ��܂��B
		/// �m�[�h��������Ȃ������ꍇ�ɂ́A(TreeNodeHitType)0 ��Ԃ��܂��B
		/// </param>
		/// <returns>��v����m�[�h�����������ۂ� true ��Ԃ��܂��A
		/// ������Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool ClientPosition2Node(Gdi::Point pClient,out TreeNode node,out TreeNodeHitType type){
			return this.LogicalPosition2Node(ClientPos2LogicalPos(pClient),out node,out type);
		}
		/// <summary>
		/// �_�����W���N���C�A���g���W�ɕϊ����܂��B
		/// </summary>
		/// <param name="pLogical">�_�����W���w�肵�܂��B</param>
		/// <returns>�w�肵�����W�l���N���C�A���g���W�ɕϊ����ĕԂ��܂��B</returns>
		protected internal Gdi::Point LogicalPos2ClientPos(Gdi::Point pLogical){
			pLogical.X+=this.AutoScrollPosition.X;
			pLogical.Y+=this.AutoScrollPosition.Y;
			return pLogical;
		}
		/// <summary>
		/// �N���C�A���g���W��_�����W�ɕϊ����܂��B
		/// </summary>
		/// <param name="pClient">�N���C�A���g���W���w�肵�܂��B</param>
		/// <returns>�w�肵�����W�l��_�����W�ɕϊ����ĕԂ��܂��B</returns>
		protected internal Gdi::Point ClientPos2LogicalPos(Gdi::Point pClient){
			pClient.X-=this.AutoScrollPosition.X;
			pClient.Y-=this.AutoScrollPosition.Y;
			return pClient;
		}
		//============================================================
		//		�m�[�h�폜���̏���
		//============================================================
		/// <summary>
		/// TreeView ����w�肵�� TreeNode ���폜���܂��B
		/// �q�m�[�h�ɑ΂���ċA�͎��s���܂���B
		/// </summary>
		/// <param name="node">�o�^���������� TreeNode �C���X�^���X���w�肵�܂��B</param>
		internal void UnregisterTreeNode(TreeNode node){
			this.SelectedNodes.Remove(node);
			if(this.LastMouseNode==node)		this.LastMouseNode=null;
			if(this.LastDragNode==node)			this.set_LastDragNode(null,null);
			if(this.MouseActivatedNode==node)	this.MouseActivatedNode=null;
			if(this.mouseDownNode==node)		this.mouseDownNode=null;
			if(this.focused_node==node)			this.FocusedNode=null;

			//if(this.dragTargetNode==node)		this.dragTargetNode=null;
			//------------------------------------------------------------------
			// �ȉ��̗��R���Ȃ� dragTargetNode �̓N���A���Ȃ�
			// 0. dragTargetNode �� null �ɂ��������ł� DD ����͐���ɏI���o���Ȃ��B
			// 1. dragTargetNode �͉��� DD ���삪�I��鎞�ɉ��������B
			// 2. �����I������ drag ���Ă��鎞�ɂ́AdragTargetNode �ɓo�^���ꂸ�ڐG���鎖���o���Ȃ��l�ɂȂ��Ă���B
			//    �P��I���̎������ADD ���� DD �������ɂȂ�l�ȓ���͕ςł���B
			// 3. �������� DD ���ɂ��̊Ԃɂ��� DD ���I����Ă���Ƃ����͎̂~�߂������ǂ��B
			//------------------------------------------------------------------
		}
#if OLD
		/// <summary>
		/// TreeView ����w�肵�� TreeNode ���폜���܂��B
		/// ���ݑI�����ꂽ�菈���̑ΏۂɂȂ��Ă���ꍇ�ɂ́A����������������܂��B
		/// </summary>
		/// <param name="node">TreeView ����폜���� node ���w�肵�܂��B</param>
		internal void UnregisterRemovedNode(TreeNode node){
			if(node.View!=this)
				throw new TreeNodeException("�w�肳�ꂽ TreeNode �͂��� TreeView �ɓo�^����Ă���m�[�h�ł͂���܂���B");

			// ��ɑI���͑S�ĉ���
			_todo.TreeNodeSelectionChanged("���ݓ�񔭐�����B��񂾂��ŏ[���BSelectedNodes �� Suppress** ��ǉ��B");
			this.SelectedNodes.Remove(node);
			this.SelectedNodes.RemoveDescendants(node);

			this.UnregisterRemovedNode_Unregister(node);
		}
		private void UnregisterRemovedNode_Unregister(TreeNode node){
			if(this.HasChildren)foreach(TreeNode child in node.Nodes)
				this.UnregisterRemovedNode_Unregister(child);

			this.UnregisterTreeNode(node);
			node.View=null;
		}
		/// <summary>
		/// TreeView ����A�w�肵���m�[�h�̎q�m�[�h��S�ĉ������܂��B
		/// </summary>
		/// <param name="node">�q�m�[�h��S�폜����e�m�[�h���w�肵�܂��B</param>
		internal void UnregisterChildrenNodes(TreeNode node){
			if(node==this.root){
				this.UnregisterAllNodes();
			}

			if(node.View!=this)
				throw new TreeNodeException("�w�肳�ꂽ TreeNode �͂��� TreeView �ɓo�^����Ă���m�[�h�ł͂���܂���B");

			this.SelectedNodes.RemoveDescendants(node);
			if(this.HasChildren)foreach(TreeNode child in node.Nodes)
				this.UnregisterRemovedNode_Unregister(child);
		}
		/// <summary>
		/// TreeView �̒��̑S�Ẵm�[�h���������܂��B
		/// </summary>
		internal void UnregisterAllNodes(){
			if(this.HasChildren)foreach(TreeNode child in this.root.Nodes)
				this.UnregisterAllNodes_ClearView(child);
			this.LastMouseNode=null;
			this.LastDragNode=null;
			this.MouseActivatedNode=null;
			this.mouseDownNode=null;
			this.focused_node=null;
			//this.dragTargetNode=null;
		}
		private void UnregisterAllNodes_ClearView(TreeNode node){
			if(this.HasChildren)foreach(TreeNode child in node.Nodes)
				this.UnregisterAllNodes_ClearView(node);
			node.View=null;
		}
#endif
	}
	/// <summary>
	/// ����̃m�[�h�̐ݒ���Ǘ�����N���X�ł��B
	/// </summary>
	//[CM::TypeConverter(typeof(Design.TreeNodeSettingsConverter))]
	public partial class TreeNodeSettings{
		/// <summary>
		/// TreeNodeSettings �N���X�̃R���X�g���N�^�ł��B
		/// </summary>
		public TreeNodeSettings(){}

		/*
		private int childIndent=18;
		/// <summary>
		/// �q�m�[�h�̃C���f���g���擾���͐ݒ肵�܂��B
		/// </summary>
		public int ChildIndent{
			get{return this.childIndent;}
			set{this.childIndent=value;}
		}

		private int height=14;
		/// <summary>
		/// ���̃m�[�h�̍������w�肵�܂��B
		/// </summary>
		public int Height{
			get{return this.height;}
			set{this.height=value;}
		}
		//*/
	}

	#region Events
	/// <summary>
	/// �v���p�e�B���ω������ۂɔ�������C�x���g�̏���񋟂��܂��B
	/// </summary>
	public sealed class TreeNodePropertyChangingEventArgs<T>:System.EventArgs{
		T oldState;
		T newState;
		internal TreeNodePropertyChangingEventArgs(T oldState,T newState){
			this.oldState=oldState;
			this.newState=newState;
		}
		/// <summary>
		/// �v���p�e�B���ω�����O�̒l���擾���܂��B
		/// </summary>
		public T OldValue{
			get{return this.oldState;}
		}
		/// <summary>
		/// �v���p�e�B���ω�������̒l���擾���܂��B
		/// </summary>
		public T NewValue{
			get{return this.newState;}
		}
	}
	/// <summary>
	/// TreeNode �Ɋւ���C�x���g����������֐���\���܂��B
	/// </summary>
	/// <param name="sender">�C�x���g�̔��������w�肵�܂��B</param>
	/// <param name="e">�C�x���g�Ɋւ������񋟂��܂��B</param>
	public delegate void TreeNodeEventHandler(object sender,TreeNodeEventArgs e);
	/// <summary>
	/// TreeNode �Ɋւ��� Event �̏���񋟂��܂��B
	/// </summary>
	public class TreeNodeEventArgs:System.EventArgs{
		private readonly TreeNode node;
		private bool handled;
		/// <summary>
		/// TreeNodeEventArgs �N���X�����������܂��B
		/// </summary>
		/// <param name="node">�C�x���g�̔����Ɋ֗^���� TreeNode ���w�肵�܂��B</param>
		public TreeNodeEventArgs(TreeNode node):base(){
			this.node=node;
			this.handled=false;
		}
		/// <summary>
		/// �C�x���g�ɒ��ڊ֌W�������m�[�h��Ԃ��܂��B
		/// </summary>
		public TreeNode Node{
			get{return this.node;}
		}
		/// <summary>
		/// �m�[�h��ێ����Ă��� TreeView ���擾���܂��B
		/// �m�[�h���������Ă��� TreeView �����݂��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeView View{
			get{return this.node.View;}
		}
		/// <summary>
		/// ���̃C�x���g�ɑ΂��鏈�����I���������ۂ����擾���͐ݒ肵�܂��B
		/// true �ɐݒ肷��ƁA�ȍ~�ɖ{���s���锤���������� (MouseDown ��̃m�[�h�̑I���Ȃ�) ��S�ďȗ����܂��B
		/// </summary>
		public bool Handled{
			get{return this.handled;}
			set{this.handled=value;}
		}
	}
	/// <summary>
	/// TreeNode ��ł̃}�E�X�Ɋւ���C�x���g����������֐���\���܂��B
	/// </summary>
	/// <param name="sender">�C�x���g�̔��������w�肵�܂��B</param>
	/// <param name="e">�C�x���g�Ɋւ������񋟂��܂��B</param>
	public delegate void TreeNodeMouseEventHandler(object sender,TreeNodeMouseEventArgs e);
	/// <summary>
	/// TreeNode ��ł̃}�E�X�C�x���g�Ɋւ������񋟂��܂��B
	/// </summary>
	public sealed class TreeNodeMouseEventArgs:TreeNodeEventArgs{
		readonly Gdi::Point location;
		readonly System.Windows.Forms.MouseButtons button;
		readonly int clicks;
		readonly TreeNodeHitType type;
		readonly int delta;
		/// <summary>
		/// System.Windows.Forms.MouseEventArgs �����ɁATreeNodeMouseEventArgs �C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="node">�C�x���g�̔����̑Ώۂ��w�肵�܂��B</param>
		/// <param name="type">���݂̃}�E�X�̈ʒu�� node �ɑ΂��Ăǂ̗l�Ȉʒu�ɂ��邩���w�肵�܂��B</param>
		/// <param name="e">�}�E�X�̃C�x���g�Ɋւ�������w�肵�܂��B</param>
		public TreeNodeMouseEventArgs(TreeNode node,TreeNodeHitType type,System.Windows.Forms.MouseEventArgs e)
			:base(node)
		{
			this.location=e.Location;
			this.button=e.Button;
			this.clicks=e.Clicks;
			this.type=type;
			this.delta=e.Delta;
		}
		/// <summary>
		/// �w�肵�� TreeView �� System.Windows.Forms.MouseEventArgs ����A�C�x���g�Ɋ֘A���� TreeNode ����������
		/// TreeNodeMouseEventArgs �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="view">�C�x���g���������� TreeView ���w�肵�܂��B</param>
		/// <param name="e">�}�E�X�C�x���g�Ɋւ������ێ����Ă��� MouseEventArgs �C���X�^���X���w�肵�܂��B</param>
		/// <returns>�C�x���g�Ɋ֘A���� TreeNode �����ɍ쐬���� TreeNodeMouseEventArgs �C���X�^���X��Ԃ��܂��B
		/// �֘A���� TreeNode �����݂��Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		public static TreeNodeMouseEventArgs CreateFromMouseEventArgs(TreeView view,Forms::MouseEventArgs e){
			TreeNode node;
			TreeNodeHitType type;
			view.ClientPosition2Node(e.Location,out node,out type);
			if(node==null)return null;

			return new TreeNodeMouseEventArgs(node,type,e);
		}
		//============================================================
		//		Properties
		//============================================================
		/// <summary>
		/// �C�x���g�������̃}�E�X�̍��W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public Gdi::Point ClientLocation{
			get{return this.location;}
		}
		/// <summary>
		/// �C�x���g�������̃}�E�X�� X ���W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public int ClientX{
			get{return this.location.X;}
		}
		/// <summary>
		/// �C�x���g�������̃}�E�X�� Y ���W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public int ClientY{
			get{return this.location.Y;}
		}
		/// <summary>
		/// �A�����ăN���b�N�����񐔂��擾���܂��B
		/// MouseDown �C�x���g���ŗ��p�\�ł��B
		/// </summary>
		public int Clicks{
			get{return this.clicks;}
		}
		/// <summary>
		/// �C�x���g�������ɉ�����Ă����}�E�X�̃{�^�����擾���܂��B
		/// </summary>
		public System.Windows.Forms.MouseButtons MouseButton{
			get{return this.button;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�̂ǂ̕����ɑ������邩��\���l���擾���܂��B
		/// </summary>
		public TreeNodeHitType HitType{
			get{return this.type;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�ɑ΂��Ăǂ̐����ʒu�ɂ��邩��\���l���擾���܂��B
		/// <see cref="TreeNodeHitType.BorderTop"/>, <see cref="TreeNodeHitType.Middle"/>,
		/// <see cref="TreeNodeHitType.BorderBottom"/> �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeVertical{
			get{return this.type&TreeNodeHitType.mask_vertical_hit;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�ɑ΂��Ăǂ̐����ʒu�ɂ��邩��\���l���擾���܂��B
		/// <see cref="TreeNodeHitType.CheckBox"/>, <see cref="TreeNodeHitType.Icon"/>,
		/// <see cref="TreeNodeHitType.Content"/>, <see cref="TreeNodeHitType.IndentArea"/>,
		/// <see cref="TreeNodeHitType.PaddingLeft"/>, <see cref="TreeNodeHitType.PaddingRight"/>
		/// �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeHorizontal{
			get{return this.type&TreeNodeHitType.mask_horizontal;}
		}
		/// <summary>
		/// �}�E�X�|�C���^���A�m�[�h�̓��蕔���̏�ɂ��邩�ǂ������擾���܂��B
		/// <see cref="TreeNodeHitType.OnPlusMinus"/>, <see cref="TreeNodeHitType.OnCheckBox"/>
		/// <see cref="TreeNodeHitType.OnIcon"/>, <see cref="TreeNodeHitType.OnContent"/>
		/// �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeOn{
			get{return this.type&TreeNodeHitType.mask_on_something;}
		}
		/// <summary>
		/// �X�N���[���ʂ̃f���^�l���擾���܂��B
		/// </summary>
		public int ScrollDelta{
			get{return this.delta;}
		}
		/// <summary>
		/// �X�N���[���ʂ��s���Ŏ擾���܂��B
		/// </summary>
		public int ScrollLines{
			get{
				return this.delta
					*Forms::SystemInformation.MouseWheelScrollLines
					/Forms::SystemInformation.MouseWheelScrollDelta;
			}
		}
	}
	/// <summary>
	/// TreeNode �̃h���b�O�Ɋւ���C�x���g����������֐���\���܂��B
	/// </summary>
	/// <param name="sender">�C�x���g�̔��������w�肵�܂��B</param>
	/// <param name="e">�C�x���g�Ɋւ������񋟂��܂��B</param>
	public delegate void TreeNodeDragEventHandler(object sender,TreeNodeDragEventArgs e);
	/// <summary>
	/// TreeNode �̃h���b�O�Ɋւ��� Event �̏���񋟂��܂��B
	/// </summary>
	public sealed class TreeNodeDragEventArgs:TreeNodeEventArgs{
		readonly Forms::DragEventArgs e;
		readonly TreeNodeHitType type;
		readonly Gdi::Point location;
		/// <summary>
		/// �w�肵�� TreeNode �� TreeNodeHitType �y�� System.Windows.Forms.DragEventArgs �����ɂ��āA
		/// TreeNodeDragDropEventArgs �C���X�^���X�����������܂��B
		/// </summary>
		/// <param name="node">Drag ��̃m�[�h���w�肵�܂��B</param>
		/// <param name="type">�}�E�X���m�[�h��̂ǂ̈ʒu�ɑ��݂��Ă��邩���w�肵�܂��B</param>
		/// <param name="e">Drag �C�x���g�Ɋւ�������擾���܂��B</param>
		public TreeNodeDragEventArgs(TreeNode node,TreeNodeHitType type,Forms::DragEventArgs e):base(node){
			this.e=e;
			this.type=type;
			this.location=new Gdi::Point(e.X,e.Y);
		}
		/// <summary>
		/// �w�肵�� TreeView �� System.Windows.Forms.DragEventArgs ����A�C�x���g�Ɋ֘A���� TreeNode ����������
		/// TreeNodeDragDropEventArgs �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="view">�C�x���g���������� TreeView ���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�C�x���g�Ɋւ������ێ����Ă��� DragEventArgs �C���X�^���X���w�肵�܂��B</param>
		/// <returns>�C�x���g�Ɋ֘A���� TreeNode �����ɍ쐬���� TreeNodeDragDropEventArgs �C���X�^���X��Ԃ��܂��B
		/// �֘A���� TreeNode �����݂��Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		public static TreeNodeDragEventArgs CreateFromDragEventArgs(TreeView view,Forms::DragEventArgs e){
			TreeNode node;
			TreeNodeHitType type;
			Gdi::Point ptC=view.PointToClient(new Gdi::Point(e.X,e.Y));
			view.ClientPosition2Node(ptC,out node,out type);
			if(node==null)return null;

			return new TreeNodeDragEventArgs(node,type,e);
		}
		//=========================================================================
		//		Properties
		//=========================================================================
		/// <summary>
		/// �C�x���g�������̃}�E�X�̍��W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public Gdi::Point ClientLocation{
			get{return this.location;}
		}
		/// <summary>
		/// �C�x���g�������̃}�E�X�� X ���W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public int ClientX{
			get{return this.location.X;}
		}
		/// <summary>
		/// �C�x���g�������̃}�E�X�� Y ���W (TreeView ���W�n) ���擾���܂��B
		/// </summary>
		public int ClientY{
			get{return this.location.Y;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�̂ǂ̕����ɑ������邩��\���l���擾���܂��B
		/// </summary>
		public TreeNodeHitType HitType{
			get{return this.type;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�ɑ΂��Ăǂ̐����ʒu�ɂ��邩��\���l���擾���܂��B
		/// <see cref="TreeNodeHitType.BorderTop"/>, <see cref="TreeNodeHitType.Middle"/>,
		/// <see cref="TreeNodeHitType.BorderBottom"/> �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeVertical{
			get{return this.type&TreeNodeHitType.mask_vertical_hit;}
		}
		/// <summary>
		/// �}�E�X�|�C���^�̈ʒu���A�m�[�h�ɑ΂��Ăǂ̐����ʒu�ɂ��邩��\���l���擾���܂��B
		/// <see cref="TreeNodeHitType.CheckBox"/>, <see cref="TreeNodeHitType.Icon"/>,
		/// <see cref="TreeNodeHitType.Content"/>, <see cref="TreeNodeHitType.IndentArea"/>,
		/// <see cref="TreeNodeHitType.PaddingLeft"/>, <see cref="TreeNodeHitType.PaddingRight"/>
		/// �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeHorizontal{
			get{return this.type&TreeNodeHitType.mask_horizontal;}
		}
		/// <summary>
		/// �}�E�X�|�C���^���A�m�[�h�̓��蕔���̏�ɂ��邩�ǂ������擾���܂��B
		/// <see cref="TreeNodeHitType.OnPlusMinus"/>, <see cref="TreeNodeHitType.OnCheckBox"/>
		/// <see cref="TreeNodeHitType.OnIcon"/>, <see cref="TreeNodeHitType.OnContent"/>
		/// �̉��ꂩ�̒l�ɂȂ�܂��B
		/// </summary>
		public TreeNodeHitType HitTypeOn{
			get{return this.type&TreeNodeHitType.mask_on_something;}
		}
		/// <summary>
		/// �h���b�O���ŋ�����Ă���h���b�v������擾���܂��B
		/// </summary>
		public Forms::DragDropEffects AllowedEffect{
			get{return e.AllowedEffect;}
		}
		/// <summary>
		/// �h���b�v��ŋ�����h���b�v������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public Forms::DragDropEffects Effect{
			get{return e.Effect;}
			set{e.Effect=value;}
		}
		/// <summary>
		/// �h���b�O����Ă���f�[�^�̏����擾���܂��B
		/// </summary>
		public Forms::IDataObject Data{
			get{return e.Data;}
		}
		//-------------------------------------------------------------------------
		// http://msdn.microsoft.com/ja-jp/library/system.windows.forms.drageventargs.keystate(v=vs.80).aspx
		internal const int KEY_MLEFT=1;
		internal const int KEY_MRIGHT=2;
		internal const int KEY_MMIDDLE=16;
		internal const int KEY_SHIFT=4;
		internal const int KEY_CTRL=8;
		internal const int KEY_ALT=32;
		/// <summary>
		/// �}�E�X�{�^���A�C���L�[�̏�Ԃ�\���������擾���܂��B
		/// </summary>
		public int KeyState{
			get{return e.KeyState;}
		}
		/// <summary>
		/// �}�E�X�̍��{�^����������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool MouseLeft{get{return (e.KeyState&KEY_MLEFT)!=0;}}
		/// <summary>
		/// �}�E�X�̉E�{�^����������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool MouseRight{get{return (e.KeyState&KEY_MRIGHT)!=0;}}
		/// <summary>
		/// �}�E�X�̒��{�^����������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool MouseMiddle{get{return (e.KeyState&KEY_MMIDDLE)!=0;}}
		/// <summary>
		/// Ctrl �L�[��������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool KeyCtrl{get{return (e.KeyState&KEY_CTRL)!=0;}}
		/// <summary>
		/// Alt �L�[��������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool KeyAlt{get{return (e.KeyState&KEY_ALT)!=0;}}
		/// <summary>
		/// Shift �L�[��������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool KeyShift{get{return (e.KeyState&KEY_SHIFT)!=0;}}
	}
	/*
	/// <summary>
	/// TreeNode ��ł̃h���b�O�Ɋւ������񋟂��܂��B
	/// </summary>
	public sealed class TreeNodeDragEventArgs:TreeNodeEventArgs{
		readonly Gdi::Point location;
		readonly Forms::Keys ctrls;
		readonly TreeNodeHitType type;
		public TreeNodeDragEventArgs(Forms::DragEventArgs e){
			this.location=new Gdi::Point(e.X,e.Y);
		}
	}
	//*/
	#endregion

	#region class: SelectedNodeCollection
	/// <summary>
	/// �I������Ă��� TreeNode �̏W�����Ǘ����܂��B
	/// </summary>
	public sealed class SelectedNodeCollection
		:Gen::ICollection<TreeNode>,Gen::IEnumerable<TreeNode>
	{
		readonly TreeView view;
		Gen::List<TreeNode> nodes=new Gen::List<TreeNode>();
		internal SelectedNodeCollection(TreeView view){
			this.view=view;
		}
		
		private static void OnSelectedNoRedraw(TreeNode node){
			node.OnIsSelectedChanged(new TreeNodePropertyChangingEventArgs<bool>(false,true));
		}
		private static void OnSelected(TreeNode node){
			node.OnIsSelectedChanged(new TreeNodePropertyChangingEventArgs<bool>(false,true));
			node.ReDraw(false);
		}
		private static void OnUnselected(TreeNode node){
			// ReDraw �� IsSelectedChanged �̑��Ŏ��s���Ă��܂����ꍇ or ���s����K�v���Ȃ��ꍇ
			node.OnIsSelectedChanged(new TreeNodePropertyChangingEventArgs<bool>(true,false));
			node.ReDraw(false);
		}
		/// <summary>
		/// �I������Ă���m�[�h�̍ĕ`����s���܂��B
		/// </summary>
		internal void ReDraw(){
			foreach(TreeNode snode in this.nodes){
				snode.ReDraw(false);
			}
		}
		//============================================================c
		//		�ݒ�
		//============================================================c
		/// <summary>
		/// �w�肵�� TreeNode ������I��������Ԃɂ��܂��B
		/// </summary>
		/// <param name="node">�I�������� TreeNode ���w�肵�܂��B</param>
		/// <param name="suppressRedraw">�ĕ`���}�����邩�ۂ����w�肵�܂��B
		/// �����ōĕ`����s���ꍇ�ɂ� true ���w�肵�ĉ������B</param>
		public void Set(TreeNode node,bool suppressRedraw){
			if(node==null){
				this.Clear();
				return;
			}

			Gen::List<TreeNode> oldNodes=this.nodes;
			if(oldNodes.Count==1&&oldNodes[0]==node)return;

			// �ݒ�
			this.nodes=new Gen::List<TreeNode>();
			this.nodes.Add(node);
			
			// �����C�x���g
			bool contained=false;
			foreach(TreeNode snode in oldNodes){
				if(snode==node){
					contained=true;
					continue;
				}
				OnUnselected(snode);
			}

			// �I���C�x���g
			if(!contained){
				if(suppressRedraw)
					OnSelectedNoRedraw(node);
				else
					OnSelected(node);
			}

			this.view.OnSelectionChanged();
		}
		/// <summary>
		/// �w�肵�� TreeNode ������I��������Ԃɂ��܂��B
		/// </summary>
		/// <param name="node">�I�������� TreeNode ���w�肵�܂��B</param>
		public void Set(TreeNode node){
			this.Set(node,false);
		}
		/// <summary>
		/// �w�肵�� TreeNode �W����I��������Ԃɂ��܂��B
		/// </summary>
		/// <param name="nodeCollection">�I������� TreeNode �̏W�����w�肵�܂��B</param>
		public void Set(Gen::IEnumerable<TreeNode> nodeCollection){
			Gen::List<TreeNode> oldNodes=this.nodes;
			bool changed=false;

			this.nodes=new Gen::List<TreeNode>();
			foreach(TreeNode addee in nodeCollection){
				nodes.Add(addee);

				int index=oldNodes.IndexOf(addee);
				if(index>=0){
					// ������I�� (�����C�x���g���)
					oldNodes[index]=null;
				}else{
					changed=true;

					// �V�K�I��
					OnSelected(addee);
				}
			}

			// �����C�x���g
			foreach(TreeNode removed in oldNodes){
				if(removed==null)continue;
				OnUnselected(removed);
				changed=true;
			}

			if(changed)this.view.OnSelectionChanged();
		}
		//============================================================
		//		�ǉ��E�폜
		//============================================================
		/// <summary>
		/// �w�肵�� TreeNode ��I�����ꂽ��Ԃɂ��܂��B
		/// </summary>
		/// <param name="node">�I������ TreeNode ���w�肵�܂��B</param>
		public void Add(TreeNode node){
			if(this.nodes.Contains(node))return;
			this.nodes.Add(node);
			OnSelected(node);
			this.view.OnSelectionChanged();
		}
		/// <summary>
		/// �w�肵�� TreeNode �̑I�����������܂��B
		/// �w�肵�� TreeNode ��������I������Ă��Ȃ��ꍇ�ɂ͉������܂���B
		/// </summary>
		/// <param name="node">�I�������������� TreeNode ���w�肵�܂��B</param>
		/// <returns>�w�肵�� TreeNode ���I������Ă��āA�I�����������ꂽ�ꍇ�� true ��Ԃ��܂��B
		/// ������I������Ă��Ȃ������ꍇ�ɂ́Afalse ��Ԃ��܂��B</returns>
		public bool Remove(TreeNode node){
			bool ret=this.nodes.Remove(node);
			if(ret){
				OnUnselected(node);
				this.view.OnSelectionChanged();
			}
			return ret;
		}
		/// <summary>
		/// �I������Ă���v�f��S�ĉ������܂��B
		/// </summary>
		public void Clear(){
			if(this.nodes.Count==0)return;

			Gen::List<TreeNode> oldNodes=this.nodes;
			this.nodes=new Gen::List<TreeNode>();
			foreach(TreeNode snode in oldNodes)
				OnUnselected(snode);

			this.view.OnSelectionChanged();
		}
		/// <summary>
		/// �w�肵�� TreeNode �̎q���̃m�[�h�̑I�����������܂��B
		/// �w�肵�� TreeNode ���̂̑I���͉�������܂���B
		/// </summary>
		/// <param name="node">�q���̃m�[�h�̑I������������ TreeNode ���w�肵�܂��B</param>
		public void RemoveDescendants(TreeNode node){
			Gen::List<TreeNode> removed=new System.Collections.Generic.List<TreeNode>();

			this.nodes.RemoveAll(delegate(TreeNode snode){
				if(!node.IsDescendant(snode))return false;
				removed.Add(snode);
				return true;
			});

			if(removed.Count==0)return;
			foreach(TreeNode rem in removed)
				OnUnselected(rem);
			this.view.OnSelectionChanged();
		}
		//============================================================
		//		�I�𔽓]
		//============================================================
		/// <summary>
		/// �w�肵�� TreeNode �̑I����Ԃ�؂�ւ��܂��B
		/// �w�肵�� TreeNode ���I�����ꂽ��Ԃɂ������ꍇ�ɂ́A�I�����������܂��B
		/// �w�肵�� TreeNode ���I������Ă��Ȃ������ꍇ�ɂ́A�I�����ꂽ��Ԃɂ��܂��B
		/// </summary>
		/// <param name="node">�I����Ԃ�؂�ւ���Ώۂ� Node ���w�肵�܂��B</param>
		/// <returns>�ؑ֌�̑I����Ԃ�Ԃ��܂��B
		/// �I�����ꂽ��ԂɂȂ����ꍇ�ɂ́Atrue ��Ԃ��܂��B
		/// �I�����������ꂽ�ꍇ�A�܂��́A�w�肵�� TreeNode ������ TreeView �ɑ����� TreeNode �łȂ������ꍇ�� false ��Ԃ��܂��B
		/// </returns>
		public bool Switch(TreeNode node){
			if(node.View!=this.view)return false;

			if(this.nodes.Contains(node)){
				this.Remove(node);
				return false;
			}else{
				this.Add(node);
				return true;
			}
		}
		/// <summary>
		/// �w�肵���W���Ɋ܂܂�� TreeNode �̑I����Ԃ𔽓]���܂��B
		/// </summary>
		/// <param name="nodeCollection">�I����Ԃ𔽓]���� TreeNode �̏W�����w�肵�܂��B</param>
		public void Switch(Gen::IEnumerable<TreeNode> nodeCollection){
			bool changed=false;

			foreach(TreeNode node in nodeCollection){
				if(node.View!=this.view)continue;

				if(this.nodes.Contains(node)){
					// �I������
					if(!this.nodes.Remove(node))continue;
					OnUnselected(node);
				}else{
					// �I��
					this.nodes.Add(node);
					OnSelected(node);
				}

				changed=true;
			}

			if(changed)
				this.view.OnSelectionChanged();
		}
		//============================================================
		//		�͈͒ǉ��E�폜
		//============================================================
		/// <summary>
		/// �w�肵���͈͂� TreeNode ��I�����܂��B
		/// </summary>
		/// <param name="axis">�I���̎��ƂȂ� TreeNode ���w�肵�܂��B
		/// null ���w�肷��� node �݂̂��I������܂��B
		/// null �ȊO���w�肷��� axis ���� node ���� TreeNode ���I������܂��B
		/// </param>
		/// <param name="node">�I���̑ΏۂƂȂ� TreeNode ���w�肵�܂��B
		/// </param>
		public void SelectRange(TreeNode axis,TreeNode node){
			if(node==null)
				throw new System.ArgumentNullException("node");

			if(axis==null){
				this.Set(node);
				return;
			}

			Gen::List<TreeNode> targets=new Gen::List<TreeNode>();
			targets.Add(axis);

			int c=TreeView.ComparePosition(axis,node);
			if(c<0){
				do{
					axis=axis.NextVisibleInTree;
					_todo.TreeViewAssert(axis!=null);

					targets.Add(axis);
				}while(axis!=node);
			}else if(c>0){
				do{
					axis=axis.PreviousVisibleInTree;
					_todo.TreeViewAssert(axis!=null);

					targets.Add(axis);
				}while(axis!=node);
			}

			this.Set(targets);
		}
		/// <summary>
		/// �w�肵���͈͂� TreeNode �̑I����Ԃ𔽓]���܂��B
		/// ��̓I�ɂ́Aaxis ���� node ���� TreeNode �̑I����Ԃ𔽓]�����܂��B
		/// </summary>
		/// <param name="axis">�I���̎��ƂȂ� TreeNode ���w�肵�܂��B
		/// axis ���̂̑I����Ԃ͔��]���܂���B
		/// null ���w�肷��� node �̑I����Ԃ݂̂����]���܂��B
		/// </param>
		/// <param name="node">�I�𔽓]�̑ΏۂƂȂ� TreeNode ���w�肵�܂��B</param>
		public void SwitchRange(TreeNode axis,TreeNode node){
			if(node==null)
				throw new System.ArgumentNullException("node");

			if(axis==null||axis==node){
				this.Switch(node);
				return;
			}

			Gen::List<TreeNode> targets=new Gen::List<TreeNode>();
			int c=TreeView.ComparePosition(axis,node);
			if(c<0){
				do{
					axis=axis.NextVisibleInTree;
					_todo.TreeViewAssert(axis!=null);

					targets.Add(axis);
				}while(axis!=node);
			}else if(c>0){
				do{
					axis=axis.PreviousVisibleInTree;
					_todo.TreeViewAssert(axis!=null);

					targets.Add(axis);
				}while(axis!=node);
			}
		
			this.Switch(targets);
		}
		//============================================================c
		//		�A�N�Z�X
		//============================================================c
		/// <summary>
		/// �w�肵�� TreeNode ���A
		/// ���̃C���X�^���X�Ɋ֘A�t�����Ă��� TreeView ���ŁA
		/// �I������Ă��邩�ۂ����擾���܂��B
		/// </summary>
		/// <param name="node">�I������Ă��邩�ۂ����擾������ TreeNode ���w�肵�܂��B</param>
		/// <returns>�w�肵�� TreeNode ���I������Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public bool Contains(TreeNode node){
			return node.View==this.view&&this.nodes.Contains(node);
		}
		/// <summary>
		/// �I������Ă��� TreeNode �̏W����z��ɃR�s�[���܂��B
		/// </summary>
		/// <param name="array">�I������Ă��� TreeNode �̊i�[��̔z��C���X�^���X���w�肵�܂��B</param>
		/// <param name="arrayIndex">array ���Ŋi�[���J�n����ʒu���w�肵�܂��B
		/// �w�肵���ʒu�ɑI������Ă���v�f�̑��v�f���i�[����܂��B</param>
		public void CopyTo(TreeNode[] array,int arrayIndex){
			if(array==null)throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex>=array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex");
			if(arrayIndex+this.Count>array.Length)
				throw new System.ArgumentException("�w�肵���z��̗e�ʂ�����܂���B","array");

			foreach(TreeNode node in nodes)
				array[arrayIndex++]=node;
		}
		/// <summary>
		/// �I������Ă��� TreeNode �̐����擾���܂��B
		/// </summary>
		public int Count{
			get{return this.nodes.Count;}
		}
		/// <summary>
		/// �I������Ă��鍀�ڂ��ǂݎ���p���ۂ����擾���܂��B
		/// SelectedNodes �ł͏�ɕύX���\�Ȃ̂ŁA���̃v���p�e�B�͏�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsReadOnly{
			get{return false;}
		}
		/// <summary>
		/// �I������Ă��� TreeNode �̗񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�I������Ă��� TreeNode �̗񋓎q���쐬���ĕԂ��܂��B</returns>
		public Gen::IEnumerator<TreeNode> GetEnumerator(){
			return this.nodes.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
	}
	#endregion
}
