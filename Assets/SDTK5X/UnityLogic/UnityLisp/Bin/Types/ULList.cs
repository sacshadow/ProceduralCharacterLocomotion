using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace UnityLisp {
	public class ULList : LispObject {
		public string start = "(", end = ")";
		
		private List<LispObject> value;
		
		public ULList() {
			value = new List<LispObject>();
		}
		public ULList(List<LispObject> val) {
			value = val;
		}
		public ULList(params LispObject[] val) {
			value = new List<LispObject>();
			AddRange(val);
		}
		
		public List<LispObject> GetValue() { return value; }
		public override bool IsList() { return true; }

		public override string ToString() {
			return start + ULFormat.Join(value, " ", true) + end;
		}
		public override string ToString(bool printReadable) {
			return start + ULFormat.Join(value, " ", printReadable) + end;
		}

		public ULList AddRange(params LispObject[] val) {
			for (int i = 0; i < val.Length; i++) {
				value.Add(val[i]);
			}
			return this;
		}
		
		public ULList Apd(LispObject val) {
			value.Add(val);
			return this;
		}

		public int GetSize() { return value.Count; }
		public LispObject Nth(int index) {
			return value.Count > index ? value[index] : LispObject.nilValue;
		}
		public LispObject this[int index] {
			get { return Nth(index); }
		}
		public virtual ULList Rest() {
			if (GetSize() > 0) {
				return new ULList(value.GetRange(1, value.Count-1));
			} else {
				return new ULList();
			}
		}
		public virtual ULList Slice(int start) {
			return new ULList(value.GetRange(start, value.Count-start));
		}
		public virtual ULList Slice(int start, int end) {
			return new ULList(value.GetRange(start, end-start));
		}
	}
}
