using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitCreateSceneUI : MonoBehaviour
{
    [SerializeField] UnitButton buttonPrefab;
    UnitButton[] unitButtons;

    private void Start()
    {
        unitButtons = new UnitButton[(int)Unit_TYPE.Count];

        for (Unit_TYPE type = 0; type < Unit_TYPE.Count; type++)
        {
            UnitData unitData = UnitManager.Instance.GetData(type);
            UnitButton newButton = Instantiate(buttonPrefab, transform);
            unitButtons[(int)type] = newButton;

            newButton.Setup(unitData);
        }
    }
}
