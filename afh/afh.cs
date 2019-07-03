using Ser=System.Runtime.Serialization;

namespace afh{
	/// <summary>
	/// �P���ȃC�x���g����������ׂ̃f���Q�[�g�ł��B
	/// </summary>
	public delegate void VoidEH(object sender);

	/// <summary>
	/// �������I����������ʒm���邾���̃R�[���o�b�N�ׂ̈̃f���Q�[�g�ł��B
	/// </summary>
	public delegate void VoidCB();
	/// <summary>
	/// ������Ɋւ���R�[���o�b�N�̃f���Q�[�g�ł��B
	/// </summary>
	public delegate void StringCB(string result);
	/// <summary>
	/// �^�U�l�Ɋւ���R�[���o�b�N�̃f���Q�[�g�ł��B
	/// </summary>
	public delegate void BoolCB(bool result);
	/// <summary>
	/// ��ʂ̃I�u�W�F�N�g�Ɋւ���R�[���o�b�N�̃f���Q�[�g�ł��B
	/// </summary>
	public delegate void ObjectCB(object result);

	/// <summary>
	/// ����I�u�W�F�N�g�������Ƃ���C�x���g�ׂ̈̃f���Q�[�g�ł��B
	/// </summary>
	/// <typeparam name="T">�C�x���g�̈����̌^��\���܂��B
	/// ����́A�K������ System.EventArgs ���p��������̂ł���K�v�͂���܂���B</typeparam>
	/// <param name="sender">�C�x���g�̔��������I�u�W�F�N�g���w�肵�܂��B</param>
	/// <param name="args">�C�x���g�Ɋւ������ێ�����C�x���g�������w�肵�܂��B</param>
	public delegate void EventHandler<T>(object sender,T args);
	/// <summary>
	/// ����I�u�W�F�N�g�������Ƃ���C�x���g�ׂ̈̃f���Q�[�g�ł��B
	/// </summary>
	/// <typeparam name="TObj">�C�x���g�̔�������I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
	/// <typeparam name="TArg">�C�x���g�̈����̌^���w�肵�܂��B
	/// ����́A�K������ System.EventArgs ���p��������̂ł���K�v�͂���܂���B</typeparam>
	/// <param name="sender">�C�x���g�̔��������I�u�W�F�N�g���w�肵�܂��B</param>
	/// <param name="args">�C�x���g�Ɋւ������ێ�����C�x���g�������w�肵�܂��B</param>
	public delegate void EventHandler<TObj,TArg>(TObj sender,TArg args);
	/// <summary>
	/// ����l��Ԃ��R�[���o�b�N�ׂ̈̃f���Q�[�g�ł��B
	/// </summary>
	/// <typeparam name="T">�Ԃ��l�̌^��\���܂��B</typeparam>
	/// <param name="value">�Ԃ��l���w�肵�܂��B</param>
	public delegate void CallBack<T>(T value);

	/// <summary>
	/// ���l�ɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public static partial class Math{}


	/// <summary>
	/// �f�o�O�Ɋւ���@�\��񋟂��܂��B
	/// </summary>
	public static class DebugUtils{
		/// <summary>
		/// �������m�F���A��������������Ă��Ȃ��ꍇ�ɂ� AssertionFailException �𔭐������܂��B
		/// </summary>
		/// <param name="cond">�������������̒��Ɏw�肵�܂��B</param>
		/// <param name="messages">��������������Ȃ������ꍇ�ɕ\������郁�b�Z�[�W���w�肵�܂��B</param>
		[System.Diagnostics.Conditional("DEBUG")]
		[System.Diagnostics.DebuggerHidden]
		public static void Assert(bool cond,params string[] messages){
			if(!cond)
				throw new AssertionFailException(
					"Assertion Failed: "+afh.Text.TextUtils.Join(messages,"\r\n")
				);
		}
	}
	/// <summary>
	/// �v���O��������̏�ł̑z�肪�j�ꂽ�ۂɔ��������O�ł��B
	/// </summary>
	[System.Serializable]
	public class AssertionFailException:System.Exception{
		/// <summary>
		/// AssertionFailException �̊���̏��������s���܂��B
		/// </summary>
		public AssertionFailException(){}
		/// <summary>
		/// �w�肵�����b�Z�[�W���g�p���� AssertionFailException �����������܂��B
		/// </summary>
		/// <param name="msg">�j�ꂽ�z��ɂ��Ă̏ڍׂȐ������w�肵�܂��B</param>
		public AssertionFailException(string msg):base(msg){}
		/// <summary>
		/// �w�肵�����b�Z�[�W���g�p���� AssertionFailException �����������܂��B
		/// </summary>
		/// <param name="msg">�j�ꂽ�z��ɂ��Ă̏ڍׂȐ������w�肵�܂��B</param>
		/// <param name="innerException">���̗�O�𔭐������錴���ƂȂ���������O���w�肵�܂��B</param>
		public AssertionFailException(string msg,System.Exception innerException)
			:base(msg,innerException){}
		/// <summary>
		/// �V���A�������� AssertionFailException �𕜌����܂��B
		/// </summary>
		/// <param name="info">�V���A���������f�[�^��ێ�����C���X�^���X���w�肵�܂��B</param>
		/// <param name="ctx">�V���A�����̊��ɂ��Ă̏����w�肵�܂��B</param>
		public AssertionFailException(Ser::SerializationInfo info,Ser::StreamingContext ctx)
			:base(info,ctx){}
	}
}
