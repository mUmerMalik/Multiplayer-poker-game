using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSizer : MonoBehaviour
{
    public float singleHeight;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transform.childCount * singleHeight);
    }
}
