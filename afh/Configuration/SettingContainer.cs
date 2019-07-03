namespace afh.Configuration{
	/// <summary>
	/// �ꎞ�I�Ȑݒ�̕ύX (�܂肻�̏�ŗL���ɂȂ�킯�ł͂Ȃ��ݒ�̕ύX)
	/// ���Ǘ�����ׂ̑����񋟂��܂��B
	/// </summary>
	/// <remarks>
	/// 䢂̐����ł́A���ݗL���ɂȂ��Ă���ݒ�̎����u�L���ݒ�v�ƌĂсA
	/// �ҏW�r��(�ύX��)�ł����Ė����K�p����Ă��Ȃ��ݒ�̎����u���ݒ�v�ƌĂԎ��ɂ��܂��B
	/// </remarks>
	public interface ISettingTemporary{
		/// <summary>
		/// ���ݒ肪�L���ݒ�ƈقȂ邩�ǂ���(�ҏW���ꂽ���ǂ���)���擾���܂��B
		/// </summary>
		bool IsChanged{get;}
		/// <summary>
		/// ���ݒ��L���ݒ�ɔ��f���܂��B
		/// </summary>
		void Apply();
		/// <summary>
		/// ���ݒ�����݂̗L���ݒ�ɖ߂��܂��B
		/// </summary>
		void Restore();
		/// <summary>
		/// �ݒ�̊���l�����ݒ�ɔ��f���܂��B
		/// (�L���ɂ���ɂ͂��̌�� Apply ���Ăяo���܂��B)
		/// </summary>
		void RestoreDefaults();
		/// <summary>
		/// ���ݒ�ɕύX�����������ɔ�������C�x���g�ł��B
		/// </summary>
		event afh.EventHandler<object> Changed;
		/// <summary>
		/// ���ݒ肪�K�p���ꂽ���ɔ������܂��B
		/// </summary>
		event afh.VoidEH Applied;
	}
	//CHECK: �q�R���e�i���ꂼ��ɂ� SettingKey �����蓖�Ă�l�Ȏd�g�݂ɂ���

	#region Attribute:SettingContainer
	/// <summary>
	/// SettingControl ��ێ�����R���e�i�̑����ł��B
	/// System.Windows.Forms.ContainerControl ���p������N���X�ɐݒ肵�A
	/// �\���O�� Initialize �����s����Ύ��ۂɐݒ���s�������o����l�ɂȂ�܂��B
	/// <para>
	/// SettingContainerAttribute �����N���X�̃t�B�[���h�ɂ��ݒ肷�鎖���o���܂��B
	/// ���̏ꍇ�́A���̃t�B�[���h�̌^�ł���N���X�ɂ� SettingContainerControl ���ݒ肳��Ă���K�v������܂��B
	/// <code>
	/// [SettingContainer]
	/// class Container1:System.Windows.Forms.ContainerControl{
	///		[SettingContainer]
	///		Container2 field1;
	/// }
	/// 
	/// class Container2:System.Windows.Forms.ContainerControl{}
	/// </code>
	/// </para>
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Field)]
	public class SettingContainerAttribute:System.Attribute,ISettingTemporary{
		/// <summary>
		/// �Ǘ����� ISettingTemporary �̏W����ێ����܂��B
		/// </summary>
		protected System.Collections.ArrayList list;
		/// <summary>
		/// �ݒ��ۑ������ł��� SettingKey ��ێ����܂��B
		/// </summary>
		protected internal SettingKey k;
		/// <summary>
		/// �ݒ��ۑ������ł��� SettingKey �̃p�X��ێ����܂��B
		/// �L���Ȃ̂͏��������閘�ł��B
		/// </summary>
		protected internal string keypath;
		//===========================================================
		//		private methods
		//===========================================================
		private void Add(ISettingTemporary temp){
			temp.Changed+=new afh.EventHandler<object>(attr_Changed);
			this.list.Add(temp);
		}
		private void attr_Changed(object sender,object value){this.OnChanged(sender,value);}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// SettingContainerAttribute �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="keyPath">
		/// �ݒ�̕ۑ���̃p�X���w�肵�܂��B
		/// �N���X�ɐݒ肵���ꍇ�ɂ́ARoot ����̑��΃p�X�ɂȂ�܂��B
		/// �t�B�[���h�ɐݒ肵���ꍇ�ɂ́A���̃t�B�[���h�����C���X�^���X�ɑΉ�����ۑ���ł��� SettingKey ����̑��΃p�X�ɂȂ�܂��B
		/// </param>
		public SettingContainerAttribute(string keyPath){
			this.k=null;
			this.keypath=keyPath;
		}
		/// <summary>
		/// SettingContainerAttribute �̃C���X�^���X���쐬���܂��B
		/// �N���X�ɐݒ肵���ꍇ�ɂ� Root\afh.Configuration.SettingContainer �ȉ��ɃN���X���̃L�[���쐬���Ă����ɐݒ��ۑ����܂��B
		/// �t�B�[���h�ɐݒ肵���ꍇ�ɂ́A���̃t�B�[���h�̌^�ɐ錾����Ă��� SettingContainerAttribute �̎w������ꏊ�ɕۑ����܂��B
		/// </summary>
		public SettingContainerAttribute(){
			this.k=null;
			this.keypath=null;
		}
		/// <summary>
		/// SettingContainerAttribute �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="key">�ݒ�̕ۑ���ł��� SettingKey ���w�肵�܂��B</param>
		public SettingContainerAttribute(SettingKey key){
			this.k=key;
			this.keypath=null;
		}
		//===========================================================
		//		ISettingTemporary
		//===========================================================
		/// <summary>
		/// �K�p����Ă��Ȃ��ݒ�̕ύX�����邩�ǂ������擾���܂��B
		/// </summary>
		public bool IsChanged{
			get{
				foreach(ISettingTemporary ctrl in this.list)if(ctrl.IsChanged)return true;
				return false;
			}
		}
		/// <summary>
		/// �K�p����Ă��Ȃ��ݒ�̕ύX��K�p���܂��B
		/// </summary>
		public void Apply(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.Apply();
			this.OnApplied();
		}
		/// <summary>
		/// ���ݒ�����ݗL���ɂȂ��Ă���ݒ�ɖ߂��܂��B
		/// </summary>
		public void Restore(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.Restore();
		}
		/// <summary>
		/// ���ݒ������l�ɖ߂��܂��B
		/// </summary>
		public void RestoreDefaults(){
			foreach(ISettingTemporary ctrl in this.list)ctrl.RestoreDefaults();
		}
		/// <summary>
		/// ���ݒ肪�ύX���ꂽ���ɔ������܂��B
		/// </summary>
		public event afh.EventHandler<object> Changed;
		/// <summary>
		/// Changed �C�x���g�𔭐������܂��B
		/// </summary>
		/// <param name="sender">�C�x���g�̔������̃I�u�W�F�N�g���w�肵�܂�</param>
		/// <param name="value">�ύX��̒l���w�肵�܂��B</param>
		protected virtual void OnChanged(object sender,object value){
			if(this.Changed==null)return;
			this.Changed(sender,value);
		}
		/// <summary>
		/// ���ݒ肪�K�p���ꂽ���ɔ������܂��B
		/// </summary>
		public event afh.VoidEH Applied;
		/// <summary>
		/// Applied �C�x���g�𔭐������܂��B
		/// </summary>
		protected virtual void OnApplied(){
			if(this.Applied==null)return;
			this.Applied(this);
		}
		//===========================================================
		//		�R���e�i�̏�����
		//===========================================================
		/// <summary>
		/// ���ȏ� Initialize ���s�����Ƃ������ɔ�������G���[�̐�����\���܂��B
		/// </summary>
		protected const string ERROR_DOUBLE_INIT="���ڈȍ~�� Initialize �͎󂯕t���܂���";
		/// <summary>
		/// Initialize �����s�ςł��邩�ۂ���\���܂��B
		/// </summary>
		protected bool initialized=false;
		/// <summary>
		/// �R���e�i�I�u�W�F�N�g�����̃t�B�[���h�̑���
		/// (SettingButtonAttribute, SettingControlAttribute, SettingContainerAttribute �Ȃ�)
		/// �ɏ]���ď��������܂��B
		/// (InitializeComponent �̌�Ŏ��s����l�ɂ��ĉ������B)
		/// </summary>
		/// <param name="obj">����������Ώۂ̃R���e�i</param>
		protected internal virtual void Initialize(System.Windows.Forms.ContainerControl obj){
			if(this.initialized){Application.Log.AfhOut.WriteError(ERROR_DOUBLE_INIT);return;}
			this.initialized=true;
			this.list=new System.Collections.ArrayList();

			System.Type t=obj.GetType();
			this.Initialize_k(t);
			this.Initialize_fields(obj,t);
		}
		/// <summary>
		/// ���� SettingKey k ���ݒ肳��Ă��Ȃ����ɁAk �����������܂��B
		/// </summary>
		/// <param name="t">���̑����̓K�p��ł���R���e�i�̌����Ȍ^���w�肵�܂��B</param>
		protected void Initialize_k(System.Type t){
			if(this.k!=null)return;
			if(this.keypath==null){
				this.k=Setting.Root["afh.Configuration.SettingContainer"]["T",t.FullName];
			}else{
				this.k=Setting.Root.GetKey(this.keypath);
			}
		}
		/// <summary>
		/// �t�B�[���h�����������܂��B
		/// </summary>
		/// <param name="obj">�������̑ΏۂƂȂ�R���e�i�R���g���[�����w�肵�܂��B</param>
		/// <param name="t">�R���e�i�R���g���[���̌^���w�肵�܂��B</param>
		protected void Initialize_fields(System.Windows.Forms.ContainerControl obj,System.Type t){
			System.Reflection.FieldInfo[] fs=t.GetFields(BF_FIELD);
			for(int i=0,iM=fs.Length;i<iM;i++){
				object fval=null;
				//--SettingControlAttribute
				SettingControlAttribute[] ctrls=(SettingControlAttribute[])
					fs[i].GetCustomAttributes(typeof(SettingControlAttribute),false);
				for(int j=0,jM=ctrls.Length;j<jM;j++){
					ctrls[j].SetKeyAndControl(this.k,fs[i].GetValue(obj));
					this.Add(ctrls[j]);
				}
				//--SettingButtonAttribute
				object[] attrs=fs[i].GetCustomAttributes(typeof(SettingButtonAttribute),false);
				if(attrs.Length>0){
					SettingButtonAttribute button=(SettingButtonAttribute)attrs[0];
					fval=fs[i].GetValue(obj);
					if(fval is System.Windows.Forms.Button)
						button.Initialize(this,(System.Windows.Forms.Button)fval);
				}
				//--SettingContainerAttribute
				attrs=fs[i].GetCustomAttributes(typeof(SettingContainerAttribute),false);
				if(attrs.Length>0){
					SettingContainerAttribute container=(SettingContainerAttribute)attrs[0];
					//--value of field
					if(fval==null)fval=fs[i].GetValue(obj);
					if(!(fval is System.Windows.Forms.ContainerControl)){
						Application.Log.AfhOut.WriteError("�w�肵���t�B�[���h�̌^�� System.Windows.Forms.ContainerControl ���p�����܂���B");
						Application.Log.AfhOut.AddIndent();
						Application.Log.AfhOut.WriteVar("Type",t.FullName);
						Application.Log.AfhOut.WriteVar("Field",fs[i].Name);
						Application.Log.AfhOut.WriteVar("FieldType",fs[i].FieldType.FullName);
						Application.Log.AfhOut.RemoveIndent();
						continue;
					}
					//--attrs
					attrs=fval.GetType().GetCustomAttributes(typeof(SettingContainerAttribute),false);
					if(attrs.Length==0){
						Application.Log.AfhOut.WriteError("�w�肵���t�B�[���h�̌^�ɂ� SettingContainerAttribute ���ݒ肳��Ă��܂���B");
						Application.Log.AfhOut.AddIndent();
						Application.Log.AfhOut.WriteVar("Type",t.FullName);
						Application.Log.AfhOut.WriteVar("Field",fs[i].Name);
						Application.Log.AfhOut.WriteVar("FieldType",fs[i].FieldType.FullName);
						Application.Log.AfhOut.RemoveIndent();
						continue;
					}
					//--ContainerAttribute
					if(container.k==null){
						if(container.keypath==null){
							container=(SettingContainerAttribute)attrs[0];
						}else{
							container.k=this.k.GetKey(container.keypath);
						}
					}
					container.Initialize((System.Windows.Forms.ContainerControl)fval);
					this.Add(container);
				}
			}
		}
		//===========================================================
		//		static
		//===========================================================
		private const string NOT_HAVE_ATTRIBUTE="�w�肵���I�u�W�F�N�g�ɂ� SettingContainerAttribute �������ݒ肳��Ă��܂���B";
		private const System.Reflection.BindingFlags BF_FIELD
			=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
		/// <summary>
		/// �w�肵���I�u�W�F�N�g��ݒ���s���ׂ̃t�H�[���Ƃ��ď��������܂��B
		/// </summary>
		/// <param name="obj">����������t�H�[�����̓R���e�i</param>
		public static void InitializeContainer(System.Windows.Forms.ContainerControl obj){
			System.Type t=obj.GetType();
			//--Attribute �̎擾
			SettingContainerAttribute[] attrs=(SettingContainerAttribute[])
				t.GetCustomAttributes(typeof(SettingContainerAttribute),false);
			if(attrs.Length==0)
				throw new System.ArgumentException(NOT_HAVE_ATTRIBUTE,"obj");
			attrs[0].Initialize(obj);
		}
	}
	#endregion

	#region Attribute:SettingButton
	/// <summary>
	/// SettingContainerAttribute �ɂ���ď��������ꂽ�R���e�i�ɉ����āA
	/// ���̃����o�̃{�^�����N���b�N���ꂽ���̓�����w�肵�܂��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class SettingButtonAttribute:System.Attribute{
		private SettingButtonAttribute.Operation ope;
		private ISettingTemporary parent;
		/// <summary>
		/// SettingButtonAttribute �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="operation">�{�^�����N���b�N���ꂽ���̓�����w�肵�܂��B</param>
		public SettingButtonAttribute(SettingButtonAttribute.Operation operation){
			this.ope=operation;
		}
		private void Exec(){
			if(this.parent==null)return;
			switch(this.ope){
				case Operation.OK:
				case Operation.Apply:
					this.parent.Apply();
					break;
				case Operation.Cancel:
				case Operation.Restore:
					this.parent.Restore();
					break;
				case Operation.RestoreDefaults:
					this.parent.RestoreDefaults();
					break;
			}
		}
		//===========================================================
		//		�t�b�N
		//===========================================================
		private System.Windows.Forms.Button b;
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̃C�x���g�Ƀt�b�N���܂��B
		/// TODO: Button �ȊO (event Click, bool Enabled{set;} ��ʓr�w��\��)
		/// </summary>
		/// <param name="parent">���̃{�^���ɂ���đ�����󂯂�Ώۂł��� ISettingTemporary ���w�肵�܂��B</param>
		/// <param name="obj">���̃C���X�^���X���擾�����t�B�[���h�̒l���w�肵�܂��B</param>
		public void Initialize(ISettingTemporary parent,System.Windows.Forms.Button obj){
			if(this.parent!=null)return;
			if(parent==null||obj==null)return;
			this.parent=parent;
			this.b=obj;
			if((0x10&(int)this.ope)!=0){
				this.b.Enabled=false;
				this.parent.Changed+=new afh.EventHandler<object>(parent_Changed);
				this.parent.Applied+=new VoidEH(parent_Applied);
			}
			obj.Click+=new System.EventHandler(obj_Click);
		}
		private void obj_Click(object sender,System.EventArgs e){
			this.Exec();
		}
		private void parent_Changed(object sender, object value){this.b.Enabled=this.parent.IsChanged;}
		private void parent_Applied(object sender){this.b.Enabled=this.parent.IsChanged;}


		#region /*[�v���O���������R�[�h] �C�x���g�t�b�N*/
		/*
		// .NET Framework 2.0 �ł� DynamicMethod �Ŏ��s���Ɋ֐����쐬�ł���
		/// <summary>
		/// /// obj �� e �Ŏw�肵���C�x���g�� OnChanged ���Ăяo���֐����t�b�N���܂��B
		/// <list type="table">
		/// <listheader>�Ή����Ă��� EventHandler �f���Q�[�g�̈ꗗ</listheader>
		/// <item><see cref="System.EventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ColumnClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ContentsResizedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ControlEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ConvertEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DateBoldEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DateRangeEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.DrawItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.GiveFeedbackEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.HelpEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InputLanguageChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InputLanguageChangingEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.InvalidateEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemCheckEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ItemDragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.KeyEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.KeyPressEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LabelEditEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LayoutEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LinkClickedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.LinkLabelLinkClickedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.MeasureItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.MouseEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.NavigateEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.NodeLabelEditEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PaintEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PropertyTabChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.PropertyValueChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.QueryAccessibilityHelpEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.QueryContinueDragEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ScrollEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.SelectedGridItemChangedEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.SplitterEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.StatusBarDrawItemEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.StatusBarPanelClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.ToolBarButtonClickEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.TreeViewCancelEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.TreeViewEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.UICuesEventHandler"/></item>
		/// <item><see cref="System.Windows.Forms.UpDownEventHandler"/></item>
		/// </list>
		/// </summary>
		/// protected virtual void HookEvent(object obj,System.Reflection.EventInfo e){
			System.Type t=e.EventHandlerType;
			switch(t.GetHashCode()){
				case 0xE3753C:
					if(t!=typeof(System.EventHandler))goto default;
					e.AddEventHandler(obj,new System.EventHandler(this.obj_Event));
					break;
				case 0x45EBBFC:
					if(t!=typeof(System.Windows.Forms.ColumnClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ColumnClickEventHandler(this.obj_ColumnClick));
					break;
				case 0x45EBC30:
					if(t!=typeof(System.Windows.Forms.ContentsResizedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ContentsResizedEventHandler(this.obj_ContentsResized));
					break;
				case 0x45EBC64:
					if(t!=typeof(System.Windows.Forms.ControlEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ControlEventHandler(this.obj_Control));
					break;
				case 0x45EBC98:
					if(t!=typeof(System.Windows.Forms.ConvertEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ConvertEventHandler(this.obj_Convert));
					break;
				case 0x45EBCCC:
					if(t!=typeof(System.Windows.Forms.DateBoldEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateBoldEventHandler(this.obj_DateBold));
					break;
				case 0x45EBD00:
					if(t!=typeof(System.Windows.Forms.DateRangeEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DateRangeEventHandler(this.obj_DateRange));
					break;
				case 0x45EBD34:
					if(t!=typeof(System.Windows.Forms.DragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DragEventHandler(this.obj_Drag));
					break;
				case 0x45EBD68:
					if(t!=typeof(System.Windows.Forms.DrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.DrawItemEventHandler(this.obj_DrawItem));
					break;
				case 0x45EBD9C:
					if(t!=typeof(System.Windows.Forms.GiveFeedbackEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.GiveFeedbackEventHandler(this.obj_GiveFeedback));
					break;
				case 0x45EBDD0:
					if(t!=typeof(System.Windows.Forms.HelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.HelpEventHandler(this.obj_Help));
					break;
				case 0x45EBE04:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangedEventHandler(this.obj_InputLanguageChanged));
					break;
				case 0x45EBE38:
					if(t!=typeof(System.Windows.Forms.InputLanguageChangingEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InputLanguageChangingEventHandler(this.obj_InputLanguageChanging));
					break;
				case 0x45EBE6C:
					if(t!=typeof(System.Windows.Forms.InvalidateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.InvalidateEventHandler(this.obj_Invalidate));
					break;
				case 0x45EBEA0:
					if(t!=typeof(System.Windows.Forms.ItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemChangedEventHandler(this.obj_ItemChanged));
					break;
				case 0x45EBED4:
					if(t!=typeof(System.Windows.Forms.ItemCheckEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemCheckEventHandler(this.obj_ItemCheck));
					break;
				case 0x45EBF08:
					if(t!=typeof(System.Windows.Forms.ItemDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ItemDragEventHandler(this.obj_ItemDrag));
					break;
				case 0x45EBF3C:
					if(t!=typeof(System.Windows.Forms.KeyEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyEventHandler(this.obj_Key));
					break;
				case 0x45EBF70:
					if(t!=typeof(System.Windows.Forms.KeyPressEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.KeyPressEventHandler(this.obj_KeyPress));
					break;
				case 0x45EBFA4:
					if(t!=typeof(System.Windows.Forms.LabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LabelEditEventHandler(this.obj_LabelEdit));
					break;
				case 0x45EBFD8:
					if(t!=typeof(System.Windows.Forms.LayoutEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LayoutEventHandler(this.obj_Layout));
					break;
				case 0x45EC00C:
					if(t!=typeof(System.Windows.Forms.LinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkClickedEventHandler(this.obj_LinkClicked));
					break;
				case 0x45EC040:
					if(t!=typeof(System.Windows.Forms.LinkLabelLinkClickedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.obj_LinkLabelLinkClicked));
					break;
				case 0x45EC074:
					if(t!=typeof(System.Windows.Forms.MeasureItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MeasureItemEventHandler(this.obj_MeasureItem));
					break;
				case 0x45EC0A8:
					if(t!=typeof(System.Windows.Forms.MouseEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.MouseEventHandler(this.obj_Mouse));
					break;
				case 0x45EC0DC:
					if(t!=typeof(System.Windows.Forms.NavigateEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NavigateEventHandler(this.obj_Navigate));
					break;
				case 0x45EC110:
					if(t!=typeof(System.Windows.Forms.NodeLabelEditEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.NodeLabelEditEventHandler(this.obj_NodeLabelEdit));
					break;
				case 0x45EC144:
					if(t!=typeof(System.Windows.Forms.PaintEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PaintEventHandler(this.obj_Paint));
					break;
				case 0x45EC178:
					if(t!=typeof(System.Windows.Forms.PropertyTabChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyTabChangedEventHandler(this.obj_PropertyTabChanged));
					break;
				case 0x45EC1AC:
					if(t!=typeof(System.Windows.Forms.PropertyValueChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.PropertyValueChangedEventHandler(this.obj_PropertyValueChanged));
					break;
				case 0x45EC1E0:
					if(t!=typeof(System.Windows.Forms.QueryAccessibilityHelpEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryAccessibilityHelpEventHandler(this.obj_QueryAccessibilityHelp));
					break;
				case 0x45EC214:
					if(t!=typeof(System.Windows.Forms.QueryContinueDragEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.QueryContinueDragEventHandler(this.obj_QueryContinueDrag));
					break;
				case 0x45EC248:
					if(t!=typeof(System.Windows.Forms.ScrollEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ScrollEventHandler(this.obj_Scroll));
					break;
				case 0x45EC27C:
					if(t!=typeof(System.Windows.Forms.SelectedGridItemChangedEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.obj_SelectedGridItemChanged));
					break;
				case 0x45EC2B0:
					if(t!=typeof(System.Windows.Forms.SplitterEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.SplitterEventHandler(this.obj_Splitter));
					break;
				case 0x45EC2E4:
					if(t!=typeof(System.Windows.Forms.StatusBarDrawItemEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarDrawItemEventHandler(this.obj_StatusBarDrawItem));
					break;
				case 0x45EC318:
					if(t!=typeof(System.Windows.Forms.StatusBarPanelClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.StatusBarPanelClickEventHandler(this.obj_StatusBarPanelClick));
					break;
				case 0x45EC34C:
					if(t!=typeof(System.Windows.Forms.ToolBarButtonClickEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.ToolBarButtonClickEventHandler(this.obj_ToolBarButtonClick));
					break;
				case 0x45EC380:
					if(t!=typeof(System.Windows.Forms.TreeViewCancelEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewCancelEventHandler(this.obj_TreeViewCancel));
					break;
				case 0x45EC3B4:
					if(t!=typeof(System.Windows.Forms.TreeViewEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.TreeViewEventHandler(this.obj_TreeView));
					break;
				case 0x45EC3E8:
					if(t!=typeof(System.Windows.Forms.UICuesEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UICuesEventHandler(this.obj_UICues));
					break;
				case 0x45EC41C:
					if(t!=typeof(System.Windows.Forms.UpDownEventHandler))goto default;
					e.AddEventHandler(obj,new System.Windows.Forms.UpDownEventHandler(this.obj_UpDown));
					break;
		default:
					throw new System.Exception("�w�肵���C�x���g�̎�ނɂ͑Ή����Ă��܂���");
			}
		}
		private void obj_Event(System.Object sender,System.EventArgs e){this.Exec();}
		private void obj_ColumnClick(System.Object sender,System.Windows.Forms.ColumnClickEventArgs e){this.Exec();}
		private void obj_ContentsResized(System.Object sender,System.Windows.Forms.ContentsResizedEventArgs e){this.Exec();}
		private void obj_Control(System.Object sender,System.Windows.Forms.ControlEventArgs e){this.Exec();}
		private void obj_Convert(System.Object sender,System.Windows.Forms.ConvertEventArgs e){this.Exec();}
		private void obj_DateBold(System.Object sender,System.Windows.Forms.DateBoldEventArgs e){this.Exec();}
		private void obj_DateRange(System.Object sender,System.Windows.Forms.DateRangeEventArgs e){this.Exec();}
		private void obj_Drag(System.Object sender,System.Windows.Forms.DragEventArgs e){this.Exec();}
		private void obj_DrawItem(System.Object sender,System.Windows.Forms.DrawItemEventArgs e){this.Exec();}
		private void obj_GiveFeedback(System.Object sender,System.Windows.Forms.GiveFeedbackEventArgs e){this.Exec();}
		private void obj_Help(System.Object sender,System.Windows.Forms.HelpEventArgs hlpevent){this.Exec();}
		private void obj_InputLanguageChanged(System.Object sender,System.Windows.Forms.InputLanguageChangedEventArgs e){this.Exec();}
		private void obj_InputLanguageChanging(System.Object sender,System.Windows.Forms.InputLanguageChangingEventArgs e){this.Exec();}
		private void obj_Invalidate(System.Object sender,System.Windows.Forms.InvalidateEventArgs e){this.Exec();}
		private void obj_ItemChanged(System.Object sender,System.Windows.Forms.ItemChangedEventArgs e){this.Exec();}
		private void obj_ItemCheck(System.Object sender,System.Windows.Forms.ItemCheckEventArgs e){this.Exec();}
		private void obj_ItemDrag(System.Object sender,System.Windows.Forms.ItemDragEventArgs e){this.Exec();}
		private void obj_Key(System.Object sender,System.Windows.Forms.KeyEventArgs e){this.Exec();}
		private void obj_KeyPress(System.Object sender,System.Windows.Forms.KeyPressEventArgs e){this.Exec();}
		private void obj_LabelEdit(System.Object sender,System.Windows.Forms.LabelEditEventArgs e){this.Exec();}
		private void obj_Layout(System.Object sender,System.Windows.Forms.LayoutEventArgs e){this.Exec();}
		private void obj_LinkClicked(System.Object sender,System.Windows.Forms.LinkClickedEventArgs e){this.Exec();}
		private void obj_LinkLabelLinkClicked(System.Object sender,System.Windows.Forms.LinkLabelLinkClickedEventArgs e){this.Exec();}
		private void obj_MeasureItem(System.Object sender,System.Windows.Forms.MeasureItemEventArgs e){this.Exec();}
		private void obj_Mouse(System.Object sender,System.Windows.Forms.MouseEventArgs e){this.Exec();}
		private void obj_Navigate(System.Object sender,System.Windows.Forms.NavigateEventArgs ne){this.Exec();}
		private void obj_NodeLabelEdit(System.Object sender,System.Windows.Forms.NodeLabelEditEventArgs e){this.Exec();}
		private void obj_Paint(System.Object sender,System.Windows.Forms.PaintEventArgs e){this.Exec();}
		private void obj_PropertyTabChanged(System.Object s,System.Windows.Forms.PropertyTabChangedEventArgs e){this.Exec();}
		private void obj_PropertyValueChanged(System.Object s,System.Windows.Forms.PropertyValueChangedEventArgs e){this.Exec();}
		private void obj_QueryAccessibilityHelp(System.Object sender,System.Windows.Forms.QueryAccessibilityHelpEventArgs e){this.Exec();}
		private void obj_QueryContinueDrag(System.Object sender,System.Windows.Forms.QueryContinueDragEventArgs e){this.Exec();}
		private void obj_Scroll(System.Object sender,System.Windows.Forms.ScrollEventArgs e){this.Exec();}
		private void obj_SelectedGridItemChanged(System.Object sender,System.Windows.Forms.SelectedGridItemChangedEventArgs e){this.Exec();}
		private void obj_Splitter(System.Object sender,System.Windows.Forms.SplitterEventArgs e){this.Exec();}
		private void obj_StatusBarDrawItem(System.Object sender,System.Windows.Forms.StatusBarDrawItemEventArgs sbdevent){this.Exec();}
		private void obj_StatusBarPanelClick(System.Object sender,System.Windows.Forms.StatusBarPanelClickEventArgs e){this.Exec();}
		private void obj_ToolBarButtonClick(System.Object sender,System.Windows.Forms.ToolBarButtonClickEventArgs e){this.Exec();}
		private void obj_TreeViewCancel(System.Object sender,System.Windows.Forms.TreeViewCancelEventArgs e){this.Exec();}
		private void obj_TreeView(System.Object sender,System.Windows.Forms.TreeViewEventArgs e){this.Exec();}
		private void obj_UICues(System.Object sender,System.Windows.Forms.UICuesEventArgs e){this.Exec();}
		private void obj_UpDown(System.Object source,System.Windows.Forms.UpDownEventArgs e){this.Exec();}
		//*/
		#endregion

		//*/
		/// <summary>
		/// �{�^���������ꂽ���̓�����w�肵�܂��B
		/// </summary>
		public enum Operation{
			/// <summary>
			/// ���ݒ��K�p���L���ɂ��܂��B
			/// [OK] �{�^���Ɏw�肳��鎖���z�肳��Ă��܂��B
			/// </summary>
			OK=0x00,
			/// <summary>
			/// ���ݒ��K�p���L���ɂ��܂��B�ύX���Ȃ��ꍇ�ɂ͖����\���ɂ��܂��B
			/// [�K�p] �{�^���Ɏw�肳��鎖���z�肳��Ă��܂��B
			/// </summary>
			Apply=0x10,
			/// <summary>
			/// ���ݒ�𖳌��ɂ��A���݂̐ݒ�ɖ߂��܂��B
			/// [�L�����Z��] �{�^���Ɏw�肳��鎖���z�肳��Ă��܂��B
			/// </summary>
			Cancel=0x01,
			/// <summary>
			/// ���ݒ�𖳌��ɂ��A���݂̐ݒ�ɖ߂��܂��B�ύX���Ȃ��ꍇ�ɂ͖����\���ɂ��܂��B
			/// [���݂̐ݒ�] �{�^���Ɏw�肳��鎖���z�肳��Ă��܂��B
			/// </summary>
			Restore=0x11,
			/// <summary>
			/// ���ݒ�Ƃ��Ċ���l��ݒ肵�܂��B
			/// [����l] �{�^���Ɏw�肳��鎖���z�肳��Ă��܂��B
			/// </summary>
			RestoreDefaults=0x03
		}
	}
	#endregion

	#region Attribute:SettingControl
	/// <summary>
	/// �X�̐ݒ���s���R���g���[���ɂ��鑮���̃N���X�ł��B
	/// SettingFormAttribute �ƕ����Ďg�p���Ă��������B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field,AllowMultiple=true)]
	public class SettingControlAttribute:System.Attribute,ISettingTemporary{
		//===========================================================
		//		�����t�B�[���h
		//===========================================================
		/// <summary>
		/// �ݒ�̒l�� SettingKey ��̖��O
		/// </summary>
		private string varName;
		/// <summary>
		/// ����l�B�ݒ���s��Ȃ���Ԃł̒l��\���܂��B
		/// </summary>
		private object vDefault;
		/// <summary>
		/// �R���g���[���̒l�Ƃ��Ďg�p����v���p�e�B�̖��O��ێ����܂��B
		/// </summary>
		private string propertyName;
		/// <summary>
		/// propertyName �Ŏw�肵���l�̕ύX�����o����ׂɎg�p����C�x���g�̖��O��ێ����܂��B
		/// </summary>
		private string eventName;
		/// <summary>
		/// ����l (�ݒ���s��Ȃ���Ԃł̒l) ���擾���͐ݒ肵�܂��B
		/// </summary>
		public object DefaultValue{
			get{return this.vDefault;}
			set{this.vDefault=value;}
		}
		//===========================================================
		//		�R���g���[����������̃t�B�[���h
		//===========================================================
		/// <summary>
		/// �ݒ�̕ۑ���� SettingKey
		/// </summary>
		private SettingKey key;
		/// <summary>
		/// �Ώۂ̃I�u�W�F�N�g�ւ̎Q�Ƃ�\���܂��B
		/// </summary>
		private IStringAccessor accessor;
		//===========================================================
		//		Properties
		//===========================================================
		/// <summary>
		/// �ύX��������O�̒l�A�܂�A���ݗL���ɂȂ��Ă���l���擾���܂��B
		/// </summary>
		protected object ValBefore{
			get{
				if(this.key.Var.HasVariable(this.varName)){
					string value=this.key.Var[this.varName];
					return StringConvert.To(this.accessor.ValueType,value);
				}else return this.vDefault;
			}
		}
		/// <summary>
		/// �R���g���[���ɂ���ĕύX����������̒l���擾���܂��B
		/// </summary>
		protected object ValAfter{
			get{return this.accessor.Value;}
		}
		/// <summary>
		/// �ύX��������O�̒l�A�܂�A���ݗL���ɂȂ��Ă���l�𕶎���Ƃ��Ď擾���܂��B
		/// </summary>
		protected string StrBefore{
			get{
				if(this.key.Var.HasVariable(this.varName)){
					return this.key.Var[this.varName];
				}else return StringConvert.From(this.vDefault);
			}
		}
		/// <summary>
		/// �R���g���[���ɂ���ĕύX����������̒l�𕶎���Ƃ��Ď擾���܂��B
		/// </summary>
		protected string StrAfter{get{return this.strafter;}}
		private string strafter;
		//===========================================================
		//		ISettingTemporary
		//===========================================================
		/// <summary>
		/// �R���g���[���ɂ���ĕύX���Ȃ��ꂽ���ǂ������擾���܂��B
		/// �܂�A���ݗL���ɂȂ��Ă���ݒ�ƃR���g���[���ɂ���Đݒ肳�ꂽ�l���قȂ邩�ǂ�����\���܂��B
		/// </summary>
		public bool IsChanged{get{return this.StrAfter!=this.StrBefore;}}
		/// <summary>
		/// �R���g���[���̏�Ԃ����ݗL���Ȑݒ�ɑΉ��������ɂ��܂��B
		/// </summary>
		public void Restore(){this.accessor.ValueString=this.StrBefore;}
		/// <summary>
		/// �R���g���[���̕\������ݒ������l�ɂ��܂��B
		/// </summary>
		public void RestoreDefaults(){this.accessor.Value=this.vDefault;}
		/// <summary>
		/// �R���g���[���Őݒ肵������L���ɂ��܂��B
		/// </summary>
		public void Apply(){
			if(!this.IsChanged)return;
			this.key.Var[this.varName]=this.strafter;
			this.OnApplied();
		}
		/// <summary>
		/// �R���g���[������Đݒ�̌�₪�ύX���ꂽ���ɔ�������C�x���g�ł��B
		/// </summary>
		public event afh.EventHandler<object> Changed;
		/// <summary>
		/// Change �C�x���g�𔭐������܂��B
		/// </summary>
		protected virtual void OnChanged(object sender,object value){
			this.strafter=StringConvert.From(this.accessor.ValueType,value);
			if(this.Changed==null)return;
			this.Changed(sender,value);
		}
		/// <summary>
		/// ���ݒ肪�K�p���ꂽ���ɔ������܂��B
		/// </summary>
		public event afh.VoidEH Applied;
		/// <summary>
		/// Applied �C�x���g�𔭐������܂��B
		/// </summary>
		protected virtual void OnApplied(){
			if(this.Applied==null)return;
			this.Applied(this);
		}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// ���̐ݒ���s���R���g���[���Ɋ֘A�Â�����
		/// SettingControlAttribute �̃R���X�g���N�^�B
		/// TODO: �J�X�^���R���g���[���̃N���X�ɑ�����t����B
		///     ��ԁ��������� �̕ϊ���񋟂�����𗘗p����B
		/// </summary>
		/// <param name="varName">�ݒ��ۑ������̕ϐ������w�肵�܂��B</param>
		/// <param name="propertyName">�R���g���[���̒l�Ƃ��Ďg�p����v���p�e�B�̖��O���w�肵�܂��B</param>
		/// <param name="eventName">propertyName �Ŏw�肵���l�̕ύX�����o����ׂɎg�p����C�x���g�̖��O��ێ����܂��B</param>
		public SettingControlAttribute(string varName,string propertyName,string eventName){
			this.varName=varName;
			this.propertyName=propertyName;
			this.eventName=eventName;
		}
		/// <summary>
		/// ���̐ݒ���s���R���g���[���Ɋ֘A�Â�����
		/// SettingControlAttribute �̃R���X�g���N�^�B
		/// </summary>
		/// <param name="varName">�ݒ��ۑ������̕ϐ������w�肵�܂��B</param>
		public SettingControlAttribute(string varName):this(varName,null,null){}
		//===========================================================
		//		InitializeControl
		//===========================================================
		private const string ERROR_TYPE_DEFAULT="�w�肳��Ă������l�̌^������Ă��܂��B�Ή�����^���A����ɕϊ��\�� string �Ŏw�肵�ĉ������B";
		private const string ERROR_CONVERT_DEFAULT="������Ŏw�肳�ꂽ����l��K���Ȍ^�ɕϊ��ł��܂���B�ϊ��\�ȕ�����Ŏw�肷�邩�A���̌^�Œ��ڎw�肵�ĉ������B";
		/// <summary>
		/// �ݒ��ۑ����� SettingKey �Ɛݒ�Ɏg�p����R���g���[����ݒ肵�܂��B
		/// ��x�ڂ̎w��̂ݗL���ł��B���ڈȍ~�̎w��͖�������܂��B
		/// </summary>
		/// <param name="k">�ݒ�̕ۑ�����w�肵�܂��B</param>
		/// <param name="ctrl">�ݒ�Ɏg�p����R���g���[�����͂��̑��̃I�u�W�F�N�g���w�肵�܂��B</param>
		internal void SetKeyAndControl(SettingKey k,object ctrl){
			//--��������
			if(this.key!=null)return;
			if(k==null)throw new System.ArgumentNullException("k","null �͎w��ł��܂���");
			if(ctrl==null)throw new System.ArgumentNullException("ctrl","null �͎w��ł��܂���");
			//--���
			this.key=k;
			this.accessor=SettingControlAttribute.CreateAccessor(ctrl,this.propertyName,this.eventName);
			if(this.accessor==null){
				this.key=null;
				throw new System.ArgumentException("�w�肵���R���g���[�����̓I�u�W�F�N�g�ɂ͑Ή����Ă��܂���","ctrl");
			}
			//--vDefault �̌^���킹
			if(this.vDefault==null){
				this.vDefault=this.accessor.Value;
			}else if(this.vDefault.GetType()!=this.accessor.ValueType){
				if(this.vDefault.GetType()!=typeof(string))throw new System.Exception(ERROR_TYPE_DEFAULT);
				try{this.vDefault=StringConvert.To(this.accessor.ValueType,(string)this.vDefault);}
				catch(System.Exception e){throw new System.InvalidCastException(ERROR_CONVERT_DEFAULT,e);}
			}
			//--�ݒ�
			if(this.key.Var.HasVariable(this.varName)){
				this.accessor.ValueString=this.key.Var[this.varName];
			}else{
				this.accessor.Value=this.vDefault;
				this.key.Var[this.varName]=StringConvert.From(this.accessor.ValueType,this.vDefault);
			}
			this.strafter=this.accessor.ValueString;
			this.accessor.Changed+=new afh.EventHandler<object>(this.OnChanged);
		}
		//===========================================================
		//		static
		//===========================================================
		static SettingControlAttribute(){
			PropEventHolder.Register(typeof(System.Windows.Forms.CheckBox),"Checked","CheckedChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.ComboBox),"SelectedIndex","SelectedIndexChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.DateTimePicker),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.DomainUpDown),"SelectedIndex","SelectedItemChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.HScrollBar),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.MonthCalendar),"SelectionRange","DateChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.NumericUpDown),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.RadioButton),"Checked","CheckedChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.TextBox),"Text","TextChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.TrackBar),"Value","ValueChanged");
			PropEventHolder.Register(typeof(System.Windows.Forms.VScrollBar),"Value","ValueChanged");
			//TODO>DONE: �ȉ��̕��� PropertyAccessor ���g�p���ď������s���Ă� Changed �C�x���g���������Ȃ�
			// MenuItemAccessor �� IStringAccessor ����������
			// PropEventHolder.Register(typeof(System.Windows.Forms.MenuItem),"Checked","Click");
		}

		#region Convert.IStringAccessor GetAccessor
		/// <summary>
		/// �Ώۂ̃I�u�W�F�N�g������t�����Ԃɑ΂��ĕ�����Œl���擾�E�ݒ肷���i��񋟂��܂��B
		/// </summary>
		/// <param name="obj">�Ώۂ̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="propertyName">�w�肵���I�u�W�F�N�g�̒l�Ƃ��Ďg�p����v���p�e�B�̖��O���w�肵�܂��B
		/// null ��ݒ肵���ꍇ�ɂ͊���̃v���p�e�B�ƃC�x���g�̑g�ݍ��킹���������܂��B</param>
		/// <param name="eventName">propertyName �Ŏw�肵���l�̕ύX�����o����ׂɎg�p�ł���C�x���g�̖��O���w�肵�܂��B
		/// null ��ݒ肵���ꍇ�ɂ͊���̃v���p�e�B�ƃC�x���g�̑g�ݍ��킹���������܂��B</param>
		/// <returns>Convert.IStringAccessor ����������C���X�^���X��Ԃ��܂��B
		/// �Ή����� IStringAccessor ���쐬�ł��Ȃ��ꍇ�ɂ� null ��Ԃ��܂��B</returns>
		public static IStringAccessor CreateAccessor(object obj,string propertyName,string eventName){
			//--�v���p�e�B�E�C�x���g�̎w�肪���鎞�̏ꍇ
			if(propertyName!=null&&eventName!=null)return new PropertyAccessor(obj,propertyName,eventName);
			//--ProertyAccessor �̍쐬��񂪓o�^����Ă���ꍇ
			System.Type t=obj.GetType();
			IStringAccessor ret=PropEventHolder.CreateAccessor(obj,t);
			if(ret!=null)return ret;
			//--���̑�
			switch(t.GUID.GetHashCode()&0x7fffffff){
				case 0x55B66084:
					if(t!=typeof(System.Windows.Forms.MenuItem))goto default;
					return new SettingControlAttribute.MenuItemAccessor((System.Windows.Forms.MenuItem)obj);
				case 0x5DCD9E6E:
					if(t!=typeof(System.Windows.Forms.ToolBarButton))goto default;
					return new SettingControlAttribute.ToolBarButtonAccessor((System.Windows.Forms.ToolBarButton)obj);
				case 0x497A7A3F:
					if(t!=typeof(System.Windows.Forms.CheckedListBox))goto default;
					return new SettingControlAttribute.CheckedListBoxAccessor((System.Windows.Forms.CheckedListBox)obj);
			//	case 0x10371C67:
			//		if(t!=typeof(System.Windows.Forms.MonthCalendar))goto default;
			//		return new SettingControlAttribute.MonthCalendarAccessor((System.Windows.Forms.MonthCalendar)obj);
				default:
					break;
			}
			return null;
		}
		/// <summary>
		/// PropertyInfo �� EventInfo ��ێ����A
		/// �^����ꂽ�I�u�W�F�N�g�� PropertyAccessor �C���X�^���X���쐬���܂��B
		/// </summary>
		private class PropEventHolder{
			private System.Reflection.PropertyInfo p;
			private System.Reflection.EventInfo e;
			public System.Reflection.PropertyInfo Property{get{return this.p;}}
			public System.Reflection.EventInfo Event{get{return this.e;}}
			internal PropEventHolder(System.Type type,string propertyName,string eventName){
				this.p=type.GetProperty(propertyName);
				this.e=type.GetEvent(eventName);
			}
			internal PropEventHolder(System.Type type,System.Reflection.PropertyInfo prop,System.Reflection.EventInfo ev){
				this.p=prop;
				this.e=ev;
			}
			private static readonly System.Collections.Hashtable hash=new System.Collections.Hashtable(10);
			/// <summary>
			/// �w�肵�� type �̊���� property �y�� event ��
			/// PropEventHolder �Ƃ��ēo�^���܂��B
			/// </summary>
			/// <param name="type">property �y�� event �����^���w�肵�܂��B</param>
			/// <param name="propertyName">property �̖��O���w�肵�܂��B</param>
			/// <param name="eventName">event �̖��O���w�肵�܂��B</param>
			public static void Register(System.Type type,string propertyName,string eventName){
				PropEventHolder.hash.Add(type,new PropEventHolder(type,propertyName,eventName));
			}
			/// <summary>
			/// Register ���ꂽ�������ɂ��� PropertyAccessor ���쐬���ĕԂ��܂��B
			/// </summary>
			/// <param name="obj">Access �������l������ object ���w�肵�܂��B</param>
			/// <param name="type">obj �̌^���w�肵�܂��BGetType ���s�R�X�g���팸����ׂ̈����ł��B
			/// ���̌^���w�肵�Ȃ��悤�ɒ��ӂ��Ă��������B</param>
			/// <returns>�쐬���� PropertyAccessor ��Ԃ��܂��B</returns>
			internal static IStringAccessor CreateAccessor(object obj,System.Type type){
				if(!PropEventHolder.hash.ContainsKey(type))return null;
				PropEventHolder peh=(PropEventHolder)PropEventHolder.hash[type];
				return new PropertyAccessor(obj,peh.p,peh.e);
			}
			/// <summary>
			/// Register ���ꂽ�������ɂ��� PropertyAccessor ���쐬���ĕԂ��܂��B
			/// </summary>
			/// <param name="obj">Access �������l������ object ���w�肵�܂��B</param>
			/// <returns>�쐬���� PropertyAccessor ��Ԃ��܂��B</returns>
			public static IStringAccessor CreateAccessor(object obj){
				return PropEventHolder.CreateAccessor(obj,obj.GetType());
			}
		}
		/// <summary>
		/// CheckedListBox �̏�Ԃ� bool[] �Ŕc�����Astring �ɂ��A�N�Z�X��񋟂��܂��B 
		/// </summary>
		private class CheckedListBoxAccessor:IStringAccessor{
			System.Windows.Forms.CheckedListBox box;
			string str;
			bool[] arr;
			public CheckedListBoxAccessor(System.Windows.Forms.CheckedListBox box){
				this.box=box;
				//--this.arr
				int i=box.Items.Count;
				this.arr=new bool[i];
				while(0<i--)this.arr[i]=false;
				for(i=0;i<box.SelectedIndices.Count;i++)this.arr[i]=true;
				//--this.str
				this.str=StringConvert.From(this.arr);

				this.box.ItemCheck+=new System.Windows.Forms.ItemCheckEventHandler(box_ItemCheck);
			}
			private void box_ItemCheck(object sender,System.Windows.Forms.ItemCheckEventArgs e){
				//--�ω����Ȃ���������
				if(e.NewValue==System.Windows.Forms.CheckState.Indeterminate)return;
				int i=e.Index;
				if(this.arr[i]^(e.NewValue==System.Windows.Forms.CheckState.Unchecked))return;
				//--�l�̕ύX
				bool val=this.arr[i]=e.NewValue==System.Windows.Forms.CheckState.Checked;
				this.str=this.str.Remove(i,1).Insert(i,val?"1":"0");
				this.OnChanged();
			}
			public string ValueString{
				get{return this.str;}
				set{
					//--value ����
					int iM=this.box.Items.Count;
					if(value.Length>iM)value=value.Substring(0,iM);
					else while(value.Length<iM)value+="0";
					//--�ݒ�
					this.str=value;
					this.arr=StringConvert.ToBooleanArray(value);
					for(int i=0;i<iM;i++)this.box.SetItemChecked(i,this.arr[i]);
				}
			}
			public object Value{
				get{return this.arr;}
				set{
					if(value is bool[]){
						int iM=this.box.Items.Count;
						bool[] val0=(bool[])value;
						int im=val0.Length;
						if(im==iM){
							this.arr=val0;
						}else if(im<iM){
							this.arr=new bool[iM];
							System.Array.Copy(val0,this.arr,im);
							do{this.arr[im]=false;}while(++im<iM);
						}else{
							this.arr=new bool[iM];
							System.Array.Copy(val0,this.arr,iM);
						}
						this.str=StringConvert.FromBooleanArray(this.arr);
						for(int i=0;i<iM;i++)this.box.SetItemChecked(i,this.arr[i]);
					}else throw new System.ArgumentException("bool[] �������ĉ�����","value");
				}
			}
			public System.Type ValueType{get{return typeof(bool[]);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.box,this.Value);
			}
		}
		/// <summary>
		/// MenuItem �̃`�F�b�N��Ԃɑ΂��� string �ɂ��A�N�Z�X��񋟂��܂��B 
		/// ���v���O�������� MenuItem.Checked ��G��ƕs�������N���܂�
		/// </summary>
		private class MenuItemAccessor:IStringAccessor{
			private System.Windows.Forms.MenuItem item;
			public MenuItemAccessor(System.Windows.Forms.MenuItem item){
				this.item=item;
				this.item.Click+=new System.EventHandler(item_Click);
			}
			private void item_Click(object sender, System.EventArgs e){
				this.item.Checked=!this.item.Checked;
				this.OnChanged();
			}
			public string ValueString{get{return this.item.Checked.ToString();}
				set{
					bool val=StringConvert.ToBoolean(value);
					if(val==this.item.Checked)return;
					this.item.Checked=val;
					this.OnChanged();
				}
			}
			public object Value{get{return this.item.Checked;}
				set{
					if(!(value is bool))
						throw new System.ArgumentException("�ݒ肷��l�̌^�� bool �łȂ���΂Ȃ�܂���B","value");
					bool val=(bool)value;
					if(val==this.item.Checked)return;
					this.item.Checked=val;
					this.OnChanged();
				}
			}
			public System.Type ValueType{get{return typeof(bool);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.item,this.Value);
			}
		}
		/// <summary>
		/// ToolBarButton �̉�����Ԃɑ΂��� string �ɂ��A�N�Z�X��񋟂��܂��B 
		/// ���v���O�������� ToolBarButton.Pushed ��G��ƕs�������N���܂�
		/// </summary>
		private class ToolBarButtonAccessor:IStringAccessor{
			private System.Windows.Forms.ToolBarButton button;
			private bool current;
			public ToolBarButtonAccessor(System.Windows.Forms.ToolBarButton button){
				this.button=button;
				this.current=this.button.Pushed;
				this.button.Parent.ButtonClick+=new System.Windows.Forms.ToolBarButtonClickEventHandler(Parent_ButtonClick);
			}
			private void Parent_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e){
				if(e.Button!=this.button||this.button.Pushed==this.current)return;
				this.current=this.button.Pushed=true;
			}
			public string ValueString{get{return this.current.ToString();}
				set{
					bool val=StringConvert.ToBoolean(value);
					if(val==this.current)return;
					this.button.Pushed=this.current=val;
					this.OnChanged();
				}
			}
			public object Value{get{return this.current;}
				set{
					if(!(value is bool))
						throw new System.ArgumentException("�ݒ肷��l�̌^�� bool �łȂ���΂Ȃ�܂���B","value");
					bool val=(bool)value;
					if(this.current==val)return;
					this.button.Pushed=this.current=val;
					this.OnChanged();
				}
			}
			public System.Type ValueType{get{return typeof(bool);}}
			public event afh.EventHandler<object> Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.button,this.Value);
			}
		}
