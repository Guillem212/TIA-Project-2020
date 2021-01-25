using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceCamera : MonoBehaviour
{

    private static WebCamTexture backCam;
    // Start is called before the first frame update
    void Start()
    {
        if(backCam is null)
        {
            backCam = new WebCamTexture();
        }

        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
        {
            backCam.Play();
        }
    }
}
