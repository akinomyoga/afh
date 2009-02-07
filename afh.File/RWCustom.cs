using Gen=System.Collections.Generic;
using Ref=System.Reflection;

namespace afh.File{
	using afh.File.Design;

	/// <summary>
	/// StreamAccessor からの読込及び書込を提供します。
	/// </summary>
	public interface ICustomAccessor:ICustomReader,ICustomWriter{}

	/// <summary>
	/// StreamAccessor からの読込を提供します。
	/// </summary>
	public interface ICustomReader{
		/// <summary>
		/// StreamAccessor から情報を読み取って <typeparamref name="T"/> のインスタンスを作成します。
		/// </summary>
		/// <param name="accessor">読み取り元の StreamAccessor を指定します。</param>
		/// <returns>読み取って作成した <typeparamref name="T"/> のインスタンスを返します。</returns>
		object Read(StreamAccessor accessor);
		/// <summary>
		/// このインスタンスを通じて読み込む事の出来る型を取得します。
		/// </summary>
		System.Type Type{get;}
	}
	/// <summary>
	/// StreamAccessor への書込を提供します。
	/// </summary>
	public interface ICustomWriter{
		/// <summary>
		/// StreamAccessor に指定した <typeparamref name="T"/> を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を指定します。</param>
		/// <param name="accessor">書込先の StreamAccessor を指定します。</param>
		void Write(object value,StreamAccessor accessor);
		/// <summary>
		/// このインスタンスを通じて書き込む事の出来る型を取得します。
		/// </summary>
		System.Type Type{get;}
	}

	public partial class StreamAccessor{
		private static Gen::Dictionary<System.Type,ICustomReader> creaders=new Gen::Dictionary<System.Type,ICustomReader>();
		private static Gen::Dictionary<System.Type,ICustomWriter> cwriters=new Gen::Dictionary<System.Type,ICustomWriter>();

		private static ICustomWriter GetCustomWriter(System.Type type){
			// 既に登録されている場合
			ICustomWriter ret;
			if(cwriters.TryGetValue(type,out ret))return ret;

			WriteTypeAttribute[] attrs;

			// 直接の属性適用
			attrs=(WriteTypeAttribute[])type.GetCustomAttributes(typeof(WriteTypeAttribute),false);
			if(attrs.Length!=0)return cwriters[type]=attrs[0].GetCustomWriter(type);

			// 派生元での属性適用
			attrs=(WriteTypeAttribute[])type.GetCustomAttributes(typeof(WriteTypeAttribute),true);
			foreach(WriteTypeAttribute attr in attrs)
				if(attr.Inheritable)return cwriters[type]=attr.GetCustomWriter(type);

			// 見つからなかった場合
			return cwriters[type]=null;
		}

		private static ICustomReader GetCustomReader(System.Type type){
			// 既に登録されている場合
			ICustomReader ret;
			if(creaders.TryGetValue(type,out ret))return ret;

			ReadTypeAttribute[] attrs;
			//-- 直接の属性適用
			attrs=(ReadTypeAttribute[])type.GetCustomAttributes(typeof(ReadTypeAttribute),false);
			if(attrs.Length!=0)return creaders[type]=attrs[0].GetCustomReader(type);
			//--派生元での属性適用
			attrs=(ReadTypeAttribute[])type.GetCustomAttributes(typeof(ReadTypeAttribute),true);
			foreach(ReadTypeAttribute attr in attrs)
				if(attr.Inheritable)return creaders[type]=attr.GetCustomReader(type);

			// 見つからなかった場合
			return creaders[type]=null;
		}

		/// <summary>
		/// ICustomReader を登録します。
		/// 登録すると ICustomReader で対応している型 T を StreamAccessor の Read&lt;T&gt;() で読み込む事が出来る様になります。
		/// </summary>
		/// <param name="reader">登録する ICustomReader を指定します。</param>
		public static void RegisterCustomReader(ICustomReader reader){
			if(creaders.ContainsKey(reader.Type)&&creaders[reader.Type]!=null)
				throw new System.ApplicationException("追加定義読込は既に定義されています。複数定義されていると不整合の原因になります。");
			creaders[reader.Type]=reader;
		}
		/// <summary>
		/// ICustomWriter を登録します。
		/// 登録すると ICustomWriter で対応している型 T を StreamAccessor の Write() で書き込む事が出来る様になります。
		/// </summary>
		/// <param name="writer">登録する ICustomWriter を指定します。</param>
		public static void RegisterCustomWriter(ICustomWriter writer){
			if(cwriters.ContainsKey(writer.Type)&&cwriters[writer.Type]!=null)
				throw new System.ApplicationException("追加定義書込は既に定義されています。複数定義されていると不整合の原因になります。");
			cwriters[writer.Type]=writer;
		}
		/// <summary>
		/// ICustomAccessor を登録します。
		/// 登録すると ICustomAccessor で対応している型 T を StreamAccessor の Read/Write 関数で読み書きできる様になります。
		/// </summary>
		/// <param name="accessor">登録する ICustomAccessor を指定します。</param>
		public static void RegisterCustomAccessor(ICustomAccessor accessor){
			RegisterCustomReader(accessor);
			RegisterCustomWriter(accessor);
		}
	}

