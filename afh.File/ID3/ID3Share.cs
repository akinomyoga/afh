using Gen=System.Collections.Generic;
using Ref=System.Reflection;
using Interop=System.Runtime.InteropServices;

namespace afh.File.ID3{
	public sealed class ID3Utils{
		private static readonly byte[] bufferA=new byte[BUFFSIZE];
		private static readonly byte[] bufferB=new byte[BUFFSIZE];
		private const int BUFFSIZE=0x10000;
		private const int BUFFSIZE_HF=0x8000;
		private static readonly uint[] crc32_table=new uint[0x100];
		private static readonly object syncroot=new object();

		static ID3Utils() {
			for(uint n=0;n<0x100;n++){
				uint c=n;
				for(uint j=0;j<8;j++){
					c=c>>1^((c&1)==0?0:0xEDB88320u);
				}
				crc32_table[n]=c;
			}
		}
		/// <summary>
		/// CRC32 値を計算します。
		/// </summary>
		/// <param name="stream">CRC32 値を計算する対象のデータを格納している Stream を指定します。</param>
		/// <param name="start">Stream 内に於ける情報の先頭位置を指定します。</param>
		/// <param name="length">CRC32 値を計算する対象のデータの長さをバイト単位で指定します。</param>
		/// <returns>計算した CRC32 値を返します。</returns>
		public static unsafe uint CalculateCRC32(System.IO.Stream stream,long start,long length) {
			uint crc=uint.MaxValue;
			long position=stream.Position;
			stream.Position=start;
			lock(syncroot) {
				fixed(uint* numRef=crc32_table){
					while(length > 0L) {
						int read=stream.Read(bufferA,0,BUFFSIZE);
						if(read==0)throw new StreamOverRunException("CRC32 を計算中に StreamOverRun を起こしました。");
						if(length<read){
							read=(int)length;
							length=0L;
						}else{
							length-=read;
						}
						fixed(byte* p=bufferA){
							byte* i=p;
							byte* m=p+read;
							while(i<m)crc=crc>>8^crc32_table[0xff&crc^*i++];
						}
					}
				}
			}
			if(position>=0L)stream.Position=position;
			return ~crc;
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void dbg_Unsync() {
			System.Random random=new System.Random();
			byte[] buffer=new byte[0x400];
			for(int i=0;i<0x2800;i++) {
				random.NextBytes(buffer);
				System.IO.MemoryStream str=new System.IO.MemoryStream(buffer);
				System.IO.Stream stream3=ID3ResolveUnsync(ID3Unsynchronize(str));
				if(!EqualsStream(str,stream3)){
					System.Console.WriteLine("不一致発生 ***** BUG *****");
					return;
				}
			}
			System.Console.WriteLine("OK: 不一致は発生しませんでした");
		}
		/// <summary>
		/// 二つの Stream の内容が同一であるかどうかを判定します。
		/// </summary>
		/// <param name="str1">比較したい Stream を指定します。</param>
		/// <param name="str2">比較したい Stream を指定します。</param>
		/// <returns>二つの Stream の内容が同一であると判断した場合に true を返します。それ以外の場合には false を返します。</returns>
		public static unsafe bool EqualsStream(System.IO.Stream str1,System.IO.Stream str2) {
			if(str1.Length!=str2.Length)return false;
			str1.Position=0L;
			str2.Position=0L;
			lock(syncroot)while(true){
				int read=str1.Read(bufferA,0,BUFFSIZE);
				if(read==0)return true;
				if(read!=str2.Read(bufferB,0,BUFFSIZE))return false;
				fixed(byte* numRef=bufferA)fixed(byte* numRef2=bufferB){
					byte* pbyte1=numRef;
					byte* pbyte2=numRef2;
					byte* pbyte1M=pbyte1+read;
					while(pbyte1<pbyte1M)if(*pbyte1++!=*pbyte2++)return false;
				}
			}
		}
		//===========================================================
		//		非同期化処理
		//===========================================================
		/// <summary>
		/// 指定した情報が、id3 タグに埋め込む際に非同期化を必要とするかどうかを取得します。
		/// </summary>
		/// <param name="str">非同期化が必要かどうかを確認したいデータを格納している Stream を指定します。</param>
		/// <returns>非同期化処理が必要と判断された場合に true を返します。それ以外の場合には false を返します。</returns>
		public static unsafe bool ID3RequiredUnsync(System.IO.Stream str) {
			long pos=str.Position;
			str.Position=0;
			lock(syncroot)while(true){
				int read=str.Read(bufferA,0,BUFFSIZE);
				if(read==0){
					str.Position=pos;
					return false;
				}
				fixed(byte* pBase=bufferA){
					byte* p=pBase;
					byte* pM=pBase+read;
					while(p<pM)if(*p++==0xff&&((*p&0xe0)==0xe0||*p==0)){
						str.Position=pos;
						return true;
					}
				}
			}
		}
		/// <summary>
		/// 非同期化処理されたデータを、非同期化する前の状態に戻します。
		/// </summary>
		/// <param name="str">非同期化処理されたデータを格納している Stream を指定しています。</param>
		/// <returns>非同期化を解除した後のデータを格納している Stream を返します。
		/// Stream の現在位置は一番初めに設定されて返されます。</returns>
		public static unsafe System.IO.Stream ID3ResolveUnsync(System.IO.Stream str){
			str.Position=0;
			System.IO.MemoryStream stream=new System.IO.MemoryStream(checked((int)str.Length));
			lock(syncroot)while(true){
				int written;
				int read=str.Read(bufferA,0,BUFFSIZE);
				if(read==0){
					stream.Position=0L;
					return stream;
				}
				fixed(byte* pABase=bufferA)fixed(byte* pBBase=bufferB){
					byte* pdst	=pBBase;
					byte* psrc	=pABase;
					byte* psrcM	=pABase+read;
					while(psrc<psrcM)if((*pdst++=*psrc++)==0xff&&*psrc==0)psrc++;
					written=(int)(pdst-pBBase);
				}
				stream.Write(bufferB,0,written);
			}
		}
		/// <summary>
		/// 非同期化処理を実行します。
		/// </summary>
		/// <param name="str">非同期化処理を行う対象のデータを格納してる Stream を指定します。</param>
		/// <returns>非同期化処理を実行した後のデータを格納している Stream を返します。
		/// Stream の現在位置は一番初めに設定されて返されます。</returns>
		public static unsafe System.IO.Stream ID3Unsynchronize(System.IO.Stream str) {
			str.Position=0;
			System.IO.MemoryStream stream=new System.IO.MemoryStream(checked((int)str.Length));
			lock(syncroot)while(true){
				int written;
				int read=str.Read(bufferA,0,BUFFSIZE_HF);
				if(read==0){
					stream.Position=0L;
					return stream;
				}
				fixed(byte* pABase=bufferA)fixed(byte* pBBase=bufferB){
					byte* pdst	=pBBase;
					byte* psrc	=pABase;
					byte* psrcM	=psrc+read;
					while(psrc<psrcM)
						if((*pdst++=*psrc++)==0xff&&((*psrc&0xe0)==0xe0||*psrc==0))
							*pdst++=0;
					written=(int)(pdst - pBBase);
				}
				stream.Write(bufferB,0,written);
			}
		}
		//===========================================================
		//		byte* → string
		//===========================================================
		/// <summary>
		/// ポインタの参照先から null 終端文字列を読み取ります。
		/// </summary>
		/// <param name="start">文字列の情報を格納しているバッファへのポインタを指定します。</param>
		/// <param name="length">読み取るデータの長さの上限、つまり、バッファのサイズを指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public static unsafe string PtrToTerminatedString(byte* start,int length) {
			string str=new string((sbyte*)start,0,length);
			int index=0;
			fixed(char* pwstr=str) while(index<length&&pwstr[index++]!='\0') ;
			return index==length?str:str.Substring(0,index);
		}
		/// <summary>
		/// ポインタの参照先から null 終端文字列を読み取ります。
		/// </summary>
		/// <param name="start">文字列の情報を格納しているバッファへのポインタを指定します。</param>
		/// <param name="length">読み取るデータの長さの上限、つまり、バッファのサイズを指定します。</param>
		/// <param name="enc">読み取るのに使用する文字コードを指定します。</param>
		/// <returns>読み取った文字列を返します。</returns>
		public static unsafe string PtrToTerminatedString(byte* start,int length,System.Text.Encoding enc) {
			string str=new string((sbyte*)start,0,length,enc);
			int index=0;
			fixed(char* pwstr=str) while(index<length&&pwstr[index++]!='\0') ;
			return index==length?str:str.Substring(0,index);
		}
	}

