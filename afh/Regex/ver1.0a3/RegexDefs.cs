//#define USE_RANGELIST
using Gen=System.Collections.Generic;
using System.Text;

namespace afh.RegularExpressions{
	public partial class RegexFactory3<T>{
		//=================================================================
		//		Status
		//=================================================================
		/// <summary>
		/// 現在の読み取りの状態を保持するクラスです。
		/// </summary>
		public class Status{
#if USE_RANGELIST
			RangeList<ICaptureRange> captures=new RangeList<ICaptureRange>();
#else
			Gen::List<ICaptureRange> captures=new Gen::List<ICaptureRange>();
#endif
			bool success;

			/// <summary>
			/// 読み取りの対象を指定して Status のインスタンスを初期化します。
			/// </summary>
			/// <param name="target">読み取りの対象を格納しているオブジェクトを指定します。</param>
			public Status(ITypedStream<T> target){
				this.target=target;
				this.success=default(bool);
			}
			/// <summary>
			/// 読み取りの対象を指定して Status のインスタンスを初期化します。
			/// </summary>
			/// <param name="target">読み取り対象の配列を指定します。</param>
			public Status(T[] target):this(target,0){}
			/// <summary>
			/// 読み取りの対象を指定して Status のインスタンスを初期化します。
			/// </summary>
			/// <param name="target">読み取り対象の配列を指定します。</param>
			/// <param name="index">読み取りの開始位置を指定します。</param>
			public Status(T[] target,int index){
				this.target=new ArrayStreamAdapter<T>(target,index);
			}

			/// <summary>
			/// 今迄にキャプチャされた範囲を保持するリストを取得します。
			/// </summary>
#if USE_RANGELIST
			public RangeList<ICaptureRange> Captures{
#else
			public Gen::List<ICaptureRange> Captures{
#endif
				get{return this.captures;}
			}
			/// <summary>
			/// 指定した番号以降に登録された Capture を削除します。
			/// </summary>
			/// <param name="start">削除を開始する位置を指定します。</param>
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
			/// 前回の Tester の判定結果を取得または設定します。
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
			/// 現在の読み取り対象の要素列を取得します。
			/// </summary>
			public ITypedStream<T> Target{
				get{return this.target;}
			}
			/// <summary>
			/// 現在の読み取り対象の要素を取得します。
			/// </summary>
			public T Current{
				get{return this.target.Current;}
			}
			/// <summary>
			/// 現在の読み取り対象の要素の位置を取得又は設定します。
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
			/// 現在の処理の対象となっている tester を取得します。
			/// これは、tester スタックの一番上にある tester です。
			/// </summary>
			internal ITester Tester{
				get{return this.st.Peek();}
			}
			/// <summary>
			/// 処理すべき tester を一つ加えます。
			/// </summary>
			/// <param name="test">追加する tester を指定します。</param>
			internal void Push(ITester test){
				this.st.Push(test);
			}
			/// <summary>
			/// 処理の終わった tester (スタックの一番上にある) を、スタックから取り除きます。
			/// </summary>
			/// <returns>
			/// 残りの ITester が無くなった時、則ち、全ての Node が一致したときに true を返します。
			/// それ以外の場合、則ち、未だ処理が終わっていない Node が残っているときに false を返します。
			/// </returns>
			internal void Pop(){
				this.st.Pop();
			}
			/// <summary>
			/// 現在処理している Node が根 Node で在るか否かを取得します。
			/// </summary>
			internal bool IsRoot{
				get{return this.st.Count==1;}
			}
		}
		/// <summary>
		/// Status の状態を復元する為の情報を保持します。
		/// 復元する為には以下の条件が必要です。
		/// ・Status::Target が変更されていない
		/// ・一度 Status::Captures に登録された Capture に変更が加わっていない
		/// </summary>
		private class StatusInfo{
			int capt_count;
			int index;
			Gen::Stack<ITester> st;
			/// <summary>
			/// 指定した Status の情報を記録します。
			/// </summary>
			/// <param name="s">状態を記録したい Status を指定します。</param>
			public StatusInfo(Status s){
				this.capt_count=s.Captures.Count;
				this.index=s.Target.Index;

				// スタックのコピー : Generics.Stack には Clone がない...
				this.st=new Gen::Stack<ITester>(s.st.Count);
				ITester[] testers=s.st.ToArray();
				if(testers.Length==0)return;
				for(int i=testers.Length-1;i>0;i--){
					this.st.Push(testers[i].Clone());
				}
				this.st.Push(testers[0]); //← 最後の要素に関しては Clone は必要ない
			}
			/// <summary>
			/// 記録されている情報を元に Status の状態を復元します。
			/// </summary>
			/// <param name="s">状態を復元する Status を指定します。</param>
			public void Restore(Status s){
				s.ClearCaptures(this.capt_count);
				s.Target.Index=this.index;
				s.st=this.st;
			}
		}
		/// <summary>
		/// Status のスタックを一時的に別の所に待避する為のクラスです。
		/// match の内部で更に match を使用する場合などに、
		/// match を using で囲んで使用します。
		/// </summary>
		protected class StatusStackRefuge:System.IDisposable{
			Status s;
			Gen::Stack<ITester> stack;
			/// <summary>
			/// Status のスタックを一時的に退避します。
			/// </summary>
			/// <param name="s">読み取りに使用している Status を指定します。</param>
			public StatusStackRefuge(Status s){
				this.s=s;
				this.stack=s.st;
				s.st=new Gen::Stack<ITester>();
			}
			/// <summary>
			/// 退避したスタックを復帰させます。
			/// </summary>
			public void Dispose(){
				this.s.st=this.stack;
			}
		}
		//=================================================================
		//		色々
		//=================================================================

