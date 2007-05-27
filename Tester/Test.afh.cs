namespace Tester.afhTest{
	[TestFunction("afh.Application.SettingContainerAttribute test","ちゃんと動作するかどうか実験")]
	public class TestSettingContainer:Tester.TestFunction{
		public override string Exec(){
			Tester.TestSettingForm f=new TestSettingForm();
			f.Show();
			return "Initialization was succeeded, and now showing window...";
		}
	}	
	[TestFunction("System.Type 別 Code 生成",@"各 type.FullName の HashCode を元に switch で分岐するコードを生成します
HashCode が偶然一致する物があった場合などには対応していません")]
	public class TestTypeHash2:Tester.TestFunction{
		public override string Exec(){
			this.WriteLine("switch(t.FullName.GetHashCode()&0x7fffffff){");
			this.WriteTypes1();
			this.WriteLine("\tdefault:");
			this.WriteLine("\t\tbreak;");
			this.WriteLine("}");
			return this.outstr;
		}
		private void WriteTypes1(){
			this.WriteType(typeof(short));
			this.WriteType(typeof(ushort));
			this.WriteType(typeof(int));
			this.WriteType(typeof(uint));
			this.WriteType(typeof(long));
			this.WriteType(typeof(ulong));
			this.WriteType(typeof(sbyte));
			this.WriteType(typeof(byte));
			this.WriteType(typeof(decimal));
			this.WriteType(typeof(char));
			this.WriteType(typeof(float));
			this.WriteType(typeof(double));
			this.WriteType(typeof(bool));
			this.WriteType(typeof(string));
			this.WriteType(typeof(System.IntPtr));
			this.WriteType(typeof(System.UIntPtr));
			this.WriteType(typeof(System.Guid));
			this.WriteType(typeof(System.TimeSpan));
			this.WriteType(typeof(System.DateTime));
			this.WriteType(typeof(System.Type));
			this.WriteType(typeof(System.Enum));
			this.WriteType(typeof(void));
			this.WriteType(typeof(object));
			this.WriteType(typeof(bool[]));
		}
		private void WriteTypes2(){
			this.WriteType(typeof(int[]));
			this.WriteType(typeof(System.Windows.Forms.SelectionRange));
			this.WriteType(typeof(string[]));
		}
		private void WriteTypes3(){
			this.WriteType(typeof(System.Windows.Forms.MenuItem));
			this.WriteType(typeof(System.Windows.Forms.CheckedListBox));
			this.WriteType(typeof(System.Windows.Forms.MonthCalendar));
			this.WriteType(typeof(System.Windows.Forms.ToolBarButton));
			//this.WriteType(typeof(System.Windows.Forms.CheckBox));
			//this.WriteType(typeof(System.Windows.Forms.Button));
			//this.WriteType(typeof(System.Windows.Forms.ColorDialog));
			//this.WriteType(typeof(System.Windows.Forms.ComboBox));
			//this.WriteType(typeof(System.Windows.Forms.DateTimePicker));
			//this.WriteType(typeof(System.Windows.Forms.DomainUpDown));
			//this.WriteType(typeof(System.Windows.Forms.HScrollBar));
			//this.WriteType(typeof(System.Windows.Forms.NumericUpDown));
			//this.WriteType(typeof(System.Windows.Forms.RadioButton));
			//this.WriteType(typeof(System.Windows.Forms.TextBox));
			//this.WriteType(typeof(System.Windows.Forms.TrackBar));
			//this.WriteType(typeof(System.Windows.Forms.VScrollBar));
		}
		private void WriteType(System.Type t){
			string name=t.FullName;
			int hash=0x7fffffff&name.GetHashCode();
			this.WriteLine("\tcase 0x"+hash.ToString("X8")+":");
			this.WriteLine("\t\tif(t!=typeof("+t.FullName+"))goto default;");
			this.WriteLine("\t\tbreak;");
			/*/
			//--
			string type=t.ToString();
			this.WriteLine("\tcase 0x"+t.FullName.GetHashCode().ToString("X")+":");
			this.WriteLine("\t\tif(t!=typeof("+type+"))goto default;");
			string hash=(t.GetHashCode()&0xfff).ToString("X3");
			this.WriteLine("\t\t"+type+" o"+hash+"=("+type+")obj;");
			this.WriteLine("\t\to"+hash+";");
			this.WriteLine("\t\treturn t.GetProperty(\"\");");
			//*/
		}
	}
}