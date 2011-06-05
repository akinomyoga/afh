//#define STACK
#define DICT
using Gen=System.Collections.Generic;
namespace afh.HTML.Selector{
	public class Selector{
#if debug1
		// href.m.html1/
		// href=m=html1/
		// href:m:html1/ →× xmlns:href の m 属性と同じになる。同様に _ や - も使えない
		// href+m+html1/
		// href$m$html1/ href#m#html1/ href%m%html1/
		// 見にくい。疎らな文字の方がよい
		// href^m^html1/ href~m~html1/ href'm'html1/
		// href<m>html1/ href(m)html1/ href{m}html1/
		// href.m=html1/
		public static void readSelectorTest(){
			SelectorParser p=new SelectorParser("td[@href.l.s=='hello'.l&@pp==pp]+td/+a[hello=99]");
			System.Console.WriteLine(p.Parse());
			return;
		}
		public static void readRuleTest2(){
			SelectorParser p=new SelectorParser("[@href.l.s=='hello'.l,@pp==pp]@hello4");
			p.WR.ReadNext();
			foreach(Selector.Rule rule in p.readAttrs()){
				System.Console.WriteLine(string.Format("{0} ({1},{2},{3},{4},{5})"
					,rule,rule.varName,rule.varModifier,rule.ope,rule.value,rule.valModifier));
			}
		}
		public static void readRuleTest(){
			SelectorParser p=new SelectorParser("@href.l.s=='hello'.l,@pp==pp");
			p.AR.ReadNext();
			p.readRule();
			System.Console.WriteLine(string.Format("{0} ({1},{2},{3},{4},{5})"
				,p.rule,p.rule.varName,p.rule.varModifier,p.rule.ope,p.rule.value,p.rule.valModifier));
			p.AR.ReadNext();
			p.readRule();
			System.Console.WriteLine(string.Format("{0} ({1},{2},{3},{4},{5})"
				,p.rule,p.rule.varName,p.rule.varModifier,p.rule.ope,p.rule.value,p.rule.valModifier));
		}

#endif
		public readonly SimpleSelector[] simples;
		private readonly SimpleEnum[] simpleEnums;
		protected Selector(){
			this.simples=null;
			this.simpleEnums=null;
		}
		public Selector(SimpleSelector[] simpleSelectors){
			this.simples=simpleSelectors;
			this.simpleEnums=new SimpleEnum[simpleSelectors.Length];
			for(int i=0;i<simpleSelectors.Length;i++){
				this.simpleEnums[i]=new SimpleEnum(this.simples[i]);
			}
		}

		public static Selector Parse(string text){
			try{
				return new Parser(text).Parse();
			}catch(System.Exception e){
				__dll__.log.WriteError(e,"Selector を Parse 中に例外が発生しました");
				__dll__.log.AddIndent();
				__dll__.log.WriteVar("text",text);
				__dll__.log.RemoveIndent();
				throw;
			}
		}
		public override string ToString(){
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			foreach(SimpleSelector simple in this.simples)sb.Append(simple);
			return sb.ToString().Trim();
		}
		public static Selector operator |(Selector a,Selector b){
			return new MergedSelector(a,b);
		}

