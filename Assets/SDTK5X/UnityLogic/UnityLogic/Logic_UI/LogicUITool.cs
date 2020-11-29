using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public static class LogicUITool {
		
		// public static TextEditor te = new TextEditor();
		
		public static void Copy(LogicDraw ld) {
			TextEditor te = new TextEditor();
			te.text =ld.Copy().ToString();
			te.SelectAll();
			te.Copy();
		}
		
		public static LogicDraw GetPaste() {
			var te = new TextEditor();
			te.Paste();
			var ast = Reader.Read_Input(te.text);

			return GetVerticalForm(ast);
		}
		
		public static T Insert<T>(LogicDraw element) where T : LogicTree, new() {
			var parent = element.parent;
			var temp = new T();
			temp.child = new List<LogicDraw> {element};
			element.parent = temp;
			temp.CalcuChildSize();
			
			parent.OnChildChanged(element, temp);
			return temp;
		}
		
		public static LogicDraw ElementInsert(LogicDraw element, List<LogicDraw> attached) {
			var parent = element.parent;
			
			if(parent.GetType() == typeof(VerticalTree) || parent.GetType() == typeof(Node))
				return VerticalTreeInsert((LogicTree)parent, element, attached);
			else
				return ElementAttached((LogicTree)parent, element, attached);
			
			// return null;
		}
		
		public static LogicDraw ElementAttached(LogicTree parent, LogicDraw element, List<LogicDraw> attached) {
			var rt = GetNodeAttached(attached);
			var list = new List<LogicDraw>{element, rt};
			var temp = new VerticalTree(list);
			parent.OnChildChanged(element, temp);
			
			return rt.OnNav();
		}
		
		public static LogicDraw VerticalTreeInsert(LogicTree parent, LogicDraw element, List<LogicDraw> attached) {
			if(parent.FindChild(element) == 0)
				return parent.InsertChild(parent.child.Count, attached);
			
			return ElementAttached(parent, element, attached);
		}
		
		public static LogicDraw GetNodeAttached(List<LogicDraw> attached) {
			if(attached.Count == 1)
				return attached[0];
			
			return new VerticalTree(attached);
		}
		
		public static LogicDraw GetVerticalForm(LispObject data) {
			if(data is ULVector)
				return new PickTree((ULList)data);
			else if(data is ULList)
				return new VerticalTree((ULList)data);
			else
				return new Label(data);
		}
		
		public static LogicDraw GetHorizonForm(LispObject data) {
			if(data is ULVector)
				return new PickTree((ULList)data);
			else if(data is ULList)
				return new HorizontalTree((ULList)data);
			else
				return new Label(data);
		}
		
		public static List<LogicDraw> GetChildVertical(ULList list, LogicDraw parent) {
			return GetChild(list, parent, GetHorizonForm);
		}
		
		public static List<LogicDraw> GetChildHorizontal(ULList list, LogicDraw parent) {
			return GetChild(list, parent, GetVerticalForm);
		}
		
		public static List<LogicDraw> GetChildPick(ULList list, LogicDraw parent) {
			var rt = list.GetValue().Select(GetHorizonForm).ToList();
			rt.ForEach(x=>x.parent = parent);
			return rt;
		}
		
		public static List<LogicDraw> GetChild(ULList list, LogicDraw parent, Func<LispObject, LogicDraw> GetFirstForm) {
			var rt = new List<LogicDraw>();
			
			rt.Add(GetFirstForm(list[0]));
			Loop.Between(1,list.GetSize()).Do(x=>rt.Add(GetVerticalForm(list[x])));
			rt.ForEach(x=>x.parent = parent);
			
			return rt;
		}
		
	}
}