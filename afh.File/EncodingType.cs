namespace afh.File{
	/// <summary>
	/// �����`�� (�ǂݏ�������ۂ̌`��) ���w�肷��ׂ̗񋓌^�ł��B
	/// </summary>
	public enum EncodingType:uint{
		/// <summary>
		/// ���Ɋi�[�`�����w�肵�Ȃ��œǂݏ������܂��B
		/// ���ۂɂ͊���̓ǂݏ����̕��@��p���ēǂݏ������s���܂��B
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		NoSpecified=0,
		/// <summary>
		/// ��{�I�ȏ��̊i�[���@�̌^��\�����镔���𒊏o����̂Ɏg�p���܂��B
		/// </summary>
		MASK_TYPE=0x3fff,
		/// <summary>
		/// �g�p���Ă��镶���R�[�h�̎w��ȂǏڍׂȏ���\������ׂ̕����𒊏o����̂Ɏg�p���܂��B
		/// </summary>
		MASK_OPTION=0xffff0000,
		//=================================================
		//		����
		//=================================================
		/// <summary>
		/// 1 byte �����t�������Ƃ��ēǂݏ������܂��B
		/// </summary>
		I1=1,
		/// <summary>
		/// 1 byte �������������Ƃ��ēǂݏ������܂��B
		/// </summary>
		U1=2,
		/// <summary>
		/// 2 byte �����t�������Ƃ��ēǂݏ������܂��B
		/// </summary>
		I2=3,
		/// <summary>
		/// 2 byte �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		I2BE=4,
		/// <summary>
		/// 2 byte �������������Ƃ��ēǂݏ������܂��B
		/// </summary>
		U2=5,
		/// <summary>
		/// 2 byte ������������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		U2BE=6,
		//=================================================
		//		System.Int32
		//=================================================
		/// <summary>
		/// 3 byte �����t�������Ƃ��ēǂݏ������܂��B
		/// </summary>
		I3=7,
		/// <summary>
		/// 3 byte �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		I3BE=8,
		/// <summary>
		/// 4 byte �����t�������Ƃ��ēǂݏ������܂��B
		/// </summary>
		I4=9,
		/// <summary>
		/// 4 byte �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		I4BE=10,
		/// <summary>
		/// 28 bit �����t�������Ƃ��ēǂݏ������܂��B
		/// 4B ���ꂼ��̉��� 7bit �ɒl��ێ����܂��B
		/// </summary>
		Int28=11,
		/// <summary>
		/// 28 bit �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// 4B ���ꂼ��̉��� 7bit �ɒl��ێ����܂��B
		/// </summary>
		Int28BE=12,
		//=================================================
		//		System.UInt32
		//=================================================
		/// <summary>
		/// 3 byte �������������Ƃ��ēǂݏ������܂��B
		/// </summary>
		U3=13,
		/// <summary>
		/// 3 byte ������������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		U3BE=14,
		/// <summary>
		/// 4 byte �������������Ƃ��ēǂݏ������܂��B
		/// </summary>
		U4=15,
		/// <summary>
		/// 4 byte ������������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		U4BE=16,
		/// <summary>
		/// 28 bit �����t�������Ƃ��ēǂݏ������܂��B
		/// 4B ���ꂼ��̉��� 7bit �ɒl��ێ����܂��B
		/// </summary>
		UInt28=17,
		/// <summary>
		/// 28 bit �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// 4B ���ꂼ��̉��� 7bit �ɒl��ێ����܂��B
		/// </summary>
		UInt28BE=18,
		//=================================================
		//		System.Int64/UInt64
		//=================================================
		/// <summary>
		/// 8 byte �����t�������Ƃ��ēǂݏ������܂��B
		/// </summary>
		I8=19,
		/// <summary>
		/// 8 byte �����t������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		I8BE=20,
		/// <summary>
		/// 8 byte �������������Ƃ��ēǂݏ������܂��B
		/// </summary>
		U8=21,
		/// <summary>
		/// 8 byte ������������ (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		U8BE=22,
		//=================================================
		//		����
		//=================================================
		/// <summary>
		/// 4 byte ���������_���Ƃ��ēǂݏ������܂��B
		/// </summary>
		F4	=0x2A,
		/// <summary>
		/// 4 byte ���������_�� (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		F4BE=0x2B,
		/// <summary>
		/// 8 byte ���������_���Ƃ��ēǂݏ������܂��B
		/// </summary>
		F8	=0x2C,
		/// <summary>
		/// 8 byte ���������_�� (Big Endian) �Ƃ��ēǂݏ������܂��B
		/// </summary>
		F8BE=0x2D,
		//=================================================
		//		������
		//=================================================
		/// <summary>
		/// ASCII �񕶎��R�[�h�̕�����Ƃ��ēǂݏ������܂��B
		/// </summary>
		CC2=0x17,
		/// <summary>
		/// ASCII �O�����R�[�h�̕�����Ƃ��ēǂݏ������܂��B
		/// </summary>
		CC3=0x18,
		/// <summary>
		/// ASCII �l�����R�[�h�̕�����Ƃ��ēǂݏ������܂��B
		/// ���� FOURCC �Ɠ����ł��B
		/// </summary>
		CC4=0x19,
		//=================================================
		//		������
		//	�`��: 0b PPPPPPPP PPPPPPPP QQ000000 00100RRR
		//=================================================
		//	100RRR: �����̎w����@
		//-------------------------------------------------
		// P: Pascal
		// B: Basic/Binary
		// T: NullTerminated
		//-------------------------------------------------
		/// <summary>
		/// ������Ƃ��ēǂݏ������܂��B
		/// ������̒����͕ʂɎw�肷��K�v������܂��B
		/// </summary>
		String			=0x20, // 0b??000000 0x20
		/// <summary>
		/// ������Ƃ��ēǂݏ������܂��B
		/// ������� null �����ŏI���܂��B
		/// </summary>
		StrTerminated	=0x21, // 0b??000000 0x21
		/// <summary>
		/// ������Ƃ��ēǂݏ������܂��B
		/// ������̒��� (������) �͐擪�� 4B(LE) �Ŋi�[����܂��B
		/// </summary>
		StrBasic		=0x22, // 0b??000000 0x22
		/// <summary>
		/// ������Ƃ��ēǂݏ������܂��B
		/// ������̒��� (������) �͐擪�� 1B �Ŋi�[����܂��B
		/// </summary>
		StrPascal		=0x23, // 0b??000000 0x23
		/// <summary>
		/// ������Ƃ��ēǂݏ������܂��B
		/// ������̒��� (Stream ���� byte ��) �͐擪�� 4B �Ŋi�[����܂��B
		/// </summary>
		StrSize			=0x24, // 0b??000000 0x23
		//-------------------------------------------------
		//	QQ: �����R�[�h�̎w����@
		//-------------------------------------------------
		// A: ANSI (1 byte ����)
		// W: 2 byte ���� (Unicode: ��������)
		// E: Encoding ���t��
		//-------------------------------------------------
		/// <summary>
		/// �����R�[�h�̎w����@�̕��������o���ׂ̃}�X�N�ł��B
		/// </summary>
		[afh.EnumDescription("EncEmbedded",Key="STRING_ENCSPEC")]
		MASK_STRING_ENCSPEC=0xC000,
		/// <summary>
		/// �����R�[�h�𒼐ڎw�肵�܂� (����l)
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		EncRaw		=0x0000,
		/// <summary>
		/// �����R�[�h�� ANSI (ASCII) ���g�p���܂��B
		/// </summary>
		EncAnsi		=0x4000,
		/// <summary>
		/// �����R�[�h�� UTF-16 (BOM �w��� UTF-16BE �y�� UTF-8 ����) ���g�p���܂��B
		/// </summary>
		EncWide		=0x8000,
		/// <summary>
		/// �����R�[�h���́A������f�[�^���n�܂�O�� 2B �� code page �Ƃ��ċL�^����܂��B
		/// </summary>
		EncEmbedded	=0xc000,
		//-------------------------------------------------
		//	PPPPPPPP:PPPPPPPP: �����R�[�h CodePage
		//-------------------------------------------------
		#region �����R�[�h
		/// <summary>
		/// �����R�[�h�Ƃ��Ċ���l���g�p���܂��B
		/// </summary>
		[afh.EnumDescription("NoSpecified",Key="TYPE")]
		[afh.EnumDescription("EncRaw",Key="STRING_ENCSPEC")]
		[afh.EnumDescription("Enc_Default",Key="CHARCTER_CODE")]
		Enc_Default	=0x00000000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (US - �J�i�_) ���g�p���܂��B
		/// </summary>
		Enc_IBM037	=0x00250000,
		/// <summary>
		/// �����R�[�h�Ƃ��� OEM �A�����J���O�� ���g�p���܂��B
		/// </summary>
		Enc_IBM437	=0x01b50000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�C���^�[�i�V���i��) ���g�p���܂��B
		/// </summary>
		Enc_IBM500	=0x01f40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (ASMO 708) ���g�p���܂��B
		/// </summary>
		Enc_ASMO_708=0x02c40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_DOS_720	=0x02d00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �M���V���� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm737	=0x02e10000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �o���g���� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm775	=0x03070000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �����[���b�p���� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm850	=0x03520000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �������[���b�p���� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm852	=0x03540000,
		/// <summary>
		/// �����R�[�h�Ƃ��� OEM �L���� ���g�p���܂��B
		/// </summary>
		Enc_IBM855	=0x03570000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �g���R�� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm857	=0x03590000,
		/// <summary>
		/// �����R�[�h�Ƃ��� OEM �}���`�����K�� ���e�� I ���g�p���܂��B
		/// </summary>
		Enc_IBM00858=0x035a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �|���g�K����  (DOS) ���g�p���܂��B
		/// </summary>
		Enc_IBM860	=0x035c0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A�C�X�����h�� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm861	=0x035d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �w�u���C�� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_DOS_862	=0x035e0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �t�����X�� (�J�i�_) (DOS) ���g�p���܂��B
		/// </summary>
		Enc_IBM863	=0x035f0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (864) ���g�p���܂��B
		/// </summary>
		Enc_IBM864	=0x03600000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �k�� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_IBM865	=0x03610000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (DOS) ���g�p���܂��B
		/// </summary>
		Enc_cp866	=0x03620000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �M���V����, Modern (DOS) ���g�p���܂��B
		/// </summary>
		Enc_ibm869	=0x03650000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�����ꃉ�e�� 2) ���g�p���܂��B
		/// </summary>
		Enc_IBM870	=0x03660000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �^�C�� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_874	=0x036a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�M���V���� Modern) ���g�p���܂��B
		/// </summary>
		Enc_cp875		=0x036b0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (�V�t�g JIS) ���g�p���܂��B
		/// </summary>
		Enc_shift_jis	=0x03a40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (GB2312) ���g�p���܂��B
		/// </summary>
		Enc_gb2312		=0x03a80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� ���g�p���܂��B
		/// </summary>
		Enc_ks_c_5601_1987	=0x03b50000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ɑ̎������� (Big5) ���g�p���܂��B
		/// </summary>
		Enc_big5		=0x03b60000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�g���R�ꃉ�e�� 5) ���g�p���܂��B
		/// </summary>
		Enc_IBM1026		=0x04020000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM ���e��-1 ���g�p���܂��B
		/// </summary>
		Enc_IBM01047	=0x04170000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (US - �J�i�_ - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01140	=0x04740000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�h�C�c - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01141	=0x04750000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�f���}�[�N - �m���E�F�[ - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01142	=0x04760000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�t�B�������h - �X�E�F�[�f�� - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01143	=0x04770000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�C�^���A - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01144	=0x04780000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�X�y�C�� - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01145	=0x04790000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (UK - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01146	=0x047a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�t�����X - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01147	=0x047b0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�C���^�[�i�V���i�� - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01148	=0x047c0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�A�C�X�����h�� - ���[���b�p) ���g�p���܂��B
		/// </summary>
		Enc_IBM01149	=0x047d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode ���g�p���܂��B
		/// </summary>
		Enc_utf_16		=0x04b00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode (Big-Endian) ���g�p���܂��B
		/// </summary>
		Enc_utf_16BE	=0x04b10000,	// unicodeFFFE
		/// <summary>
		/// �����R�[�h�Ƃ��� �������[���b�p���� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1250	=0x04e20000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1251	=0x04e30000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �����[���b�p���� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_Windows_1252	=0x04e40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �M���V���� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1253	=0x04e50000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �g���R�� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1254	=0x04e60000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �w�u���C�� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1255	=0x04e70000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1256	=0x04e80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �o���g���� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1257	=0x04e90000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �x�g�i���� (Windows) ���g�p���܂��B
		/// </summary>
		Enc_windows_1258	=0x04ea0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� (Johab) ���g�p���܂��B
		/// </summary>
		Enc_Johab			=0x05510000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �����[���b�p���� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_macintosh		=0x27100000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_japanese	=0x27110000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ɑ̎������� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_chinesetrad	=0x27120000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_korean	=0x27130000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_arabic	=0x27140000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �w�u���C�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_hebrew	=0x27150000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �M���V���� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_greek		=0x27160000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_cyrillic	=0x27170000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_chinesesimp	=0x27180000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���[�}�j�A�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_romanian	=0x271a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �E�N���C�i�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_ukrainian	=0x27210000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �^�C�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_thai		=0x27250000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �������[���b�p���� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_ce		=0x272d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A�C�X�����h�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_icelandic	=0x275f0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �g���R�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_turkish	=0x27610000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �N���A�`�A�� (Mac) ���g�p���܂��B
		/// </summary>
		Enc_x_mac_croatian	=0x27620000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode (UTF-32) ���g�p���܂��B
		/// </summary>
		Enc_utf_32			=0x2ee00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode (UTF-32 �r�b�O �G���f�B�A��) ���g�p���܂��B
		/// </summary>
		Enc_utf_32BE		=0x2ee10000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ɑ̎������� (CNS) ���g�p���܂��B
		/// </summary>
		Enc_x_Chinese_CNS	=0x4e200000,
		/// <summary>
		/// �����R�[�h�Ƃ��� TCA ��p ���g�p���܂��B
		/// </summary>
		Enc_x_cp20001		=0x4e210000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ɑ̎������� (Eten) ���g�p���܂��B
		/// </summary>
		Enc_x_Chinese_Eten	=0x4e220000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM5550 ��p ���g�p���܂��B
		/// </summary>
		Enc_x_cp20003		=0x4e230000,
		/// <summary>
		/// �����R�[�h�Ƃ��� TeleText ��p ���g�p���܂��B
		/// </summary>
		Enc_x_cp20004		=0x4e240000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Wang ��p ���g�p���܂��B
		/// </summary>
		Enc_x_cp20005		=0x4e250000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �����[���b�p���� (IA5) ���g�p���܂��B
		/// </summary>
		Enc_x_IA5			=0x4e890000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �h�C�c�� (IA5) ���g�p���܂��B
		/// </summary>
		Enc_x_IA5_German	=0x4e8a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �X�E�F�[�f���� (IA5) ���g�p���܂��B
		/// </summary>
		Enc_x_IA5_Swedish	=0x4e8b0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �m���E�F�[�� (IA5) ���g�p���܂��B
		/// </summary>
		Enc_x_IA5_Norwegian	=0x4e8c0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� US-ASCII ���g�p���܂��B
		/// </summary>
		Enc_us_ascii	=0x4e9f0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� T.61 ���g�p���܂��B
		/// </summary>
		Enc_x_cp20261	=0x4f250000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISO-6937 ���g�p���܂��B
		/// </summary>
		Enc_x_cp20269	=0x4f2d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�h�C�c) ���g�p���܂��B
		/// </summary>
		Enc_IBM273		=0x4f310000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�f���}�[�N - �m���E�F�[) ���g�p���܂��B
		/// </summary>
		Enc_IBM277		=0x4f350000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�t�B�������h - �X�E�F�[�f��) ���g�p���܂��B
		/// </summary>
		Enc_IBM278		=0x4f360000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�C�^���A) ���g�p���܂��B
		/// </summary>
		Enc_IBM280		=0x4f380000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�X�y�C��) ���g�p���܂��B
		/// </summary>
		Enc_IBM284		=0x4f3c0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (UK) ���g�p���܂��B
		/// </summary>
		Enc_IBM285		=0x4f3d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (���{��J�^�J�i) ���g�p���܂��B
		/// </summary>
		Enc_IBM290		=0x4f420000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�t�����X) ���g�p���܂��B
		/// </summary>
		Enc_IBM297		=0x4f490000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�A���r�A��) ���g�p���܂��B
		/// </summary>
		Enc_IBM420		=0x4fc40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�M���V����) ���g�p���܂��B
		/// </summary>
		Enc_IBM423		=0x4fc70000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�w�u���C��) ���g�p���܂��B
		/// </summary>
		Enc_IBM424		=0x4fc80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�؍��� Extended) ���g�p���܂��B
		/// </summary>
		Enc_x_EBCDIC_KoreanExtended	=0x51610000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�^�C��) ���g�p���܂��B
		/// </summary>
		Enc_IBM_Thai	=0x51660000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (KOI8-R) ���g�p���܂��B
		/// </summary>
		Enc_koi8_r		=0x51820000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�A�C�X�����h��) ���g�p���܂��B
		/// </summary>
		Enc_IBM871		=0x51870000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�L�������� - ���V�A��) ���g�p���܂��B
		/// </summary>
		Enc_IBM880		=0x51900000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�g���R��) ���g�p���܂��B
		/// </summary>
		Enc_IBM905		=0x51a90000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM ���e��-1 ���g�p���܂��B
		/// </summary>
		Enc_IBM00924	=0x51bc0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (JIS 0208-1990 ����� 0212-1990) ���g�p���܂��B
		/// </summary>
		Enc_EUC_JP		=0x51c40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (GB2312-80) ���g�p���܂��B
		/// </summary>
		Enc_x_cp20936	=0x51c80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� Wansung ���g�p���܂��B
		/// </summary>
		Enc_x_cp20949	=0x51d50000,
		/// <summary>
		/// �����R�[�h�Ƃ��� IBM EBCDIC (�L�������� �Z���r�A - �u���K���A) ���g�p���܂��B
		/// </summary>
		Enc_cp1025		=0x52210000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (KOI8-U) ���g�p���܂��B
		/// </summary>
		Enc_koi8_u		=0x556a0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �����[���b�p���� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_1	=0x6faf0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �������[���b�p���� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_2	=0x6fb00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���e�� 3 (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_3	=0x6fb10000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �o���g���� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_4	=0x6fb20000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �L�������� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_5	=0x6fb30000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �A���r�A�� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_6	=0x6fb40000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �M���V���� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_7	=0x6fb50000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �w�u���C�� (ISO-Visual) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_8	=0x6fb60000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �g���R�� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_9	=0x6fb70000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �G�X�g�j�A�� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_13	=0x6fbb0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���e�� 9 (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_15	=0x6fbd0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���[���b�p ���g�p���܂��B
		/// </summary>
		Enc_x_Europa	=0x71490000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �w�u���C�� (ISO-Logical) ���g�p���܂��B
		/// </summary>
		Enc_iso_8859_8_i=0x96c60000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (JIS) ���g�p���܂��B
		/// </summary>
		Enc_iso_2022_jp	=0xc42c0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (JIS 1 �o�C�g �J�^�J�i��) ���g�p���܂��B
		/// </summary>
		Enc_csISO2022JP	=0xc42d0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (JIS 1 �o�C�g �J�^�J�i�� - SO/SI) ���g�p���܂��B
		/// </summary>
		Enc_iso_2022_jp_1BKana	=0xc42e0000,	// iso_2022_jp
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� (ISO) ���g�p���܂��B
		/// </summary>
		Enc_iso_2022_kr	=0xc4310000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (ISO-2022) ���g�p���܂��B
		/// </summary>
		Enc_x_cp50227	=0xc4330000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ���{�� (EUC) ���g�p���܂��B
		/// </summary>
		Enc_euc_jp		=0xcadc0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (EUC) ���g�p���܂��B
		/// </summary>
		Enc_EUC_CN		=0xcae00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �؍��� (EUC) ���g�p���܂��B
		/// </summary>
		Enc_euc_kr		=0xcaed0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (HZ) ���g�p���܂��B
		/// </summary>
		Enc_hz_gb_2312	=0xcec80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� �ȑ̎������� (GB18030) ���g�p���܂��B
		/// </summary>
		Enc_GB18030		=0xd6980000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �f�o�i�K������ ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_de	=0xdeaa0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �x���K���� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_be	=0xdeab0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �^�~�[���� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_ta	=0xdeac0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �e���O�� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_te	=0xdead0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �A�b�T���� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_as	=0xdeae0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �I�����[�� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_or	=0xdeaf0000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �J�i���� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_ka	=0xdeb00000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �}���������� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_ma	=0xdeb10000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �O�W�����[�g�� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_gu	=0xdeb20000,
		/// <summary>
		/// �����R�[�h�Ƃ��� ISCII �p���W���u�� ���g�p���܂��B
		/// </summary>
		Enc_x_iscii_pa	=0xdeb30000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode (UTF-7) ���g�p���܂��B
		/// </summary>
		Enc_utf_7		=0xfde80000,
		/// <summary>
		/// �����R�[�h�Ƃ��� Unicode (UTF-8) ���g�p���܂��B
		/// </summary>
		Enc_utf_8		=0xfde90000,
		#endregion
	}
}