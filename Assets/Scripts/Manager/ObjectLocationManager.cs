using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public string m_name;
    public Vector3 m_vecPositon;
}

public class ObjectLocationManager : Singletone<ObjectLocationManager>
{
    List<Data> data;

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
        Debug.Log(data.Count);   
    }
}
