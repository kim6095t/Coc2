using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


[System.Serializable]
public class Data
{
    [SerializeField] public string m_name;
    [SerializeField] public Vector3 m_vecPositon;


    public string Name => m_name;
    public Vector3 Vec => m_vecPositon;

    
    public Data()
    {

    }
    public Data(string m_name, Vector3 m_vecPosition)
    {
        this.m_name = m_name;
        this.m_vecPositon = m_vecPosition;
    }
}

public class ObjectLocationManager : Singletone<ObjectLocationManager>
{
    [SerializeField] List<Data> data;

    private void Awake()
    {
        base.Awake();
        data = new List<Data>();
    }

    public void DataCreate(GameObject target)
    {
        Data targetData = new Data();
        string name=target.name.Split('(')[0];
        
        targetData.m_name = name;
        targetData.m_vecPositon = target.transform.position;

        data.Add(targetData);

        Debug.Log(targetData.m_name);
        //Debug.Log(targetData.m_vecPositon);
    }


    private void OnDestroy()
    {

        File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data));
    }
}
