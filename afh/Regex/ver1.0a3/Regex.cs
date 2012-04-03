#define USE_ARRAY_FOR_CAPTURES
using Gen=System.Collections.Generic;
using System.Text;

namespace afh.RegularExpressions{
	/// <summary>
	/// 正規表現を管理するクラスです。
	/// </summary>
	/// <typeparam name="T">正規表現を適用する対象の型を指定します。</typeparam>
	public partial class RegexFactory3<T>{
		/// <summary>
		/// 空のノード達を保持します。子ノードの無い INode 等の Children として使用します。
		/// </summary>
		protected readonly static Gen::IEnumerable<INode> EmptyNodes=new INode[0];

		/// <summary>
		/// 指定した Status を使用して matching を実行します。
		/// </summary>
		/// <param name="s">読み取りに使用する Status を指定します。
		/// ITester-Stack が空である事を想定しています。
		/// 中に何かある場合にはそれを別の所に待避して、new Stack を設定しておいて下さい。
		/// </param>
		/// <param name="node">要求するパターンを表現する構成体を指定します。</param>
		/// <returns>マッチングに成功した場合に true を返します。
		/// それ以外の場合に false を返します。</returns>
		internal bool match(Status s,INode node) {
			Gen::Stack<StatusInfo> st_indef=new Gen::Stack<StatusInfo>();

			s.Push(node.GetTester());
			while(true){
				ITester test=s.Tester.Read(s);

				if(!s.Success){
				// a. Node 失敗
				//-------------------
					// 全体 失敗
					if(s.IsRoot){
						// * INDEFINICITY *
						if(st_indef.Count==0)return false;
						st_indef.Pop().Restore(s);
					}else s.Pop();
				}else if(test!=null){
				// b. 入れ子 Node
				//-------------------
					s.Push(test);
				}else{
				// c. 成功 (Node 終了)
				//-------------------
					// 全体 成功
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
		/// キャプチャの基礎クラスです。
		/// </summary>
		/// <typeparam name="TCapture">派生クラス型を指定します。</typeparam>
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
			/// CaptureBase のインスタンスを初期化します。
			/// </summary>
			/// <param name="baseRange">作成する Capture 全体の Range を指定します。</param>
			/// <param name="index">baseRange の Range 番号を指定します。</param>
			protected internal CaptureBase(CaptureBase<TCapture> baseRange,int index){
				this.data=baseRange.data;
				this.icapt=index;
				this.groups=new GroupCollection<TCapture>((TCapture)this);
			}
			//========================================================
			//		子孫キャプチャ
			//========================================================
			/// <summary>
			/// この Capture の中で一致した子孫 Capture の集合を取得します。
			/// </summary>
			public CaptureCollection<TCapture> Captures{
				get{return new CaptureCollection<TCapture>((TCapture)this);}
			}
			/// <summary>
			/// このキャプチャ内にある Group の集合を取得します。
			/// </summary>
			public GroupCollection<TCapture> Groups{
				get{return this.groups;}
			}
			/// <summary>
			/// 子 Capture インスタンスを作成します。
			/// </summary>
			/// <param name="index">この Capture の子供の内での番号を指定します。</param>
			/// <returns>作成した <typeparamref name="TCapture"/> インスタンスを返します。</returns>
			protected internal abstract TCapture CreateCapture(int index);
			/// <summary>
			/// このキャプチャ全体を表す Range を取得します。
			/// </summary>
			protected internal ICaptureRange Range{
				get{return this.data.captures[icapt];}
			}
			/// <summary>
			/// このキャプチャ及び子孫の Range 番号で、一番若い物を返します。
			/// </summary>
			protected internal int StartInCaptures{
				get{return Range.InitialCaptureCount;}
			}
			/// <summary>
			/// このキャプチャ及び子孫の Range 番号で、一番大きい物を返します。
			/// 最大の物は、このキャプチャ全体についての Range になります。
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
			/// 一致の対象の値列を取得します。
			/// </summary>
			protected ITypedStream<T> TargetStream{
				get{return this.data.target;}
			}
			//========================================================
			//		このキャプチャの情報
			//========================================================
			/// <summary>
			/// 一致範囲の開始位置を取得します。
			/// </summary>
			public int Start{
				get{return Range.Start;}
			}
			/// <summary>
			/// 一致範囲の終了位置を取得します。
			/// 最後の要素番号の次を指し示します。
			/// </summary>
			public int End{
				get{return Range.End;}
			}
			/// <summary>
			/// 一致範囲の長さを取得します。
			/// 最後の要素番号の次を指し示します。
			/// </summary>
			public int Length{
				get{return Range.Start-Range.End;}
			}
			/// <summary>
			/// 一致範囲内の値の列を配列に変換して返します。
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
			/// 一致列の列挙子を提供するインスタンスを作成します。
			/// </summary>
			/// <returns>一致列の列挙子を提供するインスタンスを返します。</returns>
			public Gen::IEnumerable<T> EnumerateElements(){
				ITypedStream<T> str=data.target.Clone();
				str.Index=this.Start;
				int iM=this.End;
				do{
					yield return str.Current;
				}while(str.MoveNext()<iM);
			}
			//========================================================
			//		比較
			//========================================================
			/// <summary>
			/// キャプチャの対象が同じであるか否かを判定します。
			/// </summary>
			/// <param name="left">比較するインスタンスを指定します。</param>
			/// <param name="right">比較するインスタンスを指定します。</param>
			/// <returns>対象が同じであると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public static bool operator==(CaptureBase<TCapture> left,CaptureBase<TCapture> right){
				if((object)left==null||(object)right==null)return (object)left==(object)right;
				return left.icapt==right.icapt&&left.data==right.data;
			}
			/// <summary>
			/// キャプチャの対象が異なるか否かを判定します。
			/// </summary>
			/// <param name="left">比較するインスタンスを指定します。</param>
			/// <param name="right">比較するインスタンスを指定します。</param>
			/// <returns>対象が異なると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public static bool operator!=(CaptureBase<TCapture> left,CaptureBase<TCapture> right){
				return !(left==right);
			}
			/// <summary>
			/// 指定したオブジェクトインスタンスと等値比較を行います。
			/// </summary>
			/// <param name="obj">比較対象のオブジェクトインスタンスを指定します。</param>
			/// <returns>指定した object が CaptureBase&lt;TCapture&gt; 型であって、
			/// 更にこのインスタンスと内容が一致した場合に true を返します。
			/// それ以外の場合に false を返します。</returns>
			public override bool Equals(object obj){
				CaptureBase<TCapture> r=obj as CaptureBase<TCapture>;
				return r!=null&&r==this;
			}
			/// <summary>
			/// このインスタンスに関するハッシュ値を計算します。
			/// </summary>
			/// <returns>このインスタンスの内容に基づいて計算されたハッシュ値を返します。</returns>
			public override int GetHashCode(){
				return this.icapt.GetHashCode()^this.data.GetHashCode();
			}
		}
		/// <summary>
		/// Capture の集合を記述するクラスです。
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
			/// 指定した番号に対応する Capture を取得します。
			/// </summary>
			/// <param name="index">取得する Capture の番号を指定します。</param>
			/// <returns>指定した番号に対応する Capture を返します。</returns>
			public TCapture this[int index] {
				get{
					index+=parent.StartInCaptures;
					if(index<parent.StartInCaptures||parent.EndInCaptures<=index)
						throw new System.ArgumentOutOfRangeException("添字の範囲が不正です。");
					return parent.CreateCapture(index);
				}
			}
			/// <summary>
			/// これに含まれている Capture の数を取得又は設定します。
			/// </summary>
			public int Count{
				get{return parent.EndInCaptures-parent.StartInCaptures;}
			}
			/// <summary>
			/// 指定した Capture インスタンスがこれに含まれているか否かを取得又は設定します。
			/// </summary>
			/// <param name="capture">含まれているか否かを判定する Capture インスタンスを指定します。</param>
			/// <returns>指定したインスタンスが、この CaptureCollection に含まれていた場合に true を返します。
			/// 含まれていなかった場合に false を返します。</returns>
			public bool Contains(TCapture capture) {
				throw new System.Exception("The method or operation is not implemented.");
			}
			/// <summary>
			/// 配列に Capture の値をコピーします。
			/// </summary>
			/// <param name="array">コピー先の配列を指定します。</param>
			/// <param name="arrayIndex">コピー先の配列に於ける、コピーの開始位置を指定します。
			/// 指定した位置に CaptureCollection の一番初めの要素が書き込まれます。</param>
			public void CopyTo(TCapture[] array,int arrayIndex) {
				int end=arrayIndex+this.Count;
				if(arrayIndex<0||array.Length<end)
					throw new System.ArgumentOutOfRangeException("arrayIndex","コピー先の配列に入りません。充分大きな配列を用意するか arrayIndex を適切に設定して下さい。");

				int index=parent.StartInCaptures;
				while(arrayIndex<end){
					array[arrayIndex++]=parent.CreateCapture(index++);
				}
			}
			/// <summary>
			/// Capture の列挙子を取得します。
			/// </summary>
			/// <returns>作成した列挙子を返します。</returns>
			public System.Collections.Generic.IEnumerator<TCapture> GetEnumerator() {
				for(int index=parent.StartInCaptures;index<this.parent.EndInCaptures;index++)
					yield return parent.CreateCapture(index);
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
				return this.GetEnumerator();
			}
			void Gen::ICollection<TCapture>.Add(TCapture item) {
				throw new System.NotSupportedException("CaptureCollection はコレクションの変更に対応していません。");
			}
			void Gen::ICollection<TCapture>.Clear() {
				throw new System.NotSupportedException("CaptureCollection はコレクションの変更に対応していません。");
			}
			bool Gen::ICollection<TCapture>.IsReadOnly {
				get{return true;}
			}
			bool Gen::ICollection<TCapture>.Remove(TCapture item) {
				throw new System.NotSupportedException("CaptureCollection はコレクションの変更に対応していません。");
			}
		}
		/// <summary>
		/// 特定のノードに依って一致した Capture を纏めて扱う為のクラスです。
		/// </summary>
		/// <typeparam name="TCapture">Capture の型を指定します。</typeparam>
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
			/// Group 内での最後の Capture を取得します。
			/// </summary>
			public TCapture Last{
				get{
					if(this.cache!=null)
						return this.cache[this.cache.Count-1];

					// 最後の一致だけしか使わない場合 (cache 作成はしない)
					for(int i=parent.EndInCaptures-1,iM=parent.StartInCaptures;i>=iM;i--){
						if(!IsMember(parent.CaptureRanges[i].Node))continue;
						return parent.CreateCapture(i);
					}
					return null; // 一個もない場合
				}
			}
			//========================================================
			//		初期化
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
					// 自分自身だけに一致
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
			/// 指定した番号に対応する Node を取得します。
			/// </summary>
			/// <param name="index">
			/// index が 0 の場合にはそのノード自体が返されます。
			/// index が正の場合には、そのノードの子ノードの中で
			/// index 番目 (1 から始まる番号) にある CaptureNode を返します。
			/// </param>
			/// <param name="basis">検索の基準になるノードを指定します。</param>
			/// <returns>指定した番号に対応するノードが見つかった場合にそれを返します。
			/// 見つからなかった場合には null を返します。</returns>
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
			//		コレクションとして
			//========================================================
			/// <summary>
			/// Group に属する指定した番号の Capture を取得します。
			/// </summary>
			/// <param name="index">Group 内の Capture を指し示す番号を指定します。</param>
			/// <returns>指定した番号の Capture を返します。</returns>
			public TCapture this[int index]{
				get{return this.Cache[index];}
			}
			/// <summary>
			/// 指定した Capture がこの Group の中に含まれている否かを判定します。
			/// </summary>
			/// <param name="item">含まれているか否かを確認したい Capture を指定します。</param>
			/// <returns>指定した Capture が含まれていた場合に true を返します。
			/// 含まれていなかった場合に false を返します。</returns>
			public bool Contains(TCapture item){
				return this.Cache.Contains(item);
			}
			/// <summary>
			/// Group に含まれる Capture の数を取得します。
			/// </summary>
			public int Count{
				get {return this.Cache.Count;}
			}
			/// <summary>
			/// Group に含まれる Capture の集合を配列に書き出します。
			/// </summary>
			/// <param name="array">書き出す対象の配列を指定します。</param>
			/// <param name="arrayIndex">書き出しを開始する array に於ける index を指定します。
			/// 一番最初の Capture が array[arrayIndex] に書き込まれます。</param>
			public void CopyTo(TCapture[] array,int arrayIndex) {
				this.Cache.CopyTo(array,arrayIndex);
			}
			/// <summary>
			/// Capture の列挙子を取得します。
			/// </summary>
			/// <returns>作成した Capture の列挙子を返します。</returns>
			public Gen::IEnumerator<TCapture> GetEnumerator(){
				return this.Cache.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
				return this.GetEnumerator();
			}

