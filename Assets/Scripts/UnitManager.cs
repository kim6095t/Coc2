using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] Unit UnitPrefab;
    [SerializeField] LayerMask tileMask;

    //유닛 소환 시간
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
        // 마우스의 현재 위치를 Ray로 변환.
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
        // 선택한 타일이 없거나 타일에 이미 설치가 되어있는 경우.
        if (setTile == null || setTile.IsSetUnit)
            return;

        Unit newUnit = Instantiate(UnitPrefab, transform);
        setTile.Set(newUnit);
    }

}

