using System;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoomItemUI : MonoBehaviour
{
    public PokerRoomInfo pokerRoomInfo;
    [SerializeField] private Text m_RoomNameText;
    [SerializeField] private Text m_PlayersMaxCount;
    [SerializeField] private Button m_CreateRoomButton;
    [SerializeField] private GameObject noSpaceText;

    public void Set(PokerRoomInfo roomInfo, Action OnClick)
    {
        string[] s = roomInfo.name.Split('.');
        m_RoomNameText.text = s[0];
        m_PlayersMaxCount.text = roomInfo.connectedPlayers + "/" + roomInfo.maxPlayers;
        //m_CreateRoomButton.interactable = roomInfo.isOpen;
       
        if(roomInfo.connectedPlayers < roomInfo.maxPlayers)
        {
            m_CreateRoomButton.onClick.RemoveAllListeners();
            m_CreateRoomButton.onClick.AddListener(() =>
            {
                OnClick?.Invoke();
            });
        }
    }

    public void _Set(PokerRoomInfo roomInfo, Action OnClick)
    {
        if (roomInfo.connectedPlayers < roomInfo.maxPlayers)
        {
            m_CreateRoomButton.gameObject.SetActive(true);
            noSpaceText.gameObject.SetActive(false);
        }
        else
        {
            m_CreateRoomButton.gameObject.SetActive(false);
            noSpaceText.gameObject.SetActive(true);
        }

        m_CreateRoomButton.onClick.RemoveAllListeners();
        m_CreateRoomButton.onClick.AddListener(() =>
        {
            OnClick?.Invoke();
        });
    }
}
