using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgMusic;
    public AudioSource dealerVoice;
    public AudioSource soundEffect;

    public AudioClip callClip;
    public AudioClip raiseClip;
    public AudioClip foldClip;
    public AudioClip checkClip;
    public AudioClip betClip;

    public AudioClip btnClick;
    public AudioClip cardOnTable;
    public AudioClip cardDistribute;
    public AudioClip chipsAdd;
    public AudioClip chipsInPot;
    public AudioClip win;
    public AudioClip loose;

    private void Awake()
    {
        instance = this;
    }

    public void BgMusic()
    {
        bgMusic.volume = PlayerPrefs.GetFloat("BGMusic", 0.5f);
        bgMusic.Play();
    }

    public void DealerVoice(string voice)
    {
        dealerVoice.volume = PlayerPrefs.GetFloat("DealerVoice", 0.5f);

        switch (voice)
        {
            case "Call":
                dealerVoice.clip = callClip;
                break;

            case "Raise":
                dealerVoice.clip = raiseClip;
                break;

            case "Fold":
                dealerVoice.clip = foldClip;
                break;

            case "Check":
                dealerVoice.clip = checkClip;
                break;

            case "Bet":
                dealerVoice.clip = betClip;
                break;
        }
        dealerVoice.Play();
    }

    public void SoundEffect(string effect)
    {
        print("----------SoundEffect------------" + effect);
        soundEffect.volume = PlayerPrefs.GetFloat("SoundEffect", 0.5f);

        switch (effect)
        {
            case "BtnClick":
                soundEffect.clip = btnClick;
                break;

            case "CardOnTable":
                soundEffect.clip = cardOnTable;
                break;

            case "CardDistribute":
                soundEffect.clip = cardDistribute;
                break;

            case "ChipsAdd":
                soundEffect.clip = chipsAdd;
                break;

            case "ChipsInPot":
                soundEffect.clip = chipsInPot;
                break;

            case "Win":
                soundEffect.clip = win;
                break;

            case "Loose":
                soundEffect.clip = loose;
                break;
        }
        soundEffect.Play();
    }
}
