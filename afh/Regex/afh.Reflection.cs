using Emit=System.Reflection.Emit;
using Ref=System.Reflection;
using Gen=System.Collections.Generic;

namespace afh.Reflection{
	/// <summary>
	/// ���I�ȃ��\�b�h�𐶐�����⏕���s���܂��B
	/// </summary>
	/// <typeparam name="T">���\�b�h��ێ�����^���w�肵�܂��B</typeparam>
	/// <typeparam name="D">���\�b�h�̃f���Q�[�g�^���w�肵�܂��B</typeparam>
	public sealed class DynamicMethodCreater<T,D>:ILGeneratorHelper{
		Emit::DynamicMethod m;
		bool is_static;
		/// <summary>
		/// DynamicMethodCreator �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="name">���\�b�h�̖��O���w�肵�܂��B</param>
		/// <param name="is_static">�ÓI���\�b�h�ł��邩�ۂ����w�肵�܂��B</param>
		public DynamicMethodCreater(string name,bool is_static):base(null){
			this.is_static=is_static;
			this.m=CreateDynamicMethod(name);
			this.gen=this.m.GetILGenerator();
		}

		private Emit::DynamicMethod CreateDynamicMethod(string name){
			if(!typeof(System.Delegate).IsAssignableFrom(typeof(D)))
				throw new System.InvalidProgramException("DynamicMethodCreator �̑��^�����̓f���Q�[�g�^�ł���K�v������܂��B");
			Ref::MethodInfo minfo=typeof(D).GetMethod("Invoke");
			
			// �����̌^
			Gen::List<System.Type> param_types=new System.Collections.Generic.List<System.Type>();
			if(!is_static)param_types.Add(typeof(T));
			foreach(Ref::ParameterInfo p in minfo.GetParameters())
				param_types.Add(p.ParameterType);

			return new Emit::DynamicMethod(
				typeof(T).FullName+"::"+name,
				minfo.ReturnType,param_types.ToArray(),typeof(T)
				);
		}
		/// <summary>
		/// �쐬�����֐����f���Q�[�g�Ƃ��ĕԂ��܂��B
		/// </summary>
		/// <param name="instance">�C���X�^���X���\�b�h�̏ꍇ�ɂ̓C���X�^���X���w�肵�܂��B
		/// �ÓI���\�b�h�̏ꍇ�ɂ͂��̈����͖�������܂��B</param>
		/// <returns>�쐬�����֐����Ăяo���ׂ̃f���Q�[�g��Ԃ��܂��B</returns>
		public D Instantiate(T instance){
			if(is_static){
				return (D)(object)this.m.CreateDelegate(typeof(D));
			}else{
				return (D)(object)this.m.CreateDelegate(typeof(D),instance);
			}
		}
	}
	/// <summary>
	/// ILGenerator �g�p�̕⏕���s���܂��B
	/// </summary>
	public class ILGeneratorHelper{
		/// <summary>
		/// �o�͐�� ILGenerator ��ێ����܂��B
		/// </summary>
		protected Emit::ILGenerator gen;
		/// <summary>
		/// �w�肵�� ILGenerator ���g�p���� ILGeneratorHelper �����������܂��B
		/// </summary>
		/// <param name="gen">�o�͑Ώۂ� ILGenerator ���w�肵�܂��B</param>
		public ILGeneratorHelper(Emit::ILGenerator gen){
			this.gen=gen;
		}

		/// <summary>
		/// ���x�����쐬���܂��B
		/// </summary>
		/// <returns>�쐬�������x����Ԃ��܂��B</returns>
		public Emit::Label CreateLabel(){
			return gen.DefineLabel();
		}
		/// <summary>
		/// ���x�����o�͂��܂��B
		/// </summary>
		/// <param name="label">�o�͂��郉�x�����w�肵�܂��B</param>
		public void MarkLabel(Emit::Label label){
			gen.MarkLabel(label);
		}

