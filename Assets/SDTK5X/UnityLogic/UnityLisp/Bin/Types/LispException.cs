using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	
	public class ULThrowable : Exception{
		public ULThrowable() : base() { }
		public ULThrowable(string msg) : base(msg) {  }
	}
	
	public class ULError : ULThrowable {
		public ULError(string msg) :base(msg) { }
	}
	
	public class ULContinue : ULThrowable { }
	
	// Thrown by throw function
	public class ULException : ULThrowable {
		LispObject value;
		//string Message;
		public ULException(LispObject value) {
			this.value = value;
		}
		public ULException(string value) :base(value) {
			this.value = new ULString(value);
		}
		public LispObject GetValue() { return value; }
	}
	
	public class ULNilThrw : ULThrowable {
		public LispObject value {
			get{return LispObject.nilValue; }
		}
	}	
	
}
