namespace afh.Text{
	/// <summary>
	/// Unicode �̈�ʃJ�e�S���������̂Ɏg�p���܂��B
	/// </summary>
	public enum GeneralCategory:byte{
		#region GeneralCategory
		/// <summary>
		/// �啶���������܂��B
		/// </summary>
		Lu=0x01,
		/// <summary>
		/// �������������܂��B
		/// </summary>
		Ll=0x02,
		/// <summary>
		/// �^�C�g�������������܂��B
		/// </summary>
		Lt=0x03,
		/// <summary>
		/// ���������������܂��B
		/// </summary>
		Lm=0x04,
		/// <summary>
		/// ���̑��̕����������܂��B
		/// </summary>
		Lo=0x05,
		/// <summary>
		/// �땝���������������܂��B
		/// </summary>
		Mn=0x06,
		/// <summary>
		/// ���������������܂��B
		/// </summary>
		Mc=0x07,
		/// <summary>
		/// �͂݌��������������܂��B
		/// </summary>
		Me=0x08,
		/// <summary>
		/// �\�i�����������܂��B
		/// </summary>
		Nd=0x09,
		/// <summary>
		/// ���������������܂��B
		/// </summary>
		Nl=0x0a,
		/// <summary>
		/// ���̑��̐����������܂��B
		/// </summary>
		No=0x0b,

		/// <summary>
		/// �ڑ���Ǔ_�������܂��B
		/// </summary>
		Pc=0x0c,
		/// <summary>
		/// �_�b�V���������܂��B
		/// </summary>
		Pd=0x0d,
		/// <summary>
		/// �n�܂�̊��ʂ������܂��B
		/// </summary>
		Ps=0x0e,
		/// <summary>
		/// �I���̊��ʂ������܂��B
		/// </summary>
		Pe=0x0f,
		/// <summary>
		/// �n�܂�̈��p���������܂��B
		/// </summary>
		Pi=0x10,
		/// <summary>
		/// �I���̈��p���������܂��B
		/// </summary>
		Pf=0x11,
		/// <summary>
		/// ���̑��̋�Ǔ_�������܂��B
		/// </summary>
		Po=0x12,
		/// <summary>
		/// ���w�L���������܂��B
		/// </summary>
		Sm=0x13,
		/// <summary>
		/// �ʉ݋L���������܂��B
		/// </summary>
		Sc=0x14,
		/// <summary>
		/// �C�������������܂��B
		/// </summary>
		Sk=0x15,
		/// <summary>
		/// ���̑��̋L���������܂��B
		/// </summary>
		So=0x16,
		/// <summary>
		/// �󔒕����������܂��B
		/// </summary>
		Zs=0x17,
		/// <summary>
		/// ���s�����������܂��B
		/// </summary>
		Zl=0x18,
		/// <summary>
		/// �i�������������܂��B
		/// </summary>
		Zp=0x19,
		/// <summary>
		/// ���䕶���������܂��B
		/// </summary>
		Cc=0x1a,
		/// <summary>
		/// �����w�蕶���������܂��B
		/// </summary>
		Cf=0x1b,
		/// <summary>
		/// �T���Q�[�g�������܂��B
		/// </summary>
		Cs=0x1c,
		/// <summary>
		/// ���̑��̕����E���p�����������܂��B
		/// </summary>
		Co=0x1d,
		/// <summary>
		/// �����蓖�ĕ����������܂��B
		/// </summary>
		Cn=0x1e,

		/// <summary>
		/// ���䕶���������܂��B
		/// </summary>
		C=0x81,
		/// <summary>
		/// �ʏ핶���������܂��B
		/// </summary>
		L,
		/// <summary>
		/// �����L���������܂��B
		/// </summary>
		M,
		/// <summary>
		/// �����������܂��B
		/// </summary>
		N,
		/// <summary>
		/// ��Ǔ_�������܂��B
		/// </summary>
		P,
		/// <summary>
		/// �L���������܂��B
		/// </summary>
		S,
		/// <summary>
		/// ��؂蕶���������܂��B
		/// </summary>
		Z
		#endregion
	}

