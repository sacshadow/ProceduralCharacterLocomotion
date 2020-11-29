using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
// using System.Linq;

public class BReader : DataReader {

	public BinaryReader reader;
	
	public static T ReadGData<T>(string path) where T : GData {
		var r = new BReader(path);
		var rt = r.ReadClass<T>();
		r.Close();
		return rt;
	}
	
	public BReader(string fileName) {
		reader = new BinaryReader(File.Open(fileName, FileMode.Open));
	}
	
	public override void Close() {
		reader.Close();
	}
	
	public override GData ReadClass() {
		var type = ReadString();
		if(type == "nil")
			return null;
		
		try {
			var rt = DataEnv.Initialize[type]();
			rt.ReadData(this);
			return rt;
		}
		catch (System.Exception e) {
			Debug.LogError("Class not find " + type);
			throw e;
		}
	}
	
	public override int ReadInt() {
		return reader.ReadInt32();
	}
	public override int[] ReadIntArray() {
		return ReadArray(ReadInt);
	}
	public override List<int> ReadIntList() {
		return ReadList(ReadInt);
	}
	
	public override bool ReadBool() {
		return reader.ReadBoolean();
	}
	public override bool[] ReadBoolArray() {
		return ReadArray(ReadBool);
	}
	public override List<bool> ReadBoolList() {
		return ReadList(ReadBool);
	}
	
	public override float ReadFloat() {
		return reader.ReadSingle();
	}
	public override float[] ReadFloatArray() {
		return ReadArray(ReadFloat);
	}
	public override List<float> ReadFloatList() {
		return ReadList(ReadFloat);
	}
	
	public override string ReadString() {
		return reader.ReadString();
	}
	public override string[] ReadStringArray() {
		return ReadArray(ReadString);
	}
	public override List<string> ReadStringList() {
		return ReadList(ReadString);
	}
}
