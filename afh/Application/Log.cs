using Rgx=System.Text.RegularExpressions;

namespace afh.Application{
	/// <summary>
	/// afh.Application.Log に表示するデータを管理します。
	/// </summary>
	public class Log{
		/// <summary>
		/// Log コンストラクタ
		/// </summary>
		/// <param name="name">このログに関連付ける名前を指定します。</param>
		public Log(string name){
			this.name=name;
		}

		/// <summary>
		/// Log のインスタンスを作成し、LogView の管理インスタンス LogView.Instance に登録します。
		/// </summary>
		/// <param name="name">新しく作成する Log の名前を指定します。</param>
		/// <returns>新しく作成した Log のインスタンスを返します。</returns>
		[System.Obsolete]
		public static Log CreateAndRegister(string name) {
			return LogView.Instance.CreateLog(name);
		}
		//===========================================================
		//		ログの名前
		//===========================================================
		private string name;
		/// <summary>
		/// この Log に関連付けられた名前を取得します。
		/// </summary>
		public string Name{
			get{return this.name;}
		}
		/// <summary>
		/// この Log インスタンスの名前を表す文字列を指定します。
		/// </summary>
		/// <returns>この Log インスタンスの名前を返します。</returns>
		public override string ToString(){return this.Name;}
		//===========================================================
		//		表示先テキストボックス
		//===========================================================
		private LogBox logbox;
		/// <summary>
		/// この Log の内容を表示する先の LogBox を取得亦は設定します。
		/// </summary>
		public LogBox LogBox{
			get{return this.logbox;}
			set{
				if(this.logbox==value)return;
				if(this.logbox!=null)clear_logbox();
				this.logbox=value;
				this.reset_logbox();
			}
		}
		private void reset_logbox() {
			if(this.logbox!=null&&!this.logbox.IsDisposed) {
				if(this.logbox.InvokeRequired) {
					this.logbox.Invoke(new VoidCB(this.reset_logbox));
				}else{
					this.logbox.Text=this.data.ToString();
					this.logbox.Select(this.data.Length,0);
					this.logbox.ScrollToCaret();
				}
			}
		}

