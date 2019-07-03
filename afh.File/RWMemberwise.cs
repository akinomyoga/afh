using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Emit=System.Reflection.Emit;

namespace afh.File {
	using afh.File.Design;

	#region attr:ReadSchedule
	/// <summary>
	/// Stream ����̓Ǎ�����̏��Ԃ��w�肵�܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class ReadScheduleAttribute:ReadTypeAttribute{
		private string[] members;
		/// <summary>
		/// ReadScheduleAttribute �̃R���X�g���N�^�ł��BStream ����̓Ǎ�����̏��Ԃ̎w����s���܂��B
		/// </summary>
		/// <param name="membernames">Stream ����̓Ǎ��̎菇�������ׂɁA�����o�����w�肵�܂��B</param>
		public ReadScheduleAttribute(params string[] membernames){
			this.members=membernames;
		}
		/// <summary>
		/// �w�肵���^�̒l�� Stream ����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="type">�ǂݎ��l�̌^���w�肵�܂��B</param>
		/// <param name="accessor">Stream �ɑ΂��鑀���񋟂��� <see cref="StreamAccessor"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		[System.Obsolete]
		public override object Read(System.Type type,StreamAccessor accessor){
			object o=CreateInstance(type);

			for(int i=0;i<this.members.Length;i++){
				System.Reflection.MemberInfo info=GetMemberInfo(type,members[i]);
				switch(info.MemberType){
					case System.Reflection.MemberTypes.Method:
						((System.Reflection.MethodInfo)info).Invoke(o,new object[]{accessor});
						break;
					case System.Reflection.MemberTypes.Field:
					case System.Reflection.MemberTypes.Property:
						ReadWriteMemberAttribute[] attrs=
							(ReadWriteMemberAttribute[])info.GetCustomAttributes(typeof(ReadWriteMemberAttribute),true);
						int len=attrs.Length-1;
						if(len<0)break;

						int j=0;
						while(j<len){
							// ReadWriteArray �����Ɏ��̑�����ݒ�
							if(attrs[j] is ReadWriteArrayAttribute){
								((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
							}
						}
						attrs[0].Read(o,info,accessor);
						break;
					default:
						throw new System.ApplicationException("�w�肵����ނ̃����o�͓Ǎ��Ɏg�p�ł��܂���B");
				}
			}
			return o;
		}


		private static System.Type[] createInstance_types=new System.Type[0];
		private static System.Reflection.ParameterModifier[] createInstance_mods=new System.Reflection.ParameterModifier[0];
		private static object CreateInstance(System.Type type){
			const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic
				|System.Reflection.BindingFlags.Instance;
			System.Reflection.ConstructorInfo ctor=type.GetConstructor(BF,null,createInstance_types,createInstance_mods);
			if(ctor!=null){
				return ctor.Invoke(new object[0]);
			}

			if(type.IsValueType){
				object ret=type.Assembly.CreateInstance(type.FullName);
				if(ret!=null)return ret;
			}
			
			throw new System.ApplicationException("�w�肵���^�̃C���X�^���X���쐬���鎖���o���܂���ł����B");
		}

		public override ICustomReader GetCustomReader(System.Type type) {
			return new Reader(type,this.members);
		}

		#region cls:Reader
		private delegate object ReaderDelegate(StreamAccessor accessor);
		private sealed class Reader:ICustomReader{
			private System.Type type;
			private Ref::MemberInfo[] infos;
			private ReadWriteMemberAttribute[] attrs;
			private ReaderDelegate read;

			public Reader(System.Type type,string[] members){
				this.type=type;
				this.read=EmitProcess(type,members);
			}
			public System.Type Type{get{return this.type;}}
			public object Read(StreamAccessor accessor){
				return this.read(accessor);
			}

			private ReaderDelegate EmitProcess(System.Type type,string[] members){
				const string ERR_MISSING_ATTR=@"�w�肵�������o�ɂ͓ǂݏ����Ɋւ�������w�肷�鑮��������܂���B
�^: {0}
�����o��: {1}
ReadWriteMemberAttribute ���p�����鑮�����w�肵�ĉ������B";
				//---------------------------------------------------
				Emit::DynamicMethod method=new System.Reflection.Emit.DynamicMethod("read",typeof(object),new System.Type[]{typeof(Reader),typeof(StreamAccessor)},typeof(Reader));
				// arg0: this
				// arg1: accessor
				Emit::ILGenerator ilgen=method.GetILGenerator();
				// loc0: ret
				Emit::LocalBuilder loc_ret=ilgen.DeclareLocal(type);

				Gen::List<Ref::MemberInfo> infolist=new Gen::List<Ref::MemberInfo>();
				Gen::List<ReadWriteMemberAttribute> attrlist=new Gen::List<ReadWriteMemberAttribute>();

				// object ret=CreateInstance(type);
				EmitCreateInstance(ilgen,type,loc_ret);
				for(int i=0;i<members.Length;i++) {
					System.Reflection.MemberInfo info=GetMemberInfo(type,members[i]);
					switch(info.MemberType){
						case System.Reflection.MemberTypes.Method:
							System.Reflection.MethodInfo minfo=(Ref::MethodInfo)info;
							ilgen.Emit(Emit::OpCodes.Ldloc,loc_ret);
							ilgen.Emit(Emit::OpCodes.Ldarg_1);
							ilgen.Emit(Emit::OpCodes.Callvirt,minfo);
							if(minfo.ReturnType!=null&&minfo.ReturnType!=typeof(void))
								ilgen.Emit(Emit::OpCodes.Pop);
							break;
						case System.Reflection.MemberTypes.Field:
						case System.Reflection.MemberTypes.Property:
							ReadWriteMemberAttribute[] attrs=
								(ReadWriteMemberAttribute[])info.GetCustomAttributes(typeof(ReadWriteMemberAttribute),true);
							int len=attrs.Length-1;
							if(len<0){
								throw new System.ApplicationException(string.Format(ERR_MISSING_ATTR,type.FullName,members[i]));
							}

							int j=0;
							while(j<len){
								// ReadWriteArray �����Ɏ��̑�����ݒ�
								if(attrs[j] is ReadWriteArrayAttribute){
									((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
								}
							}

							// this.attrs[...]
							ilgen.Emit(Emit::OpCodes.Ldarg_0);
							ilgen.Emit(Emit::OpCodes.Ldfld,typeof(Reader).GetField("attrs",BF));
							ilgen.Emit(Emit::OpCodes.Ldc_I4,attrlist.Count);
							ilgen.Emit(Emit::OpCodes.Ldelem_Ref);
							// ret
							ilgen.Emit(Emit::OpCodes.Ldloc,loc_ret);
							// this.infos[...]
							ilgen.Emit(Emit::OpCodes.Ldarg_0);
							ilgen.Emit(Emit::OpCodes.Ldfld,typeof(Reader).GetField("infos",BF));
							ilgen.Emit(Emit::OpCodes.Ldc_I4,infolist.Count);
							ilgen.Emit(Emit::OpCodes.Ldelem_Ref);
							// accessor
							ilgen.Emit(Emit::OpCodes.Ldarg_1);
							// *.Read(*,*,*)
							ilgen.Emit(Emit::OpCodes.Callvirt,ATTR_READ);

							attrlist.Add(attrs[0]);
							infolist.Add(info);
							break;
						default:
							throw new System.ApplicationException("�w�肵����ނ̃����o�͓Ǎ��Ɏg�p�ł��܂���B");
					}
				}
				// return ret;
				ilgen.Emit(Emit::OpCodes.Ldloc,loc_ret);
				ilgen.Emit(Emit::OpCodes.Ret);

				this.infos=infolist.ToArray();
				this.attrs=attrlist.ToArray();
				
				return (ReaderDelegate)method.CreateDelegate(typeof(ReaderDelegate),this);
			}
			private static Ref::MethodInfo ATTR_READ=typeof(ReadWriteMemberAttribute).GetMethod(
				"Read",BF,null,
				new System.Type[]{typeof(object),typeof(Ref::MemberInfo),typeof(StreamAccessor)},
				null
				);
			private const System.Reflection.BindingFlags BF
				=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
			private static void EmitCreateInstance(Emit::ILGenerator ilgen,System.Type type,Emit::LocalBuilder loc_ret){
				System.Reflection.ConstructorInfo ctor=type.GetConstructor(BF,null,createInstance_types,createInstance_mods);
				if(ctor!=null) {
					ilgen.Emit(Emit::OpCodes.Newobj,ctor);
					ilgen.Emit(Emit::OpCodes.Stloc,loc_ret);
				}else if(type.IsValueType){
					ilgen.Emit(Emit::OpCodes.Ldloca_S,loc_ret);
					ilgen.Emit(Emit::OpCodes.Initobj,type);
				}else{
					throw new System.ApplicationException("�w�肵���^�̃C���X�^���X���쐬���鎖���o���܂���ł����B");
				}
			}

		}
		#endregion
	}
	#endregion

	#region attr:WriteSchedule
	/// <summary>
	/// Stream �ւ̏�������̏��Ԃ��w�肵�܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class WriteScheduleAttribute:WriteTypeAttribute{
		private string[] members;
		/// <summary>
		/// WriteScheduleAttribute �̃R���X�g���N�^�ł��BStream �ւ̏���������s�����Ԃ̎w������܂��B
		/// </summary>
		/// <param name="membernames">Stream ����̏����̎菇�������ׂɁA�����o�����w�肵�܂��B</param>
		public WriteScheduleAttribute(params string[] membernames){
			this.members=membernames;
		}
		/// <summary>
		/// �w�肵���^�̒l�� Stream ����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="obj">�������ޒl���w�肵�܂��B</param>
		/// <param name="accessor">Stream �ɑ΂��鑀���񋟂��� <see cref="StreamAccessor"/> ���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		[System.Obsolete]
		public override void Write(object obj,StreamAccessor accessor){
			System.Type type=obj.GetType();

			for(int i=0;i<this.members.Length;i++){
				System.Reflection.MemberInfo info=GetMemberInfo(type,members[i]);
				switch(info.MemberType){
					case System.Reflection.MemberTypes.Method:
						((System.Reflection.MethodInfo)info).Invoke(obj,new object[]{accessor});
						break;
					case System.Reflection.MemberTypes.Field:
					case System.Reflection.MemberTypes.Property:
						ReadWriteMemberAttribute[] attrs=
							(ReadWriteMemberAttribute[])info.GetCustomAttributes(typeof(ReadWriteMemberAttribute),true);
						int len=attrs.Length-1;
						if(len<0)break;

						int j=0;
						while(j<len){
							// ReadWriteArray �����Ɏ��̑�����ݒ�
							if(attrs[j] is ReadWriteArrayAttribute){
								((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
							}
						}
						attrs[0].Write(obj,info,accessor);
						break;
					default:
						throw new System.ApplicationException("�w�肵����ނ̃����o�͓Ǎ��Ɏg�p�ł��܂���B");
				}
			}
		}

		public override ICustomWriter GetCustomWriter(System.Type type) {
			return new Writer(type,this.members);
		}

		#region Writer
		private class Writer:ICustomWriter{
			private System.Type type;
			private afh.EventHandler<object,StreamAccessor> write;
			private ReadWriteMemberAttribute[] attrs;
			private Ref::MemberInfo[] infos;

			public Writer(System.Type type,string[] members){
				this.type=type;

				Emit::DynamicMethod method=new Emit::DynamicMethod("write",null,new System.Type[]{typeof(Writer),typeof(object),typeof(StreamAccessor)},typeof(Writer));
				EmitProcess(method.GetILGenerator(),type,members);
				try{
					this.write=(afh.EventHandler<object,StreamAccessor>)method.CreateDelegate(typeof(afh.EventHandler<object,StreamAccessor>),this);
				}catch(System.Exception e){
					__dll__.log.WriteError(e,"DynamicMethod �����Ɏ��s���܂����B");
					throw;
				}
			}

			public void Write(object obj,StreamAccessor accessor){
				this.write(obj,accessor);
			}
			public System.Type Type{get{return this.type;}}

			//=============================================
			//		Compile Method
			//=============================================
			private void EmitProcess(Emit::ILGenerator ilgen,System.Type type,string[] members){
				Gen::List<ReadWriteMemberAttribute> attrlist=new Gen::List<ReadWriteMemberAttribute>();
				Gen::List<Ref::MemberInfo> infolist=new Gen::List<Ref::MemberInfo>();

				for(int i=0;i<members.Length;i++){
					Ref::MemberInfo info=GetMemberInfo(type,members[i]);
					switch(info.MemberType){
						case System.Reflection.MemberTypes.Method:
							EmitMethodProcess(ilgen,type,(Ref::MethodInfo)info);
							break;
						case System.Reflection.MemberTypes.Field:
						case System.Reflection.MemberTypes.Property:
							ReadWriteMemberAttribute[] attrs=
								(ReadWriteMemberAttribute[])info.GetCustomAttributes(typeof(ReadWriteMemberAttribute),true);
							int len=attrs.Length-1;
							if(len<0)throw new System.ApplicationException("�w�肵�������o�ɂ͏����Ɋւ��鑮��������܂���B");

							int j=0;
							while(j<len) {
								// ReadWriteArray �����Ɏ��̑�����ݒ�
								if(attrs[j] is ReadWriteArrayAttribute) {
									((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
								}
							}
							EmitFieldProcess(ilgen,attrlist.Count,infolist.Count);
							attrlist.Add(attrs[0]);
							infolist.Add(info);
							break;
						default:
							throw new System.ApplicationException("�w�肵����ނ̃����o�͏����Ɏg�p�ł��܂���B");
					}
				}
				ilgen.Emit(Emit::OpCodes.Ret);

				this.attrs=attrlist.ToArray();
				this.infos=infolist.ToArray();
			}
			private static void EmitMethodProcess(Emit::ILGenerator ilgen,System.Type type,Ref::MethodInfo minfo){
				//
				// Emit "((T)arg_value).MINFO(arg_accessor);"
				//
				ilgen.Emit(Emit::OpCodes.Ldarg_1);
				ilgen.Emit(Emit::OpCodes.Castclass,type);
				ilgen.Emit(Emit::OpCodes.Ldarg_2);
				ilgen.Emit(Emit::OpCodes.Callvirt,minfo);
			}
			private static void EmitFieldProcess(Emit::ILGenerator ilgen,int attrindex,int infoindex){
				//
				// Emit "attrs[0].Write(arg_value,info,arg_accessor);"
				//
				// A: this.attrs[attrIndex]
				ilgen.Emit(Emit::OpCodes.Ldarg_0);
				ilgen.Emit(Emit::OpCodes.Ldfld,typeof(Writer).GetField("attrs",BF));
				ilgen.Emit(Emit::OpCodes.Ldc_I4,attrindex);
				ilgen.Emit(Emit::OpCodes.Ldelem_Ref);
				// B: value
				ilgen.Emit(Emit::OpCodes.Ldarg_1);
				// C: this.infos[infoindex]
				ilgen.Emit(Emit::OpCodes.Ldarg_0);
				ilgen.Emit(Emit::OpCodes.Ldfld,typeof(Writer).GetField("infos",BF));
				ilgen.Emit(Emit::OpCodes.Ldc_I4,infoindex);
				ilgen.Emit(Emit::OpCodes.Ldelem_Ref);
				// D: accessor
				ilgen.Emit(Emit::OpCodes.Ldarg_2);
				// A.Write(B,C,D);
				ilgen.Emit(Emit::OpCodes.Callvirt,ATTR_WRITE);
			}
			private const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
			private static Ref::MethodInfo ATTR_WRITE=typeof(ReadWriteMemberAttribute).GetMethod("Write",BF,null,new System.Type[]{typeof(object),typeof(Ref::MemberInfo),typeof(StreamAccessor)},null);
		}
		#endregion
	}
	#endregion

	#region attr:ReadWriteAs
	/// <summary>
	/// �t�@�C������ Stream �̓��o�͂ɍۂ��āA�ǂ̗l�Ȍ`�œǂݏ������邩���w�肵�܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property,AllowMultiple=true)]
	public class ReadWriteAsAttribute:ReadWriteMemberAttribute{
		private EncodingType enc;
		/// <summary>
		/// Stream ���ł̊i�[�`�����擾���܂��B
		/// </summary>
		public EncodingType EncodingType{get{return this.enc;}}
		/// <summary>
		/// Stream ���ł̊i�[�`������������Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool IsSpecifiedEncodingType{
			get{return this.enc==EncodingType.NoSpecified;}
		}
		/// <summary>
		/// ReadWriteAsAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="enc">Stream ���ł̊i�[�`�����w�肵�܂��B</param>
		public ReadWriteAsAttribute(EncodingType enc):base(){
			this.enc=enc;
		}
		/// <summary>
		/// ReadWriteAsAttribute �̃R���X�g���N�^�ł��B
		/// ���o�͂ɂ� EncodingType.NoSpecified ���K�p����܂��B
		/// </summary>
		public ReadWriteAsAttribute():this(EncodingType.NoSpecified){}
		//=================================================
		//		�e��p�����[�^
		//=================================================
		private int length=-1;
		/// <summary>
		/// ������̒������擾���͐ݒ肵�܂��B
		/// ���̒l�͎w�肪�Ȃ��ƌ������������܂��B
		/// </summary>
		public int Length{
			get{return this.length;}
			set{this.length=value;}
		}
		//=================================================
		//		�ǂݏ���
		//=================================================
		/// <summary>
		/// �w�肵���^�̒l�� StreamAccessor ��ʂ��ēǂݎ��܂��B
		/// </summary>
		/// <param name="args">�Ǎ�����E�Ǎ��ΏۂɊւ�������w�肵�܂��B</param>
		protected internal override void Read(ReadMemberArgs args) {
			object value;
			if((this.enc&EncodingType.MASK_TYPE)==EncodingType.String&&this.length>=0){
				value=args.Accessor.ReadStringByCharCount(this.enc,this.length);
			}else{
				value=args.Accessor.Read(args.TargetType,this.enc);
			}
			args.SetValueToTarget(value);
		}
		/// <summary>
		/// �w�肵���l���^�ɉ��������@�� StreamAccessor ��ʂ��� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="args">��������E�����ΏۂɊւ�������w�肵�܂��B</param>
		protected internal override void Write(WriteMemberArgs args) {
			if((this.enc&EncodingType.MASK_TYPE)==EncodingType.String&&this.length>=0)
				args.Accessor.WriteStringByCharCount((string)args.Value,this.length);
			args.Accessor.Write(args.Value,this.enc);
		}
	}
	#endregion

	#region attr:ReadWriteArray
	/// <summary>
	/// �z��̒����̎w����@��\�����܂��B
	/// </summary>
	public enum ArrayLengthType{
		/// <summary>
		/// �z��̏��߂ɁA�z��̗v�f�̌��� Int32LE �ŋL�^���܂��B
		/// </summary>
		CountEmbedded,
		/// <summary>
		/// �z��̏��߂ɁA�z��̃o�C�i���T�C�Y�� Int32LE �ŋL�^���܂��B
		/// </summary>
		SizeEmbedded,
		/// <summary>
		/// �z��̗v�f�̌���ʂɎw�肵�܂��B<see cref="P:ReadWriteArrayAttribute.Count"/> ���Q�Ƃ��ĉ������B
		/// </summary>
		CountSpecified,
		/// <summary>
		/// �z��̃o�C�i���T�C�Y��ʂɎw�肵�܂��B<see cref="P:ReadWriteArrayAttribute.Size"/> ���Q�Ƃ��ĉ������B
		/// </summary>
		SizeSpecified,
		/// <summary>
		/// Stream �̍Ō�܂œǂݎ��܂��B
		/// �ǂݎ��ő吔���w�肷��ꍇ�ɂ� <see cref="P:Count"/> �Ɏw�肵�܂��B
		/// �ǂݎ��ő吔�ɒB����� Stream �Ɏc�肪�����Ă������œǂݎ��𒆒f���܂��B
		/// </summary>
		ReadToEnd
	}
	/// <summary>
	/// �t�@�C������ Stream �̓��o�͂ɍۂ��āA�z�񓙂̃R���N�V������ǂݏ���������@���w�肵�܂��B
	/// <para>
	/// ���̑����w��Ɉ��������v�f�̓ǂݏ������@�w�葮�����w�肷��K�v������܂��B
	/// TODO: �w�肵�Ȃ������ꍇ�ɂ͊���̑��� (�^�ɉ����đ��肤��) ���g�p���ēǂݏ������܂��B
	/// </para>
	/// </summary>
	/// <remarks>
	/// �ǂݏ����̑ΏۂƂ��ĉ\�ȃR���N�V�����͈ȉ��ɋ����镨�ł��B
	/// �A���ȉ��ɋ������Ă��镨�ł��A�v�f�̌^��ǂݏ����ł��Ȃ��ꍇ�ɂ͓ǂݏ����͏o���܂���B
	/// <para>Stream ����Ǎ��\�ȕ�:
	/// T[] (���얢�m�F);
	/// System.Collections.ArrayList (���얢�m�F ��ԓ��삪�y���ł�);
	/// System.Collections.Stack (���얢�m�F);
	/// System.Collections.Queue (���얢�m�F);
	/// System.Collections.Generic.ICollection&lt;T&gt; (���얢�m�F);
	/// System.Collections.Generic.IList&lt;T&gt; (���얢�m�F);
	/// System.Collections.Generic.IEnumerable&lt;T&gt; (���얢�m�F);
	/// System.Collections.Generic.List&lt;T&gt; (���얢�m�F);
	/// System.Collections.Generic.Stack&lt;T&gt; (���얢�m�F);
	/// System.Collections.Generic.Queue&lt;T&gt; (���얢�m�F);
	/// </para>
	/// <para>Stream �ɏ����\�ȕ�:
	/// System.Collections.IEnumerable ���������Ă��镨�ɑΉ����Ă��܂� (���얢�m�F);
	/// </para>
	/// </remarks>
	[System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property,AllowMultiple=true)]
	public class ReadWriteArrayAttribute:ReadWriteMemberAttribute{
		private ReadWriteMemberAttribute child_attr;
		/// <summary>
		/// �z��̗v�f�̋L�^�Ɋւ������񋟂��܂��B
		/// </summary>
		public ReadWriteMemberAttribute ChildElement{
			get{return this.child_attr;}
			internal set{this.child_attr=value;}
		}
		/// <summary>
		/// ReadWriteArrayAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="lengthType">�z��v�f�̐��̌�����@���w�肵�܂��B</param>
		public ReadWriteArrayAttribute(ArrayLengthType lengthType):base(){
			this.lentype=lengthType;
		}
		/// <summary>
		/// ReadWriteAsArray �̃R���X�g���N�^�ł��B
		/// �z��v�f�̐��́A�z��v�f�����z��f�[�^�̐擪�ɋL�^����Ă��鎖��z�肵�܂��B
		/// </summary>
		public ReadWriteArrayAttribute():this(ArrayLengthType.CountEmbedded){}

		//=================================================
		//		�z��̒���
		//=================================================
		private ArrayLengthType lentype=ArrayLengthType.CountEmbedded;
		/// <summary>
		/// �z��̒����̎w����@���擾���͐ݒ肵�܂��B����l�� <see cref="F:LenType.CountEmbedded"/> �ł��B
		/// </summary>
		public ArrayLengthType ArrayLengthType{
			get{return this.lentype;}
			set{this.lentype=value;}
		}
		//-------------------------------------------------
		//		�v�f�̌�
		//-------------------------------------------------
		private string count=null;
		/// <summary>
		/// �z��̗v�f�̌����w�肵�܂��B
		/// �����𕶎���Ŏw�肷�邩�A�z��̗v�f�̌���ێ�����t�B�[���h�̖��O�A
		/// ���͔z��̗v�f�̌����擾����ׂ̃v���p�e�B�̖��O���w�肵�܂��B
		/// </summary>
		public string Count{
			get{return this.count;}
			set{
				this.count=value;
				if(value!=null&&value!=""){
					this.size=null;
				}
			}
		}
		/// <summary>
		/// �v�f�̐������肵�܂��B
		/// </summary>
		/// <returns>�ǂݏo���v�f�̐���Ԃ��܂��B</returns>
		private int GetCount(ReadWriteMemberArgs args){
			const string ERR_COUNT_NOSPEC="CountSpecified ���w�肳��Ă���ɂ��S��炸 count �̒l���ݒ肳��Ă��܂���B";
			const string ERR_COUNT_READ="�v�f���̎擾�Ɏ��s���܂����B�v�f���� Count �v���p�e�B��ʂ��Đݒ肵�܂��B\r\n"
				+"���ڐ��l�Ŏw�肷��ꍇ�ɂ� int.Parse �œǂݎ���l���w�肵�ĉ������B\r\n"
				+"���̃t�B�[���h���̓v���p�e�B���Q�Ƃ���ꍇ�ɂ͂��̃����o�̖��O���w�肵�ĉ������B";
			const string ERR_COUNT_NEGATIVE="�w�肵���v�f���͕��̐��ł��B���̐��̗v�f��ǂݎ�鎖�͏o���܂���B";
			//-------------------------------------
			if(this.count==null||this.count.Length==0)
				throw new System.ArgumentNullException(ERR_COUNT_NOSPEC);
			int c;
			try{
				c=int.Parse(this.count);
				goto check;
			}catch{}
			try{
				c=args.GetValue<int>(this.count);
				goto check;
			}catch{}
			throw new System.ApplicationException(ERR_COUNT_READ);
		check:
			if(c<0)throw new System.ArgumentOutOfRangeException("Count",ERR_COUNT_NEGATIVE);
			return c;
		}
		//-------------------------------------------------
		//		�f�[�^�̃T�C�Y
		//-------------------------------------------------
		private string size=null;
		/// <summary>
		/// �z��̃o�C�i���T�C�Y���w�肵�܂��B
		/// �����𕶎���Ŏw�肷�邩�A�z��̃o�C�i���T�C�Y��ێ�����t�B�[���h�̖��O�A
		/// ���͔z��̃o�C�i���T�C�Y���擾����ׂ̃v���p�e�B�̖��O���w�肵�܂��B
		/// </summary>
		public string Size{
			get{return this.size;}
			set {
				this.size=value;
				if(value!=null&&value!="") {
					this.count=null;
				}
			}
		}
		/// <summary>
		/// �f�[�^�̃T�C�Y�����肵�܂��B
		/// </summary>
		/// <returns>�ǂݏo���f�[�^�T�C�Y��Ԃ��܂��B</returns>
		private long GetSize(ReadWriteMemberArgs args) {
			const string ERR_SIZE_NOSPEC="SizeSpecified ���w�肳��Ă���ɂ��S��炸 size �̒l���ݒ肳��Ă��܂���B";
			const string ERR_SIZE_READ="�f�[�^�̑傫���̎擾�Ɏ��s���܂����B�f�[�^�̑傫���� Size �v���p�e�B��ʂ��Đݒ肵�܂��B\r\n"
				+"���ڐ��l�Ŏw�肷��ꍇ�ɂ� long.Parse �œǂݎ���l���w�肵�ĉ������B\r\n"
				+"���̃t�B�[���h���̓v���p�e�B���Q�Ƃ���ꍇ�ɂ͂��̃����o�̖��O���w�肵�ĉ������B";
			const string ERR_SIZE_NEGATIVE="�w�肵���f�[�^�T�C�Y�͕��̐��ł��B���̃T�C�Y�̃f�[�^��ǂݎ�鎖�͏o���܂���B";
			//-------------------------------------
			if(this.size==null||this.size.Length==0)
				throw new System.ArgumentNullException(ERR_SIZE_NOSPEC);
			long c;
			try{
				c=long.Parse(this.size);
				goto check;
			}catch{}
			try{
				c=args.GetValue<long>(this.size);
				goto check;
			}catch{}
			throw new System.ApplicationException(ERR_SIZE_READ);
		check:
			if(c<0) throw new System.ArgumentOutOfRangeException("Size",ERR_SIZE_NEGATIVE);
			return c;
		}
		//=================================================
		//		�ǂݏ���
		//=================================================
		/// <summary>
		/// �w�肵���^�̒l�� StreamAccessor ��ʂ��ēǂݎ��܂��B
		/// </summary>
		/// <param name="args">�Ǎ�����E�Ǎ��ΏۂɊւ�������w�肵�܂��B</param>
		protected internal override void Read(ReadMemberArgs args){
			System.Type type=args.TargetType;
			StreamAccessor accessor=args.Accessor;

			System.Type elemType=GetElementType(args.TargetType);

			int c;
			long s;
			System.Collections.ArrayList list;
			ReadElementArgs child_args;
			bool raise_error=true;
			switch(this.lentype){
				case ArrayLengthType.CountEmbedded:
					c=args.Accessor.ReadInt32(EncodingType.I4);
					goto count;
				case ArrayLengthType.CountSpecified:
					c=this.GetCount(args);
					goto count;
				case ArrayLengthType.SizeSpecified:
					s=this.GetSize(args);
					goto size;
				case ArrayLengthType.SizeEmbedded:
					s=args.Accessor.ReadInt32(EncodingType.I4);
					goto size;
				case ArrayLengthType.ReadToEnd:
					try{
						s=this.GetSize(args);
						goto size;
					}catch(System.ArgumentNullException){}

					try{
						c=this.GetCount(args);
						raise_error=false;
						goto count;
					}catch(System.ArgumentNullException){}

					list=new System.Collections.ArrayList();
					child_args=new ReadElementArgs(args.ParentObject,elemType,args.Accessor,list);
					goto endless;
				count:
					list=new System.Collections.ArrayList(c);
					child_args=new ReadElementArgs(args.ParentObject,elemType,args.Accessor,list);
					try{
						for(int i=0;i<c;i++)this.child_attr.Read(child_args);
					}catch(System.Exception e){
						afh.File.__dll__.log.WriteError(e);
						if(raise_error)throw;
					}
					args.SetValueToTarget(ToCollections(args,list));
					break;
				size:
					StreamAccessor child_accessor=new StreamAccessor(args.Accessor.ReadSubStream(s));
					list=new System.Collections.ArrayList();
					child_args=new ReadElementArgs(args.ParentObject,elemType,child_accessor,list);
					goto endless;
				endless:
					try{
						while(args.Accessor.RestLength>0)this.child_attr.Read(child_args);
					}catch(StreamOverRunException){}
					args.SetValueToTarget(ToCollections(args,list));
					break;
				default:
					throw new System.ApplicationException("������");
			}
		}
		private static object ToCollections(ReadMemberArgs args,System.Collections.ArrayList list){
			System.Type type=args.TargetType;
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("�񎟌��ȏ�̔z��ɂ͖��Ή��ł��B�W���O�z��ɂ���p�����l���������B");
				return list.ToArray(type.GetElementType());
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("���̉����Ă��Ȃ��W�F�l���b�N�N���X�̃C���X�^���X�͍쐬�ł��܂���B");
				System.Type typedef=type.GetGenericTypeDefinition();
				switch(type.Name[0]){
					case 'L':
						if(typedef==typeof(Gen::List<>)){
							return list.ToArray(type.GetGenericArguments()[0]);
						}else break;
					case 'S':
						if(typedef==typeof(Gen::Stack<>)){
							list.Reverse();
							object ret=type.GetConstructor(new System.Type[]{typeof(int)}).Invoke(new object[]{list.Count});
							System.Reflection.MethodInfo meth=type.GetMethod("Push");
							foreach(object item in list){
								meth.Invoke(ret,new object[]{item});
							}
							return ret;
						}else break;
					case 'Q':
						if(typedef==typeof(Gen::Queue<>)){
							object ret=type.GetConstructor(new System.Type[]{typeof(int)}).Invoke(new object[]{list.Count});
							System.Reflection.MethodInfo meth=type.GetMethod("Enqueue");
							foreach(object item in list){
								meth.Invoke(ret,new object[]{item});
							}
							return ret;
						}else break;
					case 'I':
						if(typedef==typeof(Gen::IList<>)||typedef==typeof(Gen::ICollection<>)||typedef==typeof(Gen::IEnumerable<>)){
							return list.ToArray(type.GetGenericArguments()[0]);
						}else break;
				}
			}else{
				switch(type.Name[0]){
					case 'A':
						if(type==typeof(System.Collections.ArrayList)){
							return list;
						}else break;
					case 'S':
						if(type==typeof(System.Collections.Stack)){
							list.Reverse();
							return new System.Collections.Stack(list);
						}else break;
					case 'Q':
						if(type==typeof(System.Collections.Queue)){
							return new System.Collections.Queue(list);
						}else break;
				}
			}
			throw new System.InvalidOperationException("�w�肵���^��z��Ƃ��ēǂݎ����@�͒񋟂���Ă��܂���B");
		}
		private static System.Type GetElementType(System.Type type){
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("�񎟌��ȏ�̔z��ɂ͖��Ή��ł��B�W���O�z��ɂ���p�����l���������B");
				return type.GetElementType();
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("���̉����Ă��Ȃ��W�F�l���b�N�N���X�̃C���X�^���X�͍쐬�ł��܂���B");
				System.Type typedef=type.GetGenericTypeDefinition();
				switch(type.Name[0]){
					case 'L':
						if(typedef==typeof(Gen::List<>)){
							goto ret_generic0;
						} else break;
					case 'S':
						if(typedef==typeof(Gen::Stack<>)){
							goto ret_generic0;
						}else break;
					case 'Q':
						if(typedef==typeof(Gen::Queue<>)){
							goto ret_generic0;
						}else break;
					case 'I':
						if(typedef==typeof(Gen::IList<>)||typedef==typeof(Gen::ICollection<>)||typedef==typeof(Gen::IEnumerable<>)){
							goto ret_generic0;
						} else break;
					ret_generic0:
						return type.GetGenericArguments()[0];
				}
			}else{
				switch(type.Name[0]){
					case 'A':
						if(type==typeof(System.Collections.ArrayList)){
							return typeof(object);
						}else break;
					case 'S':
						if(type==typeof(System.Collections.Stack)) {
							return typeof(object);
						}else break;
					case 'Q':
						if(type==typeof(System.Collections.Queue)) {
							return typeof(object);
						}else break;
				}
			}
			throw new System.InvalidOperationException("�w�肵���^��z��Ƃ��ēǂݎ����@�͒񋟂���Ă��܂���B");
		}
		/// <summary>
		/// �w�肵���l���^�ɉ��������@�� StreamAccessor ��ʂ��� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="args">��������E�����ΏۂɊւ�������w�肵�܂��B</param>
		protected internal override void Write(WriteMemberArgs args){
			StreamAccessor accessor=args.Accessor;

			System.Type elemType=GetElementType(args.TargetType);

			int c;
			
			if(args.Value==null)
				throw new System.ApplicationException();
			System.Collections.IEnumerable value=args.Value as System.Collections.IEnumerable;
			if(value==null){
				throw new System.ApplicationException(
					string.Format("�w�肵���^ {0} �̒l��z��Ƃ��ď������ގ��͏o���܂���B",args.Value.GetType().FullName)
					);
			}

			StreamAccessor substream;
			WriteElementArgs child_args;
			switch(this.lentype) {
				case ArrayLengthType.CountEmbedded:
					substream=new StreamAccessor(args.Accessor.WriteSubStream(4)); // ��Œ������������ވ�
					c=0;
					child_args=new WriteElementArgs(args.ParentObject,elemType,args.Accessor,null);
					foreach(object item in value){
						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
						c++;
					}
					substream.Write(c,EncodingType.I4);
					break;
				case ArrayLengthType.CountSpecified:
					c=this.GetCount(args);
					child_args=new WriteElementArgs(args.ParentObject,elemType,args.Accessor,null);
					foreach(object item in value){
						if(0==c--)throw new System.ApplicationException("�R���N�V�������̗v�f�̐����A�w�肵���v�f�����z���Ă��܂��B�������v�f�����w�肵�ĉ������B");

						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
					}
					if(c>0)throw new System.ApplicationException("�R���N�V�������̗v�f�����A�w�肵���v�f���ɖ����܂���B�������v�f�����w�肵�ĉ������B");
					break;
				case ArrayLengthType.SizeEmbedded:
					substream=new StreamAccessor(args.Accessor.WriteSubStream(4)); // ��Œ������������ވ�
					c=checked((int)args.Accessor.Position);
					child_args=new WriteElementArgs(args.ParentObject,elemType,args.Accessor,null);
					foreach(object item in value){
						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
					}
					substream.Write(args.Accessor.Position-c,EncodingType.I4);
					substream.Stream.Close();
					break;
				case ArrayLengthType.SizeSpecified:
					substream=new StreamAccessor(args.Accessor.WriteSubStream(this.GetSize(args)));
					child_args=new WriteElementArgs(args.ParentObject,elemType,substream,null);
					foreach(object item in value){
						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
					}
					substream.Stream.Close();
					break;
				case ArrayLengthType.ReadToEnd:
					child_args=new WriteElementArgs(args.ParentObject,elemType,args.Accessor,null);
					foreach(object item in value){
						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
					}
					break;
				default:
					throw new System.ApplicationException("������");
			}
		}
#if OBSOLETE
		[System.Obsolete]
		private static System.Collections.ICollection FromCollection(System.Type type,object value){
			//const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.InvokeMethod|System.Reflection.BindingFlags.Public;
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("�񎟌��ȏ�̔z��ɂ͖��Ή��ł��B�W���O�z��ɂ���p�����l���������B");
				return (System.Array)value;
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("���̉����Ă��Ȃ��W�F�l���b�N�N���X�̃C���X�^���X�͍쐬�ł��܂���B");
				System.Type typedef=type.GetGenericTypeDefinition();
				switch(type.Name[0]){
					case 'L':
						if(typedef==typeof(Gen::List<>)){
							goto ret_generic0;
							//return (System.Collections.ICollection)typedef.InvokeMember("ToArray",BF,null,value,new object[0]);
						} else break;
					case 'S':
						if(typedef==typeof(Gen::Stack<>)){
							goto ret_generic0;
						}else break;
					case 'Q':
						if(typedef==typeof(Gen::Queue<>)){
							goto ret_generic0;
						}else break;
					case 'I':
						if(typedef==typeof(Gen::IList<>)||typedef==typeof(Gen::ICollection<>)){
							goto ret_generic0;
						}else if(typedef==typeof(Gen::IEnumerable<>)){
							System.Collections.ArrayList list=new System.Collections.ArrayList();
							foreach(object item in value as System.Collections.IEnumerable)list.Add(item);
							return list;
						}else break;
					ret_generic0:
						return new CollectionAdapter(value);
				}
			}else{
				switch(type.Name[0]){
					case 'A':
						if(type==typeof(System.Collections.ArrayList)){
							return (System.Collections.ArrayList)value;
						}else break;
					case 'S':
						if(type==typeof(System.Collections.Stack)) {
							return (System.Collections.Stack)value;
						}else break;
					case 'Q':
						if(type==typeof(System.Collections.Queue)) {
							return (System.Collections.Queue)value;
						}else break;
				}
			}
			throw new System.InvalidOperationException("�w�肵���^��z��Ƃ��ēǂݎ����@�͒񋟂���Ă��܂���B");
		}
		/// <summary>
		/// System.Collections.Generic.ICollection`1 ���� System.Collections.ICollection 
		/// </summary>
		[System.Obsolete]
		private sealed class CollectionAdapter:System.Collections.ICollection{
			object collection;
			private const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public;
			private static System.Reflection.MethodInfo _CopyTo=typeof(Gen::ICollection<>).GetMethod("CopyTo",BF);
			private static System.Reflection.MethodInfo _get_Count=typeof(Gen::ICollection<>).GetMethod("get_Count",BF);
			private static System.Reflection.MethodInfo _GetEnumerator=typeof(Gen::ICollection<>).GetMethod("GetEnumerator",BF);

			public CollectionAdapter(object collection){
				if(collection.GetType().GetGenericTypeDefinition()==typeof(Gen::ICollection<>)){
					this.collection=collection;
				}else throw new System.ArgumentException("�w�肵���^�� System.Collections.ICollection �ɐڑ��ł��܂���B");
			}

			public void CopyTo(System.Array array,int index){
				_CopyTo.Invoke(collection,new object[]{array,index});
			}
			public int Count {
				get {return (int)_get_Count.Invoke(collection,new object[0]);}
			}
			public bool IsSynchronized {
				get {return false;}
			}

			private object syncroot=new object();
			public object SyncRoot{
				get {return this.syncroot;}
			}

			public System.Collections.IEnumerator GetEnumerator() {
				return (System.Collections.IEnumerator)_GetEnumerator.Invoke(collection,new object[0]);
			}
		}
#endif

	}
	#endregion
}

namespace afh.File.Design{

