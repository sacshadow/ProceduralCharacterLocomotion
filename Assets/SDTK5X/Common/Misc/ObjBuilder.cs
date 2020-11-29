using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace SDTK{
	public class ObjBuilder {
		private static string compareName;//检测用名称
		
		//保存obj文件
		//meshFilters 对象meshFilters节点//MeshFilters in all gameObject that you wanna save;
		//name 文件名，不用包含后缀//file name, no need add ".obj" at end
		//path 储存文件夹//the path of directory you wanna save 
		public static bool SaveAsObj(MeshFilter[] meshFilters, string name, string path, bool overrideFile){
			Mesh mesh;
			Material[] mats;
			List<string> names;
			string meshSaveName;
			StreamWriter streamWriter;
			FileInfo createFile;
			int i,j,index;
			int count;
			
			//check before
			//检测
			if(meshFilters.Length ==0 || name.Length==0 || path.Length==0){
				Debug.LogError("Bad value find, "+meshFilters.Length+"meshes; "+name+";"+path);
				return false;
			}
			
			if(!Directory.Exists(path)){
				Debug.LogError("Bad file path, path not existed");
				
				return false;
			}
			
			createFile=new FileInfo(path+"/"+name+"_sd.obj");
			names=new List<string>();
			if(createFile.Exists){
				if(overrideFile){
					createFile.Delete();
				}
				else{
					return false;
				}
				
			}
			
			//写入模型
			streamWriter=createFile.CreateText();
			
			//header
			streamWriter.WriteLine("# file create form u3d script");
			streamWriter.WriteLine("# create date: "+System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			streamWriter.WriteLine("# Start");
			streamWriter.WriteLine("#");
			
			//body;
			//遍历所有meshFilter;
			for(i=0; i< meshFilters.Length; i++){
				mesh=meshFilters[i].sharedMesh;
				
				//获得材质球
				if(meshFilters[i].GetComponent<Renderer>()){
					mats=meshFilters[i].GetComponent<Renderer>().sharedMaterials;
				}
				else{
					mats=new Material[0];
				}
				
				//checkName
				//命名检查
				meshSaveName=((i==0)?TryName(names, mesh.name):mesh.name);
				names.Add(meshSaveName);
				
				streamWriter.WriteLine("g default");
				
				//write vertices
				//写入顶点
				foreach(Vector3 v in mesh.vertices){
					streamWriter.WriteLine(string.Format("v {0} {1} {2}",-v.x,v.y,v.z));
				}
				streamWriter.WriteLine("# total vertices "+mesh.vertices.Length);
				
				//write normals
				//写入法线
				foreach(Vector3 vn in mesh.normals){
					//~ streamWriter.WriteLine(string.Format("vn {0} {1} {2}",vn.x,vn.y,vn.z));
					if(vn.sqrMagnitude>0){
						streamWriter.WriteLine("vn 0 0 1");
					}
					else{
						streamWriter.WriteLine("vn 0 0 0");
					}
				}
				streamWriter.WriteLine("# total normals "+mesh.normals.Length);
				
				//write uv
				//写入uv
				foreach(Vector2 vuv in mesh.uv){
					streamWriter.WriteLine(string.Format("vt {0} {1}",vuv.x,vuv.y));
				}
				streamWriter.WriteLine("# total texture vertices "+mesh.uv.Length);
				
				streamWriter.WriteLine("s off");//关闭光滑组// smoth group off
				streamWriter.WriteLine("g "+meshSaveName);//写入模型
				
				//subMesh
				//写入面信息
				for(index=0; index<mesh.subMeshCount; index++){
					
					//check materials
					//检测材质球
					if(mats.Length!=0){
						//write using material
						// 写入材质球信息
						if(index<mats.Length && mats[index]!=null ){
							streamWriter.WriteLine("usemtl "+mats[index].name);
							streamWriter.WriteLine("usemap "+mats[index].name);
						}
						else{
							streamWriter.WriteLine("usemtl NoName"+index.ToString());
							streamWriter.WriteLine("usemap NoName"+index.ToString());
						}
					}
					else{
						streamWriter.WriteLine("usemtl initialShadingGroup");//没有材质球//no material find, using default
					}
					
					//write face
					//写入面
					count= mesh.vertices.Length;
					int[] triangles=mesh.GetTriangles(index);
					for(j=0; j<triangles.Length; j+=3){
						streamWriter.WriteLine(string.Format("f {0}/{0}/{0} {2}/{2}/{2} {1}/{1}/{1}",
							triangles[j]-count, triangles[j+1]-count, triangles[j+2]-count));
					}
					
				}
				
			}
			
			//finish
			streamWriter.WriteLine("# END");
			streamWriter.Close();
			return true;
		}
		
		//get unique name
		//检测名称
		private static string TryName(List<string> names, string checkName){
			int i=0;
			compareName=checkName;
			
			while(names.Exists(SameName)){
				i++;
				compareName=checkName+i.ToString();
			}
			
			return compareName;
		}
		
		//list delegate
		//名称检查委托
		private static bool SameName(string s){
			if(s==compareName){
				return true;
			}
			return false;
		}
	}
}