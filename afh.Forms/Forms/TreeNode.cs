using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2=System.Drawing.Drawing2D;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Ser=System.Runtime.Serialization;
using Forms=System.Windows.Forms;

using BitSection=afh.Collections.BitSection;
//using TreeNodeMouseEH=afh.EventHandler<TreeNode,TreeNodeMouseEventArgs>;

namespace afh.Forms{
	/// <summary>
	/// ����_���A����m�[�h�ɑ΂��Ăǂ̗l�Ȉʒu�֌W�ɂ��邩�������܂��B
	/// </summary>
	[System.Flags]
	public enum TreeNodeHitType{
		//
		//	��������
		//
		/// <summary>[����] �m�[�h�̈������ɂ��鎖�������܂��B</summary>
		Above		=0x0100,
		/// <summary>[����] �m�[�h�̈�̏�[�߂��ɂ��鎖�������܂��B</summary>
		BorderTop	=0x0200,
		/// <summary>[����] �m�[�h�̈�̐^�񒆂̍����ɂ��鎖�������܂��B</summary>
		Middle		=0x0400,
		/// <summary>[����] �m�[�h�̈�̉��[�߂��ɂ��鎖�������܂��B</summary>
		BorderBottom=0x0800,
		/// <summary>[����] �m�[�h�̈�������ɂ��鎖�������܂��B</summary>
		Below		=0x1000,
		//
		//	���ʒu����
		//
		/// <summary>[���ʒu] �C���f���g�̈�̉��ʒu�ɂ��鎖�������܂��B</summary>
		IndentArea	=0x01,
		/// <summary>[���ʒu] �`�F�b�N�{�b�N�X�̉��ʒu�ɂ��鎖�������܂��B</summary>
		CheckBox	=0x02,
		/// <summary>[���ʒu] �A�C�R���̉��ʒu�ɂ��鎖�������܂��B</summary>
		Icon		=0x04,
		/// <summary>[���ʒu] �m�[�h���e�\���̈�̉��ʒu�ɂ��鎖�������܂��B</summary>
		Content		=0x08,
		/// <summary>[���ʒu] �m�[�h�̈�������ɂ��鎖�������܂��B</summary>
		PaddingLeft	=0x10,
		/// <summary>[���ʒu] �m�[�h�̈�����E�ɂ��鎖�������܂��B</summary>
		PaddingRight=0x20,
		//
		//	�����蔻��
		//
		/// <summary>[������] �m�[�h�̃C���f���g�̈��ɂ��鎖�������܂��B</summary>
		OnPlusMinus	=IndentArea	<<16, // 0x010000
		/// <summary>[������] �m�[�h�̃`�F�b�N�{�b�N�X�̈��ɂ��鎖�������܂��B</summary>
		OnCheckBox	=CheckBox	<<16, // 0x020000
		/// <summary>[������] �m�[�h�̃A�C�R���̈��ɂ��鎖�������܂��B</summary>
		OnIcon		=Icon		<<16, // 0x040000
		/// <summary>[������] �m�[�h���e�\���̈��ɂ��鎖�������܂��B</summary>
		OnContent	=Content	<<16, // 0x080000
		/// <summary>[������] �m�[�h��̉������ɂ��鎖�������܂��B</summary>
		Hit			=0x100000,
		//
		//	��
		//
		/// <summary>[�}�X�N] �m�[�h�̈�Ɠ��������ɂ��鎖�������܂��B</summary>
		mask_vertical_hit		=BorderTop|Middle|BorderBottom,
		/// <summary>[�}�X�N] ���ʒu�𔻒肷��ׂ̃}�X�N�ł��B</summary>
		mask_horizontal			=mask_horizontal_pmhit|PaddingLeft|PaddingRight,
		/// <summary>[�}�X�N] �m�[�h�̈�Ɠ������ʒu�ɂ��鎖�������܂��B</summary>
		mask_horizontal_hit		=CheckBox|Icon|Content,
		/// <summary>[�}�X�N] �m�[�h�̈�E�C���f���g�̈�Ɠ������ʒu�ɂ��鎖�������܂��B</summary>
		mask_horizontal_pmhit	=IndentArea|mask_horizontal_hit,
		/// <summary>[�}�X�N] �m�[�h�̓���̗̈�𔻒肷��ׂ̃}�X�N�ł��B</summary>
		mask_on_something		=OnPlusMinus|OnCheckBox|OnIcon|OnContent,
	}

	/// <summary>
	/// TreeNode �� Checked ��Ԃ̊Ǘ��Ɏg�p���܂��B
	/// </summary>
	[System.Flags]
	public enum TreeNodeCheckedState{
		/// <summary>
		/// CheckBox �� check �������Ă��Ȃ���Ԃ������܂��B
		/// </summary>
		Unchecked=0,
		/// <summary>
		/// CheckBox �� check �������Ă����Ԃ������܂��B
		/// </summary>
		Checked=1,
		/// <summary>
		/// CheckBox �� Check ��Ԃ����Ԃł��鎖�������܂��B
		/// </summary>
		Intermediate=2,
	}
	/// <summary>
	/// �e��ݒ�̌p���Ȃǂ̕��@���w�肷��̂Ɏg�p���܂��B
	/// </summary>
	public enum TreeNodeInheritType{
		/// <summary>
		/// TreeView �ɐݒ肳�ꂽ����l���g�p���܂��B
		/// </summary>
		Default=0,
		/// <summary>
		/// �e�m�[�h����p�����܂��B
		/// </summary>
		Inherit=1,
		/// <summary>
		/// �ŗL�̏����g�p���܂��B
		/// </summary>
		Custom=2,
	}
	/// <summary>
	/// TreeView �ɕ\��������̗v�f��\������N���X�ł��B
	/// </summary>
	[System.Serializable]
	public partial class TreeNode:System.Runtime.Serialization.ISerializable{
		private TreeView view=null;
		internal TreeNode parent=null;
		private TreeNodeCollection nodes;
		/// <summary>
		/// �g�������o�̊Ǘ��Ɏg�p���܂��B
		/// </summary>
		protected ExtraMemberCache xmem=new ExtraMemberCache();

		private afh.Collections.BitArray64 bits=default_bits;
		static afh.Collections.BitArray64 default_bits=new afh.Collections.BitArray64();
		private const BitSection sNodeHeightInherit			=(BitSection)(2<<16|0);
		private const BitSection sIconVisibleInherit		=(BitSection)(2<<16|2);
		private const BitSection sIconInherit				=(BitSection)(2<<16|4);
		private const BitSection sIconSizeInherit			=(BitSection)(2<<16|6);
		private const BitSection sCheckBoxInherit			=(BitSection)(2<<16|8);
		private const BitSection sCheckBoxVisibleInherit	=(BitSection)(2<<16|10);
		private const BitSection sBackColorInherit			=(BitSection)(2<<16|12);
		private const BitSection sForeColorInherit			=(BitSection)(2<<16|14);
		private const BitSection sIsEnabledInherit			=(BitSection)(2<<16|16);
		private const BitSection sCheckedState				=(BitSection)(2<<16|18);
		private const BitSection sChildIndentInherit		=(BitSection)(2<<16|20);
		private const BitSection sFontInherit				=(BitSection)(2<<16|22);
		private const int bExpanded				=24;
		private const int bVisible				=25;
		private const int bIconVisible			=26;
		private const int bCheckBoxVisible		=27;
		private const int bCheckBoxEnabled		=28;
		private const int bCheckReflectChildren	=29;
		private const int bIsEnabled			=30;

		private const BitSection sDDBehaviorInherit			=(BitSection)(2<<16|32);
		
		
		/*
		public event TreeNodeMouseEH Clicked;
		public event TreeNodeMouseEH MouseEnter;
		public event TreeNodeMouseEH MouseLeave;
		public event TreeNodeMouseEH MouseHover;
		public event TreeNodeMouseEH MouseMove;
		public event TreeNodeMouseEH MouseDown;
		public event TreeNodeMouseEH MouseUp;
		//*/

