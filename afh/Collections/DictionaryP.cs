using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// ��� key �ɕ����̒l��o�^���鎖���o���鎫���̎�����񋟂��܂��B
	/// </summary>
	/// <typeparam name="TKey">�o�^���鎞�̌��o���ƂȂ�C���X�^���X�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">�o�^�����l�̌^���w�肵�܂��B</typeparam>
	public interface IDictionaryP<TKey,TVal>:Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>{
		/// <summary>
		/// �w�肵�� key �ƒl�̑g��o�^���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t���� key ���w�肵�܂��B</param>
		/// <param name="val">�o�^����l���w�肵�܂��B</param>
		void Add(TKey key,TVal val);
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="val">�폜����l���w�肵�܂��B</param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		bool Remove(TKey key,TVal val);
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���o�^����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="val">�o�^����Ă��邩��l���w�肵�܂��B</param>
		/// <returns>�o�^����Ă����ꍇ�ɂ� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		bool Contains(TKey key,TVal val);
		/// <summary>
		/// �w�肵�� key �Ɗ֘A�t����ꂽ���炩�̒l���o�^����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="key">����Ɋ֘A�t����ꂽ�l���o�^����Ă��邩�ǂ����𒲂ׂ� key ���w�肵�܂��B</param>
		/// <returns>�w�肵�� key �Ɋ֘A�t����ꂽ�l���o�^����Ă����ꍇ�� true ���A����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		bool ContainsKey(TKey key);
		/// <summary>
		/// �w�肵�� key �Ɋ֘A�t����ꂽ�l�̓��A�w�肵���ԍ��̕����폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="index">
		/// �폜����l�̔ԍ����w�肵�܂��B
		/// ��̓I�ɂ͎w�肵�� key �Ɋ֘A�t����ꂽ�l�� 0 ���珇�Ԃɔԍ������蓖�Ă�����
		/// index �Ԗڂɂ������l���폜����鎖�ɂȂ�܂��B
		/// </param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		bool RemoveAt(TKey key,int index);
		/// <summary>
		/// �w�肵�� key �Ɋ֘A�t����ꂽ�S�Ă̒l���폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		bool RemoveAll(TKey key);
		/// <summary>
		/// �w�肵�� key �ɑΉ�����l�̔z����擾���܂��B
		/// </summary>
		/// <param name="key">�擾����l�Ɋ֘A�Â���ꂽ key ���w�肵�܂��B</param>
		/// <returns>�l�̔z���Ԃ��܂��B</returns>
		TVal[] this[TKey key]{get;}
		/// <summary>
		/// �o�^����Ă��� Key �̏W�����擾���܂��B
		/// </summary>
		Gen::ICollection<TKey> Keys{get;}
		/// <summary>
		/// �o�^����Ă���l�̏W�����擾���܂��B
		/// </summary>
		Gen::ICollection<TVal> Values{get;}
	}
	/// <summary>
	/// ��� key �ɕ����̒l��o�^���鎖���o���� Dictionary �̃N���X�ł��B
	/// </summary>
	/// <typeparam name="TKey">�o�^���鎞�̌��o���ƂȂ�C���X�^���X�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">�o�^�����l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public class DictionaryP<TKey,TVal>
		:IDictionaryP<TKey,TVal>
		,Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>
		,Gen::IEnumerable<Gen::KeyValuePair<TKey,TVal>>
		,System.Collections.IEnumerable
	{
		private Gen::Dictionary<TKey,Gen::List<TVal>> container=new Gen::Dictionary<TKey,Gen::List<TVal>>();

		/// <summary>
		/// ��� DictionaryP �C���X�^���X�����������܂��B
		/// </summary>
		public DictionaryP(){}
		
		/// <summary>
		/// �o�^����Ă��� Key �̏W�����擾���܂��B
		/// </summary>
		public Gen::ICollection<TKey> Keys{
			get{return this.container.Keys;}
		}
		/// <summary>
		/// �o�^����Ă���l�̏W�����擾���܂��B
		/// </summary>
		public Gen::ICollection<TVal> Values{
			get{return values??(values=new ValuesCollection(this));}
		}Gen::ICollection<TVal> values;
		/// <summary>
		/// �w�肵�� key �ɑΉ�����l�̔z����擾���܂��B
		/// </summary>
		/// <param name="key">�擾����l�Ɋ֘A�Â���ꂽ key ���w�肵�܂��B</param>
		/// <returns>�l�̔z���Ԃ��܂��B</returns>
		public TVal[] this[TKey key]{
			get{return this.container.ContainsKey(key)?this.container[key].ToArray():new TVal[0];}
		}
		/// <summary>
		/// System.Collections.Gen.KeyValuePair&lt;TKey,TVal&gt; �̗񋓎q���擾���܂��B
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public Gen::IEnumerator<Gen::KeyValuePair<TKey,TVal>> GetEnumerator(){
			foreach(TKey key in this.container.Keys){
				foreach(TVal val in this.container[key]){
					yield return new Gen::KeyValuePair<TKey,TVal>(key,val);
				}
			}
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
		//=================================================
		//		��{����
		//=================================================
		/// <summary>
		/// �w�肵�� key �ƒl�̑g��o�^���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t���� key ���w�肵�܂��B</param>
		/// <param name="val">�o�^����l���w�肵�܂��B</param>
		public void Add(TKey key,TVal val){
			if(!this.container.ContainsKey(key))this.container.Add(key,new Gen::List<TVal>());
			this.container[key].Add(val);
		}
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="val">�폜����l���w�肵�܂��B</param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		public bool Remove(TKey key,TVal val){
			return this.container.ContainsKey(key)&&this.container[key].Remove(val);
		}
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���o�^����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="val">�o�^����Ă��邩��l���w�肵�܂��B</param>
		/// <returns>�o�^����Ă����ꍇ�ɂ� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Contains(TKey key,TVal val){
			return this.container.ContainsKey(key)&&this.container[key].Contains(val);
		}
		//=================================================
		//		��������
		//=================================================
		/// <summary>
		/// �w�肵�� key �Ɗ֘A�t����ꂽ���炩�̒l���o�^����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="key">����Ɋ֘A�t����ꂽ�l���o�^����Ă��邩�ǂ����𒲂ׂ� key ���w�肵�܂��B</param>
		/// <returns>�w�肵�� key �Ɋ֘A�t����ꂽ�l���o�^����Ă����ꍇ�� true ���A����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public bool ContainsKey(TKey key){
			return this.container.ContainsKey(key)&&this.container[key].Count>0;
		}
		/// <summary>
		/// �w�肵�� key �Ɋ֘A�t����ꂽ�l�̓��A�w�肵���ԍ��̕����폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <param name="index">
		/// �폜����l�̔ԍ����w�肵�܂��B
		/// ��̓I�ɂ͎w�肵�� key �Ɋ֘A�t����ꂽ�l�� 0 ���珇�Ԃɔԍ������蓖�Ă�����
		/// index �Ԗڂɂ������l���폜����鎖�ɂȂ�܂��B
		/// </param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		public bool RemoveAt(TKey key,int index){
			if(index<0||!this.container.ContainsKey(key)||index>=this.container[key].Count)return false;
			this.container[key].RemoveAt(index);
			return true;
		}
		/// <summary>
		/// �w�肵�� key �Ɋ֘A�t����ꂽ�S�Ă̒l���폜���܂��B
		/// </summary>
		/// <param name="key">�l�Ɋ֘A�t����ꂽ key ���w�肵�܂��B</param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		public bool RemoveAll(TKey key){
			if(!this.container.ContainsKey(key)||this.container[key].Count==0)return false;
			this.container[key].Clear();
			return true;
		}
		//=================================================
		//		ICollection
		//=================================================
		/// <summary>
		/// �w�肵�� key �ƒl�̑g��o�^���܂��B
		/// </summary>
		/// <param name="pair">�o�^���� key �ƒl�̑g���w�肵�܂��B</param>
		public void Add(Gen::KeyValuePair<TKey,TVal> pair){
			this.Add(pair.Key,pair.Value);
		}
		/// <summary>
		/// �o�^����Ă��� key �ƒl�̑g��S�ď������A�C���X�^���X�����������܂��B
		/// </summary>
		public void Clear(){this.container.Clear();}
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���폜���܂��B
		/// </summary>
		/// <param name="pair">�폜���� key �ƒl�̑g���w�肵�܂��B</param>
		/// <returns>
		/// �폜���ꂽ�ꍇ�ɂ� true ��Ԃ��܂��B
		/// ������܂܂�Ă��Ȃ������Ȃǂ̗��R�ɂ��폜����Ȃ������ꍇ�ɂ́@false ��Ԃ��܂��B
		/// </returns>
		public bool Remove(Gen::KeyValuePair<TKey,TVal> pair){
			return this.Remove(pair.Key,pair.Value);
		}
		/// <summary>
		/// �w�肵�� key �ƒl�̑g���o�^����Ă��邩�ǂ������m�F���܂��B
		/// </summary>
		/// <param name="pair">�o�^����Ă��邩�ǂ������m�F���� key �ƒl�̑g���w�肵�܂��B</param>
		/// <returns>�o�^����Ă����ꍇ�ɂ� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Contains(Gen::KeyValuePair<TKey,TVal> pair){
			return this.Contains(pair.Key,pair.Value);
		}
		/// <summary>
		/// �o�^����Ă���l�̐����擾���܂��B
		/// </summary>
		public int Count{
			get{
				int c=0;
				foreach(Gen::List<TVal> list in this.container.Values)c+=list.Count;
				return c;
			}
		}
		/// <summary>
		/// <see cref="DictionaryP&lt;TKey,TVal&gt;"/> �̗v�f�� <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] �ɃR�s�[���܂��B
		/// <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] �̓���̃C���f�b�N�X����R�s�[���J�n����܂��B
		/// </summary>
		/// <param name="array">
		/// <see cref="DictionaryP&lt;TKey,TVal&gt;"/> ����v�f���R�s�[����� 1 ������ <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[]�B
		/// <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] �ɂ́A0 ����n�܂�C���f�b�N�X�ԍ����K�v�ł��B
		/// </param>
		/// <param name="arrayIndex">�R�s�[�̊J�n�ʒu�ƂȂ�Aarray �� 0 ����n�܂�C���f�b�N�X�ԍ��B</param>
		public void CopyTo(Gen::KeyValuePair<TKey,TVal>[] array,int arrayIndex){
			//-- ��O
			if(array==null)throw new System.ArgumentNullException("array");
			if(array.Rank>1)throw new System.ArgumentException("array","array ���������ł��B");
			if(arrayIndex<0)throw new System.ArgumentOutOfRangeException("arrayIndex","arrayIndex �� 0 �����ł��B");
			if(array.Length<arrayIndex+this.Count){
				if(array.Length<=arrayIndex)
					throw new System.ArgumentException("arrayIndex","arrayIndex �� array �̒����ȏ�ł��B");
				throw new System.ArgumentException("array",
					string.Format(
						"�R�s�[���� afh.DictionaryP<{0},{1}> �̗v�f�����AarrayIndex ����R�s�[��� array �̖����܂łɊi�[�ł��鐔�𒴂��Ă��܂��B",
						typeof(TKey).FullName,
						typeof(TVal).FullName
					)
				);
			}
			//-- �R�s�[
			foreach(Gen::KeyValuePair<TKey,TVal> pair in this)array[arrayIndex++]=pair;
		}
		/// <summary>
		/// ���̃C���X�^���X���Ǎ���p�ł��邩�ǂ������擾���܂��B
		/// DictionaryP �͉����ł��v�f��ύX���鎖���o����׏�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsReadOnly{get{return false;}}
		//=================================================
		//		ICollection
		//=================================================
		/// <summary>
		/// �w�肵���֐���p���Ēl��ϊ��������ʂ� DictionaryP ��Ԃ��܂��B
		/// </summary>
		/// <typeparam name="TVal2">�l�̕ϊ���̌^���w�肵�܂��B</typeparam>
		/// <param name="converter">�l��ϊ����郁�\�b�h���w�肵�܂��B</param>
		/// <returns>�l��ϊ��������ʂ� DictionaryP ��Ԃ��܂��B</returns>
		public DictionaryP<TKey,TVal2> Map<TVal2>(Converter<TVal,TVal2> converter){
			DictionaryP<TKey,TVal2> r=new DictionaryP<TKey,TVal2>();
			foreach(Gen::KeyValuePair<TKey,TVal> pair in this){
				r.Add(pair.Key,converter(pair.Value));
			}
			return r;
		}
		[System.Serializable]
		private class ValuesCollection:Gen::ICollection<TVal>{
			DictionaryP<TKey,TVal> parent;

			public ValuesCollection(DictionaryP<TKey,TVal> parent){
				this.parent=parent;
			}

			#region ICollection<TVal> �����o
			public bool Contains(TVal item) {
				foreach(Gen::List<TVal> vals in parent.container.Values)
					if(vals.Contains(item))return true;
				return false;
			}
			public void CopyTo(TVal[] array,int arrayIndex) {
				foreach(Gen::List<TVal> vals in parent.container.Values){
					vals.CopyTo(array,arrayIndex);
					arrayIndex+=vals.Count;
				}
			}
			public int Count {
				get{
					int r=0;
					foreach(Gen::List<TVal> vals in parent.container.Values)r+=vals.Count;
					return r;
				}
			}

			public bool IsReadOnly{get{return true;}}
			public bool Remove(TVal item){
				throw new System.ApplicationException(Design.CollectionWrapper<TVal>.ERR_READONLY_REMOVE);
			}
			public void Add(TVal item){
				throw new System.Exception(Design.CollectionWrapper<TVal>.ERR_READONLY_ADD);
			}
			public void Clear(){
				throw new System.Exception(Design.CollectionWrapper<TVal>.ERR_READONLY_REMOVE);
			}
			#endregion

			#region IEnumerable<TVal> �����o
			public System.Collections.Generic.IEnumerator<TVal> GetEnumerator(){
				foreach(Gen::List<TVal> vals in parent.container.Values)
					foreach(TVal val in vals)
						yield return val;
			}
			#endregion

			#region IEnumerable �����o
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
				return this.GetEnumerator();
			}
			#endregion
		}
	}
}