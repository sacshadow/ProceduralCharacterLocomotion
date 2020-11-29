using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Function : LispObject {
	
		private Func<ULList, LispObject> fn = null;
		private LispObject ast = null;
		private Env env = null;
		private ULList fparams;
		private bool macro = false;
		public Function(Func<ULList, LispObject> fn) {
			this.fn = fn;
		}
		public Function(LispObject ast, Env env, ULList fparams, Func<ULList, LispObject> fn) {
			this.fn = fn;
			this.ast = ast;
			this.env = env;
			this.fparams = fparams;
		}

		public override string ToString() {
			if (ast != null) {
				return "<fn* " + ULFormat.Print(fparams,true) + " " + ULFormat.Print(ast, true) + ">";
			} else {
				return "<builtin_function " + fn.ToString() + ">";
			}
		}

		public LispObject Apply(ULList args) {
			return fn(args);
		}

		public LispObject GetAst() { return ast; }
		public Env GetEnv() { return env; }
		public ULList GetFParams() { return fparams; }
		public Env GenEnv(ULList args) {
			return new Env(env, fparams, args);
		}
		
		public bool IsMacro() { return macro; }
		public void SetMacro() { macro = true; }
	}
}
