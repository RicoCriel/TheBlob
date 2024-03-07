using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Diagnostics.Tracing;

[SelectionBase]
public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float hoverDelay;
    [SerializeField] private float lerpDuration;
    
    private Color startColor = Color.white;
    private Color endColor = Color.red;
    private Material lerpMaterial;
    private Coroutine hoverCoroutine;


    void Start()
    {
        lerpMaterial = GetComponentInChildren<Renderer>().material;
    }

    public void ChangeHoverState(bool state)
    {
        if (state)
        {
            lerpMaterial.DOColor(endColor, lerpDuration)
               .From(lerpMaterial.color)
               .SetEase(Ease.Linear)
               .OnComplete(() => Debug.Log("Color Lerp Completed"));
        }
        else
        {
            lerpMaterial.DOColor(startColor, lerpDuration)
               .From(lerpMaterial.color)
               .SetEase(Ease.Linear)
               .OnComplete(() => Debug.Log("Color Lerp Completed"));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverCoroutine == null)
            // Start the coroutine to delay the pointer enter action
            hoverCoroutine = StartCoroutine(DelayedPointerEnter(hoverDelay));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop the coroutine if it's running
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine); hoverCoroutine = null;

        // Invoke immediate pointer exit event
        ChangeHoverState(false);
        OnPointerExitImmediate(new PointerEventArgs(transform.position,this));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnPointerClickEventRight(new PointerEventArgs(transform.position, this));
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnPointerClickEventLeft(new PointerEventArgs(transform.position, this));
        }
        
       
    }
    private IEnumerator DelayedPointerEnter(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Raise the pointer enter event after the delay
        ChangeHoverState(true);
        OnPointerEnterDelayed(new PointerEventArgs(transform.position,this));
    }

    public event EventHandler<PointerEventArgs> PointerEnterDelayed;
    public event EventHandler<PointerEventArgs> PointerExitImmediate;
    public event EventHandler<PointerEventArgs> PointerClickLeft;
    public event EventHandler<PointerEventArgs> PointerClickRight;

    protected virtual void OnPointerEnterDelayed(PointerEventArgs eventargs)
    {
        EventHandler<PointerEventArgs> handler = PointerEnterDelayed;
        handler?.Invoke(this, eventargs);
    }

    protected virtual void OnPointerExitImmediate(PointerEventArgs eventargs)
    {
        EventHandler<PointerEventArgs> handler = PointerExitImmediate;
        handler?.Invoke(this, eventargs);
    }

    protected virtual void OnPointerClickEventLeft(PointerEventArgs eventargs)
    {
        EventHandler<PointerEventArgs> handler = PointerClickLeft;
        handler?.Invoke(this, eventargs);
    }
    protected virtual void OnPointerClickEventRight(PointerEventArgs eventargs)
    {
        EventHandler<PointerEventArgs> handler = PointerClickRight;
        handler?.Invoke(this, eventargs);
    }
}

public class PointerEventArgs : EventArgs
{
    public Vector3 Pos { get; }
    public Tile Tile { get; }

    public PointerEventArgs(Vector3 pos,Tile tile)
    {
        Pos = pos;
        Tile = tile;
    }
}
