using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    private PhotonView view = null;
    public static GameManager instance;
    public int player_id;
    public int turn;

    public void OnPhotonSerializeView(){

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            view = PhotonView.Get(this);
        }
        else
        {
            Destroy(instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient) player_id = 0; //Si soy el Host mi ID es 0 (Me asigno Player)
        else player_id = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(player_id);
            if(view.IsMine) //Solo si soy propietario del objeto al tomar input ejecuto esta linea
            {
                view.TransferOwnership(PhotonNetwork.PlayerList[player_id == 0 ? 1 : 0]); //Le doy al jugador contrario al posesion de algo. 
            }                                                                                   //Por default todo lo que tiene component view(que es para transeferir datos) lo tiene el host(jugador 1)
            else
            {
                Debug.Log("El objeto no es tuyo, no puedes transferirlo");
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(view.IsMine);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {                                               //Se envia desde el qeu puse la tecla al servidor, y el serivdor ejecuta esta funcion en todos los clientes
            view.RPC("PassTurn", RpcTarget.All);  //Las variables son independientes de cada uno osea que hay que actualizarlas con un metodo como este.
        }                                               //Basicamente podemos hacer que el que realmente controle todo (sea el game manager/turnmanager real), sea el
    }                                                   //Host y el otro solo reciba datos, y envie cuando tenga que mandar ataques y demas pero el que calcula es el Host                            

    [PunRPC]
    private void PassTurn(){
        if(turn == 1) turn = 2;
        else turn = 1;
        Debug.Log(turn);
    }
}
