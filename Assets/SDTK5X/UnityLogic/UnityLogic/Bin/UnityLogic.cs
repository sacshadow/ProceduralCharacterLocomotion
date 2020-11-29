using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;
namespace UnityLogic {

	public static class ULogic  {
		
		public static Dictionary<string,List<Parser>> parser = new Dictionary<string,List<Parser>>();
		
		public static Parser Load(string fileName, string data) {
			if(!parser.ContainsKey(fileName))
				parser[fileName] = new List<Parser>();
			var temp = new Parser(Reader.Read_Input(data));
			parser[fileName].Add(temp);
			return temp;
		}
		
		
		
		
	}
}
