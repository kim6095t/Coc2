using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckEndMenu : MonoBehaviour
{
    [SerializeField] EndGame endGame;
    [SerializeField] ResultMenu resultMenu;
    [SerializeField] Button resume;
    [SerializeField] Button quit;

    public void ResumeButton()
    {
        resultMenu.SwitchResultMenu(false);
        gameObject.SetActive(false);
    }
    public void QuitButton()
    {
        resultMenu.SwitchResultMenu(true);
        endGame.GameResult();
        gameObject.SetActive(false);
    }
}
