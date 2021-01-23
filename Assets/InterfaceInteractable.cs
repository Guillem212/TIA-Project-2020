using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceInteractable : MonoBehaviour
{
    GameObject canvas;
    private void Start() 
    {
        TurnManagerRequest.instance.view.RPC("PlayerActivateStadium", Photon.Pun.RpcTarget.All);
    }


    private void OnDestroy() 
    {
        
    }
}
