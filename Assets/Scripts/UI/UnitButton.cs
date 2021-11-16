using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] Image unitImage;
    public Text priceText;

    Unit.Unit_TYPE type;

    public void Setup(UnitData unitData)
    {
        // �����͸� �޾ƿ� ���������� �Ľ�.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));
        unitImage.sprite = Resources.Load<Sprite>($"UnitSprite/{type}");
        priceText.text = string.Format("{0:#,##0}", UnitManager.Instance.unitCount[type]);

        // ��ư�� �̺�Ʈ ���.
        GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.OnSelectedUnit(type));
    }
}
