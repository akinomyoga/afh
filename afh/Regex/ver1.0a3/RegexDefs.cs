//#define USE_RANGELIST
using Gen=System.Collections.Generic;
using System.Text;

namespace afh.RegularExpressions{
	public partial class RegexFactory3<T>{
		//=================================================================
		//		Status
		//=================================================================
		/// <summary>
		/// ���݂̓ǂݎ��̏�Ԃ�ێ�����N���X�ł��B
		/// </summary>
		public class Status{
#if USE_RANGELIST
			RangeList<ICaptureRange> captures=new RangeList<ICaptureRange>();
#else
			Gen::List<ICaptureRange> captures=new Gen::List<ICaptureRange>();
#endif
			bool success;

			/// <summary>
			/// �ǂݎ��̑Ώۂ��w�肵�� Status �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="target">�ǂݎ��̑Ώۂ��i�[���Ă���I�u�W�F�N�g���w�肵�܂��B</param>
			public Status(ITypedStream<T> target){
				this.target=target;
				this.success=default(bool);
			}
			/// <summary>
			/// �ǂݎ��̑Ώۂ��w�肵�� Status �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="target">�ǂݎ��Ώۂ̔z����w�肵�܂��B</param>
			public Status(T[] target):this(target,0){}
			/// <summary>
			/// �ǂݎ��̑Ώۂ��w�肵�� Status �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="target">�ǂݎ��Ώۂ̔z����w�肵�܂��B</param>
			/// <param name="index">�ǂݎ��̊J�n�ʒu���w�肵�܂��B</param>
			public Status(T[] target,int index){
				this.target=new ArrayStreamAdapter<T>(target,index);
			}

