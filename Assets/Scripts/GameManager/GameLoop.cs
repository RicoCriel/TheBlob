using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
   GridManager gridManager;
   
   Piece currentlySelectedPiece;
   
   
   
   private void Awake()
   {
      Tile[] tiles = FindObjectsOfType<Tile>();
      Piece[] pieces = FindObjectsOfType<Piece>();

      foreach (Tile tile in tiles)
      {
         gridManager.AddTile(tile.transform.position, tile);
      }
      
      foreach (Piece piece in pieces)
      {
         gridManager.TryGetTileAt(piece.transform.position, out Tile tile);
         gridManager.AddPiece(tile, piece);
      }
      
      //add selention events
      
      //add deselect events
      
      //add move events
   }
   

   
   
   
}
