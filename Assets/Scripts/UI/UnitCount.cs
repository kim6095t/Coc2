using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitCount
{
    private const int Goblin = 1;
    private const int Archer = 2;
    private const int Warrier = 1;
    private const int Giant = 2;

    public static Dictionary<Unit_TYPE, int> numUnit = new Dictionary<Unit_TYPE, int>()
    {
        {Unit_TYPE.Goblin ,Goblin},
        {Unit_TYPE.Archer ,Archer},
        {Unit_TYPE.Warrier ,Warrier},
        {Unit_TYPE.Giant ,Giant},
    };
}

