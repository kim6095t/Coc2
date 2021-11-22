using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldMine : MonoBehaviour
{
    Camera mainCamera;
    float time;
    int perSecondGetGold;
    int goldResource;

    string sceneName;

    private void Start()
    {
        mainCamera = Camera.main;
        perSecondGetGold = 10;
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (!sceneName.Equals("TownScene"))
            return;


        time += Time.deltaTime;
        goldResource = perSecondGetGold * (int)time;

        if (Input.GetMouseButtonDown(0))
        {
            GetGold();
        }

        
    }

    private void OnMouseDrag()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.transform.gameObject == gameObject)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 transPos = mainCamera.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(transPos.x, transform.position.y, transPos.z);
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
