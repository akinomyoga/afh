using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Rgx=System.Text.RegularExpressions;

namespace afh{
	/// <summary>
	/// 型の種類を特定する為のコードを表現します。
	/// </summary>
	public enum TypeCodes{
		/// <summary>
		/// 未登録の型である事を示します。
		/// </summary>
		Unknown=-1,
		//===========================================================
		//		C# Natives
		//===========================================================
		/// <summary>
		/// System.Void を表現します。
		/// </summary>
		Void,
		/// <summary>
		/// System.Byte を表現します。
		/// </summary>
		Byte,
		/// <summary>
		/// System.SByte を表現します。
		/// </summary>
		SByte,
		/// <summary>
		/// System.Int16 を表現します。
		/// </summary>
		Short,
		/// <summary>
		/// System.UInt16 を表現します。
		/// </summary>
		UShort,
		/// <summary>
		/// System.Int32 を表現します。
		/// </summary>
		Int,
		/// <summary>
		/// System.UInt32 を表現します。
		/// </summary>
		UInt,
		/// <summary>
		/// System.Int64 を表現します。
		/// </summary>
		Long,
		/// <summary>
		/// System.UInt64 を表現します。
		/// </summary>
		ULong,
		/// <summary>
		/// System.Single を表現します。
		/// </summary>
		Float,
		/// <summary>
		/// System.Double を表現します。
		/// </summary>
		Double,
		/// <summary>
		/// System.Char を表現します。
		/// </summary>
		Char,
		/// <summary>
		/// System.Boolean を表現します。
		/// </summary>
		Bool,
		/// <summary>
		/// System.Decimal を表現します。
		/// </summary>
		Decimal,
		/// <summary>
		/// System.String を表現します。
		/// </summary>
		String,
		/// <summary>
		/// System.Object を表現します。
		/// </summary>
		Object,
		//===========================================================
		//		よく使う物
		//===========================================================
		/// <summary>
		/// System.Type を表現します。
		/// </summary>
		Type,
		/// <summary>
		/// System.IntPtr を表現します。
		/// </summary>
		IntPtr,
		/// <summary>
		/// System.UIntPtr を表現します。
		/// </summary>
		UIntPtr,
		/// <summary>
		/// System.DateTime を表現します。
		/// </summary>
		DateTime,
		/// <summary>
		/// System.TimeSpan を表現します。
		/// </summary>
		TimeSpan,

		/// <summary>
		/// System.Byte[] を表現します。
		/// </summary>
		ByteArray,
		/// <summary>
		/// System.Bool[] を表現します。
		/// </summary>
		BoolArray,
		/// <summary>
		/// System.Int32[] を表現します。
		/// </summary>
		IntArray,

		/// <summary>
		/// System.Guid を表現します。
		/// </summary>
		Guid,
		/// <summary>
		/// System.Version を表現します。
		/// </summary>
		Version,
	}

