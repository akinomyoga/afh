namespace afh.HTML{
	public partial class HTMLDocument{
		public static string ApplyEntityReference(string text){
			return text.Replace("&","&amp;")
				.Replace("<","&lt;")
				.Replace(">","&gt;")
				.Replace("\"","&quot;")
				.Replace("'","&#39;");
		}
		/// <summary>
		/// エンティティ参照を解決します。
		/// </summary>
		/// <param name="text">エンティティ参照を含んでいる可能性のある文字列を指定します。</param>
		/// <returns>エンティティ参照を実際の文字列に置き換えた後の文字列を返します。</returns>
		public static string ResolveEntityReference(string text){
			int length=text.Length;
			int mode=0;
			System.Text.StringBuilder r=new System.Text.StringBuilder(length);
			System.Text.StringBuilder name=new System.Text.StringBuilder(20);
			for(int i=0;i<length;i++)switch(mode){
				case 0:
					if(text[i]=='&'){
						mode=1;
						name.Remove(0,name.Length);
					}else r.Append(text[i]);
					break;
				case 1: // & と ; の間
					if(text[i]==';'){
						string key=name.ToString();
						if(HTMLDocument.html_entities.ContainsKey(key)){
							r.Append(HTMLDocument.html_entities[key]);
						}else{
							r.Append("&"+name.ToString()+";");
						}
						mode=0;
					}else if('a'<=text[i]&&text[i]<='z'||'A'<=text[i]&&text[i]<='Z'||name.Length>0&&'0'<=text[i]&&text[i]<='9'){
						name.Append(text[i]);
					}else if(name.Length==0&&text[i]=='#'){
						mode=2;
					}else{
						mode=0;
						r.Append("&"+name.ToString());
						goto case 0;
					}
					break;
				case 2: // &# と ; の間
					if(text[i]==';'){
						if(0<name.Length&&name.Length<=5){
							r.Append((char)int.Parse(name.ToString()));
						}else{
							r.Append("&#"+name.ToString()+";");
						}
						mode=0;
					}else if('0'<=text[i]&&text[i]<='9'){
						name.Append(text[i]);
					}else if(name.Length==0&&text[i]=='x'){
						mode=3;
					}else{
						mode=0;
						r.Append("&#"+name.ToString());
						goto case 0;
					}
					break;
				case 3: // &#x と ; の間
					if(text[i]==';'){
						//TODO:
						if(0<name.Length&&name.Length<=4){
							r.Append((char)int.Parse(name.ToString(),System.Globalization.NumberStyles.HexNumber));
						}else{
							r.Append("&#x"+name.ToString()+";");
						}
						mode=0;
					}else if('a'<=text[i]&&text[i]<='f'||'A'<=text[i]&&text[i]<='F'||'0'<=text[i]&&text[i]<='9'){
						name.Append(text[i]);
					}else{
						mode=0;
						r.Append("&#x"+name.ToString());
						goto case 0;
					}
					break;
			}

			// 中途半端に残った物の処理
			switch(mode){
				case 1: // & と ; の間
					r.Append("&"+name.ToString());
					break;
				case 2: // &# と ; の間
					r.Append("&#"+name.ToString());
					break;
				case 3: // &#x と ; の間
					r.Append("&#x"+name.ToString());
					break;
			}

			return r.ToString();
		}
		private static System.Collections.Generic.Dictionary<string,char> html_entities;
		private static void initializeEntities(){
			html_entities=new System.Collections.Generic.Dictionary<string,char>(260);
			html_entities["nbsp"]=(char)160;
			html_entities["iexcl"]=(char)161;
			html_entities["cent"]=(char)162;
			html_entities["pound"]=(char)163;
			html_entities["curren"]=(char)164;
			html_entities["yen"]=(char)165;
			html_entities["brvbar"]=(char)166;
			html_entities["sect"]=(char)167;
			html_entities["uml"]=(char)168;
			html_entities["copy"]=(char)169;
			html_entities["ordf"]=(char)170;
			html_entities["laquo"]=(char)171;
			html_entities["not"]=(char)172;
			html_entities["shy"]=(char)173;
			html_entities["reg"]=(char)174;
			html_entities["macr"]=(char)175;
			html_entities["deg"]=(char)176;
			html_entities["plusmn"]=(char)177;
			html_entities["sup2"]=(char)178;
			html_entities["sup3"]=(char)179;
			html_entities["acute"]=(char)180;
			html_entities["micro"]=(char)181;
			html_entities["para"]=(char)182;
			html_entities["middot"]=(char)183;
			html_entities["cedil"]=(char)184;
			html_entities["sup1"]=(char)185;
			html_entities["ordm"]=(char)186;
			html_entities["raquo"]=(char)187;
			html_entities["frac14"]=(char)188;
			html_entities["frac12"]=(char)189;
			html_entities["frac34"]=(char)190;
			html_entities["iquest"]=(char)191;
			html_entities["Agrave"]=(char)192;
			html_entities["Aacute"]=(char)193;
			html_entities["Acirc"]=(char)194;
			html_entities["Atilde"]=(char)195;
			html_entities["Auml"]=(char)196;
			html_entities["Aring"]=(char)197;
			html_entities["AElig"]=(char)198;
			html_entities["Ccedil"]=(char)199;
			html_entities["Egrave"]=(char)200;
			html_entities["Eacute"]=(char)201;
			html_entities["Ecirc"]=(char)202;
			html_entities["Euml"]=(char)203;
			html_entities["Igrave"]=(char)204;
			html_entities["Iacute"]=(char)205;
			html_entities["Icirc"]=(char)206;
			html_entities["Iuml"]=(char)207;
			html_entities["ETH"]=(char)208;
			html_entities["Ntilde"]=(char)209;
			html_entities["Ograve"]=(char)210;
			html_entities["Oacute"]=(char)211;
			html_entities["Ocirc"]=(char)212;
			html_entities["Otilde"]=(char)213;
			html_entities["Ouml"]=(char)214;
			html_entities["times"]=(char)215;
			html_entities["Oslash"]=(char)216;
			html_entities["Ugrave"]=(char)217;
			html_entities["Uacute"]=(char)218;
			html_entities["Ucirc"]=(char)219;
			html_entities["Uuml"]=(char)220;
			html_entities["Yacute"]=(char)221;
			html_entities["THORN"]=(char)222;
			html_entities["szlig"]=(char)223;
			html_entities["agrave"]=(char)224;
			html_entities["aacute"]=(char)225;
			html_entities["acirc"]=(char)226;
			html_entities["atilde"]=(char)227;
			html_entities["auml"]=(char)228;
			html_entities["aring"]=(char)229;
			html_entities["aelig"]=(char)230;
			html_entities["ccedil"]=(char)231;
			html_entities["egrave"]=(char)232;
			html_entities["eacute"]=(char)233;
			html_entities["ecirc"]=(char)234;
			html_entities["euml"]=(char)235;
			html_entities["igrave"]=(char)236;
			html_entities["iacute"]=(char)237;
			html_entities["icirc"]=(char)238;
			html_entities["iuml"]=(char)239;
			html_entities["eth"]=(char)240;
			html_entities["ntilde"]=(char)241;
			html_entities["ograve"]=(char)242;
			html_entities["oacute"]=(char)243;
			html_entities["ocirc"]=(char)244;
			html_entities["otilde"]=(char)245;
			html_entities["ouml"]=(char)246;
			html_entities["divide"]=(char)247;
			html_entities["oslash"]=(char)248;
			html_entities["ugrave"]=(char)249;
			html_entities["uacute"]=(char)250;
			html_entities["ucirc"]=(char)251;
			html_entities["uuml"]=(char)252;
			html_entities["yacute"]=(char)253;
			html_entities["thorn"]=(char)254;
			html_entities["yuml"]=(char)255;
			html_entities["fnof"]=(char)402;
			html_entities["Alpha"]=(char)913;
			html_entities["Beta"]=(char)914;
			html_entities["Gamma"]=(char)915;
			html_entities["Delta"]=(char)916;
			html_entities["Epsilon"]=(char)917;
			html_entities["Zeta"]=(char)918;
			html_entities["Eta"]=(char)919;
			html_entities["Theta"]=(char)920;
			html_entities["Iota"]=(char)921;
			html_entities["Kappa"]=(char)922;
			html_entities["Lambda"]=(char)923;
			html_entities["Mu"]=(char)924;
			html_entities["Nu"]=(char)925;
			html_entities["Xi"]=(char)926;
			html_entities["Omicron"]=(char)927;
			html_entities["Pi"]=(char)928;
			html_entities["Rho"]=(char)929;
			html_entities["Sigma"]=(char)931;
			html_entities["Tau"]=(char)932;
			html_entities["Upsilon"]=(char)933;
			html_entities["Phi"]=(char)934;
			html_entities["Chi"]=(char)935;
			html_entities["Psi"]=(char)936;
			html_entities["Omega"]=(char)937;
			html_entities["alpha"]=(char)945;
			html_entities["beta"]=(char)946;
			html_entities["gamma"]=(char)947;
			html_entities["delta"]=(char)948;
			html_entities["epsilon"]=(char)949;
			html_entities["zeta"]=(char)950;
			html_entities["eta"]=(char)951;
			html_entities["theta"]=(char)952;
			html_entities["iota"]=(char)953;
			html_entities["kappa"]=(char)954;
			html_entities["lambda"]=(char)955;
			html_entities["mu"]=(char)956;
			html_entities["nu"]=(char)957;
			html_entities["xi"]=(char)958;
			html_entities["omicron"]=(char)959;
			html_entities["pi"]=(char)960;
			html_entities["rho"]=(char)961;
			html_entities["sigmaf"]=(char)962;
			html_entities["sigma"]=(char)963;
			html_entities["tau"]=(char)964;
			html_entities["upsilon"]=(char)965;
			html_entities["phi"]=(char)966;
			html_entities["chi"]=(char)967;
			html_entities["psi"]=(char)968;
			html_entities["omega"]=(char)969;
			html_entities["thetasym"]=(char)977;
			html_entities["upsih"]=(char)978;
			html_entities["piv"]=(char)982;
			html_entities["bull"]=(char)8226;
			html_entities["hellip"]=(char)8230;
			html_entities["prime"]=(char)8242;
			html_entities["Prime"]=(char)8243;
			html_entities["oline"]=(char)8254;
			html_entities["frasl"]=(char)8260;
			html_entities["weierp"]=(char)8472;
			html_entities["image"]=(char)8465;
			html_entities["real"]=(char)8476;
			html_entities["trade"]=(char)8482;
			html_entities["alefsym"]=(char)8501;
			html_entities["larr"]=(char)8592;
			html_entities["uarr"]=(char)8593;
			html_entities["rarr"]=(char)8594;
			html_entities["darr"]=(char)8595;
			html_entities["harr"]=(char)8596;
			html_entities["crarr"]=(char)8629;
			html_entities["lArr"]=(char)8656;
			html_entities["uArr"]=(char)8657;
			html_entities["rArr"]=(char)8658;
			html_entities["dArr"]=(char)8659;
			html_entities["hArr"]=(char)8660;
			html_entities["forall"]=(char)8704;
			html_entities["part"]=(char)8706;
			html_entities["exist"]=(char)8707;
			html_entities["empty"]=(char)8709;
			html_entities["nabla"]=(char)8711;
			html_entities["isin"]=(char)8712;
			html_entities["notin"]=(char)8713;
			html_entities["ni"]=(char)8715;
			html_entities["prod"]=(char)8719;
			html_entities["sum"]=(char)8721;
			html_entities["minus"]=(char)8722;
			html_entities["lowast"]=(char)8727;
			html_entities["radic"]=(char)8730;
			html_entities["prop"]=(char)8733;
			html_entities["infin"]=(char)8734;
			html_entities["ang"]=(char)8736;
			html_entities["and"]=(char)8743;
			html_entities["or"]=(char)8744;
			html_entities["cap"]=(char)8745;
			html_entities["cup"]=(char)8746;
			html_entities["int"]=(char)8747;
			html_entities["there4"]=(char)8756;
			html_entities["sim"]=(char)8764;
			html_entities["cong"]=(char)8773;
			html_entities["asymp"]=(char)8776;
			html_entities["ne"]=(char)8800;
			html_entities["equiv"]=(char)8801;
			html_entities["le"]=(char)8804;
			html_entities["ge"]=(char)8805;
			html_entities["sub"]=(char)8834;
			html_entities["sup"]=(char)8835;
			html_entities["nsub"]=(char)8836;
			html_entities["sube"]=(char)8838;
			html_entities["supe"]=(char)8839;
			html_entities["oplus"]=(char)8853;
			html_entities["otimes"]=(char)8855;
			html_entities["perp"]=(char)8869;
			html_entities["sdot"]=(char)8901;
			html_entities["lceil"]=(char)8968;
			html_entities["rceil"]=(char)8969;
			html_entities["lfloor"]=(char)8970;
			html_entities["rfloor"]=(char)8971;
			html_entities["lang"]=(char)9001;
			html_entities["rang"]=(char)9002;
			html_entities["loz"]=(char)9674;
			html_entities["spades"]=(char)9824;
			html_entities["clubs"]=(char)9827;
			html_entities["hearts"]=(char)9829;
			html_entities["diams"]=(char)9830;
			html_entities["quot"]=(char)34;
			html_entities["amp"]=(char)38;
			html_entities["lt"]=(char)60;
			html_entities["gt"]=(char)62;
			html_entities["OElig"]=(char)338;
			html_entities["oelig"]=(char)339;
			html_entities["Scaron"]=(char)352;
			html_entities["scaron"]=(char)353;
			html_entities["Yuml"]=(char)376;
			html_entities["circ"]=(char)710;
			html_entities["tilde"]=(char)732;
			html_entities["ensp"]=(char)8194;
			html_entities["emsp"]=(char)8195;
			html_entities["thinsp"]=(char)8201;
			html_entities["zwnj"]=(char)8204;
			html_entities["zwj"]=(char)8205;
			html_entities["lrm"]=(char)8206;
			html_entities["rlm"]=(char)8207;
			html_entities["ndash"]=(char)8211;
			html_entities["mdash"]=(char)8212;
			html_entities["lsquo"]=(char)8216;
			html_entities["rsquo"]=(char)8217;
			html_entities["sbquo"]=(char)8218;
			html_entities["ldquo"]=(char)8220;
			html_entities["rdquo"]=(char)8221;
			html_entities["bdquo"]=(char)8222;
			html_entities["dagger"]=(char)8224;
			html_entities["Dagger"]=(char)8225;
			html_entities["permil"]=(char)8240;
			html_entities["lsaquo"]=(char)8249;
			html_entities["rsaquo"]=(char)8250;
			html_entities["euro"]=(char)8364;
		}
	}
}
