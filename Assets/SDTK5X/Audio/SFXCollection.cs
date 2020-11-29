using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;


[CreateAssetMenuAttribute(fileName = "SFXCollection", menuName = "Misc/SFXCollection")]
public class SFXCollection : ScriptableObject {

	public AudioClip[] audio;
}
