using afh.Parse;
using Gen=System.Collections.Generic;
namespace afh.HTML{
	/// <summary>
	/// HTML の構文解析を行います。
	/// </summary>
	public class HTMLParser{
		private HTMLWordReader wreader;
		private HTMLDocument doc;
		/// <summary>
		/// 現在の要素を参照します。
		/// </summary>
		private HTMLElement celem;
		private HTMLParser(string text){
			this.wreader=new HTMLWordReader(text);
		}
		static HTMLParser(){
			initialize_judgeNest();
		}
		/// <summary>
		/// HTML ソース文字列から HTMLDocument を初期化します。
		/// HTMLDocument.Parse(string) から使用して下さい。
		/// </summary>
		/// <param name="html">HTML ソースを指定します。</param>
		/// <returns>生成した HTMLDocument インスタンスを返します。</returns>
		internal static HTMLDocument Parse(string html){
			HTMLParser inst=new HTMLParser(html);
			inst.Parse();
			foreach(Gen::KeyValuePair<LinearLetterReader.TextRange,AnalyzeError> pair in inst.wreader.LetterReader.EnumErrors()){
				HTMLError err=new HTMLError();
				err.type=HTMLErrorType.ParseError;
				err.sourceName="<string:"+html.Length+"> "+html.Substring(0,30);
				err.start=pair.Key.start;
				err.end=pair.Key.end;
				err.message=pair.Value.message;
				inst.doc.ErrorList.Add(err);
			}
			return inst.doc;
		}
		//=========================================================================
		private void Parse(){
			this.doc=new HTMLDocument();
			this.celem=this.doc;
			this.wreader.context=HTMLWordReader.Context.global;
			while(this.wreader.ReadNext()){
				switch(this.wreader.CurrentType.value){
					case WordType.vComment:
						this.celem.appendChild(new HTMLComment(this.celem,this.wreader.CurrentWord));
						break;
					case HTMLWordReader.vCDATA:
						this.celem.appendChild(new HTMLCDATA(this.celem,this.wreader.CurrentWord));
						break;
					case HTMLWordReader.vDOCTYPE:
						this.celem.appendChild(new HTMLDOCTYPE(this.celem,this.wreader.CurrentWord));
						break;
					case HTMLWordReader.vText:
						string text=HTMLDocument.ResolveEntityReference(this.wreader.CurrentWord);
						this.celem.appendChild(new HTMLTextNode(this.celem,text));
						break;
					case WordType.vOperator:
						switch(this.wreader.CurrentWord){
							case "<":
								ReadElement();
								break;
							case "</":
								ReadEndElement();
								break;
							case "<?":
								ReadProcessInstruction();
								break;
							//case "<%":
						}
						break;
				}
			}
		}
		//-------------------------------------------------------------------------
		//		ReadElement
		//-------------------------------------------------------------------------
		private void ReadElement(){
			this.wreader.context=HTMLWordReader.Context.NMTOKEN;
			if(!this.wreader.ReadNext()){
				this.wreader.LinearReader.SetError("要素名がありません。< に続けて要素名を指定して下さい",0,null);
				this.wreader.context=HTMLWordReader.Context.global;
				return;
			}
			// 現在の要素の終了判定
			string name=this.wreader.CurrentWord;
			while(!JudgeNest(this.celem.tagName,name)){
				// ※ document/html/head/body の TagName は #document/html/head/body
				//	→ これは judgeNest_table に含まれていないから JudgeNest は true になる
				//	→ 必ず this.celem=document/html/head/body で止まるので this.celem!=null
				this.celem=this.celem.parentNode;
			}

			// 新しい要素の登録
			HTMLElement elem=new HTMLElement(this.celem,name);
			this.celem.appendChild(elem);

			// 属性
			this.wreader.context=HTMLWordReader.Context.attribute;
			while(this.wreader.ReadNext()){
				switch(this.wreader.CurrentType.value){
					case HTMLWordReader.vAttribute:
						string[] x=this.wreader.CurrentWord.Split(new char[]{'='},2);
						elem.setAttribute(x[0],HTMLDocument.ResolveEntityReference(x[1]));
						break;
					case WordType.vOperator:
						//this.wreader.CurrentWord= ">"|"?>"|"/>"
						if(this.wreader.CurrentWord==">"){
							if(!readElement_closedElem.Contains(elem.nodeName))this.celem=elem;
						}else if(this.wreader.CurrentWord=="?>"){
							this.wreader.LinearReader.SetError("?> で終わるのは xml 宣言及びプロセス命令のみです。> か /> で終わるようにして下さい。",0,null);
						}
						this.wreader.context=HTMLWordReader.Context.global;
						return;
					default:
						// 無効な文字
						break;
				}
			}
		}

