using CM=System.ComponentModel;
using Ref=System.Reflection;
using Compiler=System.Runtime.CompilerServices;

public static class UnitTest{
#if DEBUG
	[CM::Description("new ColorName(string) �̃e�X�g���s���܂��B")]
	public static void test_ColorNameParse(afh.Application.Log log){
		System.Action<string> test=delegate(string text){
			int i=0;
			afh.Rendering.ColorName c=new afh.Rendering.ColorName(text,ref i);
			log.WriteLine("���͕�����    : [{0}]",text);
			log.WriteLine("�ǂݎ�蕶����: [{0}]",text.Substring(0,i));
			log.WriteLine("�ǂݎ�茋��  : {0}",c);
			log.WriteLine();
		};
		test("#012345");
		test("#abAbCD");
		test("#FFdd88aa");
		test("#dfaabbcc");
		test("#747");
		test("#9876");
		test("#675 #456 #436");
		test("#�ef�`");
		test("ReD");
		test("rgb (1  , 2 ,  3  )");
		test("hsv(12.6,29.7,200) Hello");
	}

	[CM::Description("FontManager �� Serialize/Deserialize ����")]
	public static void test_FontManager_Serialization(afh.Application.Log log) {
		afh.Rendering.FontManager manage=new afh.Rendering.FontManager("MS Mincho",10);
		manage.Bold=true;
		log.WriteLine("Serialize �O:");
		log.WriteLine(manage.ToString());

		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f=new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		System.IO.Stream mstr=new System.IO.MemoryStream();
		f.Serialize(mstr,manage);
		mstr.Position=0;

		afh.Rendering.FontManager manage2=(afh.Rendering.FontManager)f.Deserialize(mstr);
		log.WriteLine("Deserialize ��:");
		log.WriteLine(manage2.ToString());
	}

