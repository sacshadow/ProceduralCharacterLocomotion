using UnityEngine;
using System.Collections;

namespace SDTK.GUI.Tools{
	public static class General {
		
		
		public static void RelationCheck(SDGUIBase guiObject){
			if(guiObject.transform.parent == null)
				DisableAndWarning(guiObject);
			
			SDGUIBase parent = guiObject.transform.parent.GetComponent<SDGUIBase>();
			
			if(parent == null)
				DisableAndWarning(guiObject);
			
			if(parent != guiObject.getParent)
				Debug.Log("[Test] parent change");
		}
		
		private static void DisableAndWarning(SDGUIBase guiObject){
			guiObject.enabled = false;
			Debug.LogError("This control must parent under a sd-gui object", guiObject);
		}
	}
}
