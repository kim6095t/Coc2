using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour
{
    private GameObject onObject;        // Ÿ�� ���� �����ϴ� ������Ʈ.
    public bool IsOnObject => onObject != null;


    public void SetUnit(Unit newUnit)
    {
        newUnit.transform.position = transform.position;   // ��ġ �� ����.
        newUnit.transform.rotation = transform.rotation;   // ȸ�� �� ����.
    }

    private void FindOnObject()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, 1.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject target = hits[i].collider.gameObject;
            if (target.CompareTag(MapManager.Instance.TAG_PLAYABLE))
            {
                onObject = target;
                break;
            }
            else
                onObject = null;
        }
    }
}