	/// <summary>
	/// ファイル内のデータ単位の為の基本クラスです。
	/// ファイル内に於けるバイナリシーケンスをキャッシュします。
	/// データに変更があった場合にだけ、バイナリ生成を行います。
	/// </summary>
	[BindCustomReader(typeof(FileDataCached.CustomReader),Inheritable=true)]
	[BindCustomWriter(typeof(FileDataCached.CustomWriter),Inheritable=true)]
	public abstract class FileDataCached{
		protected System.IO.Stream stream;

		public FileDataCached(){
			this.changed=true;
			this.stream=null;
		}

		public FileDataCached(System.IO.Stream stream) {
			this.changed=false;
			this.stream=stream;
		}

		private bool changed;
		/// <summary>
		/// 内容に変更があった事を記録します。
		/// このメソッドが呼ばれた後に書込を試行すると、バイナリを生成します。
		/// </summary>
		protected void Changed(){this.changed=true;}
		/// <summary>
		/// 情報の内容に変更があったかどうかを取得します。
		/// </summary>
		public bool IsChanged {
			get{return this.changed;}
		}

		/// <summary>
		/// Stream から読み出す場合には、初めにデフォルトコンストラクタを使用して初期化されます。
		/// Stream からの読み出し前の初期化に、追加的な処理を必要とする場合は、ここに処理を記述します。
		/// 既定では何の処理も行いません。
		/// </summary>
		protected virtual void InitializeForRead(){}
		/// <summary>
		/// 派生クラスで、Stream からの読み出しを実装します。
		/// </summary>
		/// <remarks>読み出し結果が null の場合には例外 'new NullReturn()' を投げて下さい。
		/// CustomAccessor`1 はこの例外を受け取ると null を読み出し結果として返します。</remarks>
		/// <param name="accessor">読み出す情報を格納している StreamAccessor を指定します。</param>
		protected abstract void Read(StreamAccessor accessor);
		/// <summary>
		/// 派生クラスで、Stream への書込を実装します。
		/// </summary>
		/// <param name="accessor">情報の書込先の StreamAccessor を指定します。</param>
		protected abstract void Write(StreamAccessor accessor);