			/// <summary>
			/// �����ɃL���v�`�����ꂽ�͈͂�ێ����郊�X�g���擾���܂��B
			/// </summary>
#if USE_RANGELIST
			public RangeList<ICaptureRange> Captures{
#else
			public Gen::List<ICaptureRange> Captures{
#endif
				get{return this.captures;}
			}
			/// <summary>
			/// �w�肵���ԍ��ȍ~�ɓo�^���ꂽ Capture ���폜���܂��B
			/// </summary>
			/// <param name="start">�폜���J�n����ʒu���w�肵�܂��B</param>
			internal void ClearCaptures(int start){
#if USE_RANGELIST
				this.Captures.RemoveAfter(start);
#else
				int remove_count=this.Captures.Count-start;
				if(remove_count>0)
					this.Captures.RemoveRange(start,remove_count);
#endif
			}
			/// <summary>
			/// �O��� Tester �̔��茋�ʂ��擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public bool Success{
				get{return this.success;}
				set{this.success=value;}
			}
			//========================================================
			//		Reading Target Data
			//========================================================
			readonly ITypedStream<T> target;
			/// <summary>
			/// ���݂̓ǂݎ��Ώۂ̗v�f����擾���܂��B
			/// </summary>
			public ITypedStream<T> Target{
				get{return this.target;}
			}
			/// <summary>
			/// ���݂̓ǂݎ��Ώۂ̗v�f���擾���܂��B
			/// </summary>
			public T Current{
				get{return this.target.Current;}
			}
			/// <summary>
			/// ���݂̓ǂݎ��Ώۂ̗v�f�̈ʒu���擾���͐ݒ肵�܂��B
			/// </summary>
			public int Index{
				get{return this.Target.Index;}
				set{this.Target.Index=value;}
			}
			//========================================================
			//		Tester Chain
			//========================================================
			internal Gen::Stack<ITester> st=new Gen::Stack<ITester>();
			/// <summary>
			/// ���݂̏����̑ΏۂƂȂ��Ă��� tester ���擾���܂��B
			/// ����́Atester �X�^�b�N�̈�ԏ�ɂ��� tester �ł��B
			/// </summary>
			internal ITester Tester{
				get{return this.st.Peek();}
			}
			/// <summary>
			/// �������ׂ� tester ��������܂��B
			/// </summary>
			/// <param name="test">�ǉ����� tester ���w�肵�܂��B</param>
			internal void Push(ITester test){
				this.st.Push(test);
			}
			/// <summary>
			/// �����̏I����� tester (�X�^�b�N�̈�ԏ�ɂ���) ���A�X�^�b�N�����菜���܂��B
			/// </summary>
			/// <returns>
			/// �c��� ITester �������Ȃ������A�����A�S�Ă� Node ����v�����Ƃ��� true ��Ԃ��܂��B
			/// ����ȊO�̏ꍇ�A�����A�����������I����Ă��Ȃ� Node ���c���Ă���Ƃ��� false ��Ԃ��܂��B
			/// </returns>
			internal void Pop(){
				this.st.Pop();
			}
			/// <summary>
			/// ���ݏ������Ă��� Node ���� Node �ō݂邩�ۂ����擾���܂��B
			/// </summary>
			internal bool IsRoot{
				get{return this.st.Count==1;}
			}
		}
		/// <summary>
		/// Status �̏�Ԃ𕜌�����ׂ̏���ێ����܂��B
		/// ��������ׂɂ͈ȉ��̏������K�v�ł��B
		/// �EStatus::Target ���ύX����Ă��Ȃ�
		/// �E��x Status::Captures �ɓo�^���ꂽ Capture �ɕύX��������Ă��Ȃ�
		/// </summary>
		private class StatusInfo{
			int capt_count;
			int index;
			Gen::Stack<ITester> st;
			/// <summary>
			/// �w�肵�� Status �̏����L�^���܂��B
			/// </summary>
			/// <param name="s">��Ԃ��L�^������ Status ���w�肵�܂��B</param>
			public StatusInfo(Status s){
				this.capt_count=s.Captures.Count;
				this.index=s.Target.Index;

				// �X�^�b�N�̃R�s�[ : Generics.Stack �ɂ� Clone ���Ȃ�...
				this.st=new Gen::Stack<ITester>(s.st.Count);
				ITester[] testers=s.st.ToArray();
				if(testers.Length==0)return;
				for(int i=testers.Length-1;i>0;i--){
					this.st.Push(testers[i].Clone());
				}
				this.st.Push(testers[0]); //�� �Ō�̗v�f�Ɋւ��Ă� Clone �͕K�v�Ȃ�
			}
			/// <summary>
			/// �L�^����Ă���������� Status �̏�Ԃ𕜌����܂��B
			/// </summary>
			/// <param name="s">��Ԃ𕜌����� Status ���w�肵�܂��B</param>
			public void Restore(Status s){
				s.ClearCaptures(this.capt_count);
				s.Target.Index=this.index;
				s.st=this.st;
			}
		}
		/// <summary>
		/// Status �̃X�^�b�N���ꎞ�I�ɕʂ̏��ɑҔ�����ׂ̃N���X�ł��B
		/// match �̓����ōX�� match ���g�p����ꍇ�ȂǂɁA
		/// match �� using �ň͂�Ŏg�p���܂��B
		/// </summary>
		protected class StatusStackRefuge:System.IDisposable{
			Status s;
			Gen::Stack<ITester> stack;
			/// <summary>
			/// Status �̃X�^�b�N���ꎞ�I�ɑޔ����܂��B
			/// </summary>
			/// <param name="s">�ǂݎ��Ɏg�p���Ă��� Status ���w�肵�܂��B</param>
			public StatusStackRefuge(Status s){
				this.s=s;
				this.stack=s.st;
				s.st=new Gen::Stack<ITester>();
			}
			/// <summary>
			/// �ޔ������X�^�b�N�𕜋A�����܂��B
			/// </summary>
			public void Dispose(){
				this.s.st=this.stack;
			}
		}
		//=================================================================
		//		�F�X
		//=================================================================

