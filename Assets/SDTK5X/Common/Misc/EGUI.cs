using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK {
	public static class EGUI {
		public delegate bool TryParse<T>(string input, out T output);
		
		public static string[] toggleText = new string[]{"On", "Off"};
		
		
		public static void Btn(string label, Action Callback) {
			if(GUILayout.Button(label))
				Callback();
		}
		public static void Btn<T>(string label, Action<T> Callback, T data) {
			if(GUILayout.Button(label))
				Callback(data);
		}
		
		public static bool Foldout(bool state, string label, Action Process) {
			var rt = state;
			string s = state ? ">":"V";
			
			HorizontalLayout(()=>{
				if(GUILayout.Button(s, GUILayout.Width(20)))
					rt = !rt;
				GUILayout.Label(label);
			});
			
			if(rt) Process();
			
			return rt;
		}
		
		
		public static void NextColor(Color color, Action Process) {
			var c = UnityEngine.GUI.backgroundColor;
			UnityEngine.GUI.backgroundColor = color;
			Process();
			UnityEngine.GUI.backgroundColor = c;
		}
		
		public static void Label(string label, object value) {
			GUILayout.Label(label + " : " + value.ToString());
		}
		
		public static int IntField(string label, int value) {
			return PairHoriz(label, value, int.TryParse);
		}
		
		public static float FloatField(string label, float value) {
			return PairHoriz(label, value, float.TryParse);
		}
		
		public static float FloatRange(string label, float value, float min, float max) {
			var k = value;
			var l = value;
			
			HorizontalLayout(()=>{
				GUILayout.Label(label);
				k = GUILayout.HorizontalSlider(k,min,max);
				var r = GUILayout.TextField(k.ToString(), GUILayout.Width(80));
				if(float.TryParse(r, out l))
					k = l;
			});
			
			return k;
		}
		
		public static int IntRange(string label, int value, int min, int max) {
			var k = FloatRange(label, value, min, max);
			return Mathf.RoundToInt(k);
		}
		
		public static int SelectEnum(string label, int select, int xCount = 4, params string[] enumText) {
			GUILayout.Label(label + " : ");
			return GUILayout.SelectionGrid((int)select,enumText,xCount);
		}
		
		/****************************************************************************************************************************************/
		public static int SelectMask(string label, int mask, int xCount, params string[] enumText) {
			return SelectMask(label, mask, enumText.Length, xCount, x=>enumText[x]);
		}
		public static int SelectMask(string label, int mask, int Count, int xCount, Func<int,string> GetName) {
			var rt = mask;
			GUILayout.Label(label);
			MaskPatten(true, mask, Count, xCount, GetName, x=>rt = mask & (~(1<<x)));
			MaskPatten(false, mask, Count, xCount, GetName, x=>rt = mask | (1<<x));
			
			return rt;
		}
		
		private static void MaskPatten(bool isSelect,int mask, int Count, int xCount, Func<int,string> GetName, Action<int> OnClicked) {
			int count = 0, c = 0, index=0;
			for(int i=0; i< Count; i++) {
				if(isSelect == ((mask & 1<<i) > 0))
					count ++;
			}
			
			GUILayout.BeginVertical("box"); {
				GUILayout.Label(isSelect ? "Selected" : "UnSelected");
				GUILayout.BeginHorizontal(); {
					while(c<count) {
						if(isSelect != ((mask & 1<<index) > 0)) {
							index++;
							continue;
						}
						
						if(GUILayout.Button(GetName(index), GUILayout.Width(90)))
							OnClicked(index);
						
						if(c%xCount == xCount-1 && c != count-1) {
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
						}
						
						index++;
						c++;
					}
				} GUILayout.EndHorizontal();
			} GUILayout.EndVertical();
		}
		/****************************************************************************************************************************************/
		
		public static bool Toggle(string label, bool state) {
			var rt = state;
			var select = state ? 0 : 1;
			HorizontalLayout(()=>{
				GUILayout.Label(label + " : ");
				select = GUILayout.Toolbar(select, toggleText);
				rt = select == 0;
			});
			return rt;
		}
		
		public static Action SelectFieldAction<T>(List<T> t, Func<T,string> element, Action<int> Callback) {
			return ()=>{
				for(int i=0; i<t.Count; i++) {
					if(GUILayout.Button(element(t[i]))) {
						Callback(i);
						break;
					}
				}};
		}
	
	#region Tools
		public static T PairHoriz<T>(string label, T value, TryParse<T> Parse) {
			T rt = value;
			HorizontalLayout(Pair<T>(label, value, Parse, x=>rt = x));
			return rt;
		}
		
		public static Action Pair<T>(string label,  T value,  TryParse<T> Parse, Action<T> Callback) {
			var rt = value;
			return ()=> {
				GUILayout.Label(label + " : ");
				var r = GUILayout.TextField(value.ToString());
				if(Parse(r, out rt))
					Callback(rt);
			};
		}
		
		public static Action Sequence(params Action[] Process) {
			return ()=> Loop.ForEach(Process, p=> p());
		}
		
		public static void HorizontalLayout(Action GUIAction) {
			GUILayout.BeginHorizontal();
			GUIAction();
			GUILayout.EndHorizontal();
		}
		public static void VerticalLayout(Action GUIAction) {
			GUILayout.BeginVertical();
			GUIAction();
			GUILayout.EndVertical();
		}
		
		public static Vector2 ScrollView(Vector2 scrollPosition, Action GUIAction, params GUILayoutOption[] options) {
			return ScrollView(scrollPosition, false, false, GUIAction, options);
		}
		
		public static Vector2 ScrollView(Vector2 scrollPosition, bool showHorz, bool showVert, Action GUIAction, params GUILayoutOption[] options) {
			var scrollPos = GUILayout.BeginScrollView(scrollPosition, showHorz, showVert, options); {
				GUIAction();
			} GUILayout.EndScrollView();
			return scrollPos;
		}
		
	#endregion	
	}
}
