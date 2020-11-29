using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityLisp {
	public static class ULFormat {
	
		public static string Join(List<LispObject> value, string delim, bool printReadable) {
			var cell = new string[value.Count];
			Loop.Count(cell.Length).Do(x=>cell[x] = value[x].ToString(printReadable));
			return String.Join(delim, cell);
		}

		public static string Join(Dictionary<string,LispObject> value, string delim, bool printReadable) {
			List<string> strs = new List<string>();
			foreach (var kvp in value) {
				if (kvp.Key.Length > 0 && kvp.Key[0] == '\u029e') {
					strs.Add(":" + kvp.Key.Substring(1));
				} else if (printReadable) {
					strs.Add("\"" + kvp.Key.ToString() + "\"");
				} else {
					strs.Add(kvp.Key.ToString());
				}
				strs.Add(kvp.Value.ToString(printReadable));
			}
			
			return String.Join(delim, strs.ToArray());
		}

		public static string Print(LispObject value, bool printReadable) {
			return value.ToString(printReadable);
		}

		public static string PrintArgs(ULList args, String sep, bool printReadable) {
			return Join(args.GetValue(), sep, printReadable);
		}

		public static string EscapeString(string value) {
			return Regex.Escape(value);
		}
	}
}
