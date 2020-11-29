using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
using UnityLogic.UI;
using UnityLispEditor;

namespace UnityLogicEditor {
	public class LogicEditor : EditorWindow {
		// public static string newDescription = "(logic (tag \"New Tag\" (node 0 0 (\"New-Topic\"))))";
		// public static string newTag = "(tag \"New Tag\" (node 0 0 (\"New-Topic\")))";
		// public static string newNodeData = "(\"New-Topic\")";
		
		// public string path;
		// public LogicCanvas logicCanvas;
		
		// public bool isMouseDown = false;
		// public bool isDragCanvas = false, isDragNode = false;
		// public Vector2 lastMousePosition;
		
		// public LogicDraw hoveredLogic;
		// public LogicDraw selectedLogic, newNode;
		
		// public Node dragedNode;
		
		// public Vector2 mouseDownPos;
		
		public LogicGUI logicGUI;
		public string path= "";
		
		
		[MenuItem("UnityLogic/Bind to REPL")]
		public static void BindToREPL() {
			var window = GetWindow(typeof(LogicEditor)) as LogicEditor;
			
			var env = ULTools.GetNewEnv();
			UnityLogic.LogicCore2.GetLogicEnv(env);
			ULisp.Eval(window.logicGUI.logicCanvas.ToLO(), env);
			
			ULispREPL.SetEnv(window.logicGUI.logicCanvas.canvasName, env);
		}
		
		[MenuItem("UnityLogic/Logic Editor")]
		public static void Open() {
			GetWindow(typeof(LogicEditor));
		}
		
		void OnEnable() {
			wantsMouseMove = true;
			logicGUI = new LogicGUI(true);
			if(path != null && path.Length > 0 && SDTK.DataRW.IsDataExists(path))
				logicGUI.path = path;
		}
		
		void OnGUI() {
			
			logicGUI.CheckEventBefore();
			logicGUI.OnGUI((int)position.width, (int)position.height);
			logicGUI.CheckEventAfter();
			
			// EditorGUILayout.BeginHorizontal("box");
			// if(GUILayout.Button("New", GUILayout.Width(80)))
				// NewFile();
			// if(GUILayout.Button("Save", GUILayout.Width(80)))
				// SaveFile();	
			// if(GUILayout.Button("Load", GUILayout.Width(80)))
				// LoadFile();
			// if(GUILayout.Button("Refresh", GUILayout.Width(80)))
				// RefreshFile();
			// GUILayout.FlexibleSpace();
			
			// if(logicCanvas != null)
				// logicCanvas.selectedTag.scale = GUILayout.HorizontalSlider(
					// logicCanvas.selectedTag.scale, 0f, 1, GUILayout.Width(200));
			// else
				// GUILayout.HorizontalSlider(1,0f,1, GUILayout.Width(200));
			
			// if(GUILayout.Button("Bind"))
				// BindEnv();
			// EditorGUILayout.EndHorizontal();
			
			// if(logicCanvas == null) {
				// if(path != null && path.Length > 0)
					// RefreshFile();
				// else
					// NewFile();
			// }
			
			// try {
				// var rect = new Rect(0,30,position.width, position.height);
				
				// CheckEventBefore();
				
				// if(isMouseDown)
					// CheckNodeDrag();
				// else if(isDragNode)
					// DragNode();
				
				// logicCanvas.Draw(rect);
				
				// if(!isDragNode)
					// CheckHover();
				
				// CheckEventAfter();
			// } catch (Exception e) {
				// Debug.LogError(e.Message);
			// }
		}
		
