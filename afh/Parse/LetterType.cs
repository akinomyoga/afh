namespace afh.Parse{
	/// <summary>
	/// 文字の種類を表現する為の構造体です。
	/// </summary>
	public struct LetterType{
		/// <summary>
		/// 文字の予想される使用目的を示す数値を保持します。
		/// </summary>
		public byte purpose;
		/// <summary>
		/// その文字を使用している文化圏を示す数値を保持します。
		/// </summary>
		public byte culture;
		/// <summary>
		/// 文字の種類を表す LetterType のインスタンスを作成します。
		/// </summary>
		/// <param name="purpose">文字の予想される使用目的を指定します。</param>
		/// <param name="culture">その文字を使用している文化圏を指定します。</param>
		public LetterType(byte purpose,byte culture){
			this.purpose=purpose;
			this.culture=culture;
		}
		/// <summary>
		/// この文字の予想される使用目的が識別子である事を表す数値です。
		/// </summary>
		public const byte P_Token=1;
		/// <summary>
		/// この文字の予想される使用目的が演算子である事を表す数値です。
		/// </summary>
		public const byte P_Operator=2;
		/// <summary>
		/// この文字の予想される使用目的が空白である事を表す数値です。
		/// </summary>
		public const byte P_Space=3;
		/// <summary>
		/// この文字の予想される使用目的が数値である事を表す数値です。
		/// </summary>
		public const byte P_Number=4;
		/// <summary>
		/// P_Token に対して使用します。
		/// 文字がローマ字系統の物である事を示します。
		/// </summary>
		public const byte C_Alphabet=1;
		/// <summary>
		/// P_Token に対して使用します。
		/// 文字が漢字である事を示します。。
		/// </summary>
		public const byte C_Hanzi=2;
		/// <summary>
		/// P_Token に対して使用します。
		/// 文字が仮名である事を示します。
		/// </summary>
		public const byte C_Kana=3;
		/// <summary>
		/// P_Token に対して使用します。
		/// 文字がハングルである事を示します。
		/// </summary>
		public const byte C_Hangul=4;
		/// <summary>
		/// P_Token に対して使用します。
		/// 文字がアラビア文字である事を示します。
		/// </summary>
		public const byte C_Arabic=5;
		/// <summary>
		/// P_Space に対して使用します。
		/// 文字が終端文字 (改行など) である事を示します。
		/// </summary>
		public const byte C_Terminator=1;
		/// <summary>
		/// 無効な文字である事を示します。
		/// </summary>
		public static readonly LetterType Invalid=new LetterType(0,0);
		/// <summary>
		/// 識別子などに使用する為の文字である事を示します。
		/// </summary>
		public static readonly LetterType Token=new LetterType(P_Token,0);
		/// <summary>
		/// 演算子などの記号に使用する為の文字である事を示します。
		/// </summary>
		public static readonly LetterType Operator=new LetterType(P_Operator,0);
		/// <summary>
		/// 空白などの文字として使用する為の文字である事を示します。
		/// </summary>
		public static readonly LetterType Space=new LetterType(P_Space,0);
		/// <summary>
		/// 数字として使用する為の文字である事を示します。
		/// </summary>
		public static readonly LetterType Number=new LetterType(P_Number,0);
		/// <summary>
		/// アルファベットである事を示します。
		/// </summary>
		public static readonly LetterType Alphabet=new LetterType(P_Token,C_Alphabet);
		/// <summary>
		/// 漢字である事を示します。
		/// </summary>
		public static readonly LetterType Hanzi=new LetterType(P_Token,C_Hanzi);
		/// <summary>
		/// 仮名である事を示します。
		/// </summary>
		public static readonly LetterType Kana=new LetterType(P_Token,C_Kana);
		/// <summary>
		/// ハングルである事を示します。
		/// </summary>
		public static readonly LetterType Hangul=new LetterType(P_Token,C_Hangul);
		/// <summary>
		/// アラビア文字である事を示します。
		/// </summary>
		public static readonly LetterType Arabic=new LetterType(P_Token,C_Arabic);
		/// <summary>
		/// 終端文字である事を示します。
		/// </summary>
		public static readonly LetterType Terminator=new LetterType(P_Space,C_Terminator);
		/// <summary>このインスタンスの表す文字種が「無効な文字」であるかどうかを取得します。</summary>
		public bool IsInvalid{get{return this.purpose==0;}}
		/// <summary>このインスタンスの表す文字種が「識別子用」であるかどうかを取得します。</summary>
		public bool IsToken{get{return this.purpose==P_Token;}}
		/// <summary>このインスタンスの表す文字種が「演算子」であるかどうかを取得します。</summary>
		public bool IsOperator{get{return this.purpose==P_Operator;}}
		/// <summary>このインスタンスの表す文字種が「空白類」であるかどうかを取得します。</summary>
		public bool IsSpace{get{return this.purpose==P_Space;}}
		/// <summary>このインスタンスの表す文字種が「数字」であるかどうかを取得します。</summary>
		public bool IsNumber{get{return this.purpose==P_Number;}}
		/// <summary>このインスタンスの表す文字種が「終端文字」であるかどうかを取得します。</summary>
		public bool IsTerminator{get{return this.purpose==P_Space&&this.culture==C_Terminator;}}
		/// <summary>
		/// 文字の種類を実際の文字から取得します。
		/// </summary>
		/// <param name="x">種類を調べたい文字を指定します。</param>
		/// <returns>指定した文字の種類を LetterType 構造体で返します。</returns>
		public static LetterType GetLetterType(char x){
			int x0=(int)x>>8;
			int x1=(int)x&0xff;
			switch(x0) {
				case 0x00:
					// 0x7E 迄
					if(0x41<=x1&&x1<=0x5a||0x61<=x1&&x1<=0x7a)return Alphabet;
					if(x1==0x0d||x1==0x0a)return Terminator;
					if(x1<=0x20)return Space;
					if(0x30<=x1&&x1<=0x39)return Number;
					if(x1<=0x7e)return Operator;

					//0x7F 以降
					if(0xc0<=x1) return x1==0xd7||x1==0xf7?Operator:Alphabet;
					if(x1==0x7f||x1==0x80||x1==0x81||x1==0x85||x1==0x8d||x1==0x8e||x1==0x8f||x1==0x90||x1==0x95||x1==0x9d||x1==0x9e||x1==0xa0) {
						return Space;
					}
					return Operator;
				case 0x01:
					return 0xf1<=x1&&x1<=0xfa&&x1!=0xf5?Invalid:Alphabet;
				case 0x02:
					if(0x50<=x1&&x1<=0xa8) return Alphabet;
					if(0xb0<=x1&&x1<=0xe9&&x1!=0xdf) return Token;
					return Invalid;
				case 0x03:
					if(0x86<=x1&&x1<=0xd6)
						return x1!=0x87&&x1!=0x8b&&x1!=0x8d&&x1!=0xcf&&x1!=0xcf?Alphabet:Invalid;
					if(0xe2<=x1&&x1<=0xf3||x1==0xda||x1==0xdc||x1==0xde||x1==0xf0)
						return Alphabet;
					if(x1<=0x4e) return Operator;
					if(0x60<=x1&&x1<=0x85)
						if(x1==0x60||x1==0x61||x1==0x62||x1==0x74||x1==0x75||x1==0x7a||x1==0x7e||x1==0x84||x1==0x85)
							return Operator;
					return Invalid;
				case 0x04:
					if(x1<=0x81&&x1!=0x00&&x1!=0x0d&&x1!=0x50&&x1!=0x5d
						||0x90<=x1&&x1<=0xcc&&x1!=0xc5&&x1!=0xc6&&x1!=0xc9&&x1!=0xca
						||0xd0<=x1&&x1<=0xf9&&x1!=0xec&&x1!=0xed&&x1!=0xf6&&x1!=0xf7)
						return Alphabet;
					return 0x82<=x1&&x1<=0x86?Operator:Invalid;
				// 0x1E** 以降
				case 0x1e:
					return x1<=0x9b||0xa0<=x1&&x1<=0xf9?Alphabet:Invalid;
				case 0x1f:
					if(x1<=0xbc) {
						if(x1!=0x16&&x1!=0x17&&x1!=0x1e&&x1!=0x1f
							&&x1!=0x46&&x1!=0x47&&x1!=0x4e&&x1!=0x4f
							&&x1!=0x58&&x1!=0x5a&&x1!=0x5c&&x1!=0x5e
							&&x1!=0x7e&&x1!=0x7f&&x1!=0xb5)
							return Alphabet;
						return Invalid;
					} else {
						if(0xd<=(x1&0xf)||x1==0xc0||x1==0xc1)
							return Operator;
						if(x1!=0xc5&&x1!=0xd4&&x1!=0xd5&&x1!=0xdc&&x1!=0xf0&&x1!=0xf1&&x1!=0xf5)
							return Alphabet;
						return Invalid;
					}
				case 0x20:
					if(0xa0<=x1&&x1<=0xac)
						return Alphabet;
					if(x1==0x28||x1==0x29)return Terminator;
					if(0x10<=x1&&x1<=0x46||x1==0x70||0x74<=x1&&x1<=0x8e||0xd0<=x1&&x1<=0xe1)
						return Operator;
					return Invalid;
				case 0x21:
					if(x1<=0x38) return Alphabet;
					if(0x53<=x1&&x1<=0x82) return Number;
					if(0x90<=x1&&x1<=0xea) return Operator;
					return Invalid;
				case 0x22:
					return x1<=0xf1?Operator:Invalid;
				case 0x23:
					return x1<=0x7a?Operator:
						x1<=0x7a?Alphabet:
						x1==0x95?Operator:Invalid;
				case 0x24:
					if(x1<=0x24||0x60<=x1&&x1<=0xea) return Token;
					if(0x40<=x1&&x1<=0x4a) return Operator;
					return Invalid;
				case 0x25:
					return x1<=0x95||0xa0<=x1&&x1<=0xef?Operator:Invalid;
				case 0x26:
					return x1<=0x13||0x1a<=x1&&x1<=0x6f?Operator:Invalid;
				case 0x27:
					if(x1<=0x52&&x!=0x00&&x1!=0x05&&x1!=0x0a&&x1!=0x0b&&x1!=0x28&&x1!=0x4c&&x1!=0x4e
						||0x56<=x1&&x1<=0x67&&x1!=0x57&&x1!=0x5f&&x1!=0x60
						||x1==0x94||0x98<=x1&&x1<=0xbe&&x1!=0xb0)
						return Operator;
					if(0x76<=x1&&x1<=0x93) return Token;
					return Invalid;
				case 0x28:
				case 0x29:
				case 0x2a:
				case 0x2b:
				case 0x2c:
				case 0x2d: return Invalid;
				case 0x2e:
					if(x1<0x81||0xca<x1) return Invalid;
					if(x1==0x81||x1==0x84||x1==0x88||x1==0x8b||x1==0x8c
						||x1==0xa7||x1==0xaa||x1==0xae
						||x1==0xb3||x1==0xb6||x1==0xb7||x1==0xbb||x1==0xca)
						return Hanzi;
					return Invalid;
				case 0x2f: return 0xf0<=x1&&x1<=0xfb?Hanzi:Invalid;
				case 0x30:
					if(x1==0x00) return Space;
					if(x1<=0x1f&&x1!=0x05&&x1!=0x06&&x1!=0x12||0x2a<=x1&&x1<=0x30) return Operator;
					if(x1<=0x37) return Token;
					if(0x41<=x1&&x1<=0x94||0x99<=x1&&x1<=0x9e||0xa1<=x1&&x1<=0xfe) return Kana;
					return Invalid;
				case 0x31:
					if(0x05<=x1&&x1<=0x2c) return Hanzi;
					if(0x31<=x1&&x1<=0x8e&&x1!=64) return Hangul;
					if((x1&0xf0)==0x90) return Token;
					return Invalid;
				case 0x32:
					if(x1<=0x1c||0x20<=x1&&x1<=0x43||0x60<=x1&&x1<=0x7b
						||0x7f<=x1&&x1<=0xb0||0xc0<=x1&&x1<=0xcb||0xd0<=x1&&x1<=0xfe)
						return Token;
					return Invalid;
				case 0x33:
					if(x1<=0x57||0x71<=x1&&x1<=0x76||0x80<=x1&&x1<=0xdd)
						return Operator;
					if(x1<=0x70||0x7b<=x1&&x1<=0x7f||0xe0<=x1&&x1<=0xfe)
						return Token;
					return Invalid;
				case 0x34: return x1==0x47||x1==0x73?Hanzi:Invalid;
				case 0x35: return x1==0x9e?Hanzi:Invalid;
				case 0x36: return x1==0x0e||x1==0x1a?Hanzi:Invalid;
				case 0x39: return x1==0x18||x1==0x6e||x1==0xcf||x1==0xd0||x1==0xdf?Hanzi:Invalid;
				case 0x3a: return x1==0x73?Hanzi:Invalid;
				case 0x3b: return x1==0x4e?Hanzi:Invalid;
				case 0x3c: return x1==0x6e||x1==0xe0?Hanzi:Invalid;
				case 0x40: return x1==0x56?Hanzi:Invalid;
				case 0x41: return x1==0x5f?Hanzi:Invalid;
				case 0x43: return x1==0x37||x1==0xac||x1==0xb1||x1==0xdd?Hanzi:Invalid;
				case 0x44: return x1==0xd6?Hanzi:Invalid;
				case 0x46: return x1==0x4c||x1==0x61?Hanzi:Invalid;
				case 0x47: return x1==0x23||x1==0x29||x1==0x7c||x1==0x8d?Hanzi:Invalid;
				case 0x49: return x1==0x47||x1==0x7a||x1==0x7d||x1==0x82
					||x1==0x83||x1==0x85||x1==0x86||x1==0x9b
					||x1==0x9f||x1==0xb6||x1==0xb7?Hanzi:Invalid;
				case 0x4c: return x1==0x77||x1==0x9f||x1==0xa1||x1==0xa2
					||x1==0xa3||x1==0xad?Hanzi:Invalid;
				case 0x4d: return 0x13<=x1&&x1<=0x19||x1==0xae?Hanzi:Invalid;
				case 0x37:
				case 0x38:
				case 0x3d:
				case 0x3e:
				case 0x3f:
				case 0x42:
				case 0x45:
				case 0x48:
				case 0x4a:
				case 0x4b:
					return Invalid;
				//-------------
				//	0xF9** 以降
				//-------------
				case 0xf9: return Hanzi;
				case 0xfa: return x1<=0x2d?Hanzi:Invalid;
				case 0xfb:
					if(x1<=0x06||0x13<=x1&&x1<=0x17)
						return Alphabet;
					if(0x50<=x1&&x1<=0xb1||0xd3<=x1)
						return Arabic;
					if(0x1d<=x1&&x1<=0x4f&&x1!=0x37&&x1!=0x3d&&x1!=0x3f&&x1!=0x42&&x1!=0x45)
						return Token;
					return Invalid;
				case 0xfc: return Arabic;
				case 0xfd://-3d,50-8f,92-c7,f0-f8
					if(x1<=0x3d||0x50<=x1&&x1<=0x8f||0x92<=x1&&x1<=0xc7||0xf0<=x1&&x1<=0xf8)
						return Arabic;
					return x1==0x3e||x1==0x3f?Operator:Invalid;
				case 0xfe:
					if(0x20<=x1&&x1<=0x23||0x30<=x1&&x1<=0x44||0x49<=x1&&x1<=0x6b&&x1!=0x53&&x1!=0x67)
						return Operator;
					if(0x70<=x1&&x1<=0xfc&&x1!=0x73&&x1!=0x75)
						return Arabic;
					return Invalid;
				case 0xff:
					if(x1<=0x64) {
						if(0x10<=x1&&x1<=0x19)
							return Number;
						if(0x21<=x1&&x1<=0x3a||0x41<=x1&&x1<=0x5a)
							return Alphabet;
						return x1!=0x00&&x1!=0x5f&&x1!=0x60?Operator:Invalid;
					}
					if(0x66<=x1&&x1<=0x9f)
						return Kana;
					if(0xa1<=x1&&x1<=0xbe||0xc2<=x1&&x1<=0xdc&&x1!=0xc8&&x1!=0xc9&&x1!=0xd0&&x1!=0xd1&&x1!=0xd8&&x1!=0xd9)
						return Hangul;
					if(0xe0<=x1&&x1<=0xee&&x1!=0xe7||x1==0xfc||x1==0xfd)
						return Operator;
					return Invalid;
				// 他の物
				default:
					//0x4e--0xf8
					if(0x4e<=x0) {
						// 0x4e≦x0≦0x9e
						if(x0<=0x9e) return Hanzi;
						// x0＝0x9f
						if(x0==0x9f) return x1<=0xa5?Hanzi:Invalid;
						// 0xa0≦x0≦0xab
						if(x0<=0xab) return Invalid;
						// 0xac≦x0≦0xd6
						if(x0<=0xd6) return Hangul;
						// x0＝0xd7
						if(x0==0xd7) return x1<=0xa3?Hangul:Invalid;
						// 0xd8≦x0≦0xf8 私用領域
						return Invalid;
					}
					return Invalid;
			}
		}
	}
}