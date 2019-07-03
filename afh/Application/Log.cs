using Rgx=System.Text.RegularExpressions;

namespace afh.Application{
	/// <summary>
	/// afh.Application.Log �ɕ\������f�[�^���Ǘ����܂��B
	/// </summary>
	public class Log{
		/// <summary>
		/// Log �R���X�g���N�^
		/// </summary>
		/// <param name="name">���̃��O�Ɋ֘A�t���閼�O���w�肵�܂��B</param>
		public Log(string name){
			this.name=name;
		}

		/// <summary>
		/// Log �̃C���X�^���X���쐬���ALogView �̊Ǘ��C���X�^���X LogView.Instance �ɓo�^���܂��B
		/// </summary>
		/// <param name="name">�V�����쐬���� Log �̖��O���w�肵�܂��B</param>
		/// <returns>�V�����쐬���� Log �̃C���X�^���X��Ԃ��܂��B</returns>
		[System.Obsolete]
		public static Log CreateAndRegister(string name) {
			return LogView.Instance.CreateLog(name);
		}
		//===========================================================
		//		���O�̖��O
		//===========================================================
		private string name;
		/// <summary>
		/// ���� Log �Ɋ֘A�t����ꂽ���O���擾���܂��B
		/// </summary>
		public string Name{
			get{return this.name;}
		}
		/// <summary>
		/// ���� Log �C���X�^���X�̖��O��\����������w�肵�܂��B
		/// </summary>
		/// <returns>���� Log �C���X�^���X�̖��O��Ԃ��܂��B</returns>
		public override string ToString(){return this.Name;}
		//===========================================================
		//		�\����e�L�X�g�{�b�N�X
		//===========================================================
		private LogBox logbox;
		/// <summary>
		/// ���� Log �̓��e��\�������� LogBox ���擾���͐ݒ肵�܂��B
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
		//		�����p���\�b�h
		//===========================================================
		//
		//	INDENT
		//
		private string indent="";
		/// <summary>
		/// �C���f���g�������܂��B
		/// </summary>
		public void AddIndent() {
			indent+="    ";
		}
		/// <summary>
		/// �C���f���g����菜���܂��B
		/// </summary>
		public void RemoveIndent() {
			if(indent.Length>=4)indent=indent.Substring(4);
		}
		//-----------------------------------------------------------
		//	WRITELINE
		//-----------------------------------------------------------
		/// <summary>
		/// ���s���������݂܂��B
		/// </summary>
		public void WriteLine() {
			this.AppendText("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// �s�����������܂��B
		/// </summary>
		/// <param name="message">����������s�̓��e���w�肵�܂��B</param>
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
		/// �s�����������܂��B
		/// </summary>
		/// <param name="obj">�������ރI�u�W�F�N�g���w�肵�܂��B</param>
		public void WriteLine(object obj){
			if(obj==null)obj="<null>";
			this.WriteLine(obj.ToString());
		}
		/// <summary>
		/// ������������g�p���čs�����������܂��B
		/// </summary>
		/// <param name="format">������������e�̏������w�肵�܂��B</param>
		/// <param name="args">�o�͂���l���w�肵�܂��B</param>
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
		/// ������������g�p���čs�����������܂��B
		/// </summary>
		/// <param name="format">������������e�̏������w�肵�܂��B</param>
		/// <param name="arg0">�o�͂����Ԗڂ̒l���w�肵�܂��B</param>
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
		/// ������������g�p���čs�����������܂��B
		/// </summary>
		/// <param name="format">������������e�̏������w�肵�܂��B</param>
		/// <param name="arg0">�o�͂����Ԗڂ̒l���w�肵�܂��B</param>
		/// <param name="arg1">�o�͂����Ԗڂ̒l���w�肵�܂��B</param>
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
		/// ������������g�p���čs�����������܂��B
		/// </summary>
		/// <param name="format">������������e�̏������w�肵�܂��B</param>
		/// <param name="arg0">�o�͂����Ԗڂ̒l���w�肵�܂��B</param>
		/// <param name="arg1">�o�͂����Ԗڂ̒l���w�肵�܂��B</param>
		/// <param name="arg2">�o�͂���O�Ԗڂ̒l���w�肵�܂��B</param>
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
		/// �G���[������������ʒm���܂��B
		/// ��O�Ƃ��ē�������A�[���ȏ󋵂ł͂Ȃ��ꍇ�Ɏg�p���鎖��z�肵�Ă��܂��B
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		public void WriteError(string message) {
			this.writeSource();
			this.WriteLine("    "+message);
		}
		/// <summary>
		/// ��O��������������ʒm���A��O�̏ڍׂ��o�͂��܂��B
		/// </summary>
		/// <param name="e">����������O���w�肵�܂��B</param>
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
		/// ��O��������������ʒm���A��O�̏ڍׂ��o�͂��܂��B
		/// </summary>
		/// <param name="e">����������O���w�肵�܂��B</param>
		/// <param name="message">�ǉ��̃��b�Z�[�W���w�肵�܂��B</param>
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
		/// �ϐ��̖��O�ƒl�̑g�ݍ��킹���o�͂��܂��B
		/// </summary>
		/// <param name="name">�ϐ��̖��O�ɑΉ����镶������w�肵�܂��B</param>
		/// <param name="value">�ϐ��̒l�ɑΉ����镶������w�肵�܂��B</param>
		public void WriteVar(string name,string value) {
			this.WriteLine(name+":\t"+value);
		}
		/// <summary>
		/// �ϐ��̖��O�ƒl�̑g�ݍ��킹���o�͂��܂��B
		/// </summary>
		/// <param name="name">�ϐ��̖��O�ɑΉ����镶������w�肵�܂��B</param>
		/// <param name="value">�ϐ��̒l���w�肵�܂��B</param>
		public void WriteVar(string name,object value) {
			this.WriteVar(name,value==null?"null":value.ToString());
		}
		/// <summary>
		/// �ϐ��̖��O�ƒl�̑g�ݍ��킹���o�͂��܂��B
		/// </summary>
		/// <param name="name">�ϐ��̖��O�ɑΉ����镶������w�肵�܂��B</param>
		/// <param name="value">�ϐ��̒l�ɑΉ����镶������w�肵�܂��B</param>
		/// <param name="skipIfEmpty">value �� null ���󕶎���̏ꍇ�ɏo�͂��Ȃ������w�肵�܂��B
		/// true ���w�肵���ꍇ�ɂ� value �� null ���͋󕶎���̍ۂɏo�͂��܂���B
		/// false ���w�肵���ꍇ�ɂ� value ���ǂ̗l�Ȓl�ł����Ă���������o�͂��܂��B</param>
		public void WriteVar(string name,string value,bool skipIfEmpty) {
			if(skipIfEmpty&&(value==null||value==""))return;
			this.WriteVar(name,value);
		}
		//-----------------------------------------------------------
		//	DUMP
		//-----------------------------------------------------------
		/// <summary>
		/// byte[] �̓��e�������o���܂��B
		/// </summary>
		/// <param name="data">�����o���z����w�肵�܂��B</param>
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
		/// ������̓��e���s�ԍ��t���ŏo�͂��܂��B
		/// </summary>
		/// <param name="data">������</param>
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
		//		�ύX�̒ʒm
		//===========================================================
		private int locked=0;
		private System.Text.StringBuilder sbuffer;
		/// <summary>
		/// ���e�����������܂��B
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
		/// Log �ɕ������ǉ����܂��B
		/// </summary>
		/// <param name="text">�ǉ����镶������w�肵�ĉ������B</param>
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
		/// ���e�ɕύX�������Ă��A�\���̍X�V�y��Changed �C�x���g���������Ȃ��l�ɂ��܂��B
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
		/// Locked �Őݒ肵����Ԃ����ɖ߂�
		/// ���e�ɕύX������������ Changed �C�x���g����������l�ɂ��܂��B
		/// (Locked ���s�����񐔂Ɠ����񐔂��� Unlocked ���s��Ȃ��Ə�Ԃ͌��ɖ߂�܂���B)
		/// ���ɖ߂������ɂ̓��b�N����Ă����Ԃɓ��e�ɕύX�����������ǂ����ɍS��炸 Changed �C�x���g���������܂��B
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
		/// ���e�ɕύX�����������ɔ������܂��B
		/// </summary>
		public event afh.VoidEH Changed;
		/// <summary>
		/// Changed �C�x���g�𔭐������܂��B
		/// </summary>
		protected virtual void OnChanged() {
			if(this.locked>0||this.Changed==null) return;
			this.Changed(this);
		}
		//===========================================================
		//		�A�Z���u�����o�͗p�̃C���X�^���X
		//===========================================================
		internal static readonly Log AfhOut=LogView.Instance.CreateLog("<afh.dll>");
	}
}