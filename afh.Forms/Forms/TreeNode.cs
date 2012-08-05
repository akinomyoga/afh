using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2=System.Drawing.Drawing2D;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Ser=System.Runtime.Serialization;
using Forms=System.Windows.Forms;

using BitSection=afh.Collections.BitSection;
//using TreeNodeMouseEH=afh.EventHandler<TreeNode,TreeNodeMouseEventArgs>;

namespace afh.Forms{
	/// <summary>
	/// 或る点が、或るノードに対してどの様な位置関係にあるかを示します。
	/// </summary>
	[System.Flags]
	public enum TreeNodeHitType{
		//
		//	高さ判定
		//
		/// <summary>[高さ] ノード領域よりも上にある事を示します。</summary>
		Above		=0x0100,
		/// <summary>[高さ] ノード領域の上端近くにある事を示します。</summary>
		BorderTop	=0x0200,
		/// <summary>[高さ] ノード領域の真ん中の高さにある事を示します。</summary>
		Middle		=0x0400,
		/// <summary>[高さ] ノード領域の下端近くにある事を示します。</summary>
		BorderBottom=0x0800,
		/// <summary>[高さ] ノード領域よりも下にある事を示します。</summary>
		Below		=0x1000,
		//
		//	横位置判定
		//
		/// <summary>[横位置] インデント領域の横位置にある事を示します。</summary>
		IndentArea	=0x01,
		/// <summary>[横位置] チェックボックスの横位置にある事を示します。</summary>
		CheckBox	=0x02,
		/// <summary>[横位置] アイコンの横位置にある事を示します。</summary>
		Icon		=0x04,
		/// <summary>[横位置] ノード内容表示領域の横位置にある事を示します。</summary>
		Content		=0x08,
		/// <summary>[横位置] ノード領域よりも左にある事を示します。</summary>
		PaddingLeft	=0x10,
		/// <summary>[横位置] ノード領域よりも右にある事を示します。</summary>
		PaddingRight=0x20,
		//
		//	当たり判定
		//
		/// <summary>[当たり] ノードのインデント領域上にある事を示します。</summary>
		OnPlusMinus	=IndentArea	<<16, // 0x010000
		/// <summary>[当たり] ノードのチェックボックス領域上にある事を示します。</summary>
		OnCheckBox	=CheckBox	<<16, // 0x020000
		/// <summary>[当たり] ノードのアイコン領域上にある事を示します。</summary>
		OnIcon		=Icon		<<16, // 0x040000
		/// <summary>[当たり] ノード内容表示領域上にある事を示します。</summary>
		OnContent	=Content	<<16, // 0x080000
		/// <summary>[当たり] ノード上の何処かにある事を示します。</summary>
		Hit			=0x100000,
		//
		//	他
		//
		/// <summary>[マスク] ノード領域と同じ高さにある事を示します。</summary>
		mask_vertical_hit		=BorderTop|Middle|BorderBottom,
		/// <summary>[マスク] 横位置を判定する為のマスクです。</summary>
		mask_horizontal			=mask_horizontal_pmhit|PaddingLeft|PaddingRight,
		/// <summary>[マスク] ノード領域と同じ横位置にある事を示します。</summary>
		mask_horizontal_hit		=CheckBox|Icon|Content,
		/// <summary>[マスク] ノード領域・インデント領域と同じ横位置にある事を示します。</summary>
		mask_horizontal_pmhit	=IndentArea|mask_horizontal_hit,
		/// <summary>[マスク] ノードの特定の領域を判定する為のマスクです。</summary>
		mask_on_something		=OnPlusMinus|OnCheckBox|OnIcon|OnContent,
	}

	/// <summary>
	/// TreeNode の Checked 状態の管理に使用します。
	/// </summary>
	[System.Flags]
	public enum TreeNodeCheckedState{
		/// <summary>
		/// CheckBox に check が入っていない状態を示します。
		/// </summary>
		Unchecked=0,
		/// <summary>
		/// CheckBox に check が入っている状態を示します。
		/// </summary>
		Checked=1,
		/// <summary>
		/// CheckBox の Check 状態が中間である事を示します。
		/// </summary>
		Intermediate=2,
	}
	/// <summary>
	/// 各種設定の継承などの方法を指定するのに使用します。
	/// </summary>
	public enum TreeNodeInheritType{
		/// <summary>
		/// TreeView に設定された既定値を使用します。
		/// </summary>
		Default=0,
		/// <summary>
		/// 親ノードから継承します。
		/// </summary>
		Inherit=1,
		/// <summary>
		/// 固有の情報を使用します。
		/// </summary>
		Custom=2,
	}
	/// <summary>
	/// TreeView に表示する一つ一つの要素を表現するクラスです。
	/// </summary>
	[System.Serializable]
	public partial class TreeNode:System.Runtime.Serialization.ISerializable{
		private TreeView view=null;
		internal TreeNode parent=null;
		private TreeNodeCollection nodes;
		/// <summary>
		/// 拡張メンバの管理に使用します。
		/// </summary>
		protected ExtraMemberCache xmem=new ExtraMemberCache();

		private afh.Collections.BitArray64 bits=default_bits;
		static afh.Collections.BitArray64 default_bits=new afh.Collections.BitArray64();
		private const BitSection sNodeHeightInherit			=(BitSection)(2<<16|0);
		private const BitSection sIconVisibleInherit		=(BitSection)(2<<16|2);
		private const BitSection sIconInherit				=(BitSection)(2<<16|4);
		private const BitSection sIconSizeInherit			=(BitSection)(2<<16|6);
		private const BitSection sCheckBoxInherit			=(BitSection)(2<<16|8);
		private const BitSection sCheckBoxVisibleInherit	=(BitSection)(2<<16|10);
		private const BitSection sBackColorInherit			=(BitSection)(2<<16|12);
		private const BitSection sForeColorInherit			=(BitSection)(2<<16|14);
		private const BitSection sIsEnabledInherit			=(BitSection)(2<<16|16);
		private const BitSection sCheckedState				=(BitSection)(2<<16|18);
		private const BitSection sChildIndentInherit		=(BitSection)(2<<16|20);
		private const BitSection sFontInherit				=(BitSection)(2<<16|22);
		private const int bExpanded				=24;
		private const int bVisible				=25;
		private const int bIconVisible			=26;
		private const int bCheckBoxVisible		=27;
		private const int bCheckBoxEnabled		=28;
		private const int bCheckReflectChildren	=29;
		private const int bIsEnabled			=30;

		private const BitSection sDDBehaviorInherit			=(BitSection)(2<<16|32);
		
		
		/*
		public event TreeNodeMouseEH Clicked;
		public event TreeNodeMouseEH MouseEnter;
		public event TreeNodeMouseEH MouseLeave;
		public event TreeNodeMouseEH MouseHover;
		public event TreeNodeMouseEH MouseMove;
		public event TreeNodeMouseEH MouseDown;
		public event TreeNodeMouseEH MouseUp;
		//*/

