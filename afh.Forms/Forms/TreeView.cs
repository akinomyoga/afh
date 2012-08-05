using Forms=System.Windows.Forms;
using Gdi=System.Drawing;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Gen=System.Collections.Generic;

namespace afh.Forms{
	/// <summary>
	/// 拡張可能な TreeView コントロールを提供します。
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.TreeView))]
	[CM::DefaultProperty("Nodes")]
	[CM::DefaultEvent("FocusedNodeChanged")]
	public class TreeView:System.Windows.Forms.ScrollableControl{
		/// <summary>
		/// ノードを集合を保持します。
		/// </summary>
		internal readonly TreeNode root;
		/// <summary>
		/// この View に登録されているノードの集合を指定します。
		/// </summary>
		[CM::Category("Tree")]
		[CM::Description("この View に登録されているノード達を指定します。")]
		public TreeNodeCollection Nodes{
			get{return this.root.Nodes;}
		}
		/// <summary>
		/// ノードの基本設定を保持します。
		/// </summary>
		private readonly TreeNodeSettings default_param=new TreeNodeSettings();
		/// <summary>
		/// この View に登録されているノードの基本設定を管理するインスタンスを取得します。
		/// </summary>
		[CM::TypeConverter(typeof(Design.TreeNodeSettingsConverter))]
		[CM::DesignerSerializationVisibility(CM::DesignerSerializationVisibility.Content)]
		[CM::Category("Tree")]
		[CM::Description("この View に登録されているノードの基本設定を指定します。")]
		public TreeNodeSettings DefaultNodeParams{
			get{return this.default_param;}
		}

		private bool multiSelect=false;
		/// <summary>
		/// TreeNode を複数選択可能か否かを取得又は設定します。
		/// </summary>
		[CM::Category("Tree")]
		[CM::Description("TreeNode を複数選択可能か否かを指定します。")]
		[CM::DefaultValue(false)]
		public bool MultiSelect{
			get{return this.multiSelect;}
			set{this.multiSelect=value;}
		}
		//============================================================
		//		ノード状態
		//============================================================
		private TreeNode focused_node=null;
		/// <summary>
		/// 現在注目しているノードを取得します。
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
		/// 現在選択されているノードの集合を取得します。
		/// </summary>
		[CM::Browsable(false)]
		public SelectedNodeCollection SelectedNodes{
			get{
				//OK:_todo.EssentialTreeView("選択要素コレクションが変更された事を検知してイベントを発する");
				return this.selected;
			}
		}
		/// <summary>
		/// 指定した TreeNode が現在活性化しているか否かを判定します。
		/// </summary>
		/// <param name="node">活性化しているか確認する TreeNode を指定します。</param>
		/// <returns>node が活性化した状態にある時に true を返します。
		/// それ以外の場合に false を返します。</returns>
		internal bool IsActivated(TreeNode node){
			if(this.dragging)
				return IsDraggedNode(node);
			if(this.mouseActivatedNode!=null)
				return node==this.mouseActivatedNode;
			return node.IsSelected;
		}
		/// <summary>
		/// 現在選択されているノードの集合に変更があった時に発生します。
		/// </summary>
		[CM::Category("Tree")]
		public event afh.CallBack<TreeView> SelectionChanged;
		internal void OnSelectionChanged(){
			if(this.SelectionChanged==null)return;
			this.SelectionChanged(this);
		}
		/// <summary>
		/// 現在フォーカスを持っているノードが変更された時に発生します。
		/// </summary>
		[CM::Category("Tree")]
		public event afh.CallBack<TreeView> FocusedNodeChanged;
		internal void OnFocusedNodeChanged(){
			if(this.FocusedNodeChanged==null)return;
			this.FocusedNodeChanged(this);
		}
		//============================================================
		//		初期化
		//============================================================
		/// <summary>
		/// TreeView のインスタンスを作成し、初期化します。
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
			sz.Height+=5; // 少し余裕 (続きがない事が視覚的に分かる)
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
		/// コントロール ハンドルが作成されるときに必要な作成パラメータを取得します。
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
		//		描画
		//============================================================
		/// <summary>
		/// Paint イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している PaintEventArgs。</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e){
			base.OnPaint(e);
			this.DrawNodes(e);
		}
		/// <summary>
		/// ノードを描画します。
		/// </summary>
		protected internal void DrawNodes(){
			using(Gdi::Graphics g=this.CreateGraphics()){
				g.SetClip(this.ClientRectangle);
				this.DrawNodes(g);
			}
		}
		/// <summary>
		/// ノードを指定した PaintEventArgs を使用して描画します。
		/// </summary>
		/// <param name="e">描画に関する情報を提供するオブジェクトを指定します。</param>
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
		//		キーボードイベント
		//============================================================
		/// <summary>
		/// コマンド キーを処理します。
		/// </summary>
		/// <param name="msg">処理するウィンドウ メッセージを表す、参照渡しされた Message。</param>
		/// <param name="keyData">処理するキーを表す Keys 値の 1 つ。</param>
		/// <returns>文字がコントロールによって処理された場合は true。それ以外の場合は false。</returns>
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
		//		ドラッグドロップイベント
		//============================================================
		//	Dragged Items
		//------------------------------------------------------------
		private TreeNode dragTargetNode=null;
		private bool dragging=false;
		/// <summary>
		/// 指定した TreeNode が現在ドラッグされている TreeNode か否かを取得します。
		/// </summary>
		/// <param name="node">ドラッグされているか否かを判定する TreeNode を指定します。</param>
		/// <returns>
		/// 指定した TreeNode が現在ドラッグされている TreeNode の場合に true を返します。
		/// それ以外の場合に false を返します。
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
		/// DragOver イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している DragEventArgs。</param>
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
		/// DragLeave イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している EventArgs。</param>
		protected override void OnDragLeave(System.EventArgs e){
			this.set_LastDragNode(null,null);
			base.OnDragLeave(e);
		}
		/// <summary>
		/// DragDrop イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している DragEventArgs。</param>
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
		/// GiveFeedback イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している GiveFeedbackEventArgs。</param>
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
		//		マウスイベント
		//============================================================
		/// <summary>
		/// 現在活性化しているノードを示します。
		/// activatedNode はマウスのボタンが押下されている間、その下にあった TreeNode が設定されます。
		/// </summary>
		private TreeNode mouseActivatedNode=null;
		/// <summary>
		/// ActivatedNode が null の場合には、現在選択されているノードが IsActive になります。
		/// ActivatedNode が null でない場合には、そのノードが IsActive の状態になります。
		/// </summary>
		private TreeNode MouseActivatedNode{
			get{return this.mouseActivatedNode;}
			set{
				if(mouseActivatedNode==value)return;
				TreeNode oldval=mouseActivatedNode;
				mouseActivatedNode=value;

				if(oldval!=null)oldval.ReDraw(false);
				if(value!=null)value.ReDraw(false);

				// SelectedNodes の IsActivated も変化
				if(oldval==null||value==null){
					this.SelectedNodes.ReDraw();
				}
			}
		}
		/// <summary>
		/// マウスのボタンが押下された時点での、その下にあった TreeNode を保持します。
		/// マウスポインタがその TreeNode の外に出た時点で null となります。
		/// <remarks>
		/// null になる機会が異なるので、mouseDownArgs とは別々に管理します。
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
		/// マウスが領域を去った時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
		protected override void OnMouseLeave(System.EventArgs e) {
			this.LastMouseNode=null;
			base.OnMouseLeave(e);
		}
		/// <summary>
		/// マウスが領域を動いた時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
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

				// Drag & Drop 開始
				do{
					if(e.Button!=Forms::MouseButtons.Left)break;
					if(mouseDownArgs==null||mouseDownArgs.HitTypeOn!=TreeNodeHitType.OnContent)break;

					// ドラッグ範囲の確認
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
		/// マウスのボタンが押下された時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
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
		/// マウスのボタンが上がった時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
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
		/// マウスを以てクリックが為された時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
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
							//>_todo.EssentialTreeView("選択状態の変更に対するイベント発生。これは class SelectedNodes を作った方が早いかも知れない。");

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
		/// マウスを以てダブルクリックが為された時のイベントを発生させます。
		/// </summary>
		/// <param name="e">イベントに関連する情報を指定します。</param>
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
		/// MouseWheel イベントを発生させます。
		/// </summary>
		/// <param name="e">イベント データを格納している MouseEventArgs。</param>
		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e){
			_todo.TreeNode("TreeNode それぞれに対しても MouseWheel を発生させる?");

			int scroll=e.Delta
				*(this.HScroll?this.HorizontalScroll.SmallChange:this.VerticalScroll.SmallChange)
				*Forms::SystemInformation.MouseWheelScrollLines
				/Forms::SystemInformation.MouseWheelScrollDelta;
				
			base.OnMouseWheel(new Forms::MouseEventArgs(e.Button,e.Clicks,e.X,e.Y,scroll));
		}
		#endregion

		//============================================================
		//		ノード選択
		//============================================================
		/// <summary>
		/// 同じ TreeView に属する二つの TreeNode を比較して、どちらの方が下に表示されるかを取得します。
		/// </summary>
		/// <param name="node1">比較する TreeNode を指定します。</param>
		/// <param name="node2">比較する TreeNode を指定します。</param>
		/// <returns>
		/// node1 と node2 が同じ TreeNode の場合には 0 を返します。
		/// node1 の方が node2 より下に表示される時には 1 を返します。
		/// node2 の方が node1 より下に表示される時には -1 を返します。
		/// </returns>
		public static int ComparePosition(TreeNode node1,TreeNode node2){
			// exceptions
			if(node1.View==null)
				throw new TreeViewMissingException("引数 node1 に親 View が関連付けられていません。");
			if(node2.View==null)
				throw new TreeViewMissingException("引数 node2 に親 View が関連付けられていません。");
			if(node1.View!=node2.View)
				throw new TreeNodeException("比較対象の TreeNode が異なる TreeView に所属しています。");

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
		//		ノード位置探索
		//============================================================
		/// <summary>
		/// 指定した位置にあるノードを取得します。
		/// </summary>
		/// <param name="p">取得したいノードの位置を、論理的な座標で指定します。</param>
		/// <param name="node">指定した位置にノードが存在した場合には、そのノードを返します。
		/// ノードが見つからなかった場合には null を返します。</param>
		/// <param name="type">
		/// 指定した位置にノードが存在した場合には、
		/// 指定した座標がそのノードの中でも、どの位置に相当しているかを返します。
		/// ノードが見つからなかった場合には、(TreeNodeHitType)0 を返します。
		/// </param>
		/// <returns>一致するノードが見つかった際に true を返します、
		/// 見つからなかった場合に false を返します。</returns>
		public bool LogicalPosition2Node(Gdi::Point p,out TreeNode node,out TreeNodeHitType type) {
			_todo.ExamineTreeView();
			node=this.root.LocalPosition2Node(p,out type);
			return node!=null;
		}
		/// <summary>
		/// 指定した位置にあるノードを取得します。
		/// </summary>
		/// <param name="p">取得したいノードの位置を、論理的な座標で指定します。</param>
		/// <param name="node">指定した位置にノードが存在した場合には、そのノードを返します。
		/// ノードが見つからなかった場合には null を返します。</param>
		/// <returns>一致するノードが見つかった際に true を返します、
		/// 見つからなかった場合に false を返します。</returns>
		public bool LogicalPosition2Node(Gdi::Point p,out TreeNode node){
			TreeNodeHitType temp;
			return this.LogicalPosition2Node(p,out node,out temp);
		}
		/// <summary>
		/// 指定した位置にあるノードを取得します。
		/// </summary>
		/// <param name="pClient">取得したいノードの位置を、クライアント座標で指定します。</param>
		/// <param name="node">指定した位置にノードが存在した場合には、そのノードを返します。
		/// ノードが見つからなかった場合には null を返します。</param>
		/// <param name="type">
		/// 指定した位置にノードが存在した場合には、
		/// 指定した座標がそのノードの中でも、どの位置に相当しているかを返します。
		/// ノードが見つからなかった場合には、(TreeNodeHitType)0 を返します。
		/// </param>
		/// <returns>一致するノードが見つかった際に true を返します、
		/// 見つからなかった場合に false を返します。</returns>
		public bool ClientPosition2Node(Gdi::Point pClient,out TreeNode node,out TreeNodeHitType type){
			return this.LogicalPosition2Node(ClientPos2LogicalPos(pClient),out node,out type);
		}
		/// <summary>
		/// 論理座標をクライアント座標に変換します。
		/// </summary>
		/// <param name="pLogical">論理座標を指定します。</param>
		/// <returns>指定した座標値をクライアント座標に変換して返します。</returns>
		protected internal Gdi::Point LogicalPos2ClientPos(Gdi::Point pLogical){
			pLogical.X+=this.AutoScrollPosition.X;
			pLogical.Y+=this.AutoScrollPosition.Y;
			return pLogical;
		}
		/// <summary>
		/// クライアント座標を論理座標に変換します。
		/// </summary>
		/// <param name="pClient">クライアント座標を指定します。</param>
		/// <returns>指定した座標値を論理座標に変換して返します。</returns>
		protected internal Gdi::Point ClientPos2LogicalPos(Gdi::Point pClient){
			pClient.X-=this.AutoScrollPosition.X;
			pClient.Y-=this.AutoScrollPosition.Y;
			return pClient;
		}
		//============================================================
		//		ノード削除時の処理
		//============================================================
		/// <summary>
		/// TreeView から指定した TreeNode を削除します。
		/// 子ノードに対する再帰は実行しません。
		/// </summary>
		/// <param name="node">登録を解除する TreeNode インスタンスを指定します。</param>
		internal void UnregisterTreeNode(TreeNode node){
			this.SelectedNodes.Remove(node);
			if(this.LastMouseNode==node)		this.LastMouseNode=null;
			if(this.LastDragNode==node)			this.set_LastDragNode(null,null);
			if(this.MouseActivatedNode==node)	this.MouseActivatedNode=null;
			if(this.mouseDownNode==node)		this.mouseDownNode=null;
			if(this.focused_node==node)			this.FocusedNode=null;

			//if(this.dragTargetNode==node)		this.dragTargetNode=null;
			//------------------------------------------------------------------
			// 以下の理由を以て dragTargetNode はクリアしない
			// 0. dragTargetNode を null にしただけでは DD 操作は正常に終了出来ない。
			// 1. dragTargetNode は何れ DD 操作が終わる時に解除される。
			// 2. 複数選択して drag している時には、dragTargetNode に登録されず接触する事が出来ない様になっている。
			//    単一選択の時だけ、DD 中に DD が無効になる様な動作は変である。
			// 3. そもそも DD 中にいつの間にかに DD が終わっているというのは止めた方が良い。
			//------------------------------------------------------------------
		}
#if OLD
		/// <summary>
		/// TreeView から指定した TreeNode を削除します。
		/// 現在選択されたり処理の対象になっている場合には、其処からも解除します。
		/// </summary>
		/// <param name="node">TreeView から削除する node を指定します。</param>
		internal void UnregisterRemovedNode(TreeNode node){
			if(node.View!=this)
				throw new TreeNodeException("指定された TreeNode はこの TreeView に登録されているノードではありません。");

			// 先に選択は全て解除
			_todo.TreeNodeSelectionChanged("現在二回発生する。一回だけで充分。SelectedNodes に Suppress** を追加。");
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
		/// TreeView から、指定したノードの子ノードを全て解除します。
		/// </summary>
		/// <param name="node">子ノードを全削除する親ノードを指定します。</param>
		internal void UnregisterChildrenNodes(TreeNode node){
			if(node==this.root){
				this.UnregisterAllNodes();
			}

			if(node.View!=this)
				throw new TreeNodeException("指定された TreeNode はこの TreeView に登録されているノードではありません。");

			this.SelectedNodes.RemoveDescendants(node);
			if(this.HasChildren)foreach(TreeNode child in node.Nodes)
				this.UnregisterRemovedNode_Unregister(child);
		}
		/// <summary>
		/// TreeView の中の全てのノードを解除します。
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
	/// 既定のノードの設定を管理するクラスです。
	/// </summary>
	//[CM::TypeConverter(typeof(Design.TreeNodeSettingsConverter))]
	public partial class TreeNodeSettings{
		/// <summary>
		/// TreeNodeSettings クラスのコンストラクタです。
		/// </summary>
		public TreeNodeSettings(){}

		/*
		private int childIndent=18;
		/// <summary>
		/// 子ノードのインデントを取得又は設定します。
		/// </summary>
		public int ChildIndent{
			get{return this.childIndent;}
			set{this.childIndent=value;}
		}

		private int height=14;
		/// <summary>
		/// そのノードの高さを指定します。
		/// </summary>
		public int Height{
			get{return this.height;}
			set{this.height=value;}
		}
		//*/
	}

	#region Events
	/// <summary>
	/// プロパティが変化した際に発生するイベントの情報を提供します。
	/// </summary>
	public sealed class TreeNodePropertyChangingEventArgs<T>:System.EventArgs{
		T oldState;
		T newState;
		internal TreeNodePropertyChangingEventArgs(T oldState,T newState){
			this.oldState=oldState;
			this.newState=newState;
		}
		/// <summary>
		/// プロパティが変化する前の値を取得します。
		/// </summary>
		public T OldValue{
			get{return this.oldState;}
		}
		/// <summary>
		/// プロパティが変化した後の値を取得します。
		/// </summary>
		public T NewValue{
			get{return this.newState;}
		}
	}
	/// <summary>
	/// TreeNode に関するイベントを処理する関数を表します。
	/// </summary>
	/// <param name="sender">イベントの発生源を指定します。</param>
	/// <param name="e">イベントに関する情報を提供します。</param>
	public delegate void TreeNodeEventHandler(object sender,TreeNodeEventArgs e);
	/// <summary>
	/// TreeNode に関する Event の情報を提供します。
	/// </summary>
	public class TreeNodeEventArgs:System.EventArgs{
		private readonly TreeNode node;
		private bool handled;
		/// <summary>
		/// TreeNodeEventArgs クラスを初期化します。
		/// </summary>
		/// <param name="node">イベントの発生に関与した TreeNode を指定します。</param>
		public TreeNodeEventArgs(TreeNode node):base(){
			this.node=node;
			this.handled=false;
		}
		/// <summary>
		/// イベントに直接関係をしたノードを返します。
		/// </summary>
		public TreeNode Node{
			get{return this.node;}
		}
		/// <summary>
		/// ノードを保持している TreeView を取得します。
		/// ノードが所属している TreeView が存在しない場合には null を返します。
		/// </summary>
		public TreeView View{
			get{return this.node.View;}
		}
		/// <summary>
		/// このイベントに対する処理が終了したか否かを取得又は設定します。
		/// true に設定すると、以降に本来行われる筈だった動作 (MouseDown 後のノードの選択など) を全て省略します。
		/// </summary>
		public bool Handled{
			get{return this.handled;}
			set{this.handled=value;}
		}
	}
	/// <summary>
	/// TreeNode 上でのマウスに関するイベントを処理する関数を表します。
	/// </summary>
	/// <param name="sender">イベントの発生源を指定します。</param>
	/// <param name="e">イベントに関する情報を提供します。</param>
	public delegate void TreeNodeMouseEventHandler(object sender,TreeNodeMouseEventArgs e);
	/// <summary>
	/// TreeNode 上でのマウスイベントに関する情報を提供します。
	/// </summary>
	public sealed class TreeNodeMouseEventArgs:TreeNodeEventArgs{
		readonly Gdi::Point location;
		readonly System.Windows.Forms.MouseButtons button;
		readonly int clicks;
		readonly TreeNodeHitType type;
		readonly int delta;
		/// <summary>
		/// System.Windows.Forms.MouseEventArgs を元に、TreeNodeMouseEventArgs インスタンスを作成します。
		/// </summary>
		/// <param name="node">イベントの発生の対象を指定します。</param>
		/// <param name="type">現在のマウスの位置が node に対してどの様な位置にあるかを指定します。</param>
		/// <param name="e">マウスのイベントに関する情報を指定します。</param>
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
		/// 指定した TreeView と System.Windows.Forms.MouseEventArgs から、イベントに関連する TreeNode を検索して
		/// TreeNodeMouseEventArgs のインスタンスを作成します。
		/// </summary>
		/// <param name="view">イベントが発生した TreeView を指定します。</param>
		/// <param name="e">マウスイベントに関する情報を保持している MouseEventArgs インスタンスを指定します。</param>
		/// <returns>イベントに関連する TreeNode を元に作成した TreeNodeMouseEventArgs インスタンスを返します。
		/// 関連する TreeNode が存在しなかった場合には null を返します。</returns>
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
		/// イベント発生時のマウスの座標 (TreeView 座標系) を取得します。
		/// </summary>
		public Gdi::Point ClientLocation{
			get{return this.location;}
		}
		/// <summary>
		/// イベント発生時のマウスの X 座標 (TreeView 座標系) を取得します。
		/// </summary>
		public int ClientX{
			get{return this.location.X;}
		}
		/// <summary>
		/// イベント発生時のマウスの Y 座標 (TreeView 座標系) を取得します。
		/// </summary>
		public int ClientY{
			get{return this.location.Y;}
		}
		/// <summary>
		/// 連続してクリックした回数を取得します。
		/// MouseDown イベント等で利用可能です。
		/// </summary>
		public int Clicks{
			get{return this.clicks;}
		}
		/// <summary>
		/// イベント発生時に押されていたマウスのボタンを取得します。
		/// </summary>
		public System.Windows.Forms.MouseButtons MouseButton{
			get{return this.button;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードのどの部分に相当するかを表す値を取得します。
		/// </summary>
		public TreeNodeHitType HitType{
			get{return this.type;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードに対してどの垂直位置にあるかを表す値を取得します。
		/// <see cref="TreeNodeHitType.BorderTop"/>, <see cref="TreeNodeHitType.Middle"/>,
		/// <see cref="TreeNodeHitType.BorderBottom"/> の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeVertical{
			get{return this.type&TreeNodeHitType.mask_vertical_hit;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードに対してどの水平位置にあるかを表す値を取得します。
		/// <see cref="TreeNodeHitType.CheckBox"/>, <see cref="TreeNodeHitType.Icon"/>,
		/// <see cref="TreeNodeHitType.Content"/>, <see cref="TreeNodeHitType.IndentArea"/>,
		/// <see cref="TreeNodeHitType.PaddingLeft"/>, <see cref="TreeNodeHitType.PaddingRight"/>
		/// の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeHorizontal{
			get{return this.type&TreeNodeHitType.mask_horizontal;}
		}
		/// <summary>
		/// マウスポインタが、ノードの特定部分の上にあるかどうかを取得します。
		/// <see cref="TreeNodeHitType.OnPlusMinus"/>, <see cref="TreeNodeHitType.OnCheckBox"/>
		/// <see cref="TreeNodeHitType.OnIcon"/>, <see cref="TreeNodeHitType.OnContent"/>
		/// の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeOn{
			get{return this.type&TreeNodeHitType.mask_on_something;}
		}
		/// <summary>
		/// スクロール量のデルタ値を取得します。
		/// </summary>
		public int ScrollDelta{
			get{return this.delta;}
		}
		/// <summary>
		/// スクロール量を行数で取得します。
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
	/// TreeNode のドラッグに関するイベントを処理する関数を表します。
	/// </summary>
	/// <param name="sender">イベントの発生源を指定します。</param>
	/// <param name="e">イベントに関する情報を提供します。</param>
	public delegate void TreeNodeDragEventHandler(object sender,TreeNodeDragEventArgs e);
	/// <summary>
	/// TreeNode のドラッグに関する Event の情報を提供します。
	/// </summary>
	public sealed class TreeNodeDragEventArgs:TreeNodeEventArgs{
		readonly Forms::DragEventArgs e;
		readonly TreeNodeHitType type;
		readonly Gdi::Point location;
		/// <summary>
		/// 指定した TreeNode と TreeNodeHitType 及び System.Windows.Forms.DragEventArgs を元にして、
		/// TreeNodeDragDropEventArgs インスタンスを初期化します。
		/// </summary>
		/// <param name="node">Drag 先のノードを指定します。</param>
		/// <param name="type">マウスがノード上のどの位置に存在しているかを指定します。</param>
		/// <param name="e">Drag イベントに関する情報を取得します。</param>
		public TreeNodeDragEventArgs(TreeNode node,TreeNodeHitType type,Forms::DragEventArgs e):base(node){
			this.e=e;
			this.type=type;
			this.location=new Gdi::Point(e.X,e.Y);
		}
		/// <summary>
		/// 指定した TreeView と System.Windows.Forms.DragEventArgs から、イベントに関連する TreeNode を検索して
		/// TreeNodeDragDropEventArgs のインスタンスを作成します。
		/// </summary>
		/// <param name="view">イベントが発生した TreeView を指定します。</param>
		/// <param name="e">ドラッグイベントに関する情報を保持している DragEventArgs インスタンスを指定します。</param>
		/// <returns>イベントに関連する TreeNode を元に作成した TreeNodeDragDropEventArgs インスタンスを返します。
		/// 関連する TreeNode が存在しなかった場合には null を返します。</returns>
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
		/// イベント発生時のマウスの座標 (TreeView 座標系) を取得します。
		/// </summary>
		public Gdi::Point ClientLocation{
			get{return this.location;}
		}
		/// <summary>
		/// イベント発生時のマウスの X 座標 (TreeView 座標系) を取得します。
		/// </summary>
		public int ClientX{
			get{return this.location.X;}
		}
		/// <summary>
		/// イベント発生時のマウスの Y 座標 (TreeView 座標系) を取得します。
		/// </summary>
		public int ClientY{
			get{return this.location.Y;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードのどの部分に相当するかを表す値を取得します。
		/// </summary>
		public TreeNodeHitType HitType{
			get{return this.type;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードに対してどの垂直位置にあるかを表す値を取得します。
		/// <see cref="TreeNodeHitType.BorderTop"/>, <see cref="TreeNodeHitType.Middle"/>,
		/// <see cref="TreeNodeHitType.BorderBottom"/> の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeVertical{
			get{return this.type&TreeNodeHitType.mask_vertical_hit;}
		}
		/// <summary>
		/// マウスポインタの位置が、ノードに対してどの水平位置にあるかを表す値を取得します。
		/// <see cref="TreeNodeHitType.CheckBox"/>, <see cref="TreeNodeHitType.Icon"/>,
		/// <see cref="TreeNodeHitType.Content"/>, <see cref="TreeNodeHitType.IndentArea"/>,
		/// <see cref="TreeNodeHitType.PaddingLeft"/>, <see cref="TreeNodeHitType.PaddingRight"/>
		/// の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeHorizontal{
			get{return this.type&TreeNodeHitType.mask_horizontal;}
		}
		/// <summary>
		/// マウスポインタが、ノードの特定部分の上にあるかどうかを取得します。
		/// <see cref="TreeNodeHitType.OnPlusMinus"/>, <see cref="TreeNodeHitType.OnCheckBox"/>
		/// <see cref="TreeNodeHitType.OnIcon"/>, <see cref="TreeNodeHitType.OnContent"/>
		/// の何れかの値になります。
		/// </summary>
		public TreeNodeHitType HitTypeOn{
			get{return this.type&TreeNodeHitType.mask_on_something;}
		}
		/// <summary>
		/// ドラッグ元で許可されているドロップ操作を取得します。
		/// </summary>
		public Forms::DragDropEffects AllowedEffect{
			get{return e.AllowedEffect;}
		}
		/// <summary>
		/// ドロップ先で許可するドロップ操作を取得または設定します。
		/// </summary>
		public Forms::DragDropEffects Effect{
			get{return e.Effect;}
			set{e.Effect=value;}
		}
		/// <summary>
		/// ドラッグされているデータの情報を取得します。
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
		/// マウスボタン、修飾キーの状態を表す整数を取得します。
		/// </summary>
		public int KeyState{
			get{return e.KeyState;}
		}
		/// <summary>
		/// マウスの左ボタンが押されているかどうかを取得します。
		/// </summary>
		public bool MouseLeft{get{return (e.KeyState&KEY_MLEFT)!=0;}}
		/// <summary>
		/// マウスの右ボタンが押されているかどうかを取得します。
		/// </summary>
		public bool MouseRight{get{return (e.KeyState&KEY_MRIGHT)!=0;}}
		/// <summary>
		/// マウスの中ボタンが押されているかどうかを取得します。
		/// </summary>
		public bool MouseMiddle{get{return (e.KeyState&KEY_MMIDDLE)!=0;}}
		/// <summary>
		/// Ctrl キーが押されているかどうかを取得します。
		/// </summary>
		public bool KeyCtrl{get{return (e.KeyState&KEY_CTRL)!=0;}}
		/// <summary>
		/// Alt キーが押されているかどうかを取得します。
		/// </summary>
		public bool KeyAlt{get{return (e.KeyState&KEY_ALT)!=0;}}
		/// <summary>
		/// Shift キーが押されているかどうかを取得します。
		/// </summary>
		public bool KeyShift{get{return (e.KeyState&KEY_SHIFT)!=0;}}
	}
	/*
	/// <summary>
	/// TreeNode 上でのドラッグに関する情報を提供します。
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
	/// 選択されている TreeNode の集合を管理します。
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
			// ReDraw を IsSelectedChanged の側で実行してしまった場合 or 実行する必要がない場合
			node.OnIsSelectedChanged(new TreeNodePropertyChangingEventArgs<bool>(true,false));
			node.ReDraw(false);
		}
		/// <summary>
		/// 選択されているノードの再描画を行います。
		/// </summary>
		internal void ReDraw(){
			foreach(TreeNode snode in this.nodes){
				snode.ReDraw(false);
			}
		}
		//============================================================c
		//		設定
		//============================================================c
		/// <summary>
		/// 指定した TreeNode だけを選択した状態にします。
		/// </summary>
		/// <param name="node">選択したい TreeNode を指定します。</param>
		/// <param name="suppressRedraw">再描画を抑制するか否かを指定します。
		/// 自分で再描画を行う場合には true を指定して下さい。</param>
		public void Set(TreeNode node,bool suppressRedraw){
			if(node==null){
				this.Clear();
				return;
			}

			Gen::List<TreeNode> oldNodes=this.nodes;
			if(oldNodes.Count==1&&oldNodes[0]==node)return;

			// 設定
			this.nodes=new Gen::List<TreeNode>();
			this.nodes.Add(node);
			
			// 解除イベント
			bool contained=false;
			foreach(TreeNode snode in oldNodes){
				if(snode==node){
					contained=true;
					continue;
				}
				OnUnselected(snode);
			}

			// 選択イベント
			if(!contained){
				if(suppressRedraw)
					OnSelectedNoRedraw(node);
				else
					OnSelected(node);
			}

			this.view.OnSelectionChanged();
		}
		/// <summary>
		/// 指定した TreeNode だけを選択した状態にします。
		/// </summary>
		/// <param name="node">選択したい TreeNode を指定します。</param>
		public void Set(TreeNode node){
			this.Set(node,false);
		}
		/// <summary>
		/// 指定した TreeNode 集合を選択した状態にします。
		/// </summary>
		/// <param name="nodeCollection">選択される TreeNode の集合を指定します。</param>
		public void Set(Gen::IEnumerable<TreeNode> nodeCollection){
			Gen::List<TreeNode> oldNodes=this.nodes;
			bool changed=false;

			this.nodes=new Gen::List<TreeNode>();
			foreach(TreeNode addee in nodeCollection){
				nodes.Add(addee);

				int index=oldNodes.IndexOf(addee);
				if(index>=0){
					// 元から選択 (解除イベント回避)
					oldNodes[index]=null;
				}else{
					changed=true;

					// 新規選択
					OnSelected(addee);
				}
			}

			// 解除イベント
			foreach(TreeNode removed in oldNodes){
				if(removed==null)continue;
				OnUnselected(removed);
				changed=true;
			}

			if(changed)this.view.OnSelectionChanged();
		}
		//============================================================
		//		追加・削除
		//============================================================
		/// <summary>
		/// 指定した TreeNode を選択された状態にします。
		/// </summary>
		/// <param name="node">選択する TreeNode を指定します。</param>
		public void Add(TreeNode node){
			if(this.nodes.Contains(node))return;
			this.nodes.Add(node);
			OnSelected(node);
			this.view.OnSelectionChanged();
		}
		/// <summary>
		/// 指定した TreeNode の選択を解除します。
		/// 指定した TreeNode が元から選択されていない場合には何もしません。
		/// </summary>
		/// <param name="node">選択を解除したい TreeNode を指定します。</param>
		/// <returns>指定した TreeNode が選択されていて、選択が解除された場合に true を返します。
		/// 元から選択されていなかった場合には、false を返します。</returns>
		public bool Remove(TreeNode node){
			bool ret=this.nodes.Remove(node);
			if(ret){
				OnUnselected(node);
				this.view.OnSelectionChanged();
			}
			return ret;
		}
		/// <summary>
		/// 選択されている要素を全て解除します。
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
		/// 指定した TreeNode の子孫のノードの選択を解除します。
		/// 指定した TreeNode 自体の選択は解除されません。
		/// </summary>
		/// <param name="node">子孫のノードの選択を解除する TreeNode を指定します。</param>
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
		//		選択反転
		//============================================================
		/// <summary>
		/// 指定した TreeNode の選択状態を切り替えます。
		/// 指定した TreeNode が選択された状態にあった場合には、選択を解除します。
		/// 指定した TreeNode が選択されていなかった場合には、選択された状態にします。
		/// </summary>
		/// <param name="node">選択状態を切り替える対象の Node を指定します。</param>
		/// <returns>切替後の選択状態を返します。
		/// 選択された状態になった場合には、true を返します。
		/// 選択が解除された場合、または、指定した TreeNode がこの TreeView に属する TreeNode でなかった場合に false を返します。
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
		/// 指定した集合に含まれる TreeNode の選択状態を反転します。
		/// </summary>
		/// <param name="nodeCollection">選択状態を反転する TreeNode の集合を指定します。</param>
		public void Switch(Gen::IEnumerable<TreeNode> nodeCollection){
			bool changed=false;

			foreach(TreeNode node in nodeCollection){
				if(node.View!=this.view)continue;

				if(this.nodes.Contains(node)){
					// 選択解除
					if(!this.nodes.Remove(node))continue;
					OnUnselected(node);
				}else{
					// 選択
					this.nodes.Add(node);
					OnSelected(node);
				}

				changed=true;
			}

			if(changed)
				this.view.OnSelectionChanged();
		}
		//============================================================
		//		範囲追加・削除
		//============================================================
		/// <summary>
		/// 指定した範囲の TreeNode を選択します。
		/// </summary>
		/// <param name="axis">選択の軸となる TreeNode を指定します。
		/// null を指定すると node のみが選択されます。
		/// null 以外を指定すると axis から node 迄の TreeNode が選択されます。
		/// </param>
		/// <param name="node">選択の対象となる TreeNode を指定します。
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
		/// 指定した範囲の TreeNode の選択状態を反転します。
		/// 具体的には、axis から node 迄の TreeNode の選択状態を反転させます。
		/// </summary>
		/// <param name="axis">選択の軸となる TreeNode を指定します。
		/// axis 自体の選択状態は反転しません。
		/// null を指定すると node の選択状態のみが反転します。
		/// </param>
		/// <param name="node">選択反転の対象となる TreeNode を指定します。</param>
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
		//		アクセス
		//============================================================c
		/// <summary>
		/// 指定した TreeNode が、
		/// このインスタンスに関連付けられている TreeView 内で、
		/// 選択されているか否かを取得します。
		/// </summary>
		/// <param name="node">選択されているか否かを取得したい TreeNode を指定します。</param>
		/// <returns>指定した TreeNode が選択されていた場合に true を返します。
		/// それ以外の場合に false を返します。</returns>
		public bool Contains(TreeNode node){
			return node.View==this.view&&this.nodes.Contains(node);
		}
		/// <summary>
		/// 選択されている TreeNode の集合を配列にコピーします。
		/// </summary>
		/// <param name="array">選択されている TreeNode の格納先の配列インスタンスを指定します。</param>
		/// <param name="arrayIndex">array 内で格納を開始する位置を指定します。
		/// 指定した位置に選択されている要素の第一要素が格納されます。</param>
		public void CopyTo(TreeNode[] array,int arrayIndex){
			if(array==null)throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex>=array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex");
			if(arrayIndex+this.Count>array.Length)
				throw new System.ArgumentException("指定した配列の容量が足りません。","array");

			foreach(TreeNode node in nodes)
				array[arrayIndex++]=node;
		}
		/// <summary>
		/// 選択されている TreeNode の数を取得します。
		/// </summary>
		public int Count{
			get{return this.nodes.Count;}
		}
		/// <summary>
		/// 選択されている項目が読み取り専用か否かを取得します。
		/// SelectedNodes では常に変更が可能なので、このプロパティは常に false を返します。
		/// </summary>
		public bool IsReadOnly{
			get{return false;}
		}
		/// <summary>
		/// 選択されている TreeNode の列挙子を取得します。
		/// </summary>
		/// <returns>選択されている TreeNode の列挙子を作成して返します。</returns>
		public Gen::IEnumerator<TreeNode> GetEnumerator(){
			return this.nodes.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
	}
	#endregion
}
