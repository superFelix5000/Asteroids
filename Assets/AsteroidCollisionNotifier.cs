using UnityEngine;
using System.Collections;

public class AsteroidCollisionNotifier : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Asteroid"))
        {
            EventManager.Send(GameEvents.SimpleEvent.AsteroidHitGround);
        }
    }

}
