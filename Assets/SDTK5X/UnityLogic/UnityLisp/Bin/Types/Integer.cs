using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Integer : LispObject {
		
		private Int64 value;
		
		public Integer(Int64 v) { value = v; }
		public new Integer Copy() { return this; }

		public Int64 GetValue() { return value; }
		public override string ToString() {
			return value.ToString();
		}
		public override string ToString(bool printReadable) {
			return value.ToString();
		}
		public static Constant operator <(Integer a, Integer b) {
			return a.GetValue() < b.GetValue() ? LispObject.trueValue : LispObject.falseValue;
		}
		public static Constant operator <=(Integer a, Integer b) {
			return a.GetValue() <= b.GetValue() ? LispObject.trueValue : LispObject.falseValue;
		}
		public static Constant operator >(Integer a, Integer b) {
			return a.GetValue() > b.GetValue() ? LispObject.trueValue : LispObject.falseValue;
		}
		public static Constant operator >=(Integer a, Integer b) {
			return a.GetValue() >= b.GetValue() ? LispObject.trueValue : LispObject.falseValue;
		}
		public static Integer operator +(Integer a, Integer b) {
			return new Integer(a.GetValue() + b.GetValue());
		}
		public static Integer operator -(Integer a, Integer b) {
			return new Integer(a.GetValue() - b.GetValue());
		}
		public static Integer operator *(Integer a, Integer b) {
			return new Integer(a.GetValue() * b.GetValue());
		}
		public static Integer operator /(Integer a, Integer b) {
			return new Integer(a.GetValue() / b.GetValue());
		}
		
	}
}
