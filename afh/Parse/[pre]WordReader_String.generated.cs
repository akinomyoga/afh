/*
	このソースコードは [afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Parse.Design] afh.Parse.Design.WordReaderPreprocessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
namespace afh.Parse {
	//-------------------------------------------------
	//		文字列リテラル
	//		正規表現リテラル
	//-------------------------------------------------
	//		"([^"\\]|\[^\r\n])*"
	//		'([^'\\]|\[^\r\n])*'
	//		@"([^"]|"")*"
	//		
	//		/([^/\\]|\[^\r\n])*/[igm]*
	//	原則:
	//		文字列:***;位置:***:
	//		で指定される条件を満たす状態で呼び出す事
	//-------------------------------------------------
	public abstract partial class AbstractWordReader {
		/// <summary>
		/// double quotation で囲まれた文字列を取得します。
		/// 改行は許されていません。
		/// 現在位置に " が在る状態で呼び出して下さい。
		/// </summary>
		protected void ReadStringDQ() {
			this.wtype=WordType.Literal;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if((this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029'))goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '"':
						this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
					case '\\':skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError(("文字列に終端の \" が存在しません。"),0,null);
		}
		/// <summary>
		/// single quotation で囲まれた文字列を取得します。
		/// 改行は許されていません。
		/// 現在位置に ' が在る状態で呼び出して下さい。
		/// </summary>
		protected void ReadStringSQ() {
			this.wtype=WordType.Literal;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if((this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029'))goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '\'':
						this.cword+=this.lreader.CurrentLetter;{this.lreader.MoveNext();return;}
					case '\\':
						skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError(("文字列に終端の ' が存在しません。"),0,null);
		}
		/// <summary>
		/// 複数行文字列を取得します。
		/// 位置: '"' の位置;
		/// 文字列: "@";
		/// </summary>
		protected void ReadStringMulti() {
			this.wtype=WordType.Literal;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			while(true)switch(this.lreader.CurrentLetter){
				case '"':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(!(this.lreader.CurrentLetter=='\"'))return;
					goto default;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		err:this.lreader.SetError(("文字列に終端の \" が存在しません。"),0,null);
		}
		/// <summary>
		/// 正規表現リテラルを取得します。
		/// </summary>
		protected void ReadRegExp(){
			this.wtype=WordType.Literal;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if((this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029'))goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '/':
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		suffix:
			while(true)switch(this.lreader.CurrentLetter){
				case 'g':case 'i':case 'm':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
				default:
					return;
			}

		err: this.lreader.SetError(("正規表現リテラルに終端の / が存在しません。"),0,null);
		}
	}
}