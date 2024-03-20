using UnityEngine;
using System;
using UnityEngine.UI;

public class TournamentDetailsPanel : MonoBehaviour
{
    #region Private_Vars
    [SerializeField]
    private GameObject[] m_TournamentDropDowns;
    [SerializeField]
    private Button m_BackButton;
    #endregion
    #region Public_Vars
    public static Action s_OnBackButtonTap;
    #endregion

    #region Unity_Callbacks
    private void Awake()
    {
        m_BackButton.onClick.AddListener(() =>
        {
            s_OnBackButtonTap?.Invoke();
            ResetPanels();
        });
    }
    private void Start()
    {
        ResetPanels();
        
    }
    #endregion

    #region Private_Methods


    #endregion

    #region Public_Methods
    public void ResetPanels()
    {
        foreach (GameObject panels in m_TournamentDropDowns)
        {
            panels.SetActive(false);
        }
    }
    public void ActivatePanel(int PanelIndex)
    {
        ResetPanels();
        m_TournamentDropDowns[PanelIndex].SetActive(true);
    }

    #endregion





}