		#region methods:反復子
		//===========================================================
		//		初期反復子
		//===========================================================
		public HTMLElement GetFirstElement(HTMLElement parent){
			foreach(HTMLElement elem in this.Enumerate(parent))return elem;
			return null;
		}
		public Gen::Stack<HTMLElement> GetFirstSelectRoute(HTMLElement parent){
			foreach(Gen::Stack<HTMLElement> stc in this.EnumerateSelectRoute(parent))return stc;
			return null;
		}
#if DICT
		public Gen::Dictionary<string,string> GetFirstDictionary(HTMLElement parent){
			foreach(Gen::Dictionary<string,string> dic in this.EnumDictionary(parent))return dic;
			return null;
		}
#endif
#if STACK
		public Gen::Stack<string> GetFirstStack(HTMLElement parent){
			foreach(Gen::Stack<string> stc in this.EnumerateStack(parent))return stc;
			return null;
		}
#endif
		//===========================================================
		//		反復子
		//===========================================================
		public virtual System.Collections.Generic.IEnumerable<HTMLElement> Enumerate(HTMLElement parent){
			int m=simpleEnums.Length;
			if(m==0)yield break;
			System.Collections.IEnumerator[] @enum=new System.Collections.IEnumerator[m];
			HTMLElement e;
			bool move;int i=0,imax=m-1;
			@enum[0]=this.simpleEnums[0].Enumerate(parent).GetEnumerator();
			while(i>=0){
				try{move=@enum[i].MoveNext();}catch(System.Exception exc){
					__dll__.log.WriteError(exc,"HTML 要素の列挙中にエラーが発生しました。(列挙に使用している Selector は "+this.ToString()+" )");
					yield break;
				}
				if(move){
					e=(HTMLElement)@enum[i].Current;
					if(i==imax){
						yield return e;
					}else @enum[++i]=simpleEnums[i].Enumerate(e).GetEnumerator();
				}else i--;
			}
		}
		public virtual Gen::IEnumerable<Gen::Stack<HTMLElement>> EnumerateSelectRoute(HTMLElement parent){
			int m=simpleEnums.Length;
			if(m==0)yield break;
			System.Collections.IEnumerator[] @enum=new System.Collections.IEnumerator[m];
	/*☆*/	Gen::Stack<HTMLElement> estack=new System.Collections.Generic.Stack<HTMLElement>();
	/*☆*/	estack.Push(parent);
			bool move;int i=0,imax=m-1;
			@enum[0]=this.simpleEnums[0].Enumerate(estack.Peek()).GetEnumerator();
			while(i>=0){
				try{move=@enum[i].MoveNext();}catch(System.Exception exc){
					__dll__.log.WriteError(exc,"HTML 要素の列挙中にエラーが発生しました。(列挙に使用している Selector は "+this.ToString()+" )");
					yield break;
				}
				if(move){
	/*☆*/			estack.Push((HTMLElement)@enum[i].Current);
					if(i==imax){
	/*☆*/				yield return (Gen::Stack<HTMLElement>)((System.ICloneable)estack).Clone();;
	/*☆*/				estack.Pop();
					}else @enum[++i]=simpleEnums[i].Enumerate(estack.Peek()).GetEnumerator();
				}else{
					i--;
	/*☆*/			estack.Pop();
				}
			}
		}
#if DICT
		public virtual System.Collections.Generic.IEnumerable<Gen::Dictionary<string,string>> EnumDictionary(HTMLElement parent){
			int m=simpleEnums.Length;
			if(m==0)yield break;
			System.Collections.IEnumerator[] @enum=new System.Collections.IEnumerator[m];
	/*☆*/	Gen::Dictionary<string,string> dic=new Gen::Dictionary<string,string>();
			HTMLElement e;
			bool move;int i=0,imax=m-1;
	/*☆*/	@enum[0]=this.simpleEnums[0].Enumerate(parent,dic).GetEnumerator();
			while(i>=0){
				try{move=@enum[i].MoveNext();}catch(System.Exception exc){
					__dll__.log.WriteError(exc,"HTML 要素の列挙中にエラーが発生しました。(列挙に使用している Selector は "+this.ToString()+" )");
					yield break;
				}
				if(move){
					e=(HTMLElement)@enum[i].Current;
					if(i==imax){
						yield return dic;
	/*☆*/			}else @enum[++i]=simpleEnums[i].Enumerate(e,dic).GetEnumerator();
				}else i--;
			}
		}
#endif
#if STACK
		public System.Collections.Generic.IEnumerable<Gen::Stack<string>> EnumerateStack(HTMLElement parent){
			int m=simpleEnums.Length;
			if(m==0)yield break;
			System.Collections.IEnumerator[] @enum=new System.Collections.IEnumerator[m];
	/*☆*/	Gen::Stack<string> stc=new System.Collections.Generic.Stack<string>();
			HTMLElement e;
			bool move;int i=0,imax=m-1;
	/*☆*/	@enum[0]=this.simpleEnums[0].Enumerate(parent,stc).GetEnumerator();
			while(i>=0){
				try{move=@enum[i].MoveNext();}catch(System.Exception exc){
					__dll__.log.WriteError(exc,"HTML 要素の列挙中にエラーが発生しました。(列挙に使用している Selector は "+this.ToString()+" )");
					yield break;
				}
				if(move){
					e=(HTMLElement)@enum[i].Current;
					if(i==imax){
						yield return stc;
	/*☆*/			}else @enum[++i]=simpleEnums[i].Enumerate(e,stc).GetEnumerator();
				}else i--;
			}
		}
#endif
		#endregion

	}
	internal sealed class MergedSelector:Selector{
		private Selector a;
		private Selector b;
		public MergedSelector(Selector a,Selector b){
			this.a=a;
			this.b=b;
		}
		public override Gen::IEnumerable<Gen::Dictionary<string,string>> EnumDictionary(HTMLElement parent){
			foreach(Gen::Dictionary<string,string> d in a.EnumDictionary(parent))yield return d;
			foreach(Gen::Dictionary<string,string> d in b.EnumDictionary(parent))yield return d;
		}
		public override Gen::IEnumerable<HTMLElement> Enumerate(HTMLElement parent) {
			foreach(HTMLElement d in a.Enumerate(parent))yield return d;
			foreach(HTMLElement d in b.Enumerate(parent))yield return d;
		}
		public override Gen::IEnumerable<Gen::Stack<HTMLElement>> EnumerateSelectRoute(HTMLElement parent) {
			foreach(Gen::Stack<HTMLElement> d in a.EnumerateSelectRoute(parent)) yield return d;
			foreach(Gen::Stack<HTMLElement> d in b.EnumerateSelectRoute(parent)) yield return d;
		}
		public override string ToString() {
			return a.ToString()+" 及び "+b.ToString();
		}
	}


