using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using System;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager instance;

    public static Action<string> myCustomEvents;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        print("-----eventCode---OnEnable--->>>>");
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
      
        // myCustomEvents += OnEventReceived;
    }

    private void OnDisable()
    {
        print("-----eventCode---OnDisable--->>>>");
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    public void RaiseEventMethod_All(byte Event_Code, object[] content)
    {
        PhotonNetwork.RaiseEvent(Event_Code, content, new RaiseEventOptions() { Receivers = ReceiverGroup.All}, SendOptions.SendUnreliable);
    }

    public void RaiseEventMethod(byte Event_Code, object[] content)
    {
        PhotonNetwork.RaiseEvent(Event_Code, content, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void OnEventReceived(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        print("-----eventCode------>>>>" + eventCode + "--------->>>>> " + photonEvent.Code);
        switch(photonEvent.Code)
        {
            case ConstEvents.CALL:
                {
                    print("-----CALL------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    int playerid = (int)data[0];
                    float callValue = (float)data[1];
                    GameManager.instance.Call_MultiPlayer(callValue);
                    break;
                }

            case ConstEvents.RAISE:
                {
                    print("-----RAISE------>>>>");
                    object[] data = (object[])photonEvent.CustomData;                    
                    float value = (float)data[0];
                    GameManager.instance.Raise_MultiPlayer(value);
                    break;
                }

            case ConstEvents.BET:
                {
                    print("-----BET------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    float value = (float)data[0];
                    GameManager.instance.Bet_MultiPlayer(value);
                    break;
                }

            case ConstEvents.CHECK:
                {
                    print("-----CHECK------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    GameManager.instance.Check_MultiPlayer();
                    break;
                }

            case ConstEvents.FOLD:
                {
                    print("-----FOLD------>>>>");
                    GameManager.instance.Fold_multiplayer();
                    break;
                }

            case ConstEvents.GAME_START:
                {
                    print("-----GAME_START------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    string s = data[0].ToString();
                    GameManager.instance.DistributeCards_multiplayer(s);
                    break;
                }

            case ConstEvents.CARD_SHOW:
                {
                    print("-----CARD_SHOW------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    int id = (int)data[0];
                    //int index = (int)data[1];
                    GameManager.instance.Card_Show_On_Table_multiplayer(id);
                    break;
                }

            case ConstEvents.SET_GAME:
                {
                    print("-----SET_GAME------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    string s = data[0].ToString();
                    bool check3 = (bool)data[1];
                    bool check4 = (bool)data[2];
                    bool check5 = (bool)data[3];
                    bool checked3 = (bool)data[4];
                    bool checked4 = (bool)data[5];
                    bool checked5 = (bool)data[6];
                    bool endGame = (bool)data[7];
                    float call_value = (float)data[8];
                    float totalPot_value = (float)data[9];
                    int currentPlayer = (int)data[10];
                    string player_info = (string)data[11];
                    int player_d = (int)data[12];
                    int player_1 = (int)data[13];
                    int player_2 = (int)data[14];
                    GameManager.instance.SetGame_Multiplayer(s, check3, check4, check5, checked3, checked4, checked5, endGame, call_value, totalPot_value,
                        currentPlayer, player_info, player_d, player_1, player_2);
                    break;
                }

            case ConstEvents.ADD_PLAYER:
                {
                    print("-----ADD_PLAYER------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    float balance = (float)data[0];
                    int seat = (int)data[1];
                    int index = (int)data[2];
                    GameManager.instance.Add_New_Player(balance, seat, index);
                    break;
                }

            case ConstEvents.ALL_IN:
                {
                    print("-----ALL_IN------>>>>");
                    GameManager.instance.gameAllin_MultiPlayer();
                    break;
                }

            case ConstEvents.SELECT_SEAT:
                {
                    print("-----SELECT_SEAT------>>>>");
                    object[] data = (object[])photonEvent.CustomData;
                    int seat = (int)data[0];
                    GameManager.instance.SeatSelected_Multiplayer(seat);
                    break;
                }
        }
    }


}
