using Reg=System.Text.RegularExpressions;

// __minor(continuous)
//--------------------------------------------------------------------
// ・if(...)return; には対応していません。if(...){...} と書きましょう。
// ・入れ子のループには対応していません。(break が変な事になります。)
//--------------------------------------------------------------------
namespace mwg_minor {
	public static class Program{
		public static void Main(string[] args){
			if(args.Length<1){
				WriteInstruction();
				return;
			}

			if(!System.IO.File.Exists(args[0])){
				System.Console.WriteLine("! 指定されたファイルは存在しません。");
				return;
			}

			string content=System.IO.File.ReadAllText(args[0],System.Text.Encoding.Default);
			if(!Process(ref content)){
				System.Console.WriteLine("! 処理に失敗しました。");
				return;
			}

			System.IO.File.WriteAllText(args[0]+".tmp",content,System.Text.Encoding.Default);
			System.IO.File.Replace(args[0]+".tmp",args[0],args[0]+".bk");
		}

		private static void WriteInstruction(){
			System.Console.WriteLine("Usage 使い方");
			System.Console.WriteLine("\tmwg_minor <入力ファイル名>");
			System.Console.WriteLine("__minor(continuous)");
		}

		private static bool Process(ref string content){
			// #if __MWG_MINOR
			string PATTERN_MWG_MINOR
				=(@"(?<pro>:N)\#if\s+__MWG_MINOR\s*:n(?<src>[\s\S]+?):n"
				+@"\s*\#else\s*:n(?:\s*\#region\b[^\r\n]+:n)?(?<tgt>[\s\S]+?):n"
				+@"(?:\s*\#endregion\b[^\r\n]+:n)?\#endif(?<epi>:N)");

			content=RegExp.Replace(content,PATTERN_MWG_MINOR,delegate(Reg::Match m){
				System.Console.WriteLine("__MWG_MINOR Block: {0}+{1}",m.Index,m.Length);

				string result=Translate(m.Groups["src"].Value);

				return string.Format(@"{2}#if __MWG_MINOR
{0}
#else
#region generated from __minor code
{1}
#endregion
#endif{3}",m.Groups["src"].Value,result,m.Groups["pro"],m.Groups["epi"]);
			});
			return true;
		}

		private static string Translate(string source){
			string PATTERN_CONTINUOUS
				=@"__minor:b\(:b(?:instance:b)?continuous:b\):b"
				+@"\{(?<src>:{braced})\}";

			source=RegExp.Replace(source,PATTERN_CONTINUOUS,delegate(Reg::Match m) {
				string members;
				string continued=ContinuousProcessor.Process(m.Groups["src"].Value,out members);
				return "{"+continued+"}\r\n"+members;
			});
			return source;
		}
	}
	internal static class RegExp{
		private const string BRACED_SEQ=@"(?:[^\{\}]|(?<open>\{)|(?<close-open>\}))*(?(open)(?!))";

		public static string Replace(string text,string pattern,Reg::MatchEvaluator deleg){
			return Reg::Regex.Replace(text,Modify(pattern),deleg);
		}
		public static string InfiniteReplace(string text,string pattern,Reg::MatchEvaluator deleg){
			pattern=Modify(pattern);
			Reg::Regex regex=new Reg::Regex(pattern,Reg::RegexOptions.Compiled);

			int i;
			do{
				i=0;
				text=regex.Replace(text,delegate(Reg::Match m){
					i++;
					return deleg(m);
				});
			}while(i>0);

			return text;
		}

		private static string Modify(string regex){
			regex=regex.Replace(":{expression}",@"([^\(\)""'\{\}\;]|:""|:'|(?<open>\()|(?<close-open>\)))*(?(open)(?!))");
			regex=regex.Replace(":{braced}",BRACED_SEQ);
			regex=regex.Replace(":b",@"(?:\b|[\s\r\n\f]+)");
			regex=regex.Replace(":\"",@"""([^""\\\r\n\f]\.)*""");
			regex=regex.Replace(":'",@"'([^'\\\r\n\f]\.)*'");
			regex=regex.Replace(":N","(?:^|$|\r?\n|\r)");
			regex=regex.Replace(":n","(?:\r?\n|\r)");
			return regex;
		}

		public class Match:System.IFormattable{
			readonly Reg::Match m;
			public Match(Reg::Match m){
				this.m=m;
			}
			public static implicit operator Match(Reg::Match m){
				return new Match(m);
			}

