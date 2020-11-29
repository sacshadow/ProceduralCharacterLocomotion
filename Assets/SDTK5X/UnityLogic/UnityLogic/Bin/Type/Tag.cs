using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
namespace UnityLogic {
	public class Tag : LogicObject {
		
		public Node titleNode;
		public List<Node> node;
		
		public Tag(string title) {
			titleNode = new Node(title);
			node = new List<Node>();
		}
		
		public void Attach(Node newNode) {
			newNode.parent = this;
			node.Add(newNode);
		}
		
		public override LogicObject GetMouseOver(Vector2 mousePosition) {
			var rt = titleNode.GetMouseOver(mousePosition);
			if(rt != null)
				return rt;
				
			foreach(var n in node) {
				rt = n.GetMouseOver(mousePosition);
				if(rt != null)
				return rt;
			}
			return null;
		}
		
	}
}
