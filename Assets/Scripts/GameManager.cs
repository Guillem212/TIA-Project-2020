using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    #region Private Variables
    public GameObject information_Panel; 
    [SerializeField] private GameObject ImageCard_Panel; 
    [SerializeField] private int ph_timerChoosePokemon;
    [SerializeField] private int ph_timerChooseAttack;
    private int ph_currentTime;
    private Coroutine h_TimerCoroutine;
    #endregion

    #region Public Variables
    public static GameManager instance;
    public GameObject player; 
    [HideInInspector] public PhotonView view = null;
    [HideInInspector] public int player_id;
    public int turn;
    public int ph_playersWithStadiumActive;
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

        /*var xrManagerSettings = XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
        xrManagerSettings.InitializeLoaderSync();*/
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient) player_id = 0; //Si soy el Host mi ID es 0 (Me asigno Player)
        else player_id = 1;
        ph_playersWithStadiumActive = 0;
        turn = 0;
        AudioManager.instance.PlayMusic(1);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.X)) //DEBUG
       {
           view.RPC("PlayerActivateStadium", Photon.Pun.RpcTarget.All);
       }
    }

    #region RPC FUNCTIONS

    [PunRPC]
    public void PassTurn(){
        if(turn == 1)
        {
            if(player.GetComponent<Player>().activePokemon is null) player.GetComponent<Player>().activePokemon = player.GetComponent<Player>().pokemons[Random.Range(0, player.GetComponent<Player>().pokemons.Length)];
            AttackTurn();
            AudioManager.instance.PlayMusic(2);
        }
        else //Ha muerto un pokemon y hay que volver a elegir (VUELVE EL TURNO 1)
        {  
            ChoosePokemonTurn();
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
        information_Panel.SetActive(timeLeft >-1);
        information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = timeLeft.ToString();
    }

    [PunRPC]
    public void RefreshUIAttacksStart(Category category, string pokemonName, string attackName, string statusModified){
        information_Panel.SetActive(true);
        ImageCard_Panel.SetActive(false);
        //DEBUG
        switch (category)
        {
            case Category.PHYSICAL:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = pokemonName + " use " + attackName + ".";
                Debug.Log(pokemonName + " use " + attackName+ ".");
                break;
            case Category.SPECIAL:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = pokemonName + " use " + attackName + ".";
                Debug.Log(pokemonName + " use " + attackName + ".");
                break;
            default:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = pokemonName + " has improved his " + statusModified + " using " + attackName + ".";
                Debug.Log(pokemonName + " has improved his " + statusModified + " using " + attackName + ".");
                break;
        }
    }

    [PunRPC]
    public void PassAttackToHost(){
        player.GetComponent<Player>().Attack(player.GetComponent<Player>().selectedAttack);     
        GameObject canvas = GameObject.Find("Player_Canvas");
        //canvas.transform.Find("Attack_Panel").gameObject.SetActive(false);
        if(PhotonNetwork.IsMasterClient) StartCoroutine(StartAttacksCoroutine());
    }

    [PunRPC]
    public void theWinnerIs(int playerID){
        string message = "";
        if(player_id != playerID) message = "VICTORY!";
        else message = "DEFEAT...";

        information_Panel.SetActive(true);
        ImageCard_Panel.SetActive(false);
        information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = message;
        StartCoroutine(LoadEmptyScene());
    }
    
    #endregion

    #region PUN CALLBACKS
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _ShowAndroidToastMessage("Opponent trainer left the Match");
        StopAllCoroutines();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //PhotonNetwork.Disconnect();
        AudioManager.instance.PlayMusic(0);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        UnityEngine.XR.ARFoundation.ARSession.Destroy(FindObjectOfType<UnityEngine.XR.ARFoundation.ARSession>());
    }

    #endregion

    #region COROUTINES
    IEnumerator Timer()
    {
        while(ph_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);      
            ph_currentTime--;
            view.RPC("RefreshTimer_UI", RpcTarget.All, ph_currentTime);
        }
        h_TimerCoroutine = null;
        if(turn == 1) GameManager.instance.view.RPC("PassTurn", RpcTarget.All);
        else view.RPC("PassAttackToHost", RpcTarget.All);
    }

    IEnumerator nextAttackTurn()
    {
        yield return new WaitForSeconds(2f);
        AttackTurn();
    }

    IEnumerator StartAttacksCoroutine()
    {
        yield return new WaitUntil(()=> TurnManagerRequest.instance.requests.Count > 1);
        yield return new WaitForSeconds(2f);
        TurnManagerRequest.instance.StartAttacks(); 
    }

    IEnumerator TheresAWinnerCoroutine(int playerID)
    {
        yield return new WaitForSeconds(2f);
        view.RPC("theWinnerIs", RpcTarget.All, playerID);
    }

    IEnumerator LoadEmptyScene()
    {
        yield return new WaitForSeconds(2f);
        ph_LeaveRoom();
    }
    #endregion

    #region PUBLIC METHODS
    public void ph_LeaveRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        StopAllCoroutines();
        PhotonNetwork.LeaveRoom();
    }

    public void RefresUIAttacksResult(string pokemonAttacked, float effective, bool TheresAWinner, int playerID)
    {
        information_Panel.SetActive(true);
        ImageCard_Panel.SetActive(false);
        switch (effective)
        {
            case 0:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = "It doesn't affect " + pokemonAttacked + ".";
                Debug.Log("It doesn't affect " + pokemonAttacked + ".");
                break;
            case 0.5f:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = "It's not very effective...";
                Debug.Log("It's not very effective...");
                break;
            case 2:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = "It's super effective!";
                Debug.Log("It's super effective!");
                break;
            default:
                information_Panel.GetComponent<TMPro.TextMeshProUGUI>().text = "It's effective.";
                Debug.Log("It's effective.");
                break;
        }
        if(TheresAWinner) StartCoroutine(TheresAWinnerCoroutine(playerID));
    }

    #endregion

    #region PRIVATE METHODS

    private void ChoosePokemonTurn(){
        turn = 1;
        GameObject canvas = GameObject.Find("Player_Canvas");
        canvas.transform.Find("Select_Pokemon_Panel").gameObject.SetActive(true);
        canvas.transform.Find("Attack_Panel").gameObject.SetActive(false);
        canvas.GetComponent<PlayerCanvasManager>().InitializeCanvas();


        InitializeTimer(ph_timerChoosePokemon);

    }

    public void AttackTurn(){
        turn = 2;
        GameObject canvas = GameObject.Find("Player_Canvas");
        canvas.transform.Find("Attack_Panel").gameObject.SetActive(true);
        canvas.transform.Find("Select_Pokemon_Panel").gameObject.SetActive(false);
        UpdatePokemonInformationCanvas[] auxPCanvas = FindObjectsOfType<UpdatePokemonInformationCanvas>();
        foreach (var item in auxPCanvas)
        {
            item.gameObject.SetActive(false);   
        }

        /*GameObject stadium = GameObject.Find("Stadium");

        GameObject auxCombatCanvas1 = stadium.transform.Find("Pokemon_2_Canvas").gameObject;
        GameObject auxCombatCanvas2 = stadium.transform.Find("Pokemon_1_Canvas").gameObject;

        auxCombatCanvas1.GetComponent<UpdateCombatCanvasInformation>().StartHacerCosas();
        auxCombatCanvas2.GetComponent<UpdateCombatCanvasInformation>().StartHacerCosas();
        */

        canvas.GetComponent<PlayerCanvasManager>().UpdateAttacks();

        InitializeTimer(ph_timerChooseAttack);

    }
    private void InitializeTimer(int time){
        information_Panel.SetActive(true);
        ImageCard_Panel.SetActive(false);
    
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
