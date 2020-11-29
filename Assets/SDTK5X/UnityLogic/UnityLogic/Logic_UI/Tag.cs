using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	
	public class Tag {
		public const int border = 500;
		
		public List<Node> node;
		public Vector2 scroll;
		public Rect viewRect, renderRect, displayRect;
		public string tagName;
		
		public float scale = 1;
		
		public static Rect GetViewRect(List<Node> node) {
			var min = Vector2.zero;
			var max = Vector2.zero;
			
			node.ForEach(x=> {
				min.x = Mathf.Min(min.x, x.x);
				min.y = Mathf.Min(min.y, x.y);
				max.x = Mathf.Max(max.x, x.x + x.width);
				max.y = Mathf.Max(max.y, x.y + x.height);
			});
			
			return new Rect(min.x - border, min.y - border, max.x - min.x + border * 2, max.y - min.y + border * 2);
		}
		
		public static Rect GetMaxSize(Rect rect, Rect area) {
			var rt = rect;
			rt.width = Mathf.Max(rect.width, area.width);
			rt.height = Mathf.Max(rect.height, area.height);
			return rt;
		}
		
		public static Vector2 WindowPosToCanvasPos(Vector2 input, Vector2 scroll, Rect fullRect, Rect areaRect) {
			return input + scroll + new Vector2(fullRect.x, fullRect.y) - new Vector2(areaRect.x, areaRect.y);
		}
		
		public Tag(LispObject tagData) {
			tagName = ((ULString)((ULList)tagData)[1]).GetValue();
			node = ((ULList)tagData).Rest().Rest().GetValue().Select(x=>new Node(x, this)).ToList();
			viewRect = GetViewRect(node);
			scroll = new Vector2(viewRect.width + viewRect.x, viewRect.height + viewRect.y)/2f;
		}
		
		
		public LogicDraw SelectLogic(Vector2 mousePosition) {
			var localPos = GetWP2CP(mousePosition) / LogicDraw.canvasScale;
			
			LogicDraw rt = null;
			node.ForEach(x=> rt = SelectFromNode(x.Select(localPos), rt));
			return rt;
		}
		
		public LogicDraw InsertLogic(Vector2 mousePosition, Node draged) {
			var localPos = GetWP2CP(mousePosition) / LogicDraw.canvasScale;
			
			for(int i=0; i< node.Count; i++) {
				if(node[i] == draged)
					continue;
				var rt = node[i].TryInsert(localPos);
				if(rt != null) {
					rt = rt.Insert(draged.child);
					node.Remove(draged);
					return rt;
				}
			}
			
			return draged.child[0];
		}
		
		public LogicDraw TryInsertLogic(Vector2 mousePosition, Node draged) {
			var localPos = GetWP2CP(mousePosition) / LogicDraw.canvasScale;
			
			LogicDraw rt = null;
			node.ForEach(x=> {
				if(x != draged)
					rt = SelectFromNode(x.TryInsert(localPos), rt);});
			return rt;
		}
		
		public LogicDraw SelectFromNode(LogicDraw newLd, LogicDraw org) {
			return newLd != null ? newLd : org;
		}
		
		public Node AddNode(LogicDraw ld, Vector2 mousePosition, bool resize = true) {
			var list = ld.GetType() == typeof(VerticalTree) ? 
					((VerticalTree)ld).child : new List<LogicDraw> {ld};
		
			var rt = new Node(GetWP2CP(mousePosition), list, this);
			node.Add(rt);
			if(resize)
				OnNodeSizeChange();
			return rt;
		}
		
		public Node AddNode(LispObject nodeData, Vector2 mousePosition, bool resize = true) {
			var rt = new Node(GetWP2CP(mousePosition), nodeData, this);
			node.Add(rt);
			if(resize)
				OnNodeSizeChange();
			return rt;
		}
		
		public void OnNodeSizeChange() {
			viewRect = GetViewRect(node);
		}
		
		public Vector2 GetWP2CP(Vector2 input) {
			return WindowPosToCanvasPos(input,scroll,renderRect, displayRect);
		}
		
		public string GetName() {
			return tagName;
		}
		
		public LispObject ToLO() {
			var list = node.Select(x=>x.ToLO()).ToList();
			list.Insert(0,new Symbol("tag"));
			list.Insert(1,new ULString(tagName));
			return new ULList(list);
		}
		
		public void Draw(Rect area) {
			displayRect = area;
			renderRect = GetMaxSize(viewRect, area);
			
			Label.style.fontSize = (int)Mathf.Lerp(8,12,scale);
			LogicDraw.canvasScale = Mathf.Lerp(0.8f,1.2f,scale);
			// LogicTree.UpdateScale();
			
			renderRect.x *= LogicDraw.canvasScale;
			renderRect.y *= LogicDraw.canvasScale;
			renderRect.width *= LogicDraw.canvasScale;
			renderRect.height *= LogicDraw.canvasScale;
			
			GUILayout.BeginArea(area, "", "box");
			// GUILayout.BeginArea(area);
			
				scroll = GUI.BeginScrollView(new Rect(0,0, area.width, area.height), scroll, renderRect);
					// var orgColor = GUI.color;
					// GUI.color = Color.black;
				
					node.ForEach(x=>x.OnDrawGUI());
					
					// GUI.color = orgColor;	
				GUI.EndScrollView(); 
			
			GUILayout.EndArea();
			
			Label.style.fontSize = 0;
			
		}
		
	}
}
