using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : Singletone<LoadSceneManager>
{
    [SerializeField] LoadingSceneUI loadUI;
    string nextScene;

    private void Start()
    {
        loadUI.progressBar.fillAmount = 0f;
    }
    public void LoadScene(string sceneName) 
    {
        loadUI.gameObject.SetActive(true);
        nextScene = sceneName;
        StartCoroutine(LoadScene());
        loadUI.gameObject.SetActive(false);
    }

    IEnumerator LoadScene() {
        yield return null; 
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); 
        op.allowSceneActivation = false; 
        float timer = 0.0f; 
        
        while (!op.isDone) { 
            yield return null; 
            timer += Time.deltaTime;

            if (op.progress < 0.9f) {
                loadUI.progressBar.fillAmount = Mathf.Lerp(loadUI.progressBar.fillAmount, op.progress, timer); 
                if (loadUI.progressBar.fillAmount >= op.progress) { 
                    timer = 0f; 
                } 
            } 
            else {
                loadUI.progressBar.fillAmount = Mathf.Lerp(loadUI.progressBar.fillAmount, 1f, timer); 
                if (loadUI.progressBar.fillAmount == 1.0f) { 
                    op.allowSceneActivation = true;

                    yield break; 
                } 
            }
        }
        SceneManager.LoadScene(nextScene);
    }
}
