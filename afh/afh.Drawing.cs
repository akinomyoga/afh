using Imaging=System.Drawing.Imaging;
using Interop=System.Runtime.InteropServices;
using Gen=System.Collections.Generic;
using Gdi=System.Drawing;

namespace afh.Drawing{
	/// <summary>
	/// 色を ARGB で表す 32bit 値の構造体です。
	/// </summary>
	[Interop.StructLayout(Interop.LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct Color32Argb{
		/// <summary>
		/// 青の強度を表します。
		/// </summary>
		public byte B;
		/// <summary>
		/// 緑の強度を表します。
		/// </summary>
		public byte G;
		/// <summary>
		/// 赤の強度を表します。
		/// </summary>
		public byte R;
		/// <summary>
		/// アルファ値 (不透明度) を指定します。
		/// </summary>
		public byte A;

		/// <summary>
		/// シアン成分の量を取得又は設定します。
		/// </summary>
		public byte C{
			get{return (byte)(255-this.R);}
			set{this.R=(byte)(255-value);}
		}
		/// <summary>
		/// マゼンタ成分の量を取得又は設定します。
		/// </summary>
		public byte M{
			get{return (byte)(255-this.G);}
			set{this.G=(byte)(255-value);}
		}
		/// <summary>
		/// 黄成分の量を取得又は設定します。
		/// </summary>
		public byte Y{
			get{return (byte)(255-this.B);}
			set{this.B=(byte)(255-value);}
		}

		#region Operators
		/// <summary>
		/// 色を反転します。
		/// </summary>
		/// <param name="color">反転前の色を指定します。</param>
		/// <returns>計算結果の反転した色を返します。</returns>
		public static unsafe Color32Argb operator ~(Color32Argb color){
			uint icol=*(uint*)&color;
			icol=~(icol&0x00ffffff)|(icol&0xff000000);
			return *(Color32Argb*)&icol;
		}
		//-----------------------------------------------------------
		//			cast<System.Int32>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb の色を int で表現した色に変換します。
		/// </summary>
		/// <param name="color">Color32Argb 形式の色を指定します。</param>
		/// <returns>System.Int32 で表現した色を返します。</returns>
		public static unsafe explicit operator int(Color32Argb color){return *(int*)&color;}
		/// <summary>
		/// int で表現した色を Color32Argb の色に変換します。
		/// </summary>
		/// <param name="color">System.Int32 で表現した色を指定します。</param>
		/// <returns>Color32Argb 形式の色を返します。</returns>
		public static unsafe implicit operator Color32Argb(int color){return *(Color32Argb*)&color;}
		//-----------------------------------------------------------
		//			cast<System.UInt32>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb の色を uint で表現した色に変換します。
		/// </summary>
		/// <param name="color">Color32Argb 形式の色を指定します。</param>
		/// <returns>System.UInt32 で表現した色を返します。</returns>
		public static unsafe explicit operator uint(Color32Argb color){return *(uint*)&color;}
		/// <summary>
		/// int で表現した色を Color32Argb の色に変換します。
		/// </summary>
		/// <param name="color">System.UInt32 で表現した色を指定します。</param>
		/// <returns>Color32Argb 形式の色を返します。</returns>
		public static unsafe implicit operator Color32Argb(uint color){return *(Color32Argb*)&color;}
		//-----------------------------------------------------------
		//			cast<afh.Drawing.ColorRef>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb の色を Color24Rgb の色に変換します。
		/// </summary>
		/// <param name="color">Color32Argb 形式の色を指定します。</param>
		/// <returns>Color24Rgb 形式の色を返します。</returns>
		public static implicit operator ColorRef(Color32Argb color){
			return new ColorRef(color.R,color.G,color.G);
		}
		/// <summary>
		/// Color24Rgb の色を Color32Argb の色に変換します。
		/// </summary>
		/// <param name="color">Color24Rgb 形式の色を指定します。</param>
		/// <returns>Color32Argb 形式の色を返します。</returns>
		public static implicit operator Color32Argb(ColorRef color){
			return new Color32Argb(255,color.R,color.G,color.B);
		}
		//-----------------------------------------------------------
		//			cast<System.Drawing.Color>
		//-----------------------------------------------------------
		/// <summary>
		/// System.Drawing.Color の色を Color32Argb に変換します。
		/// </summary>
		/// <param name="color">System.Drawing.Color で表現された色を指定します。</param>
		/// <returns>Color32Argb 形式に変換した色を返します。</returns>
		public static implicit operator Color32Argb(System.Drawing.Color color){
			return (Color32Argb)color.ToArgb();
		}
		/// <summary>
		/// Color32Argb の色を System.Drawing.Color に変換します。
		/// </summary>
		/// <param name="color">Color32Argb 形式の色を指定します。</param>
		/// <returns>System.Drawing.Color 形式に変換した色を返します。</returns>
		public static implicit operator System.Drawing.Color(Color32Argb color){
			return System.Drawing.Color.FromArgb((int)color);
		}
		//-----------------------------------------------------------
		//			Identity
		//-----------------------------------------------------------
		/// <summary>
		/// 指定した二つの色が等しいかどうかを判定します。
		/// </summary>
		/// <param name="l">比較する色を指定します。</param>
		/// <param name="r">比較する色を指定します。</param>
		/// <returns>等しい場合に true を返します。それ以外の時に false を返します。</returns>
		public static bool operator ==(Color32Argb l,Color32Argb r){
			return (int)l==(int)r;
		}
		/// <summary>
		/// 指定した二つの色が異なるかどうかを判定します。
		/// </summary>
		/// <param name="l">比較する色を指定します。</param>
		/// <param name="r">比較する色を指定します。</param>
		/// <returns>異なる場合に true を返します。それ以外の時に false を返します。</returns>
		public static bool operator !=(Color32Argb l,Color32Argb r){
			return (int)l!=(int)r;
		}
		/// <summary>
		/// 指定した object とこのインスタンスが等しいかどうかを判定します。
		/// </summary>
		/// <param name="obj">比較対象の object を指定します。</param>
		/// <returns>等しいと評価できる場合に true を返します。</returns>
		public override bool Equals(object obj){
			if(obj is Color32Argb){
				return this==(Color32Argb)obj;
			}else if(obj is ColorRef){
				return this==(Color32Argb)(ColorRef)obj;
			}else if(obj is int){
				return (int)this==(int)obj;
			}else if(obj is uint){
				return (uint)this==(uint)obj;
			}else if(obj is System.Drawing.Color) {
				return (int)this==((System.Drawing.Color)obj).ToArgb();
			}else return false;
		}
		/// <summary>
		/// ハッシュ値を計算します。
		/// </summary>
		/// <returns>計算したハッシュ値を返します。</returns>
		public override int GetHashCode() {
			return ((int)this).GetHashCode();
		}
		#endregion

		/// <summary>
		/// Color32Argb コンストラクタです。
		/// </summary>
		/// <param name="A">不透明度を指定します。</param>
		/// <param name="R">赤の強度を指定します。</param>
		/// <param name="G">緑の強度を指定します。</param>
		/// <param name="B">青の強度を指定します。</param>
		public Color32Argb(byte A,byte R,byte G,byte B){
			this.A=A;this.R=R;this.G=G;this.B=B;
		}
		/// <summary>
		/// A (不透明度), R (赤の強度), G (緑の強度), B (青の強度) から ColorArgb32 インスタンスを取得します。
		/// </summary>
		/// <param name="a">不透明度を指定します。</param>
		/// <param name="r">赤の強度を指定します。</param>
		/// <param name="g">緑の強度を指定します。</param>
		/// <param name="b">青の強度を指定します。</param>
		/// <returns>指定した値を元にして作成した ColorArgb32 の値を返します。</returns>
		public static Color32Argb FromArgb(byte a,byte r,byte g,byte b){
			return new Color32Argb(a,r,g,b);
		}
		/// <summary>
		/// R (赤の強度), G (緑の強度), B (青の強度) から ColorArgb32 インスタンスを取得します。
		/// </summary>
		/// <param name="r">赤の強度を指定します。</param>
		/// <param name="g">緑の強度を指定します。</param>
		/// <param name="b">青の強度を指定します。</param>
		/// <returns>指定した値を元にして作成した ColorArgb32 の値を返します。</returns>
		public static Color32Argb FromRgb(byte r,byte g,byte b){
			return new Color32Argb(0xff,r,g,b);
		}
		/// <summary>
		/// H (色相; Hue), S (彩度), V (明度) から ColorArgb32 インスタンスを取得します。
		/// </summary>
		/// <param name="h">色相を指定します。色相の詳細に関しては、<see cref="ColorHsv"/> を参照して下さい。</param>
		/// <param name="s">彩度を指定します。</param>
		/// <param name="v">明度を指定します。</param>
		/// <returns>指定した値を元にして作成した ColorArgb32 の値を返します。</returns>
		public static Color32Argb FromHsv(double h,byte s,byte v){
			return (Color32Argb)new ColorHsv(0xff,h,s,v);
		}
		/// <summary>
		/// C (シアンの量), M (マゼンタの量), Y (黄の量) から ColorArgb32 インスタンスを取得します。
		/// </summary>
		/// <param name="c">シアンの量を指定します。</param>
		/// <param name="m">マゼンタの量を指定します。</param>
		/// <param name="y">黄の量を指定します。</param>
		/// <returns>指定した値を元にして作成した ColorArgb32 の値を返します。</returns>
		public static Color32Argb FromCmy(byte c,byte m,byte y){
			return new Color32Argb(0xff,(byte)(255-c),(byte)(255-m),(byte)(255-y));
		}

		/// <summary>
		/// 色を文字列として表現します。
		/// </summary>
		/// <returns>ARGB の各強度を示した文字列で返します。</returns>
		public override string ToString() {
			return base.ToString()+string.Format("{{A:0x{0:X2}; R:0x{1:X2}; G:0x{2:X2}; B:0x{3:X2};}}",this.A,this.R,this.G,this.B);
		}

		/// <summary>
		/// 黒色を表す Color32Argb を取得します。
		/// </summary>
		public static Color32Argb Black{get{return 0xff000000;}}
		/// <summary>
		/// 白色を表す Color32Argb を取得します。
		/// </summary>
		public static Color32Argb White{get{return 0xffffffff;}}
		/// <summary>
		/// 赤色を表す Color32Argb を取得します。
		/// </summary>
		public static Color32Argb Red  {get{return 0xffff0000;}}
		/// <summary>
		/// 緑色を表す Color32Argb を取得します。
		/// </summary>
		public static Color32Argb Green{get{return 0xff00ff00;}}
		/// <summary>
		/// 青色を表す Color32Argb を取得します。
		/// </summary>
		public static Color32Argb Blue {get{return 0xff0000ff;}}
	}
	/// <summary>
	/// 色相・彩度・明度による色表現です。
	/// </summary>
	[Interop::StructLayout(Interop::LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct ColorHsv{
		/// <summary>
		/// 色相・彩度・明度を指定して ColorHsv を初期化します。
		/// </summary>
		/// <param name="hue">色相を指定します。</param>
		/// <param name="saturation">彩度を指定します。</param>
		/// <param name="value">明度を指定します。</param>
		public ColorHsv(double hue,byte saturation,byte value){
			this.H=hue;
			this.S=saturation;
			this.V=value;
			this.A=0xff;
		}
		/// <summary>
		/// 不透明度・色相・彩度・明度を指定して ColorHsv を初期化します。
		/// </summary>
		/// <param name="alpha">不透明度を指定します。</param>
		/// <param name="hue">色相を指定します。</param>
		/// <param name="saturation">彩度を指定します。</param>
		/// <param name="value">明度を指定します。</param>
		public ColorHsv(byte alpha,double hue,byte saturation,byte value) {
			this.H=hue;
			this.S=saturation;
			this.V=value;
			this.A=alpha;
		}
		/// <summary>
		/// 色相を保持します。
		/// 色相は 0.0 から 360.0 迄の値で表現されます。
		/// 又、この範囲の外の色相は、360 を法にして剰余を取った物と同一視されます。
		/// <para>
		/// 0.0 は赤を表します。60.0 は黄を表します。120.0 は緑を表します。
		/// 180.0 はシアンを表します。240.0 は青を表します。300.0 は マゼンタを表します。
		/// </para>
		/// </summary>
		public double H;
		/// <summary>
		/// 彩度を保持します。
		/// 255 が最も鮮やかな色である事を示し、
		/// 0 が最も鈍い色 (グレースケール) である事を示します。
		/// </summary>
		public byte S;
		/// <summary>
		/// 明度を保持します。0 が最も暗い事を示し、
		/// 255 が最も明るい色である事を示します。
		/// </summary>
		public byte V;
		/// <summary>
		/// アルファ値 (不透明度) を保持します。
		/// 0 が透明な色である事を示し、255 が不透明な色である事を示します。
		/// </summary>
		public byte A;

		/// <summary>
		/// 色を文字列として表現します。
		/// </summary>
		/// <returns>ARGB の各強度を示した文字列で返します。</returns>
		public override string ToString() {
			return base.ToString()+string.Format("{{A:0x{0:X2}; H:{1:F1}; S:0x{2:X2}; V:0x{3:X2};}}",this.A,this.H,this.S,this.V);
		}

		//-----------------------------------------------------------
		//			cast<afh.Drawing.ColorHsv>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb の色を Color24Rgb の色に変換します。
		/// </summary>
		/// <param name="color">Color32Argb 形式の色を指定します。</param>
		/// <returns>Color24Rgb 形式の色を返します。</returns>
		public unsafe static explicit operator ColorHsv(Color32Argb color){
			byte* pB=(byte*)&color;
			int min,max;

			//-- afh.Math.MinMax より拝借
			if(pB[0]<=pB[1]) {
				if(pB[1]<pB[2]){
					min=0;max=2;
				}else if(pB[0]<=pB[2]){
					min=0;max=1;
				}else{
					min=2;max=1;
				}
			}else if(pB[0]<=pB[2]){
				min=1;max=2;
			}else if(pB[2]<=pB[1]){
				min=2;max=0;
			}else{
				min=1;max=0;
			}

			int range=pB[max]-pB[min];
			if(range==0)return new ColorHsv(0,0,*pB);
			double hue=(pB[(max+2)%3]-pB[(max+1)%3])*60.0/range+120*(2-max);
			return new ColorHsv(color.A,hue,(byte)(255*range/pB[max]),pB[max]);
		}
		/// <summary>
		/// Color24Rgb の色を Color32Argb の色に変換します。
		/// </summary>
		/// <param name="color">Color24Rgb 形式の色を指定します。</param>
		/// <returns>Color32Argb 形式の色を返します。</returns>
		public unsafe static explicit operator Color32Argb(ColorHsv color){
			if(color.S==0)return new Color32Argb(color.A,color.V,color.V,color.V);
			Color32Argb ret=new Color32Argb();
			byte* pB=(byte*)&ret;
			pB[3]=color.A;

			// f: color.H/60 小数部
			// H: [color.H/60]+1 mod 6
			double f=color.H/60+1;
			int H=(int)System.Math.Floor(f);
			f-=H;
			H%=6;if(H<0)H+=6;

			int max=2-(H>>1);
			pB[max]=color.V;

			byte c2=(byte)(color.V*(255-color.S)/255);
			int LR=H&1;
			if(LR==1)f=1-f;

			pB[(max+2-LR)%3]=c2;
			pB[(max+1+LR)%3]=(byte)(color.V*(255-f*color.S)/255);

			return ret;
		}
	}
	/// <summary>
	/// 色を RGB で表す 32bit 値の構造体です。上位 8 bit は使用されません。
	/// </summary>
	[Interop.StructLayout(Interop.LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct ColorRef{
		/// <summary>
		/// 赤の強度を表します。
		/// </summary>
		public byte R;
		/// <summary>
		/// 青の強度を表します。
		/// </summary>
		public byte B;
		/// <summary>
		/// 緑の強度を表します。
		/// </summary>
		public byte G;
		/// <summary>
		/// アルファ値 (不透明度) を指定します。使用されません。常に 0 である必要があります。
		/// </summary>
		private byte A;

		/// <summary>
		/// Color24Rgb の色を int で表現した色に変換します。
		/// </summary>
		/// <param name="color">Color24Rgb 形式の色を指定します。</param>
		/// <returns>System.Int32 で表現した色を返します。</returns>
		public static unsafe explicit operator uint(ColorRef color){return *(uint*)&color; }
		/// <summary>
		/// int で表現した色を Color24Rgb の色に変換します。
		/// </summary>
		/// <param name="color">System.Int32 で表現した色を指定します。</param>
		/// <returns>Color24Rgb 形式の色を返します。</returns>
		public static unsafe implicit operator ColorRef(uint color){return *(ColorRef*)&color; }

		/// <summary>
		/// Color24Rgb コンストラクタです。
		/// </summary>
		/// <param name="R">赤の強度を指定します。</param>
		/// <param name="G">緑の強度を指定します。</param>
		/// <param name="B">青の強度を指定します。</param>
		public ColorRef(byte R,byte G,byte B) {
			this.A=0;this.R=R;this.G=G;this.B=B;
		}
	}
	/// <summary>
	/// Bitmap に様々な効果を与える関数を提供するクラスです。
	/// </summary>
	public static class BitmapEffect{
		/// <summary>
		/// 指定した Bitmap の色を反転します。
		/// </summary>
		/// <param name="source">反転前の画像を指定します。</param>
		/// <returns>反転後の画像を指定します。</returns>
		public static System.Drawing.Bitmap Invert(System.Drawing.Bitmap source){
			System.Drawing.Bitmap bmp=(System.Drawing.Bitmap)source.Clone();
			System.Drawing.Rectangle rect=new System.Drawing.Rectangle(0,0,bmp.Width,bmp.Height);
			Imaging.BitmapData data=bmp.LockBits(rect,Imaging.ImageLockMode.ReadWrite,Imaging.PixelFormat.Format32bppArgb);

			System.IntPtr scan0=data.Scan0;
			int M=bmp.Width*bmp.Height;
			unsafe{
				Color32Argb* p=(Color32Argb*)(void*)scan0;
				Color32Argb* pM=p+M;
				while(p<pM){*p=~*p;p++;}
			}
			bmp.UnlockBits(data);
			return bmp;
		}
		/// <summary>
		/// 指定した色で指定した System.Drawing.Image を塗りつぶして返します。
		/// </summary>
		/// <param name="source">塗りつぶす前の画像を指定します。</param>
		/// <param name="color">塗り潰すのに使用する色を指定します。</param>
		/// <returns>塗り潰した結果の Bitmap を返します。</returns>
		public static System.Drawing.Bitmap ClearRGB(System.Drawing.Image source,Color32Argb color){
			return ClearRGB(new System.Drawing.Bitmap(source),color);
		}
		/// <summary>
		/// 指定した色で指定した System.Drawing.Bitmap を塗りつぶして返します。
		/// 透明度は保持します。
		/// </summary>
		/// <param name="source">塗りつぶす前の画像を指定します。</param>
		/// <param name="color">塗り潰すのに使用する色を指定します。</param>
		/// <returns>塗り潰した結果の Bitmap を返します。</returns>
		public static System.Drawing.Bitmap ClearRGB(System.Drawing.Bitmap source,Color32Argb color){
			System.Drawing.Bitmap bmp=(System.Drawing.Bitmap)source.Clone();
			System.Drawing.Rectangle rect=new System.Drawing.Rectangle(0,0,bmp.Width,bmp.Height);
			Imaging.BitmapData data=bmp.LockBits(rect,Imaging.ImageLockMode.ReadWrite,Imaging.PixelFormat.Format32bppArgb);
			
			System.IntPtr scan0=data.Scan0;
			int M=bmp.Width*bmp.Height;
			unsafe{
				Color32Argb* p=(Color32Argb*)(void*)scan0;
				for(int cnt=0;cnt<M;cnt++){
					p->R=color.R;
					p->G=color.G;
					p->B=color.B;
					p++;
				}
			}
			bmp.UnlockBits(data);
			return bmp;
		}

		/// <summary>
		/// 指定した Bitmap の或る色の Pixel を全て他の色に置き換えます。
		/// </summary>
		/// <param name="bmp">変換を施す対象の Bitmap を指定します。</param>
		/// <param name="before">置き換え前の色を指定します。</param>
		/// <param name="after">置き換え後の色を指定します。</param>
		public static void ReplaceColor(System.Drawing.Bitmap bmp,Color32Argb before,Color32Argb after){
			System.Drawing.Rectangle rect=new System.Drawing.Rectangle(0,0,bmp.Width,bmp.Height);
			Imaging.BitmapData data=bmp.LockBits(rect,Imaging.ImageLockMode.ReadWrite,Imaging.PixelFormat.Format32bppArgb);
			
			System.IntPtr scan0=data.Scan0;
			int M=bmp.Width*bmp.Height;
			unsafe{
				Color32Argb* p=(Color32Argb*)(void*)scan0;
				Color32Argb* pM=p+M;
				for(;p<pM;p++){
					if(*p==before)*p=after;
				}
			}
			bmp.UnlockBits(data);
		}
	}

	/// <summary>
	/// 色々なアイコンを提供します。
	/// </summary>
	public static class Icons {
		private static Gen::Dictionary<string,System.Drawing.Bitmap> bmps;
		private static Gen::Dictionary<string,System.Reflection.Assembly> asms;
		private const string FORMS="System.Windows.Forms";
		private const string DESIGN="System.Design";
		static Icons(){
			bmps=new System.Collections.Generic.Dictionary<string,System.Drawing.Bitmap>();
			asms=new System.Collections.Generic.Dictionary<string,System.Reflection.Assembly>();
			asms[FORMS]=System.Reflection.Assembly.GetAssembly(typeof(System.Windows.Forms.Form));
			asms[DESIGN]=System.Reflection.Assembly.GetAssembly(typeof(System.ComponentModel.Design.CollectionEditor));
		}
		/// <summary>
		/// 指定したリソースを System.Drawing.Bitmap として取得します。
		/// </summary>
		/// <param name="asm">リソース読み取り元のアセンブリ識別文字列を指定します。</param>
		/// <param name="key">リソースの名前を指定します。</param>
		/// <param name="transparent">左上の色を投下させるかどうかを指定します。</param>
		/// <returns>取得した Bitmap を返します。</returns>
		private static System.Drawing.Bitmap GetBitmap(string asm,string key,bool transparent){
			if(bmps.ContainsKey(key))return bmps[key];

			System.Drawing.Bitmap bmp=ReadBitmap(asms[asm],key);
			if(transparent)afh.Drawing.BitmapEffect.ReplaceColor(
				bmp,
				(afh.Drawing.Color32Argb)bmp.GetPixel(0,0),
				(afh.Drawing.Color32Argb)System.Drawing.Color.Transparent
				);

			return bmps[key]=bmp;
		}
		private static System.Drawing.Bitmap ReadBitmap(System.Reflection.Assembly asm,string key) {
			using(System.IO.Stream str=asm.GetManifestResourceStream(key)) {
				System.Drawing.Bitmap bmp0=new System.Drawing.Bitmap(str);

				System.Drawing.Bitmap bmp=ChangePixelFormat(bmp0);

				/* 何故か PixelFormat が変わらない。
				System.Drawing.Bitmap bmp=bmp0.Clone(
					new System.Drawing.RectangleF(0,0,bmp0.Width,bmp0.Height),
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
					);//*/
				
				bmp0.Dispose();
				return bmp;
			}
		}
		private static System.Drawing.Bitmap ChangePixelFormat(System.Drawing.Bitmap bmp){
			System.Drawing.Bitmap ret=new System.Drawing.Bitmap(bmp.Width,bmp.Height,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Rectangle rect=new System.Drawing.Rectangle(0,0,bmp.Width,bmp.Height);
			Imaging.BitmapData bmpdat=bmp.LockBits(rect,System.Drawing.Imaging.ImageLockMode.ReadOnly,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Imaging.BitmapData retdat=ret.LockBits(rect,System.Drawing.Imaging.ImageLockMode.WriteOnly,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			unsafe{
				int* src=(int*)bmpdat.Scan0;
				int* dst=(int*)retdat.Scan0;
				int* srcM=src+bmp.Width*bmp.Height;
				while(src<srcM)*dst++=*src++;
			}
			ret.UnlockBits(retdat);
			bmp.UnlockBits(bmpdat);

			return ret;
		}
		/// <summary>
		/// [上に移動] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap SortUp{
			get{
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.SortUp.ico",false);
				//return GetBitmap(DESIGN,"System.ComponentModel.Design.SortUp.ico",false);
			}
		}
		/// <summary>
		/// [下に移動] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap SortDown {
			get {
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.SortDown.ico",false);
				//return GetBitmap(DESIGN,"System.ComponentModel.Design.SortDown.ico",false);
			}
		}
		/// <summary>
		/// [削除] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Delete {
			get {
				return GetBitmap(DESIGN,"System.Windows.Forms.Design.Delete.ico",false);
				//return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.Delete.ico",false); // ← GDI+ でエラーが起こる。
			}
		}
		/// <summary>
		/// [追加] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap AddNew {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.BindingNavigator.AddNew.bmp",true);
			}
		}
		/// <summary>
		/// [保存] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Save{
			get{
				return GetBitmap(FORMS,"System.Windows.Forms.save.bmp",true);
			}
		}
		/// <summary>
		/// [開く] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Open {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.open.bmp",true);
			}
		}
		/// <summary>
		/// [貼り付け] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Paste {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.paste.bmp",true);
			}
		}
		/// <summary>
		/// [切り取り] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Cut {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.cut.bmp",true);
			}
		}
		/// <summary>
		/// [コピー] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Copy {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.copy.bmp",true);
			}
		}
		/// <summary>
		/// [削除] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Delete2 {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.delete.bmp",true);
			}
		}
		/// <summary>
		/// [印刷] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Print {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.print.bmp",true);
			}
		}
		/// <summary>
		/// [新規作成] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap New {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.new.bmp",true);
			}
		}
		/// <summary>
		/// [ヘルプ] アイコンを Bitmap で返します。
		/// </summary>
		public static System.Drawing.Bitmap Help {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.help.bmp",true);
			}
		}
		/// <summary>
		/// 左右の矢印のアイコンを Bitmap で返します。
		/// このアイコンは左矢印と右矢印が上下に重なった模様のアイコンです。
		/// </summary>
		public static System.Drawing.Bitmap LeftRight{
			get{
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.ListControls.DataGridPagingPage.ico",true);
			}
		}
	}

	/*
	/// <summary>
	/// 点を扱うクラスです。
	/// </summary>
	public static class Vector2{
		/// <summary>
		/// <see cref="Gdi::PointF"/> で表現された点を
		/// <see cref="Gdi::Point"/> に変換します。
		/// </summary>
		/// <param name="p">変換元の <see cref="Gdi::PointF"/> を指定します。</param>
		/// <returns>変換後の <see cref="Gdi::Point"/> を返します。</returns>
		public static Gdi::Point ToPoint(Gdi::PointF p){
			return new Gdi::Point((int)p.X,(int)p.Y);
		}
	}
	//*/
}