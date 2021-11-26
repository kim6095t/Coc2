using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unit;     // Unit 클래스의 영역을 포함하겠다.


[System.Serializable]
public class UnitCountData
{
    public string m_name;
    public int m_count;
}

public class UnitCountJson<UnitCountData>
{
    public UnitCountData[] datas;
    public UnitCountJson(UnitCountData[] datas)
    {
        this.datas = datas;
    }
}


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
public struct UnitStruct
{
    public UnitData newData;
    public Unit_TYPE type;
    public int countUnit;
}

public class UnitManager : Singletone<UnitManager>
{
    [SerializeField] TextAsset data;
    public UnitStruct[] unitData;
    
    Dictionary<string, string>[] csvDatas;
    List<UnitCountData> unitCountdata = new List<UnitCountData>();

    private void Awake()
    {
        base.Awake();
        unitData = new UnitStruct[(int)Unit.Unit_TYPE.Count];
        // CSV데이터를 우리가 원하는 데이터로 가공.
        csvDatas = CSVReader.ReadCSV(data);

        //예외처리
        if (csvDatas == null)
            return;

        string loadData = File.ReadAllText(Application.dataPath + "/UnitCountJson.json");
        UnitCountJson<UnitCountData> unitCountdata = JsonUtility.FromJson<UnitCountJson<UnitCountData>>(loadData);
        Debug.Log(unitCountdata);

        for (int i = 0; i < csvDatas.Length; i++)
        {
            UnitStruct newUnit;
            newUnit.newData = new UnitData(csvDatas[i]);
            newUnit.type = (Unit_TYPE)System.Enum.Parse(typeof(Unit_TYPE), newUnit.newData.GetData(KEY_NAME));
            newUnit.countUnit = 5;

            //newUnit.countUnit = unitCountdata[i].m_count;

            unitData[i]= newUnit;
        }
    }

    public UnitData GetData(Unit_TYPE type)
    {
        return unitData[(int)type].newData;
    }


    public void DataSave(UnitStruct target)
    {
        UnitCountData targetData = new UnitCountData();

        targetData.m_count = target.countUnit;
        targetData.m_name = target.type.ToString();
        unitCountdata.Add(targetData);
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < unitData.Length; i++)
            DataSave(unitData[i]);


        UnitCountJson<UnitCountData> saveData = new UnitCountJson<UnitCountData>(unitCountdata.ToArray());
        File.WriteAllText(Application.dataPath + "/UnitCountJson.json", JsonUtility.ToJson(saveData));
    }
}