using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

[CreateAssetMenuAttribute(fileName = "GDSTemplate", menuName = "GDS")]
public class GDSTemplate : ScriptableObject {

	[HideInInspector]
	public string savePath = "";
	public TextAsset classTemplate, envTemplate;
	
	
	
}
