using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
 using UnityEngine.UI;
 using System.Net;
 using System.Net.NetworkInformation;
 using System.Net.Sockets;

public class move : NetworkBehaviour
{
    private void HandleMovement()
    {
        if(isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal*0.01f, moveVertical*0.01f, 0);
            transform.position = transform.position + movement;

            if(Input.GetKeyDown(KeyCode.X)){
                print(hintText);
            }
        }
    }
    private void Update() 
    {
        HandleMovement(); 
    }

      public string hintText;
 
     private void Start()
     {
         GetLocalIPAddress();
     }
     public string GetLocalIPAddress()
     {
         var host = Dns.GetHostEntry(Dns.GetHostName());
         foreach (var ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 hintText = ip.ToString();
                 return ip.ToString();
             }
         }
         throw new System.Exception("No network adapters with an IPv4 address in the system!");
     }
 
 

}
