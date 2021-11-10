using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitButtonManager : MonoBehaviour
{
    [SerializeField] UnitButton buttonPrefab;
    [SerializeField] Transform emptySlotPrefab;

    private void Start()
    {
        int MadeSlot = 10;

        for (Unit_TYPE type = 0; type < Unit_TYPE.Count; type++)
        {
            UnitData towerData = UnitManager.Instance.GetData(type);
            UnitButton newButton = Instantiate(buttonPrefab, transform);

            newButton.Setup(towerData);
            MadeSlot--;
        }

        for(int i=0; i<MadeSlot; i++)
        {
            Instantiate(emptySlotPrefab, transform);
        }
    }
}