		#region ReadElement subfunctions
		private static System.Collections.Generic.List<string> readElement_closedElem
			=new System.Collections.Generic.List<string>(new string[]{
				"meta","link","base","basefont","bgsound",
				"img","input","isindex",
				"br","hr","wbr","spacer",
				"col","frame","area","param"
				// "keygen","embed","source",
			});
		/// <summary>
		/// 指定した要素に指定した要素を入れ子に出来るかどうかを取得します。
		/// </summary>
		/// <param name="parent">
		/// 現在の文脈である要素のタグ名を指定します。新しい要素の親となりうる要素のタグ名です。
		/// </param>
		/// <param name="child">
		/// 新しい要素の開始タグ名を指定します。parent の子要素と為りうる要素のタグ名です。</param>
		/// <returns>
		/// parent の子要素として child を認めるかどうかを返します。
		/// parent の子要素として child を認める場合には true を返します。
		/// parent の子要素として child を認めず、parent の隠れ終了タグをそこに想定する場合には false を返します。
		/// </returns>
		/// <remarks>
		/// これは HTML の文法上の規則に従って結果を返すのではなく、
		/// parent の終了タグが省略可能の時に、何処に終了タグがあると見なすかを判定する為にあります。
		/// <para>
		/// 例えば HTML の文法では body 要素の中に meta は来ては為りませんが、
		/// JudgeNest("body","meta")　は true を返します。
		/// もし、false を返すとそれ以降の要素が全て html 要素内に直接記述されてしまうことになる為です。
		/// </para>
		/// </remarks>
		private bool JudgeNest(string parent,string child){
			parent=parent.ToLower();
			child=child.ToLower();
			Gen::Dictionary<string,bool> list;
			if(!judgeNest_table.TryGetValue(parent,out list))return true;

			bool ret;
			if(list.TryGetValue(child,out ret))return ret;
			ret=list[JUDGENEST_LISTKEY_DEFAULT];
			list[child]=ret;
			return ret;
		}
		private const string JUDGENEST_LISTKEY_DEFAULT="<default>";
		static string[] TAGS_BLOCKELEM={
			"p","div","h1","h2","h3","h4","h5","h6",
			"dl","ol","ul","table","address","blockquote","center",
			"pre","xmp","listing","plaintext"
		};
		private static Gen::Dictionary<string,Gen::Dictionary<string,bool>> judgeNest_table
			=new Gen::Dictionary<string,Gen::Dictionary<string,bool>>();
		private static void initialize_judgeNest(){
			Gen::Dictionary<string,bool> listNoChild=new Gen::Dictionary<string,bool>();
			listNoChild.Add(JUDGENEST_LISTKEY_DEFAULT,false);
			foreach(string tagName in readElement_closedElem)
				judgeNest_table.Add(tagName,listNoChild);

			Gen::Dictionary<string,bool> listNoBlock=new Gen::Dictionary<string,bool>();
			listNoBlock.Add(JUDGENEST_LISTKEY_DEFAULT,true);
			foreach(string childTag in TAGS_BLOCKELEM)
				listNoBlock.Add(childTag,false);
			judgeNest_table.Add("p",listNoBlock);
			judgeNest_table.Add("h1",listNoBlock);
			judgeNest_table.Add("h2",listNoBlock);
			judgeNest_table.Add("h3",listNoBlock);
			judgeNest_table.Add("h4",listNoBlock);
			judgeNest_table.Add("h5",listNoBlock);
			judgeNest_table.Add("h6",listNoBlock);
			judgeNest_table.Add("address",listNoBlock);

			{
				Gen::Dictionary<string,bool> listLi=new Gen::Dictionary<string,bool>();
				listLi.Add(JUDGENEST_LISTKEY_DEFAULT,true);
				listLi.Add("li",false);
				judgeNest_table.Add("li",listLi);

				Gen::Dictionary<string,bool> listDt=new Gen::Dictionary<string,bool>(listNoBlock);
				listDt.Add("dt",false);
				listDt.Add("dd",false);
				judgeNest_table.Add("dt",listDt);

				Gen::Dictionary<string,bool> listDd=new Gen::Dictionary<string,bool>();
				listDd.Add(JUDGENEST_LISTKEY_DEFAULT,true);
				listDd.Add("dt",false);
				listDd.Add("dd",false);
				judgeNest_table.Add("dd",listDd);
			}

			{
				Gen::Dictionary<string,bool> listTablePart=new Gen::Dictionary<string,bool>();
				listTablePart.Add(JUDGENEST_LISTKEY_DEFAULT,true);
				listTablePart.Add("thead",false);
				listTablePart.Add("tbody",false);
				listTablePart.Add("tfoot",false);
				judgeNest_table.Add("thead",listTablePart);
				judgeNest_table.Add("tbody",listTablePart);
				judgeNest_table.Add("tfoot",listTablePart);

				Gen::Dictionary<string,bool> listTableRow=new Gen::Dictionary<string,bool>(listTablePart);
				listTableRow.Add("tr",false);
				judgeNest_table.Add("tr",listTableRow);
				judgeNest_table.Add("caption",listTableRow);

				Gen::Dictionary<string,bool> listTableCell=new Gen::Dictionary<string,bool>(listTableRow);
				listTableCell.Add("th",false);
				listTableCell.Add("td",false);
				judgeNest_table.Add("th",listTableCell);
				judgeNest_table.Add("td",listTableCell);
			}

			{
				Gen::Dictionary<string,bool> list=new Gen::Dictionary<string,bool>(listNoBlock);
				list.Add("rb",false);
				list.Add("rt",false);
				list.Add("rp",false);
				judgeNest_table.Add("rb",list);
				judgeNest_table.Add("rt",list);
				judgeNest_table.Add("rp",list);
			}

		}
#if OLD
		// 滅茶苦茶遅い
		private bool JudgeNest(string parent,string child){
			parent=parent.ToLower();
			child=child.ToLower();
			if(judgeNest_denyTable.ContainsKey(parent))
				return !judgeNest_denyTable[parent].Contains(child.ToLower());
			if(judgeNest_denyList.Contains(parent))
				return false;
			return true;
		}
		private static Gen::List<string> judgeNest_denyList
			=new Gen::List<string>();
		private static Gen::Dictionary<string,Gen::List<string>> judgeNest_denyTable
			=new Gen::Dictionary<string,Gen::List<string>>();
		private static void initialize_judgeNest(){
			//----------------------------------------------------------------------
			// 親要素になる事の出来ない要素のリストを作成します。
			//----------------------------------------------------------------------
			judgeNest_denyList.Add("meta");
			judgeNest_denyList.Add("link");
			judgeNest_denyList.Add("br");
			judgeNest_denyList.Add("hr");
			judgeNest_denyList.Add("img");
			judgeNest_denyList.Add("col");
			judgeNest_denyList.Add("embed");
			judgeNest_denyList.Add("param");
			judgeNest_denyList.Add("area");
			judgeNest_denyList.Add("keygen");
			judgeNest_denyList.Add("source");
			judgeNest_denyList.Add("base");

			//----------------------------------------------------------------------
			// 子要素として持つ事が出来ない要素のリストを作成します。
			//----------------------------------------------------------------------
			// block element
			System.Collections.Generic.List<string> blockElements=new System.Collections.Generic.List<string>(new string[]{
				"p","div","h1","h2","h3","h4","h5","h6",
				"dl","ol","ul","table","address","blockquote","center",
				"pre","xmp","listing","plaintext"
			});
			judgeNest_denyTable.Add("p",blockElements);
			judgeNest_denyTable.Add("h1",blockElements);
			judgeNest_denyTable.Add("h2",blockElements);
			judgeNest_denyTable.Add("h3",blockElements);
			judgeNest_denyTable.Add("h4",blockElements);
			judgeNest_denyTable.Add("h5",blockElements);
			judgeNest_denyTable.Add("h6",blockElements);
			judgeNest_denyTable.Add("address",blockElements);

			judgeNest_denyTable.Add("li",new Gen::List<string>(new string[]{"li"}));

			// dictionary element
			Gen::List<string> list=new Gen::List<string>{"dt","dd"};
			list.AddRange(blockElements);
			judgeNest_denyTable.Add("dt",list);
			judgeNest_denyTable.Add("dd",list);

			// table element
			list=new System.Collections.Generic.List<string>(new string[]{"thead","tbody","tfoot"});
			judgeNest_denyTable.Add("thead",list);
			judgeNest_denyTable.Add("tbody",list);
			judgeNest_denyTable.Add("tfoot",list);
			list=new System.Collections.Generic.List<string>(new string[]{"tr","thead","tbody","tfoot"});
			judgeNest_denyTable.Add("tr",list);
			judgeNest_denyTable.Add("caption",list);
			list=new System.Collections.Generic.List<string>(new string[]{"td","th","tr","thead","tbody","tfoot"});
			judgeNest_denyTable.Add("td",list);
			judgeNest_denyTable.Add("th",list);

			// ruby element
			list=new System.Collections.Generic.List<string>(new string[]{"rb","rt","rp"});
			judgeNest_denyTable.Add("rb",list);
			judgeNest_denyTable.Add("rt",list);
			judgeNest_denyTable.Add("rp",list);
		}
#endif
		#endregion

