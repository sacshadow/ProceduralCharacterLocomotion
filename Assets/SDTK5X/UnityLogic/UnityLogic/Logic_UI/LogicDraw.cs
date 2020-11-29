using UnityEngine;
// #if UNITY_EDITOR
	// using UnityEditor;
// #endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public abstract class LogicDraw {
		public static float canvasScale = 1;
		public static Color normal = Color.black;
		public static Color hover = Color.cyan;
		public static Color select = Color.green;
		
		public bool isSelect {get; protected set; }
		public bool isHighlight {get; protected set; }
		
		// public Vector2 size, offset = Vector2.zero;
		public Rect displayRect = new Rect(0,0,0,0);
		public float joinHeight;
		
		public LogicDraw parent;
		
		protected int insertIndex = -1;
		
		public Vector2 offset {
			get {return new Vector2(displayRect.x, displayRect.y); }
			set {displayRect.x = value.x; displayRect.y = value.y; }
		}
		
		public float width {
			get {return displayRect.width; }
			set {displayRect.width = value; }
		}
		
		public float height {
			get {return displayRect.height; }
			set {displayRect.height = value; }
		}
		
		
		public virtual LogicDraw OnDrag() {
			return parent.OnChildDetach(this);
		}
		
		public virtual LogicDraw OnChildDetach(LogicDraw detachChild) {
			throw new Exception("Type can't detach child " + GetType().ToString());
		}
		
		public virtual LogicDraw OnNav() {
			return this;
		}
		
		public virtual LogicDraw OnNavigation(KeyCode key) {
			return parent.OnChildNavigation(key, this);
		}
		
		public virtual LogicDraw OnChildNavigation(KeyCode key, LogicDraw ld) {
			throw new Exception("Type can't navigation child " + GetType().ToString());
		}
		
		// public virtual void OnDragOver(Vector2 pos) {
		
		// }
		
		// public void OnDrop(Node node) {
			
		// }
		
		public virtual LispObject OnCopy(LogicDraw child) {
			throw new Exception("Type can't decide copy child " + GetType().ToString());
		}
		
		public virtual LispObject Copy() {
			return parent.OnCopy(this);
		}
		
		public virtual LogicDraw Paste(List<LogicDraw> pasted) {
			return Insert(pasted);
		}
		
		public virtual void SetSelect(bool state) {
			isSelect = state;
		}
		
		public virtual void SetHighlight(bool state) {
			isHighlight = state;
			insertIndex = -1;
		}
		
		public virtual void DrawInsert(Vector2 pos) {
			
		}
		
		public void SetOffset(float x, float y) {
			displayRect.x = x;
			displayRect.y = y;
		}
		
		public void SetSize(float width, float height) {
			displayRect.width = width;
			displayRect.height = height;
		}
		
		public Vector2 GetSize() {
			return new Vector2(displayRect.width, displayRect.height);
		}
		
		public virtual void OnChildChanged(LogicDraw oldChild, LogicDraw newChild) {
			throw new Exception("Type dont have child " + GetType());
		}
		
		public virtual LogicDraw TryInsert(Vector2 pos) {
			return null;
		}
		
		// public virtual LogicDraw Insert(Vector2 pos, Node attached) {
			// return null;
		// }
		
		public virtual LogicDraw Insert(List<LogicDraw> attached) {
			throw new Exception("Type is can not insert child " + GetType());
		}
		
		public virtual LogicDraw InsertParallel(LogicDraw selected) {
			throw new Exception("Type is not a tree " + GetType());
		}
		
		public virtual LogicDraw Select(Vector2 pos) {
			return IsInsideArea(pos) ? this : null;
		}
		
		public bool IsInsideArea(Vector2 pos) {
			return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
		}
		
		public virtual void OnDrawGUI() {OnDrawGUI(Vector2.zero); }
		public abstract void OnDrawGUI(Vector2 parentOffset);
		
		public virtual void OnChildSizeChange() {}
		
		public abstract LispObject ToLO();
		
		public virtual string ToDataFormat() {
			return ToLO().ToString();
		}
		
		public virtual void DrawBorder(Vector2 parentOffset, Color color) {
			var o = parentOffset;
			
			var tl = new Vector2(o.x, o.y);
			var tr = new Vector2(o.x + width, o.y);
			var bl = new Vector2(o.x, o.y + height);
			var br = new Vector2(o.x + width , o.y + height);
			
			DrawLines(color, tl, tr, tl, bl, tr, br, bl, br);
		}
		
		public virtual void DrawLines(Color color, params Vector2[] points) {
			if(Event.current.type != EventType.Repaint)
				return;
			
			LogicCanvas.lineMat.SetPass(0);
			GL.PushMatrix ();
			GL.Begin (GL.LINES);
			GL.Color (color);
				Loop.Count(points.Length/2).Do(x=>DrawLine((offset + points[x*2]) * canvasScale, (offset + points[x*2+1]) * canvasScale));
			GL.End ();
			GL.PopMatrix ();
		}
		
		public void DrawLine (Vector2 p1, Vector2 p2) {
			GL.Vertex (p1);
			GL.Vertex (p2);
		}
	}
}
