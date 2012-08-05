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
	/// TreeNode の CheckBox の表示を扱うクラスの実装を提供します。
	/// </summary>
	[CM::TypeConverter(typeof(Design.TreeNodeCheckBoxConverter))]
	public interface ITreeNodeCheckBox{
		/// <summary>
		/// CheckBox の大きさを取得します。
		/// </summary>
		Gdi::Size Size{get;}
		/// <summary>
		/// 指定した状態に応じた CheckBox を表示します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		/// <param name="state">Check 状態を指定します。</param>
		/// <param name="enabled">CheckBox が有効になっているかどうかを指定します。</param>
		void DrawBox(Gdi::Graphics g,bool? state,bool enabled);
	}
	/// <summary>
	/// TreeNode の CheckBox の描画を行います。
	/// </summary>
	public class TreeNodeCheckBox:ITreeNodeCheckBox{
		private Gdi::Image image;
		private Gdi::Rectangle rect;
		private TreeNodeCheckBox(Gdi::Image image,Gdi::Rectangle rect){
			this.image=image;
			this.rect=rect;
		}
		/// <summary>
		/// CheckBox の大きさを取得します。
		/// </summary>
		public Gdi::Size Size{
			get{return rect.Size;}
		}
		/// <summary>
		/// 指定した状態に応じた CheckBox を表示します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		/// <param name="state">Check 状態を指定します。</param>
		/// <param name="enabled">CheckBox が有効になっているかどうかを指定します。</param>
		public void DrawBox(Gdi::Graphics g,bool? state,bool enabled){
			Gdi::Rectangle src=rect;
			src.X+=14*(state==null?1: (bool)state?2: 0);
			src.Y+=14*(enabled?0:1);
			g.DrawImage(this.image,0,0,src,Gdi::GraphicsUnit.Pixel);
		}
		//============================================================
		//		CheckBox 色々
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
				throw new System.InvalidOperationException("指定した名前 "+name+" の ITreeNodeCheckBox は既に登録されています。");
			str2ins.Add(name,chk);
			ins2str.Add(chk,name);
		}
		/// <summary>
		/// 指定したインスタンスの名前を取得します。
		/// </summary>
		/// <param name="chkbox">登録名を知りたいインスタンスを指定します。</param>
		/// <returns>指定したインスタンスが登録されていない場合には null を返します。</returns>
		internal static string GetName(ITreeNodeCheckBox chkbox){
			string ret;
			if(ins2str.TryGetValue(chkbox,out ret))return ret;
			return null;
		}
		/// <summary>
		/// 指定した名前を以て登録されたインスタンスを取得します。
		/// </summary>
		/// <param name="name">取得したいインスタンスの登録名を指定します。</param>
		/// <returns>指定した名前に対応するインスタンスが登録されていない場合には null を返します。</returns>
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
		/// 二重枠線の CheckBox を取得します。既定の CheckBox です。
		/// </summary>
		public static TreeNodeCheckBox DoubleBorder{
			get{return double_border;}
		}
		/// <summary>
		/// 三次元に凹んだ CheckBox を取得します。
		/// </summary>
		public static TreeNodeCheckBox ThreeD{
			get{return three_d;}
		}
		/// <summary>
		/// 単一枠線の CheckBox を取得します。
		/// </summary>
		public static TreeNodeCheckBox SingleBorder{
			get{return single_border;}
		}
		/// <summary>
		/// 枠付きの CheckBox を取得します。
		/// </summary>
		public static TreeNodeCheckBox Frame{
			get{return frame;}
		}
		/// <summary>
		/// Luna 風の CheckBox を取得します。
		/// </summary>
		public static TreeNodeCheckBox Visual{
			get{return visual;}
		}
		/// <summary>
		/// Yahoo! User Interface 風の CheckBox を取得します。
		/// </summary>
		public static TreeNodeCheckBox YUI{
			get{return inst_yui;}
		}
	}
	#endregion

	#region interface: ITreeNodeIcon
	/// <summary>
	/// TreeNode に表示する Icon の実装を提供します。
	/// </summary>
	public interface ITreeNodeIcon{
		/// <summary>
		/// 指定した領域に Icon を描画します。
		/// </summary>
		/// <param name="g">描画先の Graphics を指定します。</param>
		/// <param name="rect">描画先の領域を指定します。アイコンをこの領域全体に拡大して描画します。</param>
		/// <param name="node">アイコンの対象である TreeNode を指定します。
		/// TreeNode の状態によってアイコンを変更する場合に使用します。</param>
		void DrawIcon(Gdi::Graphics g,Gdi::Rectangle rect,TreeNode node);
	}
	/// <summary>
	/// TreeNode に表示する Icon を管理します。
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
		/// 現在のアイコンの種類を取得します。
		/// </summary>
		public IconType Type{
			get{return this.type;}
		}
		/// <summary>
		/// アイコンの指定の方法を記述します。
		/// </summary>
		public enum IconType{
			/// <summary>
			/// System.Drawing.Image をアイコンとして使用します。
			/// </summary>
			Image,
			/// <summary>
			/// System.Drawing.Icon をアイコンとして使用します。
			/// </summary>
			Icon,
			/// <summary>
			/// イメージリストを指定し、その中の番号としてアイコンを指定します。
			/// </summary>
			ImageList,
			/// <summary>
			/// System.Drawing.Image の一部分をアイコンとして切り取って使用します。
			/// </summary>
			ClippedImage,
		}
		//============================================================
		//		初期化
		//============================================================
		private TreeNodeIcon(){}
		/// <summary>
		/// System.Drawing.Icon を元に TreeNodeIcon を初期化して返します。
		/// </summary>
		/// <param name="icon">TreeNode のアイコンの元になるアイコンを指定します。</param>
		/// <returns>生成した TreeNodeIcon を返します。</returns>
		public static explicit operator TreeNodeIcon(Gdi::Icon icon){
			TreeNodeIcon ret=new TreeNodeIcon();
			ret.SetIcon(icon);
			return ret;
		}
		/// <summary>
		/// System.Drawing.Image を元に TreeNodeIcon を初期化して返します。
		/// </summary>
		/// <param name="image">アイコンの元になる画像を指定します。</param>
		/// <returns>生成した TreeNodeIcon を返します。</returns>
		public static explicit operator TreeNodeIcon(Gdi::Image image) {
			TreeNodeIcon ret=new TreeNodeIcon();
			ret.SetImage(image);
			return ret;
		}
		//============================================================
		//		既定値
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
		/// 既定の TreeViewIcon を提供します。
		/// </summary>
		public static TreeNodeIcon File{
			get{return TreeNodeIcon.file;}
		}
		/// <summary>
		/// 既定の TreeViewIcon を提供します。
		/// </summary>
		public static TreeNodeIcon Folder{
			get{return TreeNodeIcon.folder;}
		}
		internal sealed class DefaultValueAttribute:CM::DefaultValueAttribute{
			public DefaultValueAttribute()
				:base(TreeNodeIcon.File){}
		}
		//============================================================
		//		画像の設定
		//============================================================
		/// <summary>
		/// System.Drawing.Icon でアイコンを設定します。
		/// </summary>
		/// <param name="value">設定するアイコンを指定します。</param>
		public void SetIcon(Gdi::Icon value){
			//if(type==IconType.Icon&&icon==value)return;
			this.type=IconType.Icon;
			icon=value;
			img=null;
			list=null;
		}
		/// <summary>
		/// System.Drawing.Image でアイコンを設定します。
		/// </summary>
		/// <param name="value">設定するアイコンを指定します。</param>
		public void SetImage(Gdi::Image value){
			//if(type==IconType.Image&&img==value)return;
			this.type=IconType.Image;
			img=value;
			icon=null;
			list=null;
		}
		/// <summary>
		/// 画像の一部をアイコンとして設定します。
		/// </summary>
		/// <param name="image">元となる画像を指定します。</param>
		/// <param name="rect">アイコンとして使用する画像の部分を指定する矩形を指定します。</param>
		public void SetImage(Gdi::Image image,Gdi::Rectangle rect){
			this.type=IconType.ClippedImage;
			this.img=image;
			this.rect=rect;
		}
		/// <summary>
		/// ImageList に登録されている画像をアイコンとして使用します。
		/// </summary>
		/// <param name="list">使用する画像を含んでいる ImageList を指定します。</param>
		/// <param name="index">使用する画像の ImageList の中に於ける番号を指定します。</param>
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
	/// TreeNode の Indent-Area (± や枝の部分) を表示するクラスの実装を提供します。
	/// </summary>
	public interface ITreeNodeIndentArea{
		/// <summary>
		/// ±領域の最大高さを取得します。
		/// 負の値を指定した場合には ± はノードの丁度中央の高さに表示されます。
		/// ノードの高さよりもこの値の方が大きい場合には ± はノードの丁度中央の高さに表示されます。
		/// ノードの高さの方がこの値よりも大きい場合には ± はノードの上端に合わせて描画されます。
		/// </summary>
		int Height{get;}
		/// <summary>
		/// Indent Area の幅を取得します。
		/// ±のマークを含めたインデント量を表現します。
		/// </summary>
		int Width{get;}
		/// <summary>
		/// Indent 領域の内容を描画します。
		/// </summary>
		/// <param name="g">描画する対象の Graphics を指定します。
		/// 原点は Indent 領域の右上に設定されます。</param>
		/// <param name="node">描画される node を指定します。</param>
		/// <param name="areaheight">描画する対象の領域の高さを指定します。</param>
		void DrawIndentArea(Gdi::Graphics g,TreeNode node,int areaheight);
		/// <summary>
		/// 子孫領域の為の Indent 領域を描画します。
		/// </summary>
		/// <param name="g">描画する対象の Graphics を指定します。
		/// 原点は Indent 領域の右上に設定されます。</param>
		/// <param name="node">子孫領域の親ノードを指定します。
		/// その Indent 領域と同じ Indent を受けている node です。</param>
		/// <param name="areaheight">描画する対象の領域の高さを指定します。</param>
		void DrawIndentForDescendant(Gdi::Graphics g,TreeNode node,int areaheight);
		/// <summary>
		/// 指定した点が ± の記号の上に存在しているか否かを取得又は設定します。
		/// </summary>
		/// <param name="x">局所座標での x 座標を指定します。
		/// ※ IndentArea は局所座標に於いて x≦0 です。</param>
		/// <param name="y">局所座標での y 座標を指定します。</param>
		/// <param name="height">Indent 領域の高さを指定します。
		/// これは、Indent 領域の高さに応じて ± の位置が変化する様な場合に参照されます。</param>
		/// <param name="node">IndentArea の右に描画されるノードを指定します。
		/// これは、ノードの状態によって ± の位置や大きさが変化する様な場合に参照されます。</param>
		/// <returns>指定した点が ± の記号の上にあった時に true を返します。
		/// それ以外の場合に false を返します。</returns>
		bool HitPlusMinus(int x,int y,int height,TreeNode node);
	}
	/// <summary>
	/// ITreeNodeIndentArea の標準的な実装を提供します。
	/// </summary>
	[System.Serializable]
	public sealed class TreeNodeIndentArea:ITreeNodeIndentArea{
		private Gdi::Size size;
		/// <summary>
		/// 幅を指定して TreeNodeIndentArea を初期化します。
		/// </summary>
		/// <param name="width">TreeNodeIndentArea の幅を指定します。</param>
		public TreeNodeIndentArea(int width):this(width,-1){}
		/// <summary>
		/// IndentArea の幅と高さを指定して TreeNodeIndentArea を初期化します。
		/// </summary>
		/// <param name="width">TreeNodeIndentArea の幅を指定します。</param>
		/// <param name="height">
		/// </param>
		public TreeNodeIndentArea(int width,int height){
			this.size=new Gdi::Size(width,height);
		}

		const int TWIG_LENGTH=10;

		//OK: _todo.TreeNodeCheck("Pen.Clone でちゃんと HANDLE も複製されているのか?");
		private static readonly Gdi::Pen pen_g=(Gdi::Pen)Gdi::Pens.Gray.Clone();
		static TreeNodeIndentArea(){
			pen_g.DashStyle=Gdi::Drawing2D.DashStyle.Dot;
		}

		const int ICON_H=9;
		const int ICON_W=9;

		#region ITreeNodeIndentArea メンバ
		/// <summary>
		/// ±領域の最大高さを取得します。
		/// 負の値を指定した場合には ± はノードの丁度中央の高さに表示されます。
		/// ノードの高さよりもこの値の方が大きい場合には ± はノードの丁度中央の高さに表示されます。
		/// ノードの高さの方がこの値よりも大きい場合には ± はノードの上端に合わせて描画されます。
		/// </summary>
		public int Height{
			get{return this.size.Height;}
		}
		/// <summary>
		/// Indent Area の幅を取得します。
		/// ±のマークを含めたインデント量を表現します。
		/// </summary>
		public int Width{
			get{return this.size.Width;}
		}
		/// <summary>
		/// Indent Area の大きさを取得します。
		/// Width は ± のマークを含めたインデント量を表します。
		/// Height は 最小の高さを表します。
		/// (枝の描画は実際の高さに合わせて行われます。)
		/// </summary>
		public System.Drawing.Size Size{
			get{return this.size;}
		}
		/// <summary>
		/// Indent 領域の内容を描画します。
		/// </summary>
		/// <param name="g">描画する対象の Graphics を指定します。
		/// 原点は Indent 領域の右上に設定されます。</param>
		/// <param name="node">描画される node を指定します。</param>
		/// <param name="areaheight">描画する対象の領域の高さを指定します。</param>
		public void DrawIndentArea(System.Drawing.Graphics g,TreeNode node,int areaheight){
			int oy=y_origin(areaheight);

			/* 枝模様を描く */
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
		/// 子孫領域の為の Indent 領域を描画します。
		/// </summary>
		/// <param name="g">描画する対象の Graphics を指定します。
		/// 原点は Indent 領域の右上に設定されます。</param>
		/// <param name="node">子孫領域の親ノードを指定します。
		/// その Indent 領域と同じ Indent を受けている node です。</param>
		/// <param name="areaheight">描画する対象の領域の高さを指定します。</param>
		public void DrawIndentForDescendant(System.Drawing.Graphics g,TreeNode node,int areaheight) {
			if(node.IsLastChild)return;
			g.DrawLine(pen_g,-TWIG_LENGTH,0,-TWIG_LENGTH,areaheight);
		}
		/// <summary>
		/// 指定した点が ± の記号の上に存在しているか否かを取得又は設定します。
		/// </summary>
		/// <param name="x">局所座標での x 座標を指定します。</param>
		/// <param name="y">局所座標での y 座標を指定します。</param>
		/// <param name="height">Indent 領域の高さを指定します。</param>
		/// <param name="node">IndentArea の右に描画されるノードを指定します。</param>
		/// <returns>指定した点が ± の記号の上にあった時に true を返します。</returns>
		public bool HitPlusMinus(int x,int y,int height,TreeNode node){
			y-=y_origin(height)-ICON_H/2;
			x-=-(TWIG_LENGTH+ICON_W/2);
			return 0<=x&&x<ICON_W&&0<=y&&y<ICON_H;
		}
		#endregion

		/// <summary>
		/// 描画対象領域の高さから、ノード枝の起点高さを求めます。
		/// </summary>
		/// <param name="areaheight"></param>
		/// <returns></returns>
		private int y_origin(int areaheight){
			int h=this.Height;
			if(h<0||areaheight<h)return areaheight/2;
			return h/2;
		}
		//==================================================================
		//		既定のインスタンス
		//==================================================================
		private static readonly TreeNodeIndentArea ins_def=new TreeNodeIndentArea(18);
		/// <summary>
		/// 標準の TreeNodeIndentArea インスタンスを取得します。
		/// </summary>
		public static ITreeNodeIndentArea Default{
			get{return ins_def;}
		}
		/// <summary>
		/// プロパティの既定値が TreeNodeIndentArea.Default で在る事を示します。
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
	/// Drag &amp; Drop の際のノードの動作を実装します。
	/// </summary>
	public interface ITreeNodeDDBehavior{
		/// <summary>
		/// 指定した TreeNode をドラッグ可能か否か判定し、
		/// 可能なドラッグドロップエフェクトを返します。
		/// </summary>
		/// <param name="node">判定対象の TreeNode を指定します。</param>
		/// <returns>ドラッグ可能の場合には、可能な DragDropEffects を返します。
		/// ドラッグ不可能の場合には DragDropEffects.None を返します。</returns>
		Forms::DragDropEffects GetAllowedDDE(TreeNode node);
		/// <summary>
		/// データがノードの上にドラッグされて来た時の動作を提供します。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。</param>
		void OnDrag(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// ドロップ処理を実行します。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。</param>
		void OnDrop(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// ドラッグが入ってくる時の処理を行います。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。</param>
		void OnEnter(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// ドラッグが要素の外へ出て行く時の処理を行います。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。
		/// 情報がない場合には null を指定します。</param>
		void OnLeave(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// 指定した効果に対応する特別なカーソルを取得します。
		/// 既定のカーソルを使用する場合は null を返します。
		/// </summary>
		/// <param name="node">ドロップ対象のノードを指定します。</param>
		/// <param name="effect">現在のドロップ効果を指定します。</param>
		/// <returns>ドロップ効果に対応するカーソルを返します。
		/// そのドロップ効果の既定のカーソルを使用する場合は null を返します。
		/// </returns>
		Forms::Cursor GetCursor(TreeNode node,Forms::DragDropEffects effect);
	}
	/// <summary>
	/// ドラッグドロップの対象を指定する値です。
	/// 現在マウスがあるノードからの相対関係で指定します。
	/// </summary>
	[System.Flags]
	[System.Serializable]
	public enum TreeNodeDDTarget{
		/// <summary>
		/// 自分自身がドロップの対象である事を示します。
		/// </summary>
		Self=1,
		/// <summary>
		/// 自ノードの直前に、ドロップ項目を挿入する事を示します。
		/// </summary>
		Prev=2,
		/// <summary>
		/// 自ノードの直後に、ドロップ項目を挿入する事を示します。
		/// </summary>
		Next=4,
		/// <summary>
		/// 自ノードの子要素の先頭に、ドロップ項目を追加する事を示します。
		/// </summary>
		Child=8,
	}
	/// <summary>
	/// ドラッグドロップ操作を実装する為の、便利な基本クラスです。
	/// </summary>
	[System.Serializable]
	public abstract class TreeNodeDDBehaviorBase:ITreeNodeDDBehavior{
		/// <summary>
		/// 指定した TreeNode をドラッグ可能か否か判定し、
		/// 可能なドラッグドロップエフェクトを返します。
		/// </summary>
		/// <param name="node">判定対象の TreeNode を指定します。</param>
		/// <returns>ドラッグ可能の場合には、可能な DragDropEffects を返します。
		/// ドラッグ不可能の場合には DragDropEffects.None を返します。</returns>
		public abstract Forms::DragDropEffects GetAllowedDDE(TreeNode node);
		//-------------------------------------------------------------------------
		/// <summary>
		/// ドロップ操作を実行します。
		/// </summary>
		/// <param name="node">ドロップ先のノードを指定します。</param>
		/// <param name="e">ドラッグに関する情報を指定します。</param>
		public abstract void OnDrop(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// ドラッグドロップ操作の期待される DDE を取得します。
		/// </summary>
		/// <param name="node">ドロップ先のノードを指定します。</param>
		/// <param name="e">ドラッグに関する情報を指定します。</param>
		/// <returns>ドロップによって期待される DDE を返します。</returns>
		protected abstract Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e);
		/// <summary>
		/// ドロップ先からのデータの続柄を取得します。
		/// </summary>
		/// <param name="node">ドロップ先のノードを指定します。</param>
		/// <param name="e">ドラッグに関する情報を指定します。</param>
		/// <returns>ドロップ先からのデータの続柄を返します。</returns>
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
		/// データがノードの上にドラッグされて来た時の動作を提供します。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。</param>
		public virtual void OnDrag(TreeNode node,TreeNodeDragEventArgs e){
			e.Effect=GetEffect(node,e);
			this.SetRevert(node,e);
		}
		/// <summary>
		/// ドラッグが入ってくる時の処理を行います。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。</param>
		public virtual void OnEnter(TreeNode node,TreeNodeDragEventArgs e){
			this.SetRevert(node,e);
		}
		/// <summary>
		/// ドラッグが要素の外へ出て行く時の処理を行います。
		/// </summary>
		/// <param name="node">処理対象のノードを指定します。</param>
		/// <param name="e">ドラッグの情報を指定します。
		/// 情報がない場合には null を指定します。</param>
		public virtual void OnLeave(TreeNode node,TreeNodeDragEventArgs e){
			node.View.dropArea.Clear();
		}
		/// <summary>
		/// 指定した効果に対応する特別なカーソルを取得します。
		/// 既定のカーソルを使用する場合は null を返します。
		/// </summary>
		/// <param name="node">ドロップ対象のノードを指定します。</param>
		/// <param name="effect">現在のドロップ効果を指定します。</param>
		/// <returns>ドロップ効果に対応するカーソルを返します。
		/// そのドロップ効果の既定のカーソルを使用する場合は null を返します。
		/// </returns>
		public virtual Forms::Cursor GetCursor(TreeNode node,Forms::DragDropEffects effect){return null;}
	}
	/// <summary>
	/// 静的なドラッグドロップ動作を表現するクラスです。
	/// ドラッグされるノードの状態に拘わらず、一定のドラッグドロップエフェクトを提供します。
	/// </summary>
	[System.Serializable]
	public class TreeNodeDDBehaviorStatic:TreeNodeDDBehaviorBase{
		Forms::DragDropEffects allowed;
		/// <summary>
		/// TreeNodeDDBehaviorStatic を初期化します。
		/// </summary>
		/// <param name="allowedEffects">可能な被ドラッグ操作を指定します。</param>
		public TreeNodeDDBehaviorStatic(Forms::DragDropEffects allowedEffects){
			this.allowed=allowedEffects;
		}

		/// <summary>
		/// 指定した TreeNode をドラッグ可能か否か判定し、
		/// 可能なドラッグドロップエフェクトを返します。
		/// </summary>
		/// <param name="node">判定対象の TreeNode を指定します。</param>
		/// <returns>ドラッグ可能の場合には、可能な DragDropEffects を返します。
		/// ドラッグ不可能の場合には DragDropEffects.None を返します。</returns>
		public override Forms::DragDropEffects GetAllowedDDE(TreeNode node){return this.allowed;}
		/// <summary>
		/// ドロップ操作を実行します。
		/// </summary>
		/// <param name="node">ドロップ先のノードを指定します。</param>
		/// <param name="e">ドラッグに関する情報を指定します。</param>
		public override void OnDrop(TreeNode node,TreeNodeDragEventArgs e){}
		/// <summary>
		/// ドラッグドロップ操作の期待される DDE を取得します。
		/// </summary>
		/// <param name="node">ドロップ先のノードを指定します。</param>
		/// <param name="e">ドラッグに関する情報を指定します。</param>
		/// <returns>ドロップによって期待される DDE を返します。</returns>
		protected override Forms::DragDropEffects GetEffect(TreeNode node,TreeNodeDragEventArgs e) {
			return Forms::DragDropEffects.Copy;
		}
		//============================================================
		//		静的メンバ
		//============================================================
		private static ITreeNodeDDBehavior empty=new TreeNodeDDBehaviorEmpty();
		/// <summary>
		/// 空のドラッグドロップ動作を取得します。
		/// ドラッグドロップ操作に対して何も実行しません。
		/// </summary>
		public static ITreeNodeDDBehavior Empty{
			get{return empty;}
		}

		private static ITreeNodeDDBehavior debug=new TreeNodeDDBehaviorStatic(Forms::DragDropEffects.All);
		/// <summary>
		/// 全ての被ドラッグ操作が可能な DDBehavior インスタンスです。
		/// </summary>
		public static ITreeNodeDDBehavior AllForDebug{
			get{return debug;}
		}

		/// <summary>
		/// プロパティの既定値が TreeNodeIndentArea.Default で在る事を示します。
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
		/// コントロール上の反転領域を管理します。
		/// </summary>
		internal class ReversibleArea{
			readonly Forms::Control ctrl;
			public ReversibleArea(Forms::Control ctrl){
				this.ctrl=ctrl;
			}
#if OLD
			/// <summary>
			/// 0: 反転表示されている部分はない
			/// 1: 反転表示されている直線
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
			/// 反転表示している位置を変更します。
			/// </summary>
			/// <param name="rect">新しい反転表示位置を指定します。</param>
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
			/// 現在反転している領域を解除します。
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
			/// 指定した点から右へ線を引きます。
			/// </summary>
			/// <param name="pt">線の開始点を指定します。</param>
			public void RevertLine(Gdi::Point pt){
				Gdi::Rectangle rect=new Gdi::Rectangle(
					pt.X,pt.Y-1,
					this.ctrl.ClientRectangle.Width-pt.X,3
				);
				this.RevertRect(rect);
			}
			/// <summary>
			/// 指定した矩形領域を反転します。
			/// </summary>
			/// <param name="rect">反転する領域を指定します。</param>
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
