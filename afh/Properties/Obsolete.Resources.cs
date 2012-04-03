namespace afh{
	[System.Obsolete()]
	internal static class Resource {
		private static System.Reflection.Assembly sys_design;
		[System.Obsolete()]
		public static System.Reflection.Assembly SysDesign {
			get{
				return sys_design??(sys_design=System.Reflection.Assembly.GetAssembly(typeof(System.ComponentModel.Design.CollectionEditor)));
			}
		}
		private static System.Reflection.Assembly sys_win_form;
		[System.Obsolete()]
		public static System.Reflection.Assembly SysWinForm {
			get{
				return sys_win_form??(sys_win_form=System.Reflection.Assembly.GetAssembly(typeof(System.Windows.Forms.Form)));
			}
		}

		[System.Obsolete("afh.Drawing.Icons")]
		public static class Icons{
			static System.Drawing.Bitmap sortup;
			public static System.Drawing.Bitmap SortUp{
				get{
					const string SORTUP="System.Web.UI.Design.WebControls.SortUp.ico";
					//const string SORTUP="System.ComponentModel.Design.SortUp.ico";
					return sortup??(sortup=ReadBitmap(SysDesign,SORTUP));
				}
			}
			static System.Drawing.Bitmap sortdn;
			public static System.Drawing.Bitmap SortDown{
				get{
					const string SORTDN="System.Web.UI.Design.WebControls.SortDown.ico";
					//const string SORTDN="System.ComponentModel.Design.SortDown.ico";
					return sortdn??(sortdn=ReadBitmap(SysDesign,SORTDN));
				}
			}
			static System.Drawing.Bitmap delete;
			public static System.Drawing.Bitmap Delete{
				get{
					//const string DELETE="System.Web.UI.Design.WebControls.Delete.ico"; // ← GDI+ でエラーが起こる。
					return delete??(delete=ReadBitmap(SysDesign,"System.Windows.Forms.Design.Delete.ico"));
				}
			}
			static System.Drawing.Bitmap addnew;
			public static System.Drawing.Bitmap AddNew{
				get{
					if(addnew==null){
						addnew=ReadBitmap(SysWinForm,"System.Windows.Forms.BindingNavigator.AddNew.bmp");
						afh.Drawing.BitmapEffect.ReplaceColor(
							addnew,
							(afh.Drawing.Color32Argb)addnew.GetPixel(0,0),
							(afh.Drawing.Color32Argb)System.Drawing.Color.Transparent
							);
					}
					return addnew;
				}
			}
			private static System.Drawing.Bitmap ReadBitmap(System.Reflection.Assembly asm,string key){
				using(System.IO.Stream str=asm.GetManifestResourceStream(key)){
					System.Drawing.Bitmap bmp0=new System.Drawing.Bitmap(str);
					System.Drawing.Bitmap bmp=bmp0.Clone(
						new System.Drawing.Rectangle(0,0,bmp0.Width-1,bmp0.Height-1),
						System.Drawing.Imaging.PixelFormat.Format32bppArgb
						);
					bmp0.Dispose();
					return bmp;
				}
			}
#if DEBUG
			public static System.Drawing.Bitmap test(){
				const string ADDNEW="System.Windows.Forms.BindingNavigator.AddNew.bmp";
				System.Reflection.Assembly asm=System.Reflection.Assembly.GetAssembly(typeof(System.Windows.Forms.Form));
				using(System.IO.Stream str=asm.GetManifestResourceStream(ADDNEW)){
					System.Drawing.Bitmap bmp0=(System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(str);
					System.Drawing.Bitmap bmp=bmp0.Clone(
						new System.Drawing.Rectangle(0,0,bmp0.Width,bmp0.Height),
						System.Drawing.Imaging.PixelFormat.Format32bppArgb
						);
					bmp0.Dispose();
					afh.Drawing.BitmapEffect.ReplaceColor(
						bmp,
						(afh.Drawing.Color32Argb)bmp.GetPixel(0,0),
						(afh.Drawing.Color32Argb)System.Drawing.Color.Transparent
						);
					return bmp;
				}
			}
#endif
		}
	}
}