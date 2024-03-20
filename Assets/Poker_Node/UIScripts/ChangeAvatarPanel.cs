using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeAvatarPanel : MonoBehaviour
{
    #region Private_Vars
    [SerializeField]
    private Button m_SubmitButton;
    [SerializeField]
    private int avatarId;
    [SerializeField]
    private int userId;
    #endregion

    #region Public_Vars
    public static Action<int> s_OnAvatarChangesCallback;
    public static Action<int> s_OnActivateCurrentAvatar;
    #endregion

    #region Unity_Callbacks
    private void Awake()
    {
        m_SubmitButton.onClick.AddListener(() =>
        {
            ServerManager.instance.OnChangeUserAvatar(userId, avatarId);

        });
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        s_OnActivateCurrentAvatar?.Invoke(avatarId);
    }
    private void OnEnable()
    {
        
       
    }
    #endregion
    #region Public_Methods
    public void SetData(int userId, int avatarId)
    {
        this.userId = userId;
        this.avatarId = avatarId;
    }

    #endregion

    #region Private_Methods
    public void OnAvatarChanges(int avatarId)
    {

        this.avatarId = avatarId;
        s_OnAvatarChangesCallback?.Invoke(this.avatarId);
    }

    #endregion

}
