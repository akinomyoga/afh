using afh.File.ID3v2_3_;

namespace afh.File.Mp3_{
	public class Mp3DataControl:System.Windows.Forms.UserControl{
		protected Mp3_.MP3File file;
		protected ID3v2_3_.Tag tag230;

		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual Mp3_.MP3File File{
			get{return this.file;}
			set{
				this.file=value;
				this.Enabled=value!=null;
				this.tag230=this.Enabled?value.Tag230:null;
				this.OnUpdateTag();
			}
		}
		/// <summary>
		/// 指定した tag を読み込みます。
		/// Tag が null の場合には Tag が何も設定されていない状態に初期化します。
		/// </summary>
		protected virtual void OnUpdateTag(){}
		/// <summary>
		/// Mp3DataControl のコンストラクタです。
		/// </summary>
		protected Mp3DataControl():base(){
			this.Enabled=false;
		}
	}
}