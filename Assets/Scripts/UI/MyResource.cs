using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyResource : MonoBehaviour
{
    [SerializeField] Text goldText;
    [SerializeField] Text jellyText;
    public Image goldBar;
    public Image jellyBar;

    public void OnUpdateResource()
    {
        Debug.Log("이거 실행");
        Debug.Log(ResourceDateManager.Instance.myGold);
        goldText.text = $"{ ResourceDateManager.Instance.myGold}";
        jellyText.text = $"{ ResourceDateManager.Instance.myJelly}";

        goldBar.fillAmount = ResourceDateManager.Instance.myGold / ResourceDateManager.Instance.maxGold;
        jellyBar.fillAmount = ResourceDateManager.Instance.myJelly / ResourceDateManager.Instance.maxJelly;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DataManager.DeleteAll();
        }
    }
}
