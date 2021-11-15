using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    [SerializeField] GameObject endGameUI;
    [SerializeField] GameObject bottomBarUI;


    public void ClickEndButton()
    {
        endGameUI.SetActive(true);
        bottomBarUI.SetActive(false);
    }

    public void ClickReturnButton()
    {

        endGameUI.SetActive(false);
        bottomBarUI.SetActive(true);
    }
}
