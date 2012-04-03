using Gen=System.Collections.Generic;

namespace afh.Collections.Design{
	/// <summary>
	/// コレクションの基本クラスです。
	/// </summary>
	public class CollectionBase:IVersion{
		/// <summary>
		/// コレクションに変更があったかどうかを確認する為の番号です。
		/// 変更がある度に version も変わります。
		/// </summary>
		protected int version=int.MinValue;
		/// <summary>
		/// コレクションに変更があった事を記録します。
		/// コレクションの version が変わります。
		/// </summary>
		protected void Changed(){
			if(this.version==int.MaxValue)this.version=int.MinValue;
			else this.version++;
		}

		#region IVersion メンバ

		int IVersion.Version {
			get {return this.version;}
		}

		#endregion
	}

	/// <summary>
	/// コレクションをラップし、操作に対して変更を加える為のクラスです。
	/// </summary>
	/// <typeparam name="T">コレクションの要素の型を指定します。</typeparam>
	public class CollectionWrapper<T>:Gen::ICollection<T>{
		/// <summary>
		/// 本体のコレクションを保持します。
		/// </summary>
		protected Gen::ICollection<T> body;

		//=================================================
		//	依存先オブジェクト
		//=================================================
		private IVersion parent=null;
		private int initial_ver;
		/// <summary>
		/// 依存先のオブジェクトを設定亦は取得します。
		/// 依存先のオブジェクトの version が変わるとこのコレクションは無効になります。
		/// 無効になった場合でも再設定すれば有効になります。
		/// </summary>
		protected IVersion Parent{
			get{return this.parent;}
			set{
				if(value!=null)this.initial_ver=value.Version;
				this.parent=value;
			}
		}
		/// <summary>
		/// 依存先オブジェクトに変更がないか確認します。変更があった場合には例外を発生させます。
		/// </summary>
		protected void check(){
			if(parent!=null&&parent.Version!=this.initial_ver)throw new CollectionChangedException();
		}

		/// <summary>
		/// CollectionWrapper のインスタンスを作成します。
		/// </summary>
		/// <param name="collection">ラップされるコレクションを指定します。</param>
		public CollectionWrapper(Gen::ICollection<T> collection){
			this.body=collection;
		}
		/// <summary>
		/// CollectionWrapper のインスタンスを作成します。
		/// </summary>
		/// <param name="collection">ラップされるコレクションを指定します。</param>
		/// <param name="parent">依存先のオブジェクトを指定します。
		/// 依存先のオブジェクトの version が変わるとこのコレクションは無効になります。</param>
		public CollectionWrapper(Gen::ICollection<T> collection,IVersion parent){
			this.body=collection;
			this.Parent=parent;
		}

		/// <summary>
		/// 読み取り専用の際に使用できるメッセージです。「読込専用コレクションなので変更できません。」
		/// </summary>
		protected const string ERR_READONLY="読込専用コレクションなので変更できません。";
		/// <summary>
		/// 読み取り専用コレクションに要素を追加しようとした時に使用できるメッセージです。
		/// </summary>
		public const string ERR_READONLY_ADD="このコレクションは読み取り専用の為、要素を追加する事は出来ません。";
		/// <summary>
		/// 読み取り専用コレクションの内容を初期化しようとした時に使用できるメッセージです。
		/// </summary>
		public const string ERR_READONLY_REMOVE="このコレクションは読み取り専用の為、要素を削除する事は出来ません。";
		/// <summary>
		/// 読み取り専用コレクションの要素を削除しようとした時に使用できるメッセージです。
		/// </summary>
		public const string ERR_READONLY_CLEAR="このコレクションは読み取り専用の為、要素を初期化する事は出来ません。";

		#region ICollection<T> メンバ
		/// <summary>
		/// コレクションに要素を追加します。
		/// </summary>
		/// <param name="item">追加するオブジェクトを指定します。</param>
		public virtual void Add(T item){
			this.check();
			this.body.Add(item);
		}
		/// <summary>
		/// コレクションの内容を初期化します。
		/// </summary>
		public virtual void Clear(){
			this.check();
			this.body.Clear();
		}
		/// <summary>
		/// コレクションに指定したオブジェクトが含まれているかどうかを取得します。
		/// </summary>
		/// <param name="item">コレクションに含まれているかどうかを確認するオブジェクトを指定します。</param>
		/// <returns>コレクションに指定したオブジェクトが含まれていた場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
		public virtual bool Contains(T item) {
			this.check();
			return this.body.Contains(item);
		}
		/// <summary>
		/// コレクションの内容を配列にコピーします。
		/// </summary>
		/// <param name="array">コピー先の配列を指定します。</param>
		/// <param name="arrayIndex">コピー先の配列 <paramref name="array"/> の中でのコピー開始位置を指定します。</param>
		public virtual void CopyTo(T[] array,int arrayIndex){
			this.check();
			this.body.CopyTo(array,arrayIndex);
		}
		/// <summary>
		/// コレクションに含まれている要素の数を取得します。
		/// </summary>
		public virtual int Count{
			get{
				this.check();
				return this.body.Count;
			}
		}
		/// <summary>
		/// コレクションが読み取り専用であるかどうかを取得します。
		/// </summary>
		public virtual bool IsReadOnly{
			get{
				this.check();
				return this.body.IsReadOnly;
			}
		}
		/// <summary>
		/// 指定したオブジェクトをこのコレクションから削除します。
		/// </summary>
		/// <param name="item">削除するオブジェクトを返します。</param>
		/// <returns>指定したオブジェクトをコレクションから削除した場合に true を返します。
		/// オブジェクトが見つからなかった場合などコレクションから削除する事が出来なかった場合に false を返します。</returns>
		public virtual bool Remove(T item){
			this.check();
			return this.body.Remove(item);
		}
		#endregion

