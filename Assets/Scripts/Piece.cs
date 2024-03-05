using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public enum PieceState
{
    Dead,
    Blobbed,
    Hostile
}

public enum PieceType
{
    JustABlob,
    Car,
    Building
}

public class Piece : MonoBehaviour
{
    private bool _hasMoved = false;
    private Vector3 _blobSize;

    [Header("enenmyStuff")]
    public int blobTakeoverMin = int.MinValue;
    public int blobTakeoverMax = int.MinValue;
    public int currentEnemyAmount;
    public int BlobHealth;

    public void initalizeBlob(int health)
    {
        BlobHealth = health;
        currentEnemyAmount = 0;
    }

    public Vector3 BlobSize{
        get => _blobSize;
        set => _blobSize = value;
    }

    public PieceState pieceState;
    public PieceType pieceType;
   

    private void Start()
    {
        if (pieceType == PieceType.JustABlob)
        {
            _blobSize = transform.localScale;
        }

    }
    public void Take()
    {
        gameObject.SetActive(false);
    }
    
    public void PieceDeathButModelStays()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.DOColor(Color.gray, 0.5f);
    }

   
    
    public bool IsForcedTakeOverAmount(Piece OtherBlobPiece, out int BlobhealthToTake, out bool hitMax)
    {
        int AttackingBlobHealth = OtherBlobPiece.BlobHealth;

        if (AttackingBlobHealth > blobTakeoverMax)
        {
            BlobhealthToTake = blobTakeoverMax;
            hitMax = true;
            return true;
        }
        else if (AttackingBlobHealth < blobTakeoverMin)
        {
            BlobhealthToTake = blobTakeoverMin;
            hitMax = false;
            return true;
        }
        else
        {
            BlobhealthToTake = 0;
            hitMax = false;
            return false;
        }
       
    }
    
    public void LoseBlobHealth(int damageAmount, PieceType type)
    {
        if (type == PieceType.JustABlob)
        {
            if (_blobSize.x <= damageAmount || _blobSize.y <= damageAmount || _blobSize.z <= damageAmount)
            {
                Debug.Log("The blob is dead");
                _blobSize = Vector3.zero;
            }
            else
            {
                _blobSize -= new Vector3(damageAmount, damageAmount, damageAmount);
            }
        }
    }

    public void DisplaySize(PieceState state, PieceType type)
    {
        string message = $"Piece state: {state}, Piece type: {type}, Blob size: {_blobSize}";
        Debug.Log(message);
    }

    public void MovetoTile(Tile tile)
    {
        if (pieceType == PieceType.JustABlob)
        {
            Vector3 newPosition = tile.transform.position + new Vector3(1, 0, 1);
            transform.DOMove(newPosition, 1f).SetEase(Ease.Linear);
            transform.DOJump(newPosition, 100f, 1, 1, true).SetEase(Ease.InElastic);
        }

    }

    public void MoveToPath(List<Tile> path)
    {
        StartCoroutine(PathMoveRoutine(path));
    }

    private IEnumerator PathMoveRoutine(List<Tile> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Tile tile = path[i];
            transform.DOMove(tile.transform.position, 0.5f, true).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.5f);
        }

    }

    public void ChangeColor(PieceType type)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            switch (type)
            {
                case PieceType.Car:
                    renderer.material.DOColor(Color.green, 0.5f);
                    break;
                case PieceType.Building:
                    renderer.material.DOColor(Color.blue, 0.5f);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the object.");
        }
    }


    public void TakeOverAndIncreaseBlobHealth(int blobhealthToTake)
    {
        BlobHealth += currentEnemyAmount;
        currentEnemyAmount = 0;
        pieceState = PieceState.Blobbed;
    }
}
