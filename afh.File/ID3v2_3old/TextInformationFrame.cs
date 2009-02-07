using Gen=System.Collections.Generic;
using EnumDesc=afh.EnumDescriptionAttribute;
namespace afh.File.ID3v2_3_{

	#region frame:TextInformation
	public class TextInformationFrame:Frame{
		public string text="";
		public virtual string Text{
			get{return RemoveNewLine(this.text);}
			set{this.text=value;}
		}
		public TextInformationFrame(FrameHeader header):base(header){
			this.ReadText(null);
		}
		/// <summary>
		/// 文字列を元のデータから読み出します。
		/// </summary>
		/// <param name="defaultEncoding">
		/// 既定の文字エンコーディングを指定します。
		/// 特に或る文字エンコーディングを使用したい場合に指定します。
		/// null を指定した場合にはシステムの既定文字エンコーディングが使用されます。
		/// </param>
		public void ReadText(System.Text.Encoding defaultEncoding){
			if(this.OriginalData!=null){
				this.OriginalData.Clear();
				this.Text=OriginalData.ReadTextToEnd(this.OriginalData.ReadByte()==1?null:defaultEncoding);
			}else{
				this.Text="";
				//throw new System.ApplicationException("これはファイルから読み取った frame ではありません。");
			}
		}

		protected static string RemoveNewLine(string str){
			return str.Replace("\r\n"," ").Replace('\r',' ').Replace('\n',' ');
		}
	}
	#endregion

	#region frame:NumericalString
	public class NumericalStringFrame:TextInformationFrame{
		private long value;
		private SignFlags sign;
		public long Value{
			get{return this.value;}
			set{this.value=value;}
		}
		public override string Text{
			get{return value.ToString();}
			set{
				long val;
				try{val=long.Parse(value);}catch(System.Exception e){
					__dll__.log.WriteError(
						e,
						string.Format(
@"'{0}' frame には数字列を指定して下さい。
※現在このプログラムでは {1} から {2} 迄の整数にしか対応していません。",
							this.header.frameId,
							long.MinValue,
							long.MaxValue
						)
					);
					return;
				}
				//-- 符号確認
				if(val>0){
					if((this.sign&SignFlags.Positive)==0){
						__dll__.log.WriteLine("正の整数は受容しない設定になっています: {0}",value);
						return;
					}
				}else if(val<0){
					if((this.sign&SignFlags.Negative)==0){
						__dll__.log.WriteLine("負の整数は受容しない設定になっています: {0}",value);
						return;
					}
				}else{
					if((this.sign&SignFlags.Zero)==0){
						__dll__.log.WriteLine("0 は受容しない設定になっています: 0");
						return;
					}
				}
			}
		}
		/// <summary>
		/// NumericalStringFrame のインスタンスを初期化します。
		/// 値としては自然数 (0 及び正の整数) を許容します。
		/// </summary>
		/// <param name="fheader">FrameHeader を指定します。</param>
		public NumericalStringFrame(FrameHeader fheader):this(fheader,SignFlags.Any){}
		/// <summary>
		/// 指定した許容符号設定を用いて NumericalStringFrame のインスタンスを初期化します。
		/// </summary>
		/// <param name="fheader">FrameHeader を指定します。</param>
		/// <param name="signature">値として許容する符号を指定します。</param>
		public NumericalStringFrame(FrameHeader fheader,SignFlags signature):base(fheader){
			this.sign=signature;
		}
		[System.Flags]
		public enum SignFlags{
			Positive=0x01,
			Negative=0x02,
			Zero    =0x04,
			Any     =0x07,
			/// <summary>
			/// 0 を含む自然数を示します。
			/// </summary>
			Natural =0x05
		}
	}
	#endregion

	#region frame:SlashNameList
	public class SlashNameListFrame:TextInformationFrame{
		private Gen::List<string> namelist;
		public Gen::List<string> Names{
			get{return this.namelist;}
		}
		public override string Text{
			get{
				if(this.namelist.Count==0)return "";
				if(this.namelist.Count==1)return this.namelist[0].Replace('/','／');
				string[] r0=this.namelist.ToArray();
				System.Text.StringBuilder strbuild=new System.Text.StringBuilder(r0[0].Replace('/','／'));
				for(int i=1;i<r0.Length;i++){
					strbuild.Append('/');
					strbuild.Append(r0[i].Replace('/','／'));
				}
				return RemoveNewLine(strbuild.ToString());
			}
			set{this.namelist=new System.Collections.Generic.List<string>(value.Split('/'));}
		}
		public SlashNameListFrame(FrameHeader fheader):base(fheader){}
	}
	#endregion

