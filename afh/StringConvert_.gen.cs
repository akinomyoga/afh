/*
	このソースコードは [afh.Design.dll] afh.Design.TemplateProcessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Design.dll] afh.Design.TemplateProcessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
namespace afh{
	public static partial class StringConvert{
		/*

		*/

		/// <summary>
		/// 文字列から System.TimeSpan 型に変換します。
		/// </summary>
		/// <param name="value">System.TimeSpan 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の System.TimeSpan 型の値を返します。</returns>
		public static System.TimeSpan ToTimeSpan(string value){
			return System.Xml.XmlConvert.ToTimeSpan(value) ;
		}
		/// <summary>
		/// System.TimeSpan 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい System.TimeSpan 型 の値を指定します。</param>
		/// <returns>文字列で表現した System.TimeSpan 型値の情報を返します。</returns>
		public static string FromTimeSpan(System.TimeSpan value){
			return System.Xml.XmlConvert.ToString(value);
		}

		/// <summary>
		/// 文字列から bool[] 型に変換します。
		/// </summary>
		/// <param name="value">bool[] 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の bool[] 型の値を返します。</returns>
		public static bool[] ToBooleanArray(string value){
			char[] c=value.ToCharArray();
			int l=c.Length;
			bool[] r=new bool[l];
			for(int i=0;i<l;i++)r[i]=c[i]=='1';
			return r;
		}
		/// <summary>
		/// bool[] 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい bool[] 型 の値を指定します。</param>
		/// <returns>文字列で表現した bool[] 型値の情報を返します。</returns>
		public static string FromBooleanArray(bool[] value){
			char[] chrs=new char[value.Length];
			for(int i=0;i<value.Length;i++)
				chrs[i]=value[i]?'1':'0';
			return new string(chrs);
		}

		/// <summary>
		/// 文字列から byte[] 型に変換します。
		/// </summary>
		/// <param name="value">byte[] 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の byte[] 型の値を返します。</returns>
		public static byte[] ToByteArray(string value) {
			return System.Convert.FromBase64String(value);
		}
		/// <summary>
		/// byte[] 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい byte[] 型 の値を指定します。</param>
		/// <returns>文字列で表現した byte[] 型値の情報を返します。</returns>
		public static string FromByteArray(byte[] value) {
			return System.Convert.ToBase64String(value);
		}

		/// <summary>
		/// 文字列から byte[] 型に変換します。
		/// </summary>
		/// <param name="value">byte[] 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の byte[] 型の値を返します。</returns>
		public static System.Guid ToGuid(string value){
			return System.Xml.XmlConvert.ToGuid(value);
		}
		/// <summary>
		/// byte[] 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい byte[] 型 の値を指定します。</param>
		/// <returns>文字列で表現した byte[] 型値の情報を返します。</returns>
		public static string FromGuid(System.Guid value){
			return System.Xml.XmlConvert.ToString(value);
		}

		//#define ToName(x)	To##x
		//#define FromName(x)	From##x
		/*

		*/

		/// <summary>
		/// 文字列から short 型に変換します。
		/// </summary>
		/// <param name="value">short 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の short 型の値を返します。</returns>
		public static short ToInt16(string value){return System.Convert.ToInt16(value);}
		/// <summary>
		/// short 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい short 型 の値を指定します。</param>
		/// <returns>文字列で表現した short 型値の情報を返します。</returns>
		public static string FromInt16(short value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から int 型に変換します。
		/// </summary>
		/// <param name="value">int 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の int 型の値を返します。</returns>
		public static int ToInt32(string value){return System.Convert.ToInt32(value);}
		/// <summary>
		/// int 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい int 型 の値を指定します。</param>
		/// <returns>文字列で表現した int 型値の情報を返します。</returns>
		public static string FromInt32(int value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から long 型に変換します。
		/// </summary>
		/// <param name="value">long 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の long 型の値を返します。</returns>
		public static long ToInt64(string value){return System.Convert.ToInt64(value);}
		/// <summary>
		/// long 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい long 型 の値を指定します。</param>
		/// <returns>文字列で表現した long 型値の情報を返します。</returns>
		public static string FromInt64(long value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から ushort 型に変換します。
		/// </summary>
		/// <param name="value">ushort 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の ushort 型の値を返します。</returns>
		public static ushort ToUInt16(string value){return System.Convert.ToUInt16(value);}
		/// <summary>
		/// ushort 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい ushort 型 の値を指定します。</param>
		/// <returns>文字列で表現した ushort 型値の情報を返します。</returns>
		public static string FromUInt16(ushort value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から uint 型に変換します。
		/// </summary>
		/// <param name="value">uint 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の uint 型の値を返します。</returns>
		public static uint ToUInt32(string value){return System.Convert.ToUInt32(value);}
		/// <summary>
		/// uint 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい uint 型 の値を指定します。</param>
		/// <returns>文字列で表現した uint 型値の情報を返します。</returns>
		public static string FromUInt32(uint value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から ulong 型に変換します。
		/// </summary>
		/// <param name="value">ulong 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の ulong 型の値を返します。</returns>
		public static ulong ToUInt64(string value){return System.Convert.ToUInt64(value);}
		/// <summary>
		/// ulong 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい ulong 型 の値を指定します。</param>
		/// <returns>文字列で表現した ulong 型値の情報を返します。</returns>
		public static string FromUInt64(ulong value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から float 型に変換します。
		/// </summary>
		/// <param name="value">float 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の float 型の値を返します。</returns>
		public static float ToSingle(string value){return System.Convert.ToSingle(value);}
		/// <summary>
		/// float 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい float 型 の値を指定します。</param>
		/// <returns>文字列で表現した float 型値の情報を返します。</returns>
		public static string FromSingle(float value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から double 型に変換します。
		/// </summary>
		/// <param name="value">double 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の double 型の値を返します。</returns>
		public static double ToDouble(string value){return System.Convert.ToDouble(value);}
		/// <summary>
		/// double 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい double 型 の値を指定します。</param>
		/// <returns>文字列で表現した double 型値の情報を返します。</returns>
		public static string FromDouble(double value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から decimal 型に変換します。
		/// </summary>
		/// <param name="value">decimal 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の decimal 型の値を返します。</returns>
		public static decimal ToDecimal(string value){return System.Convert.ToDecimal(value);}
		/// <summary>
		/// decimal 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい decimal 型 の値を指定します。</param>
		/// <returns>文字列で表現した decimal 型値の情報を返します。</returns>
		public static string FromDecimal(decimal value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から sbyte 型に変換します。
		/// </summary>
		/// <param name="value">sbyte 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の sbyte 型の値を返します。</returns>
		public static sbyte ToSByte(string value){return System.Convert.ToSByte(value);}
		/// <summary>
		/// sbyte 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい sbyte 型 の値を指定します。</param>
		/// <returns>文字列で表現した sbyte 型値の情報を返します。</returns>
		public static string FromSByte(sbyte value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から byte 型に変換します。
		/// </summary>
		/// <param name="value">byte 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の byte 型の値を返します。</returns>
		public static byte ToByte(string value){return System.Convert.ToByte(value);}
		/// <summary>
		/// byte 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい byte 型 の値を指定します。</param>
		/// <returns>文字列で表現した byte 型値の情報を返します。</returns>
		public static string FromByte(byte value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から char 型に変換します。
		/// </summary>
		/// <param name="value">char 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の char 型の値を返します。</returns>
		public static char ToChar(string value){return System.Convert.ToChar(value);}
		/// <summary>
		/// char 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい char 型 の値を指定します。</param>
		/// <returns>文字列で表現した char 型値の情報を返します。</returns>
		public static string FromChar(char value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から bool 型に変換します。
		/// </summary>
		/// <param name="value">bool 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の bool 型の値を返します。</returns>
		public static bool ToBoolean(string value){return System.Convert.ToBoolean(value);}
		/// <summary>
		/// bool 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい bool 型 の値を指定します。</param>
		/// <returns>文字列で表現した bool 型値の情報を返します。</returns>
		public static string FromBoolean(bool value){return System.Convert.ToString(value);}
		/// <summary>
		/// 文字列から System.DateTime 型に変換します。
		/// </summary>
		/// <param name="value">System.DateTime 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の System.DateTime 型の値を返します。</returns>
		public static System.DateTime ToDateTime(string value){return System.Convert.ToDateTime(value);}
		/// <summary>
		/// System.DateTime 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい System.DateTime 型 の値を指定します。</param>
		/// <returns>文字列で表現した System.DateTime 型値の情報を返します。</returns>
		public static string FromDateTime(System.DateTime value){return System.Convert.ToString(value);}
	}
}