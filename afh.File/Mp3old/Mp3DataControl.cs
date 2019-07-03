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
		/// �w�肵�� tag ��ǂݍ��݂܂��B
		/// Tag �� null �̏ꍇ�ɂ� Tag �������ݒ肳��Ă��Ȃ���Ԃɏ��������܂��B
		/// </summary>
		protected virtual void OnUpdateTag(){}
		/// <summary>
		/// Mp3DataControl �̃R���X�g���N�^�ł��B
		/// </summary>
		protected Mp3DataControl():base(){
			this.Enabled=false;
		}
	}
}