	/// <summary>
	/// �����o�̓ǂݏ����̕��@���w�肷�鑮���̊�{�N���X�ł��B
	/// </summary>
	public abstract class ReadWriteMemberAttribute:System.Attribute{
		protected ReadWriteMemberAttribute(){}
		/// <summary>
		/// �Ǎ��������m�F���ēǍ������s���܂��B
		/// </summary>
		/// <param name="args">�Ǎ��Ɏg�p��������w�肵�܂��B</param>
		internal void ReadMember(ReadMemberArgs args){
			if(readCondition!=null&&readCondition!=""){
				if(!args.GetValue<bool>(readCondition))return;
			}
			this.Read(args);
		}
		internal void Read(object o,System.Reflection.MemberInfo info,StreamAccessor accessor){
			this.ReadMember(new ReadMemberArgs(o,info,accessor));
		}
		/// <summary>
		/// �����������m�F���ď��������s���܂��B
		/// </summary>
		/// <param name="args">�����Ɏg�p��������w�肵�܂��B</param>
		internal void WriteMember(WriteMemberArgs args){
			if(writeCondition!=null&&writeCondition!=""){
				if(!args.GetValue<bool>(writeCondition))return;
			}
			this.Write(args);
		}
		internal void Write(object o,System.Reflection.MemberInfo info,StreamAccessor accessor){
			this.WriteMember(new WriteMemberArgs(o,info,accessor));
		}
		/// <summary>
		/// �w�肵���^�̒l��ǂݎ��܂��B�ڍׂȎ����Ɋւ��Ă� <see cref="ReadMemberArgs"/> ���Q�Ƃ��ĉ������B
		/// </summary>
		/// <param name="args">�Ǎ�����E�Ǎ��ΏۂɊւ�������w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		protected internal abstract void Read(ReadMemberArgs args);
		/// <summary>
		/// �w�肵���l�����̌^�ɍ��������@���g�p���ď������݂܂��B�ڍׂȎ����Ɋւ��Ă� <see cref="WriteMemberArgs"/> ���Q�Ƃ��ĉ������B
		/// </summary>
		/// <param name="args">��������E�����ΏۂɊւ�������w�肵�܂��B</param>
		protected internal abstract void Write(WriteMemberArgs args);

