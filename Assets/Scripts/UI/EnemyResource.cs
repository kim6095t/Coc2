using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyResource : MonoBehaviour
{
    [SerializeField] Text goldText;
    [SerializeField] Text jellyText;

    public void OnUpdateResource()
    {
        goldText.text = $"{ ResourceDateManager.Instance.enemyGold}";
        jellyText.text = $"{ ResourceDateManager.Instance.enemyJelly}";
    }
}
