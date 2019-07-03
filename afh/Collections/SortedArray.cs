using Gen=System.Collections.Generic;

/* 
 * [�\��]
 * Synchronization
 * ICloneable
 */

namespace afh.Collections{

	#region cls:SortedArray`1
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// �����l�̗v�f�͈�܂ł����i�[���鎖�͏o���܂���B
	/// </summary>
	/// <typeparam name="T">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public class SortedArray<T>:Design.SortedArrayBase<T>
		,Gen::ICollection<T>
		,Gen::IEnumerable<T>
		,System.Collections.IEnumerable
		where T:System.IComparable<T>,System.IEquatable<T>
	{
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArray(Gen::IList<T> original):base(original){
			// �d�����镨���폜
			for(int i=1;i<vals.Length;i++){
				if(this.vals[i-1].Equals(this.vals[i]))this.RemoveAt(--i);		
			}
		}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArray():this(4){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArray(int capacity):base(capacity){}
		/// <summary>
		/// �w�肵���l�� SortedArray ���ɉ�����ʒu���擾���܂��B
		/// </summary>
		/// <param name="val">��������l���w�肵�܂��B</param>
		/// <returns>�w�肵���l�� SortedArray ���ɉ�����ʒu��Ԃ��܂��B
		/// �w�肵���l�ȉ��̒l���o�^����Ă��Ȃ����ɂ� -1 ��Ԃ��܂��B</returns>
		public virtual int BinarySearchSup(T val){
			if(this.count==0)return 0;
			int d=0;			if(val.CompareTo(this.vals[d])<0)return 0;
			int u=this.count;	if(val.CompareTo(this.vals[u-1])>=0)return u;
			while(u>d){
				int c=(u+d)/2;
				int compare=val.CompareTo(this.vals[c]);
				if(compare<0){
					u=c;
				}else if(compare==0){
					return c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		//=================================================
		//		IList<T> �����o
		//=================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="val">�ǉ�����l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		public override void Add(T val){
			int target=this.BinarySearchSup(val);
			if(0<=target&&target<this.count&&this.vals[target].Equals(val)){
				// �㏑��
				this.vals[target]=val;
				return;
			}

			// �����ꏊ���m��
			int shiftlen=this.count-target;
			if(++this.count>=this.vals.Length){
				int len=vals.Length*2;
				T[] newkeys=new T[len];
				if(target>0){
					System.Array.Copy(this.vals,newkeys,target);
				}
				if(shiftlen>0){
					System.Array.Copy(this.vals,target,newkeys,target+1,shiftlen);
				}
				this.vals=newkeys;
			}else{
				if(shiftlen>0){
					System.Array.Copy(this.vals,target,this.vals,target+1,shiftlen);
				}
			}

			// �}��
			this.vals[target]=val;
		}
	}
	#endregion

	#region cls:SortedArrayP`1
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// ���������̒l���i�[���鎖���\�ł��B
	/// </summary>
	/// <typeparam name="T">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public sealed class SortedArrayP<T>:Design.SortedArrayBase<T>
		,Gen::ICollection<T>
		,Gen::IEnumerable<T>
		,System.Collections.IEnumerable
		where T:System.IComparable<T>,System.IEquatable<T> 
	{
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArrayP(Gen::IList<T> original):base(original){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArrayP():this(4){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArrayP(int capacity):base(capacity){}
		//===========================================================
		//		Binary Searches
		//===========================================================
		/// <summary>
		/// �w�肵�� key �̔ԍ����擾���܂��B��v���� key ����������ꍇ�ɂ͂��̒��ł��ŏ��̕���Ԃ��܂��B
		/// </summary>
		/// <param name="val">�w�肵�� key ���݂� SortedArray ���̈ʒu��Ԃ��܂��B</param>
		/// <returns>key �����������ꍇ�ɂ͂��̈ʒu��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public override int BinarySearch(T val){
			if(this.count==0)return -1;
			int d=0;
			int u=this.count;
			while(u>d){
				int c=(u+d)/2;
				int compare=val.CompareTo(this.vals[c]);
				if(compare<=0){
					u=c;
				}else{
					d=c+1;
				}
			}
			return d<this.count&&val.Equals(this.vals[d])?d:-1;
		}
		/// <summary>�w�肵���������g�B�̖��[���������܂��B���[�́A�Ō�̓K������g�̎����w���܂��B</summary>
		/// <remarks>
		/// <pre>
		/// ����������������������
		/// �@�@�@�@�@�@�@�@��
		/// </pre>
		/// </remarks>
		/// <param name="val">�ԍ����������� key ���w�肵�܂��B</param>
		/// <returns>
		/// �o�^����Ă��錮�ƒl�̑g�̒��ŁA�w�肵�����������̗̈�̍Ō�̎��̔ԍ���Ԃ��܂��B
		/// �w�肵���������g��������Ȃ������ꍇ�ɂ́A�w�肵�� key ���ǉ�������̔ԍ���Ԃ��܂��B
		/// </returns>
		public int BinarySearchSup(T val) {
			if(this.count==0)	return 0;
			int d=0;			if(val.CompareTo(this.vals[d])<0)	return 0;
			int u=this.count;	if(val.CompareTo(this.vals[u-1])>=0)return u;
			while(u>d) {
				int c=(u+d)/2;
				int compare=val.CompareTo(this.vals[c]);
				if(compare<0){
					u=c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		/// <summary>�w�肵���������g�̈�ԎႢ�ԍ����������܂��B</summary>
		/// <remarks>
		/// <pre>
		/// ����������������������
		/// �@�@�@�@��
		/// </pre>
		/// </remarks>
		/// <param name="val">�ԍ����������� key ���w�肵�܂��B</param>
		/// <returns>
		/// �o�^����Ă��錮�ƒl�̑g�̒��ŁA�w�肵�����������̗̈�̍ŏ��̔ԍ���Ԃ��܂��B
		/// �w�肵���������g��������Ȃ������ꍇ�ɂ́A�w�肵�� key ���ǉ�������̔ԍ���Ԃ��܂��B
		/// </returns>
		public int BinarySearchInf(T val){
			if(this.count==0)	return 0;
			int d=0;			if(val.CompareTo(this.vals[d])<0)	return 0;
			int u=this.count;	if(val.CompareTo(this.vals[u-1])>=0)return u;
			while(u>d) {
				int c=(u+d)/2;
				int compare=val.CompareTo(this.vals[c]);
				if(compare<=0) {
					u=c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		//===========================================================
		//		Add
		//===========================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="val">�ǉ�����l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		public override void Add(T val) {
		// SortedArray`2.Add �̓��e�̏㏑���������폜
		//-------------------------------------------------
			int target=this.BinarySearchSup(val);
		//	if(target>=0&&this.keys[target].Equals(key)) {
		//		// �㏑��
		//		this.keys[target]=key;
		//		this.vals[target]=value;
		//		return;
		//	}

			// �����ꏊ���m��
			int shiftlen=this.count-target;
			if(++this.count>=this.vals.Length) {
				int len=vals.Length*2;
				T[] newkeys=new T[len];
				if(target>0){
					System.Array.Copy(this.vals,newkeys,target);
				}
				if(shiftlen>0){
					System.Array.Copy(this.vals,target,newkeys,target+1,shiftlen);
				}
				this.vals=newkeys;
			}else{
				if(shiftlen>0) {
					System.Array.Copy(this.vals,target,this.vals,target+1,shiftlen);
				}
			}

			// �}��
			this.vals[target]=val;
		}
	}
	#endregion

	#region cls:SortedArray`2
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// ���� key �ɑ΂��Ĉ�̒l�����Ή��Â����܂���B
	/// </summary>
	/// <typeparam name="TKey">����Ɏg�p���� Key �̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public class SortedArray<TKey,TVal>:Design.SortedArrayBase<TKey,TVal>
		,Gen::IDictionary<TKey,TVal>
		,Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>
		,Gen::IEnumerable<Gen::KeyValuePair<TKey,TVal>>
		,System.Collections.IEnumerable
		where TKey:System.IComparable<TKey>,System.IEquatable<TKey>
	{
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArray(Gen::IDictionary<TKey,TVal> original):base(original){
			// �d�����镨���폜
			for(int i=1;i<keys.Length;i++) {
				if(this.keys[i-1].Equals(this.keys[i]))this.RemoveAt(--i);
			}
		}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArray():this(4){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArray(int capacity):base(capacity){}
		/// <summary>
		/// �w�肵���l�� SortedArray ���ɉ�����ʒu���擾���܂��B
		/// </summary>
		/// <param name="key">�������� key ���w�肵�܂��B</param>
		/// <returns>�w�肵���l�� SortedArray ���ɉ�����ʒu��Ԃ��܂��B
		/// �w�肵�� key �ȉ��� key ���o�^����Ă��Ȃ����ɂ� -1 ��Ԃ��܂��B</returns>
		public virtual int BinarySearchSup(TKey key){
			if(this.count==0)return 0;
			int d=0;			if(key.CompareTo(this.keys[d])<0)return 0;
			int u=this.count;	if(key.CompareTo(this.keys[u-1])>=0)return u;
			while(u>d){
				int c=(u+d)/2;
				int compare=key.CompareTo(this.keys[c]);
				if(compare<0){
					u=c;
				}else if(compare==0){
					return c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		//=================================================
		//		IDictionary<TKey,TVal> �����o
		//=================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="key">�ǉ�����l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		/// <param name="value">�ǉ�����l���w�肵�܂��B</param>
		public override void Add(TKey key,TVal value) {
			int target=this.BinarySearchSup(key);
			if(0<=target&&target<this.count&&this.keys[target].Equals(key)) {
				// �㏑��
				this.keys[target]=key;
				this.vals[target]=value;
				return;
			}

			// �����ꏊ���m��
			int shiftlen=this.count-target;
			if(++this.count>=this.keys.Length){
				int len=keys.Length*2;
				TKey[] newkeys=new TKey[len];
				TVal[] newvals=new TVal[len];
				if(target>0){
					System.Array.Copy(this.keys,newkeys,target);
					System.Array.Copy(this.vals,newvals,target);
				}
				if(shiftlen>0){
					System.Array.Copy(this.keys,target,newkeys,target+1,shiftlen);
					System.Array.Copy(this.vals,target,newvals,target+1,shiftlen);
				}
				this.keys=newkeys;
				this.vals=newvals;
			}else{
				if(shiftlen>0){
					System.Array.Copy(this.keys,target,this.keys,target+1,shiftlen);
					System.Array.Copy(this.vals,target,this.vals,target+1,shiftlen);
				}
			}

			// �}��
			this.keys[target]=key;
			this.vals[target]=value;
		}
		/// <summary>
		/// �w�肵�� key �Ɗ֘A�t����ꂽ�l���폜���܂��B
		/// </summary>
		/// <param name="key">�폜���錮���w�肵�܂��B</param>
		/// <returns>�w�肵�� key �Ɋ֘A�t����ꂽ�l�����݂��č폜���ꂽ�ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Remove(TKey key){
			int index=this.BinarySearch(key);
			if(index<0)return false;
			this.RemoveAt(index);
			return true;
		}
		/// <summary>
		/// �w�肵�����Ɋ֘A�t����ꂽ�l�����݂��邩�ǂ����m�F���A���݂����ꍇ�ɂ͂��̒l���擾���܂��B
		/// </summary>
		/// <param name="key">�֘A�t����ꂽ�l�����݂��邩�ǂ������m�F���錮���w�肵�܂��B</param>
		/// <param name="value">�֘A�t����ꂽ�l�����݂����ꍇ�ɂ��̒l��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ͌^�̊���l��Ԃ��܂��B</param>
		/// <returns>�֘A�t���ꂽ�l�����݂������� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool TryGetValue(TKey key,out TVal value) {
			int index=this.BinarySearch(key);
			if(index<0){
				value=default(TVal);
				return false;
			}else{
				value=this.vals[index];
				return true;
			}
		}
		/// <summary>
		/// �w�肵�����ɑΉ�����l��Ԃ��܂��B
		/// </summary>
		/// <param name="key">�擾�������l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		/// <returns>�w�肵�����Ɋ֘A�t�����Ă���l��Ԃ��܂��B</returns>
		public TVal this[TKey key]{
			get{
				int index=this.BinarySearch(key);
				if(index<0)throw new System.ArgumentOutOfRangeException("key",key,"�w�肵���I�u�W�F�N�g�� SortedArray �� key �Ƃ��Ċ܂܂�Ă��܂���B");
				return this.vals[index];
			}
			set{
				int index=this.BinarySearch(key);
				if(index<0)this.Add(key,value);
				else this.vals[index]=value;
			}
		}
	}
	#endregion

	#region cls:SortedArrayP`2
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// ���� key �ɑ΂��ĕ����̒l��Ή��t���鎖���\�ł��B
	/// ���� key �����l�̊i�[����鏇�Ԃ͕s��ł��B
	/// </summary>
	/// <typeparam name="TKey">����Ɏg�p���� Key �̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public sealed class SortedArrayP<TKey,TVal>:Design.SortedArrayBase<TKey,TVal>
		,IDictionaryP<TKey,TVal>
		,Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>
		,Gen::IEnumerable<Gen::KeyValuePair<TKey,TVal>>
		,System.Collections.IEnumerable
		where TKey:System.IComparable<TKey>,System.IEquatable<TKey> 
	{
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArrayP(Gen::IDictionary<TKey,TVal> original):base(original){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArrayP():this(4){}
		/// <summary>
		/// SortedArray �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArrayP(int capacity):base(capacity){}
		//===========================================================
		//		Binary Searches
		//===========================================================
		/// <summary>
		/// �w�肵�� key �̔ԍ����擾���܂��B��v���� key ����������ꍇ�ɂ͂��̒��ł��ŏ��̕���Ԃ��܂��B
		/// </summary>
		/// <param name="key">�w�肵�� key ���݂� SortedArray ���̈ʒu��Ԃ��܂��B</param>
		/// <returns>key �����������ꍇ�ɂ͂��̈ʒu��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public override int BinarySearch(TKey key){
			if(this.count==0)return -1;
			int d=0;
			int u=this.count;
			while(u>d){
				int c=(u+d)/2;
				int compare=key.CompareTo(this.keys[c]);
				if(compare<=0){
					u=c;
				}else{
					d=c+1;
				}
			}
			return d<this.count&&key.Equals(this.keys[d])?d:-1;
		}
		/// <summary>�w�肵���������g�B�̖��[���������܂��B���[�́A�Ō�̓K������g�̎����w���܂��B</summary>
		/// <remarks>
		/// <pre>
		/// ����������������������
		/// �@�@�@�@�@�@�@�@��
		/// </pre>
		/// </remarks>
		/// <param name="key">�ԍ����������� key ���w�肵�܂��B</param>
		/// <returns>
		/// �o�^����Ă��錮�ƒl�̑g�̒��ŁA�w�肵�����������̗̈�̍Ō�̎��̔ԍ���Ԃ��܂��B
		/// �w�肵���������g��������Ȃ������ꍇ�ɂ́A�w�肵�� key ���ǉ�������̔ԍ���Ԃ��܂��B
		/// </returns>
		public int BinarySearchSup(TKey key) {
			if(this.count==0)	return 0;
			int d=0;			if(key.CompareTo(this.keys[d])<0)	return 0;
			int u=this.count;	if(key.CompareTo(this.keys[u-1])>=0)return u;
			while(u>d) {
				int c=(u+d)/2;
				int compare=key.CompareTo(this.keys[c]);
				if(compare<0){
					u=c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		/// <summary>�w�肵���������g�̈�ԎႢ�ԍ����������܂��B</summary>
		/// <param name="key">�ԍ����������� key ���w�肵�܂��B</param>
		/// <remarks>
		/// <pre>
		/// ����������������������
		/// �@�@�@�@��
		/// </pre>
		/// </remarks>
		/// <returns>
		/// �o�^����Ă��錮�ƒl�̑g�̒��ŁA�w�肵�����������̗̈�̍ŏ��̔ԍ���Ԃ��܂��B
		/// �w�肵���������g��������Ȃ������ꍇ�ɂ́A�w�肵�� key ���ǉ�������̔ԍ���Ԃ��܂��B
		/// </returns>
		public int BinarySearchInf(TKey key){
			if(this.count==0)	return 0;
			int d=0;			if(key.CompareTo(this.keys[d])<0)	return 0;
			int u=this.count;	if(key.CompareTo(this.keys[u-1])>=0)return u;
			while(u>d) {
				int c=(u+d)/2;
				int compare=key.CompareTo(this.keys[c]);
				if(compare<=0) {
					u=c;
				}else{
					d=c+1;
				}
			}
			return d;
		}
		//===========================================================
		//		Add
		//===========================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="key">�ǉ�����l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		/// <param name="value">�ǉ�����l���w�肵�܂��B</param>
		public override void Add(TKey key,TVal value) {
		// SortedArray`2.Add �̓��e�̏㏑���������폜
		//-------------------------------------------------
			int target=this.BinarySearchSup(key);
		//	if(target>=0&&this.keys[target].Equals(key)) {
		//		// �㏑��
		//		this.keys[target]=key;
		//		this.vals[target]=value;
		//		return;
		//	}

			// �����ꏊ���m��
			int shiftlen=this.count-target;
			if(++this.count>=this.keys.Length) {
				int len=keys.Length*2;
				TKey[] newkeys=new TKey[len];
				TVal[] newvals=new TVal[len];
				if(target>0){
					System.Array.Copy(this.keys,newkeys,target);
					System.Array.Copy(this.vals,newvals,target);
				}
				if(shiftlen>0){
					System.Array.Copy(this.keys,target,newkeys,target+1,shiftlen);
					System.Array.Copy(this.vals,target,newvals,target+1,shiftlen);
				}
				this.keys=newkeys;
				this.vals=newvals;
			}else{
				if(shiftlen>0) {
					System.Array.Copy(this.keys,target,this.keys,target+1,shiftlen);
					System.Array.Copy(this.vals,target,this.vals,target+1,shiftlen);
				}
			}

			// �}��
			this.keys[target]=key;
			this.vals[target]=value;
		}

		//===========================================================
		//		IDictionaryP<TKey,TVal> �����o
		//===========================================================
		/// <summary>
		/// �w�肵�����ƒl�̑g���폜���܂��B
		/// �����g�������܂܂�Ă���ꍇ�ɂ͈���������폜����܂���B
		/// </summary>
		/// <param name="key">�폜����g�̌����w�肵�܂��B</param>
		/// <param name="val">�폜����g�̒l���w�肵�܂��B</param>
		/// <returns>�w�肵�����ƒl�̑g��������폜���ꂽ�ꍇ�� true ��Ԃ��܂��B</returns>
		public bool Remove(TKey key,TVal val){
			int inf=BinarySearchInf(key);
			int sup=BinarySearchSup(key);
			while(inf<sup){
				if(this.vals[inf].Equals(val)){
					this.RemoveAt(inf);
					return true;
				}
				inf++;
			}
			return false;
		}
		/// <summary>
		/// �w�肵�����ƒl�̑g���܂܂�Ă��邩�ǂ�����Ԃ��܂��B
		/// </summary>
		/// <param name="key">�܂܂�Ă��邩�ǂ������m�F����g�̌����w�肵�܂��B</param>
		/// <param name="val">�܂܂�Ă��邩�ǂ������m�F����g�̒l���w�肵�܂��B</param>
		/// <returns>�w�肵���g���܂܂�Ă����ꍇ�ɂ� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Contains(TKey key,TVal val) {
			int inf=BinarySearchInf(key);
			int sup=BinarySearchSup(key);
			while(inf<sup){
				if(this.vals[inf].Equals(val))return true;
				inf++;
			}
			return false;
		}
		/// <summary>
		/// �w�肵���������g�̒��ŁA�w�肵���ԍ��̕����폜���܂��B
		/// </summary>
		/// <param name="key">�폜����g�̌����w�肵�܂��B</param>
		/// <param name="index">�폜����g������ 0 ����n�܂�ԍ����w�肵�܂��B</param>
		/// <returns>�w�肵���������g���A�w�肵���ԍ������������݂��đg���폜���鎖���o�����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool RemoveAt(TKey key,int index) {
			int inf=BinarySearchInf(key);
			int sup=BinarySearchSup(key);
			inf+=index;
			if(inf<sup){
				this.RemoveAt(inf);
				return true;
			}
			return false;
		}
		/// <summary>
		/// �w�肵���������g��S�č폜���܂��B
		/// </summary>
		/// <param name="key">�폜����g�̌����w�肵�܂��B</param>
		/// <returns>�w�肵���������g��������폜�����ꍇ�ɂ� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool RemoveAll(TKey key){
			int inf=BinarySearchInf(key);
			int sup=BinarySearchSup(key);
			int len=sup-inf;
			if(len>0){
				this.count-=len;
				len=this.count-inf;
				System.Array.Copy(this.keys,sup,this.keys,inf,len);
				System.Array.Copy(this.vals,sup,this.vals,inf,len);
				return true;
			}else return false;
		}
		/// <summary>
		/// �w�肵���������g�̒l��S�Ĕz��ɂ��ĕԂ��܂��B
		/// </summary>
		/// <param name="key">�擾����l�Ɋ֘A�t�����Ă��錮���w�肵�Ă��܂��B</param>
		/// <returns>�w�肵�����Ɋ֘A�t�����Ă���l��S�Ĕz��Ɋi�[���ĕԂ��܂��B</returns>
		public TVal[] this[TKey key] {
			get {
				int inf=BinarySearchInf(key);
				int sup=BinarySearchSup(key);
				int len=sup-inf;
				TVal[] ret=new TVal[len];
				if(len>0)System.Array.Copy(this.vals,inf,ret,0,len);
				return ret;
			}
		}
	}
	#endregion

}

namespace afh.Collections.Design{

	#region cls:SortedArrayBase`1
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// </summary>
	/// <typeparam name="T">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public abstract class SortedArrayBase<T>:Design.CollectionBase,Gen::IList<T>
		where T:System.IComparable<T>,System.IEquatable<T>{
		/// <summary>
		/// ����ێ�����z��ł��B
		/// </summary>
		protected T[] vals;
		/// <summary>
		/// ���ݔz��Ɋ܂܂�Ă���v�f�̐���ێ����܂��B
		/// </summary>
		protected int count;
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArrayBase(Gen::IList<T> original):this(original.Count+4) {
			if(original==null) throw new System.ArgumentNullException("original");
			original.CopyTo(this.vals,0);
			System.Array.Sort<T>(this.vals);
			this.count=original.Count;
		}
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArrayBase():this(4){ }
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArrayBase(int capacity) {
			int len=capacity<4?4:capacity;
			this.vals=new T[len];
			this.count=0;
		}
		/// <summary>
		/// �w�肵���l�̔ԍ����擾���܂��B
		/// </summary>
		/// <param name="val">��������l���w�肵�܂��B</param>
		/// <returns>val �����������ꍇ�ɂ͂��̈ʒu��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public virtual int BinarySearch(T val) {
			int d=0;			// �����Ώۂ̏���
			int u=this.count;	// �����Ώۂ̖��[: [u] ���̂͊܂܂Ȃ�
			while(u>d) {
				int c=(u+d)/2;
				int compare=val.CompareTo(this.vals[c]);
				if(compare<0) {
					u=c;
				}else if(compare==0){
					return c;
				}else{
					d=c+1;
				}
			}
			return -1;
		}
		//=============================================
		//		Accessor
		//=============================================
		/// <summary>
		/// �w�肵���ԍ��ɑΉ�����l���擾���܂��B
		/// </summary>
		/// <param name="index">�擾����l�̔ԍ����w�肵�܂��B</param>
		/// <returns>�w�肵���ԍ��ɑΉ�����l��Ԃ��܂��B</returns>
		public T this[int index] {
			get{
				if(index<0||this.count<=index)throw new System.ArgumentOutOfRangeException("index");
				return this.vals[index];
			}
		}
		//=================================================
		//		IEnumerable<KeyValuePair<TKey,TVal>> �����o
		//=================================================
		/// <summary>
		/// ���ƒl�̑g��񋓂���񋓎q���擾���܂��B
		/// </summary>
		/// <returns>���ƒl�̑g��񋓂���񋓎q��Ԃ��܂��B</returns>
		public Gen::IEnumerator<T> GetEnumerator() {
			int initial_ver=this.version;
			for(int i=0,iM=this.count;i<iM;i++) {
				if(this.version!=initial_ver) throw new CollectionChangedException();
				yield return this.vals[i];
			}
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
		//=================================================
		//		ICollection<T> �����o
		//=================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="item">�ǉ�����v�f���w�肵�܂��B</param>
		public abstract void Add(T item);
		/// <summary>
		/// �o�^����Ă���v�f��S�č폜���܂��B
		/// </summary>
		public void Clear() {
			System.Array.Clear(this.vals,0,this.count);
			this.count=0;
		}
		/// <summary>
		/// �w�肵���l���܂܂�Ă��邩�ǂ������m�F���܂��B
		/// </summary>
		/// <param name="item">�܂܂�Ă��邩�ǂ����m�F����l���w�肵�܂��B</param>
		/// <returns>�w�肵���l���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Contains(T item){
			return 0<=this.BinarySearch(item);
		}
		/// <summary>
		/// �w�肵���z��ɂ��� SortedArray �̓��e���R�s�[���܂��B
		/// </summary>
		/// <param name="array">�R�s�[��̔z����w�肵�܂��B</param>
		/// <param name="arrayIndex">�R�s�[��̔z��̒��ł̃R�s�[�J�n�ʒu���w�肵�܂��B</param>
		public void CopyTo(T[] array,int arrayIndex) {
			// check
			const string OUTofRANGE="arrayIndex �� array �z��̓Y�����͈̔͂ɓ����Ă��Ȃ��� array ���R�s�[��̔z��Ƃ��ĒZ�����܂��B";
			if(array==null) throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex+this.count>array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex",OUTofRANGE);

			System.Array.Copy(this.vals,0,array,arrayIndex,this.count);
		}
		/// <summary>
		/// ���� SortedArray �Ɋ܂܂�Ă���v�f�̐����擾���܂��B
		/// </summary>
		public int Count {
			get { return this.count; }
		}
		/// <summary>
		/// ���� SortedArray ���ǂݎ���p���ǂ������擾���܂��B
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}
		/// <summary>
		/// �w�肵���l������ SortedArray ����폜���܂��B
		/// </summary>
		/// <param name="item">�폜����l���w�肵�܂��B</param>
		/// <returns>�w�肵���l���������č폜���鎖���o�����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Remove(T item){
			int index=this.BinarySearch(item);
			if(index>=0){
				this.RemoveAt(index);
				return true;
			}
			return false;
		}
		//=================================================
		//		IList<KeyValuePair<TKey,TVal>> �����o
		//=================================================
		/// <summary>
		/// �w�肵���l�̈ʒu���擾���܂��B
		/// </summary>
		/// <param name="item">�ʒu��m�肽���l���w�肵�܂��B</param>
		/// <returns>�w�肵���l�����������ꍇ�ɂ��̈ʒu��Ԃ��܂��B
		/// �w�肵���l��������Ȃ������ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public int IndexOf(T item){
			return this.BinarySearch(item);
		}
		/// <summary>
		/// �l�� SortedArray �ɒǉ����܂��B
		/// </summary>
		/// <param name="index">�{���͑}������ʒu���w�肷�镨�ł����A���̃R���N�V�����ł͏�ɐ��񂳂��̂Ŏw�肵�Ă��Ӗ�����܂���B</param>
		/// <param name="item">�ǉ�����l���w�肵�܂��B</param>
		public void Insert(int index,T item) {
			this.Add(item);
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ���l���폜���܂��B
		/// </summary>
		/// <param name="index">�l�̈ʒu���w�肵�܂��B</param>
		public void RemoveAt(int index) {
			if(index<0||this.count<=index) throw new System.ArgumentOutOfRangeException("index");
			this.count--;
			int len=this.count-index;
			if(len>0) {
				System.Array.Copy(this.vals,index+1,this.vals,index,len);
			}
			this.vals[this.count]=default(T);
		}
		T System.Collections.Generic.IList<T>.this[int index]{
			get {
				if(index<0||this.count<=index)throw new System.ArgumentOutOfRangeException("index");
				return this.vals[index];
			}
			set { this.Add(value); }
		}
	}
	#endregion

	#region cls:SortedArrayBase`2
	/// <summary>
	/// �v�f����ɐ��񂳂ꂽ�ϒ��z����������܂��B
	/// ���� key �ɑ΂��Ĉ�̒l�����Ή��Â����܂���B
	/// </summary>
	/// <typeparam name="TKey">����Ɏg�p���� Key �̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">���񂳂��l�̌^���w�肵�܂��B</typeparam>
	[System.Serializable]
	public abstract class SortedArrayBase<TKey,TVal>:Design.CollectionBase,Gen::IList<Gen::KeyValuePair<TKey,TVal>>
		where TKey:System.IComparable<TKey>,System.IEquatable<TKey> {
		/// <summary>
		/// ����ێ�����z��ł��B
		/// </summary>
		protected TKey[] keys;
		/// <summary>
		/// �l��ێ�����z��ł��B
		/// </summary>
		protected TVal[] vals;
		/// <summary>
		/// ���ݔz��Ɋ܂܂�Ă���v�f�̐���ێ����܂��B
		/// </summary>
		protected int count;
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="original">�쐬�̌��ƂȂ�R���N�V�������w�肵�܂��B
		/// ���̃R���N�V�����̓��e���Ȃď���������܂��B</param>
		public SortedArrayBase(Gen::IDictionary<TKey,TVal> original):this(original.Count+4){
			if(original==null)throw new System.ArgumentNullException("original");
			original.Keys.CopyTo(this.keys,0);
			original.Values.CopyTo(this.vals,0);
			System.Array.Sort<TKey,TVal>(this.keys,this.vals);
			this.count=original.Count;
		}
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		public SortedArrayBase():this(4){}
		/// <summary>
		/// SortedArrayBase �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="capacity">�ŏ��Ɋm�ۂ���e�ʂ��w�肵�܂��B</param>
		public SortedArrayBase(int capacity) {
			int len=capacity<4?4:capacity;
			this.keys=new TKey[len];
			this.vals=new TVal[len];
			this.count=0;
		}
		/// <summary>
		/// �w�肵�� key �̔ԍ����擾���܂��B
		/// </summary>
		/// <param name="key">��������g�̌����w�肵�܂��B</param>
		/// <returns>key �����������ꍇ�ɂ͂��̈ʒu��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public virtual int BinarySearch(TKey key) {
			int d=0;			// �����Ώۂ̏���
			int u=this.count;	// �����Ώۂ̖��[: [u] ���̂͊܂܂Ȃ�
			while(u>d) {
				int c=(u+d)/2;
				int compare=key.CompareTo(this.keys[c]);
				if(compare<0) {
					u=c;
				} else if(compare==0) {
					return c;
				} else {
					d=c+1;
				}
			}
			return -1;
		}
		//=============================================
		//		Accessor
		//=============================================
		/// <summary>
		/// �w�肵���ԍ��ɑΉ����錮�ƒl�̑g������ SortedArrayEntry ���擾���܂��B
		/// </summary>
		/// <param name="index">�擾���錮�ƒl�̑g�̔ԍ����w�肵�܂��B</param>
		/// <returns>�w�肵���ԍ��ɑΉ����� SortedArrayEntry ��Ԃ��܂��B</returns>
		public SortedArrayEntry this[int index] {
			get {
				if(index<0||this.count<=index) throw new System.ArgumentOutOfRangeException("index");
				return new SortedArrayEntry(this,index);
			}
		}
		/// <summary>
		/// ����� SortedArray �̌��ƒl�̑g�ɃA�N�Z�X����ׂ̍\���̂ł��B
		/// </summary>
		[System.Serializable]
		public struct SortedArrayEntry {
			private int index;
			private SortedArrayBase<TKey,TVal> array;
			private int version;


			internal SortedArrayEntry(SortedArrayBase<TKey,TVal> array,int index) {
				if(index<0||array.count<=index) throw new System.ArgumentOutOfRangeException("index",index,"�w�肵���ԍ��� SoryedArray �̗v�f�͑��݂��܂���B");
				this.version=array.version;
				this.array=array;
				this.index=index;
			}
			/// <summary>
			/// Key ���擾���͐ݒ肵�܂��B
			/// </summary>
			public TKey Key {
				get {
					if(this.version!=this.array.version) throw new CollectionChangedException();
					return this.array.keys[this.index];
				}
				internal set {
					if(this.version!=this.array.version) throw new CollectionChangedException();
					this.array.keys[this.index]=value;
				}
			}
			/// <summary>
			/// �l���擾���͐ݒ肵�܂��B
			/// </summary>
			public TVal Value {
				get {
					if(this.version!=this.array.version) throw new CollectionChangedException();
					return this.array.vals[this.index];
				}
				set {
					if(this.version!=this.array.version) throw new CollectionChangedException();
					this.array.vals[this.index]=value;
				}
			}
		}
		//=================================================
		//		IEnumerable<KeyValuePair<TKey,TVal>> �����o
		//=================================================
		/// <summary>
		/// ���ƒl�̑g��񋓂���񋓎q���擾���܂��B
		/// </summary>
		/// <returns>���ƒl�̑g��񋓂���񋓎q��Ԃ��܂��B</returns>
		public Gen::IEnumerator<Gen::KeyValuePair<TKey,TVal>> GetEnumerator(){
			int initial_ver=this.version;
			for(int i=0,iM=this.count;i<iM;i++) {
				if(this.version!=initial_ver) throw new CollectionChangedException();
				yield return new Gen::KeyValuePair<TKey,TVal>(this.keys[i],this.vals[i]);
			}
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
		//=================================================
		//		ICollection<KeyValuePair<TKey,TVal>> �����o
		//=================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="item">�ǉ�����v�f���w�肵�܂��B</param>
		public void Add(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			this.Add(item.Key,item.Value);
		}
		/// <summary>
		/// �o�^����Ă���v�f��S�č폜���܂��B
		/// </summary>
		public void Clear() {
			System.Array.Clear(this.keys,0,this.count);
			System.Array.Clear(this.vals,0,this.count);
			this.count=0;
		}
		/// <summary>
		/// �w�肵���l�̑g���܂܂�Ă��邩�ǂ������m�F���܂��B
		/// </summary>
		/// <param name="item">�܂܂�Ă��邩�ǂ����m�F����l�̑g���w�肵�܂��B</param>
		/// <returns>�w�肵���l�̑g���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool Contains(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			int index;
			return 0<=(index=this.BinarySearch(item.Key))&&this.vals[index].Equals(item.Value);
		}
		/// <summary>
		/// �w�肵���z��ɂ��� SortedArray �̓��e���R�s�[���܂��B
		/// </summary>
		/// <param name="array">�R�s�[��̔z����w�肵�܂��B</param>
		/// <param name="arrayIndex">�R�s�[��̔z��̒��ł̃R�s�[�J�n�ʒu���w�肵�܂��B</param>
		public void CopyTo(System.Collections.Generic.KeyValuePair<TKey,TVal>[] array,int arrayIndex) {
			// check
			const string OUTofRANGE="arrayIndex �� array �z��̓Y�����͈̔͂ɓ����Ă��Ȃ��� array ���R�s�[��̔z��Ƃ��ĒZ�����܂��B";
			if(array==null) throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex+this.count>array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex",OUTofRANGE);

			// copy
			for(int i=0;i<this.count;i++)
				array[arrayIndex++]=new Gen::KeyValuePair<TKey,TVal>(this.keys[i],this.vals[i]);
		}
		/// <summary>
		/// ���� SortedArray �Ɋ܂܂�Ă���v�f�̐����擾���܂��B
		/// </summary>
		public int Count{
			get{return this.count;}
		}
		/// <summary>
		/// ���� SortedArray ���ǂݎ���p���ǂ������擾���܂��B
		/// </summary>
		public bool IsReadOnly {
			get {return false;}
		}
		/// <summary>
		/// �w�肵�����ƒl�̑g������ SortedArray ����폜���܂��B
		/// </summary>
		/// <param name="item">�폜���錮�ƒl�̑g���w�肵�܂��B</param>
		/// <returns>�w�肵�����ƒl�̑g���������č폜���鎖���o�����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public virtual bool Remove(System.Collections.Generic.KeyValuePair<TKey,TVal> item){
			int index=this.BinarySearch(item.Key);
			if(index>=0&&this.vals[index].Equals(item.Value)){
				this.RemoveAt(index);
				return true;
			}
			return false;
		}
		//=================================================
		//		IDictionary<TKey,TVal> �����o
		//=================================================
		/// <summary>
		/// �w�肵���v�f��ǉ����܂��B
		/// </summary>
		/// <param name="key">�ǉ�����l�Ɋ֘A�t����ꂽ�����w�肵�܂��B</param>
		/// <param name="value">�ǉ�����l���w�肵�܂��B</param>
		public abstract void Add(TKey key,TVal value);
		/// <summary>
		/// �w�肵���������� SortedArray �Ɋ܂܂�Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="key">�܂܂�Ă��邩�ǂ�����m�肽�������w�肵�܂��B</param>
		/// <returns>�w�肵���������̔z��Ɋ܂܂�Ă������� true ������ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public bool ContainsKey(TKey key) {
			return 0<=this.BinarySearch(key);
		}
		/// <summary>
		/// �ǂݎ���p�� TKey �R���N�V�������擾���܂��B
		/// </summary>
		public Gen::ICollection<TKey> Keys {
			get{return new Design.ReadOnlyCollectionWrapper<TKey>(this.keys,this.count,this);}
		}
		/// <summary>
		/// TVal �R���N�V�������擾���܂��B
		/// </summary>
		public Gen::ICollection<TVal> Values {
			get{return new Design.ReadOnlyCollectionWrapper<TVal>(this.vals,this.count,this);}
		}
		//=================================================
		//		IList<KeyValuePair<TKey,TVal>> �����o
		//=================================================
		/// <summary>
		/// �w�肵�����ƒl�̑g�̈ʒu���擾���܂��B
		/// </summary>
		/// <param name="item">�ʒu��m�肽�����ƒl�̑g���w�肵�܂��B</param>
		/// <returns>�w�肵�����ƒl�̑g�����������ꍇ�ɂ��̈ʒu��Ԃ��܂��B
		/// �w�肵�����ƒl�̑g��������Ȃ������ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public int IndexOf(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			int index=this.BinarySearch(item.Key);
			return index>=0&&this.vals[index].Equals(item.Value)?index:-1;
		}
		/// <summary>
		/// ���ƒl�̑g�� SortedArray �ɒǉ����܂��B
		/// </summary>
		/// <param name="index">�{���͑}������ʒu���w�肷�镨�ł����A���̃R���N�V�����ł͏�ɐ��񂳂��̂Ŏw�肵�Ă��Ӗ�����܂���B</param>
		/// <param name="item">�ǉ����錮�ƒl�̑g���w�肵�܂��B</param>
		public void Insert(int index,System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			this.Add(item.Key,item.Value);
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ��錮�ƒl�̑g���폜���܂��B
		/// </summary>
		/// <param name="index">���ƒl�̑g�̈ʒu���w�肵�܂��B</param>
		public void RemoveAt(int index) {
			if(index<0||this.count<=index) throw new System.ArgumentOutOfRangeException("index");
			this.count--;
			int len=this.count-index;
			if(len>0) {
				System.Array.Copy(this.keys,index+1,this.keys,index,len);
				System.Array.Copy(this.vals,index+1,this.keys,index,len);
			}
			this.keys[this.count]=default(TKey);
			this.vals[this.count]=default(TVal);
		}
		Gen::KeyValuePair<TKey,TVal> System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<TKey,TVal>>.this[int index] {
			get {
				if(index<0||this.count<=index) throw new System.ArgumentOutOfRangeException("index");
				return new Gen::KeyValuePair<TKey,TVal>(this.keys[index],this.vals[index]);
			}
			set { this.Add(value.Key,value.Value); }
		}
	}
	#endregion

}