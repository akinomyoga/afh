using Gen=System.Collections.Generic;
using BaseRegexFactory=afh.RegularExpressions.RegexFactory3<char>;
using Emit=System.Reflection.Emit;
using Ref=System.Reflection;

namespace afh.RegularExpressions{
	using INode=afh.RegularExpressions.RegexFactory3<char>.INode;
	using ITester=afh.RegularExpressions.RegexFactory3<char>.ITester;
	using ElemNodeBase=afh.RegularExpressions.RegexFactory3<char>.ElemNodeBase;

	/// <summary>
	/// �����N���X�̔���Ɏg�p����C���X�^���X�B���Ǘ����܂��B
	/// </summary>
	internal static partial class CharClasses{
		internal const string CONTROL_LETTERS="()[]<>{}|+-*?!\\\'^$.:";
		internal const string COMMAND_LETTERS_SPACE="tvnrfabe";
		internal const string SPACE_LETTERS="\t\v\n\r\f\a\b\x1b";

		/// <summary>
		/// \p{} �R�}���h�ɑΉ�����m�[�h��ێ����܂��B
		/// </summary>
		private static Gen::Dictionary<string,CharClassInfo> infos
			=new Gen::Dictionary<string,CharClassInfo>();

		public static CharClassInfo GetInfo(string name){
			if(name.StartsWith("Is"))name=name.ToLower();
			name=name.Replace("-","").ToLower();

			CharClassInfo info;
			if(infos.TryGetValue(name,out info)){
				return info;
			}else return null;
		}
		private static string CharToSource(char c){
			if(CONTROL_LETTERS.IndexOf(c)>=0)return @"\"+c;

			int index;
			if((index=SPACE_LETTERS.IndexOf(c))>=0)
				return @"\"+COMMAND_LETTERS_SPACE[index];

			return c.ToString();
		}
		//========================================================
		//		������
		//========================================================
		static CharClasses(){
			foreach(afh.Text.GeneralCategory cat in System.Enum.GetValues(typeof(afh.Text.GeneralCategory))){
				RegisterClass(new CharClassInfo(cat.ToString(),cat));
			}
			
			foreach(afh.Text.UnicodeBlock blk in System.Enum.GetValues(typeof(afh.Text.UnicodeBlock))){
				RegisterClass(new CharClassInfo(blk.ToString(),blk));
			}
			RegisterClass(new CharClassInfo("IsGreek",afh.Text.UnicodeBlock.GreekAndCoptic));
			RegisterClass(new CharClassInfo("IsCombiningMarksForSymbols",afh.Text.UnicodeBlock.CombiningDiacriticalMarksForSymbols));

			foreach(afh.Text.CLangCType ctype in System.Enum.GetValues(typeof(afh.Text.CLangCType))){
				string name=ctype.ToString();
				if(name.StartsWith("_"))continue;
				RegisterClass(new CharClassInfo(name,ctype));
			}
		}
		private static void RegisterClass(CharClassInfo info){
			infos[info.name.ToLower()]=info;
		}
		//============================================================
		//		CharClassInfo
		//============================================================
		public enum CharClassType{
			Custom,
			UnicodeBlock,
			UnicodeGenralCategory,
			CLanguageCType,
		}
		public class CharClassInfo{
			public readonly string name;
			public readonly CharClassType type;
			public readonly BaseRegexFactory.INode node_pos;
			public readonly BaseRegexFactory.INode node_neg;
			public readonly int value;

			public CharClassInfo(string name,CharClassType type){
				this.name=name;
				this.type=type;
			}
			public CharClassInfo(string name,afh.Text.GeneralCategory cat){
				this.name=name;
				this.type=CharClassType.UnicodeGenralCategory;
				this.node_pos=new GeneralCategoryNode(name,cat,true);
				this.node_neg=new GeneralCategoryNode(name,cat,false);
				this.value=(byte)cat;
			}
			public CharClassInfo(string name,afh.Text.UnicodeBlock block){
				this.name=name;
				this.type=CharClassType.UnicodeBlock;
				this.node_pos=new UnicodeBlockNode(name,block,true);
				this.node_neg=new UnicodeBlockNode(name,block,false);
				this.value=(byte)block;
			}
			public CharClassInfo(string name,afh.Text.CLangCType type){
				this.name=name;
				this.type=CharClassType.CLanguageCType;
				this.node_pos=new CLangCTypeNode(name,type,true);
				this.node_neg=new CLangCTypeNode(name,type,false);
				this.value=(byte)type;
			}
		}
		//============================================================
		//		INode
		//============================================================
		private class GeneralCategoryNode:ElemNodeBase{
			readonly afh.Text.GeneralCategory cat;
			readonly bool positive;
			readonly string name;
			public GeneralCategoryNode(string name,afh.Text.GeneralCategory cat,bool positive){
				this.name=@"\"+(positive?'p':'P')+"{"+name+"}";
				this.cat=cat;
				this.positive=positive;
#if Ver1_0a2
				this.inst=new Tester(cat,positive);
#endif
			}

