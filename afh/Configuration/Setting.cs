using Path=afh.Application.Path;

namespace afh.Configuration{
	/// <summary>
	/// 設定を保存したりする為の XML ファイルを管理するクラスです。
	/// <para>指定した filepath にファイルが存在しない場合は、そこに新しくファイルを作成します。
	/// 新しく作成したファイルの内容は、defaultontent になります。</para>
	/// <para>既にファイルが存在していて、読み込みに失敗した場合には、defaultcontent の内容を読み込みます。
	/// もともとあったファイルは保存をしない限り上書きすることはありません。</para>
	/// </summary>
	public class Setting:System.Xml.XmlDocument{
		//private const string SETTING_FILENAME="setting.cfg";
		private static readonly string SETTING_FILENAME=System.IO.Path.GetFileNameWithoutExtension(Path.ExecutableFileName)+".cfg";
		internal const string XMLNS_KEY="http://www.example.com/key/";
		private const string DEFAULT_SETTING="<?xml version=\"1.0\"?>\n<setting key:class=\"afh.Configuration.Setting\" xmlns:key=\"{0}\"></setting>";
		/// <summary>
		/// 設定の保存先
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
		/// Setting のコンストラクタ
		/// </summary>
		private Setting(){
			this.filepath=System.IO.Path.Combine(Path.ExecutableDirectory,SETTING_FILENAME);
			//--保存先のファイルが存在しないとき
			if(!System.IO.File.Exists(this.filepath))this.makeXml();
			//--読み込み
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
		/// Setting のデストラクタです。現在の設定を保存します。
		/// </summary>
		~Setting(){this.Save();}
		//===========================================================
		//		操作
		//===========================================================
		static Setting(){
			Setting.root=new SettingKey(Setting.xdoc.DocumentElement);
		}
		/// <summary>
		/// 現在の設定を保存します。
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
		/// ルートの設定キーを取得します。
		/// </summary>
		public static SettingKey Root{get{return Setting.root;}}
	}

