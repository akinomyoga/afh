namespace afh.HTML{
	public class HTMLAttributeCollection:System.Collections.Generic.IEnumerable<HTMLAttribute>{
		private HTMLElement parent;
		public System.Collections.Generic.SortedList<string,HTMLAttribute> list;
		public HTMLAttributeCollection(HTMLElement parent){
			this.parent=parent;
			this.list=new System.Collections.Generic.SortedList<string,HTMLAttribute>();
		}
		static HTMLAttributeCollection(){
			System.Globalization.CultureInfo ci=new System.Globalization.CultureInfo(0x11); //日本語
			strcmp=System.StringComparer.Create(ci,true);
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.list.Values.GetEnumerator();
		}
		public System.Collections.Generic.IEnumerator<HTMLAttribute> GetEnumerator(){
			return this.list.Values.GetEnumerator();
		}
		/// <summary>
		/// 指定した key に一致する属性を大文字小文字を区別せずに検索します。
		/// </summary>
		/// <param name="key">検索する属性の名前を指定します。</param>
		/// <returns>
		/// 指定した key に一致する属性のインデックスの内最小の物を返します。
		/// 一致する属性が見つからなかった場合、-1 を返します。
		/// </returns>
		private int SearchCaseInsensitive(string key){
			int min=0;
			int max=list.Count;
			if(max<=0)return -1;
			int c=max/2;
			do{
				int val=strcmp.Compare(key,list.Keys[c]);
				if(val==0){
					while(c>0&&strcmp.Compare(key,list.Keys[c-1])==0)c--;
					return c;
				}
				if(val<0){
					max=c;
					c=(max+min)/2;
				}else{
					min=c+1;
					c=(min+max)/2;
				}
			}while(min!=max);
			return -1;
		}
		/// <summary>
		/// 大文字小文字を区別しない文字列比較オブジェクトです。
		/// </summary>
		private static System.StringComparer strcmp;
		public override string ToString() {
			System.Text.StringBuilder r=new System.Text.StringBuilder();
			foreach(HTMLAttribute attr in this) {
				r.Append(" ");
				r.Append(attr.name);
				r.Append("=\"");
				r.Append(HTMLDocument.ApplyEntityReference(attr.value.ToString()));
				r.Append("\"");
			}
			return r.ToString();
		}
		//===========================================================
		//		Properties & Methods
		//===========================================================
		public int length{get{return this.list.Count;}}
		public HTMLAttribute getNamedItem(string attrName){
			if(this.list.ContainsKey(attrName))return this.list[attrName];
			return HTMLAttribute.CreateUnspecified(this.parent,attrName);
		}
		public HTMLAttribute removeNamedItem(string attrName){
			if(!this.list.ContainsKey(attrName))return null;
			HTMLAttribute ret=this.list[attrName];
			return this.list.Remove(attrName)?ret:null;
		}
		public HTMLAttribute item(string attrName){
			return this.getNamedItem(attrName);
		}
		public HTMLAttribute item(int index){
			if(index<0||this.list.Count<=index)return null;
			return this.list.Values[index];
		}
		public HTMLAttribute setNamedItem(HTMLAttribute newAttribute){
			HTMLAttribute ret=this.list.ContainsKey(newAttribute.name)?this.list[newAttribute.name]:null;
			this.list[newAttribute.name]=newAttribute;
			return ret;
		}
		//===========================================================
		//		internal operations
		//===========================================================
		internal int IndexOf(HTMLAttribute attr){
			return this.list.IndexOfValue(attr);
		}
		internal string getAttribute(string attrName,bool caseSensitive){
			if(caseSensitive){
				if(this.list.ContainsKey(attrName))return this.list[attrName].ToString();
			}else{
				int index=this.SearchCaseInsensitive(attrName);
				if(index>=0)return this.list.Values[index].ToString();
			}
			return "";
		}
		internal void setAttribute(string attrName,object value,bool keepCase){
			if(!keepCase)attrName=attrName.ToLower();
			this.list[attrName]=new HTMLAttribute(this.parent,attrName,value);
		}
		internal bool removeAttribute(string attrName,bool caseSensitive){
			if(caseSensitive){
				if(this.list.Remove(attrName))return true;
				return false;
			}else{
				int index=this.SearchCaseInsensitive(attrName);
				if(index<0)return false;
				while(index<this.list.Count&&strcmp.Compare(attrName,this.list.Keys[index])==0){
					this.list.RemoveAt(index);
				}
				return true;
			}
		}
		internal HTMLAttribute getAttributeNode(string attrName){
			if(this.list.ContainsKey(attrName))return this.list[attrName];
			int index=this.SearchCaseInsensitive(attrName);
			if(index>=0)return this.list.Values[index];
			return HTMLAttribute.CreateUnspecified(this.parent,attrName);
		}
		internal HTMLAttribute setAttributeNode(HTMLAttribute attr){
			HTMLAttribute ret=this.list.ContainsKey(attr.name)?this.list[attr.name]:null;
			this.list[attr.name]=attr;
			return ret;
		}
		internal HTMLAttribute removeAttributeNode(HTMLAttribute attr){
			return this.list.Values.Remove(attr)?attr:null;
		}
		/// <summary>
		/// 指定した属性が存在するかどうかを確認します。
		/// 大文字小文字の区別をするかどうか指定出来ます。
		/// </summary>
		/// <param name="attrName">属性の名前を指定します。</param>
		/// <param name="caseSensitive">大文字小文字を区別する場合には true を渡して下さい</param>
		/// <returns>指定した属性が存在する場合に true を返します。それ以外の場合には false を返します。</returns>
		internal bool haveAttribute(string attrName,bool caseSensitive){
			if(this.list.ContainsKey(attrName))return true;
			return !caseSensitive&&this.SearchCaseInsensitive(attrName)>=0;
		}
	}
	/// <summary>
	/// HTML の属性を表現するクラスです。
	/// </summary>
	public class HTMLAttribute{
		private HTMLElement parent;
		private string _name;
		private object _value;
		private bool _specified=true;
		public HTMLAttribute(HTMLElement parent,string name,object value){
			this.parent=parent;
			this._name=name;
			this._value=value;
		}
		public HTMLElement parentNode{
			get{return this.parent;}
		}
		private int index{
			get{
				if(this.parent==null)return -1;
				return this.parent._attributes.IndexOf(this);
			}
		}

		public override string ToString(){
			return value==null?"":this.value.ToString();
		}
		internal static HTMLAttribute CreateUnspecified(HTMLElement parent,string name){
			HTMLAttribute r=new HTMLAttribute(parent,name,null);
			r._specified=false;
			return r;
		}
		//===========================================================
		//		Properties
		//===========================================================
		public HTMLAttribute previousSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index-1;
				return index<0?null:this.parent._attributes.item(index);
			}
		}
		public HTMLAttribute nextSibling{
			get{
				if(this.parent==null)return null;
				int index=this.index+1;
				return this.parent._attributes.length<=index?null:this.parent._attributes.item(index);
			}
		}
		public nodeType nodeType{get{return nodeType.ATTRIBUTE_NODE;}}
		public string nodeName{
			get{return this._name;}
			set{this._name=value;}
		}
		public object nodeValue{
			get{return this._value;}
			set{this._value=value;}
		}
		public string name{
			get{return this._name;}
			set{this._name=value;}
		}
		public object value{
			get{return this._value;}
			set{this._value=value;}
		}
		public bool specified{
			get{return this._specified;}
			internal set{this._specified=value;}
		}
		//===========================================================
		//		Methods
		//===========================================================
		public bool hasChildNodes(){return false;}
		public HTMLAttribute cloneNode(){
			return new HTMLAttribute(null,this._name,this._value);
		}
	}
}