namespace afh.File.ID3v2_3_{
	// BinarySubstr に対する要件
	//   出来るだけ byte[] インスタンスを生成しない様にしてその部分列に対する参照を作成する。
	//   読み取るに連れて範囲の開始位置を移動する事が出来る様にする。(一つの BinarySubstr を用いて次々に読み込む)
	//   或る時点での範囲を保存する。 (後で、読み取り直したり、遅延読込を実行する為に必要)
	// →クラスに昇格
	/// <summary>
	/// byte[] の或る範囲に対する参照を管理します。
	/// </summary>
	public class BinarySubstr:System.ICloneable{
		/// <summary>
		/// データの範囲の開始位置を保持します。
		/// </summary>
		public int offset;
		/// <summary>
		/// データの範囲の長さを保持します。
		/// </summary>
		public int length;
		/// <summary>
		/// データを部分に持つ byte[] インスタンスを保持します。
		/// </summary>
		public byte[] array;

		/// <summary>
		/// 範囲の開始位置を取得亦は設定します。末端位置には変更がない様に設定されます。
		/// </summary>
		public int start{
			get{return this.offset;}
			set{this.length+=this.offset-value;this.offset=value;}
		}
		/// <summary>
		/// 範囲の末端位置を取得亦は設定します。
		/// </summary>
		public int end{
			get{return this.offset+this.length;}
			set{this.length=value-this.offset;}
		}

		public byte this[int index]{
			get{return this.array[this.offset+index];}
		}

		/// <summary>
		/// CRC32 値を計算します。
		/// </summary>
		/// <returns>計算した CRC32 値を返します。</returns>
		public unsafe uint CalculateCRC32(){
			const uint seed=0;	// 普通は 0 ?
			uint crc=~seed;
			fixed(byte* p=&array[offset]){
				byte* i=p;
				byte* m=p+length;
				while(i<m)crc=crc>>8^crc32_table[0xff&crc^*i++];
			}
			return ~crc;
		}

		private static uint[] crc32_table;
		static BinarySubstr(){
			crc32_table=new uint[256];
			for(uint n=0;n<256;n++){
				uint c=n;
				for(uint k=0;k<8;k++){
					c=c>>1^((c&1)==0?0:0xEDB88320u);
				}
				crc32_table[n]=c;
			}
		}