			public string ToString(string format,System.IFormatProvider formatProvider) {
				if(format=="")return m.Value;
				return m.Groups[format].Value;
			}
		}
	}
	internal static class ContinuousProcessor{
		public static string Process(string input,out string member_decls){
			string START="";
			string END="";
			string LINEHEAD="";
			{
				Reg::Match m1=Reg::Regex.Match(input,@"^\s*(?:\r?\n|\r)(?<linehead>\s*)");
				if(m1.Success){
					int i=m1.Groups["linehead"].Index;
					START=input.Substring(0,i);
					input=input.Substring(i);

					LINEHEAD=m1.Groups["linehead"].Value;
				}

				m1=Reg::Regex.Match(input,@"(?:\r?\n|\r)(?<spaces>\s*)$");
				if(m1.Success){
					int i=m1.Groups["spaces"].Index;
					END=input.Substring(i);
					input=input.Substring(0,i);
				}else{
					input+="\r\n";
				}
			}
			//--------------------------------------------------------
			//		Flattening
			//--------------------------------------------------------
			// while(){} の処理
			string PATTERN=@"\bwhile:b\((?<condition>:{expression})\)\s*\{(?<content>:{braced})\}";
			input=RegExp.InfiniteReplace(input,PATTERN,delegate(Reg::Match m){
				string l=CreateLabel();
				string l2=CreateLabel();
				string content=m.Groups["content"].Value
					.Replace("break;","goto "+l2+";")
					.Replace("continue;","goto "+l+";");
				bool infinite=m.Groups["condition"].Value=="true";
				return string.Format(
					@"{0}:"+(infinite?"":"if(!({1}))goto {3};")+"{2}goto {0};{3}:;",
					l,m.Groups["condition"].Value,content,l2);
			});

			// for(;;) の処理
			PATTERN=@"\bfor:b\((?<decl>:{expression})\;(?<cond>:{expression})\;(?<epil>:{expression})\)"
				+@"\s*\{(?<content>:{braced})\}";
			input=RegExp.InfiniteReplace(input,PATTERN,delegate(Reg::Match m){
				string L1=CreateLabel();
				string L2=CreateLabel();
				string L3=CreateLabel();
				string content=m.Groups["content"].Value
					.Replace("break;","goto "+L3+";")
					.Replace("continue;","goto "+L2+";");
				bool infinite=m.Groups["cond"].Value=="true"||m.Groups["cond"].Value=="";

				return string.Format(
					@"{0:decl};{1}:"+(infinite?"":"if(!({0:cond}))goto {3};")
					+	"{4}"
					+@"{2}:{0:epil};goto {1};{3}:;",
					(RegExp.Match)m,L1,L2,L3,content);
			});

			// if(){}else{} の処理
			PATTERN=@"\bif:b\((?<condition>:{expression})\)\s*\{(?<content>:{braced})\}:belse:b\{(?<else>:{braced})\}";
			input=RegExp.InfiniteReplace(input,PATTERN,delegate(Reg::Match m){
				string l=CreateLabel();
				string l2=CreateLabel();
				return string.Format(
					@"if(!({0:condition}))goto {1};{0:content}goto {2};{1}:{0:else}{2}:;",
					new RegExp.Match(m),l,l2);
			});

			// if(){} の処理
			PATTERN=@"\bif:b\((?<condition>:{expression})\)\s*\{(?<content>:{braced})\}";
			input=RegExp.InfiniteReplace(input,PATTERN,delegate(Reg::Match m){
				string l=CreateLabel();
				return string.Format(
					@"if(!({0:condition}))goto {1};{0:content}{1}:;",
					new RegExp.Match(m),l);
			});
			//--------------------------------------------------------
			//		Instance Members;
			//--------------------------------------------------------
			PATTERN=@"\binstance:b(?<decl>(?:[\w\<\>\.]|\:\:)+:b(?<var>\w+))\s*(?<scolon>\;?)";
			System.Text.StringBuilder members=new System.Text.StringBuilder();
			members.Append(LINEHEAD);
			members.AppendLine("private int continuous_state=0;");
			input=RegExp.Replace(input,PATTERN,delegate(Reg::Match m){
				members.Append(LINEHEAD);
				members.AppendFormat("private {0};",m.Groups["decl"]);
				members.AppendLine();

				if(m.Groups["scolon"].Length==1)return "";
				return m.Groups["var"].Value+" ";
			});
			member_decls=members.ToString();

			//--------------------------------------------------------
			//		Replace Return
			//--------------------------------------------------------
			System.Text.StringBuilder initial_switch=new System.Text.StringBuilder();
			initial_switch.Append(LINEHEAD);
			initial_switch.AppendLine("switch(continuous_state){");
			initial_switch.Append(LINEHEAD);
			initial_switch.AppendLine("\tdefault:throw new System.Exception(\"この関数の実行は終了しています。\");");
			initial_switch.Append(LINEHEAD);
			initial_switch.AppendLine("\tcase 0:break;");


			int state=1;

			PATTERN=@"\b(?<statement>break\s+return|return)(?<ret>:b:{expression})\;";
			input=RegExp.Replace(input,PATTERN,delegate(Reg::Match m){
				if(m.Groups["statement"].Length>6){
					// break return;
					return string.Format("continuous_state=-1;return {0:ret};",(RegExp.Match)m);
				}else{
					// return;
					string R1=CreateLabelR();

					initial_switch.Append(LINEHEAD);
					initial_switch.AppendFormat("\tcase {0}:goto {1};\r\n",state,R1);

					return string.Format("continuous_state={0};return {1:ret};{2}:;",state++,(RegExp.Match)m,R1);
				}
			});

			initial_switch.Append(LINEHEAD);
			initial_switch.AppendLine("}");

			return START+initial_switch+"#pragma warning disable 164\r\n"+input+"#pragma warning restore 164\r\n"+END;
		}

		static int index=0;
		private static string CreateLabel(){
			return "L"+index++.ToString();
		}
		static int indexR=0;
		private static string CreateLabelR(){
			return "R"+indexR++.ToString();
		}
	}
}