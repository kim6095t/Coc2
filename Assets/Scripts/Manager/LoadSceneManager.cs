using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : Singletone<LoadSceneManager>
{
    [SerializeField] LoadingSceneUI loadUI;
    string nextScene;


    public void LoadScene(string sceneName) 
    {
        loadUI.gameObject.SetActive(true);
        nextScene = sceneName;

        if(SceneManager.GetActiveScene().name.Equals("StartScene"))
            loadUI.backGround.sprite = Resources.Load<Sprite>($"LoadingSprite/StartScene");
        else
            loadUI.backGround.sprite= Resources.Load<Sprite>($"LoadingSprite/{sceneName}");

        StartCoroutine(LoadScene());
        
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals(nextScene))
            loadUI.gameObject.SetActive(false);
    }

    IEnumerator LoadScene() {
        yield return null;
        loadUI.progressBar.fillAmount = 0f;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); 
        op.allowSceneActivation = false; 
        float timer = 0.0f; 
        
        while (!op.isDone && timer < 3.5f) { 
            yield return null; 
            timer += Time.deltaTime;

            if (op.progress < 0.9f && timer < 2f) {
                loadUI.progressBar.fillAmount = Mathf.Lerp(loadUI.progressBar.fillAmount, op.progress, Time.deltaTime); 
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
