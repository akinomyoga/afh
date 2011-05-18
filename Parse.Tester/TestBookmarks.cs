using Gen=System.Collections.Generic;
using afh.Collections.Utils;

namespace afh.Application.Bookmarks{
	class FaviconData{
		public int id;
		public string url;
		public string data;
	}
	class FaviconList{
		int idmax=-1;
		readonly Gen::Dictionary<string,FaviconData> dict_url=new System.Collections.Generic.Dictionary<string,FaviconData>();
		readonly Gen::SortedList<int,FaviconData> dict_id=new System.Collections.Generic.SortedList<int,FaviconData>();

		public FaviconList(){}
		public FaviconList(string filename){
			this.LoadXml(filename);
		}
		void LoadXml(string filename){
			idmax=-1;
			dict_url.Clear();
			dict_id.Clear();

			System.Xml.XmlDocument doc=new System.Xml.XmlDocument();
			doc.Load(filename);
			foreach(System.Xml.XmlElement elem in doc.GetElementsByTagName("favicon")){
				FaviconData icon=new FaviconData();
				if(!int.TryParse(elem.GetAttribute("id"),out icon.id))continue;
				icon.url=elem.GetAttribute("url");
				icon.data=elem.GetAttribute("data");

				if(idmax<icon.id)idmax=icon.id;
				dict_url[icon.url]=icon;
				dict_id[icon.id]=icon;
			}
		}
		//-------------------------------------------------------------------------
		public void SaveXml(string filename){
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter("bm_fav.xml")){
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sw.WriteLine("<favicons>");
				foreach(FaviconData icon in dict_id.Values)
					sw.WriteLine("  <favicon id=\"{0}\" url=\"{1}\" data=\"{2}\" />",icon.id,icon.url,icon.data);
				sw.WriteLine("</favicons>");
			}
		}
		public void WriteCss(string filename){
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter(filename)){
				sw.WriteLine("/* favicons */");
				sw.WriteLine("li[favicon]{padding-left:20px;background-position:left top;background-repeat:no-repeat;margin-bottom:2px;}");
				foreach(FaviconData icon in dict_id.Values){
					string k=icon.url;
					string d=icon.data;
					if(d=="")d=k;
					sw.WriteLine("li[favicon=\"{0}\"]{{background-image:url({1});}}",k,d);
				}
			}
		}
		//-------------------------------------------------------------------------
		public void Add(string url,string data){
			if(dict_url.ContainsKey(url)){
				if(data!="")dict_url[url].data=data; // 更新
			}else{
				// 新規追加
				FaviconData icon=new FaviconData();
				icon.id=++idmax;
				icon.url=url;
				icon.data=data;

				// 追加
				dict_url[icon.url]=icon;
				dict_id[icon.id]=icon;
			}
		}
	}
	//---------------------------------------------------------------------------
	//---------------------------------------------------------------------------
	public abstract class BookmarkNode:afh.Collections.Design.TreeBase<BookmarkNode>{
		/// <summary>
		/// Unix 時刻から System.DateTime に変換を行います。
		/// </summary>
		/// <param name="unixtime">時刻を unix 時刻 (整数) の文字列で指定します。</param>
		/// <returns>指定した unix 時刻を System.DateTime で表現した物を返します。</returns>
		public static System.DateTime UnixTime2DateTime(string unixtime){
			long buff;
			if(long.TryParse(unixtime,out buff))
				return UnixTime2DateTime(buff);
			else
				return System.DateTime.MinValue;
		}
		/// <summary>
		/// Unix 時刻から System.DateTime に変換を行います。
		/// </summary>
		/// <param name="unixtime">時刻を unix 時刻 (1970/01/01 00:00:00 からの秒数) で指定します。</param>
		/// <returns>指定した unix 時刻を System.DateTime で表現した物を返します。</returns>
		public static System.DateTime UnixTime2DateTime(long unixtime){
			return new System.DateTime(1970,1,1,0,0,0).AddSeconds(unixtime);
		}
	}

