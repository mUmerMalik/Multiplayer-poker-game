using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

[Serializable]
public class PokerRoomInfo
{
    public string name;
    public byte maxPlayers;
    public int connectedPlayers;
    public bool isOpen;
    public string roomId;

    public PokerRoomInfo(string name, byte maxPlayers, int connectedPlayers, bool isOpen)
    {
        this.name = name;
        this.maxPlayers = maxPlayers;
        this.connectedPlayers = connectedPlayers;
        this.isOpen = isOpen;
    }
    public PokerRoomInfo(string name, byte maxPlayers, int connectedPlayers, bool isOpen, string roomId)
    {
        this.name = name;
        this.maxPlayers = maxPlayers;
        this.connectedPlayers = connectedPlayers;
        this.isOpen = isOpen;
        this.roomId = roomId;
    }
}

public class PhotonUI : MonoBehaviour
{
    [Space(10), Header("Player")]
    [SerializeField] private Transform m_PlayerPanel;
    [SerializeField] private InputField m_PlayerNameText;
    [SerializeField] private Button m_LogInPlayerButton;
    [SerializeField] private Button m_GetStartedButton;
    [Space(10), Header("Room")]
    [SerializeField] private Transform m_RoomsPanel;
    [SerializeField] private PhotonRoomItemUI m_PhotonRoomItemUIPrefab;
    [SerializeField] private Transform m_RoomItemsContent;
    [SerializeField] private InputField m_PlayersMaxCount;
    [SerializeField] private Button m_CreateRoomButton;
    //[SerializeField] private Button m_CreateRoomButton_Ok;
    [Space(10), Header("Start Game")]
    //[SerializeField] private Transform m_StartGamePanel;
    [SerializeField] private GameObject m_wait_masterPanel;
    [SerializeField] private Button m_StartGameButton;
    [SerializeField] private Button m_AddBotButton;
    //[SerializeField] private Text m_ConnectedPlayersText;
    [SerializeField] private Text m_ConnectedPlayersText_master;
    [Space(10), Header("Wait Game")]
    [SerializeField] private Transform m_WaitGamePanel;
    [SerializeField] private Transform m_JoinGamePanel;
    [SerializeField] private Text m_RoomPlayersText;

    //public Action<string> OnLogInPlayerButton;
    //public Action<string, int> OnCreateRoomButton;
    public Action<PokerRoomInfo> OnJoinRoomButton;
    //public Action OnStartGameButton;
    //public Action OnAddBotButton;

    private PhotonRoomItemUI[] roomsUI;

    public delegate void Resetdata();
    public static event Resetdata _resetData;

    public GameObject LoginPanel;
    public GameObject SignUpPanel_1;
    public GameObject SignUpPanel_2;
    public GameObject SignUpPanel_3;
    public GameObject ForgotPassPanel;
    public GameObject SelectCountryPanel;
    public GameObject SelectAvtarPanel;
    public GameObject tableListPanel;
    public GameObject CreateTablePanel;
    public GameObject DisconnectionPanel;
    public GameObject ExitPanel;
    public GameObject ChangeAvtarPanel;
    public GameObject BalancePanel;
    public GameObject WithdrawHistoryPanel;
    public GameObject DepositHistoryPanel;
    public GameObject LogoutPanel;
    public GameObject CardsPanel;
    public GameObject LanguagePanel;
    public GameObject SoundsPanel;
    public GameObject MusicPanel;
    public GameObject TermsConditionsPanel;
    public GameObject PrivacyPolicyPanel;
    public GameObject GameRulesPanel;
    public GameObject SettingsPanel;
    public GameObject MePanel;

    public int currentPlayerCount;
    public Image select6;
    public Image select9;
    public Image select6_dot;
    public Image select9_dot;

    public Sprite selectSprite;
    public Sprite deselectSprite;
    public Sprite selectSprite_dot;
    public Sprite deselectSprite_dot;

    public Image selectEnglish;
    public Image selectJapanese;
    public Sprite selectSprite_language;
    public Sprite deselectSprite_language;

    public string roomType;

    public GameObject noRoomText;