		private string readCondition="";
		private string writeCondition="";

		/// <summary>
		/// �����o��ǂݏ�������������Abool �l���������o�Ŏw�肵�܂��B
		/// </summary>
		public string Condition{
			get{return readCondition==writeCondition?readCondition:"";}
			set{
				this.readCondition=value;
				this.writeCondition=value;
			}
		}
		/// <summary>
		/// �����o��ǂݍ��ޏ������Abool �l���������o�Ŏw�肵�܂��B
		/// </summary>
		public string ConditionToRead{
			get{return this.readCondition;}
			set{this.readCondition=value;}
		}
		/// <summary>
		/// �����o���������ޏ������Abool �l���������o�Ŏw�肵�܂��B
		/// </summary>
		public string ConditionToWrite{
			get{return this.writeCondition;}
			set{this.writeCondition=value;}
		}
#if OBSOLETE
		[System.Obsolete]
		protected static void GetValue(out object value,object o,string memberName,System.Type requestedType){
			const string ERR_CONDITION="�w�肵�����O�̃t�B�[���h���̓v���p�e�B��������܂���ł����B���݂���t�B�[���h������ (��������) �v���p�e�B�����w�肵�ĉ������B";
			const string ERR_NOT_REQUESTEDTYPEL="�w�肵�����O�̃����o�͗v�����ꂽ�^�ł͂���܂���B�v�����Ă���^�̃t�B�[���h/�v���p�e�B�����w�肵�ĉ������B";
			//-----------------------
			System.Type parentType=o.GetType();
			try{
				GetValue(out value,o,GetMemberInfo(parentType,memberName));
			}catch(System.Exception e){
				throw new System.ApplicationException(ERR_CONDITION,e);
			}
			if(!requestedType.IsAssignableFrom(value.GetType()))
				throw new System.ApplicationException(ERR_NOT_REQUESTEDTYPEL);
		}
		[System.Obsolete]
		protected static void GetValue(out object value,object o,System.Reflection.MemberInfo info){
			if(info.MemberType==System.Reflection.MemberTypes.Field){
				value=((System.Reflection.FieldInfo)info).GetValue(o);
				return;
			}else if(info.MemberType==System.Reflection.MemberTypes.Property){
				System.Reflection.PropertyInfo pinfo=(System.Reflection.PropertyInfo)info;
				if(pinfo.GetIndexParameters().Length==0&&pinfo.CanRead){
					value=pinfo.GetValue(o,new object[]{});
					return;
				}
			}
			throw new System.ArgumentException("�w�肵�������o����l���擾���鎖�͏o���܂���B","info");
		}
		[System.Obsolete]
		protected static void SetValue(object value,object o,System.Reflection.MemberInfo info){
			if(info.MemberType==System.Reflection.MemberTypes.Field) {
				((System.Reflection.FieldInfo)info).SetValue(o,value);
				return;
			} else if(info.MemberType==System.Reflection.MemberTypes.Property) {
				System.Reflection.PropertyInfo pinfo=(System.Reflection.PropertyInfo)info;
				if(pinfo.GetIndexParameters().Length==0&&pinfo.CanWrite) {
					pinfo.SetValue(o,value,new object[]{});
					return;
				}
			}
			throw new System.ArgumentException("�w�肵�������o�ɒl��ݒ肷�鎖�͏o���܂���B","info");
		}
		/// <summary>
		/// �w�肵�����O�����ǂݏ����o���郁���o�Ɋւ�������擾���܂��B
		/// </summary>
		/// <param name="parentType">�����o��ێ�����^���w�肵�܂��B</param>
		/// <param name="memberName">�����o�̖��O���w�肵�܂��B</param>
		/// <returns>�擾���������o����Ԃ��܂��B</returns>
		/// <exception cref="System.MissingFieldException">�K������t�B�[���h���̓v���p�e�B��������Ȃ������ꍇ�ɔ������܂��B</exception>
		/// <exception cref="System.Reflection.AmbiguousMatchException">�K������t�B�[���h���̓v���p�e�B���������݂���ꍇ�ɔ������܂��B</exception>
		[System.Obsolete]
		protected static System.Reflection.MemberInfo GetMemberInfo(System.Type parentType,string memberName){
			const System.Reflection.BindingFlags BF
				=System.Reflection.BindingFlags.Public
				|System.Reflection.BindingFlags.NonPublic
				|System.Reflection.BindingFlags.Instance
				|System.Reflection.BindingFlags.Static;
			const System.Reflection.MemberTypes MT
				=System.Reflection.MemberTypes.Field
				|System.Reflection.MemberTypes.Property;
			System.Reflection.MemberInfo[] infos=parentType.GetMember(memberName,MT,BF);
			int c=0;
			System.Reflection.MemberInfo ret=null;
			if(infos.Length>0)for(int i=0;i<infos.Length;i++){
				if(
					infos[i].MemberType==System.Reflection.MemberTypes.Field||
					infos[i].MemberType==System.Reflection.MemberTypes.Property
					&&((System.Reflection.PropertyInfo)infos[i]).GetIndexParameters().Length==0
				){
					c++;
					ret=infos[i];
				}
			}
			if(c==0)throw new System.MissingFieldException(parentType.FullName,memberName);
			if(c>0)throw new System.Reflection.AmbiguousMatchException();
			return ret;
		}
		[System.Obsolete]
		protected static System.Type GetTypeOfMember(System.Reflection.MemberInfo info){
			switch(info.MemberType){
				case System.Reflection.MemberTypes.Field:
					return ((System.Reflection.FieldInfo)info).FieldType;
				case System.Reflection.MemberTypes.Property:
					return ((System.Reflection.PropertyInfo)info).PropertyType;
				case System.Reflection.MemberTypes.Method:
					return ((System.Reflection.MethodInfo)info).ReturnType;
				default:
					throw new System.ApplicationException("�w�肵����ނ̃����o�̌^�͎擾�o���܂���B");
			}
		}
#endif
	}

