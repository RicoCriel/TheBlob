using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverDelay;
    [SerializeField] private Material hoverMat;
    [SerializeField] private Material normMat;

    private Coroutine hoverCoroutine;


  

    public void ChangeHoverState(bool state)
    {
        if (state)
        {
            GetComponent<MeshRenderer>().material = hoverMat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = normMat;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverCoroutine != null)
            // Start the coroutine to delay the pointer enter action
            hoverCoroutine = StartCoroutine(DelayedPointerEnter(hoverDelay));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop the coroutine if it's running
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        // Invoke immediate pointer exit event
        ChangeHoverState(false);
        OnPointerExitImmediate(new PointerExitEventArgs(transform.position));
    }

    private IEnumerator DelayedPointerEnter(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Raise the pointer enter event after the delay
        ChangeHoverState(true);
        OnPointerEnterDelayed(new PointerEnterEventArgs(transform.position));
    }

    public event EventHandler<PointerEnterEventArgs> PointerEnterDelayed;
    public event EventHandler<PointerExitEventArgs> PointerExitImmediate;

    protected virtual void OnPointerEnterDelayed(PointerEnterEventArgs eventargs)
    {
        EventHandler<PointerEnterEventArgs> handler = PointerEnterDelayed;
        handler?.Invoke(this, eventargs);
    }

    protected virtual void OnPointerExitImmediate(PointerExitEventArgs eventargs)
    {
        EventHandler<PointerExitEventArgs> handler = PointerExitImmediate;
        handler?.Invoke(this, eventargs);
    }
    
    //public PieceData GetPieceData()
    //{
    // pData = somehow get piecedata for this tile
    // return pData;
    //}
}

public class PointerEnterEventArgs : EventArgs
{
    public Vector3 Pos { get; }

    public PointerEnterEventArgs(Vector3 pos)
    {
        Pos = pos;
    }
}

public class PointerExitEventArgs : EventArgs
{
    public Vector3 Pos { get; }

    public PointerExitEventArgs(Vector3 pos)
    {
        Pos = pos;
    }
}
