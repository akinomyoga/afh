using Ref=System.Reflection;

namespace afh.File {
	using afh.File.Design;

	/// <summary>
	/// StreamAccessor からの読み取り関数を自分で定義した場合に、それを指定するのに使用します。
	/// </summary>
	public class CustomReadAttribute:ReadTypeAttribute{
		private string method;
		/// <summary>
		/// この属性に関連付けられた関数 (StreamAccessor からの読込に使用する関数) の名前を取得します。
		/// </summary>
		public string MethodName{
			get{return this.method;}
		}
		/// <summary>
		/// CustomReadAttribute の初期化子です。StreamAccessor からの読込関数を指定します。
		/// </summary>
		/// <param name="methodName">関数の名前を指定します。
		/// 関数は StreamAccessor を引数に取る静的関数である必要があります。</param>
		public CustomReadAttribute(string methodName){
			this.method=methodName;
		}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.ReadTypeAttribute::Read"]/*'/>
		[System.Obsolete]
		public override object Read(System.Type type,StreamAccessor accessor){
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Static;
			Ref::MethodInfo info=(Ref::MethodInfo)GetMemberInfo(type,this.method,BF,InvokingFlags.Method_ParamAccessor);
			object ret;
			try{
				ret=info.Invoke(null,new object[]{accessor});
			}catch(System.Exception e){
				__dll__.log.WriteError(e,"独自定義ストリーム読込で例外が発生しました。");
				throw;
			}
			if(!type.IsInstanceOfType(ret))throw new System.InvalidCastException();
			return ret;
		}

		/// <summary>
		/// 書込を行う為の ICustomReader を取得します。
		/// </summary>
		/// <param name="type">読み取りの対象の型を指定します。</param>
		/// <returns>作成した ICustomReader を返します。</returns>
		public override ICustomReader GetCustomReader(System.Type type) {
			return new Reader(this,type);
		}
		private sealed class Reader:ICustomReader{
			private System.Type type;
			private Ref::MethodInfo info;

			public Reader(CustomReadAttribute attr,System.Type type) {
				const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Static;
				this.info=(Ref::MethodInfo)GetMemberInfo(type,attr.method,BF,InvokingFlags.Method_ParamAccessor);
				this.type=type;
			}

			public System.Type Type { get { return this.type; } }
			public object Read(StreamAccessor accessor) {
				object ret;
				try{
					ret=info.Invoke(null,new object[]{accessor});
				}catch(System.Exception e){
					__dll__.log.WriteError(e,"独自定義ストリーム読込で例外が発生しました。");
					throw;
				}
				if(!type.IsInstanceOfType(ret))throw new System.InvalidCastException();
				return ret;
			}
		}
	}
	/// <summary>
	/// StreamAccessor への書込関数を自分で定義した場合に、それを指定するのに使用します。
	/// </summary>
	public class CustomWriteAttribute:WriteTypeAttribute{
		private string method;
		/// <summary>
		/// この属性に関連付けられた関数 (StreamAccessor への書込に使用する関数) の名前を取得します。
		/// </summary>
		public string MethodName{
			get{return this.method;}
		}
		/// <summary>
		/// CustomReadAttribute の初期化子です。StreamAccessor からの読込関数を指定します。
		/// </summary>
		/// <param name="methodName">関数の名前を指定します。
		/// 関数は StreamAccessor を引数に取る静的関数である必要があります。</param>
		public CustomWriteAttribute(string methodName){
			this.method=methodName;
		}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.WriteTypeAttribute::Write"]/*'/>
		[System.Obsolete]
		public override void Write(object value,StreamAccessor accessor) {
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Static;
			Ref::MethodInfo info=(Ref::MethodInfo)GetMemberInfo(value.GetType(),this.method,BF,InvokingFlags.Method_ParamObjectAccessor);
			try{
				info.Invoke(null,new object[]{value,accessor});
			}catch(System.Exception e){
				__dll__.log.WriteError(e,"独自定義ストリーム書込で例外が発生しました。");
				throw;
			}
		}

		/// <summary>
		/// この属性をラップする ICustomWriter を取得します。
		/// </summary>
		/// <param name="type">書き込む型を指定します。</param>
		/// <returns>作成した ICustomWriter を返します。</returns>
		public override ICustomWriter GetCustomWriter(System.Type type){
			return new Writer(this,type);
		}
		private sealed class Writer:ICustomWriter{
			private System.Type type;
			private Ref::MethodInfo info;