#if OLD
	#region class:EmptyBookmarkList
	internal class EmptyBookmarkList:Gen::IList<BookmarkNode>{
		static EmptyBookmarkList inst=new EmptyBookmarkList();
		public static EmptyBookmarkList Instance{
			get{return inst;}
		}

		public int IndexOf(BookmarkNode item){return -1;}
		public void Insert(int index,BookmarkNode item){throw new System.NotSupportedException();}
		public void RemoveAt(int index){throw new System.NotSupportedException();}
		public BookmarkNode this[int index] {
			get{throw new System.ArgumentOutOfRangeException("index");}
			set{throw new System.ArgumentOutOfRangeException("index");}
		}
		public void Add(BookmarkNode item){
			throw new System.NotSupportedException();
		}
		public void Clear(){return;}
		public bool Contains(BookmarkNode item){return false;}
		public void CopyTo(BookmarkNode[] array,int arrayIndex){
			if(arrayIndex<0||arrayIndex>=array.Length)
				throw new System.ArgumentOutOfRangeException("index");
			return;
		}
		public int Count{get{return 0;}}
		public bool IsReadOnly{get{return true;}}
		public bool Remove(BookmarkNode item){return false;}
		public System.Collections.Generic.IEnumerator<BookmarkNode> GetEnumerator(){yield break;}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
	}
	#endregion
