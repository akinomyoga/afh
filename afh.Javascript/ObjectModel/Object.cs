#define doublehash
using Generic=System.Collections.Generic;
namespace afh.JavaScript{
	/// <summary>
	/// Javascript ��̃N���X�I�u�W�F�N�g�̊�{�Ƃ��Ȃ�ׂ� Object ��\���N���X�ł��B
	/// </summary>
	public class Object{
		//===========================================================
		//		field:members
		//===========================================================
		/// <summary>
		/// members: �ʏ�̃����o
		/// </summary>
		private Generic::Dictionary<string,JavaScript.Object> members;
		/// <summary>
		/// �w�肵�����O�̃����o�� _members �ɐݒ肳��Ă��邩�ǂ������m�F
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <returns>�w�肵�����O�̃����o���܂܂�Ă��邩�ǂ�����Ԃ��܂�</returns>
		protected bool members_contain(string propName){
			if(this.members==null)return false;
			return this.members.ContainsKey(propName);
		}
		/// <summary>
		/// �w�肵�����O�����o�� _members ����擾���܂�
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <returns>�擾���������o���A���܂�</returns>
		protected JavaScript.Object members_get(string propName){
			if(this.members==null||!this.members.ContainsKey(propName))return null;
			return this.members[propName];
		}
		/// <summary>
		/// _members �̎w�肵�����O�ɐV���������o��ݒ肵�܂�
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <param name="obj">�ݒ肷��I�u�W�F�N�g</param>
		protected void members_set(string propName,JavaScript.Object obj){
			if(this.members==null)this.members=new Generic::Dictionary<string,JavaScript.Object>();
			this.members[propName]=obj;
		}
#if doublehash
		//===========================================================
		//		field:pmembers
		//===========================================================
		/// <summary>
		/// private members: �V�X�e���̑��ŉB���Ēu�������o
		/// </summary>
		private Generic::Dictionary<string,JavaScript.Object> pmembers;
		/// <summary>
		/// �w�肵�����O�̃����o�� _members �ɐݒ肳��Ă��邩�ǂ������m�F
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <returns>�w�肵�����O�̃����o���܂܂�Ă��邩�ǂ�����Ԃ��܂�</returns>
		protected bool pmembers_contain(string propName){
			if(this.pmembers==null)return false;
			return this.pmembers.ContainsKey(propName);
		}
		/// <summary>
		/// �w�肵�����O�����o�� _members ����擾���܂�
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <returns>�擾���������o���A���܂�</returns>
		protected JavaScript.Object pmembers_get(string propName){
			if(this.pmembers==null||!this.pmembers.ContainsKey(propName))return null;
			return this.pmembers[propName];
		}
		/// <summary>
		/// _members �̎w�肵�����O�ɐV���������o��ݒ肵�܂�
		/// </summary>
		/// <param name="propName">�����o�̖��O</param>
		/// <param name="obj">�ݒ肷��I�u�W�F�N�g</param>
		protected void pmembers_set(string propName,JavaScript.Object obj){
			if(this.pmembers==null)this.pmembers=new Generic::Dictionary<string,JavaScript.Object>();
			this.pmembers[propName]=obj;
		}
#endif
		//===========================================================
		//		field:_proto
		//===========================================================
		/// <summary>
		/// __proto__ (���̊֐��̌p������\������ׂ̃I�u�W�F�N�g) ��ێ����܂�
		/// </summary>
		private JavaScript.Object _proto;
		/// <summary>
		/// ���̃I�u�W�F�N�g���Q�Ƃ��� prototype ���擾���͐ݒ肵�܂��B
		/// </summary>
		/// <remarks>
		/// �ʏ�̕���p����� "__proto__ ����������ׂ� __proto__ ����������"
		/// �Ƃ����������[�v�ɂȂ�� __proto__ �͓��ʂɕʊ֐��ŏ���
		/// </remarks>
		public JavaScript.Object __proto__{
			get{
				JavaScript.Object o=this.members_get("__proto__");
				if(o!=null)return o;
#if doublehash
				if((o=this.pmembers_get("__proto__"))!=null)return o;
#endif
				if(this._proto!=null)return this._proto;
				return this._proto=new Null();		// �K�v�ɂȂ��Ă�����
			}
			set{this._proto=value;}
		}//*/
		//===========================================================
		//		.ctor
		//===========================================================
		public Object(){}
		public Object(JavaScript.Object __proto__):this(){
			this.__proto__=__proto__;
		}
		/// <summary>
		/// Object �̐V���� instance ���擾���܂��B
		/// </summary>
		/// <returns>�V�����C���X�^���X��Ԃ��܂��B</returns>
		public static JavaScript.Object Construct(){
			JavaScript.Object r=new Object();
			r.__proto__=Global._global["Object"]["prototype"];
			return r;
		}
		//===========================================================
		//		Member Access
		//===========================================================
		/// <summary>
		/// ���̃I�u�W�F�N�g�̎������o���������܂�
		/// </summary>
		public virtual JavaScript.Object this[string propName]{
			get{
				if(propName=="__proto__")return this.__proto__;
				//--�{���̃����o
				JavaScript.Object o=this.members_get(propName);
				if(o!=null)return o;
#if doublehash
				//--�B�������o
				if(null!=(o=this.pmembers_get(propName)))return o;
#endif
				//--�p�����̃����o
				if(null!=(o=this._proto))if(null!=(o=o[propName]))return o;
				return null;
			}
			set{this.members_set(propName,value);}
		}
		/// <summary>
		/// Object �؂�H���ĖړI�̃����o�̒l��Ԃ��܂�
		/// </summary>
		/// <param name="index">
		/// ���Ɍ�������ׂ� names �̈ʒu�B
		/// ���̃��\�b�h�����C���X�^���X��
		/// names[index] �ƌ������̃����o�����������҂���Ă��܂��B
		/// </param>
		/// <param name="names">�I�u�W�F�N�g�̌n���B<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <returns>
		/// �������������o��Ԃ��܂��B
		/// �w�肵�������o���̂�����`�������ꍇ�ɂ� null �l��Ԃ��܂��B
		/// </returns>
		/// <exception cref="Null.UndefinedException">
		/// �w�肵�������o�ɒH�蒅���O�ɖ���`���������ꍇ�ɔ������܂��B
		/// �w�肵�������o���̂�����`�������ꍇ�ɂ͔������܂���B
		/// </exception>
		public JavaScript.Object GetValue(int index,string[] names){
			JavaScript.Object o=this.TraceMember(index,names.Length-1,names);
			if(this is JavaScript.ManagedObject&&o is JavaScript.ManagedMethod){
				//--ManagedMethod �̏ꍇ
				return new JavaScript.ManagedDelegate(
					((JavaScript.ManagedObject)this).Value,
					(JavaScript.ManagedMethod)o
				);
			}else return o;
		}
		/// <summary>
		/// Object �؂�H���ĖړI�̃����o��ݒ肵�܂�
		/// </summary>
		/// <param name="obj">�ݒ�l��\���I�u�W�F�N�g�ł�</param>
		/// <param name="index">
		/// ���Ɍ�������ׂ� names �̈ʒu�B
		/// ���̃��\�b�h�����C���X�^���X��
		/// names[index] �ƌ������̃����o�����������҂���Ă��܂��B
		/// </param>
		/// <param name="names">�I�u�W�F�N�g�̌n���B<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <exception cref="Null.UndefinedException">
		/// �w�肵�������o������`�������ꍇ�ɔ������܂��B
		/// </exception>
		public void SetValue(JavaScript.Object obj,int index,string[] names){
			//--���ڂ̐e�� key �̎擾
			int dest=names.Length-2;
			JavaScript.Object parent=index<=dest?this.TraceMember(index,dest,names):this;
			string key=names[dest+1];
			//--�l�̐ݒ�
			if(parent[key] is JavaScript.PropertyBase){
				((JavaScript.PropertyBase)parent[key]).SetValue(parent,obj);
			}else parent[key]=obj;
		}
		/// <summary>
		/// Object �؂�H���ĖړI�̃����o�֐������s���܂�
		/// </summary>
		/// <param name="index">
		/// ���Ɍ�������ׂ� names �̈ʒu�B
		/// ���̃��\�b�h�����C���X�^���X��
		/// names[index] �ƌ������̃����o�����������҂���Ă��܂��B
		/// </param>
		/// <param name="names">�I�u�W�F�N�g�̌n���B<see cref="afh.Javascript.Object.TraceMember"/></param>
		/// <returns>�������������o�����s�������ʂ�Ԃ��܂�</returns>
		/// <exception cref="Null.UndefinedException">
		/// �w�肵�������o������`�������ꍇ�ɔ������܂��B
		/// </exception>
		public JavaScript.Object InvokeMember(int index,string[] names,JavaScript.Array arguments){
			//--���ڂ̐e�� key �̎擾
			int dest=names.Length-2;
			JavaScript.Object parent=index<=dest?this.TraceMember(index,dest,names):this;
			string key=names[dest+1];
			//--Invoke
			try{
				return ((FunctionBase)parent[key]).Invoke(parent,arguments);
			}catch(System.NullReferenceException e){
				//��: ���ڂ� NullReferenceException �͎��Ԃ�������
				throw new Null.UndefinedException(e);
			}catch(System.InvalidCastException e){
				throw new System.Exception("�w�肵���I�u�W�F�N�g�͊֐��ł͂���܂���",e);
			}catch(System.Exception e){throw e;}
		}
		/// <summary>
		/// �q����H���Ă����܂�
		/// </summary>
		/// <param name="index">
		/// ���Ɍ�������ׂ� names �̈ʒu�B
		/// ���̃��\�b�h�����C���X�^���X��
		/// names[index] �ƌ������̃����o�����������҂���Ă��܂��B
		/// ���̒l���w�肵������ 0 �ƌ��Ȃ���܂��B
		/// </param>
		/// <param name="dest">names �̉��Ԗ�(Base0)�ɑΉ����� Javascript.Object ���擾���邩���w�肵�܂�</param>
		/// <param name="names">
		/// �I�u�W�F�N�g�̌n���B
		/// Object.prototype.myProperty �̏ꍇ�� <code>new string[]{"Object","prototype","myProperty"}</code> �ƕ\������܂�
		/// </param>
		/// <returns>
		/// dest �Ŏw�肵�� Javascript.Object ��Ԃ��܂��B
		/// �w�肵�������v���p�e�B�̏ꍇ�A���̒l��Ԃ��܂��B
		/// </returns>
		/// <exception cref="Null.UndefinedException">
		/// �w�肵�������o������`�������ꍇ�ɔ������܂��B
		/// </exception>
		internal virtual JavaScript.Object TraceMember(int index,int dest,string[] names){
			JavaScript.Object o=this[names[index]];
			if(o==null)
				throw new Null.UndefinedException(null,index,names);
			if(o is JavaScript.PropertyBase)
				o=((JavaScript.PropertyBase)o).GetValue(this);
			return index<dest?o.TraceMember(index+1,dest,names):o;
		}
		//===========================================================
		//		.NET ���݉^�p
		//===========================================================
		/// <summary>
		/// �w�肵���^�ɕϊ�����ۂ̓K������Ԃ��܂��B
		/// �w�肵���^�ɕϊ����鎖���o���Ȃ��ꍇ�ɂ� float.NegativeInfinity ��Ԃ��܂��B
		/// ����́AJavascript ���� .NET �̃I�[�o�[���[�h���ꂽ�֐����Ăяo���ۂɁA
		/// �����̌^���Q�l�ɂ��Ċ֐���I������̂Ɏg�p���܂��B
		/// </summary>
		/// <remarks>
		/// �ʏ�� Object �ł́Aobject (System.Object) �ɕϊ�����ۂɂ� 1,
		/// string (System.String) �ɕϊ�����ۂɂ� 0,
		/// bool (System.Boolean) �ɕϊ�����ۂɂ� -1
		/// �̓K�����ɂȂ��Ă��܂��B
		/// <para>
		/// �� Processor �̖��� set �̊֌W�ŁAfloat ���� double �ɂ���������������
		/// </para>
		/// </remarks>
		/// <param name="t">�ϊ���̌^</param>
		/// <returns>�ϊ��̓K������Ԃ��܂�</returns>
		public virtual float ConvertCompat(System.Type t){
			if(t==typeof(object)||t==typeof(JavaScript.Object))return 1;
			if(t==typeof(string))return 0;
			if(t==typeof(bool))return -1;
			return float.NegativeInfinity;
		}
		/// <summary>
		/// �w�肵���^�ɕϊ����܂��B
		/// �w�肵���^�ɕϊ����鎖���o���Ȃ��ꍇ�� null �l��Ԃ��܂��B
		/// </summary>
		/// <param name="t">�ϊ���̌^</param>
		/// <returns>�ϊ��������ʂ̃I�u�W�F�N�g��Ԃ��܂��B</returns>
		public virtual object Convert(System.Type t){
			if(t==typeof(object)||t==typeof(JavaScript.Object))return this;
			if(t==typeof(string))return this.ToString();
			if(t==typeof(bool))return true;
			return null;
		}
		//===========================================================
		//		Other Methods
		//===========================================================
#if doublehash
		// �ʏ탁���o�݂̂̃R�s�[
		public void CopyMembers(JavaScript.Object source){
			foreach(string key in source.members.Keys){
				this.members[key]=source.members[key];
			}
		}
#else
		public void CopyMembers(Javascript.Object source){
			foreach(string key in source.PublicOwnKeys){
				this.members[key]=this.members[source.members[key]];
			}
		}
		//===========================================================
		//		��
		//===========================================================
		public System.Collections.IEnumerable PublicOwnKeys{get{return new ObjectEnumerable(this);}}
		private class ObjectEnumerable:System.Collections.IEnumerable{
			private Javascript.Object obj;
			public ObjectEnumerable(Javascript.Object o){this.obj=o;}
			public System.Collections.IEnumerator GetEnumerator(){return new ObjectEnumerator(this.obj);}
		}
		private class ObjectEnumerator:System.Collections.IEnumerator{
			private const string OUTOFRANGE="�񋓎q���A�R���N�V�����̍ŏ��̗v�f�̑O�A�܂��͍Ō�̗v�f�̌�Ɉʒu���Ă��܂��B";
			private System.Collections.Hashtable hash;
			private System.Collections.IEnumerator ie;
			public object Current{get{return ie.Current;}}
			public ObjectEnumerator(Javascript.Object o){
				this.hash=o.members;
				this.ie=this.hash.Keys.GetEnumerator();
			}
			public void Reset(){this.ie.Reset();}
			public bool MoveNext(){
				do{
					if(!this.ie.MoveNext())return false;
					System.Type t=this.hash[ie.Current].GetType();
				}while(
					t.IsSubclassOf(typeof(Javascript.ManagedMethod))
					||t.IsSubclassOf(typeof(Javascript.ManagedProperty))
				);
				return true;
			}
		}
#endif
		//public virtual Javascript.Object Clone(){}
		internal static void Initialize(){
			JavaScript.Object o=new ManagedDelegate(typeof(JavaScript.Object),"Construct");

			//CHECK_OK:
			// ������g�p����� object �N���X�� ToString() Method ���g�p�����
			// �I�[�o�[���C�h���ꂽ�����g�p����Ȃ������m��Ȃ��c
			// �����ۂɂ���Ă݂����A�����ƌp����ŃI�[�o�[���C�h���������Ăяo���ꂽ�B
			o["prototype"]["toString"]=new ManagedMethod(typeof(object),"ToString");
			Global._global["Object"]=o;
		}
	}