			public override bool Judge(char value){
				return positive==afh.Text.CharUtils.Is(value,this.cat);
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}
#endif

			public override string ToString(){
				return this.name;
			}
#if Ver1_0a2
			private class Tester:TesterBase {
				afh.Text.GeneralCategory cat;
				bool positive;
				public Tester(afh.Text.GeneralCategory cat,bool positive){
					this.cat=cat;
					this.positive=positive;
				}
				public override bool Judge(char value){
					return positive==afh.Text.CharUtils.Is(value,this.cat);
				}
			}
#endif
		}
		private class UnicodeBlockNode:ElemNodeBase{
			readonly afh.Text.UnicodeBlock block;
			readonly string name;
			readonly bool positive;
			public UnicodeBlockNode(string name,afh.Text.UnicodeBlock block,bool positive){
				this.name=@"\"+(positive?'p':'P')+"{"+name+"}";
				this.block=block;
				this.positive=positive;
#if Ver1_0a2
				this.inst=new Tester(block,positive);
#endif
			}
			public override string ToString(){
				return this.name;
			}
			public override bool Judge(char value) {
				return positive==afh.Text.CharUtils.Is(value,this.block);
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}
#endif

#if Ver1_0a2
			private class Tester:TesterBase {
				afh.Text.UnicodeBlock block;
				bool positive;
				public Tester(afh.Text.UnicodeBlock block,bool positive){
					this.block=block;
					this.positive=positive;
				}
				public override bool Judge(char value){
					return positive==afh.Text.CharUtils.Is(value,this.block);
				}
			}
#endif
		
		}
		private class CLangCTypeNode:ElemNodeBase{
			readonly afh.Text.CLangCType ctype;
			readonly string name;
			readonly bool positive;
			public CLangCTypeNode(string name,afh.Text.CLangCType ctype,bool positive){
				this.name=@"\"+(positive?'p':'P')+"{"+name+"}";
				this.ctype=ctype;
				this.positive=positive;
#if Ver1_0a2
				this.inst=new Tester(ctype,positive);
#endif
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}
#endif

			public override string ToString(){
				return this.name;
			}
			public override bool Judge(char value) {
				return positive==afh.Text.CharUtils.Is(value,this.ctype);
			}
#if Ver1_0a2
			private class Tester:TesterBase {
				afh.Text.CLangCType ctype;
				bool positive;
				public Tester(afh.Text.CLangCType ctype,bool positive) {
					this.ctype=ctype;
					this.positive=positive;
				}
				public override bool Judge(char value){
					return positive==afh.Text.CharUtils.Is(value,this.ctype);
				}
			}
#endif
		}
		//============================================================
		//		�����N���X����
		//============================================================
		internal delegate void DReportError(string msg,string expression,int index);

		internal partial class CharClassNodeGenerator{
			int i;
			readonly DReportError reporter;
			readonly string expression;
			public CharClassNodeGenerator(DReportError reporter,string expression){
				this.reporter=reporter;
				this.expression=expression;
			}
			public INode CreateNode(){
				return new CharClassNode(this.ReadClass());
			}
			private void ReportError(string msg){
				this.reporter(msg,expression,i);
			}

			private static IClassHandler CreateNamedClassHandler(bool positive,string name){
				CharClassInfo info=CharClasses.GetInfo(name);
				switch(info.type){
					case CharClassType.UnicodeGenralCategory:
						return new GeneralCategoryHandler(info.name,(afh.Text.GeneralCategory)info.value,positive);
					case CharClassType.UnicodeBlock:
						return new UnicodeBlockHandler(info.name,(afh.Text.UnicodeBlock)info.value,positive);
					case CharClassType.CLanguageCType:
						return new CLangCTypeHandler(info.name,(afh.Text.CLangCType)info.value,positive);
					default:
						__debug__.RegexParserAssert(false,"���̎�ނ̖��O�t���N���X�ɂ͑Ή����Ă��܂���B");
						return null;
				}
			}
		}