		private System.Text.StringBuilder data=new System.Text.StringBuilder();
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
		public void AddIndent() {
			indent+="    ";
		}
		/// <summary>
		/// インデントを取り除きます。
		/// </summary>
		public void RemoveIndent() {
			if(indent.Length>=4)indent=indent.Substring(4);
		}
		//-----------------------------------------------------------
		//	WRITELINE
		//-----------------------------------------------------------
		/// <summary>
		/// 改行を書き込みます。
		/// </summary>
		public void WriteLine() {
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 行を書き加えます。
		/// </summary>
		/// <param name="message">書き加える行の内容を指定します。</param>
		public void WriteLine(string message) {
			if(this.indent.Length>0) {
				message=message.Replace("\r\n","\r\n"+this.indent);
				this.AppendText(this.indent);
			}
			this.AppendText(message);
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 行を書き加えます。
		/// </summary>
		/// <param name="obj">書き込むオブジェクトを指定します。</param>
		public void WriteLine(object obj){
			if(obj==null)obj="<null>";
			this.WriteLine(obj.ToString());
		}
		/// <summary>
		/// 書式文字列を使用して行を書き加えます。
		/// </summary>
		/// <param name="format">書き加える内容の書式を指定します。</param>
		/// <param name="args">出力する値を指定します。</param>
		public void WriteLine(string format,params object[] args) {
			if(this.indent.Length>0) {
				format=format.Replace("\n","\n"+this.indent);
				this.AppendText(this.indent);
			}
			this.AppendText(string.Format(format,args));
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 書式文字列を使用して行を書き加えます。
		/// </summary>
		/// <param name="format">書き加える内容の書式を指定します。</param>
		/// <param name="arg0">出力する一番目の値を指定します。</param>
		public void WriteLine(string format,object arg0) {
			if(this.indent.Length>0) {
				format=format.Replace("\n","\n"+this.indent);
				this.AppendText(this.indent);
			}
			this.AppendText(string.Format(format,arg0));
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 書式文字列を使用して行を書き加えます。
		/// </summary>
		/// <param name="format">書き加える内容の書式を指定します。</param>
		/// <param name="arg0">出力する一番目の値を指定します。</param>
		/// <param name="arg1">出力する二番目の値を指定します。</param>
		public void WriteLine(string format,object arg0,object arg1) {
			if(this.indent.Length>0) {
				format=format.Replace("\n","\n"+this.indent);
				this.AppendText(this.indent);
			}
			this.AppendText(string.Format(format,arg0,arg1));
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// 書式文字列を使用して行を書き加えます。
		/// </summary>
		/// <param name="format">書き加える内容の書式を指定します。</param>
		/// <param name="arg0">出力する一番目の値を指定します。</param>
		/// <param name="arg1">出力する二番目の値を指定します。</param>
		/// <param name="arg2">出力する三番目の値を指定します。</param>
		public void WriteLine(string format,object arg0,object arg1,object arg2) {
			if(this.indent.Length>0) {
				format=format.Replace("\n","\n"+this.indent);
				this.AppendText(this.indent);
			}
			this.AppendText(string.Format(format,arg0,arg1,arg2));
			this.AppendText("\r\n");
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
		public void WriteError(string message) {
			this.writeSource();
			this.WriteLine("    "+message);
		}
		/// <summary>
		/// 例外が発生した事を通知し、例外の詳細を出力します。
		/// </summary>
		/// <param name="e">発生した例外を指定します。</param>
		public void WriteError(System.Exception e) {
			this.Lock();
			this.writeSource();
			this.AddIndent();
			this.writeError(e);
			if(e.InnerException!=null) WriteError(e.InnerException);
			this.RemoveIndent();
			this.Unlock();
		}
		/// <summary>
		/// 例外が発生した事を通知し、例外の詳細を出力します。
		/// </summary>
		/// <param name="e">発生した例外を指定します。</param>
		/// <param name="message">追加のメッセージを指定します。</param>
		public void WriteError(System.Exception e,string message) {
			this.Lock();
			this.writeSource();
			this.AddIndent();
			this.WriteLine(message);
			this.writeError(e);
			if(e.InnerException!=null) WriteError(e.InnerException);
			this.RemoveIndent();
			this.Unlock();
		}
		private void writeSource() {
			System.Diagnostics.StackTrace trace=new System.Diagnostics.StackTrace(false);
			if(trace.FrameCount>2) {
				System.Reflection.MethodBase m=trace.GetFrame(2).GetMethod();
				string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
				this.WriteLine("<"+dll+"> "+afh.Types.CSharpName(m));
			}
		}
		private void writeError(System.Exception e) {
			this.WriteVar("ExceptionType",e.GetType().ToString());
			this.WriteVar("Message",e.Message);
			this.WriteVar("Source",e.Source);
			if(e.HelpLink!="") this.WriteVar("HelpLink",e.HelpLink);
			this.WriteLine("StackTrace:");
			this.AddIndent();
			{
				this.WriteLine(e.StackTrace);
			} this.RemoveIndent();
		}
		/// <summary>
		/// 変数の名前と値の組み合わせを出力します。
		/// </summary>
		/// <param name="name">変数の名前に対応する文字列を指定します。</param>
		/// <param name="value">変数の値に対応する文字列を指定します。</param>
		public void WriteVar(string name,string value) {
			this.WriteLine(name+":\t"+value);
		}
		/// <summary>
		/// 変数の名前と値の組み合わせを出力します。
		/// </summary>
		/// <param name="name">変数の名前に対応する文字列を指定します。</param>
		/// <param name="value">変数の値を指定します。</param>
		public void WriteVar(string name,object value) {
			this.WriteVar(name,value==null?"null":value.ToString());
		}
		/// <summary>
		/// 変数の名前と値の組み合わせを出力します。
		/// </summary>
		/// <param name="name">変数の名前に対応する文字列を指定します。</param>
		/// <param name="value">変数の値に対応する文字列を指定します。</param>
		/// <param name="skipIfEmpty">value が null か空文字列の場合に出力しない事を指定します。
		/// true を指定した場合には value が null 亦は空文字列の際に出力しません。
		/// false を指定した場合には value がどの様な値であっても文字列を出力します。</param>
		public void WriteVar(string name,string value,bool skipIfEmpty) {
			if(skipIfEmpty&&(value==null||value==""))return;
			this.WriteVar(name,value);
		}
		//-----------------------------------------------------------
		//	DUMP
		//-----------------------------------------------------------
		/// <summary>
		/// byte[] の内容を書き出します。
		/// </summary>
		/// <param name="data">書き出す配列を指定します。</param>
		public void DumpBytes(byte[] data){
			this.Lock();
			this.AppendText(this.indent);
			this.AppendText("byte[] Length: ");
			this.AppendText(data.Length.ToString());
			for(int i=0;i<data.Length;i++){
				if(i%16==0){
					this.AppendText(this.indent);
					this.AppendText("\r\n    ");
				}
				this.AppendText(data[i].ToString("X2"));
				this.AppendText(" ");
			}
			this.AppendText("\r\n");
			this.Unlock();
		}
		/// <summary>
		/// 文字列の内容を行番号付きで出力します。
		/// </summary>
		/// <param name="data">文字列</param>
		public void DumpString(string data){
			Rgx::MatchCollection mc=afh.Text.Regexs.Rx_NewLine.Matches(data);
			string f="D"+System.Math.Max(mc.Count.ToString().Length,4).ToString();

			this.Lock();
			int i=0;int prev=0;
			foreach(Rgx::Match m in mc){
				this.AppendText(this.indent);
				this.AppendText(i++.ToString(f));
				this.AppendText("|");
				this.AppendText(data.Substring(prev,m.Index-prev));
				this.AppendText("\r\n");
				prev=m.Index+m.Length;
			}
			this.AppendText(this.indent);
			this.AppendText(i++.ToString(f));
			this.AppendText("|");
			this.AppendText(data.Substring(prev));
			this.AppendText("\r\n");

			this.Unlock();
		}
		//===========================================================
		//		変更の通知
		//===========================================================
		private int locked=0;
		private System.Text.StringBuilder sbuffer;
		/// <summary>
		/// 内容を初期化します。
		/// </summary>
		public void Clear(){
			this.clear_logbox();
			if(this.locked>0)this.sbuffer=new System.Text.StringBuilder();
			this.data=new System.Text.StringBuilder();
		}
		private void clear_logbox(){
			if(this.logbox!=null&&!this.logbox.IsDisposed){
				if(this.logbox.InvokeRequired){
					this.logbox.BeginInvoke((afh.VoidCB)this.logbox.Clear);
				}else this.logbox.Clear();
			}
		}
		/// <summary>
		/// Log に文字列を追加します。
		/// </summary>
		/// <param name="text">追加する文字列を指定して下さい。</param>
		public void AppendText(string text) {
			if(this.locked==0){
				if(this.logbox!=null&&!this.logbox.IsDisposed){
					if(this.logbox.InvokeRequired){
						this.logbox.BeginInvoke((afh.CallBack<string>)this.logbox.AppendText,text);
					}else this.logbox.AppendText(text);
				}
			}else{
				this.sbuffer.Append(text);
			}
			this.data.Append(text);
		}
		/// <summary>
		/// 内容に変更があっても、表示の更新及びChanged イベントが発生しない様にします。
		/// </summary>
		public System.IDisposable Lock(){
			if(this.locked==0){
				this.sbuffer=new System.Text.StringBuilder();
			}
			this.locked++;
			return lock_rel??(lock_rel=new LockReleaser(this));
		}
		LockReleaser lock_rel=null;
		private class LockReleaser:System.IDisposable{
			Log parent;
			public LockReleaser(Log parent){
				this.parent=parent;
			}
			public void Dispose(){
				parent.Unlock();
			}
		}
		/// <summary>
		/// Locked で設定した状態を元に戻し
		/// 内容に変更があった時に Changed イベントが発生する様にします。
		/// (Locked を行った回数と同じ回数だけ Unlocked を行わないと状態は元に戻りません。)
		/// 元に戻った時にはロックされていた間に内容に変更があったかどうかに拘わらず Changed イベントが発生します。
		/// </summary>
		public void Unlock() {
			if(this.locked==0)return;
			this.locked--;
			if(this.locked==0){
				if(this.logbox!=null&&!this.logbox.IsDisposed)
					this.logbox.AppendText(this.sbuffer.ToString());
				this.sbuffer=null;
				this.OnChanged();
			}
		}
		/// <summary>
		/// 内容に変更があった時に発生します。
		/// </summary>
		public event afh.VoidEH Changed;
		/// <summary>
		/// Changed イベントを発生させます。
		/// </summary>
		protected virtual void OnChanged() {
			if(this.locked>0||this.Changed==null) return;
			this.Changed(this);
		}
		//===========================================================
		//		アセンブリ内出力用のインスタンス
		//===========================================================
		internal static readonly Log AfhOut=LogView.Instance.CreateLog("<afh.dll>");
	}
}