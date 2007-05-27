namespace Tester.Threading{
	[TestFunctionAttribute("BeginInvoke-EndInvoke の実験","BeginInvoke EndInvoke の実験")]
	public class InvokingTest:TestFunction{
		public override string Exec(){
			this.completed=false;
			this.ExecNext("test",5);
			while(!this.completed)System.Threading.Thread.Sleep(200);
			return this.outstr;
		}

		private bool completed;

		private bool ExecNext(string msg,int count){
			//--実行権の取得
			this.WriteLine("Borrow");
			TestFunction dlg=new TestFunction(this.testFunction);
			TestThread dlg2=new TestThread(this.Exec_thread);
			dlg2.BeginInvoke(
				dlg,msg,count,
				new System.AsyncCallback(this.Exec_callback),
				new object[]{dlg2,msg}//←Exec_callback() の ar.AsyncState に入る
				);
			return true;
		}
		private void Exec_thread(TestFunction dlg,string msg,int count){
			System.Threading.Thread th=System.Threading.Thread.CurrentThread;
			if(th.Name==""||true){
				th.IsBackground=true;
				th.Name="ConnectQueue Thread";
			}
			if(count>0)System.Threading.Thread.Sleep(count*100);
			this.WriteLine("Exec_thread: before Execute");
			try{
				dlg(msg,count);
			}catch{
				this.WriteLine("Error: <while> Executing testFucntion");
			}
		}
		private void Exec_callback(System.IAsyncResult ar){
			this.WriteLine("Exec_callback: before cast");
			object[] array=(object[])ar.AsyncState;
			TestThread dlg2=(TestThread)array[0];
			string c=(string)array[1];
			try{
				this.WriteLine("Exec_callback: before EndInvoke");
				dlg2.EndInvoke(ar);
				this.WriteLine("Exec_callback: after EndInvoke");
			}catch(System.Exception e){
				//DEBUG:
				this.WriteLine("caught Error Invoke:"+e.GetType().ToString()+"\n\t"+e.Message);
				ErrorView view=new ErrorView();
				view.AddException(e);
				view.ShowDialog();
			}
			//--実行権の返上
			this.WriteLine("Return");
			this.completed=true;
		}
		private delegate void TestThread(TestFunction dlg,string message,int count);
		private delegate void TestFunction(string message,int count);
		private void testFunction(string message,int count){
			while(--count>=0)this.WriteLine("testFunction: "+message);
		}
	}
	[TestFunctionAttribute("lock された object へのアクセス"
		 ,@"lock されたオブジェクトに lock statement の外側からアクセスできるのかどうかを確認します。
一回目は二つのスレッドから一つのオブジェクトを lock をして実行します。
二回目は片方のスレッドで lock して片方のスレッドから lock の外でそのオブジェクトにアクセスします"
		 )]
	public class LockTest:TestFunction{
		public override string Exec(){
			this.outstr="=====一回目=====\n";
			System.Threading.ThreadStart dlg=new System.Threading.ThreadStart(this.LockFunc);
			System.Threading.Thread th=new System.Threading.Thread(dlg);
			th.Start();
			lock(this.lockObj){
				for(int i=0;i<10;i++){
					this.WriteLine("Exec: "+i.ToString());
					System.Threading.Thread.Sleep(50);
				}
			}
			th.Join();
			this.WriteLine("=====二回目=====");
			th=new System.Threading.Thread(dlg);
			th.Start();
			string x="";
			for(int i=0;i<10;i++){
				x+=this.lockObj.ToString();
				this.WriteLine("Exec: "+i.ToString());
				System.Threading.Thread.Sleep(50);
			}
			th.Join();
			return this.outstr;
		}
		private object lockObj=new object();
		private void LockFunc(){
			lock(this.lockObj){
				for(int i=0;i<10;i++){
					this.WriteLine("LockFunc: "+i.ToString());
					System.Threading.Thread.Sleep(50);
				}
			}
		}
	}

}