//==============================================================================
//	PAREN_MATCH
//------------------------------------------------------------------------------
//	始まりの括弧と、終わりの括弧の種類が同じである事を要求する場合に指定します。
//	#define PAREN_MATCH
//==============================================================================

using Gen=System.Collections.Generic;
using prio_t=System.Single;

namespace afh.Cobalt.Parse{
	public enum WordType{
		Operator,
		Identifier,
		Literal,
	}
	public class Word{
		public WordType type;
		public string word;

		public Word(WordType type,string word){
			this.type=type;
			this.word=word;
		}
	}

	#region OperatorDefs
	//==========================================================================
	//	Operator の定義
	//==========================================================================
	/// <summary>
	/// 接頭辞演算子の定義です。
	/// </summary>
	internal class PrefixOperatorDef{
		/// <summary>
		/// 演算子を識別する為の名前を保持します。
		/// </summary>
		public readonly string word;
		/// <summary>
		/// 結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioR;
		/// <summary>
		/// PrefixOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioR">結合の優先順位を指定します。</param>
		public PrefixOperatorDef(string word,prio_t prioR){
			this.word=word;
			this.prioR=prioR;
		}

		public virtual bool TryParse(Word w,Parser p){
			p.Stack.Push(new UnaryPrefixElement(this));
			return true;
		}
	}
	/// <summary>
	/// 接尾辞演算子の定義です。
	/// </summary>
	internal class SuffixOperatorDef{
		/// <summary>
		/// 演算子を識別する為の名前を保持します。
		/// </summary>
		public readonly string word;
		/// <summary>
		/// 結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioL;
		/// <summary>
		/// この演算子が右結合性か否かを保持します。
		/// </summary>
		public readonly bool assocR;
		/// <summary>
		/// SuffixOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioL">結合の優先順位を指定します。</param>
		/// <param name="assocR">右結合性か否かを指定します。</param>
		public SuffixOperatorDef(string word,prio_t prioL,bool assocR){
			this.word=word;
			this.prioL=prioL;
			this.assocR=assocR;
		}
		/// <summary>
		/// SuffixOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioL">結合の優先順位を指定します。</param>
		public SuffixOperatorDef(string word,prio_t prioL):this(word,prioL,false){}

		public virtual bool TryParse(Word w,Parser p){
			IExpressionElement left=p.Stack.PopOperand(this.prioL,this.assocR);
			if(left==null)return false;

			p.Stack.Push(new UnaryExpressionElement(this.word,left));
			return true;
		}
	}
	/// <summary>
	/// 二項演算子の定義です。
	/// </summary>
	internal sealed class BinaryOperatorDef:SuffixOperatorDef{
		/// <summary>
		/// 右結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioR;
		/// <summary>
		/// BinaryOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioL">左結合の優先順位を保持します。</param>
		/// <param name="prioR">右結合の優先順位を保持します。</param>
		/// <param name="assocR">右結合性か否かを保持します。</param>
		public BinaryOperatorDef(string word,prio_t prioL,prio_t prioR,bool assocR)
			:base(word,prioL,assocR)
		{
			this.prioR=prioR;
		}

		public override bool TryParse(Word w,Parser p){
			IExpressionElement left=p.Stack.PopOperand(this.prioL,this.assocR);
			if(left==null)return false;

			p.Stack.Push(new BinaryPrefixElement(this,left));
			return true;
		}
	}
	//--------------------------------------------------------------------------
	//	括弧
	//--------------------------------------------------------------------------
	/// <summary>
	/// 括弧の始まりの定義です。
	/// </summary>
	internal sealed class PrefixParenDef:PrefixOperatorDef{
		/// <summary>
		/// 括弧の開始によって入る新しい Context を保持します。
		/// </summary>
		public readonly Context ctx;
		/// <summary>
		/// OpenParenDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">括弧の開始を識別する為の名前を指定します。</param>
		/// <param name="ctx">新しく開始する Context を指定します。</param>
		public PrefixParenDef(string word,Context ctx):base(word,-1){
			this.ctx=ctx;
		}

