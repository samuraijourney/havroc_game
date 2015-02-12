using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HVR_Network
{
    public HVR_Network() { }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SentCallback(IntPtr data, int length);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReceiveCallback(IntPtr data, int length);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ConnectCallback();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DisconnectCallback();

    private const CallingConvention callingConvention = CallingConvention.Cdecl;
    private const string dll_name = "havroc_library_dll.dll";

    [DllImport(dll_name, EntryPoint = "hvr_start_connection", CallingConvention = callingConvention)]
    public static extern int StartConnection([Out, MarshalAs(UnmanagedType.LPStr)] string ip);
    public static        int StartConnection() { return StartConnection(null); }

    [DllImport(dll_name, EntryPoint = "hvr_end_connection", CallingConvention = callingConvention)]
    public static extern int EndConnection();

    [DllImport(dll_name, EntryPoint = "hvr_is_active", CallingConvention = callingConvention)]
    public static extern bool IsActive();

    [DllImport(dll_name, EntryPoint = "hvr_send_motor_command", CallingConvention = callingConvention)]
    public static extern int SendMotorCommand([Out, MarshalAs(UnmanagedType.LPArray)] byte[] index,
                                              [Out, MarshalAs(UnmanagedType.LPArray)] byte[] intensity, int length);

    [DllImport(dll_name, EntryPoint = "hvr_register_sent_callback", CallingConvention = callingConvention)]
    public static extern void RegisterSentCallback(SentCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_register_receive_callback", CallingConvention = callingConvention)]
    public static extern void RegisterReceiveCallback(ReceiveCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_register_connect_callback", CallingConvention = callingConvention)]
    public static extern void RegisterConnectCallback(ConnectCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_register_disconnect_callback", CallingConvention = callingConvention)]
    public static extern void RegisterDisconnectCallback(DisconnectCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_sent_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterSentCallback(SentCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_receive_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterReceiveCallback(ReceiveCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_connect_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterConnectCallback(ConnectCallback callback);

    [DllImport(dll_name, EntryPoint = "hvr_unregister_disconnect_callback", CallingConvention = callingConvention)]
    public static extern void UnregisterDisconnectCallback(DisconnectCallback callback);
}
