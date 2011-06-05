using CM=System.ComponentModel;
using Ref=System.Reflection;
using Compiler=System.Runtime.CompilerServices;

public static class UnitTest{
#if DEBUG
	[CM::Description("new ColorName(string) のテストを行います。")]
	public static void test_ColorNameParse(afh.Application.Log log){
		System.Action<string> test=delegate(string text){
			int i=0;
			afh.Rendering.ColorName c=new afh.Rendering.ColorName(text,ref i);
			log.WriteLine("入力文字列    : [{0}]",text);
			log.WriteLine("読み取り文字列: [{0}]",text.Substring(0,i));
			log.WriteLine("読み取り結果  : {0}",c);
			log.WriteLine();
		};
		test("#012345");
		test("#abAbCD");
		test("#FFdd88aa");
		test("#dfaabbcc");
		test("#747");
		test("#9876");
		test("#675 #456 #436");
		test("#ＦfＡ");
		test("ReD");
		test("rgb (1  , 2 ,  3  )");
		test("hsv(12.6,29.7,200) Hello");
	}

	[CM::Description("FontManager の Serialize/Deserialize 実験")]
	public static void test_FontManager_Serialization(afh.Application.Log log) {
		afh.Rendering.FontManager manage=new afh.Rendering.FontManager("MS Mincho",10);
		manage.Bold=true;
		log.WriteLine("Serialize 前:");
		log.WriteLine(manage.ToString());

		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f=new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		System.IO.Stream mstr=new System.IO.MemoryStream();
		f.Serialize(mstr,manage);
		mstr.Position=0;

		afh.Rendering.FontManager manage2=(afh.Rendering.FontManager)f.Deserialize(mstr);
		log.WriteLine("Deserialize 後:");
		log.WriteLine(manage2.ToString());
	}

	[CM::Description("Generic で IComparable 等で原始型を比較させた時...")]
	public static void analyze_generic_PrimitiveTypes(afh.Application.Log log){
		const Ref::BindingFlags BF=Ref::BindingFlags.Static|Ref::BindingFlags.NonPublic;
		Ref::MethodInfo gen_meth=typeof(UnitTest).GetMethod("analyze_genprim_IsLargerThan",BF);
		Ref::MethodInfo nrm_meth=typeof(UnitTest).GetMethod("analyze_genprim_IsLargerThanPrim",BF);
		log.WriteLine("generic<T> body");
		log.DumpBytes(gen_meth.GetMethodBody().GetILAsByteArray()); // null らしい
		log.WriteLine("generic<int> body");
		log.DumpBytes(gen_meth.MakeGenericMethod(typeof(int)).GetMethodBody().GetILAsByteArray());
		log.WriteLine("normal body");
		log.DumpBytes(nrm_meth.GetMethodBody().GetILAsByteArray());

		// 実際に実験
		log.WriteLine(analyze_genprim_IsLargerThan<int>(10,8));
		log.WriteLine(analyze_genprim_IsLargerThanPrim(10,8));
	}
	private static bool analyze_genprim_IsLargerThan<T>(T a,T b)where T:System.IComparable<T>{
		return a.CompareTo(b)>0;
		// T==int の時
		//--------------------------
		// lea         ecx,[ebp-3Ch]
		// mov         edx,edi
		// call        78193AB0
		// mov         esi,eax
		// test        esi,esi
		// setg        al
		// movzx       eax,al
		// mov         ebx,eax
	}
	private static bool analyze_genprim_IsLargerThanPrim(int a,int b) {
		return a>b;
		// cmp         edi,esi
		// setg        al
		// movzx       eax,al
		// mov         ebx,eax
	}

