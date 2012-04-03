//#define USING_EXTRA_CAPTURE

using Gen=System.Collections.Generic;
using System.Text;
using Emit=System.Reflection.Emit;

namespace afh.RegularExpressions {
	public partial class RegexFactory3<T> {
		//============================================================
		//		OrNode
		//============================================================
#if __MWG_MINOR
				private ITester _Read(Status s)
					__minor(instance continuous)
				{
					instance BacktrackRange range=new BacktrackRange(s);
					for(instance int iNode=0;iNode<nodes.Length;iNode++){
						range.Restore(s);

						bool ret;
						if(nodes[iNode].Nondeterministic){
							s.Success=true;
							return nodes[iNode].GetTester();
							ret=s.Success;
						}else{
							ret=nodes[iNode].Read(s);
						}

						if(ret){
							s.Success=true;
							return null;
						}
					}
					s.Success=false;
					break return null;
				}
#else
		#region generated from __minor code
		private ITester _Read(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R0;
				case 2: goto R1;
			}
#pragma warning disable 164
			range=new BacktrackRange(s);
			iNode=0;
		L0: if(!(iNode<nodes.Length)) goto L2;
			range.Restore(s);

			bool ret;
			if(!(nodes[iNode].Nondeterministic)) goto L3;
			s.Success=true;
			continuous_state=1; return nodes[iNode].GetTester();
		R0: ;
			ret=s.Success;
			goto L4;
		L3:
			ret=nodes[iNode].Read(s);
		L4: ;

			if(!(ret)) goto L5;
			s.Success=true;
			continuous_state=2; return null;
		R1: ;
		L5: ;
		L1: iNode++; goto L0;
		L2: ;
			s.Success=false;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private BacktrackRange range;
		private int iNode;

		#endregion
#endif
		//============================================================
		//		SequenceNode
		//============================================================
#if __MWG_MINOR
				public ITester Read(Status s)
					__minor(continuous)
				{
					for(instance int iNode=0;iNode<nodes.Length;iNode++){
						bool ret;
						if(nodes[iNode].Nondeterministic){
							s.Success=true;
							return nodes[iNode].GetTester();
							ret=s.Success;
						}else{
							ret=nodes[iNode].Read(s);
						}

						if(!ret){
							s.Success=false;
							break return null;
						}
					}

					s.Success=true;
					break return null;
				}
#else
		#region generated from __minor code
		public ITester Read(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R2;
			}
#pragma warning disable 164
			iNode=0;
		L6: if(!(iNode<nodes.Length)) goto L8;
			bool ret;
			if(!(nodes[iNode].Nondeterministic)) goto L9;
			s.Success=true;
			continuous_state=1; return nodes[iNode].GetTester();
		R2: ;
			ret=s.Success;
			goto L10;
		L9:
			ret=nodes[iNode].Read(s);
		L10: ;

			if(!(!ret)) goto L11;
			s.Success=false;
			continuous_state=-1; return null;
		L11: ;
		L7: iNode++; goto L6;
		L8: ;

			s.Success=true;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private int iNode;

		#endregion
#endif
		//============================================================
		//		RepeatNode
		//============================================================
#if __MWG_MINOR
				public ITester Read_Nondeterm(Status s)
					__minor(instance continuous)
				{
					instance Gen::Stack<BacktrackRange> capt_ch=new Gen::Stack<BacktrackRange>();
					while(capt_ch.Count!=this.max){
						s.Success=true;
						capt_ch.Push(new BacktrackRange(s));
						return node.GetTester();

						if(!s.Success){
							capt_ch.Pop().Restore(s);
							break;
						}
					}
					
					while(capt_ch.Count>=this.min){
						s.Success=true;
						return null;

						if(capt_ch.Count==0)break;
						capt_ch.Pop().Restore(s);
					}

					s.Success=false;
					break return null;
				}
				public ITester Read_Determ(Status s)
					__minor(instance continuous)
				{
					instance Gen::Stack<BacktrackRange> capt_ch=new Gen::Stack<BacktrackRange>();
					while(capt_ch.Count!=this.max){
						capt_ch.Push(new BacktrackRange(s));
						if(!node.Read(s)){
							capt_ch.Pop().Restore(s);
							break;
						}
					}
					
					while(capt_ch.Count>=this.min){
						s.Success=true;
						return null;

						if(capt_ch.Count==0)break;
						capt_ch.Pop().Restore(s);
					}

					s.Success=false;
					break return null;
				}
#else
		#region generated from __minor code
		public ITester Read_Nondeterm(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R3;
				case 2: goto R4;
			}
#pragma warning disable 164
			capt_ch=new Gen::Stack<BacktrackRange>();
		L12: if(!(capt_ch.Count!=this.max)) goto L13;
			s.Success=true;
			capt_ch.Push(new BacktrackRange(s));
			continuous_state=1; return node.GetTester();
		R3: ;