	#region frame:Year
	public class YearFrame:TextInformationFrame{
		private int val=0;
		public int Value{
			get{return this.val;}
			set{
				if(value<0||value>9999){
					__dll__.log.WriteError(string.Format("'{0}' frame には 0 以上 9999 以下の数値を指定して下さい。"));
				}
				this.val=value;
			}
		}
		public YearFrame(FrameHeader fheader):base(fheader){}
		public override string Text{
			get{return this.val.ToString("d4");}
			set{
				try{this.Value=int.Parse(value);}catch(System.Exception e){
					__dll__.log.WriteError(
						e,
						string.Format(
@"'{0}' frame は四桁の数字列で表現して下さい。
""{1}"" という値は受容できません。",this.header.frameId,value)
					);
				}
			}
		}
	}
	#endregion

	#region frame:Position
	public class PositionFrame:TextInformationFrame{
		private int position;
		/// <summary>
		/// 位置番号を取得亦は設定します。位置番号に負の値を指定する事は出来ません。
		/// </summary>
		public int Position{
			get{return this.position;}
			set{
				if(value<0){
					__dll__.log.WriteLine("この frame の position 値には正の値を指定して下さい。");
					return;
				}
				if(this.hastotal&&value>this.total){
					__dll__.log.WriteLine("指定しようとした位置番号は、音声の総数を超えています。音声の総数以下の値を指定して下さい。");
					return;
				}
				this.position=value;
			}
		}
		private bool hastotal;
		public bool HasTotalNumber{
			get{return this.hastotal;}
			set{
				if(!this.hastotal&&value&&this.total<this.position){
					this.total=this.position;
				}
				this.hastotal=value;
			}
		}
		private int total;
		/// <summary>
		/// 一纏まりの組の中に含まれる要素の総数を取得亦は設定します。
		/// </summary>
		public int TotalNumber{
			get{return this.total;}
			set{
				if(value<0){
					__dll__.log.WriteLine("この frame の position 値には正の値を指定して下さい。");
					return;
				}
				if(this.hastotal&&value>this.total){
					__dll__.log.WriteLine("指定しようとした音声の総数は、位置番号よりも小さな値です。位置番号よりも大きな総数を指定して下さい。");
					return;
				}
				this.total=value;
			}
		}
		public PositionFrame(FrameHeader fheader):base(fheader){}
		public override string Text{
			get{
				string r=this.position.ToString();
				return hastotal?r+"/"+this.total.ToString():r;
			}
			set{
				int index=value.IndexOf('/');
				string pos;
				System.Exception e;
				if(this.hastotal=index>=0){
					string tot=value.Substring(index+1);
					try{this.total=int.Parse(tot);}catch(System.Exception e1){e=e1;goto err;}
					pos=value.Substring(0,index);
				}else{
					this.total=0;
					pos=value;
				}
				try{this.position=int.Parse(pos);}catch(System.Exception e2){e=e2;goto err;}
				return;
			err:
				__dll__.log.WriteError(e,@"この種類の frame では、指定する文字列の形式は ""<自然数>"" 亦は ""<自然数>/<自然数>"" になる様にして下さい。
但し、<自然数> には 0x7FFFFFFF 以下の値を指定して下さい。(例: ""4/9"" など)
指定した文字列はこの形式から外れている為に解釈できません。
指定文字列: "+value);
			}
		}
	}
	#endregion

	#region frame:TDAT
	/// <summary>
	/// 音声を記録亦は作成した日付を管理する frame です。
	/// </summary>
	public class TDATFrame:TextInformationFrame{
		private int day;
		private int month;
		/// <summary>
		/// 日を取得亦は設定します。
		/// </summary>
		public int Day{
			get{return this.day;}
			set{
				if(value<1){
					__dll__.log.WriteLine("日付は 1 以上 31 以下の整数である必要があります。");
					value=1;
				}else if(value>31){
					__dll__.log.WriteLine("日付は 1 以上 31 以下の整数である必要があります。");
					value=31;
				}
				this.day=value;
			}
		}
		/// <summary>
		/// 月を取得亦は設定します。
		/// </summary>
		public int Month{
			get{return this.month;}
			set{
				if(value<1){
					__dll__.log.WriteLine("月は 1 以上 12 以下の整数である必要があります。");
					value=1;
				}else if(value>12){
					__dll__.log.WriteLine("月は 1 以上 12 以下の整数である必要があります。");
					value=12;
				}
				this.month=value;
			}
		}
		public override string Text{
			get{return this.day.ToString("d2")+this.month.ToString("d2");}
			set{
				if(value.Length!=4){
					__dll__.log.WriteLine("TDAT frame の内容は四桁の数字でなければ為りません。");
					while(value.Length<4)value+="0";
				}
				this.Day=10*(value[0]-'0')+(value[1]-'0');
				this.Month=10*(value[2]-'0')+(value[3]-'0');
			}
		}
		public TDATFrame(FrameHeader fh):base(fh){
			this.Text=this.text;
		}
	}
	#endregion

