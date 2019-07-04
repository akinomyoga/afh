namespace Tester.Type{
	[TestFunction("System.Type.GetHashCode()","�e type �� HashCode ���擾���܂�")]
	public class TestTypeHash:Tester.TestFunction{
		public override string Exec(){
			this.WriteType(typeof(short));
			this.WriteType(typeof(ushort));
			this.WriteType(typeof(int));
			this.WriteType(typeof(uint));
			this.WriteType(typeof(long));
			this.WriteType(typeof(ulong));
			this.WriteType(typeof(sbyte));
			this.WriteType(typeof(byte));
			this.WriteType(typeof(decimal));
			this.WriteType(typeof(char));
			this.WriteType(typeof(float));
			this.WriteType(typeof(double));
			this.WriteType(typeof(bool));
			this.WriteType(typeof(string));
			this.WriteType(typeof(System.IntPtr));
			this.WriteType(typeof(System.UIntPtr));
			this.WriteType(typeof(System.Guid));
			this.WriteType(typeof(System.TimeSpan));
			this.WriteType(typeof(System.DateTime));
			this.WriteType(typeof(System.Type));
			this.WriteType(typeof(System.Enum));
			this.WriteType(typeof(void));
			return this.outstr;
		}
		private void WriteType(System.Type t){
			//this.WriteLine("�^ "+t.ToString()+":");
			//this.WriteLine("\tHash:\t"+t.GetHashCode().ToString());
			//this.WriteLine("\tHash&0xf:\t"+(t.GetHashCode()&0xf).ToString());
			string str=t.ToString();
			//str=str.Substring(str.LastIndexOf(".")+1);
			this.WriteLine(str+"\t=0x"+t.GetHashCode().ToString("X")+",");
		}
	}
	[TestFunction("EventCatcherCode ����",@"�O��:
	�e delegate type.GUID �� HashCode ������ switch �ŕ��򂷂�R�[�h�𐶐����A
	AddEventHandler ���܂��B
	HashCode �����R��v���镨���������ꍇ�Ȃǂɂ͑Ή����Ă��܂���
�㔼:
	AddEventHandler �Ɏg�p�����֐������������܂��B")]
	public class EventCatcher:TestFunction{
		private const string PART1=@"#region [�v���O���������R�[�h] �C�x���g�t�b�N
// .NET Framework 2.0 �ł� DynamicMethod �Ŏ��s���Ɋ֐����쐬�ł���
/// <summary>
/// obj �� e �Ŏw�肵���C�x���g�� OnChanged ���Ăяo���֐����t�b�N���܂��B
/// <list type=""table"">
/// <listheader>�Ή����Ă��� EventHandler �f���Q�[�g�̈ꗗ</listheader>";
		private const string PART2=@"/// </list>
/// </summary>
protected virtual void HookEvent(object obj,System.Reflection.EventInfo e){
	System.Type t=e.EventHandlerType;
	switch(t.GUID.GetHashCode()&0xffff){";
		private const string PART3=@"default:
			throw new System.Exception(""�w�肵���C�x���g�̎�ނɂ͑Ή����Ă��܂���"");
	}
}";
		public EventCatcher(){}
		private delegate void TypeProcessor(System.Type t);
		public override string Exec(){
			this.WriteLine(PART1);
			this.WriteEventHandlers(new TypeProcessor(this.WriteCodeCommentList));
			this.WriteLine(PART2);
			this.WriteEventHandlers(new TypeProcessor(this.WriteCases));
			this.WriteLine(PART3);
			this.WriteEventHandlers(new TypeProcessor(this.WriteMethod));
			this.WriteLine("#endregion");
			return this.outstr;
		}
		private void WriteEventHandlers(TypeProcessor Write){
			Write(typeof(System.EventHandler));
			Write(typeof(System.Windows.Forms.ColumnClickEventHandler));
			Write(typeof(System.Windows.Forms.ContentsResizedEventHandler));
			Write(typeof(System.Windows.Forms.ControlEventHandler));
			Write(typeof(System.Windows.Forms.ConvertEventHandler));
			Write(typeof(System.Windows.Forms.DateBoldEventHandler));
			Write(typeof(System.Windows.Forms.DateRangeEventHandler));
			Write(typeof(System.Windows.Forms.DragEventHandler));
			Write(typeof(System.Windows.Forms.DrawItemEventHandler));
			Write(typeof(System.Windows.Forms.GiveFeedbackEventHandler));
			Write(typeof(System.Windows.Forms.HelpEventHandler));
			Write(typeof(System.Windows.Forms.InputLanguageChangedEventHandler));
			Write(typeof(System.Windows.Forms.InputLanguageChangingEventHandler));
			Write(typeof(System.Windows.Forms.InvalidateEventHandler));
			Write(typeof(System.Windows.Forms.ItemChangedEventHandler));
			Write(typeof(System.Windows.Forms.ItemCheckEventHandler));
			Write(typeof(System.Windows.Forms.ItemDragEventHandler));
			Write(typeof(System.Windows.Forms.KeyEventHandler));
			Write(typeof(System.Windows.Forms.KeyPressEventHandler));
			Write(typeof(System.Windows.Forms.LabelEditEventHandler));
			Write(typeof(System.Windows.Forms.LayoutEventHandler));
			Write(typeof(System.Windows.Forms.LinkClickedEventHandler));
			Write(typeof(System.Windows.Forms.LinkLabelLinkClickedEventHandler));
			Write(typeof(System.Windows.Forms.MeasureItemEventHandler));
			//Write(typeof(System.Windows.Forms.MethodInvoker));
			Write(typeof(System.Windows.Forms.MouseEventHandler));
			Write(typeof(System.Windows.Forms.NavigateEventHandler));
			Write(typeof(System.Windows.Forms.NodeLabelEditEventHandler));
			Write(typeof(System.Windows.Forms.PaintEventHandler));
			Write(typeof(System.Windows.Forms.PropertyTabChangedEventHandler));
			Write(typeof(System.Windows.Forms.PropertyValueChangedEventHandler));
			Write(typeof(System.Windows.Forms.QueryAccessibilityHelpEventHandler));
			Write(typeof(System.Windows.Forms.QueryContinueDragEventHandler));
			Write(typeof(System.Windows.Forms.ScrollEventHandler));
			Write(typeof(System.Windows.Forms.SelectedGridItemChangedEventHandler));
			Write(typeof(System.Windows.Forms.SplitterEventHandler));
			Write(typeof(System.Windows.Forms.StatusBarDrawItemEventHandler));
			Write(typeof(System.Windows.Forms.StatusBarPanelClickEventHandler));
			Write(typeof(System.Windows.Forms.ToolBarButtonClickEventHandler));
			Write(typeof(System.Windows.Forms.TreeViewCancelEventHandler));
			Write(typeof(System.Windows.Forms.TreeViewEventHandler));
			Write(typeof(System.Windows.Forms.UICuesEventHandler));
			Write(typeof(System.Windows.Forms.UpDownEventHandler));
		}
		private void WriteCodeCommentList(System.Type t){
			this.WriteLine("/// <item><see cref=\""+t.FullName+"\"/></item>");
		}
		private void WriteCases(System.Type t){
			//--�֐����̌���
			string name=t.FullName;
			int i=name.LastIndexOf(".");
			if(i>=0)name=name.Substring(i+1).Replace("EventHandler","");
			if(name=="")name="Event";
			name="obj_"+name;

			//--�֐��̏����o��
			//�� params �� sender ���Ȃ����̓R���p�C���G���[�ɂȂ�̂ŕ�����B
			this.WriteLine("\t\tcase 0x"+(t.GUID.GetHashCode()&0xffff).ToString("X8")+":");
			this.WriteLine("\t\t\tif(t!=typeof("+t.ToString()+"))goto default;");
			this.WriteLine("\t\t\te.AddEventHandler(obj,new "+t.FullName+"(this."+name+"));");
			this.WriteLine("\t\t\tbreak;");
		}
		private void WriteMethod(System.Type t){
			//string code="this.OnChanged(sender);";
			string code="this.Exec();";
			//--�֐����̌���
			string name=t.FullName;
			int i=name.LastIndexOf(".");
			if(i>=0)name=name.Substring(i+1).Replace("EventHandler","");
			if(name=="")name="Event";
			name="obj_"+name;

			//--�������X�g�̎擾
			System.Reflection.ParameterInfo[] pms=t.GetMethod("Invoke").GetParameters();
			string plist="";
			int iM=pms.Length;
			for(i=0;i<iM;i++)plist+=","+pms[i].ParameterType.FullName+" "+pms[i].Name;
			
			//--�֐��̏����o��
			//�� params �� sender ���Ȃ����̓R���p�C���G���[�ɂȂ�̂ŕ�����B
			this.WriteLine("private void "+name+"("+plist.TrimStart(',')+"){"+code+"}");
		}
		public static implicit operator EventCatcher(string str1){return new EventCatcher();}
		public static implicit operator string(EventCatcher str2){return "this is EventCatcher";}
	}
	[TestFunction("�e�^���� TypeConverter ���擾",@"")]
	public class TypeConverter:Tester.TestFunction{
		public override string Exec(){
			//this.WriteLine("switch(t.FullName.GetHashCode()){");
			this.WriteTypes1();
			//this.WriteLine("\tdefault:");
			//this.WriteLine("\t\tbreak;");
			//this.WriteLine("}");
			return this.outstr;
		}
		private void WriteTypes1(){
			this.WriteType(typeof(System.Globalization.CultureInfo));
			this.WriteType(typeof(System.Configuration.ConfigurationSettings));
			this.WriteType(typeof(System.Drawing.Color));
			this.WriteType(typeof(System.Drawing.Font));
			this.WriteType(typeof(System.Drawing.Image));
			this.WriteType(typeof(System.Drawing.Imaging.ImageFormat));
			this.WriteType(typeof(System.Drawing.Point));
			this.WriteType(typeof(System.Drawing.PointF));
			this.WriteType(typeof(System.Drawing.Rectangle));
			this.WriteType(typeof(System.Drawing.RectangleF));
			this.WriteType(typeof(System.Drawing.Size));
			this.WriteType(typeof(System.Drawing.SizeF));
			this.WriteType(typeof(System.Resources.ResXFileRef));
			this.WriteType(typeof(System.Windows.Forms.AxHost.State));
			this.WriteType(typeof(System.Windows.Forms.Cursor));
			this.WriteType(typeof(System.Windows.Forms.LinkArea));
			this.WriteType(typeof(System.Windows.Forms.Binding));
			this.WriteType(typeof(System.Windows.Forms.ScrollableControl.DockPaddingEdges));
			this.WriteType(typeof(System.Windows.Forms.SelectionRange));
			this.WriteType(typeof(System.Windows.Forms.TreeNode));
			//new System.Windows.Forms.AxHost.StateConverter();
		}
		private void WriteType(System.Type t){
			/*/
			string name=t.FullName;
			int hash=0x7fffffff&name.GetHashCode();
			this.WriteLine("\tcase 0x"+hash.ToString("X8")+":");
			this.WriteLine("\t\tif(t!=typeof("+t.FullName+"))goto default;");
			this.WriteLine("\t\tbreak;");
			/*/
			System.ComponentModel.TypeConverter conv=System.ComponentModel.TypeDescriptor.GetConverter(t);
			string converter=conv==null?"null":conv.GetType().FullName;
			this.WriteLine("<tr><td>"+t.FullName+"</td><td>"+converter+"</td></tr>");
			//*/

		}
	}
}