		#region Emit Ld***
		/// <summary>
		/// �w�肵���ԍ��̈������X�^�b�N�ɐς݂܂��B
		/// </summary>
		/// <param name="index">�����̔ԍ����w�肵�܂��B</param>
		public void EmitLdarg(short index){
			if(index<0)
				throw new System.ArgumentOutOfRangeException("index","ldarg: �����̔ԍ��� 0 �ȏ�ł���K�v������܂��B");
			switch(index){
				case 0:
					gen.Emit(Emit::OpCodes.Ldarg_0);
					break;
				case 1:
					gen.Emit(Emit::OpCodes.Ldarg_1);
					break;
				case 2:
					gen.Emit(Emit::OpCodes.Ldarg_2);
					break;
				case 3:
					gen.Emit(Emit::OpCodes.Ldarg_3);
					break;
				default:
					if(index<=225)
						gen.Emit(Emit::OpCodes.Ldarg_S,(byte)index);
					else
						gen.Emit(Emit::OpCodes.Ldarg,index);
					break;
			}
		}
		/// <summary>
		/// ���l���X�^�b�N�ɐς݂܂��B
		/// </summary>
		/// <param name="value">�X�^�b�N�ɐςޒl���w�肵�܂��B</param>
		public void EmitLdc(byte value){
			this.EmitLdc((int)value);
		}
		/// <summary>
		/// ���l���X�^�b�N�ɐς݂܂��B
		/// </summary>
		/// <param name="value">�X�^�b�N�ɐςޒl���w�肵�܂��B</param>
		public void EmitLdc(char value){
			this.EmitLdc((int)value);
		}
		/// <summary>
		/// ���l���X�^�b�N�ɐς݂܂��B
		/// </summary>
		/// <param name="value">�X�^�b�N�ɐςޒl���w�肵�܂��B</param>
		public void EmitLdc(bool value){
			gen.Emit(value?Emit::OpCodes.Ldc_I4_1:Emit::OpCodes.Ldc_I4_0);
		}
		/// <summary>
		/// ���l���X�^�b�N�ɐς݂܂��B
		/// </summary>
		/// <param name="i">�X�^�b�N�ɐςޒl���w�肵�܂��B</param>
		public void EmitLdc(int i){
			switch(i){
				case 0:
					gen.Emit(Emit::OpCodes.Ldc_I4_0);
					break;
				case 1:
					gen.Emit(Emit::OpCodes.Ldc_I4_1);
					break;
				case 2:
					gen.Emit(Emit::OpCodes.Ldc_I4_2);
					break;
				case 3:
					gen.Emit(Emit::OpCodes.Ldc_I4_3);
					break;
				case 4:
					gen.Emit(Emit::OpCodes.Ldc_I4_4);
					break;
				case 5:
					gen.Emit(Emit::OpCodes.Ldc_I4_5);
					break;
				case 6:
					gen.Emit(Emit::OpCodes.Ldc_I4_6);
					break;
				case 7:
					gen.Emit(Emit::OpCodes.Ldc_I4_7);
					break;
				case 8:
					gen.Emit(Emit::OpCodes.Ldc_I4_8);
					break;
				default:
					gen.Emit(Emit::OpCodes.Ldc_I4,i);
					break;
			}
		}
		/// <summary>
		/// �w�肵���t�B�[���h�̒l���擾���܂��B
		/// </summary>
		/// <param name="type">�t�B�[���h��ێ����Ă���^���w�肵�܂��B</param>
		/// <param name="fieldName">�t�B�[���h�̖��O���w�肵�܂��B</param>
		/// <param name="is_static">�ÓI�t�B�[���h���ۂ����w�肵�܂��B</param>
		/// <param name="is_private">����J�����o���ۂ����w�肵�܂��B</param>
		public void EmitLdfld(System.Type type,string fieldName,bool is_static,bool is_private){
			Ref::BindingFlags BINDING
				=(is_private?Ref::BindingFlags.NonPublic:Ref::BindingFlags.Public)
				|(is_static?Ref::BindingFlags.Static:Ref::BindingFlags.Instance);
			Ref::FieldInfo finfo=type.GetField(fieldName,BINDING);
			gen.Emit(Emit::OpCodes.Ldfld,finfo);
		}
		/// <summary>
		/// �w�肵���^�́A�z��v�f���X�^�b�N�ɍڂ��܂��B
		/// </summary>
		/// <param name="type">�z��v�f�̌^���w�肵�܂��B</param>
		public void EmitLdelem(System.Type type){
			switch(afh.Types.GetTypeCode(type)){
				case TypeCodes.SByte:
					gen.Emit(Emit::OpCodes.Ldelem_I1);
					break;
				case TypeCodes.Short:
					gen.Emit(Emit::OpCodes.Ldelem_I2);
					break;
				case TypeCodes.Int:
					gen.Emit(Emit::OpCodes.Ldelem_I4);
					break;
				case TypeCodes.Long:
					gen.Emit(Emit::OpCodes.Ldelem_I8);
					break;
				case TypeCodes.Byte:
					gen.Emit(Emit::OpCodes.Ldelem_U1);
					break;
				case TypeCodes.UShort:
					gen.Emit(Emit::OpCodes.Ldelem_U2);
					break;
				case TypeCodes.UInt:
					gen.Emit(Emit::OpCodes.Ldelem_U4);
					break;
				case TypeCodes.Float:
					gen.Emit(Emit::OpCodes.Ldelem_R4);
					break;
				case TypeCodes.Double:
					gen.Emit(Emit::OpCodes.Ldelem_R8);
					break;
				//==========================================
				//	? �����Ă���̂��ǂ����s�� ?
				//==========================================
				case TypeCodes.ULong:
					gen.Emit(Emit::OpCodes.Ldelem_I8);
					break;
				case TypeCodes.Bool: // OK
					gen.Emit(Emit::OpCodes.Ldelem_I1);
					break;
				default:
					if(type.IsValueType)
						gen.Emit(Emit::OpCodes.Ldelem,type);
					else
						gen.Emit(Emit::OpCodes.Ldelem_Ref);
					break;
			}
		}
		#endregion

