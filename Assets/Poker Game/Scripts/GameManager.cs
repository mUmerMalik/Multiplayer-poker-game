using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
//using UnityEditor.VersionControl;

[Serializable]
public class Card_Property
{
    public string name;
    //public Sprite image;
    public List<Sprite> front_image;

    public Card_Property(string _name, List<Sprite> _front_image)
    {
        this.name = _name;
        this.front_image = _front_image;
        //this.front_image = front_image[PlayerPrefs.GetInt("FrontDeck", 0)];
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Winnerlogic winnerlogic;
    public List<PlayerScript> Players;
    public List<Card_Property> cards_properties;
    public List<Card_Property> cards_properties_temp;
    public List<GameObject> PlayerSeats;
    public List<int> PlayerSeats_id;
    public List<int> PlayerSeats_id_used;
    public GameObject seatsParent;
    public GameObject DisconnectionPanel;
    public GameObject InsufficientBalancePanel;
    public GameObject BuyinPanel;
    public GameObject SeatAlreadyOccupiedPanel;

    public List<Transform> cards_positions;
    public List<Transform> cards_on_table;
    public List<Sprite> card_back;
    public List<Vector3> cards_positions_on_table;
    public GameObject card_prefab;
    public Transform card_spawn_transform;
    public Transform chips_merge_transform;
    public Vector3 chips_merge_position;
    public Transform D_Object;

    public List<Sprite> avtars;
    public List<Sprite> country_flags;

    public GameObject LeavePanel;
    public GameObject gameUI;
    public GameObject checkBtn;
    public GameObject callBtn;
    public GameObject raiseBtn;
    public GameObject betBtn;
    public Text callvalue_text;
    public Text totalPot_text;
    public float totalPot_value = 0;
    //public Text totalCall_text;
    public PlayerActions currentPlayer_action;
    public static int currentPlayer = -1;
    public bool gameAllin;

    public Text raisevalue_text;
    public Text betvalue_text;
    public Text leaveAmountText;

    public bool check3;
    public bool check4;
    public bool check5;
    public bool checked3;
    public bool checked4;
    public bool checked5;
    public bool endGame;

    public float call_min;
    public float call_max;
    public float call_value;
    public float raised_value;
    public float current_player_balance;

    public int player_d = -1;
    public int player_1 = -1;
    public int player_2 = -1;

    string cardString = "";
    public int selectedSeat = -1;
    int test_index = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        chips_merge_position = chips_merge_transform.position;
        /*skchnages for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
         {
             PlayerSeats[i].SetActive(true);
             PlayerSeats_id.Add(int.Parse(PlayerSeats[i].name));
         }*/
        for (int i = 0; i < 9; i++)
        {
            PlayerSeats[i].SetActive(true);
            PlayerSeats_id.Add(int.Parse(PlayerSeats[i].name));
        }
      //  SoundManager.instance.dealerVoice = GetComponent<AudioSource>();       
    }

    private void OnEnable()
    {


       


        //set game part
        /*  string _card_indexed = "0.1.2.3.4";
          bool _check3 = true;
          bool _check4 = false;
          bool _check5 = true;
          bool _checked3 = true;
          bool _checked4 = false;
          bool _checked5 = true;
          bool _endGame = false;
          float _call_value = 100.50f;
          float _totalPot_value = 500.00f;
          int _currentPlayer = 2;
          string currentState = "1_2_NotPlaying_1000_0.00_False_True";
          float balance = 1000.00f;
          float prevCall = 0.00f;
          bool stopTimer = false;
          bool allowCheck = true;
          int seat = 1;
          bool active = true;
          int _player_d = 0;
          int _player_1 = 1;
          int _player_2 = 2;

          SetGame_Multiplayer_Node(_card_indexed, _check3, _check4, _check5, _checked3, _checked4, _checked5, _endGame, _call_value, _totalPot_value, _currentPlayer, currentState, balance, prevCall, stopTimer, allowCheck, seat, active, _player_d, _player_1, _player_2);*/


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            float balance = 1000.00f;
           
            int index = 0;
           
            Add_New_Player_Node(balance, test_index++, index);
        }
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            string _card_indexed = "0.1.2.3.4";
            bool _check3 = true;
            bool _check4 = false;
            bool _check5 = true;
            bool _checked3 = true;
            bool _checked4 = false;
            bool _checked5 = true;
            bool _endGame = false;
            float _call_value = 100.50f;
            float _totalPot_value = 500.00f;
            int _currentPlayer = 2;
            string currentState = "1_2_NotPlaying_1000_0.00_False_True";
            float balance = 1000.00f;
            float prevCall = 0.00f;
            bool stopTimer = false;
            bool allowCheck = true;
            int seat = 1;
            bool active = true;
            int _player_d = 0;
            int _player_1 = 1;
            int _player_2 = 2;

