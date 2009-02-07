using Marshal=System.Runtime.InteropServices.Marshal;
using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;

namespace afh.File{

	#region cls:SubStream
	/// <summary>
	/// 部分 Stream を提供します。
	/// </summary>
	public class SubStream:System.IO.Stream{
		System.IO.Stream root;
		long offset;
		long length;
		long position;
		/// <summary>
		/// これが大元の Stream 全体に対する SubStream で在るかどうかを保持します。
		/// 大元の Stream に対する SubStream の場合には Stream 境界を越えた書込が可能です。
		/// </summary>
		bool is_rootsubstr=false;

		/// <summary>
		/// SubStream のコンストラクタです。元 Stream の現在位置から指定した長さ分を新しい Stream とします。
		/// </summary>
		/// <param name="rootstream">部分 Stream の元になる Stream を指定します。
		/// 指定した Stream が SubStream でなかった場合には、SubStream でラップした物に置き換えられるので注意して下さい。
		/// <para>これは、複数の SubStream から元の Stream を触った場合に Seek の競合を防ぐ為の物です。
		/// 複数の場所に rootstream への参照がある場合には注意して下さい。</para>
		/// <para>置き換えられた場合、長さの変更が出来なくなります。</para>
		/// </param>
		/// <param name="length">新しい Stream の長さを指定します。</param>
		public SubStream(ref System.IO.Stream rootstream,long length)
			:this(ref rootstream,rootstream.Position,length){}
		/// <summary>
		/// SubStream のコンストラクタです。元 Stream の指定した位置から指定した長さ分を新しい Stream とします。
		/// </summary>
		/// <param name="rootstream">部分 Stream の元になる Stream を指定します。
		/// 指定した Stream が SubStream でなかった場合には、SubStream でラップした物に置き換えられるので注意して下さい。
		/// これは、複数の SubStream から元の Stream を触った場合に Seek の競合を防ぐ為の物です。
		/// 複数の場所に rootstream への参照がある場合には注意して下さい。</param>
		/// <param name="offset">元の Stream に於ける、新しい Stream の開始位置を指定します。</param>
		/// <param name="length">新しい Stream の長さを指定します。</param>
		public SubStream(ref System.IO.Stream rootstream,long offset,long length){
			SubStream rootstr=rootstream as SubStream;
			if(rootstr!=null){
				this.root=rootstr.root;
				this.offset=rootstr.offset+offset;
				this.length=length;

				if(rootstr.is_rootsubstr)goto regist; // 以降のチェックの必要なし

				long rootstr_end=rootstr.offset+rootstr.length;
				if(rootstr_end<this.offset){
					this.root=null; // 参照の切断
					throw new System.ArgumentOutOfRangeException("offset","SubStream の開始位置が元 Stream の終端を越えています。");
				}
				if(rootstr_end<this.offset+this.length){
					this.root=null;
					throw new System.ArgumentOutOfRangeException("length","SubStream の終端位置が元 Stream の終端を越えています。");
				}

			}else{
				this.root=rootstream;
				rootstream=new SubStream(rootstream);
				this.offset=offset;
				this.length=length;
			}

		regist:
			this.register_root();
		}
		private SubStream(System.IO.Stream rootstream){
			if(rootstream is SubStream)
				throw new System.ArgumentException("指定した Stream は既に SubStream です。","rootstream");
			if(!rootstream.CanSeek)
				throw new System.ArgumentException("この stream は Seek に対応していませんので、SubStream を作成する事は出来ません。","rootstream");

			this.root=rootstream;
			this.offset=0;
			this.length=rootstream.Length;
			this.position=rootstream.Position;
			this.is_rootsubstr=true;

			this.register_root();
		}
		private void register_root(){
			lock(this.root)if(ref_counter.ContainsKey(this.root)){
				ref_counter[this.root]++;
			}else{
				ref_counter[this.root]=1;
			}
		}
		/// <summary>
		/// Stream を SubStream に変換します。stream が元から SubStream の場合には単にキャストを行います。
		/// それ以外の場合には新しい SubStream インスタンスで stream をラップします。
		/// </summary>
		/// <param name="stream">変換前の Stream を指定します。</param>
		/// <returns>変換後の Strema を返します。</returns>
		public static SubStream ToSubStream(System.IO.Stream stream){
			return (stream as SubStream)??new SubStream(stream);
		}

