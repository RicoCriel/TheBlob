using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAE.Commons;

public class GridManager : MonoBehaviour
{
    private BidirectionalDictionary<Vector2, Tile> Grid = new BidirectionalDictionary<Vector2, Tile>();
    private BidirectionalDictionary<Tile, Piece> Board = new BidirectionalDictionary<Tile, Piece>();

    public bool TryGetTileAt(Vector2 coord, out Tile position)
    {
        return Grid.TryGetValue(coord, out position);
    }
    
    public bool trygetCoordOfTile(Tile tile, out Vector2 coord)
    {
        return Grid.TryGetKey(tile, out coord);
    }
    
    public bool TryGetPieceAt(Tile tile, out Piece piece)
    {
        return Board.TryGetValue(tile, out piece);
    }
    
    public bool TryGetTileOfPiece(Piece piece, out Tile tile)
    {
        return Board.TryGetKey(piece, out tile);
    }


    public void AddTile(Vector2 position, Tile tile)
    {
        Grid.Add(position, tile);
    }

    public void RemoveTile(Vector2 position, Tile tile)
    {
        Grid.Remove(position);
    }
   
    
    public void Clear()
    {
        Grid.Clear();
    }
    
    public void RemoveAllTiles(Vector2 position)
    {
        Grid.Remove(position);
    }
    
    public void AddPiece(Tile tile, Piece piece)
    {
        Board.Add(tile, piece);
    }

    public void ClearPieces()
    {
        Board.Clear();
    }
    
    public void RemoveAllPieces(Tile tile)
    {
        Board.Remove(tile);
    }
}