		public override bool TryParse(Word w,Parser p){
			p.Stack.Push(new StartParenMarker(this.word));
			p.PushContext(this.ctx);
			return true;
		}
	}
	internal sealed class ClosePrefixParenDef:SuffixOperatorDef{
		readonly string start_paren;
		public ClosePrefixParenDef(string word,string start_paren):base(word,-1){
			this.start_paren=start_paren;
		}
		public override bool TryParse(Word w,Parser p){
			if(!p.Stack.IsTopExpression){
				p.ReportError(w,"終わりの括弧に対応する中身がありません。");
				return true;
			}
			
			IExpressionElement content=p.Stack.PopOperand(-1,false);

			StartParenMarker m=p.Stack.Peek() as StartParenMarker;
			if(m==null){
				p.ReportError(w,"終わりの括弧に対応する始まりの括弧が存在しません。");
				return false;
			}

			p.Stack.Pop();
			p.PopContext();

#if PAREN_MATCH
			// ※ 区間などの場合には始まりと終わりが食い違っても OK
			if(m.StartParen!=this.start_paren){
				p.ReportError(w,"始まりの括弧と終わりの括弧が一致しません。");
				return true;
			}
#endif

			if(start_paren=="("&&w.word==")")
				p.Stack.Push(content);
			else
				p.Stack.Push(new UnaryExpressionElement(m.StartParen+w.word,content));

			return true;
		}
	}
	//--------------------------------------------------------------------------
	//	関数適用
	//--------------------------------------------------------------------------
	/// <summary>
	/// 関数適用括弧の始まりの定義です。
	/// </summary>
	internal sealed class SuffixParenDef:SuffixOperatorDef{
		/// <summary>
		/// 括弧の開始によって入る新しい Context を保持します。
		/// </summary>
		public readonly Context ctx;
		/// <summary>
		/// SuffixParenDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">括弧の開始を識別する為の名前を指定します。</param>
		/// <param name="prioL">結合の優先順位を指定します。</param>
		/// <param name="ctx">新しく開始する Context を指定します。</param>
		public SuffixParenDef(string word,prio_t prioL,Context ctx):base(word,prioL,false){
			this.ctx=ctx;
		}

		public override bool TryParse(Word w,Parser p){
			IExpressionElement left=p.Stack.PopOperand(this.prioL,false);
			if(left==null)return false;

			p.Stack.Push(new StartArgsMarker(word,left));
			p.PushContext(this.ctx);
			return true;
		}
	}
	internal sealed class ArgsDelimiterDef:SuffixOperatorDef{
		public ArgsDelimiterDef(string delim):base(delim,-1){}
		public override bool TryParse(Word w,Parser p){
			p.Stack.Push(new DelimArgsMarker(w.word));
			return true;
		}
	}
	internal sealed class CloseArgsParenDef:SuffixOperatorDef{
		readonly string start_paren;
		public CloseArgsParenDef(string word,string start_paren):base(word,-1){
			this.start_paren=start_paren;
		}

		class Arguments{
			int index=0;
			/// <summary>
			/// 引数を逆転順序で格納します。
			/// </summary>
			Gen::List<Tree.IExpression> args_rev=new Gen::List<Tree.IExpression>();
			/// <summary>
			/// 引数の区切り記号を逆転順序で格納します。
			/// </summary>
			Gen::List<string> delims=new Gen::List<string>();
			public Arguments(){
				// 一個要素を追加
				args_rev.Add(null);
			}

			//------------------------------------------------------------------
			//	読み取った物を格納
			//------------------------------------------------------------------
			public bool AddArgument(IExpressionElement elem){
				bool ret=args_rev[index]!=null;
				args_rev[index]=elem.Expression;
				return ret;
			}
			public void AddDelim(string d){
				delims.Add(d);
				args_rev.Add(null);
				index++;
			}
			//------------------------------------------------------------------
			//	結果の取得
			//------------------------------------------------------------------
			/// <summary>
			/// 引数を順番に格納した配列を取得します。
			/// </summary>
			/// <returns></returns>
			public Tree.IExpression[] GetArguments(){
				if(index==0&&args_rev[0]==null)
					return new Tree.IExpression[0];

				Tree.IExpression[] args=new Tree.IExpression[args_rev.Count];
				for(int iS=args_rev.Count-1,iD=0;iS>=0;iS--,iD++)
					args[iD]=args_rev[iS];

				return args;
			}

