
namespace afh.JavaScript.Parse{
	public partial class JavascriptParser{
		afh.Parse.AbstractWordReader wreader;
		System.Collections.Stack stack;
		readonly object OpenMarker=new object();

		
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_main(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E1();
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E1(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E2();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="=")||(word=="|=")||(word=="^=")||(word=="&=")||(word=="<<=")||(word==">>=")||(word==">>>=")||(word=="+=")||(word=="-=")||(word=="*=")||(word=="/=")||(word=="%=")){
				this.wreader.ReadNext();
				this.ReadContext_E1();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else if((word=="?")){
				this.wreader.ReadNext();
				this.ReadContext_三項();
				this.stack.Push(new TripleOperator((IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				return;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_三項(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E1();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==":")){
				this.wreader.ReadNext();
				this.ReadContext_E1();
			}else {
				this.wreader.LetterReader.SetError("エラー: \"'?' に対応する ':' が必要です\"",0,null);
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E2(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E3();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="||")){
				this.wreader.ReadNext();
				this.ReadContext_E3();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E3(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E4();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="^^")){
				this.wreader.ReadNext();
				this.ReadContext_E4();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E4(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E5();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="&&")){
				this.wreader.ReadNext();
				this.ReadContext_E5();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E5(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E6();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="|")){
				this.wreader.ReadNext();
				this.ReadContext_E6();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E6(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E7();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="^")){
				this.wreader.ReadNext();
				this.ReadContext_E7();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E7(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E8();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="&")){
				this.wreader.ReadNext();
				this.ReadContext_E8();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E8(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_E9();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="==")||(word=="!=")){
				this.wreader.ReadNext();
				this.ReadContext_E9();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_E9(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_Ea();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="<")||(word==">")||(word=="<=")||(word==">=")||(word=="instanceof")||(word=="in")){
				this.wreader.ReadNext();
				this.ReadContext_Ea();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Ea(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_Eb();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="<<")||(word==">>")||(word==">>>")){
				this.wreader.ReadNext();
				this.ReadContext_Eb();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Eb(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_Ec();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="+")||(word=="-")){
				this.wreader.ReadNext();
				this.ReadContext_Ec();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Ec(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_Ed();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="*")||(word=="/")||(word=="%")){
				this.wreader.ReadNext();
				this.ReadContext_Ed();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Ed(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="+")||(word=="-")||(word=="++")||(word=="--")||(word=="*")||(word=="&")){
				this.wreader.ReadNext();
				this.ReadContext_Ed();
				this.stack.Push(new UnaryOperator(word,(IScriptNode)this.stack.Pop(),false));
			}else if((word=="var")){
				this.wreader.ReadNext();
				this.ReadContext_var();
				this.stack.Push(new UnaryOperator(word,(IScriptNode)this.stack.Pop(),false));
			}else if((word=="/")){
				this.wreader.ReadNext();
				this.stack.Push(new Word(word));
				this.wreader.LetterReader.SetError("解析中の不明なエラー",0,null);
			}else if((this.wreader.CurrentType.value==0)){
				return;
			}else {
				this.ReadContext_Ee();
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Ee(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.ReadContext_Ef();
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="(")){
				this.wreader.ReadNext();
				this.ReadContext_引数リスト();
				this.stack.Push(new FunctionCall((IScriptNode[])this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				this.ReadContext_Eg();
				goto label_1;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Ef(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="new")){
				this.wreader.ReadNext();
				this.ReadContext_Ef();
				this.stack.Push(new UnaryOperator(word,(IScriptNode)this.stack.Pop(),false));
				goto label_1;
			}else {
				this.ReadContext_核();
				this.ReadContext_Eg();
				return;
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="(")){
				this.wreader.ReadNext();
				this.ReadContext_引数リスト();
				this.stack.Push(new FunctionCall((IScriptNode[])this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				this.ReadContext_Eg();
				return;
			}else {
				this.wreader.LetterReader.SetError("エラー: \"new の後に () が無いと困ります\"",0,null);
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_Eg(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="++")||(word=="--")){
				this.wreader.ReadNext();
				this.stack.Push(new UnaryOperator(word,(IScriptNode)this.stack.Pop(),true));
				goto label_0;
			}else if((word==".")||(word=="->")){
				this.wreader.ReadNext();
				this.ReadContext_nmtk();
				this.stack.Push(new BinaryOperator(word,(IScriptNode)this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				goto label_0;
			}else if((word=="[")){
				this.wreader.ReadNext();
				this.ReadContext_引数リスト角();
				this.stack.Push(new MemberInvoke((IScriptNode[])this.stack.Pop(),(IScriptNode)this.stack.Pop()));
				this.ReadContext_Eg();
				goto label_0;
			}else {
				return;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_核(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word=="(")){
				this.wreader.ReadNext();
				this.ReadContext_E1();
				goto label_1;
			}else if((this.wreader.CurrentType.value==2)||(this.wreader.CurrentType.value==4)){
				this.wreader.ReadNext();
				this.stack.Push(new Word(word));
				return;
			}else {
				this.wreader.LetterReader.SetError("解析中の不明なエラー",0,null);
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")){
				this.wreader.ReadNext();
			}else {
				this.wreader.LetterReader.SetError("エラー: \"'(' に対応する ')' が見つからないです。\"",0,null);
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_nmtk(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((this.wreader.CurrentType.value==2)||(this.wreader.CurrentType.value==4)){
				this.wreader.ReadNext();
				this.stack.Push(new Word(word));
			}else if((word=="def")){
				this.wreader.LetterReader.SetError("解析中の不明なエラー",0,null);
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_引数リスト(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.stack.Push(this.OpenMarker);
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")){
				this.wreader.ReadNext();
				System.Collections.ArrayList list=new System.Collections.ArrayList();
				while(this.stack.Count>0){
					object v=this.stack.Pop();
					if(v==this.OpenMarker)break;
					list.Add(this.stack.Pop());
				}
				list.Reverse();
				this.stack.Push(list.ToArray());
				return;
			}else {
				this.ReadContext_E1();
			}
		#pragma warning disable 164
		label_2:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")){
				this.wreader.ReadNext();
				System.Collections.ArrayList list=new System.Collections.ArrayList();
				while(this.stack.Count>0){
					object v=this.stack.Pop();
					if(v==this.OpenMarker)break;
					list.Add(this.stack.Pop());
				}
				list.Reverse();
				this.stack.Push(list.ToArray());
				return;
			}else if((word==",")){
				this.wreader.ReadNext();
			}else {
				this.wreader.LetterReader.SetError("解析中の不明なエラー",0,null);
			}
		#pragma warning disable 164
		label_3:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")||(word==",")){
				this.wreader.LetterReader.SetError("エラー: \"',' に続く引数がありません。\"",0,null);
			}else {
				this.ReadContext_E1();
				goto label_2;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_引数リスト角(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			{
				this.stack.Push(this.OpenMarker);
			}
		#pragma warning disable 164
		label_1:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")){
				this.wreader.ReadNext();
				System.Collections.ArrayList list=new System.Collections.ArrayList();
				while(this.stack.Count>0){
					object v=this.stack.Pop();
					if(v==this.OpenMarker)break;
					list.Add(this.stack.Pop());
				}
				list.Reverse();
				this.stack.Push(list.ToArray());
				return;
			}else {
				this.ReadContext_E1();
			}
		#pragma warning disable 164
		label_2:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")){
				this.wreader.ReadNext();
				System.Collections.ArrayList list=new System.Collections.ArrayList();
				while(this.stack.Count>0){
					object v=this.stack.Pop();
					if(v==this.OpenMarker)break;
					list.Add(this.stack.Pop());
				}
				list.Reverse();
				this.stack.Push(list.ToArray());
				return;
			}else if((word==",")){
				this.wreader.ReadNext();
			}else {
				this.wreader.LetterReader.SetError("解析中の不明なエラー",0,null);
			}
		#pragma warning disable 164
		label_3:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((word==")")||(word==",")){
				this.wreader.LetterReader.SetError("エラー: \"',' に続く引数がありません。\"",0,null);
			}else {
				this.ReadContext_E1();
				goto label_2;
			}
		}
		[System.Runtime.CompilerServices.CompilerGenerated]
		private void ReadContext_var(){
			string word;
		#pragma warning disable 164
		label_0:
		#pragma warning restore 164
			word=this.wreader.CurrentWord;
			if((this.wreader.CurrentType.value==2)||(this.wreader.CurrentType.value==4)){
				this.stack.Push(new Word(word));
				this.wreader.ReadNext();
			}else {
				this.wreader.LetterReader.SetError("エラー: \"var の次には識別子が来る必要があります。\"",0,null);
			}
		}
	}
}
