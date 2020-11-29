using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	public class Env {
		public static Env standeredEnv {
			get {
				if(std == null)
					std = ULTools.GetStanderedEnv();
				return std;
			}
		}
		private static Env std;
		
		public int Count {
			get{return data.Keys.Count; }
		}
		
		private Env outer = null;
		public Dictionary<string, LispObject> data = new Dictionary<string, LispObject>();
		
		public Env() { }
		public Env(Dictionary<string, LispObject> data) {this.data = data; }
		public Env(Env outer) {
			this.outer = outer;
		}
		
		public Env(Env outer, ULList binds, ULList exprs) {
			this.outer = outer;
			
			// Debug.Log("binds " + binds.GetSize() + " " + exprs.GetSize());
			
			for (int i=0; i<binds.GetSize(); i++) {
				var sym = (Symbol)binds.Nth(i);
				if (sym.GetName() == "&") {
					Set((Symbol)binds.Nth(i+1), exprs.Slice(i));
					break;
				} else {
					Set(sym, exprs.Nth(i));
				}
			}
		}
		
		public Env Find(Symbol key) {
			if (data.ContainsKey(key.GetName())) {
				return this;
			} else if (outer != null) {
				return outer.Find(key);
			} else {
				return null;
			}
		}

		public LispObject Get(Symbol key) {
			Env e = Find(key);
			if (e == null) {
				throw new ULException("'" + key.GetName() + "' not found");
			} else {
				return e.data[key.GetName()];
			}
		}

		public Env Set(Symbol key, LispObject value) {
			data[key.GetName()] = value;
			return this;
		}
		
		public Env Bind(string key, LispObject value) {
			data[key] = value;
			return this;
		}
		
	}
}

