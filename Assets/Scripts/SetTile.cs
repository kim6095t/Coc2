using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetTile : MonoBehaviour
{
    private GameObject onObject;        // 타일 위에 존재하는 오브젝트.

    //타일 클릭시 UI사라지게 만들기 위한 변수선언
    private ObjectInformation objectInfScene;
    private Text objectInfTitle;
    private ObjectInfButton buttonPosition;
    private string sceneName;

    public bool IsOnObject => onObject != null;


    private void Start()
    {
        FindOnObject();
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals("TownScene"))
        {
            objectInfScene = GameObject.Find("ObjectInformation").GetComponent<ObjectInformation>();

            objectInfTitle = objectInfScene.transform.Find("objectInfTitle").GetComponent<Text>();
            buttonPosition = objectInfScene.transform.Find("ButtonPosition").GetComponent<ObjectInfButton>();
        }
    }

    public void SetUnit(Unit newUnit)
    {
        newUnit.transform.position = transform.position;   // 위치 값 갱신.
        newUnit.transform.rotation = transform.rotation;   // 회전 값 갱신.
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && sceneName.Equals("TownScene")) {
            buttonPosition.gameObject.SetActive(false);
            objectInfTitle.gameObject.SetActive(false);
        }
    }

    private void FindOnObject()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, 1.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject target = hits[i].collider.gameObject;
            
            //예외처리
            if (MapManager.Instance==null)
                return;

            if (target.CompareTag(MapManager.Instance.TAG_PLAYABLE))
            {
                onObject = target;
                break;
            }
            else
                onObject = null;
        }
    }
}
