//#define USING_EXTRA_CAPTURE

using Gen=System.Collections.Generic;
using System.Text;
using Emit=System.Reflection.Emit;
using afh.Collections.Utils;

namespace afh.RegularExpressions{
	public partial class RegexFactory3<T>{

		#region node:Or
		private class OrNode:INode{
			public OrNode(INode[] nodes){
				if(nodes.Length<2)
					throw new System.ArgumentException("選言 Node は二つ以上の子ノードを含まなければ為りません。","nodes");

				this.nodes=nodes;
			}

			protected readonly INode[] nodes;
			public Gen::IEnumerable<INode> EnumChildren{
				get{return this.nodes;}
			}
			public bool Nondeterministic{
				get{return true;}
			}
			public ITester GetTester(){
				return new Tester(this,this.nodes);
			}
			bool INode.Read(Status s){throw new System.NotImplementedException();}
			//=================================================================
			//		文字列表現
			//=================================================================
			public override string ToString(){
				System.Text.StringBuilder build=new StringBuilder();
				build.Append(nodes[0].ToString());
				for(int i=1;i<nodes.Length;i++){
					build.Append('|');
					build.Append(nodes[i].ToString());
				}
				return build.ToString();
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Weak;}
			}
			//=================================================================
			//		Tester
			//=================================================================
			private class Tester:OrNode,ITester{
				private readonly OrNode parent;

				public Tester(OrNode parent,INode[] nodes):base(nodes){
					this.parent=parent;
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.nodes);

					ret.continuous_state=this.continuous_state;
					ret.iNode=this.iNode;
					ret.range=this.range;

					return ret;
				}