		/// <summary>
		/// ���\�b�h�Ăяo�����o�͂��܂��B
		/// </summary>
		/// <param name="type">���\�b�h���錾����Ă���N���X���w�肵�܂��B</param>
		/// <param name="is_private">���\�b�h�� private/internal �ł��邩�ۂ����w�肵�܂��B</param>
		/// <param name="is_static">�ÓI���\�b�h�ł��邩�ۂ����w�肵�܂��B</param>
		/// <param name="methodName">���\�b�h�̖��O���w�肵�܂��B</param>
		/// <param name="param_types">���\�b�h�̈����̌^���w�肵�܂��B</param>
		public void EmitCall(
			System.Type type,bool is_private,bool is_static,
			string methodName,params System.Type[] param_types
		){
			Ref::BindingFlags BINDING
				=(is_private?Ref::BindingFlags.NonPublic:Ref::BindingFlags.Public)
				|(is_static?Ref::BindingFlags.Static:Ref::BindingFlags.Instance);
			Ref::MethodInfo method=type.GetMethod(methodName,BINDING,null,param_types,null);
			gen.Emit(Emit::OpCodes.Call,method);
		}
		/// <summary>
		/// ���\�b�h�ďo���o�͂��܂��B
		/// </summary>
		/// <param name="minfo">�Ăяo�����\�b�h���w�肵�܂��B</param>
		public void EmitCall(Ref::MethodInfo minfo){
			gen.Emit(Emit::OpCodes.Call,minfo);
		}
		/// <summary>
		/// ���z�֐��Ăяo�����o�͂��܂��B
		/// </summary>
		/// <param name="type">���z���\�b�h���錾����Ă���N���X���w�肵�܂��B</param>
		/// <param name="is_private">���z���\�b�h�� private/internal �ł��邩�ۂ����w�肵�܂��B</param>
		/// <param name="is_static">�ÓI���\�b�h�ł��邩�ۂ����w�肵�܂��B</param>
		/// <param name="methodName">���z���\�b�h�̖��O���w�肵�܂��B</param>
		/// <param name="param_types">���z���\�b�h�̈����̌^���w�肵�܂��B</param>
		public void EmitCallvirt(
			System.Type type,bool is_private,bool is_static,
			string methodName,params System.Type[] param_types
		){
			Ref::BindingFlags BINDING
				=(is_private?Ref::BindingFlags.NonPublic:Ref::BindingFlags.Public)
				|(is_static?Ref::BindingFlags.Static:Ref::BindingFlags.Instance);
			Ref::MethodInfo method=type.GetMethod(methodName,BINDING,null,param_types,null);
			gen.Emit(Emit::OpCodes.Callvirt,method);
		}

