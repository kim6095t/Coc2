using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] Text gold;
    [SerializeField] Text jelly;

    private void Start()
    {
        gold.text = $"{ResourceDateManager.Instance.getGold}";
        jelly.text = $"{ResourceDateManager.Instance.getJelly}";
    }
}
