namespace afh.JavaScript{
	public class Global{
		//=================================================
		//		System.Object �� afh.Javascript.Object
		//=================================================
		/// <summary>
		/// ManagedObject ���� Javascript.Object �ւ̕ϊ���񋟂��܂��B
		/// </summary>
		/// <param name="obj">�ϊ��O�� .NET �I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>
		/// �w�肵���I�u�W�F�N�g�� afh.Javascript.Object �̏ꍇ�ɂ͕ϊ��������ɂ��̂܂ܕԂ��܂��B
		/// �w�肵���I�u�W�F�N�g�����l�E������E�^�U�l�Ȃǂ̏ꍇ�ɂ́A�Ή����� Javascript.Object �ɕϊ����ĕԂ��܂��B
		/// �w�肵���I�u�W�F�N�g����̉���ɂ����ěƂ܂�Ȃ����ɂ� Javascript.ManagedObject �Ƃ��ĕԂ��܂��B
		/// </returns>
		public static JavaScript.Object ConvertFromManaged(object obj){
			System.Type t=obj.GetType();
			if(t.IsSubclassOf(typeof(JavaScript.Object)))
				return (JavaScript.Object)obj;
			switch(t.FullName){
				case "System.String": return new String((string)obj);
				case "System.SByte": return new Number((long)(sbyte)obj);
				case "System.Int16": return new Number((long)(short)obj);
				case "System.Int32": return new Number((long)(int)obj);
				case "System.IntPtr": return new Number((long)(System.IntPtr)obj);
				case "System.Int64": return new Number((long)obj);
				case "System.Byte": return new Number((long)(byte)obj);
				case "System.UInt16": return new Number((long)(ushort)obj);
				case "System.UInt32": return new Number((long)(uint)obj);
				case "System.UIntPtr": return new Number((long)(System.UIntPtr)obj);
				//case "System.UInt64":case "System.Decimal":
				case "System.Double":return new Number((double)obj);
				case "System.Single":return new Number((double)(float)obj);
				default:
					if(t.GetInterface("System.Collections.IEnumerable")!=null)
						//TODO: Hashtable/Dictionary ���̏ꍇ�͂ǂ�����̂�?
						// ������Ȃǂɂ��������Ă��܂��̂ŋp��
						return new JavaScript.Array((System.Collections.IEnumerable)obj);

					//TODO: �v���O�C���Ȃǂ���ǉ��ł���悤�ɂ���
					return new ManagedObject(obj);
			}
		}
		public static Object ConvertFromManaged(sbyte i){return new Number((long)i);}
		public static Object ConvertFromManaged(byte i) { return new Number((long)i); }
		public static Object ConvertFromManaged(short i) { return new Number((long)i); }
		public static Object ConvertFromManaged(ushort i) { return new Number((long)i); }
		public static Object ConvertFromManaged(int i) { return new Number((long)i); }
		public static Object ConvertFromManaged(uint i) { return new Number((long)i); }
		public static Object ConvertFromManaged(long i) { return new Number(i); }
		public static Object ConvertFromManaged(float i) { return new Number((double)i); }
		public static Object ConvertFromManaged(double i) { return new Number(i); }
		public static JavaScript.Object _global;
		//=================================================
		//		������
		//=================================================
		static Global(){
			Global._global=new Object(new Null());
			Global._global["CollectGarbage"]=new ManagedDelegate(typeof(System.GC),"Collect");
			Object.Initialize();
			Number.Initialize();
			Array.Initialize();
		}
		public static bool IsCastable(System.Type src,System.Type target) {
			return src==target||src.IsSubclassOf(target)
				||target.IsInterface&&src.GetInterface(target.FullName)!=null;
		}
	}

}