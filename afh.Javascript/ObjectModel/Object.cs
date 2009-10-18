#define doublehash
using Generic=System.Collections.Generic;
namespace afh.JavaScript{
	/// <summary>
	/// Javascript 上のクラスオブジェクトの基本ともなるべき Object を表すクラスです。
	/// </summary>
	public class Object{
		//===========================================================
		//		field:members
		//===========================================================
		/// <summary>
		/// members: 通常のメンバ
		/// </summary>
		private Generic::Dictionary<string,JavaScript.Object> members;
		/// <summary>
		/// 指定した名前のメンバが _members に設定されているかどうかを確認
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <returns>指定した名前のメンバが含まれているかどうかを返します</returns>
		protected bool members_contain(string propName){
			if(this.members==null)return false;
			return this.members.ContainsKey(propName);
		}
		/// <summary>
		/// 指定した名前メンバを _members から取得します
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <returns>取得したメンバを帰します</returns>
		protected JavaScript.Object members_get(string propName){
			if(this.members==null||!this.members.ContainsKey(propName))return null;
			return this.members[propName];
		}
		/// <summary>
		/// _members の指定した名前に新しくメンバを設定します
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <param name="obj">設定するオブジェクト</param>
		protected void members_set(string propName,JavaScript.Object obj){
			if(this.members==null)this.members=new Generic::Dictionary<string,JavaScript.Object>();
			this.members[propName]=obj;
		}
#if doublehash
		//===========================================================
		//		field:pmembers
		//===========================================================
		/// <summary>
		/// private members: システムの側で隠して置くメンバ
		/// </summary>
		private Generic::Dictionary<string,JavaScript.Object> pmembers;
		/// <summary>
		/// 指定した名前のメンバが _members に設定されているかどうかを確認
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <returns>指定した名前のメンバが含まれているかどうかを返します</returns>
		protected bool pmembers_contain(string propName){
			if(this.pmembers==null)return false;
			return this.pmembers.ContainsKey(propName);
		}
		/// <summary>
		/// 指定した名前メンバを _members から取得します
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <returns>取得したメンバを帰します</returns>
		protected JavaScript.Object pmembers_get(string propName){
			if(this.pmembers==null||!this.pmembers.ContainsKey(propName))return null;
			return this.pmembers[propName];
		}
		/// <summary>
		/// _members の指定した名前に新しくメンバを設定します
		/// </summary>
		/// <param name="propName">メンバの名前</param>
		/// <param name="obj">設定するオブジェクト</param>
		protected void pmembers_set(string propName,JavaScript.Object obj){
			if(this.pmembers==null)this.pmembers=new Generic::Dictionary<string,JavaScript.Object>();
			this.pmembers[propName]=obj;
		}
#endif
		//===========================================================
		//		field:_proto
		//===========================================================
		/// <summary>
		/// __proto__ (この関数の継承元を表現する為のオブジェクト) を保持します
		/// </summary>
		private JavaScript.Object _proto;
		/// <summary>
		/// このオブジェクトが参照する prototype を取得亦は設定します。
		/// </summary>
		/// <remarks>
		/// 通常の物を用いると "__proto__ を検索する為に __proto__ を検索する"
		/// という無限ループになる為 __proto__ は特別に別関数で準備
		/// </remarks>
		public JavaScript.Object __proto__{
			get{
				JavaScript.Object o=this.members_get("__proto__");
				if(o!=null)return o;
#if doublehash
				if((o=this.pmembers_get("__proto__"))!=null)return o;
#endif
				if(this._proto!=null)return this._proto;
				return this._proto=new Null();		// 必要になってから代入
			}
			set{this._proto=value;}
		}//*/
		//===========================================================
		//		.ctor
		//===========================================================
		public Object(){}
		public Object(JavaScript.Object __proto__):this(){
			this.__proto__=__proto__;
		}
		/// <summary>
		/// Object の新しい instance を取得します。
		/// </summary>
		/// <returns>新しいインスタンスを返します。</returns>
		public static JavaScript.Object Construct(){
			JavaScript.Object r=new Object();
			r.__proto__=Global._global["Object"]["prototype"];
			return r;
		}
		//===========================================================
		//		Member Access
		//===========================================================
		/// <summary>
		/// このオブジェクトの持つメンバを検索します
		/// </summary>
		public virtual JavaScript.Object this[string propName]{
			get{
				if(propName=="__proto__")return this.__proto__;
				//--本来のメンバ
				JavaScript.Object o=this.members_get(propName);
				if(o!=null)return o;
#if doublehash
				//--隠しメンバ
				if(null!=(o=this.pmembers_get(propName)))return o;
#endif
				//--継承元のメンバ
				if(null!=(o=this._proto))if(null!=(o=o[propName]))return o;
				return null;
			}
			set{this.members_set(propName,value);}
		}
		/// <summary>
		/// Object 木を辿って目的のメンバの値を返します
		/// </summary>
		/// <param name="index">
		/// 次に検索するべき names の位置。
		/// このメソッドを持つインスタンスが
		/// names[index] と言う名のメンバを持つ事が期待されています。
		/// </param>
		/// <param name="names">オブジェクトの系譜。<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <returns>
		/// 見つかったメンバを返します。
		/// 指定したメンバ自体が未定義だった場合には null 値を返します。
		/// </returns>
		/// <exception cref="Null.UndefinedException">
		/// 指定したメンバに辿り着く前に未定義があった場合に発生します。
		/// 指定したメンバ自体が未定義だった場合には発生しません。
		/// </exception>
		public JavaScript.Object GetValue(int index,string[] names){
			JavaScript.Object o=this.TraceMember(index,names.Length-1,names);
			if(this is JavaScript.ManagedObject&&o is JavaScript.ManagedMethod){
				//--ManagedMethod の場合
				return new JavaScript.ManagedDelegate(
					((JavaScript.ManagedObject)this).Value,
					(JavaScript.ManagedMethod)o
				);
			}else return o;
		}
		/// <summary>
		/// Object 木を辿って目的のメンバを設定します
		/// </summary>
		/// <param name="obj">設定値を表すオブジェクトです</param>
		/// <param name="index">
		/// 次に検索するべき names の位置。
		/// このメソッドを持つインスタンスが
		/// names[index] と言う名のメンバを持つ事が期待されています。
		/// </param>
		/// <param name="names">オブジェクトの系譜。<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <exception cref="Null.UndefinedException">
		/// 指定したメンバが未定義だった場合に発生します。
		/// </exception>
		public void SetValue(JavaScript.Object obj,int index,string[] names){
			//--直接の親と key の取得
			int dest=names.Length-2;
			JavaScript.Object parent=index<=dest?this.TraceMember(index,dest,names):this;
			string key=names[dest+1];
			//--値の設定
			if(parent[key] is JavaScript.PropertyBase){
				((JavaScript.PropertyBase)parent[key]).SetValue(parent,obj);
			}else parent[key]=obj;
		}
		/// <summary>
		/// Object 木を辿って目的のメンバ関数を実行します
		/// </summary>
		/// <param name="index">
		/// 次に検索するべき names の位置。
		/// このメソッドを持つインスタンスが
		/// names[index] と言う名のメンバを持つ事が期待されています。
		/// </param>
		/// <param name="names">オブジェクトの系譜。<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <returns>見つかったメンバを実行した結果を返します</returns>
		/// <exception cref="Null.UndefinedException">
		/// 指定したメンバが未定義だった場合に発生します。
		/// </exception>
		public JavaScript.Object InvokeMember(int index,string[] names,JavaScript.Array arguments){
			//--直接の親と key の取得
			int dest=names.Length-2;
			JavaScript.Object parent=index<=dest?this.TraceMember(index,dest,names):this;
			string key=names[dest+1];
			//--Invoke
			try{
				return ((FunctionBase)parent[key]).Invoke(parent,arguments);
			}catch(System.NullReferenceException e){
				//※: 一回目の NullReferenceException は時間がかかる
				throw new Null.UndefinedException(e);
			}catch(System.InvalidCastException e){
				throw new System.Exception("指定したオブジェクトは関数ではありません",e);
			}catch(System.Exception e){throw e;}
		}
		/// <summary>
		/// 子孫を辿っていきます
		/// </summary>
		/// <param name="index">
		/// 次に検索するべき names の位置。
		/// このメソッドを持つインスタンスが
		/// names[index] と言う名のメンバを持つ事が期待されています。
		/// 負の値を指定した時は 0 と見なされます。
		/// </param>
		/// <param name="dest">names の何番目(Base0)に対応する Javascript.Object を取得するかを指定します</param>
		/// <param name="names">
		/// オブジェクトの系譜。
		/// Object.prototype.myProperty の場合は <code>new string[]{"Object","prototype","myProperty"}</code> と表現されます
		/// </param>
		/// <returns>
		/// dest で指定した Javascript.Object を返します。
		/// 指定した物がプロパティの場合、その値を返します。
		/// </returns>
		/// <exception cref="Null.UndefinedException">
		/// 指定したメンバが未定義だった場合に発生します。
		/// </exception>
		internal virtual JavaScript.Object TraceMember(int index,int dest,string[] names){
			JavaScript.Object o=this[names[index]];
			if(o==null)
				throw new Null.UndefinedException(null,index,names);
			if(o is JavaScript.PropertyBase)
				o=((JavaScript.PropertyBase)o).GetValue(this);
			return index<dest?o.TraceMember(index+1,dest,names):o;
		}
		//===========================================================
		//		.NET 相互運用
		//===========================================================
		/// <summary>
		/// 指定した型に変換する際の適合性を返します。
		/// 指定した型に変換する事が出来ない場合には float.NegativeInfinity を返します。
		/// これは、Javascript から .NET のオーバーロードされた関数を呼び出す際に、
		/// 引数の型を参考にして関数を選択するのに使用します。
		/// </summary>
		/// <remarks>
		/// 通常の Object では、object (System.Object) に変換する際には 1,
		/// string (System.String) に変換する際には 0,
		/// bool (System.Boolean) に変換する際には -1
		/// の適合性になっています。
		/// <para>
		/// ※ Processor の命令 set の関係で、float よりも double にした方が速いかも
		/// </para>
		/// </remarks>
		/// <param name="t">変換先の型</param>
		/// <returns>変換の適合性を返します</returns>
		public virtual float ConvertCompat(System.Type t){
			if(t==typeof(object)||t==typeof(JavaScript.Object))return 1;
			if(t==typeof(string))return 0;
			if(t==typeof(bool))return -1;
			return float.NegativeInfinity;
		}
		/// <summary>
		/// 指定した型に変換します。
		/// 指定した型に変換する事が出来ない場合は null 値を返します。
		/// </summary>
		/// <param name="t">変換先の型</param>
		/// <returns>変換した結果のオブジェクトを返します。</returns>
		public virtual object Convert(System.Type t){
			if(t==typeof(object)||t==typeof(JavaScript.Object))return this;
			if(t==typeof(string))return this.ToString();
			if(t==typeof(bool))return true;
			return null;
		}
		//===========================================================
		//		Other Methods
		//===========================================================
#if doublehash
		// 通常メンバのみのコピー
		public void CopyMembers(JavaScript.Object source){
			foreach(string key in source.members.Keys){
				this.members[key]=source.members[key];
			}
		}
#else
		public void CopyMembers(Javascript.Object source){
			foreach(string key in source.PublicOwnKeys){
				this.members[key]=this.members[source.members[key]];
			}
		}
		//===========================================================
		//		列挙
		//===========================================================
		public System.Collections.IEnumerable PublicOwnKeys{get{return new ObjectEnumerable(this);}}
		private class ObjectEnumerable:System.Collections.IEnumerable{
			private Javascript.Object obj;
			public ObjectEnumerable(Javascript.Object o){this.obj=o;}
			public System.Collections.IEnumerator GetEnumerator(){return new ObjectEnumerator(this.obj);}
		}
		private class ObjectEnumerator:System.Collections.IEnumerator{
			private const string OUTOFRANGE="列挙子が、コレクションの最初の要素の前、または最後の要素の後に位置しています。";
			private System.Collections.Hashtable hash;
			private System.Collections.IEnumerator ie;
			public object Current{get{return ie.Current;}}
			public ObjectEnumerator(Javascript.Object o){
				this.hash=o.members;
				this.ie=this.hash.Keys.GetEnumerator();
			}
			public void Reset(){this.ie.Reset();}
			public bool MoveNext(){
				do{
					if(!this.ie.MoveNext())return false;
					System.Type t=this.hash[ie.Current].GetType();
				}while(
					t.IsSubclassOf(typeof(Javascript.ManagedMethod))
					||t.IsSubclassOf(typeof(Javascript.ManagedProperty))
				);
				return true;
			}
		}
#endif
		//public virtual Javascript.Object Clone(){}
		internal static void Initialize(){
			JavaScript.Object o=new ManagedDelegate(typeof(JavaScript.Object),"Construct");

			//CHECK_OK:
			// これを使用すると object クラスの ToString() Method が使用されて
			// オーバーライドされた物が使用されないかも知れない…
			// →実際にやってみた所、ちゃんと継承先でオーバーライドした物が呼び出された。
			o["prototype"]["toString"]=new ManagedMethod(typeof(object),"ToString");
			Global._global["Object"]=o;
		}
	}

