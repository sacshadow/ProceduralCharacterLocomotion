using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

[Serializable]
public class TestB : GData {
	
	public int a = 101;
	public float b = 5.83f;
	public string c = "zaqxsw";
	
	public override void ReadData(DataReader reader) {
		a = reader.ReadInt();
		b = reader.ReadFloat();
		c = reader.ReadString();
	}
	
	public override void WriteData(DataWriter writer) {
		writer.Write(a);
		writer.Write(b);
		writer.Write(c);
	}
}
