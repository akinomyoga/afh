using Gen=System.Collections.Generic;
namespace afh{
	/// <summary>
	/// <see cref="System.Enum"/> に関する操作を提供します。
	/// </summary>
	public static class Enum{
		#region GetDescription
		private static Gen::Dictionary<TypeNamePair,string> descriptions=new Gen::Dictionary<TypeNamePair,string>();
		/// <summary>
		/// 指定した System.Enum 値をそれに対応する文字列に変換します。
		/// 具体的には afh.EnumDescriptionAttribute を用いて関連付けられた文字列を検索します。
		/// Key としては既定の Key, "" を使用します。
		/// </summary>
		/// <param name="enumeration">列挙型の値を指定します。</param>
		/// <returns>取得した文字列を返します。</returns>
		public static string GetDescription(System.Enum enumeration){
			return GetDescription(new TypeNamePair(enumeration,""));
		}
		/// <summary>
		/// 指定した System.Enum 値をそれに対応する文字列に変換します。
		/// 具体的には afh.EnumDescriptionAttribute を用いて関連付けられた文字列を検索します。
		/// Key としては既定の Key, "" を使用します。
		/// </summary>
		/// <param name="finfo">列挙型の静的 field の情報を指定します。</param>
		/// <returns>取得した文字列を返します。</returns>
		public static string GetDescription(System.Reflection.FieldInfo finfo){
			return GetDescription(new TypeNamePair(finfo,""));
		}
		/// <summary>
		/// 指定した System.Enum 値をそれに対応する文字列に変換します。
		/// 具体的には afh.EnumDescriptionAttribute を用いて関連付けられた文字列を検索します。
		/// </summary>
		/// <param name="enumeration">列挙型の値を指定します。</param>
		/// <param name="key">
		/// 検索の際に使用する key 文字列を指定します。
		/// 具体的には <see cref="afh.EnumDescriptionAttribute.Key"/> を参照して下さい。
		/// </param>
		/// <returns>取得した文字列を返します。</returns>
		public static string GetDescription(System.Enum enumeration,string key){
			return GetDescription(new TypeNamePair(enumeration,key));
		}
		/// <summary>
		/// 指定した System.Enum 値をそれに対応する文字列に変換します。
		/// 具体的には afh.EnumDescriptionAttribute を用いて関連付けられた文字列を検索します。
		/// </summary>
		/// <param name="finfo">列挙型の静的 field の情報を指定します。</param>
		/// <param name="key">
		/// 検索の際に使用する key 文字列を指定します。
		/// 具体的には <see cref="afh.EnumDescriptionAttribute.Key"/> を参照して下さい。
		/// </param>
		/// <returns>取得した文字列を返します。</returns>
		public static string GetDescription(System.Reflection.FieldInfo finfo,string key){
			return GetDescription(new TypeNamePair(finfo,key));
		}
		private static string GetDescription(TypeNamePair pair){
			if(descriptions.ContainsKey(pair)){
				return descriptions[pair];
			}else{
				return descriptions[pair]=GetNewDescription(pair);
			}
		}
		private static string GetNewDescription(TypeNamePair pair){
			System.Reflection.FieldInfo finfo=pair.type.GetField(pair.fieldname);
			if(finfo==null)return pair.fieldname;

			afh.EnumDescriptionAttribute[] attrs
				=(afh.EnumDescriptionAttribute[])finfo.GetCustomAttributes(typeof(afh.EnumDescriptionAttribute),false);
			for(int i=0;i<attrs.Length;i++){
				if(attrs[i].Key==pair.key)return attrs[i].Description;
			}
			return pair.fieldname;
		}
		private struct TypeNamePair{
			public System.Type type;
			public string fieldname;
			public string key;
			public TypeNamePair(System.Reflection.FieldInfo finfo,string key){
				this.type=finfo.DeclaringType;
				this.fieldname=finfo.Name;
				this.key=key;
			}
			public TypeNamePair(System.Enum enumeration,string key){
				this.type=enumeration.GetType();
				this.fieldname=enumeration.ToString();
				this.key=key;
			}
			public TypeNamePair(System.Type type,string fieldname,string key){
				this.type=type;
				this.fieldname=fieldname;
				this.key=key;
			}
			public override bool Equals(object obj) {
				if(!(obj is TypeNamePair))return false;
				TypeNamePair pair=(TypeNamePair)obj;
				return this.type==pair.type&&this.fieldname==pair.fieldname&&this.key==pair.key;
			}
			public override int GetHashCode(){
				return this.type.GetHashCode()^this.fieldname.GetHashCode()^this.key.GetHashCode();
			}
		}
		#endregion
	}

	/// <summary>
	/// 列挙体のメンバに関する説明などの文字列を記述します。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field,AllowMultiple=true)]
	public class EnumDescriptionAttribute:System.Attribute{
		private string description;
		/// <summary>
		/// 関連付けられた説明を取得します。
		/// </summary>
		public string Description{
			get{return this.description;}
		}
		private string key="";
		/// <summary>
		/// 説明に関連付けられた鍵を取得亦は設定します。
		/// 設定は初期化の際に使用します。
		/// Key を使用する事によって状況依存の説明・異なる種類の説明を保持させる事が出来ます。
		/// </summary>
		public string Key{
			get{return this.key;}
			set{this.key=value;}
		}
		/// <summary>
		/// EnumDescriotionAttribute を指定した文字列を使用して初期化します。
		/// </summary>
		public EnumDescriptionAttribute(string desc){
			this.description=desc;
		}
	}
}