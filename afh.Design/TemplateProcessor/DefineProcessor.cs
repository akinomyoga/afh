using Gen=System.Collections.Generic;
using Interop=System.Runtime.InteropServices;
using Rgx=System.Text.RegularExpressions;
using REP=afh.Text.RegexPatterns;

namespace afh.Design{
	internal interface IDefineProcessor{
		string Instantiate(string[] args);
	}
	internal abstract class DefineProcessorBase:IDefineProcessor{
		public abstract string Instantiate(string[] args);

		const string PARAM_HEAD="__AFH_DEFINE_PARAM__";
		private static Rgx::Regex rx_param=new Rgx::Regex(@"\b"+PARAM_HEAD+@"(?<index>\d+)\b",Rgx::RegexOptions.Multiline|Rgx::RegexOptions.Compiled);
		protected static string ReplaceParameters(string content,string[] args){
			return rx_param.Replace(content,delegate(Rgx::Match m) {
				string arg=args[int.Parse(m.Groups["index"].Value)];
				return ResolveQuotedArgument(arg);
			}).Replace("##","");
		}
		/// <summary>
		/// "#�Ȃ�Ƃ�����Ƃ�#" �̌`�̈������������܂��B
		/// </summary>
		/// <param name="arg">"#�Ȃ�Ƃ�����Ƃ�#" �̌`�ɂȂ��Ă���\���̂��������������w�肵�܂��B</param>
		/// <returns>
		/// �w�肵��������̓��e�� \"#�Ȃ�Ƃ�����Ƃ�#\" �̌`�ɂȂ��Ă����ꍇ�ɂ́A
		/// �u�Ȃ�Ƃ�����Ƃ��v�̕����̃G�X�P�[�v�V�[�P���X�����������������Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� arg ��f���ԕԂ��܂��B
		/// </returns>
		protected static string ResolveQuotedArgument(string arg){
			if(arg.Length>=4&&arg.StartsWith("\"#")&&arg.EndsWith("#\"")){
				return Rgx::Regex.Unescape(arg.Substring(2,arg.Length-4));
			}
			return arg;
		}
	}
	internal class DefineProcessor:DefineProcessorBase{
		/// <summary>
		/// �u����̕������ێ����܂��B
		/// </summary>
		string content;
		/// <summary>
		/// DefineProcessor �����������܂��B
		/// </summary>
		/// <param name="content">
		/// �u����̕�������w�肵�܂��B
		/// ������ _AFH_DEFINE_PARAM__���l �̌`�Ŗ��ߍ���ŉ������B
		/// </param>
		public DefineProcessor(string content){
			this.content=content;
		}

		public override string Instantiate(string[] args){
			return ReplaceParameters(this.content,args);
		}
	}
	//*
	/// <summary>
	/// __afh::�Ȃ�Ƃ� �Ŏg�p�o����}�N����`��񋟂��܂��B
	/// </summary>
	/// <remarks>
	/// //#define FOO __afh::bar(...)
	/// �̌`�Ŏg�p���܂��B
	/// </remarks>
	internal class AfhDefineProcessor:DefineProcessorBase{
		string name;
		string[] arg_templates;
		internal AfhDefineProcessor(string name,string[] args){
			this.name=name;
			this.arg_templates=args;
		}
		//============================================================
		//		����
		//============================================================
		public override string Instantiate(string[] args){
			// �����̕ϊ�
			string[] afh_args=new string[arg_templates.Length];
			for(int i=0;i<arg_templates.Length;i++)
				afh_args[i]=ReplaceParameters(arg_templates[i],args);

			// ���ۂ̏���
			return methods[this.name](afh_args);
		}
		/// <summary>
		/// �͈͊O�ǂݎ��ɑ΂��󕶎����Ԃ��z��ł��B
		/// </summary>
		private class StringArray{
			string[] array;
			public StringArray(string[] array){
				this.array=array;
			}
			public string this[int index]{
				get{
					if(index<0||this.array.Length<=index)
						return "";
					return this.array[index];
				}
			}
			public static implicit operator StringArray(string[] array){
				return new StringArray(array);
			}
		}

		//============================================================
		//		������
		//============================================================
		static string re_arg=@"\s*(?<arg>(?:"
			+@"[^\(\)\,\'\""]"
			+@"|"+REP.DOUBLEQUOTED
			+@"|"+REP.SINGLEQUOTED
			+@"|\("+REP.CreateMatchParen('(',')',true)+@"\)"
		+@")+)\s*";
		static string re_args="(?:"+re_arg+@"(?:\,"+re_arg+")*)?";
		static Rgx::Regex rx_afhdefine=new Rgx::Regex(
			@"^__afh\:\:(?<name>\w+)\s*\("+re_args+@"\)$",
			Rgx::RegexOptions.Singleline|Rgx::RegexOptions.Compiled
			);
		/// <summary>
		/// �w�肵�� define ��`�� __afh:: �^�ł��邩�ǂ����𔻒肵�A
		/// __afh:: �^�ł���Ɣ��f���ꂽ�ꍇ�ɂ́A���̃n���h�����쐬���ĕԂ��܂��B
		/// </summary>
		/// <param name="content"></param>
		/// <param name="proc"></param>
		/// <returns>__afh:: �^�ł���Ɣ��肳�ꂽ�ꍇ�� true ��Ԃ��܂��B</returns>
		public static bool TryCreateInstance(string content,out IDefineProcessor proc){
			do{
				Rgx::Match m=rx_afhdefine.Match(content);
				if(!m.Success)break;

				string name=m.Groups["name"].Value;
				if(!methods.ContainsKey(name))break;

				Rgx::CaptureCollection cc=m.Groups["arg"].Captures;
				string[] args=new string[cc.Count];
				for(int i=0;i<cc.Count;i++)
					args[i]=cc[i].Value.Trim();

				proc=new AfhDefineProcessor(name,args);
				return true;
#pragma warning disable 162
			}while(false);
#pragma warning restore 162

			// ���s�����ꍇ
			proc=null;
			return false;
		}
		//============================================================
		//		�֐���`
		//============================================================
		private static Gen::Dictionary<string,System.Converter<StringArray,string>> methods
			=new Gen::Dictionary<string,System.Converter<StringArray,string>>();
		static AfhDefineProcessor(){
			methods.Add("equal",delegate(StringArray args){
				return args[0]==args[1]?"true":"false";
			});
			methods.Add("iif",delegate(StringArray args){
				int c;
				bool eval=args[0]=="true"||int.TryParse(args[0],out c)&&c!=0;
				return eval?args[1]:args[2];
			});
		}
	}
	//*/
	// TODO: DefineImportProcessor
	internal class DefineImportProcessor{
		//========================================================
		//	dllimport
		//========================================================
		//	�}�N���u�������łȂ��A�����Œ�`�����֐��ɂ���ă}�N������������Ƃ��������\�ɂ���
		//--------------------------------------------------------
		public static void ProcessDllImport(){
		//	if(this.content.StartsWith("__afh::import(")&&this.content.EndsWith(")")){
		//		this.isimport=true;
		//	}
		
		}
	}

}