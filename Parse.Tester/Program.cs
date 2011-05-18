using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace afh.Parse.Tester {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() {
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			System.Windows.Forms.Application.Run(new Form1());
			//System.Windows.Forms.Application.Run(new TestDraw());
		}
	}
}