	#region frame:TIME
	/// <summary>
	/// 音声を記録亦は作成した日付を管理する frame です。
	/// </summary>
	public class TIMEFrame:TextInformationFrame{
		private int hour;
		private int minute;
		/// <summary>
		/// 時を取得亦は設定します。
		/// </summary>
		public int Hour{
			get{return this.hour;}
			set{
				if(value<1){
					__dll__.log.WriteLine("時は 1 以上 23 以下の整数である必要があります。");
					value=1;
				}else if(value>23){
					__dll__.log.WriteLine("時は 1 以上 23以下の整数である必要があります。");
					value=23;
				}
				this.hour=value;
			}
		}
		/// <summary>
		/// 分を取得亦は設定します。
		/// </summary>
		public int Minute{
			get{return this.minute;}
			set{
				if(value<1){
					__dll__.log.WriteLine("分は 1 以上 59 以下の整数である必要があります。");
					value=1;
				}else if(value>59){
					__dll__.log.WriteLine("分は 1 以上 59 以下の整数である必要があります。");
					value=59;
				}
				this.minute=value;
			}
		}
		public override string Text{
			get{return this.hour.ToString("d2")+this.minute.ToString("d2");}
			set{
				if(value.Length!=4){
					__dll__.log.WriteLine("TIME frame の内容は四桁の数字でなければ為りません。");
					while(value.Length<4)value+="0";
				}
				this.Hour=10*(value[0]-'0')+(value[1]-'0');
				this.Minute=10*(value[2]-'0')+(value[3]-'0');
			}
		}
		public TIMEFrame(FrameHeader fh):base(fh){
			this.Text=this.text;
		}
	}
	#endregion

	#region frame:TCON
	public sealed class TCONFrame:TextInformationFrame{
		private string description;
		/// <summary>
		/// Content-Type に関する詳細な説明を取得亦は設定します。
		/// </summary>
		public string Description{
			get{
				if(this.description.StartsWith("(("))return this.description.Substring(1);
				return this.description;
			}
			set{
				this.description=(value.Length>0&&value[0]=='(')?"("+value:value;
			}
		}
		private Gen::List<Genre> genres=new Gen::List<Genre>();
		/// <summary>
		/// Genre の一覧を取得します。
		/// </summary>
		public Gen::List<Genre> Genres{
			get{return this.genres;}
		}

		public TCONFrame(FrameHeader fheader):base(fheader){}
		public override string Text {
			get{
				System.Text.StringBuilder r=new System.Text.StringBuilder();
				foreach(Genre g in this.genres){
					r.Append(GenreToId(g));
				}
				r.Append(this.description);
				return r.ToString();
			}
			set{
				this.genres.Clear();
				int mode=0;
				string g=null;
				for(int i=0;i<value.Length;i++)switch(mode){
					case 0:
						if(value[i]=='('){
							mode=1;
							break;
						}else{
							this.description=value.Substring(i);
							return;
						}
					case 1:
						if(value[i]=='('){
							this.description=value.Substring(i-1);
							return;
						}else{
							mode=2;
							g="";
							goto case 2;
						}
					case 2:
						if(value[i]==')'){
							try{
								this.genres.Add(g=="RX"?Genre.RX:g=="CR"?Genre.CR:(Genre)byte.Parse(g));
							}catch(System.Exception e){
								__dll__.log.WriteError(e,"次の文字列は Content-Type として有効ではありません: "+g);
							}
							mode=0;
						}else{
							g+=value[i];
						}
						break;
				}
				this.description="";
			}
		}

		/// <summary>
		/// Genre を "(" と ")" で囲んで表した Id で表現します。
		/// </summary>
		/// <param name="g">識別子に変換する前の Genre を指定します。</param>
		/// <returns>"(0)" 等の識別子を文字列で返します。</returns>
		public static string GenreToId(Genre g){
			string id=g==Genre.RX||g==Genre.CR?g.ToString():((byte)g).ToString();
			return "("+id+")";
		}
	}
	#endregion

