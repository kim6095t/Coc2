using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitCount : MonoBehaviour
{
    public static Dictionary<Unit_TYPE, int> unitCount;         // À¯´ÖÀÇ ¼ö.

    void Start()
    {
        unitCount= new Dictionary<Unit_TYPE, int>();
        
        for(int i=0; i <= (int)Unit_TYPE.Count; i++)
        {
            unitCount.Add(type, 0);
        }
    }

    void Update()
    {
        
    }
}
