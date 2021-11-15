using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearPersent : Singletone<StageClearPersent>
{
    [SerializeField] Text persent;
    [SerializeField] Image[] stars;
    [SerializeField] SubClear subclear;

    float nowTargetNum;
    float allTargetNum;
    float nowClearPersent;
    bool[] activeStar;

    public string Persent => persent.text;
    public bool[] ActiveStar => activeStar;
    public int NowClearPersent => (int)nowClearPersent;

    void Start()
    {
        persent.text = "0%";
        GameObject[] target = GameObject.FindGameObjectsWithTag("Playable");
        nowTargetNum = target.Length;
        allTargetNum = target.Length;

        //별이 켜저있는지 아닌지 판단하기 위함
        activeStar = new bool[stars.Length];
    }


    public void OnDestroyTarget()
    {
        nowTargetNum--;
        RenewPersent();
    }

    private void RenewPersent()
    {
        nowClearPersent = ((allTargetNum - nowTargetNum) / allTargetNum) * 100;
        persent.text = $"{(int)nowClearPersent}%";


        if (nowClearPersent > 33 && activeStar[0]==false)
        {
            subclear.OnActiveStar();
            activeStar[0] = true;
        }
        else if (!subclear.isActiveStar && activeStar[1] == false && nowClearPersent > 66 )
        {
            subclear.OnActiveStar();
            activeStar[1] = true;
        }
        else if (!subclear.isActiveStar && activeStar[2] == false && nowClearPersent >= 100 )
        {
            subclear.OnActiveStar();
            activeStar[2] = true;
        }
    }

    //진행창 별 불들어오게 하는 함수
    public void OnSetActiveStar()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i].color != new Color(255, 255, 255, 255))
            {
                stars[i].color = new Color(255, 255, 255, 255);
                break;
            }
        }
    }
    public bool OnIsActiveStar(int num)
    {
        if (stars[num].color != new Color(255, 255, 255, 255))
            return false;
        else
            return true;
    }


    //진행창 별의 위치 반환
    public Vector3 onStarPosition()
    {
        for(int i=0; i < stars.Length; i++)
        {
            if (stars[i].color != new Color(255, 255, 255, 255))
                return stars[i].rectTransform.position;
        }
        return Vector3.zero;
    }

    //진행창 별의 크기 반환
    public Vector3 onStarSize()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i].color != new Color(255, 255, 255, 255))
                return stars[i].rectTransform.sizeDelta;
        }
        return Vector3.zero;
    }
}
