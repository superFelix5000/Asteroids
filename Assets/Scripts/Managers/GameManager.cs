using UnityEngine;
using System.Collections;
using System;

public class GameManager : SingletonMonobehaviour<GameManager> {

    private enum InputMode
    {
        Normal,
        AI
    }

    private InputMode m_inputMode;

    [Serializable]
    private class InputModule
    {
        public InputMode m_inputMode = InputMode.Normal;
        public GameObject m_inputManager = null;
    }

    [SerializeField]
    private InputModule[] m_inputModules;

    // how many points did the player get
    private int m_playerPoints;

    protected override void Awake()
    {
        base.Awake();

        // game
        EventManager.Connect(GameEvents.SimpleEvent.AsteroidHitGround, OnGroundHit);

        // ui
        EventManager.Connect(GameEvents.SimpleEvent.StartGameClicked, OnStartGameNormal);
        EventManager.Connect(GameEvents.SimpleEvent.StartGameAIClicked, OnStartGameAI);
        EventManager.Connect(GameEvents.SimpleEvent.ExitGameClicked, OnExitGameClicked);
        EventManager.Connect(GameEvents.SimpleEvent.BackToMenuClicked, OnBackToMenu);
        EventManager.Connect(GameEvents.SimpleEvent.RestartGameClicked, OnRestart);
        EventManager.Connect(GameEvents.SimpleEvent.AsteroidDestroyed, OnAsteroidDestroyed);
    }

    private void OnAsteroidDestroyed()
    {
        ++m_playerPoints;
        EventManager.Send<GameEvents.PointsUpdated>(new GameEvents.PointsUpdated(m_playerPoints));
    }

    private void OnRestart()
    {
        Application.LoadLevel("level1");
    }

    private void OnBackToMenu()
    {
        EventManager.Send(GameEvents.SimpleEvent.LevelStopped);
        Application.LoadLevel("main");
        MenuManager.Instance.ShowMainMenu();
    }

    private void OnStartGameNormal()
    {        
        m_inputMode = InputMode.Normal;
        Application.LoadLevel("level1");
    }

    private void OnStartGameAI()
    {
        m_inputMode = InputMode.AI;
        Application.LoadLevel("level1");        
    }

    private void OnExitGameClicked()
    {
        Application.Quit();
    }

    private void OnGroundHit()
    {
        Time.timeScale = 0;
        EventManager.Send(GameEvents.SimpleEvent.LevelStopped);
        MenuManager.Instance.ShowGameOver();
    }

    private GameObject GetPrefabForPlayerMode(InputMode playerMode)
    {
        for(int i = 0; i < m_inputModules.Length; i++)
        {
            if(m_inputModules[i].m_inputMode == playerMode)
            {
                return m_inputModules[i].m_inputManager;
            }
        }
        return null;
    }

    void OnLevelWasLoaded(int level)
    {
        if(Application.loadedLevelName == "level1")
        {
            Instantiate(GetPrefabForPlayerMode(m_inputMode));
            MenuManager.Instance.ShowInamgeUI();
            m_playerPoints = 0;
            EventManager.Send(GameEvents.SimpleEvent.LevelStarted);
            Time.timeScale = 1;
        }
        else if(Application.loadedLevelName == "main")
        {
            MenuManager.Instance.ShowMainMenu();
        }
    }

}
