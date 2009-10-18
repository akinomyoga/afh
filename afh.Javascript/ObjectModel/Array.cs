namespace afh.JavaScript{
	/*
	 * 配列の実装
	 * 
	 * * 今迄に設定された値の中で最も大きい物を length として返す
	 * * length より小さい値へのアクセスは、メンバが無くても null を返す
	 * */
	/// <summary>
	/// 配列を表すオブジェクトです
	/// </summary>
	public class Array:Object{
		private int len=0;
		public Array(){}
		public Array(object[] array){
			foreach(object o in array)this[this.len++]=Global.ConvertFromManaged(o);
		}
		public Array(System.Collections.IEnumerable list){
			foreach(object o in list)this[this.len++]=Global.ConvertFromManaged(o);
		}
		/// <summary>
		/// 配列の初期化子
		/// </summary>
		/// <param name="array">配列の要素を指定します。</param>
		/// <returns>指定した引数を要素として持つ配列 Javascript.Array を返します。</returns>
		public static JavaScript.Array Construct(params JavaScript.Object[] array){
			JavaScript.Array ret=new JavaScript.Array();
			foreach(JavaScript.Object o in array)ret[ret.len++]=Global.ConvertFromManaged(o);
			return ret;
		}
		/// <summary>
		/// 配列の初期化子
		/// </summary>
		/// <param name="count">配列の大きさを指定します</param>
		/// <returns>指定した大きさを持つ配列 Javascript.Array を返します</returns>
		public static Array Construct(int count){
			JavaScript.Array array=new Array();
			array.len=count;
			return array;
		}

		public JavaScript.Object this[int i]{
			get{
				Object r=this[i.ToString()];
				return r==null&&i<this.len?new Null():r;
			}
			set{
				this[i.ToString()]=value;
				if(this.len<=i)this.len=i+1;
			}
		}
		public override Object this[string propName]{
			get{
				//TODO: property での実装に変更の予定
				if(propName=="length")return new Number(this.length);
				return base[propName];
			}
			set{base[propName]=value;}
		}


		public int length{get{return this.len;}}

		public override string ToString() {
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			sb.Append("[");
			if(this.len>0){
				sb.Append(this[0].ToString());
				for(int i=1;i<this.len;i++){
					sb.Append(",");
					sb.Append(this[i].ToString());
				}
			}
			sb.Append("]");
			return sb.ToString();
		}

		internal static new void Initialize(){
			Global._global["Array"]=new ManagedDelegate(typeof(Array),"Construct");
			JavaScript.Object o=JavaScript.Object.Construct();
			Global._global["Array"]["prototype"]=o;
		}
	}
}