	#region args:ReadWriteMember
	/// <summary>
	/// �v�����ꂽ�v�f�ǂݏo������E�Ǎ��ΏۂɊւ������񋟂��܂��B
	/// </summary>
	public sealed class ReadElementArgs:ReadMemberArgs{
		/// <summary>
		/// ReadElementArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�ǂݏ������郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="type">�ǂݏ���������̌^���w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		/// <param name="setvalue">�ǂݍ��񂾒l��񍐂������w�肵�܂��B
		/// �����o�̗v�f�̓ǂݎ��ɉ����ẮA�ǂݎ��ꂽ�l�𒼐ڐe�I�u�W�F�N�g�̃����o�Ɋi�[���鎖���o���Ȃ��̂ŁA
		/// �����Ɏ󂯎��p�̊֐����w�肷��K�v������̂ł��B</param>
		public ReadElementArgs(object parent,System.Type type,StreamAccessor accessor,afh.CallBack<object> setvalue):base(parent,null,accessor){
			this.type=type;
			this.setvalue=setvalue;
		}
		/// <summary>
		/// ReadElementArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�ǂݏ������郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="type">�ǂݏ���������̌^���w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		/// <param name="list">�ǂݍ��񂾒l���i�[���� System.Collections.ArrayList ���w�肵�܂��B</param>
		public ReadElementArgs(object parent,System.Type type,StreamAccessor accessor,System.Collections.ArrayList list):base(parent,null,accessor){
			this.type=type;
			this.setvalue=SetValueToList;
			this.list=list;
		}
		/// <summary>
		/// �ǂݎ�����l��ݒ肷���̃����o�̏����擾����ׂ̕��ł����A�v�f�ǂݏo���ł͎g�p���܂���B
		/// </summary>
		public override System.Reflection.MemberInfo TargetInfo {
			get{throw new System.InvalidOperationException("�v�f�ǂݏo���ł͑Ή����郁���o�͑��݂��܂���B");}
		}
		private System.Type type;
		/// <summary>
		/// �ǂݎ��v�f�̌^���擾���܂��B
		/// </summary>
		public override System.Type TargetType {
			get {return this.type;}
		}

