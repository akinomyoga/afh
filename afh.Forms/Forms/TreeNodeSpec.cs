using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2=System.Drawing.Drawing2D;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Ser=System.Runtime.Serialization;
using Forms=System.Windows.Forms;

using BitSection=afh.Collections.BitSection;

namespace afh.Forms{

	#region class:TextTreeNode
	/// <summary>
	/// �������\������ׂ� TreeNode �ł��B
	/// </summary>
	[System.Serializable]
	public class TextTreeNode:TreeNode,Ser::ISerializable{
		private string text;
		Gdi::Size sizeContent;
		/// <summary>
		/// �\����������擾���͐ݒ肵�܂��B
		/// </summary>
		public string Text{
			get{return this.text;}
			set{
				if(this.text==value)return;
				this.text=value??"";
				Gdi::SizeF sz=afh.Drawing.GraphicsUtils.MeasureString(this.text,this.Font);
				this.sizeContent.Width=(int)sz.Width;
				this.sizeContent.Height=(int)sz.Height+1;

			//	Forms::TreeNode node;
			//	TreeNode nd;
				_todo.TreeNode("���e�ω��ɔ����ĕ`��");
				_todo.TreeNodeDisplayHeight("���e�ω��ɔ������g�̃T�C�Y�ω�");
			}
		}
		//============================================================
		//		������
		//============================================================
		/// <summary>
		/// �w�肵����������g�p���� TextTreeNode �����������܂��B
		/// </summary>
		/// <param name="text">TreeNode �̕\���Ɏg�p���镶������w�肵�܂��B</param>
		public TextTreeNode(string text):base(){
			this.Text=text;
		}
		/// <summary>
		/// �w�肵���V���A���C�Y��񂩂�ATextTreeNode �𕜌����܂��B
		/// </summary>
		/// <param name="info">�V���A���C�Y�����f�[�^���w�肵�܂��B</param>
		/// <param name="ctx">�V���A���C�Y�̊��Ɋ֘A��������w�肵�܂��B</param>
		public TextTreeNode(Ser::SerializationInfo info,Ser::StreamingContext ctx)
			:base(info,ctx)
		{
			this.Text=info.GetString("text");
		}
		/// <summary>
		/// �V���A���C�Y�̍ۂɋL�^����f�[�^�� SerializationInfo �ɏ������݂܂��B
		/// </summary>
		/// <param name="info">�f�[�^�̋L�^��� SerializationInfo ���w�肵�܂��B</param>
		/// <param name="context">�V���A���C�Y�̊��Ɋ֘A��������w�肵�܂��B</param>
		protected override void GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context){
			base.GetObjectData(info,context);
			info.AddValue("text",this.text);
		}
		//============================================================
		//		�\��
		//============================================================
		/// <summary>
		/// ���e��\������̈�̑傫�����擾���܂��B
		/// </summary>
		public override Gdi::Size ContentSize{
			get{return this.sizeContent;}
		}
		/// <summary>
		/// ���̃m�[�h�̓��e��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		protected override void DrawContent(System.Drawing.Graphics g){
			this.DrawContentBackground(g);

			Gdi::Rectangle rect=new Gdi::Rectangle(Gdi::Point.Empty,this.ContentSize);

			Gdi::Brush brush;
			if(this.IsEnabled){
				brush=this.IsActivated?Gdi::Brushes.White:Gdi::Brushes.Black;
			}else{
				brush=this.IsActivated?Gdi::Brushes.Gray:Gdi::Brushes.Silver;
			}

