namespace afh{
	public static partial class StringConvert{
		/*
		//#→template xmldoc_to<type>
		/// <summary>
		/// 文字列から type 型に変換します。
		/// </summary>
		/// <param name="value">type 型の値を表現する文字列を指定します。</param>
		/// <returns>変換後の type 型の値を返します。</returns>
		//#←template
		//#→template xmldoc_from<type>
		/// <summary>
		/// type 型の値を文字列で表現します。
		/// </summary>
		/// <param name="value">文字列に変換したい type 型 の値を指定します。</param>
		/// <returns>文字列で表現した type 型値の情報を返します。</returns>
		//#←template
		*/

		//#xmldoc_to<System.TimeSpan>
		public static System.TimeSpan ToTimeSpan(string value){
			return System.Xml.XmlConvert.ToTimeSpan(value) ;
		}
		//#xmldoc_from<System.TimeSpan>
		public static string FromTimeSpan(System.TimeSpan value){
			return System.Xml.XmlConvert.ToString(value);
		}

		//#xmldoc_to<bool[]>
		public static bool[] ToBooleanArray(string value){
			char[] c=value.ToCharArray();
			int l=c.Length;
			bool[] r=new bool[l];
			for(int i=0;i<l;i++)r[i]=c[i]=='1';
			return r;
		}
		//#xmldoc_from<bool[]>
		public static string FromBooleanArray(bool[] value){
			char[] chrs=new char[value.Length];
			for(int i=0;i<value.Length;i++)
				chrs[i]=value[i]?'1':'0';
			return new string(chrs);
		}

		//#xmldoc_to<byte[]>
		public static byte[] ToByteArray(string value) {
			return System.Convert.FromBase64String(value);
		}
		//#xmldoc_from<byte[]>
		public static string FromByteArray(byte[] value) {
			return System.Convert.ToBase64String(value);
		}

		//#xmldoc_to<byte[]>
		public static System.Guid ToGuid(string value){
			return System.Xml.XmlConvert.ToGuid(value);
		}
		//#xmldoc_from<byte[]>
		public static string FromGuid(System.Guid value){
			return System.Xml.XmlConvert.ToString(value);
		}

		//#define ToName(x)	To##x
		//#define FromName(x)	From##x
		/*
		//#→template SysConv<type,name>
		//#xmldoc_to<type>
		public static type ToName(name)(string value){return System.Convert.ToName(name)(value);}
		//#xmldoc_from<type>
		public static string FromName(name)(type value){return System.Convert.ToString(value);}
		//#←template
		*/

		//#SysConv<short,Int16>
		//#SysConv<int,Int32>
		//#SysConv<long,Int64>
		//#SysConv<ushort,UInt16>
		//#SysConv<uint,UInt32>
		//#SysConv<ulong,UInt64>
		//#SysConv<float,Single>
		//#SysConv<double,Double>
		//#SysConv<decimal,Decimal>
		//#SysConv<sbyte,SByte>
		//#SysConv<byte,Byte>
		//#SysConv<char,Char>
		//#SysConv<bool,Boolean>
		//#SysConv<System.DateTime,DateTime>
	}
}