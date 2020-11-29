using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;

namespace UnityLogic {

	public class LogicExecute : LispObject {
		
		public bool isEval = false;
		private Func<ULList, Env, LispObject> fn = null;
		
		public LogicExecute(Func<ULList, Env, LispObject> fn, bool isEval = false) {
			this.isEval = isEval;
			this.fn = fn;
		}
		
		public LispObject Apply(ULList args, Env env) {
			return fn(args, env);
		}
		
		
		
	}
}