using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScene : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Text objectName;
    [SerializeField] Text explain;
    [SerializeField] InformationSceneBar OutPutBar;
    [SerializeField] InformationSceneBar HpBar;

    public void GetSettingData(ObjectProperty target)
    {
        Dictionary<string, string>[] datas = target.CsvDatas;
        int level = target.NowLevel;

        //����� ����
        mainImage.sprite = Resources.Load<Sprite>($"ShopSprite/{datas[level]["Name"]}");
        objectName.text = $"{ datas[level]["Name"]} ({datas[level]["Level"]}����)";
        explain.text = datas[level]["Explain"];


        //�ڿ��ǹ��� �߰����� ����
        if (datas[level]["Name"].Equals("GoldBox") || datas[level]["Name"].Equals("JellyBox"))
        {
            OutPutBar.text.text = $"���귮: �ð��� {datas[level]["Output"]}";
            OutPutBar.bar.fillAmount = float.Parse(datas[level]["Output"])/ float.Parse(datas[datas.Length -1]["Output"]);

            HpBar.text.text = $"HP: {datas[level]["Hp"]}/{datas[level]["Hp"]}";
            HpBar.bar.fillAmount = float.Parse(datas[level]["Hp"])/ float.Parse(datas[datas.Length - 1]["Hp"]);
        }
    }
}
