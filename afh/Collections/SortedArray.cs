using Gen=System.Collections.Generic;

/* 
 * [予定]
 * Synchronization
 * ICloneable
 */

namespace afh.Collections{

	#region cls:SortedArray`1
	/// <summary>
	/// 要素が常に整列された可変長配列を実装します。
	/// 同じ値の要素は一つまでしか格納する事は出来ません。
	/// </summary>
	/// <typeparam name="T">整列される値の型を指定します。</typeparam>
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
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArray(Gen::IList<T> original):base(original){
			// 重複する物を削除
			for(int i=1;i<vals.Length;i++){
				if(this.vals[i-1].Equals(this.vals[i]))this.RemoveAt(--i);		
			}
		}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		public SortedArray():this(4){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArray(int capacity):base(capacity){}
		/// <summary>
		/// 指定した値の SortedArray 内に於ける位置を取得します。
		/// </summary>
		/// <param name="val">検索する値を指定します。</param>
		/// <returns>指定した値の SortedArray 内に於ける位置を返します。
		/// 指定した値以下の値が登録されていない時には -1 を返します。</returns>
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
		//		IList<T> メンバ
		//=================================================
		/// <summary>
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="val">追加する値に関連付けられた鍵を指定します。</param>
		public override void Add(T val){
			int target=this.BinarySearchSup(val);
			if(0<=target&&target<this.count&&this.vals[target].Equals(val)){
				// 上書き
				this.vals[target]=val;
				return;
			}

			// 入れる場所を確保
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

			// 挿入
			this.vals[target]=val;
		}
	}
	#endregion

