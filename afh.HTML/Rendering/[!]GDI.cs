using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using afh.Rendering;
using Interop=System.Runtime.InteropServices;
using HDC=System.IntPtr;
using Color=afh.Drawing.Color32Argb;
using ColorRef=afh.Drawing.ColorRef;

namespace afh.Gdi{
	using afh.Win32;

	public class DeviceContext:System.IDisposable{
		private HDC hdc;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g">
		/// �쐬���� Graphics ���w�肵�܂��B
		/// ���� Graphics �𑀍삷��O�ɕK�����̃C���X�^���X�� Dispose ���Ăяo���ĉ������B
		/// </param>
		public DeviceContext(Gdi::Graphics g){
			this.hdc=g.GetHdc();
			this.dispose=new System.Action<HDC>(g.ReleaseHdc);
		}

		private System.Action<HDC> dispose;

		public void Dispose(){
			if(this.dispose!=null)dispose(this.hdc);
		}

		public void LineTo(int x,int y){
			if(!LineTo(this.hdc,x,y))
				throw new System.Exception("���݈ʒu�̈ړ��Ɏ��s���܂����B");
		}
		[Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool LineTo(HDC hdc,int nXEnd,int nYEnd);

		public void MoveTo(int x,int y){
			if(!MoveToEx(this.hdc,x,y,System.IntPtr.Zero))
				throw new System.Exception("���݈ʒu�̈ړ��Ɏ��s���܂����B");
		}
		[Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool MoveToEx(HDC hdc,int X,int Y,[Interop::Out]System.IntPtr lpPoint);

		//===========================================================
		//		LineDDA
		//===========================================================
		/// <summary>
		/// �w�肵���_�Ɠ_�����Ԓ������\������_��񋓂��܂��B
		/// ���ꂼ��̓_�ɑ΂��ď������s���֐����w�肵�܂��B
		/// </summary>
		/// <typeparam name="T">�֐��ɓn���A�v���P�[�V������`�̃I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
		/// <param name="nXStart">�J�n�_�� x ���W���w�肵�܂��B</param>
		/// <param name="nYStart">�J�n�_�� y ���W���w�肵�܂��B</param>
		/// <param name="nXEnd">�I�[�_�� x ���W���w�肵�܂��B</param>
		/// <param name="nYEnd">�I�[�_�� y ���W���w�肵�܂��B</param>
		/// <param name="lpLineFunc">�������s���֐����w�肵�܂��B</param>
		/// <param name="data">�֐��ɓn���A�v���P�[�V������`�̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>���������ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public static bool LineDDA<T>(int nXStart,int nYStart,int nXEnd,int nYEnd,LineDDAProc<T> lpLineFunc,T data){
			return internalLineDDA(nXStart,nYStart,nXEnd,nYEnd,
				delegate(int x,int y,System.IntPtr p){lpLineFunc(x,y,data);},
				System.IntPtr.Zero);
		}
		/// <summary>
		/// �w�肵���_�Ɠ_�����Ԓ������\������_��񋓂��܂��B
		/// ���ꂼ��̓_�ɑ΂��ď������s���֐����w�肵�܂��B
		/// </summary>
		/// <param name="nXStart">�J�n�_�� x ���W���w�肵�܂��B</param>
		/// <param name="nYStart">�J�n�_�� y ���W���w�肵�܂��B</param>
		/// <param name="nXEnd">�I�[�_�� x ���W���w�肵�܂��B</param>
		/// <param name="nYEnd">�I�[�_�� y ���W���w�肵�܂��B</param>
		/// <param name="lpLineFunc">�������s���֐����w�肵�܂��B</param>
		/// <returns>���������ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public static bool LineDDA(int nXStart,int nYStart,int nXEnd,int nYEnd,LineDDAProc lpLineFunc){
			return internalLineDDA(nXStart,nYStart,nXEnd,nYEnd,
				delegate(int x,int y,System.IntPtr p){lpLineFunc(x,y);},
				System.IntPtr.Zero);
		}
		[Interop::DllImport("gdi32.dll",EntryPoint="LineDDA")]
		private static extern bool internalLineDDA(int nXStart,int nYStart,int nXEnd,int nYEnd,internalLineDDAProc lpLineFunc,System.IntPtr lpData);
		private delegate void internalLineDDAProc(int X,int Y,System.IntPtr lpData);

