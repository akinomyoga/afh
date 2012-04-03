using Generic=System.Collections.Generic;
namespace afh.Parse{
	/// <summary>
	/// 解析中に発生したエラーを記述します。
	/// </summary>
	public class AnalyzeError{
		/// <summary>
		/// エラーの内容を説明する文字列を保持します。
		/// </summary>
		public string message;
		/// <summary>
		/// 解析中に発生したエラーを記述する AnalyzeError のインスタンスを作成します。
		/// </summary>
		/// <param name="message">エラーの内容に関する説明を指定します。</param>
		public AnalyzeError(string message){this.message=message;}
	}
	/// <summary>
	/// 文字を一つずつ読み取るクラスを実装するインタフェースです。
	/// </summary>
	public interface ILetterReader{
		/// <summary>
		/// 現在の文字を取得します。
		/// </summary>
		char CurrentLetter{get;}
		/// <summary>
		/// 現在の文字の種類を記述する LetterType を取得します。
		/// </summary>
		LetterType CurrentType{get;}
		/// <summary>
		/// 次の文字に移動します。
		/// </summary>
		/// <returns>次の文字に進む事が出来た場合に true を返します。文字がもう無かった場合には false を返します。</returns>
		bool MoveNext();

		/// <summary>
		/// 現在の位置を指定したオブジェクトに関連付けて記録します。
		/// </summary>
		/// <param name="key">位置情報を管理する為の名札となるべきオブジェクトを指定します。</param>
		void StoreCurrentPos(object key);
		/// <summary>
		/// 指定したオブジェクトに関連付けられた位置に移動します。
		/// </summary>
		/// <param name="key">移動先の位置を参照するオブジェクトを指定します。</param>
		void MoveToPos(object key);
		/// <summary>
		/// 指定したオブジェクトに関連付けられた位置に移動します。
		/// オブジェクトに関連付けられた位置情報は、移動後に消されます。
		/// </summary>
		/// <param name="key">移動先の位置を参照するオブジェクトを指定します。</param>
		void ReturnToPos(object key);
		/// <summary>
		/// 現在保持している位置情報をすべて消去します。
		/// </summary>
		void ClearPositions();
		/// <summary>
		/// 位置情報をコピーします。
		/// </summary>
		/// <param name="sourceKey">コピー元の位置情報識別オブジェクトを指定します。</param>
		/// <param name="targetKey">コピー先の位置情報識別オブジェクトを指定します。</param>
		void CopyPosition(object sourceKey,object targetKey);

