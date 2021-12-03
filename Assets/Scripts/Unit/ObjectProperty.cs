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
    public int nowLevel;
    protected Dictionary<string, string>[] csvDatas;

    [SerializeField] protected TextAsset data;


    //TownScene에서만 사용가능하게
    protected ObjectInformation objectInfScene;
    protected Text objectInfTitle;


    enum level
    {
        level1,
        level2,
        level3,

        count,
    }


    //레벨 조정해보기
    protected void Start()
    {
        nowLevel = (int)level.level3;
        //데이터 읽어오기
        csvDatas = CSVReader.ReadCSV(data);
        
        //현재 터치 개수를 받아온다 
        touchCount = Input.touchCount;

        mainCamera = Camera.main;
        sceneName = SceneManager.GetActiveScene().name;
        tileMask = 1<<LayerMask.NameToLayer("Floor");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        if (sceneName.Equals("TownScene"))
        {
            objectInfScene = GameObject.Find("ObjectInformation").GetComponent<ObjectInformation>();
            objectInfTitle= objectInfScene.transform.FindChild("objectInfTitle").GetComponent<Text>();
        }
    }

    protected void OnMouseDown()
    {
        //최초클릭시부터 시간측정을 위한 초기화
        timer = 0;
    }

    protected void OnMouseUp()
    {
        PinchZoom.Instance.isObjectMove = false;

        //물체가 드래그 되지 않았다면 정보띄우기(OnMouseDrag() 코드확인)
        if (!PinchZoom.Instance.isObjectMove && sceneName.Equals("TownScene"))
        {
            objectInfScene.ChildSetActive();
            objectInfScene.GetSettingData(csvDatas[nowLevel]);

            objectInfTitle.text = $"{ csvDatas[nowLevel]["Name"]} ({csvDatas[nowLevel]["Level"]}레벨)";
        }

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
