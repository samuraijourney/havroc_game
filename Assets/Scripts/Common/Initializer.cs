using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour 
{
	public bool useDLL = true;
	public bool startConnection = true;
	public bool startTracking = true;

	public void Awake () 
    {
		if(useDLL)
		{
			if(startConnection)
			{
				HVR_Network.RegisterConnectCallback(this.OnConnect);
				HVR_Network.RegisterDisconnectCallback(this.OnDisconnect);
				
				// Initialize network connection
				if (!HVR_Network.IsActive()) 
				{
					Debug.Log("Starting connection");
					HVR_Network.AsyncStartConnection("127.0.0.1");
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
			else
			{
				Cleanup();
			}
		}
	}

	// Use this for initialization
	public void Start () 
	{

	}
		
	// Update is called once per frame
	public void Update () 
    {
		if(startTracking && HVR_Network.IsActive() && !HVR_Tracking.IsActive())
		{
			HVR_Tracking.StartTrackingService();
		}
	}
	
	private void Cleanup()
	{
		if(useDLL)
		{
			Debug.Log("Ending tracking");
			HVR_Tracking.EndTrackingService();

			Debug.Log("Ending connection");
			HVR_Network.EndConnection();
		}
	}

	public void OnConnect()
	{
		Debug.Log("Connect successful");
	}
	
	public void OnDisconnect()
	{
		Debug.Log("Disconnect successful");
	}

	public void OnDestroy()
	{
		Cleanup();
	}
}
