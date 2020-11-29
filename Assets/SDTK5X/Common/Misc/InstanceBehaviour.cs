using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public abstract class InstanceBehaviour<T> : MonoBehaviour where T:MonoBehaviour {
	protected static T singleton;
	public static T Instance {
		get {
			if(singleton != null)
				return singleton;
			else
				throw new System.Exception("Not instantiated : " + typeof(T).ToString());
		}
	}
	
	protected virtual void StopRunning() {
		enabled = false;
		throw new System.Exception("can not have more than one instance");
	}
	
	protected virtual void Awake() {
		if(singleton != null && singleton != this)
			StopRunning();
		else
			singleton = this as T;
	}
	
}
