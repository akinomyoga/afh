using System;
using Gen=System.Collections.Generic;
using CM=System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ref=System.Reflection;
using Emit=System.Reflection.Emit;

namespace afh.Tester{
	[afh.Configuration.RestoreProperties]
	public partial class Form1:Form{
		public Form1(){
			this.InitializeComponent();
			this.Initialize2();
			this.dllpath="";
			afh.Configuration.RestorePropertiesAttribute.SetupForm(this,true);
		}

		public Form1(string dllpath){
			this.InitializeComponent();
			this.Initialize2();
			this.DllPath=dllpath;
			afh.Configuration.RestorePropertiesAttribute.SetupForm(this,dllpath,true);
		}

		private void Initialize2(){
			afh.Application.LogView view=afh.Application.LogView.Instance;
			view.Dock=System.Windows.Forms.DockStyle.Fill;
			this.panelLog.Controls.Add(view);
			view.EnsureVisibleLog(Program.log);
		}

		private string dllpath;
		public string DllPath{
			get{return this.dllpath;}
			set{
				this.dllpath=value;
				Program.log.WriteLine("ファイルを読み込みます: {0}",this.dllpath);
				Program.log.Lock();
				try{
					Ref::Assembly asm=Ref::Assembly.LoadFile(this.dllpath);
					this.Text="UnitTest - "+System.IO.Path.GetFileName(asm.CodeBase);
					foreach(System.Type type in asm.GetTypes()){
						afh.Tester.TestTargetAttribute[] attrs
							=(afh.Tester.TestTargetAttribute[])type.GetCustomAttributes(typeof(afh.Tester.TestTargetAttribute),false);
						if(attrs.Length==0&&type.Name!="UnitTest")continue;
						ReadType(type);
					}
				}catch(System.Exception e){
					Program.log.WriteError(e,"アセンブリの読込中にエラーが発生しました...");
				}finally{
					Program.log.Unlock();
				}
			}
		}

		private void ReadType(System.Type type){
			const Ref::BindingFlags BF=Ref::BindingFlags.Static|Ref::BindingFlags.Public|Ref::BindingFlags.NonPublic;
			afh.Application.Log log=Program.log;

			log.WriteLine("ReadType: {0}",type.FullName);
			foreach(Ref::MethodInfo minfo in type.GetMethods(BF)){
				// 超いい加減引数チェック
				if(TestMethod.IsTestable(minfo)){
					TestMethod test=new TestMethod(minfo);
					test.WriteSummary(log);
					this.listBox1.Items.Add(test);
				}

				BenchMethodAttribute benchattr=BenchMethod.GetAttribute(minfo,log);
				if(benchattr!=null){
					BenchMethod bench=new BenchMethod(minfo,benchattr);
					this.listBox1.Items.Add(bench);
				}
			}
		}

		private void listBox1_SelectedIndexChanged(object sender,EventArgs e) {
			ITest test=this.listBox1.SelectedItem as ITest;
			if(test==null)return;
			this.label1.Text=test.Description;
		}

		private void listBox1_DoubleClick(object sender,EventArgs e) {
			ITest test=this.listBox1.SelectedItem as ITest;
			if(test==null)return;
			test.Execute(Program.log);
		}
	}

	public interface ITest{
		string Description{get;}
		void Execute(afh.Application.Log log);
	}

	/// <summary>
	/// Test を実行するクラスです。
	/// </summary>
	public class TestMethod:ITest{
		Ref::MethodInfo info;
		string description;

