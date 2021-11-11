using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearPersent : Singletone<StageClearPersent>
{
    [SerializeField] Text persent;
    [SerializeField] Image[] stars;

    float nowTargetNum;
    float allTargetNum;
    float nowClearPersent;

    void Start()
    {
        persent.text = "0%";
        GameObject[] target = GameObject.FindGameObjectsWithTag("Playable");
        nowTargetNum = target.Length;
        allTargetNum = target.Length;
    }

    public void OnDestroyTarget()
    {
        nowTargetNum--;
        RenewPersent();
    }

    private void RenewPersent()
    {
        nowClearPersent = ((allTargetNum - nowTargetNum) / allTargetNum) * 100;
        persent.text = $"{(int)(((allTargetNum-nowTargetNum)/allTargetNum) * 100)}%";

        if (nowClearPersent > 30)
        {
            stars[0].color = new Color(255,255,255,255);
            Debug.Log("º°ÇÏ³ª");
        }
    }
    
}
