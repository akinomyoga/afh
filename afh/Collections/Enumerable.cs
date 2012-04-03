using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// 様々な System.Collections.IEnumerator を生成する為のクラスです。
	/// </summary>
	[System.Serializable]
	public class Enumerable:System.Collections.IEnumerable{
		/// <summary>
		/// 指定した型の物のみ列挙します。
		/// </summary>
		/// <typeparam name="T">列挙する型を指定します。</typeparam>
		/// <returns>指定した要素のみ列挙する列挙子を返します。</returns>
		public static Gen::IEnumerable<T> EnumByType<T>(System.Collections.IEnumerable baseEnumerable){
			foreach(object o in baseEnumerable)if(o is T)yield return (T)o;
		}
		private System.Collections.IEnumerable @enum;
		/// <summary>
		/// このインスタンスに関連付けられた列挙子を取得します。
		/// </summary>
		/// <returns>列挙子を返します。</returns>
		public System.Collections.IEnumerator GetEnumerator(){return this.@enum.GetEnumerator();}
		//=================================================
		//		演算子など
		//=================================================
		/// <summary>
		/// 既存の System.Collections.IEnumerable から Enumerable のインスタンスを作成します。
		/// </summary>
		/// <param name="enumerable">Enumerable の元になるインスタンスを返します。</param>
		private Enumerable(System.Collections.IEnumerable enumerable){
			this.@enum=enumerable;
		}
		/// <summary>
		/// System.Collections.IEnumerable を afh.Collections.Enumerable に変換します。
		/// </summary>
		/// <param name="enumerable">afh.Collections.Enumerable かどうか分からない IEnumerable を指定します。</param>
		/// <returns>指定した System.Collections.IEnumerable が Enumerable の時はその儘返します。
		/// それ以外の場合には Enumerable インスタンスに変換して返します。</returns>
		public static Enumerable From(System.Collections.IEnumerable enumerable){
			return (enumerable as Enumerable)??new Enumerable(enumerable);
		}
		/// <summary>
		/// System.Collections.IEnumerable を <see cref="afh.Collections.Enumerable&lt;T&gt;"/> に変換します。
		/// </summary>
		/// <param name="enumerable"><see cref="afh.Collections.Enumerable&lt;T&gt;"/>
		/// 型かどうか分からない IEnumerable を指定します。
		/// </param>
		/// <returns>指定した System.Collections.IEnumerable が
		/// <see cref="afh.Collections.Enumerable&lt;T&gt;"/> の時はその儘返します。
		/// それ以外の場合には Enumerable インスタンスに変換して返します。</returns>
		public static Enumerable<T> From<T>(Gen::IEnumerable<T> enumerable){
			return Enumerable<T>.From(enumerable);
		}
		//=================================================
		//		列挙操作
		//=================================================
		/// <summary>
		/// 指定したフィルタで受容できると判定されたオブジェクトのみを列挙します。
		/// </summary>
		/// <param name="filter">列挙するか否かを判定する <see cref="afh.Filter"/> を指定します。</param>
		/// <returns>篩に掛けた後のオブジェクトを列挙する Enumerable を返します。</returns>
		public Enumerable Filter(Filter filter){
			return new Enumerable(Filter(this.@enum,filter));
		}
		private static System.Collections.IEnumerable Filter(System.Collections.IEnumerable enumerable,Filter filter){
			foreach(object obj in enumerable)if(filter(obj))yield return obj;
		}
		/// <summary>
		/// 列挙するオブジェクトに変換を掛けます。
		/// </summary>
		/// <param name="conv">変換に使用する <see cref="Converter"/> を指定します。</param>
		/// <returns>変換結果のオブジェクトを列挙する Enumerable を返します。</returns>
		public Enumerable Map(Converter conv){
			return new Enumerable(Map(this.@enum,conv));
		}
		private static System.Collections.IEnumerable Map(System.Collections.IEnumerable enumerable,Converter conv){
			foreach(object obj in enumerable)yield return conv(obj);
		}
		/// <summary>
		/// 列挙されるオブジェクトを配列に格納します。
		/// </summary>
		/// <returns>列挙された内容を格納した配列を返します。</returns>
		public object[] ToArray(){
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(object obj in this.@enum)list.Add(obj);
			return list.ToArray();
		}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の Enumerable を使用して列挙します。次に右辺式の Enumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される Enumerable を指定します。</param>
		/// <param name="r">後に列挙される Enumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable operator +(Enumerable l,Enumerable r){return new Enumerable(op_Add(l.@enum,r.@enum));}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の Enumerable を使用して列挙します。次に右辺式の IEnumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される Enumerable を指定します。</param>
		/// <param name="r">後に列挙される IEnumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable operator +(Enumerable l,System.Collections.IEnumerable r){return new Enumerable(op_Add(l.@enum,r));}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の IEnumerable を使用して列挙します。次に右辺式の Enumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される IEnumerable を指定します。</param>
		/// <param name="r">後に列挙される Enumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable operator +(System.Collections.IEnumerable l,Enumerable r){return new Enumerable(op_Add(l,r.@enum));}
		private static System.Collections.IEnumerable op_Add(System.Collections.IEnumerable l,System.Collections.IEnumerable r){
			foreach(object item in l)yield return item;
			foreach(object item in r)yield return item;
		}
	}
	/// <summary>
	/// <typeparamref name="T"/> のオブジェクトの列挙の方法を提供するクラスです。
	/// </summary>
	/// <typeparam name="T">列挙の対象となるオブジェクトの型を指定します。</typeparam>
	[System.Serializable]
	public class Enumerable<T>:System.Collections.IEnumerable{
		private Gen::IEnumerable<T> @enum;
		/// <summary>
		/// このインスタンスに関連付けられた列挙子を取得します。
		/// </summary>
		/// <returns>列挙子を返します。</returns>
		public Gen::IEnumerator<T> GetEnumerator(){return this.@enum.GetEnumerator();}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return ((System.Collections.IEnumerable)this.@enum).GetEnumerator();
		}
		//=================================================
		//		演算子など
		//=================================================
		/// <summary>
		/// 既存の System.Collections.IEnumerable から Enumerable のインスタンスを作成します。
		/// </summary>
		/// <param name="enumerable">Enumerable の元になるインスタンスを返します。</param>
		private Enumerable(Gen::IEnumerable<T> enumerable){
			this.@enum=enumerable;
		}
		/// <summary>
		/// System.Collections.IEnumerable を afh.Collections.Enumerable に変換します。
		/// </summary>
		/// <param name="enumerable">afh.Collections.Enumerable 型かどうか分からない IEnumerable を指定します。</param>
		/// <returns>指定した System.Collections.IEnumerable が Enumerable の時はその儘返します。
		/// それ以外の場合には Enumerable インスタンスに変換して返します。</returns>
		public static Enumerable<T> From(Gen::IEnumerable<T> enumerable){
			return (enumerable as Enumerable<T>)??new Enumerable<T>(enumerable);
		}
		//=================================================
		//		列挙操作
		//=================================================
		/// <summary>
		/// 指定したフィルタで受容できると判定されたオブジェクトのみを列挙します。
		/// </summary>
		/// <param name="filter">列挙するか否かを判定する <see cref="afh.Filter&lt;T&gt;"/> を指定します。</param>
		/// <returns>篩に掛けた後のオブジェクトを列挙する Enumerable を返します。</returns>
		public Enumerable<T> Filter(Filter<T> filter){
			return new Enumerable<T>(Filter(this.@enum,filter));
		}
		private static Gen::IEnumerable<T> Filter(Gen::IEnumerable<T> enumerable,Filter<T> filter){
			foreach(T obj in enumerable)if(filter(obj))yield return obj;
		}
		/// <summary>
		/// 列挙するオブジェクトに変換を掛けます。
		/// </summary>
		/// <param name="conv">変換に使用する <see cref="Converter"/> を指定します。</param>
		/// <returns>変換結果のオブジェクトを列挙する Enumerable を返します。</returns>
		public Enumerable<U> Map<U>(Converter<T,U> conv){
			return new Enumerable<U>(Map<U>(this.@enum,conv));
		}
		private static Gen::IEnumerable<U> Map<U>(Gen::IEnumerable<T> enumerable,Converter<T,U> conv){
			foreach(T obj in enumerable)yield return conv(obj);
		}
		/// <summary>
		/// 列挙されるオブジェクトを配列に格納します。
		/// </summary>
		/// <returns>列挙された内容を格納した配列を返します。</returns>
		public T[] ToArray(){
			// List ?
			Gen::List<T> list=this.@enum as Gen::List<T>;
			if(list!=null)return list.ToArray();

			// ICollection?
			Gen::ICollection<T> col=this.@enum as Gen::ICollection<T>;
			if(col!=null){
				T[] ret=new T[col.Count];
				col.CopyTo(ret,0);
				return ret;
			}
			
			// IEnumerable
			list=new Gen::List<T>();
			foreach(T obj in this.@enum)list.Add(obj);
			return list.ToArray();
		}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の Enumerable を使用して列挙します。次に右辺式の Enumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される Enumerable を指定します。</param>
		/// <param name="r">後に列挙される Enumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable<T> operator +(Enumerable<T> l,Enumerable<T> r){return new Enumerable<T>(op_Add(l.@enum,r.@enum));}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の Enumerable を使用して列挙します。次に右辺式の IEnumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される Enumerable を指定します。</param>
		/// <param name="r">後に列挙される IEnumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable<T> operator +(Enumerable<T> l,Gen::IEnumerable<T> r){return new Enumerable<T>(op_Add(l.@enum,r));}
		/// <summary>
		/// 列挙子を併合します。
		/// <para>先ず初めに左辺式の IEnumerable を使用して列挙します。次に右辺式の Enumerable を使用して列挙します。
		/// 従ってこれは厳密な併合ではなく、右辺式と左辺式に同じ要素が含まれていた場合その要素は含まれている回数だけ列挙される事になります。</para>
		/// </summary>
		/// <param name="l">先に列挙される IEnumerable を指定します。</param>
		/// <param name="r">後に列挙される Enumerable を指定します。</param>
		/// <returns>左辺と右辺の内容を列挙する Enumerable を返します。</returns>
		public static Enumerable<T> operator +(Gen::IEnumerable<T> l,Enumerable<T> r){return new Enumerable<T>(op_Add(l,r.@enum));}
		private static Gen::IEnumerable<T> op_Add(Gen::IEnumerable<T> l,Gen::IEnumerable<T> r){
			foreach(T item in l)yield return item;
			foreach(T item in r)yield return item;
		}
	}
}
namespace afh{
	/// <summary>
	/// 指定したオブジェクトが受容可能かどうかを判定します。
	/// </summary>
	/// <param name="obj">判定の対象となるオブジェクトを指定します。</param>
	/// <returns>受容可能と判定された場合に true を返します。それ以外の場合には false を返します。</returns>
	[System.Serializable]
	public delegate bool Filter(object obj);
	/// <summary>
	/// 指定したオブジェクトが受容可能かどうかを判定します。
	/// </summary>
	/// <typeparam name="T">判定の対象となるオブジェクトの型を指定します。</typeparam>
	/// <param name="obj">判定の対象となるオブジェクトを指定します。</param>
	/// <returns>受容可能と判定された場合に true を返します。それ以外の場合には false を返します。</returns>
	[System.Serializable]
	public delegate bool Filter<T>(T obj);
	/// <summary>
	/// 指定したオブジェクトを別のオブジェクトに変換します。
	/// </summary>
	/// <param name="obj">変換前のオブジェクトを指定します。</param>
	/// <returns>変換後のオブジェクトを返します。</returns>
	[System.Serializable]
	public delegate object Converter(object obj);
	/// <summary>
	/// 指定したオブジェクトを別のオブジェクトに変換します。
	/// </summary>
	/// <typeparam name="T">オブジェクトの変換前の型を指定します。</typeparam>
	/// <typeparam name="U">オブジェクトの変換後の型を指定します。</typeparam>
	/// <param name="obj">変換前のオブジェクトを指定します。</param>
	/// <returns>変換後のオブジェクトを返します。</returns>
	[System.Serializable]
	public delegate U Converter<T,U>(T obj);
}