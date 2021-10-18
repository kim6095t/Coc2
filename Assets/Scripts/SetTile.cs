using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    public void Set(Unit newUnit)
    {
        newUnit.transform.position = transform.position;   // 위치 값 갱신.
        newUnit.transform.rotation = transform.rotation;   // 회전 값 갱신.
    }
}
