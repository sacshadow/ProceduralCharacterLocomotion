using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;

namespace UnityLogic {
	public class Parser {
		
		public Env rootEnv;	
		
		public Parser(LispObject logicData) {
			rootEnv = ULTools.GetNewEnv();
			// LogicCore.GetLogicEnv(rootEnv);
			LogicCore2.GetLogicEnv(rootEnv);
			// Eval(logicData, rootEnv);
			ULisp.Eval(logicData, rootEnv);
		}
		
		// public LispObject Eval(LispObject ast, Env env) {
			// if(ast.GetType() != typeof(ULList))
				// return ast;
			
			// var list = (ULList)ast;
			
			// if(!(list[0] is Symbol))
				// return ast;
			
			// var symbol = (Symbol)list[0];
			
			// var le = (LogicExecute)env.Get(symbol);
			// return le.Apply(list.Rest(), env);
		// }
		
		
		
	}
}
