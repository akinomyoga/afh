/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
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
			while(this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace){
				if(!lreader.MoveNext()){
					this.wtype=WordType.Invalid;
					return false;
				}
			}
			this.lreader.StoreCurrentPos(0);

			// コメントの始まりの場合
			if(this.IsComment()){
				if((this.lreader.CurrentLetter=='/')){
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return true;

					this.wtype=WordType.Comment;
#if !REMOVE_COMMENT
					if((this.lreader.CurrentLetter=='*')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
							this.lreader.SetError(("コメントに終端 */ がありません。"),0,null);
							return true;
						}
						ReadMultiComment();
						return true;
					}else if((this.lreader.CurrentLetter=='/')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return true;
						ReadLineComment();
						return true;
					}
#else
					if((this.lreader.CurrentLetter=='*')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
							this.lreader.SetError(("コメントに終端 */ がありません。"),0,null);
							return false;
						}
						ReadMultiComment();
						goto start;
					}else if((this.lreader.CurrentLetter=='/')){
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return false;
						ReadLineComment();
						goto start;
					}
#endif
				}
				throw new System.ApplicationException("Fatal: ここに制御は来ない筈…IsComment 関数の内容に誤りがあるのかも");
			}

			// 通常の読み取り
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
		/// 地の文にいる時に読み取る操作です。
		/// </summary>
		private void ReadPlain(){
			// 兎に角単語の読み取り
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='{')||this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace||this.IsComment())return;
			}while(true);
		}
		private void ReadCommandDeclaration(){
			// 通常時
			if(!(this.lreader.CurrentLetter=='{'))do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='{')||this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace||this.IsComment())return;
			}while(true);

			// '{' の時: 内容の読み取り
			if(!this.lreader.MoveNext())goto err;
			int nestlevel=0;
			while(true)switch(this.lreader.CurrentLetter){
				case '"':
					this.ReadStringDQ();
					if(this.lreader.CurrentType.IsInvalid)goto err;
					break;
				case '\\':
					if(!this.lreader.MoveNext())goto err;
					goto default;
				case '{':
					nestlevel++;
					goto default;
				case '}':
					if(0==nestlevel--){
						if(!this.lreader.MoveNext())return;goto exit;
					}
					goto default;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		exit:
			this.wtype=WordType.Literal;
			this.mode=WordReaderMode.Plain;
			return;
		err:
			this.lreader.SetError(("'{' に対応する '}' がありません"),0,null);
		}
		//=================================================
		//		read context
		//=================================================
		/// <summary>
		/// Context の宣言部を読み取ります。
		/// </summary>
		private void ReadContextDeclaration(){
			// context の内容の始まり
			if((this.lreader.CurrentLetter=='{')){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.ContextConditions;
				this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
			}
			// その他の場合
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='{')||this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace||this.IsComment())return;
			}while(true);
		}
		/// <summary>
		/// context の分岐標を読み取ります。
		/// </summary>
		private void ReadContextConditions(){
			if((this.lreader.CurrentLetter=='}')){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.Plain;
				this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
			}else if((this.lreader.CurrentLetter==':')){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.ContextCommands;
				this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
			}else do{
				if((this.lreader.CurrentLetter=='\\'))if(!this.lreader.MoveNext())return;
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter==':')||(this.lreader.CurrentLetter=='}')||this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace||this.IsComment())return;
			}while(true);
		}
		/// <summary>
		/// <para>
		/// ( で始まる場合は引数の読み取りを行います。( ではじまり ) で終わる迄を読み取ります。括弧文字自体は含めません。
		/// </para>
		/// <para>; の場合には命令の読み取りモードを抜けて、命令分岐コード取得に移行します。</para>
		/// <para>} の場合には現在のコンテクスト定義を抜けます。</para>
		/// </summary>
		private void ReadContextCommands(){
			const string MISS_ENDQUOTE="引用符 '\"' に終端 '\"' がありません。";
			const string MISS_ENDPAREN="括弧 '(' に終端 ')' がありません。";
			switch(this.lreader.CurrentLetter){
				case '(':
					this.wtype=WordType.Suffix;
					if(!this.lreader.MoveNext()){this.lreader.SetError((MISS_ENDPAREN),0,null);return;}
					while(true){
						if((this.lreader.CurrentLetter=='\"')){
							while(true){
								this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){this.lreader.SetError((MISS_ENDQUOTE),0,null);return;}
								if((this.lreader.CurrentLetter=='\\')){
									if(!this.lreader.MoveNext()){this.lreader.SetError((MISS_ENDQUOTE),0,null);return;}
								}else if((this.lreader.CurrentLetter=='\"')){
									break;
								}else if((this.lreader.CurrentLetter=='\r')||(this.lreader.CurrentLetter=='\n')){
									{this.lreader.SetError((MISS_ENDQUOTE),0,null);return;}
								}
							}
						}else if((this.lreader.CurrentLetter==')')){
							if(!this.lreader.MoveNext()){this.lreader.SetError((MISS_ENDPAREN),0,null);return;}
							return;
						}
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){this.lreader.SetError((MISS_ENDPAREN),0,null);return;}
					}
				case ';':
					this.wtype=WordType.Operator;
					this.mode=WordReaderMode.ContextConditions;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				case '}':
					this.wtype=WordType.Operator;
					this.mode=WordReaderMode.Plain;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				default:
					while(true){
						if((this.lreader.CurrentLetter=='\\'))if(!this.lreader.MoveNext())return;
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						if((this.lreader.CurrentLetter=='(')||(this.lreader.CurrentLetter==';')||(this.lreader.CurrentLetter=='}')||this.lreader.CurrentType.IsInvalid||this.lreader.CurrentType.IsSpace||this.IsComment())return;
					}
			}
		}
		//=================================================
		//		read comment
		//=================================================
		private bool IsComment(){
			char ch;
			return (this.lreader.CurrentLetter=='/')&&this.lreader.PeekChar(out ch)&&(ch=='/'||ch=='*');
		}
		/// <summary>
		/// 複数行コメントを読み取ります。
		/// /* の次の位置で呼び出して下さい。
		/// </summary>
		private void ReadMultiComment(){
			this.wtype=WordType.Comment;
			bool reach;
			do{
				reach=(this.lreader.CurrentLetter=='*');
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){this.lreader.SetError(("コメントに終端 */ がありません"),0,null);return;}
			}while(!reach||!(this.lreader.CurrentLetter=='/'));
			this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
		}
		/// <summary>
		/// 単一行コメントを読み取ります。
		/// // の次の位置で呼び出して下さい。
		/// </summary>
		private void ReadLineComment(){
			this.wtype=WordType.Comment;
			while(!(this.lreader.CurrentLetter=='\r')&&!(this.lreader.CurrentLetter=='\n')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}
			{this.lreader.MoveNext();return;}
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