		#region Definitions
		/// <summary>
		/// ���K�\���̍\���v�f�������܂��B
		/// </summary>
		public interface INode{
			/// <summary>
			/// ������\�L�ɉ����錋�����x��Ԃ��܂��B
			/// </summary>
			NodeAssociativity Associativity{get;}
			/// <summary>
			/// �ǂݎ��Ɏg�p����q�m�[�h��񋓂��܂��B
			/// ����́A�L���v�`���O���[�v�̔ԍ��t���ȂǂɎg�p���܂��B
			/// </summary>
			Gen::IEnumerable<INode> EnumChildren{get;}
			/// <summary>
			/// �񌈒萫���������邩�ۂ����擾���͐ݒ肵�܂��B
			/// �q�m�[�h�ɂ���Ĕ񌈒萫���o��ꍇ�ɂ� true ��Ԃ��܂��B
			/// </summary>
			bool Nondeterministic{get;}
			/// <summary>
			/// �V���� Tester ���擾���܂��B
			/// </summary>
			/// <returns>�V�����쐬���� ITester �C���X�^���X��Ԃ��܂��B</returns>
			ITester GetTester();
			/// <summary>
			/// INode ����ǂݎ����s���܂��B
			/// ���̃��\�b�h�� Nondeterministic �� false �̏ꍇ�Ɏg�p����܂��B
			/// (Nondeterministic �� true �̏ꍇ�ɂ͎�������Ă���K�v�͂���܂���B)
			/// </summary>
			/// <param name="s">�ǂݎ�茳�� Status ���w�肵�܂��B</param>
			/// <returns>�ǂݎ��ɐ��������ꍇ�� true ��Ԃ��܂��B
			/// �ǂݎ��Ɏ��s�����ꍇ�� false ��Ԃ��܂��B</returns>
			bool Read(Status s);
		}
		/// <summary>
		/// ���K�\���̍\���v�f�ɑΉ����錟�؊�ł��B
		/// ���؂̓r���o�߂̏��Ȃǂ�ێ����āA���؂��s���܂��B
		/// </summary>
		public interface ITester{
			/// <summary>
			/// �ǂݎ����s���܂��B
			/// </summary>
			/// <param name="s">�ǂݎ��ɕK�v�ȏ���ێ�����I�u�W�F�N�g���w�肵�܂��B</param>
			/// <returns>
			/// ���̎q�ǂݎ��Ɏg�p���� ITester ��Ԃ��܂��B
			/// �q�ǂݎ������s������ɍēx���̊֐����Ă΂�܂��B
			/// �� �q�ǂݎ�������ȏ�s��Ȃ��ꍇ�� null ��Ԃ��܂��B
			/// ���̏ꍇ�ɂ́A���� ITester �̓ǂݎ��͏I���������ƌ��􂳂�܂��B
			/// </returns>
			ITester Read(Status s);
			//========================================================
			//		�񌈒萫
			//========================================================
			/// <summary>
			/// �񌈒萫�������܂��B
			/// �S�Ă̍l���������񋓂��I�����ꍇ�� false ��Ԃ��܂��B
			/// �����A���̓ǂݎ����@���l������ꍇ�� true ��Ԃ��܂��B
			/// </summary>
			/// <remarks>
			/// ����� "(Reg1|Reg2|...)" �� "Reg*", "Reg+" ���ŁA������ match �̎d�����l������ꍇ�Ɏg�p���܂��B
			/// (�g�p���Ȃ��ꍇ�ɂ͏�� false ��Ԃ��l�ɂ��Ă����Ζ��݂�܂���B)
			/// <ol>
			/// <li>���ڂ� Read �ł͎�芸�����A�ŗD��� match �̎d���ōs���܂��B
			/// Indefinite �� true ��Ԃ��l�ɂ��܂��B
			/// ���� match �̎d���Ŏ��s�����Ƃ��ɂ́A�Ă� Read ���Ăяo����܂��B</li>
			/// <li>���ڂ� Read �ł͓�ڂ� match �̎d���Ō��ʂ�Ԃ��܂��B
			/// �O�ڈȍ~�� match �̎d�����l�����鎞�ɂ́AIndefinite �� true �ɂ��܂��B
			/// �l������ match �̎d����������Ȃ��ꍇ�ɂ� Indefinite �� false ��Ԃ��l�ɂ��܂��B</li>
			/// <li>�u���K�\���S�̂� match ����v���A�umatch �̃p�^�[�����s���� Indefinite=false ��Ԃ��v�܂�
			/// "2." �̓��삪�����܂��B</li>
			/// </ol>
			/// </remarks>
			/// <example>
			/// �Ⴆ�΁A"(abcd|abc)..." �Ƃ������K�\���ɂ́A"'abcd' or 'abc'" �Ƃ��� Node (NodeA �Ƃ���) ���܂܂�Ă��܂��B
			/// ���� NodeA �� abcd �� match ���A�c��� "..." �̕����Ŏ��s�����Ƃ��܂��B
			/// ���̎� NodeA-Tester �� Indefinite==true �̏ꍇ�ɂ́A����͍Ă� NodeA �̕��������蒼���ɂȂ�܂��B
			/// (���̗�ł́Aabc �� match ���܂��B)
			/// </example>
			bool Indefinite{get;}
			/// <summary>
			/// ���݂̏�Ԃ�ێ����������̃C���X�^���X���쐬���܂��B
			/// </summary>
			/// <returns>�쐬�����C���X�^���X��Ԃ��܂��B</returns>
			/// <remarks>�q�m�[�h�����m�[�h�ȊO�ɂ��ẮA
			/// ���̃��\�b�h���Ăяo����鎖�͂���܂���B
			/// �]���Ă��̗l�ȃm�[�h�ł������������K�v�͌�����܂���B
			/// �X�ɓ�����Ԃ������Ȃ��m�[�h�ł��������������K�v�͂���܂���B
			/// </remarks>
			ITester Clone();
		}
		/// <summary>
		/// ������ŕ\�������ꍇ�̃m�[�h�̌����K���������܂��B
		/// ����́AToString �������s����ۂɎg�p����܂��B
		/// </summary>
		public enum NodeAssociativity{
			/// <summary>
			/// �ō��̌����D��x�������܂��B
			/// </summary>
			Strong=0x30,
			/// <summary>
			/// ���������D��x�������܂��B
			/// </summary>
			Stronger=0x28,
			/// <summary>
			/// ���Ԃ̌����D��x�������܂��B
			/// </summary>
			Middle=0x20,
			/// <summary>
			/// ��Ԏア�����D��x�������܂��B
			/// </summary>
			Weak=0x10
		}
		#endregion

