
using Gen=System.Collections.Generic;

namespace afh.File {
	#if DEBUG
	public static class UnitTest{

		#region èëçûë¨ìx benchmark
		private static byte[] dbg_static_buff=new byte[4];
		public unsafe static void dbg_Int32ToBytesBenchmark(){
			byte[] buff=new byte[4];
			const int loop=0x4000000;
			System.DateTime dt0,dt1;

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++)fixed(byte* b=dbg_static_buff){
				*(int*)b=i;
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("å¬ÅXÇÃ fixed (field): {0}",(dt1-dt0).Ticks);

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++)fixed(byte* b=buff){
				*(int*)b=i;
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("å¬ÅXÇÃ fixed (local): {0}",(dt1-dt0).Ticks);

			dt0=System.DateTime.Now;
			fixed(byte* b=buff)for(int i=0;i<loop;i++){
				*(int*)b=i;
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("ëSëÃÇ≈ fixed ÇÃèÍçá : {0}",(dt1-dt0).Ticks);
		
			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				buff[0]=(byte)i;
				buff[1]=(byte)(i>>8);
				buff[2]=(byte)(i>>16);
				buff[3]=(byte)(i>>24);
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("Shift ââéZÇÃèÍçá    : {0}",(dt1-dt0).Ticks);
		}
		public unsafe static void dbg_Int32ToBigEndianBenchmark(){
			byte[] buff=new byte[4];
			byte[] buff2=new byte[4];
			const int loop=0x4000000;
			System.DateTime dt0,dt1;

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				buff[0]=(byte)(i>>24);
				buff[1]=(byte)(i>>16);
				buff[2]=(byte)(i>>8);
				buff[3]=(byte)i;
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("Shift ââéZ 1B ñà   : {0}",(dt1-dt0).Ticks);

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				fixed(byte* b=&buff[0])*(int*)b=i;
				buff2[0]=buff[3];
				buff2[1]=buff[2];
				buff2[2]=buff[1];
				buff2[3]=buff[0];
			}

			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("Ptr ââéZ (LE Å® BE): {0}",(dt1-dt0).Ticks);

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++)fixed(byte* b=&buff[0]){
				byte* t=b+3;
				byte* v=(byte*)&i;
				while(t>=b)*t--=*v++;
			}
			dt1=System.DateTime.Now;
			__dll__.log.WriteLine("Ptr ââéZ (bytewise): {0}",(dt1-dt0).Ticks);
		}
		public unsafe static void dbg_I3ToInt32Benchmark(){
			byte[] buff=new byte[4];
			const int loop=0x4000000;
			System.DateTime dt0,dt1;
			int value;

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				buff[2]=(byte)i;
				buff[3]=(buff[2]&0x80)!=0?(byte)0xff:(byte)0;
				fixed(byte* b=&buff[0])value=*(int*)b;
			}
			dt1=System.DateTime.Now;
			//&  ?: ïÑçÜägí£
			__dll__.log.AppendText((dt1-dt0).Ticks.ToString());