	#region attr:BindCustomReader
	/// <summary>
	/// 特定の型に ICustomReader を関連付けます。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public class BindCustomReaderAttribute:ReadTypeAttribute{
		private System.Type reader_type;
		private ICustomReader reader;
		/// <summary>
		/// この属性を適用した型を StreamAccessor で読む時に使用する ICustomReader を指定します。
		/// </summary>
		/// <param name="typeICustomReader">
		/// ICustomReader を実装するクラスの型を指定して下さい。
		/// 亦、ここに指定する型は System.Type を一つ引数に取るコンストラクタ、亦は既定のコンストラクタを持っている必要があります。
		/// その両方を持っている場合には System.Type を一つ引数に取るコンストラクタが優先されます。
		/// <para>
		/// System.Type 一つを引数に取るコンストラクタには、その ICustomReader を使用して読み取るデータの型を指定します。
		/// 則ち、この属性の適用先の型、亦はその派生クラス (Inheritable=true を指定した場合) が指定されます。
		/// </para>
		/// </param>
		public BindCustomReaderAttribute(System.Type typeICustomReader){
			if(!typeof(ICustomReader).IsAssignableFrom(typeICustomReader))
				throw new System.ArgumentException("BindCustomReaderAttribute の初期化に使用する型は ICustomReader を継承した物にして下さい。");
			if(typeICustomReader.IsAbstract||typeICustomReader.IsGenericTypeDefinition)
				throw new System.ArgumentException("指定した型はインスタンス化出来ません。");
			reader_type=typeICustomReader;
		}

		public override ICustomReader GetCustomReader(System.Type type){
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Instance;
			const string MISSING_CTOR=@"[BindCustomReaderAttribute({0})]
指定した ICustomReader 実装型{0}には、System.Type 一つを引数に持つコンストラクタ亦は、既定のコンストラクタが定義されていません。
.ctor(System.Type) / .ctor() の何れかが実装されている必要があります。詳細に関しては BindCustomReaderAttribute の説明で確認して下さい。";
			//-------------------------------------------------------
			if(reader!=null)return reader;

			Ref::ConstructorInfo ctor;
			ctor=reader_type.GetConstructor(BF,null,new System.Type[]{typeof(System.Type)},null);
			if(ctor!=null)
				return this.reader=(ICustomReader)ctor.Invoke(new object[]{type});
			ctor=reader_type.GetConstructor(BF,null,new System.Type[0],null);
			if(ctor!=null)
				return this.reader=(ICustomReader)ctor.Invoke(new object[0]);

			throw new System.MissingMethodException(string.Format(MISSING_CTOR,reader_type));
		}
	}
	#endregion

	#region attr:BindCustomReader
	/// <summary>
	/// 特定の型に ICustomWriter を関連付けます。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public class BindCustomWriterAttribute:WriteTypeAttribute{
		private System.Type writer_type;
		private ICustomWriter writer;
		/// <summary>
		/// この属性を適用した型を StreamAccessor で書き込む時に使用する ICustomWriter を指定します。
		/// </summary>
		/// <param name="typeICustomWriter">
		/// ICustomWriter を実装するクラスの型を指定して下さい。
		/// 亦、ここに指定する型は System.Type を一つ引数に取るコンストラクタ、亦は既定のコンストラクタを持っている必要があります。
		/// その両方を持っている場合には System.Type を一つ引数に取るコンストラクタが優先されます。
		/// <para>
		/// System.Type 一つを引数に取るコンストラクタには、その ICustomWriter を使用して読み取るデータの型を指定します。
		/// 則ち、この属性の適用先の型、亦はその派生クラス (Inheritable=true を指定した場合) が指定されます。
		/// </para>
		/// </param>
		public BindCustomWriterAttribute(System.Type typeICustomWriter){
			if(!typeof(ICustomWriter).IsAssignableFrom(typeICustomWriter))
				throw new System.ArgumentException("BindCustomWriterAttribute の初期化に使用する型は ICustomReader を継承した物にして下さい。");
			if(typeICustomWriter.IsAbstract||typeICustomWriter.IsGenericTypeDefinition)
				throw new System.ArgumentException("指定した型はインスタンス化出来ません。");
			writer_type=typeICustomWriter;
		}

		public override ICustomWriter GetCustomWriter(System.Type type){
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Instance;
			const string MISSING_CTOR=@"[BindCustomWriterAttribute({0})]
指定した ICustomWriter 実装型{0}には、System.Type 一つを引数に持つコンストラクタ亦は、既定のコンストラクタが定義されていません。
.ctor(System.Type) / .ctor() の何れかが実装されている必要があります。詳細に関しては BindCustomWriterAttribute の説明で確認して下さい。";
			//-------------------------------------------------------
			if(writer!=null)return writer;

			Ref::ConstructorInfo ctor;
			ctor=writer_type.GetConstructor(BF,null,new System.Type[]{typeof(System.Type)},null);
			if(ctor!=null)
				return this.writer=(ICustomWriter)ctor.Invoke(new object[]{type});
			ctor=writer_type.GetConstructor(BF,null,new System.Type[0],null);
			if(ctor!=null)
				return this.writer=(ICustomWriter)ctor.Invoke(new object[0]);

			throw new System.MissingMethodException(string.Format(MISSING_CTOR,writer_type));
		}
	}
	#endregion
}

