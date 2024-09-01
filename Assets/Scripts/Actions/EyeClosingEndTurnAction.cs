using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EyeClosingEndTurnAction : EndTurnAction
{
    public List<Eye> Eyes = new List<Eye>();

    public EyeClosingEndTurnAction() : base()
    {
    }

    public EyeClosingEndTurnAction(PlayerData playerData) : base(playerData)
    {
    }
}
