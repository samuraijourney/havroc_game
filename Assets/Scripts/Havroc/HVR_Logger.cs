using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class HVR_Logger : MonoBehaviour 
{
	private const CallingConvention callingConvention = CallingConvention.Cdecl;
	private const string dll_name = "havroc_library_dll.dll";
	
	[UnmanagedFunctionPointer(callingConvention)]
	public delegate void LoggerCallback(byte type, [MarshalAs(UnmanagedType.LPStr)]string msg);
	
	[DllImport(dll_name, EntryPoint = "hvr_set_logging_callback", CallingConvention = callingConvention)]
	public static extern void SetLogger(LoggerCallback callback);
}
