using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unit;     // Unit Ŭ������ ������ �����ϰڴ�.


public class UnitCreateManager : UnitManager
{
    [SerializeField] Unit[] unitPrefabs;
    [SerializeField] UnitButtonManager unitButtonManager;
    [SerializeField] LayerMask tileMask;
    [SerializeField] ResultMenu resultMenu;
    [SerializeField] EndGame endGame;
    [HideInInspector] public int maxUnitCount;

    public delegate void DelUnitEvent();                  // ��������Ʈ ����.
    event DelUnitEvent OnDelUnit;                         // �̺�Ʈ �Լ� ����.

    Unit_TYPE selectedType = Unit_TYPE.None;             // ���� ������ Ÿ���� Ÿ��.

    //���� ��ȯ �ð�
    float callRate = 1;
    float originCallRate;
    float nextCallTime = 0f;

    private void Awake()
    {
        base.Awake();
        for (int i = 0; i < unitData.Length; i++)
            maxUnitCount += unitData[i].countUnit;
        originCallRate = callRate;
    }

    private void Update()
    {
        //����ó��
        if (EventSystem.current == null)
            return;

        if (!EventSystem.current.IsPointerOverGameObject())
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
            if (Input.GetMouseButtonUp(0))
            {
                callRate = originCallRate;
            }
        }

        //���̻� ������ ���� ��
        if (maxUnitCount <= 0)
        {
            resultMenu.SwitchResultMenu(true);
            endGame.GameResult();
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
        // ������ �������� �ʰ� ��ȯ�� �� ȭ�鿡 �����޼��� ���
        if (selectedType == Unit_TYPE.None)
        {
            TextCollect.Instance.OnNotSelectedUnit();
            return;
        }

        if (setTile == null || setTile.IsOnObject)
            return;

        //��ȯ������ ������ ��μ�ȯ ������
        if (unitData[(int)selectedType].countUnit <= 0)
            return;

        Unit newUnit = Instantiate(unitPrefabs[(int)selectedType]);
        newUnit.Setup(unitData[(int)newUnit.Type].newData);

        setTile.SetUnit(newUnit);
        unitData[(int)newUnit.Type].countUnit -= 1;
        unitButtonManager.OnCreateUnit(newUnit.Type);

        TextCollect.Instance.OnFalseAllText();
    }

    public void OnSelectedUnit(Unit.Unit_TYPE type)
    {
        selectedType = type;
    }
    
    public void RegestedDelUnit(DelUnitEvent OnDelUnit)
    {
        this.OnDelUnit += OnDelUnit;
    }
    public void RemoveDelUnit(DelUnitEvent OnDelUnit)
    {
        this.OnDelUnit -= OnDelUnit;
    }

    public void OnDelUnitInvoke()
    {
        OnDelUnit?.Invoke();
    }
}
