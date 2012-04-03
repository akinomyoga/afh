
namespace afh.Plugin{
	/// <summary>
	/// それぞれのプラグインの種類に応じて、
	/// プラグインを読み込む為の属性の基本クラスです。
	/// 静的メンバはスレッドセーフです。
	/// </summary>
	public abstract class PluginReaderAttribute:System.Attribute{
		/// <summary>
		/// PluginReaderAttribute のコンストラクタ。
		/// </summary>
		protected PluginReaderAttribute(){}
		/// <summary>
		/// 指定されたメンバを使用して、適当な初期化を行います。
		/// </summary>
		/// <param name="mem">この属性が適用されていたメンバを表す System.Reflection.MemberInfo を指定します。</param>
		protected abstract void Read(System.Reflection.MemberInfo mem);
		/// <summary>
		/// プラグイン読込中のメッセージなどを出力する為に使用する <see cref="Application.Log"/> です。
		/// </summary>
		protected static Application.Log Output{get{return Application.Log.AfhOut;}}
		//===========================================================
		//		PluginDirectory
		//===========================================================
		private const string PLUGINDIR="plugins";
		/// <summary>
		/// プラグインファイルを保存してあるディレクトリを表します。
		/// </summary>
		public static string PluginDirectory{
			get{return System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory,PLUGINDIR);}
		}
		//===========================================================
		//		Read
		//===========================================================
		private static object syncroot=new object();
		/// <summary>
		/// プラグインの読込を実行したか否かを表します。
		/// </summary>
		private static volatile bool read=false;
		/// <summary>
		/// プラグインの読込を実行します。
		/// </summary>
		public static void ReadAssemblies(){
			lock(PluginReaderAttribute.syncroot){
				if(PluginReaderAttribute.read)return;
				PluginReaderAttribute.read=true;
			}
			PluginReaderAttribute.ReadAssemblies(PluginReaderAttribute.PluginDirectory);
		}
		/// <summary>
		/// 指定したディレクトリからプラグインを読み込みます。
		/// </summary>
		/// <param name="dir">
		/// プラグインのファイルが配置してあるディレクトリを指定して下さい。
		/// 指定したディレクトリが存在しない場合には新しくディレクトリを作成します。
		/// </param>
		private static void ReadAssemblies(string dir){
			if(!System.IO.Directory.Exists(dir)){
				System.IO.Directory.CreateDirectory(dir);
				return;
			}
			//--declarations
			System.Reflection.Assembly asm;
			System.Type[] types;
			System.Reflection.MemberInfo[] mems;
			object[] attrs;
			Plugin.PluginReaderAttribute reader;
			Application.Log l=Application.Log.AfhOut;
			l.WriteError("プラグイン読込開始");
			l.AddIndent();
			l.WriteLine("------------------");
			//--reading loop
			foreach(string path in System.IO.Directory.GetFiles(dir,"*.dll")){
				try{
					asm=System.Reflection.Assembly.LoadFrom(path);
					l.WriteLine("Load Assembly: "+asm.ToString().Split(new char[]{','})[0]+" at "+path);
					types=asm.GetTypes();
					for(int i=0,iM=types.Length;i<iM;i++){
						if(types[i].Name!=CLASSNAME)continue;
						Application.Log.AfhOut.WriteLine("Load Type    : "+types[i].FullName);
						//if(!type.IsClass||!type.IsPublic||type.IsAbstract||type.GetInterface(iface)==null)continue;
						mems=types[i].GetMembers(STATIC_MEMBER);
						for(int j=0,jM=mems.Length;j<jM;j++){
							attrs=mems[j].GetCustomAttributes(typeof(Plugin.PluginReaderAttribute),false);
							if(attrs.Length==0)continue;
							reader=(Plugin.PluginReaderAttribute)attrs[0];
							Application.Log.AfhOut.WriteLine("Read Member  : "+mems[j].Name+" -> "+reader.GetType().FullName);
							reader.Read(mems[j]);
						}
					}
				}catch(System.Exception e){
					Application.Log.AfhOut.WriteError(e,"Assembly "+path+" の読込中にエラーが発生しました");
				}
			}
			l.WriteLine("------------------");
			l.WriteLine("プラグイン読込終了");
			l.RemoveIndent();
		}
		/// <summary>
		/// プラグインの初期化を行う為の静的メンバを持つクラスの名前を指定します。
		/// </summary>
		private const string CLASSNAME="PluginInformation";
		private const System.Reflection.BindingFlags STATIC_MEMBER
			=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.DeclaredOnly
			|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;

		//===========================================================
		//		protected Methods
		//===========================================================
		/// <summary>
		/// 指定したメンバ情報がメソッド情報であるか確認し、メソッド情報にして返します。
		/// </summary>
		/// <param name="mem">メソッドに関するメンバ情報を指定します。</param>
		/// <returns>メソッド情報を返します。
		/// 指定したメンバ情報がメソッド情報でなかった場合には、null を返します。</returns>
		protected static System.Reflection.MethodInfo ToMethod(System.Reflection.MemberInfo mem){
			if(mem.MemberType!=System.Reflection.MemberTypes.Method) {
				Output.WriteLine("    error: この属性はメソッド (亦はプロパティの getter/setter) に指定して下さい");
				return null;
			}
			return (System.Reflection.MethodInfo)mem;
		}
		/// <summary>
		/// メソッドの実行の失敗を表現するオブジェクトです。
		/// </summary>
		protected static readonly object failed=new object();
		/// <summary>
		/// メソッドを呼び出します。メソッドの呼び出しに失敗した場合にはその説明も出力します。
		/// </summary>
		/// <param name="meth">呼び出すメソッドに関する情報を指定します。</param>
		/// <param name="obj">メソッドのインスタンスオブジェクトを指定します。
		/// 静的関数を呼び出す時には null を指定して下さい (何を指定しても無視されます)。</param>
		/// <param name="arguments">メソッドに渡す引数を指定します。</param>
		/// <returns>無事に値を取得する事が出来た場合にはその返値を、呼び出しに失敗した場合には PluginReaderAttribute.failed を返します。</returns>
		protected static object InvokeMethod(System.Reflection.MethodInfo meth,object obj,params object[] arguments){
			const string ERROR_PARAM="error: 指定されたメソッドは {0} つの引数を要求します。{1} つ以下の引数を要求するメソッドに指定して下さい。";
			const string ERROR_INVOK="error: 指定されたメソッドの呼び出しに失敗しました。";

			int paramlen=meth.GetParameters().Length;
			if(paramlen>arguments.Length) {
				Output.WriteLine(ERROR_PARAM,paramlen,arguments.Length);
				return failed;
			}

			try{
				return meth.Invoke(null,arguments);
			}catch(System.Exception e){
				Output.WriteError(e,ERROR_INVOK);
				return failed;
			}
		}
	}

}