using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSizeStar : MonoBehaviour
{
    [SerializeField] Image star;
    [SerializeField] Text text;

    public Vector2 Star
    {
        get
        {
            return star.rectTransform.sizeDelta;
        }
        set
        {
            star.rectTransform.sizeDelta = value;
        }
    }

    public int Label
    {
        get
        {
            return text.fontSize;
        }
        set
        {
            text.fontSize = value;
        }
    }

    public Vector2 StarInterval
    {
        get
        {
            return star.rectTransform.anchoredPosition;
        }
        set
        {
            star.rectTransform.anchoredPosition = value;
        }
    }
    public Vector2 LabelInterval
    {
        get
        {
            return text.rectTransform.anchoredPosition;
        }
        set
        {
            text.rectTransform.anchoredPosition = value;
        }
    }
}
