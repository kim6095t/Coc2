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
    [SerializeField] UnitButtonManager unitButtonManager;
    [SerializeField] LayerMask tileMask;

    [SerializeField] ResultMenu resultMenu;
    [SerializeField] EndGame endGame;

    [HideInInspector] public int maxUnitCount;


    public delegate void DelUnitEvent();                  // ��������Ʈ ����.
    event DelUnitEvent OnDelUnit;                         // �̺�Ʈ �Լ� ����.

    private Dictionary<Unit_TYPE, UnitData> unitDatas;   // ������ Ÿ�� ������.
    public Dictionary<Unit_TYPE, int> unitCount;         // ������ ��.
    Unit_TYPE selectedType = Unit_TYPE.None;             // ���� ������ Ÿ���� Ÿ��.

    int[] numUnit;
    //���� ��ȯ �ð�
    float callRate = 1;
    float originCallRate;
    float nextCallTime = 0f;

    private void Awake()
    {
        base.Awake();

        // CSV�����͸� �츮�� ���ϴ� �����ͷ� ����.
        unitDatas = new Dictionary<Unit_TYPE, UnitData>();
        unitCount= new Dictionary<Unit_TYPE, int>();
        numUnit = new int[4];

        numUnit[0] = 100;
        numUnit[1] = 2;
        numUnit[2] = 3;
        numUnit[3] = 4;


        Dictionary<string, string>[] csvDatas = CSVReader.ReadCSV(data);
        for (int i = 0; i < csvDatas.Length; i++)
        {
            UnitData newData = new UnitData(csvDatas[i]);
            Unit_TYPE type = (Unit_TYPE)System.Enum.Parse(typeof(Unit_TYPE), newData.GetData(KEY_NAME));
            unitDatas.Add(type, newData);
            unitCount.Add(type, numUnit[i]);

            maxUnitCount += unitCount[type];
        }

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

        //���̻� ������ ���� ��
        if (maxUnitCount <= 0)
        {
            resultMenu.SwitchResultMenu(true);
            endGame.GameResult();
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

        //��ȯ������ ������ ��μ�ȯ ������
        if (unitCount[selectedType]<=0)
            return;

        Unit newUnit = Instantiate(unitPrefabs[(int)selectedType]);
        newUnit.Setup(unitDatas[newUnit.Type]);

        setTile.SetUnit(newUnit);
        unitCount[newUnit.Type] -= 1;
        unitButtonManager.OnCreateUnit(newUnit.Type);

        TextCollect.Instance.OnFalseAllText();
    }

    public UnitData GetData(Unit_TYPE type)
    {
        return unitDatas[type];
    }

    public void OnSelectedUnit(Unit.Unit_TYPE type)
    {
        selectedType = type;
    }

    public void RegestedDelUnit(DelUnitEvent OnDelUnit)
    {
        this.OnDelUnit += OnDelUnit;
    }
    public void RemoveDelUnit(DelUnitEvent OnDelUnit)
    {
        this.OnDelUnit -= OnDelUnit;
    }

    public void OnDelUnitInvoke()
    {
        OnDelUnit?.Invoke();
    }
}