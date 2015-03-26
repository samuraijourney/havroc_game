using UnityEngine;
using System.Collections;

public class ShoulderFPSDisplay : MonoBehaviour 
{
	public Arm arm;

	private FPSMonitor m_fpsMonitor;
	private TextMesh m_textMesh;
	private HVR_Tracking.ShoulderCallback m_callback;

	void Start () 
	{
		m_textMesh = GetComponent<TextMesh> ();
		m_fpsMonitor = new FPSMonitor (4.0f);

		m_callback = new HVR_Tracking.ShoulderCallback(OnShoulderEvent);

		HVR_Tracking.RegisterShoulderCallback(m_callback);
	}

	void Update () 
	{
		string text;
		
		if(arm == Arm.Left)
		{
			text = "LS";
		}
		else if(arm == Arm.Right)
		{
			text = "RS";
		}
		else
		{
			text = "";
		}
		
		m_textMesh.text = text + ": " + m_fpsMonitor.FPS.ToString();
	}

	public void OnShoulderEvent(float s_w, float s_x, float s_y, float s_z, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			m_fpsMonitor.Update();
		}
	}
}
