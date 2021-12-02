using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoldMine : ObjectProperty
{
    [SerializeField] Text getResourceText;
    float time;
    int perSecondGetGold;
    int goldResource;

    private void Start()
    {
        base.Start();
        perSecondGetGold = 10;
    }

    private void Update()
    {
        time += Time.deltaTime;
        goldResource = perSecondGetGold * (int)time;
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
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
            SetFloating(gameObject, goldResource.ToString());
        }
    }

    private void SetFloating(GameObject vr, string getResource)
    {
        FloatingText Ftxt;

        getResourceText.color = Color.yellow;
        GameObject TXT = Instantiate(getResourceText.gameObject);

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(vr.transform.position);

        uiPosition.y += 50f;
        TXT.transform.localPosition = uiPosition;
        TXT.transform.SetParent(canvas.gameObject.transform);
        Ftxt = TXT.gameObject.transform.GetComponent<FloatingText>();
        Ftxt.print(getResource);
    }
}
