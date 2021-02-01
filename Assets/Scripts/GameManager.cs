using System;
using Cycles;
using DG.Tweening;
using Events;
using Player;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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
    public int cyclesNum { get; private set; }
    
    [SerializeField] private TMP_Text msg;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color originColor;
    private Tween tween;
    private Vector3 originScale;

    
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
            if (sheepSettings.sheeps.Count == 1) Lose();
        });
        onPlayerDeath.Register(gameObject, o => Lose());
        
        Physics2D.queriesHitTriggers = true;
        // Camera.main.eventMask = 1 << LayerMask.NameToLayer("Selectable") | 1 << LayerMask.NameToLayer("UI");
        // TODO: Check if things still good without this line ^^^^^^
        
        InitializeUI();
        Time.timeScale = 0;
        CyclesManager.Instance.DaySettings.OnCycleStart.Register(gameObject, AddCycle);
        originScale = msg.transform.localScale;
        msg.transform.localScale = Vector3.zero;
        
    }

    public void StartGame()
    {
        onStartGame.Raise();
        Time.timeScale = 1;
        IsPlaying = true;
    }

    public void Lose()
    {
        Time.timeScale = 0; // TODO slow down slowly until 0??
        RaiseDeathWindow();
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
    [SerializeField] private GameObject deathWindow;
    [SerializeField] private TMP_Text numOfCycles;
    [SerializeField] private TMP_Text numOfWoods;
    [SerializeField] private TMP_Text numOfRocks;
    
    
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
        
        pauseMenu.GetComponent<PauseMenu>().InitReferences(RestartGame, SetPauseMenu);
    }

    private void SetPauseMenu()
    {
        if (!IsPlaying) return;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        GameEvent trigger = pauseMenu.activeSelf ? triggerBlur : triggerUnblur;
        mainUI.SetActive(!mainUI.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        trigger.Raise();
    }

    private void RaiseDeathWindow()
    {
        mainUI.SetActive(false);
        triggerBlur.Raise();
        deathWindow.SetActive(true);
        numOfCycles.text = cyclesNum.ToString();
        numOfRocks.text = PlayerController.CurrentInventory.GetFinalRocks.ToString();
        numOfWoods.text = PlayerController.CurrentInventory.GetFinalWoods.ToString();
    }

    #endregion

    private void AddCycle(object o)
    {
        cyclesNum++;
    }

    public void DisplayMsg(String message)
    {
        msg.text = message;
        tween?.Kill(true);
        tween = DOTween.Sequence()
            // .Append(msg.DOFade(1, duration).SetDelay(2f)).Append(msg.DOFade(0, duration));
            .Append(msg.transform.DOScale(originScale, duration).SetEase(ease))
            // .Append(msg.DOColor(targetColor, duration).SetEase(ease))
            // .Append(msg.DOColor(originColor, duration).SetDelay(2f));
            .Append(msg.transform.DOScale(0, duration).SetDelay(1.3f));

        // tween = DOTween.To(msg.alpha, value => msg.alpha = value, 0, duration);
    }

    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(Screen.width/2-40, 5, 80, 30), $"Load: {waitingList.Count(b=>b)} / {waitingList.Count}" );
    // }
}
