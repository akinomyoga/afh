namespace Tester.Javascript{
	[TestFunctionAttribute(
			"afh.Javascript: Number"
			,@"Number Ç∆ Number ÇÃâ¡éZÇÃé¿å±ÇçsÇ¢Ç‹Ç∑ÅB"
	)]
	public class NumberTest:TestFunction{
		public NumberTest() { }
		public override string Exec(){
			return new JSExecutor().ExecuteTest1().ToString();
#if bytecode
			var num1=new Number(0.555);
			var num2=new Number(103.6767);
			var num3=num1+num2;
			return num3.toString();
			------
			[var num1=]Å©(new Number)Å©0.555
			[var num2=]Å©(new Number)Å©103.6767
			[var num3=]Å©(num1+)Å©num2
			[return]Å©(num3.toString)Å©
			------
			push	0.555				// 0.555
			invoke	[Number] 1			// Number(0.555)
			store	[num1]				// Number(0.555)
			pop							//
			push	103.6767			// 103.6767
			invoke	[Number]			// Number(103.6767)
			store	[num2]				// Number(103.6767)
			pop							//
			load	[num2]				// Number(103.6767)
			invoke	[num1,:+:] 1		// Number(104.2317)
			store	[num3]				// Number(104.2317)
			pop							//
			invoke	[num3,toString] 0	// String("104.2317")
			ret
#endif
		}
		public class JSExecutor{
			System.Collections.Generic.Stack<afh.Javascript.Object> stack
				=new System.Collections.Generic.Stack<afh.Javascript.Object>();
			afh.Javascript.Object _context;
			public JSExecutor(){
				this._context=afh.Javascript.Global._global;
			}
			public afh.Javascript.Object ExecuteTest1(){
				this.Push(0.555);
				this.Invoke(1,"Number");
				this.Store("num1");
				this.Pop();
				this.Push(103.6767);
				this.Invoke(1,"Number");
				this.Store("num2");
				this.Pop();
				this.Load("num2");
				this.Invoke(1,"num1",":+:");
				this.Store("num3");
				this.Pop();
				this.Invoke(0,"num3","toString");
				return this.Pop();
				/*/
				this.Push(0.122);
				this.Push(3.1231);
				this.Push(100);
				this.Push(this);
				this.Invoke(3,"Array");
				return this.Pop();
				//*/
			}
			protected void Push(object value){
				this.stack.Push(afh.Javascript.Global.ConvertFromManaged(value));
			}
			protected afh.Javascript.Object Pop(){return this.stack.Pop();}
			protected void Store(params string[] member){
				this._context.SetValue(stack.Peek(),0,member);
			}
			protected void Load(params string[] member){
				this.stack.Push(this._context.GetValue(0,member));
			}
			protected void Invoke(int paramNum,params string[] function){
				afh.Javascript.Array array=new afh.Javascript.Array();
				for(int i=0;i<paramNum;i++)array[paramNum-i-1]=stack.Pop();
				stack.Push(this._context.InvokeMember(0,function,array));
			}
		}
	}

}