	/// <summary>
	/// Javascript ��ł� null �l��\���N���X
	/// </summary>
	public class Null:Object{
		public Null():base(){}
		public override Object this[string propName]{
			get{return null;}
			set{throw new NullRefException();}
		}
		public override object Convert(System.Type t){
			if(t==typeof(bool))return false;
			return base.Convert(t);
		}
		public class NullRefException:System.Exception{
			public NullRefException(){}
		}
		public class UndefinedException:System.Exception{
			private const string MSG="�w�肵�����ʎq�͒�`����Ă��܂���";
			public UndefinedException(System.Exception e,int index,string[] names)
				:base(CreateMessage(index,names),e){}
			public UndefinedException(int index,string[] names)
				:base(CreateMessage(index,names)){}
			public UndefinedException(System.Exception e):base(MSG,e){}
			public UndefinedException():base(MSG){}
			/// <summary>
			/// ���ʎq�̌n�� names �� undefined �ł������ʒu index ����A
			/// ���ʎq����`����Ă��Ȃ��Ƃ������b�Z�[�W���쐬���܂�
			/// </summary>
			/// <param name="index">undefined �ł������ʒu</param>
			/// <param name="names">���ʎq�̌n��</param>
			/// <returns>���b�Z�[�W�� string �ŕԂ��܂�</returns>
			private static string CreateMessage(int index,string[] names){
				string r="UndefinedException";
				if(index>=names.Length){
					r+="���� Error: index �� names.Length �͈̔͂𒴂��Ă��܂�\n";
					r+="\tindex: "+index.ToString()+"\n";
					r+="\tnames.Length: "+names.Length.ToString()+"\n";
					index=names.Length-1;
				}
				if(names.Length>0){
					r+=names[0];
					for(int i=1;i<index;i++){
						r+="."+names[i];
					}
					r+=" �͒�`����Ă��܂���\n";
				}
				return r;
			}
		}
	}

	public class ManagedObject:JavaScript.Object{
		private object obj;
		public object Value{get{return this.obj;}}
		public ManagedObject(object obj):base(new Null()){
			this.obj=obj;
			// TODO: ���\�b�h�̌��J
			//  ���J���郁�\�b�h��ێ����� prototype ���쐬���āA
			//  System.Type �� key �ɐÓI�� Hashtable �ŊǗ�
		}

		public override string ToString() {
			return "<mgdObj:"+this.obj.ToString()+">";
		}
	}
	//public class ManagedNamespace:Javascript.Object{}
	// GetValue SetValue InvokeMember ���̏�����u��������
	/*
	public class Property:Object{

	}//*/
}
