using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyResourceUI : MonoBehaviour
{
    [SerializeField] Text goldText;
    [SerializeField] Text jellyText;
    public Image goldBar;
    public Image jellyBar;

    private void Start()
    {
        OnUpdateResource();
    }

    public void OnUpdateResource()
    {
        if (MyResourceData.Instance && goldText && jellyText)
        //예외처리
        {
            goldText.text = $"{ MyResourceData.Instance.myGold}";
            jellyText.text = $"{ MyResourceData.Instance.myJelly}";

            goldBar.fillAmount = MyResourceData.Instance.myGold / MyResourceData.Instance.maxGold;
            jellyBar.fillAmount = MyResourceData.Instance.myJelly / MyResourceData.Instance.maxJelly;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DataManager.DeleteAll();
        }
    }
}
