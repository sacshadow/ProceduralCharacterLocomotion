using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLispEditor {
	public class ULispREPL : EditorWindow {
	
		public string input = "";
		public string output = ">> Ready To Input\n\n";
		
		public Vector2 scroll;
		
		public Env consoleEnvironment;
		public int inputType = 0;
		public string[] iptName = new string[]{"输入", "复制粘贴"};
		
		[MenuItem("UnityLisp/REPL")]
		public static void OpenWindow() {
			GetWindow(typeof(ULispREPL));
		}
		
		public static void SetEnv(string ns, Env env) {
			var w = GetWindow(typeof(ULispREPL)) as ULispREPL;
			w.consoleEnvironment = env;
			w.output = ">>load env " + ns + "\n\n>> Ready To Input\n\n";
		}
		
		void OnEnable() {
			consoleEnvironment = ULTools.GetNewEnv();
		}
		
		void OnGUI() {
			LayoutHorizontal(HeadLine);
			
			ShowBody();
			CheckKeyEvent();
		}
		
		public void HeadLine() {
			if(GUILayout.Button("Clean"))
				output = ">> Ready To Input\n\n";
		}
		
		public void LayoutHorizontal(Action Process) {
			GUILayout.BeginHorizontal();
				Process();
			GUILayout.EndHorizontal();
		}
		
		
		public void ShowBody() {
			scroll = GUILayout.BeginScrollView(scroll);
				// GUILayout.Label(output);
				// EditorGUILayout.TextArea(output,"label");
				EditorGUILayout.TextArea(output);
			GUILayout.EndScrollView();
			
			inputType = GUILayout.Toolbar(inputType, iptName);
			
			if(inputType == 1) {
				input = EditorGUILayout.TextArea(input, GUILayout.Height(80));
				if(GUILayout.Button("Eval")) {
					GUIUtility.keyboardControl = 0;
					MatchBracket();
					CheckInput();
				}
			}
			else
				input = GUILayout.TextArea(input, GUILayout.Height(80));
		}
		
		public void CheckKeyEvent() {
			if(Event.current!=null && Event.current.isKey)
				KeyCommand(Event.current);
		}
		
		private void KeyCommand(Event e) {
			if(e.keyCode == KeyCode.Return)
				CheckInput();
				
			if(e.keyCode == KeyCode.Tab)
				MatchBracket();
		}
		
		private void CheckInput() {
			int fb = input.Replace("(","").Length;
			int rb = input.Replace(")","").Length;
			
			if(fb == rb)
				RunREPL();
		}
		
		private void MatchBracket() {
			int fb = input.Replace("(","").Length;
			int rb = input.Replace(")","").Length;
			string attach = "";
			
			Loop.Count(rb - fb).Do(x=>attach+=")");
			input += attach;
			
			Repaint();
		}
		
		private void RunREPL() {
			string[] inputCmd = input.Split('\n');
			string str = input;
			
			// if(inputType == 1)
				// GUIUtility.keyboardControl = 0;
			
			Loop.ForEach(inputCmd, AddEachLine);
			
			input = "";
			output += "\n";
			
			try {
				var result = ULisp.Eval(ULisp.Parse(str), consoleEnvironment);
				output += ">>" + ULFormat.Print(result, true) + "\n\n";
			}
			catch (ULContinue) {
				Debug.Log("ULContinue");
			}
			catch (System.Exception e) {
				scroll = Vector2.up * 4000f;
				Repaint();
				
				throw e;
			}
			
			scroll = Vector2.up * 4000f;
			Repaint();
		}
		
		private void AddEachLine(string line) {
			if(line.Length == 0)
				return;
			output += "<<" + line + "\n";
		}
		
	}
}













