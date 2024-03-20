using System;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using UnityEngine.UI;
using BestHTTP.Examples.Helpers;
using BestHTTP.JSON.LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BestHTTP.SocketIO.Events;

public class ServerManager : MonoBehaviour
{
    #region Public_Vars
    public static ServerManager instance;
    public static Action<string> s_OnLoginResultRecived;
    public static Action<string> s_OnUpdateProfileRecived;
    public static Action<string> s_OnRegistrationResultRecived;
    public static Action<string> s_OnVerificationMailResultRecived;
    public static Action<string> s_OnRoomResultRecived;
    #endregion
    #region Private_Vars
    private SocketManager socketManager;
    [SerializeField]
    private string serverUrl;
    #endregion  

    #region Unity_Callbacks
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeSocket();
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    #endregion

    #region Private_methods
    private void InitializeSocket()
    {
        Debug.Log("The server is connected");
        socketManager = new SocketManager(new Uri(serverUrl));
        socketManager.Socket.On(SocketIOEventTypes.Connect, (s, p, a) =>
        {
            Debug.Log("The server is connected");
        });

        socketManager.Socket.On(SocketIOEventTypes.Disconnect, (s, p, a) =>
        {
            Debug.Log("The server is disconnected");

        });
    }
    private void SubscribeEvents()
    {
        socketManager.Socket.On(PokerConstants.VALID_NAME_RESULT, OnGetValidName);
        socketManager.Socket.On(PokerConstants.LOGIN_RESULT, OnGetLoginResult);
        socketManager.Socket.On(PokerConstants.REGISTER_RESULT, OnGetRegisterResult);
        socketManager.Socket.On(PokerConstants.USER_RESPONSE, OnUserInformationResult);
        socketManager.Socket.On(PokerConstants.VERIFY_RESPONSE, OnUserVerificationResult);
        socketManager.Socket.On(PokerConstants.GET_ROOMS, OnRoomListingRecieved);
        socketManager.Socket.On(PokerConstants.WAITING_PLAYERS, OnWaitingPlayerRecieved);
    }
    private void UnSubscribeEvents()
    {
        socketManager.Socket.Off(PokerConstants.VALID_NAME_RESULT, OnGetValidName);
        socketManager.Socket.Off(PokerConstants.LOGIN_RESULT, OnGetLoginResult);
        socketManager.Socket.Off(PokerConstants.REGISTER_RESULT, OnGetRegisterResult);
        socketManager.Socket.Off(PokerConstants.USER_RESPONSE, OnUserInformationResult);
        socketManager.Socket.Off(PokerConstants.VERIFY_RESPONSE, OnUserVerificationResult);
        socketManager.Socket.Off(PokerConstants.GET_ROOMS, OnRoomListingRecieved);
        socketManager.Socket.Off(PokerConstants.WAITING_PLAYERS, OnWaitingPlayerRecieved);
    }

