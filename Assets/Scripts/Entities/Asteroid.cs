using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {    

    private Rigidbody2D m_rigidBody;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    public Rigidbody2D RigidBody
    {
        get { return m_rigidBody; }
    }
}
