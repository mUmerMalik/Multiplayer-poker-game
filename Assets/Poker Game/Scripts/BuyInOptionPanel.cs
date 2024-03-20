using System.Collections;
using System.Collections.Generic;
using PlayFab.MultiplayerModels;
using UnityEngine;
using UnityEngine.UI;

public class BuyInOptionPanel : MonoBehaviour
{
    public Text min_max_value;
    public Text buyinMin;
    public Text buyinMax;
    public Text buyinAmount;
    public Slider buyinAmountSlider;
    public Text tableText;

    public Text timerText;
    public Button buyInOk;

    private void Start()
    {
        tableText.text = "$" + PlayerPrefs.GetFloat("MIN_VAL", 0) + " / $" + PlayerPrefs.GetFloat("MAX_VAL", 0);
        min_max_value.text = "$" + PlayerPrefs.GetFloat("MIN_VAL", 0) + " / $" + PlayerPrefs.GetFloat("MAX_VAL", 0);
        buyinAmountSlider.minValue = PlayerPrefs.GetFloat("BUYIN_MIN_VAL", 0);
        if (PlayerPrefs.GetFloat("BUYIN_MAX_VAL", 0) > PlayerPrefs.GetFloat("Balance", 0))
            buyinAmountSlider.maxValue = PlayerPrefs.GetFloat("Balance", 0);
        else
            buyinAmountSlider.maxValue = PlayerPrefs.GetFloat("BUYIN_MAX_VAL", 0);
        buyinAmountSlider.value = PlayerPrefs.GetFloat("BUYIN_MIN_VAL", 0);
        buyinMin.text = "$" + buyinAmountSlider.minValue.ToString("0.00");
        buyinMax.text = "$" + buyinAmountSlider.maxValue.ToString("0.00");
        buyinAmount.text = "$" + buyinAmountSlider.value.ToString("0.00");        
    }

    private void OnEnable()
    {                           
        buyInOk.interactable = true;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        int sec = 10;

        for(int i = sec; i > 0; i--)
        {
            timerText.text = i + "s";
            yield return new WaitForSeconds(1);
        }

        gameObject.SetActive(false);
    }

    public void SetBuyInAmount()
    {
        buyinAmount.text = "$" + buyinAmountSlider.value.ToString("0.00");
    }

    public void Buyin_Ok()
    {
        //buyInOk.interactable = false;
       //sksoundchnages SoundManager.instance.SoundEffect("BtnClick");
        GameManager.instance.current_player_balance = buyinAmountSlider.value;
        PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance", 0) - GameManager.instance.current_player_balance);
        StartCoroutine(GameManager.instance._start(gameObject));
        //gameObject.SetActive(false);
        //
        object[] content = new object[] { GameManager.instance.selectedSeat };
        MultiplayerManager.instance.RaiseEventMethod(ConstEvents.SELECT_SEAT, content);
    }

    public void Buyin_Cancel()
    {
        gameObject.SetActive(false);
        SoundManager.instance.SoundEffect("BtnClick");
    }
}
