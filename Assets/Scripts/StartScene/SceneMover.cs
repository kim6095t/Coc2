using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene("TownScene");
    }
}