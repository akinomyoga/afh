namespace afh.JavaScript{
	public class Number:Object{
		private bool isInt=true;
		private long numL;
		private double numD;
		public Number(long num):base(Global._global["Number"]["prototype"]){
			this.numL=num;
		}
		public Number(double num):base(Global._global["Number"]["prototype"]){
			if(num%1==0&&num<=long.MaxValue&&num>=long.MinValue){
				this.numL=(long)num;
				return;
			}else{
				this.numD=num;
				this.isInt=false;
			}
		}

		public static Number Construct(JavaScript.Object o){
			if(Number.instanceof(o))return (Number)o;
			throw new System.ArgumentException("数に対応するオブジェクトを指定して下さい");
		}
		public override float ConvertCompat(System.Type t){
			//TODO: 適切な物になる様に拡充
			return base.ConvertCompat(t);
		}
		public override object Convert(System.Type t) {
			//TODO: 適切な物になる様に拡充
			return base.Convert(t);
		}
		public override string ToString() {
			return this.isInt?this.numL.ToString():this.numD.ToString();
		}
		//===========================================================
		//		演算子
		//===========================================================
		public static bool instanceof(JavaScript.Object o){
			return o is JavaScript.Number;//Global.IsCastable(o.GetType(),typeof(Javascript.Number));
		}
		public JavaScript.Object Add(JavaScript.Object num2){
			JavaScript.Number n2=num2 as JavaScript.Number;
			if(n2!=null){
				if(this.isInt){
					if(n2.isInt)return Global.ConvertFromManaged(this.numL+n2.numL);
					else return Global.ConvertFromManaged(this.numL+n2.numD);
				}else{
					if(n2.isInt)return Global.ConvertFromManaged(this.numD+n2.numL);
					else return Global.ConvertFromManaged(this.numD+n2.numD);
				}
			}
            throw new System.NotImplementedException("文字列オブジェクトに変換して加算を実行");
		}
		public JavaScript.Object Subtract(JavaScript.Object num2){
			JavaScript.Number n2=num2 as JavaScript.Number;
			if(n2!=null){
				if(this.isInt){
					if(n2.isInt)return Global.ConvertFromManaged(this.numL-n2.numL);
					else return Global.ConvertFromManaged(this.numL-n2.numD);
				}else{
					if(n2.isInt)return Global.ConvertFromManaged(this.numD-n2.numL);
					else return Global.ConvertFromManaged(this.numD-n2.numD);
				}
			}
            throw new System.NotImplementedException("指定した物の減算には対応していません…");
		}
		public JavaScript.Object Multiply(JavaScript.Object num2){
			JavaScript.Number n2=num2 as JavaScript.Number;
			if(n2!=null){
				if(this.isInt){
					if(n2.isInt)return Global.ConvertFromManaged(this.numL*n2.numL);
					else return Global.ConvertFromManaged(this.numL*n2.numD);
				}else{
					if(n2.isInt)return Global.ConvertFromManaged(this.numD*n2.numL);
					else return Global.ConvertFromManaged(this.numD*n2.numD);
				}
			}
            throw new System.NotImplementedException("指定した物の乗算には対応していません…");
		}
		//===========================================================
		//		静的コンストラクタ
		//===========================================================
		//TODO: 初期化段階で確実に呼び出しがかかる様にする
		// (もし、new Number を呼び出す前にこちらが呼び出される事が保証されるのならば特別な処理は要らない)
		// → static Global から呼び出す事にした。
		internal static new void Initialize(){
			Global._global["Number"]=new ManagedDelegate(typeof(Number),"Construct");
			JavaScript.Object o=JavaScript.Object.Construct();
			o[":+:"]=new ManagedJSBinaryOperator(typeof(Number).GetMethod("Add"));
			o[":-:"]=new ManagedJSBinaryOperator(typeof(Number).GetMethod("Subtract"));
			o[":*:"]=new ManagedJSBinaryOperator(typeof(Number).GetMethod("Multiply"));
			Global._global["Number"]["prototype"]=o;
		}
	}
}