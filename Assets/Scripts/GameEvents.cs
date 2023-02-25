using UnityEngine;
using System.Collections;

public class GameEvents {
    
    public enum SimpleEvent
    {
        //UI
        StartGameClicked,
        StartGameAIClicked,
        ExitGameClicked,
        BackToMenuClicked,
        RestartGameClicked,

        // Game
        AsteroidHitGround,
        AsteroidDestroyed,
        LevelStarted,
        LevelStopped
    }

    public class ScreenTapped : EventManager.Event
    {
        public readonly Vector3 m_touchPosition;

        public ScreenTapped(Vector3 touchPosition)
        {
            m_touchPosition = touchPosition;
        }
    }

    public class AsteroidIncoming : EventManager.Event
    {
        public readonly Asteroid m_asteroid;

        public AsteroidIncoming(Asteroid asteroid)
        {
            m_asteroid = asteroid;
        }
    }

    public class PointsUpdated : EventManager.Event
    {
        public readonly int m_newPointAmount;

        public PointsUpdated(int newPointsAmount)
        {
            m_newPointAmount = newPointsAmount;
        }
    }
    
}

