using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPopUpPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("OffPanel", 2);
    }

    void OffPanel()
    {
        gameObject.SetActive(false);
    }
}
