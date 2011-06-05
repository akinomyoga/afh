using afh.Parse;
namespace afh.HTML{
	public class HTMLWordReader:Parse.AbstractWordReader{
		public HTMLWordReader(string text):base(text){}
		public Context context=Context.global;

		public const int vCDATA=0x100;
		public const int vText=0x200;
		public const int vAttribute=0x300;
		public const int vDOCTYPE=0x400;
		public static readonly Parse.WordType wtCDATA=new afh.Parse.WordType(vCDATA);
		public static readonly Parse.WordType wtText=new afh.Parse.WordType(vText);
		public static readonly Parse.WordType wtAttribute=new afh.Parse.WordType(vAttribute);
		public static readonly Parse.WordType wtDOCTYPE=new afh.Parse.WordType(vDOCTYPE);
		
		public LinearLetterReader LinearReader{get{return this.lreader;}}
		public override bool ReadNext(){
			this.cword="";
			this.wtype=WordType.Invalid;
			while(lreader.CurrentType.IsInvalid||lreader.CurrentType.IsSpace){
				if(!lreader.MoveNext())return false;
			}
			this.lreader.StoreCurrentPos(0);
			switch(this.context){
				case Context.global:
					this.readNext_global();
					break;
				case Context.NMTOKEN:
					this.readNext_nmtoken();
					break;
				case Context.attribute:
					this.readNext_attribute();
					break;
			}
			return true;
		}