		#region ���򖽗�
		/// <summary>
		/// brtrue ���߂��o�͂��܂��B�X�^�b�N�̃g�b�v���^�ł��鎞�ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBrtrue(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brtrue,label);

			// ���� label �Ƃ̋�����������Ȃ��̂Ŏg���邩�ǂ���������Ȃ��B
			// gen.Emit(Emit::OpCodes.Brtrue_S,label);
		}
		/// <summary>
		/// brfalse ���߂��o�͂��܂��B�X�^�b�N�̃g�b�v���U�ł��鎞�ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBrfalse(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brfalse,label);
		}
		/// <summary>
		/// br ���߂��o�͂��܂��B�������ŕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBr(Emit::Label label){
			gen.Emit(Emit::OpCodes.Br,label);
		}
		/// <summary>
		/// beq ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�����������ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBeq(Emit::Label label){
			gen.Emit(Emit::OpCodes.Beq,label);
		}
		/// <summary>
		/// bne ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l���قȂ鎞�ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBne(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bne_Un,label);
		}

		/// <summary>
		/// blt.un ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�������������ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBltUn(Emit::Label label) {
			gen.Emit(Emit::OpCodes.Blt_Un,label);
		}
		/// <summary>
		/// ble.un ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȉ��̎��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBleUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_Un,label);
		}
		/// <summary>
		/// bgt.un ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�����傫�����ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgtUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_Un,label);
		}
		/// <summary>
		/// bge.un ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȏ�̎��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgeUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_Un,label);
		}

		/// <summary>
		/// blt ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�������������ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBlt(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt,label);
		}
		/// <summary>
		/// ble ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȉ��̎��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBle(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble,label);
		}
		/// <summary>
		/// bgt ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�����傫�����ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgt(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt,label);
		}
		/// <summary>
		/// bge ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȏ�̎��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBge(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge,label);
		}
		//------------------------------------------------------------
		//		�Z�`��
		//------------------------------------------------------------
		/// <summary>
		/// brtrue ���߂��o�͂��܂��B�X�^�b�N�̃g�b�v���^�ł��鎞�ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBrtrueS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brtrue_S,label);
		}
		/// <summary>
		/// brfalse ���߂��o�͂��܂��B�X�^�b�N�̃g�b�v���U�ł��鎞�ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBrfalseS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brfalse_S,label);
		}
		/// <summary>
		/// br ���߂��o�͂��܂��B�������ŁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBrS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Br_S,label);
		}
		/// <summary>
		/// beq ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�����������ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBeqS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Beq_S,label);
		}
		/// <summary>
		/// bne ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l���قȂ鎞�ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBneS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bne_Un_S,label);
		}

		/// <summary>
		/// blt.un.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�������������ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBltUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt_Un_S,label);
		}
		/// <summary>
		/// ble.un.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȉ��̎��ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBleUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_Un_S,label);
		}
		/// <summary>
		/// bgt.un.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�����傫�����ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgtUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_Un_S,label);
		}
		/// <summary>
		/// bge.un.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍����������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȏ�̎��ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgeUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_Un_S,label);
		}

		/// <summary>
		/// blt.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�������������ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBltS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt_S,label);
		}
		/// <summary>
		/// ble.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȉ��̎��ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBleS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_S,label);
		}
		/// <summary>
		/// bgt.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�����傫�����ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgtS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_S,label);
		}
		/// <summary>
		/// bge.s ���߂��o�͂��܂��B�X�^�b�N�̏�̓�̒l�𕄍��t�������Ƃ��Ĕ�r���āA
		/// ��ڂ̒l�̕�����ڂ̒l�ȏ�̎��ɁA�߂��ɕ�����s���܂��B
		/// </summary>
		/// <param name="label">�����̃��x�����w�肵�܂��B</param>
		public void EmitBgeS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_S,label);
		}
		#endregion

		/// <summary>
		/// �X�^�b�N����f�[�^���������܂��B
		/// </summary>
		public void EmitPop(){
			gen.Emit(Emit::OpCodes.Pop);
		}
		/// <summary>
		/// �֐��𔲂��܂��B
		/// �߂�l������ꍇ�ɂ́A�X�^�b�N�̃g�b�v����f�[�^������Ė߂�l�ɂ��܂��B
		/// </summary>
		public void EmitRet(){
			gen.Emit(Emit::OpCodes.Ret);
		}
	}
}