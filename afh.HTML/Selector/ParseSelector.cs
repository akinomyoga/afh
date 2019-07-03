using afh.Parse;
using Gen=System.Collections.Generic;
namespace afh.HTML.Selector{

	#region class Parser
	/// <summary>
	/// ������ŕ\���ꂽ Selector ��ǂݎ��܂��B
	/// </summary>
	internal sealed class Parser{
		private WordReader wreader;
		private AWordReader areader;
		private Rule rule;
		private SimpleSelector simple;
		public Parser(string text){
			this.wreader=new WordReader(text);
			this.areader=new AWordReader(this.wreader.LetterReader);
		}
		public Selector Parse(){
			System.Collections.Generic.List<SimpleSelector> simples=new System.Collections.Generic.List<SimpleSelector>();
			while(!this.wreader.LetterReader.IsEndOfText&&this.readSimple()){
				simples.Add(this.simple);
			}
			return new Selector(simples.ToArray());
		}
		//===========================================================
		//		readSimple : SimpleSelector ��ǂݎ��܂�
		//===========================================================
		/// <summary>
		/// SimpleSelector ��ǂݎ��܂��B
		/// �Ăяo����{wreader�ʒu: SimpleSelector ���\�����镨�̓��ň�ԏ��߂� word;}
		/// �Ăяo����{wreader�ʒu: ���ɏ��������ׂ��P��;}
		/// </summary>
		/// <returns>�Ǎ��ɐ��������ꍇ�� true ��Ǎ��Ɏ��s�����ꍇ�� false ��Ԃ��܂��B
		/// �ُ킪�����Ă��o���邾���ǂݍ��ނ悤�ɂ���ׁAfalse ���Ԃ���鎞�͕�����̖��[�ɂȂ�܂��B</returns>
		private bool readSimple(){
			this.simple.name=null;
			bool relat=false;
			do switch(this.wreader.CurrentType.value){
				case WordReader.vRelation:
					if(relat)goto next;
					relat=true;
					this.simple.relation=this.wreader.CurrentWord[0];
					break;
				case WordType.vIdentifier:
					if(!relat){
						relat=true;
						this.simple.relation=' ';
					}
					this.simple.name=this.wreader.CurrentWord;
					break;
				case WordReader.vOpenBra:
					if(!relat)this.simple.relation=' ';
					if(this.simple.name==null)this.simple.name="*";
					this.simple.rules=this.readSimple_attrs();
					return true;
			}while(this.wreader.ReadNext());
		next:
			if(this.simple.name==null)this.simple.name="*";
			this.simple.rules=new Rule[]{};
			return relat;
		}
		/// <summary>
		/// [] �ň͂܂ꂽ�����Ɉ˂� Selector �K����ǂݎ��܂��B
		/// �Ăяo����{areader�ʒu: "[";}
		/// �Ăяo����{wreader�ʒu: "]";}
		/// </summary>
		/// <returns>�ǂݎ�����K����Ԃ��܂��B</returns>
		private Rule[] readSimple_attrs(){
			System.Collections.Generic.List<Rule> rules=new System.Collections.Generic.List<Rule>();
			this.rule.varName=null;
			this.rule.value=null;
			while(this.areader.ReadNext_modeId()){
				if(this.readRule())rules.Add(this.rule);
				if(this.areader.CurrentType==WordType.Invalid){
					this.areader.LetterReader.SetError("�����l�K���ꗗ�ɏI�[ ']' ������܂���B",0,null);
					break;
				}
				switch(this.areader.CurrentWord[0]){
					case ']':
						this.wreader.ReadNext();
						goto ret;
					case ',':case '&':case '|':
					default:
						break;
				}
			}
		ret:
			return rules.ToArray();
		}
		