		private sealed class CharClassNode:ElemNodeBase{
			readonly IClassHandler handler;
			readonly string name;
			public CharClassNode(IClassHandler handler){
#if Ver1_0a2
				this.inst=new Tester(handler);
#else
				this.handler=handler;
				this.name="["+handler.Source+"]";
#endif
			}

			public override string ToString(){
#if Ver1_0a2
				return this.inst.ToString();
#else
				return this.name;
#endif
			}
			public override bool Judge(char value){
				return handler.Judge(value);
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}
			private class Tester:TesterBase{
				IClassHandler handler;
				string name;

				public Tester(IClassHandler handler){
					this.handler=handler;
					this.name="["+this.handler.Source+"]";
				}
				public override bool Judge(char value){
					return handler.Judge(value);
				}
				public override string ToString() {
					return this.name;
				}
			}
#endif
		}

		#region IClassHandler
		/// <summary>
		/// �����N���X�̍\���q��\�����܂��B
		/// </summary>
		private interface IClassHandler{
			/// <summary>
			/// �w�肵�����������̃C���X�^���X�ɍ��v���邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="c">����Ώۂ̕������w�肵�܂��B</param>
			/// <returns>���̃C���X�^���X�ɍ��v����Ɣ��肳��鎞�� true ��Ԃ��܂��B
			/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			bool Judge(char c);
			/// <summary>
			/// ���̕����N���X�\���q�̃C���X�^���X���쐬���܂��B
			/// </summary>
			string Source{get;}
			/// <summary>
			/// ���肪�^�̎��� label �ɃW�����v����l�ȃR�[�h�𐶐����܂��B
			/// </summary>
			/// <param name="gh">�R�[�h�̐����Ώۂ��w�肵�܂��B</param>
			/// <param name="label">���肪�S�������ꍇ�̃W�����v����w�肵�܂��B</param>
			/// <returns>�R�[�h�𐶐������ꍇ�� true ��Ԃ��܂��B
			/// �W���̃R�[�h���g�p����ꍇ�ɂ́A�R�[�h�𐶐������� false ��Ԃ��܂��B</returns>
			bool Emit(afh.Reflection.ILGeneratorHelper gh,Emit::Label label);
		}
		/// <summary>
		/// �����N���X�̒P�ꔻ����s���܂��B
		/// </summary>
		private sealed class AtomHandler:IClassHandler{
			char c;
			public AtomHandler(char c){
				this.c=c;
			}
			/// <summary>
			/// �w�肵�����������̃C���X�^���X�ɍ��v���邩�ۂ��𔻒肵�܂��B
			/// </summary>
			/// <param name="c">����Ώۂ̕������w�肵�܂��B</param>
			/// <returns>���̃C���X�^���X�ɍ��v����Ɣ��肳��鎞�� true ��Ԃ��܂��B
			/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
			public bool Judge(char c){
				return c==this.c;
			}
			/// <summary>
			/// ���̕����N���X�\���q�̃C���X�^���X���쐬���܂��B
			/// </summary>
			public string Source{
				get{return CharToSource(this.c);}
			}

			public char Character{
				get{return c;}
			}

			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				gh.EmitLdarg(1);
				gh.EmitLdc(this.c);
				gh.EmitBeq(label);
				return true;
			}
		}
		/// <summary>
		/// �����N���X�̍\���q�ł��B����������͈̔͂ɑ����Ă��邩�ۂ��𔻒肵�܂��B
		/// </summary>
		private sealed class RangeHandler:IClassHandler{
			char c1;
			char c2;
			public RangeHandler(char start,char end){
				this.c1=start;
				this.c2=end;
			}
			public bool Judge(char c){
				return this.c1<=c&&c<=this.c2;
			}
			public string Source{
				get{return CharToSource(this.c1)+"-"+CharToSource(this.c2);}
			}
			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				Emit::Label l=gh.CreateLabel();
				gh.EmitLdarg(1);
				gh.EmitLdc(this.c1);
				gh.EmitBltUnS(l);
				gh.EmitLdarg(1);
				gh.EmitLdc(this.c2);
				gh.EmitBleUn(label);
				gh.MarkLabel(l);
				return true;
			}