	/// <summary>
	/// �������������Ă���u���b�N�������̂Ɏg�p���܂��B
	/// </summary>
	public enum UnicodeBlock:byte{
		#region UnicodeBlocks
		/// <summary>
		/// ��������u���b�N�����݂��Ȃ����������܂��B
		/// </summary>
		No_Block,
		/// <summary>
		/// ��{���e�������ł��鎖�������܂��B
		/// </summary>
		BasicLatin,
		/// <summary>
		/// ���e�� -1 �⏕�����ł��鎖�������܂��B
		/// </summary>
		Latin1Supplement,
		/// <summary>
		/// ���e���g������ A �ł��鎖�������܂��B
		/// </summary>
		LatinExtendedA,
		/// <summary>
		/// ���e���g������ B �ł��鎖�������܂��B
		/// </summary>
		LatinExtendedB,
		/// <summary>
		/// IPA �g�������ł��鎖�������܂��B
		/// </summary>
		IPAExtensions,
		/// <summary>
		/// �X�y�[�X���������ł��鎖�������܂��B
		/// </summary>
		SpacingModifierLetters,
		/// <summary>
		/// ���������L���ł��鎖�������܂��B
		/// </summary>
		CombiningDiacriticalMarks,
		/// <summary>
		/// �M���V�������y�уR�v�g�ł��邱�Ƃ������܂��B
		/// </summary>
		GreekAndCoptic,
		/// <summary>
		/// �L���������ł��鎖�������܂��B
		/// </summary>
		Cyrillic,
		/// <summary>
		/// �L�����⏕�����ł��鎖�������܂��B
		/// </summary>
		CyrillicSupplement,
		/// <summary>
		/// �A�����j�A�����ł��鎖�������܂��B
		/// </summary>
		Armenian,
		/// <summary>
		/// �w�u���C�����ł��鎖�������܂��B
		/// </summary>
		Hebrew,
		/// <summary>
		/// �A���r�A�����ł��鎖�������܂��B
		/// </summary>
		Arabic,
		/// <summary>
		/// �V���A�����ł��鎖�������܂��B
		/// </summary>
		Syriac,
		/// <summary>
		/// �A���r�A�⏕�����ł��鎖�������܂��B
		/// </summary>
		ArabicSupplement,
		/// <summary>
		/// �^�[�i�����ł��鎖�������܂��B
		/// </summary>
		Thaana,
		/// <summary>
		/// ���R�����ł��鎖�������܂��B
		/// </summary>
		NKo,
		/// <summary>
		/// �f�[���A�i�[�K���[�����ł��鎖�������܂��B
		/// </summary>
		Devanagari,
		/// <summary>
		/// �x���K�������ł��邱�Ƃ������܂��B
		/// </summary>
		Bengali,
		/// <summary>
		/// �O�����L�[�����ł��鎖�������܂��B
		/// </summary>
		Gurmukhi,
		/// <summary>
		/// �O�W�����[�e�C�����ł��鎖�������܂��B
		/// </summary>
		Gujarati,
		/// <summary>
		/// �I�����[�����ł��鎖�������܂��B
		/// </summary>
		Oriya,
		/// <summary>
		/// �^�~�������ł��鎖�������܂��B
		/// </summary>
		Tamil,
		/// <summary>
		/// �e���O�����ł��鎖�������܂��B
		/// </summary>
		Telugu,
		/// <summary>
		/// �J���i�_�����ł��鎖�������܂��B
		/// </summary>
		Kannada,
		/// <summary>
		/// �}�����[���������ł��鎖�������܂��B
		/// </summary>
		Malayalam,
		/// <summary>
		/// �V���n�������ł��鎖�������܂��B
		/// </summary>
		Sinhala,
		/// <summary>
		/// �^�C�����ł��鎖�������܂��B
		/// </summary>
		Thai,
		/// <summary>
		/// ���I�����ł��鎖�������܂��B
		/// </summary>
		Lao,
		/// <summary>
		/// �`�x�b�g�����ł��鎖�������܂��B
		/// </summary>
		Tibetan,
		/// <summary>
		/// �~�����}�[�����ł��鎖�������܂��B
		/// </summary>
		Myanmar,
		/// <summary>
		/// �O���W�A�����ł��鎖�������܂��B
		/// </summary>
		Georgian,
		/// <summary>
		/// �n���O������ł��鎖�������܂��B
		/// </summary>
		HangulJamo,
		/// <summary>
		/// �G�`�I�s�A�����ł��鎖�������܂��B
		/// </summary>
		Ethiopic,
		/// <summary>
		/// �G�`�I�s�A�⏕�����ł��鎖�������܂��B
		/// </summary>
		EthiopicSupplement,
		/// <summary>
		/// �`�F���L�[�����ł��鎖�������܂��B
		/// </summary>
		Cherokee,
		/// <summary>
		/// �����J�i�_���Z�����ߕ����ł��鎖�������܂��B
		/// </summary>
		UnifiedCanadianAboriginalSyllabics,
		/// <summary>
		/// �I�K�������ł��鎖�������܂��B
		/// </summary>
		Ogham,
		/// <summary>
		/// ���[�������ł��鎖�������܂��B
		/// </summary>
		Runic,
		/// <summary>
		/// �^�K���O�����ł��鎖�������܂��B
		/// </summary>
		Tagalog,
		/// <summary>
		/// �n�k�m�I�����ł��鎖�������܂��B
		/// </summary>
		Hanunoo,
		/// <summary>
		/// �u�q�h�����ł��鎖�������܂��B
		/// </summary>
		Buhid,
		/// <summary>
		/// �^�O�o���������ł��鎖�������܂��B
		/// </summary>
		Tagbanwa,
		/// <summary>
		/// �N���[�������ł��鎖�������܂��B
		/// </summary>
		Khmer,
		/// <summary>
		/// �����S�������ł��鎖�������܂��B
		/// </summary>
		Mongolian,
		/// <summary>
		/// �����u�����ł��鎖�������܂��B
		/// </summary>
		Limbu,
		/// <summary>
		/// �^�C�E�������ł��鎖�������܂��B
		/// </summary>
		TaiLe,
		/// <summary>
		/// �V�^�C�E���[�����ł��鎖�������܂��B
		/// </summary>
		NewTaiLue,
		/// <summary>
		/// �N���[���L���ł��邱�Ƃ������܂��B
		/// </summary>
		KhmerSymbols,
		/// <summary>
		/// �u�M�����ł��鎖�������܂��B
		/// </summary>
		Buginese,
		/// <summary>
		/// �o�������ł��鎖�������܂��B
		/// </summary>
		Balinese,
		/// <summary>
		/// �X�[�_�������ł��鎖�������܂��B
		/// </summary>
		Sundanese,
		/// <summary>
		/// ���v�`�������ł��鎖�������܂��B
		/// </summary>
		Lepcha,
		/// <summary>
		/// �I���E�`�L�����ł��鎖�������܂��B
		/// </summary>
		OlChiki,
		/// <summary>
		/// �����g�������ł��鎖�������܂��B
		/// </summary>
		PhoneticExtensions,
		/// <summary>
		/// �����g���⏕�����ł��鎖�������܂��B
		/// </summary>
		PhoneticExtensionsSupplement,
		/// <summary>
		/// ���������⏕�L���ł��鎖�������܂��B
		/// </summary>
		CombiningDiacriticalMarksSupplement,
		/// <summary>
		/// ���e���g���ǉ������ł��鎖�������܂��B
		/// </summary>
		LatinExtendedAdditional,
		/// <summary>
		/// �M���V���g�������ł��鎖�������܂��B
		/// </summary>
		GreekExtended,
		/// <summary>
		/// ��ʋ�Ǔ_�ł��鎖�������܂��B
		/// </summary>
		GeneralPunctuation,
		/// <summary>
		/// ��t�y�щ��t�����ł��鎖�������܂��B
		/// </summary>
		SuperscriptsAndSubscripts,
		/// <summary>
		/// �ʉ݋L���ł��鎖�������܂��B
		/// </summary>
		CurrencySymbols,
		/// <summary>
		/// �L���p�����L���ł��鎖�������܂��B
		/// </summary>
		CombiningDiacriticalMarksForSymbols,
		/// <summary>
		/// �����l�L���ł��鎖�������܂��B
		/// </summary>
		LetterlikeSymbols,
		/// <summary>
		/// �����`�ł��鎖�������܂��B
		/// </summary>
		NumberForms,
		/// <summary>
		/// ���ł��鎖�������܂��B
		/// </summary>
		Arrows,
		/// <summary>
		/// ���w�L���ł��鎖�������܂��B
		/// </summary>
		MathematicalOperators,
		/// <summary>
		/// ���̑��̋Z�p�p�L���ł��鎖�������܂��B
		/// </summary>
		MiscellaneousTechnical,
		/// <summary>
		/// ����@�\�p�L���ł��鎖�������܂��B
		/// </summary>
		ControlPictures,
		/// <summary>
		/// OCR (���w�������F��) �p�L���ł��鎖�������܂��B
		/// </summary>
		OpticalCharacterRecognition,
		/// <summary>
		/// �͂݉p�����ł��鎖�������܂��B
		/// </summary>
		EnclosedAlphanumerics,
		/// <summary>
		/// �r���f�Ђł��鎖�������܂��B
		/// </summary>
		BoxDrawing,
		/// <summary>
		/// �u���b�N�v�f�ł��鎖�������܂��B
		/// </summary>
		BlockElements,
		/// <summary>
		/// �􉽊w�}�`�ł��鎖�������܂��B
		/// </summary>
		GeometricShapes,
		/// <summary>
		/// ���̑��̋L���ł��鎖�������܂��B
		/// </summary>
		MiscellaneousSymbols,
		/// <summary>
		/// �����L���ł��鎖�������܂��B
		/// </summary>
		Dingbats,
		/// <summary>
		/// ���̑��̐��w�L�� A �ł��鎖�������܂��B
		/// </summary>
		MiscellaneousMathematicalSymbolsA,
		/// <summary>
		/// ���⏕ A �ł��鎖�������܂��B
		/// </summary>
		SupplementalArrowsA,
		/// <summary>
		/// �_���ł��鎖�������܂��B
		/// </summary>
		BraillePatterns,
		/// <summary>
		/// ���⏕ B �ł��鎖�������܂��B
		/// </summary>
		SupplementalArrowsB,
		/// <summary>
		/// ���̑��̐��w�L�� B �ł��鎖�������܂��B
		/// </summary>
		MiscellaneousMathematicalSymbolsB,
		/// <summary>
		/// ���w���Z�q�⏕�ł��鎖�������܂��B
		/// </summary>
		SupplementalMathematicalOperators,
		/// <summary>
		/// ���̑��̋L���y�і��ł��鎖�������܂��B
		/// </summary>
		MiscellaneousSymbolsAndArrows,
		/// <summary>
		/// �O���S�[�������ł��鎖�������܂��B
		/// </summary>
		Glagolitic,
		/// <summary>
		/// ���e���g������ C �ł��鎖�������܂��B
		/// </summary>
		LatinExtendedC,
		/// <summary>
		/// �R�v�g�����ł��鎖�������܂��B
		/// </summary>
		Coptic,
		/// <summary>
		/// �O���W�A�⏕�����ł��鎖�������܂��B
		/// </summary>
		GeorgianSupplement,
		/// <summary>
		/// �e�B�t�i�O�����ł��鎖�������܂��B
		/// </summary>
		Tifinagh,
		/// <summary>
		/// �G�`�I�s�A�g�������ł��鎖�������܂��B
		/// </summary>
		EthiopicExtended,
		/// <summary>
		/// �L�����g������ A �ł��鎖�������܂��B
		/// </summary>
		CyrillicExtendedA,
		/// <summary>
		/// �⏕��Ǔ_�ł��鎖�������܂��B
		/// </summary>
		SupplementalPunctuation,
		/// <summary>
		/// CJK �⏕����ł��鎖�������܂��B
		/// </summary>
		CJKRadicalsSupplement,
		/// <summary>
		/// �N�����T����ł��鎖�������܂��B
		/// </summary>
		KangxiRadicals,
		/// <summary>
		/// �����\���ł��鎖�������܂��B
		/// </summary>
		IdeographicDescriptionCharacters,
		/// <summary>
		/// CJK �L���y�ы�Ǔ_�ł��鎖�������܂��B
		/// </summary>
		CJKSymbolsAndPunctuation,
		/// <summary>
		/// �������ł��鎖�������܂��B
		/// </summary>
		Hiragana,
		/// <summary>
		/// �Љ����ł��鎖�������܂��B
		/// </summary>
		Katakana,
		/// <summary>
		/// ��������ł��鎖�������܂��B
		/// </summary>
		Bopomofo,
		/// <summary>
		/// �n���O���݊������ł��鎖�������܂��B
		/// </summary>
		HangulCompatibilityJamo,
		/// <summary>
		/// �����L���ł��鎖�������܂�
		/// </summary>
		Kanbun,
		/// <summary>
		/// ��������ł��鎖�������܂��B
		/// </summary>
		BopomofoExtended,
		/// <summary>
		/// CJK ����ł��鎖�������܂��B
		/// </summary>
		CJKStrokes,
		/// <summary>
		/// �Љ��������g�������ł��鎖�������܂��B
		/// </summary>
		KatakanaPhoneticExtensions,
		/// <summary>
		/// �͂� CJK �����y�ь��L���ł��鎖�������܂��B
		/// </summary>
		EnclosedCJKLettersAndMonths,
		/// <summary>
		/// CJK �݊������ł��鎖�������܂��B
		/// </summary>
		CJKCompatibility,
		/// <summary>
		/// CJK ���������g�� A �ł��鎖�������܂��B
		/// </summary>
		CJKUnifiedIdeographsExtensionA,
		/// <summary>
		/// �Ռo�L���ł��鎖�������܂��B
		/// </summary>
		YijingHexagramSymbols,
		/// <summary>
		/// CJK ���������ł��鎖�������܂��B
		/// </summary>
		CJKUnifiedIdeographs,
		/// <summary>
		/// �C�������߂ł��鎖�������܂��B
		/// </summary>
		YiSyllables,
		/// <summary>
		/// �C��������ł��鎖�������܂��B
		/// </summary>
		YiRadicals,
		/// <summary>
		/// ���@�C�����ł��鎖�������܂��B
		/// </summary>
		Vai,
		/// <summary>
		/// �L�����g������ B �ł��鎖�������܂��B
		/// </summary>
		CyrillicExtendedB,
		/// <summary>
		/// �����L���ł��鎖�������܂��B
		/// </summary>
		ModifierToneLetters,
		/// <summary>
		/// ���e���g������ D �ł��鎖�������܂��B
		/// </summary>
		LatinExtendedD,
		/// <summary>
		/// �V���e�B�E�i�O�������ł��鎖�������܂��B
		/// </summary>
		SylotiNagri,
		/// <summary>
		/// �p�X�p�����ł��鎖�������܂��B
		/// </summary>
		PhagsPa,
		/// <summary>
		/// �T�E���[�V���g�������ł��鎖�������܂��B
		/// </summary>
		Saurashtra,
		/// <summary>
		/// �J���[�����ł��鎖�������܂��B
		/// </summary>
		KayahLi,
		/// <summary>
		/// ���W���������ł��鎖�������܂��B
		/// </summary>
		Rejang,
		/// <summary>
		/// �`���������ł��鎖�������܂��B
		/// </summary>
		Cham,
		/// <summary>
		/// �n���O�����ߕ����ł��鎖�������܂��B
		/// </summary>
		HangulSyllables,
		/// <summary>
		/// ��ʃT���Q�[�g�̈�ł��鎖�������܂��B
		/// </summary>
		HighSurrogates,
		/// <summary>
		/// ��ʎ��p�T���Q�[�g�ł��鎖�������܂��B
		/// </summary>
		HighPrivateUseSurrogates,
		/// <summary>
		/// ���ʃT���Q�[�g�̈�ł��鎖�������܂��B
		/// </summary>
		LowSurrogates,
		/// <summary>
		/// ���p�̈�ł��鎖�������܂��B
		/// </summary>
		PrivateUseArea,
		/// <summary>
		/// CJK �݊������ł��鎖�������܂��B
		/// </summary>
		CJKCompatibilityIdeographs,
		/// <summary>
		/// �A���t�@�x�b�g�\���`�ł��鎖�������܂��B
		/// </summary>
		AlphabeticPresentationForms,
		/// <summary>
		/// �A���r�A�\���` A �ł��鎖�������܂��B
		/// </summary>
		ArabicPresentationFormsA,
		/// <summary>
		/// �ّ̎��I���q�ł��鎖�������܂��B
		/// </summary>
		VariationSelectors,
		/// <summary>
		/// �c�����`�ł��鎖�������܂��B
		/// </summary>
		VerticalForms,
		/// <summary>
		/// �����\���L���ł��鎖�������܂��B
		/// </summary>
		CombiningHalfMarks,
		/// <summary>
		/// CJK �݊��`�ł��鎖�������܂��B
		/// </summary>
		CJKCompatibilityForms,
		/// <summary>
		/// �����`�ł��鎖�������܂��B
		/// </summary>
		SmallFormVariants,
		/// <summary>
		/// �A���r�A�\���` B �ł��鎖�������܂��B
		/// </summary>
		ArabicPresentationFormsB,
		/// <summary>
		/// ���p�E�S�p�`�̕����ł��鎖�������܂��B
		/// </summary>
		HalfwidthAndFullwidthForms,
		/// <summary>
		/// ���ꕶ���ł��鎖�������܂��B
		/// </summary>
		Specials,
		//============================================================
		//		U+10000 �ȍ~
		//============================================================
		/// <summary>
		/// ���^���� B ���ߋL���ł��鎖�������܂��B
		/// </summary>
		LinearBSyllabary,
		/// <summary>
		/// ���^���� B �\�������ł��鎖�������܂��B
		/// </summary>
		LinearBIdeograms,
		/// <summary>
		/// �G�[�Q�����ł��鎖�������܂��B
		/// </summary>
		AegeanNumbers,
		/// <summary>
		/// �Ñ�M���V�������ł��鎖�������܂��B
		/// </summary>
		AncientGreekNumbers,
		/// <summary>
		/// �Ñ�L���ł��鎖�������܂��B
		/// </summary>
		AncientSymbols,
		/// <summary>
		/// �t�@�C�X�g�X�̉~�Ղ̕����ł��鎖�������܂��B
		/// </summary>
		PhaistosDisc,
		/// <summary>
		/// ���L�A�����ł��鎖�������܂��B
		/// </summary>
		Lycian,
		/// <summary>
		/// �J���A�����ł��鎖�������܂��B
		/// </summary>
		Carian,
		/// <summary>
		/// �Ñ�C�^���A�����ł��鎖�������܂��B
		/// </summary>
		OldItalic,
		/// <summary>
		/// �S�[�g�����ł��鎖�������܂��B
		/// </summary>
		Gothic,
		/// <summary>
		/// �E�K���g�����ł��鎖�������܂��B
		/// </summary>
		Ugaritic,
		/// <summary>
		/// �Ñ�y���V�������ł��鎖�������܂��B
		/// </summary>
		OldPersian,
		/// <summary>
		/// �f�U���b�g�����ł��鎖�������܂��B
		/// </summary>
		Deseret,
		/// <summary>
		/// �V�F�C���B�A�������ł��鎖�������܂��B
		/// </summary>
		Shavian,
		/// <summary>
		/// �I�X�}�j�A�����ł��鎖�������܂��B
		/// </summary>
		Osmanya,
		/// <summary>
		/// �L�v���X���ߕ����ł��鎖�������܂��B
		/// </summary>
		CypriotSyllabary,
		/// <summary>
		/// �t�F�j�L�A�����ł��鎖�������܂��B
		/// </summary>
		Phoenician,
		/// <summary>
		/// ���f�B�A�����ł��鎖�������܂��B
		/// </summary>
		Lydian,
		/// <summary>
		/// �J���[�V���e�B�[�����ł��鎖�������܂��B
		/// </summary>
		Kharoshthi,
		/// <summary>
		/// ���`�����ł��鎖�������܂��B
		/// </summary>
		Cuneiform,
		/// <summary>
		/// ���`�����̐����y�ы�Ǔ_�ł��鎖�������܂��B
		/// </summary>
		CuneiformNumbersAndPunctuation,
		/// <summary>
		/// �r�U���`�����y�L���ł��鎖�������܂��B
		/// </summary>
		ByzantineMusicalSymbols,
		/// <summary>
		/// ���y�L���ł��鎖�������܂��B
		/// </summary>
		MusicalSymbols,
		/// <summary>
		/// �Ñ�M���V�������L���ł��鎖�������܂��B
		/// </summary>
		AncientGreekMusicalNotation,
		/// <summary>
		/// �����o�L���ł��鎖�������܂��B
		/// </summary>
		TaiXuanJingSymbols,
		/// <summary>
		/// �Z�ؗp�����ł��鎖�������܂��B
		/// </summary>
		CountingRodNumerals,
		/// <summary>
		/// ���w�p�p�����ł��鎖�������܂��B
		/// </summary>
		MathematicalAlphanumericSymbols,
		/// <summary>
		/// �����v�L���ł��鎖�������܂��B
		/// </summary>
		MahjongTiles,
		/// <summary>
		/// �h�~�m�v�L���ł��鎖�������܂��B
		/// </summary>
		DominoTiles,
		/// <summary>
		/// CJK ���������g�� B �ł��鎖�������܂��B
		/// </summary>
		CJKUnifiedIdeographsExtensionB,
		/// <summary>
		/// CJK �݊������⏕�ł��鎖�������܂��B
		/// </summary>
		CJKCompatibilityIdeographsSupplement,
		/// <summary>
		/// �^�O�ł��鎖�������܂��B
		/// </summary>
		Tags,
		/// <summary>
		/// �ّ̎��I���q�ł��鎖�������܂��B
		/// </summary>
		VariationSelectorsSupplement,
		/// <summary>
		/// ���p�̈�⏕ A �ł��鎖�������܂��B
		/// </summary>
		SupplementaryPrivateUseAreaA,
		/// <summary>
		/// ���p�̈�⏕ B �ł��鎖�������܂��B
		/// </summary>
		SupplementaryPrivateUseAreaB,
		#endregion
	}
	/// <summary>
	/// C ����̕����^�C�v��\�����܂��B
	/// </summary>
	public enum CLangCType:byte{
		/// <summary>
		/// �p�啶����\���܂��B
		/// </summary>
		upper	=0x01,
		/// <summary>
		/// �p��������\���܂��B
		/// </summary>
		lower	=0x02,
		/// <summary>
		/// ���p������\���܂��B
		/// </summary>
		digit	=0x04,
		/// <summary>
		/// �󔒌n���̕����ł��鎖��\���܂��B�󔒂͊܂܂�܂���B
		/// </summary>
		_space	=0x08,
		/// <summary>
		/// ��Ǔ_�ł��鎖��\���܂��B
		/// </summary>
		punct	=0x10,
		/// <summary>
		/// ���䕶���ł��鎖��\���܂��B
		/// </summary>
		cntrl	=0x20,
		/// <summary>
		/// �󔒕�����\���܂��B
		/// </summary>
		blank	=0x40,
		/// <summary>
		/// �\�Z�i�\�L�Ɏg�p����镶���ł��鎖��\���܂��B
		/// </summary>
		xdigit	=0x80,

