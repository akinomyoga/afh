using Gen=System.Collections.Generic;

namespace afh.HTML{

	public abstract class HTMLNode:afh.Collections.ITree<HTMLNode>{
		//-------------------------------------------------------------------------
		//	基本木構造
		//-------------------------------------------------------------------------
		internal int index=-1;
		internal HTMLElement parent=null;

		#region ITree<HTMLNode> メンバ
		HTMLNode afh.Collections.ITree<HTMLNode>.Parent{
			get{return this.parent;}
		}
		Gen::IList<HTMLNode> afh.Collections.ITree<HTMLNode>.Nodes{
			get{return HTMLNodeListEmpty.Instance;}
		}
		#endregion

		//-------------------------------------------------------------------------
		//	HTMLNode
		//-------------------------------------------------------------------------
		public HTMLNode(){}
		public HTMLNode(HTMLElement parent){
			this.parent=parent;
		}

		public abstract nodeType nodeType{get;}
		public abstract string nodeName{get;}
		public abstract string outerHTML{get;}
		/// <summary>
		/// ノードの親要素を取得します。
		/// </summary>
		public HTMLElement parentNode{get{return this.parent;}}
		/// <summary>
		/// このノードの次に来る兄弟ノードを取得します。
		/// </summary>
		public HTMLNode nextSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index+1;
				return this.parent._childNodes.Count<=index?null:this.parent._childNodes[index];
			}
		}
		/// <summary>
		/// このノードの前にある兄弟ノードを取得します。
		/// </summary>
		public HTMLNode previousSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index-1;
				return index<0?null:this.parent._childNodes[index];
			}
		}

		//TODO:
		//nextElementSibling
		//previousElementSibling
	}

	#region class:HTMLNodeListEmpty
	internal class HTMLNodeListEmpty:Gen::IList<HTMLNode>{
		static readonly HTMLNodeListEmpty instance=new HTMLNodeListEmpty();
		public static HTMLNodeListEmpty Instance{get{return instance;}}

		// IList<HTMLNode> メンバ
		public int IndexOf(HTMLNode item){return -1;}
		public void Insert(int index,HTMLNode item){
			throw new System.NotSupportedException();
		}
		public void RemoveAt(int index){
			throw new System.ArgumentOutOfRangeException("index");
		}
		public HTMLNode this[int index] {
			get{throw new System.ArgumentOutOfRangeException("index");}
			set{throw new System.ArgumentOutOfRangeException("index");}
		}

		// ICollection<HTMLNode> メンバ
		public void Add(HTMLNode item) {
			throw new System.NotSupportedException();
		}
		public void Clear(){return;}
		public bool Contains(HTMLNode item){return false;}
		public void CopyTo(HTMLNode[] array,int arrayIndex){
			if(array==null)
				throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex>=array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex");
			return;
		}
		public int Count{get{return 0;}}
		public bool IsReadOnly{get{return true;}}
		public bool Remove(HTMLNode item){
			throw new System.NotSupportedException();
		}

		// IEnumerable<HTMLNode> メンバ
		public System.Collections.Generic.IEnumerator<HTMLNode> GetEnumerator(){
			return HTMLNodeList.NullEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
	}
	#endregion


	public interface IHTMLElementList{
		HTMLElement item(int index);
		int length{get;}
		HTMLElement this[int index]{get;}

		// MSHTML
		//  item(index)      : 番号で項目を選択
		//  item(id)				 : name/id で項目を選択
		//  item(name,index) : name/id 及び index で項目を選択
		//  this[index]      : 番号で項目を選択
		//  this[id]         : name/id で項目を選択
		//  this[name,index] : name/id 及び index で項目を選択
		//  namedItem(name)  : name/id で項目を選択
		//  urns(urn)        : urn で指定される behavior が適用されている要素の一覧
	}

	public class HTMLNodeList:Gen::IList<HTMLNode>{
		readonly HTMLElement parent;
		Gen::List<HTMLNode> _nodes=null;
		Gen::List<HTMLNode> nodes{
			get{
				if(_nodes==null)_nodes=new Gen::List<HTMLNode>();
				return _nodes;
			}
		}

		public HTMLNodeList(HTMLElement parent){
			this.parent=parent;
		}

		/// <summary>
		/// 指定した番号にあるノードを取得します。
		/// </summary>
		/// <param name="index">ノード番号を指定します。</param>
		/// <returns>指定した位置にあるノードを返します。</returns>
		public HTMLNode item(int index){
			return this[index];
		}

		//-------------------------------------------------------------------------
		// IList<HTMLNode> メンバ
		//-------------------------------------------------------------------------
		public int IndexOf(HTMLNode item){
			return item.parent!=parent?-1:item.index;
			//if(_nodes==null)return -1;
			//return _nodes.IndexOf(item);
		}
		public void Insert(int index,HTMLNode item){
			if(item.parent!=null)
				item.parent._childNodes.RemoveAt(item.index);

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

		public HTMLNode this[int index]{
			get{
				if(_nodes==null||index<0||index>=_nodes.Count)return null;
				return this._nodes[index];
			}
			set{
				if(_nodes==null||index<0||index>=_nodes.Count)
					throw new System.ArgumentOutOfRangeException("index");

				HTMLNode node=_nodes[index];
				node.parent=null;
				node.index=-1;
				
				if(value.parent!=null)
					value.parent._childNodes.RemoveAt(value.index);
				value.parent=parent;
				value.index=index;
				_nodes[index]=value;
			}
		}
		//-------------------------------------------------------------------------
		// ICollection<HTMLNode> メンバ
		//-------------------------------------------------------------------------
		public void Add(HTMLNode item){
			this.nodes.Add(item);
			if(item.parent!=null)
				item.parent._childNodes.RemoveAt(item.index);
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

		public bool Contains(HTMLNode item){
			return _nodes!=null&&_nodes.Contains(item);
		}

		public void CopyTo(HTMLNode[] array,int arrayIndex){
			if(_nodes==null)return;
			_nodes.CopyTo(array,arrayIndex);
		}

		public int Count{
			get{return _nodes==null?0:_nodes.Count;}
		}

		public bool IsReadOnly{get{return false;}}

		public bool Remove(HTMLNode item){
			int index=this.IndexOf(item);
			if(index<0)return false;
			this.RemoveAt(index);
			return true;
		}
		//-------------------------------------------------------------------------
		// IEnumerable<HTMLNode> メンバ
		//-------------------------------------------------------------------------
		internal static Gen::IEnumerator<HTMLNode> NullEnumerator(){yield break;}
		public Gen::IEnumerator<HTMLNode> GetEnumerator(){
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
