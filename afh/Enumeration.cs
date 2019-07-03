using Gen=System.Collections.Generic;
namespace afh{
	/// <summary>
	/// <see cref="System.Enum"/> �Ɋւ��鑀���񋟂��܂��B
	/// </summary>
	public static class Enum{
		#region GetDescription
		private static Gen::Dictionary<TypeNamePair,string> descriptions=new Gen::Dictionary<TypeNamePair,string>();
		/// <summary>
		/// �w�肵�� System.Enum �l������ɑΉ����镶����ɕϊ����܂��B
		/// ��̓I�ɂ� afh.EnumDescriptionAttribute ��p���Ċ֘A�t����ꂽ��������������܂��B
		/// Key �Ƃ��Ă͊���� Key, "" ���g�p���܂��B
		/// </summary>
		/// <param name="enumeration">�񋓌^�̒l���w�肵�܂��B</param>
		/// <returns>�擾�����������Ԃ��܂��B</returns>
		public static string GetDescription(System.Enum enumeration){
			return GetDescription(new TypeNamePair(enumeration,""));
		}
		/// <summary>
		/// �w�肵�� System.Enum �l������ɑΉ����镶����ɕϊ����܂��B
		/// ��̓I�ɂ� afh.EnumDescriptionAttribute ��p���Ċ֘A�t����ꂽ��������������܂��B
		/// Key �Ƃ��Ă͊���� Key, "" ���g�p���܂��B
		/// </summary>
		/// <param name="finfo">�񋓌^�̐ÓI field �̏����w�肵�܂��B</param>
		/// <returns>�擾�����������Ԃ��܂��B</returns>
		public static string GetDescription(System.Reflection.FieldInfo finfo){
			return GetDescription(new TypeNamePair(finfo,""));
		}
		/// <summary>
		/// �w�肵�� System.Enum �l������ɑΉ����镶����ɕϊ����܂��B
		/// ��̓I�ɂ� afh.EnumDescriptionAttribute ��p���Ċ֘A�t����ꂽ��������������܂��B
		/// </summary>
		/// <param name="enumeration">�񋓌^�̒l���w�肵�܂��B</param>
		/// <param name="key">
		/// �����̍ۂɎg�p���� key ��������w�肵�܂��B
		/// ��̓I�ɂ� <see cref="afh.EnumDescriptionAttribute.Key"/> ���Q�Ƃ��ĉ������B
		/// </param>
		/// <returns>�擾�����������Ԃ��܂��B</returns>
		public static string GetDescription(System.Enum enumeration,string key){
			return GetDescription(new TypeNamePair(enumeration,key));
		}
		/// <summary>
		/// �w�肵�� System.Enum �l������ɑΉ����镶����ɕϊ����܂��B
		/// ��̓I�ɂ� afh.EnumDescriptionAttribute ��p���Ċ֘A�t����ꂽ��������������܂��B
		/// </summary>
		/// <param name="finfo">�񋓌^�̐ÓI field �̏����w�肵�܂��B</param>
		/// <param name="key">
		/// �����̍ۂɎg�p���� key ��������w�肵�܂��B
		/// ��̓I�ɂ� <see cref="afh.EnumDescriptionAttribute.Key"/> ���Q�Ƃ��ĉ������B
		/// </param>
		/// <returns>�擾�����������Ԃ��܂��B</returns>
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
	/// �񋓑̂̃����o�Ɋւ�������Ȃǂ̕�������L�q���܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field,AllowMultiple=true)]
	public class EnumDescriptionAttribute:System.Attribute{
		private string description;
		/// <summary>
		/// �֘A�t����ꂽ�������擾���܂��B
		/// </summary>
		public string Description{
			get{return this.description;}
		}
		private string key="";
		/// <summary>
		/// �����Ɋ֘A�t����ꂽ�����擾���͐ݒ肵�܂��B
		/// �ݒ�͏������̍ۂɎg�p���܂��B
		/// Key ���g�p���鎖�ɂ���ď󋵈ˑ��̐����E�قȂ��ނ̐�����ێ������鎖���o���܂��B
		/// </summary>
		public string Key{
			get{return this.key;}
			set{this.key=value;}
		}
		/// <summary>
		/// EnumDescriotionAttribute ���w�肵����������g�p���ď��������܂��B
		/// </summary>
		public EnumDescriptionAttribute(string desc){
			this.description=desc;
		}
	}
}