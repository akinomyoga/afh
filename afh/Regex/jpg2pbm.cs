using Gdi=System.Drawing;

namespace mwg.Tool{
	public static class Jpg2Pbm{
		public static int Main(string[] args){
			if(args.Length<1){
				WriteUsage();
				return 0;
			}
			if(!System.IO.File.Exists(args[0])){
				System.Console.WriteLine("指定したファイル '{0}' が見つかりません。",args[0]);
				return 1;
			}

			string ext=System.IO.Path.GetExtension(args[0]);
			string output=args[0].Substring(0,args[0].Length-ext.Length)+".pbm";
			WriteToPbm(args[0],output);

			return 0;
		}

		public static void WriteToPbm(string input,string output){
			Gdi::Bitmap image=new Gdi::Bitmap(input);
			int w=image.Width;
			int h=image.Height;
			Gdi::Imaging.BitmapData data=image.LockBits(
				new Gdi::Rectangle(Gdi::Point.Empty,image.Size),
				Gdi::Imaging.ImageLockMode.ReadOnly,
				Gdi::Imaging.PixelFormat.Format24bppRgb);

			System.IO.Stream str=System.IO.File.OpenWrite(output);
			System.IO.StreamWriter sw=new System.IO.StreamWriter(str,System.Text.Encoding.ASCII);
			sw.WriteLine("P1");
			sw.WriteLine("{0} {1}",w,h);
			unsafe{
				int i=0;
				for(int y=0;y<h;y++){
					RGB* ppx=(RGB*)((byte*)data.Scan0+data.Stride*y);
					RGB* ppxM=ppx+w;
					while(ppx<ppxM){
						sw.Write((ppx++)->Intensity()>0x80?"0":"1");
						if(++i%64==0)
							sw.WriteLine();
						else
							sw.Write(" ");
					}
				}
			}
			sw.Close();
			str.Close();

			image.UnlockBits(data);
			image.Dispose();
		}

		private static void WriteUsage(){
			System.Console.WriteLine("使い方");
			System.Console.WriteLine("\tjpg2pbm <image-filename>");
		}

	}

	public struct RGB{
		public byte B;
		public byte G;
		public byte R;

		public byte Intensity(){
			return (byte)(0.299*R+0.587*G+0.114*B);
		}
	}
}