		//-------------------------------------------------------------------------
		//		ReadEndElement
		//-------------------------------------------------------------------------
		private void ReadEndElement(){
			this.wreader.context=HTMLWordReader.Context.NMTOKEN;
			if(!this.wreader.ReadNext()){
				this.wreader.LinearReader.SetError("要素名がありません。< に続けて要素名を指定して下さい",0,null);
				this.wreader.context=HTMLWordReader.Context.global;
				return;
			}
			// 開始タグ探索
			HTMLElement elem=this.celem;
			while(elem!=null&&!HTMLElement.IsEqualTagName(elem.tagName,this.wreader.CurrentWord)){
				elem=elem.parentNode;
			}
			if(elem==null){
				this.wreader.LinearReader.SetError("この名前に対応する開始タグがありません。",0,null);
				elem=new HTMLElement(this.celem,"/"+this.wreader.CurrentWord);
				this.celem.appendChild(elem);
			}else{
				this.celem=elem.parentNode;
			}

			// 属性
			this.wreader.context=HTMLWordReader.Context.attribute;
			while(this.wreader.ReadNext()){
				switch(this.wreader.CurrentType.value){
					case HTMLWordReader.vAttribute:
						string[] x=this.wreader.CurrentWord.Split(new char[]{'='},2);
						elem.setAttribute(x[0],HTMLDocument.ResolveEntityReference(x[1]));
						break;
					case WordType.vOperator:
						//this.wreader.CurrentWord= ">"|"?>"|"/>"
						if(this.wreader.CurrentWord=="?>"||this.wreader.CurrentWord=="/>"){
							this.wreader.LinearReader.SetError("終了タグは > で終わるようにして下さい。",0,null);
						}
						this.wreader.context=HTMLWordReader.Context.global;
						return;
					default:
						// 無効な文字
						break;
				}
			}
		}

