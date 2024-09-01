using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
public class Wall : Piece
{
    public override void ReceiveLaser(Laser? laser, Vector2Int direction)
    {
        return;
    }
}