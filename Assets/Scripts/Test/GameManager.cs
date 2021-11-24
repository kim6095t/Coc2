using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public string name;
        public int age;
        public float height;

        public Vector3 pos;
    }

    List<Data> dataList;

    private void Start()
    {
        Init();
        Save();
    }

    private void Init()
    {
        dataList = new List<Data>();

        for (int i = 0; i < 5; i++)
        {
            Data newData = new Data();
            newData.name = string.Concat("ÀÌ¸§", i);
            newData.age = i * 10;
            newData.height = i * 3.14f;
            newData.pos = new Vector3(i * 2, i * 5, i * 10);

            dataList.Add(newData);
        }
    }
    private void Save()
    {
        string path = string.Concat(Application.dataPath, "/jsonText.json");
        using (StreamWriter writer = new StreamWriter(path))
        {
            string json = JsonManager.ObjectsToJson(dataList.ToArray());
            writer.Write(json);
        }
    }
}