		/// <summary>
		/// �󔒌n���̕����ł��鎖��₢�܂��B
		/// </summary>
		space=blank|_space,
		/// <summary>
		/// �p���ł��鎖��₢�܂��B
		/// </summary>
		alpha=upper|lower,
		/// <summary>
		/// �p�����͐����ł��鎖��₢�܂��B
		/// </summary>
		alnum=upper|lower|digit,
		/// <summary>
		/// ���`���������ł��鎖��₢�܂��B
		/// </summary>
		graph=punct|alnum,
		/// <summary>
		/// �󎚉\�����ł��鎖��₢�܂��B
		/// </summary>
		print=graph|blank,
		/// <summary>
		/// ASCII �����ł��鎖��₢�܂��B
		/// </summary>
		ascii=graph|cntrl,

		/// <summary>
		/// C ���ʎq�Ɏg�p����镶���ł��鎖��₢�܂��B
		/// </summary>
		csym=0xFF, //
		/// <summary>
		/// C ���ʎq�̐擪�Ɏg�p����镶���ł��鎖��₢�܂��B
		/// </summary>
		csymf=0xFE, //

		/// <summary>
		/// C ���ʎq�Ɏg�p����镶���ł��鎖��₢�܂��B
		/// </summary>
		word=csym,
	}
	/// <summary>
	/// �����Ɋւ��鑀���񋟂��܂��B
	/// </summary>
	public static class CharUtils{
		const string RESOURCE_GENCAT="afh.Regex.GenCatTable.bin";

