using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
namespace UnityLogic {
	public class Node : LogicObject {
		
		public GUIContent name;
		public LogicObject value;
		public int x, y, width, height, areaWidth, areaHeight;
		
		public bool isDirty = true;
		
		public Node(string name) {
			this.name = new GUIContent(name);
		}
		
		public Node(string name, LogicObject value) {
			this.name = new GUIContent(name);
			SetValue(value);
		}
		
		public void SetValue(LogicObject value) {
			this.value = value;
			value.parent = this;
		}
		
		public void SetPos(Vector2 pos) {
			x = (int)pos.x;
			y = (int)pos.y;
		}
		
		public void CalculateSize() {
			var style = GUI.skin.button;
			Vector2 size = style.CalcSize(name);
			width = (int)Mathf.Max(size.x,60);
			height = (int)Mathf.Max(size.y,30);
			
			areaWidth = width + 20;
			areaHeight = height + 20;
		}
		
		public override LogicObject GetMouseOver(Vector2 mousePosition) {
			if(!(new Rect(x,y,areaWidth,areaHeight).Contains(mousePosition)))
				return null;
			if(new Rect(x,y,width,height).Contains(mousePosition))
				return this;
			return null;
		}
		
		public override void OnGUI() {
			if(isDirty)
				Recalculate();
			
			var c = GUI.color;
			GUI.color = new Color(0.5f,0.5f,0.5f,0.1f);
			GUILayout.BeginArea(new Rect(x,y,areaWidth,areaHeight),"", "box");
			GUI.color = c;
			if(isSelect) {
				if(isEdit) {
					var temp = GUI.TextArea(new Rect(0,0,width,height), name.text);
					if(temp != name.text) OnNameChange(temp);
				}
				else {
					GUI.color = new Color(0.4f,0.4f,1f,1f);
					GUI.Label(new Rect(0,0,width,height), name, "button");
					GUI.color = c;
				}
			}
			else
				GUI.Label(new Rect(0,0,width,height), name, "button");
			GUILayout.EndArea();
		}
		
		public void Recalculate() {
			isDirty = false;
			CalculateSize();
		}
		
		public void OnNameChange(string newName) {
			name.text = newName;
			CalculateSize();
		}
		
	}
}
