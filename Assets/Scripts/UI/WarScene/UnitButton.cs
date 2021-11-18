using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] Image unitImage;
    [SerializeField] Text priceText;

    Unit.Unit_TYPE type;
    Image backGround;


    public void Setup(UnitData unitData)
    {
        backGround = gameObject.GetComponent<Image>();

        // �����͸� �޾ƿ� ���������� �Ľ�.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));
        unitImage.sprite = Resources.Load<Sprite>($"UnitSprite/{type}");
        priceText.text = string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)type].countUnit);

        // ��ư�� �̺�Ʈ ���.
        GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.OnSelectedUnit(type));
    }

    public void OnCountUnit(UnitButton unitButton)
    {
        unitButton.priceText.text = $"{UnitManager.Instance.unitData[(int)type].countUnit}";
        if (UnitManager.Instance.unitData[(int)type].countUnit <= 0)
        {
            backGround.color = new Color(100f/255f, 100f/255f, 100f/255f);
            unitImage.color = new Color(100f/255f, 100f/255f, 100f/255f);
        }
    }
}
