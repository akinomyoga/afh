using Gen=System.Collections.Generic;
using System.Text;
using afh.Parse;

namespace afh.RegularExpressions{
	//================================================================
	//		���K�\���̍\�����
	//================================================================
	/// <summary>
	/// ���K�\���̓ǂݎ��̍ۂɔ���������O���X���[����ۂɎg�p���܂��B
	/// </summary>
	[System.Serializable]
	public class RegexParseException:System.FormatException{
		/// <summary>
		/// RegexParseException �̊���̃R���X�g���N�^�ł��B
		/// </summary>
		public RegexParseException():base(){}
		/// <summary>
		/// �w�肵�����b�Z�[�W���g�p���� RegexParseException �����������܂��B
		/// </summary>
		/// <param name="message">��O�Ɋւ���ڍׂȐ������w�肵�܂��B</param>
		public RegexParseException(string message):base(message){}
		/// <summary>
		/// �w�肵�����b�Z�[�W���g�p���� RegexParseException �����������܂��B
		/// </summary>
		/// <param name="message">��O�Ɋւ���ڍׂȐ������w�肵�܂��B</param>
		/// <param name="inner">���̗�O�𔭐������錴���ƂȂ���������O���w�肵�܂��B</param>
		public RegexParseException(string message,System.Exception inner):base(message,inner){}
		/// <summary>
		/// �V���A�������ꂽ RegexParseException �𕜌����܂��B
		/// </summary>
		/// <param name="info">�V���A���������f�[�^��ێ�����C���X�^���X���w�肵�܂��B</param>
		/// <param name="context">�V���A�����̊��ɂ��Ă̏����w�肵�܂��B</param>
		public RegexParseException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context
			):base(info,context)
		{}
	}
	/// <summary>
	/// �R�}���h�̐��K�\���Ƃ��Ă̐�����񋟂��܂��B
	/// </summary>
	public class CommandData{
		private CommandArgumentType arg_type=CommandArgumentType.None;
		/// <summary>
		/// �R�}���h�\���̈����̌`�����擾���܂��B
		/// </summary>
		public CommandArgumentType ArgumentType{
			get{return this.arg_type;}
		}

		/// <summary>
		/// �w�肵�������̌`�����g�p���� CommandData �����������܂��B
		/// </summary>
		/// <param name="argumentType">���K�\���ɉ���������̌`�����w�肵�܂��B</param>
		public CommandData(CommandArgumentType argumentType){
			this.arg_type=argumentType;
		}
		/// <summary>
		/// ���������Ȃ��R�}���h��\������C���X�^���X�ł��B
		/// </summary>
		public static readonly CommandData NoArg=new CommandData(CommandArgumentType.None);
		/// <summary>
		/// {} �ɂ���Ĉ������󂯎��R�}���h��\������C���X�^���X�ł��B
		/// </summary>
		public static readonly CommandData Brace=new CommandData(CommandArgumentType.Brace);
	}
	/// <summary>
	/// �R�}���h�̈����̌`��\������̂Ɏg�p���܂��B
	/// </summary>
	public enum CommandArgumentType{
		/// <summary>
		/// ���������Ȃ��R�}���h�ł��B
		/// </summary>
		None,
		/// <summary>
		/// �R�}���h���g���ʂň͂܂ꂽ��������鎖�������܂��B
		/// </summary>
		Brace,		// {}
		/// <summary>
		/// �R�}���h���l�p���� [] �ň͂܂ꂽ��������鎖�������܂��B
		/// </summary>
		Bracket,	// []
		/// <summary>
		/// �R�}���h���p���ʂň͂܂ꂽ��������鎖�������܂��B
		/// </summary>
		Angle,		// <>
		/// <summary>
		/// �R�}���h���V���O���N�I�e�[�V�����ň͂܂ꂽ��������鎖�������܂��B
		/// </summary>
		Quotation,	// ''
		/// <summary>
		/// �R�}���h���\�i�����������Ɏ�鎖�������܂��B
		/// </summary>
		Decimal,	// \d+
		/// <summary>
		/// �R�}���h���\�Z�i�����������Ɏ�鎖�������܂��B
		/// </summary>
		HexaDecimal,// \p{xdigit}+
	}

	/// <summary>
	/// ���K�\���̕��@���`����̂Ɏg�p���܂��B
	/// </summary>
	public interface IRegexGrammar{
		/// <summary>
		/// �o�^����Ă��邻�ꂼ��̃R�}���h�ɂ��Ă̏���񋟂��܂��B
		/// </summary>
		Gen::Dictionary<string,CommandData> CommandSet{get;}
		/// <summary>
		/// �R�����Ŏn�܂�R�}���h��L���ɂ��邩�ۂ����擾���܂��B
		/// </summary>
		bool EnablesColonCommand{get;}
		/// <summary>
		/// �R�����Ŏn�܂�R�}���h�ɂ��Ă̏���񋟂��܂��B
		/// EnablesColonCommand �� false �̏ꍇ�ɂ� null ��Ԃ��č\���܂���B
		/// </summary>
		Gen::Dictionary<string,CommandData> ColonCommandSet{get;}
	}
	internal partial class RegexScannerA:AbstractWordReader{
		private string flags="imnsx"; // TODO: Parser �Ɉ˂��ĕύX�ł���悤�ɂ���B
		private IRegexGrammar grammar;

		private string value;
		public string Value{
			get{return this.value;}
		}
		public RegexScannerA(string text,IRegexGrammar grammar):base(text){
			this.grammar=grammar;
		}

		private static WordType WtCharClass=WordType.Literal;
		private static WordType WtCommand=WordType.Element;
		private static WordType WtCommandC=WordType.Tag;

		public const int WT_COMMAND=WordType.vElement;
		public const int WT_CHARCLASS=WordType.vLiteral;
		public const int WT_COMMAND_C=WordType.vTag;
	}
	//================================================================
	//		���K�\���̓K�p�Ώ�
	//================================================================
	/// <summary>
	/// ����̌^�̘A������C���X�^���X�ɂ��Ĉ����܂��B
	/// </summary>
	/// <typeparam name="T">����ł���C���X�^���X�̌^���w�肵�܂��B</typeparam>
	public interface ITypedStream<T>{
		/// <summary>
		/// ���݂̈ʒu���擾���͐ݒ肵�܂��B
		/// ���̒l�͌��݂̈ʒu�� Stream �O�ɂ��鎖�������܂��B
		/// �A�����͕K�������v������܂���B
		/// </summary>
		int Index{get;set;}
		/// <summary>
		/// ���̈ʒu�Ɉړ����܂��B
		/// ���� Index �͕K���������݂� Index+1 �ł���K�v�͂���܂���B
		/// (�Ⴆ�΁ASurrogate ���̕����΂��ꍇ�Ȃǂ��l�����܂��B)
		/// </summary>
		/// <returns>�V�����ʒu��Ԃ��܂��B</returns>
		int MoveNext();
		/// <summary>
		/// ���݂̒l���擾���܂��B
		/// </summary>
		T Current{get;}
		/// <summary>
		/// Stream �̖��[�ɒB���Ă��邩�ǂ������擾���܂��B
		/// Stream �̊O�ɂ���ꍇ�� true ��Ԃ��܂��B
		/// Stream �̒��ɂ��Č��݂̒l���L���ł���ꍇ�� false ��Ԃ��܂��B
		/// </summary>
		bool EndOfStream{get;}
		/// <summary>
		/// �o�b�N�g���b�L���O�ׂ̈̃N���[���쐬���s���܂��B
		/// </summary>
		/// <returns>�쐬�����N���[����Ԃ��܂��B</returns>
		ITypedStream<T> Clone();
		//------------------------------------------------------------
		//	�V�K�ǉ�
		//------------------------------------------------------------
		/// <summary>
		/// ���݃X�g���[���̊J�n�ʒu�ɂ��邩�ۂ����擾���܂��B
		/// </summary>
		bool IsStart{get;}
		/// <summary>
		/// �O�̈ʒu�Ɉړ����܂��B
		/// </summary>
		/// <returns>�ړ�������̏ꏊ���w�肷��ԍ���Ԃ��܂��B</returns>
		int MovePrev();
		/// <summary>
		/// �O�̈ʒu�ɂ���l���擾���܂��B
		/// </summary>
		T Previous{get;}
	}

	/// <summary>
	/// �z��� ITypedStream �̃A�_�v�^�ł��B
	/// </summary>
	public class ArrayStreamAdapter<T>:ITypedStream<T>{
		private readonly T[] data;
		private int index;
		/// <summary>
		/// ArrayStreamAdapter �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="data">�ǂݎ��Ώۂ̔z����w�肵�܂��B</param>
		public ArrayStreamAdapter(T[] data):this(data,0){}
		/// <summary>
		/// ArrayStreamAdapter �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="data">�ǂݎ��Ώۂ̔z����w�肵�܂��B</param>
		/// <param name="index">�����̓ǂݎ��ʒu���w�肵�܂��B</param>
		public ArrayStreamAdapter(T[] data,int index){
			this.index=index;
			this.data=data;
		}
		/// <summary>
		/// ���̃C���X�^���X�������ɕێ����Ă���z����擾���܂��B
		/// </summary>
		public T[] Array{
			get{return this.data;}
		}
		//============================================================
		//		ITypedStream
		//============================================================
		/// <summary>
		/// ���݂̓ǂݎ��ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public int Index{
			get{return this.index;}
			set{this.index=value;}
		}
		/// <summary>
		/// ���݂̈ʒu������̈ʒu�֐i�߂܂��B
		/// </summary>
		/// <returns>�V�����ʒu��Ԃ��܂��B</returns>
		public int MoveNext(){return ++this.index;}
		/// <summary>
		/// ���݂̈ʒu����O�̈ʒu�֖߂��܂��B
		/// </summary>
		/// <returns>�V�����ʒu��Ԃ��܂��B</returns>
		public int MovePrev(){
			if(index>0)index--;
			return index;
		}
		/// <summary>
		/// ���݂̗v�f�̒l��Ԃ��܂��B
		/// </summary>
		public T Current{
			get {return this.data[this.index];}
		}
		/// <summary>
		/// �O�̈ʒu�ɂ���l��Ԃ��܂��B
		/// </summary>
		public T Previous{
			get{return this.data[this.index-1];}
		}
		/// <summary>
		/// Stream �̖��[�ɂ��邩�ۂ����擾���܂��B
		/// </summary>
		public bool EndOfStream{
			get{return this.index>=this.data.Length;}
		}
		/// <summary>
		/// Stream �̐擪�ɂ��邩�ۂ����擾���܂��B
		/// </summary>
		public bool IsStart{
			get{return this.index<=0;}
		}
		/// <summary>
		/// ArrayStreamAdapter �̃N���[�����쐬���܂��B
		/// (�N���[�������̂͌��݈ʒu��񂾂��ł����āA�����z��C���X�^���X�ւ̎Q�Ƃ�ێ����܂��B)
		/// </summary>
		/// <returns>�N���[����Ԃ��܂��B</returns>
		public ITypedStream<T> Clone(){
			return new ArrayStreamAdapter<T>(this.data,this.index);
		}
	}

	/// <summary>
	/// �����񂩂� ITypedStream �ւ̃A�_�v�^�ł��B
	/// </summary>
	public class StringStreamAdapter:ITypedStream<char>{
		int index;
		readonly string text;
		/// <summary>
		/// StringStreamAdapter �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="text">�ǂݎ��Ώۂ̕�������w�肵�܂��B</param>
		/// <param name="index">��������̏����ʒu���w�肵�܂��B</param>
		public StringStreamAdapter(string text,int index){
			this.text=text;
			this.index=index;
		}
		/// <summary>
		/// StringStreamAdapter �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="text">�ǂݎ��Ώۂ̕�������w�肵�܂��B</param>
		public StringStreamAdapter(string text):this(text,0){}
		/// <summary>
		/// ���̃C���X�^���X���ێ����Ă��镶������擾���܂��B
		/// </summary>
		public string ContentText{
			get{return this.text;}
		}
		/// <summary>
		/// ������������擾���܂��B
		/// </summary>
		/// <param name="start">����������̊J�n�ʒu���w�肵�܂��B</param>
		/// <param name="end">����������̏I�[���w�肵�܂��B�Ō�̕����̎��̈ʒu���w�肵�܂��B</param>
		/// <returns>�؂�o���������������Ԃ��܂��B</returns>
		public string Substr(int start,int end){
			return this.text.Substring(start,end-start);
		}

		/// <summary>
		/// ���݈ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public int Index{
			get {return this.index;}
			set {this.index=value;}
		}
		/// <summary>
		/// ���̕����Ɉړ����܂��B
		/// </summary>
		/// <returns>�ړ���̈ʒu��Ԃ��܂��B</returns>
		public int MoveNext(){
			return ++this.index;
		}
		/// <summary>
		/// �O�̕����Ɉړ����܂��B
		/// </summary>
		/// <returns>�ړ���̈ʒu��Ԃ��܂��B</returns>
		public int MovePrev(){
			if(index>0)index--;
			return this.index;
		}
		/// <summary>
		/// ���݂̈ʒu�ɂ��镶����Ԃ��܂��B
		/// </summary>
		public char Current{
			get{return this.text[this.index];}
		}
		/// <summary>
		/// �O�̈ʒu�ɂ��镶����Ԃ��܂��B
		/// </summary>
		public char Previous{
			get{return this.text[this.index-1];}
		}
		/// <summary>
		/// ������̏I���ɒB�������ۂ����擾���܂��B
		/// </summary>
		public bool EndOfStream{
			get {return this.index>=this.text.Length;}
		}
		/// <summary>
		/// ���݈ʒu��������̐�[�ɂ��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		public bool IsStart{
			get{return this.index==0;}
		}
		/// <summary>
		/// StringStreamAdapter �̃R�s�[���쐬���܂��B
		/// </summary>
		/// <returns>�쐬�����R�s�[��Ԃ��܂��B</returns>
		public ITypedStream<char> Clone(){
			return new StringStreamAdapter(this.text,this.index);
		}
	}

