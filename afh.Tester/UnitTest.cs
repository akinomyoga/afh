using Compiler=System.Runtime.CompilerServices;

namespace afh.Tester{
	public static class UnitTest{
		[afh.Tester.BenchMethod("��̃��\�b�h�Ăяo���̎��Ԃ��v�����܂��B")]
		[Compiler::MethodImpl(Compiler::MethodImplOptions.NoInlining)]
		public static void bench_Empty(){
		}
	}
}