using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

[System.Serializable]
public class UnitCountData
{
    public int m_nLevel;
    public Vector3 m_vecPositon;
}

public class UnitCount
{
    private static int Goblin;
    private static int Archer;
    private static int Warrier;
    private static int Giant;

    public static Dictionary<Unit_TYPE, int> numUnit = new Dictionary<Unit_TYPE, int>()
    {
        {Unit_TYPE.Goblin ,Goblin},
        {Unit_TYPE.Archer ,Archer},
        {Unit_TYPE.Warrier ,Warrier},
        {Unit_TYPE.Giant ,Giant},
    };
}

