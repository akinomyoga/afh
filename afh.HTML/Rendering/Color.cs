using Gen=System.Collections.Generic;
using Color=afh.Drawing.Color32Argb;
using __dll__=afh.HTML.__dll__;
using Serial=System.Runtime.Serialization;

namespace afh.Rendering{
	/// <summary>
	/// �X�^�C����ł̐F�̕\���ł��B
	/// </summary>
	[System.Serializable]
	public partial struct ColorName:Serial::ISerializable{
		/// <summary>
		/// �F�̕\�����@��ێ����܂��B
		/// true �Ɏw�肳��Ă��鎞�ɂ́Acolor ���F���̔ԍ��ł��鎖�������܂��B
		/// false �Ɏw�莌����Ă��鎞�ɂ́Acolor �����̂��ꂼ��̃o�C�g�� ARGB �̋��x�̏��������Ă��鎖�������܂��B
		/// </summary>
		private bool hasName;
		/// <summary>
		/// �F�̏���ێ����܂��B
		/// </summary>
		private int color;

		/// <summary>
		/// �F�̕\�����@���擾���܂��B
		/// true �̏ꍇ�ɂ́A�F���F�̖��O�Ƃ��ĕ\������Ă��鎖�������܂��B
		/// false �̏ꍇ�ɂ́AARGB �̋��x�ŐF���\������Ă��鎖�������܂��B
		/// </summary>
		public bool IsNamed{
			get{return this.hasName;}
		}

