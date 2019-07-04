namespace Tester.Javascript{
	[TestFunctionAttribute(
		"�L���X�g�̃e�X�g"
		,@"�L���X�g�̓���ɂ��ăe�X�g���܂�
-----
null is CastTest �̌���
null as CastTest �̌���
(CastTest)null �Ŕ�������G���[�̎��"
	)]
	public class CastTest:TestFunction{
		public CastTest(){}
		public override string Exec(){
			string r="";
			CastTest Null=null;
			r+=(Null is CastTest).ToString()+"\n";
			r+=(null as CastTest)==null
				?"as ���Z�q�ɂ���� null �ɕϊ�����܂���"
				:"as ���Z�q�ɂ���� null �ȊO�̉����ɕϊ�����܂���";
			r+="\n";
			try{
				r+=((CastTest)null).ToString()+"\n";
			}catch(System.Exception e){
				r+=e.GetType().ToString()+"\n";
			}
			return r;
		}
	}

	[TestFunctionAttribute("throw new System.Exception() �e�X�g","�\�� throw �������� catch ���܂��B����ɂ����鎞�Ԃ��m�F���ĉ�����")]
	public class ThrowTest:TestFunction{
		public ThrowTest(){}
		public override string Exec(){
			for(int i=0;i<10;i++){
				try{this.ThrowError();}catch{}
			}
			return "����";
		}
		public void ThrowError(){
			throw new System.Exception("����͎����œ������G���[�ł�");
		}
	}

	[TestFunctionAttribute("System.Type#GetMethods �e�X�g","GetMethods �Ōp�����������o���擾���鎖���o���邩�ǂ���������")]
	public class GetMethods:TestFunction{
		public GetMethods(){}
		public override string Exec(){
			string r="=== �ȉ� class GetMethods\n";
			foreach(System.Reflection.MethodInfo m in typeof(GetMethods).GetMethods()){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			r+="=== �ȉ� class System.GC\n";
			foreach(System.Reflection.MethodInfo m in typeof(System.GC).GetMethods()){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			r+="=== �ȉ� class System.GC (Static|Public)\n";
			foreach(System.Reflection.MethodInfo m in typeof(System.GC).GetMethods(
				System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public
			)){
				r+=m.Name+"("+this.Parameter2String(m)+");\n";
			}
			return r;
		}
		public string Parameter2String(System.Reflection.MethodInfo m){
			string r="";
			foreach(System.Reflection.ParameterInfo p in m.GetParameters()){
				r+=","+p.ParameterType.ToString();
			}
			return r.Length==0?"":r.Substring(1);
		}
	}
	[TestFunctionAttribute("System.Type#IsSubclassOf �e�X�g",@"IsSubclassOf �Ōp��(����)���� interface ���w�肵�Ă� true ��Ԃ����ۂ�")]
	public class IsSubclassOf:TestFunction,IIsSubclassOf{
		public IsSubclassOf(){}
		public override string Exec(){
			string r="";
			System.Type t=typeof(IsSubclassOf);
			r+=t.IsSubclassOf(typeof(IIsSubclassOf))
				?"�p��(����)���� interface �ł� true ��Ԃ��܂�"
				:"interface ���p��(����)���Ă��Ă� false ��Ԃ��܂�";
			r+="\n";
			r+=t.GetInterface("IIsSubclassOf")!=null
				?"GetInterface(\"...\")!=null ���g���܂�":"GetInterface �́~";
			r+="\n";
			return r;
		}
	}
	public interface IIsSubclassOf{
		string Exec();
	}

	[TestFunctionAttribute("params ����",@"params ������ ParameterInfo
�cParamsAttribute �� GetCustomAttributes �Ŏ擾���鎖���o���邩?
���������ŌĂяo�������o���邩?
�����̔z������ɒu���ČĂяo�������o���邩?")]
	public class ParamsAttribute:TestFunction{
		public ParamsAttribute(){}
		public override string Exec(){
			string r=this.hasParams(typeof(ParamsAttribute).GetMethod("testF").GetParameters()[0])
				?"GetCustomAttribute �Ŏ擾���鎖���o���܂�":"GetCustomAttribute �ł͎擾���鎖�͏o���܂���";
			r+="\n";
			r+=@"���������ŌĂяo�������o���܂��B
	�����z��̗v�f�̐��� "+this.testF()+" �ł��B\n";
			int[] ints=new int[]{1,2,2,3,3,4};
			r+=@"�����̔z������ɒu���ČĂяo�������o���܂��B
	�n�����z��̗v�f�̐��� 6 �ł��B
	�����z��̗v�f�̐��� "+this.testF(ints)+" �ł��B\n";
			r+=@"�����̔z��𕡐�����ɒu���ČĂяo�����͏o���܂���B
	(�R���p�C���G���[�ɂȂ���)";
			/*
			r+=@"�X�Ɉ����̔z��𕡐�����ɒu���ČĂяo�������o���܂��B
	�n�����z��̗v�f�̐��� 6 �ł��B
	�n�����z��̐��� 2 �ł��B
	���ڎw�肵�������� 1 �ł��B
	�����z��̗v�f�̐��� "+this.testF(ints,ints)+" �ł��B\n";//*/
			return r;
		}
		private bool hasParams(System.Reflection.ParameterInfo p){
			return p.GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0;
		}
		public int testF(params int[] ints){
			return ints.Length;
		}
	}
}