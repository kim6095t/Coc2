using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubClear : MonoBehaviour
{
    [SerializeField] RectTransform[] rectTransform;
    ChangeSizeStar[] changeSizeStar;

    int LabelSize;
    RectTransform rect;
    ChangeSizeStar chanSizeStar;

    bool IsActiveStar;
    public bool isActiveStar => IsActiveStar;

    void Start()
    {
        changeSizeStar = new ChangeSizeStar[rectTransform.Length];

        for (int i = 0; i < rectTransform.Length; i++)
        {
            changeSizeStar[i] = rectTransform[i].gameObject.GetComponent<ChangeSizeStar>();
        }
    }

    public void OnActiveStar()
    {
        IsActiveStar = true;

        for (int i = 0; i < rectTransform.Length; i++)
        {
            if (StageClearPersent.Instance.OnIsActiveStar(i) == false && !rect)
            {
                rect = rectTransform[i];
                chanSizeStar = changeSizeStar[i];

                rect.gameObject.SetActive(true);
                //라벨 비율 공식 =기존라벨의 글자크기 * (도착지 별의크기/원래별의 크기)
                LabelSize = chanSizeStar.Label * (int)(StageClearPersent.Instance.onStarSize().x / chanSizeStar.Star.x);

                StartCoroutine(MoveAndChangeSize());
                IsActiveStar = false;
                break;
            }
        }
    }


    IEnumerator MoveAndChangeSize()
    {
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(rect.position, StageClearPersent.Instance.onStarPosition()) > 5f)
        {
            rect.position
                = Vector3.Lerp(rect.position, StageClearPersent.Instance.onStarPosition(), Time.deltaTime*2f);

            chanSizeStar.Star
                = Vector2.Lerp(chanSizeStar.Star, StageClearPersent.Instance.onStarSize(), Time.deltaTime * 2f);

            chanSizeStar.Label
                = (int)Mathf.Lerp(chanSizeStar.Label, LabelSize, Time.deltaTime * 2f);

            chanSizeStar.StarInterval
                = Vector2.Lerp(chanSizeStar.StarInterval, new Vector2(0,30f), Time.deltaTime * 2f);

            chanSizeStar.LabelInterval
                = Vector2.Lerp(chanSizeStar.LabelInterval, new Vector2(0, -30f), Time.deltaTime * 2f);

            yield return null;
        }

        //오브젝트의 이동이 마무리 되면 해당함수 사용
        rect.gameObject.SetActive(false);
        StageClearPersent.Instance.OnSetActiveStar();

    }
}