    public LobbyOfTables lobby_5_10;
    public LobbyOfTables lobby_2_5;
    public LobbyOfTables lobby_1_2;
    public LobbyOfTables lobby_05_1;
    public LobbyOfTables lobby_025_05;
    public LobbyOfTables lobby_01_025;
    public LobbyOfTables lobby_005_01;
    public LobbyOfTables lobby_002_005;
    public LobbyOfTables lobby_001_002;    

    PokerRoomInfo[] allRooms;// = new List<PokerRoomInfo>();

    public List<GameObject> frontDeckOutlines;
    public List<GameObject> backDeckOutlines;

    public Scrollbar musicScroll;
    public Scrollbar soundScroll;

    public Image soundImage;
    public Image musicImage;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public Text musicScrollValText;
    public Text soundScrollValText;    

    private void Awake()
    {
        m_PhotonRoomItemUIPrefab.gameObject.SetActive(false);

        m_WaitGamePanel.gameObject.SetActive(false);
        m_RoomsPanel.gameObject.SetActive(false);
        m_PlayerPanel.gameObject.SetActive(true);
    }

    private void Start()
    {
        frontDeckOutlines[PlayerPrefs.GetInt("FrontDeck", 0)].SetActive(true);
        backDeckOutlines[PlayerPrefs.GetInt("BackDeck", 0)].SetActive(true);

        soundScroll.value = PlayerPrefs.GetFloat("SoundEffect", 0.5f);
        musicScroll.value = PlayerPrefs.GetFloat("BGMusic", 0.5f);

        soundScrollValText.text = (soundScroll.value * 100).ToString("0") + "%";
        musicScrollValText.text = (musicScroll.value * 100).ToString("0") + "%";

        if (musicScroll.value == 0)
        {
            musicImage.sprite = musicOffSprite;
        }
        else
        {
            musicImage.sprite = musicOnSprite;
        }
        if (soundScroll.value == 0)
        {
            soundImage.sprite = soundOffSprite;
        }
        else
        {
            soundImage.sprite = soundOnSprite;
        }

        if (PlayerPrefs.GetInt("Language", 0) == 0)
        {
            selectEnglish.sprite = selectSprite_language;
            selectJapanese.sprite = deselectSprite_language;
        }
        else
        {
            selectEnglish.sprite = deselectSprite_language;
            selectJapanese.sprite = selectSprite_language;
        }
        _resetData();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Back();
    }

    public void DisconnectedFromServer()
    {
        m_WaitGamePanel.gameObject.SetActive(false);
        m_RoomsPanel.gameObject.SetActive(false);
        //m_StartGamePanel.gameObject.SetActive(false);
        m_PlayerPanel.gameObject.SetActive(true);
    }

    public void ConnectedToServer()
    {
        print("-----ConnectedToServer -->>> ");
        //m_RoomsPanel.gameObject.SetActive(true);
        //m_StartGamePanel.gameObject.SetActive(false);

        //m_CreateRoomButton.onClick.RemoveAllListeners();
        //m_CreateRoomButton.onClick.AddListener(() =>
        //{
        //    int.TryParse(currentPlayerCount.ToString(), out int playersMaxCount);
        //    playersMaxCount = currentPlayerCount;
        //    int roomCount = roomsUI == null ? 0 : roomsUI.Length;
        //OnCreateRoomButton?.Invoke("Table_" + UnityEngine.Random.Range(1000, 9999) + "." + roomType, playersMaxCount);
        //});
    }

    public void CreatedRoom(int maxPlayers)
    {
        print("CreatedRoom -->>> " + maxPlayers);

        m_RoomsPanel.gameObject.SetActive(false);
        m_wait_masterPanel.gameObject.SetActive(true);
    }

    public void CreatedRoom_Node(int maxPlayers)
    {
        print("CreatedRoom -->>> " + maxPlayers);

        m_RoomsPanel.gameObject.SetActive(false);
        //m_wait_masterPanel.gameObject.SetActive(true);
        ServerManager.instance.OnCreateRoom(maxPlayers);
    }

    public void PlayersCountUpdated(int playersCount, int maxPlayers)
    {
        //SetConnectedPlayersText(playersCount, maxPlayers);
        //m_AddBotButton.interactable = playersCount < maxPlayers;
    }

