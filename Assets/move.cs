using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
        }
    }
    private void Update() 
    {
        HandleMovement(); 
    }

}
