using Gen=System.Collections.Generic;

namespace afh.File.Riff{
	[CustomRead("read"),CustomWrite("write")]
	public class RiffFile:List{
		public RiffFile(string filetype):base(filetype){
			this.name="RIFF";
		}
		private RiffFile(){}


		public void Close(){
			if(filestream!=null)filestream.Close();
		}

		private System.IO.Stream filestream;
		/// <summary>
		/// RIFF �`���̃t�@�C����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="filename">�Ǎ����̃t�@�C�����w�肵�܂��B</param>
		/// <returns>�쐬���� RiffFile �C���X�^���X��Ԃ��܂��B</returns>
		public static RiffFile FromFile(string filename){
			System.IO.Stream str=System.IO.File.OpenRead(filename);
			return FromStream(str,true);
		}
		/// <summary>
		/// Stream ���� RIFF �f�[�^��ǂݎ��ׂ� RiffFile ���쐬���܂��B
		/// </summary>
		/// <param name="stream">RIFF �f�[�^�̓ǂݎ�茳�ƂȂ� Stream ���w�肵�܂��B</param>
		/// <param name="closestream">
		/// �����ō쐬���� RiffFile ������ꂽ�� (Close ���Ăяo���ꂽ��) �ɁA
		/// ���ɂȂ��� Stream <paramref name="stream"/> �����ꍇ�� true ���w�肵�܂��B
		/// Stream �����L���Ă���ꍇ�ȂǏ���ɕ��Ȃ��ꍇ�ɂ� false ���w�肵�܂��B</param>
		/// <returns>�쐬���� RiffFile �C���X�^���X��Ԃ��܂��B</returns>
		public static RiffFile FromStream(System.IO.Stream stream,bool closestream){
			RiffFile r=read(new StreamAccessor(stream));
			if(closestream)r.filestream=stream;
			return r;
		}

		#region ReadWrite
		private static new RiffFile read(StreamAccessor accessor){
			if("RIFF"!=accessor.ReadString(EncodingType.CC4))
				throw new System.ApplicationException("�w�肵���f�[�^�� RIFF �f�[�^�ł͂���܂���B");

			RiffFile ret=new RiffFile();
			uint size=accessor.ReadUInt32(EncodingType.U4);
			ret.type=accessor.ReadString(EncodingType.CC4);

			ret.chunks=new Gen::List<Chunk>();
			StreamAccessor acChunks=new StreamAccessor(accessor.ReadSubStream(size-4));
			try{
				while(acChunks.RestLength>0){
					ret.chunks.Add(acChunks.Read<Chunk>());
				}
			}finally{
				acChunks.Stream.Close();
			}
			return ret;
		}
		private static void write(RiffFile value,StreamAccessor accessor){
			accessor.Write("RIFF",EncodingType.CC4);

			StreamAccessor acSize=new StreamAccessor(accessor.WriteSubStream(4));
			long pos0=accessor.Position;

			accessor.Write(value.type,EncodingType.CC4);
			foreach(Chunk chunk in value.chunks){
				accessor.Write(chunk,EncodingType.NoSpecified);
			}

			uint size=checked((uint)(accessor.Position-pos0));
			acSize.Write(size,EncodingType.U4);
			acSize.Stream.Close();
		}
		#endregion
	}

	[CustomRead("read"),CustomWrite("write")]
	public class Chunk:System.IDisposable{
		protected string name;
		/// <summary>
		/// Chunk �̖��O���擾���͐ݒ肵�܂��B
		/// </summary>
		public string Name{
			get{return this.name;}
			set{
				int len=value.Length;
				if(len!=4)value=len<4?value+new string(' ',4-len):value.Substring(4);
				this.name=value;
			}
		}

		public Chunk(string name){
			this.name=name;
		}

