using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HVR_NetworkTestScript : MonoBehaviour 
{
    public bool startConnection = false;

	// Use this for initialization
	public void Start () 
    {
        if(startConnection)
        {
            HVR_Network.RegisterSentCallback(TestSentCallback);
            HVR_Network.RegisterReceiveCallback(TestReceiveCallback);
            HVR_Network.RegisterConnectCallback(TestConnectCallback);
            HVR_Network.RegisterDisconnectCallback(TestDisconnectCallback);

			ThreadBase thread = new ThreadBase();
			thread.Start(InitiateConnection);
		}
	}

    public void InitiateConnection()
	{
		Debug.Log("Starting connection");
		HVR_Network.StartConnection("127.0.0.1");
	}

    public void TerminateConnection()
	{
		Debug.Log("Ending connection");
		HVR_Network.EndConnection();
	}
	
	// Update is called once per frame
    public void Update() 
    {
        if (startConnection)
        {
            int length = 20;
            byte[] indices = new byte[length];
            byte[] intensities = new byte[length];

            for (int i = 0; i < length; i++)
            {
                indices[i] = (byte)i;
                intensities[i] = (byte)(UnityEngine.Random.Range(0, 100) % 100 + 1);
            }

            int error = HVR_Network.SendMotorCommand(indices, intensities, length);
            Debug.Log("Sent motor command with error code: " + error);
        }
	}

    public void OnDestroy()
    {
        if (startConnection)
        {
			ThreadBase thread = new ThreadBase();
			thread.Start(TerminateConnection);
        }
    }

    public void TestSentCallback(IntPtr dataPtr, int size)
    {
        byte[] data = new byte[size];
        Marshal.Copy(dataPtr, data, 0, size);

        Debug.Log("Command sent successfully!");
    }

    public void TestReceiveCallback(IntPtr dataPtr, int size)
    {
        byte[] data = new byte[size];
        Marshal.Copy(dataPtr, data, 0, size);

        Debug.Log("Data received successfully!");
    }

    public void TestConnectCallback()
    {
        Debug.Log("Connect successful!");
    }

    public void TestDisconnectCallback()
    {
        Debug.Log("Disconnect successful!");
    }
}