	#region enum:Genre
	public enum Genre:byte{
		Blues=0,
		[EnumDesc("Classic Rock")]
		ClassicRock=1,
		Country=2,
		Dance=3,
		Disco=4,
		Funk=5,
		Grunge=6,
		[EnumDesc("Hip-Hop")]
		HipHop=7,
		Jazz=8,
		Metal=9,
		[EnumDesc("New Age")]
		NewAge=10,
		Oldies=11,
		Other=12,
		Pop=13,
		[EnumDesc("R&B")]
		RAndB=14,
		Rap=15,
		Reggae=16,
		Rock=17,
		Techno=18,
		Industrial=19,
		Alternative=20,
		Ska=21,
		[EnumDesc("Death Metal")]
		DeathMetal=22,
		Prank=23,
		SoundTrack=24,
		[EnumDesc("Euro-Techno")]
		EuroTechno=25,
		Ambient=26,
		[EnumDesc("Trip-Hop")]
		TripHop=27,
		Vocal=28,
		[EnumDesc("Jazz+Funk")]
		JazzFunk=29,
		Fusion=30,
		Trance=31,
		Classical=32,
		Instrumental=33,
		Acid=34,
		House=35,
		Game=36,
		[EnumDesc("Sound Clip")]
		SoundClip=37,
		Gospel=38,
		Noise=39,
		AlternRock=40,
		Bass=41,
		Soul=42,
		Punk=43,
		Space=44,
		Meditative=45,
		[EnumDesc("Instrumental Pop")]
		InstrumentalPop=46,
		[EnumDesc("Instrumental Rock")]
		InstrumentalRock=47,
		Ethnic=48,
		Gothic=49,
		Darkwave=50,
		[EnumDesc("Techno-Industrial")]
		TechnoIndustrial=51,
		Electronic=52,
		[EnumDesc("Pop-Folk")]
		PopFolk=53,
		Eurodance=54,
		Dream=55,
		[EnumDesc("Southern Rock")]
		SouthernRock=56,
		Comedy=57,
		Cult=58,
		Gangsta=59,
		[EnumDesc("Top 40")]
		Top40=60,
		[EnumDesc("Christian Rap")]
		ChristianRap=61,
		[EnumDesc("Pop/Funk")]
		PopFunk=62,
		Jungle=63,
		[EnumDesc("Native American")]
		NativeAmerican=64,
		Cabaret=65,
		[EnumDesc("New Wave")]
		NewWave=66,
		Psychedelic=67,
		Rave=68,
		Showtunes=69,
		Trailer=70,
		[EnumDesc("Lo-Fi")]
		LoFi=71,
		Tribal=72,
		[EnumDesc("Acid Punk")]
		AcidPunk=73,
		[EnumDesc("Acid Jazz")]
		AcidJazz=74,
		Polca=75,
		Retro=76,
		Musical=77,
		[EnumDesc("Rock & Roll")]
		RockAndRoll=78,
		[EnumDesc("Hard Rock")]
		HardRock=79,

		//
		//	これより下は Winamp による拡張
		//

		Folk=80,
		[EnumDesc("Folk-Rock")]
		FolkRock=81,
		[EnumDesc("National Folk")]
		NationalFolk=82,
		Swing=83,
		[EnumDesc("Fast Fusion")]
		FastFusion=84,
		Bebob=85,
		Latin=86,
		Revival=87,
		Celtic=88,
		Bluegrass=89,
		Avantgarde=90,
		[EnumDesc("Gothic Rock")]
		GothicRock=91,
		[EnumDesc("Progressive Rock")]
		ProgressiveRock=92,
		[EnumDesc("Psychedelic Rock")]
		PsychedelicRock=93,
		[EnumDesc("Symphonic Rock")]
		SymphonicRock=94,
		[EnumDesc("Slow Rock")]
		SlowRock=95,
		[EnumDesc("Big Band")]
		BigBand=96,
		Chorus=97,
		[EnumDesc("Easy Listening")]
		EasyListening=98,
		Acoustic=99,
		Humour=100,
		Speech=101,
		Chanson=102,
		Opera=103,
		[EnumDesc("Chamber Music")]
		ChamberMusic=104,
		Sonata=105,
		Symphony=106,
		BootyBass=107,
		Primus=108,
		[EnumDesc("Porn Groove")]
		PornGroove=109,
		Satire=110,
		[EnumDesc("Slow Jam")]
		SlowJam=111,
		Club=112,
		Tango=113,
		Samba=114,
		Folklore=115,
		Ballad=116,
		[EnumDesc("Power Ballad")]
		PowerBallad=117,
		[EnumDesc("Rhythmic Soul")]
		RhythmicSoul=118,
		Freestyle=119,
		Duet=120,
		[EnumDesc("Punk Rock")]
		PunkRock=121,
		[EnumDesc("Drum Solo")]
		DrumSolo=122,
		[EnumDesc("A capella")]
		ACapella=123,
		[EnumDesc("Euro-Hosue")]
		EuroHouse=124,
		[EnumDesc("Dance Hall")]
		DanceHall=125,

