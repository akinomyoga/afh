using Ref=System.Reflection;

namespace afh.File {
	using afh.File.Design;

	/// <summary>
	/// StreamAccessor ����̓ǂݎ��֐��������Œ�`�����ꍇ�ɁA������w�肷��̂Ɏg�p���܂��B
	/// </summary>
	public class CustomReadAttribute:ReadTypeAttribute{
		private string method;
		/// <summary>
		/// ���̑����Ɋ֘A�t����ꂽ�֐� (StreamAccessor ����̓Ǎ��Ɏg�p����֐�) �̖��O���擾���܂��B
		/// </summary>
		public string MethodName{
			get{return this.method;}
		}
		/// <summary>
		/// CustomReadAttribute �̏������q�ł��BStreamAccessor ����̓Ǎ��֐����w�肵�܂��B
		/// </summary>
		/// <param name="methodName">�֐��̖��O���w�肵�܂��B
		/// �֐��� StreamAccessor �������Ɏ��ÓI�֐��ł���K�v������܂��B</param>
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
				__dll__.log.WriteError(e,"�Ǝ���`�X�g���[���Ǎ��ŗ�O���������܂����B");
				throw;
			}
			if(!type.IsInstanceOfType(ret))throw new System.InvalidCastException();
			return ret;
		}

		/// <summary>
		/// �������s���ׂ� ICustomReader ���擾���܂��B
		/// </summary>
		/// <param name="type">�ǂݎ��̑Ώۂ̌^���w�肵�܂��B</param>
		/// <returns>�쐬���� ICustomReader ��Ԃ��܂��B</returns>
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
					__dll__.log.WriteError(e,"�Ǝ���`�X�g���[���Ǎ��ŗ�O���������܂����B");
					throw;
				}
				if(!type.IsInstanceOfType(ret))throw new System.InvalidCastException();
				return ret;
			}
		}
	}
	/// <summary>
	/// StreamAccessor �ւ̏����֐��������Œ�`�����ꍇ�ɁA������w�肷��̂Ɏg�p���܂��B
	/// </summary>
	public class CustomWriteAttribute:WriteTypeAttribute{
		private string method;
		/// <summary>
		/// ���̑����Ɋ֘A�t����ꂽ�֐� (StreamAccessor �ւ̏����Ɏg�p����֐�) �̖��O���擾���܂��B
		/// </summary>
		public string MethodName{
			get{return this.method;}
		}
		/// <summary>
		/// CustomReadAttribute �̏������q�ł��BStreamAccessor ����̓Ǎ��֐����w�肵�܂��B
		/// </summary>
		/// <param name="methodName">�֐��̖��O���w�肵�܂��B
		/// �֐��� StreamAccessor �������Ɏ��ÓI�֐��ł���K�v������܂��B</param>
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
				__dll__.log.WriteError(e,"�Ǝ���`�X�g���[�������ŗ�O���������܂����B");
				throw;
			}
		}

		/// <summary>
		/// ���̑��������b�v���� ICustomWriter ���擾���܂��B
		/// </summary>
		/// <param name="type">�������ތ^���w�肵�܂��B</param>
		/// <returns>�쐬���� ICustomWriter ��Ԃ��܂��B</returns>
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
					__dll__.log.WriteError(e,"�Ǝ���`�X�g���[�������ŗ�O���������܂����B");
					throw;
				}
			}
			public System.Type Type { get { return this.type; } }
		}

	}
}

namespace afh.File.Design{
	/// <summary>
	/// �^�� Stream ����̓Ǎ��Ɋւ������񋟂���ׂ̊�{�����ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public abstract class ReadTypeAttribute:ReadWriteTypeAttribute{
		public ReadTypeAttribute(){}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.ReadTypeAttribute::Read"]/*'/>
		[System.Obsolete]
		public virtual object Read(System.Type type,StreamAccessor accessor){
			throw new System.NotImplementedException("��������Ă��܂���B");
		}

