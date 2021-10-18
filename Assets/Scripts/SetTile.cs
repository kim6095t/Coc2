using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    Unit setUnit;      // 내 위에 설치된 타워.
    public bool IsSetUnit => setUnit != null;

    public void Set(Unit newUnit)
    {
        setUnit = newUnit;
        newUnit.transform.position = transform.position;   // 위치 값 갱신.
        newUnit.transform.rotation = transform.rotation;   // 회전 값 갱신.
        Debug.Log(newUnit.transform.position);
    }
    public Unit Remove()
    {
        Unit removeTower = setUnit;
        setUnit = null;

        return removeTower;
    }
}
