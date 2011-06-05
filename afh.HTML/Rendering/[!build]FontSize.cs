using Gen=System.Collections.Generic;
using Gdi=System.Drawing;

namespace afh.Rendering{
	/// <summary>
	/// 単位付き長さを表します。
	/// <remarks>
	/// 既定では画面の解像度は 96dpi を想定しています。
	/// </remarks>
	/// </summary>
	[System.Obsolete]
	public sealed class FontSize:Length{

		private FontSize parent;
		private Gen::List<FontSize> children=null;

		//===========================================================
		//		初期化子
		//===========================================================
		private FontSize(float length,LengthUnit unit,FontSize parent,float[] units):base(length,unit,units){
			this.parent=parent;
			this.UpdateParent();
		}
		/// <summary>
		/// 独立インスタンスの親 Length です。
		/// </summary>
		private static readonly FontSize def_inst=new FontSize();
		private FontSize():this(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,LengthUnit.Pixel,null,new float[]{1}){}
		/// <summary>
		/// Length の独立した新しいインスタンスを作成します。
		/// </summary>
		/// <param name="length">長さを指定します。</param>
		/// <param name="unit">長さの単位を指定します。</param>
		public FontSize(float length,LengthUnit unit):this(length,unit,def_inst,new float[6]) {
			this.Dpi=96;
		}
		/// <summary>
		/// Length のインスタンスを既存の Length の子として作成します。
		/// </summary>
		/// <param name="length">長さを指定します。</param>
		/// <param name="unit">長さの単位を指定します。</param>
		/// <param name="parent">親 Length を指定します。</param>
		public FontSize(float length,LengthUnit unit,FontSize parent):this(length,unit,parent,parent.units){
			if(parent.children==null)parent.children=new Gen::List<FontSize>();
			parent.children.Add(this);
		}
		//===========================================================
		//		値の読み取り
		//===========================================================
		public static FontSize Parse(string text,FontSize parent){
			FontSize fsize=new FontSize(1,LengthUnit.Pixel,parent);
			fsize.Parse(text);
			return fsize;
		}
		protected override void OnPixelsChanged(){
			if(this.children!=null)foreach(FontSize len in children)len.UpdateParent();
		}
		/// <summary>
		/// 親に変更があった際に呼び出されます。
		/// </summary>
		private void UpdateParent(){
			this.PercentNorm=this.EmNorm=parent.Pixels;
		}
	}

	/// <summary>
	/// 描画物の長さの尺度を管理するクラスです。
	/// </summary>
	[System.Obsolete]
	public class Length{
		private float number;
		private LengthUnit unit;
		internal float[] units;
		private float pixels;

