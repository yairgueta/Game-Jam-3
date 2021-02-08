using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    private void Start()
    {
        StartCoroutine(LoadScenes());
    }

    IEnumerator LoadScenes()
    {
        var asyncLoadInitScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        var asyncLoadMainScene = SceneManager.LoadSceneAsync(2);
        
        while (!asyncLoadInitScene.isDone || !asyncLoadMainScene.isDone)
        {
            yield return null;
            slider.value = (asyncLoadInitScene.progress + asyncLoadMainScene.progress) / 2f;
        }
    }
}
