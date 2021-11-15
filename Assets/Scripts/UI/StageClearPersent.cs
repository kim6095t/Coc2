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
    bool[] activeStar;


    void Start()
    {
        persent.text = "0%";
        GameObject[] target = GameObject.FindGameObjectsWithTag("Playable");
        nowTargetNum = target.Length;
        allTargetNum = target.Length;

        //���� �����ִ��� �ƴ��� �Ǵ��ϱ� ����
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
        persent.text = $"{(int)(((allTargetNum-nowTargetNum)/allTargetNum) * 100)}%";

        if (nowClearPersent > 33 && activeStar[0]==false)
        {
            activeStar[0] = true;
            SubClear.Instance.OnActiveStar();
        }
        if (nowClearPersent > 66 && activeStar[1] == false)
        {
            activeStar[1] = true;
            SubClear.Instance.OnActiveStar();
        }
        if (nowClearPersent >= 100 && activeStar[2] == false)
        {
            activeStar[2] = true;
            SubClear.Instance.OnActiveStar();
        }
    }

    //����â �� �ҵ����� �ϴ� �Լ�
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


    //����â ���� ��ġ ��ȯ
    public Vector3 onStarPosition()
    {
        for(int i=0; i < stars.Length; i++)
        {
            if (stars[i].color != new Color(255, 255, 255, 255))
                return stars[i].rectTransform.position;
        }
        return Vector3.zero;
    }

    //����â ���� ũ�� ��ȯ
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
