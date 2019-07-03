using Imaging=System.Drawing.Imaging;
using Interop=System.Runtime.InteropServices;
using Gen=System.Collections.Generic;
using Gdi=System.Drawing;

namespace afh.Drawing{
	/// <summary>
	/// �F�� ARGB �ŕ\�� 32bit �l�̍\���̂ł��B
	/// </summary>
	[Interop.StructLayout(Interop.LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct Color32Argb{
		/// <summary>
		/// �̋��x��\���܂��B
		/// </summary>
		public byte B;
		/// <summary>
		/// �΂̋��x��\���܂��B
		/// </summary>
		public byte G;
		/// <summary>
		/// �Ԃ̋��x��\���܂��B
		/// </summary>
		public byte R;
		/// <summary>
		/// �A���t�@�l (�s�����x) ���w�肵�܂��B
		/// </summary>
		public byte A;

		/// <summary>
		/// �V�A�������̗ʂ��擾���͐ݒ肵�܂��B
		/// </summary>
		public byte C{
			get{return (byte)(255-this.R);}
			set{this.R=(byte)(255-value);}
		}
		/// <summary>
		/// �}�[���^�����̗ʂ��擾���͐ݒ肵�܂��B
		/// </summary>
		public byte M{
			get{return (byte)(255-this.G);}
			set{this.G=(byte)(255-value);}
		}
		/// <summary>
		/// �������̗ʂ��擾���͐ݒ肵�܂��B
		/// </summary>
		public byte Y{
			get{return (byte)(255-this.B);}
			set{this.B=(byte)(255-value);}
		}

		#region Operators
		/// <summary>
		/// �F�𔽓]���܂��B
		/// </summary>
		/// <param name="color">���]�O�̐F���w�肵�܂��B</param>
		/// <returns>�v�Z���ʂ̔��]�����F��Ԃ��܂��B</returns>
		public static unsafe Color32Argb operator ~(Color32Argb color){
			uint icol=*(uint*)&color;
			icol=~(icol&0x00ffffff)|(icol&0xff000000);
			return *(Color32Argb*)&icol;
		}
		//-----------------------------------------------------------
		//			cast<System.Int32>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb �̐F�� int �ŕ\�������F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color32Argb �`���̐F���w�肵�܂��B</param>
		/// <returns>System.Int32 �ŕ\�������F��Ԃ��܂��B</returns>
		public static unsafe explicit operator int(Color32Argb color){return *(int*)&color;}
		/// <summary>
		/// int �ŕ\�������F�� Color32Argb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">System.Int32 �ŕ\�������F���w�肵�܂��B</param>
		/// <returns>Color32Argb �`���̐F��Ԃ��܂��B</returns>
		public static unsafe implicit operator Color32Argb(int color){return *(Color32Argb*)&color;}
		//-----------------------------------------------------------
		//			cast<System.UInt32>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb �̐F�� uint �ŕ\�������F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color32Argb �`���̐F���w�肵�܂��B</param>
		/// <returns>System.UInt32 �ŕ\�������F��Ԃ��܂��B</returns>
		public static unsafe explicit operator uint(Color32Argb color){return *(uint*)&color;}
		/// <summary>
		/// int �ŕ\�������F�� Color32Argb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">System.UInt32 �ŕ\�������F���w�肵�܂��B</param>
		/// <returns>Color32Argb �`���̐F��Ԃ��܂��B</returns>
		public static unsafe implicit operator Color32Argb(uint color){return *(Color32Argb*)&color;}
		//-----------------------------------------------------------
		//			cast<afh.Drawing.ColorRef>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb �̐F�� Color24Rgb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color32Argb �`���̐F���w�肵�܂��B</param>
		/// <returns>Color24Rgb �`���̐F��Ԃ��܂��B</returns>
		public static implicit operator ColorRef(Color32Argb color){
			return new ColorRef(color.R,color.G,color.G);
		}
		/// <summary>
		/// Color24Rgb �̐F�� Color32Argb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color24Rgb �`���̐F���w�肵�܂��B</param>
		/// <returns>Color32Argb �`���̐F��Ԃ��܂��B</returns>
		public static implicit operator Color32Argb(ColorRef color){
			return new Color32Argb(255,color.R,color.G,color.B);
		}
		//-----------------------------------------------------------
		//			cast<System.Drawing.Color>
		//-----------------------------------------------------------
		/// <summary>
		/// System.Drawing.Color �̐F�� Color32Argb �ɕϊ����܂��B
		/// </summary>
		/// <param name="color">System.Drawing.Color �ŕ\�����ꂽ�F���w�肵�܂��B</param>
		/// <returns>Color32Argb �`���ɕϊ������F��Ԃ��܂��B</returns>
		public static implicit operator Color32Argb(System.Drawing.Color color){
			return (Color32Argb)color.ToArgb();
		}
		/// <summary>
		/// Color32Argb �̐F�� System.Drawing.Color �ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color32Argb �`���̐F���w�肵�܂��B</param>
		/// <returns>System.Drawing.Color �`���ɕϊ������F��Ԃ��܂��B</returns>
		public static implicit operator System.Drawing.Color(Color32Argb color){
			return System.Drawing.Color.FromArgb((int)color);
		}
		//-----------------------------------------------------------
		//			Identity
		//-----------------------------------------------------------
		/// <summary>
		/// �w�肵����̐F�����������ǂ����𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��r����F���w�肵�܂��B</param>
		/// <param name="r">��r����F���w�肵�܂��B</param>
		/// <returns>�������ꍇ�� true ��Ԃ��܂��B����ȊO�̎��� false ��Ԃ��܂��B</returns>
		public static bool operator ==(Color32Argb l,Color32Argb r){
			return (int)l==(int)r;
		}
		/// <summary>
		/// �w�肵����̐F���قȂ邩�ǂ����𔻒肵�܂��B
		/// </summary>
		/// <param name="l">��r����F���w�肵�܂��B</param>
		/// <param name="r">��r����F���w�肵�܂��B</param>
		/// <returns>�قȂ�ꍇ�� true ��Ԃ��܂��B����ȊO�̎��� false ��Ԃ��܂��B</returns>
		public static bool operator !=(Color32Argb l,Color32Argb r){
			return (int)l!=(int)r;
		}
		/// <summary>
		/// �w�肵�� object �Ƃ��̃C���X�^���X�����������ǂ����𔻒肵�܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ� object ���w�肵�܂��B</param>
		/// <returns>�������ƕ]���ł���ꍇ�� true ��Ԃ��܂��B</returns>
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
		/// �n�b�V���l���v�Z���܂��B
		/// </summary>
		/// <returns>�v�Z�����n�b�V���l��Ԃ��܂��B</returns>
		public override int GetHashCode() {
			return ((int)this).GetHashCode();
		}
		#endregion

		/// <summary>
		/// Color32Argb �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="A">�s�����x���w�肵�܂��B</param>
		/// <param name="R">�Ԃ̋��x���w�肵�܂��B</param>
		/// <param name="G">�΂̋��x���w�肵�܂��B</param>
		/// <param name="B">�̋��x���w�肵�܂��B</param>
		public Color32Argb(byte A,byte R,byte G,byte B){
			this.A=A;this.R=R;this.G=G;this.B=B;
		}
		/// <summary>
		/// A (�s�����x), R (�Ԃ̋��x), G (�΂̋��x), B (�̋��x) ���� ColorArgb32 �C���X�^���X���擾���܂��B
		/// </summary>
		/// <param name="a">�s�����x���w�肵�܂��B</param>
		/// <param name="r">�Ԃ̋��x���w�肵�܂��B</param>
		/// <param name="g">�΂̋��x���w�肵�܂��B</param>
		/// <param name="b">�̋��x���w�肵�܂��B</param>
		/// <returns>�w�肵���l�����ɂ��č쐬���� ColorArgb32 �̒l��Ԃ��܂��B</returns>
		public static Color32Argb FromArgb(byte a,byte r,byte g,byte b){
			return new Color32Argb(a,r,g,b);
		}
		/// <summary>
		/// R (�Ԃ̋��x), G (�΂̋��x), B (�̋��x) ���� ColorArgb32 �C���X�^���X���擾���܂��B
		/// </summary>
		/// <param name="r">�Ԃ̋��x���w�肵�܂��B</param>
		/// <param name="g">�΂̋��x���w�肵�܂��B</param>
		/// <param name="b">�̋��x���w�肵�܂��B</param>
		/// <returns>�w�肵���l�����ɂ��č쐬���� ColorArgb32 �̒l��Ԃ��܂��B</returns>
		public static Color32Argb FromRgb(byte r,byte g,byte b){
			return new Color32Argb(0xff,r,g,b);
		}
		/// <summary>
		/// H (�F��; Hue), S (�ʓx), V (���x) ���� ColorArgb32 �C���X�^���X���擾���܂��B
		/// </summary>
		/// <param name="h">�F�����w�肵�܂��B�F���̏ڍׂɊւ��ẮA<see cref="ColorHsv"/> ���Q�Ƃ��ĉ������B</param>
		/// <param name="s">�ʓx���w�肵�܂��B</param>
		/// <param name="v">���x���w�肵�܂��B</param>
		/// <returns>�w�肵���l�����ɂ��č쐬���� ColorArgb32 �̒l��Ԃ��܂��B</returns>
		public static Color32Argb FromHsv(double h,byte s,byte v){
			return (Color32Argb)new ColorHsv(0xff,h,s,v);
		}
		/// <summary>
		/// C (�V�A���̗�), M (�}�[���^�̗�), Y (���̗�) ���� ColorArgb32 �C���X�^���X���擾���܂��B
		/// </summary>
		/// <param name="c">�V�A���̗ʂ��w�肵�܂��B</param>
		/// <param name="m">�}�[���^�̗ʂ��w�肵�܂��B</param>
		/// <param name="y">���̗ʂ��w�肵�܂��B</param>
		/// <returns>�w�肵���l�����ɂ��č쐬���� ColorArgb32 �̒l��Ԃ��܂��B</returns>
		public static Color32Argb FromCmy(byte c,byte m,byte y){
			return new Color32Argb(0xff,(byte)(255-c),(byte)(255-m),(byte)(255-y));
		}

		/// <summary>
		/// �F�𕶎���Ƃ��ĕ\�����܂��B
		/// </summary>
		/// <returns>ARGB �̊e���x��������������ŕԂ��܂��B</returns>
		public override string ToString() {
			return base.ToString()+string.Format("{{A:0x{0:X2}; R:0x{1:X2}; G:0x{2:X2}; B:0x{3:X2};}}",this.A,this.R,this.G,this.B);
		}

		/// <summary>
		/// ���F��\�� Color32Argb ���擾���܂��B
		/// </summary>
		public static Color32Argb Black{get{return 0xff000000;}}
		/// <summary>
		/// ���F��\�� Color32Argb ���擾���܂��B
		/// </summary>
		public static Color32Argb White{get{return 0xffffffff;}}
		/// <summary>
		/// �ԐF��\�� Color32Argb ���擾���܂��B
		/// </summary>
		public static Color32Argb Red  {get{return 0xffff0000;}}
		/// <summary>
		/// �ΐF��\�� Color32Argb ���擾���܂��B
		/// </summary>
		public static Color32Argb Green{get{return 0xff00ff00;}}
		/// <summary>
		/// �F��\�� Color32Argb ���擾���܂��B
		/// </summary>
		public static Color32Argb Blue {get{return 0xff0000ff;}}
	}
	/// <summary>
	/// �F���E�ʓx�E���x�ɂ��F�\���ł��B
	/// </summary>
	[Interop::StructLayout(Interop::LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct ColorHsv{
		/// <summary>
		/// �F���E�ʓx�E���x���w�肵�� ColorHsv �����������܂��B
		/// </summary>
		/// <param name="hue">�F�����w�肵�܂��B</param>
		/// <param name="saturation">�ʓx���w�肵�܂��B</param>
		/// <param name="value">���x���w�肵�܂��B</param>
		public ColorHsv(double hue,byte saturation,byte value){
			this.H=hue;
			this.S=saturation;
			this.V=value;
			this.A=0xff;
		}
		/// <summary>
		/// �s�����x�E�F���E�ʓx�E���x���w�肵�� ColorHsv �����������܂��B
		/// </summary>
		/// <param name="alpha">�s�����x���w�肵�܂��B</param>
		/// <param name="hue">�F�����w�肵�܂��B</param>
		/// <param name="saturation">�ʓx���w�肵�܂��B</param>
		/// <param name="value">���x���w�肵�܂��B</param>
		public ColorHsv(byte alpha,double hue,byte saturation,byte value) {
			this.H=hue;
			this.S=saturation;
			this.V=value;
			this.A=alpha;
		}
		/// <summary>
		/// �F����ێ����܂��B
		/// �F���� 0.0 ���� 360.0 ���̒l�ŕ\������܂��B
		/// ���A���͈̔͂̊O�̐F���́A360 ��@�ɂ��ď�]����������Ɠ��ꎋ����܂��B
		/// <para>
		/// 0.0 �͐Ԃ�\���܂��B60.0 �͉���\���܂��B120.0 �͗΂�\���܂��B
		/// 180.0 �̓V�A����\���܂��B240.0 �͐�\���܂��B300.0 �� �}�[���^��\���܂��B
		/// </para>
		/// </summary>
		public double H;
		/// <summary>
		/// �ʓx��ێ����܂��B
		/// 255 ���ł��N�₩�ȐF�ł��鎖�������A
		/// 0 ���ł��݂��F (�O���[�X�P�[��) �ł��鎖�������܂��B
		/// </summary>
		public byte S;
		/// <summary>
		/// ���x��ێ����܂��B0 ���ł��Â����������A
		/// 255 ���ł����邢�F�ł��鎖�������܂��B
		/// </summary>
		public byte V;
		/// <summary>
		/// �A���t�@�l (�s�����x) ��ێ����܂��B
		/// 0 �������ȐF�ł��鎖�������A255 ���s�����ȐF�ł��鎖�������܂��B
		/// </summary>
		public byte A;

		/// <summary>
		/// �F�𕶎���Ƃ��ĕ\�����܂��B
		/// </summary>
		/// <returns>ARGB �̊e���x��������������ŕԂ��܂��B</returns>
		public override string ToString() {
			return base.ToString()+string.Format("{{A:0x{0:X2}; H:{1:F1}; S:0x{2:X2}; V:0x{3:X2};}}",this.A,this.H,this.S,this.V);
		}

		//-----------------------------------------------------------
		//			cast<afh.Drawing.ColorHsv>
		//-----------------------------------------------------------
		/// <summary>
		/// Color32Argb �̐F�� Color24Rgb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color32Argb �`���̐F���w�肵�܂��B</param>
		/// <returns>Color24Rgb �`���̐F��Ԃ��܂��B</returns>
		public unsafe static explicit operator ColorHsv(Color32Argb color){
			byte* pB=(byte*)&color;
			int min,max;

			//-- afh.Math.MinMax ���q��
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
		/// Color24Rgb �̐F�� Color32Argb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color24Rgb �`���̐F���w�肵�܂��B</param>
		/// <returns>Color32Argb �`���̐F��Ԃ��܂��B</returns>
		public unsafe static explicit operator Color32Argb(ColorHsv color){
			if(color.S==0)return new Color32Argb(color.A,color.V,color.V,color.V);
			Color32Argb ret=new Color32Argb();
			byte* pB=(byte*)&ret;
			pB[3]=color.A;

			// f: color.H/60 ������
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
	/// �F�� RGB �ŕ\�� 32bit �l�̍\���̂ł��B��� 8 bit �͎g�p����܂���B
	/// </summary>
	[Interop.StructLayout(Interop.LayoutKind.Sequential,Pack=1)]
	[System.Serializable]
	public struct ColorRef{
		/// <summary>
		/// �Ԃ̋��x��\���܂��B
		/// </summary>
		public byte R;
		/// <summary>
		/// �̋��x��\���܂��B
		/// </summary>
		public byte B;
		/// <summary>
		/// �΂̋��x��\���܂��B
		/// </summary>
		public byte G;
		/// <summary>
		/// �A���t�@�l (�s�����x) ���w�肵�܂��B�g�p����܂���B��� 0 �ł���K�v������܂��B
		/// </summary>
		private byte A;

		/// <summary>
		/// Color24Rgb �̐F�� int �ŕ\�������F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">Color24Rgb �`���̐F���w�肵�܂��B</param>
		/// <returns>System.Int32 �ŕ\�������F��Ԃ��܂��B</returns>
		public static unsafe explicit operator uint(ColorRef color){return *(uint*)&color; }
		/// <summary>
		/// int �ŕ\�������F�� Color24Rgb �̐F�ɕϊ����܂��B
		/// </summary>
		/// <param name="color">System.Int32 �ŕ\�������F���w�肵�܂��B</param>
		/// <returns>Color24Rgb �`���̐F��Ԃ��܂��B</returns>
		public static unsafe implicit operator ColorRef(uint color){return *(ColorRef*)&color; }

		/// <summary>
		/// Color24Rgb �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="R">�Ԃ̋��x���w�肵�܂��B</param>
		/// <param name="G">�΂̋��x���w�肵�܂��B</param>
		/// <param name="B">�̋��x���w�肵�܂��B</param>
		public ColorRef(byte R,byte G,byte B) {
			this.A=0;this.R=R;this.G=G;this.B=B;
		}
	}
	/// <summary>
	/// Bitmap �ɗl�X�Ȍ��ʂ�^����֐���񋟂���N���X�ł��B
	/// </summary>
	public static class BitmapEffect{
		/// <summary>
		/// �w�肵�� Bitmap �̐F�𔽓]���܂��B
		/// </summary>
		/// <param name="source">���]�O�̉摜���w�肵�܂��B</param>
		/// <returns>���]��̉摜���w�肵�܂��B</returns>
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
		/// �w�肵���F�Ŏw�肵�� System.Drawing.Image ��h��Ԃ��ĕԂ��܂��B
		/// </summary>
		/// <param name="source">�h��Ԃ��O�̉摜���w�肵�܂��B</param>
		/// <param name="color">�h��ׂ��̂Ɏg�p����F���w�肵�܂��B</param>
		/// <returns>�h��ׂ������ʂ� Bitmap ��Ԃ��܂��B</returns>
		public static System.Drawing.Bitmap ClearRGB(System.Drawing.Image source,Color32Argb color){
			return ClearRGB(new System.Drawing.Bitmap(source),color);
		}
		/// <summary>
		/// �w�肵���F�Ŏw�肵�� System.Drawing.Bitmap ��h��Ԃ��ĕԂ��܂��B
		/// �����x�͕ێ����܂��B
		/// </summary>
		/// <param name="source">�h��Ԃ��O�̉摜���w�肵�܂��B</param>
		/// <param name="color">�h��ׂ��̂Ɏg�p����F���w�肵�܂��B</param>
		/// <returns>�h��ׂ������ʂ� Bitmap ��Ԃ��܂��B</returns>
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
		/// �w�肵�� Bitmap �̈���F�� Pixel ��S�đ��̐F�ɒu�������܂��B
		/// </summary>
		/// <param name="bmp">�ϊ����{���Ώۂ� Bitmap ���w�肵�܂��B</param>
		/// <param name="before">�u�������O�̐F���w�肵�܂��B</param>
		/// <param name="after">�u��������̐F���w�肵�܂��B</param>
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
	/// �F�X�ȃA�C�R����񋟂��܂��B
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
		/// �w�肵�����\�[�X�� System.Drawing.Bitmap �Ƃ��Ď擾���܂��B
		/// </summary>
		/// <param name="asm">���\�[�X�ǂݎ�茳�̃A�Z���u�����ʕ�������w�肵�܂��B</param>
		/// <param name="key">���\�[�X�̖��O���w�肵�܂��B</param>
		/// <param name="transparent">����̐F�𓊉������邩�ǂ������w�肵�܂��B</param>
		/// <returns>�擾���� Bitmap ��Ԃ��܂��B</returns>
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

				/* ���̂� PixelFormat ���ς��Ȃ��B
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
		/// [��Ɉړ�] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap SortUp{
			get{
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.SortUp.ico",false);
				//return GetBitmap(DESIGN,"System.ComponentModel.Design.SortUp.ico",false);
			}
		}
		/// <summary>
		/// [���Ɉړ�] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap SortDown {
			get {
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.SortDown.ico",false);
				//return GetBitmap(DESIGN,"System.ComponentModel.Design.SortDown.ico",false);
			}
		}
		/// <summary>
		/// [�폜] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Delete {
			get {
				return GetBitmap(DESIGN,"System.Windows.Forms.Design.Delete.ico",false);
				//return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.Delete.ico",false); // �� GDI+ �ŃG���[���N����B
			}
		}
		/// <summary>
		/// [�ǉ�] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap AddNew {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.BindingNavigator.AddNew.bmp",true);
			}
		}
		/// <summary>
		/// [�ۑ�] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Save{
			get{
				return GetBitmap(FORMS,"System.Windows.Forms.save.bmp",true);
			}
		}
		/// <summary>
		/// [�J��] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Open {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.open.bmp",true);
			}
		}
		/// <summary>
		/// [�\��t��] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Paste {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.paste.bmp",true);
			}
		}
		/// <summary>
		/// [�؂���] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Cut {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.cut.bmp",true);
			}
		}
		/// <summary>
		/// [�R�s�[] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Copy {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.copy.bmp",true);
			}
		}
		/// <summary>
		/// [�폜] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Delete2 {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.delete.bmp",true);
			}
		}
		/// <summary>
		/// [���] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Print {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.print.bmp",true);
			}
		}
		/// <summary>
		/// [�V�K�쐬] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap New {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.new.bmp",true);
			}
		}
		/// <summary>
		/// [�w���v] �A�C�R���� Bitmap �ŕԂ��܂��B
		/// </summary>
		public static System.Drawing.Bitmap Help {
			get {
				return GetBitmap(FORMS,"System.Windows.Forms.help.bmp",true);
			}
		}
		/// <summary>
		/// ���E�̖��̃A�C�R���� Bitmap �ŕԂ��܂��B
		/// ���̃A�C�R���͍����ƉE��󂪏㉺�ɏd�Ȃ����͗l�̃A�C�R���ł��B
		/// </summary>
		public static System.Drawing.Bitmap LeftRight{
			get{
				return GetBitmap(DESIGN,"System.Web.UI.Design.WebControls.ListControls.DataGridPagingPage.ico",true);
			}
		}
	}

	/*
	/// <summary>
	/// �_�������N���X�ł��B
	/// </summary>
	public static class Vector2{
		/// <summary>
		/// <see cref="Gdi::PointF"/> �ŕ\�����ꂽ�_��
		/// <see cref="Gdi::Point"/> �ɕϊ����܂��B
		/// </summary>
		/// <param name="p">�ϊ����� <see cref="Gdi::PointF"/> ���w�肵�܂��B</param>
		/// <returns>�ϊ���� <see cref="Gdi::Point"/> ��Ԃ��܂��B</returns>
		public static Gdi::Point ToPoint(Gdi::PointF p){
			return new Gdi::Point((int)p.X,(int)p.Y);
		}
	}
	//*/
}