using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// 一つの key に複数の値を登録する事が出来る辞書の実装を提供します。
	/// </summary>
	/// <typeparam name="TKey">登録する時の見出しとなるインスタンスの型を指定します。</typeparam>
	/// <typeparam name="TVal">登録される値の型を指定します。</typeparam>
	public interface IDictionaryP<TKey,TVal>:Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>{
		/// <summary>
		/// 指定した key と値の組を登録します。
		/// </summary>
		/// <param name="key">値に関連付ける key を指定します。</param>
		/// <param name="val">登録する値を指定します。</param>
		void Add(TKey key,TVal val);
		/// <summary>
		/// 指定した key と値の組を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="val">削除する値を指定します。</param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		bool Remove(TKey key,TVal val);
		/// <summary>
		/// 指定した key と値の組が登録されているかどうかを取得します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="val">登録されているかを値を指定します。</param>
		/// <returns>登録されていた場合には true を返します。それ以外の場合には false を返します。</returns>
		bool Contains(TKey key,TVal val);
		/// <summary>
		/// 指定した key と関連付けられた何らかの値が登録されているかどうかを取得します。
		/// </summary>
		/// <param name="key">それに関連付けられた値が登録されているかどうかを調べる key を指定します。</param>
		/// <returns>指定した key に関連付けられた値が登録されていた場合に true を、それ以外の場合に false を返します。</returns>
		bool ContainsKey(TKey key);
		/// <summary>
		/// 指定した key に関連付けられた値の内、指定した番号の物を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="index">
		/// 削除する値の番号を指定します。
		/// 具体的には指定した key に関連付けられた値に 0 から順番に番号を割り当てた時に
		/// index 番目にあった値が削除される事になります。
		/// </param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		bool RemoveAt(TKey key,int index);
		/// <summary>
		/// 指定した key に関連付けられた全ての値を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		bool RemoveAll(TKey key);
		/// <summary>
		/// 指定した key に対応する値の配列を取得します。
		/// </summary>
		/// <param name="key">取得する値に関連づけられた key を指定します。</param>
		/// <returns>値の配列を返します。</returns>
		TVal[] this[TKey key]{get;}
		/// <summary>
		/// 登録されている Key の集合を取得します。
		/// </summary>
		Gen::ICollection<TKey> Keys{get;}
		/// <summary>
		/// 登録されている値の集合を取得します。
		/// </summary>
		Gen::ICollection<TVal> Values{get;}
	}
	/// <summary>
	/// 一つの key に複数の値を登録する事が出来る Dictionary のクラスです。
	/// </summary>
	/// <typeparam name="TKey">登録する時の見出しとなるインスタンスの型を指定します。</typeparam>
	/// <typeparam name="TVal">登録される値の型を指定します。</typeparam>
	[System.Serializable]
	public class DictionaryP<TKey,TVal>
		:IDictionaryP<TKey,TVal>
		,Gen::ICollection<Gen::KeyValuePair<TKey,TVal>>
		,Gen::IEnumerable<Gen::KeyValuePair<TKey,TVal>>
		,System.Collections.IEnumerable
	{
		private Gen::Dictionary<TKey,Gen::List<TVal>> container=new Gen::Dictionary<TKey,Gen::List<TVal>>();

		/// <summary>
		/// 空の DictionaryP インスタンスを初期化します。
		/// </summary>
		public DictionaryP(){}
		
		/// <summary>
		/// 登録されている Key の集合を取得します。
		/// </summary>
		public Gen::ICollection<TKey> Keys{
			get{return this.container.Keys;}
		}
		/// <summary>
		/// 登録されている値の集合を取得します。
		/// </summary>
		public Gen::ICollection<TVal> Values{
			get{return values??(values=new ValuesCollection(this));}
		}Gen::ICollection<TVal> values;
		/// <summary>
		/// 指定した key に対応する値の配列を取得します。
		/// </summary>
		/// <param name="key">取得する値に関連づけられた key を指定します。</param>
		/// <returns>値の配列を返します。</returns>
		public TVal[] this[TKey key]{
			get{return this.container.ContainsKey(key)?this.container[key].ToArray():new TVal[0];}
		}
		/// <summary>
		/// System.Collections.Gen.KeyValuePair&lt;TKey,TVal&gt; の列挙子を取得します。
		/// </summary>
		/// <returns>列挙子を返します。</returns>
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
		//		基本操作
		//=================================================
		/// <summary>
		/// 指定した key と値の組を登録します。
		/// </summary>
		/// <param name="key">値に関連付ける key を指定します。</param>
		/// <param name="val">登録する値を指定します。</param>
		public void Add(TKey key,TVal val){
			if(!this.container.ContainsKey(key))this.container.Add(key,new Gen::List<TVal>());
			this.container[key].Add(val);
		}
		/// <summary>
		/// 指定した key と値の組を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="val">削除する値を指定します。</param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		public bool Remove(TKey key,TVal val){
			return this.container.ContainsKey(key)&&this.container[key].Remove(val);
		}
		/// <summary>
		/// 指定した key と値の組が登録されているかどうかを取得します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="val">登録されているかを値を指定します。</param>
		/// <returns>登録されていた場合には true を返します。それ以外の場合には false を返します。</returns>
		public bool Contains(TKey key,TVal val){
			return this.container.ContainsKey(key)&&this.container[key].Contains(val);
		}
		//=================================================
		//		辞書操作
		//=================================================
		/// <summary>
		/// 指定した key と関連付けられた何らかの値が登録されているかどうかを取得します。
		/// </summary>
		/// <param name="key">それに関連付けられた値が登録されているかどうかを調べる key を指定します。</param>
		/// <returns>指定した key に関連付けられた値が登録されていた場合に true を、それ以外の場合に false を返します。</returns>
		public bool ContainsKey(TKey key){
			return this.container.ContainsKey(key)&&this.container[key].Count>0;
		}
		/// <summary>
		/// 指定した key に関連付けられた値の内、指定した番号の物を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <param name="index">
		/// 削除する値の番号を指定します。
		/// 具体的には指定した key に関連付けられた値に 0 から順番に番号を割り当てた時に
		/// index 番目にあった値が削除される事になります。
		/// </param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		public bool RemoveAt(TKey key,int index){
			if(index<0||!this.container.ContainsKey(key)||index>=this.container[key].Count)return false;
			this.container[key].RemoveAt(index);
			return true;
		}
		/// <summary>
		/// 指定した key に関連付けられた全ての値を削除します。
		/// </summary>
		/// <param name="key">値に関連付けられた key を指定します。</param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
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
		/// 指定した key と値の組を登録します。
		/// </summary>
		/// <param name="pair">登録する key と値の組を指定します。</param>
		public void Add(Gen::KeyValuePair<TKey,TVal> pair){
			this.Add(pair.Key,pair.Value);
		}
		/// <summary>
		/// 登録されている key と値の組を全て消去し、インスタンスを初期化します。
		/// </summary>
		public void Clear(){this.container.Clear();}
		/// <summary>
		/// 指定した key と値の組を削除します。
		/// </summary>
		/// <param name="pair">削除する key と値の組を指定します。</param>
		/// <returns>
		/// 削除された場合には true を返します。
		/// 元から含まれていなかったなどの理由により削除されなかった場合には　false を返します。
		/// </returns>
		public bool Remove(Gen::KeyValuePair<TKey,TVal> pair){
			return this.Remove(pair.Key,pair.Value);
		}
		/// <summary>
		/// 指定した key と値の組が登録されているかどうかを確認します。
		/// </summary>
		/// <param name="pair">登録されているかどうかを確認する key と値の組を指定します。</param>
		/// <returns>登録されていた場合には true を返します。それ以外の場合には false を返します。</returns>
		public bool Contains(Gen::KeyValuePair<TKey,TVal> pair){
			return this.Contains(pair.Key,pair.Value);
		}
		/// <summary>
		/// 登録されている値の数を取得します。
		/// </summary>
		public int Count{
			get{
				int c=0;
				foreach(Gen::List<TVal> list in this.container.Values)c+=list.Count;
				return c;
			}
		}
		/// <summary>
		/// <see cref="DictionaryP&lt;TKey,TVal&gt;"/> の要素を <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] にコピーします。
		/// <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] の特定のインデックスからコピーが開始されます。
		/// </summary>
		/// <param name="array">
		/// <see cref="DictionaryP&lt;TKey,TVal&gt;"/> から要素がコピーされる 1 次元の <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[]。
		/// <see cref="Gen::KeyValuePair&lt;TKey,TValue&gt;"/>[] には、0 から始まるインデックス番号が必要です。
		/// </param>
		/// <param name="arrayIndex">コピーの開始位置となる、array の 0 から始まるインデックス番号。</param>
		public void CopyTo(Gen::KeyValuePair<TKey,TVal>[] array,int arrayIndex){
			//-- 例外
			if(array==null)throw new System.ArgumentNullException("array");
			if(array.Rank>1)throw new System.ArgumentException("array","array が多次元です。");
			if(arrayIndex<0)throw new System.ArgumentOutOfRangeException("arrayIndex","arrayIndex が 0 未満です。");
			if(array.Length<arrayIndex+this.Count){
				if(array.Length<=arrayIndex)
					throw new System.ArgumentException("arrayIndex","arrayIndex が array の長さ以上です。");
				throw new System.ArgumentException("array",
					string.Format(
						"コピー元の afh.DictionaryP<{0},{1}> の要素数が、arrayIndex からコピー先の array の末尾までに格納できる数を超えています。",
						typeof(TKey).FullName,
						typeof(TVal).FullName
					)
				);
			}
			//-- コピー
			foreach(Gen::KeyValuePair<TKey,TVal> pair in this)array[arrayIndex++]=pair;
		}
		/// <summary>
		/// このインスタンスが読込専用であるかどうかを取得します。
		/// DictionaryP は何時でも要素を変更する事が出来る為常に false を返します。
		/// </summary>
		public bool IsReadOnly{get{return false;}}
		//=================================================
		//		ICollection
		//=================================================
		/// <summary>
		/// 指定した関数を用いて値を変換した結果の DictionaryP を返します。
		/// </summary>
		/// <typeparam name="TVal2">値の変換後の型を指定します。</typeparam>
		/// <param name="converter">値を変換するメソッドを指定します。</param>
		/// <returns>値を変換した結果の DictionaryP を返します。</returns>
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

			#region ICollection<TVal> メンバ
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

			#region IEnumerable<TVal> メンバ
			public System.Collections.Generic.IEnumerator<TVal> GetEnumerator(){
				foreach(Gen::List<TVal> vals in parent.container.Values)
					foreach(TVal val in vals)
						yield return val;
			}
			#endregion

			#region IEnumerable メンバ
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
				return this.GetEnumerator();
			}
			#endregion
		}
	}
}