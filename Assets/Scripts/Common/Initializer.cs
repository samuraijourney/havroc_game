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
					
					(new Thread()).Start(() => HVR_Network.StartConnection("127.0.0.1"));
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
	
	}

	private void Cleanup()
	{
		if(useDLL)
		{
			if(HVR_Tracking.IsActive())
			{
				HVR_Tracking.EndTrackingService();
			}
			
			if (HVR_Network.IsActive())
			{
				Debug.Log("Ending connection");
				
				(new Thread()).Start(() => HVR_Network.EndConnection());
			}
		}
	}

	public void OnConnect()
	{
		Debug.Log("Connect successful");

		// Initialize tracking service
		if (startTracking && !HVR_Tracking.IsActive()) 
		{
			HVR_Tracking.StartTrackingService();
		}
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
