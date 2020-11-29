using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class KS_LimbStanceMove : KS_LimbGroundMove {
	
	protected override void CalculateFeetMoveOffset() {
		locomotion.bodyStructure.leg_L.offset = feetOffset_L;
		locomotion.bodyStructure.leg_R.offset = feetOffset_R;
	}
	
}