		static CharUtils(){
			System.IO.Stream str=typeof(CharUtils).Assembly.GetManifestResourceStream(RESOURCE_GENCAT);
			for(int i=0;i<gencat_table.Length;i++){
				gencat_table[i]=(GeneralCategory)str.ReadByte();
			}
			str.Close();

			InitializeCType();
		}
		//============================================================
		//		Unicode Block
		//============================================================
		/// <summary>
		/// �������������Ă��� Unicode Block ���擾���܂��B
		/// </summary>
		/// <param name="c">Block �𒲂ׂ����������w�肵�܂��B</param>
		/// <returns>�w�肵���������������Ă��� Unicode �� Block ��Ԃ��܂��B
		/// �������Ă��� block ���Ȃ��ꍇ�ɂ� UnicodeBlock.No_Block ��Ԃ��܂��B</returns>
		private static UnicodeBlock GetUnicodeBlock(char c){
			byte h=(byte)((int)c>>8);
			int x=0xff&c;

			if(0x34<=h&&h<0x4D)return UnicodeBlock.CJKUnifiedIdeographsExtensionA;
			if(0x4E<=h&&h<=0x9F)return UnicodeBlock.CJKUnifiedIdeographs;
			if(0xAC<=h&&h<0xD7)return UnicodeBlock.HangulSyllables;
			if(0xE0<=h&&h<=0xF8)return UnicodeBlock.PrivateUseArea;

			#region switch(h)
			switch(h){
				case 0x00:
					if(x<0x80)return UnicodeBlock.BasicLatin;
					return UnicodeBlock.Latin1Supplement;
				case 0x01:
					if(x<0x80)return UnicodeBlock.LatinExtendedA;
					return UnicodeBlock.LatinExtendedB;
				case 0x02:
					if(x<0x50)return UnicodeBlock.LatinExtendedB;
					if(x<0xB0)return UnicodeBlock.IPAExtensions;
					return UnicodeBlock.SpacingModifierLetters;
				case 0x03:
					if(x<0x70)return UnicodeBlock.CombiningDiacriticalMarks;
					return UnicodeBlock.GreekAndCoptic;
				case 0x04:
					return UnicodeBlock.Cyrillic;
				case 0x05:
					if(x<0x30)return UnicodeBlock.CyrillicSupplement;
					if(x<0x90)return UnicodeBlock.Armenian;
					return UnicodeBlock.Hebrew;
				case 0x06:
					return UnicodeBlock.Hebrew;
				case 0x07:
					if(x<0x50)return UnicodeBlock.Syriac;
					if(x<0x80)return UnicodeBlock.ArabicSupplement;
					if(x<0xC0)return UnicodeBlock.Thaana;
					return UnicodeBlock.NKo;
				case 0x08:
					return UnicodeBlock.No_Block;
				case 0x09:
					if(x<0x80)return UnicodeBlock.Devanagari;
					return UnicodeBlock.Bengali;
				case 0x0a:
					if(x<0x80)return UnicodeBlock.Gurmukhi;
					return UnicodeBlock.Gujarati;
				case 0x0B:
					if(x<0x80)return UnicodeBlock.Oriya;
					return UnicodeBlock.Tamil;
				case 0x0c:
					if(x<0x80)return UnicodeBlock.Telugu;
					return UnicodeBlock.Kannada;
				case 0x0D:
					if(x<0x80)return UnicodeBlock.Malayalam;
					return UnicodeBlock.Sinhala;
				case 0x0E:
					if(x<0x80)return UnicodeBlock.Thai;
					return UnicodeBlock.Lao;
				case 0x0F:
					return UnicodeBlock.Tibetan;
				case 0x10:
					if(x<0xA0)return UnicodeBlock.Myanmar;
					return UnicodeBlock.Georgian;
				case 0x11:
					return UnicodeBlock.HangulJamo;
				case 0x12:
					return UnicodeBlock.Ethiopic;
				case 0x13:
					if(x<0x80)return UnicodeBlock.Ethiopic;
					if(x<0xA0)return UnicodeBlock.EthiopicSupplement;
					return UnicodeBlock.Cherokee;
				case 0x14:
				case 0x15:
					return UnicodeBlock.UnifiedCanadianAboriginalSyllabics;
				case 0x16:
					if(x<0x80)return UnicodeBlock.UnifiedCanadianAboriginalSyllabics;
					if(x<0xA0)return UnicodeBlock.Ogham;
					return UnicodeBlock.Runic;
				case 0x17:
					if(x<0x20)return UnicodeBlock.Tagalog;
					if(x<0x40)return UnicodeBlock.Hanunoo;
					if(x<0x60)return UnicodeBlock.Buhid;
					if(x<0x80)return UnicodeBlock.Tagbanwa;
					return UnicodeBlock.Khmer;
				case 0x18:
					if(x<0xB0)return UnicodeBlock.Mongolian;
					return UnicodeBlock.No_Block;
				case 0x19:
					if(x<0x50)return UnicodeBlock.Limbu;
					if(x<0x80)return UnicodeBlock.TaiLe;
					if(x<0xE0)return UnicodeBlock.NewTaiLue;
					return UnicodeBlock.KhmerSymbols;
				case 0x1a:
					if(x<0x20)return UnicodeBlock.Buginese;
					return UnicodeBlock.No_Block;
				case 0x1B:
					if(x<0x80)return UnicodeBlock.Balinese;
					if(x<0xC0)return UnicodeBlock.Sundanese;
					return UnicodeBlock.No_Block;
				case 0x1C:
					if(x<0x50)return UnicodeBlock.Lepcha;
					if(x<0x80)return UnicodeBlock.OlChiki;
					return UnicodeBlock.No_Block;
				case 0x1D:
					if(x<0x80)return UnicodeBlock.PhoneticExtensions;
					if(x<0xC0)return UnicodeBlock.PhoneticExtensionsSupplement;
					return UnicodeBlock.CombiningDiacriticalMarksSupplement;
				case 0x1E:
					return UnicodeBlock.LatinExtendedAdditional;
				case 0x1F:
					return UnicodeBlock.GreekExtended;
				case 0x20:
					if(x<0x70)return UnicodeBlock.GeneralPunctuation;
					if(x<0xA0)return UnicodeBlock.SuperscriptsAndSubscripts;
					if(x<0xD0)return UnicodeBlock.CurrencySymbols;
					return UnicodeBlock.CombiningDiacriticalMarksForSymbols;
				case 0x21:
					if(x<0x50)return UnicodeBlock.LetterlikeSymbols;
					if(x<0x90)return UnicodeBlock.NumberForms;
					return UnicodeBlock.Arrows;
				case 0x22:
					return UnicodeBlock.MathematicalOperators;
				case 0x23:
					return UnicodeBlock.MiscellaneousTechnical;
				case 0x24:
					if(x<0x40)return UnicodeBlock.ControlPictures;
					if(x<0x60)return UnicodeBlock.OpticalCharacterRecognition;
					return UnicodeBlock.EnclosedAlphanumerics;
				case 0x25:
					if(x<0x80)return UnicodeBlock.BoxDrawing;
					if(x<0xA0)return UnicodeBlock.BlockElements;
					return UnicodeBlock.GeometricShapes;
				case 0x26:
					return UnicodeBlock.MiscellaneousSymbols;
				case 0x27:
					if(x<0xC0)return UnicodeBlock.Dingbats;
					if(x<0xF0)return UnicodeBlock.MiscellaneousMathematicalSymbolsA;
					return UnicodeBlock.SupplementalArrowsA;
				case 0x28:
					return UnicodeBlock.BraillePatterns;
				case 0x29:
					if(x<0x80)return UnicodeBlock.SupplementalArrowsB;
					return UnicodeBlock.MiscellaneousMathematicalSymbolsB;
				case 0x2A:
					return UnicodeBlock.SupplementalMathematicalOperators;
				case 0x2B:
					return UnicodeBlock.MiscellaneousSymbolsAndArrows;
				case 0x2C:
					if(x<0x60)return UnicodeBlock.Glagolitic;
					if(x<0x80)return UnicodeBlock.LatinExtendedC;
					return UnicodeBlock.Coptic;
				case 0x2D:
					if(x<0x30)return UnicodeBlock.GeorgianSupplement;
					if(x<0x80)return UnicodeBlock.Tifinagh;
					if(x<0xE0)return UnicodeBlock.EthiopicExtended;
					return UnicodeBlock.CyrillicExtendedA;
				case 0x2E:
					if(x<0x80)return UnicodeBlock.SupplementalPunctuation;
					return UnicodeBlock.CJKRadicalsSupplement;
				case 0x2F:
					if(x<0xF0)return UnicodeBlock.KangxiRadicals;
					return UnicodeBlock.IdeographicDescriptionCharacters;
				case 0x30:
					if(x<0x40)return UnicodeBlock.CJKSymbolsAndPunctuation;
					if(x<0xA0)return UnicodeBlock.Hiragana;
					return UnicodeBlock.Katakana;
				case 0x31:
					if(x<0x30)return UnicodeBlock.Bopomofo;
					if(x<0x90)return UnicodeBlock.HangulCompatibilityJamo;
					if(x<0xA0)return UnicodeBlock.Kanbun;
					if(x<0xC0)return UnicodeBlock.BopomofoExtended;
					if(x<0xF0)return UnicodeBlock.CJKStrokes;
					return UnicodeBlock.KatakanaPhoneticExtensions;
				case 0x32:
					return UnicodeBlock.EnclosedCJKLettersAndMonths;
				case 0x33:
					return UnicodeBlock.CJKCompatibility;
				//====================================================
				case 0x4D:
					if(x<0xC0)return UnicodeBlock.CJKUnifiedIdeographsExtensionA;
					return UnicodeBlock.YijingHexagramSymbols;
				//====================================================
				case 0xA0:
				case 0xA1:
				case 0xA2:
				case 0xA3:
					return UnicodeBlock.YiSyllables;
				case 0xA4:
					if(x<0x90)return UnicodeBlock.YiSyllables;
					if(x<0xB0)return UnicodeBlock.YiRadicals;
					return UnicodeBlock.No_Block;
				case 0xA5:
					return UnicodeBlock.Vai;
				case 0xA6:
					if(x<0x40)return UnicodeBlock.Vai;
					if(x<0xA0)return UnicodeBlock.CyrillicExtendedB;
					return UnicodeBlock.No_Block;
				case 0xA7:
					if(x<0x20)return UnicodeBlock.ModifierToneLetters;
					return UnicodeBlock.LatinExtendedD;
				case 0xA8:
					if(x<0x30)return UnicodeBlock.SylotiNagri;
					if(x<0x40)return UnicodeBlock.No_Block;
					if(x<0x80)return UnicodeBlock.PhagsPa;
					if(x<0xE0)return UnicodeBlock.Saurashtra;
					return UnicodeBlock.No_Block;
				case 0xA9:
					if(x<0x30)return UnicodeBlock.KayahLi;
					if(x<0x60)return UnicodeBlock.Rejang;
					return UnicodeBlock.No_Block;
				case 0xAA:
					if(x<0x60)return UnicodeBlock.Cham;
					return UnicodeBlock.No_Block;

				case 0xD7:
					if(x<0xB0)return UnicodeBlock.HangulSyllables;
					return UnicodeBlock.No_Block;
				case 0xD8:
				case 0xD9:
				case 0xDA:
					return UnicodeBlock.HighSurrogates;
				case 0xDB:
					if(x<0x80)return UnicodeBlock.HighSurrogates;
					return UnicodeBlock.HighPrivateUseSurrogates;
				case 0xDC:
				case 0xDE:
				case 0xDF:
					return UnicodeBlock.LowSurrogates;
				case 0xF9:
				case 0xFA:
					return UnicodeBlock.CJKCompatibilityIdeographs;
				case 0xFB:
					if(x<0x50)return UnicodeBlock.AlphabeticPresentationForms;
					return UnicodeBlock.ArabicPresentationFormsA;
				case 0xFC:
				case 0xFD:
					return UnicodeBlock.ArabicPresentationFormsA;
				case 0xFE:
					if(x<0x10)return UnicodeBlock.VariationSelectors;
					if(x<0x20)return UnicodeBlock.VerticalForms;
					if(x<0x30)return UnicodeBlock.CombiningHalfMarks;
					if(x<0x50)return UnicodeBlock.CJKCompatibilityForms;
					if(x<0x70)return UnicodeBlock.SmallFormVariants;
					return UnicodeBlock.ArabicPresentationFormsB;
				case 0xFF:
					if(x<0xF0)return UnicodeBlock.HalfwidthAndFullwidthForms;
					return UnicodeBlock.Specials;
/*
'\x10000'..'\x1007F';LinearBSyllabary,
'\x10080'..'\x100FF';LinearBIdeograms,

'\x10100'..'\x1013F';AegeanNumbers,
'\x10140'..'\x1018F';AncientGreekNumbers,
'\x10190'..'\x101CF';AncientSymbols,
'\x101D0'..'\x101FF';PhaistosDisc,

'\x10280'..'\x1029F';Lycian,
'\x102A0'..'\x102DF';Carian,

'\x10300'..'\x1032F';OldItalic,
'\x10330'..'\x1034F';Gothic,
'\x10380'..'\x1039F';Ugaritic,
'\x103A0'..'\x103DF';OldPersian,

'\x10400'..'\x1044F';Deseret,
'\x10450'..'\x1047F';Shavian,
'\x10480'..'\x104AF';Osmanya,

'\x10800'..'\x1083F';CypriotSyllabary,

'\x10900'..'\x1091F';Phoenician,
'\x10920'..'\x1093F';Lydian,

'\x10A00'..'\x10A5F';Kharoshthi,

'\x12000'..'\x123FF';Cuneiform,

'\x12400'..'\x1247F';CuneiformNumbersAndPunctuation,

'\x1D000'..'\x1D0FF';ByzantineMusicalSymbols,
'\x1D100'..'\x1D1FF';MusicalSymbols,
'\x1D200'..'\x1D24F';AncientGreekMusicalNotation,

'\x1D300'..'\x1D35F';TaiXuanJingSymbols,
'\x1D360'..'\x1D37F';CountingRodNumerals,

'\x1D400'..'\x1D7FF';MathematicalAlphanumericSymbols,

'\x1F000'..'\x1F02F';MahjongTiles,
'\x1F030'..'\x1F09F';DominoTiles,
'\x20000'..'\x2A6DF';CJKUnifiedIdeographsExtensionB,
'\x2F800'..'\x2FA1F';CJKCompatibilityIdeographsSupplement,
'\xE0000'..'\xE007F';Tags,
'\xE0100'..'\xE01EF';VariationSelectorsSupplement,
'\xF0000'..'\xFFFFF';SupplementaryPrivateUseAreaA,
'\x100000'..'\x10FFFF';SupplementaryPrivateUseAreaB,
*/
			}
			#endregion

			return UnicodeBlock.No_Block;
		}
		/// <summary>
		/// ����������� Unicode Block �ɏ������Ă��邩�ۂ��𔻒肵�܂��B
		/// </summary>
		/// <param name="c">����̑Ώۂ̕������w�肵�܂��B</param>
		/// <param name="block">�������������Ă��邩�ǂ����𒲂ׂ� block ���w�肵�܂��B</param>
		/// <returns>�������w�肵�� block �ɏ������Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public static bool Is(char c,UnicodeBlock block){
			return GetUnicodeBlock(c)==block;
		}
		//============================================================
		//		Unicode General Category
		//============================================================
		private static GeneralCategory[] gencat_table=new GeneralCategory[38*0x100];
		/*
		const int NO_TBL=0xFFFF;
		private static int[] gencat_table_L30={
			0x0000,0x0100,0x0200,0x0300,0x0400,0x0500,0x0600,0x0700, // 0x00-0x07
			NO_TBL,0x0800,0x0900,0x0a00,0x0b00,0x0c00,0x0d00,0x0e00, // 0x08-0x0f
			0x0f00,NO_TBL,0x1000,NO_TBL,NO_TBL,NO_TBL,NO_TBL,0x1100, // 0x10-0x17
			NO_TBL,0x1200,NO_TBL,0x1300,0x1400,NO_TBL,NO_TBL,0x1500, // 0x18-0x1f
			0x1600,0x1700,NO_TBL,0x1800,NO_TBL,NO_TBL,NO_TBL,0x1900, // 0x20-0x27
			NO_TBL,0x1a00,NO_TBL,0x1b00,NO_TBL,0x1c00,0x1d00,NO_TBL, // 0x28-0x2f
			0x1e00, // 0x30
		};
		//*/

