using Emit=System.Reflection.Emit;
using Ref=System.Reflection;
using Gen=System.Collections.Generic;

namespace afh.Reflection{
	/// <summary>
	/// 動的なメソッドを生成する補助を行います。
	/// </summary>
	/// <typeparam name="T">メソッドを保持する型を指定します。</typeparam>
	/// <typeparam name="D">メソッドのデリゲート型を指定します。</typeparam>
	public sealed class DynamicMethodCreater<T,D>:ILGeneratorHelper{
		Emit::DynamicMethod m;
		bool is_static;
		/// <summary>
		/// DynamicMethodCreator のインスタンスを作成します。
		/// </summary>
		/// <param name="name">メソッドの名前を指定します。</param>
		/// <param name="is_static">静的メソッドであるか否かを指定します。</param>
		public DynamicMethodCreater(string name,bool is_static):base(null){
			this.is_static=is_static;
			this.m=CreateDynamicMethod(name);
			this.gen=this.m.GetILGenerator();
		}

		private Emit::DynamicMethod CreateDynamicMethod(string name){
			if(!typeof(System.Delegate).IsAssignableFrom(typeof(D)))
				throw new System.InvalidProgramException("DynamicMethodCreator の第二型引数はデリゲート型である必要があります。");
			Ref::MethodInfo minfo=typeof(D).GetMethod("Invoke");
			
			// 引数の型
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
		/// 作成した関数をデリゲートとして返します。
		/// </summary>
		/// <param name="instance">インスタンスメソッドの場合にはインスタンスを指定します。
		/// 静的メソッドの場合にはこの引数は無視されます。</param>
		/// <returns>作成した関数を呼び出す為のデリゲートを返します。</returns>
		public D Instantiate(T instance){
			if(is_static){
				return (D)(object)this.m.CreateDelegate(typeof(D));
			}else{
				return (D)(object)this.m.CreateDelegate(typeof(D),instance);
			}
		}
	}
	/// <summary>
	/// ILGenerator 使用の補助を行います。
	/// </summary>
	public class ILGeneratorHelper{
		/// <summary>
		/// 出力先の ILGenerator を保持します。
		/// </summary>
		protected Emit::ILGenerator gen;
		/// <summary>
		/// 指定した ILGenerator を使用して ILGeneratorHelper を初期化します。
		/// </summary>
		/// <param name="gen">出力対象の ILGenerator を指定します。</param>
		public ILGeneratorHelper(Emit::ILGenerator gen){
			this.gen=gen;
		}

		/// <summary>
		/// ラベルを作成します。
		/// </summary>
		/// <returns>作成したラベルを返します。</returns>
		public Emit::Label CreateLabel(){
			return gen.DefineLabel();
		}
		/// <summary>
		/// ラベルを出力します。
		/// </summary>
		/// <param name="label">出力するラベルを指定します。</param>
		public void MarkLabel(Emit::Label label){
			gen.MarkLabel(label);
		}

