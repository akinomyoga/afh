namespace afh{
	/// <summary>
	/// ������Ƃ̉t�ȑ��ݕϊ���񋟂��܂��B
	/// </summary>
	/// <remarks>
	/// �ϊ�����镨��...
	/// 1. ��{�^
	/// 2. System.ComponentModel.TypeConverterAttribute ���w�肵�Ă��镨
	/// 3. implicit operator T(string) / implicit operator string(T) ���������Ă��镨
	/// 3. System.SerializableAttribute ���w�肵�Ă��镨
	/// </remarks>
	public static partial class StringConvert {
		private const string FROMSTR_NOTSUPPORTEDTYPE="<afh.dll> afh.Convert.FromString: �w�肵���^ {0} ����̕ϊ��ɂ͑Ή����Ă��܂���";
		private const string TOSTR_NOTSUPPORTEDTYPE="<afh.dll> afh.Convert.ToString: �w�肵���^ {0} �ւ̕ϊ��ɂ͑Ή����Ă��܂���";
		private const System.Reflection.BindingFlags BF_PublicStatic=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static;
		private static readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binF
			=new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		/// <summary>
		/// �����񂩂�w�肵���^�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="value">�ϊ����̕�������w�肵�܂��B</param>
		/// <returns>�ϊ��������ʂ̃I�u�W�F�N�g��Ԃ��܂��B</returns>
		/// <typeparam name="T">�ϊ���̌^���w�肵�܂��B</typeparam>
		public static T To<T>(string value){
			return (T)To(typeof(T),value);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g���w�肵���^�Ƃ��ĕ�����ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		/// <typeparam name="T">�ϊ����̃I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
		public static string FromAs<T>(T value){
			return From(typeof(T),value);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		public static string From(object value){
			return From(value.GetType(),value);
		}
		/// <summary>
		/// �����񂩂�w�肵���^�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="t">�ϊ���̌^���w�肵�܂��B</param>
		/// <param name="value">�ϊ����̕�������w�肵�܂��B</param>
		/// <returns>�ϊ��������ʂ̃I�u�W�F�N�g��Ԃ��܂��B</returns>
		public static object To(System.Type t,string value){
			switch(Types.GetTypeCode(t)){
				case TypeCodes.SByte:return ToSByte(value);
				case TypeCodes.Byte:return ToByte(value);
				case TypeCodes.Short:return ToInt16(value);
				case TypeCodes.UShort:return ToUInt16(value);
				case TypeCodes.Int:return ToInt32(value);
				case TypeCodes.UInt:return ToUInt32(value);
				case TypeCodes.Long:return ToInt64(value);
				case TypeCodes.ULong:return ToUInt64(value);
				case TypeCodes.Decimal:return ToDecimal(value);
 				case TypeCodes.Float:return ToSingle(value);
				case TypeCodes.Double:return ToDouble(value);
				case TypeCodes.Bool:return ToBoolean(value);
				case TypeCodes.Char:return ToChar(value);
				case TypeCodes.String:return value;

				case TypeCodes.Guid:return ToGuid(value);
				case TypeCodes.TimeSpan:return ToTimeSpan(value);
				case TypeCodes.DateTime:return ToDateTime(value);
				case TypeCodes.IntPtr:return (System.IntPtr)ToInt64(value);
				case TypeCodes.UIntPtr:return (System.UIntPtr)ToUInt64(value);
				case TypeCodes.BoolArray:return ToBooleanArray(value);
				case TypeCodes.ByteArray:return ToByteArray(value);
				case TypeCodes.Type:
					if(t!=typeof(System.Type))goto default;
					object val=System.Type.GetType(value,false,false);
					return val!=null?val:System.Type.GetType(value,true,true);
				default:
			//	typeconv:
					if(t.IsEnum)return System.Enum.Parse(t,value);//System.Enum
					if(t.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute),false).Length==0)goto serial;
					System.ComponentModel.TypeConverter conv=System.ComponentModel.TypeDescriptor.GetConverter(t);
					if(conv.CanConvertFrom(typeof(string)))
						try{return conv.ConvertFromString(value);}catch{}
				serial:
					if(t.GetCustomAttributes(typeof(System.SerializableAttribute),false).Length==0)goto op_implicit;
					using(System.IO.MemoryStream memstr=new System.IO.MemoryStream(System.Convert.FromBase64String(value))){
						return binF.Deserialize(memstr);
					}
				op_implicit:
					System.Reflection.MethodInfo[] ms=t.GetMethods(BF_PublicStatic);
					for(int i=0;i<ms.Length;i++){
						if(ms[i].ReturnType!=t)continue;
						if(ms[i].Name!="op_Implicit"&&ms[i].Name!="op_Explicit")continue;
						System.Reflection.ParameterInfo[] ps=ms[i].GetParameters();
						if(ps.Length!=1||ps[0].ParameterType!=typeof(string))continue;
						return ms[i].Invoke(null,new object[]{value});
					}
					throw new System.ArgumentException(string.Format(FROMSTR_NOTSUPPORTEDTYPE,t),"t");
			}
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g���w�肵���^�Ƃ��ĕ�����ɕϊ����܂��B
		/// </summary>
		/// <param name="t">�ϊ����̃I�u�W�F�N�g�̌^���w�肵�܂��B</param>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		public static string From(System.Type t,object value){
			switch(Types.GetTypeCode(t)){
				case TypeCodes.SByte:return FromSByte((sbyte)value);
				case TypeCodes.Byte:return FromByte((byte)value);
				case TypeCodes.Short:return FromInt16((short)value);
				case TypeCodes.UShort:return FromUInt16((ushort)value);
				case TypeCodes.Int:return FromInt32((int)value);
				case TypeCodes.UInt:return FromUInt32((uint)value);
				case TypeCodes.Long:return FromInt64((long)value);
				case TypeCodes.ULong:return FromUInt64((ulong)value);
				case TypeCodes.Decimal:return FromDecimal((decimal)value);
 				case TypeCodes.Float:return FromSingle((float)value);
				case TypeCodes.Double:return FromDouble((double)value);
				case TypeCodes.Bool:return FromBoolean((bool)value);
				case TypeCodes.Char:return FromChar((char)value);
				case TypeCodes.String:return (string)value;

				case TypeCodes.Guid:return FromGuid((System.Guid)value);
				case TypeCodes.TimeSpan:return FromTimeSpan((System.TimeSpan)value);
				//.NET Framework 2.0 : System.Xml.XmlDateTimeSerializationMode ���w��
				case TypeCodes.DateTime: return FromDateTime((System.DateTime)value);
				case TypeCodes.IntPtr:return FromInt64((long)(System.IntPtr)value);
				case TypeCodes.UIntPtr:return FromUInt64((ulong)(System.UIntPtr)value);
				case TypeCodes.BoolArray:return FromBooleanArray((bool[])value);
				case TypeCodes.ByteArray:return FromByteArray((byte[])value);
				case TypeCodes.Type:
					return ((System.Type)value).FullName;
				default:
			//	typeconv:
					if(t.IsEnum)return value.ToString();//System.Enum
					if(t.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute),false).Length==0)goto serial;
					System.ComponentModel.TypeConverter conv=System.ComponentModel.TypeDescriptor.GetConverter(t);
					if(conv.CanConvertTo(typeof(string))&&conv.CanConvertFrom(typeof(string)))
						try{return conv.ConvertToString(value);}catch{}
				serial:
					if(t.GetCustomAttributes(typeof(System.SerializableAttribute),false).Length==0)goto op_implicit;
					using(System.IO.MemoryStream memstr=new System.IO.MemoryStream()){
						binF.Serialize(memstr,value);
						//--����
						int len;
						try{
							len=(int)memstr.Length;
						}catch(System.Exception e){
							throw new System.Exception("�f�[�^�̗ʂ��傫������̂ŕ�����ɕϊ��ł��܂���",e);
						}
						//--������ɕϊ�
						byte[] buff=new byte[len];
						memstr.Position=0;
						memstr.Read(buff,0,len);
						return System.Convert.ToBase64String(buff);
					}
				op_implicit:
					System.Reflection.MethodInfo[] ms=t.GetMethods(BF_PublicStatic);
					for(int i=0;i<ms.Length;i++){
						if(ms[i].Name!="op_Implicit"||ms[i].ReturnType!=typeof(string))continue;
						System.Reflection.ParameterInfo[] ps=ms[i].GetParameters();
						if(ps.Length!=1||ps[0].ParameterType!=t)continue;
						return (string)ms[i].Invoke(null,new object[]{value});
					}
					throw new System.ArgumentException(string.Format(TOSTR_NOTSUPPORTEDTYPE,t),"t");
			}
		}
	}
}