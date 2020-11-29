using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

// using Rand = UnityEngine.Random;

using UnityLisp;

namespace UnityLogic {
	public static class LogicCore2 {
		
		public static Function ExpF;
		
		public static Env GetLogicEnv(Env env) {
			Action<string, Func<ULList, LispObject>> BM = (x,y)=> BindMacro(env,x,y);
			Action<string, Func<ULList, LispObject>> BF = (x,y)=> BindFunc(env,x,y);
			
			ExpF = new Function(x=>TxtExpand(x[0]));
			
			BM("logic", ReadLogic);
			BM("tag", ReadTag);
			BM("node", ReadNode);
			
			BM("bd", BindData);
			BM("text-expand", TextExpand);
			BF("pick", Pick);
			BF("join", Join);
			
			return env;
		}
		
		public static void BindFunc(Env env, string key, Func<ULList, LispObject> fn) {
			var func = new Function(fn);
			env.Bind(key, func);
		}
		
		public static void BindMacro(Env env, string key, Func<ULList, LispObject> fn) {
			var macro = new Function(fn);
			macro.SetMacro();
			
			env.Bind(key, macro);
		}
		
		public static LispObject Symb(string key) {
			return new Symbol(key);
		}
		
		public static LispObject Block(params LispObject[] apd) {
			return new ULList(apd);
		}
		
		public static List<LispObject> Copy(ULList list) {
			return new List<LispObject>(list.GetValue());
		}
		
		public static List<LispObject> Copy(ULList list, int start) {
			return Copy(list, start, list.GetSize()-start);
		}
		
		public static List<LispObject> Copy(ULList list, int start, int end) {
			return list.GetValue().GetRange(start, end);
		}
		
		public static LispObject TxtExpand(LispObject ast) {
			Debug.Log(ast);
			
			if(ast is ULVector)
				ast = PickFromVector((ULVector)ast);
				
			if(ast is ULString)
				ast = ExpandString((ULString)ast);
			
			if(ast is ULList) {
				var ls = new List<LispObject>();
				ls.Add(Symb("join"));
			
				var list = ((ULList)ast).GetValue();
				list.ForEach(x=> ls.Add(TxtExpand(x)));
				
				return new ULList(ls);
			}
			else if(ast is Symbol)
				return Block(Symb("do"), Block(ExpF, ast));
			
			return ast;
		}
		
		public static LispObject ExpandString(ULString ulStr) {
			var str = ulStr.GetValue();
				
			if(str[0] == '@')
				return ulStr;
			
			if(str.IndexOf("#") == -1)
				return ulStr;
			
			var s = str.Split(' ');
			var list = new List<LispObject>();
			
			for(int i =0; i<s.Length; i++) {
				if(s[i][0] == '#')
					list.Add(new Symbol(s[i].Substring(1,s[i].Length-1)));
				else
					list.Add(new ULString(s[i]));
			}
			
			return new ULList(list);
		}
		
		public static LispObject PickFromVector(ULVector vector) {
			var size = vector.GetSize();
			if( size == 0)
				return LispObject.nilValue;
			
			return vector[Rand.Range(0,size*100)%size];
		}
		
		public static LispObject PickElement(LispObject val) {
			if(val is ULVector)
				return PickFromVector((ULVector)val);
			if(val is ULList) {
				var ls = (ULList)val;
				var rt = new List<LispObject>();
				ls.GetValue().ForEach(x=>JoinElement(rt, PickElement(x)));
				
				return new ULList(rt);
			}
			
			return val;
		}
		
		public static void JoinElement(List<LispObject> rt, LispObject join) {
			if(join is ULList)
				rt.AddRange(((ULList)join).GetValue());
			else
				rt.Add(join);
		}
		
		
		public static LispObject Expand(LispObject ast) {
			
			if(ast is ULString)
				return ExpandString((ULString)ast);
			else
				return ast;
		}
		
		/*********************************CORE FUNC************************************/
		
		
		
		public static LispObject Join(ULList list) {
			// Debug.Log(list);
			var s = "";
			
			list.GetValue().ForEach(x=> s += ((ULString)x).GetValue());
			
			return new ULString(s);
		}
		
		public static LispObject Pick(ULList list) {
			return PickElement(list[0]);
		}
		
		// public static LispObject LogicProcess(ULList list) {
			// return 
		// }
		
		public static LispObject TextExpand(ULList list) {
			return TxtExpand(list[0]);
		}
		
		public static LispObject ReadLogic(ULList list) {
			var copy = Copy(list);
			copy.Insert(0,Symb("do"));
			return new ULList(copy);
		}
		
		public static LispObject ReadTag(ULList list) {
			var copy = Copy(list,1);
			copy.Insert(0,Symb("do"));
			copy.Add(LispObject.nilValue);
			return new ULList(copy);
		}
		
		public static LispObject ReadNode(ULList list) {
			var data = (ULList)list[2];
			
			if(data.GetSize() > 1) {
				var copy = Copy(data);
				copy.Insert(0,Symb("bd"));
				return new ULList(copy);
			}
			
			return LispObject.nilValue;
		}
		
		public static LispObject BindData(ULList list) {
			return Block(Symb("def!"), list[0], Block(Symb("quote"),list.Rest()));
		}
		
		
		
		
		/*********************************CORE FUNC************************************/
	}
}