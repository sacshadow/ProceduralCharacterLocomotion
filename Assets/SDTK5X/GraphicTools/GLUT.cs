using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

namespace SDTK.GraphicTools {
	//GL Utility
	public static class GLUT {
		
		public static void PushMatrix(params Action[] DrawProcess) {
			GL.PushMatrix();
			for(int i=0; i<DrawProcess.Length; i++)
				DrawProcess[i]();
			GL.PopMatrix ();
		}
		
		public static void Begin(int mode, Material mat, int pass = 0, params Action[] DrawProcess) {
			mat.SetPass(pass);
			GL.Begin(mode);
			
			for(int i=0; i<DrawProcess.Length; i++)
				DrawProcess[i]();
			
			GL.End();
		}		
		
		public static void PushPoint(params Vector3[] point) {
			for(int i=0; i<point.Length; i++)
				GL.Vertex(point[i]);
		}
		
		public static void DrawLines(params Vector3[] point) {
			GL.Begin(GL.LINES);
			PushPoint(point);
			GL.End();
		}
		
		public static void Line(Vector3 p0, Vector3 p1){
			GL.Vertex(p0);
			GL.Vertex(p1);
		}
		
		// public static void LinePair(params Vector3[] point) {
			// for(int i=0; i<point.Length; i++)
				// GL.Vertex(point[i]);
		// }
		
		public static void LineSegments(params Vector3[] point) {
			for(int i=0; i<point.Length-1; i++) {
				Line(point[i], point[i+1]);
			}
		}
		
		public static void ClosedLineSegments(params Vector3[] point) {
			for(int i=0; i<point.Length; i++) {
				Line(point[i], point[(i+1)%point.Length]);
			}
		}
		
		public static void WireCube(Vector3 middle, Vector3 size) {
			var p0 = middle - size/2f;
			var p1 = p0 + Vector3.right * size.x;
			var p2 = p1 + Vector3.forward * size.z;
			var p3 = p0 + Vector3.forward * size.z;
			
			var up = Vector3.up*size.y;
			var p4 = p0 + up;
			var p5 = p1 + up;
			var p6 = p2 + up;
			var p7 = p3 + up;
			
			ClosedLineSegments(p0,p1,p2,p3);
			ClosedLineSegments(p4,p5,p6,p7);
			PushPoint(p0,p4, p1,p5, p2,p6, p3,p7);
		}
		
		public static void DrawFlatQuads(float x, float z, params Vector3[] blPoint) {
			GL.Begin(GL.QUADS);
			for(int i=0; i<blPoint.Length; i++)
				FlatQuad(blPoint[i], x, z);
			GL.End();
		}
		
		public static void FlatQuad(Vector3 blPoint, float x , float z) {
			GL.Vertex(blPoint);
			GL.Vertex(blPoint + new Vector3(0,0,z));
			GL.Vertex(blPoint + new Vector3(x,0,z));
			GL.Vertex(blPoint + new Vector3(x,0,0));
		}
		
		public static void DrawArrow(Vector3 p0, Vector3 p1, Color color, float size = 0.25f) {
			GL.Color(color);
			DrawLines(p0,p1);
			
			var nor = (p1 - p0).normalized;
			var q = Quaternion.LookRotation(nor);
			var k = nor * -size + p1;
			
			GL.Begin(GL.TRIANGLES);
			GL.Color(color);
			
			var z = size * 0.25f;
			var q0 = q*Vector3.up*z+k;
			var q1 = q*Vector3.right*z+k;
			var q2 = q*Vector3.down*z+k;
			var q3 = q*Vector3.left*z+k;
			
			PushPoint(
				p1,q0,q1,
				p1,q1,q2,
				p1,q2,q3,
				p1,q3,q0
			);
			
			GL.End();
		}
		
	}
}
