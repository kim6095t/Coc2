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
public struct UnitStruct
{
    public UnitData newData;
    public Unit_TYPE type;
    public int countUnit;
}

public class UnitManager : Singletone<UnitManager>
{
    [SerializeField] protected TextAsset data;
    public UnitStruct[] unitData;
    protected Dictionary<string, string>[] csvDatas;

    protected void Awake()
    {
        base.Awake();

        unitData = new UnitStruct[(int)Unit.Unit_TYPE.Count];

        // CSV�����͸� �츮�� ���ϴ� �����ͷ� ����.
        csvDatas = CSVReader.ReadCSV(data);

        //����ó��
        if (csvDatas == null)
            return;

        for (int i = 0; i < csvDatas.Length; i++)
        {
            UnitStruct newUnit;
            newUnit.newData = new UnitData(csvDatas[i]);
            newUnit.type = (Unit_TYPE)System.Enum.Parse(typeof(Unit_TYPE), newUnit.newData.GetData(KEY_NAME));
            newUnit.countUnit = UnitCount.numUnit[newUnit.type];

            unitData[i]= newUnit;
        }
    }

    public UnitData GetData(Unit_TYPE type)
    {
        return unitData[(int)type].newData;
    }
}