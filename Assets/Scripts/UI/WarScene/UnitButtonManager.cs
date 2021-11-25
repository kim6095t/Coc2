using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitButtonManager : MonoBehaviour
{
    [SerializeField] UnitButton buttonPrefab;
    [SerializeField] Transform emptySlotPrefab;

    UnitButton[] unitButtons;

    private void Start()
    {
        int MadeSlot = 10;
        unitButtons = new UnitButton[MadeSlot];    

        for (Unit_TYPE type = 0; type < Unit_TYPE.Count; type++)
        {
            UnitData unitData = UnitManager.Instance.GetData(type);
            UnitButton newButton = Instantiate(buttonPrefab, transform);
            unitButtons[(int)type] = newButton;

            newButton.SetUpToWar(unitData);
            MadeSlot--;
        }

        for(int i=0; i<MadeSlot; i++)
        {
            Instantiate(emptySlotPrefab, transform);
        }
    }

    //���ֻ����Ǹ� UI������ ��������
    public void OnCreateUnit(Unit_TYPE type)
    {
        unitButtons[(int)type].OnCountUnit(unitButtons[(int)type]);
    }
}