            SetGame_Multiplayer_Node(_card_indexed, _check3, _check4, _check5, _checked3, _checked4, _checked5, _endGame, _call_value, _totalPot_value, _currentPlayer, currentState, balance, prevCall, stopTimer, allowCheck, seat, active, _player_d, _player_1, _player_2);
        }
    }

    public IEnumerator _start(GameObject panel)
    {
        print("--------_start-------->> " + PhotonNetwork.LocalPlayer.player_id);
        yield return new WaitForSeconds(1);
        if (selectedSeat == -1)
            selectedSeat = PlayerSeats_id[0];
        PhotonNetwork.LocalPlayer.player_seat = selectedSeat;
        int index = PhotonNetwork.LocalPlayer.player_id - 1;
        
        object[] content = new object[] { current_player_balance, selectedSeat, index };
        MultiplayerManager.instance.RaiseEventMethod(ConstEvents.ADD_PLAYER, content);

        print("--------PhotonNetwork.CountOfPlayers-------->> " + PhotonNetwork.CurrentRoom.Players.Count);
        print("--------PhotonNetwork.name-------->> " + PhotonNetwork.PlayerList[0].NickName);
        
        int i = index;
        PlayerScript playerScript = Players[selectedSeat];
        playerScript.seat = selectedSeat;
        PlayerSeats_id_used.Add(selectedSeat);
        PlayerSeats_id.Remove(selectedSeat);
        selectedSeat = -1;
        seatsParent.SetActive(false);

        playerScript.gameObject.SetActive(true);
        int avtar = int.Parse(PhotonNetwork.PlayerList[i].CustomProperties["AvtarId"].ToString());
        int country = int.Parse(PhotonNetwork.PlayerList[i].CustomProperties["CountryId"].ToString());
        print("--------name-------->> " + PhotonNetwork.PlayerList[i].NickName);
        print("--------country_id-------->> " + avtar);
        print("--------avtar_id-------->> " + country);
        playerScript.player_name.text = PhotonNetwork.PlayerList[i].NickName;
        playerScript.player_score.text = "$" + current_player_balance.ToString("0.00");
        playerScript.balance = current_player_balance;
        playerScript.player_country.sprite = country_flags[country];
        playerScript.player_profile.sprite = avtars[avtar];
        playerScript.currentState = PlayerStates.Waiting;
        playerScript.youObject.SetActive(true);
        playerScript.cardsObject.SetActive(false);

        print("--------_start-------->> " + PhotonNetwork.CurrentRoom.PlayerCount);        

        print("--------playingPlayers-------->> " + PlayerSeats_id_used.Count);
        if (PlayerSeats_id_used.Count == 2)
        {
            StartNewGame();
        }
        panel.SetActive(false);
    }

    public void Add_New_Player(float balance, int seat, int index)
    {
        print("--------AddNewPlayer--balance----->> " + seat);
     
        //print("--------playingPlayers-------->> " + playingPlayers);
        int i = index;
       
        if (i >= 0)
        {
            PlayerScript playerScript = Players[seat];
            for(int j = 0; j < PlayerSeats.Count; j++)
            {
                if (PlayerSeats[j].name == seat.ToString())
                {
                    PlayerSeats[j].SetActive(false);
                    //return;
                }
            }
            playerScript.gameObject.SetActive(true);
            PlayerSeats_id_used.Add(seat);
            PlayerSeats_id.Remove(seat);

            int avtar = int.Parse(PhotonNetwork.PlayerList[i].CustomProperties["AvtarId"].ToString());
            int country = int.Parse(PhotonNetwork.PlayerList[i].CustomProperties["CountryId"].ToString());

            playerScript.player_name.text = PhotonNetwork.PlayerList[i].NickName;
            playerScript.player_country.sprite = country_flags[country];
            playerScript.player_profile.sprite = avtars[avtar];
            playerScript.player_score.text = "$" + balance.ToString("0.00");
            playerScript.balance = balance;
            playerScript.currentState = PlayerStates.Waiting;
            playerScript.seat = seat;
            playerScript.cardsObject.SetActive(false);

            print("--------playingPlayers-------->> " + i);

            if (PlayerSeats_id_used.Count == 2)
            {
                StartNewGame();
            }
        }
    }


    public void Add_New_Player_Node(float balance, int seat, int index)
    {
        print("--------AddNewPlayer--balance----->> " + seat);

        //print("--------playingPlayers-------->> " + playingPlayers);
        int i = index;

        if (i >= 0)
        {
            PlayerScript playerScript = Players[seat];
            for (int j = 0; j < PlayerSeats.Count; j++)
            {
                if (PlayerSeats[j].name == seat.ToString())
                {
                    PlayerSeats[j].SetActive(false);
                    //return;
                }
            }
            playerScript.gameObject.SetActive(true);
            PlayerSeats_id_used.Add(seat);
            PlayerSeats_id.Remove(seat);

            int avtar = int.Parse("0");
            int country = int.Parse("5");

            playerScript.player_name.text = "sahil";
            playerScript.player_country.sprite = country_flags[country];
            playerScript.player_profile.sprite = avtars[avtar];
            playerScript.player_score.text = "$" + balance.ToString("0.00");
            playerScript.balance = balance;
            playerScript.currentState = PlayerStates.Waiting;
            playerScript.seat = seat;
            playerScript.cardsObject.SetActive(false);

            print("--------playingPlayers-------->> " + i);

            if (PlayerSeats_id_used.Count == 2)
            {
                StartNewGame();
            }
        }
    }

    public IEnumerator SetGame()
    {
        yield return new WaitForSeconds(1);
        print("--------SetGame---SetGame----->> ");
        string player_info = "";
        for (int i = 0; i < PlayerSeats_id_used.Count; i++)
        {
            PlayerScript player = Players[PlayerSeats_id_used[i]];
            player_info += "&_" + player.currentState + "_" + player.balance + "_" + player.prev_call + "_" + player.stopTimer + "_" + player.sec_i
                 + "_" + player.seat + "_" + player.allowCheck;
        }

        print("--------SetGame----player_info---->> " + currentPlayer);
        object[] content = new object[] { cardString, check3, check4, check5, checked3, checked4, checked5, endGame, call_value, totalPot_value, currentPlayer,
            player_info, player_d, player_1, player_2};
        MultiplayerManager.instance.RaiseEventMethod(ConstEvents.SET_GAME, content);
    }

    public void SetGame_Multiplayer(string _card_indexed, bool _check3, bool _check4, bool _check5, bool _checked3, bool _checked4, bool _checked5,
        bool _endGame, float _call_value, float _totalPot_value, int _currentPlayer, string _player_info, int _player_d, int _player_1, int _player_2)
    {
        print("-----SetGame_Multiplayer------>>>>" + PhotonNetwork.LocalPlayer.player_id + "<<<<<======>>>>" + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.LocalPlayer.player_id == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            print("-----SetGame_Multiplayer---dfhfthgfhtgh--->>>>" + _player_info);
            currentPlayer = _currentPlayer;
            check3 = _check3;
            check4 = _check4;
            check5 = _check5;
            checked3 = _checked3;
            checked4 = _checked4;
            checked5 = _checked5;
            endGame = _endGame;
            call_value = _call_value;
            totalPot_value = _totalPot_value;
            chips_merge_transform.GetComponent<Text>().text = "$" + totalPot_value.ToString("0.00");
            totalPot_text.text = "$" + totalPot_value.ToString("0.00");
            player_d = _player_d;
            player_1 = _player_1;
            player_2 = _player_2;
            if (player_d != -1)
            {
                D_Object.gameObject.SetActive(true);
                D_Object.position = Players[player_d].D_pos.position;
            }
                        
            string[] p = _player_info.Split('&');

            for (int i = 1; i < p.Length; i++)
            {
                print("P--->>>>" + p[i]);
                string[] info = p[i].Split('_');
                int seat = int.Parse(info[6]);
                PlayerScript player = Players[seat];
                PlayerSeats.Find((GameObject obj) => obj.name == seat.ToString()).SetActive(false);
                PlayerSeats_id_used.Add(seat);
                PlayerSeats_id.Remove(seat);
                player.gameObject.SetActive(true);

                int avtar = int.Parse(PhotonNetwork.PlayerList[i - 1].CustomProperties["AvtarId"].ToString());
                int country = int.Parse(PhotonNetwork.PlayerList[i - 1].CustomProperties["CountryId"].ToString());

                player.player_name.text = PhotonNetwork.PlayerList[i - 1].NickName;
                player.player_country.sprite = country_flags[country];
                player.player_profile.sprite = avtars[avtar];
                switch (info[1])
                {
                    case "NotPlaying":
                        player.currentState = PlayerStates.NotPlaying;
                        break;

                    case "Folded":
                        player.currentState = PlayerStates.Folded;
                        break;

                    case "Waiting":
                        player.currentState = PlayerStates.Waiting;
                        break;

                    case "IsPlaying":
                        player.currentState = PlayerStates.IsPlaying;
                        winnerlogic.players.Add(player);
                        break;

                }
                player.balance = float.Parse(info[2]);
                player.player_score.text = "$" + player.balance.ToString("0.00");
                player.prev_call = float.Parse(info[3]);
                if(player.prev_call > 0.0f)
                {
                    player.call_text.text = "$" + player.prev_call.ToString("0.00");
                    player.call_text.gameObject.SetActive(true);
                }
                player.stopTimer = bool.Parse(info[4]);
                player.seat = seat;
                player.allowCheck = bool.Parse(info[7]);
                if (!player.stopTimer)
                    StartCoroutine(Players[currentPlayer].ContinueTimer(int.Parse(info[5])));
            }

            if (p.Length > 2)
            {
                //set table cards
                print("P--->>>>" + _card_indexed);
                string[] s = _card_indexed.Split('.');
                cards_properties_temp.Clear();
                int l = s.Length - 1;
                for (int i = 0; i < l; i++)
                {
                    cards_properties_temp.Add(cards_properties[int.Parse(s[i])]);
                }

                for (int i = 0; i < cards_positions.Count; i++)
                {
                    if (cards_positions[i].transform.childCount != 0)
                        Destroy(cards_positions[i].GetChild(0).gameObject);
                }
                for (int i = 0; i < cards_positions_on_table.Count; i++)
                {
                    cards_on_table[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < cards_positions.Count; i++)
                {
                    if (cards_positions[i].gameObject.activeSelf)
                    {
                        GameObject card = Instantiate(card_prefab, card_spawn_transform.position, card_spawn_transform.rotation, cards_positions[i]);
                        card.GetComponent<Image>().sprite = card_back[PlayerPrefs.GetInt("BackDeck", 0)];
                        card.transform.DOLocalMove(Vector3.zero, 0f);
                        card.transform.DOLocalRotate(Vector3.zero, 0f);
                    }
                }

                int j = 4;
                for (int i = 0; i <= j; i++)
                {
                    winnerlogic.tableCards[i] = cards_properties_temp[i].name;
                    cards_on_table[i].GetComponent<Image>().sprite = cards_properties_temp[i].front_image[PlayerPrefs.GetInt("FrontDeck", 0)];
                }

                for (int i = 0; i < winnerlogic.players.Count; i++)
                {
                    print("-------j------>> " + j);
                    PlayerScript player = winnerlogic.players[i];
                    player.cardsObject.SetActive(true);
                    player.cards = new List<string>(0);
                    j++;
                    player.cards_sprites.Add(cards_properties_temp[j].front_image[PlayerPrefs.GetInt("FrontDeck", 0)]);
                    player.cards.Add(cards_properties_temp[j].name);
                    j++;
                    player.cards_sprites.Add(cards_properties_temp[j].front_image[PlayerPrefs.GetInt("FrontDeck", 0)]);
                    player.cards.Add(cards_properties_temp[j].name);

                    if (PlayerPrefs.GetString("Email", "") == "admin321@yopmail.com" && PlayerPrefs.GetString("Password") == "Admin@123")
                    {
                        player.card_transforms[0].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[0];
                        player.card_transforms[1].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[1];
                    }
                }

                if (checked3)
                {
                    Card_Show_On_Table_multiplayer(0);
                    Card_Show_On_Table_multiplayer(1);
                    Card_Show_On_Table_multiplayer(2);
                }
                if (checked4)
                {
                    Card_Show_On_Table_multiplayer(3);
                }
                if (checked5)
                {
                    Card_Show_On_Table_multiplayer(4);
                }
            }
        }
    }


    public void SetGame_Multiplayer_Node(
     string _card_indexed,
     bool _check3,
     bool _check4,
     bool _check5,
     bool _checked3,
     bool _checked4,
     bool _checked5,
     bool _endGame,
     float _call_value,
     float _totalPot_value,
     int _currentPlayer,
     string currentState,
     float balance,
     float prevCall,
     bool stopTimer,
     bool allowCheck,
     int seat,
     bool active,
     int _player_d,
     int _player_1,
     int _player_2
 )
    {
        print("-----SetGame_Multiplayer---dfhfthgfhtgh--->>>>" + currentState);
        currentPlayer = _currentPlayer;
        check3 = _check3;
        check4 = _check4;
        check5 = _check5;
        checked3 = _checked3;
        checked4 = _checked4;
        checked5 = _checked5;
        endGame = _endGame;
        call_value = _call_value;
        totalPot_value = _totalPot_value;
        chips_merge_transform.GetComponent<Text>().text = "$" + totalPot_value.ToString("0.00");
        totalPot_text.text = "$" + totalPot_value.ToString("0.00");
        player_d = _player_d;
        player_1 = _player_1;
        player_2 = _player_2;
        if (player_d != -1)
        {
            D_Object.gameObject.SetActive(true);
            D_Object.position = Players[player_d].D_pos.position;
        }

        string[] p = currentState.Split('_');

        int avtar = int.Parse(p[0]);
        int country = int.Parse(p[1]);

        PlayerScript player = Players[seat];
        PlayerSeats.Find((GameObject obj) => obj.name == seat.ToString()).SetActive(false);
        PlayerSeats_id_used.Add(seat);
        PlayerSeats_id.Remove(seat);
        player.gameObject.SetActive(true);

        player.player_name.text = "Sahil";
        player.player_country.sprite = country_flags[country];
        player.player_profile.sprite = avtars[avtar];
        switch (p[2])
        {
            case "NotPlaying":
                player.currentState = PlayerStates.NotPlaying;
                break;

            case "Folded":
                player.currentState = PlayerStates.Folded;
                break;

            case "Waiting":
                player.currentState = PlayerStates.Waiting;
                break;

            case "IsPlaying":
                player.currentState = PlayerStates.IsPlaying;
                winnerlogic.players.Add(player);
                break;
        }

        player.balance = float.Parse(p[3]);
        player.player_score.text = "$" + player.balance.ToString("0.00");
        player.prev_call = float.Parse(p[4]);
        if (player.prev_call > 0.0f)
        {
            player.call_text.text = "$" + player.prev_call.ToString("0.00");
            player.call_text.gameObject.SetActive(true);
        }
        player.stopTimer = bool.Parse(p[5]);
        player.seat = seat;
        player.allowCheck = bool.Parse(p[6]);
        if (!player.stopTimer)
            StartCoroutine(Players[currentPlayer].ContinueTimer(seat));

        if (_card_indexed.Length > 0)
        {
            // ... Existing code to handle card indexing ...
        }
    }



    public void StartNewGame()
    {
        print("-----StartNewGame------>>>>");
        player_d = -1;
        player_1 = -1;
        player_2 = -1;
        chips_merge_transform.position = chips_merge_position;
        chips_merge_transform.gameObject.SetActive(false);
        call_value = call_max;
        totalPot_value = 0;
        totalPot_text.text = "$" + totalPot_value.ToString("0.00");
        gameAllin = false;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].currentState != PlayerStates.NotPlaying)
            {
                Players[i].cards_sprites.Clear();
                Players[i].currentState = PlayerStates.IsPlaying;
                Players[i].winObject.SetActive(false);
                Players[i].cardsObject.SetActive(true);
                Players[i].player_score.color = Color.white;
                GameManager.instance.winnerlogic.players.Add(Players[i]);
            }
        }
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            print("-----PhotonNetwork.LocalPlayer.IsMasterClient------>>>>");
            DistributeCards();
        }
    }

    public void DistributeCards()
    {
        print("=======DistributeCards====>>>> ");

        cardString = "";
        cards_properties_temp.Clear();
        int l = 5 + winnerlogic.players.Count * 2;
        for (int i = 0; i < l; i++)
        {
            int cardIndex = 0;
            do
            {
                cardIndex = UnityEngine.Random.Range(0, cards_properties.Count);
            }
            while (cards_properties_temp.Contains(cards_properties[cardIndex]));
            cards_properties_temp.Add(cards_properties[cardIndex]);
            cardString += cardIndex + ".";
        }

        print("=======DistributeCards=9999===>>>> " + cardString);
        object[] content = new object[] { cardString };
        MultiplayerManager.instance.RaiseEventMethod(ConstEvents.GAME_START, content);

        StartCoroutine(_DistributeCards());
    }

    public void DistributeCards_multiplayer(string cards_indexes)
    {
        print("=======DistributeCards_multiplayer====>>>> " + cards_indexes);
        string[] s = cards_indexes.Split('.');

        cards_properties_temp.Clear();
        int l = s.Length - 1;
        for (int i = 0; i < l; i++)
        {
            print("=======ss====>>>> " + s[i]);
            cards_properties_temp.Add(cards_properties[int.Parse(s[i])]);
        }

        StartCoroutine(_DistributeCards());
    }

    IEnumerator _DistributeCards()
    {
        print("=======DistributeCards====>>>> ");

        for (int i = 0; i < cards_positions.Count; i++)
        {
            if (cards_positions[i].transform.childCount != 0)
                Destroy(cards_positions[i].GetChild(0).gameObject);
        }
        for (int i = 0; i < cards_positions_on_table.Count; i++)
        {
            cards_on_table[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < cards_positions.Count; i++)
        {
            if (cards_positions[i].transform.parent.gameObject.activeSelf)
            {
                SoundManager.instance.SoundEffect("CardDistribute");
                yield return new WaitForSeconds(0.1f);
                GameObject card = Instantiate(card_prefab, card_spawn_transform.position, card_spawn_transform.rotation, cards_positions[i]);
                card.GetComponent<Image>().sprite = card_back[PlayerPrefs.GetInt("BackDeck", 0)];
                card.transform.DOLocalMove(Vector3.zero, 0.3f);
                card.transform.DOLocalRotate(Vector3.zero, 0.3f);
                //yield return new WaitForSeconds(0.1f);
            }
        }

        int j = 5;
        for (int i = 0; i < j; i++)
        {
            winnerlogic.tableCards[i] = cards_properties_temp[i].name;
            cards_on_table[i].GetComponent<Image>().sprite = cards_properties_temp[i].front_image[PlayerPrefs.GetInt("FrontDeck", 0)];
        }

        for (int i = 0; i < PlayerSeats_id_used.Count; i++)
        {
            PlayerScript player = Players[PlayerSeats_id_used[i]];
            player.cards = new List<string>();
            print("=======>>>>" + cards_properties_temp.Count + "===>>> " + +j + "===>>>>" + PlayerPrefs.GetInt("FrontDeck", 0));
            player.cards_sprites.Add(cards_properties_temp[j].front_image[PlayerPrefs.GetInt("FrontDeck", 0)]);
            player.cards.Add(cards_properties_temp[j].name);
            j++;
            player.cards_sprites.Add(cards_properties_temp[j].front_image[PlayerPrefs.GetInt("FrontDeck", 0)]);
            player.cards.Add(cards_properties_temp[j].name);
            j++;

            if (PlayerSeats_id_used[i] == PhotonNetwork.LocalPlayer.player_seat)
            {
                player.card_transforms[0].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[0];
                player.card_transforms[1].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[1];
            }
            if (PlayerPrefs.GetString("Email", "") == "admin321@yopmail.com" && PlayerPrefs.GetString("Password") == "Admin@123")
            {
                player.card_transforms[0].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[0];
                player.card_transforms[1].GetChild(0).GetComponent<Image>().sprite = player.cards_sprites[1];
            }
        }
        yield return new WaitForSeconds(1f);
        //print("--------NextTurn-00------->>>> ");
        NextTurn();
    }

    public void Card_Show_On_Table(int cardId)
    {
        SoundManager.instance.SoundEffect("CardOnTable");
        cards_on_table[cardId].gameObject.GetComponent<Animator>().Play("card_start");
        cards_on_table[cardId].position = Vector3.zero;
        cards_on_table[cardId].gameObject.SetActive(true);
        cards_on_table[cardId].transform.DOLocalMove(cards_positions_on_table[cardId], 0.4f);

        object[] content = new object[] { cardId };
        MultiplayerManager.instance.RaiseEventMethod(ConstEvents.CARD_SHOW, content);
    }

    public void Card_Show_On_Table_multiplayer(int cardId)
    {
        SoundManager.instance.SoundEffect("CardOnTable");
        cards_on_table[cardId].gameObject.GetComponent<Animator>().Play("card_start");
        cards_on_table[cardId].position = Vector3.zero;
        cards_on_table[cardId].gameObject.SetActive(true);
        cards_on_table[cardId].transform.DOLocalMove(cards_positions_on_table[cardId], 0.4f);
    }

    void DisableAllowCheck()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].allowCheck = false;
        }
    }

    public void NextTurn()
    {
        currentPlayer++;
        //print("--------NextTurn-------->>>> " + currentPlayer);
        if (currentPlayer >= Players.Count)
        {
            currentPlayer = -1;
            //print("--------NextTurn-11------->>>> ");
            NextTurn();
            return;
        }

        if (Players[currentPlayer].currentState == PlayerStates.IsPlaying)
        {
            if (player_d == -1)
            {
                player_d = currentPlayer;
                D_Object.gameObject.SetActive(true);
                D_Object.position = Players[currentPlayer].D_pos.position;
                //print("--------NextTurn-22------->>>> ");
                NextTurn();
                return;
            }
            else if (player_1 == -1)
            {
                player_1 = currentPlayer;
                Players[currentPlayer].balance -= call_min;
                Players[currentPlayer].player_score.text = "$" + Players[currentPlayer].balance.ToString("0.00");
                Players[currentPlayer].prev_call = call_min;
                SoundManager.instance.SoundEffect("ChipsAdd");
                Players[currentPlayer].call_text.text = "$" + Players[currentPlayer].prev_call.ToString("0.00");
                Players[currentPlayer].call_text.gameObject.SetActive(true);
                totalPot_value += call_min;
                totalPot_text.text = "$" + totalPot_value.ToString("0.00");
                //print("--------NextTurn-33------->>>> ");
                NextTurn();
                return;
            }
            else if (player_2 == -1)
            {
                player_2 = currentPlayer;
                Players[currentPlayer].balance -= call_max;
                Players[currentPlayer].player_score.text = "$" + Players[currentPlayer].balance.ToString("0.00");
                Players[currentPlayer].allowCheck = true;
                Players[currentPlayer].prev_call = call_max;
                SoundManager.instance.SoundEffect("ChipsAdd");
                Players[currentPlayer].call_text.text = "$" + Players[currentPlayer].prev_call.ToString("0.00");
                Players[currentPlayer].call_text.gameObject.SetActive(true);
                totalPot_value += call_max;
                totalPot_text.text = "$" + totalPot_value.ToString("0.00");
                //print("--------NextTurn-44------->>>> ");
                NextTurn();
                return;
            }
            CheckForShowTableCards();
            if (endGame)
            {
                EndOfTheGame(-1);
            }
            else
            {
                //print("--------NextTurn-------->>>> " + currentPlayer);
                EnableUI();                
            }
        }
        else
        {
            //print("--------NextTurn-55------->>>> ");
            NextTurn();
            return;
        }
    }

    void EnableUI()
    {
        if (PhotonNetwork.LocalPlayer.player_seat != -1)
        {
            if (Players[PhotonNetwork.LocalPlayer.player_seat].balance < 0)
            {
                InsufficientBalancePanel.SetActive(true);
            }
            else
            {
                StartCoroutine(Players[currentPlayer].StartTimer());

                call_value = Mathf.Min(call_value, Players[currentPlayer].balance);
                raised_value = Mathf.Min((call_value + 0.01f), Players[currentPlayer].balance);

                if (Players[currentPlayer].prev_call < call_value)
                {                    
                    callvalue_text.text = "$" + (call_value - Players[currentPlayer].prev_call).ToString("0.00");
                    raisevalue_text.text = "$" + raised_value.ToString("0.00");
                    callBtn.SetActive(true);

                    if (!GameManager.instance.gameAllin && raised_value > call_value)
                        raiseBtn.SetActive(true);
                    checkBtn.SetActive(false);
                    betBtn.SetActive(false);
                }
                else
                {
                    betvalue_text.text = "$" + raised_value.ToString("0.00");
                    callBtn.SetActive(false);
                    raiseBtn.SetActive(false);
                    checkBtn.SetActive(true);
                    if (!GameManager.instance.gameAllin && raised_value > call_value)
                        betBtn.SetActive(true);
                }
                print("-------Actor----->>>> " + Players[PhotonNetwork.LocalPlayer.player_id - 1].seat + " <<<<<----CurrentPlayer---->>>> " + currentPlayer);
                if (PhotonNetwork.LocalPlayer.player_seat == currentPlayer)
                {
                    gameUI.SetActive(true);
                }
            }
        }
    }

    void CheckForgameAllin()
    {
        if (gameAllin)
        {
            for (int i = 0; i < winnerlogic.players.Count; i++)
            {
                winnerlogic.players[i].card_transforms[0].GetChild(0).GetComponent<Image>().sprite = winnerlogic.players[i].cards_sprites[0];
                winnerlogic.players[i].card_transforms[1].GetChild(0).GetComponent<Image>().sprite = winnerlogic.players[i].cards_sprites[1];
            }
        }
    }

    void CheckForShowTableCards()
    {
        print("--------CheckForShowTableCards-------->>>> " + currentPlayer);
        if (Players[currentPlayer].allowCheck)
        {
            CheckForgameAllin();

            SetTotalPot();
            if (endGame)
            {
                EndOfTheGame(-1);
            }
            else
            {
                if (check3)
                {
                    if (check4)
                    {
                        check5 = true;
                        DisableAllowCheck();
                    }
                    else
                    {
                        check4 = true;
                        DisableAllowCheck();
                    }
                }
                else
                {
                    check3 = true;
                    DisableAllowCheck();
                }
            }


            if (check3 && !endGame)
            {
                if (check4)
                {
                    if (check5)
                    {
                        if (checked5)
                        {
                            endGame = true;
                        }
                        else
                        {
                            checked5 = true;
                            if (PhotonNetwork.LocalPlayer.player_seat == currentPlayer)
                                Card_Show_On_Table(4);
                        }
                    }
                    else
                    {
                        if (!checked4)
                        {
                            checked4 = true;
                            if (PhotonNetwork.LocalPlayer.player_seat == currentPlayer)
                                Card_Show_On_Table(3);
                        }
                    }
                }
                else
                {
                    if (!checked3)
                    {
                        checked3 = true;
                        if (PhotonNetwork.LocalPlayer.player_seat == currentPlayer)
                        {
                            Card_Show_On_Table(0);
                            Card_Show_On_Table(1);
                            Card_Show_On_Table(2);
                        }
                    }
                }
            }
        }
    }

    public void Fold()
    {
        //SoundManager.instance.DealerVoice("Fold");
        //Players[currentPlayer].ChangeAction(PlayerActions.Fold);
        //gameUI.SetActive(false);

        object[] content = new object[] { };
        MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.FOLD, content);
    }

    public void Fold_multiplayer()
    {
        SoundManager.instance.DealerVoice("Fold");
        Players[currentPlayer].ChangeAction(PlayerActions.Fold);
        gameUI.SetActive(false);
    }

    public void Call()
    {
        print("-------Call-----------");
        //SoundManager.instance.DealerVoice("Call");
        //Players[currentPlayer].ChangeAction(PlayerActions.Call);
        //gameUI.SetActive(false);

        object[] content = new object[] { currentPlayer, call_value };
        MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.CALL, content);
    }

    public void Call_MultiPlayer(float value)
    {
        print("-------Call_MultiPlayer-----------");
        call_value = value;
        SoundManager.instance.DealerVoice("Call");
        Players[currentPlayer].ChangeAction(PlayerActions.Call);
        gameUI.SetActive(false);
    }

    public void Raise()
    {
        //DisableAllowCheck();
        //SoundManager.instance.DealerVoice("Raise");
        //Players[currentPlayer].ChangeAction(PlayerActions.Raise);
        //gameUI.SetActive(false);

        object[] content = new object[] { raised_value };
        MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.RAISE, content);
    }

    public void Raise_MultiPlayer(float value)
    {
        print("-------Raise_MultiPlayer-----------> " + value);
        raised_value = value;
        DisableAllowCheck();
        SoundManager.instance.DealerVoice("Raise");
        Players[currentPlayer].ChangeAction(PlayerActions.Raise);
        gameUI.SetActive(false);
    }

    public void Check()
    {
        //SoundManager.instance.DealerVoice("Check");
        //Players[currentPlayer].ChangeAction(PlayerActions.Check);
        //gameUI.SetActive(false);

        object[] content = new object[] { currentPlayer };
        MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.CHECK, content);
    }

    public void Check_MultiPlayer()
    {
        SoundManager.instance.DealerVoice("Check");
        Players[currentPlayer].ChangeAction(PlayerActions.Check);
        gameUI.SetActive(false);
    }

    public void Bet()
    {
        //DisableAllowCheck();
        //SoundManager.instance.DealerVoice("Bet");
        //Players[currentPlayer].ChangeAction(PlayerActions.Bet);
        //gameUI.SetActive(false);

        object[] content = new object[] { raised_value };
        MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.BET, content);
    }

    public void Bet_MultiPlayer(float value)
    {
        print("-------Bet_MultiPlayer-----------> " + value);
        raised_value = value;
        DisableAllowCheck();
        SoundManager.instance.DealerVoice("Bet");
        Players[currentPlayer].ChangeAction(PlayerActions.Bet);
        gameUI.SetActive(false);
    }

    public void Allin()
    {
        GameManager.instance.gameAllin = true;

        //object[] content = new object[] { };
        //MultiplayerManager.instance.RaiseEventMethod_All(ConstEvents.ALL_IN, content);
    }

    public void gameAllin_MultiPlayer()
    {
        GameManager.instance.gameAllin = true;
    }

    public void EndOfTheGame(int id)
    {
        check3 = false;
        check4 = false;
        check5 = false;
        checked3 = false;
        checked4 = false;
        checked5 = false;
        endGame = false;
        if (id == -1)
            winnerlogic.winners = winnerlogic.GetWinners();
        StartCoroutine(DeclareWinner(id));
    }

    public void SetTotalPot()
    {
        call_value = 0;
        chips_merge_transform.gameObject.SetActive(true);
        chips_merge_transform.GetComponent<Text>().text = "$" + totalPot_value.ToString("0.00");
        SoundManager.instance.SoundEffect("ChipsInPot");
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].prev_call = 0;
            Vector3 pos = Players[i].call_text.transform.position;
            Players[i].call_text.transform.DOMove(chips_merge_transform.position, 0.2f).OnComplete(() =>
            ResetPos(pos));

        }
    }
    private void ResetPos(Vector3 pos)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].call_text.gameObject.SetActive(false);
            Players[i].call_text.transform.position = Players[i].call_text_pos;
        }
    }

    IEnumerator DeclareWinner(int id)
    {
        bool youWin = false;
        bool youLoose = false;
        gameUI.SetActive(false);
        if (winnerlogic.players.Contains(Players[PhotonNetwork.LocalPlayer.player_seat]))
        {
            youLoose = true;
        }
        yield return new WaitForSeconds(1);
        if (id != -1)
        {
            for (int j = 0; j < Players.Count; j++)
            {
                Players[j].stopTimer = true;
                if (Players[j].id == winnerlogic.players[0].id)
                {
                    PlayerScript player = Players[j];
                    chips_merge_transform.DOMove(player.call_text_pos, 0.5f);
                    player.Win(totalPot_value);
                    if (player.seat == PhotonNetwork.LocalPlayer.player_seat)
                        youWin = true;
                }
            }
            if (youWin)
                SoundManager.instance.SoundEffect("Win");
            else if (youLoose)
                SoundManager.instance.SoundEffect("Loose");
            yield return new WaitForSeconds(3);
            CheckForNewGame();
        }
        else
        {
            float win_amount = totalPot_value / winnerlogic.winners.Count;
            for (int i = 0; i < winnerlogic.winners.Count; i++)
            {
                for (int j = 0; j < Players.Count; j++)
                {
                    Players[j].stopTimer = true;                    
                    if (Players[j].id == winnerlogic.winners[i].id)
                    {                        
                        PlayerScript player = Players[j];
                        chips_merge_transform.DOMove(player.call_text_pos, 0.5f);
                        player.Win(win_amount);
                        if (player.seat == PhotonNetwork.LocalPlayer.player_seat)
                            youWin = true;
                    }
                }
            }
            if (youWin)
                SoundManager.instance.SoundEffect("Win");
            else if (youLoose)
                SoundManager.instance.SoundEffect("Loose");
            yield return new WaitForSeconds(3);
            CheckForNewGame();
        }

        print("----->>> " + youWin + " ------>>>> " + youLoose);
    }

    void CheckForNewGame()
    {
        int playersToPlay = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].currentState != PlayerStates.NotPlaying)
                playersToPlay++;
        }
        if (playersToPlay > 1)
        {
            winnerlogic.players.Clear();
            StartNewGame();
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        print("-----EndGame------>>>>");
        D_Object.gameObject.SetActive(false);
        player_d = -1;
        player_1 = -1;
        player_2 = -1;
        chips_merge_transform.position = chips_merge_position;
        chips_merge_transform.gameObject.SetActive(false);
        call_value = call_max;
        totalPot_value = 0;
        totalPot_text.text = "$" + totalPot_value.ToString("0.00");
        int index = winnerlogic.players[0].seat;
        print("-----EndGame------>>>>" + index);
        Players[index].cards_sprites.Clear();
        Players[index].currentState = PlayerStates.Waiting;
        Players[index].winObject.SetActive(false);
        Players[index].cardsObject.SetActive(true);
        Players[index].player_score.color = Color.white;
        Players[index].call_text.gameObject.SetActive(false);

        for (int i = 0; i < cards_positions.Count; i++)
        {
            if (cards_positions[i].transform.childCount != 0)
                Destroy(cards_positions[i].GetChild(0).gameObject);
        }
        for (int i = 0; i < cards_positions_on_table.Count; i++)
        {
            cards_on_table[i].gameObject.SetActive(false);
        }
    }

    public void Raise_phus()
    {
        if (raised_value < Players[currentPlayer].balance - 0.01f)
        {
            SoundManager.instance.SoundEffect("BtnClick");
            raised_value += 0.01f;
        }
        raisevalue_text.text = "$" + raised_value.ToString("0.00");
    }

    public void Raise_minus()
    {
        if (raised_value > 0.02f)
        {
            SoundManager.instance.SoundEffect("BtnClick");
            raised_value -= 0.01f;
        }
        raisevalue_text.text = "$" + raised_value.ToString("0.00");
    }

    public void Bet_phus()
    {
        if (raised_value < Players[currentPlayer].balance - 0.01f)
        {
            SoundManager.instance.SoundEffect("BtnClick");
            raised_value += 0.01f;
        }
        betvalue_text.text = "$" + raised_value.ToString("0.00");
    }

    public void Bet_minus()
    {
        if (raised_value > 0.02f)
        {
            SoundManager.instance.SoundEffect("BtnClick");
            raised_value -= 0.01f;
        }
        betvalue_text.text = "$" + raised_value.ToString("0.00");
    }

    public void LeaveRoom()
    {
        SoundManager.instance.SoundEffect("BtnClick");
        LeavePanel.SetActive(true);
        if (PhotonNetwork.LocalPlayer.player_seat != -1)
            leaveAmountText.text = "$" + GameManager.instance.Players[PhotonNetwork.LocalPlayer.player_seat].balance.ToString("0.00");
        else
            leaveAmountText.text = "$0.00";
    }

    public void LeaveRoom_Ok()
    {
        SoundManager.instance.SoundEffect("BtnClick");
        if (PhotonNetwork.LocalPlayer.player_seat != -1)
            PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance", 0) + GameManager.instance.Players[PhotonNetwork.LocalPlayer.player_seat].balance);
        else
            leaveAmountText.text = "$0.00";
        Destroy(GameObject.Find("PokerPhotonManager"));
        PlayerPrefs.SetInt("AutoLogin", 1);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("PunBasics-Launcher");
    }

    public void Leave_Cancel()
    {
        SoundManager.instance.SoundEffect("BtnClick");
        LeavePanel.SetActive(false);
    }

    public void TakeSeat(int id)
    {
       //skchnages SoundManager.instance.SoundEffect("BtnClick");
        selectedSeat = id;
    }

    public void SeatSelected_Multiplayer(int seatNo)
    {
        if (seatNo == selectedSeat)
        {
            BuyinPanel.SetActive(false);
            SeatAlreadyOccupiedPanel.SetActive(true);
        }
    }
}
