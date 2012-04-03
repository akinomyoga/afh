#if NET3_5
#define HAS_LINQ
#elif NET4_0
#define HAS_LINQ
#endif

using Gen=System.Collections.Generic;

#if NET2_0
namespace System.Runtime.CompilerServices{
	/// <summary>
	/// 拡張メソッドである事を示す為の属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly|AttributeTargets.Class|AttributeTargets.Method)]
	public sealed class ExtensionAttribute:Attribute{}
}
#endif

namespace afh.Collections.Utils{
	/// <summary>
	/// コレクションに対する操作を提供します。
	/// </summary>
	public static class Utility{
		/// <summary>
		/// 同じ内容を持つスタックのインスタンスを作成します。
		/// </summary>
		/// <typeparam name="T">スタックの要素の型を指定します。</typeparam>
		/// <param name="stack">コピー元のスタックを指定します。</param>
		/// <returns>指定したスタックと同じ内容を持つ、新しいスタックを返します。</returns>
		public static Gen::Stack<T> Clone<T>(this Gen::Stack<T> stack){
			T[] values=stack.ToArray();

			Gen::Stack<T> ret=new Gen::Stack<T>(values.Length);
			for(int i=values.Length-1;i>=0;i--)
				ret.Push(values[i]);
			return ret;
		}
		/// <summary>
		/// スタックの各要素を変換した新しいスタックを作成します。
		/// </summary>
		/// <typeparam name="T">変換前の要素の型を指定します。</typeparam>
		/// <typeparam name="U">変換後の要素の型を指定します。</typeparam>
		/// <param name="stack">変換前の要素を格納したスタックを指定します。</param>
		/// <param name="converter">要素の変換に使用するデリゲートを指定します。</param>
		/// <returns>要素を変換して出来たスタックを返します。</returns>
		public static Gen::Stack<U> ConvertAll<T,U>(this Gen::Stack<T> stack,Converter<T,U> converter){
			T[] values=stack.ToArray();

			Gen::Stack<U> ret=new Gen::Stack<U>(values.Length);
			for(int i=values.Length-1;i>=0;i--)
				ret.Push(converter(values[i]));
			return ret;
		}
#if !HAS_LINQ
		/// <summary>
		/// 列挙子から一番最初の要素を取得します。
		/// </summary>
		/// <typeparam name="T">列挙されるオブジェクトの型を指定します。</typeparam>
		/// <param name="enumerable">列挙子を指定します。</param>
		/// <returns>一番最初の要素を返します。もし、要素が一つも含まれていない場合には、型 T の既定値を返します。</returns>
		public static T First<T>(this Gen::IEnumerable<T> enumerable){
			T ret=default(T);
			foreach(T value in enumerable){ret=value;break;}
			return ret;
		}
#endif
	}
}