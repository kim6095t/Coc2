using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    Unit setUnit;      // �� ���� ��ġ�� Ÿ��.
    public bool IsSetUnit => setUnit != null;

    public void Set(Unit newUnit)
    {
        setUnit = newUnit;
        newUnit.transform.position = transform.position;   // ��ġ �� ����.
        newUnit.transform.rotation = transform.rotation;   // ȸ�� �� ����.
        Debug.Log(newUnit.transform.position);
    }
    public Unit Remove()
    {
        Unit removeTower = setUnit;
        setUnit = null;

        return removeTower;
    }
}
