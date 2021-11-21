using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public Image image;
    public Text price;
    private ResourceListUI resourceListUI;
    private ShopScene shopSceneUI;

    private void Start()
    {
        resourceListUI = FindObjectOfType<ResourceListUI>();
        shopSceneUI = FindObjectOfType<ShopScene>();
    }

    public void ClickSlot()
    {
        if (int.Parse(price.text) <= MyResourceData.Instance.myGold)
        {
            MyResourceData.Instance.UseGoldToMine(int.Parse(price.text));
            resourceListUI.ReloadResourceUI();
            shopSceneUI.SwitchResultMenu(false);

            BuildManager.Instance.GetbuildPrefab(image.sprite.name);
        }


    }
}
