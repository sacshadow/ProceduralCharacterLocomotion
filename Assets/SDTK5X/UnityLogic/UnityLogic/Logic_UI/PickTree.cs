using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public class PickTree : VerticalTree {
		public static int defaultHeight = 28;
		public static Texture2D bracketsOn, bracketsOff;
	
		public PickTree() {
			if(bracketsOn == null)
				bracketsOn = Resources.Load<Texture2D>("bracketsOn");
			if(bracketsOff == null)
				bracketsOff = Resources.Load<Texture2D>("bracketsOff");
		
			child = new List<LogicDraw>();
			CalcuChildSize();
		}
		
		public PickTree(LispObject data) {
			if(bracketsOn == null)
				bracketsOn = Resources.Load<Texture2D>("bracketsOn");
			if(bracketsOff == null)
				bracketsOff = Resources.Load<Texture2D>("bracketsOff");
			
			child = LogicUITool.GetChildPick((ULList)data, this);
			CalcuChildSize();
		}
		
		public override LogicDraw Select(Vector2 pos) {
			if(!IsInsideArea(pos))
				return null;
			
			int index = TryChildHeight(pos.y);
	
			if(index == -1)
				return this;
			
			var childSelect = child[index].Select(pos - child[index].offset);
			
			return childSelect != null ? childSelect : this;
		}
		
		public override LispObject OnCopy(LogicDraw selected) {
			int index = FindChild(selected);
			if(index == -1)
				throw new Exception("Child not belong to this tree " + GetType());
				
			return selected.ToLO();
		}
		
		public override bool AutoSelectTree(int index) {
			return false;
		}
		
		public override LispObject ToLO() {
			return new ULVector(child.Select(x=>x.ToLO()).ToList());
		}
		
		public override void OnDrawGUI(Vector2 parentOffset) {
			var lineColor = isSelect ? select : (isHighlight ? hover : normal);
			
			DrawBorder(parentOffset, lineColor);
			child.ForEach(x=>x.OnDrawGUI(parentOffset + offset));
			
			var r = new Rect(0,0,0,0);
			r.x = (parentOffset.x + offset.x) * canvasScale;
			r.y = (parentOffset.y + offset.y) * canvasScale;
			r.width = indent * canvasScale;
			r.height = height * canvasScale;
			GUI.DrawTexture(new Rect(r), bracketsOn);
			
			r.x = (parentOffset.x + offset.x + width - indent) * canvasScale;
			GUI.DrawTexture(new Rect(r), bracketsOff);
			
			if(isHighlight && insertIndex != -1)
				DrawInsert(parentOffset);
		}
		
		public override LogicDraw InsertPAtIndex(int index) {
			return SInsertPAtIndex(this,index);
		}
		
		public override int ClampIndex(Vector2 pos) {
			if(child.Count == 0)
				return -2;
			
			var rt = 0;
			int index = TryChildHeight(pos.y);
			
			if(index == -1) index = child.Count -1;
			
			rt = (pos.y > child[index].offset.y + child[index].joinHeight) ? 
				index : index -1;
			
			if(rt == -1)
				return -2;
			else
				return rt;
		}
		
		public override void DrawInsert(Vector2 pos) {
			var insertHeight = ySpace/2f;
		
			if(insertIndex != -2)
				insertHeight += child[insertIndex].offset.y + child[insertIndex].height;
			else if(child.Count == 0)
				insertHeight = defaultHeight/2f;
			
			var pl = new Vector2(15 , insertHeight) + pos;
			var pr = new Vector2(width - 15, insertHeight) + pos;
			
			DrawLines(Color.black, pl, pr);
		}
		
		public override Vector2[] GetFormatLines(Vector2 parentOffset) {
			return new Vector2[0];
		}
		
		public override void CalcuChildSize() {
			if(child.Count == 0) {
				SetSize(indent*2 + 20, defaultHeight);
				joinHeight = defaultHeight/2f;
				return;
			}
			
			Vector2 borderOffset = new Vector2(xBorder/2f + indent, yBorder/2f);
			var size = child[0].GetSize();
			child[0].offset = borderOffset;
			
			Loop.Between(1,child.Count).Do(x=> {
				size.y += ySpace;
				
				child[x].SetOffset(borderOffset.x, size.y + borderOffset.y);
			
				size.x = Mathf.Max(size.x, child[x].width);
				size.y += child[x].height;
			});
			
			size.x += xBorder + indent*2;
			size.y += yBorder;
			SetSize(size.x, size.y);
			
			joinHeight = size.y/2f;
			
		}
	}
}