		#region Definitions
		/// <summary>
		/// 正規表現の構成要素を示します。
		/// </summary>
		public interface INode{
			/// <summary>
			/// 文字列表記に於ける結合強度を返します。
			/// </summary>
			NodeAssociativity Associativity{get;}
			/// <summary>
			/// 読み取りに使用する子ノードを列挙します。
			/// これは、キャプチャグループの番号付けなどに使用します。
			/// </summary>
			Gen::IEnumerable<INode> EnumChildren{get;}
			/// <summary>
			/// 非決定性を持ち得るか否かを取得又は設定します。
			/// 子ノードによって非決定性が出る場合にも true を返します。
			/// </summary>
			bool Nondeterministic{get;}
			/// <summary>
			/// 新しい Tester を取得します。
			/// </summary>
			/// <returns>新しく作成した ITester インスタンスを返します。</returns>
			ITester GetTester();
			/// <summary>
			/// INode から読み取りを行います。
			/// このメソッドは Nondeterministic が false の場合に使用されます。
			/// (Nondeterministic が true の場合には実装されている必要はありません。)
			/// </summary>
			/// <param name="s">読み取り元の Status を指定します。</param>
			/// <returns>読み取りに成功した場合に true を返します。
			/// 読み取りに失敗した場合に false を返します。</returns>
			bool Read(Status s);
		}
		/// <summary>
		/// 正規表現の構成要素に対応する検証器です。
		/// 検証の途中経過の情報などを保持して、検証を行います。
		/// </summary>
		public interface ITester{
			/// <summary>
			/// 読み取りを行います。
			/// </summary>
			/// <param name="s">読み取りに必要な情報を保持するオブジェクトを指定します。</param>
			/// <returns>
			/// 次の子読み取りに使用する ITester を返します。
			/// 子読み取りを実行した後に再度この関数が呼ばれます。
			/// ※ 子読み取りをこれ以上行わない場合は null を返します。
			/// この場合には、この ITester の読み取りは終了した物と見做されます。
			/// </returns>
			ITester Read(Status s);
			//========================================================
			//		非決定性
			//========================================================
			/// <summary>
			/// 非決定性を示します。
			/// 全ての考え得る候補を列挙し終えた場合に false を返します。
			/// 未だ、他の読み取り方法が考えられる場合に true を返します。
			/// </summary>
			/// <remarks>
			/// これは "(Reg1|Reg2|...)" や "Reg*", "Reg+" 等で、複数の match の仕方が考えられる場合に使用します。
			/// (使用しない場合には常に false を返す様にしておけば問題在りません。)
			/// <ol>
			/// <li>一回目の Read では取り敢えず、最優先の match の仕方で行います。
			/// Indefinite は true を返す様にします。
			/// その match の仕方で失敗したときには、再び Read が呼び出されます。</li>
			/// <li>二回目の Read では二つ目の match の仕方で結果を返します。
			/// 三つ目以降の match の仕方が考えられる時には、Indefinite を true にします。
			/// 考えられる match の仕方が二つしかない場合には Indefinite は false を返す様にします。</li>
			/// <li>「正規表現全体が match する」か、「match のパターンが尽きて Indefinite=false を返す」まで
			/// "2." の動作が続きます。</li>
			/// </ol>
			/// </remarks>
			/// <example>
			/// 例えば、"(abcd|abc)..." という正規表現には、"'abcd' or 'abc'" という Node (NodeA とする) が含まれています。
			/// 初め NodeA で abcd に match し、残りの "..." の部分で失敗したとします。
			/// この時 NodeA-Tester が Indefinite==true の場合には、判定は再び NodeA の部分からやり直しになります。
			/// (この例では、abc に match します。)
			/// </example>
			bool Indefinite{get;}
			/// <summary>
			/// 現在の状態を保持するもう一つのインスタンスを作成します。
			/// </summary>
			/// <returns>作成したインスタンスを返します。</returns>
			/// <remarks>子ノードを持つノード以外については、
			/// このメソッドが呼び出される事はありません。
			/// 従ってその様なノードでこれを実装する必要は御座いません。
			/// 更に内部状態を持たないノードでもこれを実装する必要はありません。
			/// </remarks>
			ITester Clone();
		}
		/// <summary>
		/// 文字列で表現した場合のノードの結合規則を示します。
		/// これは、ToString 等を実行する際に使用されます。
		/// </summary>
		public enum NodeAssociativity{
			/// <summary>
			/// 最高の結合優先度を持ちます。
			/// </summary>
			Strong=0x30,
			/// <summary>
			/// 高い結合優先度を持ちます。
			/// </summary>
			Stronger=0x28,
			/// <summary>
			/// 中間の結合優先度を持ちます。
			/// </summary>
			Middle=0x20,
			/// <summary>
			/// 一番弱い結合優先度を持ちます。
			/// </summary>
			Weak=0x10
		}
		#endregion

