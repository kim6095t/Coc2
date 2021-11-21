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
        // ���콺�� ���� ��ġ�� Ray�� ��ȯ.
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
        // ���õ� �ǹ��� ���� ���� ��� �Ұ�
        if (buildPrefab == null)
            return;

        GameObject build=Instantiate(Resources.Load<GameObject>($"BuildObject/{buildPrefab}"));
        build.transform.position = setTile.transform.position;

        //��ġ���� �ʱ�ȭ
        buildPrefab = null;
    }

    public void GetbuildPrefab(string prefabName)
    {
        buildPrefab = prefabName;
        Debug.Log(buildPrefab);
    }
}
    
