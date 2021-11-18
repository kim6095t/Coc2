using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static Dictionary<string, string>[] ReadCSV(TextAsset data)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        //����ó��
        if (data == null)
        {
            return null;
        }


        string[] split = data.text.Split('\n');
        string[] keys = split[0].Split(',');                      // 0��° ���� Ű ���� ����.
        string[] dataColumns = new string[split.Length - 1];      // ��ü ���� ���� - 1(Ű ��)
        for (int i = 0; i < dataColumns.Length; i++)                 // ��� ���� ������ ������ ���� ����.
            dataColumns[i] = split[i + 1];

        for (int index = 0; index < dataColumns.Length; index++)
        {
            if (string.IsNullOrEmpty(dataColumns[index]))         // index��° ������ ���� �ƹ��� �����͵� ���� ���.
                continue;

            string[] datas = dataColumns[index].Split(',');       // index��° ������ ���� ���� �����ͷ� �ڸ���.
            result.Add(new Dictionary<string, string>());         // ���� ���� ��ųʸ� ��ü ����.

            for (int row = 0; row < datas.Length; row++)
            {
                result[index].Add(keys[row], datas[row]);         // row(��)��° Ű ���� ������ ���� ����.
            }
        }

        return result.ToArray();
    }
}
