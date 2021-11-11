using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        Debug.Log(!EventSystem.current.IsPointerOverGameObject());
    }
}
