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
		/// CRC32 �l���v�Z���܂��B
		/// </summary>
		/// <param name="stream">CRC32 �l���v�Z����Ώۂ̃f�[�^���i�[���Ă��� Stream ���w�肵�܂��B</param>
		/// <param name="start">Stream ���ɉ�������̐擪�ʒu���w�肵�܂��B</param>
		/// <param name="length">CRC32 �l���v�Z����Ώۂ̃f�[�^�̒������o�C�g�P�ʂŎw�肵�܂��B</param>
		/// <returns>�v�Z���� CRC32 �l��Ԃ��܂��B</returns>
		public static unsafe uint CalculateCRC32(System.IO.Stream stream,long start,long length) {
			uint crc=uint.MaxValue;
			long position=stream.Position;
			stream.Position=start;
			lock(syncroot) {
				fixed(uint* numRef=crc32_table){
					while(length > 0L) {
						int read=stream.Read(bufferA,0,BUFFSIZE);
						if(read==0)throw new StreamOverRunException("CRC32 ���v�Z���� StreamOverRun ���N�����܂����B");
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
					System.Console.WriteLine("�s��v���� ***** BUG *****");
					return;
				}
			}
			System.Console.WriteLine("OK: �s��v�͔������܂���ł���");
		}
		/// <summary>
		/// ��� Stream �̓��e������ł��邩�ǂ����𔻒肵�܂��B
		/// </summary>
		/// <param name="str1">��r������ Stream ���w�肵�܂��B</param>
		/// <param name="str2">��r������ Stream ���w�肵�܂��B</param>
		/// <returns>��� Stream �̓��e������ł���Ɣ��f�����ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
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
		//		�񓯊�������
		//===========================================================
		/// <summary>
		/// �w�肵����񂪁Aid3 �^�O�ɖ��ߍ��ލۂɔ񓯊�����K�v�Ƃ��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="str">�񓯊������K�v���ǂ������m�F�������f�[�^���i�[���Ă��� Stream ���w�肵�܂��B</param>
		/// <returns>�񓯊����������K�v�Ɣ��f���ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
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
		/// �񓯊����������ꂽ�f�[�^���A�񓯊�������O�̏�Ԃɖ߂��܂��B
		/// </summary>
		/// <param name="str">�񓯊����������ꂽ�f�[�^���i�[���Ă��� Stream ���w�肵�Ă��܂��B</param>
		/// <returns>�񓯊���������������̃f�[�^���i�[���Ă��� Stream ��Ԃ��܂��B
		/// Stream �̌��݈ʒu�͈�ԏ��߂ɐݒ肳��ĕԂ���܂��B</returns>
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
		/// �񓯊������������s���܂��B
		/// </summary>
		/// <param name="str">�񓯊����������s���Ώۂ̃f�[�^���i�[���Ă� Stream ���w�肵�܂��B</param>
		/// <returns>�񓯊������������s������̃f�[�^���i�[���Ă��� Stream ��Ԃ��܂��B
		/// Stream �̌��݈ʒu�͈�ԏ��߂ɐݒ肳��ĕԂ���܂��B</returns>
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
		//		byte* �� string
		//===========================================================
		/// <summary>
		/// �|�C���^�̎Q�Ɛ悩�� null �I�[�������ǂݎ��܂��B
		/// </summary>
		/// <param name="start">������̏����i�[���Ă���o�b�t�@�ւ̃|�C���^���w�肵�܂��B</param>
		/// <param name="length">�ǂݎ��f�[�^�̒����̏���A�܂�A�o�b�t�@�̃T�C�Y���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public static unsafe string PtrToTerminatedString(byte* start,int length) {
			string str=new string((sbyte*)start,0,length);
			int index=0;
			fixed(char* pwstr=str) while(index<length&&pwstr[index++]!='\0') ;
			return index==length?str:str.Substring(0,index);
		}
		/// <summary>
		/// �|�C���^�̎Q�Ɛ悩�� null �I�[�������ǂݎ��܂��B
		/// </summary>
		/// <param name="start">������̏����i�[���Ă���o�b�t�@�ւ̃|�C���^���w�肵�܂��B</param>
		/// <param name="length">�ǂݎ��f�[�^�̒����̏���A�܂�A�o�b�t�@�̃T�C�Y���w�肵�܂��B</param>
		/// <param name="enc">�ǂݎ��̂Ɏg�p���镶���R�[�h���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public static unsafe string PtrToTerminatedString(byte* start,int length,System.Text.Encoding enc) {
			string str=new string((sbyte*)start,0,length,enc);
			int index=0;
			fixed(char* pwstr=str) while(index<length&&pwstr[index++]!='\0') ;
			return index==length?str:str.Substring(0,index);
		}
	}

	/// <summary>
	/// �t�@�C�����̃f�[�^�P�ʂׂ̈̊�{�N���X�ł��B
	/// �t�@�C�����ɉ�����o�C�i���V�[�P���X���L���b�V�����܂��B
	/// �f�[�^�ɕύX���������ꍇ�ɂ����A�o�C�i���������s���܂��B
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
		/// ���e�ɕύX�������������L�^���܂��B
		/// ���̃��\�b�h���Ă΂ꂽ��ɏ��������s����ƁA�o�C�i���𐶐����܂��B
		/// </summary>
		protected void Changed(){this.changed=true;}
		/// <summary>
		/// ���̓��e�ɕύX�����������ǂ������擾���܂��B
		/// </summary>
		public bool IsChanged {
			get{return this.changed;}
		}

		/// <summary>
		/// Stream ����ǂݏo���ꍇ�ɂ́A���߂Ƀf�t�H���g�R���X�g���N�^���g�p���ď���������܂��B
		/// Stream ����̓ǂݏo���O�̏������ɁA�ǉ��I�ȏ�����K�v�Ƃ���ꍇ�́A�����ɏ������L�q���܂��B
		/// ����ł͉��̏������s���܂���B
		/// </summary>
		protected virtual void InitializeForRead(){}
		/// <summary>
		/// �h���N���X�ŁAStream ����̓ǂݏo�����������܂��B
		/// </summary>
		/// <remarks>�ǂݏo�����ʂ� null �̏ꍇ�ɂ͗�O 'new NullReturn()' �𓊂��ĉ������B
		/// CustomAccessor`1 �͂��̗�O���󂯎��� null ��ǂݏo�����ʂƂ��ĕԂ��܂��B</remarks>
		/// <param name="accessor">�ǂݏo�������i�[���Ă��� StreamAccessor ���w�肵�܂��B</param>
		protected abstract void Read(StreamAccessor accessor);
		/// <summary>
		/// �h���N���X�ŁAStream �ւ̏������������܂��B
		/// </summary>
		/// <param name="accessor">���̏������ StreamAccessor ���w�肵�܂��B</param>
		protected abstract void Write(StreamAccessor accessor);

		private sealed class CustomReader:Design.CustomReaderBase<FileDataCached>{
			private CustomReader(System.Type filedata_type){
				if(typeof(FileDataCached).IsAssignableFrom(filedata_type))
					throw new System.ArgumentException("�w�肵���^�� FileDataCached �̔h���^�ł͂���܂���B");
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
					throw new System.ArgumentException("�w�肵���^�� FileDataCached �̔h���^�ł͂���܂���B");
				if(filedata_type.IsAbstract||filedata_type.IsGenericTypeDefinition)
					throw new System.ArgumentException("�w�肵���^�̓C���X�^���X���o���܂���B");
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
		/// Stream ����̓ǂݏo���̌��ʂ� null �ł���ꍇ�� FileDataCached.Read ���瓊�����O�ł��B
		/// CustomAccessor`1 �œǂݍ���ł���r���ɔ��������ꍇ�ɂ� CustomAccessor`1 �� null ��Ԃ��܂��B
		/// </summary>
		public sealed class NullReturn:System.Exception{
			public NullReturn(){}
		}
	}
	/// <summary>
	/// ID3v2 �̃t���[���̓��e��ێ����� FileDataChached �̊�{�N���X�ł��B
	/// </summary>
	public abstract class FrameContent:FileDataCached{
		protected FrameContent(){}
		public FrameContent(System.IO.Stream stream):base(stream){}
	}
}
