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
        if (ResourceDateManager.Instance)
        //예외처리
        {
            goldText.text = $"{ ResourceDateManager.Instance.myGold}";
            jellyText.text = $"{ ResourceDateManager.Instance.myJelly}";

            goldBar.fillAmount = ResourceDateManager.Instance.myGold / ResourceDateManager.Instance.maxGold;
            jellyBar.fillAmount = ResourceDateManager.Instance.myJelly / ResourceDateManager.Instance.maxJelly;
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
