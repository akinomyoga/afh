namespace afh.File.ID3v2_3_{
	// BinarySubstr �ɑ΂���v��
	//   �o���邾�� byte[] �C���X�^���X�𐶐����Ȃ��l�ɂ��Ă��̕�����ɑ΂���Q�Ƃ��쐬����B
	//   �ǂݎ��ɘA��Ĕ͈͂̊J�n�ʒu���ړ����鎖���o����l�ɂ���B(��� BinarySubstr ��p���Ď��X�ɓǂݍ���)
	//   ���鎞�_�ł͈̔͂�ۑ�����B (��ŁA�ǂݎ�蒼������A�x���Ǎ������s����ׂɕK�v)
	// ���N���X�ɏ��i
	/// <summary>
	/// byte[] �̈���͈͂ɑ΂���Q�Ƃ��Ǘ����܂��B
	/// </summary>
	public class BinarySubstr:System.ICloneable{
		/// <summary>
		/// �f�[�^�͈̔͂̊J�n�ʒu��ێ����܂��B
		/// </summary>
		public int offset;
		/// <summary>
		/// �f�[�^�͈̔͂̒�����ێ����܂��B
		/// </summary>
		public int length;
		/// <summary>
		/// �f�[�^�𕔕��Ɏ��� byte[] �C���X�^���X��ێ����܂��B
		/// </summary>
		public byte[] array;

		/// <summary>
		/// �͈͂̊J�n�ʒu���擾���͐ݒ肵�܂��B���[�ʒu�ɂ͕ύX���Ȃ��l�ɐݒ肳��܂��B
		/// </summary>
		public int start{
			get{return this.offset;}
			set{this.length+=this.offset-value;this.offset=value;}
		}
		/// <summary>
		/// �͈̖͂��[�ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public int end{
			get{return this.offset+this.length;}
			set{this.length=value-this.offset;}
		}

		public byte this[int index]{
			get{return this.array[this.offset+index];}
		}

		/// <summary>
		/// CRC32 �l���v�Z���܂��B
		/// </summary>
		/// <returns>�v�Z���� CRC32 �l��Ԃ��܂��B</returns>
		public unsafe uint CalculateCRC32(){
			const uint seed=0;	// ���ʂ� 0 ?
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
		/// �񓯊����������s���܂��B
		/// </summary>
		/// <returns>�񓯊���������������̃f�[�^��Ԃ��܂��B</returns>
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
		//		�R���X�g���N�^
		//=================================================
		/// <summary>
		/// BinarySubstr �̃R���X�g���N�^�B�w�肵�������g�p���� BinarySubstr �����������܂��B
		/// array �S�̂��f�[�^�͈̔͂Ƃ��܂��B
		/// </summary>
		/// <param name="array">�Q�Ɛ�̏����܂� byte[] �C���X�^���X���w�肵�܂��B</param>
		public BinarySubstr(byte[] array):this(array,0,array.Length){}
		/// <summary>
		/// BinarySubstr �̃R���X�g���N�^�B�w�肵�������g�p���� BinarySubstr �����������܂��B
		/// �w�肵���J�n�ʒu���� array �̖��[�܂ł�͈͂Ƃ��܂��B
		/// </summary>
		/// <param name="array">�Q�Ɛ�̏����܂� byte[] �C���X�^���X���w�肵�܂��B</param>
		/// <param name="offset">�f�[�^�� array ���ł̊J�n�ʒu���w�肵�܂��B</param>
		public BinarySubstr(byte[] array,int offset):this(array,offset,array.Length-offset){}
		/// <summary>
		/// BinarySubstr �̃R���X�g���N�^�B�w�肵�������g�p���� BinarySubstr �����������܂��B
		/// </summary>
		/// <param name="array">�Q�Ɛ�̏����܂� byte[] �C���X�^���X���w�肵�܂��B</param>
		/// <param name="offset">�f�[�^�� array ���ł̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="length">�f�[�^�� array ���ł̒������w�肵�܂��B</param>
		public BinarySubstr(byte[] array,int offset,int length){
			this.array=array;
			this.offset=offset;
			this.length=length;
			this.index=0;
		}
		//=================================================
		//		�ǂݎ��
		//=================================================
		/// <summary>
		/// ���݂̓ǂݎ��ʒu��ێ����܂��B
		/// </summary>
		public int index;
		/// <summary>
		/// ���݂̓ǂݎ��ʒu�����������܂��B
		/// </summary>
		public void Clear(){this.index=0;}
		/// <summary>
		/// ���݂̓ǂݎ��ʒu�ւ� byte* ���擾���܂��B
		/// �K�� array �� fixed �̏�ԂŎg�p���ĉ������B
		/// </summary>
		public unsafe byte* CurrentPtr{
			get{fixed(byte* r=&this.array[this.offset+this.index])return r;}
		}
		/// <summary>
		/// �ǂݎ��͈͂̏I�[�ւ� byte* ���擾���܂��B
		/// �K�� array �� fixed �̏�ԂŎg�p���ĉ������B
		/// </summary>
		public unsafe byte* ptrEnd{
			get{fixed(byte* r=&this.array[this.offset+this.length])return r;}
		}
		/// <summary>
		/// �w�肵�������͈̔͂̃f�[�^��ǂݎ��܂��B
		/// </summary>
		/// <param name="rangeLength">�ǂݎ��f�[�^�̒������w�肵�܂��B</param>
		/// <returns>�ǂݎ�����͈͂� BinarySubstr �Ƃ��ĕԂ��܂��B</returns>
		public BinarySubstr ReadRange(int rangeLength){
			BinarySubstr r=new BinarySubstr(this.array,this.offset+this.index,rangeLength);
			this.index+=rangeLength;
			return r;
		}
		/// <summary>
		/// ���݈ʒu����Ō㖘�͈̔͂̃f�[�^��ǂݎ��܂��B
		/// </summary>
		/// <returns>�ǂݎ�����͈͂� BinarySubstr �Ƃ��ĕԂ��܂��B</returns>
		public BinarySubstr ReadRangeToEnd(){
			return this.ReadRange(this.length-this.index);
		}
		/// <summary>
		/// 1B �ǂݎ��܂��B
		/// </summary>
		/// <returns>�ǂݎ���� byte ��Ԃ��܂��B</returns>
		public byte ReadByte(){
			return this[this.index++];
		}
		//=================================================
		//		������ǂݎ��
		//=================================================
		/// <summary>
		/// ���݂̈ʒu����Ō㖘�������ǂݎ��܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��Ɏg�p���� Encoding ���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadTextToEnd(System.Text.Encoding enc){
			string r=(enc??this.GetEncoding()).GetString(this.array,this.offset+this.index,this.length-this.index);
			this.index=this.length;
			return r;
		}
		/// <summary>
		/// Null �I�[�������ǂݎ��܂��B
		/// �ǂݎ�����������A���̃C���X�^���X�̗L���͈͂̊J�n�ʒu���ɂɐi�߂܂��B
		/// </summary>
		/// <param name="enc">������̓ǂݎ��Ɏg�p���镶���G���R�[�f�B���O���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ReadTextNullTerminated(System.Text.Encoding enc){
			if(enc==null)enc=this.GetEncoding();

			//-- null �������̒����𒲂ׂ�
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
		/// ���͈̔͂̃f�[�^�S�̂𕶎���ɕϊ����܂��B
		/// </summary>
		/// <param name="enc">�ǂݎ��Ɏg�p���� Encoding ���w�肵�܂��B</param>
		/// <returns>�ǂݎ�����������Ԃ��܂��B</returns>
		public string ToString(System.Text.Encoding enc){
			return (enc??this.GetEncoding()).GetString(this.array,this.offset,this.length);
		}
		/// <summary>
		/// BOM ���Q�Ƃ��� Encoding �𔻒肵�܂��BBOM �������ꍇ�ɂ� Encoding.Default ��Ԃ��܂��B
		/// </summary>
		/// <returns>�K���� Encoding �𔻒肵�ĕԂ��܂��B</returns>
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