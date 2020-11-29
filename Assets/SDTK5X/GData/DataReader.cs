using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class DataReader {
	
	public abstract void Close();
	
	public abstract GData ReadClass();
	
	public T ReadClass<T>() where T : GData {
		return (T)ReadClass();
	}
	public T[] ReadClassArray<T>() where T : GData {
		ClassSupportCheck();
		return ReadArray<T>(ReadClass<T>);
	}
	public List<T> ReadClassList<T>() where T: GData {
		ClassSupportCheck();
		return ReadList<T>(ReadClass<T>);
	}
	public Dictionary<int,T> ReadClassDict<T>(Func<T,int> getKey) where T : GData {
		var temp = ReadClassArray<T>();
		return temp.ToDictionary(getKey);
	}
	
	public abstract int ReadInt();
	public abstract int[] ReadIntArray();
	public abstract List<int> ReadIntList();
	
	public abstract bool ReadBool();
	public abstract bool[] ReadBoolArray();
	public abstract List<bool> ReadBoolList();
	
	public abstract float ReadFloat();
	public abstract float[] ReadFloatArray();
	public abstract List<float> ReadFloatList();
	
	public abstract string ReadString();
	public abstract string[] ReadStringArray();
	public abstract List<string> ReadStringList();
	
	public virtual T[] ReadArray<T>(Func<T> ReadProcess) {
		var rt = new T[ReadInt()];
		Loop.Count(rt.Length).Do(x=>rt[x] = ReadProcess());
		return rt;
	}
	
	public virtual List<T> ReadList<T>(Func<T> ReadProcess) {
		return Loop.Count(ReadInt()).Select(x=>ReadProcess());
	}
	
	public virtual void ClassSupportCheck() {}
}
