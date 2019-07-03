using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Gdi2=System.Drawing.Drawing2D;
using Diag=System.Diagnostics;
using CM=System.ComponentModel;
using Ser=System.Runtime.Serialization;
using Forms=System.Windows.Forms;

namespace afh.Forms{

	#region interface: ITreeNodeCheckBox
	/// <summary>
	/// TreeNode �� CheckBox �̕\���������N���X�̎�����񋟂��܂��B
	/// </summary>
	[CM::TypeConverter(typeof(Design.TreeNodeCheckBoxConverter))]
	public interface ITreeNodeCheckBox{
		/// <summary>
		/// CheckBox �̑傫�����擾���܂��B
		/// </summary>
		Gdi::Size Size{get;}
		/// <summary>
		/// �w�肵����Ԃɉ����� CheckBox ��\�����܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		/// <param name="state">Check ��Ԃ��w�肵�܂��B</param>
		/// <param name="enabled">CheckBox ���L���ɂȂ��Ă��邩�ǂ������w�肵�܂��B</param>
		void DrawBox(Gdi::Graphics g,bool? state,bool enabled);
	}
	/// <summary>
	/// TreeNode �� CheckBox �̕`����s���܂��B
	/// </summary>
	public class TreeNodeCheckBox:ITreeNodeCheckBox{
		private Gdi::Image image;
		private Gdi::Rectangle rect;
		private TreeNodeCheckBox(Gdi::Image image,Gdi::Rectangle rect){
			this.image=image;
			this.rect=rect;
		}
		/// <summary>
		/// CheckBox �̑傫�����擾���܂��B
		/// </summary>
		public Gdi::Size Size{
			get{return rect.Size;}
		}
		/// <summary>
		/// �w�肵����Ԃɉ����� CheckBox ��\�����܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		/// <param name="state">Check ��Ԃ��w�肵�܂��B</param>
		/// <param name="enabled">CheckBox ���L���ɂȂ��Ă��邩�ǂ������w�肵�܂��B</param>
		public void DrawBox(Gdi::Graphics g,bool? state,bool enabled){
			Gdi::Rectangle src=rect;
			src.X+=14*(state==null?1: (bool)state?2: 0);
			src.Y+=14*(enabled?0:1);
			g.DrawImage(this.image,0,0,src,Gdi::GraphicsUnit.Pixel);
		}
		//============================================================
		//		CheckBox �F�X
		//============================================================
		private readonly static Gen::Dictionary<string,ITreeNodeCheckBox> str2ins
			=new Gen::Dictionary<string,ITreeNodeCheckBox>();
		private readonly static Gen::Dictionary<ITreeNodeCheckBox,string> ins2str
			=new Gen::Dictionary<ITreeNodeCheckBox,string>();
		static TreeNodeCheckBox(){
			RegisterCheckBox("DoubleBorder",double_border);
			RegisterCheckBox("SingleBorder",single_border);
			RegisterCheckBox("ThreeD",three_d);
			RegisterCheckBox("Frame",frame);
			RegisterCheckBox("Visual",visual);
			RegisterCheckBox("YUI",inst_yui);
		}
		private static void RegisterCheckBox(string name,ITreeNodeCheckBox chk){
			if(chk==null)
				throw new System.ArgumentNullException("chk");
			if(str2ins.ContainsKey(name))
				throw new System.InvalidOperationException("�w�肵�����O "+name+" �� ITreeNodeCheckBox �͊��ɓo�^����Ă��܂��B");
			str2ins.Add(name,chk);
			ins2str.Add(chk,name);
		}
		/// <summary>
		/// �w�肵���C���X�^���X�̖��O���擾���܂��B
		/// </summary>
		/// <param name="chkbox">�o�^����m�肽���C���X�^���X���w�肵�܂��B</param>
		/// <returns>�w�肵���C���X�^���X���o�^����Ă��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		internal static string GetName(ITreeNodeCheckBox chkbox){
			string ret;
			if(ins2str.TryGetValue(chkbox,out ret))return ret;
			return null;
		}
		/// <summary>
		/// �w�肵�����O���Ȃēo�^���ꂽ�C���X�^���X���擾���܂��B
		/// </summary>
		/// <param name="name">�擾�������C���X�^���X�̓o�^�����w�肵�܂��B</param>
		/// <returns>�w�肵�����O�ɑΉ�����C���X�^���X���o�^����Ă��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		internal static ITreeNodeCheckBox GetInstance(string name){
			ITreeNodeCheckBox ret;
			if(str2ins.TryGetValue(name,out ret))return ret;
			return null;
		}
		internal static System.Collections.ICollection CheckBoxInstances{
			get{return str2ins.Values;}
		}
		//------------------------------------------------------------
		private readonly static TreeNodeCheckBox double_border
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,1,12,12));
		private readonly static TreeNodeCheckBox three_d
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,29,13,13));
		private readonly static TreeNodeCheckBox single_border
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,57,11,11));
		private readonly static TreeNodeCheckBox frame
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,85,13,13));
		private readonly static TreeNodeCheckBox visual
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,113,13,13));
		private readonly static TreeNodeCheckBox inst_yui
			=new TreeNodeCheckBox(_resource.TreeIcons,new Gdi::Rectangle(1,141,12,12));

		/// <summary>
		/// ��d�g���� CheckBox ���擾���܂��B����� CheckBox �ł��B
		/// </summary>
		public static TreeNodeCheckBox DoubleBorder{
			get{return double_border;}
		}
		/// <summary>
		/// �O�����ɉ��� CheckBox ���擾���܂��B
		/// </summary>
		public static TreeNodeCheckBox ThreeD{
			get{return three_d;}
		}
		/// <summary>
		/// �P��g���� CheckBox ���擾���܂��B
		/// </summary>
		public static TreeNodeCheckBox SingleBorder{
			get{return single_border;}
		}
		/// <summary>
		/// �g�t���� CheckBox ���擾���܂��B
		/// </summary>
		public static TreeNodeCheckBox Frame{
			get{return frame;}
		}
		/// <summary>
		/// Luna ���� CheckBox ���擾���܂��B
		/// </summary>
		public static TreeNodeCheckBox Visual{
			get{return visual;}
		}
		/// <summary>
		/// Yahoo! User Interface ���� CheckBox ���擾���܂��B
		/// </summary>
		public static TreeNodeCheckBox YUI{
			get{return inst_yui;}
		}
	}
	#endregion

	#region interface: ITreeNodeIcon
	/// <summary>
	/// TreeNode �ɕ\������ Icon �̎�����񋟂��܂��B
	/// </summary>
	public interface ITreeNodeIcon{
		/// <summary>
		/// �w�肵���̈�� Icon ��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`���� Graphics ���w�肵�܂��B</param>
		/// <param name="rect">�`���̗̈���w�肵�܂��B�A�C�R�������̗̈�S�̂Ɋg�債�ĕ`�悵�܂��B</param>
		/// <param name="node">�A�C�R���̑Ώۂł��� TreeNode ���w�肵�܂��B
		/// TreeNode �̏�Ԃɂ���ăA�C�R����ύX����ꍇ�Ɏg�p���܂��B</param>
		void DrawIcon(Gdi::Graphics g,Gdi::Rectangle rect,TreeNode node);
	}
	/// <summary>
	/// TreeNode �ɕ\������ Icon ���Ǘ����܂��B
	/// </summary>
	public sealed class TreeNodeIcon:ITreeNodeIcon{
		IconType type=IconType.Image;
		private Gdi::Icon icon=null;
		private Gdi::Image img=null;
		private Forms::ImageList list=null;
		private int index=0;
		private Gdi::Rectangle rect=default(Gdi::Rectangle);

		void ITreeNodeIcon.DrawIcon(Gdi::Graphics g,Gdi::Rectangle rect,TreeNode node){
			switch(this.type){
				case IconType.Image:
					g.DrawImage(this.img,rect);
					break;
				case IconType.Icon:
					g.DrawIcon(this.icon,rect);
					break;
				case IconType.ImageList:
					g.DrawImage(this.list.Images[this.index],rect);
					break;
				case IconType.ClippedImage:
					g.DrawImage(this.img,rect,this.rect,Gdi::GraphicsUnit.Pixel);
					break;
			}
		}
		/// <summary>
		/// ���݂̃A�C�R���̎�ނ��擾���܂��B
		/// </summary>
		public IconType Type{
			get{return this.type;}
		}
		/// <summary>
		/// �A�C�R���̎w��̕��@���L�q���܂��B
		/// </summary>
		public enum IconType{
			/// <summary>
			/// System.Drawing.Image ���A�C�R���Ƃ��Ďg�p���܂��B
			/// </summary>
			Image,
			/// <summary>
			/// System.Drawing.Icon ���A�C�R���Ƃ��Ďg�p���܂��B
			/// </summary>
			Icon,
			/// <summary>
			/// �C���[�W���X�g���w�肵�A���̒��̔ԍ��Ƃ��ăA�C�R�����w�肵�܂��B
			/// </summary>
			ImageList,
			/// <summary>
			/// System.Drawing.Image �̈ꕔ�����A�C�R���Ƃ��Đ؂����Ďg�p���܂��B
			/// </summary>
			ClippedImage,
		}
		//============================================================
		//		������
		//============================================================
		private TreeNodeIcon(){}
		/// <summary>
		/// System.Drawing.Icon ������ TreeNodeIcon �����������ĕԂ��܂��B
		/// </summary>
		/// <param name="icon">TreeNode �̃A�C�R���̌��ɂȂ�A�C�R�����w�肵�܂��B</param>
		/// <returns>�������� TreeNodeIcon ��Ԃ��܂��B</returns>
		public static explicit operator TreeNodeIcon(Gdi::Icon icon){
			TreeNodeIcon ret=new TreeNodeIcon();
			ret.SetIcon(icon);
			return ret;
		}
		/// <summary>
		/// System.Drawing.Image ������ TreeNodeIcon �����������ĕԂ��܂��B
		/// </summary>
		/// <param name="image">�A�C�R���̌��ɂȂ�摜���w�肵�܂��B</param>
		/// <returns>�������� TreeNodeIcon ��Ԃ��܂��B</returns>
		public static explicit operator TreeNodeIcon(Gdi::Image image) {
			TreeNodeIcon ret=new TreeNodeIcon();
			ret.SetImage(image);
			return ret;
		}
		//============================================================
		//		����l
		//============================================================
		private static readonly TreeNodeIcon file=new TreeNodeIcon();
		private static readonly TreeNodeIcon folder=new TreeNodeIcon();
		static TreeNodeIcon(){
			TreeNodeIcon.file.SetImage(
				_resource.TreeIcons,
				new Gdi::Rectangle(43,1,12,12)
				);
			TreeNodeIcon.folder.SetImage(
				_resource.TreeIcons,
				new Gdi::Rectangle(57,1,12,12)
				);
		}
		/// <summary>
		/// ����� TreeViewIcon ��񋟂��܂��B
		/// </summary>
		public static TreeNodeIcon File{
			get{return TreeNodeIcon.file;}
		}
		/// <summary>
		/// ����� TreeViewIcon ��񋟂��܂��B
		/// </summary>
		public static TreeNodeIcon Folder{
			get{return TreeNodeIcon.folder;}
		}
		internal sealed class DefaultValueAttribute:CM::DefaultValueAttribute{
			public DefaultValueAttribute()
				:base(TreeNodeIcon.File){}
		}
		//============================================================
		//		�摜�̐ݒ�
		//============================================================
		/// <summary>
		/// System.Drawing.Icon �ŃA�C�R����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�ݒ肷��A�C�R�����w�肵�܂��B</param>
		public void SetIcon(Gdi::Icon value){
			//if(type==IconType.Icon&&icon==value)return;
			this.type=IconType.Icon;
			icon=value;
			img=null;
			list=null;
		}
		/// <summary>
		/// System.Drawing.Image �ŃA�C�R����ݒ肵�܂��B
		/// </summary>
		/// <param name="value">�ݒ肷��A�C�R�����w�肵�܂��B</param>
		public void SetImage(Gdi::Image value){
			//if(type==IconType.Image&&img==value)return;
			this.type=IconType.Image;
			img=value;
			icon=null;
			list=null;
		}
		/// <summary>
		/// �摜�̈ꕔ���A�C�R���Ƃ��Đݒ肵�܂��B
		/// </summary>
		/// <param name="image">���ƂȂ�摜���w�肵�܂��B</param>
		/// <param name="rect">�A�C�R���Ƃ��Ďg�p����摜�̕������w�肷���`���w�肵�܂��B</param>
		public void SetImage(Gdi::Image image,Gdi::Rectangle rect){
			this.type=IconType.ClippedImage;
			this.img=image;
			this.rect=rect;
		}
		/// <summary>
		/// ImageList �ɓo�^����Ă���摜���A�C�R���Ƃ��Ďg�p���܂��B
		/// </summary>
		/// <param name="list">�g�p����摜���܂�ł��� ImageList ���w�肵�܂��B</param>
		/// <param name="index">�g�p����摜�� ImageList �̒��ɉ�����ԍ����w�肵�܂��B</param>
		public void SetImage(Forms::ImageList list,int index){
			//if(this.type==IconType.ImageList&&this.list==list&&this.index=index)return;
			this.type=IconType.ImageList;
			this.list=list;
			this.index=index;
			this.icon=null;
			this.img=null;
		}
	}
	#endregion

	#region interface: ITreeNodeIndentArea
	/// <summary>
	/// TreeNode �� Indent-Area (�} ��}�̕���) ��\������N���X�̎�����񋟂��܂��B
	/// </summary>
	public interface ITreeNodeIndentArea{
		/// <summary>
		/// �}�̈�̍ő卂�����擾���܂��B
		/// ���̒l���w�肵���ꍇ�ɂ� �} �̓m�[�h�̒��x�����̍����ɕ\������܂��B
		/// �m�[�h�̍����������̒l�̕����傫���ꍇ�ɂ� �} �̓m�[�h�̒��x�����̍����ɕ\������܂��B
		/// �m�[�h�̍����̕������̒l�����傫���ꍇ�ɂ� �} �̓m�[�h�̏�[�ɍ��킹�ĕ`�悳��܂��B
		/// </summary>
		int Height{get;}
		/// <summary>
		/// Indent Area �̕����擾���܂��B
		/// �}�̃}�[�N���܂߂��C���f���g�ʂ�\�����܂��B
		/// </summary>
		int Width{get;}
		/// <summary>
		/// Indent �̈�̓��e��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`�悷��Ώۂ� Graphics ���w�肵�܂��B
		/// ���_�� Indent �̈�̉E��ɐݒ肳��܂��B</param>
		/// <param name="node">�`�悳��� node ���w�肵�܂��B</param>
		/// <param name="areaheight">�`�悷��Ώۂ̗̈�̍������w�肵�܂��B</param>
		void DrawIndentArea(Gdi::Graphics g,TreeNode node,int areaheight);
		/// <summary>
		/// �q���̈�ׂ̈� Indent �̈��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`�悷��Ώۂ� Graphics ���w�肵�܂��B
		/// ���_�� Indent �̈�̉E��ɐݒ肳��܂��B</param>
		/// <param name="node">�q���̈�̐e�m�[�h���w�肵�܂��B
		/// ���� Indent �̈�Ɠ��� Indent ���󂯂Ă��� node �ł��B</param>
		/// <param name="areaheight">�`�悷��Ώۂ̗̈�̍������w�肵�܂��B</param>
		void DrawIndentForDescendant(Gdi::Graphics g,TreeNode node,int areaheight);
		/// <summary>
		/// �w�肵���_�� �} �̋L���̏�ɑ��݂��Ă��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="x">�Ǐ����W�ł� x ���W���w�肵�܂��B
		/// �� IndentArea �͋Ǐ����W�ɉ����� x��0 �ł��B</param>
		/// <param name="y">�Ǐ����W�ł� y ���W���w�肵�܂��B</param>
		/// <param name="height">Indent �̈�̍������w�肵�܂��B
		/// ����́AIndent �̈�̍����ɉ����� �} �̈ʒu���ω�����l�ȏꍇ�ɎQ�Ƃ���܂��B</param>
		/// <param name="node">IndentArea �̉E�ɕ`�悳���m�[�h���w�肵�܂��B
		/// ����́A�m�[�h�̏�Ԃɂ���� �} �̈ʒu��傫�����ω�����l�ȏꍇ�ɎQ�Ƃ���܂��B</param>
		/// <returns>�w�肵���_�� �} �̋L���̏�ɂ��������� true ��Ԃ��܂��B
		/// ����ȊO�̏ꍇ�� false ��Ԃ��܂��B</returns>
		bool HitPlusMinus(int x,int y,int height,TreeNode node);
	}
	/// <summary>
	/// ITreeNodeIndentArea �̕W���I�Ȏ�����񋟂��܂��B
	/// </summary>
	[System.Serializable]
	public sealed class TreeNodeIndentArea:ITreeNodeIndentArea{
		private Gdi::Size size;
		/// <summary>
		/// �����w�肵�� TreeNodeIndentArea �����������܂��B
		/// </summary>
		/// <param name="width">TreeNodeIndentArea �̕����w�肵�܂��B</param>
		public TreeNodeIndentArea(int width):this(width,-1){}
		/// <summary>
		/// IndentArea �̕��ƍ������w�肵�� TreeNodeIndentArea �����������܂��B
		/// </summary>
		/// <param name="width">TreeNodeIndentArea �̕����w�肵�܂��B</param>
		/// <param name="height">
		/// </param>
		public TreeNodeIndentArea(int width,int height){
			this.size=new Gdi::Size(width,height);
		}

		const int TWIG_LENGTH=10;

		//OK: _todo.TreeNodeCheck("Pen.Clone �ł����� HANDLE ����������Ă���̂�?");
		private static readonly Gdi::Pen pen_g=(Gdi::Pen)Gdi::Pens.Gray.Clone();
		static TreeNodeIndentArea(){
			pen_g.DashStyle=Gdi::Drawing2D.DashStyle.Dot;
		}

		const int ICON_H=9;
		const int ICON_W=9;

		#region ITreeNodeIndentArea �����o
		/// <summary>
		/// �}�̈�̍ő卂�����擾���܂��B
		/// ���̒l���w�肵���ꍇ�ɂ� �} �̓m�[�h�̒��x�����̍����ɕ\������܂��B
		/// �m�[�h�̍����������̒l�̕����傫���ꍇ�ɂ� �} �̓m�[�h�̒��x�����̍����ɕ\������܂��B
		/// �m�[�h�̍����̕������̒l�����傫���ꍇ�ɂ� �} �̓m�[�h�̏�[�ɍ��킹�ĕ`�悳��܂��B
		/// </summary>
		public int Height{
			get{return this.size.Height;}
		}
		/// <summary>
		/// Indent Area �̕����擾���܂��B
		/// �}�̃}�[�N���܂߂��C���f���g�ʂ�\�����܂��B
		/// </summary>
		public int Width{
			get{return this.size.Width;}
		}
		/// <summary>
		/// Indent Area �̑傫�����擾���܂��B
		/// Width �� �} �̃}�[�N���܂߂��C���f���g�ʂ�\���܂��B
		/// Height �� �ŏ��̍�����\���܂��B
		/// (�}�̕`��͎��ۂ̍����ɍ��킹�čs���܂��B)
		/// </summary>
		public System.Drawing.Size Size{
			get{return this.size;}
		}
		/// <summary>
		/// Indent �̈�̓��e��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`�悷��Ώۂ� Graphics ���w�肵�܂��B
		/// ���_�� Indent �̈�̉E��ɐݒ肳��܂��B</param>
		/// <param name="node">�`�悳��� node ���w�肵�܂��B</param>
		/// <param name="areaheight">�`�悷��Ώۂ̗̈�̍������w�肵�܂��B</param>
		public void DrawIndentArea(System.Drawing.Graphics g,TreeNode node,int areaheight){
			int oy=y_origin(areaheight);

			/* �}�͗l��`�� */
			if(node.IsLastChild){
				g.DrawLine(pen_g,-TWIG_LENGTH,0,-TWIG_LENGTH,oy);
			}else{
				g.DrawLine(pen_g,-TWIG_LENGTH,0,-TWIG_LENGTH,areaheight);
			}
			g.DrawLine(pen_g,1-TWIG_LENGTH,oy,0,oy);

			if(node.HasChildren){

				Gdi::Rectangle srcRect;
				if(node.IsExpanded){
					srcRect=new Gdi::Rectangle(85,1,ICON_W,ICON_H);
				}else{
					srcRect=new Gdi::Rectangle(71,1,ICON_W,ICON_H);
				}

				g.DrawImage(
					_resource.TreeIcons,-TWIG_LENGTH-ICON_W/2,oy-ICON_H/2,
					srcRect,Gdi::GraphicsUnit.Pixel
					);
			}
		}
		/// <summary>
		/// �q���̈�ׂ̈� Indent �̈��`�悵�܂��B
		/// </summary>
		/// <param name="g">�`�悷��Ώۂ� Graphics ���w�肵�܂��B
		/// ���_�� Indent �̈�̉E��ɐݒ肳��܂��B</param>
		/// <param name="node">�q���̈�̐e�m�[�h���w�肵�܂��B
		/// ���� Indent �̈�Ɠ��� Indent ���󂯂Ă��� node �ł��B</param>
		/// <param name="areaheight">�`�悷��Ώۂ̗̈�̍������w�肵�܂��B</param>
		public void DrawIndentForDescendant(System.Drawing.Graphics g,TreeNode node,int areaheight) {
			if(node.IsLastChild)return;
			g.DrawLine(pen_g,-TWIG_LENGTH,0,-TWIG_LENGTH,areaheight);
		}
		/// <summary>
		/// �w�肵���_�� �} �̋L���̏�ɑ��݂��Ă��邩�ۂ����擾���͐ݒ肵�܂��B
		/// </summary>
		/// <param name="x">�Ǐ����W�ł� x ���W���w�肵�܂��B</param>
		/// <param name="y">�Ǐ����W�ł� y ���W���w�肵�܂��B</param>
		/// <param name="height">Indent �̈�̍������w�肵�܂��B</param>
		/// <param name="node">IndentArea �̉E�ɕ`�悳���m�[�h���w�肵�܂��B</param>
		/// <returns>�w�肵���_�� �} �̋L���̏�ɂ��������� true ��Ԃ��܂��B</returns>
		public bool HitPlusMinus(int x,int y,int height,TreeNode node){
			y-=y_origin(height)-ICON_H/2;
			x-=-(TWIG_LENGTH+ICON_W/2);
			return 0<=x&&x<ICON_W&&0<=y&&y<ICON_H;
		}
		#endregion

		/// <summary>
		/// �`��Ώۗ̈�̍�������A�m�[�h�}�̋N�_���������߂܂��B
		/// </summary>
		/// <param name="areaheight"></param>
		/// <returns></returns>
		private int y_origin(int areaheight){
			int h=this.Height;
			if(h<0||areaheight<h)return areaheight/2;
			return h/2;
		}
		//==================================================================
		//		����̃C���X�^���X
		//==================================================================
		private static readonly TreeNodeIndentArea ins_def=new TreeNodeIndentArea(18);
		/// <summary>
		/// �W���� TreeNodeIndentArea �C���X�^���X���擾���܂��B
		/// </summary>
		public static ITreeNodeIndentArea Default{
			get{return ins_def;}
		}
		/// <summary>
		/// �v���p�e�B�̊���l�� TreeNodeIndentArea.Default �ō݂鎖�������܂��B
		/// </summary>
		internal sealed class DefaultValueAttribute:CM::DefaultValueAttribute{
			public DefaultValueAttribute()
				:base(TreeNodeIndentArea.Default){}
		}
	}
	internal sealed class TreeNodeIndentAreaEmpty:ITreeNodeIndentArea{
		public int Height{get{return 0;}}
		public int Width{get{return 0;}}
		public void DrawIndentArea(System.Drawing.Graphics g,TreeNode node,int areaheight){}
		public void DrawIndentForDescendant(System.Drawing.Graphics g,TreeNode node,int areaheight){}
		public bool HitPlusMinus(int x,int y,int height,TreeNode node){
			return false;
		}
	}
	#endregion

	#region interface: ITreeNodeDDBehavior
	/// <summary>
	/// Drag &amp; Drop �̍ۂ̃m�[�h�̓�����������܂��B
	/// </summary>
	public interface ITreeNodeDDBehavior{
		/// <summary>
		/// �w�肵�� TreeNode ���h���b�O�\���ۂ����肵�A
		/// �\�ȃh���b�O�h���b�v�G�t�F�N�g��Ԃ��܂��B
		/// </summary>
		/// <param name="node">����Ώۂ� TreeNode ���w�肵�܂��B</param>
		/// <returns>�h���b�O�\�̏ꍇ�ɂ́A�\�� DragDropEffects ��Ԃ��܂��B
		/// �h���b�O�s�\�̏ꍇ�ɂ� DragDropEffects.None ��Ԃ��܂��B</returns>
		Forms::DragDropEffects GetAllowedDDE(TreeNode node);
		/// <summary>
		/// �f�[�^���m�[�h�̏�Ƀh���b�O����ė������̓����񋟂��܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
		void OnDrag(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �h���b�v���������s���܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
		void OnDrop(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �h���b�O�������Ă��鎞�̏������s���܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
		void OnEnter(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �h���b�O���v�f�̊O�֏o�čs�����̏������s���܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B
		/// ��񂪂Ȃ��ꍇ�ɂ� null ���w�肵�܂��B</param>
		void OnLeave(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �w�肵�����ʂɑΉ�������ʂȃJ�[�\�����擾���܂��B
		/// ����̃J�[�\�����g�p����ꍇ�� null ��Ԃ��܂��B
		/// </summary>
		/// <param name="node">�h���b�v�Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="effect">���݂̃h���b�v���ʂ��w�肵�܂��B</param>
		/// <returns>�h���b�v���ʂɑΉ�����J�[�\����Ԃ��܂��B
		/// ���̃h���b�v���ʂ̊���̃J�[�\�����g�p����ꍇ�� null ��Ԃ��܂��B
		/// </returns>
		Forms::Cursor GetCursor(TreeNode node,Forms::DragDropEffects effect);
	}
	/// <summary>
	/// �h���b�O�h���b�v�̑Ώۂ��w�肷��l�ł��B
	/// ���݃}�E�X������m�[�h����̑��Ί֌W�Ŏw�肵�܂��B
	/// </summary>
	[System.Flags]
	[System.Serializable]
	public enum TreeNodeDDTarget{
		/// <summary>
		/// �������g���h���b�v�̑Ώۂł��鎖�������܂��B
		/// </summary>
		Self=1,
		/// <summary>
		/// ���m�[�h�̒��O�ɁA�h���b�v���ڂ�}�����鎖�������܂��B
		/// </summary>
		Prev=2,
		/// <summary>
		/// ���m�[�h�̒���ɁA�h���b�v���ڂ�}�����鎖�������܂��B
		/// </summary>
		Next=4,
		/// <summary>
		/// ���m�[�h�̎q�v�f�̐擪�ɁA�h���b�v���ڂ�ǉ����鎖�������܂��B
		/// </summary>
		Child=8,
	}
	/// <summary>
	/// �h���b�O�h���b�v�������������ׂ́A�֗��Ȋ�{�N���X�ł��B
	/// </summary>
	[System.Serializable]
	public abstract class TreeNodeDDBehaviorBase:ITreeNodeDDBehavior{
		/// <summary>
		/// �w�肵�� TreeNode ���h���b�O�\���ۂ����肵�A
		/// �\�ȃh���b�O�h���b�v�G�t�F�N�g��Ԃ��܂��B
		/// </summary>
		/// <param name="node">����Ώۂ� TreeNode ���w�肵�܂��B</param>
		/// <returns>�h���b�O�\�̏ꍇ�ɂ́A�\�� DragDropEffects ��Ԃ��܂��B
		/// �h���b�O�s�\�̏ꍇ�ɂ� DragDropEffects.None ��Ԃ��܂��B</returns>
		public abstract Forms::DragDropEffects GetAllowedDDE(TreeNode node);
		//-------------------------------------------------------------------------
		/// <summary>
		/// �h���b�v��������s���܂��B
		/// </summary>
		/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
		public abstract void OnDrop(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �h���b�O�h���b�v����̊��҂���� DDE ���擾���܂��B
		/// </summary>
		/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
		/// <returns>�h���b�v�ɂ���Ċ��҂���� DDE ��Ԃ��܂��B</returns>
		protected abstract Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// �h���b�v�悩��̃f�[�^�̑������擾���܂��B
		/// </summary>
		/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
		/// <returns>�h���b�v�悩��̃f�[�^�̑�����Ԃ��܂��B</returns>
		protected virtual TreeNodeDDTarget GetTarget(TreeNode node,TreeNodeDragEventArgs e){
			//System.Console.WriteLine("dbg: "+e.HitTypeVertical.ToString());
			switch(e.HitTypeVertical){
				case TreeNodeHitType.Above:
				case TreeNodeHitType.BorderTop:
					return TreeNodeDDTarget.Prev;
				case TreeNodeHitType.Below:
				case TreeNodeHitType.BorderBottom:
					if(node.HasChildren&&node.IsExpanded)
						return TreeNodeDDTarget.Child;
					return TreeNodeDDTarget.Next;
				default:
					return TreeNodeDDTarget.Self;
			}
		}
		private void SetRevert(TreeNode node,TreeNodeDragEventArgs e){
			switch(GetTarget(node,e)){
				case TreeNodeDDTarget.Self:{
					Gdi::Rectangle rect=new Gdi::Rectangle(node.ClientPosition,node.ContentSize);
					rect.X+=node.ContentOffsetX;
					node.View.dropArea.RevertRect(rect);
					break;
				}
				case TreeNodeDDTarget.Prev:{
					Gdi::Point p=node.ClientPosition;
					p.X+=node.ContentOffsetX;
					node.View.dropArea.RevertLine(p);
					break;
				}
				case TreeNodeDDTarget.Next:{
					Gdi::Point p=node.ClientPosition;
					p.X+=node.ContentOffsetX;
					p.Y+=node.ContentSize.Height;
					node.View.dropArea.RevertLine(p);
					break;
				}
				case TreeNodeDDTarget.Child:{
					Gdi::Point p=node.ClientPosition;
					p.X+=node.ContentOffsetX+node.ChildIndent.Width;
					p.Y+=node.ContentSize.Height;
					node.View.dropArea.RevertLine(p);
					break;
				}
			}
		}
		//-------------------------------------------------------------------------
		/// <summary>
		/// �f�[�^���m�[�h�̏�Ƀh���b�O����ė������̓����񋟂��܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
		public virtual void OnDrag(TreeNode node,TreeNodeDragEventArgs e){
			e.Effect=GetEffect(node,e);
			this.SetRevert(node,e);
		}
		/// <summary>
		/// �h���b�O�������Ă��鎞�̏������s���܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B</param>
		public virtual void OnEnter(TreeNode node,TreeNodeDragEventArgs e){
			this.SetRevert(node,e);
		}
		/// <summary>
		/// �h���b�O���v�f�̊O�֏o�čs�����̏������s���܂��B
		/// </summary>
		/// <param name="node">�����Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�̏����w�肵�܂��B
		/// ��񂪂Ȃ��ꍇ�ɂ� null ���w�肵�܂��B</param>
		public virtual void OnLeave(TreeNode node,TreeNodeDragEventArgs e){
			node.View.dropArea.Clear();
		}
		/// <summary>
		/// �w�肵�����ʂɑΉ�������ʂȃJ�[�\�����擾���܂��B
		/// ����̃J�[�\�����g�p����ꍇ�� null ��Ԃ��܂��B
		/// </summary>
		/// <param name="node">�h���b�v�Ώۂ̃m�[�h���w�肵�܂��B</param>
		/// <param name="effect">���݂̃h���b�v���ʂ��w�肵�܂��B</param>
		/// <returns>�h���b�v���ʂɑΉ�����J�[�\����Ԃ��܂��B
		/// ���̃h���b�v���ʂ̊���̃J�[�\�����g�p����ꍇ�� null ��Ԃ��܂��B
		/// </returns>
		public virtual Forms::Cursor GetCursor(TreeNode node,Forms::DragDropEffects effect){return null;}
	}
	/// <summary>
	/// �ÓI�ȃh���b�O�h���b�v�����\������N���X�ł��B
	/// �h���b�O�����m�[�h�̏�ԂɍS��炸�A���̃h���b�O�h���b�v�G�t�F�N�g��񋟂��܂��B
	/// </summary>
	[System.Serializable]
	public class TreeNodeDDBehaviorStatic:TreeNodeDDBehaviorBase{
		Forms::DragDropEffects allowed;
		/// <summary>
		/// TreeNodeDDBehaviorStatic �����������܂��B
		/// </summary>
		/// <param name="allowedEffects">�\�Ȕ�h���b�O������w�肵�܂��B</param>
		public TreeNodeDDBehaviorStatic(Forms::DragDropEffects allowedEffects){
			this.allowed=allowedEffects;
		}

		/// <summary>
		/// �w�肵�� TreeNode ���h���b�O�\���ۂ����肵�A
		/// �\�ȃh���b�O�h���b�v�G�t�F�N�g��Ԃ��܂��B
		/// </summary>
		/// <param name="node">����Ώۂ� TreeNode ���w�肵�܂��B</param>
		/// <returns>�h���b�O�\�̏ꍇ�ɂ́A�\�� DragDropEffects ��Ԃ��܂��B
		/// �h���b�O�s�\�̏ꍇ�ɂ� DragDropEffects.None ��Ԃ��܂��B</returns>
		public override Forms::DragDropEffects GetAllowedDDE(TreeNode node){return this.allowed;}
		/// <summary>
		/// �h���b�v��������s���܂��B
		/// </summary>
		/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
		public override void OnDrop(TreeNode node,TreeNodeDragEventArgs e){}
		/// <summary>
		/// �h���b�O�h���b�v����̊��҂���� DDE ���擾���܂��B
		/// </summary>
		/// <param name="node">�h���b�v��̃m�[�h���w�肵�܂��B</param>
		/// <param name="e">�h���b�O�Ɋւ�������w�肵�܂��B</param>
		/// <returns>�h���b�v�ɂ���Ċ��҂���� DDE ��Ԃ��܂��B</returns>
		protected override Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e) {
			return Forms::DragDropEffects.Copy;
		}
		//============================================================
		//		�ÓI�����o
		//============================================================
		private static ITreeNodeDDBehavior empty=new TreeNodeDDBehaviorEmpty();
		/// <summary>
		/// ��̃h���b�O�h���b�v������擾���܂��B
		/// �h���b�O�h���b�v����ɑ΂��ĉ������s���܂���B
		/// </summary>
		public static ITreeNodeDDBehavior Empty{
			get{return empty;}
		}

		private static ITreeNodeDDBehavior debug=new TreeNodeDDBehaviorStatic(Forms::DragDropEffects.All);
		/// <summary>
		/// �S�Ă̔�h���b�O���삪�\�� DDBehavior �C���X�^���X�ł��B
		/// </summary>
		public static ITreeNodeDDBehavior AllForDebug{
			get{return debug;}
		}

		/// <summary>
		/// �v���p�e�B�̊���l�� TreeNodeIndentArea.Default �ō݂鎖�������܂��B
		/// </summary>
		internal sealed class DefaultValueAttribute:CM::DefaultValueAttribute{
			public DefaultValueAttribute()
				:base(TreeNodeDDBehaviorStatic.Empty){}
		}
	}
	[System.Serializable]
	internal sealed class TreeNodeDDBehaviorEmpty:ITreeNodeDDBehavior{
		public Forms::DragDropEffects GetAllowedDDE(TreeNode node){
			return Forms::DragDropEffects.None;
		}
		public void OnDrag(TreeNode node,TreeNodeDragEventArgs e){}
		public void OnDrop(TreeNode node,TreeNodeDragEventArgs e){}
		public void OnEnter(TreeNode node,TreeNodeDragEventArgs e){}
		public void OnLeave(TreeNode node,TreeNodeDragEventArgs e){}
		public Forms::Cursor GetCursor(TreeNode node,Forms::DragDropEffects effect){return null;}
	}
	namespace Design{
		/// <summary>
		/// �R���g���[����̔��]�̈���Ǘ����܂��B
		/// </summary>
		internal class ReversibleArea{
			readonly Forms::Control ctrl;
			public ReversibleArea(Forms::Control ctrl){
				this.ctrl=ctrl;
			}
#if OLD
			/// <summary>
			/// 0: ���]�\������Ă��镔���͂Ȃ�
			/// 1: ���]�\������Ă��钼��
			/// </summary>
			private int currentState=0;
			public void Clear(){
				if(currentState==1){
					this.RevertLine(currentRect);
				}else if(currentState==2){
					this.RevertRect(currentRect);
				}
				currentState=0;
			}
			/// <summary>
			/// ���]�\�����Ă���ʒu��ύX���܂��B
			/// </summary>
			/// <param name="rect">�V�������]�\���ʒu���w�肵�܂��B</param>
			public void Revert(System.Drawing.Rectangle rect,int mode){
				if(currentRect!=rect){
					this.Clear();
					currentState=mode;
					currentRect=rect;
					if(currentState==1){
						this.RevertLine(currentRect.Location);
					}else if(currentState==2){
						this.RevertRect(currentRect);
					}
				}
			}
#endif
			private bool currentState=false;
			private Gdi::Rectangle currentRect;
			/// <summary>
			/// ���ݔ��]���Ă���̈���������܂��B
			/// </summary>
			public void Clear(){
				if(currentState){
					Forms::ControlPaint.FillReversibleRectangle(
						this.ctrl.RectangleToScreen(currentRect),
						Gdi::Color.Black);
					currentState=false;
				}
			}
			/// <summary>
			/// �w�肵���_����E�֐��������܂��B
			/// </summary>
			/// <param name="pt">���̊J�n�_���w�肵�܂��B</param>
			public void RevertLine(Gdi::Point pt){
				Gdi::Rectangle rect=new Gdi::Rectangle(
					pt.X,pt.Y-1,
					this.ctrl.ClientRectangle.Width-pt.X,3
				);
				this.RevertRect(rect);
			}
			/// <summary>
			/// �w�肵����`�̈�𔽓]���܂��B
			/// </summary>
			/// <param name="rect">���]����̈���w�肵�܂��B</param>
			public void RevertRect(Gdi::Rectangle rect){
				if(!currentState||currentRect!=rect){
					this.Clear();
					currentState=true;
					currentRect=rect;
					Forms::ControlPaint.FillReversibleRectangle(
						this.ctrl.RectangleToScreen(currentRect),
						Gdi::Color.Black);
				}
			}
		}
	}
	#endregion

	internal sealed class FontDefaultValueAttribute:CM::DefaultValueAttribute{
		public FontDefaultValueAttribute()
			:base(Forms::Control.DefaultFont){}
	}
}
