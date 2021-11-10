using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] Image unitImage;
    [SerializeField] Text priceText;

    Unit.Unit_TYPE type;

    public void Setup(UnitData unitData)
    {
        // 데이터를 받아와 열거형으로 파싱.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));

        // towerImage.sprite = tower.towerSprite;
        priceText.text = string.Format("{0:#,##0}", unitData.GetData(Unit.KEY_PRICE));

        // 버튼에 이벤트 등록.
        GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.OnSelectedUnit(type));
    }
}