			public Writer(CustomWriteAttribute attr,System.Type type) {
				const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Static;
				this.info=(Ref::MethodInfo)GetMemberInfo(type,attr.method,BF,InvokingFlags.Method_ParamObjectAccessor);
				this.type=type;
			}

			public void Write(object value,StreamAccessor accessor) {
				try{
					info.Invoke(null,new object[]{value,accessor});
				}catch(System.Exception e){
					__dll__.log.WriteError(e,"独自定義ストリーム書込で例外が発生しました。");
					throw;
				}
			}
			public System.Type Type { get { return this.type; } }
		}

	}
}

namespace afh.File.Design{
	/// <summary>
	/// 型の Stream からの読込に関する情報を提供する為の基本属性です。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public abstract class ReadTypeAttribute:ReadWriteTypeAttribute{
		public ReadTypeAttribute(){}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.ReadTypeAttribute::Read"]/*'/>
		[System.Obsolete]
		public virtual object Read(System.Type type,StreamAccessor accessor){
			throw new System.NotImplementedException("実装されていません。");
		}

		/// <summary>
		/// この属性をラップする ICustomReader を提供します。
		/// </summary>
		/// <param name="type">読み取りの対象の型を指定します。</param>
		/// <returns>作成した ICustomReader を返します。</returns>
		public virtual ICustomReader GetCustomReader(System.Type type){
			return new Reader(this,type);
		}
		private sealed class Reader:ICustomReader{
			private System.Type type;
			private ReadTypeAttribute attr;

			public Reader(ReadTypeAttribute attr,System.Type type){
				this.attr=attr;
				this.type=type;
			}

