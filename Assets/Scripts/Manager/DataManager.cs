using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    static string FLAG_SAVE = "SAVED";

    public static event System.Action OnSave;       // 데이터 세이브.
    public static event System.Action OnLoad;       // 데이터 로드.
    public static event System.Action OnInit;       // 초기 값으로.

    public static bool IsSavedData => GetBool(FLAG_SAVE);   // 세이브 파일 존재 유무.

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    public static bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(FLAG_SAVE, 1);
        PlayerPrefs.SetInt(key, value);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetInt(FLAG_SAVE, 1);
        PlayerPrefs.SetFloat(key, value);
    }
    public static void SetBool(string key, bool isBool)
    {
        PlayerPrefs.SetInt(FLAG_SAVE, 1);
        PlayerPrefs.SetInt(key, isBool ? 1 : 0);
    }

    public static void SaveAll()
    {
        OnSave?.Invoke();
    }
    public static void LoadAll()
    {
        OnLoad?.Invoke();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();        // 저장소의 모든 데이터 삭제.
        LoadAll();                      // 게임 내 변수의 값 모두 삭제.
        OnInit?.Invoke();               // 초기 값이 필요한 변수 세팅.
    }
}