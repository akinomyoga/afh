/*
	このソースコードは [afh.Design.dll] afh.Design.TemplateProcessor によって自動的に生成された物です。
	このソースコードを変更しても、このソースコードの元になったファイルを変更しないと変更は適用されません。

	This source code was generated automatically by a file-generator, '[afh.Design.dll] afh.Design.TemplateProcessor'.
	Changes to this source code may not be applied to the binary file, which will cause inconsistency of the whole project.
	If you want to modify any logics in this file, you should change THE SOURCE OF THIS FILE. 
*/
using Gen=System.Collections.Generic;
using BaseRegexFactory=afh.RegularExpressions.RegexFactory3<char>;

namespace afh.RegularExpressions{
	using INode=afh.RegularExpressions.RegexFactory3<char>.INode;
	using ITester=afh.RegularExpressions.RegexFactory3<char>.ITester;
	using ElemNodeBase=afh.RegularExpressions.RegexFactory3<char>.ElemNodeBase;

	/// <summary>
	/// 通常の文字列に対する正規表現を提供します。
	/// </summary>
	internal static partial class CharClasses{
		internal partial class CharClassNodeGenerator{
			//#define EOF		(i>=expression.Length)
			//#define NOT(c)	(expression[i]!=(c))
			//#define NEXT		(++i<expression.Length)
			//#define IS(c)		(expression[i]==(c))
			private IClassHandler ReadClass(){
				bool mode=(expression[i]!=('^')); // [...] or [^...]
				if(!mode)i++;
				ClassHandlerGenerator gen=new ClassHandlerGenerator(mode);

				while(i<expression.Length){
					char c1,c2;
					if(ReadChar(out c1)){
						c2='\0';
					}else{
						c2=expression[i];
					}

					switch(c2){
						case '\0':
							// 通常の文字
							if((i>=expression.Length)||(expression[i]!=('-'))){
								gen.Add(new AtomHandler(c1),false);
								break;
							}

							// 範囲指定 ?
							if((++i<expression.Length)){
								if(ReadChar(out c2)){
									gen.Add(new RangeHandler(c1,c2),false);
									break;
								}
								if((expression[i]!=('[')))
									this.ReportError("文字範囲指定 「a-b」 の終端に当たる文字がありません。或いは - にエスケープが必要です。");
							}
							i--; // 範囲指定でなかったので無かった事にする

							// やはり通常の文字
							gen.Add(new AtomHandler(c1),false);
							break;
						case '\\':{
							if(!(++i<expression.Length)){
								this.ReportError(@"\ に続く文字が必要です。");
								goto ED;
							}

							//-- 名前付き文字クラス
							if(c1=='p'||c1=='P'){
								gen.Add(this.CreateNamedClass(),false);
							}else{
								this.ReportError("認識出来ないコマンドです。");
								i++;
							}
							break;
						}
						case '-':
							if((++i<expression.Length)&&(expression[i]==('['))){
								i++;
								gen.Add(ReadClass(),true);
								break;
							}
							gen.Add(new AtomHandler(c1),false);
							break;
						case ']':
							i++;
							goto ED;
						default:
							__debug__.RegexParserAssert(false);
							break;
					}
				}
			ED:
				return gen.Create();
			}
			/// <summary>
			/// 通常の文字を読み取れる場合には、その文字を読み取って true を返します。
			/// 通常の文字でない場合には、読み取らずに false を返します。
			/// </summary>
			/// <param name="c">読み取った文字を返します。</param>
			/// <returns>通常の文字を読み取れた場合に true を返します。それ以外の場合に false を返します。</returns>
			/// <remarks>CharClassNodeGenerator.i は、文字を読み取った場合には、その次の文字の位置を指した状態で返します。</remarks>
			private bool ReadChar(out char c){
				c=expression[i];

				switch(c){
					case '\\':{
						if(!(++i<expression.Length)){
							this.ReportError(@"\ に続く文字が必要です。");
							i--;
							return false;
						}

						//-- 特別の文字
						int index=COMMAND_LETTERS_SPACE.IndexOf(expression[i]);
						if(index>=0){
							c=SPACE_LETTERS[index];
							return true;
						}

						//-- 文字エスケープ
						index=CONTROL_LETTERS.IndexOf(expression[i]);
						if(index>=0){
							c=expression[i];
							return true;
						}

						i--;
						return false;
					}
					case '-':
					case ']':
						return false;
					default:
						i++;
						return true;
				}
			}
			/// <summary>
			/// 名前付き文字クラス指定を読み取ります。
			/// </summary>
			/// <returns>読み取りが成功した場合に判定関数を返します。それ以外の場合に null を返します。</returns>
			private IClassHandler CreateNamedClass(){
				char c=expression[i];

				//-- 読み取り
				if(!(++i<expression.Length)||(expression[i]!=('{'))){
					this.ReportError("名前付きクラスに引数が割り当てられていません。");
					return null;
				}

				string arg="";
				while((++i<expression.Length)&&(expression[i]!=('}')))
					arg+=expression[i];

				if((i>=expression.Length)){
					this.ReportError("名前付きクラスの引数に終端 '}' がありません。");
					return null;
				}else i++;

				//-- インスタンス作成
				return CreateNamedClassHandler(c=='p',arg);
			}
		}
	}
}