using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class Data
{
    public int m_nLevel;
    public Vector3 m_vecPositon;

    public void printData()
    {
        Debug.Log("Level : " + m_nLevel);
        Debug.Log("Position : " + m_vecPositon);
    }
}

public class JsonExample : MonoBehaviour
{
    void Start()
    {
        //1. 데이터 생성 및 데이터 삽입
        Data data = new Data();
        data.m_nLevel = 12;
        data.m_vecPositon = new Vector3(3.4f, 5.6f, 7.8f);



        //2. 오브젝트 데이터-> Json형식
        string str = JsonUtility.ToJson(data);
        Debug.Log("ToJson : " + str);

        //2-1 파일 save
        Data data3 = new Data();
        data3.m_nLevel = 99;
        data3.m_vecPositon = new Vector3(8.1f, 9.2f, 7.2f);
        File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data3));



        //3. Json형식 -> 오브젝트 데이터 
        Data data2 = JsonUtility.FromJson<Data>(str);
        data2.printData();

        //3-1. 파일 load
        string str2 = File.ReadAllText(Application.dataPath + "/TestJson.json");
        Data data4 = JsonUtility.FromJson<Data>(str2);
        data4.printData();
    }
}
