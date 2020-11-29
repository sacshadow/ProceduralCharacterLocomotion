using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityLisp;

namespace UnityLogic.UI {
	public static class InputUtiltiy {
	
		public static List<KeyCode> characterKey = new List<KeyCode> {
			KeyCode.Alpha0,KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4,
			KeyCode.Alpha5,KeyCode.Alpha6,KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9,
			
			KeyCode.Q,KeyCode.W,KeyCode.E,KeyCode.R,KeyCode.T,KeyCode.Y,KeyCode.U,KeyCode.I,KeyCode.O,KeyCode.P,
			KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.F,KeyCode.G,KeyCode.H,KeyCode.J,KeyCode.K,KeyCode.L,
			KeyCode.Z,KeyCode.X,KeyCode.C,KeyCode.V,KeyCode.B,KeyCode.N,KeyCode.M,
		};
		
		
		public static bool IsCharacterKey(KeyCode keyCode) {
			
			return characterKey.Contains(keyCode);
		}
		
		
	}
}