			void Gen::ICollection<TCapture>.Add(TCapture item){
				throw new System.NotSupportedException("Group はコレクションの変更に対応していません。");
			}
			void Gen::ICollection<TCapture>.Clear() {
				throw new System.NotSupportedException("Group はコレクションの変更に対応していません。");
			}
			bool Gen::ICollection<TCapture>.IsReadOnly {
				get {return false;}
			}
			bool Gen::ICollection<TCapture>.Remove(TCapture item) {
				throw new System.NotSupportedException("Group はコレクションの変更に対応していません。");
			}
		}
		/// <summary>
		/// Capture の子 Group を生成して返す為のクラスです。。
		/// </summary>
		/// <typeparam name="TCapture">Capture の型を指定します。</typeparam>
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
			/// 指定した番号の一致グループを取得します。
			/// </summary>
			/// <param name="index">取得するグループの番号を指定します。
			/// 番号は、現在のノードを基準にして付けられます。
			/// 則ち、現在のノードの子ノードの</param>
			/// <returns>指定した番号の一致グループを返します。</returns>
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
			/// 指定した名前の一致グループを取得します。
			/// </summary>
			/// <param name="name">取得する一致グループの名前を指定します。</param>
			/// <returns>指定した名前の一致グループを返します。</returns>
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
			/// 読み取りに使用しているルートノードを取得します。
			/// </summary>
			public INode Node{
				get{return this.node;}
			}
			/// <summary>
			/// 現在の読み取り開始位置を取得します。
			/// </summary>
			public int StartIndex{
				get{return this.index0;}
			}
			/// <summary>
			/// 読み取り状態を表す Status を取得します。
			/// </summary>
			public Status Status{
				get{return this.s;}
			}
			//=======================================================
			//		マッチ結果
			//=======================================================
			//=======================================================
			//		マッチの設定
			//=======================================================
			private bool fix_start=false;
			private bool overlap_search=false;
			/// <summary>
			/// 別の開始点で試さない場合に true の値を持ちます。
			/// </summary>
			public bool FixStart{
				get{return this.fix_start;}
				set{this.fix_start=value;}
			}
			/// <summary>
			/// 一度一致した範囲での別の一致も試みるか否かを取得又は設定します。
			/// 別の一致も試みる場合に true を指定します。
			/// それ以外の場合には false を指定します。
			/// 既定値は false です。
			/// </summary>
			public bool OverlapSearch{
				get{return this.overlap_search;}
				set{this.overlap_search=value;}
			}
			/*
			bool end_of_match=false;
			/// <summary>
			/// マッチが終了したか否かを取得します。
			/// もうこれ以上マッチする物がない時に true の値を返します。
			/// </summary>
			bool EndOfMatch{
				get{__debug__.RegexTestNotImplemented();return false;}
			}
			//*/
			//=======================================================
			//		マッチ
			//=======================================================
			bool first=true;
			/// <summary>
			/// 次に一致する部分を検索します。
			/// </summary>
			/// <returns>検索の結果一致する部分が見つかった場合に true を返します。
			/// それ以外の場合に false を返します。</returns>
			public bool Match(){
				do{
					bool fInit=false;
					if(first){
						first=false;
						fInit=true;
					}else{
						__debug__.RegexTestAssert(s.IsRoot);

						if(OverlapSearch&&st_indef.Count!=0){
							// 次の可能性
							st_indef.Pop().Restore(s);
						}else if(!FixStart&&!s.Target.EndOfStream){
							st_indef.Clear();

							// 次の検索開始点
							s.Pop();
							s.ClearCaptures(0);
							if(!OverlapSearch){
								if(s.Index==index0)
									// 空一致の次
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
				Status s=this.s; // ローカルに
				while(true){
					ITester test=s.Tester.Read(s);

					if(!s.Success){
					// a. Node 失敗
					//-------------------
						// 全体 失敗
						if(s.IsRoot){
							// * INDEFINICITY *
							if(st_indef.Count==0)return false;
							st_indef.Pop().Restore(s);
						}else s.Pop();
					}else if(test!=null){
					// b. 入れ子 Node
					//-------------------
						s.Push(test);
					}else{
					// c. 成功 (Node 終了)
					//-------------------
						if(s.Tester.Indefinite){
							// * INDEFINICITY *
							st_indef.Push(new StatusInfo(s));
						}

						// 全体 成功
						if(s.IsRoot)return true;

						s.Pop();
					}
				}
			}

			// NextPossibility
		}
	}
}