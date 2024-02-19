using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StateAction
{
    public State state;
    public List<UnityEvent> callbacks = new List<UnityEvent>();
}

public class StateListener : MonoBehaviour
{
    [SerializeField]
    List<StateAction> stateActions = new List<StateAction>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < stateActions.Count; i++)
        {
            for (int j = 0; j < stateActions[i].callbacks.Count; j++)
            {
                if (stateActions[i].callbacks[j].GetPersistentMethodName(0) != "")
                {
                    StateMachine.Instance?.AddListener(stateActions[i].state, stateActions[i].callbacks[j]);
                }
                else
                {
                    Debug.Log("GameObject " + gameObject.name + " is missing a callback on his " + stateActions[i].state.ToString() + " state.");
                }
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < stateActions.Count; i++)
        {
            for (int j = 0; j < stateActions[i].callbacks.Count; j++)
            {
                StateMachine.Instance?.RemoveListener(stateActions[i].state, stateActions[i].callbacks[j]);
            }
        }
    }
}
