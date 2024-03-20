using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class RegistrationErrorData
{
    public string result;
    public string data;
}

public class PokerConstants 
{
    public const string VALID_NAME_RESULT = "REQ_VALID_NAME_RESULT";
    public const string VALID_NAME_REQUEST = "REQ_VALID_NAME";
    public const string LOGIN_RESULT = "GET_LOGIN_RESULT";
    public const string LOGIN_REQUEST = "REQ_LOGIN";
    public const string REGISTER_RESULT = "GET_REGISTER_RESULT";
    public const string REGISTER_REQUEST = "REQ_REGISTER";
    public const string USER_REQUEST = "REQ_USER_INFO";
    public const string USER_RESPONSE = "GET_USERINFO_RESULT";
    public const string PHOTOUPDATE_REQUEST = "UDATE_PHOTO_INDEX";
    public const string VERIFY_REQUEST = "REQ_VERIFY";
    public const string VERIFY_RESPONSE = "REQ_VERIFY_RESULT";
    public const string CREATE_ROOM_REQUEST = "REQ_ENTER_ROOM";
    public const string GET_ROOMS = "GET_ROOMS";
    public const string WAITING_PLAYERS = "WAITING_PLAYERS";
}


[Serializable]
public class BestWinningHand
{
    public List<object> cards;
    public string hand;
    public double handval;
}

[Serializable]
public class RegisterData
{
    public string username;
    public double userid;
    public string password;
    public string photo;
    public double photo_index;
    public double points;
    public double level;
    public List<double> archivement;
    public double hands_played;
    public double hands_won;
    public double biggest_pot_won;
    public BestWinningHand best_winning_hand;
    public double win_percent_holdem;
    public double win_percent_spin;
    public double tour_won;
    public double likes;
    public double buddies;
    public List<object> friends;
    public List<object> recents;
    public string referral_code;
    public double referral_count;
    public List<object> referral_users;
    public DateTime created_date;
    public DateTime mail_date;
    public DateTime spin_date;
    public DateTime dailyReward_date;
    public List<object> messages;
    public double status;
    public double connected_room;
    public string connect;
    public string email;
    public string language;
    public string country;
    public string verified;
    public string code;
    public string _id;
}

[Serializable]
public class UserRegister
{
    public string result;
    public RegisterData data;
}


//64a0d946d4381c09a08ca24e

[Serializable]
public class PlayerProfileData
{
    public string _id;
    public string username;
    public double userid;
    public string password;
    public string photo;
    public double photo_index;
    public double photo_type;
    public string facebook_id;
    public double points;
    public double level;
    public List<double> archivement;
    public double hands_played;
    public double hands_won;
    public double biggest_pot_won;
    public string best_winning_hand;
    public double win_percent_holdem;
    public double win_percent_spin;
    public double tour_won;
    public double likes;
    public double buddies;
    public List<object> friends;
    public List<object> recents;
    public string referral_code;
    public double referral_count;
    public List<object> referral_users;
    public DateTime created_date;
    public DateTime mail_date;
    public DateTime spin_date;
    public DateTime dailyReward_date;
    public List<object> messages;
    public double status;
    public string connected_room;
    public string connect;
    public string email;
    public string language;
    public string country;
}

[Serializable]
public class PlayerProfile
{
    public string result;
    public PlayerProfileData data;
}