		//=================================================================
		//		ICapture
		//=================================================================
		/// <summary>
		/// ��v�v�f�̏���񋟂��܂��B
		/// </summary>
		public interface ICaptureRange{
			/// <summary>
			/// ���̃L���v�`���Ɏg�p�����m�[�h�̃C���X�^���X��Ԃ��܂��B
			/// </summary>
			INode Node{get;}

			/// <summary>
			/// ���̗v�f�̊J�n�ʒu���擾���܂��B
			/// </summary>
			int Start{get;}
			/// <summary>
			/// ���̗v�f�̏I���ʒu���擾���܂��B
			/// </summary>
			int End{get;}
			/// <summary>
			/// ���̗v�f���ǂݎ�����͈͂̊J�n�ʒu���擾���܂��B
			/// �땝�A�T�[�V�����Ȃǂ̏ꍇ�ɂ́A����� Start �ƈقȂ�l����蓾�܂��B
			/// </summary>
			int ReadStart{get;}
			/// <summary>
			/// ���̗v�f���ǂݎ�����͈͂̏I���ʒu���擾���܂��B
			/// �땝�A�T�[�V�����Ȃǂ̏ꍇ�ɂ́A����� End �ƈقȂ�l����蓾�܂��B
			/// </summary>
			int ReadEnd{get;}

			/// <summary>
			/// �ǂݎ����J�n�������_�ł� Capture �̌����擾���܂��B
			/// ����́A�ǂ͈̗̔͂v�f������ ICapture �̎q�v�f�ł��邩���m���߂�ׂ̕��ł��B
			/// �q�v�f�������Ȃ��Ȃǂ̗��R�ł��̏����g�p���Ȃ��ꍇ�ɂ͕��̒l��Ԃ��܂��B
			/// </summary>
			int InitialCaptureCount{get;}
		}
		/// <summary>
		/// ��v�v�f�̏���񋟂���N���X�̊�{�����ł��B
		/// ��v�ł��邩�ۂ��̔���̉ߒ��ł��g�p����܂��B
		/// </summary>
		protected internal class CaptureRangeBase:ICaptureRange{
			readonly INode node;
			readonly int cInitialCapture;
			readonly int start;
			int end;
			/// <summary>
			/// CaptureRangeBase �̃C���X�^���X�����������܂��B
			/// </summary>
			/// <param name="s">�ǂݎ��Ɏg�p���Ă��� Status �̃C���X�^���X���w�肵�܂��B</param>
			/// <param name="node">��v�v�f�ɑΉ����鐳�K�\���v�f���w�肵�܂��B</param>
			public CaptureRangeBase(Status s,INode node){
				this.node=node;
				this.cInitialCapture=s.Captures.Count;
				this.start=s.Index;

				this.end=this.start; // �b��
			}