				public bool Indefinite{
					get{return iNode+1<nodes.Length;}
				}
				//====================================================
				//		Read 関数
				//====================================================
				public ITester Read(Status s){
					//TODO: もう match する物がないのに Indefinite を返す場合がある
					return this._Read(s);
				}
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
			}

		}
		#endregion

		#region node:Sequence
		private class SequenceNode:INode{
			protected readonly INode[] nodes;
			private readonly bool nondet;

			public SequenceNode(INode[] nodes){
				if(nodes.Length<2)
					throw new System.ArgumentException("連続 Node には二つ以上の子 node を指定してください。","nodes");
				this.nodes=nodes;

				this.nondet=CalcNondeterministic();
			}
			public Gen::IEnumerable<INode> EnumChildren{
				get{return this.nodes;}
			}

			//=================================================================
			//		非決定性
			//=================================================================
			private bool CalcNondeterministic(){
				foreach(INode node in this.nodes)
					if(node.Nondeterministic)return true;
				return false;
			}
			public bool Nondeterministic{
				get{return this.nondet;}
			}
			public ITester GetTester(){
				return new Tester(this,this.nodes);
			}
			bool INode.Read(Status s){
				__debug__.RegexTestAssert(!Nondeterministic);
				for(int i=0;i<this.nodes.Length;i++){
					if(!this.nodes[i].Read(s))return false;
				}
				return true;
			}
			//=================================================================
			//		文字列表現
			//=================================================================
			public override string ToString(){
				System.Text.StringBuilder build=new StringBuilder();
				foreach(INode node in nodes){
					bool paren=node.Associativity==NodeAssociativity.Weak;
					if(paren)build.Append("(?:");
					build.Append(node);
					if(paren)build.Append(")");
				}
				return build.ToString();
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Middle;}
			}
			//=================================================================
			//		Tester
			//=================================================================
			private class Tester:SequenceNode,ITester{
				private readonly SequenceNode parent;

				public Tester(SequenceNode parent,INode[] nodes):base(nodes){
					this.parent=parent;
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.nodes);

					ret.continuous_state=this.continuous_state;
					ret.iNode=this.iNode;
					return ret;
				}

				public bool Indefinite{
					get{return false;}
				}

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
			}
			private class CompiledTester:SequenceNode,ITester{
				private readonly SequenceNode parent;

				public CompiledTester(SequenceNode parent,INode[] nodes):base(nodes){
					this.parent=parent;
				}

				public ITester Clone(){
					CompiledTester ret=new CompiledTester(this.parent,this.nodes);

					ret.iNode=this.iNode;
					return ret;
				}

				public bool Indefinite{
					get{return false;}
				}

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
				/*
				delegate ITester DGetTester();
				public System.Converter<Status,ITester> CreateRead(Status s){
					afh.Reflection.DynamicMethodCreater<CompiledTester,System.Converter<Status,ITester>> gen
						=new afh.Reflection.DynamicMethodCreater<CompiledTester,System.Converter<Status,ITester>>("Read/"+this.ToString()+"/",false);
					for(int i=0;i<nodes.Length;i++){
						if(!nodes[i].Nondeterministic){
							// this.nodes[i].GetTester().Read(s);
							gen.EmitLdarg(0);
							gen.EmitLdfld(typeof(SequenceNode),"nodes",false,false);
							gen.EmitLdc(i);
							gen.EmitLdelem(typeof(INode));
							gen.EmitCall(((DGetTester)nodes[i].GetTester).Method);
							gen.EmitLdarg(1);
							gen.EmitCallvirt(typeof(ITester),false,false,"Read",typeof(Status));
							gen.EmitPop();

							// if(!s.Success)
							gen.EmitLdarg(1);
							gen.EmitCall(typeof(Status),false,false,"get_Success");
							Emit::Label l=gen.CreateLabel();
							gen.EmitBrtrueS(l);
							{
								// return null;
								gen.EmitLdc(0);
								gen.EmitRet();
							}
							gen.MarkLabel(l);
						}else{
							// TODO: 戻って来る為のラベルの宣言
							// TODO: return this.nodes[i].GetTester();
						}
						// TODO: もっと場合分け: bool INode::Read / ElemEqualsNode を ElemsEqualsNode に纏める。
					}

					return gen.Instantiate(this);
				}
				//*/
			}
		}
		#endregion

		#region node:Repeat
		private class RepeatNode:INode{
			protected INode node;
			protected int min;
			protected int max;

			public RepeatNode(INode node,int min,int max){
				if(min<0)
					throw new System.ArgumentOutOfRangeException("min","min は 0 以上の値でなければ為りません。");
				if(max==0||this.max>=0&&this.min>this.max)
					throw new System.ArgumentOutOfRangeException("max","max は 「0 より大きく min 以上」か負 (負は無限の意) の値でなければ為りません。");

				this.node=node;
				this.min=min;
				this.max=max;
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}
			public override string ToString(){
				string str=node.ToString();
				if((int)node.Associativity<(int)this.Associativity){
					str="(?:"+str+")";
				}
				if(max<0){
					if(min==0)return str+"*";
					if(min==1)return str+"+";
					return str+"{"+min.ToString()+",}";
				}else{
					if(min==max)return str+"{"+min.ToString()+"}";
					if(max==1)return str+"?";
					return str+"{"+min.ToString()+","+max.ToString()+"}";
				}
			}

			public NodeAssociativity Associativity{
				get{
					if(min==0&&max==1||min==1&&max<0)return NodeAssociativity.Strong;
					return NodeAssociativity.Stronger;
				}
			}
			//========================================================
			//		非決定性
			//========================================================
			public bool Nondeterministic{
				get{return true;}
			}
			public ITester GetTester(){
				if(node.Nondeterministic){
					return new NTester(this,this.node,this.min,this.max);
				}else{
					return new DTester(this,this.node,this.min,this.max);
				}
			}
			bool INode.Read(Status s){throw new System.NotImplementedException();}
			//========================================================
			//		ITester
			//========================================================
			private class NTester:RepeatNode,ITester{
				private readonly RepeatNode parent;

				public NTester(RepeatNode parent,INode node,int min,int max):base(node,min,max){
					this.parent=parent;
				}

				public ITester Clone(){
					NTester ret=new NTester(this.parent,this.node,this.min,this.max);

					ret.continuous_state=this.continuous_state;
					ret.capt_ch=this.capt_ch.Clone();
					return ret;
				}

				public bool Indefinite{
					get{return capt_ch!=null&&capt_ch.Count>this.min;}
				}

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
#else
				#region generated from __minor code
				public ITester Read(Status s) {
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
				#endregion
#endif
			}
			private class DTester:RepeatNode,ITester{
				private readonly RepeatNode parent;

				public DTester(RepeatNode parent,INode node,int min,int max):base(node,min,max){
					this.parent=parent;
				}

				public ITester Clone(){
					DTester ret=new DTester(this.parent,this.node,this.min,this.max);

					ret.continuous_state=this.continuous_state;
					ret.capt_ch=this.capt_ch.Clone();
					return ret;
				}

				public bool Indefinite{
					get{return capt_ch!=null&&capt_ch.Count>this.min;}
				}

#if __MWG_MINOR
				public ITester Read(Status s)
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
				public ITester Read(Status s) {
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
			}
		}
		/// <summary>
		/// バックトラックの為の情報を保持します。
		/// </summary>
		private struct BacktrackRange{
			public int index;
			public int init_listcount;
			public BacktrackRange(Status s){
				this.index=s.Index;
				this.init_listcount=s.Captures.Count;
			}
			public void Restore(Status s){
				s.Index=index;
				s.ClearCaptures(this.init_listcount);
			}
		}
		#endregion

		#region node:LazyRepeat
		private class LazyRepeatNode:INode{
			protected readonly INode node;
			protected readonly int min;
			protected readonly int max;

			public LazyRepeatNode(INode node,int min,int max){
				if(min<0)
					throw new System.ArgumentOutOfRangeException("min","min は 0 以上の値でなければ為りません。");
				if(max==0||this.max>=0&&this.min>this.max)
					throw new System.ArgumentOutOfRangeException("max","max は 「0 より大きく min 以上」か負 (負は無限の意) の値でなければ為りません。");

				this.node=node;
				this.min=min;
				this.max=max;
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}

			public override string ToString(){
				string str=node.ToString();
				if((int)node.Associativity<(int)this.Associativity){
					str="(?:"+str+")";
				}
				if(max<0){
					if(min==0)return str+"*?";
					if(min==1)return str+"+?";
					return str+"{"+min.ToString()+",?}";
				}else{
					if(min==max)return str+"{"+min.ToString()+"?}";
					if(max==1)return str+"??";
					return str+"{"+min.ToString()+","+max.ToString()+"?}";
				}
			}
			public NodeAssociativity Associativity{
				get{
					if(min==0&&max==1||min==1&&max<0)return NodeAssociativity.Strong;
					return NodeAssociativity.Stronger;
				}
			}

			public bool Nondeterministic{
				get{return true;}
			}
			public ITester GetTester(){
				return new Tester(this,this.node,this.min,this.max);
			}
			bool INode.Read(Status s){throw new System.NotImplementedException();}

			private class Tester:LazyRepeatNode,ITester{
				private readonly LazyRepeatNode parent;

				public Tester(LazyRepeatNode parent,INode node,int min,int max):base(node,min,max){
					this.parent=parent;
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.node,this.min,this.max);
					ret.continuous_state=this.continuous_state;
					ret.imatch=this.imatch;
					return ret;
				}

				public bool Indefinite{
					get{return this.max<0||imatch<this.max;}
				}

				public ITester Read(Status s){
					if(this.node.Nondeterministic)
						return Read_Nondet(s);
					else
						return Read_Determ(s);
				}
#if __MWG_MINOR
				public ITester Read_Nondet(Status s)
					__minor(instance continuous)
				{
					for(instance int imatch=0;true;imatch++){
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
					for(instance int imatch=0;true;imatch++){
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
			}
		}
		#endregion

		#region node:StickyRepeat
		private class StickyRepeatNode:INode{
			protected INode node;
			protected int min;
			protected int max;

			public StickyRepeatNode(INode node,int min,int max){
				if(min<0)
					throw new System.ArgumentOutOfRangeException("min","min は 0 以上の値でなければ為りません。");
				if(max==0||this.max>=0&&this.min>this.max)
					throw new System.ArgumentOutOfRangeException("max","max は 「0 より大きく min 以上」か負 (負は無限の意) の値でなければ為りません。");

				this.node=node;
				this.min=min;
				this.max=max;
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}

			public override string ToString(){
				string str=node.ToString();
				if((int)node.Associativity<(int)this.Associativity){
					str="(?:"+str+")";
				}
				if(max<0){
					if(min==0)return str+"*+";
					if(min==1)return str+"++";
					return str+"{"+min.ToString()+",+}";
				}else{
					if(min==max)return str+"{"+min.ToString()+"+}";
					if(max==1)return str+"?+";
					return str+"{"+min.ToString()+","+max.ToString()+"+}";
				}
			}
			public bool Nondeterministic{
				get{return this.node.Nondeterministic;}
			}
			public ITester GetTester(){
				return new Tester(this,this.node,this.min,this.max);
			}
			bool INode.Read(Status s){
				__debug__.RegexTestAssert(!Nondeterministic);
				int i=0;
				while(i++!=this.max&&this.node.Read(s));
				return this.min<=i;
			}

			public NodeAssociativity Associativity{
				get{
					if(min==0&&max==1||min==1&&max<0)return NodeAssociativity.Strong;
					return NodeAssociativity.Stronger;
				}
			}

			private class Tester:StickyRepeatNode,ITester{
				private readonly StickyRepeatNode parent;

				public Tester(StickyRepeatNode parent,INode node,int min,int max):base(node,min,max){
					this.parent=parent;
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.node,this.min,this.max);
					ret.continuous_state=this.continuous_state;
					ret.imatch=this.imatch;
					return ret;
				}

				public bool Indefinite{
					get{return false;}
				}

#if __MWG_MINOR
				public ITester Read(Status s)
					__minor(instance continuous)
				{
					for(instance int imatch=0;imatch!=this.max;imatch++){
						s.Success=true;
						return node.GetTester();
						if(!s.Success)break;
					}

					s.Success=imatch>=this,min;
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

					s.Success=imatch>=this.min;
					continuous_state=-1; return null;
#pragma warning restore 164
				}
				private int continuous_state=0;
				private int imatch;

				#endregion
#endif
			}
		}
		#endregion

		#region node:ElemNodeBase
		/// <summary>
		/// 単一要素に一致する正規表現要素の基本クラスを提供します。
		/// </summary>
		public abstract class ElemNodeBase:INode{
			internal ElemNodeBase(){}

			/// <summary>
			/// 正規表現文字列に於ける表記の結合性を返します。
			/// </summary>
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			/// <summary>
			/// 子正規表現要素の集合を返します。この要素では常に空の集合を返します。
			/// </summary>
			public Gen::IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			/// <summary>
			/// ノードが非決定性を持つか否かを返します。この要素では常に false を返します。
			/// </summary>
			public bool Nondeterministic{
				get{return false;}
			}

			/// <summary>
			/// 試験を行う為のインスタンスを取得します。
			/// この要素ではこのメソッドを呼び出す事は想定していません。
			/// 呼び出すと例外を発生させます。
			/// </summary>
			/// <returns>この要素では戻り値はありません。</returns>
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			/// <summary>
			/// 指定した値に一致するか否かを判定します。
			/// </summary>
			/// <param name="value">一致するか判定する対象の値を指定します。</param>
			/// <returns>一致すると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public abstract bool Judge(T value);

			bool INode.Read(Status s){
				ITypedStream<T> t=s.Target;
				if(t.EndOfStream||!this.Judge(t.Current))return false;
				t.MoveNext();
				return true;
			}

#if Ver1_0a2
			[System.Obsolete]
			protected abstract class TesterBase:ITester{
				public TesterBase(){}

				public ITester Clone(){
					throw new System.InvalidProgramException("Clone 関数の呼び出しは想定していません");
				}

				public abstract bool Judge(T value);

				public ITester Read(Status s){
					bool success=!s.Target.EndOfStream&&this.Judge(s.Target.Current);
					s.Success=success;
					if(success)s.Target.MoveNext();
					return null;
				}
				public bool Indefinite{
					get{return false;}
				}
			}
#endif
		}

		/// <summary>
		/// 任意の値に一致する正規表現要素です。
		/// </summary>
		protected sealed class AnyElemNode:ElemNodeBase{
			private AnyElemNode(){}
			private static readonly AnyElemNode instance=new AnyElemNode();
			/// <summary>
			/// この Node の単一インスタンスを取得します。
			/// </summary>
			public static AnyElemNode Instance{
				get{return instance;}
			}
			/// <summary>
			/// 指定した値に一致するか否かを判定します。
			/// </summary>
			/// <param name="value">一致するか判定する対象の値を指定します。</param>
			/// <returns>一致すると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public override bool Judge(T value) {
				return true;
			}
			/// <summary>
			/// この要素の文字列表現を取得します。
			/// </summary>
			/// <returns>この要素の正規表現文字列に於ける表記を返します。</returns>
			public override string ToString(){
				return ".";
			}
#if Ver1_0a2
			Tester inst=new Tester();
			public override ITester GetTester(){
				return this.inst;
			}

			[System.Obsolete]
			private sealed class Tester:TesterBase{
				public Tester(){}

				public override bool Judge(T value){
					return true;
				}

				public override string ToString(){
					return ".";
				}
			}
#endif
		}
		/// <summary>
		/// 特定の値と等しい値に一致する正規表現要素です。
		/// </summary>
		protected sealed class EquivElemNode:ElemNodeBase{
			readonly T value;
			/// <summary>
			/// 指定した値を使用して EquivElemNode を初期化します。
			/// </summary>
			/// <param name="name">この Node の正規表現文字列に於ける表記を指定します。</param>
			/// <param name="value">比較対象の値を指定します。
			/// この値と等しい要素に対して一致します。</param>
			public EquivElemNode(string name,T value){
				this.value=value;
				this.name=name;
#if Ver1_0a2
				this.inst=new Tester(value);
#endif
			}

			private readonly string name;
			/// <summary>
			/// 此の要素に対応する正規表現を取得します。
			/// </summary>
			public string Name{
				get{return this.name;}
			}
			/// <summary>
			/// この要素の文字列表現を取得します。
			/// </summary>
			/// <returns>この要素の正規表現文字列に於ける表記を返します。</returns>
			public override string ToString() {
				return this.name;
			}
			/// <summary>
			/// 指定した値に一致するか否かを判定します。
			/// </summary>
			/// <param name="value">一致するか判定する対象の値を指定します。</param>
			/// <returns>一致すると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public override bool Judge(T value){
				return this.value.Equals(value);
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}

			private sealed class Tester:TesterBase{
				private readonly T value;
				public Tester(T value){
					this.value=value;
				}

				public override bool Judge(T value){
					return this.value.Equals(value);
				}
			}
#endif
		}
		/// <summary>
		/// 関数を使用して単一要素に一致するか否か判定する正規表現要素です。
		/// </summary>
		protected sealed class FunctionElemNode:ElemNodeBase{
			System.Predicate<T> f;

			internal FunctionElemNode(string name,System.Predicate<T> f){
				this.f=f;
				this.name=name;
#if Ver1_0a2
				this.inst=new Tester(f,name);
#endif
			}
			/// <summary>
			/// 指定した値に一致するか否かを判定します。
			/// </summary>
			/// <param name="value">一致するか判定する対象の値を指定します。</param>
			/// <returns>一致すると判定された場合に true を返します。それ以外の場合に false を返します。</returns>
			public override bool Judge(T value){
				return this.f(value);
			}

#if Ver1_0a2
			Tester inst;
			public override ITester GetTester(){
				return this.inst;
			}

			private sealed class Tester:TesterBase {
				private readonly System.Predicate<T> f;
				private readonly string name;
				public Tester(System.Predicate<T> f,string name){
					this.f=f;
					this.name=name;
				}

				public override bool Judge(T value){
					return this.f(value);
				}
			}
#endif

			private readonly string name;
			/// <summary>
			/// 此の要素に対応する正規表現を取得します。
			/// </summary>
			public string Name{
				get{return this.name;}
			}

			/// <summary>
			/// この要素の文字列表現を取得します。
			/// </summary>
			/// <returns>この要素の正規表現文字列に於ける表記を返します。</returns>
			public override string ToString() {
				return this.name;
			}
		}
		#endregion

#if Ver1_0a2
		#region node:ElemEquals
		/// <summary>
		/// </summary>
		[System.Obsolete]
		protected class ElemEqualsNode:INode{
			protected T value;
			internal ElemEqualsNode(T value){
				this.value=value;
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			public override string ToString(){
				if(this.value==null)return "{null}";
				return this.value.ToString();
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			public bool Nondeterministic{
				get{return false;}
			}
			public ITester GetTester(){
				return new DeterministicTester(this);
			}
			bool INode.Read(Status s){
				ITypedStream<T> str=s.Target;
				bool ret=!str.EndOfStream&&this.value.Equals(str.Current);
				if(ret)str.MoveNext();
				return ret;
			}

#if Ver1_0a2
			private class Tester:ITester{
				private readonly T value;
				public Tester(T value){
					this.value=value;
				}

				public override string ToString(){
					if(this.value==null)return "{null}";
					return this.value.ToString();
				}

#if TESTER_ASSOC
				public NodeAssociativity Associativity{
					get{return NodeAssociativity.Strong;}
				}
#endif
				public ITester Clone(){
					return new Tester(this.value);
				}

				public ITester Read(Status s){
					s.Success=!s.Target.EndOfStream&&this.value.Equals(s.Target.Current);
					if(s.Success)s.Target.MoveNext();
					return null;
				}

				public bool Indefinite{
					get{return false;}
				}
			}
#endif
		}
		#endregion
#endif

		#region node:FunctionNode
		/// <summary>
		/// 判定関数を使用した判定を実行します。
		/// </summary>
		protected sealed class FunctionNode:INode{
			private readonly string src;
			private readonly System.Predicate<ITypedStream<T>> proc;
			/// <summary>
			/// 指定した判定関数を使用して FunctionNode を初期化します。
			/// </summary>
			/// <param name="src">この要素の正規表現文字列に於ける表記を指定します。</param>
			/// <param name="proc">判定を行う関数を指定します。</param>
			public FunctionNode(string src,System.Predicate<ITypedStream<T>> proc){
				this.src=src;
				this.proc=proc;
			}
			/// <summary>
			/// この要素の文字列表現を取得します。
			/// </summary>
			/// <returns>この要素の正規表現文字列に於ける表記を返します。</returns>
			public override string ToString() {
				return this.src;
			}

			//--------------------------------------------------------
			//		INode
			//--------------------------------------------------------
			/// <summary>
			/// 正規表現文字列に於ける表記の結合性を返します。
			/// </summary>
			public NodeAssociativity Associativity {
				get{return NodeAssociativity.Strong;}
			}
			/// <summary>
			/// 子正規表現要素の集合を返します。この要素では常に空の集合を返します。
			/// </summary>
			public System.Collections.Generic.IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			/// <summary>
			/// ノードが非決定性を持つか否かを返します。この要素では常に false を返します。
			/// </summary>
			public bool Nondeterministic{
				get{return false;}
			}
			/// <summary>
			/// 試験を行う為のインスタンスを取得します。
			/// この要素ではこのメソッドを呼び出す事は想定していません。
			/// 呼び出すと例外を発生させます。
			/// </summary>
			/// <returns>この要素では戻り値はありません。</returns>
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			/// <summary>
			/// 指定した要素に一致するか否かを判定します。
			/// </summary>
			/// <param name="s">読み取りに使用している Status を指定します。</param>
			/// <returns>要素に一致した場合に true を返します。それ以外の場合に false を返します。</returns>
			public bool Read(Status s) {
				return proc(s.Target);
			}
		}
		#endregion

		/* 以下未 check */

		#region node:AheadAssert
		/// <summary>
		/// 零幅の先読みを実行します。
		/// </summary>
		private class AheadAssertNode:INode{
			protected bool positive;
			protected INode node;
			protected RegexFactory3<T> fac;

			internal AheadAssertNode(INode node,bool positive,RegexFactory3<T> factory){
				this.positive=positive;
				this.node=node;
				this.fac=factory;
			}
			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}

			public override string ToString(){
				return (this.positive?"(?=":"(?!")+this.node.ToString()+")";
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			public bool Nondeterministic{
				get{return false;}
			}
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			bool INode.Read(Status s){
				int index0=s.Index;
				bool ret;
				using(new StatusStackRefuge(s))
					ret=this.positive==this.fac.match(s,this.node);
				s.Index=index0;
				return ret;
			}

#if Ver1_0a2
			private class Tester:AheadAssertNode,ITester{
				private readonly AheadAssertNode parent;

				public Tester(AheadAssertNode parent,INode node,bool positive,RegexFactory3<T> factory)
					:base(node,positive,factory)
				{
					this.parent=parent;
				}


				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.node,this.positive,this.fac);
					return ret;
				}

				public bool Indefinite{
					get{return false;}
				}

				public ITester Read(Status s){
					int index0=s.Index;
#if USING_EXTRA_CAPTURE
					CaptureRange capt=new CaptureRange(s,this.parent);
#endif
					using(new StatusStackRefuge(s))
						s.Success=this.positive==this.fac.match(s,this.node);
#if USING_EXTRA_CAPTURE
					if(s.Success)capt.Complete(s);
#endif
					s.Index=index0;
					return null;
				}
			}
#endif
			[System.Obsolete]
			private class CaptureRange:CaptureRangeBase{
				int read_end;
				public CaptureRange(Status s,INode parent):base(s,parent){}
				public override int ReadEnd{
					get{return this.read_end;}
				}
				public override void Complete(Status s){
					this.read_end=s.Index;
					s.Index=this.Start;
					base.Complete(s);
				}
			}
		}
		#endregion

		#region node:NonBacktrack
		private class NonBacktrackNode:INode{
			protected readonly INode node;
			protected readonly RegexFactory3<T> factory;

			public NonBacktrackNode(INode node,RegexFactory3<T> factory){
				this.node=node;
				this.factory=factory;
			}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}

			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			public override string ToString(){
				return "(?>"+this.node.ToString()+")";
			}
			public bool Nondeterministic{
				get{return false;}
			}
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			bool INode.Read(Status s){
				using(new StatusStackRefuge(s))
					return this.factory.match(s,this.node);
			}

#if Ver1_0a2
			private class Tester:NonBacktrackNode,ITester{
				private readonly NonBacktrackNode parent;

				public Tester(NonBacktrackNode parent,INode node,RegexFactory3<T> factory):base(node,factory){
					this.parent=parent;
				}

				public ITester Read(Status s){
					using(new StatusStackRefuge(s))
						s.Success=this.factory.match(s,this.node);

					return null;
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.node,this.factory);
					return ret;
				}

				public bool Indefinite{
					get{return false;}
				}
			}