		#region ������/�t�H���g
		/// <summary>
		/// �������`�悵�܂��B
		/// </summary>
		/// <param name="text">�`�悷�镶������w�肵�܂��B</param>
		/// <param name="rect">�������`�悷��Ώۂ̋�`���w�肵�܂��B</param>
		/// <param name="flags">�������`�悷��ۂ̏ڍׂȎw����s���܂��B</param>
		public int DrawString(string text,Gdi::Rectangle rect,StringFormat flags){
			RECT rc=rect;
			int ret=DrawText(this.hdc,text,text.Length,ref rc,flags);
			if(ret==0)throw new System.Exception("������̕`��Ɏ��s���܂����B");
			return ret;
		}
		[Interop::DllImport("gdi32.dll",CharSet=Interop::CharSet.Auto)]
		private static extern int DrawText(HDC hdc,string lpString,int nCount,ref RECT lpRect,StringFormat uFormat);
		/// <summary>
		/// �������`�悵�܂��B
		/// </summary>
		/// <param name="text">�`�悷�镶������w�肵�܂��B</param>
		/// <param name="x">�������`�悷��ʒu�� x ���W���w�肵�܂��B</param>
		/// <param name="y">�������`�悷��ʒu�� y ���W���w�肵�܂��B</param>
		public void DrawString(string text,int x,int y){
			if(!TextOut(this.hdc,x,y,text,text.Length)){
				throw new System.Exception("������̕`��Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll",CharSet=Interop::CharSet.Auto)]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool TextOut(HDC hdc,int nXStart,int nYStart,string lpString,int cbString);

		/// <summary>
		/// �`��Ɏg�p����t�H���g��ݒ肵�܂��B
		/// </summary>
		public Gdi::Font Font{
			set{
				if(SelectObject(this.hdc,value.ToHfont())==System.IntPtr.Zero)
					throw new System.Exception("�t�H���g�̐ݒ�Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern System.IntPtr SelectObject(HDC hdc,System.IntPtr hgdiobj);

		//===========================================================
		//		Get/SetTextColor
		//		Get/SetBkColor
		//		Get/SetBkMode
		//		Get/SetCharacterExtra
		//===========================================================
		/// <summary>
		/// �����F���擾���͐ݒ肵�܂��B
		/// </summary>
		public Color TextColor{
			get{return GetTextColor(this.hdc);}
			set{
				if(0xffffffff==(uint)SetTextColor(hdc,value))
					throw new System.Exception("�����F�̐ݒ�Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef SetTextColor(HDC hdc,ColorRef color);
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef GetTextColor(HDC hdc);

		/// <summary>
		/// �w�i�F���擾���͐ݒ肵�܂��B
		/// </summary>
		public Color BackColor{
			get{return GetBkColor(this.hdc);}
			set{
				if(0xffffffff==(uint)SetBkColor(hdc,value))
					throw new System.Exception("�w�i�F�̐ݒ�Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef SetBkColor(HDC hdc,ColorRef color);
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef GetBkColor(HDC hdc);

		/// <summary>
		/// �w�i���[�h�̎擾���͐ݒ���s���܂��B
		/// true �ɐݒ肳��Ă��鎞�ɂ́A�w�i�F��`�悵�܂��B
		/// false �ɐݒ肳��Ă��鎞�ɂ͔w�i��`�悹���A���߂��܂��B
		/// </summary>
		public bool Background{
			get{
				switch(GetBkMode(this.hdc)){
					case BkMode.Opaque:
						return true;
					case BkMode.Transparent:
						return false;
					case BkMode.Failed:
						throw new System.Exception("�w�i���[�h�̎擾�Ɏ��s���܂����B");
					default:
						throw new System.Exception("�w�i���[�h�͖��m�̒l�ł��B");
				}
			}
			set{
				if(!SetBkMode(this.hdc,value?BkMode.Opaque:BkMode.Transparent))
					throw new System.Exception("�w�i���[�h�̕ύX�Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool SetBkMode(HDC hdc,BkMode iBkMode);
		[Interop::DllImport("gdi32.dll")]
		private static extern BkMode GetBkMode(HDC hdc);
		private enum BkMode:int{
			Failed=0,
			Transparent=1,
			Opaque=2
		}

		/// <summary>
		/// �����Ԋu���擾���͐ݒ肵�܂��B
		/// �`��̍ۂɂ��̒l�����ꂼ��̕����̊Ԋu�ɉ��Z����܂��B
		/// </summary>
		public int LetterSpacing{
			get{
				int ret=GetCharacterExtra(this.hdc);
				if(ret==unchecked((int)0x80000000))
					throw new System.Exception("�����Ԋu�̎擾�Ɏ��s���܂����B");
				return ret;
			}
			set{
				if(0x80000000==SetCharacterExtra(this.hdc,value))
					throw new System.Exception("�����Ԋu�̐ݒ�Ɏ��s���܂����B");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern int GetCharacterExtra(HDC hdc);
		[Interop::DllImport("gdi32.dll")]
		private static extern uint SetCharacterExtra(HDC hdc,int charExtra);

		//===========================================================
		//		GetRasterizerCaps
		//===========================================================
		/// <summary>
		/// �g���Ă���V�X�e���ɁA�g�p�\�� TrueType �t�H���g�����݂��邩�ǂ������擾���܂��B
		/// </summary>
		public static bool TrueTypeAvailable{
			get{
				if(rs.nSize==0)GetRasterizerStatus();
				return (rs.wFlags&RASTERIZER_STATUS_Flags.TT_Available)!=0;
			}
		}
		/// <summary>
		/// �g���Ă���V�X�e���� TrueType �t�H���g�����������o���邩�ǂ������擾���܂��B
		/// </summary>
		public static bool TrueTypeEnabled{
			get{
				if(rs.nSize==0)GetRasterizerStatus();
				return (rs.wFlags&RASTERIZER_STATUS_Flags.TT_Enabled)!=0;
			}
		}
		/// <summary>
		/// �g�p���Ă���V�X�e���̌��� ID ���擾���܂��B
		/// </summary>
		public static int LanguageId{
			get{
				if(rs.nSize==0) GetRasterizerStatus();
				return rs.nLanguageID;
			}
		}
		private static RASTERIZER_STATUS rs;
		private static void GetRasterizerStatus(){
			rs.nSize=6;//sizeof(RASTERIZER_STATUS)
			if(!GetRasterizerCaps(ref rs,(uint)rs.nSize))
				throw new System.Exception("���X�^���C�U�̏��̎擾�Ɏ��s���܂����B");
		}
		[method:Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool GetRasterizerCaps(ref RASTERIZER_STATUS lprs,uint cb);

		//===========================================================
		//		GetCharacterPlacement
		//===========================================================
		//
		//	���Ɏg���̂��ǂ�������Ȃ��֐��B��ŏڂ��� MSDN ������B
		//
		public SIZE16 GetCharacterPlacement(string text,GCP flags,int maxExtent,ref GCP_RESULTS results){
			return GetCharacterPlacement(this.hdc,text,text.Length,maxExtent,ref results,flags);
		}
		[Interop::DllImport("gdi32.dll",CharSet=Interop::CharSet.Auto)]
		private static extern SIZE16 GetCharacterPlacement(HDC hdc,string lpString,int nCount,int nMaxExtent,ref GCP_RESULTS lpResults,GCP dwFlags);
		#endregion

	}
	public delegate void LineDDAProc(int X,int Y);
	public delegate void LineDDAProc<T>(int X,int Y,T lpData);

	#region ������/�t�H���g
	/// <summary>
	/// ������̕`��̎d�����w�肷��̂Ɏg�p���܂��B
	/// </summary>
	public enum StringFormat:uint{
		/// <summary>
		/// �����`�̈�̏�[�Ƀe�L�X�g�𑵂��܂��i�P��s�̂Ƃ��̂݁j�B
		/// </summary>
		DT_TOP=0x00000000,
		/// <summary>
		/// �e�L�X�g���������ɂ��܂��B
		/// </summary>
		DT_LEFT=0x00000000,
		/// <summary>
		/// �e�L�X�g�𒷕��`�̈���ŉ������ɒ��������ŕ\�����܂��B
		/// </summary>
		DT_CENTER=0x00000001,
		/// <summary>
		/// �e�L�X�g���E�����ɂ��܂��B
		/// </summary>
		DT_RIGHT=0x00000002,
		/// <summary>
		/// �e�L�X�g���c�����ɒ��������ŕ\�����܂��B���̒l�́ADT_SINGLELINE ���w�肵���ꍇ�ɂ����g�p�ł��܂���B
		/// </summary>
		DT_VCENTER=0x00000004,
		/// <summary>
		/// �����`�̈�̉��[�Ƀe�L�X�g�𑵂��܂��BDT_SINGLELINE �Ɠ����Ɏw�肵�Ȃ���΂Ȃ�܂���B
		/// </summary>
		DT_BOTTOM=0x00000008,
		/// <summary>
		/// �e�L�X�g�𕡐��s�ŕ\�����܂��B�܂�Ԃ��́AlpRect �p�����[�^�Ŏw�肵�������`�̈�̒[����P�ꂪ�͂ݏo�������Ŏ����I�ɍs���܂��B
		/// �L�����b�W���^�[���ƃ��C���t�B�[�h�̑g�ݍ��킹�ɂ���Ă��܂�Ԃ���܂��B
		/// </summary>
		DT_WORDBREAK=0x00000010,
		/// <summary>
		/// �e�L�X�g��P��s�ŕ\�����܂��B���Ƃ��A�e�L�X�g���L�����b�W���^�[���⃉�C���t�B�[�h���܂�ł��Ă��A���s����܂���B
		/// </summary>
		DT_SINGLELINE=0x00000020,
		/// <summary>
		/// �^�u������W�J���܂��B����̃^�u�Ԋu�� 8 �����ł��B
		/// ���̃t���O���Z�b�g����ꍇ�ADT_WORD_ELLIPSIS�ADT_PATH_ELLIPSIS�A����� DT_END_ELLIPSIS �͎w��ł��܂���B
		/// </summary>
		DT_EXPANDTABS=0x00000040,
		/// <summary>
		/// �^�u�Ԋu��ݒ肵�܂��B���̒l���w�肵���Ƃ��́AuFormat �p�����[�^�� 15 �r�b�g���� 8 �r�b�g�i���ʃ��[�h�̏�ʃo�C�g�j�ŁA
		/// �^�u�Ԋu�̕��������w�肵�܂��B����̃^�u�Ԋu�� 8 �����ł��B���̒l���w�肵���ꍇ�A
		/// DT_CALCRECT�ADT_EXTERNALLEADING�ADT_INTERNAL�ADT_NOCLIP�A����� DT_NOPREFIX �͎w��ł��܂���B
		/// </summary>
		DT_TABSTOP=0x00000080,
		/// <summary>
		/// �N���b�s���O�����܂���B�`�悪���������Ȃ�܂��B
		/// </summary>
		DT_NOCLIP=0x00000100,
		/// <summary>
		/// �s�̍����ɁA�O�����f�B���O�̍����i�e�L�X�g�̍s�ԂƂ��ēK�؂ȍ����j�����Z���܂��B�ʏ�A�O�����f�B���O�̓e�L�X�g�s�̍����ɉ������܂���B
		/// </summary>
		DT_EXTERNALLEADING=0x00000200,
		/// <summary>
		/// �w�肳�ꂽ�e�L�X�g��\�����邽�߂ɕK�v�Ȓ����`�̈�̕��ƍ����𒲂ׂ܂��B
		/// �����s�e�L�X�g�̏ꍇ�́AlpRect �p�����[�^�Ŏw�肳�ꂽ�����`�̈�̕����g���A
		/// �����`�̈�̉��[���e�L�X�g�̍ŏI�s�̉����̋��E���ɂ܂ōL���܂��B
		/// �e�L�X�g�� 1 �s�ŕ\������ꍇ�́A�����`�̈�̉E�[���s�̍Ō�̕����̉E���̋��E���ɍ����悤�ɕύX���܂��B
		/// �ǂ���̏ꍇ���ADrawText �֐��́A�e�L�X�g�̕`��͍s�킸�A���`���ꂽ�e�L�X�g�̍�����Ԃ��܂��B
		/// </summary>
		DT_CALCRECT=0x00000400,
		/// <summary>
		/// <para>
		/// �v���t�B�b�N�X�����̏������s��Ȃ��悤�ɂ��܂��B�ʏ�́A�j�[���j�b�N�v���t�B�N�X�����́u&�v�́A
		/// ���̎��ɂ��镶���ɉ����i_�j��t���ĕ\������Ƃ̖��߂ł���Ɖ��߂���A
		/// �j�[���j�b�N�v���t�B�b�N�X�����́u&&�v�́A1 �́u&�v��\������Ƃ̖��߂ł���Ɖ��߂���܂��B
		/// DT_NOPREFIX ���w�肷��ƁA���̏������s���Ȃ��Ȃ�܂��B���ɗ�������܂��B 
		/// </para>
		/// <list>
		/// <item><description>���͕�����F   "A&bc&&d"</description></item>
		/// <item><description>�ʏ�F         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "A&bc&&d"</description></item>
		/// </list>
		/// <para>DT_HIDEPREFIX ����� DT_PREFIXONLY �̐������Q�Ƃ��Ă��������B</para>
		/// </summary>
		DT_NOPREFIX=0x00000800,
		/// <summary>
		/// �e�L�X�g�̕\���T�C�Y���v�Z����ۂɃV�X�e���t�H���g���g�p���܂��B
		/// </summary>
		DT_INTERNAL=0x00001000,

		/// <summary>
		/// �����s�G�f�B�b�g�R���g���[�����������Ɠ��������ŕ`�悵�܂��B
		/// ���ɁA���ϕ��������G�f�B�b�g�R���g���[���Ɠ������@�Ōv�Z����A�����I�Ɍ����Ă���Ō�̍s�͕\������܂���B
		/// </summary>
		DT_EDITCONTROL=0x00002000,
		/// <summary>
		/// <para>
		/// �w�肵�������`�̈�Ɏ��܂�悤�ɁA�K�v�ɉ����ăe�L�X�g�̓r�����ȗ����� (...) �ɒu�������܂��B
		/// �~�L���i\�j���܂܂�Ă���e�L�X�g�̏ꍇ�A�Ō�̉~�L���̌��̃e�L�X�g���\�Ȍ���ێ�����܂��B 
		/// </para>
		/// <para>DT_MODIFYSTRING �t���O���w�肵�Ă��Ȃ�����A�����񂪕ύX����邱�Ƃ͂���܂���B</para>
		/// <para>DT_END_ELLIPSIS ����� DT_WORD_ELLIPSIS �̐������Q�Ƃ��Ă��������B</para>
		/// </summary>
		DT_PATH_ELLIPSIS=0x00004000,
		/// <summary>
		/// <para>
		/// ������̍Ō�̕����������`�̈�ɔ[�܂�؂�Ȃ��ꍇ�A�͂ݏo���������؂����A
		/// �����ɏȗ������i...�j���ǉ�����܂��B
		/// ������̍Ō�ł͂Ȃ��ꏊ�ɂ���P�ꂪ�����`�̈悩��͂ݏo���ꍇ�́A�ȗ��L���Ȃ��Ő؂����܂��B
		/// </para><para>
		/// DT_MODIFYSTRING �t���O���Z�b�g����Ă��Ȃ�����A�����񂪕ύX����邱�Ƃ͂���܂���B
		/// </para><para>
		/// DT_PATH_ELLIPSIS ����� DT_WORD_ELLIPSIS �̐������Q�Ƃ��Ă��������B
		/// </para>
		/// </summary>
		DT_END_ELLIPSIS=0x00008000,
		/// <summary>
		/// lpString �p�����[�^���w���o�b�t�@�ɁA���ۂɕ\�����ꂽ��������i�[���܂��B
		/// DT_END_ELLIPSIS �t���O�܂��� DT_PATH_ELLIPSIS �t���O���w�肵���Ƃ��ɂ����Ӗ��������܂���B
		/// </summary>
		DT_MODIFYSTRING=0x00010000,
		/// <summary>
		/// hdc �p�����[�^�Ŏw�肵���f�o�C�X�R���e�L�X�g�őI������Ă���t�H���g���w�u���C�ꂩ�A���r�A�ꂾ�����ꍇ�ɁA
		/// �o�����e�L�X�g���E���獶�ւ̓ǂݎ�菇���ŕ\�����܂��B����̓ǂݎ�菇���́A�ǂ̃e�L�X�g�ł�������E�ł��B
		/// </summary>
		DT_RTLREADING=0x00020000,
		/// <summary>
		/// <para>�����`�̈���ɔ[�܂�Ȃ�����������ꍇ�A�����؂����������ŁA�K���ȗ��L���i...�j��ǉ����܂��B</para>
		/// <para>DT_END_ELLIPSIS ����� DT_PATH_ELLIPSIS �̐������Q�Ƃ��Ă��������B</para>
		/// </summary>
		DT_WORD_ELLIPSIS=0x00040000,

		/// <summary>
		/// Windows 98�AWindows 2000�F�s�� DBCS�i�_�u���o�C�g�����Z�b�g�̕�����j�ŉ��s�����̂�h���܂��B
		/// ���̂��߁A���s�K���� SBCS ������Ɠ����ɂȂ�܂��B���Ƃ��΁A�؍���� Windows �Ŏg�p����ƁA
		/// �A�C�R�����x���̕\���̐M�������オ��܂��BDT_WORDBREAK �t���O���w�肵�Ă��Ȃ���΁A���̒l�͈Ӗ��������܂���B
		/// </summary>
		DT_NOFULLWIDTHCHARBREAK=0x00080000,

		/// <summary>
		/// <para>
		/// Windows 2000�F�e�L�X�g�ɖ��ߍ��܂�Ă���v���t�B�b�N�X�Ƃ��ẴA���p�T���h�i&�j�𖳎����܂��B
		/// ���ɑ��������ɉ������{����Ȃ��Ȃ�܂��B�������A���̑��̃j�[���j�b�N�v���t�B�b�N�X�����́A
		/// �ʏ�ǂ��菈������܂��B���ɗ�������܂��B 
		/// </para>
		/// <list>
		/// <item><description>���͕�����F   "A&bc&&d"</description></item>
		/// <item><description>�ʏ�F         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "Abc&d"</description></item>
		/// </list>
		/// <para>DT_NOPREFIX ����� DT_PREFIXONLY �̐������Q�Ƃ��Ă��������B</para>
		/// </summary>
		DT_HIDEPREFIX=0x00100000,
		/// <summary>
		/// Windows 2000�F�A���p�T���h�v���t�B�b�N�X�����i&�j�̌��̕���������ʒu�̉���������`�悵�܂��B
		/// ��������̂��̑��̕����͈�ؕ`�悵�܂���B���ɗ�������܂��B 
		/// <list>
		/// <item><description>���͕�����F   "A&bc&&d"</description></item>
		/// <item><description>�ʏ�F         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "&nbsp;_&nbsp;&nbsp;&nbsp;"</description></item>
		/// </list>
		/// <para>DT_HIDEPREFIX ����� DT_NOPREFIX �̐������Q�Ƃ��Ă��������B</para>
		/// </summary>
		DT_PREFIXONLY=0x00200000,
	}
	/// <summary>
	/// <para>
	/// GetCharacterPlacement �y�� GetFontLanguageInfo �Ŏg�p����܂��B
	/// �t�H���g�̏���ێ����܂��B
	/// </para>
	/// <para>
	/// �����͈ȉ����
	/// http://msdn.microsoft.com/library/ja/default.asp?url=/library/ja/jpgdi/html/_win32_getcharacterplacement.asp
	/// http://msdn.microsoft.com/library/ja/default.asp?url=/library/ja/jpgdi/html/_win32_getfontlanguageinfo.asp
	/// </para>
	/// </summary>
	public enum GCP{
		/// <summary>
		/// �����Z�b�g�� DBCS �ł��B
		/// </summary>
		GCP_DBCS=0x0001,
		/// <summary>
		/// <para>
		/// ���������ג����܂��BSBCS �łȂ��A�ǂݎ�菇��������E�ł��錾��Ŏg���܂��B
		/// ���̒l���^�����Ă��Ȃ��ꍇ�́A������͂��łɕ\�����ł���Ƃ݂Ȃ���܂��B 
		/// </para><para>
		/// �Z����ɑ΂��Ă��̃t���O���Z�b�g����Ă���AlpClass �z�񂪎g���Ă���ꍇ�A
		/// �ǂݎ�菇�𕶎���̋��E���z���Ďw�肷�邽�߂ɁA�z��̍ŏ��� 2 �̗v�f���g���܂��B
		/// �����̏������Z�b�g����ɂ́AGCP_CLASS_PREBOUNDRTL ����� GCP_CLASS_PREBOUNDLTR ���g�����Ƃ��ł��܂��B
		/// �����̏��������炩���߃Z�b�g���Ă����K�v���Ȃ��ꍇ�A���̒l�� 0 �ɃZ�b�g���܂��B
		/// GCPCLASSIN �t���O�̒l���Z�b�g����Ă���ꍇ�A�����̒l�͑��̒l�Ƒg�ݍ����邱�Ƃ��ł��܂��B
		/// </para><para>
		/// GCP_REORDER �̒l���^�����Ă��Ȃ��ꍇ�A���ꂪ�g���镔���ł̌���̕\�������� lpString �p�����[�^�Ō��܂�A
		/// lpOutString ����� lpOrder �t�B�[���h�͖�������܂��B
		/// </para><para>
		/// ���݂̃t�H���g�����בւ����T�|�[�g���Ă��邩�ǂ����𒲂ׂ�ɂ́AGetFontLanguageInfo �֐����g���܂��B
		/// </para>
		/// </summary>
		GCP_REORDER=0x0002,
		/// <summary>
		/// <para>
		/// ���̔z����쐬����Ƃ��ɁA���̃t�H���g�̃J�[�j���O�΁i���݂���ꍇ�j���g���܂��B
		/// ���݂̃t�H���g���J�[�j���O�΂��T�|�[�g���邩�ǂ����𒲂ׂ�ɂ́AGetFontLanguageInfo �֐����g���܂��B
		/// </para><para>
		/// GetFontLanguageInfo �֐��� GCP_USEKERNING �t���O��Ԃ��܂����A
		/// �����K�� GetCharacterPlacement �֐��ւ̌Ăяo���Ŏg��Ȃ���΂Ȃ�Ȃ��킯�ł͂Ȃ��A
		/// �P�ɗ��p�\�ł���Ƃ������Ƃɒ��ӂ��ĉ������B
		/// �啔���� True Type �t�H���g�ɂ̓J�[�j���O�e�[�u��������܂����A�g���K�v�͂���܂���B
		/// </para>
		/// </summary>
		GCP_USEKERNING=0x0008,
		/// <summary>
		/// ��������̈ꕔ�܂��͑S���̕����́A���݂̃R�[�h�y�[�W�ł́A
		/// �I�𒆂̃t�H���g�Œ�`����Ă���W���̎��`�łȂ����`���g���ĕ\������܂��B
		/// �A���r�A��ȂǁA����ɂ���ẮA���̒l���^�����Ȃ���΁A�O���t���쐬���邱�Ƃ��ł��܂���B
		/// ��ʋK���Ƃ��āAGetFontLanguageInfo �֐������镶����ɑ΂��Ă��̒l��Ԃ����ꍇ�A
		/// ���̒l�� GetCharacterPlacement �֐��Ŏg��Ȃ���΂Ȃ�܂���B
		/// </summary>
		GCP_GLYPHSHAPE=0x0010,
		/// <summary>
		/// <para>
		/// �������A�����镔���͂��ׂč������g���܂��B
		/// ������ 1 �̃O���t�� 2 �ȏ�̕����Ŏg���Ă���ꍇ�ɔh�����܂��B
		/// ���Ƃ��΁Aa �� e �̍����� &#xE6; �ł��B�������A������g���ɂ́A�K�v�ȃO���t�����̌��ꂪ�T�|�[�g���A
		/// ���ɂ��̃t�H���g�͕K���T�|�[�g���Ă��Ȃ���΂Ȃ�܂���
		/// �i���Ƃ��΁A�����ŋ�������́A�p��̊���ł͏�������܂���j�B
		/// </para><para>
		/// GetFontLanguageInfo �֐����g���A���݂̃t�H���g���������T�|�[�g���邩�ǂ����𒲂ׂ܂��B
		/// �t�H���g���������T�|�[�g���A�����ɂȂ镶�����ŗL�̍ő�l���K�v�ȏꍇ�A
		/// lpGlyphs �z��̍ŏ��̗v�f�̒l��ݒ肵�܂��B�ʏ�̍������K�v�ȏꍇ�A���̒l�� 0 �ɃZ�b�g���܂��B
		/// GCP_LIGATE ���^�����Ă��Ȃ��ꍇ�A�����͍s���܂���B�ڍׂɂ��Ă� GCP_RESULTS �\���̂��Q�Ƃ��Ă��������B
		/// </para><para>
		/// ���̕����̃Z�b�g�ɑ΂��Ēʏ� GCP_REORDER �̒l���K�v�ł���̂ɁA���ꂪ�^�����Ă��Ȃ��ꍇ�A
		/// �n����镶���񂪂��łɕ\�����ɕ��ׂ��Ă��Ȃ��ꍇ�A�o�͖͂��Ӗ��ɂȂ�܂�
		/// �܂�AGetCharacterPlacement �֐��ւ̂P��ڂ̌Ăяo���� lpGcpResults �p�����[�^����
		/// lpOutString �p�����[�^�֊i�[���ꂽ���ʂ��A���̌Ăяo���̓��͕�����ɂȂ�܂��j�B
		/// </para><para>
		/// GetFontLanguageInfo �֐��� GCP_LIGATE �t���O��Ԃ��܂����A
		/// �����K�� GetCharacterPlacement �֐��ւ̌Ăяo���Ŏg��Ȃ���΂Ȃ�Ȃ��킯�ł͂Ȃ��A
		/// �P�ɗ��p�\�ł���Ƃ������Ƃɒ��ӂ��ĉ������B
		/// </para>
		/// </summary>
		GCP_LIGATE=0x0020,

		/// <summary>
		/// �g�p���Ȃ��ŉ������B
		/// </summary>
		[System.Obsolete]
		GCP_GLYPHINDEXING=0x0080,

		/// <summary>
		/// ��������̕������̎�舵�����@�����肵�܂��B���̒l���Z�b�g����Ă��Ȃ��ꍇ�A
		/// �������͕��� 0 �̕�����Ƃ��Ĉ����܂��B���Ƃ��΁A�w�u���C��̕�����ɕ��������܂܂�Ă��Ă��A
		/// �����\���������Ȃ��ꍇ�Ȃǂ��Y�����܂��B 
		/// <para>
		/// �t�H���g�����������T�|�[�g���邩�ǂ����𒲂ׂ�ɂ́AGetFontLanguageInfo �֐����g���܂��B
		/// �T�|�[�g����ꍇ�AGetCharacterPlacement �֐����Ăяo���ہA
		/// �A�v���P�[�V�����ł̕K�v���ɉ����� GCP_DIACRITIC �t���O���g�����Ƃ��A�܂��g��Ȃ����Ƃ��ł��܂��B
		/// </para>
		/// </summary>
		GCP_DIACRITIC=0x0100,
		/// <summary>
		/// <para>
		/// ������̒����𒲐߂��� nMaxExtent �p�����[�^�ŗ^����ꂽ�l�Ɠ����ɂ��邽�߁A
		/// �͈͂𒲐߂������ɁA�܂��͔͈͂𒲐߂���Ƌ��ɁA�J�V�_���g���܂��B
		/// lpDx �z����ł́A�J�V�_�͕��̈ʒu���킹�C���f�b�N�X�Ƃ��ĕ\����܂��B
		/// GCP_KASHIDA �́A���̃t�H���g�i����т��̌���j���J�V�_���T�|�[�g����ꍇ�Ɏg���A
		/// �K�� GCP_JUSTIFY �Ƌ��Ɏg���܂��B
		/// ���݂̃t�H���g���J�V�_���T�|�[�g���邩�ǂ����́AGetFontLanguageInfo �֐����g���Ē��ׂ܂��B 
		/// </para><para>
		/// ������̈ʒu���킹�ɃJ�V�_���g���ƁA������ɕK�v�ȃO���t�����A
		/// ���͕�������̕��������������Ȃ�ꍇ������܂��B���̂��߃J�V�_���g���ƁA
		/// ���͕�����ɑ΂��č쐬�����z��̃T�C�Y���\�����ǂ������A�v���P�[�V���������ׂ邱�Ƃ͂ł��܂���
		/// �\�z�����ő�l�͂����ނ� dxPageWidth/dxAveCharWidth �ɂȂ�܂��B
		/// �����ŁAdxPageWidth �͕����̕��AdxAveCharWidth �� GetTextMetrics �֐����Ăяo���Ė߂��ꂽ���ϕ������ł��j�B
		/// </para><para>
		/// GetFontLanguageInfo �֐��� GCP_KASHIDA �t���O��Ԃ��܂����A
		/// �����K�� GetCharacterPlacement �֐��ւ̌Ăяo���Ŏg��Ȃ���΂Ȃ�Ȃ��킯�ł͂Ȃ��A
		/// �P�ɗ��p�\�ł���Ƃ������Ƃɒ��ӂ��ĉ������B
		/// </para>
		/// </summary>
		GCP_KASHIDA=0x0400,
		/// <summary>
		/// GetFontLanguageInfo �̌Ăяo���ŃG���[�����������ꍇ�AGCP_ERROR ���Ԃ�܂��B
		/// </summary>
		GCP_ERROR=0x8000,
		/// <summary>
		/// FLI_MASK �Ń}�X�N����Ă���ꍇ�A�߂�l�𒼐� GetCharacterPlacement �֐��ɓn�����Ƃ��ł��܂��B
		/// </summary>
		FLI_MASK=0x103B,

		/// <summary>
		/// ������̒����� nMaxExtent �p�����[�^�Ɠ����ɂȂ�悤�ɁAlpDx �z����͈̔͂𒲐߂��܂��B
		/// GCP_JUSTIFY �t���O�� GCP_MAXEXTENT �t���O�ƈꏏ�̏ꍇ�ɂ̂ݎg���܂��B
		/// </summary>
		GCP_JUSTIFY=0x00010000,

		/// <summary>
		/// GCP_NODIACRITICS �̒l�͒�`����O����Ă��邽�߁A�g��Ȃ��ł�������
		/// </summary>
		[System.Obsolete]
		GCP_NODIACRITICS=0x00020000,

		/// <summary>
		/// �t�H���g�ɂ͒ʏ킻�̃R�[�h�y�[�W���g���ăA�N�Z�X�ł��Ȃ��A�]���ȃO���t���܂܂�Ă��܂��B
		/// ���̃O���t�ɃA�N�Z�X����ɂ́AGetCharacterPlacement �֐����g���܂��B
		/// ���̒l�͏���񋟂��邾���ŁAGetCharacterPlacement �֐��ɓn�����߂Ɏg�����Ƃ͂ł��܂���B
		/// </summary>
		FLI_GLYPHS=0x00040000,

		/// <summary>
		/// lpClass �z��ɁA�\�ߐݒ肳�ꂽ�����̕��ނ������Ă��邱�Ƃ�\���܂��B
		/// ���ނ͏o�͂Ɠ����ɂȂ�܂��B���镶���ɑ΂������̕��ނ����m�̏ꍇ�A
		/// ����ɑΉ�����z����̈ʒu�� 0 �ɐݒ肳��܂��B
		/// ���ނ̏ڍׂɂ��ẮAGCP_RESULTS �\���̂��Q�Ƃ��Ă��������B
		/// ����́AGetFontLanguageInfo �֐��� GCP_REORDER �t���O��Ԃ����ꍇ�ɂ����g�p�\�ł��B
		/// </summary>
		GCP_CLASSIN=0x00080000,
		/// <summary>
		/// ������͈̔͂� nMaxExtent �p�����[�^�ŗ^������_���P�ʂł̒l�𒴂��Ȃ��ꍇ�Ɍ���A������v�Z���܂��B
		/// </summary>
		GCP_MAXEXTENT=0x00100000,
		/// <summary>
		/// ?
		/// </summary>
		GCP_JUSTIFYIN=0x00200000,
		/// <summary>
		/// �P����̕����̈ʒu�ɂ���āA�O���t���قȂ�����A������ύX�����肷��K�v�����錾��ł́A
		/// ��\���������R�[�h�y�[�W���ɕ\�������ꍇ������܂��B���Ƃ��΃w�u���C��̃R�[�h�y�[�W�ł́A
		/// �o�͕�����̍Ō�̈ʒu�����肷�邽�߂ɁA������E�}�[�J�[����щE���獶�}�[�J�[�����݂��܂��B
		/// �ʏ킱���̃}�[�J�[�͕\�����ꂸ�AlpGlyphs �z�񂨂�� lpDx �z�񂩂珜������܂��B
		/// �����̕�����\������ɂ́AGCP_DISPLAYZWG �t���O���g���܂��B
		/// </summary>
		GCP_DISPLAYZWG=0x00400000,
		/// <summary>
		/// �Z����ł����g���܂��B�����\�ȕ����̓��Z�b�g����Ȃ����Ƃ�\���܂��B
		/// ���Ƃ��΁A�E���獶�̕�����ł́A'�i'and'�j' �͋t�ɂȂ�܂���B
		/// </summary>
		GCP_SYMSWAPOFF=0x00800000,
		/// <summary>
		/// ����̌���ł����g���܂��B�ʏ�̐����̏����𖳌��ɂ��A
		/// ������̓ǂݎ�菇�ɍ��v���鋭�������Ƃ��ď������܂��BGCP_REORDER �t���O�ƈꏏ�̏ꍇ�ɂ����L�p�ł��B
		/// </summary>
		GCP_NUMERICOVERRIDE=0x01000000,
		/// <summary>
		/// ����̌���ł����g���܂��B�ʏ�̒��ԕ�����̏����𖳌��ɂ��A
		/// ������̓ǂݎ�菇�ɍ��v���鋭�������Ƃ��ď������܂��BGCP_REORDER �t���O�Ƌ��Ɏg����ꍇ�ɂ����L�p�ł��B
		/// </summary>
		GCP_NEUTRALOVERRIDE=0x02000000,
		/// <summary>
		/// �A���r�A��ƃ^�C��ł����g���܂��B�����ɑ΂��Ă͕W���̃��e���O���t���g���A
		/// �V�X�e���̊���𖳌��ɂ��܂��B���̌���̂��̃t�H���g�ł��̃I�v�V���������p�\���ǂ����𒲂ׂ�ɂ́A
		/// GetStringTypeEx �֐����g���āA���̌��ꂪ�����̐����������T�|�[�g���邩�ǂ����𒲂ׂ܂��B
		/// </summary>
		GCP_NUMERICSLATIN=0x04000000,
		/// <summary>
		/// �A���r�A��ƃ^�C��ł����g���܂��B�����ɑ΂��ă��[�J���O���t���g���A�V�X�e���̊���𖳌��ɂ��܂��B
		/// ���̌���̂��̃t�H���g�ł��̃I�v�V���������p�\���ǂ����𒲂ׂ�ɂ́AGetStringTypeEx �֐����g���āA
		/// ���̌��ꂪ�����̐����������T�|�[�g���邩�ǂ����𒲂ׂ܂��B
		/// </summary>
		GCP_NUMERICSLOCAL=0x08000000,
	}
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public unsafe struct GCP_RESULTS{
		public int		lStructSize;
		[Interop::MarshalAs(Interop::UnmanagedType.LPTStr)]
		public string	lpOutString;
		public uint*	lpOrder;
		public int*	lpDx;
		public int*	lpCaretPos;
		[Interop::MarshalAs(Interop::UnmanagedType.LPStr)]
		public string	lpClass;
		[Interop::MarshalAs(Interop::UnmanagedType.LPWStr)]
		public string	lpGlyphs;
		public uint	nGlyphs;
		public int	nMaxFit;
	}
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	internal struct RASTERIZER_STATUS{
		public short nSize;
		public RASTERIZER_STATUS_Flags wFlags;
		public short nLanguageID;
	}
	[System.Flags]
	internal enum RASTERIZER_STATUS_Flags:short{
		TT_Available=1,
		TT_Enabled=2
	}
	#endregion

}
namespace afh.Win32{
	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct SIZE16{
		public short cx;
		public short cy;
	}

	[Interop::StructLayout(Interop::LayoutKind.Sequential)]
	public struct RECT{
		public int left;
		public int top;
		public int right;
		public int bottom;

		public RECT(int left,int top,int right,int bottom){
			this.left=left;
			this.top=top;
			this.right=right;
			this.bottom=bottom;
		}

		public static implicit operator Gdi::Rectangle(RECT rc){
			return Gdi::Rectangle.FromLTRB(rc.left,rc.top,rc.right,rc.bottom);
		}
		public static implicit operator RECT(Gdi::Rectangle rect){
			return new RECT(rect.Left,rect.Top,rect.Right,rect.Bottom);
		}
	}
}