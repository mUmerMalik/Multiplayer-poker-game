using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    public int id;
    public Text player_name;
    public Text player_score;
    public Image player_profile;
    public Image player_country;
    public Image timer_fill;

    public List<Transform> card_transforms;
    public List<string> cards;
    public List<Sprite> cards_sprites;
    public Animator label_Animator;
    public GameObject glowObject;
    public GameObject cardsObject;
    public GameObject youObject;
    public GameObject winObject;
    public Transform D_pos;
    //public bool outOfGame;
    //public bool fold;
    public bool stopTimer = true;
    public bool allowCheck;

    public PlayerStates currentState;
    public Text call_text;
    public Vector3 call_text_pos;
    public float prev_call = 0;
    public float balance;
    public int sec_i = 0;
    public int seat = -1;

    // Start is called before the first frame update
    void Start()
    {
        call_text_pos = call_text.transform.position;
    }

    private void OnEnable()
    {
        for (int i = 0; i < card_transforms.Count; i++)
        {
            card_transforms[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < card_transforms.Count; i++)
        {
            card_transforms[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator StartTimer()
    {
        stopTimer = false;
        int sec = 20;
        timer_fill.fillAmount = 1;
        glowObject.SetActive(true);
        timer_fill.transform.parent.gameObject.SetActive(true);
        for (int i = 0; i <= sec; i++)
        {
            sec_i = i;
            if (!stopTimer)
            {
                yield return new WaitForSeconds(1);
                timer_fill.fillAmount = 1 - (float)i / sec;
            }
        }
        glowObject.SetActive(false);
        timer_fill.transform.parent.gameObject.SetActive(false);

        if (!stopTimer && PhotonNetwork.LocalPlayer.player_seat == seat)
        {
            if (GameManager.instance.callBtn.gameObject.activeSelf)
                GameManager.instance.Call();
            else
                GameManager.instance.Check();

        }
    }

    public IEnumerator ContinueTimer(int sec_i)
    {
        sec_i += 2;
        stopTimer = false;
        int sec = 20;
        timer_fill.fillAmount = 1 - (float)sec_i / sec;
        glowObject.SetActive(true);
        timer_fill.transform.parent.gameObject.SetActive(true);
        for (int i = sec_i; i <= sec; i++)
        {
            if (!stopTimer)
            {
                yield return new WaitForSeconds(1);
                timer_fill.fillAmount = 1 - (float)i / sec;
            }
        }
        glowObject.SetActive(false);
        timer_fill.transform.parent.gameObject.SetActive(false);

        if (!stopTimer && PhotonNetwork.LocalPlayer.player_seat == seat)
        {
            if (GameManager.instance.callBtn.gameObject.activeSelf)
            {
                if (GameManager.instance.callBtn.GetComponent<Button>().interactable)
                    GameManager.instance.Call();
                else
                    GameManager.instance.Fold();
            }
            else
                GameManager.instance.Check();

        }
    }

    public void ChangeAction(PlayerActions action)
    {
        stopTimer = true;
        switch (action)
        {
            case PlayerActions.Call:
                print("=======Call=======");
                label_Animator.SetTrigger("Call");
                StartCoroutine(Call());
                break;

            case PlayerActions.Raise:
                print("=======Raise=======");
                label_Animator.SetTrigger("Raise");
                StartCoroutine(Raise());
                break;

            case PlayerActions.Check:
                print("=======Check=======");
                label_Animator.SetTrigger("Check");
                StartCoroutine(Check());
                break;

            case PlayerActions.Fold:
                print("=======Fold=======");
                label_Animator.SetTrigger("Fold");
                StartCoroutine(Fold());
                break;

            case PlayerActions.Bet:
                print("=======Bet=======");
                label_Animator.SetTrigger("Bet");
                StartCoroutine(Bet());
                break;
        }
    }

    IEnumerator Fold()
    {
        GameManager.instance.winnerlogic.players.Remove(this);
        currentState = PlayerStates.Folded;
        cardsObject.transform.DOScaleY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //card_transforms[0].gameObject.SetActive(false);
        //card_transforms[1].gameObject.SetActive(false);
        cardsObject.SetActive(false);
        cardsObject.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1);
        if (GameManager.instance.winnerlogic.players.Count > 1)
        {
                print("--------NextTurn-66------->>>> ");
            GameManager.instance.NextTurn();
        }
        else
        {
            GameManager.instance.EndOfTheGame(0);
            //GameManager.instance.winnerlogic.players[0].Win(GameManager.instance.totalPot_value);
        }
    }

    IEnumerator Call()
    {
        allowCheck = true;
        prev_call = GameManager.instance.call_value - prev_call;
        SoundManager.instance.SoundEffect("ChipsAdd");
        call_text.text = "$" + prev_call.ToString("0.00");
        call_text.gameObject.SetActive(true);
        balance -= prev_call;
        print("--------balance------->>>> " + Mathf.Approximately(balance, 0.0f));
        if (Mathf.Approximately(balance, 0.0f))
        {
            print("--------All-22------->>>> ");
            GameManager.instance.Allin();
            player_score.text = "All-In";
            player_score.color = Color.red;
        }
        else
            player_score.text = "$" + balance.ToString("0.00");
        GameManager.instance.totalPot_value += GameManager.instance.call_value;
        GameManager.instance.totalPot_text.text = "$" + GameManager.instance.totalPot_value.ToString("0.00");
        yield return new WaitForSeconds(0.1f);
                print("--------NextTurn-77------->>>> ");
        GameManager.instance.NextTurn();
    }

    IEnumerator Raise()
    {
        prev_call = GameManager.instance.call_value + GameManager.instance.raised_value;
        SoundManager.instance.SoundEffect("ChipsAdd");
        call_text.text = "$" + prev_call.ToString("0.00");
        call_text.gameObject.SetActive(true);
        balance -= prev_call;
        print("--------balance------->>>> " + (balance - 0.0f));
        if (Mathf.Approximately(balance, 0.0f))
        {
                print("--------All-11------->>>> ");
            GameManager.instance.Allin();
            player_score.text = "All-In";
            player_score.color = Color.red;
        }
        else
            player_score.text = "$" + balance.ToString("0.00");
        GameManager.instance.call_value = prev_call;
        GameManager.instance.totalPot_value += GameManager.instance.call_value;
        GameManager.instance.totalPot_text.text = "$" + GameManager.instance.totalPot_value.ToString("0.00");
        allowCheck = true;
        yield return new WaitForSeconds(0.1f);
                print("--------NextTurn-88------->>>> ");
        GameManager.instance.NextTurn();
    }

    IEnumerator Bet()
    {
        prev_call = GameManager.instance.call_value + GameManager.instance.raised_value;
        GameManager.instance.call_value = prev_call;
        SoundManager.instance.SoundEffect("ChipsAdd");
        call_text.text = "$" + prev_call.ToString("0.00");
        call_text.gameObject.SetActive(true);
        balance -= prev_call;
                print("--------balance------->>>> " + (balance - 0.01f));
        if (Mathf.Approximately(balance, 0.0f))
        {
                print("--------All-22------->>>> ");
            GameManager.instance.Allin();
            player_score.text = "All-In";
            player_score.color = Color.red;
        }
        else
            player_score.text = "$" + balance.ToString("0.00");
        GameManager.instance.totalPot_value += GameManager.instance.call_value;
        GameManager.instance.totalPot_text.text = "$" + GameManager.instance.totalPot_value.ToString("0.00");
        allowCheck = true;
        yield return new WaitForSeconds(0.1f);
                print("--------NextTurn-99------->>>> ");
        GameManager.instance.NextTurn();
    }

    IEnumerator Check()
    {
        allowCheck = true;
        yield return new WaitForSeconds(0.1f);
                print("--------NextTurn-bb------->>>> ");
        GameManager.instance.NextTurn();
    }

    public void Win(float win_amount)
    {
        print("-------Winn-------->>>>>> " + id);
        winObject.SetActive(true);
        winObject.GetComponent<Animator>().Play("win_anim");
        balance += win_amount;
        player_score.text = "$" + balance.ToString("0.00");
    }

    public IEnumerator ShowCards()
    {
        for (int i = 0; i < card_transforms.Count; i++)
        {
            card_transforms[i].DOScaleX(0, 0.2f);
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < card_transforms.Count; i++)
        {
            card_transforms[i].GetChild(0).gameObject.GetComponent<Image>().sprite = cards_sprites[i];
            card_transforms[i].DOScaleX(1, 0.2f);
        }
    }
}
