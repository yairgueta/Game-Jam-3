using System;
using Events;
using UnityEngine;

namespace Selections
{
    [CreateAssetMenu]
    public class SelectionManager : ScriptableObject
    {
        public static SelectionManager Instance { get; private set; }

        [Header("Default selection materials")]
        [SerializeField] private Material defaultOverMaterial;
        [SerializeField] private Material defaultClickedDownMaterial;
        [SerializeField] private Material defaultSelectedMaterial;

        public Material DefaultOverMaterial => defaultOverMaterial;
        public Material DefaultClickedDownMaterial => defaultClickedDownMaterial;
        public Material DefaultSelectedMaterial => defaultSelectedMaterial;


        public GameEvent onSelectionChanged;
        public Selectable CurrentSelected => currentSelected;
        private Selectable currentSelected;

        private void OnEnable()
        {
            currentSelected = null;
            Instance = this;
        }

        internal void NewSelected(Selectable selected)
        {
            Deselect();
            currentSelected = selected;
            currentSelected.Select(); 
            onSelectionChanged.Raise();
        }

        public void Deselect()
        {
            if (currentSelected != null) currentSelected.Deselect();
            currentSelected = null;
            onSelectionChanged.Raise();
        }
    }
}
