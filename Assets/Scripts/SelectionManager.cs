using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private Material defaultOverMaterial;
    [SerializeField] private Material defaultClickedDownMaterial;
    [SerializeField] private Material defaultSelectedMaterial;

    public Material DefaultOverMaterial => defaultOverMaterial;
    public Material DefaultClickedDownMaterial => defaultClickedDownMaterial;
    public Material DefaultSelectedMaterial => defaultSelectedMaterial;

    public Selectable CurrentSelected => currentSelected;
    private Selectable currentSelected;

    private void Start()
    {
        currentSelected = null;
    }
}
