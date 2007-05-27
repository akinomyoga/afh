namespace Tester.Javascript{
	[TestFunctionAttribute(
		"キャストのテスト"
		,@"キャストの動作についてテストします
-----
null is CastTest の結果
null as CastTest の結果
(CastTest)null で発生するエラーの種類"
	)]
	public class CastTest:TestFunction{
		public CastTest(){}
		public override string Exec(){
			string r="";
			CastTest Null=null;
			r+=(Null is CastTest).ToString()+"\n";
			r+=(null as CastTest)==null
				?"as 演算子によって null に変換されました"
				:"as 演算子によって null 以外の何かに変換されました";
			r+="\n";
			try{
				r+=((CastTest)null).ToString()+"\n";
			}catch(System.Exception e){
				r+=e.GetType().ToString()+"\n";
			}
			return r;
		}
	}

	[TestFunctionAttribute("throw new System.Exception() テスト","十回 throw した物を catch します。それにかかる時間を確認して下さい")]
	public class ThrowTest:TestFunction{
		public ThrowTest(){}
		public override string Exec(){
			for(int i=0;i<10;i++){
				try{this.ThrowError();}catch{}
			}
			return "完了";
		}
		public void ThrowError(){
			throw new System.Exception("これは自分で投げたエラーです");
		}
	}

	[TestFunctionAttribute("System.Type#GetMethods テスト","GetMethods で継承したメンバも取得する事が出来るかどうかを実験")]
	public class GetMethods:TestFunction{
		public GetMethods(){}
		public override string Exec(){
			string r="=== 以下 class GetMethods\n";
			foreach(System.Reflection.MethodInfo m in typeof(GetMethods).GetMethods()){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			r+="=== 以下 class System.GC\n";
			foreach(System.Reflection.MethodInfo m in typeof(System.GC).GetMethods()){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			r+="=== 以下 class System.GC (Static|Public)\n";
			foreach(System.Reflection.MethodInfo m in typeof(System.GC).GetMethods(
				System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public
			)){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			return r;
		}
		public string Parameter2String(System.Reflection.MethodInfo m){
			string r="";
			foreach(System.Reflection.ParameterInfo p in m.GetParameters()){
				r+=","+p.ParameterType.ToString();
			}
			return r.Length==0?"":r.Substring(1);
		}
	}
	[TestFunctionAttribute("System.Type#IsSubclassOf テスト",@"IsSubclassOf で継承(実装)した interface を指定しても true を返すか否か")]
	public class IsSubclassOf:TestFunction,IIsSubclassOf{
		public IsSubclassOf(){}
		public override string Exec(){
			string r="";
			System.Type t=typeof(IsSubclassOf);
			r+=t.IsSubclassOf(typeof(IIsSubclassOf))
				?"継承(実装)した interface でも true を返します"
				:"interface を継承(実装)していても false を返します";
			r+="\n";
			r+=t.GetInterface("IIsSubclassOf")!=null
				?"GetInterface(\"...\")!=null を使えます":"GetInterface は×";
			r+="\n";
			return r;
		}
	}
	public interface IIsSubclassOf{
		string Exec();
	}

	[TestFunctionAttribute("params 引数",@"params 引数の ParameterInfo
…ParamsAttribute を GetCustomAttributes で取得する事が出来るか?
引数無しで呼び出す事が出来るか?
引数の配列を代わりに置いて呼び出す事が出来るか?")]
	public class ParamsAttribute:TestFunction{
		public ParamsAttribute(){}
		public override string Exec(){
			string r=this.hasParams(typeof(ParamsAttribute).GetMethod("testF").GetParameters()[0])
				?"GetCustomAttribute で取得する事が出来ます":"GetCustomAttribute では取得する事は出来ません";
			r+="\n";
			r+=@"引数無しで呼び出す事が出来ます。
	引数配列の要素の数は "+this.testF()+" 個です。\n";
			int[] ints=new int[]{1,2,2,3,3,4};
			r+=@"引数の配列を代わりに置いて呼び出す事が出来ます。
	渡した配列の要素の数は 6 個です。
	引数配列の要素の数は "+this.testF(ints)+" 個です。\n";
			r+=@"引数の配列を複数代わりに置いて呼び出す事は出来ません。
	(コンパイルエラーになった)";
			/*
			r+=@"更に引数の配列を複数代わりに置いて呼び出す事が出来ます。
	渡した配列の要素の数は 6 個です。
	渡した配列の数は 2 個です。
	直接指定した引数は 1 個です。
	引数配列の要素の数は "+this.testF(ints,ints)+" 個です。\n";//*/
			return r;
		}
		private bool hasParams(System.Reflection.ParameterInfo p){
			return p.GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0;
		}
		public int testF(params int[] ints){
			return ints.Length;
		}
	}
}