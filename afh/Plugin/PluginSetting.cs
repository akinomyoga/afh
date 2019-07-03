using afh.Configuration;

namespace afh.Plugin{
	/// <summary>
	/// Plugin �̐ݒ���s���ׂ̃R���e�i�R���g���[�����Ǘ����A���[�U�̑I���ɉ����ĕ\�����܂��B
	/// �R���e�i�R���g���[���� afh.Application.SettingContainerControlAttribute ��ݒ肵���N���X�C���X�^���X�ł���ꍇ�ɂ́A
	/// �������Ȃǂ̑���͎����I�ɍs���܂��B
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(afh.Plugin.SettingTreePanel))]
	[afh.Plugin.SettingTreePanel.SettingTreePanel()]
	public class SettingTreePanel:System.Windows.Forms.UserControl{
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components=null;
		/// <summary>
		/// SettingTreePanel �̃R���X�g���N�^�ł��B
		/// </summary>
		private SettingTreePanel(){
			InitializeComponent();
		}
		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#region �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
		/// ���ݗL���ɂȂ��Ă��� ContainerControl �ɑΉ����� TreeNode ���擾�E�ݒ肵�܂��B
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
		/// ���ݗL���ɂȂ��Ă��� ContainerControl �ɑΉ����� TreeNode ��ێ����܂��B
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
		/// �c���[�r���[�����̕����擾���͐ݒ肵�܂��B
		/// </summary>
		public int TreeViewWidth{
			get{return this.treeView1.Width;}
			set{this.treeView1.Width=value;}
		}
		//===========================================================
		//		Singleton
		//===========================================================
		/// <summary>
		/// SettingTreePanel �Ɋ֘A�t����ꂽ SettingKey ��ێ����܂��B
		/// </summary>
		private static SettingKey key=Setting.Root["afh.Plugin.SettingTreePanel"];
		/// <summary>
		/// SettingTreePanel �Ɋ֘A�t����ꂽ SettingKey ���擾���܂��B
		/// </summary>
		public static SettingKey Key{get{return SettingTreePanel.key;}}
		/// <summary>
		/// SettingTreePanel �̒P��C���X�^���X��ێ����܂��B
		/// </summary>
		private static readonly SettingTreePanel inst=new SettingTreePanel();
		/// <summary>
		/// SettingTreePanel �̒P��C���X�^���X���擾���܂��B
		/// </summary>
		public static SettingTreePanel Instance{get{return SettingTreePanel.inst;}}
		/// <summary>
		/// �Ǘ����ɂ��� ISettingTemporary �W����ێ����܂��B
		/// </summary>
		private static readonly System.Collections.ArrayList list=System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
		//===========================================================
		//		AddTreeNode
		//===========================================================
		/// <summary>
		/// �ݒ���s���R���e�i�R���g���[���̏������� TreeNode �� TreeView �ɓo�^���܂��B
		/// </summary>
		/// <param name="node">
		/// �o�^���� TreeNode ���w�肵�܂��B
		/// node ���� node �̎q�� TreeNode �̒��� ContainerControlTreeNode ���܂܂�Ă��鎖�����҂���܂��B
		/// </param>
		public static void AddTreeNode(System.Windows.Forms.TreeNode node){
			SettingTreePanel.inst.treeView1.Nodes.Add(node);
		}
		/// <summary>
		/// �ݒ���s���R���e�i�R���g���[���̏������� TreeNode �� TreeView �ɓo�^���܂��B
		/// </summary>
		/// <param name="nodes">
		/// �o�^���� TreeNode[] ���w�肵�܂��B
		/// nodes ���� nodes �̎q�� TreeNode �̒��� ContainerControlTreeNode ���܂܂�Ă��鎖�����҂���܂��B
		/// </param>
		public static void AddTreeNode(System.Windows.Forms.TreeNode[] nodes){
			SettingTreePanel.inst.treeView1.Nodes.AddRange(nodes);
		}
		//===========================================================
		//		event:Changed
		//===========================================================
		/// <summary>
		/// �o�^����Ă���R���g���[�� (ISettingTemporary) ��ʂ��Đݒ�̕ύX�����������ɔ������܂��B
		/// </summary>
		public static event afh.EventHandler<object> Changed;
		/// <summary>
		/// Changed �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="sender">�ݒ�̕ύX�Ɏg�p�����R���g���[���Ȃǂ��w�肵�܂��B</param>
		/// <param name="value">�ύX��̐ݒ�l��񋟂��܂��B</param>
		private static void OnChanged(object sender,object value){
			if(SettingTreePanel.Changed==null)return;
			SettingTreePanel.Changed(sender,value);
		}
		/// <summary>
		/// �R���e�i�R���g���[���Ȃ� ISettingTemporary ��o�^���܂��B
		/// </summary>
		/// <param name="temp">�o�^���� ISettingTemporary</param>
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
			/// �{���͎w�肵���I�u�W�F�N�g������������ׂ̃��\�b�h�ł����A
			/// ���̑����� (���߂��珉�������Ă���) �V���O���g���ɑ΂��镨�Ȃ̂ŁA
			/// list ���擾���� Changed �C�x���g�Ƀt�b�N���邾���ł��B
			/// </summary>
			/// <param name="obj">
			/// �w�肵�Ă��Ӗ��͂���܂���B
			/// (�`���I�ɂ� SettingTreePanel.inst ���w�肳���ׂ��ł�)
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
	/// �R���e�i�R���g���[�����f�[�^�Ƃ��Ď��� TreeNode �ł��B
	/// SettingTreePanel �� TreeView �ɓo�^����ׂɎg�p���܂��B
	/// </summary>
	public class ContainerControlTreeNode:System.Windows.Forms.TreeNode{
		private readonly System.Windows.Forms.ContainerControl ctrl;
		/// <summary>
		/// ���� TreeNode �Ɋ֘A�t�����Ă���R���e�i�R���g���[�����擾���܂��B
		/// </summary>
		public System.Windows.Forms.ContainerControl Control{get{return this.ctrl;}}
		/// <summary>
		/// �u���݃R���e�i�R���g���[�����I������Ă���v��Ԃ�ݒ肵�܂��B
		/// ����ɂ���āA�O�ς�ς����[�U�ɑI����Ԃ�������l�ɂ��܂��B
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
		/// ContainerControlTreeNode �̃R���X�g���N�^�B
		/// </summary>
		/// <param name="name">TreeNode �ɕ\�����镶������w�肵�ĉ������B</param>
		/// <param name="ctrl">
		/// TreeNode �Ɋ֘A�t����R���e�i�R���g���[�����w�肵�ĉ������B
		/// SettingContainerControl.Initialize �ɂ�鏉�����͍s���Ă��Ȃ���ԂŎw�肵�ĉ������B
		/// </param>
		public ContainerControlTreeNode(string name,System.Windows.Forms.ContainerControl ctrl):base(name){
			this.ctrl=ctrl;
			this.initialize_ctrl();
		}
		/// <summary>
		/// ContainerControlTreeNode �̃R���X�g���N�^�B
		/// </summary>
		/// <param name="name">TreeNode �ɕ\�����镶������w�肵�ĉ������B</param>
		/// <param name="t">
		/// �֘A�t����R���e�i�R���g���[���̌^���w�肵�ĉ������B
		/// �^�� System.Windows.Forms.ContainerControl ���p�����A
		/// ���A����� public �R���X�g���N�^�������Ă��Ȃ���Έׂ�܂���B
		/// </param>
		public ContainerControlTreeNode(string name,System.Type t):base(name){
			if(!t.IsSubclassOf(typeof(System.Windows.Forms.ContainerControl)))
				throw new System.ArgumentException("System.Windows.Forms.ContainerControl ���p������^���w�肵�ĉ�����","t");
			System.Reflection.ConstructorInfo ctor=t.GetConstructor(new System.Type[]{});
			if(ctor==null)throw new System.ArgumentException("�w�肵�� System.Type �͊���� public �R���X�g���N�^�������܂���B","t");
			this.ctrl=(System.Windows.Forms.ContainerControl)ctor.Invoke(new object[]{});
			this.initialize_ctrl();
		}
		/// <summary>
		/// this.ctrl �� SettingContainerAttribute ��ݒ肳��Ă��鎞�A
		/// this.ctrl �̏��������s���܂��B
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
	/// Plugin �p�� SettingContainer ��ێ�����
	/// TreeNode ���擾����ׂ̃��\�b�h�ł��鎖�������ׂ̑����ł��B
	/// �v���O�C���Ǎ����Ƀ��\�b�h�����s���A���ʂ� afh.Plugin.SettingTreePanel �ɓo�^���܂��B
	/// </summary>
	/// <remarks>
	/// �����̎w���̏���: 1. �ÓI���\�b�h 2.�V�O�j�`���� TreeNode() ���� TreeNode[]()
	/// </remarks>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public sealed class PluginSettingTreeNodeAttribute:PluginReaderAttribute{
		/// <summary>
		/// PluginSettingTreeNodeAttribute �̃R���X�g���N�^�ł��B
		/// </summary>
		public PluginSettingTreeNodeAttribute(){}
		/// <summary>
		/// <see cref="ContainerControlTreeNode"/> ��ێ�����
		/// TreeNode ���擾���A<see cref="SettingTreePanel"/> �ɓo�^���܂��B
		/// </summary>
		/// <param name="mem">
		/// <see cref="ContainerControlTreeNode"/> ��ێ�����
		/// TreeNode ���擾����ׂ̃��\�b�h���w�肵�܂��B
		/// </param>
		protected override void Read(System.Reflection.MemberInfo mem){
			if(mem.MemberType!=System.Reflection.MemberTypes.Method){
				afh.Application.Log.AfhOut.WriteLine("    error:���\�b�h���w�肵�ĉ�����");
				return;
			}
			System.Reflection.MethodInfo m=(System.Reflection.MethodInfo)mem;
			if(m.GetParameters().Length>0){
				afh.Application.Log.AfhOut.WriteLine("    error:�w�肵�����\�b�h�̓p�����[�^��v�����܂��B�p�����[�^��v�����Ȃ����\�b�h���w�肵�ĉ������B");
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
		private const string ERROR_CAST=@"    error:���\�b�h�̕Ԓl��K�؂Ȍ^�ɕϊ����鎖���o���܂���ł����B
          ���\�b�h�̕Ԓl�͈ȉ��̓��̂ǂ��炩�ɕϊ��ł��鎖���o���Ȃ���Έׂ�܂���B
          �ESystem.Windows.Forms.TreeNode
          �ESystem.Windows.Forms.TreeNode[]";
	}
}
