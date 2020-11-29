using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public abstract class LispObject {
		public static Constant trueValue = new Constant("true");
		public static Constant falseValue = new Constant("false");
		public static Constant nilValue = new Constant("nil");
		
		private LispObject meta;
		
		public virtual LispObject car {
			get { throw new Exception("Type dose not have car, type " + GetType());}
		}
		public virtual LispObject cdr {
			get { throw new Exception("Type dose not have cdr, type " + GetType());}
		}
		
		
		public virtual LispObject Copy() {
			return (LispObject)this.MemberwiseClone();
		}
		
		public virtual string ToString(bool printReadable) {
			return this.ToString();
		}
		public LispObject GetMeta() { return meta; }
		public LispObject SetMeta(LispObject m) { meta = m; return this; }
		public virtual bool IsList() { return false; }
		
	}
}