	#region struct SimpleSelector
	public struct SimpleSelector{
		/// <summary>
		/// 前の simpleselector との関係を保持します。
		/// ' ' : 子孫; '/' : 子;
		/// '-' : 直前; '+' : 直後;
		/// '&gt;' : 以前; '&lt;' : 以後;
		/// </summary>
		public char relation;
		/// <summary>
		/// 要素名を保持します。
		/// </summary>
		public string name;
		/// <summary>
		/// 各種の条件を保持します。
		/// </summary>
		public Rule[] rules;
		public SimpleSelector(char relation,string name){
			this.relation=relation;
			this.name=name;
			this.rules=null;
		}
		public override string ToString() {
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			if(this.relation=='-')sb.Append(' ');
			sb.Append(this.relation);
			sb.Append(this.name);
			if(this.rules!=null&&this.rules.Length>0){
				sb.Append("[");
				sb.Append(this.rules[0].ToString());
				for(int i=1;i<this.rules.Length;i++){
					sb.Append('&');
					sb.Append(this.rules[i].ToString());
				}
				sb.Append("]");
			}
			return sb.ToString();
		}
	}
	#endregion

	#region struct Rule
	public struct Rule{
		/// <summary>
		/// 変数を参照する名前を指定します。
		/// </summary>
		public string varName;
		/// <summary>
		/// 変数値に対する註釈を保持します。
		/// "l" 小文字に変換してから比較;
		/// "u" 大文字に変換してから比較;
		/// "t" ("tb"?) 両端の空白文字を削除;
		/// "ts" 初めの空白文字を削除;
		/// "te" 末端の空白文字を削除;
		/// "sub[n]" 部分文字列 n 文字目以降;
		/// "sub[n,m]" 部分文字列 n 文字目以降 m 文字;
		/// "sub[n-m]" 部分文字列 n 文字目以降 m 文字目未満;
		/// (= 亦は ! の比較に際して){
		///		'm' 中間一致; 's' 前方一致; 'e' 後方一致;
		/// }
		/// </summary>
		public string varModifier;
		/// <summary>
		/// 比較に使用する演算を指定します。
		/// '=' 等値; '!' 不等; '~' 含有(space 区切り);
		/// '&lt;' 小也; '&gt;' 大也;
		/// '≦' 以下; '≧' 以上;
		/// </summary>
		public char ope;
		/// <summary>
		/// 比較対象値を保持します。
		/// </summary>
		public string value;
		/// <summary>
		/// 比較対象値に対する註釈を保持します。
		/// "l" 小文字に変換してから比較;
		/// "u" 大文字に変換してから比較;
		/// "tb" 両端の空白文字を削除;
		/// "ts" 初めの空白文字を削除;
		/// "te" 末端の空白文字を削除;
		/// TODO: 以下は未実装
		/// "sub[n]" 部分文字列 n 文字目以降;
		/// "sub[n,m]" 部分文字列 n 文字目以降 m 文字;
		/// "sub[n-m]" 部分文字列 n 文字目以降 m 文字目未満;
		/// </summary>
		public string valModifier;
		public Rule(string varName,string varModifier,char ope,string value,string valModifier){
			this.varName=varName;
			this.varModifier=varModifier;
			this.ope=ope;
			this.value=value;
			this.valModifier=valModifier;
		}
		/// <summary>
		/// 規則を確認する為の RuleTester インスタンスを作成します。
		/// </summary>
		/// <returns>作成したインスタンスを返します。</returns>
		internal RuleTester CreateRuleTester(){
			return RuleTester.CreateRuleTester(this);
		}
		public override string ToString(){
			string strOpe;
			switch(ope){
				case '~':strOpe="~=";break;
				case '=':strOpe="==";break;
				case '!':strOpe="!=";break;
				case '<':strOpe="<";break;
				case '>':strOpe=">";break;
				case '≦':strOpe="<=";break;
				case '≧':strOpe=">=";break;
#if DICT
				case '→':strOpe="→";break;
#endif
#if STACK
				case '↓':
					return this.varName+this.varModifier+'↓';
#endif
				case '籠':
					return "{"+this.varName+"}";
				case ' ':default:
					return this.varName+this.varModifier;
			}
			return this.varName+this.varModifier+strOpe+"\""+this.value+"\""+this.valModifier;
		}
	}
	#endregion

