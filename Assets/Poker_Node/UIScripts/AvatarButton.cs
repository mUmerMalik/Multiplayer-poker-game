using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarButton : MonoBehaviour
{
    #region Private_Vars
    [SerializeField]
    private int m_AvatarId;
    [SerializeField]
    private bool m_IsActivated;
    [SerializeField]
    private Sprite m_Enabled;
    [SerializeField]
    private Sprite m_Disabled;
    #endregion

    #region Unity_Callbacks
    private void OnEnable()
    {
        ChangeAvatarPanel.s_OnAvatarChangesCallback += OnAvatarChanges;
        ChangeAvatarPanel.s_OnActivateCurrentAvatar += OnAvatarChanges;
    }
    private void OnDisable()
    {
        ChangeAvatarPanel.s_OnAvatarChangesCallback -= OnAvatarChanges;
        ChangeAvatarPanel.s_OnActivateCurrentAvatar -= OnAvatarChanges;
    }
    #endregion

    #region Private_Methods
    private void OnAvatarChanges(int avatarId)
    {
        if(m_AvatarId == avatarId)
        {
            GetComponent<Image>().sprite = m_Enabled;
            m_IsActivated = true;
        }
        else if(m_IsActivated)
        {
            GetComponent<Image>().sprite = m_Disabled;
            m_IsActivated = false;
        }
    }

    #endregion

}