			__dll__.log.AppendText(",");

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				buff[2]=(byte)i;
				buff[3]=buff[2]>>7!=0?(byte)0xff:(byte)0;
				fixed(byte* b=&buff[0])value=*(int*)b;
			}
			dt1=System.DateTime.Now;
			//>> ?: ïÑçÜägí£
			__dll__.log.AppendText((dt1-dt0).Ticks.ToString());

			__dll__.log.AppendText(",");

			dt0=System.DateTime.Now;
			for(int i=0;i<loop;i++){
				buff[2]=(byte)i;
				buff[3]=0;
				fixed(byte* b=&buff[0])value=*(int*)b;
				value=value<<8>>8;
			}
			dt1=System.DateTime.Now;
			//<< >> ïÑçÜägí£
			__dll__.log.AppendText((dt1-dt0).Ticks.ToString());

			__dll__.log.AppendText("\r\n");
		}
		#endregion

		#region ï∂éöóÒì«çû
		public static void dbg_DecoderTest(){
			System.IO.Stream s=System.IO.File.OpenRead(@"C:\Documents and Settings\murase\ÉfÉXÉNÉgÉbÉv\enum2.txt");
			System.Text.Decoder dec=System.Text.Encoding.Default.GetDecoder();
			System.Text.StringBuilder builder=new System.Text.StringBuilder();
			int bi;
			const int LEN=1;
			do{
				byte[] buffer=new byte[LEN];
				char[] chars=new char[LEN];
				bi=s.Read(buffer,0,LEN);
				int ci=dec.GetChars(buffer,0,bi,chars,0);
				builder.Append(chars,0,ci);
			}while(bi==LEN);
			System.Console.WriteLine(builder.ToString());
			// ï™Ç©Ç¡ÇΩéñ:
			//		Decoder ÇÕ 1B Ç∏Ç¬èÓïÒÇìnÇµÇƒÇ‡ÇøÇ·ÇÒÇ∆ decode ÇµÇƒÇ≠ÇÍÇÈÅB
		}
		public static void dbg_RWString() {
			System.IO.MemoryStream str=new System.IO.MemoryStream();
			StreamAccessor ac=new StreamAccessor(str);

			ac.Write("ç°ì˙ÇÕ",EncodingType.NoSpecified);
			ac.Stream.Position=0;
			dumpMemoryStream(str);

			System.Console.WriteLine(ac.Read<string>(EncodingType.NoSpecified));
		}
		#endregion

		#region äÓñ{å^èëçû
		public static void dbg_RWPrimitiveTypes(){
			System.IO.MemoryStream str=new System.IO.MemoryStream();
			StreamAccessor ac=new StreamAccessor(str);
			ac.Write((byte)1,EncodingType.U1);
			ac.Write((sbyte)2,EncodingType.I1);
			ac.Write((short)3,EncodingType.I2);
			ac.Write((short)4,EncodingType.I2BE);
			ac.Write((ushort)5,EncodingType.U2);
			ac.Write((ushort)6,EncodingType.U2BE);
			ac.Write((int)7,EncodingType.I3);
			ac.Write((int)8,EncodingType.I3BE);
			ac.Write((int)9,EncodingType.Int28);
			ac.Write((int)10,EncodingType.Int28BE);
			ac.Write((int)11,EncodingType.I4);
			ac.Write((int)12,EncodingType.I4BE);
			ac.Write((uint)13,EncodingType.U3);
			ac.Write((uint)14,EncodingType.U3BE);
			ac.Write((uint)15,EncodingType.UInt28);
			ac.Write((uint)16,EncodingType.UInt28BE);
			ac.Write((uint)17,EncodingType.U4);
			ac.Write((uint)18,EncodingType.U4BE);
			ac.Write((long)19,EncodingType.I8);
			ac.Write((long)20,EncodingType.I8BE);
			ac.Write((ulong)21,EncodingType.U8);
			ac.Write((ulong)22,EncodingType.U8BE);
			str.Position=0;
			System.Console.WriteLine(ac.ReadByte(EncodingType.U1));
			System.Console.WriteLine(ac.ReadSByte(EncodingType.I1));
			System.Console.WriteLine(ac.ReadInt16(EncodingType.I2));
			System.Console.WriteLine(ac.ReadInt16(EncodingType.I2BE));
			System.Console.WriteLine(ac.ReadUInt16(EncodingType.U2));
			System.Console.WriteLine(ac.ReadUInt16(EncodingType.U2BE));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.I3));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.I3BE));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.Int28));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.Int28BE));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.I4));
			System.Console.WriteLine(ac.ReadInt32(EncodingType.I4BE));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.U3));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.U3BE));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.UInt28));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.UInt28BE));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.U4));
			System.Console.WriteLine(ac.ReadUInt32(EncodingType.U4BE));
			System.Console.WriteLine(ac.ReadInt64(EncodingType.I8));
			System.Console.WriteLine(ac.ReadInt64(EncodingType.I8BE));
			System.Console.WriteLine(ac.ReadUInt64(EncodingType.U8));
			System.Console.WriteLine(ac.ReadUInt64(EncodingType.U8BE));
			// åãâ :
			//		ulong ãyÇ— long ÇÃì«Ç›èëÇ´ÇèCê≥
			//		Å®OK
		}
		#endregion

		#region attr:RWSchedule
		public static void dbg_RWScheduledClass(){
			System.IO.MemoryStream str=new System.IO.MemoryStream();
			StreamAccessor ac=new StreamAccessor(str);

			TargetClass t1=new TargetClass();
			t1.Init();
			t1.WriteToConsole();
			ac.Write(t1,EncodingType.NoSpecified);
			ac.Stream.Position=0;

			dumpMemoryStream(str);

			TargetClass t2=ac.Read<TargetClass>();
			t2.WriteToConsole();
		}

		[WriteSchedule("f1","f2","P3","f4","atoms")]
		[ReadSchedule("f1","f2","read_X","P3","f4","atoms")]
		private class TargetClass{
			public TargetClass(){}

			private void read_X(){
				System.Console.WriteLine("read_X Ç™åƒÇ—èoÇ≥ÇÍÇ‹ÇµÇΩÅB");
			}

			//===================================
			//		ReadWriteAs
			//===================================
#pragma warning disable 0414
			private bool condition=false;
#pragma warning restore 0414

			[ReadWriteAs(EncodingType.I4)]
			private int f1=1;

			[ReadWriteAs(EncodingType.I8BE)]
			private long f2=2;

			private short f3=13;
			[ReadWriteAs(EncodingType.I2)]
			private short P3{
				get{
					System.Console.WriteLine("f3 Ç©ÇÁílÇ™ì«Ç‹ÇÍÇ‹ÇµÇΩÅB");
					return this.f3;
				}
				set{
					System.Console.WriteLine("f3 Ç…ílÇ™ê›íËÇ≥ÇÍÇ‹ÇµÇΩÅB");
					this.f3=value;
				}
			}

			[ReadWriteAs(EncodingType.StrBasic|EncodingType.EncEmbedded|EncodingType.Enc_utf_16)]
			private string f4;

			//===================================
			//		ReadWriteArray
			//===================================
			[ReadWriteArray(ArrayLengthType=ArrayLengthType.CountEmbedded)]
			[ReadWriteAs(EncodingType.StrSize|EncodingType.EncRaw|EncodingType.Enc_utf_16)]
			private string[] atoms;

			public void Init(){
				//this.atoms=new string[]{"H","I"};
				this.atoms=new string[]{"H","He","Li","Be","B","C","N","O","F","Ne","Na","Mg","Al","Si","P","S","Cl","Ar",
					"K","Ca","Sc","Ti","V","Cr","Mn","Fe","Co","Ni","Cu","Zn","Ga","Ge","As","Se","I","Xe"};
				this.f4="Ç±ÇÍÇÕ field 4 Ç…äiî[Ç≥ÇÍÇƒÇ¢ÇÈï∂éöóÒÇ≈Ç∑ÅB";
			}
			public void WriteToConsole(){
				System.Console.WriteLine("TargetClass.WriteToConsole");
				System.Console.WriteLine(this.f1);
				System.Console.WriteLine(this.f2);
				System.Console.WriteLine(this.f3);
				System.Console.WriteLine(this.f4);
				foreach(string s in atoms)System.Console.Write(s+" ");
				System.Console.WriteLine();
			}
		}
		#endregion

		private static void dumpMemoryStream(System.IO.MemoryStream str){
			// stream ÇÃì‡óe
			int i=0;
			foreach(byte b in str.GetBuffer()){
				if(i++==str.Length) break;
				System.Console.Write(b.ToString("X2")+" ");
			}
			System.Console.WriteLine();
		}
	}
	#endif
}