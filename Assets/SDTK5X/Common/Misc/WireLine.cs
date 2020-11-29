using UnityEngine;
using System.Collections.Generic;
using SDTK.Geometry;

public class WireLine : MonoBehaviour {
	private static float scale=1;
	
	public Camera renderedCamera;
	public Material material;
	public int layer=0;
	public bool useWorldSpace=true;
	
	private Vector3[] node;
	private float orgScale=1;
	private float width;
	private Mesh mesh;
	private bool upToData=false;
	
	private Transform self;
	
	private float Width{
		get{
			return width*orgScale;
		}
	}
	
	void Awake(){
		mesh=new Mesh();
		self=this.transform;
	}
	
	void OnDestroy(){
		Destroy(mesh);
	}
	
	public void Setup(Vector3[] node, float width){
		Setup(node,width,material,false);
	}
	public void Setup(Vector3[] node, float width, Material mat){
		Setup(node, width, mat, false);
	}
	public void Setup(Vector3[] node, float width, Material mat, bool update){
		this.node=node;
		this.width=Mathf.Abs(width);
		material=mat;
		upToData=update;
		
		if(node==null || node.Length<2)
			throw new System.Exception("Illegal Array");
		
		DrawLineMesh();
	}
	
	public void ChangeNode(int index, Vector3 position){
		if(node==null || index>node.Length)
			throw new System.Exception("Illegal index");
		
		node[index]=position;
		
		mesh.vertices=GetVertices();
		mesh.uv=GetUV();
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
	
	private Vector3[] GetVertices(){
		List<Vector3> vertices=new List<Vector3>(GTools.GetOffsetShape(node,Width/2f,false));
		vertices.AddRange(GTools.GetOffsetShape(node,-Width/2f,false));
		
		
		return vertices.ToArray();
	}
	
	
	public void SetNode(Vector3[] node){
		this.node=node;
		
		if(node==null || node.Length<2)
			throw new System.Exception("Illegal Array");
		
		DrawLineMesh();
	}
	
	public void SetWidth(float width){
		this.width=Mathf.Abs(width);
		
		if(Mathf.Approximately(width,0))
			throw new System.Exception("width can not be zero");
		
		DrawLineMesh();
	}
	
	private Vector2[] GetUV(){
		Vector2[] uv=new Vector2[node.Length*2];
		Vector2 length;
		int half=node.Length;
		
		uv[0]=Vector2.zero;
		uv[half]=Vector2.up;
		
		for(int uCount=1; uCount<half; uCount++){
			length=Vector2.right*(Vector3.Distance(node[uCount], node[uCount-1])/Width);
			uv[uCount]=uv[uCount-1]+length;
			uv[uCount+half]=uv[uCount+half-1]+length;
		}
		return uv;
	}
	
	private int[] GetTriangle(){
		int[] triangles=new int[(node.Length-1)*6];
		
		for(int tCount=0, sCount=0, offset=node.Length; tCount<triangles.Length; tCount+=6,sCount+=1){
			triangles[tCount+0]=sCount+0;
			triangles[tCount+1]=sCount+1;
			triangles[tCount+2]=sCount+1+offset;
			triangles[tCount+3]=sCount+0;
			triangles[tCount+4]=sCount+1+offset;
			triangles[tCount+5]=sCount+0+offset;
		}
		
		return triangles;
	}
	
	private void DrawLineMesh(){
		Vector3[] vertices=GetVertices();
		Vector2[] uv=GetUV();
		int[] triangles=GetTriangle();
		
		mesh.vertices=vertices;
		mesh.uv=uv;
		mesh.triangles=triangles;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
	
	void LateUpdate(){
		if(upToData && orgScale!=scale)
			DrawLineMesh();
		
		Matrix4x4 matrix=useWorldSpace?Matrix4x4.identity: self.localToWorldMatrix;
		
		Graphics.DrawMesh(mesh, matrix, material, layer, renderedCamera);
	}
	
}
