using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageHandlerInOtherScene : MonoBehaviour
{
    public List<string> languages;

    void OnEnable()
    {
        Resetdata();
    }

    public void Resetdata()
    {
        int index = PlayerPrefs.GetInt("Language", 0);
        GetComponent<Text>().text = languages[index];
    }
}