    public void JoinedRoom(bool isMasterPlayer, int playersCount, int maxPlayers)
    {
        print("PlayerList -->>> " + PhotonNetwork.PlayerList.Length + " ---->>>> " + (PhotonNetwork.LocalPlayer.ActorNumber-1));
        //PhotonNetwork.PlayerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].NickName = UserInfo.instance._username + "." + UserInfo.instance._avtarId + "." + UserInfo.instance._countryId;
        

        //if (!isMasterPlayer)
        //{
        //    print("----------JoinedRoom----------------");
        //    m_RoomsPanel.gameObject.SetActive(false);
        //    m_JoinGamePanel.gameObject.SetActive(false);
        //    m_WaitGamePanel.gameObject.SetActive(true);

        //    //OnStartGameButton();
        //}

        //SetConnectedPlayersText(playersCount, maxPlayers);
    }

    private void SetConnectedPlayersText(int playersCount, int maxPlayers)
    {
        print("SetConnectedPlayersText -->>> " + maxPlayers);
        //m_ConnectedPlayersText.text = "Connected players: " + playersCount + " from " + maxPlayers;
        m_ConnectedPlayersText_master.text = "Connected players: " + playersCount + " from " + maxPlayers;
        m_RoomPlayersText.text = "Connected players: " + playersCount + " from " + maxPlayers;
    }

    public void ActivateStartGameButton()
    {
        //m_StartGameButton.interactable = true;
        m_AddBotButton.interactable = false;
    }

