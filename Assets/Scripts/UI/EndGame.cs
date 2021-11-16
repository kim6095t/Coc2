using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] Text gold;
    [SerializeField] Text jelly;
    [SerializeField] Text score;
    [SerializeField] Image[] stars;

    public void GameResult()
    {
        int activeStar = 0;
        for (int i = 0; i < stars.Length; i++) {
            
            if (StageClearPersent.Instance.ActiveStar[i])
            {
                activeStar++;
            }
        }
        StartCoroutine(CountStar(activeStar));

        StartCoroutine(Count(StageClearPersent.Instance.NowClearPersent, score));
        StartCoroutine(Count(ResourceDateManager.Instance.getGold, gold));
        StartCoroutine(Count(ResourceDateManager.Instance.getJelly, jelly));
    }

    IEnumerator CountStar(int target)
    {
        float current = 0;
        float duration = 3f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            stars[(int)offset].color = new Color(255, 255, 255, 255);

            yield return null;

        }
        current = target;
        stars[(int)offset].color = new Color(255, 255, 255, 255);
    }

    IEnumerator Count(float target,  Text Label)
    {
        float current = 0;
        float duration = 3f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)

        {
            current += offset * Time.deltaTime;
            if(Label==score)
                Label.text = $"{(int)current}%";
            else
                Label.text = ((int)current).ToString();
            yield return null;

        }
        current = target;
        if (Label == score)
            Label.text = $"{(int)current}%";
        else
            Label.text = ((int)current).ToString();
    }
}
