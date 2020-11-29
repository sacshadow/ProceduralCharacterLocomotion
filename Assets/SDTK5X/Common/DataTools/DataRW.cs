/*
	读取数据
	最后修改 2012-8-18
*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace SDTK{
	
	public class DataRW {
		
		//检查路径
		public static void CheckDirectory(string directory){
			if(!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}
		
		//检查数据是否存在
		public static bool IsDataExists(string path){
			return File.Exists(path);
		}
		
		//复制文件夹（及文件夹下所有子文件夹和文件）
		public static void CopyDirectory(string sourcePath, string destinationPath){
			DirectoryInfo info = new DirectoryInfo(sourcePath);
			Directory.CreateDirectory(destinationPath);
			
			foreach (FileSystemInfo fsi in info.GetFileSystemInfos()){
				String destName = Path.Combine(destinationPath, fsi.Name);
	 			if (fsi is System.IO.FileInfo)//如果是文件，复制文件
					File.Copy(fsi.FullName, destName);
				else{//如果是文件夹，新建文件夹，递归
					Directory.CreateDirectory(destName);
					CopyDirectory(fsi.FullName, destName);
				}
			}
		}
		
		//判断是否符合文件命名规范（英文和数字only）
		public static bool IsLegalFileName(string fileName){
			Regex r = new Regex("^[a-zA-Z0-9_]+$",RegexOptions.IgnoreCase|RegexOptions.Singleline);
			Match m = r.Match(fileName);
			return m.Success;
		}
		
		//存储数据为bin
		public static void SetClassToBin<T>(T saveData, string path){
			DeleteFile(path);
			
			BinaryFormatter formatter=new BinaryFormatter();
			Stream stream=new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, saveData);
			stream.Close();
		}
		//从bin文件读取数据
		public static T GetClassFromBin<T>(string path){
			BinaryFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			T rt=(T)formatter.Deserialize(stream);
			stream.Close();
			return rt;
		}
		
		public static byte[] SetClassToByte<T>(T saveData){
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			formatter.Serialize(ms, saveData);
			byte[] rt=ms.ToArray();
			ms.Close();
			ms.Dispose();
			return rt;
		}
		
		public static T GetClassFromByte<T>(byte[] loadData){
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream(loadData);
			T rt=(T)formatter.Deserialize(ms);
			ms.Close();
			ms.Dispose();
			return rt;
		}
		
		//存储数据为xml
		//save to xml
		public static void SetClassToXML<T>(T saveData, string path){
			CreateFile(SetClassToXMLString<T>(saveData),path);
		}
		public static string SetClassToXMLString<T>(T saveData){
			if(saveData==null)
				throw new Exception("Data 未赋值");
			return SerializeObject(saveData,typeof(T));
		}
		
		//从xml 中读取类数据
		//read from xml
		public static T GetClassFromXML<T>(string path){
			string data=LoadFile(path);
			return DeserializeObject<T>(data);
		}
		public static T GetClassFromXMLString<T>(string xml){//read from xml string
			return DeserializeObject<T>(xml);
		}
		
		
		
		//保存byte数据至指定位置文件
		public static string SaveBytesToFile(byte[] data, FileSaveInfo info){
			string savePath=FormFileInfo(info);
			CheckDirectory(info.savePath);
			
			if(!info.isOverWrite)//如果不覆盖，重命名文件
				savePath=RenameFile(info);
			
			File.WriteAllBytes(savePath,data);
			return savePath;
		}
		
		public static void SaveByte(byte[] data, string savePath){
			File.WriteAllBytes(savePath,data);
		}
		
		public static byte[] ReadByte(string savePath) {
			return File.ReadAllBytes(savePath);
		}
		
		//创建读取文件
		public static void CreateFile(string data, string savePath){
			DeleteFile(savePath);
			
			FileInfo file = new FileInfo(savePath);
			StreamWriter writer;
			
			writer = file.CreateText(); 
			writer.Write(data); 
			writer.Close(); 
		}

		public static string LoadFile(string savePath){
			StreamReader reader;
			string data;
			
			if(!File.Exists(savePath))
				throw new Exception("文件不存在或不可访问: "+savePath);
			
			reader = File.OpenText(savePath); 
			data = reader.ReadToEnd();
			reader.Close(); 
			
			if(data==null || data.Length<1)
				throw new Exception("读取错误: data"+((data==null)?" == null":" 长度为0"));
			
			return data;
		}

        public static bool RemoveFile( string savePath ) {
            if ( File.Exists( savePath ) ) {
                File.Delete( savePath );
                return true;
            }
            return false;
        }
		
		////////////////////////////内部调用函数///////////////////////////////////
		private static string RenameFile(FileSaveInfo info){
			string newPath=FormFileInfo(info);
			int i=-1;
			while(IsDataExists(newPath)){
				i++;
				newPath=FormFileInfo(info,i);
			}
			return newPath;
		}
		
		private static string FormFileInfo(FileSaveInfo info){
			return info.savePath+"/"+info.fileName+"."+info.fileExt;
		}
		private static string FormFileInfo(FileSaveInfo info, int i){
			return info.savePath+"/"+info.fileName+"_"+i.ToString()+"."+info.fileExt;
		}
		
		private static void DeleteFile(string path){
			FileInfo file = new FileInfo(path);
			
			if(file.Exists)
				file.Delete();
		}
	
		private static string UTF8ByteArrayToString(byte[] characters){      
			UTF8Encoding encoding = new UTF8Encoding(); 
			string constructedString = encoding.GetString(characters); 
			return (constructedString); 
		} 
    
		private static byte[] StringToUTF8ByteArray(string pXmlString){ 
			UTF8Encoding encoding = new UTF8Encoding(); 
			byte[] byteArray = encoding.GetBytes(pXmlString); 
			return byteArray; 
		} 

		private static string SerializeObject(object pObject, System.Type type){ 
			string XmlizedString = null; 
			MemoryStream memoryStream = new MemoryStream(); 
			XmlSerializer xs = new XmlSerializer(type); 
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
			xs.Serialize(xmlTextWriter, pObject); 
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
			XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
			return XmlizedString; 
		}
		
		private static object DeserializeObject(string pXmlizedString, System.Type type) { 
			XmlSerializer xs = new XmlSerializer(type); 
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
			return xs.Deserialize(memoryStream); 
		}
		private static T DeserializeObject<T>(string pXmlizedString) { 
			XmlSerializer xs = new XmlSerializer(typeof(T)); 
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
			return (T)xs.Deserialize(memoryStream); 
		}
		
	}
	
	public class FileSaveInfo{
		public string fileName;
		public string fileExt;
		public string savePath;
		public bool isOverWrite;
		
		public FileSaveInfo(string fileName, string fileExt, string savePath){
			this.fileName=fileName;
			this.fileExt=fileExt;
			this.savePath=savePath;
			this.isOverWrite=false;
		}
		public FileSaveInfo(string fileName, string fileExt, string savePath, bool isOverWrite){
			this.fileName=fileName;
			this.fileExt=fileExt;
			this.savePath=savePath;
			this.isOverWrite=isOverWrite;
		}
	}
}
