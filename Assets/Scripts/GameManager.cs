using System;
using Events;
using Player;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Menu = UI.Menu;

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
    
    
    private WaitingList waitingList;

    protected override void Awake()
    {
        base.Awake();
        waitingList = new WaitingList(()=>onFinishLoading.Raise());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu();
        }
    }

    private void Start()
    {
        onSheepDeath.Register(gameObject, o =>
        {
            if (sheepSettings.sheeps.Count == 0) Lose();
        });
        onPlayerDeath.Register(gameObject, o => Lose());
        
        Physics2D.queriesHitTriggers = true;
        // Camera.main.eventMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << LayerMask.NameToLayer("UI");
        // TODO: Check if things still good without this line ^^^^^^
        
        InitializeUI();
    }

    public void StartGame()
    {
        onStartGame.Raise();
        IsPlaying = true;
    }

    public void Lose()
    {
        Time.timeScale = 0; // TODO slow down slowly until 0??
        onLose.Raise();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        waitingList.Clear();
    }
    
    // Return a function to run when finished the task!
    public Action RegisterToWaitingList() => waitingList.Register();
    
    public bool FinishedLoading => waitingList.AreAllDone;


    #region UI Management

    [Header("UI Refernces")] 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject pauseMenu;
    
    [Header("UI Triggers")]
    [SerializeField] private GameEvent triggerBlur;
    [SerializeField] private GameEvent triggerUnblur;
    
    
    private void InitializeUI()
    {
        mainMenu.SetActive(true);
        tutorial.SetActive(false);
        mainUI.SetActive(false);
        pauseMenu.SetActive(false);
        triggerBlur.Raise();

        mainMenu.GetComponent<Menu>().onClickedPlay += () =>
        {
            mainMenu.SetActive(false);
            tutorial.SetActive(true);
        };
        tutorial.GetComponent<TutorialManager>().onClickedStart += () =>
        {
            tutorial.SetActive(false);
            mainUI.SetActive(true);
            triggerUnblur.Raise();
            StartGame();
        };
    }

    private void SetPauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        GameEvent trigger = pauseMenu.activeSelf ? triggerBlur : triggerUnblur;
        mainUI.SetActive(!mainUI.activeSelf);
        trigger.Raise();
    }

    #endregion



    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(Screen.width/2-40, 5, 80, 30), $"Load: {waitingList.Count(b=>b)} / {waitingList.Count}" );
    // }
}
