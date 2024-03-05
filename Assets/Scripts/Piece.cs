using UnityEngine;
using UnityEngine.UI;

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
}
