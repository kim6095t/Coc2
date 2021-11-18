using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyResourceUI : MonoBehaviour
{
    [SerializeField] Text goldText;
    [SerializeField] Text jellyText;

    public void OnUpdateResource()
    {
        //����ó��
        if (goldText && jellyText && ResourceDateManager.Instance)
        {
            goldText.text = $"{ ResourceDateManager.Instance.enemyGold}";
            jellyText.text = $"{ ResourceDateManager.Instance.enemyJelly}";
        }
    }
}
