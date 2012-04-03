namespace afh.Application{
	using Gen=System.Collections.Generic;
	using Rgx=System.Text.RegularExpressions;

	/// <summary>
	/// ファイルパス (亦はファイル名) に関する操作を提供します。
	/// </summary>
	// TODO: \\ で始まるパスに対する対応
	public static class Path{
		/// <summary>
		/// 現在のディレクトリを取得または設定します。
		/// </summary>
		public static string CurrentDirectory{
			get{return System.Environment.CurrentDirectory;}
			set{System.Environment.CurrentDirectory=value;}
		}
		/// <summary>
		/// 実行ファイルの在るディレクトリのパスを取得します。
		/// </summary>
		public static string ExecutableDirectory{
			get{return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);}
		}
		/// <summary>
		/// 実行ファイルのファイル名を取得します。
		/// </summary>
		public static string ExecutableFileName{
			get{return System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);}
		}
		/// <summary>
		/// 指定したファイル名に指定した拡張子を付加したパスを取得します。
		/// 既にファイルが存在する場合には [0] 等の文字列を拡張子の前に付加します。
		/// </summary>
		/// <param name="path">ファイル名</param>
		/// <param name="extension">付加する拡張子</param>
		/// <returns>生成したファイル名を返します。</returns>
		public static string GetAvailablePath(string path,string extension){
			if(extension!=""&&!extension.StartsWith("."))extension="."+extension;
			string p=path+extension;
			int i=0;
			while(System.IO.File.Exists(p)||System.IO.Directory.Exists(p))
				p=path+"["+(i++).ToString()+"]"+extension;
			return p;
		}
		/// <summary>
		/// 指定したファイルが、既に存在していないかを確認します。
		/// 既にファイルが存在していた場合には [0] 等の文字列を拡張子の前に付加します。
		/// 既にファイルが存在する場合には [0] 等の文字列を拡張子の前に付加します。
		/// </summary>
		/// <param name="path">ファイル名</param>
		/// <returns>生成したファイル名を返します。</returns>
		public static string GetAvailablePath(string path){
			if(System.IO.File.Exists(path)||System.IO.Directory.Exists(path)){
				string ext=System.IO.Path.GetExtension(path);
				string basePath=path.Substring(0,path.Length-ext.Length);
				return GetAvailablePath(basePath,ext);
			}
			return path;
		}
		/// <summary>
		/// \ や / や " 等のファイル名として無効な文字を全角に変換するなどしてファイル名として有効な物に変換します。
		/// </summary>
		/// <param name="filename">ファイル名として無効な可能性のある文字列を指定します。</param>
		/// <returns>ファイル名として有効な文字列に変換した物を返します。</returns>
		public static string GetValidFileName(string filename){
			if(filename=="")return "　";
			if(filename==".")return "．";
			if(filename=="..")return "．．";
			return filename.Replace("\\","￥").Replace("/","／").Replace(":","：")
				.Replace("<","＜").Replace(">","＞").Replace("*","＊")
				.Replace("?","？").Replace("\"","”").Replace("|","｜");
		}
		/// <summary>
		/// 指定した基底ディレクトリを基準にした相対パスを求めて返します。
		/// ドライブが異なるときには指定したパスを直接返します。
		/// </summary>
		/// <param name="pathName">対象の絶対パスを指定します。</param>
		/// <param name="baseDirectory">基底ディレクトリの絶対パスを指定します。</param>
		/// <returns>指定した対象のパスを相対パスにして返します。</returns>
		public static string GetRelativePath(string pathName,string baseDirectory){
			string[] paths=splitPath(pathName);
			string[] bases=splitPath(baseDirectory);

			//-- different drive letter.
			if(IsDifferentDrive(bases[0],paths[0]))return pathName;

			//-- i: baseDirectory に一致する数
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

			// \\ で始まるパスの為
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
		/// 指定したパスを結合し、.. や . 等の指定を含む場合にはそれらを解決します。
		/// </summary>
		/// <param name="path1">先に来るパスを指定します。</param>
		/// <param name="path2">後にくるパスを指定します。</param>
		/// <returns>結合した後のパスを返します。</returns>
		public static string Combine(string path1,string path2){
			string path0=System.IO.Path.Combine(path1,path2);
			if(path0.IndexOf("..")<0)return path0;

			//-- 正規化
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
		/// 指定したディレクトリが存在する事を確認し、なければ新しく作成します。
		/// 若し、同名のファイルが既に存在している場合には、指定したパスを少し変更してディレクトリを作成します。
		/// この場合、変更後のディレクトリのパスは <paramref name="dirpath"/> に返されるので注意して下さい。
		/// </summary>
		/// <param name="dirpath">
		/// 存在を保証するディレクトリへのパスを指定します。
		/// 存在を保証したディレクトリへのパスを返します。
		/// 指定したパスと実際に存在を保証したディレクトリへのパスは異なる場合があるので注意して下さい。
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
		/// 指定したディレクトリが存在することを確認し、なければ新しく作成します。
		/// 若し、同名のファイルが既に存在している場合には、そのファイルの名前を少し変更してからディレクトリを作成します。
		/// </summary>
		/// <param name="dirpath">存在を保証するディレクトリへのパスを指定します。</param>
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
		/// special:* で始まる形式の Path を実際のパスに変換します。
		/// </summary>
		/// <param name="path">special: で始まる可能性のパスを指定します。</param>
		/// <returns>解決した結果のパスを返します。</returns>
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
		/// 現在のユーザーのデスクトップのパスを取得します。
		/// </summary>
		public static string Desktop{get{return (string)Path.specialDirs["desktop"];}}
	}
}