using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class TestRW : MonoBehaviour {
	
	public TestA testA;
	
	// Use this for initialization
	void Start () {
		DataEnv.InitEnv();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if(GUILayout.Button("Read"))
			testA = BReader.ReadGData<TestA>(Application.dataPath + "/test.bin");
		if(GUILayout.Button("Write"))
			BWriter.WriteGData(Application.dataPath + "/test.bin",testA);
	}
	
	
}
