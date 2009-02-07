using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Emit=System.Reflection.Emit;

namespace afh.File {
	using afh.File.Design;

	#region attr:ReadSchedule
	/// <summary>
	/// Stream からの読込操作の順番を指定します。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class ReadScheduleAttribute:ReadTypeAttribute{
		private string[] members;
		/// <summary>
		/// ReadScheduleAttribute のコンストラクタです。Stream からの読込操作の順番の指定を行います。
		/// </summary>
		/// <param name="membernames">Stream からの読込の手順を示す為に、メンバ名を指定します。</param>
		public ReadScheduleAttribute(params string[] membernames){
			this.members=membernames;
		}
		/// <summary>
		/// 指定した型の値を Stream から読み込みます。
		/// </summary>
		/// <param name="type">読み取る値の型を指定します。</param>
		/// <param name="accessor">Stream に対する操作を提供する <see cref="StreamAccessor"/> を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
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
							// ReadWriteArray 属性に次の属性を設定
							if(attrs[j] is ReadWriteArrayAttribute){
								((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
							}
						}
						attrs[0].Read(o,info,accessor);
						break;
					default:
						throw new System.ApplicationException("指定した種類のメンバは読込に使用できません。");
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
			
			throw new System.ApplicationException("指定した型のインスタンスを作成する事が出来ませんでした。");
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
				const string ERR_MISSING_ATTR=@"指定したメンバには読み書きに関する情報を指定する属性がありません。
型: {0}
メンバ名: {1}
ReadWriteMemberAttribute を継承する属性を指定して下さい。";
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
								// ReadWriteArray 属性に次の属性を設定
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
							throw new System.ApplicationException("指定した種類のメンバは読込に使用できません。");
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
					throw new System.ApplicationException("指定した型のインスタンスを作成する事が出来ませんでした。");
				}
			}

		}
		#endregion
	}
	#endregion

	#region attr:WriteSchedule
	/// <summary>
	/// Stream への書込操作の順番を指定します。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public sealed class WriteScheduleAttribute:WriteTypeAttribute{
		private string[] members;
		/// <summary>
		/// WriteScheduleAttribute のコンストラクタです。Stream への書込操作を行う順番の指定をします。
		/// </summary>
		/// <param name="membernames">Stream からの書込の手順を示す為に、メンバ名を指定します。</param>
		public WriteScheduleAttribute(params string[] membernames){
			this.members=membernames;
		}
		/// <summary>
		/// 指定した型の値を Stream から読み込みます。
		/// </summary>
		/// <param name="obj">書き込む値を指定します。</param>
		/// <param name="accessor">Stream に対する操作を提供する <see cref="StreamAccessor"/> を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
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
							// ReadWriteArray 属性に次の属性を設定
							if(attrs[j] is ReadWriteArrayAttribute){
								((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
							}
						}
						attrs[0].Write(obj,info,accessor);
						break;
					default:
						throw new System.ApplicationException("指定した種類のメンバは読込に使用できません。");
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
					__dll__.log.WriteError(e,"DynamicMethod 生成に失敗しました。");
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
							if(len<0)throw new System.ApplicationException("指定したメンバには書込に関する属性がありません。");

							int j=0;
							while(j<len) {
								// ReadWriteArray 属性に次の属性を設定
								if(attrs[j] is ReadWriteArrayAttribute) {
									((ReadWriteArrayAttribute)attrs[j]).ChildElement=attrs[++j];
								}
							}
							EmitFieldProcess(ilgen,attrlist.Count,infolist.Count);
							attrlist.Add(attrs[0]);
							infolist.Add(info);
							break;
						default:
							throw new System.ApplicationException("指定した種類のメンバは書込に使用できません。");
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
	/// ファイル等の Stream の入出力に際して、どの様な形で読み書きするかを指定します。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property,AllowMultiple=true)]
	public class ReadWriteAsAttribute:ReadWriteMemberAttribute{
		private EncodingType enc;
		/// <summary>
		/// Stream 中での格納形式を取得します。
		/// </summary>
		public EncodingType EncodingType{get{return this.enc;}}
		/// <summary>
		/// Stream 中での格納形式が明示されているかどうかを取得します。
		/// </summary>
		public bool IsSpecifiedEncodingType{
			get{return this.enc==EncodingType.NoSpecified;}
		}
		/// <summary>
		/// ReadWriteAsAttribute のコンストラクタです。
		/// </summary>
		/// <param name="enc">Stream 中での格納形式を指定します。</param>
		public ReadWriteAsAttribute(EncodingType enc):base(){
			this.enc=enc;
		}
		/// <summary>
		/// ReadWriteAsAttribute のコンストラクタです。
		/// 入出力には EncodingType.NoSpecified が適用されます。
		/// </summary>
		public ReadWriteAsAttribute():this(EncodingType.NoSpecified){}
		//=================================================
		//		各種パラメータ
		//=================================================
		private int length=-1;
		/// <summary>
		/// 文字列の長さを取得亦は設定します。
		/// 負の値は指定がないと言う事を示します。
		/// </summary>
		public int Length{
			get{return this.length;}
			set{this.length=value;}
		}
		//=================================================
		//		読み書き
		//=================================================
		/// <summary>
		/// 指定した型の値を StreamAccessor を通して読み取ります。
		/// </summary>
		/// <param name="args">読込操作・読込対象に関する情報を指定します。</param>
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
		/// 指定した値を型に応じた方法で StreamAccessor を通して Stream に書き込みます。
		/// </summary>
		/// <param name="args">書込操作・書込対象に関する情報を指定します。</param>
		protected internal override void Write(WriteMemberArgs args) {
			if((this.enc&EncodingType.MASK_TYPE)==EncodingType.String&&this.length>=0)
				args.Accessor.WriteStringByCharCount((string)args.Value,this.length);
			args.Accessor.Write(args.Value,this.enc);
		}
	}
	#endregion

	#region attr:ReadWriteArray
	/// <summary>
	/// 配列の長さの指定方法を表現します。
	/// </summary>
	public enum ArrayLengthType{
		/// <summary>
		/// 配列の初めに、配列の要素の個数を Int32LE で記録します。
		/// </summary>
		CountEmbedded,
		/// <summary>
		/// 配列の初めに、配列のバイナリサイズを Int32LE で記録します。
		/// </summary>
		SizeEmbedded,
		/// <summary>
		/// 配列の要素の個数を別に指定します。<see cref="P:ReadWriteArrayAttribute.Count"/> を参照して下さい。
		/// </summary>
		CountSpecified,
		/// <summary>
		/// 配列のバイナリサイズを別に指定します。<see cref="P:ReadWriteArrayAttribute.Size"/> を参照して下さい。
		/// </summary>
		SizeSpecified,
		/// <summary>
		/// Stream の最後まで読み取ります。
		/// 読み取り最大数を指定する場合には <see cref="P:Count"/> に指定します。
		/// 読み取り最大数に達すると Stream に残りがあってもそこで読み取りを中断します。
		/// </summary>
		ReadToEnd
	}
	/// <summary>
	/// ファイル等の Stream の入出力に際して、配列等のコレクションを読み書きする方法を指定します。
	/// <para>
	/// この属性指定に引き続き要素の読み書き方法指定属性を指定する必要があります。
	/// TODO: 指定しなかった場合には既定の属性 (型に応じて代わりうる) を使用して読み書きします。
	/// </para>
	/// </summary>
	/// <remarks>
	/// 読み書きの対象として可能なコレクションは以下に挙げる物です。
	/// 但し以下に挙げられている物でも、要素の型を読み書きできない場合には読み書きは出来ません。
	/// <para>Stream から読込可能な物:
	/// T[] (動作未確認);
	/// System.Collections.ArrayList (動作未確認 一番動作が軽いです);
	/// System.Collections.Stack (動作未確認);
	/// System.Collections.Queue (動作未確認);
	/// System.Collections.Generic.ICollection&lt;T&gt; (動作未確認);
	/// System.Collections.Generic.IList&lt;T&gt; (動作未確認);
	/// System.Collections.Generic.IEnumerable&lt;T&gt; (動作未確認);
	/// System.Collections.Generic.List&lt;T&gt; (動作未確認);
	/// System.Collections.Generic.Stack&lt;T&gt; (動作未確認);
	/// System.Collections.Generic.Queue&lt;T&gt; (動作未確認);
	/// </para>
	/// <para>Stream に書込可能な物:
	/// System.Collections.IEnumerable を実装している物に対応しています (動作未確認);
	/// </para>
	/// </remarks>
	[System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property,AllowMultiple=true)]
	public class ReadWriteArrayAttribute:ReadWriteMemberAttribute{
		private ReadWriteMemberAttribute child_attr;
		/// <summary>
		/// 配列の要素の記録に関する情報を提供します。
		/// </summary>
		public ReadWriteMemberAttribute ChildElement{
			get{return this.child_attr;}
			internal set{this.child_attr=value;}
		}
		/// <summary>
		/// ReadWriteArrayAttribute のコンストラクタです。
		/// </summary>
		/// <param name="lengthType">配列要素の数の決定方法を指定します。</param>
		public ReadWriteArrayAttribute(ArrayLengthType lengthType):base(){
			this.lentype=lengthType;
		}
		/// <summary>
		/// ReadWriteAsArray のコンストラクタです。
		/// 配列要素の数は、配列要素数が配列データの先頭に記録されている事を想定します。
		/// </summary>
		public ReadWriteArrayAttribute():this(ArrayLengthType.CountEmbedded){}

		//=================================================
		//		配列の長さ
		//=================================================
		private ArrayLengthType lentype=ArrayLengthType.CountEmbedded;
		/// <summary>
		/// 配列の長さの指定方法を取得亦は設定します。既定値は <see cref="F:LenType.CountEmbedded"/> です。
		/// </summary>
		public ArrayLengthType ArrayLengthType{
			get{return this.lentype;}
			set{this.lentype=value;}
		}
		//-------------------------------------------------
		//		要素の個数
		//-------------------------------------------------
		private string count=null;
		/// <summary>
		/// 配列の要素の個数を指定します。
		/// 整数を文字列で指定するか、配列の要素の個数を保持するフィールドの名前、
		/// 亦は配列の要素の個数を取得する為のプロパティの名前を指定します。
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
		/// 要素の数を決定します。
		/// </summary>
		/// <returns>読み出す要素の数を返します。</returns>
		private int GetCount(ReadWriteMemberArgs args){
			const string ERR_COUNT_NOSPEC="CountSpecified が指定されているにも拘わらず count の値が設定されていません。";
			const string ERR_COUNT_READ="要素数の取得に失敗しました。要素数は Count プロパティを通して設定します。\r\n"
				+"直接数値で指定する場合には int.Parse で読み取れる値を指定して下さい。\r\n"
				+"他のフィールド亦はプロパティを参照する場合にはそのメンバの名前を指定して下さい。";
			const string ERR_COUNT_NEGATIVE="指定した要素数は負の数です。負の数の要素を読み取る事は出来ません。";
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
		//		データのサイズ
		//-------------------------------------------------
		private string size=null;
		/// <summary>
		/// 配列のバイナリサイズを指定します。
		/// 整数を文字列で指定するか、配列のバイナリサイズを保持するフィールドの名前、
		/// 亦は配列のバイナリサイズを取得する為のプロパティの名前を指定します。
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
		/// データのサイズを決定します。
		/// </summary>
		/// <returns>読み出すデータサイズを返します。</returns>
		private long GetSize(ReadWriteMemberArgs args) {
			const string ERR_SIZE_NOSPEC="SizeSpecified が指定されているにも拘わらず size の値が設定されていません。";
			const string ERR_SIZE_READ="データの大きさの取得に失敗しました。データの大きさは Size プロパティを通して設定します。\r\n"
				+"直接数値で指定する場合には long.Parse で読み取れる値を指定して下さい。\r\n"
				+"他のフィールド亦はプロパティを参照する場合にはそのメンバの名前を指定して下さい。";
			const string ERR_SIZE_NEGATIVE="指定したデータサイズは負の数です。負のサイズのデータを読み取る事は出来ません。";
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
		//		読み書き
		//=================================================
		/// <summary>
		/// 指定した型の値を StreamAccessor を通して読み取ります。
		/// </summary>
		/// <param name="args">読込操作・読込対象に関する情報を指定します。</param>
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
					throw new System.ApplicationException("未実装");
			}
		}
		private static object ToCollections(ReadMemberArgs args,System.Collections.ArrayList list){
			System.Type type=args.TargetType;
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("二次元以上の配列には未対応です。ジャグ配列による代用をお考え下さい。");
				return list.ToArray(type.GetElementType());
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("実体化していないジェネリッククラスのインスタンスは作成できません。");
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
			throw new System.InvalidOperationException("指定した型を配列として読み取る方法は提供されていません。");
		}
		private static System.Type GetElementType(System.Type type){
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("二次元以上の配列には未対応です。ジャグ配列による代用をお考え下さい。");
				return type.GetElementType();
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("実体化していないジェネリッククラスのインスタンスは作成できません。");
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
			throw new System.InvalidOperationException("指定した型を配列として読み取る方法は提供されていません。");
		}
		/// <summary>
		/// 指定した値を型に応じた方法で StreamAccessor を通して Stream に書き込みます。
		/// </summary>
		/// <param name="args">書込操作・書込対象に関する情報を指定します。</param>
		protected internal override void Write(WriteMemberArgs args){
			StreamAccessor accessor=args.Accessor;

			System.Type elemType=GetElementType(args.TargetType);

			int c;
			
			if(args.Value==null)
				throw new System.ApplicationException();
			System.Collections.IEnumerable value=args.Value as System.Collections.IEnumerable;
			if(value==null){
				throw new System.ApplicationException(
					string.Format("指定した型 {0} の値を配列として書き込む事は出来ません。",args.Value.GetType().FullName)
					);
			}

			StreamAccessor substream;
			WriteElementArgs child_args;
			switch(this.lentype) {
				case ArrayLengthType.CountEmbedded:
					substream=new StreamAccessor(args.Accessor.WriteSubStream(4)); // 後で長さを書き込む為
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
						if(0==c--)throw new System.ApplicationException("コレクション内の要素の数が、指定した要素数を越えています。正しい要素数を指定して下さい。");

						child_args.init_ElementValue(item);
						child_attr.Write(child_args);
					}
					if(c>0)throw new System.ApplicationException("コレクション内の要素数が、指定した要素数に満ちません。正しい要素数を指定して下さい。");
					break;
				case ArrayLengthType.SizeEmbedded:
					substream=new StreamAccessor(args.Accessor.WriteSubStream(4)); // 後で長さを書き込む為
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
					throw new System.ApplicationException("未実装");
			}
		}
