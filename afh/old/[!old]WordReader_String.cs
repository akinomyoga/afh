namespace afh.Parse{
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
	public abstract partial class AbstractWordReader{
		/// <summary>
		/// double quotation で囲まれた文字列を取得します。
		/// 改行は許されていません。
		/// 現在位置に " が在る状態で呼び出して下さい。
		/// </summary>
		protected void ReadStringDQ(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '"':
						[add][nexit];
					case '\\':skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		err:[error:"文字列に終端の \" が存在しません。"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '"':
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;;
					case '\\':skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError("文字列に終端の \" が存在しません。",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// single quotation で囲まれた文字列を取得します。
		/// 改行は許されていません。
		/// 現在位置に ' が在る状態で呼び出して下さい。
		/// </summary>
		protected void ReadStringSQ(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '\'':
						[add][nexit];
					case '\\':
						skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		err:[error:"文字列に終端の ' が存在しません。"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '\'':
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;;
					case '\\':
						skip=true;
						goto default;
					default:
						this.cword+=this.lreader.CurrentLetter;break;
				}
				if(!this.lreader.MoveNext())goto err;
			}
		err:this.lreader.SetError("文字列に終端の ' が存在しません。",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// 複数行文字列を取得します。
		/// 位置: '"' の位置;
		/// 文字列: "@";
		/// </summary>
		protected void ReadStringMulti(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			while(true)switch([letter]){
				case '"':
					[add][next]
					if([not:"])return;
					goto default;
				default:
					[add][if!next]goto err;
					break;
			}
		err:[error:"文字列に終端の \" が存在しません。"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			while(true)switch(this.lreader.CurrentLetter){
				case '"':
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					if(this.lreader.CurrentLetter!='"')return;
					goto default;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
					break;
			}
		err:this.lreader.SetError("文字列に終端の \" が存在しません。",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// 正規表現リテラルを取得します。
		/// </summary>
		protected void ReadRegExp(){
			this.wtype=WordType.Literal;
#if MACRO_WORDREADER
			[add][if!next]goto err;
			bool skip=false;
			while(true){
				if([is:term])goto err;
				if(skip){
					[add]
					skip=false;
				}else switch([letter]){
					case '/':
						[add][next];
						goto suffix;
					case '\\':
						skip=true;
						goto default;
					default:
						[add]break;
				}
				[if!next]goto err;
			}
		suffix:
			while(true)switch([letter]){
				case 'g':case 'i':case 'm':
					[add][next]
					break;
				default:
					return;
			}

		err:[error:"正規表現リテラルに終端の / が存在しません。"]
#endif
			#region #OUT#
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())goto err;
			bool skip=false;
			while(true){
				if(this.lreader.CurrentLetter=='\r'||this.lreader.CurrentLetter=='\n'||this.lreader.CurrentLetter=='\u2028'||this.lreader.CurrentLetter=='\u2029')goto err;
				if(skip){
					this.cword+=this.lreader.CurrentLetter;
					skip=false;
				}else switch(this.lreader.CurrentLetter){
					case '/':
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;;
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

		err:this.lreader.SetError("正規表現リテラルに終端の / が存在しません。",0,null);
			#endregion #OUT#
		}
	}
}