using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public static class ULT {
	
		public static ULList List(params int[] value) {
			return new ULList(Loop.SelectArray(value, x=> new Integer(x)));
		}
		
		public static ULList List(params string[] value) {
			return new ULList(Loop.SelectArray(value, x=> new Symbol(x)));
		}
		
		public static ULList Apd(this ULList list, params int[] value) {
			return list.AddRange(Loop.SelectArray(value, x=> new Integer(x)));
		}
		
		public static ULList Apd(this ULList list, params string[] value) {
			return list.AddRange(Loop.SelectArray(value, x=> new Symbol(x)));
		}
	}
}