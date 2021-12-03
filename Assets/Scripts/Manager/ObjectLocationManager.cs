using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Data
{
    public string m_name;
    public int m_level;
    public Vector3 m_vecPositon;
}

public class ArrayJson<Data>
{
    public Data[] datas;
    public ArrayJson(Data[] datas)
    {
        this.datas = datas;
    }
}

public class ObjectLocationManager : Singletone<ObjectLocationManager>
{
    List<Data> data;

    private void Awake()
    {
        base.Awake();

        data = new List<Data>();
        string loadData = File.ReadAllText(Application.dataPath + "/ObjectLocationJson.json");
        ArrayJson<Data> json = JsonUtility.FromJson<ArrayJson<Data>>(loadData);
        

        for(int i=0; i < json.datas.Length; i++)
        {
            GameObject prefab= (GameObject)AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Resources/BuildObject/" +
                $"{json.datas[i].m_name}.prefab", typeof(GameObject));
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.position = json.datas[i].m_vecPositon;
        }
    }

    public void DataSave(GameObject target)
    {
        ObjectProperty targetLevel = target.GetComponent<ObjectProperty>();

        Data targetData = new Data();
        string name=target.name.Split('(')[0];
        int level = targetLevel.nowLevel;

        targetData.m_name = name;
        targetData.m_level = level;
        targetData.m_vecPositon = target.transform.position;


        data.Add(targetData);
    }

    private void OnApplicationQuit()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Playable");
        for(int i=0; i<obj.Length; i++)
            DataSave(obj[i]);

        ArrayJson<Data> saveData = new ArrayJson<Data>(data.ToArray());
        File.WriteAllText(Application.dataPath + "/ObjectLocationJson.json", JsonUtility.ToJson(saveData));
    }
}