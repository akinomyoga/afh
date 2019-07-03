using afh.Parse;
using Gen=System.Collections.Generic;

namespace afh.RegularExpressions{
	internal partial class RegexScannerA:AbstractWordReader{
		public override bool ReadNext(){
			this.cword="";
			this.wtype=WordType.Invalid;
			if(lreader.IsEndOfText)return false;

			this.lreader.StoreCurrentPos(0);
			switch(letter){
				case '\\':
					wtype=WtCommand;
					readCommand(grammar.CommandSet);
					break;
				case ':':
					if(!grammar.EnablesColonCommand){
						error("�R���� : �̕����̑O�ɂ� \\ ��u���l�ɂ��ĉ������B");
						goto default;
					}
					wtype=WtCommandC;
					readCommand(grammar.ColonCommandSet);
					break;
				// + * ? +? *? ??
				case '+':case '*':case '?':
					readSuffix();
					break;
				case '(':
					readOpenParen();
					break;
				case ')':case '|':
					wtype=WordType.Operator;
					add;
					lreader.MoveNext();
					break;
				case '{':
					readQuantifier();
					break;
				case '[':
					readCharClass();
					break;
				default:
					wtype=WordType.Text;
					add;
					lreader.MoveNext();
					break;
			}
			return true;
		}
		// + * ? +? *? ??
		private void readSuffix(){
			wtype=WordType.Suffix;
			add;next;
			if("is:?"){
				add;next;
			}
		}
		//============================================================
		//		�����N���X [...] �̓ǂݎ��
		//============================================================
		private void readCharClass(){
			wtype=WtCharClass;
			if(!next)goto MISSING_CLOSE;

			int level=0;
			while(true)switch(letter){
				case '[':
					level++;
					goto default;
				case ']':
					if(level--==0)nexit;
					goto default;
				case '\\':
					add;if(!next){
						error("�G�X�P�[�v�V�[�P���X/�R�}���h���������Ă��܂���B");
						goto MISSING_CLOSE;
					}
					goto default;
				default:
					add;if(!next)goto MISSING_CLOSE;
					break;
			}
		MISSING_CLOSE:
			error("�N���X��`�ɖ��[�� ] �����݂��܂���B");
			return;
		}
		//============================================================
		//		�ʎw�莌 {...} �̓ǂݎ��
		//============================================================
		private void readQuantifier() {
			wtype=WordType.Suffix;
			if(!next)goto ERR_Q;

			//-- �������l�̓ǂݎ��
			string arg="";
			string arg1=null;
			bool comma=false;
			while("not:}"&&"not:+"&&"not:?"){
				if("is:0-9"){
					arg+=letter;
				}else if("is:,"){
					if(comma)
						error("�ʎw�莌�̒��� , �͍ő��܂łł��B");
					else{
						comma=true;
						arg1=arg;
						arg="";
					}
				}else{
					error("�ʎw�莌���̕����Ƃ��Ė����ł��B������ , ���w�肵�ĉ������B");
				}
				if(!next)goto ERR_Q;
			}

			//-- �×~�� {m,n} {m,n?} {m,n+} ����
			char chGreedy=letter;
			if("not:}"){
				if(!next)
					error("�ʎw�莌���I�����Ă��Ȃ����ɕ\���̖��[�ɒB���܂����B");
				else if("not:}")
					error("�ʎw�莌���ł́A+/? �̒���� } �����Ȃ���Έׂ�܂���B'}' ���݂������ƌ��􂵂܂��B");
			}
			lreader.MoveNext(); // } �ǂݔ�΂�

			//-- ��ށE�����̌���
			if(comma){
				if(arg==""){
					cword="q>"; // {m,} �`��
					value=arg1;
				}else{
					cword="q"; // {m,n} �`��
					value=arg1+","+arg;
				}
			}else{
				if(arg==""){
					error("�ʎw�莌�̒��Ɏw�肪���݂��Ă��܂���B");
					return;
				}else{
					cword="q="; // {m,n} �`��
					value=arg;
				}
			}

			// if("is:?"){add;next;} // �p�~
			if(chGreedy!='}')cword+=chGreedy;
			return;
		ERR_Q:
			error("�ʎw�莌�ɏI�[�� } ���݂�܂���B");
		}
		//============================================================
		//		�R�}���h \* �̓ǂݎ��
		//============================================================
		private void readCommand(Gen::Dictionary<string,CommandData> commands){
			__debug__.RegexParserToDo("�����������̃R�}���h");

			if(!next) {
				error(@"\ �̌�ɃR�}���h���������Ă��܂���B");
				wtype=WordType.Text;
				return;
			}
			add;next;
			
			CommandData dat;
			if(!commands.TryGetValue(cword,out dat)){
				error(@"�w�肵�����O�̃R�}���h�͓o�^����Ă��܂���B");
				wtype=WordType.Text;
				return;
			}

			if(dat.ArgumentType==CommandArgumentType.Brace){
				if("not:{"){
					value=null;
				}else{
					string cmd=cword;
					cword="";
					if(!next)goto ERR_ARG;

					int iP=1;
					bool escape=false;
					while(true){
						if(escape){
							escape=false;
						}else{
							if("is:{"){
								iP++;
							}else if("is:}"){
								if(--iP==0){lreader.MoveNext();break;}
							}else if("is:\\"){
								escape=true;
							}
						}
						add;if(!next)goto ERR_ARG;
					}
					value=cword;
					cword=cmd;
					return;
				ERR_ARG:
					value=cword;
					cword=cmd;
					error("�����̏I�[�����Ȃ����ɁA���K�\���̖��[�ɒB���܂����B");
				}
			}
		}
		//============================================================
		//		�O���[�v (...) �̓ǂݎ��
		//============================================================
		private void readOpenParen() {
			// ( (?: (?<= (?<! (?> (?! (?=
			// (?imxn-imxn: (?<...>
			wtype=WordType.Operator;
			add;next;
			if("not:?")return; // "(": Capture
			add;next;
			switch(letter){
				case ':':
					cword="(?:";
					nexit;
				case '\'':
					next;
					cword="";
					while("not:\'"){
						add;if(!next){
							error("�L���v�`�����ɏI�[������܂���B�L���v�`������ '\\'' �ŏI���K�v������܂��B");
							break;
						}
					}
					value=cword;
					cword="(?<>";
					nexit; // ' ��ǂݔ�΂�
				case '<':
					// "(?<=" : ��ǂ݈�v
					// "(?<!" : ��ǂݕs��v
					// "(?<>" : ���O�t���ߊl 
					add;next;
					if("is:!"||"is:="){
						add;nexit;
					}else{
						cword="";
						while("not:>"){
							add;if(!next){
								error("�L���v�`�����ɏI�[������܂���B�L���v�`������ '>' �ŏI���K�v������܂��B");
								break;
							}
						}
						value=cword;
						cword="(?<>";
						nexit; // > ��ǂݔ�΂�
					}
				case '!': // ��ǂݕs��v
				case '=': // ��ǂ݈�v
				case '>': // ��o�b�N�g���b�N
					add;nexit;
				case '#':
					wtype=WordType.Comment;
					while("not:)"){
						add;if(!next){
							error("�R�����g�̏I��� ')' ���݂�܂���B");
							return;
						}
					}
					nexit; // skip ')'
				default:
					// "(?flags:": (?imnx-imnx:
					// "(?flags)": (?imnx-imnx)
					if("is:-"||flags.IndexOf(letter)>=0){
						cword="";
						bool minused="is:-";
						do{
							add;if(!next){
								value=cword;
								cword="(?flags:";
							}
							if("is:-"){
								if(minused)break;
								minused=true;
							}
						}while("is:-"||flags.IndexOf(letter)>=0);

						value=cword;
						cword="(?flags";
						if("is::"||"is:)"){
							add;nexit;
						}else{
							string errmsg="'"+letter+"' �� '(?flags' �̌�Ƃ��ė\�����Ȃ������ł��B':' ���Ԃɂ���Ɖ��߂��܂��B";
							error(errmsg);
							cword="(?flags:";
							return;
						}
					}else{
						string errmsg="'"+letter+"' �� '(?' �̌�Ƃ��ė\�����Ȃ������ł��B'(?:' �Ɖ��߂��܂��B";
						error(errmsg);
						cword="(?:";
						return;
					}
			}
		}

	}
}