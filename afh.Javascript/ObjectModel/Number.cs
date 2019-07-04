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
			throw new System.ArgumentException("���ɑΉ�����I�u�W�F�N�g���w�肵�ĉ�����");
		}
		public override float ConvertCompat(System.Type t){
			//TODO: �K�؂ȕ��ɂȂ�l�Ɋg�[
			return base.ConvertCompat(t);
		}
		public override object Convert(System.Type t) {
			//TODO: �K�؂ȕ��ɂȂ�l�Ɋg�[
			return base.Convert(t);
		}
		public override string ToString() {
			return this.isInt?this.numL.ToString():this.numD.ToString();
		}
		//===========================================================
		//		���Z�q
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
            throw new System.NotImplementedException("������I�u�W�F�N�g�ɕϊ����ĉ��Z�����s");
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
            throw new System.NotImplementedException("�w�肵�����̌��Z�ɂ͑Ή����Ă��܂���c");
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
            throw new System.NotImplementedException("�w�肵�����̏�Z�ɂ͑Ή����Ă��܂���c");
		}
		//===========================================================
		//		�ÓI�R���X�g���N�^
		//===========================================================
		//TODO: �������i�K�Ŋm���ɌĂяo����������l�ɂ���
		// (�����Anew Number ���Ăяo���O�ɂ����炪�Ăяo����鎖���ۏ؂����̂Ȃ�Γ��ʂȏ����͗v��Ȃ�)
		// �� static Global ����Ăяo�����ɂ����B
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