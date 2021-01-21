using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PhotonTutorial.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel = null;
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
        public void FindRandomOpponent()
        {
            isJoiningRandomRoom = true;

            findOpponentPanel.SetActive(false);
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

            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Creating Room...";

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

            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Joining Room " + RoomNameField.text + "...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(RoomNameField.text.Equals("")?"Default" : RoomNameField.text);
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
                PhotonNetwork.JoinRoom(RoomNameField.text.Equals("")?"Default" : RoomNameField.text);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No clients are waiting for an opponent, creating a new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if(!RoomNameField.text.Equals("")) Debug.Log("The Room " + RoomNameField.text + " does not exist");
            else Debug.Log("You didn't enter any name");
            findOpponentPanel.SetActive(true);
            waitingStatusPanel.SetActive(false);
            isJoiningRoomByName = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MaxPlayersPerRoom)
            {
                if(isCreatingRoom) waitingStatusText.text = "Waiting For Opponent\nRoom Name: " + PhotonNetwork.CurrentRoom.Name;
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
            isJoiningRandomRoom = false;
            isCreatingRoom = false;
            isJoiningRoomByName = false;
            PhotonNetwork.LeaveRoom();

            findOpponentPanel.SetActive(true);
            waitingStatusPanel.SetActive(false);

            BackButton.SetActive(false);
        }
    }

}