using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

namespace SDTK.Geometry{
	public static class GeometryPlane {
	
		public static bool IsPointInPolygon(Vector3[] polygon, Vector3 testPoint) {
			bool result = false;
			int j = polygon.Length - 1;
			for (int i = 0; i < polygon.Length; i++) {
				if (polygon[i].z < testPoint.z && polygon[j].z >= testPoint.z || polygon[j].z < testPoint.z && polygon[i].z >= testPoint.z) {
					if (polygon[i].x + (testPoint.z - polygon[i].z) / (polygon[j].z - polygon[i].z) * (polygon[j].x - polygon[i].x) < testPoint.x) {
						result = !result;
					}
				}
				j = i;
			}
			return result;
		}
		public static bool IsPointInPolygon(List<Vector3> polygon, Vector3 testPoint) {
			bool result = false;
			int j = polygon.Count - 1;
			for (int i = 0; i < polygon.Count; i++) {
				if (polygon[i].z < testPoint.z && polygon[j].z >= testPoint.z || polygon[j].z < testPoint.z && polygon[i].z >= testPoint.z) {
					if (polygon[i].x + (testPoint.z - polygon[i].z) / (polygon[j].z - polygon[i].z) * (polygon[j].x - polygon[i].x) < testPoint.x) {
						result = !result;
					}
				}
				j = i;
			}
			return result;
		}
	}
}
