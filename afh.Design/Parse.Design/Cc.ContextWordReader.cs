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

			// コメントの始まりの場合
			if(this.IsComment()){
				if("is:/"){
					add;if(!next)return true;

					this.wtype=WordType.Comment;
#if !REMOVE_COMMENT
					if("is:*"){
						add;if(!next){
							error("コメントに終端 */ がありません。");
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
							error("コメントに終端 */ がありません。");
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
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);
		}
		private void ReadCommandDeclaration(){
			// 通常時
			if("not:{")do{
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);

			// '{' の時: 内容の読み取り
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
			error("'{' に対応する '}' がありません");
		}
		//=================================================
		//		read context
		//=================================================
		/// <summary>
		/// Context の宣言部を読み取ります。
		/// </summary>
		private void ReadContextDeclaration(){
			// context の内容の始まり
			if("is:{"){
				this.wtype=WordType.Operator;
				this.mode=WordReaderMode.ContextConditions;
				add;nexit;
			}
			// その他の場合
			do{
				add;next;
				if("is:{"||type.IsInvalid||type.IsSpace||this.IsComment())return;
			}while(true);
		}
		/// <summary>
		/// context の分岐標を読み取ります。
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
		/// ( で始まる場合は引数の読み取りを行います。( ではじまり ) で終わる迄を読み取ります。括弧文字自体は含めません。
		/// </para>
		/// <para>; の場合には命令の読み取りモードを抜けて、命令分岐コード取得に移行します。</para>
		/// <para>} の場合には現在のコンテクスト定義を抜けます。</para>
		/// </summary>
		private void ReadContextCommands(){
			const string MISS_ENDQUOTE="引用符 '\"' に終端 '\"' がありません。";
			const string MISS_ENDPAREN="括弧 '(' に終端 ')' がありません。";
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
		/// 複数行コメントを読み取ります。
		/// /* の次の位置で呼び出して下さい。
		/// </summary>
		private void ReadMultiComment(){
			this.wtype=WordType.Comment;
			bool reach;
			do{
				reach="is:*";
				add;if(!next)errorexit("コメントに終端 */ がありません");
			}while(!reach||"not:/");
			add;nexit;
		}
		/// <summary>
		/// 単一行コメントを読み取ります。
		/// // の次の位置で呼び出して下さい。
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