	#region class SimpleEnum
	internal class SimpleEnum{
		public readonly char relation;
		public readonly string name;
		public readonly RuleTester[] testers;
		private readonly int len;
		public SimpleEnum(SimpleSelector simple){
			this.relation=simple.relation;
			this.name=simple.name;
			System.Collections.Generic.List<RuleTester> testers
				=new System.Collections.Generic.List<RuleTester>();
			foreach(Rule rule in simple.rules){
				testers.Add(rule.CreateRuleTester());
			}
			this.testers=testers.ToArray();
			this.len=this.testers.Length;
		}
		public System.Collections.IEnumerable Enumerate(HTMLElement parent){
			System.Collections.IEnumerable @enum=this.getBaseEnum(parent);
			if(@enum==null)yield break;
			int i;
			bool first=this.relation=='+'||this.relation=='-';
			foreach(HTMLElement elem in @enum){
				for(i=0;i<this.len;i++)if(!this.testers[i].Test(elem))break;
				if(i==this.len){
					yield return elem;
					if(first)yield break;
				}
			}
		}
#if DICT
		public System.Collections.IEnumerable Enumerate(HTMLElement parent,Gen::Dictionary<string,string> dict){
			System.Collections.IEnumerable @enum=this.getBaseEnum(parent);
			if(@enum==null)yield break;
			int i;
			bool first=this.relation=='+'||this.relation=='-';
			int stcC=dict.Count;
			foreach(HTMLElement elem in @enum){
				for(i=0;i<this.len;i++){
					if(!this.testers[i].Test(elem,dict))break;
				}
				if(i==this.len){
					yield return elem;
					if(first)break;
				}
			}
		}
#endif
#if STACK
		public System.Collections.IEnumerable Enumerate(HTMLElement parent,Gen::Stack<string> stack){
			System.Collections.IEnumerable @enum=this.getBaseEnum(parent);
			if(@enum==null)yield break;
			int i;
			bool first=this.relation=='+'||this.relation=='-';
			int stcC=stack.Count;
			foreach(HTMLElement elem in @enum){
				for(i=0;i<this.len;i++){
					if(!this.testers[i].Test(elem,stack))break;
				}
				if(i==this.len){
					yield return elem;
					while(stack.Count>stcC)stack.Pop();
					if(first)break;
				}
			}
			// 元の状態に戻してから終わる (MoveNext = false)
			while(stack.Count>stcC)stack.Pop();
		}
#endif
		private System.Collections.IEnumerable getBaseEnum(HTMLElement parent){
			if(this.name=="*")switch(this.relation){
				case ' ':return parent.enumAllElements(false);
				case '/':return parent.enumAllElements(true);
				case '>':return parent.enumFollowingElements();
				case '<':return parent.enumPrecedingElements();
				case '+':return parent.enumFollowingElements();
				case '-':return parent.enumPrecedingElements();
				default:return null;
			}else switch(this.relation){
				case ' ':return parent.enumElementsByTagName(this.name,false);
				case '/':return parent.enumElementsByTagName(this.name,true);
				case '>':return parent.enumFollowingElementsByTagName(this.name);
				case '<':return parent.enumPrecedingElementsByTagName(this.name);
				case '+':return parent.enumFollowingElementsByTagName(this.name);
				case '-':return parent.enumPrecedingElementsByTagName(this.name);
				default:return null;
			}
		}
	}
	#endregion

