using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public static class ULTools  {
		
		public static Env GetStanderedEnv() {
			var stdEnv = new Env(Core.function);
			Func<string, LispObject> RE = str => ULisp.Eval(ULisp.Parse(str),stdEnv);
			
			stdEnv.Bind("eval", new Function(arg=>ULisp.Eval(arg[0], stdEnv)));
			
			RE("(def! not (fn* (a) (if a false true)))");
			RE("(def! load-file (fn* (f) (eval (read-string (str \"(do \" (slurp f) \")\")))))");
			RE("(defmacro! cond (fn* (& xs) (if (> (count xs) 0) (list 'if (first xs) (if (> (count xs) 1) (nth xs 1) (throw \"odd number of forms to cond\")) (cons 'cond (rest (rest xs)))))))");
            RE("(defmacro! or (fn* (& xs) (if (empty? xs) nil (if (= 1 (count xs)) (first xs) `(let* (or_FIXME ~(first xs)) (if or_FIXME or_FIXME (or ~@(rest xs))))))))");

			return stdEnv;
		}
		
		public static Env GetNewEnv() {
			return new Env(Env.standeredEnv);
		}
		
		
	}
}

