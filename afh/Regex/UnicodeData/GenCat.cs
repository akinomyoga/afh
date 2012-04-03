using Gen=System.Collections.Generic;

public static class Program{
	public static void Main(string[] args){
		/*
		if(args.Length<1){
			System.Console.WriteLine("引数を入力して下さい。");
			return;
		}
		System.Console.WriteLine(CreateRange(args[0]));
		//*/

		/*
		if(args.Length<2){
			System.Console.WriteLine("引数を入力して下さい。");
			return;
		}
		MethodTrial.SwitchCase(str2char(args[0]),str2char(args[1]));
		//*/

		System.IO.Stream str=System.IO.File.OpenWrite("GeneralCategories.dat");
		System.IO.StreamWriter sw=new System.IO.StreamWriter(str);
		for(int i=0x0;i<0x10000;i+=0x100){
			System.Console.WriteLine(i.ToString("X4"));
			MethodTrial.SelectMethod(sw,(char)i,(char)(i+0xFF));
		}
		sw.Close();
		str.Close();
	}

	// 特定のカテゴリに対する範囲を出力します。
	public static string CreateRange(string category){
		WriteIntRange w=new WriteIntRange();

		foreach(string[] dat in MethodTrial.enum_chars(delegate(string[] d){return d[2]==category;})){
			w.Register(dat[0][0]);
		}

		return w.ToString();
	}

	private static char str2char(string s){
		int c=int.Parse(s,System.Globalization.NumberStyles.HexNumber);
		return (char)c;
	}
}


/// <summary>
/// カテゴリを判定する為の色々な方法を試します。
/// </summary>
public static class MethodTrial{
	// 特定の範囲に対するカテゴリの分類コードを出力します。
	public static void SwitchCase(char start,char end) {
		System.IO.TextWriter stdout=System.Console.Out;

		stdout.WriteLine("Range {0} - {1}",(int)start,(int)end);

		// 方法一: 連続する部分を不等号で判定
		MapCategories dic10=SwitchCase_hojo(start,(char)(end-1),1,0);
		int weight1=dic10.EvalWeight;

		// 方法二: 大文字小文字など交互になっている部分に対応
		MapCategories dic20=SwitchCase_hojo(start,(char)(end-1),2,0);
		MapCategories dic21=SwitchCase_hojo(start,(char)(end-1),2,1);
		int weight2=dic20.EvalWeight+dic21.EvalWeight+1;

		if(20<weight1&&20<weight2){
			// 方法零: テーブル
		}else if(weight1<=weight2){
			stdout.WriteLine("EvalWeight: {0}",weight1);
			stdout.WriteLine(dic10.ToString());
		}else{
			stdout.WriteLine("EvalWeight: {0}",weight2);
			stdout.WriteLine("** mod. == 0 **");
			stdout.WriteLine(dic20.ToString());
			stdout.WriteLine("** mod. == 1 **");
			stdout.WriteLine(dic21.ToString());
		}
	}

	public static void SelectMethod(System.IO.TextWriter w,char start,char end_inclusive){
		w.WriteLine("==== Charcode-Range {0:X4} - {1:X4} ====",(int)start,(int)end_inclusive);

		// 方法一: 連続する部分を不等号で判定
		MapCategories dic10=SwitchCase_hojo(start,end_inclusive,1,0);
		int weight1=dic10.EvalWeight;

		// 方法二: 大文字小文字など交互になっている部分に対応
		MapCategories dic20=SwitchCase_hojo(start,end_inclusive,2,0);
		MapCategories dic21=SwitchCase_hojo(start,end_inclusive,2,1);
		int weight2=dic20.EvalWeight+dic21.EvalWeight+1;

		// 出力
		if(20<weight1&&20<weight2){
			// 方法零: テーブル
			const string SEP=",";
			const string ENDL=",\r\n";

			int i=start;
			foreach(string[] dat in enum_chars(delegate(string[] d) {
				char c=d[0][0];
				return start<=c&&c<=end_inclusive;
			})){
				int c=dat[0][0];

				// 空白のコード (割り当てられていない文字)
				while(i<c){
					w.Write("Cn");
					w.Write(++i%16==0?ENDL:SEP);	
				}

				// 文字
				w.Write(dat[2]);
				w.Write(++i%16==0?ENDL:SEP);
			}

			// 空白のコード (割り当てられていない文字)
			while(i<end_inclusive+1){
				w.Write("Cn");
				w.Write(++i%16==0?ENDL:SEP);	
			}
			if(i%16!=0) w.Write(ENDL);
		}else if(weight1<=weight2){
			// 方法一:
			w.WriteLine(dic10.ToString('#'));
		}else{
			// 方法二:
			w.WriteLine(":: if(c%2==0) ::");
			w.WriteLine(dic20.ToString('#'));
			w.WriteLine(":: if(c%2==1) ::");
			w.WriteLine(dic21.ToString('#'));
		}
		
	}

	private static MapCategories SwitchCase_hojo(char start,char end_inclusive,int mod,int modval) {
		MapCategories dic=new MapCategories();

		foreach(string[] dat in enum_chars(delegate(string[] d){
			char c=d[0][0];
			return start<=c&&c<=end_inclusive&&(int)c%mod==modval;
		})){
			if(!dic.ContainsKey(dat[2]))
				dic[dat[2]]=new WriteIntRange(mod);
			dic[dat[2]].Register(dat[0][0]);
		}
	
		return dic;
	}

