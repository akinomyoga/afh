#if NET3_5
#define HAS_LINQ
#elif NET4_0
#define HAS_LINQ
#endif

using Gen=System.Collections.Generic;

#if NET2_0
namespace System.Runtime.CompilerServices{
	/// <summary>
	/// �g�����\�b�h�ł��鎖�������ׂ̑����ł��B
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly|AttributeTargets.Class|AttributeTargets.Method)]
	public sealed class ExtensionAttribute:Attribute{}
}
#endif

namespace afh.Collections.Utils{
	/// <summary>
	/// �R���N�V�����ɑ΂��鑀���񋟂��܂��B
	/// </summary>
	public static class Utility{
		/// <summary>
		/// �������e�����X�^�b�N�̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <typeparam name="T">�X�^�b�N�̗v�f�̌^���w�肵�܂��B</typeparam>
		/// <param name="stack">�R�s�[���̃X�^�b�N���w�肵�܂��B</param>
		/// <returns>�w�肵���X�^�b�N�Ɠ������e�����A�V�����X�^�b�N��Ԃ��܂��B</returns>
		public static Gen::Stack<T> Clone<T>(this Gen::Stack<T> stack){
			T[] values=stack.ToArray();

			Gen::Stack<T> ret=new Gen::Stack<T>(values.Length);
			for(int i=values.Length-1;i>=0;i--)
				ret.Push(values[i]);
			return ret;
		}
		/// <summary>
		/// �X�^�b�N�̊e�v�f��ϊ������V�����X�^�b�N���쐬���܂��B
		/// </summary>
		/// <typeparam name="T">�ϊ��O�̗v�f�̌^���w�肵�܂��B</typeparam>
		/// <typeparam name="U">�ϊ���̗v�f�̌^���w�肵�܂��B</typeparam>
		/// <param name="stack">�ϊ��O�̗v�f���i�[�����X�^�b�N���w�肵�܂��B</param>
		/// <param name="converter">�v�f�̕ϊ��Ɏg�p����f���Q�[�g���w�肵�܂��B</param>
		/// <returns>�v�f��ϊ����ďo�����X�^�b�N��Ԃ��܂��B</returns>
		public static Gen::Stack<U> ConvertAll<T,U>(this Gen::Stack<T> stack,Converter<T,U> converter){
			T[] values=stack.ToArray();

			Gen::Stack<U> ret=new Gen::Stack<U>(values.Length);
			for(int i=values.Length-1;i>=0;i--)
				ret.Push(converter(values[i]));
			return ret;
		}
#if !HAS_LINQ
		/// <summary>
		/// �񋓎q�����ԍŏ��̗v�f���擾���܂��B
		/// </summary>
		/// <typeparam name="T">�񋓂����I�u�W�F�N�g�̌^���w�肵�܂��B</typeparam>
		/// <param name="enumerable">�񋓎q���w�肵�܂��B</param>
		/// <returns>��ԍŏ��̗v�f��Ԃ��܂��B�����A�v�f������܂܂�Ă��Ȃ��ꍇ�ɂ́A�^ T �̊���l��Ԃ��܂��B</returns>
		public static T First<T>(this Gen::IEnumerable<T> enumerable){
			T ret=default(T);
			foreach(T value in enumerable){ret=value;break;}
			return ret;
		}
#endif
	}
}