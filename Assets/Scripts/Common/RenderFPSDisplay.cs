using UnityEngine;
using System.Collections;

public class RenderFPSDisplay : MonoBehaviour 
{
	private FPSMonitor m_fpsMonitor;

	private TextMesh m_textMesh;
	
	void Start () 
	{
		m_textMesh = GetComponent<TextMesh> ();
		m_fpsMonitor = new FPSMonitor (4.0f);
	}

	void Update () 
	{
		m_fpsMonitor.Update();

		m_textMesh.text = "GM: " + m_fpsMonitor.FPS.ToString();
	}
}
