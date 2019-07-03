using Path=afh.Application.Path;

namespace afh.Configuration{
	/// <summary>
	/// �ݒ��ۑ������肷��ׂ� XML �t�@�C�����Ǘ�����N���X�ł��B
	/// <para>�w�肵�� filepath �Ƀt�@�C�������݂��Ȃ��ꍇ�́A�����ɐV�����t�@�C�����쐬���܂��B
	/// �V�����쐬�����t�@�C���̓��e�́Adefaultontent �ɂȂ�܂��B</para>
	/// <para>���Ƀt�@�C�������݂��Ă��āA�ǂݍ��݂Ɏ��s�����ꍇ�ɂ́Adefaultcontent �̓��e��ǂݍ��݂܂��B
	/// ���Ƃ��Ƃ������t�@�C���͕ۑ������Ȃ�����㏑�����邱�Ƃ͂���܂���B</para>
	/// </summary>
	public class Setting:System.Xml.XmlDocument{
		//private const string SETTING_FILENAME="setting.cfg";
		private static readonly string SETTING_FILENAME=System.IO.Path.GetFileNameWithoutExtension(Path.ExecutableFileName)+".cfg";
		internal const string XMLNS_KEY="http://www.example.com/key/";
		private const string DEFAULT_SETTING="<?xml version=\"1.0\"?>\n<setting key:class=\"afh.Configuration.Setting\" xmlns:key=\"{0}\"></setting>";
		/// <summary>
		/// �ݒ�̕ۑ���
		/// </summary>
		private string filepath;
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// Single instance
		/// </summary>
		private static readonly Setting xdoc=new afh.Configuration.Setting();
		private static readonly SettingKey root;
		/// <summary>
		/// Setting �̃R���X�g���N�^
		/// </summary>
		private Setting(){
			this.filepath=System.IO.Path.Combine(Path.ExecutableDirectory,SETTING_FILENAME);
			//--�ۑ���̃t�@�C�������݂��Ȃ��Ƃ�
			if(!System.IO.File.Exists(this.filepath))this.makeXml();
			//--�ǂݍ���
			try{this.Load(this.filepath);}
			catch{
				ConfirmOverwriteSetting.Result r=ConfirmOverwriteSetting.Confirm();
				if(r.overwrite){
					if(r.backup)System.IO.File.Move(this.filepath,Path.GetAvailablePath(this.filepath,"bk"));
					this.makeXml();
				}else this.filepath="";
				this.LoadXml(DEFAULT_SETTING);
			}
		}
		private void makeXml(){
			System.IO.StreamWriter sw=new System.IO.StreamWriter(this.filepath);
			sw.WriteLine(DEFAULT_SETTING,XMLNS_KEY);
			sw.Close();
		}
		/// <summary>
		/// Setting �̃f�X�g���N�^�ł��B���݂̐ݒ��ۑ����܂��B
		/// </summary>
		~Setting(){this.Save();}
		//===========================================================
		//		����
		//===========================================================
		static Setting(){
			Setting.root=new SettingKey(Setting.xdoc.DocumentElement);
		}
		/// <summary>
		/// ���݂̐ݒ��ۑ����܂��B
		/// </summary>
		public void Save(){
			if(this.filepath==""){
				ConfirmOverwriteSetting.Result r=Configuration.ConfirmOverwriteSetting.Confirm2();
				if(!r.overwrite)return;
				this.filepath=System.IO.Path.Combine(Path.ExecutableDirectory,SETTING_FILENAME);
				if(r.backup)System.IO.File.Move(this.filepath,Path.GetAvailablePath(this.filepath,"bk"));
			}
			base.Save(this.filepath);
		}
		/// <summary>
		/// ���[�g�̐ݒ�L�[���擾���܂��B
		/// </summary>
		public static SettingKey Root{get{return Setting.root;}}
	}

