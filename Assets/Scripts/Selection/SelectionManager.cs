using Algorythms;
using System.Collections.Generic;
using UnityEngine;
namespace SelectionBlob
{
    public class SelectionManager
    {
        public List<Tile> GetTilesNormalMovement(Tile center, GridManager gridManager)
        {
            List<Tile> foundtiles = new List<Tile>();

            foreach (var VARIABLE in _directions)
            {
                if ( gridManager.TryGetTileAt(center.transform.position + VARIABLE, out Tile tile))
                {
                    // if (gridManager.TryGetPieceAt(tile, out Piece piece))
                    // {
                    //     if(piece.pieceType == PieceType.)
                    //         continue;
                    // }
                    foundtiles.Add(tile);
                }
            }

            return foundtiles;
        }

        public List<Tile> GetAllTilesInRange(Tile ceterTile, int range, GridManager gridManager)
        {
            List<Tile> neighbourStrategyBFS(Tile Ctile, GridManager grid) => NeighbourLogic(Ctile, grid);

            BreathFirstSearch bfs = new BreathFirstSearch(neighbourStrategyBFS);
            return bfs.Area(ceterTile, range);
        }
        
        private List<Tile> NeighbourLogic(Tile centertile, GridManager grid)
        {
            throw new System.NotImplementedException();
        }


        static public Vector3[] _directions =
            new Vector3[4]
            {
                new Vector3(1, 0,  0), new Vector3(1,0, -1), new Vector3(0,0, -1),
                new Vector3(-1, 0, 0)
            };
        
    }
    
    
}
