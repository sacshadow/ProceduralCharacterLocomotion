using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

[AttributeUsage(AttributeTargets.Class)]
public class RFClass : Attribute {
	public string tag = "";
	public RFClass(string tag) {
		this.tag = tag;
	}
}

[AttributeUsage(AttributeTargets.Method)]
public class RFFunc : Attribute {
	public string tag = "";
	public RFFunc(string tag) {
		this.tag = tag;
	}
}