		private afh.CallBack<object> setvalue;
		public override void SetValueToTarget(object value) {
			setvalue(value);
		}

		private System.Collections.ArrayList list;
		private void SetValueToList(object value){this.list.Add(value);}
	}
	/// <summary>
	/// �v�����ꂽ�v�f��������E�����ΏۂɊւ������񋟂��܂��B
	/// </summary>
	public sealed class WriteElementArgs:WriteMemberArgs{
		/// <summary>
		/// WriteElementArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�������ޗv�f���i�[���Ă��郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="type">�������ޗv�f�̌^���w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����̑Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		/// <param name="element">�������ޗv�f�̒l���w�肵�܂��B</param>
		public WriteElementArgs(object parent,System.Type type,StreamAccessor accessor,object element):base(parent,null,accessor){
			this.type=type;
			this.value=element;
		}

		/// <summary>
		/// �������ޒl���擾���錳�̃����o�̏����擾����ׂ̕��ł����A�v�f�����ł͎g�p���܂���B
		/// </summary>
		public override System.Reflection.MemberInfo TargetInfo {
			get{throw new System.InvalidOperationException("�v�f�����ł͑Ή����郁���o�͑��݂��܂���B"); }
		}
		private System.Type type;
		/// <summary>
		/// �������ޗv�f�̌^���擾���܂��B
		/// </summary>
		public override System.Type TargetType{
			get{return this.type;}
		}

