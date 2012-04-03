using Gen=System.Collections.Generic;
namespace afh.Collections{
	/// <summary>
	/// 廃止予定です。名前が PluralDictionary から DictionaryP に変わりました。
	/// </summary>
	/// <typeparam name="TKey">登録する時の見出しとなるインスタンスの型を指定します。</typeparam>
	/// <typeparam name="TVal">登録される値の型を指定します。</typeparam>
	[System.Obsolete("名前が PluralDictionary から DictionaryP に変わりました。")]
	public class PluralDictionary<TKey,TVal>:DictionaryP<TKey,TVal>{
		/// <summary>
		/// PluralDictionary のコンストラクタです。
		/// </summary>
		[System.Obsolete("名前が PluralDictionary から DictionaryP に変わりました。")]
		public PluralDictionary(){}
	}
	/// <summary>
	/// 指定した型への変換を提供します。
	/// </summary>
	/// <typeparam name="T">変換先の型を指定します。</typeparam>
	[System.Obsolete("afh.Converter を使用する実装に置き換える事を勧めます。")]
	public interface IConvertible<T>{
		/// <summary>
		/// このインスタンスを <typeparamref name="T"/> のインスタンスに変換します。
		/// </summary>
		/// <returns>変換後の値を返します。</returns>
		T ConvertTo();
	}
	/// <summary>
	/// 廃止予定です。afh.Converter を使用して下さい
	/// </summary>
	/// <typeparam name="TFrom">変換前の型を指定します。</typeparam>
	/// <typeparam name="TDest">変換後の型を指定します。</typeparam>
	/// <param name="value">変換前の値を指定します。</param>
	/// <returns>変換後の値を返します。</returns>
	[System.Obsolete("afh.Converter を使用して下さい")]
	public delegate TDest DlgConverter<TFrom,TDest>(TFrom value);
}

namespace afh{
	/// <summary>
	/// 文字列に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<string> を使用して下さい")]
	public delegate void StringEH(object sender,string value);
	/// <summary>
	/// sbyte に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<sbyte> を使用して下さい")]
	public delegate void SByteEH(object sender,sbyte value);
	/// <summary>
	/// short に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<short> を使用して下さい")]
	public delegate void Int16EH(object sender,short value);
	/// <summary>
	/// int に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<int> を使用して下さい")]
	public delegate void Int32EH(object sender,int value);
	/// <summary>
	/// long に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<long> を使用して下さい")]
	public delegate void Int64EH(object sender,long value);
	/// <summary>
	/// byte に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<byte> を使用して下さい")]
	public delegate void ByteEH(object sender,byte value);
	/// <summary>
	/// ushort に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<ushort> を使用して下さい")]
	public delegate void UInt16EH(object sender,ushort value);
	/// <summary>
	/// uint に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<uint> を使用して下さい")]
	public delegate void UInt32EH(object sender,uint value);
	/// <summary>
	/// ulong に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<ulong> を使用して下さい")]
	public delegate void UInt64EH(object sender,ulong value);
	/// <summary>
	/// float に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<float> を使用して下さい")]
	public delegate void SingleEH(object sender,float value);
	/// <summary>
	/// double に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<double> を使用して下さい")]
	public delegate void DoubleEH(object sender,double value);
	/// <summary>
	/// char に関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<char> を使用して下さい")]
	public delegate void CharEH(object sender,char value);
	/// <summary>
	/// 一般のオブジェクトに関するイベントを処理する為のデリゲートです。
	/// </summary>
	[System.Obsolete("afh.EventHandler<object> を使用して下さい")]
	public delegate void ObjectEH(object sender,object value);
}

