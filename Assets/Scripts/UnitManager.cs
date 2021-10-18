using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] Unit UnitPrefab;
    [SerializeField] LayerMask tileMask;

    //���� ��ȯ �ð�
    float callRate = 1;
    float originCallRate;
    float nextCallTime = 0f;

    private void Start()
    {
        originCallRate = callRate;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            MousePointToRay();
            nextCallTime = Time.time + callRate;
        }

        if (Input.GetMouseButton(0) && nextCallTime <= Time.time)
        {
            MousePointToRay();
            callRate = Mathf.Clamp(callRate /= 2, 0.01f, originCallRate);
            nextCallTime = Time.time + callRate;
        }

        if (Input.GetMouseButtonUp(0)){
            callRate = originCallRate;
        }
    }

    private void MousePointToRay()
    {
        // ���콺�� ���� ��ġ�� Ray�� ��ȯ.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
        {
            SetTile setTile = hit.collider.GetComponent<SetTile>();
            CreateUnit(setTile);
        }
    }

    private void CreateUnit(SetTile setTile)
    {
        // ������ Ÿ���� ���ų� Ÿ�Ͽ� �̹� ��ġ�� �Ǿ��ִ� ���.
        if (setTile == null || setTile.IsSetUnit)
            return;

        Unit newUnit = Instantiate(UnitPrefab, transform);
        setTile.Set(newUnit);
    }

}

