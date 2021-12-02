using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfButton : MonoBehaviour
{
    [SerializeField] Image informationButton;
    [SerializeField] Image upgradeButton;

    public void ClickInfBtn()
    {
        Debug.Log("ClickImfButton");
    }

    public void ClickUpgradeBtn()
    {
        Debug.Log("ClickUpgradeBtn");
    }

}
