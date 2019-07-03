#define REMOVE_COMMENT
using System;
using Gen=System.Collections.Generic;
using System.Text;

namespace afh.Parse.Cc {
	internal class ContextWordReader:afh.Parse.AbstractWordReader{
		public ContextWordReader(string text):base(text){}

		private WordReaderMode mode=WordReaderMode.Plain;
		public override bool ReadNext(){
		start:
			this.cword="";
			while(type.IsInvalid||type.IsSpace){
				if(!lreader.MoveNext()){
					this.wtype=WordType.Invalid;
					return false;
				}
			}
			this.lreader.StoreCurrentPos(0);

			// �R�����g�̎n�܂�̏ꍇ
			if(this.IsComment()){
				if("is:/"){
					add;if(!next)return true;

					this.wtype=WordType.Comment;
#if !REMOVE_COMMENT
					if("is:*"){
						add;if(!next){
							error("�R�����g�ɏI�[ */ ������܂���B");
							return true;
						}
						ReadMultiComment();
						return true;
					}else if("is:/"){
						add;if(!next)return true;
						ReadLineComment();
						return true;
					}
#else
					if("is:*"){
						add;if(!next){
							error("�R�����g�ɏI�[ */ ������܂���B");
							return false;
						}
						ReadMultiComment();
						goto start;
					}else if("is:/"){
						add;if(!next)return false;
						ReadLineComment();
						goto start;
					}
#endif
				}
				throw new System.ApplicationException("Fatal: �����ɐ���͗��Ȃ����cIsComment �֐��̓��e�Ɍ�肪����̂���");
			}

			// �ʏ�̓ǂݎ��
			this.wtype=WordType.Identifier;
			switch(mode){
				case WordReaderMode.Plain:
					this.ReadPlain();
					if(this.wtype==WordType.Identifier){
						if(this.cword=="context")
							this.mode=WordReaderMode.ContextDeclaration;
						else if(this.cword=="command")
							this.mode=WordReaderMode.CommandDeclaration;
						else if(this.cword=="condition")
							this.mode=WordReaderMode.ConditionDeclaration;
					}
					break;
				case WordReaderMode.ContextDeclaration:
					this.ReadContextDeclaration();
					break;
				case WordReaderMode.ContextConditions:
					this.ReadContextConditions();
					break;
				case WordReaderMode.ContextCommands:
					this.ReadContextCommands();
					break;
				case WordReaderMode.CommandDeclaration:
				case WordReaderMode.ConditionDeclaration:
					this.ReadCommandDeclaration();
					break;
			}
			return true;
		}
		/// <summary>
		/// �n�̕��ɂ��鎞�ɓǂݎ�鑀��ł��B
		/// </summary>
		private void ReadPlain(){
			// �e�Ɋp�P��̓ǂݎ��
			do{
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);
		}
		private void ReadCommandDeclaration(){
			// �ʏ펞
			if("not:{")do{
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);

			// '{' �̎�: ���e�̓ǂݎ��
			if(!next)goto err;
			int nestlevel=0;
			while(true)switch(letter){
				case '"':
					this.ReadStringDQ();
					if(type.IsInvalid)goto err;
					break;
				case '\\':
					if(!next)goto err;
					goto default;
				case '{':
					nestlevel++;
					goto default;
				case '}':
					if(0==nestlevel--){
						next;goto exit;
					}
					goto default;
				default:
					add;if(!next)goto err;
					break;
			}
		exit:
			this.wtype=WordType.Literal;
			this.mode=WordReaderMode.Plain;
			return;
		err:
			error("'{' �ɑΉ����� '}' ������܂���");
		}
		//=================================================
		//		read context
		//=================================================
		/// <summary>
		/// Context �̐錾����ǂݎ��܂��B
		/// </summary>
		private void ReadContextDeclaration(){
			// context �̓��e�̎n�܂�
			if("is:{"){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.ContextConditions;
				add;nexit;
			}
			// ���̑��̏ꍇ
			do{
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);
		}
		/// <summary>
		/// context �̕���W��ǂݎ��܂��B
		/// </summary>
		private void ReadContextConditions(){
			if("is:}"){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.Plain;
				add;nexit;
			}else if("is::"){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.ContextCommands;
				add;nexit;
			}else do{
				if("is:\\")next;
				add;next;
				if("is::"||"is:}"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);
		}
		/// <summary>
		/// <para>
		/// ( �Ŏn�܂�ꍇ�͈����̓ǂݎ����s���܂��B( �ł͂��܂� ) �ŏI��閘��ǂݎ��܂��B���ʕ������̂͊܂߂܂���B
		/// </para>
		/// <para>; �̏ꍇ�ɂ͖��߂̓ǂݎ�胂�[�h�𔲂��āA���ߕ���R�[�h�擾�Ɉڍs���܂��B</para>
		/// <para>} �̏ꍇ�ɂ͌��݂̃R���e�N�X�g��`�𔲂��܂��B</para>
		/// </summary>
		private void ReadContextCommands(){
			const string MISS_ENDQUOTE="���p�� '\"' �ɏI�[ '\"' ������܂���B";
			const string MISS_ENDPAREN="���� '(' �ɏI�[ ')' ������܂���B";
			switch(letter){
				case '(':
					this.wtype=WordType.Suffix;
					if(!next)errorexit(MISS_ENDPAREN);
					while(true){
						if("is:\""){
							while(true){
								add;if(!next)errorexit(MISS_ENDQUOTE);
								if("is:\\"){
									if(!next)errorexit(MISS_ENDQUOTE);
								}else if("is:\""){
									break;
								}else if("is:\r"||"is:\n"){
									errorexit(MISS_ENDQUOTE);
								}
							}
						}else if("is:)"){
							if(!next)errorexit(MISS_ENDPAREN);
							return;
						}
						add;if(!next)errorexit(MISS_ENDPAREN);
					}
				case ';':
					this.wtype=WordType.Operator;
					this.mode=WordReaderMode.ContextConditions;
					add;nexit;
				case '}':
					this.wtype=WordType.Operator;
					this.mode=WordReaderMode.Plain;
					add;nexit;
				default:
					while(true){
						if("is:\\")next;
						add;next;
						if("is:("||"is:;"||"is:}"||type.IsInvalid||type.IsSpace||this.IsComment())return;
					}
			}
		}
		//=================================================
		//		read comment
		//=================================================
		private bool IsComment(){
			char ch;
			return "is:/"&&this.lreader.PeekChar(out ch)&&(ch=='/'||ch=='*');
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
	internal enum WordReaderMode{
		Plain,
		ContextDeclaration,
		ContextConditions,
		ContextCommands,
		CommandDeclaration,
		ConditionDeclaration
	}
}