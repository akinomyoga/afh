using Gen=System.Collections.Generic;
using Gdi=System.Drawing;
using Serial=System.Runtime.Serialization;
namespace afh.Rendering{
	/// <summary>
	/// フォントの管理を行います。
	/// </summary>
	[System.Serializable]
	public class FontManager:System.Runtime.Serialization.ISerializable{
		private Gdi::Font f;
		private string fontname;
		private Gdi::FontStyle style;

		public FontManager(string fontname,float size){
			this.f=null;
			this.fontname=fontname;
			this.size=size;
		}
		public override string ToString(){
			return string.Format("{0}{{face: '{1}'; size: {2}; style: {3}}}",base.ToString(),this.fontname,this._size,this.style);
		}
		/// <summary>
		/// 描画に使用するフォントを取得します。
		/// </summary>
		public Gdi::Font Font{
			get{return this.f??(this.f=CreateFont(this.fontname,this._size,this.style));}
		}
		//=================================================
		//		フォント名
		//=================================================
		/// <summary>
		/// フォント名を指定します。
		/// </summary>
		public string FontName{
			get{return this.fontname;}
			set{
				if(this.fontname==null)return;
				this.fontname=value;
				this.f=null;
			}
		}
		//=================================================
		//		大きさ
		//=================================================
		private float _size;
		private float size{
			get{return this._size;}
			set{
				if(this._size==value)return;
				this._size=value;
				this.f=null;
			}
		}

		//=================================================
		//		フォントスタイル
		//=================================================
		/// <summary>
		/// 文字を太字で描画するか否かを取得亦は設定します。
		/// </summary>
		public bool Bold{
			get{return (this.style&Gdi::FontStyle.Bold)!=0;}
			set{
				if(this.Bold==value)return;
				if(value)this.style|=Gdi::FontStyle.Bold;else this.style&=~Gdi::FontStyle.Bold;
				this.f=null;
			}
		}
		/// <summary>
		/// 文字を斜体で用がするか否かを取得亦は設定します。
		/// </summary>
		public bool Italic{
			get{return (this.style&Gdi::FontStyle.Italic)!=0;}
			set{
				if(this.Italic==value) return;
				if(value) this.style|=Gdi::FontStyle.Italic; else this.style&=~Gdi::FontStyle.Italic;
				this.f=null;
			}
		}
		/// <summary>
		/// 文字の上に打ち消し線を描画するか否かを取得亦は設定します。
		/// </summary>
		public bool Strike {
			get { return (this.style&Gdi::FontStyle.Strikeout)!=0; }
			set{
				if(this.Strike==value) return;
				if(value) this.style|=Gdi::FontStyle.Strikeout; else this.style&=~Gdi::FontStyle.Strikeout;
				this.f=null;
			}
		}
		/// <summary>
		/// 文字の下に下線を描画するか否かを取得亦は設定します。
		/// </summary>
		public bool Underline {
			get { return (this.style&Gdi::FontStyle.Underline)!=0; }
			set{
				if(this.Underline==value) return;
				if(value) this.style|=Gdi::FontStyle.Underline; else this.style&=~Gdi::FontStyle.Underline;
				this.f=null;
			}
		}
		/// <summary>
		/// 文字のスタイルを指定します。
		/// </summary>
		public Gdi::FontStyle Style{
			get{return this.style;}
			set{
				if(this.style==value)return;
				this.style=value;
				this.f=null;
			}
		}
		//===========================================================
		//		ISerializable
		//===========================================================
		private FontManager(Serial::SerializationInfo info,Serial::StreamingContext context){
			this.f=null;
			this.fontname=(string)info.GetValue("fontname",typeof(string));
			this._size=info.GetSingle("_size");
			this.style=(Gdi::FontStyle)info.GetValue("style",typeof(Gdi::FontStyle));
		}
		void Serial::ISerializable.GetObjectData(Serial::SerializationInfo info,Serial::StreamingContext context) {
			info.AddValue("fontname",this.fontname);
			info.AddValue("_size",this._size);
			info.AddValue("style",this.style);
		}
		//===========================================================
		//		作成済みフォントの管理
		//===========================================================
		// TODO: 暫く使わなかったフォントを自動的に Dispose する機構
		//-----------------------------------------------------------
		private static Gen::Dictionary<FontKey,Gdi::Font> fonts=new Gen::Dictionary<FontKey,Gdi::Font>();
		private static Gdi::Font CreateFont(string name,float size,Gdi::FontStyle style){
			FontKey k=new FontKey(name,size,style);
			if(!fonts.ContainsKey(k)){
				Gdi::Font f=new Gdi::Font(name,size,style,Gdi::GraphicsUnit.Pixel);
				fonts[k]=f;
				return f;
			}
			return fonts[k];
		}
		[System.Serializable]
		private struct FontKey{
			public string facename;
			public float size;
			public Gdi::FontStyle style;

			public FontKey(string facename,float size,Gdi::FontStyle style){
				this.facename=facename;
				this.size=size;
				this.style=style;
			}

			public static bool operator ==(FontKey f1,FontKey f2){
				return f1.facename==f2.facename
					&&f1.size==f2.size
					&&f1.style==f2.style;
			}
			public static bool operator !=(FontKey f1,FontKey f2){
				return !(f1==f2);
			}
			public override bool Equals(object obj) {
				return obj is FontKey&&this==(FontKey)obj;
			}
			public override int GetHashCode(){
				return this.facename.GetHashCode()
					^this.size.GetHashCode()
					^this.style.GetHashCode();
			}
		}

	}

}