		//
		// 更に
		//		参考:http://www.tsg.ne.jp/GANA/S/maid3/README.txt
		Goa=126,
		[EnumDesc("Drum & Bass")]
		DrumAndBass=127,
		[EnumDesc("Club-House")]
		ClubHouse=128,
		Hardcore=129,
		Terror=130,
		Indie=131,
		BripPop=132,
		Negerpunk=133,
		[EnumDesc("Polsk Punk")]
		PolskPunk=134,
		Beat=135,
		[EnumDesc("Christian Gangsta Rap")]
		ChristianGangstaRap=136,
		[EnumDesc("Heavy Metal")]
		HeavyMetal=137,
		[EnumDesc("Black Metal")]
		BlackMetal=138,
		Crossover=139,
		[EnumDesc("Contemporary Christian")]
		ContemporaryChristian=140,
		[EnumDesc("Christian Rock")]
		ChristianRock=141,
		Merengue=142,
		Salsa=143,
		[EnumDesc("Thrash Metal")]
		ThrashMetal=144,
		Anime=145,
		JPop=146,
		Synthpop=147,

		//
		//	ID3v2
		//
		[EnumDesc("Remix")]
		RX,
		[EnumDesc("Cover")]
		CR,

		//
		//	SCMPX 拡張
		//

		[EnumDesc("[SCMPX] Sacred")]
		Sacred=240,
		[EnumDesc("[SCMPX] Northern Europe")]
		NorthernEurope=241,
		[EnumDesc("[SCMPX] Irish & Scottish")]
		IrishAndScottish=242,
		[EnumDesc("[SCMPX] Scotland")]
		Scotland=243,
		[EnumDesc("[SCMPX] Ethnic Europe")]
		EthnicEurope=244,
		[EnumDesc("[SCMPX] 演歌")]
		Enka=245,
		[EnumDesc("[SCMPX] Children's Song")]
		ChildrensSong=246,
		[EnumDesc("[SCMPX] Heavy Rock (日本)")]
		HeavyRockJ=248,
		[EnumDesc("[SCMPX] Doom Rock (日本)")]
		DoomRockJ=249,
		[EnumDesc("[SCMPX] J-ポップ (日本)")]
		JPopJ=250,
		[EnumDesc("[SCMPX] 声優 (日本)")]
		SeiyuJ=251,
		[EnumDesc("[SCMPX] Tecno Ambient (日本)")]
		TecnoAmbientJ=252,
		[EnumDesc("[SCMPX] 萌え萌え (日本)")]
		MoemoeJ=253,
		[EnumDesc("[SCMPX] 特撮 (日本)")]
		TokusatsuJ=254,
		[EnumDesc("[SCMPX] アニメ (日本)")]
		AnimeJ=255
	}
	#endregion

