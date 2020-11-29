using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Symbol : LispObject {
		private string value;
		public Symbol(string v) { value = v; }
		public Symbol(ULString v) { value = v.GetValue(); }
		public new Symbol Copy() { return this; }

		public string GetName() { return value; }
		public override string ToString() {
			return value;
		}
		public override string ToString(bool printReadable) {
			return value;
		}
	}
}
