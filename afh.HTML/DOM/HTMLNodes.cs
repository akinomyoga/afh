using Gen=System.Collections.Generic;

namespace afh.HTML{

	public enum nodeType{
		ELEMENT_NODE=1,
		ATTRIBUTE_NODE=2,
		TEXT_NODE=3,
		CDATA_SECTION_NODE=4,
		ENTITY_REFERENCE_NODE=5,
		ENTITY_NODE=5,
		PROCESSING_INSTRUCTION_NODE=7,
		COMMENT_NODE=8,
		DOCUMENT_NODE=9,
		DOCUMENT_TYPE_NODE=10,
		DOCUMENT_FRAGMENT_NODE=11,
		NOTATION_NODE=12,
		xmldeclaration=17
	}
	public class HTMLElement:HTMLNode,afh.Collections.ITree<HTMLNode>{
		//-------------------------------------------------------------------------
		//	基本木構造
		//-------------------------------------------------------------------------
		#region ITree<HTMLNode> メンバ
		Gen::IList<HTMLNode> afh.Collections.ITree<HTMLNode>.Nodes{
			get{return this._childNodes;}
		}
		#endregion
		//-------------------------------------------------------------------------
		//	HTMLNode
		//-------------------------------------------------------------------------
		private string name;
		internal readonly Gen::List<HTMLNode> _childNodes;
		internal readonly HTMLAttributeCollection _attributes;
		public HTMLElement(HTMLElement parent,string name):base(parent){
			this.name=name;
			this._childNodes=new Gen::List<HTMLNode>();
			this._attributes=new HTMLAttributeCollection(this.parent);
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
		public void appendChild(HTMLNode node){
			this._childNodes.Add(node);
		}
		public HTMLAttributeCollection attributes{
			get{return this._attributes;}
		}
		//-------------------------------------------------------------------------
		//	子ノード・子要素
		//-------------------------------------------------------------------------
		/// <summary>
		/// 子ノードの集合を取得します。
		/// </summary>
		public Gen::IList<HTMLNode> childNodes{
			get{return this._childNodes;}
		}
		/// <summary>
		/// 一番最初の子ノードを取得します。
		/// </summary>
		public HTMLNode firstChild{
			get{return this._childNodes.Count==0?null:this._childNodes[0];}
		}
		/// <summary>
		/// 一番最後の子ノードを取得します。
		/// </summary>
		public HTMLNode lastChild{
			get{return this._childNodes.Count==0?null:this._childNodes[this._childNodes.Count-1];}
		}
		//TODO:
		//children
		//firstElementChild
		//lastElementChild
		//childElementCount
		//=================================================
		//	methods: 内容文字列に関する操作
		//=================================================
		public string textContent{get{return this.innerText;}}
		public virtual string outerText{get{return this.innerText;}}
		public virtual string innerText{
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				foreach(HTMLNode node in this._childNodes){
					if(node is HTMLTextNode){
						r.Append(((HTMLTextNode)node).data);
					}else if(node is HTMLElement){
						r.Append(((HTMLElement)node).innerText);
					}
				}
				return r.ToString();
			}
		}
		public virtual string innerHTML{
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				foreach(HTMLNode node in this._childNodes){
					r.Append(node.outerHTML);
				}
				return r.ToString();
			}
		}
		public override string outerHTML{
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				r.Append("<");
				r.Append(this.tagName);
				r.Append(this._attributes.ToString());
				string inner=this.innerHTML;
				if(inner!=""){
					r.Append(">");
					r.Append(inner);
					r.Append("</");
					r.Append(this.tagName);
					r.Append(">");
				}else r.Append(this.tagName[0]=='/'?">":"/>");
				return r.ToString();
			}
		}
		//=================================================
		//	methods: 要素列挙に関する操作
		//=================================================
		public HTMLElement[] getElementsByTagName(string tagName,bool onlyChild){
			System.Collections.Generic.List<HTMLElement> list
				=new System.Collections.Generic.List<HTMLElement>();
			foreach(HTMLElement e in this.enumElementsByTagName(tagName,onlyChild))list.Add(e);
			return list.ToArray();
		}
		public HTMLElement[] getElementsByName(string name,bool onlyChild){
			System.Collections.Generic.List<HTMLElement> list
				=new System.Collections.Generic.List<HTMLElement>();
			foreach(HTMLElement e in this.enumElementsByName(name,onlyChild))list.Add(e);
			return list.ToArray();
		}
		public HTMLElement getElementById(string id,bool onlyChild){
			foreach(HTMLElement e in this.enumElementsById(id,onlyChild))return e;
			return null;
		}
		public Gen::IEnumerable<HTMLElement> enumElementsByTagName(string tagName,bool onlyChild){
			foreach(HTMLNode node in this._childNodes){
				if(node.nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)node;
				if(HTMLElement.IsEqualTagName(elem.tagName,tagName))yield return elem;
				if(onlyChild)continue;
				foreach(HTMLElement elem2 in elem.enumElementsByTagName(tagName,false))yield return elem2;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumElementsByName(string name,bool onlyChild){
			foreach(HTMLNode node in this._childNodes){
				if(node.nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)node;
				if(elem.haveAttribute("name")&&elem.getAttribute("name")==name)yield return elem;
				if(onlyChild)continue;
				foreach(HTMLElement elem2 in elem.enumElementsByName(name,false))yield return elem2;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumElementsById(string id,bool onlyChild){
			foreach(HTMLNode node in this._childNodes){
				if(node.nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)node;
				if(elem.haveAttribute("id")&&elem.getAttribute("id")==id)yield return elem;
				if(onlyChild)continue;
				foreach(HTMLElement elem2 in elem.enumElementsById(id,false))yield return elem2;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumAllElements(bool onlyChild){
			foreach(HTMLNode node in this._childNodes){
				if(node.nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)node;
				yield return elem;
				if(onlyChild)continue;
				foreach(HTMLElement elem2 in elem.enumAllElements(false))yield return elem2;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumFollowingElements(){
			for(int i=this.index;i<this.parent._childNodes.Count;i++){
				if(this.parent._childNodes[i].nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)this.parent._childNodes[i];
				yield return elem;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumFollowingElementsByTagName(string tagName){
			for(int i=this.index+1;i<this.parent._childNodes.Count;i++){
				if(this.parent._childNodes[i].nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)this.parent._childNodes[i];
				if(HTMLElement.IsEqualTagName(elem.tagName,tagName))yield return elem;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumPrecedingElements(){
			for(int i=this.index-1;i>=0;i--){
				if(this.parent._childNodes[i].nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)this.parent._childNodes[i];
				yield return elem;
			}
		}
		public Gen::IEnumerable<HTMLElement> enumPrecedingElementsByTagName(string tagName){
			for(int i=this.index-1;i>=0;i--){
				if(this.parent._childNodes[i].nodeType!=nodeType.ELEMENT_NODE)continue;
				HTMLElement elem=(HTMLElement)this.parent._childNodes[i];
				if(HTMLElement.IsEqualTagName(elem.tagName,tagName))yield return elem;
			}
		}
		//=================================================
		//	methods: attributes に対する操作
		//=================================================
		/// <summary>
		/// 指定した名前の属性値を文字列として取得します。大文字小文字は区別しません。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <returns>属性の値を文字列として返します。</returns>
		public string getAttribute(string attrName){
			return this._attributes.getAttribute(attrName,false);
		}
		/// <summary>
		/// 指定した名前の属性値を文字列として取得します。
		/// 大文字小文字を区別するかどうかを指定出来ます。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <param name="caseSensitive">大文字小文字を区別する時に true 区別しない時には false を指定します。</param>
		/// <returns>属性の値を文字列として返します。</returns>
		public string getAttribute(string attrName,bool caseSensitive){
			return this._attributes.getAttribute(attrName,caseSensitive);
		}
		/// <summary>
		/// 指定した属性のオブジェクトを取得します。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <returns>指定した名前に対応する属性オブジェクトを返します。</returns>
		public HTMLAttribute getAttributeNode(string attrName){
			return this._attributes.getAttributeNode(attrName);
		}
		/// <summary>
		/// 指定した名前の属性に指定した値を設定します。
		/// 属性の名前の大文字小文字は保持します。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <param name="value">設定する値を指定します。</param>
		public void setAttribute(string attrName,string value){
			this._attributes.setAttribute(attrName,value,true);
		}
		/// <summary>
		/// 指定した名前の属性に指定した値を設定します。
		/// 属性の名前の大文字小文字を保持するか否かを指定出来ます。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <param name="value">設定する値を指定します。</param>
		/// <param name="keepCase">
		/// 属性名の大文字小文字を保持する場合には true を、保持しない場合には false を指定します。
		/// 属性名の大文字小文字を保持しない場合には、属性名は強制的に小文字として扱われます。
		/// </param>
		public void setAttribute(string attrName,string value,bool keepCase){
			this._attributes.setAttribute(attrName,value,keepCase);
		}
		/// <summary>
		/// 指定した属性を設定します。
		/// </summary>
		/// <param name="attr">属性を表す HTMLAttribute インスタンスを指定します。</param>
		/// <returns>
		/// 既存の同じ名前の属性があった場合には、その属性は属性リストから削除されます。
		/// その場合、削除された属性が返されます。
		/// 同じ名前の属性がなかった場合には null 値が返されます。
		/// </returns>
		public HTMLAttribute setAttributeNode(HTMLAttribute attr){
			return this._attributes.setAttributeNode(attr);
		}
		/// <summary>
		/// 指定した属性が存在するかどうかを確認します。
		/// 大文字小文字は区別しません。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <returns>指定した属性が存在する場合に true を返します。それ以外の場合には false を返します。</returns>
		public bool haveAttribute(string attrName){
			return this._attributes.haveAttribute(attrName,false);
		}
		/// <summary>
		/// 指定した属性が存在するかどうかを確認します。
		/// 大文字小文字の区別をするかどうか指定出来ます。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <param name="caseSensitive">大文字小文字を区別する場合には true を渡して下さい</param>
		/// <returns>指定した属性が存在する場合に true を返します。それ以外の場合には false を返します。</returns>
		public bool haveAttribute(string attrName,bool caseSensitive) {
			return this._attributes.haveAttribute(attrName,caseSensitive);
		}
		//=================================================
		//	static タグ名比較
		//=================================================
		/// <summary>
		/// 指定した二つのタグ名が同じかどうかを判定します。
		/// 既に HTML として存在するタグ名の場合は大文字・小文字を区別しません。
		/// HTML として登録されていないタグ名・xml のタグ名などの場合には大文字・小文字を区別して判定します。
		/// </summary>
		/// <param name="tag1">比較の対象であるタグ名の一方を指定します。</param>
		/// <param name="tag2">比較の対象であるタグ名のもう一方を指定します。</param>
		/// <returns>
		/// 指定した二つのタグ名が同じであると判定された場合には true を返します。
		/// それ以外の場合には false を返します。</returns>
		public static bool IsEqualTagName(string tag1,string tag2){
			if(tag1==tag2)return true;
			return dictHtmlTagName.ContainsKey(tag1=tag1.ToLower())&&tag1==tag2.ToLower();
		}
		readonly static Gen::Dictionary<string,bool> dictHtmlTagName;
		static HTMLElement(){
			dictHtmlTagName=new Gen::Dictionary<string,bool>();
			string[] tagNames={
				"a","abbr","acronym","address","applet","area","b","base","basefont","bdo",
				"bgsound","big","blockquote","body","br","button","caption","center","cite","code",
				"col","colgroup","comment","dd","del","dfn","dir","div","dl","dt",
				"em","embed","fieldset","font","form","frame","frameset","head","h1","h2",
				"h3","h4","h5","h6","hr","html","i","iframe","img","input",
				"ins","isindex","kbd","label","legend","li","link","listing","map","marquee",
				"menu","meta","nobr","noframes","noscript","object","ol","optgroup","option","p",
				"param","plaintext","q","rt","ruby","rule","s","samp","script","select",
				"small","span","strike","strong","style","sub","sup","table","tbody","td",
				"textarea","tfoot","th","thead","title","tr","tt","u","ul","var",
				"wbr","xmp",
				"blink","ilayer","layer","multicol","nolayer","spacer"
			};
			foreach(string tagName in tagNames)dictHtmlTagName.Add(tagName,true);
		}
	}
	public sealed class XmlDeclaration:HTMLElement{
		public XmlDeclaration(HTMLElement parent):base(parent,"?xml"){}
		public override nodeType nodeType{
			get{return nodeType.xmldeclaration;}
		}
		public override string outerHTML{
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				r.Append("<");
				r.Append(this.tagName);
				foreach(HTMLAttribute attr in this._attributes) {
					r.Append(" ");
					r.Append(attr.name);
					r.Append("=\"");
					r.Append(HTMLDocument.ApplyEntityReference(attr.value.ToString()));
					r.Append("\"");
				}
				r.Append("?>");
				return r.ToString();
			}
		}
	}
	public sealed class ProcInstruction:HTMLElement{
		public ProcInstruction(HTMLElement parent,string name):base(parent,"?"+name){}
		public override nodeType nodeType{
			get{return nodeType.PROCESSING_INSTRUCTION_NODE;}
		}
		public override string outerHTML{
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				r.Append("<");
				r.Append(this.tagName);
				foreach(HTMLAttribute attr in this._attributes) {
					r.Append(" ");
					r.Append(attr.name);
					r.Append("=\"");
					r.Append(HTMLDocument.ApplyEntityReference(attr.value.ToString()));
					r.Append("\"");
				}
				r.Append("?>");
				return r.ToString();
			}
		}
	}
	public sealed class HTMLTextNode:HTMLNode{
		private string _data;
		public HTMLTextNode(HTMLElement parent,string text):base(parent){
			this._data=text;
		}

		#region Properties
		public string data{get{return this._data;}set{this._data=value;}}
		public int length{get{return this._data.Length;}}
		public string nodeValue{get{return this._data;}set{this._data=value;}}
		public override string nodeName{get{return "#text";}}
		public override nodeType nodeType{get{return nodeType.TEXT_NODE;}}
		public override string outerHTML{get{return HTMLDocument.ApplyEntityReference(this._data);}}
		#endregion

		#region Methods
		public void appendData(string str){this._data+=str;}
		public void deleteData(int offset,int count){this._data=this._data.Remove(offset,count);}
		public void insertData(int offset,string str){this._data=this._data.Insert(offset,str);}
		public void replaceData(int offset,int count,string str){this._data=this._data.Remove(offset,count).Insert(offset,str);}
		#endregion
	}
	public sealed class HTMLCDATA:HTMLNode{
		public HTMLCDATA(HTMLElement parent,string text):base(parent){this._text=text;}
		public string _text;
		public string text{get{return this._text;}}
		public override nodeType nodeType{get{return nodeType.CDATA_SECTION_NODE;}}
		public override string nodeName{get{return "#cdata";}}
		public override string outerHTML{
			get{return "<![CDATA["+this._text+"]]>";}
		}
	}
	public sealed class HTMLDOCTYPE:HTMLNode{
		public HTMLDOCTYPE(HTMLElement parent,string text):base(parent){this._text=text;}
		public string _text;
		public string text{get{return this._text;}}
		public override nodeType nodeType{get{return nodeType.DOCUMENT_TYPE_NODE;}}
		public override string nodeName{get{return "#doctype";}}
		public override string outerHTML{
			get{return "<!DOCTYPE"+this._text+">";}
		}
	}
	public sealed class HTMLComment:HTMLNode{
		public HTMLComment(HTMLElement parent,string text):base(parent){this._text=text;}
		public string _text;
		public string text{get{return this._text;}}
		public override nodeType nodeType{get{return nodeType.COMMENT_NODE;}}
		public override string nodeName{get{return "#comment";}}
		public override string outerHTML{
			get{return "<!--"+this._text+"-->";}
		}
	}
}