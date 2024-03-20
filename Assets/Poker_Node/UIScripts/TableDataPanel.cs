using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDataPanel : MonoBehaviour
{
    #region Private_Vars
    [SerializeField]
    private List<string> m_roomIds;
    #endregion


    #region Unity_Callbacks
    private void OnEnable()
    {
        InvokeRepeating("CallOnGetRooms", 0f, 5f);
        ServerManager.s_OnRoomResultRecived += OnRoomDataRecived;
    }

    private void OnDisable()
    {
        CancelInvoke("CallOnGetRooms");
        ServerManager.s_OnRoomResultRecived -= OnRoomDataRecived;
    }

    #endregion

    #region Private_Methods
    private void CallOnGetRooms()
    {
        Debug.Log("The room socket is called");
        ServerManager.instance.OnGetRooms();
    }
    private void OnRoomDataRecived(string jsondata)
    {
        m_roomIds.Clear();
        Debug.Log("The room callback recived");

        RoomDataIds roomData = JsonUtility.FromJson<RoomDataIds>(jsondata);
        string dataString = roomData.data;

        // Split the data string by commas
        string[] idStrings = dataString.Split(',');

        // Iterate over each ID string and add it to the list
        foreach (string idString in idStrings)
        {
            string roomID = idString.Trim();
            m_roomIds.Add(roomID);
            if(roomID != null)
            {
                ServerManager.instance.OnPlayerInfo(roomID);
            }
        }
    }
    #endregion
}
