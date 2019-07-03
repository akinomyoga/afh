using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Rgx=System.Text.RegularExpressions;

namespace afh{
	/// <summary>
	/// �^�̎�ނ���肷��ׂ̃R�[�h��\�����܂��B
	/// </summary>
	public enum TypeCodes{
		/// <summary>
		/// ���o�^�̌^�ł��鎖�������܂��B
		/// </summary>
		Unknown=-1,
		//===========================================================
		//		C# Natives
		//===========================================================
		/// <summary>
		/// System.Void ��\�����܂��B
		/// </summary>
		Void,
		/// <summary>
		/// System.Byte ��\�����܂��B
		/// </summary>
		Byte,
		/// <summary>
		/// System.SByte ��\�����܂��B
		/// </summary>
		SByte,
		/// <summary>
		/// System.Int16 ��\�����܂��B
		/// </summary>
		Short,
		/// <summary>
		/// System.UInt16 ��\�����܂��B
		/// </summary>
		UShort,
		/// <summary>
		/// System.Int32 ��\�����܂��B
		/// </summary>
		Int,
		/// <summary>
		/// System.UInt32 ��\�����܂��B
		/// </summary>
		UInt,
		/// <summary>
		/// System.Int64 ��\�����܂��B
		/// </summary>
		Long,
		/// <summary>
		/// System.UInt64 ��\�����܂��B
		/// </summary>
		ULong,
		/// <summary>
		/// System.Single ��\�����܂��B
		/// </summary>
		Float,
		/// <summary>
		/// System.Double ��\�����܂��B
		/// </summary>
		Double,
		/// <summary>
		/// System.Char ��\�����܂��B
		/// </summary>
		Char,
		/// <summary>
		/// System.Boolean ��\�����܂��B
		/// </summary>
		Bool,
		/// <summary>
		/// System.Decimal ��\�����܂��B
		/// </summary>
		Decimal,
		/// <summary>
		/// System.String ��\�����܂��B
		/// </summary>
		String,
		/// <summary>
		/// System.Object ��\�����܂��B
		/// </summary>
		Object,
		//===========================================================
		//		�悭�g����
		//===========================================================
		/// <summary>
		/// System.Type ��\�����܂��B
		/// </summary>
		Type,
		/// <summary>
		/// System.IntPtr ��\�����܂��B
		/// </summary>
		IntPtr,
		/// <summary>
		/// System.UIntPtr ��\�����܂��B
		/// </summary>
		UIntPtr,
		/// <summary>
		/// System.DateTime ��\�����܂��B
		/// </summary>
		DateTime,
		/// <summary>
		/// System.TimeSpan ��\�����܂��B
		/// </summary>
		TimeSpan,

		/// <summary>
		/// System.Byte[] ��\�����܂��B
		/// </summary>
		ByteArray,
		/// <summary>
		/// System.Bool[] ��\�����܂��B
		/// </summary>
		BoolArray,
		/// <summary>
		/// System.Int32[] ��\�����܂��B
		/// </summary>
		IntArray,

		/// <summary>
		/// System.Guid ��\�����܂��B
		/// </summary>
		Guid,
		/// <summary>
		/// System.Version ��\�����܂��B
		/// </summary>
		Version,
	}