			/// <summary>
			/// 引数区切を順番に格納した配列を取得します。
			/// </summary>
			/// <returns></returns>
			public string[] GetDelims(){
				string[] ret=new string[delims.Count];
				for(int iS=delims.Count-1,iD=0;iS>=0;iS--,iD++)
					ret[iD]=delims[iS];
				return ret;
			}
		}
		public override bool TryParse(Word w,Parser p){
			Arguments arg=new Arguments();

			// 始まりの括弧が見つかる迄
			StartArgsMarker m=p.Stack.Peek() as StartArgsMarker;
			if(m==null)while(!p.Stack.IsEmpty){
				if(p.Stack.IsTopPrefix){
					// Prefix の適用先が見つからない
					return false;
				}

				if(p.Stack.IsTopExpression){
					if(!arg.AddArgument(p.Stack.PopOperand(-1,false))){
						// 引数が同じスロットに上書きされた時
						p.ReportError(w,"引数と引数の間に区切りが入っていない可能性があります。");
					}
					continue;
				}

				DelimArgsMarker d=p.Stack.Peek() as DelimArgsMarker;
				if(d!=null){
					p.Stack.Pop();
					arg.AddDelim(d.Word);
					continue;
				}
				
				m=p.Stack.Peek() as StartArgsMarker;
				if(m!=null)break;

				break;
			}

			if(m!=null){
				p.Stack.Pop();
				p.PopContext();

#if PAREN_MATCH
				if(m.StartParen!=this.start_paren){
					// ※ 区間などの場合には始まりと終わりが食い違っても OK
					p.ReportError(w,"始まりの括弧と終わりの括弧が一致しません。");
					return true;
				}
#endif

				p.Stack.Push(new ExpressionElement(
					new Tree.FunctionCallExpression(
						m.StartParen+w.word,
						m.FunctionExpression,
						arg.GetArguments(),
						arg.GetDelims()
					)
				));

				return true;
			}else{
				p.ReportError(w,"対応する始まりの括弧が見つかりません。");
				return true;
			}
		}
#if false
		public override bool TryParse(Word w,Parser p){
			Gen::List<Tree.IExpression> args_rev=new Gen::List<Tree.IExpression>();

			// 始まりの括弧が見つかる迄
			StartArgsMarker m=p.Stack.Peek() as StartArgsMarker;
			int c=-1;
			if(m==null){
				args_rev.Add(null);
				c++;
			}
			while(m==null&&!p.Stack.IsEmpty){
				if(p.Stack.IsTopExpression){
					args_rev[c]=p.Stack.PopOperand(-1,false).Expression;

					// 区切り?
					DelimArgsMarker d=p.Stack.Peek() as DelimArgsMarker;
					if(d!=null){
						args_rev.Add(null);c++;
						continue;
					}

					// 始まりの括弧?
					m=p.Stack.Peek() as StartArgsMarker;
					if(m!=null)continue;

					p.ReportError(w,"引数と引数の間に区切りが入っていない可能性があります。");
					continue;
				}
				
				if(p.Stack.IsTopPrefix){
					// Prefix の適用先が見つからない
					return false;
				}

				break;
			}

			if(m!=null){
				p.Stack.Pop();
				p.PopContext();

#if PAREN_MATCH
				if(m.StartParen!=this.start_paren){
					// ※ 区間などの場合には始まりと終わりが食い違っても OK
					p.ReportError(w,"始まりの括弧と終わりの括弧が一致しません。");
					return true;
				}
#endif

				p.Stack.Push(new ExpressionElement(
					new Tree.FunctionCallExpression(
						start_paren+w.word,
						m.FunctionExpression,
						GetArguments(args_rev)
					)
				));
				return true;
			}else{
				p.ReportError(w,"対応する始まりの括弧が存在しません。");
				return true;
			}
		}

		private static Tree.IExpression[] GetArguments(Gen::List<Tree.IExpression> args_rev){
			Tree.IExpression[] args=new Tree.IExpression[args_rev.Count];
			for(int iS=args_rev.Count-1,iD=0;iS>=0;iS--,iD++)
				args[iD]=args_rev[iS];

			return args;
		}
#endif
	}
	#endregion

	//==========================================================================
	//	文脈の定義
	//==========================================================================
	public class Context{
		public Context parent;
		public Context(){}

