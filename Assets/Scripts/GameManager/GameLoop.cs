using JetBrains.Annotations;
using SelectionBlob;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private GridManager gridManager;
    private SelectionManager selectionManager;


    private bool IsCurrentlySelecting;
    private Piece currentlySelectedPiece;
    private Tile currentlySelectedTile;
    private List<Tile> currentSelection;

    [SerializeField]
    private Piece BlobBaseObject;

    private void Awake()
    {
        selectionManager = new SelectionManager();
        currentSelection = new List<Tile>();

        // to be sure
        gridManager = FindObjectOfType<GridManager>();

        Tile[] tiles = FindObjectsOfType<Tile>();
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Tile tile in tiles)
        {
            var position = tile.transform.position;
            gridManager.AddTile(new Vector2(position.x, position.z), tile);

            tile.PointerEnterDelayed += (sender, args) => {
                if (gridManager.TryGetPieceAt(tile, out Piece piece))
                {
                    //start popup logic.
                }
            };

            tile.PointerExitImmediate += (sender, args) => {
                if (gridManager.TryGetPieceAt(tile, out Piece piece))
                {
                    // stop popup logic.
                }
            };

            tile.PointerClickLeft += (sender, args) => {
                tileClickLogic(tile, true);
            };

            tile.PointerClickRight += (sender, args) => {
                tileClickLogic(tile, false);
                //popup logic immidiately
            };

            // tileClickLogic(tile, ActionType.TakenObjectExecution);

        }

        foreach (Piece piece in pieces)
        {
            var transform1 = piece.transform;
            if (gridManager.TryGetTileAt(new Vector2(transform1.position.x, transform1.position.z), out Tile tile))
            {
                gridManager.AddPiece(tile, piece);
            }
        }
    }


    private void tileClickLogic(Tile tile, bool leftClick)
    {
        
        //already selected stuff
        Debug.Log("Tile Clicked");
        if (currentSelection.Contains(tile))
        {
            MovementLogic(tile, gridManager, leftClick);
            currentlySelectedPiece = null;
            currentlySelectedTile = null;
            DeselectAllTiles(currentSelection);
            currentSelection = null;
            currentSelection = new List<Tile>();
            //todo tick all the blobs for damage
            return;
        }

        if (gridManager.TryGetPieceAt(tile, out Piece piece))
        {
            currentlySelectedPiece = piece;
            currentlySelectedTile = tile;
            //try get piece
            //highlight
            currentSelection = HighlightLogic(tile, piece);
            HighlightAlTiles(currentSelection);
            //todo piece.OpenPopupImmediately();


        }


        //todo extra exception als we op een tile drukken die niet in de selectie zit, maar er is wel een selectie
        //todo -> clear cashes
    }
    private void MovementLogic(Tile ToTile, GridManager grid, bool leftClick)
    {
        if (currentlySelectedPiece.pieceType == PieceType.Car)
        {
            CarMovement(ToTile, grid);
            return;
        }

        ExecuteMovement(ToTile, grid, leftClick);
    }
    private void CarMovement(Tile toTile, GridManager grid)
    {
        grid.MovePieceModel(currentlySelectedPiece, toTile);
        List<Tile> MovementPath = selectionManager.GetAStarPath(currentlySelectedTile, toTile, gridManager);
        //move sequentially
        currentlySelectedPiece.MoveToPath(MovementPath);

        int blobSize = currentlySelectedPiece.BlobHealth;

        //call after movement execution
        DestroyPiece(currentlySelectedPiece);
        SpawnNewBlob(toTile, blobSize);
    }
    private void SpawnNewBlob(Tile toTile, int blobhealth)
    {
        Piece spawnedpiece = Instantiate(BlobBaseObject, toTile.transform.position, Quaternion.identity);
        gridManager.AddPiece(toTile, spawnedpiece);
    }
    private void DestroyPiece(Piece piece)
    {
        gridManager.RemovePiece(piece);
        piece.Take();
    }


    private void ExecuteMovement(Tile Totile, GridManager grid, bool isLeftClick)
    {
        if (grid.TryGetPieceAt(currentlySelectedTile, out Piece piece))
        {
            switch (currentlySelectedPiece.pieceType)
            {
                case PieceType.JustABlob:
                    if (isLeftClick)
                    {
                        NonSplittingMovement(Totile, grid, piece);
                    }
                    else
                    {
                        SplittingMovement(Totile, grid, piece);
                    }

                    break;

                case PieceType.Building:
                    if (isLeftClick)
                    {
                        NonSplittingMovement(Totile, grid, piece);
                        //todo kill oroginal piece if slime left.
                        // currentlySelectedPiece.pieceState = PieceState.Dead;
                    }
                    else
                    {
                        SplittingMovement(Totile, grid, piece);
                        //todo kill oroginal piece if slime left.
                        // currentlySelectedPiece.pieceState = PieceState.Dead;
                    }


                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    private void NonSplittingMovement(Tile Totile, GridManager grid, Piece piece)
    {
        grid.MovePieceModel(piece, Totile);

        if (grid.TryGetPieceAt(Totile, out Piece toPiece1))
        {
            if (toPiece1.IsForcedTakeOverAmount(currentlySelectedPiece, out var amount, out bool forcedMAx))
            {
                int newBlobHealth = currentlySelectedPiece.BlobHealth - amount;
                if (newBlobHealth > 0)
                {
                    SpawnNewBlob(currentlySelectedTile, newBlobHealth);
                }
                toPiece1.TakeOverAndIncreaseBlobHealth(amount);
            }
        }

        piece.MovetoTile(Totile);
        //to piece overname animation
    }

    private void SplittingMovement(Tile Totile, GridManager grid, Piece piece)
    {
        grid.MovePieceModel(piece, Totile);

        if (grid.TryGetPieceAt(Totile, out Piece toPiece1))
        {
            int blobhalfHealth = currentlySelectedPiece.BlobHealth / 2;

            if (toPiece1.IsForcedTakeOverAmount(currentlySelectedPiece, out var amount, out bool forcedMax))
            {
                if (forcedMax)
                {
                    if (amount >= blobhalfHealth)
                    {
                        int newBlobHealth = currentlySelectedPiece.BlobHealth - amount;
                        if (newBlobHealth > 0)
                        {
                            SpawnNewBlob(currentlySelectedTile, newBlobHealth);
                        }
                        toPiece1.TakeOverAndIncreaseBlobHealth(amount);
                    }
                }
                else
                {
                    if (amount <= blobhalfHealth)
                    {
                        amount = blobhalfHealth;
                        int newBlobHealth = currentlySelectedPiece.BlobHealth - amount;
                        if (newBlobHealth > 0)
                        {
                            SpawnNewBlob(currentlySelectedTile, newBlobHealth);
                        }
                        toPiece1.TakeOverAndIncreaseBlobHealth(amount);
                    }
                }

            }
            else
            {
                SpawnNewBlob(currentlySelectedTile, blobhalfHealth);
                toPiece1.TakeOverAndIncreaseBlobHealth(amount);
            }
        }
        else
        {
            int blobhalfHealth = currentlySelectedPiece.BlobHealth / 2;
            currentlySelectedPiece.BlobHealth = currentlySelectedPiece.BlobHealth / 2;
            currentlySelectedPiece.MovetoTile(Totile);
            SpawnNewBlob(currentlySelectedTile, blobhalfHealth);
        }
    }


    private void HighlightAlTiles(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.ChangeHoverState(true);
        }
    }

    private void DeselectAllTiles(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.ChangeHoverState(false);
        }
    }

    private List<Tile> HighlightLogic(Tile tile, Piece piece)
    {
        if (piece.pieceState == PieceState.Blobbed)
        {
            switch (piece.pieceType)
            {
                case PieceType.JustABlob:
                    return selectionManager.GetTilesNormalMovement(tile, gridManager);
                case PieceType.Car:
                    return selectionManager.GetAllTilesInRangeBFS(tile, 4, gridManager);
                case PieceType.Building:
                    return selectionManager.GetTilesNormalMovement(tile, gridManager);
                default:
                    return null;
            }
        }

        return null;
    }

    public enum ActionType
    {
        Normal,
        Split,
        TakenObjectExecution
    }
}