	/// <summary>
	/// System.Type �̕�����\���ɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public static class Types{
		static Types(){
			type_dic[typeof(void)]		=TypeCodes.Void;
			type_dic[typeof(sbyte)]		=TypeCodes.SByte;
			type_dic[typeof(byte)]		=TypeCodes.Byte;
			type_dic[typeof(short)]		=TypeCodes.Short;
			type_dic[typeof(ushort)]	=TypeCodes.UShort;
			type_dic[typeof(int)]		=TypeCodes.Int;
			type_dic[typeof(uint)]		=TypeCodes.UInt;
			type_dic[typeof(long)]		=TypeCodes.Long;
			type_dic[typeof(ulong)]		=TypeCodes.ULong;
			type_dic[typeof(float)]		=TypeCodes.Float;
			type_dic[typeof(double)]	=TypeCodes.Double;
			type_dic[typeof(char)]		=TypeCodes.Char;
			type_dic[typeof(bool)]		=TypeCodes.Bool;
			type_dic[typeof(decimal)]	=TypeCodes.Decimal;
			type_dic[typeof(string)]	=TypeCodes.String;
			type_dic[typeof(object)]	=TypeCodes.Object;
			type_dic[typeof(System.IntPtr)]	=TypeCodes.IntPtr;
			type_dic[typeof(System.UIntPtr)]=TypeCodes.UIntPtr;
			type_dic[typeof(byte[])]		=TypeCodes.ByteArray;
			type_dic[typeof(bool[])]		=TypeCodes.BoolArray;
			type_dic[typeof(int[])]			=TypeCodes.IntArray;
		}
		//===========================================================
		//		CSharpName
		//===========================================================
		/// <summary>
		/// ���\�b�h�̃V�O�j�`����������镶����� C# �`���Ŏ擾���܂��B
		/// </summary>
		/// <param name="m">���\�b�h���w�肵�܂��B</param>
		/// <returns>���\�b�h�� C# �`���ŕ\�������������Ԃ��܂��B</returns>
		public static string CSharpName(Ref::MethodBase m){
			string ret=CSharpName(m.DeclaringType)+"."+m.Name;
			if(m.IsGenericMethod){
				System.Type[] args=m.GetGenericArguments();
				string gen=CSharpName(args[0]);
				for(int i=1;i<args.Length;i++)
					gen+=","+CSharpName(args[i]);
				return ret+"<"+gen+">";
			}else return ret;
		}
		/// <summary>
		/// System.Type ��\��������� C# �`���Ŏ擾���܂��B
		/// </summary>
		/// <param name="t">������ɒ����O�� System.Type ���w�肵�܂��B</param>
		/// <returns>System.Type �𕶎���ŕ\���������擾���܂��B</returns>
		public static string CSharpName(System.Type t){
			if(t.IsArray)return CSharpName(t.GetElementType())+"["+new string(',',t.GetArrayRank()-1)+"]";
			if(t.IsPointer)return CSharpName(t.GetElementType())+"*";
			if(t.IsByRef)return CSharpName(t.GetElementType())+"&";
			if(t.IsGenericType){
				if(t.GetGenericTypeDefinition()==typeof(System.Nullable<>))
					return CSharpName(t.GetGenericArguments()[0])+"?";
				string temp=CSharpName_gen(t);
				System.Type[] args=t.GetGenericArguments();
				int i=0;
				return CSharpName_rxGen.Replace(temp,delegate(Rgx::Match m){
					int M=int.Parse(m.Groups[1].Value);
					if(M==0||i+M>args.Length)return m.Value;

					string ret=CSharpName(args[i++]);
					for(int j=1;j<M;j++){
						ret+=","+CSharpName(args[i++]);
					}
					return "<"+ret+">";
				});
			}
			if(t.IsGenericParameter)return t.Name;

			switch(GetTypeCode(t)){
				case TypeCodes.Void:return "void";
				case TypeCodes.SByte:return "sbyte";
				case TypeCodes.Byte:return "byte";
				case TypeCodes.Short:return "short";
				case TypeCodes.UShort:return "ushort";
				case TypeCodes.Int:return "int";
				case TypeCodes.UInt:return "uint";
				case TypeCodes.Long:return "long";
				case TypeCodes.ULong:return "ulong";
				case TypeCodes.Float:return "float";
				case TypeCodes.Double:return "double";
				case TypeCodes.Decimal:return "decimal";
				case TypeCodes.Char:return "char";
				case TypeCodes.Bool:return "bool";
				case TypeCodes.String:return "string";
				case TypeCodes.Object:return "object";
				default:return t.FullName;
			}
		}
		private static readonly Rgx::Regex CSharpName_rxGen=new Rgx::Regex(@"\`(\d+)",Rgx::RegexOptions.Compiled);
		private static string CSharpName_gen(System.Type t){
			if(!t.IsNested)return t.Namespace+"."+t.Name;
			return CSharpName_gen(t.DeclaringType)+"+"+t.Name;
		}
		/// <summary>
		/// �p�����[�^�̃��X�g�𕶎���ɂ��Ď擾���܂��B
		/// </summary>
		/// <param name="m">���\�b�h��\�� System.Reflection.MethodBase ���w�肵�ĉ������B</param>
		/// <returns>�p�����[�^�̃��X�g��\���������Ԃ��܂��B</returns>
		public static string GetParameterList(System.Reflection.MethodBase m){
			System.Reflection.ParameterInfo[] ps=m.GetParameters();
			System.Text.StringBuilder build=new System.Text.StringBuilder();
			for(int i=0,iM=ps.Length;i<iM;i++){
				if(i!=0)build.Append(',');

				System.Type t=ps[i].ParameterType;
				if(t.IsByRef){
					build.AppendFormat("{0} {1}",ps[i].IsOut?"out":"ref",CSharpName(t.GetElementType()));
				}else{
					if(ps[i].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
						build.Append("params ");
					}
					build.Append(CSharpName(t));
				}
				build.Append(' ');
				build.Append(ps[i].Name);
			}
			return build.ToString();
		}
		/// <summary>
		/// ���\�b�h�̖��O�y�уV�O�j�`�����𕶎���Ƃ��Ď擾���܂��B
		/// </summary>
		/// <param name="meth">���𕶎���ɂ��郁�\�b�h���w�肵�܂��B</param>
		/// <returns>���\�b�h�̏��𕶎���ɂ��ĕԂ��܂��B</returns>
		public static string GetMethodSignature(Ref::MethodBase meth){
			//string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
			string ret="";
			
			if(meth.IsStatic)ret+="static ";

			Ref::MethodInfo minfo=meth as Ref::MethodInfo;
			if(minfo!=null)ret+=CSharpName(minfo.ReturnType)+" ";
			
			//"<"+dll+"> "+
			return ret+CSharpName(meth)+"("+Types.GetParameterList(meth)+");";
		}
		//===========================================================
		//		Type ����
		//===========================================================
		/// <summary>
		/// System.Type �� TypeCodes �Ƃ̑Ή���t����ׂɎg�p���܂��B
		/// </summary>
		public static readonly Gen::Dictionary<System.Type,TypeCodes> type_dic=new Gen::Dictionary<System.Type,TypeCodes>(20);
		/// <summary>
		/// System.Type �� type_dic �ŗ��p�ł���悤�ɓo�^���܂��B
		/// </summary>
		/// <param name="type">�o�^����^���w�肵�܂��B</param>
		/// <returns>�V�����o�^���� TypeCodes ��Ԃ��܂��B
		/// ���ɓo�^����Ă����ꍇ�ɂ́A�o�^����Ă��� TypeCodes ��Ԃ��܂��B</returns>
		public static TypeCodes RegisterTypeCode(System.Type type){
			TypeCodes code;
			if(type_dic.TryGetValue(type,out code)){
				return code;
			}else{
				code=(TypeCodes)type_dic.Count;
				type_dic.Add(type,code);
				return code;
			}
		}
		/// <summary>
		/// System.Type ���� TypeCodes ���擾���܂��B
		/// </summary>
		/// <param name="type">�Ή����� TypeCodes ���擾�������^���w�肵�܂��B</param>
		/// <returns>�w�肵���^�Ɋ֘A�t�����ēo�^����Ă��� TypeCodes ��Ԃ��܂��B
		/// �w�肵���^���o�^����Ă��Ȃ��ꍇ�ɂ� TypeCodes.Unknown ��Ԃ��܂��B</returns>
		public static TypeCodes GetTypeCode(System.Type type){
			TypeCodes code;
			if(type_dic.TryGetValue(type,out code)){
				return code;
			}else return TypeCodes.Unknown;
		}
	}
}