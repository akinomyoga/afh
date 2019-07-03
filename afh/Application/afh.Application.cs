namespace afh.Application{
	using Gen=System.Collections.Generic;
	using Rgx=System.Text.RegularExpressions;

	/// <summary>
	/// �t�@�C���p�X (���̓t�@�C����) �Ɋւ��鑀���񋟂��܂��B
	/// </summary>
	// TODO: \\ �Ŏn�܂�p�X�ɑ΂���Ή�
	public static class Path{
		/// <summary>
		/// ���݂̃f�B���N�g�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public static string CurrentDirectory{
			get{return System.Environment.CurrentDirectory;}
			set{System.Environment.CurrentDirectory=value;}
		}
		/// <summary>
		/// ���s�t�@�C���݂̍�f�B���N�g���̃p�X���擾���܂��B
		/// </summary>
		public static string ExecutableDirectory{
			get{return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);}
		}
		/// <summary>
		/// ���s�t�@�C���̃t�@�C�������擾���܂��B
		/// </summary>
		public static string ExecutableFileName{
			get{return System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);}
		}
		/// <summary>
		/// �w�肵���t�@�C�����Ɏw�肵���g���q��t�������p�X���擾���܂��B
		/// ���Ƀt�@�C�������݂���ꍇ�ɂ� [0] ���̕�������g���q�̑O�ɕt�����܂��B
		/// </summary>
		/// <param name="path">�t�@�C����</param>
		/// <param name="extension">�t������g���q</param>
		/// <returns>���������t�@�C������Ԃ��܂��B</returns>
		public static string GetAvailablePath(string path,string extension){
			if(extension!=""&&!extension.StartsWith("."))extension="."+extension;
			string p=path+extension;
			int i=0;
			while(System.IO.File.Exists(p)||System.IO.Directory.Exists(p))
				p=path+"["+(i++).ToString()+"]"+extension;
			return p;
		}
		/// <summary>
		/// �w�肵���t�@�C�����A���ɑ��݂��Ă��Ȃ������m�F���܂��B
		/// ���Ƀt�@�C�������݂��Ă����ꍇ�ɂ� [0] ���̕�������g���q�̑O�ɕt�����܂��B
		/// ���Ƀt�@�C�������݂���ꍇ�ɂ� [0] ���̕�������g���q�̑O�ɕt�����܂��B
		/// </summary>
		/// <param name="path">�t�@�C����</param>
		/// <returns>���������t�@�C������Ԃ��܂��B</returns>
		public static string GetAvailablePath(string path){
			if(System.IO.File.Exists(path)||System.IO.Directory.Exists(path)){
				string ext=System.IO.Path.GetExtension(path);
				string basePath=path.Substring(0,path.Length-ext.Length);
				return GetAvailablePath(basePath,ext);
			}
			return path;
		}
		/// <summary>
		/// \ �� / �� " ���̃t�@�C�����Ƃ��Ė����ȕ�����S�p�ɕϊ�����Ȃǂ��ăt�@�C�����Ƃ��ėL���ȕ��ɕϊ����܂��B
		/// </summary>
		/// <param name="filename">�t�@�C�����Ƃ��Ė����ȉ\���̂��镶������w�肵�܂��B</param>
		/// <returns>�t�@�C�����Ƃ��ėL���ȕ�����ɕϊ���������Ԃ��܂��B</returns>
		public static string GetValidFileName(string filename){
			if(filename=="")return "�@";
			if(filename==".")return "�D";
			if(filename=="..")return "�D�D";
			return filename.Replace("\\","��").Replace("/","�^").Replace(":","�F")
				.Replace("<","��").Replace(">","��").Replace("*","��")
				.Replace("?","�H").Replace("\"","�h").Replace("|","�b");
		}
		/// <summary>
		/// �w�肵�����f�B���N�g������ɂ������΃p�X�����߂ĕԂ��܂��B
		/// �h���C�u���قȂ�Ƃ��ɂ͎w�肵���p�X�𒼐ڕԂ��܂��B
		/// </summary>
		/// <param name="pathName">�Ώۂ̐�΃p�X���w�肵�܂��B</param>
		/// <param name="baseDirectory">���f�B���N�g���̐�΃p�X���w�肵�܂��B</param>
		/// <returns>�w�肵���Ώۂ̃p�X�𑊑΃p�X�ɂ��ĕԂ��܂��B</returns>
		public static string GetRelativePath(string pathName,string baseDirectory){
			string[] paths=splitPath(pathName);
			string[] bases=splitPath(baseDirectory);

			//-- different drive letter.
			if(IsDifferentDrive(bases[0],paths[0]))return pathName;

			//-- i: baseDirectory �Ɉ�v���鐔
			int i;for(i=0;i<bases.Length;i++){
				if(i<paths.Length&&paths[i]==bases[i])continue;
				break;
			}

			System.Text.StringBuilder r=new System.Text.StringBuilder();
			for(int j=i;j<bases.Length;j++)r.Append("..\\");
			for(;i<paths.Length;i++){
				r.Append(paths[i]);
				r.Append("\\");
			}

			string r2=r.ToString();
			return r2==""?".":r2.TrimEnd('\\');
		}
		private static string[] splitPath(string path){
			if(path==null)return null;

			// \\ �Ŏn�܂�p�X�̈�
			bool handle_root=false;
			if(path.Length>=2&&(path[0]=='\\'||path[0]=='/')&&(path[1]=='\\'||path[1]=='/')){
				path=path.Substring(1);
				handle_root=true;
			}

			string[] paths=path.TrimEnd('\\','/').Split('\\','/');
			if(handle_root)paths[0]=@"\";

			return paths;
		}
		private static bool IsDifferentDrive(string left,string right){
			bool leftIsDrive=left==@"\"||left.EndsWith(":");
			bool rightIsDrive=right==@"\"||right.EndsWith(":");
			return leftIsDrive&&rightIsDrive&&left!=right;
		}
		/// <summary>
		/// �w�肵���p�X���������A.. �� . ���̎w����܂ޏꍇ�ɂ͂������������܂��B
		/// </summary>
		/// <param name="path1">��ɗ���p�X���w�肵�܂��B</param>
		/// <param name="path2">��ɂ���p�X���w�肵�܂��B</param>
		/// <returns>����������̃p�X��Ԃ��܂��B</returns>
		public static string Combine(string path1,string path2){
			string path0=System.IO.Path.Combine(path1,path2);
			if(path0.IndexOf("..")<0)return path0;

			//-- ���K��
			string[] paths=path0.TrimEnd('\\','/').Split('\\','/');
			Gen::List<string> paths2=new Gen::List<string>();
			System.Text.StringBuilder r=new System.Text.StringBuilder();
			for(int i=0;i<paths.Length;i++){
				if(paths[i]==".."){
					if(paths2.Count==0)r.Append(@"..\");else paths2.RemoveAt(paths2.Count-1);
				}else if(paths[i]!="."){
					paths2.Add(paths[i]);
				}
			}
			foreach(string x in paths2.ToArray()){r.Append(x);r.Append(@"\");};

			string r2=r.ToString();
			return r2==""?".":r2.TrimEnd('\\');
		}
		/// <summary>
		/// �w�肵���f�B���N�g�������݂��鎖���m�F���A�Ȃ���ΐV�����쐬���܂��B
		/// �Ⴕ�A�����̃t�@�C�������ɑ��݂��Ă���ꍇ�ɂ́A�w�肵���p�X�������ύX���ăf�B���N�g�����쐬���܂��B
		/// ���̏ꍇ�A�ύX��̃f�B���N�g���̃p�X�� <paramref name="dirpath"/> �ɕԂ����̂Œ��ӂ��ĉ������B
		/// </summary>
		/// <param name="dirpath">
		/// ���݂�ۏ؂���f�B���N�g���ւ̃p�X���w�肵�܂��B
		/// ���݂�ۏ؂����f�B���N�g���ւ̃p�X��Ԃ��܂��B
		/// �w�肵���p�X�Ǝ��ۂɑ��݂�ۏ؂����f�B���N�g���ւ̃p�X�͈قȂ�ꍇ������̂Œ��ӂ��ĉ������B
		/// </param>
		public static void EnsureDirectoryExistence(ref string dirpath) {
			string path=dirpath;
			int num=0;
			while(System.IO.File.Exists(path)){
				path=dirpath+"["+num++.ToString()+"]";
			}
			if(!System.IO.Directory.Exists(dirpath)){
				System.IO.Directory.CreateDirectory(path);
			}
			dirpath=path;
		}
		/// <summary>
		/// �w�肵���f�B���N�g�������݂��邱�Ƃ��m�F���A�Ȃ���ΐV�����쐬���܂��B
		/// �Ⴕ�A�����̃t�@�C�������ɑ��݂��Ă���ꍇ�ɂ́A���̃t�@�C���̖��O�������ύX���Ă���f�B���N�g�����쐬���܂��B
		/// </summary>
		/// <param name="dirpath">���݂�ۏ؂���f�B���N�g���ւ̃p�X���w�肵�܂��B</param>
		public static void EnsureDirectoryExistence(string dirpath){
			if(System.IO.Directory.Exists(dirpath))return;

			if(System.IO.File.Exists(dirpath)){
				System.IO.File.Move(dirpath,GetAvailablePath(dirpath));
			}

			System.IO.Directory.CreateDirectory(dirpath);
		}
		//===========================================================
		//		Special Directory
		//===========================================================
		private static System.Collections.Hashtable specialDirs=new System.Collections.Hashtable();
		static Path(){
			specialDirs.Add("execdir",		Path.ExecutableDirectory);
			specialDirs.Add("appdata",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
			specialDirs.Add("cmappdata",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData));
			specialDirs.Add("cmprogfiles",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonProgramFiles));
			specialDirs.Add("cookies",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.Cookies));
			specialDirs.Add("desktop",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory));
			specialDirs.Add("favorites",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.Favorites));
			specialDirs.Add("history",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.History));
			specialDirs.Add("cache",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.InternetCache));
			specialDirs.Add("localappdata",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
			specialDirs.Add("mymusic",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic));
			specialDirs.Add("mypictures",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures));
			specialDirs.Add("personal",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
			specialDirs.Add("progfiles",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles));
			specialDirs.Add("progs",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.Programs));
			specialDirs.Add("recent",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.Recent));
			specialDirs.Add("sendto",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.SendTo));
			specialDirs.Add("startmenu",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu));
			specialDirs.Add("startup",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup));
			specialDirs.Add("system",		System.Environment.GetFolderPath(System.Environment.SpecialFolder.System));
			specialDirs.Add("templates",	System.Environment.GetFolderPath(System.Environment.SpecialFolder.Templates));
		}
		/// <summary>
		/// special:* �Ŏn�܂�`���� Path �����ۂ̃p�X�ɕϊ����܂��B
		/// </summary>
		/// <param name="path">special: �Ŏn�܂�\���̃p�X���w�肵�܂��B</param>
		/// <returns>�����������ʂ̃p�X��Ԃ��܂��B</returns>
		public static string ResolveSpecialDirectory(string path){
			if(!path.StartsWith("special:"))return path;
			int end1=path.IndexOf("\\");
			int end2=path.IndexOf("/");
			int end=end1<0?end2:end2<0?end1:end1<end2?end1:end2;
			if(end<0){
				path=path.Substring(8);
				if(specialDirs.ContainsKey(path)){
					return (string)specialDirs[path];
				}
				return path;
			}else{
				string key=path.Substring(8,end-8);
				if(specialDirs.ContainsKey(key)){
					return System.IO.Path.Combine((string)specialDirs[key],path.Substring(end+1));
				}
				return path;
			}
		}
		/// <summary>
		/// ���݂̃��[�U�[�̃f�X�N�g�b�v�̃p�X���擾���܂��B
		/// </summary>
		public static string Desktop{get{return (string)Path.specialDirs["desktop"];}}
	}
}