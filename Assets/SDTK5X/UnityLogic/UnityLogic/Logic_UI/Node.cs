using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public class Node : VerticalTree {
		
		public float x, y;
		public Tag tag;
		
		public Node(Vector2 position, List<LogicDraw> child, Tag tag) {
			this.tag = tag;
			this.child = child;
			child.ForEach(x=>x.parent = this);
			CalcuChildSize();
			
			x = Mathf.Round(position.x / canvasScale - 10);
			y = Mathf.Round(position.y / canvasScale - 10);
		}
		
		public Node(Vector2 position, LispObject data, Tag tag) {
			this.tag = tag;
			child = LogicUITool.GetChildVertical((ULList)data, this);
			CalcuChildSize();
			
			x = Mathf.Round(position.x / canvasScale - width/2f);
			y = Mathf.Round(position.y / canvasScale - height/2f);
		}
		
		public Node(LispObject data, Tag tag) {
			this.tag = tag;
			var list = (ULList) data;
			x = ((Integer)list[1]).GetValue();
			y = ((Integer)list[2]).GetValue();
			
			child = LogicUITool.GetChildVertical((ULList)list[3], this);
			CalcuChildSize();
		}
		
		public override LogicDraw OnChildDetach(LogicDraw detachChild) {
			int index = FindChild(detachChild);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
			
			if(index == 0)
				return this;
			
			child.RemoveAt(index);
			OnChildSizeChange();
			return detachChild;
		}
		
		public override LispObject OnCopy(LogicDraw selected) {
			int index = FindChild(selected);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
				
			if(index == 0)
				return base.ToLO();
				
			return selected.ToLO();
		}
		
		public override LogicDraw NavChild(int index) {
			if(index < 0 || index >= child.Count)
				return null;
			
			return child[index].OnNav();
		}
		
		public override LogicDraw SelfNav(KeyCode key) {
			if(key == KeyCode.RightArrow)
				return NavToChild();
			
			return OnNav();
		}
		
		public void DragNode(Vector2 position) {
			x = Mathf.Round(position.x / canvasScale - 10);
			y = Mathf.Round(position.y / canvasScale - 10);
		}
		
		
		public override void OnDrawGUI() {
			GUILayout.BeginArea(new Rect(x * canvasScale, y * canvasScale, width * canvasScale, height * canvasScale));
				base.OnDrawGUI();
				if(!isHighlight)
					DrawBorder(Vector2.zero, Color.red);
			GUILayout.EndArea();
		}
		
		public override LogicDraw Select(Vector2 pos) {
			return base.Select(pos - new Vector2(x,y));
		}
		
		public override LispObject ToLO() {
			return new ULList(new Symbol("node"), new Integer((long)x), new Integer((long)y), base.ToLO());
		}
		
		// public override LogicDraw Insert(Vector2 pos, Node attached) {
			// return base.Insert(pos - new Vector2(x,y), attached);
		// }
		
		public override LogicDraw TryInsert(Vector2 pos) {
			return base.TryInsert(pos - new Vector2(x,y));
		}
		
		public override void OnChildSizeChange() {
			CalcuChildSize();
			tag.OnNodeSizeChange();
		}
	}
}
