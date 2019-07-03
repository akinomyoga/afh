using Generic=System.Collections.Generic;
namespace afh.Parse{
	/// <summary>
	/// ��͒��ɔ��������G���[���L�q���܂��B
	/// </summary>
	public class AnalyzeError{
		/// <summary>
		/// �G���[�̓��e��������镶�����ێ����܂��B
		/// </summary>
		public string message;
		/// <summary>
		/// ��͒��ɔ��������G���[���L�q���� AnalyzeError �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="message">�G���[�̓��e�Ɋւ���������w�肵�܂��B</param>
		public AnalyzeError(string message){this.message=message;}
	}
	/// <summary>
	/// ����������ǂݎ��N���X����������C���^�t�F�[�X�ł��B
	/// </summary>
	public interface ILetterReader{
		/// <summary>
		/// ���݂̕������擾���܂��B
		/// </summary>
		char CurrentLetter{get;}
		/// <summary>
		/// ���݂̕����̎�ނ��L�q���� LetterType ���擾���܂��B
		/// </summary>
		LetterType CurrentType{get;}
		/// <summary>
		/// ���̕����Ɉړ����܂��B
		/// </summary>
		/// <returns>���̕����ɐi�ގ����o�����ꍇ�� true ��Ԃ��܂��B�������������������ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		bool MoveNext();

		/// <summary>
		/// ���݂̈ʒu���w�肵���I�u�W�F�N�g�Ɋ֘A�t���ċL�^���܂��B
		/// </summary>
		/// <param name="key">�ʒu�����Ǘ�����ׂ̖��D�ƂȂ�ׂ��I�u�W�F�N�g���w�肵�܂��B</param>
		void StoreCurrentPos(object key);
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�Ɋ֘A�t����ꂽ�ʒu�Ɉړ����܂��B
		/// </summary>
		/// <param name="key">�ړ���̈ʒu���Q�Ƃ���I�u�W�F�N�g���w�肵�܂��B</param>
		void MoveToPos(object key);
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�Ɋ֘A�t����ꂽ�ʒu�Ɉړ����܂��B
		/// �I�u�W�F�N�g�Ɋ֘A�t����ꂽ�ʒu���́A�ړ���ɏ�����܂��B
		/// </summary>
		/// <param name="key">�ړ���̈ʒu���Q�Ƃ���I�u�W�F�N�g���w�肵�܂��B</param>
		void ReturnToPos(object key);
		/// <summary>
		/// ���ݕێ����Ă���ʒu�������ׂď������܂��B
		/// </summary>
		void ClearPositions();
		/// <summary>
		/// �ʒu�����R�s�[���܂��B
		/// </summary>
		/// <param name="sourceKey">�R�s�[���̈ʒu��񎯕ʃI�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="targetKey">�R�s�[��̈ʒu��񎯕ʃI�u�W�F�N�g���w�肵�܂��B</param>
		void CopyPosition(object sourceKey,object targetKey);