	[CM::Description("Generic �� IComparable ���Ō��n�^���r��������...")]
	public static void analyze_generic_PrimitiveTypes(afh.Application.Log log){
		const Ref::BindingFlags BF=Ref::BindingFlags.Static|Ref::BindingFlags.NonPublic;
		Ref::MethodInfo gen_meth=typeof(UnitTest).GetMethod("analyze_genprim_IsLargerThan",BF);
		Ref::MethodInfo nrm_meth=typeof(UnitTest).GetMethod("analyze_genprim_IsLargerThanPrim",BF);
		log.WriteLine("generic<T> body");
		log.DumpBytes(gen_meth.GetMethodBody().GetILAsByteArray()); // null �炵��
		log.WriteLine("generic<int> body");
		log.DumpBytes(gen_meth.MakeGenericMethod(typeof(int)).GetMethodBody().GetILAsByteArray());
		log.WriteLine("normal body");
		log.DumpBytes(nrm_meth.GetMethodBody().GetILAsByteArray());

		// ���ۂɎ���
		log.WriteLine(analyze_genprim_IsLargerThan<int>(10,8));
		log.WriteLine(analyze_genprim_IsLargerThanPrim(10,8));
	}
	private static bool analyze_genprim_IsLargerThan<T>(T a,T b)where T:System.IComparable<T>{
		return a.CompareTo(b)>0;
		// T==int �̎�
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

	#region string �����o�b�t�@�̊m�ۂ̎d��
	// [�ȑO�̎��s�ŕ���������]
	// .SubString(0) �� +"" �����s���Ă������o�b�t�@���Q��
	// .SubString(2,3) �������s����ƐV�����o�b�t�@�����������
	//     �����z��̈ꕔ�Ȃ̂�����A�u�����o�b�t�@�̈قȂ�ꏊ�v�ł��\��Ȃ��l�ȋC�����邪�A
	//     �������Ǘ��̓s����A��ȏ�� managed object ������ unmanaged memory �𗘗p�ł��Ȃ��̂��낤
	[CM::Description(@"string �̕����z��̊m�ۂ̎d���𒲂ׂ�...")]
	public static void test_string_buffer(afh.Application.Log log){
		log.Lock();

		log.WriteLine("---- test1 -----------");
		log.WriteLine("  �����e�̕����񑦒l");
		log.WriteLine("----------------------");
		string text0="AIUEO";
		string text1="AIUEO";
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1);
		log.WriteLine("text1 �𒼐ڏ�������...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // ����: �����ύX����Ă���
		log.WriteLine();

		log.WriteLine("---- test2 -----------");
		log.WriteLine("  ���l����������A���������O�̑��l������������");
		log.WriteLine("----------------------");
		log.WriteLine("text0 �� AIUEO ����...");
		text0="AIUEO";
		log.WriteVar("text0",text0);
		log.WriteLine("text0 �� \"AIUE\" ����...");
		log.WriteLine("text0+=\"O\"...");
		text0="AIUE";
		text0+="O";
		log.WriteVar("text0",text0);
		log.WriteLine();

		log.WriteLine("---- test3 -----------");
		log.WriteLine("  +\"\" �ňقȂ�o�b�t�@�ɂȂ邩");
		log.WriteLine("----------------------");
		text0="IROHA";
		text1=text0+"";
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text0+\"\"",text1);
		log.WriteLine("text1 �𒼐ڏ�������...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1);
		log.WriteLine();

		log.WriteLine("---- test4 -----------");
		log.WriteLine("  SubString(0) �ňقȂ�o�b�t�@�ɂȂ邩");
		log.WriteLine("----------------------");
		text0="AMETSUCHI";
		text1=text0.Substring(0);
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text2.SubString(0)",text1);
		log.WriteLine("text1 �𒼐ڏ�������...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // �����C���X�^���X
		log.WriteLine();

		log.WriteLine("---- test5 -----------");
		log.WriteLine("  SubString(*,*) �ňقȂ�o�b�t�@�ɂȂ邩");
		log.WriteLine("----------------------");
		text0="TAWYINI";
		text1=text0.Substring(2,3);
		log.WriteVar("text0",text0);
		log.WriteVar("text1=text2.SubString(2,3)",text1);
		log.WriteLine("text1 �𒼐ڏ�������...");
		string_ToLower(text1);
		log.WriteVar("text0",text0);
		log.WriteVar("text1",text1); // �قȂ�C���X�^���X
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
	// [Release �\���Ŏ�������]
	// �v�����ʂɂ́A�΂�����������̂ŁA
	//  1. ���x����������ʂ��炩������Ă��镨�́A�ُ�l�Ƃ��ď���
	//  2. �������ʂ�A���ŏo�������̕����̗p
	// �Ƃ������������ȕ��Ȃ̂ŁA�L�������͏��E�񌅂��炢�����Ȃ��Ǝv�����ƁB
	//
	// MethodName:  UnitTest::test_inline
	//     �o�ߎ���: 562.5 ms
	// MethodName:  UnitTest::test_inline_cmp
	//     �o�ߎ���: 1859.375 ms
	// MethodName:  UnitTest::test_sub
	//     �o�ߎ���: 562.5 ms
	// MethodName:  UnitTest::test_sub_ni
	//     �o�ߎ���: 1859.375 ms
	// MethodName:  UnitTest::test_gen
	//     �o�ߎ���: 2609.375 ms
	// MethodName:  UnitTest::test_gen_ni
	//     �o�ߎ���: 2609.375 ms
	//
	// ���_:
	// A. generic �ō���...
	//   1. inline �W�J���炳��Ȃ�
	//   2. CompareTo �Ăяo���̘�
	// B. �ʏ�̊֐��ō���...
	//   1. inline �W�J�����
	// C. inline �� CompareTo ���L�q�����...
	//   1. inline �W�J����Ȃ�
	//     (�ʏ�̊֐��� NoInlining �̎��Ɠ����v�����ʂ̈�)
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

	// �ʏ�֐� �Ăяo��?/inline?
	public static void test_sub(afh.Application.Log log) {
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--) {
			if(compare(i,j)) c++;
		}
	}
	private static bool compare(int a,int b) {
		return a>b;
	}

	// �ʏ�֐� �Ăяo��
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

	// gen �֐� �Ăяo��?/inline?
	public static void test_gen(afh.Application.Log log){
		int c=0;
		for(int i=0,j=LOOP;i<LOOP;i++,j--){
			if(compare_gen(i,j))c++;
		}
	}
	private static bool compare_gen<T>(T a,T b) where T:System.IComparable<T> {
		return a.CompareTo(b)>0;
	}

	// gen �֐� �Ăяo��
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
