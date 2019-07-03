using Gen=System.Collections.Generic;
using Ref=System.Reflection;

namespace afh.File{
	using afh.File.Design;

	/// <summary>
	/// StreamAccessor ����̓Ǎ��y�я�����񋟂��܂��B
	/// </summary>
	public interface ICustomAccessor:ICustomReader,ICustomWriter{}

	/// <summary>
	/// StreamAccessor ����̓Ǎ���񋟂��܂��B
	/// </summary>
	public interface ICustomReader{
		/// <summary>
		/// StreamAccessor �������ǂݎ���� <typeparamref name="T"/> �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="accessor">�ǂݎ�茳�� StreamAccessor ���w�肵�܂��B</param>
		/// <returns>�ǂݎ���č쐬���� <typeparamref name="T"/> �̃C���X�^���X��Ԃ��܂��B</returns>
		object Read(StreamAccessor accessor);
		/// <summary>
		/// ���̃C���X�^���X��ʂ��ēǂݍ��ގ��̏o����^���擾���܂��B
		/// </summary>
		System.Type Type{get;}
	}
	/// <summary>
	/// StreamAccessor �ւ̏�����񋟂��܂��B
	/// </summary>
	public interface ICustomWriter{
		/// <summary>
		/// StreamAccessor �Ɏw�肵�� <typeparamref name="T"/> ���������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ����w�肵�܂��B</param>
		/// <param name="accessor">������� StreamAccessor ���w�肵�܂��B</param>
		void Write(object value,StreamAccessor accessor);
		/// <summary>
		/// ���̃C���X�^���X��ʂ��ď������ގ��̏o����^���擾���܂��B
		/// </summary>
		System.Type Type{get;}
	}

	public partial class StreamAccessor{
		private static Gen::Dictionary<System.Type,ICustomReader> creaders=new Gen::Dictionary<System.Type,ICustomReader>();
		private static Gen::Dictionary<System.Type,ICustomWriter> cwriters=new Gen::Dictionary<System.Type,ICustomWriter>();

		private static ICustomWriter GetCustomWriter(System.Type type){
			// ���ɓo�^����Ă���ꍇ
			ICustomWriter ret;
			if(cwriters.TryGetValue(type,out ret))return ret;

			WriteTypeAttribute[] attrs;

			// ���ڂ̑����K�p
			attrs=(WriteTypeAttribute[])type.GetCustomAttributes(typeof(WriteTypeAttribute),false);
			if(attrs.Length!=0)return cwriters[type]=attrs[0].GetCustomWriter(type);

			// �h�����ł̑����K�p
			attrs=(WriteTypeAttribute[])type.GetCustomAttributes(typeof(WriteTypeAttribute),true);
			foreach(WriteTypeAttribute attr in attrs)
				if(attr.Inheritable)return cwriters[type]=attr.GetCustomWriter(type);

			// ������Ȃ������ꍇ
			return cwriters[type]=null;
		}

		private static ICustomReader GetCustomReader(System.Type type){
			// ���ɓo�^����Ă���ꍇ
			ICustomReader ret;
			if(creaders.TryGetValue(type,out ret))return ret;

			ReadTypeAttribute[] attrs;
			//-- ���ڂ̑����K�p
			attrs=(ReadTypeAttribute[])type.GetCustomAttributes(typeof(ReadTypeAttribute),false);
			if(attrs.Length!=0)return creaders[type]=attrs[0].GetCustomReader(type);
			//--�h�����ł̑����K�p
			attrs=(ReadTypeAttribute[])type.GetCustomAttributes(typeof(ReadTypeAttribute),true);
			foreach(ReadTypeAttribute attr in attrs)
				if(attr.Inheritable)return creaders[type]=attr.GetCustomReader(type);

			// ������Ȃ������ꍇ
			return creaders[type]=null;
		}

