using System.Collections;
using UnityEngine;

/**
    Creates asteroids and lets them fly towards the ground.
    Interval is randomly chosen.
    Uses the camera frustum for start and end position calculation;
*/
public class AsteroidManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_asteroidPrefab;

    [SerializeField]
    private float m_minAsteroidInterval;

    [SerializeField]
    private float m_maxAsteroidInterval;

    [SerializeField]
    private float m_asteroidSpeedMin;

    [SerializeField]
    private float m_asteroidSpeedMax;

    private Vector2 m_startMin;
    private Vector2 m_startMax;
    private Vector2 m_goalMin;
    private Vector2 m_goalMax;

    private bool m_asteroidsFlying;

    void Awake()
    {
        var camera = Camera.main;
        var height = camera.orthographicSize * 2;
        var width = height * (float)Screen.width / (float)Screen.height;
        var camPos = camera.transform.position;

        m_startMin = new Vector2(camPos.x - width / 2, camPos.y + height / 2);
        m_startMax = new Vector2(camPos.x + width / 2, camPos.y + height / 2);
        m_goalMin = new Vector2(camPos.x - width / 2, camPos.y - height / 2);
        m_goalMax = new Vector2(camPos.x + width / 2, camPos.y - height / 2);

        EventManager.Connect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
        EventManager.Connect(GameEvents.SimpleEvent.LevelStopped, OnGameOver);
    }

    void OnDestroy()
    {
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStarted, OnLevelStarted);
        EventManager.Disconnect(GameEvents.SimpleEvent.LevelStopped, OnGameOver);
    }

    private void OnGameOver()
    {
        m_asteroidsFlying = false;
    }

    private void OnLevelStarted()
    {
        m_asteroidsFlying = true;
        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while(m_asteroidsFlying)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(Random.Range(m_minAsteroidInterval, m_maxAsteroidInterval));
        }
    }

    private void SpawnAsteroid()
    {
        var pos = new Vector2(Random.Range(m_startMin.x, m_startMax.x), Random.Range(m_startMin.y, m_startMax.y));
        var target = new Vector2(Random.Range(m_goalMin.x, m_goalMax.x), Random.Range(m_goalMin.y, m_goalMax.y));
        var asteroid = (Instantiate(m_asteroidPrefab, pos, Quaternion.identity) as GameObject).GetComponent<Asteroid>();
        asteroid.RigidBody.velocity = (target-pos).normalized * Random.Range(m_asteroidSpeedMin, m_asteroidSpeedMax);
        EventManager.Send<GameEvents.AsteroidIncoming>(new GameEvents.AsteroidIncoming(asteroid));
    }
	
}