		private object value;
		/// <summary>
		/// �X�g���[���ɏ������ވׂ̒l��Ώۃ����o����ǂݎ��܂��B
		/// </summary>
		public override object Value {
			 get{return this.value;}
		}
		internal void init_ElementValue(object value){
			this.value=value;
		}
	}
	/// <summary>
	/// �v�����ꂽ�Ǎ�����E�Ǎ��ΏۂɊւ������ێ����܂��B
	/// </summary>
	/// <remarks>
	/// ReadWriteMemberAttribute::Read �֐��̈����Ƃ��ēn���ꂽ�ꍇ�ɂ́A
	/// �A�N�Z�T��ʂ��ăX�g���[������K���ȏ���ǂݏo���āA�l�𐶐����A
	/// ���̒l�� SetValueToTarget �őΏۃ����o�ɔ��f�����܂��B
	/// <pre>--�Ǎ�����̐}--
	/// �e�I�u�W�F�N�g [<see cref="P:Parent"/>, <see cref="P:ParentInfo"/>]
	/// ���Ώۃ����o [<see cref="P:TargetInfo"/>, <see cref="P:TargetType"/>]
	///    ��
	///  �����A�N�Z�T [<see cref="P:Accessor"/>] ��
	///  ������ �X�g���[��       ��
	///  ��������������������������
	/// </pre>
	/// </remarks>
	public class ReadMemberArgs:ReadWriteMemberArgs{
		/// <summary>
		/// ReadMemberArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�ǂݏ������郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="member">�ǂݏ����̑Ώۂ̃����o�Ɋւ�������w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����̑Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		public ReadMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor) : base(parent,member,accessor) { }

