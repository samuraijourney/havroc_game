using UnityEngine;
using System;
using System.Collections;
using System.IO.Pipes;

public class TextureAccess : MonoBehaviour 
{
	public Material cameraMaterial;
	
	private NamedPipeClientStream m_pipeline;
	private Texture2D m_texture;

	// Use this for initialization
	void Start () 
	{
		m_texture = cameraMaterial.mainTexture as Texture2D;

		m_pipeline = new NamedPipeClientStream("UnityEnemyCameraPipeline");

		(new Thread ()).Start (() => m_pipeline.Connect (5000));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_pipeline.IsConnected)
		{
			byte[] width = BitConverter.GetBytes(m_texture.width);
			byte[] height = BitConverter.GetBytes(m_texture.height);

			for(int i = 0; i < width.Length; i++)
			{
				m_pipeline.WriteByte(width[i]);
			}

			for(int i = 0; i < height.Length; i++)
			{
				m_pipeline.WriteByte(height[i]);
			}

			Color32[] pixels = m_texture.GetPixels32();

			for(int i = 0; i < pixels.Length; i++)
			{
				m_pipeline.WriteByte(pixels[i].b);
				m_pipeline.WriteByte(pixels[i].g);
				m_pipeline.WriteByte(pixels[i].r);
				m_pipeline.WriteByte(pixels[i].a);
			}
		}
	}
}
