using Gen=System.Collections.Generic;
using CM=System.ComponentModel;
using Ref=System.Reflection;

namespace afh{

#pragma warning disable 1591
#if DEBUG
	/// <summary>
	/// ちゃんと動作するかの実験を行います。
	/// </summary>
	public static class UnitTest{
		[afh.Tester.TestMethod("RGB と HSV の相互変換を試験します。")]
		public static void test_RgbHsvTrans(afh.Application.Log log){
			System.Action<afh.Drawing.Color32Argb> test=delegate(afh.Drawing.Color32Argb c1){
				afh.Drawing.ColorHsv c2=(afh.Drawing.ColorHsv)c1;

				log.WriteLine("--- test ---");
				log.WriteLine(c1);
				log.WriteLine(c2);
				log.WriteLine((afh.Drawing.Color32Argb)c2);
			};
			test(0x78123456);
			test(0xffAACC98);
			test(0x03daf400);
			test(0x285f0088);
		}

		private static System.Random random=new System.Random();

		#region CollectionEditor
		/// <summary>
		/// CollectionEditor が動作するか試します。
		/// </summary>
		public static void dbg_CollectionEditor(){
			new Test2().ShowDialog();
		}
		/// <summary>
		/// Test2
		/// </summary>
		private class Test2:System.Windows.Forms.Form {
			private Test test1;

			/// <summary>
			/// .ctor of Test2.
			/// </summary>
			public Test2():base() {
				this.InitializeComponent();
			}
			private void InitializeComponent() {
				this.test1=new Test();
				this.SuspendLayout();
				// 
				// test1
				// 
				this.test1.Location=new System.Drawing.Point(12,12);
				this.test1.Name="test1";
				this.test1.Padding=new System.Windows.Forms.Padding(1);
				this.test1.Size=new System.Drawing.Size(420,263);
				this.test1.TabIndex=0;
				// 
				// Test2
				// 
				this.ClientSize=new System.Drawing.Size(463,314);
				this.Controls.Add(this.test1);
				this.Name="Test2";
				this.ResumeLayout(false);

			}
		}
		/// <summary>
		/// Test
		/// </summary>
		private class Test:afh.Collections.CollectionEditor<test0> {
			/// <summary>
			/// .ctor of Test.
			/// </summary>
			public Test()
				: base() {
				this.ShowString=showString;
			}
			private string showString(test0 v) {
				return afh.Enum.GetDescription(v);
			}
		}
		private enum test0 {
			[afh.EnumDescription("あろは")]
			aloha,
			[afh.EnumDescription("黒い葉っぱかな?")]
			kuroha,
			[afh.EnumDescription("何の事だか、さっぱり…")]
			mahaha
		}
		#endregion

		#region SortedArray
		/// <summary>
		/// afh.Collections.SortedArray`2 が正しく動作するかの実験を行います。
		/// </summary>
		public static void dbg_SortedArray2(){
			//afh.Collections.SortedArray<int,int> s=new afh.Collections.SortedArray<int,int>();
			afh.Collections.SortedArrayP<int,int> s=new afh.Collections.SortedArrayP<int,int>();

			s.Add(342,4);
			s.Add(543,5);
			s.Add(123,78);
			s.Add(123,98);
			s.Add(565,89);
			s.Add(563,312);
			s.Add(112,455);
			s.Add(453,89);
			s.Add(563,34);
			s.Add(345,789);
			s.Add(435,890);
			s.Add(321,3);
			s.Add(312,8);

			foreach(Gen::KeyValuePair<int,int> pair in s){
				System.Console.WriteLine("{0} : {1}",pair.Key,pair.Value);
			}
		}
		/// <summary>
		/// afh.Collections.SortedArray`2 が正しく動作するかの実験を行います。
		/// </summary>
		public static void dbg_SortedArray1(){
			//afh.Collections.SortedArray<int,int> s=new afh.Collections.SortedArray<int,int>();
			afh.Collections.SortedArrayP<int> s=new afh.Collections.SortedArrayP<int>();

			s.Add(342);
			s.Add(543);
			s.Add(123);
			s.Add(123);
			s.Add(565);
			s.Add(563);
			s.Add(112);
			s.Add(453);
			s.Add(563);
			s.Add(345);
			s.Add(435);
			s.Add(321);
			s.Add(312);
			s.Remove(112);
			s.Add(142);

			int i=0;
			foreach(int value in s){
				System.Console.WriteLine("{0} : {1}",i++,value);
			}
		}
		#endregion