		public Length(float number,LengthUnit unit){
		}
		internal Length(float number,LengthUnit unit,float[] units){
			this.number=number;
			this.unit=unit;
			this.units=units;

			this.UpdateValue();
		}
		//===========================================================
		//		値の取得・設定
		//===========================================================
		/// <summary>
		/// 長さを指定する単位の部分を取得します。
		/// </summary>
		public LengthUnit Unit{
			get{return this.unit;}
			set{
				if(this.unit==value)return;
				this.unit=value;
				this.UpdateValue();
			}
		}
		/// <summary>
		/// 長さを指定する数値の部分を取得します。
		/// </summary>
		public float Number{
			get{return this.number;}
			set{
				if(this.number==value)return;
				this.number=value;
				this.UpdateValue();
			}
		}
		/// <summary>
		/// このインスタンスが表現する長さを pixel 単位で取得します。
		/// </summary>
		public float Pixels{
			get{return this.pixels;}
			private set{
				if(this.pixels==value)return;
				this.pixels=value;

				this.OnPixelsChanged();
				if(this.child_em!=null)   foreach(Length l in this.child_em)   l.EmNorm=this.pixels;
				if(this.child_perc!=null) foreach(Length l in this.child_perc) l.PercentNorm=this.pixels;
			}
		}
		protected virtual void OnPixelsChanged(){}
		//===========================================================
		//		文字列との相互変換
		//===========================================================
		#region 文字列 相互変換
		/// <summary>
		/// 長さを "数値 + 単位" の形の文字列にします。
		/// </summary>
		/// <returns>文字列で表現した長さを返します。</returns>
		public override string ToString(){
			return this.number.ToString()+unit_names[3+(int)this.unit];
		}
		private readonly static string[] unit_names=new string[]{"em","ex","%","px","pt","in","mm","cm","pc"};
		/// <summary>
		/// 文字列から font-size 値を読み取って、このインスタンスに反映させます。
		/// </summary>
		/// <param name="text">新しい font-size の値を表現する文字列を指定します。</param>
		public void Parse(string text){
			int i=0;

			// 数値部読み取り
			string num="";
			float number;
			while(i<text.Length) {
				char c=text[i++];
				if('0'<=c&&c<='9'||c=='.'||c=='e'||c=='E'||c=='+'||c=='-') {
					num+=c;
				} else if('０'<=c&&c<='９'||c=='．'||c=='ｅ'||c=='Ｅ'||c=='＋'||c=='－') {
					num+=(c+'0'-'０');
				} else break;
			}
			try{
				number=float.Parse(num);
			}catch(System.FormatException e){
				throw new System.FormatException("[afh.Renderer.Length.Parse]\r\n    数値部分の読み取りに失敗しました。\r\n    入力文字列: "+text,e);
			}

			// 空白飛ばし
			while(i<text.Length) {
				afh.Parse.LetterType ctype=afh.Parse.LetterType.GetLetterType(text[i++]);
				if(!ctype.IsInvalid&&!ctype.IsSpace) break;
			}

			// 単位部読み取り
			string uni="";
			while(i<text.Length) {
				char c=text[i++];
				afh.Parse.LetterType ctype=afh.Parse.LetterType.GetLetterType(c);
				if(ctype.IsInvalid||ctype.IsSpace) break;

				if('Ａ'<=c&&c<='Ｚ'||'ａ'<=c&&c<='ｚ')
					uni+=(c+'A'-'Ａ');
				else
					uni+=c;
			}
			LengthUnit unit;
			if(!dic_unames.TryGetValue(uni.ToLower(),out unit)){
				throw new System.FormatException("[afh.Renderer.Length.Parse]\r\n    単位部分の読み取りに失敗しました。\r\n    入力文字列: "+text+"\r\n    読み取った単位: "+uni);
			}

			this.unit=unit;
			this.number=number;
			this.UpdateValue();
		}
		private static readonly Gen::Dictionary<string,LengthUnit> dic_unames=new Gen::Dictionary<string,LengthUnit>();
		static Length(){
			dic_unames[""]=LengthUnit.Default;
			// px
			dic_unames["ピクセル"]=LengthUnit.Pixel;
			dic_unames["pixel"]=LengthUnit.Pixel;
			dic_unames["ﾋﾟｸｾﾙ"]=LengthUnit.Pixel;
			dic_unames["px"]=LengthUnit.Pixel;
			dic_unames["ドット"]=LengthUnit.Pixel;
			dic_unames["dot"]=LengthUnit.Pixel;
			dic_unames["ﾄﾞｯﾄ"]=LengthUnit.Pixel;
			dic_unames["dt"]=LengthUnit.Pixel;
			// pt
			dic_unames["ポイント"]=LengthUnit.Point;
			dic_unames["point"]=LengthUnit.Point;
			dic_unames["ﾎﾟｲﾝﾄ"]=LengthUnit.Point;
			dic_unames["pt"]=LengthUnit.Point;
			// in
			dic_unames["インチ"]=LengthUnit.Inch;
			dic_unames["inch"]=LengthUnit.Inch;
			dic_unames["ｲﾝﾁ"]=LengthUnit.Inch;
			dic_unames["in"]=LengthUnit.Inch;
			dic_unames["㏌"]=LengthUnit.Inch;
			dic_unames["吋"]=LengthUnit.Inch;
			dic_unames["㌅"]=LengthUnit.Inch;
			// mm
			dic_unames["ミリメートル"]=LengthUnit.MilliMeter;
			dic_unames["millimeter"]=LengthUnit.MilliMeter;
			dic_unames["ﾐﾘﾒｰﾄﾙ"]=LengthUnit.MilliMeter;
			dic_unames["mm"]=LengthUnit.MilliMeter;
			dic_unames["㎜"]=LengthUnit.MilliMeter;
			dic_unames["粍"]=LengthUnit.MilliMeter;
			dic_unames["㍉㍍"]=LengthUnit.MilliMeter;
			dic_unames["㍉"]=LengthUnit.MilliMeter;
			// cm
			dic_unames["センチメートル"]=LengthUnit.CentiMeter;
			dic_unames["centimeter"]=LengthUnit.CentiMeter;
			dic_unames["ｾﾝﾁﾒｰﾄﾙ"]=LengthUnit.CentiMeter;
			dic_unames["cm"]=LengthUnit.CentiMeter;
			dic_unames["㎝"]=LengthUnit.CentiMeter;
			dic_unames["糎"]=LengthUnit.CentiMeter;
			dic_unames["㌢㍍"]=LengthUnit.CentiMeter;
			dic_unames["㌢"]=LengthUnit.CentiMeter;
			// pc
			dic_unames["パイカ"]=LengthUnit.Pica;
			dic_unames["pica"]=LengthUnit.Pica;
			dic_unames["ﾊﾟｲｶ"]=LengthUnit.Pica;
			dic_unames["pc"]=LengthUnit.Pica;
			dic_unames["㍶"]=LengthUnit.Pica;
			// %
			dic_unames["パーセント"]=LengthUnit.Percent;
			dic_unames["percent"]=LengthUnit.Percent;
			dic_unames["ﾊﾟｰｾﾝﾄ"]=LengthUnit.Percent;
			dic_unames["%"]=LengthUnit.Percent;
			dic_unames["％"]=LengthUnit.Percent;
			dic_unames["㌫"]=LengthUnit.Percent;
			// ex/em
			dic_unames["ex"]=LengthUnit.Ex;
			dic_unames["em"]=LengthUnit.Em;
		}
		#endregion

