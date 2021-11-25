using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] TextAsset data;
    public UnitStruct[] unitData;
    Dictionary<string, string>[] csvDatas;

    private void Awake()
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
            newUnit.countUnit = (int)System.Enum.Parse(typeof(Unit_TYPE), newUnit.newData.GetData(KEY_COUNT));

            unitData[i]= newUnit;
        }
    }

    public UnitData GetData(Unit_TYPE type)
    {
        Debug.Log(1);
        return unitData[(int)type].newData;
    }

    private void OnDestroy()
    {
        sw = new StreamWriter("C:\\Users\\user\\Desktop\\Test.txt", false);
        if (count == 0)
        {
            sw.WriteLine(txtRevisionNext.Text);

            for (int i = 1; i < lines.Length; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();
            MessageBox.Show("�����Ǿ����ϴ�.");
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.WriteLine(txtRevisionNext.Text);
            for (int i = count + 1; i < lines.Length; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();
            MessageBox.Show("�����Ǿ����ϴ�.");
        }
    }

    https://lena19760323.tistory.com/1
}