	#region class RuleTester
	internal abstract class RuleTester{
		protected internal string varName;
		protected internal string[] varMods;
		protected internal string value;
		internal IVarReader varReader;
		protected RuleTester(){}
		protected RuleTester(string varname,string varmods,string value,string valmods){
			this.varName=varname;
			this.value=this.Modify(value,splitModifiers(valmods));
			this.varMods=splitModifiers(varmods);

			//-- varReader
			if(this.varName.Length>1&&this.varName[0]=='@'){
				this.varReader=new AttributeVarReader(this,this.varName);
			}else{
				this.varReader=new MiscellaneousVarReader(this.varName);
			}
		}
		/// <summary>
		/// 指定した要素がこのインスタンスに関連付けられた規則に適合するかどうかを判断します。
		/// 変数の値を取得し Test(string) を呼び出します。
		/// </summary>
		/// <param name="elem">適合するかどうかを確かめる要素を指定します。</param>
		/// <returns>適合すると判断した場合、亦は規則が無効であると判断された場合には true を返します。
		/// それ以外の場合には false を返します。
		/// <para>規則が無効である場合に true を返すのは、
		/// この RuleTester が特定の条件を満たす要素を取り出すというよりは、
		/// 特定の条件を持つ要素を排除するのに使用されると想定されている為です。
		/// 或いは「(規則が有効)⇒(要素規則を満たす)」の条件式を判断して居るとも言えます。</para>
		/// </returns>
		public virtual bool Test(HTMLElement elem){
			bool? result=this.varReader.Read(elem);
			if(result!=null)return (bool)result;
			return this.Test(this.Modify(this.varReader.Value,this.varMods));
		}
#if DICT
		/// <summary>
		/// 値読込命令を含む際に値をスタックに返す形式の判断メソッドです。
		/// 通常ではこのメソッドは Test(HTMLElement) をそのまま呼び出すだけです。
		/// </summary>
		/// <param name="elem">適合するかどうかを確かめる要素を指定します。</param>
		/// <param name="dict">値を設定する先の辞書を指定します。</param>
		/// <returns>適合した場合に true を返します。</returns>
		public virtual bool Test(HTMLElement elem,Gen::Dictionary<string,string> dict){
			return Test(elem);
		}
#endif
#if STACK
		/// <summary>
		/// 値読込命令を含む際に値をスタックに返す形式の判断メソッドです。
		/// 通常ではこのメソッドは Test(HTMLElement) をそのまま呼び出すだけです。
		/// </summary>
		/// <param name="elem">適合するかどうかを確かめる要素を指定します。</param>
		/// <param name="stack">値を返す先のスタックを指定します。</param>
		/// <returns>適合した場合に true を返します。</returns>
		public virtual bool Test(HTMLElement elem,Gen::Stack<string> stack){
			return Test(elem);
		}
#endif
		protected abstract bool Test(string var);
		public string Modify(string val,string[] mods){
			int m=mods.Length;
			for(int i=0;i<m;i++)switch(mods[i]){
				case "l":val=val.ToLower();break;
				case "u":val=val.ToUpper();break;
				case "t":val=val.Trim();break;
				case "ts":val=val.TrimStart();break;
				case "te":val=val.TrimEnd();break;
			}
			return val;
		}
		public string[] splitModifiers(string mod){
			Gen::List<string> list=new Gen::List<string>();
			string[] cands=mod.Split('.');
			string cand;
			for(int i=0;i<cands.Length;i++){
				cand=cands[i].Trim();
				if(cand!="")list.Add(cand);
			}
			return list.ToArray();
		}
		/// <summary>
		/// 規則を確認する為の RuleTester インスタンスを作成します。
		/// </summary>
		/// <returns>作成したインスタンスを返します。</returns>
		public static RuleTester CreateRuleTester(Rule rule){
			switch(rule.ope){
				case '=':
					return new EqualityTester(true,rule.varName,rule.varModifier,rule.value,rule.valModifier);
				case '!':
					return new EqualityTester(false,rule.varName,rule.varModifier,rule.value,rule.valModifier);
				case '~':
					return new ContainsTester(rule.varName,rule.varModifier,rule.value,rule.valModifier);
				case '<':case '>':case '≦':case '≧':
					return new NumberTester(rule.ope,rule.varName,rule.varModifier,rule.value,rule.valModifier);
#if DICT
				case '→':
					return new AddToDictionaryTester(rule.varName,rule.varModifier,rule.value,rule.valModifier);
#endif
#if STACK
				case '↓':
					return new PushStackTester(rule.varName,rule.varModifier,rule.value,rule.valModifier);
#endif
				case '籠':
					return new NestTester(rule.varName);
				case ' ':default:
					return new ExistTester(rule.varName,rule.varModifier,rule.value,rule.valModifier);
			}
		}
	}
	#endregion

