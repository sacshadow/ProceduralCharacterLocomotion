using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class DFAManager {
	
	public static Dictionary<string, DFA> data;
	
	public static void Init() {
		data = Resources.LoadAll("DataFromAnimation", typeof(DFA)).Select(x=>x as DFA).ToDictionary(x=>x.name);
	}
	
	public static DFA Find(string dtName) {
		if(data == null) Init();
		try{
			return data[dtName];
		}
		catch (Exception e) {
			Debug.LogError("GetData fail : " + dtName);
			throw e;
		}
	}
	
	public static DFA[] Find(params string[] dtName) {
		if(data == null) Init();
		return Loop.SelectArray(dtName, x=>Find(x));
	}
}
