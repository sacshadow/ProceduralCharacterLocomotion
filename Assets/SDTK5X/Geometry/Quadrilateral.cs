using UnityEngine;
using System.Collections;

namespace SDTK.Geometry{

	public class Quadrilateral{
		private class Bound2D{
			public int minX, maxX, minY, maxY;
		
			public Bound2D(){}
			public Bound2D(Vector3[] p){
				
				minX=maxX=Mathf.RoundToInt(p[0].x);
				minY=maxY=Mathf.RoundToInt(p[0].z);
			
				for(int i=1; i<p.Length; i++){
					minX=(int)Mathf.Min(minX, p[i].x);
					maxX=(int)Mathf.Max(maxX,p[i].x);
					minY=(int)Mathf.Min(minY,p[i].z);
					maxY=(int)Mathf.Max(maxY,p[i].z);
				}
				minX=Mathf.FloorToInt(minX)-4;
				maxX=Mathf.CeilToInt(maxX)+4;
				minY=Mathf.FloorToInt(minY)-4;
				maxY=Mathf.CeilToInt(maxY)+4;
				
				//~ Debug.Log("bound2D "+minX+"; "+minY+"; "+maxX+"; "+maxY);
			}
		}
		public class Segment{
			public Vector3 p1,p2;
			public float a,b;
			public bool isSpecial;
			
			public Segment(){}
			public Segment(Vector3 p1, Vector3 p2){
				float h=(p2.x-p1.x), t=(p2.z-p1.z);
				
				this.p1=p1;
				this.p2=p2;
				
				if(h==0 && t==0){
					isSpecial=true;
					return;
				}
				
				this.a=(p2.z-p1.z)/(p2.x-p1.x);
				this.b= p1.z-a*p1.x;
			}
			
			public bool IsPointXInSegment(float x){
				return (p1.x-x)*(p2.x-x)<0 && !isSpecial;
			}
			public bool IsPointYInSegment(float y){
				return (p1.z-y)*(p2.z-y)<0 && !isSpecial;
			}
			
			public int GetXPointOnY(int x){
				float rt=a*(float)x+b;
				return Mathf.RoundToInt(rt);
			}
			
		}
		
		public Vector3 GetStartPoint{
			get{
				return new Vector3(bound.minX,0,bound.minY);
			}
		}
		
		private Segment[] seg;
		
		private Bound2D bound;
		
		public Quadrilateral(){}
		public Quadrilateral(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3){
			seg=new Segment[4];
			seg[0]=new Segment(p0,p1);
			seg[1]=new Segment(p1,p2);
			seg[2]=new Segment(p2,p3);
			seg[3]=new Segment(p3,p0);
			bound=new Bound2D(new Vector3[]{p0,p1,p2,p3});
		}

		public Rect GetBoundArea(){
			return new Rect(bound.minX, bound.minY, bound.maxX-bound.minX, bound.maxY-bound.minY);
		}
		
		public int[] GetInternalBounds(){
			int[] rt=new int[(bound.maxX-bound.minX)*2];
			int min, max, ans;
			
			for(int i=bound.minX; i<bound.maxX; i++){
				min=-1;
				max=-1;
				
				for(int j=0; j<seg.Length; j++){
					if(!seg[j].IsPointXInSegment((float)i))
						continue;
					
					ans=seg[j].GetXPointOnY(i);
					
					if(min==-1)
						min=ans;
					
					min=Mathf.Min(min,ans);
					max=Mathf.Max(max,ans);
				}
				
				//~ if(i==bound.maxX-1)
					//~ Debug.Log(min+"; "+max+"; "+i);
				
				//~ if(min-bound.minY<-1 || max-bound.minY<-1)
					//~ Debug.Log(min+"; "+max+"; "+i);
				
				rt[(i-bound.minX)*2]=min-bound.minY;
				rt[(i-bound.minX)*2+1]=max-bound.minY;
			}
			
			return rt;
		}	
	}

}