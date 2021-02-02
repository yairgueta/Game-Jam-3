using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    private Camera mainCam;
    private int counter;
    private TMP_Text TMPtext;
    private StringBuilder sb;
    private string dot;
    
    private void Start()
    {
        mainCam = Camera.current;
        TMPtext = GetComponent<TMP_Text>();
        sb = new StringBuilder(TMPtext.text);
        dot = ".";

        StartCoroutine(LoadScenes());
    }

    IEnumerator LoadScenes()
    {
        var asyncLoadInitScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        var asyncLoadMainScene = SceneManager.LoadSceneAsync(2);
        
        while (!asyncLoadInitScene.isDone || !asyncLoadMainScene.isDone)
        {
            yield return null;
            var factor = Mathf.FloorToInt((asyncLoadInitScene.progress * asyncLoadMainScene.progress) * 5);
            if (factor >= counter)
            {
                sb.Append(dot);
                TMPtext.text = sb.ToString();
                counter++;
            }
        }
    }
}
