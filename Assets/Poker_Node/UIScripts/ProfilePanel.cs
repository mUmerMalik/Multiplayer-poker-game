using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{

    #region Private_Vars
    [SerializeField]
    private ChangeAvatarPanel m_AvatarPanel;
    [SerializeField]
    private Button m_AvatarButton;
    #endregion

    #region Unity_Callbacks
    private void Awake()
    {
        m_AvatarButton.onClick.AddListener(() =>
        {
            OnProfileButtonTap();
        });
    }

    #endregion

    #region Private_Methods
    private void OnProfileButtonTap()   
    {   
        m_AvatarPanel.gameObject.SetActive(true);
        m_AvatarPanel.SetData((int)UserInfo.instance.PlayerCurrentData.data.userid, (int)UserInfo.instance.PlayerCurrentData.data.photo_index);
    }

    #endregion


}