	/// <summary>
	/// Javascript 上での null 値を表すクラス
	/// </summary>
	public class Null:Object{
		public Null():base(){}
		public override Object this[string propName]{
			get{return null;}
			set{throw new NullRefException();}
		}
		public override object Convert(System.Type t){
			if(t==typeof(bool))return false;
			return base.Convert(t);
		}
		public class NullRefException:System.Exception{
			public NullRefException(){}
		}
		public class UndefinedException:System.Exception{
			private const string MSG="指定した識別子は定義されていません";
			public UndefinedException(System.Exception e,int index,string[] names)
				:base(CreateMessage(index,names),e){}
			public UndefinedException(int index,string[] names)
				:base(CreateMessage(index,names)){}
			public UndefinedException(System.Exception e):base(MSG,e){}
			public UndefinedException():base(MSG){}
			/// <summary>
			/// 識別子の系譜 names と undefined であった位置 index から、
			/// 識別子が定義されていないというメッセージを作成します
			/// </summary>
			/// <param name="index">undefined であった位置</param>
			/// <param name="names">識別子の系譜</param>
			/// <returns>メッセージを string で返します</returns>
			private static string CreateMessage(int index,string[] names){
				string r="UndefinedException";
				if(index>=names.Length){
					r+="内部 Error: index が names.Length の範囲を超えています\n";
					r+="\tindex: "+index.ToString()+"\n";
					r+="\tnames.Length: "+names.Length.ToString()+"\n";
					index=names.Length-1;
				}
				if(names.Length>0){
					r+=names[0];
					for(int i=1;i<index;i++){
						r+="."+names[i];
					}
					r+=" は定義されていません\n";
				}
				return r;
			}
		}
	}

	public class ManagedObject:JavaScript.Object{
		private object obj;
		public object Value{get{return this.obj;}}
		public ManagedObject(object obj):base(new Null()){
			this.obj=obj;
			// TODO: メソッドの公開
			//  公開するメソッドを保持する prototype を作成して、
			//  System.Type を key に静的な Hashtable で管理
		}

		public override string ToString() {
			return "<mgdObj:"+this.obj.ToString()+">";
		}
	}
	//public class ManagedNamespace:Javascript.Object{}
	// GetValue SetValue InvokeMember 等の処理を置き換える
	/*
	public class Property:Object{

	}//*/
}
