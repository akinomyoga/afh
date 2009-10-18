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

	//==========================================================================
	//	Operator の定義
	//==========================================================================
	/// <summary>
	/// 二項演算子の定義です。
	/// </summary>
	internal class BinaryOperatorDef{
		/// <summary>
		/// 演算子を識別する為の名前を保持します。
		/// </summary>
		public readonly string word;
		/// <summary>
		/// 左結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioL;
		/// <summary>
		/// 右結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioR;
		/// <summary>
		/// この演算子が右結合性か否かを保持します。
		/// </summary>
		public readonly bool assocR;
		/// <summary>
		/// BinaryOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioL">左結合の優先順位を保持します。</param>
		/// <param name="prioR">右結合の優先順位を保持します。</param>
		/// <param name="assocR">右結合性か否かを保持します。</param>
		public BinaryOperatorDef(string word,prio_t prioL,prio_t prioR,bool assocR){
			this.word=word;
			this.prioR=prioR;
			this.prioL=prioL;
			this.assocR=assocR;
		}

		public bool TryParse(Word w,Stack s){
			IExpressionElement left=s.PopOperand(this.prioL,this.assocR);
			if(left==null)return false;

			s.Push(new BinaryPrefixElement(this,left));
			return true;
		}
	}
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

		public bool TryParse(Word w,Stack s){
			s.Push(new UnaryPrefixElement(this));
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
		/// SuffixOperatorDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">演算子を識別する為の名前を指定します。</param>
		/// <param name="prioL">結合の優先順位を指定します。</param>
		public SuffixOperatorDef(string word,prio_t prioL){
			this.word=word;
			this.prioL=prioL;
		}

		public bool TryParse(Word w,Stack s){
			IExpressionElement left=s.PopOperand(this.prioL,false);
			if(left==null)return false;

			s.Push(new UnaryExpressionElement(this.word,left));
			return true;
		}
	}
	/// <summary>
	/// 括弧の始まりの定義です。
	/// </summary>
	internal class OpenParenDef{
		/// <summary>
		/// 括弧の開始を識別する為の名前を保持します。
		/// </summary>
		public readonly string word;
		/// <summary>
		/// 括弧の開始によって入る新しい Context を保持します。
		/// </summary>
		public readonly Context ctx;
		/// <summary>
		/// OpenParenDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">括弧の開始を識別する為の名前を指定します。</param>
		/// <param name="ctx">新しく開始する Context を指定します。</param>
		public OpenParenDef(string word,Context ctx){
			this.word=word;
			this.ctx=ctx;
		}
	}
	/// <summary>
	/// 関数適用括弧の始まりの定義です。
	/// </summary>
	internal class FunctionParenDef{
		/// <summary>
		/// 括弧の開始を識別する為の名前を保持します。
		/// </summary>
		public string word;
		/// <summary>
		/// 結合の優先順位を保持します。値が大きい程結合が強くなります。
		/// </summary>
		public readonly prio_t prioL;
		/// <summary>
		/// 括弧の開始によって入る新しい Context を保持します。
		/// </summary>
		public Context ctx;
		/// <summary>
		/// FunctionParenDef のインスタンスを初期化します。
		/// </summary>
		/// <param name="word">括弧の開始を識別する為の名前を指定します。</param>
		/// <param name="prioL">結合の優先順位を指定します。</param>
		/// <param name="ctx">新しく開始する Context を指定します。</param>
		public FunctionParenDef(string word,prio_t prioL,Context ctx){
			this.word=word;
			this.prioL=prioL;
			this.ctx=ctx;
		}
	}
	//==========================================================================
	//	文脈の定義
	//==========================================================================
	public class Context{
		public Context parent;
		public Context(){}

		Gen::Dictionary<string,BinaryOperatorDef> data_binary=new System.Collections.Generic.Dictionary<string,BinaryOperatorDef>();
		public void AddBinaryOperatorDef(string op,prio_t priorityL,prio_t priorityR,bool assocR){
			data_binary.Add(op,new BinaryOperatorDef(op,priorityL,priorityR,assocR));
		}
		internal BinaryOperatorDef GetBinaryOperator(string op){
			BinaryOperatorDef def;
			if(this.data_binary.TryGetValue(op,out def)){
				return def;
			}
			if(this.parent!=null)
				return this.parent.GetBinaryOperator(op);
			return null;
		}

		Gen::Dictionary<string,PrefixOperatorDef> data_prefix=new System.Collections.Generic.Dictionary<string,PrefixOperatorDef>();
		public void AddPrefixOperatorDef(string op,prio_t priority){
			data_prefix.Add(op,new PrefixOperatorDef(op,priority));
		}
		internal PrefixOperatorDef GetPrefixOperator(string op){
			PrefixOperatorDef def;
			if(this.data_prefix.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetPrefixOperator(op);
			return null;
		}

		Gen::Dictionary<string,SuffixOperatorDef> data_suffix=new System.Collections.Generic.Dictionary<string,SuffixOperatorDef>();
		public void AddSuffixOperatorDef(string op,prio_t priority){
			data_suffix.Add(op,new SuffixOperatorDef(op,priority));
		}
		internal SuffixOperatorDef GetSuffixOperator(string op){
			SuffixOperatorDef def;
			if(this.data_suffix.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetSuffixOperator(op);
			return null;
		}

		Gen::Dictionary<string,OpenParenDef> data_openp=new System.Collections.Generic.Dictionary<string,OpenParenDef>();
		public void AddOpenParenDef(string op,Context ctx){
			data_openp.Add(op,new OpenParenDef(op,ctx));
		}
		internal OpenParenDef GetOpenParen(string op){
			OpenParenDef def;
			if(this.data_openp.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetOpenParen(op);
			return null;
		}

		Gen::Dictionary<string,FunctionParenDef> data_openf=new System.Collections.Generic.Dictionary<string,FunctionParenDef>();
		public void AddFunctionParenDef(string op,prio_t prioL,Context ctx){
			data_openf.Add(op,new FunctionParenDef(op,prioL,ctx));
		}
		internal FunctionParenDef GetFunctionParen(string op){
			FunctionParenDef def;
			if(this.data_openf.TryGetValue(op,out def))return def;

			if(this.parent!=null)return this.parent.GetFunctionParen(op);
			return null;
		}
	}
	//==========================================================================
	//	解析スタックの定義
	//==========================================================================
	internal class Stack{
		Gen::List<IStackElement> data=new System.Collections.Generic.List<IStackElement>();
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
		/// スタックの一番上にある物が IExpressionElement か否かを取得します。
		/// </summary>
		public bool IsTopExpression{
			get{return this.Peek() is IExpressionElement;}
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
	//==========================================================================
	//	解析器
	//==========================================================================
	internal class Parser{
		public Parser(){}
		//----------------------------------------------------------------------
		//	解析スタック
		//----------------------------------------------------------------------
		Stack stk_parse;
		Context ctx;
		//----------------------------------------------------------------------
		//	解析
		//----------------------------------------------------------------------
		public Tree.IExpression Parse(Context startContext,Gen::IEnumerable<Word> input){
			this.stk_parse=new Stack();
			this.ctx=startContext;
			foreach(Word w in input){
				switch(w.type){
					case WordType.Operator:
						if(TryBinaryOperator(w.word))break;
						if(TrySuffixOperator(w.word))break;
						if(TryPrefixOperator(w.word))break;
						if(TryPParenOperator(w.word))break;
						if(TrySParenOperator(w.word))break;
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

		private bool TryBinaryOperator(string word){
			BinaryOperatorDef def=this.ctx.GetBinaryOperator(word);
			if(def==null)return false;

			IExpressionElement left=this.stk_parse.PopOperand(def.prioL,def.assocR);
			if(left==null)return false;

			this.stk_parse.Push(new BinaryPrefixElement(def,left));
			return true;
		}
		private bool TryPrefixOperator(string word){
			PrefixOperatorDef def=this.ctx.GetPrefixOperator(word);
			if(def==null)return false;

			this.stk_parse.Push(new UnaryPrefixElement(def));
			return true;
		}
		private bool TrySuffixOperator(string word){
			SuffixOperatorDef def=this.ctx.GetSuffixOperator(word);
			if(def==null)return false;

			IExpressionElement left=this.stk_parse.PopOperand(def.prioL,false);
			if(left==null)return false;

			this.stk_parse.Push(new UnaryExpressionElement(def.word,left));
			return true;
		}
		private bool TryPParenOperator(string word){
			OpenParenDef def=this.ctx.GetOpenParen(word);
			if(def==null)return false;

			this.stk_parse.Push(new MarkerElement(word));
			
			// ■ push ctx
			return true;
		}
		private bool TrySParenOperator(string word){
			FunctionParenDef def=this.ctx.GetFunctionParen(word);
			if(def==null)return false;

			IExpressionElement left=this.stk_parse.PopOperand(def.prioL,false);
			if(left==null)return false;

			this.stk_parse.Push(new MarkerElement("F"+word));
			// ■ push ctx
			return true;
		}
	}

	public static class LanguageDefinition{
		private static readonly Context ctxExp;
		private static readonly Context ctxPParen;
		private static readonly Context ctxPBrace;
		private static readonly Context ctxFParen;
		private static readonly Context ctxFBrace;
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
			ctxExp.AddOpenParenDef("(",ctxPParen);
			// ctxExp.AddOpenParenDef("[",ctxPBrace); // 配列型と混同するので×
			ctxExp.AddFunctionParenDef("(",3,ctxPParen);
			ctxExp.AddFunctionParenDef("[",3,ctxPBrace);
			//
			//	ctxParen の定義
			//
			ctxPParen.parent=ctxExp;
			ctxPParen.AddSuffixOperatorDef(")",-10);
			//ctxFParen.parent=ctxExp;
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