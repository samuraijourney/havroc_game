using UnityEngine;
using System.Collections;

public class HavrocFlash : MonoBehaviour, IEndStateMember
{
	private TextMesh m_textMesh;
	private bool m_enable;
	
	void Start () 
	{
		m_textMesh = GetComponent<TextMesh> ();
	}

	void Update () 
	{
		if(m_enable)
		{
			m_textMesh.GetComponent<MeshRenderer> ().material.color = new Color(0, 1.0f, 1.0f, (Mathf.Sin (Time.time) + 1) * 0.5f * 0.75f);
		}
	}

	public void OnStateBaseStart(GameState gameState)
	{
		if(gameState == GameState.End)
		{
			m_enable = true;
			m_textMesh.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	public void OnStateBaseEnd(GameState gameState)
	{
		if(gameState == GameState.End)
		{
			m_enable = false;
			m_textMesh.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
