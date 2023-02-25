using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/**
    AI observing incoming asteroids and signaling the turret where to shoot and when to let the missile explode.
    Keeps queue of all incoming asteroids from where the next one is picked after the previous one has been destroyed.
    Shoots a bit ahead of the asteroid and lets the missile explode there to let the explosion take care of the asteroid,
    not letting the missile and asteroid itself collide
*/
public class AIInputManager : MonoBehaviour {

    private bool m_running;

    private bool m_missileLock;    // can a new rocket be launched?
    private Asteroid m_currentAsteroid;
    Queue<Asteroid> m_incomingAsteroids = new Queue<Asteroid>();

    void Awake()
    {
        EventManager.Connect(GameEvents.SimpleEvent.LevelStopped, OnLevelStopped);
        EventManager.Connect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
        EventManager.Connect<GameEvents.AsteroidIncoming>(OnAsteroidIncoming);
    }

    void OnDestroy()
    {
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStopped, OnLevelStopped);
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
        EventManager.Disconnect<GameEvents.AsteroidIncoming>(OnAsteroidIncoming);
    }

    private void OnAsteroidIncoming(GameEvents.AsteroidIncoming ev)
    {
        m_incomingAsteroids.Enqueue(ev.m_asteroid);        
    }

    private IEnumerator ShootingLoop()
    {
        while(m_running)
        {
            if (!m_missileLock && m_incomingAsteroids.Count > 0)
            {
                m_missileLock = true;

                // skip null elements in queue if asteroids have been destroyed through other missile
                while(m_currentAsteroid == null && m_incomingAsteroids.Count > 0)
                {
                    m_currentAsteroid = m_incomingAsteroids.Dequeue();
                }

                StartCoroutine(ShootMissile());
            }
            yield return null;
        }
    }

    private IEnumerator ShootMissile()
    {
        /* 
            motion of target: p + v*t
            target = asteroid position
            vel = asteroid velocity vector
            s = missile speed
        */

        var target = (Vector2)m_currentAsteroid.transform.position;
        var vel = m_currentAsteroid.RigidBody.velocity;
        var s = 3.0f;

        // shoot a bit ahead of the asteroid and let the rocket explode there
        var colTime = GetCollisionTime(target, vel * 1.5f,s);
        var colPoint = target + vel * 1.5f * colTime;

        // shoot missile
        EventManager.Send<GameEvents.ScreenTapped>(new GameEvents.ScreenTapped(colPoint));

        var asteroidToDestroy = m_currentAsteroid;
        float detonationTimeElapsed = 0.0f;

        while(asteroidToDestroy != null && detonationTimeElapsed < colTime)
        {
            detonationTimeElapsed += Time.deltaTime;
            yield return null;
        }

        // let missile explode
        EventManager.Send<GameEvents.ScreenTapped>(new GameEvents.ScreenTapped(colPoint));
        m_currentAsteroid = null;
        m_missileLock = false;
    }
    /**
        quadratic equation solved through quadratic formula
        Initial thought: target + time * targetVelocity = time * bulletSpeed
    */
    private float GetCollisionTime(Vector2 target, Vector2 vel, float s)
    {
        float a = s * s - (vel.x * vel.x + vel.y * vel.y);
        float b = target.x * vel.x + target.y * vel.y;
        float c = target.x * target.x + target.y * target.y;

        if (a.CompareTo(0.0f) == 0)
            return 0.0f;

        var d = b*b + a*c;
        var t1 = (b + Mathf.Sqrt(d)) / a;

        return t1;
    }

    private void OnLevelStopped()
    {
        m_running = false;
        m_incomingAsteroids.Clear();
    }

    private void OnLevelStarted()
    {
        m_running = true;        
        StartCoroutine(ShootingLoop());
    }
}
