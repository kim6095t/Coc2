using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unit;     // Unit 클래스의 영역을 포함하겠다.


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

    Dictionary<Unit_TYPE, UnitData> unitDatas;           // 가공된 타워 데이터.
    Unit_TYPE selectedType = Unit_TYPE.None;             // 현재 선택한 타워의 타입.


    private void Awake()
    {
        base.Awake();

        // CSV데이터를 우리가 원하는 데이터로 가공.
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


    //유닛 소환 시간
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
        // 마우스의 현재 위치를 Ray로 변환.
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
        // 유닛을 선택하지 않고 소환할 시 화면에 에러메세지 출력
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


    //해야할 것: 유닛 클릭 버튼을 눌렀을 때 유닛은 선택이 되지 않게
    //방법1. bool변수를 이용한다.
    //OnSelectedUnit
    //선택한 영역이 검은 박스 영역일때는 유닛생성 불가능

    public void OnSelectedUnit(Unit.Unit_TYPE type)
    {
        selectedType = type;
    }
}