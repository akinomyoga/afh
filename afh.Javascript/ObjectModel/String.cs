namespace afh.JavaScript{
	public class String:JavaScript.Object{
		protected string str;
		public String():base(){this.str="";}
		public String(string str):base(){this.str=str;}

		public override string ToString(){return this.str;}
	}
}