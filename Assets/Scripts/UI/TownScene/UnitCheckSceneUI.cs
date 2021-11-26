using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitCheckSceneUI : MonoBehaviour
{
    [SerializeField] UnitButton buttonPrefab;
    [SerializeField] Transform transform;
    UnitButton[] unitButtons;
    

    private void Start()
    {
        unitButtons = new UnitButton[(int)Unit_TYPE.Count];

        for (Unit_TYPE type = 0; type < Unit_TYPE.Count; type++)
        {
            UnitData unitData = UnitManager.Instance.GetData(type);
            UnitButton newButton = Instantiate(buttonPrefab, transform);
            unitButtons[(int)type] = newButton;

            newButton.SetUpToTown(unitData);
        }
    }

}