		//-------------------------------------------------------------------------
		//		ReadProcessInstruction
		//-------------------------------------------------------------------------
		private void ReadProcessInstruction(){
			this.wreader.context=HTMLWordReader.Context.NMTOKEN;
			if(!this.wreader.ReadNext()){
				this.wreader.LinearReader.SetError("要素名がありません。<? に続けて要素名を指定して下さい",0,null);
				this.wreader.context=HTMLWordReader.Context.global;
				return;
			}
			string name=this.wreader.CurrentWord.ToLower();
			HTMLElement elem=name=="xml"
				?(HTMLElement)new XmlDeclaration(this.celem)
				:(HTMLElement)new ProcInstruction(this.celem,name);
			this.celem.appendChild(elem);

			// 属性
			this.wreader.context=HTMLWordReader.Context.attribute;
			while(this.wreader.ReadNext()){
				switch(this.wreader.CurrentType.value){
					case HTMLWordReader.vAttribute:
						string[] x=this.wreader.CurrentWord.Split(new char[]{'='},2);
						elem.setAttribute(x[0],HTMLDocument.ResolveEntityReference(x[1]));
						break;
					case WordType.vOperator:
						//this.wreader.CurrentWord= ">"|"?>"|"/>"
						if((this.wreader.CurrentWord==">"||this.wreader.CurrentWord=="/>")&&elem.tagName!="?import"){
							this.wreader.LinearReader.SetError("xml 宣言及び ?import を除くプロセス命令は ?> で終わる様にして下さい。",0,null);
						}
						this.wreader.context=HTMLWordReader.Context.global;
						return;
					default:
						// 無効な文字
						break;
				}
			}
		}
	}
}