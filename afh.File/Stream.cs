using Marshal=System.Runtime.InteropServices.Marshal;
using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;

namespace afh.File{

	#region cls:SubStream
	/// <summary>
	/// ���� Stream ��񋟂��܂��B
	/// </summary>
	public class SubStream:System.IO.Stream{
		System.IO.Stream root;
		long offset;
		long length;
		long position;
		/// <summary>
		/// ���ꂪ�匳�� Stream �S�̂ɑ΂��� SubStream �ō݂邩�ǂ�����ێ����܂��B
		/// �匳�� Stream �ɑ΂��� SubStream �̏ꍇ�ɂ� Stream ���E���z�����������\�ł��B
		/// </summary>
		bool is_rootsubstr=false;

		/// <summary>
		/// SubStream �̃R���X�g���N�^�ł��B�� Stream �̌��݈ʒu����w�肵����������V���� Stream �Ƃ��܂��B
		/// </summary>
		/// <param name="rootstream">���� Stream �̌��ɂȂ� Stream ���w�肵�܂��B
		/// �w�肵�� Stream �� SubStream �łȂ������ꍇ�ɂ́ASubStream �Ń��b�v�������ɒu����������̂Œ��ӂ��ĉ������B
		/// <para>����́A������ SubStream ���猳�� Stream ��G�����ꍇ�� Seek �̋�����h���ׂ̕��ł��B
		/// �����̏ꏊ�� rootstream �ւ̎Q�Ƃ�����ꍇ�ɂ͒��ӂ��ĉ������B</para>
		/// <para>�u��������ꂽ�ꍇ�A�����̕ύX���o���Ȃ��Ȃ�܂��B</para>
		/// </param>
		/// <param name="length">�V���� Stream �̒������w�肵�܂��B</param>
		public SubStream(ref System.IO.Stream rootstream,long length)
			:this(ref rootstream,rootstream.Position,length){}
		/// <summary>
		/// SubStream �̃R���X�g���N�^�ł��B�� Stream �̎w�肵���ʒu����w�肵����������V���� Stream �Ƃ��܂��B
		/// </summary>
		/// <param name="rootstream">���� Stream �̌��ɂȂ� Stream ���w�肵�܂��B
		/// �w�肵�� Stream �� SubStream �łȂ������ꍇ�ɂ́ASubStream �Ń��b�v�������ɒu����������̂Œ��ӂ��ĉ������B
		/// ����́A������ SubStream ���猳�� Stream ��G�����ꍇ�� Seek �̋�����h���ׂ̕��ł��B
		/// �����̏ꏊ�� rootstream �ւ̎Q�Ƃ�����ꍇ�ɂ͒��ӂ��ĉ������B</param>
		/// <param name="offset">���� Stream �ɉ�����A�V���� Stream �̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="length">�V���� Stream �̒������w�肵�܂��B</param>
		public SubStream(ref System.IO.Stream rootstream,long offset,long length){
			SubStream rootstr=rootstream as SubStream;
			if(rootstr!=null){
				this.root=rootstr.root;
				this.offset=rootstr.offset+offset;
				this.length=length;

				if(rootstr.is_rootsubstr)goto regist; // �ȍ~�̃`�F�b�N�̕K�v�Ȃ�

				long rootstr_end=rootstr.offset+rootstr.length;
				if(rootstr_end<this.offset){
					this.root=null; // �Q�Ƃ̐ؒf
					throw new System.ArgumentOutOfRangeException("offset","SubStream �̊J�n�ʒu���� Stream �̏I�[���z���Ă��܂��B");
				}
				if(rootstr_end<this.offset+this.length){
					this.root=null;
					throw new System.ArgumentOutOfRangeException("length","SubStream �̏I�[�ʒu���� Stream �̏I�[���z���Ă��܂��B");
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
				throw new System.ArgumentException("�w�肵�� Stream �͊��� SubStream �ł��B","rootstream");
			if(!rootstream.CanSeek)
				throw new System.ArgumentException("���� stream �� Seek �ɑΉ����Ă��܂���̂ŁASubStream ���쐬���鎖�͏o���܂���B","rootstream");

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
		/// Stream �� SubStream �ɕϊ����܂��Bstream �������� SubStream �̏ꍇ�ɂ͒P�ɃL���X�g���s���܂��B
		/// ����ȊO�̏ꍇ�ɂ͐V���� SubStream �C���X�^���X�� stream �����b�v���܂��B
		/// </summary>
		/// <param name="stream">�ϊ��O�� Stream ���w�肵�܂��B</param>
		/// <returns>�ϊ���� Strema ��Ԃ��܂��B</returns>
		public static SubStream ToSubStream(System.IO.Stream stream){
			return (stream as SubStream)??new SubStream(stream);
		}

		/// <summary>
		/// ���� Stream ���ǂݏo���\���ǂ������擾���܂��B
		/// </summary>
		public override bool CanRead{
			get{return !closed&&this.root.CanRead;}
		}
		/// <summary>
		/// ���� Stream �����ňړ��\���ǂ������擾���܂��B
		/// </summary>
		public override bool CanSeek {
			get{return !closed;}
		}
		/// <summary>
		/// ���� Stream �������\���ǂ������擾���܂��B
		/// </summary>
		public override bool CanWrite {
			get{return !closed&&this.root.CanWrite; }
		}
		/// <summary>
		/// ���� Stream �̒������擾���܂��B
		/// </summary>
		public override long Length {
			get{
				if(closed)throw new System.ObjectDisposedException("SubStream");
				return this.length;
			}
		}
		/// <summary>
		/// ���� Stream �̃o�b�t�@�̖������f�[�^���������܂��B
		/// </summary>
		public override void Flush(){return;}
		/// <summary>
		/// ���� Stream ���ł̌��݂̓ǎ�E�����ʒu���擾���͐ݒ肵�܂��B
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
		/// ���� Stream ���ł̌��݈ʒu��ݒ肵�܂��B
		/// </summary>
		/// <param name="offset">�ړ���� <paramref name="origin"/> �Ɏw�肵���ʒu����̑��Έʒu���w�肵�܂��B</param>
		/// <param name="origin">�ړ���̊�ƂȂ�ʒu���w�肵�܂��B</param>
		/// <returns>���ۂɈړ�������̈ʒu��Ԃ��܂��B</returns>
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
					throw new System.ArgumentException("�w�肵�� SeekOrigin �͉��ߕs�\�ł��B","origin");
			}
		}
		/// <summary>
		/// Stream �̒�����ݒ肵�܂��B�匳�� SubStream �̎��ȊO�ɂ͒�����ݒ肷�鎖�͏o���܂���B
		/// </summary>
		/// <param name="value">�V���������̒l���w�肵�܂��B</param>
		public override void SetLength(long value){
			if(closed) throw new System.ObjectDisposedException("SubStream");
			if(!is_rootsubstr)throw new System.InvalidOperationException("�����ȑ���ł��BSubStream �̒�����ύX���鎖�͏o���܂���B");
			this.root.SetLength(value);
		}
		/// <summary>
		/// �w�肵�������̏���ǂݎ��܂��B
		/// </summary>
		/// <param name="buffer">�ǂݎ���������i�[����ׂ̃o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset"><paramref name="buffer"/> ���ł̏����J�n�ʒu���w�肵�܂��B</param>
		/// <param name="count">�ǂݎ��f�[�^�̒������o�C�g�P�ʂŎw�肵�܂��B</param>
		/// <returns>���ۂɓǂݎ��ꂽ���̒������o�C�g�P�ʂŕԂ��܂��B</returns>
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
		/// �w�肵������ Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="buffer">�������ޓ��e��ێ����Ă���o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset"><paramref name="buffer"/> ���ɉ����鏑�����ޏ��̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="count">�������ޏ��̒������o�C�g�P�ʂŎw�肵�܂��B</param>
		public override void Write(byte[] buffer,int offset,int count) {
			if(closed) throw new System.ObjectDisposedException("SubStream");
			if(offset<0) throw new System.ArgumentOutOfRangeException("offset","���̒l�͎w��ł��܂���B");
			if(count<0)throw new System.ArgumentOutOfRangeException("count","���̒l�͎w��ł��܂���B");
			if(buffer.Length<offset+count)throw new System.ArgumentException("buffer �̒����� offset+count ��菬�����ł��B","buffer");
			long pos=this.offset+this.position;
			long len=this.length-this.position;
			if(this.position<0||len<0)throw new System.InvalidOperationException("���݈ʒu�������\�Ȉʒu�ł͂���܂���B");
			if(!is_rootsubstr&&len<count)throw new System.InvalidCastException("������� Stream �̗e�ʂ�����܂���B");

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
		/// �I�u�W�F�N�g�̕ێ����郊�\�[�X���n�����܂��B
		/// </summary>
		/// <param name="disposing">�}�l�[�W���\�[�X���n������ꍇ�ɂ� true ���w�肵�܂��B</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&!closed)this.Close();
			base.Dispose(disposing);
		}

		private static Gen::Dictionary<System.IO.Stream,int> ref_counter=new Gen::Dictionary<System.IO.Stream,int>();
	}
	#endregion

