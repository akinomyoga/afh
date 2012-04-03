using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// [***実装途中***]
	/// 配列を繋げた構造のコレクションです。
	/// 具体的には適当な大きさの配列を両方向リスト構造で繋げた物です。
	/// </summary>
	/// <typeparam name="T">要素の型を指定します。</typeparam>
	public class ArrayList<T>{
		Gen::SortedList<int,Element> elems=new Gen::SortedList<int,Element>();
		/// <summary>
		/// ArrayList のインスタンスを作成します。
		/// </summary>
		public ArrayList(){
			this.elems[0]=null;
		}

		private Element pool;
		private Element Pool{
			get{return this.pool--??new Element(new T[4],this);}
			set{this.pool+=value;}
		}

		private sealed class Element{
			public T[] array;
			public int count;
			public ArrayList<T> owner;
			public Element prev;
			public Element next;

			public Element(T[] array,ArrayList<T> owner){
				this.array=array;
				this.owner=owner;
			}
			public T this[int index]{
				get{return this.array[index];}
				set{this.array[index]=value;}
			}
			//=============================================
			//		操作
			//=============================================
			/// <summary>
			/// 指定した場所にある要素を削除します。
			/// </summary>
			/// <param name="index">正の値を指定する事。</param>
			public void Remove(int index){
				this.count--;
				int len=this.count-index;
				if(len>0)System.Array.Copy(this.array,index+1,this.array,index,len);
			}
			public bool Insert(int index,T value){
				if(this.count<this.array.Length){
					int len=this.count-index;
					if(len>0)System.Array.Copy(this.array,index,this.array,index+1,len);
					array[index]=value;
					this.count++;
					// この this.array.length が大きすぎる場合には二つに折る
					// →初めから大きく為りすぎない様にすればよい。
					// 0. 理想の大きさを決める
					// 1. 両断片が均等になる様に
				}else if(this.prev!=null&&this.prev.array.Length>this.prev.count){
					this.prev.InsertFromNext(this.array[0]);
					if(index>0)System.Array.Copy(array,1,array,0,index);
					this.array[index]=value;
					int r=this.next.array.Length-this.next.count;
				}else if(this.next!=null&&this.next.array.Length>this.next.count){
					this.next.InsertFromPrev(this.array[this.count-1]);
					int len=this.count-index-1;
					if(len>0)System.Array.Copy(array,index,array,index+1,len);
				}else return false;

				return true;
			}
			/// <summary>
			/// this.count&lt;this.length が保証されている時のみに呼び出します。
			/// </summary>
			private void InsertFromNext(T item){
				this.array[this.count++]=item;
			}
			/// <summary>
			/// this.count&lt;this.length が保証されている時のみに呼び出します。
			/// </summary>
			private void InsertFromPrev(T item){
				System.Array.Copy(array,0,array,1,this.count++);
				this.array[0]=item;
			}
			//=============================================
			//		接続
			//=============================================
			public static void Connect(Element former,Element latter){
				if(former!=null)former.next=latter;
				if(latter!=null)latter.prev=former;
			}
			public static Element operator +(Element former,Element latter){
				Connect(former,latter);
				return latter;
			}
			public static Element operator --(Element elem){
				if(elem==null||elem.prev==null)return null;
				Element r=elem.prev;
				r.next=null;
				elem.prev=null;
				return r;
			}
		}
	}
	/// <summary>
	/// コレクションに変更があったかどうかを知る為の Version プロパティを持ちます。
	/// </summary>
	public interface IVersion{
		/// <summary>
		/// コレクションに変更があったかどうかを知る為の番号を取得します。
		/// </summary>
		int Version{get;}
	}
	/// <summary>
	/// コレクションに変更がないと言う事を想定して行われる操作が、
	/// コレクションに変更があった後に為されようとした際に発生します。
	/// </summary>
	public sealed class CollectionChangedException:System.InvalidOperationException{
		/// <summary>
		/// CollectionChangedException の新しいインスタンスを作成します。
		/// </summary>
		public CollectionChangedException(){}
		/// <summary>
		/// CollectionChangedException の新しいインスタンスを作成します。
		/// </summary>
		/// <param name="innerException">この例外の元になった例外がある場合に指定します。</param>
		public CollectionChangedException(System.Exception innerException){}
		/// <summary>
		/// この例外に関する情報を文字列で返します。
		/// </summary>
		public override string Message {
			get{return "列挙子またはアクセサが生成された後に、元になるコレクションが変更されました。";}
		}
	}
}