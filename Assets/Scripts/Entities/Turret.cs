using UnityEngine;
using System.Collections;

/**
    doesn't have any functionality except notifying the missile manager of its existence
    so that the reference doesn't have to be placed in editor
*/
public class Turret : MonoBehaviour {

    [SerializeField]
    private GameObject m_missilePrefab;

    // the missile that is currently underway
    private Missile m_flyingMissile;

    void Awake()
    {
        EventManager.Connect<GameEvents.ScreenTapped>(OnScreenTapped);
    }

    void OnDestroy()
    {
        EventManager.Disconnect<GameEvents.ScreenTapped>(OnScreenTapped);
    }

    private void OnScreenTapped(GameEvents.ScreenTapped data)
    {
        if (m_flyingMissile != null)
        {
            m_flyingMissile.Detonate();
            m_flyingMissile = null;
        }
        else
        {
            var missile = Instantiate(m_missilePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            m_flyingMissile = missile.GetComponent<Missile>();
            m_flyingMissile.StartFlying(data.m_touchPosition);
        }
    }

}
