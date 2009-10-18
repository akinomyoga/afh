using Gen=System.Collections.Generic;

namespace afh.Cobalt.Tree{
	public interface ITreeElement{}
	public interface IExpression:ITreeElement{
		string ToSource();
	}

	public class UnknownExpression:IExpression{
		public string ToSource(){return "";}
	}
	public class BinaryExpression:IExpression{
		public readonly string op;
		public readonly IExpression left;
		public readonly IExpression right;

		public BinaryExpression(string op,IExpression left,IExpression right){
			this.op=op;
			this.left=left;
			this.right=right;
		}

		public string ToSource(){
			return "("+left.ToSource()+op+right.ToSource()+")";
		}
	}

	public class UnaryExpression:IExpression{
		public readonly string op;
		public readonly IExpression expression;

		public UnaryExpression(string op,IExpression expression){
			this.op=op;
			this.expression=expression;
		}

		public string ToSource(){
			return "("+op+expression.ToSource()+")";
		}
	}

	public class IdExpression:IExpression{
		public readonly string word;

		public IdExpression(string word){
			this.word=word;
		}

		public string ToSource(){
			return this.word;
		}
	}

	public class LiteralExpression:IExpression{
		public readonly string word;

		public LiteralExpression(string word){
			this.word=word;
		}

		public string ToSource(){
			return this.word;
		}
	}

	public class FunctionCallExpression:IExpression{
		/// <summary>
		/// 呼出に使用した括弧を保持します。
		/// </summary>
		public readonly string paren;
		/// <summary>
		/// 適用先の関数を保持します。
		/// </summary>
		public readonly IExpression func;
		/// <summary>
		/// 適用する引数の配列を保持します。
		/// </summary>
		public readonly IExpression[] args;
		/// <summary>
		/// 引数区切に使用された記号を保持します。
		/// </summary>
		public readonly string[] delims;
		
		public FunctionCallExpression(string paren,IExpression func,IExpression[] args,string[] delims){
			this.paren=paren;
			this.func=func;
			this.args=args;
			this.delims=delims;
		}

		public string ToSource(){
			System.Text.StringBuilder buff=new System.Text.StringBuilder();
			buff.Append(this.func.ToSource());
			buff.Append(paren.Substring(0,1));
			for(int i=0;i<this.args.Length;i++){
				buff.Append(args[i]==null?"":args[i].ToSource());
				if(i<delims.Length)buff.Append(delims[i]);
			}
			buff.Append(paren.Substring(1));
			return buff.ToString();
		}
	}
}