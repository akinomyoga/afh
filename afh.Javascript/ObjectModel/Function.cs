namespace afh.JavaScript{
	public abstract class FunctionBase:Object{
		/// <summary>
		/// FunctionBase �̃R���X�g���N�^
		/// prototype ��ݒ肵�܂�
		/// </summary>
		protected FunctionBase(){
			this["prototype"]=new JavaScript.Object();
		}
		/// <summary>
		/// ���̊֐������s���܂��B
		/// </summary>
		/// <param name="obj">���̊֐���ێ����� Object �ł��B�֐�������� this �ŎQ�Ƃ���܂�</param>
		/// <param name="arguments">���̊֐��ɓn���������̔z���ݒ肵�܂�</param>
		/// <returns>���̊֐������s�������ʂ�Ԃ��܂�</returns>
		public abstract JavaScript.Object Invoke(JavaScript.Object obj,JavaScript.Array arguments);
		//===========================================================
		//		.NET��Javascript �p�̐ÓI���\�b�h
		//===========================================================
		/// <summary>
		/// �w�肵�� .NET �̃N���X�E�\���̂���A
		/// �w�肵�����O�����C���X�^���X���\�b�h���擾���܂�
		/// </summary>
		/// <param name="t">�N���X�E�\���̂�\�� System.Type</param>
		/// <param name="name">���\�b�h��\�����O</param>
		/// <returns>�����������\�b�h���̔z��</returns>
		public static System.Reflection.MethodInfo[] GetMethods(System.Type t,string name){
			//CHECK>OK: GetMethods �Ōp�����̃����o���������Ă����̂��ǂ����s��
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in t.GetMethods()){
				if(m.IsStatic||m.Name!=name)continue;
				list.Add(m);
			}
			return (System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// Javascript ���Ŏw�肵�������̌��̕��� MethodInfo �̗v����������̌���葽���ꍇ�A
		/// �ߏ�Ȉ����������̓K����
		/// </summary>
		public const float COMPAT_OVERPARAM=-1.5f;
		/// <summary>
		/// Javascript ���Ŏw�肵�������ƁA�w�肵�� MethodInfo �̈����̓K�����𔻒肵�܂�
		/// </summary>
		/// <returns>
		/// �v�Z�����K������Ԃ��܂��B
		/// ������ϊ����鎖���o���Ȃ��ꍇ�ɂ� float.NegativeInfinity ��Ԃ��܂�
		/// </returns>
		/// <remarks>
		/// TODO: params �C���q���t���Ă��鎞�A����v�f���w�肵�Ȃ��ꍇ�̓��������
		/// 1. arrParam.Length>arguments.length �̔��肾���ł͕s�\��
		/// 2. �_����Ⴍ����(params �̕t�������������������ʂ̃I�[�o�[���[�h�̉\��)
		/// </remarks>
		public static float ConvertParamsCompat(System.Reflection.MethodInfo m,JavaScript.Array arguments){
			System.Reflection.ParameterInfo[] arrParam=m.GetParameters();
			if(arrParam.Length==0)return 0;
			if(arrParam.Length>arguments.length)return float.NegativeInfinity;
			int iM=arrParam.Length;
			float r=0;
			for(int i=0;i<iM;i++){
				//CHECK>OK: params �̓������֐������ʏo���邩���ۂɊm�F���鎖
				if(i+1==iM&&arrParam[iM-1].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
					System.Type t=arrParam[iM-1].ParameterType.GetElementType();
					iM=arguments.length;
					for(;i<iM;i++)r+=arguments[i].ConvertCompat(t);
					break;
				}
				r+=arguments[i].ConvertCompat(arrParam[i].ParameterType);
			}
			return r+(arguments.length-iM)*COMPAT_OVERPARAM;
		}
		/// <summary>
		/// Javascript ���Ŏw�肵�� Javascript.Array �^�̈����z���
		/// object[] �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="m">�ŏI�I�ɌĂяo���������\�b�h</param>
		/// <param name="arguments">�Ăяo���ɗp���� Javascript.Array �̈����z��</param>
		/// <returns>���ۂɌĂяo���ɗp���鎖�̏o��������z�� object[]</returns>
		/// <remarks>
		/// ����: ConvertParamsCompat �̏����Ǝ����������s���̂ŁA
		/// ConvertParamsCompat �̍ۂ� System.Type[] ������Ēu���̂���B
		/// </remarks>
		public static object[] ConvertParams(System.Reflection.MethodInfo m,JavaScript.Array arguments){
			System.Reflection.ParameterInfo[] arrParam=m.GetParameters();
			int iM=arrParam.Length;
			object[] r=new object[iM];
			for(int i=0;i<iM;i++){
				if(i+1==iM&&arrParam[iM-1].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
					//�ϊ�
					System.Collections.ArrayList list=new System.Collections.ArrayList();
					System.Type t=arrParam[iM-1].ParameterType.GetElementType();
					for(;i<arguments.length;i++)list.Add(arguments[i].Convert(t));

					r[iM-1]=list.ToArray(t);
				}else{
					r[i]=arguments[i].Convert(arrParam[i].ParameterType);
				}
			}
			return r;
		}
	}
/*
=====================================================================
		Function
---------------------------------------------------------------------
	Javascript �Œ�`���ꂽ�֐�
	Context �Ƃ��� Javascript.Object �������������ɕϐ����i�[����
		this		(���������Ă�����) this �ɑ�����鎖�͕s�\�Ƃ���
					(this �ɑ�����鎖�������Ă��ǂ����A���̍s�ׂ͊ԈႢ�̌��ɂȂ��)
		caller		�Ăяo�����Ɗ֐��̏��
		arguments	[out][ref] ���̎������l���Ă��ǂ�
		__proto__	�֐����`�������̕ϐ��ɑ΂���Q��(�Ⴕ��ԊO���Ȃ�� __proto__=_global)
					(���l��ύX���鎞�ɂ͌��̕ϐ�������������l�ɂ���)
			��:		function main(){
						var func=funtion(){
							...
						}
					}
					
					func.Context -------�� main.Context -------�� _global
					             __proto__              __proto__

*/
	/// <summary>
	/// .NET �̃C���X�^���X���\�b�h��\���I�u�W�F�N�g�ł��B
	/// ���̃N���X�͌^���ƃ��\�b�h���������A
	/// ���s���Ɏw�肵���I�u�W�F�N�g�̊Y�����\�b�h�����s���܂��B
	/// </summary>
	/// <remarks>
	/// Object ���֐�������Ă������́A�e�I�u�W�F�N�g���L������ ManagedDelegate �Ɏ����I�ɕϊ�����܂��B
	/// </remarks>
	public class ManagedMethod:FunctionBase{
		internal System.Type type;
		internal System.Reflection.MethodInfo[] methods;
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// ManagedMethod ���^�ƃ��\�b�h���y��
		/// ���\�b�h�̌`�Ԃ�\�� BindingFlags �ɂ���ăC���X�^���X�����܂�
		/// </summary>
		/// <param name="t">���\�b�h�����^</param>
		/// <param name="name">���\�b�h�̖��O</param>
		/// <param name="fBind">���\�b�h�̌`�Ԃ�����</param>
		/// <remarks>
		/// ������ fBind �̖����ꍇ�Ɩw�Ǔ���
		/// �c�قȂ�̂� this.type.GetMethods() �̈�������
		/// </remarks>
		public ManagedMethod(System.Type t,string name,System.Reflection.BindingFlags fBind){
			this.type=t;
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in this.type.GetMethods(fBind)){
				if(m.Name!=name)continue;
				list.Add(m);
			}
			this.methods=(System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// ManagedMethod ���^�ƃ��\�b�h���ɂ���ăC���X�^���X�����܂�
		/// </summary>
		/// <param name="t">���\�b�h�����^</param>
		/// <param name="name">���\�b�h�̖��O</param>
		public ManagedMethod(System.Type t,string name){
			this.type=t;
			System.Collections.ArrayList list=new System.Collections.ArrayList();
			foreach(System.Reflection.MethodInfo m in this.type.GetMethods()){
				if(m.Name!=name)continue;
				list.Add(m);
			}
			this.methods=(System.Reflection.MethodInfo[])list.ToArray(typeof(System.Reflection.MethodInfo));
		}
		/// <summary>
		/// �I�u�W�F�N�g�̌^�ƃ��\�b�h�����g�p���āAManageMethod �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="t">���\�b�h�����I�u�W�F�N�g�̌^</param>
		/// <param name="methods">
		/// t �Ŏw�肵���^�̎����\�b�h�̏��B
		/// t �Ɏw�肵���^�ȊO�̌^�������\�b�h���w�肵�Ȃ��ŉ������B
		/// </param>
		public ManagedMethod(System.Type t,params System.Reflection.MethodInfo[] methods){
			this.type=t;
			this.methods=methods;
		}
		//===========================================================
		//		Invoke
		//===========================================================
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̓��Y���\�b�h�����s���܂�
		/// </summary>
		/// <param name="obj">�I�u�W�F�N�g���w�肵�܂�</param>
		/// <param name="arguments">���\�b�h�ɓn���������w�肵�܂�</param>
		/// <returns>���\�b�h�̌��ʂ�Ԃ��܂�</returns>
		/// <remarks>
		/// ��1: Type �� Methods ��ێ��� Invoke �̖��߂�������
		/// <list>
		/// <item><description>Type �� obj ���ƍ�</description></item>
		/// <item><description>�������X�g�̔�r</description></item>
		/// <item><description>���s</description></item>
		/// </list>
		/// </remarks>
		public override JavaScript.Object Invoke(JavaScript.Object obj,JavaScript.Array arguments){
			// ManagedObject �ȊO�̎� (Javascript.Object �𒼐ڎw�肵���ꍇ�̎�) �ɂ��Ή����鎖�ɂ���B
			//	�Â��R�[�h
			//		if(!(obj is Javascript.ManagedObject))throw new System.ArgumentException(INVOKE_NOTMANAGED,"obj");
			//		object o=((Javascript.ManagedObject)obj).Value;
			object o=(obj is JavaScript.ManagedObject)?((JavaScript.ManagedObject)obj).Value:obj;

			//CHECK>�~: IsSubclassOf �Ōp������ interface ���m�F���鎖���o���邩�ǂ���
			if(!Global.IsCastable(o.GetType(),this.type))throw new System.ArgumentException(INVOKE_NOTSUPPORTED,"obj");
			System.Reflection.MethodInfo m;
			try{
				m=this.SelectOverload(arguments);
			}catch(System.Exception e){throw e;}
			object ret=m.Invoke(o,FunctionBase.ConvertParams(m,arguments));
			return Global.ConvertFromManaged(ret);
		}
		private const string INVOKE_NOTMANAGED="obj �ɂ� ManagedObject ���w�肵�ĉ�����";
		private const string INVOKE_NOTSUPPORTED="�w�肵���I�u�W�F�N�g�͓����\�b�h�������܂���";
		/// <summary>
		/// �w�肵�������ɍł��K�����郁�\�b�h�̃I�[�o�[���[�h���������܂�
		/// </summary>
		/// <param name="arguments">����</param>
		/// <returns>�ł��K�����郁�\�b�h�̃I�[�o�[���[�h</returns>
		/// <exception cref="ArgumentException">
		/// �w�肵�������ɓK�����郁�\�b�h�̃I�[�o�[���[�h��������Ȃ������ꍇ�ɔ������܂��B
		/// </exception>
		protected System.Reflection.MethodInfo SelectOverload(JavaScript.Array arguments){
			float comp0,compat=float.NegativeInfinity;
			System.Reflection.MethodInfo m0,m=null;
			int iM=this.methods.Length;
			int w=0;	//�d�Ȃ�
			for(int i=0;i<iM;i++){
				m0=this.methods[i];
				comp0=FunctionBase.ConvertParamsCompat(m0,arguments);
				if(comp0<compat)continue;
				if(comp0==compat){w++;continue;}
				w=1;
				compat=comp0;
				m=m0;
			}
			if(compat==float.NegativeInfinity)throw new System.ArgumentException(OVERLOAD_NONE,"arguments");
			if(w>1)throw new System.ArgumentException(OVERLOAD_AMBIGUOUS,"arguments");
			return m;
		}
		private const string OVERLOAD_NONE="�w�肵�������ɓK������I�[�o�[���[�h��������܂���ł���";
		private const string OVERLOAD_AMBIGUOUS="�w�肵�������̌^���B���Ȉ׃I�[�o�[���[�h����ɍi��܂���";
	}
	
	/// <summary>
	/// .NET Framework �̃��\�b�h��\���N���X�ł��B
	/// ����̃I�u�W�F�N�g�̃C���X�^���X���\�b�h�A
	/// ���́A����̃N���X�E�\���̂̐ÓI���\�b�h��\���܂��B
	/// ����̃C���X�^���X�̃��\�b�h��\�����ɂ́A
	/// ���̃N���X�͂��̃C���X�^���X�ւ̎Q�ƂƁA���\�b�h����ێ����܂��B
	/// �ÓI���\�b�h��\�����ɂ́A���\�b�h���݂̂�ێ����܂��B
	/// </summary>
	public class ManagedDelegate:ManagedMethod{
		/// <summary>
		/// ���̃N���X�C���X�^���X�̕ێ�����
		/// managed object (.NET Framework �� object) �ւ̎Q�ƁB
		/// </summary>
		protected object obj;
		//===========================================================
		//		.ctor ; �C���X�^���X���\�b�h�p
		//===========================================================
		public ManagedDelegate(object obj,System.Type type,params System.Reflection.MethodInfo[] methods):base(type,methods){
			this.obj=obj;
		}
		public ManagedDelegate(object obj,ManagedMethod meth):base(meth.type,meth.methods){
			this.obj=obj;
		}
		public ManagedDelegate(object obj,System.Type type,string methodName):base(type,methodName){
			this.obj=obj;
		}
		//===========================================================
		//		.ctor ; �ÓI���\�b�h�p
		//===========================================================
		/// <summary>
		/// �ÓI���\�b�h��\�� MethodDelegate �̃R���X�g���N�^
		/// </summary>
		/// <param name="methods">�ÓI���\�b�h�̏��</param>
		public ManagedDelegate(System.Reflection.MethodInfo methods):this(null,null,methods){}
		/// <summary>
		/// �ÓI���\�b�h��\�� MethodDelegate �̃R���X�g���N�^
		/// </summary>
		/// <param name="type">�ÓI���\�b�h�̏�������N���X�E�\����</param>
		/// <param name="methodName">
		/// �ÓI���\�b�h�̖��O�B
		/// TODO: �ÓI�E��ÓI�̃`�F�b�N�@�\�����Ȃ���΂Ȃ�Ȃ��cbindingFlags ���g�p
		/// </param>
		public ManagedDelegate(System.Type type,string methodName):this(null,type,methodName){}
		//===========================================================
		//		Invoke
		//===========================================================
		public override Object Invoke(JavaScript.Object dummy, Array arguments){
			System.Reflection.MethodInfo m;
			try{
				m=this.SelectOverload(arguments);
			}catch(System.Exception e){throw e;}
			object ret=m.Invoke(this.obj,FunctionBase.ConvertParams(m,arguments));
			return Global.ConvertFromManaged(ret);
		}
	}
	/// <summary>
	/// Javascript.Object �������Ɏ��񍀉��Z���\�b�h�� .NET ������\�����܂��B
	/// �I�[�o�[���[�h�ɂ͑Ή����Ă��܂���B
	/// </summary>
	public class ManagedJSBinaryOperator:FunctionBase{
		System.Reflection.MethodInfo minfo;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m">
		/// Javascript.Object �̓񍀉��Z�����s���郁�\�b�h�̊֐��ł��B
		/// �Ԓl�� Javascript.Object �y�т�����p������I�u�W�F�N�g�ł���K�v������܂��B
		/// <para>�C���X�^���X���\�b�h�̏ꍇ:
		/// �֐��͈�p�����[�^�������AJavascript.Object ���w�肷�鎖���o����K�v������܂��B
		/// ����́A�C���X�^���X�쐬���Ɋm�F����܂��B
		/// ����ŁAthis �Ɏw�肷��ϐ� (���\�b�h��ێ�����C���X�^���X) �̌^�͎��s���Ɋm�F����܂��B
		/// </para>
		/// <para>�ÓI���\�b�h�̏ꍇ:
		/// �֐��͓�p�����[�^�������A���� Javascript.Object ���w�肷�鎖���o����K�v������܂��B
		/// ����́A�C���X�^���X�쐬���Ɋm�F����܂��B
		/// </para>
		/// </param>
		public ManagedJSBinaryOperator(System.Reflection.MethodInfo m){
			this.minfo=m;
			if(!Global.IsCastable(m.ReturnType,typeof(JavaScript.Object)))goto err;
			System.Reflection.ParameterInfo[] pinfos=this.minfo.GetParameters();
			if(this.minfo.IsStatic){
				if(pinfos.Length!=2)goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[0].ParameterType)) goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[1].ParameterType)) goto err;
			}else{
				if(pinfos.Length!=1)goto err;
				if(!Global.IsCastable(typeof(JavaScript.Object),pinfos[0].ParameterType)) goto err;
			}
			return;
		err:
			throw new System.ApplicationException("ManagedJSBinaryOperator: �w�肵�����\�b�h�� Javascript.Object �������Ɏ��񍀉��Z�Ƃ��ĕs�K�؂ł��B");
		}
		public override Object Invoke(Object obj,Array arguments) {
			if(arguments.length>0)return InvokeBinaryOperator(obj,arguments[0]);
			else throw new System.ArgumentException("�����͏��Ȃ��Ƃ���͕K�v�ł��B","arguments");
		}
		public JavaScript.Object InvokeBinaryOperator(JavaScript.Object left,JavaScript.Object right){
			if(this.minfo.IsStatic){
				return (JavaScript.Object)this.minfo.Invoke(null,new object[]{left,right});
			}else{
				if(!Global.IsCastable(left.GetType(),this.minfo.DeclaringType))
					throw new System.ArgumentException("ManagedJSBinaryOperator: �w�肵�������� this �p�����[�^�̌^�ɍ��v���܂���B","left");
				return (JavaScript.Object)this.minfo.Invoke(left,new object[]{right});
			}
		}
	}
}