	#region cls:SortedArrayP`1
	/// <summary>
	/// 要素が常に整列された可変長配列を実装します。
	/// 同じ複数の値を格納する事が可能です。
	/// </summary>
	/// <typeparam name="T">整列される値の型を指定します。</typeparam>
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
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArrayP(Gen::IList<T> original):base(original){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		public SortedArrayP():this(4){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArrayP(int capacity):base(capacity){}
		//===========================================================
		//		Binary Searches
		//===========================================================
		/// <summary>
		/// 指定した key の番号を取得します。一致する key が複数ある場合にはその中でも最初の物を返します。
		/// </summary>
		/// <param name="val">指定した key が在る SortedArray 中の位置を返します。</param>
		/// <returns>key が見つかった場合にはその位置を返します。それ以外の場合には -1 を返します。</returns>
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
		/// <summary>指定した鍵を持つ組達の末端を検索します。末端は、最後の適合する組の次を指します。</summary>
		/// <remarks>
		/// <pre>
		/// □□□□■■■■□□□
		/// 　　　　　　　　↑
		/// </pre>
		/// </remarks>
		/// <param name="val">番号を検索する key を指定します。</param>
		/// <returns>
		/// 登録されている鍵と値の組の中で、指定した鍵を持つ物の領域の最後の次の番号を返します。
		/// 指定した鍵を持つ組が見つからなかった場合には、指定した key が追加される先の番号を返します。
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
		/// <summary>指定した鍵を持つ組の一番若い番号を検索します。</summary>
		/// <remarks>
		/// <pre>
		/// □□□□■■■■□□□
		/// 　　　　↑
		/// </pre>
		/// </remarks>
		/// <param name="val">番号を検索する key を指定します。</param>
		/// <returns>
		/// 登録されている鍵と値の組の中で、指定した鍵を持つ物の領域の最初の番号を返します。
		/// 指定した鍵を持つ組が見つからなかった場合には、指定した key が追加される先の番号を返します。
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
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="val">追加する値に関連付けられた鍵を指定します。</param>
		public override void Add(T val) {
		// SortedArray`2.Add の内容の上書き部分を削除
		//-------------------------------------------------
			int target=this.BinarySearchSup(val);
		//	if(target>=0&&this.keys[target].Equals(key)) {
		//		// 上書き
		//		this.keys[target]=key;
		//		this.vals[target]=value;
		//		return;
		//	}

			// 入れる場所を確保
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

			// 挿入
			this.vals[target]=val;
		}
	}
	#endregion

	#region cls:SortedArray`2
	/// <summary>
	/// 要素が常に整列された可変長配列を実装します。
	/// 同じ key に対して一つの値しか対応づけられません。
	/// </summary>
	/// <typeparam name="TKey">整列に使用する Key の型を指定します。</typeparam>
	/// <typeparam name="TVal">整列される値の型を指定します。</typeparam>
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
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArray(Gen::IDictionary<TKey,TVal> original):base(original){
			// 重複する物を削除
			for(int i=1;i<keys.Length;i++) {
				if(this.keys[i-1].Equals(this.keys[i]))this.RemoveAt(--i);
			}
		}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		public SortedArray():this(4){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArray(int capacity):base(capacity){}
		/// <summary>
		/// 指定した値の SortedArray 内に於ける位置を取得します。
		/// </summary>
		/// <param name="key">検索する key を指定します。</param>
		/// <returns>指定した値の SortedArray 内に於ける位置を返します。
		/// 指定した key 以下の key が登録されていない時には -1 を返します。</returns>
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
		//		IDictionary<TKey,TVal> メンバ
		//=================================================
		/// <summary>
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="key">追加する値に関連付けられた鍵を指定します。</param>
		/// <param name="value">追加する値を指定します。</param>
		public override void Add(TKey key,TVal value) {
			int target=this.BinarySearchSup(key);
			if(0<=target&&target<this.count&&this.keys[target].Equals(key)) {
				// 上書き
				this.keys[target]=key;
				this.vals[target]=value;
				return;
			}

			// 入れる場所を確保
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

			// 挿入
			this.keys[target]=key;
			this.vals[target]=value;
		}
		/// <summary>
		/// 指定した key と関連付けられた値を削除します。
		/// </summary>
		/// <param name="key">削除する鍵を指定します。</param>
		/// <returns>指定した key に関連付けられた値が存在して削除された場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
		public bool Remove(TKey key){
			int index=this.BinarySearch(key);
			if(index<0)return false;
			this.RemoveAt(index);
			return true;
		}
		/// <summary>
		/// 指定した鍵に関連付けられた値が存在するかどうか確認し、存在した場合にはその値を取得します。
		/// </summary>
		/// <param name="key">関連付けられた値が存在するかどうかを確認する鍵を指定します。</param>
		/// <param name="value">関連付けられた値が存在した場合にその値を返します。
		/// それ以外の場合には型の既定値を返します。</param>
		/// <returns>関連付けれた値が存在した時に true を返します。それ以外の場合には false を返します。</returns>
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
		/// 指定した鍵に対応する値を返します。
		/// </summary>
		/// <param name="key">取得したい値に関連付けられた鍵を指定します。</param>
		/// <returns>指定した鍵に関連付けられている値を返します。</returns>
		public TVal this[TKey key]{
			get{
				int index=this.BinarySearch(key);
				if(index<0)throw new System.ArgumentOutOfRangeException("key",key,"指定したオブジェクトは SortedArray の key として含まれていません。");
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
	/// 要素が常に整列された可変長配列を実装します。
	/// 同じ key に対して複数の値を対応付ける事が可能です。
	/// 同じ key を持つ値の格納される順番は不定です。
	/// </summary>
	/// <typeparam name="TKey">整列に使用する Key の型を指定します。</typeparam>
	/// <typeparam name="TVal">整列される値の型を指定します。</typeparam>
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
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArrayP(Gen::IDictionary<TKey,TVal> original):base(original){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		public SortedArrayP():this(4){}
		/// <summary>
		/// SortedArray のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArrayP(int capacity):base(capacity){}
		//===========================================================
		//		Binary Searches
		//===========================================================
		/// <summary>
		/// 指定した key の番号を取得します。一致する key が複数ある場合にはその中でも最初の物を返します。
		/// </summary>
		/// <param name="key">指定した key が在る SortedArray 中の位置を返します。</param>
		/// <returns>key が見つかった場合にはその位置を返します。それ以外の場合には -1 を返します。</returns>
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
		/// <summary>指定した鍵を持つ組達の末端を検索します。末端は、最後の適合する組の次を指します。</summary>
		/// <remarks>
		/// <pre>
		/// □□□□■■■■□□□
		/// 　　　　　　　　↑
		/// </pre>
		/// </remarks>
		/// <param name="key">番号を検索する key を指定します。</param>
		/// <returns>
		/// 登録されている鍵と値の組の中で、指定した鍵を持つ物の領域の最後の次の番号を返します。
		/// 指定した鍵を持つ組が見つからなかった場合には、指定した key が追加される先の番号を返します。
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
		/// <summary>指定した鍵を持つ組の一番若い番号を検索します。</summary>
		/// <param name="key">番号を検索する key を指定します。</param>
		/// <remarks>
		/// <pre>
		/// □□□□■■■■□□□
		/// 　　　　↑
		/// </pre>
		/// </remarks>
		/// <returns>
		/// 登録されている鍵と値の組の中で、指定した鍵を持つ物の領域の最初の番号を返します。
		/// 指定した鍵を持つ組が見つからなかった場合には、指定した key が追加される先の番号を返します。
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
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="key">追加する値に関連付けられた鍵を指定します。</param>
		/// <param name="value">追加する値を指定します。</param>
		public override void Add(TKey key,TVal value) {
		// SortedArray`2.Add の内容の上書き部分を削除
		//-------------------------------------------------
			int target=this.BinarySearchSup(key);
		//	if(target>=0&&this.keys[target].Equals(key)) {
		//		// 上書き
		//		this.keys[target]=key;
		//		this.vals[target]=value;
		//		return;
		//	}

			// 入れる場所を確保
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

			// 挿入
			this.keys[target]=key;
			this.vals[target]=value;
		}

		//===========================================================
		//		IDictionaryP<TKey,TVal> メンバ
		//===========================================================
		/// <summary>
		/// 指定した鍵と値の組を削除します。
		/// 同じ組が複数含まれている場合には一つだけしか削除されません。
		/// </summary>
		/// <param name="key">削除する組の鍵を指定します。</param>
		/// <param name="val">削除する組の値を指定します。</param>
		/// <returns>指定した鍵と値の組が見つかり削除された場合に true を返します。</returns>
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
		/// 指定した鍵と値の組が含まれているかどうかを返します。
		/// </summary>
		/// <param name="key">含まれているかどうかを確認する組の鍵を指定します。</param>
		/// <param name="val">含まれているかどうかを確認する組の値を指定します。</param>
		/// <returns>指定した組が含まれていた場合には true を返します。
		/// それ以外の場合には false を返します。</returns>
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
		/// 指定した鍵を持つ組の中で、指定した番号の物を削除します。
		/// </summary>
		/// <param name="key">削除する組の鍵を指定します。</param>
		/// <param name="index">削除する組を示す 0 から始まる番号を指定します。</param>
		/// <returns>指定した鍵を持つ組が、指定した番号よりも多く存在して組を削除する事が出来た場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
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
		/// 指定した鍵を持つ組を全て削除します。
		/// </summary>
		/// <param name="key">削除する組の鍵を指定します。</param>
		/// <returns>指定した鍵を持つ組が見つかり削除した場合には true を返します。
		/// それ以外の場合には false を返します。</returns>
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
		/// 指定した鍵を持つ組の値を全て配列にして返します。
		/// </summary>
		/// <param name="key">取得する値に関連付けられている鍵を指定しています。</param>
		/// <returns>指定した鍵に関連付けられている値を全て配列に格納して返します。</returns>
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
	/// 要素が常に整列された可変長配列を実装します。
	/// </summary>
	/// <typeparam name="T">整列される値の型を指定します。</typeparam>
	[System.Serializable]
	public abstract class SortedArrayBase<T>:Design.CollectionBase,Gen::IList<T>
		where T:System.IComparable<T>,System.IEquatable<T>{
		/// <summary>
		/// 鍵を保持する配列です。
		/// </summary>
		protected T[] vals;
		/// <summary>
		/// 現在配列に含まれている要素の数を保持します。
		/// </summary>
		protected int count;
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArrayBase(Gen::IList<T> original):this(original.Count+4) {
			if(original==null) throw new System.ArgumentNullException("original");
			original.CopyTo(this.vals,0);
			System.Array.Sort<T>(this.vals);
			this.count=original.Count;
		}
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		public SortedArrayBase():this(4){ }
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArrayBase(int capacity) {
			int len=capacity<4?4:capacity;
			this.vals=new T[len];
			this.count=0;
		}
		/// <summary>
		/// 指定した値の番号を取得します。
		/// </summary>
		/// <param name="val">検索する値を指定します。</param>
		/// <returns>val が見つかった場合にはその位置を返します。それ以外の場合には -1 を返します。</returns>
		public virtual int BinarySearch(T val) {
			int d=0;			// 検索対象の初め
			int u=this.count;	// 検索対象の末端: [u] 自体は含まない
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
		/// 指定した番号に対応する値を取得します。
		/// </summary>
		/// <param name="index">取得する値の番号を指定します。</param>
		/// <returns>指定した番号に対応する値を返します。</returns>
		public T this[int index] {
			get{
				if(index<0||this.count<=index)throw new System.ArgumentOutOfRangeException("index");
				return this.vals[index];
			}
		}
		//=================================================
		//		IEnumerable<KeyValuePair<TKey,TVal>> メンバ
		//=================================================
		/// <summary>
		/// 鍵と値の組を列挙する列挙子を取得します。
		/// </summary>
		/// <returns>鍵と値の組を列挙する列挙子を返します。</returns>
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
		//		ICollection<T> メンバ
		//=================================================
		/// <summary>
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="item">追加する要素を指定します。</param>
		public abstract void Add(T item);
		/// <summary>
		/// 登録されている要素を全て削除します。
		/// </summary>
		public void Clear() {
			System.Array.Clear(this.vals,0,this.count);
			this.count=0;
		}
		/// <summary>
		/// 指定した値が含まれているかどうかを確認します。
		/// </summary>
		/// <param name="item">含まれているかどうか確認する値を指定します。</param>
		/// <returns>指定した値が含まれていた場合に true を返します。それ以外の場合には false を返します。</returns>
		public bool Contains(T item){
			return 0<=this.BinarySearch(item);
		}
		/// <summary>
		/// 指定した配列にこの SortedArray の内容をコピーします。
		/// </summary>
		/// <param name="array">コピー先の配列を指定します。</param>
		/// <param name="arrayIndex">コピー先の配列の中でのコピー開始位置を指定します。</param>
		public void CopyTo(T[] array,int arrayIndex) {
			// check
			const string OUTofRANGE="arrayIndex が array 配列の添え字の範囲に入っていないか array がコピー先の配列として短すぎます。";
			if(array==null) throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex+this.count>array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex",OUTofRANGE);

			System.Array.Copy(this.vals,0,array,arrayIndex,this.count);
		}
		/// <summary>
		/// この SortedArray に含まれている要素の数を取得します。
		/// </summary>
		public int Count {
			get { return this.count; }
		}
		/// <summary>
		/// この SortedArray が読み取り専用かどうかを取得します。
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}
		/// <summary>
		/// 指定した値をこの SortedArray から削除します。
		/// </summary>
		/// <param name="item">削除する値を指定します。</param>
		/// <returns>指定した値が見つかって削除する事が出来た場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
		public bool Remove(T item){
			int index=this.BinarySearch(item);
			if(index>=0){
				this.RemoveAt(index);
				return true;
			}
			return false;
		}
		//=================================================
		//		IList<KeyValuePair<TKey,TVal>> メンバ
		//=================================================
		/// <summary>
		/// 指定した値の位置を取得します。
		/// </summary>
		/// <param name="item">位置を知りたい値を指定します。</param>
		/// <returns>指定した値が見つかった場合にその位置を返します。
		/// 指定した値が見つからなかった場合には -1 を返します。</returns>
		public int IndexOf(T item){
			return this.BinarySearch(item);
		}
		/// <summary>
		/// 値を SortedArray に追加します。
		/// </summary>
		/// <param name="index">本来は挿入する位置を指定する物ですが、このコレクションでは常に整列されるので指定しても意味ありません。</param>
		/// <param name="item">追加する値を指定します。</param>
		public void Insert(int index,T item) {
			this.Add(item);
		}
		/// <summary>
		/// 指定した位置にある値を削除します。
		/// </summary>
		/// <param name="index">値の位置を指定します。</param>
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
	/// 要素が常に整列された可変長配列を実装します。
	/// 同じ key に対して一つの値しか対応づけられません。
	/// </summary>
	/// <typeparam name="TKey">整列に使用する Key の型を指定します。</typeparam>
	/// <typeparam name="TVal">整列される値の型を指定します。</typeparam>
	[System.Serializable]
	public abstract class SortedArrayBase<TKey,TVal>:Design.CollectionBase,Gen::IList<Gen::KeyValuePair<TKey,TVal>>
		where TKey:System.IComparable<TKey>,System.IEquatable<TKey> {
		/// <summary>
		/// 鍵を保持する配列です。
		/// </summary>
		protected TKey[] keys;
		/// <summary>
		/// 値を保持する配列です。
		/// </summary>
		protected TVal[] vals;
		/// <summary>
		/// 現在配列に含まれている要素の数を保持します。
		/// </summary>
		protected int count;
		//=============================================
		//		.ctor
		//=============================================
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		/// <param name="original">作成の元となるコレクションを指定します。
		/// このコレクションの内容を以て初期化されます。</param>
		public SortedArrayBase(Gen::IDictionary<TKey,TVal> original):this(original.Count+4){
			if(original==null)throw new System.ArgumentNullException("original");
			original.Keys.CopyTo(this.keys,0);
			original.Values.CopyTo(this.vals,0);
			System.Array.Sort<TKey,TVal>(this.keys,this.vals);
			this.count=original.Count;
		}
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		public SortedArrayBase():this(4){}
		/// <summary>
		/// SortedArrayBase のインスタンスを作成します。
		/// </summary>
		/// <param name="capacity">最初に確保する容量を指定します。</param>
		public SortedArrayBase(int capacity) {
			int len=capacity<4?4:capacity;
			this.keys=new TKey[len];
			this.vals=new TVal[len];
			this.count=0;
		}
		/// <summary>
		/// 指定した key の番号を取得します。
		/// </summary>
		/// <param name="key">検索する組の鍵を指定します。</param>
		/// <returns>key が見つかった場合にはその位置を返します。それ以外の場合には -1 を返します。</returns>
		public virtual int BinarySearch(TKey key) {
			int d=0;			// 検索対象の初め
			int u=this.count;	// 検索対象の末端: [u] 自体は含まない
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
		/// 指定した番号に対応する鍵と値の組を扱う SortedArrayEntry を取得します。
		/// </summary>
		/// <param name="index">取得する鍵と値の組の番号を指定します。</param>
		/// <returns>指定した番号に対応する SortedArrayEntry を返します。</returns>
		public SortedArrayEntry this[int index] {
			get {
				if(index<0||this.count<=index) throw new System.ArgumentOutOfRangeException("index");
				return new SortedArrayEntry(this,index);
			}
		}
		/// <summary>
		/// 特定の SortedArray の鍵と値の組にアクセスする為の構造体です。
		/// </summary>
		[System.Serializable]
		public struct SortedArrayEntry {
			private int index;
			private SortedArrayBase<TKey,TVal> array;
			private int version;


			internal SortedArrayEntry(SortedArrayBase<TKey,TVal> array,int index) {
				if(index<0||array.count<=index) throw new System.ArgumentOutOfRangeException("index",index,"指定した番号の SoryedArray の要素は存在しません。");
				this.version=array.version;
				this.array=array;
				this.index=index;
			}
			/// <summary>
			/// Key を取得亦は設定します。
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
			/// 値を取得亦は設定します。
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
		//		IEnumerable<KeyValuePair<TKey,TVal>> メンバ
		//=================================================
		/// <summary>
		/// 鍵と値の組を列挙する列挙子を取得します。
		/// </summary>
		/// <returns>鍵と値の組を列挙する列挙子を返します。</returns>
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
		//		ICollection<KeyValuePair<TKey,TVal>> メンバ
		//=================================================
		/// <summary>
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="item">追加する要素を指定します。</param>
		public void Add(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			this.Add(item.Key,item.Value);
		}
		/// <summary>
		/// 登録されている要素を全て削除します。
		/// </summary>
		public void Clear() {
			System.Array.Clear(this.keys,0,this.count);
			System.Array.Clear(this.vals,0,this.count);
			this.count=0;
		}
		/// <summary>
		/// 指定した値の組が含まれているかどうかを確認します。
		/// </summary>
		/// <param name="item">含まれているかどうか確認する値の組を指定します。</param>
		/// <returns>指定した値の組が含まれていた場合に true を返します。それ以外の場合には false を返します。</returns>
		public bool Contains(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			int index;
			return 0<=(index=this.BinarySearch(item.Key))&&this.vals[index].Equals(item.Value);
		}
		/// <summary>
		/// 指定した配列にこの SortedArray の内容をコピーします。
		/// </summary>
		/// <param name="array">コピー先の配列を指定します。</param>
		/// <param name="arrayIndex">コピー先の配列の中でのコピー開始位置を指定します。</param>
		public void CopyTo(System.Collections.Generic.KeyValuePair<TKey,TVal>[] array,int arrayIndex) {
			// check
			const string OUTofRANGE="arrayIndex が array 配列の添え字の範囲に入っていないか array がコピー先の配列として短すぎます。";
			if(array==null) throw new System.ArgumentNullException("array");
			if(arrayIndex<0||arrayIndex+this.count>array.Length)
				throw new System.ArgumentOutOfRangeException("arrayIndex",OUTofRANGE);

			// copy
			for(int i=0;i<this.count;i++)
				array[arrayIndex++]=new Gen::KeyValuePair<TKey,TVal>(this.keys[i],this.vals[i]);
		}
		/// <summary>
		/// この SortedArray に含まれている要素の数を取得します。
		/// </summary>
		public int Count{
			get{return this.count;}
		}
		/// <summary>
		/// この SortedArray が読み取り専用かどうかを取得します。
		/// </summary>
		public bool IsReadOnly {
			get {return false;}
		}
		/// <summary>
		/// 指定した鍵と値の組をこの SortedArray から削除します。
		/// </summary>
		/// <param name="item">削除する鍵と値の組を指定します。</param>
		/// <returns>指定した鍵と値の組が見つかって削除する事が出来た場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
		public virtual bool Remove(System.Collections.Generic.KeyValuePair<TKey,TVal> item){
			int index=this.BinarySearch(item.Key);
			if(index>=0&&this.vals[index].Equals(item.Value)){
				this.RemoveAt(index);
				return true;
			}
			return false;
		}
		//=================================================
		//		IDictionary<TKey,TVal> メンバ
		//=================================================
		/// <summary>
		/// 指定した要素を追加します。
		/// </summary>
		/// <param name="key">追加する値に関連付けられた鍵を指定します。</param>
		/// <param name="value">追加する値を指定します。</param>
		public abstract void Add(TKey key,TVal value);
		/// <summary>
		/// 指定した鍵がこの SortedArray に含まれているかどうかを取得します。
		/// </summary>
		/// <param name="key">含まれているかどうかを知りたい鍵を指定します。</param>
		/// <returns>指定した鍵がこの配列に含まれていた時に true をそれ以外の場合に false を返します。</returns>
		public bool ContainsKey(TKey key) {
			return 0<=this.BinarySearch(key);
		}
		/// <summary>
		/// 読み取り専用の TKey コレクションを取得します。
		/// </summary>
		public Gen::ICollection<TKey> Keys {
			get{return new Design.ReadOnlyCollectionWrapper<TKey>(this.keys,this.count,this);}
		}
		/// <summary>
		/// TVal コレクションを取得します。
		/// </summary>
		public Gen::ICollection<TVal> Values {
			get{return new Design.ReadOnlyCollectionWrapper<TVal>(this.vals,this.count,this);}
		}
		//=================================================
		//		IList<KeyValuePair<TKey,TVal>> メンバ
		//=================================================
		/// <summary>
		/// 指定した鍵と値の組の位置を取得します。
		/// </summary>
		/// <param name="item">位置を知りたい鍵と値の組を指定します。</param>
		/// <returns>指定した鍵と値の組が見つかった場合にその位置を返します。
		/// 指定した鍵と値の組が見つからなかった場合には -1 を返します。</returns>
		public int IndexOf(System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			int index=this.BinarySearch(item.Key);
			return index>=0&&this.vals[index].Equals(item.Value)?index:-1;
		}
		/// <summary>
		/// 鍵と値の組を SortedArray に追加します。
		/// </summary>
		/// <param name="index">本来は挿入する位置を指定する物ですが、このコレクションでは常に整列されるので指定しても意味ありません。</param>
		/// <param name="item">追加する鍵と値の組を指定します。</param>
		public void Insert(int index,System.Collections.Generic.KeyValuePair<TKey,TVal> item) {
			this.Add(item.Key,item.Value);
		}
		/// <summary>
		/// 指定した位置にある鍵と値の組を削除します。
		/// </summary>
		/// <param name="index">鍵と値の組の位置を指定します。</param>
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