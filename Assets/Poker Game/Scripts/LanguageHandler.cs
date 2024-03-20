using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LanguageHandler : MonoBehaviour
{
    public List<string> languages;

    void OnEnable()
    {
        PhotonUI._resetData += Resetdata;
        Resetdata();        
    }

    private void OnDisable()
    {
        PhotonUI._resetData -= Resetdata;
    }

    public void Resetdata()
    {
        int index = PlayerPrefs.GetInt("Language", 0);
            //print("--------gameObject-----------" + transform.parent.parent.gameObject.name);
        if (transform.parent.parent.gameObject.name.Contains("TMP"))
        {
            //print("--------TextMeshPro-----------"+ GetComponent<TextMeshPro>());
            if(GetComponent<TextMeshPro>() != null)
                GetComponent<TextMeshPro>().text = languages[index];
        }
        else
        {
            //print("--------Text-----------"+ index);
            GetComponent<Text>().text = languages[index];
        }
    }
}
