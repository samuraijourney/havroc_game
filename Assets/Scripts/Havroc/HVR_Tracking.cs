using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HVR_Tracking
{
    public HVR_Tracking() { }

    private const CallingConvention callingConvention = CallingConvention.Cdecl;
    private const string dll_name = "havroc_library_dll.dll";

	[UnmanagedFunctionPointer(callingConvention)]
	public delegate void ShoulderCallback(float yaw, float pitch, float roll, byte side);
	[UnmanagedFunctionPointer(callingConvention)]
    public delegate void ElbowCallback(float yaw, float pitch, float roll, byte side);
	[UnmanagedFunctionPointer(callingConvention)]
    public delegate void WristCallback(float yaw, float pitch, float roll, byte side);

	[DllImport(dll_name, EntryPoint = "hvr_start_tracking_service", CallingConvention = callingConvention)]
	public static extern void StartTrackingService();

	[DllImport(dll_name, EntryPoint = "hvr_end_tracking_service", CallingConvention = callingConvention)]
	public static extern void EndTrackingService();

	[DllImport(dll_name, EntryPoint = "hvr_is_tracking_active", CallingConvention = callingConvention)]
	public static extern bool IsActive();

    [DllImport(dll_name, EntryPoint = "hvr_register_shoulder_callback", CallingConvention = callingConvention)]
    public static extern void RegisterShoulderCallback(ShoulderCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_register_elbow_callback", CallingConvention = callingConvention)]
    public static extern void RegisterElbowCallback(ElbowCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_register_wrist_callback", CallingConvention = callingConvention)]
    public static extern void RegisterWristCallback(WristCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_shoulder_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterShoulderCallback(ShoulderCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_elbow_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterElbowCallback(ElbowCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_wrist_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterWristCallback(WristCallback callback);
}
