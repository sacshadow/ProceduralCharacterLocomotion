using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Loop {
	
	public int begin = 0;
	public int end = 0;

#region static	
	public static Loop Count(int index) {
		return new Loop{begin = 0, end = index};
	}
	
	public static Loop Between(int bg, int ed) {
		return new Loop{begin = bg, end = ed};
	}
	
	public static void Seq(params Action[] Execute) {
		ForEach(Execute, exe=>exe());
	}
	
	public static R Calculate<T,R>(T[] array, R defaultValue, Func<T,R,R> Process) {
		if(array.Length == 0)
			throw new System.Exception("Empty array");
	
		R rt = defaultValue;
		for(int i=0; i<array.Length; i++) rt = Process(array[i], rt);
		return rt;
	}
	
	public static R Calculate<T,R>(List<T> list, R defaultValue, Func<T,R,R> Process) {
		if(list.Count == 0)
			throw new System.Exception("Empty list");
		
		R rt = defaultValue;
		for(int i=0; i<list.Count; i++) rt = Process(list[i], rt);
		return rt;
	}
	
	public static T Calculate<T>(T[] array, Func<T,T,T> Process) {	
		if(array.Length == 0)
			throw new System.Exception("Empty array");
		
		T rt = array[0];
		for(int i=1; i<array.Length; i++) rt = Process(rt, array[i]);
		return rt;
	}
	
	public static T Calculate<T>(List<T> list, Func<T,T,T> Process) {
		if(list.Count == 0)
			throw new System.Exception("Empty list");
		
		T rt = list[0];
		for(int i=1; i<list.Count; i++) rt = Process(rt, list[i]);
		return rt;
	}
	
	public static T CalculateIE<T>(IEnumerable<T> input, T defaultValue, Func<T,T,T> Process) {
		T rt = defaultValue;
		foreach(T t in input) rt = Process(rt,t);
		return rt;
	}
	
	public static void Enumerate<T>(Action<T> Process, params T[] array) {
		for(int i=0; i<array.Length; i++) Process(array[i]);
	}
	
	public static void ForEach<T>(T[] array, Action<T> Process) {
		for(int i=0; i<array.Length; i++) Process(array[i]);
	}
	
	public static void ForEverTwo<T>(T[] array, Action<T,T> Process) {
		for(int i=1; i<array.Length; i++) Process(array[i-1], array[i]);
	}
	public static void ForEverTwo<T>(List<T> list, Action<T,T> Process) {
		for(int i=1; i<list.Count; i++) Process(list[i-1], list[i]);
	}
	
	public static void ForIndexEachIn<T>(T[] array, Action<int,T> Process) {
		for(int i=0; i<array.Length; i++) Process(i,array[i]);
	}
	
	public static void ForIndexEachIn<T>(List<T> list, Action<int,T> Process) {
		for(int i=0; i<list.Count; i++) Process(i,list[i]);
	}
	
	public static List<K> SelectEach<T,K>(T[] array, Func<T,K> Collect) {
		List<K> rt = new List<K>();
		for(int i =0; i<array.Length; i++) rt.Add(Collect(array[i]));
		return rt;
	}
	public static List<K> SelectEach<T,K>(List<T> list, Func<T,K> Collect) {
		List<K> rt = new List<K>();
		for(int i =0; i<list.Count; i++) rt.Add(Collect(list[i]));
		return rt;
	}
	
	public static K[] SelectArray<T,K>(T[] array, Func<T,K> Collect) {
		K[] rt = new K[array.Length];
		ForIndexEachIn(array,(i,x)=>rt[i] = Collect(x));
		return rt;
	}
	
	public static K[] SelectArray<T,K>(List<T> list, Func<T,K> Collect) {
		K[] rt = new K[list.Count];
		ForIndexEachIn(list,(i,x)=>rt[i] = Collect(x));
		return rt;
	}
	
	public static K[] SelectArray<T,K>(Func<T,K> Collect, params T[] array) {
		return SelectArray(array, Collect);
	}
	
	public static T[] Array<T>(params T[] element) {
		return element;
	}
#endregion	
	
#region functions
	public void Do(Action<int> Process) {
		for(int i = begin; i<end; i++) Process(i);
	}
	
	public List<T> Select<T>(Func<int,T> Collect) {
		List<T> rt = new List<T>();
		for(int i = begin; i<end; i++) rt.Add(Collect(i));
		return rt;
	}
	
	public T[] SelectArrayElement<T>(Func<int, T> Collect) {
		T[] rt = new T[end - begin];
		for(int i = 0; i<rt.Length; i++) rt[i] = Collect(i + begin);
		return rt;
	}
#endregion	
	
	
}
