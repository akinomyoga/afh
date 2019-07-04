namespace afh.JavaScript{
	public abstract class PropertyBase:JavaScript.Object{
		public PropertyBase(){}
		/// <summary>
		/// parent �̓��Y�v���p�e�B����l���擾���܂�
		/// </summary>
		/// <param name="parent">�v���p�e�B�̎�����ł���I�u�W�F�N�g</param>
		/// <returns>�v���p�e�B����擾�����l��Ԃ��܂�</returns>
		public virtual JavaScript.Object GetValue(JavaScript.Object parent){
			JavaScript.FunctionBase f=this[":propget:"] as JavaScript.FunctionBase;
			if(f==null)throw new System.NotSupportedException(GET_NOTSUPPORT);
			return f.Invoke(parent,new JavaScript.Array());
		}
		private const string GET_NOTSUPPORT="���� Property �� get ���T�|�[�g���Ă��܂���";
		/// <summary>
		/// parent �̓��Y�v���p�e�B�ɒl��ݒ肵�܂�
		/// </summary>
		/// <param name="parent">�v���p�e�B�̎�����ł���I�u�W�F�N�g</param>
		/// <param name="value">�v���p�e�B�ɐݒ肷��l</param>
		public virtual void SetValue(JavaScript.Object parent,JavaScript.Object value){
			JavaScript.FunctionBase f=this[":propput:"] as JavaScript.FunctionBase;
			if(f==null)throw new System.NotSupportedException(SET_NOTSUPPORT);
			f.Invoke(parent,Array.Construct(value));
		}
		private const string SET_NOTSUPPORT="���� Property �� set ���T�|�[�g���Ă��܂���";
		/// <summary>
		/// TraceMember �� override ���܂��B
		/// Property �Ɋւ��Ă� TraceMember ���Ăяo����鎖�͑z�肳��Ă��Ȃ��̂ŗ�O�𓊂��܂��B
		/// </summary>
		internal override JavaScript.Object TraceMember(int index,int dest,string[] names){
			throw new System.Exception(TRACEMEM_NOTSUPPOSED);
		}
		private const string TRACEMEM_NOTSUPPOSED="���� Error: Property Object ���̂� trace �͗\�����Ă��܂���";
	}
	/// <summary>
	/// .NET �̃I�u�W�F�N�g�̃v���p�e�B��\���܂��B
	/// </summary>
	public class ManagedProperty:PropertyBase{
		private const string NOTSUPPORTED="�w�肵���I�u�W�F�N�g�͂��̃v���p�e�B�������܂���";
		//System.Reflection.PropertyInfo prop;
		/// <summary>
		/// ManagedProperty �C���X�^���X�� PropertyInfo ����쐬���܂��B
		/// </summary>
		/// <param name="prop">PropertyInfo</param>
		public ManagedProperty(System.Reflection.PropertyInfo prop){
			//this.prop=prop;
			System.Type type=prop.ReflectedType;
			if(prop.CanRead)this[":propget:"]=new ManagedMethod(type,prop.GetGetMethod());
			if(prop.CanWrite)this[":propput:"]=new ManagedMethod(type,prop.GetSetMethod());
		}
	}
}