			public char Start{
				get{return this.c1;}
			}
			public char End{
				get{return this.c2;}
			}
		}
		private class ClassHandlerGenerator{
			bool mode;
			Gen::List<IClassHandler> list=new Gen::List<IClassHandler>();
			Gen::List<bool> issub_list=new Gen::List<bool>();
			public ClassHandlerGenerator(bool mode){
				this.mode=mode;
			}

			public void Add(IClassHandler handler,bool isSubtract){
				if(handler==null)return;
				list.Add(handler);
				issub_list.Add(isSubtract);
			}
			public IClassHandler Create(){
				//return new ClassHandler(this.mode,this.list.ToArray(),this.issub_list.ToArray());
				return new CompiledClassHandler(this.mode,this.list.ToArray(),this.issub_list.ToArray());
			}
		}
		private sealed class CompiledClassHandler:IClassHandler{
			readonly System.Predicate<char> handler;
			readonly IClassHandler[] child;
			readonly string name;
			public CompiledClassHandler(bool positive,IClassHandler[] handlers,bool[] isSubtractList){
				__debug__.RegexParserAssert(handlers.Length==isSubtractList.Length);
				this.handler=this.CreateHandler(positive,handlers,isSubtractList,out this.child);
				this.name=CreateName(positive,handlers,isSubtractList);
			}
			public bool Judge(char c){
				return this.handler(c);
				// �œK��
				// 1. issub ���ɓZ�߂�
				// 2.
#if SAMPLE
			// [123] �̏ꍇ
				if(proc1(c))goto next;
				if(proc2(c))goto next;
				if(proc3(c))goto next;
				return false;
			next:
				return true;

			// [123-4-5] �̏ꍇ
				if(proc1(c))goto next;
				if(proc2(c))goto next;
				if(proc3(c))goto next;
				goto next2;
			next:
				if(proc4(c))goto next2;
				if(proc5(c))goto next2;
				return true;
			next2:
				return false;

			// [123-4-567] �̏ꍇ
				if(proc1(c))goto next;
				if(proc2(c))goto next;
				if(proc3(c))goto next;
				goto next2;
			next:
				if(proc4(c))goto next2;
				if(proc5(c))goto next2;
				goto next3;
			next2:
				if(proc6(c))goto next3;
				if(proc7(c))goto next3;
				return false;
			next3:
				return true;
#endif
				// 3. �X�Ɋe procN() �Ăяo����W�J�o����ꍇ�ɂ͓W�J����
			}
			public string Source{
				get{return this.name;}
			}
			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label) {
				return false;
			}
			private System.Predicate<char> CreateHandler(
				bool positive,IClassHandler[] handlers,bool[] issub,
				out IClassHandler[] child
			){
				int[] borders;
				{
					Gen::List<int> borders_l=new System.Collections.Generic.List<int>();
					bool current=true;
					for(int i=0;i<issub.Length;i++){
						if(issub[i]==current)continue;
						borders_l.Add(i); // *
						current=issub[i];
					}
					// issub: [F T T T F F T T F T F F F T T T ]
					// brdrs:    *     *   *   * * *     *     @

					borders_l.Add(issub.Length); // @
					borders=borders_l.ToArray();
				}

				afh.Reflection.DynamicMethodCreater<CompiledClassHandler,System.Predicate<char>> gh
					=new afh.Reflection.DynamicMethodCreater<CompiledClassHandler,System.Predicate<char>>("<generated>",false);

				Gen::List<IClassHandler> child_l=new Gen::List<IClassHandler>();

				Emit::Label label=gh.CreateLabel();
				for(int k=0,kM=borders.Length-1;k<kM;k++){
					for(int i=borders[k];i<borders[k+1];i++){
						//-- ���ꂼ��̃n���h���ɉ���������
						// ����z�� �� goto label;
						// ����A�� �� ���̘Ԏ��֗����
						if(!handlers[i].Emit(gh,label)){
							// child ���X�g����Ăяo���ꍇ
							int iChild=child_l.Count;
							child_l.Add(handlers[i]);

							// (s1)= this.child[iChild]
							gh.EmitLdarg(0);
							gh.EmitLdfld(typeof(CompiledClassHandler),"child",false,true);
							gh.EmitLdc(iChild);
							gh.EmitLdelem(typeof(IClassHandler));

							// if((s1).Judge(c))goto label;
							gh.EmitLdarg(1);
							gh.EmitCall(handlers[i].GetType(),false,false,"Judge",typeof(char));
							gh.EmitBrtrue(label);
						}
					}

					// �����I��鎞
					if(k+1==kM){
						break;
					}else{
						Emit::Label newlabel=gh.CreateLabel();
						gh.EmitBr(newlabel);
						gh.MarkLabel(label);
						label=newlabel;

						positive=!positive;
					}
				}
				gh.EmitLdc(!positive);
				gh.EmitRet();
				gh.MarkLabel(label);
				gh.EmitLdc(positive);
				gh.EmitRet();

				child=child_l.ToArray();

				return gh.Instantiate(this);
			}
			private static string CreateName(bool positive,IClassHandler[] handlers,bool[] issub){
				System.Text.StringBuilder name
					=new System.Text.StringBuilder(positive?"":"^");
				for(int i=0;i<handlers.Length;i++){
					if(issub[i])name.Append("-[");
					name.Append(handlers[i].Source);
					if(issub[i])name.Append(']');
				}
				return name.ToString();
			}
		}
		private sealed class ClassHandler:IClassHandler{
			readonly bool positive;
			readonly IClassHandler[] handlers;
			readonly bool[] issub;
			readonly string name;
			public ClassHandler(bool positive,IClassHandler[] handlers,bool[] isSubtractList){
				this.positive=positive;
				this.handlers=handlers;
				this.issub=isSubtractList;
				this.name=CreateName();
			}
			public bool Judge(char c){
				bool ret=false;
				for(int i=0;i<handlers.Length;i++){
					if(ret!=issub[i])continue;
					if(handlers[i].Judge(c))
						ret=!ret;
				}
				return positive==ret;
			}
			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				return false;
			}
			private bool Sample(char c){
				return positive;
			}
			public string Source{
				get{return this.name;}
			}

			private string CreateName(){
				System.Text.StringBuilder name
					=new System.Text.StringBuilder(positive?"":"^");
				for(int i=0;i<handlers.Length;i++){
					if(issub[i])name.Append("-[");
					name.Append(handlers[i].Source);
					if(issub[i])name.Append(']');
				}
				return name.ToString();
			}
		}
		private sealed class GeneralCategoryHandler:IClassHandler{
			bool positive;
			string name;
			afh.Text.GeneralCategory cat;
			public GeneralCategoryHandler(string name,afh.Text.GeneralCategory cat,bool positive){
				this.name=(positive?@"\p{":@"\P{")+name+"}";
				this.cat=cat;
				this.positive=positive;
			}
			public bool Judge(char c){
				return positive==afh.Text.CharUtils.Is(c,this.cat);
			}
			public string Source{
				get{return this.name;}
			}

			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				gh.EmitLdarg(1);
				gh.EmitLdc((byte)this.cat);
				gh.EmitCall(typeof(afh.Text.CharUtils),false,true,"Is",typeof(char),typeof(afh.Text.GeneralCategory));
				if(positive){
					gh.EmitBrtrue(label);
				}else{
					gh.EmitBrfalse(label);
				}
				return true;
			}
		}
		private sealed class UnicodeBlockHandler:IClassHandler{
			bool positive;
			string name;
			afh.Text.UnicodeBlock cat;
			public UnicodeBlockHandler(string name,afh.Text.UnicodeBlock cat,bool positive){
				this.name=(positive?@"\p{":@"\P{")+name+"}";
				this.cat=cat;
				this.positive=positive;
			}
			public bool Judge(char c){
				return positive==afh.Text.CharUtils.Is(c,this.cat);
			}
			public string Source {
				get {return this.name;}
			}

			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				gh.EmitLdarg(1);
				gh.EmitLdc((byte)this.cat);
				gh.EmitCall(typeof(afh.Text.CharUtils),false,true,"Is",typeof(char),typeof(afh.Text.UnicodeBlock));
				if(positive){
					gh.EmitBrtrue(label);
				}else{
					gh.EmitBrfalse(label);
				}
				return true;
			}
		}
		private sealed class CLangCTypeHandler:IClassHandler{
			bool positive;
			string name;
			afh.Text.CLangCType cat;
			public CLangCTypeHandler(string name,afh.Text.CLangCType cat,bool positive){
				this.name=(positive?@"\p{":@"\P{")+name+"}";
				this.cat=cat;
				this.positive=positive;
			}
			public bool Judge(char c){
				return positive==afh.Text.CharUtils.Is(c,this.cat);
			}
			public string Source {
				get {return this.name;}
			}

			public bool Emit(afh.Reflection.ILGeneratorHelper gh,System.Reflection.Emit.Label label){
				gh.EmitLdarg(1);
				gh.EmitLdc((byte)this.cat);
				gh.EmitCall(typeof(afh.Text.CharUtils),false,true,"Is",typeof(char),typeof(afh.Text.CLangCType));
				if(positive){
					gh.EmitBrtrue(label);
				}else{
					gh.EmitBrfalse(label);
				}
				return true;
			}
		}
		#endregion
	}
}
