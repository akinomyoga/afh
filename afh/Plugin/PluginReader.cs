
namespace afh.Plugin{
	/// <summary>
	/// ���ꂼ��̃v���O�C���̎�ނɉ����āA
	/// �v���O�C����ǂݍ��ވׂ̑����̊�{�N���X�ł��B
	/// �ÓI�����o�̓X���b�h�Z�[�t�ł��B
	/// </summary>
	public abstract class PluginReaderAttribute:System.Attribute{
		/// <summary>
		/// PluginReaderAttribute �̃R���X�g���N�^�B
		/// </summary>
		protected PluginReaderAttribute(){}
		/// <summary>
		/// �w�肳�ꂽ�����o���g�p���āA�K���ȏ��������s���܂��B
		/// </summary>
		/// <param name="mem">���̑������K�p����Ă��������o��\�� System.Reflection.MemberInfo ���w�肵�܂��B</param>
		protected abstract void Read(System.Reflection.MemberInfo mem);
		/// <summary>
		/// �v���O�C���Ǎ����̃��b�Z�[�W�Ȃǂ��o�͂���ׂɎg�p���� <see cref="Application.Log"/> �ł��B
		/// </summary>
		protected static Application.Log Output{get{return Application.Log.AfhOut;}}
		//===========================================================
		//		PluginDirectory
		//===========================================================
		private const string PLUGINDIR="plugins";
		/// <summary>
		/// �v���O�C���t�@�C����ۑ����Ă���f�B���N�g����\���܂��B
		/// </summary>
		public static string PluginDirectory{
			get{return System.IO.Path.Combine(afh.Application.Path.ExecutableDirectory,PLUGINDIR);}
		}
		//===========================================================
		//		Read
		//===========================================================
		private static object syncroot=new object();
		/// <summary>
		/// �v���O�C���̓Ǎ������s�������ۂ���\���܂��B
		/// </summary>
		private static volatile bool read=false;
		/// <summary>
		/// �v���O�C���̓Ǎ������s���܂��B
		/// </summary>
		public static void ReadAssemblies(){
			lock(PluginReaderAttribute.syncroot){
				if(PluginReaderAttribute.read)return;
				PluginReaderAttribute.read=true;
			}
			PluginReaderAttribute.ReadAssemblies(PluginReaderAttribute.PluginDirectory);
		}
		/// <summary>
		/// �w�肵���f�B���N�g������v���O�C����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="dir">
		/// �v���O�C���̃t�@�C�����z�u���Ă���f�B���N�g�����w�肵�ĉ������B
		/// �w�肵���f�B���N�g�������݂��Ȃ��ꍇ�ɂ͐V�����f�B���N�g�����쐬���܂��B
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
			l.WriteError("�v���O�C���Ǎ��J�n");
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
					Application.Log.AfhOut.WriteError(e,"Assembly "+path+" �̓Ǎ����ɃG���[���������܂���");
				}
			}
			l.WriteLine("------------------");
			l.WriteLine("�v���O�C���Ǎ��I��");
			l.RemoveIndent();
		}
		/// <summary>
		/// �v���O�C���̏��������s���ׂ̐ÓI�����o�����N���X�̖��O���w�肵�܂��B
		/// </summary>
		private const string CLASSNAME="PluginInformation";
		private const System.Reflection.BindingFlags STATIC_MEMBER
			=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.DeclaredOnly
			|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;

		//===========================================================
		//		protected Methods
		//===========================================================
		/// <summary>
		/// �w�肵�������o��񂪃��\�b�h���ł��邩�m�F���A���\�b�h���ɂ��ĕԂ��܂��B
		/// </summary>
		/// <param name="mem">���\�b�h�Ɋւ��郁���o�����w�肵�܂��B</param>
		/// <returns>���\�b�h����Ԃ��܂��B
		/// �w�肵�������o��񂪃��\�b�h���łȂ������ꍇ�ɂ́Anull ��Ԃ��܂��B</returns>
		protected static System.Reflection.MethodInfo ToMethod(System.Reflection.MemberInfo mem){
			if(mem.MemberType!=System.Reflection.MemberTypes.Method) {
				Output.WriteLine("    error: ���̑����̓��\�b�h (���̓v���p�e�B�� getter/setter) �Ɏw�肵�ĉ�����");
				return null;
			}
			return (System.Reflection.MethodInfo)mem;
		}
		/// <summary>
		/// ���\�b�h�̎��s�̎��s��\������I�u�W�F�N�g�ł��B
		/// </summary>
		protected static readonly object failed=new object();
		/// <summary>
		/// ���\�b�h���Ăяo���܂��B���\�b�h�̌Ăяo���Ɏ��s�����ꍇ�ɂ͂��̐������o�͂��܂��B
		/// </summary>
		/// <param name="meth">�Ăяo�����\�b�h�Ɋւ�������w�肵�܂��B</param>
		/// <param name="obj">���\�b�h�̃C���X�^���X�I�u�W�F�N�g���w�肵�܂��B
		/// �ÓI�֐����Ăяo�����ɂ� null ���w�肵�ĉ����� (�����w�肵�Ă���������܂�)�B</param>
		/// <param name="arguments">���\�b�h�ɓn���������w�肵�܂��B</param>
		/// <returns>�����ɒl���擾���鎖���o�����ꍇ�ɂ͂��̕Ԓl���A�Ăяo���Ɏ��s�����ꍇ�ɂ� PluginReaderAttribute.failed ��Ԃ��܂��B</returns>
		protected static object InvokeMethod(System.Reflection.MethodInfo meth,object obj,params object[] arguments){
			const string ERROR_PARAM="error: �w�肳�ꂽ���\�b�h�� {0} �̈�����v�����܂��B{1} �ȉ��̈�����v�����郁�\�b�h�Ɏw�肵�ĉ������B";
			const string ERROR_INVOK="error: �w�肳�ꂽ���\�b�h�̌Ăяo���Ɏ��s���܂����B";

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