		//===========================================================
		//		値の更新
		//===========================================================
		/// <summary>
		///  このインスタンスの dpi を取得亦は設定します。
		/// </summary>
		public float Dpi{
			get{return this.units[2];}
			set{
				this.units[0]=1;
				this.units[1]=value/72f;			// pt
				this.units[2]=value;				// in
				this.units[3]=value/25.4f;			// mm
				this.units[4]=this.units[4]*10;		// cm
				this.units[5]=this.units[1]*12;		// pt
				this.UpdateUnits(this.units);
			}
		}
		/// <summary>
		/// 長さの単位の変更 (dpi の変更など) を行います。
		/// </summary>
		/// <param name="newunits">新しい長さの単位を表現する配列を渡します。</param>
		private void UpdateUnits(float[] newunits){
			this.units=newunits;
			int unit_num=(int)this.unit;

			// set pixels
			switch(this.unit) {
				case LengthUnit.Em:
					this.pixels=this.number*em_norm;
					break;
				case LengthUnit.Ex:
					this.pixels=this.number*em_norm/2;
					break;
				case LengthUnit.Percent:
					this.pixels=this.number*percent_norm/100;
					break;
				default:
					this.pixels=this.number*this.units[(int)unit];
					break;
			}

			if(this.child_em!=null)foreach(Length len in this.child_em)len.UpdateUnits(newunits);
		}
		/// <summary>
		/// 値を計算し直します。
		/// </summary>
		private void UpdateValue(){
			switch(this.unit){
				case LengthUnit.Em:
					this.Pixels=this.number*em_norm;
					break;
				case LengthUnit.Ex:
					this.Pixels=this.number*em_norm/2;
					break;
				case LengthUnit.Percent:
					this.Pixels=this.number*percent_norm/100;
					break;
				default:
					this.Pixels=this.number*this.units[(int)unit];
					break;
			}
		}

		private float em_norm;
		private float percent_norm;
		private Gen::List<Length> child_em=new Gen::List<Length>();
		private Gen::List<Length> child_perc=new Gen::List<Length>();
		/// <summary>
		/// em 及び ex 単位の基準となる長さを取得又は設定します。
		/// </summary>
		protected float EmNorm{
			get{return this.em_norm;}
			set{
				if(this.em_norm==value)return;
				this.em_norm=value;
				if(this.unit==LengthUnit.Em||this.unit==LengthUnit.Ex)this.UpdateValue();
			}
		}
		/// <summary>
		/// % 単位の基準となる長さを取得又は設定します。
		/// </summary>
		protected float PercentNorm{
			get{return this.percent_norm;}
			set{
				if(this.percent_norm==value)return;
				this.percent_norm=value;
				if(this.unit==LengthUnit.Percent)this.UpdateValue();
			}
		}
	}

	/// <summary>
	/// 長さの単位を表現する列挙体です。
	/// </summary>
	public enum LengthUnit{
		/// <summary>
		/// 既定の単位を用いて長さを指定します。既定の単位は pixel です。
		/// </summary>
		Default=0,
		/// <summary>
		/// ピクセル (pixel, px) を単位として長さを指定します。
		/// </summary>
		Pixel=0,
		/// <summary>
		/// ポイント (point, pt) を単位として長さを指定します。
		/// </summary>
		Point=1,
		/// <summary>
		/// インチ (inch, in) を単位として長さを指定します。
		/// </summary>
		Inch=2,
		/// <summary>
		/// ミリメートル (millimeter, mm, 粍) を単位として長さを指定します。
		/// </summary>
		MilliMeter=3,
		/// <summary>
		/// センチメートル (centimeter, cm, 糎) を単位として長さを指定します。
		/// </summary>
		CentiMeter=4,
		/// <summary>
		/// パイカ (pica, pc) を単位として長さを指定します。
		/// </summary>
		Pica=5,
		/// <summary>
		/// 文字の大きさを単位として長さを指定します。
		/// </summary>
		Em=-1,
		/// <summary>
		/// x-height (文字 'x' の高さに等しい) を単位として長さを指定します。
		/// (この処理系では便宜上、em の半分として扱う。)
		/// </summary>
		Ex=-2,
		/// <summary>
		/// 親 Length を基準にして長さを % で指定します。
		/// </summary>
		Percent=-100,
	}
}