using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour {

    private bool m_running;

    void Awake()
    {
        EventManager.Connect(GameEvents.SimpleEvent.LevelStopped, OnLevelStopped);
        EventManager.Connect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
    }

    void OnDestroy()
    {
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStopped, OnLevelStopped);
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
    }

    private void OnLevelStopped()
    {
        m_running = false;
    }

    private void OnLevelStarted()
    {
        m_running = true;
        StartCoroutine(InputCoroutine());
    }

    private IEnumerator InputCoroutine()
    {
        while(m_running)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.Send<GameEvents.ScreenTapped>(
                    new GameEvents.ScreenTapped(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            }
            yield return null;
        }
    }

}