		//=================================================================
		//		ICapture
		//=================================================================
		/// <summary>
		/// 一致要素の情報を提供します。
		/// </summary>
		public interface ICaptureRange{
			/// <summary>
			/// このキャプチャに使用したノードのインスタンスを返します。
			/// </summary>
			INode Node{get;}

			/// <summary>
			/// この要素の開始位置を取得します。
			/// </summary>
			int Start{get;}
			/// <summary>
			/// この要素の終了位置を取得します。
			/// </summary>
			int End{get;}
			/// <summary>
			/// この要素が読み取った範囲の開始位置を取得します。
			/// 零幅アサーションなどの場合には、これは Start と異なる値を取り得ます。
			/// </summary>
			int ReadStart{get;}
			/// <summary>
			/// この要素が読み取った範囲の終了位置を取得します。
			/// 零幅アサーションなどの場合には、これは End と異なる値を取り得ます。
			/// </summary>
			int ReadEnd{get;}

			/// <summary>
			/// 読み取りを開始した時点での Capture の個数を取得します。
			/// これは、どの範囲の要素がこの ICapture の子要素であるかを確かめる為の物です。
			/// 子要素を持たないなどの理由でこの情報を使用しない場合には負の値を返します。
			/// </summary>
			int InitialCaptureCount{get;}
		}
		/// <summary>
		/// 一致要素の情報を提供するクラスの基本実装です。
		/// 一致できるか否かの判定の過程でも使用されます。
		/// </summary>
		protected internal class CaptureRangeBase:ICaptureRange{
			readonly INode node;
			readonly int cInitialCapture;
			readonly int start;
			int end;
			/// <summary>
			/// CaptureRangeBase のインスタンスを初期化します。
			/// </summary>
			/// <param name="s">読み取りに使用している Status のインスタンスを指定します。</param>
			/// <param name="node">一致要素に対応する正規表現要素を指定します。</param>
			public CaptureRangeBase(Status s,INode node){
				this.node=node;
				this.cInitialCapture=s.Captures.Count;
				this.start=s.Index;

				this.end=this.start; // 暫定
			}

