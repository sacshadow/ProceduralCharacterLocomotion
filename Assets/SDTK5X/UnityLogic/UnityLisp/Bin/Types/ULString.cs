using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class ULString : LispObject {
		public const char kwMark = '\u029e';
	
		private string value;
		public ULString(string v) { value = v; }
		public new ULString Copy() { return this; }

		public string GetValue() { return value; }
		public override string ToString() {
			return "\"" + value + "\"";
		}
		public override string ToString(bool printReadable) {
			if (value.Length > 0 && value[0] == kwMark) {
				return ":" + value.Substring(1);
			} else if (printReadable) {
				return "\"" + value.Replace("\\", "\\\\")
					.Replace("\"", "\\\"")
					.Replace("\n", "\\n") + "\"";
			} else {
				return value;
			}
		}
	}
}
