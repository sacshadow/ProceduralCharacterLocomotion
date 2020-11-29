using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public static class ULCompare {
		
		public static bool IsEqual(LispObject a, LispObject b) {
			Type ota = a.GetType(), otb = b.GetType();
			if (!((ota == otb) || (a is ULList && b is ULList)))
				return false;
			else {
				if (a is Integer)
					return ((Integer)a).GetValue() == ((Integer)b).GetValue();
				else if (a is Symbol)
					return ((Symbol)a).GetName() == ((Symbol)b).GetName();
				else if (a is ULString)
					return ((ULString)a).GetValue() == ((ULString)b).GetValue();
				else if (a is ULList) {
					ULList listA = (ULList)a, listB = (ULList)b;
					if (listA.GetSize() != listB.GetSize())
						return false;
					for (int i=0; i<listA.GetSize(); i++) {
						if (!IsEqual(listA[i], listB[i]))
							return false;
					}
					return true;
				}
				else if (a is HashMap) {
					var aValue = ((HashMap)a).GetValue();
					var bValue = ((HashMap)b).GetValue();
					var aKeys = aValue.Keys;
					var bKeys = bValue.Keys;
					if (aKeys.Count != bKeys.Count)
						return false;
					foreach (var k in aKeys) {
						if (!IsEqual(aValue[k],bValue[k]))
							return false;
					}
					return true;
				}
				else
					return a == b;
			}
		}
	}
}
