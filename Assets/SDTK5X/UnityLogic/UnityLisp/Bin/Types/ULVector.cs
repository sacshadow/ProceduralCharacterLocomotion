using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class ULVector : ULList {
	
		public ULVector() :base() {
			start = "[";
			end = "]";
		}
		public ULVector(List<LispObject> val) :base(val) {
			start = "[";
			end = "]";
		}

		public override bool IsList() { return false; }

		public override ULList Slice(int start, int end) {
			var val = this.GetValue();
			return new ULVector(val.GetRange(start, val.Count-start));
		}
	}
}
