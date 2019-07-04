namespace afh.JavaScript.Parse{
	public partial class JavascriptParser{
		public JavascriptParser(){
		}
		public void Read(string text){
			this.stack=new System.Collections.Stack();
			this.wreader=new WordReader(text);
			this.wreader.ReadNext();
			this.ReadContext_main();
			if(this.stack.Count>0)
				System.Console.WriteLine(this.stack.Pop().ToString());
		}
	}


	public interface IScriptNode{
	
	}

	/// <summary>
	/// �ŏ��̒P���\������߂ł��B
	/// </summary>
	public class Word:IScriptNode{
		public string word;
		public Word(string word){
			this.word=word;
		}
		public override string ToString() {
			return this.word;
		}
	}
	/// <summary>
	/// �񍀉��Z��\������߂ł��B
	/// </summary>
	public class BinaryOperator:IScriptNode{
		public string op;
		public IScriptNode left;
		public IScriptNode right;

		public BinaryOperator(string operatorName,IScriptNode rightparam,IScriptNode leftparam){
			this.op=operatorName;
			this.left=leftparam;
			this.right=rightparam;
		}
		public override string ToString() {
			return "("+
				this.left.ToString()+
				this.op+
				this.right.ToString()+
				")";
		}
	}
	/// <summary>
	/// �O�����Z�q��\������߂ł��B
	/// </summary>
	public class TripleOperator:IScriptNode{
		public IScriptNode condition;
		public IScriptNode whentrue;
		public IScriptNode whenfalse;

		/// <summary>
		/// �O�����Z�q��\������߂����������܂��B
		/// �������w�肷�鏇�Ԃɒ��ӂ��ĉ������B
		/// </summary>
		/// <param name="whenfalse"><paramref name="condition"/> �̕]���� false �������ꍇ�ɕ]�����鎮�w�肵�܂��B</param>
		/// <param name="whentrue"><paramref name="condition"/> �̕]���� true �������ꍇ�ɕ]�����鎮���w�肵�܂��B</param>
		/// <param name="condition">�����𔻒肷�鎮���w�肵�܂��B</param>
		public TripleOperator(IScriptNode whenfalse,IScriptNode whentrue,IScriptNode condition){
			this.condition=condition;
			this.whentrue=whentrue;
			this.whenfalse=whenfalse;
		}
		public override string ToString(){
			return "("+this.condition.ToString()+
				"?"+this.whentrue.ToString()+
				":"+this.whenfalse.ToString()+
				")";
		}
	}
	public class UnaryOperator:IScriptNode{
		public string ope;
		public IScriptNode target;
		public bool post;

		public UnaryOperator(string operatorName,IScriptNode targetparam,bool postfix){
			this.ope=operatorName;
			this.target=targetparam;
			this.post=postfix;
		}
		public override string ToString() {
			if(this.post){
				return "("+this.target.ToString()+this.ope+")";
			}else{
				return "("+this.ope+this.target.ToString()+")";
			}
		}
	}
	public class FunctionCall:IScriptNode{
		public IScriptNode obj;
		public IScriptNode[] args;

		public FunctionCall(IScriptNode[] arguments,IScriptNode obj){
			this.obj=obj;
			this.args=arguments;
		}
		public override string ToString() {
			System.Text.StringBuilder r=new System.Text.StringBuilder();
			r.Append(this.obj.ToString());
			r.Append("(");
			if(args.Length>0){
				r.Append(args[0].ToString());
				for(int i=1;i<args.Length;i++){
					r.Append(',');
					r.Append(args[i].ToString());
				}
			}
			r.Append(")");
			return r.ToString();
		}
	}
	public class MemberInvoke:IScriptNode{
		public IScriptNode obj;
		public IScriptNode[] args;

		public MemberInvoke(IScriptNode[] arguments,IScriptNode obj){
			this.obj=obj;
			this.args=arguments;
		}
		public override string ToString() {
			System.Text.StringBuilder r=new System.Text.StringBuilder();
			r.Append(this.obj.ToString());
			r.Append("[");
			if(args.Length>0){
				r.Append(args[0].ToString());
				for(int i=1;i<args.Length;i++){
					r.Append(',');
					r.Append(args[i].ToString());
				}
			}
			r.Append("]");
			return r.ToString();
		}
	}
}