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

    protected void Start()
    {
        //현재 터치 개수를 받아온다 
        touchCount = Input.touchCount;

        mainCamera = Camera.main;
        sceneName = SceneManager.GetActiveScene().name;
        tileMask = 1<<LayerMask.NameToLayer("Floor");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    protected void OnMouseDown()
    {
        timer = 0;
    }

    protected void OnMouseUp()
    {
        Debug.Log(PinchZoom.Instance.isObjectMove);
        PinchZoom.Instance.isObjectMove = false;
    }

    protected void OnMouseDrag()
    {
        timer += Time.deltaTime;

        if (!sceneName.Equals("TownScene") || timer<0.2f || touchCount >1)
            return;

        PinchZoom.Instance.isObjectMove = true;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (!hit.transform.gameObject == gameObject)
            return;

        if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
        {
            transform.position = hit.transform.position;
        }
    }


}
