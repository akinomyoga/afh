#define USE_ARRAY_FOR_CAPTURES
using Gen=System.Collections.Generic;
using System.Text;

namespace afh.RegularExpressions{
	/// <summary>
	/// ���K�\�����Ǘ�����N���X�ł��B
	/// </summary>
	/// <typeparam name="T">���K�\����K�p����Ώۂ̌^���w�肵�܂��B</typeparam>
	public partial class RegexFactory3<T>{
		/// <summary>
		/// ��̃m�[�h�B��ێ����܂��B�q�m�[�h�̖��� INode ���� Children �Ƃ��Ďg�p���܂��B
		/// </summary>
		protected readonly static Gen::IEnumerable<INode> EmptyNodes=new INode[0];

		/// <summary>
		/// �w�肵�� Status ���g�p���� matching �����s���܂��B
		/// </summary>
		/// <param name="s">�ǂݎ��Ɏg�p���� Status ���w�肵�܂��B
		/// ITester-Stack ����ł��鎖��z�肵�Ă��܂��B
		/// ���ɉ�������ꍇ�ɂ͂����ʂ̏��ɑҔ����āAnew Stack ��ݒ肵�Ă����ĉ������B
		/// </param>
		/// <param name="node">�v������p�^�[����\������\���̂��w�肵�܂��B</param>
		/// <returns>�}�b�`���O�ɐ��������ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		internal bool match(Status s,INode node) {
			Gen::Stack<StatusInfo> st_indef=new Gen::Stack<StatusInfo>();

			s.Push(node.GetTester());
			while(true){
				ITester test=s.Tester.Read(s);

				if(!s.Success){
				// a. Node ���s
				//-------------------
					// �S�� ���s
					if(s.IsRoot){
						// * INDEFINICITY *
						if(st_indef.Count==0)return false;
						st_indef.Pop().Restore(s);
					}else s.Pop();
				}else if(test!=null){
				// b. ����q Node
				//-------------------
					s.Push(test);
				}else{
				// c. ���� (Node �I��)
				//-------------------
					// �S�� ����
					if(s.IsRoot)return true;

					if(s.Tester.Indefinite){
						// * INDEFINICITY *
						st_indef.Push(new StatusInfo(s));
					}

					s.Pop();
				}
			}
		}

		internal struct MatchData{
			public readonly INode root;
			public readonly ITypedStream<T> target;
#if USE_ARRAY_FOR_CAPTURES
			public readonly ICaptureRange[] captures;
#else
			public readonly Gen::List<ICaptureRange> captures;
#endif
			public MatchData(Evaluator eval){
				Status s=eval.Status;
				this.root=eval.Node;
#if USE_ARRAY_FOR_CAPTURES
				this.captures=new ICaptureRange[s.Captures.Count+1];
				s.Captures.CopyTo(this.captures,0);
				this.captures[s.Captures.Count]=CaptureRangeBase.CreateInstanceForRootNode(eval);
#else
				this.captures=new Gen::List<ICaptureRange>(s.Captures);
				this.captures.Add(CaptureRangeBase.CreateInstanceForRootNode(eval));
#endif
				this.target=s.Target;
			}

			public static bool operator==(MatchData l,MatchData r){
				return l.root==r.root&&l.target==r.target&&l.captures==r.captures;
			}
			public static bool operator!=(MatchData l,MatchData r){
				return !(l==r);
			}
			public override bool Equals(object obj) {
				return obj is MatchData&&(MatchData)obj==this;
			}
			public override int GetHashCode() {
				return this.root.GetHashCode()^this.target.GetHashCode()^this.captures.GetHashCode();
			}
		}

