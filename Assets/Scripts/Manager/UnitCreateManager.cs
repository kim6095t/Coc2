using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unit;     // Unit 클래스의 영역을 포함하겠다.


public class UnitCreateManager : Singletone<UnitCreateManager>
{
    [System.Serializable]
    public struct UnitPrefab
    {
        public Unit prefab;
        public Unit_TYPE type;
    }

    [SerializeField] UnitPrefab[] unitPrefabs;
    [SerializeField] UnitButtonManager unitButtonManager;
    [SerializeField] LayerMask tileMask;
    [SerializeField] ResultMenu resultMenu;
    [SerializeField] EndGame endGame;
    [HideInInspector] public int maxUnitCount;

    public delegate void DelUnitEvent();                  // 델리게이트 정의.
    event DelUnitEvent OnDelUnit;                         // 이벤트 함수 선언.

    Unit_TYPE selectedType = Unit_TYPE.None;             // 현재 선택한 타워의 타입.

    //유닛 소환 시간
    float callRate = 1;
    float originCallRate;
    float nextCallTime = 0f;

    private void Start()
    {
        base.Awake();

        for (int i = 0; i < UnitManager.Instance.unitData.Length; i++)
            maxUnitCount += UnitManager.Instance.unitData[i].countUnit;
        originCallRate = callRate;
    }

    private void Update()
    {
        //예외처리
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

        //더이상 유닛이 없을 때
        if (maxUnitCount <= 0)
        {
            resultMenu.SwitchResultMenu(true);
            endGame.GameResult();
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

    private Unit GetPrefab(Unit_TYPE type)
    {
        foreach(UnitPrefab unitPrefab in unitPrefabs)
        {
            if (unitPrefab.type == type)
                return unitPrefab.prefab;
        }

        return null;
    }

    private void CreateUnit(SetTile setTile)
    {
        // 유닛을 선택하지 않고 소환할 시 화면에 에러메세지 출력
        if (selectedType == Unit_TYPE.None)
        {
            TextCollect.Instance.OnNotSelectedUnit();
            return;
        }

        if (setTile == null || setTile.IsOnObject)
            return;

        //소환가능한 유닛을 모두소환 했을때
        if (UnitManager.Instance.unitData[(int)selectedType].countUnit <= 0)
            return;

        Unit newUnit = Instantiate(GetPrefab(selectedType));
        newUnit.Setup(UnitManager.Instance.unitData[(int)newUnit.Type].newData);

        setTile.SetUnit(newUnit);
        UnitManager.Instance.unitData[(int)newUnit.Type].countUnit -= 1;
        unitButtonManager.OnCreateUnit(newUnit.Type);

        TextCollect.Instance.OnFalseAllText();
    }

    public void OnCreatedUnit(Text unitCount, Text priceText, Unit.Unit_TYPE type)
    {
        Debug.Log("hihi");
        //UnitManager.Instance.unitData[(int)type].countUnit += 1;
        //unitCount.text= string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)type].countUnit);
        //MyResourceData.Instance.UseJellyToMine(int.Parse(priceText.text));
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
