using UnityEngine;
using System.Collections;

public class SimpleEventSender : MonoBehaviour {

    [SerializeField]
    private GameEvents.SimpleEvent m_event = GameEvents.SimpleEvent.StartGameClicked;

    public void Send()
    {
        EventManager.Send(m_event);
    }
	
}