#if OBSOLETE
		[System.Obsolete]
		private static System.Collections.ICollection FromCollection(System.Type type,object value){
			//const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.InvokeMethod|System.Reflection.BindingFlags.Public;
			if(type.IsArray){
				if(type.GetArrayRank()!=1)
					throw new System.Exception("二次元以上の配列には未対応です。ジャグ配列による代用をお考え下さい。");
				return (System.Array)value;
			}else if(type.IsGenericType){
				// Generic Collections
				if(type.ContainsGenericParameters)
					throw new System.Exception("実体化していないジェネリッククラスのインスタンスは作成できません。");
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
			throw new System.InvalidOperationException("指定した型を配列として読み取る方法は提供されていません。");
		}
		/// <summary>
		/// System.Collections.Generic.ICollection`1 から System.Collections.ICollection 
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
				}else throw new System.ArgumentException("指定した型を System.Collections.ICollection に接続できません。");
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
	/// メンバの読み書きの方法を指定する属性の基本クラスです。
	/// </summary>
	public abstract class ReadWriteMemberAttribute:System.Attribute{
		protected ReadWriteMemberAttribute(){}
		/// <summary>
		/// 読込条件を確認して読込を実行します。
		/// </summary>
		/// <param name="args">読込に使用する情報を指定します。</param>
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
		/// 書込条件を確認して書込を実行します。
		/// </summary>
		/// <param name="args">書込に使用する情報を指定します。</param>
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
		/// 指定した型の値を読み取ります。詳細な実装に関しては <see cref="ReadMemberArgs"/> を参照して下さい。
		/// </summary>
		/// <param name="args">読込操作・読込対象に関する情報を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
		protected internal abstract void Read(ReadMemberArgs args);
		/// <summary>
		/// 指定した値をその型に合った方法を使用して書き込みます。詳細な実装に関しては <see cref="WriteMemberArgs"/> を参照して下さい。
		/// </summary>
		/// <param name="args">書込操作・書込対象に関する情報を指定します。</param>
		protected internal abstract void Write(WriteMemberArgs args);

		private string readCondition="";
		private string writeCondition="";

		/// <summary>
		/// メンバを読み書きする条件を、bool 値を持つメンバで指定します。
		/// </summary>
		public string Condition{
			get{return readCondition==writeCondition?readCondition:"";}
			set{
				this.readCondition=value;
				this.writeCondition=value;
			}
		}
		/// <summary>
		/// メンバを読み込む条件を、bool 値を持つメンバで指定します。
		/// </summary>
		public string ConditionToRead{
			get{return this.readCondition;}
			set{this.readCondition=value;}
		}
		/// <summary>
		/// メンバを書き込む条件を、bool 値を持つメンバで指定します。
		/// </summary>
		public string ConditionToWrite{
			get{return this.writeCondition;}
			set{this.writeCondition=value;}
		}
#if OBSOLETE
		[System.Obsolete]
		protected static void GetValue(out object value,object o,string memberName,System.Type requestedType){
			const string ERR_CONDITION="指定した名前のフィールド亦はプロパティが見つかりませんでした。存在するフィールド名亦は (引数無し) プロパティ名を指定して下さい。";
			const string ERR_NOT_REQUESTEDTYPEL="指定した名前のメンバは要求された型ではありません。要求している型のフィールド/プロパティ名を指定して下さい。";
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
			throw new System.ArgumentException("指定したメンバから値を取得する事は出来ません。","info");
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
			throw new System.ArgumentException("指定したメンバに値を設定する事は出来ません。","info");
		}
		/// <summary>
		/// 指定した名前を持つ読み書き出来るメンバに関する情報を取得します。
		/// </summary>
		/// <param name="parentType">メンバを保持する型を指定します。</param>
		/// <param name="memberName">メンバの名前を指定します。</param>
		/// <returns>取得したメンバ情報を返します。</returns>
		/// <exception cref="System.MissingFieldException">適合するフィールド亦はプロパティが見つからなかった場合に発生します。</exception>
		/// <exception cref="System.Reflection.AmbiguousMatchException">適合するフィールド亦はプロパティが複数存在する場合に発生します。</exception>
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
					throw new System.ApplicationException("指定した種類のメンバの型は取得出来ません。");
			}
		}