		#region global
		private void readNext_global(){
			this.wtype=WordType.Literal;
		#if MACRO_WORDREADER
			if([is:<]){
				this.wtype=WordType.Operator;
				[add][next]
				switch([letter]){
					case '/':
					case '?':
					case '%':
						[add][nexit]
					case '!':
						[add][next]
						if([is:-]){
							// <!--
							[add][next]
							if([not:-]){
								[error:"不完全なマークアップです。<!-- が来る事が期待されています。"]
								this.ReadText();break;
							}
							this.ReadComment();break;
						}else if([is:[]){
							// <![CDATA[
							[add][next]
							if(!this.ReadTest("CDATA[")){
								[error:"不完全なマークアップです。<![CDATA[ が来る事が期待されています。"]
								this.ReadText();break;
							}
							this.ReadCDATA();break;
						}else if([is:D]){
							// <!DOCTYPE
							[add][next]
							if(!this.ReadTest("OCTYPE")){
								[error:"不完全なマークアップです。<!DOCTYPE が来る事が期待されています。"]
								this.ReadText();break;
							}
							this.ReadDocType();break;
						}else{
							[error:"不完全なマークアップです。"]
							this.ReadText();break;
						}
					default:
						if([type].IsToken||[is:_])break;
						[error:"不完全なマークアップです。< の次に識別子を記述するか、'<' の代わりにエンティティ参照 &lt; を使用して下さい。"]
						this.ReadText();break;
				}
			}else ReadText();
		#endif
			#region #OUT#
			if(this.lreader.CurrentLetter=='<'){
				this.wtype=WordType.Operator;
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				switch(this.lreader.CurrentLetter){
					case '/':
					case '?':
					case '%':
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
					case '!':
						this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						if(this.lreader.CurrentLetter=='-'){
							// <!--
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
							if(this.lreader.CurrentLetter!='-'){
								this.lreader.SetError("不完全なマークアップです。<!-- が来る事が期待されています。",0,null);
								this.ReadText();break;
							}
							this.ReadComment();break;
						}else if(this.lreader.CurrentLetter=='['){
							// <![CDATA[
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
							if(!this.ReadTest("CDATA[")){
								this.lreader.SetError("不完全なマークアップです。<![CDATA[ が来る事が期待されています。",0,null);
								this.ReadText();break;
							}
							this.ReadCDATA();break;
						}else if(this.lreader.CurrentLetter=='D'){
							// <![CDATA[
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
							if(!this.ReadTest("OCTYPE")){
								this.lreader.SetError("不完全なマークアップです。<!DOCTYPE が来る事が期待されています。",0,null);
								this.ReadText();break;
							}
							this.ReadDocType();break;
						}else{
							this.lreader.SetError("不完全なマークアップです。",0,null);
							this.ReadText();break;
						}
					default:
						if(this.lreader.CurrentType.IsToken||this.lreader.CurrentLetter=='_')break;
						this.lreader.SetError("不完全なマークアップです。< の次に識別子を記述するか、'<' の代わりにエンティティ参照 &lt; を使用して下さい。",0,null);
						this.ReadText();break;
				}
			}else ReadText();
			#endregion #OUT#
		}
		/// <summary>
		/// 続く文字列が指定した文字列に一致するかどうかを判定します。
		/// 現在位置は一致した最後の文字の次になります。
		/// </summary>
		/// <param name="str">一致の対象となる文字列を指定します。</param>
		/// <returns>一致した場合に true を、そうでない場合に false を返します。</returns>
		private bool ReadTest(string str){
			if(str==null)return false;
			if(str=="")return true;
			int max=str.Length;
		#if MACRO_WORDREADER
			for(int index=0;index<max;index++){
				if([letter]!=str[index])return false;
				[add][if!next]return index+1==max;
			}
		#endif
			#region #OUT#
			for(int index=0;index<max;index++){
				if(this.lreader.CurrentLetter!=str[index])return false;
				this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return index+1==max;
			}
			#endregion #OUT#
			return true;
		}
		/// <summary>
		/// [^&lt;]* を読み取ります。
		/// </summary>
		private void ReadText(){
			this.wtype=wtText;
		#if MACRO_WORDREADER
			while(true)switch([letter]){
				case '<':
					return;
				default:
					[add][next]
					break;
			}
		#endif
			#region #OUT#
			while(true)switch(this.lreader.CurrentLetter){
				case '<':
					return;
				default:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					break;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// コメントの内容を読み取ります。
		/// 現在位置: &lt;!-- の最後の -。
		/// cword: (消すので何でも良い)
		/// </summary>
		private void ReadComment(){
			this.wtype=WordType.Comment;
			this.cword="";
		#if MACRO_WORDREADER
			while(true){
				[if!next]{
					[error:"コメントに終端 --> がありません。"]
					return;
				}
				if([is:-]&&ReadTest("-->")){
					this.cword=this.cword.Substring(0,this.cword.Length-3);
					return;
				}
				[add]
			}
		#endif
			#region #OUT#
			while(true){
				if(!this.lreader.MoveNext()){
					this.lreader.SetError("コメントに終端 --> がありません。",0,null);
					return;
				}
				if(this.lreader.CurrentLetter=='-'&&ReadTest("-->")){
					this.cword=this.cword.Substring(0,this.cword.Length-3);
					return;
				}
				this.cword+=this.lreader.CurrentLetter;
			}
			#endregion #OUT#
		}
		/// <summary>
		/// (.*?)]\s*]\s*> を読み取ります。
		/// this.cword に $1 を追加します。
		/// </summary>
		private void ReadCDATA(){
			this.wtype=HTMLWordReader.wtCDATA;
			int mode=0;
			string cache="";
			this.cword="";
		#if MACRO_WORDREADER
			if(lreader.IsEndOfText){
				[error:"<![CDATA[ に末端がありません。"]
				return;
			}
			do switch(mode){
				case 0:
					if([is:]]){
						mode++;
						cache+=[letter];
					}else{
						[add]
					}
					break;
				case 1:
					if([is:]]){
						mode++;
						cache+=[letter];
					}else if([type].IsSpace){
						cache+=[letter];
					}else{
						mode=0;
						this.cword+=cache;
						cache="";
					}
					break;
				case 2:
					if([is:>]){
						[nexit]
					}else if([type].IsSpace){
						cache+=[letter];
					}else{
						mode=0;
						this.cword+=cache;
						cache="";
					}
					break;
			}while(lreader.MoveNext());
			[error:"<![CDATA[ に末端がありません。"]
		#endif
			#region #OUT#
			if(lreader.IsEndOfText){
				this.lreader.SetError("<![CDATA[ に末端がありません。",0,null);
				return;
			}
			do switch(mode){
				case 0:
					if(this.lreader.CurrentLetter==']'){
						mode++;
						cache+=this.lreader.CurrentLetter;
					}else{
						this.cword+=this.lreader.CurrentLetter;
					}
					break;
				case 1:
					if(this.lreader.CurrentLetter==']'){
						mode++;
						cache+=this.lreader.CurrentLetter;
					}else if(this.lreader.CurrentType.IsSpace){
						cache+=this.lreader.CurrentLetter;
					}else{
						mode=0;
						this.cword+=cache;
						cache="";
					}
					break;
				case 2:
					if(this.lreader.CurrentLetter=='>'){
						this.lreader.MoveNext();return;
					}else if(this.lreader.CurrentType.IsSpace){
						cache+=this.lreader.CurrentLetter;
					}else{
						mode=0;
						this.cword+=cache;
						cache="";
					}
					break;
			}while(lreader.MoveNext());
			this.lreader.SetError("<![CDATA[ に末端がありません。",0,null);
			#endregion #OUT#
		}
		/// <summary>
		/// コメントの内容を読み取ります。
		/// 現在位置: &lt;!DOCTYPE の最後の E。
		/// cword: (消すので何でも良い)
		/// </summary>
		private void ReadDocType(){
			this.wtype=HTMLWordReader.wtDOCTYPE;
			this.cword="";
			bool bra=false; // [] で囲まれる部分を既に処理したか
			int mode=0;
		#if MACRO_WORDREADER
			while(true)switch(mode){
				case 0:
					if([is:[]){
						if(bra){
							[error:"DOCTYPE 宣言に二つ以上の [] で囲まれる部分が出て来るのは変です。"]
							bra=true;
						}
						ReadDocType_bracket();
						while([type].IsSpace||[type].IsInvalid){
							[add][next]
						}
						break;
					}else if([is:>]){
						[nexit] // 最後の > は含めない
					}else if([is:\']){
						mode=1;
					}else if([is:"]){
						mode=3;
					}
					goto addxt;
				case 1: // '' 内
					if([is:\\])mode=2;
					else if([is:\'])mode=0;
					goto addxt;
				case 3: // "" 内
					if([is:\\])mode=4;
					else if([is:"])mode=0;
					goto addxt;
				case 2: // '' 内 \ 後
				case 4: // "" 内 \ 後
					mode--;
					goto addxt;
				addxt:
					[add][if!next]{
						[error:"終わりの括弧 '>' がありません。"]
					}
			}
		#endif
			#region #OUT#
			while(true)switch(mode){
				case 0:
					if(this.lreader.CurrentLetter=='['){
						if(bra){
							this.lreader.SetError("DOCTYPE 宣言に二つ以上の [] で囲まれる部分が出て来るのは変です。",0,null);
							bra=true;
						}
						ReadDocType_bracket();
						while(this.lreader.CurrentType.IsSpace||this.lreader.CurrentType.IsInvalid){
							this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
						}
						break;
					}else if(this.lreader.CurrentLetter=='>'){
						this.lreader.MoveNext();return; // 最後の > は含めない
					}else if(this.lreader.CurrentLetter=='\''){
						mode=1;
					}else if(this.lreader.CurrentLetter=='"'){
						mode=3;
					}
					goto addxt;
				case 1: // '' 内
					if(this.lreader.CurrentLetter=='\\')mode=2;
					else if(this.lreader.CurrentLetter=='\'')mode=0;
					goto addxt;
				case 3: // "" 内
					if(this.lreader.CurrentLetter=='\\')mode=4;
					else if(this.lreader.CurrentLetter=='"')mode=0;
					goto addxt;
				case 2: // '' 内 \ 後
				case 4: // "" 内 \ 後
					mode--;
					goto addxt;
				addxt:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
						this.lreader.SetError("終わりの括弧 '>' がありません。",0,null);
					}
					break;
			}
			#endregion #OUT#
		}
		private void ReadDocType_bracket(){
		#if MACRO_WORDREADER
			int mode=0;
			while(true)switch(mode){
				case 0:
					if([is:]]){
						[add][nexit]
					}else if([is:\']){
						mode=1;
					}else if([is:"]){
						mode=3;
					}
					goto addxt;
				case 1: // '' 内
					if([is:\\])mode=2;
					else if([is:\'])mode=0;
					goto addxt;
				case 3: // "" 内
					if([is:\\])mode=4;
					else if([is:"])mode=0;
					goto addxt;
				case 2: // '' 内 \ 後
				case 4: // "" 内 \ 後
					mode--;
					goto addxt;
				addxt:
					[add][if!next]{
						[error:"終わりの括弧 ']' がありません。"]
					}
			}
		#endif
			#region #OUT#
			int mode=0;
			while(true)switch(mode){
				case 0:
					if(this.lreader.CurrentLetter==']'){
						this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
					}else if(this.lreader.CurrentLetter=='\''){
						mode=1;
					}else if(this.lreader.CurrentLetter=='"'){
						mode=3;
					}
					goto addxt;
				case 1: // '' 内
					if(this.lreader.CurrentLetter=='\\')mode=2;
					else if(this.lreader.CurrentLetter=='\'')mode=0;
					goto addxt;
				case 3: // "" 内
					if(this.lreader.CurrentLetter=='\\')mode=4;
					else if(this.lreader.CurrentLetter=='"')mode=0;
					goto addxt;
				case 2: // '' 内 \ 後
				case 4: // "" 内 \ 後
					mode--;
					goto addxt;
				addxt:
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
						this.lreader.SetError("終わりの括弧 ']' がありません。",0,null);
					}
					break;
			}
			#endregion #OUT#
		}
		#endregion

		private void readNext_nmtoken(){
		#if MACRO_WORDREADER
			if(![type].IsToken&&[not:_])return;
			bool colon=false;
			[add][next]
			while(true){
				if([type].IsToken||[type].IsNumber||[is:-]||[is:_]){
					[add][next]
				}else if([is::]){
					if(colon)return;
					colon=true;
					[add][next]
				}else return;
			}
		#endif
			#region #OUT#
			if(!this.lreader.CurrentType.IsToken&&this.lreader.CurrentLetter!='_')return;
			bool colon=false;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
			while(true){
				if(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||this.lreader.CurrentLetter=='-'||this.lreader.CurrentLetter=='_'){
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				}else if(this.lreader.CurrentLetter==':'){
					if(colon)return;
					colon=true;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				}else return;
			}
			#endregion #OUT#
		}

		#region attribute
		private void readNext_attribute(){
		#if MACRO_WORDREADER
			if([is:/]||[is:?]){
				[add]
				if(lreader.MoveNext()&&[is:>]){
					this.wtype=WordType.Operator;
					[add][next]
					return;
				}else{
					this.wtype=WordType.Invalid;
					[error:this.cword+" は無効です。"+this.cword+" の後に > が来ていません。"]
					return;
				}
			}else if([is:>]){
				this.wtype=WordType.Operator;
				[add][nexit]
			}else if(!read_attrname()){
				this.wtype=WordType.Invalid;
				[error:"無効な属性名です。"]
				[add][nexit]
			}

			// Boundary
			this.wtype=wtAttribute;
			this.cword+="=";
			while([type].IsSpace||[type].IsInvalid)[next]
			if([not:=])return;
			[next]
			while([type].IsSpace||[type].IsInvalid)[next]

			// Read Attribute Value
			lreader.StoreCurrentPos(0);
			this.read_attrvalue();
		#endif
			#region #OUT#
			if(this.lreader.CurrentLetter=='/'||this.lreader.CurrentLetter=='?'){
				this.cword+=this.lreader.CurrentLetter;
				if(lreader.MoveNext()&&this.lreader.CurrentLetter=='>'){
					this.wtype=WordType.Operator;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
					return;
				}else{
					this.wtype=WordType.Invalid;
					this.lreader.SetError(this.cword+" は無効です。"+this.cword+" の後に > が来ていません。",0,null);
					return;
				}
			}else if(this.lreader.CurrentLetter=='>'){
				this.wtype=WordType.Operator;
				this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
			}else if(!read_attrname()){
				this.wtype=WordType.Invalid;
				this.lreader.SetError("無効な属性名です。",0,null);
				this.cword+=this.lreader.CurrentLetter;this.lreader.MoveNext();return;
			}

			// Boundary
			this.wtype=wtAttribute;
			this.cword+="=";
			while(this.lreader.CurrentType.IsSpace||this.lreader.CurrentType.IsInvalid)if(!this.lreader.MoveNext())return;
			if(this.lreader.CurrentLetter!='=')return;
			if(!this.lreader.MoveNext())return;
			while(this.lreader.CurrentType.IsSpace||this.lreader.CurrentType.IsInvalid)if(!this.lreader.MoveNext())return;

			// Read Attribute Value
			lreader.StoreCurrentPos(0);
			this.read_attrvalue();
			#endregion #OUT#
		}
		private bool read_attrname(){
		#if MACRO_WORDREADER
			if(![type].IsToken&&[not:_])return false;
			[add][if!next]return true;

			bool colon=false;
			while(true){
				if([type].IsToken||[type].IsNumber||[is:-]||[is:_]){
					[add][if!next]return true;
				}else if([is::]){
					if(colon){
						[error:"colon が過剰です"]
						return false;
					}
					colon=true;
					[add][if!next]{
						[error:"コロンの後に識別子が必要です。"]
						return false;
					}
					if(![type].IsToken&&[not:_]){
						[error:"コロンの後に識別子が必要です。"]
						return false;
					}
					[add][if!next]return true;
				}else return true;
			}
		#endif
			#region #OUT#
			if(!this.lreader.CurrentType.IsToken&&this.lreader.CurrentLetter!='_')return false;
			this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return true;

			bool colon=false;
			while(true){
				if(this.lreader.CurrentType.IsToken||this.lreader.CurrentType.IsNumber||this.lreader.CurrentLetter=='-'||this.lreader.CurrentLetter=='_'){
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return true;
				}else if(this.lreader.CurrentLetter==':'){
					if(colon){
						this.lreader.SetError("colon が過剰です",0,null);
						return false;
					}
					colon=true;
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
						this.lreader.SetError("コロンの後に識別子が必要です。",0,null);
						return false;
					}
					if(!this.lreader.CurrentType.IsToken&&this.lreader.CurrentLetter!='_'){
						this.lreader.SetError("コロンの後に識別子が必要です。",0,null);
						return false;
					}
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return true;
				}else return true;
			}
			#endregion #OUT#
		}
		private void read_attrvalue(){
		#if MACRO_WORDREADER
			if([is:\']||[is:"]){
				char end=[letter];
				[if!next]{
					[error:"文字列に終端がありません。"+end+" が必要です"]
					return;
				}
				while([letter]!=end){
					[add][if!next]{
						[error:"文字列に終端がありません。"+end+" が必要です"]
					}
				}
				[nexit]
			}else{
				while(![type].IsSpace&&![type].IsInvalid&&[not:<]&&[not:>]&&[not:/]&&[not:?]){
					[add][next]
				}
			}
		#endif
			#region #OUT#
			if(this.lreader.CurrentLetter=='\''||this.lreader.CurrentLetter=='"'){
				char end=this.lreader.CurrentLetter;
				if(!this.lreader.MoveNext()){
					this.lreader.SetError("文字列に終端がありません。"+end+" が必要です",0,null);
					return;
				}
				while(this.lreader.CurrentLetter!=end){
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext()){
						this.lreader.SetError("文字列に終端がありません。"+end+" が必要です",0,null);
					}
				}
				this.lreader.MoveNext();return;
			}else{
				while(!this.lreader.CurrentType.IsSpace&&!this.lreader.CurrentType.IsInvalid&&this.lreader.CurrentLetter!='/'&&this.lreader.CurrentLetter!='<'&&this.lreader.CurrentLetter!='>'){
					this.cword+=this.lreader.CurrentLetter;if(!this.lreader.MoveNext())return;
				}
			}
			#endregion #OUT#
		}
		#endregion

		public enum Context{
			global,
			NMTOKEN,
			attribute,
		}
	}
}