using System;
using Cycles;
using DG.Tweening;
using Events;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameEvent ONFinishLoading => onFinishLoading;
    public GameEvent ONStartGame => onStartGame;
    public bool IsPlaying { get; private set; }
    
    
    [SerializeField] private GameEvent onFinishLoading;
    [SerializeField] private GameEvent onStartGame;
    [SerializeField] private GameEvent onLose;
    [SerializeField] private SheepSettings sheepSettings;
     
    [Header("Lose Cases Events")]
    [SerializeField] private GameEvent onSheepDeath;
    [SerializeField] private GameEvent onPlayerDeath;

    public UIManager UIManagerInstance;
    private WaitingList waitingList;
    
    protected override void Awake()
    {
        base.Awake();
        waitingList = new WaitingList(()=>onFinishLoading.Raise());
    }
    
    private void Start()
    {
        onSheepDeath.Register(gameObject, o =>
        {
            if (sheepSettings.sheeps.Count == 1) Lose();
        });
        onPlayerDeath.Register(gameObject, o => Lose());
        
        Physics2D.queriesHitTriggers = true;
        // Camera.main.eventMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << LayerMask.NameToLayer("UI");
        // TODO: Check if things still good without this line ^^^^^^
        
        Time.timeScale = 0;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            UIManagerInstance.SetPauseMenu();
        }
    }
    
    public void StartGame()
    {
        onStartGame.Raise();
        Time.timeScale = 1;
        IsPlaying = true;
    }

    public void Lose()
    {
        onLose.Raise();
        Time.timeScale = 0; // TODO slow down slowly until 0??
        UIManagerInstance.RaiseLoseScreen();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        waitingList.Clear();
    }
    
    // Return a function to run when finished the task!
    public Action RegisterToWaitingList() => waitingList.Register();
    
    public bool FinishedLoading => waitingList.AreAllDone;
    
    
}
