using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KeyState {
	
	public KeyCode key = KeyCode.None;
	
	public KeyState() {}
	
	public KeyState(KeyCode key) {
		this.key = key;
	}
	
	public virtual bool IsDown() {
		return Legal(Input.GetKeyDown);
	}
	
	public virtual bool IsPress() {
		return Legal(Input.GetKey);
	}
	
	public virtual bool IsUp() {
		return Legal(Input.GetKeyUp);
	}
	
	public virtual bool Legal(Func<KeyCode,bool> Process) {
		return Process(key);
	}
}
