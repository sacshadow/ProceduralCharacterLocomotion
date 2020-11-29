using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public class LogicCanvas {
		public const int tagTabHeight = 50;
		// public const int controlAreaWidth = 200;
		public const int controlAreaWidth = 0;
		
		public static Material lineMat;
		
		public string canvasName;
		public List<Tag> tag;
		public int selectTag = 0;
		public int editorDrawOffset = 20;
		
		public Tag selectedTag {get {return tag[selectTag]; }}
		
		public LogicCanvas(LispObject logicData) {
			var list = (ULList)logicData;
			// canvasName = list[1].ToString();
			var tagData = list.Rest().GetValue();
			tag = tagData.Select(x=>new Tag(x)).ToList();
		}
		
		public void AddTag(LispObject tagData) {
			tag.Add(new Tag(tagData));
		}
		
		public void AddNode(LispObject nodeData, Vector2 mousePosition) {
			tag[selectTag].AddNode(nodeData, mousePosition);
		}		
		
		public void Draw(Rect rect) {
			var tagArea = rect;
			tagArea.height -= tagTabHeight;
			tagArea.width -= controlAreaWidth;
			
			if(LogicCanvas.lineMat == null)
				lineMat = Resources.Load<Material>("OnGUILineMat");
			lineMat.SetVector("_displayArea",new Vector4(0,30 + editorDrawOffset,rect.width-controlAreaWidth-15, rect.height - editorDrawOffset- 45));
			
			tag[selectTag].Draw(tagArea);
			
			GUILayout.BeginArea(new Rect(rect.x, rect.y + tagArea.height, rect.width, 30));
			
			GUILayout.BeginHorizontal();
			Loop.Count(tag.Count).Do(x=> {
				if(x == selectTag)
					GUILayout.Box(tag[x].GetName());
				else if(GUILayout.Button(tag[x].GetName()))
					selectTag = x;
			});
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
		}
		
		public LispObject ToLO() {
			var list = tag.Select(x=>x.ToLO()).ToList();
			list.Insert(0,new Symbol("logic"));
			return new ULList(list);
		}
		
		private void DrawLine (Vector2 p1, Vector2 p2) {
			GL.Vertex (p1);
			GL.Vertex (p2);
		}
		
	}
}