		/// <summary>
		/// ���̃C���X�^���X���\������F�̒l���擾���܂��B
		/// </summary>
		/// <returns>���̃C���X�^���X���\������F�̒l�� afh.Drawing.Color32Argb �ŕԂ��܂��B</returns>
		public Color ToColor(){
			return this.hasName?c_values[color]:(Color)color;
		}
		/// <summary>
		/// �F�̏��𕶎���ŕ\�����܂��B
		/// </summary>
		/// <returns>������ŕ\�������F�̏���Ԃ��܂��B</returns>
		public override string ToString(){
			const int A_MASK=unchecked((int)0xff000000);
			if(this.hasName)return  c_names[color];

			string ret;
			if((color&A_MASK)==A_MASK){
				ret=(color&~A_MASK).ToString("x6");
				if(ret[0]==ret[1]&&ret[2]==ret[3]&&ret[4]==ret[5])unsafe{
					char* pch=stackalloc char[3];
					pch[0]=ret[0];
					pch[1]=ret[2];
					pch[2]=ret[4];
					ret=new string(pch,0,3);
				}
			}else{
				ret=color.ToString("x8");
			}
			return '#'+ret;
		}
		/// <summary>
		/// �w�肵���I�u�W�F�N�g���� ColorName �̃C���X�^���X�𐶐����܂��B
		/// </summary>
		/// <param name="value">ColorName �̌��ɂȂ�����܂�ł���I�u�W�F�N�g���w�肵�܂��B</param>
		/// <returns>���������I�u�W�F�N�g��Ԃ��܂��B</returns>
		public static ColorName FromObject(object value){
			int i;Color c32;ColorName ret;

			System.Type t=value.GetType();
			if(t.IsPrimitive){
				if(t==typeof(int)){
					i=(int)value;
					goto int32;
				}else if(t==typeof(uint)){
					i=(int)(uint)value;
					goto int32;
				}
			}else if(t.IsValueType){
				if(t==typeof(ColorName)){
					return (ColorName)value;
				}else if(t==typeof(System.Drawing.Color)){
					System.Drawing.Color c=(System.Drawing.Color)value;
					ret=new ColorName();
					if(ret.hasName=c.IsNamedColor){
						string name=c.Name.ToString();
						if(!c_indices.TryGetValue(name,out i)){
							ret.color=RegisterColor(name,c);
							throw new System.Exception(
								string.Format("�w�肵�����O {0} �̐F�� ColorName �ł͈����܂���",name));
						}else{
							ret.color=i;
						}
					}else{
						ret.color=c.ToArgb();
					}
					return ret;
				}else if(t==typeof(afh.Drawing.Color32Argb)){
					c32=(Color)value;
					goto color32;
				}else if(t==typeof(afh.Drawing.ColorRef)){
					c32=(afh.Drawing.ColorRef)value;
					goto color32;
				}else if(t==typeof(afh.Drawing.ColorHsv)){
					c32=(Color)(afh.Drawing.ColorHsv)value;
					goto color32;
				}
			}else if(t==typeof(string)){
				i=0;
				return new ColorName((string)value,ref i);
			}
			throw new System.Exception("�w�肵���I�u�W�F�N�g��F���ɕϊ����鎖���o���܂���B");

		color32: // argument: c32
			i=(int)c32;
		int32: // argument: i
			ret=new ColorName();
			ret.hasName=false;
			ret.color=i;
			return ret;
		}
		//===========================================================
		//		Serialization
		//===========================================================
		private ColorName(Serial::SerializationInfo info,Serial::StreamingContext context){
			if(this.hasName=info.GetBoolean("hasName")){
				string name=info.GetString("name");
				int index;
				if(c_indices.TryGetValue(name,out index)){
					this.color=index;
				}else{
					this.color=RegisterColor(name,info.GetInt32("color"));
				}
			}else{
				this.color=info.GetInt32("color");
			}
		}
		void Serial::ISerializable.GetObjectData(Serial::SerializationInfo info,Serial::StreamingContext context){
			info.AddValue("hasName",this.hasName);
			if(this.hasName){
				info.AddValue("color",(int)c_values[this.color]);
				info.AddValue("name",c_names[this.color]);
			}else{
				info.AddValue("color",this.color);
			}
		}
		//===========================================================
		//		�F�̓ǂݎ��
		//===========================================================
		/// <summary>
		/// �������ǂݎ���� ColorName �̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="text">�F�̏����i�[���Ă��镶������w�肵�܂��B</param>
		/// <param name="index">�F�̏��̊J�n�ʒu���w�肵�܂��B</param>
		public ColorName(string text,ref int index){
			int i=index;
			
			char c;afh.Parse.LetterType lt;

			// �󔒓ǂݔ�΂�
			do{
				if(i>=text.Length)
					throw new System.FormatException("�L���ȕ������܂܂�Ă��܂���B");
				lt=afh.Parse.LetterType.GetLetterType(c=text[i++]);
			}while(lt.IsInvalid||lt.IsSpace);
			// [����: ���߂̔�󔒕���]
			// [�ʒu: "����" �̎�]

			if(c=='#'){
				if(!read_sharp(text,ref i,out this.color)){
					throw new System.FormatException("�F��\�����镶����̌`��������Ă��܂��B");
				}
				this.hasName=false;
				index=i;
			}else{
				string name;
				// ���O�̎擾
				while(true){
					// next
					if(i<text.Length){
						lt=afh.Parse.LetterType.GetLetterType(c=text[i]);
					}else{
						c='\0';
						name=text.Substring(index,i-index).ToLower();
						break;
					}

					// ��������
					if(c=='('||lt.IsSpace||lt.IsInvalid){
						name=text.Substring(index,i-index).ToLower();

						// �󔒔�΂�
						while((lt.IsInvalid||lt.IsSpace)&&++i<text.Length){
							lt=afh.Parse.LetterType.GetLetterType(c=text[i]);
						}

						break;
					}

					i++;
				}
				// [����: "���O" ��̏��߂� '��X�y�[�X����'|'null �I�[']
				// [�ʒu: "����"]

				if(c=='('){
					this.hasName=false;
					double[] args=read_func_args(text,ref i);
					switch(name){
						case "rgb":
							this.color=(int)Color.FromRgb(
								read_double2byte(args[0]),
								read_double2byte(args[1]),
								read_double2byte(args[2])
								);
							break;
						case "argb":
							this.color=(int)Color.FromArgb(
								read_double2byte(args[0]),
								read_double2byte(args[1]),
								read_double2byte(args[2]),
								read_double2byte(args[3])
								);
							break;
						case "cmy":
							this.color=(int)Color.FromCmy(
								read_double2byte(args[0]),
								read_double2byte(args[1]),
								read_double2byte(args[2])
								);
							break;
						case "hsv":
							this.color=(int)Color.FromHsv(
								args[0],
								read_double2byte(args[1]),
								read_double2byte(args[2])
								);
							break;
						default:
							throw new System.FormatException("'"+name+"' �Ƃ����F�֐��͓o�^����Ă��܂���");
					}
				}else{
					this.hasName=true;
					if(!c_indices.TryGetValue(name,out this.color))
						throw new System.FormatException("'"+name+"' �Ƃ����F�͓o�^����Ă��܂���");
				}
				index=i;
			}
		}
		/// <summary>
		/// # �Ŏn�܂�F�̕�����\����ǂݎ��܂��B
		/// </summary>
		/// <param name="text">�ǂݎ��l��ێ����Ă��镶������w�肵�܂��B</param>
		/// <param name="i"># �̎��̕����̈ʒu���w�肵�܂��B
		/// �ǂݎ�����Ō�̕����̎��̕����̈ʒu��Ԃ��܂��B
		/// �ǂݎ��Ɏ��s�����ꍇ�ɂ́A���s�ʒu��Ԃ��܂��B</param>
		/// <param name="color">�ǂݎ�����F��Ԃ��܂��B</param>
		/// <returns>�F�̓ǂݎ��ɐ��������ꍇ�� true ��Ԃ��܂��B����ȊO�̏ꍇ�ɂ� false ��Ԃ��܂��B</returns>
		private unsafe static bool read_sharp(string text,ref int i,out int color){
			// xdigit �ǂݎ��
			byte* hexB=stackalloc byte[8];
			byte* hexU=hexB+8;
			byte* hexP=hexU;
			// �� hexB                 �� hexU
			// XX XX XX XX XX XX XX XX ?? [STACK]
			//  �� �� �� �� �� �� �� �� hexP
			// (��̕����珇�Ԃɖ��߂čs��)
			while(i<text.Length&&hexB<hexP){
				char c=text[i++];

				// �S�p�����p
				if(c>='\xff00')c=(char)(c-'\xff00'+'\x20');
				
				// xdigit �ǂݎ��
				if('0'<=c&&c<='9'){
					*--hexP=(byte)(c-'0');
				}else if('a'<=c&&c<='f'){
					*--hexP=(byte)(c-'a'+10);
				}else if('A'<=c&&c<='F'){
					*--hexP=(byte)(c-'A'+10);
				}else{
					i--; // �����߂�
					break;
				}
			}

			int va4;long va8;
			switch(hexU-hexP){							//                [��] �� hexP �̈ʒu
				case 3:	// #rgb							// 00 00 00 00 00 [0b] 0g 0r ?? ?? [STACK]
					va4=0x0f000000|*(int*)hexP;			// 0xff0r0g0b [va4]
					goto va4read;
				case 4:	// #argb						// 00 00 00 00 [0b] 0g 0r 0a ?? ?? [STACK]
					va4=*(int*)hexP;					// 0x0a0r0g0b [va4]
					goto va4read;
				va4read:								// 0x0a0r0g0b [va4]
					color=va4|va4<<4;					// 0xaarrggbb
					return true;
				case 6: // #RrGgBb						// 00 00 [0b] 0B 0g 0G 0r 0R ?? ?? [STACK]
					va8=0x0fff000000000000|*(long*)hexP;// 0x?fff0R0r0G0g0B0b [va8]
					goto va8read;
				case 8: // #AaRrGgBb					// [0b] 0B 0g 0G 0r 0R 0a 0A ?? ?? [STACK]
					va8=*(long*)hexP;					// 0x0A0a0R0r0G0g0B0b [va8]
					goto va8read;
				va8read:								// 0x0A0a0R0r0G0g0B0b [va8]
					va8=0x00ff00ff00ff00ff&(va8>>4|va8);// 0x__Aa__Rr__Gg__Bb [va8] // '_' was 0 cleared
					va8=0x0000ffff0000ffff&(va8>>8|va8);// 0x____AaRr____GgBb [va8]
					color=(int)(va8>>16|va8);			// 0x________AaRrGgBb
					return true;
				default:
					color=0;
					return false;
			}
		}
		private static byte read_double2byte(double value){
			return value<0?(byte)0:value>255?(byte)255:(byte)value;
		}
		//===========================================================
		//		�o�^����Ă���F
		//===========================================================
		#region Named Colors
		private const int C_CAPACITY=180;
		private static Gen::List<string> c_names=new Gen::List<string>(C_CAPACITY);
		private static Gen::List<Color> c_values=new Gen::List<Color>(C_CAPACITY);
		private static Gen::Dictionary<string,int> c_indices=new Gen::Dictionary<string,int>(C_CAPACITY);
		public static int RegisterColor(string name,Color color){
			name=name.ToLower();
			int index=c_names.Count;
			c_indices.Add(name,index); // �d���������䢂ŗ�O
			c_names.Add(name);
			c_values.Add(color);
			return index;
		}
		static ColorName(){
			// HTML 4.01
			RegisterColor("black"				,0xff000000);
			RegisterColor("silver"				,0xffc0c0c0);
			RegisterColor("gray"				,0xff808080);
			RegisterColor("white"				,0xffffffff);
			RegisterColor("maroon"				,0xff800000);
			RegisterColor("red"					,0xffff0000);
			RegisterColor("purple"				,0xff800080);
			RegisterColor("fuchsia"				,0xffff00ff);
			RegisterColor("green"				,0xff008000);
			RegisterColor("lime"				,0xff00ff00);
			RegisterColor("olive"				,0xff808000);
			RegisterColor("yellow"				,0xffffff00);
			RegisterColor("navy"				,0xff000080);
			RegisterColor("blue"				,0xff0000ff);
			RegisterColor("teal"				,0xff008080);
			RegisterColor("aqua"				,0xff00ffff);
			// CSS
			RegisterColor("orange"				,0xffffa500);
			// IE 4.0
			RegisterColor("aliceblue"			,0xfff0f8ff);
			RegisterColor("darkcyan"			,0xff008b8b);
			RegisterColor("lightyellow"			,0xffffffe0);
			RegisterColor("coral"				,0xffff7f50);
			RegisterColor("dimgray"				,0xff696969);
			RegisterColor("lavender"			,0xffe6e6fa);
			RegisterColor("lightgoldenrodyellow",0xfffafad2);
			RegisterColor("tomato"				,0xffff6347);
			RegisterColor("lightsteelblue"		,0xffb0c4de);
			RegisterColor("darkslategray"		,0xff2f4f4f);
			RegisterColor("lemonchiffon"		,0xfffffacd);
			RegisterColor("orangered"			,0xffff4500);
			RegisterColor("darkgray"			,0xffa9a9a9);
			RegisterColor("lightslategray"		,0xff778899);
			RegisterColor("darkgreen"			,0xff006400);
			RegisterColor("wheat"				,0xfff5deb3);
			RegisterColor("slategray"			,0xff708090);
			RegisterColor("burlywood"			,0xffdeb887);
			RegisterColor("crimson"				,0xffdc143c);
			RegisterColor("lightgrey"			,0xffd3d3d3);
			RegisterColor("steelblue"			,0xff4682b4);
			RegisterColor("forestgreen"			,0xff228b22);
			RegisterColor("tan"					,0xffd2b48c);
			RegisterColor("mediumvioletred"		,0xffc71585);
			RegisterColor("gainsboro"			,0xffdcdcdc);
			RegisterColor("royalblue"			,0xff4169e1);
			RegisterColor("seagreen"			,0xff2e8b57);
			RegisterColor("khaki"				,0xfff0e68c);
			RegisterColor("deeppink"			,0xffff1493);
			RegisterColor("whitesmoke"			,0xfff5f5f5);
			RegisterColor("midnightblue"		,0xff191970);
			RegisterColor("mediumseagreen"		,0xff3cb371);
			RegisterColor("hotpink"				,0xffff69b4);
			RegisterColor("mediumaquamarine"	,0xff66cdaa);
			RegisterColor("gold"				,0xffffd700);
			RegisterColor("palevioletred"		,0xffdb7093);
			RegisterColor("snow"				,0xfffffafa);
			RegisterColor("darkblue"			,0xff00008b);
			RegisterColor("darkseagreen"		,0xff8fbc8f);
			RegisterColor("pink"				,0xffffc0cb);
			RegisterColor("ghostwhite"			,0xfff8f8ff);
			RegisterColor("mediumblue"			,0xff0000cd);
			RegisterColor("aquamarine"			,0xff7fffd4);
			RegisterColor("sandybrown"			,0xfff4a460);
			RegisterColor("lightpink"			,0xffffb6c1);
			RegisterColor("floralwhite"			,0xfffffaf0);
			RegisterColor("palegreen"			,0xff98fb98);
			RegisterColor("darkorange"			,0xffff8c00);
			RegisterColor("thistle"				,0xffd8bfd8);
			RegisterColor("linen"				,0xfffaf0e6);
			RegisterColor("dodgerblue"			,0xff1e90ff);
			RegisterColor("lightgreen"			,0xff90ee90);
			RegisterColor("goldenrod"			,0xffdaa520);
			RegisterColor("magenta"				,0xffff00ff);
			RegisterColor("antiquewhite"		,0xfffaebd7);
			RegisterColor("cornflowerblue"		,0xff6495ed);
			RegisterColor("springgreen"			,0xff00ff7f);
			RegisterColor("peru"				,0xffcd853f);
			RegisterColor("papayawhip"			,0xffffefd5);
			RegisterColor("deepskyblue"			,0xff00bfff);
			RegisterColor("mediumspringgreen"	,0xff00fa9a);
			RegisterColor("darkgoldenrod"		,0xffb8860b);
			RegisterColor("violet"				,0xffee82ee);
			RegisterColor("blanchedalmond"		,0xffffebcd);
			RegisterColor("lightskyblue"		,0xff87cefa);
			RegisterColor("lawngreen"			,0xff7cfc00);
			RegisterColor("chocolate"			,0xffd2691e);
			RegisterColor("plum"				,0xffdda0dd);
			RegisterColor("bisque"				,0xffffe4c4);
			RegisterColor("skyblue"				,0xff87ceeb);
			RegisterColor("chartreuse"			,0xff7fff00);
			RegisterColor("sienna"				,0xffa0522d);
			RegisterColor("orchid"				,0xffda70d6);
			RegisterColor("moccasin"			,0xffffe4b5);
			RegisterColor("lightblue"			,0xffadd8e6);
			RegisterColor("greenyellow"			,0xffadff2f);
			RegisterColor("saddlebrown"			,0xff8b4513);
			RegisterColor("mediumorchid"		,0xffba55d3);
			RegisterColor("navajowhite"			,0xffffdead);
			RegisterColor("powderblue"			,0xffb0e0e6);
			RegisterColor("darkorchid"			,0xff9932cc);
			RegisterColor("peachpuff"			,0xffffdab9);
			RegisterColor("paleturquoise"		,0xffafeeee);
			RegisterColor("limegreen"			,0xff32cd32);
			RegisterColor("darkred"				,0xff8b0000);
			RegisterColor("darkviolet"			,0xff9400d3);
			RegisterColor("mistyrose"			,0xffffe4e1);
			RegisterColor("lightcyan"			,0xffe0ffff);
			RegisterColor("yellowgreen"			,0xff9acd32);
			RegisterColor("brown"				,0xffa52a2a);
			RegisterColor("darkmagenta"			,0xff8b008b);
			RegisterColor("lavenderblush"		,0xfffff0f5);
			RegisterColor("cyan"				,0xff00ffff);
			RegisterColor("darkolivegreen"		,0xff556b2f);
			RegisterColor("firebrick"			,0xffb22222);
			RegisterColor("seashell"			,0xfffff5ee);
			RegisterColor("olivedrab"			,0xff6b8e23);
			RegisterColor("indianred"			,0xffcd5c5c);
			RegisterColor("indigo"				,0xff4b0082);
			RegisterColor("oldlace"				,0xfffdf5e6);
			RegisterColor("turquoise"			,0xff40e0d0);
			RegisterColor("rosybrown"			,0xffbc8f8f);
			RegisterColor("darkslateblue"		,0xff483d8b);
			RegisterColor("ivory"				,0xfffffff0);
			RegisterColor("mediumturquoise"		,0xff48d1cc);
			RegisterColor("darkkhaki"			,0xffbdb76b);
			RegisterColor("darksalmon"			,0xffe9967a);
			RegisterColor("blueviolet"			,0xff8a2be2);
			RegisterColor("honeydew"			,0xfff0fff0);
			RegisterColor("darkturquoise"		,0xff00ced1);
			RegisterColor("palegoldenrod"		,0xffeee8aa);
			RegisterColor("lightcoral"			,0xfff08080);
			RegisterColor("mediumpurple"		,0xff9370db);
			RegisterColor("mintcream"			,0xfff5fffa);
			RegisterColor("lightseagreen"		,0xff20b2aa);
			RegisterColor("cornsilk"			,0xfffff8dc);
			RegisterColor("salmon"				,0xfffa8072);
			RegisterColor("slateblue"			,0xff6a5acd);
			RegisterColor("azure"				,0xfff0ffff);
			RegisterColor("cadetblue"			,0xff5f9ea0);
			RegisterColor("beige"				,0xfff5f5dc);
			RegisterColor("lightsalmon"			,0xffffa07a);
			RegisterColor("mediumslateblue"		,0xff7b68ee);

			// SystemColors
			RegisterColor("activeborder"		,System.Drawing.SystemColors.ActiveBorder);
			RegisterColor("activecaption"		,System.Drawing.SystemColors.ActiveCaption);
			RegisterColor("appworkspace"		,System.Drawing.SystemColors.AppWorkspace);
			RegisterColor("background"			,System.Drawing.SystemColors.Desktop);
			RegisterColor("buttonface"			,System.Drawing.SystemColors.ButtonFace);
			RegisterColor("buttonhighlight"		,System.Drawing.SystemColors.ButtonHighlight);
			RegisterColor("buttonshadow"		,System.Drawing.SystemColors.ButtonShadow);
			RegisterColor("buttontext"			,System.Drawing.SystemColors.ControlText);
			RegisterColor("captiontext"			,System.Drawing.SystemColors.ActiveCaptionText);
			RegisterColor("graytext"			,System.Drawing.SystemColors.GrayText);
			RegisterColor("highlight"			,System.Drawing.SystemColors.Highlight);
			RegisterColor("highlighttext"		,System.Drawing.SystemColors.HighlightText);
			RegisterColor("inactiveborder"		,System.Drawing.SystemColors.InactiveBorder);
			RegisterColor("inactivecaption"		,System.Drawing.SystemColors.InactiveCaption);
			RegisterColor("inactivecaptiontext"	,System.Drawing.SystemColors.InactiveCaptionText);
			RegisterColor("infobackground"		,System.Drawing.SystemColors.Info);
			RegisterColor("infotext"			,System.Drawing.SystemColors.InfoText);
			RegisterColor("menu"				,System.Drawing.SystemColors.Menu);
			RegisterColor("menutext"			,System.Drawing.SystemColors.MenuText);
			RegisterColor("scrollbar"			,System.Drawing.SystemColors.ScrollBar);
			RegisterColor("threeddarkshadow"	,System.Drawing.SystemColors.ControlDarkDark);
			RegisterColor("threedface"			,System.Drawing.SystemColors.Control);
			RegisterColor("threedhighlight"		,System.Drawing.SystemColors.ControlLightLight);
			RegisterColor("threedlightshadow"	,System.Drawing.SystemColors.ControlLight);
			RegisterColor("threedshadow"		,System.Drawing.SystemColors.ControlDark);
			RegisterColor("window"				,System.Drawing.SystemColors.Window);
			RegisterColor("windowframe"			,System.Drawing.SystemColors.WindowFrame);
			RegisterColor("windowtext"			,System.Drawing.SystemColors.WindowText);

			RegisterColor("transparent"			,0x00000000);
		}
		#endregion
	}
}