		/// <summary>
		/// 解析中のエラーを設定します。
		/// </summary>
		/// <param name="message">エラーの内容を説明する文字列を指定します。</param>
		/// <param name="start">エラーの原因となった部分文字列の開始位置に関連付けられたオブジェクトを指定します。
		/// null を指定した場合には現在位置と見做されます。</param>
		/// <param name="end">エラーの原因となった部分文字列の末端位置に関連付けられたオブジェクトを指定します。
		/// null を指定した場合には現在位置と見做されます。</param>
		void SetError(string message,object start,object end);
	}
	/// <summary>
	/// 文字列を単一行として扱う文字読み取りオブジェクトを表現します。
	/// 文字列に改行を含める事は可能ですが、行番号・列番号によるアクセスは出来ません。
	/// </summary>
	public interface ILinearLetterReader:ILetterReader{
		/// <summary>
		/// 解析対象の文字列を取得亦は設定します。
		/// 文字列を設定した場合には、現在の解析位置・保持している位置情報は初期化されます。
		/// </summary>
		string Text{get;set;}
		/// <summary>
		/// 解析対象の文字列と、解析開始位置を設定します。
		/// </summary>
		/// <param name="text">解析対象の文字列を指定します。</param>
		/// <param name="startIndex">解析を開始する位置を指定します。</param>
		void SetText(string text,int startIndex);
		/// <summary>
		/// 現在の位置を取得亦は設定します。
		/// </summary>
		int Index{get;set;}
	}
	/// <summary>
	/// 線形文字読み取り器です。
	/// 一つの string インスタンスの内容を一行の文字列として読み取ります。
	/// (改行コードを読まないという意味ではなく、文字に「何文字目」でアクセスするという意味です。
	/// 「何行何列目」の文字というアクセスの仕方はしません。)
	/// </summary>
	public class LinearLetterReader:ILinearLetterReader{
		/// <summary>
		/// 指定した文字列を使用して LinearLetterReader を初期化します。
		/// </summary>
		/// <param name="text">読み込む対象の文字列を指定します。</param>
		public LinearLetterReader(string text){this.Text=text;}
		/// <summary>
		/// 空文字列を使用して LinearLetterReader を初期化します。
		/// </summary>
		public LinearLetterReader():this(""){}
		//=================================================
		//		位置の記録に関する部分
		//=================================================
		private Generic::Dictionary<object,int> pos
			=new System.Collections.Generic.Dictionary<object,int>();
		/// <summary>
		/// 現在の位置を記録します。
		/// </summary>
		/// <param name="key">
		/// 位置を識別する為のオブジェクトを指定します。
		/// 0 は単語の始まりを記録するのに使用します。
		/// </param>
		public void StoreCurrentPos(object key){this.pos[key]=this.index;}
		/// <summary>
		/// 現在保持している文字情報を全て破棄します。
		/// </summary>
		public void ClearPositions(){this.pos.Clear();}
		/// <summary>
		/// 指定した位置に現在の位置を移動します。
		/// </summary>
		/// <param name="key">移動先の位置を識別する為のオブジェクトを指定します。</param>
		public void MoveToPos(object key){
			if(!this.pos.ContainsKey(key))
				throw new System.ArgumentException("key","指定した key に対応する位置情報は存在しません。");
			this.Index=this.pos[key];
		}
		/// <summary>
		/// 指定した位置に現在の位置を移動し、移動した先の位置情報を削除します。
		/// </summary>
		/// <param name="key">移動先の位置を識別する為のオブジェクトを指定します。</param>
		public void ReturnToPos(object key){
			if(!this.pos.ContainsKey(key))
				throw new System.ArgumentException("key","指定した key に対応する位置情報は存在しません。");
			this.Index=this.pos[key];
			this.pos.Remove(key);
		}
		/// <summary>
		/// 現在位置が末端に達しているか否かを取得します。
		/// </summary>
		public bool IsEndOfText{get{return this.index>=this.length;}}
		/// <summary>
		/// 位置情報をコピーします。
		/// </summary>
		/// <param name="sourceKey">コピー元の位置情報識別オブジェクトを指定します。</param>
		/// <param name="targetKey">コピー先の位置情報識別オブジェクトを指定します。</param>
		public void CopyPosition(object sourceKey,object targetKey){
			if(!this.pos.ContainsKey(sourceKey))
				throw new System.ArgumentException("sourceKey","指定した key に対応する位置情報は存在しません。");
			this.pos[targetKey]=this.pos[sourceKey];
		}
		//=================================================
		//		位置の記録に関する部分
		//=================================================
		/// <summary>
		/// LinearLetterReader に於いて部分文字列の範囲を指し示すのに使用するクラスです。
		/// </summary>
		public class TextRange:System.IComparable<TextRange>{
			/// <summary>
			/// 部分文字列の開始位置を保持します。
			/// </summary>
			public int start;
			/// <summary>
			/// 部分文字列の末端位置を保持します。
			/// このフィールド指定される文字は含みません。
			/// </summary>
			public int end;
			/// <summary>
			/// TextRange のインスタンスを作成します。
			/// </summary>
			/// <param name="start">部分文字列の開始位置を指定します。</param>
			/// <param name="end">部分文字列の終端位置を指定します。これによって指定される文字自体は含みません。</param>
			public TextRange(int start,int end){
				this.start=start;
				this.end=end;
			}
			/// <summary>
			/// どちらの範囲の方が先に来るかの順序を取得します。
			/// </summary>
			/// <param name="other">比較対象のインスタンスを指定します。</param>
			/// <returns>
			/// このインスタンスの方が指定したインスタンスよりも始めに近い所に位置するならば正の値を返します。
			/// このインスタンスの方が指定したインスタンスよりも末端に近い所に位置するならば負の値を返します。
			/// 両者の表現する部分文字列の範囲が完全に一致するならば 0 を返します。
			/// </returns>
			public int CompareTo(TextRange other){
				int r=this.start-other.end;
				return r!=0?r:this.end-other.end;
			}
		}
		/// <summary>
		/// 解析中に発生したエラーを纏めて保持します。
		/// </summary>
		public Generic::SortedList<TextRange,AnalyzeError> errors
			=new System.Collections.Generic.SortedList<TextRange,AnalyzeError>();
		/// <summary>
		/// エラーを設定します。
		/// </summary>
		/// <param name="message">エラーの内容を説明する文字列を指定します。</param>
		/// <param name="start">
		/// エラーの開始位置識別子を指定します。
		/// 詳細は CreateTextRange メソッドの start を参照して下さい。
		/// </param>
		/// <param name="end">
		/// エラーの終了位置識別子を指定します。
		/// 詳細は CreateTextRange メソッドの end を参照して下さい。
		/// </param>
		public void SetError(string message,object start,object end){
			this.errors[this.CreateTextRange(start,end)]=new AnalyzeError(message);
		}
		/// <summary>
		/// 現在までに登録されたエラーを列挙します。
		/// </summary>
		/// <returns>エラーの列挙子を返します。</returns>
		public Generic::IEnumerable<Generic::KeyValuePair<TextRange,AnalyzeError>> EnumErrors(){
			return this.errors;
		}
		//=================================================
		//		現在参照位置の管理部分
		//=================================================
		/// <summary>
		/// 次の文字を読み取ります。
		/// </summary>
		/// <returns>無事に次の文字を読み取る事が出来た場合に true を返します。
		/// 文字列の末端に達し次の文字がない場合には false を返します。</returns>
		public bool MoveNext(){
			this.Index++;
			return this.index<this.length;
		}
		/// <summary>
		/// 現在参照している位置を取得亦は設定します。
		/// </summary>
		public int Index{
			get{return this.index;}
			set{
				this.index=value;
				if(this.index<this.length){
					this.ltype=LetterType.GetLetterType(this.text[this.index]);
				}else this.ltype=LetterType.Invalid;
			}
		}
		private int index;
		private int length;
		/// <summary>
		/// 文字列の範囲を指定します。
		/// </summary>
		/// <param name="start">
		/// 文字列範囲の開始文字の位置情報を参照する為の識別オブジェクトを指定します。
		/// これは、StoreCurrentPos に key として渡した物です。
		/// null を指定した場合には現在の位置を文字列範囲の開始文字とします。
		/// </param>
		/// <param name="end">
		/// 文字列範囲の終了文字の位置情報を参照する為の識別オブジェクトを指定します。
		/// これは、StoreCurrentPos に key として渡した物です。
		/// null を指定した場合には現在の位置を文字列範囲の終了文字とします。
		/// </param>
		public TextRange CreateTextRange(object start,object end){
			int s=
				start!=null&&this.pos.ContainsKey(start)
				?this.pos[start]
				:this.index;
			int e=
				end!=null&&this.pos.ContainsKey(end)
				?this.pos[end]
				:this.index;
			if(e<s){int c=e;e=s;s=c;}
			return new TextRange(s,e);
		}
		//=================================================
		//		文字列データの管理部分
		//=================================================
		/// <summary>
		/// 解析対象の文字列を取得亦は設定します。
		/// </summary>
		public string Text{
			get{return this.text;}
			set{
				this.text=value;
				this.length=value.Length;
				this.Index=0;
				this.ClearPositions();
			}
		}
		private string text;
		/// <summary>
		/// 解析対象の文字列と解析開始位置を纏めて設定します。
		/// </summary>
		/// <param name="text">新しい解析対象の文字列を指定します。</param>
		/// <param name="startIndex">解析開始位置を指定します。</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// startIndex が text.Length 以上である場合に発生します。解析開始位置が指定した文字列の末端を越えた所に位置している為です。
		/// </exception>
		public void SetText(string text,int startIndex){
			this.Text=text;
			// ↓ これがあると空白文字列を設定出来ない。
			//if(startIndex>=this.length)
			//	throw new System.ArgumentOutOfRangeException("startIndex","指定した解析開始位置は文字列の末端を越えています。");
			this.Index=startIndex;
		}
		//=================================================
		//		現在の文字に関する情報
		//=================================================
		/// <summary>
		/// 現在の文字を取得します。
		/// </summary>
		public char CurrentLetter{
			get{return this.text[this.index];}
		}
		private LetterType ltype;
		/// <summary>
		/// 現在の文字の種類に関する情報を保持する LetterType を取得します。
		/// </summary>
		public LetterType CurrentType{
			get{return this.ltype;}
		}
		//=================================================
		//	LinearLetterReader 独自の実装
		//		ILetterReader には採用されていない
		//		将来的に採用されるかも知れない。
		//=================================================
		/// <summary>
		/// 現在の位置の次にある文字を読み取ります。
		/// </summary>
		/// <param name="ch">読み取った文字を返します。</param>
		/// <returns>現在の位置が終端に達している場合には false を返します。
		/// それ以外の場合には次の文字を読み取って true を返します。</returns>
		public bool PeekChar(out char ch){
			if(this.index+1<this.text.Length){
				ch=this.text[this.index+1];
				return true;
			}else{
				ch='\0';
				return false;
			}
		}
	}
}