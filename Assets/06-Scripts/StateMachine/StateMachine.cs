using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum State
{
    GAME_LOADING,
    MAIN_MENU,
    GAME_PHASE,
    GAME_OVER,
    VICTORY
}

public class StateMachine : Singleton<StateMachine>
{
    [SerializeField]
    State _currentState = State.GAME_LOADING;

    Dictionary<State, List<UnityEvent>> _stateActions = new Dictionary<State, List<UnityEvent>>();

    State CurrentState => _currentState;

    private void Awake()
    {
        InitializeSingleton(true);
    }

    public void SetNewState(State newState)
    {
        _currentState = newState;

        if (_stateActions.ContainsKey(_currentState))
        {
            List<UnityEvent> actions = _stateActions[_currentState];

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i]?.Invoke();
            }
        }
    }

    public void AddListener(State state, UnityEvent eventToAdd)
    {
        if (!_stateActions.ContainsKey(state))
        {
            _stateActions.Add(state, new List<UnityEvent>());
        }

        _stateActions[state].Add(eventToAdd);
    }

    public void RemoveListener(State state, UnityEvent eventToRemove)
    {
        if (_stateActions.ContainsKey(state))
        {
            _stateActions[state].Remove(eventToRemove);
        }
    }
}
