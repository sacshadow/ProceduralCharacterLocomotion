using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
namespace UnityLogic {

	public abstract class LogicObject : LispObject {
		
		public LogicObject parent;
		public bool isSelect, isEdit;
		
		public virtual LispObject Eval(ULList arg) {
			throw new Exception("Type cant Eval : " + GetType().ToString());
		}
		
		public virtual LogicObject GetMouseOver(Vector2 mousePosition) {
			return null;
		}
		
		public virtual void OnSelect(bool state) {
			isSelect = state;
			isEdit = false;
		}
		
		public virtual void OnSelectGUI() {}
		
		public virtual void OnEdit() {
			isSelect = true;
			isEdit = true;
		}
		
		public virtual void OnGUI() {}
		
	}
}
