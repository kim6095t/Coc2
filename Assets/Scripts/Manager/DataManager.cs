using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    static string FLAG_SAVE = "SAVED";

    public static event System.Action OnSave;       // ������ ���̺�.
    public static event System.Action OnLoad;       // ������ �ε�.
    public static event System.Action OnInit;       // �ʱ� ������.

    public static bool IsSavedData => GetBool(FLAG_SAVE);   // ���̺� ���� ���� ����.

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
        PlayerPrefs.DeleteAll();        // ������� ��� ������ ����.
        LoadAll();                      // ���� �� ������ �� ��� ����.
        OnInit?.Invoke();               // �ʱ� ���� �ʿ��� ���� ����.
    }
}