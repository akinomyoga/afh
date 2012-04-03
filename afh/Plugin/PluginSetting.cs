using afh.Configuration;

namespace afh.Plugin{
	/// <summary>
	/// Plugin の設定を行う為のコンテナコントロールを管理し、ユーザの選択に応じて表示します。
	/// コンテナコントロールが afh.Application.SettingContainerControlAttribute を設定したクラスインスタンスである場合には、
	/// 初期化などの操作は自動的に行います。
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(afh.Plugin.SettingTreePanel))]
	[afh.Plugin.SettingTreePanel.SettingTreePanel()]
	public class SettingTreePanel:System.Windows.Forms.UserControl{
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components=null;
		/// <summary>
		/// SettingTreePanel のコンストラクタです。
		/// </summary>
		private SettingTreePanel(){
			InitializeComponent();
		}
		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(121, 272);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(121, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 272);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(124, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 272);
			this.panel1.TabIndex = 2;
			// 
			// SettingTreePanel
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.treeView1);
			this.Name = "SettingTreePanel";
			this.Size = new System.Drawing.Size(408, 272);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 現在有効になっている ContainerControl に対応する TreeNode を取得・設定します。
		/// </summary>
		public ContainerControlTreeNode Current{
			get{return this.current;}
			set{
				if(this.current!=null)this.current.IsCurrent=false;
				this.current=value;
				if(this.current!=null)this.current.IsCurrent=true;
				this.OnChangeControl();
			}
		}
		/// <summary>
		/// 現在有効になっている ContainerControl に対応する TreeNode を保持します。
		/// </summary>
		private ContainerControlTreeNode current=null;
		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e){
			if(e.Node is ContainerControlTreeNode){
				this.Current=(ContainerControlTreeNode)e.Node;
			}
		}
		private void OnChangeControl(){
			System.Windows.Forms.Control ctrl=this.Current.Control;
			if(ctrl==null)return;
			this.panel1.Controls.Clear();
			this.panel1.Controls.Add(ctrl);
		}
		//===========================================================
		//		UI
		//===========================================================
		/// <summary>
		/// ツリービュー部分の幅を取得亦は設定します。
		/// </summary>
		public int TreeViewWidth{
			get{return this.treeView1.Width;}
			set{this.treeView1.Width=value;}
		}
		//===========================================================
		//		Singleton
		//===========================================================
		/// <summary>
		/// SettingTreePanel に関連付けられた SettingKey を保持します。
		/// </summary>
		private static SettingKey key=Setting.Root["afh.Plugin.SettingTreePanel"];
		/// <summary>
		/// SettingTreePanel に関連付けられた SettingKey を取得します。
		/// </summary>
		public static SettingKey Key{get{return SettingTreePanel.key;}}
		/// <summary>
		/// SettingTreePanel の単一インスタンスを保持します。
		/// </summary>
		private static readonly SettingTreePanel inst=new SettingTreePanel();
		/// <summary>
		/// SettingTreePanel の単一インスタンスを取得します。
		/// </summary>
		public static SettingTreePanel Instance{get{return SettingTreePanel.inst;}}
		/// <summary>
		/// 管理下にある ISettingTemporary 集合を保持します。
		/// </summary>
		private static readonly System.Collections.ArrayList list=System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
		//===========================================================
		//		AddTreeNode
		//===========================================================
		/// <summary>
		/// 設定を行うコンテナコントロールの情報を持つ TreeNode を TreeView に登録します。
		/// </summary>
		/// <param name="node">
		/// 登録する TreeNode を指定します。
		/// node 亦は node の子孫 TreeNode の中に ContainerControlTreeNode が含まれている事が期待されます。
		/// </param>
		public static void AddTreeNode(System.Windows.Forms.TreeNode node){
			SettingTreePanel.inst.treeView1.Nodes.Add(node);
		}
		/// <summary>
		/// 設定を行うコンテナコントロールの情報を持つ TreeNode を TreeView に登録します。
		/// </summary>
		/// <param name="nodes">
		/// 登録する TreeNode[] を指定します。
		/// nodes 亦は nodes の子孫 TreeNode の中に ContainerControlTreeNode が含まれている事が期待されます。
		/// </param>
		public static void AddTreeNode(System.Windows.Forms.TreeNode[] nodes){
			SettingTreePanel.inst.treeView1.Nodes.AddRange(nodes);
		}
		//===========================================================
		//		event:Changed
		//===========================================================
		/// <summary>
		/// 登録されているコントロール (ISettingTemporary) を通じて設定の変更があった時に発生します。
		/// </summary>
		public static event afh.EventHandler<object> Changed;
		/// <summary>
		/// Changed イベントを発生させます。
		/// </summary>
		/// <param name="sender">設定の変更に使用したコントロールなどを指定します。</param>
		/// <param name="value">変更後の設定値を提供します。</param>
		private static void OnChanged(object sender,object value){
			if(SettingTreePanel.Changed==null)return;
			SettingTreePanel.Changed(sender,value);
		}
		/// <summary>
		/// コンテナコントロールなど ISettingTemporary を登録します。
		/// </summary>
		/// <param name="temp">登録する ISettingTemporary</param>
		internal static void AddSettingTemporary(ISettingTemporary temp){
			temp.Changed+=new afh.EventHandler<object>(OnChanged);
			SettingTreePanel.list.Add(temp);
		}
		//===========================================================
		//		attr:SettingContainer'
		//===========================================================
		[System.AttributeUsage(System.AttributeTargets.Class)]
		internal sealed class SettingTreePanelAttribute:afh.Configuration.SettingContainerAttribute{
			public SettingTreePanelAttribute():base(SettingTreePanel.key){}
			/// <summary>
			/// 本来は指定したオブジェクトを初期化する為のメソッドですが、
			/// この属性は (初めから初期化してある) シングルトンに対する物なので、
			/// list を取得して Changed イベントにフックするだけです。
			/// </summary>
			/// <param name="obj">
			/// 指定しても意味はありません。
			/// (形式的には SettingTreePanel.inst が指定されるべきです)
			/// </param>
			protected internal override void Initialize(System.Windows.Forms.ContainerControl obj){
				if(this.initialized){System.Console.Error.WriteLine(ERROR_DOUBLE_INIT);return;}
				this.initialized=true;
				this.list=SettingTreePanel.list;
				SettingTreePanel.Changed+=new afh.EventHandler<object>(this.SettingTreePanel_Changed);
			}
			private void SettingTreePanel_Changed(object sender,object value){this.OnChanged(sender,value);}
		}
	}
	/// <summary>
	/// コンテナコントロールをデータとして持つ TreeNode です。
	/// SettingTreePanel の TreeView に登録する為に使用します。
	/// </summary>
	public class ContainerControlTreeNode:System.Windows.Forms.TreeNode{
		private readonly System.Windows.Forms.ContainerControl ctrl;
		/// <summary>
		/// この TreeNode に関連付けられているコンテナコントロールを取得します。
		/// </summary>
		public System.Windows.Forms.ContainerControl Control{get{return this.ctrl;}}
		/// <summary>
		/// 「現在コンテナコントロールが選択されている」状態を設定します。
		/// これによって、外観を変えユーザに選択状態が分かる様にします。
		/// </summary>
		public bool IsCurrent{
			set{
				if(value){
					this.ForeColor=System.Drawing.Color.Red;
				}else{
					this.ForeColor=System.Drawing.SystemColors.ControlText;
				}
			}
		}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// ContainerControlTreeNode のコンストラクタ。
		/// </summary>
		/// <param name="name">TreeNode に表示する文字列を指定して下さい。</param>
		/// <param name="ctrl">
		/// TreeNode に関連付けるコンテナコントロールを指定して下さい。
		/// SettingContainerControl.Initialize による初期化は行っていない状態で指定して下さい。
		/// </param>
		public ContainerControlTreeNode(string name,System.Windows.Forms.ContainerControl ctrl):base(name){
			this.ctrl=ctrl;
			this.initialize_ctrl();
		}
		/// <summary>
		/// ContainerControlTreeNode のコンストラクタ。
		/// </summary>
		/// <param name="name">TreeNode に表示する文字列を指定して下さい。</param>
		/// <param name="t">
		/// 関連付けるコンテナコントロールの型を指定して下さい。
		/// 型は System.Windows.Forms.ContainerControl を継承し、
		/// 亦、既定の public コンストラクタを持っていなければ為りません。
		/// </param>
		public ContainerControlTreeNode(string name,System.Type t):base(name){
			if(!t.IsSubclassOf(typeof(System.Windows.Forms.ContainerControl)))
				throw new System.ArgumentException("System.Windows.Forms.ContainerControl を継承する型を指定して下さい","t");
			System.Reflection.ConstructorInfo ctor=t.GetConstructor(new System.Type[]{});
			if(ctor==null)throw new System.ArgumentException("指定した System.Type は既定の public コンストラクタを持ちません。","t");
			this.ctrl=(System.Windows.Forms.ContainerControl)ctor.Invoke(new object[]{});
			this.initialize_ctrl();
		}
		/// <summary>
		/// this.ctrl が SettingContainerAttribute を設定されている時、
		/// this.ctrl の初期化を行います。
		/// </summary>
		private void initialize_ctrl(){
			object[] attrs=this.ctrl.GetType().GetCustomAttributes(typeof(SettingContainerAttribute),false);
			if(attrs.Length==0)return;
			SettingContainerAttribute container=(SettingContainerAttribute)attrs[0];
			if(container.k==null){
				if(container.keypath==null){
					System.Type t=this.ctrl.GetType();
					container.k=SettingTreePanel.Key[t.FullName+"+"+t.GUID.ToString("N")];
				}else{
					container.k=SettingTreePanel.Key.GetKey(container.keypath);
				}
			}
			container.Initialize(this.ctrl);
			SettingTreePanel.AddSettingTemporary(container);
		}
	}
	/// <summary>
	/// Plugin 用の SettingContainer を保持する
	/// TreeNode を取得する為のメソッドである事を示す為の属性です。
	/// プラグイン読込時にメソッドを実行し、結果を afh.Plugin.SettingTreePanel に登録します。
	/// </summary>
	/// <remarks>
	/// 属性の指定先の条件: 1. 静的メソッド 2.シグニチャは TreeNode() 亦は TreeNode[]()
	/// </remarks>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class PluginSettingTreeNodeAttribute:PluginReaderAttribute{
		/// <summary>
		/// PluginSettingTreeNodeAttribute のコンストラクタです。
		/// </summary>
		public PluginSettingTreeNodeAttribute(){}
		/// <summary>
		/// <see cref="ContainerControlTreeNode"/> を保持する
		/// TreeNode を取得し、<see cref="SettingTreePanel"/> に登録します。
		/// </summary>
		/// <param name="mem">
		/// <see cref="ContainerControlTreeNode"/> を保持する
		/// TreeNode を取得する為のメソッドを指定します。
		/// </param>
		protected override void Read(System.Reflection.MemberInfo mem){
			if(mem.MemberType!=System.Reflection.MemberTypes.Method){
				afh.Application.Log.AfhOut.WriteLine("    error:メソッドを指定して下さい");
				return;
			}
			System.Reflection.MethodInfo m=(System.Reflection.MethodInfo)mem;
			if(m.GetParameters().Length>0){
				afh.Application.Log.AfhOut.WriteLine("    error:指定したメソッドはパラメータを要求します。パラメータを要求しないメソッドを指定して下さい。");
				return;
			}
			object result=m.Invoke(null,new object[]{});
			if(result is System.Windows.Forms.TreeNode){
				Plugin.SettingTreePanel.AddTreeNode((System.Windows.Forms.TreeNode)result);
			}else if(result is System.Windows.Forms.TreeNode[]){
				Plugin.SettingTreePanel.AddTreeNode((System.Windows.Forms.TreeNode[])result);
			}else{
				afh.Application.Log.AfhOut.WriteLine(ERROR_CAST);
			}
		}
		private const string ERROR_CAST=@"    error:メソッドの返値を適切な型に変換する事が出来ませんでした。
          メソッドの返値は以下の内のどちらかに変換できる事が出来なければ為りません。
          ・System.Windows.Forms.TreeNode
          ・System.Windows.Forms.TreeNode[]";
	}
}
