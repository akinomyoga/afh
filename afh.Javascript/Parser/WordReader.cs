using afh.Parse;

namespace afh.JavaScript.Parse{
	/// <summary>
	/// Javascript �̒P���ǂݎ��ׂ̃N���X�ł��B
	/// </summary>
	public sealed partial class WordReader:AbstractWordReader{
		public WordReader(string text):base(text){}
		private string oword="";
		private WordType otype=WordType.Invalid;

		public override bool ReadNext(){
			this.oword=this.cword;
			this.otype=this.wtype;
			this.cword="";
			this.wtype=WordType.Invalid;
			while(lreader.CurrentType.IsInvalid||lreader.CurrentType.IsSpace){
				if(!lreader.MoveNext())return false;
			}
			this.lreader.StoreCurrentPos(0);
			switch(lreader.CurrentType.purpose){
				case LetterType.P_Number:
					ReadNumber();
					break;
				case LetterType.P_Token:
					ReadIdentifier();
					break;
				case LetterType.P_Operator:
					ReadOperator();
					break;
			}
			return true;
		}
		/// <summary>
		/// �L���Ŏn�܂�P���ǂݎ��܂��B
		/// </summary>
		private void ReadOperator(){
			this.wtype=WordType.Operator;
			char c;
			switch(letter){
				case '=':case '!':
					// o o= o==
					add;next;
					if("not:=")return;
					add;next;
					goto aft_equal;
				case '+':case '-':case ':':
					// o oo
					c=letter;
					add;next;
					if(letter!=c)goto aft_equal;
					add;nexit;
				case '&':case '|':case '<':
					// o oo o= oo=
					c=letter;
					add;next;
					if(letter!=c)goto aft_equal;
					add;next;
					goto aft_equal;
				case '>':
					// o oo ooo o= oo= ooo=
					add;next;
					if("not:>")goto aft_equal;
					add;next;
					if("not:>")goto aft_equal;
					add;next;
					goto aft_equal;
				case '*':case '%':case '^':
					// o o=
					add;next;
					goto aft_equal;
				case '/':
					add;next;
					if("is:*"){
						add;if(!next)errorexit("�R�����g�ɏI�[ */ ������܂���B");
						ReadMultiComment();return;
					}else if("is:/"){
						add;next;
						ReadLineComment();return;
					}else if(this.otype==WordType.Invalid||this.otype==WordType.Operator&&this.oword!="}"&&this.oword!=")"&&this.oword!="]"){
						lreader.MoveToPos(0);
						this.cword="";
						ReadRegExp();return;
					}else goto aft_equal;
				case '"':ReadStringDQ();return;
				case '\'':ReadStringSQ();return;
				case '.':
					add;next;
					if("is:0-9"){
						lreader.MoveToPos(0);
						this.cword="";
						ReadNumber();
						return;
					}
					if("not:.")return;
					add;next;
					if("not:.")return;
					add;nexit;		// ... ���Z�q
				default:
					add;nexit;
				aft_equal:
					if("not:=")return;
					add;nexit;
			}
		}
		/// <summary>
		/// �����s�R�����g��ǂݎ��܂��B
		/// /* �̎��̈ʒu�ŌĂяo���ĉ������B
		/// </summary>
		private void ReadMultiComment(){
			this.wtype=WordType.Comment;
			bool reach;
			do{
				reach="is:*";
				add;if(!next)errorexit("�R�����g�ɏI�[ */ ������܂���");
			}while(!reach||"not:/");
			add;nexit;
		}
		/// <summary>
		/// �P��s�R�����g��ǂݎ��܂��B
		/// // �̎��̈ʒu�ŌĂяo���ĉ������B
		/// </summary>
		private void ReadLineComment(){
			this.wtype=WordType.Comment;
			while("not:\r"&&"not:\n"){
				add;next;
			}
			nexit;
		}
	}
}