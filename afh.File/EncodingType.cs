namespace afh.File{
	/// <summary>
	/// 符号形式 (読み書きする際の形式) を指定する為の列挙型です。
	/// </summary>
	public enum EncodingType:uint{
		/// <summary>
		/// 特に格納形式を指定しないで読み書きします。
		/// 実際には既定の読み書きの方法を用いて読み書きが行われます。
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		NoSpecified=0,
		/// <summary>
		/// 基本的な情報の格納方法の型を表現する部分を抽出するのに使用します。
		/// </summary>
		MASK_TYPE=0x3fff,
		/// <summary>
		/// 使用している文字コードの指定など詳細な情報を表現する為の部分を抽出するのに使用します。
		/// </summary>
		MASK_OPTION=0xffff0000,
		//=================================================
		//		整数
		//=================================================
		/// <summary>
		/// 1 byte 符号付き整数として読み書きします。
		/// </summary>
		I1=1,
		/// <summary>
		/// 1 byte 符号無し整数として読み書きします。
		/// </summary>
		U1=2,
		/// <summary>
		/// 2 byte 符号付き整数として読み書きします。
		/// </summary>
		I2=3,
		/// <summary>
		/// 2 byte 符号付き整数 (Big Endian) として読み書きします。
		/// </summary>
		I2BE=4,
		/// <summary>
		/// 2 byte 符号無し整数として読み書きします。
		/// </summary>
		U2=5,
		/// <summary>
		/// 2 byte 符号無し整数 (Big Endian) として読み書きします。
		/// </summary>
		U2BE=6,
		//=================================================
		//		System.Int32
		//=================================================
		/// <summary>
		/// 3 byte 符号付き整数として読み書きします。
		/// </summary>
		I3=7,
		/// <summary>
		/// 3 byte 符号付き整数 (Big Endian) として読み書きします。
		/// </summary>
		I3BE=8,
		/// <summary>
		/// 4 byte 符号付き整数として読み書きします。
		/// </summary>
		I4=9,
		/// <summary>
		/// 4 byte 符号付き整数 (Big Endian) として読み書きします。
		/// </summary>
		I4BE=10,
		/// <summary>
		/// 28 bit 符号付き整数として読み書きします。
		/// 4B それぞれの下位 7bit に値を保持します。
		/// </summary>
		Int28=11,
		/// <summary>
		/// 28 bit 符号付き整数 (Big Endian) として読み書きします。
		/// 4B それぞれの下位 7bit に値を保持します。
		/// </summary>
		Int28BE=12,
		//=================================================
		//		System.UInt32
		//=================================================
		/// <summary>
		/// 3 byte 符号無し整数として読み書きします。
		/// </summary>
		U3=13,
		/// <summary>
		/// 3 byte 符号無し整数 (Big Endian) として読み書きします。
		/// </summary>
		U3BE=14,
		/// <summary>
		/// 4 byte 符号無し整数として読み書きします。
		/// </summary>
		U4=15,
		/// <summary>
		/// 4 byte 符号無し整数 (Big Endian) として読み書きします。
		/// </summary>
		U4BE=16,
		/// <summary>
		/// 28 bit 符号付き整数として読み書きします。
		/// 4B それぞれの下位 7bit に値を保持します。
		/// </summary>
		UInt28=17,
		/// <summary>
		/// 28 bit 符号付き整数 (Big Endian) として読み書きします。
		/// 4B それぞれの下位 7bit に値を保持します。
		/// </summary>
		UInt28BE=18,
		//=================================================
		//		System.Int64/UInt64
		//=================================================
		/// <summary>
		/// 8 byte 符号付き整数として読み書きします。
		/// </summary>
		I8=19,
		/// <summary>
		/// 8 byte 符号付き整数 (Big Endian) として読み書きします。
		/// </summary>
		I8BE=20,
		/// <summary>
		/// 8 byte 符号無し整数として読み書きします。
		/// </summary>
		U8=21,
		/// <summary>
		/// 8 byte 符号無し整数 (Big Endian) として読み書きします。
		/// </summary>
		U8BE=22,
		//=================================================
		//		小数
		//=================================================
		/// <summary>
		/// 4 byte 浮動小数点数として読み書きします。
		/// </summary>
		F4	=0x2A,
		/// <summary>
		/// 4 byte 浮動小数点数 (Big Endian) として読み書きします。
		/// </summary>
		F4BE=0x2B,
		/// <summary>
		/// 8 byte 浮動小数点数として読み書きします。
		/// </summary>
		F8	=0x2C,
		/// <summary>
		/// 8 byte 浮動小数点数 (Big Endian) として読み書きします。
		/// </summary>
		F8BE=0x2D,
		//=================================================
		//		文字列
		//=================================================
		/// <summary>
		/// ASCII 二文字コードの文字列として読み書きします。
		/// </summary>
		CC2=0x17,
		/// <summary>
		/// ASCII 三文字コードの文字列として読み書きします。
		/// </summary>
		CC3=0x18,
		/// <summary>
		/// ASCII 四文字コードの文字列として読み書きします。
		/// 所謂 FOURCC と同じです。
		/// </summary>
		CC4=0x19,
		//=================================================
		//		文字列
		//	形式: 0b PPPPPPPP PPPPPPPP QQ000000 00100RRR
		//=================================================
		//	100RRR: 長さの指定方法
		//-------------------------------------------------
		// P: Pascal
		// B: Basic/Binary
		// T: NullTerminated
		//-------------------------------------------------
		/// <summary>
		/// 文字列として読み書きします。
		/// 文字列の長さは別に指定する必要があります。
		/// </summary>
		String			=0x20, // 0b??000000 0x20
		/// <summary>
		/// 文字列として読み書きします。
		/// 文字列は null 文字で終わります。
		/// </summary>
		StrTerminated	=0x21, // 0b??000000 0x21
		/// <summary>
		/// 文字列として読み書きします。
		/// 文字列の長さ (文字数) は先頭に 4B(LE) で格納されます。
		/// </summary>
		StrBasic		=0x22, // 0b??000000 0x22
		/// <summary>
		/// 文字列として読み書きします。
		/// 文字列の長さ (文字数) は先頭に 1B で格納されます。
		/// </summary>
		StrPascal		=0x23, // 0b??000000 0x23
		/// <summary>
		/// 文字列として読み書きします。
		/// 文字列の長さ (Stream 内の byte 数) は先頭に 4B で格納されます。
		/// </summary>
		StrSize			=0x24, // 0b??000000 0x23
		//-------------------------------------------------
		//	QQ: 文字コードの指定方法
		//-------------------------------------------------
		// A: ANSI (1 byte 文字)
		// W: 2 byte 文字 (Unicode: 自動判定)
		// E: Encoding 情報付き
		//-------------------------------------------------
		/// <summary>
		/// 文字コードの指定方法の部分を取り出す為のマスクです。
		/// </summary>
		[afh.EnumDescription("EncEmbedded",Key="STRING_ENCSPEC")]
		MASK_STRING_ENCSPEC=0xC000,
		/// <summary>
		/// 文字コードを直接指定します (既定値)
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		EncRaw		=0x0000,
		/// <summary>
		/// 文字コードは ANSI (ASCII) を使用します。
		/// </summary>
		EncAnsi		=0x4000,
		/// <summary>
		/// 文字コードは UTF-16 (BOM 指定で UTF-16BE 及び UTF-8 も可) を使用します。
		/// </summary>
		EncWide		=0x8000,
		/// <summary>
		/// 文字コード情報は、文字列データが始まる前に 2B で code page として記録されます。
		/// </summary>
		EncEmbedded	=0xc000,
		//-------------------------------------------------
		//	PPPPPPPP:PPPPPPPP: 文字コード CodePage
		//-------------------------------------------------
		#region 文字コード
		/// <summary>
		/// 文字コードとして既定値を使用します。
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		Enc_Default	=0x00000000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (US - カナダ) を使用します。
		/// </summary>
		Enc_IBM037	=0x00250000,
		/// <summary>
		/// 文字コードとして OEM アメリカ合衆国 を使用します。
		/// </summary>
		Enc_IBM437	=0x01b50000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (インターナショナル) を使用します。
		/// </summary>
		Enc_IBM500	=0x01f40000,
		/// <summary>
		/// 文字コードとして アラビア語 (ASMO 708) を使用します。
		/// </summary>
		Enc_ASMO_708=0x02c40000,
		/// <summary>
		/// 文字コードとして アラビア語 (DOS) を使用します。
		/// </summary>
		Enc_DOS_720	=0x02d00000,
		/// <summary>
		/// 文字コードとして ギリシャ語 (DOS) を使用します。
		/// </summary>
		Enc_ibm737	=0x02e10000,
		/// <summary>
		/// 文字コードとして バルト言語 (DOS) を使用します。
		/// </summary>
		Enc_ibm775	=0x03070000,
		/// <summary>
		/// 文字コードとして 西ヨーロッパ言語 (DOS) を使用します。
		/// </summary>
		Enc_ibm850	=0x03520000,
		/// <summary>
		/// 文字コードとして 中央ヨーロッパ言語 (DOS) を使用します。
		/// </summary>
		Enc_ibm852	=0x03540000,
		/// <summary>
		/// 文字コードとして OEM キリル を使用します。
		/// </summary>
		Enc_IBM855	=0x03570000,
		/// <summary>
		/// 文字コードとして トルコ語 (DOS) を使用します。
		/// </summary>
		Enc_ibm857	=0x03590000,
		/// <summary>
		/// 文字コードとして OEM マルチリンガル ラテン I を使用します。
		/// </summary>
		Enc_IBM00858=0x035a0000,
		/// <summary>
		/// 文字コードとして ポルトガル語  (DOS) を使用します。
		/// </summary>
		Enc_IBM860	=0x035c0000,
		/// <summary>
		/// 文字コードとして アイスランド語 (DOS) を使用します。
		/// </summary>
		Enc_ibm861	=0x035d0000,
		/// <summary>
		/// 文字コードとして ヘブライ語 (DOS) を使用します。
		/// </summary>
		Enc_DOS_862	=0x035e0000,
		/// <summary>
		/// 文字コードとして フランス語 (カナダ) (DOS) を使用します。
		/// </summary>
		Enc_IBM863	=0x035f0000,
		/// <summary>
		/// 文字コードとして アラビア語 (864) を使用します。
		/// </summary>
		Enc_IBM864	=0x03600000,
		/// <summary>
		/// 文字コードとして 北欧 (DOS) を使用します。
		/// </summary>
		Enc_IBM865	=0x03610000,
		/// <summary>
		/// 文字コードとして キリル言語 (DOS) を使用します。
		/// </summary>
		Enc_cp866	=0x03620000,
		/// <summary>
		/// 文字コードとして ギリシャ語, Modern (DOS) を使用します。
		/// </summary>
		Enc_ibm869	=0x03650000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (多国語ラテン 2) を使用します。
		/// </summary>
		Enc_IBM870	=0x03660000,
		/// <summary>
		/// 文字コードとして タイ語 (Windows) を使用します。
		/// </summary>
		Enc_windows_874	=0x036a0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (ギリシャ語 Modern) を使用します。
		/// </summary>
		Enc_cp875		=0x036b0000,
		/// <summary>
		/// 文字コードとして 日本語 (シフト JIS) を使用します。
		/// </summary>
		Enc_shift_jis	=0x03a40000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (GB2312) を使用します。
		/// </summary>
		Enc_gb2312		=0x03a80000,
		/// <summary>
		/// 文字コードとして 韓国語 を使用します。
		/// </summary>
		Enc_ks_c_5601_1987	=0x03b50000,
		/// <summary>
		/// 文字コードとして 繁体字中国語 (Big5) を使用します。
		/// </summary>
		Enc_big5		=0x03b60000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (トルコ語ラテン 5) を使用します。
		/// </summary>
		Enc_IBM1026		=0x04020000,
		/// <summary>
		/// 文字コードとして IBM ラテン-1 を使用します。
		/// </summary>
		Enc_IBM01047	=0x04170000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (US - カナダ - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01140	=0x04740000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (ドイツ - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01141	=0x04750000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (デンマーク - ノルウェー - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01142	=0x04760000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (フィンランド - スウェーデン - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01143	=0x04770000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (イタリア - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01144	=0x04780000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (スペイン - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01145	=0x04790000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (UK - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01146	=0x047a0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (フランス - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01147	=0x047b0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (インターナショナル - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01148	=0x047c0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (アイスランド語 - ヨーロッパ) を使用します。
		/// </summary>
		Enc_IBM01149	=0x047d0000,
		/// <summary>
		/// 文字コードとして Unicode を使用します。
		/// </summary>
		Enc_utf_16		=0x04b00000,
		/// <summary>
		/// 文字コードとして Unicode (Big-Endian) を使用します。
		/// </summary>
		Enc_utf_16BE	=0x04b10000,	// unicodeFFFE
		/// <summary>
		/// 文字コードとして 中央ヨーロッパ言語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1250	=0x04e20000,
		/// <summary>
		/// 文字コードとして キリル言語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1251	=0x04e30000,
		/// <summary>
		/// 文字コードとして 西ヨーロッパ言語 (Windows) を使用します。
		/// </summary>
		Enc_Windows_1252	=0x04e40000,
		/// <summary>
		/// 文字コードとして ギリシャ語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1253	=0x04e50000,
		/// <summary>
		/// 文字コードとして トルコ語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1254	=0x04e60000,
		/// <summary>
		/// 文字コードとして ヘブライ語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1255	=0x04e70000,
		/// <summary>
		/// 文字コードとして アラビア語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1256	=0x04e80000,
		/// <summary>
		/// 文字コードとして バルト言語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1257	=0x04e90000,
		/// <summary>
		/// 文字コードとして ベトナム語 (Windows) を使用します。
		/// </summary>
		Enc_windows_1258	=0x04ea0000,
		/// <summary>
		/// 文字コードとして 韓国語 (Johab) を使用します。
		/// </summary>
		Enc_Johab			=0x05510000,
		/// <summary>
		/// 文字コードとして 西ヨーロッパ言語 (Mac) を使用します。
		/// </summary>
		Enc_macintosh		=0x27100000,
		/// <summary>
		/// 文字コードとして 日本語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_japanese	=0x27110000,
		/// <summary>
		/// 文字コードとして 繁体字中国語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_chinesetrad	=0x27120000,
		/// <summary>
		/// 文字コードとして 韓国語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_korean	=0x27130000,
		/// <summary>
		/// 文字コードとして アラビア語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_arabic	=0x27140000,
		/// <summary>
		/// 文字コードとして ヘブライ語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_hebrew	=0x27150000,
		/// <summary>
		/// 文字コードとして ギリシャ語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_greek		=0x27160000,
		/// <summary>
		/// 文字コードとして キリル言語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_cyrillic	=0x27170000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_chinesesimp	=0x27180000,
		/// <summary>
		/// 文字コードとして ルーマニア語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_romanian	=0x271a0000,
		/// <summary>
		/// 文字コードとして ウクライナ語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_ukrainian	=0x27210000,
		/// <summary>
		/// 文字コードとして タイ語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_thai		=0x27250000,
		/// <summary>
		/// 文字コードとして 中央ヨーロッパ言語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_ce		=0x272d0000,
		/// <summary>
		/// 文字コードとして アイスランド語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_icelandic	=0x275f0000,
		/// <summary>
		/// 文字コードとして トルコ語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_turkish	=0x27610000,
		/// <summary>
		/// 文字コードとして クロアチア語 (Mac) を使用します。
		/// </summary>
		Enc_x_mac_croatian	=0x27620000,
		/// <summary>
		/// 文字コードとして Unicode (UTF-32) を使用します。
		/// </summary>
		Enc_utf_32			=0x2ee00000,
		/// <summary>
		/// 文字コードとして Unicode (UTF-32 ビッグ エンディアン) を使用します。
		/// </summary>
		Enc_utf_32BE		=0x2ee10000,
		/// <summary>
		/// 文字コードとして 繁体字中国語 (CNS) を使用します。
		/// </summary>
		Enc_x_Chinese_CNS	=0x4e200000,
		/// <summary>
		/// 文字コードとして TCA 台湾 を使用します。
		/// </summary>
		Enc_x_cp20001		=0x4e210000,
		/// <summary>
		/// 文字コードとして 繁体字中国語 (Eten) を使用します。
		/// </summary>
		Enc_x_Chinese_Eten	=0x4e220000,
		/// <summary>
		/// 文字コードとして IBM5550 台湾 を使用します。
		/// </summary>
		Enc_x_cp20003		=0x4e230000,
		/// <summary>
		/// 文字コードとして TeleText 台湾 を使用します。
		/// </summary>
		Enc_x_cp20004		=0x4e240000,
		/// <summary>
		/// 文字コードとして Wang 台湾 を使用します。
		/// </summary>
		Enc_x_cp20005		=0x4e250000,
		/// <summary>
		/// 文字コードとして 西ヨーロッパ言語 (IA5) を使用します。
		/// </summary>
		Enc_x_IA5			=0x4e890000,
		/// <summary>
		/// 文字コードとして ドイツ語 (IA5) を使用します。
		/// </summary>
		Enc_x_IA5_German	=0x4e8a0000,
		/// <summary>
		/// 文字コードとして スウェーデン語 (IA5) を使用します。
		/// </summary>
		Enc_x_IA5_Swedish	=0x4e8b0000,
		/// <summary>
		/// 文字コードとして ノルウェー語 (IA5) を使用します。
		/// </summary>
		Enc_x_IA5_Norwegian	=0x4e8c0000,
		/// <summary>
		/// 文字コードとして US-ASCII を使用します。
		/// </summary>
		Enc_us_ascii	=0x4e9f0000,
		/// <summary>
		/// 文字コードとして T.61 を使用します。
		/// </summary>
		Enc_x_cp20261	=0x4f250000,
		/// <summary>
		/// 文字コードとして ISO-6937 を使用します。
		/// </summary>
		Enc_x_cp20269	=0x4f2d0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (ドイツ) を使用します。
		/// </summary>
		Enc_IBM273		=0x4f310000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (デンマーク - ノルウェー) を使用します。
		/// </summary>
		Enc_IBM277		=0x4f350000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (フィンランド - スウェーデン) を使用します。
		/// </summary>
		Enc_IBM278		=0x4f360000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (イタリア) を使用します。
		/// </summary>
		Enc_IBM280		=0x4f380000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (スペイン) を使用します。
		/// </summary>
		Enc_IBM284		=0x4f3c0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (UK) を使用します。
		/// </summary>
		Enc_IBM285		=0x4f3d0000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (日本語カタカナ) を使用します。
		/// </summary>
		Enc_IBM290		=0x4f420000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (フランス) を使用します。
		/// </summary>
		Enc_IBM297		=0x4f490000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (アラビア語) を使用します。
		/// </summary>
		Enc_IBM420		=0x4fc40000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (ギリシャ語) を使用します。
		/// </summary>
		Enc_IBM423		=0x4fc70000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (ヘブライ語) を使用します。
		/// </summary>
		Enc_IBM424		=0x4fc80000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (韓国語 Extended) を使用します。
		/// </summary>
		Enc_x_EBCDIC_KoreanExtended	=0x51610000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (タイ語) を使用します。
		/// </summary>
		Enc_IBM_Thai	=0x51660000,
		/// <summary>
		/// 文字コードとして キリル言語 (KOI8-R) を使用します。
		/// </summary>
		Enc_koi8_r		=0x51820000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (アイスランド語) を使用します。
		/// </summary>
		Enc_IBM871		=0x51870000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (キリル言語 - ロシア語) を使用します。
		/// </summary>
		Enc_IBM880		=0x51900000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (トルコ語) を使用します。
		/// </summary>
		Enc_IBM905		=0x51a90000,
		/// <summary>
		/// 文字コードとして IBM ラテン-1 を使用します。
		/// </summary>
		Enc_IBM00924	=0x51bc0000,
		/// <summary>
		/// 文字コードとして 日本語 (JIS 0208-1990 および 0212-1990) を使用します。
		/// </summary>
		Enc_EUC_JP		=0x51c40000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (GB2312-80) を使用します。
		/// </summary>
		Enc_x_cp20936	=0x51c80000,
		/// <summary>
		/// 文字コードとして 韓国語 Wansung を使用します。
		/// </summary>
		Enc_x_cp20949	=0x51d50000,
		/// <summary>
		/// 文字コードとして IBM EBCDIC (キリル言語 セルビア - ブルガリア) を使用します。
		/// </summary>
		Enc_cp1025		=0x52210000,
		/// <summary>
		/// 文字コードとして キリル言語 (KOI8-U) を使用します。
		/// </summary>
		Enc_koi8_u		=0x556a0000,
		/// <summary>
		/// 文字コードとして 西ヨーロッパ言語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_1	=0x6faf0000,
		/// <summary>
		/// 文字コードとして 中央ヨーロッパ言語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_2	=0x6fb00000,
		/// <summary>
		/// 文字コードとして ラテン 3 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_3	=0x6fb10000,
		/// <summary>
		/// 文字コードとして バルト言語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_4	=0x6fb20000,
		/// <summary>
		/// 文字コードとして キリル言語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_5	=0x6fb30000,
		/// <summary>
		/// 文字コードとして アラビア語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_6	=0x6fb40000,
		/// <summary>
		/// 文字コードとして ギリシャ語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_7	=0x6fb50000,
		/// <summary>
		/// 文字コードとして ヘブライ語 (ISO-Visual) を使用します。
		/// </summary>
		Enc_iso_8859_8	=0x6fb60000,
		/// <summary>
		/// 文字コードとして トルコ語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_9	=0x6fb70000,
		/// <summary>
		/// 文字コードとして エストニア語 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_13	=0x6fbb0000,
		/// <summary>
		/// 文字コードとして ラテン 9 (ISO) を使用します。
		/// </summary>
		Enc_iso_8859_15	=0x6fbd0000,
		/// <summary>
		/// 文字コードとして ヨーロッパ を使用します。
		/// </summary>
		Enc_x_Europa	=0x71490000,
		/// <summary>
		/// 文字コードとして ヘブライ語 (ISO-Logical) を使用します。
		/// </summary>
		Enc_iso_8859_8_i=0x96c60000,
		/// <summary>
		/// 文字コードとして 日本語 (JIS) を使用します。
		/// </summary>
		Enc_iso_2022_jp	=0xc42c0000,
		/// <summary>
		/// 文字コードとして 日本語 (JIS 1 バイト カタカナ可) を使用します。
		/// </summary>
		Enc_csISO2022JP	=0xc42d0000,
		/// <summary>
		/// 文字コードとして 日本語 (JIS 1 バイト カタカナ可 - SO/SI) を使用します。
		/// </summary>
		Enc_iso_2022_jp_1BKana	=0xc42e0000,	// iso_2022_jp
		/// <summary>
		/// 文字コードとして 韓国語 (ISO) を使用します。
		/// </summary>
		Enc_iso_2022_kr	=0xc4310000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (ISO-2022) を使用します。
		/// </summary>
		Enc_x_cp50227	=0xc4330000,
		/// <summary>
		/// 文字コードとして 日本語 (EUC) を使用します。
		/// </summary>
		Enc_euc_jp		=0xcadc0000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (EUC) を使用します。
		/// </summary>
		Enc_EUC_CN		=0xcae00000,
		/// <summary>
		/// 文字コードとして 韓国語 (EUC) を使用します。
		/// </summary>
		Enc_euc_kr		=0xcaed0000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (HZ) を使用します。
		/// </summary>
		Enc_hz_gb_2312	=0xcec80000,
		/// <summary>
		/// 文字コードとして 簡体字中国語 (GB18030) を使用します。
		/// </summary>
		Enc_GB18030		=0xd6980000,
		/// <summary>
		/// 文字コードとして ISCII デバナガリ文字 を使用します。
		/// </summary>
		Enc_x_iscii_de	=0xdeaa0000,
		/// <summary>
		/// 文字コードとして ISCII ベンガル語 を使用します。
		/// </summary>
		Enc_x_iscii_be	=0xdeab0000,
		/// <summary>
		/// 文字コードとして ISCII タミール語 を使用します。
		/// </summary>
		Enc_x_iscii_ta	=0xdeac0000,
		/// <summary>
		/// 文字コードとして ISCII テルグ語 を使用します。
		/// </summary>
		Enc_x_iscii_te	=0xdead0000,
		/// <summary>
		/// 文字コードとして ISCII アッサム語 を使用します。
		/// </summary>
		Enc_x_iscii_as	=0xdeae0000,
		/// <summary>
		/// 文字コードとして ISCII オリヤー語 を使用します。
		/// </summary>
		Enc_x_iscii_or	=0xdeaf0000,
		/// <summary>
		/// 文字コードとして ISCII カナラ語 を使用します。
		/// </summary>
		Enc_x_iscii_ka	=0xdeb00000,
		/// <summary>
		/// 文字コードとして ISCII マラヤラム語 を使用します。
		/// </summary>
		Enc_x_iscii_ma	=0xdeb10000,
		/// <summary>
		/// 文字コードとして ISCII グジャラート語 を使用します。
		/// </summary>
		Enc_x_iscii_gu	=0xdeb20000,
		/// <summary>
		/// 文字コードとして ISCII パンジャブ語 を使用します。
		/// </summary>
		Enc_x_iscii_pa	=0xdeb30000,
		/// <summary>
		/// 文字コードとして Unicode (UTF-7) を使用します。
		/// </summary>
		Enc_utf_7		=0xfde80000,
		/// <summary>
		/// 文字コードとして Unicode (UTF-8) を使用します。
		/// </summary>
		Enc_utf_8		=0xfde90000,
		#endregion
	}
}