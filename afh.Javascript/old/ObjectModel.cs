namespace afh.Javascript{
	public class Object{
		public Javascript.Object GetValue(int index,string[] names){
			if(index+1==names.Length){
				//--末端に来た時
				Javascript.Object o=this[names[index]];
				//CHECK>OK: o が null の時にどうなるか
				if(o is Javascript.PropertyBase){
					//--Property の場合
					return ((Javascript.PropertyBase)o).GetValue();
				}else if(this is Javascript.ManagedObject&&o is Javascript.ManagedMethod){
					//--ManagedMethod の場合
					return new Javascript.ManagedDelegate(
						((Javascript.ManagedObject)this).Value,
						(Javascript.ManagedMethod)o
					);
				}else return o;
			}else{
				if(this is Javascript.PropertyBase){
					Javascript.Object this2=((Javascript.PropertyBase)this).GetValue();
					return this2[names[index]].GetValue(index+1,names);
				}else return this[names[index]].GetValue(index+1,names);
			}
		}
		public void SetValue(Javascript.Object obj,int index,string[] names){
			if(index+1==names.Length){
				if(this[names[index]] is Javascript.PropertyBase){
					//--Property の場合
					((Javascript.PropertyBase)this[names[index]]).SetValue(obj);
				}else this[names[index]]=obj;
			}else{
				if(this is Javascript.PropertyBase){
					Javascript.Object this2=((Javascript.PropertyBase)this).GetValue();
					this2[names[index]].SetValue(obj,index+1,names);
				}else this[names[index]].SetValue(obj,index+1,names);
			}
		}
		public Javascript.Object InvokeMember(int index,string[] names,Javascript.Array arguments){
			if(index+1==names.Length){
				try{
					return ((FunctionBase)this[names[index]]).Invoke(this,arguments);
				}catch(System.NullReferenceException e){
					//※: 一回目の NullReferenceException は時間がかかる
					throw new Null.UndefinedException(e);
				}catch(System.InvalidCastException e){
					throw new System.Exception("指定したオブジェクトは関数ではありません",e);
				}catch(System.Exception e){throw e;}
			}else{
				return this[names[index]].InvokeMember(index+1,names,arguments);
			}
		}
		private class ObjectEnumerator:IEnumerator{
			private const string OUTOFRANGE="列挙子が、コレクションの最初の要素の前、または最後の要素の後に位置しています。";
			private System.Collections.Hashtable hash;
			private System.Collections.IEnumerator ie;
			private object current=null;
			public object Current{
				get{
					if(this.current==null)throw new System.InvalidOperationException(OUTOFRANGE);
					return this.current;
				}
			}
			public ObjectEnumerator(Javascript.Object o){
				this.hash=o.members;
				this.ie=this.hash.Keys.GetEnumerator();
			}
			public void Reset(){
				this.ie.Reset();
				this.current=null;
			}
			public bool MoveNext(){
				do{
					if(!this.ie.MoveNext()){
						this.current=null;
						return false;
					}
					this.current=this.o.members[ie.Current];
					System.Type t=this.current.GetType();
				}while(
					t.IsSubclassOf(typeof(Javascript.ManagedMethod))
					||t.IsSubclassOf(typeof(Javascript.ManagedProperty))
				);
				return true;
			}
		}
	}
	public class ManagedProperty:PropertyBase{
		public override Object GetValue(Javascript.Object parent){
			if(!(parent is Javascript.ManagedObject))throw new System.ArgumentException(NOTSUPPORTED,"parent");
			object o=((Javascript.ManagedObject)parent).Value;
			if(!this.IsCastableFrom(o.GetType()))throw new System.ArgumentException(NOTSUPPORTED,"parent");
			//TODO:書きかけ
			return null;
		}
		public override void SetValue(Javascript.Object parent,Javascript.Object value){
			if(!(parent is Javascript.ManagedObject))throw new System.ArgumentException(NOTSUPPORTED,"parent");
			object o=((Javascript.ManagedObject)parent).Value;
			if(!this.IsCastableFrom(o.GetType()))throw new System.ArgumentException(NOTSUPPORTED,"parent");
			//TODO:書きかけ
			return;
		}
		private bool IsCastableFrom(System.Type t){
			return t.IsSubclassOf(this.type)||t.GetInterface(this.type.FullName)!=null;
		}
	}
}