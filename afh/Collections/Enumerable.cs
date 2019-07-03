using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// �l�X�� System.Collections.IEnumerator �𐶐�����ׂ̃N���X�ł��B
	/// </summary>
	[System.Serializable]
	public class Enumerable:System.Collections.IEnumerable{
		/// <summary>
		/// �w�肵���^�̕��̂ݗ񋓂��܂��B
		/// </summary>
		/// <typeparam name="T">�񋓂���^���w�肵�܂��B</typeparam>
		/// <returns>�w�肵���v�f�̂ݗ񋓂���񋓎q��Ԃ��܂��B</returns>
		public static Gen::IEnumerable<T> EnumByType<T>(System.Collections.IEnumerable baseEnumerable){
			foreach(object o in baseEnumerable)if(o is T)yield return (T)o;
		}
		private System.Collections.IEnumerable @enum;
		/// <summary>
		/// ���̃C���X�^���X�Ɋ֘A�t����ꂽ�񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public System.Collections.IEnumerator GetEnumerator(){return this.@enum.GetEnumerator();}
		//=================================================
		//		���Z�q�Ȃ�
		//=================================================
		/// <summary>
		/// ������ System.Collections.IEnumerable ���� Enumerable �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerable">Enumerable �̌��ɂȂ�C���X�^���X��Ԃ��܂��B</param>
		private Enumerable(System.Collections.IEnumerable enumerable){
			this.@enum=enumerable;
		}
		/// <summary>
		/// System.Collections.IEnumerable �� afh.Collections.Enumerable �ɕϊ����܂��B
		/// </summary>
		/// <param name="enumerable">afh.Collections.Enumerable ���ǂ���������Ȃ� IEnumerable ���w�肵�܂��B</param>
		/// <returns>�w�肵�� System.Collections.IEnumerable �� Enumerable �̎��͂��̘ԕԂ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� Enumerable �C���X�^���X�ɕϊ����ĕԂ��܂��B</returns>
		public static Enumerable From(System.Collections.IEnumerable enumerable){
			return (enumerable as Enumerable)??new Enumerable(enumerable);
		}
		/// <summary>
		/// System.Collections.IEnumerable �� <see cref="afh.Collections.Enumerable&lt;T&gt;"/> �ɕϊ����܂��B
		/// </summary>
		/// <param name="enumerable"><see cref="afh.Collections.Enumerable&lt;T&gt;"/>
		/// �^���ǂ���������Ȃ� IEnumerable ���w�肵�܂��B
		/// </param>
		/// <returns>�w�肵�� System.Collections.IEnumerable ��
		/// <see cref="afh.Collections.Enumerable&lt;T&gt;"/> �̎��͂��̘ԕԂ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� Enumerable �C���X�^���X�ɕϊ����ĕԂ��܂��B</returns>
		public static Enumerable<T> From<T>(Gen::IEnumerable<T> enumerable){
			return Enumerable<T>.From(enumerable);
		}
		//=================================================
		//		�񋓑���
		//=================================================
		/// <summary>
		/// �w�肵���t�B���^�Ŏ�e�ł���Ɣ��肳�ꂽ�I�u�W�F�N�g�݂̂�񋓂��܂��B
		/// </summary>
		/// <param name="filter">�񋓂��邩�ۂ��𔻒肷�� <see cref="afh.Filter"/> ���w�肵�܂��B</param>
		/// <returns>⿂Ɋ|������̃I�u�W�F�N�g��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public Enumerable Filter(Filter filter){
			return new Enumerable(Filter(this.@enum,filter));
		}
		private static System.Collections.IEnumerable Filter(System.Collections.IEnumerable enumerable,Filter filter){
			foreach(object obj in enumerable)if(filter(obj))yield return obj;
		}
		/// <summary>
		/// �񋓂���I�u�W�F�N�g�ɕϊ����|���܂��B
		/// </summary>
		/// <param name="conv">�ϊ��Ɏg�p���� <see cref="Converter"/> ���w�肵�܂��B</param>
		/// <returns>�ϊ����ʂ̃I�u�W�F�N�g��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public Enumerable Map(Converter conv){
			return new Enumerable(Map(this.@enum,conv));
		}
		private static System.Collections.IEnumerable Map(System.Collections.IEnumerable enumerable,Converter conv){
			foreach(object obj in enumerable)yield return conv(obj);
		}
		/// <summary>
		/// �񋓂����I�u�W�F�N�g��z��Ɋi�[���܂��B
		/// </summary>
		/// <returns>�񋓂��ꂽ���e���i�[�����z���Ԃ��܂��B</returns>
		public object[] ToArray(){
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(object obj in this.@enum)list.Add(obj);
			return list.ToArray();
		}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� Enumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� Enumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable operator +(Enumerable l,Enumerable r){return new Enumerable(op_Add(l.@enum,r.@enum));}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� Enumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� IEnumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� IEnumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable operator +(Enumerable l,System.Collections.IEnumerable r){return new Enumerable(op_Add(l.@enum,r));}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� IEnumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� Enumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� IEnumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable operator +(System.Collections.IEnumerable l,Enumerable r){return new Enumerable(op_Add(l,r.@enum));}
		private static System.Collections.IEnumerable op_Add(System.Collections.IEnumerable l,System.Collections.IEnumerable r){
			foreach(object item in l)yield return item;
			foreach(object item in r)yield return item;
		}
	}
	/// <summary>
	/// <typeparamref name="T"/> �̃I�u�W�F�N�g�̗񋓂̕��@��񋟂���N���X�ł��B
	/// </summary>
	/// <typeparam name="T">�񋓂̑ΏۂƂȂ�I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public class Enumerable<T>:System.Collections.IEnumerable{
		private Gen::IEnumerable<T> @enum;
		/// <summary>
		/// ���̃C���X�^���X�Ɋ֘A�t����ꂽ�񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public Gen::IEnumerator<T> GetEnumerator(){return this.@enum.GetEnumerator();}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return ((System.Collections.IEnumerable)this.@enum).GetEnumerator();
		}
		//=================================================
		//		���Z�q�Ȃ�
		//=================================================
		/// <summary>
		/// ������ System.Collections.IEnumerable ���� Enumerable �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerable">Enumerable �̌��ɂȂ�C���X�^���X��Ԃ��܂��B</param>
		private Enumerable(Gen::IEnumerable<T> enumerable){
			this.@enum=enumerable;
		}
		/// <summary>
		/// System.Collections.IEnumerable �� afh.Collections.Enumerable �ɕϊ����܂��B
		/// </summary>
		/// <param name="enumerable">afh.Collections.Enumerable �^���ǂ���������Ȃ� IEnumerable ���w�肵�܂��B</param>
		/// <returns>�w�肵�� System.Collections.IEnumerable �� Enumerable �̎��͂��̘ԕԂ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� Enumerable �C���X�^���X�ɕϊ����ĕԂ��܂��B</returns>
		public static Enumerable<T> From(Gen::IEnumerable<T> enumerable){
			return (enumerable as Enumerable<T>)??new Enumerable<T>(enumerable);
		}
		//=================================================
		//		�񋓑���
		//=================================================
		/// <summary>
		/// �w�肵���t�B���^�Ŏ�e�ł���Ɣ��肳�ꂽ�I�u�W�F�N�g�݂̂�񋓂��܂��B
		/// </summary>
		/// <param name="filter">�񋓂��邩�ۂ��𔻒肷�� <see cref="afh.Filter&lt;T&gt;"/> ���w�肵�܂��B</param>
		/// <returns>⿂Ɋ|������̃I�u�W�F�N�g��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public Enumerable<T> Filter(Filter<T> filter){
			return new Enumerable<T>(Filter(this.@enum,filter));
		}
		private static Gen::IEnumerable<T> Filter(Gen::IEnumerable<T> enumerable,Filter<T> filter){
			foreach(T obj in enumerable)if(filter(obj))yield return obj;
		}
		/// <summary>
		/// �񋓂���I�u�W�F�N�g�ɕϊ����|���܂��B
		/// </summary>
		/// <param name="conv">�ϊ��Ɏg�p���� <see cref="Converter"/> ���w�肵�܂��B</param>
		/// <returns>�ϊ����ʂ̃I�u�W�F�N�g��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public Enumerable<U> Map<U>(Converter<T,U> conv){
			return new Enumerable<U>(Map<U>(this.@enum,conv));
		}
		private static Gen::IEnumerable<U> Map<U>(Gen::IEnumerable<T> enumerable,Converter<T,U> conv){
			foreach(T obj in enumerable)yield return conv(obj);
		}
		/// <summary>
		/// �񋓂����I�u�W�F�N�g��z��Ɋi�[���܂��B
		/// </summary>
		/// <returns>�񋓂��ꂽ���e���i�[�����z���Ԃ��܂��B</returns>
		public T[] ToArray(){
			// List ?
			Gen::List<T> list=this.@enum as Gen::List<T>;
			if(list!=null)return list.ToArray();

			// ICollection?
			Gen::ICollection<T> col=this.@enum as Gen::ICollection<T>;
			if(col!=null){
				T[] ret=new T[col.Count];
				col.CopyTo(ret,0);
				return ret;
			}
			
			// IEnumerable
			list=new Gen::List<T>();
			foreach(T obj in this.@enum)list.Add(obj);
			return list.ToArray();
		}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� Enumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� Enumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable<T> operator +(Enumerable<T> l,Enumerable<T> r){return new Enumerable<T>(op_Add(l.@enum,r.@enum));}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� Enumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� IEnumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� IEnumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable<T> operator +(Enumerable<T> l,Gen::IEnumerable<T> r){return new Enumerable<T>(op_Add(l.@enum,r));}
		/// <summary>
		/// �񋓎q�𕹍����܂��B
		/// <para>�悸���߂ɍ��ӎ��� IEnumerable ���g�p���ė񋓂��܂��B���ɉE�ӎ��� Enumerable ���g�p���ė񋓂��܂��B
		/// �]���Ă���͌����ȕ����ł͂Ȃ��A�E�ӎ��ƍ��ӎ��ɓ����v�f���܂܂�Ă����ꍇ���̗v�f�͊܂܂�Ă���񐔂����񋓂���鎖�ɂȂ�܂��B</para>
		/// </summary>
		/// <param name="l">��ɗ񋓂���� IEnumerable ���w�肵�܂��B</param>
		/// <param name="r">��ɗ񋓂���� Enumerable ���w�肵�܂��B</param>
		/// <returns>���ӂƉE�ӂ̓��e��񋓂��� Enumerable ��Ԃ��܂��B</returns>
		public static Enumerable<T> operator +(Gen::IEnumerable<T> l,Enumerable<T> r){return new Enumerable<T>(op_Add(l,r.@enum));}
		private static Gen::IEnumerable<T> op_Add(Gen::IEnumerable<T> l,Gen::IEnumerable<T> r){
			foreach(T item in l)yield return item;
			foreach(T item in r)yield return item;
		}
	}
}
namespace afh{
	/// <summary>
	/// �w�肵���I�u�W�F�N�g����e�\���ǂ����𔻒肵�܂��B
	/// </summary>
	/// <param name="obj">����̑ΏۂƂȂ�I�u�W�F�N�g���w�肵�܂��B</param>
	/// <returns>��e�\�Ɣ��肳�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
	[System.Serializable]
	public delegate bool Filter(object obj);
	/// <summary>
	/// �w�肵���I�u�W�F�N�g����e�\���ǂ����𔻒肵�܂��B
	/// </summary>
	/// <typeparam name="T">����̑ΏۂƂȂ�I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
	/// <param name="obj">����̑ΏۂƂȂ�I�u�W�F�N�g���w�肵�܂��B</param>
	/// <returns>��e�\�Ɣ��肳�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
	[System.Serializable]
	public delegate bool Filter<T>(T obj);
	/// <summary>
	/// �w�肵���I�u�W�F�N�g��ʂ̃I�u�W�F�N�g�ɕϊ����܂��B
	/// </summary>
	/// <param name="obj">�ϊ��O�̃I�u�W�F�N�g���w�肵�܂��B</param>
	/// <returns>�ϊ���̃I�u�W�F�N�g��Ԃ��܂��B</returns>
	[System.Serializable]
	public delegate object Converter(object obj);
	/// <summary>
	/// �w�肵���I�u�W�F�N�g��ʂ̃I�u�W�F�N�g�ɕϊ����܂��B
	/// </summary>
	/// <typeparam name="T">�I�u�W�F�N�g�̕ϊ��O�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="U">�I�u�W�F�N�g�̕ϊ���̌^���w�肵�܂��B</typeparam>
	/// <param name="obj">�ϊ��O�̃I�u�W�F�N�g���w�肵�܂��B</param>
	/// <returns>�ϊ���̃I�u�W�F�N�g��Ԃ��܂��B</returns>
	[System.Serializable]
	public delegate U Converter<T,U>(T obj);
}