namespace afh{
	/// <summary>
	/// 廃止されました。afh.Collections.Enumerable を使用して下さい。
	/// </summary>
	[System.Obsolete("afh.Collections.Enumerable")]
	public class Enumerable{
		/// <summary>
		/// 指定した System.Collections.IEnumerator を返す System.Collections.IEnumerable を作成します。
		/// </summary>
		/// <param name="enumerator">GetEnumerator で返す System.Collections.IEnumerator を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator){throw new System.NotImplementedException("廃止されました");}
		/// <summary>
		/// 値を選んで列挙する列挙子を返すインスタンスを作成します。
		/// </summary>
		/// <param name="enumerator">元の列挙子を指定します。</param>
		/// <param name="filter">特定の値を列挙するかしないかを判定する為の IFilter を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,IFilter filter){throw new System.NotImplementedException("廃止されました");}
		/// <summary>
		/// 特定の型の値を列挙する列挙子を返すインスタンスを作成します。
		/// </summary>
		/// <param name="enumerator">元の列挙子を指定します。</param>
		/// <param name="type">列挙する値の型を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,System.Type type){throw new System.NotImplementedException("廃止されました");}
		/// <summary>
		/// 列挙される値を事前に跳ねる働きをするオブジェクトを表します。
		/// </summary>
		[System.Obsolete]
		public interface IFilter{}
#if ENUMERATOR
		/// <summary>
		/// 指定した System.Collections.IEnumerator を返す System.Collections.IEnumerable を作成します。
		/// </summary>
		/// <param name="enumerator">GetEnumerator で返す System.Collections.IEnumerator を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator){this.@enum=enumerator;}
		/// <summary>
		/// 値を選んで列挙する列挙子を返すインスタンスを作成します。
		/// </summary>
		/// <param name="enumerator">元の列挙子を指定します。</param>
		/// <param name="filter">特定の値を列挙するかしないかを判定する為の IFilter を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,IFilter filter){
			this.@enum=new FiltEnumerator(enumerator,filter);
		}
		/// <summary>
		/// 特定の型の値を列挙する列挙子を返すインスタンスを作成します。
		/// </summary>
		/// <param name="enumerator">元の列挙子を指定します。</param>
		/// <param name="type">列挙する値の型を指定します。</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,System.Type type){
			this.@enum=new FiltEnumerator(enumerator,new TypeFilter(type));
		}
		//GENERICS:
		private sealed class TypeFilter:IFilter{
			private System.Type type;
			public TypeFilter(System.Type type){this.type=type;}
			bool IFilter.Filt(object obj){
				if(obj==null)return false;
				System.Type t=obj.GetType();
				return t.IsSubclassOf(this.type)||t.GetInterface(this.type.FullName)!=null;
			}
		}
		private sealed class FiltEnumerator:System.Collections.IEnumerator{
			private System.Collections.IEnumerator @enum;
			private IFilter filter;
			public FiltEnumerator(System.Collections.IEnumerator enumerator,IFilter filter){
				this.@enum=enumerator;
				this.filter=filter;
			}
			public object Current{get{return this.@enum.Current;}}
			public bool MoveNext(){
				while(this.@enum.MoveNext())if(this.filter.Filt(this.@enum.Current))return true;
				return false;
			}
			public void Reset(){this.@enum.Reset();}
		}
		/// <summary>
		/// 列挙される値を事前に跳ねる働きをするオブジェクトを表します。
		/// </summary>
		public interface IFilter{
			/// <summary>
			/// 指定した値を列挙するかしないかを判定します。
			/// </summary>
			/// <param name="obj">列挙の候補である値を指定します。</param>
			/// <returns>指定した値を列挙する場合に true を返します。列挙しない場合には false を返します。</returns>
			bool Filt(object obj);
		}