		/// <summary>
		/// この Stream が読み出し可能かどうかを取得します。
		/// </summary>
		public override bool CanRead{
			get{return !closed&&this.root.CanRead;}
		}
		/// <summary>
		/// この Stream が中で移動可能かどうかを取得します。
		/// </summary>
		public override bool CanSeek {
			get{return !closed;}
		}
		/// <summary>
		/// この Stream が書込可能かどうかを取得します。
		/// </summary>
		public override bool CanWrite {
			get{return !closed&&this.root.CanWrite; }
		}
		/// <summary>
		/// この Stream の長さを取得します。
		/// </summary>
		public override long Length {
			get{
				if(closed)throw new System.ObjectDisposedException("SubStream");
				return this.length;
			}
		}
		/// <summary>
		/// この Stream のバッファの未処理データを処理します。
		/// </summary>
		public override void Flush(){return;}
		/// <summary>
		/// この Stream 内での現在の読取・書込位置を取得亦は設定します。
		/// </summary>
		public override long Position {
			get{
				if(closed) throw new System.ObjectDisposedException("SubStream");
				return this.position;
			}
			set{
				if(closed) throw new System.ObjectDisposedException("SubStream");
				this.position=value;
				if(is_rootsubstr&&this.position>=this.length)
					this.length=this.position;
			}
		}
		/// <summary>
		/// この Stream 内での現在位置を設定します。
		/// </summary>
		/// <param name="offset">移動先の <paramref name="origin"/> に指定した位置からの相対位置を指定します。</param>
		/// <param name="origin">移動先の基準となる位置を指定します。</param>
		/// <returns>実際に移動した先の位置を返します。</returns>
		public override long Seek(long offset,System.IO.SeekOrigin origin){
			if(closed) throw new System.ObjectDisposedException("SubStream");
			switch(origin) {
				case System.IO.SeekOrigin.Begin:
					this.Position=offset;
					return this.position;
				case System.IO.SeekOrigin.Current:
					this.Position+=offset;
					return this.position;
				case System.IO.SeekOrigin.End:
					this.Position=this.length+offset;
					return this.position;
				default:
					throw new System.ArgumentException("指定した SeekOrigin は解釈不能です。","origin");
			}
		}
		/// <summary>
		/// Stream の長さを設定します。大元の SubStream の時以外には長さを設定する事は出来ません。
		/// </summary>
		/// <param name="value">新しい長さの値を指定します。</param>
		public override void SetLength(long value){
			if(closed) throw new System.ObjectDisposedException("SubStream");
			if(!is_rootsubstr)throw new System.InvalidOperationException("無効な操作です。SubStream の長さを変更する事は出来ません。");
			this.root.SetLength(value);
		}
		/// <summary>
		/// 指定した長さの情報を読み取ります。
		/// </summary>
		/// <param name="buffer">読み取った情報を格納する為のバッファを指定します。</param>
		/// <param name="offset"><paramref name="buffer"/> 内での書込開始位置を指定します。</param>
		/// <param name="count">読み取るデータの長さをバイト単位で指定します。</param>
		/// <returns>実際に読み取られた情報の長さをバイト単位で返します。</returns>
		public override int Read(byte[] buffer,int offset,int count) {
			if(closed) throw new System.ObjectDisposedException("SubStream");
			long pos=this.offset+this.position;
			long len=this.length-this.position;
			if(this.position<0||len<0)return 0;
			if(len<count)count=(int)len;

			lock(this.root){
				if(this.root.Position!=pos)this.root.Position=pos;
				this.root.Read(buffer,offset,count);
			}

			this.position+=count;
			return count;
		}
		/// <summary>
		/// 指定した情報を Stream に書き込みます。
		/// </summary>
		/// <param name="buffer">書き込む内容を保持しているバッファを指定します。</param>
		/// <param name="offset"><paramref name="buffer"/> 内に於ける書き込む情報の開始位置を指定します。</param>
		/// <param name="count">書き込む情報の長さをバイト単位で指定します。</param>
		public override void Write(byte[] buffer,int offset,int count) {
			if(closed) throw new System.ObjectDisposedException("SubStream");
			if(offset<0) throw new System.ArgumentOutOfRangeException("offset","負の値は指定できません。");
			if(count<0)throw new System.ArgumentOutOfRangeException("count","負の値は指定できません。");
			if(buffer.Length<offset+count)throw new System.ArgumentException("buffer の長さが offset+count より小さいです。","buffer");
			long pos=this.offset+this.position;
			long len=this.length-this.position;
			if(this.position<0||len<0)throw new System.InvalidOperationException("現在位置が書込可能な位置ではありません。");
			if(!is_rootsubstr&&len<count)throw new System.InvalidCastException("書込先の Stream の容量が足りません。");

			lock(this.root){
				if(this.root.Position!=pos)this.root.Position=pos;
				this.root.Write(buffer,offset,count);
				if(is_rootsubstr)this.length=this.root.Length;
			}

			this.position+=count;
		}

		bool closed=false;
		public override void Close(){
			if(this.closed)return;
			this.closed=true;
			if(--ref_counter[this.root]==0){
				this.root.Close();
				ref_counter.Remove(this.root);
				this.root=null;
			}
			base.Close();
		}
		/// <summary>
		/// オブジェクトの保持するリソースを始末します。
		/// </summary>
		/// <param name="disposing">マネージリソースも始末する場合には true を指定します。</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&!closed)this.Close();
			base.Dispose(disposing);
		}

