using Gen=System.Collections.Generic;

namespace afh.Collections.Design{
	/// <summary>
	/// �R���N�V�����̊�{�N���X�ł��B
	/// </summary>
	public class CollectionBase:IVersion{
		/// <summary>
		/// �R���N�V�����ɕύX�����������ǂ������m�F����ׂ̔ԍ��ł��B
		/// �ύX������x�� version ���ς��܂��B
		/// </summary>
		protected int version=int.MinValue;
		/// <summary>
		/// �R���N�V�����ɕύX�������������L�^���܂��B
		/// �R���N�V������ version ���ς��܂��B
		/// </summary>
		protected void Changed(){
			if(this.version==int.MaxValue)this.version=int.MinValue;
			else this.version++;
		}

		#region IVersion �����o

		int IVersion.Version {
			get {return this.version;}
		}

		#endregion
	}

	/// <summary>
	/// �R���N�V���������b�v���A����ɑ΂��ĕύX��������ׂ̃N���X�ł��B
	/// </summary>
	/// <typeparam name="T">�R���N�V�����̗v�f�̌^���w�肵�܂��B</typeparam>
	public class CollectionWrapper<T>:Gen::ICollection<T>{
		/// <summary>
		/// �{�̂̃R���N�V������ێ����܂��B
		/// </summary>
		protected Gen::ICollection<T> body;

		//=================================================
		//	�ˑ���I�u�W�F�N�g
		//=================================================
		private IVersion parent=null;
		private int initial_ver;
		/// <summary>
		/// �ˑ���̃I�u�W�F�N�g��ݒ薒�͎擾���܂��B
		/// �ˑ���̃I�u�W�F�N�g�� version ���ς��Ƃ��̃R���N�V�����͖����ɂȂ�܂��B
		/// �����ɂȂ����ꍇ�ł��Đݒ肷��ΗL���ɂȂ�܂��B
		/// </summary>
		protected IVersion Parent{
			get{return this.parent;}
			set{
				if(value!=null)this.initial_ver=value.Version;
				this.parent=value;
			}
		}
		/// <summary>
		/// �ˑ���I�u�W�F�N�g�ɕύX���Ȃ����m�F���܂��B�ύX���������ꍇ�ɂ͗�O�𔭐������܂��B
		/// </summary>
		protected void check(){
			if(parent!=null&&parent.Version!=this.initial_ver)throw new CollectionChangedException();
		}

		/// <summary>
		/// CollectionWrapper �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="collection">���b�v�����R���N�V�������w�肵�܂��B</param>
		public CollectionWrapper(Gen::ICollection<T> collection){
			this.body=collection;
		}
		/// <summary>
		/// CollectionWrapper �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="collection">���b�v�����R���N�V�������w�肵�܂��B</param>
		/// <param name="parent">�ˑ���̃I�u�W�F�N�g���w�肵�܂��B
		/// �ˑ���̃I�u�W�F�N�g�� version ���ς��Ƃ��̃R���N�V�����͖����ɂȂ�܂��B</param>
		public CollectionWrapper(Gen::ICollection<T> collection,IVersion parent){
			this.body=collection;
			this.Parent=parent;
		}

		/// <summary>
		/// �ǂݎ���p�̍ۂɎg�p�ł��郁�b�Z�[�W�ł��B�u�Ǎ���p�R���N�V�����Ȃ̂ŕύX�ł��܂���B�v
		/// </summary>
		protected const string ERR_READONLY="�Ǎ���p�R���N�V�����Ȃ̂ŕύX�ł��܂���B";
		/// <summary>
		/// �ǂݎ���p�R���N�V�����ɗv�f��ǉ����悤�Ƃ������Ɏg�p�ł��郁�b�Z�[�W�ł��B
		/// </summary>
		public const string ERR_READONLY_ADD="���̃R���N�V�����͓ǂݎ���p�ׁ̈A�v�f��ǉ����鎖�͏o���܂���B";
		/// <summary>
		/// �ǂݎ���p�R���N�V�����̓��e�����������悤�Ƃ������Ɏg�p�ł��郁�b�Z�[�W�ł��B
		/// </summary>
		public const string ERR_READONLY_REMOVE="���̃R���N�V�����͓ǂݎ���p�ׁ̈A�v�f���폜���鎖�͏o���܂���B";
		/// <summary>
		/// �ǂݎ���p�R���N�V�����̗v�f���폜���悤�Ƃ������Ɏg�p�ł��郁�b�Z�[�W�ł��B
		/// </summary>
		public const string ERR_READONLY_CLEAR="���̃R���N�V�����͓ǂݎ���p�ׁ̈A�v�f�����������鎖�͏o���܂���B";