#endif
		/// <summary>
		/// 指定した型の物のみ列挙します。
		/// </summary>
		/// <typeparam name="T">列挙する型を指定します。</typeparam>
		/// <returns>指定した要素のみ列挙する列挙子を返します。</returns>
		[System.Obsolete("afh.Collections.Enumerable.EnumByType に移動しました。")]
		public static Gen::IEnumerable<T> EnumByType<T>(System.Collections.IEnumerable baseEnumerable){
			return afh.Collections.Enumerable.EnumByType<T>(baseEnumerable);
		}
	}
}
namespace afh.Application{
	/// <summary>
	/// System.Type の文字列表現に対する操作を提供します。
	/// 廃止されました…afh.Types を使用して下さい。
	/// </summary>
	[System.Obsolete("afh.Types を使用して下さい。")]
	public class Types {
		/// <summary>
		/// 型の静的な hash 値として使える byte を計算して返します。
		/// </summary>
		/// <param name="t">Hash 値を求めたい System.Type を指定します。</param>
		/// <returns>計算した hash byte を返します。</returns>
		public static byte GetHashByte(System.Type t) {
			int hash=t.FullName.GetHashCode();
			return (byte)(hash^hash>>8^hash>>16^hash>>24);
		}
		/// <summary>
		/// System.Type を表す文字列を取得します。
		/// C# の予約語に一致する型は予約語で表します。
		/// </summary>
		/// <param name="t">文字列に直す前の System.Type を指定します。</param>
		/// <returns>System.Type を文字列で表した物を取得します。</returns>
		public static string CSharpString(System.Type t) {
			return afh.Types.CSharpName(t);
		}
		/// <summary>
		/// FullName ではなくて、名前空間などを含まないその型自体の名前を取得します。
		/// </summary>
		/// <param name="fullname">
		/// System.Type.FullName 等によって取得できる型を表す文字列を指定します。
		/// 型の名前として不適切な物を指定した場合の結果については保証しません。
		/// </param>
		/// <returns>型自体の名前を返します。</returns>
		public static string GetTypeName(string fullname) {
			string x=System.IO.Path.GetExtension(fullname);
			if(x=="") x=fullname;
			int i=x.IndexOf("+");
			return i>=0?x.Substring(i+1):x;
		}
		/// <summary>
		/// パラメータのリストを文字列にして取得します。
		/// </summary>
		/// <param name="m">メソッドを表す System.Reflection.MethodBase を指定して下さい。</param>
		/// <returns>パラメータのリストを表す文字列を返します。</returns>
		public static string GetParameters(System.Reflection.MethodBase m) {
			return afh.Types.GetParameterList(m);
		}
	}

