using Ser=System.Runtime.Serialization;

namespace afh{
	/// <summary>
	/// 単純なイベントを処理する為のデリゲートです。
	/// </summary>
	public delegate void VoidEH(object sender);

	/// <summary>
	/// 処理が終了した事を通知するだけのコールバックの為のデリゲートです。
	/// </summary>
	public delegate void VoidCB();
	/// <summary>
	/// 文字列に関するコールバックのデリゲートです。
	/// </summary>
	public delegate void StringCB(string result);
	/// <summary>
	/// 真偽値に関するコールバックのデリゲートです。
	/// </summary>
	public delegate void BoolCB(bool result);
	/// <summary>
	/// 一般のオブジェクトに関するコールバックのデリゲートです。
	/// </summary>
	public delegate void ObjectCB(object result);

	/// <summary>
	/// 或るオブジェクトを引数とするイベントの為のデリゲートです。
	/// </summary>
	/// <typeparam name="T">イベントの引数の型を表します。
	/// これは、必ずしも System.EventArgs を継承するものである必要はありません。</typeparam>
	/// <param name="sender">イベントの発生したオブジェクトを指定します。</param>
	/// <param name="args">イベントに関する情報を保持するイベント引数を指定します。</param>
	public delegate void EventHandler<T>(object sender,T args);
	/// <summary>
	/// 或るオブジェクトを引数とするイベントの為のデリゲートです。
	/// </summary>
	/// <typeparam name="TObj">イベントの発生するオブジェクトの型を指定します。</typeparam>
	/// <typeparam name="TArg">イベントの引数の型を指定します。
	/// これは、必ずしも System.EventArgs を継承するものである必要はありません。</typeparam>
	/// <param name="sender">イベントの発生したオブジェクトを指定します。</param>
	/// <param name="args">イベントに関する情報を保持するイベント引数を指定します。</param>
	public delegate void EventHandler<TObj,TArg>(TObj sender,TArg args);
	/// <summary>
	/// 或る値を返すコールバックの為のデリゲートです。
	/// </summary>
	/// <typeparam name="T">返す値の型を表します。</typeparam>
	/// <param name="value">返す値を指定します。</param>
	public delegate void CallBack<T>(T value);

	/// <summary>
	/// 数値に対する操作を提供します。
	/// </summary>
	public static partial class Math{}


	/// <summary>
	/// デバグに関する機能を提供します。
	/// </summary>
	public static class DebugUtils{
		/// <summary>
		/// 条件を確認し、条件が満たされていない場合には AssertionFailException を発生させます。
		/// </summary>
		/// <param name="cond">条件式を引数の中に指定します。</param>
		/// <param name="messages">条件が満たされなかった場合に表示されるメッセージを指定します。</param>
		[System.Diagnostics.Conditional("DEBUG")]
		[System.Diagnostics.DebuggerHidden]
		public static void Assert(bool cond,params string[] messages){
			if(!cond)
				throw new AssertionFailException(
					"Assertion Failed: "+afh.Text.TextUtils.Join(messages,"\r\n")
				);
		}
	}
	/// <summary>
	/// プログラム動作の上での想定が破れた際に発生する例外です。
	/// </summary>
	[System.Serializable]
	public class AssertionFailException:System.Exception{
		/// <summary>
		/// AssertionFailException の既定の初期化を行います。
		/// </summary>
		public AssertionFailException(){}
		/// <summary>
		/// 指定したメッセージを使用して AssertionFailException を初期化します。
		/// </summary>
		/// <param name="msg">破れた想定についての詳細な説明を指定します。</param>
		public AssertionFailException(string msg):base(msg){}
		/// <summary>
		/// 指定したメッセージを使用して AssertionFailException を初期化します。
		/// </summary>
		/// <param name="msg">破れた想定についての詳細な説明を指定します。</param>
		/// <param name="innerException">この例外を発生させる原因となった内部例外を指定します。</param>
		public AssertionFailException(string msg,System.Exception innerException)
			:base(msg,innerException){}
		/// <summary>
		/// シリアル化した AssertionFailException を復元します。
		/// </summary>
		/// <param name="info">シリアル化したデータを保持するインスタンスを指定します。</param>
		/// <param name="ctx">シリアル化の環境についての情報を指定します。</param>
		public AssertionFailException(Ser::SerializationInfo info,Ser::StreamingContext ctx)
			:base(info,ctx){}
	}
}