		#region ICollection<T> �����o
		/// <summary>
		/// �R���N�V�����ɗv�f��ǉ����܂��B
		/// </summary>
		/// <param name="item">�ǉ�����I�u�W�F�N�g���w�肵�܂��B</param>
		public virtual void Add(T item){
			this.check();
			this.body.Add(item);
		}
		/// <summary>
		/// �R���N�V�����̓��e�����������܂��B
		/// </summary>
		public virtual void Clear(){
			this.check();
			this.body.Clear();
		}
		/// <summary>
		/// �R���N�V�����Ɏw�肵���I�u�W�F�N�g���܂܂�Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="item">�R���N�V�����Ɋ܂܂�Ă��邩�ǂ������m�F����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�R���N�V�����Ɏw�肵���I�u�W�F�N�g���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public virtual bool Contains(T item) {
			this.check();
			return this.body.Contains(item);
		}
		/// <summary>
		/// �R���N�V�����̓��e��z��ɃR�s�[���܂��B
		/// </summary>
		/// <param name="array">�R�s�[��̔z����w�肵�܂��B</param>
		/// <param name="arrayIndex">�R�s�[��̔z�� <paramref name="array"/> �̒��ł̃R�s�[�J�n�ʒu���w�肵�܂��B</param>
		public virtual void CopyTo(T[] array,int arrayIndex){
			this.check();
			this.body.CopyTo(array,arrayIndex);
		}
		/// <summary>
		/// �R���N�V�����Ɋ܂܂�Ă���v�f�̐����擾���܂��B
		/// </summary>
		public virtual int Count{
			get{
				this.check();
				return this.body.Count;
			}
		}
		/// <summary>
		/// �R���N�V�������ǂݎ���p�ł��邩�ǂ������擾���܂��B
		/// </summary>
		public virtual bool IsReadOnly{
			get{
				this.check();
				return this.body.IsReadOnly;
			}
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�����̃R���N�V��������폜���܂��B
		/// </summary>
		/// <param name="item">�폜����I�u�W�F�N�g��Ԃ��܂��B</param>
		/// <returns>�w�肵���I�u�W�F�N�g���R���N�V��������폜�����ꍇ�� true ��Ԃ��܂��B
		/// �I�u�W�F�N�g��������Ȃ������ꍇ�ȂǃR���N�V��������폜���鎖���o���Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public virtual bool Remove(T item){
			this.check();
			return this.body.Remove(item);
		}
		#endregion

		#region IEnumerable<T> �����o
		/// <summary>
		/// �R���N�V�����̗v�f�̗񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public virtual System.Collections.Generic.IEnumerator<T> GetEnumerator(){
			this.check();
			return this.body.GetEnumerator();
		}

		#endregion

		#region IEnumerable �����o
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.body.GetEnumerator();
		}
		#endregion

	}
	/// <summary>
	/// �Ǎ���p�ɂ������R���N�V���������b�v����N���X�ł��B
	/// </summary>
	/// <typeparam name="T">�R���N�V�����̗v�f�̌^���w�肵�܂��B</typeparam>
	public sealed class ReadOnlyCollectionWrapper<T>:CollectionWrapper<T>{
		/// <summary>
		/// ReadOnlyCollectionWrapper �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="collection">���b�v�����R���N�V�������w�肵�܂��B</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection):this(collection,-1){}
		/// <summary>
		/// ReadOnlyCollectionWrapper �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="collection">���b�v�����R���N�V�������w�肵�܂��B</param>
		/// <param name="count">�A�N�Z�X�ł���v�f�̐��ɐ����������܂��B</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection,int count):base(collection){
			this.count=count;
		}
		/// <summary>
		/// ReadOnlyCollectionWrapper �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="collection">���b�v�����R���N�V�������w�肵�܂��B</param>
		/// <param name="count">�A�N�Z�X�ł���v�f�̐��ɐ����������܂��B�����������Ȃ��ꍇ�ɂ͕��̐��l���w�肵�܂��B</param>
		/// <param name="parent">�ˑ���̃I�u�W�F�N�g���w�肵�܂��B
		/// �ˑ���̃I�u�W�F�N�g�� version ���ς��Ƃ��̃R���N�V�����͖����ɂȂ�܂��B</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection,int count,IVersion parent):base(collection,parent){
			this.count=count;
		}

		#region ReadOnly �����o
