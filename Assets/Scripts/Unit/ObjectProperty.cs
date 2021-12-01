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

    protected void Start()
    {
        mainCamera = Camera.main;
        sceneName = SceneManager.GetActiveScene().name;
        tileMask = 1<<LayerMask.NameToLayer("Floor");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    protected void OnMouseDrag()
    {
        if (!sceneName.Equals("TownScene"))
            return;

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