	#region cls:CacheFileStream
	/// <summary>
	/// �f�B�X�N��̃L���b�V���t�@�C���ɓ��e��ǂݏ������� Stream �ł��B
	/// </summary>
	public sealed class CacheFileStream : System.IO.Stream{
		private string cache;
		private bool closed=false;
		private bool leave=false;
		private static System.Random random=new System.Random();
		private System.IO.FileStream str;

		/// <summary>
		/// CacheFileStream �̐V�����C���X�^���X�����������܂��B
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
		/// �g�p���Ă���L���b�V���t�@�C���̃p�X���擾���܂��B
		/// </summary>
		public string FileName{
			get{return this.cache;}
		}
		/// <summary>
		/// Stream �������ɃL���b�V���t�@�C�����c�����ǂ������擾���͐ݒ肵�܂��B
		/// ����ł� false, �܂�A�t�@�C���� Stream �����Ƌ��ɍ폜����܂��B
		/// </summary>
		public bool LeaveFile{
			get{return this.leave;}
			set{this.leave=true;}
		}
		/// <summary>
		/// �X�g���[������ALeaveFile �v���p�e�B������l false �ɐݒ肳��Ă���ꍇ�ɂ̓L���b�V���t�@�C�����폜���܂��B
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
		/// System.IO.Stream �ɂ���Ďg�p����Ă���A���}�l�[�W ���\�[�X��������A
		/// �I�v�V�����Ń}�l�[�W ���\�[�X��������܂��B
		/// </summary>
		/// <param name="disposing">
		/// �}�l�[�W ���\�[�X�ƃA���}�l�[�W ���\�[�X�̗������������ꍇ�� true�B
		/// �A���}�l�[�W ���\�[�X�������������ꍇ�� false�B
		/// </param>
		protected override void Dispose(bool disposing){
			this.Close();
			base.Dispose(disposing);
		}
		/// <summary>
		/// �X�g���[���ɑΉ����邷�ׂẴo�b�t�@���N���A���A�o�b�t�@���̃f�[�^����ɂȂ�f�o�C�X�ɏ������݂܂��B
		/// </summary>
		public override void Flush(){
			this.str.Flush();
		}
		/// <summary>
		/// �X�g���[������o�C�g�̃u���b�N��ǂݎ��A���̃f�[�^�����̃o�b�t�@�ɏ������݂܂��B
		/// </summary>
		/// <param name="buffer">�ǂݎ�������e���i�[����o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset">�ǂݎ�������� <paramref name="buffer"/> ���̏����J�n�ʒu���w�肵�܂��B</param>
		/// <param name="count">�ǂݎ����̒������w�肵�܂��B</param>
		/// <returns>���ۂɓǂݎ�������̒������o�C�g�P�ʂŕԂ��܂��B</returns>
		public override int Read(byte[] buffer, int offset, int count){
			return this.str.Read(buffer, offset, count);
		}
		/// <summary>
		/// �X�g���[���̌��݈ʒu�����̒l�ɐݒ肵�܂��B
		/// </summary>
		/// <param name="offset">�V�����ʒu�� <paramref name="origin"/> ����ɂ��Ďw�肵�܂��B</param>
		/// <param name="origin">�V�����ʒu�����߂�ۂ̊�̈ʒu���w�肵�܂��B</param>
		/// <returns>�V�������݈ʒu��Ԃ��܂��B</returns>
		public override long Seek(long offset,System.IO.SeekOrigin origin){
			return this.str.Seek(offset, origin);
		}
		/// <summary>
		/// �X�g���[���������̒l�ɐݒ肵�܂��B
		/// </summary>
		/// <param name="value">�V���� Stream �̒������w�肵�܂��B</param>
		public override void SetLength(long value){
			this.str.SetLength(value);
		}
		/// <summary>
		/// �o�b�t�@�̃f�[�^���g�p���āA�X�g���[���Ƀo�C�g�̃u���b�N���������݂܂��B
		/// </summary>
		/// <param name="buffer">�������ޏ���ێ����Ă���o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset">�������ޏ��� <paramref name="buffer"/> ���̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="count">�������ޏ��̒������w�肵�܂��B</param>
		public override void Write(byte[] buffer, int offset, int count){
			this.str.Write(buffer, offset, count);
		}
		/// <summary>
		/// ���݂̃X�g���[�����ǂݎ����T�|�[�g���Ă��邩�ǂ����������l���擾���܂��B
		/// </summary>
		public override bool CanRead{
			get{return this.str.CanRead;}
		}
		/// <summary>
		/// ���݂̃X�g���[�����V�[�N���T�|�[�g���Ă��邩�ǂ����������l���擾���܂��B
		/// </summary>
		public override bool CanSeek{
			get{return this.str.CanSeek;}
		}
		/// <summary>
		/// ���݂̃X�g���[�����^�C���A�E�g�ł��邩�ǂ��������肷��l���擾���܂��B
		/// </summary>
		public override bool CanTimeout{
			get{return this.str.CanTimeout;}
		}
		/// <summary>
		/// ���݂̃X�g���[�����������݂��T�|�[�g���Ă��邩�ǂ����������l���擾���܂��B
		/// </summary>
		public override bool CanWrite{
			get{return this.str.CanWrite;}
		}
		/// <summary>
		/// �X�g���[���� (�o�C�g�P��) ���擾���܂��B
		/// </summary>
		public override long Length{
			get{return this.str.Length;}
		}
		/// <summary>
		/// �X�g���[���̌��݈ʒu���擾�܂��͐ݒ肵�܂��B
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
	/// ������ Stream �ɓ����ɓ������e�̏���������ꍇ�Ɏg�p���܂��B
	/// </summary>
	public sealed class MultiStream:System.IO.Stream{
		private const string CANNOT_READ="afh.File.MultiStream\r\n����͏�����p�� System.IO.Stream �ō݂�ׁA�Ǎ��͏o���܂���B";
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
		//		Stream �̓o�^�E���
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
				throw new System.ArgumentOutOfRangeException("multiplicity","���̒l���w�肵�ĉ������B");
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
				throw new System.ArgumentOutOfRangeException("multiplicity","���̒l���w�肵�ĉ������B");
			}
			if(this.instance.TryRemoveStream(str,multiplicity,out removed)){
				switch(this.instance.StreamType){
					case StreamTypes.Null:
						__dll__.ThrowFatalException("Null �͊i�����ł��܂���B");
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
				__dll__.ThrowFatalException("���m�� StreamTypes �ł��B");
			}
			return removed;
		}

