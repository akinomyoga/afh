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
		/// 作成元の Graphics を指定します。
		/// 元の Graphics を操作する前に必ずこのインスタンスの Dispose を呼び出して下さい。
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
				throw new System.Exception("現在位置の移動に失敗しました。");
		}
		[Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool LineTo(HDC hdc,int nXEnd,int nYEnd);

		public void MoveTo(int x,int y){
			if(!MoveToEx(this.hdc,x,y,System.IntPtr.Zero))
				throw new System.Exception("現在位置の移動に失敗しました。");
		}
		[Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool MoveToEx(HDC hdc,int X,int Y,[Interop::Out]System.IntPtr lpPoint);

		//===========================================================
		//		LineDDA
		//===========================================================
		/// <summary>
		/// 指定した点と点を結ぶ直線を構成する点を列挙します。
		/// それぞれの点に対して処理を行う関数を指定します。
		/// </summary>
		/// <typeparam name="T">関数に渡すアプリケーション定義のオブジェクトの型を指定します。</typeparam>
		/// <param name="nXStart">開始点の x 座標を指定します。</param>
		/// <param name="nYStart">開始点の y 座標を指定します。</param>
		/// <param name="nXEnd">終端点の x 座標を指定します。</param>
		/// <param name="nYEnd">終端点の y 座標を指定します。</param>
		/// <param name="lpLineFunc">処理を行う関数を指定します。</param>
		/// <param name="data">関数に渡すアプリケーション定義のオブジェクトを指定します。</param>
		/// <returns>成功した場合に true を返します。それ以外の場合には false を返します。</returns>
		public static bool LineDDA<T>(int nXStart,int nYStart,int nXEnd,int nYEnd,LineDDAProc<T> lpLineFunc,T data){
			return internalLineDDA(nXStart,nYStart,nXEnd,nYEnd,
				delegate(int x,int y,System.IntPtr p){lpLineFunc(x,y,data);},
				System.IntPtr.Zero);
		}
		/// <summary>
		/// 指定した点と点を結ぶ直線を構成する点を列挙します。
		/// それぞれの点に対して処理を行う関数を指定します。
		/// </summary>
		/// <param name="nXStart">開始点の x 座標を指定します。</param>
		/// <param name="nYStart">開始点の y 座標を指定します。</param>
		/// <param name="nXEnd">終端点の x 座標を指定します。</param>
		/// <param name="nYEnd">終端点の y 座標を指定します。</param>
		/// <param name="lpLineFunc">処理を行う関数を指定します。</param>
		/// <returns>成功した場合に true を返します。それ以外の場合には false を返します。</returns>
		public static bool LineDDA(int nXStart,int nYStart,int nXEnd,int nYEnd,LineDDAProc lpLineFunc){
			return internalLineDDA(nXStart,nYStart,nXEnd,nYEnd,
				delegate(int x,int y,System.IntPtr p){lpLineFunc(x,y);},
				System.IntPtr.Zero);
		}
		[Interop::DllImport("gdi32.dll",EntryPoint="LineDDA")]
		private static extern bool internalLineDDA(int nXStart,int nYStart,int nXEnd,int nYEnd,internalLineDDAProc lpLineFunc,System.IntPtr lpData);
		private delegate void internalLineDDAProc(int X,int Y,System.IntPtr lpData);

		#region 文字列/フォント
		/// <summary>
		/// 文字列を描画します。
		/// </summary>
		/// <param name="text">描画する文字列を指定します。</param>
		/// <param name="rect">文字列を描画する対象の矩形を指定します。</param>
		/// <param name="flags">文字列を描画する際の詳細な指定を行います。</param>
		public int DrawString(string text,Gdi::Rectangle rect,StringFormat flags){
			RECT rc=rect;
			int ret=DrawText(this.hdc,text,text.Length,ref rc,flags);
			if(ret==0)throw new System.Exception("文字列の描画に失敗しました。");
			return ret;
		}
		[Interop::DllImport("gdi32.dll",CharSet=Interop::CharSet.Auto)]
		private static extern int DrawText(HDC hdc,string lpString,int nCount,ref RECT lpRect,StringFormat uFormat);
		/// <summary>
		/// 文字列を描画します。
		/// </summary>
		/// <param name="text">描画する文字列を指定します。</param>
		/// <param name="x">文字列を描画する位置の x 座標を指定します。</param>
		/// <param name="y">文字列を描画する位置の y 座標を指定します。</param>
		public void DrawString(string text,int x,int y){
			if(!TextOut(this.hdc,x,y,text,text.Length)){
				throw new System.Exception("文字列の描画に失敗しました。");
			}
		}
		[Interop::DllImport("gdi32.dll",CharSet=Interop::CharSet.Auto)]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool TextOut(HDC hdc,int nXStart,int nYStart,string lpString,int cbString);

		/// <summary>
		/// 描画に使用するフォントを設定します。
		/// </summary>
		public Gdi::Font Font{
			set{
				if(SelectObject(this.hdc,value.ToHfont())==System.IntPtr.Zero)
					throw new System.Exception("フォントの設定に失敗しました。");
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
		/// 文字色を取得亦は設定します。
		/// </summary>
		public Color TextColor{
			get{return GetTextColor(this.hdc);}
			set{
				if(0xffffffff==(uint)SetTextColor(hdc,value))
					throw new System.Exception("文字色の設定に失敗しました。");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef SetTextColor(HDC hdc,ColorRef color);
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef GetTextColor(HDC hdc);

		/// <summary>
		/// 背景色を取得亦は設定します。
		/// </summary>
		public Color BackColor{
			get{return GetBkColor(this.hdc);}
			set{
				if(0xffffffff==(uint)SetBkColor(hdc,value))
					throw new System.Exception("背景色の設定に失敗しました。");
			}
		}
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef SetBkColor(HDC hdc,ColorRef color);
		[Interop::DllImport("gdi32.dll")]
		private static extern ColorRef GetBkColor(HDC hdc);

		/// <summary>
		/// 背景モードの取得亦は設定を行います。
		/// true に設定されている時には、背景色を描画します。
		/// false に設定されている時には背景を描画せず、透過します。
		/// </summary>
		public bool Background{
			get{
				switch(GetBkMode(this.hdc)){
					case BkMode.Opaque:
						return true;
					case BkMode.Transparent:
						return false;
					case BkMode.Failed:
						throw new System.Exception("背景モードの取得に失敗しました。");
					default:
						throw new System.Exception("背景モードは未知の値です。");
				}
			}
			set{
				if(!SetBkMode(this.hdc,value?BkMode.Opaque:BkMode.Transparent))
					throw new System.Exception("背景モードの変更に失敗しました。");
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
		/// 文字間隔を取得亦は設定します。
		/// 描画の際にこの値がそれぞれの文字の間隔に加算されます。
		/// </summary>
		public int LetterSpacing{
			get{
				int ret=GetCharacterExtra(this.hdc);
				if(ret==unchecked((int)0x80000000))
					throw new System.Exception("文字間隔の取得に失敗しました。");
				return ret;
			}
			set{
				if(0x80000000==SetCharacterExtra(this.hdc,value))
					throw new System.Exception("文字間隔の設定に失敗しました。");
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
		/// 使っているシステムに、使用可能な TrueType フォントが存在するかどうかを取得します。
		/// </summary>
		public static bool TrueTypeAvailable{
			get{
				if(rs.nSize==0)GetRasterizerStatus();
				return (rs.wFlags&RASTERIZER_STATUS_Flags.TT_Available)!=0;
			}
		}
		/// <summary>
		/// 使っているシステムで TrueType フォントを扱う事が出来るかどうかを取得します。
		/// </summary>
		public static bool TrueTypeEnabled{
			get{
				if(rs.nSize==0)GetRasterizerStatus();
				return (rs.wFlags&RASTERIZER_STATUS_Flags.TT_Enabled)!=0;
			}
		}
		/// <summary>
		/// 使用しているシステムの言語 ID を取得します。
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
				throw new System.Exception("ラスタライザの情報の取得に失敗しました。");
		}
		[method:Interop::DllImport("gdi32.dll")]
		[return:Interop::MarshalAs(Interop::UnmanagedType.Bool)]
		private static extern bool GetRasterizerCaps(ref RASTERIZER_STATUS lprs,uint cb);

		//===========================================================
		//		GetCharacterPlacement
		//===========================================================
		//
		//	何に使うのか良く分からない関数。後で詳しく MSDN を見る。
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

	#region 文字列/フォント
	/// <summary>
	/// 文字列の描画の仕方を指定するのに使用します。
	/// </summary>
	public enum StringFormat:uint{
		/// <summary>
		/// 長方形領域の上端にテキストを揃えます（単一行のときのみ）。
		/// </summary>
		DT_TOP=0x00000000,
		/// <summary>
		/// テキストを左揃えにします。
		/// </summary>
		DT_LEFT=0x00000000,
		/// <summary>
		/// テキストを長方形領域内で横方向に中央揃えで表示します。
		/// </summary>
		DT_CENTER=0x00000001,
		/// <summary>
		/// テキストを右揃えにします。
		/// </summary>
		DT_RIGHT=0x00000002,
		/// <summary>
		/// テキストを縦方向に中央揃えで表示します。この値は、DT_SINGLELINE を指定した場合にしか使用できません。
		/// </summary>
		DT_VCENTER=0x00000004,
		/// <summary>
		/// 長方形領域の下端にテキストを揃えます。DT_SINGLELINE と同時に指定しなければなりません。
		/// </summary>
		DT_BOTTOM=0x00000008,
		/// <summary>
		/// テキストを複数行で表示します。折り返しは、lpRect パラメータで指定した長方形領域の端から単語がはみ出す部分で自動的に行われます。
		/// キャリッジリターンとラインフィードの組み合わせによっても折り返されます。
		/// </summary>
		DT_WORDBREAK=0x00000010,
		/// <summary>
		/// テキストを単一行で表示します。たとえ、テキストがキャリッジリターンやラインフィードを含んでいても、改行されません。
		/// </summary>
		DT_SINGLELINE=0x00000020,
		/// <summary>
		/// タブ文字を展開します。既定のタブ間隔は 8 文字です。
		/// このフラグをセットする場合、DT_WORD_ELLIPSIS、DT_PATH_ELLIPSIS、および DT_END_ELLIPSIS は指定できません。
		/// </summary>
		DT_EXPANDTABS=0x00000040,
		/// <summary>
		/// タブ間隔を設定します。この値を指定したときは、uFormat パラメータの 15 ビットから 8 ビット（下位ワードの上位バイト）で、
		/// タブ間隔の文字数を指定します。既定のタブ間隔は 8 文字です。この値を指定した場合、
		/// DT_CALCRECT、DT_EXTERNALLEADING、DT_INTERNAL、DT_NOCLIP、および DT_NOPREFIX は指定できません。
		/// </summary>
		DT_TABSTOP=0x00000080,
		/// <summary>
		/// クリッピングをしません。描画が多少速くなります。
		/// </summary>
		DT_NOCLIP=0x00000100,
		/// <summary>
		/// 行の高さに、外部レディングの高さ（テキストの行間として適切な高さ）を加算します。通常、外部レディングはテキスト行の高さに加えられません。
		/// </summary>
		DT_EXTERNALLEADING=0x00000200,
		/// <summary>
		/// 指定されたテキストを表示するために必要な長方形領域の幅と高さを調べます。
		/// 複数行テキストの場合は、lpRect パラメータで指定された長方形領域の幅を使い、
		/// 長方形領域の下端をテキストの最終行の下側の境界線にまで広げます。
		/// テキストを 1 行で表示する場合は、長方形領域の右端を行の最後の文字の右側の境界線に合うように変更します。
		/// どちらの場合も、DrawText 関数は、テキストの描画は行わず、整形されたテキストの高さを返します。
		/// </summary>
		DT_CALCRECT=0x00000400,
		/// <summary>
		/// <para>
		/// プリフィックス文字の処理を行わないようにします。通常は、ニーモニックプレフィクス文字の「&」は、
		/// その次にくる文字に下線（_）を付けて表示せよとの命令であると解釈され、
		/// ニーモニックプレフィックス文字の「&&」は、1 つの「&」を表示せよとの命令であると解釈されます。
		/// DT_NOPREFIX を指定すると、この処理が行われなくなります。次に例を示します。 
		/// </para>
		/// <list>
		/// <item><description>入力文字列：   "A&bc&&d"</description></item>
		/// <item><description>通常：         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "A&bc&&d"</description></item>
		/// </list>
		/// <para>DT_HIDEPREFIX および DT_PREFIXONLY の説明も参照してください。</para>
		/// </summary>
		DT_NOPREFIX=0x00000800,
		/// <summary>
		/// テキストの表示サイズを計算する際にシステムフォントを使用します。
		/// </summary>
		DT_INTERNAL=0x00001000,

		/// <summary>
		/// 複数行エディットコントロールがもつ特性と同じ特性で描画します。
		/// 特に、平均文字幅がエディットコントロールと同じ方法で計算され、部分的に見えている最後の行は表示されません。
		/// </summary>
		DT_EDITCONTROL=0x00002000,
		/// <summary>
		/// <para>
		/// 指定した長方形領域に収まるように、必要に応じてテキストの途中を省略符号 (...) に置き換えます。
		/// 円記号（\）が含まれているテキストの場合、最後の円記号の後ろのテキストが可能な限り保持されます。 
		/// </para>
		/// <para>DT_MODIFYSTRING フラグを指定していない限り、文字列が変更されることはありません。</para>
		/// <para>DT_END_ELLIPSIS および DT_WORD_ELLIPSIS の説明も参照してください。</para>
		/// </summary>
		DT_PATH_ELLIPSIS=0x00004000,
		/// <summary>
		/// <para>
		/// 文字列の最後の部分が長方形領域に納まり切らない場合、はみ出す部分が切り取られ、
		/// 末尾に省略符号（...）が追加されます。
		/// 文字列の最後ではない場所にある単語が長方形領域からはみ出す場合は、省略記号なしで切り取られます。
		/// </para><para>
		/// DT_MODIFYSTRING フラグがセットされていない限り、文字列が変更されることはありません。
		/// </para><para>
		/// DT_PATH_ELLIPSIS および DT_WORD_ELLIPSIS の説明も参照してください。
		/// </para>
		/// </summary>
		DT_END_ELLIPSIS=0x00008000,
		/// <summary>
		/// lpString パラメータが指すバッファに、実際に表示された文字列を格納します。
		/// DT_END_ELLIPSIS フラグまたは DT_PATH_ELLIPSIS フラグを指定したときにしか意味を持ちません。
		/// </summary>
		DT_MODIFYSTRING=0x00010000,
		/// <summary>
		/// hdc パラメータで指定したデバイスコンテキストで選択されているフォントがヘブライ語かアラビア語だった場合に、
		/// 双方向テキストを右から左への読み取り順序で表示します。既定の読み取り順序は、どのテキストでも左から右です。
		/// </summary>
		DT_RTLREADING=0x00020000,
		/// <summary>
		/// <para>長方形領域内に納まらない部分がある場合、それを切り取ったうえで、必ず省略記号（...）を追加します。</para>
		/// <para>DT_END_ELLIPSIS および DT_PATH_ELLIPSIS の説明も参照してください。</para>
		/// </summary>
		DT_WORD_ELLIPSIS=0x00040000,

		/// <summary>
		/// Windows 98、Windows 2000：行が DBCS（ダブルバイト文字セットの文字列）で改行されるのを防ぎます。
		/// このため、改行規則は SBCS 文字列と同じになります。たとえば、韓国語版 Windows で使用すると、
		/// アイコンラベルの表示の信頼性が上がります。DT_WORDBREAK フラグを指定していなければ、この値は意味を持ちません。
		/// </summary>
		DT_NOFULLWIDTHCHARBREAK=0x00080000,

		/// <summary>
		/// <para>
		/// Windows 2000：テキストに埋め込まれているプレフィックスとしてのアンパサンド（&）を無視します。
		/// 後ろに続く文字に下線が施されなくなります。ただし、その他のニーモニックプレフィックス文字は、
		/// 通常どおり処理されます。次に例を示します。 
		/// </para>
		/// <list>
		/// <item><description>入力文字列：   "A&bc&&d"</description></item>
		/// <item><description>通常：         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "Abc&d"</description></item>
		/// </list>
		/// <para>DT_NOPREFIX および DT_PREFIXONLY の説明も参照してください。</para>
		/// </summary>
		DT_HIDEPREFIX=0x00100000,
		/// <summary>
		/// Windows 2000：アンパサンドプレフィックス文字（&）の後ろの文字がくる位置の下線だけを描画します。
		/// 文字列内のその他の文字は一切描画しません。次に例を示します。 
		/// <list>
		/// <item><description>入力文字列：   "A&bc&&d"</description></item>
		/// <item><description>通常：         "A<u>b</u>c&d"</description></item>
		/// <item><description>DT_NOPREFIX:    "&nbsp;_&nbsp;&nbsp;&nbsp;"</description></item>
		/// </list>
		/// <para>DT_HIDEPREFIX および DT_NOPREFIX の説明も参照してください。</para>
		/// </summary>
		DT_PREFIXONLY=0x00200000,
	}
	/// <summary>
	/// <para>
	/// GetCharacterPlacement 及び GetFontLanguageInfo で使用されます。
	/// フォントの情報を保持します。
	/// </para>
	/// <para>
	/// 説明は以下より
	/// http://msdn.microsoft.com/library/ja/default.asp?url=/library/ja/jpgdi/html/_win32_getcharacterplacement.asp
	/// http://msdn.microsoft.com/library/ja/default.asp?url=/library/ja/jpgdi/html/_win32_getfontlanguageinfo.asp
	/// </para>
	/// </summary>
	public enum GCP{
		/// <summary>
		/// 文字セットは DBCS です。
		/// </summary>
		GCP_DBCS=0x0001,
		/// <summary>
		/// <para>
		/// 文字列を並べ直します。SBCS でなく、読み取り順が左から右である言語で使います。
		/// この値が与えられていない場合は、文字列はすでに表示順であるとみなされます。 
		/// </para><para>
		/// セム語に対してこのフラグがセットされており、lpClass 配列が使われている場合、
		/// 読み取り順を文字列の境界を越えて指定するために、配列の最初の 2 つの要素が使われます。
		/// 文字の順序をセットするには、GCP_CLASS_PREBOUNDRTL および GCP_CLASS_PREBOUNDLTR を使うことができます。
		/// 文字の順序をあらかじめセットしておく必要がない場合、この値を 0 にセットします。
		/// GCPCLASSIN フラグの値がセットされている場合、これらの値は他の値と組み合せることができます。
		/// </para><para>
		/// GCP_REORDER の値が与えられていない場合、これが使われる部分での言語の表示順序は lpString パラメータで決まり、
		/// lpOutString および lpOrder フィールドは無視されます。
		/// </para><para>
		/// 現在のフォントが並べ替えをサポートしているかどうかを調べるには、GetFontLanguageInfo 関数を使います。
		/// </para>
		/// </summary>
		GCP_REORDER=0x0002,
		/// <summary>
		/// <para>
		/// 幅の配列を作成するときに、そのフォントのカーニング対（存在する場合）を使います。
		/// 現在のフォントがカーニング対をサポートするかどうかを調べるには、GetFontLanguageInfo 関数を使います。
		/// </para><para>
		/// GetFontLanguageInfo 関数は GCP_USEKERNING フラグを返しますが、
		/// それを必ず GetCharacterPlacement 関数への呼び出しで使わなければならないわけではなく、
		/// 単に利用可能であるということに注意して下さい。
		/// 大部分の True Type フォントにはカーニングテーブルがありますが、使う必要はありません。
		/// </para>
		/// </summary>
		GCP_USEKERNING=0x0008,
		/// <summary>
		/// 文字列内の一部または全部の文字は、現在のコードページでの、
		/// 選択中のフォントで定義されている標準の字形でない字形を使って表示されます。
		/// アラビア語など、言語によっては、この値が与えられなければ、グリフを作成することができません。
		/// 一般規則として、GetFontLanguageInfo 関数がある文字列に対してこの値を返した場合、
		/// その値を GetCharacterPlacement 関数で使わなければなりません。
		/// </summary>
		GCP_GLYPHSHAPE=0x0010,
		/// <summary>
		/// <para>
		/// 文字が連結する部分はすべて合字を使います。
		/// 合字は 1 つのグリフが 2 つ以上の文字で使われている場合に派生します。
		/// たとえば、a と e の合字は &#xE6; です。しかし、これを使うには、必要なグリフをその言語がサポートし、
		/// 特にそのフォントは必ずサポートしていなければなりません
		/// （たとえば、ここで挙げた例は、英語の既定では処理されません）。
		/// </para><para>
		/// GetFontLanguageInfo 関数を使い、現在のフォントが合字をサポートするかどうかを調べます。
		/// フォントが合字をサポートし、合字になる文字数固有の最大値が必要な場合、
		/// lpGlyphs 配列の最初の要素の値を設定します。通常の合字が必要な場合、この値を 0 にセットします。
		/// GCP_LIGATE が与えられていない場合、合字は行われません。詳細については GCP_RESULTS 構造体を参照してください。
		/// </para><para>
		/// その文字のセットに対して通常 GCP_REORDER の値が必要であるのに、それが与えられていない場合、
		/// 渡される文字列がすでに表示順に並べられていない場合、出力は無意味になります
		/// つまり、GetCharacterPlacement 関数への１回目の呼び出しで lpGcpResults パラメータから
		/// lpOutString パラメータへ格納された結果が、次の呼び出しの入力文字列になります）。
		/// </para><para>
		/// GetFontLanguageInfo 関数は GCP_LIGATE フラグを返しますが、
		/// それを必ず GetCharacterPlacement 関数への呼び出しで使わなければならないわけではなく、
		/// 単に利用可能であるということに注意して下さい。
		/// </para>
		/// </summary>
		GCP_LIGATE=0x0020,

		/// <summary>
		/// 使用しないで下さい。
		/// </summary>
		[System.Obsolete]
		GCP_GLYPHINDEXING=0x0080,

		/// <summary>
		/// 文字列内の分音符の取り扱い方法を決定します。この値がセットされていない場合、
		/// 分音符は幅が 0 の文字列として扱われます。たとえば、ヘブライ語の文字列に分音符が含まれていても、
		/// それを表示したくない場合などが該当します。 
		/// <para>
		/// フォントが分音符をサポートするかどうかを調べるには、GetFontLanguageInfo 関数を使います。
		/// サポートする場合、GetCharacterPlacement 関数を呼び出す際、
		/// アプリケーションでの必要性に応じて GCP_DIACRITIC フラグを使うことも、また使わないこともできます。
		/// </para>
		/// </summary>
		GCP_DIACRITIC=0x0100,
		/// <summary>
		/// <para>
		/// 文字列の長さを調節して nMaxExtent パラメータで与えられた値と同じにするため、
		/// 範囲を調節する代わりに、または範囲を調節すると共に、カシダを使います。
		/// lpDx 配列内では、カシダは負の位置合わせインデックスとして表されます。
		/// GCP_KASHIDA は、そのフォント（およびその言語）がカシダをサポートする場合に使われ、
		/// 必ず GCP_JUSTIFY と共に使われます。
		/// 現在のフォントがカシダをサポートするかどうかは、GetFontLanguageInfo 関数を使って調べます。 
		/// </para><para>
		/// 文字列の位置合わせにカシダを使うと、文字列に必要なグリフ数が、
		/// 入力文字列内の文字数よりも多くなる場合があります。このためカシダを使うと、
		/// 入力文字列に対して作成した配列のサイズが十分かどうかをアプリケーションが調べることはできません
		/// 予想される最大値はおおむね dxPageWidth/dxAveCharWidth になります。
		/// ここで、dxPageWidth は文書の幅、dxAveCharWidth は GetTextMetrics 関数を呼び出して戻された平均文字幅です）。
		/// </para><para>
		/// GetFontLanguageInfo 関数は GCP_KASHIDA フラグを返しますが、
		/// それを必ず GetCharacterPlacement 関数への呼び出しで使わなければならないわけではなく、
		/// 単に利用可能であるということに注意して下さい。
		/// </para>
		/// </summary>
		GCP_KASHIDA=0x0400,
		/// <summary>
		/// GetFontLanguageInfo の呼び出しでエラーが発生した場合、GCP_ERROR が返ります。
		/// </summary>
		GCP_ERROR=0x8000,
		/// <summary>
		/// FLI_MASK でマスクされている場合、戻り値を直接 GetCharacterPlacement 関数に渡すことができます。
		/// </summary>
		FLI_MASK=0x103B,

		/// <summary>
		/// 文字列の長さが nMaxExtent パラメータと同じになるように、lpDx 配列内の範囲を調節します。
		/// GCP_JUSTIFY フラグは GCP_MAXEXTENT フラグと一緒の場合にのみ使われます。
		/// </summary>
		GCP_JUSTIFY=0x00010000,

		/// <summary>
		/// GCP_NODIACRITICS の値は定義から外されているため、使わないでください
		/// </summary>
		[System.Obsolete]
		GCP_NODIACRITICS=0x00020000,

		/// <summary>
		/// フォントには通常そのコードページを使ってアクセスできない、余分なグリフが含まれています。
		/// そのグリフにアクセスするには、GetCharacterPlacement 関数を使います。
		/// この値は情報を提供するだけで、GetCharacterPlacement 関数に渡すために使うことはできません。
		/// </summary>
		FLI_GLYPHS=0x00040000,

		/// <summary>
		/// lpClass 配列に、予め設定された文字の分類が入っていることを表します。
		/// 分類は出力と同じになります。ある文字に対する特定の分類が未知の場合、
		/// それに対応する配列内の位置は 0 に設定されます。
		/// 分類の詳細については、GCP_RESULTS 構造体を参照してください。
		/// これは、GetFontLanguageInfo 関数が GCP_REORDER フラグを返した場合にだけ使用可能です。
		/// </summary>
		GCP_CLASSIN=0x00080000,
		/// <summary>
		/// 文字列の範囲が nMaxExtent パラメータで与えられる論理単位での値を超えない場合に限り、それを計算します。
		/// </summary>
		GCP_MAXEXTENT=0x00100000,
		/// <summary>
		/// ?
		/// </summary>
		GCP_JUSTIFYIN=0x00200000,
		/// <summary>
		/// 単語内の文字の位置によって、グリフが異なったり、順序を変更したりする必要がある言語では、
		/// 非表示文字がコードページ内に表示される場合があります。たとえばヘブライ語のコードページでは、
		/// 出力文字列の最後の位置を決定するために、左から右マーカーおよび右から左マーカーが存在します。
		/// 通常これらのマーカーは表示されず、lpGlyphs 配列および lpDx 配列から除去されます。
		/// これらの文字を表示するには、GCP_DISPLAYZWG フラグを使います。
		/// </summary>
		GCP_DISPLAYZWG=0x00400000,
		/// <summary>
		/// セム語でだけ使われます。交換可能な文字はリセットされないことを表します。
		/// たとえば、右から左の文字列では、'（'and'）' は逆になりません。
		/// </summary>
		GCP_SYMSWAPOFF=0x00800000,
		/// <summary>
		/// 特定の言語でだけ使われます。通常の数字の処理を無効にし、
		/// 文字列の読み取り順に合致する強い文字として処理します。GCP_REORDER フラグと一緒の場合にだけ有用です。
		/// </summary>
		GCP_NUMERICOVERRIDE=0x01000000,
		/// <summary>
		/// 特定の言語でだけ使われます。通常の中間文字種の処理を無効にし、
		/// 文字列の読み取り順に合致する強い文字として処理します。GCP_REORDER フラグと共に使われる場合にだけ有用です。
		/// </summary>
		GCP_NEUTRALOVERRIDE=0x02000000,
		/// <summary>
		/// アラビア語とタイ語でだけ使われます。数字に対しては標準のラテングリフを使い、
		/// システムの既定を無効にします。その言語のそのフォントでこのオプションが利用可能かどうかを調べるには、
		/// GetStringTypeEx 関数を使って、その言語が複数の数字書式をサポートするかどうかを調べます。
		/// </summary>
		GCP_NUMERICSLATIN=0x04000000,
		/// <summary>
		/// アラビア語とタイ語でだけ使われます。数字に対してローカルグリフを使い、システムの既定を無効にします。
		/// その言語のそのフォントでこのオプションが利用可能かどうかを調べるには、GetStringTypeEx 関数を使って、
		/// その言語が複数の数字書式をサポートするかどうかを調べます。
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