		public string Description{
			get{return this.description;}
		}
		public override string ToString() {
			return this.info.Name;
		}
		public void WriteSummary(afh.Application.Log log){
			log.WriteLine("Method: "+Types.GetMethodSignature(info));
			if(description!=""){
				log.AddIndent();
				log.WriteVar("Description",this.description);
				log.RemoveIndent();
			}
		}
		//===========================================================
		//		初期化
		//===========================================================
		public TestMethod(Ref::MethodInfo minfo){
			CM::DescriptionAttribute[] attrs
					=(CM::DescriptionAttribute[])minfo.GetCustomAttributes(typeof(CM::DescriptionAttribute),false);
			string desc=attrs.Length==0?"":attrs[0].Description;
			this.info=minfo;
			this.description=desc;
		}
		public static bool IsTestable(Ref::MethodInfo minfo){
			Ref::ParameterInfo[] pinfos=minfo.GetParameters();
			if(pinfos.Length>1)return false;
			if(pinfos.Length==1){
				return pinfos[0].ParameterType==typeof(afh.Application.Log);
			}
			return false;
		}
		//===========================================================
		//		実験
		//===========================================================
		const string STR_LINE="------------------------------------------------------------";
		public void Execute(afh.Application.Log log){
			// 前書き
			log.Lock();
			log.WriteLine();
			log.WriteLine(STR_LINE);
			log.WriteLine("\tTest を開始します。");
			log.WriteLine("\t\tMethodName:  {0}::{1}",info.DeclaringType.FullName,info.Name);
			if(description!="")
				log.WriteLine("\t\tDescription: {0}",this.description);
			log.WriteLine(STR_LINE);
			log.Unlock();

			bool failed=false;
			object ret=null;

			System.DateTime start=System.DateTime.Now;
			try {
				ret=this.info.Invoke(null,new object[]{log});
			}catch(System.Exception e){
				log.WriteError(e,"Test の実行中に例外が発生しました。");
				failed=true;
			}
			System.TimeSpan ts=System.DateTime.Now-start;

			// 後書き
			log.Lock();
			if(!failed&&info.ReturnType!=typeof(void)){
				log.WriteLine(STR_LINE);
				log.WriteLine("戻り値 ({0}):",info.ReturnType);
				log.WriteLine(ret??"null");
			}
			log.WriteLine(STR_LINE);
			log.WriteLine("\tTest を終了しました。");
			log.WriteLine("\t\t経過時間: {0} ms",ts.TotalMilliseconds);
			log.WriteLine(STR_LINE);
			log.Unlock();
		}
	}

	public class BenchMethod:ITest{
		Ref::MethodInfo minfo;
		BenchMethodAttribute attr;
		MeasureTime meth;

		private delegate System.TimeSpan MeasureTime(int loop);