			/// <summary>
			/// このキャプチャに使用しているノードのインスタンスを返します。
			/// </summary>
			public INode Node{get{return this.node;}}
			/// <summary>
			/// この要素の開始位置を取得します。
			/// </summary>
			public int Start{get{return this.start;}}
			/// <summary>
			/// この要素の終了位置を取得します。
			/// </summary>
			public int End{
				get{return this.end;}
				//protected internal set{this.end=value;}
			}
			/// <summary>
			/// この要素が読み取った範囲の開始位置を取得します。
			/// 零幅アサーションなどの場合には、これは Start と異なる値を取り得ます。
			/// </summary>
			public virtual int ReadStart{get{return this.start;}}
			/// <summary>
			/// この要素が読み取った範囲の終了位置を取得します。
			/// 零幅アサーションなどの場合には、これは End と異なる値を取り得ます。
			/// </summary>
			public virtual int ReadEnd{get{return this.end;}}
			/// <summary>
			/// 読み取りを開始した時点での Capture の個数を取得します。
			/// これは、どの範囲の要素がこの ICapture の子要素であるかを確かめる為の物です。
			/// 子要素を持たないなどの理由でこの情報を使用しない場合には負の値を返します。
			/// </summary>
			public int InitialCaptureCount{get{return this.cInitialCapture;}}

			/// <summary>
			/// 終了位置を記録して、s.Captures に登録を行います。
			/// また、s.Success 値を true に設定します。
			/// </summary>
			/// <param name="s">登録先の Status を指定します。</param>
			public virtual void Complete(Status s){
				this.end=s.Index;
				s.Captures.Add(this);
				s.Success=true;
			}
			/// <summary>
			/// 失敗をした時の処理を行います。
			/// s.Captures でこの要素の子要素として登録された物を消します。
			/// また、s.Success に false を設定します。
			/// </summary>
			/// <param name="s">処理対象の Status を指定します。</param>
			public void Fail(Status s){
				//int remove_count=s.Captures.Count-this.cInitialCapture;
				//if(remove_count>0)
				//	s.Captures.RemoveRange(this.cInitialCapture,remove_count);

				s.Success=false;
			}
			/// <summary>
			/// このノードの読み取りを無かった事にします。
			/// 具体的にはこのノードを読み取り始めて以降の Capture を削除し、
			/// 読み取り位置を元に戻します。
			/// </summary>
			/// <param name="s">対象の Status を指定します。</param>
			public void Clear(Status s){
				s.ClearCaptures(this.cInitialCapture);
				s.Index=this.start;
			}
			//========================================================
			//	一致結果用インスタンス作成
			//========================================================
			/// <summary>
			/// 一致全体に対するキャプチャを作成します。
			/// </summary>
			/// <param name="eval">読み取りを終了した直後の Evaluator を指定します。</param>
			/// <returns>作成した CaptureRange インスタンスを返します。</returns>
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