#if USE_RANGELIST
	public class RangeList<T>:Gen::ICollection<T>{
		private int count;
		private T[] data;
		public RangeList(){
			this.count=0;
			this.data=new T[4];
		}
		public RangeList(RangeList<T> copye){
			int count=copye.count;
			this.count=count;
			this.data=new T[count+4];
			for(int i=0;i<count;i++)
				this.data[i]=copye.data[i];
		}
		private void EnsureIndex(int index){
			if(index<this.data.Length)return;
			T[] newdata=new T[index*2];
			for(int i=0;i<this.count;i++)
				newdata[i]=this.data[i];
			this.data=newdata;
		}
		/// <summary>
		/// �V�����v�f�𖖔��ɒǉ����܂��B
		/// </summary>
		/// <param name="item">�ǉ�����v�f���w�肵�܂��B</param>
		public void Add(T item){
			EnsureIndex(this.count);
			this.data[this.count++]=item;
		}
		/// <summary>
		/// �o�^���ꂽ�v�f��S�č폜���܂��B
		/// <!--(�C���f�b�N�X�𖳌��ɂ��邾���Ȃ̂ŎQ�Ƃ͎c��܂��B
		/// �]���ēo�^�������e�͒Z���ł��邪�A���̃��X�g���̂͒����ł���
		/// �Ƃ����l�Ȏg����������ƃ�������Q���\��������܂��B)-->
		/// </summary>
		public void Clear(){
			this.count=0;
		}
		/// <summary>
		/// ���ݓo�^����Ă���v�f�̐����w�肵�܂��B
		/// </summary>
		public int Count{
			get{return this.count;}
		}
		public bool IsReadOnly{
			get{return false;}
		}
		/// <summary>
		/// ���e�̗񋓎q���擾���܂��B
		/// (�p�t�H�[�}���X�̖�肩��r���̃R���N�V�����̓��e�ύX�ɂ͑Ή����Ă��܂���B)
		/// </summary>
		/// <returns>�񋓎q��Ԃ��܂��B</returns>
		public System.Collections.Generic.IEnumerator<T> GetEnumerator(){
			int iM=this.count;
			for(int i=0;i<iM;i++)
				yield return this.data[i];
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
		public void CopyTo(T[] array,int arrayIndex){
			for(int i=0;i<this.count;i++)
				array[arrayIndex++]=data[i];
		}
		//===========================================================
		//		�ǉ����\�b�h
		//===========================================================
		public void ClearReference(){
			int i=this.count,len=data.Length;
			while(i<len)
				this.data[i++]=default(T);
		}
		/// <summary>
		/// �w�肵���ԍ��ȍ~�̗v�f���폜���܂��B
		/// �w�肵���ԍ��ɂ���v�f���폜���܂��B
		/// </summary>
		/// <param name="index">�ŏ��̍폜����v�f�̔ԍ����w�肵�܂��B</param>
		public void RemoveAfter(int index){
			for(int i=index,len=data.Length;i<len;i++)
				this.data[i]=default(T);
			this.count=index;
		}
		public T[] ToArray(){
			T[] ret=new T[this.count];
			for(int i=0;i<ret.Length;i++)
				ret[i]=this.data[i];
			return ret;
		}

		public bool Contains(T item){
			throw new System.Exception("The method or operation is not implemented.");
		}
		public bool Remove(T item) {
			throw new System.Exception("The method or operation is not implemented.");
		}
	}
#endif
}