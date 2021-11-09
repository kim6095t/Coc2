using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}

public class UnitManager : Singletone<UnitManager>
{
    [SerializeField] TextAsset data;
    [SerializeField] Unit[] unitPrefabs;
    [SerializeField] LayerMask tileMask;

    Dictionary<Unit_TYPE, UnitData> unitDatas;           // 가공된 타워 데이터.
    Unit_TYPE selectedType = Unit_TYPE.None;              // 현재 선택한 타워의 타입.


    private void Awake()
    {
        base.Awake();

        // CSV데이터를 우리가 원하는 데이터로 가공.
        unitDatas = new Dictionary<Unit_TYPE, UnitData>();
        Dictionary<string, string>[] csvDatas = CSVReader.ReadCSV(data);
        for (int i = 0; i < csvDatas.Length; i++)
        {
            UnitData newData = new UnitData(csvDatas[i]);
            Debug.Log(csvDatas[i]);
            Unit_TYPE type = (Unit_TYPE)System.Enum.Parse(typeof(Unit_TYPE), newData.GetData(KEY_NAME));
            unitDatas.Add(type, newData);
        }

        foreach(Unit_TYPE key in unitDatas.Keys)
        {
            Debug.Log(key);
        }


    }


    //유닛 소환 시간
    //float callRate = 1;
    //float originCallRate;
    //float nextCallTime = 0f;

    //private void Start()
    //{
    //    originCallRate = callRate;
    //}

    //private void Update()
    //{

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        MousePointToRay();
    //        nextCallTime = Time.time + callRate;
    //    }

    //    if (Input.GetMouseButton(0) && nextCallTime <= Time.time)
    //    {
    //        MousePointToRay();
    //        callRate = Mathf.Clamp(callRate /= 2, 0.01f, originCallRate);
    //        nextCallTime = Time.time + callRate;
    //    }

    //    if (Input.GetMouseButtonUp(0)){
    //        callRate = originCallRate;
    //    }
    //}

    //private void MousePointToRay()
    //{
    //    // 마우스의 현재 위치를 Ray로 변환.
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
    //    {
    //        SetTile setTile = hit.collider.GetComponent<SetTile>();
    //        CreateUnit(setTile);
    //    }
    //}

    //private void CreateUnit(SetTile setTile)
    //{
    //    // 선택한 타일이 없거나 타일에 이미 설치가 되어있는 경우.
    //    if (setTile == null)
    //        return;

    //    Unit newUnit = Instantiate(UnitPrefab);
    //    setTile.SetUnit(newUnit);
    //}

}