			/// <summary>
			/// ���̃L���v�`���Ɏg�p���Ă���m�[�h�̃C���X�^���X��Ԃ��܂��B
			/// </summary>
			public INode Node{get{return this.node;}}
			/// <summary>
			/// ���̗v�f�̊J�n�ʒu���擾���܂��B
			/// </summary>
			public int Start{get{return this.start;}}
			/// <summary>
			/// ���̗v�f�̏I���ʒu���擾���܂��B
			/// </summary>
			public int End{
				get{return this.end;}
				//protected internal set{this.end=value;}
			}
			/// <summary>
			/// ���̗v�f���ǂݎ�����͈͂̊J�n�ʒu���擾���܂��B
			/// �땝�A�T�[�V�����Ȃǂ̏ꍇ�ɂ́A����� Start �ƈقȂ�l����蓾�܂��B
			/// </summary>
			public virtual int ReadStart{get{return this.start;}}
			/// <summary>
			/// ���̗v�f���ǂݎ�����͈͂̏I���ʒu���擾���܂��B
			/// �땝�A�T�[�V�����Ȃǂ̏ꍇ�ɂ́A����� End �ƈقȂ�l����蓾�܂��B
			/// </summary>
			public virtual int ReadEnd{get{return this.end;}}
			/// <summary>
			/// �ǂݎ����J�n�������_�ł� Capture �̌����擾���܂��B
			/// ����́A�ǂ͈̗̔͂v�f������ ICapture �̎q�v�f�ł��邩���m���߂�ׂ̕��ł��B
			/// �q�v�f�������Ȃ��Ȃǂ̗��R�ł��̏����g�p���Ȃ��ꍇ�ɂ͕��̒l��Ԃ��܂��B
			/// </summary>
			public int InitialCaptureCount{get{return this.cInitialCapture;}}

			/// <summary>
			/// �I���ʒu���L�^���āAs.Captures �ɓo�^���s���܂��B
			/// �܂��As.Success �l�� true �ɐݒ肵�܂��B
			/// </summary>
			/// <param name="s">�o�^��� Status ���w�肵�܂��B</param>
			public virtual void Complete(Status s){
				this.end=s.Index;
				s.Captures.Add(this);
				s.Success=true;
			}
			/// <summary>
			/// ���s���������̏������s���܂��B
			/// s.Captures �ł��̗v�f�̎q�v�f�Ƃ��ēo�^���ꂽ���������܂��B
			/// �܂��As.Success �� false ��ݒ肵�܂��B
			/// </summary>
			/// <param name="s">�����Ώۂ� Status ���w�肵�܂��B</param>
			public void Fail(Status s){
				//int remove_count=s.Captures.Count-this.cInitialCapture;
				//if(remove_count>0)
				//	s.Captures.RemoveRange(this.cInitialCapture,remove_count);

				s.Success=false;
			}
			/// <summary>
			/// ���̃m�[�h�̓ǂݎ��𖳂��������ɂ��܂��B
			/// ��̓I�ɂ͂��̃m�[�h��ǂݎ��n�߂Ĉȍ~�� Capture ���폜���A
			/// �ǂݎ��ʒu�����ɖ߂��܂��B
			/// </summary>
			/// <param name="s">�Ώۂ� Status ���w�肵�܂��B</param>
			public void Clear(Status s){
				s.ClearCaptures(this.cInitialCapture);
				s.Index=this.start;
			}
			//========================================================
			//	��v���ʗp�C���X�^���X�쐬
			//========================================================
			/// <summary>
			/// ��v�S�̂ɑ΂���L���v�`�����쐬���܂��B
			/// </summary>
			/// <param name="eval">�ǂݎ����I����������� Evaluator ���w�肵�܂��B</param>
			/// <returns>�쐬���� CaptureRange �C���X�^���X��Ԃ��܂��B</returns>
			internal static CaptureRangeBase CreateInstanceForRootNode(Evaluator eval){
				return new CaptureRangeBase(eval);
			}
			private CaptureRangeBase(Evaluator eval){
				this.node=eval.Node;
				this.start=eval.StartIndex;
				this.cInitialCapture=0;
				this.end=eval.Status.Index;
			}
		}
	}
}