		/// <summary>
		/// ICustomReader ��o�^���܂��B
		/// �o�^����� ICustomReader �őΉ����Ă���^ T �� StreamAccessor �� Read&lt;T&gt;() �œǂݍ��ގ����o����l�ɂȂ�܂��B
		/// </summary>
		/// <param name="reader">�o�^���� ICustomReader ���w�肵�܂��B</param>
		public static void RegisterCustomReader(ICustomReader reader){
			if(creaders.ContainsKey(reader.Type)&&creaders[reader.Type]!=null)
				throw new System.ApplicationException("�ǉ���`�Ǎ��͊��ɒ�`����Ă��܂��B������`����Ă���ƕs�����̌����ɂȂ�܂��B");
			creaders[reader.Type]=reader;
		}
		/// <summary>
		/// ICustomWriter ��o�^���܂��B
		/// �o�^����� ICustomWriter �őΉ����Ă���^ T �� StreamAccessor �� Write() �ŏ������ގ����o����l�ɂȂ�܂��B
		/// </summary>
		/// <param name="writer">�o�^���� ICustomWriter ���w�肵�܂��B</param>
		public static void RegisterCustomWriter(ICustomWriter writer){
			if(cwriters.ContainsKey(writer.Type)&&cwriters[writer.Type]!=null)
				throw new System.ApplicationException("�ǉ���`�����͊��ɒ�`����Ă��܂��B������`����Ă���ƕs�����̌����ɂȂ�܂��B");
			cwriters[writer.Type]=writer;
		}
		/// <summary>
		/// ICustomAccessor ��o�^���܂��B
		/// �o�^����� ICustomAccessor �őΉ����Ă���^ T �� StreamAccessor �� Read/Write �֐��œǂݏ����ł���l�ɂȂ�܂��B
		/// </summary>
		/// <param name="accessor">�o�^���� ICustomAccessor ���w�肵�܂��B</param>
		public static void RegisterCustomAccessor(ICustomAccessor accessor){
			RegisterCustomReader(accessor);
			RegisterCustomWriter(accessor);
		}
	}

	#region attr:BindCustomReader
	/// <summary>
	/// ����̌^�� ICustomReader ���֘A�t���܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public class BindCustomReaderAttribute:ReadTypeAttribute{
		private System.Type reader_type;
		private ICustomReader reader;
		/// <summary>
		/// ���̑�����K�p�����^�� StreamAccessor �œǂގ��Ɏg�p���� ICustomReader ���w�肵�܂��B
		/// </summary>
		/// <param name="typeICustomReader">
		/// ICustomReader ����������N���X�̌^���w�肵�ĉ������B
		/// ���A�����Ɏw�肷��^�� System.Type ��������Ɏ��R���X�g���N�^�A���͊���̃R���X�g���N�^�������Ă���K�v������܂��B
		/// ���̗����������Ă���ꍇ�ɂ� System.Type ��������Ɏ��R���X�g���N�^���D�悳��܂��B
		/// <para>
		/// System.Type ��������Ɏ��R���X�g���N�^�ɂ́A���� ICustomReader ���g�p���ēǂݎ��f�[�^�̌^���w�肵�܂��B
		/// �����A���̑����̓K�p��̌^�A���͂��̔h���N���X (Inheritable=true ���w�肵���ꍇ) ���w�肳��܂��B
		/// </para>
		/// </param>
		public BindCustomReaderAttribute(System.Type typeICustomReader){
			if(!typeof(ICustomReader).IsAssignableFrom(typeICustomReader))
				throw new System.ArgumentException("BindCustomReaderAttribute �̏������Ɏg�p����^�� ICustomReader ���p���������ɂ��ĉ������B");
			if(typeICustomReader.IsAbstract||typeICustomReader.IsGenericTypeDefinition)
				throw new System.ArgumentException("�w�肵���^�̓C���X�^���X���o���܂���B");
			reader_type=typeICustomReader;
		}