			public System.Type Type{get{return this.type;}}
			[System.Obsolete]
			public object Read(StreamAccessor accessor){
				return this.attr.Read(this.type,accessor);
			}
		}
		private bool inheritable;
		/// <summary>
		/// この属性を或るクラスに付けた場合に、派生先にも適用するかどうかを取得亦は設定します。
		/// 既定では false です。
		/// </summary>
		public bool Inheritable{
			get{return this.inheritable;}
			set{this.inheritable=value;}
		}
	}
	/// <summary>
	/// 型の Stream への書込に関する情報を提供する為の基本属性です。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public abstract class WriteTypeAttribute:ReadWriteTypeAttribute{
		public WriteTypeAttribute(){}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.WriteTypeAttribute::Write"]/*'/>
		[System.Obsolete]
		public virtual void Write(object value,StreamAccessor accessor){
			throw new System.NotImplementedException("実装されていません。");
		}

		/// <summary>
		/// この属性をラップする ICustomWriter を取得します。
		/// </summary>
		/// <param name="type">書き込む型を指定します。</param>
		/// <returns>作成した ICustomWriter を返します。</returns>
		public virtual ICustomWriter GetCustomWriter(System.Type type){
			return new Writer(this,type);
		}
		private sealed class Writer:ICustomWriter{
			private System.Type type;
			private WriteTypeAttribute attr;

			public Writer(WriteTypeAttribute attr,System.Type type){
				this.attr=attr;
				this.type=type;
			}
			[System.Obsolete]
			public void Write(object value,StreamAccessor accessor){
				this.attr.Write(value,accessor);
			}
			public System.Type Type{get{return this.type;}}
		}

		private bool inheritable=false;
		/// <summary>
		/// この属性を或るクラスに付けた場合に、派生先にも適用するかどうかを取得亦は設定します。
		/// 既定では false です。
		/// </summary>
		public bool Inheritable{
			get{return this.inheritable;}
			set{this.inheritable=value;}
		}
	}
	/// <summary>
	/// 型の読み書きに関する情報を提供する基本属性です。
	/// </summary>
	public abstract class ReadWriteTypeAttribute:System.Attribute{
		public ReadWriteTypeAttribute(){}
		/// <summary>
		/// 読み書きに使用するメンバ情報を取得します。
		/// </summary>
		/// <param name="type">メンバを保持する型を指定します。</param>
		/// <param name="member">メンバ名を指定します。</param>
		/// <returns>見つかったメンバの情報を返します。</returns>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// 適合するメンバが複数見つかった場合に発生します。
		/// </exception>
		/// <exception cref="System.MissingFieldException">
		/// 適合するメンバ (フィールドに限らない) が見つからなかった場合に発生します。
		/// </exception>
		protected static System.Reflection.MemberInfo GetMemberInfo(System.Type type,string member){
			const System.Reflection.BindingFlags BF
				=System.Reflection.BindingFlags.Public
				|System.Reflection.BindingFlags.NonPublic
				|System.Reflection.BindingFlags.Instance
				|System.Reflection.BindingFlags.Static;
			const InvokingFlags IF=InvokingFlags.Field|InvokingFlags.PropertyNoIndexParameter
				|InvokingFlags.Method_NoParameter|InvokingFlags.Method_ParamAccessor;
			return GetMemberInfo(type,member,BF,IF);
		}
		/// <summary>
		/// 読み書きに使用するメンバ情報を取得します。
		/// </summary>
		/// <param name="type">メンバを保持する型を指定します。</param>
		/// <param name="member">メンバ名を指定します。</param>
		/// <param name="bindings">検索するメンバのバインドの形態を指定します。</param>
		/// <param name="flags">検索するメンバの種類を指定します。</param>
		/// <returns>見つかったメンバの情報を返します。</returns>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// 適合するメンバが複数見つかった場合に発生します。
		/// </exception>
		/// <exception cref="System.MissingFieldException">
		/// 適合するメンバ (フィールドに限らない) が見つからなかった場合に発生します。
		/// </exception>
		protected static System.Reflection.MemberInfo GetMemberInfo(System.Type type,string member,Ref::BindingFlags bindings,InvokingFlags flags){
			const System.Reflection.MemberTypes MT
				=System.Reflection.MemberTypes.Field
				|System.Reflection.MemberTypes.Property
				|System.Reflection.MemberTypes.Method;
			System.Reflection.MemberInfo[] infos=type.GetMember(member,MT,bindings);
			bool found=false;
			System.Reflection.MemberInfo ret=null;
			for(int i=0;i<infos.Length;i++){
				switch(infos[i].MemberType){
					case System.Reflection.MemberTypes.Field:
						if((flags&InvokingFlags.Field)!=0)goto regist;
						break;
					case System.Reflection.MemberTypes.Property:
						if((flags&InvokingFlags.PropertyNoIndexParameter)!=0
							&&((System.Reflection.PropertyInfo)infos[i]).GetIndexParameters().Length==0)goto regist;
						break;
					case System.Reflection.MemberTypes.Method:
						System.Reflection.ParameterInfo[] pinfos=((System.Reflection.MethodInfo)infos[i]).GetParameters();
						if((flags&InvokingFlags.Method_NoParameter)!=0&&pinfos.Length==0)goto regist;
						if((flags&InvokingFlags.Method_ParamAccessor)!=0
							&&pinfos.Length==1
							&&pinfos[0].ParameterType.IsAssignableFrom(typeof(StreamAccessor)))goto regist;
						if((flags&InvokingFlags.Method_ParamObjectAccessor)!=0
							&&pinfos.Length==2
							&&pinfos[1].ParameterType.IsAssignableFrom(typeof(StreamAccessor)))goto regist;
						break;
					regist:
						if(found)throw new System.Reflection.AmbiguousMatchException();
						found=true;
						ret=infos[i];
						break;
				}
			}
			if(!found)throw new System.MissingFieldException(type.FullName,member);
			return ret;
		}
	}
	/// <summary>
	/// 検索するメンバの種類を指定するのに使用します。
	/// </summary>
	[System.Flags]
	public enum InvokingFlags{
		/// <summary>
		/// フィールドを検索します。
		/// </summary>
		Field=0x1,
		/// <summary>
		/// 引数を取らないプロパティを検索します。
		/// </summary>
		PropertyNoIndexParameter=0x2,
		/// <summary>
		/// 引数を受け取らないメソッドを検索します。
		/// </summary>
		Method_NoParameter=0x4,
		/// <summary>
		/// 引数として StreamAccessor を受け取るメソッドを検索します。
		/// </summary>
		Method_ParamAccessor=0x8,
		/// <summary>
		/// 引数として object と StreamAccessor を受け取るメソッドを検索します。
		/// </summary>
		Method_ParamObjectAccessor=0x10,
	}
}