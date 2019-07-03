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
		//	��{�؍\��
		//-------------------------------------------------------------------------
		#region ITree<HTMLNode> �����o
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
		//	�q�m�[�h�E�q�v�f
		//-------------------------------------------------------------------------
		/// <summary>
		/// �q�m�[�h�̏W�����擾���܂��B
		/// </summary>
		public Gen::IList<HTMLNode> childNodes{
			get{return this._childNodes;}
		}
		/// <summary>
		/// ��ԍŏ��̎q�m�[�h���擾���܂��B
		/// </summary>
		public HTMLNode firstChild{
			get{return this._childNodes.Count==0?null:this._childNodes[0];}
		}
		/// <summary>
		/// ��ԍŌ�̎q�m�[�h���擾���܂��B
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
		//	methods: ���e������Ɋւ��鑀��
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
		//	methods: �v�f�񋓂Ɋւ��鑀��
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
		//	methods: attributes �ɑ΂��鑀��
		//=================================================
		/// <summary>
		/// �w�肵�����O�̑����l�𕶎���Ƃ��Ď擾���܂��B�啶���������͋�ʂ��܂���B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <returns>�����̒l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public string getAttribute(string attrName){
			return this._attributes.getAttribute(attrName,false);
		}
		/// <summary>
		/// �w�肵�����O�̑����l�𕶎���Ƃ��Ď擾���܂��B
		/// �啶������������ʂ��邩�ǂ������w��o���܂��B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <param name="caseSensitive">�啶������������ʂ��鎞�� true ��ʂ��Ȃ����ɂ� false ���w�肵�܂��B</param>
		/// <returns>�����̒l�𕶎���Ƃ��ĕԂ��܂��B</returns>
		public string getAttribute(string attrName,bool caseSensitive){
			return this._attributes.getAttribute(attrName,caseSensitive);
		}
		/// <summary>
		/// �w�肵�������̃I�u�W�F�N�g���擾���܂��B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <returns>�w�肵�����O�ɑΉ����鑮���I�u�W�F�N�g��Ԃ��܂��B</returns>
		public HTMLAttribute getAttributeNode(string attrName){
			return this._attributes.getAttributeNode(attrName);
		}
		/// <summary>
		/// �w�肵�����O�̑����Ɏw�肵���l��ݒ肵�܂��B
		/// �����̖��O�̑啶���������͕ێ����܂��B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <param name="value">�ݒ肷��l���w�肵�܂��B</param>
		public void setAttribute(string attrName,string value){
			this._attributes.setAttribute(attrName,value,true);
		}
		/// <summary>
		/// �w�肵�����O�̑����Ɏw�肵���l��ݒ肵�܂��B
		/// �����̖��O�̑啶����������ێ����邩�ۂ����w��o���܂��B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <param name="value">�ݒ肷��l���w�肵�܂��B</param>
		/// <param name="keepCase">
		/// �������̑啶����������ێ�����ꍇ�ɂ� true ���A�ێ����Ȃ��ꍇ�ɂ� false ���w�肵�܂��B
		/// �������̑啶����������ێ����Ȃ��ꍇ�ɂ́A�������͋����I�ɏ������Ƃ��Ĉ����܂��B
		/// </param>
		public void setAttribute(string attrName,string value,bool keepCase){
			this._attributes.setAttribute(attrName,value,keepCase);
		}
		/// <summary>
		/// �w�肵��������ݒ肵�܂��B
		/// </summary>
		/// <param name="attr">������\�� HTMLAttribute �C���X�^���X���w�肵�܂��B</param>
		/// <returns>
		/// �����̓������O�̑������������ꍇ�ɂ́A���̑����͑������X�g����폜����܂��B
		/// ���̏ꍇ�A�폜���ꂽ�������Ԃ���܂��B
		/// �������O�̑������Ȃ������ꍇ�ɂ� null �l���Ԃ���܂��B
		/// </returns>
		public HTMLAttribute setAttributeNode(HTMLAttribute attr){
			return this._attributes.setAttributeNode(attr);
		}
		/// <summary>
		/// �w�肵�����������݂��邩�ǂ������m�F���܂��B
		/// �啶���������͋�ʂ��܂���B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <returns>�w�肵�����������݂���ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool haveAttribute(string attrName){
			return this._attributes.haveAttribute(attrName,false);
		}
		/// <summary>
		/// �w�肵�����������݂��邩�ǂ������m�F���܂��B
		/// �啶���������̋�ʂ����邩�ǂ����w��o���܂��B
		/// </summary>
		/// <param name="attrName">�����̖��O���w�肵�܂��B</param>
		/// <param name="caseSensitive">�啶������������ʂ���ꍇ�ɂ� true ��n���ĉ�����</param>
		/// <returns>�w�肵�����������݂���ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool haveAttribute(string attrName,bool caseSensitive) {
			return this._attributes.haveAttribute(attrName,caseSensitive);
		}
		//=================================================
		//	static �^�O����r
		//=================================================
		/// <summary>
		/// �w�肵����̃^�O�����������ǂ����𔻒肵�܂��B
		/// ���� HTML �Ƃ��đ��݂���^�O���̏ꍇ�͑啶���E����������ʂ��܂���B
		/// HTML �Ƃ��ēo�^����Ă��Ȃ��^�O���Exml �̃^�O���Ȃǂ̏ꍇ�ɂ͑啶���E����������ʂ��Ĕ��肵�܂��B
		/// </summary>
		/// <param name="tag1">��r�̑Ώۂł���^�O���̈�����w�肵�܂��B</param>
		/// <param name="tag2">��r�̑Ώۂł���^�O���̂���������w�肵�܂��B</param>
		/// <returns>
		/// �w�肵����̃^�O���������ł���Ɣ��肳�ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
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