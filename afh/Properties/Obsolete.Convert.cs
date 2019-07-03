namespace afh.Convert{
	/// <summary>
	/// ������Ƃ̉t�ȑ��ݕϊ���񋟂���N���X�ł��B
	/// </summary>
	/// <remarks>
	/// �ϊ�����镨��...
	/// 1. ��{�^
	/// 2. System.ComponentModel.TypeConverterAttribute ���w�肵�Ă��镨
	/// 3. implicit operator T(string) / implicit operator string(T) ���������Ă��镨
	/// 3. System.SerializableAttribute ���w�肵�Ă��镨
	/// </remarks>
	[System.Obsolete("afh.StringConvert ���g�p���ĉ������B")]
	public class Convert{
		private const string FROMSTR_NOTSUPPORTEDTYPE="<afh.dll> afh.Convert.FromString: �w�肵���^ {0} ����̕ϊ��ɂ͑Ή����Ă��܂���";
		private const string TOSTR_NOTSUPPORTEDTYPE="<afh.dll> afh.Convert.ToString: �w�肵���^ {0} �ւ̕ϊ��ɂ͑Ή����Ă��܂���";
		private const System.Reflection.BindingFlags BF_PublicStatic=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static;
		private static readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binF
			=new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		//private static readonly System.Windows.Forms.SelectionRangeConverter selectionRangeConv
		//	=new System.Windows.Forms.SelectionRangeConverter();
		/// <summary>
		/// �����񂩂�w�肵���^�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="t">�ϊ���̌^���w�肵�܂��B</param>
		/// <param name="value">�ϊ����̕�������w�肵�܂��B</param>
		/// <returns>�ϊ��������ʂ̃I�u�W�F�N�g��Ԃ��܂��B</returns>
		public static object FromString(System.Type t,string value){
			switch(t.FullName.GetHashCode()&0x7fffffff){
                case 0x038D0F82:
					if(t!=typeof(System.Int16))goto default;
					return System.Convert.ToInt16(value);
                case 0x6D318EFD:
					if(t!=typeof(System.UInt16))goto default;
					return System.Convert.ToUInt16(value);
                case 0x1B47F8B8:
					if(t!=typeof(System.Int32))goto default;
					return FromStringToInt32(value);
                case 0x03FB8EF9:
					if(t!=typeof(System.UInt32))goto default;
					return System.Convert.ToUInt32(value);
                case 0x4A1B9AE7:
					if(t!=typeof(System.Int64))goto default;
					return System.Convert.ToInt64(value);
                case 0x61CC8EFB:
					if(t!=typeof(System.UInt64))goto default;
					return System.Convert.ToUInt64(value);
                case 0x4EE7D89D:
					if(t!=typeof(System.SByte))goto default;
					return System.Convert.ToSByte(value);
                case 0x2F001A17:
					if(t!=typeof(System.Byte))goto default;
					return System.Convert.ToByte(value);
                case 0x1B1EFB13:
					if(t!=typeof(System.Decimal))goto default;
					return System.Convert.ToDecimal(value);
                case 0x44059415:
					if(t!=typeof(System.Char))goto default;
					return System.Convert.ToChar(value);
                case 0x3EACB635:
					if(t!=typeof(System.Single))goto default;
					return System.Convert.ToSingle(value);
                case 0x0ED5FC72:
					if(t!=typeof(System.Double))goto default;
					return System.Convert.ToDouble(value);
                case 0x71309BFC:
					if(t!=typeof(System.Boolean))goto default;
					return FromStringToBoolean(value);
                case 0x5981F920:
					if(t!=typeof(System.String))goto default;
					return value;
                case 0x1D77C984:
					if(t!=typeof(System.IntPtr))goto default;
					return (System.IntPtr)System.Convert.ToInt64(value);
                case 0x65648F3B:
					if(t!=typeof(System.UIntPtr))goto default;
					return (System.UIntPtr)System.Convert.ToUInt64(value);
                case 0x29BD8EB6:
					if(t!=typeof(System.Guid))goto default;
					return System.Xml.XmlConvert.ToGuid(value);
                case 0x1FE98930:
					if(t!=typeof(System.TimeSpan))goto default;
					return FromStringToTimeSpan(value);
                case 0x4A398CD8:
					if(t!=typeof(System.DateTime))goto default;
					return FromStringToDateTime(value);
                case 0x7F721A17:
					if(t!=typeof(System.Type))goto default;
					object val=System.Type.GetType(value,false,false);
					return val!=null?val:System.Type.GetType(value,true,true);
                case 0x0458EA59:
					if(t!=typeof(System.Boolean[]))goto default;
					return FromStringToBooleanArray(value);
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
						return Convert.binF.Deserialize(memstr);
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
#if NET1_1
          	switch(t.FullName.GetHashCode()&0x7fffffff){
				case 0x2DBDA61A://case 0xE31638:
					if(t!=typeof(System.Int16))goto default;
					return System.Convert.ToInt16(value);
				case 0x1107D7EF://case 0xE3166C:
					if(t!=typeof(System.UInt16))goto default;
					return System.Convert.ToUInt16(value);
				case 0x2DBDA65C://case 0xE312F4:
					if(t!=typeof(System.Int32))goto default;
					return FromStringToInt32(value);
				case 0x1107D829://case 0xE316A0:
					if(t!=typeof(System.UInt32))goto default;
					return System.Convert.ToUInt32(value);
				case 0x2DBDA63F://case 0xE316D4:
					if(t!=typeof(System.Int64))goto default;
					return System.Convert.ToInt64(value);
				case 0x1107D84A://case 0xE31708:
					if(t!=typeof(System.UInt64))goto default;
					return System.Convert.ToUInt64(value);
				case 0x2EF00F97://case 0xE315D0:
					if(t!=typeof(System.SByte))goto default;
					return System.Convert.ToSByte(value);
				case 0x244D3E44://case 0xE31604:
					if(t!=typeof(System.Byte))goto default;
					return System.Convert.ToByte(value);
				case 0x32C73145://case 0xE317A4:
					if(t!=typeof(System.Decimal))goto default;
					return System.Convert.ToDecimal(value);
				case 0x244C7CD6://case 0xE3159C:
					if(t!=typeof(System.Char))goto default;
					return System.Convert.ToChar(value);
				case 0x0EC74674://case 0xE3173C:
					if(t!=typeof(System.Single))goto default;
					return System.Convert.ToSingle(value);
				case 0x5E38073B://case 0xE31770:
					if(t!=typeof(System.Double))goto default;
					return System.Convert.ToDouble(value);
				case 0x604332EA://case 0xE31568:
					if(t!=typeof(System.Boolean))goto default;
					return FromStringToBoolean(value);
				case 0x0DE37C3B://case 0xE31328:
					if(t!=typeof(System.String))goto default;
					return value;
				case 0x6572ED4B://case 0xE37678:
					if(t!=typeof(System.IntPtr))goto default;
					return (System.IntPtr)System.Convert.ToInt64(value);
				case 0x3203515E://case 0xE376AC:
					if(t!=typeof(System.UIntPtr))goto default;
					return (System.UIntPtr)System.Convert.ToUInt64(value);
				case 0x244AC511://case 0xE376E0:
					if(t!=typeof(System.Guid))goto default;
					return System.Xml.XmlConvert.ToGuid(value);
				case 0x4BD7DD17://case 0xE37714:
					if(t!=typeof(System.TimeSpan))goto default;
					return FromStringToTimeSpan(value);
				case 0x7F9DDECF://case 0xE317D8://.NET Framework 2.0 : System.Xml.XmlDateTimeSerializationMode ���w��
					if(t!=typeof(System.DateTime))goto default;
					return FromStringToDateTime(value);
				case 0x24524716://case 0xE37610:
					if(t!=typeof(System.Type))goto default;
					object val=System.Type.GetType(value,false,false);
					return val!=null?val:System.Type.GetType(value,true,true);
			//	case 0x2453BC7A://case 0xE37748:
			//		if(t!=typeof(void))goto default;
			//		break;
				case 0x7DDB9ECC://case 0xE37778:
					if(t!=typeof(System.Boolean[]))goto default;
					return FromStringToBooleanArray(value);
			//	case 0x45EBBE0:
			//		if(t!=typeof(System.Windows.Forms.SelectionRange))goto default;
			//		return Convert.selectionRangeConv.ConvertFromString(value);
			//	case 0x244D9E9D://case 0xE311BC:
			//		if(t!=typeof(System.Enum))goto default;
			//		return System.Xml.XmlConvert.ToInt64(value);
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
						return Convert.binF.Deserialize(memstr);
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
#endif
        }

		#region FromStringTo***
		/// <summary>
		/// ������� System.Int32 �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ��O�̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̐��l��Ԃ��܂��B</returns>
		public static int FromStringToInt32(string value){return System.Convert.ToInt32(value);}
		/// <summary>
		/// ������� System.Boolean �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ��O�̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̐��l��Ԃ��܂��B</returns>
		public static bool FromStringToBoolean(string value){return System.Convert.ToBoolean(value);}
		/// <summary>
		/// ������� System.DateTime �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ��O�̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̐��l��Ԃ��܂��B</returns>
		public static System.DateTime FromStringToDateTime(string value){return System.Convert.ToDateTime(value);}
		/// <summary>
		/// ������� System.TimeSpan �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ��O�̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̐��l��Ԃ��܂��B</returns>
		public static System.TimeSpan FromStringToTimeSpan(string value){return System.Xml.XmlConvert.ToTimeSpan(value);}
		/// <summary>
		/// ������� System.Boolean[] �ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ��O�̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̐��l��Ԃ��܂��B</returns>
		public static bool[] FromStringToBooleanArray(string value){
			char[] c=value.ToCharArray();
			int l=c.Length;
			bool[] r=new bool[l];
			for(int i=0;i<l;i++)r[i]=c[i]=='1';
			return r;
		}
#if !NET1_1
		/// <summary>
		/// ��������w�肵���^�ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="T">�ϊ��ڕW�̌^���w�肵�܂��B</typeparam>
		/// <param name="value">�ϊ����̕�������w�肵�܂��B</param>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		public static T FromString<T>(string value){return (T)FromString(typeof(T),value);}
#endif
		#endregion

		/// <summary>
		/// �w�肵���I�u�W�F�N�g�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		public static string ToString(object value){
			return Convert.ToString(value.GetType(),value);
		}
		/// <summary>
		/// �w�肵���^�̎w�肵���I�u�W�F�N�g�𕶎���ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="T">�ϊ����̃I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		public static string ToString<T>(T value){
			return Convert.ToString(typeof(T),value);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g���w�肵���^�Ƃ��ĕ�����ɕϊ����܂��B
		/// </summary>
		/// <param name="t">�ϊ����̃I�u�W�F�N�g�̌^���w�肵�܂��B</param>
		/// <param name="value">�ϊ����̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�ϊ���̕������Ԃ��܂��B</returns>
		public static string ToString(System.Type t,object value){
			switch(t.FullName.GetHashCode()&0x7fffffff){
				case 0x038D0F82:
					if(t!=typeof(System.Int16))goto default;
					return System.Convert.ToString((System.Int16)value);
				case 0x6D318EFD:
					if(t!=typeof(System.UInt16))goto default;
					return System.Convert.ToString((System.UInt16)value);
				case 0x1B47F8B8:
					if(t!=typeof(System.Int32))goto default;
					return System.Convert.ToString((System.Int32)value);
				case 0x03FB8EF9:
					if(t!=typeof(System.UInt32))goto default;
					return System.Convert.ToString((System.UInt32)value);
				case 0x4A1B9AE7:
					if(t!=typeof(System.Int64))goto default;
					return System.Convert.ToString((System.Int64)value);
				case 0x61CC8EFB:
					if(t!=typeof(System.UInt64))goto default;
					return System.Convert.ToString((System.UInt64)value);
				case 0x4EE7D89D:
					if(t!=typeof(System.SByte))goto default;
					return System.Convert.ToString((System.SByte)value);
				case 0x2F001A17:
					if(t!=typeof(System.Byte))goto default;
					return System.Convert.ToString((System.Byte)value);
				case 0x1B1EFB13:
					if(t!=typeof(System.Decimal))goto default;
					return System.Convert.ToString((System.Decimal)value);
				case 0x44059415:
					if(t!=typeof(System.Char))goto default;
					return System.Convert.ToString((System.Char)value);
				case 0x3EACB635:
					if(t!=typeof(System.Single))goto default;
					return System.Convert.ToString((System.Single)value);
				case 0x0ED5FC72:
					if(t!=typeof(System.Double))goto default;
					return System.Convert.ToString((System.Double)value);
				case 0x71309BFC:
					if(t!=typeof(System.Boolean))goto default;
					return System.Convert.ToString((System.Boolean)value);
				case 0x5981F920:
					if(t!=typeof(System.String))goto default;
					return (string)value;
				case 0x1D77C984:
					if(t!=typeof(System.IntPtr))goto default;
					return System.Convert.ToString((System.Int64)value);
				case 0x65648F3B:
					if(t!=typeof(System.UIntPtr))goto default;
					return System.Convert.ToString((System.UInt64)value);
				case 0x29BD8EB6:
					if(t!=typeof(System.Guid))goto default;
					return System.Xml.XmlConvert.ToString((System.Guid)value);
				case 0x1FE98930:
					if(t!=typeof(System.TimeSpan))goto default;
					return System.Xml.XmlConvert.ToString((System.TimeSpan)value);
				case 0x4A398CD8:
					if(t!=typeof(System.DateTime))goto default;
					//.NET Framework 2.0 : System.Xml.XmlDateTimeSerializationMode ���w��
					return System.Convert.ToString((System.DateTime)value);
				case 0x7F721A17:
					if(t!=typeof(System.Type))goto default;
					return ((System.Type)value).FullName;
				case 0x0458EA59:
					if(t!=typeof(System.Boolean[]))goto default;
					return ToString((bool[])value);
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
						Convert.binF.Serialize(memstr,value);
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
#if NET1_1
			switch(t.FullName.GetHashCode()&0x7fffffff){
				case 0x2DBDA61A://case 0xE31638:
					if(t!=typeof(System.Int16))goto default;
					return System.Convert.ToString((System.Int16)value);
				case 0x1107D7EF://case 0xE3166C:
					if(t!=typeof(System.UInt16))goto default;
					return System.Convert.ToString((System.UInt16)value);
				case 0x2DBDA65C://case 0xE312F4:
					if(t!=typeof(System.Int32))goto default;
					return System.Convert.ToString((System.Int32)value);
				case 0x1107D829://case 0xE316A0:
					if(t!=typeof(System.UInt32))goto default;
					return System.Convert.ToString((System.UInt32)value);
				case 0x2DBDA63F://case 0xE316D4:
					if(t!=typeof(System.Int64))goto default;
					return System.Convert.ToString((System.Int64)value);
				case 0x1107D84A://case 0xE31708:
					if(t!=typeof(System.UInt64))goto default;
					return System.Convert.ToString((System.UInt64)value);
				case 0x2EF00F97://case 0xE315D0:
					if(t!=typeof(System.SByte))goto default;
					return System.Convert.ToString((System.SByte)value);
				case 0x244D3E44://case 0xE31604:
					if(t!=typeof(System.Byte))goto default;
					return System.Convert.ToString((System.Byte)value);
				case 0x32C73145://case 0xE317A4:
					if(t!=typeof(System.Decimal))goto default;
					return System.Convert.ToString((System.Decimal)value);
				case 0x244C7CD6://case 0xE3159C:
					if(t!=typeof(System.Char))goto default;
					return System.Convert.ToString((System.Char)value);
				case 0x0EC74674://case 0xE3173C:
					if(t!=typeof(System.Single))goto default;
					return System.Convert.ToString((System.Single)value);
				case 0x5E38073B://case 0xE31770:
					if(t!=typeof(System.Double))goto default;
					return System.Convert.ToString((System.Double)value);
				case 0x604332EA://case 0xE31568:
					if(t!=typeof(System.Boolean))goto default;
					return System.Convert.ToString((System.Boolean)value);
				case 0x0DE37C3B://case 0xE31328:
					if(t!=typeof(System.String))goto default;
					return (string)value;
				case 0x6572ED4B://case 0xE37678:
					if(t!=typeof(System.IntPtr))goto default;
					return System.Convert.ToString((System.Int64)value);
				case 0x3203515E://case 0xE376AC:
					if(t!=typeof(System.UIntPtr))goto default;
					return System.Convert.ToString((System.UInt64)value);
				case 0x244AC511://case 0xE376E0:
					if(t!=typeof(System.Guid))goto default;
					return System.Xml.XmlConvert.ToString((System.Guid)value);
				case 0x4BD7DD17://case 0xE37714:
					if(t!=typeof(System.TimeSpan))goto default;
					return System.Xml.XmlConvert.ToString((System.TimeSpan)value);
				case 0x7F9DDECF://case 0xE317D8:
					if(t!=typeof(System.DateTime))goto default;
					//.NET Framework 2.0 : System.Xml.XmlDateTimeSerializationMode ���w��
					return System.Convert.ToString((System.DateTime)value);
				case 0x24524716://case 0xE37610:
					if(t!=typeof(System.Type))goto default;
					return ((System.Type)value).FullName;
			//	case 0x2453BC7A://case 0xE37748:
			//		if(t!=typeof(void))goto default;
			//		break;
				case 0x7DDB9ECC://case 0xE37778:
					if(t!=typeof(System.Boolean[]))goto default;
					return ToString((bool[])value);
			//	case 0x45EBBE0:
			//		if(t!=typeof(System.Windows.Forms.SelectionRange))goto default;
			//		return Convert.selectionRangeConv.ConvertToString(value);
			//	case 0x244D9E9D://case 0xE311BC:
			//		if(t!=typeof(System.Enum))goto default;
			//		return System.Xml.XmlConvert.ToString((System.Int64)value);
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
						Convert.binF.Serialize(memstr,value);
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
#endif
		}
		#region ToString(***)
		/// <summary>
		/// bool �𕶎���ɕϊ����܂��B 
		/// </summary>
		/// <param name="value">�ϊ��O�̒l���w�肵�܂��B</param>
		/// <returns>�ϊ����ʂ̕������Ԃ��܂��B</returns>
		public static string ToString(bool value){return System.Convert.ToString(value);}
		/// <summary>
		/// bool[] �𕶎���ɕϊ����܂��B 
		/// </summary>
		/// <param name="value">�ϊ��O�̒l���w�肵�܂��B</param>
		/// <returns>�ϊ����ʂ̕������Ԃ��܂��B</returns>
		public static string ToString(bool[] value){
			string r="";
			for(int i=0;i<value.Length;i++)r+=value[i]?"1":"0";
			return r;
		}
		#endregion
	}
}