using Gen=System.Collections.Generic;

namespace afh.Collections{
	/// <summary>
	/// [***�����r��***]
	/// �z����q�����\���̃R���N�V�����ł��B
	/// ��̓I�ɂ͓K���ȑ傫���̔z��𗼕������X�g�\���Ōq�������ł��B
	/// </summary>
	/// <typeparam name="T">�v�f�̌^���w�肵�܂��B</typeparam>
	public class ArrayList<T>{
		Gen::SortedList<int,Element> elems=new Gen::SortedList<int,Element>();
		/// <summary>
		/// ArrayList �̃C���X�^���X���쐬���܂��B
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
			//		����
			//=============================================
			/// <summary>
			/// �w�肵���ꏊ�ɂ���v�f���폜���܂��B
			/// </summary>
			/// <param name="index">���̒l���w�肷�鎖�B</param>
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
					// ���� this.array.length ���傫������ꍇ�ɂ͓�ɐ܂�
					// �����߂���傫���ׂ肷���Ȃ��l�ɂ���΂悢�B
					// 0. ���z�̑傫�������߂�
					// 1. ���f�Ђ��ϓ��ɂȂ�l��
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
			/// this.count&lt;this.length ���ۏ؂���Ă��鎞�݂̂ɌĂяo���܂��B
			/// </summary>
			private void InsertFromNext(T item){
				this.array[this.count++]=item;
			}
			/// <summary>
			/// this.count&lt;this.length ���ۏ؂���Ă��鎞�݂̂ɌĂяo���܂��B
			/// </summary>
			private void InsertFromPrev(T item){
				System.Array.Copy(array,0,array,1,this.count++);
				this.array[0]=item;
			}
			//=============================================
			//		�ڑ�
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
	/// �R���N�V�����ɕύX�����������ǂ�����m��ׂ� Version �v���p�e�B�������܂��B
	/// </summary>
	public interface IVersion{
		/// <summary>
		/// �R���N�V�����ɕύX�����������ǂ�����m��ׂ̔ԍ����擾���܂��B
		/// </summary>
		int Version{get;}
	}
	/// <summary>
	/// �R���N�V�����ɕύX���Ȃ��ƌ�������z�肵�čs���鑀�삪�A
	/// �R���N�V�����ɕύX����������Ɉׂ���悤�Ƃ����ۂɔ������܂��B
	/// </summary>
	public sealed class CollectionChangedException:System.InvalidOperationException{
		/// <summary>
		/// CollectionChangedException �̐V�����C���X�^���X���쐬���܂��B
		/// </summary>
		public CollectionChangedException(){}
		/// <summary>
		/// CollectionChangedException �̐V�����C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="innerException">���̗�O�̌��ɂȂ�����O������ꍇ�Ɏw�肵�܂��B</param>
		public CollectionChangedException(System.Exception innerException){}
		/// <summary>
		/// ���̗�O�Ɋւ�����𕶎���ŕԂ��܂��B
		/// </summary>
		public override string Message {
			get{return "�񋓎q�܂��̓A�N�Z�T���������ꂽ��ɁA���ɂȂ�R���N�V�������ύX����܂����B";}
		}
	}
}