    public void RoomListUpdate(PokerRoomInfo[] rooms)
    {
        Debug.Log("OnRoomListUpdate xxxx ");
        allRooms = rooms;
        //if (roomsUI != null)
        //{
        //Debug.Log("OnRoomListUpdate xxxxx ");
        //    foreach (var roomUI in roomsUI)
        //    {
        //        Destroy(roomUI.gameObject);
        //    }
        //}
        Debug.Log("OnRoomListUpdate xxxxxx ");
        roomsUI = new PhotonRoomItemUI[rooms.Length];
        print("---RoomListUpdate4yhrthrthr---->>> " + rooms.Length);
        for (int i = 0; i < rooms.Length; i++)
        {
            bool add = true;
            print("---rooms["+i+ "].name---->>> " + rooms[i].name);
            if(rooms[i].name.Contains(".5_10"))
            {
                int index = lobby_5_10._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_5_10._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_5_10._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_5_10._rooms.Add(rooms[i]);
                }
                // lobby_5_10._rooms.Add(rooms[i]);
                lobby_5_10.SetValues();
            }
            if (rooms[i].name.Contains(".2_5"))
            {
                int index = lobby_2_5._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_2_5._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_2_5._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_2_5._rooms.Add(rooms[i]);
                }
                //if (!lobby_2_5._rooms.Contains(rooms[i]))
                //   lobby_2_5._rooms.Add(rooms[i]);
                lobby_2_5.SetValues();
            }
            if (rooms[i].name.Contains(".1_2"))
            {
                int index = lobby_1_2._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_1_2._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_1_2._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_1_2._rooms.Add(rooms[i]);
                }
                // if (!lobby_1_2._rooms.Contains(rooms[i]))
                //    lobby_1_2._rooms.Add(rooms[i]);
                lobby_1_2.SetValues();
            }
            if (rooms[i].name.Contains(".0.5_1"))
            {
                int index = lobby_05_1._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_05_1._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_05_1._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_05_1._rooms.Add(rooms[i]);
                }

                // if (!lobby_05_1._rooms.Contains(rooms[i]))
                //   lobby_05_1._rooms.Add(rooms[i]);
                lobby_05_1.SetValues();
            }
            if (rooms[i].name.Contains(".0.25_0.5"))
            {
                int index = lobby_025_05._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_025_05._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_025_05._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_025_05._rooms.Add(rooms[i]);
                }
                // if (!lobby_025_05._rooms.Contains(rooms[i]))
                //    lobby_025_05._rooms.Add(rooms[i]);
                lobby_025_05.SetValues();
            }
            if (rooms[i].name.Contains(".0.1_0.25"))
            {
                int index = lobby_01_025._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_01_025._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_01_025._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_01_025._rooms.Add(rooms[i]);
                }
                // if (!lobby_01_025._rooms.Contains(rooms[i]))
                //   lobby_01_025._rooms.Add(rooms[i]);
                lobby_01_025.SetValues();
            }
            if (rooms[i].name.Contains(".0.05_0.1"))
            {
                int index = lobby_005_01._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_005_01._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_005_01._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_005_01._rooms.Add(rooms[i]);
                }
                // if (!lobby_005_01._rooms.Contains(rooms[i]))
                //  lobby_005_01._rooms.Add(rooms[i]);
                lobby_005_01.SetValues();
            }
            if (rooms[i].name.Contains(".0.02_0.05"))
            {
                int index = lobby_002_005._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_002_005._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_002_005._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_002_005._rooms.Add(rooms[i]);
                }
                // if (!lobby_002_005._rooms.Contains(rooms[i]))
                //   lobby_002_005._rooms.Add(rooms[i]);
                lobby_002_005.SetValues();
            }
            if (rooms[i].name.Contains(".0.01_0.02"))
            {
                int index = lobby_001_002._rooms.FindIndex(s => s.name == rooms[i].name);

                if (index != -1)
                {
                  //  print("-----------foundInstance----index------=" + index);
                   // print("-----------foundInstance----foundInstance.connectedPlayers------=" + rooms[i].connectedPlayers);
                    if (rooms[i].connectedPlayers == 0)
                    {
                        lobby_001_002._rooms.RemoveAt(index);
                    }
                    else
                    {
                        lobby_001_002._rooms[index] = rooms[i];
                    }
                }
                else
                {
                    lobby_001_002._rooms.Add(rooms[i]);
                }
                // if (!lobby_001_002._rooms.Contains(rooms[i]))
                //   lobby_001_002._rooms.Add(rooms[i]);
                lobby_001_002.SetValues();
            }
        }
        //print("---lobby_5_10._rooms---->>> " + lobby_5_10._rooms.Count);
        //for (int i = 0; i < lobby_5_10._rooms.Count; i++)
        //{
            //print("---lobby_5_10._room["+i+"]---->>> " + lobby_5_10._rooms[i].name);
        //}
    }

    public void MyRoomListUpdate(PokerRoomInfo[] rooms)
    {
        //if (roomsUI != null)
        //{
        //    foreach (var roomUI in roomsUI)
        //    {
        //        Destroy(roomUI.gameObject);
        //    }
        //}
        //print("m_RoomItemsContent.childCount -->> " + m_RoomItemsContent.childCount);
        roomsUI = new PhotonRoomItemUI[rooms.Length];
        //for(int i = 0; i < m_RoomItemsContent.childCount; i++)
        //{
        //    Destroy(m_RoomItemsContent.GetChild(i).gameObject);
        //}
        if (rooms.Length != 0)
        {
            print("-------rooms.Length--------" + rooms.Length);
            for (int i = 0; i < rooms.Length; i++)
            {
                roomsUI[i] = Instantiate(m_PhotonRoomItemUIPrefab,
                    m_RoomItemsContent.position, m_RoomItemsContent.rotation, m_RoomItemsContent);
                roomsUI[i].pokerRoomInfo = rooms[i];
                roomsUI[i].gameObject.SetActive(true);
                PokerRoomInfo pokerRoomInfo = rooms[i];
                roomsUI[i].Set(pokerRoomInfo, () =>
                    {
                        OnJoinRoomButton?.Invoke(pokerRoomInfo);
                    });
            }
        }
    }

    public void JoinTableButton(PhotonRoomItemUI roomUI)
    {
        m_JoinGamePanel.gameObject.SetActive(true);
        roomUI._Set(roomUI.pokerRoomInfo, () =>
        {
            OnJoinRoomButton?.Invoke(roomUI.pokerRoomInfo);
        });
    }

    public void PanelChange(GameObject panel)
    {
        //LoginPanel.SetActive(false);
        SignUpPanel_1.SetActive(false);
        SignUpPanel_2.SetActive(false);
        SignUpPanel_3.SetActive(false);
        ForgotPassPanel.SetActive(false);
        SelectCountryPanel.SetActive(false);
        SelectAvtarPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void ShowTableList(LobbyOfTables _lobby)
    {
        roomType = _lobby.lobbyName;
        //int allRooms = PhotonNetwork.CountOfRooms;
        print("Rooms -->> " + _lobby._rooms.Count);
        print("m_RoomItemsContent.childCount -->> " + m_RoomItemsContent.childCount);
        for (int i = 0; i < m_RoomItemsContent.childCount; i++)
        {
            Destroy(m_RoomItemsContent.GetChild(i).gameObject);
        }
        //List<PokerRoomInfo> pokerRooms = new List<PokerRoomInfo>();
        if (_lobby._rooms.Count != 0)
        {
            noRoomText.SetActive(false);
            MyRoomListUpdate(_lobby._rooms.ToArray());
        }
        else
        {
            noRoomText.SetActive(true);

        }

        tableListPanel.SetActive(true);

        PlayerPrefs.SetFloat("MIN_VAL", _lobby.min_value);
        PlayerPrefs.SetFloat("MAX_VAL", _lobby.max_value);
        PlayerPrefs.SetFloat("BUYIN_MIN_VAL", _lobby.buyIn_min_value);
        PlayerPrefs.SetFloat("BUYIN_MAX_VAL", _lobby.buyIn_max_value);
    }    

    public void CreateTableButton()
    {
        CreateTablePanel.SetActive(true);
    }

    public void SelectPlayer_6()
    {
        currentPlayerCount = 6;
        select6.sprite = selectSprite;
        select9.sprite = deselectSprite;
        select6_dot.sprite = selectSprite_dot;
        select9_dot.sprite = deselectSprite_dot;
        m_CreateRoomButton.interactable = true;
    }

    public void SelectPlayer_9()
    {
        currentPlayerCount = 9;
        select9.sprite = selectSprite;
        select6.sprite = deselectSprite;
        select9_dot.sprite = selectSprite_dot;
        select6_dot.sprite = deselectSprite_dot;
        m_CreateRoomButton.interactable = true;
    }

    public void CreateTableButtonClick()
    {
        if (PlayerPrefs.GetFloat("BUYIN_MIN_VAL") <= float.Parse(UserInfo.instance._balance))
        {
            CreateTablePanel.SetActive(true);
        }
        else
        {
            UserInfo.instance.errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                UserInfo.instance.errorMessageText_popUp.text = "残高不足";
            else
                UserInfo.instance.errorMessageText_popUp.text = "Insufficient Balance";
        }
    }

    public void SelectFrontDeck(int deckId)
    {
        frontDeckOutlines[PlayerPrefs.GetInt("FrontDeck", 0)].SetActive(false);
        PlayerPrefs.SetInt("FrontDeck", deckId);
        frontDeckOutlines[deckId].SetActive(true);
    }

    public void SelectBackDeck(int deckId)
    {
        backDeckOutlines[PlayerPrefs.GetInt("BackDeck", 0)].SetActive(false);
        PlayerPrefs.SetInt("BackDeck", deckId);
        backDeckOutlines[deckId].SetActive(true);
    }

    public void Select_English()
    {
        PlayerPrefs.SetInt("Language", 0);
        selectEnglish.sprite = selectSprite_language;
        selectJapanese.sprite = deselectSprite_language;
        _resetData();
    }

    public void Select_Japanese()
    {
        PlayerPrefs.SetInt("Language", 1);
        selectEnglish.sprite = deselectSprite_language;
        selectJapanese.sprite = selectSprite_language;
        _resetData();
    }

    public void SetSoundVolume()
    {
        soundScrollValText.text = (soundScroll.value * 100).ToString("0") + "%";
        SoundManager.instance.soundEffect.volume = soundScroll.value;
        SoundManager.instance.dealerVoice.volume = soundScroll.value;
        PlayerPrefs.SetFloat("SoundEffect", soundScroll.value);
        PlayerPrefs.SetFloat("DealerVoice", soundScroll.value);

        if(soundScroll.value == 0)
        {
            soundImage.sprite = soundOffSprite;
        }
        else
        {
            soundImage.sprite = soundOnSprite;
        }
    }

    public void SetMusicVolume()
    {
        musicScrollValText.text = (musicScroll.value * 100).ToString("0") + "%";
        SoundManager.instance.bgMusic.volume = musicScroll.value;
        PlayerPrefs.SetFloat("BGMusic", musicScroll.value);

        if (musicScroll.value == 0)
        {
            musicImage.sprite = musicOffSprite;
        }
        else
        {
            musicImage.sprite = musicOnSprite;
        }
    }


    public void Back()
    {
        if (m_wait_masterPanel.activeSelf || UserInfo.instance.errorPopUpPanel.activeSelf)
        {
            return;
        }
        else if(UserInfo.instance.verifyEmailPanel.activeSelf)
        {
            UserInfo.instance.verifyEmailPanel.SetActive(false);
        }
        else if (DisconnectionPanel.activeSelf)
        {
            ExitPanel.SetActive(true);
        }
        else if (ExitPanel.activeSelf)
        {
            ExitPanel.SetActive(false);
        }
        else if (CreateTablePanel.activeSelf)
        {
            CreateTablePanel.SetActive(false);
        }
        else if (UserInfo.instance.AddBalancePanel.activeSelf)
        {
            UserInfo.instance.AddBalancePanel.SetActive(false);
        }
        else if (tableListPanel.activeSelf)
        {
            tableListPanel.SetActive(false);
        }
        else if (BalancePanel.activeSelf)
        {
            BalancePanel.SetActive(false);
        }
        else if (WithdrawHistoryPanel.activeSelf)
        {
            WithdrawHistoryPanel.SetActive(false);
        }
        else if (DepositHistoryPanel.activeSelf)
        {
            DepositHistoryPanel.SetActive(false);
        }
        else if (LogoutPanel.activeSelf)
        {
            LogoutPanel.SetActive(false);
        }
        else if(ChangeAvtarPanel.activeSelf)
        {
            ChangeAvtarPanel.SetActive(false);
        }
        else if (UserInfo.instance.myprofilePanel.activeSelf)
        {
            UserInfo.instance.myprofilePanel.SetActive(false);
        }
        else if (CardsPanel.activeSelf)
        {
            CardsPanel.SetActive(false);
        }
        else if (LanguagePanel.activeSelf)
        {
            LanguagePanel.SetActive(false);
        }
        else if (SoundsPanel.activeSelf)
        {
            SoundsPanel.SetActive(false);
        }
        else if (MusicPanel.activeSelf)
        {
            MusicPanel.SetActive(false);
        }
        else if (TermsConditionsPanel.activeSelf)
        {
            TermsConditionsPanel.SetActive(false);
        }
        else if (PrivacyPolicyPanel.activeSelf)
        {
            PrivacyPolicyPanel.SetActive(false);
        }
        else if (GameRulesPanel.activeSelf)
        {
            GameRulesPanel.SetActive(false);
        }
        else if (SettingsPanel.activeSelf)
        {
            SettingsPanel.SetActive(false);
            UserInfo.instance.holdemPanel.SetActive(true);
        }
        else if (MePanel.activeSelf)
        {
            MePanel.SetActive(false);
            UserInfo.instance.holdemPanel.SetActive(true);
        }
        else if (UserInfo.instance.holdemPanel.activeSelf)
        {
            ExitPanel.SetActive(true);
        }
        else if (ForgotPassPanel.activeSelf)
        {
            ForgotPassPanel.SetActive(false);
        }
        else if (SelectAvtarPanel.activeSelf)
        {
            SelectAvtarPanel.SetActive(false);
        }
        else if (SignUpPanel_3.activeSelf)
        {
            SignUpPanel_3.SetActive(false);
        }
        else if (SignUpPanel_2.activeSelf)
        {
            SignUpPanel_2.SetActive(false);
        }
        else if (SignUpPanel_1.activeSelf)
        {
            SignUpPanel_1.SetActive(false);
        }
        else if (SelectCountryPanel.activeSelf)
        {
            SelectCountryPanel.SetActive(false);
        }
        else if (LoginPanel.activeSelf)
        {
            ExitPanel.SetActive(true);
        }
    }
}
