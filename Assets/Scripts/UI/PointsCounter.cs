using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/*
    Updating player points ingame UI 
*/
public class PointsCounter : MonoBehaviour {

    [SerializeField]
    private Text m_pointsText;

    void Awake()
    {
        EventManager.Connect<GameEvents.PointsUpdated>(OnPointsUpdated);
        EventManager.Connect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
    }

    private void OnLevelStarted()
    {
        m_pointsText.text = "0";
    }

    void OnDestroy()
    {
        EventManager.Disconnect<GameEvents.PointsUpdated>(OnPointsUpdated);
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
    }

    private void OnPointsUpdated(GameEvents.PointsUpdated ev)
    {
        m_pointsText.text = ev.m_newPointAmount.ToString();
    }
}
