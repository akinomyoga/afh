namespace afh.JavaScript{
	/*
	 * �z��̎���
	 * 
	 * * �����ɐݒ肳�ꂽ�l�̒��ōł��傫������ length �Ƃ��ĕԂ�
	 * * length ��菬�����l�ւ̃A�N�Z�X�́A�����o�������Ă� null ��Ԃ�
	 * */
	/// <summary>
	/// �z���\���I�u�W�F�N�g�ł�
	/// </summary>
	public class Array:Object{
		private int len=0;
		public Array(){}
		public Array(object[] array){
			foreach(object o in array)this[this.len++]=Global.ConvertFromManaged(o);
		}
		public Array(System.Collections.IEnumerable list){
			foreach(object o in list)this[this.len++]=Global.ConvertFromManaged(o);
		}
		/// <summary>
		/// �z��̏������q
		/// </summary>
		/// <param name="array">�z��̗v�f���w�肵�܂��B</param>
		/// <returns>�w�肵��������v�f�Ƃ��Ď��z�� Javascript.Array ��Ԃ��܂��B</returns>
		public static JavaScript.Array Construct(params JavaScript.Object[] array){
			JavaScript.Array ret=new JavaScript.Array();
			foreach(JavaScript.Object o in array)ret[ret.len++]=Global.ConvertFromManaged(o);
			return ret;
		}
		/// <summary>
		/// �z��̏������q
		/// </summary>
		/// <param name="count">�z��̑傫�����w�肵�܂�</param>
		/// <returns>�w�肵���傫�������z�� Javascript.Array ��Ԃ��܂�</returns>
		public static Array Construct(int count){
			JavaScript.Array array=new Array();
			array.len=count;
			return array;
		}

		public JavaScript.Object this[int i]{
			get{
				Object r=this[i.ToString()];
				return r==null&&i<this.len?new Null():r;
			}
			set{
				this[i.ToString()]=value;
				if(this.len<=i)this.len=i+1;
			}
		}
		public override Object this[string propName]{
			get{
				//TODO: property �ł̎����ɕύX�̗\��
				if(propName=="length")return new Number(this.length);
				return base[propName];
			}
			set{base[propName]=value;}
		}


		public int length{get{return this.len;}}

		public override string ToString() {
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			sb.Append("[");
			if(this.len>0){
				sb.Append(this[0].ToString());
				for(int i=1;i<this.len;i++){
					sb.Append(",");
					sb.Append(this[i].ToString());
				}
			}
			sb.Append("]");
			return sb.ToString();
		}

		internal static new void Initialize(){
			Global._global["Array"]=new ManagedDelegate(typeof(Array),"Construct");
			JavaScript.Object o=JavaScript.Object.Construct();
			Global._global["Array"]["prototype"]=o;
		}
	}
}