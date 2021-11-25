using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitCreateSceneUI : MonoBehaviour
{
    [SerializeField] UnitButton buttonPrefab;
    [SerializeField] Transform transform;
    UnitButton[] unitButtons;
    

    private void Start()
    {
        unitButtons = new UnitButton[(int)Unit_TYPE.Count];

        for (Unit_TYPE type = 0; type < Unit_TYPE.Count; type++)
        {
            Debug.Log(type);
            Debug.Log((int)Unit_TYPE.Count);

            UnitData unitData = UnitManager.Instance.GetData(type);
            UnitButton newButton = Instantiate(buttonPrefab, transform);
            unitButtons[(int)type] = newButton;

            newButton.SetUpToWar(unitData);
        }
    }

}
