using Gen=System.Collections.Generic;

namespace afh.HTML{
#if !OLD // 2011/05/16 04:15:27
	public interface IHTMLNode{
		nodeType nodeType{get;}
		string nodeName{get;}
		string outerHTML{get;}
	}
	public abstract class HTMLNode:IHTMLNode{
		public HTMLNode(HTMLElement parent){
			this.parent=parent;
		}
		protected HTMLElement parent;
		protected int index{
			get{
				if(this.parent==null)return -1;
				return this.parent._childNodes.FindIndex(
					delegate(IHTMLNode e){return this==e;}
				);
			}
		}
		/// <summary>
		/// ノードの親要素を取得します。
		/// </summary>
		public HTMLElement parentNode{get{return this.parent;}}
		/// <summary>
		/// このノードの次に来る兄弟ノードを取得します。
		/// </summary>
		public IHTMLNode nextSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index+1;
				return this.parent._childNodes.Count<=index?null:this.parent._childNodes[index];
			}
		}
		/// <summary>
		/// このノードの前にある兄弟ノードを取得します。
		/// </summary>
		public IHTMLNode previousSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index-1;
				return index<0?null:this.parent._childNodes[index];
			}
		}
		public abstract nodeType nodeType{get;}
		public abstract string nodeName{get;}
		public abstract string outerHTML{get;}

		//TODO:
		//nextElementSibling
		//previousElementSibling
	}
#endif

#if !OLD // 2011/05/16 04:15:27
	public class HTMLElement:HTMLNode,afh.Collections.ITree<HTMLNode>{
		//-------------------------------------------------------------------------
		//	基本木構造
		//-------------------------------------------------------------------------
		internal readonly HTMLNodeList _childNodes;
		public override nodeType nodeType{
			get{return nodeType.ELEMENT_NODE;}
		}

		#region ITree<HTMLNode> メンバ
		Gen::IList<HTMLNode> afh.Collections.ITree<HTMLNode>.Children{
			get{return this._childNodes;}
		}
		#endregion

		//-------------------------------------------------------------------------
		//	HTMLNode
		//-------------------------------------------------------------------------
		private string name;
		internal readonly HTMLAttributeCollection _attributes;
		public HTMLElement(){
			_childNodes=new HTMLNodeList(this);
			_attributes=new HTMLAttributeCollection(this.parent);
		}

		public string tagName{
			get{return this.name;}
			set{this.name=value;}
		}
		public override string nodeName{
			get{return this.name;}
		}
		public override nodeType nodeType{
			get{return nodeType.ELEMENT_NODE;}
		}
		public HTMLAttributeCollection attributes{
			get{return this._attributes;}
		}
		public void appendChild(HTMLNode node){
			this._childNodes.Add(node);
		}

	}
#endif

}