	#region VarReader
	internal interface IVarReader{
		string Value{get;}
		bool? Read(HTMLElement elem);
	}
	internal sealed class AttributeVarReader:IVarReader{
		private string attrName;
		private string _val;
		private RuleTester parent;
		public string Value{get{return this._val;}}
		public AttributeVarReader(RuleTester parent,string varName){
			this.attrName=varName.Substring(1);
			this._val=null;
			this.parent=parent;
		}
		public bool? Read(HTMLElement elem){
			HTMLAttribute attr=elem.getAttributeNode(this.attrName);
			if(!attr.specified)return false;
			this._val=attr.ToString();
			return null;
		}
	}
	internal sealed class MiscellaneousVarReader:IVarReader{
		private MiscellaneousVarReaderType type;
		private string _val;
		public string Value{get{return this._val;}}
		public MiscellaneousVarReader(string varName){
			switch(varName){
				case "innerText":
				case "textContent":
					this.type=MiscellaneousVarReaderType.innerText;
					break;
				case "innerHTML":
					this.type=MiscellaneousVarReaderType.innerHTML;
					break;
				case "outerText":
					this.type=MiscellaneousVarReaderType.outerText;
					break;
				case "outerHTML":
					this.type=MiscellaneousVarReaderType.outerHTML;
					break;
				case "tagName":
				case "nodeName":
					this.type=MiscellaneousVarReaderType.tagName;
					break;
				case "":
					__dll__.log.WriteLine("空白の変数指定は意味を為しません。");
					this.type=MiscellaneousVarReaderType.None;
					break;
				default:
					__dll__.log.WriteLine("変数指定が無効です。{0} には対応していません。",varName);
					this.type=MiscellaneousVarReaderType.None;
					break;
			}
		}
		public bool? Read(HTMLElement elem){
			switch(this.type){
				case MiscellaneousVarReaderType.innerText:
					if(elem._childNodes.Count==0)return false;
					this._val=elem.innerText;
					break;
				case MiscellaneousVarReaderType.innerHTML:
					if(elem._childNodes.Count==0)return false;
					this._val=elem.innerHTML;
					break;
				case MiscellaneousVarReaderType.outerText:
					this._val=elem.outerText;
					break;
				case MiscellaneousVarReaderType.outerHTML:
					this._val=elem.outerHTML;
					break;
				case MiscellaneousVarReaderType.tagName:
					this._val=elem.tagName;
					break;
				default:
					return true;
			}
			return null;
		}
	}
	internal enum MiscellaneousVarReaderType{
		innerText,outerText,innerHTML,outerHTML,tagName,None
	}
	#endregion

