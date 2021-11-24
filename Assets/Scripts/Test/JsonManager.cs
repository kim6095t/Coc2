using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    /*
     * [���ǻ���]
     * Json�� class ������ �����͸� ��ȯ�ϸ� List<T> �ڷ��� ���� ���� �ٷ� json�����ͷ� ��ȯ�� �� ����.
     * ���� List �����͸� class�� ���� �� �ش� class�� �̿��� json�����ͷ� ��ȯ��Ų��.     * 
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
      
    // JsonUtility.ToJson(�ؽ�Ʈ ����, ���� ����)
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
