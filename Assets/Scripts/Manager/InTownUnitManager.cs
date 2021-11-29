using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InTownUnitManager : Singletone<InTownUnitManager>
{
    [SerializeField] Transform trans;
    [SerializeField] CheckUnitSlot checkUnit;

    float timer;
    bool useTimer;
    Unit.Unit_TYPE lastType;
    List<Unit.Unit_TYPE> unitList;


    struct unitStruct
    {
        Unit.Unit_TYPE;

    }

    private void Start()
    {
        unitList = new List<Unit.Unit_TYPE>();

        lastType = Unit.Unit_TYPE.None;
        timer = 0f;
        useTimer = false;
    }

    private void Update()
    {
        if (!useTimer && unitList[0] != null)
        {
            timer = 0f;
            StartCoroutine(createTime(unitList[0]));
        }
    }

    public void OnCreatedUnit(Text unitCount, Text priceText, Unit.Unit_TYPE type)
    {
        unitList.Add(type);

        CheckUnitSlot slot = Instantiate(checkUnit);
        slot.transform.parent = trans;
        MyResourceData.Instance.UseJellyToMine(int.Parse(priceText.text));
        lastType = type;

    }

    IEnumerator createTime(Unit.Unit_TYPE type)
    {
        useTimer = true;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
            {
                UnitManager.Instance.unitData[(int)type].countUnit += 1;
                unitCount.text = string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)type].countUnit);

                unitList.RemoveAt(0);
                useTimer = false;
                break;
            }
            yield return null;
        }
        
    }
}
