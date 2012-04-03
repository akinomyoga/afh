using Gen=System.Collections.Generic;
using System.Text;
using afh.Parse;

namespace afh.RegularExpressions{
	//================================================================
	//		正規表現の構文解析
	//================================================================
	/// <summary>
	/// 正規表現の読み取りの際に発生した例外をスローする際に使用します。
	/// </summary>
	[System.Serializable]
	public class RegexParseException:System.FormatException{
		/// <summary>
		/// RegexParseException の既定のコンストラクタです。
		/// </summary>
		public RegexParseException():base(){}
		/// <summary>
		/// 指定したメッセージを使用して RegexParseException を初期化します。
		/// </summary>
		/// <param name="message">例外に関する詳細な説明を指定します。</param>
		public RegexParseException(string message):base(message){}
		/// <summary>
		/// 指定したメッセージを使用して RegexParseException を初期化します。
		/// </summary>
		/// <param name="message">例外に関する詳細な説明を指定します。</param>
		/// <param name="inner">この例外を発生させる原因となった内部例外を指定します。</param>
		public RegexParseException(string message,System.Exception inner):base(message,inner){}
		/// <summary>
		/// シリアル化された RegexParseException を復元します。
		/// </summary>
		/// <param name="info">シリアル化したデータを保持するインスタンスを指定します。</param>
		/// <param name="context">シリアル化の環境についての情報を指定します。</param>
		public RegexParseException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context
			):base(info,context)
		{}
	}
	/// <summary>
	/// コマンドの正規表現としての性質を提供します。
	/// </summary>
	public class CommandData{
		private CommandArgumentType arg_type=CommandArgumentType.None;
		/// <summary>
		/// コマンド表現の引数の形式を取得します。
		/// </summary>
		public CommandArgumentType ArgumentType{
			get{return this.arg_type;}
		}

		/// <summary>
		/// 指定した引数の形式を使用して CommandData を初期化します。
		/// </summary>
		/// <param name="argumentType">正規表現に於ける引数の形式を指定します。</param>
		public CommandData(CommandArgumentType argumentType){
			this.arg_type=argumentType;
		}
		/// <summary>
		/// 引数を取らないコマンドを表現するインスタンスです。
		/// </summary>
		public static readonly CommandData NoArg=new CommandData(CommandArgumentType.None);
		/// <summary>
		/// {} によって引数を受け取るコマンドを表現するインスタンスです。
		/// </summary>
		public static readonly CommandData Brace=new CommandData(CommandArgumentType.Brace);
	}
	/// <summary>
	/// コマンドの引数の形を表現するのに使用します。
	/// </summary>
	public enum CommandArgumentType{
		/// <summary>
		/// 引数を取らないコマンドです。
		/// </summary>
		None,
		/// <summary>
		/// コマンドが波括弧で囲まれた引数を取る事を示します。
		/// </summary>
		Brace,		// {}
		/// <summary>
		/// コマンドが四角括弧 [] で囲まれた引数を取る事を示します。
		/// </summary>
		Bracket,	// []
		/// <summary>
		/// コマンドが角括弧で囲まれた引数を取る事を示します。
		/// </summary>
		Angle,		// <>
		/// <summary>
		/// コマンドがシングルクオテーションで囲まれた引数を取る事を示します。
		/// </summary>
		Quotation,	// ''
		/// <summary>
		/// コマンドが十進整数を引数に取る事を示します。
		/// </summary>
		Decimal,	// \d+
		/// <summary>
		/// コマンドが十六進整数を引数に取る事を示します。
		/// </summary>
		HexaDecimal,// \p{xdigit}+
	}

	/// <summary>
	/// 正規表現の文法を定義するのに使用します。
	/// </summary>
	public interface IRegexGrammar{
		/// <summary>
		/// 登録されているそれぞれのコマンドについての情報を提供します。
		/// </summary>
		Gen::Dictionary<string,CommandData> CommandSet{get;}
		/// <summary>
		/// コロンで始まるコマンドを有効にするか否かを取得します。
		/// </summary>
		bool EnablesColonCommand{get;}
		/// <summary>
		/// コロンで始まるコマンドについての情報を提供します。
		/// EnablesColonCommand が false の場合には null を返して構いません。
		/// </summary>
		Gen::Dictionary<string,CommandData> ColonCommandSet{get;}
	}
	internal partial class RegexScannerA:AbstractWordReader{
		private string flags="imnsx"; // TODO: Parser に依って変更できるようにする。
		private IRegexGrammar grammar;

		private string value;
		public string Value{
			get{return this.value;}
		}
		public RegexScannerA(string text,IRegexGrammar grammar):base(text){
			this.grammar=grammar;
		}

		private static WordType WtCharClass=WordType.Literal;
		private static WordType WtCommand=WordType.Element;
		private static WordType WtCommandC=WordType.Tag;

		public const int WT_COMMAND=WordType.vElement;
		public const int WT_CHARCLASS=WordType.vLiteral;
		public const int WT_COMMAND_C=WordType.vTag;
	}
	//================================================================
	//		正規表現の適用対象
	//================================================================
	/// <summary>
	/// 特定の型の連続するインスタンスについて扱います。
	/// </summary>
	/// <typeparam name="T">並んでいるインスタンスの型を指定します。</typeparam>
	public interface ITypedStream<T>{
		/// <summary>
		/// 現在の位置を取得又は設定します。
		/// 負の値は現在の位置が Stream 外にある事を示します。
		/// 連続性は必ずしも要求されません。
		/// </summary>
		int Index{get;set;}
		/// <summary>
		/// 次の位置に移動します。
		/// 次の Index は必ずしも現在の Index+1 である必要はありません。
		/// (例えば、Surrogate 等の分を飛ばす場合などが考えられます。)
		/// </summary>
		/// <returns>新しい位置を返します。</returns>
		int MoveNext();
		/// <summary>
		/// 現在の値を取得します。
		/// </summary>
		T Current{get;}
		/// <summary>
		/// Stream の末端に達しているかどうかを取得します。
		/// Stream の外にいる場合に true を返します。
		/// Stream の中にいて現在の値が有効である場合に false を返します。
		/// </summary>
		bool EndOfStream{get;}
		/// <summary>
		/// バックトラッキングの為のクローン作成を行います。
		/// </summary>
		/// <returns>作成したクローンを返します。</returns>
		ITypedStream<T> Clone();
		//------------------------------------------------------------
		//	新規追加
		//------------------------------------------------------------
		/// <summary>
		/// 現在ストリームの開始位置にいるか否かを取得します。
		/// </summary>
		bool IsStart{get;}
		/// <summary>
		/// 前の位置に移動します。
		/// </summary>
		/// <returns>移動した後の場所を指定する番号を返します。</returns>
		int MovePrev();
		/// <summary>
		/// 前の位置にある値を取得します。
		/// </summary>
		T Previous{get;}
	}

	/// <summary>
	/// 配列と ITypedStream のアダプタです。
	/// </summary>
	public class ArrayStreamAdapter<T>:ITypedStream<T>{
		private readonly T[] data;
		private int index;
		/// <summary>
		/// ArrayStreamAdapter のインスタンスを作成します。
		/// </summary>
		/// <param name="data">読み取り対象の配列を指定します。</param>
		public ArrayStreamAdapter(T[] data):this(data,0){}
		/// <summary>
		/// ArrayStreamAdapter のインスタンスを作成します。
		/// </summary>
		/// <param name="data">読み取り対象の配列を指定します。</param>
		/// <param name="index">初期の読み取り位置を指定します。</param>
		public ArrayStreamAdapter(T[] data,int index){
			this.index=index;
			this.data=data;
		}
		/// <summary>
		/// このインスタンスが内部に保持している配列を取得します。
		/// </summary>
		public T[] Array{
			get{return this.data;}
		}
		//============================================================
		//		ITypedStream
		//============================================================
		/// <summary>
		/// 現在の読み取り位置を取得又は設定します。
		/// </summary>
		public int Index{
			get{return this.index;}
			set{this.index=value;}
		}
		/// <summary>
		/// 現在の位置を一つ次の位置へ進めます。
		/// </summary>
		/// <returns>新しい位置を返します。</returns>
		public int MoveNext(){return ++this.index;}
		/// <summary>
		/// 現在の位置を一つ前の位置へ戻します。
		/// </summary>
		/// <returns>新しい位置を返します。</returns>
		public int MovePrev(){
			if(index>0)index--;
			return index;
		}
		/// <summary>
		/// 現在の要素の値を返します。
		/// </summary>
		public T Current{
			get {return this.data[this.index];}
		}
		/// <summary>
		/// 前の位置にある値を返します。
		/// </summary>
		public T Previous{
			get{return this.data[this.index-1];}
		}
		/// <summary>
		/// Stream の末端にいるか否かを取得します。
		/// </summary>
		public bool EndOfStream{
			get{return this.index>=this.data.Length;}
		}
		/// <summary>
		/// Stream の先頭にいるか否かを取得します。
		/// </summary>
		public bool IsStart{
			get{return this.index<=0;}
		}
		/// <summary>
		/// ArrayStreamAdapter のクローンを作成します。
		/// (クローンされるのは現在位置情報だけであって、同じ配列インスタンスへの参照を保持します。)
		/// </summary>
		/// <returns>クローンを返します。</returns>
		public ITypedStream<T> Clone(){
			return new ArrayStreamAdapter<T>(this.data,this.index);
		}
	}

	/// <summary>
	/// 文字列から ITypedStream へのアダプタです。
	/// </summary>
	public class StringStreamAdapter:ITypedStream<char>{
		int index;
		readonly string text;
		/// <summary>
		/// StringStreamAdapter のインスタンスを作成します。
		/// </summary>
		/// <param name="text">読み取り対象の文字列を指定します。</param>
		/// <param name="index">文字列内の初期位置を指定します。</param>
		public StringStreamAdapter(string text,int index){
			this.text=text;
			this.index=index;
		}
		/// <summary>
		/// StringStreamAdapter のインスタンスを作成します。
		/// </summary>
		/// <param name="text">読み取り対象の文字列を指定します。</param>
		public StringStreamAdapter(string text):this(text,0){}
		/// <summary>
		/// このインスタンスが保持している文字列を取得します。
		/// </summary>
		public string ContentText{
			get{return this.text;}
		}
		/// <summary>
		/// 部分文字列を取得します。
		/// </summary>
		/// <param name="start">部分文字列の開始位置を指定します。</param>
		/// <param name="end">部分文字列の終端を指定します。最後の文字の次の位置を指定します。</param>
		/// <returns>切り出した部分文字列を返します。</returns>
		public string Substr(int start,int end){
			return this.text.Substring(start,end-start);
		}

		/// <summary>
		/// 現在位置を取得又は設定します。
		/// </summary>
		public int Index{
			get {return this.index;}
			set {this.index=value;}
		}
		/// <summary>
		/// 次の文字に移動します。
		/// </summary>
		/// <returns>移動後の位置を返します。</returns>
		public int MoveNext(){
			return ++this.index;
		}
		/// <summary>
		/// 前の文字に移動します。
		/// </summary>
		/// <returns>移動後の位置を返します。</returns>
		public int MovePrev(){
			if(index>0)index--;
			return this.index;
		}
		/// <summary>
		/// 現在の位置にある文字を返します。
		/// </summary>
		public char Current{
			get{return this.text[this.index];}
		}
		/// <summary>
		/// 前の位置にある文字を返します。
		/// </summary>
		public char Previous{
			get{return this.text[this.index-1];}
		}
		/// <summary>
		/// 文字列の終わりに達したか否かを取得します。
		/// </summary>
		public bool EndOfStream{
			get {return this.index>=this.text.Length;}
		}
		/// <summary>
		/// 現在位置が文字列の先端にあるか否かを取得又は設定します。
		/// </summary>
		public bool IsStart{
			get{return this.index==0;}
		}
		/// <summary>
		/// StringStreamAdapter のコピーを作成します。
		/// </summary>
		/// <returns>作成したコピーを返します。</returns>
		public ITypedStream<char> Clone(){
			return new StringStreamAdapter(this.text,this.index);
		}
	}

