using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityLisp;

namespace UnityLogic.UI {
	public class Label : LogicDraw {
		public static GUIStyle style = GUI.skin.label;
		public static GUIContent content = new GUIContent();
		
		public string text;
		
		public bool isInput {get; protected set; }
		
		public static Vector2 GetSize(string text) {
			content.text = text;
			style.fontSize = 0;
			style.alignment = TextAnchor.MiddleCenter;
			return style.CalcSize(content) + new Vector2(4,4);
		}
		
		public Label(LispObject data) {
			if(data is ULString)
				text = ((ULString)data).GetValue();
			else if(data is Symbol)
				text = "#"+data.ToString();
			else
				text = data.ToString();
				
			CalcuSize();
		}
		
		public Label(string text) {
			this.text = text;
			CalcuSize();
		}
		
		public override LogicDraw TryInsert(Vector2 pos) {
			if(!IsInsideArea(pos))
				return null;
			return this;
		}
		
		public override LogicDraw Insert(List<LogicDraw> attached) {
			return LogicUITool.ElementInsert(this, attached);
		}
		
		public override void SetSelect(bool state) {
			base.SetSelect(state);
			if(!state) isInput = false;
		}
		
		public void SetInput(bool state) {
			isInput = state;
			if(text[text.Length-1] == '\n')
				SetText(text.Substring(0,text.Length-1));
		}
		
		public override void OnDrawGUI(Vector2 parentOffset) {
			var lineColor = isSelect ? select : (isHighlight ? hover : normal);
		
			DrawBorder(parentOffset, lineColor);
			
			var dRect = displayRect;
			dRect.x = (dRect.x + parentOffset.x) * canvasScale;
			dRect.y = (dRect.y + parentOffset.y) * canvasScale;
			dRect.width *= canvasScale;
			dRect.height *= canvasScale;
			
			// TextEditor te = new TextEditor();
			
			if(isInput)
				SetText(GUI.TextArea(dRect,text));
			else
				GUI.Label(dRect, text);
		}
		
		public override LispObject ToLO() {
			if(text.IndexOf(" ") == -1 && text[0] == '#')
				return new Symbol(text.Substring(1,text.Length-1));
			
			return new ULString(text);
		}
		
		public void SetText(string newText) {
			if(newText == text)
				return;
			
			text = newText;
			CalcuSize();
			parent.OnChildSizeChange();
		}
		
		public void CalcuSize() {
			var size = GetSize(text);
			size.x = Mathf.Max(20,size.x);
			SetSize(size.x, size.y);
			joinHeight = size.y/2f;
		}
		
	}
}
