using System.Collections.Generic;
using UnityEngine;
namespace Selection
{
    public class SelectionManager
    {
        public List<Tile> GetTilesNormalMovement(Tile center, GridManager gridManager)
        {
            List<Tile> foundtiles = new List<Tile>();

            foreach (var VARIABLE in _directions)
            {
                gridManager.TryGetTileAt(center.transform.position + VARIABLE, out Tile tile);
            }

            return foundtiles;
        }
        
        
        static public Vector3[] _directions =
            new Vector3[4]
            {
                new Vector3(1, 0,  0), new Vector3(1,0, -1), new Vector3(0,0, -1),
                new Vector3(-1, 0, 0)
            };
        
    }
    
    
}
