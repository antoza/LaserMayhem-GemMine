using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;
using System.Linq;

#nullable enable
public class BoardManagerShredder : BoardManager
{
    public static new BoardManagerShredder Instance => (BoardManagerShredder)BoardManager.Instance;

    [field: SerializeField]
    public int ConveyorHeight { get; private set; }
    [field: SerializeField]
    public int ConveyorWidth { get; private set; }

    private List<ConveyorBoardTile> _conveyors = new List<ConveyorBoardTile>();
    private List<ConveyorBoardTile> _topConveyors = new List<ConveyorBoardTile>();
    private List<BoardTile> _hiddenSpawnTiles = new List<BoardTile>();
    private List<InvisibleBoardTile> _shreddingTiles = new List<InvisibleBoardTile>();

    public void OperateConveyor()
    {
        foreach (ConveyorBoardTile tile in _topConveyors)
        {
            tile.ConveyPiece();
        }
        UIManagerGame.Instance.OperateConveyor();
    }

    public void SpawnOnTopConveyors(List<PieceName> pieces)
    {
        Assert.AreEqual(pieces.Count, ConveyorWidth);
        for (int i = 0; i < ConveyorWidth; i++)
        {
            _hiddenSpawnTiles[i].InstantiatePiece(pieces[i]);
            StartCoroutine(AnimateSpawnCoroutine(i));
        }
    }

    private IEnumerator AnimateSpawnCoroutine(int i)
    {
        yield return null;
        _topConveyors[i].Piece = _hiddenSpawnTiles[i].Piece;
        _topConveyors[i].Piece?.GetComponent<Animator>().SetTrigger("PieceLanding");
    }

    public IEnumerable<Gem> GetAllCrystals()
    {
        foreach(ConveyorBoardTile tile in _conveyors)
        {
            if (tile.Piece is Gem) yield return (Gem)tile.Piece;
        }
    }

    public IEnumerable<TNT> GetAllBombs()
    {
        foreach(ConveyorBoardTile tile in _conveyors)
        {
            if (tile.Piece is TNT) yield return (TNT)tile.Piece;
        }
    }

    public IEnumerable<Piece> GetOrbsOnShreddingTiles() {
        foreach (InvisibleBoardTile tile in _shreddingTiles)
        {
            if (tile.Piece is Gem) yield return (Gem)tile.Piece;
        }
    }

    public void ShredPiecesOnShreddingTiles()
    {
        for (int i = 0; i < ConveyorWidth; i++)
        {
            Piece? oldPiece = _shreddingTiles[i].Piece;
            if (oldPiece != null)
            {
                _shreddingTiles[i].Piece = null;
                Destroy(oldPiece!);
            }
        }
    }

    protected override void GenerateAllTiles()
    {
        int centerX = ConveyorWidth / 2;
        int centerY = ConveyorHeight / 2 - 3;

        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        List<BoardTile> conveyors = GenerateBoardTilesInArea(-centerX, -centerX + ConveyorWidth - 1, -centerY, -centerY + ConveyorHeight, TileName.ConveyorBoardTile, boardTilesParent);
        foreach (BoardTile conveyor in conveyors) _conveyors.Add((ConveyorBoardTile)conveyor);
        for (int i = -centerX; i < -centerX + ConveyorWidth; i++) _topConveyors.Add((ConveyorBoardTile)GetBoardTile(new Vector2Int(i, -centerY + ConveyorHeight))!);

        _hiddenSpawnTiles = GenerateBoardTilesInArea(-centerX, -centerX + ConveyorWidth - 1, -centerY + ConveyorHeight + 100, -centerY + ConveyorHeight + 100, TileName.NormalBoardTile, boardTilesParent);

        GameObject shreddingTilesParent = new GameObject("ShreddingTiles");
        shreddingTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-centerX, -centerX + ConveyorWidth - 1, -centerY - 1, -centerY - 1, TileName.InvisibleBoardTile, shreddingTilesParent);
        for (int i = -centerX; i < -centerX + ConveyorWidth; i++) _shreddingTiles.Add((InvisibleBoardTile)GetBoardTile(new Vector2Int(i, -centerY - 1))!);

        GameObject invisibleTilesParent = new GameObject("InvisibleTiles");
        invisibleTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-10, -centerX - 1, -centerY, -centerY + ConveyorHeight, TileName.InvisibleBoardTile, invisibleTilesParent);
        GenerateBoardTilesInArea(-centerX + ConveyorWidth, 10, -centerY, -centerY + ConveyorHeight, TileName.InvisibleBoardTile, invisibleTilesParent);
        GenerateBoardTile(0, -centerY - 2, TileName.InvisibleBoardTile, invisibleTilesParent);
        GenerateBoardTile(0, -centerY - 3, TileName.InvisibleBoardTile, invisibleTilesParent);

        BoardTile laserGeneratorTile = GenerateBoardTile(0, -centerY - 4, TileName.InvisibleBoardTile, _boardParent);
        laserGeneratorTile.InstantiatePiece(PieceName.LaserEmitter);
        laserGeneratorTile.Piece!.RotateAnticlockwise();
    }
}
