using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInformation : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] ObjectInfButton infButton;
    [SerializeField] InformationScene informationScene;
    [SerializeField] UpgradeScene upgradeScene;

    public void ChildSetActive()
    {
        title.gameObject.SetActive(true);
        infButton.gameObject.SetActive(true);

        int titleCount = title.transform.childCount;
        int infButtonCount = infButton.transform.childCount;

        for (int i = 0; i < titleCount; i++)
        {
            Transform trChild = title.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }

        for (int i = 0; i < infButtonCount; i++)
        {
            Transform trChild = infButton.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
    }

    public void GetSettingData(ObjectProperty target)
    {
        informationScene.GetSettingData(target);
        upgradeScene.GetSettingData(target);
    }
}
