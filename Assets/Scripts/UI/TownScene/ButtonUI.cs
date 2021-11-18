using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject GameSelectMenu;

    public void OnClickAttack()
    {
        GameSelectMenu.gameObject.SetActive(true);
    }

    public void OnClickExit()
    {
        GameSelectMenu.gameObject.SetActive(false);
    }


    public void OnClickStage1()
    {
        SceneManager.LoadScene("WarScene 1");
    }

    public void OnClickStage2()
    {
        SceneManager.LoadScene("WarScene 2");
    }
}
