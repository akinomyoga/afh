using Gen=System.Collections.Generic;
namespace afh.Collections{
	/// <summary>
	/// �p�~�\��ł��B���O�� PluralDictionary ���� DictionaryP �ɕς��܂����B
	/// </summary>
	/// <typeparam name="TKey">�o�^���鎞�̌��o���ƂȂ�C���X�^���X�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TVal">�o�^�����l�̌^���w�肵�܂��B</typeparam>
	[System.Obsolete("���O�� PluralDictionary ���� DictionaryP �ɕς��܂����B")]
	public class PluralDictionary<TKey,TVal>:DictionaryP<TKey,TVal>{
		/// <summary>
		/// PluralDictionary �̃R���X�g���N�^�ł��B
		/// </summary>
		[System.Obsolete("���O�� PluralDictionary ���� DictionaryP �ɕς��܂����B")]
		public PluralDictionary(){}
	}
	/// <summary>
	/// �w�肵���^�ւ̕ϊ���񋟂��܂��B
	/// </summary>
	/// <typeparam name="T">�ϊ���̌^���w�肵�܂��B</typeparam>
	[System.Obsolete("afh.Converter ���g�p��������ɒu�������鎖�����߂܂��B")]
	public interface IConvertible<T>{
		/// <summary>
		/// ���̃C���X�^���X�� <typeparamref name="T"/> �̃C���X�^���X�ɕϊ����܂��B
		/// </summary>
		/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
		T ConvertTo();
	}
	/// <summary>
	/// �p�~�\��ł��Bafh.Converter ���g�p���ĉ�����
	/// </summary>
	/// <typeparam name="TFrom">�ϊ��O�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TDest">�ϊ���̌^���w�肵�܂��B</typeparam>
	/// <param name="value">�ϊ��O�̒l���w�肵�܂��B</param>
	/// <returns>�ϊ���̒l��Ԃ��܂��B</returns>
	[System.Obsolete("afh.Converter ���g�p���ĉ�����")]
	public delegate TDest DlgConverter<TFrom,TDest>(TFrom value);
}

namespace afh{
	/// <summary>
	/// ������Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<string> ���g�p���ĉ�����")]
	public delegate void StringEH(object sender,string value);
	/// <summary>
	/// sbyte �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<sbyte> ���g�p���ĉ�����")]
	public delegate void SByteEH(object sender,sbyte value);
	/// <summary>
	/// short �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<short> ���g�p���ĉ�����")]
	public delegate void Int16EH(object sender,short value);
	/// <summary>
	/// int �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<int> ���g�p���ĉ�����")]
	public delegate void Int32EH(object sender,int value);
	/// <summary>
	/// long �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<long> ���g�p���ĉ�����")]
	public delegate void Int64EH(object sender,long value);
	/// <summary>
	/// byte �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<byte> ���g�p���ĉ�����")]
	public delegate void ByteEH(object sender,byte value);
	/// <summary>
	/// ushort �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<ushort> ���g�p���ĉ�����")]
	public delegate void UInt16EH(object sender,ushort value);
	/// <summary>
	/// uint �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<uint> ���g�p���ĉ�����")]
	public delegate void UInt32EH(object sender,uint value);
	/// <summary>
	/// ulong �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<ulong> ���g�p���ĉ�����")]
	public delegate void UInt64EH(object sender,ulong value);
	/// <summary>
	/// float �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<float> ���g�p���ĉ�����")]
	public delegate void SingleEH(object sender,float value);
	/// <summary>
	/// double �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<double> ���g�p���ĉ�����")]
	public delegate void DoubleEH(object sender,double value);
	/// <summary>
	/// char �Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<char> ���g�p���ĉ�����")]
	public delegate void CharEH(object sender,char value);
	/// <summary>
	/// ��ʂ̃I�u�W�F�N�g�Ɋւ���C�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	[System.Obsolete("afh.EventHandler<object> ���g�p���ĉ�����")]
	public delegate void ObjectEH(object sender,object value);
}

