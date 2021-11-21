using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyResourceData : Singletone<MyResourceData>
{
    [SerializeField] MyResourceUI myResource;

    [HideInInspector] public float maxGold;
    [HideInInspector] public float maxJelly;
    [HideInInspector] public float myGold;
    [HideInInspector] public float myJelly;



    private bool isSaved;                 // 저장 여부.
    private const string MAXGOLD_NUM = "maxgold";
    private const string MAXJELLY_NUM = "maxjelly";
    private const string GOLD_NUM = "gold";
    private const string JELLY_NUM = "jelly";
    private const string DATA = "Data";


    public void Awake()
    {
        base.Awake();

        DataManager.OnSave += OnSave;
        DataManager.OnLoad += OnLoad;
        DataManager.OnInit += OnInit;

        OnLoad();
        // 최초에 실행이 되었을 때.
        if (isSaved == false)
        {
            OnInit();
        }
    }

    public void GetGoldToMine(int amount)
    {
        myGold += amount;
        myResource.OnUpdateResource();
    }

    public void UseGoldToMine(int amount)
    {
        if (amount > myGold)
        {
            Debug.Log("사용가능 골드 초과");
            return;
        }

        myGold -= amount;
        myResource.OnUpdateResource();
    }

    public void GetJellyToMine(int amount)
    {
        myJelly += amount;
        myResource.OnUpdateResource();
    }

    void OnInit()
    {
        maxGold = 10000f;
        maxJelly = 10000f;
        myResource.goldBar.fillAmount = 0f;
        myResource.jellyBar.fillAmount = 0f;
    }

    public void OnSave()
    {
        Debug.Log("Data Saved!!");
        DataManager.SetFloat(MAXGOLD_NUM, maxGold);
        DataManager.SetFloat(MAXJELLY_NUM, maxJelly);
        DataManager.SetFloat(GOLD_NUM, myGold);
        DataManager.SetFloat(JELLY_NUM, myJelly);

        DataManager.SetBool(DATA, true);
    }

    public void OnLoad()
    {
        maxGold = DataManager.GetFloat(MAXGOLD_NUM);
        maxJelly = DataManager.GetFloat(MAXJELLY_NUM);
        myGold = DataManager.GetFloat(GOLD_NUM);
        myJelly = DataManager.GetFloat(JELLY_NUM);

        isSaved = DataManager.GetBool(DATA);
    }
    private void OnDestroy()
    {
        OnSave();
    }
}
