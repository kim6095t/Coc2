using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceListUI : MonoBehaviour
{
    [SerializeField] Text gold;
    [SerializeField] Text jelly;

    void Start()
    {
        ReloadResourceUI();
    }

    public void ReloadResourceUI()
    {
        gold.text = $"{ MyResourceData.Instance.myGold}";
        jelly.text = $"{ MyResourceData.Instance.myJelly}";
    }
}
