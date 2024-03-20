using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TournamentListingItem : MonoBehaviour
{
    #region Private_Button
    private Button m_TournamentButton;
    #endregion
    #region Public_Vars
    public static Action s_OnTournamentButtonTap;
    #endregion

    #region Unity_Callbacks
    private void Awake()
    {
        m_TournamentButton = GetComponent<Button>();
        m_TournamentButton.onClick.AddListener(() =>
        {
            s_OnTournamentButtonTap?.Invoke();
        });
    }
    #endregion


}