	#region cls:SettingKey
	/// <summary>
	/// 設定値を保持する Key を表すクラスです。XML 要素へのアクセスを提供する事で実現しています。
	/// </summary>
	public class SettingKey{
		private System.Xml.XmlElement elem;
		private const string DEFAULT_CLASS="key";
		//===========================================================
		//		既定の値
		//===========================================================
		/// <summary>
		/// この key の分類を取得します。XML 要素の要素名に対応します。
		/// </summary>
		public string Class{
			get{return this.elem.Name;}
		}
		/// <summary>
		/// この key を特定する名前を取得します。XML 要素の key:name 属性に対応します。
		/// </summary>
		public string Name{
			get{return this.elem.GetAttribute("name",Setting.XMLNS_KEY);}
		}
		/// <summary>
		/// この key の既定の値を取得します
		/// </summary>
		public string Value{
			get{return this.elem.GetAttribute("value",Setting.XMLNS_KEY);}
			set{this.elem.SetAttribute("value",Setting.XMLNS_KEY,value);}
		}
		//===========================================================
		//		値
		//===========================================================
		private VarCollection vars;
		/// <summary>
		/// この Key の保持する値へのアクセスを提供します。
		/// </summary>
		public VarCollection Var{get{return this.vars;}}
		/// <summary>
		/// SettingKey が保持する値へのアクセスを提供するクラスです。
		/// </summary>
		public class VarCollection:System.Collections.IEnumerable{
			private SettingKey p;
			/// <summary>
			/// VarCollection のコンストラクタ。
			/// </summary>
			/// <param name="p">対応する SettingKey を指定します。</param>
			internal VarCollection(SettingKey p){this.p=p;}
			/// <summary>
			/// 指定した名前の値のデータを取得亦は設定します。
			/// </summary>
			public string this[string var]{
				get{return this.p.elem.GetAttribute(escAttrName(var));}
				set{this.p.elem.SetAttribute(escAttrName(var),value);}
			}
			/// <summary>
			/// 値を System.Xml.XmlAttribute として列挙する列挙子を取得します。
			/// </summary>
			/// <returns>値の列挙子を返します。</returns>
			public System.Collections.IEnumerator GetEnumerator(){
				return new VarEnumerator(this.p.elem);
			}
			/// <summary>
			/// 指定した値が既に設定されているかどうかを取得します。
			/// </summary>
			/// <param name="var">既に設定されているかどうかを確かめる値の名前を指定します。</param>
			/// <returns>指定した値が既に設定されている場合に true を返します。
			/// 指定した値が設定されていない場合には false を返します。</returns>
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
			/// 指定した名前のデータを、指定した型で取得します。
			/// </summary>
			/// <typeparam name="T">取得するデータの型を指定します。</typeparam>
			/// <param name="var">データを格納する変数の名前を指定します。</param>
			/// <returns>文字列情報を指定した型に変換して返します。</returns>
			public T GetValue<T>(string var){
				string value=this[var];
				try{
					return afh.StringConvert.To<T>(value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("値の変換中にエラーが発生しました。");
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
			/// 指定した名前のデータを、指定した型で取得します。
			/// </summary>
			/// <param name="t">取得するデータの型を指定します。</param>
			/// <param name="var">データを格納する変数の名前を指定します。</param>
			/// <returns>文字列情報を指定した型に変換して返します。</returns>
			/// <exception cref="System.ArgumentException">
			/// 文字列から指定した型への変換に失敗した場合に発生します。
			/// afh.Convert.Convert で対応している型への変換を使用して下さい。
			/// </exception>
			public object GetValue(System.Type t,string var){
				string value=this[var];
				try{
					return afh.StringConvert.To(t,value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("値の変換中にエラーが発生しました。");
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
			/// 指定した変数に指定した型のオブジェクトを設定します。
			/// </summary>
			/// <typeparam name="T">取得するデータの型を指定します。</typeparam>
			/// <param name="var">データを格納する変数の名前を指定します。</param>
			/// <param name="value">指定したオブジェクトを文字列に変換して返します。</param>
			/// <exception cref="System.ArgumentException">
			/// 指定したデータの型から文字列型への変換が出来なかった場合に発生します。
			/// afh.Convert.Convert での変換に対応している型を使用して下さい。
			/// </exception>
			public void SetValue<T>(string var,T value){
				string v;
				try{
					v=afh.StringConvert.FromAs<T>(value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("値の変換中にエラーが発生しました。");
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
			/// 指定した変数に指定した型のオブジェクトを設定します。
			/// </summary>
			/// <param name="var">データを格納する変数の名前を指定します。</param>
			/// <param name="t">取得するデータの型を指定します。</param>
			/// <param name="value">指定したオブジェクトを文字列に変換して返します。</param>
			/// <exception cref="System.ArgumentException">
			/// 指定したデータの型から文字列型への変換が出来なかった場合に発生します。
			/// afh.Convert.Convert での変換に対応している型を使用して下さい。
			/// </exception>
			public void SetValue(string var,System.Type t,object value){
				string v;
				try{
					v=afh.StringConvert.From(t,value);
				}catch(System.Exception e){
					Application.Log l=Application.Log.AfhOut;
					l.WriteError("値の変換中にエラーが発生しました。");
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
			/// 指定した変数にオブジェクトを設定します。
			/// </summary>
			/// <param name="var">データを格納する変数の名前を指定します。</param>
			/// <param name="value">指定したオブジェクトを文字列に変換して返します。</param>
			/// <exception cref="System.ArgumentException">
			/// 指定したオブジェクトを文字列型への変換が出来なかった場合に発生します。
			/// afh.Convert.Convert での変換に対応している型を使用して下さい。
			/// </exception>
			public void SetValue(string var,object value){
				this.SetValue(var,value.GetType(),value);
			}
		}
		/// <summary>
		/// 値の列挙子。値は System.Xml.XmlAttribute として返します。
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
		//		子 Key
		//===========================================================
		/// <summary>
		/// 分類と名前を指定して SettingKey を取得します。
		/// 対応する存在しない場合には新しく要素を作成します。
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
		/// 名前を指定して既定の分類の中から SettingKey を取得します。
		/// 対応する存在しない場合には新しく要素を作成します。
		/// </summary>
		public SettingKey this[string name]{get{return this[DEFAULT_CLASS,name];}}
		/// <summary>
		/// パスを指定して子孫の SettingKey を取得します。
		/// 存在しない場合は新しく作成します。
		/// </summary>
		/// <param name="path">取得する SettingKey のパスを指定します。
		/// クラス名を指定する場合には名前の手前に "&lt;" と "&gt;" で括って記述します。
		/// </param>
		/// <returns>取得した SettingKey を返します</returns>
		/// <example>
		/// <code>
		/// SettingKey main=Setting.Root.GetKey("user\&lt;example&gt;setting1\keyMain");
		/// </code>
		/// </example>
		public SettingKey GetKey(string path){
			return this.GetKey(path.Split(new char[]{'\\'}),0);
		}
		/// <summary>
		/// 指定したパスの SettingKey を取得します
		/// </summary>
		/// <param name="names">パスの [それぞれの階層の名前を表す文字列] の配列を指定します。
		/// クラスを指定する際には "&lt;" と "&gt;" で囲んで名前の前に指定します</param>
		/// <param name="index">次に検索する名前</param>
		/// <returns>パスによって指定される SettingKey を返します。
		/// 既存でない場合は新しく作成して返します。</returns>
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
		//		子 SettingKeyCollection
		//===========================================================
		/// <summary>
		/// 指定した分類に対応する SettingKey 集合へのアクセスを提供します。
		/// </summary>
		/// <param name="cls">SettingKey の分類を指定してください。</param>
		/// <returns>SettingKey 集合へのアクセスを提供する SettingKeyCollection を返します。</returns>
		public SettingKeyCollection GetKeys(string cls){
			return new SettingKeyCollection(this.elem,cls);
		}
		/// <summary>
		/// 既定の分類の SettingKey の集合へのアクセスを提供します。
		/// </summary>
		/// <returns>SettingKey へのアクセスを提供する SettingKeyCollection を返します。</returns>
		public SettingKeyCollection GetKeys(){return this.GetKeys(DEFAULT_CLASS);}
		/// <summary>
		/// 或る分類の SettingKey へのアクセスを提供するクラスです。
		/// </summary>
		public class SettingKeyCollection:System.Collections.IEnumerable{
			private System.Xml.XmlElement p;
			private string cls;
			private System.Xml.XmlNodeList list;
			/// <summary>
			/// SettingKeyCollection コンストラクタ。
			/// </summary>
			/// <param name="parent">
			/// 親の SettingKey に対応する System.Xml.XmlElement を指定します。
			/// parent.OwnerDocument には xmlns:key が設定されている必要があります。
			/// </param>
			/// <param name="cls">SettingKey 集合を特定する分類を指定します。</param>
			public SettingKeyCollection(System.Xml.XmlElement parent,string cls){
				this.p=parent;
				this.cls=cls;
				this.list=this.p.GetElementsByTagName(cls);
			}
			/// <summary>
			/// 集合に含まれる SettingKey の数を取得します。
			/// </summary>
			public int Count{get{return this.list.Count;}}
			/// <summary>
			/// 番号を指定して SettingKey を取得します。
			/// </summary>
			public SettingKey this[int index]{
				get{return new SettingKey((System.Xml.XmlElement)this.list[index]);}
			}
			/// <summary>
			/// 名前を指定して SettingKey を取得します。
			/// 対応する XML 要素がなかった場合は新しく要素を作成します。 
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
			/// 名前を指定して SettingKey を取得します。
			/// </summary>
			/// <param name="name">取得する SettingKey の名前を指定します。</param>
			/// <returns>見つかった XML 要素に対応する SettingKey のインスタンスを返します。
			/// 対応する XML 要素が見つからなかった場合には null を返します。</returns>
			public SettingKey GetIfExists(string name){
				foreach(System.Xml.XmlElement e in this.list){
					if(e.GetAttribute("name",Setting.XMLNS_KEY)==name)return new SettingKey(e);
				}
				return null;
			}
			/// <summary>
			/// 現在の分類に含まれる SettingKey を列挙する列挙子を取得します。
			/// </summary>
			/// <returns>SettingKey の列挙子を返します。</returns>
			public System.Collections.IEnumerator GetEnumerator(){
				return new SettingKeyEnumerator(this.list.GetEnumerator());
			}
		}
		/// <summary>
		/// SettingKey の列挙子
		/// </summary>
		private class SettingKeyEnumerator:System.Collections.IEnumerator{
			System.Collections.IEnumerator enu;
			/// <summary>
			/// SettingKey の列挙子を初期化します。
			/// </summary>
			/// <param name="enu">System.Xml.XmlElement に変換できるオブジェクトを列挙する列挙子を指定してください。</param>
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
		/// 新しく要素を作成して、SettingKey を初期化します。
		/// </summary>
		/// <param name="parent">
		/// この要素を保持する親要素を指定します。
		/// parent の OwnerDocument にはxmlns:key が設定されていなければなりません。
		/// </param>
		/// <param name="cls">分類を指定する文字列を指定します。
		/// 空白を指定した場合 "key" (既定の分類を表す文字列)と解釈されます。</param>
		/// <param name="name">この SettingKey を特定する文字列を指定します。
		/// 空白を指定した場合には名前は設定されません。</param>
		public SettingKey(System.Xml.XmlElement parent,string cls,string name):this(parent.OwnerDocument.CreateElement(cls==""?DEFAULT_CLASS:cls)){
			try{
				if(name!="")this.elem.SetAttribute("name",Setting.XMLNS_KEY,name);
				parent.AppendChild(this.elem);
			}catch(System.Exception e){throw e;}
		}
		/// <summary>
		/// 既存の XML 要素から SettingKey インスタンスを作成します。
		/// </summary>
		/// <param name="elem">
		/// この SettingKey の元となる System.Xml.XmlElement を指定します。
		/// parent の OwnerDocument にはxmlns:key が設定されていなければなりません。
		/// </param>
		public SettingKey(System.Xml.XmlElement elem){
			this.elem=elem;
			this.vars=new VarCollection(this);
		}
	}
	#endregion

	#region Attribute:RestoreProperties
	/// <summary>
	/// 状態(プロパティ)の保存・復元に関する機能を提供するクラスです。
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Field)]
	public class RestorePropertiesAttribute:System.Attribute{
		private string[] props;
		/// <summary>
		/// RestorePropertiesAttribute のインスタンスを作成します。
		/// クラスに設定した時には、そのクラスインスタンスのプロパティの値を保存・復元します。
		/// フィールドに設定した時にはそのフィールドのプロパティの値を保存・復元します。
		/// </summary>
		/// <param name="propertyNames">状態を保存・復元するプロパティの名前を指定します。
		/// <para>
		/// &gt;で区切って二つのプロパティ名を記述した場合には、
		/// &gt; の左側から読み取ってファイルに保存し、
		/// 復元時には &gt; の右側にファイルから読み取った値を設定します。
		/// </para>
		/// </param>
		/// <remarks>プロパティの型が対応していない型の場合は、
		/// 指定しても保存・復元する事はできません。</remarks>
		public RestorePropertiesAttribute(params string[] propertyNames){
			this.props=propertyNames;
			//System.Console.WriteLine("initialized RestorePropertiesAttribute");
		}
		private bool traceField=false;
		/// <summary>
		/// RestorePropertiesAttribute を設定したフィールドの型自体にも、
		/// RestorePropertiesAttribute が設定されている場合に、
		/// そのフィールド変数に対しても Restore を自動で実行させる事が出来ます。
		/// この TraceFieldType プロパティを使って、自動で実行するか否かを取得亦は設定します。
		/// 既定の値は false です。
		/// </summary>
		/// <remarks>
		/// 円環参照になっている物に対してこれを設定すると、
		/// 保存・復元の際に無限呼び出しになり StackOverflow の危険性が生じます。
		/// </remarks>
		public bool TraceFieldType{
			get{return this.traceField;}
			set{this.traceField=value;}
		}
		//===========================================================
		//		静的メンバ
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
		private const string PROPERTY_NOSUCH="{0}:\t\n指定したプロパティ {2} は {1} には存在しません";
		private const string PROPERTY_CANNOTREAD="{0}:\t\n指定したプロパティ {1}.{2} には getter が実装されていません";
		private const string PROPERTY_CANNOTWRITE="{0}:\t\n指定したプロパティ {1}.{2} には setter が実装されていません";
		private const string ERROR_CONVERT=@"{0}:
	変換中にエラーが発生しました
	Message :	{1}
	Property:	{2}
	Type    :	{3}
	Value   :	{4}";
		private const string ERROR_SETTING=@"{0}:
	値の設定中にエラーが発生しました
	Message :	{1}
	Property:	{2}
	Value   :	{3}";
		//*/
		private const string PROPERTY_NOSUCH="指定したプロパティ {2} は {1} には存在しません";
		private const string PROPERTY_CANNOTREAD="指定したプロパティ {1}.{2} には getter が実装されていません";
		private const string PROPERTY_CANNOTWRITE="指定したプロパティ {1}.{2} には setter が実装されていません";
		private const string ERROR_CONVERT="値の変換中にエラーが発生しました";
		private const string ERROR_SETTING="値の設定中にエラーが発生しました";
		//===========================================================
		//		復元
		//===========================================================
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を復元します。
		/// </summary>
		/// <param name="obj">状態を復元するオブジェクトを指定します。</param>
		/// <param name="id">同じ型の複数のオブジェクトを区別する為の識別子を指定します。</param>
		public static void Restore(object obj,string id){
			string name=obj.GetType().ToString()+":"+id;
			Restore(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を復元します。
		/// </summary>
		/// <param name="obj">状態を復元するオブジェクトを指定します。</param>
		public static void Restore(object obj){
			string name=obj.GetType().ToString();
			Restore(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を復元します。
		/// </summary>
		/// <param name="obj">復元するオブジェクトを指定します。</param>
		/// <param name="k">復元に使用する昔の情報を持つ SettingKey を指定します。</param>
		private static void Restore(object obj,SettingKey k){
			System.Type t=obj.GetType();
			RestorePropertiesAttribute[] attrs
				=(RestorePropertiesAttribute[])t.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
			if(attrs.Length==0)return;
			//--obj 自体の復元
			Restore(obj,k,attrs);
			//--Field の復元
			object field;SettingKey kf;
			foreach(System.Reflection.FieldInfo f in t.GetFields(BF_FIELD)){
				attrs=(RestorePropertiesAttribute[])f.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
				if(attrs.Length==0)continue;
				//--準備
				field=f.GetValue(obj);kf=k["F",f.Name];
				//--復元
				Restore(field,kf,attrs);
				if(attrs[0].traceField)Restore(field,kf);
			}
		}
		/// <summary>
		/// 指定した RestorePropertiesAttribute に従って
		/// 指定したオブジェクトのプロパティの値を復元します。
		/// </summary>
		/// <param name="obj">復元するオブジェクトを指定します。</param>
		/// <param name="k">復元に使用する昔の情報を持つ SettingKey を指定します。</param>
		/// <param name="attrs">どのプロパティを復元するかの情報を指定します。</param>
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
		//		保存
		//===========================================================
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を保存します。
		/// </summary>
		/// <param name="obj">状態を保存するオブジェクトを指定します。</param>
		/// <param name="id">同じ型の複数のオブジェクトを区別する為の識別子を指定します。</param>
		public static void Save(object obj,string id){
			string name=obj.GetType().ToString()+":"+id;
			Save(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を保存します。
		/// </summary>
		/// <param name="obj">状態を保存するオブジェクトを指定します。</param>
		public static void Save(object obj){
			string name=obj.GetType().ToString();
			Save(obj,Setting.Root[SETTINGKEY_NAME]["T",name]);
		}
		/// <summary>
		/// 指定したオブジェクトの型に設定された RestorePropertiesAttribute に従って
		/// プロパティの値を保存します。
		/// </summary>
		/// <param name="obj">保存するオブジェクトを指定します。</param>
		/// <param name="k">現在の状態の保存先である SettingKey を指定します。</param>
		private static void Save(object obj,SettingKey k){
			System.Type t=obj.GetType();
			RestorePropertiesAttribute[] attrs
				=(RestorePropertiesAttribute[])t.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
			if(attrs.Length==0)return;
			//--obj 自体の保存
			Save(obj,k,attrs);
			//--Field の保存
			object field;SettingKey kf;
			foreach(System.Reflection.FieldInfo f in t.GetFields(BF_FIELD)){
				attrs=(RestorePropertiesAttribute[])f.GetCustomAttributes(typeof(RestorePropertiesAttribute),true);
				if(attrs.Length==0)continue;
				//--準備
				field=f.GetValue(obj);kf=k["F",f.Name];
				//--復元
				Save(field,kf,attrs);
				if(attrs[0].traceField)Save(field,kf);
			}
		}
		/// <summary>
		/// 指定した RestorePropertiesAttribute に従って
		/// 指定したオブジェクトのプロパティの値を保存します。
		/// </summary>
		/// <param name="obj">保存するオブジェクトを指定します。</param>
		/// <param name="k">現在の状態の保存先である SettingKey を指定します。</param>
		/// <param name="attrs">どのプロパティを保存するかの情報を指定します。</param>
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
		//		プロパティの設定・取得
		//===========================================================
		/// <summary>
		/// 指定したオブジェクトの指定したプロパティに文字列で指定した値を設定します。
		/// </summary>
		/// <param name="obj">設定先のオブジェクトを指定します。</param>
		/// <param name="prop">設定先のプロパティの識別子を指定します。</param>
		/// <param name="value">設定する値を指定します。</param>
		private static void SetValue(object obj,string prop,string value){
			System.Type t=obj.GetType();
			System.Reflection.PropertyInfo p=t.GetProperty(prop);
			if(p==null){
				Application.Log.AfhOut.WriteError("指定したプロパティ "+prop+" は "+t.FullName+" には存在しません");
				return;
			}else if(!p.CanWrite){
				Application.Log.AfhOut.WriteError("指定したプロパティ "+t.FullName+"."+prop+" には setter が実装されていません");
				return;
			}
			object val;
			try{
				val=afh.StringConvert.To(p.PropertyType,value);
			}catch(System.Exception e){
				Application.Log l=Application.Log.AfhOut;
				l.WriteError("値の変換中にエラーが発生しました。");
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
				l.WriteError("値の設定中にエラーが発生しました。");
				l.AddIndent();
				l.WriteVar("Message",e.InnerException==null?e.Message:e.InnerException.Message);
				l.WriteVar("Property",p.ReflectedType.ToString()+"."+p.Name);
				l.WriteVar("Value",val.ToString());
				l.RemoveIndent();
				return;
			}
			
		}
		/// <summary>
		/// 指定したオブジェクトの指定したプロパティを文字列として取得します。
		/// </summary>
		/// <param name="obj">取得元のオブジェクトを指定します。</param>
		/// <param name="prop">取得元のプロパティの識別子を指定します。</param>
		/// <returns>取得したプロパティを文字列として返します。</returns>
		private static string GetValue(object obj,string prop){
			System.Type t=obj.GetType();
			System.Reflection.PropertyInfo p=t.GetProperty(prop);
			if(p==null){
				Application.Log.AfhOut.WriteError("指定したプロパティ "+prop+" は "+t.FullName+" には存在しません");
				return null;
			}else if(!p.CanRead){
				Application.Log.AfhOut.WriteError("指定したプロパティ "+t.FullName+"."+prop+" には getter が実装されていません");
				return null;
			}
			object value=p.GetValue(obj,null);
			if(value==null)return null;
			try{
				return afh.StringConvert.From(p.PropertyType,value);
			}catch(System.Exception e){
				Application.Log l=Application.Log.AfhOut;
				l.WriteError("値の変換中にエラーが発生しました。");
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
		//		System.Windows.Forms.Form の特別設定
		//===========================================================
		/// <summary>
		/// System.Windows.Forms.Form の復元・保存を自動的に設定します。
		/// </summary>
		/// <param name="form">状態を復元・保存する Form のインスタンスを指定します。</param>
		/// <param name="savesize">
		/// Form の大きさ、位置、WindowState を復元したい場合に true を指定します。
		/// 復元しない場合・自分で復元のコードを各場合には false を指定します。
		/// <para>※ Left, Top, Width, Height, WindowState を別々に復元しても完全には復元されません。
		/// WindowState が Maximized の場合で保存を実行すると、
		/// Normal に戻した時にどの位置・大きさになる筈だったかの情報が失われてしまいます。
		/// 茲を true に指定した場合には、完全に復元出来るように特別の処理を行います。
		/// </para>
		/// </param>
		public static void SetupForm(System.Windows.Forms.Form form,bool savesize){
			new FormSetting(form,savesize);
		}
		/// <summary>
		/// System.Windows.Forms.Form の復元・保存を自動的に設定します。
		/// </summary>
		/// <param name="form">状態を復元・保存する Form のインスタンスを指定します。</param>
		/// <param name="id">同じ型の複数の Form インスタンスを区別する為の識別子を指定します。</param>
		/// <param name="savesize">
		/// Form の大きさ、位置、WindowState を復元したい場合に true を指定します。
		/// 復元しない場合・自分で復元のコードを各場合には false を指定します。
		/// <para>※ Left, Top, Width, Height, WindowState を別々に復元しても完全には復元されません。
		/// WindowState が Maximized の場合で保存を実行すると、
		/// Normal に戻した時にどの位置・大きさになる筈だったかの情報が失われてしまいます。
		/// 茲を true に指定した場合には、完全に復元出来るように特別の処理を行います。
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
			//		初期化
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
			//		記録・復元
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
			//		大きさの記録
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