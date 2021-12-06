using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScene : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Text objectName;
    [SerializeField] UpgradeSceneBar OutPutBar;
    [SerializeField] UpgradeSceneBar HpBar;
    [SerializeField] Text upgradeBtnText;
    [SerializeField] Button upgradeBtn;

    //���׷��̵带 ���� ����
    [SerializeField] ObjectInformation objectInformation;
    [SerializeField] ProgressBar progressBar;

    private int nextLevel;
    private Vector3 pos;
    private Canvas canvas;

    ObjectProperty target;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void GetSettingData(ObjectProperty target)
    {
        this.target = target;

        Dictionary<string, string>[] datas = target.CsvDatas;
        int level = target.NowLevel;

        nextLevel = target.NowLevel + 1;
        pos = target.transform.position;

        

        //����� ����
        mainImage.sprite = Resources.Load<Sprite>($"ShopSprite/{datas[level]["Name"]}");
        objectName.text = $"{ datas[level]["Name"]} ({datas[level]["Level"]}����)";
        upgradeBtnText.text = $"{ datas[level]["UpgradePrice"]}";

        //Max ������ �� ��ư ��Ȱ��ȭ
        if (upgradeBtnText.text.Equals(datas[datas.Length - 1]["UpgradePrice"]))
            upgradeBtn.interactable = false;
        else
            upgradeBtn.interactable = true;

        //�ڿ��ǹ��� �߰����� ����
        if (datas[level]["Name"].Equals("GoldBox") || datas[level]["Name"].Equals("JellyBox"))
        {
            OutPutBar.text.text = $"���귮: �ð��� {datas[level]["Output"]}";
            OutPutBar.nowLvBar.fillAmount = float.Parse(datas[level]["Output"]) / float.Parse(datas[datas.Length - 1]["Output"]);

            if (OutPutBar.nowLvBar.fillAmount < 1f)
                OutPutBar.nextLvBar.fillAmount = float.Parse(datas[nextLevel]["Output"]) / float.Parse(datas[datas.Length - 1]["Output"]);

            HpBar.text.text = $"HP: {datas[level]["Hp"]}/{datas[level]["Hp"]}";
            HpBar.nowLvBar.fillAmount = float.Parse(datas[level]["Hp"]) / float.Parse(datas[datas.Length - 1]["Hp"]);

            if(HpBar.nowLvBar.fillAmount < 1f)
                OutPutBar.nextLvBar.fillAmount = float.Parse(datas[nextLevel]["Output"]) / float.Parse(datas[datas.Length - 1]["Output"]);
        }
    }
    public void UpgradeButton()
    {
        gameObject.SetActive(false);
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(pos);

        ProgressBar pgb = Instantiate(progressBar);
        uiPosition.y += 100f;
        pgb.transform.localPosition = uiPosition;
        pgb.transform.SetParent(canvas.gameObject.transform);
    }

    //ProgressBar�� ������ ȣ��ȴ�
    public void OnUpgrade()
    {
        target.OnUpgradeTower(nextLevel);
    }
}
