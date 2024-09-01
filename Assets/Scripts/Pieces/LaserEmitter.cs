using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class LaserEmitter : Piece // TODO : renommer en Emitter
{
    // TODO : permettre qqc comme �a dans l'editor : private List<Direction> _startingDirections;
    // TODO : tourner la pi�ce de la bonne rotation en fonction du laser de d�part
    protected List<Vector2Int> _startingDirections = new List<Vector2Int>() { Vector2Int.right };

    public override void RotateClockwise()
    {
        base.RotateClockwise();
        for (int i = 0; i < _startingDirections.Count; i++)
        {
            Vector2Int oldDirection = _startingDirections[i];
            _startingDirections[i] = new Vector2Int(oldDirection.y, -oldDirection.x);
        }
    }

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        return;
    }

    public void StartLaser(Laser? laser)
    {
        Assert.IsTrue(ParentTile is BoardTile);
        foreach (Vector2Int startingDirection in _startingDirections)
        {
            ((BoardTile?)ParentTile)!.TransferLaser(laser, startingDirection);
        }
    }
}
