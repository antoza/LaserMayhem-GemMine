using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class MovePieceAction : PlayerAction
{    
    public Tile SourceTile;
    public Tile TargetTile;
    public Piece? SourcePiece;
    public Piece? TargetPiece;

    public MovePieceAction() : base()
    {
    }

    public MovePieceAction(PlayerData playerData, Tile sourceTile, Tile targetTile) : base(playerData)
    {
        SourceTile = sourceTile;
        TargetTile = targetTile;
    }
}
