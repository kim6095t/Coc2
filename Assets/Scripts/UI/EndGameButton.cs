using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameButton : MonoBehaviour
{
    [SerializeField] CheckEndMenu checkEndMenu;

    public void OnCheckEndMenu()
    {
        checkEndMenu.gameObject.SetActive(true);
    }
}