namespace afh{
	/// <summary>
	/// �p�~����܂����Bafh.Collections.Enumerable ���g�p���ĉ������B
	/// </summary>
	[System.Obsolete("afh.Collections.Enumerable")]
	public class Enumerable{
		/// <summary>
		/// �w�肵�� System.Collections.IEnumerator ��Ԃ� System.Collections.IEnumerable ���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">GetEnumerator �ŕԂ� System.Collections.IEnumerator ���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator){throw new System.NotImplementedException("�p�~����܂���");}
		/// <summary>
		/// �l��I��ŗ񋓂���񋓎q��Ԃ��C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">���̗񋓎q���w�肵�܂��B</param>
		/// <param name="filter">����̒l��񋓂��邩���Ȃ����𔻒肷��ׂ� IFilter ���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,IFilter filter){throw new System.NotImplementedException("�p�~����܂���");}
		/// <summary>
		/// ����̌^�̒l��񋓂���񋓎q��Ԃ��C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">���̗񋓎q���w�肵�܂��B</param>
		/// <param name="type">�񋓂���l�̌^���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,System.Type type){throw new System.NotImplementedException("�p�~����܂���");}
		/// <summary>
		/// �񋓂����l�����O�ɒ��˂铭��������I�u�W�F�N�g��\���܂��B
		/// </summary>
		[System.Obsolete]
		public interface IFilter{}
#if ENUMERATOR
		/// <summary>
		/// �w�肵�� System.Collections.IEnumerator ��Ԃ� System.Collections.IEnumerable ���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">GetEnumerator �ŕԂ� System.Collections.IEnumerator ���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator){this.@enum=enumerator;}
		/// <summary>
		/// �l��I��ŗ񋓂���񋓎q��Ԃ��C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">���̗񋓎q���w�肵�܂��B</param>
		/// <param name="filter">����̒l��񋓂��邩���Ȃ����𔻒肷��ׂ� IFilter ���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,IFilter filter){
			this.@enum=new FiltEnumerator(enumerator,filter);
		}
		/// <summary>
		/// ����̌^�̒l��񋓂���񋓎q��Ԃ��C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="enumerator">���̗񋓎q���w�肵�܂��B</param>
		/// <param name="type">�񋓂���l�̌^���w�肵�܂��B</param>
		[System.Obsolete]
		public Enumerable(System.Collections.IEnumerator enumerator,System.Type type){
			this.@enum=new FiltEnumerator(enumerator,new TypeFilter(type));
		}
		//GENERICS:
		private sealed class TypeFilter:IFilter{
			private System.Type type;
			public TypeFilter(System.Type type){this.type=type;}
			bool IFilter.Filt(object obj){
				if(obj==null)return false;
				System.Type t=obj.GetType();
				return t.IsSubclassOf(this.type)||t.GetInterface(this.type.FullName)!=null;
			}
		}
		private sealed class FiltEnumerator:System.Collections.IEnumerator{
			private System.Collections.IEnumerator @enum;
			private IFilter filter;
			public FiltEnumerator(System.Collections.IEnumerator enumerator,IFilter filter){
				this.@enum=enumerator;
				this.filter=filter;
			}
			public object Current{get{return this.@enum.Current;}}
			public bool MoveNext(){
				while(this.@enum.MoveNext())if(this.filter.Filt(this.@enum.Current))return true;
				return false;
			}
			public void Reset(){this.@enum.Reset();}
		}
		/// <summary>
		/// �񋓂����l�����O�ɒ��˂铭��������I�u�W�F�N�g��\���܂��B
		/// </summary>
		public interface IFilter{
			/// <summary>
			/// �w�肵���l��񋓂��邩���Ȃ����𔻒肵�܂��B
			/// </summary>
			/// <param name="obj">�񋓂̌��ł���l���w�肵�܂��B</param>
			/// <returns>�w�肵���l��񋓂���ꍇ�� true ��Ԃ��܂��B�񋓂��Ȃ��ꍇ�ɂ� false ��Ԃ��܂��B</returns>
			bool Filt(object obj);
		}
#endif
		/// <summary>
		/// �w�肵���^�̕��̂ݗ񋓂��܂��B
		/// </summary>
		/// <typeparam name="T">�񋓂���^���w�肵�܂��B</typeparam>
		/// <returns>�w�肵���v�f�̂ݗ񋓂���񋓎q��Ԃ��܂��B</returns>
		[System.Obsolete("afh.Collections.Enumerable.EnumByType �Ɉړ����܂����B")]
		public static Gen::IEnumerable<T> EnumByType<T>(System.Collections.IEnumerable baseEnumerable){
			return afh.Collections.Enumerable.EnumByType<T>(baseEnumerable);
		}
	}
}
namespace afh.Application{
	/// <summary>
	/// System.Type �̕�����\���ɑ΂��鑀���񋟂��܂��B
	/// �p�~����܂����cafh.Types ���g�p���ĉ������B
	/// </summary>
	[System.Obsolete("afh.Types ���g�p���ĉ������B")]
	public class Types {
		/// <summary>
		/// �^�̐ÓI�� hash �l�Ƃ��Ďg���� byte ���v�Z���ĕԂ��܂��B
		/// </summary>
		/// <param name="t">Hash �l�����߂��� System.Type ���w�肵�܂��B</param>
		/// <returns>�v�Z���� hash byte ��Ԃ��܂��B</returns>
		public static byte GetHashByte(System.Type t) {
			int hash=t.FullName.GetHashCode();
			return (byte)(hash^hash>>8^hash>>16^hash>>24);
		}
		/// <summary>
		/// System.Type ��\����������擾���܂��B
		/// C# �̗\���Ɉ�v����^�͗\���ŕ\���܂��B
		/// </summary>
		/// <param name="t">������ɒ����O�� System.Type ���w�肵�܂��B</param>
		/// <returns>System.Type �𕶎���ŕ\���������擾���܂��B</returns>
		public static string CSharpString(System.Type t) {
			return afh.Types.CSharpName(t);
		}
		/// <summary>
		/// FullName �ł͂Ȃ��āA���O��ԂȂǂ��܂܂Ȃ����̌^���̖̂��O���擾���܂��B
		/// </summary>
		/// <param name="fullname">
		/// System.Type.FullName ���ɂ���Ď擾�ł���^��\����������w�肵�܂��B
		/// �^�̖��O�Ƃ��ĕs�K�؂ȕ����w�肵���ꍇ�̌��ʂɂ��Ă͕ۏ؂��܂���B
		/// </param>
		/// <returns>�^���̖̂��O��Ԃ��܂��B</returns>
		public static string GetTypeName(string fullname) {
			string x=System.IO.Path.GetExtension(fullname);
			if(x=="") x=fullname;
			int i=x.IndexOf("+");
			return i>=0?x.Substring(i+1):x;
		}
		/// <summary>
		/// �p�����[�^�̃��X�g�𕶎���ɂ��Ď擾���܂��B
		/// </summary>
		/// <param name="m">���\�b�h��\�� System.Reflection.MethodBase ���w�肵�ĉ������B</param>
		/// <returns>�p�����[�^�̃��X�g��\���������Ԃ��܂��B</returns>
		public static string GetParameters(System.Reflection.MethodBase m) {
			return afh.Types.GetParameterList(m);
		}
	}

