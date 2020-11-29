using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Linq;

public class BWriter : DataWriter {
	
	public BinaryWriter writer;
	
	public static void WriteGData(string path, GData data) {
		var w = new BWriter(path);
		w.Write(data);
		w.Close();
	}
	
	public BWriter(string fileName) {
		writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
	}
	
	public override void Close() {
		writer.Close();
	}
	
	public override void Write(GData data) {
		if(data == null) {
			Write("nil");
			return;
		}
		
		Write(data.GetType().ToString());
		data.WriteData(this);
	}
	public override void Write(GData[] data) {
		WriteArray<GData>(data,Write);
	}
	
	public override void Write(int data) {
		writer.Write(data);
	}
	public override void Write(int[] data) {
		WriteArray(data,Write);
	}
	public override void Write(List<int> data) {
		WriteList(data,Write);
	}
	
	public override void Write(bool data) {
		writer.Write(data);
	}
	public override void Write(bool[] data) {
		WriteArray(data,Write);
	}
	public override void Write(List<bool> data) {
		WriteList(data,Write);
	}
	
	public override void Write(float data) {
		writer.Write(data);
	}
	public override void Write(float[] data) {
		WriteArray(data,Write);
	}
	public override void Write(List<float> data) {
		WriteList(data,Write);
	}
	
	public override void Write(string data) {
		if(data == null) data = "";
		writer.Write(data);
	}
	public override void Write(string[] data) {
		WriteArray(data,Write);
	}
	public override void Write(List<string> data) {
		WriteList(data,Write);
	}
	
	
}
