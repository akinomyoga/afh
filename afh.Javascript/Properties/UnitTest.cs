
#if DEBUG
namespace afh.Cobalt.Debug{
	[afh.Tester.TestTarget]
	public static class UnitTest{
		public static string test1(afh.Application.Log log){
			System.Text.StringBuilder b=new System.Text.StringBuilder();
			log.WriteLine(TestParse("x=y=+++a+b--c*d+a/c/o!"));
			log.WriteLine(TestParse("(a+b)*c"));
			return b.ToString();
		}

		public static string TestParse(string input){
			Tree.IExpression e=Cobalt.Parse.LanguageDefinition.Parse(input);
			return e.ToSource();
		}
	}
}
#endif