#endif

	public class BookmarkLink:BookmarkNode{
		public string name;
		public string url;
		public string charset;
		public string title;
		public string description;
		public System.DateTime ctime; // creation time
		public System.DateTime utime; // updated time
		public int faviId=-1; // favicon id;

		public BookmarkLink(){
			this.NodesReadOnly=true;
		}
	}
	public class BookmarkDir:BookmarkNode{
		public string name;
		public string description;
		public System.DateTime ctime; // creation time
		public System.DateTime utime; // updated time
	}
	public class BookmarkSep:BookmarkNode{
		public BookmarkSep(){
			this.NodesReadOnly=true;
		}
	}

	public class BookmarkTree{
		readonly BookmarkDir root;
		public BookmarkDir RootNode{
			get{return root;}
		}

		public BookmarkTree(){
			this.root=new BookmarkDir();
		}

		/// <summary>
		/// bookmark の内容を xml ファイルに書き出します。
		/// </summary>
		/// <param name="filename">書き出し先のファイル名を指定します。</param>
		public void ExportToXml(string filename){
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter(filename)){
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sw.WriteLine("<bookmarks>");
				string indent="  ";
				this.ExportToXml_WriteNodes(sw,this.root.Nodes,ref indent);
				sw.WriteLine("</bookmarks>");
			}
		}
		void ExportToXml_WriteNodes(System.IO.StreamWriter sw,Gen::IList<BookmarkNode> nodelist,ref string indent){
			foreach(BookmarkNode node in nodelist){
				BookmarkLink lnk=node as BookmarkLink;
				if(lnk!=null){
					sw.Write(indent);
					sw.Write("<link name=\"{0}\" url=\"{1}\" ctime=\"{2}\" utime=\"{3}\" charset=\"{4}\"",
						afh.Text.TextUtils.EscapeXml(lnk.name),
						afh.Text.TextUtils.EscapeXml(lnk.url),
						afh.Text.TextUtils.EscapeXml(lnk.ctime.ToString("u")),
						afh.Text.TextUtils.EscapeXml(lnk.utime.ToString("u")),
						afh.Text.TextUtils.EscapeXml(lnk.charset)
						);
					if(lnk.faviId>=0)
						sw.Write(" favicon=\"{0}\"",lnk.faviId);
					if(lnk.title!=null)
						sw.Write(" title=\"{0}\"",afh.Text.TextUtils.EscapeXml(lnk.title));
					if(lnk.description!=null)
						sw.Write(" desc=\"{0}\"",afh.Text.TextUtils.EscapeXml(lnk.description));
					sw.WriteLine("/>");
					continue;
				}

				BookmarkDir dir=node as BookmarkDir;
				if(dir!=null){
					sw.Write(indent);
					sw.Write("<directory name=\"{0}\" ctime=\"{1}\" utime=\"{2}\"",
						afh.Text.TextUtils.EscapeXml(dir.name),
						afh.Text.TextUtils.EscapeXml(dir.ctime.ToString("u")),
						afh.Text.TextUtils.EscapeXml(dir.utime.ToString("u"))
						);
					if(dir.description!=null)
						sw.Write(" desc=\"{0}\"",afh.Text.TextUtils.EscapeXml(dir.description));
					sw.WriteLine(">");
					indent="  "+indent;
					this.ExportToXml_WriteNodes(sw,dir.Nodes,ref indent);
					indent=indent.Substring(2);
					sw.Write(indent);
					sw.WriteLine("</directory>");
					continue;
				}

				BookmarkSep sep=node as BookmarkSep;
				if(sep!=null){
					sw.Write(indent);
					sw.WriteLine("<separator />");
					continue;
				}
			}
		}
		/// <summary>
		/// Firefox から出力される bookmark.html を解析して、
		/// その内容に対応する BookmarkTree を生成します。
		/// </summary>
		/// <param name="document">Firefox から出力される bookmarks.html の内容を保持する HTMLDocument を指定します。</param>
		/// <returns>生成した BookmarkTree を返します。</returns>
		public static BookmarkTree CreateFromFirefoxBookmarkHtml(afh.HTML.HTMLDocument document){
			BookmarkTree ret=new BookmarkTree();
			BookmarkDir currentDir=ret.RootNode;
			BookmarkNode currentNode=ret.RootNode;
			foreach(afh.HTML.HTMLElement elem in document.enumAllElements(false)){
				switch(elem.tagName){
					case "H3":{
						BookmarkDir newDir=new BookmarkDir();
						newDir.name=elem.innerText;
						newDir.ctime=BookmarkNode.UnixTime2DateTime(elem.getAttribute("add_date",false));
						newDir.utime=BookmarkNode.UnixTime2DateTime(elem.getAttribute("last_modified",false));

						currentDir.Nodes.Add(newDir);
						currentDir=newDir;
						currentNode=newDir;
						break;
					}
					case "HR":
						currentDir.Nodes.Add(currentNode=new BookmarkSep());
						break;
					case "DT":{
						afh.HTML.HTMLElement a=elem.enumElementsByTagName("A",true).First();
						if(a==null)break;

						BookmarkLink newNode=new BookmarkLink();
						newNode.name=a.innerText;
						newNode.url=a.getAttribute("href",false);
						newNode.ctime=BookmarkNode.UnixTime2DateTime(a.getAttribute("add_date",false));
						newNode.utime=BookmarkNode.UnixTime2DateTime(a.getAttribute("last_modified",false));
						newNode.charset=a.getAttribute("last_charset",false);

						currentDir.Nodes.Add(newNode);
						currentNode=newNode;
						break;
					}
					case "DD":
						string text="";
						foreach(afh.HTML.HTMLNode node in elem.childNodes){
							afh.HTML.HTMLTextNode textNode=node as afh.HTML.HTMLTextNode;
							if(textNode==null)continue;

							text=textNode.data.Trim();
							break;
						}
						if(text=="")break;

						BookmarkDir isDir=currentNode as BookmarkDir;
						if(isDir!=null){
							isDir.description=text;
							break;
						}

						BookmarkLink isLnk=currentNode as BookmarkLink;
						if(isLnk!=null){
							isLnk.description=text;
							break;
						}
						break;
				}

				// directory 終了判定 (本当にこれで大丈夫か?)
				if(elem.parentNode.lastChild==elem){
					afh.HTML.HTMLElement directory=null;
					// DL>last or DL>last-child:DD>last
					if(elem.tagName=="p"){
						if(elem.childNodes.Count==0)
							directory=elem.parentNode;
					}else if(elem.parentNode.tagName=="p"){
						afh.HTML.HTMLElement p=elem.parentNode;
						if(p.parentNode!=null&&p.parentNode.lastChild==p){
							directory=p.parentNode;
						}
					}

					if(directory!=null){
						if(directory.tagName=="DD"&&directory.parentNode!=null)
							directory=directory.parentNode;
						if(directory.tagName=="DL"&&directory.parentNode!=null&&directory.parentNode.nodeType==afh.HTML.nodeType.ELEMENT_NODE){
							// go to outer directory
							if(currentDir!=ret.RootNode)
								currentDir=(BookmarkDir)currentDir.Parent;
						}
					}
				}
			}
		
			return ret;
		}
	}
	//---------------------------------------------------------------------------
	//	Firefox Bookmark
	//---------------------------------------------------------------------------
	class Test{
		public static void extract_bookmarks1(string filename){
			string html=System.IO.File.ReadAllText(filename);
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(html);

			using(System.IO.StreamWriter sw=new System.IO.StreamWriter("bm_all.xml")){
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sw.WriteLine("<bookmarks>");
				string indent="  ";
				foreach(afh.HTML.HTMLElement elem in doc.enumAllElements(false)){
					switch(elem.tagName){
						case "H3":
							sw.Write(indent);
							sw.WriteLine("<directory name=\"{0}\">",
								afh.Text.TextUtils.EscapeXml(elem.innerText)
								);
							indent+="  ";
							break;
						case "HR":
							sw.Write(indent);
							sw.WriteLine("<separator/>");
							break;
						case "DT":
							string name=null;
							string url=null;
							foreach(afh.HTML.HTMLElement a in elem.enumElementsByTagName("A",true)){
								name=a.innerText;
								url=a.getAttribute("href",false);
								break;
							}
							if(url==null)break;

							sw.Write(indent);
							sw.WriteLine("<link name=\"{0}\" url=\"{1}\"/>",
								afh.Text.TextUtils.EscapeXml(name),
								afh.Text.TextUtils.EscapeXml(url)
								);
							break;
						case "DD":
							string text="";
							foreach(afh.HTML.HTMLNode node in elem.childNodes){
								afh.HTML.HTMLTextNode textNode=node as afh.HTML.HTMLTextNode;
								if(textNode==null)continue;

								text=textNode.data;
								break;
							}
							if(text!=""){
								sw.Write(indent);
								sw.WriteLine("<desc>{0}</desc>",
									afh.Text.TextUtils.EscapeXml(text.Trim())
									);
							}
							break;
					}

					if(elem.parentNode.lastChild==elem){
						afh.HTML.HTMLElement directory=null;
						// DL>last or DL>last-child:DD>last
						if(elem.tagName=="p"){
							if(elem.childNodes.Count==0)
								directory=elem.parentNode;
						}else if(elem.parentNode.tagName=="p"){
							afh.HTML.HTMLElement p=elem.parentNode;
							if(p.parentNode!=null&&p.parentNode.lastChild==p){
								directory=p.parentNode;
							}
						}

						if(directory!=null){
							if(directory.tagName=="DD"&&directory.parentNode!=null)
								directory=directory.parentNode;
							if(directory.tagName=="DL"&&directory.parentNode!=null&&directory.parentNode.nodeType==afh.HTML.nodeType.ELEMENT_NODE){
								indent=indent.Substring(2);
								sw.Write(indent);
								sw.WriteLine("</directory>");
							}
						}
					}

				}
				sw.WriteLine("</bookmarks>");
			}
			
		}
		public static void extract_bookmarks2(string filename){
			string html=System.IO.File.ReadAllText(filename);
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(html);
			BookmarkTree result=BookmarkTree.CreateFromFirefoxBookmarkHtml(doc);
			result.ExportToXml("bm_all2.xml");
		}

		public static void extract_favicon1(string filename){
			string html=System.IO.File.ReadAllText(filename);
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(html);
			
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter("bm_favicons.css")){
				sw.WriteLine("/* favicons */");
				sw.WriteLine("li[favicon]{padding-left:20px;background-position:left top;background-repeat:no-repeat;margin-bottom:2px;}");

				//Gen::Dictionary<string,string> dict=new Gen::Dictionary<string,string>();
				//foreach(afh.HTML.HTMLElement a in doc.enumElementsByTagName("a",false)){
				//  string k=a.getAttribute("icon_uri",false);
				//  if(k==null||k==""||dict.ContainsKey(k))continue;
				//  string data=a.getAttribute("icon",false);
				//  if(data=="")data=k;
				//  dict.Add(k,data);
				//  sw.WriteLine("li[favicon=\"{0}\"]{{background-image:url({1});}}",k,data);
				//}

				//foreach(afh.HTML.HTMLElement a in doc.enumElementsByTagName("a",false)){
				//  string k=a.getAttribute("icon_uri",false);
				//  if(k==null||k=="")continue;
				//  string data=a.getAttribute("icon",false);
				//  if(data=="")data=k;
				//  sw.WriteLine("li[favicon=\"{0}\"]{{background-image:url({1});}}",k,data);
				//}

				Gen::SortedList<string,string> dict=new Gen::SortedList<string,string>();
				foreach(afh.HTML.HTMLElement a in doc.enumElementsByTagName("a",false)){
					string k=a.getAttribute("icon_uri",false);
					if(k==null||k=="")continue;

					string data=a.getAttribute("icon",false);
					if(dict.ContainsKey(k)&&dict[k]!="")continue;

					dict[k]=data;
				}
				foreach(Gen::KeyValuePair<string,string> p in dict){
					string k=p.Key;
					string d=p.Value;
					if(d=="")d=k;
					sw.WriteLine("li[favicon=\"{0}\"]{{background-image:url({1});}}",k,d);
				}
			}
		}
		public static void extract_favicon2(string filename){
			string html=System.IO.File.ReadAllText(filename);
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(html);
			
			// read bookmarks.html
			Gen::SortedList<string,string> dict=new Gen::SortedList<string,string>();
			foreach(afh.HTML.HTMLElement a in doc.enumElementsByTagName("a",false)){
				string k=a.getAttribute("icon_uri",false);
				if(k==null||k=="")continue;

				string data=a.getAttribute("icon",false);
				if(dict.ContainsKey(k)&&dict[k]!="")continue;

				dict[k]=data;
			}

			// output xml
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter("bm_fav.xml")){
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sw.WriteLine("<favicons>");
				foreach(Gen::KeyValuePair<string,string> p in dict){
					string k=p.Key;
					string d=p.Value;
					sw.WriteLine("  <favicon url=\"{0}\" data=\"{1}\" />",k,d);
				}
				sw.WriteLine("</favicons>");
			}

			// output css
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter("bm_fav.css")){
				sw.WriteLine("/* favicons */");
				sw.WriteLine("li[favicon]{padding-left:20px;background-position:left top;background-repeat:no-repeat;margin-bottom:2px;}");
				foreach(Gen::KeyValuePair<string,string> p in dict){
					string k=p.Key;
					string d=p.Value;
					if(d=="")d=k;
					sw.WriteLine("li[favicon=\"{0}\"]{{background-image:url({1});}}",k,d);
				}
			}
		}
		public static void extract_favicon3(string filename){
			string html=System.IO.File.ReadAllText(filename);
			afh.HTML.HTMLDocument doc=afh.HTML.HTMLDocument.Parse(html);
			
			// read bookmarks.html
			FaviconList flist=new FaviconList();
			foreach(afh.HTML.HTMLElement a in doc.enumElementsByTagName("a",false)){
				string k=a.getAttribute("icon_uri",false);
				if(k==null||k=="")continue;
				flist.Add(k,a.getAttribute("icon",false));
			}

			flist.SaveXml("bm_fav.xml");
			flist.WriteCss("bm_fav.css");
		}
	}
}