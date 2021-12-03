using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScene : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Text objectName;
    [SerializeField] Text explain;
    [SerializeField] StatusBar AmountBar;
    [SerializeField] StatusBar OutPutBar;
    [SerializeField] StatusBar HpBar;

    public void GetSettingData(Dictionary<string, string> datas)
    {
        mainImage.sprite = Resources.Load<Sprite>($"ShopSprite/{datas["Name"]}");
        objectName.text = $"{ datas["Name"]} ({datas["Level"]}����)";
        explain.text = datas["Explain"];

        if (datas["Name"].Equals("GoldBox"))
        {
            AmountBar.text.text = $"�뷮: 1/{ datas["MaxAmount"]}";
            OutPutBar.text.text = $"���귮: �ð��� {datas["Output"]}";
            //OutPutBar.bar.fillAmount=
            HpBar.text.text = $"HP: {datas["Hp"]}/{datas["Hp"]}";
        }
    }
}
