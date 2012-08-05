using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2=System.Drawing.Drawing2D;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Ser=System.Runtime.Serialization;
using Forms=System.Windows.Forms;

using BitSection=afh.Collections.BitSection;

namespace afh.Forms{

	#region class:TextTreeNode
	/// <summary>
	/// 文字列を表示する為の TreeNode です。
	/// </summary>
	[System.Serializable]
	public class TextTreeNode:TreeNode,Ser::ISerializable{
		private string text;
		Gdi::Size sizeContent;
		/// <summary>
		/// 表示文字列を取得又は設定します。
		/// </summary>
		public string Text{
			get{return this.text;}
			set{
				if(this.text==value)return;
				this.text=value??"";
				Gdi::SizeF sz=afh.Drawing.GraphicsUtils.MeasureString(this.text,this.Font);
				this.sizeContent.Width=(int)sz.Width;
				this.sizeContent.Height=(int)sz.Height+1;

			//	Forms::TreeNode node;
			//	TreeNode nd;
				_todo.TreeNode("内容変化に伴う再描画");
				_todo.TreeNodeDisplayHeight("内容変化に伴う中身のサイズ変化");
			}
		}
		//============================================================
		//		初期化
		//============================================================
		/// <summary>
		/// 指定した文字列を使用して TextTreeNode を初期化します。
		/// </summary>
		/// <param name="text">TreeNode の表示に使用する文字列を指定します。</param>
		public TextTreeNode(string text):base(){
			this.Text=text;
		}
		/// <summary>
		/// 指定したシリアライズ情報から、TextTreeNode を復元します。
		/// </summary>
		/// <param name="info">シリアライズしたデータを指定します。</param>
		/// <param name="ctx">シリアライズの環境に関連する情報を指定します。</param>
		public TextTreeNode(Ser::SerializationInfo info,Ser::StreamingContext ctx)
			:base(info,ctx)
		{
			this.Text=info.GetString("text");
		}
		/// <summary>
		/// シリアライズの際に記録するデータを SerializationInfo に書き込みます。
		/// </summary>
		/// <param name="info">データの記録先の SerializationInfo を指定します。</param>
		/// <param name="context">シリアライズの環境に関連する情報を指定します。</param>
		protected override void GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context){
			base.GetObjectData(info,context);
			info.AddValue("text",this.text);
		}
		//============================================================
		//		表示
		//============================================================
		/// <summary>
		/// 内容を表示する領域の大きさを取得します。
		/// </summary>
		public override Gdi::Size ContentSize{
			get{return this.sizeContent;}
		}
		/// <summary>
		/// このノードの内容を描画します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		protected override void DrawContent(System.Drawing.Graphics g){
			this.DrawContentBackground(g);

			Gdi::Rectangle rect=new Gdi::Rectangle(Gdi::Point.Empty,this.ContentSize);

			Gdi::Brush brush;
			if(this.IsEnabled){
				brush=this.IsActivated?Gdi::Brushes.White:Gdi::Brushes.Black;
			}else{
				brush=this.IsActivated?Gdi::Brushes.Gray:Gdi::Brushes.Silver;
			}

