using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public class HorizontalTree : LogicTree {
		
		public HorizontalTree() {}
		
		public HorizontalTree (LispObject data) {
			child = LogicUITool.GetChildHorizontal((ULList)data, this);
			CalcuChildSize();
		}
		
		// public override void OnDrawGUI(Vector2 parentOffset) {
			// DrawBorder(parentOffset, Color.black);
			// child.ForEach(x=>x.OnDrawGUI(parentOffset + offset));
		// }
		
		public override Vector2[] GetFormatLines(Vector2 parentOffset) {
			return new Vector2[0];
		}
		
		public override LogicDraw Select(Vector2 pos) {
			if(!IsInsideArea(pos))
				return null;
			
			int index = TryChildWidth(pos.x);
			if(index == -1)
				return null;
			
			return child[index].Select(pos - child[index].offset);
		}
		
		public int TryChildWidth(float width) {
			for(int i=0; i<child.Count; i++) {
				if(width < child[i].offset.x + child[i].width)
					return i;
			}
			
			return -1;
		}
		
		public override int ClampIndex(Vector2 pos) {
			return 0;
		}
		
		public override LogicDraw OnChildDetach(LogicDraw detachChild) {
			var rt = base.OnChildDetach(detachChild);
			
			if(rt == this)
				return rt;
			
			if(child.Count == 1 && child[0] is Label && GetType() == typeof(HorizontalTree))
				parent.OnChildChanged(this, child[0]);
			
			return rt;
		}
		
		public override void CalcuChildSize() {
			Vector2 borderOffset = new Vector2(xBorder/2f, yBorder/2f);
			var size = child[0].GetSize();
			child[0].offset = borderOffset;
			
			Loop.Between(1,child.Count).Do(x=> {
				size.x += xSpace;
				child[x].SetOffset(size.x + borderOffset.x, borderOffset.y);
				
				size.x += child[x].width;
				size.y = Mathf.Max(size.y, child[x].height);
			});
			
			size.x += xBorder;
			size.y += yBorder;
			
			SetSize(size.x, size.y);
			
			joinHeight = child[0].offset.x + child[0].joinHeight;
		}
		
	}
}