#pragma warning disable 0809
		/// <summary>
		/// ���̃R���N�V�����͓ǂݎ���p�ׁ̈A�v�f��ǉ����鎖�͏o���܂���B
		/// </summary>
		/// <param name="item">�{���͒ǉ�����I�u�W�F�N�g���w�肵�܂��B</param>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_ADD)]
		public override void Add(T item){throw new System.NotSupportedException(ERR_READONLY);}
		/// <summary>
		/// ���̃R���N�V�����͓ǂݎ���p�ׁ̈A�v�f�����������鎖�͏o���܂���B
		/// </summary>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_CLEAR)]
		public override void Clear(){throw new System.NotSupportedException(ERR_READONLY);}
		/// <summary>
		/// ���̃R���N�V�����͓ǂݎ���p�Ȃ̂ŗv�f���폜���鎖�͏o���܂���B
		/// </summary>
		/// <param name="item">�{���͍폜����v�f���w�肵�܂��B</param>
		/// <returns>��O�𔭐�������̂ŕԒl�͂���܂���B</returns>
		/// <exception cref="System.NotSupportedException">���̃R���N�V�����͓ǂݎ���p�Ȃ̂ŏ�ɗ�O�𔭐������܂��B</exception>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_REMOVE)]
		public override bool Remove(T item){throw new System.NotSupportedException(ERR_READONLY);}
#pragma warning restore 0809
		#endregion

		#region Count �����o
		private int count;
		/// <summary>
		/// �R���N�V�����Ɋ܂܂�Ă���v�f�̐����擾���܂��B
		/// </summary>
		public override int Count {
			get{
				if(this.count<0)return base.Count;
				this.check();
				return this.count;
			}
		}
		/// <summary>
		/// �R���N�V�����Ɏw�肵���I�u�W�F�N�g���܂܂�Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="item">�R���N�V�����Ɋ܂܂�Ă��邩�ǂ������m�F����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>�R���N�V�����Ɏw�肵���I�u�W�F�N�g���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public override bool Contains(T item){
			if(count<0)return base.Contains(item);
			this.check();
			int i=0;
			foreach(T obj in this.body){
				if(i++>=count)break;
				if(obj.Equals(item))return true;
			}
			return false;
		}
		/// <summary>
		/// �R���N�V�����̓��e��z��ɃR�s�[���܂��B
		/// </summary>
		/// <param name="array">�R�s�[��̔z����w�肵�܂��B</param>
		/// <param name="arrayIndex">�R�s�[��̔z�� <paramref name="array"/> �̒��ł̃R�s�[�J�n�ʒu���w�肵�܂��B</param>
		public override void CopyTo(T[] array,int arrayIndex){
			if(count<0){
				base.CopyTo(array,arrayIndex);
				return;
			}
			// check
			this.check();
			if(array==null)
				throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex+this.count>array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex","�w�肵���z��̎w�肵���ʒu����R�s�[���J�n����� array �Ɏ��܂�܂���");

			// copy
			int i=0;
			foreach(T obj in this.body){
				if(i++>=count)break;
				array[arrayIndex++]=obj;
			}
		}
		/// <summary>
		/// �R���N�V�����̗v�f�̗񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public override System.Collections.Generic.IEnumerator<T> GetEnumerator(){
			if(count<0)return base.GetEnumerator();
			this.check();
			return this.GetEnumerator_withcount();
		}
		private Gen::IEnumerator<T> GetEnumerator_withcount(){
			int i=0;
			foreach(T item in this.body){
				if(i++>=count)break;
				yield return item;
			}
		}
		#endregion
	}

}