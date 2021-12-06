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
    unitStruct lastUnit;
    List<unitStruct> unitList;
    int unitCount;

    struct unitStruct
    {
        public CheckUnitSlot slot;
        public Unit.Unit_TYPE type;
        public Text countText;
    }

    private void Start()
    {
        unitList = new List<unitStruct>();

        lastUnit.type = Unit.Unit_TYPE.None;
        timer = 0f;
        useTimer = false;
    }

    private void Update()
    {
        if (unitList.Count <= 0)
            return;

        if (!useTimer)
        {
            timer = 0f;
            StartCoroutine(createTime(unitList[0]));
        }
    }

    public void OnCreatedUnit(Text unitCount, Text priceText, Unit.Unit_TYPE type)
    {
        //리스트에 추가
        unitStruct unit = new unitStruct();
        unit.type = type;
        unit.countText = unitCount;

        if (lastUnit.type != type) {
            CheckUnitSlot slot = Instantiate(checkUnit);
            slot.transform.parent = trans;
            slot.unitImage.sprite = Resources.Load<Sprite>($"UnitSprite/{type}");
            slot.unitCount.text = "1";
            unit.slot = slot;

            lastUnit = unit;
        }
        else
        {
            string unitNum = (int.Parse(lastUnit.slot.unitCount.text) + 1).ToString();
            lastUnit.slot.unitCount.text = unitNum;
            unit.slot = lastUnit.slot;
        }
        
        unitList.Add(unit);

        MyResourceData.Instance.UseJellyToMine(int.Parse(priceText.text));
    }

    IEnumerator createTime(unitStruct unit)
    {
        useTimer = true;
        while (true)
        {
            timer += Time.deltaTime;
            unit.slot.fillBox.fillAmount = timer / 3f;

            if (timer > 3f)
            {
                UnitManager.Instance.unitData[(int)unit.type].countUnit += 1;
                unit.countText.text = string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)unit.type].countUnit);

                //생성해야 할 유닛이 하나면 슬롯 제거
                if (unit.slot.unitCount.text.ToString().Equals("1"))
                    Destroy(unit.slot.gameObject);
                //두개이상이면 카운트하기
                else
                {
                    unit.slot.unitCount.text = (int.Parse(unit.slot.unitCount.text)-1).ToString();
                }
                unitList.RemoveAt(0);
                useTimer = false;
                break;
            }
            yield return null;
        }
    }
}
