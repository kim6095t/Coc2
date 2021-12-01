using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject GameSelectMenu;
    [SerializeField] GameObject unitCreateScene;
    [SerializeField] ShopScene shopScene;
    [SerializeField] GameObject settingScene;

    public void OnClickAttack()
    {
        GameSelectMenu.gameObject.SetActive(true);
    }

    public void OnClickMenuExit()
    {
        GameSelectMenu.gameObject.SetActive(false);
    }

    public void OnClickSetting()
    {
        settingScene.gameObject.SetActive(true);
    }

    public void OnClickSettingExit()
    {
        settingScene.gameObject.SetActive(false);
    }


    public void OnClickShop()
    {
        shopScene.SwitchResultMenu(true);
    }

    public void OnClickShopExit()
    {
        shopScene.SwitchResultMenu(false);
    }

    public void OnClickUnitScene()
    {
        unitCreateScene.gameObject.SetActive(true);
    }

    public void OnClickUnitExit()
    {
        unitCreateScene.gameObject.SetActive(false);
    }

    private void CloseAllMenu()
    {
        GameSelectMenu.gameObject.SetActive(false);
        shopScene.SwitchResultMenu(false);
        settingScene.gameObject.SetActive(false);
        unitCreateScene.gameObject.SetActive(false);
    }

    public void OnClickStage1()
    {
        CloseAllMenu();
        LoadSceneManager.Instance.LoadScene("WarScene 1");
    }

    public void OnClickStage2()
    {
        CloseAllMenu();
        LoadSceneManager.Instance.LoadScene("WarScene 2");
    }
}
