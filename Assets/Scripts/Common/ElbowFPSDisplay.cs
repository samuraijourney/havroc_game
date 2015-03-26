using UnityEngine;
using System.Collections;

public class ElbowFPSDisplay : MonoBehaviour 
{
	public Arm arm;
	
	private FPSMonitor m_fpsMonitor;
	private TextMesh m_textMesh;
	private HVR_Tracking.ElbowCallback m_callback;

	void Start () 
	{
		m_textMesh = GetComponent<TextMesh> ();
		m_fpsMonitor = new FPSMonitor (4.0f);
		
		m_callback = new HVR_Tracking.ElbowCallback(OnElbowEvent);

		HVR_Tracking.RegisterElbowCallback(m_callback);
	}

	void Update () 
	{
		string text;

		if(arm == Arm.Left)
		{
			text = "LE";
		}
		else if(arm == Arm.Right)
		{
			text = "RE";
		}
		else
		{
			text = "";
		}

		m_textMesh.text = text + ": " + m_fpsMonitor.FPS.ToString();
	}
	
	public void OnElbowEvent(float s_w, float s_x, float s_y, float s_z, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			m_fpsMonitor.Update();
		}
	}
}
