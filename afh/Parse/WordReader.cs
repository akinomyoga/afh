namespace afh.Parse{
	/// <summary>
	/// 読み取った単語の種類を表現する構造体です。
	/// </summary>
	public struct WordType{
		/// <summary>
		/// 単語の種類を保持します。
		/// </summary>
		public int value;
		/// <summary>
		/// 新しい WordType を初期化します。
		/// </summary>
		/// <param name="value">
		/// WordType を識別する番号です。
		/// 0 から 255 迄は既に使用亦は予約されています。別の物を使用して下さい。
		/// </param>
		public WordType(int value){this.value=value;}
		//=================================================
		//		定数
		//=================================================
		/// <summary>無効な単語を表す数値です。</summary>
		public const int vInvalid=0;
		/// <summary>空白を表す数値です。</summary>
		public const int vSpace=1;
		/// <summary>識別子を表す数値です。</summary>
		public const int vIdentifier=2;
		/// <summary>演算子を表す数値です。</summary>
		public const int vOperator=3;
		/// <summary>文字列表現を表す数値です。</summary>
		public const int vLiteral=4;
		/// <summary>キーワードを表す数値です。</summary>
		public const int vKeyWord=5;
		/// <summary>コメントを表す数値です。</summary>
		public const int vComment=6;
		/// <summary>無効な単語を表す WordType です。</summary>
		public static readonly WordType Invalid=new WordType(0);
		/// <summary>空白を表す WordType です。</summary>
		public static readonly WordType Space=new WordType(1);
		/// <summary>識別子を表す WordType です。</summary>
		public static readonly WordType Identifier=new WordType(2);
		/// <summary>演算子を表す WordType です。</summary>
		public static readonly WordType Operator=new WordType(3);
		/// <summary>文字列表現を表す WordType です。</summary>
		public static readonly WordType Literal=new WordType(4);
		/// <summary>キーワードを表す WordType です。</summary>
		public static readonly WordType KeyWord=new WordType(5);
		/// <summary>コメントを表す WordType です。</summary>
		public static readonly WordType Comment=new WordType(6);
		/// <summary>接頭辞を表す数値です。</summary>
		public const int vPrefix=0x10;
		/// <summary>接尾辞を表す数値です。</summary>
		public const int vSuffix=0x11;
		/// <summary>修飾子を表す数値です。</summary>
		public const int vModifier=0x12;
		/// <summary>文字列を表す数値です。</summary>
		public const int vText=0x13;
		/// <summary>何らかのタグを表す数値です。</summary>
		public const int vTag=0x14;
		/// <summary>何らかの属性を表す数値です。</summary>
		public const int vAttribute=0x15;
		/// <summary>何らかの要素を表す数値です。</summary>
		public const int vElement=0x16;
		/// <summary>接頭辞を表す WordType です。</summary>
		public static readonly WordType Prefix=new WordType(0x10);
		/// <summary>接尾辞を表す WordType です。</summary>
		public static readonly WordType Suffix=new WordType(0x11);
		/// <summary>修飾子を表す WordType です。</summary>
		public static readonly WordType Modifier=new WordType(0x12);
		/// <summary>文字列を表す WordType です。</summary>
		public static readonly WordType Text=new WordType(0x13);
		/// <summary>何らかのタグを表す WordType です。</summary>
		public static readonly WordType Tag=new WordType(0x14);
		/// <summary>何らかの属性を表す WordType です。</summary>
		public static readonly WordType Attribute=new WordType(0x15);
		/// <summary>何らかの要素を表す WordType です。</summary>
		public static readonly WordType Element=new WordType(0x16);
		//=================================================
		//		文字列に変換
		//=================================================
		/// <summary>
		/// 現在の単語の種類を表す文字列を取得します。
		/// </summary>
		/// <returns>現在の単語の種類を表す文字列を返します。</returns>
		public override string ToString() {
			if(names.ContainsKey(this.value))return names[this.value];
			return "不明[value="+this.value.ToString()+"]";
		}
		private static System.Collections.Generic.Dictionary<int,string> names
			=new System.Collections.Generic.Dictionary<int,string>();
		static WordType(){
			names.Add(0,"Invalid");
			names.Add(1,"Space");
			names.Add(2,"Identifier");
			names.Add(3,"Operator");
			names.Add(4,"Literal");
			names.Add(5,"KeyWord");
			names.Add(6,"Comment");
			names.Add(0x10,"Prefix");
			names.Add(0x11,"Suffix");
			names.Add(0x12,"Modifier");
			names.Add(0x13,"Text");
			names.Add(0x14,"Tag");
			names.Add(0x15,"Attribute");
			names.Add(0x16,"Element");
		}
		//=================================================
		//		比較演算
		//=================================================
		/// <summary>
		/// このインスタンスの表す単語の種類に特有の hash 値を取得します。
		/// </summary>
		/// <returns>このインスタンスの表す単語の種類に特有の hash 値を返します。</returns>
		public override int GetHashCode() {
			return this.value.GetHashCode();
		}
		/// <summary>
		/// 等値比較を行います。
		/// </summary>
		/// <param name="obj">比較対象のオブジェクトを指定します。</param>
		/// <returns>obj が WordType のインスタンスであって且つ表す単語の種類が此のインスタンスと同じである場合に true を返します。
		/// それ以外の場合には false を返します。</returns>
		public override bool Equals(object obj) {
			return obj.GetType()==typeof(WordType)&&((WordType)obj).value==this.value;
		}
		/// <summary>
		/// 等値比較を行います。
		/// </summary>
		/// <param name="a">比較の対象のインスタンスを指定します。</param>
		/// <param name="b">比較の対象のインスタンスを指定します。</param>
		/// <returns>表す単語の種類が同じ際に true を返します。それ以外の時には false を返します。</returns>
		public static bool operator ==(WordType a,WordType b){return a.value==b.value;}
		/// <summary>
		/// 不等比較を行います。
		/// </summary>
		/// <param name="a">比較の対象のインスタンスを指定します。</param>
		/// <param name="b">比較の対象のインスタンスを指定します。</param>
		/// <returns>表す単語の種類が異なる際に true を返します。それ以外の時には false を返します。</returns>
		public static bool operator !=(WordType a,WordType b){return !(a==b);}
	}
	/// <summary>
	/// 文字列などの対象から語句を読み取るクラスです。
	/// 語句の内容 (System.String) と、語句の種類 (WordType) を返します。
	/// </summary>
	public interface IWordReader{
		/// <summary>
		/// 現在の語句の内容を取得します。
		/// </summary>
		string CurrentWord{get;}
		/// <summary>
		/// 現在の語句の種類を表す WordType を返します。
		/// </summary>
		WordType CurrentType{get;}
		/// <summary>
		/// 次の単語に移動します。
		/// </summary>
		/// <returns>
		/// 次の単語がなかった場合に false を返します。
		/// それ以外の時には true を返します。
		/// </returns>
		bool ReadNext();
	}
	/// <summary>
	/// LinearLetterReader を通じて語句を読み取るクラスです。IWordReader を実装します。
	/// </summary>
	public abstract partial class AbstractWordReader:IWordReader{
		/// <summary>
		/// 次の単語に移動します。
		/// </summary>
		/// <returns>
		/// 次の単語がなかった場合に false を返します。
		/// それ以外の時には true を返します。
		/// </returns>
		public abstract bool ReadNext();
		/// <summary>
		/// AbstractWordReader のコンストラクタです。
		/// 指定した文字列を元にして新しいインスタンスを作成します。
		/// </summary>
		/// <param name="text">読み取る対象である文字列を指定します。</param>
		protected AbstractWordReader(string text):this(new LinearLetterReader(text)){}
		/// <summary>
		/// AbstractWordReader のコンストラクタです。
		/// 指定した LinearLetterReader を元にして新しいインスタンスを作成します。
		/// </summary>
		/// <param name="lreader">読み取る対象の LinearLetterReader を指定します。</param>
		protected AbstractWordReader(LinearLetterReader lreader){
			this.lreader=lreader;
			this.cword="";
			this.wtype=WordType.Invalid;
		}
		//===========================================================
		//		fields
		//===========================================================
		/// <summary>
		/// 文字の読み取りに使用している LetterReader を取得します。
		/// </summary>
		public LinearLetterReader LetterReader{get{return lreader;}}
		/// <summary>
		/// 文字の読み取りに使用している LinearLetterReader を保持しています。
		/// </summary>
		protected LinearLetterReader lreader;
		/// <summary>
		/// 現在の語句を保持します。
		/// </summary>
		protected string cword;
		/// <summary>
		/// 現在の語句の内容を取得します。
		/// </summary>
		public string CurrentWord{get{return this.cword;}}
		/// <summary>
		/// 現在の語句の種類を保持します。
		/// </summary>
		protected WordType wtype;
		/// <summary>
		/// 現在の語句の種類を取得します。
		/// </summary>
		public WordType CurrentType{get{return this.wtype;}}
		//===========================================================
		//		reading methods
		//===========================================================
		/// <summary>
		/// 通常の文字・数字・アンダースコアから構成される識別子を読み取ります。
		/// </summary>
		protected void ReadIdentifier(){
			this.wtype=WordType.Identifier;
#if MACRO_WORDREADER
			[add][next]
			while([type].IsToken||[type].IsNumber||[is:_]){
				[add][next]
			}
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||lreader.CurrentLetter=='_'){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// 通常の文字・数字及び指定した記号から構成される識別子を読み取ります。
		/// </summary>
		/// <param name="acceptables">識別子に含める事が出来る文字を指定します。</param>
		protected void ReadIdentifier(params char[] acceptables){
			this.wtype=WordType.Identifier;
#if MACRO_WORDREADER
			[add][next]
			while(true){
				if([type].IsToken||[type].IsNumber||[is:_])goto add;
				for(int i=0;i<acceptables.Length;i++)if([letter]==acceptables[i])goto add;
				return;
			add:
				[add][next]
			}
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true){
				if(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||lreader.CurrentLetter=='_')goto add;
				for(int i=0;i<acceptables.Length;i++)if(this.lreader.CurrentLetter==acceptables[i])goto add;
				return;
			add:
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
			#endregion #OUT#

		}
	}
}