//http://www.terathon.com/code/tangent.html
//http://answers.unity3d.com/questions/7789/calculating-tangents-vector4.html
//http://forum.unity3d.com/threads/38984-How-to-Calculate-Mesh-Tangents

using UnityEngine;
using System.Collections;

public class TangentSolver {
	
	public static void Solver(Mesh theMesh) {
		int vertexCount=theMesh.vertexCount; 
		Vector3[] vertices=theMesh.vertices;
		Vector3[] normals= theMesh.normals;
		Vector2[] texcoords= theMesh.uv;
		int[] triangles= theMesh.triangles;
		int triangleCount= triangles.Length/3;
		Vector4[] tangents=new Vector4[vertexCount];
		Vector3[] tan1=new  Vector3[vertexCount];
		Vector3[] tan2=new Vector3[vertexCount];
		
		int tri=0;
		
		int i1,i2,i3;
		
		Vector3 v1,v2,v3,w1,w2,w3;
		Vector3 sdir,tdir;
		
		float x1,x2,y1,y2,z1,z2,s1,s2,t1,t2;
		float r;

		int i;
		
		Vector3 n,t;
		
		for (i = 0; i < (triangleCount); i++)
		{
			i1 = triangles[tri];
			i2 = triangles[tri+1];
			i3 = triangles[tri+2];

			v1 = vertices[i1];
			v2 = vertices[i2];
			v3 = vertices[i3];

			w1 = texcoords[i1];
			w2 = texcoords[i2];
			w3 = texcoords[i3];

			x1 = v2.x - v1.x;
			x2 = v3.x - v1.x;
			y1 = v2.y - v1.y;
			y2 = v3.y - v1.y;
			z1 = v2.z - v1.z;
			z2 = v3.z - v1.z;

			s1 = w2.x - w1.x;
			s2 = w3.x - w1.x;
			t1 = w2.y - w1.y;
			t2 = w3.y - w1.y;

			r = 1.0f / (s1 * t2 - s2 * t1);
			sdir =new  Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			tdir =new  Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;

			tri += 3;
		}
		for ( i = 0; i < (vertexCount); i++){
			n = normals[i];
			t = tan1[i];
		
			// Gram-Schmidt orthogonalize
			Vector3.OrthoNormalize(ref n,ref t);

			tangents[i].x  = t.x;
			tangents[i].y  = t.y;
			tangents[i].z  = t.z;

			// Calculate handedness
			tangents[i].w = ( Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f ) ? -1.0f : 1.0f;
		}
	
		theMesh.tangents = tangents;
	}
}
