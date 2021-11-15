using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDateManager : Singletone<ResourceDateManager>
{
    [SerializeField] EnemyResource enemyResource;
    [SerializeField] MyResource myResource;

    public delegate void ResourceDataEvent();         // ��������Ʈ ����.
    event ResourceDataEvent DlResourceData;           // �̺�Ʈ �Լ� ����.

    public float enemyGold;
    public float enemyJelly;

    public float maxGold;
    public float maxJelly;

    public float myGold;
    public float myJelly;

    public float getGold;
    public float getJelly;


    private bool isSaved;                 // ���� ����.
    private const string MAXGOLD_NUM = "maxgold";
    private const string MAXJELLY_NUM = "maxjelly";
    private const string GOLD_NUM = "gold";
    private const string JELLY_NUM = "jelly";
    private const string DATA = "Data";

    public void Start()
    {
        DataManager.OnSave += OnSave;
        DataManager.OnLoad += OnLoad;
        DataManager.OnInit += OnInit;

        OnLoad();
        // ���ʿ� ������ �Ǿ��� ��.
        if (isSaved == false)
        {
            OnInit();
        }
    }

    public void RegestedResource(ResourceDataEvent DlResourceData)
    {
        this.DlResourceData += DlResourceData;
        DlResourceData?.Invoke();

        enemyResource.OnUpdateResource();
        myResource.OnUpdateResource();
    }
    public void RemoveResource(ResourceDataEvent DlResourceData)
    {
        this.DlResourceData -= DlResourceData;
        DlResourceData?.Invoke();

        enemyResource.OnUpdateResource();
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