		/// <summary>
		/// ��͒��̃G���[��ݒ肵�܂��B
		/// </summary>
		/// <param name="message">�G���[�̓��e��������镶������w�肵�܂��B</param>
		/// <param name="start">�G���[�̌����ƂȂ�������������̊J�n�ʒu�Ɋ֘A�t����ꂽ�I�u�W�F�N�g���w�肵�܂��B
		/// null ���w�肵���ꍇ�ɂ͌��݈ʒu�ƌ��􂳂�܂��B</param>
		/// <param name="end">�G���[�̌����ƂȂ�������������̖��[�ʒu�Ɋ֘A�t����ꂽ�I�u�W�F�N�g���w�肵�܂��B
		/// null ���w�肵���ꍇ�ɂ͌��݈ʒu�ƌ��􂳂�܂��B</param>
		void SetError(string message,object start,object end);
	}
	/// <summary>
	/// �������P��s�Ƃ��Ĉ��������ǂݎ��I�u�W�F�N�g��\�����܂��B
	/// ������ɉ��s���܂߂鎖�͉\�ł����A�s�ԍ��E��ԍ��ɂ��A�N�Z�X�͏o���܂���B
	/// </summary>
	public interface ILinearLetterReader:ILetterReader{
		/// <summary>
		/// ��͑Ώۂ̕�������擾���͐ݒ肵�܂��B
		/// �������ݒ肵���ꍇ�ɂ́A���݂̉�͈ʒu�E�ێ����Ă���ʒu���͏���������܂��B
		/// </summary>
		string Text{get;set;}
		/// <summary>
		/// ��͑Ώۂ̕�����ƁA��͊J�n�ʒu��ݒ肵�܂��B
		/// </summary>
		/// <param name="text">��͑Ώۂ̕�������w�肵�܂��B</param>
		/// <param name="startIndex">��͂��J�n����ʒu���w�肵�܂��B</param>
		void SetText(string text,int startIndex);
		/// <summary>
		/// ���݂̈ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		int Index{get;set;}
	}
	/// <summary>
	/// ���`�����ǂݎ���ł��B
	/// ��� string �C���X�^���X�̓��e����s�̕�����Ƃ��ēǂݎ��܂��B
	/// (���s�R�[�h��ǂ܂Ȃ��Ƃ����Ӗ��ł͂Ȃ��A�����Ɂu�������ځv�ŃA�N�Z�X����Ƃ����Ӗ��ł��B
	/// �u���s����ځv�̕����Ƃ����A�N�Z�X�̎d���͂��܂���B)
	/// </summary>
	public class LinearLetterReader:ILinearLetterReader{
		/// <summary>
		/// �w�肵����������g�p���� LinearLetterReader �����������܂��B
		/// </summary>
		/// <param name="text">�ǂݍ��ޑΏۂ̕�������w�肵�܂��B</param>
		public LinearLetterReader(string text){this.Text=text;}
		/// <summary>
		/// �󕶎�����g�p���� LinearLetterReader �����������܂��B
		/// </summary>
		public LinearLetterReader():this(""){}
		//=================================================
		//		�ʒu�̋L�^�Ɋւ��镔��
		//=================================================
		private Generic::Dictionary<object,int> pos
			=new System.Collections.Generic.Dictionary<object,int>();
		/// <summary>
		/// ���݂̈ʒu���L�^���܂��B
		/// </summary>
		/// <param name="key">
		/// �ʒu�����ʂ���ׂ̃I�u�W�F�N�g���w�肵�܂��B
		/// 0 �͒P��̎n�܂���L�^����̂Ɏg�p���܂��B
		/// </param>
		public void StoreCurrentPos(object key){this.pos[key]=this.index;}
		/// <summary>
		/// ���ݕێ����Ă��镶������S�Ĕj�����܂��B
		/// </summary>
		public void ClearPositions(){this.pos.Clear();}
		/// <summary>
		/// �w�肵���ʒu�Ɍ��݂̈ʒu���ړ����܂��B
		/// </summary>
		/// <param name="key">�ړ���̈ʒu�����ʂ���ׂ̃I�u�W�F�N�g���w�肵�܂��B</param>
		public void MoveToPos(object key){
			if(!this.pos.ContainsKey(key))
				throw new System.ArgumentException("key","�w�肵�� key �ɑΉ�����ʒu���͑��݂��܂���B");
			this.Index=this.pos[key];
		}
		/// <summary>
		/// �w�肵���ʒu�Ɍ��݂̈ʒu���ړ����A�ړ�������̈ʒu�����폜���܂��B
		/// </summary>
		/// <param name="key">�ړ���̈ʒu�����ʂ���ׂ̃I�u�W�F�N�g���w�肵�܂��B</param>
		public void ReturnToPos(object key){
			if(!this.pos.ContainsKey(key))
				throw new System.ArgumentException("key","�w�肵�� key �ɑΉ�����ʒu���͑��݂��܂���B");
			this.Index=this.pos[key];
			this.pos.Remove(key);
		}
		/// <summary>
		/// ���݈ʒu�����[�ɒB���Ă��邩�ۂ����擾���܂��B
		/// </summary>
		public bool IsEndOfText{get{return this.index>=this.length;}}
		/// <summary>
		/// �ʒu�����R�s�[���܂��B
		/// </summary>
		/// <param name="sourceKey">�R�s�[���̈ʒu��񎯕ʃI�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="targetKey">�R�s�[��̈ʒu��񎯕ʃI�u�W�F�N�g���w�肵�܂��B</param>
		public void CopyPosition(object sourceKey,object targetKey){
			if(!this.pos.ContainsKey(sourceKey))
				throw new System.ArgumentException("sourceKey","�w�肵�� key �ɑΉ�����ʒu���͑��݂��܂���B");
			this.pos[targetKey]=this.pos[sourceKey];
		}
		//=================================================
		//		�ʒu�̋L�^�Ɋւ��镔��
		//=================================================
		/// <summary>
		/// LinearLetterReader �ɉ����ĕ���������͈̔͂��w�������̂Ɏg�p����N���X�ł��B
		/// </summary>
		public class TextRange:System.IComparable<TextRange>{
			/// <summary>
			/// ����������̊J�n�ʒu��ێ����܂��B
			/// </summary>
			public int start;
			/// <summary>
			/// ����������̖��[�ʒu��ێ����܂��B
			/// ���̃t�B�[���h�w�肳��镶���͊܂݂܂���B
			/// </summary>
			public int end;
			/// <summary>
			/// TextRange �̃C���X�^���X���쐬���܂��B
			/// </summary>
			/// <param name="start">����������̊J�n�ʒu���w�肵�܂��B</param>
			/// <param name="end">����������̏I�[�ʒu���w�肵�܂��B����ɂ���Ďw�肳��镶�����̂͊܂݂܂���B</param>
			public TextRange(int start,int end){
				this.start=start;
				this.end=end;
			}
			/// <summary>
			/// �ǂ���͈̔͂̕�����ɗ��邩�̏������擾���܂��B
			/// </summary>
			/// <param name="other">��r�Ώۂ̃C���X�^���X���w�肵�܂��B</param>
			/// <returns>
			/// ���̃C���X�^���X�̕����w�肵���C���X�^���X�����n�߂ɋ߂����Ɉʒu����Ȃ�ΐ��̒l��Ԃ��܂��B
			/// ���̃C���X�^���X�̕����w�肵���C���X�^���X�������[�ɋ߂����Ɉʒu����Ȃ�Ε��̒l��Ԃ��܂��B
			/// ���҂̕\�����镔��������͈̔͂����S�Ɉ�v����Ȃ�� 0 ��Ԃ��܂��B
			/// </returns>
			public int CompareTo(TextRange other){
				int r=this.start-other.end;
				return r!=0?r:this.end-other.end;
			}
		}
		/// <summary>
		/// ��͒��ɔ��������G���[��Z�߂ĕێ����܂��B
		/// </summary>
		public Generic::SortedList<TextRange,AnalyzeError> errors
			=new System.Collections.Generic.SortedList<TextRange,AnalyzeError>();
		/// <summary>
		/// �G���[��ݒ肵�܂��B
		/// </summary>
		/// <param name="message">�G���[�̓��e��������镶������w�肵�܂��B</param>
		/// <param name="start">
		/// �G���[�̊J�n�ʒu���ʎq���w�肵�܂��B
		/// �ڍׂ� CreateTextRange ���\�b�h�� start ���Q�Ƃ��ĉ������B
		/// </param>
		/// <param name="end">
		/// �G���[�̏I���ʒu���ʎq���w�肵�܂��B
		/// �ڍׂ� CreateTextRange ���\�b�h�� end ���Q�Ƃ��ĉ������B
		/// </param>
		public void SetError(string message,object start,object end){
			this.errors[this.CreateTextRange(start,end)]=new AnalyzeError(message);
		}
		/// <summary>
		/// ���݂܂łɓo�^���ꂽ�G���[��񋓂��܂��B
		/// </summary>
		/// <returns>�G���[�̗񋓎q��Ԃ��܂��B</returns>
		public Generic::IEnumerable<Generic::KeyValuePair<TextRange,AnalyzeError>> EnumErrors(){
			return this.errors;
		}
		//=================================================
		//		���ݎQ�ƈʒu�̊Ǘ�����
		//=================================================
		/// <summary>
		/// ���̕�����ǂݎ��܂��B
		/// </summary>
		/// <returns>�����Ɏ��̕�����ǂݎ�鎖���o�����ꍇ�� true ��Ԃ��܂��B
		/// ������̖��[�ɒB�����̕������Ȃ��ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool MoveNext(){
			this.Index++;
			return this.index<this.length;
		}
		/// <summary>
		/// ���ݎQ�Ƃ��Ă���ʒu���擾���͐ݒ肵�܂��B
		/// </summary>
		public int Index{
			get{return this.index;}
			set{
				this.index=value;
				if(this.index<this.length){
					this.ltype=LetterType.GetLetterType(this.text[this.index]);
				}else this.ltype=LetterType.Invalid;
			}
		}
		private int index;
		private int length;
		/// <summary>
		/// ������͈̔͂��w�肵�܂��B
		/// </summary>
		/// <param name="start">
		/// ������͈͂̊J�n�����̈ʒu�����Q�Ƃ���ׂ̎��ʃI�u�W�F�N�g���w�肵�܂��B
		/// ����́AStoreCurrentPos �� key �Ƃ��ēn�������ł��B
		/// null ���w�肵���ꍇ�ɂ͌��݂̈ʒu�𕶎���͈͂̊J�n�����Ƃ��܂��B
		/// </param>
		/// <param name="end">
		/// ������͈͂̏I�������̈ʒu�����Q�Ƃ���ׂ̎��ʃI�u�W�F�N�g���w�肵�܂��B
		/// ����́AStoreCurrentPos �� key �Ƃ��ēn�������ł��B
		/// null ���w�肵���ꍇ�ɂ͌��݂̈ʒu�𕶎���͈͂̏I�������Ƃ��܂��B
		/// </param>
		public TextRange CreateTextRange(object start,object end){
			int s=
				start!=null&&this.pos.ContainsKey(start)
				?this.pos[start]
				:this.index;
			int e=
				end!=null&&this.pos.ContainsKey(end)
				?this.pos[end]
				:this.index;
			if(e<s){int c=e;e=s;s=c;}
			return new TextRange(s,e);
		}
		//=================================================
		//		������f�[�^�̊Ǘ�����
		//=================================================
		/// <summary>
		/// ��͑Ώۂ̕�������擾���͐ݒ肵�܂��B
		/// </summary>
		public string Text{
			get{return this.text;}
			set{
				this.text=value;
				this.length=value.Length;
				this.Index=0;
				this.ClearPositions();
			}
		}
		private string text;
		/// <summary>
		/// ��͑Ώۂ̕�����Ɖ�͊J�n�ʒu��Z�߂Đݒ肵�܂��B
		/// </summary>
		/// <param name="text">�V������͑Ώۂ̕�������w�肵�܂��B</param>
		/// <param name="startIndex">��͊J�n�ʒu���w�肵�܂��B</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// startIndex �� text.Length �ȏ�ł���ꍇ�ɔ������܂��B��͊J�n�ʒu���w�肵��������̖��[���z�������Ɉʒu���Ă���ׂł��B
		/// </exception>
		public void SetText(string text,int startIndex){
			this.Text=text;
			// �� ���ꂪ����Ƌ󔒕������ݒ�o���Ȃ��B
			//if(startIndex>=this.length)
			//	throw new System.ArgumentOutOfRangeException("startIndex","�w�肵����͊J�n�ʒu�͕�����̖��[���z���Ă��܂��B");
			this.Index=startIndex;
		}
		//=================================================
		//		���݂̕����Ɋւ�����
		//=================================================
		/// <summary>
		/// ���݂̕������擾���܂��B
		/// </summary>
		public char CurrentLetter{
			get{return this.text[this.index];}
		}
		private LetterType ltype;
		/// <summary>
		/// ���݂̕����̎�ނɊւ������ێ����� LetterType ���擾���܂��B
		/// </summary>
		public LetterType CurrentType{
			get{return this.ltype;}
		}
		//=================================================
		//	LinearLetterReader �Ǝ��̎���
		//		ILetterReader �ɂ͍̗p����Ă��Ȃ�
		//		�����I�ɍ̗p����邩���m��Ȃ��B
		//=================================================
		/// <summary>
		/// ���݂̈ʒu�̎��ɂ��镶����ǂݎ��܂��B
		/// </summary>
		/// <param name="ch">�ǂݎ����������Ԃ��܂��B</param>
		/// <returns>���݂̈ʒu���I�[�ɒB���Ă���ꍇ�ɂ� false ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ͎��̕�����ǂݎ���� true ��Ԃ��܂��B</returns>
		public bool PeekChar(out char ch){
			if(this.index+1<this.text.Length){
				ch=this.text[this.index+1];
				return true;
			}else{
				ch='\0';
				return false;
			}
		}
	}
}