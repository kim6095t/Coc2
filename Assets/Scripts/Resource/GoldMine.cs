using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : MonoBehaviour
{
    Camera mainCamera;
    float time;
    int perSecondGetGold;
    int goldResource;

    private void Start()
    {
        mainCamera = Camera.main;
        perSecondGetGold = 10;
    }

    private void Update()
    {
        time += Time.deltaTime;
        goldResource = perSecondGetGold * (int)time;

        if (Input.GetMouseButtonDown(0))
        {
            GetGold();
        }
    }

    private void GetGold()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider == null)
            return;

        if (hit.transform.gameObject == gameObject)
        {
            Debug.Log($"È¹µæÇÑ °ñµå´Â {goldResource}ÀÔ´Ï´Ù");
            MyResourceData.Instance.GetGoldToMine(goldResource);
            time = 0f;
        }
    }
}
