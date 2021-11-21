using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScene : MonoBehaviour
{
    [SerializeField] ShopScene shopScene;

    public void SwitchResultMenu(bool isShow)
    {
        shopScene.gameObject.SetActive(isShow);
    }
}
