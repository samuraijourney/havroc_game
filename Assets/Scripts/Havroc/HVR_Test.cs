using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HVR_Test
{
    public HVR_Test() { }

	private const CallingConvention callingConvention = CallingConvention.Cdecl;
    private const string dll_name = "havroc_library_dll.dll";

	[UnmanagedFunctionPointer(callingConvention)]
	public delegate void MirrorCallback(float yaw, float pitch, float roll, byte armID);
	[DllImport(dll_name, EntryPoint="hvr_register_mirror_callback", CallingConvention = callingConvention)]
	public extern static int RegisterMirrorCallback(MirrorCallback callback);
}
