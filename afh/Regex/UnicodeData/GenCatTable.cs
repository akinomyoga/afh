// GeneralCategory.dat ���� GenCatTable.bin �𐶐����܂��B

namespace afh.Text{
	public enum GeneralCategory:byte{
		Lu=0x01,Ll=0x02,Lt=0x03,Lm=0x04,Lo=0x05,
		Mn=0x06,Mc=0x07,Me=0x08,
		Nd=0x09,Nl=0x0a,No=0x0b,

		Pc=0x0c,Pd=0x0d,Ps=0x0e,Pe=0x0f,Pi=0x10,Pf=0x11,Po=0x12,
		Sm=0x13,Sc=0x14,Sk=0x15,So=0x16,
		Zs=0x17,Zl=0x18,Zp=0x19,
		Cc=0x1a,Cf=0x1b,Cs=0x1c,Co=0x1d,Cn=0x1e,
	}
	public static class Unicode{
		const GeneralCategory Ll=GeneralCategory.Ll;
		const GeneralCategory Lu=GeneralCategory.Lu;
		const GeneralCategory Lo=GeneralCategory.Lo;
		const GeneralCategory Lt=GeneralCategory.Lt;
		const GeneralCategory Lm=GeneralCategory.Lm;
		const GeneralCategory Cc=GeneralCategory.Cc;
		const GeneralCategory Cf=GeneralCategory.Cf;
		const GeneralCategory Cn=GeneralCategory.Cn;
		const GeneralCategory Mn=GeneralCategory.Mn;
		const GeneralCategory Me=GeneralCategory.Me;
		const GeneralCategory Mc=GeneralCategory.Mc;
		const GeneralCategory Po=GeneralCategory.Po;
		const GeneralCategory Pd=GeneralCategory.Pd;
		const GeneralCategory Ps=GeneralCategory.Ps;
		const GeneralCategory Pe=GeneralCategory.Pe;
		const GeneralCategory Pc=GeneralCategory.Pc;
		const GeneralCategory Pi=GeneralCategory.Pi;
		const GeneralCategory Pf=GeneralCategory.Pf;
		const GeneralCategory Sc=GeneralCategory.Sc;
		const GeneralCategory So=GeneralCategory.So;
		const GeneralCategory Sm=GeneralCategory.Sm;
		const GeneralCategory Sk=GeneralCategory.Sk;
		const GeneralCategory No=GeneralCategory.No;
		const GeneralCategory Nd=GeneralCategory.Nd;
		const GeneralCategory Nl=GeneralCategory.Nl;
		const GeneralCategory Zs=GeneralCategory.Zs;
		const GeneralCategory Zl=GeneralCategory.Zl;
		const GeneralCategory Zp=GeneralCategory.Zp;

