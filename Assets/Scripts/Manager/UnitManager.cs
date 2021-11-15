using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unit;     // Unit Ŭ������ ������ �����ϰڴ�.


public class UnitData
{
    private Dictionary<string, string> data;

    public UnitData(Dictionary<string, string> data)
    {
        this.data = data;
    }

    public string GetData(string key)
    {
        return data[key];
    }

    public override string ToString()
    {
        string text = string.Empty;

        foreach (string key in data.Keys)
        {
            text += string.Format("key:{0}, value:{1}", key, data[key]);
            text += "\n";
        }

        return text;
    }
}

public class UnitManager : Singletone<UnitManager>
{
    [SerializeField] TextAsset data;
    [SerializeField] Unit[] unitPrefabs;
    [SerializeField] LayerMask tileMask;

    Dictionary<Unit_TYPE, UnitData> unitDatas;           // ������ Ÿ�� ������.
    Unit_TYPE selectedType = Unit_TYPE.None;             // ���� ������ Ÿ���� Ÿ��.


    private void Awake()
    {
        base.Awake();

        // CSV�����͸� �츮�� ���ϴ� �����ͷ� ����.
        unitDatas = new Dictionary<Unit_TYPE, UnitData>();
        Dictionary<string, string>[] csvDatas = CSVReader.ReadCSV(data);
        for (int i = 0; i < csvDatas.Length; i++)
        {
            UnitData newData = new UnitData(csvDatas[i]);
            Unit_TYPE type = (Unit_TYPE)System.Enum.Parse(typeof(Unit_TYPE), newData.GetData(KEY_NAME));
            unitDatas.Add(type, newData);
        }

        foreach(Unit_TYPE key in unitDatas.Keys)
        {
            Debug.Log(unitDatas[key]);
        }
    }


    //���� ��ȯ �ð�
    float callRate = 1;
    float originCallRate;
    float nextCallTime = 0f;

    private void Start()
    {
        originCallRate = callRate;
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0))
            {
                MousePointToRay();
                nextCallTime = Time.time + callRate;
            }
            if (Input.GetMouseButton(0) && nextCallTime <= Time.time)
            {
                MousePointToRay();
                callRate = Mathf.Clamp(callRate /= 2, 0.01f, originCallRate);
                nextCallTime = Time.time + callRate;
            }
            if (Input.GetMouseButtonUp(0))
            {
                callRate = originCallRate;
            }
        }
    }

    private void MousePointToRay()
    {
        // ���콺�� ���� ��ġ�� Ray�� ��ȯ.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
        {
            SetTile setTile = hit.collider.GetComponent<SetTile>();
            CreateUnit(setTile);
        }
    }

    private void CreateUnit(SetTile setTile )
    {
        // ������ �������� �ʰ� ��ȯ�� �� ȭ�鿡 �����޼��� ���
        if (selectedType == Unit_TYPE.None)
        {
            TextCollect.Instance.OnNotSelectedUnit();
            return;
        }

        if (setTile == null || setTile.IsOnObject)
            return;

        Unit newUnit = Instantiate(unitPrefabs[(int)selectedType]);
        newUnit.Setup(unitDatas[newUnit.Type]);

        setTile.SetUnit(newUnit);
        TextCollect.Instance.OnFalseAllText();
    }

    public UnitData GetData(Unit_TYPE type)
    {
        return unitDatas[type];
    }


    //�ؾ��� ��: ���� Ŭ�� ��ư�� ������ �� ������ ������ ���� �ʰ�
    //���1. bool������ �̿��Ѵ�.
    //OnSelectedUnit
    //������ ������ ���� �ڽ� �����϶��� ���ֻ��� �Ұ���

    public void OnSelectedUnit(Unit.Unit_TYPE type)
    {
        selectedType = type;
    }
}