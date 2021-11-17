using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTownButton : MonoBehaviour
{
    public void ReturnTown()
    {
        SceneManager.LoadScene("TownScene");
    }
}