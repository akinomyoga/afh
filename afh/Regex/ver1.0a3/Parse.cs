using Gen=System.Collections.Generic;
using afh.Parse;
using RangeErrorPair=System.Collections.Generic.KeyValuePair<
	afh.Parse.LinearLetterReader.TextRange,
	afh.Parse.AnalyzeError
>;

namespace afh.RegularExpressions{
	public partial class RegexFactory3<T>{
		/// <summary>
		/// ���K�\���ǂݎ����s���N���X�̊�{�N���X�ł��B
		/// </summary>
		protected abstract class ParserBase{
			RegexScannerA scanner;

			/// <summary>
			/// ���K�\�����K�肷�鐶�����ێ����܂��B
			/// </summary>
			protected readonly RegexFactory3<T> parent;
			/// <summary>
			/// ��������w�肵�� ParserBase �����������܂��B
			/// </summary>
			/// <param name="parent">���K�\�����K�肷�鐶������w�肵�܂��B</param>
			protected ParserBase(RegexFactory3<T> parent){
				this.parent=parent;
			}

			/// <summary>
			/// �w�肵�����K�\�������߂��܂��B
			/// </summary>
			/// <param name="regex">���K�\�����L�q���镶������w�肵�܂��B</param>
			/// <returns>���߂̌��ʂƂ��ďo�����\����Ԃ��܂��B</returns>
			public INode Parse(string regex){
				this.scanner=new RegexScannerA(regex,this.GetGrammar());
				if(!this.scanner.ReadNext())return null;

				INode ret=Read();

				// �����ǂݎ���Ă��Ȃ��������c���Ă���ꍇ
				if(this.scanner.CurrentType!=WordType.Invalid){
					if(this.scanner.CurrentType==WordType.Operator&&this.scanner.CurrentWord==")"){
						scanner.LetterReader.SetError("�]���� ')' �ł��B",0,null);
					}
					__debug__.RegexParserAssert(false);
				}
				return ret;
			}
			/// <summary>
			/// ���K�\���̉��߂̍ۂɐ������G���[�̗񋓂��s���܂��B
			/// </summary>
			public Gen::IEnumerable<RangeErrorPair> Errors{
				get{
					if(this.scanner==null)return null;
					return this.scanner.LetterReader.EnumErrors();
				}
			}
			//========================================================
			//		�p���Ҍ���
			//========================================================
			/// <summary>
			/// ���̐��K�\���̕��@���`���� IRegexGrammar �C���X�^���X���擾���܂��B
			/// </summary>
			/// <returns>���̐��K�\���̕��@���`���Ă��� RegexGrammar �C���X�^���X��Ԃ��܂��B</returns>
			protected abstract IRegexGrammar GetGrammar();
			/// <summary>
			/// �R�}���h�ɑΉ����鐳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="name">�R�}���h�̖��O���w�肵�܂��B</param>
			/// <param name="arg">�R�}���h�̈������w�肵�܂��B</param>
			/// <returns>�쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected abstract INode ProcessCommand(string name,string arg);
			/// <summary>
			/// �R�����R�}���h�ɑΉ����鐳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="name">�R�����R�}���h�̖��O���w�肵�܂��B</param>
			/// <param name="arg">�R�����R�}���h�̈������w�肵�܂��B</param>
			/// <returns>�쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected abstract INode ProcessCommandC(string name,string arg);
			/// <summary>
			/// �������琳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="letter">���K�\���v�f�̌��ƂȂ镶�����w�肵�܂��B</param>
			/// <returns>�w�肵�����������ɍ쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected abstract INode ProcessLetter(char letter);
			/// <summary>
			/// �N���X�\�����琳�K�\���v�f�����܂��B
			/// </summary>
			/// <param name="content">�N���X�w�� [...] �̒��g���w�肵�܂��B</param>
			/// <returns>�w�肵������������ɍ쐬�������K�\���v�f��Ԃ��܂��B</returns>
			protected abstract INode ProcessClass(string content);
			/// <summary>
			/// �ǂݎ��̍ۂɐ������G���[��ݒ肵�܂��B
			/// </summary>
			/// <param name="message">�G���[�̓��e�ɂ��Đ������镶������w�肵�܂��B</param>
			protected void Error(string message){
				this.scanner.LetterReader.SetError(message,0,null);
			}
			//========================================================
			//		�ǂݎ��̊֐�
			//========================================================
			private INode ReadUntilCloseParen(){
				INode ret=Read();

				// �ǂݎ��I����Ă��Ȃ��̂ɏI�������ꍇ�B
				if(this.scanner.CurrentType==WordType.Invalid){
					this.Error("'(' �ɑΉ�����I���� ')' ������܂���B");
				}else{
					__debug__.RegexParserAssert(this.scanner.CurrentType==WordType.Operator&&this.scanner.CurrentWord==")");
					this.scanner.ReadNext();
				}

				if(ret==null){
					this.Error("���ʂ̒��g�����݂��܂���B");
				}

				return ret;
			}
			//========================================================
			/// <summary>
			/// '|' �ŋ�؂�ꂽ���B��ǂݎ��܂��B
			/// </summary>
			/// <returns>�ǂݎ���ďo�����m�[�h��Ԃ��܂��B�L���Ȑ��K�\���w�肪������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			private INode Read(){
				Gen::List<INode> nodes=new Gen::List<INode>();
				while(true)switch(this.scanner.CurrentType.value) {
					case WordType.vInvalid:
						goto ED_LOOP;
					case WordType.vOperator:
						if(scanner.CurrentWord==")")
							goto ED_LOOP;
						else if(scanner.CurrentWord=="|"){
							this.scanner.ReadNext();
							goto default;
						}else{
							goto default;
						}
					default:
						INode node=this.ReadSequence();
						if(node==null){
							scanner.LetterReader.SetError("���K�\���v�f���������܂���B",0,null);
							break;
						}

						nodes.Add(node);
						break;
				}
			ED_LOOP:
				if(nodes.Count==0)return null;
				if(nodes.Count==1)return nodes[0];
				return new OrNode(nodes.ToArray());
			}
			/// <summary>
			/// �v�f�A������ ('|' '(' ')' �Ȃǂŋ�؂�ꂽ����) ��ǂݎ��܂��B
			/// </summary>
			/// <returns>�ǂݎ���ďo�����m�[�h��Ԃ��܂��B�L���Ȑ��K�\���w�肪������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			private INode ReadSequence(){
				Gen::List<INode> nodes=new Gen::List<INode>();
				while(true)switch(this.scanner.CurrentType.value){
					case WordType.vInvalid:
						goto ED_LOOP;
					case WordType.vComment: // (?#)
						this.scanner.ReadNext();
						continue;
					case RegexScannerA.WT_CHARCLASS: // [�����N���X]
						nodes.Add(this.ProcessClass(this.scanner.CurrentWord));
						this.scanner.ReadNext();
						break;
					case RegexScannerA.WT_COMMAND: // �R�}���h
						parseCommand(ref nodes);
						break;
					case RegexScannerA.WT_COMMAND_C: // : �R�}���h
						nodes.Add(this.ProcessCommandC(scanner.CurrentWord,scanner.Value));
						this.scanner.ReadNext();
						break;
					case WordType.vSuffix: // ? + *
						parseSuffix(ref nodes);
						break;
					case WordType.vOperator: // ( | )
						if(scanner.CurrentWord==")"||scanner.CurrentWord=="|")
							goto ED_LOOP;
						if(scanner.CurrentWord=="(?flags)"){
							// ���ʂ̎n�܂�O�� flags ���o���Ă����d�g��
							__debug__.RegexParserToDo("������");
						}
						INode node=parseOperator();
						if(node!=null)nodes.Add(node);
						break;
					case WordType.vText: // �ʏ�̕���
						switch(scanner.CurrentWord[0]){
							case '.':nodes.Add(AnyElemNode.Instance);break;
							case '^':nodes.Add(StartOfStreamNode.instance);break;
							case '$':nodes.Add(EndOfStreamNode.instance);break;
							default:
								nodes.Add(this.ProcessLetter(scanner.CurrentWord[0]));
								break;
						}
						this.scanner.ReadNext();
						break;
				}
			ED_LOOP:
				if(nodes.Count==0)return null;
				if(nodes.Count==1)return nodes[0];
				return new SequenceNode(nodes.ToArray());
			}
			//========================================================
			private void parseCommand(ref Gen::List<INode> list){
				try{
					list.Add(this.ProcessCommand(scanner.CurrentWord,scanner.Value));
				}catch(RegexParseException e){
					this.scanner.LetterReader.SetError(e.Message,0,null);
				}
				this.scanner.ReadNext();
			}
			/// <summary>
			/// ���ʂ̎n�܂�A���̑��̉��Z�q��ǂݎ��܂��B
			/// </summary>
			/// <returns></returns>
			private INode parseOperator(){
				string word=this.scanner.CurrentWord;
				string val=this.scanner.Value;
				this.scanner.ReadNext();

				INode content=null;
				if(word[0]=='(')
					content=this.ReadUntilCloseParen();
				if(content==null)return null;

				switch(word){
					case "(":
						return new CaptureNode(content);
					case "(?:":
						return content;
					case "(?<>":
						return new CaptureNode(content,val);
					case "(!":return new AheadAssertNode(content,false,parent);
					case "(=":return new AheadAssertNode(content,true,parent);
					case "(>":return new NonBacktrackNode(content,parent);
					case "(?<=":
					case "(?<!":
					case "(?flags:":
					default:
						__debug__.RegexParserToDo("������");
						throw new System.NotImplementedException();
					case "(?flags)":
					case ")":
					case "|":
						__debug__.RegexParserAssert(false,"�Ăяo�����ŏ�������̂ŁA�����ɂ͓��B���Ȃ���");
						return null;
				}
			}
			/// <summary>
			/// �ʎw�莌��ǂݎ��܂��B
			/// </summary>
			/// <param name="list">�����ɓǂݎ���� INode ����w�肵�܂��B�ʎw�莌�͍Ō�� INode �ɓK�p����܂��B</param>
			private void parseSuffix(ref Gen::List<INode> list){
				if(list.Count==0){
					this.scanner.LetterReader.SetError("Suffix "+scanner.CurrentWord+" �ɐ�s����w�莌�����݂��܂���B",0,null);
					this.scanner.ReadNext();
					return;
				}

				INode node=list[list.Count-1];
				switch(scanner.CurrentWord){
					case "+":
						node=new RepeatNode(node,1,-1);
						break;
					case "*":
						node=new RepeatNode(node,0,-1);
						break;
					case "?":
						node=new RepeatNode(node,0,1);
						break;
					case "q":{ // {m,n}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new RepeatNode(node,a,b);
						break;
					}
					case "q=":{ // {m}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);
						
						node=new RepeatNode(node,a,a);
						break;
					}
					case "q>":{ // {m,}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new RepeatNode(node,a,-1);
						break;
					}
					//================================================
					//		Lazy-Repeat
					//================================================
					case "+?":
						node=new LazyRepeatNode(node,1,-1);
						break;
					case "*?":
						node=new LazyRepeatNode(node,0,-1);
						break;
					case "??":
						node=new LazyRepeatNode(node,0,1);
						break;
					case "q?":{ // {m,n?}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,b);
						break;
					}
					case "q=?":{ // {m?} �� �P�Ȃ� {m} �Ɠ���
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,a);
						break;
					}
					case "q>?":{ // {m,?}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new LazyRepeatNode(node,a,-1);
						break;
					}
					//================================================
					//		Sticky-Repeat
					//================================================
					case "++":
						node=new StickyRepeatNode(node,1,-1);
						break;
					case "*+":
						node=new StickyRepeatNode(node,0,-1);
						break;
					case "?+":
						node=new StickyRepeatNode(node,0,1);
						break;
					case "q+":{ // {m,n+}
						__debug__.RegexParserAssert(scanner.Value!=null);
						string[] nums=scanner.Value.Split(',');
						__debug__.RegexParserAssert(nums.Length==2);

						int a,b;
						bool debug=int.TryParse(nums[0],out a)&int.TryParse(nums[1],out b);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,b);
						break;
					}
					case "q=+":{ // {m+} �� �P�Ȃ� {m} �Ɠ���
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,a);
						break;
					}
					case "q>+":{ // {m,+}
						int a;
						bool debug=int.TryParse(scanner.Value,out a);
						__debug__.RegexParserAssert(debug);

						node=new StickyRepeatNode(node,a,-1);
						break;
					}
					default:
						__debug__.RegexParserAssert(false);
						break;
				}
				list[list.Count-1]=node;

				this.scanner.ReadNext();
			}
		}
	}
}