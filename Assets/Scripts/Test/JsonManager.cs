using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    /*
     * [주의사항]
     * Json은 class 단위로 데이터를 변환하며 List<T> 자료형 변수 등은 바로 json데이터로 변환할 수 없다.
     * 따라서 List 데이터를 class로 감싼 후 해당 class를 이용해 json데이터로 변환시킨다.     * 
     */


    [System.Serializable]
    public class ArrayJson<T>
    {
        public T[] datas;
        public ArrayJson(T[] datas)
        {
            this.datas = datas;
        }
    }
    [System.Serializable]
    public class OneJson<T>
    {
        public T data;
        public OneJson(T data)
        {
            this.data = data;
        }
    }
      
    // JsonUtility.ToJson(텍스트 파일, 정렬 유무)
    public static string ObjectToJson<T>(T data)
    {
        OneJson<T> package = new OneJson<T>(data);
        return JsonUtility.ToJson(package, true);

    }
    public static string ObjectsToJson<T>(T[] data)
    {
        ArrayJson<T> package = new ArrayJson<T>(data);
        return JsonUtility.ToJson(package, true);
    }


    public static T JsonToObject<T>(string jsonData)
    {
        OneJson<T> json = JsonUtility.FromJson<OneJson<T>>(jsonData);
        return json.data;
    }
    public static T[] JsonToObjects<T>(string jsonData)
    {
        ArrayJson<T> json = JsonUtility.FromJson<ArrayJson<T>>(jsonData);
        return json.datas;
    }
}