		//----------------------------------------------------------------------
		//	検索
		//----------------------------------------------------------------------
		Gen::Dictionary<string,SuffixOperatorDef> data_suf=new Gen::Dictionary<string,SuffixOperatorDef>();
		internal SuffixOperatorDef GetSuffixOperator(string op){
			SuffixOperatorDef def;
			if(this.data_suf.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetSuffixOperator(op);
			return null;
		}
		Gen::Dictionary<string,PrefixOperatorDef> data_pre=new Gen::Dictionary<string,PrefixOperatorDef>();
		internal PrefixOperatorDef GetPrefixOperator(string op){
			PrefixOperatorDef def;
			if(this.data_pre.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetPrefixOperator(op);
			return null;
		}

		//----------------------------------------------------------------------
		//	登録
		//----------------------------------------------------------------------
		public void AddBinaryOperatorDef(string op,prio_t priorityL,prio_t priorityR,bool assocR){
			data_suf.Add(op,new BinaryOperatorDef(op,priorityL,priorityR,assocR));
		}
		public void AddPrefixOperatorDef(string op,prio_t priority){
			data_pre.Add(op,new PrefixOperatorDef(op,priority));
		}
		public void AddSuffixOperatorDef(string op,prio_t priority){
			data_suf.Add(op,new SuffixOperatorDef(op,priority));
		}

		public void RegisterStartParen(string start,Context ctx){
			data_pre.Add(start,new PrefixParenDef(start,ctx));
		}
		public void RegisterStartCall(string start,prio_t prioL,Context ctx){
			data_suf.Add(start,new SuffixParenDef(start,prioL,ctx));
		}
		public void RegisterEndParen(string end,string start){
			data_suf.Add(end,new ClosePrefixParenDef(end,start));
		}
		public void RegisterEndCall(string end,string start){
			data_suf.Add(end,new CloseArgsParenDef(end,start));
		}
		public void RegisterDelimCall(string d){
			data_suf.Add(d,new ArgsDelimiterDef(d));
		}
#if PAREN_MATCH
		public void AddParen(string start,string end,Context ctx){
			data_pre.Add(start,new PrefixParenDef(start,ctx));
			data_suf.Add(end,new ClosePrefixParenDef(end,start));
		}
		public void AddCall(string start,string end,prio_t prioL,Context ctx){
			data_suf.Add(start,new SuffixParenDef(start,prioL,ctx));
			data_suf.Add(end,new CloseArgsParenDef(end,start));
		}
#endif
	}

	#region ParseStack
	//==========================================================================
	//	解析スタックの定義
	//==========================================================================
	internal class Stack{
		Gen::List<IStackElement> data=new Gen::List<IStackElement>();
		public Stack(){}

		public void Push(IStackElement elem){
			this.data.Add(elem);
		}
		public IStackElement Pop(){
			int i=this.data.Count-1;
			if(i<0)return null;

			IStackElement r=this.data[i];
			this.data.RemoveAt(i);
			return r;
		}
		public IStackElement Peek(){
			if(this.data.Count==0)return null;
			return this.data[this.data.Count-1];
		}
		/// <summary>
		/// スタックの内容が空か否かを取得します。
		/// </summary>
		public bool IsEmpty{
			get{return this.data.Count==0;}
		}
		/// <summary>
		/// スタックの一番上にある物が IExpressionElement か否かを取得します。
		/// </summary>
		public bool IsTopExpression{
			get{return this.Peek() is IExpressionElement;}
		}
		/// <summary>
		/// スタックの一番上にある物が IPrefixElement か否かを取得します。
		/// </summary>
		public bool IsTopPrefix{
			get{return this.Peek() is IPrefixElement;}
		}
		/// <summary>
		/// 指定した優先度を使用して左オペランドを取得します。
		/// </summary>
		/// <param name="prio">オペランド切り出しの結合優先度を指定します。
		/// 結合度が高い prefix はオペランドに含められます。
		/// 結合度が低い prefix はオペランドに含められず、オペランドはそれ以降に始まると解釈されます。
		/// </param>
		/// <param name="assocR">同じ優先順位の prefix があった場合の処理を指定します。
		/// true を指定すると右結合規則としてオペランドを読み取ります。
		/// false を指定すると左結合規則としてオペランドを読み取ります。
		/// </param>
		/// <returns>読み取った内容を指定します。</returns>
		public IExpressionElement PopOperand(prio_t prio,bool assocR){
			if(!this.IsTopExpression)return null;

			int i=this.data.Count-1;
			if(assocR){
				for(;i>0;i--){
					IPrefixElement elm=this.data[i-1] as IPrefixElement;
					if(elm==null)break;
					if(elm.Priority<=prio)break;
				}
			}else{
				for(;i>0;i--){
					IPrefixElement elm=this.data[i-1] as IPrefixElement;
					if(elm==null)break;
					if(elm.Priority<prio)break;
				}
			}
			return this.PopOperand(i);
		}
		/// <summary>
		/// 指定した index 以降を一つのオペランドとして読み取ります。
		/// </summary>
		/// <param name="index">オペランドとしての読み出しを開始する位置を指定します。</param>
		/// <returns>スタック要素達を一つのオペランドとして縮約した結果を返します。</returns>
		public IExpressionElement PopOperand(int index){
			IExpressionElement ret=this.data[this.data.Count-1] as IExpressionElement;
			if(ret==null)throw new System.Exception("ParseError");

			for(int i=this.data.Count-2;i>=index;i--){
				ret=((IPrefixElement)this.data[i]).Operate(ret);
			}

			this.data.RemoveRange(index,this.data.Count-index);
			return ret;
		}
	}
	/// <summary>
	/// スタック要素を表現するインタフェースです。
	/// </summary>
	public interface IStackElement{}

	public interface IMarkerElement:IStackElement{
		string Key{get;}
	}

	public interface IPrefixElement:IStackElement{
		prio_t Priority{get;}
		IExpressionElement Operate(IExpressionElement elem);
	}
	public interface IExpressionElement:IStackElement{
		Tree.IExpression Expression{get;}
	}

	public delegate IExpressionElement DPrefixOperation(IExpressionElement elm);
	public class PrefixElement:IPrefixElement{
		private readonly prio_t priority;
		private readonly DPrefixOperation operation;

		public prio_t Priority{
			get{return this.priority;}
		}
		public IExpressionElement Operate(IExpressionElement elem){
			return this.operation(elem);
		}

		public PrefixElement(prio_t priority,DPrefixOperation operation){
			this.priority=priority;
			this.operation=operation;
		}
	}

	public class ExpressionElement:IExpressionElement{
		private readonly Tree.IExpression expression;
		public Tree.IExpression Expression{
			get{return this.expression;}
		}

		public ExpressionElement(Tree.IExpression expression){
			this.expression=expression;
		}
	}

	internal class BinaryPrefixElement:IPrefixElement{
		private readonly prio_t priority;
		public prio_t Priority{
			get{return this.priority;}
		}

		private readonly string op;
		private readonly Tree.IExpression left;
		public IExpressionElement Operate(IExpressionElement right){
			return new ExpressionElement(
				new Tree.BinaryExpression(op,left,right.Expression)
				);
		}

		public BinaryPrefixElement(BinaryOperatorDef def,IExpressionElement left){
			this.priority=def.prioR;
			this.op=def.word;
			this.left=left.Expression;
		}
	}

	internal class UnaryPrefixElement:IPrefixElement{
		private readonly prio_t priority;
		public prio_t Priority{
			get{return this.priority;}
		}

		private readonly string op;
		public IExpressionElement Operate(IExpressionElement elem){
			return new UnaryExpressionElement(this.op,elem);
		}
		public UnaryPrefixElement(PrefixOperatorDef def){
			this.priority=def.prioR;
			this.op=def.word;
		}
	}

	internal class UnaryExpressionElement:ExpressionElement{
		public UnaryExpressionElement(string op,IExpressionElement elem)
			:base(new Tree.UnaryExpression(op,elem.Expression)){}
	}

	public class MarkerElement:IMarkerElement{
		string key;
		public string Key{get{return this.key;}}
	
		public MarkerElement(string word){
			this.key=word;
		}
	}
	//--------------------------------------------------------------------------
	//	括弧・関数呼出
	//--------------------------------------------------------------------------
	internal class StartParenMarker:IMarkerElement{
		string word;
		public string Key{get{return "P"+this.word;}}
		public string StartParen{get{return this.word;}}

		public StartParenMarker(string word){
			this.word=word;
		}
	}
	internal class StartArgsMarker:IMarkerElement{
		string word;
		public string Key{get{return "F"+this.word;}}
		public string StartParen{get{return this.word;}}

		IExpressionElement elem;
		public Tree.IExpression FunctionExpression{
			get{return elem.Expression;}
		}

		public StartArgsMarker(string word,IExpressionElement elem){
			this.word=word;
			this.elem=elem;
		}
	}
	internal class DelimArgsMarker:IMarkerElement{
		string word;
		public string Key{get{return "D"+this.word;}}
		public string Word{get{return this.word;}}

		public DelimArgsMarker(string word){
			this.word=word;
		}
	}
	#endregion

	//==========================================================================
	//	解析器
	//==========================================================================
	internal class Parser{
		public Parser(){}
		public void ReportError(Word w,string message){
			// ■
		}
		//----------------------------------------------------------------------
		//	解析スタック
		//----------------------------------------------------------------------
		Stack stk_parse;
		/// <summary>
		/// この Parser で使用されている解析用スタックを取得します。
		/// </summary>
		public Stack Stack{
			get{return this.stk_parse;}
		}
		//----------------------------------------------------------------------
		//	文脈
		//----------------------------------------------------------------------
		Context ctx;
		Gen::Stack<Context> stk_ctx;
		public void PushContext(Context ctx){
			this.ctx=ctx;
			this.stk_ctx.Push(ctx);
		}
		public void PopContext(){
			this.stk_ctx.Pop();
			this.ctx=this.stk_ctx.Peek();
		}
		//----------------------------------------------------------------------
		//	解析
		//----------------------------------------------------------------------
		public Tree.IExpression Parse(Context startContext,Gen::IEnumerable<Word> input){
			this.stk_parse=new Stack();
			this.stk_ctx=new Gen::Stack<Context>();
			this.PushContext(startContext);
			foreach(Word w in input){
				switch(w.type){
					case WordType.Operator:
						if(TrySuffixOperator(w))break;
						if(TryPrefixOperator(w))break;
						break;
					case WordType.Identifier:
						this.stk_parse.Push(
							new ExpressionElement(new Tree.IdExpression(w.word))
							);
						break;
					case WordType.Literal:
						this.stk_parse.Push(
							new ExpressionElement(new Tree.LiteralExpression(w.word))
							);
						break;
				}
			}

			if(!this.stk_parse.IsTopExpression)
				throw new System.Exception("ParseError");

			return this.stk_parse.PopOperand(0).Expression;
		}
		private bool TryPrefixOperator(Word w){
			PrefixOperatorDef def=this.ctx.GetPrefixOperator(w.word);
			if(def==null)return false;

			return def.TryParse(w,this);
		}
		private bool TrySuffixOperator(Word w){
			SuffixOperatorDef def=this.ctx.GetSuffixOperator(w.word);
			if(def==null)return false;

			return def.TryParse(w,this);
		}
	}

	public static class LanguageDefinition{
		private static readonly Context ctxExp;
		private static readonly Context ctxPParen;
		private static readonly Context ctxFParen;
		static LanguageDefinition(){
			ctxExp=new Context();
			ctxPParen=new Context();
			ctxFParen=new Context();
			//
			//	ctxExp の定義
			//
			ctxExp.AddBinaryOperatorDef("+",1,1,false);
			ctxExp.AddBinaryOperatorDef("-",1,1,false);
			ctxExp.AddBinaryOperatorDef("*",2,2,false);
			ctxExp.AddBinaryOperatorDef("/",2,2,false);
			ctxExp.AddBinaryOperatorDef("=",0,0,true);
			ctxExp.AddPrefixOperatorDef("+",2);
			ctxExp.AddPrefixOperatorDef("-",2);
			ctxExp.AddSuffixOperatorDef("!",2);
			ctxExp.AddSuffixOperatorDef("?",2);
			ctxExp.RegisterStartParen("(",ctxPParen);
			// ctxExp.AddOpenParenDef("[",ctxPBrace); // 配列型と混同するので×
			ctxExp.RegisterStartCall("(",3,ctxFParen);
			ctxExp.RegisterStartCall("[",3,ctxFParen);
			//
			//	ctxPParen の定義
			//
			ctxPParen.parent=ctxExp;
			ctxPParen.RegisterEndParen(")","(");
			//
			//	ctxFParen の定義
			//
			ctxFParen.parent=ctxExp;
			ctxFParen.RegisterEndCall(")","(");
			ctxFParen.RegisterEndCall("]","[");
			ctxFParen.RegisterDelimCall(",");
			ctxFParen.RegisterDelimCall(";");
			//ctxFParen.AddBinaryOperatorDef(",",-10);
			//ctxFParen.AddSuffixOperatorDef(")",-10);
		}

		public static Tree.IExpression Parse(string input){
			return new Parser().Parse(ctxExp,Scan1(input));
		}

		/// <summary>
		/// テスト用の Word 生成器です。
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private static Gen::IEnumerable<Word> Scan1(string input){
			for(int i=0;i<input.Length;i++){
				char c=input[i];
				WordType type=
					'a'<=c&&c<='z'||'A'<=c&&c<='Z'?WordType.Identifier:
					'0'<=c&&c<='9'?WordType.Literal:
					WordType.Operator;
				yield return new Word(type,c.ToString());
			}
		}
	}

}