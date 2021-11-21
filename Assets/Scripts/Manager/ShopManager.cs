using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] TextAsset data;
    [SerializeField] ShopSlotUI slotUIPrefab;
    [SerializeField] Transform parentslotUI;

    ShopSlotUI slotUI;
    private void Start()
    {
        //아이템 슬롯 생성
        Dictionary<string, string>[] csvDatas = CSVReader.ReadCSV(data);
        for(int i=0; i<csvDatas.Length; i++)
        {
            slotUI=Instantiate(slotUIPrefab);
            slotUI.price.text=csvDatas[i]["Price"];
            slotUI.image.sprite= Resources.Load<Sprite>($"ShopSprite/{csvDatas[i]["Name"]}");

            slotUI.transform.parent = parentslotUI.transform;
        }
    }
}
