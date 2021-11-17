using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] GameObject endGameUI;
    [SerializeField] GameObject bottomBarUI;


    public void SwitchResultMenu(bool isShow)
    {
        endGameUI.SetActive(isShow);
        bottomBarUI.SetActive(!isShow);
        if(isShow)
            UnitManager.Instance.OnDelUnitInvoke();
    }
}
