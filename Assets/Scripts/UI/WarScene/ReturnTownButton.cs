using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTownButton : MonoBehaviour
{
    [SerializeField] EndGame endGameScene;
    public void ReturnTown()
    {
        endGameScene.gameObject.SetActive(false);
        LoadSceneManager.Instance.LoadScene("TownScene");
    }
}