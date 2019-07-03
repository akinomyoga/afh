﻿/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
namespace afh.Parse{
	//-------------------------------------------------
	//		数値リテラル
	//-------------------------------------------------
	//		0[xX][0-9a-fA-F]+
	//		(\d*\.)?\d+([eE][+-]?\d+)?[fF]?
	//		\d+[uU]?[lL]?
	//	原則:
	//		文字列:***;位置:***:
	//		で指定される条件を満たす状態で呼び出す事
	//-------------------------------------------------
	public abstract partial class AbstractWordReader{
		/// <summary>
		/// 数値リテラルを読み取ります。
		/// 文字列: "";
		/// 位置: \d;
		/// ※ '.' の次に数字が来る時のみ位置を '.' として呼び出し可能です。
		/// </summary>
		/// <remarks>
		/// 想定される呼び出し状況は、
		/// ①単語の一文字目に数字があった場合
		/// ②単語の一文字目に '.' があり『その次に数字を確認した』場合
		/// 　(この場合は MoveToPos(0) 等を行って位置を '.' に合わせて下さい)
		/// </remarks>
		protected void ReadNumber() {
			this.wtype=WordType.Literal;
			if((this.lreader.CurrentLetter=='0')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				if((this.lreader.CurrentLetter=='x')||(this.lreader.CurrentLetter=='X')){
					ReadNumber_x();
					return;
				}
			}
			ReadNumber_f();
		}
		/// <summary>
		/// 16 進数を読み取ります。
		/// 文字列: "0";
		/// 位置: 'x' | 'X';
		/// </summary>
		private void ReadNumber_x(){
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [0-9a-fA-F]+
			if(!(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')||('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')||('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F')))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')||('a'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='f')||('A'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='F'));
			return;

		err:this.lreader.SetError(("0x の次には [0-9a-fA-F]+ が来るべきです"),0,null);
		}
		/// <summary>
		/// 通常の数を読み取ります。
		/// 文字列: "0" | "";
		/// 位置: 文字列に次に追加されるであろう文字;
		/// 注意: ".a" 等の文字列を与えられても一文字も読み取りません。無限ループになります。
		/// </summary>
		private void ReadNumber_f() {
			bool dot=false;
			while(true)switch(this.lreader.CurrentLetter){
				case '0':case '1':case '2':case '3':case '4':
				case '5':case '6':case '7':case '8':case '9':
					goto _add;
				case '.':
					// 既出だったら×
					if(dot)goto default;
					// 先読み 0-9 だったら○
					lreader.StoreCurrentPos(1);
					if(!this.lreader.MoveNext())return;
					if(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')){
						this.cword+='.';
						dot=true;
						goto _add;
					}
					lreader.MoveToPos(1);
					goto default;
				case 'e':case 'E':
					ReadNumber_e();
					if((this.lreader.CurrentLetter=='f')||(this.lreader.CurrentLetter=='F'))goto case 'f';
					return;
				case 'f':case 'F':
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				case 'u':case 'U':
					if(dot)goto default;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if((this.lreader.CurrentLetter=='l')||(this.lreader.CurrentLetter=='L'))goto case 'l';
					return;
				case 'l':case 'L':
					if(dot)goto default;
					this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
				default:
					return;
				_add:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
			}
		}
		/// <summary>
		/// 浮動小数点の指数部を読み取ります。
		/// 文字列: それ迄に読んだ数;
		/// 位置: 'e' | 'E'
		/// </summary>
		private void ReadNumber_e(){
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;

			// [+-]?
			if((this.lreader.CurrentLetter=='-')||(this.lreader.CurrentLetter=='+')){
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			}

			// \d+
			if(!(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9')))goto err;
			do{
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			}while(('0'<=this.lreader.CurrentLetter&&this.lreader.CurrentLetter<='9'));

			return;

		err:this.lreader.SetError(("指数表記の後には [+-]?\\d+ が必要です。"),0,null);
		}
	}
}