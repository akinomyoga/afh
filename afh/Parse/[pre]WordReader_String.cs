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
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '"':
						add;nexit;
					case '\\':skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		err:error("文字列に終端の \" が存在しません。");
		}
		/// <summary>
		/// single quotation で囲まれた文字列を取得します。
		/// 改行は許されていません。
		/// 現在位置に ' が在る状態で呼び出して下さい。
		/// </summary>
		protected void ReadStringSQ() {
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '\'':
						add;nexit;
					case '\\':
						skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		err:error("文字列に終端の ' が存在しません。");
		}
		/// <summary>
		/// 複数行文字列を取得します。
		/// 位置: '"' の位置;
		/// 文字列: "@";
		/// </summary>
		protected void ReadStringMulti() {
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			while(true)switch(letter){
				case '"':
					add;next;
					if("not:\"")return;
					goto default;
				default:
					add;if(!next)goto err;
					break;
			}
		err:error("文字列に終端の \" が存在しません。");
		}
		/// <summary>
		/// 正規表現リテラルを取得します。
		/// </summary>
		protected void ReadRegExp(){
			this.wtype=WordType.Literal;
			add;if(!next)goto err;
			bool skip=false;
			while(true){
				if("is:term")goto err;
				if(skip){
					add;
					skip=false;
				}else switch(letter){
					case '/':
						add;next;
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						add;break;
				}
				if(!next)goto err;
			}
		suffix:
			while(true)switch(letter){
				case 'g':case 'i':case 'm':
					add;next;
					break;
				default:
					return;
			}

		err: error("正規表現リテラルに終端の / が存在しません。");
		}
	}
}