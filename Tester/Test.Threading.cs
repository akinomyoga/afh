namespace Tester.Threading{
	[TestFunctionAttribute("BeginInvoke-EndInvoke �̎���","BeginInvoke EndInvoke �̎���")]
	public class InvokingTest:TestFunction{
		public override string Exec(){
			this.completed=false;
			this.ExecNext("test",5);
			while(!this.completed)System.Threading.Thread.Sleep(200);
			return this.outstr;
		}

		private bool completed;

		private bool ExecNext(string msg,int count){
			//--���s���̎擾
			this.WriteLine("Borrow");
			TestFunction dlg=new TestFunction(this.testFunction);
			TestThread dlg2=new TestThread(this.Exec_thread);
			dlg2.BeginInvoke(
				dlg,msg,count,
				new System.AsyncCallback(this.Exec_callback),
				new object[]{dlg2,msg}//��Exec_callback() �� ar.AsyncState �ɓ���
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
			//--���s���̕ԏ�
			this.WriteLine("Return");
			this.completed=true;
		}
		private delegate void TestThread(TestFunction dlg,string message,int count);
		private delegate void TestFunction(string message,int count);
		private void testFunction(string message,int count){
			while(--count>=0)this.WriteLine("testFunction: "+message);
		}
	}
	[TestFunctionAttribute("lock ���ꂽ object �ւ̃A�N�Z�X"
		 ,@"lock ���ꂽ�I�u�W�F�N�g�� lock statement �̊O������A�N�Z�X�ł���̂��ǂ������m�F���܂��B
���ڂ͓�̃X���b�h�����̃I�u�W�F�N�g�� lock �����Ď��s���܂��B
���ڂ͕Е��̃X���b�h�� lock ���ĕЕ��̃X���b�h���� lock �̊O�ł��̃I�u�W�F�N�g�ɃA�N�Z�X���܂�"
		 )]
	public class LockTest:TestFunction{
		public override string Exec(){
			this.outstr="=====����=====\n";
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
			this.WriteLine("=====����=====");
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