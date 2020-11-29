using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;


using UnityLisp;
namespace UnityLogic {
	public class Definition : LogicObject {
		
		public Env env;
		public LispObject value;
		
		public Func<ULList, LispObject> OnEval;
		
		
		public Definition(LispObject define) {
			if(define is ULList) {
				var list = (ULList)define;
				if(list[0] is ULString && Core.B_IsKeyword((ULString)list[0]))
					define = new HashMap(list);
			}
				
			value = define;
			if(value is Function)
				OnEval = Fn;
			else if(value is ULVector)
				OnEval = Pick;
			else if(value is HashMap)
				OnEval = Hm;
			else
				OnEval = Val;
		}
		
		public LispObject Fn(ULList arg) {
			var ast = new ULList(value);
			ast.AddRange(arg);
			return ULisp.Eval(ast, env);
		}
		
		public LispObject Pick(ULList arg) {
			var v = ((ULVector)value).GetValue();
			return v[UnityEngine.Random.Range(0,v.Count*100)%v.Count];
		}
		
		public LispObject Hm(ULList arg) {
			return Core.Get(new ULList(value, arg[0]));
		}
		
		public LispObject Val(ULList arg) {
			return value;
		}
		
		
		public override LispObject Eval(ULList arg) {
			return OnEval(arg);
		}
		
	}
}