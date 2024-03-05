using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    private int _enemyAmount;
    private bool _hasMoved = false;
    private Vector3 _blobSize; 

    public Vector3 BlobSize
    {
        get => _blobSize;
        set => _blobSize = value;
    }

    public PieceState pieceState;
    public PieceType pieceType;
    public GridManager gridManager;

    private void Start()
    {
        if (pieceType == PieceType.JustABlob)
        {
            _blobSize = transform.localScale;
        }

        gridManager = FindObjectOfType<GridManager>();
    }
    public void Take()
    {
        gameObject.SetActive(false);
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

    public void MoveTo(PieceType type, Tile tile)
    {
        if(type == PieceType.JustABlob)
        {
            Vector3 newPosition = tile.transform.position + new Vector3(1, 0, 1);
            transform.DOMove(newPosition, 1f).SetEase(Ease.Linear);
            transform.DOJump(newPosition, 100f, 1, 1, true).SetEase(Ease.InElastic);
        }

        if (type == PieceType.Car)
        {
            transform.DOMove(tile.transform.position, 1, true).SetEase(Ease.Linear);
        }
    }

    public void ChangeColor(PieceType type,  Color color)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            switch (type)
            {
                case PieceType.Car:
                    renderer.material.color = Color.green;
                    break;
                case PieceType.Building:
                    renderer.material.color = Color.blue;
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


}
