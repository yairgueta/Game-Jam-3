using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameEvent ONFinishLoading => onFinishLoading;
    public GameEvent ONStartGame => onStartGame;
    public bool IsPlaying { get; private set; }
    
    [SerializeField] private GameEvent onFinishLoading;
    [SerializeField] private GameEvent onStartGame;
    private List<bool> waitingList;

    private bool canRegisterToWaitingList = true;

    protected override void Awake()
    {
        base.Awake();
        waitingList = new List<bool>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void Start()
    {
        Physics2D.queriesHitTriggers = true;
        Camera.main.eventMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << LayerMask.NameToLayer("UI");

        canRegisterToWaitingList = false;
    }

    public void StartGame()
    {
        onStartGame.Raise();
        IsPlaying = true;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        waitingList = new List<bool>();
    }

    // Return a function to run when finished the task!
    public Action RegisterToWaitingList()
    {
        if (!canRegisterToWaitingList)
        {
            Debug.LogError("Cannot register to waiting list after game startup!");
            return null;
        }
        var i = waitingList.Count;
        waitingList.Add(false);
        return () => OnFinishedTask(i);
    }

    private void OnFinishedTask(int i)
    {
        waitingList[i] = true;
        if (!waitingList.Contains(false))
        {
            onFinishLoading.Raise();
            // StartGame();
        }
    }
}
