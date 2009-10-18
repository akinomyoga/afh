namespace afh.JavaScript{
	public abstract class PropertyBase:JavaScript.Object{
		public PropertyBase(){}
		/// <summary>
		/// parent の当該プロパティから値を取得します
		/// </summary>
		/// <param name="parent">プロパティの持ち主であるオブジェクト</param>
		/// <returns>プロパティから取得した値を返します</returns>
		public virtual JavaScript.Object GetValue(JavaScript.Object parent){
			JavaScript.FunctionBase f=this[":propget:"] as JavaScript.FunctionBase;
			if(f==null)throw new System.NotSupportedException(GET_NOTSUPPORT);
			return f.Invoke(parent,new JavaScript.Array());
		}
		private const string GET_NOTSUPPORT="この Property は get をサポートしていません";
		/// <summary>
		/// parent の当該プロパティに値を設定します
		/// </summary>
		/// <param name="parent">プロパティの持ち主であるオブジェクト</param>
		/// <param name="value">プロパティに設定する値</param>
		public virtual void SetValue(JavaScript.Object parent,JavaScript.Object value){
			JavaScript.FunctionBase f=this[":propput:"] as JavaScript.FunctionBase;
			if(f==null)throw new System.NotSupportedException(SET_NOTSUPPORT);
			f.Invoke(parent,Array.Construct(value));
		}
		private const string SET_NOTSUPPORT="この Property は set をサポートしていません";
		/// <summary>
		/// TraceMember を override します。
		/// Property に関しては TraceMember が呼び出される事は想定されていないので例外を投げます。
		/// </summary>
		internal override JavaScript.Object TraceMember(int index,int dest,string[] names){
			throw new System.Exception(TRACEMEM_NOTSUPPOSED);
		}
		private const string TRACEMEM_NOTSUPPOSED="内部 Error: Property Object 自体の trace は予期していません";
	}
	/// <summary>
	/// .NET のオブジェクトのプロパティを表します。
	/// </summary>
	public class ManagedProperty:PropertyBase{
		private const string NOTSUPPORTED="指定したオブジェクトはこのプロパティを持ちません";
		//System.Reflection.PropertyInfo prop;
		/// <summary>
		/// ManagedProperty インスタンスを PropertyInfo から作成します。
		/// </summary>
		/// <param name="prop">PropertyInfo</param>
		public ManagedProperty(System.Reflection.PropertyInfo prop){
			//this.prop=prop;
			System.Type type=prop.ReflectedType;
			if(prop.CanRead)this[":propget:"]=new ManagedMethod(type,prop.GetGetMethod());
			if(prop.CanWrite)this[":propput:"]=new ManagedMethod(type,prop.GetSetMethod());
		}
	}
}