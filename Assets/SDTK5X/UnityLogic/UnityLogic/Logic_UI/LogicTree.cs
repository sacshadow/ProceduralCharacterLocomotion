using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public abstract class LogicTree : LogicDraw {
		public static float indent = 20;
		public static float xSpace = 6, ySpace = 2;
		public static float xBorder = 4, yBorder = 4;
		
		public static Color borderColor = new Color(1,1,1,0.15f);
	
		public List<LogicDraw> child;
		
		public static LogicDraw SInsertPAtIndex(LogicTree tree, int index) {
			var temp = new Label("Subtopic " + tree.child.Count);
			temp.parent = tree;
			tree.child.Insert(index+1, temp);
			tree.OnChildSizeChange();
			return temp;
		}
		
		public override void OnDrawGUI(Vector2 parentOffset) {
			var lineColor = isSelect ? select : (isHighlight ? hover : normal);
		
			DrawBorder(parentOffset, lineColor);
			child.ForEach(x=>x.OnDrawGUI(parentOffset + offset));
			
			if(isHighlight && insertIndex != -1)
				DrawInsert(parentOffset);
			
			if(child.Count > 1)
				DrawLines(Color.black, GetFormatLines(parentOffset));
		}
		
		public abstract Vector2[] GetFormatLines(Vector2 parentOffset);
		
		public abstract void CalcuChildSize();
		
		public override void OnChildSizeChange() {
			CalcuChildSize();
			parent.OnChildSizeChange();
		}
		
		public override void OnChildChanged(LogicDraw oldChild, LogicDraw newChild) {
			var index = FindChild(oldChild);
			child[index] = newChild;
			newChild.parent = this;
			OnChildSizeChange();
		}
		
		public override LogicDraw InsertParallel(LogicDraw selected) {
			int index = FindChild(selected);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
			
			return InsertPAtIndex(index);
		}
		
		public override LispObject OnCopy(LogicDraw selected) {
			int index = FindChild(selected);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
				
			if(index == 0)
				return parent.OnCopy(this);
				
			return selected.ToLO();
		}
		
		public override LispObject ToLO() {
			return new ULList(child.Select(x=>x.ToLO()).ToList());
		}
		
		public virtual LogicDraw InsertPAtIndex(int index) {
			return SInsertPAtIndex(this,index);
		}
		
		public override LogicDraw Insert(List<LogicDraw> attached) {
			return InsertChild(insertIndex,attached);
		}
		
		public virtual LogicDraw InsertChild(int index, List<LogicDraw> attached) {
			var insertPos = Mathf.Clamp(index+1,0,child.Count);
			
			//TODO: muilt select insert
			// foreach(var temp in newChild) {
				// temp.parent = this;
				// child.Insert(insertPos, temp);
				// insertPos++;
			// }
			
			LogicDraw temp = LogicUITool.GetNodeAttached(attached);
			temp.parent = this;
			child.Insert(insertPos, temp);
			
			OnChildSizeChange();
			return temp;
		}
		
		// public override LogicDraw Insert(Vector2 pos, Node attached) {
			// if(!IsInsideArea(pos))
				// return null;
		
			// if(pos.y < yBorder)
				// return null;
			
			// for(int i = 0; i<child.Count; i++) {
				// var temp = child[i].Insert(pos-child[i].offset, attached);
				// if(temp != null) {
					// return temp;
				// }
			// }
			
			// return InsertChild(ClampIndex(pos), attached.child);
		// }
		
		public override LogicDraw TryInsert(Vector2 pos) {
			if(!IsInsideArea(pos))
				return null;
		
			if(pos.y < yBorder)
				return null;
			
			insertIndex = -1;
			
			for(int i = 0; i<child.Count; i++) {
				var temp = child[i].TryInsert(pos-child[i].offset);
				if(temp != null)
					return temp;
			}
			
			insertIndex = ClampIndex(pos);
			
			return this;
		}
		
		public abstract int ClampIndex(Vector2 pos);
		
		public override LogicDraw OnChildDetach(LogicDraw detachChild) {
			int index = FindChild(detachChild);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
			
			if(AutoSelectTree(index))
				return parent.OnChildDetach(this);
			
			child.RemoveAt(index);
			OnChildSizeChange();
			return detachChild;
		}
		
		public override LogicDraw OnChildNavigation(KeyCode key, LogicDraw ld) {
			int index = FindChild(ld);
			
			if(index == 0)
				return SelfNav(key);
			else
				return ChildNav(index, key);
		}
		
		public virtual LogicDraw SelfNav(KeyCode key) {
			if(key == KeyCode.RightArrow)
				return NavToChild();
				
			else 
				return parent.OnChildNavigation(key, this);
		}
		
		public virtual LogicDraw ChildNav(int index, KeyCode key) {
			if(key == KeyCode.LeftArrow)
				return child[0].OnNav();
			if(key == KeyCode.RightArrow)
				return ChildNavChild(child[index]);
			if(key == KeyCode.UpArrow)
				return NavChild(index - 1);
			if(key == KeyCode.DownArrow)
				return NavChild(index + 1);
			return this;
		}
		
		public virtual LogicDraw ChildNavChild(LogicDraw ld) {
			if(ld is LogicTree)
				return ((LogicTree)ld).SelfNav(KeyCode.RightArrow);
			else return ld;
		}
		
		public override LogicDraw OnNav() {
			if(child.Count > 0)
				return child[0].OnNav();
			return this;
		}
		
		public virtual LogicDraw NavToChild() {
			if(child.Count > 1)
				return child[1].OnNav();
			return OnNav();
		}
		
		// public virtual LogicDraw NavToParent() {
			// return parent.OnChildNavigation(KeyCode.UpArrow, this);
		// }
		
		public virtual LogicDraw NavChild(int index) {
			if(index < 0)
				return parent.OnChildNavigation(KeyCode.UpArrow, this);
			if(index >= child.Count)
				return parent.OnChildNavigation(KeyCode.DownArrow, this);
			
			return child[index].OnNav();
		}
		
		public virtual bool AutoSelectTree(int index) {
			return index == 0;
		}
		
		public int FindChild(LogicDraw ld) {
			for(int i=0; i<child.Count; i++) {
				if(child[i] == ld)
					return i;
			}
			
			return -1;
		}
		
	}
}