		#region Emit Ld***
		/// <summary>
		/// 指定した番号の引数をスタックに積みます。
		/// </summary>
		/// <param name="index">引数の番号を指定します。</param>
		public void EmitLdarg(short index){
			if(index<0)
				throw new System.ArgumentOutOfRangeException("index","ldarg: 引数の番号は 0 以上である必要があります。");
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
		/// 即値をスタックに積みます。
		/// </summary>
		/// <param name="value">スタックに積む値を指定します。</param>
		public void EmitLdc(byte value){
			this.EmitLdc((int)value);
		}
		/// <summary>
		/// 即値をスタックに積みます。
		/// </summary>
		/// <param name="value">スタックに積む値を指定します。</param>
		public void EmitLdc(char value){
			this.EmitLdc((int)value);
		}
		/// <summary>
		/// 即値をスタックに積みます。
		/// </summary>
		/// <param name="value">スタックに積む値を指定します。</param>
		public void EmitLdc(bool value){
			gen.Emit(value?Emit::OpCodes.Ldc_I4_1:Emit::OpCodes.Ldc_I4_0);
		}
		/// <summary>
		/// 即値をスタックに積みます。
		/// </summary>
		/// <param name="i">スタックに積む値を指定します。</param>
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
		/// 指定したフィールドの値を取得します。
		/// </summary>
		/// <param name="type">フィールドを保持している型を指定します。</param>
		/// <param name="fieldName">フィールドの名前を指定します。</param>
		/// <param name="is_static">静的フィールドか否かを指定します。</param>
		/// <param name="is_private">非公開メンバか否かを指定します。</param>
		public void EmitLdfld(System.Type type,string fieldName,bool is_static,bool is_private){
			Ref::BindingFlags BINDING
				=(is_private?Ref::BindingFlags.NonPublic:Ref::BindingFlags.Public)
				|(is_static?Ref::BindingFlags.Static:Ref::BindingFlags.Instance);
			Ref::FieldInfo finfo=type.GetField(fieldName,BINDING);
			gen.Emit(Emit::OpCodes.Ldfld,finfo);
		}
		/// <summary>
		/// 指定した型の、配列要素をスタックに載せます。
		/// </summary>
		/// <param name="type">配列要素の型を指定します。</param>
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
				//	? 合っているのかどうか不明 ?
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
		/// メソッド呼び出しを出力します。
		/// </summary>
		/// <param name="type">メソッドが宣言されているクラスを指定します。</param>
		/// <param name="is_private">メソッドが private/internal であるか否かを指定します。</param>
		/// <param name="is_static">静的メソッドであるか否かを指定します。</param>
		/// <param name="methodName">メソッドの名前を指定します。</param>
		/// <param name="param_types">メソッドの引数の型を指定します。</param>
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
		/// メソッド呼出を出力します。
		/// </summary>
		/// <param name="minfo">呼び出すメソッドを指定します。</param>
		public void EmitCall(Ref::MethodInfo minfo){
			gen.Emit(Emit::OpCodes.Call,minfo);
		}
		/// <summary>
		/// 仮想関数呼び出しを出力します。
		/// </summary>
		/// <param name="type">仮想メソッドが宣言されているクラスを指定します。</param>
		/// <param name="is_private">仮想メソッドが private/internal であるか否かを指定します。</param>
		/// <param name="is_static">静的メソッドであるか否かを指定します。</param>
		/// <param name="methodName">仮想メソッドの名前を指定します。</param>
		/// <param name="param_types">仮想メソッドの引数の型を指定します。</param>
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

		#region 分岐命令
		/// <summary>
		/// brtrue 命令を出力します。スタックのトップが真である時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBrtrue(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brtrue,label);

			// ↓は label との距離が分からないので使えるかどうか分からない。
			// gen.Emit(Emit::OpCodes.Brtrue_S,label);
		}
		/// <summary>
		/// brfalse 命令を出力します。スタックのトップが偽である時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBrfalse(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brfalse,label);
		}
		/// <summary>
		/// br 命令を出力します。無条件で分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBr(Emit::Label label){
			gen.Emit(Emit::OpCodes.Br,label);
		}
		/// <summary>
		/// beq 命令を出力します。スタックの上の二つの値が等しい時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBeq(Emit::Label label){
			gen.Emit(Emit::OpCodes.Beq,label);
		}
		/// <summary>
		/// bne 命令を出力します。スタックの上の二つの値が異なる時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBne(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bne_Un,label);
		}

		/// <summary>
		/// blt.un 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも小さい時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBltUn(Emit::Label label) {
			gen.Emit(Emit::OpCodes.Blt_Un,label);
		}
		/// <summary>
		/// ble.un 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値以下の時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBleUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_Un,label);
		}
		/// <summary>
		/// bgt.un 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも大きい時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgtUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_Un,label);
		}
		/// <summary>
		/// bge.un 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値以上の時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgeUn(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_Un,label);
		}

		/// <summary>
		/// blt 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも小さい時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBlt(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt,label);
		}
		/// <summary>
		/// ble 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値以下の時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBle(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble,label);
		}
		/// <summary>
		/// bgt 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも大きい時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgt(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt,label);
		}
		/// <summary>
		/// bge 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値以上の時に分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBge(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge,label);
		}
		//------------------------------------------------------------
		//		短形式
		//------------------------------------------------------------
		/// <summary>
		/// brtrue 命令を出力します。スタックのトップが真である時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBrtrueS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brtrue_S,label);
		}
		/// <summary>
		/// brfalse 命令を出力します。スタックのトップが偽である時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBrfalseS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Brfalse_S,label);
		}
		/// <summary>
		/// br 命令を出力します。無条件で、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBrS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Br_S,label);
		}
		/// <summary>
		/// beq 命令を出力します。スタックの上の二つの値が等しい時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBeqS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Beq_S,label);
		}
		/// <summary>
		/// bne 命令を出力します。スタックの上の二つの値が異なる時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBneS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bne_Un_S,label);
		}

		/// <summary>
		/// blt.un.s 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも小さい時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBltUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt_Un_S,label);
		}
		/// <summary>
		/// ble.un.s 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値以下の時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBleUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_Un_S,label);
		}
		/// <summary>
		/// bgt.un.s 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも大きい時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgtUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_Un_S,label);
		}
		/// <summary>
		/// bge.un.s 命令を出力します。スタックの上の二つの値を符号無し整数として比較して、
		/// 一つ目の値の方が二つ目の値以上の時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgeUnS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_Un_S,label);
		}

		/// <summary>
		/// blt.s 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも小さい時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBltS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Blt_S,label);
		}
		/// <summary>
		/// ble.s 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値以下の時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBleS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Ble_S,label);
		}
		/// <summary>
		/// bgt.s 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値よりも大きい時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgtS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bgt_S,label);
		}
		/// <summary>
		/// bge.s 命令を出力します。スタックの上の二つの値を符号付き整数として比較して、
		/// 一つ目の値の方が二つ目の値以上の時に、近くに分岐を行います。
		/// </summary>
		/// <param name="label">分岐先のラベルを指定します。</param>
		public void EmitBgeS(Emit::Label label){
			gen.Emit(Emit::OpCodes.Bge_S,label);
		}
		#endregion

		/// <summary>
		/// スタックからデータを除去します。
		/// </summary>
		public void EmitPop(){
			gen.Emit(Emit::OpCodes.Pop);
		}
		/// <summary>
		/// 関数を抜けます。
		/// 戻り値がある場合には、スタックのトップからデータを取って戻り値にします。
		/// </summary>
		public void EmitRet(){
			gen.Emit(Emit::OpCodes.Ret);
		}
	}
}