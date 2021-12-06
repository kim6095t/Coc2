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
    [SerializeField] Text upgradeBtn;

    //업그레이드를 위한 변수
    [SerializeField] ObjectInformation objectInformation;
    [SerializeField] ProgressBar progressBar;

    private int nextLevel;
    private int nowLevel;
    private Vector3 pos;
    private Canvas canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void GetSettingData(Dictionary<string, string>[] datas, int level, Vector3 pos)
    {
        nowLevel = level;
        nextLevel = level + 1;
        this.pos = pos;

        //공통된 정보
        mainImage.sprite = Resources.Load<Sprite>($"ShopSprite/{datas[level]["Name"]}");
        objectName.text = $"{ datas[level]["Name"]} ({datas[level]["Level"]}레벨)";
        upgradeBtn.text = $"{ datas[level]["UpgradePrice"]}";

        //자원건물의 추가적인 정보
        if (datas[level]["Name"].Equals("GoldBox") || datas[level]["Name"].Equals("JellyBox"))
        {
            OutPutBar.text.text = $"생산량: 시간당 {datas[level]["Output"]}";
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
        uiPosition.y += 50f;
        pgb.transform.localPosition = uiPosition;
        pgb.transform.SetParent(canvas.gameObject.transform);

        objectInformation.Upgrade(nextLevel);
    }
}
