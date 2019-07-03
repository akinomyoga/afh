using Gen=System.Collections.Generic;
using BaseRegexFactory=afh.RegularExpressions.RegexFactory3<char>;

namespace afh.RegularExpressions{
	using INode=afh.RegularExpressions.RegexFactory3<char>.INode;
	using ITester=afh.RegularExpressions.RegexFactory3<char>.ITester;
	using ElemNodeBase=afh.RegularExpressions.RegexFactory3<char>.ElemNodeBase;

	/// <summary>
	/// �ʏ�̕�����ɑ΂��鐳�K�\����񋟂��܂��B
	/// </summary>
	internal static partial class CharClasses{
		internal partial class CharClassNodeGenerator{
			//#define EOF		(i>=expression.Length)
			//#define NOT(c)	(expression[i]!=(c))
			//#define NEXT		(++i<expression.Length)
			//#define IS(c)		(expression[i]==(c))
			private IClassHandler ReadClass(){
				bool mode=NOT('^'); // [...] or [^...]
				if(!mode)i++;
				ClassHandlerGenerator gen=new ClassHandlerGenerator(mode);

				while(i<expression.Length){
					char c1,c2;
					if(ReadChar(out c1)){
						c2='\0';
					}else{
						c2=expression[i];
					}

					switch(c2){
						case '\0':
							// �ʏ�̕���
							if(EOF||NOT('-')){
								gen.Add(new AtomHandler(c1),false);
								break;
							}

							// �͈͎w�� ?
							if(NEXT){
								if(ReadChar(out c2)){
									gen.Add(new RangeHandler(c1,c2),false);
									break;
								}
								if(NOT('['))
									this.ReportError("�����͈͎w�� �ua-b�v �̏I�[�ɓ����镶��������܂���B������ - �ɃG�X�P�[�v���K�v�ł��B");
							}
							i--; // �͈͎w��łȂ������̂Ŗ����������ɂ���

							// ��͂�ʏ�̕���
							gen.Add(new AtomHandler(c1),false);
							break;
						case '\\':{
							if(!NEXT){
								this.ReportError(@"\ �ɑ����������K�v�ł��B");
								goto ED;
							}

							//-- ���O�t�������N���X
							if(c1=='p'||c1=='P'){
								gen.Add(this.CreateNamedClass(),false);
							}else{
								this.ReportError("�F���o���Ȃ��R�}���h�ł��B");
								i++;
							}
							break;
						}
						case '-':
							if(NEXT&&IS('[')){
								i++;
								gen.Add(ReadClass(),true);
								break;
							}
							gen.Add(new AtomHandler(c1),false);
							break;
						case ']':
							i++;
							goto ED;
						default:
							__debug__.RegexParserAssert(false);
							break;
					}
				}
			ED:
				return gen.Create();
			}
			/// <summary>
			/// �ʏ�̕�����ǂݎ���ꍇ�ɂ́A���̕�����ǂݎ���� true ��Ԃ��܂��B
			/// �ʏ�̕����łȂ��ꍇ�ɂ́A�ǂݎ�炸�� false ��Ԃ��܂��B
			/// </summary>
			/// <param name="c">�ǂݎ����������Ԃ��܂��B</param>
			/// <returns>�ʏ�̕�����ǂݎ�ꂽ�ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			/// <remarks>CharClassNodeGenerator.i �́A������ǂݎ�����ꍇ�ɂ́A���̎��̕����̈ʒu���w������ԂŕԂ��܂��B</remarks>
			private bool ReadChar(out char c){
				c=expression[i];

				switch(c){
					case '\\':{
						if(!NEXT){
							this.ReportError(@"\ �ɑ����������K�v�ł��B");
							i--;
							return false;
						}

						//-- ���ʂ̕���
						int index=COMMAND_LETTERS_SPACE.IndexOf(expression[i]);
						if(index>=0){
							c=SPACE_LETTERS[index];
							return true;
						}

						//-- �����G�X�P�[�v
						index=CONTROL_LETTERS.IndexOf(expression[i]);
						if(index>=0){
							c=expression[i];
							return true;
						}

						i--;
						return false;
					}
					case '-':
					case ']':
						return false;
					default:
						i++;
						return true;
				}
			}
			/// <summary>
			/// ���O�t�������N���X�w���ǂݎ��܂��B
			/// </summary>
			/// <returns>�ǂݎ�肪���������ꍇ�ɔ���֐���Ԃ��܂��B����ȊO�̏ꍇ�� null ��Ԃ��܂��B</returns>
			private IClassHandler CreateNamedClass(){
				char c=expression[i];

				//-- �ǂݎ��
				if(!NEXT||NOT('{')){
					this.ReportError("���O�t���N���X�Ɉ��������蓖�Ă��Ă��܂���B");
					return null;
				}

				string arg="";
				while(NEXT&&NOT('}'))
					arg+=expression[i];

				if(EOF){
					this.ReportError("���O�t���N���X�̈����ɏI�[ '}' ������܂���B");
					return null;
				}else i++;

				//-- �C���X�^���X�쐬
				return CreateNamedClassHandler(c=='p',arg);
			}
		}
	}
}