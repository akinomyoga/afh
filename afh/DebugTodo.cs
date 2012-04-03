using Diag=System.Diagnostics;

internal static class __debug__ {
	const string COND_REGEXPARSER="DEBUG";

	[Diag::Conditional(COND_REGEXPARSER)]
	public static void RegexParserAssert(bool condition,string msg,string detailedMsg){
		Diag::Debug.Assert(condition,msg,detailedMsg);
	}
	[Diag::Conditional(COND_REGEXPARSER)]
	public static void RegexParserAssert(bool condition,string msg){
		Diag::Debug.Assert(condition,msg);
	}
	[Diag::Conditional(COND_REGEXPARSER)]
	public static void RegexParserAssert(bool condition){
		Diag::Debug.Assert(condition);
	}

	[Diag::Conditional(COND_REGEXPARSER)]
	public static void RegexParserToDo(string message){
	}

	[Diag::Conditional(COND_REGEXPARSER)]
	internal static void RegexTestAssert(bool condition){
		Diag::Debug.Assert(condition);
	}
	[System.Obsolete]
	internal static void RegexTestNotImplemented(params object[] ooo){
		throw new System.Exception("The method or operation is not implemented.");
	}
	[System.Obsolete]
	internal static void RegexTestToDo(params object[] ooo){}
}