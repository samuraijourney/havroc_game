using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Initializer : MonoBehaviour 
{
	public enum ConnectTarget { Local, CC3200, Custom };

	public bool useDLL = true;
	public bool startConnection = true;
	public bool startTracking = true;
	public ConnectTarget endpoint = ConnectTarget.Local;
	public string customIP;

	private HVR_Logger.LoggerCallback m_loggerCallback;

	public void Awake () 
    {
		if(useDLL)
		{
			m_loggerCallback = new HVR_Logger.LoggerCallback(OnLoggerEvent);
			HVR_Logger.SetLogger(m_loggerCallback);

			if(startConnection)
			{	
				// Initialize network connection
				if (!HVR_Network.IsActive()) 
				{
					if(endpoint == ConnectTarget.Local)
					{
						HVR_Network.AsyncStartConnection("127.0.0.1");
					}
					else if(endpoint == ConnectTarget.CC3200)
					{
						HVR_Network.AsyncStartConnection();
					}
					else if(endpoint == ConnectTarget.Custom)
					{
						HVR_Network.AsyncStartConnection(customIP);
					}
				}
				else
				{
					// Initialize tracking service
					if (startTracking && !HVR_Tracking.IsActive()) 
					{
						HVR_Tracking.StartTrackingService();
					}
					else if(!startTracking && HVR_Tracking.IsActive())
					{
						HVR_Tracking.EndTrackingService();
					}
				}
			}
		}
	}

	public float motorSendInterval = 120.0f;
	private float m_time;

	public void Start () 
	{
		m_time = motorSendInterval;
	}

	public void Update () 
    {
		if(m_time >= motorSendInterval)
		{
			int motorLength = 40;
			int motorMaxIndex = motorLength;
			
			byte[] motorIndices = new byte[motorLength];
			byte[] motorIntensities = new byte[motorLength];
			
			for(int i = 0; i < motorLength; i++)
			{
				motorIndices[i] = (byte)UnityEngine.Random.Range(0,motorMaxIndex);
				motorIntensities[i] = 255;

				//Debug.Log ("Sent motor - Index:" + motorIndices[i] + " Intensity:" + motorIntensities[i]);
			}
			
			//HVR_Network.SendMotorCommand(motorIndices, motorIntensities, motorLength);

			m_time -= motorSendInterval;
		}
		else
		{
			m_time += Time.deltaTime;
		}
	}

	public void OnDestroy()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		if(useDLL)
		{
			HVR_Tracking.EndTrackingService();
			HVR_Network.EndConnection();
		}
	}

	private void OnLoggerEvent(byte type, string msg)
	{
		if(type == 0)
		{
			Debug.Log(msg);
		}
		else if(type == 1)
		{
			Debug.LogWarning(msg);
		}
		else if(type == 2)
		{
			Debug.LogError(msg);
		}
	}
}
