using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityLisp {
	
	public static class ULisp {
		public static string fnMark = "__<*fn*>__";
	
		public static LispObject Parse(string input) {
			return Reader.Read_Input(input);
		}
		
		// static int indent = 0;
		
		// public static string Ind() {
			// var rt = "";
			// Loop.Count(indent).Do(x=>rt += "\t");
			// indent ++;
			// return rt;
		// }
		
		// public static string ent() {
			// var rt = "";
			// indent --;
			// Loop.Count(indent).Do(x=>rt += "\t");
			// return rt;
		// }
		
		public static LispObject Eval(LispObject orig_ast, Env env) {
			// Debug.Log("Eval: " + ULFormat.Print(orig_ast, true));
			var crtAst = orig_ast;
			var crtEnv = env;
			// LispObject evalOut;
			
			Action<LispObject, Env> TOC = (x,y)=>{
				crtAst = x;
				crtEnv = y;
			};
			
			
			// try{
			//toc;
			while(true) {
				// evalOut = null;
				// Debug.Log(Ind() + "Eval: " + ULFormat.Print(crtAst, true));
				
				if(!crtAst.IsList()) {
					
					return Eval_Ast(crtAst, crtEnv);
					// evalOut = Eval_Ast(crtAst, crtEnv);
					// Debug.Log(ent() + "Out: " + evalOut.ToString() + " type " + evalOut.GetType());
					// return evalOut;
				}
				
				var expanded = Macroexpand(crtAst, crtEnv);
				if(!expanded.IsList()) {
					
					return Eval_Ast(expanded, crtEnv);
					// evalOut = Eval_Ast(expanded, crtEnv);
					// Debug.Log(ent() + "Out: " + evalOut.ToString() + " type " + evalOut.GetType());
					// return evalOut;
				}
				
				ULList ast = (ULList)expanded;
				if(ast.GetSize() == 0) {
					
					return ast;
					// evalOut =  ast;
					// Debug.Log(ent() + "Out: " + evalOut.ToString() + " type " + evalOut.GetType());
					// return evalOut;
				}
				
				LispObject a0 = ast[0];
				var a0sym = a0 is Symbol ? ((Symbol)a0).GetName() : fnMark;
				
				// Debug.Log(ast);
				
				switch(a0sym) {
					case "def!": return Define(ast[1], ast[2], crtEnv);
					// case "def!": evalOut =  Define(ast[1], ast[2], crtEnv);break;
					case "let*": Let((ULList)ast[1], ast[2], crtEnv, TOC); break;
					case "quote": return ast[1];
					// case "quote": evalOut = ast[1];break;
					case "quasiquote" : crtAst = Quasiquote(ast[1]); break;
					case "defmacro!" : return DefMacro(ast[1], ast[2], crtEnv);
					// case "defmacro!" : evalOut = DefMacro(ast[1], ast[2], crtEnv);break;
					case "macroexpand": return Macroexpand(ast[1], crtEnv);
					// case "macroexpand": evalOut = Macroexpand(ast[1], crtEnv); break;
					case "do": Do(ast.Rest(), crtEnv, TOC); break;
					case "if": return If(ast.Rest(), crtEnv);
					// case "if": evalOut = If(ast.Rest(), crtEnv);break;
					case "fn*": return Fn(ast.Rest(), crtEnv);
					// case "fn*": evalOut = Fn(ast.Rest(), crtEnv); break;
					default: { 
						try {
							var el = (ULList)Eval_Ast(ast, crtEnv);
							var f = (Function)el[0];
							var fnast = f.GetAst();
							if(fnast != null)
								TOC(fnast,f.GenEnv(el.Rest()));
							else
								return f.Apply(el.Rest());
								// evalOut= f.Apply(el.Rest());
							break;
						}
						catch(Exception e) {
							Debug.LogError("Eval " + orig_ast.ToString(true));
							Debug.LogError("Eval fn* " + ((ULList)Eval_Ast(ast, crtEnv)).ToString(true));
							throw e;
						}
					}
				}
				
				// if(evalOut != null) {
					// Debug.Log(ent() + "Out: " + evalOut.ToString() + " type " + evalOut.GetType());
					// return evalOut;
				// }
				
			//Toc loop
			}
			
			// }
			// catch (Exception e) {
				// Debug.LogError(orig_ast.ToString(true));
				// throw e;
			// }
		}
		
		public static bool IsMacroCall(LispObject ast, Env env) {
			if(ast is ULList) {
				LispObject a0 = ((ULList)ast)[0];
				if((a0 is Symbol) && (env.Find((Symbol)a0) != null)) {
					LispObject mac = env.Get((Symbol)a0);
					return (mac is Function) && ((Function)mac).IsMacro();
				}
			}
			return false;
		}
		
		public static LispObject Macroexpand(LispObject ast, Env env) {
			while(IsMacroCall(ast, env)) {
				// Debug.Log("IsMacroCall " + ast.ToString());
				Symbol a0 = (Symbol)((ULList)ast)[0];
				Function mac = (Function) env.Get(a0);
				ast = mac.Apply(((ULList)ast).Rest());
			}
			return ast;
		}
		
		public static LispObject DefMacro(LispObject a1, LispObject a2, Env env) {
			var res = Eval(a2, env);
			((Function)res).SetMacro();
			env.Set((Symbol)a1, res);
			return res;
		}
		
		public static bool IsPair(LispObject x) {
			return x is ULList && ((ULList)x).GetSize() > 0;
		}
		
		public static LispObject Quasiquote(LispObject ast) {
			// Debug.Log("Quasiquote " + ast.ToString());
		
			if(!IsPair(ast))
				return new ULList(new Symbol("quote"), ast);
			else {
				LispObject a0 = ((ULList)ast)[0];
				if((a0 is Symbol) && (((Symbol)a0).GetName() == "unquote"))
					return ((ULList)ast)[1];
				else if(IsPair(a0)) {
					LispObject a00 = ((ULList)a0)[0];
					if((a00 is Symbol) && (((Symbol)a00).GetName() == "splice-unquote")) {
						return new ULList(
							new Symbol("concat"), 
							((ULList)a0)[1],
							Quasiquote(((ULList)ast).Rest()));
					}
				}
				
				var rest = ((ULList)ast).Rest();
				return new ULList(new Symbol("cons"),Quasiquote(a0),Quasiquote(rest));
			}
		}
		
		public static LispObject Fn(ULList list, Env env) {
			var crtEnv = env;
			var param = (ULList) list[0];
			var body = list[1];
			return new Function(body, env, param,
				args=>Eval(body, new Env(crtEnv, param, args)));
		}
		
		//TODO: Support TOC
		public static LispObject If(ULList list, Env env) {
		// public static void If(ULList list, Env env, Action<LispObject, Env> TOC) {
			var cond = Eval(list[0], env);
			if(cond == LispObject.nilValue || cond == LispObject.falseValue) {
				if(list.GetSize() > 2)
					return Eval(list[2], env);
					// TOC(list[2], env);
				else
					return LispObject.nilValue;
					// throw new ULNilThrw();
					// TOC(LispObject.nilValue, env);
			}
			else
				return Eval(list[1], env);
				// TOC(list[1], env);
		}
		
		// public static LispObject Do(ULList list, Env env) {
		public static void Do(ULList list, Env env, Action<LispObject, Env> TOC) {
			var temp = (ULList)Eval_Ast(list, env);
			// return temp[temp.GetSize()-1];
			TOC(temp[temp.GetSize()-1], env);
		}
		
		public static LispObject Define(LispObject key, LispObject value, Env env) {
			var res = Eval(value, env);
			env.Set((Symbol)key, res);
			return res;
		}
		
		// public LispObject void Let(ULList kvp, LispObject ast, Env env) {
		public static void Let(ULList kvp, LispObject ast, Env env, Action<LispObject, Env> TOC) {
			var let_env = new Env(env);
			for(int i =0; i< kvp.GetSize(); i+= 2) {
				let_env.Set((Symbol)kvp[i], Eval(kvp[i+1], let_env));
			}
			// return Eval(ast, let_env);
			TOC(ast, let_env);
		}
		
		public static LispObject Eval_Ast(LispObject ast, Env env) {
			// Debug.Log("Eval_Ast " + ast.ToString(true) + " type " + ast.GetType());
		
			if(ast is Symbol) {
				// var get = env.Get((Symbol)ast);
				// Debug.Log("env : " + ast.ToString() + " " + get.ToString(true));
				return env.Get((Symbol)ast);
			}
			else if(ast is ULList) {
				var old_lst = (ULList)ast;
				var new_lst = ast.IsList() ? new ULList() : new ULVector();
				foreach(var lo in old_lst.GetValue()) {
					new_lst.AddRange(Eval(lo,env));
				}
				return new_lst;
			}
			else if(ast is HashMap) {
				var new_dict = new Dictionary<string, LispObject>();
				foreach(var kvp in ((HashMap)ast).GetValue()) {
					new_dict.Add(kvp.Key, Eval(kvp.Value, env));
				}
				return new HashMap(new_dict);
			}
			else
				return ast;
		}
	}
}
	
