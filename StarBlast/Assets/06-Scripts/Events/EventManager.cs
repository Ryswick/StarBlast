﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    public bool LimitQueueProcessing = false;
    public float QueueProcessTime = 0.0f;
    private Queue _eventQueue = new Queue();

    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
    private Dictionary<System.Delegate, System.Delegate> onceLookups = new Dictionary<System.Delegate, System.Delegate>();

    private void Awake()
    {
        InitializeSingleton();
    }

    private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
    {
        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }

    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        AddDelegate<T>(del);
    }

    public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate result = AddDelegate<T>(del);

        if (result != null)
        {
            // remember this is only called once
            onceLookups[result] = del;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    public void RemoveAll()
    {
        delegates.Clear();
        delegateLookup.Clear();
        onceLookups.Clear();
    }

    public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        return delegateLookup.ContainsKey(del);
    }

    public void TriggerEvent(GameEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);

            // remove listeners which should only be called once
            foreach (EventDelegate k in delegates[e.GetType()].GetInvocationList())
            {
                if (onceLookups.ContainsKey(k))
                {
                    delegates[e.GetType()] -= k;

                    if (delegates[e.GetType()] == null)
                    {
                        delegates.Remove(e.GetType());
                    }

                    delegateLookup.Remove(onceLookups[k]);
                    onceLookups.Remove(k);
                }
            }
        }
        else
        {
            Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }
    }

    //Inserts the event into the current queue.
    public bool QueueEvent(GameEvent evt)
    {
        if (!delegates.ContainsKey(evt.GetType()))
        {
#if UNITY_EDITOR
            Debug.Log("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
#endif
            return false;
        }

        _eventQueue.Enqueue(evt);
        return true;
    }

    // Every update cycle the queue is processed, if the queue processing is limited,
    // a maximum processing time per update can be set after which the events will have
    // to be processed next update loop.
    void Update()
    {
        float timer = 0.0f;
        while (_eventQueue.Count > 0)
        {
            if (LimitQueueProcessing)
            {
                if (timer > QueueProcessTime)
                    return;
            }

            GameEvent evt = _eventQueue.Dequeue() as GameEvent;
            TriggerEvent(evt);

            if (LimitQueueProcessing)
                timer += Time.deltaTime;
        }
    }

    public void OnApplicationQuit()
    {
        RemoveAll();
        _eventQueue.Clear();
    }
}