		/// <summary>
		/// ���̑��������b�v���� ICustomReader ��񋟂��܂��B
		/// </summary>
		/// <param name="type">�ǂݎ��̑Ώۂ̌^���w�肵�܂��B</param>
		/// <returns>�쐬���� ICustomReader ��Ԃ��܂��B</returns>
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
		/// ���̑���������N���X�ɕt�����ꍇ�ɁA�h����ɂ��K�p���邩�ǂ������擾���͐ݒ肵�܂��B
		/// ����ł� false �ł��B
		/// </summary>
		public bool Inheritable{
			get{return this.inheritable;}
			set{this.inheritable=value;}
		}
	}
	/// <summary>
	/// �^�� Stream �ւ̏����Ɋւ������񋟂���ׂ̊�{�����ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public abstract class WriteTypeAttribute:ReadWriteTypeAttribute{
		public WriteTypeAttribute(){}
		/// <include file='document.xml' path='desc[@name="afh.File.Design.WriteTypeAttribute::Write"]/*'/>
		[System.Obsolete]
		public virtual void Write(object value,StreamAccessor accessor){
			throw new System.NotImplementedException("��������Ă��܂���B");
		}

		/// <summary>
		/// ���̑��������b�v���� ICustomWriter ���擾���܂��B
		/// </summary>
		/// <param name="type">�������ތ^���w�肵�܂��B</param>
		/// <returns>�쐬���� ICustomWriter ��Ԃ��܂��B</returns>
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
		/// ���̑���������N���X�ɕt�����ꍇ�ɁA�h����ɂ��K�p���邩�ǂ������擾���͐ݒ肵�܂��B
		/// ����ł� false �ł��B
		/// </summary>
		public bool Inheritable{
			get{return this.inheritable;}
			set{this.inheritable=value;}
		}
	}
	/// <summary>
	/// �^�̓ǂݏ����Ɋւ������񋟂����{�����ł��B
	/// </summary>
	public abstract class ReadWriteTypeAttribute:System.Attribute{
		public ReadWriteTypeAttribute(){}
		/// <summary>
		/// �ǂݏ����Ɏg�p���郁���o�����擾���܂��B
		/// </summary>
		/// <param name="type">�����o��ێ�����^���w�肵�܂��B</param>
		/// <param name="member">�����o�����w�肵�܂��B</param>
		/// <returns>�������������o�̏���Ԃ��܂��B</returns>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// �K�����郁���o���������������ꍇ�ɔ������܂��B
		/// </exception>
		/// <exception cref="System.MissingFieldException">
		/// �K�����郁���o (�t�B�[���h�Ɍ���Ȃ�) ��������Ȃ������ꍇ�ɔ������܂��B
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
		/// �ǂݏ����Ɏg�p���郁���o�����擾���܂��B
		/// </summary>
		/// <param name="type">�����o��ێ�����^���w�肵�܂��B</param>
		/// <param name="member">�����o�����w�肵�܂��B</param>
		/// <param name="bindings">�������郁���o�̃o�C���h�̌`�Ԃ��w�肵�܂��B</param>
		/// <param name="flags">�������郁���o�̎�ނ��w�肵�܂��B</param>
		/// <returns>�������������o�̏���Ԃ��܂��B</returns>
		/// <exception cref="System.Reflection.AmbiguousMatchException">
		/// �K�����郁���o���������������ꍇ�ɔ������܂��B
		/// </exception>
		/// <exception cref="System.MissingFieldException">
		/// �K�����郁���o (�t�B�[���h�Ɍ���Ȃ�) ��������Ȃ������ꍇ�ɔ������܂��B
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
	/// �������郁���o�̎�ނ��w�肷��̂Ɏg�p���܂��B
	/// </summary>
	[System.Flags]
	public enum InvokingFlags{
		/// <summary>
		/// �t�B�[���h���������܂��B
		/// </summary>
		Field=0x1,
		/// <summary>
		/// ���������Ȃ��v���p�e�B���������܂��B
		/// </summary>
		PropertyNoIndexParameter=0x2,
		/// <summary>
		/// �������󂯎��Ȃ����\�b�h���������܂��B
		/// </summary>
		Method_NoParameter=0x4,
		/// <summary>
		/// �����Ƃ��� StreamAccessor ���󂯎�郁�\�b�h���������܂��B
		/// </summary>
		Method_ParamAccessor=0x8,
		/// <summary>
		/// �����Ƃ��� object �� StreamAccessor ���󂯎�郁�\�b�h���������܂��B
		/// </summary>
		Method_ParamObjectAccessor=0x10,
	}
}