			g.DrawString(this.text,this.Font,brush,new Gdi::PointF(0,1));
		}
	}
	#endregion

	#region class:TreeTreeNode
	/// <summary>
	/// afh.Collections.ITree&lt;T&gt; 型を表示する為の TreeNode です。
	/// </summary>
	/// <typeparam name="T">表示するオブジェクトの型です。</typeparam>
	[System.Serializable]
	public class TreeTreeNode<T>:TextTreeNode,Ser::ISerializable
		where T:afh.Collections.ITree<T>
	{
		//■ITree 側で変更があった場合の feedback は?

		readonly afh.Collections.ITree<T> value;
		/// <summary>
		/// TreeTreeNode のコンストラクタです。
		/// </summary>
		/// <param name="value">対応する ITree オブジェクトを指定します。</param>
		public TreeTreeNode(T value):base(value.ToString()){
			this.value=value;
		}
		/// <summary>
		/// TreeTreeNode のコンストラクタです。
		/// </summary>
		/// <param name="value">対応する ITree オブジェクトを指定します。</param>
		/// <param name="name">ITree オブジェクトの名前を指定します。</param>
		protected TreeTreeNode(T value,string name):base(name){
			this.value=value;
		}
		/// <summary>
		/// このノードに対応する ITree オブジェクトを取得します。
		/// </summary>
		public T Value{
			get{return (T)value;}
		}

		/// <summary>
		/// 子ノードを初期化します。
		/// </summary>
		/// <returns>新しく作成した子ノード集合を返します。</returns>
		protected override void InitializeNodes(TreeNodeCollection nodes){
			foreach(T child in this.value.Nodes){
				TreeTreeNode<T> childNode=new TreeTreeNode<T>(child);
				if(this.DDBehaviorInherit==TreeNodeInheritType.Custom)
					childNode.DDBehavior=this.DDBehavior;
				nodes.Add(childNode);
			}
		}
		/// <summary>
		/// 子ノードが存在するか否かを取得します。
		/// </summary>
		public override bool HasChildren{
			get{return value.Nodes.Count>0;}
		}

		#region DDBehavior
		/// <summary>
		/// 自動ドラッグドロップ動作を有効にします。
		/// </summary>
		public virtual void SetupAutoDD(){
			this.DDBehavior=MyDDBehavior.Instance;
		}

		//-------------------------------------------------------------------------
		//	for MyDDBehavior
		//-------------------------------------------------------------------------
		/// <summary>
		/// この要素を他の場所へ移動する事が出来るか否かを取得します。
		/// </summary>
		protected virtual bool IsMovable{
			get{return this.Value.Parent==null||!this.Value.Parent.Nodes.IsReadOnly;}
		}
		/// <summary>
		/// この要素をコピーする事が出来るか否かを取得します。
		/// </summary>
		protected virtual bool IsCopyable{
			get{return this is System.ICloneable;}
		}
		/// <summary>
		/// この要素へのリンクを作成する事が可能か否かを取得します。
		/// </summary>
		protected virtual bool IsLinkable{
			get{return false;}
		}

		private Forms::DragDropEffects DragEffectToTreeNode{
			get{
				System.Windows.Forms.DragDropEffects ret=0;
				if(this.IsMovable)ret|=System.Windows.Forms.DragDropEffects.Move;
				if(this.IsCopyable)ret|=System.Windows.Forms.DragDropEffects.Copy;
				if(this.IsLinkable)ret|=System.Windows.Forms.DragDropEffects.Link;
				return ret;
			}
		}

		/// <summary>
		/// ITree に対する既定の DDBehavior クラスです。
		/// </summary>
		protected class MyDDBehavior:TreeNodeDDBehaviorBase{
			static MyDDBehavior inst=null;
			/// <summary>
			/// MyDDBegavior インスタンスを取得します。
			/// </summary>
			public static MyDDBehavior Instance{
				get{return inst??(inst=new MyDDBehavior());}
			}

			/// <summary>
			/// 指定した TreeNode をドラッグ可能か否か判定し、
			/// 可能なドラッグドロップエフェクトを返します。
			/// </summary>
			/// <param name="node">判定対象の TreeNode を指定します。</param>
			/// <returns>ドラッグ可能の場合には、可能な DragDropEffects を返します。
			/// ドラッグ不可能の場合には DragDropEffects.None を返します。</returns>
			public override System.Windows.Forms.DragDropEffects GetAllowedDDE(TreeNode node){
				TreeTreeNode<T> _node=node as TreeTreeNode<T>;
				if(_node==null)return Forms::DragDropEffects.None;

				// 他種類の物体へドロップする為、取り敢えず全て可能。
				return Forms::DragDropEffects.All;
			}

			//-----------------------------------------------------------------------
			// OnDrop
			//-----------------------------------------------------------------------
			/// <summary>
			/// ドロップ処理を実行します。
			/// </summary>
			/// <param name="node">処理対象のノードを指定します。</param>
			/// <param name="e">ドラッグの情報を指定します。</param>
			public override void OnDrop(TreeNode node,TreeNodeDragEventArgs e){
				TreeTreeNode<T> _dst=node as TreeTreeNode<T>;
				TreeNodeDDTarget target=this.GetTarget(node,e);
				switch(e.Effect&this.GetEffect(node,e)){
					case Forms::DragDropEffects.Move:{
						TreeNode node2=e.Data.GetData("afh.Forms.TreeNode") as TreeNode;
						if(node2!=null){
							this.OnDrop_MoveNode(_dst,node2,target);
							break;
						}

						Gen::List<TreeNode> list=e.Data.GetData("afh.Forms.TreeNode:List") as Gen::List<TreeNode>;
						if(list!=null){
							this.OnDrop_MoveList(_dst,list,target);
							break;
						}

						break;
					}
					case Forms::DragDropEffects.Link:
					case Forms::DragDropEffects.Copy:
						throw new System.NotImplementedException();
					default:
						break;
				}
			}
			private void OnDrop_MoveNode(TreeTreeNode<T> _dst,TreeNode src,TreeNodeDDTarget target){
				TreeTreeNode<T> _src=src as TreeTreeNode<T>;
				if(_src==null)return;

				T dval=_dst.Value;

				switch(target){
					case TreeNodeDDTarget.Self:
						dval.Nodes.Add(_src.Value);
						_dst.Nodes.Add(_src);
						break;
					case TreeNodeDDTarget.Child:
						dval.Nodes.Insert(0,_src.Value);
						_dst.Nodes.Insert(0,_src);
						break;
					case TreeNodeDDTarget.Prev:
						dval.Parent.Nodes.Insert(dval.Parent.Nodes.IndexOf(dval),_src.Value);
						_dst.ParentNode.Nodes.InsertBefore(_dst,_src);
						break;
					case TreeNodeDDTarget.Next:
						dval.Parent.Nodes.Insert(dval.Parent.Nodes.IndexOf(dval)+1,_src.Value);
						_dst.ParentNode.Nodes.InsertAfter(_dst,_src);
						break;
				}
			}
			private void OnDrop_MoveList(TreeTreeNode<T> _dst,Gen::List<TreeNode> list,TreeNodeDDTarget target){
				_todo.TreeNodeDisplayHeight("TreeNodeCollection.Suppress: ノードリストの幾何再計算・再描画を抑止");
				_todo.TreeNodeDisplayHeight("target==Child の時は AddRange を利用する様にする?");
				using(_dst.Nodes.SuppressLayout())
				foreach(TreeNode node3 in list)
					this.OnDrop_MoveNode(_dst,node3,target);
			}
			//-----------------------------------------------------------------------
			//	GetEffect
			//-----------------------------------------------------------------------
			/// <summary>
			/// ドラッグドロップ操作の期待される DDE を取得します。
			/// </summary>
			/// <param name="node">ドロップ先のノードを指定します。</param>
			/// <param name="e">ドラッグに関する情報を指定します。</param>
			/// <returns>ドロップによって期待される DDE を返します。</returns>
			protected override Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e){
				//■TODO: 他のプロセスから来た場合はどうなるのか?

				// キー状態
				Forms::DragDropEffects mask;
				switch(e.KeyState){
					case TreeNodeDragEventArgs.KEY_MLEFT|TreeNodeDragEventArgs.KEY_CTRL:
						mask=Forms::DragDropEffects.Copy;
						break;
					case TreeNodeDragEventArgs.KEY_MLEFT|TreeNodeDragEventArgs.KEY_ALT:
						mask=Forms::DragDropEffects.Link;
						break;
					default:
						mask=Forms::DragDropEffects.Move;
						break;
				}

				// ドロップ先
				TreeTreeNode<T> _dst=node as TreeTreeNode<T>;
				if(_dst==null)goto none;
				switch(this.GetTarget(node,e)){
					case TreeNodeDDTarget.Self:
					case TreeNodeDDTarget.Child:
						break;
					case TreeNodeDDTarget.Next:
					case TreeNodeDDTarget.Prev:
						TreeTreeNode<T> parent=_dst.ParentNode as TreeTreeNode<T>;
						if(parent==null)goto none;
						_dst=parent;
						break;
					default:
						goto none;
				}
				if(_dst.Value.Nodes.IsReadOnly)goto none;

				// ドラッグオブジェクト
				{
					// 単一ノードの場合
					TreeNode node2=e.Data.GetData("afh.Forms.TreeNode") as TreeNode;
					if(node2!=null)
						return GetEffect_Node(_dst,node2,mask);

					// ノードリストの場合
					Gen::List<TreeNode> list=e.Data.GetData("afh.Forms.TreeNode:List") as Gen::List<TreeNode>;
					if(list!=null)
						return GetEffect_List(_dst,list,mask);

					//■此処で拡張可能に
				}

			none:
				return Forms::DragDropEffects.None;
			}
			private Forms::DragDropEffects GetEffect_Node(TreeTreeNode<T> _dst,TreeNode src,Forms::DragDropEffects mask){
				TreeTreeNode<T> _src=src as TreeTreeNode<T>;
				if(_src==null)return Forms::DragDropEffects.None;

				mask&=_src.DragEffectToTreeNode;
				if(_src.IsDescendant(_dst)||_src==_dst)mask&=~Forms::DragDropEffects.Move;
				
				return mask;
			}
			private Forms::DragDropEffects GetEffect_List(TreeTreeNode<T> _dst,Gen::List<TreeNode> list,Forms::DragDropEffects mask){
				foreach(TreeNode node2 in list){
					TreeTreeNode<T> _src=node2 as TreeTreeNode<T>;
					if(_src==null)return Forms::DragDropEffects.None;

					mask&=_src.DragEffectToTreeNode;
					if(_src.IsDescendant(_dst)||_src==_dst)mask&=~Forms::DragDropEffects.Move;
				}

				return mask;
			}
		}
		#endregion
	}
	#endregion
}
