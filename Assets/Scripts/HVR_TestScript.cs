using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HVR_TestScript : MonoBehaviour
{
    // Use this for initialization
    public void Start()
    {
		HVR_Test.RegisterMirrorCallback(MirrorCallback);
    }

    public void MirrorCallback(float yaw, float pitch, float roll, byte armID)
    {
        Debug.Log("Mirror successful: armID:" + armID + " yaw:" + yaw + " pitch:" + pitch + " roll:" + roll);
    }

    // Update is called once per frame
    public void Update()
    {
    }
}