		/// <summary>
		/// 子ノードの集合を取得します。
		/// </summary>
		public TreeNodeCollection Nodes{
			get{
				if(this.nodes==null){
					TreeNodeCollection nodes=new TreeNodeCollection(this);
					this.nodes=nodes;
					using(nodes.SuppressLayout())
						this.InitializeNodes(nodes);
				}
				return this.nodes;
			}
		}
		/// <summary>
		/// 子 TreeNode 集合を初期化します。
		/// </summary>
		/// <param name="nodes">ノードリストを指定します。</param>
		protected virtual void InitializeNodes(TreeNodeCollection nodes){
			/*
			/// <returns>初期化した結果の TreeNodeCollection を返します。</returns>
			/// <remarks>子要素を遅延的に初期化したい場合にはこのメソッドを override して下さい。
			/// override した際は base.InitializeNodes() でインスタンスを作成し、
			/// 要素を追加してから返す様にすると良いでしょう。
			/// </remarks>
			*/
		}
		/// <summary>
		/// この TreeNode が子要素を保持しているか否かを取得します。
		/// </summary>
		/// <remarks>子要素を遅延的に初期化したい場合には、
		/// このプロパティを override して true を返す様にしましょう。</remarks>
		public virtual bool HasChildren{
			get{return this.nodes!=null&&this.nodes.Count>0;}
		}
		// 渡す情報: View 変更のルートノード・イベントの起こったノード・View 等
		// public event afh.EventHandler<TreeNode,TreeView> ViewChanged;
		// TreeNode に於いて親 View を保持する必要性はあるのか?
		// ×TreeView に追加した際に、子ノードが沢山あると更新が大変
		// →View の取得は毎回親を辿る方がよいかもしれない。
		/// <summary>
		/// 親 TreeView を取得します。何処の TreeView にも属していない時には null を返します。
		/// </summary>
		public TreeView View{
			get{return this.view;}
			internal set{
				if(this.view==value)return;

				// view 置換
				_todo.TreeNodeOptimize("現状 SelectionChanged が登録されている回数だけ発生する。Suppress/Resume を実装する必要?");
				if(this.view!=null)
					this.view.UnregisterTreeNode(this);
				this.view=value;

				// 子ノードに適用
				if(this.HasChildren&&this.nodes!=null)foreach(TreeNode node in Nodes){
					node.View=value;
				}
			}
		}
		/// <summary>
		/// 親 TreeNode を取得します。どの TreeNode にも属していない時には null を返します。
		/// </summary>
		public TreeNode ParentNode{
			get{return this.parent;}
		}
		/// <summary>
		/// このノードを親 TreeView 内で見える位置に移動します。
		/// </summary>
		public void EnsureVisible(){
			if(view==null)throw new TreeViewMissingException();

			// 展開・可視確認
			{
				if(!this.IsVisible)
					throw new TreeNodeException("指定した TreeNode は不可視設定になっているので表示できません。");

				TreeNode ances=this.parent;
				while(ances!=null){
					if(!ances.IsVisible)
						throw new TreeNodeException("先祖に不可視設定になっている物があるので、指定した TreeNode は表示できません。");
					if(!ances.IsExpanded)ances.IsExpanded=true;
					ances=ances.parent;
				}
			}

			Gdi::Point pos=view.AutoScrollPosition;
			pos=new Gdi::Point(-pos.X,-pos.Y); // get_AutoScrollPosition は何故か値が逆

			Gdi::Point pnode=this.LocalPos2ClientPos(Gdi::Point.Empty);

			// 縦
			if(pnode.Y<0){
				pos.Y+=pnode.Y;
			}else{
				int y_over=pnode.Y+this.NodeHeight-view.ClientSize.Height;
				if(y_over>0)pos.Y+=System.Math.Min(pnode.Y,y_over);
			}

			// 横
			if(pnode.X<0){
				pos.X+=pnode.X;
			}else{
				int x_over=pnode.X+this.NodeWidth-view.ClientSize.Width;
				if(x_over>0)pos.X+=System.Math.Min(pnode.X,x_over);
			}

			view.AutoScrollPosition=pos;
		}
		//============================================================
		//		初期化
		//============================================================
		/// <summary>
		/// TreeNode のコンストラクタです。
		/// </summary>
		public TreeNode(){
			// ・継承元の初期化が終わっていない内に呼び出すのは不味い
			// ・計算は TreeView に追加される時に実行されるので茲で行う必要はない (?
			// this.RefreshDisplaySize();
		}
		static TreeNode(){
			default_bits[bVisible]=true;
			default_bits[sCheckBoxVisibleInherit]=(uint)TreeNodeInheritType.Inherit;
			default_bits[bCheckBoxEnabled]=true;

			default_bits[sIsEnabledInherit]=(uint)TreeNodeInheritType.Inherit;
			// 或るノードが無効になったら、その子孫ノードも全て無効になるべきである。

			//default_bits[sChildIndentInherit]=(uint)TreeNodeInheritType.Inherit;
		}
		void Ser::ISerializable.GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context) {
			this.GetObjectData(info,context);
		}
		/// <summary>
		/// このノードをシリアライズします。
		/// </summary>
		/// <param name="info">データの格納先を指定します。</param>
		/// <param name="context">シリアライズに関する情報を指定します。</param>
		protected virtual void GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context) {
			TreeNode[] nodes;
			if(this.HasChildren){
				nodes=new TreeNode[this.nodes.Count];
				this.nodes.CopyTo(nodes,0);
			}else{
				nodes=new TreeNode[0];
			}
			info.AddValue("child-nodes",nodes);
			info.AddValue("extra-member",this.xmem);
			info.AddValue("bits",this.bits);

			// parent : なし (子ノードには自分で設定する)
			// view : なし
			// szDisplay : 再計算
		}
		/// <summary>
		/// シリアライズされた情報からノードを復元します。
		/// </summary>
		/// <param name="info">データの格納先を指定します。</param>
		/// <param name="context">シリアライズに関する情報を指定します。</param>
		protected TreeNode(Ser::SerializationInfo info,Ser::StreamingContext context){
			this.bits=(afh.Collections.BitArray64)info.GetValue("bits",typeof(afh.Collections.BitArray64));
			this.xmem=(ExtraMemberCache)info.GetValue("xmem",typeof(ExtraMemberCache));
			TreeNode[] nodes=(TreeNode[])info.GetValue("child-nodes",typeof(TreeNode[]));
			if(nodes.Length!=0){
				this.nodes=new TreeNodeCollection(this); // InitializeNodes は使用せずに初期化
				this.nodes.AddRange(nodes);
			}

			this.RefreshDisplaySize();
		}

		#region logics: 描画
		//============================================================
		//		描画
		//============================================================
		/// <summary>
		/// この TreeNode の内容を指定した Graphics に描画します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		/// <param name="draw_child">子孫ノードも描画するか否かを取得又は設定します。</param>
		public void DrawTo(Gdi::Graphics g,bool draw_child){
			Gdi2::Matrix orig_trans=g.Transform.Clone();

			this.DrawNode(g);

			if(draw_child&&this.IsExpanded){
				g.Transform=orig_trans;
				g.TranslateTransform(this.ChildIndent.Width,this.NodeHeight);
				this.DrawChildren(g);
			}

			g.Transform=orig_trans;
		}
		private void DrawNode(Gdi::Graphics g){
			int nodeheight=this.NodeHeight;

			/* BackColor */
			if(this.BackColor!=Gdi::Color.Transparent)
			using(Gdi::Brush brush=new Gdi::SolidBrush(this.BackColor)){
				float ox=g.Transform.OffsetX;
				_todo.TreeNodeOptimize("BackColor の種類が Brush の方がよい。その折は、Brush の管理は別の所で。");
				g.FillRectangle(brush,-ox,0,ox+g.ClipBounds.Right,this.NodeHeight);
			}

			Gdi2::Matrix draw_pivot=g.Transform.Clone();

			/* IndentArea */
			if(this.parent!=null){
				this.parent.ChildIndent.DrawIndentArea(g,this,nodeheight);

				TreeNode ascen=parent;
				while(ascen.parent!=null){
					g.TranslateTransform(-ascen.ChildIndent.Width,0);
					ascen.parent.ChildIndent.DrawIndentForDescendant(g,ascen,nodeheight);
					ascen=ascen.parent;
				}

				// 元に戻す
				if(ascen!=parent)g.Transform=draw_pivot;
			}
			
			/* CheckBox */
			if(this.CheckBoxVisible){
				ITreeNodeCheckBox box=this.CheckBox;
				g.TranslateTransform(PADDING_CHKBOX_LEFT,(nodeheight-box.Size.Height+1)/2);

				box.DrawBox(g,this.IsChecked,this.CheckBoxEnabled);

				draw_pivot.Translate(box.Size.Width+PADDING_CHKBOX_LEFT+PADDING_CHKBOX_RIGHT,0);
				g.Transform=draw_pivot;
			}

			/* Icon */
			if(this.IconVisible){
				Gdi::Size szIcon=this.IconSize;
				int y=(nodeheight-szIcon.Height+1/*四捨五入*/)/2;
				Gdi::Rectangle rect=new Gdi::Rectangle(PADDING_ICON_LEFT,y,szIcon.Width,szIcon.Height);
				this.Icon.DrawIcon(g,rect,this);

				draw_pivot.Translate(szIcon.Width+PADDING_ICON_LEFT+PADDING_ICON_RIGHT,0);
				g.Transform=draw_pivot;
			}

			/* Content */{
				Gdi::Size sz=this.ContentSize;
				g.TranslateTransform(PADDING_CONTENT_LEFT,(nodeheight-sz.Height+1)/2);

				this.DrawContent(g);

				draw_pivot.Translate(PADDING_CONTENT_LEFT+sz.Width,0);
				g.Transform=draw_pivot;
			}
		}
		internal void DrawChildren(Gdi::Graphics g){
			if(!this.HasChildren)return;

			int start,end;
			// 先に範囲を絞る
			{
				int iM=this.Nodes.Count; // Nodes の呼び出しで子要素を初期化
				int tM=(int)g.ClipBounds.Top;

				//-- start = min { i | nodes[i].bottom>=clipBounds.top}
				//-- node_t = node[start].top
				int node_t=0,node_b=0;
				int i=0;
				for(;i<iM;i++){
					node_t=node_b;
					node_b+=nodes[i].DisplayHeight;

					if(node_b>=tM)break;
				}
				start=i;

				tM=(int)g.ClipBounds.Bottom;
				g.TranslateTransform(0,node_t);

				// end = min{ i | nodes[i].top>clipBounds.bottom}
				while(node_t<=tM&&i<iM){
					node_t+=nodes[i++].DisplayHeight;
				}
				end=i;
			}

			for(int index=start;index<end;index++){
				TreeNode node=this.nodes[index];
				if(!node.IsVisible)continue;

				// 背景が横方向全体に亙っているので結局描画しなければならない。
				/*
				Gdi::RectangleF rect=new Gdi::RectangleF(Gdi::PointF.Empty,node.DisplaySize);

				_todo.OptimizeTreeNode("交差判定の高速化");
				if(rect.IntersectsWith(g.ClipBounds))
					node.DrawTo(g);
				//*/

				node.DrawTo(g,true);
				g.TranslateTransform(0,node.DisplayHeight);
			}

		}
		//============================================================
		//		再描画要求
		//============================================================
		/// <summary>
		/// この TreeNode を強制的に再描画させます。
		/// </summary>
		protected internal void ReDraw(bool draw_child){
			_todo.TreeNodeOptimize("自分だけ再描画する場合と、子孫も描画する場合");
			if(this.view==null)
				throw new TreeViewMissingException("描画先の TreeView が無いので描画出来ません。");

			if(!this.IsVisibleInTree)return;

			Gdi::Point pDst=this.ClientPosition;
			Gdi::Rectangle rect=new Gdi::Rectangle(pDst,this.DisplaySize);

			// こうしないと DisplaySize が変更される前に表示していたごみの部分が残存する可能性…
			// 具体的には DisplaySize.Width が縮んだ時に、縮む前に表示していた部分を消さなければならない。
			rect.X=0;
			rect.Width=this.view.ClientRectangle.Width;

			//if(!this.view.ClientRectangle.IntersectsWith(rect))return;
			rect.Intersect(this.view.ClientRectangle);
			if(rect.IsEmpty)return;

			_todo.ExamineTreeView("位置を特定し、表示範囲内にあったら描画");
			using(Gdi::Graphics g=this.view.CreateGraphics()){
				g.SetClip(rect);
				g.TranslateTransform(pDst.X,pDst.Y);
//				g.Clear(this.BackColor);
				this.DrawTo(g,draw_child);
			}
		}
		/// <summary>
		/// 木構造の中で描画されるか否かを取得します。
		/// 全ての親が IsVisible 且つ IsExpanded で、自分自身も IsVisible の時に true の値を持ちます。
		/// それ以外の場合に false の値になります。
		/// </summary>
		private bool IsVisibleInTree{
			get{
				if(!this.IsVisible)return false;
				TreeNode descen=this.parent;
				while(descen!=null){
					if(!descen.IsExpanded||!descen.IsVisible)return false;
					descen=descen.parent;
				}
				return true;
			}
		}
		//============================================================
		//		本体の描画
		//============================================================
		/// <summary>
		/// ノード固有の内容を描画します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		protected virtual void DrawContent(Gdi::Graphics g){
			this.DrawContentBackground(g);

			Gdi::Brush brush;
			if(this.IsEnabled){
				brush=this.IsActivated?Gdi::Brushes.White:Gdi::Brushes.Black;
			}else{
				brush=this.IsActivated?Gdi::Brushes.Gray:Gdi::Brushes.Silver;
			}
			
			g.DrawString("<TreeNode>",this.Font,brush,new Gdi::PointF(0,0));
		}
		/// <summary>
		/// ノード本体の標準の背景を描画します。
		/// 標準の背景を使用したい場合には DrawContent の先頭で呼び出して下さい。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		protected void DrawContentBackground(Gdi::Graphics g){
			Gdi::Rectangle rect=new Gdi::Rectangle(Gdi::Point.Empty,this.ContentSize);

			if(this.IsFocused){
				if(this.IsActivated){
					afh.Drawing.GraphicsUtils.FillRectangleReverseDotFramed(
						g,
						this.IsEnabled?Gdi::SystemColors.Highlight:Gdi::Color.LightGray,
						rect);
				}else{
					afh.Drawing.GraphicsUtils.DrawRectangleReverseDotFramed(g,this.BackColor,rect);
				}
			}else if(this.IsActivated){
				g.FillRectangle(
					this.IsEnabled?Gdi::SystemBrushes.Highlight:Gdi::Brushes.LightGray,
					rect);
			}
		}
		#endregion

		#region logics: 幾何処理
		//************************************************************
		//		ノード幾何処理
		//============================================================
		//		ノードの大きさ
		//============================================================
		private const int PADDING_CHKBOX_LEFT=1;
		private const int PADDING_CHKBOX_RIGHT=2;
		private const int PADDING_ICON_LEFT=1;
		private const int PADDING_ICON_RIGHT=2;
		private const int PADDING_CONTENT_LEFT=0;
		//------------------------------------------------------------
		/// <summary>
		/// 子ノードの表示領域も含めた表示サイズを取得します。
		/// </summary>
		internal Gdi::Size DisplaySize{
			get{return new Gdi::Size(this.DisplayWidth,this.DisplayHeight);}
		}
		internal int DisplayWidth{
			get{
				//_todo.OptimizeTreeNode("cache する仕組み");
				int ret=this.NodeWidth;
				if(this.HasChildren&&this.IsExpanded){
					int c=this.Nodes.TotalWidth;
					if(c>ret)ret=c;
				}
				return ret;
			}
		}
		internal int DisplayHeight{
			get{
				//_todo.OptimizeTreeNode("cache する仕組み");
				return this.szDisplay.Height;
			}
		}
		Gdi::Size szDisplay;
		//-------------------------------------------------------------------------
		/// <summary>
		/// DisplaySize の再計算を促します。
		/// </summary>
		/// <returns>
		/// 大きさに変更があった場合に true を返します。
		/// 大きさに変更がなかった場合に false を返します。
		/// </returns>
		/// <remarks>
		/// 特に、true が返された場合 (大きさに変更があった場合) には、描画は上位ノードが担当します。
		/// 従って描画を呼び出し元でする必要はありません。
		/// 一方で、false が返された場合 (大きさに変更がなかった場合) の再描画は呼び出し元で実行して下さい。
		/// </remarks>
		protected internal bool RefreshDisplaySize(){
			bool ischanged=this.recalcDisplaySize();
			if(ischanged){
				_todo.TreeNodeDisplayHeight("親に通知");
				if(this.parent==null){
					if(this.view!=null)
						// root node の時
						//this.view.DrawNodes();
						this.view.Refresh();
				}else if(!this.parent.RefreshDisplaySize()){
					if(this.view!=null)this.ReDraw(true);
				}
			}
			return ischanged;
		}
		private bool recalcDisplaySize(){
			_todo.TreeNodeDisplayHeight("変化を incremental に更新する仕組み");
			// OK> IsVisibleChanged
			// OK> IsExpandedChanged
			// NodeSizeChanged
			//  + IconSize
			//  + CheckBoxSize
			//  + ContentSize
			// [nodes] TotalSizeChanged
			//  + OK> Add/Remove
			//  + OK> [child] DisplaySizeChanged
			// [子孫に亙っての更新]
			//  + View の NodeParams が変更された時
			//  + OK> 追加された時
			Gdi::Size sz=this.szDisplay;

			if(!this.IsVisible){
				this.szDisplay.Width=0;
				this.szDisplay.Height=0;
			}else{
				int h=this.NodeHeight;
				if(this.HasChildren&&this.IsExpanded)
					h+=this.Nodes.TotalHeight;
				this.szDisplay.Height=h;

				int w=this.NodeWidth;
				if(this.HasChildren&&this.IsExpanded){
					int c=this.Nodes.TotalWidth;
					if(c>w)w=c;
				}
				this.szDisplay.Width=w;
			}

			bool ischanged=sz!=szDisplay;
			if(ischanged){
				// 親への通知より先に之をやっておかないと、
				// スクロールバーの出現などで再描画する羽目になる。
				// (関数を分けてしまったのでは今ではこうするしかない)
				this.OnDisplaySizeChanged(sz);
			}
			return ischanged;
		}
		//-------------------------------------------------------------------------
		/// <summary>
		/// 子孫も含めて全ての DisplaySize を更新します。
		/// 子孫に影響する様な基本設定を変更した場合などに呼び出します。
		/// </summary>
		protected internal bool RefreshDisplaySizeAll(){
			bool ischanged=this.recalcDisplaySizeAll();
			if(ischanged){
				_todo.TreeNodeDisplayHeight("親に通知");
				if(this.parent==null){
					if(this.view!=null)
						// root node の時
						this.view.Refresh();
				}else if(!this.parent.RefreshDisplaySize()){
					if(this.view!=null)this.ReDraw(true);
				}
			}
			return ischanged;
		}
		private bool recalcDisplaySizeAll(){
			bool changed=false;

			// ↓茲では Node 呼び出しによって殊更に InitializeNodes を実行する必要はない
			if(this.nodes!=null)foreach(TreeNode node in this.nodes){
				if(node.recalcDisplaySizeAll())
					changed=true;
			}

			if(this.recalcDisplaySize())changed=true;

			return changed;
		}
		//-------------------------------------------------------------------------
		/// <summary>
		/// 新しく追加された子ノードのサイズ再計算を纏めて実行し、
		/// 最後に全体の再描画を実行します。
		/// </summary>
		/// <returns>再描画が実行されたか否かを返します。</returns>
		protected internal bool RefreshDisplaySizeNewChildren(Gen::IEnumerable<TreeNode> list){
			int c=0;
			foreach(TreeNode node in list){
				if(node==this)
					c++;
				else if(node.parent!=this)
					continue;
				else{
					c++;
					node.recalcDisplaySizeAll();
				}
			}
			if(c==0)return false;

			bool fDrawn=false;
			if(this.recalcDisplaySize())
				fDrawn=this.parent!=null&&this.parent.RefreshDisplaySize();
			if(!fDrawn&&this.view!=null)this.ReDraw(true);

			return true;
		}
		//-------------------------------------------------------------------------
		// event: DisplaySizeChanged → [pre]
		private void OnDisplaySizeChanged(Gdi::Size szOld){
			_todo.TreeNode("変化した理由を通知できるようにする。例えば expanded, visible-changed ...");
			this.OnDisplaySizeChanged(
				new TreeNodePropertyChangingEventArgs<System.Drawing.Size>(szOld,this.szDisplay)
				);
		}
		//------------------------------------------------------------
		/// <summary>
		/// このノードの内容を表示する領域の大きさを取得します。
		/// </summary>
		public virtual Gdi::Size ContentSize{
			get{return new Gdi::Size(100,12);}
		}
		/// <summary>
		/// このノードの内容部分を表示する位置を取得します。
		/// </summary>
		public Gdi::Point ContentOffset{
			get{return new Gdi::Point(this.ContentOffsetX,this.ContentOffsetY);}
		}
		/// <summary>
		/// このノードの内容部分の左余白を取得します。
		/// </summary>
		public int ContentOffsetX{
			get{
				int ret=PADDING_CONTENT_LEFT;
				if(this.CheckBoxVisible)
					ret+=PADDING_CHKBOX_LEFT
						+PADDING_CHKBOX_RIGHT
						+this.CheckBox.Size.Width;
				if(this.IconVisible)
					ret+=PADDING_ICON_LEFT
						+PADDING_ICON_RIGHT
						+this.IconSize.Width;
				return ret;
			}
		}
		/// <summary>
		/// このノードの内容部分の上余白を取得します。
		/// </summary>
		public int ContentOffsetY{
			get{return 0;}
		}
		//------------------------------------------------------------
		/// <summary>
		/// このノードの表示領域の大きさを取得します。
		/// 子ノードの領域の大きさは含みません。
		/// </summary>
		public Gdi::Size NodeSize{
			get{return new Gdi::Size(this.NodeWidth,this.NodeHeight);}
		}
		/// <summary>
		/// このノードの幅を取得します。
		/// ノードの幅は ContentSize に依存して大きくなったり小さくなったりします。
		/// </summary>
		public int NodeWidth{
			get{
				int ret=PADDING_CONTENT_LEFT
					+this.ContentSize.Width;
				if(this.CheckBoxVisible)
					ret+=PADDING_CHKBOX_LEFT
						+PADDING_CHKBOX_RIGHT
						+this.CheckBox.Size.Width;
				if(this.IconVisible)
					ret+=PADDING_ICON_LEFT
						+PADDING_ICON_RIGHT
						+this.IconSize.Width;
				return ret;
			}
		}
		// NodeHeight -> [pre]
		//------------------------------------------------------------
		// ChildIndent -> [pre]
		//============================================================
		//		ノードの局所座標
		//============================================================
		/// <summary>
		/// このノードの論理的位置を取得します。
		/// </summary>
		public Gdi::Point LogicalPosition{
			get{
				if(this.view==null)
					throw new TreeViewMissingException();

				TreeNode node=this;
				Gdi::Point p=Gdi::Point.Empty;
				while(node.parent!=null){
					p=node.LocalPos2ParentLocalPos(p);
					node=node.parent;
				}
				Diag::Debug.Assert(node==this.view.root);
				return p;
			}
		}
		/// <summary>
		/// このノードのクライアント座標位置を取得します。
		/// </summary>
		public Gdi::Point ClientPosition{
			get{
				if(this.view==null)
					throw new TreeViewMissingException();
				return this.view.LogicalPos2ClientPos(this.LogicalPosition);
			}
		}
		/// <summary>
		/// 自ノードの座標系で表された位置を、親ノードの座標系で表現します。
		/// </summary>
		/// <param name="pos">このノードの座標系で表現された位置を指定します。</param>
		/// <returns>指定した位置を親ノードの座標系で表現した値を返します。</returns>
		public Gdi::Point LocalPos2ParentLocalPos(Gdi::Point pos){
			if(this.parent==null)
				throw new TreeNodeMissingParentException();

			// _todo.TreeNode_Visible("表示している物についてだけ高さを積算");
			// 表示していない物の DisplayHeight を 0 にした

			TreeNodeCollection c=this.parent.Nodes;

			Gdi::Point ret=new Gdi::Point(pos.X+this.parent.ChildIndent.Width,pos.Y+this.parent.NodeHeight);
			for(int i=0,iM=this.IndexInSiblings;i<iM;i++){
				ret.Y+=c[i].DisplayHeight;
			}
			return ret;
		}
		/// <summary>
		/// TreeView の表示座標系からこのノードの局所座標系に変換を行います。
		/// </summary>
		/// <param name="pos">TreeView の表示座標系で表された点を指定します。</param>
		/// <returns>指定した位置をこのノードの局所座標系で表した値を返します。</returns>
		public Gdi::Point ClientPos2LocalPos(Gdi::Point pos){
			Gdi::Point orig=this.ClientPosition;
			return new Gdi::Point(pos.X-orig.X,pos.Y-orig.Y);
		}
		/// <summary>
		/// TreeView の論理座標系からこのノードの局所座標系に変換を行います。
		/// </summary>
		/// <param name="pos">TreeView の論理座標系で表された点を指定します。</param>
		/// <returns>指定した位置をこのノードの局所座標系で表した値を返します。</returns>
		public Gdi::Point LogicalPos2LocalPos(Gdi::Point pos){
			Gdi::Point orig=this.LogicalPosition;
			return new Gdi::Point(pos.X-orig.X,pos.Y-orig.Y);
		}
		/// <summary>
		/// このノードの局所座標系から TreeView の表示座標系に変換を行います。
		/// </summary>
		/// <param name="pos">このノードの局所座標系で表された点を指定します。</param>
		/// <returns>指定した位置を TreeView の表示座標系で表した値を返します。</returns>
		public Gdi::Point LocalPos2ClientPos(Gdi::Point pos){
			Gdi::Point orig=this.ClientPosition;
			return new Gdi::Point(pos.X+orig.X,pos.Y+orig.Y);
		}
		/// <summary>
		/// このノードの局所座標系から TreeView の論理座標系に変換を行います。
		/// </summary>
		/// <param name="pos">このノードの局所座標系で表された点を指定します。</param>
		/// <returns>指定した位置を TreeView の論理座標系で表した値を返します。</returns>
		public Gdi::Point LocalPos2LogicalPos(Gdi::Point pos){
			Gdi::Point orig=this.LogicalPosition;
			return new Gdi::Point(pos.X+orig.X,pos.Y+orig.Y);
		}
		//============================================================
		//		当たり判定
		//============================================================
		/// <summary>
		/// 指定した座標位置に存在する子孫ノード又は自ノードを取得します。
		/// </summary>
		/// <param name="p">ノードを検索する座標位置を指定します。</param>
		/// <param name="type">指定した位置にノードが存在した場合には、
		/// 指定した位置が具体的にそのノードの何処に当たるかを返します。
		/// それ以外の場合には 0 を返します。</param>
		/// <returns>指定した位置にノードが存在した場合には、そのノードを返します。
		/// ノードが存在したなかった場合には、null を返します。</returns>
		public TreeNode LocalPosition2Node(Gdi::Point p,out TreeNodeHitType type){
			const TreeNodeHitType vertical_hit
				=TreeNodeHitType.BorderTop|TreeNodeHitType.Middle|TreeNodeHitType.BorderBottom;

			type=this.HitLocalPosition(p);
			if(0!=(vertical_hit&type))return this;
			p.Y-=this.NodeHeight;


			//-- 子ノードの判定
			if(this.IsExpanded){
				p.X-=this.ChildIndent.Width;
				if(this.HasChildren)foreach(TreeNode node in this.Nodes){
					if(!node.IsVisible)continue;

					TreeNode ret=node.LocalPosition2Node(p,out type);
					if(ret!=null)return ret;
					p.Y-=node.DisplayHeight;
				}
			}

			//-- 見つからなかった時
			type=0;
			return null;
		}
		/// <summary>
		/// 指定した位置にこのノードが存在しているかどうかを取得します。
		/// </summary>
		/// <param name="p">ノードの左上を原点とした座標を指定します。</param>
		/// <returns>指定した位置にこのノードが存在しているかどうか、
		/// 指定した位置がこのノードにとってどの様な位置であるかの情報を返します。</returns>
		internal TreeNodeHitType HitLocalPosition(Gdi::Point p){
			TreeNodeHitType type=0;
			int height=this.NodeHeight;
			if(height==0){
				type|=p.Y<0?TreeNodeHitType.Above:TreeNodeHitType.Below;
				return type;
			}

			// 縦位置の判定
			float y=p.Y*4f/height;
			if(y<0)
				type|=TreeNodeHitType.Above;
			else if(y<1)
				type|=TreeNodeHitType.BorderTop;
			else if(y<=3)
				type|=TreeNodeHitType.Middle;
			else if(y<=4)
				type|=TreeNodeHitType.BorderBottom;
			else
				type|=TreeNodeHitType.Below;
#if OLD
			// 横位置の判定
			do{
				if(this.parent!=null&&this.parent.ChildIndent+p.X<0) {
					type|=TreeNodeHitType.PaddingLeft;
					break;
				}
				if(p.X<0){
					type|=TreeNodeHitType.PlusMinus;
					break;
				}
				if(this.CheckBoxVisible){
					// CheckBox 直上
					p.X-=PADDING_CHKBOX_LEFT;
					Gdi::Size sz=this.CheckBox.Size;
					if(new Gdi::Rectangle(0,(height-sz.Height+1)/2,sz.Width,sz.Height).Contains(p))
						type|=TreeNodeHitType.OnCheckBox;
					p.X-=sz.Width+PADDING_CHKBOX_RIGHT;

					// CheckBox 領域
					if(p.X<0){
						type|=TreeNodeHitType.CheckBox;
						break;
					}
				}
				if(this.IconVisible){
					// Icon 直上
					p.X-=PADDING_ICON_LEFT;
					Gdi::Size sz=this.IconSize;
					if(new Gdi::Rectangle(0,(height-sz.Height+1)/2,sz.Width,sz.Height).Contains(p))
						type|=TreeNodeHitType.OnIcon;
					p.X-=sz.Width+PADDING_CHKBOX_RIGHT;

					// Icon 領域
					if(p.X<0){
						type|=TreeNodeHitType.Icon;
						break;
					}
				}
				if(p.X<=this.Width)
					type|=TreeNodeHitType.Content;
				else
					type|=TreeNodeHitType.PaddingRight;
			}while(false);
#else
			_todo.TreeNode("PlusMinus 詳細");

			// 横位置の判定
			Gdi::Size sz=default(Gdi::Size);
			do{
				int x=p.X;
				if(this.parent!=null&&this.parent.ChildIndent.Width+x<0){
					type|=TreeNodeHitType.PaddingLeft;
					break;
				}
				if(x<0){
					type|=TreeNodeHitType.IndentArea;
					if(this.parent.ChildIndent.HitPlusMinus(x,p.Y,height,this))
						type|=TreeNodeHitType.OnPlusMinus;
					break;
				}
				if(this.CheckBoxVisible){
					sz=this.CheckBox.Size;
					p.X=x-=PADDING_CHKBOX_LEFT; // 直上判定用
					x-=sz.Width+PADDING_CHKBOX_RIGHT;

					// CheckBox 領域
					if(x<0){
						type|=TreeNodeHitType.CheckBox;
						break;
					}
				}
				if(this.IconVisible){
					sz=this.IconSize;
					p.X=x-=PADDING_ICON_LEFT; // 直上判定用
					x-=sz.Width+PADDING_CHKBOX_RIGHT;

					// Icon 領域
					if(x<0){
						type|=TreeNodeHitType.Icon;
						break;
					}
				}
				sz=this.ContentSize;
				p.X=x-=PADDING_CONTENT_LEFT; // 直上判定用
				if(x<=this.ContentSize.Width){
					type|=TreeNodeHitType.Content;
					break;
				}

				type|=TreeNodeHitType.PaddingRight;
			}while(false);

			_todo.ExamineTreeView("HitType.On*** の判定");
			// 当たり判定
			if(
				0!=(type&TreeNodeHitType.mask_vertical_hit)
				&&0!=(type&TreeNodeHitType.mask_horizontal_pmhit)
			){
				if(0!=(type&TreeNodeHitType.mask_horizontal_hit))type|=TreeNodeHitType.Hit;

				// 直上判定
				if(new Gdi::Rectangle(new Gdi::Point(0,(height-sz.Height+1)/2),sz).Contains(p))
					type|=(TreeNodeHitType)((int)(type&TreeNodeHitType.mask_horizontal_pmhit)<<16);
			}
#endif

			return type;
		}
		/// <summary>
		/// 指定した位置にこのノードが存在しているかどうかを取得します。
		/// </summary>
		/// <param name="p">位置を TreeView の論理座標で指定します。</param>
		/// <returns>指定した位置にこのノードが存在しているかどうか、
		/// 指定した位置がこのノードにとってどの様な位置であるかの情報を返します。</returns>
		public TreeNodeHitType HitLogicalPosition(Gdi::Point p){
			Gdi::Point pL=this.LogicalPosition;
			p.X-=pL.X;
			p.Y-=pL.Y;
			return HitLocalPosition(p);
		}
		/// <summary>
		/// 指定した位置にこのノードが存在しているかどうかを取得します。
		/// </summary>
		/// <param name="p">位置を TreeView のクライアント座標で指定します。</param>
		/// <returns>指定した位置にこのノードが存在しているかどうか、
		/// 指定した位置がこのノードにとってどの様な位置であるかの情報を返します。</returns>
		public TreeNodeHitType HitClientPosition(Gdi::Point p){
			Gdi::Point pC=this.ClientPosition;
			p.X-=pC.X;
			p.Y-=pC.Y;
			return HitLocalPosition(p);
		}
		#endregion

		#region logics: Relations
		//============================================================
		//		親子 / 先祖関係
		//============================================================
		/// <summary>
		/// 指定した TreeNode がこの Node の子孫であるか否かを取得又は設定します。
		/// </summary>
		/// <param name="descen">子孫であるか否かを知りたい要素を指定します。</param>
		/// <returns>指定した TreeNode がこの TreeNode の子孫であった場合に true を返します。
		/// 指定した TreeNode が自分自身だった時、及び、それ以外の場合に false を返します。</returns>
		public bool IsDescendant(TreeNode descen){
			while(true){
				if(descen.ParentNode==null)return false;
				descen=descen.ParentNode;
				if(descen==this)return true;
			}
		}
		/// <summary>
		/// この TreeNode の階層レベルを取得又は設定します。
		/// 何処の TreeNode にも属していない場合には 0 を返します。
		/// TreeView の一番上の階層に登録されている場合に 1 を返します。
		/// 他の TreeNode の子である場合は、親ノードの階層レベルより 1 大きい数を返します。
		/// </summary>
		public int Level{
			get{
				int i=0;
				TreeNode ances=this.parent;
				while(ances!=null){
					ances=ances.parent;
					i++;
				}
				return i;
			}
		}
		/// <summary>
		/// このノードの次に表示されているノードを取得します。
		/// 次に表示されるべきノードが存在しない場合には null を返します。
		/// </summary>
		public TreeNode NextVisibleInTree{
			get{
				if(this.HasChildren&&this.IsExpanded){
					if(this.Nodes.Count>0)return this.Nodes.FirstNode;
				}

				TreeNode ances=this;
				while(ances.parent!=null){
					TreeNode ret=ances.NextVisibleSibling;
					if(ret!=null)return ret;

					ances=ances.parent;
				}
				
				return null;
			}
		}
		/// <summary>
		/// このノードの前に表示されているノードを取得します。
		/// </summary>
		public TreeNode PreviousVisibleInTree{
			get{
				if(this.parent==null)return null;

				TreeNode ret=this.PreviousSibling;
				if(ret==null)return this.parent;

				while(ret.HasChildren&&ret.IsExpanded){
					if(ret.Nodes.Count==0)break;
					ret=ret.Nodes.LastNode;
				}

				return ret;
			}
		}
		//============================================================
		//		兄弟ノード
		//============================================================
		/// <summary>
		/// 新しい TreeNode をこのノードの直前に挿入します。
		/// </summary>
		/// <param name="node">挿入する TreeNode を指定します。</param>
		public void InsertBefore(TreeNode node){
			if(this.parent==null)
				throw new TreeNodeMissingParentException("兄弟ノードを追加する事は出来ません。");
			this.parent.Nodes.InsertBefore(this,node);
		}
		/// <summary>
		/// 新しい TreeNode をこのノードの直後に挿入します。
		/// </summary>
		/// <param name="node">挿入する TreeNode を指定します。</param>
		public void InsertAfter(TreeNode node){
			if(this.parent==null)
				throw new TreeNodeMissingParentException("兄弟ノードを追加する事は出来ません。");
			this.parent.Nodes.InsertAfter(this,node);
		}
		/// <summary>
		/// この TreeNode が兄弟の中で何番目の物かを取得します。
		/// この TreeNode が一番最初にある場合には 0 です。
		/// </summary>
		public int IndexInSiblings{
			get{
				if(this.parent==null)
					throw new TreeNodeMissingParentException("兄弟中の番号は意味を為しません。");
				Diag::Debug.Assert(this.parent.nodes!=null&&this.parent.nodes.Contains(this));
				return this.parent.nodes.IndexOf(this);
			}
		}
		/// <summary>
		/// このノードの次の兄弟ノードを取得します。
		/// 直後に兄弟ノードが存在していない場合には null を返します。
		/// </summary>
		public TreeNode NextSibling{
			get{
				int i=this.IndexInSiblings+1;
				if(i>=this.parent.nodes.Count)return null;
				return this.parent.nodes[i];
			}
		}
		/// <summary>
		/// このノードの次の見える兄弟ノードを取得します。
		/// このノードより後に、見える兄弟ノードが存在していない場合には null を返します。
		/// </summary>
		public TreeNode NextVisibleSibling{
			get{
				int i=this.IndexInSiblings+1;
				while(i<this.parent.nodes.Count){
					TreeNode ret=parent.nodes[i];
					if(ret.IsVisible)return ret;
				}
				return null;
			}
		}
		/// <summary>
		/// このノードの直前の兄弟ノードを取得します。
		/// 直前に兄弟ノードが存在していない場合には null を返します。
		/// </summary>
		public TreeNode PreviousSibling{
			get{
				int i=this.IndexInSiblings-1;
				if(i<0)return null;
				return this.parent.nodes[i];
			}
		}
		/// <summary>
		/// このノードの前の見える兄弟ノードを取得します。
		/// このノードより前に見える兄弟ノードが存在しない場合には null を返します。
		/// </summary>
		public TreeNode PreviousVisibleSibling{
			get{
				int i=this.IndexInSiblings-1;
				while(i>=0){
					TreeNode ret=this.parent.nodes[i];
					if(ret.IsVisible)return ret;
				}
				return null;
			}
		}

		/// <summary>
		/// この要素が、親要素の最後の子ノードであるか否かを取得します。
		/// 親要素が存在しない場合には false を返します。
		/// </summary>
		public bool IsLastChild{
			get{return this.parent!=null&&this.parent.Nodes.LastNode==this;}
		}
		/// <summary>
		/// この要素が、親要素の最初の子ノードであるか否かを取得します。
		/// 親要素が存在しない場合には false を返します。
		/// </summary>
		public bool IsFirstChild{
			get{return this.parent!=null&&this.parent.Nodes.LastNode==this;}
		}
		#endregion

		#region logics: TreeNode 状態
		//************************************************************
		//		状態色々
		//============================================================
		//		Icon 表示
		//============================================================
		// IsIconVisible → [pre]
		// IconSize → [pre]
		//============================================================
		//		Focus 状態
		//============================================================
		// AcquiredFocus → [pre]
		// LosingFocus → [pre]
		/// <summary>
		/// このノードが TreeView の中で注目されているかどうかを取得します。
		/// </summary>
		public bool IsFocused{
			get{return this.view!=null&&this==this.view.FocusedNode;}
		}
		/// <summary>
		/// このノードに TreeView 内の Focus を移動します。
		/// </summary>
		public void SetFocus(){
			if(this.view==null)
				throw new TreeViewMissingException("Focus は親 TreeView が存在しない場合には意味を為しません。");

			this.view.FocusedNode=this;
		}
		/// <summary>
		/// このノードが親 TreeView 内で選択されているか否かを取得又は設定します。
		/// </summary>
		public bool IsSelected{
			get{return this.view!=null&&this.view.SelectedNodes.Contains(this);}
		}
		/// <summary>
		/// このノードを選択した状態にします。
		/// </summary>
		public void SetSelected(){
			if(this.view==null)
				throw new TreeViewMissingException("親 TreeView が存在しないので、選択状態にする事が出来ません。");
			this.view.SelectedNodes.Set(this);
		}
		/// <summary>
		/// このノードが活性化した状態にあるか否かを取得します。
		/// 親 TreeView がない場合には活性化した状態には為りません。
		/// 親 TreeView でマウスのボタンが降りている場合には、
		/// マウスのボタンが降りた時に下にあった TreeNode が活性化した状態にあります。
		/// それ以外の場合には現在選択されている TreeNode が活性化した状態にあります。
		/// </summary>
		public bool IsActivated{
			get{return this.view!=null&&this.view.IsActivated(this);}
		}
		//============================================================
		//		Enabled 状態
		//============================================================
		// IsEnabled → [pre]
		// IsEnabledChanged → [pre]
		/// <summary>
		/// IsEnabled が変化した際の処理を実行します。
		/// </summary>
		/// <param name="changedTop">
		/// このノードを起点とした IsEnabled 状態の変化の場合に true を指定します。
		/// 親からの伝達によって変化している場合に false を指定します。
		/// </param>
		/// <param name="e">変化する値についての情報を提供します。</param>
		private void OnIsEnabledChanged(bool changedTop,TreeNodePropertyChangingEventArgs<bool> e){
			if(this.HasChildren)foreach(TreeNode child in this.Nodes){
				if(child.IsEnabledInherit==TreeNodeInheritType.Inherit)
					child.OnIsEnabledChanged(false,e);
			}
			this.OnIsEnabledChanged(e);
			if(changedTop)this.ReDraw(true);
		}
		//============================================================
		//		Expand 状態
		//============================================================
		/// <summary>
		/// 全ての子孫の開閉状態を設定します。
		/// </summary>
		/// <param name="expanded">設定する状態を指定します。
		/// 全ての子孫を展開する場合には true を指定します。
		/// 全ての子孫を折り畳む場合には false を指定します。</param>
		public void ExpandAllDescendant(bool expanded){
			this.IsExpanded=expanded;
			if(this.HasChildren)
				this.Nodes.ExpandAllDescendant(expanded);
		}
		/// <summary>
		/// このノードが現在開いた状態になっているか否かを取得又は設定します。
		/// </summary>
		public bool IsExpanded{
			get{return this.bits[bExpanded];}
			set{
				if(value==this.bits[bExpanded])return;

				TreeNodePropertyChangingEventArgs<bool> e=new TreeNodePropertyChangingEventArgs<bool>(!value,value);
				afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> m;
				if(this.xmem.GetMember("BeforeIsExpandedChange",out m))m(this,e);

				_todo.TreeNodeDisplayHeight("拡縮表示更新");
				this.bits[bExpanded]=value;
				this.RefreshDisplaySize();

				if(this.xmem.GetMember("AfterIsExpandedChange",out m))m(this,e);
			}
		}
		/// <summary>
		/// このノードの展開状態が変化する前に発生するイベントです。
		/// </summary>
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> AfterIsExpandedChange{
			add{this.xmem.AddHandler("AfterIsExpandedChange",value);}
			remove{this.xmem.RemoveHandler("AfterIsExpandedChange",value);}
		}
		/// <summary>
		/// このノードの展開状態が変化した時に発生するイベントです。
		/// </summary>
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> BeforeIsExpandedChange{
			add{this.xmem.AddHandler("BeforeIsExpandedChange",value);}
			remove{this.xmem.RemoveHandler("BeforeIsExpandedChange",value);}
		}
		//============================================================
		//		Visible 状態
		//============================================================
		/// <summary>
		/// このノードが TreeView 内に表示されるか否かを取得又は設定します。
		/// </summary>
		public bool IsVisible{
			get{return this.bits[bVisible];}
			set{
				if(this.bits[bVisible]==value)return;
				this.bits[bVisible]=value;
				this.OnIsVisibleChanged();
			}
		}
		/// <summary>
		/// IsVisible の値が変化した時に発生するイベントです。
		/// </summary>
		public event afh.CallBack<TreeNode> IsVisibleChanged{
			add{this.xmem.AddHandler("IsVisibleChanged",value);}
			remove{this.xmem.RemoveHandler("IsVisibleChanged",value);}
		}
		private void OnIsVisibleChanged(){
			afh.CallBack<TreeNode> m;
			if(this.xmem.GetMember("IsVisibleChanged",out m))m(this);

			_todo.TreeNodeDisplayHeight("親ノードへの通知 / DisplayHeight の更新");
			this.RefreshDisplaySize();
		}
		//============================================================
		//		Check 状態
		//============================================================
		/// <summary>
		/// Check 状態が子供の状態に依存して変化するか否かを取得又は設定します。
		/// true の場合には子状態が変化すると自分自身の Checked 状態も変化します。
		/// また自分自身の Checked プロパティに値を設定するとそれが全ての子ノードに適用されます。
		/// 但し、中間状態 null を設定した場合には、子ノードも自分自身も何も変更しません。
		/// </summary>
		public bool CheckedReflectChildren{
			get{return this.bits[bCheckReflectChildren];}
			set{this.bits[bCheckReflectChildren]=value;}
		}
		/// <summary>
		/// CheckBox の状態が現在有効であるか否かを取得又は設定します。
		/// </summary>
		public bool CheckBoxEnabled{
			get{return this.bits[bCheckBoxEnabled];}
			set{this.bits[bCheckBoxEnabled]=value;}
		}
		//------------------------------------------------------------
		/// <summary>
		/// TreeNode の状態がチェックされた状態になっているか否かを取得又は設定します。
		/// null は中間状態である事を示します。
		/// </summary>
		public bool? IsChecked{
			get{
				// MirrorChildren の場合
				if(this.CheckedReflectChildren){
					bool fst=true;
					bool c=default(bool);
					if(this.HasChildren)foreach(TreeNode node in this.Nodes){
						bool? v=node.IsChecked;
						if(v==null)return null;
						if(fst){
							c=(bool)v;
							fst=false;
						}else{
							if(c!=(bool)v)return null;
						}
					}
					return fst?(bool?)null: c;
				}
				return CheckState2Bool((TreeNodeCheckedState)this.bits[sCheckedState]);
			}
			set{
				TreeNodeCheckedState oldstat=(TreeNodeCheckedState)this.bits[sCheckedState];

				// MirrorChildren の場合
				if(this.CheckedReflectChildren&&value!=null){
					bool v=(bool)value;
					if(this.HasChildren)foreach(TreeNode child in this.Nodes)
						child.IsChecked=v;

					// FALL_THROUGH : 一応自分自身も変えておく為
				}

				TreeNodeCheckedState newstat;
				if(value==null){
					newstat=TreeNodeCheckedState.Intermediate;
					//newstat|=TreeNodeCheckedState.Intermediate;
				}else if((bool)value){
					newstat=TreeNodeCheckedState.Checked;
					//newstat&=~TreeNodeCheckedState.Intermediate;
					//newstat|=TreeNodeCheckedState.Checked;
				}else{
					newstat=TreeNodeCheckedState.Unchecked;
					//newstat&=~TreeNodeCheckedState.Intermediate;
					//newstat&=~TreeNodeCheckedState.Checked;
				}

				this.bits[sCheckedState]=(uint)newstat;
				if(oldstat!=newstat){
					this.OnCheckedStateChanged(new CheckedStateEventArgs(
						CheckState2Bool(oldstat),
						CheckState2Bool(newstat)
					));
					this.ReDraw(false);
				}
			}
		}
		private static bool? CheckState2Bool(TreeNodeCheckedState state){
			switch(state){
				case TreeNodeCheckedState.Unchecked:return false;
				case TreeNodeCheckedState.Checked:return true;
				case TreeNodeCheckedState.Intermediate:return null;
				default:
					Diag::Debug.Assert(false);
					return null;
			}
		}
		//------------------------------------------------------------
		/// <summary>
		/// Checked 又は Intermediate が変化した時に発生します。
		/// </summary>
		public event afh.EventHandler<TreeNode,CheckedStateEventArgs> CheckedStateChanged{
			add{this.xmem.AddHandler("CheckedStateChanged",value);}
			remove{this.xmem.RemoveHandler("CheckedStateChanged",value);}
		}
		/// <summary>
		/// Checked 又は Intermediate が変化した時に呼び出されます。
		/// </summary>
		protected void OnCheckedStateChanged(CheckedStateEventArgs e){
			afh.EventHandler<TreeNode,CheckedStateEventArgs> m;
			if(this.xmem.GetMember("CheckedStateChanged",out m))m(this,e);
		}
		/// <summary>
		/// Check 状態の変更に伴うイベントの詳細を保持します。
		/// </summary>
		public sealed class CheckedStateEventArgs:System.EventArgs{
			bool? oldState;
			bool? newState;
			internal CheckedStateEventArgs(bool? oldState,bool? newState){
				this.oldState=oldState;
				this.newState=newState;
			}
			/// <summary>
			/// Check 状態が変更される前の状態を取得します。
			/// </summary>
			public bool? OldState{
				get{return this.oldState;}
			}
			/// <summary>
			/// Check 状態が変更された後の (現在の) 状態を取得します。
			/// </summary>
			public bool? NewState{
				get{return this.newState;}
			}
		}
		#endregion
	}

	#region class:TreeNodeCollection
	/// <summary>
	/// TreeNode の集合を管理するクラスです。
	/// </summary>
	public sealed class TreeNodeCollection
		:Gen::IEnumerable<TreeNode>,Gen::ICollection<TreeNode>,System.Collections.IEnumerable,Gen::IList<TreeNode>
	{
		private readonly TreeNode parent;
		private Gen::List<TreeNode> children;

		internal TreeNodeCollection(TreeNode parent){
			this.parent=parent;
			this.children=new Gen::List<TreeNode>();
		}

		internal void ExpandAllDescendant(bool expanded){
			foreach(TreeNode node in this)
				node.ExpandAllDescendant(expanded);
		}
		//============================================================
		//		コレクション
		//============================================================
		/// <summary>
		/// 登録されている要素の数を取得します。
		/// </summary>
		public int Count{
			get{return this.children.Count;}
		}
		/// <summary>
		/// 指定した位置にある子要素を取得します。
		/// </summary>
		/// <param name="index">取得したい TreeNode の位置を指定します。</param>
		/// <returns>指定した位置にあった子要素を返します。</returns>
		public TreeNode this[int index]{
			get{return this.children[index];}
			set{
				if(index<0||index>=this.children.Count)
					throw new System.ArgumentOutOfRangeException("index");

				if(value==null){
					this.RemoveAt(index);
					return;
				}

				// 元の場所から削除
				if(value.parent!=null){
					if(value.parent==this.parent){
						// 自身に既に含まれていた場合→移動
						int oldIndex=this.children.IndexOf(value);
						if(index==oldIndex)return; // 変化無し
						if(oldIndex<index)index--;
					}

					value.parent.Nodes.Remove(value);
				}

				// 繋ぎ替え
				this.children[index].View=null;
				this.children[index].parent=null;
				value.View=this.parent.View;
				value.parent=this.parent;
				this.children[index]=value;

				// 更新
				//value.ReDraw(true);_todo.TreeNodeDisplayHeight("必要か?");
				this.suppress_register(value,true);
			}
		}
		/// <summary>
		/// この TreeNodeCollection に含まれている TreeNode の列挙子を取得します。
		/// </summary>
		/// <returns>TreeNode の列挙子を返します。</returns>
		public Gen::IEnumerator<TreeNode> GetEnumerator(){
			return this.children.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
		/// <summary>
		/// この TreeNodeCollection に含まれている子ノードを全て削除します。
		/// </summary>
		public void Clear(){
			foreach(TreeNode ch in this.children){
				ch.View=null;
				ch.parent=null;
			}
			this.children.Clear();
		}
		/// <summary>
		/// この TreeNodeCollection の内容を配列に書き出します。
		/// </summary>
		/// <param name="array">書き出す先の配列を指定します。</param>
		/// <param name="arrayIndex">指定した配列の中で、書き出しを開始する位置を指定します。
		/// 指定した位置にこの集合の第一要素が書き込まれます。</param>
		public void CopyTo(TreeNode[] array,int arrayIndex){
			if(array==null)throw new System.ArgumentNullException("array");
			if(arrayIndex<0||array.Length-arrayIndex>this.children.Count)
				throw new System.ArgumentOutOfRangeException("arrayIndex");

			foreach(TreeNode ch in this.children)array[arrayIndex++]=ch;
		}
		/// <summary>
		/// この集合が読み取り専用であるか否かを取得又は設定します。
		/// TreeNodeCollection の場合には常に false を返します。
		/// </summary>
		public bool IsReadOnly{
			get{return false;}
		}
		//============================================================
		//		幾何
		//============================================================
		internal int TotalHeight{
			get{
				int ret=0;
				foreach(TreeNode node in this)
					ret+=node.DisplayHeight;
				return ret;
			}
		}
		internal int TotalWidth{
			get{
				int ret=0;
				foreach(TreeNode node in this){
					int c=node.DisplayWidth;
					if(c>ret)ret=c;
				}
				return ret;
			}
		}
		//============================================================
		//		ノードの検索
		//============================================================
		/// <summary>
		/// 指定した TreeNode が集合に含まれているか否かを取得します。
		/// </summary>
		/// <param name="node">含まれているかどうかを知りたい TreeNode を指定します。</param>
		/// <returns>指定した TreeNode が含まれていた場合に true を返します。
		/// 含まれていなかった場合に false を返します。</returns>
		public bool Contains(TreeNode node){
			return this.children.Contains(node);
		}
		/// <summary>
		/// 指定した TreeNode が集合内で何番目に属しているかを取得します。
		/// </summary>
		/// <param name="node">番号を知りたい TreeNode を指定します。</param>
		/// <returns>指定した TreeNode の番号を返します。
		/// 指定した TreeNode が中に含まれていない場合には -1 を返します。</returns>
		public int IndexOf(TreeNode node){
			return this.children.IndexOf(node);
		}
		/// <summary>
		/// このノード集合に含まれる TreeNode の内、一番初めに登録されている物を取得します。
		/// この集合に一つも TreeNode が登録されていない時には null を返します。
		/// </summary>
		public TreeNode FirstNode{
			get{return this.children.Count==0?null: this.children[0];}
		}
		/// <summary>
		/// このノード集合に含まれる TreeNode の内、一番最後に登録されている物を取得します。
		/// この集合に一つも TreeNode が登録されていない時には null を返します。
		/// </summary>
		public TreeNode LastNode{
			get{
				int i=this.children.Count-1;
				return i<0?null: this.children[i];
			}
		}
		//============================================================
		//		ノードの追加削除
		//============================================================
		/// <summary>
		/// ノード集合を纏めて追加します。
		/// </summary>
		/// <param name="collection">追加するノードの集合を指定します。</param>
		public void AddRange(Gen::IEnumerable<TreeNode> collection){
			//_todo.TreeNodeDisplayHeight("TreeNodeCollection.Suppress: ノードリストの幾何再計算・再描画を抑止。Resume 時に変更が在れば Refresh");
			using(this.SuppressLayout())
			foreach(TreeNode node in collection){
				this.Add(node);
			}

			//if(!this.parent.RefreshDisplaySize())
			//  this.parent.ReDraw(true);
		}
		/// <summary>
		/// 新しく子要素を登録します。
		/// </summary>
		/// <param name="node">追加する TreeNode を指定します。</param>
		public void Add(TreeNode node){
			// 既に追加されている : 末端に移動したい時?
			//if(node.parent==this.parent)return;

			// 既に別の所に居る時
			if(node.parent!=null)
				node.parent.Nodes.Remove(node);

			node.View=this.parent.View;
			node.parent=this.parent;
			this.children.Add(node);

			this.suppress_register(node);
		}
		/// <summary>
		/// 指定した子要素を削除します。
		/// 複数登録されている場合には、最初に見つかった要素を削除します。
		/// </summary>
		/// <param name="node">削除したい TreeNode を指定します。</param>
		/// <returns>無事に削除された場合に true を返します。指定した要素が見つからなかった場合などに false を返します。</returns>
		public bool Remove(TreeNode node){
			node.View=null;
			node.parent=null;
			bool ret=this.children.Remove(node);

			this.suppress_register(this.parent);
			return ret;
		}
		/// <summary>
		/// 指定した位置にある子要素を削除します。
		/// </summary>
		/// <param name="index">削除する TreeNode の位置を指定します。</param>
		/// <returns>削除された要素を返します。</returns>
		public TreeNode RemoveAt(int index){
			TreeNode ret=this.children[index];
			this.children.RemoveAt(index);
			
			ret.View=null;
			ret.parent=null;

			this.suppress_register(this.parent);
			return ret;
		}
		void System.Collections.Generic.IList<TreeNode>.RemoveAt(int index){
			this.RemoveAt(index);
		}
		/// <summary>
		/// 指定した位置に指定したノードを追加します。
		/// </summary>
		/// <param name="index">ノードを追加する場所を指定します。</param>
		/// <param name="item">追加するノードを指定します。</param>
		public void Insert(int index,TreeNode item){
			if(index<0||index>this.children.Count)
				throw new System.ArgumentOutOfRangeException("index");

			// 元の場所から削除
			if(item.parent!=null){
				if(item.parent==this.parent){
					// 自身に既に含まれていた場合→移動
					int oldIndex=this.children.IndexOf(item);
					if(index==oldIndex)return; // 変化無し
					if(oldIndex<index)index--;
				}

				item.parent.Nodes.Remove(item);
			}

			item.View=this.parent.View;
			item.parent=this.parent;
			this.children.Insert(index,item);

			this.suppress_register(item);
		}
		/// <summary>
		/// 基準ノードの直後にノードを挿入します。
		/// </summary>
		/// <param name="pivot">基準ノードを指定します。</param>
		/// <param name="addee">挿入するノードを指定します。</param>
		public void InsertBefore(TreeNode pivot,TreeNode addee){
			if(pivot==addee)return;

			if(!this.children.Contains(pivot))
				throw new System.ArgumentException("指定した TreeNode pivot はこのノード集合に含まれていません。","pivot");

			this.Insert(this.children.IndexOf(pivot),addee);
			//if(addee.parent!=null)
			//  addee.parent.Nodes.Remove(addee);
			//int index=this.children.IndexOf(pivot); // remove で番号が変わる可能性から茲で
			//addee.View=this.parent.View;
			//addee.parent=this.parent;
			//this.children.Insert(index,addee);
			//_todo.TreeNodeDisplayHeight("挿入");
			//if(!addee.RefreshDisplaySizeAll())
			//  this.parent.RefreshDisplaySize();
		}
		/// <summary>
		/// 基準ノードの直前にノードを挿入します。
		/// </summary>
		/// <param name="pivot">基準ノードを指定します。</param>
		/// <param name="addee">挿入するノードを指定します。</param>
		public void InsertAfter(TreeNode pivot,TreeNode addee){
			if(pivot==addee)return;

			if(!this.children.Contains(pivot))
				throw new System.ArgumentException("指定した TreeNode pivot はこのノード集合に含まれていません。","pivot");

			this.Insert(this.children.IndexOf(pivot)+1,addee);
			//if(addee.parent!=null)
			//  addee.parent.Nodes.Remove(addee);
			//int index=this.children.IndexOf(pivot); // remove で番号が変わる可能性から茲で
			//addee.View=this.parent.View;
			//addee.parent=this.parent;
			//this.children.Insert(index+1,addee);
			//_todo.TreeNodeDisplayHeight("挿入");
			//if(!addee.RefreshDisplaySizeAll())
			//  this.parent.RefreshDisplaySize();
		}

		#region SuppressLayout
		readonly object suppress_sync=new object();
		Gen::List<TreeNode> suppress_nodes=null;
		int suppress_count=0;
		/// <summary>
		/// 再配置・再描画処理を一旦停止します。
		/// </summary>
		/// <returns>一旦停止していた処理を再開する為のオブジェクトを返します。
		/// 処理が終了したら必ず解放して下さい。</returns>
		/// <example>
		/// TreeNode node;
		/// using(node.Nodes.SuppressLayout()){
		///	  // 処理...
		/// }
		/// </example>
		public System.IDisposable SuppressLayout(){
			return new suppress_disposable(this);
		}
		/// <summary>
		/// 指定した要素のサイズを再計算する必要がある事を通知します。
		/// 自ノードを指定した時には、自身の再計算を実行します。
		/// 子ノードを指定した時には、子孫まで遡って詳細に再計算します。
		/// </summary>
		/// <param name="node">再計算するノードを指定します。</param>
		private void suppress_register(TreeNode node){
			this.suppress_register(node,false);
		}
		/// <summary>
		/// 指定した要素のサイズを再計算する必要がある事を通知します。
		/// 自ノードを指定した時には、自身の再計算を実行します。
		/// 子ノードを指定した時には、子孫まで遡って詳細に再計算します。
		/// </summary>
		/// <param name="node">再計算するノードを指定します。</param>
		/// <param name="forceParentRecalc">親の再計算も強制するか否かを指定します。</param>
		private void suppress_register(TreeNode node,bool forceParentRecalc){
			lock(suppress_sync){
				if(suppress_count==0){
					if(node==parent||!node.RefreshDisplaySizeAll()||forceParentRecalc)
						this.parent.RefreshDisplaySize();
				}else{
					if(suppress_nodes==null)
						suppress_nodes=new Gen::List<TreeNode>();
					suppress_nodes.Add(node);
					if(forceParentRecalc)
						suppress_nodes.Add(this.parent);
				}
			}
		}
		private void suppress_increment(){
			System.Threading.Interlocked.Increment(ref this.suppress_count);
		}
		private void suppress_decrement(){
			if(System.Threading.Interlocked.Decrement(ref this.suppress_count)>0)return;
			lock(this.suppress_sync){
				if(this.suppress_nodes==null)return;

				this.parent.RefreshDisplaySizeNewChildren(suppress_nodes);
				this.suppress_nodes.Clear();
				//bool fRecalcParent=false;
				//foreach(TreeNode node in this.suppress_nodes){
				//  if(node==parent)
				//    fRecalcParent=true;
				//  else if(!node.RefreshDisplaySizeAll())
				//    fRecalcParent=true;
				//}
				//this.suppress_nodes.Clear();
				//if(fRecalcParent)
				//  this.parent.RefreshDisplaySize();
			}
		}
		internal class suppress_disposable:System.IDisposable{
			readonly TreeNodeCollection collection;
			int f_dec=0;
			public suppress_disposable(TreeNodeCollection collection){
				this.collection=collection;
				collection.suppress_increment();
			}
			public void Dispose(){
				if(0==System.Threading.Interlocked.Exchange(ref this.f_dec,1))
					collection.suppress_decrement();
			}
		}
		#endregion
	}
	#endregion

	/// <summary>
	/// 親ノードが存在していない事によって、要求された操作を実行出来ない場合に発生する例外です。
	/// </summary>
	[System.Serializable]
	public sealed class TreeViewMissingException:TreeNodeException{
		const string MSG0="このノードはどの TreeView にも属していません。";
		/// <summary>
		/// TreeViewMissingException を初期化します。
		/// </summary>
		public TreeViewMissingException():base(MSG0){}
		/// <summary>
		/// TreeViewMissingException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		public TreeViewMissingException(string message):base(MSG0+message){}
		/// <summary>
		/// TreeViewMissingException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		/// <param name="innerException">この例外を引き起こす原因となった他の例外を指定します。</param>
		public TreeViewMissingException(string message,System.Exception innerException)
			:base(MSG0+message,innerException){}
	}
	/// <summary>
	/// 親ノードが存在していない事によって、要求された操作を実行出来ない場合に発生する例外です。
	/// </summary>
	[System.Serializable]
	public sealed class TreeNodeMissingParentException:TreeNodeException{
		const string MSG0="親ノードが存在していません。";
		/// <summary>
		/// TreeNodeMissingParentException を初期化します。
		/// </summary>
		public TreeNodeMissingParentException():base(MSG0){}
		/// <summary>
		/// TreeNodeMissingParentException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		public TreeNodeMissingParentException(string message):base(MSG0+message){}
		/// <summary>
		/// TreeNodeMissingParentException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		/// <param name="innerException">この例外を引き起こす原因となった他の例外を指定します。</param>
		public TreeNodeMissingParentException(string message,System.Exception innerException)
			:base(MSG0+message,innerException){}
	}
	/// <summary>
	/// TreeNode に対して無効な操作を実行しようとした場合に発生する例外です。
	/// </summary>
	[System.Serializable]
	public class TreeNodeException:System.InvalidOperationException{
		/// <summary>
		/// TreeNodeException を初期化します。
		/// </summary>
		public TreeNodeException():base(){}
		/// <summary>
		/// TreeNodeException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		public TreeNodeException(string message):base(message){}
		/// <summary>
		/// TreeNodeException を初期化します。
		/// </summary>
		/// <param name="message">例外の詳細に関して説明する文字列を指定します。</param>
		/// <param name="innerException">この例外を引き起こす原因となった他の例外を指定します。</param>
		public TreeNodeException(string message,System.Exception innerException)
			:base(message,innerException){}
	}

	/// <summary>
	/// 余剰メンバを管理する為の構造体です。
	/// 殆ど使われる可能性がないメンバを沢山保持する様なクラスに使用します。
	/// </summary>
	/// <remarks>
	/// 殆ど使われる可能性がないメンバを沢山持つクラスは、
	/// 愚直に設計すると無駄に大きなサイズのデータに為る可能性があります。
	/// その様な時にこれを使うと、必要が生じるまでメンバを割り当てず、
	/// 必要が生じたら領域を割り当てるという事が可能になります。
	/// <para>
	/// それぞれのメンバの識別には文字列を使用します。
	/// 内部的にはハッシュテーブルを使用していますので、多少のオーバーヘッドがかかります。
	/// 頻繁に使用する可能性があるメンバを茲に格納するのは得策ではありません。
	/// </para>
	/// </remarks>
	[System.Serializable]
	public struct ExtraMemberCache:Ser::ISerializable{
		Gen::Dictionary<string,object> _data;
		private Gen::Dictionary<string,object> data{
			get{return this._data??(this._data=new Gen::Dictionary<string,object>());}
		}
		/// <summary>
		/// 指定した名前に関連付けられたメンバの値を取得します。
		/// </summary>
		/// <param name="name">メンバに関連付けられた名前を指定します。</param>
		/// <returns>
		/// 取得する際には、指定したメンバの値を返します。
		/// 指定したメンバが未だ用意されていない場合には、null を返します。
		/// <para>値の取得には GetMember を使用する事をお薦めします。</para>
		/// </returns>
		public object this[string name]{
			get{
				object ret;
				if(this.data.TryGetValue(name,out ret))
					return this.data[name];
				return null;
			}
			set{this.data[name]=value;}
		}
		/// <summary>
		/// 指定したメンバを指定した型として読み取って返します。
		/// </summary>
		/// <typeparam name="T">メンバの値の型を指定します。</typeparam>
		/// <param name="name">読み取るメンバの名前を指定します。</param>
		/// <param name="val">メンバの保持する指定した型の値を返します。
		/// 指定したメンバが未だ登録されていなかった場合にはその型の既定値を返します。</param>
		/// <returns>指定したメンバが既に登録されていて値を保持していた場合に true を返します。
		/// 指定したメンバが未だ登録されていなかった場合には false を返します。</returns>
		public bool GetMember<T>(string name,out T val){
			object o;
			bool ret=data.TryGetValue(name,out o);
			if(ret)val=(T)o;else val=default(T);
			return ret;
		}
		/// <summary>
		/// イベントハンドラをメンバに追加します。
		/// メンバはデリゲート型である必要があります。
		/// </summary>
		/// <param name="name">追加先のメンバの名前を指定します。</param>
		/// <param name="deleg">追加するイベントハンドラを指定します。</param>
		public void AddHandler(string name,System.Delegate deleg){
			System.Delegate orig;
			this.GetMember(name,out orig);
			this[name]=System.Delegate.Combine(orig,deleg);
		}
		/// <summary>
		/// イベントハンドラをメンバから削除します。
		/// メンバはデリゲート型である必要があります。
		/// </summary>
		/// <param name="name">削除元のメンバの名前を指定します。</param>
		/// <param name="deleg">削除するイベントハンドラを指定します。</param>
		public void RemoveHandler(string name,System.Delegate deleg) {
			System.Delegate orig;
			this.GetMember(name,out orig);
			orig=System.Delegate.Remove(orig,deleg);
			if(orig!=null)
				this[name]=orig;
			else
				this.data.Remove(name);
		}
		//============================================================
		//		シリアライズ
		//============================================================
		/// <summary>
		/// シリアライズ可能なメンバだけシリアライズします。
		/// デリゲート (イベント) のシリアライズは行いません。
		/// </summary>
		/// <param name="info">シリアライズした結果を格納する為のオブジェクトを指定します。</param>
		/// <param name="context">シリアライズの状況を提供するオブジェクトを指定します。</param>
		void Ser::ISerializable.GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context){
			Gen::Dictionary<string,object> mem=new Gen::Dictionary<string,object>();
			if(_data!=null)foreach(Gen::KeyValuePair<string,object> p in _data){
				System.Type vtype=p.Value.GetType();
				if(!vtype.IsSerializable||typeof(System.Delegate).IsAssignableFrom(vtype))continue;
				mem.Add(p.Key,p.Value);
			}
			info.AddValue("members",mem);
		}
		private ExtraMemberCache(Ser::SerializationInfo info,Ser::StreamingContext context){
			Gen::Dictionary<string,object> members=(Gen::Dictionary<string,object>)
				info.GetValue("members",typeof(Gen::Dictionary<string,object>));
			if(members.Count==0)
				_data=null;
			else
				_data=members;
		}
	}
}