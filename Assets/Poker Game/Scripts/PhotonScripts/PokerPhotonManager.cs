using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBot
{
    public string NickName;
    public int ActorNumber;

    public PlayerBot(string name, int number)
    {
        NickName = name;
        ActorNumber = number;
    }
}

public class PokerPhotonManager : MonoBehaviourPunCallbacks
{
    public static PokerPhotonManager instance;

    [SerializeField] private PhotonUI m_PhotonUI;

    public string playerName;
    private string playerProfileId;
    private string playerCountryId;
    private PhotonView netView;
    private bool isGameLoaded;

    private List<PlayerBot> playerBots;

    private int lastPlayerNumber;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        //m_PhotonUI.OnLogInPlayerButton += OnLogInPlayerButton;
        //m_PhotonUI.OnCreateRoomButton += OnCreateRoomButton;
        m_PhotonUI.OnJoinRoomButton += OnJoinRoomButton;
        //m_PhotonUI.OnStartGameButton += OnStartGameButton;

        //m_PhotonUI.OnAddBotButton += OnAddBotButton;

        netView = gameObject.GetComponent<PhotonView>();
        //netView.ViewID = 1;
        netView.Synchronization = ViewSynchronization.Off;
    }

    private int PlayersCount => PhotonNetwork.CurrentRoom.PlayerCount + (playerBots != null ? playerBots.Count : 0);


  

    public override void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
        m_PhotonUI.gameObject.SetActive(true);
        m_PhotonUI.DisconnectedFromServer();        
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }    

    public void OnLogInPlayerButton(string playerName)
    {
        this.playerName = playerName;
        //Invoke("OnLogin", 5);
        PhotonNetwork.ConnectUsingSettings();
    }

    //void OnLogin()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    public void OnCreateRoomButton()
    {        
        Debug.Log("OnCreateRoomButton");
        
        string roomName = "Table_" + Random.Range(1000, 9999) + "." + m_PhotonUI.roomType;
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)m_PhotonUI.currentPlayerCount
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

   

    private void OnJoinRoomButton(PokerRoomInfo pokerRoomInfo)
    {
        PhotonNetwork.JoinRoom(pokerRoomInfo.name);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = playerName;
        
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");

        if (SceneManager.GetActiveScene().name == "PokerGame")
        {
            GameManager.instance.DisconnectionPanel.SetActive(false);
        }
        else
        {
            m_PhotonUI.DisconnectionPanel.SetActive(false);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        playerBots = new List<PlayerBot>();
        
        m_PhotonUI.ConnectedToServer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);

        print("----PlayerList---->>" + PhotonNetwork.PlayerList.Length + " <<<--ActorNumber--->>>> " + PhotonNetwork.LocalPlayer.ActorNumber + " <<<--Current room--->>>> " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            StartCoroutine(GameManager.instance.SetGame());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched " + newMasterClient.NickName);
        base.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom " + otherPlayer.NickName);
        if (PlayersCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            if (!isGameLoaded)
            {
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }
        m_PhotonUI.PlayersCountUpdated(PlayersCount, PhotonNetwork.CurrentRoom.MaxPlayers);

        //Debug.Log("IsMasterClient " + otherPlayer.IsMasterClient);
        //if (otherPlayer.IsMasterClient)
        //{
        //    PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
        //Debug.Log("IsMasterClient after" + PhotonNetwork.MasterClient.NickName);
        //}

        for (int i = 0; i < GameManager.instance.Players.Count; i++)
        {
        Debug.Log("OnPlayerLeftRoom-->> " + otherPlayer.NickName + " <<--name--->>" + GameManager.instance.Players[i].player_name.text.Trim());
            if (GameManager.instance.Players[i].player_name.text.Trim() == otherPlayer.NickName.Trim())
            {
                PlayerScript player = GameManager.instance.Players[i];
                if (player.currentState != PlayerStates.NotPlaying)
                {
                    GameManager.instance.PlayerSeats_id_used.Remove(player.seat);
                    GameManager.instance.PlayerSeats_id.Add(player.seat);
                    player.gameObject.SetActive(false);
                    GameManager.instance.winnerlogic.players.Remove(player);
                    player.currentState = PlayerStates.NotPlaying;
                    player.cardsObject.SetActive(false);
                    if (GameManager.instance.winnerlogic.players.Count > 1)
                    {
                        print("--------NextTurn-aa------->>>> ");
                        if(!player.stopTimer)
                            GameManager.instance.NextTurn();
                    }
                    else
                    {
                        GameManager.instance.EndOfTheGame(0);
                    }
                }
                return;
            }
        }
        
    }

    
    public override void OnCreatedRoom()
    {
        print("OnCreatedRoom -->>> " + PhotonNetwork.CurrentRoom.MaxPlayers);
        Debug.Log("OnCreatedRoom");
        m_PhotonUI.CreatedRoom(PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void OnCreateRoom_Node() 
    {
        print("OnCreatedRoom -->>> " + m_PhotonUI.currentPlayerCount);
        Debug.Log("OnCreatedRoom");
        m_PhotonUI.CreatedRoom_Node(m_PhotonUI.currentPlayerCount);
    }
    

    public override void OnJoinedRoom()
    {
        print("---------OnJoinedRoom------------>>" + PhotonNetwork.CountOfPlayers);
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            lastPlayerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log("OnJoinedRoom " + lastPlayerNumber);
        }
        m_PhotonUI.JoinedRoom(PhotonNetwork.LocalPlayer.IsMasterClient,
            PlayersCount, PhotonNetwork.CurrentRoom.MaxPlayers);

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("AvtarId", UserInfo.instance._avtarId);
        hash.Add("CountryId", UserInfo.instance._countryId);
        print("----PlayerList---->>" + PhotonNetwork.PlayerList.Length + " <<<--ActorNumber--->>>> " + PhotonNetwork.LocalPlayer.player_id + " <<<--Current room--->>>> " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LocalPlayer.player_id = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.PlayerList[PhotonNetwork.LocalPlayer.player_id - 1].SetCustomProperties(hash);

        //Invoke("LoadLevel", 1);
        LoadLevel();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("------OnDisconnected-----" + cause.ToString());
        if (cause.ToString() != "DisconnectByClientLogic")
        {
            if (SceneManager.GetActiveScene().name == "PokerGame")
            {
                GameManager.instance.DisconnectionPanel.SetActive(true);
                StartCoroutine(HandleDisconnection());
            }
            else
            {
                m_PhotonUI.DisconnectionPanel.SetActive(true);
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    IEnumerator HandleDisconnection()
    {
        yield return new WaitForSeconds(3);
        GameManager.instance.LeaveRoom_Ok();
    }

    void LoadLevel()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("PokerGame");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate x " + roomList.Count);
        PokerRoomInfo[] rooms = new PokerRoomInfo[roomList.Count];
        Debug.Log("OnRoomListUpdate xx " + roomList.Count);
        for (int i = 0; i < rooms.Length; i++)
        {
        Debug.Log("OnRoomListUpdate xxx " + roomList.Count);
            RoomInfo roomInfo = roomList[i];
            rooms[i] = new PokerRoomInfo(roomInfo.Name, roomInfo.MaxPlayers, roomInfo.PlayerCount, roomInfo.IsOpen);
        }

        Debug.Log("OnRoomListUpdate xxx " + roomList.Count);
        m_PhotonUI.RoomListUpdate(rooms);
    }

    private void OnApplicationQuit()
    {
        print("application quit");
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        //PlayerPrefs.SetInt("AutoLogin", 0);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
           
            PlayerPrefs.SetInt("AutoLogin", 0);
            print("------------pause---------------" + PlayerPrefs.GetInt("AutoLogin"));
        }
        else
        {
            print("------------pause----false-----------");
            if (SceneManager.GetActiveScene().name == "PokerGame")
            {
                PlayerPrefs.SetInt("AutoLogin", 1);
                print("------------pause----false-----------" + PlayerPrefs.GetInt("AutoLogin"));
            }

        }

    }    
}