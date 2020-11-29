using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using URD = UnityEngine.Random;

[CreateAssetMenuAttribute(fileName = "HairStyleCollection", menuName = "WJDR/HairStyleCollection")]
public class HairStyleCollection : ScriptableObject {
	
	public static HairStyleCollection Instance {
		get { 
			if(sington == null) 
				sington = Resources.Load<HairStyleCollection>("Misc/HairStyleCollection");
			return sington; 
		}
	}
	
	
	private static HairStyleCollection sington;
	
	public List<Transform> banditHairStyle;
}
