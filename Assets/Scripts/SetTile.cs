using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    public void Set(Unit newUnit)
    {
        newUnit.transform.position = transform.position;   // ��ġ �� ����.
        newUnit.transform.rotation = transform.rotation;   // ȸ�� �� ����.
    }
}