		public override ICustomReader GetCustomReader(System.Type type){
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Instance;
			const string MISSING_CTOR=@"[BindCustomReaderAttribute({0})]
�w�肵�� ICustomReader �����^{0}�ɂ́ASystem.Type ��������Ɏ��R���X�g���N�^���́A����̃R���X�g���N�^����`����Ă��܂���B
.ctor(System.Type) / .ctor() �̉��ꂩ����������Ă���K�v������܂��B�ڍׂɊւ��Ă� BindCustomReaderAttribute �̐����Ŋm�F���ĉ������B";
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
	/// ����̌^�� ICustomWriter ���֘A�t���܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public class BindCustomWriterAttribute:WriteTypeAttribute{
		private System.Type writer_type;
		private ICustomWriter writer;
		/// <summary>
		/// ���̑�����K�p�����^�� StreamAccessor �ŏ������ގ��Ɏg�p���� ICustomWriter ���w�肵�܂��B
		/// </summary>
		/// <param name="typeICustomWriter">
		/// ICustomWriter ����������N���X�̌^���w�肵�ĉ������B
		/// ���A�����Ɏw�肷��^�� System.Type ��������Ɏ��R���X�g���N�^�A���͊���̃R���X�g���N�^�������Ă���K�v������܂��B
		/// ���̗����������Ă���ꍇ�ɂ� System.Type ��������Ɏ��R���X�g���N�^���D�悳��܂��B
		/// <para>
		/// System.Type ��������Ɏ��R���X�g���N�^�ɂ́A���� ICustomWriter ���g�p���ēǂݎ��f�[�^�̌^���w�肵�܂��B
		/// �����A���̑����̓K�p��̌^�A���͂��̔h���N���X (Inheritable=true ���w�肵���ꍇ) ���w�肳��܂��B
		/// </para>
		/// </param>
		public BindCustomWriterAttribute(System.Type typeICustomWriter){
			if(!typeof(ICustomWriter).IsAssignableFrom(typeICustomWriter))
				throw new System.ArgumentException("BindCustomWriterAttribute �̏������Ɏg�p����^�� ICustomReader ���p���������ɂ��ĉ������B");
			if(typeICustomWriter.IsAbstract||typeICustomWriter.IsGenericTypeDefinition)
				throw new System.ArgumentException("�w�肵���^�̓C���X�^���X���o���܂���B");
			writer_type=typeICustomWriter;
		}

		public override ICustomWriter GetCustomWriter(System.Type type){
			const Ref::BindingFlags BF=Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic|Ref::BindingFlags.Instance;
			const string MISSING_CTOR=@"[BindCustomWriterAttribute({0})]
�w�肵�� ICustomWriter �����^{0}�ɂ́ASystem.Type ��������Ɏ��R���X�g���N�^���́A����̃R���X�g���N�^����`����Ă��܂���B
.ctor(System.Type) / .ctor() �̉��ꂩ����������Ă���K�v������܂��B�ڍׂɊւ��Ă� BindCustomWriterAttribute �̐����Ŋm�F���ĉ������B";
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
	/// ICustomAccessor �̎����������܂��B
	/// </summary>
	/// <typeparam name="T">�Ǎ��E�������s���^���w�肵�܂��B</typeparam>
	public abstract class CustomAccessorBase<T>:ICustomAccessor{
		/// <summary>
		/// StreamAccessor �������ǂݎ���� <typeparamref name="T"/> �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="accessor">�ǂݎ�茳�� StreamAccessor ���w�肵�܂��B</param>
		/// <returns>�ǂݎ���č쐬���� <typeparamref name="T"/> �̃C���X�^���X��Ԃ��܂��B</returns>
		protected abstract T Read(StreamAccessor accessor);
		/// <summary>
		/// StreamAccessor �Ɏw�肵�� <typeparamref name="T"/> ���������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ����w�肵�܂��B</param>
		/// <param name="accessor">������� StreamAccessor ���w�肵�܂��B</param>
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
	/// ICustomReader �̎����������܂��B
	/// </summary>
	/// <typeparam name="T">�Ǎ����s���^���w�肵�܂��B</typeparam>
	public abstract class CustomReaderBase<T>:ICustomReader{
		/// <summary>
		/// StreamAccessor �������ǂݎ���� <typeparamref name="T"/> �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="accessor">�ǂݎ�茳�� StreamAccessor ���w�肵�܂��B</param>
		/// <returns>�ǂݎ���č쐬���� <typeparamref name="T"/> �̃C���X�^���X��Ԃ��܂��B</returns>
		protected abstract T Read(StreamAccessor accessor);
		
		object ICustomReader.Read(StreamAccessor accessor){
			return this.Read(accessor);
		}
		System.Type ICustomReader.Type{get{return typeof(T);}}
	}
	/// <summary>
	/// ICustomWriter �̎����������܂��B
	/// </summary>
	/// <typeparam name="T">�������s���^���w�肵�܂��B</typeparam>
	public abstract class CustomWriterBase<T>:ICustomWriter{
		/// <summary>
		/// StreamAccessor �Ɏw�肵�� <typeparamref name="T"/> ���������݂܂��B
		/// </summary>
		/// <param name="value">�������ޏ����w�肵�܂��B</param>
		/// <param name="accessor">������� StreamAccessor ���w�肵�܂��B</param>
		protected abstract void Write(T value,StreamAccessor accessor);
	
		void ICustomWriter.Write(object value,StreamAccessor accessor){
			this.Write((T)value,accessor);
		}
		System.Type ICustomWriter.Type{get{return typeof(T);}}
	}
}