using Compiler=System.Runtime.CompilerServices;

namespace afh.Tester{
	public static class UnitTest{
		[afh.Tester.BenchMethod("空のメソッド呼び出しの時間を計測します。")]
		[Compiler::MethodImpl(Compiler::MethodImplOptions.NoInlining)]
		public static void bench_Empty(){
		}
	}
}