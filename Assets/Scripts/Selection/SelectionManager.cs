using Algorythms;
using System.Collections.Generic;
using UnityEngine;
namespace SelectionBlob
{
    public class SelectionManager
    {
        //normal movement which is basically BFS range 1;
        public List<Tile> GetTilesNormalMovement(Tile center, GridManager gridManager)
        {
            List<Tile> foundtiles = new List<Tile>();

            foreach (Vector3 direction in _directions)
            {
                if (gridManager.TryGetTileAt(center.transform.position + direction, out Tile tile))
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

        //bfs
        public List<Tile> GetAllTilesInRangeBFS(Tile ceterTile, int range, GridManager gridManager)
        {
            List<Tile> neighbourStrategyBFS(Tile Ctile) => NeighbourLogic(Ctile, gridManager);

            BreathFirstSearch bfs = new BreathFirstSearch(neighbourStrategyBFS);
            return bfs.Area(ceterTile, range);
        }

        //Astar
        public List<Tile> GetAStarPath(Tile start, Tile end, GridManager gridManager)
        {
            List<Tile> neighbourStrategyBFS(Tile Ctile) => NeighbourLogic(Ctile, gridManager);

            //posible that we need to do distance that is not vertical 
            float distanceStrategy(Tile from, Tile to) => ManhattanDistance(from.transform.position, to.transform.position);

            AStartPAthFinding Astar = new AStartPAthFinding(neighbourStrategyBFS, distanceStrategy, distanceStrategy);
            //might return a hex to much at start or end.
            return Astar.Path(start, end);
        }

        //delegate helper methods
        private List<Tile> NeighbourLogic(Tile centertile, GridManager grid)
        {
            List<Tile> foundtiles = new List<Tile>();

            foreach (var VARIABLE in _directions)
            {
                if (grid.TryGetTileAt(centertile.transform.position + VARIABLE, out Tile tile))
                {
                    if (!grid.TryGetPieceAt(tile, out Piece piece))
                    // {
                        foundtiles.Add(tile);
                    // }
                }
            }

            return foundtiles;
        }

        public static float ManhattanDistance(Vector3 start, Vector3 goal)
        {
            return Mathf.Abs(start.x - goal.x) + Mathf.Abs(start.z - goal.z);
        }

        static public Vector3[] _directions =
            new Vector3[4]
            {
                new Vector3(1, 0, 0), new Vector3(1, 0, -1), new Vector3(0, 0, -1),
                new Vector3(-1, 0, 0)
            };

    }


}
