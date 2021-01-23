using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Private Variables

    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject information_Panel; 
    [SerializeField] private int ph_timerChoosePokemon;
    [SerializeField] private int ph_timerChooseAttack;
    private int ph_currentTime;
    private Coroutine h_TimerCoroutine;
    #endregion

    #region Public Variables
    [HideInInspector] public PhotonView view = null;
    public static GameManager instance;
    [HideInInspector] public int player_id;
    public int turn;
    public int ph_playersWithStadiumActive = 0;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        ph_playersWithStadiumActive = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    #region RPC Functions

    [PunRPC]
    public void PassTurn(){
        if(turn == 1)
        {
            AttackTurn();
            turn = 2;
        }
        else
        {
            turn = 1;
        }
        Debug.Log(turn);

    }

    [PunRPC]
    public void PlayerActivateStadium(){
        ph_playersWithStadiumActive +=1;
        if(ph_playersWithStadiumActive == 2) ChoosePokemonTurn();
    } 

    [PunRPC]
    private void RefreshTimer_UI(int timeLeft)
    {
        information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = timeLeft.ToString();
    }
    
    #endregion

    #region PUN Callbacks
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _ShowAndroidToastMessage("Opponent trainer left the Match");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    #endregion

    #region Coroutines
    IEnumerator Timer(){
        yield return new WaitForSeconds(1f);
        ph_currentTime--;
        view.RPC("RefreshTimer_UI", RpcTarget.All, ph_currentTime);
        if(ph_currentTime <= 0)
        {
            information_Panel.SetActive(false);
            if(turn == 1)GameManager.instance.view.RPC("PassTurn", RpcTarget.All);
            else PassAttackToHost();
        }
    }
    #endregion

    #region PUBLIC METHODS
    public void PassAttackToHost(){
        //player.GetComponent<Player>().Attack(player.GetComponent<Player>().selectedAttack);
    }
    public void ph_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
    #endregion

    #region PRIVATE METHODS

    private void ChoosePokemonTurn(){
        turn = 1;
        GameObject canvas = GameObject.Find("Player_Canvas");
        canvas.transform.Find("Select_Pokemon_Panel").gameObject.SetActive(true);
        canvas.transform.Find("Attack_Panel").gameObject.SetActive(false);

        InitializeTimer(ph_timerChoosePokemon);

    }

    private void AttackTurn(){
        turn = 2;
        GameObject canvas = GameObject.Find("Player_Canvas");
        canvas.transform.Find("Attack_Panel").gameObject.SetActive(true);
        canvas.transform.Find("Select_Pokemon_Panel").gameObject.SetActive(false);

        InitializeTimer(ph_timerChooseAttack);

    }
    private void InitializeTimer(int time){
        information_Panel.SetActive(true);
        ph_currentTime = time;
        view.RPC("RefreshTimer_UI", RpcTarget.All, ph_currentTime);

        if(PhotonNetwork.IsMasterClient)
        {
            h_TimerCoroutine = StartCoroutine(Timer());
        }

    }

    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {

        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
            #endif
        }
    
    }
    #endregion

}
