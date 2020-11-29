using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;


public abstract class GData {
	
	public abstract void ReadData(DataReader reader);
	
	public abstract void WriteData(DataWriter writer);
}
