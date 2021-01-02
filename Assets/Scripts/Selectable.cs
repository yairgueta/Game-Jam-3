using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider))]
public class Selectable : MonoBehaviour
{
    [SerializeField] private Material overMaterial = null;
    [SerializeField] private Material clickedDownMaterial = null;
    [SerializeField] private Material selectedMaterial = null;

    private SpriteRenderer sr;
    private Material originalMaterial;
    private void Start()
    {
        if (!overMaterial) overMaterial = SelectionManager.Instance.DefaultOverMaterial;
        if (!selectedMaterial) selectedMaterial = SelectionManager.Instance.DefaultSelectedMaterial;
        if (!clickedDownMaterial) clickedDownMaterial = SelectionManager.Instance.DefaultClickedDownMaterial;
        
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    private void OnMouseEnter()
    {
        sr.material = overMaterial;
    }

    private void OnMouseExit()
    {
        sr.material = originalMaterial;
    }

    private void OnMouseDown()
    {
        sr.material = clickedDownMaterial;
    }

    private void OnMouseUp()
    {
        sr.material = selectedMaterial;
    }
}
