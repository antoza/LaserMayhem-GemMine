using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : GameAction
{
    public PlayerData PlayerData;

    public PlayerAction() : base()
    {
    }
    
    public PlayerAction(PlayerData playerData) : base()
    {
        PlayerData = playerData;
    }
}