using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Databox;
using System.Globalization;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    PlayerInput input;

    DungeonGenerator generator;

    [SerializeField]
    DataboxObjectManager databaseManager;

    GameState state;

    SaveSlot currentSave;

    public GameState State
    {
        get
        {
            return state;
        }
        set 
        {
            if (state != value)
            {
                state = value;
                OnStateChanged.Invoke();
               

                
                if (IsInGame)
                {
                    input.SwitchCurrentActionMap("Player");
                } else if (state == GameState.Paused)
                {
                    input.SwitchCurrentActionMap("UI");
                }
            }
        }
    }

    public UnityEvent OnStateChanged
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DatabaseManager.Instance = new DatabaseManager(databaseManager);
            OnStateChanged = new UnityEvent();
            SceneManager.sceneLoaded += OnSceneLoad;
            DontDestroyOnLoad(gameObject);


        } else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(SaveSlot slot = null)
    {
        currentSave = slot;
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToLoadScreen()
    {
        SceneManager.LoadScene("LoadScreen");
        state = GameState.LoadScreen;
    }

    public void GoToStartMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToDeathScreen()
    {
        if (currentSave)
        {
            currentSave.PlayerDeaths++;
            currentSave.Save();
        }
        SceneManager.LoadScene("Death Screen");
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            if (state != GameState.Resetting)
            {
                state = GameState.InGame;
                Player = FindObjectOfType<Player>();
                Player.Init();
                Player.OnDeath.AddListener(GoToDeathScreen);
                input = Player.GetComponent<PlayerInput>();
                UIManager.Instance.Initialize();

                generator = new DungeonGenerator();
                generator.GenerateDungeon();
            } else
            {
                generator.ClearDungeon();
                generator.GenerateDungeon();
                Player.Heal(100);
            }
        }
    }

    void ResetGame()
    {
        generator.ClearDungeon();
        generator.GenerateDungeon();
        Player.Heal(100);
    }
    // Update is called once per frame
    void Update()
    {
        TimerManager.Instance.Update(Time.deltaTime);       
    }

    public void WinGame()
    {
        Timer winTimer = new Timer(1.0f);
        winTimer.OnComplete.AddListener(() =>
        {
            SceneManager.LoadScene("WinMenu");
        });
        winTimer.Start();
    }

    public bool IsInGame
    {
        get
        {
            return state == GameState.InGame;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

public enum GameState
{
    StartMenu,
    LoadScreen,
    InGame,
    Paused,
    ShowingMessage,
    DeathScreen,
    UpgradeScreen,
    Resetting
}