		/// <summary>
		/// �ǂݎ�����l��Ώۃ����o�ɔ��f�����܂��B
		/// </summary>
		/// <param name="value">�X�g���[������ǂݎ�����l���w�肵�܂��B</param>
		public virtual void SetValueToTarget(object value){
			this.SetValue(this.TargetInfo,value);
		}
	}
	/// <summary>
	/// �v�����ꂽ�������ݑ���E�������ݑΏۂɊւ������ێ����܂��B
	/// </summary>
	/// <remarks>
	/// <pre>--��������̐}--
	/// �e�I�u�W�F�N�g [<see cref="P:Parent"/>, <see cref="P:ParentInfo"/>]
	/// ���Ώۃ����o [<see cref="P:TargetInfo"/>, <see cref="P:TargetType"/>]
	///    ��
	///  �����A�N�Z�T [<see cref="P:Accessor"/>] ��
	///  ������ �X�g���[��       ��
	///  ��������������������������
	/// </pre>
	/// </remarks>
	public class WriteMemberArgs:ReadWriteMemberArgs{
		/// <summary>
		/// WriteMemberArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�ǂݏ������郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="member">�ǂݏ����̑Ώۂ̃����o�Ɋւ�������w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����̑Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		public WriteMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor) : base(parent,member,accessor) { }