	//===============================================================
	//		old Log
	//===============================================================
#if old
	/// <summary>
	/// �A�v���P�[�V�������œf���o�����l�X�ȃ��b�Z�[�W���Ǘ����܂��B
	/// </summary>
	public class Log2{
		private string name;
		/// <summary>
		/// ���� Log �Ɋ֘A�t����ꂽ���O���擾���܂��B
		/// </summary>
		public string Name{
			get{return this.name;}
		}
		private System.Text.StringBuilder str;
		/// <summary>
		/// Log �R���X�g���N�^
		/// </summary>
		/// <param name="name">���̃��O�Ɋ֘A�t���閼�O���w�肵�܂��B</param>
		private Log2(string name){
			this.name=name;
			this.str=new System.Text.StringBuilder();
		}
		/// <summary>
		/// ������ Log �̓��e���������܂��B
		/// </summary>
		public void Clear(){
			this.str=new System.Text.StringBuilder();
		}
		/// <summary>
		/// �����ɏo�͂���������̓��e���擾���܂��B
		/// </summary>
		public string ContentString{
			get{return this.str.ToString();}
		}
		//===========================================================
		//		�����p���\�b�h
		//===========================================================
		//
		//	INDENT
		//
		private string indent="";
		/// <summary>
		/// �C���f���g�������܂��B
		/// </summary>
		public void AddIndent(){
			indent+="    ";
		}
		/// <summary>
		/// �C���f���g����菜���܂��B
		/// </summary>
		public void RemoveIndent(){
			if(indent.Length>=4)indent=indent.Substring(4);
		}
		//
		//	WRITELINE
		//
		/// <summary>
		/// �s�����������܂��B
		/// </summary>
		/// <param name="message">����������s�̓��e���w�肵�܂��B</param>
		public void WriteLine(string message){
			if(this.indent.Length>0){
				message=message.Replace("\r\n","\r\n"+this.indent);
				this.str.Append(this.indent);
			}
			this.str.Append(message);
			this.str.Append("\r\n");
			this.OnChanged();
		}
		/// <summary>
		/// ������������g�p���čs�����������܂��B
		/// </summary>
		/// <param name="format">������������e�̏������w�肵�܂��B</param>
		/// <param name="args">�o�͂���l���w�肵�܂��B</param>
		public void WriteLine(string format,params object[] args){
			if(this.indent.Length>0){
				format=format.Replace("\n","\n"+this.indent);
				this.str.Append(this.indent);
			}
			this.str.AppendFormat(format,args);
			this.str.Append("\r\n");
			this.OnChanged();
		}
		//
		//	WRITEERROR
		//
		/// <summary>
		/// �G���[������������ʒm���܂��B
		/// ��O�Ƃ��ē�������A�[���ȏ󋵂ł͂Ȃ��ꍇ�Ɏg�p���鎖��z�肵�Ă��܂��B
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		public void WriteError(string message){
			System.Diagnostics.StackTrace trace=new System.Diagnostics.StackTrace(false);
			if(trace.FrameCount>1){
				System.Reflection.MethodBase m=trace.GetFrame(1).GetMethod();
				string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
				string type=m.DeclaringType.FullName.Replace(".","::");
				this.WriteLine("<"+dll+"> "+type+"."+m.Name+"("+afh.Application.Types.GetParameters(m)+")");
			}
			this.WriteLine("    "+message);
		}
		/// <summary>
		/// ��O��������������ʒm���A��O�̏ڍׂ��o�͂��܂��B
		/// </summary>
		/// <param name="e">����������O���w�肵�܂��B</param>
		public void WriteError(System.Exception e){
			System.Diagnostics.StackTrace trace=new System.Diagnostics.StackTrace(false);
			if(trace.FrameCount>1){
				System.Reflection.MethodBase m=trace.GetFrame(1).GetMethod();
				string dll=System.IO.Path.GetFileName(m.DeclaringType.Assembly.CodeBase);
				string type=m.DeclaringType.FullName.Replace(".","::");
				this.WriteLine("<"+dll+"> "+type+"."+m.Name+"("+afh.Application.Types.GetParameters(m)+")");
			}
			this.AddIndent();{
				this.WriteVar("ExceptionType",e.GetType().ToString());
				this.WriteVar("Message",e.Message);
				this.WriteVar("Source",e.Source);
				if(e.HelpLink!="")this.WriteVar("HelpLink",e.HelpLink);
				this.WriteLine("StackTrace:");
				this.AddIndent();{
					this.WriteLine(e.StackTrace);
				}this.RemoveIndent();
			}this.RemoveIndent();
		}
		/// <summary>
		/// �ϐ��̖��O�ƒl�̑g�ݍ��킹���o�͂��܂��B
		/// </summary>
		/// <param name="name">�ϐ��̖��O�ɑΉ����镶������w�肵�܂��B</param>
		/// <param name="value">�ϐ��̒l�ɑΉ����镶������w�肵�܂��B</param>
		public void WriteVar(string name,string value){
			this.WriteLine(name+":\t"+value);
		}
		//===========================================================
		//		�ύX�̒ʒm
		//===========================================================
		private int locked=0;
		/// <summary>
		/// ���e�ɕύX�������Ă��AChanged �C�x���g���������Ȃ��l�ɂ��܂��B
		/// </summary>
		public void Lock(){this.locked++;}
		/// <summary>
		/// Locked �Őݒ肵����Ԃ����ɖ߂�
		/// ���e�ɕύX������������ Changed �C�x���g����������l�ɂ��܂��B
		/// (Locked ���s�����񐔂Ɠ����񐔂��� Unlocked ���s��Ȃ��Ə�Ԃ͌��ɖ߂�܂���B)
		/// ���ɖ߂������ɂ̓��b�N����Ă����Ԃɓ��e�ɕύX�����������ǂ����ɍS��炸 Changed �C�x���g���������܂��B
		/// </summary>
		public void Unlock(){
			if(this.locked==0)return;
			this.locked--;
			if(this.locked==0)this.OnChanged();
		}
		/// <summary>
		/// ���e�ɕύX�����������ɔ������܂��B
		/// </summary>
		public event afh.VoidEH Changed;
		/// <summary>
		/// Changed �C�x���g�𔭐������܂��B
		/// </summary>
		protected virtual void OnChanged(){
			if(this.locked>0||this.Changed==null)return;
			this.Changed(this);
		}
	}
#endif
}
