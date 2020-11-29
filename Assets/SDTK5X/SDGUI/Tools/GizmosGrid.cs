using UnityEngine;
using System.Collections;

namespace SDTK.GUI.Tools{
	public class GizmosGrid {

		public int screenWidth=0, screenHeight=0;
		
		private Vector3 offsetX, offsetY, offsetXY;
		
		public void SetScreenSize(int width, int height){
			screenWidth = width;
			screenHeight = height;
			
			offsetX = Vector3.right*width/100f;
			offsetY = Vector3.up*height/100f;
			offsetXY = new Vector3(width/100f, height/100f);
		}
		
		public void GridOnGizmos(Vector3 managerPosition){
			Gizmos.color = Color.red;
			Gizmos.DrawLine(managerPosition, managerPosition + offsetX);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(managerPosition, managerPosition + offsetY);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(managerPosition+offsetX, managerPosition + offsetXY);
			Gizmos.DrawLine(managerPosition+offsetY, managerPosition + offsetXY);
		}
	}
}