using CM=System.ComponentModel;
using Gen=System.Collections.Generic;

namespace afh.Forms.Design{
	public sealed class TreeNodeSettingsConverter:CM::ExpandableObjectConverter{
		public override bool CanConvertTo(CM::ITypeDescriptorContext context,System.Type destinationType){
			if(destinationType==typeof(string))return true;
			return base.CanConvertTo(context,destinationType);
		}
		public override object ConvertTo(
			CM::ITypeDescriptorContext context,
			System.Globalization.CultureInfo culture, 
			object value, 
			System.Type destinationType
		){
			if(destinationType==typeof(string)&&value is TreeNodeSettings){
				return "(Node ��{�ݒ�)"; // ���݂̏�Ԃ�\��������
			}
			return base.ConvertTo(context,culture,value,destinationType);
		}
	}
	/// <summary>
	/// TreeNodeCheckBox �ɑ΂��� TypeConverter �ł��B
	/// </summary>
	public sealed class TreeNodeCheckBoxConverter:CM::TypeConverter{
		//============================================================
		//		�ϊ��q
		//============================================================
		public override bool CanConvertTo(CM::ITypeDescriptorContext context,System.Type destinationType){
			if(typeof(string)==destinationType)return true;
			//if(typeof(ITreeNodeCheckBox)==destinationType)return true;
			if(typeof(CM::Design.Serialization.InstanceDescriptor)==destinationType)return true;
			return base.CanConvertTo(context,destinationType);
		}
		public override object ConvertTo(
			CM::ITypeDescriptorContext context,System.Globalization.CultureInfo culture,
			object value,System.Type destinationType
		){
			if(typeof(string)==destinationType){
				string ret=TreeNodeCheckBox.GetName((ITreeNodeCheckBox)value);
				if(ret!=null)return ret;
				return "("+value.GetType().ToString()+")"+value.ToString();
			}else if(typeof(CM::Design.Serialization.InstanceDescriptor)==destinationType){
				string name=TreeNodeCheckBox.GetName((ITreeNodeCheckBox)value);
				if(name==null){
					name="DoubleBorder";
				}

				return new CM::Design.Serialization.InstanceDescriptor(
					((System.Converter<string,ITreeNodeCheckBox>)GetInstance).Method,
					new object[]{name}
				);
			}
			return base.ConvertTo(context,culture,value,destinationType);
		}
		public override bool CanConvertFrom(CM::ITypeDescriptorContext context,System.Type sourceType){
			if(typeof(string)==sourceType)return true;
			if(typeof(ITreeNodeCheckBox).IsAssignableFrom(sourceType))return true;
			return base.CanConvertFrom(context,sourceType);
		}
		public override object ConvertFrom(CM::ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object value) {
			if(value is string){
				ITreeNodeCheckBox ret=TreeNodeCheckBox.GetInstance((string)value);
				if(ret!=null)return ret;
				return TreeNodeCheckBox.DoubleBorder;
			}else if(value is ITreeNodeCheckBox){
				return value;
			}
			return base.ConvertFrom(context,culture,value);
		}
		//============================================================
		//		�i�����ϊ�
		//============================================================
		public static ITreeNodeCheckBox GetInstance(string name){
			return TreeNodeCheckBox.GetInstance(name);
		}
		//============================================================
		//		�l�̌��B
		//============================================================
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
			return true;
		}
		public override CM::TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
			// ��: string �^�z���Ԃ�����AConvertTo �� string ���n����鎖�ɂȂ�B
			return new StandardValuesCollection(TreeNodeCheckBox.CheckBoxInstances);
		}
	}
}