	#region enum:IsrcNations
	public enum IsrcNations{
		[EnumDescription("アンドラ公国")]
		AD,
		[EnumDescription("アラブ首長国連邦")]
		AE,
		[EnumDescription("アフガニスタン・イスラム共和国")]
		AF,
		[EnumDescription("アンティグア・バーブーダ")]
		AG,
		[EnumDescription("アンギラ")]
		AI,
		[EnumDescription("アルバニア共和国")]
		AL,
		[EnumDescription("アルメニア共和国")]
		AM,
		[EnumDescription("オランダ領アンチル")]
		AN,
		[EnumDescription("アンゴラ共和国")]
		AO,
		[EnumDescription("南極大陸")]
		AQ,
		[EnumDescription("アルゼンチン共和国")]
		AR,
		[EnumDescription("米領サモア")]
		AS,
		[EnumDescription("オーストリア共和国")]
		AT,
		[EnumDescription("オーストラリア連邦")]
		AU,
		[EnumDescription("アルバ")]
		AW,
		[EnumDescription("オーランド諸島")]
		AX,
		[EnumDescription("アゼルバイジャン共和国")]
		AZ,
		[EnumDescription("ボスニア・ヘルツェゴビナ")]
		BA,
		[EnumDescription("バルバドス")]
		BB,
		[EnumDescription("バングラデシュ人民共和国")]
		BD,
		[EnumDescription("ベルギー王国")]
		BE,
		[EnumDescription("ブルキナファソ")]
		BF,
		[EnumDescription("ブルガリア共和国")]
		BG,
		[EnumDescription("バーレーン王国")]
		BH,
		[EnumDescription("ブルンジ共和国")]
		BI,
		[EnumDescription("ベナン共和国")]
		BJ,
		[EnumDescription("バーミューダ諸島")]
		BM,
		[EnumDescription("ブルネイ・ダルサラーム国")]
		BN,
		[EnumDescription("ボリビア共和国")]
		BO,
		[EnumDescription("ブラジル連邦共和国")]
		BR,
		[EnumDescription("バハマ国")]
		BS,
		[EnumDescription("ブータン王国")]
		BT,
		[EnumDescription("ブーベ島")]
		BV,
		[EnumDescription("ボツワナ共和国")]
		BW,
		[EnumDescription("ベラルーシ共和国")]
		BY,
		[EnumDescription("ベリーズ")]
		BZ,
		[EnumDescription("カナダ")]
		CA,
		[EnumDescription("ココス諸島")]
		CC,
		[EnumDescription("コンゴ民主共和国")]
		CD,
		[EnumDescription("中央アフリカ共和国")]
		CF,
		[EnumDescription("コンゴ共和国")]
		CG,
		[EnumDescription("スイス連邦")]
		CH,
		[EnumDescription("コートジボワール共和国")]
		CI,
		[EnumDescription("クック諸島")]
		CK,
		[EnumDescription("チリ共和国")]
		CL,
		[EnumDescription("カメルーン共和国")]
		CM,
		[EnumDescription("中華人民共和国")]
		CN,
		[EnumDescription("コロンビア共和国")]
		CO,
		[EnumDescription("コスタリカ共和国")]
		CR,
		[EnumDescription("セルビア・モンテネグロ")]
		CS,
		[EnumDescription("キューバ共和国")]
		CU,
		[EnumDescription("カーボヴェルデ共和国")]
		CV,
		[EnumDescription("クリスマス島")]
		CX,
		[EnumDescription("キプロス共和国")]
		CY,
		[EnumDescription("チェコ共和国")]
		CZ,
		[EnumDescription("ドイツ連邦共和国")]
		DE,
		[EnumDescription("ジブチ共和国")]
		DJ,
		[EnumDescription("デンマーク王国")]
		DK,
		[EnumDescription("ドミニカ国")]
		DM,
		[EnumDescription("ドミニカ共和国")]
		DO,
		[EnumDescription("アルジェリア民主人民共和国")]
		DZ,
		[EnumDescription("エクアドル共和国")]
		EC,
		[EnumDescription("エストニア共和国")]
		EE,
		[EnumDescription("エジプト・アラブ共和国")]
		EG,
		[EnumDescription("西サハラ")]
		EH,
		[EnumDescription("エリトリア国")]
		ER,
		[EnumDescription("スペイン")]
		ES,
		[EnumDescription("エチオピア連邦民主共和国")]
		ET,
		[EnumDescription("フィンランド共和国")]
		FI,
		[EnumDescription("フィジー諸島共和国")]
		FJ,
		[EnumDescription("フォークランド（マルビナス）諸島")]
		FK,
		[EnumDescription("ミクロネシア連邦")]
		FM,
		[EnumDescription("フェロー諸島")]
		FO,
		[EnumDescription("フランス共和国")]
		FR,
		[EnumDescription("ガボン共和国")]
		GA,
		[EnumDescription("グレートブリテン及び北部アイルランド連合王国")]
		GB,
		[EnumDescription("グレナダ")]
		GD,
		[EnumDescription("グルジア")]
		GE,
		[EnumDescription("仏領ギアナ")]
		GF,
		[EnumDescription("ガーナ共和国")]
		GH,
		[EnumDescription("ジブラルタル")]
		GI,
		[EnumDescription("グリーンランド")]
		GL,
		[EnumDescription("ガンビア共和国")]
		GM,
		[EnumDescription("ギニア共和国")]
		GN,
		[EnumDescription("グアドループ島")]
		GP,
		[EnumDescription("赤道ギニア共和国")]
		GQ,
		[EnumDescription("ギリシャ共和国")]
		GR,
		[EnumDescription("南ジョージア島・南サンドイッチ諸島")]
		GS,
		[EnumDescription("グアテマラ共和国")]
		GT,
		[EnumDescription("グアム")]
		GU,
		[EnumDescription("ギニアビサウ共和国")]
		GW,
		[EnumDescription("ガイアナ協同共和国")]
		GY,
		[EnumDescription("香港")]
		HK,
		[EnumDescription("ハード島・マクドナルド諸島")]
		HM,
		[EnumDescription("ホンジュラス共和国")]
		HN,
		[EnumDescription("クロアチア共和国")]
		HR,
		[EnumDescription("ハイチ共和国")]
		HT,
		[EnumDescription("ハンガリー共和国")]
		HU,
		[EnumDescription("インドネシア共和国")]
		ID,
		[EnumDescription("アイルランド")]
		IE,
		[EnumDescription("イスラエル国")]
		IL,
		[EnumDescription("インド")]
		IN,
		[EnumDescription("英領インド洋地域")]
		IO,
		[EnumDescription("イラク共和国")]
		IQ,
		[EnumDescription("イラン・イスラム共和国")]
		IR,
		[EnumDescription("アイスランド共和国")]
		IS,
		[EnumDescription("イタリア共和国")]
		IT,
		[EnumDescription("ジャマイカ")]
		JM,
		[EnumDescription("ヨルダン・ハシェミット王国")]
		JO,
		[EnumDescription("日本国")]
		JP,
		[EnumDescription("ケニア共和国")]
		KE,
		[EnumDescription("キルギス共和国")]
		KG,
		[EnumDescription("カンボジア王国")]
		KH,
		[EnumDescription("キリバス共和国")]
		KI,
		[EnumDescription("コモロ連合")]
		KM,
		[EnumDescription("セントクリストファー・ネーヴィス")]
		KN,
		[EnumDescription("朝鮮民主主義人民共和国")]
		KP,
		[EnumDescription("大韓民国")]
		KR,
		[EnumDescription("クウェート国")]
		KW,
		[EnumDescription("ケイマン諸島")]
		KY,
		[EnumDescription("カザフスタン共和国")]
		KZ,
		[EnumDescription("ラオス人民民主共和国")]
		LA,
		[EnumDescription("レバノン共和国")]
		LB,
		[EnumDescription("セントルシア")]
		LC,
		[EnumDescription("リヒテンシュタイン公国")]
		LI,
		[EnumDescription("スリランカ民主社会主義共和国")]
		LK,
		[EnumDescription("リベリア共和国")]
		LR,
		[EnumDescription("レソト王国")]
		LS,
		[EnumDescription("リトアニア共和国")]
		LT,
		[EnumDescription("ルクセンブルク大公国")]
		LU,
		[EnumDescription("ラトビア共和国")]
		LV,
		[EnumDescription("大リビア・アラブ社会主義人民ジャマーヒリーヤ国")]
		LY,
		[EnumDescription("モロッコ王国")]
		MA,
		[EnumDescription("モナコ公国")]
		MC,
		[EnumDescription("モルドバ共和国")]
		MD,
		[EnumDescription("マダガスカル共和国")]
		MG,
		[EnumDescription("マーシャル諸島共和国")]
		MH,
		[EnumDescription("マケドニア旧ユーゴスラビア共和国")]
		MK,
		[EnumDescription("マリ共和国")]
		ML,
		[EnumDescription("ミャンマー連邦")]
		MM,
		[EnumDescription("モンゴル国")]
		MN,
		[EnumDescription("マカオ")]
		MO,
		[EnumDescription("北マリアナ諸島連邦")]
		MP,
		[EnumDescription("マルティニーク島")]
		MQ,
		[EnumDescription("モーリタニア・イスラム共和国")]
		MR,
		[EnumDescription("モントセラト")]
		MS,
		[EnumDescription("マルタ共和国")]
		MT,
		[EnumDescription("モーリシャス共和国")]
		MU,
		[EnumDescription("モルディブ共和国")]
		MV,
		[EnumDescription("マラウイ共和国")]
		MW,
		[EnumDescription("メキシコ合衆国")]
		MX,
		[EnumDescription("マレーシア")]
		MY,
		[EnumDescription("モザンビーク共和国")]
		MZ,
		[EnumDescription("ナミビア共和国")]
		NA,
		[EnumDescription("ニューカレドニア")]
		NC,
		[EnumDescription("ニジェール共和国")]
		NE,
		[EnumDescription("ノーフォーク島")]
		NF,
		[EnumDescription("ナイジェリア連邦共和国")]
		NG,
		[EnumDescription("ニカラグア共和国")]
		NI,
		[EnumDescription("オランダ王国")]
		NL,
		[EnumDescription("ノルウェー王国")]
		NO,
		[EnumDescription("ネパール王国")]
		NP,
		[EnumDescription("ナウル共和国")]
		NR,
		[EnumDescription("ニウエ")]
		NU,
		[EnumDescription("ニュージーランド")]
		NZ,
		[EnumDescription("オマーン国")]
		OM,
		[EnumDescription("パナマ共和国")]
		PA,
		[EnumDescription("ペルー共和国")]
		PE,
		[EnumDescription("仏領ポリネシア")]
		PF,
		[EnumDescription("パプアニューギニア独立国")]
		PG,
		[EnumDescription("フィリピン共和国")]
		PH,
		[EnumDescription("パキスタン・イスラム共和国")]
		PK,
		[EnumDescription("ポーランド共和国")]
		PL,
		[EnumDescription("サンピエール島・ミクロン島")]
		PM,
		[EnumDescription("ピトケアン島")]
		PN,
		[EnumDescription("プエルトリコ")]
		PR,
		[EnumDescription("パレスチナ被占領地区")]
		PS,
		[EnumDescription("ポルトガル共和国")]
		PT,
		[EnumDescription("パラオ")]
		PW,
		[EnumDescription("パラグアイ共和国")]
		PY,
		[EnumDescription("カタール国")]
		QA,
		[EnumDescription("レユニオン")]
		RE,
		[EnumDescription("ルーマニア")]
		RO,
		[EnumDescription("ロシア連邦")]
		RU,
		[EnumDescription("ルワンダ共和国")]
		RW,
		[EnumDescription("サウジアラビア王国")]
		SA,
		[EnumDescription("ソロモン諸島")]
		SB,
		[EnumDescription("セーシェル共和国")]
		SC,
		[EnumDescription("スーダン共和国")]
		SD,
		[EnumDescription("スウェーデン王国")]
		SE,
		[EnumDescription("シンガポール共和国")]
		SG,
		[EnumDescription("セントヘレナ島")]
		SH,
		[EnumDescription("スロベニア共和国")]
		SI,
		[EnumDescription("スバールバル諸島・ヤンマイエン島")]
		SJ,
		[EnumDescription("スロバキア共和国")]
		SK,
		[EnumDescription("シエラレオネ共和国")]
		SL,
		[EnumDescription("サンマリノ共和国")]
		SM,
		[EnumDescription("セネガル共和国")]
		SN,
		[EnumDescription("ソマリア民主共和国")]
		SO,
		[EnumDescription("スリナム共和国")]
		SR,
		[EnumDescription("サントメ・プリンシペ民主共和国")]
		ST,
		[EnumDescription("エルサルバドル共和国")]
		SV,
		[EnumDescription("シリア・アラブ共和国")]
		SY,
		[EnumDescription("スワジランド王国")]
		SZ,
		[EnumDescription("タークス諸島・カイコス諸島")]
		TC,
		[EnumDescription("チャド共和国")]
		TD,
		[EnumDescription("仏領極南諸島")]
		TF,
		[EnumDescription("トーゴ共和国")]
		TG,
		[EnumDescription("タイ王国")]
		TH,
		[EnumDescription("タジキスタン共和国")]
		TJ,
		[EnumDescription("トケラウ諸島")]
		TK,
		[EnumDescription("東ティモール民主共和国")]
		TL,
		[EnumDescription("トルクメニスタン")]
		TM,
		[EnumDescription("チュニジア共和国")]
		TN,
		[EnumDescription("トンガ王国")]
		TO,
		[EnumDescription("トルコ共和国")]
		TR,
		[EnumDescription("トリニダード・トバゴ共和国")]
		TT,
		[EnumDescription("ツバル")]
		TV,
		[EnumDescription("台湾")]
		TW,
		[EnumDescription("タンザニア連合共和国")]
		TZ,
		[EnumDescription("ウクライナ")]
		UA,
		[EnumDescription("ウガンダ共和国")]
		UG,
		[EnumDescription("米領太平洋諸島")]
		UM,
		[EnumDescription("アメリカ合衆国")]
		US,
		[EnumDescription("ウルグアイ東方共和国")]
		UY,
		[EnumDescription("ウズベキスタン共和国")]
		UZ,
		[EnumDescription("バチカン市国")]
		VA,
		[EnumDescription("セントビンセント及びグレナディーン諸島")]
		VC,
		[EnumDescription("ベネズエラ・ボリバル共和国")]
		VE,
		[EnumDescription("英領バージン諸島")]
		VG,
		[EnumDescription("米領バージン諸島")]
		VI,
		[EnumDescription("ベトナム社会主義共和国")]
		VN,
		[EnumDescription("バヌアツ共和国")]
		VU,
		[EnumDescription("ワリス・フテュナ諸島")]
		WF,
		[EnumDescription("サモア独立国")]
		WS,
		[EnumDescription("イエメン共和国")]
		YE,
		[EnumDescription("マイヨット島")]
		YT,
		[EnumDescription("南アフリカ共和国")]
		ZA,
		[EnumDescription("ザンビア共和国")]
		ZM,
		[EnumDescription("ジンバブエ共和国")]
		ZW
	}
	#endregion
}