#endif
		}
		#endregion

		#region node:Capture
		private class CaptureNode:INode{
			protected readonly string name;
			protected readonly INode node;

			public CaptureNode(INode node,string name){
				this.name=name;
				this.node=node;
			}
			public CaptureNode(INode node):this(node,null){}

			public Gen::IEnumerable<INode> EnumChildren{
				get{return new INode[]{this.node};}
			}

			public string Name{
				get{return this.name;}
			}
			public override string ToString(){
				return (this.name!=null?"(?<"+this.name+">":"(")+this.node.ToString()+")";
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}

			public bool Nondeterministic{
				get{return this.node.Nondeterministic;}
			}
			public ITester GetTester(){
				return new Tester(this,node,name);
			}
			bool INode.Read(Status s){
				CaptureRangeBase capt=new CaptureRangeBase(s,this);
				bool ret=this.node.Read(s);
				if(ret)capt.Complete(s);
				return ret;
			}

			private class Tester:CaptureNode,ITester{
				private readonly CaptureNode parent;

				public Tester(CaptureNode parent,INode node,string name):base(node,name){
					this.parent=parent;
				}

				int status=0;
				CaptureRangeBase capt;
				public ITester Read(Status s){
					if(status==0){
						capt=new CaptureRangeBase(s,this.parent);
						this.status=1;return this.node.GetTester();
					}else if(status==1){
						// ※ s.Success は既に設定されている物を流用
						if(s.Success)capt.Complete(s);
						status=2;return null;
					}else{
						throw new System.InvalidOperationException("この Node による読み取りは既に終了しています。");
					}
				}

				public bool Indefinite{
					get{return false;}
				}

				public ITester Clone(){
					Tester ret=new Tester(this.parent,this.node,this.name);
					ret.status=this.status;
					ret.capt=this.capt;
					return ret;
				}
			}
		}
		#endregion

		#region node:StartAndEnd
		private sealed class StartOfStreamNode:INode{
			private StartOfStreamNode(){}

			public static readonly StartOfStreamNode instance=new StartOfStreamNode();

			public override string ToString(){
				return "^";
			}
			public Gen::IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			public bool Nondeterministic{
				get{return false;}
			}
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			bool INode.Read(Status s){
				return s.Target.IsStart;
			}
#if Ver1_0a2
			//========================================================
			//		ITester
			//========================================================
			public ITester Read(Status s){
				s.Success=s.Target.IsStart;
				return null;
			}
			public bool Indefinite{
				get{return false;}
			}
			public ITester Clone(){
				throw new System.NotImplementedException("未実装。呼び出されない筈。");
			}
#endif
		}
		private sealed class EndOfStreamNode:INode{
			private EndOfStreamNode(){}

			public static readonly EndOfStreamNode instance=new EndOfStreamNode();

			public override string ToString(){
				return "$";
			}
			public Gen::IEnumerable<INode> EnumChildren{
				get{return EmptyNodes;}
			}
			public NodeAssociativity Associativity{
				get{return NodeAssociativity.Strong;}
			}
			public bool Nondeterministic{
				get{return false;}
			}
			public ITester GetTester(){
				throw new System.NotImplementedException();
				//return new DeterministicTester(this);
			}
			bool INode.Read(Status s){
				return s.Target.EndOfStream;
			}
#if Ver1_0a2
			//========================================================
			//		ITester
			//========================================================
			public ITester Read(Status s){
				s.Success=s.Target.EndOfStream;
				return null;
			}
			public bool Indefinite{
				get{return false;}
			}
			public ITester Clone(){
				throw new System.NotImplementedException("未実装。呼び出されない筈。");
			}
#endif
		}
		#endregion

		/// <summary>
		/// Deterministic な INode に対する ITester を提供します。
		/// <para>
		/// 本来、Deterministic な INode については ITester を実装する必要はありません。
		/// 然し乍ら、ルートノードになってしまった時は GetTester() を返さなければなりません。
		/// その時にはこの DeterministicTester を用いると実装の手間を省く事が出来ます。
		/// </para>
		/// </summary>
		internal class DeterministicTester:ITester{
			readonly INode node;
			/// <summary>
			/// Deterministic な Node を使用して DeterministicTester を初期化します。
			/// </summary>
			/// <param name="node">テストに使用する INode を指定します。
			/// node.Nondeterministic は false である必要があります。</param>
			public DeterministicTester(INode node){
				__debug__.RegexTestAssert(!node.Nondeterministic);
				this.node=node;
			}

			public override string ToString(){
				return this.node.ToString();
			}

			public ITester Read(Status s){
				s.Success=this.node.Read(s);
				return null;
			}
			public bool Indefinite{get{return false;}}
			public ITester Clone(){throw new System.Exception("The method or operation is not implemented.");}
		}
	}
}