#if old
		/// <summary>
		/// MonthCalendar �̑I�����Ԃɑ΂��� string �ɂ��A�N�Z�X��񋟂��܂��B 
		/// </summary>
		private class MonthCalendarAccessor:Convert.IStringAccessor{
			private System.Windows.Forms.MonthCalendar cal;
			public MonthCalendarAccessor(System.Windows.Forms.MonthCalendar calendar){
				this.cal=calendar;
				this.cal.DateChanged+=new System.Windows.Forms.DateRangeEventHandler(cal_DateChanged);
			}
			private void cal_DateChanged(object sender, System.Windows.Forms.DateRangeEventArgs e){
				this.OnChanged();
			}
			public string ValueString{
				get{return Convert.Convert.ToString(typeof(System.Windows.Forms.SelectionRange),this.cal.SelectionRange);}
				set{
					this.cal.SelectionRange=(System.Windows.Forms.SelectionRange)
						Convert.Convert.FromString(typeof(System.Windows.Forms.SelectionRange),value);
				}
			}
			public object Value{
				get{return this.cal.SelectionRange;}
				set{
					if(!(value is System.Windows.Forms.SelectionRange))
						throw new System.ArgumentException("�ݒ肷��l�̌^�� System.Windows.Forms.SelectionRange �łȂ���΂Ȃ�܂���B","value");
					this.cal.SelectionRange=(System.Windows.Forms.SelectionRange)value;
				}
			}
			public System.Type ValueType{get{return typeof(System.Windows.Forms.SelectionRange);}}
			public event afh.ObjectEH Changed;
			protected virtual void OnChanged(){
				if(this.Changed==null)return;
				this.Changed(this.cal,this.Value);
			}

		}
#endif
		#endregion
	}
	#endregion

}