		private sealed class CustomReader:Design.CustomReaderBase<FileDataCached>{
			private CustomReader(System.Type filedata_type){
				if(typeof(FileDataCached).IsAssignableFrom(filedata_type))
					throw new System.ArgumentException("指定した型は FileDataCached の派生型ではありません。");
				this.target_type=filedata_type;
			}
			private System.Type target_type;
			protected override FileDataCached Read(StreamAccessor accessor) {
				FileDataCached ret=(FileDataCached)System.Activator.CreateInstance(target_type,true);
				ret.InitializeForRead();

				long pos_bef=accessor.Position;
				try {
					ret.Read(accessor);
				} catch(FileDataCached.NullReturn) {
					return null;
				}
				long pos_aft=accessor.Position;

				accessor.Stream.Position=pos_bef;
				ret.stream=accessor.ReadSubStream(pos_aft-pos_bef);
				ret.changed=false;
				return ret;
			}
		}
		private sealed class CustomWriter:Design.CustomWriterBase<FileDataCached>{
			private CustomWriter(System.Type filedata_type){
				if(typeof(FileDataCached).IsAssignableFrom(filedata_type))
					throw new System.ArgumentException("指定した型は FileDataCached の派生型ではありません。");
				if(filedata_type.IsAbstract||filedata_type.IsGenericTypeDefinition)
					throw new System.ArgumentException("指定した型はインスタンス化出来ません。");
				this.target_type=filedata_type;
			}
			private System.Type target_type;
			protected override void Write(FileDataCached value,StreamAccessor accessor) {
				// Update
				if(value.changed) {
					if(value.stream!=null) value.stream.Close();
					value.stream=new System.IO.MemoryStream();
					value.Write(new StreamAccessor(value.stream));
				}
				accessor.WriteStream(value.stream);
			}
		}
		/// <summary>
		/// Stream からの読み出しの結果が null である場合に FileDataCached.Read から投げる例外です。
		/// CustomAccessor`1 で読み込んでいる途中に発生した場合には CustomAccessor`1 は null を返します。
		/// </summary>
		public sealed class NullReturn:System.Exception{
			public NullReturn(){}
		}
	}
	/// <summary>
	/// ID3v2 のフレームの内容を保持する FileDataChached の基本クラスです。
	/// </summary>
	public abstract class FrameContent:FileDataCached{
		protected FrameContent(){}
		public FrameContent(System.IO.Stream stream):base(stream){}
	}
}