#endif
	}

	#region args:ReadWriteMember
	/// <summary>
	/// 要求された要素読み出し操作・読込対象に関する情報を提供します。
	/// </summary>
	public sealed class ReadElementArgs:ReadMemberArgs{
		/// <summary>
		/// ReadElementArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">読み書きするメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="type">読み書きする情報の型を指定します。</param>
		/// <param name="accessor">読み書き対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		/// <param name="setvalue">読み込んだ値を報告する先を指定します。
		/// メンバの要素の読み取りに於いては、読み取られた値を直接親オブジェクトのメンバに格納する事が出来ないので、
		/// ここに受け取り用の関数を指定する必要があるのです。</param>
		public ReadElementArgs(object parent,System.Type type,StreamAccessor accessor,afh.CallBack<object> setvalue):base(parent,null,accessor){
			this.type=type;
			this.setvalue=setvalue;
		}
		/// <summary>
		/// ReadElementArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">読み書きするメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="type">読み書きする情報の型を指定します。</param>
		/// <param name="accessor">読み書き対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		/// <param name="list">読み込んだ値を格納する System.Collections.ArrayList を指定します。</param>
		public ReadElementArgs(object parent,System.Type type,StreamAccessor accessor,System.Collections.ArrayList list):base(parent,null,accessor){
			this.type=type;
			this.setvalue=SetValueToList;
			this.list=list;
		}
		/// <summary>
		/// 読み取った値を設定する先のメンバの情報を取得する為の物ですが、要素読み出しでは使用しません。
		/// </summary>
		public override System.Reflection.MemberInfo TargetInfo {
			get{throw new System.InvalidOperationException("要素読み出しでは対応するメンバは存在しません。");}
		}
		private System.Type type;
		/// <summary>
		/// 読み取る要素の型を取得します。
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
	/// 要求された要素書込操作・書込対象に関する情報を提供します。
	/// </summary>
	public sealed class WriteElementArgs:WriteMemberArgs{
		/// <summary>
		/// WriteElementArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">書き込む要素を格納しているメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="type">書き込む要素の型を指定します。</param>
		/// <param name="accessor">読み書きの対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		/// <param name="element">書き込む要素の値を指定します。</param>
		public WriteElementArgs(object parent,System.Type type,StreamAccessor accessor,object element):base(parent,null,accessor){
			this.type=type;
			this.value=element;
		}

		/// <summary>
		/// 書き込む値を取得する元のメンバの情報を取得する為の物ですが、要素書込では使用しません。
		/// </summary>
		public override System.Reflection.MemberInfo TargetInfo {
			get{throw new System.InvalidOperationException("要素書込では対応するメンバは存在しません。"); }
		}
		private System.Type type;
		/// <summary>
		/// 書き込む要素の型を取得します。
		/// </summary>
		public override System.Type TargetType{
			get{return this.type;}
		}

		private object value;
		/// <summary>
		/// ストリームに書き込む為の値を対象メンバから読み取ります。
		/// </summary>
		public override object Value {
			 get{return this.value;}
		}
		internal void init_ElementValue(object value){
			this.value=value;
		}
	}
	/// <summary>
	/// 要求された読込動作・読込対象に関する情報を保持します。
	/// </summary>
	/// <remarks>
	/// ReadWriteMemberAttribute::Read 関数の引数として渡された場合には、
	/// アクセサを通してストリームから適当な情報を読み出して、値を生成し、
	/// その値を SetValueToTarget で対象メンバに反映させます。
	/// <pre>--読込操作の図--
	/// 親オブジェクト [<see cref="P:Parent"/>, <see cref="P:ParentInfo"/>]
	/// └対象メンバ [<see cref="P:TargetInfo"/>, <see cref="P:TargetType"/>]
	///    ↑
	///  ┌┼アクセサ [<see cref="P:Accessor"/>] ┐
	///  │└─ ストリーム       │
	///  └───────────┘
	/// </pre>
	/// </remarks>
	public class ReadMemberArgs:ReadWriteMemberArgs{
		/// <summary>
		/// ReadMemberArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">読み書きするメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="member">読み書きの対象のメンバに関する情報を指定します。</param>
		/// <param name="accessor">読み書きの対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		public ReadMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor) : base(parent,member,accessor) { }

		/// <summary>
		/// 読み取った値を対象メンバに反映させます。
		/// </summary>
		/// <param name="value">ストリームから読み取った値を指定します。</param>
		public virtual void SetValueToTarget(object value){
			this.SetValue(this.TargetInfo,value);
		}
	}
	/// <summary>
	/// 要求された書き込み操作・書き込み対象に関する情報を保持します。
	/// </summary>
	/// <remarks>
	/// <pre>--書込操作の図--
	/// 親オブジェクト [<see cref="P:Parent"/>, <see cref="P:ParentInfo"/>]
	/// └対象メンバ [<see cref="P:TargetInfo"/>, <see cref="P:TargetType"/>]
	///    │
	///  ┌┼アクセサ [<see cref="P:Accessor"/>] ┐
	///  │└→ ストリーム       │
	///  └───────────┘
	/// </pre>
	/// </remarks>
	public class WriteMemberArgs:ReadWriteMemberArgs{
		/// <summary>
		/// WriteMemberArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">読み書きするメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="member">読み書きの対象のメンバに関する情報を指定します。</param>
		/// <param name="accessor">読み書きの対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		public WriteMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor) : base(parent,member,accessor) { }

		/// <summary>
		/// ストリームに書き込む為の値を対象メンバから読み取ります。
		/// </summary>
		public virtual object Value{
			get{return this.GetValue(this.TargetInfo);}
		}
	}
	/// <summary>
	/// 読み書きの対象に関する情報を保持し、亦、対象に対する操作を提供します。
	/// </summary>
	public class ReadWriteMemberArgs:System.EventArgs{
		/// <summary>
		/// ReadWriteMemberArgs のコンストラクタです。
		/// </summary>
		/// <param name="parent">読み書きするメンバを保持する親オブジェクトを指定します。</param>
		/// <param name="member">読み書きの対象のメンバに関する情報を指定します。</param>
		/// <param name="accessor">読み書きの対象の Stream に対するアクセスを提供する StreamAccessor を指定します。</param>
		public ReadWriteMemberArgs(object parent,System.Reflection.MemberInfo member,StreamAccessor accessor){
			this.parent=parent;
			this.member=member;
			this.accessor=accessor;
		}
		//=======================================
		//		親オブジェクト
		//=======================================
		private object parent;
		/// <summary>
		/// 読み書きするメンバを保持するオブジェクトを取得します。
		/// </summary>
		public object ParentObject{get{return parent;}}
		/// <summary>
		/// 読み書きするメンバを保持する型を取得します。
		/// </summary>
		public System.Type ParentType{
			get{return this.parent.GetType();}
		}
		//=======================================
		//		親のメンバへのアクセス
		//=======================================
		/// <summary>
		/// 読み書きの対象となるメンバに関する情報を取得します。
		/// 読み書きの対象となるメンバとは、フィールド亦は引数無しプロパティに為ります。
		/// </summary>
		/// <param name="memberName">検索するメンバの名前を指定します。</param>
		/// <returns>取得したメンバの情報を返します。</returns>
		/// <exception cref="System.MissingFieldException">
		/// 指定した名前を持つ、読み書きの対象として可能なメンバが見つからなかった場合に発生します。</exception>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// 指定した名前を持つ、読み書きの対象として可能なメンバが複数見つかり、一つに絞る事が出来無かった場合に発生します。</exception>
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
		/// 指定した名前のメンバを読み取ります。
		/// </summary>
		/// <param name="memberName">読み取るメンバの名前を指定します。</param>
		/// <typeparam name="T">読み取る値の型として要求する物を指定します。</typeparam>
		/// <returns>読み取った値を返します。</returns>
		public T GetValue<T>(string memberName){
			const string ERR_NOT_REQUESTEDTYPE="指定した名前のメンバは要求された型ではありません。要求している型のフィールド/プロパティ名を指定して下さい。";
			//-----------------------
			object value=GetValue(memberName);
			if(!(value is T))throw new System.ApplicationException(ERR_NOT_REQUESTEDTYPE);
			return (T)value;
		}
		/// <summary>
		/// 指定した名前のメンバを読み取ります。
		/// </summary>
		/// <param name="memberName">読み取るメンバの名前を指定します。</param>
		/// <param name="requestedType">メンバの型として要求する物を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
		public object GetValue(string memberName,System.Type requestedType) {
			const string ERR_NOT_REQUESTEDTYPE="指定した名前のメンバは要求された型ではありません。要求している型のフィールド/プロパティ名を指定して下さい。";
			//-----------------------
			object r=GetValue(memberName);
			if(!requestedType.IsAssignableFrom(r.GetType()))
				throw new System.ApplicationException(ERR_NOT_REQUESTEDTYPE);
			return r;
		}
		/// <summary>
		/// 指定した名前のメンバを読み取ります。
		/// </summary>
		/// <param name="memberName">読み取るメンバの名前を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
		public object GetValue(string memberName) {
			const string ERR_CONDITION="指定した名前のフィールド亦は引数無しプロパティが見つかりませんでした。存在するフィールド名亦は引数無しプロパティ名を指定して下さい。";
			//-----------------------
			try{
				return GetValue(GetMemberInfo(memberName));
			}catch(System.Exception e) {
				throw new System.ApplicationException(ERR_CONDITION,e);
			}
		}
		/// <summary>
		/// 親オブジェクト (<see cref="P:Parent"/>) の指定したメンバから値を読み出します。
		/// </summary>
		/// <param name="info">値の読込元のメンバの情報を指定します。</param>
		/// <returns>読み取った値を返します。</returns>
		public object GetValue(System.Reflection.MemberInfo info) {
			if(info.MemberType==System.Reflection.MemberTypes.Field){
				return ((System.Reflection.FieldInfo)info).GetValue(this.parent);
			}else if(info.MemberType==System.Reflection.MemberTypes.Property){
				System.Reflection.PropertyInfo pinfo=(System.Reflection.PropertyInfo)info;
				if(pinfo.GetIndexParameters().Length==0&&pinfo.CanRead){
					return pinfo.GetValue(this.parent,new object[]{});
				}
			}
			throw new System.ArgumentException("指定したメンバから値を取得する事は出来ません。","info");
		}
		/// <summary>
		/// 親オブジェクトの指定したメンバに値を設定します。
		/// </summary>
		/// <param name="info">書き込み先のメンバに関する情報を指定します。</param>
		/// <param name="value">書き込む値を指定します。</param>
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
			throw new System.ArgumentException("指定したメンバに値を設定する事は出来ません。","info");
		}
		//=======================================
		//		目標のメンバ
		//=======================================
		private System.Reflection.MemberInfo member;
		/// <summary>
		/// 読み書きの対象となるメンバの情報を取得します。
		/// </summary>
		public virtual System.Reflection.MemberInfo TargetInfo{
			get{return this.member;}
		}
		/// <summary>
		/// 読み書きの対象となるメンバの型を取得します。
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
						throw new System.ApplicationException("指定した種類のメンバの型は取得出来ません。");
				}
			}
		}
		//=======================================
		//		読み書きを行う StreamAccessor
		//=======================================
		private StreamAccessor accessor;
		/// <summary>
		/// 読み書きを行う対象を保持する <see cref="StreamAccessor"/> を取得します。
		/// </summary>
		public StreamAccessor Accessor{
			get{return this.accessor;}
		}
	}
	#endregion

}