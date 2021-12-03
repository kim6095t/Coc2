using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfButton : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Image informationButton;
    [SerializeField] Image upgradeButton;
    [SerializeField] InformationScene informationScene;
    [SerializeField] UpgradeScene upgradeScene;

    public void ClickInfBtn()
    {
        informationScene.gameObject.SetActive(true);

        title.gameObject.SetActive(false);
        informationButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
    }

    public void ClickUpgradeBtn()
    {
        upgradeScene.gameObject.SetActive(true);

        title.gameObject.SetActive(false);
        informationButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
    }


    public void ExitInformationScene()
    {
        informationScene.gameObject.SetActive(false);
    }
    public void ExitUpgradeScene()
    {
        upgradeScene.gameObject.SetActive(false);
    }
}