		/// <summary>
		/// �w�肵�������� Unicode General-Category ���擾���܂��B
		/// </summary>
		/// <param name="c">���ׂ镶�����w�肵�܂��B</param>
		/// <returns>�w�肵�������� GeneralCategory ��Ԃ��܂��B</returns>
		public static GeneralCategory GetGeneralCategory(char c){
			const GeneralCategory Ll=GeneralCategory.Ll;
			const GeneralCategory Lu=GeneralCategory.Lu;
			const GeneralCategory Lo=GeneralCategory.Lo;
			//const GeneralCategory Lt=GeneralCategory.Lt;
			const GeneralCategory Lm=GeneralCategory.Lm;
			const GeneralCategory Cc=GeneralCategory.Cc;
			//const GeneralCategory Cf=GeneralCategory.Cf;
			const GeneralCategory Cn=GeneralCategory.Cn;
			const GeneralCategory Cs=GeneralCategory.Cs;
			const GeneralCategory Co=GeneralCategory.Co;
			const GeneralCategory Mn=GeneralCategory.Mn;
			//const GeneralCategory Me=GeneralCategory.Me;
			const GeneralCategory Mc=GeneralCategory.Mc;
			const GeneralCategory Po=GeneralCategory.Po;
			const GeneralCategory Pd=GeneralCategory.Pd;
			const GeneralCategory Ps=GeneralCategory.Ps;
			const GeneralCategory Pe=GeneralCategory.Pe;
			//const GeneralCategory Pc=GeneralCategory.Pc;
			//const GeneralCategory Pi=GeneralCategory.Pi;
			//const GeneralCategory Pf=GeneralCategory.Pf;
			const GeneralCategory Sc=GeneralCategory.Sc;
			const GeneralCategory So=GeneralCategory.So;
			const GeneralCategory Sm=GeneralCategory.Sm;
			//const GeneralCategory Sk=GeneralCategory.Sk;
			const GeneralCategory No=GeneralCategory.No;
			const GeneralCategory Nd=GeneralCategory.Nd;
			const GeneralCategory Nl=GeneralCategory.Nl;
			const GeneralCategory Zs=GeneralCategory.Zs;
			//const GeneralCategory Zl=GeneralCategory.Zl;
			//const GeneralCategory Zp=GeneralCategory.Zp;
		//------------------------------------------------------------
			if(c<='\x07FF')return gencat_table[(int)c];

			byte h=(byte)((int)c>>8);
			int x=0xff&c;
			if(0x34<=h&&h<=0x9E&&h!=0x4d)return Lo; // CJK
			if(0xAC<=h&&h<=0xD6)return Lo; // Hangul
			if(0xD8<=h&&h<=0xDF)return Cs; // Surrogate
			if(0xE0<=h&&h<=0xF8)return Co; // Private

			#region switch(h)
			switch(h){
			//	case 0x00: - case 0x07:goto table;
				case 0x08:
					return Cn;
				case 0x09:case 0x0a:case 0x0b:
				case 0x0c:case 0x0d:case 0x0e:
				case 0x0f:case 0x10:
					h--;
					goto table; // 16
				case 0x11:
					if(x<=0x59||0x5F<=x&&x<=0xA2||0xA8<=x&&x<=0xF9)return Lo;
					return Cn;
				case 0x12:h=16;goto table; // 17
				case 0x13:
					if(x<=0x10||0x12<=x&&x<=0x15||0x18<=x&&x<=0x5A||0x80<=x&&x<=0x8F||0xA0<=x&&x<=0xF4)return Lo;
					if(x==0x5F)return Mn;
					if(x==0x60||0x90<=x&&x<=0x99)return So;
					if(0x61<=x&&x<=0x68)return Po;
					if(0x69<=x&&x<=0x7C)return No;
					return Cn;
				case 0x14:
					if(x==0x00)return Cc;
					return Lo;
				case 0x15:
					return Lo;
				case 0x16:
					if(x<=0x6C||0x6F<=x&&x<=0x76||0x81<=x&&x<=0x9A||0xA0<=x&&x<=0xEA) return Lo;
					if(x==0x6D||x==0x6E||0xEB<=x&&x<=0xED)return Po;
					if(0xEE<=x&&x<=0xF0)return Nl;
					if(x==0x80)return Zs;
					if(x==0x9B)return Ps;
					if(x==0x9C)return Pe;
					return Cn;
				case 0x17:h=17;goto table; // 18
				case 0x18:
					if(x<=0x05||0x07<=x&&x<=0x0A)return Po;
					if(x==0x06)return Pd;
					if(0x0B<=x&&x<=0x0D||x==0xA9)return Mn;
					if(x==0x0E)return Zs;
					if(0x10<=x&&x<=0x19)return Nd;
					if(0x20<=x&&x<=0x42||0x44<=x&&x<=0x77||0x80<=x&&x<=0xA8||x==0xAA)return Lo;
					if(x==0x43)return Lm;
					return Cn;
				case 0x19:h=18;goto table; // 19
				case 0x1a:
					if(x<=0x16)return Lo;
					if(x==0x17||x==0x18)return Mn;
					if(0x19<=x&&x<=0x1B)return Mc;
					if(x==0x1E||x==0x1F)return Po;
					return Cn;
				case 0x1B:h=19;goto table; // 20
				case 0x1C:h=20;goto table; // 21
				case 0x1D:
					if(0x00<=x&&x<=0x2B||0x62<=x&&x<=0x77||0x79<=x&&x<=0x9A)return Ll;
					if(0x2C<=x&&x<=0x61||x==0x78||0x9B<=x&&x<=0xBF)return Lm;
					if(0xC0<=x&&x<=0xE6||x==0xFE||x==0xFF)return Mn;
					return Cn;
				case 0x1E:
					if((x&1)==0){
						if(0x00<=x&&x<=0x94||0x9E<=x&&x<=0xFE)return Lu;
						if(0x96<=x&&x<=0x9C)return Ll;
					}else{
						if(0x01<=x&&x<=0xFF)return Ll;
					}
					return Cn;
				case 0x1F:h=21;goto table; // 22
				case 0x20:h=22;goto table; // 23
				case 0x21:h=23;goto table; // 24
				case 0x22:
					return Sm;
				case 0x23:h=24;goto table; // 25
				case 0x24:
					if(x<=0x26||0x40<=x&&x<=0x4A||0x9C<=x&&x<=0xE9)return So;
					if(0x60<=x&&x<=0x9B||0xEA<=x&&x<=0xFF)return No;
					return Cn;
				case 0x25:
					if(x<=0xB6||0xB8<=x&&x<=0xC0||0xC2<=x&&x<=0xF7)return So;
					if(x==0xB7||x==0xC1||0xF8<=x)return Sm;
					return Cn;
				case 0x26:
					if(x<=0x6E||0x70<=x&&x<=0x9D||0xA0<=x&&x<=0xBC||0xC0<=x&&x<=0xC3)return So;
					if(x==0x6F)return Sm;
					return Cn;
				case 0x27:h=25;goto table; // 26
				case 0x28:
					return So;
				case 0x29:h=26;goto table; // 27
				case 0x2A:
					return Sm;
				case 0x2B:
					if(x<=0x2F||x==0x45||x==0x46||0x50<=x&&x<=0x54)return So;
					if(0x30<=x&&x<=0x44||0x47<=x&&x<=0x4C)return Sm;
					return Cn;
				case 0x2C:h=27;goto table; // 28
				case 0x2D:h=28;goto table; // 29
				case 0x2E:h=29;goto table; // 30
				case 0x2F:
					if(x<=0xD5||0xF0<=x&&x<=0xFB)return So;
					return Cn;
				case 0x30:h=30;goto table; // 31
				case 0x31:
					if(0x05<=x&&x<=0x2D||0x31<=x&&x<=0x8E||0xA0<=x&&x<=0xB7||0xF0<=x)return Lo;
					if(x==0x90||x==0x91||0x96<=x&&x<=0x9F||0xC0<=x&&x<=0xE3)return So;
					if(0x92<=x&&x<=0x95)return No;
					return Cn;
				case 0x32:
					if(x<=0x1E||0x2A<=x&&x<=0x43||x==0x50||0x60<=x&&x<=0x7F||0x8A<=x&&x<=0xB0||0xC0<=x&&x!=0xFF)return So;
					if(0x20<=x&&x<=0x29||0x51<=x&&x<=0x5F||0x80<=x&&x<=0x89||0xB1<=x&&x<=0xBF)return No;
					return Cn;
				case 0x33:
					return So;
			//========================================================
			//	case 0x34: - case 0x4c:
				case 0x4d:
					if(x<=0xB5)return Lo;
					if(0xC0<=x)return So;
					return Cn;
			//	case 0x4e: - case 0x9E:
				case 0x9F:
					if(x<=0xC3)return Lo;
					return Cn;
			//========================================================
				case 0xA0:
					if(x==0x15)return Lm;
					return Lo;
				case 0xA1:
					return Lo;
				case 0xA2:
					return Lo;
				case 0xA3:
					return Lo;
				case 0xA4:
					if(x<=0x8C)return Lo;
					if(0x90<=x&&x<=0xC6)return So;
					return Cn;
				case 0xA5:
					return Lo;
				case 0xA6:h=31;goto table; // 32
				case 0xA7:h=32;goto table; // 33
				case 0xA8:h=33;goto table; // 34
				case 0xA9:
					if(x<=0x09)return Nd;
					if(0x0A<=x&&x<=0x25||0x30<=x&&x<=0x46)return Lo;
					if(0x26<=x&&x<=0x2D||0x47<=x&&x<=0x51)return Mn;
					if(x==0x2E||x==0x2F||x==0x5F)return Po;
					if(x==0x52||x==0x53)return Mc;
					return Cn;
				case 0xAA:h=34;goto table; // 35
				case 0xAB:
					return Cn;
			//--Hangul
			//	case 0xAC: - case 0xD6
				case 0xD7:
					if(x<=0xA3)return Lo;
					return Cn;
			//--Surrogate
			//	case 0xD8: - case 0xDF
			//--Private
			//	case 0xE0: - case 0xF8
			//========================================================
				case 0xF9:
					return Lo;
				case 0xFA:
					if(x<=0x2D||0x30<=x&&x<=0x6A||0x70<=x&&x<=0xD9)return Lo;
					return Cn;
				case 0xFB:h=35;goto table; // 36
				case 0xFC:
					return Lo;
				case 0xFD:
					if(x<=0x3D||0x50<=x&&x<=0x8F||0x92<=x&&x<=0xC7||0xF0<=x&&x<=0xFB)return Lo;
					if(x==0x3E)return Ps;
					if(x==0x3F)return Pe;
					if(x==0xFC)return Sc;
					if(x==0xFD)return So;
					return Cn;
				case 0xFE:h=36;goto table; // 37
				case 0xFF:h=37;goto table; // 38
				table:
					return gencat_table[h<<8|x];
			}
			#endregion

			return GeneralCategory.Cn;
		}
		/// <summary>
		/// �w�肵���������w�肵���J�e�S���ɓ����Ă��邩�ۂ��𔻒肵�܂��B
		/// </summary>
		/// <param name="c">���ׂ�Ώۂ̕������w�肵�܂��B</param>
		/// <param name="cat">�w�肵�������������Ă��邩�m���߂����J�e�S�����w�肵�܂��B</param>
		/// <returns>�w�肵���������w�肵���J�e�S���ɓ����Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public static bool Is(char c,GeneralCategory cat){
			return Is(GetGeneralCategory(c),cat);
		}
		/// <summary>
		/// �w�肵���J�e�S�����A�ʂ̃J�e�S���Ɠ����������͂���Ɋ܂܂�Ă��邩�𔻒肵�܂��B
		/// </summary>
		/// <param name="c">���ׂ�Ώۂ̃J�e�S�����w�肵�܂��B</param>
		/// <param name="cat">�������Ɠ����������������܂މ\���̂���J�e�S�����w�肵�܂��B</param>
		/// <returns>�w�肵���J�e�S�����A�������Ɏw�肵���J�e�S���Ɠ�����������Ɋ܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public static bool Is(GeneralCategory c,GeneralCategory cat){
			if(c==cat)return true;
			if((0x80&(int)cat)==0)return false;

			switch(cat){
				case GeneralCategory.C:
					return GeneralCategory.Cc<=c&&c<=GeneralCategory.Cn;
				case GeneralCategory.L:
					return GeneralCategory.Lu<=c&&c<=GeneralCategory.Lo;
				case GeneralCategory.M:
					return GeneralCategory.Mn<=c&&c<=GeneralCategory.Me;
				case GeneralCategory.N:
					return GeneralCategory.Nd<=c&&c<=GeneralCategory.No;
				case GeneralCategory.P:
					return GeneralCategory.Pc<=c&&c<=GeneralCategory.Po;
				case GeneralCategory.S:
					return GeneralCategory.Sm<=c&&c<=GeneralCategory.So;
				case GeneralCategory.Z:
					return GeneralCategory.Zs<=c&&c<=GeneralCategory.Zp;
				default:
					return false;
			}
		}
		//============================================================
		//		C Language <ctype.h>
		//============================================================
		private static CLangCType[] ctypes;
		static void InitializeCType(){
			const CLangCType C=CLangCType.cntrl;
			const CLangCType L=CLangCType.lower;
			const CLangCType U=CLangCType.upper;
			const CLangCType P=CLangCType.punct;

			const CLangCType N=CLangCType.digit|CLangCType.xdigit;
			const CLangCType X=CLangCType.upper|CLangCType.xdigit;
			const CLangCType x=CLangCType.lower|CLangCType.xdigit;
			const CLangCType S=CLangCType.cntrl|CLangCType._space;
			const CLangCType B=CLangCType.cntrl|CLangCType.blank;

			ctypes=new CLangCType[]{
				C,C,C,C,C,C,C,C, //0x0*
				C,S,S,S,S,S,C,C,
				C,C,C,C,C,C,C,C, //0x1*
				C,C,C,C,C,C,C,C,
				B,P,P,P,P,P,P,P, //0x2*
				P,P,P,P,P,P,P,P,
				N,N,N,N,N,N,N,N, //0x3*
				N,N,P,P,P,P,P,P,
				P,X,X,X,X,X,X,U, //0x4*
				U,U,U,U,U,U,U,U,
				U,U,U,U,U,U,U,U, // 0x5*
				U,U,U,P,P,P,P,P,
				P,x,x,x,x,x,x,L, // 0x6*
				L,L,L,L,L,L,L,L,
				L,L,L,L,L,L,L,L, // 0x7*
				L,L,L,P,P,P,P,C,
			};
		}
		/// <summary>
		/// �������w�肵�� C ���� ctype �ɑ����Ă��邩�ǂ����𔻒肵�܂��B
		/// </summary>
		/// <param name="c">����̑Ώۂ̕������w�肵�܂��B</param>
		/// <param name="ctype">�����̎�ނ��w�肵�܂��B</param>
		/// <returns>
		/// �������w�肵�� C ���� ctype �ɑ����Ă���ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B
		/// (������ ASCII �����ȊO�̕����ɑ΂��Ă͏�� false ��Ԃ��܂��B)
		/// </returns>
		public static bool Is(char c,CLangCType ctype){
			if(c>'\x7F')return false;

			// csym/csymf
			if(ctype==CLangCType.csym){
				if(c=='_')return true;
				ctype=CLangCType.alnum;
			}else if(ctype==CLangCType.csymf){
				if(c=='_')return true;
				ctype=CLangCType.alpha;
			}

			return (int)(ctypes[c]&ctype)!=0;
		}
	}
}