namespace afh.Parse{
	/// <summary>
	/// �ǂݎ�����P��̎�ނ�\������\���̂ł��B
	/// </summary>
	public struct WordType{
		/// <summary>
		/// �P��̎�ނ�ێ����܂��B
		/// </summary>
		public int value;
		/// <summary>
		/// �V���� WordType �����������܂��B
		/// </summary>
		/// <param name="value">
		/// WordType �����ʂ���ԍ��ł��B
		/// 0 ���� 255 ���͊��Ɏg�p���͗\�񂳂�Ă��܂��B�ʂ̕����g�p���ĉ������B
		/// </param>
		public WordType(int value){this.value=value;}
		//=================================================
		//		�萔
		//=================================================
		/// <summary>�����ȒP���\�����l�ł��B</summary>
		public const int vInvalid=0;
		/// <summary>�󔒂�\�����l�ł��B</summary>
		public const int vSpace=1;
		/// <summary>���ʎq��\�����l�ł��B</summary>
		public const int vIdentifier=2;
		/// <summary>���Z�q��\�����l�ł��B</summary>
		public const int vOperator=3;
		/// <summary>������\����\�����l�ł��B</summary>
		public const int vLiteral=4;
		/// <summary>�L�[���[�h��\�����l�ł��B</summary>
		public const int vKeyWord=5;
		/// <summary>�R�����g��\�����l�ł��B</summary>
		public const int vComment=6;
		/// <summary>�����ȒP���\�� WordType �ł��B</summary>
		public static readonly WordType Invalid=new WordType(0);
		/// <summary>�󔒂�\�� WordType �ł��B</summary>
		public static readonly WordType Space=new WordType(1);
		/// <summary>���ʎq��\�� WordType �ł��B</summary>
		public static readonly WordType Identifier=new WordType(2);
		/// <summary>���Z�q��\�� WordType �ł��B</summary>
		public static readonly WordType Operator=new WordType(3);
		/// <summary>������\����\�� WordType �ł��B</summary>
		public static readonly WordType Literal=new WordType(4);
		/// <summary>�L�[���[�h��\�� WordType �ł��B</summary>
		public static readonly WordType KeyWord=new WordType(5);
		/// <summary>�R�����g��\�� WordType �ł��B</summary>
		public static readonly WordType Comment=new WordType(6);
		/// <summary>�ړ�����\�����l�ł��B</summary>
		public const int vPrefix=0x10;
		/// <summary>�ڔ�����\�����l�ł��B</summary>
		public const int vSuffix=0x11;
		/// <summary>�C���q��\�����l�ł��B</summary>
		public const int vModifier=0x12;
		/// <summary>�������\�����l�ł��B</summary>
		public const int vText=0x13;
		/// <summary>���炩�̃^�O��\�����l�ł��B</summary>
		public const int vTag=0x14;
		/// <summary>���炩�̑�����\�����l�ł��B</summary>
		public const int vAttribute=0x15;
		/// <summary>���炩�̗v�f��\�����l�ł��B</summary>
		public const int vElement=0x16;
		/// <summary>�ړ�����\�� WordType �ł��B</summary>
		public static readonly WordType Prefix=new WordType(0x10);
		/// <summary>�ڔ�����\�� WordType �ł��B</summary>
		public static readonly WordType Suffix=new WordType(0x11);
		/// <summary>�C���q��\�� WordType �ł��B</summary>
		public static readonly WordType Modifier=new WordType(0x12);
		/// <summary>�������\�� WordType �ł��B</summary>
		public static readonly WordType Text=new WordType(0x13);
		/// <summary>���炩�̃^�O��\�� WordType �ł��B</summary>
		public static readonly WordType Tag=new WordType(0x14);
		/// <summary>���炩�̑�����\�� WordType �ł��B</summary>
		public static readonly WordType Attribute=new WordType(0x15);
		/// <summary>���炩�̗v�f��\�� WordType �ł��B</summary>
		public static readonly WordType Element=new WordType(0x16);
		//=================================================
		//		������ɕϊ�
		//=================================================
		/// <summary>
		/// ���݂̒P��̎�ނ�\����������擾���܂��B
		/// </summary>
		/// <returns>���݂̒P��̎�ނ�\���������Ԃ��܂��B</returns>
		public override string ToString() {
			if(names.ContainsKey(this.value))return names[this.value];
			return "�s��[value="+this.value.ToString()+"]";
		}
		private static System.Collections.Generic.Dictionary<int,string> names
			=new System.Collections.Generic.Dictionary<int,string>();
		static WordType(){
			names.Add(0,"Invalid");
			names.Add(1,"Space");
			names.Add(2,"Identifier");
			names.Add(3,"Operator");
			names.Add(4,"Literal");
			names.Add(5,"KeyWord");
			names.Add(6,"Comment");
			names.Add(0x10,"Prefix");
			names.Add(0x11,"Suffix");
			names.Add(0x12,"Modifier");
			names.Add(0x13,"Text");
			names.Add(0x14,"Tag");
			names.Add(0x15,"Attribute");
			names.Add(0x16,"Element");
		}
		//=================================================
		//		��r���Z
		//=================================================
		/// <summary>
		/// ���̃C���X�^���X�̕\���P��̎�ނɓ��L�� hash �l���擾���܂��B
		/// </summary>
		/// <returns>���̃C���X�^���X�̕\���P��̎�ނɓ��L�� hash �l��Ԃ��܂��B</returns>
		public override int GetHashCode() {
			return this.value.GetHashCode();
		}
		/// <summary>
		/// ���l��r���s���܂��B
		/// </summary>
		/// <param name="obj">��r�Ώۂ̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>obj �� WordType �̃C���X�^���X�ł����Ċ��\���P��̎�ނ����̃C���X�^���X�Ɠ����ł���ꍇ�� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public override bool Equals(object obj) {
			return obj.GetType()==typeof(WordType)&&((WordType)obj).value==this.value;
		}
		/// <summary>
		/// ���l��r���s���܂��B
		/// </summary>
		/// <param name="a">��r�̑Ώۂ̃C���X�^���X���w�肵�܂��B</param>
		/// <param name="b">��r�̑Ώۂ̃C���X�^���X���w�肵�܂��B</param>
		/// <returns>�\���P��̎�ނ������ۂ� true ��Ԃ��܂��B����ȊO�̎��ɂ� false ��Ԃ��܂��B</returns>
		public static bool operator ==(WordType a,WordType b){return a.value==b.value;}
		/// <summary>
		/// �s����r���s���܂��B
		/// </summary>
		/// <param name="a">��r�̑Ώۂ̃C���X�^���X���w�肵�܂��B</param>
		/// <param name="b">��r�̑Ώۂ̃C���X�^���X���w�肵�܂��B</param>
		/// <returns>�\���P��̎�ނ��قȂ�ۂ� true ��Ԃ��܂��B����ȊO�̎��ɂ� false ��Ԃ��܂��B</returns>
		public static bool operator !=(WordType a,WordType b){return !(a==b);}
	}
	/// <summary>
	/// ������Ȃǂ̑Ώۂ������ǂݎ��N���X�ł��B
	/// ���̓��e (System.String) �ƁA���̎�� (WordType) ��Ԃ��܂��B
	/// </summary>
	public interface IWordReader{
		/// <summary>
		/// ���݂̌��̓��e���擾���܂��B
		/// </summary>
		string CurrentWord{get;}
		/// <summary>
		/// ���݂̌��̎�ނ�\�� WordType ��Ԃ��܂��B
		/// </summary>
		WordType CurrentType{get;}
		/// <summary>
		/// ���̒P��Ɉړ����܂��B
		/// </summary>
		/// <returns>
		/// ���̒P�ꂪ�Ȃ������ꍇ�� false ��Ԃ��܂��B
		/// ����ȊO�̎��ɂ� true ��Ԃ��܂��B
		/// </returns>
		bool ReadNext();
	}
	/// <summary>
	/// LinearLetterReader ��ʂ��Č���ǂݎ��N���X�ł��BIWordReader ���������܂��B
	/// </summary>
	public abstract partial class AbstractWordReader:IWordReader{
		/// <summary>
		/// ���̒P��Ɉړ����܂��B
		/// </summary>
		/// <returns>
		/// ���̒P�ꂪ�Ȃ������ꍇ�� false ��Ԃ��܂��B
		/// ����ȊO�̎��ɂ� true ��Ԃ��܂��B
		/// </returns>
		public abstract bool ReadNext();
		/// <summary>
		/// AbstractWordReader �̃R���X�g���N�^�ł��B
		/// �w�肵������������ɂ��ĐV�����C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="text">�ǂݎ��Ώۂł��镶������w�肵�܂��B</param>
		protected AbstractWordReader(string text):this(new LinearLetterReader(text)){}
		/// <summary>
		/// AbstractWordReader �̃R���X�g���N�^�ł��B
		/// �w�肵�� LinearLetterReader �����ɂ��ĐV�����C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="lreader">�ǂݎ��Ώۂ� LinearLetterReader ���w�肵�܂��B</param>
		protected AbstractWordReader(LinearLetterReader lreader){
			this.lreader=lreader;
			this.cword="";
			this.wtype=WordType.Invalid;
		}
		//===========================================================
		//		fields
		//===========================================================
		/// <summary>
		/// �����̓ǂݎ��Ɏg�p���Ă��� LetterReader ���擾���܂��B
		/// </summary>
		public LinearLetterReader LetterReader{get{return lreader;}}
		/// <summary>
		/// �����̓ǂݎ��Ɏg�p���Ă��� LinearLetterReader ��ێ����Ă��܂��B
		/// </summary>
		protected LinearLetterReader lreader;
		/// <summary>
		/// ���݂̌���ێ����܂��B
		/// </summary>
		protected string cword;
		/// <summary>
		/// ���݂̌��̓��e���擾���܂��B
		/// </summary>
		public string CurrentWord{get{return this.cword;}}
		/// <summary>
		/// ���݂̌��̎�ނ�ێ����܂��B
		/// </summary>
		protected WordType wtype;
		/// <summary>
		/// ���݂̌��̎�ނ��擾���܂��B
		/// </summary>
		public WordType CurrentType{get{return this.wtype;}}
		//===========================================================
		//		reading methods
		//===========================================================
		/// <summary>
		/// �ʏ�̕����E�����E�A���_�[�X�R�A����\������鎯�ʎq��ǂݎ��܂��B
		/// </summary>
		protected void ReadIdentifier(){
			this.wtype=WordType.Identifier;
#if MACRO_WORDREADER
			[add][next]
			while([type].IsToken||[type].IsNumber||[is:_]){
				[add][next]
			}
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||lreader.CurrentLetter=='_'){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// �ʏ�̕����E�����y�юw�肵���L������\������鎯�ʎq��ǂݎ��܂��B
		/// </summary>
		/// <param name="acceptables">���ʎq�Ɋ܂߂鎖���o���镶�����w�肵�܂��B</param>
		protected void ReadIdentifier(params char[] acceptables){
			this.wtype=WordType.Identifier;
#if MACRO_WORDREADER
			[add][next]
			while(true){
				if([type].IsToken||[type].IsNumber||[is:_])goto add;
				for(int i=0;i<acceptables.Length;i++)if([letter]==acceptables[i])goto add;
				return;
			add:
				[add][next]
			}
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true){
				if(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||lreader.CurrentLetter=='_')goto add;
				for(int i=0;i<acceptables.Length;i++)if(this.lreader.CurrentLetter==acceptables[i])goto add;
				return;
			add:
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
			#endregion #OUT#

		}
	}
}