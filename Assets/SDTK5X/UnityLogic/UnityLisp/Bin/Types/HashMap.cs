using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class HashMap : LispObject {
		private Dictionary<string, LispObject> value;
		
		public HashMap(Dictionary<string, LispObject> val) {
			value = val;
		}
		public HashMap(ULList lst) {
			value = new Dictionary<String, LispObject>();
			AddRange(lst);
		}
		public new HashMap Copy() {
			var new_self = (HashMap)this.MemberwiseClone();
			new_self.value = new Dictionary<string, LispObject>(value);
			return new_self;
		}

		public Dictionary<string, LispObject> GetValue() { return value; }

		public override string ToString() {
			return "{" + ULFormat.Join(value, " ", true) + "}";
		}
		public override string ToString(bool printReadable) {
			return "{" + ULFormat.Join(value, " ", printReadable) + "}";
		}

		public HashMap AddRange(ULList lst) {
			for (int i=0; i<lst.GetSize(); i+=2) {
				value[((ULString)lst[i]).GetValue()] = lst[i+1];
			}
			return this;
		}

		public HashMap RemoveRange(ULList lst) {
			for (int i=0; i<lst.GetSize(); i++) {
				value.Remove(((ULString)lst[i]).GetValue());
			}
			return this;
		}
		
	}
}
