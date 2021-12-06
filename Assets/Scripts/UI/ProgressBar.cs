using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Image backGround;
    UpgradeScene upgradeScene;

    float time;
    float upgradeCompleteTime;
    private void Start()
    {
        upgradeScene=GameObject.Find("ObjectInformation").transform.Find("UpgradeScene").GetComponent<UpgradeScene>();
        upgradeCompleteTime = 3f;
        progressBar.fillAmount = 0f;
        StartCoroutine(FilledGage());
    }

    IEnumerator FilledGage()
    {
        while (time <= upgradeCompleteTime)
        {
            time = time + Time.deltaTime;
            progressBar.fillAmount = time / upgradeCompleteTime;
            yield return null;
        }
        upgradeScene.OnUpgrade();
        Destroy(gameObject);
    }
}