		#region class Capture;
		/// <summary>
		/// �L���v�`���̊�b�N���X�ł��B
		/// </summary>
		/// <typeparam name="TCapture">�h���N���X�^���w�肵�܂��B</typeparam>
		public abstract class CaptureBase<TCapture>
			where TCapture:CaptureBase<TCapture>
		{
			int icapt;
			MatchData data;
			private GroupCollection<TCapture> groups;
			internal CaptureBase(MatchData data,int index){
				this.data=data;
				this.icapt=index;
				this.groups=new GroupCollection<TCapture>((TCapture)this);
			}
			//internal CaptureBase(Evaluator eval,int index)
			//	:this(new MatchData(eval),index){}
			internal CaptureBase(Evaluator eval){
				this.data=new MatchData(eval);
#if USE_ARRAY_FOR_CAPTURES
				this.icapt=this.data.captures.Length-1;
#else
				this.icapt=this.data.captures.Count-1;
#endif
				this.groups=new GroupCollection<TCapture>((TCapture)this);
			}
			/// <summary>
			/// CaptureBase �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="baseRange">�쐬���� Capture �S�̂� Range ���w�肵�܂��B</param>
			/// <param name="index">baseRange �� Range �ԍ����w�肵�܂��B</param>
			protected internal CaptureBase(CaptureBase<TCapture> baseRange,int index){
				this.data=baseRange.data;
				this.icapt=index;
				this.groups=new GroupCollection<TCapture>((TCapture)this);
			}
			//========================================================
			//		�q���L���v�`��
			//========================================================
			/// <summary>
			/// ���� Capture �̒��ň�v�����q�� Capture �̏W�����擾���܂��B
			/// </summary>
			public CaptureCollection<TCapture> Captures{
				get{return new CaptureCollection<TCapture>((TCapture)this);}
			}
			/// <summary>
			/// ���̃L���v�`�����ɂ��� Group �̏W�����擾���܂��B
			/// </summary>
			public GroupCollection<TCapture> Groups{
				get{return this.groups;}
			}
			/// <summary>
			/// �q Capture �C���X�^���X���쐬���܂��B
			/// </summary>
			/// <param name="index">���� Capture �̎q���̓��ł̔ԍ����w�肵�܂��B</param>
			/// <returns>�쐬���� <typeparamref name="TCapture"/> �C���X�^���X��Ԃ��܂��B</returns>
			protected internal abstract TCapture CreateCapture(int index);
			/// <summary>
			/// ���̃L���v�`���S�̂�\�� Range ���擾���܂��B
			/// </summary>
			protected internal ICaptureRange Range{
				get{return this.data.captures[icapt];}
			}
			/// <summary>
			/// ���̃L���v�`���y�юq���� Range �ԍ��ŁA��ԎႢ����Ԃ��܂��B
			/// </summary>
			protected internal int StartInCaptures{
				get{return Range.InitialCaptureCount;}
			}
			/// <summary>
			/// ���̃L���v�`���y�юq���� Range �ԍ��ŁA��ԑ傫������Ԃ��܂��B
			/// �ő�̕��́A���̃L���v�`���S�̂ɂ��Ă� Range �ɂȂ�܂��B
			/// </summary>
			protected internal int EndInCaptures{
				get{return this.icapt;}
			}
#if USE_ARRAY_FOR_CAPTURES
			internal ICaptureRange[] CaptureRanges{
#else
			internal Gen::List<ICaptureRange> CaptureRanges{
#endif
				get{return this.data.captures;}
			}
			/// <summary>
			/// ��v�̑Ώۂ̒l����擾���܂��B
			/// </summary>
			protected ITypedStream<T> TargetStream{
				get{return this.data.target;}
			}
			//========================================================
			//		���̃L���v�`���̏��
			//========================================================
			/// <summary>
			/// ��v�͈͂̊J�n�ʒu���擾���܂��B
			/// </summary>
			public int Start{
				get{return Range.Start;}
			}
			/// <summary>
			/// ��v�͈͂̏I���ʒu���擾���܂��B
			/// �Ō�̗v�f�ԍ��̎����w�������܂��B
			/// </summary>
			public int End{
				get{return Range.End;}
			}
			/// <summary>
			/// ��v�͈͂̒������擾���܂��B
			/// �Ō�̗v�f�ԍ��̎����w�������܂��B
			/// </summary>
			public int Length{
				get{return Range.Start-Range.End;}
			}
			/// <summary>
			/// ��v�͈͓��̒l�̗��z��ɕϊ����ĕԂ��܂��B
			/// </summary>
			public T[] ToArray(){
				int len=this.End-this.Start;
				if(len<=0)return new T[0];
				T[] ret=new T[len];
				int i=0;
				foreach(T val in this.EnumerateElements())ret[i++]=val;
				return ret;
			}
			/// <summary>
			/// ��v��̗񋓎q��񋟂���C���X�^���X���쐬���܂��B
			/// </summary>
			/// <returns>��v��̗񋓎q��񋟂���C���X�^���X��Ԃ��܂��B</returns>
			public Gen::IEnumerable<T> EnumerateElements(){
				ITypedStream<T> str=data.target.Clone();
				str.Index=this.Start;
				int iM=this.End;
				do{
					yield return str.Current;
				}while(str.MoveNext()<iM);
			}
			//========================================================
			//		��r
			//========================================================
			/// <summary>
			/// �L���v�`���̑Ώۂ������ł��邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="left">��r����C���X�^���X���w�肵�܂��B</param>
			/// <param name="right">��r����C���X�^���X���w�肵�܂��B</param>
			/// <returns>�Ώۂ������ł���Ɣ��肳�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			public static bool operator==(CaptureBase<TCapture> left,CaptureBase<TCapture> right){
				if((object)left==null||(object)right==null)return (object)left==(object)right;
				return left.icapt==right.icapt&&left.data==right.data;
			}
			/// <summary>
			/// �L���v�`���̑Ώۂ��قȂ邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="left">��r����C���X�^���X���w�肵�܂��B</param>
			/// <param name="right">��r����C���X�^���X���w�肵�܂��B</param>
			/// <returns>�Ώۂ��قȂ�Ɣ��肳�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			public static bool operator!=(CaptureBase<TCapture> left,CaptureBase<TCapture> right){
				return !(left==right);
			}
			/// <summary>
			/// �w�肵���I�u�W�F�N�g�C���X�^���X�Ɠ��l��r���s���܂��B
			/// </summary>
			/// <param name="obj">��r�Ώۂ̃I�u�W�F�N�g�C���X�^���X���w�肵�܂��B</param>
			/// <returns>�w�肵�� object �� CaptureBase&lt;TCapture&gt; �^�ł����āA
			/// �X�ɂ��̃C���X�^���X�Ɠ��e����v�����ꍇ�� true ��Ԃ��܂��B
			/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			public override bool Equals(object obj){
				CaptureBase<TCapture> r=obj as CaptureBase<TCapture>;
				return r!=null&&r==this;
			}
			/// <summary>
			/// ���̃C���X�^���X�Ɋւ���n�b�V���l���v�Z���܂��B
			/// </summary>
			/// <returns>���̃C���X�^���X�̓��e�Ɋ�Â��Čv�Z���ꂽ�n�b�V���l��Ԃ��܂��B</returns>
			public override int GetHashCode(){
				return this.icapt.GetHashCode()^this.data.GetHashCode();
			}
		}
		/// <summary>
		/// Capture �̏W�����L�q����N���X�ł��B
		/// </summary>
		public class CaptureCollection<TCapture>
			:Gen::ICollection<TCapture>,Gen::IEnumerable<TCapture>,System.Collections.IEnumerable
			where TCapture:CaptureBase<TCapture>
		{
			TCapture parent;
			internal CaptureCollection(TCapture parent){
				this.parent=parent;
			}
			/// <summary>
			/// �w�肵���ԍ��ɑΉ����� Capture ���擾���܂��B
			/// </summary>
			/// <param name="index">�擾���� Capture �̔ԍ����w�肵�܂��B</param>
			/// <returns>�w�肵���ԍ��ɑΉ����� Capture ��Ԃ��܂��B</returns>
			public TCapture this[int index] {
				get{
					index+=parent.StartInCaptures;
					if(index<parent.StartInCaptures||parent.EndInCaptures<=index)
						throw new System.ArgumentOutOfRangeException("�Y���͈̔͂��s���ł��B");
					return parent.CreateCapture(index);
				}
			}
			/// <summary>
			/// ����Ɋ܂܂�Ă��� Capture �̐����擾���͐ݒ肵�܂��B
			/// </summary>
			public int Count{
				get{return parent.EndInCaptures-parent.StartInCaptures;}
			}
			/// <summary>
			/// �w�肵�� Capture �C���X�^���X������Ɋ܂܂�Ă��邩�ۂ����擾���͐ݒ肵�܂��B
			/// </summary>
			/// <param name="capture">�܂܂�Ă��邩�ۂ��𔻒肷�� Capture �C���X�^���X���w�肵�܂��B</param>
			/// <returns>�w�肵���C���X�^���X���A���� CaptureCollection �Ɋ܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
			/// �܂܂�Ă��Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
			public bool Contains(TCapture capture) {
				throw new System.Exception("The method or operation is not implemented.");
			}
			/// <summary>
			/// �z��� Capture �̒l���R�s�[���܂��B
			/// </summary>
			/// <param name="array">�R�s�[��̔z����w�肵�܂��B</param>
			/// <param name="arrayIndex">�R�s�[��̔z��ɉ�����A�R�s�[�̊J�n�ʒu���w�肵�܂��B
			/// �w�肵���ʒu�� CaptureCollection �̈�ԏ��߂̗v�f���������܂�܂��B</param>
			public void CopyTo(TCapture[] array,int arrayIndex) {
				int end=arrayIndex+this.Count;
				if(arrayIndex<0||array.Length<end)
					throw new System.ArgumentOutOfRangeException("arrayIndex","�R�s�[��̔z��ɓ���܂���B�[���傫�Ȕz���p�ӂ��邩 arrayIndex ��K�؂ɐݒ肵�ĉ������B");

				int index=parent.StartInCaptures;
				while(arrayIndex<end){
					array[arrayIndex++]=parent.CreateCapture(index++);
				}
			}
			/// <summary>
			/// Capture �̗񋓎q���擾���܂��B
			/// </summary>
			/// <returns>�쐬�����񋓎q��Ԃ��܂��B</returns>
			public System.Collections.Generic.IEnumerator<TCapture> GetEnumerator() {
				for(int index=parent.StartInCaptures;index<this.parent.EndInCaptures;index++)
					yield return parent.CreateCapture(index);
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
				return this.GetEnumerator();
			}
			void Gen::ICollection<TCapture>.Add(TCapture item) {
				throw new System.NotSupportedException("CaptureCollection �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
			void Gen::ICollection<TCapture>.Clear() {
				throw new System.NotSupportedException("CaptureCollection �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
			bool Gen::ICollection<TCapture>.IsReadOnly {
				get{return true;}
			}
			bool Gen::ICollection<TCapture>.Remove(TCapture item) {
				throw new System.NotSupportedException("CaptureCollection �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
		}
		/// <summary>
		/// ����̃m�[�h�Ɉ˂��Ĉ�v���� Capture ��Z�߂Ĉ����ׂ̃N���X�ł��B
		/// </summary>
		/// <typeparam name="TCapture">Capture �̌^���w�肵�܂��B</typeparam>
		public sealed class Group<TCapture>
			:Gen::ICollection<TCapture>,Gen::IEnumerable<TCapture>,System.Collections.IEnumerable
			where TCapture:CaptureBase<TCapture>
		{
			readonly TCapture parent;
			readonly INode node;
			readonly string name;
			private Gen::List<TCapture> cache=null;
			private Gen::List<TCapture> Cache{
				get{
					if(this.cache!=null)return this.cache;
					this.cache=new Gen::List<TCapture>();
					foreach(TCapture c in parent.Captures){
						if(this.IsMember(c.Range.Node))
							this.cache.Add(c);
					}
					return this.cache;
				}
			}
			/// <summary>
			/// Group ���ł̍Ō�� Capture ���擾���܂��B
			/// </summary>
			public TCapture Last{
				get{
					if(this.cache!=null)
						return this.cache[this.cache.Count-1];

					// �Ō�̈�v���������g��Ȃ��ꍇ (cache �쐬�͂��Ȃ�)
					for(int i=parent.EndInCaptures-1,iM=parent.StartInCaptures;i>=iM;i--){
						if(!IsMember(parent.CaptureRanges[i].Node))continue;
						return parent.CreateCapture(i);
					}
					return null; // ����Ȃ��ꍇ
				}
			}
			//========================================================
			//		������
			//========================================================
			internal Group(TCapture parent,INode node){
				this.parent=parent;
				this.node=node;
				this.name=null;
			}
			internal Group(TCapture parent,int index){
				this.parent=parent;
				this.name=null;
				if(index==0){
					// �������g�����Ɉ�v
					this.cache=new Gen::List<TCapture>(1);
					this.cache.Add(this.parent);
				}else{
					this.node=GetIndexedNode(ref index,parent.Range.Node);
				}
			}
			internal Group(TCapture parent,string name){
				this.parent=parent;
				this.node=null;
				this.name=name;
			}
			/// <summary>
			/// �w�肵���ԍ��ɑΉ����� Node ���擾���܂��B
			/// </summary>
			/// <param name="index">
			/// index �� 0 �̏ꍇ�ɂ͂��̃m�[�h���̂��Ԃ���܂��B
			/// index �����̏ꍇ�ɂ́A���̃m�[�h�̎q�m�[�h�̒���
			/// index �Ԗ� (1 ����n�܂�ԍ�) �ɂ��� CaptureNode ��Ԃ��܂��B
			/// </param>
			/// <param name="basis">�����̊�ɂȂ�m�[�h���w�肵�܂��B</param>
			/// <returns>�w�肵���ԍ��ɑΉ�����m�[�h�����������ꍇ�ɂ����Ԃ��܂��B
			/// ������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			private static INode GetIndexedNode(ref int index,INode basis){
				__debug__.RegexTestAssert(basis!=null&&index>=0);
				if(index==0)return basis;

				if(basis is CaptureNode&&index--==1)
					return basis;

				foreach(INode node in basis.EnumChildren){
					INode ret=GetIndexedNode(ref index,basis);
					if(ret!=null)return ret;
				}

				return null;
			}
			private bool IsMember(INode node){
				if(this.name!=null){
					CaptureNode cnode=node as CaptureNode;
					return cnode!=null&&cnode.Name==this.name;
				}else{
					return node==this.node;
				}
			}
			//========================================================
			//		�R���N�V�����Ƃ���
			//========================================================
			/// <summary>
			/// Group �ɑ�����w�肵���ԍ��� Capture ���擾���܂��B
			/// </summary>
			/// <param name="index">Group ���� Capture ���w�������ԍ����w�肵�܂��B</param>
			/// <returns>�w�肵���ԍ��� Capture ��Ԃ��܂��B</returns>
			public TCapture this[int index]{
				get{return this.Cache[index];}
			}
			/// <summary>
			/// �w�肵�� Capture ������ Group �̒��Ɋ܂܂�Ă���ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="item">�܂܂�Ă��邩�ۂ����m�F������ Capture ���w�肵�܂��B</param>
			/// <returns>�w�肵�� Capture ���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
			/// �܂܂�Ă��Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
			public bool Contains(TCapture item){
				return this.Cache.Contains(item);
			}
			/// <summary>
			/// Group �Ɋ܂܂�� Capture �̐����擾���܂��B
			/// </summary>
			public int Count{
				get {return this.Cache.Count;}
			}
			/// <summary>
			/// Group �Ɋ܂܂�� Capture �̏W����z��ɏ����o���܂��B
			/// </summary>
			/// <param name="array">�����o���Ώۂ̔z����w�肵�܂��B</param>
			/// <param name="arrayIndex">�����o�����J�n���� array �ɉ����� index ���w�肵�܂��B
			/// ��ԍŏ��� Capture �� array[arrayIndex] �ɏ������܂�܂��B</param>
			public void CopyTo(TCapture[] array,int arrayIndex) {
				this.Cache.CopyTo(array,arrayIndex);
			}
			/// <summary>
			/// Capture �̗񋓎q���擾���܂��B
			/// </summary>
			/// <returns>�쐬���� Capture �̗񋓎q��Ԃ��܂��B</returns>
			public Gen::IEnumerator<TCapture> GetEnumerator(){
				return this.Cache.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
				return this.GetEnumerator();
			}

			void Gen::ICollection<TCapture>.Add(TCapture item){
				throw new System.NotSupportedException("Group �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
			void Gen::ICollection<TCapture>.Clear() {
				throw new System.NotSupportedException("Group �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
			bool Gen::ICollection<TCapture>.IsReadOnly {
				get {return false;}
			}
			bool Gen::ICollection<TCapture>.Remove(TCapture item) {
				throw new System.NotSupportedException("Group �̓R���N�V�����̕ύX�ɑΉ����Ă��܂���B");
			}
		}
		/// <summary>
		/// Capture �̎q Group �𐶐����ĕԂ��ׂ̃N���X�ł��B�B
		/// </summary>
		/// <typeparam name="TCapture">Capture �̌^���w�肵�܂��B</typeparam>
		public sealed class GroupCollection<TCapture>
			where TCapture:CaptureBase<TCapture>
		{
			TCapture parent;
			internal GroupCollection(TCapture parent){
				this.parent=parent;
			}

			Gen::Dictionary<int,Group<TCapture>> cacheI;
			private Gen::Dictionary<int,Group<TCapture>> CacheI{
				get{return cacheI??(cacheI=new Gen::Dictionary<int,Group<TCapture>>());}
			}
			/// <summary>
			/// �w�肵���ԍ��̈�v�O���[�v���擾���܂��B
			/// </summary>
			/// <param name="index">�擾����O���[�v�̔ԍ����w�肵�܂��B
			/// �ԍ��́A���݂̃m�[�h����ɂ��ĕt�����܂��B
			/// �����A���݂̃m�[�h�̎q�m�[�h��</param>
			/// <returns>�w�肵���ԍ��̈�v�O���[�v��Ԃ��܂��B</returns>
			public Group<TCapture> this[int index]{
				get{
					Gen::Dictionary<int,Group<TCapture>> c=CacheI;
					Group<TCapture> ret;
					if(!c.TryGetValue(index,out ret))
						c[index]=new Group<TCapture>(parent,index);

					return c[index];
				}
			}
			Gen::Dictionary<string,Group<TCapture>> cacheN;
			private Gen::Dictionary<string,Group<TCapture>> CacheN{
				get{return cacheN??(cacheN=new Gen::Dictionary<string,Group<TCapture>>());}
			}
			/// <summary>
			/// �w�肵�����O�̈�v�O���[�v���擾���܂��B
			/// </summary>
			/// <param name="name">�擾�����v�O���[�v�̖��O���w�肵�܂��B</param>
			/// <returns>�w�肵�����O�̈�v�O���[�v��Ԃ��܂��B</returns>
			public Group<TCapture> this[string name]{
				get{
					Gen::Dictionary<string,Group<TCapture>> c=CacheN;
					Group<TCapture> ret;
					if(!c.TryGetValue(name,out ret))
						c[name]=new Group<TCapture>(parent,name);

					return c[name];
				}
			}
		}
		#endregion

		internal class Evaluator{
			Gen::Stack<StatusInfo> st_indef=new Gen::Stack<StatusInfo>();
			readonly INode node;
			readonly Status s;
			int index0;

			public Evaluator(Status s,INode node){
				this.node=node;
				this.s=s;
				this.st_indef=new Gen::Stack<StatusInfo>();

				this.index0=this.s.Index;
			}
			/// <summary>
			/// �ǂݎ��Ɏg�p���Ă��郋�[�g�m�[�h���擾���܂��B
			/// </summary>
			public INode Node{
				get{return this.node;}
			}
			/// <summary>
			/// ���݂̓ǂݎ��J�n�ʒu���擾���܂��B
			/// </summary>
			public int StartIndex{
				get{return this.index0;}
			}
			/// <summary>
			/// �ǂݎ���Ԃ�\�� Status ���擾���܂��B
			/// </summary>
			public Status Status{
				get{return this.s;}
			}
			//=======================================================
			//		�}�b�`����
			//=======================================================
			//=======================================================
			//		�}�b�`�̐ݒ�
			//=======================================================
			private bool fix_start=false;
			private bool overlap_search=false;
			/// <summary>
			/// �ʂ̊J�n�_�Ŏ����Ȃ��ꍇ�� true �̒l�������܂��B
			/// </summary>
			public bool FixStart{
				get{return this.fix_start;}
				set{this.fix_start=value;}
			}
			/// <summary>
			/// ��x��v�����͈͂ł̕ʂ̈�v�����݂邩�ۂ����擾���͐ݒ肵�܂��B
			/// �ʂ̈�v�����݂�ꍇ�� true ���w�肵�܂��B
			/// ����ȊO�̏ꍇ�ɂ� false ���w�肵�܂��B
			/// ����l�� false �ł��B
			/// </summary>
			public bool OverlapSearch{
				get{return this.overlap_search;}
				set{this.overlap_search=value;}
			}
			/*
			bool end_of_match=false;
			/// <summary>
			/// �}�b�`���I���������ۂ����擾���܂��B
			/// ��������ȏ�}�b�`���镨���Ȃ����� true �̒l��Ԃ��܂��B
			/// </summary>
			bool EndOfMatch{
				get{__debug__.RegexTestNotImplemented();return false;}
			}
			//*/
			//=======================================================
			//		�}�b�`
			//=======================================================
			bool first=true;
			/// <summary>
			/// ���Ɉ�v���镔�����������܂��B
			/// </summary>
			/// <returns>�����̌��ʈ�v���镔�������������ꍇ�� true ��Ԃ��܂��B
			/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			public bool Match(){
				do{
					bool fInit=false;
					if(first){
						first=false;
						fInit=true;
					}else{
						__debug__.RegexTestAssert(s.IsRoot);

						if(OverlapSearch&&st_indef.Count!=0){
							// ���̉\��
							st_indef.Pop().Restore(s);
						}else if(!FixStart&&!s.Target.EndOfStream){
							st_indef.Clear();

							// ���̌����J�n�_
							s.Pop();
							s.ClearCaptures(0);
							if(!OverlapSearch){
								if(s.Index==index0)
									// ���v�̎�
									s.Target.MoveNext();
							}else{
								s.Index=index0;
								s.Target.MoveNext();
							}

							fInit=true;
						}else return false;
					}

					if(fInit){
						index0=s.Target.Index;
						s.Push(node.Nondeterministic?node.GetTester():new DeterministicTester(node));
					}
				}while(!match_core());

				return true;
			}
			private bool match_core(){
				Status s=this.s; // ���[�J����
				while(true){
					ITester test=s.Tester.Read(s);

					if(!s.Success){
					// a. Node ���s
					//-------------------
						// �S�� ���s
						if(s.IsRoot){
							// * INDEFINICITY *
							if(st_indef.Count==0)return false;
							st_indef.Pop().Restore(s);
						}else s.Pop();
					}else if(test!=null){
					// b. ����q Node
					//-------------------
						s.Push(test);
					}else{
					// c. ���� (Node �I��)
					//-------------------
						if(s.Tester.Indefinite){
							// * INDEFINICITY *
							st_indef.Push(new StatusInfo(s));
						}

						// �S�� ����
						if(s.IsRoot)return true;

						s.Pop();
					}
				}
			}

			// NextPossibility
		}
	}
}