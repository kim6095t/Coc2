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

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
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
