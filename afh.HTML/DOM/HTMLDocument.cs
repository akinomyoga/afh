using Gen=System.Collections.Generic;

namespace afh.HTML{
	public partial class HTMLDocument:HTMLElement{
		readonly Gen::List<HTMLError> errors=new Gen::List<HTMLError>();
		public Gen::List<HTMLError> ErrorList{
			get{return this.errors;}
		}

		public HTMLDocument():base(null,"#document"){}
		public override nodeType nodeType{
			get{return nodeType.DOCUMENT_NODE;}
		}

		public static HTMLDocument Parse(string text){
			return HTMLParser.Parse(text);
		}
		static HTMLDocument(){
			initializeEntities();
			__dll__.log.WriteLine("afh::HTML::HTMLDocument 初期化終了");
		}
	}

	public enum HTMLErrorType{
		ParseError,
		Warning,
		Error,
	}
	public sealed class HTMLError{
		public string sourceName;
		public int start;
		public int end;

		public HTMLErrorType type;
		public string message;
	}

}