	#region string 内部バッファの確保の仕方
	// [以前の実行で分かった事]
	// .SubString(0) や +"" を実行しても同じバッファを参照
	// .SubString(2,3) 等を実行すると新しくバッファが生成される
	//     文字配列の一部なのだから、「同じバッファの異なる場所」でも構わない様な気がするが、
	//     メモリ管理の都合上、二つ以上の managed object が同じ unmanaged memory を利用できないのだろう
	[CM::Description(@"string の文字配列の確保の仕方を調べる...")]
	public static void test_string_buffer(afh.Application.Log log){
		log.Lock();

		log.WriteLine("---- test1 -----------");
		log.WriteLine("  同内容の文字列即値");
		log.WriteLine("----------------------");
		string text0="AIUEO";
		string text1="AIUEO";
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1);
		log.WriteLine("text1 を直接書き換え...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // 結果: 両方変更されていた
		log.WriteLine();

		log.WriteLine("---- test2 -----------");
		log.WriteLine("  即値書き換え後、書き換え前の即値を代入したつもり");
		log.WriteLine("----------------------");
		log.WriteLine("text0 に AIUEO を代入...");
		text0="AIUEO";
		log.WriteVar("text0",text0);
		log.WriteLine("text0 に \"AIUE\" を代入...");
		log.WriteLine("text0+=\"O\"...");
		text0="AIUE";
		text0+="O";
		log.WriteVar("text0",text0);
		log.WriteLine();

		log.WriteLine("---- test3 -----------");
		log.WriteLine("  +\"\" で異なるバッファになるか");
		log.WriteLine("----------------------");
		text0="IROHA";
		text1=text0+"";
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text0+\"\"",text1);
		log.WriteLine("text1 を直接書き換え...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1);
		log.WriteLine();

		log.WriteLine("---- test4 -----------");
		log.WriteLine("  SubString(0) で異なるバッファになるか");
		log.WriteLine("----------------------");
		text0="AMETSUCHI";
		text1=text0.Substring(0);
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text2.SubString(0)",text1);
		log.WriteLine("text1 を直接書き換え...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // 同じインスタンス
		log.WriteLine();

		log.WriteLine("---- test5 -----------");
		log.WriteLine("  SubString(*,*) で異なるバッファになるか");
		log.WriteLine("----------------------");
		text0="TAWYINI";
		text1=text0.Substring(2,3);
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text2.SubString(2,3)",text1);
		log.WriteLine("text1 を直接書き換え...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // 異なるインスタンス
		log.WriteLine();

		log.Unlock();
	}
	private static unsafe void string_ToLower(string text){
		fixed(char* chB=text) {
			char* ch=chB;
			char* chM=ch+text.Length;
			while(ch<chM) {
				if('A'<=*ch&&*ch<='Z') *ch=(char)(*ch-'A'+'a');
				ch++;
			}
		}
	}
	#endregion
#else
	// [Release 構成で実験した]
	// 計測結果には、ばらつきがあったので、
	//  1. 何度かやった結果からかけ離れている物は、異常値として除去
	//  2. 同じ結果を連続で出した時の物を採用
	// といういい加減な物なので、有効数字は上一・二桁ぐらいしかないと思うこと。
	//
	// MethodName:  UnitTest::test_inline
	//     経過時間: 562.5 ms
	// MethodName:  UnitTest::test_inline_cmp
	//     経過時間: 1859.375 ms
	// MethodName:  UnitTest::test_sub
	//     経過時間: 562.5 ms
	// MethodName:  UnitTest::test_sub_ni
	//     経過時間: 1859.375 ms
	// MethodName:  UnitTest::test_gen
	//     経過時間: 2609.375 ms
	// MethodName:  UnitTest::test_gen_ni
	//     経過時間: 2609.375 ms
	//
	// 結論:
	// A. generic で作ると...
	//   1. inline 展開すらされない
	//   2. CompareTo 呼び出しの儘
	// B. 通常の関数で作ると...
	//   1. inline 展開される
	// C. inline で CompareTo を記述すると...
	//   1. inline 展開されない
	//     (通常の関数で NoInlining の時と同じ計測結果の為)
	//
	private const int LOOP=0x10000000;
	public static void test_inline(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(i>j) c++;
		}
	}

	public static void test_inline_cmp(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(i.CompareTo(j)>0) c++;
		}
	}

	// 通常関数 呼び出し?/inline?
	public static void test_sub(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(compare(i,j)) c++;
		}
	}
	private static bool compare(int a,int b) {
		return a>b;
	}

	// 通常関数 呼び出し
	public static void test_sub_ni(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(compare_ni(i,j)) c++;
		}
	}
	[Compiler::MethodImpl(Compiler::MethodImplOptions.NoInlining)]
	private static bool compare_ni(int a,int b) {
		return a>b;
	}

	// gen 関数 呼び出し?/inline?
	public static void test_gen(afh.Application.Log log){
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--){
			if(compare_gen(i,j))c++;
		}
	}
	private static bool compare_gen<T>(T a,T b) where T:System.IComparable<T> {
		return a.CompareTo(b)>0;
	}

	// gen 関数 呼び出し
	public static void test_gen_ni(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(compare_gen(i,j)) c++;
		}
	}
	[Compiler::MethodImpl(Compiler::MethodImplOptions.NoInlining)]
	private static bool compare_gen_ni<T>(T a,T b) where T:System.IComparable<T> {
		return a.CompareTo(b)>0;
	}
#endif
}
