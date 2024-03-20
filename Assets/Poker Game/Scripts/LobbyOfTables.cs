using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class RoomDataIds
{
    public string data;
}

public class LobbyOfTables : MonoBehaviour
{
    public string lobbyName;
    public List<PokerRoomInfo> _rooms = new List<PokerRoomInfo>();
    public float min_value;
    public float max_value;
    public float buyIn_min_value;
    public float buyIn_max_value;
    

    public Text totalPlayerText;
    public Text totalTableText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        SetValues();
    }
    #region Unity_Callbacks

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
       
    }

    #endregion

    #region Private_Methods
    private void GetRoomInfo()
    {

        ServerManager.instance.OnGetRooms();
    }
    #endregion

    #region Private_Methods
   

    /*  Debug.Log("The room callback recived");
       RoomDataIds roomData = JsonUtility.FromJson<RoomDataIds>(jsondata);

        // Print the room IDs
        foreach (int roomId in roomData.data)
        {
            m_roomIds.Add(roomId);
        }*/
    #endregion



    // Update is called once per frame
    public void SetValues()
    {
        totalTableText.text = _rooms.Count.ToString();
        int players = 0;
        for (int i = 0; i < _rooms.Count; i++)
        {
            players += _rooms[i].connectedPlayers;
        }
        totalPlayerText.text = players.ToString();
    }
}
