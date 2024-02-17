using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishedEvent : GameEvent
{
    public bool playerWon = false;

    public GameFinishedEvent(bool playerWon)
    {
        this.playerWon = playerWon;
    }
}
