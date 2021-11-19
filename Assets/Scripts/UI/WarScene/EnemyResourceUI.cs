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
        //예외처리
        if (goldText && jellyText && EnemyResourceData.Instance)
        {
            goldText.text = $"{ EnemyResourceData.Instance.enemyGold}";
            jellyText.text = $"{ EnemyResourceData.Instance.enemyJelly}";
        }
    }
}