	/// <summary>
	/// System.Type の文字列表現に対する操作を提供します。
	/// </summary>
	public static class Types{
		static Types(){
			type_dic[typeof(void)]		=TypeCodes.Void;
			type_dic[typeof(sbyte)]		=TypeCodes.SByte;
			type_dic[typeof(byte)]		=TypeCodes.Byte;
			type_dic[typeof(short)]		=TypeCodes.Short;
			type_dic[typeof(ushort)]	=TypeCodes.UShort;
			type_dic[typeof(int)]		=TypeCodes.Int;
			type_dic[typeof(uint)]		=TypeCodes.UInt;
			type_dic[typeof(long)]		=TypeCodes.Long;
			type_dic[typeof(ulong)]		=TypeCodes.ULong;
			type_dic[typeof(float)]		=TypeCodes.Float;
			type_dic[typeof(double)]	=TypeCodes.Double;
			type_dic[typeof(char)]		=TypeCodes.Char;
			type_dic[typeof(bool)]		=TypeCodes.Bool;
			type_dic[typeof(decimal)]	=TypeCodes.Decimal;
			type_dic[typeof(string)]	=TypeCodes.String;
			type_dic[typeof(object)]	=TypeCodes.Object;
			type_dic[typeof(System.IntPtr)]	=TypeCodes.IntPtr;
			type_dic[typeof(System.UIntPtr)]=TypeCodes.UIntPtr;
			type_dic[typeof(byte[])]		=TypeCodes.ByteArray;
			type_dic[typeof(bool[])]		=TypeCodes.BoolArray;
			type_dic[typeof(int[])]			=TypeCodes.IntArray;
		}
		//===========================================================
		//		CSharpName
		//===========================================================
		/// <summary>
		/// メソッドのシグニチャを説明する文字列を C# 形式で取得します。
		/// </summary>
		/// <param name="m">メソッドを指定します。</param>
		/// <returns>メソッドを C# 形式で表現した文字列を返します。</returns>
		public static string CSharpName(Ref::MethodBase m){
			string ret=CSharpName(m.DeclaringType)+"."+m.Name;
			if(m.IsGenericMethod){
				System.Type[] args=m.GetGenericArguments();
				string gen=CSharpName(args[0]);
				for(int i=1;i<args.Length;i++)
					gen+=","+CSharpName(args[i]);
				return ret+"<"+gen+">";
			}else return ret;
		}
		/// <summary>
		/// System.Type を表す文字列を C# 形式で取得します。
		/// </summary>
		/// <param name="t">文字列に直す前の System.Type を指定します。</param>
		/// <returns>System.Type を文字列で表した物を取得します。</returns>
		public static string CSharpName(System.Type t){
			if(t.IsArray)return CSharpName(t.GetElementType())+"["+new string(',',t.GetArrayRank()-1)+"]";
			if(t.IsPointer)return CSharpName(t.GetElementType())+"*";
			if(t.IsByRef)return CSharpName(t.GetElementType())+"&";
			if(t.IsGenericType){
				if(t.GetGenericTypeDefinition()==typeof(System.Nullable<>))
					return CSharpName(t.GetGenericArguments()[0])+"?";
				string temp=CSharpName_gen(t);
				System.Type[] args=t.GetGenericArguments();
				int i=0;
				return CSharpName_rxGen.Replace(temp,delegate(Rgx::Match m){
					int M=int.Parse(m.Groups[1].Value);
					if(M==0||i+M>args.Length)return m.Value;

					string ret=CSharpName(args[i++]);
					for(int j=1;j<M;j++){
						ret+=","+CSharpName(args[i++]);
					}
					return "<"+ret+">";
				});
			}
			if(t.IsGenericParameter)return t.Name;

			switch(GetTypeCode(t)){
				case TypeCodes.Void:return "void";
				case TypeCodes.SByte:return "sbyte";
				case TypeCodes.Byte:return "byte";
				case TypeCodes.Short:return "short";
				case TypeCodes.UShort:return "ushort";
				case TypeCodes.Int:return "int";
				case TypeCodes.UInt:return "uint";
				case TypeCodes.Long:return "long";
				case TypeCodes.ULong:return "ulong";
				case TypeCodes.Float:return "float";
				case TypeCodes.Double:return "double";
				case TypeCodes.Decimal:return "decimal";
				case TypeCodes.Char:return "char";
				case TypeCodes.Bool:return "bool";
				case TypeCodes.String:return "string";
				case TypeCodes.Object:return "object";
				default:return t.FullName;
			}
		}
		private static readonly Rgx::Regex CSharpName_rxGen=new Rgx::Regex(@"\`(\d+)",Rgx::RegexOptions.Compiled);
		private static string CSharpName_gen(System.Type t){
			if(!t.IsNested)return t.Namespace+"."+t.Name;
			return CSharpName_gen(t.DeclaringType)+"+"+t.Name;
		}
		/// <summary>
		/// パラメータのリストを文字列にして取得します。
		/// </summary>
		/// <param name="m">メソッドを表す System.Reflection.MethodBase を指定して下さい。</param>
		/// <returns>パラメータのリストを表す文字列を返します。</returns>
		public static string GetParameterList(System.Reflection.MethodBase m){
			System.Reflection.ParameterInfo[] ps=m.GetParameters();
			System.Text.StringBuilder build=new System.Text.StringBuilder();
			for(int i=0,iM=ps.Length;i<iM;i++){
				if(i!=0)build.Append(',');

				System.Type t=ps[i].ParameterType;
				if(t.IsByRef){
					build.AppendFormat("{0} {1}",ps[i].IsOut?"out":"ref",CSharpName(t.GetElementType()));
				}else{
					if(ps[i].GetCustomAttributes(typeof(System.ParamArrayAttribute),false).Length>0){
						build.Append("params ");
					}
					build.Append(CSharpName(t));
				}
				build.Append(' ');
				build.Append(ps[i].Name);
			}
			return build.ToString();
		}
		/// <summary>
		/// メソッドの名前及びシグニチャ情報を文字列として取得します。
		/// </summary>
		/// <param name="meth">情報を文字列にするメソッドを指定します。</param>
		/// <returns>メソッドの情報を文字列にして返します。</returns>
		public static string GetMethodSignature(Ref::MethodBase meth){
			//string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
			string ret="";
			
			if(meth.IsStatic)ret+="static ";

			Ref::MethodInfo minfo=meth as Ref::MethodInfo;
			if(minfo!=null)ret+=CSharpName(minfo.ReturnType)+" ";
			
			//"<"+dll+"> "+
			return ret+CSharpName(meth)+"("+Types.GetParameterList(meth)+");";
		}
		//===========================================================
		//		Type 分岐
		//===========================================================
		/// <summary>
		/// System.Type と TypeCodes との対応を付ける為に使用します。
		/// </summary>
		public static readonly Gen::Dictionary<System.Type,TypeCodes> type_dic=new Gen::Dictionary<System.Type,TypeCodes>(20);
		/// <summary>
		/// System.Type を type_dic で利用できるように登録します。
		/// </summary>
		/// <param name="type">登録する型を指定します。</param>
		/// <returns>新しく登録した TypeCodes を返します。
		/// 既に登録されていた場合には、登録されていた TypeCodes を返します。</returns>
		public static TypeCodes RegisterTypeCode(System.Type type){
			TypeCodes code;
			if(type_dic.TryGetValue(type,out code)){
				return code;
			}else{
				code=(TypeCodes)type_dic.Count;
				type_dic.Add(type,code);
				return code;
			}
		}
		/// <summary>
		/// System.Type から TypeCodes を取得します。
		/// </summary>
		/// <param name="type">対応する TypeCodes を取得したい型を指定します。</param>
		/// <returns>指定した型に関連付けられて登録されている TypeCodes を返します。
		/// 指定した型が登録されていない場合には TypeCodes.Unknown を返します。</returns>
		public static TypeCodes GetTypeCode(System.Type type){
			TypeCodes code;
			if(type_dic.TryGetValue(type,out code)){
				return code;
			}else return TypeCodes.Unknown;
		}
	}
}