		#region Type to String
		public static void testComposedType(afh.Application.Log log){
			System.Action<System.Type> write=delegate(System.Type t){
				testComposedType_write(t,log);
			};

			Ref::MethodInfo minfo=typeof(UnitTest).GetMethod("testComposedType_meth",Ref::BindingFlags.Static|Ref::BindingFlags.NonPublic);
			Ref::ParameterInfo[] pinfos=minfo.GetParameters();
			log.Lock();
			write(typeof(int*));
			write(pinfos[0].ParameterType);
			write(pinfos[1].ParameterType);
			write(pinfos[2].ParameterType);
			log.Unlock();

			Ref::MethodInfo minfo2=pinfos[2].ParameterType.GetMethod("Method",Ref::BindingFlags.Static|Ref::BindingFlags.Public);
			log.WriteLine(Types.GetMethodSignature(minfo));
			log.WriteLine(Types.GetMethodSignature(minfo2));
		}
		private static void testComposedType_write(System.Type t,afh.Application.Log log){
			System.Type elem=t.GetElementType();
			log.WriteLine(Types.CSharpName(t));
			log.AddIndent();
			log.WriteVar("&",t.IsByRef);
			log.WriteVar("*",t.IsPointer);
			log.WriteVar("[]",t.IsArray);
			if(t.IsGenericParameter)log.WriteLine("This is a generic parameter.");
			if(t.IsGenericType){
				log.WriteVar("Namespace",t.Namespace);
				log.WriteVar("Name",t.Name);
				log.WriteVar("FullName",t.FullName);
				foreach(System.Type type in t.GetGenericArguments())
					testComposedType_write(type,log);
			}
			if(t.IsNested){
				log.WriteVar("ParentType",t.DeclaringType);
			}
			if(elem!=null){
				log.WriteVar("ElementType",t.GetElementType());
				testComposedType_write(elem,log);
			}
			log.RemoveIndent();
		}
		private unsafe static void testComposedType_meth<T>(ref T[,,] arg0,out int* arg1,testComposedType_gen<T,int> arg2){
			arg1=(int*)System.IntPtr.Zero;
		}
		private class testComposedType_gen<T,U>{
			public static void Method<V>(){
				return;
			}
		}
		#endregion
	}

	// TYPE分岐MAX=0x100000;
	// 分岐1  312.5 ms
	// 分岐2  171.875 ms
	// 分岐3  156.25 ms
	// 分岐4  171.875 ms

	// TYPE分岐MAX=0x400000;
	// 分岐0:  203.125 ms
	// 分岐1: 1281.250 ms / 1078.125 ms // if 連
	// 分岐2:  718.750 ms /  515.625 ms // switch(Type.Name[0])
	// 分岐3:  640.625 ms /  437.500 ms // switch(Type.Name.GetHashCode()&0xf)
	// 分岐4:  703.125 ms /  500.000 ms // Hashtable
	// ×上の結果は、プログラム自体に欠陥があった為に無効
	[afh.Tester.TestTarget]
	public static class BenchTypeBranch{
		private static System.Random random=new System.Random();
		private static System.Type[] types={
			typeof(void),
			typeof(long),typeof(ulong),
			typeof(int),typeof(uint),
			typeof(short),typeof(ushort),
			typeof(sbyte),typeof(byte),
			typeof(float),typeof(double),
			typeof(System.Guid),typeof(byte[])
		};
		[CM::Description("実験対象 TypeName 一覧")]
		private static void typeNames(afh.Application.Log log){
			foreach(System.Type t in types){
				string name=t.Name;
				log.WriteLine("{0} // hashcode : 0x{1:X}",name,name.GetHashCode());
			}
		}
		private static Gen::Dictionary<System.Type,int> Type分岐4_dic=new Gen::Dictionary<System.Type,int>();
		static BenchTypeBranch() {
			int i=0;
			foreach(System.Type t in types)Type分岐4_dic.Add(t,i++);
		}

