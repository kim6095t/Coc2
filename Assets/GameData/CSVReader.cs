using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static Dictionary<string, string>[] ReadCSV(TextAsset data)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        //예외처리
        if (data == null)
        {
            return null;
        }


        string[] split = data.text.Split('\n');
        string[] keys = split[0].Split(',');                      // 0번째 열의 키 값을 대입.
        string[] dataColumns = new string[split.Length - 1];      // 전체 열의 개수 - 1(키 값)
        for (int i = 0; i < dataColumns.Length; i++)                 // 헤드 값을 제외한 나머지 값을 대입.
            dataColumns[i] = split[i + 1];

        for (int index = 0; index < dataColumns.Length; index++)
        {
            if (string.IsNullOrEmpty(dataColumns[index]))         // index번째 데이터 줄이 아무런 데이터도 없을 경우.
                continue;

            string[] datas = dataColumns[index].Split(',');       // index번째 데이터 줄을 개별 데이터로 자른다.
            result.Add(new Dictionary<string, string>());         // 값을 담을 딕셔너리 객체 생성.

            for (int row = 0; row < datas.Length; row++)
            {
                result[index].Add(keys[row], datas[row]);         // row(열)번째 키 값과 데이터 값을 대입.
            }
        }

        return result.ToArray();
    }
}