		private static GeneralCategory[] arr_gencat=new GeneralCategory[]{
			//==== Charcode-Range 0000 - 00FF ====
			Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,
			Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,
			Zs,Po,Po,Po,Sc,Po,Po,Po,Ps,Pe,Po,Sm,Po,Pd,Po,Po,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Sm,Sm,Sm,Po,
			Po,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Ps,Po,Pe,Sk,Pc,
			Sk,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ps,Sm,Pe,Sm,Cc,
			Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,
			Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,Cc,
			Zs,Po,Sc,Sc,Sc,Sc,So,So,Sk,So,Ll,Pi,Sm,Cf,So,Sk,
			So,Sm,No,No,Sk,Ll,So,Po,Sk,No,Ll,Pf,No,No,No,Po,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Sm,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Sm,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			//==== Charcode-Range 0100 - 01FF ====
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,
			Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Lu,Ll,Lu,Ll,Lu,Ll,Ll,
			Ll,Lu,Lu,Ll,Lu,Ll,Lu,Lu,Ll,Lu,Lu,Lu,Ll,Ll,Lu,Lu,
			Lu,Lu,Ll,Lu,Lu,Ll,Lu,Lu,Lu,Ll,Ll,Ll,Lu,Lu,Ll,Lu,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Lu,Ll,Lu,Ll,Ll,Lu,Ll,Lu,Lu,
			Ll,Lu,Lu,Lu,Ll,Lu,Ll,Lu,Lu,Ll,Ll,Lo,Lu,Ll,Ll,Ll,
			Lo,Lo,Lo,Lo,Lu,Lt,Ll,Lu,Lt,Ll,Lu,Lt,Ll,Lu,Ll,Lu,
			Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Ll,Lu,Lt,Ll,Lu,Ll,Lu,Lu,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			//==== Charcode-Range 0200 - 02FF ====
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Ll,Lu,Lu,Ll,
			Ll,Lu,Ll,Lu,Lu,Lu,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Lo,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,
			Lm,Lm,Sk,Sk,Sk,Sk,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,
			Lm,Lm,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,
			Lm,Lm,Lm,Lm,Lm,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Lm,Sk,Lm,Sk,
			Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,
			//==== Charcode-Range 0300 - 03FF ====
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Lu,Ll,Lu,Ll,Lm,Sk,Lu,Ll,Cn,Cn,Lm,Ll,Ll,Ll,Po,Cn,
			Cn,Cn,Cn,Cn,Sk,Sk,Lu,Po,Lu,Lu,Lu,Cn,Lu,Cn,Lu,Lu,
			Ll,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Cn,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,
			Ll,Ll,Lu,Lu,Lu,Ll,Ll,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Ll,Ll,Ll,Ll,Lu,Ll,Sm,Lu,Ll,Lu,Lu,Ll,Ll,Lu,Lu,Lu,
			//==== Charcode-Range 0400 - 04FF ====
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,So,Mn,Mn,Mn,Mn,Mn,Me,Me,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			//==== Charcode-Range 0500 - 05FF ====
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Cn,Cn,Lm,Po,Po,Po,Po,Po,Po,
			Cn,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Cn,Po,Pd,Cn,Cn,Cn,Cn,Cn,
			Cn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Pd,Mn,
			Po,Mn,Mn,Po,Mn,Mn,Po,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Po,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0600 - 06FF ====
			Cf,Cf,Cf,Cf,Cn,Cn,Sm,Sm,Sm,Po,Po,Sc,Po,Po,So,So,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Po,Cn,Cn,Po,Po,
			Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lm,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Po,Po,Lo,Lo,
			Mn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Po,Lo,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cf,Me,Mn,
			Mn,Mn,Mn,Mn,Mn,Lm,Lm,Mn,Mn,So,Mn,Mn,Mn,Mn,Lo,Lo,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Lo,Lo,Lo,So,So,Lo,
			//==== Charcode-Range 0700 - 07FF ====
			Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Cn,Cf,
			Lo,Mn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Cn,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Lm,Lm,So,Po,Po,Po,Lm,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 0900 - 09FF ====
			Cn,Mn,Mn,Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Lo,Mc,Mc,
			Mc,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mc,Mc,Mc,Mc,Mn,Cn,Cn,
			Lo,Mn,Mn,Mn,Mn,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Mn,Mn,Po,Po,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Po,Lm,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Lo,
			Cn,Mn,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Lo,
			Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Cn,Lo,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Lo,Mc,Mc,
			Mc,Mn,Mn,Mn,Mn,Cn,Cn,Mc,Mc,Cn,Cn,Mc,Mc,Mn,Lo,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Mc,Cn,Cn,Cn,Cn,Lo,Lo,Cn,Lo,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Lo,Lo,Sc,Sc,No,No,No,No,No,No,So,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0A00 - 0AFF ====
			Cn,Mn,Mn,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Lo,
			Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Cn,Lo,Lo,Cn,Lo,Lo,Cn,Lo,Lo,Cn,Cn,Mn,Cn,Mc,Mc,
			Mc,Mn,Mn,Cn,Cn,Cn,Cn,Mn,Mn,Cn,Cn,Mn,Mn,Mn,Cn,Cn,
			Cn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Cn,Lo,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Mn,Mn,Lo,Lo,Lo,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Mn,Mn,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,
			Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Cn,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Lo,Mc,Mc,
			Mc,Mn,Mn,Mn,Mn,Mn,Cn,Mn,Mn,Mc,Cn,Mc,Mc,Mn,Cn,Cn,
			Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Cn,Sc,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0B00 - 0BFF ====
			Cn,Mn,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Lo,
			Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Cn,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Lo,Mc,Mn,
			Mc,Mn,Mn,Mn,Mn,Cn,Cn,Mc,Mc,Cn,Cn,Mc,Mc,Mn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Mn,Mc,Cn,Cn,Cn,Cn,Lo,Lo,Cn,Lo,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			So,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Mn,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Lo,
			Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Lo,Cn,Lo,Cn,Lo,Lo,
			Cn,Cn,Cn,Lo,Lo,Cn,Cn,Cn,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Mc,Mc,
			Mn,Mc,Mc,Cn,Cn,Cn,Mc,Mc,Mc,Cn,Mc,Mc,Mc,Mn,Cn,Cn,
			Lo,Cn,Cn,Cn,Cn,Cn,Cn,Mc,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			No,No,No,So,So,So,So,So,So,Sc,So,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0C00 - 0CFF ====
			Cn,Mc,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,
			Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Mn,Mn,
			Mn,Mc,Mc,Mc,Mc,Cn,Mn,Mn,Mn,Cn,Mn,Mn,Mn,Mn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Mn,Mn,Cn,Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,No,No,No,No,No,No,No,So,
			Cn,Cn,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,
			Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Lo,Mc,Mn,
			Mc,Mc,Mc,Mc,Mc,Cn,Mn,Mc,Mc,Cn,Mc,Mc,Mn,Mn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Mc,Mc,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Lo,Cn,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Cn,So,So,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0D00 - 0DFF ====
			Cn,Cn,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,
			Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Mc,Mc,
			Mc,Mn,Mn,Mn,Mn,Cn,Mc,Mc,Mc,Cn,Mc,Mc,Mc,Mn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Mc,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Mn,Mn,Cn,Cn,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			No,No,No,No,No,No,Cn,Cn,Cn,So,Lo,Lo,Lo,Lo,Lo,Lo,
			Cn,Cn,Mc,Mc,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Mn,Cn,Cn,Cn,Cn,Mc,
			Mc,Mc,Mn,Mn,Mn,Cn,Mn,Cn,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Mc,Mc,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0E00 - 0EFF ====
			Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Mn,Lo,Lo,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Cn,Cn,Cn,Sc,
			Lo,Lo,Lo,Lo,Lo,Lo,Lm,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Po,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Lo,Lo,Cn,Lo,Cn,Cn,Lo,Lo,Cn,Lo,Cn,Cn,Lo,Cn,Cn,
			Cn,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Cn,Lo,Lo,Lo,Cn,Lo,Cn,Lo,Cn,Cn,Lo,Lo,Cn,Lo,Lo,Lo,
			Lo,Mn,Lo,Lo,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Mn,Mn,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Cn,Lm,Cn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Lo,Lo,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 0F00 - 0FFF ====
			Lo,So,So,So,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,
			Po,Po,Po,So,So,So,So,So,Mn,Mn,So,So,So,So,So,So,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,No,No,No,No,No,No,
			No,No,No,No,So,Mn,So,Mn,So,Mn,Ps,Pe,Ps,Pe,Mc,Mc,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,
			Cn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mc,
			Mn,Mn,Mn,Mn,Mn,Po,Mn,Mn,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,So,So,
			So,So,So,So,So,So,Mn,So,So,So,So,So,So,Cn,So,So,
			Po,Po,Po,Po,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 1000 - 10FF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mc,Mc,Mn,Mn,Mn,
			Mn,Mc,Mn,Mn,Mn,Mn,Mn,Mn,Mc,Mn,Mn,Mc,Mc,Mn,Mn,Lo,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Po,Po,Po,Po,
			Lo,Lo,Lo,Lo,Lo,Lo,Mc,Mc,Mn,Mn,Lo,Lo,Lo,Lo,Mn,Mn,
			Mn,Lo,Mc,Mc,Mc,Lo,Lo,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Lo,Lo,
			Lo,Mn,Mn,Mn,Mn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Mn,Mc,Mc,Mn,Mn,Mc,Mc,Mc,Mc,Mc,Mc,Mn,Lo,Mc,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Cn,So,So,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Po,Lm,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 1200 - 12FF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Lo,Cn,Lo,Lo,Lo,Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
		//--------------------------------------------------
			//==== Charcode-Range 1700 - 17FF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,
			Lo,Lo,Mn,Mn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Mn,Mn,Mn,Po,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Mn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,
			Lo,Cn,Mn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Cf,Cf,Mc,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mc,Mc,
			Mc,Mc,Mc,Mc,Mc,Mc,Mn,Mc,Mc,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Po,Po,Po,Lm,Po,Po,Po,Sc,Lo,Mn,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Cn,Cn,Cn,
			No,No,No,No,No,No,No,No,No,No,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 1900 - 19FF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,
			Mn,Mn,Mn,Mc,Mc,Mc,Mc,Mn,Mn,Mc,Mc,Mc,Cn,Cn,Cn,Cn,
			Mc,Mc,Mn,Mc,Mc,Mc,Mc,Mc,Mc,Mn,Mn,Mn,Cn,Cn,Cn,Cn,
			So,Cn,Cn,Cn,Po,Po,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,
			Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,
			Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mc,Mc,Cn,Cn,Cn,Cn,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Cn,Po,Po,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
		//--------------------------------------------------
			//==== Charcode-Range 1B00 - 1BFF ====
			Mn,Mn,Mn,Mn,Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Mn,Mc,Mn,Mn,Mn,Mn,Mn,Mc,Mn,Mc,Mc,Mc,
			Mc,Mc,Mn,Mc,Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Po,Po,Po,Po,
			Po,So,So,So,So,So,So,So,So,So,So,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,So,So,So,So,So,So,So,So,So,Cn,Cn,Cn,
			Mn,Mn,Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Mc,Mn,Mn,Mn,Mn,Mc,Mc,Mn,Mn,Mc,Cn,Cn,Cn,Lo,Lo,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 1C00 - 1CFF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mc,Mc,Mn,Mn,Cn,Cn,Cn,Po,Po,Po,Po,Po,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Lo,Lo,Lo,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lm,Lm,Lm,Lm,Lm,Lm,Po,Po,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 1F00 - 1FFF ====
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Cn,Cn,Lu,Lu,Lu,Lu,Lu,Lu,Cn,Cn,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Cn,Cn,Lu,Lu,Lu,Lu,Lu,Lu,Cn,Cn,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Cn,Lu,Cn,Lu,Cn,Lu,Cn,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Cn,Cn,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lt,Lt,Lt,Lt,Lt,Lt,Lt,Lt,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lt,Lt,Lt,Lt,Lt,Lt,Lt,Lt,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lt,Lt,Lt,Lt,Lt,Lt,Lt,Lt,
			Ll,Ll,Ll,Ll,Ll,Cn,Ll,Ll,Lu,Lu,Lu,Lu,Lt,Sk,Ll,Sk,
			Sk,Sk,Ll,Ll,Ll,Cn,Ll,Ll,Lu,Lu,Lu,Lu,Lt,Sk,Sk,Sk,
			Ll,Ll,Ll,Ll,Cn,Cn,Ll,Ll,Lu,Lu,Lu,Lu,Cn,Sk,Sk,Sk,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Lu,Lu,Lu,Lu,Sk,Sk,Sk,
			Cn,Cn,Ll,Ll,Ll,Cn,Ll,Ll,Lu,Lu,Lu,Lu,Lt,Sk,Sk,Cn,
			//==== Charcode-Range 2000 - 20FF ====
			Zs,Zs,Zs,Zs,Zs,Zs,Zs,Zs,Zs,Zs,Zs,Cf,Cf,Cf,Cf,Cf,
			Pd,Pd,Pd,Pd,Pd,Pd,Po,Po,Pi,Pf,Ps,Pi,Pi,Pf,Ps,Pi,
			Po,Po,Po,Po,Po,Po,Po,Po,Zl,Zp,Cf,Cf,Cf,Cf,Cf,Zs,
			Po,Po,Po,Po,Po,Po,Po,Po,Po,Pi,Pf,Po,Po,Po,Po,Pc,
			Pc,Po,Po,Po,Sm,Ps,Pe,Po,Po,Po,Po,Po,Po,Po,Po,Po,
			Po,Po,Sm,Po,Pc,Po,Po,Po,Po,Po,Po,Po,Po,Po,Po,Zs,
			Cf,Cf,Cf,Cf,Cf,Cn,Cn,Cn,Cn,Cn,Cf,Cf,Cf,Cf,Cf,Cf,
			No,Ll,Cn,Cn,No,No,No,No,No,No,Sm,Sm,Sm,Ps,Pe,Ll,
			No,No,No,No,No,No,No,No,No,No,Sm,Sm,Sm,Ps,Pe,Cn,
			Lm,Lm,Lm,Lm,Lm,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,Sc,
			Sc,Sc,Sc,Sc,Sc,Sc,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Me,Me,Me,
			Me,Mn,Me,Me,Me,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range 2100 - 21FF ====
			So,So,Lu,So,So,So,So,Lu,So,So,Ll,Lu,Lu,Lu,Ll,Ll,
			Lu,Lu,Lu,Ll,So,Lu,So,So,So,Lu,Lu,Lu,Lu,Lu,So,So,
			So,So,So,So,Lu,So,Lu,So,Lu,So,Lu,Lu,Lu,Lu,So,Ll,
			Lu,Lu,Lu,Lu,Ll,Lo,Lo,Lo,Lo,Ll,So,So,Ll,Ll,Lu,Lu,
			Sm,Sm,Sm,Sm,Sm,Lu,Ll,Ll,Ll,Ll,So,Sm,So,So,Ll,So,
			Cn,Cn,Cn,No,No,No,No,No,No,No,No,No,No,No,No,No,
			Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,
			Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,
			Nl,Nl,Nl,Lu,Ll,Nl,Nl,Nl,Nl,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Sm,Sm,Sm,Sm,Sm,So,So,So,So,So,Sm,Sm,So,So,So,So,
			Sm,So,So,Sm,So,So,Sm,So,So,So,So,So,So,So,Sm,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,Sm,Sm,
			So,So,Sm,So,Sm,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
		//--------------------------------------------------
			//==== Charcode-Range 2300 - 23FF ====
			So,So,So,So,So,So,So,So,Sm,Sm,Sm,Sm,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			Sm,Sm,So,So,So,So,So,So,So,Ps,Pe,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,Sm,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,Sm,Sm,Sm,Sm,
			Sm,Sm,So,So,So,So,So,So,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 2700 - 27FF ====
			Cn,So,So,So,So,Cn,So,So,So,So,Cn,Cn,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,Cn,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,Cn,So,Cn,So,
			So,So,So,Cn,Cn,Cn,So,Cn,So,So,So,So,So,So,So,Cn,
			Cn,So,So,So,So,So,So,So,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,
			Ps,Pe,Ps,Pe,Ps,Pe,No,No,No,No,No,No,No,No,No,No,
			No,No,No,No,No,No,No,No,No,No,No,No,No,No,No,No,
			No,No,No,No,So,Cn,Cn,Cn,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			Cn,So,So,So,So,So,So,So,So,So,So,So,So,So,So,Cn,
			Sm,Sm,Sm,Sm,Sm,Ps,Pe,Sm,Sm,Sm,Sm,Cn,Sm,Cn,Cn,Cn,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
		//--------------------------------------------------
			//==== Charcode-Range 2900 - 29FF ====
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,
			Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Ps,Pe,Ps,Pe,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,
			Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Sm,Ps,Pe,Sm,Sm,
		//--------------------------------------------------
			//==== Charcode-Range 2C00 - 2CFF ====
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Cn,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Cn,
			Lu,Ll,Lu,Lu,Lu,Ll,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Lu,Lu,
			Cn,Ll,Lu,Ll,Ll,Lu,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lm,Cn,Cn,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Ll,So,So,So,So,So,So,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Po,Po,Po,Po,No,Po,Po,
			//==== Charcode-Range 2D00 - 2DFF ====
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Lm,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			//==== Charcode-Range 2E00 - 2EFF ====
			Po,Po,Pi,Pf,Pi,Pf,Po,Po,Po,Pi,Pf,Po,Pi,Pf,Po,Po,
			Po,Po,Po,Po,Po,Po,Po,Pd,Po,Po,Pd,Po,Pi,Pf,Po,Po,
			Pi,Pf,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Po,Po,Po,Po,Po,Lm,
			Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,Cn,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,So,
			So,So,So,So,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range 3000 - 30FF ====
			Zs,Po,Po,Po,So,Lm,Lo,Nl,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,
			Ps,Pe,So,So,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Pd,Ps,Pe,Pe,
			So,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Nl,Mn,Mn,Mn,Mn,Mn,Mn,
			Pd,Lm,Lm,Lm,Lm,Lm,So,So,Nl,Nl,Nl,Lm,Lo,Po,So,So,
			Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Mn,Mn,Sk,Sk,Lm,Lm,Lo,
			Pd,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Po,Lm,Lm,Lm,Lo,
		//--------------------------------------------------
			//==== Charcode-Range A600 - A6FF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lm,Po,Po,Po,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Lo,Lo,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Cn,Cn,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lo,Mn,
			Me,Me,Me,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Mn,Mn,Po,Lm,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			//==== Charcode-Range A700 - A7FF ====
			Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,Sk,
			Sk,Sk,Sk,Sk,Sk,Sk,Sk,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,Lm,
			Sk,Sk,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Ll,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,
			Lm,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Lu,Ll,Lu,Ll,Lu,Lu,Ll,
			Lu,Ll,Lu,Ll,Lu,Ll,Lu,Ll,Lm,Sk,Sk,Lu,Ll,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Lo,Lo,Lo,Lo,Lo,
			//==== Charcode-Range A800 - A8FF ====
			Lo,Lo,Mn,Lo,Lo,Lo,Mn,Lo,Lo,Lo,Lo,Mn,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Mc,Mc,Mn,Mn,Mc,So,So,So,So,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Po,Po,Po,Po,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Mc,Mc,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,Mc,
			Mc,Mc,Mc,Mc,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Po,Po,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range AA00 - AAFF ====
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mn,Mn,Mn,Mn,Mn,Mn,Mc,
			Mc,Mn,Mn,Mc,Mc,Mn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Mn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Mn,Mc,Cn,Cn,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Cn,Cn,Po,Po,Po,Po,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
		//--------------------------------------------------
			//==== Charcode-Range FB00 - FBFF ====
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Ll,Ll,Ll,Ll,Ll,Cn,Cn,Cn,Cn,Cn,Lo,Mn,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Sm,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Cn,Lo,Cn,
			Lo,Lo,Cn,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Cn,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
		//--------------------------------------------------
			//==== Charcode-Range FE00 - FEFF ====
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,Mn,
			Po,Po,Po,Po,Po,Po,Po,Ps,Pe,Po,Cn,Cn,Cn,Cn,Cn,Cn,
			Mn,Mn,Mn,Mn,Mn,Mn,Mn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,
			Po,Pd,Pd,Pc,Pc,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,Pe,Ps,
			Pe,Ps,Pe,Ps,Pe,Po,Po,Ps,Pe,Po,Po,Po,Po,Pc,Pc,Pc,
			Po,Po,Po,Cn,Po,Po,Po,Po,Pd,Ps,Pe,Ps,Pe,Ps,Pe,Po,
			Po,Po,Sm,Pd,Sm,Sm,Sm,Cn,Po,Sc,Po,Po,Cn,Cn,Cn,Cn,
			Lo,Lo,Lo,Lo,Lo,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Cf,
			//==== Charcode-Range FF00 - FFFF ====
			Cn,Po,Po,Po,Sc,Po,Po,Po,Ps,Pe,Po,Sm,Po,Pd,Po,Po,
			Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Nd,Po,Po,Sm,Sm,Sm,Po,
			Po,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,
			Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Lu,Ps,Po,Pe,Sk,Pc,
			Sk,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,
			Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ll,Ps,Sm,Pe,Sm,Ps,
			Pe,Po,Ps,Pe,Po,Po,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lm,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lm,Lm,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,
			Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Lo,Cn,
			Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,
			Cn,Cn,Lo,Lo,Lo,Lo,Lo,Lo,Cn,Cn,Lo,Lo,Lo,Cn,Cn,Cn,
			Sc,Sc,Sm,Sk,So,Sc,Sc,Cn,So,Sm,Sm,Sm,Sm,So,So,Cn,
			Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cn,Cf,Cf,Cf,So,So,Cn,Cn,
		};
		public static void Main(){
			System.IO.Stream str=System.IO.File.OpenWrite("GenCatTable.bin");
			for(int i=0;i<arr_gencat.Length;i++){
				str.WriteByte((byte)arr_gencat[i]);
			}
			str.Close();

			System.Console.WriteLine(
				"Completed: {0} * 0x100 values were written to GenCatTable.bin",
				arr_gencat.Length/0x100
				);
		}
	}

}