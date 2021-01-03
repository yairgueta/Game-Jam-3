using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("When clicking");
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BEGIN DRAGGING");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("END DRAGGING");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAGGING");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

  
}