		/// <summary>
		/// Chunk �̓��e���w�肵���^�̃C���X�^���X�Ƃ��Ď擾���܂��B
		/// </summary>
		/// <typeparam name="T">�擾����C���X�^���X�̌^���w�肵�܂��B�ȉ��̕����w��ł��܂��B
		/// 1. <see cref="byte"/>[]
		/// 2. <see cref="System.IO.Stream"/>
		/// 3. RiffChunkReadWriteAttribute ������K�p�����^
		/// </typeparam>
		/// <returns>�w�肵���^�̃C���X�^���X��Ԃ��܂��B</returns>
		public T GetContent<T>(){
			if(typeof(T)==typeof(byte[]))
				return (T)(object)this.Data;
			if(typeof(T)==typeof(System.IO.Stream))
				return (T)(object)this.Stream;
			return (T)RiffChunkReadWriteAttribute.Read(typeof(T),new StreamAccessor(this.Stream));
		}
		/// <summary>
		/// Chunk �ɏ������ޓ��e���w�肵�܂��B
		/// </summary>
		/// <param name="value">�������ޓ��e���w�肵�܂��B</param>
		/// <include file='document.xml' path='desc[@name="afh.File.Riff.Chunk.SetContent"]/*'/>
		public void SetContent(object value){
			this.content=value;
		}
		private object content=null;
		public byte[] Data{
			get{
				if(this._data==null)this.ReadStream();
				return this._data;
			}
			set{
				if(value!=null)this.Stream=null;
				this._data=value;
			}
		}private byte[] _data;

		/// <summary>
		/// Chunk �̓��e�� Stream ���擾���͐ݒ肵�܂��B
		/// </summary>
		/// <include file='document.xml' path='desc[@name="afh.File.Riff.Chunk.SetContent"]/*'/>
		public System.IO.Stream Stream{
			get{return this._stream??new System.IO.MemoryStream(this._data);}
			set{
				if(value!=null)this._data=null;
				if(this._stream==value)return;
				if(this._stream!=null)this._stream.Close();
				this._stream=value;
				this._stream=value;
			}
		}private System.IO.Stream _stream;

		public void Dispose(){
			this.Stream=null;
		}

		#region ReadWrite
		private void ReadStream(){
			if(this._stream==null)return;
			long len=this._stream.Length;

			this._data=new byte[len];
			this._stream.Position=0;

			int c=0;
			if(len>int.MaxValue){
				len-=c=this._stream.Read(this._data,0,int.MaxValue);
			}

			this._stream.Read(this._data,c,(int)len);
			this._stream=null;
		}
		private static Chunk read(StreamAccessor accessor){
			string name=accessor.ReadString(EncodingType.CC4);
			if(name=="LIST"){
				accessor.Stream.Seek(-4,System.IO.SeekOrigin.Current);
				return List.read(accessor);
			}

			Chunk ret=new Chunk(name);

			uint size=accessor.ReadUInt32(EncodingType.U4);
			ret._stream=accessor.ReadSubStream(size);

			return ret;
		}
		private static void write(Chunk chunk,StreamAccessor accessor){
			accessor.Write(chunk.name,EncodingType.CC4);
			StreamAccessor ac2=new StreamAccessor(accessor.WriteSubStream(4));
			long pos0=accessor.Position;

			// ���e
			if(chunk.content!=null) {
				if(chunk.content is byte[]){
					accessor.Write((byte[])chunk.content);
				}else if(chunk.content is System.IO.Stream){
					accessor.WriteStream((System.IO.Stream)chunk.content);
				}else{
					RiffChunkReadWriteAttribute.Write(chunk.content,accessor);
				}
			}else if(chunk._data!=null){
				accessor.Write(chunk._data);
			}else if(chunk._stream!=null){
				accessor.WriteStream(chunk.Stream);
			}

			ac2.Write(checked((uint)(accessor.Position-pos0)),EncodingType.U4);
			ac2.Stream.Close();
		}
		#endregion
	}

	[CustomRead("read"),CustomWrite("write")]
	public class List:Chunk{
		protected string type;
		protected Gen::List<Chunk> chunks;
		public Gen::List<Chunk> Chunks{
			get{return this.chunks;}
		}

		public List(string type):base("LIST"){
			this.type=type;
			this.chunks=new Gen::List<Chunk>();
		}

		protected List():base("LIST"){}

		/// <summary>
		/// �w�肵�����O������ Chunk ��񋓂��܂��B
		/// </summary>
		/// <param name="name">�񋓂��� Chunk �̖��O���w�肵�܂��B</param>
		/// <returns>�w�肵�����O������ Chunk ��񋓂���ׂ� IEnumerable ��Ԃ��܂��B</returns>
		public Gen::IEnumerable<Chunk> EnumerateChunksByName(string name){
			foreach(Chunk chunk in this.chunks)
				if(chunk.Name==name)yield return chunk;
		}