#if USE_RANGELIST
	public class RangeList<T>:Gen::ICollection<T>{
		private int count;
		private T[] data;
		public RangeList(){
			this.count=0;
			this.data=new T[4];
		}
		public RangeList(RangeList<T> copye){
			int count=copye.count;
			this.count=count;
			this.data=new T[count+4];
			for(int i=0;i<count;i++)
				this.data[i]=copye.data[i];
		}
		private void EnsureIndex(int index){
			if(index<this.data.Length)return;
			T[] newdata=new T[index*2];
			for(int i=0;i<this.count;i++)
				newdata[i]=this.data[i];
			this.data=newdata;
		}
		/// <summary>
		/// 新しい要素を末尾に追加します。
		/// </summary>
		/// <param name="item">追加する要素を指定します。</param>
		public void Add(T item){
			EnsureIndex(this.count);
			this.data[this.count++]=item;
		}
		/// <summary>
		/// 登録された要素を全て削除します。
		/// <!--(インデックスを無効にするだけなので参照は残ります。
		/// 従って登録される内容は短命であるが、このリスト自体は長寿である
		/// という様な使い方をするとメモリを浪費する可能性があります。)-->
		/// </summary>
		public void Clear(){
			this.count=0;
		}
		/// <summary>
		/// 現在登録されている要素の数を指定します。
		/// </summary>
		public int Count{
			get{return this.count;}
		}
		public bool IsReadOnly{
			get{return false;}
		}
		/// <summary>
		/// 内容の列挙子を取得します。
		/// (パフォーマンスの問題から途中のコレクションの内容変更には対応していません。)
		/// </summary>
		/// <returns>列挙子を返します。</returns>
		public System.Collections.Generic.IEnumerator<T> GetEnumerator(){
			int iM=this.count;
			for(int i=0;i<iM;i++)
				yield return this.data[i];
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
		public void CopyTo(T[] array,int arrayIndex){
			for(int i=0;i<this.count;i++)
				array[arrayIndex++]=data[i];
		}
		//===========================================================
		//		追加メソッド
		//===========================================================
		public void ClearReference(){
			int i=this.count,len=data.Length;
			while(i<len)
				this.data[i++]=default(T);
		}
		/// <summary>
		/// 指定した番号以降の要素を削除します。
		/// 指定した番号にある要素も削除します。
		/// </summary>
		/// <param name="index">最初の削除する要素の番号を指定します。</param>
		public void RemoveAfter(int index){
			for(int i=index,len=data.Length;i<len;i++)
				this.data[i]=default(T);
			this.count=index;
		}
		public T[] ToArray(){
			T[] ret=new T[this.count];
			for(int i=0;i<ret.Length;i++)
				ret[i]=this.data[i];
			return ret;
		}

		public bool Contains(T item){
			throw new System.Exception("The method or operation is not implemented.");
		}
		public bool Remove(T item) {
			throw new System.Exception("The method or operation is not implemented.");
		}
	}
#endif
}