using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Atom : LispObject {
	
		private LispObject value;
		public Atom(LispObject value) { this.value = value; }
		
		//??public MalAtom copy() { return new MalAtom(value); }
		
		public LispObject GetValue() { return value; }
		public LispObject SetValue(LispObject value) { return this.value = value; }
		public override string ToString() {
			return "(atom " + ULFormat.Print(value, true) + ")";
		}
		public override string ToString(bool printReadable) {
			return "(atom " + ULFormat.Print(value, printReadable) + ")";
		}
	}
}
