using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
// using Rand = UnityEngine.Random;

using UnityLisp;

namespace UnityLogic {
	public static class LogicCore {
		
		public static Env GetLogicEnv(Env env) {
			EnvBind(env,"set-data","写入数据", new LogicExecute((x,y)=>SetData(x,y)));
			EnvBind(env,"env-process","环境", new LogicExecute((x,y)=>EnvProcess(x,y)));
			EnvBind(env,"process","执行", new LogicExecute((x,y)=>Process(x,y)));
			EnvBind(env,"evalue","求值", new LogicExecute((x,y)=>Evalue(x,y)));
			EnvBind(env,"eval-ast","求目标值", new LogicExecute((x,y)=>Eval_ast(x,y)));
			EnvBind(env,"logic","写入逻辑", new LogicExecute((x,y)=>SetLogic(x,y)));
			EnvBind(env,"tag","写入标签", new LogicExecute((x,y)=>SetTag(x,y)));
			EnvBind(env,"lisp-call","lisp-call", new LogicExecute((x,y)=>LispCall(x,y)));
			EnvBind(env,"bind","设定",new LogicExecute((x,y)=>Bind(x,y)));
			EnvBind(env,"cond","条件",new LogicExecute((x,y)=>Cond(x,y)));
			EnvBind(env,"chance","概率",new LogicExecute((x,y)=>Chance(x,y)));
			return env;
		}
		
		public static void EnvBind(Env env, string en, string cn, LispObject value) {
			env.Bind(en, value);
			env.Bind(cn, value);
		}
		
		public static LispObject Bind(ULList ast, Env env) {
			var list = Eval_list_ele(ast, env);
			Debug.Log(list);
			// for(int i=0; i<list.GetSize(); i+=2) {
				// env.Bind(list[i*2], list[i*2+1]);
			// }
			
			return LispObject.nilValue;
		}
		
		public static LispObject Chance(ULList ast, Env env) {
			
			var k = ((ULString)ast[0]).GetValue();
			float f = 0;
			
			if(k[k.Length-1] == '%')
				f = float.Parse(k.Substring(0,k.Length-1))/100f;
			else
				f = float.Parse(k);
			
			return Core.Compare(Rand.value < f);
		}
		
		public static LispObject Cond(ULList ast, Env env) {
			Debug.Log(ast);
			
			
			for(int i =0; i< ast.GetSize(); i++) { 
				var list = (ULList)ast[i];
				Debug.Log(list);
				var cond = Eval_ast(list[0], env);
				Debug.Log(cond);
				if(cond == LispObject.nilValue || cond == LispObject.falseValue)
					continue;
				
				return Eval_ast(list.Rest(), env);
			}
			
			return LispObject.nilValue;
		}
		
		public static LispObject LispCall(ULList ast, Env env) {
			var list = Eval_list_ele(ast, env);
			return ULisp.Eval(list, env);
		}
		
		public static LispObject SetLogic(ULList ast, Env env) {
			
			ast.GetValue().ForEach(x=>SetTag(((ULList)x).Rest(), env));
			
			return LispObject.nilValue;
		}
		
		public static LispObject SetTag(ULList ast, Env env) {
			ast.Rest().GetValue().ForEach(x=>SetData((ULList)(((ULList)x)[3]),env));
			return LispObject.nilValue;
		}
		
		public static LispObject SetData(ULList ast, Env env) {
			Debug.Log("bind " + ((ULString)ast[0]).GetValue() + " " + ast.Rest().ToString());
			env.Bind(((ULString)ast[0]).GetValue(), ast.Rest());
			return ast[0];
		}
		
		public static LispObject EnvProcess(ULList ast, Env env) {
			return Process(ast, new Env(env));
		}
		
		public static LispObject Process(ULList ast, Env env) {
			var data = env.Get((Symbol)ast[0]);
			
			if(data is ULVector)
				data = PickFrom((ULVector)data);
			
			if(data is ULList) {
				var list = ((ULList) data).GetValue();
				foreach(var element in list) {
					if(element is ULList)
						SetData((ULList)element, env);
					else
						throw new Exception("Process error " + element.ToString());
				}
			}
			
			return Evalue(ast[1], env);
		}
		
		public static LispObject Evalue(LispObject key, Env env) {
			// Debug.Log("Evalue " + key);
			// Debug.Log(key.GetType());
			var data = env.Get((Symbol)key);
			
			return Eval_ast(data, env);
		}
		
		public static LispObject Eval_ast(LispObject ast, Env env) {
			// Debug.Log("Evalue " + ast);
			// Debug.Log(ast.GetType());
			
			if(ast is ULVector) {
				ast = PickFrom((ULVector)ast);
				Debug.Log(ast);
			}
			
			if(ast is Symbol) {
				var syb = ((Symbol)ast).ToString();
				if(syb[0] == '#') {
					return new Symbol(syb.Substring(1,syb.Length-1));
				}
				return Evalue(ast, env);
			}
			
			if(ast is ULList)
				return Eval_list(((ULList)ast).Slice(0), env);
			
			if(ast is ULString) {
				var str = ((ULString)ast).GetValue();
				
				if(str[0] == '@')
					return ast;
				
				if(str.IndexOf("#") == -1)
					return ast;
				
				var s = str.Split(' ');
				var list = new List<LispObject>();
				
				for(int i =0; i<s.Length; i++) {
					if(s[i][0] == '#')
						list.Add(new Symbol(s[i].Substring(1,s[i].Length-1)));
					else
						list.Add(new ULString(s[i]));
				}
				
				return Eval_list(new ULList(list), env);
			}
			return ast;
		}
		
		public static LispObject Eval_list(ULList list, Env env) {
			if(list[0] is Symbol) {
				var syb = ((Symbol)list[0]).ToString();
				if(syb[0] != '#') {
					var f = env.Get((Symbol)list[0]);
					if(f is LogicExecute) {
						return ((LogicExecute)f).Apply(list.Rest(), env);
					}
				}
			}
			
			return Eval_list_ele(list,env);
		}
		
		public static LispObject Eval_list_ele(ULList list, Env env) {
			var l = list.GetValue();
			var rt = new List<LispObject>();
			
			for(int i=0; i<l.Count; i++) {
				var ast = Eval_ast(l[i], env);
				if(ast.GetType() == typeof(ULList))
					rt.AddRange(((ULList)ast).GetValue());
				else
					rt.Add(ast);
			}
			return new ULList(rt);
		}
		
		public static LispObject PickFrom(ULVector optional) {
			var temp = optional.GetValue();
			if(temp.Count == 0)
				return LispObject.nilValue;
			
			return temp[Rand.Range(0,temp.Count*100)%temp.Count];
		}
		
		
	}
}
