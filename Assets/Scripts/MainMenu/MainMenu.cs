using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PhotonTutorial.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject CreateOrJoinPanel = null;
        [SerializeField] private GameObject RandomOrJoinMatchPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;
        [SerializeField] private TMP_InputField RoomNameField = null;
        [SerializeField] private GameObject BackButton = null;

        private bool isJoiningRandomRoom = false;
        private bool isCreatingRoom = false;
        private bool isJoiningRoomByName = false;
        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        private void Start() {
            isJoiningRandomRoom = false;
            isCreatingRoom = false;
            isJoiningRoomByName = false;
        }
        public void FindRandomOpponent()
        {
            isJoiningRandomRoom = true;

            RandomOrJoinMatchPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void CreateRoom()
        {
            isCreatingRoom = true;

            CreateOrJoinPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Creating Match...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(f_GetRandomMatchID(), new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
         public void JoinRoomByName()
        {
            isJoiningRoomByName = true;

            RandomOrJoinMatchPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Joining Match " + RoomNameField.text.ToUpper() + "...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(RoomNameField.text.Equals("")?"Default" : RoomNameField.text.ToUpper());
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master");

            if (isJoiningRandomRoom)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else if(isCreatingRoom)
            {
                PhotonNetwork.CreateRoom(f_GetRandomMatchID(), new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
            }
            else if(isJoiningRoomByName)
            {
                PhotonNetwork.JoinRoom(RoomNameField.text.Equals("")?"Default" : RoomNameField.text.ToUpper());
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            RandomOrJoinMatchPanel.SetActive(false);
            CreateOrJoinPanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
            _ShowAndroidToastMessage($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No trainers are waiting for an opponent, creating a new Match");
            _ShowAndroidToastMessage("No trainers are waiting for an opponent, creating a new Match");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if(!RoomNameField.text.Equals(""))
            {
                Debug.Log("The Match " + RoomNameField.text + " does not exist");
                _ShowAndroidToastMessage("The Match " + RoomNameField.text.ToUpper() + " does not exist");
            } 
            else
            {
                Debug.Log("You didn't enter any Match name");
                _ShowAndroidToastMessage("You didn't enter any Match name");
            } 
            RandomOrJoinMatchPanel.SetActive(true);
            waitingStatusPanel.SetActive(false);
            isJoiningRoomByName = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a Match");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MaxPlayersPerRoom)
            {
                if(isCreatingRoom) waitingStatusText.text = "Waiting For Opponent\nMatch Name: " + PhotonNetwork.CurrentRoom.Name;
                else waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
                BackButton.SetActive(true);
            }
            else
            {
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen  = false;

                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");

                PhotonNetwork.LoadLevel(1);
            }
        }

        /// <summary>
        /// Generates a random Room Name, so other player can join it.
        /// </summary>
        /// <returns></returns>
        public static string f_GetRandomMatchID(){
            string _id = string.Empty;
            for (int i = 0; i < 5; i++) {
                int random = UnityEngine.Random.Range (0, 36);
                if (random < 26) {
                    _id += (char) (random + 65);
                } else {
                    _id += (random - 26).ToString ();
                }
            }
            Debug.Log ($"Random Match ID: {_id}");
            return _id;
        }

        public void f_BackToCreateOrJoinRoomPanel()
        {
            PhotonNetwork.LeaveRoom();

            CreateOrJoinPanel.SetActive(isCreatingRoom);
            RandomOrJoinMatchPanel.SetActive(isJoiningRandomRoom || isJoiningRoomByName);
            waitingStatusPanel.SetActive(false);

            isJoiningRandomRoom = false;
            isCreatingRoom = false;
            isJoiningRoomByName = false;

            BackButton.SetActive(false);
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
    }

}