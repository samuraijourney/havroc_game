using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour 
{
	public enum ConnectTarget { Local, CC3200 };

	public bool useDLL = true;
	public bool startConnection = true;
	public bool startTracking = true;
	public ConnectTarget endpoint = ConnectTarget.Local;

	public void Awake () 
    {
		if(useDLL)
		{
			if(startConnection)
			{
				HVR_Network.RegisterConnectCallback(this.OnConnect);
				
				// Initialize network connection
				if (!HVR_Network.IsActive()) 
				{
					Debug.Log("Starting connection");

					if(endpoint == ConnectTarget.Local)
					{
						HVR_Network.AsyncStartConnection("127.0.0.1");
					}
					else if(endpoint == ConnectTarget.CC3200)
					{
						HVR_Network.AsyncStartConnection();
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

	public void Start () 
	{

	}
		
	public void Update () 
    {
		byte[] indices = new byte[72];
		byte[] intensities = new byte[72];

		for(int i = 0; i < 72; i++)
		{
			indices[i] = (byte)Mathf.FloorToInt(72.0f*Random.Range(0,1));
			intensities[i] = (byte)Mathf.FloorToInt(100.0f*Random.Range(0,1));
		}

		//HVR_Network.SendMotorCommand(indices, intensities, 72);

		for(int i = 0; i < indices.Length; i++)
		{
			//Debug.Log ("Sent motor data for index " + indices [i]);
		}
	}

	public void OnConnect()
	{
		Debug.Log("Connect successful");
	}

	public void OnDestroy()
	{
		Cleanup();
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
}