		void Update() {
			Repaint();
			
			if(logicGUI != null)
				path = logicGUI.path;
		}
		/*
		private void BindEnv() {
			
			var env = ULTools.GetNewEnv();
			UnityLogic.LogicCore2.GetLogicEnv(env);
			ULisp.Eval(logicCanvas.ToLO(), env);
		
			ULispREPL.SetEnv(logicCanvas.canvasName, env);
		}
		
		private void DragNode() {
			if(dragedNode == null)
				return;
			
			dragedNode.DragNode(logicCanvas.selectedTag.GetWP2CP(Event.current.mousePosition));
			
			var temp = logicCanvas.selectedTag.TryInsertLogic(Event.current.mousePosition, dragedNode);
			
			// if(temp != null)
				// Debug.Log(temp.GetType());
			
			SetHighlight(temp);
			SetHighlight(logicCanvas.selectedTag.TryInsertLogic(Event.current.mousePosition, dragedNode));
		}
		
		private void CheckHover() {
			SetHighlight(logicCanvas.selectedTag.SelectLogic(Event.current.mousePosition));
		}
		
		private void SetHighlight(LogicDraw ld) {
			if(ld == hoveredLogic)
				return;
			
			if(hoveredLogic != null)
				hoveredLogic.SetHighlight(false);
			
			hoveredLogic = ld;
			
			if(hoveredLogic != null)
				hoveredLogic.SetHighlight(true);
		}
		
		private void NewFile() {
			path = null;
			logicCanvas = new LogicCanvas(Reader.Read_Input(newDescription));
			logicCanvas.canvasName = "New Logic";
		}
		
		private void SaveFile() {
			if(path == null)
				path = EditorUtility.SaveFilePanel("Save File", "", logicCanvas.canvasName,"lisp");
			if(path == null || path.Length == 0)
				return;
			
			SDTK.DataRW.CreateFile(logicCanvas.ToLO().ToString(), path);
			Debug.Log("File Saved");
		}
		
		private void LoadFile() {
			path = EditorUtility.OpenFilePanel("读取文件", "", "lisp");
			if(path == null || path.Length == 0)
				return;
			
			RefreshFile();
			Debug.Log("load file " + path);
		}
		
		private void RefreshFile() {
			if(path == null || path.Length == 0)
				return;
		
			var text = SDTK.DataRW.LoadFile(path);
			logicCanvas = new LogicCanvas(Reader.Read_Input(text));
		}
		
		private void CheckEventBefore() {
			// mousePos = Event.current.mousePosition;
			Event evt = Event.current;
			
			if(evt.type == EventType.ScrollWheel)
				OnMouseScrollWheel(evt);
		}
		
		private void CheckEventAfter() {
			Event evt = Event.current;
			
			if(evt.type == EventType.MouseDown)
				OnMouseDown(evt);
			else if(evt.type == EventType.ContextClick)
				OnContextClick(evt);
			else if(evt.type == EventType.MouseUp)
				OnMouseUp(evt);
			else if(evt.type == EventType.KeyUp)
				CheckKeyUp(evt);
			
			if(isDragCanvas)
				DragCanvasUpdate(evt);	
		}
		
		private void CheckKeyUp(Event evt) {
		
			if(evt.keyCode == KeyCode.F2)
				OnEditSelect(evt);
			else if(evt.keyCode == KeyCode.Return)
				AddParallel(evt);
			else if(evt.keyCode == KeyCode.Insert)
				InsertNewChild(evt);
			else if(evt.keyCode == KeyCode.Delete)
				RemoveSelected(evt);
			else if(evt.keyCode == KeyCode.Escape)
				UnSelect(evt);
			else if(evt.control)
				OnControlKey(evt);
			else if(IsNavigationKey(evt.keyCode))
				BeginNavigation(evt.keyCode);
			
			//TODO: 输入 和 输入法
			// else if(IsCharacterInputMode(evt))
				// SetToInput(evt);
		}
		
		private void OnControlKey(Event evt) {
			if(evt.keyCode == KeyCode.T)
				logicCanvas.AddTag(Reader.Read_Input(newTag));
			else if(evt.keyCode == KeyCode.S)
				SaveFile();
			else if(evt.keyCode == KeyCode.O)
				LoadFile();
			else if(evt.keyCode == KeyCode.N)
				NewFile();
			else if(evt.keyCode == KeyCode.X)
				Cut(evt);
			else if(evt.keyCode == KeyCode.C)
				Copy(evt);
			else if(evt.keyCode == KeyCode.V)
				Paste(evt);
		}
		
		private void OnMouseScrollWheel(Event evt) {
			if(!evt.control)
				return;
			
			logicCanvas.selectedTag.scale =
				Mathf.Clamp(logicCanvas.selectedTag.scale - evt.delta.y * 0.05f, 0 ,1);
			
			evt.Use();
		}
		
		private void Cut(Event evt) {
			if(selectedLogic == null)
				return;
			
			Copy(evt);
			RemoveSelected(evt);
		}
		
		private void Copy(Event evt) {
			if(selectedLogic == null)
				return;
		
			LogicUITool.Copy(selectedLogic);
		}
		
		private void Paste(Event evt) {
			var paste = LogicUITool.GetPaste();
			
			if(selectedLogic != null)
				selectedLogic.Insert(new List<LogicDraw>{paste});
			else
				SelectLogic(logicCanvas.selectedTag.AddNode(paste, evt.mousePosition));
		}
		
		private void InsertNewChild(Event evt) {
			if(selectedLogic == null)
				return;
		
			if(evt.control) {
				LogicUITool.Insert<HorizontalTree>(selectedLogic);
				SelectLogic(selectedLogic.parent.InsertParallel(selectedLogic));
			}
			else if(evt.alt) {
				var pickTree = selectedLogic.Insert(new List<LogicDraw> {new PickTree()});
				SelectLogic(pickTree.Insert(new List<LogicDraw>{new Label("Subtopic 1")}));
			}
			else
				SelectLogic(selectedLogic.Insert(new List<LogicDraw>{new Label("Subtopic 1")}));
		}
		
		private void RemoveSelected(Event evt) {
			if(selectedLogic == null)
				return;
			
			var removed = selectedLogic.OnDrag();
			if(removed is Node && logicCanvas.selectedTag.node.Count > 1)
				logicCanvas.selectedTag.node.Remove((Node)removed);
			logicCanvas.selectedTag.OnNodeSizeChange();
			selectedLogic = null;
		}
		
		private void SetToInput(Event evt) {
			((Label)selectedLogic).text = "" + evt.character;
			OnEditSelect(evt);
		}
		
		private bool IsCharacterInputMode(Event evt) {
			return IsSelectLabelAndNotInput() && InputUtiltiy.IsCharacterKey(evt.keyCode);
		}
		
		private bool IsSelectLabelAndNotInput() {
			if(selectedLogic == null || !(selectedLogic is Label))
				return false;
			
			return !((Label)selectedLogic).isInput;
		}
		
		private bool IsNavigationKey(KeyCode key) {
			if(selectedLogic != null && selectedLogic is Label) {
				if(((Label)selectedLogic).isInput)
					return false;
			}
			
			return key == KeyCode.LeftArrow
				|| key == KeyCode.RightArrow
				|| key == KeyCode.UpArrow
				|| key == KeyCode.DownArrow;
		}
		
		private void BeginNavigation(KeyCode key) {
			if(selectedLogic == null)
				return;
			SelectLogic(selectedLogic.OnNavigation(key));
		}
		
		private void UnSelect(Event evt) {
			if(selectedLogic == null)
				return;
				
			if(IsLabelAndInput())
				((Label)selectedLogic).SetInput(false);
			else {
				selectedLogic.SetSelect(false);
				selectedLogic = null;
			}
			evt.Use();
		}
		
		private void AddParallel(Event evt) {
			if(selectedLogic == null)
				return;
			
			if(IsLabelAndInput()) {
				var label = (Label)selectedLogic;
				if(!evt.control)
					label.SetInput(false);
				evt.Use();
			}
			else
				SelectLogic(selectedLogic.parent.InsertParallel(selectedLogic));
		}
		
		private bool IsLabelAndInput() {
			if(selectedLogic == null) return false;
			return selectedLogic is Label && ((Label)selectedLogic).isInput;
		}
		
		private void OnMouseDown(Event evt) {
			if(evt.button == 0)
				if(evt.clickCount == 2)
					DoubleClick(evt);
				else
					SingleClick(evt);
			else if(evt.button == 1 || evt.button == 2)
				BeginDragCanvas(evt);
		}
		
		private void OnContextClick(Event evt) {
			if(hoveredLogic != null)
				SelectLogic(hoveredLogic);
			if(selectedLogic != null)
				ContextClickLogic(selectedLogic, evt);
			else
				ContextClickEmpty(evt);
		}
		
		private void ContextClickLogic(LogicDraw ld, Event evt) {
			
		}
		
		private void ContextClickEmpty(Event evt) {
			
		}
		
		private void OnMouseUp(Event evt) {
			if(evt.button == 1 || evt.button == 2)
				EndDragCanvas();
			if(evt.button == 0)
				EndDragNode();
		}
		
		private void BeginDragNode() {
			isDragNode = true;
			
			var dragedItem = selectedLogic.OnDrag();
			if(dragedItem is Node)
				dragedNode = (Node)dragedItem;
			else
				dragedNode = logicCanvas.selectedTag.AddNode(dragedItem, Event.current.mousePosition);
		}
		
		private void CheckNodeDrag() {
			if(Vector2.Distance(mouseDownPos, Event.current.mousePosition) < 20)
				return;
			
			isMouseDown = false;
			
			BeginDragNode();
			Repaint();
		}
		
		private void EndDragNode() {
			if(dragedNode != null)
				TryDrop();
			
			dragedNode = null;
			isDragNode = false;
			isMouseDown = false;
			logicCanvas.selectedTag.OnNodeSizeChange();
		}
		
		private void TryDrop() {
			SelectLogic(logicCanvas.selectedTag.InsertLogic(Event.current.mousePosition, dragedNode));
		}
		
		private void OnEditSelect(Event evt) {
			if(selectedLogic == null || !(selectedLogic is Label))
				return;
				
			((Label) selectedLogic).SetInput(true);
			Repaint();
		}
		
		private void DoubleClick(Event evt) {
			if(selectedLogic == null || !(selectedLogic is Label))
				SelectLogic(logicCanvas.selectedTag.AddNode(Reader.Read_Input(newNodeData), evt.mousePosition));
			else
				OnEditSelect(evt);
		}
		
		private void SingleClick(Event evt) {
			var temp = logicCanvas.selectedTag.SelectLogic(evt.mousePosition);
			
			isMouseDown = (temp != null);
			mouseDownPos = evt.mousePosition;
			
			if(temp != selectedLogic)
				SelectLogic(temp);
		}
		
		private void SelectLogic(LogicDraw ld) {
			if(selectedLogic != null)
				selectedLogic.SetSelect(false);
			
			if(ld is Node)
				selectedLogic = ((Node) ld).child[0];
			else
				selectedLogic = ld;
			
			if(selectedLogic == null)
				return;
				
			selectedLogic.SetSelect(true);
		}
		
		private void BeginDragCanvas(Event evt) {
			isDragCanvas = true;
			lastMousePosition = evt.mousePosition;
		}
		
		private void DragCanvasUpdate(Event evt) {
			logicCanvas.selectedTag.scroll -= evt.mousePosition - lastMousePosition;
			lastMousePosition = evt.mousePosition;
		}
		
		private void EndDragCanvas() {
			isDragCanvas = false;
		}
		*/
	}
}