			g.DrawString(this.text,this.Font,brush,new Gdi::PointF(0,1));
		}
	}
	#endregion

	#region class:TreeTreeNode
	/// <summary>
	/// afh.Collections.ITree&lt;T&gt; �^��\������ׂ� TreeNode �ł��B
	/// </summary>
	/// <typeparam name="T">�\������I�u�W�F�N�g�̌^�ł��B</typeparam>
	[System.Serializable]
	public class TreeTreeNode<T>:TextTreeNode,Ser::ISerializable
		where T:afh.Collections.ITree<T>
	{
		//��ITree ���ŕύX���������ꍇ�� feedback ��?

		readonly afh.Collections.ITree<T> value;
		/// <summary>
		/// TreeTreeNode �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="value">�Ή����� ITree �I�u�W�F�N�g���w�肵�܂��B</param>
		public TreeTreeNode(T value):base(value.ToString()){
			this.value=value;
		}
		/// <summary>
		/// TreeTreeNode �̃R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="value">�Ή����� ITree �I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="name">ITree �I�u�W�F�N�g�̖��O���w�肵�܂��B</param>
		protected TreeTreeNode(T value,string name):base(name){
			this.value=value;
		}
		/// <summary>
		/// ���̃m�[�h�ɑΉ����� ITree �I�u�W�F�N�g���擾���܂��B
		/// </summary>
		public T Value{
			get{return (T)value;}
		}

		/// <summary>
		/// �q�m�[�h�����������܂��B
		/// </summary>
		/// <returns>�V�����쐬�����q�m�[�h�W����Ԃ��܂��B</returns>
		protected override void InitializeNodes(TreeNodeCollection nodes){
			foreach(T child in this.value.Nodes){
				TreeTreeNode<T> childNode=new TreeTreeNode<T>(child);
				if(this.DDBehaviorInherit==TreeNodeInheritType.Custom)
					childNode.DDBehavior=this.DDBehavior;
				nodes.Add(childNode);
			}
		}
		/// <summary>
		/// �q�m�[�h�����݂��邩�ۂ����擾���܂��B
		/// </summary>
		public override bool HasChildren{
			get{return value.Nodes.Count>0;}
		}

		#region DDBehavior
		/// <summary>
		/// �����h���b�O�h���b�v�����L���ɂ��܂��B
		/// </summary>
		public virtual void SetupAutoDD(){
			this.DDBehavior=MyDDBehavior.Instance;
		}

		//-------------------------------------------------------------------------
		//	for MyDDBehavior
		//-------------------------------------------------------------------------
		/// <summary>
		/// ���̗v�f�𑼂̏ꏊ�ֈړ����鎖���o���邩�ۂ����擾���܂��B
		/// </summary>
		protected virtual bool IsMovable{
			get{return this.Value.Parent==null||!this.Value.Parent.Nodes.IsReadOnly;}
		}
		/// <summary>
		/// ���̗v�f���R�s�[���鎖���o���邩�ۂ����擾���܂��B
		/// </summary>
		protected virtual bool IsCopyable{
			get{return this is System.ICloneable;}
		}
		/// <summary>
		/// ���̗v�f�ւ̃����N���쐬���鎖���\���ۂ����擾���܂��B
		/// </summary>
		protected virtual bool IsLinkable{
			get{return false;}
		}

		private Forms::DragDropEffects DragEffectToTreeNode{
			get{
				System.Windows.Forms.DragDropEffects ret=0;
				if(this.IsMovable)ret|=System.Windows.Forms.DragDropEffects.Move;
				if(this.IsCopyable)ret|=System.Windows.Forms.DragDropEffects.Copy;
				if(this.IsLinkable)ret|=System.Windows.Forms.DragDropEffects.Link;
				return ret;
			}
		}

		/// <summary>
		/// ITree �ɑ΂������� DDBehavior �N���X�ł��B
		/// </summary>
		protected class MyDDBehavior:TreeNodeDDBehaviorBase{
			static MyDDBehavior inst=null;
			/// <summary>
			/// MyDDBegavior �C���X�^���X���擾���܂��B
			/// </summary>
			public static MyDDBehavior Instance{
				get{return inst??(inst=new MyDDBehavior());}
			}

			/// <summary>
			/// �w�肵�� TreeNode ���h���b�O�\���ۂ����肵�A
			/// �\�ȃh���b�O�h���b�v�G�t�F�N�g��Ԃ��܂��B
			/// </summary>
			/// <param name="node">����Ώۂ� TreeNode ���w�肵�܂��B</param>
			/// <returns>�h���b�O�\�̏ꍇ�ɂ́A�\�� DragDropEffects ��Ԃ��܂��B
			/// �h���b�O�s�\�̏ꍇ�ɂ� DragDropEffects.None ��Ԃ��܂��B</returns>
			public override System.Windows.Forms.DragDropEffects GetAllowedDDE(TreeNode node){
				TreeTreeNode<T> _node=node as TreeTreeNode<T>;
				if(_node==null)return Forms::DragDropEffects.None;

				// ����ނ̕��̂փh���b�v����ׁA��芸�����S�ĉ\�B
				return Forms::DragDropEffects.All;
			}

			//-----------------------------------------------------------------------
			// OnDrop
			//-----------------------------------------------------------------------
			/// <summary>
			/// �h���b�v���������s���܂��B
			/// </summary>
			/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
			/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
			public override void OnDrop(TreeNode node,TreeNodeDragEventArgs e){
				TreeTreeNode<T> _dst=node as TreeTreeNode<T>;
				TreeNodeDDTarget target=this.GetTarget(node,e);
				switch(e.Effect&this.GetEffect(node,e)){
					case Forms::DragDropEffects.Move:{
						TreeNode node2=e.Data.GetData("afh.Forms.TreeNode") as TreeNode;
						if(node2!=null){
							this.OnDrop_MoveNode(_dst,node2,target);
							break;
						}

						Gen::List<TreeNode> list=e.Data.GetData("afh.Forms.TreeNode:List") as Gen::List<TreeNode>;
						if(list!=null){
							this.OnDrop_MoveList(_dst,list,target);
							break;
						}

						break;
					}
					case Forms::DragDropEffects.Link:
					case Forms::DragDropEffects.Copy:
						throw new System.NotImplementedException();
					default:
						break;
				}
			}
			private void OnDrop_MoveNode(TreeTreeNode<T> _dst,TreeNode src,TreeNodeDDTarget target){
				TreeTreeNode<T> _src=src as TreeTreeNode<T>;
				if(_src==null)return;

				T dval=_dst.Value;

				switch(target){
					case TreeNodeDDTarget.Self:
						dval.Nodes.Add(_src.Value);
						_dst.Nodes.Add(_src);
						break;
					case TreeNodeDDTarget.Child:
						dval.Nodes.Insert(0,_src.Value);
						_dst.Nodes.Insert(0,_src);
						break;
					case TreeNodeDDTarget.Prev:
						dval.Parent.Nodes.Insert(dval.Parent.Nodes.IndexOf(dval),_src.Value);
						_dst.ParentNode.Nodes.InsertBefore(_dst,_src);
						break;
					case TreeNodeDDTarget.Next:
						dval.Parent.Nodes.Insert(dval.Parent.Nodes.IndexOf(dval)+1,_src.Value);
						_dst.ParentNode.Nodes.InsertAfter(_dst,_src);
						break;
				}
			}
			private void OnDrop_MoveList(TreeTreeNode<T> _dst,Gen::List<TreeNode> list,TreeNodeDDTarget target){
				_todo.TreeNodeDisplayHeight("TreeNodeCollection.Suppress: �m�[�h���X�g�̊􉽍Čv�Z�E�ĕ`���}�~");
				_todo.TreeNodeDisplayHeight("target==Child �̎��� AddRange �𗘗p����l�ɂ���?");
				using(_dst.Nodes.SuppressLayout())
				foreach(TreeNode node3 in list)
					this.OnDrop_MoveNode(_dst,node3,target);
			}
			//-----------------------------------------------------------------------
			//	GetEffect
			//-----------------------------------------------------------------------
			/// <summary>
			/// �h���b�O�h���b�v����̊��҂���� DDE ���擾���܂��B
			/// </summary>
			/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
			/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
			/// <returns>�h���b�v�ɂ���Ċ��҂���� DDE ��Ԃ��܂��B</returns>
			protected override Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e){
				//��TODO: ���̃v���Z�X���痈���ꍇ�͂ǂ��Ȃ�̂�?

				// �L�[���
				Forms::DragDropEffects mask;
				switch(e.KeyState){
					case TreeNodeDragEventArgs.KEY_MLEFT|TreeNodeDragEventArgs.KEY_CTRL:
						mask=Forms::DragDropEffects.Copy;
						break;
					case TreeNodeDragEventArgs.KEY_MLEFT|TreeNodeDragEventArgs.KEY_ALT:
						mask=Forms::DragDropEffects.Link;
						break;
					default:
						mask=Forms::DragDropEffects.Move;
						break;
				}

				// �h���b�v��
				TreeTreeNode<T> _dst=node as TreeTreeNode<T>;
				if(_dst==null)goto none;
				switch(this.GetTarget(node,e)){
					case TreeNodeDDTarget.Self:
					case TreeNodeDDTarget.Child:
						break;
					case TreeNodeDDTarget.Next:
					case TreeNodeDDTarget.Prev:
						TreeTreeNode<T> parent=_dst.ParentNode as TreeTreeNode<T>;
						if(parent==null)goto none;
						_dst=parent;
						break;
					default:
						goto none;
				}
				if(_dst.Value.Nodes.IsReadOnly)goto none;

				// �h���b�O�I�u�W�F�N�g
				{
					// �P��m�[�h�̏ꍇ
					TreeNode node2=e.Data.GetData("afh.Forms.TreeNode") as TreeNode;
					if(node2!=null)
						return GetEffect_Node(_dst,node2,mask);

					// �m�[�h���X�g�̏ꍇ
					Gen::List<TreeNode> list=e.Data.GetData("afh.Forms.TreeNode:List") as Gen::List<TreeNode>;
					if(list!=null)
						return GetEffect_List(_dst,list,mask);

					//�������Ŋg���\��
				}

			none:
				return Forms::DragDropEffects.None;
			}
			private Forms::DragDropEffects GetEffect_Node(TreeTreeNode<T> _dst,TreeNode src,Forms::DragDropEffects mask){
				TreeTreeNode<T> _src=src as TreeTreeNode<T>;
				if(_src==null)return Forms::DragDropEffects.None;

				mask&=_src.DragEffectToTreeNode;
				if(_src.IsDescendant(_dst)||_src==_dst)mask&=~Forms::DragDropEffects.Move;
				
				return mask;
			}
			private Forms::DragDropEffects GetEffect_List(TreeTreeNode<T> _dst,Gen::List<TreeNode> list,Forms::DragDropEffects mask){
				foreach(TreeNode node2 in list){
					TreeTreeNode<T> _src=node2 as TreeTreeNode<T>;
					if(_src==null)return Forms::DragDropEffects.None;

					mask&=_src.DragEffectToTreeNode;
					if(_src.IsDescendant(_dst)||_src==_dst)mask&=~Forms::DragDropEffects.Move;
				}

				return mask;
			}
		}
		#endregion
	}
	#endregion
}
