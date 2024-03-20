using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOadingPanel : MonoBehaviour
{
    public Transform image;
    public bool disable;
    public float sec = 2;

    private void OnEnable()
    {
        if(disable)
            Invoke("DisablePanel", sec);
    }

    void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(image != null)
            image.Rotate(0, 0, -2);
    }
}