		private static Gen::Dictionary<System.IO.Stream,int> ref_counter=new Gen::Dictionary<System.IO.Stream,int>();
	}
	#endregion

	#region cls:CacheFileStream
	/// <summary>
	/// ディスク上のキャッシュファイルに内容を読み書きする Stream です。
	/// </summary>
	public sealed class CacheFileStream : System.IO.Stream{
		private string cache;
		private bool closed=false;
		private bool leave=false;
		private static System.Random random=new System.Random();
		private System.IO.FileStream str;

		/// <summary>
		/// CacheFileStream の新しいインスタンスを初期化します。
		/// </summary>
		public CacheFileStream(){
			string dirpath=System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory, "temp");
			afh.Application.Path.EnsureDirectoryExistence(ref dirpath);

			byte[] buffer=new byte[10];
			random.NextBytes(buffer);
			this.cache=System.IO.Path.Combine(dirpath,HexString.BytesToHex(buffer));
			this.cache=afh.Application.Path.GetAvailablePath(this.cache, ".stream");
			this.str=System.IO.File.Open(this.cache,System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite);
		}

		/// <summary>
		/// 使用しているキャッシュファイルのパスを取得します。
		/// </summary>
		public string FileName{
			get{return this.cache;}
		}
		/// <summary>
		/// Stream を閉じた後にキャッシュファイルを残すかどうかを取得亦は設定します。
		/// 既定では false, つまり、ファイルは Stream を閉じると共に削除されます。
		/// </summary>
		public bool LeaveFile{
			get{return this.leave;}
			set{this.leave=true;}
		}
		/// <summary>
		/// ストリームを閉じ、LeaveFile プロパティが既定値 false に設定されている場合にはキャッシュファイルを削除します。
		/// </summary>
		public override void Close(){
			if (!this.closed){
				this.closed=true;
				this.str.Close();
				if (!this.leave){
					System.IO.File.Delete(this.cache);
				}
				base.Close();
			}
		}
		/// <summary>
		/// System.IO.Stream によって使用されているアンマネージ リソースを解放し、
		/// オプションでマネージ リソースも解放します。
		/// </summary>
		/// <param name="disposing">
		/// マネージ リソースとアンマネージ リソースの両方を解放する場合は true。
		/// アンマネージ リソースだけを解放する場合は false。
		/// </param>
		protected override void Dispose(bool disposing){
			this.Close();
			base.Dispose(disposing);
		}
		/// <summary>
		/// ストリームに対応するすべてのバッファをクリアし、バッファ内のデータを基になるデバイスに書き込みます。
		/// </summary>
		public override void Flush(){
			this.str.Flush();
		}
		/// <summary>
		/// ストリームからバイトのブロックを読み取り、そのデータを特定のバッファに書き込みます。
		/// </summary>
		/// <param name="buffer">読み取った内容を格納するバッファを指定します。</param>
		/// <param name="offset">読み取った情報の <paramref name="buffer"/> 内の書込開始位置を指定します。</param>
		/// <param name="count">読み取る情報の長さを指定します。</param>
		/// <returns>実際に読み取った情報の長さをバイト単位で返します。</returns>
		public override int Read(byte[] buffer, int offset, int count){
			return this.str.Read(buffer, offset, count);
		}
		/// <summary>
		/// ストリームの現在位置を特定の値に設定します。
		/// </summary>
		/// <param name="offset">新しい位置を <paramref name="origin"/> を基準にして指定します。</param>
		/// <param name="origin">新しい位置を決める際の基準の位置を指定します。</param>
		/// <returns>新しい現在位置を返します。</returns>
		public override long Seek(long offset,System.IO.SeekOrigin origin){
			return this.str.Seek(offset, origin);
		}
		/// <summary>
		/// ストリーム長を特定の値に設定します。
		/// </summary>
		/// <param name="value">新しい Stream の長さを指定します。</param>
		public override void SetLength(long value){
			this.str.SetLength(value);
		}
		/// <summary>
		/// バッファのデータを使用して、ストリームにバイトのブロックを書き込みます。
		/// </summary>
		/// <param name="buffer">書き込む情報を保持しているバッファを指定します。</param>
		/// <param name="offset">書き込む情報の <paramref name="buffer"/> 内の開始位置を指定します。</param>
		/// <param name="count">書き込む情報の長さを指定します。</param>
		public override void Write(byte[] buffer, int offset, int count){
			this.str.Write(buffer, offset, count);
		}
		/// <summary>
		/// 現在のストリームが読み取りをサポートしているかどうかを示す値を取得します。
		/// </summary>
		public override bool CanRead{
			get{return this.str.CanRead;}
		}
		/// <summary>
		/// 現在のストリームがシークをサポートしているかどうかを示す値を取得します。
		/// </summary>
		public override bool CanSeek{
			get{return this.str.CanSeek;}
		}
		/// <summary>
		/// 現在のストリームがタイムアウトできるかどうかを決定する値を取得します。
		/// </summary>
		public override bool CanTimeout{
			get{return this.str.CanTimeout;}
		}
		/// <summary>
		/// 現在のストリームが書き込みをサポートしているかどうかを示す値を取得します。
		/// </summary>
		public override bool CanWrite{
			get{return this.str.CanWrite;}
		}
		/// <summary>
		/// ストリーム長 (バイト単位) を取得します。
		/// </summary>
		public override long Length{
			get{return this.str.Length;}
		}
		/// <summary>
		/// ストリームの現在位置を取得または設定します。
		/// </summary>
		public override long Position{
			get{return this.str.Position;}
			set{this.str.Position=value;}
		}
	}

	internal static class HexString {
		private unsafe static char* hex2char = ((char*)Marshal.AllocHGlobal(0x20));

		static unsafe HexString(){
			int index = 0;
			while(index<=9)hex2char[index]=(char)('0'+index++);
			while(index<=15)hex2char[index]=(char)('A'-10+index++);
		}

		public static unsafe string BytesToHex(byte[] bytes){
			char* pwstr=stackalloc char[(bytes.Length<<1)+1];
			pwstr[bytes.Length<<1]='\0';
			fixed(byte* data0=bytes){
				char* pwch=pwstr;
				byte* src=data0;
				byte* srcM=data0+bytes.Length;
				while(src<srcM){
					*pwch++=hex2char[*src>>4];
					*pwch++=hex2char[7&*src++];
				}
			}
			return new string(pwstr);
		}
	}


	#endregion

	#region cls:MultiStream
	/// <summary>
	/// 複数の Stream に同時に同じ内容の書込をする場合に使用します。
	/// </summary>
	public sealed class MultiStream:System.IO.Stream{
		private const string CANNOT_READ="afh.File.MultiStream\r\nこれは書込専用の System.IO.Stream で在る為、読込は出来ません。";
		private MultiStreamBase instance;

		public MultiStream(){
			this.instance=new NullStream();
		}

		public MultiStream(System.IO.Stream str,bool autoDispose){
			if(str is MultiStream){
				this.instance=((MultiStream)str).instance;
			}else{
				this.instance=new SingleStream(new NullStream(),str,autoDispose,1);
			}
		}
		//===========================================================
		//		Stream の登録・解約
		//===========================================================
		public void AddStream(System.IO.Stream str,bool autoDispose){
			if(str is MultiStream){
				foreach(Gen::KeyValuePair<System.IO.Stream,StreamInfo> pair in ((MultiStream)str).instance.EnumStreams()){
					this.AddNormalStream(pair.Key,autoDispose && pair.Value.autoDispose,pair.Value.multiplicity);
				}
			}else{
				this.AddNormalStream(str,autoDispose,1);
			}
		}
		private void AddNormalStream(System.IO.Stream str,bool autoDispose,int multiplicity){
			if(multiplicity<=0){
				throw new System.ArgumentOutOfRangeException("multiplicity","正の値を指定して下さい。");
			}
			switch(this.instance.StreamType){
				case StreamTypes.Null:
					this.instance=new SingleStream((NullStream)this.instance,str,autoDispose,multiplicity);
					break;
				case StreamTypes.Single:
					if(!this.instance.TryAddStream(str,autoDispose,multiplicity)){
						this.instance=new DoubleStream((SingleStream)this.instance,str,autoDispose,multiplicity);
					}
					break;
				case StreamTypes.Double:
					if(!this.instance.TryAddStream(str,autoDispose,multiplicity)){
						this.instance=new ListStream((DoubleStream)this.instance,str,autoDispose,multiplicity);
					}
					break;
				case StreamTypes.List:
					this.instance.TryAddStream(str,autoDispose,multiplicity);
					break;
			}
		}
		public System.IO.Stream RemoveStream(System.IO.Stream str){
			if(str is MultiStream){
				MultiStream stream=new MultiStream();
				foreach(Gen::KeyValuePair<System.IO.Stream,StreamInfo> pair in ((MultiStream)str).instance.EnumStreams()){
					int multiplicity=this.RemoveNormalStream(pair.Key,pair.Value.multiplicity);
					if(multiplicity>0){
						stream.AddNormalStream(pair.Key,pair.Value.autoDispose,multiplicity);
					}
				}
				return ((stream.instance.StreamType==StreamTypes.Null)?null:((System.IO.Stream)stream));
			}
			return ((this.RemoveNormalStream(str,1)>0)?str:null);
		}
		private int RemoveNormalStream(System.IO.Stream str,int multiplicity){
			int removed;
			if(multiplicity<=0){
				throw new System.ArgumentOutOfRangeException("multiplicity","正の値を指定して下さい。");
			}
			if(this.instance.TryRemoveStream(str,multiplicity,out removed)){
				switch(this.instance.StreamType){
					case StreamTypes.Null:
						__dll__.ThrowFatalException("Null は格下げできません。");
						return removed;
					case StreamTypes.Single:
						this.instance=new NullStream(this.instance);
						return removed;
					case StreamTypes.Double:
						this.instance=new SingleStream((DoubleStream)this.instance);
						return removed;
					case StreamTypes.List:
						this.instance=new DoubleStream((ListStream)this.instance);
						return removed;
				}
				__dll__.ThrowFatalException("未知の StreamTypes です。");
			}
			return removed;
		}

		//===========================================================
		//		他
		//===========================================================
		public override void Close() {
			this.instance.Close();
		}
		protected override void Dispose(bool disposing){
			base.Dispose(disposing);
			foreach(Gen::KeyValuePair<System.IO.Stream,StreamInfo> pair in this.instance.EnumStreams()){
				if(pair.Value.autoDispose){
					pair.Key.Dispose();
				}
			}
		}
		public override void Flush(){
			this.instance.Flush();
		}
		[System.Obsolete(CANNOT_READ)]
		public override int Read(byte[] buffer,int offset,int count){
			throw new System.Exception(CANNOT_READ);
		}
		public override long Seek(long offset,System.IO.SeekOrigin origin){
			return this.instance.Seek(offset,origin);
		}
		public override void SetLength(long value){
			this.instance.SetLength(value);
		}
		public override void Write(byte[] buffer,int offset,int count){
			this.instance.Write(buffer,offset,count);
		}
		public override bool CanRead{
			get{return this.instance.CanRead;}
		}
		public override bool CanSeek{
			get{return this.instance.CanSeek;}
		}
		public override bool CanTimeout{
			get{return this.instance.CanTimeout;}
		}
		public override bool CanWrite{
			get{return this.instance.CanWrite;}
		}
		public override long Length{
			get{return this.instance.Length;}
		}
		public override long Position{
			get{return this.instance.Position;}
			set{this.instance.Position=value;}
		}

		#region cls:MultiStreamBase
		private abstract class MultiStreamBase:System.IO.Stream{
			protected long length;
			protected long position;

			public MultiStreamBase(){
				this.position=0L;
				this.length=0L;
			}

			public MultiStreamBase(MultiStreamBase mstrbase){
				this.position=mstrbase.position;
				this.length=mstrbase.length;
			}

			public abstract int CountStream(System.IO.Stream str);
			protected StreamInfo CreateStreamInfo(System.IO.Stream str,bool autoDispose,int multiplicity){
				StreamInfo info=new StreamInfo();
				info.autoDispose=autoDispose;
				info.multiplicity=multiplicity;
				info.posOffset=str.Position - this.position;
				return info;
			}

			public abstract Gen::IEnumerable<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> EnumStreams();
			protected abstract void MovePosition(long delta);
			[System.Obsolete(CANNOT_READ)]
			public sealed override int Read(byte[] buffer,int offset,int count){
				throw new System.Exception(CANNOT_READ);
			}

			public sealed override long Seek(long offset,System.IO.SeekOrigin origin){
				switch(origin){
					case System.IO.SeekOrigin.Begin:
						this.Position=offset;
						return this.position;

					case System.IO.SeekOrigin.Current:
						this.length += offset;
						this.MovePosition(offset);
						return this.position;
				}
				throw new System.Exception("MultiStream は指定したシークには対応していません。");
			}

			public sealed override void SetLength(long value){
				this.length=value;
			}

			protected abstract void SetPosition();
			protected void SetPosition_Stream(System.IO.Stream str,StreamInfo info){
				str.Position=info.posOffset + this.position;
			}

			public abstract bool TryAddStream(System.IO.Stream str,bool autoDispose,int multiplicity);
			public abstract bool TryRemoveStream(System.IO.Stream str,int multiplicity,out int removed);
			public override void Write(byte[] buffer,int offset,int count){
				if(buffer==null){
					throw new System.ArgumentNullException();
				}
				if((offset<0) || (buffer.Length<(offset + count))){
					throw new System.ArgumentOutOfRangeException();
				}
				if(count<=0){
					throw new System.ArgumentException("DoubleStream.Write\r\n書き込む長さには正の値を設定して下さい。","count");
				}
				this.position += count;
			}

			public sealed override bool CanRead{
				get{return false;}
			}

			public sealed override long Length{
				get{return this.length;}
			}

			public sealed override long Position{
				get{return this.position;}
				set{
					this.position=value;
					this.SetPosition();
				}
			}

			public abstract StreamTypes StreamType{ get; }
		}
		#endregion

		#region cls:NullStream
		private class NullStream:MultiStreamBase{
			public NullStream(){}

			public NullStream(MultiStreamBase basestr) : base(basestr){}

			public override int CountStream(System.IO.Stream str){ return 0; }

			public override Gen::IEnumerable<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> EnumStreams(){
				yield break;
			}

			public override void Flush(){}

			protected override void MovePosition(long delta){}

			protected override void SetPosition(){}

			public override bool TryAddStream(System.IO.Stream str,bool autoDispose,int multiplicity){ return false; }

			public override bool TryRemoveStream(System.IO.Stream str,int multiplicity,out int removed){ removed=0; return false; }

			public override void Write(byte[] buffer,int offset,int count){
				base.Write(buffer,offset,count);
			}

			public override bool CanSeek{
				get{return true;}
			}

			public override bool CanWrite{
				get{return true;}
			}

			public override StreamTypes StreamType{
				get{return StreamTypes.Null;}
			}

		}
		#endregion

		#region cls:SingleStream
		private sealed class SingleStream:MultiStreamBase{
			internal StreamInfo info;
			internal System.IO.Stream stream;

			public SingleStream(DoubleStream dstr):base(dstr){
				StreamInfo info1=dstr.info1;
				if(info1.multiplicity==0) {
					this.stream=dstr.stream2;
					this.info=dstr.info2;
				}else{
					this.stream=dstr.stream1;
					this.info=dstr.info1;
				}
			}

			public SingleStream(NullStream basestr,System.IO.Stream stream,bool autoDispose,int multiplicity)
				: base(basestr){
				this.stream=stream;
				this.info=base.CreateStreamInfo(stream,autoDispose,multiplicity);
			}

			public override int CountStream(System.IO.Stream str){
				return ((this.stream==str) ? this.info.multiplicity : 0);
			}

			public override Gen::IEnumerable<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> EnumStreams(){
				yield return new Gen::KeyValuePair<System.IO.Stream,StreamInfo>(this.stream,this.info);
			}

			public override void Flush(){
				this.stream.Flush();
			}

			protected override void MovePosition(long delta){
				this.stream.Seek(delta,System.IO.SeekOrigin.Current);
			}

			protected override void SetPosition(){
				base.SetPosition_Stream(this.stream,this.info);
			}

			public override bool TryAddStream(System.IO.Stream str,bool autoDispose,int multiplicity){
				if(this.stream==str){
					this.info.multiplicity += multiplicity;
					if(!autoDispose){
						this.info.autoDispose=false;
					}
					return true;
				}
				return false;
			}

			public override bool TryRemoveStream(System.IO.Stream str,int multiplicity,out int removed){
				if(this.stream==str){
					if(this.info.multiplicity<=multiplicity){
						removed=this.info.multiplicity;
						this.info.multiplicity=0;
						return true;
					}
					removed=multiplicity;
					this.info.multiplicity -= multiplicity;
					return false;
				}
				removed=0;
				return false;
			}

			public override void Write(byte[] buffer,int offset,int count){
				base.Write(buffer,offset,count);
				this.stream.Write(buffer,offset,count);
			}

			public override bool CanSeek{
				get{
					return this.stream.CanSeek;
				}
			}

			public override bool CanWrite{
				get{
					return this.stream.CanWrite;
				}
			}

			public override StreamTypes StreamType{
				get{return StreamTypes.Single;}
			}
		}
		#endregion

		#region cls:DoubleStream
		private sealed class DoubleStream:MultiStream.MultiStreamBase{
			internal MultiStream.StreamInfo info1;
			internal MultiStream.StreamInfo info2;
			internal System.IO.Stream stream1;
			internal System.IO.Stream stream2;

			public DoubleStream(MultiStream.ListStream liststr):base(liststr){
				if(liststr.slist.Count != 2){
					__dll__.ThrowFatalException("ListStream から DoubleStream に格下げできません。");
				}
				Gen::IEnumerator<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> enumerator=liststr.slist.GetEnumerator();
				enumerator.Reset();
				if(!enumerator.MoveNext()){
					__dll__.ThrowFatalException("要素が二つある筈なんだけれど…");
				}
				Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo> current=enumerator.Current;
				this.stream1=current.Key;
				current=enumerator.Current;
				this.info1=current.Value;
				if(!enumerator.MoveNext()){
					__dll__.ThrowFatalException("要素が二つある筈なんだけれど…");
				}
				current=enumerator.Current;
				this.stream2=current.Key;
				current=enumerator.Current;
				this.info2=current.Value;
				enumerator.Dispose();
			}

			public DoubleStream(MultiStream.SingleStream single,System.IO.Stream str,bool autoDispose,int multiplicity)
				: base(single){
				this.stream1=single.stream;
				this.info1=single.info;
				this.stream2=str;
				this.info2=base.CreateStreamInfo(str,autoDispose,multiplicity);
			}

			public override int CountStream(System.IO.Stream str){
				return ((this.stream1==str) ? this.info1.multiplicity : ((this.stream2==str) ? this.info2.multiplicity : 0));
			}

			public override Gen::IEnumerable<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> EnumStreams(){
				yield return new Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>(this.stream1,this.info1);
				yield return new Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>(this.stream2,this.info2);
			}

			public override void Flush(){
				this.stream1.Flush();
				this.stream2.Flush();
			}

			protected override void MovePosition(long delta){
				this.stream1.Seek(delta,System.IO.SeekOrigin.Current);
				this.stream2.Seek(delta,System.IO.SeekOrigin.Current);
			}

			protected override void SetPosition(){
				base.SetPosition_Stream(this.stream1,this.info1);
				base.SetPosition_Stream(this.stream2,this.info2);
			}

			public override bool TryAddStream(System.IO.Stream str,bool autoDispose,int multiplicity){
				if(this.stream1==str){
					this.info1.multiplicity += multiplicity;
					if(!autoDispose){
						this.info1.autoDispose=false;
					}
					return true;
				}
				if(this.stream2==str){
					this.info2.multiplicity=multiplicity;
					if(!autoDispose){
						this.info2.autoDispose=false;
					}
					return true;
				}
				return false;
			}

			public override bool TryRemoveStream(System.IO.Stream str,int multiplicity,out int removed){
				if(this.stream1==str){
					if(this.info1.multiplicity<=multiplicity){
						removed=this.info1.multiplicity;
						this.info1.multiplicity=0;
						return true;
					}
					removed=multiplicity;
					this.info1.multiplicity -= multiplicity;
					return false;
				}
				if(this.stream2==str){
					if(this.info2.multiplicity<=multiplicity){
						removed=this.info2.multiplicity;
						this.info2.multiplicity=0;
						return true;
					}
					removed=multiplicity;
					this.info2.multiplicity -= multiplicity;
					return false;
				}
				removed=0;
				return false;
			}

			public override void Write(byte[] buffer,int offset,int count){
				base.Write(buffer,offset,count);
				this.stream1.Write(buffer,offset,count);
				this.stream2.Write(buffer,offset,count);
			}

			public override bool CanSeek{
				get{
					return (this.stream1.CanSeek && this.stream2.CanSeek);
				}
			}

			public override bool CanWrite{
				get{
					return (this.stream1.CanWrite && this.stream2.CanWrite);
				}
			}

			public override MultiStream.StreamTypes StreamType{
				get{
					return MultiStream.StreamTypes.Double;
				}
			}
		}
		#endregion

		#region cls:ListStream
		private sealed class ListStream:MultiStream.MultiStreamBase{
			internal Gen::Dictionary<System.IO.Stream,MultiStream.StreamInfo> slist;

			public ListStream(MultiStream.DoubleStream dstr,System.IO.Stream str,bool autoDispose,int multiplicity)
				: base(dstr){
				this.slist=new Gen::Dictionary<System.IO.Stream,MultiStream.StreamInfo>();
				this.slist.Add(dstr.stream1,dstr.info1);
				this.slist.Add(dstr.stream2,dstr.info2);
				this.slist.Add(str,base.CreateStreamInfo(str,autoDispose,multiplicity));
			}

			public override int CountStream(System.IO.Stream str){
				foreach(Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo> pair in this.slist){
					if(pair.Key==str){
						return pair.Value.multiplicity;
					}
				}
				return 0;
			}

			public override Gen::IEnumerable<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> EnumStreams(){
				return this.slist;
			}

			public override void Flush(){
				Gen::Dictionary<System.IO.Stream,MultiStream.StreamInfo>.KeyCollection.Enumerator enumerator=this.slist.Keys.GetEnumerator();
				try{
					while(enumerator.MoveNext()){
						enumerator.Current.Flush();
					}
				}finally{
					enumerator.Dispose();
				}
			}

			protected override void MovePosition(long delta){
				Gen::Dictionary<System.IO.Stream,MultiStream.StreamInfo>.KeyCollection.Enumerator enumerator=this.slist.Keys.GetEnumerator();
				try{
					while(enumerator.MoveNext()){
						enumerator.Current.Seek(delta,System.IO.SeekOrigin.Current);
					}
				}finally{
					enumerator.Dispose();
				}
			}

			protected override void SetPosition(){
				foreach(Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo> pair in this.slist){
					base.SetPosition_Stream(pair.Key,pair.Value);
				}
			}

			public override bool TryAddStream(System.IO.Stream str,bool autoDispose,int multiplicity){
				MultiStream.StreamInfo info;
				foreach(Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo> pair in this.slist){
					if(pair.Key==str){
						info=pair.Value;
						info.multiplicity += multiplicity;
						if(!autoDispose){
							info.autoDispose=false;
						}
						this.slist[str]=info;
						return true;
					}
				}
				info=base.CreateStreamInfo(str,autoDispose,multiplicity);
				this.slist.Add(str,info);
				return true;
			}

			public override bool TryRemoveStream(System.IO.Stream str,int multiplicity,out int removed){
				foreach(Gen::KeyValuePair<System.IO.Stream,StreamInfo> pair in this.slist){
					if(pair.Key==str){
						StreamInfo info=pair.Value;
						if(info.multiplicity<=multiplicity){
							removed=info.multiplicity;
							this.slist.Remove(str);
							if(this.slist.Count<2){
								throw new System.Exception("Fatal @ afh.File.MultiStream.ListStream.TryRemoveStream: あり得ません。\r\n\t一度に二つ以上の System.IO.Stream が slist から削除されている?\r\n\t要素が二つ以下なのに ListStream に昇格した?\r\n\t要素が二つになった時に DoubleStream に変換されなかった?");
							}
							return (this.slist.Count==2);
						}
						removed=multiplicity;
						info.multiplicity -= multiplicity;
						this.slist[str]=info;
						return false;
					}
				}
				removed=0;
				return false;
			}

			public override void Write(byte[] buffer,int offset,int count){
				base.Write(buffer,offset,count);
				Gen::Dictionary<System.IO.Stream,StreamInfo>.KeyCollection.Enumerator enumerator=this.slist.Keys.GetEnumerator();
				try{
					while(enumerator.MoveNext()){
						enumerator.Current.Write(buffer,offset,count);
					}
				}finally{
					enumerator.Dispose();
				}
			}

			public override bool CanSeek{
				get{
					Gen::Dictionary<System.IO.Stream,StreamInfo>.KeyCollection.Enumerator enumerator=this.slist.Keys.GetEnumerator();
					try{
						while(enumerator.MoveNext()){
							if(!enumerator.Current.CanSeek){
								return false;
							}
						}
					}finally{
						enumerator.Dispose();
					}
					return true;
				}
			}

			public override bool CanWrite{
				get{
					Gen::Dictionary<System.IO.Stream,StreamInfo>.KeyCollection.Enumerator enumerator=this.slist.Keys.GetEnumerator();
					try{
						while(enumerator.MoveNext()){
							if(!enumerator.Current.CanWrite){
								return false;
							}
						}
					}finally{
						enumerator.Dispose();
					}
					return true;
				}
			}

			public override StreamTypes StreamType{
				get{
					return StreamTypes.List;
				}
			}
		}
		#endregion

		[Interop::StructLayout(Interop::LayoutKind.Sequential)]
		private struct StreamInfo{
			public long posOffset;
			public int multiplicity;
			public bool autoDispose;
		}

		private enum StreamTypes{
			Null,
			Single,
			Double,
			List
		}
	}
	#endregion

	#region cls:UnsafeMemoryStream
	/// <summary>
	/// 末端の不明なメモリ領域へのポインタを、ストリームとして扱います。
	/// (末端の判断はこのクラスを使用する側で注意しなければ為りません。
	/// SetLength を使用して明示的に末端を指定すると安全です。)
	/// </summary>
	public unsafe sealed class UnsafeMemoryStream:System.IO.Stream{
		private readonly byte* _base;
		private byte* current;
		private readonly bool readOnly;
		private long length=-1;
		/// <summary>
		/// 指定した位置を基準にして UnsageMemoryStream を初期化します。
		/// </summary>
		/// <param name="pos">メモリアクセスの基準点を指定します。</param>
		public UnsafeMemoryStream(void* pos):this(pos,false){}
		/// <summary>
		/// 指定した位置を基準にして UnsageMemoryStream を初期化します。
		/// </summary>
		/// <param name="pos">メモリアクセスの基準点を指定します。</param>
		/// <param name="readOnly">Stream が読み取り専用か否かを指定します。</param>
		public UnsafeMemoryStream(void* pos,bool readOnly){
			this._base=(byte*)pos;
			this.current=_base;
			this.readOnly=readOnly;
		}
		/// <summary>
		/// 読み取り操作に対応しているか否かを取得します。常に true を返します。
		/// </summary>
		public override bool CanRead{get{return true;}}
		/// <summary>
		/// シーク操作に対応しているか否かを取得します。常に true を返します。
		/// </summary>
		public override bool CanSeek{get{return true;}}
		/// <summary>
		/// 書込操作に対応しているか否かを取得します。
		/// </summary>
		public override bool CanWrite{get{return !readOnly;}}
		/// <summary>
		/// Stream のバッファにあるデータを全て解決します。
		/// </summary>
		public override void Flush(){}
		/// <summary>
		/// Stream の長さを取得します。
		/// </summary>
		public override long Length{get{return this.length<0?long.MaxValue:this.length;}}
		/// <summary>
		/// 現在位置を取得又は設定します。
		/// </summary>
		public override long Position{
			get{return (long)(current-_base);}
			set{this.current=_base+value;}
		}
		/// <summary>
		/// Stream から指定したバッファへデータを読み取ります。
		/// </summary>
		/// <param name="buffer">読み取ったデータを格納する為のバッファを指定します。</param>
		/// <param name="offset">buffer に於ける書き込み開始位置を指定します。</param>
		/// <param name="count">読み取るバイト数を指定します。</param>
		/// <returns>実際に読み取ったバイト数を返します。</returns>
		public override int Read(byte[] buffer,int offset,int count){
			if(offset<0||offset>=buffer.Length)throw new System.ArgumentOutOfRangeException("offset");
			if(count<0||offset+count>buffer.Length)throw new System.ArgumentOutOfRangeException("count");
			if(current<_base)throw new StreamOverRunException();
			if(this.length>=0){
				long rest=_base+length-current;
				if(rest<0)throw new StreamOverRunException();
				if(count>rest)count=(int)rest;
			}

			for(int c=0;c<count;c++){
				buffer[offset++]=*current++;
			}
			return count;
		}
		/// <summary>
		/// 現在位置を移動させます。
		/// </summary>
		/// <param name="offset">origin を基準にした、移動先の位置を指定します。</param>
		/// <param name="origin">位置の指定の方法を指定します。</param>
		/// <returns>移動後の位置を返します。</returns>
		public override long Seek(long offset,System.IO.SeekOrigin origin){
			switch(origin){
				case System.IO.SeekOrigin.Begin:
					current=_base+offset;
					break;
				case System.IO.SeekOrigin.Current:
					current+=offset;
					break;
				case System.IO.SeekOrigin.End:
					if(length<0)
						throw new System.NotSupportedException("この Stream では末端からの Seek は出来ません。末端からの Seek を利用するには、SetLength で長さを指定しておく必要があります。");
					else
						current=this._base+this.length+offset;
					break;
				default:
					throw new System.ArgumentException("無効な SeekOrigin の値です。","origin");
			}
			return Position;
		}
		/// <summary>
		/// この Strema の長さを規定します。
		/// </summary>
		/// <param name="value">この Stream の長さを指定します。</param>
		public override void SetLength(long value){
			this.length=value;
		}
		/// <summary>
		/// 指定したデータを Stream に書き込みます。
		/// </summary>
		/// <param name="buffer">書き込むデータを格納してあるバッファを指定します。</param>
		/// <param name="offset">buffer 内で、書き込みたいデータが始まる位置を指定します。</param>
		/// <param name="count">書き込むデータのバイト数を指定します。</param>
		public override void Write(byte[] buffer,int offset,int count) {
			if(readOnly)throw new System.NotSupportedException("この Stream は読み取り専用 Stream です。書き込みは出来ません。");
			if(offset<0||offset>=buffer.Length)throw new System.ArgumentOutOfRangeException("offset");
			if(count<0||offset+count>buffer.Length)throw new System.ArgumentOutOfRangeException("count");
			if(current<_base||this.length>=0&&_base+length<current+count)throw new StreamOverRunException();
			
			for(int c=0;c<count;c++){
				*current++=buffer[offset++];
			}
		}
	}
	#endregion
}