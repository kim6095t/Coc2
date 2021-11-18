using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCollect : Singletone<TextCollect>
{
    [SerializeField] Text startText;
    [SerializeField] Text[] errorText;


    // Start is called before the first frame update
    void Start()
    {
        startText.gameObject.SetActive(true);
    }

    public void OnNotSelectedUnit()
    {
        for (int i = 0; i < errorText.Length; i++)
        {
            if (errorText[i].gameObject.activeSelf == false)
            {
                errorText[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void OnFalseAllText()
    {
        startText.gameObject.SetActive(false);

        for (int i = 0; i < errorText.Length; i++)
            errorText[i].gameObject.SetActive(false);
    }
}