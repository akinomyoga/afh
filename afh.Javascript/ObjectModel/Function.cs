namespace afh.JavaScript{
	public abstract class FunctionBase:Object{
		/// <summary>
		/// FunctionBase のコンストラクタ
		/// prototype を設定します
		/// </summary>
		protected FunctionBase(){
			this["prototype"]=new JavaScript.Object();
		}
		/// <summary>
		/// この関数を実行します。
		/// </summary>
		/// <param name="obj">この関数を保持する Object です。関数内からは this で参照されます</param>
		/// <param name="arguments">この関数に渡される引数の配列を設定します</param>
		/// <returns>この関数を実行した結果を返します</returns>
		public abstract JavaScript.Object Invoke(JavaScript.Object obj,JavaScript.Array arguments);
		//===========================================================
		//		.NET⇔Javascript 用の静的メソッド
		//===========================================================
		/// <summary>
		/// 指定した .NET のクラス・構造体から、
		/// 指定した名前を持つインスタンスメソッドを取得します
		/// </summary>
		/// <param name="t">クラス・構造体を表す System.Type</param>
		/// <param name="name">メソッドを表す名前</param>
		/// <returns>見つかったメソッド情報の配列</returns>
		public static System.Reflection.MethodInfo[] GetMethods(System.Type t,string name){
			//CHECK>OK: GetMethods で継承元のメンバも検索してくれるのかどうか不明
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in t.GetMethods()){
				if(m.IsStatic||m.Name!=name)continue;
				list.Add(m);
			}
			return (System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// Javascript 側で指定した引数の個数の方が MethodInfo の要求する引数の個数より多い場合、
		/// 過剰な引数一個あたりの適合性
		/// </summary>
		public const float COMPAT_OVERPARAM=-1.5f;
		/// <summary>
		/// Javascript 側で指定した引数と、指定した MethodInfo の引数の適合性を判定します
		/// </summary>
		/// <returns>
		/// 計算した適合性を返します。
		/// 引数を変換する事が出来ない場合には float.NegativeInfinity を返します
		/// </returns>
		/// <remarks>
		/// TODO: params 修飾子が付いている時、一つも要素を指定しない場合の動作を実装
		/// 1. arrParam.Length>arguments.length の判定だけでは不十分
		/// 2. 点数を低くする(params の付いた引数だけ除いた別のオーバーロードの可能性)
		/// </remarks>
		public static float ConvertParamsCompat(System.Reflection.MethodInfo m,JavaScript.Array arguments){
			System.Reflection.ParameterInfo[] arrParam=m.GetParameters();
			if(arrParam.Length==0)return 0;
			if(arrParam.Length>arguments.length)return float.NegativeInfinity;
			int iM=arrParam.Length;
			float r=0;
			for(int i=0;i<iM;i++){
				//CHECK>OK: params の入った関数を識別出来るか実際に確認する事
				if(i+1==iM&&arrParam[iM-1].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
					System.Type t=arrParam[iM-1].ParameterType.GetElementType();
					iM=arguments.length;
					for(;i<iM;i++)r+=arguments[i].ConvertCompat(t);
					break;
				}
				r+=arguments[i].ConvertCompat(arrParam[i].ParameterType);
			}
			return r+(arguments.length-iM)*COMPAT_OVERPARAM;
		}
		/// <summary>
		/// Javascript 側で指定した Javascript.Array 型の引数配列を
		/// object[] 型に変換します。
		/// </summary>
		/// <param name="m">最終的に呼び出したいメソッド</param>
		/// <param name="arguments">呼び出しに用いる Javascript.Array の引数配列</param>
		/// <returns>実際に呼び出しに用いる事の出来る引数配列 object[]</returns>
		/// <remarks>
		/// 実装: ConvertParamsCompat の処理と似た処理を行うので、
		/// ConvertParamsCompat の際に System.Type[] を作って置くのも手。
		/// </remarks>
		public static object[] ConvertParams(System.Reflection.MethodInfo m,JavaScript.Array arguments){
			System.Reflection.ParameterInfo[] arrParam=m.GetParameters();
			int iM=arrParam.Length;
			object[] r=new object[iM];
			for(int i=0;i<iM;i++){
				if(i+1==iM&&arrParam[iM-1].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
					//変換
					System.Collections.ArrayList list=new System.Collections.ArrayList();
					System.Type t=arrParam[iM-1].ParameterType.GetElementType();
					for(;i<arguments.length;i++)list.Add(arguments[i].Convert(t));

					r[iM-1]=list.ToArray(t);
				}else{
					r[i]=arguments[i].Convert(arrParam[i].ParameterType);
				}
			}
			return r;
		}
	}
/*
=====================================================================
		Function
---------------------------------------------------------------------
	Javascript で定義された関数
	Context という Javascript.Object を準備しそこに変数を格納する
		this		(落ち着いてきたら) this に代入する事は不可能とする
					(this に代入する事を許可しても良いが、その行為は間違いの元になる為)
		caller		呼び出しもと関数の情報
		arguments	[out][ref] 等の実装も考えても良い
		__proto__	関数を定義した所の変数に対する参照(若し一番外側ならば __proto__=_global)
					(※値を変更する時には元の変数を書き換える様にする)
			例:		function main(){
						var func=funtion(){
							...
						}
					}
					
					func.Context -------→ main.Context -------→ _global
					             __proto__              __proto__

*/
	/// <summary>
	/// .NET のインスタンスメソッドを表すオブジェクトです。
	/// このクラスは型情報とメソッド情報を持ち、
	/// 実行時に指定したオブジェクトの該当メソッドを実行します。
	/// </summary>
	/// <remarks>
	/// Object を関数が離れていく時は、親オブジェクトを記憶した ManagedDelegate に自動的に変換されます。
	/// </remarks>
	public class ManagedMethod:FunctionBase{
		internal System.Type type;
		internal System.Reflection.MethodInfo[] methods;
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// ManagedMethod を型とメソッド名及び
		/// メソッドの形態を表す BindingFlags によってインスタンス化します
		/// </summary>
		/// <param name="t">メソッドを持つ型</param>
		/// <param name="name">メソッドの名前</param>
		/// <param name="fBind">メソッドの形態を限定</param>
		/// <remarks>
		/// 実装は fBind の無い場合と殆ど同じ
		/// …異なるのは this.type.GetMethods() の引数だけ
		/// </remarks>
		public ManagedMethod(System.Type t,string name,System.Reflection.BindingFlags fBind){
			this.type=t;
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in this.type.GetMethods(fBind)){
				if(m.Name!=name)continue;
				list.Add(m);
			}
			this.methods=(System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// ManagedMethod を型とメソッド名によってインスタンス化します
		/// </summary>
		/// <param name="t">メソッドを持つ型</param>
		/// <param name="name">メソッドの名前</param>
		public ManagedMethod(System.Type t,string name){
			this.type=t;
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in this.type.GetMethods()){
				if(m.Name!=name)continue;
				list.Add(m);
			}
			this.methods=(System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// オブジェクトの型とメソッド情報を使用して、ManageMethod のインスタンスを作成します。
		/// </summary>
		/// <param name="t">メソッドを持つオブジェクトの型</param>
		/// <param name="methods">
		/// t で指定した型の持つメソッドの情報。
		/// t に指定した型以外の型が持つメソッドを指定しないで下さい。
		/// </param>
		public ManagedMethod(System.Type t,params System.Reflection.MethodInfo[] methods){
			this.type=t;
			this.methods=methods;
		}
		//===========================================================
		//		Invoke
		//===========================================================
		/// <summary>
		/// 指定したオブジェクトの当該メソッドを実行します
		/// </summary>
		/// <param name="obj">オブジェクトを指定します</param>
		/// <param name="arguments">メソッドに渡す引数を指定します</param>
		/// <returns>メソッドの結果を返します</returns>
		/// <remarks>
		/// 案1: Type と Methods を保持し Invoke の命令が来たら
		/// <list>
		/// <item><description>Type と obj を照合</description></item>
		/// <item><description>引数リストの比較</description></item>
		/// <item><description>実行</description></item>
		/// </list>
		/// </remarks>
		public override JavaScript.Object Invoke(JavaScript.Object obj,JavaScript.Array arguments){
			// ManagedObject 以外の時 (Javascript.Object を直接指定した場合の時) にも対応する事にする。
			//	古いコード
			//		if(!(obj is Javascript.ManagedObject))throw new System.ArgumentException(INVOKE_NOTMANAGED,"obj");
			//		object o=((Javascript.ManagedObject)obj).Value;
			object o=(obj is JavaScript.ManagedObject)?((JavaScript.ManagedObject)obj).Value:obj;

			//CHECK>×: IsSubclassOf で継承した interface も確認する事が出来るかどうか
			if(!Global.IsCastable(o.GetType(),this.type))throw new System.ArgumentException(INVOKE_NOTSUPPORTED,"obj");
			System.Reflection.MethodInfo m;
			try{
				m=this.SelectOverload(arguments);
			}catch(System.Exception e){throw e;}
			object ret=m.Invoke(o,FunctionBase.ConvertParams(m,arguments));
			return Global.ConvertFromManaged(ret);
		}
		private const string INVOKE_NOTMANAGED="obj には ManagedObject を指定して下さい";
		private const string INVOKE_NOTSUPPORTED="指定したオブジェクトは当メソッドを持ちません";
		/// <summary>
		/// 指定した引数に最も適合するメソッドのオーバーロードを検索します
		/// </summary>
		/// <param name="arguments">引数</param>
		/// <returns>最も適合するメソッドのオーバーロード</returns>
		/// <exception cref="ArgumentException">
		/// 指定した引数に適合するメソッドのオーバーロードが見つからなかった場合に発生します。
		/// </exception>
		protected System.Reflection.MethodInfo SelectOverload(JavaScript.Array arguments){
			float comp0,compat=float.NegativeInfinity;
			System.Reflection.MethodInfo m0,m=null;
			int iM=this.methods.Length;
			int w=0;	//重なり
			for(int i=0;i<iM;i++){
				m0=this.methods[i];
				comp0=FunctionBase.ConvertParamsCompat(m0,arguments);
				if(comp0<compat)continue;
				if(comp0==compat){w++;continue;}
				w=1;
				compat=comp0;
				m=m0;
			}
			if(compat==float.NegativeInfinity)throw new System.ArgumentException(OVERLOAD_NONE,"arguments");
			if(w>1)throw new System.ArgumentException(OVERLOAD_AMBIGUOUS,"arguments");
			return m;
		}
		private const string OVERLOAD_NONE="指定した引数に適合するオーバーロードが見つかりませんでした";
		private const string OVERLOAD_AMBIGUOUS="指定した引数の型が曖昧な為オーバーロードを一つに絞れません";
	}
	
	/// <summary>
	/// .NET Framework のメソッドを表すクラスです。
	/// 特定のオブジェクトのインスタンスメソッド、
	/// 亦は、特定のクラス・構造体の静的メソッドを表します。
	/// 特定のインスタンスのメソッドを表す時には、
	/// このクラスはそのインスタンスへの参照と、メソッド情報を保持します。
	/// 静的メソッドを表す時には、メソッド情報のみを保持します。
	/// </summary>
	public class ManagedDelegate:ManagedMethod{
		/// <summary>
		/// このクラスインスタンスの保持する
		/// managed object (.NET Framework の object) への参照。
		/// </summary>
		protected object obj;
		//===========================================================
		//		.ctor ; インスタンスメソッド用
		//===========================================================
		public ManagedDelegate(object obj,System.Type type,params System.Reflection.MethodInfo[] methods):base(type,methods){
			this.obj=obj;
		}
		public ManagedDelegate(object obj,ManagedMethod meth):base(meth.type,meth.methods){
			this.obj=obj;
		}
		public ManagedDelegate(object obj,System.Type type,string methodName):base(type,methodName){
			this.obj=obj;
		}
		//===========================================================
		//		.ctor ; 静的メソッド用
		//===========================================================
		/// <summary>
		/// 静的メソッドを表す MethodDelegate のコンストラクタ
		/// </summary>
		/// <param name="methods">静的メソッドの情報</param>
		public ManagedDelegate(System.Reflection.MethodInfo methods):this(null,null,methods){}
		/// <summary>
		/// 静的メソッドを表す MethodDelegate のコンストラクタ
		/// </summary>
		/// <param name="type">静的メソッドの所属するクラス・構造体</param>
		/// <param name="methodName">
		/// 静的メソッドの名前。
		/// TODO: 静的・非静的のチェック機構を作らなければならない…bindingFlags を使用
		/// </param>
		public ManagedDelegate(System.Type type,string methodName):this(null,type,methodName){}
		//===========================================================
		//		Invoke
		//===========================================================
		public override Object Invoke(JavaScript.Object dummy, Array arguments){
			System.Reflection.MethodInfo m;
			try{
				m=this.SelectOverload(arguments);
			}catch(System.Exception e){throw e;}
			object ret=m.Invoke(this.obj,FunctionBase.ConvertParams(m,arguments));
			return Global.ConvertFromManaged(ret);
		}
	}
	/// <summary>
	/// Javascript.Object を引数に取る二項演算メソッドの .NET 実装を表現します。
	/// オーバーロードには対応していません。
	/// </summary>
	public class ManagedJSBinaryOperator:FunctionBase{
		System.Reflection.MethodInfo minfo;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m">
		/// Javascript.Object の二項演算を実行するメソッドの関数です。
		/// 返値は Javascript.Object 及びそれを継承するオブジェクトである必要があります。
		/// <para>インスタンスメソッドの場合:
		/// 関数は一つパラメータを持ち、Javascript.Object を指定する事が出来る必要があります。
		/// これは、インスタンス作成時に確認されます。
		/// 一方で、this に指定する変数 (メソッドを保持するインスタンス) の型は実行時に確認されます。
		/// </para>
		/// <para>静的メソッドの場合:
		/// 関数は二つパラメータを持ち、共に Javascript.Object を指定する事が出来る必要があります。
		/// これは、インスタンス作成時に確認されます。
		/// </para>
		/// </param>
		public ManagedJSBinaryOperator(System.Reflection.MethodInfo m){
			this.minfo=m;
			if(!Global.IsCastable(m.ReturnType,typeof(JavaScript.Object)))goto err;
			System.Reflection.ParameterInfo[] pinfos=this.minfo.GetParameters();
			if(this.minfo.IsStatic){
				if(pinfos.Length!=2)goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[0].ParameterType)) goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[1].ParameterType)) goto err;
			}else{
				if(pinfos.Length!=1)goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[0].ParameterType)) goto err;
			}
			return;
		err:
			throw new System.ApplicationException("ManagedJSBinaryOperator: 指定したメソッドは Javascript.Object を引数に取る二項演算として不適切です。");
		}
		public override Object Invoke(Object obj,Array arguments) {
			if(arguments.length>0)return InvokeBinaryOperator(obj,arguments[0]);
			else throw new System.ArgumentException("引数は少なくとも一つは必要です。","arguments");
		}
		public JavaScript.Object InvokeBinaryOperator(JavaScript.Object left,JavaScript.Object right){
			if(this.minfo.IsStatic){
				return (JavaScript.Object)this.minfo.Invoke(null,new object[]{left,right});
			}else{
				if(!Global.IsCastable(left.GetType(),this.minfo.DeclaringType))
					throw new System.ArgumentException("ManagedJSBinaryOperator: 指定した引数は this パラメータの型に合致しません。","left");
				return (JavaScript.Object)this.minfo.Invoke(left,new object[]{right});
			}
		}
	}
}