		#region Make Code: Name.GetHashCode()&0xf
		[CM::Description("Type 分岐実験3 make code")]
		public static void type分岐3_準備(afh.Application.Log log) {
			afh.Collections.DictionaryP<int,System.Type> dic=null;
			afh.Collections.DictionaryP<int,System.Type> dic0;
			int minwrap=int.MaxValue;
			int minshift=0;
			for(int shift=0;shift<=28;shift++){
				int wrap=type分岐3_準備1(shift,out dic0);
				if(wrap<minwrap){
					dic=dic0;
					minwrap=wrap;
					minshift=shift;
				}
			}

			log.Lock();
			if(minshift==0){
				log.WriteLine("switch(t.Name.GetHashCode()&0xf){");
			}else{
				log.WriteLine("switch((t.Name.GetHashCode()>>{0})&0xf){{",minshift);
			}
			log.AddIndent();
			int c=1;
			for(int i=0;i<=0xf;i++){
				// case i:
				System.Text.StringBuilder build=new System.Text.StringBuilder();
				build.AppendFormat("case {0}:",i);
				bool _else=false;
				foreach(System.Type type in dic[i]){
					if(_else)build.Append("else ");
					build.AppendFormat	("if(t==typeof({0})){{\r\n",afh.Types.CSharpName(type));
					build.AppendFormat	("        c+={0};\r\n",c++);
					build.Append		("    }");
					_else=true;
				}
				build.Append("break;");
				log.WriteLine(build.ToString());
			}
			log.RemoveIndent();
			log.WriteLine("}");
			log.Unlock();//*/
		}
		private static int type分岐3_準備1(int shift,out afh.Collections.DictionaryP<int,System.Type> dic){
			dic=new afh.Collections.DictionaryP<int,System.Type>();
			int wrap=0;
			foreach(System.Type t in types) {
				int key=(t.Name.GetHashCode()>>shift)&0xf;
				if(dic.ContainsKey(key))wrap++;
				dic.Add(key,t);
			}
			return wrap;
		}
		#endregion