		/// <summary>
		/// �X�g���[���ɏ������ވׂ̒l��Ώۃ����o����ǂݎ��܂��B
		/// </summary>
		public virtual object Value{
			get{return this.GetValue(this.TargetInfo);}
		}
	}
	/// <summary>
	/// �ǂݏ����̑ΏۂɊւ������ێ����A���A�Ώۂɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public class ReadWriteMemberArgs:System.EventArgs{
		/// <summary>
		/// ReadWriteMemberArgs �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="parent">�ǂݏ������郁���o��ێ�����e�I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="member">�ǂݏ����̑Ώۂ̃����o�Ɋւ�������w�肵�܂��B</param>
		/// <param name="accessor">�ǂݏ����̑Ώۂ� Stream �ɑ΂���A�N�Z�X��񋟂��� StreamAccessor ���w�肵�܂��B</param>
		public ReadWriteMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor){
			this.parent=parent;
			this.member=member;
			this.accessor=accessor;
		}
		//=======================================
		//		�e�I�u�W�F�N�g
		//=======================================
		private object parent;
		/// <summary>
		/// �ǂݏ������郁���o��ێ�����I�u�W�F�N�g���擾���܂��B
		/// </summary>
		public object ParentObject{get{return parent;}}
		/// <summary>
		/// �ǂݏ������郁���o��ێ�����^���擾���܂��B
		/// </summary>
		public System.Type ParentType{
			get{return this.parent.GetType();}
		}
		//=======================================
		//		�e�̃����o�ւ̃A�N�Z�X
		//=======================================
		/// <summary>
		/// �ǂݏ����̑ΏۂƂȂ郁���o�Ɋւ�������擾���܂��B
		/// �ǂݏ����̑ΏۂƂȂ郁���o�Ƃ́A�t�B�[���h���͈��������v���p�e�B�Ɉׂ�܂��B
		/// </summary>
		/// <param name="memberName">�������郁���o�̖��O���w�肵�܂��B</param>
		/// <returns>�擾���������o�̏���Ԃ��܂��B</returns>
		/// <exception cref="System.MissingFieldException">
		/// �w�肵�����O�����A�ǂݏ����̑ΏۂƂ��ĉ\�ȃ����o��������Ȃ������ꍇ�ɔ������܂��B</exception>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// �w�肵�����O�����A�ǂݏ����̑ΏۂƂ��ĉ\�ȃ����o������������A��ɍi�鎖���o�����������ꍇ�ɔ������܂��B</exception>
		public System.Reflection.MemberInfo GetMemberInfo(string memberName){
			const System.Reflection.BindingFlags BF
				=System.Reflection.BindingFlags.Public
				|System.Reflection.BindingFlags.NonPublic
				|System.Reflection.BindingFlags.Instance
				|System.Reflection.BindingFlags.Static;
			const System.Reflection.MemberTypes MT
				=System.Reflection.MemberTypes.Field
				|System.Reflection.MemberTypes.Property;
			System.Reflection.MemberInfo[] infos=this.ParentType.GetMember(memberName,MT,BF);
			int c=0;
			System.Reflection.MemberInfo ret=null;
			if(infos.Length>0)for(int i=0;i<infos.Length;i++){
				if(
					infos[i].MemberType==System.Reflection.MemberTypes.Field||
					infos[i].MemberType==System.Reflection.MemberTypes.Property
					&&((System.Reflection.PropertyInfo)infos[i]).GetIndexParameters().Length==0
				){
					c++;
					ret=infos[i];
				}
			}
			if(c==0)throw new System.MissingFieldException(this.ParentType.FullName,memberName);
			if(c>1)throw new System.Reflection.AmbiguousMatchException();
			return ret;
		}
		/// <summary>
		/// �w�肵�����O�̃����o��ǂݎ��܂��B
		/// </summary>
		/// <param name="memberName">�ǂݎ�郁���o�̖��O���w�肵�܂��B</param>
		/// <typeparam name="T">�ǂݎ��l�̌^�Ƃ��ėv�����镨���w�肵�܂��B</typeparam>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		public T GetValue<T>(string memberName){
			const string ERR_NOT_REQUESTEDTYPE="�w�肵�����O�̃����o�͗v�����ꂽ�^�ł͂���܂���B�v�����Ă���^�̃t�B�[���h/�v���p�e�B�����w�肵�ĉ������B";
			//-----------------------
			object value=GetValue(memberName);
			if(!(value is T))throw new System.ApplicationException(ERR_NOT_REQUESTEDTYPE);
			return (T)value;
		}
		/// <summary>
		/// �w�肵�����O�̃����o��ǂݎ��܂��B
		/// </summary>
		/// <param name="memberName">�ǂݎ�郁���o�̖��O���w�肵�܂��B</param>
		/// <param name="requestedType">�����o�̌^�Ƃ��ėv�����镨���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		public object GetValue(string memberName,System.Type requestedType) {
			const string ERR_NOT_REQUESTEDTYPE="�w�肵�����O�̃����o�͗v�����ꂽ�^�ł͂���܂���B�v�����Ă���^�̃t�B�[���h/�v���p�e�B�����w�肵�ĉ������B";
			//-----------------------
			object r=GetValue(memberName);
			if(!requestedType.IsAssignableFrom(r.GetType()))
				throw new System.ApplicationException(ERR_NOT_REQUESTEDTYPE);
			return r;
		}
		/// <summary>
		/// �w�肵�����O�̃����o��ǂݎ��܂��B
		/// </summary>
		/// <param name="memberName">�ǂݎ�郁���o�̖��O���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		public object GetValue(string memberName) {
			const string ERR_CONDITION="�w�肵�����O�̃t�B�[���h���͈��������v���p�e�B��������܂���ł����B���݂���t�B�[���h�����͈��������v���p�e�B�����w�肵�ĉ������B";
			//-----------------------
			try{
				return GetValue(GetMemberInfo(memberName));
			}catch(System.Exception e) {
				throw new System.ApplicationException(ERR_CONDITION,e);
			}
		}
		/// <summary>
		/// �e�I�u�W�F�N�g (<see cref="P:Parent"/>) �̎w�肵�������o����l��ǂݏo���܂��B
		/// </summary>
		/// <param name="info">�l�̓Ǎ����̃����o�̏����w�肵�܂��B</param>
		/// <returns>�ǂݎ�����l��Ԃ��܂��B</returns>
		public object GetValue(System.Reflection.MemberInfo info) {
			if(info.MemberType==System.Reflection.MemberTypes.Field){
				return ((System.Reflection.FieldInfo)info).GetValue(this.parent);
			}else if(info.MemberType==System.Reflection.MemberTypes.Property){
				System.Reflection.PropertyInfo pinfo=(System.Reflection.PropertyInfo)info;
				if(pinfo.GetIndexParameters().Length==0&&pinfo.CanRead){
					return pinfo.GetValue(this.parent,new object[]{});
				}
			}
			throw new System.ArgumentException("�w�肵�������o����l���擾���鎖�͏o���܂���B","info");
		}
		/// <summary>
		/// �e�I�u�W�F�N�g�̎w�肵�������o�ɒl��ݒ肵�܂��B
		/// </summary>
		/// <param name="info">�������ݐ�̃����o�Ɋւ�������w�肵�܂��B</param>
		/// <param name="value">�������ޒl���w�肵�܂��B</param>
		protected void SetValue(System.Reflection.MemberInfo info,object value){
			if(info.MemberType==System.Reflection.MemberTypes.Field) {
				((System.Reflection.FieldInfo)info).SetValue(this.parent,value);
				return;
			}else if(info.MemberType==System.Reflection.MemberTypes.Property){
				System.Reflection.PropertyInfo pinfo=(System.Reflection.PropertyInfo)info;
				if(pinfo.GetIndexParameters().Length==0&&pinfo.CanWrite){
					pinfo.SetValue(this.parent,value,new object[]{});
					return;
				}
			}
			throw new System.ArgumentException("�w�肵�������o�ɒl��ݒ肷�鎖�͏o���܂���B","info");
		}
		//=======================================
		//		�ڕW�̃����o
		//=======================================
		private System.Reflection.MemberInfo member;
		/// <summary>
		/// �ǂݏ����̑ΏۂƂȂ郁���o�̏����擾���܂��B
		/// </summary>
		public virtual System.Reflection.MemberInfo TargetInfo{
			get{return this.member;}
		}
		/// <summary>
		/// �ǂݏ����̑ΏۂƂȂ郁���o�̌^���擾���܂��B
		/// </summary>
		public virtual System.Type TargetType{
			get{
				switch(member.MemberType) {
					case System.Reflection.MemberTypes.Field:
						return ((System.Reflection.FieldInfo)member).FieldType;
					case System.Reflection.MemberTypes.Property:
						return ((System.Reflection.PropertyInfo)member).PropertyType;
					case System.Reflection.MemberTypes.Method:
						return ((System.Reflection.MethodInfo)member).ReturnType;
					default:
						throw new System.ApplicationException("�w�肵����ނ̃����o�̌^�͎擾�o���܂���B");
				}
			}
		}
		//=======================================
		//		�ǂݏ������s�� StreamAccessor
		//=======================================
		private StreamAccessor accessor;
		/// <summary>
		/// �ǂݏ������s���Ώۂ�ێ����� <see cref="StreamAccessor"/> ���擾���܂��B
		/// </summary>
		public StreamAccessor Accessor{
			get{return this.accessor;}
		}
	}
	#endregion

}