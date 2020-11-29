using UnityEngine;
using System;
using System.Collections;

namespace SDTK{
	[Serializable]
	public class Ts{//transform
		public V3 v;
		public Qt q;
		public V3 s;
		
		public Ts(){}
		public Ts(Transform t){
			v=new V3(t.position);
			q=new Qt(t.rotation);
			s=new V3(t.localScale);
		}
		
		public void SetToTransform(Transform t){
			t.position=v;
			t.rotation=q;
			t.localScale=s;
		}
	}

	[Serializable]
	public class V3{
		public float x,y,z;
		
		public V3(){}
		public V3(Vector3 v){
			x=v.x;
			y=v.y;
			z=v.z;
		}
		
		public override string ToString(){
			return "("+x.ToString()+", "+y.ToString()+", "+z.ToString()+")";
		}
		
		public static implicit operator Vector3(V3 v){
			return new Vector3(v.x,v.y,v.z);
		}
	}

	[Serializable]
	public class Qt{
		public float x,y,z,w;
		
		public Qt(){}
		public Qt(Quaternion q){
			x=q.x;
			y=q.y;
			z=q.z;
			w=q.w;
		}
		
		public override string ToString(){
			return "("+x.ToString()+", "+y.ToString()+", "+z.ToString()+", "+w.ToString()+")";
		}
		
		public static implicit operator Quaternion(Qt q){
			return new Quaternion(q.x,q.y,q.z,q.w);
		}
	}
	
	[Serializable]
	public class Cl{
		public float r,g,b,a;
		
		public Cl(){}
		public Cl(Color c){
			r=c.r;
			g=c.g;
			b=c.b;
			a=c.a;
		}
		
		public static implicit operator Color(Cl c){
			return new Color(c.r,c.g, c.b, c.a);
		}
	}
}