	//===============================================================
	//		old Log
	//===============================================================
#if old
	/// <summary>
	/// アプリケーション内で吐き出される様々なメッセージを管理します。
	/// </summary>
	public class Log2{
		private string name;
		/// <summary>
		/// この Log に関連付けられた名前を取得します。
		/// </summary>
		public string Name{
			get{return this.name;}
		}
		private System.Text.StringBuilder str;
		/// <summary>
		/// Log コンストラクタ
		/// </summary>
		/// <param name="name">このログに関連付ける名前を指定します。</param>
		private Log2(string name){
			this.name=name;
			this.str=new System.Text.StringBuilder();
		}
		/// <summary>
		/// 今迄の Log の内容を消去します。
		/// </summary>
		public void Clear(){
			this.str=new System.Text.StringBuilder();
		}
		/// <summary>
		/// 今迄に出力した文字列の内容を取得します。
		/// </summary>
		public string ContentString{
			get{return this.str.ToString();}
		}
		//===========================================================
		//		書込用メソッド
		//===========================================================
		//
		//	INDENT
		//
		private string indent="";
		/// <summary>
		/// インデントを加えます。
		/// </summary>
		public void AddIndent(){
			indent+="    ";
		}
		/// <summary>
		/// インデントを取り除きます。
		/// </summary>
		public void RemoveIndent(){
			if(indent.Length>=4)indent=indent.Substring(4);
		}
		//
		//	WRITELINE
		//
		/// <summary>
		/// 行を書き加えます。
		/// </summary>
		/// <param name="message">書き加える行の内容を指定します。</param>
		public void WriteLine(string message){
			if(this.indent.Length>0){
				message=message.Replace("\r\n","\r\n"+this.indent);
				this.str.Append(this.indent);
			}
			this.str.Append(message);
			this.str.Append("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 書式文字列を使用して行を書き加えます。
		/// </summary>
		/// <param name="format">書き加える内容の書式を指定します。</param>
		/// <param name="args">出力する値を指定します。</param>
		public void WriteLine(string format,params object[] args){
			if(this.indent.Length>0){
				format=format.Replace("\n","\n"+this.indent);
				this.str.Append(this.indent);
			}
			this.str.AppendFormat(format,args);
			this.str.Append("\r\n");
			this.OnChanged();
		}
		//
		//	WRITEERROR
		//
		/// <summary>
		/// エラーがあった事を通知します。
		/// 例外として投げる程、深刻な状況ではない場合に使用する事を想定しています。
		/// </summary>
		/// <param name="message">メッセージ</param>
		public void WriteError(string message){
			System.Diagnostics.StackTrace trace=new System.Diagnostics.StackTrace(false);
			if(trace.FrameCount>1){
				System.Reflection.MethodBase m=trace.GetFrame(1).GetMethod();
				string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
				string type=m.DeclaringType.FullName.Replace(".","::");
				this.WriteLine("<"+dll+"> "+type+"."+m.Name+"("+afh.Application.Types.GetParameters(m)+")");
			}
			this.WriteLine("    "+message);
		}
		/// <summary>
		/// 例外が発生した事を通知し、例外の詳細を出力します。
		/// </summary>
		/// <param name="e">発生した例外を指定します。</param>
		public void WriteError(System.Exception e){
			System.Diagnostics.StackTrace trace=new System.Diagnostics.StackTrace(false);
			if(trace.FrameCount>1){
				System.Reflection.MethodBase m=trace.GetFrame(1).GetMethod();
				string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
				string type=m.DeclaringType.FullName.Replace(".","::");
				this.WriteLine("<"+dll+"> "+type+"."+m.Name+"("+afh.Application.Types.GetParameters(m)+")");
			}
			this.AddIndent();{
				this.WriteVar("ExceptionType",e.GetType().ToString());
				this.WriteVar("Message",e.Message);
				this.WriteVar("Source",e.Source);
				if(e.HelpLink!="")this.WriteVar("HelpLink",e.HelpLink);
				this.WriteLine("StackTrace:");
				this.AddIndent();{
					this.WriteLine(e.StackTrace);
				}this.RemoveIndent();
			}this.RemoveIndent();
		}
		/// <summary>
		/// 変数の名前と値の組み合わせを出力します。
		/// </summary>
		/// <param name="name">変数の名前に対応する文字列を指定します。</param>
		/// <param name="value">変数の値に対応する文字列を指定します。</param>
		public void WriteVar(string name,string value){
			this.WriteLine(name+":\t"+value);
		}
		//===========================================================
		//		変更の通知
		//===========================================================
		private int locked=0;
		/// <summary>
		/// 内容に変更があっても、Changed イベントが発生しない様にします。
		/// </summary>
		public void Lock(){this.locked++;}
		/// <summary>
		/// Locked で設定した状態を元に戻し
		/// 内容に変更があった時に Changed イベントが発生する様にします。
		/// (Locked を行った回数と同じ回数だけ Unlocked を行わないと状態は元に戻りません。)
		/// 元に戻った時にはロックされていた間に内容に変更があったかどうかに拘わらず Changed イベントが発生します。
		/// </summary>
		public void Unlock(){
			if(this.locked==0)return;
			this.locked--;
			if(this.locked==0)this.OnChanged();
		}
		/// <summary>
		/// 内容に変更があった時に発生します。
		/// </summary>
		public event afh.VoidEH Changed;
		/// <summary>
		/// Changed イベントを発生させます。
		/// </summary>
		protected virtual void OnChanged(){
			if(this.locked>0||this.Changed==null)return;
			this.Changed(this);
		}
	}
#endif
}