		//===========================================================
		//		readRule : �����I���K�� Rule ��ǂݎ��܂�
		//===========================================================
		private const string readRule_err11="�\�����ʉ��Z�q�ł��B���̉��Z�q�͖������܂��B";
		private const string readRule_err12="��Ɏ��ʎq���L�q���ĉ������B�����̋L�q�͖������܂��B";
		private const string readRule_err13="���߂̋K���Ŏ��ʎq�͏ȗ��o���܂���B���̔�r���Z�q�͖����������ɂ��܂��B";
		private const string readRule_err14="���߂̋K���Ŏ��ʎq�͏ȗ��o���܂���B���̏C���q�͖����������ɂ��܂��B";
		private const string readRule_err21="���ʎq�̒���ɕ����񂪗���͕̂ςł��B����͖������܂��B\r\n���ʎq�̌�ɂ͋K���̖��[���A��r���Z�q�����Ȃ���Έׂ�܂���B";
		private const string readRule_err22="���ʎq�𕡐��w�肷�鎖�͏o���܂���B����͖������܂��B\r\n���ʎq�̌�ɂ͋K���̖��[���A��r���Z�q�����Ȃ���Έׂ�܂���B";
		private const string readRule_err31="�yalgorithmicly unexpected�z\r\n���ʎq[+�ϐ��C���q] �̒����{0}������͕̂ςł��B����͖������܂��B\r\n���ʎq[+�ϐ��C���q] �̌�ɂ͋K���̖��[���A��r���Z�q�����Ȃ���Έׂ�܂���B";
		private const string readRule_err41="��r���Z�q�̌�ɖ���r���Z�q������͕̂ςł��B\r\n�������܂��B";
		private const string readRule_err42="�\�����ʉ��Z�q�ł��B\r\n�������܂��B";
		private const string readRule_err43="��r�Ώےl���ȗ�����Ă��܂��B��r�Ώےl�͋󔒂ƌ��􂵂܂��B";
		private const string readRule_err61="��r�Ώےl+�C���q �̌�ɂ͋K���̖��[�������鎖���o���܂���B�����̋L�q�͖�������܂��B";
		private enum readRule_ret{
			JumpTo1,DropFrom1,
			EnterIn2,JumpTo2,DropFrom2,
			EnterIn3,JumpTo3,DropFrom3,
			EnterIn4,JumpTo4,DropFrom4,
			EnterIn5,JumpTo5,DropFrom5,
			EnterIn6,JumpTo6,DropFrom6,
		}
		/// <summary>
		/// Rule ��ǂݎ��܂��B
		/// <para>
		/// this.rule �� varName �y�� value �ɂ͑O��ǂݎ�����K���̒l�������Ă��܂��B
		/// �V�����K���Q��ǂݎ��ۂɂ͗� field �� null �ɐݒ肵�ĉ������B
		/// [width&gt;=10&amp;&lt;=50] �� [width&gt;=10&amp;width&lt;=50] �Ɖ��߂��܂��B
		/// [href.l.s="hello"&amp;.l.m="how are you"] �� [href.l.s="hello"&amp;href.l.m="how are you"] �Ɖ��߂��܂��B
		/// </para>
		/// </summary>
		/// <returns>�ǂݎ�鎖���o�����ꍇ�� true ��ǂݎ��Ɏ��s�����ꍇ�� false ��Ԃ��܂��B</returns>
		private bool readRule(){
			readRule_ret ret=readRule_ret.JumpTo1;
			while(true)switch(ret){
				// �ϐ����Ǎ�
				case readRule_ret.JumpTo1:
					this.areader.mode=AWordReaderMode.ReadIdentifier;
					ret=this.readRule1();
					this.areader.mode=AWordReaderMode.Normal;
					break;
				case readRule_ret.DropFrom1:
					return false;
				// �ϐ��C���q�Ǎ�
				case readRule_ret.EnterIn2:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo2;
					else
						goto case readRule_ret.DropFrom2;
				case readRule_ret.JumpTo2:
					ret=this.readRule2();
					break;
				case readRule_ret.DropFrom2:
					this.rule.ope=' ';
					return true;
				// ��r���Z�q�Ǎ�
				case readRule_ret.EnterIn3:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo3;
					else
						goto case readRule_ret.DropFrom3;
				case readRule_ret.JumpTo3:
					ret=this.readRule3();
					break;
				case readRule_ret.DropFrom3:
					this.rule.ope=' ';
					return true;
				// ��r�Ώےl�Ǎ�
				case readRule_ret.EnterIn4:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo4;
					else
						goto case readRule_ret.DropFrom4;
				case readRule_ret.JumpTo4:
					ret=this.readRule4();
					break;
				case readRule_ret.DropFrom4:
					// �Ⴕ null �łȂ�������O��̒l���Q��
					if(this.rule.value==null){
						this.rule.value="";
						this.rule.valModifier="";
					}
					return true;
				// �Ώےl�C���q�Ǎ�
				case readRule_ret.EnterIn5:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo5;
					else
						goto case readRule_ret.DropFrom5;
				case readRule_ret.JumpTo5:
					ret=this.readRule5();
					break;
				case readRule_ret.DropFrom5:
					this.rule.valModifier="";
					return true;
				// ���ݓǂݐ؂�
				case readRule_ret.EnterIn6:
					if(this.areader.ReadNext())
						goto case readRule_ret.JumpTo6;
					else
						goto case readRule_ret.DropFrom6;
				case readRule_ret.JumpTo6:
					ret=this.readRule6();
					break;
				case readRule_ret.DropFrom6:
					return true;
			}
		}
		/// <summary>
		/// �ϐ����̎擾�����܂��B
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule1() {
			do switch(this.areader.CurrentType.value){
				case WordType.vIdentifier:
					this.rule.varName=this.areader.CurrentWord;
					return readRule_ret.EnterIn2;
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom1;
						case '~':case '<':case '>':case '=':case '!':case '��':case '��':
							if(this.rule.varName!=null){
								return readRule_ret.JumpTo3;
							}
							this.areader.LetterReader.SetError(readRule_err13,0,null);
							break;
						default:
							this.areader.LetterReader.SetError(readRule_err11,0,null);
							break;
					}
					break;
				case WordType.vModifier:
					if(this.rule.varName!=null){
						return readRule_ret.JumpTo2;
					}
					this.areader.LetterReader.SetError(readRule_err14,0,null);
					break;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(readRule_err12,0,null);
					break;
				case WordType.vElement:
					this.rule.varName=this.areader.CurrentWord;
					this.rule.varModifier="";
					this.rule.ope='��';
					this.rule.value=null;
					this.rule.valModifier="";
					return readRule_ret.EnterIn6;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom1;
		}
		/// <summary>
		/// �ϐ��ɑ΂���C���q�̎擾�����܂��B
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule2(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					this.rule.varModifier="";
					return readRule_ret.JumpTo3;
				case WordType.vModifier:
					rule.varModifier=this.areader.CurrentWord;
					return readRule_ret.EnterIn3;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(readRule_err21,0,null);
					break;
				case WordType.vIdentifier:
					this.areader.LetterReader.SetError(readRule_err22,0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom2;
		}
		/// <summary>
		/// ��r���Z�q�̎擾�����܂��B
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule3(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					char char1=this.areader.CurrentWord[0];
					switch(char1){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom3;
						case '~':case '=':case '!':case '��':
							rule.ope=char1;
							return readRule_ret.EnterIn4;
						case '<':
							rule.ope=this.areader.CurrentWord.Length==2?'��':'<';
							return readRule_ret.EnterIn4;
						case '>':
							rule.ope=this.areader.CurrentWord.Length==2?'��':'>';
							return readRule_ret.EnterIn4;
						case '��':
							rule.ope=char1;
							rule.value="";
							rule.valModifier="";
							return readRule_ret.EnterIn6;
						default:
							this.areader.LetterReader.SetError(readRule_err42,0,null);
							break;
					}
					break;
				// �ȉ��A���S���Y���㓞�B�s�\�̔������O�̈�
				case WordType.vModifier:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"�ēx�C���q"),0,null);
					break;
				case WordType.vLiteral:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"������"),0,null);
					break;
				case WordType.vIdentifier:
					this.areader.LetterReader.SetError(string.Format(readRule_err31,"���ʎq"),0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom3;
		}
		/// <summary>
		/// ��r�Ώےl�̎擾
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule4(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom4;
						case '~':case '<':case '>':case '=':case '!':case '��':case '��':
							this.areader.LetterReader.SetError(readRule_err41,0,null);
							break;
						default:
							this.areader.LetterReader.SetError(readRule_err42,0,null);
							break;
					}
					break;
				case WordType.vLiteral:
				case WordType.vIdentifier:
					this.rule.value=this.areader.CurrentWord;
					return readRule_ret.EnterIn5;
				case WordType.vModifier:
					if(this.rule.value==null){
						this.areader.LetterReader.SetError(readRule_err43,0,0);
						this.rule.value="";
					}
					return readRule_ret.JumpTo5;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom4;
		}
		/// <summary>
		/// ��r�Ώےl�C���q�̎擾
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule5(){
			do switch(this.areader.CurrentType.value){
				case WordType.vOperator:
					switch(this.areader.CurrentWord[0]){
						case ']':case ',':case '|':case '&':
							return readRule_ret.DropFrom5;
						default:
							this.rule.value+=this.areader.CurrentWord;
							break;
					}
					break;
				case WordType.vLiteral:
				case WordType.vIdentifier:
					this.rule.value+=this.areader.CurrentWord;
					break;
				case WordType.vModifier:
					this.rule.valModifier=this.areader.CurrentWord;
					return readRule_ret.EnterIn6;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom5;
		}
		/// <summary>
		/// ���݂̓ǂݐ؂�
		/// </summary>
		/// <returns>���̓�����w�肵�܂��B</returns>
		private readRule_ret readRule6(){
			do switch(this.areader.CurrentWord[0]){
				case ']':case ',':case '|':case '&':
					return readRule_ret.DropFrom6;
				default:
					this.areader.LetterReader.SetError(readRule_err61,0,null);
					break;
			}while(this.areader.ReadNext());
			return readRule_ret.DropFrom6;
		}
	}
	#endregion

	#region class WordReader
	/// <summary>
	/// �Z���N�^�̓ǂݎ��ׂ̈� wordreader �ł��B
	/// </summary>
	internal sealed class WordReader:afh.Parse.AbstractWordReader{
		public WordReader(string text):base(text){}
		//=======================================
		//		�萔
		//=======================================
		internal static readonly WordType wtOpenBra=new WordType(vOpenBra);
		internal static readonly WordType wtRelation=new WordType(vRelation);
		internal const int vOpenBra=0x100;
		internal const int vRelation=0x101;
		//=======================================
		//		�ǂݎ�胁�\�b�h
		//=======================================
		public override bool ReadNext(){
		#if MACRO_WORDREADER
			this.cword="";
			while([type].IsInvalid||[type].IsSpace)[if!next]return false;
			this.lreader.StoreCurrentPos(0);
			switch([type].purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
		#endif
			#region #OUT#
			this.cword="";
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace)if(!this.lreader.MoveNext())return false;
			this.lreader.StoreCurrentPos(0);
			switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
			#endregion #OUT#
		}
		private void readOperator(){
			this.wtype=WordType.Operator;
		#if MACRO_WORDREADER
			switch([letter]){
				case '@':this.readIdentifier();break;
				case '/':case '+':case '-':case '<':case '>':
					this.wtype=wtRelation;
					[add][nexit]
				case '[':
					this.wtype=wtOpenBra;
					[add][nexit]
				case '*':
					this.wtype=WordType.Identifier;
					[add][nexit]
				default:
					this.wtype=WordType.Invalid;
					[error:[letter]+"�͑Ή����Ă��Ȃ��L���ł��B�������܂��B"]
					[nexit]
			}
		#endif
			#region #OUT#
			switch(this.lreader.CurrentLetter){
				case '@':this.readIdentifier();break;
				case '/':case '+':case '-':case '<':case '>':
					this.wtype=wtRelation;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '[':
					this.wtype=wtOpenBra;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '*':
					this.wtype=WordType.Identifier;
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				default:
					this.wtype=WordType.Invalid;
					this.lreader.SetError(this.lreader.CurrentLetter+"�͑Ή����Ă��Ȃ��L���ł��B�������܂��B",0,null);
					this.lreader.MoveNext();return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// Selector �p�̎��ʎq��ǂݎ��܂��B
		/// \ ���G�X�P�[�v�����Ƃ��Ďg�p�o���܂��B
		/// �Ăяo������{
		///		����: ���ʎq�̕���|@
		///		�ʒu: �ꕶ���ڂ̕���
		/// }
		/// </summary>
		private void readIdentifier(){
			this.wtype=WordType.Identifier;
		#if MACRO_WORDREADER
			[add][next]
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					if([is:_]||[is:-])goto case LetterType.P_Token;
					if([not:\\])goto default;
					[next]
					goto case LetterType.P_Token;
				default:
					return;
			}
		#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					if(this.lreader.CurrentLetter=='_'||this.lreader.CurrentLetter=='-')goto case LetterType.P_Token;
					if(this.lreader.CurrentLetter!='\\')goto default;
					if(!this.lreader.MoveNext())return;
					goto case LetterType.P_Token;
				default:
					return;
			}
			#endregion #OUT#
		}
	}
	#endregion

	#region class AWordReader
	internal enum AWordReaderMode{Normal,ReadIdentifier}
	/// <summary>
	/// �����Z���N�^���̓ǂݎ��ׂ̈� wordreader �ł��B
	/// </summary>
	internal sealed class AWordReader:AbstractWordReader{
		public AWordReaderMode mode=AWordReaderMode.Normal;
		public AWordReader(LinearLetterReader lreader):base(lreader){}
		public bool ReadNext_modeId(){
			AWordReaderMode mode0=this.mode;
			this.mode=AWordReaderMode.ReadIdentifier;
			bool r=this.ReadNext();
			this.mode=mode0;
			return r;
		}
		public override bool ReadNext(){
		#if MACRO_WORDREADER
			this.cword="";
			this.wtype=WordType.Invalid;
			while([type].IsInvalid||[type].IsSpace)[if!next]return false;
			this.lreader.StoreCurrentPos(0);
			switch([type].purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
		#endif
			#region #OUT#
			this.cword="";
			this.wtype=WordType.Invalid;
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace)if(!this.lreader.MoveNext())return false;
			this.lreader.StoreCurrentPos(0);
			switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Operator:
					this.readOperator();
					break;
				case LetterType.P_Token:
					this.readIdentifier();
					break;
				case LetterType.P_Number:
					base.ReadNumber();
					break;
			}
			return true;
			#endregion #OUT#
		}
		private void readOperator(){
			this.wtype=WordType.Operator;
		#if MACRO_WORDREADER
			switch([letter]){
				//-------------------------------
				//		���Z�q
				//-------------------------------
				case '��':case '��': // �P�Ɖ��Z�q
					[add][nexit]
				case '!':case '~':case '=': // ��� = �����鉉�Z�q
					[add][next]
					if([is:=]){[add][nexit]}
					[error:"���̏� "+this.cword+" �́A��� = ������ꍇ�ɂ����Ή����Ă��܂���B"]
					this.cword+="=";
					break;
				case '<':case '>': // ��� = �����邩���m��Ȃ����Z�q
					[add][next]
					if([not:=])return;
					goto case ',';
				//-------------------------------
				//		��
				//-------------------------------
				case '@':this.readIdentifier();break;
				case '\'':
					this.ReadStringSQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '"':
					this.ReadStringDQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '{':
					if(this.mode!=AWordReaderMode.ReadIdentifier)goto default;
					this.ReadBracketRange();
					break;
				case ',':case '&':case '|':case ']':
					[add][nexit]
				case '.':
					[add][next]
					if([type].IsNumber){
						this.cword="";
						this.lreader.MoveToPos(0);
						this.ReadNumber();
					}
					this.readModifiers();
					break;
				default:
					this.wtype=WordType.Invalid;
					[error:[letter]+"�͑Ή����Ă��Ȃ��L���ł��B�������܂��B"]
					[add][nexit]
			}
		#endif
			#region #OUT#
			switch(this.lreader.CurrentLetter){
				//-------------------------------
				//		���Z�q
				//-------------------------------
				case '��':case '��': // �P�Ɖ��Z�q
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '!':case '~':case '=': // ��� = �����鉉�Z�q
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter=='='){this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;}
					this.lreader.SetError("���̏� "+this.cword+" �́A��� = ������ꍇ�ɂ����Ή����Ă��܂���B",0,null);
					this.cword+="=";
					break;
				case '<':case '>': // ��� = �����邩���m��Ȃ����Z�q
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!='=')return;
					goto case ',';
				//-------------------------------
				//		��
				//-------------------------------
				case '@':this.readIdentifier();break;
				case '\'':
					this.ReadStringSQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '"':
					this.ReadStringDQ();
					this.cword=convertStringLiteral(this.cword);
					break;
				case '{':
					if(this.mode!=AWordReaderMode.ReadIdentifier)goto default;
					this.ReadBracketRange();
					break;
				case ',':case '&':case '|':case ']':
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
				case '.':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentType.IsNumber){
						this.cword="";
						this.lreader.MoveToPos(0);
						this.ReadNumber();
					}
					this.readModifiers();
					break;
				default:
					this.wtype=WordType.Invalid;
					this.lreader.SetError(this.lreader.CurrentLetter+"�͑Ή����Ă��Ȃ��L���ł��B�������܂��B",0,null);
					this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
			}
			#endregion #OUT#
		}
		private string convertStringLiteral(string literal){
			return literal.Length<=2?""
				:literal
				.Substring(1,literal.Length-2)
				.Replace(@"\r","\r")
				.Replace(@"\n","\n")
				.Replace(@"\t","\t")
				.Replace(@"\v","\v")
				.Replace(@"\\","\\");
		}
		/// <summary>
		/// Selector �p�̎��ʎq��ǂݎ��܂��B
		/// \ ���G�X�P�[�v�����Ƃ��Ďg�p�o���܂��B
		/// �Ăяo������{
		///		����: ���ʎq�̕���|@
		///		�ʒu: �ꕶ���ڂ̕���
		/// }
		/// </summary>
		private void readIdentifier(){
			this.wtype=WordType.Identifier;
		#if MACRO_WORDREADER
			[add][next]
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					if([is:_]||[is:-])goto case LetterType.P_Token;
					if([not:\\])goto default;
					[next]
					goto case LetterType.P_Token;
				default:
					return;
			}
		#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					if(this.lreader.CurrentLetter=='_'||this.lreader.CurrentLetter=='-')goto case LetterType.P_Token;
					if(this.lreader.CurrentLetter!='\\')goto default;
					if(!this.lreader.MoveNext())return;
					goto case LetterType.P_Token;
				default:
					return;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// �����Z���N�^�Ȃǂ̏C���q��ǂݎ��܂��B
		/// �Ăяo������{
		///		����: . �̎��̕���
		///		������: "."
		/// }
		/// </summary>
		private void readModifiers(){
			this.wtype=WordType.Modifier;
			bool bra=false;
		#if MACRO_WORDREADER
			while(true)switch([type].purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					[add][next]
					break;
				case LetterType.P_Operator:
					switch([letter]){
						case '[':
							bra=true;
							break;
						case ']':
							if(!bra)return;
							bra=false;
							break;
						case '=':case '~':case '!':case '&':case '��':case '��':
						case '|':case '<':case '>':case ',':
							return;
					}
					[add][next]
					break;
				default:
					[next]
					break;
			}
		#endif
			#region #OUT#
			while(true)switch(this.lreader.CurrentType.purpose){
				case LetterType.P_Number:
				case LetterType.P_Token:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				case LetterType.P_Operator:
					switch(this.lreader.CurrentLetter){
						case '[':
							bra=true;
							break;
						case ']':
							if(!bra)return;
							bra=false;
							break;
						case '=':case '~':case '!':case '&':case '��':case '��':
						case '|':case '<':case '>':case ',':
							return;
					}
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				default:
					if(!this.lreader.MoveNext())return;
					break;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// {} �ň͂܂ꂽ�̈��ǂݎ��܂��B
		/// �Ăяo���O{�����ʒu:'{';}
		/// </summary>
		private void ReadBracketRange(){
			this.wtype=WordType.Element;
#if MACRO_WORDREADER
			[if!next]goto err;
			int count=0;
			while(true)switch([letter]){
				case '{':
					count++;
					goto default;
				case '}':
					count--;
					if(count<0){
						this.wtype=WordType.Element;
						[nexit]
					}
					goto default;
				case '\'':this.ReadStringSQ();break;
				case '"':this.ReadStringDQ();break;
				default:
					[add][if!next]goto err;
					break;
			}
		err:[error:"{} �̈�ɏI�[�� } �����݂��܂���B"]
#endif
			#region #OUT#
			if(!this.lreader.MoveNext())goto err;
			int count=0;
			while(true)switch(this.lreader.CurrentLetter){
				case '{':
					count++;
					goto default;
				case '}':
					count--;
					if(count<0){
						this.wtype=WordType.Element;
						this.lreader.MoveNext();return;
					}
					goto default;
				case '\'':this.ReadStringSQ();break;
				case '"':this.ReadStringDQ();break;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		err:this.lreader.SetError("{} �̈�ɏI�[�� } �����݂��܂���B",0,null);
			#endregion #OUT#
		}

	}
	#endregion

}