namespace afh.File.Design{
	/// <summary>
	/// ICustomAccessor の実装を助けます。
	/// </summary>
	/// <typeparam name="T">読込・書込を行う型を指定します。</typeparam>
	public abstract class CustomAccessorBase<T>:ICustomAccessor{
		/// <summary>
		/// StreamAccessor から情報を読み取って <typeparamref name="T"/> のインスタンスを作成します。
		/// </summary>
		/// <param name="accessor">読み取り元の StreamAccessor を指定します。</param>
		/// <returns>読み取って作成した <typeparamref name="T"/> のインスタンスを返します。</returns>
		protected abstract T Read(StreamAccessor accessor);
		/// <summary>
		/// StreamAccessor に指定した <typeparamref name="T"/> を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を指定します。</param>
		/// <param name="accessor">書込先の StreamAccessor を指定します。</param>
		protected abstract void Write(T value,StreamAccessor accessor);

		object ICustomReader.Read(StreamAccessor accessor){
			return this.Read(accessor);
		}
		void ICustomWriter.Write(object value,StreamAccessor accessor){
			this.Write((T)value,accessor);
		}
		System.Type ICustomReader.Type{get{return typeof(T);}}
		System.Type ICustomWriter.Type{get{return typeof(T);}}
	}

	/// <summary>
	/// ICustomReader の実装を助けます。
	/// </summary>
	/// <typeparam name="T">読込を行う型を指定します。</typeparam>
	public abstract class CustomReaderBase<T>:ICustomReader{
		/// <summary>
		/// StreamAccessor から情報を読み取って <typeparamref name="T"/> のインスタンスを作成します。
		/// </summary>
		/// <param name="accessor">読み取り元の StreamAccessor を指定します。</param>
		/// <returns>読み取って作成した <typeparamref name="T"/> のインスタンスを返します。</returns>
		protected abstract T Read(StreamAccessor accessor);
		
		object ICustomReader.Read(StreamAccessor accessor){
			return this.Read(accessor);
		}
		System.Type ICustomReader.Type{get{return typeof(T);}}
	}
	/// <summary>
	/// ICustomWriter の実装を助けます。
	/// </summary>
	/// <typeparam name="T">書込を行う型を指定します。</typeparam>
	public abstract class CustomWriterBase<T>:ICustomWriter{
		/// <summary>
		/// StreamAccessor に指定した <typeparamref name="T"/> を書き込みます。
		/// </summary>
		/// <param name="value">書き込む情報を指定します。</param>
		/// <param name="accessor">書込先の StreamAccessor を指定します。</param>
		protected abstract void Write(T value,StreamAccessor accessor);
	
		void ICustomWriter.Write(object value,StreamAccessor accessor){
			this.Write((T)value,accessor);
		}
		System.Type ICustomWriter.Type{get{return typeof(T);}}
	}
}