
namespace afh.Tester {
	static class Program {
		public static afh.Application.Log log;
		public static Form1 wndMain;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[System.STAThread]
		static void Main(string[] args){
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			log=afh.Application.LogView.Instance.CreateLog("afh.Tester");

			log.WriteLine(STR_紹介文);
			log.WriteLine("<引数>");
			foreach(string arg in args)log.WriteLine(arg);
			log.WriteLine("</引数>");
			log.WriteLine();

			
			if(args.Length==0){
				log.WriteLine("ファイルが指定されていません。.NET assembly を指定して下さい。");
				log.WriteLine(STR_説明);
			}else{
				string dllpath=SearchAssemblyLocation(args[0]);
				if(dllpath!=null)
					wndMain=new Form1(dllpath);
			}
			if(wndMain==null)wndMain=new Form1();

			System.Windows.Forms.Application.Run(wndMain);
		}


		private const string STR_紹介文=@"===========================================================
	afh.Tester (execute UnitTest)
		Copyright 2007-2008, K.Murase
===========================================================";
		private const string STR_説明=@"
	アセンブリの中から UnitTest class を検索し、その中の静的メソッドを実行します。
	実行するメソッドには [afh.dll]afh.UnitTesterTargetAttribute を適用して下さい。
	実行するメソッドのシグニチャは static void(afh.Application.Log) にして下さい。";

		private static string SearchAssemblyLocation(string str){
			string cand=System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory,str);
			if(System.IO.File.Exists(cand))return cand;
			string cand2=cand+".dll";
			if(System.IO.File.Exists(cand2))return cand2;
			cand2=cand+".exe";
			if(System.IO.File.Exists(cand2))return cand2;

			cand=System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),str);
			if(System.IO.File.Exists(cand))return cand;
			cand2=cand+".dll";
			if(System.IO.File.Exists(cand2))return cand2;
			cand2=cand+".exe";
			if(System.IO.File.Exists(cand2))return cand2;

			log.WriteLine("指定したファイルは見つかりません。正しいファイル名を指定して下さい。");
			return null;
		}
	}
}