		#region IEnumerable<T> メンバ
		/// <summary>
		/// コレクションの要素の列挙子を取得します。
		/// </summary>
		/// <returns>列挙子を返します。</returns>
		public virtual System.Collections.Generic.IEnumerator<T> GetEnumerator(){
			this.check();
			return this.body.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバ
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.body.GetEnumerator();
		}
		#endregion

	}
	/// <summary>
	/// 読込専用にしたいコレクションをラップするクラスです。
	/// </summary>
	/// <typeparam name="T">コレクションの要素の型を指定します。</typeparam>
	public sealed class ReadOnlyCollectionWrapper<T>:CollectionWrapper<T>{
		/// <summary>
		/// ReadOnlyCollectionWrapper のインスタンスを作成します。
		/// </summary>
		/// <param name="collection">ラップされるコレクションを指定します。</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection):this(collection,-1){}
		/// <summary>
		/// ReadOnlyCollectionWrapper のインスタンスを作成します。
		/// </summary>
		/// <param name="collection">ラップされるコレクションを指定します。</param>
		/// <param name="count">アクセスできる要素の数に制限を加えます。</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection,int count):base(collection){
			this.count=count;
		}
		/// <summary>
		/// ReadOnlyCollectionWrapper のインスタンスを作成します。
		/// </summary>
		/// <param name="collection">ラップされるコレクションを指定します。</param>
		/// <param name="count">アクセスできる要素の数に制限を加えます。制限を加えない場合には負の数値を指定します。</param>
		/// <param name="parent">依存先のオブジェクトを指定します。
		/// 依存先のオブジェクトの version が変わるとこのコレクションは無効になります。</param>
		public ReadOnlyCollectionWrapper(Gen::ICollection<T> collection,int count,IVersion parent):base(collection,parent){
			this.count=count;
		}

		#region ReadOnly メンバ
#pragma warning disable 0809
		/// <summary>
		/// このコレクションは読み取り専用の為、要素を追加する事は出来ません。
		/// </summary>
		/// <param name="item">本来は追加するオブジェクトを指定します。</param>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_ADD)]
		public override void Add(T item){throw new System.NotSupportedException(ERR_READONLY);}
		/// <summary>
		/// このコレクションは読み取り専用の為、要素を初期化する事は出来ません。
		/// </summary>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_CLEAR)]
		public override void Clear(){throw new System.NotSupportedException(ERR_READONLY);}
		/// <summary>
		/// このコレクションは読み取り専用なので要素を削除する事は出来ません。
		/// </summary>
		/// <param name="item">本来は削除する要素を指定します。</param>
		/// <returns>例外を発生させるので返値はありません。</returns>
		/// <exception cref="System.NotSupportedException">このコレクションは読み取り専用なので常に例外を発生させます。</exception>
		[System.Obsolete(CollectionWrapper<T>.ERR_READONLY_REMOVE)]
		public override bool Remove(T item){throw new System.NotSupportedException(ERR_READONLY);}
#pragma warning restore 0809
		#endregion

		#region Count メンバ
		private int count;
		/// <summary>
		/// コレクションに含まれている要素の数を取得します。
		/// </summary>
		public override int Count {
			get{
				if(this.count<0)return base.Count;
				this.check();
				return this.count;
			}
		}
		/// <summary>
		/// コレクションに指定したオブジェクトが含まれているかどうかを取得します。
		/// </summary>
		/// <param name="item">コレクションに含まれているかどうかを確認するオブジェクトを指定します。</param>
		/// <returns>コレクションに指定したオブジェクトが含まれていた場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
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
		/// コレクションの内容を配列にコピーします。
		/// </summary>
		/// <param name="array">コピー先の配列を指定します。</param>
		/// <param name="arrayIndex">コピー先の配列 <paramref name="array"/> の中でのコピー開始位置を指定します。</param>
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
				throw new System.ArgumentOutOfRangeException("arrayIndex","指定した配列の指定した位置からコピーを開始すると array に収まりません");

			// copy
			int i=0;
			foreach(T obj in this.body){
				if(i++>=count)break;
				array[arrayIndex++]=obj;
			}
		}
		/// <summary>
		/// コレクションの要素の列挙子を取得します。
		/// </summary>
		/// <returns>列挙子を返します。</returns>
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