		/// <summary>
		/// �q�m�[�h�̏W�����擾���܂��B
		/// </summary>
		public TreeNodeCollection Nodes{
			get{
				if(this.nodes==null){
					TreeNodeCollection nodes=new TreeNodeCollection(this);
					this.nodes=nodes;
					using(nodes.SuppressLayout())
						this.InitializeNodes(nodes);
				}
				return this.nodes;
			}
		}
		/// <summary>
		/// �q TreeNode �W�������������܂��B
		/// </summary>
		/// <param name="nodes">�m�[�h���X�g���w�肵�܂��B</param>
		protected virtual void InitializeNodes(TreeNodeCollection nodes){
			/*
			/// <returns>�������������ʂ� TreeNodeCollection ��Ԃ��܂��B</returns>
			/// <remarks>�q�v�f��x���I�ɏ������������ꍇ�ɂ͂��̃��\�b�h�� override ���ĉ������B
			/// override �����ۂ� base.InitializeNodes() �ŃC���X�^���X���쐬���A
			/// �v�f��ǉ����Ă���Ԃ��l�ɂ���Ɨǂ��ł��傤�B
			/// </remarks>
			*/
		}
		/// <summary>
		/// ���� TreeNode ���q�v�f��ێ����Ă��邩�ۂ����擾���܂��B
		/// </summary>
		/// <remarks>�q�v�f��x���I�ɏ������������ꍇ�ɂ́A
		/// ���̃v���p�e�B�� override ���� true ��Ԃ��l�ɂ��܂��傤�B</remarks>
		public virtual bool HasChildren{
			get{return this.nodes!=null&&this.nodes.Count>0;}
		}
		// �n�����: View �ύX�̃��[�g�m�[�h�E�C�x���g�̋N�������m�[�h�EView ��
		// public event afh.EventHandler<TreeNode,TreeView> ViewChanged;
		// TreeNode �ɉ����Đe View ��ێ�����K�v���͂���̂�?
		// �~TreeView �ɒǉ������ۂɁA�q�m�[�h����R����ƍX�V�����
		// ��View �̎擾�͖���e��H������悢��������Ȃ��B
		/// <summary>
		/// �e TreeView ���擾���܂��B������ TreeView �ɂ������Ă��Ȃ����ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeView View{
			get{return this.view;}
			internal set{
				if(this.view==value)return;

				// view �u��
				_todo.TreeNodeOptimize("���� SelectionChanged ���o�^����Ă���񐔂�����������BSuppress/Resume ����������K�v?");
				if(this.view!=null)
					this.view.UnregisterTreeNode(this);
				this.view=value;

				// �q�m�[�h�ɓK�p
				if(this.HasChildren&&this.nodes!=null)foreach(TreeNode node in Nodes){
					node.View=value;
				}
			}
		}
		/// <summary>
		/// �e TreeNode ���擾���܂��B�ǂ� TreeNode �ɂ������Ă��Ȃ����ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode ParentNode{
			get{return this.parent;}
		}
		/// <summary>
		/// ���̃m�[�h��e TreeView ���Ō�����ʒu�Ɉړ����܂��B
		/// </summary>
		public void EnsureVisible(){
			if(view==null)throw new TreeViewMissingException();

			// �W�J�E���m�F
			{
				if(!this.IsVisible)
					throw new TreeNodeException("�w�肵�� TreeNode �͕s���ݒ�ɂȂ��Ă���̂ŕ\���ł��܂���B");

				TreeNode ances=this.parent;
				while(ances!=null){
					if(!ances.IsVisible)
						throw new TreeNodeException("��c�ɕs���ݒ�ɂȂ��Ă��镨������̂ŁA�w�肵�� TreeNode �͕\���ł��܂���B");
					if(!ances.IsExpanded)ances.IsExpanded=true;
					ances=ances.parent;
				}
			}

			Gdi::Point pos=view.AutoScrollPosition;
			pos=new Gdi::Point(-pos.X,-pos.Y); // get_AutoScrollPosition �͉��̂��l���t

			Gdi::Point pnode=this.LocalPos2ClientPos(Gdi::Point.Empty);

			// �c
			if(pnode.Y<0){
				pos.Y+=pnode.Y;
			}else{
				int y_over=pnode.Y+this.NodeHeight-view.ClientSize.Height;
				if(y_over>0)pos.Y+=System.Math.Min(pnode.Y,y_over);
			}

			// ��
			if(pnode.X<0){
				pos.X+=pnode.X;
			}else{
				int x_over=pnode.X+this.NodeWidth-view.ClientSize.Width;
				if(x_over>0)pos.X+=System.Math.Min(pnode.X,x_over);
			}

			view.AutoScrollPosition=pos;
		}
		//============================================================
		//		������
		//============================================================
		/// <summary>
		/// TreeNode �̃R���X�g���N�^�ł��B
		/// </summary>
		public TreeNode(){
			// �E�p�����̏��������I����Ă��Ȃ����ɌĂяo���͕̂s����
			// �E�v�Z�� TreeView �ɒǉ�����鎞�Ɏ��s�����̂�䢂ōs���K�v�͂Ȃ� (?
			// this.RefreshDisplaySize();
		}
		static TreeNode(){
			default_bits[bVisible]=true;
			default_bits[sCheckBoxVisibleInherit]=(uint)TreeNodeInheritType.Inherit;
			default_bits[bCheckBoxEnabled]=true;

			default_bits[sIsEnabledInherit]=(uint)TreeNodeInheritType.Inherit;
			// ����m�[�h�������ɂȂ�����A���̎q���m�[�h���S�Ė����ɂȂ�ׂ��ł���B

			//default_bits[sChildIndentInherit]=(uint)TreeNodeInheritType.Inherit;
		}
		void Ser::ISerializable.GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context) {
			this.GetObjectData(info,context);
		}
		/// <summary>
		/// ���̃m�[�h���V���A���C�Y���܂��B
		/// </summary>
		/// <param name="info">�f�[�^�̊i�[����w�肵�܂��B</param>
		/// <param name="context">�V���A���C�Y�Ɋւ�������w�肵�܂��B</param>
		protected virtual void GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context) {
			TreeNode[] nodes;
			if(this.HasChildren){
				nodes=new TreeNode[this.nodes.Count];
				this.nodes.CopyTo(nodes,0);
			}else{
				nodes=new TreeNode[0];
			}
			info.AddValue("child-nodes",nodes);
			info.AddValue("extra-member",this.xmem);
			info.AddValue("bits",this.bits);

			// parent : �Ȃ� (�q�m�[�h�ɂ͎����Őݒ肷��)
			// view : �Ȃ�
			// szDisplay : �Čv�Z
		}
		/// <summary>
		/// �V���A���C�Y���ꂽ��񂩂�m�[�h�𕜌����܂��B
		/// </summary>
		/// <param name="info">�f�[�^�̊i�[����w�肵�܂��B</param>
		/// <param name="context">�V���A���C�Y�Ɋւ�������w�肵�܂��B</param>
		protected TreeNode(Ser::SerializationInfo info,Ser::StreamingContext context){
			this.bits=(afh.Collections.BitArray64)info.GetValue("bits",typeof(afh.Collections.BitArray64));
			this.xmem=(ExtraMemberCache)info.GetValue("xmem",typeof(ExtraMemberCache));
			TreeNode[] nodes=(TreeNode[])info.GetValue("child-nodes",typeof(TreeNode[]));
			if(nodes.Length!=0){
				this.nodes=new TreeNodeCollection(this); // InitializeNodes �͎g�p�����ɏ�����
				this.nodes.AddRange(nodes);
			}

			this.RefreshDisplaySize();
		}

		#region logics: �`��
		//============================================================
		//		�`��
		//============================================================
		/// <summary>
		/// ���� TreeNode �̓��e���w�肵�� Graphics �ɕ`�悵�܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		/// <param name="draw_child">�q���m�[�h���`�悷�邩�ۂ����擾���͐ݒ肵�܂��B</param>
		public void DrawTo(Gdi::Graphics g,bool draw_child){
			Gdi2::Matrix orig_trans=g.Transform.Clone();

			this.DrawNode(g);

			if(draw_child&&this.IsExpanded){
				g.Transform=orig_trans;
				g.TranslateTransform(this.ChildIndent.Width,this.NodeHeight);
				this.DrawChildren(g);
			}

			g.Transform=orig_trans;
		}
		private void DrawNode(Gdi::Graphics g){
			int nodeheight=this.NodeHeight;

			/* BackColor */
			if(this.BackColor!=Gdi::Color.Transparent)
			using(Gdi::Brush brush=new Gdi::SolidBrush(this.BackColor)){
				float ox=g.Transform.OffsetX;
				_todo.TreeNodeOptimize("BackColor �̎�ނ� Brush �̕����悢�B���̐܂́ABrush �̊Ǘ��͕ʂ̏��ŁB");
				g.FillRectangle(brush,-ox,0,ox+g.ClipBounds.Right,this.NodeHeight);
			}

			Gdi2::Matrix draw_pivot=g.Transform.Clone();

			/* IndentArea */
			if(this.parent!=null){
				this.parent.ChildIndent.DrawIndentArea(g,this,nodeheight);

				TreeNode ascen=parent;
				while(ascen.parent!=null){
					g.TranslateTransform(-ascen.ChildIndent.Width,0);
					ascen.parent.ChildIndent.DrawIndentForDescendant(g,ascen,nodeheight);
					ascen=ascen.parent;
				}

				// ���ɖ߂�
				if(ascen!=parent)g.Transform=draw_pivot;
			}
			
			/* CheckBox */
			if(this.CheckBoxVisible){
				ITreeNodeCheckBox box=this.CheckBox;
				g.TranslateTransform(PADDING_CHKBOX_LEFT,(nodeheight-box.Size.Height+1)/2);

				box.DrawBox(g,this.IsChecked,this.CheckBoxEnabled);

				draw_pivot.Translate(box.Size.Width+PADDING_CHKBOX_LEFT+PADDING_CHKBOX_RIGHT,0);
				g.Transform=draw_pivot;
			}

			/* Icon */
			if(this.IconVisible){
				Gdi::Size szIcon=this.IconSize;
				int y=(nodeheight-szIcon.Height+1/*�l�̌ܓ�*/)/2;
				Gdi::Rectangle rect=new Gdi::Rectangle(PADDING_ICON_LEFT,y,szIcon.Width,szIcon.Height);
				this.Icon.DrawIcon(g,rect,this);

				draw_pivot.Translate(szIcon.Width+PADDING_ICON_LEFT+PADDING_ICON_RIGHT,0);
				g.Transform=draw_pivot;
			}

			/* Content */{
				Gdi::Size sz=this.ContentSize;
				g.TranslateTransform(PADDING_CONTENT_LEFT,(nodeheight-sz.Height+1)/2);

				this.DrawContent(g);

				draw_pivot.Translate(PADDING_CONTENT_LEFT+sz.Width,0);
				g.Transform=draw_pivot;
			}
		}
		internal void DrawChildren(Gdi::Graphics g){
			if(!this.HasChildren)return;

			int start,end;
			// ��ɔ͈͂��i��
			{
				int iM=this.Nodes.Count; // Nodes �̌Ăяo���Ŏq�v�f��������
				int tM=(int)g.ClipBounds.Top;

				//-- start = min { i | nodes[i].bottom>=clipBounds.top}
				//-- node_t = node[start].top
				int node_t=0,node_b=0;
				int i=0;
				for(;i<iM;i++){
					node_t=node_b;
					node_b+=nodes[i].DisplayHeight;

					if(node_b>=tM)break;
				}
				start=i;

				tM=(int)g.ClipBounds.Bottom;
				g.TranslateTransform(0,node_t);

				// end = min{ i | nodes[i].top>clipBounds.bottom}
				while(node_t<=tM&&i<iM){
					node_t+=nodes[i++].DisplayHeight;
				}
				end=i;
			}

			for(int index=start;index<end;index++){
				TreeNode node=this.nodes[index];
				if(!node.IsVisible)continue;

				// �w�i���������S�̂ɘi���Ă���̂Ō��Ǖ`�悵�Ȃ���΂Ȃ�Ȃ��B
				/*
				Gdi::RectangleF rect=new Gdi::RectangleF(Gdi::PointF.Empty,node.DisplaySize);

				_todo.OptimizeTreeNode("��������̍�����");
				if(rect.IntersectsWith(g.ClipBounds))
					node.DrawTo(g);
				//*/

				node.DrawTo(g,true);
				g.TranslateTransform(0,node.DisplayHeight);
			}

		}
		//============================================================
		//		�ĕ`��v��
		//============================================================
		/// <summary>
		/// ���� TreeNode �������I�ɍĕ`�悳���܂��B
		/// </summary>
		protected internal void ReDraw(bool draw_child){
			_todo.TreeNodeOptimize("���������ĕ`�悷��ꍇ�ƁA�q�����`�悷��ꍇ");
			if(this.view==null)
				throw new TreeViewMissingException("�`���� TreeView �������̂ŕ`��o���܂���B");

			if(!this.IsVisibleInTree)return;

			Gdi::Point pDst=this.ClientPosition;
			Gdi::Rectangle rect=new Gdi::Rectangle(pDst,this.DisplaySize);

			// �������Ȃ��� DisplaySize ���ύX�����O�ɕ\�����Ă������݂̕������c������\���c
			// ��̓I�ɂ� DisplaySize.Width ���k�񂾎��ɁA�k�ޑO�ɕ\�����Ă��������������Ȃ���΂Ȃ�Ȃ��B
			rect.X=0;
			rect.Width=this.view.ClientRectangle.Width;

			//if(!this.view.ClientRectangle.IntersectsWith(rect))return;
			rect.Intersect(this.view.ClientRectangle);
			if(rect.IsEmpty)return;

			_todo.ExamineTreeView("�ʒu����肵�A�\���͈͓��ɂ�������`��");
			using(Gdi::Graphics g=this.view.CreateGraphics()){
				g.SetClip(rect);
				g.TranslateTransform(pDst.X,pDst.Y);
//				g.Clear(this.BackColor);
				this.DrawTo(g,draw_child);
			}
		}
		/// <summary>
		/// �؍\���̒��ŕ`�悳��邩�ۂ����擾���܂��B
		/// �S�Ă̐e�� IsVisible ���� IsExpanded �ŁA�������g�� IsVisible �̎��� true �̒l�������܂��B
		/// ����ȊO�̏ꍇ�� false �̒l�ɂȂ�܂��B
		/// </summary>
		private bool IsVisibleInTree{
			get{
				if(!this.IsVisible)return false;
				TreeNode descen=this.parent;
				while(descen!=null){
					if(!descen.IsExpanded||!descen.IsVisible)return false;
					descen=descen.parent;
				}
				return true;
			}
		}
		//============================================================
		//		�{�̂̕`��
		//============================================================
		/// <summary>
		/// �m�[�h�ŗL�̓��e��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		protected virtual void DrawContent(Gdi::Graphics g){
			this.DrawContentBackground(g);

			Gdi::Brush brush;
			if(this.IsEnabled){
				brush=this.IsActivated?Gdi::Brushes.White:Gdi::Brushes.Black;
			}else{
				brush=this.IsActivated?Gdi::Brushes.Gray:Gdi::Brushes.Silver;
			}
			
			g.DrawString("<TreeNode>",this.Font,brush,new Gdi::PointF(0,0));
		}
		/// <summary>
		/// �m�[�h�{�̂̕W���̔w�i��`�悵�܂��B
		/// �W���̔w�i���g�p�������ꍇ�ɂ� DrawContent �̐擪�ŌĂяo���ĉ������B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		protected void DrawContentBackground(Gdi::Graphics g){
			Gdi::Rectangle rect=new Gdi::Rectangle(Gdi::Point.Empty,this.ContentSize);

			if(this.IsFocused){
				if(this.IsActivated){
					afh.Drawing.GraphicsUtils.FillRectangleReverseDotFramed(
						g,
						this.IsEnabled?Gdi::SystemColors.Highlight:Gdi::Color.LightGray,
						rect);
				}else{
					afh.Drawing.GraphicsUtils.DrawRectangleReverseDotFramed(g,this.BackColor,rect);
				}
			}else if(this.IsActivated){
				g.FillRectangle(
					this.IsEnabled?Gdi::SystemBrushes.Highlight:Gdi::Brushes.LightGray,
					rect);
			}
		}
		#endregion

		#region logics: �􉽏���
		//************************************************************
		//		�m�[�h�􉽏���
		//============================================================
		//		�m�[�h�̑傫��
		//============================================================
		private const int PADDING_CHKBOX_LEFT=1;
		private const int PADDING_CHKBOX_RIGHT=2;
		private const int PADDING_ICON_LEFT=1;
		private const int PADDING_ICON_RIGHT=2;
		private const int PADDING_CONTENT_LEFT=0;
		//------------------------------------------------------------
		/// <summary>
		/// �q�m�[�h�̕\���̈���܂߂��\���T�C�Y���擾���܂��B
		/// </summary>
		internal Gdi::Size DisplaySize{
			get{return new Gdi::Size(this.DisplayWidth,this.DisplayHeight);}
		}
		internal int DisplayWidth{
			get{
				//_todo.OptimizeTreeNode("cache ����d�g��");
				int ret=this.NodeWidth;
				if(this.HasChildren&&this.IsExpanded){
					int c=this.Nodes.TotalWidth;
					if(c>ret)ret=c;
				}
				return ret;
			}
		}
		internal int DisplayHeight{
			get{
				//_todo.OptimizeTreeNode("cache ����d�g��");
				return this.szDisplay.Height;
			}
		}
		Gdi::Size szDisplay;
		//-------------------------------------------------------------------------
		/// <summary>
		/// DisplaySize �̍Čv�Z�𑣂��܂��B
		/// </summary>
		/// <returns>
		/// �傫���ɕύX���������ꍇ�� true ��Ԃ��܂��B
		/// �傫���ɕύX���Ȃ������ꍇ�� false ��Ԃ��܂��B
		/// </returns>
		/// <remarks>
		/// ���ɁAtrue ���Ԃ��ꂽ�ꍇ (�傫���ɕύX���������ꍇ) �ɂ́A�`��͏�ʃm�[�h���S�����܂��B
		/// �]���ĕ`����Ăяo�����ł���K�v�͂���܂���B
		/// ����ŁAfalse ���Ԃ��ꂽ�ꍇ (�傫���ɕύX���Ȃ������ꍇ) �̍ĕ`��͌Ăяo�����Ŏ��s���ĉ������B
		/// </remarks>
		protected internal bool RefreshDisplaySize(){
			bool ischanged=this.recalcDisplaySize();
			if(ischanged){
				_todo.TreeNodeDisplayHeight("�e�ɒʒm");
				if(this.parent==null){
					if(this.view!=null)
						// root node �̎�
						//this.view.DrawNodes();
						this.view.Refresh();
				}else if(!this.parent.RefreshDisplaySize()){
					if(this.view!=null)this.ReDraw(true);
				}
			}
			return ischanged;
		}
		private bool recalcDisplaySize(){
			_todo.TreeNodeDisplayHeight("�ω��� incremental �ɍX�V����d�g��");
			// OK> IsVisibleChanged
			// OK> IsExpandedChanged
			// NodeSizeChanged
			//  + IconSize
			//  + CheckBoxSize
			//  + ContentSize
			// [nodes] TotalSizeChanged
			//  + OK> Add/Remove
			//  + OK> [child] DisplaySizeChanged
			// [�q���ɘi���Ă̍X�V]
			//  + View �� NodeParams ���ύX���ꂽ��
			//  + OK> �ǉ����ꂽ��
			Gdi::Size sz=this.szDisplay;

			if(!this.IsVisible){
				this.szDisplay.Width=0;
				this.szDisplay.Height=0;
			}else{
				int h=this.NodeHeight;
				if(this.HasChildren&&this.IsExpanded)
					h+=this.Nodes.TotalHeight;
				this.szDisplay.Height=h;

				int w=this.NodeWidth;
				if(this.HasChildren&&this.IsExpanded){
					int c=this.Nodes.TotalWidth;
					if(c>w)w=c;
				}
				this.szDisplay.Width=w;
			}

			bool ischanged=sz!=szDisplay;
			if(ischanged){
				// �e�ւ̒ʒm����ɔV������Ă����Ȃ��ƁA
				// �X�N���[���o�[�̏o���Ȃǂōĕ`�悷��H�ڂɂȂ�B
				// (�֐��𕪂��Ă��܂����̂ł͍��ł͂������邵���Ȃ�)
				this.OnDisplaySizeChanged(sz);
			}
			return ischanged;
		}
		//-------------------------------------------------------------------------
		/// <summary>
		/// �q�����܂߂đS�Ă� DisplaySize ���X�V���܂��B
		/// �q���ɉe������l�Ȋ�{�ݒ��ύX�����ꍇ�ȂǂɌĂяo���܂��B
		/// </summary>
		protected internal bool RefreshDisplaySizeAll(){
			bool ischanged=this.recalcDisplaySizeAll();
			if(ischanged){
				_todo.TreeNodeDisplayHeight("�e�ɒʒm");
				if(this.parent==null){
					if(this.view!=null)
						// root node �̎�
						this.view.Refresh();
				}else if(!this.parent.RefreshDisplaySize()){
					if(this.view!=null)this.ReDraw(true);
				}
			}
			return ischanged;
		}
		private bool recalcDisplaySizeAll(){
			bool changed=false;

			// ��䢂ł� Node �Ăяo���ɂ���Ď�X�� InitializeNodes �����s����K�v�͂Ȃ�
			if(this.nodes!=null)foreach(TreeNode node in this.nodes){
				if(node.recalcDisplaySizeAll())
					changed=true;
			}

			if(this.recalcDisplaySize())changed=true;

			return changed;
		}
		//-------------------------------------------------------------------------
		/// <summary>
		/// �V�����ǉ����ꂽ�q�m�[�h�̃T�C�Y�Čv�Z��Z�߂Ď��s���A
		/// �Ō�ɑS�̂̍ĕ`������s���܂��B
		/// </summary>
		/// <returns>�ĕ`�悪���s���ꂽ���ۂ���Ԃ��܂��B</returns>
		protected internal bool RefreshDisplaySizeNewChildren(Gen::IEnumerable<TreeNode> list){
			int c=0;
			foreach(TreeNode node in list){
				if(node==this)
					c++;
				else if(node.parent!=this)
					continue;
				else{
					c++;
					node.recalcDisplaySizeAll();
				}
			}
			if(c==0)return false;

			bool fDrawn=false;
			if(this.recalcDisplaySize())
				fDrawn=this.parent!=null&&this.parent.RefreshDisplaySize();
			if(!fDrawn&&this.view!=null)this.ReDraw(true);

			return true;
		}
		//-------------------------------------------------------------------------
		// event: DisplaySizeChanged �� [pre]
		private void OnDisplaySizeChanged(Gdi::Size szOld){
			_todo.TreeNode("�ω��������R��ʒm�ł���悤�ɂ���B�Ⴆ�� expanded, visible-changed ...");
			this.OnDisplaySizeChanged(
				new TreeNodePropertyChangingEventArgs<System.Drawing.Size>(szOld,this.szDisplay)
				);
		}
		//------------------------------------------------------------
		/// <summary>
		/// ���̃m�[�h�̓��e��\������̈�̑傫�����擾���܂��B
		/// </summary>
		public virtual Gdi::Size ContentSize{
			get{return new Gdi::Size(100,12);}
		}
		/// <summary>
		/// ���̃m�[�h�̓��e������\������ʒu���擾���܂��B
		/// </summary>
		public Gdi::Point ContentOffset{
			get{return new Gdi::Point(this.ContentOffsetX,this.ContentOffsetY);}
		}
		/// <summary>
		/// ���̃m�[�h�̓��e�����̍��]�����擾���܂��B
		/// </summary>
		public int ContentOffsetX{
			get{
				int ret=PADDING_CONTENT_LEFT;
				if(this.CheckBoxVisible)
					ret+=PADDING_CHKBOX_LEFT
						+PADDING_CHKBOX_RIGHT
						+this.CheckBox.Size.Width;
				if(this.IconVisible)
					ret+=PADDING_ICON_LEFT
						+PADDING_ICON_RIGHT
						+this.IconSize.Width;
				return ret;
			}
		}
		/// <summary>
		/// ���̃m�[�h�̓��e�����̏�]�����擾���܂��B
		/// </summary>
		public int ContentOffsetY{
			get{return 0;}
		}
		//------------------------------------------------------------
		/// <summary>
		/// ���̃m�[�h�̕\���̈�̑傫�����擾���܂��B
		/// �q�m�[�h�̗̈�̑傫���͊܂݂܂���B
		/// </summary>
		public Gdi::Size NodeSize{
			get{return new Gdi::Size(this.NodeWidth,this.NodeHeight);}
		}
		/// <summary>
		/// ���̃m�[�h�̕����擾���܂��B
		/// �m�[�h�̕��� ContentSize �Ɉˑ����đ傫���Ȃ����菬�����Ȃ����肵�܂��B
		/// </summary>
		public int NodeWidth{
			get{
				int ret=PADDING_CONTENT_LEFT
					+this.ContentSize.Width;
				if(this.CheckBoxVisible)
					ret+=PADDING_CHKBOX_LEFT
						+PADDING_CHKBOX_RIGHT
						+this.CheckBox.Size.Width;
				if(this.IconVisible)
					ret+=PADDING_ICON_LEFT
						+PADDING_ICON_RIGHT
						+this.IconSize.Width;
				return ret;
			}
		}
		// NodeHeight -> [pre]
		//------------------------------------------------------------
		// ChildIndent -> [pre]
		//============================================================
		//		�m�[�h�̋Ǐ����W
		//============================================================
		/// <summary>
		/// ���̃m�[�h�̘_���I�ʒu���擾���܂��B
		/// </summary>
		public Gdi::Point LogicalPosition{
			get{
				if(this.view==null)
					throw new TreeViewMissingException();

				TreeNode node=this;
				Gdi::Point p=Gdi::Point.Empty;
				while(node.parent!=null){
					p=node.LocalPos2ParentLocalPos(p);
					node=node.parent;
				}
				Diag::Debug.Assert(node==this.view.root);
				return p;
			}
		}
		/// <summary>
		/// ���̃m�[�h�̃N���C�A���g���W�ʒu���擾���܂��B
		/// </summary>
		public Gdi::Point ClientPosition{
			get{
				if(this.view==null)
					throw new TreeViewMissingException();
				return this.view.LogicalPos2ClientPos(this.LogicalPosition);
			}
		}
		/// <summary>
		/// ���m�[�h�̍��W�n�ŕ\���ꂽ�ʒu���A�e�m�[�h�̍��W�n�ŕ\�����܂��B
		/// </summary>
		/// <param name="pos">���̃m�[�h�̍��W�n�ŕ\�����ꂽ�ʒu���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu��e�m�[�h�̍��W�n�ŕ\�������l��Ԃ��܂��B</returns>
		public Gdi::Point LocalPos2ParentLocalPos(Gdi::Point pos){
			if(this.parent==null)
				throw new TreeNodeMissingParentException();

			// _todo.TreeNode_Visible("�\�����Ă��镨�ɂ��Ă���������ώZ");
			// �\�����Ă��Ȃ����� DisplayHeight �� 0 �ɂ���

			TreeNodeCollection c=this.parent.Nodes;

			Gdi::Point ret=new Gdi::Point(pos.X+this.parent.ChildIndent.Width,pos.Y+this.parent.NodeHeight);
			for(int i=0,iM=this.IndexInSiblings;i<iM;i++){
				ret.Y+=c[i].DisplayHeight;
			}
			return ret;
		}
		/// <summary>
		/// TreeView �̕\�����W�n���炱�̃m�[�h�̋Ǐ����W�n�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="pos">TreeView �̕\�����W�n�ŕ\���ꂽ�_���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�����̃m�[�h�̋Ǐ����W�n�ŕ\�����l��Ԃ��܂��B</returns>
		public Gdi::Point ClientPos2LocalPos(Gdi::Point pos){
			Gdi::Point orig=this.ClientPosition;
			return new Gdi::Point(pos.X-orig.X,pos.Y-orig.Y);
		}
		/// <summary>
		/// TreeView �̘_�����W�n���炱�̃m�[�h�̋Ǐ����W�n�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="pos">TreeView �̘_�����W�n�ŕ\���ꂽ�_���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�����̃m�[�h�̋Ǐ����W�n�ŕ\�����l��Ԃ��܂��B</returns>
		public Gdi::Point LogicalPos2LocalPos(Gdi::Point pos){
			Gdi::Point orig=this.LogicalPosition;
			return new Gdi::Point(pos.X-orig.X,pos.Y-orig.Y);
		}
		/// <summary>
		/// ���̃m�[�h�̋Ǐ����W�n���� TreeView �̕\�����W�n�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="pos">���̃m�[�h�̋Ǐ����W�n�ŕ\���ꂽ�_���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�� TreeView �̕\�����W�n�ŕ\�����l��Ԃ��܂��B</returns>
		public Gdi::Point LocalPos2ClientPos(Gdi::Point pos){
			Gdi::Point orig=this.ClientPosition;
			return new Gdi::Point(pos.X+orig.X,pos.Y+orig.Y);
		}
		/// <summary>
		/// ���̃m�[�h�̋Ǐ����W�n���� TreeView �̘_�����W�n�ɕϊ����s���܂��B
		/// </summary>
		/// <param name="pos">���̃m�[�h�̋Ǐ����W�n�ŕ\���ꂽ�_���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�� TreeView �̘_�����W�n�ŕ\�����l��Ԃ��܂��B</returns>
		public Gdi::Point LocalPos2LogicalPos(Gdi::Point pos){
			Gdi::Point orig=this.LogicalPosition;
			return new Gdi::Point(pos.X+orig.X,pos.Y+orig.Y);
		}
		//============================================================
		//		�����蔻��
		//============================================================
		/// <summary>
		/// �w�肵�����W�ʒu�ɑ��݂���q���m�[�h���͎��m�[�h���擾���܂��B
		/// </summary>
		/// <param name="p">�m�[�h������������W�ʒu���w�肵�܂��B</param>
		/// <param name="type">�w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A
		/// �w�肵���ʒu����̓I�ɂ��̃m�[�h�̉����ɓ����邩��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�ɂ� 0 ��Ԃ��܂��B</param>
		/// <returns>�w�肵���ʒu�Ƀm�[�h�����݂����ꍇ�ɂ́A���̃m�[�h��Ԃ��܂��B
		/// �m�[�h�����݂����Ȃ������ꍇ�ɂ́Anull ��Ԃ��܂��B</returns>
		public TreeNode LocalPosition2Node(Gdi::Point p,out TreeNodeHitType type){
			const TreeNodeHitType vertical_hit
				=TreeNodeHitType.BorderTop|TreeNodeHitType.Middle|TreeNodeHitType.BorderBottom;

			type=this.HitLocalPosition(p);
			if(0!=(vertical_hit&type))return this;
			p.Y-=this.NodeHeight;


			//-- �q�m�[�h�̔���
			if(this.IsExpanded){
				p.X-=this.ChildIndent.Width;
				if(this.HasChildren)foreach(TreeNode node in this.Nodes){
					if(!node.IsVisible)continue;

					TreeNode ret=node.LocalPosition2Node(p,out type);
					if(ret!=null)return ret;
					p.Y-=node.DisplayHeight;
				}
			}

			//-- ������Ȃ�������
			type=0;
			return null;
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="p">�m�[�h�̍�������_�Ƃ������W���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ����A
		/// �w�肵���ʒu�����̃m�[�h�ɂƂ��Ăǂ̗l�Ȉʒu�ł��邩�̏���Ԃ��܂��B</returns>
		internal TreeNodeHitType HitLocalPosition(Gdi::Point p){
			TreeNodeHitType type=0;
			int height=this.NodeHeight;
			if(height==0){
				type|=p.Y<0?TreeNodeHitType.Above:TreeNodeHitType.Below;
				return type;
			}

			// �c�ʒu�̔���
			float y=p.Y*4f/height;
			if(y<0)
				type|=TreeNodeHitType.Above;
			else if(y<1)
				type|=TreeNodeHitType.BorderTop;
			else if(y<=3)
				type|=TreeNodeHitType.Middle;
			else if(y<=4)
				type|=TreeNodeHitType.BorderBottom;
			else
				type|=TreeNodeHitType.Below;
#if OLD
			// ���ʒu�̔���
			do{
				if(this.parent!=null&&this.parent.ChildIndent+p.X<0) {
					type|=TreeNodeHitType.PaddingLeft;
					break;
				}
				if(p.X<0){
					type|=TreeNodeHitType.PlusMinus;
					break;
				}
				if(this.CheckBoxVisible){
					// CheckBox ����
					p.X-=PADDING_CHKBOX_LEFT;
					Gdi::Size sz=this.CheckBox.Size;
					if(new Gdi::Rectangle(0,(height-sz.Height+1)/2,sz.Width,sz.Height).Contains(p))
						type|=TreeNodeHitType.OnCheckBox;
					p.X-=sz.Width+PADDING_CHKBOX_RIGHT;

					// CheckBox �̈�
					if(p.X<0){
						type|=TreeNodeHitType.CheckBox;
						break;
					}
				}
				if(this.IconVisible){
					// Icon ����
					p.X-=PADDING_ICON_LEFT;
					Gdi::Size sz=this.IconSize;
					if(new Gdi::Rectangle(0,(height-sz.Height+1)/2,sz.Width,sz.Height).Contains(p))
						type|=TreeNodeHitType.OnIcon;
					p.X-=sz.Width+PADDING_CHKBOX_RIGHT;

					// Icon �̈�
					if(p.X<0){
						type|=TreeNodeHitType.Icon;
						break;
					}
				}
				if(p.X<=this.Width)
					type|=TreeNodeHitType.Content;
				else
					type|=TreeNodeHitType.PaddingRight;
			}while(false);
#else
			_todo.TreeNode("PlusMinus �ڍ�");

			// ���ʒu�̔���
			Gdi::Size sz=default(Gdi::Size);
			do{
				int x=p.X;
				if(this.parent!=null&&this.parent.ChildIndent.Width+x<0){
					type|=TreeNodeHitType.PaddingLeft;
					break;
				}
				if(x<0){
					type|=TreeNodeHitType.IndentArea;
					if(this.parent.ChildIndent.HitPlusMinus(x,p.Y,height,this))
						type|=TreeNodeHitType.OnPlusMinus;
					break;
				}
				if(this.CheckBoxVisible){
					sz=this.CheckBox.Size;
					p.X=x-=PADDING_CHKBOX_LEFT; // ���㔻��p
					x-=sz.Width+PADDING_CHKBOX_RIGHT;

					// CheckBox �̈�
					if(x<0){
						type|=TreeNodeHitType.CheckBox;
						break;
					}
				}
				if(this.IconVisible){
					sz=this.IconSize;
					p.X=x-=PADDING_ICON_LEFT; // ���㔻��p
					x-=sz.Width+PADDING_CHKBOX_RIGHT;

					// Icon �̈�
					if(x<0){
						type|=TreeNodeHitType.Icon;
						break;
					}
				}
				sz=this.ContentSize;
				p.X=x-=PADDING_CONTENT_LEFT; // ���㔻��p
				if(x<=this.ContentSize.Width){
					type|=TreeNodeHitType.Content;
					break;
				}

				type|=TreeNodeHitType.PaddingRight;
			}while(false);

			_todo.ExamineTreeView("HitType.On*** �̔���");
			// �����蔻��
			if(
				0!=(type&TreeNodeHitType.mask_vertical_hit)
				&&0!=(type&TreeNodeHitType.mask_horizontal_pmhit)
			){
				if(0!=(type&TreeNodeHitType.mask_horizontal_hit))type|=TreeNodeHitType.Hit;

				// ���㔻��
				if(new Gdi::Rectangle(new Gdi::Point(0,(height-sz.Height+1)/2),sz).Contains(p))
					type|=(TreeNodeHitType)((int)(type&TreeNodeHitType.mask_horizontal_pmhit)<<16);
			}
#endif

			return type;
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="p">�ʒu�� TreeView �̘_�����W�Ŏw�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ����A
		/// �w�肵���ʒu�����̃m�[�h�ɂƂ��Ăǂ̗l�Ȉʒu�ł��邩�̏���Ԃ��܂��B</returns>
		public TreeNodeHitType HitLogicalPosition(Gdi::Point p){
			Gdi::Point pL=this.LogicalPosition;
			p.X-=pL.X;
			p.Y-=pL.Y;
			return HitLocalPosition(p);
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="p">�ʒu�� TreeView �̃N���C�A���g���W�Ŏw�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�ɂ��̃m�[�h�����݂��Ă��邩�ǂ����A
		/// �w�肵���ʒu�����̃m�[�h�ɂƂ��Ăǂ̗l�Ȉʒu�ł��邩�̏���Ԃ��܂��B</returns>
		public TreeNodeHitType HitClientPosition(Gdi::Point p){
			Gdi::Point pC=this.ClientPosition;
			p.X-=pC.X;
			p.Y-=pC.Y;
			return HitLocalPosition(p);
		}
		#endregion

		#region logics: Relations
		//============================================================
		//		�e�q / ��c�֌W
		//============================================================
		/// <summary>
		/// �w�肵�� TreeNode ������ Node �̎q���ł��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="descen">�q���ł��邩�ۂ���m�肽���v�f���w�肵�܂��B</param>
		/// <returns>�w�肵�� TreeNode ������ TreeNode �̎q���ł������ꍇ�� true ��Ԃ��܂��B
		/// �w�肵�� TreeNode ���������g���������A�y�сA����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		public bool IsDescendant(TreeNode descen){
			while(true){
				if(descen.ParentNode==null)return false;
				descen=descen.ParentNode;
				if(descen==this)return true;
			}
		}
		/// <summary>
		/// ���� TreeNode �̊K�w���x�����擾���͐ݒ肵�܂��B
		/// ������ TreeNode �ɂ������Ă��Ȃ��ꍇ�ɂ� 0 ��Ԃ��܂��B
		/// TreeView �̈�ԏ�̊K�w�ɓo�^����Ă���ꍇ�� 1 ��Ԃ��܂��B
		/// ���� TreeNode �̎q�ł���ꍇ�́A�e�m�[�h�̊K�w���x����� 1 �傫������Ԃ��܂��B
		/// </summary>
		public int Level{
			get{
				int i=0;
				TreeNode ances=this.parent;
				while(ances!=null){
					ances=ances.parent;
					i++;
				}
				return i;
			}
		}
		/// <summary>
		/// ���̃m�[�h�̎��ɕ\������Ă���m�[�h���擾���܂��B
		/// ���ɕ\�������ׂ��m�[�h�����݂��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode NextVisibleInTree{
			get{
				if(this.HasChildren&&this.IsExpanded){
					if(this.Nodes.Count>0)return this.Nodes.FirstNode;
				}

				TreeNode ances=this;
				while(ances.parent!=null){
					TreeNode ret=ances.NextVisibleSibling;
					if(ret!=null)return ret;

					ances=ances.parent;
				}
				
				return null;
			}
		}
		/// <summary>
		/// ���̃m�[�h�̑O�ɕ\������Ă���m�[�h���擾���܂��B
		/// </summary>
		public TreeNode PreviousVisibleInTree{
			get{
				if(this.parent==null)return null;

				TreeNode ret=this.PreviousSibling;
				if(ret==null)return this.parent;

				while(ret.HasChildren&&ret.IsExpanded){
					if(ret.Nodes.Count==0)break;
					ret=ret.Nodes.LastNode;
				}

				return ret;
			}
		}
		//============================================================
		//		�Z��m�[�h
		//============================================================
		/// <summary>
		/// �V���� TreeNode �����̃m�[�h�̒��O�ɑ}�����܂��B
		/// </summary>
		/// <param name="node">�}������ TreeNode ���w�肵�܂��B</param>
		public void InsertBefore(TreeNode node){
			if(this.parent==null)
				throw new TreeNodeMissingParentException("�Z��m�[�h��ǉ����鎖�͏o���܂���B");
			this.parent.Nodes.InsertBefore(this,node);
		}
		/// <summary>
		/// �V���� TreeNode �����̃m�[�h�̒���ɑ}�����܂��B
		/// </summary>
		/// <param name="node">�}������ TreeNode ���w�肵�܂��B</param>
		public void InsertAfter(TreeNode node){
			if(this.parent==null)
				throw new TreeNodeMissingParentException("�Z��m�[�h��ǉ����鎖�͏o���܂���B");
			this.parent.Nodes.InsertAfter(this,node);
		}
		/// <summary>
		/// ���� TreeNode ���Z��̒��ŉ��Ԗڂ̕������擾���܂��B
		/// ���� TreeNode ����ԍŏ��ɂ���ꍇ�ɂ� 0 �ł��B
		/// </summary>
		public int IndexInSiblings{
			get{
				if(this.parent==null)
					throw new TreeNodeMissingParentException("�Z�풆�̔ԍ��͈Ӗ����ׂ��܂���B");
				Diag::Debug.Assert(this.parent.nodes!=null&&this.parent.nodes.Contains(this));
				return this.parent.nodes.IndexOf(this);
			}
		}
		/// <summary>
		/// ���̃m�[�h�̎��̌Z��m�[�h���擾���܂��B
		/// ����ɌZ��m�[�h�����݂��Ă��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode NextSibling{
			get{
				int i=this.IndexInSiblings+1;
				if(i>=this.parent.nodes.Count)return null;
				return this.parent.nodes[i];
			}
		}
		/// <summary>
		/// ���̃m�[�h�̎��̌�����Z��m�[�h���擾���܂��B
		/// ���̃m�[�h����ɁA������Z��m�[�h�����݂��Ă��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode NextVisibleSibling{
			get{
				int i=this.IndexInSiblings+1;
				while(i<this.parent.nodes.Count){
					TreeNode ret=parent.nodes[i];
					if(ret.IsVisible)return ret;
				}
				return null;
			}
		}
		/// <summary>
		/// ���̃m�[�h�̒��O�̌Z��m�[�h���擾���܂��B
		/// ���O�ɌZ��m�[�h�����݂��Ă��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode PreviousSibling{
			get{
				int i=this.IndexInSiblings-1;
				if(i<0)return null;
				return this.parent.nodes[i];
			}
		}
		/// <summary>
		/// ���̃m�[�h�̑O�̌�����Z��m�[�h���擾���܂��B
		/// ���̃m�[�h���O�Ɍ�����Z��m�[�h�����݂��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode PreviousVisibleSibling{
			get{
				int i=this.IndexInSiblings-1;
				while(i>=0){
					TreeNode ret=this.parent.nodes[i];
					if(ret.IsVisible)return ret;
				}
				return null;
			}
		}

		/// <summary>
		/// ���̗v�f���A�e�v�f�̍Ō�̎q�m�[�h�ł��邩�ۂ����擾���܂��B
		/// �e�v�f�����݂��Ȃ��ꍇ�ɂ� false ��Ԃ��܂��B
		/// </summary>
		public bool IsLastChild{
			get{return this.parent!=null&&this.parent.Nodes.LastNode==this;}
		}
		/// <summary>
		/// ���̗v�f���A�e�v�f�̍ŏ��̎q�m�[�h�ł��邩�ۂ����擾���܂��B
		/// �e�v�f�����݂��Ȃ��ꍇ�ɂ� false ��Ԃ��܂��B
		/// </summary>
		public bool IsFirstChild{
			get{return this.parent!=null&&this.parent.Nodes.LastNode==this;}
		}
		#endregion

		#region logics: TreeNode ���
		//************************************************************
		//		��ԐF�X
		//============================================================
		//		Icon �\��
		//============================================================
		// IsIconVisible �� [pre]
		// IconSize �� [pre]
		//============================================================
		//		Focus ���
		//============================================================
		// AcquiredFocus �� [pre]
		// LosingFocus �� [pre]
		/// <summary>
		/// ���̃m�[�h�� TreeView �̒��Œ��ڂ���Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool IsFocused{
			get{return this.view!=null&&this==this.view.FocusedNode;}
		}
		/// <summary>
		/// ���̃m�[�h�� TreeView ���� Focus ���ړ����܂��B
		/// </summary>
		public void SetFocus(){
			if(this.view==null)
				throw new TreeViewMissingException("Focus �͐e TreeView �����݂��Ȃ��ꍇ�ɂ͈Ӗ����ׂ��܂���B");

			this.view.FocusedNode=this;
		}
		/// <summary>
		/// ���̃m�[�h���e TreeView ���őI������Ă��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		public bool IsSelected{
			get{return this.view!=null&&this.view.SelectedNodes.Contains(this);}
		}
		/// <summary>
		/// ���̃m�[�h��I��������Ԃɂ��܂��B
		/// </summary>
		public void SetSelected(){
			if(this.view==null)
				throw new TreeViewMissingException("�e TreeView �����݂��Ȃ��̂ŁA�I����Ԃɂ��鎖���o���܂���B");
			this.view.SelectedNodes.Set(this);
		}
		/// <summary>
		/// ���̃m�[�h��������������Ԃɂ��邩�ۂ����擾���܂��B
		/// �e TreeView ���Ȃ��ꍇ�ɂ͊�����������Ԃɂׂ͈�܂���B
		/// �e TreeView �Ń}�E�X�̃{�^�����~��Ă���ꍇ�ɂ́A
		/// �}�E�X�̃{�^�����~�肽���ɉ��ɂ����� TreeNode ��������������Ԃɂ���܂��B
		/// ����ȊO�̏ꍇ�ɂ͌��ݑI������Ă��� TreeNode ��������������Ԃɂ���܂��B
		/// </summary>
		public bool IsActivated{
			get{return this.view!=null&&this.view.IsActivated(this);}
		}
		//============================================================
		//		Enabled ���
		//============================================================
		// IsEnabled �� [pre]
		// IsEnabledChanged �� [pre]
		/// <summary>
		/// IsEnabled ���ω������ۂ̏��������s���܂��B
		/// </summary>
		/// <param name="changedTop">
		/// ���̃m�[�h���N�_�Ƃ��� IsEnabled ��Ԃ̕ω��̏ꍇ�� true ���w�肵�܂��B
		/// �e����̓`�B�ɂ���ĕω����Ă���ꍇ�� false ���w�肵�܂��B
		/// </param>
		/// <param name="e">�ω�����l�ɂ��Ă̏���񋟂��܂��B</param>
		private void OnIsEnabledChanged(bool changedTop,TreeNodePropertyChangingEventArgs<bool> e){
			if(this.HasChildren)foreach(TreeNode child in this.Nodes){
				if(child.IsEnabledInherit==TreeNodeInheritType.Inherit)
					child.OnIsEnabledChanged(false,e);
			}
			this.OnIsEnabledChanged(e);
			if(changedTop)this.ReDraw(true);
		}
		//============================================================
		//		Expand ���
		//============================================================
		/// <summary>
		/// �S�Ă̎q���̊J��Ԃ�ݒ肵�܂��B
		/// </summary>
		/// <param name="expanded">�ݒ肷���Ԃ��w�肵�܂��B
		/// �S�Ă̎q����W�J����ꍇ�ɂ� true ���w�肵�܂��B
		/// �S�Ă̎q����܂��ޏꍇ�ɂ� false ���w�肵�܂��B</param>
		public void ExpandAllDescendant(bool expanded){
			this.IsExpanded=expanded;
			if(this.HasChildren)
				this.Nodes.ExpandAllDescendant(expanded);
		}
		/// <summary>
		/// ���̃m�[�h�����݊J������ԂɂȂ��Ă��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		public bool IsExpanded{
			get{return this.bits[bExpanded];}
			set{
				if(value==this.bits[bExpanded])return;

				TreeNodePropertyChangingEventArgs<bool> e=new TreeNodePropertyChangingEventArgs<bool>(!value,value);
				afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> m;
				if(this.xmem.GetMember("BeforeIsExpandedChange",out m))m(this,e);

				_todo.TreeNodeDisplayHeight("�g�k�\���X�V");
				this.bits[bExpanded]=value;
				this.RefreshDisplaySize();

				if(this.xmem.GetMember("AfterIsExpandedChange",out m))m(this,e);
			}
		}
		/// <summary>
		/// ���̃m�[�h�̓W�J��Ԃ��ω�����O�ɔ�������C�x���g�ł��B
		/// </summary>
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> AfterIsExpandedChange{
			add{this.xmem.AddHandler("AfterIsExpandedChange",value);}
			remove{this.xmem.RemoveHandler("AfterIsExpandedChange",value);}
		}
		/// <summary>
		/// ���̃m�[�h�̓W�J��Ԃ��ω��������ɔ�������C�x���g�ł��B
		/// </summary>
		public event afh.EventHandler<TreeNode,TreeNodePropertyChangingEventArgs<bool>> BeforeIsExpandedChange{
			add{this.xmem.AddHandler("BeforeIsExpandedChange",value);}
			remove{this.xmem.RemoveHandler("BeforeIsExpandedChange",value);}
		}
		//============================================================
		//		Visible ���
		//============================================================
		/// <summary>
		/// ���̃m�[�h�� TreeView ���ɕ\������邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		public bool IsVisible{
			get{return this.bits[bVisible];}
			set{
				if(this.bits[bVisible]==value)return;
				this.bits[bVisible]=value;
				this.OnIsVisibleChanged();
			}
		}
		/// <summary>
		/// IsVisible �̒l���ω��������ɔ�������C�x���g�ł��B
		/// </summary>
		public event afh.CallBack<TreeNode> IsVisibleChanged{
			add{this.xmem.AddHandler("IsVisibleChanged",value);}
			remove{this.xmem.RemoveHandler("IsVisibleChanged",value);}
		}
		private void OnIsVisibleChanged(){
			afh.CallBack<TreeNode> m;
			if(this.xmem.GetMember("IsVisibleChanged",out m))m(this);

			_todo.TreeNodeDisplayHeight("�e�m�[�h�ւ̒ʒm / DisplayHeight �̍X�V");
			this.RefreshDisplaySize();
		}
		//============================================================
		//		Check ���
		//============================================================
		/// <summary>
		/// Check ��Ԃ��q���̏�ԂɈˑ����ĕω����邩�ۂ����擾���͐ݒ肵�܂��B
		/// true �̏ꍇ�ɂ͎q��Ԃ��ω�����Ǝ������g�� Checked ��Ԃ��ω����܂��B
		/// �܂��������g�� Checked �v���p�e�B�ɒl��ݒ肷��Ƃ��ꂪ�S�Ă̎q�m�[�h�ɓK�p����܂��B
		/// �A���A���ԏ�� null ��ݒ肵���ꍇ�ɂ́A�q�m�[�h���������g�������ύX���܂���B
		/// </summary>
		public bool CheckedReflectChildren{
			get{return this.bits[bCheckReflectChildren];}
			set{this.bits[bCheckReflectChildren]=value;}
		}
		/// <summary>
		/// CheckBox �̏�Ԃ����ݗL���ł��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		public bool CheckBoxEnabled{
			get{return this.bits[bCheckBoxEnabled];}
			set{this.bits[bCheckBoxEnabled]=value;}
		}
		//------------------------------------------------------------
		/// <summary>
		/// TreeNode �̏�Ԃ��`�F�b�N���ꂽ��ԂɂȂ��Ă��邩�ۂ����擾���͐ݒ肵�܂��B
		/// null �͒��ԏ�Ԃł��鎖�������܂��B
		/// </summary>
		public bool? IsChecked{
			get{
				// MirrorChildren �̏ꍇ
				if(this.CheckedReflectChildren){
					bool fst=true;
					bool c=default(bool);
					if(this.HasChildren)foreach(TreeNode node in this.Nodes){
						bool? v=node.IsChecked;
						if(v==null)return null;
						if(fst){
							c=(bool)v;
							fst=false;
						}else{
							if(c!=(bool)v)return null;
						}
					}
					return fst?(bool?)null: c;
				}
				return CheckState2Bool((TreeNodeCheckedState)this.bits[sCheckedState]);
			}
			set{
				TreeNodeCheckedState oldstat=(TreeNodeCheckedState)this.bits[sCheckedState];

				// MirrorChildren �̏ꍇ
				if(this.CheckedReflectChildren&&value!=null){
					bool v=(bool)value;
					if(this.HasChildren)foreach(TreeNode child in this.Nodes)
						child.IsChecked=v;

					// FALL_THROUGH : �ꉞ�������g���ς��Ă�����
				}

				TreeNodeCheckedState newstat;
				if(value==null){
					newstat=TreeNodeCheckedState.Intermediate;
					//newstat|=TreeNodeCheckedState.Intermediate;
				}else if((bool)value){
					newstat=TreeNodeCheckedState.Checked;
					//newstat&=~TreeNodeCheckedState.Intermediate;
					//newstat|=TreeNodeCheckedState.Checked;
				}else{
					newstat=TreeNodeCheckedState.Unchecked;
					//newstat&=~TreeNodeCheckedState.Intermediate;
					//newstat&=~TreeNodeCheckedState.Checked;
				}

				this.bits[sCheckedState]=(uint)newstat;
				if(oldstat!=newstat){
					this.OnCheckedStateChanged(new CheckedStateEventArgs(
						CheckState2Bool(oldstat),
						CheckState2Bool(newstat)
					));
					this.ReDraw(false);
				}
			}
		}
		private static bool? CheckState2Bool(TreeNodeCheckedState state){
			switch(state){
				case TreeNodeCheckedState.Unchecked:return false;
				case TreeNodeCheckedState.Checked:return true;
				case TreeNodeCheckedState.Intermediate:return null;
				default:
					Diag::Debug.Assert(false);
					return null;
			}
		}
		//------------------------------------------------------------
		/// <summary>
		/// Checked ���� Intermediate ���ω��������ɔ������܂��B
		/// </summary>
		public event afh.EventHandler<TreeNode,CheckedStateEventArgs> CheckedStateChanged{
			add{this.xmem.AddHandler("CheckedStateChanged",value);}
			remove{this.xmem.RemoveHandler("CheckedStateChanged",value);}
		}
		/// <summary>
		/// Checked ���� Intermediate ���ω��������ɌĂяo����܂��B
		/// </summary>
		protected void OnCheckedStateChanged(CheckedStateEventArgs e){
			afh.EventHandler<TreeNode,CheckedStateEventArgs> m;
			if(this.xmem.GetMember("CheckedStateChanged",out m))m(this,e);
		}
		/// <summary>
		/// Check ��Ԃ̕ύX�ɔ����C�x���g�̏ڍׂ�ێ����܂��B
		/// </summary>
		public sealed class CheckedStateEventArgs:System.EventArgs{
			bool? oldState;
			bool? newState;
			internal CheckedStateEventArgs(bool? oldState,bool? newState){
				this.oldState=oldState;
				this.newState=newState;
			}
			/// <summary>
			/// Check ��Ԃ��ύX�����O�̏�Ԃ��擾���܂��B
			/// </summary>
			public bool? OldState{
				get{return this.oldState;}
			}
			/// <summary>
			/// Check ��Ԃ��ύX���ꂽ��� (���݂�) ��Ԃ��擾���܂��B
			/// </summary>
			public bool? NewState{
				get{return this.newState;}
			}
		}
		#endregion
	}

	#region class:TreeNodeCollection
	/// <summary>
	/// TreeNode �̏W�����Ǘ�����N���X�ł��B
	/// </summary>
	public sealed class TreeNodeCollection
		:Gen::IEnumerable<TreeNode>,Gen::ICollection<TreeNode>,System.Collections.IEnumerable,Gen::IList<TreeNode>
	{
		private readonly TreeNode parent;
		private Gen::List<TreeNode> children;

		internal TreeNodeCollection(TreeNode parent){
			this.parent=parent;
			this.children=new Gen::List<TreeNode>();
		}

		internal void ExpandAllDescendant(bool expanded){
			foreach(TreeNode node in this)
				node.ExpandAllDescendant(expanded);
		}
		//============================================================
		//		�R���N�V����
		//============================================================
		/// <summary>
		/// �o�^����Ă���v�f�̐����擾���܂��B
		/// </summary>
		public int Count{
			get{return this.children.Count;}
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ���q�v�f���擾���܂��B
		/// </summary>
		/// <param name="index">�擾������ TreeNode �̈ʒu���w�肵�܂��B</param>
		/// <returns>�w�肵���ʒu�ɂ������q�v�f��Ԃ��܂��B</returns>
		public TreeNode this[int index]{
			get{return this.children[index];}
			set{
				if(index<0||index>=this.children.Count)
					throw new System.ArgumentOutOfRangeException("index");

				if(value==null){
					this.RemoveAt(index);
					return;
				}

				// ���̏ꏊ����폜
				if(value.parent!=null){
					if(value.parent==this.parent){
						// ���g�Ɋ��Ɋ܂܂�Ă����ꍇ���ړ�
						int oldIndex=this.children.IndexOf(value);
						if(index==oldIndex)return; // �ω�����
						if(oldIndex<index)index--;
					}

					value.parent.Nodes.Remove(value);
				}

				// �q���ւ�
				this.children[index].View=null;
				this.children[index].parent=null;
				value.View=this.parent.View;
				value.parent=this.parent;
				this.children[index]=value;

				// �X�V
				//value.ReDraw(true);_todo.TreeNodeDisplayHeight("�K�v��?");
				this.suppress_register(value,true);
			}
		}
		/// <summary>
		/// ���� TreeNodeCollection �Ɋ܂܂�Ă��� TreeNode �̗񋓎q���擾���܂��B
		/// </summary>
		/// <returns>TreeNode �̗񋓎q��Ԃ��܂��B</returns>
		public Gen::IEnumerator<TreeNode> GetEnumerator(){
			return this.children.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
			return this.GetEnumerator();
		}
		/// <summary>
		/// ���� TreeNodeCollection �Ɋ܂܂�Ă���q�m�[�h��S�č폜���܂��B
		/// </summary>
		public void Clear(){
			foreach(TreeNode ch in this.children){
				ch.View=null;
				ch.parent=null;
			}
			this.children.Clear();
		}
		/// <summary>
		/// ���� TreeNodeCollection �̓��e��z��ɏ����o���܂��B
		/// </summary>
		/// <param name="array">�����o����̔z����w�肵�܂��B</param>
		/// <param name="arrayIndex">�w�肵���z��̒��ŁA�����o�����J�n����ʒu���w�肵�܂��B
		/// �w�肵���ʒu�ɂ��̏W���̑��v�f���������܂�܂��B</param>
		public void CopyTo(TreeNode[] array,int arrayIndex){
			if(array==null)throw new System.ArgumentNullException("array");
			if(arrayIndex<0||array.Length-arrayIndex>this.children.Count)
				throw new System.ArgumentOutOfRangeException("arrayIndex");

			foreach(TreeNode ch in this.children)array[arrayIndex++]=ch;
		}
		/// <summary>
		/// ���̏W�����ǂݎ���p�ł��邩�ۂ����擾���͐ݒ肵�܂��B
		/// TreeNodeCollection �̏ꍇ�ɂ͏�� false ��Ԃ��܂��B
		/// </summary>
		public bool IsReadOnly{
			get{return false;}
		}
		//============================================================
		//		��
		//============================================================
		internal int TotalHeight{
			get{
				int ret=0;
				foreach(TreeNode node in this)
					ret+=node.DisplayHeight;
				return ret;
			}
		}
		internal int TotalWidth{
			get{
				int ret=0;
				foreach(TreeNode node in this){
					int c=node.DisplayWidth;
					if(c>ret)ret=c;
				}
				return ret;
			}
		}
		//============================================================
		//		�m�[�h�̌���
		//============================================================
		/// <summary>
		/// �w�肵�� TreeNode ���W���Ɋ܂܂�Ă��邩�ۂ����擾���܂��B
		/// </summary>
		/// <param name="node">�܂܂�Ă��邩�ǂ�����m�肽�� TreeNode ���w�肵�܂��B</param>
		/// <returns>�w�肵�� TreeNode ���܂܂�Ă����ꍇ�� true ��Ԃ��܂��B
		/// �܂܂�Ă��Ȃ������ꍇ�� false ��Ԃ��܂��B</returns>
		public bool Contains(TreeNode node){
			return this.children.Contains(node);
		}
		/// <summary>
		/// �w�肵�� TreeNode ���W�����ŉ��Ԗڂɑ����Ă��邩���擾���܂��B
		/// </summary>
		/// <param name="node">�ԍ���m�肽�� TreeNode ���w�肵�܂��B</param>
		/// <returns>�w�肵�� TreeNode �̔ԍ���Ԃ��܂��B
		/// �w�肵�� TreeNode �����Ɋ܂܂�Ă��Ȃ��ꍇ�ɂ� -1 ��Ԃ��܂��B</returns>
		public int IndexOf(TreeNode node){
			return this.children.IndexOf(node);
		}
		/// <summary>
		/// ���̃m�[�h�W���Ɋ܂܂�� TreeNode �̓��A��ԏ��߂ɓo�^����Ă��镨���擾���܂��B
		/// ���̏W���Ɉ�� TreeNode ���o�^����Ă��Ȃ����ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode FirstNode{
			get{return this.children.Count==0?null: this.children[0];}
		}
		/// <summary>
		/// ���̃m�[�h�W���Ɋ܂܂�� TreeNode �̓��A��ԍŌ�ɓo�^����Ă��镨���擾���܂��B
		/// ���̏W���Ɉ�� TreeNode ���o�^����Ă��Ȃ����ɂ� null ��Ԃ��܂��B
		/// </summary>
		public TreeNode LastNode{
			get{
				int i=this.children.Count-1;
				return i<0?null: this.children[i];
			}
		}
		//============================================================
		//		�m�[�h�̒ǉ��폜
		//============================================================
		/// <summary>
		/// �m�[�h�W����Z�߂Ēǉ����܂��B
		/// </summary>
		/// <param name="collection">�ǉ�����m�[�h�̏W�����w�肵�܂��B</param>
		public void AddRange(Gen::IEnumerable<TreeNode> collection){
			//_todo.TreeNodeDisplayHeight("TreeNodeCollection.Suppress: �m�[�h���X�g�̊􉽍Čv�Z�E�ĕ`���}�~�BResume ���ɕύX���݂�� Refresh");
			using(this.SuppressLayout())
			foreach(TreeNode node in collection){
				this.Add(node);
			}

			//if(!this.parent.RefreshDisplaySize())
			//  this.parent.ReDraw(true);
		}
		/// <summary>
		/// �V�����q�v�f��o�^���܂��B
		/// </summary>
		/// <param name="node">�ǉ����� TreeNode ���w�肵�܂��B</param>
		public void Add(TreeNode node){
			// ���ɒǉ�����Ă��� : ���[�Ɉړ���������?
			//if(node.parent==this.parent)return;

			// ���ɕʂ̏��ɋ��鎞
			if(node.parent!=null)
				node.parent.Nodes.Remove(node);

			node.View=this.parent.View;
			node.parent=this.parent;
			this.children.Add(node);

			this.suppress_register(node);
		}
		/// <summary>
		/// �w�肵���q�v�f���폜���܂��B
		/// �����o�^����Ă���ꍇ�ɂ́A�ŏ��Ɍ��������v�f���폜���܂��B
		/// </summary>
		/// <param name="node">�폜������ TreeNode ���w�肵�܂��B</param>
		/// <returns>�����ɍ폜���ꂽ�ꍇ�� true ��Ԃ��܂��B�w�肵���v�f��������Ȃ������ꍇ�Ȃǂ� false ��Ԃ��܂��B</returns>
		public bool Remove(TreeNode node){
			node.View=null;
			node.parent=null;
			bool ret=this.children.Remove(node);

			this.suppress_register(this.parent);
			return ret;
		}
		/// <summary>
		/// �w�肵���ʒu�ɂ���q�v�f���폜���܂��B
		/// </summary>
		/// <param name="index">�폜���� TreeNode �̈ʒu���w�肵�܂��B</param>
		/// <returns>�폜���ꂽ�v�f��Ԃ��܂��B</returns>
		public TreeNode RemoveAt(int index){
			TreeNode ret=this.children[index];
			this.children.RemoveAt(index);
			
			ret.View=null;
			ret.parent=null;

			this.suppress_register(this.parent);
			return ret;
		}
		void System.Collections.Generic.IList<TreeNode>.RemoveAt(int index){
			this.RemoveAt(index);
		}
		/// <summary>
		/// �w�肵���ʒu�Ɏw�肵���m�[�h��ǉ����܂��B
		/// </summary>
		/// <param name="index">�m�[�h��ǉ�����ꏊ���w�肵�܂��B</param>
		/// <param name="item">�ǉ�����m�[�h���w�肵�܂��B</param>
		public void Insert(int index,TreeNode item){
			if(index<0||index>this.children.Count)
				throw new System.ArgumentOutOfRangeException("index");

			// ���̏ꏊ����폜
			if(item.parent!=null){
				if(item.parent==this.parent){
					// ���g�Ɋ��Ɋ܂܂�Ă����ꍇ���ړ�
					int oldIndex=this.children.IndexOf(item);
					if(index==oldIndex)return; // �ω�����
					if(oldIndex<index)index--;
				}

				item.parent.Nodes.Remove(item);
			}

			item.View=this.parent.View;
			item.parent=this.parent;
			this.children.Insert(index,item);

			this.suppress_register(item);
		}
		/// <summary>
		/// ��m�[�h�̒���Ƀm�[�h��}�����܂��B
		/// </summary>
		/// <param name="pivot">��m�[�h���w�肵�܂��B</param>
		/// <param name="addee">�}������m�[�h���w�肵�܂��B</param>
		public void InsertBefore(TreeNode pivot,TreeNode addee){
			if(pivot==addee)return;

			if(!this.children.Contains(pivot))
				throw new System.ArgumentException("�w�肵�� TreeNode pivot �͂��̃m�[�h�W���Ɋ܂܂�Ă��܂���B","pivot");

			this.Insert(this.children.IndexOf(pivot),addee);
			//if(addee.parent!=null)
			//  addee.parent.Nodes.Remove(addee);
			//int index=this.children.IndexOf(pivot); // remove �Ŕԍ����ς��\������䢂�
			//addee.View=this.parent.View;
			//addee.parent=this.parent;
			//this.children.Insert(index,addee);
			//_todo.TreeNodeDisplayHeight("�}��");
			//if(!addee.RefreshDisplaySizeAll())
			//  this.parent.RefreshDisplaySize();
		}
		/// <summary>
		/// ��m�[�h�̒��O�Ƀm�[�h��}�����܂��B
		/// </summary>
		/// <param name="pivot">��m�[�h���w�肵�܂��B</param>
		/// <param name="addee">�}������m�[�h���w�肵�܂��B</param>
		public void InsertAfter(TreeNode pivot,TreeNode addee){
			if(pivot==addee)return;

			if(!this.children.Contains(pivot))
				throw new System.ArgumentException("�w�肵�� TreeNode pivot �͂��̃m�[�h�W���Ɋ܂܂�Ă��܂���B","pivot");

			this.Insert(this.children.IndexOf(pivot)+1,addee);
			//if(addee.parent!=null)
			//  addee.parent.Nodes.Remove(addee);
			//int index=this.children.IndexOf(pivot); // remove �Ŕԍ����ς��\������䢂�
			//addee.View=this.parent.View;
			//addee.parent=this.parent;
			//this.children.Insert(index+1,addee);
			//_todo.TreeNodeDisplayHeight("�}��");
			//if(!addee.RefreshDisplaySizeAll())
			//  this.parent.RefreshDisplaySize();
		}

		#region SuppressLayout
		readonly object suppress_sync=new object();
		Gen::List<TreeNode> suppress_nodes=null;
		int suppress_count=0;
		/// <summary>
		/// �Ĕz�u�E�ĕ`�揈������U��~���܂��B
		/// </summary>
		/// <returns>��U��~���Ă����������ĊJ����ׂ̃I�u�W�F�N�g��Ԃ��܂��B
		/// �������I��������K��������ĉ������B</returns>
		/// <example>
		/// TreeNode node;
		/// using(node.Nodes.SuppressLayout()){
		///	  // ����...
		/// }
		/// </example>
		public System.IDisposable SuppressLayout(){
			return new suppress_disposable(this);
		}
		/// <summary>
		/// �w�肵���v�f�̃T�C�Y���Čv�Z����K�v�����鎖��ʒm���܂��B
		/// ���m�[�h���w�肵�����ɂ́A���g�̍Čv�Z�����s���܂��B
		/// �q�m�[�h���w�肵�����ɂ́A�q���܂ők���ďڍׂɍČv�Z���܂��B
		/// </summary>
		/// <param name="node">�Čv�Z����m�[�h���w�肵�܂��B</param>
		private void suppress_register(TreeNode node){
			this.suppress_register(node,false);
		}
		/// <summary>
		/// �w�肵���v�f�̃T�C�Y���Čv�Z����K�v�����鎖��ʒm���܂��B
		/// ���m�[�h���w�肵�����ɂ́A���g�̍Čv�Z�����s���܂��B
		/// �q�m�[�h���w�肵�����ɂ́A�q���܂ők���ďڍׂɍČv�Z���܂��B
		/// </summary>
		/// <param name="node">�Čv�Z����m�[�h���w�肵�܂��B</param>
		/// <param name="forceParentRecalc">�e�̍Čv�Z���������邩�ۂ����w�肵�܂��B</param>
		private void suppress_register(TreeNode node,bool forceParentRecalc){
			lock(suppress_sync){
				if(suppress_count==0){
					if(node==parent||!node.RefreshDisplaySizeAll()||forceParentRecalc)
						this.parent.RefreshDisplaySize();
				}else{
					if(suppress_nodes==null)
						suppress_nodes=new Gen::List<TreeNode>();
					suppress_nodes.Add(node);
					if(forceParentRecalc)
						suppress_nodes.Add(this.parent);
				}
			}
		}
		private void suppress_increment(){
			System.Threading.Interlocked.Increment(ref this.suppress_count);
		}
		private void suppress_decrement(){
			if(System.Threading.Interlocked.Decrement(ref this.suppress_count)>0)return;
			lock(this.suppress_sync){
				if(this.suppress_nodes==null)return;

				this.parent.RefreshDisplaySizeNewChildren(suppress_nodes);
				this.suppress_nodes.Clear();
				//bool fRecalcParent=false;
				//foreach(TreeNode node in this.suppress_nodes){
				//  if(node==parent)
				//    fRecalcParent=true;
				//  else if(!node.RefreshDisplaySizeAll())
				//    fRecalcParent=true;
				//}
				//this.suppress_nodes.Clear();
				//if(fRecalcParent)
				//  this.parent.RefreshDisplaySize();
			}
		}
		internal class suppress_disposable:System.IDisposable{
			readonly TreeNodeCollection collection;
			int f_dec=0;
			public suppress_disposable(TreeNodeCollection collection){
				this.collection=collection;
				collection.suppress_increment();
			}
			public void Dispose(){
				if(0==System.Threading.Interlocked.Exchange(ref this.f_dec,1))
					collection.suppress_decrement();
			}
		}
		#endregion
	}
	#endregion

	/// <summary>
	/// �e�m�[�h�����݂��Ă��Ȃ����ɂ���āA�v�����ꂽ��������s�o���Ȃ��ꍇ�ɔ��������O�ł��B
	/// </summary>
	[System.Serializable]
	public sealed class TreeViewMissingException:TreeNodeException{
		const string MSG0="���̃m�[�h�͂ǂ� TreeView �ɂ������Ă��܂���B";
		/// <summary>
		/// TreeViewMissingException �����������܂��B
		/// </summary>
		public TreeViewMissingException():base(MSG0){}
		/// <summary>
		/// TreeViewMissingException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		public TreeViewMissingException(string message):base(MSG0+message){}
		/// <summary>
		/// TreeViewMissingException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		/// <param name="innerException">���̗�O�������N���������ƂȂ������̗�O���w�肵�܂��B</param>
		public TreeViewMissingException(string message,System.Exception innerException)
			:base(MSG0+message,innerException){}
	}
	/// <summary>
	/// �e�m�[�h�����݂��Ă��Ȃ����ɂ���āA�v�����ꂽ��������s�o���Ȃ��ꍇ�ɔ��������O�ł��B
	/// </summary>
	[System.Serializable]
	public sealed class TreeNodeMissingParentException:TreeNodeException{
		const string MSG0="�e�m�[�h�����݂��Ă��܂���B";
		/// <summary>
		/// TreeNodeMissingParentException �����������܂��B
		/// </summary>
		public TreeNodeMissingParentException():base(MSG0){}
		/// <summary>
		/// TreeNodeMissingParentException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		public TreeNodeMissingParentException(string message):base(MSG0+message){}
		/// <summary>
		/// TreeNodeMissingParentException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		/// <param name="innerException">���̗�O�������N���������ƂȂ������̗�O���w�肵�܂��B</param>
		public TreeNodeMissingParentException(string message,System.Exception innerException)
			:base(MSG0+message,innerException){}
	}
	/// <summary>
	/// TreeNode �ɑ΂��Ė����ȑ�������s���悤�Ƃ����ꍇ�ɔ��������O�ł��B
	/// </summary>
	[System.Serializable]
	public class TreeNodeException:System.InvalidOperationException{
		/// <summary>
		/// TreeNodeException �����������܂��B
		/// </summary>
		public TreeNodeException():base(){}
		/// <summary>
		/// TreeNodeException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		public TreeNodeException(string message):base(message){}
		/// <summary>
		/// TreeNodeException �����������܂��B
		/// </summary>
		/// <param name="message">��O�̏ڍׂɊւ��Đ������镶������w�肵�܂��B</param>
		/// <param name="innerException">���̗�O�������N���������ƂȂ������̗�O���w�肵�܂��B</param>
		public TreeNodeException(string message,System.Exception innerException)
			:base(message,innerException){}
	}

	/// <summary>
	/// �]�胁���o���Ǘ�����ׂ̍\���̂ł��B
	/// �w�ǎg����\�����Ȃ������o���R�ێ�����l�ȃN���X�Ɏg�p���܂��B
	/// </summary>
	/// <remarks>
	/// �w�ǎg����\�����Ȃ������o���R���N���X�́A
	/// �𒼂ɐ݌v����Ɩ��ʂɑ傫�ȃT�C�Y�̃f�[�^�Ɉׂ�\��������܂��B
	/// ���̗l�Ȏ��ɂ�����g���ƁA�K�v��������܂Ń����o�����蓖�Ă��A
	/// �K�v����������̈�����蓖�Ă�Ƃ��������\�ɂȂ�܂��B
	/// <para>
	/// ���ꂼ��̃����o�̎��ʂɂ͕�������g�p���܂��B
	/// �����I�ɂ̓n�b�V���e�[�u�����g�p���Ă��܂��̂ŁA�����̃I�[�o�[�w�b�h��������܂��B
	/// �p�ɂɎg�p����\�������郁���o��䢂Ɋi�[����͓̂���ł͂���܂���B
	/// </para>
	/// </remarks>
	[System.Serializable]
	public struct ExtraMemberCache:Ser::ISerializable{
		Gen::Dictionary<string,object> _data;
		private Gen::Dictionary<string,object> data{
			get{return this._data??(this._data=new Gen::Dictionary<string,object>());}
		}
		/// <summary>
		/// �w�肵�����O�Ɋ֘A�t����ꂽ�����o�̒l���擾���܂��B
		/// </summary>
		/// <param name="name">�����o�Ɋ֘A�t����ꂽ���O���w�肵�܂��B</param>
		/// <returns>
		/// �擾����ۂɂ́A�w�肵�������o�̒l��Ԃ��܂��B
		/// �w�肵�������o�������p�ӂ���Ă��Ȃ��ꍇ�ɂ́Anull ��Ԃ��܂��B
		/// <para>�l�̎擾�ɂ� GetMember ���g�p���鎖�����E�߂��܂��B</para>
		/// </returns>
		public object this[string name]{
			get{
				object ret;
				if(this.data.TryGetValue(name,out ret))
					return this.data[name];
				return null;
			}
			set{this.data[name]=value;}
		}
		/// <summary>
		/// �w�肵�������o���w�肵���^�Ƃ��ēǂݎ���ĕԂ��܂��B
		/// </summary>
		/// <typeparam name="T">�����o�̒l�̌^���w�肵�܂��B</typeparam>
		/// <param name="name">�ǂݎ�郁���o�̖��O���w�肵�܂��B</param>
		/// <param name="val">�����o�̕ێ�����w�肵���^�̒l��Ԃ��܂��B
		/// �w�肵�������o�������o�^����Ă��Ȃ������ꍇ�ɂ͂��̌^�̊���l��Ԃ��܂��B</param>
		/// <returns>�w�肵�������o�����ɓo�^����Ă��Ēl��ێ����Ă����ꍇ�� true ��Ԃ��܂��B
		/// �w�肵�������o�������o�^����Ă��Ȃ������ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		public bool GetMember<T>(string name,out T val){
			object o;
			bool ret=data.TryGetValue(name,out o);
			if(ret)val=(T)o;else val=default(T);
			return ret;
		}
		/// <summary>
		/// �C�x���g�n���h���������o�ɒǉ����܂��B
		/// �����o�̓f���Q�[�g�^�ł���K�v������܂��B
		/// </summary>
		/// <param name="name">�ǉ���̃����o�̖��O���w�肵�܂��B</param>
		/// <param name="deleg">�ǉ�����C�x���g�n���h�����w�肵�܂��B</param>
		public void AddHandler(string name,System.Delegate deleg){
			System.Delegate orig;
			this.GetMember(name,out orig);
			this[name]=System.Delegate.Combine(orig,deleg);
		}
		/// <summary>
		/// �C�x���g�n���h���������o����폜���܂��B
		/// �����o�̓f���Q�[�g�^�ł���K�v������܂��B
		/// </summary>
		/// <param name="name">�폜���̃����o�̖��O���w�肵�܂��B</param>
		/// <param name="deleg">�폜����C�x���g�n���h�����w�肵�܂��B</param>
		public void RemoveHandler(string name,System.Delegate deleg) {
			System.Delegate orig;
			this.GetMember(name,out orig);
			orig=System.Delegate.Remove(orig,deleg);
			if(orig!=null)
				this[name]=orig;
			else
				this.data.Remove(name);
		}
		//============================================================
		//		�V���A���C�Y
		//============================================================
		/// <summary>
		/// �V���A���C�Y�\�ȃ����o�����V���A���C�Y���܂��B
		/// �f���Q�[�g (�C�x���g) �̃V���A���C�Y�͍s���܂���B
		/// </summary>
		/// <param name="info">�V���A���C�Y�������ʂ��i�[����ׂ̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="context">�V���A���C�Y�̏󋵂�񋟂���I�u�W�F�N�g���w�肵�܂��B</param>
		void Ser::ISerializable.GetObjectData(Ser::SerializationInfo info,Ser::StreamingContext context){
			Gen::Dictionary<string,object> mem=new Gen::Dictionary<string,object>();
			if(_data!=null)foreach(Gen::KeyValuePair<string,object> p in _data){
				System.Type vtype=p.Value.GetType();
				if(!vtype.IsSerializable||typeof(System.Delegate).IsAssignableFrom(vtype))continue;
				mem.Add(p.Key,p.Value);
			}
			info.AddValue("members",mem);
		}
		private ExtraMemberCache(Ser::SerializationInfo info,Ser::StreamingContext context){
			Gen::Dictionary<string,object> members=(Gen::Dictionary<string,object>)
				info.GetValue("members",typeof(Gen::Dictionary<string,object>));
			if(members.Count==0)
				_data=null;
			else
				_data=members;
		}
	}
}