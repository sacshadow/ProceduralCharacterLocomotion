using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

[Serializable]
public class TestA : GData {
	
	public int a = 0;
	public float b = 3.14f;
	public string c = "asdf";
	public TestB d = new TestB();
	
	public int[] aa = new int[]{1,2,3,4};
	public float[] bb = new float[]{3.14f,1,2,3,4};
	public string[] cc = new string[]{"asdf"};
	public TestB[] dd = new TestB[]{new TestB()};
	
	public override void ReadData(DataReader reader) {
		a = reader.ReadInt();
		b = reader.ReadFloat();
		c = reader.ReadString();
		d = reader.ReadClass<TestB>();
		
		aa = reader.ReadIntArray();
		bb = reader.ReadFloatArray();
		cc = reader.ReadStringArray();
		dd = reader.ReadClassArray<TestB>();
		
	}
	
	public override void WriteData(DataWriter writer) {
		writer.Write(a);
		writer.Write(b);
		writer.Write(c);
		writer.Write(d);
		
		writer.Write(aa);
		writer.Write(bb);
		writer.Write(cc);
		writer.Write(dd);
	}
	
}
