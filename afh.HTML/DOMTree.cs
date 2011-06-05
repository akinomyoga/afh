using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Gdi=System.Drawing;

namespace afh.HTML{
	public partial class DOMTree:TreeView{
		public DOMTree() {
			InitializeComponent();
		}
		public void Add(HTMLDocument htm){
			this.Nodes.Add(new ElementTreeNode(htm));
		}
		private sealed class ElementTreeNode:TreeNode{
			public HTMLElement element;
			public ElementTreeNode(HTMLElement elem):base("<"+elem.tagName+elem._attributes.ToString()+">"){
				this.element=elem;
				this.Nodes.Add("dummy");
			}
			public void BeforeExpand(){
				this.Nodes.Clear();
				foreach(HTMLNode node in this.element._childNodes){
					if(node is HTMLElement){
						this.Nodes.Add(new ElementTreeNode((HTMLElement)node));
					}else if(node is HTMLTextNode){
						this.Nodes.Add("#text "+((HTMLTextNode)node).data);
					}
				}
			}
		}

		private void DOMViewer_BeforeExpand(object sender,TreeViewCancelEventArgs e) {
			if(e.Node!=null&&e.Node is ElementTreeNode){
				((ElementTreeNode)e.Node).BeforeExpand();
			}
		}
	}
}
