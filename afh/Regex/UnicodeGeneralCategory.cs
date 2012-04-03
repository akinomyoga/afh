namespace afh.Text{
	/// <summary>
	/// Unicode の一般カテゴリを示すのに使用します。
	/// </summary>
	public enum GeneralCategory:byte{
		#region GeneralCategory
		/// <summary>
		/// 大文字を示します。
		/// </summary>
		Lu=0x01,
		/// <summary>
		/// 小文字を示します。
		/// </summary>
		Ll=0x02,
		/// <summary>
		/// タイトル文字を示します。
		/// </summary>
		Lt=0x03,
		/// <summary>
		/// 装飾文字を示します。
		/// </summary>
		Lm=0x04,
		/// <summary>
		/// その他の文字を示します。
		/// </summary>
		Lo=0x05,
		/// <summary>
		/// 零幅結合文字を示します。
		/// </summary>
		Mn=0x06,
		/// <summary>
		/// 結合文字を示します。
		/// </summary>
		Mc=0x07,
		/// <summary>
		/// 囲み結合文字を示します。
		/// </summary>
		Me=0x08,
		/// <summary>
		/// 十進数字を示します。
		/// </summary>
		Nd=0x09,
		/// <summary>
		/// 数字文字を示します。
		/// </summary>
		Nl=0x0a,
		/// <summary>
		/// その他の数字を示します。
		/// </summary>
		No=0x0b,

		/// <summary>
		/// 接続句読点を示します。
		/// </summary>
		Pc=0x0c,
		/// <summary>
		/// ダッシュを示します。
		/// </summary>
		Pd=0x0d,
		/// <summary>
		/// 始まりの括弧を示します。
		/// </summary>
		Ps=0x0e,
		/// <summary>
		/// 終わりの括弧を示します。
		/// </summary>
		Pe=0x0f,
		/// <summary>
		/// 始まりの引用符を示します。
		/// </summary>
		Pi=0x10,
		/// <summary>
		/// 終わりの引用符を示します。
		/// </summary>
		Pf=0x11,
		/// <summary>
		/// その他の句読点を示します。
		/// </summary>
		Po=0x12,
		/// <summary>
		/// 数学記号を示します。
		/// </summary>
		Sm=0x13,
		/// <summary>
		/// 通貨記号を示します。
		/// </summary>
		Sc=0x14,
		/// <summary>
		/// 修飾文字を示します。
		/// </summary>
		Sk=0x15,
		/// <summary>
		/// その他の記号を示します。
		/// </summary>
		So=0x16,
		/// <summary>
		/// 空白文字を示します。
		/// </summary>
		Zs=0x17,
		/// <summary>
		/// 改行文字を示します。
		/// </summary>
		Zl=0x18,
		/// <summary>
		/// 段落文字を示します。
		/// </summary>
		Zp=0x19,
		/// <summary>
		/// 制御文字を示します。
		/// </summary>
		Cc=0x1a,
		/// <summary>
		/// 書式指定文字を示します。
		/// </summary>
		Cf=0x1b,
		/// <summary>
		/// サロゲートを示します。
		/// </summary>
		Cs=0x1c,
		/// <summary>
		/// その他の文字・私用文字を示します。
		/// </summary>
		Co=0x1d,
		/// <summary>
		/// 未割り当て文字を示します。
		/// </summary>
		Cn=0x1e,

		/// <summary>
		/// 制御文字を示します。
		/// </summary>
		C=0x81,
		/// <summary>
		/// 通常文字を示します。
		/// </summary>
		L,
		/// <summary>
		/// 分音記号を示します。
		/// </summary>
		M,
		/// <summary>
		/// 数字を示します。
		/// </summary>
		N,
		/// <summary>
		/// 句読点を示します。
		/// </summary>
		P,
		/// <summary>
		/// 記号を示します。
		/// </summary>
		S,
		/// <summary>
		/// 区切り文字を示します。
		/// </summary>
		Z
		#endregion
	}

	/// <summary>
	/// 文字が所属しているブロックを示すのに使用します。
	/// </summary>
	public enum UnicodeBlock:byte{
		#region UnicodeBlocks
		/// <summary>
		/// 所属するブロックが存在しない事を示します。
		/// </summary>
		No_Block,
		/// <summary>
		/// 基本ラテン文字である事を示します。
		/// </summary>
		BasicLatin,
		/// <summary>
		/// ラテン -1 補助文字である事を示します。
		/// </summary>
		Latin1Supplement,
		/// <summary>
		/// ラテン拡張文字 A である事を示します。
		/// </summary>
		LatinExtendedA,
		/// <summary>
		/// ラテン拡張文字 B である事を示します。
		/// </summary>
		LatinExtendedB,
		/// <summary>
		/// IPA 拡張文字である事を示します。
		/// </summary>
		IPAExtensions,
		/// <summary>
		/// スペース調整文字である事を示します。
		/// </summary>
		SpacingModifierLetters,
		/// <summary>
		/// 結合分音記号である事を示します。
		/// </summary>
		CombiningDiacriticalMarks,
		/// <summary>
		/// ギリシャ文字及びコプトであることを示します。
		/// </summary>
		GreekAndCoptic,
		/// <summary>
		/// キリル文字である事を示します。
		/// </summary>
		Cyrillic,
		/// <summary>
		/// キリル補助文字である事を示します。
		/// </summary>
		CyrillicSupplement,
		/// <summary>
		/// アルメニア文字である事を示します。
		/// </summary>
		Armenian,
		/// <summary>
		/// ヘブライ文字である事を示します。
		/// </summary>
		Hebrew,
		/// <summary>
		/// アラビア文字である事を示します。
		/// </summary>
		Arabic,
		/// <summary>
		/// シリア文字である事を示します。
		/// </summary>
		Syriac,
		/// <summary>
		/// アラビア補助文字である事を示します。
		/// </summary>
		ArabicSupplement,
		/// <summary>
		/// ターナ文字である事を示します。
		/// </summary>
		Thaana,
		/// <summary>
		/// ンコ文字である事を示します。
		/// </summary>
		NKo,
		/// <summary>
		/// デーヴアナーガリー文字である事を示します。
		/// </summary>
		Devanagari,
		/// <summary>
		/// ベンガル文字であることを示します。
		/// </summary>
		Bengali,
		/// <summary>
		/// グルムキー文字である事を示します。
		/// </summary>
		Gurmukhi,
		/// <summary>
		/// グジャラーテイ文字である事を示します。
		/// </summary>
		Gujarati,
		/// <summary>
		/// オリヤー文字である事を示します。
		/// </summary>
		Oriya,
		/// <summary>
		/// タミル文字である事を示します。
		/// </summary>
		Tamil,
		/// <summary>
		/// テルグ文字である事を示します。
		/// </summary>
		Telugu,
		/// <summary>
		/// カンナダ文字である事を示します。
		/// </summary>
		Kannada,
		/// <summary>
		/// マラヤーラム文字である事を示します。
		/// </summary>
		Malayalam,
		/// <summary>
		/// シンハラ文字である事を示します。
		/// </summary>
		Sinhala,
		/// <summary>
		/// タイ文字である事を示します。
		/// </summary>
		Thai,
		/// <summary>
		/// ラオ文字である事を示します。
		/// </summary>
		Lao,
		/// <summary>
		/// チベット文字である事を示します。
		/// </summary>
		Tibetan,
		/// <summary>
		/// ミャンマー文字である事を示します。
		/// </summary>
		Myanmar,
		/// <summary>
		/// グルジア文字である事を示します。
		/// </summary>
		Georgian,
		/// <summary>
		/// ハングル字母である事を示します。
		/// </summary>
		HangulJamo,
		/// <summary>
		/// エチオピア文字である事を示します。
		/// </summary>
		Ethiopic,
		/// <summary>
		/// エチオピア補助文字である事を示します。
		/// </summary>
		EthiopicSupplement,
		/// <summary>
		/// チェロキー文字である事を示します。
		/// </summary>
		Cherokee,
		/// <summary>
		/// 統合カナダ原住民音節文字である事を示します。
		/// </summary>
		UnifiedCanadianAboriginalSyllabics,
		/// <summary>
		/// オガム文字である事を示します。
		/// </summary>
		Ogham,
		/// <summary>
		/// ルーン文字である事を示します。
		/// </summary>
		Runic,
		/// <summary>
		/// タガログ文字である事を示します。
		/// </summary>
		Tagalog,
		/// <summary>
		/// ハヌノオ文字である事を示します。
		/// </summary>
		Hanunoo,
		/// <summary>
		/// ブヒド文字である事を示します。
		/// </summary>
		Buhid,
		/// <summary>
		/// タグバンワ文字である事を示します。
		/// </summary>
		Tagbanwa,
		/// <summary>
		/// クメール文字である事を示します。
		/// </summary>
		Khmer,
		/// <summary>
		/// モンゴル文字である事を示します。
		/// </summary>
		Mongolian,
		/// <summary>
		/// リンブ文字である事を示します。
		/// </summary>
		Limbu,
		/// <summary>
		/// タイ・レ文字である事を示します。
		/// </summary>
		TaiLe,
		/// <summary>
		/// 新タイ・ルー文字である事を示します。
		/// </summary>
		NewTaiLue,
		/// <summary>
		/// クメール記号であることを示します。
		/// </summary>
		KhmerSymbols,
		/// <summary>
		/// ブギ文字である事を示します。
		/// </summary>
		Buginese,
		/// <summary>
		/// バリ文字である事を示します。
		/// </summary>
		Balinese,
		/// <summary>
		/// スーダン文字である事を示します。
		/// </summary>
		Sundanese,
		/// <summary>
		/// レプチャ文字である事を示します。
		/// </summary>
		Lepcha,
		/// <summary>
		/// オル・チキ文字である事を示します。
		/// </summary>
		OlChiki,
		/// <summary>
		/// 発音拡張文字である事を示します。
		/// </summary>
		PhoneticExtensions,
		/// <summary>
		/// 発音拡張補助文字である事を示します。
		/// </summary>
		PhoneticExtensionsSupplement,
		/// <summary>
		/// 結合分音補助記号である事を示します。
		/// </summary>
		CombiningDiacriticalMarksSupplement,
		/// <summary>
		/// ラテン拡張追加文字である事を示します。
		/// </summary>
		LatinExtendedAdditional,
		/// <summary>
		/// ギリシャ拡張文字である事を示します。
		/// </summary>
		GreekExtended,
		/// <summary>
		/// 一般句読点である事を示します。
		/// </summary>
		GeneralPunctuation,
		/// <summary>
		/// 上付及び下付文字である事を示します。
		/// </summary>
		SuperscriptsAndSubscripts,
		/// <summary>
		/// 通貨記号である事を示します。
		/// </summary>
		CurrencySymbols,
		/// <summary>
		/// 記号用分音記号である事を示します。
		/// </summary>
		CombiningDiacriticalMarksForSymbols,
		/// <summary>
		/// 文字様記号である事を示します。
		/// </summary>
		LetterlikeSymbols,
		/// <summary>
		/// 数字形である事を示します。
		/// </summary>
		NumberForms,
		/// <summary>
		/// 矢印である事を示します。
		/// </summary>
		Arrows,
		/// <summary>
		/// 数学記号である事を示します。
		/// </summary>
		MathematicalOperators,
		/// <summary>
		/// その他の技術用記号である事を示します。
		/// </summary>
		MiscellaneousTechnical,
		/// <summary>
		/// 制御機能用記号である事を示します。
		/// </summary>
		ControlPictures,
		/// <summary>
		/// OCR (光学式文字認識) 用記号である事を示します。
		/// </summary>
		OpticalCharacterRecognition,
		/// <summary>
		/// 囲み英数字である事を示します。
		/// </summary>
		EnclosedAlphanumerics,
		/// <summary>
		/// 罫線素片である事を示します。
		/// </summary>
		BoxDrawing,
		/// <summary>
		/// ブロック要素である事を示します。
		/// </summary>
		BlockElements,
		/// <summary>
		/// 幾何学図形である事を示します。
		/// </summary>
		GeometricShapes,
		/// <summary>
		/// その他の記号である事を示します。
		/// </summary>
		MiscellaneousSymbols,
		/// <summary>
		/// 装飾記号である事を示します。
		/// </summary>
		Dingbats,
		/// <summary>
		/// その他の数学記号 A である事を示します。
		/// </summary>
		MiscellaneousMathematicalSymbolsA,
		/// <summary>
		/// 矢印補助 A である事を示します。
		/// </summary>
		SupplementalArrowsA,
		/// <summary>
		/// 点字である事を示します。
		/// </summary>
		BraillePatterns,
		/// <summary>
		/// 矢印補助 B である事を示します。
		/// </summary>
		SupplementalArrowsB,
		/// <summary>
		/// その他の数学記号 B である事を示します。
		/// </summary>
		MiscellaneousMathematicalSymbolsB,
		/// <summary>
		/// 数学演算子補助である事を示します。
		/// </summary>
		SupplementalMathematicalOperators,
		/// <summary>
		/// その他の記号及び矢印である事を示します。
		/// </summary>
		MiscellaneousSymbolsAndArrows,
		/// <summary>
		/// グラゴール文字である事を示します。
		/// </summary>
		Glagolitic,
		/// <summary>
		/// ラテン拡張文字 C である事を示します。
		/// </summary>
		LatinExtendedC,
		/// <summary>
		/// コプト文字である事を示します。
		/// </summary>
		Coptic,
		/// <summary>
		/// グルジア補助文字である事を示します。
		/// </summary>
		GeorgianSupplement,
		/// <summary>
		/// ティフナグ文字である事を示します。
		/// </summary>
		Tifinagh,
		/// <summary>
		/// エチオピア拡張文字である事を示します。
		/// </summary>
		EthiopicExtended,
		/// <summary>
		/// キリル拡張文字 A である事を示します。
		/// </summary>
		CyrillicExtendedA,
		/// <summary>
		/// 補助句読点である事を示します。
		/// </summary>
		SupplementalPunctuation,
		/// <summary>
		/// CJK 補助部首である事を示します。
		/// </summary>
		CJKRadicalsSupplement,
		/// <summary>
		/// 康煕字典部首である事を示します。
		/// </summary>
		KangxiRadicals,
		/// <summary>
		/// 漢字構成である事を示します。
		/// </summary>
		IdeographicDescriptionCharacters,
		/// <summary>
		/// CJK 記号及び句読点である事を示します。
		/// </summary>
		CJKSymbolsAndPunctuation,
		/// <summary>
		/// 平仮名である事を示します。
		/// </summary>
		Hiragana,
		/// <summary>
		/// 片仮名である事を示します。
		/// </summary>
		Katakana,
		/// <summary>
		/// 注音字母である事を示します。
		/// </summary>
		Bopomofo,
		/// <summary>
		/// ハングル互換字音である事を示します。
		/// </summary>
		HangulCompatibilityJamo,
		/// <summary>
		/// 漢文記号である事を示します
		/// </summary>
		Kanbun,
		/// <summary>
		/// 注音字母である事を示します。
		/// </summary>
		BopomofoExtended,
		/// <summary>
		/// CJK 字画である事を示します。
		/// </summary>
		CJKStrokes,
		/// <summary>
		/// 片仮名発音拡張文字である事を示します。
		/// </summary>
		KatakanaPhoneticExtensions,
		/// <summary>
		/// 囲み CJK 文字及び月記号である事を示します。
		/// </summary>
		EnclosedCJKLettersAndMonths,
		/// <summary>
		/// CJK 互換文字である事を示します。
		/// </summary>
		CJKCompatibility,
		/// <summary>
		/// CJK 統合漢字拡張 A である事を示します。
		/// </summary>
		CJKUnifiedIdeographsExtensionA,
		/// <summary>
		/// 易経記号である事を示します。
		/// </summary>
		YijingHexagramSymbols,
		/// <summary>
		/// CJK 統合漢字である事を示します。
		/// </summary>
		CJKUnifiedIdeographs,
		/// <summary>
		/// イ文字音節である事を示します。
		/// </summary>
		YiSyllables,
		/// <summary>
		/// イ文字部首である事を示します。
		/// </summary>
		YiRadicals,
		/// <summary>
		/// ヴァイ文字である事を示します。
		/// </summary>
		Vai,
		/// <summary>
		/// キリル拡張文字 B である事を示します。
		/// </summary>
		CyrillicExtendedB,
		/// <summary>
		/// 声調記号である事を示します。
		/// </summary>
		ModifierToneLetters,
		/// <summary>
		/// ラテン拡張文字 D である事を示します。
		/// </summary>
		LatinExtendedD,
		/// <summary>
		/// シロティ・ナグリ文字である事を示します。
		/// </summary>
		SylotiNagri,
		/// <summary>
		/// パスパ文字である事を示します。
		/// </summary>
		PhagsPa,
		/// <summary>
		/// サウラーシュトラ文字である事を示します。
		/// </summary>
		Saurashtra,
		/// <summary>
		/// カヤー文字である事を示します。
		/// </summary>
		KayahLi,
		/// <summary>
		/// ルジャン文字である事を示します。
		/// </summary>
		Rejang,
		/// <summary>
		/// チャム文字である事を示します。
		/// </summary>
		Cham,
		/// <summary>
		/// ハングル音節文字である事を示します。
		/// </summary>
		HangulSyllables,
		/// <summary>
		/// 上位サロゲート領域である事を示します。
		/// </summary>
		HighSurrogates,
		/// <summary>
		/// 上位私用サロゲートである事を示します。
		/// </summary>
		HighPrivateUseSurrogates,
		/// <summary>
		/// 下位サロゲート領域である事を示します。
		/// </summary>
		LowSurrogates,
		/// <summary>
		/// 私用領域である事を示します。
		/// </summary>
		PrivateUseArea,
		/// <summary>
		/// CJK 互換漢字である事を示します。
		/// </summary>
		CJKCompatibilityIdeographs,
		/// <summary>
		/// アルファベット表示形である事を示します。
		/// </summary>
		AlphabeticPresentationForms,
		/// <summary>
		/// アラビア表示形 A である事を示します。
		/// </summary>
		ArabicPresentationFormsA,
		/// <summary>
		/// 異体字選択子である事を示します。
		/// </summary>
		VariationSelectors,
		/// <summary>
		/// 縦書き形である事を示します。
		/// </summary>
		VerticalForms,
		/// <summary>
		/// 合成可能半記号である事を示します。
		/// </summary>
		CombiningHalfMarks,
		/// <summary>
		/// CJK 互換形である事を示します。
		/// </summary>
		CJKCompatibilityForms,
		/// <summary>
		/// 小字形である事を示します。
		/// </summary>
		SmallFormVariants,
		/// <summary>
		/// アラビア表示形 B である事を示します。
		/// </summary>
		ArabicPresentationFormsB,
		/// <summary>
		/// 半角・全角形の文字である事を示します。
		/// </summary>
		HalfwidthAndFullwidthForms,
		/// <summary>
		/// 特殊文字である事を示します。
		/// </summary>
		Specials,
		//============================================================
		//		U+10000 以降
		//============================================================
		/// <summary>
		/// 線型文字 B 音節記号である事を示します。
		/// </summary>
		LinearBSyllabary,
		/// <summary>
		/// 線型文字 B 表示文字である事を示します。
		/// </summary>
		LinearBIdeograms,
		/// <summary>
		/// エーゲ数字である事を示します。
		/// </summary>
		AegeanNumbers,
		/// <summary>
		/// 古代ギリシャ数字である事を示します。
		/// </summary>
		AncientGreekNumbers,
		/// <summary>
		/// 古代記号である事を示します。
		/// </summary>
		AncientSymbols,
		/// <summary>
		/// ファイストスの円盤の文字である事を示します。
		/// </summary>
		PhaistosDisc,
		/// <summary>
		/// リキア文字である事を示します。
		/// </summary>
		Lycian,
		/// <summary>
		/// カリア文字である事を示します。
		/// </summary>
		Carian,
		/// <summary>
		/// 古代イタリア文字である事を示します。
		/// </summary>
		OldItalic,
		/// <summary>
		/// ゴート文字である事を示します。
		/// </summary>
		Gothic,
		/// <summary>
		/// ウガリト文字である事を示します。
		/// </summary>
		Ugaritic,
		/// <summary>
		/// 古代ペルシャ文字である事を示します。
		/// </summary>
		OldPersian,
		/// <summary>
		/// デザレット文字である事を示します。
		/// </summary>
		Deseret,
		/// <summary>
		/// シェイヴィアン文字である事を示します。
		/// </summary>
		Shavian,
		/// <summary>
		/// オスマニア文字である事を示します。
		/// </summary>
		Osmanya,
		/// <summary>
		/// キプロス音節文字である事を示します。
		/// </summary>
		CypriotSyllabary,
		/// <summary>
		/// フェニキア文字である事を示します。
		/// </summary>
		Phoenician,
		/// <summary>
		/// リディア文字である事を示します。
		/// </summary>
		Lydian,
		/// <summary>
		/// カローシュティー文字である事を示します。
		/// </summary>
		Kharoshthi,
		/// <summary>
		/// 楔形文字である事を示します。
		/// </summary>
		Cuneiform,
		/// <summary>
		/// 楔形文字の数字及び句読点である事を示します。
		/// </summary>
		CuneiformNumbersAndPunctuation,
		/// <summary>
		/// ビザンチン音楽記号である事を示します。
		/// </summary>
		ByzantineMusicalSymbols,
		/// <summary>
		/// 音楽記号である事を示します。
		/// </summary>
		MusicalSymbols,
		/// <summary>
		/// 古代ギリシャ音符記号である事を示します。
		/// </summary>
		AncientGreekMusicalNotation,
		/// <summary>
		/// 太玄経記号である事を示します。
		/// </summary>
		TaiXuanJingSymbols,
		/// <summary>
		/// 算木用数字である事を示します。
		/// </summary>
		CountingRodNumerals,
		/// <summary>
		/// 数学用英数字である事を示します。
		/// </summary>
		MathematicalAlphanumericSymbols,
		/// <summary>
		/// 麻雀牌記号である事を示します。
		/// </summary>
		MahjongTiles,
		/// <summary>
		/// ドミノ牌記号である事を示します。
		/// </summary>
		DominoTiles,
		/// <summary>
		/// CJK 統合漢字拡張 B である事を示します。
		/// </summary>
		CJKUnifiedIdeographsExtensionB,
		/// <summary>
		/// CJK 互換漢字補助である事を示します。
		/// </summary>
		CJKCompatibilityIdeographsSupplement,
		/// <summary>
		/// タグである事を示します。
		/// </summary>
		Tags,
		/// <summary>
		/// 異体字選択子である事を示します。
		/// </summary>
		VariationSelectorsSupplement,
		/// <summary>
		/// 私用領域補助 A である事を示します。
		/// </summary>
		SupplementaryPrivateUseAreaA,
		/// <summary>
		/// 私用領域補助 B である事を示します。
		/// </summary>
		SupplementaryPrivateUseAreaB,
		#endregion
	}
	/// <summary>
	/// C 言語の文字タイプを表現します。
	/// </summary>
	public enum CLangCType:byte{
		/// <summary>
		/// 英大文字を表します。
		/// </summary>
		upper	=0x01,
		/// <summary>
		/// 英小文字を表します。
		/// </summary>
		lower	=0x02,
		/// <summary>
		/// 半角数字を表します。
		/// </summary>
		digit	=0x04,
		/// <summary>
		/// 空白系統の文字である事を表します。空白は含まれません。
		/// </summary>
		_space	=0x08,
		/// <summary>
		/// 句読点である事を表します。
		/// </summary>
		punct	=0x10,
		/// <summary>
		/// 制御文字である事を表します。
		/// </summary>
		cntrl	=0x20,
		/// <summary>
		/// 空白文字を表します。
		/// </summary>
		blank	=0x40,
		/// <summary>
		/// 十六進表記に使用される文字である事を表します。
		/// </summary>
		xdigit	=0x80,

		/// <summary>
		/// 空白系統の文字である事を問います。
		/// </summary>
		space=blank|_space,
		/// <summary>
		/// 英字である事を問います。
		/// </summary>
		alpha=upper|lower,
		/// <summary>
		/// 英字又は数字である事を問います。
		/// </summary>
		alnum=upper|lower|digit,
		/// <summary>
		/// 字形を持つ文字である事を問います。
		/// </summary>
		graph=punct|alnum,
		/// <summary>
		/// 印字可能文字である事を問います。
		/// </summary>
		print=graph|blank,
		/// <summary>
		/// ASCII 文字である事を問います。
		/// </summary>
		ascii=graph|cntrl,

		/// <summary>
		/// C 識別子に使用される文字である事を問います。
		/// </summary>
		csym=0xFF, //
		/// <summary>
		/// C 識別子の先頭に使用される文字である事を問います。
		/// </summary>
		csymf=0xFE, //

		/// <summary>
		/// C 識別子に使用される文字である事を問います。
		/// </summary>
		word=csym,
	}
	/// <summary>
	/// 文字に関する操作を提供します。
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
		/// 文字が所属している Unicode Block を取得します。
		/// </summary>
		/// <param name="c">Block を調べたい文字を指定します。</param>
		/// <returns>指定した文字が所属している Unicode の Block を返します。
		/// 所属している block がない場合には UnicodeBlock.No_Block を返します。</returns>
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
		/// 文字が特定の Unicode Block に所属しているか否かを判定します。
		/// </summary>
		/// <param name="c">判定の対象の文字を指定します。</param>
		/// <param name="block">文字が所属しているかどうかを調べる block を指定します。</param>
		/// <returns>文字が指定した block に所属していた場合に true を返します。
		/// それ以外の場合に false を返します。</returns>
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
		/// 指定した文字の Unicode General-Category を取得します。
		/// </summary>
		/// <param name="c">調べる文字を指定します。</param>
		/// <returns>指定した文字の GeneralCategory を返します。</returns>
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
		/// 指定した文字が指定したカテゴリに入っているか否かを判定します。
		/// </summary>
		/// <param name="c">調べる対象の文字を指定します。</param>
		/// <param name="cat">指定した文字が入っているか確かめたいカテゴリを指定します。</param>
		/// <returns>指定した文字が指定したカテゴリに入っていた場合に true を返します。
		/// それ以外の場合に false を返します。</returns>
		public static bool Is(char c,GeneralCategory cat){
			return Is(GetGeneralCategory(c),cat);
		}
		/// <summary>
		/// 指定したカテゴリが、別のカテゴリと等しいか又はそれに含まれているかを判定します。
		/// </summary>
		/// <param name="c">調べる対象のカテゴリを指定します。</param>
		/// <param name="cat">第一引数と等しいか第一引数を含む可能性のあるカテゴリを指定します。</param>
		/// <returns>指定したカテゴリが、第二引数に指定したカテゴリと等しいかそれに含まれていた場合に true を返します。
		/// それ以外の場合に false を返します。</returns>
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
		/// 文字が指定した C 言語 ctype に属しているかどうかを判定します。
		/// </summary>
		/// <param name="c">判定の対象の文字を指定します。</param>
		/// <param name="ctype">文字の種類を指定します。</param>
		/// <returns>
		/// 文字が指定した C 言語 ctype に属している場合に true を返します。
		/// それ以外の場合には false を返します。
		/// (文字が ASCII 文字以外の文字に対しては常に false を返します。)
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