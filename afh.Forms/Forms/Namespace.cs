using Gdi=System.Drawing;
using Diag=System.Diagnostics;

namespace afh.Forms{
	internal static class _resource{
		// 茲は、他のプロジェクトに参加させる際には変更しなければならない
		public const string DEFAULT_NAMESPACE="afh";
		private static readonly System.Reflection.Assembly assembly=typeof(_resource).Assembly;
		public static Gdi::Image TreeIcons{
			get{
				if(treeIcons==null){
					const string name=DEFAULT_NAMESPACE+".Forms.TreeIcons.png";
					treeIcons=Gdi::Image.FromStream(
						assembly.GetManifestResourceStream(name));
					if(treeIcons is Gdi::Bitmap){
						afh.Drawing.BitmapEffect.ReplaceColor(
							treeIcons as Gdi::Bitmap,
							Gdi::Color.Magenta,Gdi::Color.Transparent
							);
					}
				}
				return treeIcons;
			}
		}
		private static Gdi::Image treeIcons=null;
	}
	//================================================================
	//		実装用
	//================================================================
	internal static class _todo{
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void TreeNodeOptimize(params object[] x){}

		[System.Obsolete]
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void EssentialTreeView(string msg){}
		[System.Obsolete]
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void EssentialTreeView(){}

		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void ExamineTreeView(){}
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void ExamineTreeView(string msg){}

		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void TreeViewMultiSelect(params object[] x){}

		/*
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void TreeNode_Visible(){}
		[Diag::Conditional("CALL_TODO_SPEC")]
		public static void TreeNode_Visible(string msg){}
		//*/

		/*
		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNode_Width(){}
		//*/

		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNode(){}
		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNode(string p){}

		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNodeCheck(params object[] x) { }

		/// <summary>
		/// RefreshDisplaySize を呼び出して false が帰った場合には、
		/// 描画は呼び出し元で適宜行う事。
		/// </summary>
		/// <param name="p"></param>
		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNodeDisplayHeight(string p) {
			throw new System.Exception("The method or operation is not implemented.");
		}

		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void ExperimentTreeView(params object[] x){
			throw new System.Exception("The method or operation is not implemented.");
		}

		[System.Diagnostics.Conditional("DEBUG")]
		[System.Diagnostics.DebuggerHidden]
		public static void TreeViewAssert(bool cond,params string[] messages){
			afh.DebugUtils.Assert(cond,messages);
		}


		[Diag::Conditional("CALL_TODO_SPEC")]
		internal static void TreeNodeSelectionChanged(params object[] x) {
			throw new System.NotImplementedException();
		}
	}
}
