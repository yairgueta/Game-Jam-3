using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;

    public void Play()
    {
        FindObjectOfType<SoundController>().TurnMenuMusicOff();
        tutorial.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
