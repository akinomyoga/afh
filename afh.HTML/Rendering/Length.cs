using Gen=System.Collections.Generic;

namespace afh.Rendering{
	/// <summary>
	/// 単位付きの長さの表現です。
	/// </summary>
	public struct LengthValue{
		/// <summary>
		/// 長さの値を保持します。
		/// </summary>
		public float value;
		/// <summary>
		/// 長さの単位を保持します。
		/// </summary>
		public LengthUnit unit;

		/// <summary>
		/// このインスタンスの表現する長さを px で取得します。
		/// </summary>
		/// <param name="norm">% 亦は em/ex 指定の場合の基準を指定します。</param>
		/// <returns>このインスタンスの表現する長さを px を単位にして表現した場合の数値を返します。</returns>
		public float ToPixel(float norm){
			int unit=(int)this.unit;
			return unit<0?this.value*norm/-unit:this.value*unit2px[unit];
		}

		private static float[] unit2px=new float[6];
		/// <summary>
		///  このインスタンスの dpi を取得亦は設定します。
		/// </summary>
		public static float Dpi{
			get{return unit2px[2];}
			set{
				unit2px[0]=1;
				unit2px[1]=value/72f;			// pt
				unit2px[2]=value;				// in
				unit2px[3]=value/25.4f;			// mm
				unit2px[4]=unit2px[4]*10;		// cm
				unit2px[5]=unit2px[1]*12;		// pt
			}
		}
		//===========================================================
		//		初期化子
		//===========================================================
		/// <summary>
		/// 指定した値と単位で LengthValue を初期化します。
		/// </summary>
		/// <param name="value">長さの値を指定します。</param>
		/// <param name="unit">長さの単位を指定します。</param>
		public LengthValue(float value,LengthUnit unit){
			this.value=value;
			this.unit=unit;
		}
		/// <summary>
		/// 文字列を読み取って LengthValue のインスタンスを作成します。
		/// </summary>
		/// <param name="text">長さの値を表現する文字列を指定します。例えば、"1em" や ".5cm" などです。</param>
		/// <param name="index">読み取り開始位置を指定します。読み取り終了位置を返します。</param>
		public LengthValue(string text,ref int index):this(1,LengthUnit.Default){
			this.Parse(text,ref index);
		}
		/// <summary>
		/// 文字列を読み取って LengthValue のインスタンスを作成します。
		/// </summary>
		/// <param name="text">長さの値を表現する文字列を指定します。例えば、"1em" や ".5cm" などです。</param>
		public LengthValue(string text):this(1,LengthUnit.Default){
			int i=0;
			this.Parse(text,ref i);
		}
		//===========================================================
		//		文字列との相互変換
		//===========================================================
		#region 文字列 相互変換
		/// <summary>
		/// 長さを "数値 + 単位" の形の文字列にします。
		/// </summary>
		/// <returns>文字列で表現した長さを返します。</returns>
		public override string ToString(){
			return this.value.ToString()+unit_names[3+(int)this.unit];
		}
		private readonly static string[] unit_names=new string[]{"em","ex","%","px","pt","in","mm","cm","pc"};
		/// <summary>
		/// 文字列から font-size 値を読み取って、このインスタンスに反映させます。
		/// </summary>
		/// <param name="text">新しい font-size の値を表現する文字列を指定します。</param>
		/// <param name="index">読み取り開始位置を指定します。読み取り終了位置を返します。</param>
		public void Parse(string text,ref int index){
			const string READ_NUMBER_ERROR=@"[afh.Renderer.LengthValue.Parse]
数値部分の読み取りに失敗しました。
入力文字列: ";
			const string READ_UNIT_ERROR=@"[afh.Renderer.LengthValue.Parse]
単位部分の読み取りに失敗しました。
入力文字列:     {0}
読み取った単位: {1}";
			//-------------------------------------------------------
			string num="";
			string uni="";

			// 数値部分の切り出し
			while(index<text.Length){
				char c=text[index++];
				if('0'<=c&&c<='9'||c=='.'||c=='e'||c=='E'||c=='+'||c=='-') {
					num+=c;
				}else if('０'<=c&&c<='９'||c=='．'||c=='ｅ'||c=='Ｅ'||c=='＋'||c=='－') {
					num+=(c+'0'-'０');
				}else break;
			}
			{
				char endc=num[num.Length-1];
				if(endc=='e'||endc=='E'){
					uni="e";
					num=num.Substring(0,num.Length-1);
				}
			}

			// 数値部分の読み取り
			float number;
			try{
				number=float.Parse(num);
			}catch(System.FormatException e){
				throw new System.FormatException(READ_NUMBER_ERROR+text,e);
			}

			// 空白飛ばし
			while(index<text.Length) {
				afh.Parse.LetterType ctype=afh.Parse.LetterType.GetLetterType(text[index++]);
				if(!ctype.IsInvalid&&!ctype.IsSpace)break;
			}

			// 単位部分の切り出し
			while(index<text.Length) {
				char c=text[index++];
				afh.Parse.LetterType ctype=afh.Parse.LetterType.GetLetterType(c);
				if(ctype.IsInvalid||ctype.IsSpace)break;

				if('Ａ'<=c&&c<='Ｚ'||'ａ'<=c&&c<='ｚ')
					uni+=(c+'A'-'Ａ');
				else
					uni+=c;
			}

			// 単位部分の読み取り
			LengthUnit unit;
			if(!dic_unames.TryGetValue(uni.ToLower(),out unit)){
				throw new System.FormatException(string.Format(READ_UNIT_ERROR,text,uni));
			}

			this.unit=unit;
			this.value=number;
		}
		private static readonly Gen::Dictionary<string,LengthUnit> dic_unames=new Gen::Dictionary<string,LengthUnit>();
		static LengthValue(){
			Dpi=96;

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
	}
	/// <summary>
	/// 長さの単位を表現する列挙体です。
	/// </summary>
	public enum LengthUnit:int{
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