		#region ReadWrite
		internal static List read(StreamAccessor accessor){
			List ret=new List();
			if("LIST"!=accessor.ReadString(EncodingType.CC4))
				throw new System.ApplicationException("�ǂݍ������Ƃ��Ă���f�[�^�� LIST �`�����N�ł͂Ȃ��ł��B");

			uint size=accessor.ReadUInt32(EncodingType.U4);
			ret.type=accessor.ReadString(EncodingType.CC4);

			ret.chunks=new Gen::List<Chunk>();
			StreamAccessor acChunks=new StreamAccessor(accessor.ReadSubStream(size-4));
			try{
				while(acChunks.RestLength>0){
					ret.chunks.Add(acChunks.Read<Chunk>());
				}
			}finally{
				acChunks.Stream.Close();
			}
			return ret;
		}
		private static void write(List value,StreamAccessor accessor){
			accessor.Write("LIST",EncodingType.CC4);

			StreamAccessor acSize=new StreamAccessor(accessor.WriteSubStream(4));
			long pos0=accessor.Position;

			accessor.Write(value.type,EncodingType.CC4);
			foreach(Chunk chunk in value.chunks){
				accessor.Write(chunk,EncodingType.NoSpecified);
			}

			uint size=checked((uint)(accessor.Position-pos0));
			acSize.Write(size,EncodingType.U4);
			acSize.Stream.Close();
		}
		#endregion
	}

	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
	public class RiffChunkReadWriteAttribute:System.Attribute{
		private string read;
		private string write;
		/// <summary>
		/// RIFF Chunk ����̓Ǎ��^�ւ̏������T�|�[�g���郁�\�b�h���w�肵�܂��B
		/// </summary>
		/// <param name="readingMethod">
		/// RIFF Chunk �̖{�� Stream ����C���X�^���X���쐬���郁�\�b�h�̖��O���w�肵�܂��B
		/// �V�O�j�`���� static &lt;�����̕t�����Ă���^&gt;(<see cref="StreamAccessor"/> accessor) �̃��\�b�h���w�肵�ĉ������B
		/// </param>
		/// <param name="writingMethod">
		/// RIFF Chunk �̖{�̂̓��e���������ރ��\�b�h���w�肵�܂��B
		/// �V�O�j�`���� static void(&lt;�����̕t�����Ă���^&gt; value,<see cref="StreamAccessor"/> accessor) �̃��\�b�h���w�肵�ĉ������B
		/// </param>
		public RiffChunkReadWriteAttribute(string readingMethod,string writingMethod){
			this.read=readingMethod;
			this.write=writingMethod;
		}

		private static System.Type[] read_types=new System.Type[]{typeof(StreamAccessor)};
		private static System.Reflection.ParameterModifier[] read_mods=new System.Reflection.ParameterModifier[0];
		private static Gen::Dictionary<System.Type,RiffChunkReadWriteAttribute> attr_cache=new Gen::Dictionary<System.Type,RiffChunkReadWriteAttribute>();
		internal static object Read(System.Type type,StreamAccessor accessor){
			RiffChunkReadWriteAttribute attr=GetAttribute(type);

			const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
			System.Reflection.MethodInfo m=type.GetMethod(attr.read,BF,null,read_types,read_mods);

			object ret=m.Invoke(null,new object[]{accessor});
			if(ret==null)
				throw new System.ApplicationException("RIFF Chunk �Ǎ��ׂ̈ɌĂяo�������\�b�h�̕Ԓl�� null �ł��B");
			return ret;
		}
		internal static void Write(object value,StreamAccessor accessor){
			System.Type type=value.GetType();
			RiffChunkReadWriteAttribute attr=GetAttribute(type);

			const System.Reflection.BindingFlags BF=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
			System.Reflection.MethodInfo m=type.GetMethod(attr.write,BF,null,new System.Type[]{type,typeof(StreamAccessor)},read_mods);
			m.Invoke(null,new object[]{value,accessor});
		}

		private static RiffChunkReadWriteAttribute GetAttribute(System.Type type){
			if(!attr_cache.ContainsKey(type)){
				RiffChunkReadWriteAttribute[] attrs
					=(RiffChunkReadWriteAttribute[])type.GetCustomAttributes(typeof(RiffChunkReadWriteAttribute),false);
				attr_cache[type]=attrs.Length==0?null:attrs[0];
			}
			RiffChunkReadWriteAttribute attr=attr_cache[type];
			if(attr==null)
				throw new System.ApplicationException("�w�肵���^�� RIFF Chunk �Ƃ��ēǂݏ������鎖�͏o���܂���B");
			return attr;
		}
	}
}