			if(!(!s.Success)) goto L16;
			capt_ch.Pop().Restore(s);
			goto L13;
		L16: ;
			goto L12;
		L13: ;

		L14: if(!(capt_ch.Count>=this.min)) goto L15;
			s.Success=true;
			continuous_state=2; return null;
		R4: ;

			if(capt_ch.Count==0) goto L15;
			capt_ch.Pop().Restore(s);
			goto L14;
		L15: ;

			s.Success=false;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private Gen::Stack<BacktrackRange> capt_ch;

		public ITester Read_Determ(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R5;
			}
#pragma warning disable 164
			capt_ch=new Gen::Stack<BacktrackRange>();
		L17: if(!(capt_ch.Count!=this.max)) goto L18;
			capt_ch.Push(new BacktrackRange(s));
			if(!(!node.Read(s))) goto L21;
			capt_ch.Pop().Restore(s);
			goto L18;
		L21: ;
			goto L17;
		L18: ;

		L19: if(!(capt_ch.Count>=this.min)) goto L20;
			s.Success=true;
			continuous_state=1; return null;
		R5: ;

			if(capt_ch.Count==0) goto L20;
			capt_ch.Pop().Restore(s);
			goto L19;
		L20: ;

			s.Success=false;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private Gen::Stack<BacktrackRange> capt_ch;

		#endregion
#endif
		//============================================================
		//		LazyRepeatNode
		//============================================================
#if __MWG_MINOR
				public ITester Read_Nondet(Status s)
					__minor(instance continuous)
				{
					for(instance int imatch=0;;imatch++){
						if(imatch>=this.min){
							s.Success=true;
							return null;
						}
		
						if(imatch==this.max)break;

						s.Success=true;
						return node.GetTester();
						if(!s.Success)break;
					}
					s.Success=false;
					break return null;
				}
				public ITester Read_Determ(Status s)
					__minor(instance continuous)
				{
					for(instance int imatch=0;;imatch++){
						if(imatch>=this.min){
							s.Success=true;
							return null;
						}
		
						if(imatch==this.max)break;

						if(!node.Read(s))break;
					}
					s.Success=false;
					break return null;
				}
#else
		#region generated from __minor code
		public ITester Read_Nondet(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R6;
				case 2: goto R7;
			}
#pragma warning disable 164
			imatch=0;
		L22:
			if(!(imatch>=this.min)) goto L25;
			s.Success=true;
			continuous_state=1; return null;
		R6: ;
		L25: ;

			if(imatch==this.max) goto L24;

			s.Success=true;
			continuous_state=2; return node.GetTester();
		R7: ;
			if(!s.Success) goto L24;
		L23: imatch++; goto L22;
		L24: ;
			s.Success=false;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private int imatch;

		public ITester Read_Determ(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R8;
			}
#pragma warning disable 164
			imatch=0;
		L26:
			if(!(imatch>=this.min)) goto L29;
			s.Success=true;
			continuous_state=1; return null;
		R8: ;
		L29: ;

			if(imatch==this.max) goto L28;

			if(!node.Read(s)) goto L28;
		L27: imatch++; goto L26;
		L28: ;
			s.Success=false;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private int imatch;

		#endregion
#endif
		//============================================================
		//		StickyRepeatNode
		//============================================================
#if __MWG_MINOR
				public ITester Read(Status s)
					__minor(instance continuous)
				{
					for(instance int imatch=0;imatch!=this.max;imatch++){
						s.Success=true;
						return node.GetTester();
						if(!s.Success)break;
					}

					s.Success=imatch>=this.min;
					break return null;
				}
#else
		#region generated from __minor code
		public ITester Read(Status s) {
			switch(continuous_state) {
				default: throw new System.Exception("この関数の実行は終了しています。");
				case 0: break;
				case 1: goto R9;
			}
#pragma warning disable 164
			imatch=0;
		L30: if(!(imatch!=this.max)) goto L32;
			s.Success=true;
			continuous_state=1; return node.GetTester();
		R9: ;
			if(!s.Success) goto L32;
		L31: imatch++; goto L30;
		L32: ;

			s.Success=imatch>=index0;
			continuous_state=-1; return null;
#pragma warning restore 164
		}
		private int continuous_state=0;
		private int imatch;

		#endregion
#endif
	}
}