	#region cls:SettingKey
	/// <summary>
	/// �ݒ�l��ێ����� Key ��\���N���X�ł��BXML �v�f�ւ̃A�N�Z�X��񋟂��鎖�Ŏ������Ă��܂��B
	/// </summary>
	public class SettingKey{
		private System.Xml.XmlElement elem;
		private const string DEFAULT_CLASS="key";
		//===========================================================
		//		����̒l
		//===========================================================
		/// <summary>
		/// ���� key �̕��ނ��擾���܂��BXML �v�f�̗v�f���ɑΉ����܂��B
		/// </summary>
		public string Class{
			get{return this.elem.Name;}
		}
		/// <summary>
		/// ���� key ����肷�閼�O���擾���܂��BXML �v�f�� key:name �����ɑΉ����܂��B
		/// </summary>
		public string Name{
			get{return this.elem.GetAttribute("name",Setting.XMLNS_KEY);}
		}
		/// <summary>
		/// ���� key �̊���̒l���擾���܂�
		/// </summary>
		public string Value{
			get{return this.elem.GetAttribute("value",Setting.XMLNS_KEY);}
			set{this.elem.SetAttribute("value",Setting.XMLNS_KEY,value);}
		}
		//===========================================================
		//		�l
		//===========================================================
		private VarCollection vars;
		/// <summary>
		/// ���� Key �̕ێ�����l�ւ̃A�N�Z�X��񋟂��܂��B
		/// </summary>
		public VarCollection Var{get{return this.vars;}}
		/// <summary>
		/// SettingKey ���ێ�����l�ւ̃A�N�Z�X��񋟂���N���X�ł��B
		/// </summary>
		public class VarCollection:System.Collections.IEnumerable{
			private SettingKey p;
			/// <summary>
			/// VarCollection �̃R���X�g���N�^�B
			/// </summary>
			/// <param name="p">�Ή����� SettingKey ���w�肵�܂��B</param>
			internal VarCollection(SettingKey p){this.p=p;}
			/// <summary>
			/// �w�肵�����O�̒l�̃f�[�^���擾���͐ݒ肵�܂��B
			/// </summary>
			public string this[string var]{
				get{return this.p.elem.GetAttribute(escAttrName(var));}
				set{this.p.elem.SetAttribute(escAttrName(var),value);}
			}
			/// <summary>
			/// �l�� System.Xml.XmlAttribute �Ƃ��ė񋓂���񋓎q���擾���܂��B
			/// </summary>
			/// <returns>�l�̗񋓎q��Ԃ��܂��B</returns>
			public System.Collections.IEnumerator GetEnumerator(){
				return new VarEnumerator(this.p.elem);
			}
			/// <summary>
			/// �w�肵���l�����ɐݒ肳��Ă��邩�ǂ������擾���܂��B
			/// </summary>
			/// <param name="var">���ɐݒ肳��Ă��邩�ǂ������m���߂�l�̖��O���w�肵�܂��B</param>
			/// <returns>�w�肵���l�����ɐݒ肳��Ă���ꍇ�� true ��Ԃ��܂��B
			/// �w�肵���l���ݒ肳��Ă��Ȃ��ꍇ�ɂ� false ��Ԃ��܂��B</returns>
			public bool HasVariable(string var){
				return this.p.elem.HasAttribute(escAttrName(var));
			}
			private static string escAttrName(string name){
				return name
					.Replace(":","-colon-")
					.Replace("+","-plus-")
					.Replace(" ","-space-");
			}
			//---------------------------------------------
			//		Get Value
			//---------------------------------------------
			/// <summary>
			/// �w�肵�����O�̃f�[�^���A�w�肵���^�Ŏ擾���܂��B
			/// </summary>
			/// <typeparam name="T">�擾����f�[�^�̌^���w�肵�܂��B</typeparam>
			/// <param name="var">�f�[�^���i�[����ϐ��̖��O���w�肵�܂��B</param>
			/// <returns>����������w�肵���^�ɕϊ����ĕԂ��܂��B</returns>
			public T GetValue<T>(string var){
				string value=this[var];
				try{
					return afh.StringConvert.To<T>(value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
					l.AddIndent();
					l.WriteVar("Message",e.Message);
					l.WriteVar("Target Type",typeof(T).ToString());
					l.WriteVar("Var Name",var);
					l.WriteVar("Value",value);
					l.RemoveIndent();
					throw;
				}
			}
			/// <summary>
			/// �w�肵�����O�̃f�[�^���A�w�肵���^�Ŏ擾���܂��B
			/// </summary>
			/// <param name="t">�擾����f�[�^�̌^���w�肵�܂��B</param>
			/// <param name="var">�f�[�^���i�[����ϐ��̖��O���w�肵�܂��B</param>
			/// <returns>����������w�肵���^�ɕϊ����ĕԂ��܂��B</returns>
			/// <exception cref="System.ArgumentException">
			/// �����񂩂�w�肵���^�ւ̕ϊ��Ɏ��s�����ꍇ�ɔ������܂��B
			/// afh.Convert.Convert �őΉ����Ă���^�ւ̕ϊ����g�p���ĉ������B
			/// </exception>
			public object GetValue(System.Type t,string var){
				string value=this[var];
				try{
					return afh.StringConvert.To(t,value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
					l.AddIndent();
					l.WriteVar("Message",e.Message);
					l.WriteVar("Target Type",t.ToString());
					l.WriteVar("Var Name",var);
					l.WriteVar("Value",value);
					l.RemoveIndent();
					throw;
				}
			}
			//---------------------------------------------
			//		Set Value
			//---------------------------------------------
			/// <summary>
			/// �w�肵���ϐ��Ɏw�肵���^�̃I�u�W�F�N�g��ݒ肵�܂��B
			/// </summary>
			/// <typeparam name="T">�擾����f�[�^�̌^���w�肵�܂��B</typeparam>
			/// <param name="var">�f�[�^���i�[����ϐ��̖��O���w�肵�܂��B</param>
			/// <param name="value">�w�肵���I�u�W�F�N�g�𕶎���ɕϊ����ĕԂ��܂��B</param>
			/// <exception cref="System.ArgumentException">
			/// �w�肵���f�[�^�̌^���當����^�ւ̕ϊ����o���Ȃ������ꍇ�ɔ������܂��B
			/// afh.Convert.Convert �ł̕ϊ��ɑΉ����Ă���^���g�p���ĉ������B
			/// </exception>
			public void SetValue<T>(string var,T value){
				string v;
				try{
					v=afh.StringConvert.FromAs<T>(value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
					l.AddIndent();
					l.WriteVar("Message",e.Message);
					l.WriteVar("Source Type",typeof(T).Name);
					l.WriteVar("Var Name",var);
					l.WriteVar("Value",value.ToString());
					l.RemoveIndent();
					throw;
				}
				this[var]=v;
			}
			/// <summary>
			/// �w�肵���ϐ��Ɏw�肵���^�̃I�u�W�F�N�g��ݒ肵�܂��B
			/// </summary>
			/// <param name="var">�f�[�^���i�[����ϐ��̖��O���w�肵�܂��B</param>
			/// <param name="t">�擾����f�[�^�̌^���w�肵�܂��B</param>
			/// <param name="value">�w�肵���I�u�W�F�N�g�𕶎���ɕϊ����ĕԂ��܂��B</param>
			/// <exception cref="System.ArgumentException">
			/// �w�肵���f�[�^�̌^���當����^�ւ̕ϊ����o���Ȃ������ꍇ�ɔ������܂��B
			/// afh.Convert.Convert �ł̕ϊ��ɑΉ����Ă���^���g�p���ĉ������B
			/// </exception>
			public void SetValue(string var,System.Type t,object value){
				string v;
				try{
					v=afh.StringConvert.From(t,value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
					l.AddIndent();
					l.WriteVar("Message",e.Message);
					l.WriteVar("Source Type",t.Name);
					l.WriteVar("Var Name",var);
					l.WriteVar("Value",value.ToString());
					l.RemoveIndent();
					throw;
				}
				this[var]=v;
			}
			/// <summary>
			/// �w�肵���ϐ��ɃI�u�W�F�N�g��ݒ肵�܂��B
			/// </summary>
			/// <param name="var">�f�[�^���i�[����ϐ��̖��O���w�肵�܂��B</param>
			/// <param name="value">�w�肵���I�u�W�F�N�g�𕶎���ɕϊ����ĕԂ��܂��B</param>
			/// <exception cref="System.ArgumentException">
			/// �w�肵���I�u�W�F�N�g�𕶎���^�ւ̕ϊ����o���Ȃ������ꍇ�ɔ������܂��B
			/// afh.Convert.Convert �ł̕ϊ��ɑΉ����Ă���^���g�p���ĉ������B
			/// </exception>
			public void SetValue(string var,object value){
				this.SetValue(var,value.GetType(),value);
			}
		}
		/// <summary>
		/// �l�̗񋓎q�B�l�� System.Xml.XmlAttribute �Ƃ��ĕԂ��܂��B
		/// </summary>
		private class VarEnumerator:System.Collections.IEnumerator{
			private System.Collections.IEnumerator enu;
			public VarEnumerator(System.Xml.XmlElement elem){
				this.enu=elem.Attributes.GetEnumerator();
			}
			public void Reset(){this.enu.Reset();}
			public bool MoveNext(){
				System.Xml.XmlAttribute a;
				while(this.enu.MoveNext()){
					a=(System.Xml.XmlAttribute)this.enu.Current;
					if(a.Prefix!=DEFAULT_CLASS)return true;
				}
				return false;
			}
			public object Current{get{return this.enu.Current;}}
		}
		//===========================================================
		//		�q Key
		//===========================================================
		/// <summary>
		/// ���ނƖ��O���w�肵�� SettingKey ���擾���܂��B
		/// �Ή����鑶�݂��Ȃ��ꍇ�ɂ͐V�����v�f���쐬���܂��B
		/// </summary>
		public SettingKey this[string cls,string name]{
			get{
				foreach(System.Xml.XmlElement e in this.elem.GetElementsByTagName(cls)){
					if(e.GetAttribute("name",Setting.XMLNS_KEY)==name)return new SettingKey(e);
				}
				return new SettingKey(this.elem,cls,name);
			}
		}
		/// <summary>
		/// ���O���w�肵�Ċ���̕��ނ̒����� SettingKey ���擾���܂��B
		/// �Ή����鑶�݂��Ȃ��ꍇ�ɂ͐V�����v�f���쐬���܂��B
		/// </summary>
		public SettingKey this[string name]{get{return this[DEFAULT_CLASS,name];}}
		/// <summary>
		/// �p�X���w�肵�Ďq���� SettingKey ���擾���܂��B
		/// ���݂��Ȃ��ꍇ�͐V�����쐬���܂��B
		/// </summary>
		/// <param name="path">�擾���� SettingKey �̃p�X���w�肵�܂��B
		/// �N���X�����w�肷��ꍇ�ɂ͖��O�̎�O�� "&lt;" �� "&gt;" �Ŋ����ċL�q���܂��B
		/// </param>
		/// <returns>�擾���� SettingKey ��Ԃ��܂�</returns>
		/// <example>
		/// <code>
		/// SettingKey main=Setting.Root.GetKey("user\&lt;example&gt;setting1\keyMain");
		/// </code>
		/// </example>
		public SettingKey GetKey(string path){
			return this.GetKey(path.Split(new char[]{'\\'}),0);
		}
		/// <summary>
		/// �w�肵���p�X�� SettingKey ���擾���܂�
		/// </summary>
		/// <param name="names">�p�X�� [���ꂼ��̊K�w�̖��O��\��������] �̔z����w�肵�܂��B
		/// �N���X���w�肷��ۂɂ� "&lt;" �� "&gt;" �ň͂�Ŗ��O�̑O�Ɏw�肵�܂�</param>
		/// <param name="index">���Ɍ������閼�O</param>
		/// <returns>�p�X�ɂ���Ďw�肳��� SettingKey ��Ԃ��܂��B
		/// �����łȂ��ꍇ�͐V�����쐬���ĕԂ��܂��B</returns>
		private SettingKey GetKey(string[] names,int index){
			string cls=DEFAULT_CLASS;
			string name=names[index];
			if(name.StartsWith("<")){
				int i=name.IndexOf(">");
				if(i>0){
					cls=name.Substring(1,i-1);
					name=name.Substring(i+1);
				}
			}
			SettingKey r=this[cls,name];
			if(++index==names.Length)return r;
			return r.GetKey(names,index);
		}
		//===========================================================
		//		�q SettingKeyCollection
		//===========================================================
		/// <summary>
		/// �w�肵�����ނɑΉ����� SettingKey �W���ւ̃A�N�Z�X��񋟂��܂��B
		/// </summary>
		/// <param name="cls">SettingKey �̕��ނ��w�肵�Ă��������B</param>
		/// <returns>SettingKey �W���ւ̃A�N�Z�X��񋟂��� SettingKeyCollection ��Ԃ��܂��B</returns>
		public SettingKeyCollection GetKeys(string cls){
			return new SettingKeyCollection(this.elem,cls);
		}
		/// <summary>
		/// ����̕��ނ� SettingKey �̏W���ւ̃A�N�Z�X��񋟂��܂��B
		/// </summary>
		/// <returns>SettingKey �ւ̃A�N�Z�X��񋟂��� SettingKeyCollection ��Ԃ��܂��B</returns>
		public SettingKeyCollection GetKeys(){return this.GetKeys(DEFAULT_CLASS);}
		/// <summary>
		/// ���镪�ނ� SettingKey �ւ̃A�N�Z�X��񋟂���N���X�ł��B
		/// </summary>
		public class SettingKeyCollection:System.Collections.IEnumerable{
			private System.Xml.XmlElement p;
			private string cls;
			private System.Xml.XmlNodeList list;
			/// <summary>
			/// SettingKeyCollection �R���X�g���N�^�B
			/// </summary>
			/// <param name="parent">
			/// �e�� SettingKey �ɑΉ����� System.Xml.XmlElement ���w�肵�܂��B
			/// parent.OwnerDocument �ɂ� xmlns:key ���ݒ肳��Ă���K�v������܂��B
			/// </param>
			/// <param name="cls">SettingKey �W������肷�镪�ނ��w�肵�܂��B</param>
			public SettingKeyCollection(System.Xml.XmlElement parent,string cls){
				this.p=parent;
				this.cls=cls;
				this.list=this.p.GetElementsByTagName(cls);
			}
			/// <summary>
			/// �W���Ɋ܂܂�� SettingKey �̐����擾���܂��B
			/// </summary>
			public int Count{get{return this.list.Count;}}
			/// <summary>
			/// �ԍ����w�肵�� SettingKey ���擾���܂��B
			/// </summary>
			public SettingKey this[int index]{
				get{return new SettingKey((System.Xml.XmlElement)this.list[index]);}
			}
			/// <summary>
			/// ���O���w�肵�� SettingKey ���擾���܂��B
			/// �Ή����� XML �v�f���Ȃ������ꍇ�͐V�����v�f���쐬���܂��B 
			/// </summary>
			public SettingKey this[string name]{
				get{
					foreach(System.Xml.XmlElement e in this.list){
						if(e.GetAttribute("name",Setting.XMLNS_KEY)==name)return new SettingKey(e);
					}
					SettingKey r=new SettingKey(this.p,this.cls,name);
					this.list=this.p.GetElementsByTagName(this.cls);
					return r;
				}
			}
			/// <summary>
			/// ���O���w�肵�� SettingKey ���擾���܂��B
			/// </summary>
			/// <param name="name">�擾���� SettingKey �̖��O���w�肵�܂��B</param>
			/// <returns>�������� XML �v�f�ɑΉ����� SettingKey �̃C���X�^���X��Ԃ��܂��B
			/// �Ή����� XML �v�f��������Ȃ������ꍇ�ɂ� null ��Ԃ��܂��B</returns>
			public SettingKey GetIfExists(string name){
				foreach(System.Xml.XmlElement e in this.list){
					if(e.GetAttribute("name",Setting.XMLNS_KEY)==name)return new SettingKey(e);
				}
				return null;
			}
			/// <summary>
			/// ���݂̕��ނɊ܂܂�� SettingKey ��񋓂���񋓎q���擾���܂��B
			/// </summary>
			/// <returns>SettingKey �̗񋓎q��Ԃ��܂��B</returns>
			public System.Collections.IEnumerator GetEnumerator(){
				return new SettingKeyEnumerator(this.list.GetEnumerator());
			}
		}
		/// <summary>
		/// SettingKey �̗񋓎q
		/// </summary>
		private class SettingKeyEnumerator:System.Collections.IEnumerator{
			System.Collections.IEnumerator enu;
			/// <summary>
			/// SettingKey �̗񋓎q�����������܂��B
			/// </summary>
			/// <param name="enu">System.Xml.XmlElement �ɕϊ��ł���I�u�W�F�N�g��񋓂���񋓎q���w�肵�Ă��������B</param>
			public SettingKeyEnumerator(System.Collections.IEnumerator enu){
				this.enu=enu;
			}
			public void Reset(){this.enu.Reset();}
			public bool MoveNext(){return this.enu.MoveNext();}
			public object Current{get{return new SettingKey((System.Xml.XmlElement)this.enu.Current);}}
		}
		//===========================================================
		//		.ctor
		//===========================================================
		/// <summary>
		/// �V�����v�f���쐬���āASettingKey �����������܂��B
		/// </summary>
		/// <param name="parent">
		/// ���̗v�f��ێ�����e�v�f���w�肵�܂��B
		/// parent �� OwnerDocument �ɂ�xmlns:key ���ݒ肳��Ă��Ȃ���΂Ȃ�܂���B
		/// </param>
		/// <param name="cls">���ނ��w�肷�镶������w�肵�܂��B
		/// �󔒂��w�肵���ꍇ "key" (����̕��ނ�\��������)�Ɖ��߂���܂��B</param>
		/// <param name="name">���� SettingKey ����肷�镶������w�肵�܂��B
		/// �󔒂��w�肵���ꍇ�ɂ͖��O�͐ݒ肳��܂���B</param>
		public SettingKey(System.Xml.XmlElement parent,string cls,string name):this(parent.OwnerDocument.CreateElement(cls==""?DEFAULT_CLASS:cls)){
			try{
				if(name!="")this.elem.SetAttribute("name",Setting.XMLNS_KEY,name);
				parent.AppendChild(this.elem);
			}catch(System.Exception e){throw e;}
		}
		/// <summary>
		/// ������ XML �v�f���� SettingKey �C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="elem">
		/// ���� SettingKey �̌��ƂȂ� System.Xml.XmlElement ���w�肵�܂��B
		/// parent �� OwnerDocument �ɂ�xmlns:key ���ݒ肳��Ă��Ȃ���΂Ȃ�܂���B
		/// </param>
		public SettingKey(System.Xml.XmlElement elem){
			this.elem=elem;
			this.vars=new VarCollection(this);
		}
	}
	#endregion

	#region Attribute:RestoreProperties
	/// <summary>
	/// ���(�v���p�e�B)�̕ۑ��E�����Ɋւ���@�\��񋟂���N���X�ł��B
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Field)]
	public class RestorePropertiesAttribute:System.Attribute{
		private string[] props;
		/// <summary>
		/// RestorePropertiesAttribute �̃C���X�^���X���쐬���܂��B
		/// �N���X�ɐݒ肵�����ɂ́A���̃N���X�C���X�^���X�̃v���p�e�B�̒l��ۑ��E�������܂��B
		/// �t�B�[���h�ɐݒ肵�����ɂ͂��̃t�B�[���h�̃v���p�e�B�̒l��ۑ��E�������܂��B
		/// </summary>
		/// <param name="propertyNames">��Ԃ�ۑ��E��������v���p�e�B�̖��O���w�肵�܂��B
		/// <para>
		/// &gt;�ŋ�؂��ē�̃v���p�e�B�����L�q�����ꍇ�ɂ́A
		/// &gt; �̍�������ǂݎ���ăt�@�C���ɕۑ����A
		/// �������ɂ� &gt; �̉E���Ƀt�@�C������ǂݎ�����l��ݒ肵�܂��B
		/// </para>
		/// </param>
		/// <remarks>�v���p�e�B�̌^���Ή����Ă��Ȃ��^�̏ꍇ�́A
		/// �w�肵�Ă��ۑ��E�������鎖�͂ł��܂���B</remarks>
		public RestorePropertiesAttribute(params string[] propertyNames){
			this.props=propertyNames;
			//System.Console.WriteLine("initialized RestorePropertiesAttribute");
		}
		private bool traceField=false;
		/// <summary>
		/// RestorePropertiesAttribute ��ݒ肵���t�B�[���h�̌^���̂ɂ��A
		/// RestorePropertiesAttribute ���ݒ肳��Ă���ꍇ�ɁA
		/// ���̃t�B�[���h�ϐ��ɑ΂��Ă� Restore �������Ŏ��s�����鎖���o���܂��B
		/// ���� TraceFieldType �v���p�e�B���g���āA�����Ŏ��s���邩�ۂ����擾���͐ݒ肵�܂��B
		/// ����̒l�� false �ł��B
		/// </summary>
		/// <remarks>
		/// �~�Q�ƂɂȂ��Ă��镨�ɑ΂��Ă����ݒ肷��ƁA
		/// �ۑ��E�����̍ۂɖ����Ăяo���ɂȂ� StackOverflow �̊댯���������܂��B
		/// </remarks>
		public bool TraceFieldType{
			get{return this.traceField;}
			set{this.traceField=value;}
		}
		//===========================================================
		//		�ÓI�����o
		//===========================================================
		private const string SETTINGKEY_NAME="afh.Configuration.RestoreProperties";
		private const System.Reflection.BindingFlags BF_FIELD
			=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
		//
		//	Exception Messages
		//
		/*
		private const string LOCATION_RESTORE="<afh.dll> afh.Configuration.RestorePropertiesAttribute.Restore";
		private const string LOCATION_SAVE="<afh.dll> afh.Configuration.RestorePropertiesAttribute.Save";
		private const string LOCATION_SETVAL="<afh.dll> afh.Configuration.RestorePropertiesAttribute.SetValue";
		private const string LOCATION_GETVAL="<afh.dll> afh.Configuration.RestorePropertiesAttribute.GetValue";
		private const string PROPERTY_NOSUCH="{0}:\t\n�w�肵���v���p�e�B {2} �� {1} �ɂ͑��݂��܂���";
		private const string PROPERTY_CANNOTREAD="{0}:\t\n�w�肵���v���p�e�B {1}.{2} �ɂ� getter ����������Ă��܂���";
		private const string PROPERTY_CANNOTWRITE="{0}:\t\n�w�肵���v���p�e�B {1}.{2} �ɂ� setter ����������Ă��܂���";
		private const string ERROR_CONVERT=@"{0}:
	�ϊ����ɃG���[���������܂���
	Message :	{1}
	Property:	{2}
	Type    :	{3}
	Value   :	{4}";
		private const string ERROR_SETTING=@"{0}:
	�l�̐ݒ蒆�ɃG���[���������܂���
	Message :	{1}
	Property:	{2}
	Value   :	{3}";
		//*/
		private const string PROPERTY_NOSUCH="�w�肵���v���p�e�B {2} �� {1} �ɂ͑��݂��܂���";
		private const string PROPERTY_CANNOTREAD="�w�肵���v���p�e�B {1}.{2} �ɂ� getter ����������Ă��܂���";
		private const string PROPERTY_CANNOTWRITE="�w�肵���v���p�e�B {1}.{2} �ɂ� setter ����������Ă��܂���";
		private const string ERROR_CONVERT="�l�̕ϊ����ɃG���[���������܂���";
		private const string ERROR_SETTING="�l�̐ݒ蒆�ɃG���[���������܂���";
		//===========================================================
		//		����
		//===========================================================
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l�𕜌����܂��B
		/// </summary>
		/// <param name="obj">��Ԃ𕜌�����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="id">�����^�̕����̃I�u�W�F�N�g����ʂ���ׂ̎��ʎq���w�肵�܂��B</param>
		public static void Restore(object obj,string id){
			string name=obj.GetType().ToString()+":"+id;
			Restore(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l�𕜌����܂��B
		/// </summary>
		/// <param name="obj">��Ԃ𕜌�����I�u�W�F�N�g���w�肵�܂��B</param>
		public static void Restore(object obj){
			string name=obj.GetType().ToString();
			Restore(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l�𕜌����܂��B
		/// </summary>
		/// <param name="obj">��������I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="k">�����Ɏg�p����̂̏������� SettingKey ���w�肵�܂��B</param>
		private static void Restore(object obj,SettingKey k){
			System.Type t=obj.GetType();
			RestorePropertiesAttribute[] attrs
				=(RestorePropertiesAttribute[])t.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
			if(attrs.Length==0)return;
			//--obj ���̂̕���
			Restore(obj,k,attrs);
			//--Field �̕���
			object field;SettingKey kf;
			foreach(System.Reflection.FieldInfo f in t.GetFields(BF_FIELD)){
				attrs=(RestorePropertiesAttribute[])f.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
				if(attrs.Length==0)continue;
				//--����
				field=f.GetValue(obj);kf=k["F",f.Name];
				//--����
				Restore(field,kf,attrs);
				if(attrs[0].traceField)Restore(field,kf);
			}
		}
		/// <summary>
		/// �w�肵�� RestorePropertiesAttribute �ɏ]����
		/// �w�肵���I�u�W�F�N�g�̃v���p�e�B�̒l�𕜌����܂��B
		/// </summary>
		/// <param name="obj">��������I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="k">�����Ɏg�p����̂̏������� SettingKey ���w�肵�܂��B</param>
		/// <param name="attrs">�ǂ̃v���p�e�B�𕜌����邩�̏����w�肵�܂��B</param>
		private static void Restore(object obj,SettingKey k,RestorePropertiesAttribute[] attrs){
			foreach(RestorePropertiesAttribute attr in attrs){
				foreach(string prop in attr.props){
					if(prop.IndexOf(">")>=0){
						string[] prop2=prop.Split(new char[]{'>'},2);
						if(!k.Var.HasVariable(prop2[0]))continue;
						SetValue(obj,prop2[1],k.Var[prop2[0]]);
					}else{
						if(!k.Var.HasVariable(prop))continue;
						SetValue(obj,prop,k.Var[prop]);
					}
				}
			}
		}
		//===========================================================
		//		�ۑ�
		//===========================================================
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l��ۑ����܂��B
		/// </summary>
		/// <param name="obj">��Ԃ�ۑ�����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="id">�����^�̕����̃I�u�W�F�N�g����ʂ���ׂ̎��ʎq���w�肵�܂��B</param>
		public static void Save(object obj,string id){
			string name=obj.GetType().ToString()+":"+id;
			Save(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l��ۑ����܂��B
		/// </summary>
		/// <param name="obj">��Ԃ�ۑ�����I�u�W�F�N�g���w�肵�܂��B</param>
		public static void Save(object obj){
			string name=obj.GetType().ToString();
			Save(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̌^�ɐݒ肳�ꂽ RestorePropertiesAttribute �ɏ]����
		/// �v���p�e�B�̒l��ۑ����܂��B
		/// </summary>
		/// <param name="obj">�ۑ�����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="k">���݂̏�Ԃ̕ۑ���ł��� SettingKey ���w�肵�܂��B</param>
		private static void Save(object obj,SettingKey k){
			System.Type t=obj.GetType();
			RestorePropertiesAttribute[] attrs
				=(RestorePropertiesAttribute[])t.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
			if(attrs.Length==0)return;
			//--obj ���̂̕ۑ�
			Save(obj,k,attrs);
			//--Field �̕ۑ�
			object field;SettingKey kf;
			foreach(System.Reflection.FieldInfo f in t.GetFields(BF_FIELD)){
				attrs=(RestorePropertiesAttribute[])f.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
				if(attrs.Length==0)continue;
				//--����
				field=f.GetValue(obj);kf=k["F",f.Name];
				//--����
				Save(field,kf,attrs);
				if(attrs[0].traceField)Save(field,kf);
			}
		}
		/// <summary>
		/// �w�肵�� RestorePropertiesAttribute �ɏ]����
		/// �w�肵���I�u�W�F�N�g�̃v���p�e�B�̒l��ۑ����܂��B
		/// </summary>
		/// <param name="obj">�ۑ�����I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="k">���݂̏�Ԃ̕ۑ���ł��� SettingKey ���w�肵�܂��B</param>
		/// <param name="attrs">�ǂ̃v���p�e�B��ۑ����邩�̏����w�肵�܂��B</param>
		private static void Save(object obj,SettingKey k,RestorePropertiesAttribute[] attrs){
			int i;string prop2;
			foreach(RestorePropertiesAttribute attr in attrs){
				foreach(string prop in attr.props){
					i=prop.IndexOf(">");
					prop2=i<0?prop:prop.Substring(0,i);
					k.Var[prop2]=GetValue(obj,prop2);
				}
			}
		}
		//===========================================================
		//		�v���p�e�B�̐ݒ�E�擾
		//===========================================================
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̎w�肵���v���p�e�B�ɕ�����Ŏw�肵���l��ݒ肵�܂��B
		/// </summary>
		/// <param name="obj">�ݒ��̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="prop">�ݒ��̃v���p�e�B�̎��ʎq���w�肵�܂��B</param>
		/// <param name="value">�ݒ肷��l���w�肵�܂��B</param>
		private static void SetValue(object obj,string prop,string value){
			System.Type t=obj.GetType();
			System.Reflection.PropertyInfo p=t.GetProperty(prop);
			if(p==null){
				Application.Log.AfhOut.WriteError("�w�肵���v���p�e�B "+prop+" �� "+t.FullName+" �ɂ͑��݂��܂���");
				return;
			}else if(!p.CanWrite){
				Application.Log.AfhOut.WriteError("�w�肵���v���p�e�B "+t.FullName+"."+prop+" �ɂ� setter ����������Ă��܂���");
				return;
			}
			object val;
			try{
				val=afh.StringConvert.To(p.PropertyType,value);
			}catch(System.Exception e){
				Application.Log l=Application.Log.AfhOut;
				l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
				l.AddIndent();
				l.WriteVar("Message",e.Message);
				l.WriteVar("Property",p.ReflectedType.ToString()+"."+p.Name);
				l.WriteVar("TargetType",p.PropertyType.ToString());
				l.WriteVar("Value",value);
				l.RemoveIndent();
				return;
			}
			try{
				p.SetValue(obj,val,null);
			}catch(System.Exception e){
				Application.Log l=Application.Log.AfhOut;
				l.WriteError("�l�̐ݒ蒆�ɃG���[���������܂����B");
				l.AddIndent();
				l.WriteVar("Message",e.InnerException==null?e.Message:e.InnerException.Message);
				l.WriteVar("Property",p.ReflectedType.ToString()+"."+p.Name);
				l.WriteVar("Value",val.ToString());
				l.RemoveIndent();
				return;
			}
			
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g�̎w�肵���v���p�e�B�𕶎���Ƃ��Ď擾���܂��B
		/// </summary>
		/// <param name="obj">�擾���̃I�u�W�F�N�g���w�肵�܂��B</param>
		/// <param name="prop">�擾���̃v���p�e�B�̎��ʎq���w�肵�܂��B</param>
		/// <returns>�擾�����v���p�e�B�𕶎���Ƃ��ĕԂ��܂��B</returns>
		private static string GetValue(object obj,string prop){
			System.Type t=obj.GetType();
			System.Reflection.PropertyInfo p=t.GetProperty(prop);
			if(p==null){
				Application.Log.AfhOut.WriteError("�w�肵���v���p�e�B "+prop+" �� "+t.FullName+" �ɂ͑��݂��܂���");
				return null;
			}else if(!p.CanRead){
				Application.Log.AfhOut.WriteError("�w�肵���v���p�e�B "+t.FullName+"."+prop+" �ɂ� getter ����������Ă��܂���");
				return null;
			}
			object value=p.GetValue(obj,null);
			if(value==null)return null;
			try{
				return afh.StringConvert.From(p.PropertyType,value);
			}catch(System.Exception e){
				Application.Log l=Application.Log.AfhOut;
				l.WriteError("�l�̕ϊ����ɃG���[���������܂����B");
				l.AddIndent();
				l.WriteVar("Message",e.Message);
				l.WriteVar("Property",p.ReflectedType.ToString()+"."+p.Name);
				l.WriteVar("TargetType",p.PropertyType.ToString());
				l.WriteVar("Value",value.ToString());
				l.RemoveIndent();
				return null;
			}
		}
		//===========================================================
		//		System.Windows.Forms.Form �̓��ʐݒ�
		//===========================================================
		/// <summary>
		/// System.Windows.Forms.Form �̕����E�ۑ��������I�ɐݒ肵�܂��B
		/// </summary>
		/// <param name="form">��Ԃ𕜌��E�ۑ����� Form �̃C���X�^���X���w�肵�܂��B</param>
		/// <param name="savesize">
		/// Form �̑傫���A�ʒu�AWindowState �𕜌��������ꍇ�� true ���w�肵�܂��B
		/// �������Ȃ��ꍇ�E�����ŕ����̃R�[�h���e�ꍇ�ɂ� false ���w�肵�܂��B
		/// <para>�� Left, Top, Width, Height, WindowState ��ʁX�ɕ������Ă����S�ɂ͕�������܂���B
		/// WindowState �� Maximized �̏ꍇ�ŕۑ������s����ƁA
		/// Normal �ɖ߂������ɂǂ̈ʒu�E�傫���ɂȂ锤���������̏�񂪎����Ă��܂��܂��B
		/// 䢂� true �Ɏw�肵���ꍇ�ɂ́A���S�ɕ����o����悤�ɓ��ʂ̏������s���܂��B
		/// </para>
		/// </param>
		public static void SetupForm(System.Windows.Forms.Form form,bool savesize){
			new FormSetting(form,savesize);
		}
		/// <summary>
		/// System.Windows.Forms.Form �̕����E�ۑ��������I�ɐݒ肵�܂��B
		/// </summary>
		/// <param name="form">��Ԃ𕜌��E�ۑ����� Form �̃C���X�^���X���w�肵�܂��B</param>
		/// <param name="id">�����^�̕����� Form �C���X�^���X����ʂ���ׂ̎��ʎq���w�肵�܂��B</param>
		/// <param name="savesize">
		/// Form �̑傫���A�ʒu�AWindowState �𕜌��������ꍇ�� true ���w�肵�܂��B
		/// �������Ȃ��ꍇ�E�����ŕ����̃R�[�h���e�ꍇ�ɂ� false ���w�肵�܂��B
		/// <para>�� Left, Top, Width, Height, WindowState ��ʁX�ɕ������Ă����S�ɂ͕�������܂���B
		/// WindowState �� Maximized �̏ꍇ�ŕۑ������s����ƁA
		/// Normal �ɖ߂������ɂǂ̈ʒu�E�傫���ɂȂ锤���������̏�񂪎����Ă��܂��܂��B
		/// 䢂� true �Ɏw�肵���ꍇ�ɂ́A���S�ɕ����o����悤�ɓ��ʂ̏������s���܂��B
		/// </para>
		/// </param>
		public static void SetupForm(System.Windows.Forms.Form form,string id,bool savesize){
			new FormSetting(form,id,savesize);
		}
		private class FormSetting{
			private System.Windows.Forms.Form form;
			private SettingKey key;
			private bool savesize;

			//-------------------------------------------------------
			//		������
			//-------------------------------------------------------
			public FormSetting(System.Windows.Forms.Form form,bool savesize){
				this.Initialize(form,null,savesize);
			}

			public FormSetting(System.Windows.Forms.Form form,string id,bool savesize){
				if(id==null)throw new System.ArgumentNullException("id");
				this.Initialize(form,id,savesize);
			}

			private void Initialize(System.Windows.Forms.Form form,string id,bool savesize){
				if(form==null)throw new System.ArgumentNullException("form");
				this.form=form;
				this.form.Load+=new System.EventHandler(form_Load);
				this.form.FormClosing+=new System.Windows.Forms.FormClosingEventHandler(form_FormClosing);

				string name=this.form.GetType().ToString();
				if(id!=null)name+=':'+id;
				this.key=Setting.Root[SETTINGKEY_NAME]["T",name];

				if(this.savesize=savesize){
					this.form.SizeChanged+=new System.EventHandler(form_SizeChanged);
					this.form.LocationChanged+=new System.EventHandler(form_LocationChanged);
				}
			}

			//-------------------------------------------------------
			//		�L�^�E����
			//-------------------------------------------------------
			void form_FormClosing(object sender,System.Windows.Forms.FormClosingEventArgs e) {
				Save(this.form,this.key);
				if(savesize)
					Save(this,this.key["ATTACH","afh.Configuration.RestoreProperties+FormSetting"],form_setting_attr);
			}

			void form_Load(object sender,System.EventArgs e) {
				Restore(this.form,this.key);
				if(savesize)
					Restore(this,this.key["ATTACH","afh.Configuration.RestoreProperties+FormSetting"],form_setting_attr);
			}
			//-------------------------------------------------------
			//		�傫���̋L�^
			//-------------------------------------------------------
			private static RestorePropertiesAttribute[] form_setting_attr
				={new RestorePropertiesAttribute("NormalLeft","NormalTop","NormalWidth","NormalHeight","WindowState")};
			private int normal_t=-1;
			private int normal_l=-1;
			private int normal_w=-1;
			private int normal_h=-1;

			public int NormalTop{
				get{return this.normal_t<0?this.form.Top:this.normal_t;}
				set{this.normal_t=this.form.Top=value;}
			}
			public int NormalLeft{
				get{return this.normal_l<0?this.form.Left:this.normal_l;}
				set{this.normal_l=this.form.Left=value;}
			}
			public int NormalWidth{
				get{return this.normal_w<0?this.form.Width:this.normal_w;}
				set{this.normal_w=this.form.Width=value;}
			}
			public int NormalHeight{
				get{return this.normal_h<0?this.form.Height:this.normal_h;}
				set{this.normal_h=this.form.Height=value;}
			}
			public System.Windows.Forms.FormWindowState WindowState{
				get{return this.form.WindowState;}
				set{this.form.WindowState=value;}
			}

			void form_SizeChanged(object sender,System.EventArgs e) {
				if(this.form.WindowState==System.Windows.Forms.FormWindowState.Normal){
					this.normal_w=this.form.Width;
					this.normal_h=this.form.Height;
				}
			}

			void form_LocationChanged(object sender,System.EventArgs e) {
				if(this.form.WindowState==System.Windows.Forms.FormWindowState.Normal) {
					this.normal_l=this.form.Left;
					this.normal_t=this.form.Top;
				}
			}
		}
	}
	#endregion

}