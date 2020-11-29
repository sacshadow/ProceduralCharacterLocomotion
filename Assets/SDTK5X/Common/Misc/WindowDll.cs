using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace SDTK {
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenFileName
	{
		public int structSize = 0;
		public IntPtr dlgOwner = IntPtr.Zero;
		public IntPtr instance = IntPtr.Zero;
		public String filter = null;
		public String customFilter = null;
		public int maxCustFilter = 0;
		public int filterIndex = 0;
		public String file = null;
		public int maxFile = 0;
		public String fileTitle = null;
		public int maxFileTitle = 0;
		public String initialDir = null;
		public String title = null;
		public int flags = 0;
		public short fileOffset = 0;
		public short fileExtension = 0;
		public String defExt = null;
		public IntPtr custData = IntPtr.Zero;
		public IntPtr hook = IntPtr.Zero;
		public String templateName = null;
		public IntPtr reservedPtr = IntPtr.Zero;
		public int reservedInt = 0;
		public int flagsEx = 0;
	}
	
	public class WindowDll
	{
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
		
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
		
		public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
		{
			return GetOpenFileName(ofn);
		}
		
		public static void OpenFilePanel(string title, string defaultPath, string defaultName, string extension, Action<string> OnPathLoaded) {
			OpenFileName ofn = new OpenFileName();
			ofn.structSize = Marshal.SizeOf(ofn);
			ofn.filter = "选择文件(*."+extension+")\0*."+extension;
			ofn.file = new string(new char[256]);
			ofn.maxFile = ofn.file.Length;
			ofn.fileTitle = CopyStr(defaultName, 64);
			ofn.maxFileTitle = ofn.fileTitle.Length;
			ofn.initialDir = defaultPath;
			ofn.title = title; 
			ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST|OFN_NOCHANGEDIR  
			if (GetOpenFileName(ofn))
				OnPathLoaded(ofn.file);
		}
		
		public static void SaveFilePanel(string title, string defaultPath, string defaultName, string extension, Action<string> OnPathLoaded) {
			OpenFileName ofn = new OpenFileName();
			ofn.structSize = Marshal.SizeOf(ofn);
			ofn.filter = "选择文件(*."+extension+")\0*."+extension;
			ofn.file = new string(new char[256]);
			ofn.maxFile = ofn.file.Length;
			ofn.fileTitle = CopyStr(defaultName, 64);
			ofn.maxFileTitle = ofn.fileTitle.Length;
			ofn.initialDir = defaultPath;
			ofn.title = title;
			ofn.flags = 0x00080000 | 0x00000800 | 0x00000008;//OFN_EXPLORER|OFN_PATHMUSTEXIST|OFN_NOCHANGEDIR  
			if (GetSaveFileName(ofn)) {
				var s = ofn.file;
				if(s.Length < extension.Length + 1)
					s = s + "." + extension;
				
				if(s.Substring(s.Length - extension.Length - 1) != ("." + extension))
					s += "." + extension;
				
	
				OnPathLoaded(s);
			}
		}
		
		public static string CopyStr(string str, int size) {
			Debug.Log("str " + str);
			return str + new string(new char[size - str.Length]);
		}	
	}
}