		[afh.Tester.BenchMethod("Type 分岐実験0 空回し")]
		public static int type分岐0(){
			int c;
			System.Type t=types[c=random.Next(types.Length)];
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験1 if 連発")]
		public static int type分岐1(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			if(t==typeof(void)){
				c=1;
			}else if(t==typeof(long)){
				c=2;
			}else if(t==typeof(ulong)){
				c=3;
			}else if(t==typeof(int)){
				c=4;
			}else if(t==typeof(uint)){
				c=5;
			}else if(t==typeof(short)){
				c=6;
			}else if(t==typeof(ushort)){
				c=7;
			}else if(t==typeof(sbyte)){
				c=8;
			}else if(t==typeof(byte)){
				c=9;
			}else if(t==typeof(float)){
				c=10;
			}else if(t==typeof(double)){
				c=11;
			}else if(t==typeof(System.Guid)){
				c=12;
			}else if(t==typeof(byte[])){
				c=13;
			}
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験2 TypeName[0] switch")]
		public static int type分岐2(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			switch(t.Name[0]){
				case 'U':if(t==typeof(uint)){
					c=5;
				}else if(t==typeof(ulong)){
					c=3;
				}else if(t==typeof(ushort)){
					c=7;
				}break;
				case 'S':if(t==typeof(sbyte)){
					c=8;
				}else if(t==typeof(float)){
					c=10;
				}break;
				case 'B':if(t==typeof(byte)){
					c=9;
				}else if(t==typeof(byte[])){
					c=13;
				}break;
				case 'V':if(t==typeof(void)){
					c=1;
				}break;
				case 'I':if(t==typeof(short)){
					c=6;
				}else if(t==typeof(int)){
					c=4;
				}else if(t==typeof(long)){
					c=2;
				}break;
				case 'D':if(t==typeof(double)){
					c=11;
				}break;
				case 'G':if(t==typeof(System.Guid)){
					c=12;
				}break;
			}
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験3 TypeName.Hash&0xf switch")]
		public static int type分岐3(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			switch(t.Name.GetHashCode()&0xf){
				case 0:break;
				case 1:if(t==typeof(short)){
						c=1;
					}break;
				case 2:break;
				case 3:if(t==typeof(float)){
						c=2;
					}break;
				case 4:if(t==typeof(byte)){
						c=3;
					}break;
				case 5:if(t==typeof(int)){
						c=4;
					}else if(t==typeof(sbyte)){
						c=5;
					}break;
				case 6:if(t==typeof(ushort)){
						c=6;
					}else if(t==typeof(System.Guid)){
						c=7;
					}break;
				case 7:if(t==typeof(void)){
						c=8;
					}break;
				case 8:if(t==typeof(uint)){
						c=9;
					}break;
				case 9:break;
				case 10:break;
				case 11:if(t==typeof(ulong)){
						c=10;
					}break;
				case 12:break;
				case 13:break;
				case 14:break;
				case 15:if(t==typeof(long)){
						c=11;
					}else if(t==typeof(double)){
						c=12;
					}else if(t==typeof(byte[])){
						c=13;
					}break;
			}
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験3 (TypeName.Hash>>**)&0xf switch")]
		public static int type分岐3_shift(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			switch((t.Name.GetHashCode()>>1)&0xf){
				case 0:break;
				case 1:break;
				case 2:if(t==typeof(sbyte)){
						c=1;
					}else if(t==typeof(byte)){
						c=2;
					}break;
				case 3:if(t==typeof(ushort)){
						c=3;
					}else if(t==typeof(System.Guid)){
						c=4;
					}break;
				case 4:if(t==typeof(uint)){
						c=5;
					}break;
				case 5:if(t==typeof(ulong)){
						c=6;
					}break;
				case 6:break;
				case 7:if(t==typeof(long)){
						c=7;
					}else if(t==typeof(double)){
						c=8;
					}break;
				case 8:if(t==typeof(short)){
						c=9;
					}break;
				case 9:if(t==typeof(float)){
						c=10;
					}break;
				case 10:if(t==typeof(int)){
						c=11;
					}break;
				case 11:if(t==typeof(void)){
						c=12;
					}break;
				case 12:break;
				case 13:break;
				case 14:break;
				case 15:if(t==typeof(byte[])){
						c=13;
					}break;
			}
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験4 hashtable")]
		public static int type分岐4(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			int typecode;
			if(Type分岐4_dic.TryGetValue(t,out typecode)){
				switch(typecode){
					case 0:c=1;break;
					case 1:c=4;break;
					case 2:c=2;break;
					case 3:c=8;break;
					case 4:c=3;break;
					case 5:c=11;break;
					case 6:c=12;break;
					case 7:c=6;break;
					case 8:c=1;break;
					case 9:c=5;break;
					case 10:c=13;break;
					case 11:c=7;break;
					case 12:c=10;break;
					case 13:c=9;break;
				}
			}
			return c;
		}
		[afh.Tester.BenchMethod("Type 分岐実験5 afh.TypeCodes")]
		public static int type分岐5(){
			int c=0;
			System.Type t=types[random.Next(types.Length)];
			TypeCodes code;
			if(afh.Types.type_dic.TryGetValue(t,out code)){
				switch(code){
					case TypeCodes.Void:	c=1;break;
					case TypeCodes.SByte:	c=2;break;
					case TypeCodes.Byte:	c=3;break;
					case TypeCodes.Short:	c=4;break;
					case TypeCodes.UShort:	c=5;break;
					case TypeCodes.Int:		c=6;break;
					case TypeCodes.UInt:	c=7;break;
					case TypeCodes.Long:	c=8;break;
					case TypeCodes.ULong:	c=9;break;
					case TypeCodes.Float:	c=10;break;
					case TypeCodes.Double:	c=11;break;
					case TypeCodes.ByteArray:c=12;break;
				}
			}else if(t==typeof(System.Guid)){
				c=13;
			}
			return c;
		}

	}
#endif
}