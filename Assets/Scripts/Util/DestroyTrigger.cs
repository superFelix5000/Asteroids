using UnityEngine;
using System.Collections;

/**
    Destroys objects either on trigger enter or exit.
    Useful for making sure objects don't fly / fall into infinity.
*/
public class DestroyTrigger : MonoBehaviour {

    [SerializeField]
    private bool m_triggerEnter;

    [SerializeField]
    private bool m_triggerExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(m_triggerEnter)
        {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(m_triggerExit)
        {
            Destroy(other.gameObject);
        }
    }

}