    private void OnGetValidName(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            Debug.Log("Raw JSON response from server: " + responseJson);
        }
    }

    private void OnUserInformationResult(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            Debug.Log("On User response from server: " + responseJson);
            s_OnUpdateProfileRecived?.Invoke(responseJson);
            //  s_OnRegistrationResultRecived?.Invoke(responseJson);
        }
    }

    

    

    private void OnUserVerificationResult(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {

            string jsonResponse = JsonConvert.SerializeObject(args);
         
            s_OnVerificationMailResultRecived?.Invoke(jsonResponse);
            // Print the JSON response
            Debug.Log("On User verification from server: "+jsonResponse);

         
        }
    }

    private void OnRoomListingRecieved(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            Debug.Log("The room listing is: " + responseJson);
            s_OnRoomResultRecived?.Invoke(responseJson);
            //s_OnRegistrationResultRecived?.Invoke(responseJson);
        }
    }
    private void OnWaitingPlayerRecieved(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            Debug.Log("The waiting players listing is: " + responseJson);
          //  s_OnRoomResultRecived?.Invoke(responseJson);
            //s_OnRegistrationResultRecived?.Invoke(responseJson);
        }
    }

    private void OnGetRegisterResult(Socket socket, Packet packet, object[] args)
    {
        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            Debug.Log("On Registration response from server: " + responseJson);
            s_OnRegistrationResultRecived?.Invoke(responseJson);
        }
    }
    private void OnGetLoginResult(Socket socket, Packet packet, object[] args)
    {

        if (args != null && args.Length > 0)
        {
            Dictionary<string, object> responseData = args[0] as Dictionary<string, object>;
            string responseJson = JsonConvert.SerializeObject(responseData);
            s_OnLoginResultRecived?.Invoke(responseJson);
            Debug.Log("The login callback recievced" + responseJson);
        }
    }

    #endregion

    #region Public_Methods
    public void OnRename()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("name", "milan");
        socketManager.Socket.Emit(PokerConstants.VALID_NAME_REQUEST, JsonMapper.ToJson(data));

    }

    public void GetUserData()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data.Add("userid", UserInfo.instance._userId.ToString());
        Debug.Log("The Request is: " + JsonMapper.ToJson(data));
        socketManager.Socket.Emit(PokerConstants.USER_REQUEST, JsonMapper.ToJson(data));

    }
    [Serializable]
    public class UserVerification
    {
        public int userid;
        public string code;
    }

    public void OnUserVerification(int userid, string code)
    {
        UserVerification user = new UserVerification();
        user.userid = userid;
        user.code = code;
        string targetJson = JsonUtility.ToJson(user);
        Debug.Log("The verification request: " + targetJson);
        socketManager.Socket.Emit(PokerConstants.VERIFY_REQUEST, targetJson);
    }
    //username, userid,balance, roomid, newtable(Value: True), seatlimit, min_buyin, max_buyin, bigblind, avatar, photoUrl, photoType, mode(value: 'cash')
    public void OnCreateRoom(int maxPlayers)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("username", UserInfo.instance.PlayerCurrentData.data.username);
        data.Add("userid", Convert.ToInt32(UserInfo.instance.PlayerCurrentData.data.userid));
        data.Add("balance", UserInfo.instance.PlayerCurrentData.data.points);
        data.Add("roomid", "");
        data.Add("newtable", "True");
        data.Add("seatlimit", maxPlayers);
        data.Add("min_buyin", 500);
        data.Add("max_buyin", 2000);
        data.Add("bigblind", 10);
        data.Add("avatar", UserInfo.instance.PlayerCurrentData.data.photo);
        data.Add("photoUrl", Convert.ToInt32(UserInfo.instance.PlayerCurrentData.data.photo_index));
        data.Add("photoType", UserInfo.instance.PlayerCurrentData.data.photo_type);
        data.Add("mode", "cash");

        Debug.Log(JsonMapper.ToJson(data));
        socketManager.Socket.Emit(PokerConstants.CREATE_ROOM_REQUEST, JsonMapper.ToJson(data));
    }

    public void OnGetRooms()
    {
        socketManager.Socket.Emit(PokerConstants.GET_ROOMS);
    }

    public void OnPlayerInfo(string room_id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("room_id", room_id);
        Debug.Log("The waiting player request is: " + JsonMapper.ToJson(data));
        socketManager.Socket.Emit(PokerConstants.WAITING_PLAYERS, JsonMapper.ToJson(data));
    }

    public void OnRegister(string username, string password, string email, string country)
    {

        //username, password,email, language, country

        Dictionary<string, string> data = new Dictionary<string, string>();

        data.Add("username", username);
        data.Add("password", password);
        data.Add("email", email);
        data.Add("language", "english");
        data.Add("country", country);

        Debug.Log("The registration request is: " + JsonMapper.ToJson(data));

        socketManager.Socket.Emit(PokerConstants.REGISTER_REQUEST, JsonMapper.ToJson(data));
    }

    [Serializable]
    public class UserLogin
    {
        public string username;
        public string password;
    }

   
    public void OnLogin(string username, string password)
    {

        /* UserLogin userLogin = new UserLogin();
         userLogin.username = username;
         userLogin.password = password;
         string jsonData = JsonUtility.ToJson(userLogin);*/
        Debug.Log("The user data had sended" + "Username: " + username + " password: " + password);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("username", username);
        data.Add("password", password);

        string jsonData = JsonConvert.SerializeObject(data);
        Debug.Log(jsonData); 
        socketManager.Socket.Emit(PokerConstants.LOGIN_REQUEST, JsonMapper.ToJson(data));
       // socketManager.Socket.Emit(PokerConstants.LOGIN_REQUEST, jsonData);
    }

    public void OnChangeUserAvatar(int userid, int avatarId)
    {

        //username, password,email, language, country

        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("userid", userid);
        data.Add("photo_index", avatarId);
     


        Debug.Log("The Avatar request: " + JsonMapper.ToJson(data));

        socketManager.Socket.Emit(PokerConstants.PHOTOUPDATE_REQUEST, JsonMapper.ToJson(data));
    }

    #endregion
}