	#region class:***Tester
	internal sealed class ExistTester:RuleTester{
		public ExistTester(string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){}
		protected override bool Test(string var){
			return true;
		}
	}
	internal sealed class EqualityTester:RuleTester{
		private EqualityType type;
		public EqualityTester(bool equal,string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){
			int m=varMods.Length;
			for(int i=0;i<m;i++)switch(varMods[i]){
				case "m":
					this.type=equal?EqualityType.Contains:EqualityType.NotContains;
					return;
				case "s":
					this.type=equal?EqualityType.StartsWith:EqualityType.NotStartsWith;
					return;
				case "e":
					this.type=equal?EqualityType.EndsWith:EqualityType.NotEndsWith;
					return;
			}
			this.type=equal?EqualityType.Equal:EqualityType.Inequal;
		}
		protected override bool Test(string var) {
			switch(this.type){
				case EqualityType.Equal:		return var==value;
				case EqualityType.Inequal:		return var!=value;
				case EqualityType.Contains:		return var.IndexOf(value)>=0;
				case EqualityType.NotContains:	return var.IndexOf(value)<0;
				case EqualityType.StartsWith:	return var.StartsWith(value);
				case EqualityType.NotStartsWith:return !var.StartsWith(value);
				case EqualityType.EndsWith:		return var.EndsWith(value);
				case EqualityType.NotEndsWith:	return !var.EndsWith(value);
				default:return true;
			}
		}
	}
	internal enum EqualityType{Equal,Inequal,Contains,NotContains,EndsWith,NotEndsWith,StartsWith,NotStartsWith}
	internal sealed class ContainsTester:RuleTester{
		public ContainsTester(string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){}
		protected override bool Test(string var){
			string[] vars=var.Split(' ');
			int len=vars.Length;
			for(int i=0;i<len;i++)if(value==vars[i])return true;
			return false;
		}
	}
	internal sealed class NumberTester:RuleTester{
		private double fVal;
		private NumberTestType type;
		public bool Valid{get{return this.type!=NumberTestType.None;}}
		public NumberTester(char ope,string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){
			switch(ope){
				case '<':this.type=NumberTestType.LitterThan;break;
				case '>':this.type=NumberTestType.GreaterThan;break;
				case '≦':this.type=NumberTestType.LitterOrEqual;break;
				case '≧':this.type=NumberTestType.GreaterOrEqual;break;
				default:this.type=NumberTestType.None;break;
			}
			try{
				this.fVal=double.Parse(base.value);
			}catch{
				this.fVal=0;this.type=NumberTestType.None;
			}
		}
		protected override bool Test(string var){
			if(this.type==NumberTestType.None)return true;
			double fVar;
			try{fVar=double.Parse(var);}catch{return false;}
			switch(this.type){
				case NumberTestType.LitterThan:return fVar<fVal;
				case NumberTestType.GreaterThan:return fVar>fVal;
				case NumberTestType.LitterOrEqual:return fVar<=fVal;
				case NumberTestType.GreaterOrEqual:return fVar>=fVal;
				default:return true;
			}
		}
	}
	internal enum NumberTestType{None,GreaterThan,LitterThan,GreaterOrEqual,LitterOrEqual}
	internal sealed class NestTester:RuleTester{
		private Selector selector;
		public NestTester(string varname):base(){
			this.selector=Selector.Parse(varname);
			if(this.selector.simples.Length==0)this.selector=null;
		}
		/// <summary>
		/// Selector で指定される要素を持つかどうかを確認します。
		/// </summary>
		/// <param name="elem">確認の対象となる要素を指定します。</param>
		/// <returns>
		/// この RuleTester の保持する Selector によって elem から要素を取り出せる場合に true を返します。
		/// 保持する Selector が無効の場合にも true を返します。
		/// それ以外の場合には false を返します。
		/// </returns>
		public override bool Test(HTMLElement elem){
			if(this.selector==null)return true;
			return selector.GetFirstElement(elem)!=null;
		}
#if DICT
		public override bool Test(HTMLElement elem,Gen::Dictionary<string,string> dict) {
			if(this.selector==null)return true;
			Gen::Dictionary<string,string> d=selector.GetFirstDictionary(elem);
			if(d==null)return false;
			foreach(Gen::KeyValuePair<string,string> p in d)dict[p.Key]=p.Value;
			return true;
		}
#endif
#if STACK
		public override bool Test(HTMLElement elem,System.Collections.Generic.Stack<string> stack) {
			if(this.selector==null)return true;
			Gen::Stack<string> s=selector.GetFirstStack(elem);
			if(s==null)return false;
			foreach(string x in s)stack.Push(x);
			return true;
		}
#endif
		protected override bool Test(string var){
			throw new System.Exception("このメソッドは実装されていません。");
		}
	}
#if DICT
	internal sealed class AddToDictionaryTester:RuleTester{
		private string var;
		public AddToDictionaryTester(string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){}
		public override bool Test(HTMLElement elem,Gen::Dictionary<string,string> dict){
			this.var=null;
			bool r=base.Test(elem);
			if(this.var!=null)dict[this.value]=this.var;
			return r;
		}
		protected override bool Test(string var){
			this.var=var;
			return true;
		}
	}
#endif
#if STACK
	internal sealed class PushStackTester:RuleTester{
		private string var;
		public PushStackTester(string varname,string varmods,string value,string valmods)
			:base(varname,varmods,value,valmods){}
		public override bool Test(HTMLElement elem,System.Collections.Generic.Stack<string> stack){
			this.var=null;
			bool r=base.Test(elem);
			if(this.var!=null)stack.Push(this.var);
			return r;
		}
		protected override bool Test(string var){
			this.var=var;
			return true;
		}
	}
#endif
	#endregion
}