		//===========================================================
		//		��
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
				throw new System.Exception("MultiStream �͎w�肵���V�[�N�ɂ͑Ή����Ă��܂���B");
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
					throw new System.ArgumentException("DoubleStream.Write\r\n�������ޒ����ɂ͐��̒l��ݒ肵�ĉ������B","count");
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
					__dll__.ThrowFatalException("ListStream ���� DoubleStream �Ɋi�����ł��܂���B");
				}
				Gen::IEnumerator<Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo>> enumerator=liststr.slist.GetEnumerator();
				enumerator.Reset();
				if(!enumerator.MoveNext()){
					__dll__.ThrowFatalException("�v�f������锤�Ȃ񂾂���ǁc");
				}
				Gen::KeyValuePair<System.IO.Stream,MultiStream.StreamInfo> current=enumerator.Current;
				this.stream1=current.Key;
				current=enumerator.Current;
				this.info1=current.Value;
				if(!enumerator.MoveNext()){
					__dll__.ThrowFatalException("�v�f������锤�Ȃ񂾂���ǁc");
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
								throw new System.Exception("Fatal @ afh.File.MultiStream.ListStream.TryRemoveStream: ���蓾�܂���B\r\n\t��x�ɓ�ȏ�� System.IO.Stream �� slist ����폜����Ă���?\r\n\t�v�f����ȉ��Ȃ̂� ListStream �ɏ��i����?\r\n\t�v�f����ɂȂ������� DoubleStream �ɕϊ�����Ȃ�����?");
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
	/// ���[�̕s���ȃ������̈�ւ̃|�C���^���A�X�g���[���Ƃ��Ĉ����܂��B
	/// (���[�̔��f�͂��̃N���X���g�p���鑤�Œ��ӂ��Ȃ���Έׂ�܂���B
	/// SetLength ���g�p���Ė����I�ɖ��[���w�肷��ƈ��S�ł��B)
	/// </summary>
	public unsafe sealed class UnsafeMemoryStream:System.IO.Stream{
		private readonly byte* _base;
		private byte* current;
		private readonly bool readOnly;
		private long length=-1;
		/// <summary>
		/// �w�肵���ʒu����ɂ��� UnsageMemoryStream �����������܂��B
		/// </summary>
		/// <param name="pos">�������A�N�Z�X�̊�_���w�肵�܂��B</param>
		public UnsafeMemoryStream(void* pos):this(pos,false){}
		/// <summary>
		/// �w�肵���ʒu����ɂ��� UnsageMemoryStream �����������܂��B
		/// </summary>
		/// <param name="pos">�������A�N�Z�X�̊�_���w�肵�܂��B</param>
		/// <param name="readOnly">Stream ���ǂݎ���p���ۂ����w�肵�܂��B</param>
		public UnsafeMemoryStream(void* pos,bool readOnly){
			this._base=(byte*)pos;
			this.current=_base;
			this.readOnly=readOnly;
		}
		/// <summary>
		/// �ǂݎ�葀��ɑΉ����Ă��邩�ۂ����擾���܂��B��� true ��Ԃ��܂��B
		/// </summary>
		public override bool CanRead{get{return true;}}
		/// <summary>
		/// �V�[�N����ɑΉ����Ă��邩�ۂ����擾���܂��B��� true ��Ԃ��܂��B
		/// </summary>
		public override bool CanSeek{get{return true;}}
		/// <summary>
		/// ��������ɑΉ����Ă��邩�ۂ����擾���܂��B
		/// </summary>
		public override bool CanWrite{get{return !readOnly;}}
		/// <summary>
		/// Stream �̃o�b�t�@�ɂ���f�[�^��S�ĉ������܂��B
		/// </summary>
		public override void Flush(){}
		/// <summary>
		/// Stream �̒������擾���܂��B
		/// </summary>
		public override long Length{get{return this.length<0?long.MaxValue:this.length;}}
		/// <summary>
		/// ���݈ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public override long Position{
			get{return (long)(current-_base);}
			set{this.current=_base+value;}
		}
		/// <summary>
		/// Stream ����w�肵���o�b�t�@�փf�[�^��ǂݎ��܂��B
		/// </summary>
		/// <param name="buffer">�ǂݎ�����f�[�^���i�[����ׂ̃o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset">buffer �ɉ����鏑�����݊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="count">�ǂݎ��o�C�g�����w�肵�܂��B</param>
		/// <returns>���ۂɓǂݎ�����o�C�g����Ԃ��܂��B</returns>
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
		/// ���݈ʒu���ړ������܂��B
		/// </summary>
		/// <param name="offset">origin ����ɂ����A�ړ���̈ʒu���w�肵�܂��B</param>
		/// <param name="origin">�ʒu�̎w��̕��@���w�肵�܂��B</param>
		/// <returns>�ړ���̈ʒu��Ԃ��܂��B</returns>
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
						throw new System.NotSupportedException("���� Stream �ł͖��[����� Seek �͏o���܂���B���[����� Seek �𗘗p����ɂ́ASetLength �Œ������w�肵�Ă����K�v������܂��B");
					else
						current=this._base+this.length+offset;
					break;
				default:
					throw new System.ArgumentException("������ SeekOrigin �̒l�ł��B","origin");
			}
			return Position;
		}
		/// <summary>
		/// ���� Strema �̒������K�肵�܂��B
		/// </summary>
		/// <param name="value">���� Stream �̒������w�肵�܂��B</param>
		public override void SetLength(long value){
			this.length=value;
		}
		/// <summary>
		/// �w�肵���f�[�^�� Stream �ɏ������݂܂��B
		/// </summary>
		/// <param name="buffer">�������ރf�[�^���i�[���Ă���o�b�t�@���w�肵�܂��B</param>
		/// <param name="offset">buffer ���ŁA�������݂����f�[�^���n�܂�ʒu���w�肵�܂��B</param>
		/// <param name="count">�������ރf�[�^�̃o�C�g�����w�肵�܂��B</param>
		public override void Write(byte[] buffer,int offset,int count) {
			if(readOnly)throw new System.NotSupportedException("���� Stream �͓ǂݎ���p Stream �ł��B�������݂͏o���܂���B");
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