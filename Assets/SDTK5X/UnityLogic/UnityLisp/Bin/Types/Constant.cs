using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Constant : LispObject {
		
		private string value;
		
		public Constant(string name) { value = name; }
		public new Constant Copy() { return this; }

		public override string ToString() {
			return value;
		}
		public override string ToString(bool printReadable) {
			return value;
		}
	}
}
