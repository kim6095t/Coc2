using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JellyMine : ObjectProperty
{
    [SerializeField] Text getResourceText;
    float time;
    int perSecondGetJelly;
    int maxAmount;
    int jellyResource;

    private void Start()
    {
        base.Start();
        perSecondGetJelly = int.Parse(csvDatas[nowLevel]["Output"]);
        maxAmount = int.Parse(csvDatas[nowLevel]["MaxAmount"]);
    }

    private void Update()
    {
        time += Time.deltaTime;
        jellyResource=perSecondGetJelly * (int)time;

        //UI가 열려있으면 안되며 멀티터치가 아닐 때
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0) && touchCount<2)
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
            Debug.Log($"획득한 젤리는 {jellyResource}입니다");
            MyResourceData.Instance.GetJellyToMine(jellyResource);
            time = 0f;
            SetFloating(gameObject, jellyResource.ToString());
        }
    }

    private void SetFloating(GameObject vr, string getResource)
    {
        FloatingText Ftxt;

        getResourceText.color = new Color(0.5f,0.5f,0.5f);
        GameObject TXT = Instantiate(getResourceText.gameObject);

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(vr.transform.position);

        uiPosition.y += 50f;
        TXT.transform.localPosition = uiPosition;
        TXT.transform.SetParent(canvas.gameObject.transform);
        Ftxt = TXT.gameObject.transform.GetComponent<FloatingText>();
        Ftxt.print(getResource);
    }
}
