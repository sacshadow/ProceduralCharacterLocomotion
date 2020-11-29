using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public class VerticalTree : LogicTree {
		
		public VerticalTree() {}
		
		public VerticalTree(List<LogicDraw> ld) {
			child = ld;
			ld.ForEach(x=>x.parent = this);
			CalcuChildSize();
		}
		
		public VerticalTree(LispObject data) {
			child = LogicUITool.GetChildVertical((ULList)data, this);
			CalcuChildSize();
		}
		
		public override Vector2[] GetFormatLines(Vector2 parentOffset) {
			var rt = new Vector2[2 * child.Count];
			var xPos = indent/2f;
			
			rt[0] = new Vector2(xPos, child[0].offset.y + child[0].height) + parentOffset;
			rt[1] = new Vector2(xPos, child[child.Count-1].offset.y + child[child.Count-1].joinHeight) + parentOffset;
			
			Loop.Between(1,child.Count).Do(x=>{
				rt[x*2] = new Vector2(xPos, child[x].offset.y + child[x].joinHeight) + parentOffset;
				rt[x*2+1] = new Vector2(indent + parentOffset.x, rt[x*2].y);
			});
			
			return rt;
		}
		
		public override LogicDraw Select(Vector2 pos) {
			if(!IsInsideArea(pos))
				return null;
			
			int index = TryChildHeight(pos.y);
			if(index == -1)
				return null;
			return child[index].Select(pos - child[index].offset);
		}
		
		public override LogicDraw InsertPAtIndex(int index) {
			if(index == 0)
				return parent.InsertParallel(this);
				
			return base.InsertPAtIndex(index);
		}
		
		public int TryChildHeight(float y) {
			for(int i = 0; i< child.Count; i++) {
				if(y<child[i].offset.y + child[i].height)
					return i;
			}
			return -1;
		}
		
		public override int ClampIndex(Vector2 pos) {
			var rt = 0;
			
			if(child.Count > 1) {
				int index = TryChildHeight(pos.y);
			
				if(index == -1) index = child.Count -1;
				if(index == 0) index = 1;
				
				rt = (pos.y > child[index].offset.y + child[index].joinHeight) ? 
					index : index -1;
			}
			
			return rt;
		}
		
		public override LogicDraw OnChildDetach(LogicDraw detachChild) {
			var rt = base.OnChildDetach(detachChild);
			
			if(rt == this)
				return rt;
			
			if(child.Count == 1 && child[0] is Label && GetType() == typeof(VerticalTree))
				parent.OnChildChanged(this, child[0]);
			
			return rt;
		}
		
		public override void DrawInsert(Vector2 pos) {
			var insertHeight = child[insertIndex].offset.y + child[insertIndex].height + ySpace/2f;
			
			var pl = new Vector2(indent - 10 , insertHeight) + pos;
			var pr = new Vector2(width + 10, insertHeight) + pos;
			
			DrawLines(Color.black, pl, pr);
		}
		
		public override void CalcuChildSize() {
			Vector2 borderOffset = new Vector2(xBorder/2f, yBorder/2f);
			var size = child[0].GetSize();
			child[0].offset = borderOffset;
			
			Loop.Between(1,child.Count).Do(x=> {
				size.y += ySpace;
				
				child[x].SetOffset(indent + borderOffset.x, size.y + borderOffset.y);
			
				size.x = Mathf.Max(size.x, indent + child[x].width);
				size.y += child[x].height;
			});
			
			size.x += xBorder;
			size.y += yBorder;
			SetSize(size.x, size.y);
			
			joinHeight = child[0].offset.x + child[0].joinHeight;
		}
		
		
		
	}
}
