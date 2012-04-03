using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// 木構造のインターフェイスを提供します。
	/// </summary>
	/// <typeparam name="T">木構造のノードの型を表します。</typeparam>
	public interface ITree<T> where T:ITree<T>{
		/// <summary>
		/// 親ノードを取得します。
		/// </summary>
		T Parent{get;}
		/// <summary>
		/// 子ノードの集合を取得します。
		/// </summary>
		Gen::IList<T> Nodes{get;}
	}

#if 実装例
	public class Tree:afh.Collections.ITree<Tree>{
		internal int index=-1;
		internal Tree parent=null;
		internal readonly TreeList children;
		public Tree(){
			children=new TreeList(this);
		}

		public Gen::IList<Tree> Children{
			get{return this.children;}
		}
		public Tree Parent{
			get{return this.parent;}
		}

		//
		// 此処にノード固有のデータメンバを追加
		//
	}

	public class TreeList:Gen::IList<Tree>{
		readonly Tree parent;
		Gen::List<Tree> _nodes=null;
		Gen::List<Tree> nodes{
			get{
				if(_nodes==null)_nodes=new Gen::List<Tree>();
				return _nodes;
			}
		}

		public TreeList(Tree parent){
			this.parent=parent;
		}

		//-------------------------------------------------------------------------
		// IList<Tree> メンバ
		//-------------------------------------------------------------------------
		public int IndexOf(Tree item){
			if(_nodes==null)return -1;
			return _nodes.IndexOf(item);
		}
		public void Insert(int index,Tree item){
			if(item.parent!=null)
				item.parent.children.RemoveAt(item.index);

			item.parent=parent;
			item.index=index;
			this.nodes.Insert(index,item);
			for(index++;index<_nodes.Count;index++)
				_nodes[index].index=index;
		}
		public void RemoveAt(int index){
			if(_nodes==null||index<0||index>=_nodes.Count)
				throw new System.ArgumentOutOfRangeException("index");
			_nodes[index].index=-1;
			_nodes.RemoveAt(index);
			for(;index<_nodes.Count;index++)
				_nodes[index].index=index;
		}

		public Tree this[int index]{
			get{
				if(_nodes==null||index<0||index>=_nodes.Count)return null;
				return this._nodes[index];
			}
			set{
				if(_nodes==null||index<0||index>=_nodes.Count)
					throw new System.ArgumentOutOfRangeException("index");

				Tree node=_nodes[index];
				node.parent=null;
				node.index=-1;
				
				if(value.parent!=null)
					value.parent.children.RemoveAt(value.index);
				value.parent=parent;
				value.index=index;
				_nodes[index]=value;
			}
		}
		//-------------------------------------------------------------------------
		// ICollection<Tree> メンバ
		//-------------------------------------------------------------------------
		public void Add(Tree item){
			this.nodes.Add(item);
			if(item.parent!=null)
				item.parent.children.RemoveAt(item.index);
			item.parent=parent;
			item.index=_nodes.Count;
		}

		public void Clear(){
			if(_nodes==null)return;
			for(int i=0,iN=_nodes.Count;i<iN;i++){
				_nodes[i].parent=null;
				_nodes[i].index=-1;
			}
			_nodes.Clear();
		}

		public bool Contains(Tree item){
			return _nodes!=null&&_nodes.Contains(item);
		}

		public void CopyTo(Tree[] array,int arrayIndex){
			if(_nodes==null)return;
			_nodes.CopyTo(array,arrayIndex);
		}

		public int Count{
			get{return _nodes==null?0:_nodes.Count;}
		}

		public bool IsReadOnly{get{return false;}}

		public bool Remove(Tree item){
			int index=this.IndexOf(item);
			if(index<0)return false;
			this.RemoveAt(index);
			return true;
		}
		//-------------------------------------------------------------------------
		// IEnumerable<Tree> メンバ
		//-------------------------------------------------------------------------
		static Gen::IEnumerator<Tree> NullEnumerator(){yield break;}
		public Gen::IEnumerator<Tree> GetEnumerator(){
			if(_nodes==null)
				return NullEnumerator();
			else
				return _nodes.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
	}
#endif

namespace Design{
	/// <summary>
	/// 木構造を作成する際の基本クラスを提供します。
	/// </summary>
	/// <typeparam name="NodeType">派生クラスを指定します。木構造の各ノードの型になります。</typeparam>
	public class TreeBase<NodeType>:afh.Collections.ITree<NodeType>
		where NodeType:TreeBase<NodeType>
	{
		internal int index=-1;
		internal NodeType parent=null;
		internal TreeBaseList<NodeType> children;
		/// <summary>
		/// コンストラクタを指定します。
		/// </summary>
		protected TreeBase(){
			children=new TreeBaseList<NodeType>((NodeType)this);
		}
		/// <summary>
		/// 親要素を取得します。
		/// </summary>
		public NodeType Parent{
			get{return this.parent;}
		}
		/// <summary>
		/// 子要素の集合を取得します。
		/// </summary>
		public Gen::IList<NodeType> Nodes{
			get{return this.children;}
		}
		/// <summary>
		/// 子要素集合を変更出来るかどうかを取得又は設定します。
		/// 例えば子要素を持たないノードを指定するには、これを設定します。
		/// </summary>
		protected bool NodesReadOnly{
			get{return this.children.IsReadOnly;}
			set{this.children.IsReadOnly=value;}
		}

		//
		// 此処にノード固有のデータメンバを追加
		//
	}

	/// <summary>
	/// TreeBase&lt;NodeType&gt; の子要素配列を表現します。
	/// </summary>
	/// <typeparam name="NodeType">木構造の各ノードの型を指定します。</typeparam>
	internal class TreeBaseList<NodeType>:Gen::IList<NodeType>
		where NodeType:TreeBase<NodeType>
	{
		readonly NodeType parent;
		bool isreadonly;
		Gen::List<NodeType> _nodes;
		Gen::List<NodeType> nodes{
			get{
				if(_nodes==null)_nodes=new Gen::List<NodeType>();
				return _nodes;
			}
		}

		internal TreeBaseList(NodeType parent){
			this.parent=parent;
			this._nodes=null;
			this.isreadonly=false;
		}
		//-------------------------------------------------------------------------
		// IList<Tree> メンバ
		//-------------------------------------------------------------------------
		public int IndexOf(NodeType item){
			if(_nodes==null)return -1;
			return _nodes.IndexOf(item);
		}
		public void Insert(int index,NodeType item){
			if(index<0||index>this.Count)
				throw new System.ArgumentOutOfRangeException("index");
			if(this.isreadonly)
				throw new System.NotSupportedException("This collection is readonly.");

			// 削除
			if(item.parent!=null){
				if(item.parent==parent){
					if(item.index==index)return;
					if(item.index<index)index--;
				}

				item.parent.children.RemoveAt(item.index);
			}

			item.parent=parent;
			item.index=index;
			this.nodes.Insert(index,item);
			for(index++;index<_nodes.Count;index++)
				_nodes[index].index=index;
		}
		public void RemoveAt(int index){
			if(this.isreadonly)
				throw new System.NotSupportedException("This collection is readonly.");
			if(_nodes==null||index<0||index>=_nodes.Count)
				throw new System.ArgumentOutOfRangeException("index");
			_nodes[index].index=-1;
			_nodes.RemoveAt(index);
			for(;index<_nodes.Count;index++)
				_nodes[index].index=index;
		}

		public NodeType this[int index]{
			get{
				if(_nodes==null||index<0||index>=_nodes.Count)return null;
				return this._nodes[index];
			}
			set{
				if(this.isreadonly)
					throw new System.NotSupportedException("This collection is readonly.");
				if(_nodes==null||index<0||index>=_nodes.Count)
					throw new System.ArgumentOutOfRangeException("index");

				if(value.parent!=null){
					if(value.parent==parent){
						if(value.index==index)return;
						if(value.index<index)index--;
					}

					value.parent.children.RemoveAt(value.index);
				}

				NodeType node=_nodes[index];
				node.parent=null;
				node.index=-1;
				value.parent=parent;
				value.index=index;
				_nodes[index]=value;
			}
		}
		//-------------------------------------------------------------------------
		// ICollection<Tree> メンバ
		//-------------------------------------------------------------------------
		public void Add(NodeType item){
			if(this.isreadonly)throw new System.NotSupportedException("This collection is readonly.");
			if(item.parent!=null)
				item.parent.children.RemoveAt(item.index);
			item.parent=parent;
			item.index=nodes.Count;
			this.nodes.Add(item);
		}

		public void Clear(){
			if(_nodes==null)return;
			if(this.isreadonly)throw new System.NotSupportedException("This collection is readonly.");
			for(int i=0,iN=_nodes.Count;i<iN;i++){
				_nodes[i].parent=null;
				_nodes[i].index=-1;
			}
			_nodes.Clear();
		}

		public bool Contains(NodeType item){
			return _nodes!=null&&_nodes.Contains(item);
		}

		public void CopyTo(NodeType[] array,int arrayIndex){
			if(_nodes==null)return;
			_nodes.CopyTo(array,arrayIndex);
		}

		public int Count{
			get{return _nodes==null?0:_nodes.Count;}
		}

		public bool IsReadOnly{
			get{return this.isreadonly;}
			set{this.isreadonly=value;}
		}

		public bool Remove(NodeType item){
			if(this.isreadonly)
				throw new System.NotSupportedException("This collection is readonly.");

			int index=this.IndexOf(item);
			if(index<0)return false;
			this.RemoveAt(index);
			return true;
		}
		//-------------------------------------------------------------------------
		// IEnumerable<Tree> メンバ
		//-------------------------------------------------------------------------
		static Gen::IEnumerator<NodeType> NullEnumerator(){yield break;}
		public Gen::IEnumerator<NodeType> GetEnumerator(){
			if(_nodes==null)
				return NullEnumerator();
			else
				return _nodes.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
	}

}
}

