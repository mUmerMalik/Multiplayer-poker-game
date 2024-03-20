using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentPanel : MonoBehaviour
{
    #region Private_Vars
    [SerializeField]
    private GameObject m_TournamentDetailsScreen;
    #endregion
        
    #region Unity_callbacks
    private void OnEnable()
    {
        ResetTournament();
        TournamentListingItem.s_OnTournamentButtonTap += OnTournamentCallbackReceived;
        TournamentDetailsPanel.s_OnBackButtonTap += ResetTournament;
    }

    private void OnDisable()
    {
        TournamentListingItem.s_OnTournamentButtonTap -= OnTournamentCallbackReceived;
        TournamentDetailsPanel.s_OnBackButtonTap -= ResetTournament;
    }
    #endregion

    #region Private_Methods
    private void OnTournamentCallbackReceived()
    {
        m_TournamentDetailsScreen.SetActive(true);
    }
    #endregion

    #region Public_Methods
    public void ResetTournament()
    {
        m_TournamentDetailsScreen.SetActive(false);
    }

    

    #endregion




}
