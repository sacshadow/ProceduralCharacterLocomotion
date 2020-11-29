using UnityEngine;
using System;
using System.Collections;

namespace SDTK.Math{
	public static class Bezier {

		public static Vector3 Po2Curve(Vector3 p0, Vector3 p1, Vector3 m0, float t){
			float ot = 1-t;
			float c1 = ot*ot, c2 = 2*t*ot, c3 = t*t;
			
			return c1*p0 + c2*m0 + c3*p1;
		}
		
		//Draw Bezier Line between p1 and p2
		//use m0 and m1 as the weight control
		//   p1---m0----m1---p2 
		public static Vector3 Po3Curve(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float t){
			float ot = 1-t;
			float c1 = ot*ot*ot, c2 = 3*t*ot*ot, c3 = 3*t*t*ot, c4 = t*t*t;
			
			return c1*p0+c2*m0+c3*m1+c4*p1;
		}
		
		public static Func<float,Vector3> PoNCurve(Vector3 p0, Vector3 p1, params Vector3[] m){
			if(m == null || m.Length == 0)
				return t => Vector3.Lerp(p0, p1, t);
			else if(m.Length == 1)
				return t => Po2Curve(p0,p1,m[0],t);
			else if(m.Length == 2)
				return t => Po3Curve(p0,p1,m[0],m[1],t);
			
			//TODO: power of n>3
			
			return t => {return Vector3.zero;};
		}
		
		public static Vector3 Po3Tangent(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float t) {
			float ot = 1-t;
			float c1 = -3*ot*ot, c2 = 3*(ot*ot-2*t*ot), c3 = 3*(2*t*ot - t*t), c4 = 3*t*t;
			
			return c1*p0 + c2*m0 + c3*m1 + c4*p1;
		}
		
	}
}