		/// <summary>
		/// 非同期化解除を行います。
		/// </summary>
		/// <returns>非同期化を解除した後のデータを返します。</returns>
		public BinarySubstr ResolveUnsync(){
			BinarySubstr r=new BinarySubstr(new byte[this.length],0);
			unsafe{
				fixed(byte* pImgB=&this.array[this.offset])fixed(byte* pDatB=&r.array[r.offset]){
					byte* pDat=pDatB;
					byte* pImg=pImgB;
					byte* pImgM=pImg+this.length;
					while(pImg<pImgM){
						if((*pDat++=*pImg++)==0xff&&*pImg==0)pImg++;
					}
					r.length=(int)(pDat-pDatB);
				}
			}
			return r;
		}
		public BinarySubstr Clone(){
			return new BinarySubstr(this.array,this.offset,this.length);
		}
		object System.ICloneable.Clone(){return this.Clone();}
		public unsafe static explicit operator byte[](BinarySubstr data){
			byte[] r=new byte[data.length];
			fixed(byte* pDb=&r[0])fixed(byte* pSb=&data.array[data.offset]){
				byte* pD=pDb;
				byte* pS=pSb;
				byte* pM=pD+data.length;
				while(pD<pM)*pD++=*pS++;
			}
			return r;
		}
		//=================================================
		//		コンストラクタ
		//=================================================
		/// <summary>
		/// BinarySubstr のコンストラクタ。指定した情報を使用して BinarySubstr を初期化します。
		/// array 全体をデータの範囲とします。
		/// </summary>
		/// <param name="array">参照先の情報を含む byte[] インスタンスを指定します。</param>
		public BinarySubstr(byte[] array):this(array,0,array.Length){}
		/// <summary>
		/// BinarySubstr のコンストラクタ。指定した情報を使用して BinarySubstr を初期化します。
		/// 指定した開始位置から array の末端までを範囲とします。
		/// </summary>
		/// <param name="array">参照先の情報を含む byte[] インスタンスを指定します。</param>
		/// <param name="offset">データの array 内での開始位置を指定します。</param>
		public BinarySubstr(byte[] array,int offset):this(array,offset,array.Length-offset){}
		/// <summary>
		/// BinarySubstr のコンストラクタ。指定した情報を使用して BinarySubstr を初期化します。
		/// </summary>
		/// <param name="array">参照先の情報を含む byte[] インスタンスを指定します。</param>
		/// <param name="offset">データの array 内での開始位置を指定します。</param>
		/// <param name="length">データの array 内での長さを指定します。</param>
		public BinarySubstr(byte[] array,int offset,int length){
			this.array=array;
			this.offset=offset;
			this.length=length;
			this.index=0;
		}
		//=================================================
		//		読み取り
		//=================================================
		/// <summary>
		/// 現在の読み取り位置を保持します。
		/// </summary>
		public int index;
		/// <summary>
		/// 現在の読み取り位置を初期化します。
		/// </summary>
		public void Clear(){this.index=0;}
		/// <summary>
		/// 現在の読み取り位置への byte* を取得します。
		/// 必ず array が fixed の状態で使用して下さい。
		/// </summary>
		public unsafe byte* CurrentPtr{
			get{fixed(byte* r=&this.array[this.offset+this.index])return r;}
		}
		/// <summary>
		/// 読み取り範囲の終端への byte* を取得します。
		/// 必ず array が fixed の状態で使用して下さい。
		/// </summary>
		public unsafe byte* ptrEnd{
			get{fixed(byte* r=&this.array[this.offset+this.length])return r;}
		}
		/// <summary>
		/// 指定した長さの範囲のデータを読み取ります。
		/// </summary>
		/// <param name="rangeLength">読み取るデータの長さを指定します。</param>
		/// <returns>読み取った範囲を BinarySubstr として返します。</returns>
		public BinarySubstr ReadRange(int rangeLength){
			BinarySubstr r=new BinarySubstr(this.array,this.offset+this.index,rangeLength);
			this.index+=rangeLength;
			return r;
		}
		/// <summary>
		/// 現在位置から最後迄の範囲のデータを読み取ります。
		/// </summary>
		/// <returns>読み取った範囲を BinarySubstr として返します。</returns>
		public BinarySubstr ReadRangeToEnd(){
			return this.ReadRange(this.length-this.index);
		}
		/// <summary>
		/// 1B 読み取ります。
		/// </summary>
		/// <returns>読み取った byte を返します。</returns>
		public byte ReadByte(){
			return this[this.index++];
		}
		//=================================================
		//		文字列読み取り
		//=================================================
		/// <summary>
		/// 現在の位置から最後迄文字列を読み取ります。
		/// </summary>
		/// <param name="enc">読み取りに使用する Encoding を指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadTextToEnd(System.Text.Encoding enc){
			string r=(enc??this.GetEncoding()).GetString(this.array,this.offset+this.index,this.length-this.index);
			this.index=this.length;
			return r;
		}
		/// <summary>
		/// Null 終端文字列を読み取ります。
		/// 読み取った分だけ、このインスタンスの有効範囲の開始位置を先にに進めます。
		/// </summary>
		/// <param name="enc">文字列の読み取りに使用する文字エンコーディングを指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ReadTextNullTerminated(System.Text.Encoding enc){
			if(enc==null)enc=this.GetEncoding();

			//-- null 文字迄の長さを調べる
			int len;
			bool twobytes=enc==UTF16||enc==UTF16BE;
			unsafe{
				fixed(byte* pB=&this.array[this.offset+this.index]){
					byte* p=pB;
					byte* pM=pB+this.length-this.index;
					while(p<pM)if(0==*p++&&(!twobytes||0==*p))break;
					len=(int)(p-pB);
				}
			}

			int off=this.offset+this.index;
			this.index+=len;
			return enc.GetString(this.array,off,len);
		}
		/// <summary>
		/// この範囲のデータ全体を文字列に変換します。
		/// </summary>
		/// <param name="enc">読み取りに使用する Encoding を指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public string ToString(System.Text.Encoding enc){
			return (enc??this.GetEncoding()).GetString(this.array,this.offset,this.length);
		}
		/// <summary>
		/// BOM を参照して Encoding を判定します。BOM が無い場合には Encoding.Default を返します。
		/// </summary>
		/// <returns>適当な Encoding を判定して返します。</returns>
		private System.Text.Encoding GetEncoding(){
			if(this.length<2){
				return System.Text.Encoding.Default;
			}
			byte b0=this.array[this.offset];
			byte b1=this.array[this.offset+1];
			if(b0==0xff&&b1==0xfe){
				this.start+=2;
				return System.Text.Encoding.Unicode;
			}else if(b0==0xfe&&b1==0xff){
				this.start+=2;
				return BinarySubstr.UTF16BE;
			}else if(this.length>=3&&b0==0xef&&b1==0xbb&&this.array[this.offset+2]==0xbf){
				this.start+=3;
				return System.Text.Encoding.UTF8;
			}else return System.Text.Encoding.Default;
		}
		private static System.Text.Encoding UTF16=System.Text.Encoding.Unicode;
		private static System.Text.Encoding UTF16BE=System.Text.Encoding.GetEncoding("UTF-16BE");
	}
}