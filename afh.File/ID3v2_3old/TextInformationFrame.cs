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
		/// ����������̃f�[�^����ǂݏo���܂��B
		/// </summary>
		/// <param name="defaultEncoding">
		/// ����̕����G���R�[�f�B���O���w�肵�܂��B
		/// ���Ɉ��镶���G���R�[�f�B���O���g�p�������ꍇ�Ɏw�肵�܂��B
		/// null ���w�肵���ꍇ�ɂ̓V�X�e���̊��蕶���G���R�[�f�B���O���g�p����܂��B
		/// </param>
		public void ReadText(System.Text.Encoding defaultEncoding){
			if(this.OriginalData!=null){
				this.OriginalData.Clear();
				this.Text=OriginalData.ReadTextToEnd(this.OriginalData.ReadByte()==1?null:defaultEncoding);
			}else{
				this.Text="";
				//throw new System.ApplicationException("����̓t�@�C������ǂݎ���� frame �ł͂���܂���B");
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
@"'{0}' frame �ɂ͐�������w�肵�ĉ������B
�����݂��̃v���O�����ł� {1} ���� {2} ���̐����ɂ����Ή����Ă��܂���B",
							this.header.frameId,
							long.MinValue,
							long.MaxValue
						)
					);
					return;
				}
				//-- �����m�F
				if(val>0){
					if((this.sign&SignFlags.Positive)==0){
						__dll__.log.WriteLine("���̐����͎�e���Ȃ��ݒ�ɂȂ��Ă��܂�: {0}",value);
						return;
					}
				}else if(val<0){
					if((this.sign&SignFlags.Negative)==0){
						__dll__.log.WriteLine("���̐����͎�e���Ȃ��ݒ�ɂȂ��Ă��܂�: {0}",value);
						return;
					}
				}else{
					if((this.sign&SignFlags.Zero)==0){
						__dll__.log.WriteLine("0 �͎�e���Ȃ��ݒ�ɂȂ��Ă��܂�: 0");
						return;
					}
				}
			}
		}
		/// <summary>
		/// NumericalStringFrame �̃C���X�^���X�����������܂��B
		/// �l�Ƃ��Ă͎��R�� (0 �y�ѐ��̐���) �����e���܂��B
		/// </summary>
		/// <param name="fheader">FrameHeader ���w�肵�܂��B</param>
		public NumericalStringFrame(FrameHeader fheader):this(fheader,SignFlags.Any){}
		/// <summary>
		/// �w�肵�����e�����ݒ��p���� NumericalStringFrame �̃C���X�^���X�����������܂��B
		/// </summary>
		/// <param name="fheader">FrameHeader ���w�肵�܂��B</param>
		/// <param name="signature">�l�Ƃ��ċ��e���镄�����w�肵�܂��B</param>
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
			/// 0 ���܂ގ��R���������܂��B
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
				if(this.namelist.Count==1)return this.namelist[0].Replace('/','�^');
				string[] r0=this.namelist.ToArray();
				System.Text.StringBuilder strbuild=new System.Text.StringBuilder(r0[0].Replace('/','�^'));
				for(int i=1;i<r0.Length;i++){
					strbuild.Append('/');
					strbuild.Append(r0[i].Replace('/','�^'));
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
					__dll__.log.WriteError(string.Format("'{0}' frame �ɂ� 0 �ȏ� 9999 �ȉ��̐��l���w�肵�ĉ������B"));
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
@"'{0}' frame �͎l���̐�����ŕ\�����ĉ������B
""{1}"" �Ƃ����l�͎�e�ł��܂���B",this.header.frameId,value)
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
		/// �ʒu�ԍ����擾���͐ݒ肵�܂��B�ʒu�ԍ��ɕ��̒l���w�肷�鎖�͏o���܂���B
		/// </summary>
		public int Position{
			get{return this.position;}
			set{
				if(value<0){
					__dll__.log.WriteLine("���� frame �� position �l�ɂ͐��̒l���w�肵�ĉ������B");
					return;
				}
				if(this.hastotal&&value>this.total){
					__dll__.log.WriteLine("�w�肵�悤�Ƃ����ʒu�ԍ��́A�����̑����𒴂��Ă��܂��B�����̑����ȉ��̒l���w�肵�ĉ������B");
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
		/// ��Z�܂�̑g�̒��Ɋ܂܂��v�f�̑������擾���͐ݒ肵�܂��B
		/// </summary>
		public int TotalNumber{
			get{return this.total;}
			set{
				if(value<0){
					__dll__.log.WriteLine("���� frame �� position �l�ɂ͐��̒l���w�肵�ĉ������B");
					return;
				}
				if(this.hastotal&&value>this.total){
					__dll__.log.WriteLine("�w�肵�悤�Ƃ��������̑����́A�ʒu�ԍ����������Ȓl�ł��B�ʒu�ԍ������傫�ȑ������w�肵�ĉ������B");
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
				__dll__.log.WriteError(e,@"���̎�ނ� frame �ł́A�w�肷�镶����̌`���� ""<���R��>"" ���� ""<���R��>/<���R��>"" �ɂȂ�l�ɂ��ĉ������B
�A���A<���R��> �ɂ� 0x7FFFFFFF �ȉ��̒l���w�肵�ĉ������B(��: ""4/9"" �Ȃ�)
�w�肵��������͂��̌`������O��Ă���ׂɉ��߂ł��܂���B
�w�蕶����: "+value);
			}
		}
	}
	#endregion

	#region frame:TDAT
	/// <summary>
	/// �������L�^���͍쐬�������t���Ǘ����� frame �ł��B
	/// </summary>
	public class TDATFrame:TextInformationFrame{
		private int day;
		private int month;
		/// <summary>
		/// �����擾���͐ݒ肵�܂��B
		/// </summary>
		public int Day{
			get{return this.day;}
			set{
				if(value<1){
					__dll__.log.WriteLine("���t�� 1 �ȏ� 31 �ȉ��̐����ł���K�v������܂��B");
					value=1;
				}else if(value>31){
					__dll__.log.WriteLine("���t�� 1 �ȏ� 31 �ȉ��̐����ł���K�v������܂��B");
					value=31;
				}
				this.day=value;
			}
		}
		/// <summary>
		/// �����擾���͐ݒ肵�܂��B
		/// </summary>
		public int Month{
			get{return this.month;}
			set{
				if(value<1){
					__dll__.log.WriteLine("���� 1 �ȏ� 12 �ȉ��̐����ł���K�v������܂��B");
					value=1;
				}else if(value>12){
					__dll__.log.WriteLine("���� 1 �ȏ� 12 �ȉ��̐����ł���K�v������܂��B");
					value=12;
				}
				this.month=value;
			}
		}
		public override string Text{
			get{return this.day.ToString("d2")+this.month.ToString("d2");}
			set{
				if(value.Length!=4){
					__dll__.log.WriteLine("TDAT frame �̓��e�͎l���̐����łȂ���Έׂ�܂���B");
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
	/// �������L�^���͍쐬�������t���Ǘ����� frame �ł��B
	/// </summary>
	public class TIMEFrame:TextInformationFrame{
		private int hour;
		private int minute;
		/// <summary>
		/// �����擾���͐ݒ肵�܂��B
		/// </summary>
		public int Hour{
			get{return this.hour;}
			set{
				if(value<1){
					__dll__.log.WriteLine("���� 1 �ȏ� 23 �ȉ��̐����ł���K�v������܂��B");
					value=1;
				}else if(value>23){
					__dll__.log.WriteLine("���� 1 �ȏ� 23�ȉ��̐����ł���K�v������܂��B");
					value=23;
				}
				this.hour=value;
			}
		}
		/// <summary>
		/// �����擾���͐ݒ肵�܂��B
		/// </summary>
		public int Minute{
			get{return this.minute;}
			set{
				if(value<1){
					__dll__.log.WriteLine("���� 1 �ȏ� 59 �ȉ��̐����ł���K�v������܂��B");
					value=1;
				}else if(value>59){
					__dll__.log.WriteLine("���� 1 �ȏ� 59 �ȉ��̐����ł���K�v������܂��B");
					value=59;
				}
				this.minute=value;
			}
		}
		public override string Text{
			get{return this.hour.ToString("d2")+this.minute.ToString("d2");}
			set{
				if(value.Length!=4){
					__dll__.log.WriteLine("TIME frame �̓��e�͎l���̐����łȂ���Έׂ�܂���B");
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
		/// Content-Type �Ɋւ���ڍׂȐ������擾���͐ݒ肵�܂��B
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
		/// Genre �̈ꗗ���擾���܂��B
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
								__dll__.log.WriteError(e,"���̕������ Content-Type �Ƃ��ėL���ł͂���܂���: "+g);
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
		/// Genre �� "(" �� ")" �ň͂�ŕ\���� Id �ŕ\�����܂��B
		/// </summary>
		/// <param name="g">���ʎq�ɕϊ�����O�� Genre ���w�肵�܂��B</param>
		/// <returns>"(0)" ���̎��ʎq�𕶎���ŕԂ��܂��B</returns>
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
		//	�����艺�� Winamp �ɂ��g��
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
		// �X��
		//		�Q�l:http://www.tsg.ne.jp/GANA/S/maid3/README.txt
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
		//	SCMPX �g��
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
		[EnumDesc("[SCMPX] ����")]
		Enka=245,
		[EnumDesc("[SCMPX] Children's Song")]
		ChildrensSong=246,
		[EnumDesc("[SCMPX] Heavy Rock (���{)")]
		HeavyRockJ=248,
		[EnumDesc("[SCMPX] Doom Rock (���{)")]
		DoomRockJ=249,
		[EnumDesc("[SCMPX] J-�|�b�v (���{)")]
		JPopJ=250,
		[EnumDesc("[SCMPX] ���D (���{)")]
		SeiyuJ=251,
		[EnumDesc("[SCMPX] Tecno Ambient (���{)")]
		TecnoAmbientJ=252,
		[EnumDesc("[SCMPX] �G���G�� (���{)")]
		MoemoeJ=253,
		[EnumDesc("[SCMPX] ���B (���{)")]
		TokusatsuJ=254,
		[EnumDesc("[SCMPX] �A�j�� (���{)")]
		AnimeJ=255
	}
	#endregion

	#region enum:IsrcNations
	public enum IsrcNations{
		[EnumDescription("�A���h������")]
		AD,
		[EnumDescription("�A���u�񒷍��A�M")]
		AE,
		[EnumDescription("�A�t�K�j�X�^���E�C�X�������a��")]
		AF,
		[EnumDescription("�A���e�B�O�A�E�o�[�u�[�_")]
		AG,
		[EnumDescription("�A���M��")]
		AI,
		[EnumDescription("�A���o�j�A���a��")]
		AL,
		[EnumDescription("�A�����j�A���a��")]
		AM,
		[EnumDescription("�I�����_�̃A���`��")]
		AN,
		[EnumDescription("�A���S�����a��")]
		AO,
		[EnumDescription("��ɑ嗤")]
		AQ,
		[EnumDescription("�A���[���`�����a��")]
		AR,
		[EnumDescription("�ė̃T���A")]
		AS,
		[EnumDescription("�I�[�X�g���A���a��")]
		AT,
		[EnumDescription("�I�[�X�g�����A�A�M")]
		AU,
		[EnumDescription("�A���o")]
		AW,
		[EnumDescription("�I�[�����h����")]
		AX,
		[EnumDescription("�A�[���o�C�W�������a��")]
		AZ,
		[EnumDescription("�{�X�j�A�E�w���c�F�S�r�i")]
		BA,
		[EnumDescription("�o���o�h�X")]
		BB,
		[EnumDescription("�o���O���f�V���l�����a��")]
		BD,
		[EnumDescription("�x���M�[����")]
		BE,
		[EnumDescription("�u���L�i�t�@�\")]
		BF,
		[EnumDescription("�u���K���A���a��")]
		BG,
		[EnumDescription("�o�[���[������")]
		BH,
		[EnumDescription("�u�����W���a��")]
		BI,
		[EnumDescription("�x�i�����a��")]
		BJ,
		[EnumDescription("�o�[�~���[�_����")]
		BM,
		[EnumDescription("�u���l�C�E�_���T���[����")]
		BN,
		[EnumDescription("�{���r�A���a��")]
		BO,
		[EnumDescription("�u���W���A�M���a��")]
		BR,
		[EnumDescription("�o�n�}��")]
		BS,
		[EnumDescription("�u�[�^������")]
		BT,
		[EnumDescription("�u�[�x��")]
		BV,
		[EnumDescription("�{�c���i���a��")]
		BW,
		[EnumDescription("�x�����[�V���a��")]
		BY,
		[EnumDescription("�x���[�Y")]
		BZ,
		[EnumDescription("�J�i�_")]
		CA,
		[EnumDescription("�R�R�X����")]
		CC,
		[EnumDescription("�R���S���勤�a��")]
		CD,
		[EnumDescription("�����A�t���J���a��")]
		CF,
		[EnumDescription("�R���S���a��")]
		CG,
		[EnumDescription("�X�C�X�A�M")]
		CH,
		[EnumDescription("�R�[�g�W�{���[�����a��")]
		CI,
		[EnumDescription("�N�b�N����")]
		CK,
		[EnumDescription("�`�����a��")]
		CL,
		[EnumDescription("�J�����[�����a��")]
		CM,
		[EnumDescription("���ؐl�����a��")]
		CN,
		[EnumDescription("�R�����r�A���a��")]
		CO,
		[EnumDescription("�R�X�^���J���a��")]
		CR,
		[EnumDescription("�Z���r�A�E�����e�l�O��")]
		CS,
		[EnumDescription("�L���[�o���a��")]
		CU,
		[EnumDescription("�J�[�{���F���f���a��")]
		CV,
		[EnumDescription("�N���X�}�X��")]
		CX,
		[EnumDescription("�L�v���X���a��")]
		CY,
		[EnumDescription("�`�F�R���a��")]
		CZ,
		[EnumDescription("�h�C�c�A�M���a��")]
		DE,
		[EnumDescription("�W�u�`���a��")]
		DJ,
		[EnumDescription("�f���}�[�N����")]
		DK,
		[EnumDescription("�h�~�j�J��")]
		DM,
		[EnumDescription("�h�~�j�J���a��")]
		DO,
		[EnumDescription("�A���W�F���A����l�����a��")]
		DZ,
		[EnumDescription("�G�N�A�h�����a��")]
		EC,
		[EnumDescription("�G�X�g�j�A���a��")]
		EE,
		[EnumDescription("�G�W�v�g�E�A���u���a��")]
		EG,
		[EnumDescription("���T�n��")]
		EH,
		[EnumDescription("�G���g���A��")]
		ER,
		[EnumDescription("�X�y�C��")]
		ES,
		[EnumDescription("�G�`�I�s�A�A�M���勤�a��")]
		ET,
		[EnumDescription("�t�B�������h���a��")]
		FI,
		[EnumDescription("�t�B�W�[�������a��")]
		FJ,
		[EnumDescription("�t�H�[�N�����h�i�}���r�i�X�j����")]
		FK,
		[EnumDescription("�~�N���l�V�A�A�M")]
		FM,
		[EnumDescription("�t�F���[����")]
		FO,
		[EnumDescription("�t�����X���a��")]
		FR,
		[EnumDescription("�K�{�����a��")]
		GA,
		[EnumDescription("�O���[�g�u���e���y�іk���A�C�������h�A������")]
		GB,
		[EnumDescription("�O���i�_")]
		GD,
		[EnumDescription("�O���W�A")]
		GE,
		[EnumDescription("���̃M�A�i")]
		GF,
		[EnumDescription("�K�[�i���a��")]
		GH,
		[EnumDescription("�W�u�����^��")]
		GI,
		[EnumDescription("�O���[�������h")]
		GL,
		[EnumDescription("�K���r�A���a��")]
		GM,
		[EnumDescription("�M�j�A���a��")]
		GN,
		[EnumDescription("�O�A�h���[�v��")]
		GP,
		[EnumDescription("�ԓ��M�j�A���a��")]
		GQ,
		[EnumDescription("�M���V�����a��")]
		GR,
		[EnumDescription("��W���[�W�A���E��T���h�C�b�`����")]
		GS,
		[EnumDescription("�O�A�e�}�����a��")]
		GT,
		[EnumDescription("�O�A��")]
		GU,
		[EnumDescription("�M�j�A�r�T�E���a��")]
		GW,
		[EnumDescription("�K�C�A�i�������a��")]
		GY,
		[EnumDescription("���`")]
		HK,
		[EnumDescription("�n�[�h���E�}�N�h�i���h����")]
		HM,
		[EnumDescription("�z���W�����X���a��")]
		HN,
		[EnumDescription("�N���A�`�A���a��")]
		HR,
		[EnumDescription("�n�C�`���a��")]
		HT,
		[EnumDescription("�n���K���[���a��")]
		HU,
		[EnumDescription("�C���h�l�V�A���a��")]
		ID,
		[EnumDescription("�A�C�������h")]
		IE,
		[EnumDescription("�C�X���G����")]
		IL,
		[EnumDescription("�C���h")]
		IN,
		[EnumDescription("�p�̃C���h�m�n��")]
		IO,
		[EnumDescription("�C���N���a��")]
		IQ,
		[EnumDescription("�C�����E�C�X�������a��")]
		IR,
		[EnumDescription("�A�C�X�����h���a��")]
		IS,
		[EnumDescription("�C�^���A���a��")]
		IT,
		[EnumDescription("�W���}�C�J")]
		JM,
		[EnumDescription("�����_���E�n�V�F�~�b�g����")]
		JO,
		[EnumDescription("���{��")]
		JP,
		[EnumDescription("�P�j�A���a��")]
		KE,
		[EnumDescription("�L���M�X���a��")]
		KG,
		[EnumDescription("�J���{�W�A����")]
		KH,
		[EnumDescription("�L���o�X���a��")]
		KI,
		[EnumDescription("�R�����A��")]
		KM,
		[EnumDescription("�Z���g�N���X�g�t�@�[�E�l�[���B�X")]
		KN,
		[EnumDescription("���N�����`�l�����a��")]
		KP,
		[EnumDescription("��ؖ���")]
		KR,
		[EnumDescription("�N�E�F�[�g��")]
		KW,
		[EnumDescription("�P�C�}������")]
		KY,
		[EnumDescription("�J�U�t�X�^�����a��")]
		KZ,
		[EnumDescription("���I�X�l�����勤�a��")]
		LA,
		[EnumDescription("���o�m�����a��")]
		LB,
		[EnumDescription("�Z���g���V�A")]
		LC,
		[EnumDescription("���q�e���V���^�C������")]
		LI,
		[EnumDescription("�X�������J����Љ��`���a��")]
		LK,
		[EnumDescription("���x���A���a��")]
		LR,
		[EnumDescription("���\�g����")]
		LS,
		[EnumDescription("���g�A�j�A���a��")]
		LT,
		[EnumDescription("���N�Z���u���N�����")]
		LU,
		[EnumDescription("���g�r�A���a��")]
		LV,
		[EnumDescription("�僊�r�A�E�A���u�Љ��`�l���W���}�[�q���[����")]
		LY,
		[EnumDescription("�����b�R����")]
		MA,
		[EnumDescription("���i�R����")]
		MC,
		[EnumDescription("�����h�o���a��")]
		MD,
		[EnumDescription("�}�_�K�X�J�����a��")]
		MG,
		[EnumDescription("�}�[�V�����������a��")]
		MH,
		[EnumDescription("�}�P�h�j�A�����[�S�X���r�A���a��")]
		MK,
		[EnumDescription("�}�����a��")]
		ML,
		[EnumDescription("�~�����}�[�A�M")]
		MM,
		[EnumDescription("�����S����")]
		MN,
		[EnumDescription("�}�J�I")]
		MO,
		[EnumDescription("�k�}���A�i�����A�M")]
		MP,
		[EnumDescription("�}���e�B�j�[�N��")]
		MQ,
		[EnumDescription("���[���^�j�A�E�C�X�������a��")]
		MR,
		[EnumDescription("�����g�Z���g")]
		MS,
		[EnumDescription("�}���^���a��")]
		MT,
		[EnumDescription("���[���V���X���a��")]
		MU,
		[EnumDescription("�����f�B�u���a��")]
		MV,
		[EnumDescription("�}���E�C���a��")]
		MW,
		[EnumDescription("���L�V�R���O��")]
		MX,
		[EnumDescription("�}���[�V�A")]
		MY,
		[EnumDescription("���U���r�[�N���a��")]
		MZ,
		[EnumDescription("�i�~�r�A���a��")]
		NA,
		[EnumDescription("�j���[�J���h�j�A")]
		NC,
		[EnumDescription("�j�W�F�[�����a��")]
		NE,
		[EnumDescription("�m�[�t�H�[�N��")]
		NF,
		[EnumDescription("�i�C�W�F���A�A�M���a��")]
		NG,
		[EnumDescription("�j�J���O�A���a��")]
		NI,
		[EnumDescription("�I�����_����")]
		NL,
		[EnumDescription("�m���E�F�[����")]
		NO,
		[EnumDescription("�l�p�[������")]
		NP,
		[EnumDescription("�i�E�����a��")]
		NR,
		[EnumDescription("�j�E�G")]
		NU,
		[EnumDescription("�j���[�W�[�����h")]
		NZ,
		[EnumDescription("�I�}�[����")]
		OM,
		[EnumDescription("�p�i�}���a��")]
		PA,
		[EnumDescription("�y���[���a��")]
		PE,
		[EnumDescription("���̃|���l�V�A")]
		PF,
		[EnumDescription("�p�v�A�j���[�M�j�A�Ɨ���")]
		PG,
		[EnumDescription("�t�B���s�����a��")]
		PH,
		[EnumDescription("�p�L�X�^���E�C�X�������a��")]
		PK,
		[EnumDescription("�|�[�����h���a��")]
		PL,
		[EnumDescription("�T���s�G�[�����E�~�N������")]
		PM,
		[EnumDescription("�s�g�P�A����")]
		PN,
		[EnumDescription("�v�G���g���R")]
		PR,
		[EnumDescription("�p���X�`�i���̒n��")]
		PS,
		[EnumDescription("�|���g�K�����a��")]
		PT,
		[EnumDescription("�p���I")]
		PW,
		[EnumDescription("�p���O�A�C���a��")]
		PY,
		[EnumDescription("�J�^�[����")]
		QA,
		[EnumDescription("�����j�I��")]
		RE,
		[EnumDescription("���[�}�j�A")]
		RO,
		[EnumDescription("���V�A�A�M")]
		RU,
		[EnumDescription("�������_���a��")]
		RW,
		[EnumDescription("�T�E�W�A���r�A����")]
		SA,
		[EnumDescription("�\����������")]
		SB,
		[EnumDescription("�Z�[�V�F�����a��")]
		SC,
		[EnumDescription("�X�[�_�����a��")]
		SD,
		[EnumDescription("�X�E�F�[�f������")]
		SE,
		[EnumDescription("�V���K�|�[�����a��")]
		SG,
		[EnumDescription("�Z���g�w���i��")]
		SH,
		[EnumDescription("�X���x�j�A���a��")]
		SI,
		[EnumDescription("�X�o�[���o�������E�����}�C�G����")]
		SJ,
		[EnumDescription("�X���o�L�A���a��")]
		SK,
		[EnumDescription("�V�G�����I�l���a��")]
		SL,
		[EnumDescription("�T���}���m���a��")]
		SM,
		[EnumDescription("�Z�l�K�����a��")]
		SN,
		[EnumDescription("�\�}���A���勤�a��")]
		SO,
		[EnumDescription("�X���i�����a��")]
		SR,
		[EnumDescription("�T���g���E�v�����V�y���勤�a��")]
		ST,
		[EnumDescription("�G���T���o�h�����a��")]
		SV,
		[EnumDescription("�V���A�E�A���u���a��")]
		SY,
		[EnumDescription("�X���W�����h����")]
		SZ,
		[EnumDescription("�^�[�N�X�����E�J�C�R�X����")]
		TC,
		[EnumDescription("�`���h���a��")]
		TD,
		[EnumDescription("���̋ɓ쏔��")]
		TF,
		[EnumDescription("�g�[�S���a��")]
		TG,
		[EnumDescription("�^�C����")]
		TH,
		[EnumDescription("�^�W�L�X�^�����a��")]
		TJ,
		[EnumDescription("�g�P���E����")]
		TK,
		[EnumDescription("���e�B���[�����勤�a��")]
		TL,
		[EnumDescription("�g���N���j�X�^��")]
		TM,
		[EnumDescription("�`���j�W�A���a��")]
		TN,
		[EnumDescription("�g���K����")]
		TO,
		[EnumDescription("�g���R���a��")]
		TR,
		[EnumDescription("�g���j�_�[�h�E�g�o�S���a��")]
		TT,
		[EnumDescription("�c�o��")]
		TV,
		[EnumDescription("��p")]
		TW,
		[EnumDescription("�^���U�j�A�A�����a��")]
		TZ,
		[EnumDescription("�E�N���C�i")]
		UA,
		[EnumDescription("�E�K���_���a��")]
		UG,
		[EnumDescription("�ė̑����m����")]
		UM,
		[EnumDescription("�A�����J���O��")]
		US,
		[EnumDescription("�E���O�A�C�������a��")]
		UY,
		[EnumDescription("�E�Y�x�L�X�^�����a��")]
		UZ,
		[EnumDescription("�o�`�J���s��")]
		VA,
		[EnumDescription("�Z���g�r���Z���g�y�уO���i�f�B�[������")]
		VC,
		[EnumDescription("�x�l�Y�G���E�{���o�����a��")]
		VE,
		[EnumDescription("�p�̃o�[�W������")]
		VG,
		[EnumDescription("�ė̃o�[�W������")]
		VI,
		[EnumDescription("�x�g�i���Љ��`���a��")]
		VN,
		[EnumDescription("�o�k�A�c���a��")]
		VU,
		[EnumDescription("�����X�E�t�e���i����")]
		WF,
		[EnumDescription("�T���A�Ɨ���")]
		WS,
		[EnumDescription("�C�G�������a��")]
		YE,
		[EnumDescription("�}�C���b�g��")]
		YT,
		[EnumDescription("��A�t���J���a��")]
		ZA,
		[EnumDescription("�U���r�A���a��")]
		ZM,
		[EnumDescription("�W���o�u�G���a��")]
		ZW
	}
	#endregion
}