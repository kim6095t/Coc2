using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMine : ObjectProperty
{
    float time;
    int perSecondGetJelly;
    int jellyResource;

    private void Start()
    {
        base.Start();
        perSecondGetJelly = 10;
    }

    private void Update()
    {
        time += Time.deltaTime;
        jellyResource=perSecondGetJelly * (int)time;

        if (Input.GetMouseButtonDown(0))
        {
            GetJelly();
        }
    }

    private void GetJelly()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider==null)
            return;

        if (hit.transform.gameObject== gameObject)
        {
            Debug.Log($"È¹µæÇÑ Á©¸®´Â {jellyResource}ÀÔ´Ï´Ù");
            MyResourceData.Instance.GetJellyToMine(jellyResource);
            time = 0f;
        }
    }
}