	internal static Gen::IEnumerable<string[]> enum_chars(System.Predicate<string[]> filter){
		System.IO.FileStream str=System.IO.File.OpenRead("UnicodeData.txt");
		System.IO.StreamReader sr=new System.IO.StreamReader(str);
		while(!sr.EndOfStream){
			string line=sr.ReadLine();
			string[] data=line.Split(';');
			if(data.Length<2)continue;

			int c=int.Parse(data[0],System.Globalization.NumberStyles.HexNumber);
			if(c>0xFFFF)continue;
			data[0]=((char)c).ToString();

			if(filter(data))
				yield return data;
		}
		sr.Close();
		str.Close();
	}

}
//====================================================================
/// <summary>
/// 複数のカテゴリに関する Range を保持します。
/// </summary>
public class MapCategories:Gen::Dictionary<string,WriteIntRange>{
	public int EvalWeight{
		get{
			int s=0;
			foreach(WriteIntRange w in this.Values)
				s+=w.EvalWeight;
			return s;
		}
	}

	public override string ToString(){
		System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
		foreach(Gen::KeyValuePair<string,WriteIntRange> pair in this){
			sbuf.AppendFormat("==== {0} ====\r\n",pair.Key);
			sbuf.AppendLine(pair.Value.ToString());
		}
		return sbuf.ToString();
	}

	public string ToString(char mode){
		if(mode=='f'){
			System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
			foreach(Gen::KeyValuePair<string,WriteIntRange> pair in this){
				sbuf.AppendFormat("{0}:{1}\r\n",pair.Key,pair.Value.ToString(mode));
			}
			return sbuf.ToString();
		}else if(mode=='#'){
			System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
			foreach(Gen::KeyValuePair<string,WriteIntRange> pair in this){
				string condition=pair.Value.ToString(mode);
				sbuf.AppendFormat("if({1})return {0};\r\n",pair.Key,condition.Substring(0,condition.Length-2)); // || を除去
			}
			return sbuf.ToString();
		}else{
			return this.ToString();
		}
	}
}

/// <summary>
/// 或るカテゴリに属する文字の領域を保持します。
/// </summary>
public class WriteIntRange{
	private int interval=1;

	public WriteIntRange(){}
	public WriteIntRange(int interval){
		this.interval=interval;
	}

	private bool first=true;
	private bool ended=false;

	private Bucket bCur;
	private Gen::List<Bucket> buckets=new System.Collections.Generic.List<Bucket>();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="c">小さい順番に登録する事を想定</param>
	public void Register(char c){
		if(first){
			first=false;
			this.bCur=new Bucket(c);
			return;
		}

		if(!this.bCur.AddChar(c,interval)){
			// 飛びがある場合
			this.buckets.Add(this.bCur);
			this.bCur=new Bucket(c);
		}
	}

	/*
	System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
	private void flush(){
		if(start!=current){
			this.sbuf.Append('-');
			this.append_char(current);
		}
		this.sbuf.Append("\r\n");
	}

	private void append_char(char c){
		this.sbuf.Append(@"'\x");
		this.sbuf.Append(((int)c).ToString("X4"));
		this.sbuf.Append('\'');
	}
	//*/

	private void finish(){
		if(ended)return;
		this.ended=true;
		this.buckets.Add(this.bCur);
	}

	public override string ToString(){
		this.finish();

		System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
		foreach(Bucket b in this.buckets){
			b.Write(sbuf,interval);
		}
		return sbuf.ToString();
	}

	public string ToString(char mode){
		this.finish();

		if(mode=='f'){
			System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
			foreach(Bucket b in this.buckets){
				b.Write(sbuf,interval,";");
			}
			return sbuf.ToString();
		}else if(mode=='#'){
			System.Text.StringBuilder sbuf=new System.Text.StringBuilder();
			foreach(Bucket b in this.buckets){
				b.Write(sbuf,interval,"C#");
			}
			return sbuf.ToString();
		}else{
			return this.ToString();
		}
	}

	public int EvalWeight{
		get {
			this.finish();

			int s=0;
			foreach(Bucket b in this.buckets)
				s+=b.EvalWeight;
			return s;
		}
	}

	/// <summary>
	/// 一つの纏まった Range を返します。
	/// </summary>
	private struct Bucket{
		public char start;
		public char end;

		public Bucket(char start){
			this.start=start;
			this.end=start;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		/// <param name="interval"></param>
		/// <returns>追加できなかったときに false を返します。</returns>
		public bool AddChar(char c,int interval){
			if(this.end+interval<c)return false;
			this.end=c;
			return true;
		}

		public void Write(System.Text.StringBuilder sbuf,int interval){
			this.Write(sbuf,interval,"\r\n");
		}
		public void Write(System.Text.StringBuilder sbuf,int interval,string sep){
			if(sep=="C#"){
				if(this.start==this.end){
					sbuf.AppendFormat("x==0x{0:X2}||",(int)start&0xFF);
				}else if(this.start+interval==this.end){
					sbuf.AppendFormat("x==0x{0:X2}||x==0x{1:X2}||",(int)start&0xFF,(int)end&0xFF);
				}else{
					sbuf.AppendFormat("0x{0:X2}<=x&&x<=0x{1:X2}||",(int)start&0xFF,(int)end&0xFF);
				}
			}else{
				if(this.start==this.end){
					append_char(sbuf,this.start);
					sbuf.Append(sep);
				}else if(this.start+interval==this.end){
					append_char(sbuf,this.start);
					sbuf.Append(sep);
					append_char(sbuf,this.end);
					sbuf.Append(sep);
				}else{
					append_char(sbuf,this.start);
					sbuf.Append("-");
					append_char(sbuf,this.end);
					sbuf.Append(sep);
				}
			}
		}
		private static void append_char(System.Text.StringBuilder sbuf,char c) {
			sbuf.Append(@"'\x");
			sbuf.Append(((int)c).ToString("X4"));
			sbuf.Append('\'');
		}

		public int EvalWeight{
			get{return this.start==this.end?1:2;}
		}
	}
}