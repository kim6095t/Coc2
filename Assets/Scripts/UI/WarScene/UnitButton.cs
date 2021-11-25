using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] Image unitImage;
    [SerializeField] Text text;

    Unit.Unit_TYPE type;
    Image backGround;


    public void SetUpToWar(UnitData unitData)
    {
        backGround = gameObject.GetComponent<Image>();

        // 데이터를 받아와 열거형으로 파싱.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));
        unitImage.sprite = Resources.Load<Sprite>($"UnitSprite/{type}");
        text.text = string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)type].countUnit);

        // 버튼에 이벤트 등록.
        GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.OnSelectedUnit(type));
    }


    public void SetUpToTown(UnitData unitData)
    {
        backGround = gameObject.GetComponent<Image>();

        // 데이터를 받아와 열거형으로 파싱.
        type = (Unit.Unit_TYPE)System.Enum.Parse(typeof(Unit.Unit_TYPE), unitData.GetData(Unit.KEY_TYPE));
        unitImage.sprite = Resources.Load<Sprite>($"UnitSprite/{type}");
        text.text = string.Format("{0:#,##0}", UnitManager.Instance.unitData[(int)type].countUnit);

    }

    public void OnCountUnit(UnitButton unitButton)
    {
        unitButton.text.text = $"{UnitManager.Instance.unitData[(int)type].countUnit}";
        if (UnitManager.Instance.unitData[(int)type].countUnit <= 0)
        {
            backGround.color = new Color(100f/255f, 100f/255f, 100f/255f);
            unitImage.color = new Color(100f/255f, 100f/255f, 100f/255f);
        }
    }
}
