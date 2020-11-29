using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;
namespace UnityLogic {
	public class LogicBox {
		public const int btmBorder = 40;
		
		public Env env;
		public LogicPanel panel;
		public string[] tagName;
		public int selectTag = 0;
		public Vector2 tagScroll = Vector2.zero;
		
		public List<LogicObject> select;
		
		public LogicBox() {
			env = ULTools.GetNewEnv();
			panel = new LogicPanel(env);
			RefreshTagName();
			select = new List<LogicObject>();
		}
		
		public void RefreshTagName() {
			tagName = panel.panelTag.Select(x=>x.tagName).ToArray();
		}
		
		public void OnSelectTag(int index) {
			selectTag = Mathf.Clamp(index,0,panel.panelTag.Count);
		}
		
		public void OnGUI(float width, float height) {
			GUILayout.BeginArea(new Rect(0,0,width, height-btmBorder));
			DrawTag(width, height);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0,height-btmBorder,width,btmBorder));
			DrawToolbar();
			GUILayout.EndArea();
		}
		
		public void DrawTag(float width, float height) {
			var tag = panel.panelTag[Mathf.Clamp(selectTag,0,panel.panelTag.Count)].tag;
			
			
			tag.titleNode.OnGUI();
			
			tag.node.ForEach(x=>x.OnGUI());
			
		}
		
		public void DrawToolbar() {
			tagScroll = GUILayout.BeginScrollView(tagScroll);
			var temp = GUILayout.Toolbar(selectTag, tagName);
			if(temp != selectTag) OnSelectTag(temp);
			GUILayout.EndScrollView();
		}
		
		
		public void AddNewNode(Vector2 mousePos) {
			var temp = new Node("新标签");
			temp.SetPos(mousePos);
			panel.panelTag[selectTag].tag.node.Add(temp);
		}
		
		public void Select(Vector2 mousePos) {
			var clickedObject = panel.panelTag[selectTag].tag.GetMouseOver(mousePos);
			Unselect();
			if(clickedObject != null) {
				clickedObject.OnSelect(true);
				select.Add(clickedObject);
			}
			
		}
		
		public void Unselect() {
			select.ForEach(x=>x.OnSelect(false));
			select.Clear();
		}
		
		public void OnEditSelect() {
			if(select.Count == 0)
				return;
			var edit = select[0];
			Unselect();
			select.Add(edit);
			edit.OnEdit();
		}
		
		
	}
}
