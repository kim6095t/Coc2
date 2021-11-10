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
        // �����͸� �޾ƿ� ���������� �Ľ�.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));

        // towerImage.sprite = tower.towerSprite;
        priceText.text = string.Format("{0:#,##0}", unitData.GetData(Unit.KEY_PRICE));

        // ��ư�� �̺�Ʈ ���.
        GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.OnSelectedUnit(type));
    }
}
