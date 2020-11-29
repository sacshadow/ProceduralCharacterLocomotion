using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public abstract class DataWriter {
	
	public abstract void Close();
	
	public abstract void Write(GData data);
	public abstract void Write(GData[] data);
	public void Write<T>(T[] data) where T : GData {
		WriteArray<T>(data,Write);
	}
	public void Write<T>(List<T> data) where T : GData {
		WriteList<T>(data,Write);
	}
	public void Write<T>(Dictionary<int,T> data) where T : GData {
		var value = data.Values;
		Write(value.Count);
		foreach(var element in value) Write(element);
	}
	
	public abstract void Write(int data);
	public abstract void Write(int[] data);
	public abstract void Write(List<int> data);
	
	public abstract void Write(bool data);
	public abstract void Write(bool[] data);
	public abstract void Write(List<bool> data);
	
	public abstract void Write(float data);
	public abstract void Write(float[] data);
	public abstract void Write(List<float> data);
	
	public abstract void Write(string data);
	public abstract void Write(string[] data);
	public abstract void Write(List<string> data);
	
	public virtual void WriteArray<T>(T[] data, Action<T> WriteProcess) {
		Write(data.Length);
		Loop.ForEach(data, WriteProcess);
	}
	
	public virtual void WriteList<T>(List<T> data, Action<T> WriteProcess) {
		Write(data.Count);
		data.ForEach(WriteProcess);
	}
}
