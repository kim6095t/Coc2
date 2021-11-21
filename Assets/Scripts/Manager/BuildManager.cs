using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : Singletone<BuildManager>
{
    [SerializeField] LayerMask tileMask;

    string buildPrefab;

    private void Start()
    {
        buildPrefab = null;
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                MousePointToRay();
            }
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
            CreateBuild(setTile);
        }
    }
    private void CreateBuild(SetTile setTile)
    {
        // 선택된 건물이 없을 때는 사용 불가
        if (buildPrefab == null)
            return;

        GameObject build=Instantiate(Resources.Load<GameObject>($"BuildObject/{buildPrefab}"));
        build.transform.position = setTile.transform.position;

        //설치이후 초기화
        buildPrefab = null;
    }

    public void GetbuildPrefab(string prefabName)
    {
        buildPrefab = prefabName;
        Debug.Log(buildPrefab);
    }
}
    
