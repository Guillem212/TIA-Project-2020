using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceInteractable : MonoBehaviour
{
    private void Start() 
    {
        GameObject info_panel = GameObject.Find("Information_Panel");
        info_panel.SetActive(false);

        GameManager.instance.view.RPC("PlayerActivateStadium", Photon.Pun.RpcTarget.All);
    }
}