		public override string ToString() {
			return "<bench>"+this.minfo.Name;
		}
		public string Description {
			get{return attr.Description;}
		}
		//===========================================================
		//		初期化
		//===========================================================
		public static BenchMethodAttribute GetAttribute(Ref::MethodInfo minfo,afh.Application.Log log){
			object[] attrs=minfo.GetCustomAttributes(typeof(BenchMethodAttribute),false);
			if(attrs.Length==0)return null;

			BenchMethodAttribute attr=(BenchMethodAttribute)attrs[0];
			if(!minfo.IsStatic){
				log.WriteLine("メソッド '{0}' は static でない為 Benchmark を実行出来ません。",minfo);
				return null;
			}
			if(minfo.GetParameters().Length!=0){
				log.WriteLine("メソッド '{0}' の呼び出しには引数が必要なので Benchmark を実行出来ません。",minfo);
				return null;
			}
			return attr;
		}
		public BenchMethod(Ref::MethodInfo minfo,BenchMethodAttribute attr){
			this.minfo=minfo;
			this.attr=attr;

			Emit::DynamicMethod m=new Emit::DynamicMethod(
				"bench",
				typeof(TimeSpan),
				new System.Type[]{typeof(int)},
				minfo.DeclaringType
				);
			//
			//	宣言
			//
			Emit::ILGenerator ilgen=m.GetILGenerator();
			System.Type type_dt=typeof(System.DateTime);
			Emit::LocalBuilder loc_start=ilgen.DeclareLocal(type_dt);
			Emit::LocalBuilder loc_end	=ilgen.DeclareLocal(type_dt);
			Emit::LocalBuilder loc_i	=ilgen.DeclareLocal(typeof(int));
			Ref::MethodInfo m_get_Now=type_dt.GetMethod("get_Now");
			Ref::MethodInfo m_op_Subtraction=type_dt.GetMethod("op_Subtraction",new System.Type[]{type_dt,type_dt});
			Emit::Label label_loopC		=ilgen.DefineLabel();
			Emit::Label label_loopJ		=ilgen.DefineLabel();
			//
			//	ロジック
			//
			//-- pro
			ilgen.Emit(Emit::OpCodes.Call,m_get_Now);
			ilgen.Emit(Emit::OpCodes.Stloc,loc_start);
			//-- loop
			ilgen.Emit(Emit::OpCodes.Ldc_I4_0);
			ilgen.Emit(Emit::OpCodes.Dup);//
			ilgen.Emit(Emit::OpCodes.Stloc,loc_i);
			ilgen.Emit(Emit::OpCodes.Br_S,label_loopJ);

			ilgen.MarkLabel(label_loopC);
			ilgen.Emit(Emit::OpCodes.Call,minfo);
			if(minfo.ReturnType!=typeof(void)){
				ilgen.Emit(Emit::OpCodes.Pop);
			}

			ilgen.Emit(Emit::OpCodes.Ldloc,loc_i);
			ilgen.Emit(Emit::OpCodes.Ldc_I4_1);
			ilgen.Emit(Emit::OpCodes.Add);
			ilgen.Emit(Emit::OpCodes.Dup);//
			ilgen.Emit(Emit::OpCodes.Stloc,loc_i);
			ilgen.MarkLabel(label_loopJ);
			//ilgen.Emit(Emit::OpCodes.Ldloc,loc_i);
			ilgen.Emit(Emit::OpCodes.Ldarg_0);
			ilgen.Emit(Emit::OpCodes.Blt_S,label_loopC);

			//-- epi
			ilgen.Emit(Emit::OpCodes.Call,m_get_Now);
			ilgen.Emit(Emit::OpCodes.Stloc,loc_end);
			ilgen.Emit(Emit::OpCodes.Ldloc,loc_end);
			ilgen.Emit(Emit::OpCodes.Ldloc,loc_start);
			ilgen.Emit(Emit::OpCodes.Call,m_op_Subtraction);
			ilgen.Emit(Emit::OpCodes.Ret);

			this.meth=(MeasureTime)m.CreateDelegate(typeof(MeasureTime));
		}
		//===========================================================
		//		計測
		//===========================================================
		public void Execute(afh.Application.Log log){
			// 前書き
			log.Lock();
			log.WriteLine();
			log.WriteLine(STR_LINE);
			log.WriteLine("\tBenchmark を開始します。");
			log.WriteLine("\t\tMethodName:  {0}::{1}",minfo.DeclaringType.FullName,minfo.Name);
			if(this.Description!="")
				log.WriteLine("\t\tDescription: {0}",this.Description);
			log.WriteLine(STR_LINE);
			log.Unlock();

			try{
				int loop=1;
				double msecs=0;
				for(int i=1;loop>0;loop<<=2,i++){
					log.WriteLine("第 {0} 回計測開始 (Loop 回数: {1} 回):",i,loop);
					TimeSpan ts=this.meth(loop);
					log.WriteLine("\t経過時間: {0} ms",msecs=ts.TotalMilliseconds);
					if(ts.Seconds!=0)break;
					if(msecs<16.0)loop<<=6;
				}

				double time=msecs/loop;
				int iunit=0;
				if(time>0)while(time<1){
					time*=1000;
					iunit++;
				}
				log.WriteLine("一回当たり時間: {0:F2} {1}s",time,units[iunit]);
			}catch(System.Exception e) {
				log.WriteError(e,"Bench の実行中に例外が発生しました。");
			}

			// 後書き
			log.Lock();
			log.WriteLine(STR_LINE);
			log.WriteLine("\tBenchmark を終了しました。");
			log.WriteLine(STR_LINE);
			log.Unlock();
		}
		private static readonly char[] units=new char[]{'m','μ','n','p','f','a','z','y'};
		const string STR_LINE="------------------------------------------------------------";
	}
}