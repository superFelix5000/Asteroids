using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

    [SerializeField]    
    private float m_speed;  // units per second

    private Vector2 m_direction;
    private Animation m_anim;
    private Rigidbody2D m_rigidBody;

    void Awake()
    {
        m_anim = GetComponent<Animation>();
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    public void StartFlying(Vector2 direction)
    {
        m_direction = direction.normalized;
        m_rigidBody.velocity = m_direction * m_speed;
    }   

    public void Detonate()
    {
        m_rigidBody.velocity = Vector2.zero;
        m_anim.Play();
        StartCoroutine(DestroyAfterAnim());
    }

    // missile is being destroyed when the anim is over
    private IEnumerator DestroyAfterAnim()
    {
        while(m_anim.IsPlaying("Explode"))
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetTimedDetonation(float delay)
    {
        StartCoroutine(TimedDetonation(delay));
    }

    private IEnumerator TimedDetonation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Detonate();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("Asteroid"))
        {
            Destroy(coll.gameObject);
            EventManager.Send(GameEvents.SimpleEvent.AsteroidDestroyed);
        }
    }

}
