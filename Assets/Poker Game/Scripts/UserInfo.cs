using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using System.ComponentModel;
using UnityEngine.EventSystems;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using Photon.Realtime;
using UnityEngine.UI.Extensions;
using System;
using TMPro;



[Serializable]
public class VerificationCodeData
{
    public string result;
}

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;

    public GameObject loginPanel;
    public GameObject forgottPassPanel;
    public GameObject holdemPanel;
    public GameObject loadingPanel;
    public GameObject chooseAvtarPanel;
    public GameObject errorPopUpPanel;
    public GameObject registrationPanel_country;
    public GameObject registrationPanel_1;
    public GameObject registrationPanel_2;
    public GameObject registrationPanel_3;
    public GameObject myprofilePanel;
    public GameObject exitPanel;
    public GameObject verifyEmailPanel;
    public GameObject AddBalancePanel;

    public Text errorMessageText;
    public Text errorMessageText_popUp;

    public TMP_InputField email_login;
    public TMP_InputField password_login;
    public Toggle toggle_rememberEmail;
    public Toggle toggle_rememberPass;

    public TMP_InputField email_forgotPassword;

    public AutoCompleteComboBox _autoCompleteComboBox;
    public AutoCompleteComboBox _autoCompleteComboBox_profile;

    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public Text emailIdText;

    public List<Sprite> avtars;
    public List<Button> avtars_buttons;
    public List<Button> avtars_buttons_change;

    //public TMP_InputField email_forgotPass;

    public InputField myprofile_1;
    public TMP_InputField username_1;
    public TMP_InputField email_1;
    public Dropdown country_1;
    public Image profileImage_1;

    public Image profileImage_2;
    public Text username_2;

    public Toggle toggle_acceptAll;
    public Toggle toggle_first;
    public Toggle toggle_second;
    public Button nextButton;

    public TMP_InputField pinCode;
    public Button submitCodeBtn;

    public TMP_InputField balanceToAdd;

    public string _countryId;
    [SerializeField]
    public string _avtarId = "0";
    [SerializeField]
    public int _userId = 0;
    public string _username;
    public string _email;
    public string _varificationCode;
    public string _balance = "5";
    public string _depositeHistory = "";

    public Sprite selected_avtar;
    public Sprite deselected_avtar;

    public List<Text> balanceTexts;

    public Sprite bgBox1;
    public Sprite bgBox2;

    public Transform parent_DepositeHistory;
    public GameObject item_DepositeHistory;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public const string MatchUsernamePattern = ".*[a-zA-Z]+.*";

    private string checkemail;
    [SerializeField]
    private PlayerProfile m_PlayerCurrentData;
    [SerializeField]
    private UserRegister m_PlayerRegisterData;

    public PlayerProfile PlayerCurrentData => m_PlayerCurrentData;

    private string m_RegistrationCountry;
    private string m_RegistrationName;
    private string m_RegistrationPassword;
    private string m_RegistrationEmail;
    private string m_RegistrationLanguage;
    [SerializeField]
    private GameObject m_RegistrationErrorText;
    [SerializeField]
    private GameObject m_VerificationEmailText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        print("------AutoLogin--->>>>>>" + PlayerPrefs.GetInt("AutoLogin", 0));
        if (PlayerPrefs.GetInt("AutoLogin", 0) == 1)
        {
            //skchangesStartCoroutine(AutoLogin());

            if (PlayerPrefs.GetInt("RememberEmail", 0) == 1)
            {
                toggle_rememberEmail.isOn = true;               
            }
            else
                toggle_rememberEmail.isOn = false;


            if (PlayerPrefs.GetInt("RememberPass", 0) == 1)
            {
                toggle_rememberPass.isOn = true;                
            }
            else
                toggle_rememberPass.isOn = false;
        }
        else
        {      
            if (PlayerPrefs.GetInt("RememberEmail", 0) == 1)
            {   
                toggle_rememberEmail.isOn = true;
                email_login.text = PlayerPrefs.GetString("Email", "");
            }
            else
                toggle_rememberEmail.isOn = false;

      
            if (PlayerPrefs.GetInt("RememberPass", 0) == 1)
            {      
                toggle_rememberPass.isOn = true;
                password_login.text = PlayerPrefs.GetString("Password");
            }
            else
                toggle_rememberPass.isOn = false;

            loadingPanel.SetActive(false);
            if(PlayerPrefs.GetString("Email", "") != "")
                email_forgotPassword.text = PlayerPrefs.GetString("Email", "");
        }

        print("---Deposite History--->>>>> " + System.DateTime.Now.ToString("h:m") + System.DateTime.Now.ToString("tt"));
    }

    private void OnEnable()
    {
        ServerManager.s_OnLoginResultRecived += OnLoginResponseRecevied;
        ServerManager.s_OnRegistrationResultRecived += OnRegistrationResponseRecevied;
        ServerManager.s_OnUpdateProfileRecived += OnUpdateProfileResponseRecevied;
        ServerManager.s_OnVerificationMailResultRecived += OnVerificationMailResponseRecevied;
    }

    private void OnDisable()
    {
        ServerManager.s_OnLoginResultRecived -= OnLoginResponseRecevied;
        ServerManager.s_OnRegistrationResultRecived -= OnRegistrationResponseRecevied;
        ServerManager.s_OnUpdateProfileRecived -= OnUpdateProfileResponseRecevied;
        ServerManager.s_OnVerificationMailResultRecived -= OnVerificationMailResponseRecevied;

    }



    public IEnumerator AutoLogin()  
    {
        print("----------AutoLogin------------");
        yield return new WaitForSeconds(0);
        email_login.text = PlayerPrefs.GetString("Email", "");
        password_login.text = PlayerPrefs.GetString("Password");
        loadingPanel.SetActive(true);
        print("----------Email------------" + PlayerPrefs.GetString("Email", ""));
        print("----------Password------------" + PlayerPrefs.GetString("Password"));
        var request = new LoginWithEmailAddressRequest
        {
            Email = PlayerPrefs.GetString("Email", ""),
            Password = PlayerPrefs.GetString("Password")
            //Email = "a@gmail.com",
            //Password = "123456"
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        yield return new WaitForSeconds(3);
        loadingPanel.SetActive(false);
    }

    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    public static bool validateUsername(string name)
    {
        if (name != null)
            return Regex.IsMatch(name, MatchUsernamePattern);
        else
            return false;
    }

    public void ConfirmCountryButton()
    {
        if (_autoCompleteComboBox._selectionIsValid)
        {
            m_RegistrationCountry = _autoCompleteComboBox._mainInput.text;
            Debug.Log("The selected country is: "+_autoCompleteComboBox._mainInput.text);
            registrationPanel_1.SetActive(true);
        }
        else
        {
            errorPopUpPanel.SetActive(true);
            if(PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "お国を選択してください";
            else
                errorMessageText_popUp.text = "Please select your country";
        }
    }

    public void NextBtn()
    {
        if (string.IsNullOrEmpty(username.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "名前を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter Name";
            return;
        }
        else if (!validateUsername(username.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効な名前を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid Name";
            return;
        }
        else if (string.IsNullOrEmpty(email.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter E-mail address";
            return;
        }
        else if(!validateEmail(email.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効なメールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid E-mail address";
            return;
        }
        else if (string.IsNullOrEmpty(password.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter Password";
            return;
        }
        else if (password.text.Contains(" "))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードに空白は使えません。";
            else
                errorMessageText_popUp.text = "Password can not have blank space";
            return;
        }
        else if (password.text.Length < 6)
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードは最低6桁で入力してください。";
            else
                errorMessageText_popUp.text = "Password should be minimum 6 digits";
            return;
        }
        else
        {

                CheckAccountAvailability();
        }
    }

    public void NextBtn_Node()
    {
        if (string.IsNullOrEmpty(username.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "名前を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter Name";
            return;
        }
        else if (!validateUsername(username.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効な名前を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid Name";
            return;
        }
        else if (string.IsNullOrEmpty(email.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter E-mail address";
            return;
        }
        else if (!validateEmail(email.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効なメールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid E-mail address";
            return;
        }
        else if (string.IsNullOrEmpty(password.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter Password";
            return;
        }
        else if (password.text.Contains(" "))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードに空白は使えません。";
            else
                errorMessageText_popUp.text = "Password can not have blank space";
            return;
        }
        else if (password.text.Length < 6)
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードは最低6桁で入力してください。";
            else
                errorMessageText_popUp.text = "Password should be minimum 6 digits";
            return;
        }
        else
        {
            m_RegistrationName = username.text;
            m_RegistrationEmail = email.text;
            m_RegistrationPassword = password.text;
            registrationPanel_2.SetActive(true);    
        }
    }

    public void CheckAccountAvailability()
    {
        loadingPanel.SetActive(true);
        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = "dddddddd"
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, AvailableSuccess, AvailableError);
    }

    void AvailableSuccess(LoginResult result)
    {
        errorPopUpPanel.SetActive(true);
        if (PlayerPrefs.GetInt("Language", 0) == 1)
            errorMessageText_popUp.text = "メールアドレスが利用できません。";
        else
            errorMessageText_popUp.text = "Email address not available";
        loadingPanel.SetActive(false);        
    }

    void AvailableError(PlayFabError error)
    {
        print("AvailableError-->>>" + error.ErrorMessage);
        loadingPanel.SetActive(false);
        if (error.ErrorMessage.Contains("User not found"))
            registrationPanel_2.SetActive(true);
        else
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスが利用できません。";
            else
                errorMessageText_popUp.text = "Email address not available";
        }

    }

    public void RegisterButton_Node()
    {
        if(toggle_acceptAll.isOn && toggle_second.isOn)
        {
            loadingPanel.SetActive(true);
            ServerManager.instance.OnRegister(m_RegistrationName, m_RegistrationPassword, m_RegistrationEmail, m_RegistrationCountry); ;
        }
        
    }

   

    public void RegisterButton()
    {     
        loadingPanel.SetActive(true);

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = username.text,
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        AddOrUpdateContactEmail(email.text);
        PlayerPrefs.SetString("Email", email.text);
        PlayerPrefs.SetString("Password", password.text);

        errorPopUpPanel.SetActive(true);
        if (PlayerPrefs.GetInt("Language", 0) == 1)
            errorMessageText_popUp.text = "登録が完了しました。";
        else
            errorMessageText_popUp.text = "Please verify your email address";
        print("Registration successful");
        loadingPanel.SetActive(false);
        //holdemPanel.SetActive(true);
        DateTime dateTime = DateTime.Now;
        _depositeHistory += dateTime.ToShortDateString() + "|" + dateTime.ToString("h:m") + dateTime.ToString("tt") + "|$" + _balance + "&";
        Invoke("SendData", 1);

        chooseAvtarPanel.SetActive(false);
        registrationPanel_2.SetActive(false);
        registrationPanel_1.SetActive(false);
        registrationPanel_country.SetActive(false);
        email_login.text = email.text;
        password_login.text = password.text;
    }


    void AddOrUpdateContactEmail(string emailAddress)
    {
        Debug.Log("---------------AddOrUpdateContactEmail------------------");
        var request = new AddOrUpdateContactEmailRequest
        {
            //PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    void OnErrorNode()
    {
        errorMessageText.gameObject.SetActive(true);
        errorPopUpPanel.SetActive(true);

        if (PlayerPrefs.GetInt("Language", 0) == 1)
            errorMessageText_popUp.text = "ユーザー資格情報が無効です";
        else
            errorMessageText_popUp.text = "The User Credentials are Invalid!";
    }

    void OnError(PlayFabError error)
    {
        loadingPanel.SetActive(false);
        print(error.GenerateErrorReport());
        errorMessageText.gameObject.SetActive(true);
        errorPopUpPanel.SetActive(true);
        if (error.ErrorMessage.Contains("Email address not available"))
        {
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "登録済みのメールアドレスです";
            else
                errorMessageText_popUp.text = "Email address is already registered with us";
        }
        else if (error.ErrorMessage.Contains("Invalid email address or password") || error.ErrorMessage.Contains("Invalid input parameters"))
        {
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "無効なパスワード";
            else
                errorMessageText_popUp.text = "Invalid password";
        }
        else if (error.ErrorMessage.Contains("User not found"))
        {
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "ユーザーが見つかりません";
            else
                errorMessageText_popUp.text = "User not found";
        }
        else if(error.ErrorMessage.Contains("Cannot resolve destination host"))
        {
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "Poor internet connection";
            else
                errorMessageText_popUp.text = "Poor internet connection";
        }
        else
        {
            errorMessageText_popUp.text = error.ErrorMessage;
        }

        errorMessageText.text = errorMessageText_popUp.text;
    }

    public void OnSubmitVerifcationCode()
    {
        ServerManager.instance.OnUserVerification((int)m_PlayerRegisterData.data.userid, pinCode.text);
    }

    public void LoginButton()
    {
        if (string.IsNullOrEmpty(email_login.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter E-mail address";
            return;
        }
       /* else if (!validateEmail(email_login.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効なメールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid E-mail address";
            return;
        }*/
        else if (string.IsNullOrEmpty(password_login.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "パスワードを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter Password";
            return;
        }
        /* else if (password_login.text.Length < 6)
         {
             errorPopUpPanel.SetActive(true);
             if (PlayerPrefs.GetInt("Language", 0) == 1)
                 errorMessageText_popUp.text = "パスワードは最低6桁で入力してください。";
             else
                 errorMessageText_popUp.text = "Password should be minimum 6 digits";
             return;
         }*/


        //SKCHNAGESloadingPanel.SetActive(true);

        ServerManager.instance.OnLogin(email_login.text, password_login.text);
      /*  OnError();*/
      /*  var request = new LoginWithEmailAddressRequest
        {
            Email = email_login.text,
            Password = password_login.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);*/
    }

    private void OnLoginResponseRecevied(string jsonResponse)
    {
        PlayerProfile playerProfile = JsonUtility.FromJson<PlayerProfile>(jsonResponse);
        if(playerProfile.result == "success")
        {
            m_PlayerCurrentData = playerProfile;
            loadingPanel.SetActive(true);
           // if()
            OnLoginSuccessNode();
        }
        else
        {
            OnErrorNode();
        }
        
    }

    private void OnRegistrationResponseRecevied(string jsonResponse)
    {
        UserRegister registerUser = JsonUtility.FromJson<UserRegister>(jsonResponse);
        m_RegistrationErrorText.gameObject.SetActive(false);

        if (registerUser.result == "success")
        {
            m_PlayerRegisterData = registerUser;
            loadingPanel.SetActive(false);
            Debug.Log("The registration response is: " + jsonResponse);
            registrationPanel_3.SetActive(true);
        }else if(registerUser.result == "failed")
        {
            RegistrationErrorData registerror = JsonUtility.FromJson<RegistrationErrorData>(jsonResponse);
            loadingPanel.SetActive(false);
            m_RegistrationErrorText.gameObject.SetActive(true);
            m_RegistrationErrorText.GetComponent<TextMeshProUGUI>().text = registerror.data;
            m_RegistrationErrorText.GetComponent<TextMeshProUGUI>().color = Color.red;

        }

    }

    private void OnUpdateProfileResponseRecevied(string jsonResponse)
    {
        PlayerProfile playerProfile = JsonUtility.FromJson<PlayerProfile>(jsonResponse);
        if (playerProfile.result == "success")
        {
            m_PlayerCurrentData = playerProfile;
           /* OnLoginSuccessNode();*/
        }
    }
    private void OnVerificationMailResponseRecevied(string jsonResponse)
    {
        VerificationCodeData data = JsonUtility.FromJson<VerificationCodeData>(jsonResponse);
        if (data.result == "success")
        {
            registrationPanel_1.SetActive(false);
            registrationPanel_2.SetActive(false);
            registrationPanel_3.SetActive(false);
            holdemPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
        else
        {
            m_VerificationEmailText.GetComponent<TextMeshProUGUI>().text = data.result;
        }
    }

    void OnLoginSuccessNode()
    {
        if (toggle_rememberEmail.isOn)
            PlayerPrefs.SetInt("RememberEmail", 1);

        else
            PlayerPrefs.SetInt("RememberEmail", 0);


        if (toggle_rememberPass.isOn)
            PlayerPrefs.SetInt("RememberPass", 1);

        else
            PlayerPrefs.SetInt("RememberPass", 0);

        //turn it on when if verification is ready
        /* if (PlayerPrefs.GetInt("AutoLogin", 0) == 0)
         {
             GetContactMailVarification(result.PlayFabId);
         }
         else
         {
             GetData();
             loadingPanel.SetActive(false);
             loginPanel.SetActive(false);
             holdemPanel.SetActive(true);
             SoundManager.instance.BgMusic();
         }*/
        GetDataNode();
        loadingPanel.SetActive(false);
        loginPanel.SetActive(false);
        holdemPanel.SetActive(true);
        SoundManager.instance.BgMusic();

        PlayerPrefs.SetString("Email", email_login.text);
        PlayerPrefs.SetString("Password", password_login.text);
        //if (PlayerPrefs.GetInt("Language", 0) == 1)
        //    errorMessageText_popUp.text = "ログイン成功";
        //else
        //    errorMessageText_popUp.text = "Login successful";
        print("Login successful");
        //loadingPanel.SetActive(false);
        //loginPanel.SetActive(false);
        //holdemPanel.SetActive(true);
        //GetData();


        //GetContactMailVarification_local();
    }


    void OnLoginSuccess(LoginResult result)
    {
        if (toggle_rememberEmail.isOn)
            PlayerPrefs.SetInt("RememberEmail", 1);

        else
            PlayerPrefs.SetInt("RememberEmail", 0);


        if (toggle_rememberPass.isOn)
            PlayerPrefs.SetInt("RememberPass", 1);

        else
            PlayerPrefs.SetInt("RememberPass", 0);


        if (PlayerPrefs.GetInt("AutoLogin", 0) == 0)
        {
            GetContactMailVarification(result.PlayFabId);
        }
        else
        {
            GetData();
            loadingPanel.SetActive(false);
            loginPanel.SetActive(false);
            holdemPanel.SetActive(true);
            SoundManager.instance.BgMusic();
        }
        PlayerPrefs.SetString("Email", email_login.text);
        PlayerPrefs.SetString("Password", password_login.text);
        //if (PlayerPrefs.GetInt("Language", 0) == 1)
        //    errorMessageText_popUp.text = "ログイン成功";
        //else
        //    errorMessageText_popUp.text = "Login successful";
        print("Login successful");
        //loadingPanel.SetActive(false);
        //loginPanel.SetActive(false);
        //holdemPanel.SetActive(true);
        //GetData();

        
        //GetContactMailVarification_local();
    }

    public void GetContactMailVarification(string playfabId)
    {
        //Debug.Log("£---------GetContactMailVarification-------------" + playfabId);
        var request = new GetPlayerProfileRequest()
        {
            PlayFabId = playfabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowContactEmailAddresses = true
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, OnSuccessGetContactEmail, OnErrorGetContactEmail);

        //var request = new GetAccountInfoRequest()
        //{

        //}
    }

    private void OnErrorGetContactEmail(PlayFabError error)
    {
        Debug.Log("£---------OnErrorGetContactEmail-------------");
        loadingPanel.SetActive(false);
        Debug.Log("£" + error.ErrorMessage);
    }

    //public void GetContactMailVarification_local(string email)
    //{
    //    Debug.Log("---------OnSuccessGetContactEmail-------------" + result);
    //    //if (result == null)
    //    {
    //        var contactEmailAdresses = result.PlayerProfile.ContactEmailAddresses;
    //        //var playeremail = email_login.text;
    //        var emailVerificationStatus = false ? (string?)null : null;
    //        if (string.IsNullOrEmpty(email))
    //        {
    //            emailVerificationStatus = email.VerificationStatus.ToString();
    //            if (emailVerificationStatus != null)
    //            {
    //                if (emailVerificationStatus == "Confirmed")
    //                    checkemail = contactEmailAdresses[0].EmailAddress;
    //                else
    //                    checkemail = "Current contact email is not verified.";
    //            }
    //            else
    //                checkemail = "Current email is not verified.";
    //        }
    //        else
    //            checkemail = "Error";
    //    }
    //    Debug.Log(checkemail);
    //}

    public void OnSuccessGetContactEmail(GetPlayerProfileResult result)
    {
        Debug.Log("£---------OnSuccessGetContactEmail-------------" + result);
        loadingPanel.SetActive(false);

        if (result != null)
        {
        Debug.Log("£---------result-------------" + result.PlayerProfile.ContactEmailAddresses);
            var contactEmailAdresses = result.PlayerProfile.ContactEmailAddresses;
            //var playeremail = email_login.text;
            var emailVerificationStatus = false ? (string?)null : null;
            if ((contactEmailAdresses != null) && (contactEmailAdresses.Count > 0))
            {
                emailVerificationStatus = contactEmailAdresses[0].VerificationStatus.ToString();
                if (emailVerificationStatus != null)
                {
                    if (emailVerificationStatus == "Confirmed")
                    {
                        checkemail = contactEmailAdresses[0].EmailAddress;
                        GetData();

                        errorPopUpPanel.SetActive(true);
                        if (PlayerPrefs.GetInt("Language", 0) == 1)
                            errorMessageText_popUp.text = "ログイン成功";
                        else
                            errorMessageText_popUp.text = "Login successful";
                        loginPanel.SetActive(false);
                        holdemPanel.SetActive(true);
                        SoundManager.instance.BgMusic();
                    }

                    else
                    {
                        checkemail = "Current contact email is not verified.";
                        verifyEmailPanel.SetActive(true);
                    }
                }
                else
                {
                    checkemail = "Current email is not verified.";
                    verifyEmailPanel.SetActive(true);
                }
            }
            else
            {
                checkemail = "Error";
            }
        }
        Debug.Log("£----" + checkemail);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email_forgotPassword.text,
            //TitleId = "E21FB"
            TitleId = "4D9A5"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);

    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        print("password reset successful");
        if (PlayerPrefs.GetInt("Language", 0) == 1)
            errorMessageText_popUp.text = "パスワードリセットリンクをあなたのメールに送信しました。メールを確認してください。";
        else
            errorMessageText_popUp.text = "Reset passwork link sent to your email. Please check email";
        errorPopUpPanel.SetActive(true);
        Invoke("PanelChange", 3);
    }

    void PanelChange()
    {
        loginPanel.SetActive(true);
        forgottPassPanel.SetActive(false);
    }

    public void SendData_Node()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Country", _countryId },
                { "Username", _username },
                { "AvtarId", _avtarId },
                { "Balance", _balance },
                { "DepositeHistory", _depositeHistory }
            }
        };
        loadingPanel.SetActive(false);

        //call your api here sahil  PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void SendData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Country", _countryId },
                { "Username", _username },
                { "AvtarId", _avtarId },
                { "Balance", _balance },
                { "DepositeHistory", _depositeHistory }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceive, OnError);
    }
    public void GetDataNode()
    {
        OnDataReceive_Node();
    }

    public void UpdateProfile_Node()
    {
        if (string.IsNullOrEmpty(username_1.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "ユーザー名を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter username";
            return;
        }
        else if (!validateUsername(username_1.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "ユーザー名を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid username";
            return;
        }
        else if (string.IsNullOrEmpty(email_1.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter E-mail address";
            return;
        }
       /* else if (!validateEmail(email_1.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効なメールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid E-mail address";
            return;
        }*/
        else if (!_autoCompleteComboBox_profile._selectionIsValid)
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "お国を選択してください";
            else
                errorMessageText_popUp.text = "Please select country";
            return;
        }
        else
        {
            myprofilePanel.SetActive(false);
            _username = username_1.text;
            //_countryId = country_1.value.ToString();
            _email = email_1.text;
            SendData_Node();
        }
    }


    public void UpdateProfile()
    {
        if (string.IsNullOrEmpty(username_1.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "ユーザー名を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter username";
            return;
        }
        else if (!validateUsername(username_1.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "ユーザー名を入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid username";
            return;
        }
        else if (string.IsNullOrEmpty(email_1.text.Trim()))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "メールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter E-mail address";
            return;
        }
        else if (!validateEmail(email_1.text))
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "有効なメールアドレスを入力してください。";
            else
                errorMessageText_popUp.text = "Please enter valid E-mail address";
            return;
        }
        else if(!_autoCompleteComboBox_profile._selectionIsValid)
        {
            errorPopUpPanel.SetActive(true);
            if (PlayerPrefs.GetInt("Language", 0) == 1)
                errorMessageText_popUp.text = "お国を選択してください";
            else
                errorMessageText_popUp.text = "Please select country";
            return;
        }
        else
        {
            myprofilePanel.SetActive(false);
            _username = username_1.text;
            //_countryId = country_1.value.ToString();
            _email = email_1.text;
            SendData();
        }
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        print("Data Sending successful");
        GetData();
    }    

    void fail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void OnDataReceive_Node()
    {
        print("Data Getting successful -->>> " + m_PlayerCurrentData);

        if (m_PlayerCurrentData.data != null && m_PlayerCurrentData.data.country != null)
        {
            _username = m_PlayerCurrentData.data.username;
            _countryId = m_PlayerCurrentData.data.country;
            _avtarId = m_PlayerCurrentData.data.photo_index.ToString();
            _balance = /*m_PlayerCurrentData.data.points.ToString();*/5000.ToString(); // skchanges
            _userId = Convert.ToInt32(m_PlayerCurrentData.data.userid);
            //_depositeHistory = result.Data["DepositeHistory"].Value;
            print("_balance -->>> " + _balance);
        }
        print("PlayerList -->>> " + PhotonNetwork.PlayerList.Length + " ---->>>> " + PhotonNetwork.CountOfPlayers);

        float bal = float.Parse(_balance);
        for (int i = 0; i < balanceTexts.Count; i++)
        {
            balanceTexts[i].text = "$" + bal.ToString("0.00");
        }


        print("_balance -->>> " + _balance);
        float f = float.Parse(_balance);
        PlayerPrefs.SetFloat("Balance", f);
        SetProfile();
        SetMepanel();

        //on it when email verification is done
        /*if (PlayerPrefs.GetInt("AutoLogin", 0) == 1)
        {
            PlayerPrefs.SetInt("AutoLogin", 0);
            _balance = PlayerPrefs.GetFloat("Balance").ToString();
            SendData();
        }
        else
        {
            print("_balance -->>> " + _balance);
            float f = float.Parse(_balance);
            PlayerPrefs.SetFloat("Balance", f);
            SetProfile();
            SetMepanel();
        }*/



        loadingPanel.SetActive(false);
       //skchnages SetDepositeHistoryPanel();
        //PokerPhotonManager.instance.OnLogInPlayerButton(_username);
        PokerPhotonManager.instance.playerName = _username;
        //Invoke("OnLogin", 5);
     //skchnages   PhotonNetwork.ConnectUsingSettings();
    }


    void OnDataReceive(GetUserDataResult result)
    {
        print("Data Getting successful -->>> " + result);

        if (result.Data != null && result.Data.ContainsKey("Country") && result.Data.ContainsKey("AvtarId"))
        {
            _username = result.Data["Username"].Value;
            _countryId = result.Data["Country"].Value;
            _avtarId = result.Data["AvtarId"].Value;
            _balance = result.Data["Balance"].Value;
            _depositeHistory = result.Data["DepositeHistory"].Value;
            print("_balance -->>> " + _balance);
        }
        print("PlayerList -->>> " + PhotonNetwork.PlayerList.Length + " ---->>>> " + PhotonNetwork.CountOfPlayers);

        float bal = float.Parse(_balance);
        for (int i = 0; i < balanceTexts.Count; i++)
        {
            balanceTexts[i].text = "$" + bal.ToString("0.00");
        }

        if (PlayerPrefs.GetInt("AutoLogin", 0) == 1)
        {
            PlayerPrefs.SetInt("AutoLogin", 0);
            _balance = PlayerPrefs.GetFloat("Balance").ToString();
            SendData();
        }
        else
        {
            print("_balance -->>> " + _balance);
            float f = float.Parse(_balance);
            PlayerPrefs.SetFloat("Balance", f);
            SetProfile();
            SetMepanel();
        }
        loadingPanel.SetActive(false);
        SetDepositeHistoryPanel();
        //PokerPhotonManager.instance.OnLogInPlayerButton(_username);
        PokerPhotonManager.instance.playerName = _username;
        //Invoke("OnLogin", 5);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetCountry(Dropdown country)
    {
        _countryId = country.value.ToString();
    }

    public void SetUsername(TMP_InputField username)
    {
        _username = username.text;
    }

    public void SetEmail(TMP_InputField email)
    {
        _email = email.text;
        emailIdText.text = _email;
    }

    public void SetAvtar(int _id)
    {
        _avtarId = _id.ToString();
        profileImage_1.sprite = avtars[int.Parse(_avtarId)];

        for(int i = 0; i < avtars_buttons.Count; i++)
        {
            if (i == _id)
                avtars_buttons[i].GetComponent<Image>().sprite = selected_avtar;
            else
                avtars_buttons[i].GetComponent<Image>().sprite = deselected_avtar;
        }        
    }

    public void SetAvtar_change(int _id)
    {
        _avtarId = _id.ToString();
        profileImage_1.sprite = avtars[int.Parse(_avtarId)];

        for (int i = 0; i < avtars_buttons_change.Count; i++)
        {
            if (i == _id)
                avtars_buttons_change[i].GetComponent<Image>().sprite = selected_avtar;
            else
                avtars_buttons_change[i].GetComponent<Image>().sprite = deselected_avtar;
        }
    }

    public void SetProfile()
    {
        myprofile_1.text = _username;
        username_1.text = _username;
        email_1.text = _email;
        //country_1.value = int.Parse(_countryId);//skchnages
        profileImage_1.sprite = avtars[int.Parse(_avtarId)];
        SetAvtar_change(int.Parse(_avtarId));
    }

    public void SetMepanel()
    {
        print("----------SetMepanel-----------");
        profileImage_2.sprite = avtars[int.Parse(_avtarId)];
        username_2.text = _username;
    }

    public void CheckToggles_All()
    {
        if (toggle_acceptAll.isOn)
        {
            print("--------3---------");
            toggle_first.isOn = true;
            toggle_second.isOn = true;
            nextButton.interactable = true;
        }
        else if (toggle_first.isOn && toggle_second.isOn)
        {
            toggle_first.isOn = false;
            toggle_second.isOn = false;
            nextButton.interactable = false;
        }
        else
        {            
            nextButton.interactable = false;
        }
    }

    public void CheckToggles_First()
    {
        if (toggle_first.isOn)
        {
            if (toggle_second.isOn)
            {
                toggle_acceptAll.isOn = true;
                nextButton.interactable = true;
            }
        }
        else
        {
            toggle_acceptAll.isOn = false;
            nextButton.interactable = false;
        }
    }

    public void CheckToggles_Second()
    {
        if (toggle_second.isOn)
        {
            if (toggle_first.isOn)
            {
                toggle_acceptAll.isOn = true;
                nextButton.interactable = true;
            }
        }
        else
        {
            toggle_acceptAll.isOn = false;
            nextButton.interactable = false;
        }
    }

    public void CheckVarificationCode()
    {
        //string code = code_1.text + code_2.text + code_3.text + code_4.text;
        string code = pinCode.text;
        print("=========CheckVarificationCode=========>> " + code);
        chooseAvtarPanel.SetActive(true);
    }

    public void CheckPinEmpty()
    {        
        if(pinCode.text.Length == 4)
        {
            submitCodeBtn.interactable = true;
        }
        else
        {
            submitCodeBtn.interactable = false;
        }
    }

    public void Logout()
    {
        Destroy(GameObject.Find("PokerPhotonManager"));
        SceneManager.LoadScene("PunBasics-Launcher");
    }

    public void SetToggles_Email()
    {
        if (toggle_rememberEmail.isOn)
        {
            PlayerPrefs.SetInt("RememberEmail", 1);
        }
        else
            PlayerPrefs.SetInt("RememberEmail", 0);
    }

    public void SetToggles_Password()
    {
        if (toggle_rememberPass.isOn)
        {
            PlayerPrefs.SetInt("RememberPass", 1);
        }
        else
            PlayerPrefs.SetInt("RememberPass", 0);
    }

    public void Exit_Yes()
    {
        Application.Quit();
    }

    public void OnEndEditEmail(TMP_InputField inputField)
    {
        string value = inputField.text.Trim();
        inputField.text = value.ToLower();
    }

    public void AddBalance_Node()
    {
        loadingPanel.SetActive(true);
        AddBalancePanel.SetActive(false);
        float b = float.Parse(balanceToAdd.text);
        PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance", 0) + b);
        _balance = PlayerPrefs.GetFloat("Balance", 0).ToString();
        DateTime dateTime = DateTime.Now;
        _depositeHistory += dateTime.ToShortDateString() + "|" + dateTime.ToString("h:m") + dateTime.ToString("tt") + "|$" + balanceToAdd.text + "&";
        SendData_Node();
    }

    public void AddBalance()
    {
        loadingPanel.SetActive(true);
        AddBalancePanel.SetActive(false);
        float b = float.Parse(balanceToAdd.text);
        PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance", 0) + b);
        _balance = PlayerPrefs.GetFloat("Balance", 0).ToString();
        DateTime dateTime = DateTime.Now;
        _depositeHistory += dateTime.ToShortDateString() + "|" + dateTime.ToString("h:m") + dateTime.ToString("tt") + "|$" + balanceToAdd.text + "&";
        SendData();
    }

    public void SetDepositeHistoryPanel()
    {
        string[] column = _depositeHistory.Split('&', StringSplitOptions.RemoveEmptyEntries);

        for(int i = parent_DepositeHistory.childCount; i < column.Length; i++)
        {
            GameObject newItem = Instantiate(item_DepositeHistory) as GameObject;
            if (i % 2 == 0)
                newItem.GetComponent<Image>().sprite = bgBox1;
            else
                newItem.GetComponent<Image>().sprite = bgBox2;
            newItem.SetActive(true);
            newItem.transform.SetParent(parent_DepositeHistory, false);
            string[] row = column[i].Split('|', StringSplitOptions.RemoveEmptyEntries);
            for(int j = 0; j < row.Length; j++)
            {
                newItem.transform.GetChild(j).GetComponent<Text>().text = row[j];
            }
        }
    }
}
