using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectProperty : MonoBehaviour
{
    protected Camera mainCamera;
    protected string sceneName;
    LayerMask tileMask;
    protected Canvas canvas;
    float timer;
    protected int touchCount;
    protected ObjectInformation objectInfScene;

    protected void Start()
    {
        //���� ��ġ ������ �޾ƿ´� 
        touchCount = Input.touchCount;

        mainCamera = Camera.main;
        sceneName = SceneManager.GetActiveScene().name;
        tileMask = 1<<LayerMask.NameToLayer("Floor");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        if (sceneName.Equals("TownScene"))
            objectInfScene = GameObject.Find("ObjectInformation").GetComponent<ObjectInformation>();
    }

    protected void OnMouseDown()
    {
        //����Ŭ���ú��� �ð������� ���� �ʱ�ȭ
        timer = 0;
    }

    protected void OnMouseUp()
    {
        //��ü�� �巡�� ���� �ʾҴٸ�(OnMouseDrag() �ڵ�Ȯ��)
        if (!PinchZoom.Instance.isObjectMove)
            objectInfScene.ChildSetActive();

        PinchZoom.Instance.isObjectMove = false;
    }

    protected void OnMouseDrag()
    {
        timer += Time.deltaTime;

        if (!sceneName.Equals("TownScene") || timer<0.2f || touchCount >1)
            return;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (!hit.transform.gameObject == gameObject)
            return;

        if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
        {
            transform.position = hit.transform.position;
            PinchZoom.Instance.isObjectMove = true;
        }
    }
}
