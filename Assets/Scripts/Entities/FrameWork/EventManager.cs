using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
    Event Manager for handling events that carry data 
    as well as events without the need for additional data.
*/
public static class EventManager {

    public delegate void OnEvent<T>(T data) where T : Event;

    public delegate void OnSimpleEvent();

    private static Dictionary<int, OnSimpleEvent> m_simpleEventMap = new Dictionary<int, OnSimpleEvent>();

    public interface Event
    {

    }

    private class EventTypeHandler<T> where T : Event
    {
        public static OnEvent<T> handler;
    }

    public static void Connect<T>(OnEvent<T> handler) where T : Event
    {
        EventTypeHandler<T>.handler += handler;
    }

    public static void Disconnect<T>(OnEvent<T> handler) where T : Event
    {
        if(EventTypeHandler<T>.handler != null)
        {
            EventTypeHandler<T>.handler -= handler;
        }
    }

    public static void Send<T>(T data) where T : Event
    {
        OnEvent<T> handler = EventTypeHandler<T>.handler;
        if (handler != null)
        {
            handler(data);
        }
    }

    public static void Connect(GameEvents.SimpleEvent simpleEvent, OnSimpleEvent handler)
    {
        if(m_simpleEventMap.ContainsKey((int)simpleEvent))
        {
            m_simpleEventMap[(int)simpleEvent] += handler;
        } else
        {
            m_simpleEventMap.Add((int)simpleEvent, handler);
        }
    }

    public static void Disconnect(GameEvents.SimpleEvent simpleEvent, OnSimpleEvent handler)
    {
        if (m_simpleEventMap.ContainsKey((int)simpleEvent))
        {
            if(m_simpleEventMap[(int)simpleEvent] != null)
            {
                m_simpleEventMap[(int)simpleEvent] -= handler;
            }
        } else
        {
            Debug.LogWarning("trying to disconnect from nonexistent simple event");
        }
    }

    public static void Send(GameEvents.SimpleEvent simpleEvent)
    {
        if(m_simpleEventMap.ContainsKey((int)simpleEvent))
        {
            m_simpleEventMap[(int)simpleEvent]();
        }
    }

}
