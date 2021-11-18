using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] dontDestory;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
